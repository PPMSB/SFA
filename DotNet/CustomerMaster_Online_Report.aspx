<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerMaster_Online_Report.aspx.cs" Inherits="DotNet.CustomerMaster_Online_Report" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="STYLES/bootstrap.min.css" media="screen" />

     <!-- Bootstrap -->
 <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
 <script src="https://code.jquery.com/ui/1.13.0/jquery-ui.min.js"></script>
 <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.0/themes/base/jquery-ui.css" />

 <!-- Bootstrap DatePicker -->
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />

 <link href="../Content/bootstrap.min.css" rel="stylesheet" />
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" media="screen" />
 <script src="../Scripts/bootstrap.bundle.min.js"></script>
 <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
 <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
 <link href="../Content/bootstrap-datetimepicker.min.css" rel="stylesheet" />

 <script src="../scripts/moment.min.js"></script>
 <script src="../scripts/bootstrap-datetimepicker.js"></script>

    <title>Customer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
     <script type="text/javascript">
         $(document).ready(function () {
             // Get the current date
             var currentDate = new Date();

             // Retrieve dates from server-side variables
             var fromDate = '<%= Request.Cookies["CustomerMaster_Online_Report"] != null ? Request.Cookies["CustomerMaster_Online_Report"]["StartDate"] : "" %>';
        var toDate = '<%= Request.Cookies["CustomerMaster_Online_Report"] != null ? Request.Cookies["CustomerMaster_Online_Report"]["EndDate"] : "" %>';

            // Initialize datepicker for start date
            $('#txtStartDate').datepicker({
                dateFormat: "dd/mm/yy", // Use "dd/mm/yy" for jQuery UI Datepicker
            }).datepicker("setDate", fromDate ? new Date(fromDate) : new Date(currentDate.getFullYear(), currentDate.getMonth(), 1)); // Default to first day of the month if no cookie
            // Initialize datepicker for end date
            $('#txtEndDate').datepicker({
                dateFormat: "dd/mm/yy" // Use "dd/mm/yy" for jQuery UI Datepicker
            }).datepicker("setDate", toDate ? new Date(toDate) : new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0)); // Default to last day of the month if no cookie

     </script>
<body>
    <form id="form1" runat="server">
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/Customert.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Customer</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" class="active" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
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
                     <a href="EventParticipant.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Participant</a>
                    <a href="Map3.aspx" id="MapTag2" runat="server" visible="false">Map</a>
                    <a href="NewProductHomePage.aspx" id="NewProduct2" runat="server" visible="false">New Product Request</a>
                </div>
            </div>
                <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%--                    <img src="RESOURCES/menu.png" />--%>
                    <div class="global_container" onclick=" myFunction(this);">
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
                                <div class="col-12 col-s-12 print-hide">
                    <asp:Button ID="Button_Online_Report" runat="server" Text="Back" OnClick="Button_Online_Report_Click" class="frame_style_4bar" />
                    </div>
            </div>
            <div class="col-12 col-s-12">
                <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="ProvBalanceReport" runat="server">
                            <div class="col-12 col-sm-12">
                          <!--===========================================Salesman====================================================-->
                                            <div class="col-12 col-sm-12">
                                                 <div class="col-12 col-sm-6">
                                                     <div class="col-4 col-sm-3">
                                            <label class="labeltext">Salesman:      </label>
                         </div>
                        <div class="col-8 col-sm-6">
                        <asp:ImageButton ID="ImageButton8" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_ProvisionReport_Click" Visible="false" />
                            <div class="col-9 col-sm-9">
                            <asp:DropDownList ID="DropDownList2" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList2">
                         </asp:DropDownList>
                                 </div>
                                </div>
                            </div>
                        </div>                                    
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>

                                            <div class="col-12 col-sm-12">
                                                <div class="col-8 col-sm-4">

                                                    <div class="col-4 col-sm-3">
                                                        <label class="labeltext">Start Date:</label>
                                                    </div>

                                                    <div class="col-7 col-sm-3">
                                                        <div class="input-group">
                                                            <input type="date" id="txtStartDate" runat="server" name="trip-start" min="2000-01-01">
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-8 col-sm-4">
                                                    <div class="col-3 col-s-4">
                                                        <label class="labeltext">End Date:</label>
                                                    </div>

                                                    <div class="col-7 col-sm-3">
                                                        <div class="input-group">
                                                            <input type="date" id="txtEndDate" runat="server" name="trip-end" min="2000-01-01" >
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-3 col-s-6">
                                                <%--<asp:Button ID="Button_FindList" OnClick="CheckAccInList2" runat="server" CausesValidation="false" Text="Search Customer" class="glow" Style="margin-bottom: 4px" />
   --%>                                         <asp:Button ID="btnGetReport_1" Text="Generate" runat="server" OnClick="Button_Search_ProvisionReport_Click" class="getStatement" Style="margin-bottom: 4px" Visible="false" />
                                            </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="DropDownList2" />
                                            <asp:PostBackTrigger ControlID="ImageButton8" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                        <!-- =================== Report ======================================================== -->
                                   <div class="col-12 col-s-12">
                                    <div id="grdCharges" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <asp:GridView ID="GridView1" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Style="overflow: hidden" EmptyDataText="No records found."
                                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                            HtmlEncode="False" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:BoundField DataField="Salesman ID" HeaderText="Salesman ID" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="Customer Code" HeaderText="Customer Code" />
                                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />

                                                <asp:BoundField DataField="Start Date" HeaderText="Start Date" />
                                                <asp:BoundField DataField="End Date" HeaderText="End Date" />

                                                <%--Neil - Request from Keegan change Point to Value 15/10/2025--%>
                                                <asp:BoundField DataField="Open Point" HeaderText="Open Value" />
                                                <asp:BoundField DataField="Earn Point" HeaderText="Earn Value" />
                                                <asp:BoundField DataField="Used Point" HeaderText="Used Value" />
                                                <asp:BoundField DataField="Adjust Point" HeaderText="Adjust Value" />
                                                <asp:BoundField DataField="Balance" HeaderText="Balance" />
                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>
                                </div>


                        </div>
                      </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </form>
</body>
</html>
