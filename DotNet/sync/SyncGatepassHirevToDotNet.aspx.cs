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
using static DotNetSync.SyncGatepassHirevToDotNet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DotNetSync
{
    public partial class SyncGatepassHirevToDotNet : System.Web.UI.Page
    {
        private static readonly object lock_object = new object();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static string get_log_file_name()
        {
            return "Gatepass_Hirev_to_DotNet_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        }

        public class SyncResponse
        {
            public string invoice_id { get; set; }
            public string status { get; set; }
            public string status_msg { get; set; }

        }

        [WebMethod]
        public static string run_sync(string invoice_id, string invoice_date, string gatepass, string gatepass_date, 
                            string account_num, string account_name, string transporter_code,
                            string staff_id, string coordinate, string gps_date, string gps_time,
                            string img_name_1, string img_name_2, string img_name_3, string img_name_4,
                            string img_datetime, string received_datetime, int status, string status_date, string status_by, string status_by_fullname)
        {
            string response_status = "";
            string response_status_msg = "";

            MySqlConnection conn = null;

            lock (lock_object)
            {
                conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

                try
                {
                    string sql = "SELECT COUNT(*) FROM lf_gatepass_dot_net WHERE invoiceid = @invoice_id";

                    MySqlCommand mysql_command = new MySqlCommand(sql, conn);

                    mysql_command.Parameters.AddWithValue("@invoice_id", invoice_id);

                    int existingRecords = 0;

                    int sync_out_hirev = 0;
                    int sync_out_ax = 1;

                    conn.Open();
                    existingRecords = Convert.ToInt32(mysql_command.ExecuteScalar());

                    if (existingRecords == 0)
                    {
                        string insert_sql;

                        

                        insert_sql = "INSERT INTO lf_gatepass_dot_net" +
                            "(invoiceid, invoicedate, gatepass, gatepassdate, accountnum, accountname, transportercode," +
                            "staff_id, coordinate, gps_date, gps_time, img_name_1, img_name_2, img_name_3, img_name_4," +
                            "img_datetime, received_datetime, status, status_date, status_by, status_by_fullname, sync_out_hirev, sync_out_ax) " +
                            "VALUES" +
                            "(@invoice_id, @invoice_date, @gatepass, @gatepass_date, @account_num, @account_name, @transporter_code," +
                            "@staff_id, @coordinate, @gps_date, @gps_time, @img_name_1, @img_name_2, @img_name_3, @img_name_4," +
                            "@img_datetime, @received_datetime, @status, @status_date, @status_by, @status_by_fullname, @sync_out_hirev, @sync_out_ax)";

                        //MySqlConnection insert_conn = Function_Method_Sync.getMySqlConnection();
                        MySqlCommand insert_command = new MySqlCommand(insert_sql, conn);

                        insert_command.Parameters.AddWithValue("@invoice_id", invoice_id);
                        insert_command.Parameters.AddWithValue("@invoice_date", invoice_date);
                        insert_command.Parameters.AddWithValue("@gatepass", gatepass);
                        insert_command.Parameters.AddWithValue("@gatepass_date", gatepass_date);
                        insert_command.Parameters.AddWithValue("@account_num", account_num);
                        insert_command.Parameters.AddWithValue("@account_name", account_name);
                        insert_command.Parameters.AddWithValue("@transporter_code", transporter_code);
                        insert_command.Parameters.AddWithValue("@staff_id", staff_id);
                        insert_command.Parameters.AddWithValue("@coordinate", coordinate);
                        insert_command.Parameters.AddWithValue("@gps_date", gps_date);
                        insert_command.Parameters.AddWithValue("@gps_time", gps_time);
                        insert_command.Parameters.AddWithValue("@img_name_1", img_name_1);
                        insert_command.Parameters.AddWithValue("@img_name_2", img_name_2);
                        insert_command.Parameters.AddWithValue("@img_name_3", img_name_3);
                        insert_command.Parameters.AddWithValue("@img_name_4", img_name_4);
                        insert_command.Parameters.AddWithValue("@img_datetime", img_datetime);
                        insert_command.Parameters.AddWithValue("@received_datetime", received_datetime);
                        insert_command.Parameters.AddWithValue("@status", status);
                        insert_command.Parameters.AddWithValue("@status_date", status_date);
                        insert_command.Parameters.AddWithValue("@status_by", status_by);
                        insert_command.Parameters.AddWithValue("@status_by_fullname", status_by_fullname);
                        insert_command.Parameters.AddWithValue("@sync_out_hirev", sync_out_hirev);
                        insert_command.Parameters.AddWithValue("@sync_out_ax", sync_out_ax);
                        
                        insert_command.ExecuteNonQuery();

                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success][Insert]invoice_id: " + invoice_id +
                                                                          " | invoice_date: " + invoice_date +
                                                                          " | img_datetime: " + img_datetime +
                                                                          " | status_by_fullname: " + status_by_fullname);

                        response_status = "success";
                        response_status_msg = "Insert to DotNet";
                    
                    } else
                    {
                        string update_sql;

                        update_sql = "UPDATE lf_gatepass_dot_net " +
                            "SET staff_id = @staff_id," +
                            "coordinate = @coordinate," +
                            "gps_date = @gps_date," +
                            "gps_time = @gps_time," +
                            "img_name_1 = @img_name_1," +
                            "img_name_2 = @img_name_2, " +
                            "img_name_3 = @img_name_3," +
                            "img_name_4 = @img_name_4," +
                            "img_datetime = @img_datetime," +
                            "received_datetime = @received_datetime," +
                            "status = @status," +
                            "status_date = @status_date," +
                            "status_by = @status_by," +
                            "status_by_fullname = @status_by_fullname," +
                            "sync_out_ax = @sync_out_ax " +
                            "WHERE invoiceid = @invoice_id";

                        MySqlCommand update_command = new MySqlCommand(update_sql, conn);

                        staff_id = staff_id ?? string.Empty;
                        coordinate = coordinate ?? string.Empty;
                        gps_date = gps_date ?? string.Empty;
                        gps_time = gps_time ?? string.Empty;
                        img_name_1 = img_name_1 ?? string.Empty;
                        img_name_2 = img_name_2 ?? string.Empty;
                        img_name_3 = img_name_3 ?? string.Empty;
                        img_name_4 = img_name_4 ?? string.Empty;
                        img_datetime = img_datetime ?? string.Empty;
                        received_datetime = received_datetime ?? string.Empty;
                        status_date = status_date ?? string.Empty;
                        status_by = status_by ?? string.Empty;
                        status_by_fullname = status_by_fullname ?? string.Empty;
                        invoice_id = invoice_id ?? string.Empty;

                        update_command.Parameters.AddWithValue("@staff_id", staff_id);
                        update_command.Parameters.AddWithValue("@coordinate", coordinate);
                        update_command.Parameters.AddWithValue("@gps_date", gps_date);
                        update_command.Parameters.AddWithValue("@gps_time", gps_time);
                        update_command.Parameters.AddWithValue("@img_name_1", img_name_1);
                        update_command.Parameters.AddWithValue("@img_name_2", img_name_2);
                        update_command.Parameters.AddWithValue("@img_name_3", img_name_3);
                        update_command.Parameters.AddWithValue("@img_name_4", img_name_4);
                        update_command.Parameters.AddWithValue("@img_datetime", img_datetime);
                        update_command.Parameters.AddWithValue("@received_datetime", received_datetime);
                        update_command.Parameters.AddWithValue("@status", status);
                        update_command.Parameters.AddWithValue("@status_date", status_date);
                        update_command.Parameters.AddWithValue("@status_by", status_by);
                        update_command.Parameters.AddWithValue("@status_by_fullname", status_by_fullname);
                        update_command.Parameters.AddWithValue("@invoice_id", invoice_id);
                        update_command.Parameters.AddWithValue("@sync_out_ax", sync_out_ax);

                        update_command.ExecuteNonQuery();

                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success][Update]invoice_id: " + invoice_id +
                                                                          " | invoice_date: " + invoice_date +
                                                                          " | img_datetime: " + img_datetime +
                                                                          " | status_by_fullname: " + status_by_fullname);

                        response_status = "success";
                        response_status_msg = "Update to DotNet";

                    }



                }
                catch (MySqlException ex)
                {
                    response_status = "failed";
                    response_status_msg = "MySqlException: " + ex.Message;

                    // Handle specific MySQL error codes
                    if (ex.Number == 1062) // MySQL error code for duplicate entry
                    {
                        // Handle duplicate entry error
                        response_status_msg = "MySqlException 1062 Duplicate entry: " + ex.Message;
                    }
                    else if (ex.Number == 1452) // MySQL error code for foreign key violation
                    {
                        // Handle foreign key violation error
                        response_status_msg = "MySqlException 1452 Foreign key violation: " + ex.Message;
                    }

                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[MySqlException]invoice_id: " + invoice_id +
                                                                          " | " + response_status_msg);
                }
                catch (Exception ex)
                {
                    response_status = "failed";
                    response_status_msg = "Exception: " + ex.Message;

                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception]invoice_id: " + invoice_id +
                                                                          " | " + response_status_msg);
                }
                finally
                {
                    conn.Close();
                    conn = null;
                }

                SyncResponse response = new SyncResponse
                {
                    status = response_status,
                    status_msg = response_status_msg,
                    invoice_id = invoice_id
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }

           
        }      
    }
}
