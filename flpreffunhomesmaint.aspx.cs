using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class flpreffunhomesmaint : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        try
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM flpreffunhm", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading data: " + ex.Message;
        }
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindGrid();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblMessage.Text = string.Empty;

        object keyValue = GridView1.DataKeys[e.RowIndex].Value;
        int id;
        if (!int.TryParse(keyValue.ToString(), out id))
        {
            lblMessage.Text = "Cannot update: invalid ID key.";
            return;
        }

        try
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"UPDATE flpreffunhm SET
                                            Name = @Name,
                                            Address = @Address,
                                            City = @City,
                                            State = @State,
                                            Zip = @Zip,
                                            Phone = @Phone,
                                            ChapSvc = @ChapSvc,
                                            TradSvc = @TradSvc,
                                            SameDaySvc = @SameDaySvc,
                                            GraveSvc = @GraveSvc,
                                            TradCrem = @TradCrem,
                                            SameDayCrem = @SameDayCrem,
                                            MemCrem = @MemCrem,
                                            GraveCrem = @GraveCrem,
                                            DirectCrem = @DirectCrem,
                                            Limo = @Limo,
                                            MEscort = @MEscort,
                                            AltContainer = @AltContainer,
                                            Clergy = @Clergy,
                                            MemPkg = @MemPkg,
                                            Video = @Video,
                                            Shroud = @Shroud,
                                            ShroudMuslin = @ShroudMuslin,
                                            Tahara = @Tahara,
                                            Shomer = @Shomer,
                                            Gratuities = @Gratuities
                                         WHERE Id = @Id", con))
            {
                // map values from the GridView's NewValues collection by column name
                cmd.Parameters.AddWithValue("@Name", (e.NewValues["Name"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Address", (e.NewValues["Address"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@City", (e.NewValues["City"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@State", (e.NewValues["State"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Zip", (e.NewValues["Zip"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Phone", (e.NewValues["Phone"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@ChapSvc", (e.NewValues["ChapSvc"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@TradSvc", (e.NewValues["TradSvc"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@SameDaySvc", (e.NewValues["SameDaySvc"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@GraveSvc", (e.NewValues["GraveSvc"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@TradCrem", (e.NewValues["TradCrem"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@SameDayCrem", (e.NewValues["SameDayCrem"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@MemCrem", (e.NewValues["MemCrem"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@GraveCrem", (e.NewValues["GraveCrem"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@DirectCrem", (e.NewValues["DirectCrem"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Limo", (e.NewValues["Limo"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@MEscort", (e.NewValues["MEscort"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@AltContainer", (e.NewValues["AltContainer"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Clergy", (e.NewValues["Clergy"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@MemPkg", (e.NewValues["MemPkg"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Video", (e.NewValues["Video"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Shroud", (e.NewValues["Shroud"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@ShroudMuslin", (e.NewValues["ShroudMuslin"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Tahara", (e.NewValues["Tahara"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Shomer", (e.NewValues["Shomer"] ?? string.Empty).ToString().Trim());
                cmd.Parameters.AddWithValue("@Gratuities", (e.NewValues["Gratuities"] ?? string.Empty).ToString().Trim());

                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            BindGrid();
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Record updated successfully.";
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Error updating record: " + ex.Message;
        }
    }
}
