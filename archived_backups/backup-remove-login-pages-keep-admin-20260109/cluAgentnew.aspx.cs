using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentnew : Page
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
        txthdischarge.Text = gvRow.Cells[16].Text;
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
        SendAppointmentEmail();
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

    private void SendAppointmentEmail()
    {
        if (string.IsNullOrWhiteSpace(txtDate.Text))
            return;

        var msg = new MailMessage
        {
            From = new MailAddress("info@ashersolutions.com", "Sales Lead Appointment"),
            Subject = $"{txtfname.Text} {txtlname.Text} {txtadd.Text} {txtcity.Text} {txtstate.Text} {txtzip.Text} {txthphone.Text} {txtmphone.Text} {txtbranch.Text} {txtages.Text}",
            Body = $"You have an Appointment with {txtfname.Text} {txtlname.Text} {txthphone.Text} {txtmphone.Text} {txtbranch.Text} {txthdischarge.Text} {txtages.Text}"
        };

        var toAgent = GetAgentEmail(txtagent.Text);
        msg.To.Add(new MailAddress($"{toAgent}@gmail.com", txtagent.Text));

        var dt = Convert.ToDateTime(txtDate.Text);
        var str = new StringBuilder();
        str.AppendLine("BEGIN:VCALENDAR");
        str.AppendLine("PRODID:-//Schedule a Meeting");
        str.AppendLine("VERSION:2.0");
        str.AppendLine("METHOD:REQUEST");
        str.AppendLine("BEGIN:VEVENT");
        str.AppendLine($"DTSTART:{dt.ToUniversalTime():yyyyMMddTHHmmssZ}");
        str.AppendLine($"DTSTAMP:{DateTime.Now:yyyyMMddTHHmmssZ}");
        str.AppendLine($"DTEND:{dt.AddMinutes(180).ToUniversalTime():yyyyMMddTHHmmssZ}");
        str.AppendLine($"LOCATION: {txtadd.Text} {txtcity.Text} {txtstate.Text} {txtzip.Text}");
        str.AppendLine($"UID:{Guid.NewGuid()}");
        str.AppendLine($"DESCRIPTION:{msg.Body}");
        str.AppendLine($"X-ALT-DESC;FMTTYPE=text/html:{msg.Body}");
        str.AppendLine($"SUMMARY:{msg.Subject}");
        str.AppendLine($"ORGANIZER:MAILTO:{msg.From.Address}");
        str.AppendLine($"ATTENDEE;ROLE=OWNER;CN=\"{msg.To[0].DisplayName}\";RSVP=TRUE:mailto:{msg.To[0].Address}");
        str.AppendLine("BEGIN:VALARM");
        str.AppendLine("TRIGGER:-PT30M");
        str.AppendLine("ACTION:DISPLAY");
        str.AppendLine("DESCRIPTION:Reminder");
        str.AppendLine("END:VALARM");
        str.AppendLine("END:VEVENT");
        str.AppendLine("END:VCALENDAR");

        var smtpClient = new SmtpClient("smtp.ashersolutionsinc.com")
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("info@ashersolutionsinc.com", "L3tm31npl3@s3!#$"),
            Port = 587,
            EnableSsl = true
        };

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        var contentType = new System.Net.Mime.ContentType("text/calendar")
        {
            Parameters = { { "method", "REQUEST" }, { "name", "Meeting.ics" } }
        };

        var alternateView = AlternateView.CreateAlternateViewFromString(str.ToString(), contentType);
        msg.AlternateViews.Add(alternateView);
        smtpClient.Send(msg);
    }

    private string GetAgentEmail(string agent)
    {
        return agent switch
        {
            "GC Sharon Stangler" => "rsstangler1",
            "GC Richard Stangler" => "rjsstangler",
            "Asher Sharon Stangler" => "rsstangler1",
            "Asher Richard Stangler" => "rjsstangler",
            "Sharon Stangler" => "rsstangler1",
            "Richard Stangler" => "rjsstangler",
            "Mary Jo Hudson" => "maryjoveteransprogram",
            "Amy Wallace" => "awallacetvp",
            "Serenity" => "donna.haarer",
            _ => "cj.haarer"
        };
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