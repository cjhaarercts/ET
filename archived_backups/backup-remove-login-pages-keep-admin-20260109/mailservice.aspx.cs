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

public partial class mailservice : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Set "initial" query parameters, then ...
            GridView1.DataBind();
        }
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
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

        ImageButton ImageButton1 = sender as ImageButton;
        string userName = String.Empty; // Authentication removed
        ImageButton1.Visible = true; // Default: visible to all since auth removed

        GridViewRow gvrow = (GridViewRow)ImageButton1.NamingContainer;
        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        txtlname.Text = gvrow.Cells[3].Text;
        txtfname.Text = gvrow.Cells[4].Text;
        txtadd.Text = gvrow.Cells[5].Text;
        txtcity.Text = gvrow.Cells[6].Text;
        txtstate.Text = gvrow.Cells[7].Text;
        txtzip.Text = gvrow.Cells[8].Text;
        txtstatus.Text = gvrow.Cells[9].Text;
        this.ModalPopupExtender1.Show();
        GridView1.DataBind();
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {

        ImageButton ImageButton2 = sender as ImageButton;
        ImageButton2.Visible = true; // Default: visible to all since auth removed

        GridViewRow gvrow = (GridViewRow)ImageButton2.NamingContainer;
        string ID = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();
        Label id = (Label)gvrow.FindControl("lblID");
        SqlCommand cmd = new SqlCommand("delete from Customers where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(ID));

        con.Open();

        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.DataBind();
        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;

        con.Open();
        SqlCommand cmd = new SqlCommand("update Customers set FirstName=@FirstName,LastName=@LastName,Address=@Address,City=@City,State=@State,ZIP=@ZIP,Status=@Status where Id=@Id", con);
        cmd.Parameters.AddWithValue("@FirstName", txtfname.Text);
        cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
        cmd.Parameters.AddWithValue("@Address", txtadd.Text);
        cmd.Parameters.AddWithValue("@City", txtcity.Text);
        cmd.Parameters.AddWithValue("@State", txtstate.Text);
        cmd.Parameters.AddWithValue("@ZIP", txtzip.Text);
    
        if (txtstatus.Text == "None")
        {
            //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
            cmd.Parameters.AddWithValue("@CDate", sqldatenull);
            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
        }
        if (txtstatus.Text == "New")
        {
            //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
            cmd.Parameters.AddWithValue("@CDate", sqldatenull);
            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
        }
        else
            if (txtstatus.Text == "Active")
            {
                //cmd.Parameters["@CDate"].Value = DateTime.Today.AddMonths(1);
                cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
            }
            else
                if (txtstatus.Text == "Left Message")
                {
                    //cmd.Parameters["@LDate"].Value = DateTime.Today;
                    cmd.Parameters.AddWithValue("@LDate", DateTime.Today);
                    cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                }
                else
                    if (txtstatus.Text == "Call Back")
                    {
                        //cmd.Parameters["@LDate"].Value = DateTime.Today;
                        cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                        cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                    }
                    else
                        if (txtstatus.Text == "Call Back - Jan")
                        {
                            //cmd.Parameters["@LDate"].Value = DateTime.Today;
                            cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                        }
                        else
                            if (txtstatus.Text == "Call Back - Feb")
                            {
                                //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                            }
                            else
                                if (txtstatus.Text == "Call Back - Mar")
                                {
                                    //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                    cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                }
                                else
                                    if (txtstatus.Text == "Call Back - Apr")
                                    {
                                        //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                        cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                        cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                    }
                                    else
                                        if (txtstatus.Text == "Call Back - May")
                                        {
                                            //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                            cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                        }
                                        else
                                            if (txtstatus.Text == "Call Back - Jun")
                                            {
                                                //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                            }
                                            else
                                                if (txtstatus.Text == "Call Back - Jul")
                                                {
                                                    //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                    cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                }
                                                else
                                                    if (txtstatus.Text == "Call Back - Aug")
                                                    {
                                                        //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                        cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                        cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                    }
                                                    else
                                                        if (txtstatus.Text == "Call Back - Sep")
                                                        {
                                                            //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                            cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                        }
                                                        else
                                                            if (txtstatus.Text == "Call Back - Oct")
                                                            {
                                                                //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                            }
                                                            else
                                                                if (txtstatus.Text == "Call Back - Nov")
                                                                {
                                                                    //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                    cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                                }
                                                                else
                                                                    if (txtstatus.Text == "Call Back - Dec")
                                                                    {
                                                                        //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                        cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
                                                                        cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                                    }
                                                                    else
                                                                        if (txtstatus.Text == "Sent Mail")
                                                                        {
                                                                            //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                                            cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                                                                        }
                                                                        else
                                                                            if (txtstatus.Text == "Mail - Info")
                                                                            {
                                                                                //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                                                cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                                                                            }
                                                                            else
                                                                                if (txtstatus.Text == "Mail - FUR")
                                                                                {
                                                                                    //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                                                    cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                                                                                }
                                                                                else
                                                                                    if (txtstatus.Text == "Needs Phone Number")
                                                                                    {
                                                                                        //cmd.Parameters["@LDate"].Value = DateTime.Today;
                                                                                        cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                                                                                        cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                                                                                    }
        if (txtstatus.Text == "Last Resort")
        {
            //cmd.Parameters["@LDate"].Value = DateTime.Today;
            cmd.Parameters.AddWithValue("@LDate", sqldatenull);
            cmd.Parameters.AddWithValue("@CDate", sqldatenull);
        }
        else
            if (txtstatus.Text == "Sold")
            {
                //cmd.Parameters["@LDate"].Value = DateTime.Today;
                cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                cmd.Parameters.AddWithValue("@CDate", sqldatenull);
            }
            else
                if (txtstatus.Text == "Dead")
                {
                    //cmd.Parameters["@LDate"].Value = DateTime.Today;
                    cmd.Parameters.AddWithValue("@LDate", sqldatenull);
                    cmd.Parameters.AddWithValue("@CDate", sqldatenull);
                }
        cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));
        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.DataBind();
        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }
}