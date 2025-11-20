<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewCustomer.aspx.cs" Inherits="DotNet.WebForm2" %>

<!DOCTYPE html> 
<html xmlns="http://www.w3.org/1999/xhtml"> 
    <head runat="server"> 
        <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

        <script src="scripts/GoToTab.js"></script>
        <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
        <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
        <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.0/css/bootstrap.min.css" />
        <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/dataTables.bootstrap5.min.css" />
        <link href="STYLES/New_Customer.css" rel="stylesheet" />
        <link href="STYLES/Sidebar.css" rel="stylesheet" />

        <title>New Customer</title>
        <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
        <meta name="apple-mobile-web-app-capable" content="yes" />
        <meta name="mobile-web-app-capable" content="yes" />

        <script src="scripts/BrowserReload_ThroughHistory.js"></script>
        
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script> 
        <script src="https://cdn.datatables.net/1.11.6/js/jquery.dataTables.min.js"></script>

        <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.css" />
        <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.js"></script> 
    </head> 
    <body>
        <div class="header1"> 
            <header> 
                <div class="row"> 
                    <div class="mobile_hidden"> 
                        <div class="col-3 col-s-3 image_icon"> 
                            <img src="RESOURCES/POSIM PETROLEUM MARKETING.png" class="image_newCust" /> 
                        </div>
                        <div class="col-9 col-s-9 image_title" position: fixed;>
                            <h1>New Customer</h1>
                        </div>
                    </div>
                </div>
         
                <!--==============================================================================-->
                <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
                
                <div class="topnav" id="myTopnav">
                    <a href="MainMenu.aspx">Home</a>  
                    <%--CUSTOMER--%>
                    <div class="DropDown">
                        <button type="button" class="DDBtn">
                            Customer
                            <i class="fa fa-caret-down"></i>
                        </button>
                        <div class="DropDownList">
                            <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" >Customer</a>
                            <a href="NewCustomer.aspx" class="active" id="NewCustomerTag2" runat="server">New Customer</a>
                        </div>
                    </div>
                    <%--SALES--%>
                    <div class="DropDown">
                        <button type="button" class="DDBtn">
                            Sales
                            <i class="fa fa-caret-down"></i>
                        </button>
                        <div class="DropDownList">
                            <a href="SFA.aspx" id="SFATag2" runat="server">Sales</a>
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
                        <div class="DropDownList">
                           
                            <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" >Sign & Equip</a>
                            <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" >Inventory</a>
                            <a href="WClaim.aspx" id="WClaimTag2" runat="server">Warranty</a>
                            <a href="EventBudget.aspx" id="EventBudgetTag2" runat="server">Event Budget</a>
                        </div>
                    </div>
                    <%--Setting & Map--%>
                    <div class="DropDown">
                        <button type="button" class="DDBtn">
                            Setting
                            <i class="fa fa-caret-down"></i>
                        </button>
                        <div class="DropDownList">
                            <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                            <a href="Map3.aspx" id="MapTag2" runat="server">Map</a>
                        </div>
                    </div>
                    <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                        <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                        <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                        <%--                        <img src="RESOURCES/LogOut.png" />
                            <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                    </a>

                    <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%-- <img src="RESOURCES/menu.png" />--%> 
                       <div class="container" onclick=" myFunction(this);"> 
                           <div class="bar1"></div> 
                           <div class="bar2"></div> 
                           <div class="bar3"></div> 
                       </div> 
                    </a> 
                    <script src="scripts/top_navigation.js"></script> 

                </div>


            </header> 

        </div>

        <form id="form1" runat="server">

            <div class="sidebar" id="sidebar" onclick="openSidebar()">
                <div class="sidebarTool" >
                    <h3 class="sidebarResponsive" id="sideResponsive" ><center>Tools</center></h3>
                </div>
                <div class="sidebarContent">
                   <h3>Applicant</h3>
                   <span>View Draft</span> 
                   <asp:RadioButton ID="RButton1" runat="server" CssClass="button" GroupName="sidebar" AutoPostBack="true" OnCheckedChanged="RButton1_CheckedChanged" /><br/>
                   <span>View All</span>
                   <asp:RadioButton ID="RButton2" runat="server" CssClass="button" GroupName="sidebar" AutoPostBack="true" OnCheckedChanged="RButton2_CheckedChanged" /><br/>
                   <span>Outs. No Acc. Open</span>
                   <asp:RadioButton ID="RButton3" runat="server"   CssClass="button"  GroupName="sidebar" /><br/>
                   <span>View OutStanding</span>
                   <asp:RadioButton ID="RButton4" runat="server"   CssClass="button"  GroupName="sidebar" AutoPostBack="true" OnCheckedChanged="RButton4_CheckedChanged" /><br/>
                   <span>Not Submitted</span>
                   <asp:RadioButton ID="RButton5" runat="server"   CssClass="button"  GroupName="sidebar" AutoPostBack="true" /><br/>

                   <h3>Credit Control</h3>
                   <span>View Draft</span>
                   <asp:RadioButton ID="RButton6" runat="server"   CssClass="button"  GroupName="sidebar" AutoPostBack="true" OnCheckedChanged="RButton1_CheckedChanged" /><br/>
                   <span>View All</span>
                   <asp:RadioButton ID="RButton7" runat="server"   CssClass="button"  GroupName="sidebar" AutoPostBack="true" OnCheckedChanged="RButton2_CheckedChanged" /><br/>
                   <span>View OutStanding</span>
                   <asp:RadioButton ID="RButton8" runat="server"   CssClass="button"  GroupName="sidebar" AutoPostBack="true" OnCheckedChanged="RButton4_CheckedChanged" /><br/>
                   <span>Not Submitted</span>
                   <asp:RadioButton ID="RButton9" runat="server"   CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Awaiting Documents</span>
                   <asp:RadioButton ID="RButton10" runat="server"  CssClass="button"  GroupName="sidebar"/><br/> <%--RButtonAwait_CheckedChanged--%>
                   <span>Document Received</span>
                   <asp:RadioButton ID="RButton11" runat="server"  CssClass="button"  GroupName="sidebar"/><br/>
                   <span>View KIV</span>
                   <asp:RadioButton ID="RButton12" runat="server"  CssClass="button"  GroupName="sidebar"/><br/> <%--RButtonKIV_CheckedChanged--%>
                   <span>Branch Outstandung</span>
                   <asp:RadioButton ID="RButton13" runat="server"  CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Branch Completed</span>           
                   <asp:RadioButton ID="RButton14" runat="server"  CssClass="button"  GroupName="sidebar"/><br/>
                   <span>HQ Outstanding</span>
                   <asp:RadioButton ID="RButton15" runat="server"  CssClass="button"  GroupName="sidebar"/><br/>
                   <span>HQ Completed</span>
                   <asp:RadioButton ID="RButton16" runat="server"  CssClass="button"  GroupName="sidebar"/><br/>
                    
                   <h3>Completed</h3>
                   <span>Approved</span>
                   <asp:RadioButton ID="RButton17" runat="server" CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Rejected By HOD</span>
                   <asp:RadioButton ID="RButton18" runat="server" CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Rejected By CC</span>
                   <asp:RadioButton ID="RButton19" runat="server" CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Report Inquiry</span>
                   <asp:RadioButton ID="RButton20" runat="server" CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Monthly Report</span>
                   <asp:RadioButton ID="RButton21" runat="server" CssClass="button"  GroupName="sidebar"/><br/>
                   <span>Data Lookup</span>
                   <asp:RadioButton ID="RButton22" runat="server" CssClass="button"  GroupName="sidebar"/><br/>
               </div>
            </div>
   
            <!--==============================================================================-->
        
            <div class="col-12 col-s-12">
                <div class="function">
                    <div class="function">
                        <asp:Button ID="DraftCustomerGPS" runat="server"  Text="Draft Customer GPS" class="function" OnClick="DoGPS"/>
                        <asp:Button ID="AddExistingCustomer" runat="server"  Text="Add Existing Customer" class="function"  OnClick="DoExistCustomer"/>
                        <asp:Button ID="CompanyReport" runat="server" Text="Add Company Report" class="function" OnClick="DoCompanyReport" />
                        <asp:Button ID="AccApplication" runat="server" Text="Customer Account Application" class="function" OnClick="DoAccApplication" />
                    </div>
                </div>
            </div>

            <div id="gridview-container" style="width: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" class="gridview-container" runat="server" AutoGenerateColumns="true" CssClass="table" style="max-width: 80%; overflow: auto; max-height: 100%;" 
                    AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_RowDeleting" DataKeyNames="NewCustID"> 
                    <HeaderStyle CssClass="table-header" /> 
                    <RowStyle CssClass="table-row" />
                    <Columns>
                        <asp:TemplateField HeaderText="Actions"> 
                            <ItemTemplate> 
                                <asp:Button runat="server" CommandName="CompanyReport" Text="⇒" CommandArgument='<%# Eval("NewCustID") %>' CssClass="btn btn-info" OnCommand="GoToCompanyReport" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:Button runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%# Eval("NewCustID") %>' CssClass="btn btn-info" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="10" />
                </asp:GridView>
            </div>
        
        </form> 

        <script src="htps://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

        <script src="scripts/top_navigation.js" type="text/javascript"></script>

    </body> 

</html>

