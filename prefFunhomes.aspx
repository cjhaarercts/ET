<%@ Page Language="C#" AutoEventWireup="true" CodeFile="prefFunhomes.aspx.cs" Inherits="tvp_Customer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Funeral Home Database Page</title>
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

    <h3>Funeral Home edit screen no filters</h3>

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
                 CommandName="Edit" ImageUrl="Edit.jpg" Text="Edit" Width="25" Height="25" onclick="ImageButton1_Click" Visible="true" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>        
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                 CommandName="Delete" ImageUrl="delete.png" Text="Delete" Width="25" Height="25" onclick="ImageButton2_Click" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="False" />
        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" NullDisplayText=' ' />
        <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" NullDisplayText=' ' />
        <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" NullDisplayText=' ' />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" NullDisplayText=' ' />
        <asp:BoundField DataField="Zip" HeaderText="Zip" SortExpression="Zip" NullDisplayText=' ' />
        <asp:BoundField DataField="Phone" HeaderText="Business Phone" SortExpression="BusPhone" NullDisplayText=' ' />
        <asp:BoundField DataField="ChapSvc" HeaderText="Chapel Service" SortExpression="Chapel Service" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="TradSvc" HeaderText="Traditional Service" SortExpression="Traditional Service" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="SameDaySvc" HeaderText="Same Day Service" SortExpression="Same Day Service" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="GraveSvc" HeaderText="Gravesite Service" SortExpression="Gravesite Service" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="TradCrem" HeaderText="Traditional Cremation" SortExpression="Traditional Cremation" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="SameDayCrem" HeaderText="Same Day Cremation" SortExpression="Same Day Creamation" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="MemCrem" HeaderText="Memorial Cremation" SortExpression="Memorial Cremation" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="GraveCrem" HeaderText="Gravesite Cremation" SortExpression="Gravesite Cremation" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="DirectCrem" HeaderText="Direct Cremation" SortExpression="Direct Cremation" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Limo" HeaderText="Limo" SortExpression="Limo" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="MEscort" HeaderText="Motor Escort" SortExpression="Motor Escort" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="AltContainer" HeaderText="Cremation Container" SortExpression="Alternate Container" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Clergy" HeaderText="Clergy" SortExpression="Clergy" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="MemPkg" HeaderText="Memorial Package" SortExpression="Memorial Package" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Video" HeaderText="Video" SortExpression="Video" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Shroud" HeaderText="Shroud" SortExpression="Shroud" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="ShroudMuslin" HeaderText="Shroud Muslin" SortExpression="Shroud Muslim" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Tahara" HeaderText="Tahara" SortExpression="Tahara" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Shomer" HeaderText="Shomer" SortExpression="Shomer" Visible="True" NullDisplayText=' ' />
        <asp:BoundField DataField="Gratuities" HeaderText="Gratuities" SortExpression="Gratuities" Visible="True" NullDisplayText=' ' />
     </Columns>
</asp:GridView>
<asp:Label ID="lblresult" runat="server"/>
<asp:Button ID="btnShowPopup" runat="server" style="display:none" />
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
CancelControlID="btnCancel" BackgroundCssClass="modalBackground" >
</asp:ModalPopupExtender>
<asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="700px" 
        Width="700px" style="display:none">
<div id = "dvContent" runat = "server">
<table width="100%" style="border:Solid 3px #D55500; width:100%; height:100%" cellpadding="0" cellspacing="0">
<tr style="background-color:#D55500">
<td colspan="2" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Funeral Home Details</td>
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
    <td><asp:TextBox ID="txtName" runat="server" ToolTip="Enter Funeral home name"/></td>
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
          <asp:TextBox ID="txtphone" runat="server" CausesValidation="True" ToolTip="Enter a vaild 10 digit phone # with dashes"/>
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator1" runat="server" 
                ErrorMessage="Must be a valid Phone # ie. 555-555-1212 with dashes" 
                ControlToValidate="txtphone" 
                ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic"></asp:RegularExpressionValidator>
     </td>
</tr>
<tr>
     <td align="right">
     Chapel Service:
     </td>
     <td>
          <asp:TextBox ID="txtChapSvc" runat="server" ToolTip="Enter price for Chapel Service"/>
     </td>
