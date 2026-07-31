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

public partial class custlookuphp : System.Web.UI.Page
{
    // do not read ConfigurationManager at field-initializer time (can run before config is available).
    private string _connectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        // initialize connection string safely at runtime and provide a clear error if missing
        if (string.IsNullOrEmpty(_connectionString))
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["salespipeline"];
            if (cs == null)
            {
                // optional fallback to the primary connection if present
                cs = ConfigurationManager.ConnectionStrings["salespipeline"];
            }

            if (cs == null)
            {
                // Fail fast with actionable message instead of NullReferenceException from ctor
                throw new InvalidOperationException("Missing connection string 'salespipeline2' (and fallback 'salespipeline'). Add it to web.config under <connectionStrings>.");
            }

            _connectionString = cs.ConnectionString;
        }

        if (!Page.IsPostBack)
        {
            // Calendar1.DateMin = DateTime.Now;
            GridView1.DataBind();
        }
    }

    protected void chkLinked_CheckedChanged(object sender, EventArgs args)
    {
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        // only operate on data rows
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
        e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
    }

    // shared helper used on other pages — keep consistent parsing behavior
    private DateTime? ParseDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // Explicit out variable for compatibility with older compilers
        DateTime parsed;
        if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
            return parsed;

        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            return parsed;

        return null;
    }
}
