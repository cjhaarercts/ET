<%@ Page Title="PLMS - Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent2" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release" />
    <!-- Authentication removed: show PLMS content by default -->
    <asp:Panel ID="AuthenticatedMessagePanel" runat="server" Visible="true">
        <asp:Label ID="WelcomeBackMessage" runat="server" Text="Welcome to the Partner Lead Management System (PLMS)." />
    </asp:Panel>

    <!-- Page-specific content only — PLMS buttons remain in MasterPage.master's MainContent1 -->
    <asp:Panel ID="PageIntro" runat="server">
        <p>Use the PLMS links in the toolbar above to navigate.</p>
    </asp:Panel>
</asp:Content>
