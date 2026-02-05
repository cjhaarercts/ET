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
            //Calendar1.DateMin = DateTime.Now;
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

        txtName.Text = gvrow.Cells[3].Text; 
        txtadd.Text = gvrow.Cells[4].Text;
        txtcity.Text = gvrow.Cells[5].Text;
        txtstate.Text = gvrow.Cells[6].Text;
        txtzip.Text = gvrow.Cells[7].Text;
        txtphone.Text = gvrow.Cells[8].Text;
        txtChapSvc.Text = gvrow.Cells[9].Text;
        txtTradSvc.Text = gvrow.Cells[10].Text;
        txtSameDaySvc.Text = gvrow.Cells[11].Text;
        txtGraveSvc.Text = gvrow.Cells[12].Text;
        txtTradCrem.Text = gvrow.Cells[13].Text;
        txtSameDayCrem.Text = gvrow.Cells[14].Text;
        txtMemCrem.Text = gvrow.Cells[15].Text;
        txtGraveCrem.Text = gvrow.Cells[16].Text;
        txtDirectCrem.Text = gvrow.Cells[17].Text;
        txtLimo.Text = gvrow.Cells[18].Text;
        txtMEscort.Text = gvrow.Cells[19].Text;
        txtAltContainer.Text = gvrow.Cells[20].Text;
        txtClergy.Text = gvrow.Cells[21].Text;
        txtMemPkg.Text = gvrow.Cells[22].Text;
        txtVideo.Text = gvrow.Cells[23].Text;
        txtShroud.Text = gvrow.Cells[24].Text;
        txtShroudMuslin.Text = gvrow.Cells[25].Text;
        txtTahara.Text = gvrow.Cells[26].Text;
        txtShomer.Text = gvrow.Cells[27].Text;
        txtGratuities.Text = gvrow.Cells[28].Text;
        this.ModalPopupExtender1.Show();
        GridView1.DataBind();
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton ImageButton2 = sender as ImageButton;

        // Authentication removed — default to visible
        ImageButton2.Visible = true;

        GridViewRow gvrow = (GridViewRow)ImageButton2.NamingContainer;
        string ID = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();
        Label id = (Label)gvrow.FindControl("lblID");
        SqlCommand cmd = new SqlCommand("delete from flpreffunhm where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(ID));

        con.Open();

        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.DataBind();
        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = Color.Green;
        GridView1.DataBind();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;

        con.Open();
        SqlCommand cmd = new SqlCommand("update flpreffunhm set Name=@Name,Address=@Address,City=@City,State=@State,Zip=@Zip,Phone=@Phone,ChapSvc=@ChapSvc,TradSvc=@TradSvc,SameDaySvc=@SameDaySvc,GraveSvc=@GraveSvc,TradCrem=@TradCrem,SameDayCrem=@SameDayCrem,MemCrem=@MemCrem,GraveCrem=@GraveCrem,DirectCrem=@DirectCrem,Limo=@Limo,MEscort=@MEscort,AltContainer=@AltContainer,Clergy=@Clergy,MemPkg=@MemPkg,Video=@Video,Shroud=@Shroud,ShroudMuslin=@ShroudMuslin,Tahara=@Tahara,Shomer=@Shomer,Gratuities=@Gratuities where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Name", txtName.Text);
        cmd.Parameters.AddWithValue("@Address", txtadd.Text);
        cmd.Parameters.AddWithValue("@City", txtcity.Text);
        cmd.Parameters.AddWithValue("@State", txtstate.Text);
        cmd.Parameters.AddWithValue("@Zip", txtzip.Text);
        cmd.Parameters.AddWithValue("@Phone", txtphone.Text);
        cmd.Parameters.AddWithValue("@ChapSvc", txtChapSvc.Text);
        cmd.Parameters.AddWithValue("@TradSvc", txtTradSvc.Text);
        cmd.Parameters.AddWithValue("@SameDaySvc", txtSameDaySvc.Text);
        cmd.Parameters.AddWithValue("@GraveSvc", txtGraveSvc.Text);
        cmd.Parameters.AddWithValue("@TradCrem", txtTradCrem.Text);
        cmd.Parameters.AddWithValue("@SameDayCrem", txtSameDayCrem.Text);
        cmd.Parameters.AddWithValue("@MemCrem", txtMemCrem.Text);
        cmd.Parameters.AddWithValue("@GraveCrem", txtMemCrem.Text);
        cmd.Parameters.AddWithValue("@DirectCrem", txtDirectCrem.Text);
        cmd.Parameters.AddWithValue("@MEscort", txtMEscort.Text);
        cmd.Parameters.AddWithValue("@AltContainer", txtAltContainer.Text);
        cmd.Parameters.AddWithValue("@Clergy", txtClergy.Text);
        cmd.Parameters.AddWithValue("@MemPkg", txtMemPkg.Text);
        cmd.Parameters.AddWithValue("@Video", txtVideo.Text);
        cmd.Parameters.AddWithValue("@Shroud", txtShroud.Text);
        cmd.Parameters.AddWithValue("@ShroudMuslin", txtShroudMuslin.Text);
        cmd.Parameters.AddWithValue("@Tahara", txtTahara.Text);
        cmd.Parameters.AddWithValue("@Shomer", txtShomer.Text);
        cmd.Parameters.AddWithValue("@Gratuities", txtGratuities.Text);
        cmd.Parameters.AddWithValue("@Limo", txtLimo.Text); 
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));
        cmd.ExecuteNonQuery();
        con.Close();
        lblresult.Text = "Funeral Home Record Details Updated Successfully";
        lblresult.ForeColor = Color.Green;
        GridView1.DataBind();
    }
    public void Sendmail_With_IcsAttachment(object sender, EventArgs e)
    {

        MailMessage msg = new MailMessage();
        //Now we have to set the value to Mail message properties

        //Note Please change it to correct mail-id to use this in your application
        msg.From = new MailAddress("info@eternalsolutionsllc.com", "Funeral Home Appointment");
        string toagent;
    
                toagent = "rsstangler1";
            
        msg.To.Add(new MailAddress(toagent + "@gmail.com", "txtagent.Text.ToString()"));
        //msg.CC.Add(new MailAddress("zzzzz@xyz.com", "DEF"));// it is optional, only if required
        msg.Subject = txtName.Text.ToString() + " " + txtadd.Text.ToString() + " " + txtcity.Text.ToString() + " " + txtstate.Text.ToString() + " " + txtzip.Text.ToString() + " " + txtphone.Text.ToString();
        msg.Body = "You have an Appointment with " + txtName.Text.ToString() + " " + txtphone.Text.ToString();

        //Parse the txtdate value
        string txtDate;
        txtDate = DateTime.Today.ToString();
        DateTime dt = Convert.ToDateTime(txtDate);
        
        // Now Contruct the ICS file using string builder
        StringBuilder str = new StringBuilder();
        str.AppendLine("BEGIN:VCALENDAR");
        str.AppendLine("PRODID:-//Schedule a Meeting");
        str.AppendLine("VERSION:2.0");
        str.AppendLine("METHOD:REQUEST");
        str.AppendLine("BEGIN:VEVENT");
        //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+330)));
        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+240)));
        str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now));
        //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+660)));
        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", dt.AddMinutes(+360)));
        str.AppendLine("LOCATION: " + txtadd.Text.ToString() + " " + txtcity.Text.ToString() + " " + txtstate.Text.ToString() + " " + txtzip.Text.ToString());
        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
        str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
        str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
        str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

        str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

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

}