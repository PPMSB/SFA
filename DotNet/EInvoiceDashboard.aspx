<%@ Page Title="" Language="C#" MasterPageFile="~/lhdn/EInvoiceMaster.Master" AutoEventWireup="true" CodeBehind="EInvoiceDashboard.aspx.cs" Inherits="DotNetSync.lhdn.EInvoiceDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="page_title" runat="server">
    <title>e-Invoice Dashboard</title>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="css_section" runat="server">
    <style>
        .row-flex {
            display: flex;
            flex-wrap: wrap;
        }

        .content {
            height: 100%;
            padding: 15px 25px 5px;            
        }

        .summary-col {
            margin-bottom: 20px;
        }

        .outbound{
            background: #483C46;
            color: #fff;
        }

        .error{
            background: red;
            color: #fff;
        }

        .pending{
            background: goldenrod;
            color: #fff;
        }

        .rejected{
            background: #82204A;
            color: #fff;
        }

        .completed{
            background: blue;
            color: #fff;
        }

        .link {
            color:#fff;
            text-decoration: none;
        }

        .link:hover{
            color:#ddd;
            text-decoration:underline;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <h4 class="px-3 pt-2">Summary</h4>

    <div class="datepicker-group px-3 py-1 bg-secondary-subtle radius-5px">
        <label for="invoice_date">Invoice From Date:</label>
        <input type="date" id="invoice_from_date" name="trip-start" min="2024-08-01" style="margin-right:40px" class="radius-3px p-1 px-3 border-0">
        <label for="invoice_date">Invoice To Date:</label>
        <input type="date" id="invoice_to_date" name="trip-start" min="2024-08-01" class="radius-3px p-1 px-3 border-0">
    </div>

    <div class="row row-flex mx-0 px-0 mt-3">
        <div class="col-md-4 col-lg-3 summary-col">            
            <div class="content radius-10px outbound">
                <h5>Outbound</h5>
                <h1 id="outbound_count" class="text-end">-</h1>
                <span id="outbound_count_err"></span>
            </div>            
        </div>
        <div class="col-md-4 col-lg-3 summary-col">
            <div class="content radius-10px error">
                <h5>Submission Failed</h5>
                <h1 id="submission_error_count" class="text-end">-</h1>
                <span id="submission_error_count_err"></span>
            </div>
        </div>
        <div class="col-md-4 col-lg-3 summary-col">
            <div class="content radius-10px pending">
                <h5>Pending Validation</h5>
                <h1 id="pending_validation_count" class="text-end">-</h1>
                <span id="pending_validation_count_err"></span>
            </div>
        </div>
        <div class="col-md-4 col-lg-3 summary-col">
            <div class="content radius-10px rejected">
                <h5>Validation Failed</h5>
                <h1 id="validation_failed_count" class="text-end">-</h1>
                <span id="validation_failed_count_err"></span>
            </div>
        </div>
        <div class="col-md-4 col-lg-3 summary-col">
            <div class="content radius-10px completed">
                <h5>Completed</h5>
                <h1 id="completed_count" class="text-end">-</h1>
                <span id="completed_count_err"></span>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascript_section" runat="server">    
    <script>
        $(document).ready(function () {
            init_invoice_from_date();
            init_invoice_to_date();

            get_outbound_count();
            get_submission_error_count();
            get_pending_validation_count();
            get_validation_failed_count();
            get_completed_count();

            $("#invoice_from_date").change(function () {
                console.log("invoice_from_date changed");
                get_outbound_count();
                get_submission_error_count();
                get_pending_validation_count();
                get_validation_failed_count();
                get_completed_count();
            });

            $("#invoice_to_date").change(function () {
                console.log("invoice_to_date changed");
                get_outbound_count();
                get_submission_error_count();
                get_pending_validation_count();
                get_validation_failed_count();
                get_completed_count();
            });

            $("#company").change(function () {
                get_outbound_count();
                get_submission_error_count();
                get_pending_validation_count();
                get_validation_failed_count();
                get_completed_count();
            });
        });

        function init_invoice_from_date() {
            var currentDate = new Date();

            // Calculate the Monday of the current week
            var dayOfWeek = currentDate.getDay();  // 0 = Sunday, 1 = Monday, etc.
            var mondayDate = new Date(currentDate);
            mondayDate.setDate(currentDate.getDate() - dayOfWeek + (dayOfWeek === 0 ? -6 : 1));

            // Format the dates to yyyy-mm-dd
            var formatDate = function (date) {
                var day = ("0" + date.getDate()).slice(-2);
                var month = ("0" + (date.getMonth() + 1)).slice(-2);
                return date.getFullYear() + "-" + month + "-" + day;
            };

            $('#invoice_from_date').val(formatDate(mondayDate));
        }

        function init_invoice_to_date() {
            var currentDate = new Date();

            // Format the dates to yyyy-mm-dd
            var formatDate = function (date) {
                var day = ("0" + date.getDate()).slice(-2);
                var month = ("0" + (date.getMonth() + 1)).slice(-2);
                return date.getFullYear() + "-" + month + "-" + day;
            };

            $('#invoice_to_date').val(formatDate(currentDate));
        }

        function formatDateToDMY(dateString) {
            var dateParts = dateString.split('-');
            var formattedDate = dateParts[2] + '/' + dateParts[1] + '/' + dateParts[0];
            return formattedDate;
        }

        // get outbound/new invoice count
        function get_outbound_count() {
            var company = $("#company").val();

            var formatted_from_date = "";
            var invoice_from_date = $("#invoice_from_date").val();
            if (invoice_from_date) {
                var formatted_from_date = formatDateToDMY(invoice_from_date);
                //invoice_date = formatDateToDMY(invoice_date);
                //console.log("Formatted from date:", formatted_from_date);
            } else {
                invoice_from_date = "";
                console.log("No from date selected. From date is set to an empty string.");
            }

            var formatted_to_date = "";
            var invoice_to_date = $("#invoice_to_date").val();
            if (invoice_to_date) {
                var formatted_to_date = formatDateToDMY(invoice_to_date);
                //invoice_date = formatDateToDMY(invoice_date);
                //console.log("Formatted to date:", formatted_to_date);
            } else {
                invoice_to_date = "";
                console.log("No to date selected. To date is set to an empty string.");
            }

            $.ajax({
                url: "EInvoiceDashboard.aspx/Get_Count",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    company: company,
                    invoice_from_date: formatted_from_date,
                    invoice_to_date: formatted_to_date,
                    submission_status: <%= constants.SUBMISSION_NEW %>
                }),
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var count = json_response.count;

                    if(status == "failed") {
                        $("#outbound_count_err").html('Error: ' + status_msg);
                        return;
                    }

                    if (status == "success") {
                        if (count > 0) {
                            //var outbound_list_url = `<a class='link' href='EInvoiceOutboundList.aspx?comregno=${company}'>${count}</a>`;
                            var outbound_list_url = `<a class='link' href='EInvoiceInvoiceList.aspx?comregno=${company}&status=<%= constants.SUBMISSION_NEW %>&from_date=${invoice_from_date}&to_date=${invoice_to_date}'>${count}</a>`;
                            $("#outbound_count").html(outbound_list_url);
                        } else {
                            $("#outbound_count").html("0");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#outbound_count_err").html('Error: ' + textStatus);
                }
            });
        } 

        function get_submission_error_count() {
            var company = $("#company").val();

            var formatted_from_date = "";
            var invoice_from_date = $("#invoice_from_date").val();
            if (invoice_from_date) {
                var formatted_from_date = formatDateToDMY(invoice_from_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted from date:", formatted_from_date);
            } else {
                invoice_from_date = "";
                console.log("No from date selected. From date is set to an empty string.");
            }

            var formatted_to_date = "";
            var invoice_to_date = $("#invoice_to_date").val();
            if (invoice_to_date) {
                var formatted_to_date = formatDateToDMY(invoice_to_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted to date:", formatted_to_date);
            } else {
                invoice_to_date = "";
                console.log("No to date selected. To date is set to an empty string.");
            }

            $.ajax({
                url: "EInvoiceDashboard.aspx/Get_Count",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    company: company,
                    invoice_from_date: formatted_from_date,
                    invoice_to_date: formatted_to_date,
                    submission_status: <%= constants.SUBMISSION_UUID_FAILED %>
                }),
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var count = json_response.count;

                    if (status == "failed") {
                        $("#submission_error_err").html('Error: ' + status_msg);
                        return;
                    }

                    if (status == "success") {
                        if (count > 0) {
                            var outbound_list_url = `<a class='link' href='EInvoiceInvoiceList.aspx?comregno=${company}&status=<%= constants.SUBMISSION_UUID_FAILED %>&from_date=${invoice_from_date}&to_date=${invoice_to_date}'>${count}</a>`;
                            $("#submission_error_count").html(outbound_list_url);
                        } else {
                            $("#submission_error_count").html("0");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#submission_error_count_err").html('Error: ' + textStatus);
                }
            });
        } 

        function get_pending_validation_count() {
            var company = $("#company").val();

            var formatted_from_date = "";
            var invoice_from_date = $("#invoice_from_date").val();
            if (invoice_from_date) {
                var formatted_from_date = formatDateToDMY(invoice_from_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted from date:", formatted_from_date);
            } else {
                invoice_from_date = "";
                console.log("No from date selected. From date is set to an empty string.");
            }

            var formatted_to_date = "";
            var invoice_to_date = $("#invoice_to_date").val();
            if (invoice_to_date) {
                var formatted_to_date = formatDateToDMY(invoice_to_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted to date:", formatted_to_date);
            } else {
                invoice_to_date = "";
                console.log("No to date selected. To date is set to an empty string.");
            }

            $.ajax({
                url: "EInvoiceDashboard.aspx/Get_Count",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    company: company,
                    invoice_from_date: formatted_from_date,
                    invoice_to_date: formatted_to_date,
                    submission_status: <%= constants.SUBMISSION_LONG_ID_PENDING %>
                }),
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var count = json_response.count;

                    if (status == "failed") {
                        $("#pending_validation_count_err").html('Error: ' + status_msg);
                        return;
                    }

                    if (status == "success") {
                        if (count > 0) {
                            var outbound_list_url = `<a class='link' href='EInvoiceInvoiceList.aspx?comregno=${company}&status=<%= constants.SUBMISSION_LONG_ID_PENDING %>&from_date=${invoice_from_date}&to_date=${invoice_to_date}'>${count}</a>`;
                            $("#pending_validation_count").html(outbound_list_url);
                        } else {
                            $("#pending_validation_count").html("0");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#pending_validation_count_err").html('Error: ' + textStatus);
                }
            });
        } 

        function get_validation_failed_count() {
            var company = $("#company").val();

            var formatted_from_date = "";
            var invoice_from_date = $("#invoice_from_date").val();
            if (invoice_from_date) {
                var formatted_from_date = formatDateToDMY(invoice_from_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted from date:", formatted_from_date);
            } else {
                invoice_from_date = "";
                console.log("No from date selected. From date is set to an empty string.");
            }

            var formatted_to_date = "";
            var invoice_to_date = $("#invoice_to_date").val();
            if (invoice_to_date) {
                var formatted_to_date = formatDateToDMY(invoice_to_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted to date:", formatted_to_date);
            } else {
                invoice_to_date = "";
                console.log("No to date selected. To date is set to an empty string.");
            }

            $.ajax({
                url: "EInvoiceDashboard.aspx/Get_Count",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    company: company,
                    invoice_from_date: formatted_from_date,
                    invoice_to_date: formatted_to_date,
                    submission_status: <%= constants.SUBMISSION_LONG_ID_FAILED %>
                }),
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var count = json_response.count;

                    if (status == "failed") {
                        $("#validation_failed_count_err").html('Error: ' + status_msg);
                        return;
                    }

                    if (status == "success") {
                        if (count > 0) {
                            var outbound_list_url = `<a class='link' href='EInvoiceInvoiceList.aspx?comregno=${company}&status=<%= constants.SUBMISSION_LONG_ID_FAILED %>&from_date=${invoice_from_date}&to_date=${invoice_to_date}'>${count}</a>`;
                            $("#validation_failed_count").html(outbound_list_url);
                        } else {
                            $("#validation_failed_count").html("0");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#validation_failed_count_err").html('Error: ' + textStatus);
                }
            });
        } 

        function get_completed_count() {
            var company = $("#company").val();

            var formatted_from_date = "";
            var invoice_from_date = $("#invoice_from_date").val();
            if (invoice_from_date) {
                var formatted_from_date = formatDateToDMY(invoice_from_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted from date:", formatted_from_date);
            } else {
                invoice_from_date = "";
                console.log("No from date selected. From date is set to an empty string.");
            }

            var formatted_to_date = "";
            var invoice_to_date = $("#invoice_to_date").val();
            if (invoice_to_date) {
                var formatted_to_date = formatDateToDMY(invoice_to_date);
                //invoice_date = formatDateToDMY(invoice_date);
                console.log("Formatted to date:", formatted_to_date);
            } else {
                invoice_to_date = "";
                console.log("No to date selected. To date is set to an empty string.");
            }

            $.ajax({
                url: "EInvoiceDashboard.aspx/Get_Count",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    company: company,
                    invoice_from_date: formatted_from_date,
                    invoice_to_date: formatted_to_date,
                    submission_status: <%= constants.SUBMISSION_COMPLETED %>
                }),
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var count = json_response.count;

                    if (status == "failed") {
                        $("#completed_count_err").html('Error: ' + status_msg);
                        return;
                    }

                    if (status == "success") {
                        if (count > 0) {
                            var outbound_list_url = `<a class='link' href='EInvoiceInvoiceList.aspx?comregno=${company}&status=<%= constants.SUBMISSION_COMPLETED %>&from_date=${invoice_from_date}&to_date=${invoice_to_date}'>${count}</a>`;
                            $("#completed_count").html(outbound_list_url);
                        } else {
                            $("#completed_count").html("0");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#completed_count_err").html('Error: ' + textStatus);
                }
            });
        } 
    </script>
</asp:Content>