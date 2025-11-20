<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddExistingCustomer.aspx.cs" Inherits="DotNet.AddExistingCustomer" %>

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
            window.location.href = 'NewCustomer.aspx'; 
        }
    </script>

     
    <!--==============================================================================-->
   <header>

    <img src="RESOURCES/NewCustomer.png" class="menuimage_nohighlight" alt="Customer" />
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
        <div class="topnav" id="myTopnav">

            <a href="MainMenu.aspx" class="active">Home</a>
            <a href="New_Customer.aspx" id="NewCustomer2" runat="server" visible="true">New Customer</a>
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
      <div class="sidebar">
            <h2>Field</h2>
            <h3>Applicant</h3>
             <asp:Button ID="Button1" runat="server" Text="View Draft" CssClass="button" />
             <asp:Button ID="Button2" runat="server" Text="View All" CssClass="button" />
             <asp:Button ID="Button3" runat="server" Text="Outs. No Acc. Open" CssClass="button" />
             <asp:Button ID="Button4" runat="server" Text="View OutStanding" CssClass="button" />
             <asp:Button ID="Button5" runat="server" Text="Not Submitted" CssClass="button" />

            <h3>Credit Control</h3>
             <asp:Button ID="Button6" runat="server" Text="View Draft" CssClass="button" />
             <asp:Button ID="Button7" runat="server" Text="View All" CssClass="button" />
             <asp:Button ID="Button8" runat="server" Text="View OutStanding" CssClass="button" />
             <asp:Button ID="Button9" runat="server" Text="Not Submitted" CssClass="button" />
             <asp:Button ID="Button10" runat="server" Text="Awaiting Documents" CssClass="button" />
             <asp:Button ID="Button11" runat="server" Text="Document Received" CssClass="button" />
             <asp:Button ID="Button12" runat="server" Text="View KIV" CssClass="button" />
             <asp:Button ID="Button13" runat="server" Text="Branch Outstandung" CssClass="button" />
             <asp:Button ID="Button14" runat="server" Text="Branch Completed" CssClass="button" />
             <asp:Button ID="Button15" runat="server" Text="HQ Outstanding" CssClass="button" />
             <asp:Button ID="Button16" runat="server" Text="HQ Completed" CssClass="button" />

            <h3>Completed</h3>
             <asp:Button ID="Button17" runat="server" Text="Approved" CssClass="button" />
             <asp:Button ID="Button18" runat="server" Text="Rejected By HOD" CssClass="button" />
             <asp:Button ID="Button19" runat="server" Text="Rejected By CC" CssClass="button" />
             <asp:Button ID="Button20" runat="server" Text="Report Inquiry" CssClass="button" />
             <asp:Button ID="Button21" runat="server" Text="Monthly Report" CssClass="button" />
             <asp:Button ID="Button22" runat="server" Text="Data Lookup" CssClass="button" />
        </div>


      <div class="ExistingCustomer">
    <form id="Existing Customer" onsubmit="return submitForm()">
        <div class="form-group">
            <label for="SalesmanName">Salesman Name:</label>
            <input type="text" id="SalesmanName" name="SalesmanName" required />
        </div>

        <div class="form-group">
            <label for="Company">Company:</label>
            <input type="text" id="Company" name="Company" required />
        </div>

        <div class="form-group">
            <label for="Status">Status:</label>
            <input type="text" id="status" name="status" value="New" readonly />
            <label for="FormNumber">Form No:</label>
            <input type="text" id="FormNumber" name="Form No" value="SYSTEM" readonly />
        </div>

        <div class="form-group">
            <label for="Code">Salesman Code:</label>
            <input type="text" id="code" name="code" required />
        </div>

        <div class="form-group">
            <label for="Name">Customer Name:</label>
            <input type="text" id="CustName" name="CustName" required />
        </div>

        <div class="form-group">
            <button type="button" onclick="getCoordinates()">Get GPS Coordinates</button>
        </div>

        <div class="form-group">
            <div id="map"></div>
        </div>

        <div class="form-group">
            <input type="submit" value="Save" />
            <button type="button" onclick="goBack()">Close</button>
        </div>
    </form>
</div>

      <input type="button" id="close" />
    </form>
</body>
</html>



