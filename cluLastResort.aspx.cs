<%@ Page Title="PLMS - Customer lookup" Language="C#" AutoEventWireup="true" CodeFile="cluLastResort.aspx.cs" Inherits="_cluLastResort" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Search for Last Resort by Agent</title>
<style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        z-index: 10000;
    }
    .ob_iCboIC, .ob_iDdlIC
    {
         z-index: 100002 !important;
    }
    .ob_iDdlICBC li
    {
    float: left;
    width: 125px;
    }
</style>
<script type="text/javascript">
        function Print() {
            var printWin = window.open('', '', 'left=0,top=0,width=1000,height=800,status=0');
            printWin.document.write(document.getElementById("<%=dvContent.ClientID %>").outerHTML);
            printWin.document.close();
            printWin.focus();
            printWin.print();
            printWin.close();
        }
        window.onerror = function (msg, url, num) {
            if (msg) {
                var isAddHandlerException = msg.indexOf('Handler was not added through the Sys.UI.DomEvent.addHandler method.') !== -1
                                            || msg.indexOf('b._events is undefined') !== -1;
                return isAddHandlerException; /* if it is an add handler exception then return true because we are not interested in it. */
            }
        }
        window.onload = function () {
            Obout.Interface.OboutCore.getLeft = function (element) {
                var position = $common.getLocation(element);
                return position.x;
            }

            Obout.Interface.OboutCore.getTop = function (element) {
                var position = $common.getLocation(element);
                return position.y;
            }
        }
    </script>
</head>
<body>
    <h2>Sales Lead Database</h2>
    <h3>Last Resort by Agent</h3>
    /* LoginView removed: authentication disabled in markup */
<form id="form1" runat="server">
<div>
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"/>
         <div>
            <div style="width: 275px; height: 66px">
            <asp:Label ID="Label1" runat="server" Width="100px" />
            <br />
            <br />
            <asp:DropDownList ID="DropDownList1" runat="server" ToolTip="Select the agent for this view">
                <asp:ListItem>Select One</asp:ListItem>
                <asp:ListItem>VPP Lead</asp:ListItem>
                <asp:ListItem>VPW Web Lead</asp:ListItem>
                <asp:ListItem>VPP Sharon Stangler</asp:ListItem>
                <asp:ListItem>VPP Richard Stangler</asp:ListItem>
                <asp:ListItem>Asher Lead</asp:ListItem>
                <asp:ListItem>Asher Sharon Stangler</asp:ListItem>
                <asp:ListItem>Asher Richard Stangler</asp:ListItem>
                <asp:ListItem>GC Lead</asp:ListItem>
                <asp:ListItem>GC Sharon Stangler</asp:ListItem>
                <asp:ListItem>GC Richard Stangler</asp:ListItem>
                <asp:ListItem>Stangler Lead</asp:ListItem>
                <asp:ListItem>Sharon Stangler</asp:ListItem>
                <asp:ListItem>Richard Stangler</asp:ListItem>
                <asp:ListItem>Mary Jo Hudson</asp:ListItem>
                <asp:ListItem>Amy Wallace</asp:ListItem>
                <asp:ListItem>CTS</asp:ListItem>
                <asp:ListItem>Web Lead</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Search" />
            </div>
         </div>
         <br />
