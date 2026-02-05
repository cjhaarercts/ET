<%@ Page Title="Veteran Program - Request Information" Language="C#" MasterPageFile="../tvp/masterpage.master" AutoEventWireup="true" CodeFile="request.aspx.cs" Inherits="request" MetaKeywords="pre need funeral, Pre-need funeral, pre need funeral insurance, pre need funeral planning, pre-need funeral arrangements, preneed funeral, Funeral services, funeral arrangements, funeral programs, funeral costs, funeral ceremony, cost of a funeral, Funeral information, Funeral Guide, funeral article, modern funeral, funeral procedures, funeral process, Veteran funeral, Veterans funerals, Veterans funeral, Veterans burial benefits, Veteran Cemetery, Veterans Cemetery" MetaDescription="Preneed funeral plans outline wishes for funeral services and interment. To avoid inflationary costs, prepaid funeral plans are available." %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent1" Runat="Server">
<form style="visibility:hidden" action="@@" method="post">
    <label style="visibility:hidden">Please leave this field empty</label>
    <input style="visibility:hidden" type="text" name="Email"/>
    <br />
    <input style="visibility:hidden" type="submit" />
</form>
    <!-- Begin Page Content -->
	<div id="page_content">
		<!-- Begin Left Column -->
		<!--div id="column_l">
			<!-- #BeginEditable "content" -->
    <!-- <div> -->
    <form id="form1" runat="server">
    <br />
    <h2 style="text-align: left">Request More Information</h2>
        <table align="left" class="style1">
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    First Name</td>
                <td class="style3">
                    <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Last Name</td>
                <td class="style3">
                    <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Email Address</td>
                <td class="style3">
                    <asp:TextBox ID="EmailAddress" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Home Phone</td>
                <td class="style1">
                    <asp:TextBox ID="HomePhone" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Address</td>
                <td class="style3">
                    <asp:TextBox ID="Address" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    City</td>
                <td class="style3">
                    <asp:TextBox ID="City" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    State</td>
                <td class="style3">
                    <asp:TextBox ID="State" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Zip</td>
                <td class="style3">
                    <asp:TextBox ID="ZIP" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Ages</td>
                <td class="style3">
                    <asp:TextBox ID="Ages" runat="server" Width="266px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Branch</td>
                <td class="style3">
                    <asp:DropDownList ID="Branch" runat="server">
                        <asp:ListItem>Select</asp:ListItem>
                        <asp:ListItem>Air Force</asp:ListItem>
                        <asp:ListItem>Army</asp:ListItem>
                        <asp:ListItem>Coast Guard</asp:ListItem>
                        <asp:ListItem>Marines</asp:ListItem>
                        <asp:ListItem>Navy</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    Honorable Discharge</td>
                <td class="style3">
                    <asp:DropDownList ID="HDischarge" runat="server">
                        <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2" 
                    style="font-family: Arial, Helvetica, sans-serif; font-weight: 700">
                    <asp:Button ID="Button1" runat="server" Text="Request Information" 
                        onclick="Button1_Click" />
                </td>
           </tr>
        </table>
        </form>
    </div>
    <asp:Label ID="Label1" runat="server" Text="Label1" ForeColor="Green" Visible="False"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
        SelectCommand="SELECT * FROM [Customers]"></asp:SqlDataSource>
			<!-- #EndEditable -->
        <!-- </div>
		<!-- End Left Column -->
    <!-- </div> -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent2" Runat="Server">
<!-- Begin Page Content -->
	<div id="Div1">
    <!-- Begin Right Column -->
		<div id="column_r">
			<!-- #BeginEditable "sidebar" -->
		<!-- #EndEditable -->
        </div>
		<!-- End Right Column -->
    </div>
	<!-- End Page Content -->
</asp:Content>

