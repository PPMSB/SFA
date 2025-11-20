<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncGatepassHirevToDotNet.aspx.cs" Inherits="DotNetSync.SyncGatepassHirevToDotNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="300">
    <link rel="shortcut icon" href="RESOURCES/LFIB_icon.ico" />

    <title>Sync gatepass - Hirev to DotNet</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="mobile-web-app-capable" content="yes" />


    <!-- Datatable -->
    <link rel="stylesheet" type="text/css" href="datatable/css/jquery.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/responsive.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="datatable/css/fixedHeader.dataTables.min.css">
    <link rel="stylesheet" type="text/css" href="datatable/css/select.dataTables.min.css">
    <link rel="stylesheet" type="text/css" href="datatable/css/rowReorder.dataTables.min.css">
    <link rel="stylesheet" type="text/css" href="datatable/css/custom.css">
    <link rel="stylesheet" type="text/css" href="STYLES/custom.css">

    
   
</head>
<body>
    <div class="col-12" style="padding:10px 35px; border-bottom:1px solid gray">
        <h1>Sync gatepass - Hirev to DotNet</h1>
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

    <div id="table" style="padding: 10px 35px; margin: 0px 0; width=100%" class="p-2 my-5">Retrieving list of gatepass to sync...</div>
    
    
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
            var currentTime = new Date();
            var startTime = new Date();
            startTime.setDate(startTime.getDate() - 1);
            startTime.setHours(18, 0, 0); // Set the start time to 6:00 PM
            var endTime = new Date();
            endTime.setDate(endTime.getDate()); // Set the end time to 8:00 AM the next day
            endTime.setHours(8, 0, 0);

            console.log(startTime);
            console.log(endTime);
            console.log(currentTime);

            console.log("get_table() started");
            $("#start_time").html(get_current_time());

            $.ajax({
                //url: "http://localhost:8080/0/sync_new/sync_gatepass_to_dotnet_gettable.php",
                //url: "http://edms.posim.com.my/0/sync_new/sync_gatepass_to_dotnet_gettable.php",
                url: "http://www.hi-rev.com.my/0/sync_new/sync_gatepass_to_dotnet_gettable.php",
                timeout: 60000,
                type: "POST",
                //contentType: "application/json; charset=utf-8",
                //dataType: "json",
                data: {

                },
                success: function (response) {
                    // Parse the JSON response
                    var json_response = JSON.parse(response);

                    console.log(json_response);

                    // Access properties from the JSON object                    
                    var no_of_records = json_response.gatepass_array.length;
                    var table_string = json_response.table;
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;
                    var gatepass_object_list = json_response.gatepass_array;

                    if (status == "failed") {
                        alert(status_msg);
                        return;
                    }

                    $("#no_of_records").html(no_of_records);
                    $("#table").html(table_string);
                    init_table();

                    console.log(gatepass_object_list);

                    // Access and loop through the array of objects
                    for (var i = 0; i < gatepass_object_list.length; i++) {
                        var gatepass_object = gatepass_object_list[i];

                        var invoice_id = gatepass_object.invoice_id;
                        var invoice_date = gatepass_object.invoice_date;
                        var gatepass = gatepass_object.gatepass;
                        var gatepass_date = gatepass_object.gatepass_date;
                        var account_num = gatepass_object.account_num;
                        var account_name = gatepass_object.account_name;
                        var transporter_code = gatepass_object.transporter_code;
                        var staff_id = gatepass_object.staff_id;
                        var coordinate = gatepass_object.coordinate;
                        var gps_date = gatepass_object.gps_date;
                        var gps_time = gatepass_object.gps_time;
                        var img_name_1 = gatepass_object.img_name_1;
                        var img_name_2 = gatepass_object.img_name_2;
                        var img_name_3 = gatepass_object.img_name_3;
                        var img_name_4 = gatepass_object.img_name_4;
                        var img_datetime = gatepass_object.img_datetime;
                        var received_datetime = gatepass_object.received_datetime;
                        var status = gatepass_object.status;
                        var status_date = gatepass_object.status_date;
                        var status_by = gatepass_object.status_by;
                        var status_by_fullname = gatepass_object.status_by_fullname;

                        run_sync(invoice_id, invoice_date, gatepass, gatepass_date, account_num, account_name, transporter_code,
                            staff_id, coordinate, gps_date, gps_time,
                            img_name_1, img_name_2, img_name_3, img_name_4,
                            img_datetime, received_datetime, status, status_date, status_by, status_by_fullname);

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#result").html('AJAX Error: ' + textStatus);
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



        function run_sync(invoice_id, invoice_date, gatepass, gatepass_date, account_num, account_name, transporter_code,
                            staff_id, coordinate, gps_date, gps_time,
                            img_name_1, img_name_2, img_name_3, img_name_4,
                            img_datetime, received_datetime, status, status_date, status_by, status_by_fullname) {
            console.log("Sync: " + invoice_id + "|received_datetime: " + received_datetime);
            $.ajax({
                url: "SyncGatepassHirevToDotNet.aspx/run_sync",
                timeout: 60000,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    invoice_id: invoice_id,
                    invoice_date: invoice_date,
                    gatepass: gatepass,
                    gatepass_date: gatepass_date,
                    account_num: account_num, 
                    account_name: account_name,
                    transporter_code: transporter_code,
                    staff_id: staff_id,
                    coordinate: coordinate,
                    gps_date: gps_date,
                    gps_time: gps_time,
                    img_name_1: img_name_1,
                    img_name_2: img_name_2,
                    img_name_3: img_name_3,
                    img_name_4: img_name_4,
                    img_datetime: img_datetime,
                    received_datetime: received_datetime,
                    status: status,
                    status_date: status_date,
                    status_by: status_by,
                    status_by_fullname: status_by_fullname
                }),                 
                success: function (response) {
                    // Parse the JSON response
                    var json_response = JSON.parse(response.d);

                    // Access properties from the JSON object
                    var status = json_response.status;
                    var status_msg = json_response.status_msg ;

                    var status_label = "";
                    var index_of_slash = invoice_id.indexOf('/');
                    invoice = invoice_id.slice(0, index_of_slash);

                    if (status == "success") {
                        status_label = "<span class='success_label'>" + status_msg.toUpperCase() + "</span>";
                        update_success_sync_status(invoice_id);

                    } else if (status != "" && status.toLowerCase() == "failed") {
                        status_label = "<span class='failed_label'>" + status_msg + "</span>";
                    }

                    $("#" + invoice + "_status").html(status_label);

                    

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#result").html('AJAX Error: ' + textStatus);
                }
            });
        }

        function update_success_sync_status(invoice_id) {
            console.log("update_success_sync_status(): " + invoice_id);
            $.ajax({
                //url: "http://localhost:8080/0/sync_new/sync_gatepass_to_dotnet_update_sync_status.php",
                //url: "http://edms.posim.com.my/0/sync_new/sync_gatepass_to_dotnet_update_sync_status.php",
                url: "http://www.hi-rev.com.my/0/sync_new/sync_gatepass_to_dotnet_update_sync_status.php",
                timeout: 60000,
                type: "POST",
                //contentType: "application/json; charset=utf-8",
                //dataType: "json",
                data: {
                    invoice_id: invoice_id
                },
                success: function (response) {
                    // Parse the JSON response
                    var json_response = JSON.parse(response);

                    // Access properties from the JSON object
                    var status = json_response.status;
                    var status_msg = json_response.status_msg;

                    var update_sync_status_label = "";

                    if (status == "success") {
                        update_sync_status_label = "<br/><span class='success_label'>UPDATE SYNC FLAG: " + status_msg + "</span>";
                    } else if (status == "" && status.toLowerCase() == "failed") {
                        update_sync_status_label = "<br/><span class='failed_label'>" + status_msg + "</span>";
                    }

                    var index_of_slash = invoice_id.indexOf('/');
                    //console.log("index_of_slash: " + index_of_slash);
                    invoice = invoice_id.slice(0, index_of_slash);

                    var original_html = $("#" + invoice + "_status").html();
                    $("#" + invoice + "_status").html(original_html + update_sync_status_label);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('AJAX Error:', textStatus, errorThrown);
                    $("#result").html('AJAX Error: ' + textStatus);
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

            var currentTime = hours + ":" + minutes + ":" + seconds;

            return currentTime;
        }
    </script>
</html>
