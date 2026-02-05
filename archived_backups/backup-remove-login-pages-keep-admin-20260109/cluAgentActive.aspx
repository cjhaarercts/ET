<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cluAgentActive.aspx.cs" Inherits="_cluAgentActive" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agent Active</title>
    <link href="styles/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCreated="OnRowCreated">
                <!-- Define your columns here -->
            </asp:GridView>
            <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
            <asp:Label ID="lblID" runat="server" Text=""></asp:Label>
            <asp:TextBox ID="txtagent" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtstatus" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtlname" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtfname" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtadd" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtcity" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtstate" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtzip" runat="server"></asp:TextBox>
            <asp:TextBox ID="txthphone" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtmphone" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtemail" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtbranch" runat="server"></asp:TextBox>
            <asp:TextBox ID="txthdischarge" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtages" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtnotes" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtapptset" runat="server"></asp:TextBox>
            <asp:Label ID="lblresult" runat="server" Text=""></asp:Label>
            <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" />
            <asp:ImageButton ID="ImageButton2" runat="server" OnClick="ImageButton2_Click" />
            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnUpdate" PopupControlID="Panel1"></asp:ModalPopupExtender>
        </div>
    </form>
</body>
</html>
