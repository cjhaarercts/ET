using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentLM : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Calendar1.DateMin = DateTime.Now;
            GridView1.DataBind();
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        lblID.Text = GridView1.DataKeys[gvRow.RowIndex].Value.ToString();

        // Safely apply agent value even if it is not present in the dropdown list
        string agentValue = gvRow.Cells[3].Text == null ? string.Empty : gvRow.Cells[3].Text.Trim();
        try
        {
            if (txtagent != null)
            {
                ListItem foundAgent = null;
                if (txtagent.Items != null)
                {
                    foundAgent = txtagent.Items.FindByText(agentValue) ?? txtagent.Items.FindByValue(agentValue);
                }

                if (foundAgent != null)
                {
                    txtagent.ClearSelection();
                    foundAgent.Selected = true;
                }
                else if (txtagent.Items != null)
                {
                    // insert a temporary list item so SelectedValue is valid and UI shows the value
                    txtagent.Items.Insert(0, new ListItem(agentValue, agentValue));
                    txtagent.ClearSelection();
                    txtagent.Items[0].Selected = true;
                }
                else
                {
                    txtagent.Text = agentValue;
                }
            }
        }
        catch
        {
            // final fallback: set Text and continue — avoid throwing to user
            if (txtagent != null)
            {
                txtagent.Text = agentValue;
            }
        }

        txtstatus.Text = gvRow.Cells[4].Text.Trim();
        txtlname.Text = gvRow.Cells[6].Text.Trim();
        txtfname.Text = gvRow.Cells[7].Text.Trim();
        txtadd.Text = gvRow.Cells[8].Text.Trim();
        txtcity.Text = gvRow.Cells[9].Text.Trim();
        txtstate.Text = gvRow.Cells[10].Text.Trim();
        txtzip.Text = gvRow.Cells[11].Text.Trim();
        txthphone.Text = gvRow.Cells[12].Text.Trim();
        txtmphone.Text = gvRow.Cells[13].Text.Trim();
        txtemail.Text = gvRow.Cells[14].Text.Trim();
        txtbranch.Text = gvRow.Cells[15].Text.Trim();
        txthdischarge.Text = gvRow.Cells[16].Text.Trim();
        txtages.Text = gvRow.Cells[17].Text.Trim();
        txtnotes.Text = gvRow.Cells[18].Text.Trim();
        txtDate.Text = gvRow.Cells[19].Text.Trim() == "&nbsp;" ? "" : gvRow.Cells[19].Text.Trim();
        txtapptset.Text = gvRow.Cells[20].Text.Trim();

        ModalPopupExtender1.Show();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[gvRow.RowIndex].Value);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id=@Id", con))
        {
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DateTime? appointmentDate = ParseDate(txtDate.Text);
        UpdateCustomerRecord(appointmentDate);
        if (appointmentDate.HasValue)
        {
            SendAppointmentEmail(appointmentDate.Value);
        }

        ClearForm();
        GridView1.DataBind();
    }

    private void UpdateCustomerRecord(DateTime? appointmentDate)
    {
        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(@"UPDATE Customers SET 
                                            FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, 
                                            State=@State, ZIP=@ZIP, HomePhone=@HomePhone, MobilePhone=@MobilePhone, 
                                            EmailAddress=@EmailAddress, Branch=@Branch, HDischarge=@HDischarge, 
                                            Ages=@Ages, Notes=@Notes, AppointmentSet=@AppointmentSet, Agent=@Agent, 
                                            ApptSetter=@ApptSetter, Status=@Status 
                                         WHERE Id=@Id", con))
        {
            cmd.Parameters.AddWithValue("@FirstName", txtfname.Text);
            cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
            cmd.Parameters.AddWithValue("@Address", txtadd.Text);
            cmd.Parameters.AddWithValue("@City", txtcity.Text);
            cmd.Parameters.AddWithValue("@State", txtstate.Text);
            cmd.Parameters.AddWithValue("@ZIP", txtzip.Text);
            cmd.Parameters.AddWithValue("@HomePhone", txthphone.Text);
            cmd.Parameters.AddWithValue("@MobilePhone", txtmphone.Text);
            cmd.Parameters.AddWithValue("@EmailAddress", txtemail.Text);
            cmd.Parameters.AddWithValue("@Branch", txtbranch.Text);
            cmd.Parameters.AddWithValue("@HDischarge", txthdischarge.Text);
            cmd.Parameters.AddWithValue("@Ages", txtages.Text);
            cmd.Parameters.AddWithValue("@Notes", txtnotes.Text);
            cmd.Parameters.AddWithValue("@Agent", txtagent.Text);
            cmd.Parameters.AddWithValue("@ApptSetter", txtapptset.Text);
            cmd.Parameters.AddWithValue("@Status", txtstatus.Text);

            // typed parameter for appointment date to avoid type inference issues
            cmd.Parameters.Add("@AppointmentSet", SqlDbType.DateTime).Value = appointmentDate.HasValue ? (object)appointmentDate.Value : DBNull.Value;

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(lblID.Text);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }

    private DateTime? ParseDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dt))
            return dt;

        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            return dt;

        return null;
    }

    private void SendAppointmentEmail(DateTime appointmentDate)
    {
        string subject = "Appointment with {txtfname.Text} {txtlname.Text} on {appointmentDate:MMMM d, yyyy h:mm tt}";
        string body = "You have an Appointment with {txtfname.Text} {txtlname.Text}. Home: {txthphone.Text}. Mobile: {txtmphone.Text}. Branch: {txtbranch.Text}. Discharge: {txthdischarge.Text}. Age: {txtages.Text}";

        string agentAlias = GetAgentEmailAlias(txtagent.Text);
        string recipient = agentAlias.Contains("@") ? agentAlias : "{agentAlias}@gmail.com";

        using (var msg = new MailMessage())
        {
            msg.From = new MailAddress("info@ashersolutions.com", "Sales Lead Appointment");
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = false;
            msg.To.Add(new MailAddress(recipient, txtagent.Text));

            // build ICS content using UTC
            var str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Schedule a Meeting");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine("DTSTART:" + appointmentDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
            str.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMdd\\THHmmss\\Z"));
            str.AppendLine("DTEND:" + appointmentDate.AddMinutes(180).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
            str.AppendLine("LOCATION: " + txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text);
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));
            if (msg.To.Count > 0)
            {
                str.AppendLine(string.Format("ATTENDEE;ROLE=OWNER;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));
            }
            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT30M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            var bytes = Encoding.UTF8.GetBytes(str.ToString());
            using (var stream = new MemoryStream(bytes))
            using (var attachment = new Attachment(stream, "appointment.ics", "text/calendar"))
            {
                msg.Attachments.Add(attachment);

                using (var smtpClient = new SmtpClient())
                {
                    try
                    {
                        smtpClient.Send(msg);
                        lblresult.Text = "Email sent successfully.";
                        lblresult.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblresult.Text = "Email error: " + ex.Message;
                        lblresult.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
    }

    private string GetAgentEmailAlias(string agent)
    {
        switch (agent)
        {
            case "GC Sharon Stangler":
            case "Asher Sharon Stangler":
            case "Sharon Stangler":
                return "rsstangler1";
            case "GC Richard Stangler":
            case "Asher Richard Stangler":
            case "Richard Stangler":
                return "rjsstangler";
            case "Mary Jo Hudson":
                return "maryjoveteransprogram";
            case "Amy Wallace":
                return "awallacetvp";
            case "Serenity":
                return "donna.haarer";
            default:
                return "cj.haarer";
        }
    }

    private void ClearForm()
    {
        foreach (IValidator ctrl in Validators)
        {
            ctrl.IsValid = true;
        }

        txtfname.Text = string.Empty;
        txtlname.Text = string.Empty;
        txtadd.Text = string.Empty;
        txtcity.Text = string.Empty;
        txtstate.Text = string.Empty;
        txtzip.Text = string.Empty;
        txtmphone.Text = string.Empty;
        txtemail.Text = string.Empty;
        txtages.Text = string.Empty;
        txtnotes.Text = string.Empty;
        txtDate.Text = string.Empty;
        Calendar1.SelectedDate = DateTime.MinValue;
        txtagent.Text = string.Empty;
        txtapptset.Text = string.Empty;
        txtstatus.Text = string.Empty;

        ModalPopupExtender1.Hide();
    }
}