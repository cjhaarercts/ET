<%@ Page Title="Eternal Solutions - Funeral Home entry form" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="inputfh.aspx.cs" Inherits="inputfh" MetaKeywords="pre need funeral, Pre-need funeral, pre need funeral insurance, pre need funeral planning, pre-need funeral arrangements, preneed funeral, Funeral services, funeral arrangements, funeral programs, funeral costs, funeral ceremony, cost of a funeral, Funeral information, Funeral Guide, funeral article, modern funeral, funeral procedures, funeral process, Veteran funeral, Veterans funerals, Veterans funeral, Veterans burial benefits, Veteran Cemetery, Veterans Cemetery" MetaDescription="Preneed funeral plans outline wishes for funeral services and interment. To avoid inflationary costs, prepaid funeral plans are available." %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>  
	<!-- Begin Page Content -->
<div id="page_content0">
		<!-- Begin Left Column -->
		
    <h2>New Funeral Home Input Form</h2>
    <table style="width: 100%;">
        <tr>
            <td class="style2">
                Funeral Home</td>
            <td class="style4">
                <asp:TextBox ID="txtcompany" runat="server" MaxLength="50" ToolTip="Please enter the Funeral Home name."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Director</td>
            <td class="style4">
                <asp:TextBox ID="txtdirector" runat="server" MaxLength="50" ToolTip="Please enter Funeral Home director name."></asp:TextBox>
            </td>
            <td class="style9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Contact</td>
            <td class="style4">
                <asp:TextBox ID="txtcontact" runat="server" MaxLength="50" ToolTip="Please enter Funeral Home contact name."></asp:TextBox>
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
                Business Phone</td>
            <td class="style4">
                <asp:TextBox ID="txtbphone" runat="server" MaxLength="12" ToolTip="Please enter the 10 digit phone number with dashes."></asp:TextBox>
            </td>
            <td class="style9">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txtbphone" 
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
                    ControlToValidate="txtmphone" 
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
            <td align="right">
            Status
            </td>
            <td>
                 <asp:DropDownList ID="txtstatus" runat="server" ToolTip="Select a status for this lead">
                       <asp:ListItem>New</asp:ListItem>
                       <asp:ListItem>N/A</asp:ListItem>
                 </asp:DropDownList>
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
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConflictDetection="CompareAllValues" 
        ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
        DeleteCommand="DELETE FROM [Funhomes] WHERE [ID] = @original_ID AND (([Company] = @original_Company) OR ([Company] IS NULL AND @original_Company IS NULL)) AND (([LastName] = @original_LastName) OR ([LastName] IS NULL AND @original_LastName IS NULL)) AND (([FirstName] = @original_FirstName) OR ([FirstName] IS NULL AND @original_FirstName IS NULL)) AND (([EmailAddress] = @original_EmailAddress) OR ([EmailAddress] IS NULL AND @original_EmailAddress IS NULL)) AND (([BusPhone] = @original_BusPhone) OR ([BusPhone] IS NULL AND @original_BusPhone IS NULL)) AND (([MobilePhone] = @original_MobilePhone) OR ([MobilePhone] IS NULL AND @original_MobilePhone IS NULL)) AND (([Fax Number] = @original_Fax_Number) OR ([Fax Number] IS NULL AND @original_Fax_Number IS NULL)) AND (([Address] = @original_Address) OR ([Address] IS NULL AND @original_Address IS NULL)) AND (([City] = @original_City) OR ([City] IS NULL AND @original_City IS NULL)) AND (([State] = @original_State) OR ([State] IS NULL AND @original_State IS NULL)) AND (([ZIP] = @original_ZIP) OR ([ZIP] IS NULL AND @original_ZIP IS NULL)) AND (([Notes] = @original_Notes) OR ([Notes] IS NULL AND @original_Notes IS NULL)) AND (([AppointmentSet] = @original_AppointmentSet) OR ([AppointmentSet] IS NULL AND @original_AppointmentSet IS NULL)) AND (([Director] = @original_Director) OR ([Director] IS NULL AND @original_Director IS NULL)) AND (([upsize_ts] = @original_upsize_ts) OR ([upsize_ts] IS NULL AND @original_upsize_ts IS NULL)) AND (([Created] = @original_Ceated) OR ([Ceated] IS NULL AND @original_Created IS NULL)) AND (([Contact] = @original_Contact) OR ([Contact] IS NULL AND @original_Contact IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))" 
        InsertCommand="INSERT INTO [Funhomes] ([Company], [LastName], [FirstName], [EmailAddress], [BusPhone], [MobilePhone], [Fax Number], [Address], [City], [State], [ZIP], [Notes], [Attachments], [AppointmentSet], [Director], [upsize_ts], [Created], [Contact], [Status}) VALUES (@Company, @LastName, @FirstName, @EmailAddress, @BusPhone, @MobilePhone, @Fax_Number, @Address, @City, @State, @ZIP, @Notes, @Attachments, @AppointmentSet, @Director, @upsize_ts, @Created, @Contact, @Status)" 
        OldValuesParameterFormatString="original_{0}" 
        SelectCommand="SELECT * FROM [Funhomes]" 
        UpdateCommand="UPDATE [Funhomes] SET [Company] = @Company, [LastName] = @LastName, [FirstName] = @FirstName, [EmailAddress] = @EmailAddress, [BusPhone] = @BusPhone, [MobilePhone] = @MobilePhone, [Fax Number] = @Fax_Number, [Address] = @Address, [City] = @City, [State] = @State, [ZIP] = @ZIP, [Notes] = @Notes, [Attachments] = @Attachments, [Director] = @Director, [upsize_ts] = @upsize_ts, [Created] = @Created, [Contact] = @Contact, [Status] = @Status, WHERE [ID] = @original_ID AND (([Company] = @original_Company) OR ([Company] IS NULL AND @original_Company IS NULL)) AND (([LastName] = @original_LastName) OR ([LastName] IS NULL AND @original_LastName IS NULL)) AND (([FirstName] = @original_FirstName) OR ([FirstName] IS NULL AND @original_FirstName IS NULL)) AND (([EmailAddress] = @original_EmailAddress) OR ([EmailAddress] IS NULL AND @original_EmailAddress IS NULL)) AND (([BusPhone] = @original_BusPhone) OR ([BusPhone] IS NULL AND @original_BusPhone IS NULL)) AND (([MobilePhone] = @original_MobilePhone) OR ([MobilePhone] IS NULL AND @original_MobilePhone IS NULL)) AND (([Fax Number] = @original_Fax_Number) OR ([Fax Number] IS NULL AND @original_Fax_Number IS NULL)) AND (([Address] = @original_Address) OR ([Address] IS NULL AND @original_Address IS NULL)) AND (([City] = @original_City) OR ([City] IS NULL AND @original_City IS NULL)) AND (([State] = @original_State) OR ([State] IS NULL AND @original_State IS NULL)) AND (([ZIP] = @original_ZIP) OR ([ZIP] IS NULL AND @original_ZIP IS NULL)) AND (([Notes] = @original_Notes) OR ([Notes] IS NULL AND @original_Notes IS NULL)) AND (([Attachments] = @original_Attachments) OR ([Attachments] IS NULL AND @original_Attachments IS NULL)) AND (([AppointmentSet] = @original_AppointmentSet) OR ([AppointmentSet] IS NULL AND @original_AppointmentSet IS NULL)) AND (([Director] = @original_Director) OR ([Director] IS NULL AND @original_Director IS NULL)) AND (([upsize_ts] = @original_upsize_ts) OR ([upsize_ts] IS NULL AND @original_upsize_ts IS NULL)) AND (([Created] = @original_Created) OR ([Created] IS NULL AND @original_Created IS NULL)) AND (([Contact] = @original_Contact) OR ([Contact] IS NULL AND @original_Contact IS NULL)) AND (([Status] = @original_Status) OR ([Status] IS NULL AND @original_Status IS NULL))">
        <DeleteParameters>
            <asp:Parameter Name="original_ID" Type="Int32" />
            <asp:Parameter Name="original_Company" Type="String" />
            <asp:Parameter Name="original_LastName" Type="String" />
            <asp:Parameter Name="original_FirstName" Type="String" />
            <asp:Parameter Name="original_EmailAddress" Type="String" />
            <asp:Parameter Name="original_BusPhone" Type="String" />
            <asp:Parameter Name="original_Business_Phone" Type="String" />
            <asp:Parameter Name="original_MobilePhone" Type="String" />
            <asp:Parameter Name="original_Fax_Number" Type="String" />
            <asp:Parameter Name="original_Address" Type="String" />
            <asp:Parameter Name="original_City" Type="String" />
            <asp:Parameter Name="original_State" Type="String" />
            <asp:Parameter Name="original_ZIP" Type="String" />
            <asp:Parameter Name="original_Notes" Type="String" />
            <asp:Parameter Name="original_Attachments" Type="String" />
            <asp:Parameter Name="original_AppointmentSet" Type="DateTime" />
            <asp:Parameter Name="original_Director" Type="String" />
            <asp:Parameter Name="original_upsize_ts" Type="Object" />
            <asp:Parameter DbType="Date" Name="original_Created" />
            <asp:Parameter Name="original_Contact" Type="String" />
            <asp:Parameter Name="original_Status" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Company" Type="String" />
            <asp:Parameter Name="LastName" Type="String" />
            <asp:Parameter Name="FirstName" Type="String" />
            <asp:Parameter Name="EmailAddress" Type="String" />
            <asp:Parameter Name="BusPhone" Type="String" />
            <asp:Parameter Name="Business_Phone" Type="String" />
            <asp:Parameter Name="MobilePhone" Type="String" />
            <asp:Parameter Name="Fax_Number" Type="String" />
            <asp:Parameter Name="Address" Type="String" />
            <asp:Parameter Name="City" Type="String" />
            <asp:Parameter Name="State" Type="String" />
            <asp:Parameter Name="ZIP" Type="String" />
            <asp:Parameter Name="Notes" Type="String" />
            <asp:Parameter Name="Attachments" Type="String" />
            <asp:Parameter Name="AppointmentSet" Type="DateTime" />
            <asp:Parameter Name="Director" Type="String" />
            <asp:Parameter Name="upsize_ts" Type="Object" />
            <asp:Parameter DbType="Date" Name="Created" />
            <asp:Parameter Name="Contact" Type="String" />
            <asp:Parameter Name="Status" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Company" Type="String" />
            <asp:Parameter Name="LastName" Type="String" />
            <asp:Parameter Name="FirstName" Type="String" />
            <asp:Parameter Name="EmailAddress" Type="String" />
            <asp:Parameter Name="BusPhone" Type="String" />
            <asp:Parameter Name="Business_Phone" Type="String" />
            <asp:Parameter Name="MobilePhone" Type="String" />
            <asp:Parameter Name="Fax_Number" Type="String" />
            <asp:Parameter Name="Address" Type="String" />
            <asp:Parameter Name="City" Type="String" />
            <asp:Parameter Name="State" Type="String" />
            <asp:Parameter Name="ZIP" Type="String" />
            <asp:Parameter Name="Notes" Type="String" />
            <asp:Parameter Name="Attachments" Type="String" />
            <asp:Parameter Name="AppointmentSet" Type="DateTime" />
            <asp:Parameter Name="Director" Type="String" />
            <asp:Parameter Name="upsize_ts" Type="Object" />
            <asp:Parameter Name="Created" DbType="Date" />
            <asp:Parameter Name="Contact" Type="String" />
            <asp:Parameter Name="Status" Type="String" />
            <asp:Parameter Name="original_ID" Type="Int32" />
            <asp:Parameter Name="original_Company" Type="String" />
            <asp:Parameter Name="original_LastName" Type="String" />
            <asp:Parameter Name="original_FirstName" Type="String" />
            <asp:Parameter Name="original_EmailAddress" Type="String" />
            <asp:Parameter Name="original_BusPhone" Type="String" />
            <asp:Parameter Name="original_Business_Phone" Type="String" />
            <asp:Parameter Name="original_MobilePhone" Type="String" />
            <asp:Parameter Name="original_Fax_Number" Type="String" />
            <asp:Parameter Name="original_Address" Type="String" />
            <asp:Parameter Name="original_City" Type="String" />
            <asp:Parameter Name="original_State" Type="String" />
            <asp:Parameter Name="original_ZIP" Type="String" />
            <asp:Parameter Name="original_Notes" Type="String" />
            <asp:Parameter Name="original_Attachments" Type="String" />
            <asp:Parameter Name="original_AppointmentSet" Type="DateTime" />
            <asp:Parameter Name="original_Director" Type="String" />
            <asp:Parameter Name="original_upsize_ts" Type="Object" />
            <asp:Parameter DbType="Date" Name="original_Created" />
            <asp:Parameter Name="original_Contact" Type="String" />
            <asp:Parameter Name="original_Status" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
	</div>
		
	</asp:Content>