</div>
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" 
    DataSourceID="SqlDataSource1" OnRowCreated="OnRowCreated">
    <RowStyle BackColor="#EFF3FB" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"
                 CommandName="Edit" ImageUrl="~/Edit.jpg" Text="Edit" Width="25" Height="25" OnClick="ImageButton1_Click" Visible="true" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                 CommandName="Delete" ImageUrl="~/delete.png" Text="Delete" Width="25" Height="25" OnClick="ImageButton2_Click" Enabled="true" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="False" />
        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" NullDisplayText='None' />
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText='None' />
        <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Date Created" SortExpression="Created" NullDisplayText=' ' />
        <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" NullDisplayText=' ' />
        <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" NullDisplayText=' ' />
        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" NullDisplayText=' ' />
        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" NullDisplayText=' ' />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" NullDisplayText=' ' />
        <asp:BoundField DataField="ZIP" HeaderText="ZIP" SortExpression="ZIP" NullDisplayText=' ' />
        <asp:BoundField DataField="HomePhone" HeaderText="HomePhone" SortExpression="HomePhone" NullDisplayText=' ' />
        <asp:BoundField DataField="MobilePhone" HeaderText="Mobile Phone" SortExpression="MobilePhone" NullDisplayText=' ' />
        <asp:BoundField DataField="EmailAddress" HeaderText="EMailAddress" SortExpression="EMailAddress" NullDisplayText=' ' />
        <asp:BoundField DataField="Branch" HeaderText="Branch" SortExpression="Branch" NullDisplayText='Unknown' />
        <asp:BoundField DataField="HDischarge" HeaderText="HDischarge" SortExpression="HDischarge" NullDisplayText='Unknown' />
        <asp:BoundField DataField="Ages" HeaderText="Ages" SortExpression="Ages" NullDisplayText=' ' />
        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" NullDisplayText=' ' />
        <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:g}" HeaderText="AppointmentSet" SortExpression="AppointmentSet" NullDisplayText=' ' />
        <asp:BoundField DataField="ApptSetter" HeaderText="Appt Setter" SortExpression="ApptSetter" NullDisplayText='None' />
        <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:g}" HeaderText="CTS Only" ReadOnly="True" Visible="True" NullDisplayText=' ' />
    </Columns>
</asp:GridView>
<asp:Label ID="lblresult" runat="server"/> 
<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup" BackgroundCssClass="modalBackground" />
<asp:Label runat="server" ID="LDate" Visible="False"/>
<asp:Label runat="server" ID="CDate" Visible="False"/>
<asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="700px" style="display:none">
<div id = "dvContent" runat = "server">
<table width="100%" style="border:Solid 3px #D55500; width:100%; height:100%" cellpadding="0" cellspacing="0">
<tr style="background-color:#D55500">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Customer Details</td>
</tr>
<tr>
    <td align="right" style=" width:45%">
    Record Id:
    </td>
    <td>
        <asp:Label ID="lblID" runat="server"></asp:Label>
    </td>
</tr>
<tr>
    <td align="right">
    First Name:
    </td>
    <td><asp:TextBox ID="txtfname" runat="server" ToolTip="Enter first name"/></td>
</tr>
<tr>
    <td align="right">
    Last Name:
    </td>
    <td>
        <asp:TextBox ID="txtlname" runat="server" ToolTip="Enter last name"/>
    </td>
</tr>
<tr>
     <td align="right">
     Address:
     </td>
     <td>
         <asp:TextBox ID="txtadd" runat="server" ToolTip="Enter an address"/>
     </td>
</tr>
<tr>
     <td align="right">
     City:
     </td>
     <td>
         <asp:TextBox ID="txtcity" runat="server" ToolTip="Enter a city name"/>
     </td>
</tr>
<tr>
     <td align="right">
     State:
     </td>
     <td>
         <asp:TextBox ID="txtstate" runat="server" ToolTip=" Enter a two letter state"/>
     </td>
</tr>
<tr>
     <td align="right">
     Zip:
     </td>
     <td>
          <asp:TextBox ID="txtzip" runat="server" ToolTip="Enter a valid zip code"/>
     </td>
</tr>
<tr>
     <td align="right">
     Home Phone:
     </td>
     <td>
          <asp:TextBox ID="txthphone" runat="server" CausesValidation="True" ToolTip="Enter a vaild 10 digit phone # with dashes"/>
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator1" runat="server" 
                ErrorMessage="Must be a valid Phone # ie. 555-555-1212 with dashes" 
                ControlToValidate="txthphone" 
                ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic"></asp:RegularExpressionValidator>
     </td>
</tr>
<tr>
     <td align="right">
     Mobile Phone:
     </td>
     <td>
          <asp:TextBox ID="txtmphone" runat="server" CausesValidation="True" ToolTip="Enter a valid 10 digit phone # with dashes" />
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator3" runat="server" 
                ErrorMessage="Must be a valid Phone # ie. 555-555-1212 with dashes" 
                ControlToValidate="txtmphone" 
                ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic"></asp:RegularExpressionValidator>
     </td>
</tr>
<tr>
     <td align="right">
     Email Address:
     </td>
     <td>
          <asp:TextBox ID="txtemail" runat="server" CausesValidation="True" ToolTip="Enter a valid email address"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                ErrorMessage="A valid email address must be entered ie. example@theveteranprogram.com" 
                ControlToValidate="txtemail" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
     </td>
