<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalList.aspx.cs" Inherits="DotNet.ApprovalList" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="STYLES/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" crossorigin="anonymous" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="scripts/jquery/datatables.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.min.css" />
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />

    <title>Approval Group</title>
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
                    <img src="RESOURCES/WARRANTYAPPROVAL.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Warranty Approval</h1>
                </div>
            </div>
            <!--==============================================================================-->
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
                <a href="Setting.aspx" class="active" id="SettingTag2" runat="server">Setting</a>
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
                <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="col-12 col-s-12">
                    <%--                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>
                    <asp:Button ID="Accordion_ApprovalInfo" runat="server" Text="Approval Info" class="accordion_style_sub_fixed_darkcyan" />
                    <div class="col-12 col-12">
                        <%--                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" class="glow_green" />
                                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" class="glow_red" />
                                <asp:Button ID="btnSaveGroup" runat="server" OnClick="btnSaveGroup_Click" Text="Save" class="frame_style_type2" Visible="false" />--%>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" class="btn btn-primary btn-sm" />
                        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" class="btn btn-danger btn-sm" />
                        <asp:Button ID="btnSaveGroup" runat="server" OnClick="btnSaveGroup_Click" Text="Save" class="btn btn-success btn-sm" Visible="false" />
                        <asp:Button ID="btnUpd" runat="server" OnClick="btnUpd_Click" Text="Update" CssClass="btn btn-primary btn-sm" Visible="false" />&nbsp
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-secondary btn-sm" Visible="false" />&nbsp
                        <asp:DropDownList ID="ddlWarehouseSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LocationChanged_Warranty" class="dropdownlist"></asp:DropDownList>
                    </div>


                    <%--                        <div class="col-6 col-s-12">
                             <div class="col-3 col-s-4">
                                 <label class="labeltext">Search:      </label>
                             </div>
                             <div class="col-3 col-s-8">
                              
                                 <asp:TextBox ID="TextBox_Account" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>      
                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton1" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/Search.png" />
                                    <asp:ImageButton ID="ImageButton2" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/Search_h.png"/>
                        
                                </div>
                             </div>
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>

            </div>
            <div class="mt_1 col-12 col-s-12" runat="server" id="divApproval">
                <div class="table-responsive" style="max-width: 110%; overflow: auto; max-height: 100%;">
                    <asp:GridView ID="gvApproval" runat="server" CssClass="table table-bordered table-striped table-hover" RowStyle-Font-Size="Small" OnPageIndexChanging="datagrid_PageIndexChanging"
                        AllowPaging="True" RowStyle-CssClass="rows" Width="100%" RowStyle-Wrap="true" EmptyDataText="No records found." AllowSorting="true"
                        OnRowCommand="gvApproval_RowCommand" DataKeyNames="No. ,hdRecId" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="No." HeaderText="No."></asp:BoundField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CssClass="fas fa-edit" ToolTip="Edit!" CommandName="edt"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--                            <asp:ButtonField ButtonType="Link" CommandName="OnUpdate" Text="Update" />
                            <asp:ButtonField ButtonType="Link" CommandName="OnCancel" Text="Cancel" />--%>
                            <asp:BoundField DataField="Location" HeaderText="Location" HtmlEncode="False" />
                            <asp:BoundField DataField="WarrantyType" HeaderText="Warranty Type" />
                            <asp:BoundField DataField="InvCheck" HeaderText="Invoice Check" HtmlEncode="False" />
                            <asp:BoundField DataField="Transportation" HeaderText="Transportation" HtmlEncode="False" />
                            <asp:BoundField DataField="GoodsReceive" HeaderText="Goods Receive" HtmlEncode="False" />
                            <asp:BoundField DataField="Inspection" HeaderText="Inspection" HtmlEncode="False" />
                            <asp:BoundField DataField="Verification" HeaderText="Verification" HtmlEncode="False" />
                            <asp:BoundField DataField="Approval" HeaderText="Approval" HtmlEncode="False" />
                            <asp:BoundField DataField="hdRecId" HeaderText="hdRecId" HtmlEncode="false" />
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>

            <div class="col-12 col-sm-12" id="divEdit" runat="server" visible="false">

                <div class="row">
                    <div class="col-6 col-sm-6">
                        <label for="ddlWarehouse" class="labeltext">Warehouse</label>
                        <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-control inputtext">
                            <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-6 col-sm-6">
                        <label for="ddlWarranty" class="labeltext">Warranty Type</label>
                        <asp:DropDownList ID="ddlWarranty" runat="server" CssClass="form-control inputtext">
                            <asp:ListItem Text="-- SELECT --"></asp:ListItem>
                            <asp:ListItem Text="Batch Return" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Battery" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Other Products" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Lubricant" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-6 col-s-9">
                        <div class="col-12 col-sm-4">
                            <asp:Label ID="lblInvoiceCheck" runat="server" Text="Invoice Checker" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlInvoiceCheck1" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlInvoiceCheck2" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlInvoiceCheck3" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlInvoiceCheck4" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-9">
                        <div class="col-12 col-sm-4">
                            <asp:Label ID="lblInspector" runat="server" Text="Inspector" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlInspector1" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlInspector2" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlInspector3" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlInspector4" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-9">
                        <div class="col-12 col-sm-4">
                            <asp:Label ID="lblTransport" runat="server" Text="Transportation Arrangement" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlTransport1" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlTransport2" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlTransport3" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlTransport4" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-9">
                        <div class="col-12 col-sm-4">
                            <asp:Label ID="lblVerifier" runat="server" Text="Verifier" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlVerifier1" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlVerifier2" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlVerifier3" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlVerifier4" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-9">
                        <div class="col-12 col-sm-4">
                            <asp:Label ID="lblGoodReceive" runat="server" Text="Goods Receiver" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlGoodReceive1" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlGoodReceive2" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlGoodReceive3" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlGoodReceive4" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-9">
                        <div class="col-12 col-sm-4">
                            <asp:Label ID="lblApprover" runat="server" Text="Approver" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlApprover1" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlApprover2" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlApprover3" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:DropDownList ID="ddlApprover4" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdRecId" runat="server" />
        </div>

    </form>
</body>
</html>
