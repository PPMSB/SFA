<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddExistCust.aspx.cs" Inherits="DotNet.AddExistCust" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="STYLES/New_Customer.css" rel="stylesheet" />


    <title>New Customer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="https://api.mapbox.com/mapbox-gl-js/v2.9.1/mapbox-gl.js"></script>
    <link href="https://api.mapbox.com/mapbox-gl-js/v2.9.1/mapbox-gl.css" rel="stylesheet" />
    <script defer src="scripts/gps-map.js"></script>

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    
    <script>
        function goBack() {
            window.history.back();
        }
    </script>

     
    <!--==============================================================================-->
   <header>

    <img src="RESOURCES/NewCustomer.png" class="menuimage_nohighlight" alt="Customer" />
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
        <div class="topnav" id="myTopnav">

            <a href="MainMenu.aspx" class="active">Home</a>
            <a href="NewCustomer.aspx" id="NewCustomer2" runat="server" visible="true">New Customer</a>
            <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
            <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales Automation</a>
            <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
            <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
            <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
            <%--<a href="EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>--%>
            <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
            <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
            <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
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
                <div class="container" onclick=" myFunction(this);">
                    <div class="bar1"></div>
                    <div class="bar2"></div>
                    <div class="bar3"></div>
                </div>
            </a>
            <script src="scripts/top_navigation.js"></script>
            </header>
</head>
<body>
<form id="form1" runat="server">
        <div class="ExistingCustomer">       
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="SalesmanName">Salesman Name:</asp:Label>
                        <asp:TextBox ID="SalesmanName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="Company">Company:</label>
                        <asp:TextBox ID="Company" runat="server"  ReadOnly="true" Text="Posim Petroleum Marketing Sdn Bhd"  />
                    </div>

                    <div class="form-group">
                        <label for="Branch">Branch:</label>
                        <asp:TextBox ID="Branch" runat="server"  ReadOnly="true" Text="Shah Alam"  />
                    </div>

                            <div class="form-group">
                            <label for="Code">Salesman Code:</label>
                            <asp:TextBox ID="TextBox1" runat="server" />
                        </div>
                 
                    <div class="form-group">
                        <label for="Status">Status:</label>
                        <asp:TextBox ID="status" runat="server" ReadOnly="true" Text="New" />
                        <label for="FormNumber">Form No:</label>
                        <asp:TextBox ID="FormNumber" runat="server" ReadOnly="true" Text="SYSTEM" />
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="CustName">Customer Name:</asp:Label>
                        <asp:TextBox ID="CustName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return submitForm()" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="goBack()" />
                    </div>

                    <div class="form-group">
                    <div id="output" runat="server"></div>
                    </div>
        </div>
    </form> 
</body>
</html>



