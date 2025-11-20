using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using DotNetSync.lhdn;
using DotNetSync.lhdn.Controllers;

namespace DotNetSync.lhdn
{
    public partial class EInvoiceDashboard : System.Web.UI.Page
    {
        private static readonly object lock_object = new object();

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static Constants constants = new Constants();

        public class GetCountResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public int count { get; set; }
        }


        [WebMethod]
        public static string Get_Count(string company, string invoice_from_date, string invoice_to_date, string submission_status)
        {
            lock (lock_object)
            {
                //Json Response
                string status = "";
                string status_msg = "";
                int count = 0;
                string invoice_date_from_to = $"{invoice_from_date}..{invoice_to_date}";

                //Function_Method_Sync.AddSyncLog(get_log_file_name(), "Read started: " + DateTime.Now.ToString("HH:mm:ss"));
                logger.Info($"Get outbount invoice count (LF_Status == {submission_status})");

                try
                {
                    //using (Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                    //    GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server))
                    //using (Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                    //    GLOBAL_Sync.ax_proxy_password, company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server))
                    using(Axapta dynax = AxController.get_dynax_connection(company))
                    {
                        // Get table ID for lf_einvoicegov table
                        int table_id_lf_einvoicegov = dynax.GetTableIdWithLock("lf_einvoicegov");

                        // Create a new query object
                        using (var ax_query = dynax.CreateAxaptaObject("Query"))
                        {
                            // Add lf_einvoicegov table as a data source to the query
                            using (var ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", table_id_lf_einvoicegov))
                            {
                                // Get field IDs for filtering
                                //int field_id_supp_reg_no = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "SuppRegNo");
                                //int field_id_lf_uuid = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_UUID");
                                int field_id_lf_status = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Status");
                                int field_id_invoice_id = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "InvoiceId");

                                // Add range for SuppRegNo (assuming company is a string variable containing the value)
                                //var supp_reg_no_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_supp_reg_no);
                                //supp_reg_no_range.Call("value", company);

                                // Add range for LF_UUID to filter empty or null values
                                //var lf_uuid_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_uuid);
                                //lf_uuid_range.Call("value", ""); // Empty string
                                //lf_uuid_range.Call("status", "NULL"); // Handle NULL or empty values

                                // Add range for LF_Status to filter values between 0 and 4
                                var lf_status_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_status);
                                lf_status_range.Call("value", submission_status);

                                if(!(string.IsNullOrEmpty(invoice_from_date) && string.IsNullOrEmpty(invoice_to_date)))
                                {
                                    // Add range for InvoiceDate
                                    int field_id_invoice_date = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "InvoiceDate");

                                    var invoice_date_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_invoice_date);
                                    invoice_date_range.Call("value", invoice_date_from_to);
                                }

                                // Add a group by clause for InvoiceId
                                ax_query_datasource.Call("addGroupByField", field_id_invoice_id);

                                // Create QueryRun object and execute the query
                                using (var ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query))
                                {

                                    while ((bool)ax_query_run.Call("next"))
                                    {
                                        count++;
                                    }

                                    status = "success";
                                    //status_msg = "success";
                                    status_msg = $"Total invoices: {count}";
                                }
                            }
                        }
                    }
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    // Handle X++ exceptions from Business Connector
                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message;

                    logger.Error($"XppException: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }

                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
                {
                    status = "failed";
                    status_msg = "LogonFailedException: " + ex.Message;

                    logger.Error($"LogonFailedException: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                catch (Exception ex)
                {
                    status = "failed";
                    status_msg = "Exception: " + ex.Message;

                    logger.Error($"Exception: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                finally
                {
                    //dynax.Logoff();
                    AxController.dispose_dynax_connection();
                }


                GetCountResponse response = new GetCountResponse
                {
                    status = status,
                    status_msg = status_msg,
                    count = count
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }
        }


        [WebMethod]
        public static string Get_Outbound_Count(string company)
        {
            lock (lock_object)
            {
                //AxaptaObject ax_query;
                //AxaptaObject ax_query_run;
                //AxaptaObject ax_query_datasource;
                //AxaptaRecord dynrec;

                //Json Response
                string status = "";
                string status_msg = "";
                int count = 0;

                //Function_Method_Sync.AddSyncLog(get_log_file_name(), "Read started: " + DateTime.Now.ToString("HH:mm:ss"));
                logger.Info("Get outbount invoice count (LF_Status == 0)");

                try
                {
                    //using (Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                    //    GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server))
                    using (Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server))
                    {
                        // Get table ID for lf_einvoicegov table
                        int table_id_lf_einvoicegov = dynax.GetTableIdWithLock("lf_einvoicegov");

                        // Create a new query object
                        using (var ax_query = dynax.CreateAxaptaObject("Query"))
                        {
                            // Add lf_einvoicegov table as a data source to the query
                            using (var ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", table_id_lf_einvoicegov))
                            {
                                // Get field IDs for filtering
                                //int field_id_supp_reg_no = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "SuppRegNo");
                                //int field_id_lf_uuid = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_UUID");
                                int field_id_lf_status = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Status");
                                int field_id_invoice_id = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "InvoiceId");

                                // Add range for SuppRegNo (assuming company is a string variable containing the value)
                                //var supp_reg_no_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_supp_reg_no);
                                //supp_reg_no_range.Call("value", company);

                                // Add range for LF_UUID to filter empty or null values
                                //var lf_uuid_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_uuid);
                                //lf_uuid_range.Call("value", ""); // Empty string
                                //lf_uuid_range.Call("status", "NULL"); // Handle NULL or empty values

                                // Add range for LF_Status to filter values between 0 and 4
                                var lf_status_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_status);
                                lf_status_range.Call("value", "0..1");

                                // Add a group by clause for InvoiceId
                                ax_query_datasource.Call("addGroupByField", field_id_invoice_id);

                                // Create QueryRun object and execute the query
                                using (var ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query))
                                {

                                    while ((bool)ax_query_run.Call("next"))
                                    {
                                        count++;
                                    }

                                    status = "success";
                                    //status_msg = "success";
                                    status_msg = $"Total invoices: {count}";
                                }
                            }                            
                        }
                    }
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    // Handle X++ exceptions from Business Connector
                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message;

                    logger.Error($"XppException: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }

                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
                {
                    status = "failed";
                    status_msg = "LogonFailedException: " + ex.Message;

                    logger.Error($"LogonFailedException: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                catch (Exception ex)
                {
                    status = "failed";
                    status_msg = "Exception: " + ex.Message;

                    logger.Error($"Exception: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                finally
                {
                    //dynax.Logoff();
                }


                GetCountResponse response = new GetCountResponse
                {
                    status = status,
                    status_msg = status_msg,
                    count = count
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }
        }

        [WebMethod]
        public static string Get_Completed_Count(string company)
        {
            lock (lock_object)
            {
                //AxaptaObject ax_query;
                //AxaptaObject ax_query_run;
                //AxaptaObject ax_query_datasource;
                //AxaptaRecord dynrec;

                //Json Response
                string status = "";
                string status_msg = "";
                int count = 0;

                logger.Info("Get completed invoice count (LF_Status == 5)");

                try
                {
                    //Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                    //    GLOBAL_Sync.ax_proxy_password, GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);
                    Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);

                    // Get table ID for lf_einvoicegov table
                    int table_id_lf_einvoicegov = dynax.GetTableIdWithLock("lf_einvoicegov");

                    // Create a new query object
                    using (var ax_query = dynax.CreateAxaptaObject("Query"))
                    {
                        // Add lf_einvoicegov table as a data source to the query
                        using (var ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", table_id_lf_einvoicegov))
                        {
                            // Get field IDs for filtering
                            //int field_id_supp_reg_no = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "SuppRegNo");
                            //int field_id_lf_uuid = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_UUID");
                            int field_id_lf_status = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Status");
                            int field_id_invoice_id = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "InvoiceId");

                            // Add range for SuppRegNo (assuming company is a string variable containing the value)
                            //var supp_reg_no_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_supp_reg_no);
                            //supp_reg_no_range.Call("value", company);

                            // Add range for LF_UUID to filter empty or null values
                            //var lf_uuid_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_uuid);
                            //lf_uuid_range.Call("value", ""); // Empty string
                            //lf_uuid_range.Call("status", "NULL"); // Handle NULL or empty values

                            // Add range for LF_Status to filter values between 0 and 4
                            var lf_status_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_status);
                            lf_status_range.Call("value", "5");

                            // Add a group by clause for InvoiceId
                            ax_query_datasource.Call("addGroupByField", field_id_invoice_id);

                            // Create QueryRun object and execute the query
                            using (var ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query))
                            {
                                string result = "";
                                while ((bool)ax_query_run.Call("next"))
                                {
                                    count++;
                                    using (var dynrec = (AxaptaRecord)ax_query_run.Call("Get", table_id_lf_einvoicegov))
                                    {
                                        result = count + ":" + result + (string)dynrec.get_Field("InvoiceId") + " ||";
                                    }
                                }

                                status = "success";
                                //status_msg = "success";
                                status_msg = result;
                            }
                        }
                    }
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
                {
                    // Handle X++ exceptions from Business Connector
                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message;

                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message;

                    logger.Error($"XppException: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
                {
                    status = "failed";
                    status_msg = "LogonFailedException: " + ex.Message;

                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message;

                    logger.Error($"LogonFailedException: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                catch (Exception ex)
                {
                    status = "failed";
                    status_msg = "Exception: " + ex.Message;

                    status = "failed";
                    status_msg = "X++ Exception: " + ex.Message;

                    logger.Error($"Exception: {status_msg}");
                    logger.Error($"Stack Trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner Exception: {ex.InnerException.Message}");
                        logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                    }
                }
                finally
                {
                    //dynax.Logoff();
                }


                GetCountResponse response = new GetCountResponse
                {
                    status = status,
                    status_msg = status_msg,
                    count = count
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }
        }
    }
}