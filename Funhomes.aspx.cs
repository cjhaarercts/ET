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
        ImageButton1.Visible = true; // Default: visible to all since auth removed

        GridViewRow gvrow = (GridViewRow)ImageButton1.NamingContainer;
        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        txtcompany.Text = gvrow.Cells[3].Text; 
        txtdirector.Text = gvrow.Cells[4].Text;
        txtcontact.Text = gvrow.Cells[5].Text;
        txtadd.Text = gvrow.Cells[6].Text;
        txtcity.Text = gvrow.Cells[7].Text;
        txtstate.Text = gvrow.Cells[8].Text;
        txtzip.Text = gvrow.Cells[9].Text;
        txtbphone.Text = gvrow.Cells[10].Text;
        txtmphone.Text = gvrow.Cells[11].Text; 
        txtnotes.Text = gvrow.Cells[12].Text;
        txtstatus.Text = gvrow.Cells[13].Text;
        txtDate.Text = gvrow.Cells[16].Text;
        if (txtDate.Text.Equals(" "))
            txtDate.Text = "";
        this.ModalPopupExtender1.Show();
        GridView1.DataBind();
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {

        ImageButton ImageButton2 = sender as ImageButton;
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
        lblresult.ForeColor = Color.Green;
        GridView1.DataBind();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;

        con.Open();
        SqlCommand cmd = new SqlCommand("update Funhomes set Company=@Company,Director=@Director,Contact=@Contact,Address=@Address,City=@City,State=@State,ZIP=@ZIP,BusPhone=@BusPhone,MobilePhone=@MobilePhone,EmailAddress=@EmailAddress,Notes=@Notes,Status=@Status,AppointmentSet=@AppointmentSet where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Company", txtcompany.Text);
        cmd.Parameters.AddWithValue("@Director", txtdirector.Text);
        cmd.Parameters.AddWithValue("@Contact", txtcontact.Text);
        cmd.Parameters.AddWithValue("@Address", txtadd.Text);
        cmd.Parameters.AddWithValue("@City", txtcity.Text);
        cmd.Parameters.AddWithValue("@State", txtstate.Text);
        cmd.Parameters.AddWithValue("@ZIP", txtzip.Text);
        cmd.Parameters.AddWithValue("@BusPhone", txtbphone.Text);
        cmd.Parameters.AddWithValue("@MobilePhone", txtmphone.Text);
        cmd.Parameters.AddWithValue("@EmailAddress", txtemail.Text);
        cmd.Parameters.AddWithValue("@Notes", txtnotes.Text);
        cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
        if (txtDate.Text == "")
            cmd.Parameters.AddWithValue("@AppointmentSet", sqldatenull);
        else
            cmd.Parameters.AddWithValue("@AppointmentSet", txtDate.Text); cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));
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
        msg.From = new MailAddress("info@ashersolutions.com.com", "Funeral Home Appointment");
        string toagent;
    
                toagent = "rsstangler1";
            
        msg.To.Add(new MailAddress(toagent + "@gmail.com", "txtagent.Text.ToString()"));
        //msg.CC.Add(new MailAddress("zzzzz@xyz.com", "DEF"));// it is optional, only if required
        msg.Subject = txtcompany.Text.ToString() + " " + txtdirector.Text.ToString() + " " + txtadd.Text.ToString() + " " + txtcity.Text.ToString() + " " + txtstate.Text.ToString() + " " + txtzip.Text.ToString() + " " + txtbphone.Text.ToString();
        msg.Body = "You have an Appointment with " + txtcontact.Text.ToString() + " " + txtbphone.Text.ToString();

        //Parse the txtdate value
        DateTime dt = Convert.ToDateTime(txtDate.Text);

        // Build ICS content via shared helper so format stays consistent
        string location = txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text;
        string ics = EmailHelper.BuildIcsContent(
            dt,
            location,
            msg.Body,
            msg.Subject,
            msg.From.Address,
            msg.To[0].DisplayName,
            msg.To[0].Address);

        //Now sending a mail with attachment ICS file using AlternateView pattern
        SmtpClient smtpclient = new SmtpClient("localhost");

        System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
        contype.Parameters.Add("method", "REQUEST");
        contype.Parameters.Add("name", "Meeting.ics");
        AlternateView avCal = AlternateView.CreateAlternateViewFromString(ics, contype);
        msg.AlternateViews.Add(avCal);
        smtpclient.Send(msg);


    }

}