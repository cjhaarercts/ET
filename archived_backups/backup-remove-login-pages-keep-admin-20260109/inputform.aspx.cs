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

public partial class _inputform : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        int chkValue;
        int chkValue1;
        int chkValue2;
        int chkValue3;
        int chkValue4;
        if (CheckBox.Checked)
            {
                chkValue = 1;
            }
            else
            {
                chkValue = 0;
            }
        if (CheckBox1.Checked)
            {
                chkValue1 = 1;
            }
            else
            {
                chkValue1 = 0;
            }
        if (CheckBox2.Checked)
            {
                chkValue2 = 1;
            }
            else
            {
                chkValue2 = 0;
            }
        if (CheckBox3.Checked)
            {
                chkValue3 = 1;
            }
            else
            {
                chkValue3 = 0;
            }
        if (CheckBox4.Checked)
            {
                chkValue4 = 1;
            }
        else
            {
                chkValue4 = 0;
            }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString);
        con.Open();
        string insStr = "Insert into Customers (FirstName, LastName, Address, City, State, ZIP, HomePhone, MobilePhone, EmailAddress, Branch, HDischarge, Ages, Notes, Agent, Status, Web, VPP, Asher, Seminar, VPW, Created) values (@FirstName, @LastName, @Address, @City, @State, @ZIP, @HomePhone, @MobilePhone, @EmailAddress, @Branch, @HDischarge, @Ages, @Notes, @Agent, @Status, @Web, @VPP, @Asher, @Seminar, @VPW, @Created)";
        SqlCommand insertuser = new SqlCommand(insStr, con);
        insertuser.Parameters.AddWithValue("@FirstName", txtfname.Text);
        insertuser.Parameters.AddWithValue("@LastName", txtlname.Text);
        insertuser.Parameters.AddWithValue("@Address", txtadd.Text);
        insertuser.Parameters.AddWithValue("@City", txtcity.Text);
        insertuser.Parameters.AddWithValue("@State", txtstate.Text);
        insertuser.Parameters.AddWithValue("@ZIP", txtzip.Text);
        insertuser.Parameters.AddWithValue("@HomePhone", txthphone.Text);
        insertuser.Parameters.AddWithValue("@MobilePhone", txtmphone.Text);
        insertuser.Parameters.AddWithValue("@EmailAddress", txtemail.Text);
        insertuser.Parameters.AddWithValue("@Branch", txtbranch.Text);
        insertuser.Parameters.AddWithValue("@HDischarge", txthdischarge.Text);
        insertuser.Parameters.AddWithValue("@Ages", txtages.Text);
        insertuser.Parameters.AddWithValue("@Notes", txttype.Text);
        insertuser.Parameters.AddWithValue("@Agent", txtagent.Text);
        //insertuser.Parameters.AddWithValue("@Status", txtstatus.Text);
        insertuser.Parameters.AddWithValue("@Status", "New");
        insertuser.Parameters.AddWithValue("@VPP", chkValue1);
        insertuser.Parameters.AddWithValue("@Asher", chkValue2);
        insertuser.Parameters.AddWithValue("@Web", chkValue3);
        insertuser.Parameters.AddWithValue("@Seminar", chkValue4);
        insertuser.Parameters.AddWithValue("@VPW", chkValue);
        insertuser.Parameters.AddWithValue("@Created", DateTime.Today);
        try
        {
            insertuser.ExecuteNonQuery();
            con.Close();
            Response.Write(Label1.Text=("Record Added"));
            txtfname.Text = "";
            txtlname.Text = "";
            txtadd.Text = "";
            txtcity.Text = "";
            txtstate.Text = "";
            txtzip.Text = "";
            txthphone.Text = "";
            txtmphone.Text = "";
            txtemail.Text = "";
            txtbranch.Text = "";
            txthdischarge.Text = "";
            txtages.Text = "";
            txttype.Text = "";
            txtagent.Text = "";
            //txtstatus.Text = "";
            chkValue = 0;
            chkValue1 = 0;
            chkValue2 = 0;
            chkValue3 = 0;
            chkValue4 = 0;
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

