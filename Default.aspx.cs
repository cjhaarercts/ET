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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Authentication/authorization removed for PLMS.
        // Show a friendly message; the master page contains PLMS navigation.
        if (!IsPostBack)
        {
            WelcomeBackMessage.Text = "Welcome to the Partner Lead Management System (PLMS).";
        }
    }
}
