<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestCustomer.aspx.cs" Inherits="tvp_Customer" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Administrative Edit Page</title>
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
</head>
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
<body>
    <h2>Sales Lead Database</h2>

    <h3>Customer edit screen no filters</h3>
    <!-- LoginView removed: authentication disabled -->
        <div class="auth-removed">Authentication removed; role-based messages removed.</div>

    <div>
        <br />
    </div>
<form id="form1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" 
    DataSourceID="SqlDataSource1" Visible="true" OnRowCreated="OnRowCreated">
    <RowStyle BackColor="#EFF3FB" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False"
                 CommandName="Edit" ImageUrl="~/tvp/Edit.jpg" Width="25" Height="25" onclick="ImageButton1_Click" Visible="true" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>        
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                 CommandName="Delete" ImageUrl="~/tvp/delete.png" Width="25" Height="25" onclick="ImageButton2_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="False" />
        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" NullDisplayText=' ' />
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText=' ' />
        <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Date Created" SortExpression="Created" NullDisplayText=' ' />
        <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" NullDisplayText=' ' />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" NullDisplayText=' ' />
        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" NullDisplayText=' ' />
        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" NullDisplayText=' ' />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" NullDisplayText=' ' />
        <asp:BoundField DataField="ZIP" HeaderText="ZIP" SortExpression="ZIP" NullDisplayText=' ' />
        <asp:BoundField DataField="HomePhone" HeaderText="Home Phone" SortExpression="HomePhone" NullDisplayText=' ' />
        <asp:BoundField DataField="Branch" HeaderText="Branch" SortExpression="Branch" NullDisplayText=' ' />
        <asp:BoundField DataField="HDischarge" HeaderText="HDischarge" SortExpression="HDischarge" NullDisplayText=' ' />
        <asp:BoundField DataField="Ages" HeaderText="Ages" SortExpression="Ages" NullDisplayText=' ' />
        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" NullDisplayText=' ' />
        <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:f}" HeaderText="AppointmentSet" SortExpression="AppointmentSet" NullDisplayText=' ' />
        <asp:BoundField DataField="ApptSetter" HeaderText="Appt Setter" SortExpression="ApptSetter" NullDisplayText=' ' />
        <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:g}" HeaderText="CTS Only" ReadOnly="True" Visible="True" NullDisplayText=' ' />
    </Columns>
</asp:GridView>
<asp:HyperLink runat="server" ID="lnkBPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
<asp:Label ID="lblresult" runat="server"/>
<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup" BackGroundCSSClass="modalBackground" />
<asp:Label runat="server" ID="LDate" Visible="False"/>
<asp:Label runat="server" ID="CDate" Visible="False"/>
<asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="700px" style="display:none" >
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
             TextBoxId="txtDate" ShowSecondSelector="False" TimeSelectorType="HtmlList" DatePickerImagePath="../images/icon2.gif" DateMin="01/01/2011" >
         </obout:Calendar>
     </td>
</tr>
<tr>
     <td align="right">
     Agent:
     </td>
     <td>
          <obout:OboutDropDownList ID="txtagent" runat="server" ToolTip="Select the agent for this record" >
                <asp:ListItem>None</asp:ListItem>
                <asp:ListItem>Sharon Stangler</asp:ListItem>
                <asp:ListItem>Richard Stangler</asp:ListItem>
                <asp:ListItem>CTS</asp:ListItem>
                <asp:ListItem>Aneka Forte</asp:ListItem>
                <asp:ListItem>Darel Taylor</asp:ListItem>
                <asp:ListItem>Mary Jo Hudson</asp:ListItem>
                <asp:ListItem>Alan Poindexter</asp:ListItem>
                <asp:ListItem>Stangler Lead</asp:ListItem>
                <asp:ListItem>Serenity</asp:ListItem>
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
                <asp:ListItem>Patt Lesley</asp:ListItem>
                <asp:ListItem>Donna Haarer</asp:ListItem>
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
		            AppendDataBoundItems="true" FolderStyle="styles/obout/grand_gray/OboutDropDownList"
		            >
		            <asp:ListItem>Select a Status ...</asp:ListItem>
		        </obout:OboutDropDownList>
     </td>
