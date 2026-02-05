using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentLM : Page
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
        txthdischarge.Text = gvRow.Cells[16].Text.Trim();
        txtages.Text = gvRow.Cells[17].Text.Trim();
        txtnotes.Text = gvRow.Cells[18].Text.Trim();
        txtDate.Text = gvRow.Cells[19].Text.Trim();
        txtapptset.Text = gvRow.Cells[20].Text.Trim();
        if (txtDate.Text.Equals(" "))
            txtDate.Text = "";
        this.ModalPopupExtender1.Show();

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