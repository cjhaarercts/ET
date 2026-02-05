using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentCB : Page
{
    private readonly SqlConnection _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Calendar1.DateMin = DateTime.Now;
            // Set "initial" query parameters, then ...
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
        txtDate.Text = gvRow.Cells[19].Text;
        txtapptset.Text = gvRow.Cells[20].Text; 
        if (txtDate.Text.Equals(" "))
            txtDate.Text = "";
        this.ModalPopupExtender1.Show();

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
        MailMessage msg = new MailMessage();
        //Now we have to set the value to Mail message properties

        //Note Please change it to correct mail-id to use this in your application
        msg.From = new MailAddress("info@ashersolutions.com", "Sales Lead Appointment");
        string agent;
        string toagent;

        agent = txtagent.Text;
        toagent = " ";

        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;

        UpdateCustomerRecord();
        GridView1.DataBind();
        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;

        if (txtDate.Text == "")
        {
            goto nomail;
        }
        GridView1.DataBind();
        if (txtDate.Text != "")
        {
            if (agent == "GC Sharon Stangler")
                toagent = "rsstangler1";
            else if (agent == "GC Richard Stangler")
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
            else if (agent == "Amy Wallace")
                toagent = "awallacetvp";
            else if (agent == "Serenity")
                toagent = "donna.haarer";
            else
                toagent = "cj.haarer";

            msg.To.Add(new MailAddress(toagent + "@gmail.com", "txtagent.Text.ToString()"));
            //msg.CC.Add(new MailAddress("zzzzz@xyz.com", "DEF"));// it is optional, only if required
            msg.Subject = txtfname.Text.ToString() + " " + txtlname.Text.ToString() + " " + txtadd.Text.ToString() + " " + txtcity.Text.ToString() + " " + txtstate.Text.ToString() + " " + txtzip.Text.ToString() + " " + txthphone.Text.ToString() + " " + txtmphone.Text.ToString() + " " + txtbranch.Text.ToString() + " " + txtages.Text.ToString();
            msg.Body = "You have an Appointment with " + txtfname.Text.ToString() + " " + txtlname.Text.ToString() + " " + txthphone.Text.ToString() + " " + txtmphone.Text.ToString() + " " + txtbranch.Text.ToString() + " " + txthdischarge.Text.ToString() + " " + txtages.Text.ToString();

            //Parse the txtdate value
            DateTime dt = Convert.ToDateTime(txtDate.Text);

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

            //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+330)));
            // cjh str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+300)));
            // cjh str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
            // cjh str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+660)));
            // cjh str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+480)));
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

            //Now sending a mail with attachment ICS file.                     
            // System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient();

            //SmtpClient smtpclient = new SmtpClient("localhost");
            SmtpClient smtpclient = new SmtpClient("smtp.ashersolutionsinc.com");
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Credentials = new System.Net.NetworkCredential("info@ashersolutionsinc.com", "L3tm31npl3@s3!#", "");
            smtpclient.Port = 587;
            smtpclient.EnableSsl = false;

            // Credentials are necessary if the server requires the client  
            // to authenticate before it will send e-mail on the client's behalf.
            //NetworkCredential myCreds = new NetworkCredential("info@ashersolutions.com", "L3tm31npl3@s3", "");
            //smtpclient.EnableSsl = false;
            //smtpclient.Credentials = myCreds;

            //smtpclient.Host = "smtp.eternalsolutionsllc.com"; //-------this has to given the Mailserver IP

            //smtpclient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;				

            System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
            contype.Parameters.Add("method", "REQUEST");
            contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);
            msg.AlternateViews.Add(avCal);
            smtpclient.Send(msg);
        }
    nomail:
        {
            // Clear the validators 
            foreach (IValidator ctrl in Validators)
            {
                ctrl.IsValid = true;
            }

            // Clear the control contents 
            txtfname.Text = String.Empty;
            txtlname.Text = String.Empty;
            txtadd.Text = String.Empty;
            txtcity.Text = String.Empty;
            txtstate.Text = String.Empty;
            txtzip.Text = String.Empty;
            txtmphone.Text = String.Empty;
            txtemail.Text = String.Empty;
            //txtbranch.Text = String.Empty;
            //txthdischarge.Text = String.Empty;
            txtages.Text = String.Empty;
            txtnotes.Text = String.Empty;
            txtDate.Text = String.Empty;
            Calendar1.SelectedDate = new DateTime(0);
            //txtagent.Text = String.Empty;
            //txtapptset.Text = String.Empty;
            //txtstatus.Text = String.Empty; 

            //Close the popup 
            ModalPopupExtender1.Hide();
        }
        GridView1.DataBind();

    }

    private void UpdateCustomerRecord()
    {
        using (var cmd = new SqlCommand("UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, State=@State, ZIP=@ZIP, HomePhone=@HomePhone, MobilePhone=@MobilePhone, EmailAddress=@EmailAddress, Branch=@Branch, HDischarge=@HDischarge, Ages=@Ages, Notes=@Notes, AppointmentSet=@AppointmentSet, Agent=@Agent, ApptSetter=@ApptSetter, Status=@Status WHERE Id=@Id", _connection))
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
            cmd.Parameters.AddWithValue("@AppointmentSet", string.IsNullOrWhiteSpace(txtDate.Text) ? (object)DBNull.Value : txtDate.Text);
            cmd.Parameters.AddWithValue("@Agent", txtagent.Text);
            cmd.Parameters.AddWithValue("@ApptSetter", txtapptset.Text);
            cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));

            _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
        }
    }
}