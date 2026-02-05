<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Funhomes.aspx.cs" Inherits="tvp_Customer" %>

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
</style>
    <script type="text/javascript">
        function Print()
        {
            var printWin = window.open('', '', 'left=0,top=0,width=1000,height=800,status=0');
            printWin.document.write(document.getElementById("<%=dvContent.ClientID %>").outerHTML);
            printWin.document.close();
            printWin.focus();
            printWin.print();
            printWin.close();
        }
    </script> 
</head>
<body>
    <h2>Funeral Home Database</h2>
    <p>
    <h3>Funeral Home edit screen no filters</h3>
    <!-- LoginView removed: authentication disabled -->
        <div class="auth-removed">Authentication removed; role-based messages removed.</div>
    </p>
<form id="form1" runat="server">
<asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
</asp:ToolkitScriptManager>
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
                 CommandName="Edit" ImageUrl="~/tvp/Edit.jpg" Text="Edit" Width="25" Height="25" onclick="ImageButton1_Click" Visible="true" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>        
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                 CommandName="Delete" ImageUrl="~/tvp/delete.png" Text="Delete" Width="25" Height="25" onclick="ImageButton2_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="False" />
        <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" NullDisplayText=' ' />
        <asp:BoundField DataField="Director" HeaderText="Director" SortExpression="Status" NullDisplayText=' ' />
        <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact" NullDisplayText=' ' />
        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" NullDisplayText=' ' />
        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" NullDisplayText=' ' />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" NullDisplayText=' ' />
        <asp:BoundField DataField="ZIP" HeaderText="ZIP" SortExpression="ZIP" NullDisplayText=' ' />
        <asp:BoundField DataField="BusPhone" HeaderText="Business Phone" SortExpression="BusPhone" NullDisplayText=' ' />
        <asp:BoundField DataField="MobilePhone" HeaderText="Business Cell Phone" SortExpression="MobilePhone" NullDisplayText=' ' />
        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" NullDisplayText=' ' />
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText=' ' />
        <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Date Created" SortExpression="Created" NullDisplayText=' ' />
        <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:f}" HeaderText="Appointment Set" SortExpression="AppointmentSet" NullDisplayText=' ' />
        <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:g}" HeaderText="CTS Only" ReadOnly="True" Visible="True" NullDisplayText=' ' />
     </Columns>
</asp:GridView>
<asp:HyperLink runat="server" ID="lnkBPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
<asp:Label ID="lblresult" runat="server"/>
<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground" >
</asp:ModalPopupExtender>
<asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="600px" 
        Width="700px" style="display:none">
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
        <asp:TextBox runat="server" ID="LDate" Visible="False"/>
        <asp:TextBox runat="server" ID="CDate" Visible="False"/>
        <asp:Label ID="lblID" runat="server"></asp:Label>
    </td>
</tr>
<tr>
    <td align="right">
    Funeral Home: 
    </td>
    <td><asp:TextBox ID="txtcompany" runat="server" ToolTip="Enter Funeral home name"/></td>
</tr>
<tr>
    <td align="right">
    Director:
    </td>
    <td>
        <asp:TextBox ID="txtdirector" runat="server" ToolTip="Enter Director name"/>
    </td>
</tr>
<tr>
    <td align="right">
    Contact:
    </td>
    <td>
        <asp:TextBox ID="txtcontact" runat="server" ToolTip="Enter Contact name"/>
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
     Business Phone:
     </td>
     <td>
          <asp:TextBox ID="txtbphone" runat="server" CausesValidation="True" ToolTip="Enter a vaild 10 digit phone # with dashes"/>
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator1" runat="server" 
                ErrorMessage="Must be a valid Phone # ie. 555-555-1212 with dashes" 
                ControlToValidate="txtbphone" 
                ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic"></asp:RegularExpressionValidator>
     </td>
