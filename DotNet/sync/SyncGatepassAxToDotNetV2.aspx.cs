using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//using System.Data;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using GLOBAL_VAR;
//using EncryptStringSample;
using GLOBAL_FUNCTION;
//using System.Web.UI.HtmlControls;
//using static System.Windows.Forms.AxHost;
//using System.Net;
//using System.Security.Principal;
//using System.Web.Management;
using System.Web.Services;
using Newtonsoft.Json;
//using System.Collections;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DotNetSync
{
    public partial class SyncGatepassAxToDotNetV2 : System.Web.UI.Page
    {
        //public static Axapta WebDynAx = new Axapta();
        private static readonly object lock_object = new object();
        private static readonly object lock_object_subquery = new object();
        private static readonly object mysql_lock_object = new object();

        public class Invoice
        {
            public string gatepass { get; set; }
            public DateTime gatepass_date { get; set; }
            public string account_num { get; set; }
            public string account_name { get; set; }
            public string invoice_id { get; set; }
            public DateTime invoice_date { get; set; }
            public string transporter_code { get; set; }
            public string transporter_name { get; set; }
            public int sync_status { get; set; }
            public string sync_status_message { get; set; }
            public int remove_sync_flag_status { get; set; }
            public string remove_sync_flag_message { get; set; }
        }


        public class GetTableResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string no_of_records { get; set; }
            public string table_string { get; set; }
            public List<Invoice> invoice_list { get; set; }
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
                Axapta dynax = null;
                AxaptaObject ax_query;
                AxaptaObject ax_query_run;
                AxaptaObject ax_query_datasource;
                AxaptaRecord dynrec;

                List<Invoice> invoice_list = new List<Invoice>();


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
                    dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);

                    int lf_gatepass_table_id = dynax.GetTableIdWithLock("LF_Gatepass");

                    ax_query = dynax.CreateAxaptaObject("Query");
                    ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", lf_gatepass_table_id);

                    // gatepass != ''
                    int gatepass_field_id = dynax.GetFieldIdWithLock(lf_gatepass_table_id, "gatepass");

                    var gatepass_range = (AxaptaObject)ax_query_datasource.Call("addRange", gatepass_field_id);
                    gatepass_range.Call("value", " !=''");

                    gatepass_range = (AxaptaObject)ax_query_datasource.Call("addRange", gatepass_field_id);
                    gatepass_range.Call("value", " != NULL");


                    // lf_syn = 1
                    int lf_syn_field_id = dynax.GetFieldIdWithLock(lf_gatepass_table_id, "LF_Syn");

                    var lf_syn_range = (AxaptaObject)ax_query_datasource.Call("addRange", lf_syn_field_id);
                    lf_syn_range.Call("value", "1");
                    // 2024-04-29 - Jerry - End

                    ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query);

                    int max_record_count = 0;

                    while ((bool)ax_query_run.Call("next") && max_record_count < 50)
                    {
                        dynrec = (AxaptaRecord)ax_query_run.Call("Get", lf_gatepass_table_id);

                        string gatepass = (string)dynrec.get_Field("GatePass");
                        if (string.IsNullOrEmpty(gatepass))
                        {
                            continue;
                        }

                        Invoice invoice = new Invoice();
                        invoice.invoice_id = (string)dynrec.get_Field("InvoiceId");
                        invoice.invoice_date = (DateTime)dynrec.get_Field("InvoiceDate");
                        invoice.gatepass = gatepass;
                        invoice.gatepass_date = (DateTime)dynrec.get_Field("GatePassDate");
                        invoice.account_num = (string)dynrec.get_Field("AccountNum");
                        invoice.account_name = (string)dynrec.Call("getCustomerName"); // call LF_Gatepass method
                        invoice.transporter_code = (string)dynrec.get_Field("TransporterCode");
                        invoice.transporter_name = (string)dynrec.get_Field("TransporterName");

                        invoice_list.Add(invoice);

                        max_record_count++;
                    }

                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;

                    //invoice_list = get_account_name(invoice_list);
                    invoice_list = mysql_insert_record(invoice_list);
                    invoice_list = axapta_update_sync_status(invoice_list);


                    // get the account names for all the account num
                    invoice_list = get_account_name(invoice_list);

                    table = "<table id='sync_table' class='datatable display stripe hover table dataTable dtr-inline'>" +
                            "<thead><tr>" +
                            "<th>No</th>" +
                            "<th>Gatepass</th>" +
                            "<th>Gatepass Date</th>" +
                            "<th>Invoice Id</th>" +
                            "<th>Invoice Date</th>" +
                            "<th>Account Num</th>" +
                            "<th>Account Name</th>" +
                            "<th>Transporter Code</th>" +
                            "<th>Transporter Name</th>" +
                            "<th>Status</th>" +
                            "</tr></thead>" +
                            "<tbody>";

                    for (int i = 0; i < invoice_list.Count; i++)
                    {
                        count++;
                        Invoice current_invoice = invoice_list[i];

                        string label_sync_status = "";
                        if (current_invoice.sync_status == 0)
                        {
                            label_sync_status = "failed_label";
                        }
                        else
                        {
                            if (current_invoice.sync_status_message == "INSERT")
                            {
                                label_sync_status = "success_label";
                            }
                            else
                            {
                                label_sync_status = "exists_label";
                            }
                        }

                        string label_remove_sync_status = "";
                        if (current_invoice.remove_sync_flag_status == 0)
                        {
                            label_remove_sync_status = "failed_label";
                        }
                        else
                        {
                            label_remove_sync_status = "success_label";
                        }

                        table = table + "<tr>" +
                            "<td>" + count + "</td>" +
                            "<td>" + current_invoice.gatepass + "</td>" +
                            "<td>" + current_invoice.gatepass_date.ToString("yyyy-MM-dd") + "</td>" +
                            "<td>" + current_invoice.invoice_id + "</td>" +
                            "<td>" + current_invoice.invoice_date.ToString("yyyy-MM-dd") + "</td>" +
                            "<td>" + current_invoice.account_num + "</td>" +
                            "<td>" + current_invoice.account_name + "</td>" +
                            "<td>" + current_invoice.transporter_code + "</td>" +
                            "<td>" + current_invoice.transporter_name + "</td>" +
                            $"<td><span class='{label_sync_status}'>{current_invoice.sync_status_message}</span><br/>" +
                            $"<span class='{label_remove_sync_status}'>{current_invoice.remove_sync_flag_message}</span></td></tr>";
                    }

                    table = table + "</tbody></table>";

                    no_of_records = "" + invoice_list.Count;
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
                    status_msg = "LogonFailedException: " + ex.Message + Environment.NewLine +
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
                    status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + status_msg);

                    if (dynax != null)
                    {
                        // Close the Axapta instance
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                    //return ex.Message;
                }
                finally
                {
                    //dynax.Logoff();
                    if (dynax != null)
                    {
                        // Close the Axapta instance
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                }


                GetTableResponse response = new GetTableResponse
                {
                    status = status,
                    status_msg = status_msg,
                    no_of_records = no_of_records, // Replace with the actual number of records
                    table_string = table,
                    invoice_list = invoice_list
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }

        }


        public static List<Invoice> mysql_insert_record(List<Invoice> invoice_list)
        {
            lock (mysql_lock_object)
            {
                try
                {
                    using (MySqlConnection conn = Function_Method_Sync.getMySqlConnection(GLOBAL_Sync.mysql_conn_string))
                    {
                        conn.Open();

                        string check_query = "SELECT COUNT(*) FROM lf_gatepass_dot_net WHERE invoiceid = @invoiceid";
                        string insert_query = "INSERT INTO lf_gatepass_dot_net" +
                            "(gatepass, gatepassdate, accountnum, accountname, invoiceid, invoicedate, transportercode, sync_out_hirev) " +
                            "VALUES(@gatepass,@gatepassdate,@accountnum,@accountname,@invoiceid,@invoicedate,@transportercode, 1)";

                        using (MySqlCommand check_cmd = new MySqlCommand(check_query, conn))
                        using (MySqlCommand insert_cmd = new MySqlCommand(insert_query, conn))
                        {
                            foreach (Invoice invoice in invoice_list)
                            {
                                string gatepass = invoice.gatepass;
                                DateTime gatepassdate = invoice.gatepass_date;
                                string accountnum = invoice.account_num;
                                string accountname = invoice.account_name;
                                string invoiceid = invoice.invoice_id;
                                DateTime invoicedate = invoice.invoice_date;
                                string transportercode = invoice.transporter_code;

                                try
                                {
                                    check_cmd.Parameters.Clear();
                                    check_cmd.Parameters.AddWithValue("@invoiceid", invoiceid);

                                    int existing_records = Convert.ToInt32(check_cmd.ExecuteScalar());

                                    if (existing_records == 0)
                                    {
                                        insert_cmd.Parameters.Clear();
                                        insert_cmd.Parameters.AddWithValue("@gatepass", gatepass);
                                        insert_cmd.Parameters.AddWithValue("@gatepassdate", gatepassdate.ToString("yyyy-MM-dd"));
                                        insert_cmd.Parameters.AddWithValue("@accountnum", accountnum);
                                        insert_cmd.Parameters.AddWithValue("@accountname", accountname);
                                        insert_cmd.Parameters.AddWithValue("@invoiceid", invoiceid);
                                        insert_cmd.Parameters.AddWithValue("@invoicedate", invoicedate.ToString("yyyy-MM-dd"));
                                        insert_cmd.Parameters.AddWithValue("@transportercode", transportercode);

                                        insert_cmd.ExecuteNonQuery();

                                        invoice.sync_status = 1;
                                        invoice.sync_status_message = "INSERT";
                                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success][Sync]Invoice ID: " + invoiceid +
                                                                            " | Invoice Date: " + invoicedate +
                                                                            " | Trans Code: " + transportercode +
                                                                            " | Gatepass: " + gatepass +
                                                                            " | Gatepass Date: " + gatepassdate +
                                                                            " | Account Num: " + accountnum +
                                                                            " | Account Name: " + accountname);
                                    }
                                    else
                                    {
                                        invoice.sync_status = 1;
                                        invoice.sync_status_message = "EXISTS";
                                        Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exists][Sync]Invoice ID: " + invoiceid +
                                                                            " | Invoice Date: " + invoicedate +
                                                                            " | Trans Code: " + transportercode +
                                                                            " | Gatepass: " + gatepass +
                                                                            " | Gatepass Date: " + gatepassdate +
                                                                            " | Account Num: " + accountnum +
                                                                            " | Account Name: " + accountname);
                                    }
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
                                    invoice.sync_status = 0;
                                    invoice.sync_status_message = ex.Message + Environment.NewLine +
                                                            "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                            "Source: " + ex.Source + Environment.NewLine +
                                                            "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + invoiceid + ": " + ex.Message);

                                }
                                catch (Exception ex)
                                {
                                    //throw ex;
                                    invoice.sync_status = 0;
                                    invoice.sync_status_message = ex.Message + Environment.NewLine +
                                                            "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                            "Source: " + ex.Source + Environment.NewLine +
                                                            "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception][Insert]" + invoiceid + ": " + ex.Message);
                                }
                            }
                        }

                        conn.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return invoice_list;
        }

        public static List<Invoice> axapta_update_sync_status(List<Invoice> invoice_list)
        {
            Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);
            //AxaptaRecord dynrec;


            lock (lock_object)
            {
                try
                {
                    dynax.TTSBegin();

                    foreach (Invoice invoice in invoice_list)
                    {
                        string invoiceid = invoice.invoice_id;
                        int sync_status = invoice.sync_status;

                        if (sync_status == 0)
                        {
                            continue;
                        }

                        /*
                        using (dynrec = dynax.CreateAxaptaRecord("LF_Gatepass"))
                        {
                            dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "InvoiceId", invoiceid));
                            if (dynrec.Found)
                            {
                                dynrec.set_Field("LF_Syn", 0);
                                dynrec.Call("Update");

                                invoice.remove_sync_flag_status = 1;
                                invoice.remove_sync_flag_message = "REMOVED LF_Syn";                               
                            }
                        }*/
                        using (AxaptaRecord dynrec = dynax.CreateAxaptaRecord("LF_Gatepass"))
                        {
                            string query = $"select forupdate * from %1 where %1.InvoiceId == '{invoice.invoice_id}'";
                            dynrec.ExecuteStmt(query);

                            if (dynrec.Found)
                            {
                                dynrec.set_Field("LF_Syn", 0);
                                dynrec.Call("Update");

                                invoice.remove_sync_flag_status = 1;
                                invoice.remove_sync_flag_message = "REMOVED LF_Syn";
                            }
                        }
                    }
                    dynax.TTSCommit();
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    foreach (Invoice invoice in invoice_list)
                    {
                        invoice.remove_sync_flag_status = 0;
                        invoice.remove_sync_flag_message = ex.Message;
                    }

                    string error_msg = "X++ Exception: " + ex.Message + Environment.NewLine +
                                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                     "Source: " + ex.Source + Environment.NewLine +
                                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + error_msg);

                    if (dynax != null)
                    {
                        dynax.TTSAbort();
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                    throw ex;
                }
                catch (Exception ex)
                {
                    foreach (Invoice invoice in invoice_list)
                    {
                        invoice.remove_sync_flag_status = 0;
                        invoice.remove_sync_flag_message = ex.Message;
                    }

                    string error_msg = "Exception: " + ex.Message + Environment.NewLine +
                                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                     "Source: " + ex.Source + Environment.NewLine +
                                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                    Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + error_msg);

                    if (dynax != null)
                    {
                        dynax.TTSAbort();
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                    throw ex;
                }
                finally
                {
                    // Log off from AX
                    if (dynax != null)
                    {
                        dynax.Logoff();
                        dynax.Dispose();
                        dynax = null;
                    }
                }

            }

            return invoice_list;
        }

        // obselete
        public static List<Invoice> axapta_update_sync_status2(List<Invoice> invoice_list)
        {
            Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);
            AxaptaRecord dynrec;


            lock (lock_object)
            {
                try
                {
                    foreach (Invoice invoice in invoice_list)
                    {
                        string invoiceid = invoice.invoice_id;
                        int sync_status = invoice.sync_status;

                        if (sync_status == 0)
                        {
                            continue;
                        }

                        using (dynrec = dynax.CreateAxaptaRecord("LF_Gatepass"))
                        {


                            try
                            {
                                dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "InvoiceId", invoiceid));
                                if (dynrec.Found)
                                {
                                    dynax.TTSBegin();

                                    dynrec.set_Field("LF_Syn", 0);
                                    dynrec.Call("Update");

                                    invoice.remove_sync_flag_status = 1;
                                    invoice.remove_sync_flag_message = "REMOVED LF_Syn";


                                    dynax.TTSCommit();
                                }

                            }
                            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                            {
                                Console.WriteLine(ex.Message);
                                // Handle X++ exceptions from Business Connector

                                invoice.remove_sync_flag_status = 0;
                                invoice.remove_sync_flag_message = "X++ Exception: " + ex.Message + Environment.NewLine +
                                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                     "Source: " + ex.Source + Environment.NewLine +
                                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + invoice.remove_sync_flag_message);


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
                                Console.WriteLine(ex.Message);

                                invoice.remove_sync_flag_status = 0;
                                invoice.remove_sync_flag_message = "LogonFailedException: " + ex.Message + Environment.NewLine +
                                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                     "Source: " + ex.Source + Environment.NewLine +
                                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[LogonFailedException] " + invoice.remove_sync_flag_message);

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
                                Console.WriteLine(ex.Message);

                                invoice.remove_sync_flag_status = 0;
                                invoice.remove_sync_flag_message = "Exception: " + ex.Message + Environment.NewLine +
                                                     "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                                     "Source: " + ex.Source + Environment.NewLine +
                                                     "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                                Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + invoice.remove_sync_flag_message);
                                //return ex.Message;

                            }
                            finally
                            {
                                //dynax.Logoff();
                            }



                        }

                    }
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    dynax.TTSAbort();
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                    throw ex;
                }
                catch (Exception ex)
                {
                    dynax.TTSAbort();
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                    throw ex;
                }
                finally
                {
                    // Log off from AX
                    //dynax.Logoff();
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }

            }

            return invoice_list;
        }

        // obselete
        public static List<Invoice> get_account_name(List<Invoice> invoice_list)
        {
            AxaptaObject ax_query;
            AxaptaObject ax_query_run;
            AxaptaObject ax_query_datasource;
            AxaptaRecord dynrec;

            Axapta dynax = Function_Method_Sync.getSubQueryDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                                            GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);

            try
            {
                /*
                for (int i = 0; i < invoice_list.Count; i++)
                {
                    Invoice current_invoice = invoice_list[i];
                    string account_num = current_invoice.account_num;

                    lock (lock_object_subquery)
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

                            current_invoice.account_name = (string)dynrec.get_Field("Name");
                        }
                    }
                }*/
                int cust_table_id = dynax.GetTableIdWithLock("CustTable");
                int accountnum_field_id = dynax.GetFieldIdWithLock(cust_table_id, "AccountNum");

                foreach (Invoice current_invoice in invoice_list)
                {
                    string accountNum = current_invoice.account_num;

                    lock (lock_object_subquery)
                    {
                        get_account_name(dynax, current_invoice, cust_table_id, accountnum_field_id, accountNum);
                    }
                }
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
            {
                // Handle X++ exceptions from Business Connector
                if (dynax != null)
                {
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }
                throw ex;
            }
            catch (Exception ex)
            {
                // Handle the outer exception (e.g., log it, display an error message)
                if (dynax != null)
                {
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }
                throw ex;
            }
            finally
            {
                if (dynax != null)
                {
                    dynax.Logoff();
                    dynax.Dispose();
                    dynax = null;
                }
            }


            return invoice_list;
        }

        // obselete
        private static void get_account_name(Axapta dynax, Invoice current_invoice, int cust_table_id, int accountnum_field_id, string accountNum)
        {
            using (AxaptaObject axQuery = dynax.CreateAxaptaObject("Query"))
            {
                using (AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", cust_table_id))
                {
                    var gatepassRange = (AxaptaObject)axQueryDataSource.Call("addRange", accountnum_field_id);
                    gatepassRange.Call("value", accountNum);

                    using (AxaptaObject axQueryRun = dynax.CreateAxaptaObject("QueryRun", axQuery))
                    {
                        if ((bool)axQueryRun.Call("next"))
                        {
                            using (AxaptaRecord dynrec = (AxaptaRecord)axQueryRun.Call("Get", cust_table_id))
                            {
                                current_invoice.account_name = (string)dynrec.get_Field("Name");
                            }
                        }
                    }
                }
            }
        }

    }
}