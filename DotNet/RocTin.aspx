<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RocTin.aspx.cs" Inherits="DotNet.RocTin" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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

    <title>ROC & TIN</title>
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
                    <img src="RESOURCES/Einvoice.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>ROC & TIN</h1>
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
                <a href="WClaim.aspx" id="WClaimTag2" class="active" runat="server" visible="false">Warranty</a>
                <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Budget</a>

                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <div class="global_container" onclick=" myFunction(this);">
                        <div class="bar1"></div>
                        <div class="bar2"></div>
                        <div class="bar3"></div>
                    </div>
                </a>
                <script src="scripts/top_navigation.js"></script>
            </div>
            <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="col-12 col-s-12">
                <asp:Button ID="btnOverview" runat="server" OnClick="btnOverview_Click" Text="Overview" class="frame_style_4bar_Warranty" />
                <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />

                <asp:Button ID="Button_enquiries_section" runat="server" Text="Enquiries" OnClick="Button_enquiries_section_Click" class="frame_style_4bar_Warranty" />
            </div>
            <div class="col-12 col-s-12">
                <p>(<span style="color: red">*</span> For any updates regarding ROC or TIN, kindly reach out to the Credit Control team.)</p>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            F
            <div id="dvUpdate" runat="server" visible="false">
                <div class="col-12 col-s-12">
                    <div class="col-2 col-s-4">
                        <asp:Label ID="Label1" runat="server" Text="1. Customer Name" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:Label ID="lblGetCustName" runat="server" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-2 col-s-4">
                        <asp:Label ID="Label3" runat="server" Text="2. Customer Account" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:Label ID="lblGetCustAcc" runat="server"></asp:Label>
                    </div>
                    <div class="col-2 col-s-4">
                        <asp:Label ID="lblPhone" runat="server" Text="3. Phone" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:Label ID="lblGetPhone" runat="server"></asp:Label>
                    </div>
                    <div class="col-2 col-s-4">
                        <asp:Label ID="lblOldROC" runat="server" Text="4. Old ROC" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:Label ID="lblGetOldRoc" runat="server"></asp:Label>
                    </div>

                </div>

                <div class="col-12 col-s-12">
                    <div class="col-2 col-s-4">
                        <asp:Label ID="lblNewROC" runat="server" Text="5. New ROC" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:TextBox ID="txtNewRoc" runat="server" class="gettext" Text=""></asp:TextBox>
                    </div>
                    <div class="col-2 col-s-4">
                        <asp:Label ID="lblEmail" runat="server" Text="6. E-Invoice Email" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:TextBox ID="txtGetEmail" runat="server" class="gettext" OnTextChanged="txtGetEmail_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:Label ID="LblValidEmail" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="col-2 col-s-4">
                        <asp:Label ID="Label4" runat="server" Text="7. SST Number" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-10 col-s-8">
                        <asp:TextBox ID="txtGetSSTNo" runat="server" class="gettext"></asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-s-12" runat="server" id="divTIN">
                    <div class="col-2 col-s-4">
                        <asp:Label ID="lblTin" runat="server" Text="8. TIN Number" class="labeltext"></asp:Label>
                    </div>
                    <div class="col-3 col-s-8">
                        <asp:TextBox ID="txtTIN" runat="server" class="gettext" placeholder="eg. IG123456" AutoPostBack="true" OnTextChanged="txtTIN_TextChanged"></asp:TextBox>
                        <asp:Label ID="lblValidateTin" runat="server" Text="TIN required!" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="col-12 col-s-12" runat="server" visible="false">
                    <div class="col-10 col-s-12">
                        <asp:CheckBox ID="chkAbove100mT1" runat="server" Text="Tier 1 1/8/2024" />
                    </div>
                    <div class="col-6 col-s-12">
                        <asp:CheckBox ID="chkAbove100mT2" runat="server" Text="Tier 2 1/1/2025" />
                    </div>
                </div>

                <div class="col-12 col-s-12">
                    <div class="col-2 col-s-4">
                        <asp:Label ID="Label13" runat="server" Text="Salesman Remark" CssClass="labeltext"></asp:Label>
                    </div>
                    <div class="col-6 col-s-8">
                        <asp:TextBox ID="txtSalesmandRemark" class="txt_reason" TextMode="MultiLine" Height="90px" Width="300px"
                            Row="3" Columns="20" runat="server" CssClass="txt_reason"></asp:TextBox>
                        <asp:Label ID="lblRemark" runat="server" Text="Remark required!" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-12 col-s-12" runat="server" id="dvButtonUpd">
                <asp:Button ID="btnUpd" runat="server" OnClick="btnUpd_Click" Text="Save" CssClass="btn btn-primary btn-sm" />&nbsp
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-secondary btn-sm" CausesValidation="false" />&nbsp
            </div>

            <div id="dvOverview" runat="server">

                <div class="col-12 col-s-12" runat="server" id="dvSearchCust">
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
                </div>
                <%--                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export" CssClass="btn btn-secondary btn-sm" CausesValidation="false" />--%>


                <div class="col-12 col-s-12" id="dvEnquiries" runat="server">

                    <div class="col-12 col-s-12">
                        <div class="col-2 col-s-4">
                            <label class="labeltext">Salesman:      </label>
                        </div>
                        <div class="col-6 col-s-8">
                            <div class="col-6 col-s-9">
                                <asp:DropDownList ID="DdlRoc" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="DdlRoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-12 col-s-12">
                            <div class="col-6 col-s-6">
                                <div class="col-4 col-s-4">
                                    <label class="labeltext">Total Customer:      </label>
                                </div>
                                <div class="col-4 col-s-8">
                                    <asp:Label ID="lblTotalCust" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="col-6 col-s-6">
                                <div class="col-4 col-s-4">
                                    <label class="labeltext">Total Updated Tin&Email:      </label>
                                </div>
                                <div class="col-4 col-s-6">
                                    <asp:Label ID="lblTotalUpdate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-6 col-s-6">
                                <div class="col-4 col-s-4">
                                    <label class="labeltext">Total Updated Tin:      </label>
                                </div>
                                <div class="col-4 col-s-6">
                                    <asp:Label ID="lblTotalOnly" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

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

                    <div id="grdCharges" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;" class="table-responsive">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                           <ContentTemplate>
                         <asp:GridView ID="gvRocTin" runat="server" PagerStyle-CssClass="page-link"
                            PageSize="20" HorizontalAlign="Left" AllowCustomPaging="true"
                            CssClass="mydatagrid" AllowPaging="True" EmptyDataText="No records found." 
                            OnPageIndexChanging="datagrid_PageIndexChanging" EnableSortingAndPagingCallbacks="true" 
                            HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False" 
                            OnSelectedIndexChanged="gvRocTin_SelectedIndexChanged" >
                            <%--OnRowCommand="gvRocTin_RowCommand" RowStyle-CssClass="rows"--%>
                            <Columns>
                                <asp:BoundField DataField="No" HeaderText="No" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Account">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Account" runat="server" CommandName="Select" 
                            CommandArgument='<%# Eval("Account") %>'  Text='<%# Eval("Account") %>' CssClass="link_account" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                <asp:BoundField DataField="Mobile Phone" HeaderText="Mobile Phone" />
                                <asp:BoundField DataField="ROC" HeaderText="ROC" />
                                <asp:BoundField DataField="New ROC" HeaderText="New ROC" />
                                <asp:BoundField DataField="SST No" HeaderText="SST No" />
                                <asp:BoundField DataField="TIN" HeaderText="TIN" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                <asp:BoundField DataField="Salesman ID" HeaderText="Salesman ID" />
                                <asp:BoundField DataField="Updated" HeaderText="Updated" />
                            </Columns>
                             <HeaderStyle CssClass="header" />
                            <PagerSettings PageButtonCount="2" />
                            <RowStyle CssClass="rows" />
                        </asp:GridView>
                       </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvRocTin" EventName="PageIndexChanging" />
                        <asp:AsyncPostBackTrigger ControlID="gvRocTin" EventName="Sorting" />
                        </Triggers>
                  </asp:UpdatePanel>
                   </div>
                </div>

            </div>

        </div>

    </form>
</body>
</html>