</tr>
<tr>
     <td align="right">
     Mobile Phone:
     </td>
     <td>
          <asp:TextBox ID="txtmphone" runat="server" CausesValidation="True" ToolTip="Enter a vaild 10 digit phone # with dashes"/>
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
     Status:
     </td>
     <td>
          <asp:DropDownList ID="txtstatus" runat="server" ToolTip="Select a status for this funeral home">
                <asp:ListItem>Preferred</asp:ListItem>
                <asp:ListItem>Active research</asp:ListItem>
                <asp:ListItem>Call Back</asp:ListItem>
                <asp:ListItem>Left Message</asp:ListItem>
                <asp:ListItem>No Interest</asp:ListItem>
                <asp:ListItem>Dead</asp:ListItem>
          </asp:DropDownList>
     </td>
</tr>
<tr>
    <td align="right">
    <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" onclick="btnUpdate_Click"/>
    <asp:Button ID="btnSendMail" runat="server" Text="Send Appointment" onclick="Sendmail_With_IcsAttachment" CommandName="Mail" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    <input type = "button" value  = "Print" onclick = "Print()" />
    </td>
</tr>
</table>
<asp:HyperLink runat="server" ID="lnkPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
</div>
</asp:Panel>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT * FROM [Funhomes]" ConflictDetection="CompareAllValues" 
    DeleteCommand="DELETE FROM [Funhomes] WHERE [ID] = @original_ID AND (([Company] = @original_Company) OR ([Company] IS NULL AND @original_Company IS NULL)) AND (([LastName] = @original_LastName) OR ([LastName] IS NULL AND @original_LastName IS NULL)) AND (([FirstName] = @original_FirstName) OR ([FirstName] IS NULL AND @original_FirstName IS NULL)) AND (([EmailAddress] = @original_EmailAddress) OR ([EmailAddress] IS NULL AND @original_EmailAddress IS NULL)) AND (([HomePhone] = @original_HomePhone) OR ([HomePhone] IS NULL AND @original_HomePhone IS NULL)) AND (([Business Phone] = @original_Business_Phone) OR ([Business Phone] IS NULL AND @original_Business_Phone IS NULL)) AND (([MobilePhone] = @original_MobilePhone) OR ([MobilePhone] IS NULL AND @original_MobilePhone IS NULL)) AND (([Fax Number] = @original_Fax_Number) OR ([Fax Number] IS NULL AND @original_Fax_Number IS NULL)) AND (([Address] = @original_Address) OR ([Address] IS NULL AND @original_Address IS NULL)) AND (([City] = @original_City) OR ([City] IS NULL AND @original_City IS NULL)) AND (([State] = @original_State) OR ([State] IS NULL AND @original_State IS NULL)) AND (([ZIP] = @original_ZIP) OR ([ZIP] IS NULL AND @original_ZIP IS NULL)) AND (([Notes] = @original_Notes) OR ([Notes] IS NULL AND @original_Notes IS NULL)) AND (([Attachments] = @original_Attachments) OR ([Attachments] IS NULL AND @original_Attachments IS NULL)) AND (([AppointmentSet] = @original_AppointmentSet) OR ([AppointmentSet] IS NULL AND @original_AppointmentSet IS NULL)) AND (([Director] = @original_Director) OR ([Director] IS NULL AND @original_Director IS NULL)) AND (([upsize_ts] = @original_upsize_ts) OR ([upsize_ts] IS NULL AND @original_upsize_ts IS NULL)) AND (([Created] = @original_Created) OR ([Created] IS NULL AND @original_Created IS NULL)) AND (([Contact] = @original_Contact) OR ([Contact] IS NULL AND @original_Contact IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))" InsertCommand="INSERT INTO [Funhomes] ([ID], [Company], [LastName], [FirstName], [EmailAddress], [HomePhone], [Business Phone], [MobilePhone], [Fax Number], [Address], [City], [State], [ZIP], [Notes], [Attachments], [AppointmentSet], [Director], [upsize_ts], [Created], [Contact], [Status]) VALUES (@ID, @Company, @LastName, @FirstName, @EmailAddress, @HomePhone, @Business_Phone, @MobilePhone, @Fax_Number, @Address, @City, @State, @ZIP, @Notes, @Attachments, @AppointmentSet, @Director, @upsize_ts, @Created, @Contact, @Status)" UpdateCommand="UPDATE [Funhomes] SET [Company] = @Company, [LastName] = @LastName, [FirstName] = @FirstName, [EmailAddress] = @EmailAddress, [HomePhone] = @HomePhone, [Business Phone] = @Business_Phone, [MobilePhone] = @MobilePhone, [Fax Number] = @Fax_Number, [Address] = @Address, [City] = @City, [State] = @State, [ZIP] = @ZIP, [Notes] = @Notes, [Attachments] = @Attachments, [AppointmentSet] = @AppointmentSet, [Director] = @Director, [upsize_ts] = @upsize_ts, [Created] = @Created, [Contact] = @Contact, [Status] = @Status WHERE [ID] = @original_ID AND (([Company] = @original_Company) OR ([Company] IS NULL AND @original_Company IS NULL)) AND (([LastName] = @original_LastName) OR ([LastName] IS NULL AND @original_LastName IS NULL)) AND (([FirstName] = @original_FirstName) OR ([FirstName] IS NULL AND @original_FirstName IS NULL)) AND (([EmailAddress] = @original_EmailAddress) OR ([EmailAddress] IS NULL AND @original_EmailAddress IS NULL)) AND (([HomePhone] = @original_HomePhone) OR ([HomePhone] IS NULL AND @original_HomePhone IS NULL)) AND (([Business Phone] = @original_Business_Phone) OR ([Business Phone] IS NULL AND @original_Business_Phone IS NULL)) AND (([MobilePhone] = @original_MobilePhone) OR ([MobilePhone] IS NULL AND @original_MobilePhone IS NULL)) AND (([Fax Number] = @original_Fax_Number) OR ([Fax Number] IS NULL AND @original_Fax_Number IS NULL)) AND (([Address] = @original_Address) OR ([Address] IS NULL AND @original_Address IS NULL)) AND (([City] = @original_City) OR ([City] IS NULL AND @original_City IS NULL)) AND (([State] = @original_State) OR ([State] IS NULL AND @original_State IS NULL)) AND (([ZIP] = @original_ZIP) OR ([ZIP] IS NULL AND @original_ZIP IS NULL)) AND (([Notes] = @original_Notes) OR ([Notes] IS NULL AND @original_Notes IS NULL)) AND (([Attachments] = @original_Attachments) OR ([Attachments] IS NULL AND @original_Attachments IS NULL)) AND (([AppointmentSet] = @original_AppointmentSet) OR ([AppointmentSet] IS NULL AND @original_AppointmentSet IS NULL)) AND (([Director] = @original_Director) OR ([Director] IS NULL AND @original_Director IS NULL)) AND (([upsize_ts] = @original_upsize_ts) OR ([upsize_ts] IS NULL AND @original_upsize_ts IS NULL)) AND (([Created] = @original_Created) OR ([Created] IS NULL AND @original_Created IS NULL)) AND (([Contact] = @original_Contact) OR ([Contact] IS NULL AND @original_Contact IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))">
    <DeleteParameters>
        <asp:Parameter Name="original_ID" Type="Int32" />
        <asp:Parameter Name="original_Company" Type="String" />
        <asp:Parameter Name="original_LastName" Type="String" />
        <asp:Parameter Name="original_FirstName" Type="String" />
        <asp:Parameter Name="original_EmailAddress" Type="String" />
        <asp:Parameter Name="original_HomePhone" Type="String" />
        <asp:Parameter Name="original_Business_Phone" Type="String" />
        <asp:Parameter Name="original_MobilePhone" Type="String" />
        <asp:Parameter Name="original_Fax_Number" Type="String" />
        <asp:Parameter Name="original_Address" Type="String" />
        <asp:Parameter Name="original_City" Type="String" />
        <asp:Parameter Name="original_State" Type="String" />
        <asp:Parameter Name="original_ZIP" Type="String" />
        <asp:Parameter Name="original_Notes" Type="String" />
        <asp:Parameter Name="original_Attachments" Type="String" />
        <asp:Parameter Name="original_AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="original_Director" Type="String" />
        <asp:Parameter Name="original_upsize_ts" Type="Object" />
        <asp:Parameter DbType="Date" Name="original_Created" />
        <asp:Parameter Name="original_Contact" Type="String" />
        <asp:Parameter Name="original_Status" Type="String" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="ID" Type="Int32" />
        <asp:Parameter Name="Company" Type="String" />
        <asp:Parameter Name="LastName" Type="String" />
        <asp:Parameter Name="FirstName" Type="String" />
        <asp:Parameter Name="EmailAddress" Type="String" />
        <asp:Parameter Name="HomePhone" Type="String" />
        <asp:Parameter Name="Business_Phone" Type="String" />
        <asp:Parameter Name="MobilePhone" Type="String" />
        <asp:Parameter Name="Fax_Number" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="City" Type="String" />
        <asp:Parameter Name="State" Type="String" />
        <asp:Parameter Name="ZIP" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Attachments" Type="String" />
        <asp:Parameter Name="AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="Director" Type="String" />
        <asp:Parameter Name="upsize_ts" Type="Object" />
        <asp:Parameter DbType="Date" Name="Created" />
        <asp:Parameter Name="Contact" Type="String" />
        <asp:Parameter Name="Status" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Company" Type="String" />
        <asp:Parameter Name="LastName" Type="String" />
        <asp:Parameter Name="FirstName" Type="String" />
        <asp:Parameter Name="EmailAddress" Type="String" />
        <asp:Parameter Name="HomePhone" Type="String" />
        <asp:Parameter Name="Business_Phone" Type="String" />
        <asp:Parameter Name="MobilePhone" Type="String" />
        <asp:Parameter Name="Fax_Number" Type="String" />
        <asp:Parameter Name="Address" Type="String" />
        <asp:Parameter Name="City" Type="String" />
        <asp:Parameter Name="State" Type="String" />
        <asp:Parameter Name="ZIP" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Attachments" Type="String" />
        <asp:Parameter Name="AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="Director" Type="String" />
        <asp:Parameter Name="upsize_ts" Type="Object" />
        <asp:Parameter DbType="Date" Name="Created" />
        <asp:Parameter Name="Contact" Type="String" />
        <asp:Parameter Name="Status" Type="String" />
        <asp:Parameter Name="original_ID" Type="Int32" />
        <asp:Parameter Name="original_Company" Type="String" />
        <asp:Parameter Name="original_LastName" Type="String" />
        <asp:Parameter Name="original_FirstName" Type="String" />
        <asp:Parameter Name="original_EmailAddress" Type="String" />
        <asp:Parameter Name="original_HomePhone" Type="String" />
        <asp:Parameter Name="original_Business_Phone" Type="String" />
        <asp:Parameter Name="original_MobilePhone" Type="String" />
        <asp:Parameter Name="original_Fax_Number" Type="String" />
        <asp:Parameter Name="original_Address" Type="String" />
        <asp:Parameter Name="original_City" Type="String" />
        <asp:Parameter Name="original_State" Type="String" />
        <asp:Parameter Name="original_ZIP" Type="String" />
        <asp:Parameter Name="original_Notes" Type="String" />
        <asp:Parameter Name="original_Attachments" Type="String" />
        <asp:Parameter Name="original_AppointmentSet" Type="DateTime" />
        <asp:Parameter Name="original_Director" Type="String" />
        <asp:Parameter Name="original_upsize_ts" Type="Object" />
        <asp:Parameter DbType="Date" Name="original_Created" />
        <asp:Parameter Name="original_Contact" Type="String" />
        <asp:Parameter Name="original_Status" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>
</form>
</body>
</html>
