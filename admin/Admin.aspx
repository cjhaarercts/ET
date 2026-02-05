<%@ Page Language="C#" MasterPageFile="~/tvp/Site.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="_Default" Title="Eternal Solutions - Partner Home" MetaKeywords="pre need funeral, Pre-need funeral, pre need funeral insurance, pre need funeral planning, pre-need funeral arrangements, preneed funeral, Funeral services, funeral arrangements, funeral programs, funeral costs, funeral ceremony, cost of a funeral, Funeral information, Funeral Guide, funeral article, modern funeral, funeral procedures, funeral process, Veteran funeral, Veterans funerals, Veterans funeral, Veterans burial benefits, Veteran Cemetery, Veterans Cemetery" MetaDescription="Preneed funeral plans outline wishes for funeral services and interment. To avoid inflationary costs, prepaid funeral plans are available." %>
<asp:Content ID="Content1" ContentPlaceHolderID="LoginContent1" Runat="Server">

    <asp:Panel runat="server" ID="AuthenticatedMessagePanel" >
        <asp:Label runat="server" ID="WelcomeBackMessage"></asp:Label>
        <p>

            <p><asp:Button ID="lnk" runat="server" Text="Partner Pages" PostBackUrl="~/tvp/admin/secure.aspx" /></p>


            <p><asp:Button ID="lnkLogout" runat="server" Text="Log Out" PostBackUrl="~/default.aspx" /></p>


            <p><asp:Button ID="lnkchgpwd" runat="server" Text="Change Password" PostBackUrl="~/default.aspx" /></p> <!-- Change Password archived -->

            &nbsp;</p>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LoginContent2" Runat="Server">
    
            <!-- User and Role management removed (authentication disabled) -->
            <p><!-- Manage Users / Roles UI removed -->
            </p>

    
    
        
    <asp:Panel runat="Server" ID="AnonymousMessagePanel">
        <!-- Login UI removed -->
    </asp:Panel>

       <!-- <p><asp:Button ID="tst2lnk" runat="server" Text="Partner Pages" PostBackUrl="~/tvp/admin/secure.aspx" /></p>
    
    <!-- <p><asp:HyperLink runat="server" ID="tstlnk" NavigateUrl="~/tvp/Admin/secure.aspx" Text="Partner Pages"></asp:HyperLink></p> -->
</asp:Content>