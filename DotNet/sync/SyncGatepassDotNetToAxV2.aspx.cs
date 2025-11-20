using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using GLOBAL_VAR;
using EncryptStringSample;
using GLOBAL_FUNCTION;
using System.Web.UI.HtmlControls;
using ActiveDs;
using static System.Windows.Forms.AxHost;
using System.Net;
using System.Security.Principal;
using System.Web.Management;
using System.Web.Services;
using Newtonsoft.Json;
using static DotNetSync.SyncGatepassDotNetToHirev;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;

namespace DotNetSync
{
    public partial class SyncGatepassDotNetToAxV2 : System.Web.UI.Page
    {
        //public static Axapta WebDynAx = new Axapta();
        private static readonly object lock_object = new object();

        //private static Axapta dynax = null;        

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            string status = "";
            string status_msg = "";

            try
            {
                dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
            {
                // Handle X++ exceptions from Business Connector
                status = "failed";
                status_msg = "X++ Exception (dynax): " + ex.Message + Environment.NewLine +
                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                     "Source: " + ex.Source + Environment.NewLine +
                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + status_msg);

                if (dynax != null)
                {
                    // Close the Axapta instance
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
            {
                status = "failed";
                status_msg = "LogonFailedException (dynax): " + ex.Message + Environment.NewLine +
                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                     "Source: " + ex.Source + Environment.NewLine +
                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[LogonFailedException] " + status_msg);

                if (dynax != null)
                {
                    // Close the Axapta instance
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }
            }
            catch (Exception ex)
            {
                status = "failed";
                status_msg = "Exception (dynax): " + ex.Message + Environment.NewLine +
                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                     "Source: " + ex.Source + Environment.NewLine +
                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
                //return ex.Message;
            }        
            */
        }

        public void Page_Unload(object sender, EventArgs e)
        {
            /*
            if (dynax != null)
            {
                dynax.Logoff(); // Close or log off the Axapta connection
                dynax.Dispose(); // Dispose of the Axapta object to release resources
                dynax = null;
            }
            */
        }

        public class InvoiceObject
        {
            public string invoice_id { get; set; }
            public string invoice_date { get; set; }
            public string coordinate { get; set; }
            public string gps_date { get; set; }
            public string gps_time { get; set; }
            public string pod_upload_datetime { get; set; }
            public string status { get; set; }
            public string status_by { get; set; }
            public string status_date { get; set; }
            public string sync_status { get; set; }
            public string sync_msg { get; set; }
            public string remove_flag_status { get; set; }
            public string remove_flag_msg { get; set; }
        }

        public class GetTableResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string no_of_records { get; set; }
            public string table_string { get; set; }
            public List<InvoiceObject> invoice_list { get; set; }
        }

        public class RunSyncResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
        }

        public class UpdateSuccessSyncStatusResponse
        {
            public string invoice_id { get; set; }
            public string status { get; set; }
            public string status_msg { get; set; }
        }

