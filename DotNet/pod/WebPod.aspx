<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="WebPod.aspx.cs" Inherits="DotNet.pod.WebPod" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="../RESOURCES/LFIB_icon.ico" />

    <script src="../scripts/GoToTab.js"></script>
    <link href="../STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="../STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="../STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <title>Picking Confirmation</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="../scripts/BrowserReload_ThroughHistory.js"></script>
</head>
<body>
    <form id="form1" runat="server">
         <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="../RESOURCES/WebPOD.jpg" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Picking Confirmation</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="../STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="../MainMenu.aspx">Home</a>
                <a href="../CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="../SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="../SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="../Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
                <a href="../Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
                <%--<a href="../EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>--%>
                <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
                <a href="../InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
                <a href="../WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
            <div class="DropDown">
                <button type="button" class="DDBtn">
                    Others
                        <i class="fa fa-caret-down"></i>
                </button>
                <div class="DropDownList">
                    <a href="../EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>
                    <a href="../Map3.aspx" id="MapTag2" runat="server" visible="false">Map</a>
                    <a href="../NewProductHomePage.aspx" id="NewProduct2" runat="server" visible="false">New Product Request</a>
                </div>
            </div>
                <a href="../RocTin.aspx" id="RocTinTag2" runat="server" visible="false">Roc&Tin</a>
                <a href="../Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                <a href="WebPod.aspx" id="WebPodTag2" class="active" runat="server">Picking Confirmation</a>
                <a href="../LoginPage.aspx" class="Log_Out top_nav_logout_properties">
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
                <script src="../scripts/top_navigation.js"></script>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="../RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>


        <div class="col-8 col-s-12">
            <div class="col-2 col-s-12"><asp:Label ID="Label1" runat="server" Text="Scan:" CssClass="labeltext"></asp:Label></div>
            <asp:TextBox ID="textBoxBarcode" runat="server" AutoPostBack="true" OnTextChanged="textBoxBarcode_TextChanged"></asp:TextBox>
            <asp:HiddenField ID="getScannedData" runat="server" />
            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />
            
        </div>

        <br />

        <div class="col-8 col-s-12">
            <div class="col-2 col-s-12"><asp:Label ID="pickingIDLbl" runat="server" Text="Picking ID:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getPickingID" runat="server"></asp:Label></div>
        </div>

        <div class="col-8 col-s-12">
            <div class="col-2 col-s-12"><asp:Label ID="confirmDateLbl" runat="server" Text="Confirm Date Time:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getConfirmDateTime" runat="server"></asp:Label></div>
        </div>

        <%--<div class="col-8 col-s-12">
            <div class="col-2 col-s-12"><asp:Label ID="confirmTimeLbl" runat="server" Text="Confirm Time:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getConfirmTime" runat="server"></asp:Label></div>
        </div>--%>

        <div class="col-8 col-s-12">
            <div class="col-2 col-s-12"><asp:Label ID="confirmByLbl" runat="server" Text="Confirm By:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getConfirmBy" runat="server"></asp:Label></div>
        </div>

        <br />

         <div class="col-8 col-s-12" id="unconfirmDateDiv" runat="server" visible="false">
            <div class="col-2 col-s-12"><asp:Label ID="unconfirmDateLbl" runat="server" Text="Unconfirm Date Time:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getUnconfirmDateTime" runat="server"></asp:Label></div>
        </div>

        <%--<div class="col-8 col-s-12" id="unconfirmTimeDiv" runat="server" visible="false">
            <div class="col-2 col-s-12"><asp:Label ID="unconfirmTimeLbl" runat="server" Text="Unconfirm Time:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getUnconfirmTime" runat="server"></asp:Label></div>
        </div>--%>

        <div class="col-8 col-s-12" id="unconfirmByDiv" runat="server" visible="false">
            <div class="col-2 col-s-12"><asp:Label ID="unconfirmByLbl" runat="server" Text="Unconfirm By:" CssClass="labeltext"></asp:Label></div>
            <div class="col-6 col-s-12"><asp:Label ID="getUnconfirmBy" runat="server"></asp:Label></div>
        </div>

        <!--<div>
            <asp:Button ID="Button2" runat="server" Text="Sync" OnClick="btnSync_Click" />
        </div>-->
        </div>
    </form>
</body>
</html>

