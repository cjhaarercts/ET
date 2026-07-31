using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _inputform : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        int chkValue = CheckBox.Checked ? 1 : 0;
        int chkValue1 = CheckBox1.Checked ? 1 : 0;
        int chkValue2 = CheckBox2.Checked ? 1 : 0;
        int chkValue3 = CheckBox3.Checked ? 1 : 0;
        int chkValue4 = CheckBox4.Checked ? 1 : 0;

        const string insStr = @"
            INSERT INTO Customers
                (FirstName, LastName, Address, City, State, ZIP, HomePhone, MobilePhone, EmailAddress,
                 Branch, HDischarge, Ages, Notes, Agent, Status, Web, VPP, Asher, Seminar, VPW, Created)
            VALUES
                (@FirstName, @LastName, @Address, @City, @State, @ZIP, @HomePhone, @MobilePhone, @EmailAddress,
                 @Branch, @HDischarge, @Ages, @Notes, @Agent, @Status, @Web, @VPP, @Asher, @Seminar, @VPW, @Created)";

        try
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(insStr, con))
            {
                // Text fields
                cmd.Parameters.AddWithValue("@FirstName", txtfname.Text.Trim());
                cmd.Parameters.AddWithValue("@LastName", txtlname.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtadd.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtcity.Text.Trim());
                cmd.Parameters.AddWithValue("@State", txtstate.Text.Trim());
                cmd.Parameters.AddWithValue("@ZIP", txtzip.Text.Trim());
                cmd.Parameters.AddWithValue("@HomePhone", txthphone.Text.Trim());
                cmd.Parameters.AddWithValue("@MobilePhone", txtmphone.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailAddress", txtemail.Text.Trim());
                cmd.Parameters.AddWithValue("@Branch", txtbranch.Text.Trim());
                cmd.Parameters.AddWithValue("@HDischarge", txthdischarge.Text.Trim());
                cmd.Parameters.AddWithValue("@Ages", txtages.Text.Trim());
                cmd.Parameters.AddWithValue("@Notes", txttype.Text.Trim());
                cmd.Parameters.AddWithValue("@Agent", txtagent.Text.Trim());

                // fixed status for new records
                cmd.Parameters.AddWithValue("@Status", "New");

                // flags
                cmd.Parameters.AddWithValue("@VPP", chkValue1);
                cmd.Parameters.AddWithValue("@Asher", chkValue2);
                cmd.Parameters.AddWithValue("@Web", chkValue3);
                cmd.Parameters.AddWithValue("@Seminar", chkValue4);
                cmd.Parameters.AddWithValue("@VPW", chkValue);

                // typed DateTime parameter for Created
                cmd.Parameters.Add("@Created", SqlDbType.DateTime).Value = DateTime.Today;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Label1.Text = "Record Added";
            Label1.ForeColor = System.Drawing.Color.Green;

            // clear the form
            txtfname.Text = string.Empty;
            txtlname.Text = string.Empty;
            txtadd.Text = string.Empty;
            txtcity.Text = string.Empty;
            txtstate.Text = string.Empty;
            txtzip.Text = string.Empty;
            txthphone.Text = string.Empty;
            txtmphone.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtbranch.Text = string.Empty;
            txthdischarge.Text = string.Empty;
            txtages.Text = string.Empty;
            txttype.Text = string.Empty;
            txtagent.Text = string.Empty;

            CheckBox.Checked = false;
            CheckBox1.Checked = false;
            CheckBox2.Checked = false;
            CheckBox3.Checked = false;
            CheckBox4.Checked = false;
        }
        catch (Exception ex)
        {
            // Surface a friendly message; include exception message for diagnostics
            Label1.Text = "Error adding record: " + ex.Message;
            Label1.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["New"] = null;
        // authentication/redirect removed intentionally
    }
}