        public static string get_log_file_name()
        {
            return "Gatepass_DotNet_To_Ax_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        }


        [WebMethod]
        public static string get_table()
        {
            // from dotnet


            // to ax
            /*
            string pod_gps = ""; // coordinate
            string pod_date = ""; // gps_date
            string pod_time = ""; // gps_time
            string pod_veriby = ""; // status_by
            string pod_veridate = ""; // status_date
            string pod_veritime = ""; // status_date (retrieve time)
            string lfi_invreceived = ""; // status
            */

            //Json Response
            string json_status = "";
            string json_status_msg = "";
            string no_of_records = "";
            string table = "";
            List<InvoiceObject> invoice_list = new List<InvoiceObject>();

            Function_Method_Sync.AddSyncLog(get_log_file_name(), "Sync started: " + DateTime.Now.ToString("HH:mm:ss"));

            int count = 0;

            MySqlConnection conn = null;

            try
            {
                conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

                string sql = "SELECT invoiceid, invoicedate, coordinate, gps_date, gps_time, img_datetime, status, status_by, status_date " +
                                    "FROM lf_gatepass_dot_net " +
                                    "WHERE sync_out_ax = 1 " +
                                    "LIMIT 50";

                MySqlCommand mysql_command = new MySqlCommand(sql, conn);

                conn.Open();
                MySqlDataReader reader = mysql_command.ExecuteReader();

                table = "<table id='sync_table' class='datatable display stripe hover table dataTable dtr-inline' style='width:100%'>" +
                    "<thead><tr>" +
                    "<th>No</th>" +
                    "<th>Invoice Id</th>" +
                    "<th>Invoice Date</th>" +
                    "<th>Coordinate</th>" +
                    "<th>GPS Datetime</th>" +
                    "<th>POD Upload Datetime</th>" +
                    "<th>Verify Status</th>" +
                    "<th>Verify By</th>" +
                    "<th>Verify Date</th>" +
                    "<th style='width:300px'>Sync Status</th>" +
                    "</tr></thead>" +
                    "<tbody>";

                while (reader.Read())
                {
                    string invoice_id = reader["invoiceid"].ToString();
                    string invoice_date = reader["invoicedate"].ToString();
                    string coordinate = reader["coordinate"].ToString();
                    string gps_date = reader["gps_date"].ToString();
                    string gps_time = reader["gps_time"].ToString();
                    string pod_upload_datetime = reader["img_datetime"].ToString();
                    string status = reader["status"].ToString();
                    string status_by = reader["status_by"].ToString();
                    string status_date = reader["status_date"].ToString();

                    /*
                    int index_of_slash = invoice_id.IndexOf('/');

                    count++;

                    table = table + "<tr>" +
                        "<td style='text-align:center'>" + count + "</td>" +
                        "<td style='text-align:center'>" + invoice_id + "</td>" +
                        "<td style='text-align:center'>" + invoice_date + "</td>" +
                        "<td>" + coordinate + "</td>" +
                        "<td style='text-align:center'>" + gps_date + " " + gps_time + "</td>" +
                        "<td style='text-align:center'>" + pod_upload_datetime + "</td>" +
                        "<td style='text-align:center'>" + status + "</td>" +
                        "<td style='text-align:center'>" + status_by + "</td>" +
                        "<td style='text-align:center'>" + status_date + "</td>" +
                        "<td><label id='" + invoice_id.Substring(0, index_of_slash) + "_status'></label></td></tr>";
                    */
                    InvoiceObject invoice_ob = new InvoiceObject();
                    invoice_ob.invoice_id = invoice_id;
                    invoice_ob.invoice_date = invoice_date;
                    invoice_ob.coordinate = coordinate;
                    invoice_ob.gps_date = gps_date;
                    invoice_ob.gps_time = gps_time;
                    invoice_ob.pod_upload_datetime = pod_upload_datetime;
                    invoice_ob.status = status;
                    invoice_ob.status_by = status_by;
                    invoice_ob.status_date = status_date;

                    invoice_list.Add(invoice_ob);
                }

                invoice_list = sync_invoices(invoice_list);
                invoice_list = update_sync_status(conn, invoice_list);

                foreach (InvoiceObject invoice in invoice_list)
                {
                    int index_of_slash = invoice.invoice_id.IndexOf('/');

                    string status_label = "";
                    if (invoice.sync_status == "success")
                    {
                        status_label = "<span class='success_label'>" + invoice.sync_msg + "</span>";
                    }
                    else
                    {
                        status_label = "<span class='failed_label'>" + invoice.sync_msg + "</span>";
                    }

                    string remove_flag_label = "";
                    if (invoice.remove_flag_status == "success")
                    {
                        remove_flag_label = "<span class='success_label'>" + invoice.remove_flag_msg + "</span>";
                    }
                    else
                    {
                        remove_flag_label = "<span class='failed_label'>" + invoice.remove_flag_msg + "</span>";
                    }

                    count++;

                    table = table + "<tr>" +
                        "<td style='text-align:center'>" + count + "</td>" +
                        "<td style='text-align:center'>" + invoice.invoice_id + "</td>" +
                        "<td style='text-align:center'>" + invoice.invoice_date + "</td>" +
                        "<td>" + invoice.coordinate + "</td>" +
                        "<td style='text-align:center'>" + invoice.gps_date + " " + invoice.gps_time + "</td>" +
                        "<td style='text-align:center'>" + invoice.pod_upload_datetime + "</td>" +
                        "<td style='text-align:center'>" + invoice.status + "</td>" +
                        "<td style='text-align:center'>" + invoice.status_by + "</td>" +
                        "<td style='text-align:center'>" + invoice.status_date + "</td>" +
                        "<td><label id='" + invoice.invoice_id.Substring(0, index_of_slash) + "_status'>" + status_label + remove_flag_label + "</label></td></tr>";
                }

                json_status = "success";
                json_status_msg = "success";
                table = table + "</tbody></table>";
                no_of_records = "" + count;

                Function_Method_Sync.AddSyncLog(get_log_file_name(), "No. of rows to be synced: " + no_of_records);

                conn.Close();
            }
            catch (MySqlException ex)
            {
                json_status = "failed";
                json_status_msg = "Mysql Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + json_status_msg);
            }
            catch (Exception ex)
            {
                json_status = "failed";
                json_status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + json_status_msg);

                //return ex.Message;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            GetTableResponse response = new GetTableResponse
            {
                status = json_status,
                status_msg = json_status_msg,
                no_of_records = no_of_records, // Replace with the actual number of records
                table_string = table,
                invoice_list = invoice_list
            };

            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }

        private static List<InvoiceObject> sync_invoices(List<InvoiceObject> invoices)
        {
            lock (lock_object)
            {
                Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name, GLOBAL_Sync.ax_proxy_password,
                    GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);

                // AX fields
                string invoice_id;
                string pod_gps;
                string pod_time;
                string pod_veriby;
                int lfi_invreceived;
                DateTime pod_date;
                string pod_veridate = "";
                string pod_veritime = "";
                DateTime parsed_pod_veridate;

                int lf_syn = 0;
                DateTime status_datetime;
                string date_time_format = "yyyy-MM-dd HH:mm:ss";

                try
                {
                    dynax.TTSBegin();

                    foreach (InvoiceObject invoice in invoices)
                    {
                        // prepare values for AX fields
                        invoice_id = invoice.invoice_id;
                        pod_gps = invoice.coordinate;
                        pod_time = invoice.gps_time;
                        pod_veriby = invoice.status_by;

                        if (int.TryParse(invoice.status, out lfi_invreceived))
                        {
                        }

                        if (DateTime.TryParseExact(invoice.gps_date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out pod_date))
                        {
                        }

                        if (DateTime.TryParseExact(invoice.status_date, date_time_format, null, System.Globalization.DateTimeStyles.None, out status_datetime))
                        {
                            pod_veridate = status_datetime.ToString("yyyy-MM-dd");
                            pod_veritime = status_datetime.ToString("HH:mm:ss");
                        }

                        if (DateTime.TryParseExact(pod_veridate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsed_pod_veridate))
                        {
                        }

                        using (AxaptaRecord dynrec = dynax.CreateAxaptaRecord("LF_Gatepass"))
                        {
                            dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "InvoiceId", invoice_id));
                            if (dynrec.Found)
                            {
                                dynrec.set_Field("POD_GPS", pod_gps);
                                dynrec.set_Field("POD_Date", pod_date);
                                dynrec.set_Field("POD_Time", pod_time);
                                dynrec.set_Field("POD_VeriBy", pod_veriby);
                                dynrec.set_Field("POD_VeriDate", parsed_pod_veridate);
                                dynrec.set_Field("POD_VeriTime", pod_veritime);
                                dynrec.set_Field("LFI_InvReceived", lfi_invreceived);
                                dynrec.set_Field("LF_Syn", lf_syn); // ensure wont sync to dotnet anymore
                                dynrec.Call("Update");
                            }
                            dynrec.Dispose();
                            //dynax.TTSCommit();

                            invoice.sync_status = "success";
                            invoice.sync_msg = "Sync to Ax";

                            Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success]Update to LF_Gatepass: invoice_id: " + invoice_id);
                        }
                    }
                    dynax.TTSCommit();
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    // Handle X++ exceptions from Business Connector                    
                    string exception_msg = "X++ Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + exception_msg);

