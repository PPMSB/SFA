<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewCustomerApproval.aspx.cs" Inherits="DotNet.NewCustomerApproval" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" crossorigin="anonymous" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.min.css" />
    <link href="STYLES/NEWCUSTApproval.css" rel="stylesheet" />


    <title>New Customer Approval Group</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>

<script type="text/javascript">

</script>

<body>
    <form id="form1" runat="server">
        <div class="container1">
            <%--            <div class="row">--%>

            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/NewCustApproval.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>New Customer Approval</h1>
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
                <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>
                <a href="Setting.aspx" class="active" id="SettingTag2" runat="server">Setting</a>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%--                    <img src="RESOURCES/menu.png" />--%>
                    <div class="container" onclick=" myFunction(this);">
                        <div class="bar1"></div>
                        <div class="bar2"></div>
                        <div class="bar3"></div>
                    </div>

                </a>
                <script src="scripts/top_navigation.js"></script>
            </div>
            <!--==============================================================================-->

            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <div class="col-12 col-s-12">
                <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </div>
            
            <div class="col-12 col-s-12">
                <!-- Existing buttons -->
                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" class="btn btn-primary btn-sm" />
                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" class="btn btn-danger btn-sm" Visible="false" />
                <asp:Button ID="btnSaveGroup" runat="server" OnClick ="btnSaveGroup_Click" Text="Save" class="btn btn-success btn-sm" Visible="false" />
                <asp:Button ID="btnUpd" runat="server" Text="Update" CssClass="btn btn-primary btn-sm" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" OnClick ="btnCancel_Click" Text="Cancel" Visible="false" CssClass="btn btn-secondary btn-sm" />

                    <!-- New DropDownList controls -->

            </div>
            
            <div class="col-12 col-s-12">
                <!-- Existing buttons and other controls -->
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                    <Columns>
                        <asp:BoundField DataField="CreditControlAdmin" HeaderText="Credit Control Admin" SortExpression="CreditControlAdmin" />
                        <asp:BoundField DataField="CreditControlManager" HeaderText="Credit Control Manager" SortExpression="CreditControlManager" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="col-12 col-s-12" id="ddlContainer" runat="server">
                <!-- Existing buttons and other controls -->

                <asp:Label ID="lblCreditControlAdmin" runat="server" Text="Credit Control Admin:" CssClass="h2-style" Visible="false"></asp:Label>
                <asp:DropDownList ID="ddlCreditControlAdmin" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                <asp:Label ID="lblCreditControlManager" runat="server" Text="Credit Control Manager:" CssClass="h2-style" Visible="false"></asp:Label>
                <asp:DropDownList ID="ddlCreditControlManager" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                </asp:GridView>
            </div>


        </div>
    </form>
</body>
</html>
