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

public partial class custlookupln : Page
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
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
        e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvrow = (GridViewRow)imageButton.NamingContainer;

        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        txtagent.Text = gvrow.Cells[3].Text;
        txtstatus.Text = gvrow.Cells[4].Text;
        txtlname.Text = gvrow.Cells[6].Text;
        txtfname.Text = gvrow.Cells[7].Text;
        txtadd.Text = gvrow.Cells[8].Text;
        txtcity.Text = gvrow.Cells[9].Text;
        txtstate.Text = gvrow.Cells[10].Text;
        txtzip.Text = gvrow.Cells[11].Text;
        txthphone.Text = gvrow.Cells[12].Text;
        txtmphone.Text = gvrow.Cells[13].Text;
        txtemail.Text = gvrow.Cells[14].Text;
        txtbranch.Text = gvrow.Cells[15].Text;
        txthdischarge.Text = gvrow.Cells[16].Text;
        txtages.Text = gvrow.Cells[17].Text;
        txtnotes.Text = gvrow.Cells[18].Text;
        txtDate.Text = gvrow.Cells[19].Text.Trim() == "&nbsp;" ? "" : gvrow.Cells[19].Text;
        txtapptset.Text = gvrow.Cells[20].Text;

        ModalPopupExtender1.Show();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvrow = (GridViewRow)imageButton.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[gvrow.RowIndex].Value);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", con))
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
            SendEmail(txtagent.Text, appointmentDate.Value);
        }

        ClearForm();
        GridView1.DataBind();
    }

    private void UpdateCustomerRecord(DateTime? appointmentDate)
    {
        int id = Convert.ToInt32(lblID.Text);

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

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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

    private void SendEmail(string agent, DateTime appointmentDate)
    {
        // map to primary recipient and optional gmail alias
        string primaryRecipient;
        string gmailAlias;

        switch (agent)
        {
            case "VPP Sharon Stangler":
            case "Asher Sharon Stangler":
            case "Sharon Stangler":
                primaryRecipient = "rsstangler1@gmail.com";
                gmailAlias = "rsstangler1";
                break;
            case "VPP Richard Stangler":
            case "Asher Richard Stangler":
            case "Richard Stangler":
                primaryRecipient = "rjsstangler@gmail.com";
                gmailAlias = "rjsstangler";
                break;
            case "Mary Jo Hudson":
                primaryRecipient = "maryjoveteransprogram@gmail.com";
                gmailAlias = "maryjoveteransprogram";
                break;
            case "Amy Wallace":
                primaryRecipient = "awallacetvp@gmail.com";
                gmailAlias = "awallacetvp";
                break;
            default:
                primaryRecipient = "cj.haarer@gmail.com";
                gmailAlias = "cj.haarer";
                break;
        }

        string subject = string.Format("Appointment with {0} {1} on {2:MMMM d, yyyy h:mm tt}", txtfname.Text, txtlname.Text, appointmentDate);
        string body = string.Format("You have an Appointment with {0} {1}. Home: {2}. Mobile: {3}. Branch: {4}. Discharge: {5}. Age: {6}", txtfname.Text, txtlname.Text, txthphone.Text, txtmphone.Text, txtbranch.Text, txthdischarge.Text, txtages.Text);

        using (var msg = new MailMessage())
        {
            msg.From = new MailAddress("info@ashersolutions.com", "Sales Lead Appointment");
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = false;
            msg.To.Add(new MailAddress(primaryRecipient, txtagent.Text));

            // add gmail alias as CC if different
            try
            {
                var agentGmail = gmailAlias + "@gmail.com";
                if (!string.Equals(agentGmail, primaryRecipient, StringComparison.OrdinalIgnoreCase))
                {
                    msg.CC.Add(new MailAddress(agentGmail, txtagent.Text));
                }
            }
            catch
            {
                // ignore malformed alias
            }

            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Schedule a Meeting");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:REQUEST");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + appointmentDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
            sb.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMdd\\THHmmss\\Z"));
            sb.AppendLine("DTEND:" + appointmentDate.AddMinutes(180).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
            sb.AppendLine("LOCATION: " + txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text);
            sb.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            sb.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            sb.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            sb.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            sb.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));
            if (msg.To.Count > 0)
            {
                sb.AppendLine(string.Format("ATTENDEE;ROLE=OWNER;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));
            }
            sb.AppendLine("BEGIN:VALARM");
            sb.AppendLine("TRIGGER:-PT30M");
            sb.AppendLine("ACTION:DISPLAY");
            sb.AppendLine("DESCRIPTION:Reminder");
            sb.AppendLine("END:VALARM");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            using (var stream = new MemoryStream(bytes))
            using (var attachment = new Attachment(stream, "appointment.ics", "text/calendar"))
            {
                msg.Attachments.Add(attachment);

                // Use SmtpClient() so web.config mailSettings are honored in different environments
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
        txtbranch.Text = string.Empty;
        txthdischarge.Text = string.Empty;
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