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

public partial class custlookuphp   : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline2"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Calendar1.DateMin = DateTime.Now;
            // Set "initial" query parameters, then ...
            GridView1.DataBind();
        }
    }
    protected void chkLinked_CheckedChanged(Object sender, EventArgs args)
    {

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
}
