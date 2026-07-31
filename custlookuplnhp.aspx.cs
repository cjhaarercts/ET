using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custlookuplnhp : Page
{
    private readonly CustomerRepository _customerRepository = new CustomerRepository();
    private readonly string connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Calendar1.SelectedDate = DateTime.Now;
            GridView1.DataBind();
        }

        // ensure the Button1 click handler is wired at runtime for Web Site projects
        Button1.Click -= Button1_Click; // safe: remove any duplicate delegates
        Button1.Click += Button1_Click;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.PageIndex = 0;  // optional, but avoids “empty page” when paging
        GridView1.DataBind();
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
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;

        lblID.Text = GridView1.DataKeys[row.RowIndex].Value.ToString();

        // Safely apply agent value even if it is not present in the dropdown list
        string agentValue = row.Cells[3].Text == null ? string.Empty : row.Cells[3].Text.Trim();
        try
        {
            if (txtagent != null)
            {
                ListItem foundAgent = null;
                if (txtagent.Items != null)
                {
                    foundAgent = txtagent.Items.FindByText(agentValue) ?? txtagent.Items.FindByValue(agentValue);
                }

                if (foundAgent != null)
                {
                    txtagent.ClearSelection();
                    foundAgent.Selected = true;
                }
                else if (txtagent.Items != null)
                {
                    // insert a temporary list item so SelectedValue is valid and UI shows the value
                    txtagent.Items.Insert(0, new ListItem(agentValue, agentValue));
                    txtagent.ClearSelection();
                    txtagent.Items[0].Selected = true;
                }
                else
                {
                    txtagent.Text = agentValue;
                }
            }
        }
        catch
        {
            // final fallback: set Text and continue — avoid throwing to user
            if (txtagent != null)
            {
                txtagent.Text = agentValue;
            }
        }

        txtstatus.Text = row.Cells[4].Text;
        txtlname.Text = row.Cells[6].Text;
        txtfname.Text = row.Cells[7].Text;
        txtadd.Text = row.Cells[8].Text;
        txtcity.Text = row.Cells[9].Text;
        txtstate.Text = row.Cells[10].Text;
        txtzip.Text = row.Cells[11].Text;
        txthphone.Text = row.Cells[12].Text;
        txtmphone.Text = row.Cells[13].Text;
        txtemail.Text = row.Cells[14].Text;
        txtbranch.Text = row.Cells[15].Text;
        txthdischarge.Text = row.Cells[16].Text;
        txtages.Text = row.Cells[17].Text;
        txtnotes.Text = row.Cells[18].Text;
        txtDate.Text = row.Cells[19].Text.Trim() == "&nbsp;" ? "" : row.Cells[19].Text;
        txtapptset.Text = row.Cells[20].Text;

        ModalPopupExtender1.Show();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

        _customerRepository.Delete(id);

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Parse the appointment date if provided
            DateTime? appointmentSet = null;
            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                DateTime parsedDate = Convert.ToDateTime(txtDate.Text);

                // Get browser timezone offset (in minutes from UTC)
                int timezoneOffset = 0;
                if (!string.IsNullOrEmpty(hdnTimezoneOffset.Value))
                {
                    int.TryParse(hdnTimezoneOffset.Value, out timezoneOffset);
                }

                // If we have timezone info from browser, use it
                // Otherwise, assume Eastern Time (most common for this app)
                if (timezoneOffset != 0)
                {
                    // Convert from browser timezone to UTC properly
                    appointmentSet = TimezoneHelper.ConvertToUtcForIcs(parsedDate, timezoneOffset);
                }
                else
                {
                    // Default to Eastern Time if no browser offset captured
                    appointmentSet = TimezoneHelper.ConvertEasternToUtcForIcs(parsedDate);
                }
            }

            // Map form fields to Customer entity
            var customer = new Customer
            {
                Id = Convert.ToInt32(lblID.Text),
                FirstName = txtfname.Text,
                LastName = txtlname.Text,
                Address = txtadd.Text,
                City = txtcity.Text,
                State = txtstate.Text,
                ZIP = txtzip.Text,
                HomePhone = txthphone.Text,
                MobilePhone = txtmphone.Text,
                EmailAddress = txtemail.Text,
                Branch = txtbranch.Text,
                HDischarge = txthdischarge.Text,
                Ages = txtages.Text,
                Notes = txtnotes.Text,
                Agent = txtagent.Text,
                ApptSetter = txtapptset.Text,
                Status = txtstatus.Text,
                AppointmentSet = appointmentSet
            };

            // Save to database using repository pattern
            _customerRepository.Update(customer);

            lblresult.Text = "Customer Record Details Updated Successfully";
            lblresult.ForeColor = System.Drawing.Color.Green;
            GridView1.DataBind();

            // Send appointment email if date is set
            if (customer.AppointmentSet.HasValue)
            {
                // Pass the already-converted UTC time and the original user input for display
                SendEmail(customer.Agent, customer.AppointmentSet.Value, Convert.ToDateTime(txtDate.Text));
            }
        }
        catch (Exception ex)
        {
            // Log error for debugging (AWS CloudWatch disabled - requires AWSSDK NuGet packages)
            System.Diagnostics.Debug.WriteLine(string.Format("Customer update failed: {0}", ex.Message));

            lblresult.Text = "Error updating customer record. Please try again.";
            lblresult.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void SendEmail(string agent, DateTime appointmentDateUtc, DateTime userDisplayDate)
    {
        // Use centralized agent email service instead of hard-coded if/else chains
        var agentInfo = AgentEmailService.GetAgentEmailInfo(agent);
        string recipientEmail = agentInfo.Email;
        string toagent = agentInfo.GmailAlias;

        // build subject and body with user's local time (not UTC) for readability
        string subject = string.Format("Appointment with {0} {1} on {2:MMMM d, yyyy h:mm tt}", txtfname.Text, txtlname.Text, userDisplayDate);
        string body = string.Format(
            "You have an Appointment with {0} {1}, Home Phone: {2}, Mobile Phone: {3}, Branch: {4}, Discharge: {5}, Age: {6}",
            txtfname.Text, txtlname.Text, txthphone.Text, txtmphone.Text, txtbranch.Text, txthdischarge.Text, txtages.Text);

        // build recipient lists for the shared email helper (primary + optional Gmail CC)
        var toEmails = new System.Collections.Generic.List<string>();
        var toNames = new System.Collections.Generic.List<string>();
        var ccEmails = new System.Collections.Generic.List<string>();
        var ccNames = new System.Collections.Generic.List<string>();

        toEmails.Add(recipientEmail);
        toNames.Add(txtagent.Text);

        // optionally add agent gmail alias as CC if it's different
        string agentGmail = agentInfo.GetGmailAddress();
        if (!string.Equals(agentGmail, recipientEmail, StringComparison.OrdinalIgnoreCase))
        {
            ccEmails.Add(agentGmail);
            ccNames.Add(txtagent.Text);
        }

        string location = txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text;

        string errorMessage;
        // Pass the UTC date for proper ICS calendar generation
        bool success = EmailHelper.SendAppointmentEmailWithIcs(
            "info@ashersolutions.com",
            "Asher Solutions",
            subject,
            body,
            appointmentDateUtc,  // UTC time for ICS file
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
}
