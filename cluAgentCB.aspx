<%@ Page Title="PLMS - Customer lookup" Language="C#" AutoEventWireup="true" CodeFile="cluAgentCB.aspx.cs" Inherits="_cluAgentCB" %>
<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search for Call Backs by Agent</title>
    <link href="styles/style.css" rel="stylesheet" />
    <style type="text/css">
        .modalBackground { background-color: Gray; z-index: 10000; }
        .ob_iCboIC, .ob_iDdlIC { z-index: 100002 !important; }
        .ob_iDdlICBC li { float: left; width: 125px; }
    </style>
    <script type="text/javascript">
        function Print() {
            var printWin = window.open('', '', 'left=0,top=0,width=1000,height=800,status=0');
            printWin.document.write(document.getElementById("<%=dvContent.ClientID %>").outerHTML);
            printWin.document.close();
            printWin.focus();
            printWin.print();
            printWin.close();
        }
        window.onerror = function (msg, url, num) {
            if (msg) {
                var isAddHandlerException = msg.indexOf('Handler was not added through the Sys.UI.DomEvent.addHandler method.') !== -1
                                            || msg.indexOf('b._events is undefined') !== -1;
                return isAddHandlerException;
            }
        }
        window.onload = function () {
            Obout.Interface.OboutCore.getLeft = function (element) {
                var position = $common.getLocation(element);
                return position.x;
            }
            Obout.Interface.OboutCore.getTop = function (element) {
                var position = $common.getLocation(element);
                return position.y;
            }
        }
    </script>
