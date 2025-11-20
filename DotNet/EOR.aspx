<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EOR.aspx.cs" Inherits="DotNet.EOR" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="STYLES/select2.min.css" media="screen" />
    <script src="scripts/jquery.min.js"></script>
    <script src="scripts/select2.min.js"></script>

    <title>EOR</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/GoToTab.js"></script>
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>
</head>
<%--<script type="text/javascript">
    $('.js-example-basic-multiple').select2();

</script>--%>

<body>
    <form id="form1" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/EORR.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>EOR</h1>
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
                <%--<a href="EOR.aspx" id="EORTag2" class="active" runat="server" visible="false">EOR</a>--%>
                <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
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
                <div class="col-12 col-s-12" aria-multiline="True">
                    <asp:Button ID="Button_new_applicant_section" runat="server" OnClick="Button_new_applicant_section_Click" Text="New" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_draft_section" runat="server" OnClick="Button_draft_section_Click" Text="Performance" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_overview_section" runat="server" OnClick="Button_overview_section_Click" Text="Overview" class="frame_style_4bar" CausesValidation="false" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_enquiries_section" runat="server" OnClick="Button_enquiries_section_Click" Text="Enquiries" class="frame_style_4bar" />
                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <!--new_applicant_section////////////////////////////////////////////////////////////////////////////////////-->
                <div id="new_applicant_section" style="display: none" runat="server">
                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_Submit" runat="server" OnClick="Button_Submit_Click" Text="Submit" class="frame_style_type2" ValidationGroup="submit" />
                        <asp:Button ID="Button_EORAdmin" runat="server" OnClick="Button_EORAdmin_Click" Text="Admin" class="frame_style_type2" />
                        <asp:Button ID="Button_SaveDraft" runat="server" OnClick="Button_SaveDraft_Click" Text="Save As Draft" class="frame_style_type2" Visible="false" />
                        <asp:Button ID="Button_Reject" runat="server" OnClick="Button_Reject_Click" Text="Reject" class="frame_style_type2" Visible="false" />
                        <asp:Button ID="Button_RejectToDraft" runat="server" OnClick="Button_RejectToDraft_Click" Text="Reject to Draft" class="frame_style_type2" Visible="false" />
                    </div>
                    <asp:Label ID="hidden_Label_SaveDraft" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_Label_PreviousDraftStatus" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_Label_EquipmentRecId" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_Label_EquipmentRecId_Update" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>

                    <asp:Label ID="hidden_NA_HOD" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_NA_Admin" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_NA_Manager" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_NA_GM" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_NextApproval" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_NextApprovalAlt" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                    <asp:Label ID="hidden_DocStatus" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <!-- EOR Part Admin////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_EORPartAdmin" Visible="false" runat="server" Text="Admin Control" class="accordion_style" /><br />
                            <br />

                            <div id="new_applicant_section_EORPartAdmin" style="display: none" runat="server">
                                <div class="col-12 col-s-12">
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Recalculate_Admin" runat="server" OnClick="Recalculate_Admin" Text="Refresh" class="glow" Visible="false" />
                                        &nbsp                                     
                                    </div>

                                    <br />
                                    <br />

                                    <asp:Button ID="Button3" runat="server" Text="Other Information" class="accordion_style_sub_fixed_lightGrey" /><br />
                                    <div class="col-12 col-s-12">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Duration of contracts (Months):        </label>
                                            </div>
                                            <div class="col-3 col-s-8">
                                                <asp:Label ID="Label_DurationContract_Admin" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <asp:Button ID="Button2" runat="server" Text="Equipment + Handling Cost" class="accordion_style_sub_fixed_lightGrey" /><br />
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_CalculateEquipmentAdmin" runat="server" OnClick="Button_CalculateEquipmentAdmin_click" Text="ReCalculate Equipment + Handling " class="glow_green" />&nbsp     
                                         <asp:Button ID="Button_Save_Admin" runat="server" OnClick="Save_Admin" Text="Save - ADMIN" class="glow" /><br>
                                        <asp:Label ID="Label1" class="gettext" runat="server" Text="# Note: System will not record if Description or Quantity is not filled in."></asp:Label>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div id="Div1" runat="server" style="overflow: auto;">
                                            <asp:GridView ID="GridView_EquipmentAdmin" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">

                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_Description_R" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Quantity (QTY)" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_New_QTY_R" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_New_QTY_R"
                                                                ErrorMessage="Invalid Qty!" MaximumValue="9999" SetFocusOnError="True" Display="Dynamic"
                                                                MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                            </asp:RangeValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Normal Item" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DropDownList_NormalItem" runat="server" class="inputtext">
                                                                <asp:ListItem Text="Normal" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Sensitive" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Unspecified" Value="99"></asp:ListItem>
                                                            </asp:DropDownList>

                                                            <br />
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Equipment Cost (RM)" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="top">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_Equipment_Cost_R" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                            <br />
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Equipment x QTY x Handling Cost (RM)" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="top">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="TextBox_Equipment_Handling_R" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                            <br />
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Carton" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="top">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_CartonEquipment_R" class="inputtext" runat="server" Style="background: transparent; color: black;"></asp:Label>
                                                            <br />
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Total Monthly Target:</label>

                                            </div>
                                            <div class="col-3 col-s-8">
                                                <asp:Label ID="Label_MonthlyTarget" class="gettext" Text=" " runat="server"></asp:Label>
                                                <label class="gettext">CTNS</label>
                                            </div>
                                        </div>

                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Remarks Admin:        </label>
                                            </div>
                                            <div class="col-3 col-s-8">
                                                <asp:TextBox ID="TextBox_RemarksAdmin" class="inputtext" autocomplete="off" runat="server" TextMode="MultiLine" Height="90px" Width="400px" Row="3" Columns="20" Style="resize: none;"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <asp:Label ID="hidden_EORCarton" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_EORPoint" class="gettext" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <!--Customer Info ////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_CustInfo" runat="server" Text="Customer Info (maximized)" class="accordion_style_sub_fixed_darkcyan" OnClick="HideAccordion_CustInfo" />
                            <div id="new_applicant_section_CustomerInfo" style="display: none" runat="server">
                                <!--==============================================================================-->
                                <div class="col-6 col-s-12" runat="server">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Account:      </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:TextBox ID="TextBox_Account" class="inputtext" autocomplete="off" runat="server" OnTextChanged="CheckAcc"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="TextBox_Account" CssClass="text-danger"
                                            Display="Dynamic" ErrorMessage="Account is required!" ForeColor="Red" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-2 col-s-4">
                                        <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find in list" class="glow" />&nbsp
                                    </div>
                                    <div class="col-3 col-s-4">
                                        <asp:Button ID="Button_CheckAcc" runat="server" OnClick="CheckAcc" CausesValidation="false" Text="Validate" class="glow_green" />
                                    </div>
                                </div>
                                <!--============================================================================== -->
                                <div id="div_CustInfoExtra" visible="false" runat="server">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Cust. Name:        </label>
                                        </div>
                                        <div class="col-5 col-s-8">
                                            <asp:Label ID="Label_CustName" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>
                                    <!--============================================================================== -->

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Tel No:  </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:Label ID="Label_TelNo" class="gettext" autocomplete="off" runat="server" Minlength="10" MaxLength="12"></asp:Label>
                                        </div>
                                    </div>
                                    <!--============================================================================== -->

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Contact Person:  </label>
                                        </div>
                                        <div class="col-5 col-s-8">
                                            <asp:Label ID="Label_ContactPerson" class="gettext" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <!--============================================================================== -->

                                    <div class="col-6 col-s-12">

                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">HQ / BR:        </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:Label ID="Label_HQ" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>
                                    <!--============================================================================== -->
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Class:        </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:Label ID="Label_Class" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>
                                    <!--============================================================================== -->
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Salesman:        </label>
                                        </div>
                                        <div class="col-5 col-s-8">
                                            <asp:Label ID="Label_Salesman" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <!--============================================================================== -->
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Acc Opened Date:        </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:Label ID="Label_AccOpenedDate" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <!--============================================================================== -->

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Address:</label>
                                        </div>
                                        <div class="col-5 col-s-8">
                                            <asp:Label ID="Label_Address" class="gettext" TextMode="MultiLine" max-height="120px" Rows="3" Columns="20" Style="resize: none;" runat="server" Text=" "></asp:Label>
                                            <br />
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
                                    <asp:Label ID="hidden_City" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_State" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_Country" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <!--============================================================================== -->
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Duration of Contract:</label>
                                        </div>
                                        <div class="col-8 col-s-8">
                                            <asp:RadioButtonList ID="rblContractDuration" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="RadioButtonChanged_duration" AutoPostBack="true">
                                                <asp:ListItem Text="1 year" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2 years" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3 years" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="4 years" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="5 years" Value="5"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="rfvContractionDuration" runat="server" ErrorMessage="Contraction Duration required!"
                                                ForeColor="Red" Display="Dynamic" ControlToValidate="rblContractDuration" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                            <%--                                            <asp:RadioButton ID="RadioButton1" runat="server" Text="1 year" GroupName="Duration" OnCheckedChanged="RadioButtonChanged_duration" AutoPostBack="true" class="gettext" />
                                            <br />
                                            <asp:RadioButton ID="RadioButton2" runat="server" Text="2 years" GroupName="Duration" OnCheckedChanged="RadioButtonChanged_duration" AutoPostBack="true" class="gettext" />
                                            <br />
                                            <asp:RadioButton ID="RadioButton10" runat="server" Text="3 years" GroupName="Duration" OnCheckedChanged="RadioButtonChanged_duration" AutoPostBack="true" class="gettext" />
                                            <br />
                                            <asp:RadioButton ID="RadioButton11" runat="server" Text="4 years" GroupName="Duration" OnCheckedChanged="RadioButtonChanged_duration" AutoPostBack="true" class="gettext" />
                                            <br />
                                            <asp:RadioButton ID="RadioButton12" runat="server" Text="5 years" GroupName="Duration" OnCheckedChanged="RadioButtonChanged_duration" AutoPostBack="true" class="gettext" />--%>
                                        </div>

                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Request Type:</label>
                                        </div>
                                        <div class="col-8 col-s-8">
                                            <asp:RadioButtonList ID="rblRequestType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="RadioButtonChanged_CustType">
                                                <asp:ListItem Text="New" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Existing" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Branch" Value="3"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ID="rfvRequestType" runat="server" ErrorMessage="Request type is required!"
                                                ForeColor="Red" Display="Dynamic" ControlToValidate="rblRequestType" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                            <%--                                            <asp:RadioButton ID="RadioButton13" runat="server" Text="New" GroupName="Type" OnCheckedChanged="RadioButtonChanged_CustType" AutoPostBack="true" class="gettext" />
                                            <br />
                                            <asp:RadioButton ID="RadioButton14" runat="server" Text="Existing" GroupName="Type" OnCheckedChanged="RadioButtonChanged_CustType" AutoPostBack="true" class="gettext" />
                                            <br />
                                            <asp:RadioButton ID="RadioButton15" runat="server" Text="Branch" GroupName="Type" OnCheckedChanged="RadioButtonChanged_CustType" AutoPostBack="true" class="gettext" />
                                            <br />--%>
                                        </div>
                                    </div>

                                    <asp:Label ID="hidden_customer_class" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_duration_year_month" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_duration_rate" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_customer_type" class="gettext" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <br />

                            <!-- EOR Part 1////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_EORPart1" Visible="false" runat="server" Text="" class="accordion_style" OnClick="Hide_Accordion_EORPart1" /><br />
                            <br />
                            <div id="new_applicant_section_EORPart1" style="display: none" runat="server">

                                <!-- Equipment ////////////////////////////////////////////////////////////////////////////////////-->

                                <asp:Button ID="Accordion_Equipments" runat="server" Text="1. Equipments" class="accordion_style_sub_fixed_lightGrey" /><br />
                                <asp:Button ID="ButtonAdd_Equipment" runat="server" OnClick="ButtonAdd_Click" Text="Add new row" class="glow_green" Style="margin: 4px 0px" /><br />
                                <div class="col-12 col-s-12">
                                    <%--                                    <div id="grdCharge_GridView_Equipment" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">--%>
                                    <div id="Div4" runat="server" style="max-height: 150px; overflow-y: auto;">
                                        <asp:GridView ID="GridView_Equipment" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            Style="overflow: hidden"
                                            AutoGenerateColumns="False"
                                            OnRowDeleting="GridView_Equipment_RowDeleting">

                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />

                                                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="50%" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UplEquipment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="TextBox_Description" class="inputtext" autocomplete="off" runat="server"
                                                                    Style="min-width: 80%; max-width: 80%" OnTextChanged="TextBox_DescriptionEquipment_Changed"></asp:TextBox>
                                                                <%--                                                            <select class="js-example-basic-multiple" multiple="multiple"><option value="AL">Alabama</option></select>--%>

                                                                <asp:DropDownList ID="DropDownList_SearchEquipment" runat="server" class="inputtext" Style="min-width: 50%; max-width: 100%"
                                                                    OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_SearchEquipment" Visible="false" />
                                                                <%--                                                            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description is required!" Display="Dynamic"
                                                                ForeColor="Red" ControlToValidate="DropDownList_SearchEquipment" ValidationGroup="submit"></asp:RequiredFieldValidator>--%>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="LookUp">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_SearchEquipment" runat="server" OnClick="Button_SearchEquipment_Click" CausesValidation="false" Text="Search" class="button_grid" />
                                                        <br />
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantity/ Unit" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_New_QTY" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ErrorMessage="Quantity is required!"
                                                            ForeColor="Red" ControlToValidate="TextBox_New_QTY" Display="Dynamic" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                                        <asp:RangeValidator runat="server" ControlToValidate="TextBox_New_QTY" Display="Dynamic"
                                                            ErrorMessage="Invalid Qty!" MaximumValue="9999" SetFocusOnError="True"
                                                            MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                        </asp:RangeValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Deposit" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_DepositEquipment" class="inputtext" autocomplete="off" runat="server"
                                                            OnTextChanged="TextBox_DepositEquipment_Changed" AutoPostBack="true"></asp:TextBox>
                                                        <%--                                                        <br />
                                                        <asp:RequiredFieldValidator ID="rfvDepositEquipment" runat="server" ForeColor="Red" 
                                                            ControlToValidate="TextBox_DepositEquipment" Display="Dynamic" ErrorMessage="Deposit is required!" ValidationGroup="submit"></asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Carton">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_CartonEquipment" class="inputtext" runat="server" Style="background: transparent; color: black;"></asp:Label>
                                                        <br />
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RecId">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label_RecIdEquipment" class="inputtext" runat="server" Style="background: transparent; color: black;"></asp:Label>
                                                        <br />
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkdelete" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                                        <br />
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <%--                                    </div>--%>
                                </div>
                                <br></br>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Remarks Sales HOD:        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_RemarksSalesHOD" class="inputtext" autocomplete="off" runat="server" TextMode="MultiLine" Row="3" Columns="20" Style="resize: none; min-width: 100%"></asp:TextBox>
                                    </div>
                                </div>
                                <!--  Pass 6 months////////////////////////////////////////////////////////////////////////////////////-->

                                <asp:Button ID="Accordion_SixMonth" runat="server" Text="2. Pass Purchase Record" class="accordion_style_sub_fixed_lightGrey" /><br />
                                <div class="col-12 col-s-12">
                                    <div id="Div2" runat="server" style="overflow-y: auto;">
                                        <asp:GridView ID="GridView_Past_MonthRecords" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            Style="overflow: hidden;"
                                            AutoGenerateColumns="False">

                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:BoundField DataField="Trans Date" HeaderText="Trans Date" />
                                                <asp:BoundField DataField="Debit Amount (RM)" HeaderText="Debit Amount (RM)" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="No. of Transaction" HeaderText="No. of Transaction" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <br></br>

                                <asp:Button ID="Accordion_ProposedProduct" runat="server" Text="3. Proposed Product and Qty monthly to fulfill EOR Target" class="accordion_style_sub_fixed_lightGrey" /><br />
                                <asp:Button ID="ButtonAdd_ProposedProduct" runat="server" OnClick="ButtonAdd_ProposedProduct_Click" Text="Add new row" class="glow_green" Style="margin: 4px 0px" /><br />
                                <div class="col-12 col-s-12">
                                    <div id="grdCharge_GridView_ProposedProduct" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                                        <asp:GridView ID="GridView_ProposedProduct" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            HtmlEncode="False"
                                            Style="overflow: hidden;"
                                            AutoGenerateColumns="False"
                                            OnRowDeleting="GridView_ProposedProduct_RowDeleting">

                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="50%" ItemStyle-Width="50%" ItemStyle-VerticalAlign="top">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_Description" class="inputtext" autocomplete="off" runat="server" Style="min-width: 100%; max-width: 100%"></asp:TextBox>
                                                        <asp:DropDownList ID="DropDownList_SearchProposedProduct" runat="server" Visible="false" class="inputtext"
                                                            Style="min-width: 80%; max-width: 80%" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_SearchProposedProduct" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="LookUp">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_SearchProposedProduct" runat="server" OnClick="Button_SearchProposedProduct_Click" CausesValidation="false" Text="Search" class="button_grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantity/ Unit" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="top">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox_New_QTY" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                                        <br />
                                                        <asp:RangeValidator runat="server" ControlToValidate="TextBox_New_QTY" Display="Dynamic"
                                                            ErrorMessage="Invalid Qty!" MaximumValue="9999" SetFocusOnError="True"
                                                            MinimumValue="0" Type="Integer" Style="color: red; background: transparent; font-weight: bold;">
                                                        </asp:RangeValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkdelete2" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <br></br>

                                <asp:Button ID="Accordion_Comment" runat="server" Text="4. Comments From Salesman" class="accordion_style_sub_fixed_lightGrey" /><br />
                                <div class="col-12 col-s-12">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">Type of Service Centre:</label>
                                        </div>
                                        <div class="col-3 col-s-6">
                                            <asp:TextBox ID="TextBox_ServiceCentre" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">Workshop Facilities:</label>
                                        </div>
                                        <div class="col-3 col-s-6">
                                            <asp:TextBox ID="TextBox_Facilities" class="inputtext" autocomplete="off" runat="server" TextMode="MultiLine" Height="60px" Row="3" Columns="20" Style="resize: none;"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">Owner Experience:</label>
                                        </div>
                                        <div class="col-3 col-s-6">
                                            <asp:DropDownList ID="DropDownList_Experience" runat="server" class="dropdownlist">
                                                <asp:ListItem Text="-- SELECT --" Value=""></asp:ListItem>
                                                <asp:ListItem Text=">1 year" Value="1"></asp:ListItem>
                                                <asp:ListItem Text=">3 year" Value="3"></asp:ListItem>
                                                <asp:ListItem Text=">5 year" Value="5"></asp:ListItem>
                                                <asp:ListItem Text=">10 year" Value="10"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">Workshop Size/Type:</label>
                                        </div>
                                        <div class="col-3 col-s-6">
                                            <asp:TextBox ID="TextBox_WorkshopSize" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">No. of Mechanics:</label>
                                        </div>
                                        <div class="col-s-6">
                                            <asp:TextBox ID="TextBox_Mechanics" class="inputtext" MaxLength="2" autocomplete="off" runat="server"></asp:TextBox>
                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_Mechanics"
                                                ErrorMessage="Invalid Number!" MaximumValue="999" SetFocusOnError="True"
                                                MinimumValue="0" Type="Integer" Style="color: red;">
                                            </asp:RangeValidator>
                                        </div>
                                        <%--                                        <br />
                                        <label class="gettext">person</label>--%>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">Workshop Status:</label>
                                        </div>
                                        <div class="col-3 col-s-6">
                                            <asp:TextBox ID="TextBox_WorkshopStatus" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%--                                    <asp:Button ID="btnUpload" runat="server" Text="upload" OnClick="upload_Click" />--%>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                        <ContentTemplate>
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label runat="server" Text="Signboard Picture" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgSignboard" runat="server" Width="100" Height="100" />
                                                    <%--                                            <input type="file" name="fuSignboard" oninput="imgSignboard.src=window.URL.createObjectURL(this.files[0])" />--%>
                                                    <asp:FileUpload ID="fuSignboard" runat="server" CssClass="inputstyle" oninput="imgSignboard.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revSignboard" runat="server" BorderColor="Red" ControlToValidate="fuSignboard" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvSignboard" runat="server" ValidationGroup="submit" Display="Dynamic"
                                                        ControlToValidate="fuSignboard" Color="Red" CssClass="text-danger" ErrorMessage="Signboard Picture required!"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label runat="server" Text="External Picture" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgExternal" runat="server" Width="100" Height="100" />
                                                    <asp:FileUpload ID="fuExternal" runat="server" CssClass="inputstyle" oninput="imgExternal.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revExternal" runat="server" BorderColor="Red" ControlToValidate="fuExternal" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvExternal" runat="server" ValidationGroup="submit"
                                                        ControlToValidate="fuExternal" Color="Red" CssClass="text-danger" ErrorMessage="External Picture required!"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label runat="server" Text="Internal Picture 1" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgInternal1" runat="server" Width="100" Height="100" />
                                                    <asp:FileUpload ID="fuInternal1" runat="server" CssClass="inputstyle" oninput="imgInternal1.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revInternal1" runat="server" BorderColor="Red" ControlToValidate="fuInternal1" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvInternal1" runat="server" ValidationGroup="submit"
                                                        ControlToValidate="fuInternal1" Color="Red" CssClass="text-danger" ErrorMessage="Internal Picture 1 required!"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label runat="server" Text="Internal Picture 2" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgInternal2" runat="server" Width="100" Height="100" />
                                                    <asp:FileUpload ID="fuInternal2" runat="server" CssClass="inputstyle" oninput="imgInternal2.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revInternal2" runat="server" BorderColor="Red" ControlToValidate="fuInternal2" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rvInternal2" runat="server" ValidationGroup="submit"
                                                        ControlToValidate="fuInternal2" Color="Red" CssClass="text-danger" ErrorMessage="Internal Picture 2 required!"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label runat="server" Text="Equipment Picture 1" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgEquipment1" runat="server" Width="100" Height="100" />
                                                    <asp:FileUpload ID="fuEquipment1" runat="server" CssClass="inputstyle" oninput="imgEquipment1.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revEquip1" runat="server" BorderColor="Red" ControlToValidate="fuEquipment1" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEquipment1" runat="server" ValidationGroup="submit"
                                                        ControlToValidate="fuEquipment1" Color="Red" CssClass="text-danger" ErrorMessage="Equipment Picture 1 required!"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label runat="server" Text="Equipment Picture 2" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgEquipment2" runat="server" Width="100" Height="100" />
                                                    <asp:FileUpload ID="fuEquipment2" runat="server" CssClass="inputstyle" oninput="imgEquipment2.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revEquip2" runat="server" BorderColor="Red" ControlToValidate="fuEquipment2" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEquipment2" runat="server" ValidationGroup="submit"
                                                        ControlToValidate="fuEquipment2" Color="Red" CssClass="text-danger" ErrorMessage="Equipment Picture 2 required!"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-6 col-2-12">
                                                <div class="col-3 col-s-6">
                                                    <asp:Label ID="labelSSM" runat="server" Text="Latest SSM Document" CssClass="labeltext"></asp:Label>
                                                </div>
                                                <div class="col-6 col-s-6">
                                                    <asp:Image ID="imgSSM" runat="server" Width="100" Height="100" />
                                                    <asp:FileUpload ID="fuSSM" runat="server" CssClass="inputstyle" oninput="imgSSM.src=window.URL.createObjectURL(this.files[0])" />
                                                    <asp:RegularExpressionValidator ID="revSSM" runat="server" ForeColor="Red" ControlToValidate="fuSSM" Display="Dynamic"
                                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$" ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvSSM" runat="server" ForeColor="Red" ErrorMessage="Latest SSM required!"
                                                        CssClass="text-danger" ControlToValidate="fuSSM" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Button_Submit" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-6">
                                            <label class="labeltext">Years of Establishment:</label>
                                        </div>
                                        <div class="col-5 col-s-6">
                                            <asp:TextBox ID="TextBox_Establishment" MaxLength="4" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                            <br />
                                            <asp:RangeValidator runat="server" ControlToValidate="TextBox_Establishment"
                                                ErrorMessage="Invalid Year!" MaximumValue="9999" SetFocusOnError="True"
                                                MinimumValue="1000" Type="Integer" Style="color: red;">
                                            </asp:RangeValidator>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Requested by:</label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:TextBox ID="TextBox_Requested" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br></br>
                            </div>

                            <!-- EOR Part 2////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_EORPart2" Visible="false" runat="server" Text="" class="accordion_style" OnClick="Hide_Accordion_EORPart2" /><br />
                            <br />
                            <div id="new_applicant_section_EORPart2" style="display: none" runat="server">

                                <asp:Button ID="Accordion_ApprovalManager" runat="server" Text="Approval by Manager/ Senior Manager/ Executive" class="accordion_style_sub_fixed_lightGrey" /><br />

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Date:        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_DateApproval" class="inputtext" autocomplete="off" runat="server" Minlength="6" MaxLength="10" placeholder="Eg: 01/01/2021"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Remarks Approver:        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_RemarksApproval" class="inputtext" autocomplete="off" runat="server" TextMode="MultiLine" Height="60px" Row="3" Columns="20" Style="resize: none;"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Approved by:</label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_Approved" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="UpdatePanel2">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <!--draft_section////////////////////////////////////////////////////////////////////////////////////-->
                <div id="draft_section" style="display: none" runat="server">

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="col-12 col-s-12">

                                <!--view of general table-->
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_Draft_accordion" runat="server" Text="Performance Listing" class="accordion_style_sub_fixed_darkcyan" />
                                </div>

                                <%--                        <asp:CheckBox ID="CheckBox_div_Searchable_ID" runat="server" OnCheckedChanged="CheckBox_div_Searchable" Text="Show Search Bar" AutoPostBack="true" style="background-color:darkred;color: ;" Visible="true"/><br />--%>

                                <div class="col-12 col-s-12" id="div_Searchable" runat="server" visible="true">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Search By:      </label>
                                        </div>

                                        <div class="col-3 col-s-8">
                                            <asp:DropDownList ID="DropDownList_Search" runat="server" class="dropdownlist">
                                                <asp:ListItem Text="Customer Account No." Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Customer Name" Value="1"></asp:ListItem>
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
                                    <div id="Div3" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <asp:GridView ID="GridViewPerformanceListing" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                            HtmlEncode="False"
                                            Style="overflow: hidden;"
                                            AutoGenerateColumns="False">

                                            <Columns>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:BoundField DataField="Account" HeaderText="Account" />
                                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                                <asp:BoundField DataField="Start Date" HeaderText="Start Date" />
                                                <asp:BoundField DataField="Expiry Date" HeaderText="Expiry Date" />
                                                <asp:BoundField DataField="Cur Act" HeaderText="Cur Act" />
                                                <asp:BoundField DataField="Cur Tgt" HeaderText="Cur Tgt" />
                                                <asp:BoundField DataField="CTGTCTN" HeaderText="CTGTCTN" />
                                                <asp:BoundField DataField="CCNCTN" HeaderText="CCNCTN" />
                                                <asp:BoundField DataField="CORCTN" HeaderText="CORCTN" />
                                                <asp:BoundField DataField="Performance" HeaderText="Performance" />
                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <PagerStyle CssClass="pager" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!--overview_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress3" class="gettext" AssociatedUpdatePanelID="UpdatePanel3">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div id="overview_section" style="display: none" runat="server">
                    <div class="col-12 col-s-12">
                        <asp:Button ID="Button_ListOutStanding" runat="server" OnClick="Button_ListAll_Click" Text="List All" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                        &nbsp                               
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div id="overview_section_general" visible="false" runat="server">
                                <!--start of view of general table-->
                                <div class="col-12 col-s-12">

                                    <!--view of general table-->
                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button_Overview_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" />
                                    </div>

                                    <%--                                <asp:CheckBox ID="CheckBox_div_Searchable_ID_Overview" runat="server" OnCheckedChanged="CheckBox_div_Searchable_Overview" Text="Show Search Bar" AutoPostBack="true" style="background-color:darkred;color: ;" Visible="false"/><br />--%>

                                    <div class="col-12 col-s-12" id="div_Searchable_Overview" runat="server" visible="true">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search By:      </label>
                                            </div>

                                            <div class="col-3 col-s-8">
                                                <asp:DropDownList ID="DropDownList_Search_Overview" runat="server" class="dropdownlist">
                                                    <asp:ListItem Text="Equipment Id" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Account No." Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search:      </label>
                                            </div>
                                            <div class="col-5 col-s-6">
                                                <asp:TextBox ID="TextBox_Search_Overview" class="inputtext" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton6" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_Overview_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div id="Div5" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                            <asp:GridView ID="GridViewOverviewList" runat="server" DataKeyNames="No."
                                                PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_Overview" AllowCustomPaging="True"
                                                HtmlEncode="False"
                                                Style="overflow: hidden;"
                                                AutoGenerateColumns="False">

                                                <Columns>
                                                    <asp:BoundField DataField="No." HeaderText="No." />
                                                    <asp:TemplateField HeaderText="Equipment Id">
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button_EquipmentId" runat="server" OnClick="Button_EquipmentId_Click" CausesValidation="false" Text='<%# Eval("Equipment Id") %>' class="button_grid" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Equipment Id" HeaderText="Equipment Id" />
                                                    <asp:BoundField DataField="Customer Account" HeaderText="Customer Account" />
                                                    <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                                    <asp:BoundField DataField="Customer Phone" HeaderText="Customer Phone" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                                    <asp:BoundField DataField="Next Appr" HeaderText="Next Appr" />
                                                    <asp:BoundField DataField="Next Appr Alt" HeaderText="Next Appr Alt" />
                                                    <asp:BoundField DataField="Salesman" HeaderText="Salesman" />
                                                    <asp:BoundField DataField="Applied By" HeaderText="Applied By" />
                                                    <asp:BoundField DataField="Process Status" HeaderText="Process Status" HtmlEncode="False" />
                                                </Columns>
                                                <HeaderStyle CssClass="header" />
                                                <PagerSettings PageButtonCount="2" />
                                                <PagerStyle CssClass="pager" />
                                                <RowStyle CssClass="rows" />

                                            </asp:GridView>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <!--end of view of general table-->
                        </ContentTemplate>
                        <%--                                                        <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Button_Submit" />
                                </Triggers>--%>
                    </asp:UpdatePanel>
                </div>

                <!--enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdateProgress runat="server" ID="UpdateProgress4" class="gettext" AssociatedUpdatePanelID="UpdatePanel4">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div id="enquiries_section" style="display: none" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <!--===============================================================================================-->
                            <div id="EORCartonList_section" visible="false" runat="server">
                                <asp:Label ID="hidden_Label_CartonRecId" class="gettext" runat="server" Visible="false" Style="color: tomato"></asp:Label>
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_EORCartonList" runat="server" OnClick="Button_EORCartonList_Click" Text="Carton List" class="frame_style_type2" Style="padding: 5px; margin-top: 5px;" />
                                    &nbsp                                                              
                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_enquiries_section_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" />
                                </div>
                                <div id="SearchCarton_Bar" visible="false" runat="server">
                                    <%--                                    <asp:CheckBox ID="CheckBox_div_Searchable_ID_Carton" runat="server" OnCheckedChanged="CheckBox_div_Searchable_Carton" Text="Show Search Bar" AutoPostBack="true" style="background-color:darkred;color: ;" Visible="true"/><br />--%>

                                    <div class="col-12 col-s-12" id="div_Searchable_CartonList" runat="server" visible="true">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search By:      </label>
                                            </div>

                                            <div class="col-3 col-s-8">
                                                <asp:DropDownList ID="DropDownList1" runat="server" class="inputtext">
                                                    <asp:ListItem Text="Item Name" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Item Id" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Search:      </label>
                                            </div>
                                            <div class="col-5 col-s-6">
                                                <asp:TextBox ID="TextBox_SearchCarton" class="inputtext" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton4" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_Carton_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="Div_GridViewCartonListing" visible="false" runat="server" class="col-12 col-s-12">
                                    <asp:Button ID="Button_CreateCartonNew" runat="server" OnClick="CreateSelect" Text="New List" class="glow_green" Enabled="true" />&nbsp
                                        <asp:Button ID="Button_Update" runat="server" OnClick="UpdateSelect" Text="Update" class="glow_green" Enabled="false" /><br />
                                    <br />
                                    <div id="Div6" runat="server" style="overflow-y: auto;">
                                        <asp:GridView ID="GridViewCartonListing" runat="server"
                                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_carton" AllowCustomPaging="True"
                                            HtmlEncode="False"
                                            Style="overflow: hidden;"
                                            AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRowSelect" runat="server" OnCheckedChanged="CheckBoxSelect_Changed" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="No." HeaderText="No." />
                                                <asp:BoundField DataField="Item Id" HeaderText="Item Id" />
                                                <asp:BoundField DataField="Item GroupId" HeaderText="Item GroupId" />
                                                <asp:BoundField DataField="Item Name" HeaderText="Item Name" />
                                                <asp:BoundField DataField="Sman From" HeaderText="Sman From" />
                                                <asp:BoundField DataField="Sman To" HeaderText="Sman To" />
                                                <asp:BoundField DataField="Effective Date" HeaderText="Effective Date" />
                                                <asp:BoundField DataField="CTN_Dep" HeaderText="CTN_Dep" />
                                                <asp:BoundField DataField="CTN_NoDep" HeaderText="CTN_NoDep" />
                                                <asp:BoundField DataField="Deposit" HeaderText="Deposit (RM)" />
                                                <asp:BoundField DataField="Contract Duration" HeaderText="Contract Duration (Month)" HeaderStyle-Wrap="true" />
                                                <asp:BoundField DataField="RecId" HeaderText="RecId" Visible="false" />
                                            </Columns>
                                            <HeaderStyle CssClass="header" />
                                            <PagerSettings PageButtonCount="2" />
                                            <PagerStyle CssClass="pager" />
                                            <RowStyle CssClass="rows" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div id="Div_NewCartonListing" visible="false" runat="server" class="col-12 col-s-12">
                                    <asp:Button ID="Button_CreateNewList" runat="server" Text="" OnClick="Button_CreateNewList_Click" class="glow_green" Enabled="true" /><br />
                                    <br />

                                    <div class="col-12 col-s-12">
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Item ID*:      </label>
                                            </div>
                                            <div class="col-3 col-s-8">
                                                <asp:Label ID="Label_ItemID" class="gettext" runat="server" Text=""></asp:Label>&nbsp
                                                    <asp:Button ID="Button_ItemID" runat="server" OnClick="CheckItemInList" CausesValidation="false" Text="Find in list" class="glow" Style="margin-bottom: 4px" />&nbsp     
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Item Name:      </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:Label ID="Label_ItemName" class="gettext" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-12 col-s-12">
                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Sman From*:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:DropDownList ID="DropDownList_SmanFrom" runat="server" autocomplete="off" class="inputtext" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-12 col-s-12">
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Sman To*:      </label>
                                                </div>

                                                <div class="col-3 col-s-8">
                                                    <asp:DropDownList ID="DropDownList_SmanTo" runat="server" class="inputtext" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_SmanTo" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Period (Month)*:      </label>
                                        </div>

                                        <div class="col-3 col-s-8">
                                            <asp:DropDownList ID="DropDownList_ContractDuration" runat="server" AutoPostBack="true" class="inputtext">
                                                <asp:ListItem Text="12 months" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="24 months" Value="24"></asp:ListItem>
                                                <asp:ListItem Text="36 months" Value="36"></asp:ListItem>
                                                <asp:ListItem Text="48 months" Value="48"></asp:ListItem>
                                                <asp:ListItem Text="60 months" Value="60"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Deposit Required (RM):      </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:TextBox ID="TextBox_DepositRequired" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Carton Without Deposit (Carton):      </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:TextBox ID="TextBox_CartonWithoutDeposit" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Carton With Deposit (Carton):      </label>
                                        </div>
                                        <div class="col-3 col-s-8">
                                            <asp:TextBox ID="TextBox_CartonWithDeposit" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Effective Date:</label>
                                        </div>

                                        <div class="col-3 col-s-8">
                                            </label><asp:Label ID="Label_EffectiveDate" class="gettext" runat="server" Text=" "></asp:Label>
                                            <div class="image_properties">
                                                <asp:ImageButton ID="ImageButton7" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/calendar_s.jpg" />
                                                <asp:ImageButton ID="ImageButton8" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/calendar_s_h.jpg" OnClick="Button_SalesmanTotal_StartDate_Click" />
                                            </div>
                                            <asp:Calendar ID="Calendar_EffectiveDate" class="inputcalendar" runat="server" SelectionMode="Day" OnSelectionChanged="Calendar_EffectiveDate_SelectionChanged" Visible="false"></asp:Calendar>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--===============================================================================================-->

                            <!--===============================================================================================-->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!--end of enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
            </div>
        </div>
        <%--        <script src="scripts/ButtonUp.js"></script>--%>
    </form>
</body>
</html>
