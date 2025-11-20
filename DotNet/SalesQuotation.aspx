<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesQuotation.aspx.cs" Inherits="DotNet.SalesQuotation" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Full_Page_Tab.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <!-- Bootstrap -->
    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />

    <%--    <link href="../Content/font-awesome.min.css" rel="stylesheet" />--%>
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <%--    <link rel="stylesheet" href="../Content/bootstrap.min.css" media="screen" />--%>
    <%--    <link rel="stylesheet" href="../Content/bootstrap-glyphicons.css" media="screen" />--%>
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <!-- Bootstrap DatePicker -->
    <script type="text/javascript">
        $(function () {
            $('#txtStartDate').datepicker({
                format: "dd/mm/yyyy"
            }).datepicker("setDate", 'now');

            $('#txtEndDate').datepicker({
                format: "dd/mm/yyyy"
            }).datepicker("setDate", 'now');

            $('#txtDlvDate').datepicker({
                format: "dd/mm/yyyy"
            }).datepicker("setDate", 'now')

            //$('#txtExpiryDate').datepicker({
            //    format: "dd/mm/yyyy"
            //}).datepicker("setDate", 'now')

            //$('#txtConfirmationDate').datepicker({
            //    format: "dd/mm/yyyy"
            //}).datepicker("setDate", 'now')
        });

    </script>
    <title>Quotation</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/GoToTab.js"></script>
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
<body>

    <form id="form1" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/SQ.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 style="font-weight: bold">Quotation</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="SalesQuotation.aspx" class="active" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
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

            <div class="col-12 col-s-12">

                <div class="col-12 col-s-12">
                    <asp:Button ID="Button_sales_header_section" runat="server" OnClick="sales_header_section_Click" Text="Header" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_quotation_section" runat="server" OnClick="quotation_section_Click" Text="Line" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_confirmation_section" runat="server" OnClick="confirmation_section_Click" Text="Overview" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_enquiries_section" runat="server" OnClick="enquiries_section_Click" Text="Enquiries" class="frame_style_4bar" />
                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <!--sales_header_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div id="sales_header_section" style="display: none" runat="server">
                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_SaveHeader" runat="server" OnClick="Button_SaveHeader_Click" Text="Save Header" class="frame_style_type2" />&nbsp
                        <asp:Button ID="Button_UpdateHeader" runat="server" OnClick="Button_UpdateHeader_Click" Text="Upadate Header" Visible="false" class="frame_style_type2" />
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <!--Customer ////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_sales_header_customer" runat="server" Text="Customer" class="accordion_style_sub_fixed_darkcyan" />
                            <!--==============================================================================-->
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Account Type:      </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:DropDownList ID="ddlAccount" runat="server" class="dropdownlist">
                                        <asp:ListItem Text="Business Relation" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Account:      </label>
                                </div>
                                <div class="col-3 col-s-4">
                                    <asp:TextBox ID="TextBox_Account" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                    <div id="checkun" runat="server" visible="false">
                                        <asp:Image ID="shwimg" class="image_indicator" runat="server" />
                                        <asp:Label ID="lblmsg" class="indicator" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-2 col-s-3">
                                    <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find in list" class="glow" Style="margin-bottom: 4px" />
                                </div>
                                <div class="col-2 col-s-3">
                                    <asp:Button ID="Button_CheckAcc" runat="server" OnClick="CheckAcc" CausesValidation="false" Text="Validate" class="glow_green" />
                                </div>
                            </div>

                            <!--============================================================================== -->
                            <%--                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Expiry Date:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtExpiryDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                        <label class="input-group-btn" for="txtExpiryDate">
                                            <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </label>
                                    </div>
                                </div>
                            </div>--%>

                            <!--============================================================================== -->
                            <%--                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">References:  </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_References" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Confirmation Date:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtConfirmationDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                        <label class="input-group-btn" for="txtConfirmationDate">
                                            <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </label>
                                    </div>
                                </div>
                            </div>--%>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Name:        </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:Label ID="Label_CustName" class="gettext" runat="server" Text=" "></asp:Label>
                                </div>
                            </div>

                            <!--============================================================================== -->
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Business Account:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:Label ID="lblBusinessAcc" class="gettext" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Sales Responsible:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:Label ID="lblSalesResponsible" class="gettext" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Quotation Status:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:Label ID="lblQuotationStat" class="gettext" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Address:</label>
                                </div>
                                <div class="col-9 col-s-12">
                                    <asp:Label ID="Label_Address" class="gettext" TextMode="MultiLine" Rows="3" runat="server" CssClass="col-9 col-s-12"></asp:Label>
                                </div>
                            </div>

                            <!--============================================================================== -->


                            <!--Delivery////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_sales_header_delivery" runat="server" Text="Delivery" class="accordion_style_sub_fixed_darkcyan" />

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Delivery Date:</label>
                                </div>
                                <%--                                <div class="col-3 col-s-4">
                                    </label><asp:Label ID="Label_DeliveryDate" class="gettext" runat="server" Text=" "></asp:Label>
                                    &nbsp<asp:Button ID="Button_DeliveryDate" runat="server" Text="Show" OnClick="Button_DeliveryDate_Click" class="glow_green" Enabled="false" />
                                    <asp:Calendar ID="Calendar1" class="inputcalendar" runat="server" SelectionMode="Day" OnSelectionChanged="Calendar1_SelectionChanged" Visible="false"></asp:Calendar>
                                </div>--%>
                                <div class="col-3 col-s-5">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDlvDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                        <label class="input-group-btn" for="txtDlvDate">
                                            <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Receiver Name:</label>
                                </div>
                                <div class="col-6 col-s-8">
                                    </label><asp:Label ID="Label_Receiver_Name" class="gettext" runat="server" Text=" "></asp:Label>

                                </div>
                            </div>
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Delivery Address:</label>
                                </div>
                                <div class="col-6 col-s-8">
                                    <asp:Label ID="Label_Delivery_Addr" class="gettext" TextMode="MultiLine" max-height="120px" Rows="3" Columns="20" Style="resize: none;" runat="server" Text=" "></asp:Label>
                                </div>
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Delivery_Addr" runat="server" OnClick="Button_Alt_Delivery_Addr_Click" Text="Alt. Addr." class="glow_green" />
                                    </div>

                                <br />
                                <asp:GridView ID="GridView_AltAddress" runat="server"
                                    HorizontalAlign="left" CssClass="mydatagrid"
                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows">

                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow2" runat="server" OnCheckedChanged="CheckBox_Changed2" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Label ID="hidden_alt_address_RecId" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_alt_address" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_alt_address_counter" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_Street" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_ZipCode" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_Currency" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_City" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_State" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_Country" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_QuotationID" class="gettext" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_EmployeeID" class="gettext" runat="server" Visible="false"></asp:Label>


                            <!--Sales Order////////////////////////////////////////////////////////////////////////////////////-->
                            <%--                                                        <asp:Button ID="Accordion_sales_header_SalesOrder" runat="server" Text="Sales Order" class="accordion_style_sub_fixed_darkcyan" />--%>
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Payment Terms:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:CheckBoxList ID="chkboxlistTerms" runat="server" OnSelectedIndexChanged="chkboxlistTerms_SelectedIndexChanged">
                                        <asp:ListItem Text="30 Days" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="60 Days" Value="2"></asp:ListItem>
                                    </asp:CheckBoxList> 
                                </div>
                            </div>

                            <%--                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Suffix Code:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:Label ID="Label_Suffix_Code" class="gettext" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Notes:      </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Notes" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!--Overall_sales_order_section ////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress6" class="gettext" AssociatedUpdatePanelID="UpdatePanel6">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>


                <div id="Overall_sales_order_section" style="display: none" runat="server">
                    <div class="col-12 col-s-12">
                        <asp:Label ID="Label_SQ_No" class="label_SO" runat="server"></asp:Label>
                        <asp:Label ID="hidden_Label_SQ_No" class="gettext" runat="server" Visible="false"></asp:Label>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>

                            <!--start of view of general table-->
                            <div id="sales_order_section_general" runat="server">
                                &nbsp
                                <%--                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton_FavourOrder" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/FavourOrder.png" />
                                    <asp:ImageButton ID="ImageButton_FavourOrder_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/FavourOrder_h.png" OnClick="Button_Favourite_Click" />
                                </div>--%>

                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton_SalesOrder" class="menuimage_nohighlight" runat="server" ImageUrl="~/RESOURCES/ADDQUOTATION.png" />
                                    <asp:ImageButton ID="ImageButton_SalesOrder_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/HOVERFRAME.png" OnClick="Button_SalesOrder_Click" />
                                </div>

                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton1" class="menuimage_nohighlight" runat="server" ImageUrl="~/RESOURCES/PRINT.png" />
                                    <asp:ImageButton ID="ImageButton3" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/HOVERFRAME.png" OnClick="Button_Quotation_Click" />
                                </div>

                                <div class="col-12 col-s-12">
                                    <!--view of general table-->
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Sales_Order_Line" runat="server" Text="Quotation Line" class="accordion_style_sub_fixed_darkcyan" />
                                    </div>
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Submit_SalesQuotationLine" runat="server" OnClick="Button_Submit_SalesQuotationLine_Click" Text="Submit Order" class="frame_style_type2" />
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Delete_SalesOrderLine" runat="server" OnClick="Button_Delete_SalesOrderLine_Click" Text="Delete Line" class="frame_style_type2" Visible="false" />
                                        <br />
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <asp:GridView ID="GridView_Sales_Quotation_Line_View" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            Style="overflow: hidden;"
                                            AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckBox_ToDeleteAll" runat="server" OnCheckedChanged="CheckBox_Changed_ToDeleteAll" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox_ToDeleteByOne" runat="server" OnCheckedChanged="CheckBox_Changed_ToDeleteByOne" AutoPostBack="true" Style="float: left;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:BoundField DataField="Item Id" HeaderText="Item Id" />
                                                <asp:BoundField DataField="Item Name" HeaderText="Item Name" />
                                                <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                <asp:BoundField DataField="Price Each" HeaderText="Price Each (RM)" />
                                                <asp:BoundField DataField="Net Amount" HeaderText="Net Amount" />
                                                <asp:BoundField DataField="Delivery Date" HeaderText="Delivery Date" />
                                                <asp:BoundField DataField="Hidden_RecId" HeaderText="Hidden_RecId" />
                                                <asp:BoundField DataField="Hidden_allow_alter" HeaderText="Hidden_allow_alter" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                            </div>
                            <!--end of view of general table-->
                            <!--FV order ////////////////////////////////////////////////////////////////////////////////////-->

                            <div id="FV_order_section" visible="false" runat="server">
                                <div class="col-12 col-s-12">
                                    <%--                                    <asp:Button ID="Button_SaveFavourite" runat="server" OnClick="Button_SaveFavourite_Click" Text="Save Favourite" class="frame_style_type2" />--%>
                                    &nbsp
                                    <%--                                <asp:Button ID="Button_CancelFavourite" runat="server" CausesValidation="false" OnClick="Button_CancelFavourite_Click" Text="Cancel" class="frame_style_type2" />--%>
                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_FV_Order" runat="server" Text="Favourite Order" class="accordion_style_sub_fixed_darkcyan" />
                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:GridView ID="GridView_FVOrder" runat="server"
                                        HorizontalAlign="left" CssClass="mydatagrid"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        Style="overflow: hidden;"
                                        AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="No." />
                                            <asp:BoundField DataField="Item Id" HeaderText="Item Id" />
                                            <asp:BoundField DataField="Item Name" HeaderText="Item Name" />
                                            <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" />
                                            <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                            <asp:TemplateField HeaderText="Item Id">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DropDownList_FV_Order" runat="server" class="inputtext" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox_New_QTY" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:RangeValidator runat="server" ControlToValidate="TextBox_New_QTY"
                                                        ErrorMessage="Invalid Qty!" MaximumValue="999"
                                                        MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                    </asp:RangeValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <!--end of FV order ////////////////////////////////////////////////////////////////////////////////////-->
                            <!--Quotation Line////////////////////////////////////////////////////////////////////////////////////-->
                            <div id="Sales_order_section" visible="false" runat="server">
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_SaveSalesOrder" runat="server" OnClick="Button_SaveSalesOrder_Click" Text="Save Quotation" class="frame_style_type2" />
                                    &nbsp
                                <asp:Button ID="Button_CancelSalesOrder" runat="server" CausesValidation="false" OnClick="Button_CancelSalesOrder_Click" Text="Cancel" class="frame_style_type2" />
                                </div>
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_Sales_Order" runat="server" Text="Sales Order" class="accordion_style_sub_fixed_darkcyan" />
                                </div>

                                <!--==============================================================================-->
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">ID:      </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_Id" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        <div id="checkun2" runat="server" visible="false">
                                            <asp:Image ID="shwimg2" class="image_indicator" runat="server" />
                                            <asp:Label ID="lblmsg2" class="indicator" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-2 col-s-8">
                                        <asp:Button ID="Button_InventoryMasterList" runat="server" OnClick="CheckIdInList" CausesValidation="false" Text="Find in list" class="glow" />
                                    </div>
                                    <div class="col-2 col-s-8">
                                        <asp:Button ID="Button_TextBox_Id" runat="server" OnClick="CheckId" Text="Validate" class="glow_green" />
                                    </div>

                                </div>
                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Name:</label>
                                    </div>
                                    <div class="col-7 col-s-8">
                                        <asp:Label ID="Label_IdName" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Warehouse:  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_Warehouse" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Unit:  </label>
                                    </div>
                                    <div class="col-4 col-s-8">
                                        <asp:DropDownList ID="DropDownList_Unit" runat="server" class="dropdownlist" />
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Quantity:  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_SLQuantity" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        <asp:RangeValidator runat="server" ControlToValidate="TextBox_SLQuantity"
                                            ErrorMessage="Invalid Qty!" MaximumValue="999" MinimumValue="0"
                                            Type="Integer" Style="color: red; font-weight: bold;" SetFocusOnError="True">
                                        </asp:RangeValidator>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Price (RM):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_Price" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Discount pct. (%):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="txt_Disc_pct" class="inputtext" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Discount (RM):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="txt_Discount" class="inputtext" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">

                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Multiline discount pct. (%):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="txt_MultilineDisc_pct" class="inputtext" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Multiline discount (RM):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="txt_MultilineDiscount" class="inputtext" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <asp:Label ID="hidden_itemIncQty" class="gettext" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="hidden_configIdStr" class="gettext" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="hidden_sizeStr" class="gettext" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="hidden_colorStr" class="gettext" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="hidden_Lowest_Qty" class="gettext" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="hidden_selected_item_unit" class="gettext" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="hidden_inventDim" class="gettext" runat="server" Visible="false"></asp:Label>
                                <!--============================================================================== -->
                            </div>
                            <!--end of Sales Line ////////////////////////////////////////////////////////////////////////////////////-->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!--end of Overall_sales_order_section////////////////////////////////////////////////////////////////////////////////////-->
                <!--confirmation_section/ Overview////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress7" class="gettext" AssociatedUpdatePanelID="UpdatePanel7">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div id="confirmation_section" style="display: none" runat="server">

                    <!--===============================================================================================-->
                    <div class="col-12 col-s-12">
                        <%--                        <asp:Button ID="Button_ListOutStanding" runat="server" OnClick="Button_ListOutStanding_Click" Text="List OutStanding" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />--%>

                        <%--                                <asp:Button ID="Button_ListOutStandingBlocked" runat="server" OnClick="Button_ListOutStandingBlocked_Click" Text="List OutStanding Blocked" class="frame_style_type2" Style="max-width: 80%; padding: 5px; margin-top: 5px;" />&nbsp--%>
                        <%--                                <asp:Button ID="Button_ListAll" runat="server" OnClick="Button_ListAll_Click" Text="List All" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />--%>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <div id="confirmation_section_general" visible="false" runat="server">
                                <!--start of view of general table-->
                                <div class="col-12 col-s-12">

                                    <!--view of general table-->
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Overview_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" />
                                    </div>

                                    <%--                                <asp:CheckBox ID="CheckBox_div_Searchable_ID" runat="server" OnCheckedChanged="CheckBox_div_Searchable" Text="Show Search Bar" AutoPostBack="true" style="background-color:darkred;color: ;" Visible="false"/><br />--%>

                                    <div class="col-12 col-s-12" id="div_Searchable" runat="server">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search By:      </label>
                                            </div>
                                            <div class="col-3 col-s-8">
                                                <asp:DropDownList ID="DropDownList_Search" runat="server" class="dropdownlist">
                                                    <asp:ListItem Text="Quotation Number" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Account Number" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Workshop Name"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-4 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search:      </label>
                                            </div>
                                            <div class="col-5 col-s-6">
                                                <asp:TextBox ID="TextBox_Search" class="inputtext" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton2" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div id="grdCharges" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                            <asp:GridView ID="GridViewOverviewList" runat="server"
                                                PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                                HtmlEncode="False" DataKeyNames="Customer Account"
                                                Style="overflow: hidden;" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:TemplateField HeaderText="Quotation ID">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button_QuotationID" runat="server" OnClick="Button_QuotationID_Click" CausesValidation="false" Text='<%# Eval("Quotation Id") %>' class="button_grid" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Workshop Name" HeaderText="Workshop Name" />
                                                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" />
                                                    <asp:BoundField DataField="Customer Account" HeaderText="Customer Account" />
                                                    <asp:BoundField DataField="Delivery Date" HeaderText="Delivery Date" />
                                                    <asp:BoundField DataField="Payment" HeaderText="Payment" />
                                                    <asp:BoundField DataField="Date Created" HeaderText="Date Created" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                                    <%--                                                    <asp:BoundField DataField="Reference" HeaderText="Reference" />--%>
                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerSettings PageButtonCount="2" />
                                                <RowStyle CssClass="rows" />
                                            </asp:GridView>
                                            <%--                                            <asp:HiddenField ID="hdQuotationStatus" runat="server" />--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="errMsg" runat="server" CssClass="labeltext" Visible="false"></asp:Label>
                            </div>
                            <!--end of view of general table-->
                            <!--===============================================================================================-->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!--end of confirmation_section/ Overview////////////////////////////////////////////////////////////////////////////////////-->
                <!--enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress16" class="gettext" AssociatedUpdatePanelID="UpdatePanel4">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div id="enquiries_section" style="display: none" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="col-12 col-s-12">
                                <asp:Button ID="Button_Enquiries_accordion" runat="server" Text="Customer Invoices" class="accordion_style_sub_fixed_darkcyan" />
                            </div>
                            <div class="col-12 col-s-12">
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Salesman Id:      </label>
                                    </div>

                                    <div class="col-3 col-s-8">
                                        <asp:DropDownList ID="DropDownList_Salesman" runat="server" AutoPostBack="true" class="dropdownlist">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Days:      </label>
                                    </div>
                                    <div class="col-2 col-s-4">
                                        <asp:DropDownList ID="DropDownList_DayInvoice" runat="server" AutoPostBack="true" class="inputtext" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_DayInvoice">
                                            <asp:ListItem Text="-- SELECT --" Value=""></asp:ListItem>
                                            <asp:ListItem Text="< 35 Days" Value="35"></asp:ListItem>
                                            <asp:ListItem Text="< 65 Days" Value="65"></asp:ListItem>
                                            <asp:ListItem Text="1 Year" Value="365"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </div>
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Customer Account:      </label>
                                </div>

                                <div class="col-5 col-s-6">
                                    <asp:TextBox ID="TextBox_SearchEnquiries" class="inputtext" runat="server" OnTextChanged="TextBox_SearchEnquiries_Changed" AutoPostBack="true"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton5" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="CheckEnquiries" />
                                </div>

                                <div class="col-3 col-s-4">
                                    <asp:Button ID="Button_FindList" OnClick="CheckAccInList2" runat="server" CausesValidation="false"
                                        Text="Search Customer" class="glow" Style="margin-bottom: 4px; margin-left: -10px;" />
                                </div>
                            </div>


                            <div id="Enquiries_section_general" visible="false" runat="server">
                                <!--start of view of general table-->

                                <!--start of GridEnquiriesList////////////////////////////////////////////////////////////////////////////////////-->

                                <div class="col-12 col-s-12">
                                    <div id="grdCharges2" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                                        <asp:GridView ID="GridEnquiriesList" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="Account No., Account Name"
                                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_enquiries" AllowCustomPaging="True"
                                            HtmlEncode="False" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:TemplateField HeaderText="Quotation ID">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_Quotation" runat="server" OnClick="Button_Quotation_Click" CausesValidation="false" Text='<%# Eval("Quotation ID") %>' class="button_grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Shipping Date Requested" HeaderText="Shipping Date Requested" ItemStyle-Width="200px" />
                                                <asp:BoundField DataField="Account No." HeaderText="Account No." />
                                                <asp:BoundField DataField="Account Name" HeaderText="Account Name" />
                                                <asp:BoundField DataField="Delivery Address" HeaderText="Delivery Address" />

                                                <%--                                                <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                                                <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" />
                                                <asp:BoundField DataField="Due Date" HeaderText="Due Date" />
                                                <asp:BoundField DataField="Invoice Amount" HeaderText="Invoice Amount (RM)" />
                                                <asp:BoundField DataField="Outstanding Amount" HeaderText="Outstanding Amount (RM)" />
                                                <asp:BoundField DataField="Current Payment" HeaderText="Current Payment (RM)" />
                                                <asp:BoundField DataField="Balance Outstanding" HeaderText="Balance Outstanding (RM)" />--%>
                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <!--end of GridViewOverviewList////////////////////////////////////////////////////////////////////////////////////-->
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--==============================================================================-->
                </div>

                <!--end of enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
            </div>
        </div>

        <script src="scripts/ButtonUp.js"></script>
    </form>
</body>
</html>
