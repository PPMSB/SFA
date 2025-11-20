<%@ Page Title="Generate Form" Language="C#" MasterPageFile="~/Campaign_Site.Master" AutoEventWireup="true" CodeBehind="Campaign_CreateNewOffer.aspx.cs" Inherits="DotNet.Campaign_CreateNewOffer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <link href="Content/form_custom.css" rel="stylesheet" />
    <link href="Content/custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.bundle.min.js"></script>

    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <script src="../scripts/moment.min.js"></script>
    <script src="../scripts/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript"></script>

    <style>
        #GridView1 td {
            font-size: 15px;
            padding-left: 5px;
            padding-right: 5px;
        }

        #GridView1 table, td, th {
            border: 1px solid;
        }
    </style>

    <form runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Panel runat="server" ID="CreateFormWrapper">
                    <div class="mt-4">
                        <div class="col-12" id="cusButton" runat="server">
                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">Campaign:      </label>
                                </div>

                                <div class="col-4 col-s-4">
                                    <div class="col-12 col-s-12">
                                        <asp:Label ID="ddlAvailableCampaign" class="gettext" runat="server" Text=" "></asp:Label>
                                        <%--<asp:DropDownList ID="ddlAvailableCampaign" runat="server" class="dropdownlist" Style="font-size: 12px" AutoPostBack="true" OnSelectedIndexChanged="ShowFindCustomer"></asp:DropDownList>--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Panel runat="server" ID="InformationWrapper" Visible="false">

                            <%--<asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find Customer" class="glow" />--%>

                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">Customer Account:      </label>
                                </div>

                                <div class="col-4 col-s-4">
                                    <asp:Label ID="txtCustomerAccount" class="gettext" runat="server" Text=" "></asp:Label>
                                </div>
                            </div>



                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">Workshop Name:      </label>
                                </div>

                                <div class="col-4 col-s-4">
                                    <asp:Label ID="txtCustName" class="inputtext" runat="server" Text=" "></asp:Label>
                                </div>
                            </div>

                            <asp:Label ID="txtSalesmanID" class="inputtext" runat="server" Text=" " Visible="false"></asp:Label>

                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">PPM Salesman:      </label>
                                </div>

                                <div class="col-4 col-s-4">
                                    <asp:Label ID="txtSalesman" class="inputtext" runat="server" Text=" "></asp:Label>
                                </div>
                            </div>

                            <div class="col-12 col-s-12" runat="server" id="DateWrapper" style="font-size: 11px; font-weight: bold" visible="false">
                                <div class="col-2 col-s-2">
                                    <asp:Label ID="Label1" runat="server" CssClass="labeltext" Text="Campaign Start Date: "></asp:Label>
                                </div>
                                <div class="col-3">
                                    <div class="">
                                        <div class="form-group">
                                            <div class="input-group date" id="txtFromDateTimePicker">
                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control datemonthpicker" placeholder="Select Date and Time" Style="font-size: 12px"></asp:TextBox>
                                                <span class="input-group-addon" style="display: flex; justify-content: center; align-items: center;">
                                                    <span class="glyphicon glyphicon-calendar glyph-month"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 col-s-12">
                                    <div class="col-2 col-s-6">
                                        <asp:Label ID="Label12" runat="server" Text="Target:" CssClass="labeltext"></asp:Label>
                                    </div>

                                    <div class="col-4 col-s-4">
                                        <asp:DropDownList ID="ddlTarget" runat="server" CssClass="dropdownlist" Style="font-size: 12px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div>
                                <p style="margin-left: 10px; font-size: 12px"><span style="color: red">*</span>This form is for printing purpose only, must return/upload chop sign form to confirm registration.</p>
                                <p style="margin-left: 10px; font-size: 12px"><span style="color: red">*</span>此表格仅供打印之用，必须返回/上传盖章签名表格以确认注册。</p>
                            </div>



                            <div class="col-12 col-s-12">
                                <asp:Button ID="BtnGenerate" runat="server" OnClick="GenerateDocument" Text="Register" class="glow_green button-margin" Enabled="true" Target="_blank" />
                            </div>
                        </asp:Panel>
                    </div>


                    <%--          <div class="col-3" style="display: none">
                        <div class="">
                            <div class="form-group">
                                <div class="input-group date" id="txtFromDateTimePicker">
                                    <asp:TextBox ID="txtMonthDate" runat="server" CssClass="form-control datemonthpicker" placeholder="Select Date and Time" Style="font-size: 12px"></asp:TextBox>
                                    <span class="input-group-addon" style="display: flex; justify-content: center; align-items: center;">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </asp:Panel>

                <asp:Panel runat="server" ID="CustomerMasterWrapper" Visible="false">
                    <div class="row">
                        <div class="col-6 col-s-12">
                            <div class="col-3 col-s-4">
                                <label class="labeltext">Search By:      </label>
                            </div>

                            <div class="col-3 col-s-8">
                                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" class="dropdownlist" Style="font-size: 12px">
                                    <asp:ListItem Text="Customer Name" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Account No." Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Participated VPPP" Value="2"></asp:ListItem>
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

                    <div style="display: flex; justify-content: center">
                        <div id="grdCharges" runat="server" style="max-width: 97%; overflow: auto; max-height: 100%;">
                            <asp:GridView ID="GridView1" runat="server" PagerStyle-CssClass="page-link"
                                PageSize="20" HorizontalAlign="Left" AllowCustomPaging="true"
                                CssClass="mydatagrid" AllowPaging="True" EmptyDataText="No records found."
                                OnPageIndexChanging="datagrid_PageIndexChanging" EnableSortingAndPagingCallbacks="true"
                                HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="No" HeaderText="No" />
                                    <asp:TemplateField HeaderText="Account">
                                        <ItemTemplate>
                                            <asp:Button ID="Button_Account" runat="server" OnClick="Button_Account_Click" CausesValidation="false" Style='<%# Eval("Participate VPPP").ToString() == "Yes" ? "": "text-decoration: underline" %>' Text='<%# Eval("Account") %>' class="button_grid" Enabled='<%# Eval("Participate VPPP").ToString() == "Yes" ? false : true %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Account" HeaderText="Account" />
                                    <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="Participate VPPP" HeaderText="Participate VPPP" />
                                    <asp:BoundField DataField="Campaign Start Date" HeaderText="Campaign Start Date" />

                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="State" HeaderText="State" />
                                    <asp:BoundField DataField="Employee ID" HeaderText="Employee ID" />
                                    <asp:BoundField DataField="LPPoint" HeaderText="LP Point" />
                                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                    <asp:BoundField DataField="TeleFax" HeaderText="TeleFax" />
                                    <asp:BoundField DataField="Cellular Phone" HeaderText="Cellular Phone" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                    <asp:BoundField DataField="Address" HeaderText="Address" />
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <PagerSettings PageButtonCount="2" />
                                <RowStyle CssClass="rows" />

                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

            </ContentTemplate>

        </asp:UpdatePanel>

    </form>

</asp:Content>
