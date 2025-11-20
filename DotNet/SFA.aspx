<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SFA.aspx.cs" Inherits="DotNet.SFA" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Full_Page_Tab.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <script src="scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <!-- Bootstrap -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.0/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.0/themes/base/jquery-ui.css" />

    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>

    <title>Sales Force Automation</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/GoToTab.js"></script>
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
<script type="text/javascript">
    $(document).ready(function () {
        $('#txtStartDate').datepicker({
            format: "dd/mm/yyyy",
        }).datepicker("setDate", 'now');

        $('#txtEndDate').datepicker({
            format: "dd/mm/yyyy"
        }).datepicker("setDate", 'now');

        $('#txtDeliveryDate').datepicker({
            format: "dd/mm/yyyy"
        }).datepicker("setDate", 'now')
    });
    if ($('#txtStartDate').val()) {
        datepicker('#txtStartDate').val();
    }
    //$(document).ready(function () {
    //    $('#grdCharges').DataTable();
    //});

    $(function () {
        $("[id*=GridView_PromoSection1_sum] input[type=checkbox]").click(function () {
            if ($(this).is(":checked")) {
                $("[id*=GridView_PromoSection1_sum] input[type=checkbox]").removeAttr("checked");
                $(this).attr("checked", "checked");
            }
        });
    });

    function showModal(message) {
        document.getElementById('messageText').innerText = message;
        document.getElementById('myModal').style.display = 'block';
    }

    function closeModal() {
        document.getElementById('myModal').style.display = 'none';
    }

    function onYesClick() {
        document.getElementById('<%= hdnUserResponse.ClientID %>').value = "yes";
        $('#myModal').modal('hide');
    }

    function onNoClick() {
        document.getElementById('<%= hdnUserResponse.ClientID %>').value = "no";
        $('#myModal').modal('hide');
    }

    function confirmDelete() {
        return confirm("Are you sure you want to delete this line?");
    }
    function receiveDataFromInventoryMaster(data) {
        console.log("Received data:", data);
        // alert("Received data: " + data);
        // You can update the page or do whatever you want with the data here
        // Store data in hidden field
        document.getElementById('<%= hiddenDataPassing.ClientID %>').value = data;
        // Trigger postback by clicking hidden button
        __doPostBack('<%= btnHiddenPostback.UniqueID %>', '');
    }
</script>

