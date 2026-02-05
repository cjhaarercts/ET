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

public partial class tvp_Customer : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline2"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)

        {
            ddlManufacturer.AppendDataBoundItems = true;
            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["salespipeline2"].ConnectionString;
            String strQuery = "select ID, Manufacturer from casketman";
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.Connection = con;
            try
            {
                con.Open();
                ddlManufacturer.DataSource = cmd.ExecuteReader();
                ddlManufacturer.DataTextField = "Manufacturer";
                ddlManufacturer.DataValueField = "ID";
                ddlManufacturer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    protected void ddlManufacturer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlModel.Items.Clear();
        ddlModel.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Model--", ""));
        ddlModelNumber.Items.Clear();
        ddlModelNumber.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Model Number--", ""));
        ddlModel.AppendDataBoundItems = true;
        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["salespipeline2"].ConnectionString;
        String strQuery = "select ID, Model from casketmdl where ManufacturerID=@ManufacturerID";
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.AddWithValue("@ManufacturerID",
            ddlManufacturer.SelectedItem.Value);
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = con;
        try
        {
            con.Open();
            ddlModel.DataSource = cmd.ExecuteReader();
            ddlModel.DataTextField = "Model";
            ddlModel.DataValueField = "ID";
            ddlModel.DataBind();
            if (ddlModel.Items.Count > 1)
            {
                ddlModel.Enabled = true;
            }
            else
            {
                ddlModel.Enabled = false;
                ddlModelNumber.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }

    protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlModelNumber.Items.Clear();
        ddlModelNumber.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Model Number--", ""));
        ddlModelNumber.AppendDataBoundItems = true;
        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["salespipeline2"].ConnectionString;
        String strQuery = "select ID, ModelNumber from casketmdlnum where ModelID=@ModelID";
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Parameters.AddWithValue("@ModelID",
                              ddlModel.SelectedItem.Value);
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = con;
        try
        {
            con.Open();
            ddlModelNumber.DataSource = cmd.ExecuteReader();
            ddlModelNumber.DataTextField = "ModelNumber";
            ddlModelNumber.DataValueField = "ID";
            ddlModelNumber.DataBind();
            if (ddlModelNumber.Items.Count > 1)
            {
                ddlModelNumber.Enabled = true;
            }
            else
            {
                ddlModelNumber.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void ddlModelNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtCsktModel.Text = ddlManufacturer.SelectedItem.Text + " " + ddlModel.SelectedItem.Text;

        txtCsktModelNmbr.Text = ddlModelNumber.SelectedItem.Text;

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["salespipeline2"].ConnectionString;
        String strQuery = "select * from casketmdlnum WHERE ID=@ModelNumberID";
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.Parameters.AddWithValue("ModelNumberID",
                              ddlModelNumber.SelectedItem.Value);
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        con.Open();

        SqlDataReader read = cmd.ExecuteReader();

        while (read.Read())
        {
            txtexcolr.Text = (read["ExteriorColor"].ToString());
            txtincolr.Text = (read["InteriorColor"].ToString());
            txtwdtype.Text = (read["WoodType"].ToString());
            txtMtlType.Text = (read["Type"].ToString());
            txtoz1.Text = (read["Oz"].ToString());
            txtsteelgauge1.Text = (read["SteelGauge"].ToString());
            txtprice1.Text = (read["Price"].ToString());
            txtcouch1.Text = (read["Couch"].ToString());
            txtlining1.Text = (read["Lining"].ToString());
        }
        read.Close();
        {
            //Calendar1.DateMin = DateTime.Now;
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

        txtmodelid.Text = gvrow.Cells[3].Text; 
        txtmodelnumber.Text = gvrow.Cells[4].Text;
        txtexteriorcolor.Text = gvrow.Cells[5].Text;
        txtinteriorcolor.Text = gvrow.Cells[6].Text;
        txtwoodtype.Text = gvrow.Cells[7].Text;
        txttype.Text = gvrow.Cells[8].Text;
        txtoz.Text = gvrow.Cells[9].Text;
        txtsteelgauge.Text = gvrow.Cells[10].Text;
        txtprice.Text = gvrow.Cells[11].Text;
        txtcouch.Text = gvrow.Cells[12].Text;
        txtlining.Text = gvrow.Cells[13].Text;
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
        SqlCommand cmd = new SqlCommand("delete from casketmdlnum where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(ID));

        con.Open();

        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.DataBind();
        lblresult.Text = "Casket Record Deleted Successfully";
        lblresult.ForeColor = Color.Green;
        GridView1.DataBind();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;

        con.Open();
        SqlCommand cmd = new SqlCommand("update casketmdlnum set Manufacturer=@Manufacturer,ModelNumber=@ModelNumber,ExteriorColor=@ExteriorColor,InteriorCOlor=@InteriorColor,WoodType=@WoodType,Type=@Type,Oz=@Oz,SteelGauge=@SteelGauge,Price=@Price,Couch=@Couch,Lining=@Lining where Id=@Id", con);
        cmd.Parameters.AddWithValue("@Manufacturer", txtmanufacturer.Text);
        //cmd.Parameters.AddWithValue("@ModelID", txtModelID.Text);
        cmd.Parameters.AddWithValue("@ModelNumber", txtmodelnumber.Text);
        cmd.Parameters.AddWithValue("@ExteriorColor", txtexteriorcolor.Text);
        cmd.Parameters.AddWithValue("@InteriorColor", txtinteriorcolor.Text);
        cmd.Parameters.AddWithValue("@WoodType", txtwoodtype.Text);
        cmd.Parameters.AddWithValue("@Type", txttype.Text);
        cmd.Parameters.AddWithValue("@Oz", txtoz.Text);
        cmd.Parameters.AddWithValue("@Type", txttype.Text);
        cmd.Parameters.AddWithValue("@SteelGauge", txtsteelgauge.Text);
        cmd.Parameters.AddWithValue("@Price", txtprice.Text);
        cmd.Parameters.AddWithValue("@Couch", txtcouch.Text);
        cmd.Parameters.AddWithValue("@Lining", txtlining.Text);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));
        cmd.ExecuteNonQuery();
        con.Close();
        lblresult.Text = "Caasket Record Details Updated Successfully";
        lblresult.ForeColor = Color.Green;
        GridView1.DataBind();
    }
   

}