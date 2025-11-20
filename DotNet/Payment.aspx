<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="DotNet.Payment" MaintainScrollPositionOnPostback="true" %>

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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />

    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtChequeDate').datepicker({
                format: "dd/mm/yyyy",
            }).datepicker("setDate", 'now');

            $('#txtReceivedDate').datepicker({
                format: "dd/mm/yyyy",
            }).datepicker("setDate", 'now');
        });

        if ($('#txtChequeDate').val()) {
            datepicker('#txtChequeDate').val();
        }

        if ($('#txtReceivedDate').val()) {
            datepicker('#txtReceivedDate').val();
        }
    </script>

    <title>Payment</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

</head>
<body>
    <form id="form_Payment" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/PAYMENTT.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Payment</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="Payment.aspx" id="PaymentTag2" class="active" runat="server" visible="false">Payment</a>
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
                <!--content--==============================================================================-->
                <div class="col-12 col-s-12">
                    <asp:Button ID="Button_JournalTable_section" runat="server" OnClick="Button_JournalTable_section_Click" Text="New" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_JournalLine_section" runat="server" OnClick="Button_JournalLine_section_Click" Text="Line" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_Overview_section" runat="server" OnClick="Button_Overview_section_Click" Text="Overview" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_Enquiries_section" runat="server" OnClick="Button_Enquiries_section_Click" Text="Enquiries" class="frame_style_4bar" />
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <!--JournalTable_section////////////////////////////////////////////////////////////////////////////////////-->
                <div id="JournalTable_section" style="display: none" runat="server">
                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_SaveNewEntry" runat="server" OnClick="Button_SaveNewEntry_Click" Text="SAVE" class="frame_style_type2" />&nbsp
                        <asp:Button ID="Button_DeleteDuplicate" runat="server" OnClick="Button_DeleteDuplicate_Click" Text="Clear Duplicate J/N" class="glow" />
                    </div>

                    <!--Customer ////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:Button ID="Accordion_NewPayment_PaymentJournal" runat="server" Text="Payment Journal" class="accordion_style_sub_fixed_darkcyan" />

                    <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div class="loading">
                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext">Description:      </label>
                                </div>

                                <div class="col-10 col-s-6">
                                    <asp:TextBox ID="TextBox_Description" class="textbox" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext">Name of Journal:      </label>
                                </div>

                                <div class="col-10 col-s-6">
                                    <asp:DropDownList ID="DropDownList2" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList2">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!--end of JournalTable_section////////////////////////////////////////////////////////////////////////////////////-->

                <!--JournalLine_section////////////////////////////////////////////////////////////////////////////////////-->
                <div id="JournalLine_section" style="display: none" runat="server">
                    <asp:UpdateProgress runat="server" ID="UpdateProgress3" AssociatedUpdatePanelID="">
                        <ProgressTemplate>
                            <div class="loading">
                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <div class="col-12 col-s-12">
                        <asp:Label ID="Label_Journal_No" class="label_SO" runat="server"></asp:Label>
                        <asp:Label ID="hidden_Label_Journal_No" class="gettext" runat="server" Visible="false"></asp:Label>
                    </div>

                    <!--start of view of general table-->
                    <div id="JournalLine_section_general" runat="server">
                        &nbsp
                            <div class="image_properties">
                                <asp:ImageButton ID="ImageButton_AddPaymentLine" class="menuimage_nohighlight" runat="server" ImageUrl="~/RESOURCES/ADDJOURNALLINE.png" />
                                <asp:ImageButton ID="ImageButton_AddPaymentLine_h" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/HOVERFRAME.png" OnClick="Button_AddPaymentLine_Click" />
                            </div>

                        <div class="col-12 col-s-12">
                            <!--view of general table-->
                            <div class="col-12 col-s-12">
                                <asp:Button ID="Button_Journal_Line" runat="server" Text="Payment Journal Lines" class="accordion_style" />
                            </div>

                            <div class="col-12 col-s-12">
                                <asp:Button ID="Button_Delete_PaymentLine" runat="server" OnClick="Button_Delete_PaymentLine_Click" Text="Delete Line" class="frame_style_type2" Visible="false" />
                                <br />
                            </div>
                            <div class="col-12 col-s-12">

                                <asp:GridView ID="GridView_Line_View" runat="server"
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
                                        <asp:BoundField DataField="Trans. Date" HeaderText="Trans. Date" />
                                        <asp:BoundField DataField="Account" HeaderText="Account" />
                                        <asp:BoundField DataField="Txt." HeaderText="Txt." />
                                        <asp:BoundField DataField="Credit" HeaderText="Credit" />
                                        <asp:BoundField DataField="Hidden_RecId" HeaderText="Hidden_RecId" />
                                        <asp:BoundField DataField="Hidden_allow_alter" HeaderText="Hidden_allow_alter" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!--end of view of general table-->
                    <!--start of view of AddLine section-->
                    <div id="AddLine_section" visible="false" runat="server">

                        <div class="col-12 col-s-12">
                            <asp:Button ID="Button_SaveJournalLine" runat="server" OnClick="Button_SaveJournalLine_Click" Text="SAVE" class="frame_style_type2" />
                            &nbsp
                                <asp:Button ID="Button_CancelJournalLine" runat="server" CausesValidation="false" OnClick="Button_CancelJournalLine_Click" Text="CANCEL" class="frame_style_type2" />
                        </div>
                        <div class="col-12 col-s-12">
                            <asp:Button ID="Button_JournalLine" runat="server" Text="Payment Line" class="accordion_style" />
                        </div>

                        <!--==============================================================================-->
                        <div id="AddLine_section_details1" runat="server" visible="false">
                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Customer Acc.:      </label>
                                </div>

                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_CustomerAcc" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Button ID="Button_TextBox_CustomerAcc" runat="server" OnClick="CheckAcc" Text="Validate" class="glow_green" />
                                    &nbsp
                                    <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find in list" class="glow" />

                                </div>
                            </div>
                        </div>

                        <div id="AddLine_section_details2" runat="server" visible="false">
                            <asp:Label ID="hidden_TextBox_CustomerAcc" class="gettext" runat="server" Visible="false"></asp:Label>
                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Customer Name:</label>
                                </div>
                                <div class="col-6 col-s-8">
                                    <asp:Label ID="Label_CustomerName" class="gettext" runat="server" Text=" "></asp:Label>
                                </div>
                            </div>

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Amount:  </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Amount" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <!--============================================================================== -->

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Bank Code:  </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:DropDownList ID="DropDownList_BankCode" runat="server" class="inputtext" OnTextChanged="DropDownList_BankCode_Changed" AutoPostBack="true" />
                                </div>
                            </div>

                            <!--============================================================================== -->

                            <div class="col-6 col-s-12">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Bank Branch:  </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:DropDownList ID="DropDownList_BankBranch" runat="server" class="inputtext" />
                                    <asp:TextBox ID="TextBox_Other" class="inputtext" autocomplete="off" runat="server" Style="background-color: lightcyan;" Visible="false"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:CheckBox ID="CheckBox_OtherBankBranch" runat="server" Text="Other Bank Branch" OnCheckedChanged="CheckBox_Changed_OtherBankBranch" AutoPostBack="true" class="labeltext" /><br />
                                </div>
                            </div>
                            <!--============================================================================== -->

                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Payment Reference:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Payment_Reference" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <!--============================================================================== -->
                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Txt:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Txt" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <!--============================================================================== -->
                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Settle Invoice:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Settle_Invoice" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <!--============================================================================== -->
                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Settle Amount:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Settle_Amount" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <!--============================================================================== -->
                            <div class="col-6 col-s-12">
                                <asp:Button ID="Button_SelectInvoice" runat="server" OnClick="Button_SelectInvoice_Click" Text="Select Invoice" class="glow" />
                            </div>

                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Account Type:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:DropDownList ID="DropDownList_Account_Type" runat="server" class="inputtext" /><br />
                                </div>
                            </div>
                            <!--============================================================================== -->

                            <div class="col-6 col-s-12">

                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Cheque Number:</label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_Cheque_Number" class="inputtext" autocomplete="off" runat="server" Style="width: 40%" OnTextChanged="TextBox_Cheque_Number_Changed" AutoPostBack="true"></asp:TextBox>
                                    &nbsp
                                    <asp:DropDownList ID="DropDownList_Cheque_Number" runat="server" class="inputtext" OnTextChanged="DropDownList_Cheque_Number_Changed" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="col-12 col-s-12">
                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">

                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Cheque Date:</label>
                                    </div>

                                    <div class="col-3 col-s-5">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtChequeDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                            <label class="input-group-btn" for="txtChequeDate">
                                                <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                            </label>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hdChequeDt" runat="server" />
                                </div>
                                <!--============================================================================== -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Received Date:</label>
                                    </div>
                                    <div class="col-3 col-s-5">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtReceivedDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                            <label class="input-group-btn" for="txtReceivedDate">
                                                <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                            </label>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hdReceivedDt" runat="server" />
                                </div>
                                <!--============================================================================== -->
                            </div>
                        </div>
                        <!--============================================================================== -->
                        <asp:Label ID="hidden_TransactionDate" class="gettext" runat="server" Visible="false"></asp:Label>
                        <!--============================================================================== -->

                    </div>
                    <!--end of view of AddLine section-->
                    <!--start of SelectInvoice section-->
                    <div id="SelectInvoice_section" visible="false" runat="server">
                        <div class="col-12 col-s-12">
                            <asp:Button ID="Button_Save_SelectInvoice" runat="server" OnClick="Button_Transfer_SelectInvoice_Click" Text="TRANSFER" class="frame_style_type2" />
                            &nbsp
                                    <asp:Button ID="Button_Cancel_SelectInvoice" runat="server" CausesValidation="false" OnClick="Button_Cancel_SelectInvoice_Click" Text="CANCEL" class="frame_style_type2" />
                        </div>

                        <div class="col-12 col-s-12">
                            <asp:Button ID="Button_Invoice" runat="server" Text="Invoices" class="accordion_style" />
                        </div>
                        <!--============================================================================== -->

                        <div class="col-12 col-s-12">
                            <asp:GridView ID="GridView_SelectInvoice" runat="server"
                                HorizontalAlign="Left"
                                CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                HtmlEncode="False"
                                Style="overflow: hidden;"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Full Payment">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox_FullPayment" runat="server" Style="background-color: #45A29E; float: left;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partial Payment (RM)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox_Partial_PaymentQTY" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                            <br />
                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_Partial_PaymentQTY"
                                                ErrorMessage="Invalid Qty!" MaximumValue="9999.99"
                                                MinimumValue="0" Type="Double" Style="color: red; background: transparent; font-weight: bold;" SetFocusOnError="True">
                                            </asp:RangeValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="In Use">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox_InUse" runat="server" Style="background-color: #45A29E; float: left;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                                    <asp:BoundField DataField="Invoice Amount" HeaderText="Invoice Amount" />
                                    <asp:BoundField DataField="Balance Settlement" HeaderText="Balance Settlement" />
                                    <asp:BoundField DataField="In Payment" HeaderText="In Payment" />
                                    <asp:BoundField DataField="Date" HeaderText="Date" />
                                    <asp:BoundField DataField="Due Date" HeaderText="Due Date" />
                                    <asp:BoundField DataField="CustToRecId" HeaderText="CustToRecId" />
                                    <asp:BoundField DataField="Hidden_Disable_CheckBox_FullPayment" HeaderText="Hidden_Disable_CheckBox_FullPayment" />
                                    <asp:BoundField DataField="Hidden_Disable_TextBox_PartialPayment" HeaderText="Hidden_Disable_TextBox_PartialPayment" />
                                    <asp:BoundField DataField="Hidden_Disable_CheckBox_InUse" HeaderText="Hidden_Disable_CheckBox_InUse" />
                                    <asp:BoundField DataField="Hidden_Tick_CheckBox_InUse" HeaderText="Hidden_Tick_CheckBox_InUse" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <!--end of SelectInvoice section-->

                    <%--                        </ContentTemplate>--%>
                    <%--                    </asp:UpdatePanel>--%>
                </div>
                <!--end of JournalLine_section////////////////////////////////////////////////////////////////////////////////////-->

                <!--Overview_section////////////////////////////////////////////////////////////////////////////////////-->
                <div id="Overview_section" style="display: none" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="col-12 col-s-12">
                                <asp:Button ID="Button_ListOutStanding" runat="server" OnClick="Button_ListOutStanding_Click" Text="List Outstanding" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                                &nbsp                                
                                <asp:Button ID="Button_ListAll" runat="server" OnClick="Button_ListAll_Click" Text="List All" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                                <asp:Button ID="Button_ListAutoSettlement" runat="server" OnClick="Button_ListAutoSettlement_Click" Text="List Auto Settlement" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                            </div>
                            <div id="Overview_section_general" visible="false" runat="server">
                                <!--start of view of general table-->
                                <div class="col-12 col-s-12">
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Overview_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" />
                                    </div>
                                    <asp:UpdateProgress runat="server" ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel3">
                                        <ProgressTemplate>
                                            <div class="loading">
                                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <!--start of GridViewOverviewList////////////////////////////////////////////////////////////////////////////////////-->
                                    <div runat="server" class="col-10 col-s-12" id="divSearch">
                                        <asp:Label ID="label18" runat="server" Text="Search:" CssClass="labeltext"></asp:Label>
                                        <asp:TextBox ID="TextBox_Search_Overview" runat="server" OnTextChanged="TextBox_Search_Overview_TextChanged" CssClass="inputext" AutoPostBack="true" placeholder="Journal No." />
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div id="Div6" runat="server" style="overflow-y: auto;">
                                            <asp:GridView ID="GridViewOverviewList" runat="server"
                                                PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                                HtmlEncode="False" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:TemplateField HeaderText="Journal Number">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button_Journal_Number" runat="server" OnClick="Button_Journal_Number_Click" CausesValidation="false" Text='<%# Eval("Journal Number") %>' class="button_grid" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Journal Number" HeaderText="Journal Number" />
                                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                                    <asp:BoundField DataField="Posted" HeaderText="Posted" />
                                                    <asp:BoundField DataField="Post Dated Chq." HeaderText="Post Dated Chq." />
                                                    <asp:BoundField DataField="CC Received" HeaderText="CC Received" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                    <asp:BoundField DataField="Customer" HeaderText="Customer" HtmlEncode="False" />
                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerSettings PageButtonCount="2" />
                                                <RowStyle CssClass="rows" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!--end of GridViewOverviewList////////////////////////////////////////////////////////////////////////////////////-->
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--==============================================================================-->
                </div>
                <!--end of overview_section////////////////////////////////////////////////////////////////////////////////////-->

                <!--Enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress4">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="col-12 col-s-12">
                    <asp:Button ID="btnCheckInvoice" runat="server" OnClick="btnCheckInvoice_Click" Visible="false"
                        Text="Check Invoice" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                    &nbsp                               
                                <asp:Button ID="btnCheckStatement" runat="server" OnClick="btnCheckStatement_Click" Visible="false"
                                    Text="Check Statement" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                    &nbsp                               
                                <asp:Button ID="btnCheckInvDue" runat="server" OnClick="btnCheckInvDue_Click" Visible="false"
                                    Text="Check Invoice Due" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                </div>

                <div id="Enquiries_section" style="display: none" runat="server">
                    <asp:UpdatePanel runat="server" ID="upInvoice">
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

                                    <div class="col-6 col-s-12" id="divFindList" runat="server">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="labeltext" class="labeltext" runat="server" Text="Customer Account:      "></asp:Label>
                                        </div>

                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="TextBox_SearchEnquiries" class="inputtext" runat="server" OnTextChanged="TextBox_SearchEnquiries_Changed" AutoPostBack="true"></asp:TextBox>
                                        </div>

                                        <div class="col-6 col-s-8">
                                            <asp:RadioButtonList ID="rblInvoice" runat="server">
                                                <asp:ListItem Text="Due Invoice" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Invoice To Be Due" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>

                                   
                                </div>

                                <div class="col-6 col-s-12" runat="server" id="divDays">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Days:      </label>
                                    </div>
                                    <div class="col-3 col-s-4">
                                        <asp:DropDownList ID="DropDownList_DayInvoice" runat="server" AutoPostBack="true" class="inputtext" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_DayInvoice">
                                            <asp:ListItem Text="-- SELECT --" Value=""></asp:ListItem>
                                            <asp:ListItem Text="< 35 Days" Value="35"></asp:ListItem>
                                            <asp:ListItem Text="< 65 Days" Value="65"></asp:ListItem>
                                            <asp:ListItem Text="1 Year" Value="365"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-6 col-s-12">  
                                        <div class="col-3 col-s-4">
                                                <label class="labeltext">Invoice:      </label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtInvoiceSearch" class="inputtext" runat="server"></asp:TextBox>

                                            </div>
                                    </div>                           
                                    <div class="col-6 col-s-12">
                                        <asp:Button ID="Button_FindList" OnClick="CheckAccInList2" runat="server" CausesValidation="false" Text="Search Customer" class="glow" Style="margin-bottom: 4px" />
                                        <asp:Button ID="btnGetInvoice" Text="Get Invoice" runat="server" OnClick="CheckEnquiries" class="getStatement" Style="margin-bottom: 4px" />
                                    </div>
                            </div>

                            <div id="Enquiries_section_general" visible="false" runat="server">
                                <!--start of view of general table-->

                                <!--start of GridEnquiriesList////////////////////////////////////////////////////////////////////////////////////-->

                                <div class="col-12 col-s-12">
                                    <div id="grdCharges2" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <asp:GridView ID="GridEnquiriesList" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Style="overflow: hidden" EmptyDataText="No records found."
                                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_enquiries" AllowCustomPaging="True"
                                            HtmlEncode="False" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:BoundField DataField="Salesman ID" HeaderText="Salesman ID" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="Account Name" HeaderText="Account Name" />
                                                <asp:BoundField DataField="Account No." HeaderText="Account No." />
                                                <asp:TemplateField HeaderText="Invoice">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_Invoice" runat="server" OnClick="Button_Invoice_Click" CausesValidation="false" Text='<%# Eval("Invoice") %>' class="button_grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Invoice" HeaderText="Invoice" />
                                                <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" />
                                                <asp:BoundField DataField="Due Date" HeaderText="Due Date" />
                                                <asp:BoundField DataField="Invoice Amount" HeaderText="Invoice Amount (RM)" />
                                                <asp:BoundField DataField="Outstanding Amount" HeaderText="Outstanding Amount (RM)" />
                                                <asp:BoundField DataField="Current Payment" HeaderText="Current Payment (RM)" />
                                                <asp:BoundField DataField="Balance Outstanding" HeaderText="Balance Outstanding (RM)" />
                                                <asp:BoundField DataField="Cheque Date" HeaderText="Cheque Date" />
                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <!--end of GridViewOverviewList////////////////////////////////////////////////////////////////////////////////////-->
                            </div>

                            <div id="DivInvoiceDue" visible="false" class="col-12 col-s-12" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                <asp:GridView ID="gvInvoiceDue" runat="server"
                                    HorizontalAlign="Left" CssClass="mydatagrid"
                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows" EmptyDataText="No records found."
                                    OnPageIndexChanging="gvInvoiceDue_PageIndexChanging" HtmlEncode="False" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="No." HeaderText="No." />
                                        <asp:BoundField DataField="Salesman ID" HeaderText="Salesman ID" ItemStyle-Width="10%" />
                                        <asp:BoundField DataField="Account Name" HeaderText="Account Name" />
                                        <asp:BoundField DataField="Account No." HeaderText="Account No." />
                                        <asp:TemplateField HeaderText="Invoice">
                                            <ItemTemplate>
                                                <asp:Button ID="Button_Invoice" runat="server" OnClick="Button_Invoice_Click" CausesValidation="false" Text='<%# Eval("Invoice") %>' class="button_grid" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" />
                                        <asp:BoundField DataField="Due Date" HeaderText="Due Date" />
                                        <asp:BoundField DataField="Invoice Amount" HeaderText="Invoice Amount (RM)" />
                                        <asp:BoundField DataField="Outstanding Amount" HeaderText="Outstanding Amount (RM)" />
                                        <asp:BoundField DataField="Current Payment" HeaderText="Current Payment (RM)" />
                                        <asp:BoundField DataField="Balance Outstanding" HeaderText="Balance Outstanding (RM)" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--==============================================================================-->
                    <asp:UpdatePanel ID="upStatement" runat="server">
                        <ContentTemplate>
                            <div class="col-12 col-s-12">
                                <asp:Button ID="Button2" runat="server" Text="Customer Statement" class="accordion_style_sub_fixed_darkcyan" />
                            </div>

                            <!--start of view of general table-->
                            <div class="col-12 col-s-12">
                                <div class="col-6 col-s-12">
                                    <div class="col-2 col-s-4">
                                        <label class="labeltext">Search By:      </label>
                                    </div>

                                    <div class="col-3 col-s-8">
                                        <asp:DropDownList ID="ddlCustomerAcc" runat="server" AutoPostBack="true" class="dropdownlist">
                                            <asp:ListItem Text="Account No." Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Customer Name" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-4 col-s-4">
                                        <label class="labeltext">Account Number / Name:      </label>
                                    </div>

                                    <div class="col-3 col-s-4">
                                        <asp:TextBox ID="txtCustAcc" class="inputtext" runat="server" OnTextChanged="txtCustAcc_Changed" AutoPostBack="true"></asp:TextBox>
                                    </div>

                                    <div class="col-3 col-s-6">
                                        <asp:Button ID="btnSearchCust" OnClick="CheckAccInList3" runat="server" CausesValidation="false" Text="Search Customer" class="glow" Style="margin-bottom: 4px" />
                                        <asp:Button ID="btnSearch" Text="Get Statement" runat="server" OnClick="Button_Search_Click" class="getStatement" Style="margin-bottom: 4px" />
                                    </div>
                                </div>
                            </div>

                            <!--start of GridEnquiriesList////////////////////////////////////////////////////////////////////////////////////-->

                            <div class="col-12 col-s-12">
                                <div id="customer_Section" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                    <asp:GridView ID="gvCustomerList" runat="server" Visible="false"
                                        PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        AllowPaging="True" OnPageIndexChanging="gvCustomerList_PageIndexChanging" AllowCustomPaging="True"
                                        HtmlEncode="False" DataKeyNames="No."
                                        Style="overflow: hidden;"
                                        AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="No." HeaderText="No." />
                                            <asp:TemplateField HeaderText="Customer Account">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnCusAccount" runat="server" OnClick="btnCusAccount_Click" CausesValidation="false" Text='<%# Eval("Customer Account") %>' class="button_grid" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                            <asp:BoundField DataField="Address" HeaderText="Address" />
                                            <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                            <asp:BoundField DataField="Salesman" HeaderText="Salesman" />
                                            <asp:BoundField DataField="Remark" HeaderText="Remark" />

                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings PageButtonCount="2" />
                                        <RowStyle CssClass="rows" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="col-12 col-s-12" runat="server" id="Enquiries_section_Statement">
                                <div id="Div2" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                                    <asp:GridView ID="gvStatement" runat="server" DataKeyNames="No."
                                        PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        AllowPaging="True" OnPageIndexChanging="gvStatement_PageIndexChanging" AllowCustomPaging="True"
                                        HtmlEncode="False" AutoGenerateColumns="False">
                                        <Columns>
                                            <%--                                            <asp:BoundField DataField="No." HeaderText="No." />--%>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Statement">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlStatement" runat="server" NavigateUrl='<%# Eval("Statement") %>' Target="_blank" 
                                                        CssClass="statement-link"> <%# Eval("Statement") %> </asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Description" HeaderText="Description" />
                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings PageButtonCount="2" />
                                        <PagerStyle CssClass="pager" />
                                        <RowStyle CssClass="rows" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="ErrMsg" runat="server" CssClass="labeltext" Visible="false"></asp:Label>
                            </div>
                            <!--end of GridViewOverviewList////////////////////////////////////////////////////////////////////////////////////-->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!--end of Enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
            </div>
        </div>
        <script src="scripts/ButtonUp.js"></script>
    </form>
</body>
</html>