</tr>
<tr>
    <td align="right">
        <div>
            <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" onclick="btnUpdate_Click"/>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            <input type = "button" value  = "Print" onclick = "Print()" />
        </div>
    </td>
</tr>
</table>
<asp:HyperLink runat="server" ID="lnkPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
</div>
</asp:Panel>
<asp:SqlDataSource ID="sds1" runat="server" SelectCommand="SELECT * FROM [StatusDDL] ORDER BY [Status] ASC" ConnectionString="<%$ ConnectionStrings:salespipeline %>" />
		    
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConflictDetection="CompareAllValues" 
    ConnectionString="<%$ ConnectionStrings:salespipeline2 %>" 
    DeleteCommand="DELETE FROM [Customers] WHERE [ID] = @original_ID AND (([Company] = @original_Company) OR ([Company] IS NULL AND @original_Company IS NULL)) AND (([LastName] = @original_LastName) OR ([LastName] IS NULL AND @original_LastName IS NULL)) AND (([FirstName] = @original_FirstName) OR ([FirstName] IS NULL AND @original_FirstName IS NULL)) AND (([EmailAddress] = @original_EmailAddress) OR ([EmailAddress] IS NULL AND @original_EmailAddress IS NULL)) AND (([Job Title] = @original_Job_Title) OR ([Job Title] IS NULL AND @original_Job_Title IS NULL)) AND (([HomePhone] = @original_HomePhone) OR ([HomePhone] IS NULL AND @original_HomePhone IS NULL)) AND (([Business Phone] = @original_Business_Phone) OR ([Business Phone] IS NULL AND @original_Business_Phone IS NULL)) AND (([MobilePhone] = @original_MobilePhone) OR ([MobilePhone] IS NULL AND @original_MobilePhone IS NULL)) AND (([Fax Number] = @original_Fax_Number) OR ([Fax Number] IS NULL AND @original_Fax_Number IS NULL)) AND (([Address] = @original_Address) OR ([Address] IS NULL AND @original_Address IS NULL)) AND (([City] = @original_City) OR ([City] IS NULL AND @original_City IS NULL)) AND (([State] = @original_State) OR ([State] IS NULL AND @original_State IS NULL)) AND (([ZIP] = @original_ZIP) OR ([ZIP] IS NULL AND @original_ZIP IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([Ages] = @original_Ages) OR ([Ages] IS NULL AND @original_Ages IS NULL)) AND (([Notes] = @original_Notes) OR ([Notes] IS NULL AND @original_Notes IS NULL)) AND (([Attachments] = @original_Attachments) OR ([Attachments] IS NULL AND @original_Attachments IS NULL)) AND (([AppointmentSet] = @original_AppointmentSet) OR ([AppointmentSet] IS NULL AND @original_AppointmentSet IS NULL)) AND (([Agent] = @original_Agent) OR ([Agent] IS NULL AND @original_Agent IS NULL)) AND (([ApptSetter] = @original_ApptSetter) OR ([ApptSetter] IS NULL AND @original_ApptSetter IS NULL)) AND (([upsize_ts] = @original_upsize_ts) OR ([upsize_ts] IS NULL AND @original_upsize_ts IS NULL)) AND (([Branch] = @original_Branch) OR ([Branch] IS NULL AND @original_Branch IS NULL)) AND (([HDischarge] = @original_HDischarge) OR ([HDischarge] IS NULL AND @original_HDischarge IS NULL)) AND (([ActiveDuty] = @original_ActiveDuty) OR ([ActiveDuty] IS NULL AND @original_ActiveDuty IS NULL)) AND (([Created] = @original_Created) OR ([Created] IS NULL AND @original_Created IS NULL)) AND (([LDate] = @original_LDate) OR ([LDate] IS NULL AND @original_LDate IS NULL)) AND (([CDate] = @original_CDate) OR ([CDate] IS NULL AND @original_CDate IS NULL))" 
    InsertCommand="INSERT INTO [Customers] ([Company], [LastName], [FirstName], [EmailAddress], [Job Title], [HomePhone], [Business Phone], [MobilePhone], [Fax Number], [Address], [City], [State], [ZIP], [Status], [Ages], [Notes], [Attachments], [AppointmentSet], [Agent], [ApptSetter], [upsize_ts], [Branch], [HDischarge], [ActiveDuty], [Created]) VALUES (@Company, @LastName, @FirstName, @EmailAddress, @Job_Title, @HomePhone, @Business_Phone, @MobilePhone, @Fax_Number, @Address, @City, @State, @ZIP, @Status, @Ages, @Notes, @Attachments, @AppointmentSet, @Agent, @ApptSetter, @upsize_ts, @Branch, @HDischarge, @ActiveDuty, @Created, @LDate, @CDate)" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT * FROM [Customers]" 
    UpdateCommand="UPDATE [Customers] SET [Company] = @Company, [LastName] = @LastName, [FirstName] = @FirstName, [EmailAddress] = @EmailAddress, [Job Title] = @Job_Title, [HomePhone] = @HomePhone, [Business Phone] = @Business_Phone, [MobilePhone] = @MobilePhone, [Fax Number] = @Fax_Number, [Address] = @Address, [City] = @City, [State] = @State, [ZIP] = @ZIP, [Status] = @Status, [Ages] = @Ages, [Notes] = @Notes, [Attachments] = @Attachments, [AppointmentSet] = @AppointmentSet, [Agent] = @Agent, [ApptSetter] = @ApptSetter, [upsize_ts] = @upsize_ts, [Branch] = @Branch, [HDischarge] = @HDischarge, [ActiveDuty] = @ActiveDuty, [Created] = @Created, [LDate] = @LDate, [CDate] = @CDate WHERE [ID] = @original_ID AND (([Company] = @original_Company) OR ([Company] IS NULL AND @original_Company IS NULL)) AND (([LastName] = @original_LastName) OR ([LastName] IS NULL AND @original_LastName IS NULL)) AND (([FirstName] = @original_FirstName) OR ([FirstName] IS NULL AND @original_FirstName IS NULL)) AND (([EmailAddress] = @original_EmailAddress) OR ([EmailAddress] IS NULL AND @original_EmailAddress IS NULL)) AND (([Job Title] = @original_Job_Title) OR ([Job Title] IS NULL AND @original_Job_Title IS NULL)) AND (([HomePhone] = @original_HomePhone) OR ([HomePhone] IS NULL AND @original_HomePhone IS NULL)) AND (([Business Phone] = @original_Business_Phone) OR ([Business Phone] IS NULL AND @original_Business_Phone IS NULL)) AND (([MobilePhone] = @original_MobilePhone) OR ([MobilePhone] IS NULL AND @original_MobilePhone IS NULL)) AND (([Fax Number] = @original_Fax_Number) OR ([Fax Number] IS NULL AND @original_Fax_Number IS NULL)) AND (([Address] = @original_Address) OR ([Address] IS NULL AND @original_Address IS NULL)) AND (([City] = @original_City) OR ([City] IS NULL AND @original_City IS NULL)) AND (([State] = @original_State) OR ([State] IS NULL AND @original_State IS NULL)) AND (([ZIP] = @original_ZIP) OR ([ZIP] IS NULL AND @original_ZIP IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL)) AND (([Ages] = @original_Ages) OR ([Ages] IS NULL AND @original_Ages IS NULL)) AND (([Notes] = @original_Notes) OR ([Notes] IS NULL AND @original_Notes IS NULL)) AND (([Attachments] = @original_Attachments) OR ([Attachments] IS NULL AND @original_Attachments IS NULL)) AND (([AppointmentSet] = @original_AppointmentSet) OR ([AppointmentSet] IS NULL AND @original_AppointmentSet IS NULL)) AND (([Agent] = @original_Agent) OR ([Agent] IS NULL AND @original_Agent IS NULL)) AND (([ApptSetter] = @original_ApptSetter) OR ([ApptSetter] IS NULL AND @original_ApptSetter IS NULL)) AND (([upsize_ts] = @original_upsize_ts) OR ([upsize_ts] IS NULL AND @original_upsize_ts IS NULL)) AND (([Branch] = @original_Branch) OR ([Branch] IS NULL AND @original_Branch IS NULL)) AND (([HDischarge] = @original_HDischarge) OR ([HDischarge] IS NULL AND @original_HDischarge IS NULL)) AND (([ActiveDuty] = @original_ActiveDuty) OR ([ActiveDuty] IS NULL AND @original_ActiveDuty IS NULL)) AND (([Created] = @original_Created) OR ([Created] IS NULL AND @original_Created IS NULL)) AND (([LDate] = @original_LDate) OR ([LDate] IS NULL AND @original_LDate IS NULL)) AND (([CDate] = @original_CDate) OR ([CDate] IS NULL AND @original_CDate IS NULL))">
    <DeleteParameters>
        <asp:Parameter Name="original_ID" Type="Int32" />
        <asp:Parameter Name="original_Company" Type="String" />
        <asp:Parameter Name="original_LastName" Type="String" />
        <asp:Parameter Name="original_FirstName" Type="String" />
        <asp:Parameter Name="original_EmailAddress" Type="String" />
        <asp:Parameter Name="original_Job_Title" Type="String" />
        <asp:Parameter Name="original_HomePhone" Type="String" />
        <asp:Parameter Name="original_Business_Phone" Type="String" />
        <asp:Parameter Name="original_MobilePhone" Type="String" />
        <asp:Parameter Name="original_Fax_Number" Type="String" />
        <asp:Parameter Name="original_Address" Type="String" />
        <asp:Parameter Name="original_City" Type="String" />
        <asp:Parameter Name="original_State" Type="String" />
        <asp:Parameter Name="original_ZIP" Type="String" />
        <asp:Parameter Name="original_Status" Type="String" />
        <asp:Parameter Name="original_Ages" Type="String" />
        <asp:Parameter Name="original_Notes" Type="String" />
        <asp:Parameter Name="original_Attachments" Type="String" />
        <asp:Parameter Name="original_AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="original_Agent" Type="String" />
        <asp:Parameter Name="original_ApptSetter" Type="String" />
        <asp:Parameter Name="original_upsize_ts" Type="Object" />
        <asp:Parameter Name="original_Branch" Type="String" />
        <asp:Parameter Name="original_HDischarge" Type="String" />
        <asp:Parameter Name="original_ActiveDuty" Type="String" />
        <asp:Parameter DbType="Date" Name="original_Created" />
        <asp:Parameter DbType="Date" Name="original_LDate" />
        <asp:Parameter DbType="Date" Name="original_CDate" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Company" Type="String" />
        <asp:Parameter Name="LastName" Type="String" />
        <asp:Parameter Name="FirstName" Type="String" />
        <asp:Parameter Name="EmailAddress" Type="String" />
        <asp:Parameter Name="Job_Title" Type="String" />
        <asp:Parameter Name="HomePhone" Type="String" />
        <asp:Parameter Name="Business_Phone" Type="String" />
        <asp:Parameter Name="MobilePhone" Type="String" />
        <asp:Parameter Name="Fax_Number" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="City" Type="String" />
        <asp:Parameter Name="State" Type="String" />
        <asp:Parameter Name="ZIP" Type="String" />
        <asp:Parameter Name="Status" Type="String" />
        <asp:Parameter Name="Ages" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Attachments" Type="String" />
        <asp:Parameter Name="AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="Agent" Type="String" />
        <asp:Parameter Name="ApptSetter" Type="String" />
        <asp:Parameter Name="upsize_ts" Type="Object" />
        <asp:Parameter Name="Branch" Type="String" />
        <asp:Parameter Name="HDischarge" Type="String" />
        <asp:Parameter Name="ActiveDuty" Type="String" />
        <asp:Parameter DbType="Date" Name="Created" />
        <asp:Parameter DbType="Date" Name="LDate" />
        <asp:Parameter DbType="Date" Name="CDate" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Company" Type="String" />
        <asp:Parameter Name="LastName" Type="String" />
        <asp:Parameter Name="FirstName" Type="String" />
        <asp:Parameter Name="EmailAddress" Type="String" />
        <asp:Parameter Name="Job_Title" Type="String" />
        <asp:Parameter Name="HomePhone" Type="String" />
        <asp:Parameter Name="Business_Phone" Type="String" />
        <asp:Parameter Name="MobilePhone" Type="String" />
        <asp:Parameter Name="Fax_Number" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="City" Type="String" />
        <asp:Parameter Name="State" Type="String" />
        <asp:Parameter Name="ZIP" Type="String" />
        <asp:Parameter Name="Status" Type="String" />
        <asp:Parameter Name="Ages" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Attachments" Type="String" />
        <asp:Parameter Name="AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="Agent" Type="String" />
        <asp:Parameter Name="ApptSetter" Type="String" />
        <asp:Parameter Name="upsize_ts" Type="Object" />
        <asp:Parameter Name="Branch" Type="String" />
        <asp:Parameter Name="HDischarge" Type="String" />
        <asp:Parameter Name="ActiveDuty" Type="String" />
        <asp:Parameter DbType="Date" Name="Created" />
        <asp:Parameter DbType="Date" Name="LDate" />
        <asp:Parameter DbType="Date" Name="CDate" />
        <asp:Parameter Name="original_ID" Type="Int32" />
        <asp:Parameter Name="original_Company" Type="String" />
        <asp:Parameter Name="original_LastName" Type="String" />
        <asp:Parameter Name="original_FirstName" Type="String" />
        <asp:Parameter Name="original_EmailAddress" Type="String" />
        <asp:Parameter Name="original_Job_Title" Type="String" />
        <asp:Parameter Name="original_HomePhone" Type="String" />
        <asp:Parameter Name="original_Business_Phone" Type="String" />
        <asp:Parameter Name="original_MobilePhone" Type="String" />
        <asp:Parameter Name="original_Fax_Number" Type="String" />
        <asp:Parameter Name="original_Address" Type="String" />
        <asp:Parameter Name="original_City" Type="String" />
        <asp:Parameter Name="original_State" Type="String" />
        <asp:Parameter Name="original_ZIP" Type="String" />
        <asp:Parameter Name="original_Status" Type="String" />
        <asp:Parameter Name="original_Ages" Type="String" />
        <asp:Parameter Name="original_Notes" Type="String" />
        <asp:Parameter Name="original_Attachments" Type="String" />
        <asp:Parameter Name="original_AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="original_Agent" Type="String" />
        <asp:Parameter Name="original_ApptSetter" Type="String" />
        <asp:Parameter Name="original_upsize_ts" Type="Object" />
        <asp:Parameter Name="original_Branch" Type="String" />
        <asp:Parameter Name="original_HDischarge" Type="String" />
        <asp:Parameter Name="original_ActiveDuty" Type="String" />
        <asp:Parameter DbType="Date" Name="original_Created" />
        <asp:Parameter DbType="Date" Name="original_LDate" />
        <asp:Parameter DbType="Date" Name="original_CDate" />
    </UpdateParameters>
</asp:SqlDataSource>
</form>
</body>
</html>