</tr>
<tr>
     <td align="right">
     Branch:
     </td>
     <td>
          <obout:OboutDropDownList ID="txtbranch" runat="server" ToolTip="Select a branch of service if known" >
                <asp:ListItem>Air Force</asp:ListItem>
                <asp:ListItem>Army</asp:ListItem>
                <asp:ListItem>Coast Guard</asp:ListItem>
                <asp:ListItem>Marines</asp:ListItem>
                <asp:ListItem>Navy</asp:ListItem>
                <asp:ListItem>Unknown</asp:ListItem>
           </obout:OboutDropDownList>
     </td>
</tr>
<tr>
     <td align="right">
     Honorable Discharge:
     </td>
     <td>
          <obout:OboutDropDownList ID="txthdischarge" runat="server" ToolTip="Select discharge status if known" >
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem>No</asp:ListItem>
                <asp:ListItem>Unknown</asp:ListItem>
          </obout:OboutDropDownList>
     </td>
</tr>
<tr>
     <td align="right">
     Ages:
     </td>
     <td>
          <asp:TextBox ID="txtages" runat="server" ToolTip="Enter ages if known"/>
     </td>
</tr>
<tr>
     <td align="right">
     Notes:
     </td>
     <td>
          <asp:TextBox ID="txtnotes" runat="server" ToolTip="Enter notes here"/>
     </td>
</tr>
<tr>
     <td align="right">
     Appt Date &amp; Time:
     </td>
     <td>
         <asp:TextBox runat="server" ID="txtDate" ToolTip="Using the popup calendar select a date and time for the appointment"/>
         <obout:Calendar ID="Calendar1" runat="server"
             ShowTimeSelector="true"
             DateFormat="MM/dd/yyyy hh:mm"
             DatePickerMode="true"
             TextBoxId="txtDate" ShowSecondSelector="False" TimeSelectorType="DropDownList" DatePickerImagePath="../images/icon2.gif" >
         </obout:Calendar>
     </td>
</tr>
<tr>
     <td align="right">
     Agent:
     </td>
     <td>
          <obout:OboutDropDownList ID="txtagent" runat="server" ToolTip="Select the agent for this record" >
                <asp:ListItem>Select One</asp:ListItem>
                <asp:ListItem>GC Lead</asp:ListItem>
                <asp:ListItem>GC Sharon Stangler</asp:ListItem>
                <asp:ListItem>GC Richard Stangler</asp:ListItem>
                <asp:ListItem>Asher Lead</asp:ListItem>
                <asp:ListItem>Asher Sharon Stangler</asp:ListItem>
                <asp:ListItem>Asher Richard Stangler</asp:ListItem>
                <asp:ListItem>Stangler Lead</asp:ListItem>
                <asp:ListItem>Sharon Stangler</asp:ListItem>
                <asp:ListItem>Richard Stangler</asp:ListItem>
                <asp:ListItem>Mary Jo Hudson</asp:ListItem>
                <asp:ListItem>Amy Wallace</asp:ListItem>
                <asp:ListItem>CTS</asp:ListItem>
                <asp:ListItem>Web Lead</asp:ListItem>
            </obout:OboutDropDownList>
     </td>
</tr>
<tr>
     <td align="right">
     Appointment Setter:
     </td>
     <td>
         <obout:OboutDropDownList ID="txtapptset" runat="server" ToolTip="Select an appointment setter if applicable" >     
                <asp:ListItem>None</asp:ListItem>
                <asp:ListItem>Nancy Crocker</asp:ListItem>
                <asp:ListItem>Outside Agent</asp:ListItem>
         </obout:OboutDropDownList>
     </td>
</tr>
<tr>
     <td align="right">
     Lead Status:
     </td>
     <td>
                <obout:OboutDropDownList runat="server" ID="txtstatus" MenuWidth="500" 
		            DataSourceID="sds1" DataTextField="Status" DataValueField="Status"
		            AppendDataBoundItems="true" FolderStyle="styles/grand_gray/OboutDropDownList"
		            >
		            <asp:ListItem>Select a Status ...</asp:ListItem>
		        </obout:OboutDropDownList>
     </td>
</tr>
<tr>
    <td align="right">
    <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" OnClick="btnUpdate_Click"/>
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />                 
    <input type = "button" value  = "Print" onclick = "Print()" />
    </td>
