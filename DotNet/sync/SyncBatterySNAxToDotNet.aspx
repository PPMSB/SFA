call<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncBatterySNAxToDotNet.aspx.cs" Inherits="DotNetSync.SyncBatterySNAxToDotNet" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="300">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <title>Sync Battery SN - Ax to DotNet</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />

    <!-- Datatable -->
    <link rel="stylesheet" type="text/css" href="datatable/css/jquery.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/responsive.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/fixedHeader.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/select.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/rowReorder.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/custom.css"/>
    <link rel="stylesheet" type="text/css" href="STYLES/custom.css"/>
   
</head>
<body>
    <div class="col-12" style="padding:10px 35px; border-bottom:1px solid gray">
        <h1>Sync Battery SN - Ax to DotNet</h1>
    </div>

    <div style="margin: 10px 35px">
        <table style="width:100%;">
            <tr>
                <td style="width:10%">Start Time</td>
                <td><span id="start_time" class="col-12 col-md-10 col-lg-11"></span></td>
            </tr>
            <tr>
                <td>No. of Record(s)</td>
                <td><span id="no_of_records" class="col-12 col-md-10 col-lg-11"></span></td>
            </tr>
        </table>
    </div>

    <div id="table" class="p-3 my-5">Retrieving list of gatepass to sync...</div>
    <div id="error" class="p-3 my-5"></div>
    
    
</body>
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
        var last_end_time = "";

        $(document).ready(function () {
            get_table();
        });

        function get_table() {
            console.log("get_table() started");
            $("#start_time").html(get_current_time());

            $.ajax({
                url: "SyncBatterySNAxToDotNet.aspx/get_table",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: {

                },
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object                    
                    //var start_time = json_response.StartTime;
                    var no_of_records = json_response.no_of_records;
                    var table_string = json_response.table_string;
                    var batterysn_list = json_response.batterysn_list;
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;

                    if (status == "failed") {
                        $("#table").html('AJAX Error: ' + status_msg);
                        return;
                    }

                    //$("#start_time").html(startTime);
                    $("#no_of_records").html(no_of_records);
                    $("#table").html(table_string);
                    init_table();

                   
                    for (var i = 0; i < batterysn_list.length; i++) {
                        var batterysn_ob = batterysn_list[i];

                        var serial_num = batterysn_ob.serial_num;
                        var cust_account = batterysn_ob.cust_account;
                        var cust_name = batterysn_ob.cust_name;
                        var invoice = batterysn_ob.invoice;
                        var invoice_date = batterysn_ob.invoice_date;
                        var item_id = batterysn_ob.item_id;
                        var item_name = batterysn_ob.item_name;
                        var salesid = batterysn_ob.salesid;
                        var period = batterysn_ob.period;

                        //run_sync(invoice_id, invoice_date, gatepass, gatepass_date, account_num, account_name, transporter_code);
                        run_sync(serial_num, cust_account, cust_name, invoice, invoice_date, item_id, item_name, salesid, period)
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#error").html('AJAX Error: ' + textStatus);
                }
            });
        } 

        function init_table() {
            $.extend($.fn.dataTable.defaults, {
                responsive: false
            });

            var table = $('#sync_table').DataTable({
                //select: true,
                fixedHeader: true,

                "lengthMenu": [[-1, 50, 100], ["All", 50, 100]],
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
                        title: 'Report ' + $("#sync_table option:selected").text(),
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'excel',
                        title: 'Report ' + $("#sync_table option:selected").text(),
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'pdf',
                        title: 'Report ' + $("#sync_table option:selected").text(),
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    'print'
                ]
            });


        } // end of init_table

        

        function run_sync(serial_num, cust_account, cust_name, invoice, invoice_date, item_id, item_name, salesid, period) {
            console.log("run_sync() started");
            $.ajax({
                url: "SyncBatterySNAxToDotNet.aspx/run_sync",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    serial_num: serial_num,
                    cust_account: cust_account,
                    cust_name: cust_name,
                    invoice: invoice,
                    invoice_date: invoice_date,
                    item_id: item_id,
                    item_name: item_name,
                    salesid: salesid,
                    period: period
                }), 
                success: function (response) {
                    //console.log(response.toString());
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var remove_flag_status = json_response.remove_flag_status;
                    var remove_flag_status_msg = json_response.remove_flag_status_msg;

                    var status_label = "";

                    if (status == "success") {
                        status_label = "<span class='success_label'>" + status_msg + "</span>";
                    } else if (status == "" && status.toLowerCase() == "failed") {
                        status_label = "<br/><span class='failed_label'>" + status_msg + "</span><br/>";
                    }

                    var remove_flag_label = "";

                    if (remove_flag_status == "success") {
                        remove_flag_label = "<span class='success_label'>" + remove_flag_status_msg + "</span>";
                    } else if (remove_flag_status == "" && remove_flag_status.toLowerCase() == "failed") {
                        remove_flag_label = "<br/><span class='failed_label'>" + remove_flag_status_msg + "</span><br/>";
                    }

                    //var index_of_slash = invoice_id.indexOf('/');
                    //console.log("index_of_slash: " + index_of_slash);
                    //invoice = invoice_id.slice(0, index_of_slash);
                    
                    //$("#" + invoice + "_status").html(status.toUpperCase() + ": " + statusMsg);
                    $("#" + serial_num + "_status").html(status_label + remove_flag_label);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);

                    var status_label = "<span class='failed_label'>" + 'AJAX Error:' + textStatus + "</span>";

                    var index_of_slash = invoice_id.indexOf('/');
                    invoice = invoice_id.slice(0, index_of_slash);
                    $("#" + invoice + "_status").html(status_label);

                    //$("#result").html('AJAX Error: ' + textStatus);
                }
            });
        } 

        function get_current_time() {
            var now = new Date();

            var hours = now.getHours();
            var minutes = now.getMinutes();
            var seconds = now.getSeconds();

            // Add leading zero if needed
            hours = (hours < 10) ? "0" + hours : hours;
            minutes = (minutes < 10) ? "0" + minutes : minutes;
            seconds = (seconds < 10) ? "0" + seconds : seconds;

            var current_time = hours + ":" + minutes + ":" + seconds;

            return current_time;
        }
    </script>
</html>
