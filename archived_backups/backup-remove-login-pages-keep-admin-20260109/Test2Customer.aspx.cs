using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.Net;
using System.Net.Mail;
using System.Text;

public partial class tvp_Customer : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Calendar1.DateMin = DateTime.Now;
            // Set "initial" query parameters, then ...
            GridView1.DataBind();
        }
    }
    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        decimal RowValue;
        RowValue = Decimal.Remainder(e.Row.RowIndex, 2);

        if (e.Row.RowIndex != -1)
        e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");

        if ((RowValue == 0) && (e.Row.RowIndex != -1))
        {
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '#FFFFFF';");
        }
        else if ((RowValue != 0) && (e.Row.RowIndex != -1))
        {
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '#EFF3FB';");
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton ImageButton1 = sender as ImageButton;
        string userName = String.Empty; // Authentication removed
 
        // Authentication removed — default to visible
        ImageButton1.Visible = true;

        GridViewRow gvrow = (GridViewRow)ImageButton1.NamingContainer;
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
        txtbranch.Text = gvrow.Cells[13].Text;
        txthdischarge.Text = gvrow.Cells[14].Text;
        txtages.Text = gvrow.Cells[15].Text;
        txtnotes.Text = gvrow.Cells[16].Text;
        txtDate.Text = gvrow.Cells[18].Text;
        if (txtDate.Text.Equals(" "))
            txtDate.Text = "";
        this.ModalPopupExtender1.Show();

        GridView1.DataBind();
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {

        ImageButton ImageButton2 = sender as ImageButton;
        // Authentication removed — default to visible
        ImageButton2.Visible = true; // Default: visible to all since auth removed

        GridViewRow gvrow = (GridViewRow)ImageButton2.NamingContainer;
        string ID = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();
        Label id = (Label)gvrow.FindControl("lblID");
        SqlCommand cmd = new SqlCommand("delete from Customers where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(ID));

        con.Open();

        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.DataBind();
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

        con.Open();
        SqlCommand cmd = new SqlCommand("update Customers set FirstName=@FirstName,LastName=@LastName,Address=@Address,City=@City,State=@State,ZIP=@ZIP,HomePhone=@HomePhone,MobilePhone=@MobilePhone,EmailAddress=@EmailAddress,Branch=@Branch,HDischarge=@HDischarge,Ages=@Ages,Notes=@Notes,AppointmentSet=@AppointmentSet,Agent=@Agent,ApptSetter=@ApptSetter,Status=@Status,LDate=@LDate,CDate=@CDate where Id=@Id", con);
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
        //cmd.Parameters.AddWithValue("@LDate", LDate.Text);
        //cmd.Parameters.AddWithValue("@CDate", CDate.Text);
        if (txtDate.Text == "")
            cmd.Parameters.AddWithValue("@AppointmentSet", sqldatenull);
        else
            cmd.Parameters.AddWithValue("@AppointmentSet", txtDate.Text);
        cmd.Parameters.AddWithValue("@Agent", txtagent.Text);
        cmd.Parameters.AddWithValue("@ApptSetter", txtapptset.Text);
        if (txtstatus.Text == "Left Message")
            {
            //cmd.Parameters["@LDate"].Value = DateTime.Today;
                cmd.Parameters.AddWithValue("@LDate", DateTime.Today);
                cmd.Parameters.AddWithValue("@CDate", sqldatenull);
            }
        else
            if (txtstatus.Text == "Call Back")
            {
            //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                cmd.Parameters.AddWithValue("@CDate", DateTime.Today.AddMonths(1));
                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
            }
            else
                if (txtstatus.Text == "New")
                {
                    //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                    cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                }
                else
                    if (txtstatus.Text == "Active")
                    {
                        //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                        cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                        cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                    }
                    else
                        if (txtstatus.Text == "Mail")
                        {
                            //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                            cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                        }
                        else
                            if (txtstatus.Text == "Sold")
                            {
                                //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                                cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                            }
                            else
                                if (txtstatus.Text == "Dead")
                                {
                                    //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                                    cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                }
        cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));
        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.DataBind();
        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = Color.Green;

        if ( txtDate.Text == "")
        {
            goto nomail;
        }
        GridView1.DataBind();
        if (txtDate.Text != "")
        {
            if (agent == "Sharon Stangler")
                toagent = "rsstangler1";
            else if (agent == "Richard Stangler")
                toagent = "rjsstangler";
            else if (agent == "Aneka Forte")
                toagent = "aneka.veteransprogram";
            else if (agent == "Darel Taylor")
                toagent = "darelgtaylor";
            else if (agent == "Alan Poindexter")
                toagent = "alan.veteransprogram";
            else if (agent == "Mary Jo Hudson")
                toagent = "maryjoveteransprogram";
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
            //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+330)));
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+300)));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
            //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+660)));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+480)));
            str.AppendLine("LOCATION: " + txtadd.Text.ToString() + " " + txtcity.Text.ToString() + " " + txtstate.Text.ToString() + " " + txtzip.Text.ToString());
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine(string.Format("ATTENDEE;ROLE=OWNER;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            //Now sending a mail with attachment ICS file.                     
            // System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient();

            SmtpClient smtpclient = new SmtpClient("localhost");
            //SmtpClient smtpclient = new SmtpClient("smtp.eternalsolutionsllc.com", 2525);
            // Credentials are necessary if the server requires the client  
            // to authenticate before it will send e-mail on the client's behalf.
            //NetworkCredential myCreds = new NetworkCredential("info@eternalsolutionsllc.com", "L3tm31n!", "");
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
        GridView1.DataBind();
    }
}