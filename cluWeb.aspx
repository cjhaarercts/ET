<%@ Page Title="PLMS - Customer lookup" Language="C#" AutoEventWireup="true" CodeFile="cluWeb.aspx.cs" Inherits="_cluAgentWeb" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="obout" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Search for Active website leads by Agent</title>
<style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        z-index: 10000;
    }
    .ob_iCboIC, .ob_iDdlIC
    {
         z-index: 100002 !important;
    }
    .ob_iDdlICBC li
    {
    float: left;
    width: 125px;
    }
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
    </script>
</head>
<body>
    <h2>Sales Lead Database</h2>
    <h3>Active Website Leads by Agent</h3>
    <!-- LoginView removed: authentication disabled -->
    <div class="auth-removed">Authentication removed; role-based messages removed.</div>
    <form id="form1" runat="server">
        <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ScriptMode="Release" />
            <div style="width: 275px; height: 66px">
                <asp:Label ID="Label1" runat="server" Width="100px" />
            </div>
        </div>
    </form>
</body>
</html>
