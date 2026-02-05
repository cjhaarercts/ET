<%@ Page Title="PLMS - Lead entry form" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true" CodeFile="inputform.aspx.cs" Inherits="_inputform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<!-- Begin Page Content -->
<div id="page_content0">
		<!-- Begin Left Column -->
		
    <h2>New Lead Input Form</h2>
    <table style="width: 100%;">
        <tr>
            <td class="style2">
                First Name</td>
            <td class="style4">
                <asp:TextBox ID="txtfname" runat="server" MaxLength="50" ToolTip="Please enter the First name."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Last Name</td>
            <td class="style4">
                <asp:TextBox ID="txtlname" runat="server" MaxLength="50" ToolTip="Please enter the Last name."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Address</td>
            <td class="style4">
                <asp:TextBox ID="txtadd" runat="server" MaxLength="50" ToolTip="Please enter the street address."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                City</td>
            <td class="style4">
                <asp:TextBox ID="txtcity" runat="server" MaxLength="50" ToolTip="Please enter the city."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                State</td>
            <td class="style4">
                <asp:TextBox ID="txtstate" runat="server" MaxLength="2" ToolTip="Please enter the two letter state."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style6">
                Zip</td>
            <td class="style7">
                <asp:TextBox ID="txtzip" runat="server" MaxLength="10" ToolTip="Please enter the zip code."></asp:TextBox>
            </td>
            <td class="style8">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ErrorMessage="Please enter a valid US zip code. " 
                    ValidationExpression="\d{5}(-\d{4})?" ControlToValidate="txtzip"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Home Phone</td>
            <td class="style4">
                <asp:TextBox ID="txthphone" runat="server" MaxLength="12" ToolTip="Please enter the 10 digit phone number with dashes."></asp:TextBox>
            </td>
            <td class="style9">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txthphone" 
                    ErrorMessage="Please enter a valid 10 digit phone number." 
                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ToolTip="Enter the Phone number without dashes."></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Mobile Phone</td>
            <td class="style4">
                <asp:TextBox ID="txtmphone" runat="server" MaxLength="12" ToolTip="Please enter the 10 digit phone number with dashes."></asp:TextBox>
            </td>
            <td class="style9">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                    ControlToValidate="txthphone" 
                    ErrorMessage="Please enter a valid 10 digit phone number." 
                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ToolTip="Enter the Phone number without dashes."></asp:RegularExpressionValidator>
            </td>
        </tr>

        <tr>
            <td class="style2">
                Email Address</td>
            <td class="style4">
                <asp:TextBox ID="txtemail" runat="server" ToolTip="Please enter a valid email address - example@gmail.com."></asp:TextBox>
            </td>
            <td class="style9">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                    ControlToValidate="txtemail" 
                    ErrorMessage="Please enter a valid Email address ie. abc@123.com." 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Branch</td>
            <td class="style4">
                <asp:DropDownList ID="txtbranch" runat="server" ToolTip="Please pick a Branch or N/A if not known.">
                    <asp:ListItem>Unknown</asp:ListItem>
                    <asp:ListItem>Air Force</asp:ListItem>
                    <asp:ListItem>Army</asp:ListItem>
                    <asp:ListItem>Coast Guard</asp:ListItem>
                    <asp:ListItem>Marines</asp:ListItem>
                    <asp:ListItem>Navy</asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="You must select a branch or N/A !!!" ControlToValidate="txtbranch" ValueToCompare="Value" Operator="NotEqual"></asp:CompareValidator> 
             </td>
            <td class="style9">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Honorable Discharge</td>
            <td class="style4">
                <asp:DropDownList ID="txthdischarge" runat="server" ToolTip="Please select a status or N/A if not available.">
                    <asp:ListItem>Unknown</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                    <asp:ListItem>No</asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="You must make a selection or N/A !!!" ControlToValidate="txthdischarge" ValueToCompare="Value" Operator="NotEqual"></asp:CompareValidator>
            </td>
            <td class="style9">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                Ages</td>
            <td class="style5">
                <asp:TextBox ID="txtages" runat="server" ToolTip="Please enter the age(s)."></asp:TextBox>
            </td>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td align="right">
            Type of Service:
            </td>
            <td>
                 <asp:DropDownList ID="txttype" runat="server" ToolTip="Select the type of service requested for this record">
                       <asp:ListItem>Unknown</asp:ListItem>
                       <asp:ListItem>Burial</asp:ListItem>
                       <asp:ListItem>Cremation</asp:ListItem>
                       <asp:ListItem>Burial and Cremation</asp:ListItem>
                   </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
            Agent:
            </td>
            <td>
                <asp:DropDownList ID="txtagent" runat="server" ToolTip="Select the agent for this record">
                <asp:ListItem>Select One</asp:ListItem>
                <asp:ListItem>VPP Lead</asp:ListItem>
                <asp:ListItem>VPW Web Lead</asp:ListItem>
                <asp:ListItem>VPP Sharon Stangler</asp:ListItem>
                <asp:ListItem>VPP Richard Stangler</asp:ListItem>
                <asp:ListItem>Asher Lead</asp:ListItem>
                <asp:ListItem>Asher Sharon Stangler</asp:ListItem>
                <asp:ListItem>Asher Richard Stangler</asp:ListItem>
                <asp:ListItem>Stangler Lead</asp:ListItem>
                <asp:ListItem>Sharon Stangler</asp:ListItem>
                <asp:ListItem>Richard Stangler</asp:ListItem>
                <asp:ListItem>Mary Jo Hudson</asp:ListItem>
                <asp:ListItem>CTS</asp:ListItem>

                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
            VPW Lead:
            </td>
            <td>
                <asp:CheckBox ID="CheckBox" runat="server" ToolTip="Place a check in the box if the lead came from a VP Web Ad." />
            </td>
        </tr>
        <tr>
            <td align="right">
            VPP Lead:
            </td>
            <td>
                <asp:CheckBox ID="CheckBox1" runat="server" ToolTip="Place a check in the box if the lead came from a VP Mail Ad." />
            </td>
        </tr>
        <tr>
            <td align="right">
            Asher Lead:
            </td>
            <td>
                <asp:CheckBox ID="CheckBox2" runat="server" ToolTip="Place a check in the box if the lead came from Asher Ad." />
            </td>
        </tr>
        <tr>
            <td align="right">
            Web Lead:
            </td>
            <td>
                <asp:CheckBox ID="CheckBox3" runat="server" ToolTip="Place a check in the box if the lead came from the website." />
            </td>
        </tr>
        <tr>
            <td align="right">
            Seminar Lead:
            </td>
            <td>
                <asp:CheckBox ID="CheckBox4" runat="server" ToolTip="Place a check in the box if the lead came from a Seminar Ad." />
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style4">
                <asp:Button ID="Submit" runat="server" Text="Submit" onclick="Submit_Click" />
            </td>
            <td class="style9">
                <input id="Reset1" type="reset" value="Start Over" />
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" text="Logout" />
            </td>
        </tr>
    </table>
    <asp:Label ID="Label1" runat="server" visible="false" Text="Label"></asp:Label>
    
	</div>
		
	</asp:Content>