<body>

    <form id="form1" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/SALES FORCE.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 style="font-weight: bold">Sales</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" class="active" runat="server" visible="false">Sales</a>
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
                <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>

                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%--                    <img src="RESOURCES/menu.png" />--%>
                    <div class="container" style="width: 100%" onclick=" myFunction(this);">
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
                    <asp:Button ID="Button_sales_order_section" runat="server" OnClick="sales_order_section_Click" Text="Line" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_confirmation_section" runat="server" OnClick="confirmation_section_Click" Text="Overview" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_enquiries_section" runat="server" OnClick="enquiries_section_Click" Text="Enquiries" class="frame_style_4bar" />

                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="24000"/>
                <!--sales_header_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext">
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

                    <!--Customer ////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:Button ID="Accordion_sales_header_customer" runat="server" Text="Customer" class="accordion_style_sub_fixed_darkcyan" />
                    <!--==============================================================================-->
                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Account:      </label>
                        </div>
                        <div class="col-4 col-s-5">
                            <asp:TextBox ID="TextBox_Account" class="inputtext" autocomplete="off" runat="server" OnTextChanged="TextBox_Account_TextChanged"></asp:TextBox>
                            <div id="checkun" runat="server" visible="false">
                                <asp:Image ID="shwimg" class="image_indicator" runat="server" />
                                <asp:Label ID="lblmsg" class="indicator" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-4 col-s-4">
                            <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find in list" class="glow" Style="margin-bottom: 4px" />&nbsp
                        </div>
                        <div class="col-3 col-s-4">
                            <asp:Button ID="Button_CheckAcc" runat="server" OnClick="CheckAcc" CausesValidation="false" Text="Validate" class="glow_green" />
                        </div>

                    </div>
                    <!--============================================================================== -->
                    <div class="col-6 col-s-12">

                        <div class="col-3 col-s-4">
                            <label class="labeltext">Name:        </label>
                        </div>
                        <div class="col-6 col-s-8">
                            <asp:Label ID="Label_CustName" class="gettext" runat="server" Text=" "></asp:Label>
                        </div>
                    </div>

                    <!--============================================================================== -->
                    <div class="col-6 col-s-12">

                        <div class="col-3 col-s-4">
                            <label class="labeltext">References:  </label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:TextBox ID="TextBox_References" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <!--============================================================================== -->
                    &nbsp
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Address:</label>
                                </div>
                                <div class="col-6 col-s-8">
                                    <asp:Label ID="Label_Address" class="gettext" TextMode="MultiLine" max-height="120px" Rows="3" Columns="20" Style="resize: none;" runat="server" Text=" "></asp:Label>
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
                                                                        &nbsp<asp:Button ID="Button_DeliveryDate" runat="server" OnClick="Button_DeliveryDate_Click" Text="Show" class="glow_green" Enabled="false" />
                                    <asp:Calendar ID="Calendar1" class="inputcalendar" runat="server" SelectionMode="Day" OnSelectionChanged="Calendar1_SelectionChanged" Visible="false"></asp:Calendar>
                                </div>--%>
                        <div class="col-3 col-s-5">
                            <div class="input-group">
                                <asp:TextBox ID="txtDeliveryDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                <label class="input-group-btn" for="txtDeliveryDate">
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
                            <asp:Label ID="Label_Receiver_Name" class="gettext" runat="server" Text=" "></asp:Label>

                        </div>
                    </div>
                    &nbsp
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Delivery Address:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:Label ID="Label_Delivery_Addr" class="gettext" TextMode="MultiLine" max-height="120px" Rows="3" Columns="20" Style="resize: none;" runat="server" Text=" "></asp:Label><br />
                                    <asp:Button ID="Button_Delivery_Addr" runat="server" OnClick="Button_Alt_Delivery_Addr_Click" Text="Alt. Addr." class="glow_green" />

                                </div>
                                <br />
                                <asp:GridView ID="GridView_AltAddress" runat="server"
                                    HorizontalAlign="left" CssClass="mydatagrid"
                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows">

                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow2" runat="server" OnCheckedChanged="CheckBox_Changed2" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                    <%--                    <div class="col-6 col-s-12">
                        <asp:CheckBox ID="cbAutoPost" runat="server" Text="Auto Post" CssClass="" />
                    </div>--%>

                    <asp:Label ID="hidden_alt_address_RecId" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_alt_address" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_alt_address_counter" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_Street" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_ZipCode" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_City" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_State" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_Country" class="gettext" runat="server" Visible="false"></asp:Label>

                    <!--Sales Order////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:Button ID="Accordion_sales_header_SalesOrder" runat="server" Text="Sales Order" class="accordion_style_sub_fixed_darkcyan" />

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Payment Terms:</label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:Label ID="Label_Payment_Terms" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
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

                    </div>

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
                        <asp:Label ID="Label_SO_No" class="label_SO" runat="server"></asp:Label>
                        <asp:Label ID="hidden_Label_SO_No" class="gettext" runat="server" Visible="false"></asp:Label>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>

                            <!--start of view of general table-->
                            <div id="sales_order_section_general" runat="server">
                                &nbsp
                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton_FavourOrder" class="menuimage_nohighlight" runat="server" ImageUrl="~/RESOURCES/FAVOURITE.png" />
                                    <asp:ImageButton ID="ImageButton_FavourOrder_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/HOVERFRAME.png" OnClick="Button_Favourite_Click" />
                                </div>

                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton_SalesOrder" class="menuimage_nohighlight" runat="server" ImageUrl="~/RESOURCES/ADDSALESLINE.png" />
                                    <asp:ImageButton ID="ImageButton_SalesOrder_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/HOVERFRAME.png" OnClick="Button_SalesOrder_Click" />
                                </div>

                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton_DeliveryCheck" class="menuimage_nohighlight" runat="server" ImageUrl="~/RESOURCES/DELIVERY.png" />
                                    <asp:ImageButton ID="ImageButton_DeliveryCheck_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/HOVERFRAME.png" OnClick="Button_DeliveryCheck_Click" />
                                </div>

                                <div class="col-12 col-s-12" id="dvSalesOrderLine" runat="server">
                                    <!--view of general table-->
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Sales_Order_Line" runat="server" Text="Sales Order Line" class="accordion_style_sub_fixed_darkcyan" />
                                    </div>
                                    <div class="col-12 col-s-12">
                                        <div class="col-5 col-s-6">
                                            <asp:Button ID="Button_Submit_SalesOrderLine" runat="server" OnClick="Button_Submit_SalesOrderLine_Click" Text="Submit Order" class="frame_style_type2" />
                                            <asp:Button ID="Button_Delete_SalesOrderLine" runat="server" OnClick="Button_Delete_SalesOrderLine_Click" Text="Delete Line" class="frame_style_type2" Visible="false" OnClientClick="return confirmDelete();" />
                                            <asp:Button ID="Button_Reset_CampaignID" runat="server" OnClick="Button_Reset_CampaignID_Click" Text="Reset Campaign ID" class="frame_style_type2" Visible="false" /> 
                                            </div>
                                        <div class="col-2 col-s-6">
                                            <asp:Label ID="Label_TotalQty" runat="server" CssClass="labeltext"></asp:Label><br />
                                            <asp:Label ID="Label_TotalAmount" runat="server" CssClass="labeltext"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                                        <asp:GridView ID="GridView_Sales_Order_Line_View" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            Style="overflow: hidden;" OnRowDataBound="GridView_Sales_Order_Line_View_RowDataBound"
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
                                                <asp:BoundField DataField="Misc Charge" HeaderText="Misc Charge" />
                                                <asp:BoundField DataField="Item Id" HeaderText="Item Id" />
                                                <asp:BoundField DataField="Item Name" HeaderText="Item Name" />
                                                <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                <asp:BoundField DataField="Price Each" HeaderText="Price Each (RM)" />
                                                <asp:BoundField DataField="Net Amount" HeaderText="Net Amount" />
                                                <asp:BoundField DataField="Dlv Date" HeaderText="Dlv Date" />
                                                <asp:BoundField DataField="Inv Qty" HeaderText="Inv Qty" />
                                                <asp:BoundField DataField="Dlv Qty" HeaderText="Dlv Qty" />
                                                <asp:BoundField DataField="Dlv Remainder" HeaderText="Dlv Remainder" />
                                                <asp:BoundField DataField="Hidden_RecId" HeaderText="Hidden_RecId" />
                                                <asp:BoundField DataField="Hidden_allow_alter" HeaderText="Hidden_allow_alter" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>

                                <div class="col-12 col-s-12" id="dvGatePass" runat="server">
                                    <asp:Button ID="Button_DeliveryCheck" runat="server" Text="Delivery Check" class="accordion_style_sub_fixed_darkcyan" Visible="false" />

                                    <asp:GridView ID="gvGatePass" runat="server"
                                        CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        HtmlEncode="False" AllowCustomPaging="true" Style="overflow: hidden;" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="No." />
                                            <asp:BoundField DataField="Sales ID" HeaderText="Sales ID" />
                                            <asp:BoundField DataField="Date" HeaderText="Date" />
                                            <asp:BoundField DataField="Invoice ID" HeaderText="Invoice ID" />
                                            <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" />
                                            <asp:BoundField DataField="Voucher" HeaderText="Voucher" />
                                            <asp:BoundField DataField="Currency" HeaderText="Currency" />
                                            <asp:BoundField DataField="Invoice Amount" HeaderText="Invoice Amount" />
                                            <asp:BoundField DataField="Gate Pass" HeaderText="Gate Pass" />
                                            <asp:BoundField DataField="Gate Pass Date" HeaderText="Gate Pass Date" />
                                            <asp:BoundField DataField="Vehicle No." HeaderText="Vehicle No." />
                                            <asp:BoundField DataField="Transporter Name" HeaderText="Transporter Name" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <!--end of view of general table-->

                            <!--Promotion_Campaign////////////////////////////////////////////////////////////////////////////////////-->
                            <div id="Promotion_Campaign_section" visible="false" runat="server">
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_Next_Promotion" runat="server" OnClick="Button_Next_Promotion_Click" Text="NEXT" class="frame_style_type2" />
                                    &nbsp
                                <asp:Button ID="Button_Back_Promotion" runat="server" CausesValidation="false" OnClick="Button_Back_Promotion_Click" Text="BACK" class="frame_style_type2" />
                                    <asp:Label ID="hidden_layer2_SO_number" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_layer2_camp_id" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_layer2_camp_type" class="gettext" runat="server" Visible="false"></asp:Label>
                                </div>

                                <div id="Promotion_Layer1" runat="server">
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Promotion_Campaign" runat="server" Text="Promotion" class="accordion_style_sub_fixed_darkcyan" /><br />
                                        <br />

                                        <asp:Label ID="Label_notice_Promotion_Campaign" class="gettext" runat="server" Text="Please tick on the checkbox (only one)." Style="font-weight: bold; color: black;"></asp:Label>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <asp:GridView ID="GridView_Promotion_Campaign" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            Style="overflow: hidden;"
                                            AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRow_Promotion_Campaign" runat="server" OnCheckedChanged="CheckBox_Promotion_Campaign" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Campaign" HeaderText="Campaign" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div id="Promotion_Layer2" runat="server">
                                    <!--section 1-->
                                    <!--==========================================================================================================================-->
                                    <div id="Section1" class="col-12 col-s-12" runat="server" visible="false">

                                        <asp:Button ID="Button_Section1" runat="server" Text="Section 1: FOC item" class="accordion_style_sub_fixed_darkcyan" /><br />
                                        <br />
                                        <!--==========================================================================================================================-->

                                        <div class="col-12 col-s-12" id="div_GridView_PromoSection1_sum" runat="server">
                                            <asp:Label ID="Label_PromoSection1_Total_Note" class="gettext" runat="server" Text="" Style="font-weight: bold;"></asp:Label>&nbsp
                                        <asp:Label ID="Label_PromoSection1_Total" class="gettext" runat="server" Text="" Style="font-weight: bold; color: black;"></asp:Label><br />
                                            <asp:Label ID="Label_PromoSection1_Remaining_Note" class="gettext" runat="server" Text="Quantity Remaining for selection: " Style="font-weight: bold;"></asp:Label>&nbsp
                                        <asp:Label ID="Label_PromoSection1_Remaining" class="gettext" runat="server" Text="" Style="font-weight: bold; color: black;"></asp:Label>
                                            <asp:Label ID="hidden_ItemId_Sum_Section1" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="hidden_Qty_Sum_Section1" class="gettext" runat="server" Visible="false"></asp:Label>

                                            <asp:GridView ID="GridView_PromoSection1_sum" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow_PromoSection1_sum" runat="server" OnCheckedChanged="CheckBox_PromoSection1_sum"
                                                                AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item" />

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_PromoQTYSec1_sum" autocomplete="off" runat="server" AutoPostBack="true" CssClass="inputtext"
                                                                OnTextChanged="TextBox_PromoQTYSec1_sum_changed"></asp:TextBox>
                                                            <br />
                                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_PromoQTYSec1_sum"
                                                                ErrorMessage="Invalid Quantity!" MaximumValue="99"
                                                                MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                            </asp:RangeValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                    <asp:BoundField DataField="Level" HeaderText="Level" />--%>
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <%--                                                    <asp:BoundField DataField="vtype" HeaderText="vtype" />--%>
                                                    <asp:BoundField DataField="unitid" HeaderText="unitid" />
                                                    <%--                                                    <asp:BoundField DataField="section" HeaderText="section" />--%>
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCFQty" />
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCPrice" />
                                                    <asp:BoundField DataField="FOCVol" HeaderText="FOCVol" />
                                                    <asp:BoundField DataField="FOCFQtyVol" HeaderText="FOCFQtyVol" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <!--==========================================================================================================================-->

                                        <div class="col-12 col-s-12" id="div_GridView_PromoSection1_fixed" runat="server">
                                            <asp:Label ID="hidden_ItemId_fixed_Section1" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="hidden_Qty_fixed_Section1" class="gettext" runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="Label_vtype_Sec1_fixed" class="gettext" runat="server" Text="" Style="color: #b4b4b4;"></asp:Label>
                                            <asp:GridView ID="GridView_PromoSection1_fixed" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow_PromoSection1_fixed" runat="server" OnCheckedChanged="CheckBox_PromoSection1_fixed" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item" />
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_PromoQTYSec1_fixed" class="inputtext" runat="server" Enabled="false" Style="font-weight: bold; color: red;"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                    <asp:BoundField DataField="Level" HeaderText="Level" />--%>
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <%--                                                    <asp:BoundField DataField="vtype" HeaderText="vtype" />--%>
                                                    <asp:BoundField DataField="unitid" HeaderText="unitid" />
                                                    <%--                                                    <asp:BoundField DataField="section" HeaderText="section" />--%>
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCFQty" />
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCPrice" />
                                                    <asp:BoundField DataField="FOCVol" HeaderText="FOCVol" />
                                                    <asp:BoundField DataField="FOCFQtyVol" HeaderText="FOCFQtyVol" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <!--==========================================================================================================================-->

                                    </div>
                                    <!--section 2-->
                                    <!--==========================================================================================================================-->
                                    <div id="Section2" class="col-12 col-s-12" runat="server" visible="false">
                                        <asp:Button ID="Button_Section2" runat="server" Text="Section 2: PWP" class="accordion_style_sub_fixed_darkcyan" /><br />
                                        <br />
                                        <div id="Type6_Option" class="col-12 col-s-12" runat="server" visible="false">
                                            <asp:Label ID="Label_Type6_Option" class="gettext" runat="server" Text="*Note: Select either Option A or Option B." Style="color: #b4b4b4;"></asp:Label><br />
                                            <div class="image_properties">
                                                <asp:ImageButton ID="ImageButtonOptionA" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/OptionA.png" />
                                                <asp:ImageButton ID="ImageButtonOptionA_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/OptionA_h.png" OnClick="ImageButtonOptionA_Click" />
                                            </div>
                                            &nbsp
                                        <div class="image_properties">
                                            <asp:ImageButton ID="ImageButtonOptionB" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/OptionB.png" />
                                            <asp:ImageButton ID="ImageButtonOptionB_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/OptionB_h.png" OnClick="ImageButtonOptionB_Click" />
                                        </div>
                                        </div>
                                        <!--==========================================================================================================================-->

                                        <div class="col-12 col-s-12" id="div_GridView_PromoSection2_sum" runat="server">
                                            <asp:Label ID="Label_PromoSection2_Total_Note" class="gettext" runat="server" Text="" Style="font-weight: bold;"></asp:Label>&nbsp
                                        <asp:Label ID="Label_PromoSection2_Total" class="gettext" runat="server" Text="" Style="font-weight: bold; color: black;"></asp:Label><br />
                                            <asp:Label ID="Label_PromoSection2_Remaining_Note" class="gettext" runat="server" Text="Quantity Remaining for selection: " Style="font-weight: bold;"></asp:Label>&nbsp
                                        <asp:Label ID="Label_PromoSection2_Remaining" class="gettext" runat="server" Text="" Style="font-weight: bold; color: black;"></asp:Label>
                                            <asp:Label ID="hidden_ItemId_Sum_Section2" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="hidden_Qty_Sum_Section2" class="gettext" runat="server" Visible="false"></asp:Label>

                                            <asp:GridView ID="GridView_PromoSection2_sum" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">

                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow_PromoSection2_sum" runat="server" OnCheckedChanged="CheckBox_PromoSection2_sum" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item" />

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_PromoQTYSec2_sum" class="inputtext" autocomplete="off" runat="server" AutoPostBack="true"
                                                                OnTextChanged="TextBox_PromoQTYSec2_sum_changed" Enabled="false" ReadOnly="true"></asp:TextBox>
                                                            <%--                                                            <br />
                                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_PromoQTYSec2_sum"
                                                                ErrorMessage="Invalid Quantity!" MaximumValue="99"
                                                                MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                            </asp:RangeValidator>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                    <asp:BoundField DataField="Level" HeaderText="Level" />--%>
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <%--                                                    <asp:BoundField DataField="vtype" HeaderText="vtype" />--%>
                                                    <asp:BoundField DataField="unitid" HeaderText="unitid" />
                                                    <%--                                                    <asp:BoundField DataField="section" HeaderText="section" />--%>
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCFQty" />
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCPrice" />
                                                    <asp:BoundField DataField="FOCVol" HeaderText="FOCVol" />
                                                    <asp:BoundField DataField="FOCFQtyVol" HeaderText="FOCFQtyVol" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <!--==========================================================================================================================-->

                                        <div class="col-12 col-s-12" id="div_GridView_PromoSection2_fixed" runat="server">
                                            <asp:Label ID="hidden_ItemId_fixed_Section2" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="hidden_Qty_fixed_Section2" class="gettext" runat="server" Visible="false"></asp:Label>

                                            <asp:Label ID="Label_vtype_Sec2_fixed" class="gettext" runat="server" Text="" Style="color: #b4b4b4;"></asp:Label>
                                            <asp:GridView ID="GridView_PromoSection2_fixed" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow_PromoSection2_fixed" runat="server" OnCheckedChanged="CheckBox_PromoSection2_fixed" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item" />

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_PromoQTYSec2_fixed" class="inputtext" runat="server" Enabled="false" Style="font-weight: bold; color: red;"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                    <asp:BoundField DataField="Level" HeaderText="Level" />--%>
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity Entitle" />
                                                    <%--<asp:BoundField DataField="vtype" HeaderText="vtype" />--%>
                                                    <asp:BoundField DataField="unitid" HeaderText="unitid" />
                                                    <%--                                                    <asp:BoundField DataField="section" HeaderText="section" />--%>
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCFQty" />
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCPrice" />
                                                    <asp:BoundField DataField="FOCVol" HeaderText="FOCVol" />
                                                    <asp:BoundField DataField="FOCFQtyVol" HeaderText="FOCFQtyVol" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <!--==========================================================================================================================-->
                                    </div>

                                    <!--section 3-->
                                    <!--==========================================================================================================================-->
                                    <div id="Section3" class="col-12 col-s-12" runat="server" visible="false">
                                        <asp:Button ID="Button_Section3" runat="server" Text="Section 3: FOC" class="accordion_style_sub_fixed_darkcyan" /><br />
                                        <br />

                                        <!--==========================================================================================================================-->

                                        <div class="col-12 col-s-12" id="div_GridView_PromoSection3_sum" runat="server">
                                            <asp:Label ID="Label_PromoSection3_Total_Note" class="gettext" runat="server" Text="" Style="font-weight: bold;"></asp:Label>&nbsp
                                        <asp:Label ID="Label_PromoSection3_Total" class="gettext" runat="server" Text="" Style="font-weight: bold; color: black;"></asp:Label><br />
                                            <asp:Label ID="Label_PromoSection3_Remaining_Note" class="gettext" runat="server" Text="Quantity Remaining for selection: " Style="font-weight: bold;"></asp:Label>&nbsp
                                        <asp:Label ID="Label_PromoSection3_Remaining" class="gettext" runat="server" Text="" Style="font-weight: bold; color: black;"></asp:Label>
                                            <asp:Label ID="hidden_ItemId_Sum_Section3" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="hidden_Qty_Sum_Section3" class="gettext" runat="server" Visible="false"></asp:Label>

                                            <asp:GridView ID="GridView_PromoSection3_sum" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow_PromoSection3_sum" runat="server" OnCheckedChanged="CheckBox_PromoSection3_sum" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item" />
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_PromoQTYSec3_sum" class="inputtext" autocomplete="off" runat="server" AutoPostBack="true"
                                                                OnTextChanged="TextBox_PromoQTYSec3_sum_changed"></asp:TextBox>
                                                            <br />
                                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_PromoQTYSec3_sum"
                                                                ErrorMessage="Invalid Quantity!" MaximumValue="99"
                                                                MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                            </asp:RangeValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                    <asp:BoundField DataField="Level" HeaderText="Level" />--%>
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <%--                                                    <asp:BoundField DataField="vtype" HeaderText="vtype" />--%>
                                                    <asp:BoundField DataField="unitid" HeaderText="unitid" />
                                                    <%--                                                    <asp:BoundField DataField="section" HeaderText="section" />--%>
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCFQty" />
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCPrice" />
                                                    <asp:BoundField DataField="FOCVol" HeaderText="FOCVol" />
                                                    <asp:BoundField DataField="FOCFQtyVol" HeaderText="FOCFQtyVol" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <!--==========================================================================================================================-->

                                        <div class="col-12 col-s-12" id="div_GridView_PromoSection3_fixed" runat="server">
                                            <asp:Label ID="hidden_ItemId_fixed_Section3" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="hidden_Qty_fixed_Section3" class="gettext" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="Label_vtype_Sec3_fixed" class="gettext" runat="server" Text="" Style="color: #b4b4b4;"></asp:Label>
                                            <asp:GridView ID="GridView_PromoSection3_fixed" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow_PromoSection3_fixed" runat="server" OnCheckedChanged="CheckBox_PromoSection3_fixed" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemId" HeaderText="ItemId" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item" />

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_PromoQTYSec3_fixed" class="inputtext" runat="server" Enabled="false" Style="font-weight: bold; color: red;"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--                                                    <asp:BoundField DataField="Level" HeaderText="Level" />--%>
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity Entitle" />
                                                    <%--                                                    <asp:BoundField DataField="vtype" HeaderText="vtype" />--%>
                                                    <asp:BoundField DataField="unitid" HeaderText="unitid" />
                                                    <%--                                                    <asp:BoundField DataField="section" HeaderText="section" />--%>
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCFQty" />
                                                    <asp:BoundField DataField="FOCPrice" HeaderText="FOCPrice" />
                                                    <asp:BoundField DataField="FOCVol" HeaderText="FOCVol" />
                                                    <asp:BoundField DataField="FOCFQtyVol" HeaderText="FOCFQtyVol" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <!--==========================================================================================================================-->
                                    </div>
                                </div>

                            </div>
                            <!--end of Promotion_Campaign-->
                            <!--FV order ////////////////////////////////////////////////////////////////////////////////////-->

                            <div id="FV_order_section" visible="false" runat="server">

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_SaveFavourite" runat="server" OnClick="Button_SaveFavourite_Click" Text="Save Favourite" class="frame_style_type2" />
                                    &nbsp
                                <asp:Button ID="Button_CancelFavourite" runat="server" CausesValidation="false" OnClick="Button_CancelFavourite_Click" Text="Cancel" class="frame_style_type2" />
                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_FV_Order" runat="server" Text="Favour Order" class="accordion_style_sub_fixed_darkcyan" />
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
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />

                                            <asp:TemplateField HeaderText="Item Id">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DropDownList_FV_Order" runat="server" class="inputtext" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="New Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox_New_QTY" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:RangeValidator runat="server" ControlToValidate="TextBox_New_QTY"
                                                        ErrorMessage="Invalid Quantity!" MaximumValue="999"
                                                        MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                    </asp:RangeValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>
                            <!--end of FV order ////////////////////////////////////////////////////////////////////////////////////-->
                            <!--Sales Line (previously named as Sales Order)////////////////////////////////////////////////////////////////////////////////////-->
                            <div id="Sales_order_section" visible="false" runat="server">

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_SaveSalesOrder" runat="server" OnClick="Button_SaveSalesOrder_Click" Text="Save Order" class="frame_style_type2" />
                                    &nbsp
                                <asp:Button ID="Button_CancelSalesOrder" runat="server" CausesValidation="false" OnClick="Button_CancelSalesOrder_Click" Text="Cancel" class="frame_style_type2" />
                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_Sales_Order" runat="server" Text="Sales Order" class="accordion_style_sub_fixed_darkcyan" />
                                </div>

                                <!--==============================================================================-->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Id:      </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_Id" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        <div id="checkun2" runat="server" visible="false">
                                            <asp:Image ID="shwimg2" class="image_indicator" runat="server" />
                                            <asp:Label ID="lblmsg2" class="indicator" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-2 col-s-8">
                                        <asp:Button ID="Button_TextBox_Id" runat="server" OnClick="CheckId" Text="Validate" class="glow_green" UseSubmitBehavior="false"/>
                                        <asp:HiddenField ID="hiddenDataPassing" runat="server" /><asp:Button ID="btnHiddenPostback" runat="server" OnClick="btnHiddenPostback_Click" style="display:none" />
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Button ID="Button_InventoryMasterList" runat="server" OnClick="CheckIdInList" CausesValidation="false" Text="Find in list" class="glow" />
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Name:</label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_IdName" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">

                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Warehouse:  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_Warehouse" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Unit:  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:DropDownList ID="DropDownList_Unit" runat="server" class="dropdownlist" />
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Quantity:  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_SLQuantity" class="inputtext" autocomplete="off" runat="server" OnTextChanged="TextBox_SLQuantity_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <br />
                                        <asp:RangeValidator runat="server" ControlToValidate="TextBox_SLQuantity"
                                            ErrorMessage="Invalid Quantity!" MaximumValue="999"
                                            MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;" SetFocusOnError="True">
                                        </asp:RangeValidator>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Price (RM):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_Price" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Address:  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_SalesLine_Address" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>
                                <!--============================================================================== -->

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Disc. pct. (%):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_Disc_pct" class="gettext" runat="server" Text=" "></asp:Label>
                                        %
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Discount (RM):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_Discount" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Multiline disc. pct. (%):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_MultilineDisc_pct" class="gettext" runat="server" Text=" "></asp:Label>
                                        %
                                    </div>
                                </div>

                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Multiline discount (RM):  </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_MultilineDiscount" class="gettext" runat="server" Text=" "></asp:Label>
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
                                <asp:HiddenField ID="hdnUserResponse" runat="server" />
                                <asp:HiddenField ID="hdLFStatus" runat="server" />

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
                        <asp:Button ID="Button_ListOutStanding" runat="server" OnClick="Button_ListOutStanding_Click" Text="List OutStanding" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />&nbsp
                                <asp:Button ID="Button_ListOutStandingBlocked" runat="server" OnClick="Button_ListOutStandingBlocked_Click" Text="List OutStanding Blocked"
                                    class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />&nbsp
                                <asp:Button ID="Button_ListAll" runat="server" OnClick="Button_ListAll_Click" Text="List All" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />&nbsp
                        <asp:Button ID="Button_DeliverTrack" runat="server" Text="Delivery Tracking" OnClick="Button_DeliverTrack_Click" class="frame_style_type2" />
                        <asp:Button ID="Button_UnfulfillOrder" runat="server" Text="Unfulfill Customer's Order" OnClick="Button_UnfulfillOrder_Click" class="frame_style_type2" />
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="confirmation_section_general" visible="false" runat="server">
                                <!--start of view of general table-->
                                <div class="col-12 col-s-12">

                                    <!--view of general table-->
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Overview_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" />
                                    </div>
                                    <div class="col-12 col-s-12" id="div_Searchable" runat="server">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search By:      </label>
                                            </div>
                                            <div class="col-3 col-s-8">
                                                <asp:DropDownList ID="DropDownList_Search" runat="server" class="dropdownlist">
                                                    <asp:ListItem Text="Customer Name" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Account No." Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Sales Id." Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search:      </label>
                                            </div>
                                            <div class="col-5 col-s-8">
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
                                                HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False" EnableSortingAndPagingCallbacks="True" PagerSettings-PageButtonCount="100">
                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:TemplateField HeaderText="Sales Id">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button_SalesId" runat="server" OnClick="Button_SalesId_Click" CausesValidation="false" Text='<%# Eval("Sales Id") %>' class="button_grid" CommandArgument='<%# Eval("Submitted") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Sales Id" HeaderText="Sales Id" />
                                                    <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                                    <asp:BoundField DataField="Customer Acc." HeaderText="Customer Acc." />
                                                    <asp:BoundField DataField="Dl. Date" HeaderText="Dl. Date" />
                                                    <asp:BoundField DataField="Payment" HeaderText="Payment" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                                    <asp:BoundField DataField="LFI Status" HeaderText="LFI Status" />
                                                    <asp:BoundField DataField="AutoPost" HeaderText="Auto Post" />
                                                    <asp:BoundField DataField="Submitted" HeaderText="Submitted" />
                                                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" />
                                                    <asp:BoundField DataField="Reference" HeaderText="Reference" />

                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerSettings PageButtonCount="2" />
                                                <RowStyle CssClass="rows" />
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12" id="dvDeliveryTracking" runat="server" visible="false" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <asp:GridView ID="gvDeliveryTracking" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            AllowPaging="true" OnPageIndexChanging="gvDeliveryTracking_PageIndexChanging" AllowCustomPaging="True"
                                            HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:TemplateField HeaderText="Sales Id">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_SalesId" runat="server" OnClick="Button_SalesId_Click" CausesValidation="false" Text='<%# Eval("Sales Id") %>' class="button_grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Sales ID" HeaderText="Sales ID" />
                                                <asp:BoundField DataField="Salesman ID" HeaderText="Salesman ID" />
                                                <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                                <asp:BoundField DataField="Customer Acc." HeaderText="Customer Acc." />
                                                <asp:BoundField DataField="Invoice ID" HeaderText="Invoice ID" />
                                                <asp:BoundField DataField="Picking ID" HeaderText="Picking ID" />
                                                <asp:BoundField DataField="Gate Pass" HeaderText="Gate Pass" />
                                                <asp:BoundField DataField="Gate Pass Date" HeaderText="Gate Pass Date" />
                                                <asp:BoundField DataField="Transporter Name" HeaderText="Transporter Name" />
                                                <asp:BoundField DataField="Vehicle No." HeaderText="Vehicle No." />
                                                <asp:BoundField DataField="Reversed Date" HeaderText="Reversed Date" />
                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div id="DivUnfulfillOrder" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;" visible="false">
                                            <asp:GridView ID="gvUnfulfillOrder" runat="server"
                                                PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                                HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False" EnableSortingAndPagingCallbacks="True" PagerSettings-PageButtonCount="100">
                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:TemplateField HeaderText="Sales Id">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button_SalesId" runat="server" OnClick="Button_SalesId_Click" CausesValidation="false" Text='<%# Eval("Sales Id") %>' class="button_grid" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Sales Id" HeaderText="Sales Id" />
                                                    <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                                    <asp:BoundField DataField="Customer Acc." HeaderText="Customer Acc." />
                                                    <asp:BoundField DataField="Dl. Date" HeaderText="Dl. Date" />
                                                    <asp:BoundField DataField="Item Name" HeaderText="Item Name" />
                                                    <asp:BoundField DataField="Item ID" HeaderText="Item ID" />
                                                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <asp:BoundField DataField="Dlv Quantity" HeaderText="Dlv Quantity" />
                                                    <asp:BoundField DataField="Dlv Remainder" HeaderText="Dlv Remainder" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                                    <asp:BoundField DataField="LFI Status" HeaderText="LFI Status" />
                                                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" />

                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerSettings PageButtonCount="2" />
                                                <RowStyle CssClass="rows" />
                                            </asp:GridView>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <!--end of view of general table-->
                            <!--===============================================================================================-->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!--end of confirmation_section/ Overview////////////////////////////////////////////////////////////////////////////////////-->
                <!--enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress16" class="gettext" AssociatedUpdatePanelID="UpdatePanel16">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div id="enquiries_section" style="display: none" runat="server">

                    <!--===============================================================================================-->
                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_SalesmanTotal" runat="server" OnClick="Button_SalesmanTotal_Click" Text="Salesman Total" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                        &nbsp
                                <asp:Button ID="Button_BatteryOutstanding" runat="server" OnClick="Button_BatteryOutstanding_Click" Text="Battery Outstanding" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                        &nbsp
                        <asp:Button ID="Button_QTDSalesmanInvPerform" runat="server" OnClick="Button_QTDSalesmanInvPerform_Click" Text="Quatery Salesman Enquiries" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                         &nbsp                        
                    </div>
                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_enquiries_section_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" />
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                        <ContentTemplate>
                            <!--===============================================================================================-->
                            <div id="SalesmanTotal_section" visible="false" runat="server">
                                <div class="col-12 col-s-12">
                                    <!--===========================================Salesman====================================================-->
                                    <div class="col-12 col-s-12">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Salesman:      </label>
                                            </div>
                                            <div class="col-6 col-s-8">
                                                <asp:ImageButton ID="ImageButton8" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_SalesmanTotal_Click" />
                                                <div class="col-9 col-s-9">
                                                    <asp:DropDownList ID="DropDownList2" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList2">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="col-12 col-s-12">
                                                <div class="col-6 col-s-12">

                                                    <div class="col-3 col-s-4">
                                                        <label class="labeltext">Start Date:</label>
                                                    </div>

                                                    <div class="col-3 col-s-5">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtStartDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                                            <label class="input-group-btn" for="txtStartDate">
                                                                <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-6 col-s-12">
                                                    <div class="col-3 col-s-4">
                                                        <label class="labeltext">End Date:</label>
                                                    </div>

                                                    <div class="col-3 col-s-5">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtEndDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                                            <label class="input-group-btn" for="txtEndDate">
                                                                <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="DropDownList2" />
                                            <asp:PostBackTrigger ControlID="ImageButton8" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <div class="col-12 col-s-12" style="border: solid">
                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Current Sales ID Total:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_CurrentSalesIDTotal" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Total:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_SalesmanTotal" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <!--===========================================Salesman-1====================================================-->
                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <asp:Label ID="labeltext1" runat="server" Text="Salesman - 1" class="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-8">
                                                    <div class="col-9 col-s-9">
                                                        <asp:Label ID="lblSalesman1" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Current Sales ID Total:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_CurrentSalesIDTotal1" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Total:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_SalesmanTotal1" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Subtotal:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_SalesmanSubTotal" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--===========================================Salesman-2====================================================-->
                                    <div class="col-12 col-s-12" style="border: solid; border-top: none;">
                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <asp:Label ID="labeltext2" runat="server" Text="Salesman - 2" class="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-8">
                                                    <div class="col-9 col-s-9">
                                                        <asp:Label ID="lblSalesman2" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Current Sales ID Total:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_CurrentSalesIDTotal2" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Total:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:Label ID="Label_SalesmanTotal2" class="gettext" runat="server" Text=" "></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Label ID="hidden_Label_TableRun" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                                    <asp:Label ID="hidden_Label_Test" class="gettext" runat="server" Visible="false"></asp:Label>
                                </div>

                            </div>
                            <!--===============================================================================================-->
                            <div id="BatteryOutstanding_section" visible="false" runat="server">
                                <asp:GridView ID="GridView_BatteryOutstanding" runat="server"
                                    CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                    HtmlEncode="False" AllowCustomPaging="true"
                                    Style="overflow: hidden;"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="No." HeaderText="No." />
                                        <asp:BoundField DataField="Serial No." HeaderText="Serial No." />
                                        <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                        <asp:BoundField DataField="Invoice ID" HeaderText="Invoice ID" />
                                        <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <!--===============================================================================================-->
                            <div id="QTDSmenInvoicePerformance_section" visible="false" runat="server"> 
                                <div class="col-12 col-sm-12">
                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Salesman:      </label>
                                            </div>
                                            <div class="col-8 col-sm-6">
                                                <%--<asp:ImageButton ID="ImageButton1" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_SmenInvoicePerformance_Click" />--%>
                                                <div class="col-9 col-sm-9">
                                                    <asp:DropDownList ID="ddlSmenInvoicePerformance" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_ddlSmenInvoicePerformance">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                </div>
                                    <div class="col-12 col-sm-12">

                                        <div class="col-8 col-sm-4">
                                            <div class="col-4 col-sm-3">
                                                    <label class="labeltext">Year Select:</label>
                                            </div>
                                            <div class="col-8 col-sm-6">
                                                    <asp:DropDownList ID="ddlYear" runat="server" class="dropdownlist" AutoPostBack="true" 
                                                        OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                             </div>

                                        <div class="col-8 col-sm-4">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Quarter Select:</label>
                                            </div>
                                            <div class="col-8 col-sm-6">
                                                <asp:DropDownList ID="ddlQuarter" runat="server" class="dropdownlist" AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ddlQuarter_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnSelectedQuarterRange" runat="server" />
                                                <asp:Label ID="lblQuarterStartDate" runat="server" Visible="false" />
                                                <asp:Label ID="lblQuarterEndDate" runat="server" Visible="false" />
                                            </div>
                                        </div>

                                        <div class="col-3 col-s-6">
                                             <%--<asp:Button ID="Button_FindList" OnClick="CheckAccInList2" runat="server" CausesValidation="false" Text="Search Customer" class="glow" Style="margin-bottom: 4px" />
