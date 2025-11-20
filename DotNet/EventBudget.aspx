<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventBudget.aspx.cs" Inherits="DotNet.EventBudget" %>

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
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.0/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.0/themes/base/jquery-ui.css" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <title></title>
</head>
<script type="text/javascript">
    function saveRecord() {
        var gridView = document.getElementById('<%= gvNewParticipation.ClientID %>'); // Get the GridView
        var rows = gridView.getElementsByTagName("td"); // Get all the rows in the GridView

        // Initialize arrays to store the data from each row
        var pax = [];
        var totalRoom = [];
        var chkAllowance = [];
        var custAccount = [];
        var custName = [];

        // Loop through each row in the GridView, skipping the header row
        for (var i = 1; i < rows.length; i++) {
            var row = rows[i];

            // Extract values from the respective cells (adjust cell index based on your GridView layout)
            var paxValue = row.cells[4].getElementsByTagName("input")[0].value;  // Assuming Pax is in the first column
            var totalRoomValue = row.cells[5].getElementsByTagName("input")[0].value;  // Assuming Total Room is in the second column
            var chkAllowanceValue = row.cells[6].getElementsByTagName("input")[0].checked ? "Yes" : "No";  // Assuming checkbox is in the third column
            var custAccountValue = row.cells[2].innerText;  // Assuming CustAccount is in the fourth column
            var custNameValue = row.cells[1].innerText;  // Assuming CustName is in the fifth column

            // Push the values into their respective arrays
            pax.push(paxValue);
            totalRoom.push(totalRoomValue);
            chkAllowance.push(chkAllowanceValue);
            custAccount.push(custAccountValue);
            custName.push(custNameValue);
        }

        // Make an AJAX call for each row in the GridView
        for (var i = 0; i < rows.length - 1; i++) {
            $.ajax({
                type: "POST",
                url: "EventBudget.aspx/UpdateRecord",  // The URL to your WebMethod
                data: JSON.stringify({
                    pax: pax[i],
                    totalRoom: totalRoom[i],
                    chkAllowance: chkAllowance[i],
                    eventCode: $("#ddlEventCode").val(),  // Assuming you have a dropdown for event code
                    account: custAccount[i],
                    salesman: $("#Label_Salesman").text(),  // Assuming you have a label for salesman
                    custName: custName[i]
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Handle success if necessary
                },
                error: function (error) {
                    alert('Error: ' + error.responseText);
                }
            });
        }

        alert('All records updated successfully');
    }

</script>
<body>
    <form id="form1" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/EVENTBUDGET.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 style="font-weight: bold">Event Budget</h1>
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
                <a href="EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>
                <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
                <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
                <a href="SignboardEquipment.aspx" id="SignboardTag2" runat="server">Sign & Equip</a>
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
                    <asp:Button ID="Button_eb_header_section" runat="server" OnClick="eb_header_section_Click" Text="New" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_confirmation_section" runat="server" OnClick="confirmation_section_Click" Text="Overview" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_sales_order_section" runat="server" OnClick="sales_order_section_Click" Text="Summary" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_enquiries_section" runat="server" OnClick="enquiries_section_Click" Text="Administration" class="frame_style_4bar" />
                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <!--sales_header_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="col-12 col-s-12" runat="server" id="divNewSelection">
                    <asp:Button ID="btnForm" runat="server" OnClick="btnForm_Click" Text="New Form" class="frame_style_type2" />
                    &nbsp                               
                    <asp:Button ID="btnList" runat="server" OnClick="btnList_Click" Text="New List" class="frame_style_type2" />
                </div>

                <div id="eb_header_section" runat="server" visible="false">

                    <!--Customer ////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:Button ID="Accordion_sales_header_customer" runat="server" Text="Customer" class="accordion_style_sub_fixed_darkcyan" />

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
                        <div class="col-2 col-s-4">
                            <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find in list" class="glow" Style="margin-bottom: 4px" />
                        </div>
                        <div class="col-3 col-s-4">
                            <asp:Button ID="Button_CheckAcc" runat="server" OnClick="CheckAcc" CausesValidation="false" Text="Validate" class="glow_green" />
                        </div>

                    </div>
                    <!--============================================================================== -->
                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Customer Name:        </label>
                        </div>
                        <div class="col-6 col-s-8">
                            <asp:Label ID="Label_CustName" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Salesman:        </label>
                        </div>
                        <div class="col-6 col-s-8">
                            <asp:Label ID="Label_Salesman" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Salesman HOD:        </label>
                        </div>
                        <div class="col-6 col-s-8">
                            <asp:Label ID="Label_SalesmanHod" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <!--Event Selection Setup////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:Button ID="Accordion_sales_header_delivery" runat="server" Text="Event Selection Setup" class="accordion_style_sub_fixed_darkcyan" />

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Total Participant:</label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:TextBox ID="TextBox_TotalParticipant" TextMode="Number" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Gold Pendant:</label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:TextBox ID="TextBox_GoldPendant" class="gettext" TextMode="Number" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Voucher Cheque:</label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:TextBox ID="TextBox_VoucherCheque" class="gettext" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Number of Vegetarian(Pax):</label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:TextBox ID="TextBox_NoVegetarian" class="gettext" TextMode="Number" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Latest Invoice:</label>
                        </div>
                        <div class="col-3 col-s-5">
                            <asp:Label ID="Label_LatestInvoice" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <asp:Label ID="hidden_salesmanId" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_alt_address" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_alt_address_counter" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_Street" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_ZipCode" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_City" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_State" class="gettext" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="hidden_Country" class="gettext" runat="server" Visible="false"></asp:Label>

                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_SaveHeader" runat="server" Text="Save" class="frame_style_type2" OnClick="Button_SaveHeader_Click" />&nbsp
                    </div>

                </div>

                <div class="col-12 col-s-12" runat="server" id="divNewList_section" visible="false">
                    <asp:Button ID="Button1" runat="server" Text="Customer Setup" class="accordion_style_sub_fixed_darkcyan" />

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Event Code:</label>
                        </div>
                        <div class="col-3 col-s-5">
                            <asp:DropDownList ID="ddlEventCode" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlEventCode_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Location:</label>
                        </div>
                        <div class="col-2 col-s-6">
                            <asp:Label ID="Label_Location" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Event Date:</label>
                        </div>
                        <div class="col-2 col-s-6">
                            <asp:Label ID="Label_EventDate" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <label class="labeltext">Room Points:</label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:Label ID="Label_Rp" class="gettext" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;" class="col-12 col-s-12">
                        <asp:GridView ID="gvNewParticipation" runat="server" HorizontalAlign="Left" CssClass="mydatagrid" HeaderStyle-CssClass="header" PageSize="50"
                            RowStyle-CssClass="rows" OnRowDataBound="gvNewParticipation_RowDataBound" AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging"
                            AllowCustomPaging="True" HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="No." HeaderText="No." />
                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                <asp:BoundField DataField="Account No." HeaderText="Account No." />
                                <asp:BoundField DataField="Loyalty Point" HeaderText="Loyalty Point" />
                                <asp:TemplateField HeaderText="Pax Registered">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPaxRegistered" runat="server" Text='<%# Eval("Pax Registered") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Rooms">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotalRooms" runat="server" Text='<%# Eval("Total Rooms") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Petrol/Allowance">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkPetrolAllowance" runat="server" Text='<%# Eval("Petrol/Allowance") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <%--                            <HeaderStyle CssClass="header" />--%>
                            <PagerSettings PageButtonCount="5" />
                            <%--                            <RowStyle CssClass="rows" />--%>
                        </asp:GridView>
                    </div>

                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClientClick="saveRecord(); return false;" />
                    <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" CssClass="button" />

                </div>
                <!--Overall_sales_order_section ////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress6" class="gettext">
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

                <div id="divForm_section" style="display: none" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
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
                                                <asp:ListItem Text="Event No." Value="0"></asp:ListItem>
                                                <%--                                                    <asp:ListItem Text="Customer Account No." Value="1"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
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
                                    <div runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <asp:GridView ID="GridEventBudgetList" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                            HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:TemplateField HeaderText="Event">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_EventNum" runat="server" OnClick="Button_EventNum_Click" CausesValidation="false" Text='<%# Eval("Event") %>' class="button_grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Event Date" HeaderText="Start Date" />
                                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                                <asp:BoundField DataField="Standard Room" HeaderText="Standard Room" />
                                                <asp:BoundField DataField="Single Room" HeaderText="Single Room" />
                                                <asp:BoundField DataField="Budget" HeaderText="Budget" />
                                                <asp:BoundField DataField="Point" HeaderText="Point" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" />

                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!--end of confirmation_section/ Overview////////////////////////////////////////////////////////////////////////////////////-->
                <!--enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress16" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <!--end of enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
            </div>
        </div>
        <script src="scripts/ButtonUp.js"></script>
    </form>

</body>
</html>
