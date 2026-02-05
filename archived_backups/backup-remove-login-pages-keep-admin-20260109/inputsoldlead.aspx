<%@ Page Title="Eternal Solutions - Input sold Lead" Language="C#" AutoEventWireup="true" CodeFile="inputsoldlead.aspx.cs" Inherits="custlookuphp" %>

<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Input sold Lead</title>
<style type="text/css">
.modalBackground
{
background-color: Gray;
z-index: 10000;
}
</style>
</head>
<body>
    <h3>Sales Lead Database</h3>
    <br />
    <h3>Input Sold Lead</h3>
    <h4>Search for Client by Last Name Phone Number</h4>
    <!-- LoginView removed: authentication disabled -->
        <div class="auth-removed">Authentication removed; role-based messages removed.</div>
    <br />
<form id="form1" runat="server">
<div>

     <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
     </asp:ToolkitScriptManager>

         <div style="width: 736px; height: 66px">
           
          <asp:Label ID="Label1" runat="server" Width="100px"></asp:Label>
          <br />
            <br />
            Please enter the Customers Last Name or Home Phone Number
            <br />
            <asp:TextBox ID="TextBox1" runat="server" ToolTip="Type the Last Name or Phone Number of the Client" />
      
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
    DataSourceID="SqlDataSource1" OnRowCreated="OnRowCreated" AutoGenerateEditButton="True" >
    <RowStyle BackColor="#EFF3FB" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="False" />
        <asp:BoundField DataField="LastName" HeaderText="Last Name" ReadOnly="True" SortExpression="LastName" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" ReadOnly="True" SortExpression="FirstName" />
        <asp:BoundField DataField="Address" HeaderText="Address" ReadOnly="True" SortExpression="Address" />
        <asp:BoundField DataField="City" HeaderText="City" ReadOnly="True" SortExpression="City" />
        <asp:BoundField DataField="State" HeaderText="State" ReadOnly="True" SortExpression="State" />
        <asp:BoundField DataField="ZIP" HeaderText="ZIP" ReadOnly="True" SortExpression="ZIP" />
        <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="True" SortExpression="Status" />
        <asp:TemplateField HeaderText="Sold Date">
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="SoldDate" ToolTip="Using the popup calendar select the date the policy was sold" Text='<%# Bind("SoldDate") %>' />
                <obout:Calendar ID="Calendar1" runat="server"
                    Columns="1"
                    DateFormat="MM/dd/yyyy"
                    DatePickerMode="true"
                    TextBoxId="SoldDate" DatePickerImagePath="../images/icon2.gif" DateMin="01/01/2011">
                </obout:Calendar> 
            </EditItemTemplate>
            <ItemTemplate>
                    <asp:Label ID="Label1" runat="server"
                        Text='<%# Bind("SoldDate", "{0:d}") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Paid in Full"> 
            <EditItemTemplate>
                <asp:CheckBox ID="CheckBox1" runat="server" ToolTip="Place a check in the box if the  contract is paid in full" Checked='<%# Bind("Postcard") %>' />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Postcard") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:HyperLink runat="server" ID="lnkPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
<asp:Label ID="lblresult" runat="server"/>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT [ID], [LastName], [FirstName], [Address], [City], [State], [ZIP], [Status], [SoldDate], [Postcard] FROM [Customers] WHERE ([LastName] = @LastName) OR ([HomePhone] = @HomePhone)"
     ConflictDetection="CompareAllValues"
     UpdateCommand="UPDATE [Customers] SET [Status] = 'Sold', [SoldDate] = @SoldDate, [Postcard] = @Postcard WHERE [ID] = @original_ID ">
    <SelectParameters>
        <asp:ControlParameter ControlID="TextBox1" Name="LastName" PropertyName="Text" 
            Type="String" />
        <asp:ControlParameter ControlID="TextBox1" Name="HomePhone" PropertyName="Text" 
            Type="String" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="Status" Type="String" />
        <asp:Parameter Name="SoldDate" DbType="Date" />
        <asp:Parameter Name="Postcard" Type="Byte" />
        <asp:Parameter Name="original_ID" Type="Int32" />
        <asp:Parameter Name="original_LastName" Type="String" />
        <asp:Parameter Name="original_FirstName" Type="String" />
        <asp:Parameter Name="original_Address" Type="String" />
        <asp:Parameter Name="original_City" Type="String" />
        <asp:Parameter Name="original_State" Type="String" />
        <asp:Parameter Name="original_ZIP" Type="String" />
        <asp:Parameter Name="original_Status" Type="String" />
        <asp:Parameter Name="original_Postcard" Type="Byte" />
        <asp:Parameter DbType="Date" Name="original_SoldDate" />
    </UpdateParameters>
</asp:SqlDataSource>
</form>
</body>
</html>
