<%@ Page Title="PLMS - Customer lookup" Language="C#" AutoEventWireup="true" CodeFile="Copy of custlookuplnhp.aspx.cs" Inherits="custlookuplnhp" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Customer Search by Last Name or Phone Number</title>
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
    <h3>Sales Lead Database</h3>

    <h3>Customer Search by Last Name or Phone number</h3>
    <!-- LoginView removed: authentication disabled -->
        <div class="auth-removed">Authentication removed; role-based messages removed.</div>
<form id="form1" runat="server">
<div>
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release"/>
         <div style="width: 750px; height: 66px">
          <asp:Label ID="Label1" runat="server" Width="100px"></asp:Label>
          <br />
          <br />
           Please enter the Customers Last Name or Home Phone Number
          <br />
          <asp:TextBox ID="TextBox1" runat="server" ToolTip="Select the Customer last name or phone number to look up" />
          <br />
          <br />
          <asp:Button ID="Button1" runat="server" Text="Search" />
          <br />
         </div>
         <br />
</div>
         <br />
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
                 CommandName="Edit" ImageUrl="~/Edit.jpg" Text="Edit" Width="25" Height="25" onclick="ImageButton1_Click" Visible="true" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>        
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                 CommandName="Delete" ImageUrl="~/delete.png" Text="Delete" Width="25" Height="25" onclick="ImageButton2_Click" Enabled="true" />
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
<table style="border:Solid 3px #D55500; width:100%; height:100%" >
<tr style="background-color:#D55500">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" />
</tr>
<tr>
    <td style=" width:45%">
    Record Id:
    </td>
    <td>
        <asp:Label ID="lblID" runat="server"></asp:Label>
    </td>
</tr>
<tr>
    <td>
    First Name: 
    </td>
    <td><asp:TextBox ID="txtfname" runat="server" ToolTip="Enter first name"/></td>
</tr>
<tr>
    <td>
    Last Name:
    </td>
    <td>
        <asp:TextBox ID="txtlname" runat="server" ToolTip="Enter last name"/>
    </td>
</tr>
<tr>
     <td>
     Address:
     </td>
     <td>
         <asp:TextBox ID="txtadd" runat="server" ToolTip="Enter an address"/>
     </td>
</tr>
<tr>
     <td>
     City:
     </td>
     <td>
         <asp:TextBox ID="txtcity" runat="server" ToolTip="Enter a city name"/>
     </td>
</tr>
<tr>
     <td>
     State:
     </td>
     <td>
         <asp:TextBox ID="txtstate" runat="server" ToolTip=" Enter a two letter state"/>
     </td>
</tr>
<tr>
     <td>
     Zip:
     </td>
     <td>
          <asp:TextBox ID="txtzip" runat="server" ToolTip="Enter a valid zip code"/>
     </td>
</tr>
<tr>
     <td>
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
     <td>
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
     <td>
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
     <td>
     Branch:
     </td>
     <td>
          <obout:OboutDropDownList ID="txtbranch" runat="server" ToolTip="Select a branch of service if known" >
                <asp:ListItem>Air Force</asp:ListItem>
                <asp:ListItem>Army</asp:ListItem>
                <asp:ListItem>Coast Guard</asp:ListItem>
                <asp:ListItem>Marines</asp:ListItem>
                <asp:ListItem>Navy</asp:ListItem>
                <asp:ListItem>Space Force</asp:ListItem>
                <asp:ListItem>Unknown</asp:ListItem>
           </obout:OboutDropDownList>
     </td>
</tr>
<tr>
     <td>
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
     <td>
     Ages:
     </td>
     <td>
          <asp:TextBox ID="txtages" runat="server" ToolTip="Enter ages if known"/>
     </td>
</tr>
<tr>
     <td>
     Notes:
     </td>
     <td>
          <asp:TextBox ID="txtnotes" runat="server" ToolTip="Enter notes here"/>
     </td>
</tr>
<tr>
     <td>
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
     <td>
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
     <td>
     Appointment Setter:
     </td>
     <td>
         <obout:OboutDropDownList ID="txtapptset" runat="server" ToolTip="Select an appointment setter if applicable" Value="None">     
                <asp:ListItem>None</asp:ListItem>
                <asp:ListItem>Michelle McDOnogh</asp:ListItem>
                <asp:ListItem>Outside Agent</asp:ListItem>
          </obout:OboutDropDownList>
     </td>
</tr>
<tr>
     <td>
     Lead Status:
     </td>
     <td>
                <obout:OboutDropDownList ID="txtstatus" MenuWidth="500" DataSourceID="sds1" DataTextField="Status" DataValueField="Status"
		            AppendDataBoundItems="true" runat="server" >
		            <asp:ListItem>Select a Status ...</asp:ListItem>
		        </obout:OboutDropDownList>
     </td>
</tr>
<tr>
    <td>
    <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" onclick="btnUpdate_Click"/>
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
    ConflictDetection="CompareAllValues"
    SelectCommand="SELECT * FROM [Customers] WHERE ([LastName] = @LastName) OR ([HomePhone] = @HomePhone) OR ([MobilePhone] = @MobilePhone)">
    <SelectParameters>
        <asp:ControlParameter ControlID="TextBox1" Name="LastName" PropertyName="Text" 
            Type="String" />
        <asp:ControlParameter ControlID="TextBox1" Name="HomePhone" PropertyName="Text" 
            Type="String" />
        <asp:ControlParameter ControlID="TextBox1" Name="MobilePhone" PropertyName="Text" 
            Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
</form>
</body>
</html>
