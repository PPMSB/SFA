<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WClaim.aspx.cs" Inherits="DotNet.WClaim" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <%--    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />--%>
    <%--    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>--%>
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>

    <title>Warranty Claim</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <style type="text/css">
        @media print {
            body {
                width: 210mm;
                height: 297mm;
                margin: 0;
                padding: 0;
            }

            .print-hide {
                display: none;
            }

            .print-show {
                display: block;
            }

            .container {
                display: block; /* Change to a block layout for print */
            }

            .col-6, .col-7, .col-8, .col-3, .col-4 {
                width: 50%;
            }

            .col-p-6 {
                width: 100%;
            }
            /* Additional styles for proper printing layout */
        }

        .bordered-container {
            border: 2px solid #007bff; /* Sets a solid border around the container */
            padding: 20px; /* Adds padding inside the border */
            margin: 20px 0; /* Adds margin to separate it from other elements */
            border-radius: 8px; /* Optional: rounds the corners of the border */
            background-color: #f9f9f9; /* Optional: adds a light background color */
        }

        .txt_reason{
            height:auto
        }
    </style>
</head>
<script type="text/javascript"> 
    window.addEventListener('popstate', function (event) {
        event.preventDefault();
        history.forward();
    });

    function previewMultiple(event) {
        var img = document.getElementById("fileUpload");
        if (img.files.length != 0) {
            var urls = URL.createObjectURL(event.target.files[0]);
            document.getElementById("gallery").innerHTML = "";
            document.getElementById("gallery").innerHTML += '<img src="' + urls + '">';
        } else {
            document.getElementById("fileUpload").value = null;
            document.getElementById("gallery").innerHTML = "";
        }
    }

    function previewMultiple2(event) {
        var img = document.getElementById("fileUpload2");
        if (img.files.length != 0) {
            var urls = URL.createObjectURL(event.target.files[0]);
            document.getElementById("gallery2").innerHTML = "";
            document.getElementById("gallery2").innerHTML += '<img src="' + urls + '">';
        } else {
            document.getElementById("fileUpload2").value = null;
            document.getElementById("gallery2").innerHTML = "";
        }
    }

    function previewMultiple3(event) {
        var img = document.getElementById("fileUpload3");
        if (img.files.length != 0) {
            var urls = URL.createObjectURL(event.target.files[0]);
            document.getElementById("gallery3").innerHTML = "";
            document.getElementById("gallery3").innerHTML += '<img src="' + urls + '">';
        } else {
            document.getElementById("fileUpload3").value = null;
            document.getElementById("gallery3").innerHTML = "";
        }
    }

    function previewMultiple4(event) {
        var img = document.getElementById("fileUpload4");
        if (img.files.length != 0) {
            var urls = URL.createObjectURL(event.target.files[0]);
            document.getElementById("gallery4").innerHTML = "";
            document.getElementById("gallery4").innerHTML += '<img src="' + urls + '">';
        } else {
            document.getElementById("fileUpload4").value = null;
            document.getElementById("gallery4").innerHTML = "";
        }
    }

    function upload(input) {
        var totalFileSize = 0;
        var tenMB = 10 * 1024 * 1024; // 10 MB in bytes
        // Check if files are selected
        if (input.files.length > 0) {
            for (var i = 0; i < input.files.length; i++) {
                totalFileSize += input.files[i].size; // Accumulate total file size
            }
            // Check if total file size exceeds 10 MB
            if (totalFileSize > tenMB) {
                alert("Total file size exceeds 10MB!");
                document.getElementById("fileUpload").value = null; // Clear the input
                document.getElementById("gallery").innerHTML = ''; // Clear the preview if needed
            }
        }
    }

    function previewInspect(event) {
        var img = document.getElementById("InspectFile");
        if (img.files.length != 0) {
            var urls = URL.createObjectURL(event.target.files[0]);
            document.getElementById("galleryInspect").innerHTML = "";
            document.getElementById("galleryInspect").innerHTML += '<img src="' + urls + '">';
        } else {
            document.getElementById("InspectFile").value = null;
            document.getElementById("galleryInspect").innerHTML = "";
        }
    }

    //function upload(input) {
    //    var fileSize = input.files[0].size; // in bytes
    //    var maxSize = 10 * 1024 * 1024; // 10 MB (in bytes)
    //    var fileUploadId = input.id;
    //    if (fileSize > maxSize) {
    //        alert("File size exceeds 10MB!");
    //        input.value = null; // Clear the current file input

    //        if (fileUploadId === "fileUpload") {
    //            document.getElementById("fileUpload").value = null;
    //            document.getElementById("gallery").innerHTML = "";
    //        }
    //        if (fileUploadId === "fileUpload2") {
    //            document.getElementById("fileUpload2").value = null;
    //            document.getElementById("gallery2").innerHTML = "";
    //        }
    //        if (fileUploadId === "fileUpload3") {
    //            document.getElementById("fileUpload3").value = null;
    //            document.getElementById("gallery3").innerHTML = "";
    //        }
    //    }
    //}

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

    function gv_Search(strKey) {
        var strData = strKey.value.toLowerCase().split(" ");
        var tblData = document.getElementById("<%=GridViewOverviewList.ClientID %>");
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

    function gv_Search(strKey) {
        var strData = strKey.value.toLowerCase().split(" ");
        var tblData = document.getElementById("<%=gvBatteryReport.ClientID %>");
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

    function initializeDatepicker() {
        $('#txtFromDate').datepicker({
            format: "dd/mm/yyyy"
        }).datepicker("setDate", 'now');

        $('#txtToDate').datepicker({
            format: "dd/mm/yyyy"
        }).datepicker("setDate", 'now');

        $('#txtCollectionDt').datepicker({
            format: "dd/mm/yyyy"
        }).datepicker("setDate", 'now')
    }

    window.onload = function () {
        initializeDatepicker();
    }

<%--    function validateForm(event) {
        var ddlClaimType = document.getElementById("<%= ddlClaimType.ClientID %>");
        var gridView = document.getElementById("<%= GridView_BatchItem.ClientID %>");
        var textboxes = gridView.querySelectorAll("input[type='text']");
        var errorMessageDiv = document.getElementById("errorMessage");

        if (ddlClaimType && ddlClaimType.value === "3") {
            for (var i = 0; i < textboxes.length; i++) {
                var textbox = textboxes[i];
                if (textbox.value.trim() === "") {
                    errorMessageDiv.innerText = "Item in the batch item details cannot be empty.";
                    event.preventDefault(); // Prevent the default form submission
                    return false;
                }
            }
        }

        // All validation passed, hide the error message and continue with your logic
        errorMessageDiv.innerText = "";
        return true;
    }--%>

    function openModal() {
        var modal = document.getElementById("myModal");
        modal.style.display = "block";
        return false;
    }

    function closeModal() {
        var modal = document.getElementById("myModal");
        modal.style.display = "none";
    }

    function submitReason() {
        var rejectReason = document.getElementById("rejectReason").value;
        document.getElementById("hfRejectReason").value = rejectReason;
        closeModal();
        return true; // Prevent postback
    }

    $(document).ready(function () {
        // Attach a click event handler to the button
        $('#<%= Button_Submit.ClientID %>').click(function () {
            // Show the UpdateProgress control
            $('#<%= UpdateProgress.ClientID %>').show();
        });

        $('#<%= Button_Approve.ClientID %>').click(function () {
            $('#<%= UpdateProgress.ClientID %>').show();
        });

        $('#<%= Button_CreateReturn.ClientID %>').click(function () {
            $('#<%= UpdateProgress.ClientID %>').show();
        });

        // Attach a click event listener to the radio button
        $('#rblWarranty').on("click", function () {
            // Call the function to initialize the datepicker
            initializeDatepicker();
        });

        $('#ddlClaimType').on("click", function () {
            // Call the function to initialize the datepicker
            initializeDatepicker();
        });

        $('#Button_Delivery_Addr').on("click", function () {
            initializeDatepicker();
        });
    });

    function updateCharacterCount(textBox, warningSpanId) {
        var warningSpan = document.getElementById(warningSpanId);
        var maxCharacterCount = 80; // Set your desired character limit
        var remainingCharacters = maxCharacterCount - textBox.value.length;

        if (remainingCharacters < 0) {
            //warningSpan.textContent = 'Exceeded by ' + Math.abs(remainingCharacters) + ' characters!';
            textBox.value = textBox.value.substring(0, maxCharacterCount); // Truncate input
        } else {
            warningSpan.textContent = remainingCharacters + ' characters remaining';
        }
    }

    function DOCharacterCount(textBox, warningSpanId) {
        var warningSpan = document.getElementById(warningSpanId);
        var maxCharacterCount = 20; // Set your desired character limit
        var remainingCharacters = maxCharacterCount - textBox.value.length;

        if (remainingCharacters < 0) {
            //warningSpan.textContent = 'Exceeded by ' + Math.abs(remainingCharacters) + ' characters!';
            textBox.value = textBox.value.substring(0, maxCharacterCount); // Truncate input
        } else {
            warningSpan.textContent = remainingCharacters + ' characters remaining';
        }
    }
</script>

<body onload="initializeDatepicker()">

    <form id="form1" runat="server" enctype="multipart/form-data">
        <%--onsubmit="return validateForm(event);"--%>
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/WARRANTY CLAIM.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 style="font-weight: bold">Warranty Claim</h1>
                </div>
            </div>
            <!--==============================================================================-->
            <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
            <div class="topnav print-hide" id="myTopnav">
                <a href="MainMenu.aspx">Home</a>
                <a href="CustomerMaster.aspx" id="CustomerMasterTag2" runat="server" visible="false">Customer</a>
                <a href="SFA.aspx" id="SFATag2" runat="server" visible="false">Sales</a>
                <a href="SalesQuotation.aspx" id="SalesQuotation2" runat="server" visible="false">Quotation</a>
                <a href="Payment.aspx" id="PaymentTag2" runat="server" visible="false">Payment</a>
                <a href="Redemption.aspx" id="RedemptionTag2" runat="server" visible="false">Redemption</a>
                <%--<a href="EOR.aspx" id="EORTag2" runat="server" visible="false">EOR</a>--%>
                <a href="SignboardEquipment.aspx" id="EORTag2" runat="server" visible="true">Sign & Equip</a>
                <a href="InventoryMaster.aspx" id="InventoryMasterTag2" runat="server" visible="false">Inventory</a>
                <a href="WClaim.aspx" id="WClaimTag2" class="active" runat="server" visible="false">Warranty</a>
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
            <div class="col-12 col-s-12 print-hide">
                <asp:Button ID="Button_new_applicant_section" runat="server" OnClick="Button_new_applicant_section_Click" Text="New" class="frame_style_4bar_Warranty" CausesValidation="False" />
                <img id="img1" runat="server" src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                <asp:Button ID="btnOverview" runat="server" OnClick="btnOverview_Click" Text="Overview" class="frame_style_4bar_Warranty" CausesValidation="False" />
                <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                <%--<asp:Button ID="Button_reserve_section" runat="server" Text="Reserve1" class="frame_style_4bar_Warranty" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                                        <asp:Button ID="Button_Rejected" runat="server" OnClick="Button_Rejected_Click" Visible="false" Text="Rejected" class="frame_style_4bar_Warranty" />--%>
                <asp:Button ID="Button_report_section" runat="server" OnClick="Button_report_section_Click" Text="Report" class="frame_style_4bar_Warranty" CausesValidation="False" />
                <img id="img2" runat="server" src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" visible="false" />
                <asp:Button ID="Button_enquiries_section" runat="server" OnClick="Button_enquiries_section_Click" Text="Enquiries" class="frame_style_4bar_Warranty" CausesValidation="False" />
            </div>
            <!--new_applicant_section////////////////////////////////////////////////////////////////////////////////////-->

            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="upBatch">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div id="new_applicant_section" style="display: none" runat="server">
                <div class="col-12 col-s-12 print-hide">
                    <asp:Button ID="Button_Submit" runat="server" OnClick="Button_Submit_Click" Text="Submit" class="frame_style_type2" ValidationGroup="submit" />
                    <asp:Button ID="Button_SaveDraft" runat="server" OnClick="Button_SaveDraft_Click" Text="Save As Draft" class="frame_style_type2" Visible="false" ValidationGroup="submit" />
                    <asp:Button ID="Button_Approve" runat="server" OnClick="Button_Approve_Click" Text="Approve" class="frame_style_type2" Visible="false" CausesValidation="false" />
                    <asp:Button ID="Button_Reject" runat="server" Text="Reject" class="frame_style_type2" OnClientClick="return openModal();" CausesValidation="false" />
                    <asp:Button ID="btnAmend" runat="server" Text="Amend" OnClick="btnAmend_Click" CssClass="frame_style_type2" Visible="false" CausesValidation="false" />
                    <asp:Button ID="Button_CreateReturn" runat="server" OnClick="Button_CreateReturn_Click" Text="Create Return" class="frame_style_type2" Visible="false" CausesValidation="false" />
                    <asp:Button ID="Button_Revert" runat="server" OnClick="Button_Revert_Click" Text="Revert" class="frame_style_type2" Visible="false" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to revert back to Draft?');"/>
                     <asp:Button ID="Button_Cancel_Draft" runat="server" OnClick="Button_Cancel_Click" Text="Cancel Draft" class="frame_style_type2" Visible="false" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to cancel this Draft?');"/>
                    </div>

                <asp:UpdateProgress runat="server" ID="UpdateProgress3" class="gettext" AssociatedUpdatePanelID="upImg">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="col-12 col-s-12">
                    <asp:UpdatePanel ID="upImg" runat="server">
                        <ContentTemplate>
                            <!--Customer Info ////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_CustInfo" runat="server" Text="Customer Info" class="accordion_style_sub_fixed_darkcyan" />
                            <div id="new_applicant_section_CustomerInfo" style="display: none" runat="server">
                                <!--==============================================================================-->
                                <div class="col-6 col-s-12" id="button_section" runat="server" visible="false">
                                    <div class="col-3 col-s-4" id="labelAccount" runat="server">
                                        <asp:Label ID="Label_Account" class="inputtext" autocomplete="off" runat="server" OnTextChanged="CheckAcc" Style="margin-bottom: 4px"></asp:Label>
                                    </div>

                                    <div class="col-3 col-s-4" id="cusButton" runat="server">
                                        <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find Customer" class="glow" />
                                    </div>
                                </div>

                                <div id="div_CustInfoExtra" visible="false" runat="server">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblAcc" Text="Claim No. :" runat="server" class="labeltext" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblClaimNum" runat="server" class="labeltext"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblClaimStatus" runat="server" Text="Status :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-6 col-s-6">
                                            <asp:Label ID="lblClaimText" runat="server" Text="New" CssClass="labeltext"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Customer :        </label>
                                        </div>
                                        <div class="col-6 col-s-8">
                                            <asp:Label ID="Label_CustName" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Address :        </label>
                                        </div>
                                        <div class="col-6 col-s-8">
                                            <asp:Label ID="Label_Address" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-6">
                                            <asp:Button ID="Btn_Delivery_Addr" runat="server" OnClick="Btn_Delivery_Addr_Click" Text="Alt. Addr." class="glow_green" CausesValidation="false" />
                                        </div>
                                        <asp:GridView ID="GridView_AltAddress" runat="server"
                                            HorizontalAlign="left" CssClass="mydatagrid"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRow2" runat="server" OnCheckedChanged="CheckBox_Changed2" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                    <asp:Label ID="hidden_alt_address_RecId" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_alt_address" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_alt_address_counter" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_Street" class="gettext" runat="server" Visible="false"></asp:Label>

                                    <div class="col-6 col-s-12 print-show" runat="server">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Account No. :        </label>
                                        </div>
                                        <div class="col-6 col-s-8">
                                            <asp:Label ID="Label_CustAcc" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Salesman :        </label>
                                        </div>
                                        <div class="col-5 col-s-8">
                                            <asp:Label ID="Label_Salesman" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Transportation :</label>
                                                    <asp:RequiredFieldValidator ID="rfvTransport" runat="server" ControlToValidate="rblTransport" ErrorMessage="*"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-4 col-s-8">
                                                    <asp:RadioButtonList ID="rblTransport" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="rblTransport_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                            <div class="col-6 col-s-12">
                                                <div class="col-3 col-s-4">
                                                    <label class="labeltext">Claim Type :</label>
                                                    <asp:RequiredFieldValidator ID="rfvWarranty" runat="server" ErrorMessage="*" ControlToValidate="rblWarranty" Display="Dynamic"
                                                        ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>

                                                <div class="col-8 col-s-8">
                                                    <asp:RadioButtonList ID="rblWarranty" runat="server" OnSelectedIndexChanged="rblWarranty_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Vertical">
                                                        <asp:ListItem Text="Batch Return" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Warranty" Value="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                            <div runat="server" class="col-6 col-s-12" id="divProductType" visible="false" style="float: right;">
                                                <div class="col-3 col-s-4">
                                                    <asp:Label ID="lbltype" runat="server" Text="Product Type:" class="labeltext"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="rfvClaim" runat="server" ControlToValidate="ddlClaimType" Display="Dynamic" InitialValue="0"
                                                        ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-4 col-s-8">
                                                    <asp:DropDownList ID="ddlClaimType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonChanged_Warranty" class="dropdownlist">
                                                        <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Battery" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Lubricant" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Other Products" Value="3"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="rblWarranty" />
                                            <asp:PostBackTrigger ControlID="rblTransport" />
                                            <asp:PostBackTrigger ControlID="Btn_Delivery_Addr" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-6 col-s-12" id="warehouse" runat="server" style="float: right;">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Warehouse :        </label>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="ddlWarehouse" Display="Dynamic"
ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                        <asp:DropDownList ID="ddlWarehouse" runat="server" class="dropdownlist"></asp:DropDownList>
                                    </div>

                                    <asp:Label ID="hidden_inventLocationId" class="gettext" runat="server" Visible="false"></asp:Label><br />
                                    <asp:Label ID="hidden_Label_PreviousStatus" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_Label_NextStatus" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_LabelClaimNumber" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:HiddenField ID="hdSoNumber" runat="server" />
                                    <asp:HiddenField ID="hdInventId" runat="server" />
                                    <asp:HiddenField ID="hdSalesmanId" runat="server" />
                                </div>
                            </div>
                            <!--BatteryDetails ////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_BatteryDetails" runat="server" Text="Battery Details" class="accordion_style_sub_fixed_darkcyan" />
                            <div id="new_applicant_section_BatteryDetails" runat="server">
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_BatteryList" runat="server" OnClick="CheckBatteryInList" CausesValidation="false" Text="Find Battery" class="glow" Style="margin-bottom: 4px" />
                                </div>
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Battery Name :        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_BatteryName" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Item ID :        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_ItemID" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Serial No :        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_SerialNo" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ControlToValidate="TextBox_SerialNo" Display="Dynamic"
                                        ErrorMessage="Serial number required!" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>

                                <!-- Jerry 20251119 - Add batch number -->
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Batch No :        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_BatchNo" class="inputtext" autocomplete="off" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_BatchNo" Display="Dynamic"
                                        ErrorMessage="Batch number required!" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                <!-- Jerry 20251119 End -->

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Quantity / Unit :        </label>
                                    </div>
                                    <div class="col-5 col-s-8">
                                        <asp:TextBox ID="TextBox_Quantity" class="inputtext" autocomplete="off" Style="min-width: 25%; max-width: 25%" runat="server"></asp:TextBox>
                                        <asp:DropDownList ID="DropDownList_Unit" runat="server" AutoPostBack="true" class="dropdownlist"></asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvqty" runat="server" ControlToValidate="TextBox_Quantity" Display="Dynamic"
                                        ErrorMessage="Quantity required!" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Customer D/O No :        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:TextBox ID="TextBox_CustomerDO" class="inputtext_2" autocomplete="off" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBatteryCustDO" runat="server" Display="Dynamic" ErrorMessage="Customer D/O required!"
                                            ForeColor="Red" ControlToValidate="TextBox_CustomerDO" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Return Reason :</label>
                                    </div>
                                    <div class="col-4 col-s-8">
                                        <asp:DropDownList ID="DropDownList_ReasonReturn_Battery" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_ReasonReturn_Battery">
                                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Acid Leaking" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="New Battery for Recharge" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Used Battery for Recharge" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Magic Eye" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Bulging" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Unable to Crank" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="Low Voltage" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Packing Issue" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="Others" Value="99"></asp:ListItem>
                                        </asp:DropDownList><br />
                                        <br />
                                        <asp:TextBox ID="TextBox_ReasonReturn_Battery_Other" class="txt_reason" autocomplete="off" runat="server" TextMode="MultiLine"
                                            Rows="4" Columns="20" placeholder="Reason for return" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvBattery" runat="server" Display="Dynamic" ErrorMessage="Reason required!"
                                            ForeColor="Red" ControlToValidate="Textbox_ReasonReturn_Battery_Other"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <asp:Button ID="Button_BatteryInfo" runat="server" OnClick="Button_BatteryInfo_Click" Text="Battery Info" class="glow_green" CausesValidation="false" />
                                
                                </div>
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Btn_Battery_Item_Save" runat="server" OnClick="Button_ItemChanges_Click" CausesValidation="false" Text="Save Changes" class="glow" Style="margin-bottom: 4px" Visible="false" />&nbsp                    
                                </div>
                            </div>

                            <!--ItemDetails Lubricant ////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_ItemDetails" runat="server" Text="Item Details" class="accordion_style_sub_fixed_darkcyan" />
                            <div id="new_applicant_section_ItemDetails" runat="server">
                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Button_Oil" runat="server" OnClick="CheckBatteryInList" CausesValidation="false" Text="Find Item" class="glow" Style="margin-bottom: 4px" />&nbsp                    
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Item Name :        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_ItemName_Item" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Item ID :        </label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:Label ID="Label_ItemID_Item" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Quantity / Unit :        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:TextBox ID="TextBox_Quantity_Item" class="inputtext" autocomplete="off" Style="min-width: 25%; max-width: 25%" runat="server"></asp:TextBox>
                                        &nbsp
                                                <asp:DropDownList ID="DropDownList_Unit_Items" runat="server" AutoPostBack="true" class="dropdownlist"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Customer D/O No:        </label>
                                    </div>
                                    <div class="col-8 col-s-8">
                                        <asp:TextBox ID="TextBox_CustomerDO_Item" class="inputtext_2" autocomplete="off" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCustDo" runat="server" Display="Dynamic" ErrorMessage="Customer D/O required!"
                                            ForeColor="Red" ControlToValidate="TextBox_CustomerDO_Item" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Return Reason :</label>
                                    </div>
                                    <div class="col-3 col-s-8">
                                        <asp:DropDownList ID="DropDownList_ReasonReturn_Item" runat="server" class="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_ReasonReturn_Item">
                                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Leaking/Cracked" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="Cap Seal/Foil Problem" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="Dented" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="Bottle Neck Defect" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="Contain Sediments" Value="13"></asp:ListItem>
                                            <asp:ListItem Text="Packing Issue" Value="14"></asp:ListItem>
                                            <asp:ListItem Text="Damaged During Delivery" Value="15"></asp:ListItem>
                                            <asp:ListItem Text="Less Quantity" Value="16"></asp:ListItem>
                                            <asp:ListItem Text="New Bottle Empty Oil" Value="17"></asp:ListItem>
                                            <asp:ListItem Text="Others" Value="99"></asp:ListItem>
                                        </asp:DropDownList><br />
                                        <br />
                                        <asp:TextBox ID="TextBox_ReasonReturn_Item" class="inputtext" autocomplete="off" runat="server" TextMode="MultiLine"
                                            Rows="4" Columns="20" placeholder="Reason for others" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvReasonReturn_Item" runat="server" Display="Dynamic" ErrorMessage="Reason required!" Enabled="false"
                                            ForeColor="Red" ControlToValidate="TextBox_ReasonReturn_Item"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="col-12 col-s-12">
                                    <asp:Button ID="Btn_Item_Save" runat="server" OnClick="Button_ItemChanges_Click" CausesValidation="false" Text="Save Changes" class="glow" Style="margin-bottom: 4px" Visible="false"/>&nbsp                    
                                </div>

                            </div>

                            <!--BatchItemDetails ////////////////////////////////////////////////////////////////////////////////////-->
                            <asp:Button ID="Accordion_BatchItemDetails" runat="server" Text="Batch Item Details" class="accordion_style_sub_fixed_darkcyan" />
                            <div id="errorMessage" style="color: red;"></div>
                            <asp:UpdatePanel runat="server" ID="upBatch" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="new_applicant_section_BatchItemDetails" runat="server">
                                        <div class="col-12 col-s-12">
                                            <div id="grdCharge_GridView_BatchItem" runat="server" style="max-width: 110%; overflow: auto; max-height: 100%;">
                                                <asp:GridView ID="GridView_BatchItem" runat="server"
                                                    HorizontalAlign="left" class="mydatagrid" DataKeyNames="No."
                                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                                    Style="overflow: hidden;" AutoGenerateColumns="False" OnRowDeleting="GridView_BatchItem_RowDeleting">
                                                    <Columns>
                                                        <asp:BoundField DataField="No." HeaderText="No." ItemStyle-Width="1%" />
                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="30%" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBox_DescriptionBatchItem" class="inputtext" autocomplete="off" runat="server" AutoPostBack="true" CommandName="TextChanged" CommandArgument='<%# Container.DataItemIndex %>'
                                                                    Style="min-width: 100%; max-width: 100%" OnTextChanged="TextBox_DescriptionBatchItem_Changed" AutoCompleteType="Search"></asp:TextBox>
                                                                <asp:DropDownList ID="DropDownList_SearchBatchItem" runat="server" Visible="false" class="dropdownlist" CommandName="SelectedIndexChanged" CommandArgument='<%# Container.DataItemIndex %>'
                                                                    Style="min-width: 100%; max-width: 100%" OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_SearchBatchItem" AutoPostBack="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Item ID" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemID" runat="server" CssClass="labeltext" ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBox_New_QTY" class="inputtext_3" autocomplete="off" runat="server"></asp:TextBox>
                                                                <asp:RangeValidator runat="server" ControlToValidate="TextBox_New_QTY" Style="min-width: 5%; max-width: 5%"
                                                                    ErrorMessage="Invalid Qty!" MaximumValue="9999" SetFocusOnError="True"
                                                                    MinimumValue="0" Type="Integer" Display="Dynamic">
                                                                </asp:RangeValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="rbBtl" Text="" runat="server" Value="1" AutoPostBack="true" OnCheckedChanged="rbBtl_CheckedChanged" ForeColor="Black"></asp:RadioButton><br />
                                                                <asp:RadioButton ID="rbAdd" Text="" runat="server" Value="2" AutoPostBack="true" OnCheckedChanged="rbAdd_CheckedChanged" ForeColor="Black"></asp:RadioButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Customer D/O No" HeaderStyle-Width="30%" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBox_CustomerDO_BatchItem" autocomplete="off" runat="server" CssClass="inputtext" Style="min-width: 100%; max-width: 30%" oninput="DOCharacterCount(this, 'characterCountWarning1')"></asp:TextBox>
                                                                <asp:DropDownList ID="DropDownList_CustDO" runat="server" Visible="false" class="dropdownlist" CommandName="SelectedIndexChanged" CommandArgument='<%# Container.DataItemIndex %>'
                                                                    Style="min-width: 80%; max-width: 100%" OnSelectedIndexChanged="DropDownList_CustDO_SelectedIndexChanged" AutoPostBack="true" />
                                                                <span id="characterCountWarning1" style="color: red;"></span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField ItemStyle-CssClass="print-hide" HeaderStyle-CssClass="print-hide">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hd_RecIdBatchItem" runat="server" />
                                                                <asp:LinkButton ID="lnkdelete" runat="server" CommandName="Delete" CausesValidation="false" CssClass="print-hide">Delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <asp:Button ID="Button_AddBatchItem" runat="server" OnClick="Button_AddBatchItem_Click" CausesValidation="false" Text="Add new row" class="glow_green print-hide" Style="margin: 4px 0px" /><br />
                                        <div class="col-6 col-s-12">
                                            <div class="col-3 col-s-4">
                                                <label class="labeltext">Return Reason :</label>
                                            </div>
                                            <div class="col-6 col-s-6">
                                                <asp:DropDownList ID="DropDownList_ReasonReturn_BatchItemDetails" runat="server" class="dropdownlist" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DropDownList_ReasonReturn_BatchItemDetails_SelectedIndexChanged">
                                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Shop Close" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Wrong Order" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Double Order" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Salesperson Wrong Order Item" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Salesperson Double Order" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="Sales Admin Wrongly Key In" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Reject" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="Wrong Item Delivered By Warehouse" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="Others" Value="99"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="DropDownList_ReasonReturn_OtherProducts" runat="server" class="dropdownlist" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DropDownList_ReasonReturn_OtherProducts_SelectedIndexChanged" Visible="false">
                                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Tyre" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Wiper" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Spark Plug" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Sprocket & Chain" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Brake Shoe" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="Oil Filter" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Others" Value="7"></asp:ListItem>
                                                </asp:DropDownList>

                                                <br />
                                                <br />
                                                <asp:RadioButtonList ID="rblbatch" runat="server" Visible="false" CausesValidation="false">
                                                    <asp:ListItem Text="Bulging" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Groove Cracked" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Uneven Wear" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Ply Seperation" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Open Splice" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="Side Cracked" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="Exposed Bead Wire" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="Air Leak" Value="8"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:TextBox ID="txtbatchReason" runat="server" placeholder="Reason for return" TextMode="MultiLine"
                                                    Rows="4" Columns="20" Visible="false" CssClass="txt_reason"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvBatchReason" runat="server" ErrorMessage="Reason required!" ForeColor="Red"
                                                    Display="Dynamic" ControlToValidate="txtbatchReason" Enabled="false"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <!--uploadImage ////////////////////////////////////////////////////////////////////////////////////-->
                            <div class="col-6 col-s-12" id="pdf_section" runat="server" visible="false">
                                <div class="col-3 cols-s-6">
                                    <asp:Label runat="server" Text="PDF:" CssClass="labeltext"></asp:Label>
                                </div>
                                <asp:HiddenField ID="hdFilePath" runat="server" />
                                <div class="col-6 col-s-6">
                                    <asp:LinkButton ID="lnkBtn1" runat="server" OnClick="lnkBtn1_Click" CausesValidation="False"></asp:LinkButton>

                                    <asp:LinkButton ID="lnkBtn2" runat="server" OnClick="lnkBtn2_Click" CausesValidation="False"></asp:LinkButton>

                                    <asp:LinkButton ID="lnkBtn3" runat="server" OnClick="lnkBtn3_Click" CausesValidation="False"></asp:LinkButton>

                                    <asp:LinkButton ID="lnkBtn4" runat="server" OnClick="lnkBtn4_Click" CausesValidation="False"></asp:LinkButton>
                                </div>
                            </div>

                            <div class="col-6 col-s-12" id="upload_section" runat="server" visible="false">
                                <div class="col-3 col-s-4">
                                    <asp:Label ID="lblDisplay" runat="server" Visible="false" CssClass="labeltext print-hide" Text="Pictures :"></asp:Label>
                                    <asp:Label ID="lblimg" runat="server" Text="Picture 1:" CssClass="labeltext"></asp:Label>
                                </div>
                                <div class="col-8 col-s-7">
                                    <input type="file" runat="server" onchange="previewMultiple(event); upload(this);" name="fileUpload" id="fileUpload" />
                                    <asp:RequiredFieldValidator ID="rfvProductImg" runat="server" ControlToValidate="fileUpload"
                                        ErrorMessage="Image required!" Display="Dynamic" SetFocusOnError="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revFileUpoad" runat="server" ForeColor="Red" ControlToValidate="fileUpload" Display="Dynamic"
                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(jfif)|(JFIF)|(pdf))$"
                                        ErrorMessage="Only jpg,png,jpeg,tiff,pdf,jfif are allowed!"></asp:RegularExpressionValidator>
                                    <div id="gallery" class="gallery"></div>
                                    <a class="open-button print-hide" runat="server" id="btnDisplay" popup-open="popup-1" href="javascript:void(0)">Image</a>
                                    <div class="popup" popup-name="popup-1">
                                        <div class="popup-content">
                                            <div class="img-container">
                                                <br />
                                                <asp:Repeater ID="repeater" runat="server">
                                                    <ItemTemplate>
                                                        <img id="imgProd" runat="server" class="demo cursor" src='<%# Eval("Value") %>' onclick='test(this)' />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <a class="close-button" popup-close="popup-1" href="javascript:void(0)"></a>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-3 col-s-4">
                                    <asp:Label ID="lblimg2" runat="server" Text="Picture 2:" CssClass="labeltext"></asp:Label>
                                </div>
                                <div class="col-7 col-s-7">
                                    <input type="file" runat="server" accept="image/*" onchange="previewMultiple2(event); upload(this);" name="fileUpload2" id="fileUpload2" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fileUpload2"
                                        ErrorMessage="Image required!" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red" ControlToValidate="fileUpload2" Display="Dynamic"
                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(jfif)|(JFIF)|(pdf))$"
                                        ErrorMessage="Only jpg,png,jpeg,tiff,pdf,jfif are allowed!"></asp:RegularExpressionValidator>
                                    <div id="gallery2" class="gallery2"></div>
                                </div>

                                <div class="col-3 col-s-4">
                                    <asp:Label ID="lblimg3" runat="server" Text="Picture 3:" CssClass="labeltext"></asp:Label>
                                </div>
                                <div class="col-7 col-s-7">
                                    <input type="file" accept="image/*" runat="server" onchange="previewMultiple3(event); upload(this);" name="fileUpload3" id="fileUpload3" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ForeColor="Red" ControlToValidate="fileUpload3" Display="Dynamic"
                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(jfif)|(JFIF)|(pdf))$"
                                        ErrorMessage="Only jpg,png,jpeg,tiff,pdf,jfif are allowed!"></asp:RegularExpressionValidator>
                                    <div id="gallery3" class="gallery3"></div>
                                </div>

                                <div class="col-3 col-s-4">
                                    <asp:Label ID="lblimg4" runat="server" Text="Picture 4:" CssClass="labeltext"></asp:Label>
                                </div>
                                <div class="col-7 col-s-7">
                                    <input type="file" runat="server" accept="image/*" onchange="previewMultiple4(event); upload(this);" name="fileUpload4" id="fileUpload4" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ForeColor="Red" ControlToValidate="fileUpload4" Display="Dynamic"
                                        ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(jfif)|(JFIF)|(pdf))$"
                                        ErrorMessage="Only jpg,png,jpeg,tiff,pdf,jfif are allowed!"></asp:RegularExpressionValidator>
                                    <div id="gallery4" class="gallery4"></div>
                                </div>
                                <%---------------------------------------------------DISPLAY IMAGES SLIDES-----------------------------------------%>
                            </div>

                            <div id="divRejectSection" runat="server" class="col-12 col-s-12" visible="false">
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="Label13" runat="server" Text="Rejected Remark :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:TextBox ID="txtRejectedRemark" class="txt_reason" TextMode="MultiLine"
                                            Rows="4" Columns="20" runat="server" CssClass="txt_reason"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <%--InvoiceChkDetails  ////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divInvoiceChkRemark" runat="server" visible="false" class="col-12 col-s-12">
                                <asp:Button ID="InvoiceChkSection" runat="server" Text="Invoice Check Details" class="accordion_style_sub_fixed_darkcyan" />
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="Label14" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-6">
                                        <asp:TextBox ID="txtInvoiceChkRemark" class="txt_reason" TextMode="MultiLine"
                                            Rows="4" Columns="20" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning2')"></asp:TextBox>
                                        <span id="characterCountWarning2" style="color: red;"></span>
                                    </div>
                                </div>
                            </div>

                            <%--TransportationArrangement  ////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="new_applicant_sectionTransportationArrangement" runat="server" visible="false" class="col-12 c-s-12">
                                <asp:Button ID="Accordion_TransportationArrangement" runat="server" Text="Transportation Details " class="accordion_style_sub_fixed_darkcyan" />

                                <div class="col-12 col-s-12">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblTransportName" runat="server" Text="Transporter Name :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtGetTransportName" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblStreet" runat="server" Text="Street :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtGetStreet" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 col-s-12">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblVehicle" runat="server" Text="Vehicle No. :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtVehicle" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblPostCode" runat="server" Text="PostCode / City :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtGetPostcode" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 col-s-12">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblDriver" runat="server" Text="Driver Responsible :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtGetDriver" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="Label11" runat="server" Text="State / Country :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtGetState" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12 col-s-12">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">Collection Date:</label>
                                        </div>

                                        <div class="col-3 col-s-6">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtCollectionDt" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                                <label class="input-group-btn" for="txtCollectionDt">
                                                    <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                                </label>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="Label12" runat="server" Text="Collection Point :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:TextBox ID="txtGetCollectionPoint" runat="server"></asp:TextBox>
                                        </div>
                                        <asp:HiddenField ID="SelectedDateHiddenField" runat="server" />

                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="LabelTransportationRemark" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-6">
                                        <asp:TextBox ID="txtTransportationRemark" class="txt_reason" TextMode="MultiLine"
                                            Rows="4" Columns="20" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning1')"></asp:TextBox>
                                        <span id="characterCountWarning1" style="color: red;"></span>
                                    </div>
                                </div>

                            </div>

                            <%--GoodsReceiveDetails  ////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divGoodsReceiveRemark" runat="server" visible="false">
                                <asp:Button ID="GoodsReceiveSection" runat="server" Text="Good Receive Details" class="accordion_style_sub_fixed_darkcyan" />
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="Label15" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-6">
                                        <asp:TextBox ID="txtGoodReceiveRemark" class="txt_reason" TextMode="MultiLine"
                                            Rows="4" Columns="20" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning3')"></asp:TextBox>
                                        <span id="characterCountWarning3" style="color: red;"></span>
                                    </div>
                                </div>
                            </div>

                            <%--InspectionDetails  ////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divInspectionRemark" runat="server" visible="false">
                                <asp:Button ID="InspectionSection" runat="server" Text="Inspection Details" class="accordion_style_sub_fixed_darkcyan" />
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="Label16" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-6">
                                        <asp:TextBox ID="txtInspectionRemark" class="txt_reason" TextMode="MultiLine"
                                            Rows="4" Columns="10" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning4')"></asp:TextBox>
                                        <span id="characterCountWarning4" style="color: red;"></span>
                                    </div>
                                </div>
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="LabelBatteryVol" runat="server" Text="Battery Voltage :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:TextBox ID="txtBatteryVol" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="LabelCca" runat="server" Text="CCA :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:TextBox ID="txtCca" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Load Test :</label>
                                    </div>
                                    <div class="col-8 col-s-8">
                                        <asp:DropDownList ID="ddlLoadTest" runat="server" CssClass="dropdownlist">
                                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Bad" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Good" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Test Result :</label>
                                    </div>
                                    <div class="col-8 col-s-8">
                                        <asp:DropDownList ID="ddlTestResult" runat="server" CssClass="dropdownlist">
                                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Good Battery" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Replace Battery" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Magic Eye :</label>
                                    </div>
                                    <div class="col-8 col-s-8">
                                        <asp:DropDownList ID="ddlMagicEye" runat="server" CssClass="dropdownlist">
                                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="White" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Black" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Green" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-6 col-s-12 print-hide" runat="server">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="lblImgIns" runat="server" Text="Proof of Inspection :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-7 col-s-7">
                                        <input type="file" runat="server" accept="image/*" onchange="previewInspect(event); upload(this);" name="InspectFile" id="InspectFile" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ForeColor="Red" ControlToValidate="InspectFile" Display="Dynamic"
                                            ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(jfif)|(JFIF)|(pdf))$"
                                            ErrorMessage="Only jpg,png,jpeg,tiff,pdf,jfif are allowed!"></asp:RegularExpressionValidator>
                                        <div id="galleryInspect" class="galleryInspect"></div>
                                        <a class="open-button print-hide" runat="server" id="btnInspectDisplay" popup-open="popup-2" href="javascript:void(0)">Image</a>
                                        <div class="popup" popup-name="popup-2">
                                            <div class="popup-content">
                                                <div class="img-container">
                                                    <br />
                                                    <asp:Image ID="ImgInspect" runat="server" class="demo cursor" src='<%# Eval("Value") %>' onclick='test(this)' />
                                                </div>
                                                <a class="close-button" popup-close="popup-2" href="javascript:void(0)"></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--VerificationDetails  ////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divVerficationRemark" runat="server" visible="false">
                                <asp:Button ID="VerificationSection" runat="server" Text="Verification Details" class="accordion_style_sub_fixed_darkcyan" />
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="Label17" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-6 col-s-6">
                                        <asp:TextBox ID="txtVerificationRemark" class="txt_reason" TextMode="MultiLine"
                                            Rows="4" Columns="20" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning5')"></asp:TextBox>
                                        <span id="characterCountWarning5" style="color: red;"></span>
                                    </div>
                                </div>
                            </div>
                            <%--ApprovalDetails  ////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divApproverRemark" runat="server" visible="false">
                                <asp:Button ID="ApprovalRemarkSection" runat="server" Text="Approval Details" class="accordion_style_sub_fixed_darkcyan" />
                                <div class="col-12 col-s-12">
                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <asp:Label ID="lblApproverRemark" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-6 col-s-6">
                                            <asp:TextBox ID="txtApproverRemark" class="txt_reason" TextMode="MultiLine"
                                                Rows="4" Columns="20" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning6')"></asp:TextBox>
                                            <span id="characterCountWarning6" style="color: red;"></span>
                                        </div>
                                    </div>

                                    <div class="col-6 col-s-12">
                                        <div class="col-3 col-s-4">
                                            <label class="labeltext">CN Required :</label>
                                        </div>
                                        <div class="col-3 col-s-4">
                                            <asp:RadioButtonList ID="rblCNrequired" runat="server" OnSelectedIndexChanged="rblWarranty_SelectedIndexChanged" RepeatDirection="Vertical">
                                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divProcessStat" class="col-12 col-s-12" runat="server" visible="false">
                                <div class="col-6 col-s-12">
                                    <div class="col-3 col-s-5">
                                        <label class="labeltext">Process Status :</label>
                                    </div>
                                    <div class="col-7 col-s-12">
                                        <asp:Label ID="lblGetProcessStat" runat="server" Width="400px"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <hr style="border: 1px thin #000; width: 100%;" hidden class="print-show" />
                            <div class="footer">
                                <div class="column print-show" id="signApproval" runat="server" hidden>
                                    <asp:Label ID="lblPrintedby" runat="server" Text="Returned By:" class="printed-by-label"></asp:Label>
                                    <div style="height: 70px;"></div>
                                    <hr style="border-bottom: 1px solid #000; text-align: left;" />
                                    <br />

                                    <div class="col-12 col-s-12">
                                        <asp:Label ID="lblName" runat="server" Text="Name : "></asp:Label>
                                    </div>
                                    <br />

                                    <div class="col-12 col-s-12">
                                        <br />
                                        <asp:Label ID="labelDate" runat="server" Text="Date : "></asp:Label>
                                    </div>
                                </div>

                                <div class="column print-show" id="signWarehouse_section" hidden>
                                    <asp:Label ID="Label2" runat="server" Text="[Warehouse]Received By:" class="printed-by-label"></asp:Label>
                                    <div style="height: 70px;"></div>
                                    <hr style="border-bottom: 1px solid #000; text-align: left;" />
                                    <br />

                                    <div class="col-12 col-s-1">
                                        <asp:Label ID="Label3" runat="server" Text="Name : "></asp:Label>
                                        <asp:Label ID="lblGetName" runat="server"></asp:Label>
                                    </div>

                                    <div class="col-12 col-s-1">
                                        <br />
                                        <asp:Label ID="label5" runat="server" Text="Date : "></asp:Label>
                                        <asp:Label ID="lblGetDate" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="column" id="divCustChop" runat="server" hidden>
                                    <asp:Label ID="Label4" runat="server" Text="Customer Chop:" class="printed-by-label"></asp:Label>
                                    <div style="height: 70px;"></div>
                                    <hr style="border-bottom: 1px solid #000; text-align: left;" />
                                    <br />
                                    <div class="col-12 col-s-12">
                                        <asp:Label ID="Label6" runat="server" Text="In-Charged Name : "></asp:Label>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <br />
                                        <asp:Label ID="label7" runat="server" Text="Mileage : "></asp:Label>
                                    </div>

                                    <div class="col-3 col-s-4">
                                        <br />
                                        <asp:Label ID="label8" runat="server" Text="Driver Name : "></asp:Label>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <br />
                                        <asp:Label ID="label9" runat="server" Text="Vehicle No. : "></asp:Label>
                                    </div>

                                    <div class="col-3 col-s-4">
                                        <br />
                                        <asp:Label ID="label10" runat="server" Text="Transporter (Company) : "></asp:Label>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="Button_Submit" />
                            <asp:PostBackTrigger ControlID="Button_BatteryList" />
                            <asp:PostBackTrigger ControlID="DropDownList_ReasonReturn_Battery" />
                            <asp:PostBackTrigger ControlID="fileUpload" />
                            <asp:PostBackTrigger ControlID="fileUpload2" />
                            <asp:PostBackTrigger ControlID="fileUpload3" />
                            <asp:PostBackTrigger ControlID="fileUpload4" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <!-- end of new_applicant_section==============================================================================
                    <!-- end of overview_section==============================================================================-->
            <div id="overview_section" runat="server" class="col-12 col-s-12" display="none">
                <div id="div4" runat="server" class="col-3 col-s-12">
                    <asp:Button ID="btnListAll" runat="server" OnClick="btnListAll_Click" class="custom-button frame_style_type2" Text="Pending" CausesValidation="false" />
                    <div runat="server" class="col-10 col-s-12" id="divSearch">
                        <asp:Label ID="label18" runat="server" Text="Search:" CssClass="labeltext"></asp:Label>
                        <asp:TextBox ID="TextBox_Search_Overview" runat="server" OnTextChanged="txtboxListAll_TextChanged" CssClass="inputext" AutoPostBack="true" placeholder="Name/Account/Warranty No." />
                    </div>

                </div>
                <div id="divForSalesman" runat="server" visible="false" class="col-9 col-s-12">
                    <div id="divDraft" runat="server" class="col-3 col-s-4">
                        <asp:Button ID="btnListDraft" runat="server" OnClick="btnListDraft_Click" class="custom-button frame_style_type2" Text="Draft" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblDraftBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="div1" runat="server" class="col-3 col-s-4">
                        <asp:Button ID="btnListReject" runat="server" OnClick="btnListReject_Click" class="custom-button frame_style_type2" Text="Rejected" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblRejectBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="div2" runat="server" class="col-3 col-s-4">
                        <asp:Button ID="btnListApproved" runat="server" OnClick="btnListApproved_Click" class="custom-button frame_style_type2" Text="Approved" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblApprovedBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>
                </div>
                <div id="divForApproval" runat="server" visible="false" class="col-9 col-s-12">

                    <div id="divInvoiceChk" runat="server" class="col-3 col-s-4">
                        <asp:Button ID="btnListInvoiceChk" runat="server" OnClick="btnListInvoiceChk_Click" class="custom-button frame_style_type2" Text="Invoice Check" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblInvoiceChkBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="divTransportArr" runat="server" class="col-3 col-s-4">
                        <asp:Button ID="btnListTransporter" runat="server" OnClick="btnListTransporter_Click" class="custom-button frame_style_type2" Text="Transporter" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblTransporterBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="divGoodsReceived" runat="server" class="col-3 col-s-4">
                        <asp:Button ID="btnListGoodsReceived" runat="server" OnClick="btnListGoodsReceived_Click" class="custom-button frame_style_type2" Text="Goods Received" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblGoodsReceivedBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div class="col-12 col-s-12">
                        <div id="divInspection" runat="server" class="col-3 col-s-4">
                            <asp:Button ID="btnListInspection" runat="server" OnClick="btnListInspection_Click" class="custom-button frame_style_type2" Text="Inspection" CausesValidation="false" />
                            <%--                            <asp:Label ID="LblInspectionBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                        </div>

                        <div id="divVerify" runat="server" class="col-3 col-s-4">
                            <asp:Button ID="btnListVerify" runat="server" OnClick="btnListVerify_Click" class="custom-button frame_style_type2" Text="Verify" CausesValidation="false" />
                            <%--                            <asp:Label ID="LblVerifyBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                        </div>

                        <div id="divApproval" runat="server" class="col-3 col-s-4">
                            <asp:Button ID="btnListApproval" runat="server" OnClick="btnListApproval_Click" class="custom-button frame_style_type2" Text="Awaiting Approval" CausesValidation="false" />
                            <%--                            <asp:Label ID="LblApprovalBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                        </div>

                        <%--                        <div class="col-3 col-s-3">
                            <asp:Button ID="btnListSalesOrder" runat="server" OnClick="btnListSalesOrder_Click" class="custom-button frame_style_type2" Text="Sales Order" CausesValidation="false" Visible="false" />
                        </div>--%>
                    </div>
                </div>
                <!--start of view of general table-->
                <!--view of general table-->
                <div class="col-12 col-s-12">
                    <asp:Button ID="Button_Overview_accordion" runat="server" Text="" class="accordion_style_sub_fixed_darkcyan" Visible="false" />
                </div>

                <div class="col-12 col-s-12" id="div_Searchable_Overview" runat="server" visible="false">
                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <asp:Label ID="lblSearch" class="labeltext" runat="server" Text="Search By: " />
                        </div>

                        <div class="col-6 col-s-5">
                            <asp:DropDownList ID="DropDownList_Search_Overview" runat="server" class="dropdownlist">
                                <asp:ListItem Text="Claim Number" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Customer Account No." Value="1"></asp:ListItem>
                                <asp:ListItem Text="Customer Name"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <asp:Label ID="lblStatus" runat="server" Text="Status Type: " class="labeltext"></asp:Label>
                        </div>
                        <div class="col-8 col-s-8">
                            <asp:DropDownList ID="ddlStatus" runat="server" class="dropdownlist">
                                <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Draft" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Rejected" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Approved" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Transporter" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Awaiting GoodsReceive" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Inspection" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Invoice Chk" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Verified" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Sales Order" Value="10"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ImageButton ID="ImageButton5" class="searchbtn" runat="server" CausesValidation="false" ImageUrl="~/RESOURCES/SearchIcon.png" OnClick="Button_Search_Overview_Click" />
                        </div>
                    </div>

                    <%--                    <div class="col-6 col-s-12">
                        <div class="col-3 col-s-4">
                            <asp:Label ID="lblText" runat="server" Text="Number/Name:" class="labeltext"></asp:Label>
                        </div>
                        <div class="col-5 col-s-8">
                            <asp:TextBox ID="TextBox_Search_Overview" class="textsearch" runat="server" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>--%>

                    <%--                            <div class="col-12 col-s-6">
                                <div class="col-6 col-s-6">
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="lblTotal" runat="server" Text="Total: " CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-3 col-s-4">
                                        <asp:Label ID="lblTotalStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>--%>
                </div>

                <div class="col-12 col-s-12 ">
                    <div id="Div5" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                        <asp:GridView ID="GridViewOverviewList" runat="server"
                            PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid" AllowCustomPaging="true"
                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="No."
                            AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_Overview"
                            HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claim Number">
                                    <ItemTemplate>
                                        <asp:Button ID="Button_ClaimId" runat="server" OnClick="Button_ClaimId_Click" CausesValidation="false" Text='<%# Eval("Claim Number") %>' class="button_grid" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Claim No." HeaderText="Claim No." />
                                <asp:BoundField DataField="Customer Account" HeaderText="Customer Account" />
                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="Claim Type" HeaderText="Claim Type" />
                                <asp:BoundField DataField="Claim Status" HeaderText="Claim Status" />
                                <asp:BoundField DataField="WH" HeaderText="WH" />
                                <asp:BoundField DataField="Next Approver" HeaderText="Next Approver" />
                                <asp:BoundField DataField="Transport Required" HeaderText="Transport Required" />
                                <asp:BoundField DataField="Submitted Date" HeaderText="Submitted Date" />
                                <asp:BoundField DataField="Invoice Checked Date" HeaderText="Invoice Checked Date" />
                                <asp:BoundField DataField="Transport Arrange Date" HeaderText="Transport Arrange Date" />
                                <asp:BoundField DataField="Goods Received Date" HeaderText="Goods Received Date" />
                                <asp:BoundField DataField="Inspection Date" HeaderText="Inspection Date" />
                                <asp:BoundField DataField="Verified Date" HeaderText="Verified Date" />
                                <asp:BoundField DataField="Approved Date" HeaderText="Approved Date" />
                                <asp:BoundField DataField="CN Date" HeaderText="CN Date" />
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblErrMsg" runat="server" CssClass="labeltext" Visible="false"></asp:Label>
                    </div>
                </div>
                <!--end of view of general table-->
                <asp:HiddenField ID="hidden_location" runat="server" />
                <%--                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
            </div>
            <!--Approval_section////////////////////////////////////////////////////////////////////////////////////-->
            <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!-- end of draft_section==============================================================================-->
            <!--report_section////////////////////////////////////////////////////////////////////////////////////-->
            <div id="report_section" runat="server" class="col-12 col-s-12" visible="false">
                <asp:Button ID="btnJobTaken" runat="server" OnClick="btnJobTaken_Click" Text="Jobs Taken" CssClass="frame_style_type2" CausesValidation="false" />
                <asp:Button ID="btnProductionChargingCode" runat="server" OnClick="btnProductionChargingCode_Click" Text="Production/Charging Code" class="frame_style_type2" CausesValidation="false" />
                <asp:Button ID="btnBatteryQueryReport" runat="server" OnClick="btnBatteryQueryReport_Click" Text="Battery Query Report" class="frame_style_type2" CausesValidation="false" />
                <asp:Button ID="btnBatteryStatisticReport" runat="server" OnClick="btnBatteryStatisticReport_Click" Text="Battery Statistic Report" class="frame_style_type2" CausesValidation="false" />

                <div class="col-s-12 col-12">
                    <asp:Button ID="btnTitle" runat="server" Text="Report" class="accordion_style_sub_fixed_darkcyan" /><br />
                </div>

                <div id="generalreport_section" runat="server">
                    <div class="col-6 col-s-12" id="dateRange_section" runat="server">
                        <div class="col-2 col-s-2">
                            <asp:Label ID="lbldate" runat="server" CssClass="labeltext" Text="Date Range: "></asp:Label>
                        </div>
                        <div class="col-3 col-s-6">
                            <div class="input-group">
                                <asp:TextBox ID="txtFromDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                <label class="input-group-btn" for="txtFromDate">
                                    <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                </label>
                                -
                            </div>
                        </div>
                        <div class="col-3 col-s-6">
                            <div class="input-group">
                                <asp:TextBox ID="txtToDate" runat="server" type="text" class="form-control date-input" ReadOnly="true"></asp:TextBox>
                                <label class="input-group-btn" for="txtToDate">
                                    <span class="btn btn-default"><span class="glyphicon glyphicon-calendar"></span></span>
                                </label>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div id="warranty_section" runat="server" visible="false">
                                <div class="col-2 col-s-12">
                                    <div class="col-5 col-s-2">
                                        <asp:Label ID="lblWarrantyType" runat="server" Text="Warranty Type: " CssClass="labeltext"></asp:Label>
                                    </div>

                                    <div class="col-6 col-s-6">
                                        <asp:DropDownList ID="ddlWarranty" runat="server" CssClass="dropdownlist">
                                            <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Batch Return" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Warranty" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12 cols-2">
                                <asp:Button ID="btnGenReport" runat="server" CssClass="glow_green" Text="Generate" OnClick="btnGenReport_Click" CausesValidation="false" />
                            </div>

                            <div class="col-12 col-s-12" id="export" runat="server" visible="false">
                                <div class="image_properties">
                                    <asp:ImageButton ID="imgbtnExport" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/icons8-xls-export-50.png" />
                                    <asp:ImageButton ID="ImageButton2" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/hover-xls-export-50.png" OnClick="btnExport_Click" CausesValidation="false" />
                                </div>
                            </div>
                            </div>

                            <div class="col-12 col-s-12">
                                <div id="JobDaysTaken_section" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                    <asp:GridView ID="gvJobsTaken" runat="server" EmptyDataText="No records found!"
                                        PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        OnPageIndexChanging="gvJobsTaken_PageIndexChanging" AllowCustomPaging="True"
                                        HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Form No." HeaderText="Form No." />
                                            <asp:BoundField DataField="Submitted Date" HeaderText="Date Submit" />
                                            <asp:BoundField DataField="Salesman Code" HeaderText="Salesman Code" />
                                            <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                            <asp:BoundField DataField="Type" HeaderText="Type" />
                                            <asp:BoundField DataField="Claim Type" HeaderText="Claim Type" />
                                            <asp:BoundField DataField="WH" HeaderText="WH" />
                                            <asp:BoundField DataField="Battery Request" HeaderText="Battery Request" />
                                            <asp:BoundField DataField="Serial No." HeaderText="Serial No." />
                                            <asp:BoundField DataField="Transport Arrange" HeaderText="Transport Arrange" />
                                            <asp:BoundField DataField="Invoice Check" HeaderText="Invoice Check" />
                                            <asp:BoundField DataField="Receive" HeaderText="Receive" />
                                            <asp:BoundField DataField="Inspect/Recharge" HeaderText="Inspect/ Recharge" />
                                            <asp:BoundField DataField="Verify" HeaderText="Verify" />
                                            <asp:BoundField DataField="Approval" HeaderText="Approval" />
                                            <asp:BoundField DataField="RMA" HeaderText="RMA" />
                                            <asp:BoundField DataField="CNCC" HeaderText="CNCC" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="Item" HeaderText="Item" />
                                            <asp:BoundField DataField="Reject Remark" HeaderText="Reject Remark" />
                                            <asp:BoundField DataField="Approve Remark" HeaderText="Approve Remark" />
                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings PageButtonCount="2" />
                                        <RowStyle CssClass="rows" />
                                    </asp:GridView>
                                </div>

                                <div class="col-12 col-s-12">
                                    <div id="QueryReport_section" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                        <asp:GridView ID="gvQueryReport" runat="server" EmptyDataText="No records found!"
                                            HorizontalAlign="Left" CssClass="mydatagrid" AllowCustomPaging="true"
                                            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="No."
                                            HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:TemplateField HeaderText="No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Claim Number">
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button_QueryClaimId" runat="server" OnClick="Button_QueryClaimId_Click" CausesValidation="false" Text='<%# Eval("Claim Number") %>' class="button_grid" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Claim Number" HeaderText="Claim Number" />
                                                <asp:BoundField DataField="Customer Account" HeaderText="Customer Account" />
                                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                                <asp:BoundField DataField="Claim Type" HeaderText="Claim Type" />
                                                <asp:BoundField DataField="Claim Status" HeaderText="Claim Status" />
                                                <asp:BoundField DataField="Verified Date" HeaderText="Verified Date" />
                                                <asp:BoundField DataField="Reject Remark" HeaderText="Reject Remark" />
                                                <asp:BoundField DataField="Approve Remark" HeaderText="Approve Remark" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Label ID="Label1" runat="server" CssClass="labeltext" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div id="BatteryQuery_section" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                    <asp:GridView ID="gvBatteryReport" runat="server" EmptyDataText="No records found!"
                                        PageSize="100" HorizontalAlign="Left" CssClass="mydatagrid"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows"
                                        AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_Overview" AllowCustomPaging="True"
                                        HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Form No." HeaderText="Form No." />
                                            <asp:BoundField DataField="Submitted Date" HeaderText="Date Submit" />
                                            <asp:BoundField DataField="Salesman" HeaderText="Salesman" />
                                            <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                            <asp:BoundField DataField="SN" HeaderText="SN" />
                                            <asp:BoundField DataField="Battery Request" HeaderText="Battery Request" />
                                            <asp:BoundField DataField="Transport Arrange" HeaderText="Transport Arrange" />
                                            <asp:BoundField DataField="Receive" HeaderText="Receive" />
                                            <asp:BoundField DataField="Invoice Check" HeaderText="Invoice Check" />
                                            <asp:BoundField DataField="Inspect/Recharge" HeaderText="Inspect/Recharge" />
                                            <asp:BoundField DataField="Verify" HeaderText="Verify" />
                                            <asp:BoundField DataField="Approval" HeaderText="Approval" />
                                            <asp:BoundField DataField="RMA" HeaderText="RMA" />
                                            <asp:BoundField DataField="CNSA" HeaderText="CNSA" />
                                            <asp:BoundField DataField="CNCC" HeaderText="CNCC" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" />
                                            <asp:BoundField DataField="AOI" HeaderText="AOI" />
                                            <asp:BoundField DataField="Inspection Remark" HeaderText="Inspection Remark" />
                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings PageButtonCount="2" />
                                        <RowStyle CssClass="rows" />
                                    </asp:GridView>
                                </div>

                                <div id="BatteryStatistic_section" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%;">
                                    <asp:GridView ID="gvBatteryStatisticReport" runat="server" HorizontalAlign="Left" Style="overflow: hidden;"
                                        CssClass="mydatagrid" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" EmptyDataText="No records found!"
                                        AutoGenerateColumns="False" HtmlEncode="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item Name" HeaderText="Item Name" />
                                            <asp:BoundField DataField="Total Sold" HeaderText="Total Sold" />
                                            <asp:BoundField DataField="Total Claim" HeaderText="Total Claim" />
                                            <asp:BoundField DataField="Percentage" HeaderText="Percentage" />
                                        </Columns>
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings PageButtonCount="2" />
                                        <RowStyle CssClass="rows" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ImageButton2" />
                            <asp:PostBackTrigger ControlID="Button_report_section" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <!--enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:UpdateProgress runat="server" ID="UpdateProgress4" class="gettext" AssociatedUpdatePanelID="UpdatePanel4">
                        <ProgressTemplate>
                            <div class="loading">
                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <div id="enquiries_section" style="display: none" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- end of enquiries_section==============================================================================-->
                </div>
                <!--==================================================================-->
                <div class="row mt_1 mb_2" id="Div3" runat="server">

                    <!--enquiries_section////////////////////////////////////////////////////////////////////////////////////-->
                    <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext" AssociatedUpdatePanelID="UpdatePanel5">
                        <ProgressTemplate>
                            <div class="loading">
                                <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <div id="Div6" style="display: none" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- end of enquiries_section==============================================================================-->
                </div>
                <!--==============================================================================-->
            </div>
            <%--        <script src="scripts/ButtonUp.js"></script>--%>
        </div>
        <asp:HiddenField ID="hfRejectReason" runat="server" ClientIDMode="Static" />
        <div id="myModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Reject Warranty</h2>
                <p>Please provide a reason for reject:</p>
                <textarea id="rejectReason" rows="4" cols="30" placeholder="Enter reason here"></textarea>
                <br />
                <br />
                <asp:Button ID="btnReject" runat="server" Text="OK" OnClientClick="return submitReason();" OnClick="Button_Reject_Click" />
            </div>
        </div>
        <style>
            .modal {
                display: none;
                position: fixed;
                z-index: 1;
                left: 0;
                top: 0;
                width: 100%;
                height: 100%;
                overflow: auto;
                background-color: rgba(0, 0, 0, 0.4);
            }

            .modal-content {
                background-color: #f2f2f2;
                margin: 15% auto;
                padding: 20px;
                border: 2px solid #f58345;
                width: 300px;
            }

            .close {
                float: right;
                cursor: pointer;
            }
        </style>
        <script type="text/javascript">

            // Thumbnail image controls
            function currentSlide(n) {
                showSlides(slideIndex = n + 1);
                //var thumbnail = document.getElementById("imgProd");
                //var imgZoom = document.getElementsByClassName("mySlides");
                //thumbnail.innerHTML = imgZoom.innerHTML;
            }

            function showSlides(n) {
                var newtab = window.open();

                let i;
                let slides = document.getElementsByClassName("Slides");
                let dots = document.getElementsByClassName("demo");
                //let captionText = document.getElementById("caption");

                if (n > slides.length) { slideIndex = 1 }
                if (n < 1) { slideIndex = slides.length }

                for (i = 0; i < slides.length; i++) {
                    slides[i].style.display = "none";
                }

                for (i = 0; i < dots.length; i++) {
                    dots[i].className = dots[i].className.replace(" active", "");
                }
                slides[slideIndex - 1].style.display = "block";
                dots[slideIndex - 1].className += " active";
                //    captionText.innerHTML = dots[slideIndex - 1].alt;
            }

            function test(element) {
                var src = element.src;
                setTimeout(function () {
                    window.open(src, '_blank');
                }, 500);
                return false;
            }
        </script>
    </form>
</body>
</html>
