<%@ Page Title="PLMS - Leads needing Mail Info" Language="C#" AutoEventWireup="true" CodeFile="minfo.aspx.cs" Inherits="minfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Administrative Edit Page - Mail</title>
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

    <h3>Leads needing mail service</h3>
    <!-- LoginView removed: authentication disabled -->
        <div class="auth-removed">Authentication removed; role-based messages removed.</div>
<form id="form1" runat="server">
<div>
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release"/>
    <div style="width: 275px; height: 66px">
           
            <asp:Label ID="Label1" runat="server" Width="100px" />
            <br />
            <br />
            <asp:DropDownList ID="DropDownList1" runat="server" ToolTip="Select the agent for this view">
                <asp:ListItem>Select Agent</asp:ListItem>
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

            </asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Search" />
            <br />
    </div>
</div>
<br />
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" 
    DataSourceID="SqlDataSource1" Visible="true" OnRowCreated="OnRowCreated" >
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
                 CommandName="Delete" ImageUrl="~/delete.png" Text="Delete" Width="25" Height="25" onclick="ImageButton2_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
        <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" NullDisplayText=' ' />
        <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" NullDisplayText=' ' />
        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" NullDisplayText=' ' />
        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" NullDisplayText=' ' />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" NullDisplayText=' ' />
        <asp:BoundField DataField="ZIP" HeaderText="ZIP" SortExpression="ZIP" NullDisplayText=' ' />
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText=' ' />
    </Columns>
</asp:GridView>
<asp:Label ID="lblresult" runat="server"/>
            <br />
        <asp:Button ID="btnExportWord" runat="server" Text="ExportToWord" OnClick="btnExportWord_Click" />
        &nbsp;
        <asp:Button ID="btnExportExcel" runat="server" Text="ExportToExcel" OnClick="btnExportExcel_Click" />
        &nbsp;
        <asp:Button ID="btnExportPDF" runat="server" Text="ExportToPDF" OnClick="btnExportPDF_Click" />
         &nbsp;
        <asp:Button ID="Button2" runat="server" Text="ExportToCSV" OnClick="btnExportCSV_Click" />
<asp:Label ID="Label2" runat="server"/>
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
    <td align="right">
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
    SelectCommand="SELECT * FROM [Customers] WHERE ([Agent] = @Agent) AND [Status] = 'Mail' OR ([Agent] = @Agent) AND [Status] = 'Mail - Info' ORDER BY [Created] DESC">
    <SelectParameters>
        <asp:ControlParameter ControlID="DropDownList1" Name="Agent" PropertyName="Text" 
            Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
</form>
</body>
</html>
