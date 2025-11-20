<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Battery.aspx.cs" Inherits="DotNet.Battery" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

    <title>Battery / Item</title>
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
                    <img id="imgHeader" runat="server" src="RESOURCES/Battery.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 id="HeaderTitle" runat="server">Battery</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>

                <a href="LoginPage.aspx" class="Log_Out top_nav_logout_properties">
                    <%--                    <img src="RESOURCES/LogOut.png" />
                    <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
                    <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                    <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                </a>

                <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                    <%--                    <img src="RESOURCES/menu.png" />--%>
                    <div class="container" onclick=" myFunction(this);">
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="col-12 col-s-12">
                            <asp:Button ID="Accordion_AddBatchItem" runat="server" Text="Search (Can search with one or more than 1 field(s))" class="accordion_style_sub_fixed_lightGrey" />
                            <div class="col-6 col-s-12" runat="server">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Item Name:        </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_BatteryName" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-6 col-s-12" runat="server">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Item ID:        </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_BatteryID" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-6 col-s-12" runat="server">
                                <div class="col-3 col-s-4">
                                    <label class="labeltext">Serial No:        </label>
                                </div>
                                <div class="col-3 col-s-8">
                                    <asp:TextBox ID="TextBox_SerialNo" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton3" class="searchbtn" runat="server" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="CheckAcc" />
                                </div>
                            </div>
                        </div>

                        <%--                        <div class="col-12 col-s-12">                            
                             <div class="col-3 col-s-8">                              
                                <div class="image_properties">
                                    <asp:ImageButton ID="ImageButton1" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/Search.png" />
                                    <asp:ImageButton ID="ImageButton2" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/Search_h.png" OnClick="CheckAcc"/>                        
                                </div>
                             </div>
                        </div>--%>
                        <asp:Label ID="hidden_CustAcc" class="gettext" runat="server" Style="color: red;" Visible="false"></asp:Label>
                        <asp:Label ID="hidden_WarrantyType" class="gettext" runat="server" Style="color: red;" Visible="false"></asp:Label>
                            <asp:Label ID="hidden_Label_PreviousStatus" class="gettext" runat="server" Visible="false"></asp:Label>
<asp:Label ID="hidden_LabelClaimNumber" class="gettext" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-12 col-s-12">
                <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                    <ProgressTemplate>
                        <div class="loading">

                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="grdCharges" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                            <asp:GridView ID="GridView1" runat="server"
                                PageSize="20" HorizontalAlign="Left"
                                CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="True"
                                OnPageIndexChanging="datagrid_PageIndexChanging" AllowCustomPaging="True"
                                HtmlEncode="False" EmptyDataText="No records found."
                                Style="overflow: hidden;"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="No" HeaderText="No" />

                                    <asp:TemplateField HeaderText="Serial No">
                                        <ItemTemplate>
                                            <asp:Button ID="Button_SerialNo" runat="server" OnClick="Button_SerialNo_Click" CausesValidation="false" Text='<%# Eval("SerialNo") %>' class="button_grid" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- Jerry 20251119 - Add batch number --%>
                                    <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" />
                                    <%-- Jerry 20251119 End --%>
                                    <asp:BoundField DataField="InvoiceId" HeaderText="Invoice Id" />
                                    <asp:BoundField DataField="CustomerAcc" HeaderText="Customer Acc" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="ItemId" HeaderText="Item Id" />
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                    <asp:BoundField DataField="inventLocationId" HeaderText="inventLocationId" />

                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <PagerSettings PageButtonCount="2" />
                                <PagerStyle CssClass="pager" />
                                <RowStyle CssClass="rows" />

                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div id="Div1" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                            <asp:GridView ID="gvOil" runat="server"
                                PageSize="20" HorizontalAlign="Left"
                                CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="True"
                                OnPageIndexChanging="gvOil_PageIndexChanging" AllowCustomPaging="True"
                                HtmlEncode="False" EmptyDataText="No records found."
                                Style="overflow: hidden;"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="No" HeaderText="No" />

                                    <asp:TemplateField HeaderText="Item ID">
                                        <ItemTemplate>
                                            <asp:Button ID="Button_ItemId" runat="server" OnClick="Button_ItemId_Click" CausesValidation="false" Text='<%# Eval("ItemId") %>' class="button_grid" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                                    <asp:BoundField DataField="Unit" HeaderText="Unit" />

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
    </form>
</body>
</html>
