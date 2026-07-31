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

public partial class _cluAgentDead : Page
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

        txtagent.Text = gvRow.Cells[3].Text;
        txtstatus.Text = gvRow.Cells[4].Text;
        txtlname.Text = gvRow.Cells[6].Text;
        txtfname.Text = gvRow.Cells[7].Text;
        txtadd.Text = gvRow.Cells[8].Text;
        txtcity.Text = gvRow.Cells[9].Text;
        txtstate.Text = gvRow.Cells[10].Text;
        txtzip.Text = gvRow.Cells[11].Text;
        txthphone.Text = gvRow.Cells[12].Text;
        txtmphone.Text = gvRow.Cells[13].Text;
        txtemail.Text = gvRow.Cells[14].Text;
        txtbranch.Text = gvRow.Cells[15].Text;
        txthdischarge.Text = gvRow.Cells[16].Text;
        txtages.Text = gvRow.Cells[17].Text;
        txtnotes.Text = gvRow.Cells[18].Text;
        txtDate.Text = gvRow.Cells[19].Text.Trim() == "&nbsp;" ? "" : gvRow.Cells[19].Text;
        txtapptset.Text = gvRow.Cells[20].Text;

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
                                            ApptSetter=@ApptSetter, Status=@Status, LDate=@LDate, CDate=@CDate
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

            // typed parameters for AppointmentSet, LDate, CDate and Id
            cmd.Parameters.Add("@AppointmentSet", SqlDbType.DateTime).Value = appointmentDate.HasValue ? (object)appointmentDate.Value : DBNull.Value;
            cmd.Parameters.Add("@LDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@CDate", SqlDbType.DateTime).Value = DBNull.Value;

            // Set LDate/CDate based on status; treat "Texted Lead" like "Left Message"
            if (txtstatus.Text.Equals("Left Message", StringComparison.OrdinalIgnoreCase) ||
                txtstatus.Text.Equals("Texted Lead", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@LDate"].Value = DateTime.Today;
            }
            else if (txtstatus.Text.StartsWith("Call Back", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@CDate"].Value = DateTime.Today;
            }

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

        DateTime dt;
        if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
            return dt;

        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            return dt;

        return null;
    }

    private void SendAppointmentEmail(DateTime appointmentDate)
    {
        string subject = string.Format("Appointment with {0} {1} on {2:MMMM d, yyyy h:mm tt}", txtfname.Text, txtlname.Text, appointmentDate);
        string body = string.Format("You have an Appointment with {0} {1}. Home: {2}. Mobile: {3}. Branch: {4}. Discharge: {5}. Age: {6}",
            txtfname.Text, txtlname.Text, txthphone.Text, txtmphone.Text, txtbranch.Text, txthdischarge.Text, txtages.Text);

        string agentAlias = GetAgentEmailAlias(txtagent.Text);
        string recipient = agentAlias.Contains("@") ? agentAlias : agentAlias + "@gmail.com";

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
            str.AppendLine(string.Format("DESCRIPTION:{0}", body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", body));
            str.AppendLine(string.Format("SUMMARY:{0}", subject));
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

        // clear dropdown selections safely
        if (txtagent != null && txtagent.Items != null)
        {
            txtagent.ClearSelection();
        }
        if (txtapptset != null && txtapptset.Items != null)
        {
            txtapptset.ClearSelection();
        }
        if (txtstatus != null && txtstatus.Items != null)
        {
            txtstatus.ClearSelection();
        }

        ModalPopupExtender1.Hide();
    }
}