<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.aspx.cs" Inherits="DotNet.MainMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Flip_Image.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <title>Main Menu</title>

    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
<body>
    <form runat="server">
        <div class="col-6 col-s-6 LION_POSIM">

            <div class="mobile_hidden">
                <%--                    <div class="flip-box">--%>
                <%--                        <div class="flip-box-inner">--%>
                <%--                            <div class="flip-box-front">--%>
                <img alt="LION POSIM" src="RESOURCES/LFIB_logo3.jpg" width="253" height="90" />
                <%--                            </div>--%>
                <%--                            <div class="flip-box-back">
                                <h2>LION POSIM</h2>

                            </div>--%>
                <%--                        </div>--%>
                <%--                    </div>--%>
            </div>
            <div class="mobile_show">
                <img alt="LION POSIM" src="RESOURCES/LFIB_logo3.jpg" width="165" height="90" />
            </div>
        </div>

        <div class="col-6 col-s-6 Label_UsernameBlock">
            <asp:Label ID="Label1" class="label_username" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="Label3" CssClass="label_desc" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="Label_Red" CssClass="label_desc" runat="server" Text="" Visible="false" Style="color: red"></asp:Label>
        </div>
        <!--==============================================================================-->
        <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
        <div class="topnav" id="myTopnav">

            <a href="MainMenu.aspx" class="active">Home</a>
            <div class="DropDown">
                <button type="button" class="DDBtn">
                    Customer
                        <i class="fa fa-caret-down"></i>
                </button>
                <div class="DropDownList">
                    <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">List</a>
                    <a href="#" id="NewCustomerTag2" runat="server">New</a>
                </div>
            </div>

            <%--            <a href="NewCustomer.aspx" id="NewCustomerTag2" runat="server" visible="false">New Customer</a>
            <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>--%>
            <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
            <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
            <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
            <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
            <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
           <%-- <a href="EOR.aspx" id="A1" runat="server" visible="false">EOR</a>--%>
            <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
            <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
            <a href="RocTin.aspx" id="RocTinTag2" runat="server" visible="false">Roc&Tin</a>
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
            <%--            <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>
            <a href="Map3.aspx" id="MapTag2" runat="server" visible="false">Map</a>--%>
            <a href="pod/WebPod.aspx" id="PickingConfirmation" runat="server">Picking Confirmation</a>
            <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>

            <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                <%--                <i class="fa fa-sign-out" aria-hidden="true" style="font-size:35px"></i>--%>
                <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                <%--                    <img src="RESOURCES/LogoutIcon.png" /> Logout--%>
                <%--                    <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
            </a>
            <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                <%--                                <img src="RESOURCES/menu.png" />--%>
                <div class="global_container" onclick="myFunction(this);">
                    <div class="bar1"></div>
                    <div class="bar2"></div>
                    <div class="bar3"></div>
                </div>
            </a>
            <script src="scripts/top_navigation.js"></script>

        </div>

        <!--==============================================================================-->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <div class="col-12 col-s-12" style="text-align: center">
            <!--==============================================================================-->
            <a href="CheckInRecord.aspx" id="CheckInTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/CHECKINN.png" class="menuimage_nohighlight" alt="Customer" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Customer" />
                </div>
            </a>

            <a href="NewCustomer.aspx" id="NewCustomerTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/New_Customer.png" class="menuimage_nohighlight" alt="NewCustomer" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="NewCustomer" />
                </div>
            </a>

            <a href="CustomerMaster.aspx" id="CustomerMasterTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/Customert.png" class="menuimage_nohighlight" alt="Customer" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Customer" />
                </div>
            </a>

            <a href="SFA.aspx" id="SFATag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/SALES FORCE.png" class="menuimage_nohighlight" alt="Sales" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Sales" />
                </div>
            </a>
            <a href="SalesQuotation.aspx" id="SalesQuotation" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/SQ.png" class="menuimage_nohighlight" alt="Sales Quotation" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Sales Quotation" />
                </div>
            </a>
            <!--payment-->
            <a href="Payment.aspx" id="PaymentTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/PAYMENTT.png" class="menuimage_nohighlight" alt="Payment" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Payment" />
                </div>
            </a>
            <!--Redemption-->
            <a href="Redemption.aspx" id="RedemptionTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/Redemptionn.png" class="menuimage_nohighlight" alt="Redemption" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Redemption" />
                </div>
            </a>

            <!--EOR-->
            <!--Signboard & Equipment-->
            <a href="SignboardEquipment.aspx" id="EORTag" runat="server" style="text-decoration: none; border: 0;" visible="true">
                <div class="image_properties">
                    <img src="RESOURCES/Equipment.png" class="menuimage_nohighlight" alt="EOR" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="EOR" />
                </div>
            </a>

            <!--InventoryMaster-->
            <a href="InventoryMaster.aspx" id="InventoryMasterTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/Inventoryy.png" class="menuimage_nohighlight" alt="Inventory" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Inventory" />
                </div>
            </a>
            <!--WClaim-->
            <a href="WClaim.aspx" id="WClaimTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/WARRANTY CLAIM.png" class="menuimage_nohighlight" alt="WClaim" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="WClaim" />
                </div>
            </a>

            <!--EventBudget-->
            <a href="EventBudget.aspx" id="EventBudgetTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/EVENTBUDGET.png" class="menuimage_nohighlight" alt="EventBudget" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="EventBudget" />
                </div>
            </a>

            <!--Location-->
            <a href="Map3.aspx" id="MapTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/LOCATION.png" class="menuimage_nohighlight" alt="Location" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Location" />
                </div>
            </a>

            <!--RocTin-->
            <a href="RocTin.aspx" id="RocTinTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/EInvoice.png" class="menuimage_nohighlight" alt="RocTin" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="RocTin" />
                </div>
            </a>

            <!--Campaign-->
            <a href="Campaign.aspx" id="CampaignTag" runat="server" style="text-decoration: none; border: 0;" visible="false">
                <div class="image_properties">
                    <img src="RESOURCES/Campaign.png" class="menuimage_nohighlight" alt="Campaign" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="RocTin" />
                </div>
            </a>

            <!--Picking Confirmation-->
            <a href="pod/WebPod.aspx" id="PickingConfirmationTag" runat="server" style="text-decoration: none; border: 0;" visible="true">
                <div class="image_properties">
                    <img src="RESOURCES/WebPOD.jpg" class="menuimage_nohighlight" alt="Picking Confirmation" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Picking Confirmation" />
                </div>

            </a>

            <!--Setting-->
            <a href="Setting.aspx" id="SettingTag" runat="server" style="text-decoration: none; border: 0;" visible="true">
                <div class="image_properties">
                    <img src="RESOURCES/settingg.png" class="menuimage_nohighlight" alt="Setting" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Setting" />
                </div>

            </a>

            <!--Visitor Management-->
            <%--  (Uncomment) --%>
            <%--            <div class="image_properties" id="VisitorMnagementURL" style="cursor: pointer">
                <img src="RESOURCES/VMS.png" class="menuimage_nohighlight" alt="Setting" style="border: 1px solid black; border-radius: 5px;" />
                <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Setting" />
            </div>--%>

            <asp:LinkButton ID="btnLicenseRenew" runat="server" OnClick="btnLicenseRenew_Click" Style="text-decoration: none; border: 0;">
                <div class="image_properties">
                    <img src="RESOURCES/License.png" class="menuimage_nohighlight" alt="License" style="border: 1px solid black; border-radius: 5px;" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="License Renew" />
                </div>
            </asp:LinkButton>

            <!--Incentive Campaign-->
            <a href="Campaign_MainMenu.aspx" id="CampaignMainMenuURL" runat="server" style="text-decoration: none; border: 0;" visible="true">
                <div class="image_properties">
                    <img src="RESOURCES/Campaign.png" class="menuimage_nohighlight" alt="Setting" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Setting" />
                </div>

            </a>
            <!--Signboard & Equipment-->
            <%--<a href="SignboardEquipment.aspx" id="SignboardEquipmentURL" runat="server" style="text-decoration: none; border: 0;" visible="true">
                <div class="image_properties">
                    <img src="RESOURCES/Equipment.png" class="menuimage_nohighlight" alt="Setting" />
                    <img src="RESOURCES/HOVERFRAME.png" class="image_highlight" alt="Setting" />
                </div>

            </a>--%>

            <asp:Button ID="btnTest" runat="server" Text="text" OnClick="btnTest_Click" Visible="false" />
        </div>

    </form>
    <div class="site-footer-container">
        <p>
            All Rights Reserved By Lion Posim Berhad 198201002310 (82056-X) &copy; Copyright                
                <script>document.write(new Date().getFullYear())</script>
            <br />
            <a href="http://www.posim.com.my/LFIBMainPage.nsf/termOfUse?openform" style="text-decoration: none;">Term of Use</a>
            &nbsp; | &nbsp;
                <a href="http://www.posim.com.my/privacy.nsf/privacy" style="text-decoration: none;">Privacy</a>
        </p>
    </div>

</body>

<script>

    $(document).ready(function () {
        $("#VisitorMnagementURL").on('click', function () {
            var userName = '<%= Session["user_id"] %>';
                 var productionWebsite = '<%= GLOBAL_VAR.GLOBAL.ProductionWebsite %>';

                 $.ajax({
                     type: "POST",
                     url: "/Visitor_MainMenu.aspx/RedirectToMainMenu",
                     data: JSON.stringify({ UserName: userName }),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         window.location.href = productionWebsite + "Visitor_MainMenu.aspx";
                     }
                 });
             })

         })
</script>
</html>
