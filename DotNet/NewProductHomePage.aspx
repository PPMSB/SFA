<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewProductHomePage.aspx.cs" Inherits="DotNet.NewProductHomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Flip_Image.css" rel="stylesheet" />
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />


    <title> New Product Request Home </title>

    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>

    <style>
        .Button, .Buttton {
            background-color: lightgrey !important;
            padding: 5px 10px !important;
            border-radius: 5px !important;
            text-align: center !important;
            text-decoration: none !important;
            display: inline-block !important;
        }

        .modal {
            display: none; 
            position: fixed; 
            z-index: 1; 
            left: 0;
            top: 0;
            width: 100%; 
            height: 100%; 
            overflow: auto; 
            background-color: rgb(0,0,0); 
            background-color: rgba(0,0,0,0.4); 
            justify-content: center; 
            align-items: center; 
        }

        .modal-content {
            background-color: bisque;
            margin: auto;
            padding: 20px;
            border: 1px solid #888;
            border-radius: 5px;
        }

        .modalth {
            text-align: left;
        }

        .flex-container {
            display: flex;
        }

        .flex-item {
            margin-right: 10px;
        }

        .close {
            color: #aaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

        .close:hover,
        .close:focus {
            color: black;
            text-decoration: none;
            cursor: pointer;
        }

        .headdd {
            width: 100%;
        }

        .headerr {
            display: flex;
        }

        .headimg {
            width: 300px;
            height: 90px;
        }

        .headtxt {
            font-size: 25px;
        }

        .image_newProduct {
            height: 100%;
            width: 100%;
        }

    </style>

    <script type="text/javascript">
       
        function showModal() {
            document.getElementById("formModal").style.display = "flex";
        }

        function hideModal() {
            document.getElementById("formModal").style.display = "none";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="headdd">

            <div class="mobile_hidden ">
                <div class="col-3 col-s-3 image_icon " > 
                    <img src="RESOURCES/POSIM PETROLEUM MARKETING.png" class="image_newProduct" /> 
                </div>
                <div class="col-9 col-s-9 image_stitle">
                     <h1>New Product Request</h1>
                 </div>
            </div>
            <%--<div class="mobile_show">
                <img alt="LION POSIM" src="RESOURCES/LFIB_logo3.jpg" width="165" height="90" />
            </div>--%>
        </div>
        <!--==============================================================================-->
        
        <div class="menuHead">
            <div class="topnav" id="myTopnav">

                <a href="MainMenu.aspx" class="active">Home</a>

                <%--CUSTOMER--%>
                <div class="DropDown">
                    <button type="button" class="DDBtn">
                        Customer
                        <i class="fa fa-caret-down"></i>
                    </button>
                    <div class="DropDownList" id="DDCustomer">
                        <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" >Customer</a>
                        <a href="NewCustomer.aspx" id="NewCustomerTag2" runat="server" >New Customer</a>
                    </div>
                </div>
                <%--SALES--%>
                <div class="DropDown">
                    <button type="button" class="DDBtn">
                        Sales
                        <i class="fa fa-caret-down"></i>
                    </button>
                    <div class="DropDownList" id="DDSales">
                        <a href="SFA.aspx" id="SFATag2" runat="server" >Sales</a>
                        <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" >Quotation</a>
                        <a href="Payment.aspx" id="PaymentTag2" runat="server" >Payment</a>
                        <a href="Redemption.aspx" id="RedemptionTag2" runat="server" >Redemption</a>
                    </div>
                </div>
                <%--EOR INVENTORY ETC--%>
                <div class="DropDown">
                    <button type="button" class="DDBtn">
                        Others
                        <i class="fa fa-caret-down"></i>
                    </button>
                    <div class="DropDownList" id="DDOthers">
                        <a href="NewProductHomePage.aspx" id="NPRTag2" runat="server">New Product Request</a>
                        <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" >Sign & Equip</a>
                        <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" >Inventory</a>
                        <a href="WClaim.aspx" id="WClaimTag2" runat="server" >Warranty</a>
                        <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server" >Event Budget</a>
                    </div>
                </div>
                <%--Setting & Map--%>
                <div class="DropDown">
                    <button type="button" class="DDBtn">
                        Setting
                        <i class="fa fa-caret-down"></i>
                    </button>
                    <div class="DropDownList" id="DDSetting">
                        <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                        <a href="Map3.aspx" id="MapTag2" runat="server">Map</a>
                    </div>
                </div>
                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <%--                <i class="fa fa-sign-out" aria-hidden="true" style="font-size:35px"></i>--%>
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                    <%--                    <img src="RESOURCES/LogoutIcon.png" /> Logout--%>
                    <%--                    <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                </a>
                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%--                                <img src="RESOURCES/menu.png" />--%>
                    <div class="container" onclick=" myFunction(this);">
                        <div class="bar1"></div>
                        <div class="bar2"></div>
                        <div class="bar3"></div>
                    </div>
                </a>
                

            </div>
        </div>

        <div id="btnhere">
            <center>
                <asp:Button ID="NewRequestForm" runat="server" CssClass="Buttton" Text="New Request Form" OnClick="DoNPRF"/>
                <asp:Button ID="MainMenu" runat="server" CssClass="Buttton" Text="Main Menu" OnClick="HomeMenu"/>
                <asp:Button ID="btnFindStatus" runat="server" CssClass="Buttton" Text="Waiting Approve" OnClick="btnFindStatus_Click" Visible="false" />
                <asp:Button ID="btnViewAll" runat="server" CssClass="Buttton" Text="View All" OnClick="btnViewAll_Click" Visible="false" />
                <asp:Button ID="btnAddUser" runat="server" CssClass="Buttton" Text="Add User" OnClientClick="showModal(); return false;" Visible="false" />
                <asp:Button ID="btnViewUser" runat="server" CssClass="Buttton" Text="View Approval User" OnClientClick="showModal(); return false;" />
            </center>
        </div>

        <center>
            <div id="gridview-container" style="width: 100%; overflow: auto;">
            <asp:GridView ID="GridView1" name="GridView1" class="gridview-container" runat="server" AutoGenerateColumns="true" CssClass="table" style="max-width: 80%; overflow: auto; max-height: 100%;" AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_RowDeleting" DataKeyNames="serialNo"> 
                <HeaderStyle CssClass="table-header" /> 
                <RowStyle CssClass="table-row" />
                <Columns>
                    <asp:TemplateField HeaderText="Actions"> 
                        <ItemTemplate> 
                            <asp:Button runat="server" CommandName="RequestForm" Text="⇒" CommandArgument='<%# Eval("serialNo") %>' CssClass="btn btn-info Button" OnCommand="GoToRequestForm" />
                        </ItemTemplate> 
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Eval("serialNo") %>' CssClass="btn btn-info Button" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="10" />
            </asp:GridView>
        </div>
        </center>

        <!-- The Modal -->
        <div id="formModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="hideModal()">&times;</span>
                <div id="formAddUser" runat="server">
                    <div>
                        <h2> Approval List </h2>
                    </div>
                    <table>
                        <tr>
                            <th class="modalth" > Checker: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="check1" runat="server" CssClass="textboxline autocomplete" placeholder="Checker1" />--%>
                                        <asp:DropDownList ID="ddlCheck1" runat="server" EnableViewState="true" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="check2" runat="server" CssClass="textboxline autocomplete" placeholder="Checker2" />--%>
                                        <asp:DropDownList ID="ddlCheck2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth"> QC: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
                                        <%--<asp:TextBox ID="qc1" runat="server" CssClass="textboxline autocomplete" placeholder="QC1" />--%>
                                         <asp:DropDownList ID="ddlQC1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="qc2" runat="server" CssClass="textboxline autocomplete" placeholder="QC2" />--%>
                                         <asp:DropDownList ID="ddlQC2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > Production: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="product1" runat="server" CssClass="textboxline autocomplete" placeholder="Production1" />--%>
                                         <asp:DropDownList ID="ddlProduct1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="product2" runat="server" CssClass="textboxline autocomplete" placeholder="Production2" />--%>
                                         <asp:DropDownList ID="ddlProduct2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > Purchasing: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="purchase1" runat="server" CssClass="textboxline autocomplete" placeholder="Purchasing1" />--%>
                                         <asp:DropDownList ID="ddlPurchase1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="purchase2" runat="server" CssClass="textboxline autocomplete" placeholder="Purchasing2" />--%>
                                         <asp:DropDownList ID="ddlPurchase2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > Warehouse: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="ware1" runat="server" CssClass="textboxline autocomplete" placeholder="Warehouse1" />--%>
                                         <asp:DropDownList ID="ddlWare1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="ware2" runat="server" CssClass="textboxline autocomplete" placeholder="Warehouse2" />--%>
                                         <asp:DropDownList ID="ddlWare2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > A & P (Marketing): </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="anp1" runat="server" CssClass="textboxline autocomplete" placeholder="A&P1" />--%>
                                         <asp:DropDownList ID="ddlMarket1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="anp2" runat="server" CssClass="textboxline autocomplete" placeholder="A&P2" />--%>
                                         <asp:DropDownList ID="ddlMarket2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > IT: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="it1" runat="server" CssClass="textboxline autocomplete" placeholder="IT1" />--%>
                                         <asp:DropDownList ID="ddlIT1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="it2" runat="server" CssClass="textboxline autocomplete" placeholder="IT2" />--%>
                                         <asp:DropDownList ID="ddlIT2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > Approved By: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="approve1" runat="server" CssClass="textboxline autocomplete" placeholder="Approve1" />--%>
                                         <asp:DropDownList ID="ddlApprove1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="approve2" runat="server" CssClass="textboxline autocomplete" placeholder="Approve2" />--%>
                                         <asp:DropDownList ID="ddlApprove2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <th class="modalth" > Admin: </th>
                            <td colspan="4">
                                <div class="flex-container">
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="admin1" runat="server" CssClass="textboxline autocomplete" placeholder="Admin1" />--%>
                                         <asp:DropDownList ID="ddlAdmin1" runat="server" CssClass="dropdownlist" />
                                    </div>
                                    <div class="flex-item">,</div>
                                    <div class="flex-item">
<%--                                        <asp:TextBox ID="admin2" runat="server" CssClass="textboxline autocomplete" placeholder="Admin2" />--%>
                                         <asp:DropDownList ID="ddlAdmin2" runat="server" CssClass="dropdownlist" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <div id="btnnn">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="hideModal();" Visible="false" />
                    </div>
                </div>
            </div>
        </div>   

        
    </form>

    <br /><br /><br /><br /><br /><br /><br />
   <script src="scripts/top_navigation.js"></script>
</body>
</html>
