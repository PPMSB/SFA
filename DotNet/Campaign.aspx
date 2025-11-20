<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Campaign.aspx.cs" Inherits="DotNet.Campaign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <script src="scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />
    <link rel="stylesheet" href="lhdn/scripts/bootstrap.min.js" />
    <!-- Bootstrap JS and jQuery (required for Bootstrap's JavaScript plugins) -->
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="lhdn/scripts/bootstrap.min.js"></script>
    <!-- Bootstrap Datepicker CSS -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <!-- Bootstrap Datepicker JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.2.3/css/responsive.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/fixedheader/3.1.2/css/fixedHeader.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/select/1.3.1/css/select.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/rowreorder/1.2.8/css/rowReorder.dataTables.min.css" />

    <style>
        .gallery img {
            margin: 5px;
            width: 100px;
            height: auto;
            border: 1px solid #ddd;
            border-radius: 5px;
        }
    </style>
    <title></title>
</head>
<script type="text/javascript">

    function previewMultiple(event) {
        var files = event.target.files;
        if (files.length != 0) {
            document.getElementById("gallery").innerHTML = "";
            for (var i = 0; i < files.length; i++) {
                var urls = URL.createObjectURL(files[i]);
                document.getElementById("gallery").innerHTML += '<img src="' + urls + '" class="img-thumbnail m-1" style="width: 100px; height: auto;">';
            }
        } else {
            document.getElementById("fuDocument").value = null;
            document.getElementById("gallery").innerHTML = "";
        }
    }

    $(function () {

        // Open Popup
        $('[popup-open]').on('click', function () {
            var popup_name = $(this).attr('popup-open');
            $('[popup-name="' + popup_name + '"]').fadeIn(300);
        });

        // Close Popup
        $('[popup-close]').on('click', function () {
            var popup_name = $(this).attr('popup-close');
            $('[popup-name="' + popup_name + '"]').fadeOut(300);
        });

        // Close Popup When Click Outside
        $('.popup').on('click', function () {
            var popup_name = $(this).find('[popup-close]').attr('popup-close');
            $('[popup-name="' + popup_name + '"]').fadeOut(300);
        }).children().click(function () {
            return false;
        });
    });

    function onSalesmanChange() {
        // Get the selected value from the DropDownList
        var selectedValue = document.getElementById("DropDownList_Salesman").value;

        // Display it in the console or perform any desired logic
        console.log("Selected Salesman Id: " + selectedValue);

        // Optionally set the value to a HiddenField if you need it later on the server
        document.getElementById('<%= hdEmplId.ClientID %>').value = selectedValue;
    }

    function storeSelectedValue() {
        var selectedValue = document.getElementById('<%= DropDownList_Salesman.ClientID %>').value;
        // Use sessionStorage or localStorage
        sessionStorage.setItem('selectedSalesmanId', selectedValue);
    }

    window.onload = function () {
        var selectedValue = sessionStorage.getItem('selectedSalesmanId');
        var dropdown = document.getElementById('<%= DropDownList_Salesman.ClientID %>');
        if (selectedValue) {
            dropdown.value = selectedValue; // Set the selected value
        }
    }
</script>

<body>
    <form id="form1" runat="server">
        <div class="container1">
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav print-hide" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
                <a href="Redemption.aspx" id="RedemptionTag2" class="active" runat="server" visible="false">Redemption</a>
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
                <a href="Setting.aspx" id="SettingTag2" runat="server">Setting</a>
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

            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <div class="col-12 col-s-12">
                <asp:Button ID="Button_NewCampaign_section" runat="server" OnClick="Button_NewCampaign_section_Click" Text="New" class="frame_style_4bar" />
                <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                <asp:Button ID="Button_CampaignList_section" runat="server" OnClick="Button_CampaignList_section_Click" Text="Overview" class="frame_style_4bar" />
            </div>

            <div class="col-12 col-s-12" runat="server" id="dvNew">
                <div class="col-6 col-s-12">
                    <div class="col-4 col-s-8">
                        <label for="DropDownList_Salesman" class="labeltext">Salesman Id:</label>
                    </div>

                    <div class="col-6 col-s-8">
                        <asp:DropDownList ID="DropDownList_Salesman" runat="server" class="form-control" aria-label="Salesman Id" onchange="onSalesmanChange();" Width="200px"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-6 col-s-12">
                    <div class="col-4 col-s-8">
                        <label id="lblStat" class="labeltext">Status:</label>
                    </div>

                    <div class="col-4 col-s-8">
                        <asp:Label ID="lblStatus" runat="server" Text="New" CssClass="labeltext"></asp:Label>
                    </div>
                </div>

                <div class="col-12 col-s-12">
                    <div class="col-2 col-s-4">
                        <label id="lblCustAcc" class="labeltext">Customer Account:</label>
                    </div>
                    <div class="col-2 col-s-4">
                        <asp:TextBox ID="TextboxCustAcc" runat="server" />
                    </div>

                    <div class="col-2 col-s-4">
                        <asp:Button ID="Button_FindList" runat="server" CausesValidation="false" OnClientClick="storeSelectedValue();"
                            Text="Search" class="btn btn-primary" OnClick="Button_FindList_Click" />
                        <asp:HiddenField ID="hdEmplId" runat="server" />
                    </div>

                    <div class="col-2 col-s-4">
                        <label id="lbl_from_date" class="labeltext">Start Date:</label>
                    </div>
                    <div class="col-2 col-s-4">
                        <input type="date" id="start_date" runat="server" name="trip-start" class="form-control" />
                    </div>
                </div>

                <div class="col-6 col-s-12" id="dvUpload" runat="server">
                    <div class="col-4">
                        <asp:Label runat="server" ID="lblDisplay" class="labeltext" Text="Attachment:"></asp:Label>
                    </div>
                    <div class="col-4">
                        <asp:FileUpload ID="fuDocument" runat="server" AllowMultiple="true" data-toggle="tooltip" title="Upload multiple files (jpg, png, jpeg, tiff, pdf)" />
                        <asp:RegularExpressionValidator ID="revFileUpoad" runat="server" ForeColor="Red" ControlToValidate="fuDocument" Display="Dynamic"
                            ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$"
                            ErrorMessage="Only jpg, png, jpeg, tiff, pdf are allowed!"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="col-6 col-s-12 print-hide" id="display_section" runat="server" visible="false">
                    <div class="col-4 col-s-4">
                        <asp:Label ID="lblDoc" runat="server" Text="Document:" CssClass="labeltext"></asp:Label>
                    </div>
                    <input type="file" runat="server" onchange="previewMultiple(event);" name="fileUpload" id="fileUpload" />
                    <div id="gallery" class="gallery mt-2"></div>
                    <a class="open-button" runat="server" id="btnDisplay" popup-open="popup-1" href="javascript:void(0)">Display</a>
                    <div class="popup" popup-name="popup-1">
                        <div class="popup-content">
                            <div class="img-container">
                                <br />
                                <asp:Repeater ID="repeater" runat="server">
                                    <ItemTemplate>
                                        <img id="imgProd" runat="server" class="demo cursor" src='<%# Eval("Value") %>' onclick='srcSize(this)' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <a class="close-button" popup-close="popup-1" href="javascript:void(0)"></a>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="btnSubmit" runat="server" class="btn btn-success" Text="Submit" OnClick="btnSubmit_Click" />
                        <%--                        <span id="loadingSpinner" class="spinner-border" role="status" style="display: none;"></span>--%>
                        <asp:Button ID="btnCancel" runat="server" class="btn btn-secondary" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>

            <div class="col-12 col-s-12" runat="server" id="dvList">
                <div class="col-6 col-s-12">
                    <asp:GridView runat="server" ID="gvCampaign" HorizontalAlign="left" AutoGenerateColumns="false" OnRowCommand="gvCampaign_RowCommand"
                        CssClass="mydatagrid" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                        HtmlEncode="False" Style="white-space: nowrap; overflow: hidden;">
                        <Columns>
                            <asp:TemplateField HeaderText="No.">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Account">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCustomerAccount" runat="server" CommandName="SelectAccount"
                                        Text='<%# Eval("Customer Account") %>' CommandArgument='<%# Eval("Customer Account") + "|" + 
                                            Eval("Employee ID") + "|" + Eval("Start Date") + "|" + Eval("Status") + "|" + Eval("Attachment") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Employee ID" HeaderText="Employee ID" />
                            <asp:BoundField DataField="Start Date" HeaderText="Start Date" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="Attachment" HeaderText="Attachment" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
<script src="https://code.jquery.com/jquery-3.5.1.js" integrity="sha256-QWo7LDvxbWT2tbbQ97B53yJnYU3WhH/C8ycbRAkjPDc=" crossorigin="anonymous"></script>
<script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
<script>  
    function srcSize(element) {
        var src = element.src;
        setTimeout(function () {
            window.open(src, '_blank');
        }, 500);
        return false;
    }

    var table = $('#gvCampaign').DataTable({
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
