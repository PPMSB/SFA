
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visitor_GuestAppointment.aspx.cs" Inherits="DotNet.Visitor_GuestAppointment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="Content/form_custom.css" rel="stylesheet" />
    <link href="Content/custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />


    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.bundle.min.js"></script>

    <script src="scripts/jquery.min.js"></script>

    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <script src="../scripts/moment.min.js"></script>
    <script src="../scripts/bootstrap-datetimepicker.js"></script>


    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
    <script src="scripts/top_navigation.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="display: flex; justify-content: center; align-items: center; flex-direction: column;">
            <asp:Image ID="QRCodeImage" runat="server" Width="50%" Height="50%" />
            <img class="Company_Icon" alt="POSIM PETROLEUM MARKETING" src="RESOURCES/PPM%20bg%20white.gif" style="width: 46%;" />
        </div>
        
        <asp:Button ID="BtnRedirect" runat="server" OnClick="RedirectUserToCreateNew" Text="Redirect" class="glow_green button-margin" Enabled="true" />


<%--        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="upBatch">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>



<%--        <asp:UpdatePanel ID="upBatch" runat="server">
            <ContentTemplate>
                <div class="row mt-3">
                    <asp:ListView ID="ListViewAppointments" runat="server" OnItemDataBound="ListViewAppointments_ItemDataBound">
                        <ItemTemplate>

                            <div class="col">
                                <div class="card" >
                                    <div class="card-body">
                                        <h5 class="card-title"><%# Eval("AppointmentTitle") %></h5>
                                        <p class="card-text">Date: <%# Eval("Date") %></p>
                                        <p class="card-text">Status: <%# Eval("Status") %></p>
                                        <asp:Button ID="Button1" runat="server" Text="Check Out" CssClass="btn btn-primary"
                                            CommandArgument='<%# Eval("AppointmentID") %>' OnClick="RedirectUserToCreateNew" />
                                    </div>
                                </div>
                            </div>

                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
            <%--<button id="testing" class="btn btn-primary">Test Ajax</button>--%>

</body>
</html>