--%>                                         <asp:Button ID="btnGetReport_1" Text="Generate" runat="server" OnClick="Button_Generate_SmenInvoicePerformance_Click" class="getStatement" Style="margin-bottom: 4px" Visible="false" />
                                         </div>
                                    </div>
                                    <div id="gridviewReport" class="container1" runat="server">
                                            <div class="col-12 col-s-12" id="salesreport" runat="server">
                                                <asp:Label runat="server" ID="lblAutoGenerated" Font-Underline="true" Font-Bold="true" ForeColor="Red"></asp:Label><br />
                                                    <asp:Label ID="lblDescribesQtyInvPerf" runat="server"></asp:Label><asp:Label ID="lblDateQtyInvPerf" runat="server" Font-Bold="true"></asp:Label><br />
                                                <br />
                                            <asp:GridView ID="gvReport" runat="server" HeaderStyle-CssClass="header" HorizontalAlign="Left"
                            RowStyle-CssClass="rows" Style="overflow: auto" AutoGenerateColumns="false" OnRowCreated="gvReport_RowCreated">
                                    <Columns>
                                            <asp:BoundField DataField="Sales ID" HeaderText="Sman ID" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Sman Name" HeaderText="Sman Name" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Actual Total (QTD)" HeaderText="Actual Total (QTD)" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Target" HeaderText="Target" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Balance vs T1" HeaderText="Balance vs T1" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Balance vs T2" HeaderText="Balance vs T2" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Actual (firstM)" HeaderText="Actual (firstM)" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Actual (seconM)" HeaderText="Actual (seconM)" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Actual (thirdM)" HeaderText="Actual (thirdM)" ItemStyle-Width="150" HtmlEncode="false" />
                                            <asp:BoundField DataField="Actual Last Year Sales" HeaderText="Actual Last Year Sales" ItemStyle-Width="150" HtmlEncode="false" Visible="false" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="150" HtmlEncode="false"  Visible="false"/>
                                    </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div id="myModal" class="modal">
                    <div class="modal-content">
                        <span class="close" onclick="closeModal()">&times;</span>
                        <p id="messageText"></p>
                        <asp:Button ID="btnYes" runat="server" Text="Yes" OnClientClick="onYesClick();" OnClick="btnYes_Click" />
                        <asp:Button ID="btnNo" runat="server" Text="No" />
                    </div>
                </div>
                <style>
                    .modal {
                        display: none;
                        position: fixed;
                        z-index: 1;
                        left: 0;
                        top: 0;
                        width: 100%;
                        height: 100%;
                        overflow: auto;
                        background-color: rgba(0, 0, 0, 0.4);
                    }

                    .modal-content {
                        background-color: #f2f2f2;
                        margin: 15% auto;
                        padding: 20px;
                        border: 2px solid #f58345;
                        width: 300px;
                    }

                    .close {
                        float: right;
                        cursor: pointer;
                    }
                </style>

                <!--end of enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
            </div>
        </div>
        <script src="scripts/ButtonUp.js"></script>
    </form>
</body>
</html>
