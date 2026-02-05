using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Security.Permissions;

public partial class inputfh : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline2"].ConnectionString);
        con.Open();
        string insStr = "Insert into Funhomes (Company, Director, Contact, Address, City, State, ZIP, BusPhone, MobilePhone, EmailAddress, Status, Created) values (@Company, @Director, @Contact, @Address, @City, @State, @ZIP, @BusPhone, @MobilePhone, @EmailAddress, @Status, @Created)";
        SqlCommand insertuser = new SqlCommand(insStr, con);
        insertuser.Parameters.AddWithValue("@Company", txtcompany.Text);
        insertuser.Parameters.AddWithValue("@Director", txtdirector.Text);
        insertuser.Parameters.AddWithValue("@Contact", txtcontact.Text);
        insertuser.Parameters.AddWithValue("@Address", txtadd.Text);
        insertuser.Parameters.AddWithValue("@City", txtcity.Text);
        insertuser.Parameters.AddWithValue("@State", txtstate.Text);
        insertuser.Parameters.AddWithValue("@ZIP", txtzip.Text);
        insertuser.Parameters.AddWithValue("@BusPhone", txtbphone.Text);
        insertuser.Parameters.AddWithValue("@MobilePhone", txtmphone.Text);
        insertuser.Parameters.AddWithValue("@EmailAddress", txtemail.Text);
        insertuser.Parameters.AddWithValue("@Status", txtstatus.Text);
        insertuser.Parameters.AddWithValue("@Created", DateTime.Today);
        try
        {
            insertuser.ExecuteNonQuery();
            con.Close();
            Response.Write(Label1.Text=("Record Added"));
            txtcompany.Text = "";
            txtdirector.Text = "";
            txtcontact.Text = "";
            txtadd.Text = "";
            txtcity.Text = "";
            txtstate.Text = "";
            txtzip.Text = "";
            txtbphone.Text = "";
            txtmphone.Text = "";
            txtemail.Text = "";
            txtstatus.Text = "";
        }
        catch (Exception)
        {
            Response.Write("Something really bad happened .... Please try again </br>");
        }
        finally
        {
            //other stuff here
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            // Login redirect removed as authentication is disabled
        }
}

