using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class _secure : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Authentication removed: no current user information available.
        // Previously used Membership.GetUser();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["New"] = null;
        // Logout page archived; redirect to home instead
        Response.Redirect("~/default.aspx");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../customer.aspx");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../inputform.aspx");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../custlookupln.aspx");
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../custlookuphp.aspx");
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../custlookupagnt.aspx");
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentnew.aspx");
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentActive.aspx");
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentCBTickler.aspx");
    }
    protected void Button20_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentCB.aspx");
    }
    protected void Button10_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentLMTickler.aspx");
    }
    protected void Button11_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluLastResort.aspx");
    }
    protected void Button12_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentSold.aspx");
    }
    protected void Button14_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../inputsoldlead.aspx");
    }
    protected void Button15_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Button15.Visible = true; // roles removed; enable access
        Response.Redirect("../funhomes.aspx");
    }
    protected void Button16_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Button16.Visible = true; // roles removed; enable access
        Response.Redirect("../inputfh.aspx");
    }
    protected void Button17_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentSM.aspx");
    }
    protected void Button18_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentWeb.aspx");
    }
    protected void Button111_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("../cluAgentSeminar.aspx");
    }
    protected void Button19_Click(object sender, EventArgs e)
    {
        //Session["New"] = null;
        Response.Redirect("~/pdfforms/default.aspx");
    }
}