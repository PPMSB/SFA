<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Redemption_OutBalance.aspx.cs" Inherits="DotNet.Redemption_OutBalance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <script src="scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <!-- Bootstrap -->
    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />

    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <title>Redemption</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>

<body>
    <form id="form1" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/Redemptionn.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Redemption</h1>
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
                <a href="Redemption.aspx" id="RedemptionTag2" class="active" runat="server" visible="false">Redemption</a>
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
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <!--==============================================================================-->
            <div class="col-12 col-s-12">
                <!--content--==============================================================================-->

                <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="Accordion_Redemption" runat="server" Text="Customer Credit Information" class="accordion_style_sub_fixed_darkcyan" /><br />
                        <div id="new_applicant_section" runat="server">
                            <div class="col-6 col-s-12" runat="server">
                                <div class="col-4 col-s-4">
                                    <label class="labeltext">Salesman Code / Name:      </label>
                                </div>
                                <div class="col-6 col-s-6">
                                    <asp:Label ID="lblSalesmanName" CssClass="gettext" autocomplete="off" runat="server"></asp:Label>
                                </div>
                            </div>

                            <!--============================================================================== -->
                            <div id="div_CustInfoExtra" runat="server">
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Customer Code / Name:        </label>
                                    </div>
                                    <div class="col-6 col-s-6">
                                        <asp:Label ID="lblCustName" class="gettext" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Customer Class:        </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="LabelCustClass" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>

                                </div>
                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Tel No:  </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="lblTelNo" class="inputtext_2" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Customer Main Group:        </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="Label_CustGroup" class="gettext" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Customer Type:        </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="Label_CustType" class="gettext" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">EOR Target Grand Total:  </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="Label_EORTarget" class="inputtext_2" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Expiry:  </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="Label_Expiry" class="gettext" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <!--============================================================================== -->

                                <%-- Normal --%>
                                <asp:Panel ID="panelNormal" runat="server">
                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblTargetCarton" runat="server" Text="Target Carton:" CssClass="labeltext"></asp:Label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblGetTargetCarton" runat="server" CssClass="labeltext"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblCommence" runat="server" Text="Commence:" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblGetCommence" runat="server" CssClass="inputtext"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblAveragePayDay" runat="server" Text="Average Payment Day (6 months):" CssClass="labeltext"></asp:Label>
                                        </div>

                                        <div class="col-4 vcol-s-4">
                                            <asp:Label ID="lblGetAveragePayDay" runat="server" CssClass="inputtext"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblIncorporationDate" runat="server" Text="Incorporation Date:" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-4 col-s-6">
                                            <asp:Label ID="lblGetIncorporationDate" runat="server" CssClass="inputtext"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblAverageMonthly" runat="server" Text="Average Monthly Sales (6 months):" CssClass="labeltext"></asp:Label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblGetAverageMonthlySales" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblAccOpenDt" runat="server" Text="Account Open Date:" CssClass="labeltext"></asp:Label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblGetAccOpenDt" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblCreditLmt" runat="server" CssClass="labeltext" Text="Credit Limit:"></asp:Label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="txtGetCreditLimit" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblOverdue" runat="server" CssClass="labeltext" Text="Overdue Interest:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetOverdue" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblApprovalCredit" runat="server" CssClass="labeltext" Text="Approval Credit:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetApprovalCredit" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblLastMonth" runat="server" CssClass="labeltext" Text="Last Month Collection:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetLastMonth" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblBankGuarantee" runat="server" CssClass="labeltext" Text="Bank/Corp Guarantee:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetBankGuarantee" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblBankExpiry" runat="server" CssClass="labeltext" Text="Bank/Corp Expiry:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetBankExpiry" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblCurrentMonth" runat="server" CssClass="labeltext" Text="Current Month Collection:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetCurrentMonth" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblChequeTotal" runat="server" CssClass="labeltext" Text="Posted Date Cheque Total:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetChequeTotal" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-4 col-s-4">
                                            <asp:Label ID="lblLastCheque" runat="server" CssClass="labeltext" Text="Last Return Cheque:"></asp:Label>
                                        </div>

                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblGetLastCheque" runat="server" CssClass="inputtext_2"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12" style="text-align: center">
                                        <div class="col-11 col-s-6">
                                            <asp:Label ID="lblOutstanding" runat="server" Text="Last Outstanding :" CssClass="labeltext" ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lblDays" runat="server" ForeColor="Red" Text="0 days"></asp:Label>
                                        </div>

                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div id="grdCharges2" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                            <asp:GridView ID="gvCustCredit" runat="server" Style="border-collapse: collapse; overflow: hidden;"
                                                PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                AllowPaging="True" AllowCustomPaging="True" OnPageIndexChanging="gvCustCredit_PageIndexChanging"
                                                HtmlEncode="False" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:BoundField DataField="Voucher" HeaderText="Voucher" />
                                                    <asp:BoundField DataField="Invoice No." HeaderText="Invoice No." />
                                                    <asp:BoundField DataField="Date" HeaderText="Date" />
                                                    <asp:BoundField DataField="Amount Currency" HeaderText="Amount Currency" ItemStyle-Width="30%" />
                                                    <asp:BoundField DataField="Currency" HeaderText="Currency" />
                                                    <asp:BoundField DataField="Due Date" HeaderText="Due Date" />
                                                    <asp:BoundField DataField="Total Outstanding Day" HeaderText="Total Outstanding Day" ItemStyle-Width="30%" />
                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerSettings PageButtonCount="2" />
                                                <RowStyle CssClass="rows" />
                                            </asp:GridView>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--        <script src="scripts/ButtonUp.js"></script>--%>
    </form>
</body>
</html>
