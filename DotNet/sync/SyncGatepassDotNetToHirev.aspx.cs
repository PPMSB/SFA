using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetSync
{
    public partial class SyncGatepassDotNetToHirev : System.Web.UI.Page
    {
        private static readonly object lock_object = new object();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static string get_log_file_name()
        {
            return "Gatepass_DotNet_to_Hirev_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        }

        public class GatepassObject
        {
            public string invoice_id { get; set; }
            public string invoice_date { get; set; }
            public string gatepass { get; set; }
            public string gatepass_date { get; set; }
            public string account_num { get; set; }
            public string transporter_code { get; set; }
            public string account_name { get; set; }
        }

        public class GetTableResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string no_of_records { get; set; }
            public string table_string { get; set; }
            public List<GatepassObject> gatepass_object_list { get; set; }
        }

        public class UpdateSuccessSyncStatusResponse
        {
            public string invoice_id { get; set; }
            public string status { get; set; }
            public string status_msg { get; set; }
        }

        [WebMethod]
        public static string get_table()
        {
            string gatepass = "";
            string gatepass_date;
            string account_num = "";
            string account_name = "";
            string invoice_id = "";
            string invoice_date;
            string transporter_code = "";
            GatepassObject gatepass_object;

            //Json Response
            string status = "";
            string status_msg = "";
            string no_of_records = "";
            string table = "";
            List<GatepassObject> gatepass_object_list = new List<GatepassObject>();
            int count = 0;

            Function_Method_Sync.AddSyncLog(get_log_file_name(), "Sync started: " + DateTime.Now.ToString("HH:mm:ss"));

            MySqlConnection conn = null;

            try
            {
                conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

                string sql = "SELECT gatepass, gatepassdate, invoiceid, invoicedate, transportercode, accountnum, accountname " +
                                    "FROM lf_gatepass_dot_net " +
                                    "WHERE sync_out_hirev = 1 LIMIT 50";

                MySqlCommand mysql_command = new MySqlCommand(sql, conn);

                conn.Open();
                MySqlDataReader reader = mysql_command.ExecuteReader();

                table = "<table id='sync_table' class='datatable display stripe hover table dataTable dtr-inline'>" +
                    "<thead><tr>" +
                    "<th>No</th>" +
                    "<th>gatepass</th>" +
                    "<th>GatePassDate</th>" +
                    "<th>Invoice Id</th>" +
                    "<th>Invoice Date</th>" +
                    "<th>Account Num</th>" +
                    "<th>Account Name</th>" +                    
                    "<th>Transporter Code</th>" +
                    "<th style='width:300px'>status</th>" +
                    "</tr></thead>" +
                    "<tbody>";

                while (reader.Read())
                {
                    gatepass = reader["gatepass"].ToString();
                    gatepass_date = reader["gatepassdate"].ToString();
                    invoice_id = reader["invoiceid"].ToString();
                    invoice_date = reader["invoicedate"].ToString();
                    transporter_code = reader["transportercode"].ToString();
                    account_num = reader["accountnum"].ToString();
                    account_name = reader["accountname"].ToString();

                    int index_of_slash = invoice_id.IndexOf('/');

                    count++;

                    table = table + "<tr>" +
                        "<td>" + count + "</td>" +
                        "<td>" + gatepass + "</td>" +
                        "<td>" + gatepass_date + "</td>" +
                        "<td>" + invoice_id + "</td>" +
                        "<td>" + invoice_date + "</td>" +
                        "<td>" + account_num + "</td>" +
                        "<td>" + account_name + "</td>" +                        
                        "<td>" + transporter_code + "</td>" +
                        "<td><label id='" + invoice_id.Substring(0, index_of_slash) + "_status'></label></td></tr>";

                    gatepass_object = new GatepassObject();
                    gatepass_object.gatepass = gatepass;
                    gatepass_object.gatepass_date = gatepass_date;
                    gatepass_object.account_num = account_num;
                    gatepass_object.account_name = account_name;
                    gatepass_object.invoice_id = invoice_id;
                    gatepass_object.invoice_date = invoice_date;
                    gatepass_object.transporter_code = transporter_code;

                    gatepass_object_list.Add(gatepass_object);

                    // Use the retrieved values as needed
                    //Console.WriteLine($"gatepass: {gatepass}, gatepass Date: {gatepass_date}, invoice_id: {invoice_id}, Invoice Date: {invoice_date}, Transporter Code: {transporter_code}, account_num: {account_num}, account_name: {account_name}");
                }

                status = "success";
                status_msg = "success";
                table = table + "</tbody></table>";
                no_of_records = "" + count;

                Function_Method_Sync.AddSyncLog(get_log_file_name(), "No. of rows to be synced: " + no_of_records);

                conn.Close();
            }
            catch (MySqlException ex)
            {             
                status = "failed";
                status_msg = "Mysql Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[MySqlException] " + status_msg);
            }
            catch (Exception ex)
            {
                status = "failed";
                status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);

                //return ex.Message;
            }
            finally
            {
                if(conn != null)
                {
                    conn.Close();
                }
                //dynax.Logoff();
            }

            GetTableResponse response = new GetTableResponse
            {
                status = status,
                status_msg = status_msg,
                no_of_records = no_of_records, // Replace with the actual number of records
                table_string = table,
                gatepass_object_list = gatepass_object_list
            };

            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }

        [WebMethod]
        public static string update_success_sync_status(string invoice_id)
        {
            string status = "";
            string status_msg = "";

            MySqlConnection conn = null;
            /*
            MySqlConnection conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

            string sql = "UPDATE lf_gatepass_dot_net SET sync_out_hirev = 0, sync_out_hirev_datetime = NOW() WHERE invoiceid = @invoice_id";

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

                    string sql = "UPDATE lf_gatepass_dot_net SET sync_out_hirev = 0, sync_out_hirev_datetime = NOW() WHERE invoiceid = @invoice_id";

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
                        status_msg = "success";
                        // Update successful
                        Console.WriteLine($"Rows Updated: {rowsAffected}");

                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success]Sync to Hirev and removed sync flag: invoice_id: " + invoice_id);
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
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
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
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
                }
                finally
                {
                    if(conn != null)
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