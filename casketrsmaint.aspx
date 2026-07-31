<%@ Page Language="C#" AutoEventWireup="true" CodeFile="casketrsmaint.aspx.cs" Inherits="casketrsmaint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Casket RS Maintenance</title>
</head>
<body>
    <h2>Casket RS (casketrs)</h2>
    <form id="form1" runat="server">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
        <br />
        <asp:GridView ID="GridView1" runat="server"
            AutoGenerateColumns="True"
            AutoGenerateEditButton="True"
            AllowPaging="True"
            PageSize="25"
            OnRowEditing="GridView1_RowEditing"
            OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowUpdating="GridView1_RowUpdating">
            <RowStyle BackColor="#EFF3FB" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </form>
</body>
</html>
