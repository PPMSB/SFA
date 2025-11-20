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
using static DotNetSync.SyncBatterySNAxToDotNet;

namespace DotNetSync
{
    public partial class SyncBatterySNAxToDotNet : System.Web.UI.Page
    {
        //public static Axapta WebDynAx = new Axapta();
        private static readonly object lock_object = new object();
        private static readonly object lock_object_subquery = new object();


        public class BatterySN
        {
            public string serial_num { get; set; }
            public string cust_account { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public string cust_name { get; set; }
            public DateTime invoice_date { get; set; }
            public string invoice { get; set; }
            public string salesid { get; set; }
            public string sales_order_id { get; set; }
            public string period { get; set; }
        }


        public class GetTableResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string no_of_records { get; set; }
            public string table_string { get; set; }
            public List<BatterySN> batterysn_list { get; set; }
        }

        public class RunSyncResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string remove_flag_status { get; set; }
            public string remove_flag_status_msg { get; set; }
        }

        public static string get_log_file_name()
        {
            return "Gatepass_Ax_to_DotNet_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        }


        [WebMethod]
        public static string get_table()
        {
            lock (lock_object)
            {
                AxaptaObject ax_query;
                AxaptaObject ax_query_run;
                AxaptaObject ax_query_datasource;
                AxaptaRecord dynrec;

                List<BatterySN> batterysn_list = new List<BatterySN>();

                //Json Response
                string status = "";
                string status_msg = "";
                string no_of_records = "";
                string table = "";
                //string invoice_list = "";
                int count = 0;

                Function_Method_Sync.AddSyncLog(get_log_file_name(), "Sync started: " + DateTime.Now.ToString("HH:mm:ss"));

                try
                {
                    Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);

                    int lf_autoserialtrans_table_id = dynax.GetTableIdWithLock("LF_AutoSerialTrans");

                    ax_query = dynax.CreateAxaptaObject("Query");
                    ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", lf_autoserialtrans_table_id);

                    // SerialNum != ''
                    int serialnum_field_id = dynax.GetFieldIdWithLock(lf_autoserialtrans_table_id, "SerialNum");

                    var serialnum_range = (AxaptaObject)ax_query_datasource.Call("addRange", serialnum_field_id);
                    serialnum_range.Call("value", " !=''");

                    serialnum_range = (AxaptaObject)ax_query_datasource.Call("addRange", serialnum_field_id);
                    serialnum_range.Call("value", " != NULL");

                    // LF_Transfer = 1
                    int lf_transfer_field_id = dynax.GetFieldIdWithLock(lf_autoserialtrans_table_id, "LF_Transfer");

                    var lf_transfer_range = (AxaptaObject)ax_query_datasource.Call("addRange", lf_transfer_field_id);
                    lf_transfer_range.Call("value", "1");

                    // lf_invoicevoid = 0
                    int lf_invoicevoid_field_id = dynax.GetFieldIdWithLock(lf_autoserialtrans_table_id, "LF_InvoiceVoid");

                    var lf_invoicevoid_range = (AxaptaObject)ax_query_datasource.Call("addRange", lf_invoicevoid_field_id);
                    lf_invoicevoid_range.Call("value", "0");

                    ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query);

                    int max_record_count = 0;

                    while ((bool)ax_query_run.Call("next") && max_record_count < 5)
                    {
                        dynrec = (AxaptaRecord)ax_query_run.Call("Get", lf_autoserialtrans_table_id);

                        //string Account = (string)row["Account"];
                        int lf_transfer = (int)dynrec.get_Field("LF_Transfer");
                        string serialnum = (string)dynrec.get_Field("SerialNum");
                        if (string.IsNullOrEmpty(serialnum))
                        {
                            continue;
                        }

                        BatterySN batterysn = new BatterySN();
                        batterysn.serial_num = serialnum;
                        batterysn.cust_account = (string)dynrec.get_Field("CustAccount");
                        batterysn.item_id = (string)dynrec.get_Field("ItemId");
                        batterysn.invoice = (string)dynrec.get_Field("InvoiceId");
                        batterysn.sales_order_id = (string)dynrec.get_Field("SalesId");
                        batterysn.period = ((double)dynrec.get_Field("LF_WarrantyPeriod")).ToString("N0");

                        batterysn_list.Add(batterysn);

                        max_record_count++;
                    }

                    // get the account names for all the account num
                    batterysn_list = get_item_name(batterysn_list); // get cust name, item name, sales id, invoice date
                    batterysn_list = get_account_name(batterysn_list);
                    batterysn_list = get_invoice_date(batterysn_list);

                    table = "<table id='sync_table' class='datatable display stripe hover table dataTable dtr-inline'>" +
                            "<thead><tr>" +
                            "<th>No</th>" +
                            "<th>SN</th>" +
                            "<th>Cust Account</th>" +
                            "<th>Cust Name</th>" +
                            "<th>Invoice</th>" +
                            "<th>Invoice Date</th>" +
                            "<th>Item Id</th>" +
                            "<th>Item Name</th>" +
                            "<th>Sales Id</th>" +
                            "<th>Period</th>" +
                            "<th>Status</th>" +
                            "</tr></thead>" +
                            "<tbody>";

                    for (int i = 0; i < batterysn_list.Count; i++)
                    {
                        count++;
                        BatterySN current_batterysn = batterysn_list[i];

                        //int index_of_slash = current_batterysn.invoice_id.IndexOf('/');

                        table = table + "<tr>" +
                            "<td>" + count + "</td>" +
                            "<td>" + current_batterysn.serial_num + "</td>" +
                            "<td>" + current_batterysn.cust_account + "</td>" +
                            "<td>" + current_batterysn.cust_name + "</td>" +
                            "<td>" + current_batterysn.invoice + "</td>" +
                            "<td>" + current_batterysn.invoice_date.ToString("yyyy-MM-dd") + "</td>" +
                            "<td>" + current_batterysn.item_id + "</td>" +
                            "<td>" + current_batterysn.item_name + "</td>" +
                            "<td>" + current_batterysn.salesid + "</td>" +
                            "<td>" + current_batterysn.period + "</td>" +
                            "<td><label id='" + current_batterysn.serial_num + "_status'></label></td></tr>";
                    }

                    table = table + "</tbody></table>";

                    no_of_records = "" + batterysn_list.Count;

                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "No. of rows to be synced: " + no_of_records);

                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    // Handle X++ exceptions from Business Connector
                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message + Environment.NewLine +
                                 "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                 "Source: " + ex.Source + Environment.NewLine +
                                 "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + status_msg);
                }
                catch(Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
                {
                    status = "failed";
                    status_msg = "LogonFailedException: " + ex.Message + Environment.NewLine +
                                 "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                 "Source: " + ex.Source + Environment.NewLine +
                                 "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[LogonFailedException] " + status_msg);
                }
                catch (Exception ex)
                {
                    status = "failed";
                    status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                 "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                 "Source: " + ex.Source + Environment.NewLine +
                                 "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A"); ;
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);
                    //return ex.Message;
                }
                finally
                {
                    //dynax.Logoff();
                }


                GetTableResponse response = new GetTableResponse
                {
                    status = status,
                    status_msg = status_msg,
                    no_of_records = no_of_records, // Replace with the actual number of records
                    table_string = table,
                    batterysn_list = batterysn_list
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }
            
        }

        [WebMethod]
        //SN, custacc, itemid, itemname, custname, sync_in_date, sync_in_time, invdate, inv, salesid, period, cancel_status
        //public static string run_sync(string invoice_id, string invoice_date, string gatepass, string gatepass_date, string account_num, string account_name, string transporter_code)
        public static string run_sync(string serial_num, string cust_account, string cust_name, string invoice, string invoice_date, string item_id, string item_name, string salesid, string period)
        {
            RunSyncResponse response = new RunSyncResponse();
            /*
            string status;
            string status_msg;
            string remove_flag_status = "";
            string remove_flag_status_msg = "";
            */
            /*
            try { 
                response = mysql_insert_record(response, gatepass, gatepass_date, account_num, account_name, invoice_id, invoice_date, transporter_code);
                       
                //response.status = "success";
                //status_msg = "Completed at " + DateTime.Now.ToString("HH:mm:ss");
                //response.status_msg = "Synced";

            }
            catch (MySqlException ex)
            {
                response.status = "failed";
                response.status_msg = "Exception: " + ex.Message;
                    
                // Handle specific MySQL error codes
                if (ex.Number == 1062) // MySQL error code for duplicate entry
                {
                    // Handle duplicate entry error
                    response.status_msg = "Exception 1062: " + ex.Message;
                }
                else if (ex.Number == 1452) // MySQL error code for foreign key violation
                {
                    // Handle foreign key violation error
                    response.status_msg = "Exception 1452: " + ex.Message;
                }
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][" + invoice_id + "]" + response.status_msg);
            }
            catch (Exception ex)
            {
                response.status = "failed";
                response.status_msg = "Exception: " + ex.Message;
                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][" + invoice_id + "]" + response.status_msg);
            }
            finally
            {

            }

            // Json reponse
            /*
            response = new RunSyncResponse
            {
                status = status,
                status_msg = status_msg,
                remove_flag_status = remove_flag_status,
                remove_flag_status_msg = remove_flag_status_msg
            };
            */
            response = mysql_insert_record(response, serial_num, cust_account, cust_name, invoice, invoice_date, item_id, item_name, salesid, period);
            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }

            
        

        [WebMethod]
        public static RunSyncResponse mysql_insert_record(RunSyncResponse response, string serial_num, string cust_account, string cust_name, string invoice, string invoice_date, string item_id, string item_name, string salesid, string period)
        {
            lock (lock_object)
            {
                MySqlConnection conn = null;
                try
                {
                    //MySqlConnection conn = new MySqlConnection(GLOBAL_Sync.connStr);
                    conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

                    string sql = "SELECT COUNT(*) FROM battery_sn_dot_net WHERE SN = @D1";

                    MySqlCommand mysql_command = new MySqlCommand(sql, conn);

                    MySqlParameter mysql_param = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                    mysql_param.Value = serial_num;
                    mysql_command.Parameters.Add(mysql_param);

                    int existingRecords = 0;

                    conn.Open();
                    existingRecords = Convert.ToInt32(mysql_command.ExecuteScalar());
                    conn.Close();



                    //MySqlConnection conn = new MySqlConnection(GLOBAL_Sync.connStr);
                    if (existingRecords == 0)
                    {
                        int sync_out_hirev = 1;
                        string insert_sql;

                        //Query = "update user_tbl SET logined_user_name=@D1,user_authority_lvl=@D3,page_access_authority=@D4,user_company=@D5,module_access_authority=@D6,Reserve1=@D7,Reserve2=@D8,modified_date=@D9,modified_by=@D10 where user_id=@D2";
                        insert_sql = "INSERT INTO battery_sn_dot_net" +
                            "(SN, custacc, itemid, itemname, custname, invdate, inv, salesid, period, sync_in_date, sync_in_time) " +
                            "VALUES(@D1,@D2,@D3,@D4,@D5,@D6,@D7,@D8,@D9,DATE_FORMAT(CURDATE(), '%Y-%m-%d'),DATE_FORMAT(CURTIME(), '%H:%i:%s'))";

                        MySqlCommand insert_command = new MySqlCommand(insert_sql, conn);



                        MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                        _D1.Value = serial_num;
                        insert_command.Parameters.Add(_D1);

                        MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                        _D2.Value = cust_account;
                        insert_command.Parameters.Add(_D2);

                        MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                        _D3.Value = item_id;
                        insert_command.Parameters.Add(_D3);

                        MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                        _D4.Value = item_name;
                        insert_command.Parameters.Add(_D4);

                        MySqlParameter _D5 = new MySqlParameter("@D5", MySqlDbType.VarChar, 0);
                        _D5.Value = cust_name;
                        insert_command.Parameters.Add(_D5);
                        
                        MySqlParameter _D6 = new MySqlParameter("@D6", MySqlDbType.VarChar, 0);
                        _D6.Value = invoice_date;
                        insert_command.Parameters.Add(_D6);

                        MySqlParameter _D7 = new MySqlParameter("@D7", MySqlDbType.VarChar, 0);
                        _D7.Value = invoice;
                        insert_command.Parameters.Add(_D7);

                        MySqlParameter _D8 = new MySqlParameter("@D8", MySqlDbType.Int32, 0);
                        _D8.Value = salesid;
                        insert_command.Parameters.Add(_D8);

                        MySqlParameter _D9 = new MySqlParameter("@D9", MySqlDbType.Int32, 0);
                        _D9.Value = period;
                        insert_command.Parameters.Add(_D9);

                        //MySqlDataReader MyReader;
                        conn.Open();
                        insert_command.ExecuteNonQuery();

                        conn.Close();
                        conn = null;

                        
                    }

                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success][Sync]SN: " + serial_num +
                                                                          " | Invoice: " + invoice +
                                                                          " | Invoice Date: " + invoice_date +
                                                                          " | Cust Acc: " + cust_account +
                                                                          " | Cust Name: " + cust_name +
                                                                          " | Item Id: " + item_id +
                                                                          " | Item Name: " + item_name +
                                                                          " | Sales Id: " + salesid);

                    response.status = "success";
                    response.status_msg = "Sync";


                }
                catch (MySqlException ex)
                {
                    // Handle specific MySQL error codes
                    if (ex.Number == 1062) // MySQL error code for duplicate entry
                    {
                        // Handle duplicate entry error
                    }
                    else if (ex.Number == 1452) // MySQL error code for foreign key violation
                    {
                        // Handle foreign key violation error
                    }
                    response.status = "failed";
                    response.status_msg = ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                            Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + serial_num + ": " + ex.Message);
                    //throw ex;
                    // Add more specific error code handling as needed...
                }
                catch (Exception ex)
                {
                    //throw ex;
                    response.status = "failed";
                    response.status_msg = ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + serial_num + ": " + ex.Message);
                }
                finally
                {
                    if(conn != null)
                    {
                        conn.Close();
                    }
                }

                /*
                try
                {
                    axapta_update_sync_status(response, invoice_id, 0); // update lf_gatepass_update to 0 once finished copied (1 for testing)
                    response.remove_flag_status = "success";
                    response.remove_flag_status_msg = "Sync Flag Removed";
                }
                catch (MySqlException ex)
                {
                    response.remove_flag_status = "failed";
                    response.remove_flag_status_msg = ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + invoice_id + ": " + ex.Message); 
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Remove Sync Flag]" + invoice_id + ":" + ex.Message);
                    //throw ex;
                    // Add more specific error code handling as needed...
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    response.remove_flag_status = "failed";
                    response.remove_flag_status_msg = ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + invoice_id + ": " + ex.Message); 
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Remove Sync Flag]" + invoice_id + ":" + ex.Message);
                }
                catch (Exception ex)
                {
                    //throw ex;
                    response.remove_flag_status = "failed";
                    response.remove_flag_status_msg = ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + invoice_id + ": " + ex.Message); 
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Remove Sync Flag]" + invoice_id + ":" + ex.Message);
                }
                */

                return response;
            }
        }

        [WebMethod]
        //private void axapta_update_record(string invoice_id, int sync_status) 
        public static void axapta_update_sync_status(RunSyncResponse response, string invoice_id, int sync_status)
        {
            Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name, 
                GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);
            AxaptaRecord dynrec;

            lock (lock_object)
            {
                bool exists = false;
                MySqlConnection conn = null;
                try
                {
                    //MySqlConnection conn = new MySqlConnection(GLOBAL_Sync.connStr);
                    conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string);

                    string sql = "SELECT COUNT(*) FROM lf_gatepass_dot_net WHERE invoiceid = @invoiceId";

                    conn.Open();

                    //MySqlCommand mysql_command = new MySqlCommand(sql, conn);
                    using (MySqlCommand command = new MySqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@invoiceId", invoice_id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            exists = reader.Read(); // Returns true if the invoice exists in MySQL
                        }
                    }

                    conn.Close();
                    conn = null;
                }
                catch (MySqlException ex)
                {
                    if(conn != null)
                    {
                        conn.Close();
                    }
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    throw ex;
                }

                if (exists)
                {
                    try
                    {
                        using (dynrec = dynax.CreateAxaptaRecord("LF_Gatepass"))
                        {
                            dynax.TTSBegin();


                            dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "InvoiceId", invoice_id));
                            if (dynrec.Found)
                            {
                                dynrec.set_Field("lf_gatepass_update", sync_status);
                                dynrec.Call("Update");
                            }

                            dynax.TTSCommit();

                            Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success][Remove sync flag] InvoiceId: " + invoice_id);
                            //dynax.TTSAbort();
                            //dynax.Logoff();
                        }


                    }
                    catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                    {
                        dynax.TTSAbort();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dynax.TTSAbort();
                        throw ex;
                    }
                    finally
                    {
                        // Log off from AX
                        //dynax.Logoff();
                    }
                }
            }
        }

        
        public static List<BatterySN> get_item_name(List<BatterySN> batterysn_list)
        {
            AxaptaObject ax_query;
            AxaptaObject ax_query_run;
            AxaptaObject ax_query_datasource;
            AxaptaRecord dynrec;

            Axapta dynax = Function_Method_Sync.getSubQueryDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                                            GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);


            
            for (int i = 0; i < batterysn_list.Count; i++)
            {
                BatterySN current_batterysn = batterysn_list[i];
                string item_id = current_batterysn.item_id;

                lock (lock_object_subquery)
                {
                    try
                    {
                        int inventtable_table_id = dynax.GetTableIdWithLock("InventTable");

                        ax_query = dynax.CreateAxaptaObject("Query");
                        ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", inventtable_table_id);

                        // 
                        int itemid_field_id = dynax.GetFieldIdWithLock(inventtable_table_id, "ItemId");

                        var itemid_range = (AxaptaObject)ax_query_datasource.Call("addRange", itemid_field_id);
                        itemid_range.Call("value", item_id);


                        ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query);

                        if ((bool)ax_query_run.Call("next"))
                        {
                            dynrec = (AxaptaRecord)ax_query_run.Call("Get", inventtable_table_id);

                            current_batterysn.item_name = (string)dynrec.get_Field("ItemName");
                        }

                    }
                    catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                    {
                        // Handle X++ exceptions from Business Connector
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        // Handle the outer exception (e.g., log it, display an error message)
                        throw ex;
                    }
                }
                
            }
            

            return batterysn_list;
        }



        public static List<BatterySN> get_account_name(List<BatterySN> batterysn_list)
        {
            AxaptaObject ax_query;
            AxaptaObject ax_query_run;
            AxaptaObject ax_query_datasource;
            AxaptaRecord dynrec;

            Axapta dynax = Function_Method_Sync.getSubQueryDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                                            GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);


            for (int i = 0; i < batterysn_list.Count; i++)
            {
                BatterySN current_batterysn = batterysn_list[i];
                string account_num = current_batterysn.cust_account;

                lock (lock_object_subquery)
                {
                    try
                    {
                        int custtable_table_id = dynax.GetTableIdWithLock("CustTable");

                        ax_query = dynax.CreateAxaptaObject("Query");
                        ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", custtable_table_id);

                        // gatepass != ''
                        int custtable_accoutnum_id = dynax.GetFieldIdWithLock(custtable_table_id, "AccountNum");

                        var gatepass_range = (AxaptaObject)ax_query_datasource.Call("addRange", custtable_accoutnum_id);
                        gatepass_range.Call("value", account_num);


                        ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query);

                        if ((bool)ax_query_run.Call("next"))
                        {
                            dynrec = (AxaptaRecord)ax_query_run.Call("Get", custtable_table_id);

                            current_batterysn.cust_name = (string)dynrec.get_Field("Name");
                            current_batterysn.salesid = (string)dynrec.get_Field("emplid");
                        }

                    }
                    catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                    {
                        // Handle X++ exceptions from Business Connector
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        // Handle the outer exception (e.g., log it, display an error message)
                        throw ex;
                    }
                }

            }

            return batterysn_list;
        }

        public static List<BatterySN> get_invoice_date(List<BatterySN> batterysn_list)
        {
            AxaptaObject ax_query;
            AxaptaObject ax_query_run;
            AxaptaObject ax_query_datasource;
            AxaptaRecord dynrec;

            Axapta dynax = Function_Method_Sync.getSubQueryDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                                            GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);



            for (int i = 0; i < batterysn_list.Count; i++)
            {
                BatterySN current_batterysn = batterysn_list[i];
                string invoice_id = current_batterysn.invoice;

                lock (lock_object_subquery)
                {
                    try
                    {
                        int custinvoicejour_table_id = dynax.GetTableIdWithLock("CustInvoiceJour");

                        ax_query = dynax.CreateAxaptaObject("Query");
                        ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", custinvoicejour_table_id);

                        // 
                        int invoiceid_field_id = dynax.GetFieldIdWithLock(custinvoicejour_table_id, "InvoiceId");

                        var invoiceid_range = (AxaptaObject)ax_query_datasource.Call("addRange", invoiceid_field_id);
                        invoiceid_range.Call("value", invoice_id);


                        ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query);

                        if ((bool)ax_query_run.Call("next"))
                        {
                            dynrec = (AxaptaRecord)ax_query_run.Call("Get", custinvoicejour_table_id);

                            current_batterysn.invoice_date = (DateTime)dynrec.get_Field("InvoiceDate");
                        }

                    }
                    catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                    {
                        // Handle X++ exceptions from Business Connector
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        // Handle the outer exception (e.g., log it, display an error message)
                        throw ex;
                    }
                }

            }


            return batterysn_list;
        }

    }
}