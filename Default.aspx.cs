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
        // Authentication removed - always show anonymous view
        WelcomeBackMessage.Text = String.Empty;
        AuthenticatedMessagePanel.Visible = false;
        AnonymousMessagePanel.Visible = true;

        SetButtonVisibility();
    }

    protected void NavigateToPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            Response.Redirect(button.CommandArgument);
        }
    }

    private void SetButtonVisibility()
    {
        bool isAdminOrSupervisor = false; // roles removed

        Button15.Visible = isAdminOrSupervisor;
        Button21.Visible = isAdminOrSupervisor;
        Button16.Visible = isAdminOrSupervisor;
        DButton.Visible = isAdminOrSupervisor;
    }
}
