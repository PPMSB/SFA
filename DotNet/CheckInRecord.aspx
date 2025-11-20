<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckInRecord.aspx.cs" Inherits="DotNet.CheckInRecord" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico"/>

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
   
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"/>
    
    <title>Check-In Record</title>
     <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0"/>  
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="mobile-web-app-capable" content="yes"/>

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
</head>


<body>
    <form id="form1" runat="server">
 
    <div class="row">
<div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                     <img src="RESOURCES/CHECKINN.png" class="image_resize"/> 
                </div>
                
                <div class="col-9 col-s-9 image_title">
                    <h1>Check-In Record</h1>  
                </div>
</div>   
            <!--==============================================================================-->  
                <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />      
                <div class="topnav" id="myTopnav">
                  <a href="MainMenu.aspx">Home</a>
                  <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible= "false">Customer</a>
                  <a href="SFA.aspx" id="SFATag2" runat="server" visible= "false">Sales</a>
                  <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                  <a href="Payment.aspx" id="PaymentTag2" runat="server" visible= "false">Payment</a>
                  <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible= "false">Redemption</a>
                  <%--<a href="EOR.aspx" id="EORTag2" runat="server" visible= "false">EOR</a>--%>
                    <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
                  <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible= "false">Inventory</a>
                  <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
                  <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties" >
                         <asp:Label runat="server" CssClass="fa fa-sign-out" style="font-size:20px;" ></asp:Label>
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
              <asp:ScriptManager ID="ScriptManager1" runat="server"/>

                <div class="col-12 col-s-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate> 
                        <div class="col-6 col-s-12">
                             <div class="col-3 col-s-4">
                                 <label class="labeltext">Search By:      </label>
                             </div>
                          
                            <div class="col-3 col-s-8">
                            
                                <asp:DropDownList ID="DropDownList1" runat="server" autopostback="true" class="inputtext">
                                    <asp:listitem text="Name" value="0"></asp:listitem> 
                                    <asp:listitem text="QRLocation" value="1"></asp:listitem>
                                 
                                </asp:DropDownList>
                            
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                             <div class="col-3 col-s-4">
                                 <label class="labeltext">Search:      </label>
                             </div>
                             <div class="col-3 col-s-8">
                              
                                 <asp:TextBox ID="TextBox_SearchItem" class="inputtext" runat="server"></asp:TextBox>      
                                    <asp:ImageButton ID="ImageButton2" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="CheckItem"/>
                             </div>
                        </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                </div>

              <div class="col-12 col-s-12">

                   <asp:UpdateProgress runat="server" id="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="UpdatePanel2">
                    <ProgressTemplate>
                       <div class="loading">
                            
                             <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div> 
                        
                        
                    </ProgressTemplate>
                    </asp:UpdateProgress>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate> 
                    <div id="grdCharges" runat="server" style="max-width:110%; overflow: auto; max-height: 100%;">
                    <asp:GridView ID="GridView1" runat="server" 
                        PageSize="20"  HorizontalAlign="Left" 
                        CssClass="mydatagrid" PagerStyle-CssClass="pager"
                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows" 
                        onrowdatabound="GridView_RowDataBound_Header"
                        HtmlEncode="False" 
                         style=" -space: nowrap;overflow: hidden;"
                        
                        >
                       
                    </asp:GridView>
                    </div>




                    </ContentTemplate>
                    </asp:UpdatePanel>
              </div>








        </div>
        <script src="scripts/ButtonUp.js"></script>
    </form>
      
</body>
</html>

