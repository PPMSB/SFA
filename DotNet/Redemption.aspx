<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Redemption.aspx.cs" Inherits="DotNet.Redemption" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <script src="scripts/GoToTab.js"></script>
    <link href="STYLES/xxtra_global_Content_Adjuster.css" rel="stylesheet" />
    <link href="STYLES/xtra_ButtonUp.css" rel="stylesheet" />
    <link href="STYLES/xtra_Grid_View.css" rel="stylesheet" />
    <link href="STYLES/xtra_Glowing.css" rel="stylesheet" />
    <script src="scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <!-- Bootstrap -->
    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" media="screen" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <title>Redemption</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
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
    </style>
</head>

<script type="text/javascript">
    function previewMultiple(event) {
        var files = event.target.files;
        if (files.length != 0) {
            document.getElementById("gallery").innerHTML = "";
            for (var i = 0; i < files.length; i++) {
                var urls = URL.createObjectURL(files[i]);
                document.getElementById("gallery").innerHTML += '<img src="' + urls + '">';
            }
        } else {
            document.getElementById("fileUpload").value = null;
            document.getElementById("gallery").innerHTML = "";
        }
    }

    function upload(input) {
        var totalFileSize = 0;
        var tenMB = totalFileSize / 10000000;
        if (input.files[0].totalFileSize > tenMB) {
            alert("File size exceeds 10MB!");
            document.getElementById("fileUpload").value = null;
        }
    }

    function initializeDatePicker() {
        var today = new Date();
        var sixMonthsFromNow = new Date(today.getFullYear(), today.getMonth() + 6, today.getDate());

        $('.datepicker').datepicker({
            format: "dd/mm/yyyy",
            //startDate: today,
            endDate: sixMonthsFromNow,
            autoclose: true,
            todayHighlight: true,
            //    datesDisabled: getDisabledDates(today, sixMonthsFromNow)
        });

        var selectedDate = $('#<%= gvItemPoint.ClientID %> input.datepicker').attr('data-selected-date');
    }

    //function getDisabledDates(startDate, endDate) {
    //    var disabledDates = [];
    //    var currentDate = new Date(startDate);

    //    while (currentDate < endDate) {
    //        var formattedDate = formatDate(currentDate);
    //        disabledDates.push(formattedDate);
    //        currentDate.setDate(currentDate.getDate() + 1);
    //    }

    //    return disabledDates;
    //}

    function formatDate(date) {
        var day = date.getDate();
        var month = date.getMonth() + 6;
        var year = date.getFullYear();
        return day + "/" + month + "/" + year;
    }

    $(document).ready(function () {
        initializeDatePicker();
    });

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

    function validateNumber(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }

    function calculateValues(sender) {
        var row = sender.parentNode.parentNode;
        var qty = row.cells[3].getElementsByTagName("input")[0];
        var unitPrice = row.cells[4].getElementsByTagName("input")[0];
        var total = row.cells[5].getElementsByTagName("input")[0];
        var pointVal = row.cells[6].getElementsByTagName("input")[0];

        var qtyValue = parseFloat(qty.value);
        if (unitPrice.value !== "") {
            var unitPriceValue = parseFloat(unitPrice.value);
            total.value = (qtyValue * unitPriceValue).toFixed(2);
            document.getElementById("totalValue").value = total.value;

            var totalVal = parseFloat(total.value);
    //var points = 5.5555;
<%--            var points = document.getElementById('<%= hdpoint.ClientID %>').value;--%>
            pointVal.value = (totalVal / 0.18).toFixed(2);
            document.getElementById("pointValue").value = pointVal.value;
        }
    }

    function calculateTotal() {
        var gv = document.getElementById('<%= gvItemPoint.ClientID %>');
<%--        var txtRM = document.getElementById('<%= txtRM.ClientID %>');--%>
        //var textboxes = document.getElementsByClassName('textbox-input');
        var rows = gv.getElementsByTagName('tr');
        var total = 0;

        for (var i = 1; i < rows.length; i++) {
            var cells = rows[i].getElementsByTagName('td');
            var value = cells[5].getElementsByTagName('input')[0].value;
            if (!isNaN(value) && value.length != 0) {
                total += parseFloat(value);
            }
        }
        document.getElementById('<%= hdTotalValue.ClientID %>').value = total.toFixed(2);
        document.getElementById("txtRM").value = total.toFixed(2);
    }

    function calculatePoints() {
        var gv = document.getElementById('<%= gvItemPoint.ClientID %>');
        var rows = gv.getElementsByTagName('tr');
        var total = 0;

        for (var i = 1; i < rows.length; i++) {
            var cells = rows[i].getElementsByTagName('td');
            var value = cells[6].getElementsByTagName('input')[0].value;
            let unitPrice = value
                .split(",")
                .join("");
            if (!isNaN(unitPrice) && unitPrice.length != 0) {
                total += parseFloat(unitPrice);
            }
        }
        document.getElementById("txtPts").value = total.toFixed(2);
    }

    $(document).ready(function () {
        // Define the function to handle the changes
        function handleBeneficiaryNameChange() {
            var selectedValue = $('input[name="rblBeneficiaryName"]:checked').val();

            // Hide/show elements based on selected value
            if (selectedValue === "1") { // company
                $('#lblBeneficiaryName').show();
                $('#rfvName').prop('disabled', true);
                $('#rfvIC').prop('disabled', true);
                $('#txtBeneficiaryName').hide();
                $('#txtIc').hide();
                $('#lblIc').hide();
                $('#LabelAsterisk1').hide();
                $('#LabelAsterisk3').hide();

            } else if (selectedValue === "2") { // other
                $('#lblBeneficiaryName').hide();
                $('#rfvName').prop('disabled', false);
                $('#rfvIC').prop('disabled', true);
                $('#txtBeneficiaryName').show();
                $('#txtIc').hide();
                $('#lblIc').hide();
                $('#LabelAsterisk1').hide();
                $('#LabelAsterisk3').hide();

            } else if (selectedValue === "0") { // owner
                $('#lblBeneficiaryName').hide();
                $('#rfvName').prop('disabled', false);
                $('#rfvIC').prop('disabled', false);
                $('#lblIc').show();
                $('#txtIc').show();
                $('#txtBeneficiaryName').show();
                $('#LabelAsterisk1').show();
                $('#LabelAsterisk3').show();
            }
        }

        // Handle selection change
        $('input[name="rblBeneficiaryName"]').change(handleBeneficiaryNameChange);

        // Call the function initially
        handleBeneficiaryNameChange();

        if (!$('input[name="rblBeneficiaryName"]:checked').val()) {
            $('#lblBeneficiaryName').hide();
            $('#rfvIC').prop('disabled', true);
            $('#rfvName').prop('disabled', true);
            $('#LabelAsterisk1').hide();
            $('#LabelAsterisk3').hide();
        }

        // Handle GridView selection reset
        $('#GridView_AltAddress').on('change', function () { // Replace with the actual GridView selection event
            handleBeneficiaryNameChange(); // Re-apply the visibility based on radio selection
            $('input[name="rblBeneficiaryName"]').prop('disabled', false); // Ensure rblBeneficiaryName is enabled
        });
    });

    $(document).ready(function () {
        $('input[name="rblBank"]').change(function () {
            var selectedVal = $(this).val();
            if (selectedVal === "2") {//owner
                $('#lblLetter').show();
            }
            else {
                $('#lblLetter').hide();
            }
        });

        // Initially hide lblLetter if no radio button is selected
        if (!$('input[name="rblBank"]:checked').val() ||
            $('input[name="rblBank"]:checked').val() !== "2") {
            $('#lblLetter').hide();
        }
    });

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        $('input[name="rblBank"]').change(function () {
            var selectedVal = $(this).val();
            if (selectedVal === "2") {
                $('#lblLetter').show();
            } else {
                $('#lblLetter').hide();
            }
        });

        // Initially hide lblLetter if no radio button is selected
        if (!$('input[name="rblBank"]:checked').val() || $('input[name="rblBank"]:checked').val() !== "2") {
            $('#lblLetter').hide();
        }
    });


    $(document).ready(function () {
        $('select[name="ddlRedempType"]').change(function () {
            var selectedVal = $(this).val();
            if (selectedVal === "1" && selectedVal === "2") {//po & so
                $('#lblCnCategory').hide();
                $('#ddlCnCategory').hide();
            }
            else {
                $('#lblCnCategory').show();
                $('#ddlCnCategory').show();
            }
        });

        if (!$('select[name="ddlRedempType"]').val() ||
            $('select[name="ddlRedempType"]').val() === "1" ||
            $('select[name="ddlRedempType"]').val() === "2") {
            $('#lblCnCategory').hide();
            $('#ddlCnCategory').hide();
        }
    });

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
        var Reason = document.getElementById("submitReason").value;
        document.getElementById("hdSubmitReason").value = Reason;
        closeModal();
        return true; // Prevent postback
    }

    window.onload = function () {
        var lblStatus = document.getElementById('<%= lbl_Status.ClientID %>');
        var gv = document.getElementById('<%= gvItemPoint.ClientID %>');
        if (lblStatus !== null && gv !== null && lblStatus.innerText !== 'New' && lblStatus.innerText !== 'Awaiting Sales Admin') {
            var rows = gv.getElementsByTagName('tr');
            for (var i = 0; i < rows.length; i++) {
                var cells = rows[i].getElementsByTagName('td');
                if (cells.length >= 4) { // Assuming the TextBoxes are in the second, third, and fourth columns
                    var textbox1 = cells[1].getElementsByTagName('input')[0]; // Assuming the first TextBox is the first input element
                    var textbox2 = cells[2].getElementsByTagName('input')[0]; // Assuming the second TextBox is the second input element
                    var textbox3 = cells[3].getElementsByTagName('input')[0]; // Assuming the third TextBox is the third input element
                    textbox1.readOnly = true; // Set the first TextBox to read-only
                    textbox2.readOnly = true; // Set the second TextBox to read-only
                    textbox3.readOnly = true; // Set the third TextBox to read-only
                }
            }
        }
    };

    function updateCharacterCount(textBox, warningSpanId) {
        var warningSpan = document.getElementById(warningSpanId);
        var maxCharacterCount = 100; // Set your desired character limit
        var remainingCharacters = maxCharacterCount - textBox.value.length;

        if (remainingCharacters < 0) {
            //warningSpan.textContent = 'Exceeded by ' + Math.abs(remainingCharacters) + ' characters!';
            textBox.value = textBox.value.substring(0, maxCharacterCount); // Truncate input
        } else {
            warningSpan.textContent = remainingCharacters + ' characters remaining';
        }
    }

    function confirmNegativeBalance(preposted_ap, deducted_ap) {
        var balance = preposted_ap - deducted_ap;
        if (balance < 0) {
            var negativeAmount = Math.abs(balance).toFixed(2); // absolute value with 2 decimals
            return confirm("Balance A&P Point shows negative -" + negativeAmount + ". Are you sure you want to proceed?");
        }
        return true; // No negative balance, proceed normally
    }
