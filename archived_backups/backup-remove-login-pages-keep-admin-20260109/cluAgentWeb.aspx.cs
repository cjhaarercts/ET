using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentWeb : Page
{
    private readonly SqlConnection _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ToString());

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
            e.Row.Attributes.Add("OnMouseOut", e.Row.RowIndex % 2 == 0 ? "this.style.backgroundColor = '#FFFFFF';" : "this.style.backgroundColor = '#EFF3FB';");
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        lblID.Text = GridView1.DataKeys[gvRow.RowIndex].Value.ToString();

        txtagent.Text = gvRow.Cells[3].Text.Trim();
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
        txtDate.Text = gvRow.Cells[19].Text.Trim();
        txtapptset.Text = gvRow.Cells[20].Text.Trim();

        if (txtDate.Text.Equals(" "))
            txtDate.Text = "";

        ModalPopupExtender1.Show();
        GridView1.DataBind();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        var id = GridView1.DataKeys[gvRow.RowIndex].Value.ToString();

        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id=@Id", _connection))
        {
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateCustomerRecord();
        //SendEmail();
        ClearForm();
        GridView1.DataBind();
    }

    private void UpdateCustomerRecord()
    {
        using (var cmd = new SqlCommand("UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, State=@State, ZIP=@ZIP, HomePhone=@HomePhone, MobilePhone=@MobilePhone, EmailAddress=@EmailAddress, Branch=@Branch, HDischarge=@HDischarge, Ages=@Ages, Notes=@Notes, AppointmentSet=@AppointmentSet, Agent=@Agent, ApptSetter=@ApptSetter, Status=@Status, LDate=@LDate, CDate=@CDate WHERE Id=@Id", _connection))
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
            cmd.Parameters.AddWithValue("@AppointmentSet", string.IsNullOrWhiteSpace(txtDate.Text) ? (object)DBNull.Value : txtDate.Text);
            cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));

            SetDateParameters(cmd);

            _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }

    private void SetDateParameters(SqlCommand cmd)
    {
        var sqlDateNull = SqlDateTime.Null;

        if (txtstatus.Text == "None" || txtstatus.Text == "New" || txtstatus.Text == "Active" || txtstatus.Text == "Sent Mail" || txtstatus.Text == "Mail - Info" || txtstatus.Text == "Mail - FUR" || txtstatus.Text == "Needs Phone Number" || txtstatus.Text == "Last Resort" || txtstatus.Text == "Sold" || txtstatus.Text == "Dead")
        {
            cmd.Parameters.AddWithValue("@CDate", sqlDateNull);
            cmd.Parameters.AddWithValue("@LDate", sqlDateNull);
        }
        else if (txtstatus.Text == "Left Message")
        {
            cmd.Parameters.AddWithValue("@LDate", DateTime.Today);
            cmd.Parameters.AddWithValue("@CDate", sqlDateNull);
        }
        else if (txtstatus.Text.StartsWith("Call Back"))
        {
            cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
            cmd.Parameters.AddWithValue("@LDate", sqlDateNull);
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
        msg.Body = "You have an Appointment with " + txtfname.Text.ToString() + " " + txtlname.Text.ToString() + " " + txthphone.Text.ToString() + " " + txtmphone.Text.ToString() + " " + txtbranch.Text.ToString() + " " + txthdischarge.Text.ToString() + " " + txtages.Text.ToString();

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
    
    private string GetAgentEmail(string agent)
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
        if (txtbranch.Items.FindByValue("") != null)
        {
            txtbranch.SelectedValue = "";
        }
        else
        {
            txtbranch.ClearSelection(); // Or: txtbranch.SelectedIndex = -1;
        }
        if (txthdischarge.Items.FindByValue("") != null)
        {
            txthdischarge.SelectedValue = "";
        }
        else
        {
            txthdischarge.ClearSelection(); // Or: txthdischarge.SelectedIndex = -1;
        }
        txtages.Text = string.Empty;
        txtnotes.Text = string.Empty;
        txtDate.Text = string.Empty;
        Calendar1.SelectedDate = DateTime.MinValue;
        if (txtagent.Items.FindByValue("") != null)
        {
            txtagent.SelectedValue = "";
        }
        else
        {
            txtagent.ClearSelection(); // Or: txtagent.SelectedIndex = -1;
        }
        if (txtapptset.Items.FindByValue("") != null)
        {
            txtapptset.SelectedValue = "";
        }
        else
        {
            txtapptset.ClearSelection(); // Or: txtapptset.SelectedIndex = -1;
        }
        if (txtstatus.Items.FindByValue("") != null)
        {
            txtstatus.SelectedValue = "";
        }
        else
        {
            txtstatus.ClearSelection(); // Or: txtstatus.SelectedIndex = -1;
        }

        ModalPopupExtender1.Hide();
    }
}