using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custlookuplnhp : Page
{
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Calendar1.SelectedDate = DateTime.Now;
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
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;

        lblID.Text = GridView1.DataKeys[row.RowIndex].Value.ToString();
        txtagent.Text = row.Cells[3].Text;
        txtstatus.Text = row.Cells[4].Text;
        txtlname.Text = row.Cells[6].Text;
        txtfname.Text = row.Cells[7].Text;
        txtadd.Text = row.Cells[8].Text;
        txtcity.Text = row.Cells[9].Text;
        txtstate.Text = row.Cells[10].Text;
        txtzip.Text = row.Cells[11].Text;
        txthphone.Text = row.Cells[12].Text;
        txtmphone.Text = row.Cells[13].Text;
        txtemail.Text = row.Cells[14].Text;
        txtbranch.Text = row.Cells[15].Text;
        txthdischarge.Text = row.Cells[16].Text;
        txtages.Text = row.Cells[17].Text;
        txtnotes.Text = row.Cells[18].Text;
        txtDate.Text = row.Cells[19].Text.Trim() == "&nbsp;" ? "" : row.Cells[19].Text;
        txtapptset.Text = row.Cells[20].Text;

        ModalPopupExtender1.Show();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string agent;
        string toagent = string.Empty;
        
        agent = txtagent.Text;
        toagent = " ";

        int id = Convert.ToInt32(lblID.Text);
        SqlDateTime sqldatenull = SqlDateTime.Null;
        DateTime? appointmentDate = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtDate.Text);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, " +
                "State=@State, ZIP=@ZIP, Status=@Status, HomePhone=@HomePhone, MobilePhone=@MobilePhone, EmailAddress=@EmailAddress, Branch=@Branch, " +
                "HDischarge=@HDischarge, Ages=@Ages, Notes=@Notes, AppointmentSet=@AppointmentSet, Agent=@Agent, ApptSetter=@ApptSetter, " +
                "LDate=@LDate, CDate=@CDate WHERE Id=@Id", con))
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
                cmd.Parameters.AddWithValue("@AppointmentSet", (object)appointmentDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
                cmd.Parameters.AddWithValue("@LDate", txtstatus.Text == "Left Message" ? (object)DateTime.Today : DBNull.Value);
                cmd.Parameters.AddWithValue("@CDate", txtstatus.Text.Contains("Call Back") ? (object)DateTime.Today : DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();

        if (appointmentDate.HasValue)
        {
            SendEmail(txtagent.Text, appointmentDate.Value);
        }
    }

    private void SendEmail(string agent, DateTime appointmentDate)
    {
        string toagent = "";
        string recipientEmail;
        if (agent == "VPP Sharon Stangler" || agent == "Asher Sharon Stangler" || agent == "Sharon Stangler")
            recipientEmail = "rsstangler1@gmail.com";
        else if (agent == "VPP Richard Stangler" || agent == "Asher Richard Stangler" || agent == "Richard Stangler")
            recipientEmail = "rjsstangler@gmail.com";
        else if (agent == "Mary Jo Hudson")
            recipientEmail = "maryjoveteransprogram@gmail.com";
        else
            recipientEmail = "cj.haarer@gmail.com";

        MailMessage msg = new MailMessage
        {
            From = new MailAddress("info@ashersolutions.com", "Asher Solutions")
        };

        if (agent == "VPP Sharon Stangler")
            toagent = "rsstangler1";
        else if (agent == "VPP Richard Stangler")
            toagent = "rjsstangler";
        else if (agent == "Asher Sharon Stangler")
            toagent = "rsstangler1";
        else if (agent == "Asher Richard Stangler")
            toagent = "rjsstangler";
        else if (agent == "Sharon Stangler")
            toagent = "rsstangler1";
        else if (agent == "Richard Stangler")
            toagent = "rjsstangler";
        else if (agent == "Mary Jo Hudson")
            toagent = "maryjoveteransprogram";
        else
            toagent = "cj.haarer";

        msg.To.Add(new MailAddress(toagent + "@gmail.com", txtagent.Text));
            //msg.CC.Add(new MailAddress("zzzzz@xyz.com", "DEF"));// it is optional, only if required
            DateTime dt;
            if (!DateTime.TryParse(txtDate.Text, out dt))
            {
                lblresult.Text = "Invalid date format.";
                lblresult.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //msg.Body = "You have an Appointment with " + txtfname.Text.ToString() + " " + txtlname.Text.ToString() + " " + txthphone.Text.ToString() + " " + txtmphone.Text.ToString() + " " + txtbranch.Text.ToString() + " " + txthdischarge.Text.ToString() + " " + txtages.Text.ToString();
            msg.Body = $"You have an Appointment with {txtfname.Text} {txtlname.Text}, Home Phone: {txthphone.Text}, Mobile Phone: {txtmphone.Text}, Branch: {txtbranch.Text}, Discharge: {txthdischarge.Text}, Age: {txtages.Text}";

            //Parse the txtdate value
            //DateTime dt = Convert.ToDateTime(txtDate.Text);

            // Now Contruct the ICS file using string builder
            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Schedule a Meeting");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", dt.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+180).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));

            str.AppendLine("LOCATION: " + txtadd.Text.ToString() + " " + txtcity.Text.ToString() + " " + txtstate.Text.ToString() + " " + txtzip.Text.ToString());
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine(string.Format("ATTENDEE;ROLE=OWNER;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT30M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            var bytes = Encoding.UTF8.GetBytes(str.ToString());
            var stream = new MemoryStream(bytes);
            var attachment = new Attachment(stream, "appointment.ics", "text/calendar");
            msg.Attachments.Add(attachment);

        System.Net.ServicePointManager.ServerCertificateValidationCallback = 
            delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true; // ❌ Accepts all certs – insecure!
        };
        //System.Net.ServicePointManager.ServerCertificateValidationCallback = null; // ✅ Accepts only valid certs             
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        using (SmtpClient smtpClient = new SmtpClient("smtp.ashersolutions.com", 587))
        {
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("info@ashersolutions.com", "Fr3343v3r&^%");
            smtpClient.EnableSsl = true;

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
