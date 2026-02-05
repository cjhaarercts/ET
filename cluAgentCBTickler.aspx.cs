using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentCBTickler : Page
{
    private readonly SqlConnection _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Any initialization logic here
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

        txtfname.Text = gvRow.Cells[4].Text;
        txtlname.Text = gvRow.Cells[5].Text;
        txtadd.Text = gvRow.Cells[6].Text;
        txtcity.Text = gvRow.Cells[7].Text;
        txtstate.Text = gvRow.Cells[8].Text;
        txtzip.Text = gvRow.Cells[9].Text;
        txthphone.Text = gvRow.Cells[10].Text;
        txtmphone.Text = gvRow.Cells[11].Text;
        txtemail.Text = gvRow.Cells[12].Text;
        txtbranch.Text = gvRow.Cells[13].Text;
        txthdischarge.Text = gvRow.Cells[14].Text;
        txtages.Text = gvRow.Cells[15].Text;
        txtnotes.Text = gvRow.Cells[16].Text;
        txtDate.Text = gvRow.Cells[17].Text;
        txtagent.Text = gvRow.Cells[18].Text;
        txtapptset.Text = gvRow.Cells[19].Text;
        txtstatus.Text = gvRow.Cells[20].Text;

        ModalPopupExtender1.Show();
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

        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }
}