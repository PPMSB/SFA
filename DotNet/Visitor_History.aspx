<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visitor_History.aspx.cs" Inherits="DotNet.Visitor_History" EnableEventValidation="false" %>

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

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/css/bootstrap-datetimepicker.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <%--    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>
    <link href="STYLES/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <%--    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datetimepicker/4.17.47/js/bootstrap-datetimepicker.min.js"></script>--%>
    <script src="scripts/bootstrap-datetimepicker.js"></script>
    <title>Appointment History</title>
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

        .btn {
            border: 1px solid black;
            background-color: #eeeeee;
            border-radius: 20px;
            font-weight: 800;
        }

        .fit-content {
            max-width: fit-content;
            min-width: fit-content;
        }

        .aware-color {
            margin-bottom: 15px;
            display: flex;
            align-items: flex-end;
            justify-content: flex-start;
            margin-bottom: 15px
        }

        #AppointmentRecordSection td {
            font-size: 15px;
            padding-left: 5px;
            padding-right: 5px;
        }

        .btn-checkout {
            font-size: 13px;
            margin-bottom: 5px;
            background-color: #9E9E9E;
            color: white
        }

        .btn-checkin {
            font-size: 13px;
            margin-bottom: 5px;
            background-color: #4CAF50;
            color: white
        }

        .btn-cancel {
            font-size: 13px;
            background-color: #F44336;
            color: white
        }

        .wide-column {
            min-width: 80px;
        }
    </style>
</head>
<script type="text/javascript"> 

    function initializeDatepicker() {
        $('#txtFromDate').datepicker({
            format: "dd/mm/yyyy",
        }).datepicker("setDate", 'now');

        $('#txtToDate').datepicker({
            format: "dd/mm/yyyy",
            maxDate: moment().format("MM/DD/yyyy")
        }).datepicker("setDate", 'now');

        //$('#txtFromDate').datetimepicker({
        //    format: 'DD/MM/yyyy',
        //    sideBySide: true,
        //    maxDate: moment().format("MM/DD/yyyy")
        //});

        //$('#txtToDate').datetimepicker({
        //    format: 'DD/MM/yyyy',
        //    sideBySide: true, // shows calendar and time picker side by side
        //    maxDate: moment().format("MM/DD/yyyy")

        //});

    }

    window.onload = function () {
        initializeDatepicker();
        $(document).find('#btnTodayReport').trigger('click');



    };
</script>

