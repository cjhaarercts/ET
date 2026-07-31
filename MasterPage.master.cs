using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // No authentication/authorization checks here per project requirements.
        // Page load left intentionally minimal.
    }

    protected void NavigateToPage(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn == null || string.IsNullOrEmpty(btn.CommandArgument))
            return;

        // Resolve "~/" and navigate
        string target = ResolveUrl(btn.CommandArgument);
        Response.Redirect(target);
    }
}
