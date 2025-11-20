<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="DotNet.AddUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.2.3/css/responsive.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/fixedheader/3.1.2/css/fixedHeader.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/select/1.3.1/css/select.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/rowreorder/1.2.8/css/rowReorder.dataTables.min.css" />

    <title>Add User</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

    <style>
        .frame_style {
            margin-bottom: 5px;
        }

        .glow_green {
            min-width: 150px;
        }

        .button-margin {
            margin-right: 10px;
        }
    </style>
</head>

<script type="text/javascript">
    function gv_Search(strKey) {
        var strData = strKey.value.toLowerCase().split(" ");
        var tblData = document.getElementById("<%=GridView3.ClientID %>");
        var rowData;
        for (var i = 1; i < tblData.rows.length; i++) {
            rowData = tblData.rows[i].innerHTML;
            var styleDisplay = 'none';
            for (var j = 0; j < strData.length; j++) {
                if (rowData.toLowerCase().indexOf(strData[j]) >= 0) {
                    styleDisplay = '';
                }
                else {
                    styleDisplay = 'none';
                    break;
                }
            }
            tblData.rows[i].style.display = styleDisplay;
        }
    }

</script>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/AddUser.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1>Add User</h1>
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

                <div class="mobile_hidden">
                    <a href="Setting.aspx" id="SettingTag2" class="active" runat="server">Setting</a>
                </div>
                <div class="mobile_show">
                    <a href="Setting.aspx" id="SettingTag3" class="active" runat="server">Setting (Add User)</a>
                </div>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>

                    <%--                        <img src="RESOURCES/LogOut.png" />
                        <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
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

            <div class="col-12 col-s-12">

                <!--==============================================================================-->
                <div class="col-12 col-s-12">
                    <asp:Button ID="Button_AddUser_section" runat="server" OnClick="Button_section_Click" Text="Add/Update" class="frame_style" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style" />
                    <asp:Button ID="Button_DotNetUser_section" runat="server" OnClick="Button_section_Click" Text="SFA User" class="frame_style" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style" />
                    <%--<asp:Button ID="Button_VisitorUser_Section" runat="server" OnClick="Button_section_Click" Text="Visitor User" class="frame_style" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style" />--%>
                    <asp:Button ID="Button_ActiveDirectory_section" runat="server" OnClick="Button_section_Click" Text="AD User" class="frame_style" />
                </div>

                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <!--==============================================================================-->

                <div id="AddUser_section" style="display: none" runat="server">

                    <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div class="loading">

                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>

                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div runat="server" class="col-6 col-s-12" id="RegistrationTypeWrapper">
                                <div class="col-3 col-s-4">
                                    <asp:Label ID="lbltype" runat="server" Text="Registration Type:" class="labeltext"></asp:Label>
                                    <%--                                    <asp:RequiredFieldValidator ID="rfvRegistration" runat="server" ControlToValidate="ddlRegistrationType" Display="Dynamic" InitialValue="0"
                                        ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                </div>
                                <%-- KX,JJ 25/3 --%>
                                <div class="col-4 col-s-8">
                                    <asp:DropDownList ID="ddlRegistrationType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RegistrationTypeChanged" class="dropdownlist">
                                        <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="SFA User" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Visitor Management User" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="License Management User" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>



                            <asp:Panel ID="FormWrapper" runat="server" CssClass="button-container" Visible="false">

                                <div class="col-12 col-s-12">
                                    <div class="col-2 col-s-6">
                                        <label class="labeltext">User ID:      </label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:TextBox ID="UserID" class="inputtext" runat="server" AutoPostBack="true" OnTextChanged="CheckID"></asp:TextBox>

                                        <div id="checkun" runat="server" visible="false">
                                            <asp:Image ID="shwimg" class="image_indicator" runat="server" />
                                            <asp:Label ID="lblmsg" class="indicator" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-2 col-s-6">
                                        <label class="labeltext">User Name:      </label>
                                    </div>

                                    <div class="col-4 col-s-4">
                                        <asp:TextBox ID="UserName" class="gettext" runat="server" Text=" "></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:Panel ID="AuthorityLevelWrapper" runat="server" CssClass="button-container" Visible="false">

                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Authority Level:      </label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:DropDownList ID="ddlAuthorityLevel" runat="server" class="dropdownlist"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="UserRoleWrapper" runat="server" CssClass="button-container" Visible="false">

                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">User Role:      </label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:DropDownList ID="ddlUserRole" runat="server" class="dropdownlist"></asp:DropDownList>
                                        </div>
                                    </asp:Panel>

                                    <div class="col-2 col-s-6">
                                        <label class="labeltext">Company:      </label>
                                    </div>

                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="Company" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>
                                <%-- JJ 25/3 --%>
                                <asp:Panel ID="LicenseWrapper" runat="server" CssClass="button-container" Visible="false">
                                    <div class="col-12 col-s-12">
                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Email:</label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="inputtext" />
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                ErrorMessage="Email is required." CssClass="text-danger" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Invalid email format."
                                                CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Phone Number:</label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="inputtext" />
                                            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                                ErrorMessage="Phone number is required." CssClass="text-danger" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                                                ValidationExpression="^\d{10,15}$" ErrorMessage="Enter a valid phone number (10-15 digits) without space or symbol."
                                                CssClass="text-danger" Display="Dynamic" />
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Department:</label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdownlist" />
                                            <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment"
                                                InitialValue="" ErrorMessage="Department is required." CssClass="text-danger" Display="Dynamic" />
                                        </div>

                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Role:</label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdownlist" />
                                            <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole"
                                                InitialValue="" ErrorMessage="Role is required." CssClass="text-danger" Display="Dynamic" />
                                        </div>
                                    </div>

                                    <%--<div class="col-12 col-s-12">
                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Company access:</label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdownlist" />
                                             will be a table below might can be used
                                        </div>
                                    </div>--%>

                                    <div class="col-2 col-s-6">
                                        <label class="fw-bold">Assign Company Access:</label>
                                        <div class="checkbox-container border rounded p-2">
                                            <asp:CheckBoxList ID="chkCompanies" runat="server" CssClass="w-100"></asp:CheckBoxList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <!--==============================================================================-->

                                <asp:Panel ID="AccessingWrapper" runat="server" CssClass="button-container" Visible="false">

                                    <div class="col-12 col-s-12">
                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Module access:      </label>
                                        </div>

                                        <div class="col-4 col-s-4">
                                            <asp:GridView ID="GridView2" runat="server"
                                                HorizontalAlign="left" CssClass="mydatagrid"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll2" runat="server" Text="Select ALL" OnCheckedChanged="CheckBoxAll_Changed2" AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow2" runat="server" OnCheckedChanged="CheckBox_Changed2" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <!--==============================================================================-->
                                        <div class="col-2 col-s-6">
                                            <label class="labeltext">Company access:      </label>
                                        </div>
                                        <div class="col-4 col-s-4">
                                            <asp:GridView ID="GridView1" runat="server"
                                                PageSize="10" HorizontalAlign="left"
                                                CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" Text="Select ALL" OnCheckedChanged="CheckBoxAll_Changed" AutoPostBack="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="CheckBox_Changed" AutoPostBack="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <div class="col-12 col-s-12" style="display: flex; justify-content: center; margin-top: 20px;">
                                    <%--<asp:Button ID="BtnCheckPassword" runat="server" OnClick="CheckUserPassword" Text="Check Password" class="glow_green button-margin" Enabled="true" />--%>
                                    <asp:Button ID="BtnSFARegistration" runat="server" OnClick="Save_UserRegistration_Click" Text="Add" class="glow_green" Enabled="false" />
                                    <asp:Button ID="BtnVisitorRegistration" runat="server" OnClick="Save_UserRegistration_Click" Text="Add" class="glow_green" Enabled="false" />
                                    <asp:Button ID="BtnLicenseRegistration" runat="server" OnClick="Save_UserRegistration_Click" Text="Add" class="glow_green" Enabled="false" />
                                    <%---- JJ 25/3 ----%>
                                    <% if (Session["user_authority_lvl"].ToString() == "1")
                                        { %>
                                    <asp:Button ID="BtnUpdateCompanyList" runat="server" OnClick="UpdateCompanyListBtn_Click" Text="Update Company List" class="glow_green" CausesValidation="false" />
                                    <%-- JJ 25/3 --%>
                                    <%}%>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div id="DotNetUser_section" style="display: none" runat="server">

                    <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext" AssociatedUpdatePanelID="UpdatePanel2">
                        <ProgressTemplate>
                            <div class="loading">
                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="col-6 col-s-12">
                                <%--                                <div class="col-6 col-s-6">
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="lblSearch" runat="server" Text="Search: " CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="inputtext" autocomplete="off" onkeyup="gv_Search(this)"></asp:TextBox><br />
                                    </div>
                                </div>--%>
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_Delete" runat="server" OnClick="DeleteSelect" Text="Delete" class="glow_red  " Enabled="false" Style="margin: 4px 0px" />
                                </div>
                            </div>
                            <div class="col-12 col-s-12">
                                <div id="grdCharges" runat="server" class="gridview-container" style="max-width: 110%; max-height: 100%;">
                                    <asp:GridView ID="GridView3" runat="server"
                                        HorizontalAlign="left"
                                        CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        OnRowDataBound="GridView_RowDataBound_Header"
                                        HtmlEncode="False" Style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowDelete" runat="server" OnCheckedChanged="CheckBox_Changed_Delete" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>


                <div id="VisitorSection" style="display: none" runat="server">

                    <asp:UpdateProgress runat="server" ID="UpdateProgressVisitor" class="gettext" AssociatedUpdatePanelID="UpdatePanelVisitor">
                        <ProgressTemplate>
                            <div class="loading">
                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="UpdatePanelVisitor" runat="server">
                        <ContentTemplate>
                            <div class="col-6 col-s-12">
                                <%--                                <div class="col-6 col-s-6">
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="lblSearch" runat="server" Text="Search: " CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-4 col-s-4">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="inputtext" autocomplete="off" onkeyup="gv_Search(this)"></asp:TextBox><br />
                                    </div>
                                </div>--%>
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_DeleteVisitorUser" runat="server" OnClick="DeleteSelectedVisitor" Text="Delete" class="glow_red  " Enabled="false" Style="margin: 4px 0px" />
                                </div>
                            </div>
                            <div class="col-12 col-s-12">
                                <div id="Div2" runat="server" class="gridview-container" style="max-width: 110%; max-height: 100%; display: flex; justify-content: center">
                                    <asp:GridView ID="GridViewVisitor" runat="server"
                                        HorizontalAlign="left"
                                        CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        OnRowDataBound="GridView_RowDataBound_VisitorUsersHeader"
                                        HtmlEncode="False" Style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowDelete" runat="server" OnCheckedChanged="CheckBoxVisitorUser_Changed_Delete" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>


                <div id="ActiveDirectory_section" style="display: none" runat="server">

                    <asp:UpdateProgress runat="server" ID="UpdateProgress3" class="gettext" AssociatedUpdatePanelID="UpdatePanel3">
                        <ProgressTemplate>
                            <div class="loading">

                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>

                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="col-12 col-s-12">
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Search By:      </label>
                                    </div>

                                    <div class="col-3 col-s-8">
                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" class="inputtext">
                                            <asp:ListItem Text="ID" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Name" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Search:      </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_SearchAd" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton2" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="SearchAd" />
                                    </div>
                                </div>
                            </div>

                            <asp:DropDownList ID="DropDownListResetAdUser" runat="server" AutoPostBack="true" class="inputtext" Style="min-width: 20%; max-width: 30%;" Visible="false" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownListResetAdUser">
                                <asp:ListItem Text="--- Select RESET Password Option ---" Value=""></asp:ListItem>
                                <asp:ListItem Text="Reset with default Pwd" Value="1"></asp:ListItem>
                                <asp:ListItem Text="User Required Change Pwd on Next Login" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Reset with default Pwd + User Required Change Pwd on Next Login" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Reset with customize Pwd" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Reset with customize Pwd + User Required Change Pwd on Next Login" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="TextBox_CustomizedPwd" placeholder="Enter Customized Password" autocomplete="off" class="inputtext" runat="server" Visible="false"></asp:TextBox>
                            &nbsp
                            <asp:Button ID="Button_ExecuteReset" runat="server" OnClick="Button_ExecuteReset_Click" Text="EXECUTE" class="glow_green" Visible="false" />

                            <div class="col-12 col-s-12">
                                <asp:GridView ID="GridView_ActiveDirectory" runat="server"
                                    PageSize="20" HorizontalAlign="Left"
                                    CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="True"
                                    OnPageIndexChanging="datagrid_PageIndexChanging_ActiveDirectory" AllowCustomPaging="True"
                                    HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                    <Columns>

                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRowResetPwd" runat="server" OnCheckedChanged="CheckBox_ResetPwd" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="No" HeaderText="No" />
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="Full Name" HeaderText="Full Name" />
                                        <asp:BoundField DataField="User Domain" HeaderText="User Domain" />
                                        <asp:BoundField DataField="PWD Last Set" HeaderText="PWD Last Set" />
                                        <asp:BoundField DataField="PWD Expiration" HeaderText="PWD Expiration" />
                                        <asp:BoundField DataField="Account Active" HeaderText="Account Active" />
                                        <asp:BoundField DataField="Account Expiration" HeaderText="Account Expiration" />
                                    </Columns>
                                    <HeaderStyle CssClass="header" />
                                    <PagerSettings PageButtonCount="2" />
                                    <PagerStyle CssClass="pager" />
                                    <RowStyle CssClass="rows" />
                                </asp:GridView>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>

    </form>
</body>
<script src="https://code.jquery.com/jquery-3.5.1.js" integrity="sha256-QWo7LDvxbWT2tbbQ97B53yJnYU3WhH/C8ycbRAkjPDc=" crossorigin="anonymous"></script>
<script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
<script>
    var table = $('#GridView3').DataTable({
        //select: true,
        fixedHeader: true,

        "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
        //"scrollY":"210px",
        //"scrollCollapse": "true",
        "autowidth": "false",
        "columnDefs": [
            {
                //"targets": [ 1 ],
                //"visible": false
            }
        ],
        dom: 'Blfrtip',
        buttons: [
            'copy',
            {
                extend: 'csv',
                title: 'Report ' + $("#report_table option:selected").text(),
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                title: 'Report ' + $("#report_table option:selected").text(),
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdf',
                title: 'Report ' + $("#report_table option:selected").text(),
                exportOptions: {
                    columns: ':visible'
                }
            },
            'print'
        ]
    });
</script>

</html>
