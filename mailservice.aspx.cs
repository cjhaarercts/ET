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

public partial class mailservice : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataBind();
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
        e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;
        lblID.Text = GridView1.DataKeys[row.RowIndex].Value.ToString();

        txtlname.Text = row.Cells[3].Text;
        txtfname.Text = row.Cells[4].Text;
        txtadd.Text = row.Cells[5].Text;
        txtcity.Text = row.Cells[6].Text;
        txtstate.Text = row.Cells[7].Text;
        txtzip.Text = row.Cells[8].Text;
        txtstatus.Text = row.Cells[9].Text;

        ModalPopupExtender1.Show();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", con))
        {
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(lblID.Text);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(@"UPDATE Customers SET 
                                            FirstName = @FirstName,
                                            LastName  = @LastName,
                                            Address   = @Address,
                                            City      = @City,
                                            State     = @State,
                                            ZIP       = @ZIP,
                                            Status    = @Status,
                                            LDate     = @LDate,
                                            CDate     = @CDate
                                         WHERE Id = @Id", con))
        {
            cmd.Parameters.AddWithValue("@FirstName", txtfname.Text.Trim());
            cmd.Parameters.AddWithValue("@LastName", txtlname.Text.Trim());
            cmd.Parameters.AddWithValue("@Address", txtadd.Text.Trim());
            cmd.Parameters.AddWithValue("@City", txtcity.Text.Trim());
            cmd.Parameters.AddWithValue("@State", txtstate.Text.Trim());
            cmd.Parameters.AddWithValue("@ZIP", txtzip.Text.Trim());
            cmd.Parameters.AddWithValue("@Status", txtstatus.Text.Trim());

            // default nulls
            cmd.Parameters.Add("@LDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@CDate", SqlDbType.DateTime).Value = DBNull.Value;

            // Set LDate / CDate based on status
            if (txtstatus.Text.Equals("Left Message", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@LDate"].Value = DateTime.Today;
            }
            else if (txtstatus.Text.StartsWith("Call Back", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@CDate"].Value = DateTime.Today;
            }

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            con.Open();
            cmd.ExecuteNonQuery();
        }

        GridView1.DataBind();
        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }
}