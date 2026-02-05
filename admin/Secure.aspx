<%@ Page Language="C#" MasterPageFile="site.master" AutoEventWireup="true" CodeFile="secure.aspx.cs" Inherits="_secure" Title="Eternal Solutions - Back end" %>

<asp:Content ID="Content1" ContentPlaceHolderID="LoginContent2" Runat="Server">
    <asp:Panel runat="server" ID="AuthenticatedMessagePanel"/>
        <asp:Label runat="server" ID="WelcomeBackMessage" />

<div>
	
        <asp:Label ID="Label1" runat="server" Text="Label1" Visible="false" />
        <br />
        <asp:Button ID="Button13" runat="server" onclick="Button3_Click" Text="New Lead Input" />
        &nbsp;<asp:Button ID="Button14" runat="server" onclick="Button14_Click" Text="Update Sold Leads" />
        &nbsp;<asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="Find Customer by Last Name" />
        &nbsp;<asp:Button ID="Button5" runat="server" onclick="Button5_Click" Text="Find Customer by Phone Number" />
        <br />
        <br />
        <asp:Button ID="Button7" runat="server" onclick="Button7_Click" Text="New Leads by Agent" />
        &nbsp;<asp:Button ID="Button8" runat="server" onclick="Button8_Click" Text="Active Leads by Agent" />
        &nbsp;<asp:Button ID="Button10" runat="server" onclick="Button10_Click" Text="Left Message Leads by Agent" />
        &nbsp;<asp:Button ID="Button9" runat="server" onclick="Button9_Click" Text="Call Back Tickler" />
        &nbsp;<asp:Button ID="Button20" runat="server" onclick="Button20_Click" Text="Call Back Leads by Agent" />
        <br />
        <br />
        <asp:Button ID="Button18" runat="server" onclick="Button18_Click" Text="Website Leads by Agent" />
        &nbsp;<asp:Button ID="Button111" runat="server" onClick="Button111_Click" Text="Seminar LEasds By Agent" /> -->
        &nbsp;<asp:Button ID="Button11" runat="server" onclick="Button11_Click" Text="Last Resort Leads by Agent" />
        &nbsp;<asp:Button ID="Button17" runat="server" onclick="Button17_Click" Text="Sent Mail Leads by Agent" />
        &nbsp;<asp:Button ID="Button6" runat="server" onclick="Button12_Click" Text="Sold Leads by Agent" />
        <!-- &nbsp;<asp:Button ID="Button111" runat="server" Text="Dead Leads" /> -->
        <br />
        <br />
            <asp:Button ID="Button15" runat="server" onclick="Button15_Click" 
            Text="Funeral Home Edit Screen" PostBackUrl="~/tvp/funhomes.aspx" />
        &nbsp;<asp:Button ID="Button16" runat="server" onclick="Button16_Click" 
            Text="Input New Funeral Home" PostBackUrl="~/tvp/inputfh.aspx" />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Log Out" PostBackUrl="~/tvp/Logout.aspx" />
        &nbsp;<asp:Button ID="lnkchgpwd" runat="server" Text="Change Password" PostBackUrl="~/Account/ChangePassword.aspx" />
        &nbsp;<asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Administrative Edit Form" />
        &nbsp;<asp:Button ID="Button3" runat="server" onclick="Button6_Click" Text="Find Leads by Agent Unfiltered" />
        &nbsp;<asp:Button ID="Button19" runat="server" onclick="Button19_Click" Text="NGL PDF Forms" />
        <br />
        <br />
    
</div>
   
</asp:Content>
