using DotNetSync.lhdn.Controllers;
using DotNetSync.lhdn.Models;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DotNetSync.lhdn
{
    public partial class EInvoiceInvoiceList : System.Web.UI.Page
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly object lock_object = new object();
        private static readonly object lock_object_subquery = new object();

        /*
        public static readonly string SUBMISSION_ALL = "100";
        public static readonly string SUBMISSION_NO = "0";
        public static readonly string SUBMISSION_PENDING_LONG_ID = "3";
        public static readonly string SUBMISSION_UUID_FAILED = "2";
        public static readonly string SUBMISSION_LONG_ID_FAILED = "4";
        public static readonly string SUBMISSION_COMPLETED = "5";
        */

        public static Constants constants = new Constants();

        public static readonly string MISSING_TIN_BRN_NO = "100";
        public static readonly string MISSING_TIN_BRN_YES = "101";

        public static readonly string ONLY_MISSING_TIN_BRN_NO = "102";
        public static readonly string ONLY_MISSING_TIN_BRN_YES = "103";

        public string comregno = "-";
        public string submission_status = constants.SUBMISSION_ALL;
        public string include_missing_brn_tin = MISSING_TIN_BRN_YES;
        public string invoice_date = "";
        public string invoice_from_date = "";
        public string invoice_to_date = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the customercode parameter from the URL
            string req_comregno = Request.QueryString["comregno"];
            string status = Request.QueryString["status"];
            string from_date = Request.QueryString["from_date"];
            string to_date = Request.QueryString["to_date"];
            string tinbrn = Request.QueryString["tinbrn"];

            // Assign the value of customerCode to comregno
            comregno = req_comregno;
            submission_status = status;
            include_missing_brn_tin = tinbrn;
            invoice_from_date = from_date;
            invoice_to_date = to_date;
        }

        public class GetTableResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string no_of_records { get; set; }
            public string table_string { get; set; }
            //public List<Invoice> invoice_list { get; set; }
            public List<EInvoice> einvoice_list { get; set; }
        }

        public class RequeueResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
        }

        [WebMethod]
        public static string requeue_invoices(List<string> invoice_ids)
        {
            // foreach bla bla
            return "";
        }


        [WebMethod]
        public static string get_table(string company, string submission_status, string include_missing_tinbrn, string only_missing_tinbrn, string invoice_from_date, string invoice_to_date)
        {
            lock (lock_object)
            {
                AxaptaObject ax_query;
                AxaptaObject ax_query_run;
                AxaptaObject ax_query_datasource;
                AxaptaRecord dynrec;

                List<EInvoice> einvoice_list = new List<EInvoice>();

                //Json Response
                string status = "";
                string status_msg = "";
                string no_of_records = "";
                string table = "";
                //string invoice_list = "";
                int count = 0;

                string default_from_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("d/M/yyyy");
                string default_to_date = DateTime.Now.ToString("d/M/yyyy");

                if (string.IsNullOrEmpty(invoice_from_date))
                {
                    invoice_from_date = default_from_date;
                }

                if (string.IsNullOrEmpty(invoice_to_date))
                {
                    invoice_to_date = default_to_date;
                }

                string ax_invoice_date_range = $"{invoice_from_date}..{invoice_to_date}";

                //List<EInvoice> einvoices = new List<EInvoice>();

                //Function_Method_Sync.AddSyncLog(get_log_file_name(), "Read started: " + DateTime.Now.ToString("HH:mm:ss"));
                logger.Info("Getting outbound list");

                try
                {
                    /*
                    Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);
                    */
                    using (Axapta dynax = AxController.get_dynax_connection(company))
                    {

                        int table_id_lf_einvoicegov = dynax.GetTableIdWithLock("lf_einvoicegov");

                        ax_query = dynax.CreateAxaptaObject("Query");
                        ax_query_datasource = (AxaptaObject)ax_query.Call("addDataSource", table_id_lf_einvoicegov);


                        // Add range for LF_Status
                        int field_id_lf_status = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Status");
                        if (submission_status != constants.SUBMISSION_ALL)
                        {
                            var lf_status_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_lf_status);
                            lf_status_range.Call("value", submission_status);
                        }

                        // Add range for InvoiceDate
                        int field_id_invoice_date = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "InvoiceDate");

                        var invoice_date_range = (AxaptaObject)ax_query_datasource.Call("addRange", field_id_invoice_date);
                        invoice_date_range.Call("value", ax_invoice_date_range);



                        // group by
                        int field_id_invoice_id = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "InvoiceId");
                        ax_query_datasource.Call("addGroupByField", field_id_invoice_id);

                        ax_query_datasource.Call("addGroupByField", field_id_invoice_date);

                        int field_id_original_invoice_id = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "OrigInvoiceId");
                        ax_query_datasource.Call("addGroupByField", field_id_original_invoice_id);

                        int field_id_buyer_name = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "BuyerName");
                        ax_query_datasource.Call("addGroupByField", field_id_buyer_name);

                        int field_id_buyer_tin = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "BuyerTIN");
                        ax_query_datasource.Call("addGroupByField", field_id_buyer_tin);

                        int field_id_buyer_brn = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "BuyerRegNo");
                        ax_query_datasource.Call("addGroupByField", field_id_buyer_brn);

                        int field_id_submission_uid = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_SubmissionId");
                        ax_query_datasource.Call("addGroupByField", field_id_submission_uid);

                        int field_id_uuid = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_UUID");
                        ax_query_datasource.Call("addGroupByField", field_id_uuid);

                        int field_id_long_id = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_LongId");
                        ax_query_datasource.Call("addGroupByField", field_id_long_id);

                        int field_id_status = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Status");
                        ax_query_datasource.Call("addGroupByField", field_id_status);

                        int field_id_count = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Count");
                        ax_query_datasource.Call("addGroupByField", field_id_count);

                        int field_id_total_payable_amount = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "TotalPayableAmount");
                        ax_query_datasource.Call("addGroupByField", field_id_total_payable_amount);

                        int field_id_remarks = dynax.GetFieldIdWithLock(table_id_lf_einvoicegov, "LF_Remarks");
                        ax_query_datasource.Call("addGroupByField", field_id_remarks);

                        ax_query_run = dynax.CreateAxaptaObject("QueryRun", ax_query);

                        int max_record_count = 0;

                        while ((bool)ax_query_run.Call("next"))
                        {
                            dynrec = (AxaptaRecord)ax_query_run.Call("Get", table_id_lf_einvoicegov);

                            string invoice_id = !string.IsNullOrEmpty((string)dynrec.get_Field("InvoiceId")) ? (string)dynrec.get_Field("InvoiceId") : "";
                            string buyer_tin = !string.IsNullOrEmpty((string)dynrec.get_Field("BuyerTIN")) ? (string)dynrec.get_Field("BuyerTIN") : "";
                            string buyer_reg_no = !string.IsNullOrEmpty((string)dynrec.get_Field("BuyerRegNo")) ? (string)dynrec.get_Field("BuyerRegNo") : "";


                            if (only_missing_tinbrn == ONLY_MISSING_TIN_BRN_YES)
                            {
                                if (!string.IsNullOrEmpty(buyer_tin) && !string.IsNullOrEmpty(buyer_reg_no)) // exclude those with both brn and tin
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (include_missing_tinbrn == MISSING_TIN_BRN_NO)
                                {
                                    if (string.IsNullOrEmpty(buyer_tin) || string.IsNullOrEmpty(buyer_reg_no)) // exclude those without either brn or tin
                                    {
                                        continue;
                                    }
                                }
                            }

                            /*
                            bool exists = einvoice_list.Any(einvoice => einvoice.invoice_id == invoice_id);
                            if (exists)
                                continue;
                            */
                            EInvoice einvoice = new EInvoice();
                            einvoice.invoice_id = invoice_id;
                            einvoice.original_invoice_id = !string.IsNullOrEmpty((string)dynrec.get_Field("OrigInvoiceId")) ? (string)dynrec.get_Field("OrigInvoiceId") : "";
                            einvoice.invoice_date = dynrec.get_Field("InvoiceDate") != null ? (DateTime)dynrec.get_Field("InvoiceDate") : DateTime.MinValue;
                            einvoice.buyer_name = !string.IsNullOrEmpty((string)dynrec.get_Field("BuyerName")) ? (string)dynrec.get_Field("BuyerName") : "NA";
                            einvoice.buyer_tin = buyer_tin;
                            einvoice.buyer_reg_no = buyer_reg_no;

                            einvoice.lf_count = dynrec.get_Field("LF_Count") != null ? Convert.ToInt32(dynrec.get_Field("LF_Count")) : 0;
                            //einvoice.lf_last_submit = dynrec.get_Field("LF_LastSubmit") != null ? (DateTime)dynrec.get_Field("LF_LastSubmit") : DateTime.MinValue;
                            einvoice.lf_status = dynrec.get_Field("LF_Status") != null ? Convert.ToInt32(dynrec.get_Field("LF_Status")) : 0;
                            einvoice.lf_uuid = !string.IsNullOrEmpty((string)dynrec.get_Field("LF_UUID")) ? (string)dynrec.get_Field("LF_UUID") : "";
                            einvoice.lf_long_id = !string.IsNullOrEmpty((string)dynrec.get_Field("LF_LongId")) ? (string)dynrec.get_Field("LF_LongId") : "";
                            einvoice.lf_submission_id = !string.IsNullOrEmpty((string)dynrec.get_Field("LF_SubmissionId")) ? (string)dynrec.get_Field("LF_SubmissionId") : "";
                            einvoice.lf_remarks = !string.IsNullOrEmpty((string)dynrec.get_Field("LF_Remarks")) ? (string)dynrec.get_Field("LF_Remarks") : "";
                            einvoice.invoice_total_payable_amount = dynrec.get_Field("TotalPayableAmount") != null ? Convert.ToDecimal(dynrec.get_Field("TotalPayableAmount")) : 0.0m;


                            einvoice_list.Add(einvoice);

                            max_record_count++;
                        }
                    }

                    if (submission_status != "2" && submission_status != "4")
                    {
                        table = "<table id='einvoice_table' class='datatable display stripe hover table dataTable dtr-inline'>" +
                                "<thead><tr>" +
                                "<th>No</th>" +
                                "<th>Invoice</th>" +
                                "<th>Orig Invoice</th>" +
                                "<th>Invoice Date</th>" +
                                "<th>Total Payable Amount</th>" +
                                "<th>Buyer Name</th>" +
                                "<th>Buyer BRN</th>" +
                                "<th>Buyer TIN</th>" +
                                "<th>Submission Data</th>" +
                                "<th>Remarks</th>" +
                                "</tr></thead>" +
                                "<tbody>";
                    }
                    else
                    {
                        table = "<table id='einvoice_table' class='datatable display stripe hover table dataTable dtr-inline'>" +
                                "<thead><tr>" +
                                "<th>No</th>" +
                                "<td class='no-sort'><input type='checkbox' name='select_all' id='select_all'/></td>" +
                                "<th>Invoice</th>" +
                                "<th>Orig Invoice</th>" +
                                "<th>Invoice Date</th>" +
                                "<th>Total Payable Amount</th>" +
                                "<th>Buyer Name</th>" +
                                "<th>Buyer BRN</th>" +
                                "<th>Buyer TIN</th>" +
                                "<th>Submission Data</th>" +
                                "<th>Remarks</th>" +
                                "</tr></thead>" +
                                "<tbody>";
                    }

                    string previous_invoiceid = "";
                    for (int i = 0; i < einvoice_list.Count; i++)
                    {

                        EInvoice current_invoice = einvoice_list[i];

                        if (current_invoice.invoice_id != previous_invoiceid)
                        {
                            count++;

                            if (submission_status != "2" && submission_status != "4")
                            {
                                table = table + "<tr>" +
                                    "<td>" + count + "</td>" +
                                    $"<td><a href='EInvoicePreview.aspx?inv={current_invoice.invoice_id}' target='_blank'>{current_invoice.invoice_id}</a></td>" +
                                    "<td>" + current_invoice.original_invoice_id + "</td>" +
                                    "<td>" + current_invoice.invoice_date.ToString("dd-MM-yyyy") + "</td>" +
                                    "<td width='15%'>" + current_invoice.invoice_total_payable_amount.ToString("#,##0.00") + "</td>" +
                                    "<td>" + current_invoice.buyer_name + "</td>" +
                                    "<td>" + current_invoice.buyer_reg_no + "</td>" +
                                    "<td>" + current_invoice.buyer_tin + "</td>" +
                                    "<td width='20%'><b>Submission UID</b>: " + current_invoice.lf_submission_id +
                                        "<br/><b>UUID</b>: " + current_invoice.lf_uuid +
                                        "<br/><b>Long Id</b>: " + current_invoice.lf_long_id + "</td>" +
                                    "<td>" + current_invoice.lf_remarks + "</td>" +
                                    "</tr>";
                            }
                            else
                            {
                                table = table + "<tr>" +
                                    "<td>" + count + "</td>" +
                                    $"<td><input type='checkbox' name='invoice_ids' value='{current_invoice.invoice_id}' style='margin-left:8px' /></td>" +
                                    $"<td><a href='EInvoicePreview.aspx?inv={current_invoice.invoice_id}' target='_blank'>{current_invoice.invoice_id}</a></td>" +
                                    "<td>" + current_invoice.original_invoice_id + "</td>" +
                                    "<td>" + current_invoice.invoice_date.ToString("dd-MM-yyyy") + "</td>" +
                                    "<td width='15%'>" + current_invoice.invoice_total_payable_amount.ToString("#,##0.00") + "</td>" +
                                    "<td>" + current_invoice.buyer_name + "</td>" +
                                    "<td>" + current_invoice.buyer_reg_no + "</td>" +
                                    "<td>" + current_invoice.buyer_tin + "</td>" +
                                    "<td width='20%'><b>Submission UID</b>: " + current_invoice.lf_submission_id +
                                        "<br/><b>UUID</b>: " + current_invoice.lf_uuid +
                                        "<br/><b>Long Id</b>: " + current_invoice.lf_long_id + "</td>" +
                                    "<td>" + current_invoice.lf_remarks + "</td>" +
                                    "</tr>";
                            }
                        }
                        previous_invoiceid = current_invoice.invoice_id;
                    }

                    if (submission_status != "2" && submission_status != "4")
                    {
                        table = table + "</tbody>" +
                        "<tfoot><tr>" +
                            "<th></th>" +
                            "<th colspan='2'>Total Payable Amount</th>" +
                            "<td></td>" +
                            "<td></td>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "</tr></tfoot>" +
                        "</table>";
                    }
                    else
                    {
                        table = table + "</tbody>" +
                        "<tfoot><tr>" +
                            "<th></th>" +
                            "<th colspan='3'>Total Payable Amount</th>" +
                            "<td></td>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "<th></th>" +
                            "</tr></tfoot>" +
                        "</table>";
                    }

                    no_of_records = "" + count;

                    //Function_Method_Sync.AddSyncLog(get_log_file_name(), $"{no_of_records} rows selected for ComRegNo: {company}");
                    logger.Info($"{no_of_records} rows selected for ComRegNo: {company}");

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


                GetTableResponse response = new GetTableResponse
                {
                    status = status,
                    status_msg = status_msg,
                    no_of_records = no_of_records, // Replace with the actual number of records
                    table_string = table,
                    einvoice_list = einvoice_list
                };

                // Serialize the response object to JSON
                string json_response = JsonConvert.SerializeObject(response);

                return json_response;
            }

        }

        [WebMethod]
        public static string requeue_invoices(string company_short_name, List<string> invoice_ids)
        {
            Console.Write($"{company_short_name} {invoice_ids.Count} invoices");

            try
            {
                using (Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name,
                        GLOBAL_Sync.ax_proxy_password, company_short_name, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server))
                {
                    dynax.TTSBegin();

                    using (AxaptaRecord dynrec = dynax.CreateAxaptaRecord("LF_EInvoiceGov"))
                    {
                        foreach (var invoice_id in invoice_ids)
                        {
                            logger.Info($"Re-queue for submission: {invoice_id}");

                            dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.InvoiceId == '{0}'", invoice_id));

                            while (dynrec.Found) // loop through each invoice line
                            {
                                int lf_status = dynrec.get_Field("LF_Status") != null ? Convert.ToInt32(dynrec.get_Field("LF_Status")) : 0;

                                if (!(lf_status == 2 || lf_status == 4))
                                {
                                    dynrec.Next();
                                    continue;
                                }

                                dynrec.set_Field("LF_UUID", "");
                                dynrec.set_Field("LF_Status", 0);
                                dynrec.set_Field("LF_SubmissionId", "");
                                dynrec.set_Field("LF_Count", 0);
                                dynrec.set_Field("LF_Remarks", "Re-queue");

                                dynrec.Call("Update");
                                dynrec.Next();
                            }
                        }
                    }

                    dynax.TTSCommit();

                    RequeueResponse response = new RequeueResponse
                    {
                        status = "success",
                        status_msg = "success"
                    };

                    // Serialize the response object to JSON
                    string json_response = JsonConvert.SerializeObject(response);

                    return json_response;
                }

            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
            {
                // Log the detailed error message
                logger.Error($"Exception occurred: {ex.Message}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                throw;
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
            {
                // Log the detailed error message
                logger.Error($"Exception occurred: {ex.Message}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                throw;
            }
            catch (Exception ex)
            {
                // Log the detailed error message
                logger.Error($"Exception occurred: {ex.Message}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                throw;
            }
            finally
            {
                
            }
        }
    }
}