                    foreach (InvoiceObject invoice in invoices)
                    {
                        invoice.sync_status = "failed";
                        invoice.sync_msg = exception_msg;
                    }

                    dynax.TTSAbort();
                    if (dynax != null)
                    {
                        // Close the Axapta instance
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                }
                catch (Exception ex)
                {
                    string exception_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + exception_msg);

                    foreach (InvoiceObject invoice in invoices)
                    {
                        invoice.sync_status = "failed";
                        invoice.sync_msg = exception_msg;
                    }

                    dynax.TTSAbort();
                    if (dynax != null)
                    {
                        // Close the Axapta instance
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                }
                finally
                {
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }

            }

            return invoices;
        }

        [WebMethod]
        // obselete
        public static string run_sync(string invoice_id, string coordinate, string gps_date, string gps_time, string status, string status_by, string status_date)
        {
            lock (lock_object)
            {
                /*
                string user_id = "yongwc";
                string domain_name = "LIONFIB";
                string proxy_user_name = "axbcproxy";
                string proxy_password = "aos20@9";
                string company = "PPM";
                string language = "EN-US";
                string object_server = "AOS2009:2725";
                */


                Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name, GLOBAL_Sync.ax_proxy_password,
                    GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);



                // ax lf_gatepass fields
                string pod_gps = coordinate;
                DateTime pod_date;
                string pod_time = gps_time;
                string pod_veriby = status_by;
                DateTime parsed_pod_veridate;
                string pod_veridate = "";
                string pod_veritime = "";
                int lfi_invreceived;
                // Jerry 2024-04-29 - Change from lf_gatepass_update to lf_syn - Start
                // int lf_gatepass_update = 0;
                int lf_syn = 0;
                // Jerry 2024-04-29 - Change from lf_gatepass_update to lf_syn - End

                if (int.TryParse(status, out lfi_invreceived))
                {
                    // Use the parsed integer
                    //Console.WriteLine("Parsed Number: " + parsedNumber);
                }


                if (DateTime.TryParseExact(gps_date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out pod_date))
                {

                }

                DateTime status_datetime;
                string expectedFormat = "yyyy-MM-dd HH:mm:ss";
                if (DateTime.TryParseExact(status_date, expectedFormat, null, System.Globalization.DateTimeStyles.None, out status_datetime))
                {
                    pod_veridate = status_datetime.ToString("yyyy-MM-dd");
                    pod_veritime = status_datetime.ToString("HH:mm:ss");
                }

                if (DateTime.TryParseExact(pod_veridate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parsed_pod_veridate))
                {

                }


                // json response
                string json_response;
                string response_status = "";
                string response_status_msg = "";


                try
                {
                    using (AxaptaRecord dynrec = dynax.CreateAxaptaRecord("LF_Gatepass"))
                    {
                        dynax.TTSBegin();


                        dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "InvoiceId", invoice_id));
                        if (dynrec.Found)
                        {
                            dynrec.set_Field("POD_GPS", pod_gps);
                            dynrec.set_Field("POD_Date", pod_date);
                            dynrec.set_Field("POD_Time", pod_time);
                            dynrec.set_Field("POD_VeriBy", pod_veriby);
                            dynrec.set_Field("POD_VeriDate", parsed_pod_veridate);
                            dynrec.set_Field("POD_VeriTime", pod_veritime);
                            dynrec.set_Field("LFI_InvReceived", lfi_invreceived);
                            // Jerry 2024-04-29 - Change from lf_gatepass_update to lf_syn - Start
                            // dynrec.set_Field("LF_Gatepass_Update", lf_gatepass_update); // ensure wont sync to dotnet anymore
                            dynrec.set_Field("LF_Syn", lf_syn); // ensure wont sync to dotnet anymore
                            // Jerry 2024-04-29 - Change from lf_gatepass_update to lf_syn - End
                            dynrec.Call("Update");
                        }
                        dynrec.Dispose();
                        dynax.TTSCommit();

                        response_status = "success";
                        response_status_msg = "Sync to Ax";

                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success]Update to LF_Gatepass: invoice_id: " + invoice_id);
                    }


                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    // Handle X++ exceptions from Business Connector
                    response_status = "failed";
                    response_status_msg = "X++ Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + response_status_msg);

                    dynax.TTSAbort();
                    if (dynax != null)
                    {
                        // Close the Axapta instance
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                }
                catch (Exception ex)
                {
                    response_status = "failed";
                    response_status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + response_status_msg);

                    dynax.TTSAbort();
                    if (dynax != null)
                    {
                        // Close the Axapta instance
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                }
                finally
                {
                    // Log off from AX
                    //dynax.Logoff();
                    /*
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                    */
                }

                // Json reponse
                RunSyncResponse response = new RunSyncResponse
                {
                    status = response_status,
                    status_msg = response_status_msg
                };

                // Serialize the response object to JSON
                json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }



        }

        private static List<InvoiceObject> update_sync_status(MySqlConnection connx, List<InvoiceObject> invoices)
        {
            MySqlConnection conn = null;

            try
            {
                conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);
                conn.Open();

                string sql = "UPDATE lf_gatepass_dot_net SET sync_out_ax = 0, sync_out_ax_datetime = NOW() WHERE invoiceid = @invoice_id";

                foreach (InvoiceObject invoice in invoices)
                {
                    using (MySqlCommand update_command = new MySqlCommand(sql, conn))
                    {
                        update_command.Parameters.Add(new MySqlParameter("@invoice_id", MySqlDbType.VarChar, 255) { Value = invoice.invoice_id });

                        int rowsAffected = update_command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            invoice.remove_flag_status = "success";
                            invoice.remove_flag_msg = "Update sync status";
                            // Update successful
                            Console.WriteLine($"Rows Updated: {rowsAffected}");

                            Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success]Sync to Ax and removed sync flag: invoice_id: " + invoice.invoice_id);
                        }
                        else
                        {
                            // No records were updated
                            invoice.remove_flag_status = "failed";
                            invoice.remove_flag_msg = "No records updated. Invoice ID not found (" + invoice.invoice_id + ")";
                        }
                    }
                }

            }
            catch (MySqlException ex)
            {
                string status = "failed";
                string status_msg = "MySqlException: " + ex.Message + Environment.NewLine +
                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                     "Source: " + ex.Source + Environment.NewLine +
                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");

                foreach (InvoiceObject invoice in invoices)
                {
                    invoice.remove_flag_status = status;
                    invoice.remove_flag_msg = status_msg;
                }

                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[MySqlException] " + status_msg);
            }
            catch (Exception ex)
            {
                // Handle exception, log, or display an error message
                string status = "failed";
                string status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                     "Source: " + ex.Source + Environment.NewLine +
                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                foreach (InvoiceObject invoice in invoices)
                {
                    invoice.remove_flag_status = status;
                    invoice.remove_flag_msg = status_msg;
                }

                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return invoices;
        }


        [WebMethod]
        //private void axapta_update_record(string invoice_id, int sync_status) 
        // obselete
        public static string update_sync_status2(string invoice_id)
        {
            string status = "";
            string status_msg = "";

            MySqlConnection conn = null;
            /*
            MySqlConnection conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

            string sql = "UPDATE lf_gatepass_dot_net SET sync_out_ax = 0, sync_out_ax_datetime = NOW() WHERE invoiceid = @invoice_id";

            MySqlCommand update_command = new MySqlCommand(sql, conn);

            MySqlParameter update_param = new MySqlParameter("@invoice_id", MySqlDbType.VarChar, 255);
            update_param.Value = invoice_id; // Replace with the actual invoice ID
            update_command.Parameters.Add(update_param);
            */
            lock (lock_object)
            {
                try
                {
                    conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

                    string sql = "UPDATE lf_gatepass_dot_net SET sync_out_ax = 0, sync_out_ax_datetime = NOW() WHERE invoiceid = @invoice_id";

                    MySqlCommand update_command = new MySqlCommand(sql, conn);

                    MySqlParameter update_param = new MySqlParameter("@invoice_id", MySqlDbType.VarChar, 255);
                    update_param.Value = invoice_id; // Replace with the actual invoice ID
                    update_command.Parameters.Add(update_param);


                    conn.Open();
                    int rowsAffected = update_command.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        status = "success";
                        status_msg = "Update sync status";
                        // Update successful
                        Console.WriteLine($"Rows Updated: {rowsAffected}");

                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success]Sync to Ax and removed sync flag: invoice_id: " + invoice_id);
                    }
                    else
                    {
                        // No records were updated
                        status = "failed";
                        status_msg = "No records updated. Invoice ID not found  (" + invoice_id + ")";
                    }
                }
                catch (MySqlException ex)
                {
                    status = "failed";
                    status_msg = "MySqlException: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[MySqlException] " + status_msg);
                }
                catch (Exception ex)
                {
                    // Handle exception, log, or display an error message
                    status = "failed";
                    status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            UpdateSuccessSyncStatusResponse response = new UpdateSuccessSyncStatusResponse
            {
                status = status,
                status_msg = status_msg,
                invoice_id = invoice_id
            };

            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }





    }
}