</script>

<body>
    <form id="form1" runat="server">
        <button onclick="ButtonUp()" class="Button_Up" id="Button_Up" title="Go to top">&uarr;</button>
        <div class="container1">
            <div class="mobile_hidden">
                <div class="col-3 col-s-3 image_icon">
                    <img src="RESOURCES/Redemptionn.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 style="font-weight: bold">Redemption</h1>
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
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="upItem">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:UpdateProgress runat="server" ID="UpdateProgress1" class="gettext">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <!--==============================================================================-->
            <div class="col-12 col-s-12">

                <!--content--==============================================================================-->
                <div class="col-12 col-s-12 print-hide">
                    <asp:Button ID="Button_Redemption_section" runat="server" OnClick="Button_Redemption_section_Click" Text="New" class="frame_style_4bar" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" id="img1" runat="server" />
                    <asp:Button ID="Button_Signboard_section" runat="server" OnClick="Button_Signboard_section_Click" Text="Signboard" Visible="false" class="frame_style_4bar" />
                    <asp:Button ID="Button_Redemp_Overview" runat="server" Text="Overview" OnClick="Button_Redemp_Overview_Click" class="frame_style_4bar" OnClientClick="javascript:triggerPostBack(); return false;" />
                    <img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                    <asp:Button ID="Button_Redemp_Enquiries" runat="server" Text="Enquiries" OnClick="Button_Redemp_Enquiries_Click" class="frame_style_4bar" />
                </div>

                <asp:Button ID="Accordion_Redemption" Visible="false" runat="server" Text="Customer" class="accordion_style_sub_fixed_darkcyan" /><br />
                <%--                <asp:UpdatePanel runat="server">
                    <ContentTemplate>--%>
                <div id="new_applicant_section" runat="server" style="display: none">
                    <div class="col-6 col-s-12 print-hide" runat="server">
                        <div class="col-4 col-s-4">
                            <asp:Label ID="lblAcc" runat="server" class="labeltext" Text="Account:"></asp:Label>
                        </div>
                        <div class="col-3 col-s-8">
                            <asp:TextBox ID="TextBox_Account" class="inputtext" autocomplete="off" runat="server" OnTextChanged="CheckAcc" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="TextBox_Account" CssClass="text-danger"
                                Display="Dynamic" ErrorMessage="Account required!" ForeColor="Red" ValidationGroup="submit"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-4 col-s-12">
                            <asp:Button ID="Button_CheckAcc" runat="server" OnClick="CheckAcc" CausesValidation="false" Text="Validate" class="glow_green" Style="margin-bottom: 4px" />
                            <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find in list" class="glow" Style="margin-bottom: 4px" />
                            <asp:Button ID="Button_CheckOutStanding" runat="server" OnClick="Button_CheckOutStanding_Click" CausesValidation="false" Text="Check Outstanding Invoice" CssClass="glow_yellow" />
                        </div>
                    </div>

                    <!--============================================================================== -->

                    <div id="div_CustInfoExtra" runat="server">
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:HiddenField ID="hdStatus" runat="server" />
                                <asp:Label ID="lblStat" runat="server" class="labeltext" Text="Status:"></asp:Label>
                            </div>
                            <div class="col-6 col-s-8">
                                <asp:Label ID="lbl_Status" class="gettext" runat="server" Text="New"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblHq" runat="server" class="labeltext" Text="HQ / BR:"></asp:Label>
                            </div>
                            <div class="col-3 col-s-8">
                                <asp:Label ID="Label_HQ" class="gettext" runat="server" Text=" "></asp:Label>
                            </div>
                        </div>

                        <!--============================================================================== -->
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label20" runat="server" class="labeltext" Text="Redemption ID:"></asp:Label>
                            </div>
                            <div class="col-6 col-s-8">
                                <asp:Label ID="lblGetRedempID" class="gettext" runat="server" Text=" " Font-Bold="true"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblSalesman" runat="server" class="labeltext" Text="Salesman:"></asp:Label>
                            </div>
                            <div class="col-6 col-s-8">
                                <asp:Label ID="Label_Salesman" class="gettext" runat="server" Text=" "></asp:Label>
                            </div>
                            <asp:HiddenField ID="hdsalemanID" runat="server" />
                        </div>

                        <!--============================================================================== -->
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblPhoneNo" runat="server" class="labeltext" Text="Tel No:"></asp:Label>
                            </div>
                            <div class="col-3 col-s-8">
                                <asp:TextBox ID="Textbox_TelNo" class="inputtext" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblClass" runat="server" class="labeltext" Text="Class:"></asp:Label>
                            </div>
                            <div class="col-6 col-s-8">
                                <asp:Label ID="Label_Class" class="gettext" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblCustName" runat="server" class="labeltext" Text="Customer Name:"></asp:Label>
                            </div>
                            <div class="col-8 col-s-8">
                                <asp:Label ID="Label_CustName" class="gettext" runat="server"></asp:Label>
                            </div>
                        </div>

                        <!--============================================================================== -->
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblContactPerson" runat="server" class="labeltext" Text="Contact Person:"></asp:Label>
                            </div>
                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="Textbox_ContactPerson" class="inputtext" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12" id="vendor_section" runat="server" visible="false">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblVendor" runat="server" class="labeltext" Text="Vendor Account:"></asp:Label>
                            </div>
                            <div class="col-5 col-s-5">
                                <asp:Label ID="lblGetVendor" class="inputtext" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdVendorGrp" runat="server" />
                            </div>
                        </div>

                        <!--============================================================================== -->
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblLoyalty" runat="server" class="labeltext" Text="Opening Loyalty Point:"></asp:Label>
                            </div>
                            <div class="col-3 col-s-3">
                                <asp:Label ID="Label_Point" class="gettext" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblRedempTypeNo" runat="server" class="labeltext"></asp:Label>
                            </div>
                            <div class="col-3 col-s-3">
                                <asp:Label ID="lblGetRedempTypeNo" class="gettext" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblJournalId" runat="server" Text="Journal ID:" class="labeltext" Visible="false"></asp:Label>
                            </div>
                            <div class="col-3 col-s-3">
                                <asp:Label ID="lblGetJournalID" class="gettext" runat="server" Font-Bold="true" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="col-6 col-s-12">
                            <asp:CheckBox ID="cbInterestWaive" runat="server" Text="Interest Waiver" CssClass="" />
                            <p style="font-size: 10px"><span style="color: red">*</span> For Credit Control</p>
                        </div>



                        <div class="col-6 col-s-12" id="specialReason" runat="server" visible="false">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label15" runat="server" class="labeltext" Text="Reason:"></asp:Label>
                            </div>
                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtReason" class="inputtext" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <asp:UpdatePanel runat="server" ID="upItem" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!--============================================================================== -->
                                <asp:Panel runat="server" ID="pnlInvoice" class="col-6 col-s-12" Visible="false">
                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="lblInvoice1" runat="server" Text="Invoice ID Ref 1:" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-7 col-s-6">
                                        <asp:Button ID="btnGetInvoice1" runat="server" OnClick="btnGetInvoice1_Click"></asp:Button>
                                    </div>

                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="lblInvoice2" runat="server" Text="Invoice ID Ref 2:" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-7 col-s-6">
                                        <asp:Button ID="btnGetInvoice2" runat="server" OnClick="btnGetInvoice2_Click"></asp:Button>
                                    </div>

                                    <div class="col-4 col-s-4">
                                        <asp:Label ID="lblInvoice3" runat="server" Text="Invoice ID Ref 3:" CssClass="labeltext"></asp:Label>
                                    </div>
                                    <div class="col-7 col-s-6">
                                        <asp:Button ID="btnGetInvoice3" runat="server" OnClick="btnGetInvoice3_Click"></asp:Button>
                                    </div>

                                    <%--                            <div class="col-4 cols-4">
                                <asp:Button ID="btnUpdInvoice" runat="server" Text="Update Invoice" CssClass="glow_green" OnClick="btnUpdInvoice_Click" />
                            </div>--%>
                                </asp:Panel>
                                <!--============================================================================== -->
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Label ID="hidden_customer_class" class="gettext" runat="server" Visible="false"></asp:Label>
                        <asp:HiddenField ID="hdRedempId" runat="server" />
                        <%-- Normal --%>

                        <asp:Panel ID="panelNormal" runat="server" Style="max-height: 150px;">
                            <asp:Button ID="Button1" runat="server" Text="Item Point" class="accordion_style_sub_fixed_darkcyan" /><br />
                            <div class="col-12 col-s-12" style="overflow: auto;">
                                <asp:GridView ID="gvItemPoint" runat="server" CssClass="mydatagrid responsive-gridview" Style="overflow: hidden"
                                    HorizontalAlign="left" HeaderStyle-CssClass="header" AutoGenerateColumns="false" DataKeyNames="No.">
                                    <Columns>
                                        <asp:BoundField DataField="No." HeaderText="No." />
                                        <asp:TemplateField HeaderText="Items">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtItems" runat="server" Height="40px" Width="300px" OnTextChanged="TextBox_DescriptionBatchItem_Changed"
                                                    AutoPostBack="true" CommandName="TextChanged" CommandArgument='<%# Container.DataItemIndex %>' AutoCompleteType="Search"></asp:TextBox>
                                                <asp:DropDownList ID="DropDownList_SearchBatchItem" runat="server" Visible="false" class="dropdownlist" CommandName="SelectedIndexChanged" CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnSelectedIndexChanged="OnSelectedIndexChanged_DropDownList_SearchBatchItem" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Item Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtItemCode" runat="server" Height="40px" Width="170px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" onkeypress="return validateNumber(event)" onchange="calculateValues(this); calculateTotal(this);calculatePoints(this);" CssClass="inputtext" Width="20px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Unit Price (RM)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitPrice" runat="server" onkeypress="return validateNumber(event)" onchange="calculateValues(this); calculateTotal(this);calculatePoints(this);" CssClass="inputtext"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total (RM)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="inputtext readonly-textbox textbox-input"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Points Value">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPoints" runat="server" CssClass="inputtext readonly-textbox textbox-input"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice No">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="inputtext"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="inputtext datepicker"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="hidden_inventDim" runat="server" />
                            </div>
                            <asp:HiddenField ID="hdpoint" runat="server" />
                            <input type="hidden" id="totalValue" name="totalValue" />
                            <input type="hidden" id="pointValue" name="pointValue" />
                            <asp:HiddenField ID="hdTotalValue" runat="server" />
                            <div class="col-12 col-s-12" style="overflow: auto;">
                                <div class="col-2 col-s-3">
                                    <asp:Label ID="lblTotal" runat="server" Text="Total Redemption" CssClass="labeltext"></asp:Label>
                                </div>
                                <div class="col-2 col-s-3">
                                    <asp:Label ID="lblRM" runat="server" Text="RM" CssClass="labeltext"></asp:Label>
                                    <asp:TextBox ID="txtRM" runat="server" CssClass="inputtext readonly-textbox"></asp:TextBox>
                                </div>
                                <div class="col-2 col-s-3">
                                    <asp:Label ID="lblPts" runat="server" Text="Points" CssClass="labeltext"></asp:Label>
                                    <asp:TextBox ID="txtPts" runat="server" CssClass="inputtext readonly-textbox"></asp:TextBox>
                                </div>
                                <div class="col-2 col-s-3" style="overflow: hidden">
                                    <asp:Label ID="lblGST" runat="server" Text="GST (RM)" class="labeltext"></asp:Label>
                                    <asp:TextBox ID="txtGST" runat="server" CssClass="inputtext readonly-textbox"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-6 col-s-6">
                                <asp:Button ID="BtnUpdateDoc" runat="server" CssClass="glow_green" Text="Update Item" OnClick="BtnUpdateDoc_Click" />
                            </div>

                            <asp:UpdatePanel runat="server" ID="itemDetailsPanel" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="col-12 col-s-12">
                                        <asp:Button ID="Button2" runat="server" Text="Item Details" class="accordion_style_sub_fixed_darkcyan" /><br />
                                        <div class="col-6 col-s-12">
                                            <div class="col-4 col-s-4">
                                                <asp:Label ID="lblItemDelivery" runat="server" CssClass="labeltext" Text="Deliver To: "></asp:Label>
                                            </div>

                                            <div class="col-4 col-s-4">
                                                <asp:DropDownList ID="ddlDelivery" runat="server" CssClass="dropdownlist">
                                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Customer/Workshop" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="PPM Warehouse/Salesman Deliver Via S/O" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvDeliver" runat="server" ControlToValidate="ddlDelivery"
                                                    ErrorMessage="Deliver to required!" ForeColor="Red" Display="Dynamic" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="col-6 col-s-12">
                                            <div class="col-4 col-s-4">
                                                <asp:Label ID="lblBeneficiary" runat="server" Text="Beneficiary Name:" CssClass="labeltext"></asp:Label>
                                            </div>

                                            <div class="col-6 col-s-8">
                                                <asp:RadioButtonList ID="rblBeneficiaryName" runat="server" RepeatDirection="Horizontal" CssClass="w3-radio">
                                                    <asp:ListItem Text="Owner" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:Label ID="lblBeneficiaryName" runat="server" CssClass="labetext"></asp:Label>
                                                <asp:TextBox ID="txtBeneficiaryName" runat="server" CssClass="inputtext"></asp:TextBox>
                                                <asp:Label ID="LabelAsterisk1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                <%--                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtBeneficiaryName"
                                                    ErrorMessage="Beneficiary name required!" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                <ContentTemplate>

                                    <div class="col-12 col-s-12">
                                        <div class="col-6 col-s-12">
                                            <div class="col-4 col-s-4">
                                                <asp:Label ID="lblDelAddresss" runat="server" Text="Delivery Address:" CssClass="labeltext"></asp:Label>
                                            </div>

                                            <div class="col-6 col-s-6">
                                                <asp:TextBox ID="txtDeliveryAddr" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                <asp:Button ID="Button_Delivery_Addr" runat="server" OnClick="Button_Alt_Delivery_Addr_Click" Text="Alt. Addr." class="glow_green" />

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
                                        </div>

                                        <asp:Label ID="hidden_alt_address_RecId" class="gettext" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="hidden_alt_address" class="gettext" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="hidden_alt_address_counter" class="gettext" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="hidden_Street" class="gettext" runat="server" Visible="false"></asp:Label>


                                        <div class="col-6 col-s-12">
                                            <div class="col-4 col-s-4">
                                                <asp:Label ID="lblIc" runat="server" CssClass="labeltext" Text="Beneficiary I/C:"></asp:Label>
                                            </div>

                                            <div class="col-4 col-s-4">
                                                <asp:TextBox ID="txtIc" runat="server" CssClass="inputtext"></asp:TextBox>
                                                <asp:Label ID="LabelAsterisk3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-12 col-s-12">
                                        <div class="col-6 col-s-12">
                                            <div class="col-4 col-s-4">
                                                <asp:Label ID="lblBank" runat="server" CssClass="labeltext" Text="Beneficiary Bank Account:"></asp:Label>
                                            </div>

                                            <div class="col-6 col-s-8">
                                                <asp:RadioButtonList ID="rblBank" runat="server" RepeatDirection="Horizontal" CssClass="w3-radio">
                                                    <asp:ListItem Text="Owner" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="None" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:Label ID="lblLetter" runat="server" ForeColor="Red" Text="CP58 letter required!"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvBank" runat="server" ControlToValidate="rblBank" ForeColor="Red"
                                                    ErrorMessage="Bank required!" Display="Dynamic" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                            </div>

                                        </div>
                                        <div class="col-6 col-s-12">
                                            <div class="col-4 col-s-4">
                                                <asp:Label ID="lblRemarks" runat="server" Text="Salesman Remarks:" CssClass="labeltext"></asp:Label>
                                            </div>

                                            <div class="col-5 col-s-8">
                                                <asp:DropDownList ID="DdlSalesmanRemark" runat="server" CssClass=" col-4 col-s-4 dropdownlist">
                                                    <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Campaign" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="3"></asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:TextBox ID="txtRemark" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                <asp:Label ID="LabelAsterisk4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="Button_Delivery_Addr" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>

                    <div class="col-6 col-s-12" id="pdf_section" runat="server" visible="false">
                        <div class="col-4 cols-s-6">
                            <asp:Label runat="server" Text="PDF:" CssClass="labeltext"></asp:Label>
                        </div>
                        <asp:HiddenField ID="hdFilePath" runat="server" />
                        <div class="col-6 col-s-6">
                            <asp:LinkButton ID="lnkBtn1" runat="server" OnClick="lnkBtn1_Click" CausesValidation="False"></asp:LinkButton>

                            <asp:LinkButton ID="lnkBtn2" runat="server" OnClick="lnkBtn2_Click" CausesValidation="False"></asp:LinkButton>

                            <asp:LinkButton ID="lnkBtn3" runat="server" OnClick="lnkBtn3_Click" CausesValidation="False"></asp:LinkButton>
                        </div>
                    </div>

                    <div class="col-6 col-s-12" id="upload_section" runat="server">
                        <div class="col-4 col-s-4">
                            <asp:Label ID="lblDisplay" runat="server" Visible="false" CssClass="labeltext" Text="Document:"></asp:Label>
                            <asp:Label ID="lblimg" runat="server" Text="Document:" CssClass="labeltext"></asp:Label>
                        </div>
                        <div class="col-8 col-s-7">
                            <input type="file" runat="server" onchange="previewMultiple(event); upload(this);" id="fileUpload" multiple />
                            <asp:RegularExpressionValidator ID="revFileUpoad" runat="server" ForeColor="Red" ControlToValidate="fileUpload" Display="Dynamic"
                                ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$"
                                ErrorMessage="Only jpg,png,jpeg,tiff,pdf are allowed!"></asp:RegularExpressionValidator>
                            <div id="gallery" class="gallery"></div>
                        </div>
                    </div>

                    <div class="col-6 col-s-12 print-hide" id="display_section" runat="server">
                        <div class="col-4 col-s-4">
                            <asp:Label ID="lblDoc" runat="server" Text="Document:" CssClass="labeltext"></asp:Label>
                        </div>
                        <a class="open-button" runat="server" id="btnDisplay" popup-open="popup-1" href="javascript:void(0)">Display</a>
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

                    <div class="col-12 col-s-12" id="hod_section" runat="server" visible="false">
                        <asp:Button ID="hodRemarks" runat="server" Text="HOD Remarks" class="accordion_style_sub_fixed_darkcyan" /><br />
                        <div class="col-6 col-s-12" id="dvHOD1" runat="server">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblHod1" runat="server" Text="HOD 1 Remark:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtHod1" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px" ValidateRequestMode="Disabled"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblAnP" runat="server" Text="Deduct A&P Points:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-8" runat="server" id="DivBeforeSubmit">
                                <asp:CheckBox ID="cbAnP" runat="server" Text="Points" OnCheckedChanged="cbAnP_CheckedChanged" />
                                <asp:TextBox ID="txtAnP" runat="server" CssClass="inputtext"></asp:TextBox>points<br />
                                <asp:Label ID="lblRedWarning" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label16" runat="server" Text="Balance A&P Point:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblHODaP" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label17" runat="server" Text="Last Invoice Trans:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-8">
                                <asp:Label ID="lblHODgetLastInvoice" runat="server"></asp:Label>
                            </div>

                        </div>

                    </div>
                    <div class="col-12 col-s-12" id="dvHOD2" runat="server" visible="false">
                        <div class="col-2 col-s-4">
                            <asp:Label ID="lblHod2" runat="server" Text="HOD 2 Remark:" CssClass="labeltext"></asp:Label>
                        </div>

                        <div class="col-4 col-s-4">
                            <asp:TextBox ID="txtHod2" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-12 col-s-12" id="dvHOD3" runat="server" visible="false">
                        <div class="col-2 col-s-4">
                            <asp:Label ID="lblHod3" runat="server" Text="HOD 3 Remark:" CssClass="labeltext"></asp:Label>
                        </div>

                        <div class="col-4 col-s-4">
                            <asp:TextBox ID="txtHod3" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-12 col-s-12" id="admin_section" runat="server" visible="false">
                        <asp:Button ID="Button3" runat="server" Text="Sales Admin" class="accordion_style_sub_fixed_darkcyan" /><br />
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblAdmin" runat="server" Text="Remarks:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-6 col-s-6">
                                <asp:TextBox ID="txtAdmin" runat="server" Rows="4" oninput="updateCharacterCount(this, 'characterCountWarning2')"
                                    Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                <span id="characterCountWarning2" style="color: red;"></span>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label11" runat="server" Text="Assign Point Category:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:DropDownList ID="ddlPointCategory" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label10" runat="server" Text="Credit Note Type:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:DropDownList ID="ddlCNType" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Normal CN" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Normal Invoice" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="GP CN(Stop use)" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Point Payment Contra" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="VPPP" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="EOR/EOL Related" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Interest, Legal Fee and etc" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="WDP / STAR" Value="8"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label13" runat="server" Text="Header:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtHeader" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblCnCategory" runat="server" Text="Credit Note Category:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:DropDownList ID="ddlCnCategory" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label14" runat="server" Text="CN Reason:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtCNreason" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label1" runat="server" Text="Last Invoice Trans:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-8">
                                <asp:Label ID="lblInvoiceTrans" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblredempType" runat="server" Text="Redemption Type:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:DropDownList ID="ddlRedempType" runat="server" CssClass="dropdownlist" OnTextChanged="ddlRedempType_TextChanged" AutoPostBack="true">
                                    <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Purchase Order" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Sales Order" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Credit Note" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Customer" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="5"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label2" runat="server" Text="Balance Loyalty Point:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblLoyaltyPoint" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label12" runat="server" Text="Ledger Account:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:DropDownList ID="ddlLedgerAcc" runat="server" CssClass="dropdownlist">
                                    <asp:ListItem Text="-- SELECT --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="310-2080 PROV-TRADE DISCOUNTS(4W INC)" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="310-2130 PROV-TRADE DISCOUNTS(INDUSTRIAL)" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="310-2090 PROV-TRADE DISCOUNTS(2W INC)" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <%--                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label4" runat="server" Text="EOL:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblEOL" runat="server"></asp:Label>
                            </div>
                        </div>--%>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label5" runat="server" Text="PrePosted Loyalty Point:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblPrepostedLoyalty" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label6" runat="server" Text="EOR:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblEOR" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label7" runat="server" Text="Balance A&P Point:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblBalanceAP" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label8" runat="server" Text="BDP:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblBDP" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label9" runat="server" Text="PrePosted A&P Point:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblPrePostedAP" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-s-12" id="ccMng_section" runat="server" visible="false">
                        <asp:Button ID="Button6" runat="server" Text="Credit Control Manager" class="accordion_style_sub_fixed_darkcyan" /><br />
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label4" runat="server" Text="Remarks:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtCcMng" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-s-12" id="adminMng_section" runat="server" visible="false">
                        <asp:Button ID="Button4" runat="server" Text="Sales Admin Manager" class="accordion_style_sub_fixed_darkcyan" /><br />
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="Label3" runat="server" Text="Remarks:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtAdminMng" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-6">
                                <asp:CheckBox ID="cbSpecialApproval" Text=" Special Approval" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-s-12" id="operationMng_section" runat="server" visible="false">
                        <asp:Button ID="Button5" runat="server" Text="Operation Manager" class="accordion_style_sub_fixed_darkcyan" /><br />
                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-4">
                                <asp:Label ID="lblOpMng" runat="server" Text="Remarks:" CssClass="labeltext"></asp:Label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="txtOperationMng" runat="server" Rows="4" Columns="35" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <div class="col-4 col-s-6">
                                <asp:CheckBox ID="cbGMApproval" Text=" Request GM Approval" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-s-12">
                        <div class="col-2 col-s-4">
                            <asp:Label ID="lblProcessDate" runat="server" Text="Process Date:" CssClass="labeltext"></asp:Label>
                        </div>
                        <div class="col-6 col-s-6">
                            <asp:Label ID="lblGetProcessDt" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="col-12 col-s-12">
                        <div class="col-2 col-s-6">
                            <asp:Label ID="lblProcessStatus" runat="server" Text="Process Status:" CssClass="labeltext"></asp:Label>
                            <i class="fa fa-question-circle info-icon" onclick="showInfo()"></i>
                            <div id="infoPopup" class="info-popup">
                                <p style="font-weight: bold;">Process Status</p>
                                <ul>
                                    <li>SB: Submit by</li>
                                    <li>DM: Review updated on</li>
                                    <li>VR: Verify by</li>
                                    <li>PR: Process by</li>
                                    <li>AP: Approved by</li>
                                    <li>RJ: Rejected by</li>
                                    <li>RV: Reversed</li>
                                    <li>AC: Acknowledge</li>
                                    <li>RC: Recommended</li>
                                    <li>AG: Approved to GM</li>
                                </ul>
                                <div id="btnCloseStatus" class="btn" style="cursor: pointer; border: 1px solid black; margin-top: 10px; background-color: #2c4c3b; color: #ffffff;">Close</div>
                            </div>
                        </div>

                        <div class="col-6 col-s-12">
                            <asp:Label ID="lblGetProcessStat" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="col-12 col-s-12 print-hide">
                        <asp:Button ID="Button_Submit" runat="server" Text="Submit" OnClick="Button_Submit_Click" CssClass="frame_style_type2" ValidationGroup="submit" />
                        <asp:Button ID="Button_AdminApprove" runat="server" Text="Special Approve(Admin)" Visible="false" CssClass="frame_style_type2" OnClientClick="return openModal();" />
                        <asp:Button ID="Button_Approve" runat="server" Text="Approve" OnClick="Button_Approve_Click" class="frame_style_type2" ValidationGroup="submit" ValidateRequestMode="Disabled" OnClientClick="return confirmNegativeBalance(preposted_ap_value, deducted_ap_value);" />
                        <asp:Button ID="Button_Reject" runat="server" Text="Reject" OnClick="Button_Reject_Click" class="frame_style_type2" />
                        <asp:Button ID="Button_Hold" runat="server" Text="Hold" CssClass="frame_style_type2" Visible="false" OnClick="Button_Hold_Click" />
                        <asp:Button ID="Button_Email" runat="server" Text="Email" class="frame_style_type2" OnClick="Button_Email_Click" Visible="false" />
                        <asp:Button ID="BtnAmend" runat="server" Text="Reverse - HOD" CssClass="frame_style_type2" OnClick="BtnAmend_Click" />
                        <asp:Button ID="BtnReverse" runat="server" Text="Reverse - Sales Admin" CssClass="frame_style_type2" OnClick="BtnReverse_Click" Visible="false" />
                        <asp:Button ID="Button_CreateVoucher" runat="server" Text="Create Voucher" OnClick="Button_CreateVoucher_Click" CssClass="frame_style_type2" Visible="false" />
                        <asp:Button ID="Button_Print" runat="server" Text="Print" OnClick="Button_Print_Click" CssClass="frame_style_type2" Visible="false" />
                    </div>

                </div>

            </div>
            <asp:HiddenField ID="hdRedemID" runat="server" />
            <div id="div_Overview" runat="server">
                <div id="dvOverView" runat="server" visible="false">
                    <div class="col-6 col-s-6">
                        <div class="col-4 col-s-4">
                            <asp:Label ID="lblSearch" runat="server" Text="Search: " CssClass="labeltext"></asp:Label>
                        </div>
                        <div class="col-4 col-s-4">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="inputtext" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" placeholder="Name/Account/RP No."></asp:TextBox><br />
                        </div>
                    </div>

                    <div class="col-6 col-s-12">
                        <div class="col-4 col-s-2">
                            <asp:Label ID="lblSort" runat="server" Text="Filter By:" CssClass="labeltext"></asp:Label>
                        </div>
                        <div class="col-6 col-s-10">
                            <asp:DropDownList ID="ddlSort" runat="server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                                <asp:ListItem Text="--- Please Select ---"></asp:ListItem>
                                <asp:ListItem Text="Awaiting HOD" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Sales Admin" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Sales Admin Manager" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Credit Control Manager" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Operation Manager" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Awaiting General Manager" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Rejected" Value="6"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div id="dvEnquiry" runat="server">
                    <div id="divForSalesman" runat="server" visible="false" class="col-12 col-s-12">
                        <div class="col-2 col-s-4">
                            <asp:Button ID="btnListAll" runat="server" OnClick="btnListAll_Click" class="custom-button frame_style_type2" Text="Pending" CausesValidation="false" />
                        </div>

                        <div id="div1" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListReject" runat="server" OnClick="btnListReject_Click" class="custom-button frame_style_type2" Text="Rejected" CausesValidation="false" />
                        </div>

                        <div id="div2" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListApproved" runat="server" OnClick="btnListApproved_Click" class="custom-button frame_style_type2" Text="Approved" CausesValidation="false" />
                        </div>

                        <div id="div3" runat="server" class="col-4 col-s-8">
                            <asp:Label ID="lblSearchHOD" runat="server" Text="HOD: " CssClass="labeltext"></asp:Label>
                            <asp:TextBox ID="txtSearchHOD" runat="server" CssClass="inputtext" AutoPostBack="true" placeholder="HOD Name" Visible="false"></asp:TextBox>
                            <asp:Button ID="btnSearchHOD" runat="server" OnClick="btnSearchHOD_Click" class="" Visible="false" Text="Search" CausesValidation="false" OnClientClick="this.value='Searching...'; document.getElementById('loading').style.display='block';" />
                        </div>
                    </div>

                    <div id="divForApproval" runat="server" visible="false" class="col-12 col-s-12">

                        <div id="divInvoiceChk" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListHOD" runat="server" OnClick="btnListHOD_Click" class="custom-button frame_style_type2" Text="Awaiting HOD" CausesValidation="false" />
                        </div>

                        <div id="divTransportArr" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListSalesAdmin" runat="server" OnClick="btnListSalesAdmin_Click" class="custom-button frame_style_type2" Text="Awaiting Sales Admin" CausesValidation="false" />
                        </div>

                        <div id="divGoodsReceived" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListSAManager" runat="server" OnClick="btnListSAManager_Click" class="custom-button frame_style_type2" Text="Awaiting Sales Admin Manager" CausesValidation="false" />
                        </div>

                        <div id="divInspection" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListCreditControl" runat="server" OnClick="btnListCreditControl_Click" class="custom-button frame_style_type2" Text="Awaiting Credit Control Manager" CausesValidation="false" />
                        </div>

                        <div id="divVerify" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListOperation" runat="server" OnClick="btnListOperation_Click" class="custom-button frame_style_type2" Text="Awaiting Operation Manager" CausesValidation="false" />
                        </div>

                        <div id="divApproval" runat="server" class="col-2 col-s-4">
                            <asp:Button ID="btnListGM" runat="server" OnClick="btnListGM_Click" class="custom-button frame_style_type2" Text="Awaiting General Manager" CausesValidation="false" />
                        </div>

                    </div>
                </div>

                <div class="col-12 col-s-12">
                    <div id="collapse" style="max-width: 100%; overflow: auto; max-height: 110%;">
                        <asp:GridView ID="Gv_Overview" runat="server" CssClass="mydatagrid" Style="border-collapse: collapse; overflow: hidden;"
                            HeaderStyle-CssClass="header" AutoGenerateColumns="false" OnRowDataBound="Gv_Overview_RowDataBound" OnRowCommand="Gv_Overview_RowCommand"
                            AllowCustomPaging="true" PageSize="20" AllowPaging="true" OnPageIndexChanging="Gv_Overview_PageIndexChanging" EmptyDataText="No record found!">
                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Redemption ID" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnRedemptionId" runat="server" CommandName="Redemption" CommandArgument='<%# Eval("Redemption ID") %>'
                                            Text='<%# Eval("Redemption ID") %>' class="button_grid" CausesValidation="false" Font-Underline="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Customer Account" HeaderText="Customer Account" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Sales Rep" HeaderText="Sales Representative" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount (RM)" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Approval" HeaderText="Next Approval" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Date" HeaderText="Applied Date" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <PagerSettings PageButtonCount="2" />
                            <RowStyle CssClass="rows" />
                        </asp:GridView>
                        <asp:GridView ID="Gv_OverviewSearch" runat="server" CssClass="mydatagrid" Style="border-collapse: collapse; overflow: hidden;"
                            HeaderStyle-CssClass="header" AutoGenerateColumns="false" OnRowDataBound="Gv_OverviewSearch_RowDataBound" OnRowCommand="Gv_Overview_RowCommand"
                            AllowCustomPaging="true" PageSize="20" AllowPaging="true" OnPageIndexChanging="Gv_OverviewSearch_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Redemption ID" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnRedemptionId" runat="server" CommandName="Redemption_Search" CommandArgument='<%# Eval("Redemption ID") %>'
                                            Text='<%# Eval("Redemption ID") %>' class="button_grid" CausesValidation="false" Font-Underline="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Customer Account" HeaderText="Customer Account" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Sales Rep" HeaderText="Sales Representative" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount (RM)" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Approval" HeaderText="Next Approval" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Date" HeaderText="Applied Date" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <PagerSettings PageButtonCount="2" />
                            <RowStyle CssClass="rows" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <%--        </div>--%>
        <script type="text/javascript">
            function text(element) {
                var src = element.src;
                setTimeout(function () {
                    window.open(src, '_blank');
                }, 500);
                return false;
            }

            $(document).ready(function () {
                $('select[name="DdlSalesmanRemark"]').change(function () {
                    var selectedVal = $(this).val();
                    if (selectedVal === "3") {//other reason
                        $('#txtRemark').show();
                    }
                    else {
                        $('#txtRemark').hide();
                    }
                });

                if (!$('select[name="DdlSalesmanRemark"]').val() ||
                    $('select[name="DdlSalesmanRemark"]').val() !== "3") {
                    $('#txtRemark').hide();
                }

            });

            function showInfo() {
                $('#infoPopup').show()
            }

            $('#btnCloseStatus').on('click', function () {
                $('#infoPopup').hide()

            })
        </script>
        <asp:HiddenField ID="hdSubmitReason" runat="server" ClientIDMode="Static" />
        <div id="myModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Outstanding invoice more than 120 days.</h2>
                <p>Please provide a reason for submission:</p>
                <textarea id="submitReason" rows="4" cols="70" placeholder="Enter reason here"></textarea>
                <br />
                <br />
                <asp:Button ID="btnOK" runat="server" Text="OK" OnClientClick="return submitReason();" OnClick="Button_AdminApprove_Click" />
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
                width: 650px;
            }

            .close {
                float: right;
                cursor: pointer;
            }
        </style>
        <script>
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