</tr>
</table>
<asp:HyperLink runat="server" ID="lnkPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
</div>
</asp:Panel>
<asp:SqlDataSource ID="sds1" runat="server" SelectCommand="SELECT * FROM [StatusDDL] ORDER BY [Status] ASC" ConnectionString="<%$ ConnectionStrings:salespipeline %>" />
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
    OldValuesParameterFormatString="original_{0}"  
    SelectCommand="SELECT * FROM [Customers] WHERE ([Agent] = @Agent) AND [Status] = 'Last Resort' ORDER BY [Created] DESC">
    <SelectParameters>
        <asp:ControlParameter ControlID="DropDownList1" Name="Agent" PropertyName="Text" 
            Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
</form>
</body>
</html>
```
```csharp
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _cluLastResort : Page
{
    private readonly SqlConnection _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salespipeline"].ToString());

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
            e.Row.Attributes.Add("OnMouseOut", e.Row.RowIndex % 2 == 0 ? "this.style.backgroundColor = '#FFFFFF';" : "this.style.backgroundColor = '#EFF3FB';");
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
        txthdischarge.Text = gvRow.Cells[16].Text.Trim();
        txtages.Text = gvRow.Cells[17].Text.Trim();
        txtnotes.Text = gvRow.Cells[18].Text.Trim();
        txtDate.Text = gvRow.Cells[19].Text.Trim();
        txtapptset.Text = gvRow.Cells[20].Text.Trim();

        if (txtDate.Text.Equals(" "))
            txtDate.Text = "";

        ModalPopupExtender1.Show();
        GridView1.DataBind();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var imageButton = sender as ImageButton;
        var gvRow = (GridViewRow)imageButton.NamingContainer;
        var id = GridView1.DataKeys[gvRow.RowIndex].Value.ToString();

        using (var cmd = new SqlCommand("DELETE FROM Customers WHERE Id=@Id", _connection))
        {
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
            _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        lblresult.Text = "Customer Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateCustomerRecord();
        SendAppointmentEmail();
        ClearForm();
        GridView1.DataBind();
    }

    private void UpdateCustomerRecord()
    {
        using (var cmd = new SqlCommand("UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, Address=@Address, City=@City, State=@State, ZIP=@ZIP, HomePhone=@HomePhone, MobilePhone=@MobilePhone, EmailAddress=@EmailAddress, Branch=@Branch, HDischarge=@HDischarge, Ages=@Ages, Notes=@Notes, AppointmentSet=@AppointmentSet, Agent=@Agent, ApptSetter=@ApptSetter, Status=@Status, LDate=@LDate, CDate=@CDate WHERE Id=@Id", _connection))
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
            cmd.Parameters.AddWithValue("@AppointmentSet", string.IsNullOrWhiteSpace(txtDate.Text) ? (object)DBNull.Value : txtDate.Text);
            cmd.Parameters.AddWithValue("@Status", txtstatus.Text);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblID.Text));

            SetDateParameters(cmd);

            _connection.Open();
            cmd.ExecuteNonQuery();
            _connection.Close();
        }

        lblresult.Text = "Customer Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
    }

    private void SetDateParameters(SqlCommand cmd)
    {
        var sqlDateNull = SqlDateTime.Null;

        if (txtstatus.Text == "None" || txtstatus.Text == "New" || txtstatus.Text == "Active" || txtstatus.Text == "Sent Mail" || txtstatus.Text == "Mail - Info" || txtstatus.Text == "Mail - FUR" || txtstatus.Text == "Needs Phone Number" || txtstatus.Text == "Last Resort" || txtstatus.Text == "Sold" || txtstatus.Text == "Dead")
        {
            cmd.Parameters.AddWithValue("@CDate", sqlDateNull);
            cmd.Parameters.AddWithValue("@LDate", sqlDateNull);
        }
        else if (txtstatus.Text == "Left Message")
        {
            cmd.Parameters.AddWithValue("@LDate", DateTime.Today);
            cmd.Parameters.AddWithValue("@CDate", sqlDateNull);
        }
        else if (txtstatus.Text.StartsWith("Call Back"))
        {
            cmd.Parameters.AddWithValue("@CDate", DateTime.Today);
            cmd.Parameters.AddWithValue("@LDate", sqlDateNull);
        }
    }

    private void SendAppointmentEmail()
    {
        if (string.IsNullOrWhiteSpace(txtDate.Text))
            return;

        var msg = new MailMessage
        {
            From = new MailAddress("info@ashersolutions.com", "Sales Lead Appointment"),
            Subject = $"{txtfname.Text} {txtlname.Text} {txtadd.Text} {txtcity.Text} {txtstate.Text} {txtzip.Text} {txthphone.Text} {txtmphone.Text} {txtbranch.Text} {txtages.Text}",
            Body = $"You have an Appointment with {txtfname.Text} {txtlname.Text} {txthphone.Text} {txtmphone.Text} {txtbranch.Text} {txthdischarge.Text} {txtages.Text}"
        };

        var toAgent = GetAgentEmail(txtagent.Text);
        msg.To.Add(new MailAddress($"{toAgent}@gmail.com", txtagent.Text));

        var dt = Convert.ToDateTime(txtDate.Text);
        var str = new StringBuilder();
        str.AppendLine("BEGIN:VCALENDAR");
        str.AppendLine("PRODID:-//Schedule a Meeting");
        str.AppendLine("VERSION:2.0");
        str.AppendLine("METHOD:REQUEST");
        str.AppendLine("BEGIN:VEVENT");
        str.AppendLine($"DTSTART:{dt.ToUniversalTime():yyyyMMddTHHmmssZ}");
        str.AppendLine($"DTSTAMP:{DateTime.Now:yyyyMMddTHHmmssZ}");
        str.AppendLine($"DTEND:{dt.AddMinutes(180).ToUniversalTime():yyyyMMddTHHmmssZ}");
        str.AppendLine($"LOCATION: {txtadd.Text} {txtcity.Text} {txtstate.Text} {txtzip.Text}");
        str.AppendLine($"UID:{Guid.NewGuid()}");
        str.AppendLine($"DESCRIPTION:{msg.Body}");
        str.AppendLine($"X-ALT-DESC;FMTTYPE=text/html:{msg.Body}");
        str.AppendLine($"SUMMARY:{msg.Subject}");
        str.AppendLine($"ORGANIZER:MAILTO:{msg.From.Address}");
        str.AppendLine($"ATTENDEE;ROLE=OWNER;CN=\"{msg.To[0].DisplayName}\";RSVP=TRUE:mailto:{msg.To[0].Address}");
        str.AppendLine("BEGIN:VALARM");
        str.AppendLine("TRIGGER:-PT30M");
        str.AppendLine("ACTION:DISPLAY");
        str.AppendLine("DESCRIPTION:Reminder");
        str.AppendLine("END:VALARM");
        str.AppendLine("END:VEVENT");
        str.AppendLine("END:VCALENDAR");

        var smtpClient = new SmtpClient("smtp.ashersolutionsinc.com")
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("info@ashersolutionsinc.com", "L3tm31npl3@s3!#$"),
            Port = 587,
            EnableSsl = true
        };

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        var contentType = new System.Net.Mime.ContentType("text/calendar")
        {
            Parameters = { { "method", "REQUEST" }, { "name", "Meeting.ics" } }
        };

        var alternateView = AlternateView.CreateAlternateViewFromString(str.ToString(), contentType);
        msg.AlternateViews.Add(alternateView);
        smtpClient.Send(msg);
    }

    private string GetAgentEmail(string agent)
    {
        return agent switch
        {
            "GC Sharon Stangler" => "rsstangler1",
            "GC Richard Stangler" => "rjsstangler",
            "Asher Sharon Stangler" => "rsstangler1",
            "Asher Richard Stangler" => "rjsstangler",
            "Sharon Stangler" => "rsstangler1",
            "Richard Stangler" => "rjsstangler",
            "Mary Jo Hudson" => "maryjoveteransprogram",
            "Amy Wallace" => "awallacetvp",
            "Serenity" => "donna.haarer",
            _ => "cj.haarer"
        };
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
        txtbranch.Text = string.Empty;
        txthdischarge.Text = string.Empty;
        txtages.Text = string.Empty;
        txtnotes.Text = string.Empty;
        txtDate.Text = string.Empty;
        Calendar1.SelectedDate = DateTime.MinValue;
        txtagent.Text = string.Empty;
        txtapptset.Text = string.Empty;
        txtstatus.Text = string.Empty;

        ModalPopupExtender1.Hide();
    }
}