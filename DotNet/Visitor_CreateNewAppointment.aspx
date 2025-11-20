<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visitor_CreateNewAppointment.aspx.cs" Inherits="DotNet.Visitor_CreateNewAppointment" %>

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
    <script src="../scripts/jquery/jquery.min.js" type="text/javascript"></script>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.bundle.min.js"></script>

    <link href="../Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/bootstrap-datepicker.js" type="text/javascript"></script>
    <link href="../Content/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <script src="../scripts/moment.min.js"></script>
    <script src="../scripts/bootstrap-datetimepicker.js"></script>


    <title>Create New Appointment</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <script src="scripts/BrowserReload_ThroughHistory.js"></script>
    <link href="STYLES/xtra_top_navigation.css" rel="stylesheet" />
    <script src="scripts/top_navigation.js"></script>

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

        .CurrentPage {
            background-color: #b2b2b2 !important;
        }

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

            .column {
                float: left;
                width: 50%;
                padding: 10px;
                height: 300px;
            }

            .col-p-6 {
                width: 100%;
            }


            /* Additional styles for proper printing layout */
        }

        .required:after {
            content: " *";
            color: red;
        }

        .mb-1 {
            margin-bottom: 10px !important;
        }

        .serial-number-style {
            text-decoration: underline;
            font-weight: 900;
            font-size: 30px;
        }

        .bordered-container {
            border: 2px solid #007bff; /* Sets a solid border around the container */
            padding: 20px; /* Adds padding inside the border */
            margin: 20px 0; /* Adds margin to separate it from other elements */
            border-radius: 8px; /* Optional: rounds the corners of the border */
            background-color: #f9f9f9; /* Optional: adds a light background color */
        }
    </style>
</head>
<script type="text/javascript">
    function initializeDatepicker() {
        $('#txtFromDate').datetimepicker({
            format: 'DD/MM/yyyy HH:mm',
            sideBySide: true,
            minDate: moment().format("MM/DD/yyyy HH:mm")
        });

        $('#txtToDate').datetimepicker({
            format: 'DD/MM/yyyy HH:mm',
            sideBySide: true, // shows calendar and time picker side by side
            minDate: moment().format("MM/DD/yyyy HH:mm")

        });

    }

