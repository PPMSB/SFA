<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="DotNet.Setting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <title>Setting</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/settingg.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Setting</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
                <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
                <%--<a href="EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>--%>
                <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
                <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
                <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
                <%--<a href="SignboardEquipment.aspx" id="SignboardTag2" runat="server">Sign & Equip</a>--%>
                <div class="DropDown">
                    <button type="button" class="DDBtn">
                        Others
                        <i class="fa fa-caret-down"></i>
                    </button>
                    <div class="DropDownList">
                        <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>
                        <a href="Map3.aspx" id="MapTag2" runat="server" visible="false">Map</a>
                        <a href="NewProductHomePage.aspx" id="NewProduct2" runat="server" visible="false">New Product Request</a>
                    </div>
                </div>
                <a href="RocTin.aspx" id="RocTinTag2" runat="server" visible="false">Roc&Tin</a>
                <a href="Setting.aspx" id="SettingTag2" class="active" runat="server">Setting</a>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>

                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <div class="global_container" onclick=" myFunction(this);">
                        <div class="bar1"></div>
                        <div class="bar2"></div>
                        <div class="bar3"></div>
                    </div>
                </a>
                <script src="scripts/top_navigation.js"></script>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!--==============================================================================-->
            <div class="col-12 col-s-12" style="text-align: center">
                <div id="admin" class="col-12" visible="false" runat="server">
                    <div class="button-group d-flex justify-content-between">
                        <asp:Button ID="Button_Report" runat="server" Text="PPM Report" class="btn btn-primary frame_style_type2" OnClick="Button_Report_Click" OnClientClick="return confirm('Are you sure to generate PPM Sales Report?');" />

                        <asp:Button ID="ButtonReport_Group" runat="server" Text="PPM Group Report" class="btn btn-primary frame_style_type2" OnClick="ButtonReport_Group_Click" OnClientClick="return confirm('Are you sure to generate the PPM Group Sales Report?');" />

                        <asp:Button ID="ButtonMonthlyReport" runat="server" class="btn btn-info frame_style_type2" Text="PPM Monthly Report" OnClick="ButtonMonthlyReport_Click" OnClientClick="return confirm('Are you sure to generate the PPM Monthly Sales Report?');" />

                        <asp:Button ID="ButtonReport_PBM" runat="server" Text="PBM Report" class="btn btn-success frame_style_type2" OnClick="ButtonReport_PBM_Click" OnClientClick="return confirm('Are you sure to generate PBM Sales Report?');" />

                        <asp:Button ID="ButtonReportGroup_PBM" runat="server" Text="PBM Group Report" class="btn btn-success frame_style_type2" OnClick="ButtonReportGroup_PBM_Click" OnClientClick="return confirm('Are you sure to generate the PBM Group Sales Report?');" />

                        <asp:Button ID="ButtonMonthlyReport_PBM" runat="server" class="btn btn-info frame_style_type2" Text="PBM Monthly Report" OnClick="ButtonMonthlyReport_PBM_Click" OnClientClick="return confirm('Are you sure to generate the PBM Monthly Sales Report?');" />

                        <asp:Button ID="ButtonYTDReport" runat="server" Text="YTD Report" class="btn btn-info frame_style_type2" OnClick="ButtonYTDReport_Click" OnClientClick="return confirm('Are you sure to generate the YTD Sales Report?');" />

                        <asp:Button ID="ButtonToPoonsh" runat="server" Text="PPM Report for Poon" class="btn btn-primary frame_style_type2" OnClick="Button_PoonshReport_Click" OnClientClick="return confirm('Are you sure to generate PPM Sales Report?');" />
                    </div>
                </div>

                <!--==============================================================================-->
                <a href="EditProfile.aspx" id="EditProfileTag" runat="server" style="text-decoration: none; border: 0;">
                    <div class="image_properties">
                        <img src="RESOURCES/EditProfile.png" class="menuimage_nohighlight" alt="Edit Profile" />
                        <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Edit Profile" />
                    </div>
                </a>

                <a href="AddUser.aspx" id="AddUserTag" runat="server" style="text-decoration: none; border: 0;">
                    <div class="image_properties">
                        <img src="RESOURCES/AddUser.png" class="menuimage_nohighlight" alt="Add User" />
                        <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Add User" />
                    </div>

                </a>
                <!--==============================================================================-->
                <!--==============================================================================-->

                <a href="AddUser.aspx" id="AddUserTagRed" runat="server" style="text-decoration: none; border: 0;">
                    <div class="image_properties">
                        <img src="RESOURCES/AddUser.png" class="menuimage_nohighlight" alt="Add User Red" />
                        <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Add User Red" />
                    </div>
                </a>
                <!--==============================================================================-->

                <a href="ApprovalList.aspx" id="ApprovalGroup" runat="server" style="text-decoration: none; border: 0;">
                    <div class="image_properties">
                        <img src="RESOURCES/WARRANTYAPPROVAL.png" class="menuimage_nohighlight" alt="Add User Red" />
                        <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Add User Red" />
                    </div>
                </a>

                <!--==============================================================================-->

                <a href="RedempApprovalList.aspx" id="RedempApprovalGroup" runat="server" style="text-decoration: none; border: 0;">
                    <div class="image_properties">
                        <img src="RESOURCES/REDEMPAPPROVAL.png" class="menuimage_nohighlight" alt="Add User Red" />
                        <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Add User Red" />
                    </div>
                </a>
                <!--==============================================================================-->
               
                <a href="SignboardEquipment_ApprovalList.aspx" id="SignEquipApprovalGroup" runat="server" style="text-decoration: none; border: 0;">
                    <div class="image_properties">
                        <img src="RESOURCES/SIGN&EQUIPAPPROVAL.png" class="menuimage_nohighlight" alt="Add User Red" />
                        <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Add User Red" />
                    </div>
                </a>
            </div>
        </div>
    </form>
</body>
</html>
