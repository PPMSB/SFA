<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RedempApprovalList.aspx.cs" Inherits="DotNet.RedempApprovalList" MaintainScrollPositionOnPostback="true" %>

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

    <title>Approval Group</title>
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
                    <img src="RESOURCES/REDEMPAPPROVAL.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Redemption Approval</h1>
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
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="btn btn-danger btn-sm" />
                        <asp:Button ID="btnSaveGroup" runat="server" OnClick="btnSaveGroup_Click" Text="Save" class="btn btn-success btn-sm" Visible="false" />
                        <asp:Button ID="btnUpd" runat="server" OnClick="btnUpd_Click" Text="Update" class="btn btn-primary btn-sm" Visible="false" />&nbsp
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" class="btn btn-secondary btn-sm" Visible="false" CausesValidation="false" />&nbsp
                    </div>
                    <%--                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>

            </div>
            <div class="col-12 col-s-12">
                <div class="table-responsive" runat="server" id="divApproval" style="max-width: 100%; max-height: 110%;">
                    <asp:GridView ID="gvApproval" runat="server" Width="100%" OnRowCommand="gvApproval_RowCommand" DataKeyNames="No., hdRecId"
                        CssClass="table table-bordered table-striped table-hover" AutoGenerateColumns="False" RowStyle-CssClass="rows">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="No." HeaderText="No." ReadOnly="true">
                                <ControlStyle Width="2"></ControlStyle>
                            </asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CssClass="fas fa-edit" ToolTip="Edit!" CommandName="edt"></asp:LinkButton>
                                    <%--                                    <asp:ImageButton ID="imgbtnEdit" runat="server" ImageUrl="RESOURCES/edit.png" ToolTip="Edit!" CommandName="edt" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AmountRange" HeaderText="Amount Range (RM)" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="SalesAdmin1" HeaderText="SA 1" />
                            <asp:BoundField DataField="SalesAdmin2" HeaderText="SA 2" />
                            <asp:BoundField DataField="SalesAdmin3" HeaderText="SA 3" />
                            <asp:BoundField DataField="SalesAdminMng1" HeaderText="SA Manager 1" />
                            <asp:BoundField DataField="SalesAdminMng2" HeaderText="SA Manager 2" />
                            <asp:BoundField DataField="SalesAdminMng3" HeaderText="SA Manager 3" />
                            <asp:BoundField DataField="CreditControlMng1" HeaderText="CC Manager 1" />
                            <asp:BoundField DataField="CreditControlMng2" HeaderText="CC Manager 2" />
                            <asp:BoundField DataField="CreditControlMng3" HeaderText="CC Manager 3" />
                            <asp:BoundField DataField="OpMng1" HeaderText="Operation Manager 1" />
                            <asp:BoundField DataField="OpMng2" HeaderText="Operation Manager 2" />
                            <asp:BoundField DataField="OpMng3" HeaderText="Operation Manager 3" />
                            <asp:BoundField DataField="GM1" HeaderText="GM 1" />
                            <asp:BoundField DataField="GM2" HeaderText="GM 2" />
                            <asp:BoundField DataField="hdRecId" HeaderText="hdRecId" />
                            <%--                            <asp:BoundField DataField="DefaultGM" HeaderText="Default GM" />--%>
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>

            <div class="col-12 col-s-12" id="divEdit" runat="server" visible="false">

                <asp:Button ID="btnEdit" runat="server" Text="Approval Edit" class="accordion_style_sub_fixed_darkcyan" Visible="false" />

                <div class="col-12 col-s-12">
                    <div class="col-1 col-s-4">
                        <asp:Label ID="lblAmountRange" runat="server" Text="Amount Range " CssClass="labeltext"></asp:Label>
                    </div>
                    <div class="col-8 col-s-6">
                        <asp:TextBox ID="txtAmountFrom" runat="server" CssClass="gettext"></asp:TextBox>
                        <%--                        <asp:RegularExpressionValidator ID="revAmountFrom" runat="server" ControlToValidate="txtAmountFrom" ValidationExpression="^(?:[0-9]\d{0,6}|9999999)$"
                            ErrorMessage="Numbers only!" SetFocusOnError="true" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                        -
                        <asp:TextBox ID="txtAmountTo" runat="server" CssClass="gettext"></asp:TextBox>
                        <%--                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAmountTo" ValidationExpression="^(?:[1-9]\d{0,6}|9999999)$"
                            ErrorMessage="Numbers only!" SetFocusOnError="true" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                    </div>
                </div>

                <div class="col-12 col-s-12">
                    <div class="col-6 col-s-12">
                        <div class="col-6 col-s-12" style="display: flex; justify-content: space-between;">
                            <asp:Label ID="lblSalesAdmin" runat="server" Text="Sales Admin" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlSalesAdmin1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlSalesAdmin2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlSalesAdmin3" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-6 col-s-12" style="display: flex; justify-content: space-between;">
                            <asp:Label ID="lblSalesAdminMng" runat="server" Text="Sales Admin Manager" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlSalesAdminManager1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlSalesAdminManager2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlSalesAdminManager3" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-6 col-s-12" style="display: flex; justify-content: space-between;">
                            <asp:Label ID="lblOperationMng" runat="server" Text="Operation Manager" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlOperationMng1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlOperationMng2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlOperationMng3" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-6 col-s-12" style="display: flex; justify-content: space-between;">
                            <asp:Label ID="lblCcMng" runat="server" Text="Credit Control Manager" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlCcMng1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlCcMng2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlCcMng3" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-6 col-s-12" style="display: flex; justify-content: space-between;">
                            <asp:Label ID="lblGM" runat="server" Text="General Manager" CssClass="labeltext"></asp:Label>
                            <div style="display: flex; flex-direction: column;">
                                <asp:DropDownList ID="ddlGM1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                                <asp:DropDownList ID="ddlGM2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                            </div>
                        </div>

                    </div>
                </div>

                <asp:HiddenField ID="hdRecId" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