<body onload="initializeDatepicker()">


    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress runat="server" ID="UpdateProgress" class="gettext" AssociatedUpdatePanelID="upBatch">
            <ProgressTemplate>
                <div class="loading">
                    <img src="RESOURCES/loading.gif" alt="Loading" /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="mobile_hidden">
            <div class="col-3 col-s-3 image_icon">
                <img src="RESOURCES/Appointment.png" class="image_resize" />
            </div>

            <div class="col-9 col-s-9 image_title">
                <h1 style="font-weight: bold">Appointment History</h1>
            </div>
        </div>
        <div class="topnav print-hide" id="myTopnav">

            <a href="Visitor_MainMenu.aspx">Home</a>
            <a href="Visitor_CreateNewAppointment.aspx" id="NewAppointment" runat="server" visible="false">Create New</a>

            <a id="AppointmentHistory" href="Visitor_History.aspx" runat="server" class="CurrentPage">Appointment History</a>
            <%if (Session["UserRole"].ToString() != "3")
                { %>
            <a id="SFA" href="#" class="sfa" runat="server" visible="false">SFA</a>
            <%} %>

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
        <div id="report_section" runat="server" class="col-12 col-s-12" visible="true">
            <asp:Button ID="btnTodayReport" runat="server" OnClick="ViewAppointmentHistory" Text="Today" CssClass="frame_style_type2" CausesValidation="false" />
            <asp:Button ID="btnTomorrowReport" runat="server" OnClick="ViewAppointmentHistory" Text="Upcoming" class="frame_style_type2" CausesValidation="false" />
            <asp:Button ID="btnPastReport" runat="server" OnClick="ViewAppointmentHistory" Text="Past" class="frame_style_type2" CausesValidation="false" />
            <asp:Label ID="CurrentSelectedType" runat="server" CssClass="labeltext" Text="" Style="display: none"></asp:Label>
            <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer_Tick" />
            <div class="col-s-12 col-12">
                <asp:Button ID="btnTitle" runat="server" Text="Report" class="accordion_style_sub_fixed_darkcyan" /><br />
            </div>

            <div id="generalreport_section" runat="server">
                <div class="col-10 col-s-12" id="dateRange_section" runat="server">
                    <div class="col-2 col-s-2">
                        <asp:Label ID="lbldate" runat="server" CssClass="labeltext" Text="Date Range: "></asp:Label>
                    </div>
                    <div class="col-3 col-s-6">

                        <div class="">
                            <div class="form-group">
                                <div class="input-group date" id="txtFromDateTimePicker">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="Select Date and Time"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-3 col-s-6">

                        <div class="">
                            <div class="form-group">
                                <div class="input-group date" id="txtToDateTimePicker">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" placeholder="Select Date and Time"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="col-3 col-s-12">
                        <div class="col-4 col-s-2">
                            <label class="labeltext">Status.      </label>
                        </div>
                        <div class="col-5 col-s-3">
                            <asp:DropDownList ID="ddlStatus" runat="server" class="dropdownlist"></asp:DropDownList>
                        </div>
                    </div>
                </div>


                <asp:Panel ID="SearchWrapper" runat="server" Visible="false">
                    <div class="col-4 col-s-12">
                        <div class="col-2 col-s-2">
                            <label class="labeltext">Serial No.      </label>
                        </div>

                        <div class="col-3 col-s-6">
                            <asp:TextBox ID="TxtSearchAppointment" autocomplete="off" class="inputtext" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>

                <div class="col-12 col-s-12">
                    <asp:Panel ID="AwareColorSection" runat="server" Visible="false">
                        <%--                        <div class="aware-color">
                            <div class="fit-content">
                                <div style="background-color: yellow; height: 30px; width: 50px"></div>
                            </div>
                            <div class="fit-content" style="margin-left: 5px">
                                <label style="font-weight: 900">Incoming</label>
                            </div>

                            <div class="fit-content" style="margin-left: 5px">
                                <div style="background-color: #40E0D0; height: 30px; width: 50px"></div>
                            </div>
                            <div class="fit-content" style="margin-left: 5px;">
                                <label style="font-weight: 900">Checked Out</label>
                            </div>
                        </div>--%>
                    </asp:Panel>
                </div>
                <asp:UpdatePanel ID="upBatch" runat="server">
                    <ContentTemplate>
                        <div class="col-12 cols-2" style="display: flex; justify-content: center">
                            <asp:Button ID="btnGenReport" runat="server" CssClass="glow_green" Text="Generate" OnClick="GenerateHistory" CausesValidation="false" Visible="false" />
                        </div>

                        <div class="col-12 col-s-12" id="export" runat="server" visible="true">
                            <div class="image_properties">
                                <asp:ImageButton ID="imgbtnExport" class="image_nohighlight" runat="server" ImageUrl="~/RESOURCES/icons8-xls-export-50.png" />
                                <asp:ImageButton ID="ImageButton2" class="image_highlight" runat="server" ImageUrl="~/RESOURCES/hover-xls-export-50.png" OnClick="btnExport_Click" CausesValidation="false" />
                            </div>
                        </div>
                        </div>


                            <div class="col-12 col-s-12">

                                <div id="AppointmentRecordSection" runat="server" style="max-width: 100%; overflow: auto; max-height: 110%; min-width: 100%">
                                    <asp:GridView ID="gvAppointmentReport" runat="server"
                                        PageSize="20" HorizontalAlign="Left" CssClass="mydatagrid"
                                        HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="No."
                                        OnPageIndexChanging="datagrid_PageIndexChanging_AppointmentHistory" AllowCustomPaging="false"
                                        HtmlEncode="False" Style="overflow: hidden;" AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="gvAppointmentReport_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <div class="col-12 col-s-12">
                                                        <div class="col">
                                                            <asp:Button ID="Button_CheckOut" runat="server" Text="Check Out" CssClass="button_grid btn btn-checkout" CommandArgument='<%# Eval("AppointmentID") %>' OnClick="ActionButton" />
                                                            <asp:Button ID="Button_CheckIn" class="glow_green" runat="server" Text="Check In" CssClass="button_grid btn btn-checkin" CommandArgument='<%# Eval("AppointmentID") %>' OnClick="ActionButton" />
                                                        </div>
                                                        <div class="col">
                                                            <asp:Button ID="Button_Cancel" runat="server" Text="Cancel" CssClass="button_grid btn btn-cancel" CommandArgument='<%# Eval("AppointmentID") %>' OnClick="ActionButton" />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AppointmentID" HeaderText="Appointment ID" />
                                            <asp:BoundField DataField="VisitorName" HeaderText="Visitor Name" />
                                            <asp:BoundField DataField="VisitorCompany" HeaderText="Visitor Company" />
                                            <asp:BoundField DataField="LPGCompany" HeaderText="LPG Company" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="NumberOfVisitors" HeaderText="Pax" />
                                            <asp:BoundField DataField="StartDateTime" HeaderText="Start Date" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                            <asp:BoundField DataField="EndDateTime" HeaderText="End Date" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                            <asp:BoundField DataField="ActualStartDateTime" HeaderText="Checked In Date" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                            <asp:BoundField DataField="ActualEndDateTime" HeaderText="Checked Out Date" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                            <asp:BoundField DataField="VehiclePlateNo" HeaderText="Vehicle Plate No" />
                                            <asp:BoundField DataField="PersonToMeet" HeaderText="Person To Meet" />
                                            <asp:BoundField DataField="LocationName" HeaderText="Location" />
                                            <asp:BoundField DataField="Purpose" HeaderText="Purpose" />
                                            <asp:BoundField DataField="UserName" HeaderText="Created By" />
                                            <asp:BoundField DataField="CreatedDateTime" HeaderText="Created Date Time" ItemStyle-CssClass="wide-column" HeaderStyle-CssClass="wide-column" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="AppointmentHistoryLabel" runat="server" CssClass="labeltext" Visible="false" Style="color: red">No records found!</asp:Label>
                                </div>

                            </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ImageButton2" />
                        <%--<asp:PostBackTrigger ControlID="Button_report_section" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </form>
</body>
</html>

<script>
    $(document).ready(function () {
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
    })
</script>