</head>
<body>
    <h2>Sales Lead Database</h2>
    <h3>Call Backs by Agent</h3>
    <!-- LoginView removed: authentication disabled -->
        <RoleGroups>
            <asp:RoleGroup Roles="Administrators">
                <ContentTemplate>
                    As an Administrator, you may edit and delete user accounts. Remember: With great power comes great responsibility!
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="Supervisors">
                <ContentTemplate>
                    As a Supervisor, you may edit users' Email and Comment information. Simply click the Edit button, make your changes, and then click Update.
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="Agent">
                <ContentTemplate>
                    As an Agent, you may view and edit the Sales Leads that are stored in the database that belong to your UserId.
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="Call Agent">
                <ContentTemplate>
                    As a Call Agent you may view and edit All Sales leads for all Agents.
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
        <LoggedInTemplate>
            You are not a member of any user roles. Therefore you cannot view, edit or delete any Sales Lead information.
        </LoggedInTemplate>
        <AnonymousTemplate>
            You are not logged into the system. Therefore you cannot view, edit or delete any Sales Lead information.
        </AnonymousTemplate>
    </asp:LoginView>
    <form id="form1" runat="server">
        <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release" />
            <div style="width: 275px; height: 66px">
                <asp:Label ID="Label1" runat="server" Width="100px" />
                <br /><br />
                <asp:DropDownList ID="DropDownList1" runat="server" ToolTip="Select the agent for this view">
                    <asp:ListItem>Select One</asp:ListItem>
                    <asp:ListItem>VPP Lead</asp:ListItem>
                    <asp:ListItem>VPW Web Lead</asp:ListItem>
                    <asp:ListItem>VPP Sharon Stangler</asp:ListItem>
                    <asp:ListItem>VPP Richard Stangler</asp:ListItem>
                    <asp:ListItem>Asher Lead</asp:ListItem>
                    <asp:ListItem>Asher Sharon Stangler</asp:ListItem>
                    <asp:ListItem>Asher Richard Stangler</asp:ListItem>
                    <asp:ListItem>GC Lead</asp:ListItem>
                    <asp:ListItem>GC Sharon Stangler</asp:ListItem>
                    <asp:ListItem>GC Richard Stangler</asp:ListItem>
                    <asp:ListItem>Stangler Lead</asp:ListItem>
                    <asp:ListItem>Sharon Stangler</asp:ListItem>
                    <asp:ListItem>Richard Stangler</asp:ListItem>
                    <asp:ListItem>Mary Jo Hudson</asp:ListItem>
                    <asp:ListItem>Amy Wallace</asp:ListItem>
                    <asp:ListItem>CTS</asp:ListItem>
                    <asp:ListItem>Web Lead</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:DropDownList ID="DropDownList2" runat="server" ToolTip="Select the callback status for this view">
                    <asp:ListItem>Call Back</asp:ListItem>
                    <asp:ListItem>Call Back - Jan</asp:ListItem>
                    <asp:ListItem>Call Back - Feb</asp:ListItem>
                    <asp:ListItem>Call Back - Mar</asp:ListItem>
                    <asp:ListItem>Call Back - Apr</asp:ListItem>
                    <asp:ListItem>Call Back - May</asp:ListItem>
                    <asp:ListItem>Call Back - Jun</asp:ListItem>
                    <asp:ListItem>Call Back - Jul</asp:ListItem>
                    <asp:ListItem>Call Back - Aug</asp:ListItem>
                    <asp:ListItem>Call Back - Sep</asp:ListItem>
                    <asp:ListItem>Call Back - Oct</asp:ListItem>
                    <asp:ListItem>Call Back - Nov</asp:ListItem>
                    <asp:ListItem>Call Back - Dec</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Button ID="Button1" runat="server" Text="Search" />
                <br />
            </div>
            <br />
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource1" OnRowCreated="OnRowCreated">
                <RowStyle BackColor="#EFF3FB" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="~/Edit.jpg" Text="Edit" Width="25" Height="25" OnClick="ImageButton1_Click" Visible="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/delete.png" Text="Delete" Width="25" Height="25" OnClick="ImageButton2_Click" Enabled="true" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="False" />
                    <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" NullDisplayText='None' />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" NullDisplayText='None' />
                    <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Date Created" SortExpression="Created" NullDisplayText=' ' />
                    <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" NullDisplayText=' ' />
                    <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" NullDisplayText=' ' />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" NullDisplayText=' ' />
                    <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" NullDisplayText=' ' />
                    <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" NullDisplayText=' ' />
                    <asp:BoundField DataField="ZIP" HeaderText="ZIP" SortExpression="ZIP" NullDisplayText=' ' />
                    <asp:BoundField DataField="HomePhone" HeaderText="HomePhone" SortExpression="HomePhone" NullDisplayText=' ' />
                    <asp:BoundField DataField="MobilePhone" HeaderText="Mobile Phone" SortExpression="MobilePhone" NullDisplayText=' ' />
                    <asp:BoundField DataField="EmailAddress" HeaderText="EMailAddress" SortExpression="EMailAddress" NullDisplayText=' ' />
                    <asp:BoundField DataField="Branch" HeaderText="Branch" SortExpression="Branch" NullDisplayText='Unknown' />
                    <asp:BoundField DataField="HDischarge" HeaderText="HDischarge" SortExpression="HDischarge" NullDisplayText='Unknown' />
                    <asp:BoundField DataField="Ages" HeaderText="Ages" SortExpression="Ages" NullDisplayText=' ' />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" NullDisplayText=' ' />
                    <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:g}" HeaderText="AppointmentSet" SortExpression="AppointmentSet" NullDisplayText=' ' />
                    <asp:BoundField DataField="ApptSetter" HeaderText="Appt Setter" SortExpression="ApptSetter" NullDisplayText='None' />
                    <asp:BoundField DataField="AppointmentSet" DataFormatString="{0:g}" HeaderText="CTS Only" ReadOnly="True" Visible="True" NullDisplayText=' ' />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblresult" runat="server" />
            <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup" BackgroundCssClass="modalBackground" />
            <asp:Label runat="server" ID="LDate" Visible="False" />
            <asp:Label runat="server" ID="CDate" Visible="False" />
            <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="700px" style="display:none">
                <div id="dvContent" runat="server">
                    <table width="100%" style="border:Solid 3px #D55500; width:100%; height:100%" cellpadding="0" cellspacing="0">
                        <tr style="background-color:#D55500">
                            <td colspan="2" style="height:10%; color:White; font-weight:bold; font-size:larger" align="center">Customer Details</td>
                        </tr>
                        <tr>
                            <td align="right" style="width:45%">Record Id:</td>
                            <td><asp:Label ID="lblID" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right">First Name:</td>
                            <td><asp:TextBox ID="txtfname" runat="server" ToolTip="Enter first name" /></td>
                        </tr>
                        <tr>
                            <td align="right">Last Name:</td>
                            <td><asp:TextBox ID="txtlname" runat="server" ToolTip="Enter last name" /></td>
                        </tr>
                        <tr>
                            <td align="right">Address:</td>
                            <td><asp:TextBox ID="txtadd" runat="server" ToolTip="Enter an address" /></td>
                        </tr>
                        <tr>
                            <td align="right">City:</td>
                            <td><asp:TextBox ID="txtcity" runat="server" ToolTip="Enter a city name" /></td>
                        </tr>
                        <tr>
                            <td align="right">State:</td>
                            <td><asp:TextBox ID="txtstate" runat="server" ToolTip="Enter a two letter state" /></td>
                        </tr>
                        <tr>
                            <td align="right">Zip:</td>
                            <td><asp:TextBox ID="txtzip" runat="server" ToolTip="Enter a valid zip code" /></td>
                        </tr>
                        <tr>
                            <td align="right">Home Phone:</td>
                            <td>
                                <asp:TextBox ID="txthphone" runat="server" CausesValidation="True" ToolTip="Enter a valid 10 digit phone # with dashes" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Must be a valid Phone # ie. 555-555-1212 with dashes" ControlToValidate="txthphone" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Mobile Phone:</td>
                            <td>
                                <asp:TextBox ID="txtmphone" runat="server" CausesValidation="True" ToolTip="Enter a valid 10 digit phone # with dashes" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Must be a valid Phone # ie. 555-555-1212 with dashes" ControlToValidate="txtmphone" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Email Address:</td>
                            <td>
                                <asp:TextBox ID="txtemail" runat="server" CausesValidation="True" ToolTip="Enter a valid email address" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="A valid email address must be entered ie. example@theveteranprogram.com" ControlToValidate="txtemail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Branch:</td>
                            <td>
                                <obout:OboutDropDownList ID="txtbranch" runat="server" ToolTip="Select a branch of service if known">
                                    <asp:ListItem>Air Force</asp:ListItem>
                                    <asp:ListItem>Army</asp:ListItem>
                                    <asp:ListItem>Coast Guard</asp:ListItem>
                                    <asp:ListItem>Marines</asp:ListItem>
                                    <asp:ListItem>Navy</asp:ListItem>
                                    <asp:ListItem>Space Force</asp:ListItem>
                                    <asp:ListItem>Unknown</asp:ListItem>
                                </obout:OboutDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Honorable Discharge:</td>
                            <td>
                                <obout:OboutDropDownList ID="txthdischarge" runat="server" ToolTip="Select discharge status if known">
                                    <asp:ListItem>Yes</asp:ListItem>
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Unknown</asp:ListItem>
                                </obout:OboutDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Ages:</td>
                            <td><asp:TextBox ID="txtages" runat="server" ToolTip="Enter ages if known" /></td>
                        </tr>
                        <tr>
                            <td align="right">Notes:</td>
                            <td><asp:TextBox ID="txtnotes" runat="server" ToolTip="Enter notes here" /></td>
                        </tr>
                        <tr>
                            <td align="right">Appt Date &amp; Time:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDate" ToolTip="Using the popup calendar select a date and time for the appointment" />
                                <obout:Calendar ID="Calendar1" runat="server" ShowTimeSelector="true" DateFormat="MM/dd/yyyy hh:mm" DatePickerMode="true" TextBoxId="txtDate" ShowSecondSelector="False" TimeSelectorType="DropDownList" DatePickerImagePath="../images/icon2.gif" >
                                </obout:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Agent:</td>
                            <td>
                                <obout:OboutDropDownList ID="txtagent" runat="server" ToolTip="Select the agent for this record" >
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
                                    <asp:ListItem>GC Lead</asp:ListItem>
                                    <asp:ListItem>GC Sharon Stangler</asp:ListItem>
                                    <asp:ListItem>GC Richard Stangler</asp:ListItem>
                                    <asp:ListItem>CTS</asp:ListItem>
                                    <asp:ListItem>Web Lead</asp:ListItem>
                                </obout:OboutDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Appointment Setter:</td>
                            <td>
                                <obout:OboutDropDownList ID="txtapptset" runat="server" ToolTip="Select an appointment setter if applicable">
                                    <asp:ListItem>None</asp:ListItem>
                                    <asp:ListItem>Michelle McDonough</asp:ListItem>
                                    <asp:ListItem>Outside Agent</asp:ListItem>
                                </obout:OboutDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Lead Status:</td>
                            <td>
                                <obout:OboutDropDownList runat="server" ID="txtstatus" MenuWidth="500" 
                                    DataSourceID="sds1" DataTextField="Status" DataValueField="Status"
                                    AppendDataBoundItems="true" FolderStyle="styles/grand_gray/OboutDropDownList"
                                    >
                                    <asp:ListItem>Select a Status ...</asp:ListItem>
                                </obout:OboutDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" OnClick="btnUpdate_Click"/>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                <input type="button" value="Print" onclick="Print()" />
                            </td>
                        </tr>
                    </table>
                    <asp:HyperLink runat="server" ID="lnkPartner" Text="Return to Partner Pages" NavigateUrl="~/default.aspx"></asp:HyperLink>
                </div>
            </asp:Panel>
            <asp:SqlDataSource ID="sds1" runat="server" SelectCommand="SELECT * FROM [StatusDDL] ORDER BY [Status] ASC" ConnectionString="<%$ ConnectionStrings:salespipeline %>" />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:salespipeline %>" 
                OldValuesParameterFormatString="original_{0}" 
                SelectCommand="SELECT * FROM [Customers] WHERE ([Agent] = @Agent) AND ([Status] = @Status) ORDER BY [CDate] DESC">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList1" Name="Agent" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="DropDownList2" Name="Status" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