</tr>
<tr>
     <td align="right">
     Traditional Service:
     </td>
     <td>
          <asp:TextBox ID="txtTradSvc" runat="server" ToolTip="Enter price for Traditional Service"/>
     </td>
</tr>
<tr>
     <td align="right">
     Same Day Service:
     </td>
     <td>
          <asp:TextBox ID="txtSameDaySvc" runat="server" ToolTip="Enter price for Same Day Service"/>
     </td>
</tr>
<tr>
     <td align="right">
     Gravesite Service:
     </td>
     <td>
          <asp:TextBox ID="txtGraveSvc" runat="server" ToolTip="Enter price for Gravesite Service"/>
     </td>
</tr>
<tr>
     <td align="right">
     Traditional Cremation:
     </td>
     <td>
          <asp:TextBox ID="txtTradCrem" runat="server" ToolTip="Enter price for Traditional Cremation"/>
     </td>
</tr>
<tr>
     <td align="right">
     Same Day Cremation:
     </td>
     <td>
          <asp:TextBox ID="txtSameDayCrem" runat="server" ToolTip="Enter price for Same Day Cremation"/>
     </td>
</tr>
<tr>
     <td align="right">
     Memorial Cremation:
     </td>
     <td>
          <asp:TextBox ID="txtMemCrem" runat="server" ToolTip="Enter price for Memorial Cremation"/>
     </td>
</tr>
<tr>
     <td align="right">
     Gravesite Cremation:
     </td>
     <td>
          <asp:TextBox ID="txtGraveCrem" runat="server" ToolTip="Enter price for Gravesite Cremation"/>
     </td>
</tr>
<tr>
     <td align="right">
     Direct Cremation:
     </td>
     <td>
          <asp:TextBox ID="txtDirectCrem" runat="server" ToolTip="Enter price for Direct Cremation"/>
     </td>
</tr>
<tr>
     <td align="right">
     Limo:
     </td>
     <td>
          <asp:TextBox ID="txtLimo" runat="server" ToolTip="Enter price for Limo Service"/>
     </td>
</tr><tr>
     <td align="right">
     Motor Escort:
     </td>
     <td>
          <asp:TextBox ID="txtMEscort" runat="server" ToolTip="Enter price for Motor Escort Service"/>
     </td>
</tr>
<tr>
     <td align="right">
     Alternative Container:
     </td>
     <td>
          <asp:TextBox ID="txtAltContainer" runat="server" ToolTip="Enter price for Cremation Container"/>
     </td>
</tr>
<tr>
     <td align="right">
     Clergy:
     </td>
     <td>
          <asp:TextBox ID="txtClergy" runat="server" ToolTip="Enter price for Clergy"/>
     </td>
</tr>
<tr>
     <td align="right">
     Memorial Package:
     </td>
     <td>
          <asp:TextBox ID="txtMemPkg" runat="server" ToolTip="Enter price for Memorial Package"/>
     </td>
</tr>
<tr>
     <td align="right">
     Video:
     </td>
     <td>
          <asp:TextBox ID="txtVideo" runat="server" ToolTip="Enter price for Video"/>
     </td>
</tr>
<tr>
     <td align="right">
     Shroud:
     </td>
     <td>
          <asp:TextBox ID="txtShroud" runat="server" ToolTip="Enter price for Linen Shroud"/>
     </td>
</tr>
<tr>
     <td align="right">
     Muslim Shroud:
     </td>
     <td>
          <asp:TextBox ID="txtShroudMuslin" runat="server" ToolTip="Enter price for Muslin Shroud"/>
     </td>
</tr>
<tr>
     <td align="right">
     Tahara:
     </td>
     <td>
          <asp:TextBox ID="txtTahara" runat="server" ToolTip="Enter price for Tahara"/>
     </td>
</tr>
<tr>
     <td align="right">
     Shomer:
     </td>
     <td>
          <asp:TextBox ID="txtShomer" runat="server" ToolTip="Enter price for Shomer"/>
     </td>
</tr>
<tr>
     <td align="right">
     Gratuities:
     </td>
     <td>
          <asp:TextBox ID="txtGratuities" runat="server" ToolTip="Enter price for Gratuities"/>
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
</div>
</asp:Panel>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT * FROM [flpreffunhm]" ConflictDetection="CompareAllValues" >
</asp:SqlDataSource>
</form>
</body>
</html>
