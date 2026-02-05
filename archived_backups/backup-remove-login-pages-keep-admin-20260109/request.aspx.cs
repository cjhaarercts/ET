using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;

public partial class request : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand("Insert Into Customers (FirstName,LastName,EmailAddress,HomePhone,Address,City,State,ZIP,Ages,Branch,HDischarge,Created,Web,Agent,Status) VALUES (@FirstName,@LastName,@EmailAddress,@HomePhone,@Address,@City,@State,@ZIP,@Ages,@Branch,@HDischarge,@Created,@Web,@Agent,@Status)", conn);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@FirstName", FirstName.Text);
        cmd.Parameters.AddWithValue("@LastName", LastName.Text);
        cmd.Parameters.AddWithValue("@EmailAddress", EmailAddress.Text);
        cmd.Parameters.AddWithValue("@HomePhone", HomePhone.Text);
        cmd.Parameters.AddWithValue("@Address", Address.Text);
        cmd.Parameters.AddWithValue("@City", City.Text);
        cmd.Parameters.AddWithValue("@State", State.Text);
        cmd.Parameters.AddWithValue("@ZIP", ZIP.Text);
        cmd.Parameters.AddWithValue("@Ages", Ages.Text);
        cmd.Parameters.AddWithValue("@Branch", Branch.Text);
        cmd.Parameters.AddWithValue("@HDischarge", HDischarge.Text);
        cmd.Parameters.AddWithValue("@Created", DateTime.Today);
        cmd.Parameters.AddWithValue("@Web", "1");
        cmd.Parameters.AddWithValue("@Agent", "Web Lead");
        cmd.Parameters.AddWithValue("@Status", "New");
        cmd.ExecuteNonQuery();

        using (MailMessage message = new MailMessage())
        {
            message.From = new MailAddress("info@eternalsolutions.us");
            message.To.Add(new MailAddress("rsstangler1@gmail.com"));
            message.CC.Add(new MailAddress("cj.haarer@gmail.com"));
            message.Subject = "New Lead from Eternal Solutions";
            message.Body = "First Name: " + FirstName.Text.ToString() + System.Environment.NewLine + "Last Name: " + LastName.Text.ToString() + System.Environment.NewLine + "Email Address: " + EmailAddress.Text.ToString() + System.Environment.NewLine + "Home Phone: " + HomePhone.Text.ToString() + System.Environment.NewLine + "Address: " + Address.Text.ToString() + System.Environment.NewLine + "City: " + City.Text.ToString() + System.Environment.NewLine + "State: " + State.Text.ToString() + System.Environment.NewLine + "Zip: " + ZIP.Text.ToString() + System.Environment.NewLine + "Ages: " + Ages.Text.ToString() + System.Environment.NewLine + "Service Branch: " + Branch.Text.ToString() + System.Environment.NewLine + "Honorably Discharged: " + HDischarge.Text.ToString();
            SmtpClient client = new SmtpClient("localhost"); 
            //SmtpClient client = new SmtpClient("smtp.eternalsolutions.us", 2525);
            // Credentials are necessary if the server requires the client  
            // to authenticate before it will send e-mail on the client's behalf.
            //NetworkCredential myCreds = new NetworkCredential("info@eternalsolutions.us", "L3tm31n!", "");
            //client.Credentials = myCreds;
            client.Send(message);
            Response.Redirect("thankyou.aspx");
        }
        
    }
}