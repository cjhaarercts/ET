using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class casketrsmaint : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private string[] ColumnNames
    {
        get { return ViewState["casketrsColumns"] as string[]; }
        set { ViewState["casketrsColumns"] = value; }
    }

    private void BindGrid()
    {
        try
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM casketrs", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                // capture column names for dynamic UPDATE later
                var cols = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cols[i] = dt.Columns[i].ColumnName;
                }
                ColumnNames = cols;

                // choose a key column: prefer ModelNumber, otherwise fall back to the first column
                string keyColumn = GetKeyColumnName();
                if (!string.IsNullOrEmpty(keyColumn))
                {
                    GridView1.DataKeyNames = new[] { keyColumn };
                }

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading data: " + ex.Message;
        }
    }

    private string GetKeyColumnName()
    {
        if (ColumnNames == null || ColumnNames.Length == 0)
        {
            return null;
        }

        // Prefer ModelNumber as a logical key for caskets
        for (int i = 0; i < ColumnNames.Length; i++)
        {
            if (string.Equals(ColumnNames[i], "ModelNumber", StringComparison.OrdinalIgnoreCase))
            {
                return ColumnNames[i];
            }
        }

        // Fallback: use the first column
        return ColumnNames[0];
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

        if (ColumnNames == null || ColumnNames.Length == 0)
        {
            lblMessage.Text = "Cannot update: column metadata not available.";
            return;
        }

        string keyColumn = GetKeyColumnName();
        if (string.IsNullOrEmpty(keyColumn))
        {
            lblMessage.Text = "Cannot update: key column not available.";
            return;
        }

        object keyValue = null;
        if (GridView1.DataKeys != null && GridView1.DataKeys.Count > e.RowIndex)
        {
            keyValue = GridView1.DataKeys[e.RowIndex].Value;
        }
        if (keyValue == null)
        {
            lblMessage.Text = "Cannot update: key value not available.";
            return;
        }

        GridViewRow row = GridView1.Rows[e.RowIndex];

        try
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;

                var setParts = new System.Text.StringBuilder();

                for (int i = 0; i < ColumnNames.Length && i < row.Cells.Count; i++)
                {
                    string colName = ColumnNames[i];
                    // do not update the key column
                    if (string.Equals(colName, keyColumn, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    TableCell cell = row.Cells[i];
                    string value = ExtractCellValue(cell);

                    string paramName = "@p" + i;

                    if (setParts.Length > 0)
                    {
                        setParts.Append(", ");
                    }
                    setParts.Append("[").Append(colName).Append("] = ").Append(paramName);

                    object paramValue = string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : (object)value.Trim();
                    cmd.Parameters.AddWithValue(paramName, paramValue);
                }

                cmd.CommandText = "UPDATE casketrs SET " + setParts + " WHERE [" + keyColumn + "] = @Key";
                cmd.Parameters.AddWithValue("@Key", keyValue);

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

    private static string ExtractCellValue(TableCell cell)
    {
        if (cell.Controls.Count > 0)
        {
            TextBox tb = cell.Controls[0] as TextBox;
            if (tb != null)
            {
                return tb.Text;
            }
        }

        return cell.Text == "&nbsp;" ? string.Empty : cell.Text;
    }
}
