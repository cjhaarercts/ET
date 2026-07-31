using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tvp_Customer : Page
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
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
        e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvrow = (GridViewRow)imageButton.NamingContainer;
        lblID.Text = GridView1.DataKeys[gvrow.RowIndex].Value.ToString();

        txtagent.Text = gvrow.Cells[3].Text;
        txtstatus.Text = gvrow.Cells[4].Text;
        txtlname.Text = gvrow.Cells[6].Text;
        txtfname.Text = gvrow.Cells[7].Text;
        txtadd.Text = gvrow.Cells[8].Text;
        txtcity.Text = gvrow.Cells[9].Text;
        txtstate.Text = gvrow.Cells[10].Text;
        txtzip.Text = gvrow.Cells[11].Text;
        txthphone.Text = gvrow.Cells[12].Text;
        txtmphone.Text = gvrow.Cells[13].Text;
        txtemail.Text = gvrow.Cells[14].Text;
        txtbranch.Text = gvrow.Cells[15].Text;
        txthdischarge.Text = gvrow.Cells[16].Text;
        txtages.Text = gvrow.Cells[17].Text;
        txtnotes.Text = gvrow.Cells[18].Text;
        txtDate.Text = gvrow.Cells[19].Text.Trim() == "&nbsp;" ? "" : gvrow.Cells[19].Text;
        txtapptset.Text = gvrow.Cells[20].Text;

        ModalPopupExtender1.Show();
        GridView1.DataBind();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvrow = (GridViewRow)imageButton.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[gvrow.RowIndex].Value);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", con))
        {
            cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
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
            SendEmail(txtagent.Text, appointmentDate.Value);
        }

        ClearForm();
        GridView1.DataBind();
    }

    private void UpdateCustomerRecord(DateTime? appointmentDate)
    {
        int id = Convert.ToInt32(lblID.Text);

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

            cmd.Parameters.Add("@AppointmentSet", System.Data.SqlDbType.DateTime).Value = appointmentDate.HasValue ? (object)appointmentDate.Value : DBNull.Value;
            cmd.Parameters.Add("@LDate", System.Data.SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@CDate", System.Data.SqlDbType.DateTime).Value = DBNull.Value;

            if (txtstatus.Text.Equals("Left Message", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@LDate"].Value = DateTime.Today;
            }
            else if (txtstatus.Text.StartsWith("Call Back", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters["@CDate"].Value = DateTime.Today;
            }

            cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

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

        DateTime dt;
        if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
            return dt;

        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            return dt;

        return null;
    }

    private void SendEmail(string agent, DateTime appointmentDate)
    {
        // map to primary recipient and optional gmail alias
        string primaryRecipient;
        string gmailAlias;

        switch (agent)
        {
            case "GC Sharon Stangler":
            case "Asher Sharon Stangler":
            case "Sharon Stangler":
                primaryRecipient = "rsstangler1@gmail.com";
                gmailAlias = "rsstangler1";
                break;
            case "GC Richard Stangler":
            case "Asher Richard Stangler":
            case "Richard Stangler":
                primaryRecipient = "rjsstangler@gmail.com";
                gmailAlias = "rjsstangler";
                break;
            case "Mary Jo Hudson":
                primaryRecipient = "maryjoveteransprogram@gmail.com";
                gmailAlias = "maryjoveteransprogram";
                break;
            case "Amy Wallace":
                primaryRecipient = "awallacetvp@gmail.com";
                gmailAlias = "awallacetvp";
                break;
            default:
                primaryRecipient = "cj.haarer@gmail.com";
                gmailAlias = "cj.haarer";
                break;
        }

        string subject = string.Format("Appointment with {0} {1} on {2:MMMM d, yyyy h:mm tt}", txtfname.Text, txtlname.Text, appointmentDate);
        string body = string.Format(
            "You have an Appointment with {0} {1}. Home: {2}. Mobile: {3}. Branch: {4}. Discharge: {5}. Age: {6}",
            txtfname.Text, txtlname.Text, txthphone.Text, txtmphone.Text, txtbranch.Text, txthdischarge.Text, txtages.Text);

        // prepare recipient lists for the shared email helper
        var toEmails = new System.Collections.Generic.List<string>();
        var toNames = new System.Collections.Generic.List<string>();
        var ccEmails = new System.Collections.Generic.List<string>();
        var ccNames = new System.Collections.Generic.List<string>();

        toEmails.Add(primaryRecipient);
        toNames.Add(txtagent.Text);

        // optional agent Gmail alias CC, if different
        try
        {
            var agentGmail = gmailAlias + "@gmail.com";
            if (!string.Equals(agentGmail, primaryRecipient, StringComparison.OrdinalIgnoreCase))
            {
                ccEmails.Add(agentGmail);
                ccNames.Add(txtagent.Text);
            }
        }
        catch
        {
            // ignore malformed alias
        }

        string location = txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text;

        string errorMessage;
        bool success = EmailHelper.SendAppointmentEmailWithIcs(
            "info@ashersolutions.com",
            "Sales Lead Appointment",
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
        txtages.Text = string.Empty;
        txtnotes.Text = string.Empty;
        txtDate.Text = string.Empty;
        Calendar1.SelectedDate = DateTime.MinValue;

        // clear dropdown selections safely
        if (txtbranch != null && txtbranch.Items != null)
        {
            txtbranch.ClearSelection();
        }
        if (txthdischarge != null && txthdischarge.Items != null)
        {
            txthdischarge.ClearSelection();
        }
        if (txtagent != null && txtagent.Items != null)
        {
            txtagent.ClearSelection();
        }
        if (txtapptset != null && txtapptset.Items != null)
        {
            txtapptset.ClearSelection();
        }
        if (txtstatus != null && txtstatus.Items != null)
        {
            txtstatus.ClearSelection();
        }

        ModalPopupExtender1.Hide();
    }
}