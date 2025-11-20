<%@ Page Title="" Language="C#" MasterPageFile="~/lhdn/EInvoiceMaster.Master" AutoEventWireup="true" CodeBehind="EInvoiceInvoiceList.aspx.cs" Inherits="DotNetSync.lhdn.EInvoiceInvoiceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_title" runat="server">
    <title>e-Invoice Completed List</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="css_section" runat="server">
    <!-- Datatable -->
    <link rel="stylesheet" type="text/css" href="datatable/css/jquery.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/responsive.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/fixedHeader.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/select.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/rowReorder.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/custom.css"/>
    <link rel="stylesheet" type="text/css" href="STYLES/custom.css"/>
    <style>
        .dropdown-group,
        .checkbox-group,
        .datepicker-group {
            display: flex;
            align-items: center;
            margin-bottom: 10px;
        }
        label {
           
            margin-right:5px;
            padding: 0 0 5px 0;
        }

        .checkbox-label{
            margin-left:10px;
            width:auto;
        }

        .dataTables_wrapper .dataTables_length, .dataTables_wrapper .dataTables_filter, .dataTables_wrapper .dataTables_info, .dataTables_wrapper .dataTables_processing, .dataTables_wrapper .dataTables_paginate{
            padding-right:50px;
        }

        .no-sort::after {
            display: none !important; /* Hides the sorting icon */
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <h4 class="px-3 py-3 my-0">Invoice List</h4>

    <div class="row mx-0 filter px-3 py-3 bg-secondary-subtle radius-5px">
        <div class="col-12 col-md-9">
            <div class="dropdown-group">
                <label for="submission_status">Submission Status:</label>
                <select id="submission_status" class="px-3 py-2 radius-5px">
                    <option value="100">All</option>
                    <option value="0">New</option>
                    <option value="3">Pending Validation</option>
                    <option value="2">Submission Failed</option>
                    <option value="4">Validation Failed</option>
                    <option value="5">Completed</option>
                </select>
            </div>
            <div class="checkbox-group">
                <input type="checkbox" id="include_missing_tinbrn" checked="checked">
                <label class="checkbox-label" for="include_missing_tinbrn">Include missing BRN/TIN</label>
            </div>
            <div class="checkbox-group">
                <input type="checkbox" id="only_missing_tinbrn">
                <label class="checkbox-label" for="only_missing_tinbrn">Only show missing BRN/TIN</label>
            </div>
            <div class="datepicker-group">
                <label for="invoice_date">Invoice From Date:</label>
                <input type="date" id="invoice_from_date" name="trip-start" min="2024-08-01" class="radius-5px p-1 px-3 border-0 me-5">
                <label for="invoice_date">Invoice To Date:</label>
                <input type="date" id="invoice_to_date" name="trip-start" min="2024-08-01" class="radius-5px p-1 px-3 border-0">
            </div>
        </div>
        <div class="col-12 col-md-3">
            <button type="button" id="requeue_button" onclick="requeue_selected()" style="display: none;" class="btn btn-primary text-uppercase">Re-queue Selected Invoice</button>
        </div>
    </div>

    <div id="table" style="width:100%" class="col-12 p-3 mx-0">Retrieving list of invoice...</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascript_section" runat="server">
    <script src="scripts/jquery-3.5.1.js"></script>
    <script src="datatable/js/jquery.dataTables.min.js"></script>
    <script src="datatable/js/dataTables.responsive.min.js"></script>
    <script src="datatable/js/dataTables.fixedHeader.min.js"></script>
    <script src="datatable/js/dataTables.select.min.js"></script>
    <script src="datatable/js/dataTables.rowReorder.min.js"></script>

    <script src="datatable/js/dataTables.buttons.min.js"></script>
    <script src="datatable/js/buttons.print.min.js"></script>
    <script src="datatable/js/buttons.flash.min.js"></script>

    <script src="datatable/js/jszip.min.js"></script>
    <script src="datatable/js/pdfmake.min.js"></script>
    <script src="datatable/js/vfs_fonts.js"></script>
    <script src="datatable/js/buttons.html5.min.js"></script>
    <script>
        var total_column = 4;
        var submission_status = "";

        $(document).ready(function () {
            init_invoice_from_date()
            init_invoice_to_date();

            $("#requeue_button").prop("disabled", true);
            $("#requeue_button").hide();

            var comregno = '<%= comregno %>';
            var submission_status = '<%= submission_status %>';
            
            if (comregno != "") {
                $("#company").val(comregno);
                console.log(comregno);
            }

            if (submission_status != "") {
                $("#submission_status").val(submission_status);
            }
                       

            $("#company").change(function () {
                console.log("company changed");
                get_table();
            });

            $("#submission_status").change(function () {
                console.log("submission_status changed");
                get_table();
            });

            $("#include_missing_tinbrn").change(function () {
                console.log("include_missing_tinbrn changed");
                get_table();
            });

            $("#only_missing_tinbrn").change(function () {
                console.log("only_missing_tinbrn changed");
                if ($("#only_missing_tinbrn").is(':checked')) {
                    $("#include_missing_tinbrn").prop("disabled", true);
                } else {
                    $("#include_missing_tinbrn").prop("disabled", false);
                }
                get_table();
            });

            $("#invoice_from_date").change(function () {
                console.log("invoice_from_date changed");
                get_table();
            });

            $("#invoice_to_date").change(function () {
                console.log("invoice_to_date changed");
                get_table();
            });

            get_table();
        });

        function init_checkbox() {
            // Handle the Select All checkbox
            $('#select_all').click(function () {
                $('input[name="invoice_ids"]').prop('checked', this.checked);
            });

            // Ensure Select All is unchecked if any individual checkbox is unchecked
            $('input[name="invoice_ids"]').click(function () {
                if (!this.checked) {
                    $('#select_all').prop('checked', false);
                }

                // If all checkboxes are checked, check the Select All checkbox
                if ($('input[name="invoice_ids"]:checked').length == $('input[name="invoice_ids"]').length) {
                    $('#select_all').prop('checked', true);
                }
            });
        }

        /*
        function init_invoice_from_date() {
            // Set the input date to today's date
            var today = new Date();
            var year = today.getFullYear();
            var month = ('0' + (today.getMonth() + 1)).slice(-2); // Add leading zero
            var day = ('01').slice(-2); // Add leading zero
            var todayFormatted = year + '-' + month + '-' + day;

            $('#invoice_from_date').val(todayFormatted);
        }
        */
        function init_invoice_from_date() {
            var currentDate = '<%=invoice_from_date%>';

            if (currentDate != null || currentDate != "") {
                $('#invoice_from_date').val(currentDate);
                return;
            }

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
            var currentDate = '<%=invoice_to_date%>';

            if (currentDate != null || currentDate != "") {
                $('#invoice_to_date').val(currentDate);
                return;
            }

            // Set the input date to today's date
            var today = new Date();
            var year = today.getFullYear();
            var month = ('0' + (today.getMonth() + 1)).slice(-2); // Add leading zero
            var day = ('0' + today.getDate()).slice(-2); // Add leading zero
            var todayFormatted = year + '-' + month + '-' + day;

            $('#invoice_to_date').val(todayFormatted);
        }

        function disable_filters(disable) {
            $('#invoice_to_date').prop("disabled", disable);
            $('#invoice_from_date').prop("disabled", disable);
            $('#submission_status').prop("disabled", disable);
            $('#include_missing_tinbrn').prop("disabled", disable);
            $('#only_missing_tinbrn').prop("disabled", disable);
            $('#requeue_button').prop("disabled", disable);

            if ($('#only_missing_tinbrn').is(':checked')) {
                $('#include_missing_tinbrn').prop("disabled", true);
            }
        }

        function formatDateToDMY(dateString) {
            var dateParts = dateString.split('-');
            var formattedDate = dateParts[2] + '/' + dateParts[1] + '/' + dateParts[0];
            return formattedDate;
        }

        function requeue_selected() {
            var company = $("#company").val();

            // Get all checked options
            var selected_invoices = [];
            $('input[name="invoice_ids"]:checked').each(function () {
                selected_invoices.push($(this).val());
            });

            // Make an AJAX call to the WebMethod
            $.ajax({
                type: "POST",
                url: "EInvoiceInvoiceList.aspx/requeue_invoices", // Replace 'YourPage.aspx' with your actual page name
                data: JSON.stringify({ company_short_name: company, invoice_ids: selected_invoices }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Handle success
                    alert("Requeue successful!");
                    get_table();
                },
                error: function (xhr, status, error) {
                    // Handle error
                    alert("An error occurred: " + xhr.responseText);
                }
            });
        }

        function get_table() {
            disable_filters(true);
            $("#table").html("Loading...");
            var company = $("#company").val();

            submission_status = $("#submission_status").val();

            var include_missing_tinbrn = "<%=MISSING_TIN_BRN_YES%>";
            if (!$('#include_missing_tinbrn').is(':checked')) {
                include_missing_tinbrn = "<%=MISSING_TIN_BRN_NO%>";
            }

            var only_missing_tinbrn = "<%=ONLY_MISSING_TIN_BRN_YES%>";
            if (!$('#only_missing_tinbrn').is(':checked')) {
                only_missing_tinbrn = "<%=ONLY_MISSING_TIN_BRN_NO%>";
            }

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

            if (submission_status == "2" || submission_status == "4") {
                total_column = 5;
                $("#requeue_button").prop("disabled", false);
                $("#requeue_button").show();
            } else {
                total_column = 4;
                $("#requeue_button").prop("disabled", true);
                $("#requeue_button").hide();
            }

            console.log(company); 
            console.log(submission_status);
            console.log(include_missing_tinbrn);
            console.log(formatted_from_date);
            console.log(formatted_to_date);
            console.log(total_column);
            console.log("Formatted from date:", formatted_from_date);
            console.log("Formatted to date:", formatted_to_date);
            


            $.ajax({
                url: "EInvoiceInvoiceList.aspx/get_table",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    company: company,
                    submission_status: submission_status,
                    include_missing_tinbrn: include_missing_tinbrn,
                    only_missing_tinbrn: only_missing_tinbrn,
                    invoice_from_date: formatted_from_date,
                    invoice_to_date: formatted_to_date
                }),
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    //var start_time = json_response.StartTime;
                    //var no_of_records = json_response.no_of_records;
                    var table_string = json_response.table_string;
                    var invoice_list = json_response.invoice_list;
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;

                    if (status == "failed") {
                        $("#table").html('AJAX Error: ' + status_msg);
                        return;
                    }

                    //$("#start_time").html(startTime);
                    //$("#no_of_records").html(no_of_records);
                    $("#table").html(table_string);
                    init_checkbox();
                    init_table();
                    disable_filters(false);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#error").html('AJAX Error: ' + textStatus);
                    disable_filters(false);
                }
            });
        }

        function init_table() {
            $.extend($.fn.dataTable.defaults, {
                responsive: true
            });

            var table = $('#einvoice_table').DataTable({
                //select: true,
                fixedHeader: true,

                "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],
                //"scrollY":"210px",
                //"scrollCollapse": "true",
                "autowidth": "false",
                "columnDefs": [
                    {
                        targets: 'no-sort', // Apply to columns with class 'no-sort'
                        orderable: false // Disable sorting for these columns
                    }
                ],
                
                dom: 'Blfrtip',
                buttons: [
                    'copy',
                    {
                        extend: 'csv',
                        title: 'Report ' + $("#einvoice_table option:selected").text(),
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'excel',
                        title: 'Report ' + $("#einvoice_table option:selected").text(),
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'pdf',
                        title: 'Report ' + $("#einvoice_table option:selected").text(),
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    'print'
                ],
                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();

                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string'
                            ? i.replace(/[\$,]/g, '') * 1
                            : typeof i === 'number'
                                ? i
                                : 0;
                    };

                    // Total over all pages
                    total = api
                        .column(total_column)
                        .data()
                        .reduce((a, b) => intVal(a) + intVal(b), 0);

                    // Total over this page
                    pageTotal = api
                        .column(total_column, { page: 'current' })
                        .data()
                        .reduce((a, b) => intVal(a) + intVal(b), 0);

                    // Update footer
                    //api.column(4).footer().innerHTML =
                    //    '$' + pageTotal + ' ( $' + total + ' total)';
                    api.column(total_column).footer().innerHTML =
                        total.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                }
            });


        } // end of init_table
    </script>
</asp:Content>