</script>
<body onload="initializeDatepicker()">

    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="mobile_hidden">
            <div class="col-3 col-s-3 image_icon">
                <img src="RESOURCES/CreateNew.png" class="image_resize" />
            </div>

            <div class="col-9 col-s-9 image_title">
                <h1 style="font-weight: bold">Create New Appointment</h1>
            </div>
        </div>
        <!--==============================================================================-->
        <div class="topnav print-hide" id="myTopnav">

            <a href="Visitor_MainMenu.aspx">Home</a>
            <a id="NewAppointment" href="Visitor_CreateNewAppointment.aspx" runat="server" visible="false" class="CurrentPage">New Appointment</a>
            <a id="AppointmentHistory" href="Visitor_History.aspx" runat="server">Appointment History</a>
            <a id="SFA" href="#" class="sfa" runat="server" visible="false">SFA</a>

            <a href="LoginPage.aspx" id="BtnLogOut" class="Log_Out top_nav_logout_properties">
                <%--                <i class="fa fa-sign-out" aria-hidden="true" style="font-size:35px"></i>--%>
                <asp:Label runat="server" CssClass="fa fa-sign-out" Style="font-size: 20px;"></asp:Label>
                <asp:Label runat="server" Text="Logout" Font-Bold="true"></asp:Label>
                <%--                    <img src="RESOURCES/LogoutIcon.png" /> Logout--%>
                <%--                    <img src="RESOURCES/LogOut_h.png" class="top_nav_logout_highlight" />--%>
            </a>
            <a href="javascript:void(0);" class="icon" onclick="topnavigation()">
                <%--                                <img src="RESOURCES/menu.png" />--%>
                <div class="global_container" onclick="myFunction(this);">
                    <div class="bar1"></div>
                    <div class="bar2"></div>
                    <div class="bar3"></div>
                </div>
            </a>

        </div>
        <div id="AddUser_section" style="display: initial" runat="server">

            <asp:UpdateProgress runat="server" ID="UpdateProgress2" class="gettext" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div class="loading">

                        <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                    </div>

                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>


                    <asp:Panel ID="FormWrapper" runat="server" CssClass="button-container formWrapper" class="p-3">

                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext required">Visitor Name:      </label>
                            </div>
                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="VisitorName" class="inputtext" runat="server"></asp:TextBox>

                            </div>

                        </div>

                        <div class="">
                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">Visitor Company:      </label>
                                </div>

                                <div class="col-4 col-s-4">
                                    <asp:TextBox ID="VisitorCompany" class="inputtext" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">Lion Posim Group of Companies:      </label>
                                </div>

                                <div class="col-4 col-s-4">
                                    <asp:DropDownList ID="ddlLPGCompany" runat="server" class="dropdownlist" Style="font-size: 12px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-12 col-s-12">
                                <div class="col-2 col-s-6">
                                    <label class="labeltext required">Number of Visitors:      </label>
                                </div>
                                <div class="col-4 col-s-4">
                                    <asp:TextBox ID="NumberOfVisitors" class="inputtext" runat="server"></asp:TextBox>
                                </div>
                                <asp:RegularExpressionValidator ID="revNOV"
                                    ControlToValidate="NumberOfVisitors" runat="server"
                                    ErrorMessage="Only Numbers allowed"
                                    ValidationExpression="\d+">
                                </asp:RegularExpressionValidator>
                            </div>
                            <div class="col-12 col-s-12" id="dateRange_section" runat="server">
                                <div class="col-2 col-s-2">
                                    <asp:Label ID="lbldate" runat="server" CssClass="labeltext required" Text="Date Range: "></asp:Label>
                                </div>
                                <div class="col-3">
                                    <div class="">
                                        <div class="form-group">
                                            <div class="input-group date" id="txtFromDateTimePicker">
                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="Select Date and Time" Style="font-size: 12px"></asp:TextBox>
                                                <span class="input-group-addon" style="display: flex; justify-content: center; align-items: center;">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--                                <asp:RequiredFieldValidator runat="server" id="reqDateTime" controltovalidate="txtFromDate" errormessage="Please enter a value!" />--%>

                                <div class="col-3" style="margin-left: 5px">
                                    <div class="form-group">
                                        <div class="input-group date" id="TimeDurationWrapper">

                                            <asp:DropDownList ID="ddlTimeDuration" CssClass="form-control" runat="server" class="dropdownlist" Style="font-size: 12px"></asp:DropDownList>
                                            <span class="input-group-addon" style="display: flex; justify-content: center; align-items: center;">
                                                <span class="glyphicon glyphicon-time"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-12 col-s-12" style="display: flex; justify-content: center;">
                            <asp:Button ID="CheckAvailableLocation" runat="server" OnClick="AvailableLocationDropdown" Text="Check Location Availability" class="glow_green button-margin" Style="font-size: 12px" Enabled="true" />
                        </div>

                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext required">Available Location:      </label>
                            </div>

                            <div class="col-4 col-s-4">
                                <div class="col-12 col-s-12">
                                    <asp:DropDownList ID="ddlAvailableLocation" runat="server" class="dropdownlist"></asp:DropDownList>
                                </div>
                            </div>
                        </div>



                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext required">Vehicle Plate No:      </label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="VehiclePlateNo" class="inputtext" runat="server" Text=" "></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext required">Person To Meet:      </label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="PersonToMeet" class="inputtext" runat="server" Text=" "></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12 col-s-12">
                            <div class="col-2 col-s-6">
                                <label class="labeltext required">Purpose:      </label>
                            </div>

                            <div class="col-4 col-s-4">
                                <asp:TextBox ID="Purpose" class="inputtext" runat="server" Text=" "></asp:TextBox>
                            </div>
                        </div>


                        <div class="col-12 col-s-12" style="display: flex; justify-content: center; margin-top: 20px;">
                            <asp:Button ID="BtnBack" runat="server" OnClick="BackToMainMenu" Text="Back" class="glow_green button-margin" Enabled="true" />
                            <%--<asp:Button ID="BtnSFARegistration" runat="server" OnClick="Proceed" Text="Add" class="glow_green" Enabled="true" />--%>
                            <asp:Button ID="BtnSubmit" runat="server" OnClick="CreateNewAppointment" Text="Proceed" class="glow_green button-margin BtnOpenModal" Enabled="true" />
                            <%--                            <button id="BtnOpenModal" class="glow_green">Proceed</button>--%>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="ReceiptModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-body" style="align-content: center; padding: 10px 30px 0px 30px;font-size: 12px;">
                                <span></span>
                                <h3 style="text-align: center;">This is your Appointment ID, please keep it handy:
                            <asp:Label ID="Label_SerialNumber" class="gettext serial-number-style" runat="server"></asp:Label></h3>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Full Name:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_FullName" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Visitor Company:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_VisitorCompany" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Lion Posim Group of Companies:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_LPGCompany" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Number of Visitors:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_Visitors" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Date Visit:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_DateVisit" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Vehicle Plate No:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_VehiclePlateNo" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Person To Meet:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_PersonToMeet" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Purpose:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_Purpose" class="gettext" runat="server" Text=" "></asp:Label>
                                    </div>
                                </div>

                                <div class="row mb-1">
                                    <div class="col-3 col-s-4">
                                        <label class="labeltext">Location:        </label>
                                    </div>
                                    <div class="col-6 col-s-8">
                                        <asp:Label ID="Label_Location" class="gettext" runat="server" Text=" "></asp:Label>
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

    </form>
</body>

</html>


<script>
    window.onload = function () {
        initializeDatepicker();

        $(document).on('click', '.BtnOpenModal', function () {
            //$('#ReceiptModal').show();
            initializeDatepicker();
        })

        $(document).on('click', '#BtnCloseModal', function () {
            initializeDatepicker();
            $('#ReceiptModal').hide();

            window.location.reload();
        })

        $(document).on('click', '#txtFromDate', function () {
            initializeDatepicker();
        })

        $(document).on('click', '.glyphicon', function () {
            initializeDatepicker();

        })

        $(document).on('click', '#CheckAvailableLocation', function () {
            initializeDatepicker()

        })

        $(document).on('click', '#ddlAvailableLocation', function () {
            initializeDatepicker()
        })

        $('.sfa').on('click', function () {
            var userName = '<%= Session["UserName"] %>';
            var productionWebsite = '<%= GLOBAL_VAR.GLOBAL.ProductionWebsite %>';

            $.ajax({
                type: "POST",
                url: "/MainMenu.aspx/RedirectToMainMenu",
                data: JSON.stringify({ UserName: userName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    window.location.href = productionWebsite + "MainMenu.aspx";
                }
            });
        })
    }
</script>
