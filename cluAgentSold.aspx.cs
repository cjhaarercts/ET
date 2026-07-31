using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluAgentSold : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Calendar1.DateMin = DateTime.Now;
            GridView1.DataBind();
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        lblID.Text = GridView1.DataKeys[gvRow.RowIndex].Value.ToString();

        txtagent.Text = gvRow.Cells[3].Text.Trim();
        txtstatus.Text = gvRow.Cells[4].Text.Trim();
        txtlname.Text = gvRow.Cells[6].Text.Trim();
        txtfname.Text = gvRow.Cells[7].Text.Trim();
        txtadd.Text = gvRow.Cells[8].Text.Trim();
        txtcity.Text = gvRow.Cells[9].Text.Trim();
        txtstate.Text = gvRow.Cells[10].Text.Trim();
        txtzip.Text = gvRow.Cells[11].Text.Trim();
        txthphone.Text = gvRow.Cells[12].Text.Trim();
        txtmphone.Text = gvRow.Cells[13].Text.Trim();
        txtemail.Text = gvRow.Cells[14].Text.Trim();
        txtbranch.Text = gvRow.Cells[15].Text.Trim();
        txthdischarge.Text = gvRow.Cells[16].Text;
        txtages.Text = gvRow.Cells[17].Text.Trim();
        txtnotes.Text = gvRow.Cells[18].Text.Trim();
        txtDate.Text = gvRow.Cells[19].Text.Trim() == "&nbsp;" ? "" : gvRow.Cells[19].Text.Trim();
        txtapptset.Text = gvRow.Cells[20].Text.Trim();

        ModalPopupExtender1.Show();
        GridView1.DataBind();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[gvRow.RowIndex].Value);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id=@Id", con))
        {
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DateTime? appointmentDate = ParseDate(txtDate.Text);

        UpdateCustomerRecord(appointmentDate);

        if (appointmentDate.HasValue)
        {
            SendAppointmentEmail(appointmentDate.Value);
        }

        ClearForm();
        GridView1.DataBind();
    }

    private void UpdateCustomerRecord(DateTime? appointmentDate)
    {
        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(@"UPDATE Customers SET 
                                            FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, 
                                            State=@State, ZIP=@ZIP, HomePhone=@HomePhone, MobilePhone=@MobilePhone, 
                                            EmailAddress=@EmailAddress, Branch=@Branch, HDischarge=@HDischarge, 
                                            Ages=@Ages, Notes=@Notes, AppointmentSet=@AppointmentSet, Agent=@Agent, 
                                            ApptSetter=@ApptSetter, Status=@Status, LDate=@LDate, CDate=@CDate 
                                         WHERE Id=@Id", con))
        {
            cmd.Parameters.AddWithValue("@FirstName", txtfname.Text);
            cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
            cmd.Parameters.AddWithValue("@Address", txtadd.Text);
            cmd.Parameters.AddWithValue("@City", txtcity.Text);
            cmd.Parameters.AddWithValue("@State", txtstate.Text);
            cmd.Parameters.AddWithValue("@ZIP", txtzip.Text);
            cmd.Parameters.AddWithValue("@HomePhone", txthphone.Text);
            cmd.Parameters.AddWithValue("@MobilePhone", txtmphone.Text);
            cmd.Parameters.AddWithValue("@EmailAddress", txtemail.Text);
            cmd.Parameters.AddWithValue("@Branch", txtbranch.Text);
            cmd.Parameters.AddWithValue("@HDischarge", txthdischarge.Text);
            cmd.Parameters.AddWithValue("@Ages", txtages.Text);
            cmd.Parameters.AddWithValue("@Notes", txtnotes.Text);
            cmd.Parameters.AddWithValue("@Agent", txtagent.Text);
            cmd.Parameters.AddWithValue("@ApptSetter", txtapptset.Text);
            cmd.Parameters.AddWithValue("@Status", txtstatus.Text);

            // typed parameters for dates and id
            cmd.Parameters.Add("@AppointmentSet", SqlDbType.DateTime).Value = appointmentDate.HasValue ? (object)appointmentDate.Value : DBNull.Value;
            cmd.Parameters.Add("@LDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@CDate", SqlDbType.DateTime).Value = DBNull.Value;

            // Set LDate/CDate based on status; treat "Texted Lead" like "Left Message"
            if (txtstatus.Text.Equals("Left Message", StringComparison.OrdinalIgnoreCase) ||
                txtstatus.Text.Equals("Texted Lead", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@LDate"].Value = DateTime.Today;
            }
            else if (txtstatus.Text.StartsWith("Call Back", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@CDate"].Value = DateTime.Today;
            }

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(lblID.Text);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }

    private DateTime? ParseDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // explicit out variable for compatibility with older compilers
        DateTime parsed;
        if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
            return parsed;

        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            return parsed;

        return null;
    }

    private void SendAppointmentEmail(DateTime appointmentDate)
    {
        string subject = string.Format("Appointment with {0} {1} on {2:MMMM d, yyyy h:mm tt}", txtfname.Text, txtlname.Text, appointmentDate);
        string body = string.Format("You have an Appointment with {0} {1}. Home: {2}. Mobile: {3}. Branch: {4}. Discharge: {5}. Age: {6}",
            txtfname.Text, txtlname.Text, txthphone.Text, txtmphone.Text, txtbranch.Text, txthdischarge.Text, txtages.Text);

        string agentAlias = GetAgentEmailAlias(txtagent.Text);
        string recipient = agentAlias.Contains("@") ? agentAlias : agentAlias + "@gmail.com";

        // prepare recipient lists for the shared email helper
        var toEmails = new System.Collections.Generic.List<string>();
        var toNames = new System.Collections.Generic.List<string>();
        var ccEmails = new System.Collections.Generic.List<string>();
        var ccNames = new System.Collections.Generic.List<string>();

        toEmails.Add(recipient);
        toNames.Add(txtagent.Text);

        string location = txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text;

        string errorMessage;
        bool success = EmailHelper.SendAppointmentEmailWithIcs(
            "info@ashersolutions.com",
            "Asher Solutions",
            subject,
            body,
            appointmentDate,
            location,
            toEmails,
            toNames,
            ccEmails,
            ccNames,
            out errorMessage);

        if (success)
        {
            lblresult.Text = "Email sent successfully.";
            lblresult.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            lblresult.Text = "Email error: " + errorMessage;
            lblresult.ForeColor = System.Drawing.Color.Red;
        }
    }

    private string GetAgentEmailAlias(string agent)
    {
        // classic switch for compatibility with older compilers
        switch (agent)
        {
            case "VPP Sharon Stangler":
            case "Asher Sharon Stangler":
            case "Sharon Stangler":
                return "rsstangler1";
            case "VPP Richard Stangler":
            case "Asher Richard Stangler":
            case "Richard Stangler":
                return "rjsstangler";
            case "Mary Jo Hudson":
                return "maryjoveteransprogram";
            case "Amy Wallace":
                return "awallacetvp";
            case "Serenity":
                return "donna.haarer";
            default:
                return "cj.haarer";
        }
    }

    private void ClearForm()
    {
        foreach (IValidator ctrl in Validators)
        {
            ctrl.IsValid = true;
        }

        txtfname.Text = string.Empty;
        txtlname.Text = string.Empty;
        txtadd.Text = string.Empty;
        txtcity.Text = string.Empty;
        txtstate.Text = string.Empty;
        txtzip.Text = string.Empty;
        txtmphone.Text = string.Empty;
        txtemail.Text = string.Empty;
        if (txtbranch.Items.FindByValue("") != null)
        {
            txtbranch.SelectedValue = "";
        }
        else
        {
            txtbranch.ClearSelection();
        }
        if (txthdischarge.Items.FindByValue("") != null)
        {
            txthdischarge.SelectedValue = "";
        }
        else
        {
            txthdischarge.ClearSelection();
        }
        txtages.Text = string.Empty;
        txtnotes.Text = string.Empty;
        txtDate.Text = string.Empty;
        Calendar1.SelectedDate = DateTime.MinValue;
        if (txtagent.Items.FindByValue("") != null)
        {
            txtagent.SelectedValue = "";
        }
        else
        {
            txtagent.ClearSelection();
        }
        if (txtapptset.Items.FindByValue("") != null)
        {
            txtapptset.SelectedValue = "";
        }
        else
        {
            txtapptset.ClearSelection();
        }
        if (txtstatus.Items.FindByValue("") != null)
        {
            txtstatus.SelectedValue = "";
        }
        else
        {
            txtstatus.ClearSelection();
        }

        ModalPopupExtender1.Hide();
    }
}