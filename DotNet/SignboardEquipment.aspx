<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignboardEquipment.aspx.cs" Inherits="DotNet.SignboardEquipment" %>

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
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.bundle.min.js"></script>
    <%--    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />--%>
    <%--    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>--%>
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>

    <title>Signboard & Equipment</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />
    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <style type="text/css">
        @media print {
            body {
                transform: scale(0.80); /* Scale down to 75% */
                transform-origin: top left; /* Set the origin for scaling */
                /*width: 100%;*/  /*Ensure the width is 100% */
                /*height: auto;*/  /*Allow height to adjust */
                overflow: hidden; /* Prevent overflow */
                width: 210mm;
                height: 297mm;
                margin: 0 !important;
                padding: 0 !important;
            }
            /* Optional: Adjust margins for print */
            @page {
                 margin: 10px 30px;  /* Set top/bottom to 10px and left/right to 30px */
                size: A4;
            }

            .print-hide {
                display: none;
            }

            .print-show {
                display: block;
            }

            .container {
                display: block; /* Change to a block layout for print */
                width: 100%; /* Ensure full width */
                box-sizing: border-box; /* Include padding in width */
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

        .modal-map {
            display: flex;
            justify-content: center;
            align-items: center;
            border: 1px solid black;
            background-color: #d5d5d5;
            height: 50px;
        }

        #GridViewOverviewList th, #GridViewOverviewList td, GridView1 th, #GridView1 td {
            border: 1px solid;
        }
      
/* Global box-sizing for consistency (add if not in Bootstrap) */
* {
    box-sizing: border-box;
}

/* Specific styling for the PDF label */
.pdf-label {
    font-weight: bold; /* Make the label bold */
    margin-bottom: 10px; /* Space below the label */
    display: block; /* Ensures label is on its own line */
    font-size: 14px;
}

/* PDF list container */
.pdf-list {
    margin-top: 10px; /* Space between label and list */
    display: grid; /* Use grid layout for items */
    grid-template-columns: repeat(5, 1fr); /* 5 items per row on desktop */
    gap: 15px; /* Space between items */
    width: 100%; /* Ensure full container width */
}

/* Individual PDF items */
.pdf-grid-item {
    min-width: 0; /* Prevents overflow */
    width: 100%; /* Full width within grid cell */
}

.pdf-card {
    border: 1px solid #e0e0e0;
    border-radius: 4px;
    padding: 10px;
    height: 100%;
    display: flex;
    flex-direction: column;
    width: 100%; /* Prevent card squeeze */
}

.pdf-card.even {
    background-color: #ffffff;
}

.pdf-card.odd {
    background-color: #f5f5f5;
}

.pdf-card:hover {
    background-color: #e9f5ff;
    border-color: #b3d7ff;
}

/* PDF link styling */
.pdf-link {
    color: #0066cc;
    text-decoration: none;
    word-break: break-word; /* Ensures long filenames wrap */
    margin-bottom: 8px;
    flex-grow: 1;
    overflow: hidden; /* Safety for very long text */
    text-overflow: ellipsis; /* Truncate with ... if needed */
    white-space: nowrap; /* Or remove for full wrapping */
}

.pdf-link:hover {
    text-decoration: underline;
}

/* Delete button */
.pdf-delete-btn {
    background-color: #ff4444;
    color: white;
    border: none;
    padding: 4px 8px;
    border-radius: 3px;
    cursor: pointer;
    align-self: flex-end; /* Aligns button to right */
    margin-top: auto; /* Pushes button to bottom */
    width: auto; /* Don't force full width yet */
}

.pdf-delete-btn:hover {
    background-color: #cc0000;
}

/* Specific styling for the Document label */
.doc-label {
    font-weight: bold; /* Make the label bold */
    margin-bottom: 10px; /* Space below the label */
    display: block; /* Ensures label is on its own line */
    font-size: 14px;
}

/* Image container */
.img-container {
    margin-top: 10px; /* Space between label and images */
    display: grid; /* Use grid layout for items */
    grid-template-columns: repeat(5, 1fr); /* 5 items per row on desktop */
    gap: 15px; /* Space between items */
    width: 100%; /* Ensure full container width */
}

/* Individual image items */
.img-grid-item {
    display: flex;
    flex-direction: column; /* Stack image and button vertically */
    align-items: center; /* Center items */
    width: 100%; /* Full width within grid cell */
}

/* Image styling */
.demo {
    max-width: 100%; /* Responsive image */
    height: auto; /* Maintain aspect ratio */
    cursor: pointer; /* Pointer cursor on hover */
    width: 100%; /* Ensure images fill grid cell */
}

/* Delete button */
.img-delete-btn {
    background-color: #ff4444; /* Red background */
    color: white; /* White text */
    border: none; /* No border */
    padding: 4px 8px; /* Padding for button */
    border-radius: 3px; /* Rounded corners */
    cursor: pointer; /* Pointer cursor on hover */
    margin-top: 5px; /* Space above the button */
    width: auto; /* Don't force full width yet */
}

.img-delete-btn:hover {
    background-color: #cc0000; /* Darker red on hover */
}

/* RESPONSIVE MEDIA QUERIES: The Key Fix for Mobile Squeeze */
/* Tablet (≥768px, e.g., iPad portrait) */
@media (max-width: 991.98px) {
    .pdf-list,
    .img-container {
        grid-template-columns: repeat(3, 1fr); /* 3 columns: More space than 5 */
        gap: 12px; /* Slightly smaller gap */
    }
    
    .pdf-label,
    .doc-label {
        font-size: 16px; /* Slightly larger for touch */
    }
}

/* Mobile Large (≥576px, e.g., landscape phone) */
@media (max-width: 767.99px) {
    .pdf-list,
    .img-container {
        grid-template-columns: repeat(2, 1fr); /* 2 columns: Prevents squeeze */
        gap: 10px;
    }
    
    .pdf-card,
    .img-grid-item {
        padding: 8px; /* Smaller padding on mobile */
    }
    
    .pdf-delete-btn,
    .img-delete-btn {
        padding: 6px 12px; /* Larger for easier tapping */
        width: 100%; /* Full width button on mobile */
        align-self: stretch; /* Stretch across card */
    }
}

/* Mobile Small (<576px, e.g., portrait phone) - Fallback to Single Column/Flex */
@media (max-width: 575.98px) {
    .pdf-list,
    .img-container {
        grid-template-columns: 1fr; /* 1 column: Full width, no squeeze */
        gap: 8px; /* Minimal gap */
        display: flex; /* Switch to flex for better stacking */
        flex-direction: column;
    }
    
    .pdf-label,
    .doc-label {
        font-size: 14px;
        margin-bottom: 8px;
        text-align: center; /* Center label on very small screens */
    }
    
    .pdf-card {
        flex-direction: column;
        align-items: stretch; /* Full width items */
        min-height: auto; /* No fixed height issues */
    }
    
    .pdf-link {
        white-space: normal; /* Allow full wrapping on mobile */
        text-align: center;
    }
    
    .pdf-delete-btn,
    .img-delete-btn {
        width: 100%;
        margin-top: 8px;
    }
    
    /* If Bootstrap cols are still squeezing, override */
    .col-10 {
        padding-left: 0 !important;
        padding-right: 0 !important;
        width: 100% !important;
    }
    
    .col-2 {
        width: 100% !important; /* Stack label full-width */
        text-align: center;
        margin-bottom: 10px;
    }
}
    </style>
</head>
<script type="text/javascript"> 
    window.addEventListener('popstate', function (event) {
        event.preventDefault();
        history.forward();
    });

    //function previewMultiple(event) {
    //    var img = document.getElementById("fileUpload");
    //    if (img.files.length != 0) {
    //        var urls = URL.createObjectURL(event.target.files[0]);
    //        document.getElementById("gallery").innerHTML = "";
    //        document.getElementById("gallery").innerHTML += '<img src="' + urls + '">';
    //    } else {
    //        document.getElementById("fileUpload").value = null;
    //        document.getElementById("gallery").innerHTML = "";
    //    }
    //}

    function previewMultiple(event) {
        var files = event.target.files;
        if (files.length != 0) {
            document.getElementById("gallery").innerHTML = "";
            for (var i = 0; i < files.length; i++) {
                var urls = URL.createObjectURL(files[i]);
                document.getElementById("gallery").innerHTML += '<img src="' + urls + '">';
            }
            uploadButton.style.display = 'block'; // Show the upload button 
        } else {
            document.getElementById("fileUpload").value = null;
            document.getElementById("gallery").innerHTML = "";
            uploadButton.style.display = 'none'; // Hide the upload button if no files are selected 
        }
    }
 
    function upload(input) {
        var uniqueKey = document.getElementById('<%= hidden_LabelEquipID.ClientID %>').value;
        // Optionally, you can trigger the upload function here if needed
        // if (uniqueKey) {
        //     document.getElementById('uploadForm').submit(); // Uncomment if you want to auto-submit
        // }
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

<%--    function gv_Search(strKey) {
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
    }--%>

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

    function checkValidation() {
        var isValidate = true;

        for (var i = 1; i <= 20; i++) {
            if ($('#RequiredFieldValidator' + i).is(":visible")) {

                isValidate = false;
            }
        }

        return isValidate;
    }

    $(document).ready(function () {
        // Attach a click event handler to the button
        $('#<%= Button_Submit.ClientID %>').click(function () {
            // Show the UpdateProgress control
            $('#<%= UpdateProgress.ClientID %>').show();
            if (!checkValidation()) {
                $('#<%= UpdateProgress.ClientID %>').hide();
            }
        });

        $('#<%= Button_Approve.ClientID %>').click(function () {
            $('#<%= UpdateProgress.ClientID %>').show();
        });

        $('#<%= btnUpload.ClientID %>').click(function () {
            $('#<%= UpdateProgress.ClientID %>').show();
        });

  
<%--        $('#<%= Button_CreateReturn.ClientID %>').click(function () {
            $('#<%= UpdateProgress.ClientID %>').show();
        });--%>

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
                    <img src="RESOURCES/Equipment.png" class="image_resize" />
                </div>

                <div class="col-9 col-s-9 image_title">
                    <h1 style="font-weight: bold">Signboard & Equipment</h1>
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
                <a href="WClaim.aspx" id="WClaimTag2" runat="server" visible="false">Warranty</a>
                <%--<a href="SignboardEquipment.aspx" id="SignboardTag2" runat="server">Sign & Equip</a>--%>
                <div class="DropDown">
                    <button type="button" class="DDBtn">
                        Others
                        <i class="fa fa-caret-down"></i>
                    </button>
                    <div class="DropDownList">
                         <a href="EventParticipant.aspx" id="EventBudgetTag2" runat="server" visible="false">Event Participant</a>
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
                                <%--<img src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
                --%><%--<asp:Button ID="Button_report_section" runat="server" OnClick="Button_report_section_Click" Text="Report" class="frame_style_4bar_Warranty" CausesValidation="False" />--%>
                
                <img id="img2" runat="server" src="RESOURCES/NavSeperator.png" class="line_frame_style_4bar" />
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
                <asp:UpdateProgress runat="server" ID="UpdateProgress3" class="gettext" AssociatedUpdatePanelID="upImg">
                    <ProgressTemplate>
                        <div class="loading">
                            <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <%--<div class="col-12 col-s-12">--%>
                    <asp:UpdatePanel ID="upImg" runat="server">
                        <ContentTemplate>
                            <!--Customer Info ////////////////////////////////////////////////////////////////////////////////////-->
                            <div id="new_applicant_section_CustomerInfo" runat="server" style="font-size:10px">
                            <asp:Button ID="Accordion_CustInfo" runat="server" Text="Customer Info" class="accordion_style_sub_fixed_darkcyan" />
                                <!--==============================================================================-->
                                <div class="col-12 col-sm-6" id="button_section" runat="server" visible="true">
                                    <div class="col-4 col-sm-3" id="labelAccount" runat="server">
                                        <asp:Label ID="Label_Account" class="inputtext" autocomplete="off" runat="server" OnTextChanged="CheckAcc" Style="margin-bottom: 4px"></asp:Label>
                                    </div>

                                    <div class="col-4 col-sm-3" id="cusButton" runat="server">
                                        <asp:Button ID="Button_CustomerMasterList" runat="server" OnClick="CheckAccInList" CausesValidation="false" Text="Find Customer" class="glow" />
                                    </div>
                                </div>

                                <div class="col-12 col-sm-12" id="div_CustInfoExtra" visible="false" runat="server" style="font-size:10px">
                                    <div class="col-12 col-sm-6"></div>
                                    
                                    
                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <asp:Label ID="lblAcc" Text="Application No. :" runat="server" class="labeltext" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="lblEquipID" runat="server" class="labeltext"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <asp:Label ID="lblSub" Text="Submission Date :" runat="server" class="labeltext" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-8 col-s-4">
                                            <asp:Label ID="lblSubDate" runat="server" class="labeltext"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <asp:Label ID="lblClaimStatus" runat="server" Text="Status :" class="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="lblClaimText" runat="server" Text="New" CssClass="labeltext"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Customer :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="Label_CustName" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>
                                    <asp:Label ID="hidden_alt_address_RecId" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_alt_address" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_alt_address_counter" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_Street" class="gettext" runat="server" Visible="false"></asp:Label>

                                    <div class="col-12 col-sm-6" id="warehouse" runat="server" style="float: right;">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">HQ / BR :        </label>
                                        </div>
                                         <div class="col-8 col-sm-4">
                                        <asp:Label ID="lblWarehouse" class="gettext" runat="server" Text=" "></asp:Label>
                                             </div>
                                    </div>

                                    <div class="col-12 col-sm-6 print-show" runat="server">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Account No. :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="Label_CustAcc" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Class :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="lblCustClass" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Tel / HP NO :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="lblTelNo" runat="server" Text=" "></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Salesman :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="Label_Salesman" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Contact Person :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="lblContactPerson" runat="server" Text=" "></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Acc Opened Date :        </label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="lblAccOpenedDate" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="hidden_inventLocationId" class="gettext" runat="server" Visible="false"></asp:Label><br />
                                    <asp:Label ID="hidden_Label_PreviousStatus" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_Label_NextStatus" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_LabelEquipID" class="gettext" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="hidden_LabelNextApprover" class="gettext" runat="server" Visible="false"></asp:Label> 
                                    <asp:HiddenField ID="hdSoNumber" runat="server" />
                                    <asp:HiddenField ID="hdInventId" runat="server" />
                                    <asp:HiddenField ID="hdSalesmanId" runat="server" />

                                    <div class="col-4 col-sm-8 print-hide" id="LastOutStandingWrapper" runat="server">
                                        <asp:Button ID="CheckLastOutstanding" runat="server" CausesValidation="false" Text="Check Last Outstanding Invoice" class="glow" OnClick="ActionButtons_Click" />
                                    </div>

                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>

                                            <div class="col-12 col-sm-6">
                                                <div class="col-4 col-sm-3">
                                                    <label class="labeltext"><span style="color: red">*</span> Item Type :</label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="*Please select Item Type" ControlToValidate="rblES" Display="Dynamic"
                                                        ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>

                                                <div class="col-8 col-sm-4">
                                                    <asp:RadioButtonList ID="rblES" runat="server" OnSelectedIndexChanged="rblES_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Equipment" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Signboard" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="rblES" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            <!--ItemDetails-->
                            </div>
                            <div runat="server" id="DetailsWrapper" visible="false" style="font-size:10px">
                            <asp:Button ID="Accordion_SignboardDetails" runat="server" Text="EOR/Signage/Supercede Case" class="accordion_style_sub_fixed_darkcyan" />
                                
                                <div id="new_applicant_section_ItemDetails" runat="server">
                                    <div class="row">
                                    <!-- Left Side: Request Type and Application Type -->
                                <div class="col-12 col-sm-12">
                                    <div class="col-12 col-sm-6">
                                       <div class="col-4 col-sm-3">
                    <label class="labeltext">Request Type :</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*Please select Item Type" ControlToValidate="rblES" Display="Dynamic"
                     ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>  
                </div>
                                       <div class="col-8 col-sm-4">
                    <asp:DropDownList ID="ddlRequestType" runat="server" class="dropdownlist">
                        <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                        <asp:ListItem Text="New" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Branch" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Existing" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Dealer" Value="4"></asp:ListItem>
                    </asp:DropDownList><br />
                    <br />
                </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                       <div class="col-4 col-sm-3">
                                        <label class="labeltext">Deposit for EOR/SC :</label>
                                        </div>
                                          <div class="col-8 col-sm-4">
                                             <asp:Label ID="lblDepositforEOR" class="gettext" runat="server" Text=" "></asp:Label>
                                         </div>
                                     </div>

                                </div>

                                <!-- Right Side: Deposit for EOR/SC and Deposit Amount / % -->
                                <div class="col-12 col-sm-12">
                                    <div class="col-12 col-sm-6" runat="server" id="SignboardApplicationType" style="text-align: left;">
                                    <div class="col-4 col-sm-3">
                                   <label class="labeltext"><span style="color: red">*</span> Application Type :</label>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*Please select Application Type" ControlToValidate="rblApplicationType" Display="Dynamic"
                                     ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                               </div>
                               <div class="col-8 col-sm-4">
                                   <asp:RadioButtonList ID="rblApplicationType" runat="server" RepeatDirection="Horizontal">
                                   <asp:ListItem Text="New Signboard" Value="1"></asp:ListItem>
                                  <asp:ListItem Text="Replace Hi-Rev" Value="2"></asp:ListItem>
                                  <asp:ListItem Text="Replace Other" Value="3"></asp:ListItem>
                              </asp:RadioButtonList>
                             </div>
            </div>

                                  <div class="col-12 col-sm-6">
                                    <div class="col-4 col-sm-3">
                                        <label class="labeltext">Deposit Amount / % :</label>
                                    </div>
                                    <div class="col-8 col-sm-4">
                                <label class="labeltext"> 0 </label>
                                </div>
                            </div>
                               </div>
                            </div>

                                <div class="col-12 col-sm-6" runat="server" id="EquipmentContractDuration">
                                <div class="col-4 col-sm-3">
                                    <label class="labeltext">Contract Duration :</label>
                                    </div>
                                 <div class="col-8 col-sm-4">
                                    <asp:Label ID="lblContractDuration" class="gettext" runat="server" Text=" "></asp:Label>
                                </div>
                            </div>
                            </div>
                                <asp:Panel runat="server" ID="SignboardWrapper" Visible="false">
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
                                                                            Style="min-width: 100%; max-width: 100%" AutoCompleteType="Search"></asp:TextBox>
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

                                                                <asp:TemplateField HeaderText="Size" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="TextBox_New_Size" class="inputtext_3" autocomplete="off" runat="server"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--                                                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="rbBtl" Text="" runat="server" Value="1" AutoPostBack="true" OnCheckedChanged="rbBtl_CheckedChanged" ForeColor="Black"></asp:RadioButton><br />
                                                                    <asp:RadioButton ID="rbAdd" Text="" runat="server" Value="2" AutoPostBack="true" OnCheckedChanged="rbAdd_CheckedChanged" ForeColor="Black"></asp:RadioButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>



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
                                                 <div class ="row">   
                                                <div class="col-12 col-sm-12">
                                                    <div class="col-12 col-sm-6"> 
                                                        <div class="col-4 col-sm-3"> </div>
                                                        <div class="col-8 col-sm-4"> </div>
                                                    </div>
                                                <div class="col-12 col-sm-6" style="margin-top: 10px; display: flex; justify-content: flex-end; align-items: center;">
                                                        <div class="col-4 col-sm-3"><asp:Label ID="Label_TotalCost" runat="server" Text="Total Cost:" CssClass="labeltext" ForeColor="Black" Style="margin-right: 10px;"></asp:Label> </div>
                                                        <div class="col-8 col-sm-4" style="display: flex; align-items: center;">
                                                        <span style="background: #f1f1f1; color: #333; border: 1px solid #ddd; padding: 5px 10px; border-radius: 4px 0 0 4px; font-weight: bold;">RM</span>
                                                        <asp:TextBox ID="TextBox_TotalCost" runat="server" CssClass="inputtext" Style="min-width: 150px; border-radius: 0 4px 4px 0;"></asp:TextBox>
                                                        </div>
                                                    </div>  
                                                </div>   </div>                                    
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <hr style="border: 1px thin #000; width: 100%;" hidden class="print-show" />
                                    <div class="footer">
                                        <div class="column print-show" id="signApproval" runat="server" hidden>
                                            <asp:Label ID="lblPrintedby" runat="server" Text="Returned By:" class="printed-by-label"></asp:Label>
                                            <div style="height: 70px;"></div>
                                            <hr style="border-bottom: 1px solid #000; text-align: left;" />
                                            <br />

                                            <div class="col-12 col-sm-12">
                                                <asp:Label ID="lblName" runat="server" Text="Name : "></asp:Label>
                                            </div>
                                            <br />

                                            <div class="col-12 col-sm-12">
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
                                    </div>
                                </asp:Panel>

                                <%--Delivery Details  ////////////////////////////////////////////////////////////////////////////////////--%>
                                <div id="new_applicant_sectionDeliveryDetails" runat="server" visible="false" class="col-12 c-s-12 row">
                                    <asp:Button ID="Accordion_DeliveryDetails" runat="server" Text="Delivery Details " class="accordion_style_sub_fixed_darkcyan" />

                                    <div class="col-12 col-sm-12">
                                        <div class="row">
                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                                <span style="color: red">*</span>
                                                <asp:Label ID="lblTransportName" runat="server" Text="Item Delivery To :" class="labeltext"></asp:Label>
                                            </div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" ControlToValidate="ddlItemDeliveryTo" Display="Dynamic"
                                                ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <div class="col-8 col-sm-4">
                                                <asp:DropDownList ID="ddlItemDeliveryTo" runat="server" class="dropdownlist">
                                                    <asp:ListItem Text="--- Please Select ---" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="PPM Warehouse/Salesman Deliver Via S/O" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Customer/Workshop" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Customer/Sub Dealer" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Customer/Branch" Value="4"></asp:ListItem>
                                                </asp:DropDownList><br />
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                                <span style="color: red">*</span>
                                                <asp:Label ID="lblStreet" runat="server" Text="Item Delivery Address :" class="labeltext"></asp:Label>
                                            </div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*" ControlToValidate="txtDeliveryAddress" Display="Dynamic"
                                                ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <div class="col-8 col-sm-4">
                                                <asp:TextBox ID="txtDeliveryAddress" runat="server" TextMode="MultiLine" Style="width: 100%; height: 80px;"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-12 col-sm-6">
                                      <div class="col-4 col-sm-3">
                                          
                                          <asp:Label ID="lblSBWorkshopName" runat="server" Text="Workshop Name :" class="labeltext"></asp:Label>
                                      </div>
                                      <div class="col-8 col-sm-4">
                                          <asp:TextBox ID="txtSBWorkshopName" runat="server" Style="width: 100%;" ></asp:TextBox>
                                      </div>
      
                                        </div>
                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                                <asp:Label ID="lblVehicle" runat="server" Text="Supplier/Payment Issue To :" class="labeltext"></asp:Label>
                                            </div>
                                            <div class="col-8 col-sm-4">
                                                <asp:DropDownList ID="ddlIssueTo" runat="server" class="dropdownlist">
                                                </asp:DropDownList><br />
                                                <br />
                                            </div>
                                        </div>


                                            </div>
                                    </div>

                                    <%--
                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <asp:Label ID="LabelTransportationRemark" runat="server" Text="Remark :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-6 col-sm-6">
                                            <asp:TextBox ID="txtTransportationRemark" class="txt_reason" TextMode="MultiLine"
                                                Row="3" Columns="20" runat="server" oninput="updateCharacterCount(this, 'characterCountWarning1')"></asp:TextBox>
                                            <span id="characterCountWarning1" style="color: red;"></span>
                                        </div>
                                    </div>--%>
                                    <div class="col-12 col-s-12 print-hide" id="pdf_section" runat="server" visible="false">
                                            <div class="col-12 cols-s-12">
                                                 <div class="col-2 col-sm-1">
                                                <asp:Label runat="server" Text="PDF:" CssClass="pdf-label"></asp:Label>
                                                 </div>
                                                    <div class="col-10 col-sm-10">
                                                    <asp:HiddenField ID="hdFilePath" runat="server" />
                                                        <div class="pdf-list">
                                             <asp:Repeater ID="pdfRepeater" runat="server">
                                                 <ItemTemplate>
                                                 <div class="pdf-grid-item">
                                                             <div class="pdf-card <%# Container.ItemIndex % 2 == 0 ? "even" : "odd" %>"> 
                                                            <a href='<%# Eval("Value") %>' target="_blank" class="pdf-link">
                                                                         <span><%# Eval("Text") %></span>
                                                                        </a>
                                                                <asp:Button ID="btnDeletePdf" runat="server" Text="Delete" 
                            CommandArgument='<%# Eval("Text") %>' 
                            OnCommand="DeletePdf" 
                            OnClientClick="return confirm('Are you sure you want to delete this PDF?');" 
                            CssClass="print-hide pdf-delete-btn"/>
                                                                </div>
                                                                </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                        </div>
                                                    </div>
                                            </div>
    
                                     </div>

                                    <%--display_section ///////////////////////////////////////////////////--%>

                                        <div class="col-12 col-s-12 print-hide" id="display_section" runat="server" visible="false">
                                            <div class="col-12 col-s-12">
                                                <div class="col-2 col-sm-1">
                                                    <asp:Label ID="lblDoc" runat="server" Text="Document:" CssClass="doc-label"></asp:Label>
                                                </div>
                                                <div class="col-10 col-sm-10">
    
                                                <asp:HiddenField ID="hdnImageToDelete" runat="server" />
    
                                        <div class="img-container"><br />
                                            <asp:Repeater ID="imageRepeater" runat="server">
                                                        <ItemTemplate>
                                                <div class="img-grid-item">
                                                <img src='<%# Eval("ImageUrl") %>' class="demo cursor" ondblclick="window.open('<%# Eval("ImageUrl") %>', '_blank')"
                         onclick='document.getElementById("<%= hdnImageToDelete.ClientID %>").value = "<%# Eval("ImageName") %>"; highlightSelectedImage(this)' />
                    <asp:Button ID="btnDeleteDisplayIMG" runat="server" Text="Delete" 
                                 CommandName="DeleteImage" OnCommand="DeleteImage"
                                 CommandArgument='<%# Eval("ImageName") %>'  
                                 OnClientClick="return confirm('Are you sure you want to delete this image?');" 
                                 CssClass="img-delete-btn"/>
                                                        </div>
                                                </ItemTemplate>
                                                </asp:Repeater>
                                                </div>

                                                </div>
                                            </div>
                                         </div>
                                    <%--upload_section ////////////////////////////////////////////////////////////////--%>
                                    
                                     <div class="col-12 col-s-12 print-hide" id="upload_section" runat="server">
                                    <div class="col-12 col-s-12">
                                        <div class="col-2 col-sm-1">
                                           <asp:Label ID="lblDisplay" runat="server" Visible="false" CssClass="labeltext" Text="Document:"></asp:Label>
                                            <asp:Label ID="lblimg" runat="server" Text="Upload Document:" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <div class="col-10 col-sm-10">
                                             <input type="file" runat="server" onchange="previewMultiple(event); upload(this);" id="fileUpload" multiple />
                                                <asp:RegularExpressionValidator ID="revFileUpoad" runat="server" ForeColor="Red" ControlToValidate="fileUpload" Display="Dynamic"
 ValidationExpression="^(.*?)\.((JPG)|(jpg)|(PNG)|(png)|(JPEG)|(jpeg)|(TIFF)|(tiff)|(PDF)|(pdf))$"
    ErrorMessage="Only jpg,png,jpeg,tiff,pdf are allowed!"></asp:RegularExpressionValidator>
                                        </div>
                                           </div>
                                    <div class="col-12 col-s-10">                                    
                                            <div id="gallery" class="gallery"></div>
                                        </div>
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" class="frame_style_type2" CausesValidation ="true"/>
                               
                                    </div>
                                   
                                </div>
                                <%--Signage License Status  ////////////////////////////////////////////////////////////////////////////////////--%>
                                <div id="SignageLicenseStatusWrapper" runat="server" visible="false" class="col-12 print-hide">
                                    <p id="accordionSignageLicenseStatus" runat="server" class="accordion_style_sub_fixed_darkcyan">Signage License Status<span style="color: red"> *</span></p>
                                    <div id="Div7" runat="server" class="mb-3">


                                        <div class="col-12">
                                            <asp:RadioButtonList ID="rblRequestSign" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="To pre-apply license" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="To install pending license" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Use existing license" Value="3"></asp:ListItem>

                                            </asp:RadioButtonList>
                                        </div>

                                        <p>(<span style="color: red">*</span> PPM will not be responsible for offence committed due to licensing.)</p>
                                    </div>
                                </div>
                                <%--Past 6 Months Purchase Record  ////////////////////////////////////////////////////////////////////////////////////--%> 
                               <div id="PastMonthPurchaseRecordWrapper" runat="server" visible="false" class="col-12">
                                   <p id="accordionPastMonthPurchaseRecord" runat="server" class="accordion_style_sub_fixed_darkcyan">Past 6 Months Purchase Record</p>
                                    <div id="Div8" runat="server" class="mb-3">
                                       <%-- <div class="col-12">--%>
                                            <asp:Label ID="lblPastRecord" runat="server"></asp:Label>
                                        <%--</div>--%>
                                    </div>
                               </div>
                                
                                <%--Location Map  ////////////////////////////////////////////////////////////////////////////////////--%>
                                <div id="LocationMapWrapper" runat="server" visible="false" class="col-12 c-s-12" style="font-size:10px">
                                    <asp:Button ID="accordionLocationMap" runat="server" Text="Location Map" class="accordion_style_sub_fixed_darkcyan" />
                                    <div id="Div13" runat="server" class="col-12 col-sm-12">
                                        
                                        <div id="DivDisplayMapA" class="col-12 col-sm-6" runat="server" visible="false">
                                        <div class="col-12 col-sm-4" style="border: 1px solid black; height: 140px; width: 250px;">
                                            <img src="RESOURCES/Location_Map_Type_A.png" alt="Map Type A Display" style="border: 1px solid black; height: 100%; width: 100%; object-fit: cover; display: block; margin: 0 auto;" />

                                        </div>
                                        </div>
                                        <div id="DivDisplayMapB" class="col-12 col-sm-6" runat="server" visible="false">
                                        <div class="col-12 col-sm-4" style="border: 1px solid black; height: 140px; width: 250px;">
                                            <img src="RESOURCES/Location_Map_Type_B.png" alt="Map Type B Display" style="border: 1px solid black; height: 100%; width: 100%; object-fit: cover; display: block; margin: 0 auto;" />

                                        </div>
                                        </div>
                                        <div id="DivDisplayMapC" class="col-12 col-sm-6" runat="server" visible="false">
                                        <div class="col-12 col-sm-4" style="border: 1px solid black; height: 140px; width: 250px;">
                                            <img src="RESOURCES/Location_Map_Type_C.png" alt="Map Type C Display" style="border: 1px solid black; height: 100%; width: 100%; object-fit: cover; display: block; margin: 0 auto;" />
                                        </div>

                                        </div>
                                            
                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext"><span style="color: red">*</span> Map Description :        </label>
                                            </div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtMapDescription" Display="Dynamic"
                                                ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <div class="col-8 col-sm-4">
                                                <asp:TextBox ID="txtMapDescription" class="gettext" runat="server" Text=""></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Traffic Density :        </label>
                                            </div>
                                            <div class="col-8 col-sm-4">
                                                <asp:TextBox ID="textTrafficDensity" class="gettext" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-8 col-sm-3" style="display: none">
                                                <asp:Label ID="txtTrafficDensity" class="gettext" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6">
                                            <%--<div runat="server" id="Div14">--%>
                                                <div class="col-4 col-sm-3">
                                                    <label class="labeltext">Map Remark :</label>

                                                </div>
                                                <div class="col-8 col-sm-4">
                                                    <asp:TextBox ID="txtMapRemark" class="gettext" runat="server" Text=""></asp:TextBox>
                                                </div>
                                            <%--</div>--%>
                                        </div>
                                        <div class="col-12 col-sm-6">
                                            <%--<div runat="server" id="Div15">--%>

                                                <div class="col-4 col-sm-3">
                                                    <label class="labeltext">Signboard Visibility :        </label>
                                                </div>
                                                <div class="col-8 col-sm-4">
                                                    <asp:TextBox ID="textSignboardVisibility" class="gettext" runat="server" Text="" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-8 col-sm-4" style="display: none">
                                                    <asp:Label ID="txtSignboardVisibility" class="gettext" runat="server" Text=""></asp:Label>
                                                </div>

                                            <%--</div>--%>
                                        </div>
                                        <div class="col-12 col-sm-6">
                                            <div class="col-4 col-sm-3">
                                            <asp:CheckBox ID="cbPhoneSubmission" runat="server" Text="Phone Submission" CssClass="" />
                                                </div>
                                        </div>
                                                
                                        
                                        
                                        <div class="col-12 col-sm-6 print-hide">
    <asp:Button ID="ButtonShowMap" runat="server" CausesValidation="false" Text="Select/View Application Map" class="glow" />
</div>
                                    </div>
                                </div>


                                <%--Comment from Sales Person  ////////////////////////////////////////////////////////////////////////////////////--%>
                                <div id="divCommentFromSalesPerson" runat="server" visible="false" class="col-12 col-s-12 row">
                                    <asp:Button ID="CommentFromSalesPersonSection" runat="server" Text="Comment from Sales Person" class="accordion_style_sub_fixed_darkcyan" />
                                    <div class="col-12 col-sm-12">
                                        <div class="row">
                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <span style="color: red">*</span>
                                            <asp:Label ID="LabelTypeOfServiceCenter" runat="server" Text="Type of Service Center :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ControlToValidate="txtTypeServiceCenter" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="txtTypeServiceCenter" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <span style="color: red">*</span>
                                            <asp:Label ID="LabelWorkshopFacilities" runat="server" Text="Workshop Facilities :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="txtWorkshopFacilities" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="txtWorkshopFacilities" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <span style="color: red">*</span>
                                            <asp:Label ID="LabelOwnerExperience" runat="server" Text="Owner Experience :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="txtOwnerExperience" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="txtOwnerExperience" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <span style="color: red">*</span>
                                            <asp:Label ID="Label4" runat="server" Text="Workshop Size/Type :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="txtWorkshopSizeType" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="txtWorkshopSizeType" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <span style="color: red">*</span>
                                            <asp:Label ID="Label6" runat="server" Text="No. of Mechanics :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="txtNumberOfMechanics" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="txtNumberOfMechanics" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-12 col-sm-6">
                                    <div class="col-4 col-sm-3">
                                        <span style="color: red">*</span>
                                        <label class="labeltext">Workshop Status :</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="rblWorkshopStatus" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>


                                    <div class="col-8 col-sm-4">
                                        <asp:RadioButtonList ID="rblWorkshopStatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Own" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Rental" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                     </div>
                                    <div class="col-12 col-sm-6">
                                        <div class="col-4 col-sm-3">
                                            <span style="color: red">*</span>
                                            <asp:Label ID="Label7" runat="server" Text="Year Of Establishment :" CssClass="labeltext"></asp:Label>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtYearOfEstablishment" Display="Dynamic"
                                            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                            <asp:TextBox ID="txtYearOfEstablishment" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-12 col-sm-6"></div>
                                <%--Process Status & Remarks  ////////////////////////////////////////////////////////////////////////////////////--%>

                                    <div id="divAllRemarks" class="col-12 col-sm-6" runat="server" visible="false"> 
                                          <div class="col-4 col-sm-3">
                                          <label class="labeltext">Previous Remarks :</label>
                                          </div>
                                         <div class="col-8 col-sm-4">
                                              <asp:Label ID="lblAllRemarks" runat="server" Width="400px"></asp:Label>
                                           </div>
                                         </div>
                                    <div class="col-12 col-sm-6"></div>
                                    <div id="divProcessStat" class="col-12 col-sm-6" runat="server" visible="false">
                                        <div class="col-4 col-sm-3">
                                            <label class="labeltext">Process Status :</label>
                                        </div>
                                        <div class="col-8 col-sm-4">
                                            <asp:Label ID="lblGetProcessStat" runat="server" Width="400px"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-12 col-sm-6"></div>
                                <div id="divMainRemarks" class="col-12 col-sm-6" runat="server">
                                        <div class="col-4 col-sm-3">
                                                    <span style="color: red">*</span>
                                                  <asp:Label ID="lblRemarks" runat="server" Text="Remarks :" class="labeltext"></asp:Label>
                                             </div>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="txtRemarks" Display="Dynamic"
            ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <div class="col-8 col-sm-4">
                                             <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                    </div>

                                    </div>
                                 </div>
                                </div>
                              </div>      
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="Button_Submit" />
                            <asp:PostBackTrigger ControlID="btnUpload" />
                            <%--                            <asp:PostBackTrigger ControlID="Button_BatteryList" />--%>
                            <%--<asp:PostBackTrigger ControlID="DropDownList_ReasonReturn_Battery" />--%>
                            <%--                            <asp:PostBackTrigger ControlID="fileUpload" />
                            <asp:PostBackTrigger ControlID="fileUpload2" />
                            <asp:PostBackTrigger ControlID="fileUpload3" />
                            <asp:PostBackTrigger ControlID="fileUpload4" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                <%--</div>--%>

                <div class="col-12 col-s-12 print-hide">
                    <asp:Button ID="Button_Submit" runat="server" Text="Submit" OnClick="Button_Submit_Click" class="frame_style_type2" CausesValidation="true" />
                    <asp:Button ID="Button_Proceed" runat="server" Text="Update" OnClick="Button_Proceed_Click" class="frame_style_type2" CausesValidation="false" />
                    <asp:Button ID="Button_Approve" runat="server" OnClick="Button_Approve_Click" Text="Approve" class="frame_style_type2" Visible="false" CausesValidation="false" />
                    <asp:Button ID="Button_Reject" runat="server" Text="Reject" class="frame_style_type2" OnClientClick="return openModal();" CausesValidation="false" />
                    <asp:Button ID="Button_Print" runat="server" Text="Print" OnClick="Button_Print_Click"  class="frame_style_type2" Visible="false" CausesValidation="false" />

                    <%--<asp:Button ID="btnAmend" runat="server" Text="Amend" OnClick="btnAmend_Click" CssClass="frame_style_type2" Visible="false" CausesValidation="false" />--%>
                    <%--<asp:Button ID="Button_CreateReturn" runat="server" OnClick="Button_CreateReturn_Click" Text="Create Return" class="frame_style_type2" Visible="false" CausesValidation="false" />--%>
                </div>

            </div>

            <!-- end of new_applicant_section==============================================================================
                    <!-- end of overview_section==============================================================================-->
            <div id="overview_section" runat="server" class="col-12 col-s-12" display="none">
                <div id="div4" runat="server" class="col-12 col-sm-12">
                    
                    <div runat="server" class="col-12 col-sm-10" id="divSearch">
                        <div class="col-4 col-sm-3">
                        <asp:Label ID="label18" runat="server" Text="Search:" CssClass="labeltext"></asp:Label>
                        <asp:TextBox ID="TextBox_Search_Overview" runat="server" OnTextChanged="txtboxListAll_TextChanged" CssClass="inputext" AutoPostBack="false" placeholder="Name/Account/Application No." />
                        </div>
                            
                        <div class="col-3 col-sm-1">
                        <asp:Button ID="btnListSearch" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Search" CausesValidation="false" Width="120px" />
                        </div>
                    </div>

                </div>
                <div id="divForSalesman" runat="server" visible="false" class="col-12 col-sm-9">
                    <div id="divPendingButton" runat="server" class="col-4 col-sm-3"> 
                        <asp:Button ID="btnListAll" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Pending" CausesValidation="false" Width="120px" />
                    </div>
                    <div id="divDraft" runat="server" class="col-4 col-sm-3">
                        <asp:Button ID="btnListDraft" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Draft" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblDraftBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="div1" runat="server" class="col-4 col-sm-3">
                        <asp:Button ID="btnListReject" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Rejected" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblRejectBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="div2" runat="server" class="col-4 col-sm-3">
                        <asp:Button ID="btnListApproved" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Approved" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblApprovedBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>
                </div>
                <div id="divForApproval" runat="server" visible="false" class="col-12 col-sm-9">

                    <div id="divHOD" runat="server" class="col-4 col-sm-3">
                        <asp:Button ID="btnListHOD" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Awaiting HOD" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblInvoiceChkBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="divSalesAdmin" runat="server" class="col-4 col-sm-3">
                        <asp:Button ID="btnListSalesAdmin" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Awaiting Sales Admin" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblTransporterBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div id="divAnP" runat="server" class="col-4 col-sm-3">
                        <asp:Button ID="btnListAnP" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Awaiting AnP" CausesValidation="false" />
                        <%--                        <asp:Label ID="LblGoodsReceivedBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                    </div>

                    <div class="col-12 col-s-12">
                        <div id="divSalesAdminMgr" runat="server" class="col-4 col-sm-3">
                            <asp:Button ID="btnListSalesAdminMgr" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Awaiting Sales Admin Mgr" CausesValidation="false" />
                            <%--                            <asp:Label ID="LblInspectionBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
                        </div>

                        <div id="divGM" runat="server" class="col-4 col-sm-3">
                            <asp:Button ID="btnListGM" runat="server" OnClick="ActionButtons_Click" class="custom-button frame_style_type2" Text="Awaiting GM" CausesValidation="false" />
                            <%--                            <asp:Label ID="LblVerifyBadge" runat="server" CssClass="custom-badge"></asp:Label>--%>
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
                                <asp:ListItem Text="Equipment ID" Value="0"></asp:ListItem>
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
                                <asp:ListItem Text="Awaiting HOD" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Sales Admin" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Awaiting AnP" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Awaiting Sales Admin Mgr" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Awaiting GM" Value="8"></asp:ListItem>
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
                        <%--<asp:GridView ID="GridViewOverviewList" runat="server"
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
                                <asp:TemplateField HeaderText="Equipment ID">
                                    <ItemTemplate>
                                        <asp:Button ID="Button_EquipmentID" runat="server" OnClick="Button_EquipId_Click" CausesValidation="false" Text='<%# Eval("EquipID") %>' class="button_grid" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="CustomerAccount" HeaderText="Customer Account" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="FormType" HeaderText="Form Type" />
                                <asp:BoundField DataField="DepositType" HeaderText="Deposit Type" />
                                <asp:BoundField DataField="DocStatus" HeaderText="Doc Status" />
                                <asp:BoundField DataField="AppliedDate" HeaderText="Applied Date" />
                                <asp:BoundField DataField="NextApprover" HeaderText="Next Approver" />
                                <asp:BoundField DataField="AdminDate" HeaderText="Admin Date" />
                                <asp:BoundField DataField="AnPDate" HeaderText="AnP Date" />
                                <asp:BoundField DataField="ManagerDate" HeaderText="Manager Date" />
                                <asp:BoundField DataField="GMDate" HeaderText="GM Date" />
                                <asp:BoundField DataField="ProcessStatus" HeaderText="Process Status" />
                            </Columns>
                        </asp:GridView>--%>
                        <asp:GridView ID="GridViewOverviewList" runat="server"
    PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid" AllowCustomPaging="true"
    HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="No."
    AllowPaging="True" OnPageIndexChanging="datagrid_PageIndexChanging_Overview"
    HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
    <Columns>
        <asp:TemplateField HeaderText="No.">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Application No.">
            <ItemTemplate>
                <asp:Button ID="Button_EquipmentID" runat="server" OnClick="Button_EquipId_Click" CausesValidation="false" Text='<%# Eval("EquipID") %>' class="button_grid" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Account No.">
            <ItemTemplate>
                <asp:Label ID="Label_CustomerAccount" runat="server" Text='<%# Eval("CustomerAccount") %>' Style="color:black; text-decoration:none; cursor:default;"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name" ItemStyle-Width="10%">
            <ItemTemplate>
                <%# Eval("CustomerName") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Form Type" Visible="false">
            <ItemTemplate>
                <%# Eval("FormType") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Deposit Type" Visible="false">
            <ItemTemplate>
                <asp:Label ID="Label_DepositType" runat="server" Text='<%# Eval("DepositType") %>' Style="color:black; text-decoration:none; cursor:default;"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                 <asp:Label ID="Label_DocStatus" runat="server" Text='<%# Eval("DocStatus") %>' Style="color:black; text-decoration:none; cursor:default;"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Applied Date" Visible="false">
            <ItemTemplate>
                <%# Eval("AppliedDate") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Approver">
            <ItemTemplate>
                <asp:Label ID="Label_NextApprover" runat="server" Text='<%# Eval("NextApprover") %>' Style="color:black; text-decoration:none; cursor:default;"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Admin Date" Visible="false">
            <ItemTemplate>
                <%# Eval("AdminDate") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="AnP Date" Visible="false">
            <ItemTemplate>
                <%# Eval("AnPDate") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Manager Date" Visible="false">
            <ItemTemplate>
                <%# Eval("ManagerDate") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="GM Date" Visible="false">
            <ItemTemplate>
                <%# Eval("GMDate") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Process Status" Visible="false">
            <ItemTemplate>
                <%# Eval("ProcessStatus").ToString().Replace(Environment.NewLine, "<br/>") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Applied By" Visible="false">
            <ItemTemplate>
                <asp:Label ID="Label_AppliedBy" runat="server" Text='<%# Eval("AppliedBy") %>' Style="color:black; text-decoration:none; cursor:default;"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Sales Rep.">
            <ItemTemplate>
                 <asp:Label ID="Label_EmplName" runat="server" Text='<%# Eval("EmplName") %>' Style="color:black; text-decoration:none; cursor:default;"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
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
            <%--Enquiries_section///////////////////////////////////////////////////////////////////////////////////--%>
            <div id="enquiries_section" runat="server" class="col-12 col-s-12" style="display:block;">
                <!-- Add New Button above the grid -->
                <div class="form-group">
                     <asp:Button ID="btnAddNew" runat="server" Text="Add New Item" 
                   CssClass="btn btn-success mb-3" OnClick="btnAddNew_Click" />
                 </div>
<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvIssueTo" runat="server" DataKeyNames="ID" PagerStyle-CssClass="page-link"
                PageSize="20" HorizontalAlign="Left" AllowCustomPaging="false"
                CssClass="mydatagrid" AllowPaging="True" EmptyDataText="No records found." 
                OnPageIndexChanging="dtgvIssueTo_PageIndexChanging" EnableSortingAndPagingCallbacks="true" 
                HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False" 
                OnSelectedIndexChanged="gvIssueTo_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="Type" HeaderText="Type" />
                    <asp:BoundField DataField="IssueTo" HeaderText="Issued To" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:CommandField ShowSelectButton="True" SelectText="Edit" />
                </Columns>
                <HeaderStyle CssClass="header" />
                <PagerSettings PageButtonCount="2" />
                <RowStyle CssClass="rows" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvIssueTo" EventName="PageIndexChanging" />
            <asp:AsyncPostBackTrigger ControlID="gvIssueTo" EventName="Sorting" />
            <asp:AsyncPostBackTrigger ControlID="gvIssueTo" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>                    
     <!-- Edit Form Section -->
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divIssueTo_details" runat="server" visible="false"> <!-- Initially hidden -->
                <div class="form-group">
                    <label for="txtIssueTo">Issue To:</label>
                    <asp:TextBox ID="txtIssueTo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="labelStatus">Status:</label>
                    <asp:DropDownList ID="ddlStatus_IssueTo" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Disable"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:HiddenField ID="hdnID" runat="server" />
                    <asp:Button ID="btnSave_IssueTo" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_IssueTo_Click" />
                    <asp:Button ID="btnCancel_IssueTo" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_IssueTo_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
            </div>
            <!--Approval_section////////////////////////////////////////////////////////////////////////////////////-->
            <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext">
                <ProgressTemplate>
                    <div class="loading">
                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

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


        <div id="ReceiptModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-body" style="align-content: center; padding: 10px 30px 0px 30px; font-size: 12px; overflow: auto; height: 600px">
                                <span></span>
                                <asp:Button ID="Button1" runat="server" Text="Customer" class="accordion_style_sub_fixed_darkcyan" />
                                <div id="Div3" runat="server">
                                    <!--==============================================================================-->


                                    <div id="div9" visible="true" runat="server">
                                        <div class="col-12 col-sm-12 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Salesman Code / Name:        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalSalesmanCode" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Customer Code / Name:        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalCustomerCode" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Cust Class :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalCustClass" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Contact Person :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalContactPerson" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-12 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Cust Main Group :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalCustGroup" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-12 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Telephone :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalTelephone" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2" id="Div10" runat="server" style="float: right;">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Customer Type :        </label>
                                            </div>
                                            <asp:Label ID="modalCustType" class="gettext" runat="server" Text=" "></asp:Label>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2 print-show" runat="server">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">EOR Target (RM) :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalEORTarget" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Expiry :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalExpiry" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>


                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Target Carton :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalTargetCarton" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Commence :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalCommence" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Average Payment Day (6 mth) :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalAvgPayment6Mth" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Incorporation Date :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalIncorporationDate" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>


                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Average monthly sales (6 mth) :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalAvrgMonthlySales6Mth" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Account opened date :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalAccOpenDate" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Credit Limit :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalCreditLimit" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Overdue Interest :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalOverdueInterest" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Approval for credit :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalApprovalForCredit" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Last Month Collection :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalLastMonthCollection" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Bank/Corp guarantee :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalBankGuarantee" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Current Month Collection :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalCurrentMonthCollection" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Bank/Corp expiry date :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalBankExpiryDate" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Posted dated cheque total :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalPostedDatedChequeTotal" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Last return cheque :        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="modalLastReturnCheque" class="gettext" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-12 col-s-12" style="text-align: center">  
                                            <div class="col-11 col-s-6">
                                                <asp:Label ID="lblOutstanding" runat="server" Text="Last Outstanding :" CssClass="labeltext" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblDays" runat="server" ForeColor="Red" Text="0 days"></asp:Label>
                                            </div>
                                        </div>
                                        <%--<div class="col-12 col-sm-12 d-flex justify-content-center" style="font-size: 15px; color: red">
                                            <b>Last Outstanding : <span runat="server" id="spanOutstandingDays"></span>days</b>
                                        </div>--%>


                                        <div class="col-12 col-sm-12 mt-5">
                                            <div id="Div6" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%; display:flex; justify-content:center;">
                                                <asp:GridView ID="GridView1" runat="server"
                                                    PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid" AllowCustomPaging="false"
                                                    HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="No."
                                                    AllowPaging="false" OnPageIndexChanging="datagrid_PageIndexChanging_Overview"
                                                    HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="No.">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="Voucher" HeaderText="Voucher" />
                                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No." />
                                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                                        <asp:BoundField DataField="AmountCurrency" HeaderText="Amount Currency" />
                                                        <asp:BoundField DataField="Currency" HeaderText="Currency" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" />
                                                        <asp:BoundField DataField="TotalOutStandingDay" HeaderText="Total Outstanding Day" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Label ID="Label8" runat="server" CssClass="labeltext" Visible="false"></asp:Label>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                    <div class="modal-footer" style="display: flex; justify-content: center">
                        <div class="row" style="display: flex; justify-content: center;">
                            <div>
                                <button id="BtnCloseModal" type="button" class="glow_green">Close</button>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div id="MapModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="modal-body" style="align-content: center; padding: 10px 30px 0px 30px; font-size: 12px; overflow-y: auto;">
                                <span></span>
                                <div id="Div11" runat="server" class="mb-3">
                                    <!--==============================================================================-->


                                    <div id="div12" visible="true" runat="server">
                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Selection:        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:Label ID="txtModalSelection" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-6 col-sm-2 mb-2">
                                            <div class="col-8 col-sm-8">
                                                <label class="labeltext">Traffic Density :</label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*Please select Traffic Density" ControlToValidate="rblModalTrafficDensity" Display="Dynamic"
                                                    ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>


                                            <div class="col-8 col-sm-8">
                                                <asp:RadioButtonList ID="rblModalTrafficDensity" runat="server" RepeatDirection="Vertical">
                                                    <asp:ListItem Text="Low" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Fair" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="High" Value="3"></asp:ListItem>

                                                </asp:RadioButtonList>
                                            </div>
                                        </div>

                                        <div class="col-6 col-sm-2 mb-2">
                                            <div class="col-8 col-sm-8">
                                                <label class="labeltext">Signboard Visibility :</label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="*Please select Signboard Visibility" ControlToValidate="rblModalSignboardVisibility" Display="Dynamic"
                                                    ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-8 col-sm-8">
                                                <asp:RadioButtonList ID="rblModalSignboardVisibility" runat="server" RepeatDirection="Vertical">
                                                    <asp:ListItem Text="Low" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Fair" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="High" Value="3"></asp:ListItem>

                                                </asp:RadioButtonList>
                                            </div>
                                        </div>

                                        <div class="col-6 col-sm-2 mb-2">
                                            <div class="col-8 col-sm-8">
                                                <label class="labeltext">Photo Submission :</label>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="*Please select Photo Submission" ControlToValidate="rblModalPhotoSubmission" Display="Dynamic"
                                                    ValidationGroup="submit" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-8 col-sm-8">
                                                <asp:RadioButtonList ID="rblModalPhotoSubmission" runat="server" RepeatDirection="Vertical">
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>

                                                </asp:RadioButtonList>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-6 mb-2">
                                            <div class="col-4 col-sm-3">
                                                <label class="labeltext">Remark:        </label>
                                            </div>
                                            <div class="col-8 col-sm-5">
                                                <asp:TextBox ID="txtModalMapRemark" runat="server" Text=" "></asp:TextBox>
                                            </div>
                                        </div>


                                    </div>

                                    <div style="border-bottom: 1px solid black; display: flex; width: 100%">
                                    </div>

                                    <div style="border: 1px solid black; width: 100%" class="m-1">
                                        <div class="col-12 col-sm-4" style="border: 1px solid black; height: 222px;">
                                            <div class="col-12" style="border: 3px solid black;">
                                                <p>Type : A</p>
                                            </div>
                                            <div class="col-12 d-flex justify-content-center mb-2">
                                                <div class="col-4 modal-map">
                                                    <b style="font-size: large; cursor: pointer">A1</b>
                                                </div>
                                                <div class="col-4 modal-map">
                                                    <b style="font-size: large; cursor: pointer">A2</b>
                                                </div>
                                                <div class="col-4 modal-map">
                                                    <b style="font-size: large; cursor: pointer">A3</b>
                                                </div>
                                            </div>

                                            <div class="col-12 mb-2" style="background-color: black; height: 40px; align-content: center;">
                                                <div class="col-12" style="border-bottom: 7px dashed white">
                                                </div>
                                            </div>

                                            <div class="col-12 d-flex justify-content-center mb-2">
                                                <div class="col-4 modal-map">
                                                </div>
                                                <div class="col-4 modal-map">
                                                </div>
                                                <div class="col-4 modal-map">
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-4" style="border: 1px solid black; height: 222px;">
                                            <div class="col-12" style="border: 3px solid black;">
                                                <p>Type : B</p>
                                            </div>
                                            <div class="col-5 mb-2">
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                    <b style="font-size: large; cursor: pointer">B1</b>
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                    <b style="font-size: large; cursor: pointer">B2</b>
                                                </div>
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                    <b style="font-size: large; cursor: pointer">B3</b>
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                    <b style="font-size: large; cursor: pointer">B4</b>
                                                </div>
                                            </div>

                                            <div class="col-2" style="background-color: black; height: 62px; display: flex; justify-content: center;">
                                                <div class="col-2" style="border-left: 7px dashed white">
                                                </div>
                                            </div>

                                            <div class="col-5 mb-2">
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                </div>
                                            </div>

                                            <div class="col-12" style="background-color: black; height: 40px; align-content: center;">
                                                <div class="col-12" style="border-bottom: 7px dashed white">
                                                </div>
                                            </div>

                                            <div class="col-5 mb-2">
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                </div>
                                            </div>

                                            <div class="col-2" style="background-color: black; height: 62px; display: flex; justify-content: center;">
                                                <div class="col-2" style="border-left: 7px dashed white">
                                                </div>
                                            </div>

                                            <div class="col-5 mb-2">
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="ms-2 col-5 modal-map" style="height: 25px">
                                                </div>
                                                <div class="col-5 modal-map" style="height: 25px">
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-12 col-sm-4" style="border: 1px solid black; height: 222px;">
                                            <div class="col-12" style="border: 3px solid black;">
                                                <p>Type : C</p>
                                            </div>
                                            <div class="col-12 mb-1">
                                                <div class="col-4 modal-map" style="height: 35px">
                                                    <b style="font-size: large; cursor: pointer">C1</b>
                                                </div>
                                                <div class="col-4 modal-map" style="height: 35px">
                                                    <b style="font-size: large; cursor: pointer">C2</b>
                                                </div>
                                                <div class="col-4 modal-map" style="height: 35px">
                                                    <b style="font-size: large; cursor: pointer">C3</b>
                                                </div>
                                            </div>

                                            <div class="col-12" style="background-color: black; height: 40px; align-content: center;">
                                                <div class="col-12" style="border-bottom: 7px dashed white">
                                                </div>
                                            </div>

                                            <div class="col-5">
                                                <div class="ms-2 col-11 modal-map" style="height: 65px">
                                                    <b style="font-size: large; cursor: pointer">C4</b>
                                                </div>
                                            </div>

                                            <div class="col-2" style="background-color: black; height: 62px; display: flex; justify-content: center;">
                                                <div class="col-2" style="border-left: 7px dashed white">
                                                </div>
                                            </div>

                                            <div class="col-5">
                                                <div class="ms-2 col-11 modal-map" style="height: 65px">
                                                    <b style="font-size: large; cursor: pointer">C5</b>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>
                    <div class="modal-footer" style="display: flex; justify-content: center">
                        <div class="row" style="display: flex; justify-content: center;">
                            <div>
                                <button id="BtnCloseModal1" type="button" class="glow_green">Close</button>
                                <asp:Button ID="BtnMapModalDone" runat="server" CssClass="glow_green" Text="Done" OnClick="YourSubmitButton_Click" />
                            </div>

                        </div>
                    </div>
                </div>
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


            $(document).on('click', '#CheckLastOutstanding', function () {
                $('#<%= UpdateProgress.ClientID %>').show();
                $('#ReceiptModal').show();
            });

            $('#BtnCloseModal').on('click', function () {
                $('#ReceiptModal').hide();

            })

            $(document).on('click', '#ButtonShowMap', function () {

                $('#MapModal').show();
                $('#<%= UpdateProgress.ClientID %>').hide();
            })

            $('#BtnCloseModal1').on('click', function () {
                $('#MapModal').hide();

            })

            $('#BtnMapModalDone').on('click', function () {

                if (mapModalValidate()) {
                    var selection = $('#txtModalSelection').text();
                    var modalTrafficDensity = $('input[type="radio"][name="rblModalTrafficDensity"]:checked').val()
                    var modalSignboardVisibility = $('input[type="radio"][name="rblModalSignboardVisibility"]:checked').val()
                    var modalPhotoSubmission = $('input[type="radio"][name="rblModalPhotoSubmission"]:checked').val()
                    var mapRemark = $('#txtModalMapRemark').val();
                    var textTrafficDensity = $('input[type="radio"][name="rblModalTrafficDensity"]:checked').parent().find("label").text();
                    var textSignboardVisibility = $('input[type="radio"][name="rblModalSignboardVisibility"]:checked').parent().find("label").text();

                    $('#txtMapDescription').val(selection);
                    $('#txtTrafficDensity').text(modalTrafficDensity);
                    $('#textTrafficDensity').val(textTrafficDensity);
                    $('#txtMapRemark').val(mapRemark);
                    $('#txtSignboardVisibility').text(modalSignboardVisibility);
                    $('#textSignboardVisibility').val(textSignboardVisibility);
                    if (modalPhotoSubmission == "1") {
                        $('#cbPhoneSubmission').prop("checked", true);

                    } else {
                        $('#cbPhoneSubmission').prop("checked", false);

                    }
                    $('#MapModal').hide();
                } else {
                    alert("Please fill in all the input fields.");
                }

            })

            function mapModalValidate() {
                isValidate = true;


                if ($('#txtModalSelection').text() == "" || $('input[type="radio"][name="rblModalTrafficDensity"]:checked').val() == undefined || $('input[type="radio"][name="rblModalSignboardVisibility"]:checked').val() == undefined || $('input[type="radio"][name="rblModalPhotoSubmission"]:checked').val() == undefined) {
                    isValidate = false;
                }

                return isValidate;
            }

            $(document).on('click', '.modal-map', function () {
                var data = $(this).find('b').text();

                $('#txtModalSelection').text(data)
            })

            function validateNumber(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
                    && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
            
            function showPopup(popupName) {
                const popup = document.querySelector(`[popup-name="${popupName}"]`);
                if (popup) {
                    popup.style.display = 'block';
                }
                return false;
            }
            function closePopup(popupName) {
                const popup = document.querySelector(`[popup-name="${popupName}"]`);
                if (popup) {
                    popup.style.display = 'none';
                }
                return false;
            }
        </script>
    </form>
</body>
</html>

