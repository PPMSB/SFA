<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerMaster.aspx.cs" Inherits="DotNet.CustomerMaster" MaintainScrollPositionOnPostback="true" %>

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

    <title>Customer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
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
                    <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>
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
                    <asp:Button ID="Button_Cust_Enquiries" runat="server" OnClick="Button_Cust_Enquiries_Click" Text="Enquiries" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" id="img1" runat="server" />
                    <asp:Button ID="Button_Cust_Campaign" runat="server" Text="Campaign" OnClick="Button_Cust_Campaign_Click" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" id="img2" runat="server" />
                    <asp:Button ID="Button_ROC_N_TIN" runat="server" Text="ROC & TIN" OnClick="Button_ROC_N_TIN_Click" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" id="img3" runat="server" />
                    <asp:Button ID="Button_Online_Report" runat="server" Text="Online Report" OnClick="Button_Online_Report_Click" class="frame_style_4bar" />
                    </div>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="col-6 col-s-12">
                            <div class="col-3 col-s-4">
                                <label class="labeltext">Search By:      </label>
                            </div>

                            <div class="col-3 col-s-8">
                                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" class="dropdownlist">
                                    <asp:ListItem Text="Customer Name" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Account No." Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-4 col-s-12">
                            <div class="col-3 col-s-4">
                                <label class="labeltext">Search:      </label>
                            </div>

                            <div class="col-8 col-s-6">
                                <asp:TextBox ID="TextBox_Account" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>
                                <asp:ImageButton ID="ImageButton2" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="CheckAcc" />
                            </div>
                        </div>
                        <%--                        <asp:Button ID="button" runat="server" Text="testing" OnClick="button_Click" />--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
                        <div id="grdCharges" runat="server" style="max-width: 100%; overflow: auto; max-height: 100%;">
                            <asp:GridView ID="GridView1" runat="server" PagerStyle-CssClass="page-link"
                                PageSize="20" HorizontalAlign="Left" AllowCustomPaging="true"
                                CssClass="mydatagrid" AllowPaging="True" EmptyDataText="No records found."
                                OnPageIndexChanging="datagrid_PageIndexChanging" EnableSortingAndPagingCallbacks="true"
                                HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="No" HeaderText="No" />
                                    <asp:TemplateField HeaderText="Account">
                                        <ItemTemplate>
                                            <asp:Button ID="Button_Account" runat="server" OnClick="Button_Account_Click" CausesValidation="false" Text='<%# Eval("Account") %>' class="button_grid" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Account" HeaderText="Account" />
                                    <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="State" HeaderText="State" />
                                    <asp:BoundField DataField="Employee ID" HeaderText="Employee ID" />
                                    <asp:BoundField DataField="LPPoint" HeaderText="LP Point" />
                                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                    <asp:BoundField DataField="TeleFax" HeaderText="TeleFax" />
                                    <asp:BoundField DataField="Cellular Phone" HeaderText="Cellular Phone" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" />
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <PagerSettings PageButtonCount="2" />
                                <RowStyle CssClass="rows" />

                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </form>
</body>
</html>
