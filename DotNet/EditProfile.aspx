<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="DotNet.EditProfile" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <title>Edit Profile</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/EditProfile.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Edit Profile</h1>
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

                <div class="mobile_hidden">
                    <a href="Setting.aspx" id="SettingTag2" class="active" runat="server">Setting</a>
                </div>
                <div class="mobile_show">
                    <a href="Setting.aspx" id="SettingTag3" class="active" runat="server">Setting (Edit Profile)</a>
                </div>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">

                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%--                      <img src="RESOURCES/menu.png" />--%>
                    <div class="global_container" onclick=" myFunction(this);">
                        <div class="bar1"></div>
                        <div class="bar2"></div>
                        <div class="bar3"></div>
                    </div>

                </a>
                <script src="scripts/top_navigation.js"></script>
            </div>
            <!--==============================================================================-->
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
                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext">User Id:      </label>
                            </div>
                            <div class="col-10 col-s-6">
                                <asp:TextBox ID="TextBox_UserId" class="inputtext" runat="server" AutoPostBack="true" OnTextChanged="CheckNewID"></asp:TextBox>
                            </div>
                        </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server" />
                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext">User Name:      </label>
                            </div>

                            <div class="col-10 col-s-6">
                                <asp:Label ID="Label_UserName" class="gettext" runat="server" Text=" "></asp:Label>
                            </div>
                        </div>

                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext">Current selected Company:      </label>
                            </div>

                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="col-10 col-s-6">
                                        <asp:Label ID="Label3" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext">Switch Company:      </label>
                            </div>

                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="col-10 col-s-6">
                                        <asp:DropDownList ID="DropDownList_SwitchCompany" runat="server" class="dropdownlist" OnSelectedIndexChanged="DropDownList_SwitchCompany_Change" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <!--==============================================================================-->
                        <div class="col-12 col-s-12">
                            <asp:Button ID="Button_Save" runat="server" OnClick="Save_Click" Text="Save" class="glow" Visible="false" />
                        </div>
                        <!--==============================================================================-->

                        <asp:Label ID="hidden_module_access_authority" class="gettext" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="hidden_user_authority_lvl" class="gettext" runat="server" Visible="false"></asp:Label>

                        <!--==============================================================================-->
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </form>
</body>
</html>