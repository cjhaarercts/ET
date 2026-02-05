<%@ Page Title="PLMS - Home" Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PLMS - Home</title>
    <link href="styles/style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release" />
            <asp:Panel ID="AuthenticatedMessagePanel" runat="server" Visible="false">
                <asp:Label ID="WelcomeBackMessage" runat="server" Text="Welcome back!" />
            </asp:Panel>
            <asp:Panel ID="AnonymousMessagePanel" runat="server" Visible="true">
                <asp:Label ID="AnonymousMessage" runat="server" Text="Please log in to access the system." />
            </asp:Panel>
            <asp:Button ID="Button2" runat="server" Text="Customer" OnClick="NavigateToPage" CommandArgument="customer.aspx" />
            <asp:Button ID="Button3" runat="server" Text="Input Form" OnClick="NavigateToPage" CommandArgument="inputform.aspx" />
            <asp:Button ID="Button4" runat="server" Text="Lookup LNH" OnClick="NavigateToPage" CommandArgument="custlookuplnhp.aspx" />
            <asp:Button ID="Button5" runat="server" Text="Lookup HP" OnClick="NavigateToPage" CommandArgument="custlookuphp.aspx" />
            <asp:Button ID="Button6" runat="server" Text="Lookup Agent" OnClick="NavigateToPage" CommandArgument="custlookupagnt.aspx" />
            <asp:Button ID="Button7" runat="server" Text="Agent New" OnClick="NavigateToPage" CommandArgument="cluAgentnew.aspx" />
            <asp:Button ID="Button8" runat="server" Text="Agent Active" OnClick="NavigateToPage" CommandArgument="cluAgentActive.aspx" />
            <asp:Button ID="Button9" runat="server" Text="Agent CB Tickler" OnClick="NavigateToPage" CommandArgument="cluAgentCBTickler.aspx" />
            <asp:Button ID="Button20" runat="server" Text="Agent CB" OnClick="NavigateToPage" CommandArgument="cluAgentCB.aspx" />
            <asp:Button ID="Button10" runat="server" Text="Agent LM Tickler" OnClick="NavigateToPage" CommandArgument="cluAgentLMTickler.aspx" />
            <asp:Button ID="Button11" runat="server" Text="Last Resort" OnClick="NavigateToPage" CommandArgument="cluLastResort.aspx" />
            <asp:Button ID="Button12" runat="server" Text="Agent Sold" OnClick="NavigateToPage" CommandArgument="cluAgentSold.aspx" />
            <asp:Button ID="Button14" runat="server" Text="Input Sold Lead" OnClick="NavigateToPage" CommandArgument="inputsoldlead.aspx" />
            <asp:Button ID="Button15" runat="server" Text="Fun Homes" OnClick="NavigateToPage" CommandArgument="funhomes.aspx" Visible="false" />
            <asp:Button ID="Button21" runat="server" Text="Pref Fun Homes" OnClick="NavigateToPage" CommandArgument="../PDFFiles/prefFunhomes.aspx" Visible="false" />
            <asp:Button ID="Button22" runat="server" Text="Mail Service" OnClick="NavigateToPage" CommandArgument="mailservice.aspx" />
            <asp:Button ID="Button16" runat="server" Text="Input FH" OnClick="NavigateToPage" CommandArgument="inputfh.aspx" Visible="false" />
            <asp:Button ID="Button17" runat="server" Text="Agent SM" OnClick="NavigateToPage" CommandArgument="cluAgentSM.aspx" />
            <asp:Button ID="Button18" runat="server" Text="Agent Web" OnClick="NavigateToPage" CommandArgument="cluAgentWeb.aspx" />
            <asp:Button ID="DButton" runat="server" Text="Agent Dead" OnClick="NavigateToPage" CommandArgument="cluAgentDead.aspx" Visible="false" />
        </div>
    </form>
</body>
</html>
