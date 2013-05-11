<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CallingSchedule.Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="stylesheet" type="text/css" href="Styles.css">
    <title>Direct Buy Calling Schedule</title>
</head>

<body bgcolor="#003e68">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:Timer runat="server" id="UpdateTimer" interval="1000" ontick="UpdateTimer_Tick" />
        <asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger controlid="UpdateTimer" eventname="Tick" />
            </Triggers>
            <ContentTemplate>
                <table width="400px" border="0" cellpadding="4">
                    <tr>
                        <td class="timer_header" align="right" colspan="2">
                            <asp:Panel ID="Panel1" Width="100%" Height="50px" runat="server" BackColor="#0066CC" BorderStyle="Outset" 
                                BorderColor="White" BorderWidth="1px" Direction="RightToLeft" ForeColor="Black">
                            <asp:Label runat="server" id="lblDate" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="timer_title"><asp:Label runat="server" id="lblRemoteTimeLabel" Visible="false" Text="Jacksonville Time:" /></td>
                        <td class="timer_diff" align="left" width="150px"><asp:Label runat="server" id="lblRemoteTime" /></td>
                    </tr>
                    <tr>
                        <td class="timer_title"><asp:Label runat="server" id="lblLocalTimeLabel" Visible="false" Text="Local Time:" /></td>
                        <td class="timer_diff" align="left" width="150px"><asp:Label runat="server" id="lblLocalTime" /></td>
                    </tr>
                    <tr>
                        <td class="timer_curr" align="left">                        
                            <asp:Panel ID="Panel2" Width="100%" Height="50px" runat="server" BackColor="#0066CC" BorderStyle="Outset" 
                                BorderColor="White" BorderWidth="1px" Direction="RightToLeft" ForeColor="Black">
                            <asp:Label runat="server" id="lblCurrentEvent" />
                            </asp:Panel>
                        </td>
                        <td class="timer_diff" align="left" width="150px">                    
                            <asp:Panel ID="Panel3" Width="100%" Height="50px" runat="server" BackColor="#0066CC" BorderStyle="Outset" 
                                BorderColor="White" BorderWidth="1px" Direction="RightToLeft" ForeColor="Black">
                            <asp:Label runat="server" id="lblTimeLeft" />
                            </asp:Panel></td>
                    </tr>
                    <tr>
                        <td class="timer_curr" align="left"><asp:Label runat="server" id="lblNextEventLabel" /></td>
                        <td class="timer_diff" align="left" width="150px"><asp:Label runat="server" id="lblNextEvent" /></td>
                    </tr>
                    <tr>
                        <td class="timer_title"><asp:Label runat="server" id="lblTotalTimeLabel" Visible="false" Text="Total Time:" /></td>
                        <td class="timer_diff" align="left" width="150px"><asp:Label runat="server" id="lblWorkLeft" /></td>
                    </tr>
                </table>              
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<br />
        <table width="350px" border="0" cellpadding="4">
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" CellPadding="4">
                    </asp:GridView>
                </td>
            </tr>
        </table>--%>
    </div>
    </form>
</body>
</html>
