using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using DotNet.pod.DTOs;
using DotNet.pod.Utils;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.Dynamics;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using NLog;
using NPOI.SS.Formula.Functions;
using ZXing;
using ZXing.QrCode.Internal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static DotNetSync.SyncGatepassAxToDotNetV2;
using static DotNetSync.SyncGatepassDotNetToAxV2;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace DotNet.pod
{
    public partial class WebPod : System.Web.UI.Page
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        string today = DateTime.Today.ToString("dd/MM/yyyy");
        string time = DateTime.Now.ToString("hh:mm:ss tt");
        private static readonly object lock_object = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
            Check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                //LoadSelectionMenu_Setting();
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                   null, null,
                   null, SFATag2,
                   null, SalesQuotation2,
                   null, PaymentTag2,
                   null, RedemptionTag2,
                   null, InventoryMasterTag2,

                   null, EORTag2,
                   null, null,
                   null, WClaimTag2,
                   null, EventBudgetTag2,
                   null, null,
                   null, null, null, RocTinTag2,
                   NewProduct2
                   );
                /*
                if (GLOBAL.debug == true)
                {
                    AddUserTag.Visible = true;
                }   */
            }

            f_call_ax(0);
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=../LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        /*public class PickingObject
        {
            public string pickingID { get; set; }
            public string confirmDate { get; set; }
            public string confirmTime { get; set; }
            public string confirmBy { get; set; }
        }

        public class GetTableResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
            public string no_of_records { get; set; }
            public string table_string { get; set; }
            public List<PickingObject> picking_list { get; set; }
        }*/

        private void Check_session()
        {
            try
            {
                //load session user
                GLOBAL.user_id = Session["user_id"].ToString();
                GLOBAL.axPWD = Session["axPWD"].ToString();
                GLOBAL.logined_user_name = Session["logined_user_name"].ToString();
                GLOBAL.user_authority_lvl = Convert.ToInt32(Session["user_authority_lvl"]);
                GLOBAL.page_access_authority = Convert.ToInt32(Session["page_access_authority"]);
                GLOBAL.user_company = Session["user_company"].ToString();
                GLOBAL.module_access_authority = Convert.ToInt32(Session["module_access_authority"]);
                GLOBAL.switch_Company = Session["switch_Company"].ToString();
                GLOBAL.system_checking = Convert.ToInt32(Session["system_checking"]);
                GLOBAL.data_passing = Session["data_passing"].ToString();
                //
            }
            catch
            {
                Response.Redirect("../LoginPage.aspx");
            }
        }

        protected async Task<ResultDto<List<string>>> GetInvoiceFromGatepassAsync(string pickId)
        {
            logger.Info("getInvoiceFromGatepass");
            logger.Info($"Picking Id: {pickId}");


            var invoiceList = new List<string>();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                using (Axapta DynAx = new Axapta())
                {
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    int tableId = DynAx.GetTableId("LF_Gatepass");
                    int pickIdFieldId = DynAx.GetFieldId(tableId, "PickID");

                    using (AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query"))
                    {
                        using (AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId))
                        {
                            // Add conditions
                            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", pickIdFieldId);
                            qbr.Call("value", pickId);

                            using (AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery))
                            {
                                while ((bool)axQueryRun.Call("next"))
                                {
                                    using (AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId))
                                    {
                                        string invoiceId = DynRec.get_Field("InvoiceId").ToString();

                                        invoiceList.Add(invoiceId);
                                    }
                                }
                            }
                        }
                    }

                    if (invoiceList.Count > 0)
                    {
                        return new ResultDto<List<string>>
                        {
                            Success = true,
                            Message = "Success",
                            Result = invoiceList
                        };
                    }
                    else
                    {
                        return new ResultDto<List<string>>
                        {
                            Success = false,
                            Message = $"Invoice not found for Picking Id: {pickId}"
                        };
                    }


                }
            }
            catch (AxaptaException axEx)
            {
                logger.Error(axEx, axEx.Message);
                return new ResultDto<List<string>>
                {
                    Success = false,
                    Message = $"AxaptaException: {axEx.Message}"
                };
            }
            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);
                return new ResultDto<List<string>>
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new ResultDto<List<string>>
                {
                    Success = false,
                    Message = $"General Exception: {ex.Message}"
                };
            }
        }

        protected void getInvoiceFromGatepass_original(string pickId, int status)
        {
            logger.Info("getInvoiceFromGatepass");
            logger.Info($"Picking Id: {pickId}");
            logger.Info($"Status: {status}");

            AxaptaObject axQuery;
            AxaptaObject axQueryRun;
            AxaptaObject axQueryDataSource;
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                string TableName = "LF_Gatepass";

                int tableId = DynAx.GetTableId(TableName);

                axQuery = DynAx.CreateAxaptaObject("Query");
                axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                int pickIdFieldId = DynAx.GetFieldId(tableId, "PickID");
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", pickIdFieldId);//PickID
                qbr.Call("value", pickId);

                axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //axQueryDataSource.Call("addSortField", 30001, 1);//PickId, asc

                while ((bool)axQueryRun.Call("next"))
                {
                    DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                    string invoiceId = DynRec.get_Field("InvoiceId").ToString();
                    string pick = DynRec.get_Field("PickID").ToString();
                    //Function_Method.MsgBox("Picking + Inv No: " + pick + " " + invoiceId, this.Page, this);
                    //getItemsFromCustInvoiceTrans(invoiceId, status);


                    DynRec.Dispose();
                }
                axQueryRun.Dispose();
                axQueryDataSource.Dispose();
            }
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                Function_Method.MsgBox("Database error1: " + sqlEx.Message, this.Page, this);
            }
            /*catch (FormatException formatEx)
            {
                // Handle format conversion exceptions
                Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
            }*/
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                Function_Method.MsgBox("Invalid operation1: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Function_Method.MsgBox("An error occurred1: " + ex.Message, this.Page, this);
            }
            DynAx.Dispose();
        }

        protected async Task<ResultDto<List<ItemDto>>> GetItemsFromCustInvoiceTransAsync(List<string> invoiceList)
        {
            logger.Info("getItemsFromCustInvoiceTrans");
            logger.Info($"Total of {invoiceList.Count} Invoice Ids: {string.Join(", ", invoiceList)}");


            var itemList = new List<ItemDto>();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;

                using (Axapta DynAx = new Axapta())
                {
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                        new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    int tableId = DynAx.GetTableId("CustInvoiceTrans");
                    int invoiceIdFieldId = DynAx.GetFieldId(tableId, "InvoiceId");

                    using (AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query"))
                    {
                        using (AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId))
                        {
                            foreach (var invoice in invoiceList)
                            {
                                var invoiceIdRange = (AxaptaObject)axQueryDataSource.Call("addRange", invoiceIdFieldId);
                                invoiceIdRange.Call("value", invoice);
                            }

                            using (AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery))
                            {
                                while ((bool)axQueryRun.Call("next"))
                                {
                                    using (AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId))
                                    {
                                        string invoiceId = DynRec.get_Field("InvoiceId").ToString();
                                        string itemId = DynRec.get_Field("ItemId").ToString();
                                        string dim = DynRec.get_Field("inventDimId").ToString();
                                        double qty = Convert.ToDouble(DynRec.get_Field("Qty").ToString());
                                        string unit = DynRec.get_Field("SalesUnit").ToString();

                                        var locationResult = await getLocationFromInventItemLocation(itemId, dim);

                                        if (!locationResult.Success)
                                        {
                                            return new ResultDto<List<ItemDto>>()
                                            {
                                                //Success = false,
                                                //Message = locationResult.Message
                                            };
                                        }

                                        var item = new ItemDto();
                                        item.InvoiceId = invoiceId;
                                        item.InventDimId = dim;
                                        item.ItemId = itemId;
                                        item.Qty = qty;
                                        item.Unit = unit;
                                        item.Location = locationResult.Result;

                                        itemList.Add(item);
                                    }
                                }
                            }

                            if (itemList.Count > 0)
                            {
                                return new ResultDto<List<ItemDto>>()
                                {
                                    Success = true,
                                    Message = $"{itemList.Count} item(s) found.",
                                    Result = itemList
                                };
                            }
                            else
                            {
                                return new ResultDto<List<ItemDto>>()
                                {
                                    Success = false,
                                    Message = $"Item not found."
                                };
                            }
                        }
                    }
                }




            }
            catch (AxaptaException axEx)
            {
                logger.Error(axEx, axEx.Message);
                return new ResultDto<List<ItemDto>>()
                {
                    Success = false,
                    Message = $"AxaptaException: {axEx.Message}"
                };
            }

            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);
                return new ResultDto<List<ItemDto>>()
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new ResultDto<List<ItemDto>>()
                {
                    Success = false,
                    Message = $"General Exception: {ex.Message}"
                };
            }
        }

        //protected void getItemsFromCustInvoiceTrans_original(string invoiceId, int status)
        //{
        //    logger.Info("getItemsFromCustInvoiceTrans");
        //    logger.Info($"Invoice Id: {invoiceId}");
        //    logger.Info($"Status: {status}");
        //    AxaptaObject axQuery;
        //    AxaptaObject axQueryRun;
        //    AxaptaObject axQueryDataSource;
        //    Axapta DynAx = new Axapta();
        //    AxaptaRecord DynRec;

        //    try
        //    {
        //        GLOBAL.Company = GLOBAL.switch_Company;
        //        DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
        //            GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //        int tableId = DynAx.GetTableId("CustInvoiceTrans");

        //        axQuery = DynAx.CreateAxaptaObject("Query");
        //        axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

        //        var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//InvoiceId
        //        qbr.Call("value", invoiceId);

        //        axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

        //        //axQueryDataSource.Call("addSortField", 30001, 1);//PickId, asc

        //        while ((bool)axQueryRun.Call("next"))
        //        {
        //            DynRec = (AxaptaRecord)axQueryRun.Call("Get", 64);

        //            string item = DynRec.get_Field("ItemId").ToString();
        //            string dim = DynRec.get_Field("inventDimId").ToString();
        //            //Function_Method.MsgBox("Item: " + item, this.Page, this);
        //            getLocationFromInventItemLocation(item, dim, invoiceId, status);

        //            DynRec.Dispose();
        //        }
        //        axQueryRun.Dispose();
        //        axQueryDataSource.Dispose();
        //    }
        //    catch (MySqlException sqlEx)
        //    {
        //        // Handle MySQL-specific exceptions
        //        Function_Method.MsgBox("Database error2: " + sqlEx.Message, this.Page, this);
        //    }
        //    /*catch (FormatException formatEx)
        //    {
        //        // Handle format conversion exceptions
        //        Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
        //    }*/
        //    catch (InvalidOperationException invalidOpEx)
        //    {
        //        // Handle invalid operations
        //        Function_Method.MsgBox("Invalid operation2: " + invalidOpEx.Message, this.Page, this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle all other exceptions
        //        Function_Method.MsgBox("An error occurred2: " + ex.Message, this.Page, this);
        //    }
        //    DynAx.Dispose();
        //}

        protected async Task<ResultDto<string>> getLocationFromInventItemLocation(string itemId, string dimension)
        {
            logger.Info("getLocationFromInventItemLocation");
            logger.Info($"Item Id: {itemId}");
            logger.Info($"Dimension: {dimension}");

            string location = string.Empty;

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;

                using (Axapta DynAx = new Axapta())
                {
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                        new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    int tableId = DynAx.GetTableId("InventItemLocation");
                    int itemIdFieldId = DynAx.GetFieldId(tableId, "ItemId");
                    int inventDimIdFieldId = DynAx.GetFieldId(tableId, "InventDimId");

                    using (AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query"))
                    {
                        using (AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId))
                        {
                            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", itemIdFieldId);//ItemId
                            qbr.Call("value", itemId);

                            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", inventDimIdFieldId);//inventDimId
                            qbr1.Call("value", dimension);

                            using (AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery))
                            {
                                if ((bool)axQueryRun.Call("next"))
                                {
                                    using (AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId))
                                    {
                                        location = DynRec.get_Field("wMSPickingLocation").ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(location))
                {
                    return new ResultDto<string>()
                    {
                        Success = true,
                        Message = $"Location found for Item {itemId} and warehouse  {dimension}: {location}",
                        Result = location
                    };
                }
                else
                {
                    return new ResultDto<string>()
                    {//Fooz - 02/10/2025 - some Item dont have wMSPickingLocation record, but must make the process flow works
                        Success = true,
                        //Success = false;
                        Message = $"Location not found for Item {itemId} and warehouse  {dimension}",
                        Result = ""
                    };
                }
            }
            catch (AxaptaException axEx)
            {
                logger.Error(axEx, axEx.Message);
                return new ResultDto<string>()
                {
                    Success = false,
                    Message = $"AxaptaException: {axEx.Message}"
                };
            }

            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);
                return new ResultDto<string>()
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new ResultDto<string>()
                {
                    Success = false,
                    Message = $"MySqlException: {ex.Message}"
                };
            }
        }

        //protected void getLocationFromInventItemLocation_original(string itemId, string dimension, string invoiceId, int status)
        //{
        //    logger.Info("getLocationFromInventItemLocation");
        //    logger.Info($"Invoice Id: {invoiceId}");
        //    logger.Info($"Item Id: {itemId}"); 
        //    logger.Info($"Dimension: {dimension}");
        //    logger.Info($"Status: {status}");

        //    AxaptaObject axQuery;
        //    AxaptaObject axQueryRun;
        //    AxaptaObject axQueryDataSource;
        //    Axapta DynAx = new Axapta();
        //    AxaptaRecord DynRec;

        //    try
        //    {
        //        GLOBAL.Company = GLOBAL.switch_Company;
        //        DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
        //            GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //        int tableId = DynAx.GetTableId("InventItemLocation");

        //        axQuery = DynAx.CreateAxaptaObject("Query");
        //        axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

        //        var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//ItemId
        //        qbr.Call("value", itemId);

        //        var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 16);//inventDimId
        //        qbr1.Call("value", dimension);

        //        axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

        //        //axQueryDataSource.Call("addSortField", 30001, 1);//PickId, asc

        //        while ((bool)axQueryRun.Call("next"))
        //        {
        //            DynRec = (AxaptaRecord)axQueryRun.Call("Get", 659);

        //            string location = DynRec.get_Field("wMSPickingLocation").ToString();
        //            //Function_Method.MsgBox("Loc: " + location, this.Page, this);
        //            getAllDataForUpdate(itemId, location, invoiceId, status); //updateLF_WMSInventLocationQty(itemId, location, invoiceId);

        //            DynRec.Dispose();
        //        }
        //        axQueryRun.Dispose();
        //        axQueryDataSource.Dispose();
        //    }
        //    catch (MySqlException sqlEx)
        //    {
        //        // Handle MySQL-specific exceptions
        //        Function_Method.MsgBox("Database error3: " + sqlEx.Message, this.Page, this);
        //    }
        //    /*catch (FormatException formatEx)
        //    {
        //        // Handle format conversion exceptions
        //        Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
        //    }*/
        //    catch (InvalidOperationException invalidOpEx)
        //    {
        //        // Handle invalid operations
        //        Function_Method.MsgBox("Invalid operation3: " + invalidOpEx.Message, this.Page, this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle all other exceptions
        //        Function_Method.MsgBox("An error occurred3: " + ex.Message, this.Page, this);
        //    }
        //    DynAx.Dispose();
        //}

        //protected void getAllDataForUpdate(string itemid, string location, string invoiceId, int status)
        //{
        //    logger.Info("getAllDataForUpdate");
        //    logger.Info($"Invoice Id: {invoiceId}");
        //    logger.Info($"Item Id: {itemid}");
        //    logger.Info($"Location: {location}");
        //    logger.Info($"Status: {status}");
        //    try
        //    {
        //        GLOBAL.Company = GLOBAL.switch_Company;

        //        using (Axapta DynAx = new Axapta())
        //        {
        //            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
        //                new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
        //                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //            // Get table id
        //            int tid_custInvoiceTrans = DynAx.GetTableId("CustInvoiceTrans"); 

        //            // Create a query
        //            using (AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query"))
        //            {
        //                // Add data source
        //                using (AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tid_custInvoiceTrans))
        //                {
        //                    // Add filters
        //                    var rangeInvoiceId = (AxaptaObject)axQueryDataSource.Call("addRange", DynAx.GetFieldId(tid_custInvoiceTrans, "InvoiceId"));
        //                    rangeInvoiceId.Call("value", invoiceId);

        //                    var rangeItemId = (AxaptaObject)axQueryDataSource.Call("addRange", DynAx.GetFieldId(tid_custInvoiceTrans, "ItemId"));
        //                    rangeItemId.Call("value", itemid);

        //                    // Sort by ItemId ascending
        //                    axQueryDataSource.Call("addSortField", DynAx.GetFieldId(tid_custInvoiceTrans, "ItemId"), 1);

        //                    // Run the query
        //                    using (AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery))
        //                    {
        //                        while ((bool)axQueryRun.Call("next"))
        //                        {
        //                            // Get the record from the correct data source
        //                            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("get", tid_custInvoiceTrans);

        //                            if (DynRec != null && DynRec.Found)
        //                            {
        //                                double qty = Convert.ToDouble(DynRec.get_Field("Qty").ToString());
        //                                string unit = DynRec.get_Field("SalesUnit").ToString();

        //                                updateLF_WMSInventLocationQty(location, itemid, qty, status);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException sqlEx)
        //    {
        //        // Handle MySQL-specific exceptions
        //        //DynAx.TTSAbort();
        //        Function_Method.MsgBox("Database error4: " + sqlEx.Message, this.Page, this);
        //    }
        //    /*catch (FormatException formatEx)
        //    {
        //        // Handle format conversion exceptions
        //        Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
        //    }*/
        //    catch (InvalidOperationException invalidOpEx)
        //    {
        //        // Handle invalid operations
        //        //DynAx.TTSAbort();
        //        Function_Method.MsgBox("Invalid operation4: " + invalidOpEx.Message, this.Page, this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle all other exceptions
        //        //DynAx.TTSAbort();
        //        Function_Method.MsgBox("An error occurred4: " + ex.Message, this.Page, this);
        //    }
        //    finally
        //    {

        //    }

        //}

        //protected void getAllDataForUpdateOri(string id, string location, string invoiceId, int status)
        //{
        //    AxaptaObject axQuery, axQuery1;
        //    AxaptaObject axQueryRun, axQueryRun1;
        //    AxaptaObject axQueryDataSource, axQueryDataSource1;
        //    Axapta DynAx = new Axapta();
        //    Axapta DynAx1 = new Axapta();
        //    AxaptaRecord DynRec = null, DynRec1 = null;

        //    try
        //    {
        //        GLOBAL.Company = GLOBAL.switch_Company;
        //        DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
        //            GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //        DynAx1.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
        //            GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //        int tableId = DynAx.GetTableId("CustInvoiceTrans");
        //        int tableId1 = DynAx1.GetTableId("LF_WMSInventLocationQty");

        //        axQuery = DynAx.CreateAxaptaObject("Query");
        //        axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);
        //        axQuery1 = DynAx1.CreateAxaptaObject("Query");
        //        axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId1);

        //        var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//InvoiceId
        //        qbr.Call("value", invoiceId);

        //        qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 6);//ItemId
        //        qbr.Call("value", id);

        //        var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30004);//ItemId //50013
        //        qbr1.Call("value", id);

        //        qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30002);//WMSLocationId //50002
        //        qbr1.Call("value", location);

        //        axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
        //        axQueryRun1 = DynAx1.CreateAxaptaObject("QueryRun", axQuery1);

        //        axQueryDataSource.Call("addSortField", 6, 1);//ItemId, asc
        //        axQueryDataSource1.Call("addSortField", 30004, 1);//ItemId, asc //50013

        //        while ((bool)axQueryRun.Call("next"))
        //        {
        //            DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

        //            while ((bool)axQueryRun1.Call("next"))
        //            {
        //                DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId1);  //50810

        //                string item = DynRec.get_Field("ItemId").ToString();
        //                double qty = Convert.ToDouble(DynRec.get_Field("Qty").ToString());
        //                string unit = DynRec.get_Field("SalesUnit").ToString();

        //                //foozm 20032025
        //                String query = string.Format("select * from %1 where %1.ItemId == '{0}'", item);
        //                DynRec1.ExecuteStmt(query);

        //                string item1 = DynRec1.get_Field("ItemId").ToString();

        //                if (DynRec1.Found)
        //                {
        //                    updateLF_WMSInventLocationQty(location, item1, qty, status);
        //                }
        //                //foozm 20032025
        //                /*if (item == item1)
        //                {
        //                    //Function_Method.MsgBox("True " + item + " " + item1, this.Page, this);
        //                    //object invoiceTrans = DynRec.get
        //                    //Function_Method.MsgBox("Loc: " + location, this.Page, this);
        //                    updateLF_WMSInventLocationQty(location, item1, qty, status);
        //                }*/

        //                //DynRec.Dispose();
        //                //DynRec1.Dispose();
        //            }
        //        }

        //        if (DynRec != null)
        //        {
        //            DynRec.Dispose();
        //            //DynRec = null; // Set to null to prevent further access
        //        }

        //        if (DynRec1 != null)
        //        {
        //            DynRec1.Dispose();
        //            //DynRec1 = null; // Set to null to prevent further access
        //        }

        //        axQueryRun1.Dispose();
        //        axQueryDataSource1.Dispose();

        //        axQueryRun.Dispose();
        //        axQueryDataSource.Dispose();
        //    }
        //    catch (MySqlException sqlEx)
        //    {
        //        // Handle MySQL-specific exceptions
        //        DynAx.TTSAbort();
        //        Function_Method.MsgBox("Database error4: " + sqlEx.Message, this.Page, this);
        //    }
        //    /*catch (FormatException formatEx)
        //    {
        //        // Handle format conversion exceptions
        //        Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
        //    }*/
        //    catch (InvalidOperationException invalidOpEx)
        //    {
        //        // Handle invalid operations
        //        DynAx.TTSAbort();
        //        Function_Method.MsgBox("Invalid operation4: " + invalidOpEx.Message, this.Page, this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle all other exceptions
        //        DynAx.TTSAbort();
        //        Function_Method.MsgBox("An error occurred4: " + ex.Message, this.Page, this);
        //    }
        //    finally
        //    {
        //        // Dispose of the records after completing all logic
        //        if (DynRec != null)
        //            DynRec.Dispose();

        //        if (DynRec1 != null)
        //            DynRec1.Dispose();
        //    }

        //}

        protected async Task<ResultDto<object>> UpdateLF_WMSInventLocationQtyAsync(List<ItemDto> itemList, bool confirmed)
        {
            logger.Info("UpdateLF_WMSInventLocationQtyAsync");

            double qty;
            int uomSubstringLength = 0;
            Axapta DynAx = new Axapta();
            try
            {
                using (DynAx = new Axapta())
                {
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                        new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    DynAx.TTSBegin();

                    foreach (var item in itemList)
                    {
                        logger.Info($"Updating Invoice Id: {item.InvoiceId}, Item Id: {item.ItemId}, InventDimId: {item.InventDimId}," +
                            $"Location: {item.Location}, Unit: {item.Unit}, Qty: {item.Qty}, Confirm Status: {confirmed}");
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WMSInventLocationQty"))
                        {
                            String query = string.Format(
                                "SELECT FORUPDATE * FROM %1 WHERE %1.WMSLocationId == '{0}' && %1.ItemId == '{1}' && %1.Qty != 0",
                                item.Location, item.ItemId);

                            DynRec.ExecuteStmt(query);

                            if (DynRec.Found)
                            {
                                double balance = Convert.ToDouble(DynRec.get_Field("CtnQty").ToString()) - item.Qty;
                                double unconfirmBalance = Convert.ToDouble(DynRec.get_Field("CtnQty").ToString()) + item.Qty;
                                string uomString = DynRec.get_Field("UOM").ToString().Trim();
                                double uom = 1;

                                if (!string.IsNullOrEmpty(uomString) && uomString.Length > 2)
                                {
                                    // Safely extract substring starting from index 2
                                    // Only take the remaining length after index 2
                                    uomSubstringLength = uomString.Length - 2;
                                    // Get the substring from index 2 onwards
                                    uomString = uomString.Substring(2, uomSubstringLength);

                                    if (!double.TryParse(uomString, out uom))
                                    {
                                        // Handle invalid UOM format. Default to 1 if conversion fails
                                        uom = 1;
                                    }
                                    else
                                    {
                                        uom = Convert.ToDouble(uomString);
                                    }
                                }
                                else
                                {
                                    uom = 1;
                                }

                                logger.Info($"Update UOM: {uom}");

                                if (confirmed)
                                {
                                    qty = (uom * balance + Convert.ToDouble(DynRec.get_Field("BottleQty").ToString()));
                                }
                                else
                                {
                                    qty = (uom * unconfirmBalance + Convert.ToDouble(DynRec.get_Field("BottleQty").ToString()));
                                }

                                logger.Info($"Update Qty: {qty}");

                                if (confirmed)
                                {
                                    if (balance == 0)
                                    {
                                        logger.Info($"Confirmed Status: {confirmed}, balance: {balance}");

                                        DynRec.set_Field("ItemId", "");
                                        DynRec.set_Field("ItemName", "");
                                        DynRec.set_Field("Qty", 0);
                                        DynRec.set_Field("UOM", "");
                                        DynRec.set_Field("CtnQty", 0.00);
                                        DynRec.set_Field("BottleQty", 0.00);
                                    }
                                    else if (balance > 0)
                                    {
                                        logger.Info($"Confirmed Status: {confirmed}, balance: {balance}");

                                        DynRec.set_Field("Qty", Convert.ToInt16(qty));
                                        DynRec.set_Field("CtnQty", balance);

                                    }
                                }
                                else
                                {
                                    if (unconfirmBalance == 0)
                                    {
                                        logger.Info($"Confirmed Status: {confirmed}, unconfirmBalance: {unconfirmBalance}");
                                        DynRec.set_Field("ItemId", "");
                                        DynRec.set_Field("ItemName", "");
                                        DynRec.set_Field("Qty", 0);
                                        DynRec.set_Field("UOM", "");
                                        DynRec.set_Field("CtnQty", 0.00);
                                        DynRec.set_Field("BottleQty", 0.00);
                                    }
                                    else if (unconfirmBalance > 0)
                                    {
                                        logger.Info($"Confirmed Status: {confirmed}, unconfirmBalance: {unconfirmBalance}");

                                        DynRec.set_Field("Qty", Convert.ToInt16(qty));
                                        DynRec.set_Field("CtnQty", unconfirmBalance);

                                    }
                                }
                                DynRec.Call("update");
                                logger.Info("Updated");
                            }
                        }
                    }

                    DynAx.TTSCommit();
                }

                return new ResultDto<object>()
                {
                    Success = true,
                    Message = $"{itemList.Count} item(s) updated.",
                    Result = null
                };
            }
            catch (AxaptaException axEx)
            {
                logger.Error(axEx, axEx.Message);
                //DynAx.TTSAbort();
                AxaptaTTSUtility.TryAbort(DynAx);

                return new ResultDto<object>()
                {
                    Success = false,
                    Message = $"AxaptaException: {axEx.Message}"
                };
            }

            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);
                //DynAx.TTSAbort();
                AxaptaTTSUtility.TryAbort(DynAx);

                return new ResultDto<object>()
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                //DynAx.TTSAbort();
                AxaptaTTSUtility.TryAbort(DynAx);

                return new ResultDto<object>()
                {
                    Success = false,
                    Message = $"General Exception: {ex.Message}"
                };
            }

        }

        protected void updateLF_WMSInventLocationQty(string location, string id, double quantity, int status)
        {
            logger.Info("updateLF_WMSInventLocationQty");
            logger.Info($"Location: {location}");
            logger.Info($"Item Id: {id}");
            logger.Info($"Quantity: {quantity}");
            logger.Info($"Status: {status}");
            Axapta DynAx = Function_Method.GlobalAxapta();
            AxaptaRecord DynRec = null;
            //string uomString= "";
            double qty;
            int uomSubstringLength = 0;
            try
            {
                DynRec = DynAx.CreateAxaptaRecord("LF_WMSInventLocationQty");
                DynAx.TTSBegin();
                //if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(id))
                //{
                //throw new ArgumentException("Location and ItemId cannot be null or empty.");
                //}
                String query = string.Format("select forupdate * from %1 where %1.WMSLocationId == '{0}' && %1.ItemId == '{1}' && %1.Qty != 0", location, id);
                //Function_Method.MsgBox("Query " + query, this.Page, this);
                DynRec.ExecuteStmt(query);
                if (DynRec.Found)
                {
                    //Function_Method.MsgBox("ITEM: " + "..." + location + "..." + id + "..." + quantity, this.Page, this);
                    double balance = Convert.ToDouble(DynRec.get_Field("CtnQty").ToString()) - quantity;
                    double unconfirmBalance = Convert.ToDouble(DynRec.get_Field("CtnQty").ToString()) + quantity;
                    string uomString = DynRec.get_Field("UOM").ToString().Trim();
                    double uom = 1;
                    //Function_Method.MsgBox("111 " + balance + " ... " + uomString , this.Page, this);

                    if (!string.IsNullOrEmpty(uomString) && uomString.Length > 2)
                    {
                        // Safely extract substring starting from index 2
                        uomSubstringLength = uomString.Length - 2; // Only take the remaining length after index 2
                        uomString = uomString.Substring(2, uomSubstringLength); // Get the substring from index 2 onwards
                                                                                //uom = Convert.ToDouble(uomString);

                        if (!double.TryParse(uomString, out uom))
                        {
                            // Handle invalid UOM format
                            //Function_Method.MsgBox("Invalid UOM format: " + uomString, this.Page, this);
                            uom = 1; // Default to 1 if conversion fails
                        }
                        else
                        {
                            uom = Convert.ToDouble(uomString);
                        }
                    }
                    else
                    {
                        uom = 1;
                    }

                    /* (length > 2)
                    {
                        qty = (uom * Convert.ToDouble(DynRec.get_Field("CtnQty"))) + Convert.ToDouble(DynRec.get_Field("BottleQty"));
                    } else 
                    { 
                        qty = (1 * Convert.ToDouble(DynRec.get_Field("CtnQty"))) + Convert.ToDouble(DynRec.get_Field("BottleQty"));
                    }//*/
                    if (status == 1)
                    {
                        qty = (uom * balance + Convert.ToDouble(DynRec.get_Field("BottleQty").ToString()));
                    }
                    else
                    {
                        qty = (uom * unconfirmBalance + Convert.ToDouble(DynRec.get_Field("BottleQty").ToString()));
                    }

                    //Function_Method.MsgBox("222 " + uom + " ... " + uomString + " ... " + uomSubstringLength + " ... " + qty + " ... " + balance, this.Page, this);
                    if (status == 1)
                    {
                        if (balance == 0)
                        {
                            DynRec.set_Field("ItemId", "");
                            DynRec.set_Field("ItemName", "");
                            DynRec.set_Field("Qty", 0);
                            DynRec.set_Field("UOM", "");
                            DynRec.set_Field("CtnQty", 0.00);
                            DynRec.set_Field("BottleQty", 0.00);
                        }
                        else if (balance > 0)
                        {
                            //Function_Method.MsgBox("Query " + id + ' ' + qty + ' ' + balance, this.Page, this);
                            DynRec.set_Field("Qty", Convert.ToInt16(qty));
                            DynRec.set_Field("CtnQty", balance);

                        }
                    }
                    else
                    {
                        if (unconfirmBalance == 0)
                        {
                            DynRec.set_Field("ItemId", "");
                            DynRec.set_Field("ItemName", "");
                            DynRec.set_Field("Qty", 0);
                            DynRec.set_Field("UOM", "");
                            DynRec.set_Field("CtnQty", 0.00);
                            DynRec.set_Field("BottleQty", 0.00);
                        }
                        else if (unconfirmBalance > 0)
                        {
                            //Function_Method.MsgBox("Query " + id + ' ' + qty + ' ' + balance, this.Page, this);
                            DynRec.set_Field("Qty", Convert.ToInt16(qty));
                            DynRec.set_Field("CtnQty", unconfirmBalance);

                        }
                    }
                    //DynRec.set_Field("confirmTime", time);
                    DynRec.Call("update");
                    update_PickingIDStatus(true, status);
                    //DynRec.Next();


                }
                update_PickingIDStatus(true, status);
                DynRec.Dispose();
                DynAx.TTSCommit();
            }
            /*catch (Exception ER_Invoice_01)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_POD_01: " + ER_Invoice_01.ToString(), this.Page, this);
            }*/
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                DynAx.TTSAbort();
                Function_Method.MsgBox("Database error5: " + sqlEx.Message, this.Page, this);
            }
            /*catch (FormatException formatEx)
            {
                // Handle format conversion exceptions
                Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
            }*/
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                DynAx.TTSAbort();
                Function_Method.MsgBox("Invalid operation5: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                DynAx.TTSAbort();
                Function_Method.MsgBox("An error occurred5: " + ex.Message, this.Page, this);
            }
            finally
            {
                //MessageBox.Show("Done");
                DynAx.Logoff();
            }
        }

        protected void saveToAx()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebPod"))
                {
                    DynAx.TTSBegin();

                    DynRec.set_Field("Name", GLOBAL.user_id);
                    DynRec.set_Field("Item", getScannedData.Value);

                    DynRec.Call("insert");

                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
            }
            catch (Exception ER_Invoice_01)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_POD_01: " + ER_Invoice_01.ToString(), this.Page, this);
            }
            finally
            {
                MessageBox.Show("Barcode: " + getScannedData.Value + " save.");
                DynAx.Logoff();
            }
        }


        protected async Task<ResultDto<object>> UpdatePickingIdStatusAxaptaAsync(string pickingId, DateTime updateDateTime, bool confirmed)
        {
            logger.Info("UpdatePickingIdStatusAxaptaAsync");
            logger.Debug($"Picking Id: {pickingId}");
            logger.Debug($"Update Date/Time: {updateDateTime}");
            logger.Debug($"Original Confirm Status: {confirmed}");

            Axapta DynAx = new Axapta();

            try
            {
                using (DynAx = new Axapta())
                {
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                        new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    DynAx.TTSBegin();

                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Picking"))
                    {
                        String query = string.Format("select forupdate * from %1 where %1.PickId == '{0}'", pickingId);
                        DynRec.ExecuteStmt(query);

                        if (DynRec.Found)
                        {
                            if (confirmed)
                            {
                                DynRec.set_Field("unconfirmBy", GLOBAL.user_id);
                                DynRec.set_Field("unconfirmDateTime", updateDateTime.ToString("dd-MM-yyyy HH:mm:ss"));
                                DynRec.set_Field("confirmStatus", 0);

                                DynRec.Call("update");
                            }
                            else
                            {
                                DynRec.set_Field("confirmBy", GLOBAL.user_id);
                                DynRec.set_Field("confirmDateTime", updateDateTime.ToString("dd-MM-yyyy HH:mm:ss"));
                                DynRec.set_Field("confirmStatus", 1);

                                DynRec.Call("update");
                            }
                        }
                    }

                    DynAx.TTSCommit();
                }

                return new ResultDto<object>
                {
                    Success = true,
                    Message = "Updated Picking Id on Axapta"
                };
            }
            catch (AxaptaException axEx)
            {
                logger.Error(axEx, axEx.Message);
                //DynAx.TTSAbort();
                AxaptaTTSUtility.TryAbort(DynAx);

                return new ResultDto<object>()
                {
                    Success = false,
                    Message = $"AxaptaException: {axEx.Message}"
                };
            }

            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);
                //DynAx.TTSAbort();
                AxaptaTTSUtility.TryAbort(DynAx);

                return new ResultDto<object>()
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                //DynAx.TTSAbort();
                AxaptaTTSUtility.TryAbort(DynAx);

                return new ResultDto<object>()
                {
                    Success = false,
                    Message = $"General Exception: {ex.Message}"
                };
            }
        }

        protected void updateAx(string id, string date, string time, int status)
        {
            logger.Info("updateAx");
            logger.Info($"Picking Id: {id}");
            logger.Info($"Date: {date}");
            logger.Info($"Time: {time}");
            logger.Info($"Status: {status}");
            Axapta DynAx = Function_Method.GlobalAxapta();
            AxaptaRecord DynRec = null;
            try
            {
                DynRec = DynAx.CreateAxaptaRecord("LF_Picking");
                DynAx.TTSBegin();
                if (status == 1)
                {
                    String query = string.Format("select forupdate * from %1 where %1.PickId == '{0}'", id);
                    DynRec.ExecuteStmt(query);
                    if (DynRec.Found)
                    {
                        DynRec.set_Field("confirmBy", GLOBAL.user_id);
                        DynRec.set_Field("confirmDateTime", date + " " + time);
                        ///DynRec.set_Field("confirmTime", time);
                        DynRec.set_Field("confirmStatus", 1);
                        ///DynRec.set_Field("unconfirmStatus", 0);

                        DynRec.Call("update");

                        //getInvoiceFromGatepass(id);
                    }
                }
                else
                {
                    String query = string.Format("select forupdate * from %1 where %1.PickId == '{0}'", id);
                    DynRec.ExecuteStmt(query);
                    if (DynRec.Found)
                    {
                        DynRec.set_Field("unconfirmBy", GLOBAL.user_id);
                        DynRec.set_Field("unconfirmDateTime", date + " " + time);
                        ///DynRec.set_Field("unconfirmTime", time);
                        ///DynRec.set_Field("unconfirmStatus", 1);
                        DynRec.set_Field("confirmStatus", 0);

                        DynRec.Call("update");
                    }
                }
                DynRec.Dispose();
                DynAx.TTSCommit();

                logger.Info("End");
            }
            /*catch (Exception ER_Invoice_01)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_POD_01: " + ER_Invoice_01.ToString(), this.Page, this);
            }*/
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                DynAx.TTSAbort();
                Function_Method.MsgBox("Database error: " + sqlEx.Message, this.Page, this);
            }
            /*catch (FormatException formatEx)
            {
                // Handle format conversion exceptions
                Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
            }*/
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                DynAx.TTSAbort();
                Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                DynAx.TTSAbort();
                Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
            }
            finally
            {
                if (DynRec != null)
                {
                    DynRec.Dispose();
                }

                if (status == 1)
                {
                    Function_Method.MsgBox("Picking ID: " + getScannedData.Value + " is successfully confirmed.", this.Page, this);
                }
                else
                {
                    Function_Method.MsgBox("Picking ID: " + getScannedData.Value + " is unconfirmed.", this.Page, this);
                }
                //MessageBox.Show("Barcode: " + getScannedData.Value + " save.");
                DynAx.Logoff();
            }
        }

        protected void textBoxBarcode_TextChanged(object sender, EventArgs e)
        {
            // Capture the scanned barcode data
            string barcode = textBoxBarcode.Text;

            // Process the barcode (e.g., search database, trigger some action, etc.)
            //MessageBox.Show("Scanned Barcode: " + barcode);
            getScannedData.Value = barcode;
            // Clear the TextBox for the next scan
            //textBoxBarcode.Text = "";

            // Optionally, set focus back to the TextBox
            //textBoxBarcode.Focus();

            //update_PickingID();
            //updateAx();
        }

        private async Task<ResultDto<bool>> GetConfirmStatusAsync(string id)
        {
            logger.Info("update_PickingIDWithChecking");
            logger.Info($"Picking Id: {id}");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM lf_pod WHERE pickingID = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool confirmStatus = reader.GetBoolean("confirmStatus");
                                logger.Info($"Confirm Status: {confirmStatus}");


                                return new ResultDto<bool>
                                {
                                    Success = true,
                                    Message = "Picking Id updated",
                                    Result = confirmStatus
                                };
                            }
                            else
                            {
                                return new ResultDto<bool>
                                {
                                    Success = false,
                                    Message = "Picking Id not found"
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                logger.Error(sqlEx, sqlEx.Message);

                return new ResultDto<bool>
                {
                    Success = false,
                    Message = $"MySqlException: {sqlEx.Message}"
                };
            }
            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);

                return new ResultDto<bool>
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);

                return new ResultDto<bool>
                {
                    Success = false,
                    Message = $"General Exception: {ex.Message}"
                };
            }
        }

        //private void update_PickingIDWithChecking_original(string id)
        //{
        //    logger.Info("update_PickingIDWithChecking");
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
        //        {
        //            conn.Open();
        //            string selectQuery = "SELECT * FROM lf_pod WHERE pickingID = @id";
        //            //string updateQuery = "update lf_pod SET confirmDate=@D2,confirmTime=@D3,confirmBy=@D4 where pickingID=@D1";

        //            using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
        //            {
        //                try
        //                {
        //                    selectCmd.Parameters.AddWithValue("@id", textBoxBarcode.Text);

        //                    using (MySqlDataReader selectReader = selectCmd.ExecuteReader())
        //                    {
        //                        // Check if there is at least one row with a matching user_id
        //                        if (selectReader.HasRows)
        //                        {
        //                            bool needsUpdate = false;
        //                            bool unconfirm = false;
        //                            bool updateAgain = false;
        //                            while (selectReader.Read())
        //                            {
        //                                logger.Info("update_PickingIDWithChecking - while loop");
        //                                string columnValue = selectReader.GetString("confirmStatus");
        //                                ///string columnValue1 = selectReader.GetString("unconfirmStatus");
        //                                if (columnValue.Equals("False")) ///&& columnValue1.Equals("False"))
        //                                {
        //                                    needsUpdate = true; //first time
        //                                }
        //                                else if(columnValue.Equals("True")) ///&& columnValue1.Equals("False"))
        //                                {
        //                                    unconfirm = true; //already confirm, want unconfirm
        //                                }
        //                                ///else if (columnValue.Equals("False")) ///&& columnValue1.Equals("True"))
        //                                ///{
        //                                    ///updateAgain = true; //already unconfirm, want confirm again
        //                                ///}
        //                                /*else
        //                                {
        //                                    if (!string.IsNullOrEmpty(columnValue) && string.IsNullOrEmpty(columnValue1))
        //                                    {
        //                                        unconfirm = true;
        //                                        needsUpdate = false;
        //                                    } else if (!string.IsNullOrEmpty(columnValue) && !string.IsNullOrEmpty(columnValue1))
        //                                    {
        //                                        updateAgain = true;
        //                                    }
        //                                    //updateAgain = true;

        //                                }*/
        //                                /*if (!string.IsNullOrEmpty(columnValue1))
        //                                {
        //                                    //unconfirm = true;
        //                                    updateAgain = true;
        //                                    unconfirm = false;
        //                                }*/
        //                                /*else
        //                                {
        //                                    Function_Method.MsgBox("This Picking ID is already confirmed.", this.Page, this);
        //                                    getPickingID.Text = selectReader.GetString("pickingID"); // "";
        //                                    getConfirmDate.Text = selectReader.GetString("confirmDate"); //"";
        //                                    getConfirmTime.Text = selectReader.GetString("confirmTime"); //"";
        //                                    getConfirmBy.Text = selectReader.GetString("confirmBy"); //"";
        //                                    textBoxBarcode.Text = "";
        //                                    textBoxBarcode.Focus();
        //                                }*/
        //                                //}
        //                                //selectReader.Close();
        //                                if (needsUpdate)
        //                                {
        //                                    getInvoiceFromGatepass(id, 1);
        //                                    /*using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
        //                                    {
        //                                        updateCmd.Parameters.Clear();
        //                                        MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
        //                                        _D1.Value = getScannedData.Value;
        //                                        updateCmd.Parameters.Add(_D1);
        //                                        MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
        //                                        _D2.Value = today;
        //                                        updateCmd.Parameters.Add(_D2);
        //                                        MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
        //                                        _D3.Value = time;
        //                                        updateCmd.Parameters.Add(_D3);
        //                                        MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
        //                                        _D4.Value = GLOBAL.user_id;
        //                                        updateCmd.Parameters.Add(_D4);
        //                                        //conn.Open();
        //                                        updateCmd.ExecuteNonQuery();
        //                                    }
        //                                    getPickingID.Text = textBoxBarcode.Text;
        //                                    getConfirmDate.Text = today;
        //                                    getConfirmTime.Text = time;
        //                                    getConfirmBy.Text = GLOBAL.user_id;
        //                                    updateAx(textBoxBarcode.Text, today, time);
        //                                    textBoxBarcode.Text = "";
        //                                    textBoxBarcode.Focus();
        //                                    //conn.Close();*/
        //                                }
        //                                else if (unconfirm) 
        //                                {
        //                                    Console.WriteLine("unconfirm");
        //                                    /*if (updateAgain && needsUpdate == false)
        //                                    {
        //                                        Function_Method.MsgBox("Update again", this.Page, this);
        //                                    }
        //                                    else 
        //                                    if(unconfirm == true) // && updateAgain == true
        //                                    {*/
        //                                    // Create and show the dialog box
        //                                    //DialogResult dialogResult = MessageBox.Show(
        //                                    //    "This Picking ID has already been confirmed. Would you like to unconfirm the Picking ID?",
        //                                    //    "Confirmation",
        //                                    //    MessageBoxButtons.YesNo,
        //                                    //    MessageBoxIcon.Information
        //                                    //);

        //                                    // If user clicks "Yes", allow the process to continue
        //                                    //if (dialogResult == DialogResult.Yes)
        //                                    if(true)
        //                                    {
        //                                        // If the user chooses to continue, update the record or handle accordingly
        //                                        getInvoiceFromGatepass(id, 0);
        //                                        //UpdatePickingIDDetails(conn); // Call method to update the record
        //                                        //UpdateUIFromDatabase(selectReader); // Update UI
        //                                        //ClearTextBox();
        //                                    }
        //                                    else
        //                                    {
        //                                        // If user clicks "No", just clear the text box
        //                                        getPickingID.Text = selectReader.GetString("pickingID"); // "";
        //                                        getConfirmDateTime.Text = selectReader.GetString("confirmDateTime"); //"";
        //                                        ///getConfirmTime.Text = selectReader.GetString("confirmTime"); //"";
        //                                        getConfirmBy.Text = selectReader.GetString("confirmBy"); //"";
        //                                        textBoxBarcode.Text = "";
        //                                        textBoxBarcode.Focus();
        //                                    }
        //                                } 
        //                                ///else if(updateAgain)
        //                                ///{
        //                                    ///getInvoiceFromGatepass(id, 1);
        //                                ///}
        //                                //}
        //                                //selectReader.Close();
        //                            }
        //                            selectReader.Close();
        //                        }
        //                    }
        //                }
        //                catch (MySqlException sqlEx)
        //                {
        //                    // Handle MySQL-specific exceptions
        //                    Function_Method.MsgBox("Database error1: " + sqlEx.Message, this.Page, this);
        //                }
        //                /*catch (FormatException formatEx)
        //    {
        //        // Handle format conversion exceptions
        //        Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
        //    }*/
        //                catch (InvalidOperationException invalidOpEx)
        //                {
        //                    // Handle invalid operations
        //                    Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Handle all other exceptions
        //                    Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException sqlEx)
        //    {
        //        // Handle MySQL-specific exceptions
        //        Function_Method.MsgBox("Database error2: " + sqlEx.Message, this.Page, this);
        //    }
        //    /*catch (FormatException formatEx)
        //    {
        //        // Handle format conversion exceptions
        //        Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
        //    }*/
        //    catch (InvalidOperationException invalidOpEx)
        //    {
        //        // Handle invalid operations
        //        Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle all other exceptions
        //        Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
        //    }
        //}

        private async Task<ResultDto<object>> UpdatePickingIdStatusAsync(string pickingId, DateTime updateDateTime, bool confirmed)
        {
            logger.Info("UpdatePickingIdStatusAsync");
            logger.Info($"Picking Id: {pickingId}");
            logger.Info($"Current Confirm Status: {confirmed}");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();

                    // Default as confirmed == false
                    string updateQuery = "update lf_pod SET confirmDateTime=@UpdateDateTime,confirmBy=@UserId,confirmStatus=@NewStatus where pickingID=@PickingId";

                    if (confirmed)
                    {
                        updateQuery = "update lf_pod SET unconfirmDateTime=@UpdateDateTime,unconfirmBy=@UserId,confirmStatus=@NewStatus where pickingID=@PickingId";
                    }

                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.Clear();
                        updateCmd.Parameters.AddWithValue("@PickingId", pickingId);
                        updateCmd.Parameters.AddWithValue("@UserId", GLOBAL.user_id);
                        updateCmd.Parameters.AddWithValue("@NewStatus", !confirmed);
                        updateCmd.Parameters.AddWithValue("@UpdateDateTime", updateDateTime);

                        updateCmd.ExecuteNonQuery();
                    }

                    DateTime now = DateTime.Now;
                    string formatted = now.ToString("yyyy-MM-dd HH:mm:ss");


                    if (confirmed)
                    {
                        getPickingID.Text = pickingId.Trim();
                        unconfirmDateDiv.Visible = true;
                        ///unconfirmTimeDiv.Visible = true;
                        unconfirmByDiv.Visible = true;
                        getUnconfirmDateTime.Text = formatted;
                        ///getUnconfirmTime.Text = time;
                        getUnconfirmBy.Text = GLOBAL.user_id;
                        //updateAx(getScannedData.Value, today, time, !confirmed);
                        textBoxBarcode.Text = "";
                        textBoxBarcode.Focus();
                    }
                    else
                    {
                        getPickingID.Text = pickingId.Trim();
                        getConfirmDateTime.Text = formatted;
                        ///getConfirmTime.Text = time;
                        getConfirmBy.Text = GLOBAL.user_id;
                        unconfirmDateDiv.Visible = false;
                        ///unconfirmTimeDiv.Visible = false;
                        unconfirmByDiv.Visible = false;
                        //updateAx(getScannedData.Value, today, time, status);
                        textBoxBarcode.Text = "";
                        textBoxBarcode.Focus();
                    }

                    return new ResultDto<object>
                    {
                        Success = true,
                        Message = "Picking Id updated"
                    };
                }
            }
            catch (MySqlException sqlEx)
            {
                logger.Error(sqlEx, sqlEx.Message);

                return new ResultDto<object>
                {
                    Success = false,
                    Message = $"MySqlException: {sqlEx.Message}"
                };
            }
            catch (InvalidOperationException invalidOpEx)
            {
                logger.Error(invalidOpEx, invalidOpEx.Message);

                return new ResultDto<object>
                {
                    Success = false,
                    Message = $"InvalidOperationException: {invalidOpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);

                return new ResultDto<object>
                {
                    Success = false,
                    Message = $"General Exception: {ex.Message}"
                };
            }
        }

        private void update_PickingIDStatus(bool needsUpdate, int status)
        {
            logger.Info("update_PickingIDStatus");
            logger.Info($"Needs Update: {needsUpdate}");
            logger.Info($"Status: {status}");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();
                    if (status == 1)
                    {
                        ///string updateQuery = "update lf_pod SET confirmDate=@D2,confirmTime=@D3,confirmBy=@D4,confirmStatus=@D5,unconfirmStatus=@D6 where pickingID=@D1";
                        string updateQuery = "update lf_pod SET confirmDateTime=@D2,confirmBy=@D3,confirmStatus=@D4 where pickingID=@D1";
                        if (needsUpdate)
                        {

                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.Clear();
                                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                                _D1.Value = getScannedData.Value;
                                updateCmd.Parameters.Add(_D1);
                                MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                                ///_D2.Value = today; 
                                _D2.Value = today + " " + time;
                                updateCmd.Parameters.Add(_D2);
                                ///MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                                ///_D3.Value = time;
                                ///updateCmd.Parameters.Add(_D3);
                                MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                                _D3.Value = GLOBAL.user_id;
                                updateCmd.Parameters.Add(_D3);
                                MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                                _D4.Value = 1;
                                updateCmd.Parameters.Add(_D4);
                                ///MySqlParameter _D6 = new MySqlParameter("@D6", MySqlDbType.VarChar, 0);
                                ///_D6.Value = 0;
                                ///updateCmd.Parameters.Add(_D6);

                                //conn.Open();
                                updateCmd.ExecuteNonQuery();
                            }
                            getPickingID.Text = getScannedData.Value;
                            getConfirmDateTime.Text = today + " " + time;
                            ///getConfirmTime.Text = time;
                            getConfirmBy.Text = GLOBAL.user_id;
                            unconfirmDateDiv.Visible = false;
                            ///unconfirmTimeDiv.Visible = false;
                            unconfirmByDiv.Visible = false;
                            updateAx(getScannedData.Value, today, time, status);
                            textBoxBarcode.Text = "";
                            textBoxBarcode.Focus();
                            //conn.Close();

                        }
                    }
                    else
                    {
                        ///string updateQuery = "update lf_pod SET unconfirmDate=@D2,unconfirmTime=@D3,unconfirmBy=@D4,unconfirmStatus=@D5,confirmStatus=@D6 where pickingID=@D1";
                        string updateQuery = "update lf_pod SET unconfirmDateTime=@D2,unconfirmBy=@D3,confirmStatus=@D4 where pickingID=@D1";
                        if (needsUpdate)
                        {

                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.Clear();
                                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                                _D1.Value = getScannedData.Value;
                                updateCmd.Parameters.Add(_D1);
                                MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                                ///_D2.Value = today;
                                _D2.Value = today + " " + time;
                                updateCmd.Parameters.Add(_D2);
                                ///MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                                ///_D3.Value = time;
                                ///updateCmd.Parameters.Add(_D3);
                                MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                                _D3.Value = GLOBAL.user_id;
                                updateCmd.Parameters.Add(_D3);
                                ///MySqlParameter _D5 = new MySqlParameter("@D5", MySqlDbType.VarChar, 0);
                                ///_D5.Value = 1;
                                ///updateCmd.Parameters.Add(_D5);
                                MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                                _D4.Value = 0;
                                updateCmd.Parameters.Add(_D4);

                                //conn.Open();
                                updateCmd.ExecuteNonQuery();
                            }
                            getPickingID.Text = getScannedData.Value;
                            unconfirmDateDiv.Visible = true;
                            ///unconfirmTimeDiv.Visible = true;
                            unconfirmByDiv.Visible = true;
                            getUnconfirmDateTime.Text = today + " " + time;
                            ///getUnconfirmTime.Text = time;
                            getUnconfirmBy.Text = GLOBAL.user_id;
                            updateAx(getScannedData.Value, today, time, status);
                            textBoxBarcode.Text = "";
                            textBoxBarcode.Focus();
                            //conn.Close();

                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                Function_Method.MsgBox("Database error1: " + sqlEx.Message, this.Page, this);
            }
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
            }
        }

        private void update_PickingID()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string Query;

                    Query = "update lf_pod SET confirmDate=@D2,confirmTime=@D3,confirmBy=@D4 where pickingID=@D1";  //"insert into lf_pod(pickingID) values(@D1)";

                    MySqlCommand cmd = new MySqlCommand(Query, conn);

                    MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                    _D1.Value = getScannedData.Value;
                    cmd.Parameters.Add(_D1);
                    MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                    _D2.Value = today;
                    cmd.Parameters.Add(_D2);
                    MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                    _D3.Value = time;
                    cmd.Parameters.Add(_D3);
                    MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                    _D4.Value = GLOBAL.user_id;
                    cmd.Parameters.Add(_D4);
                    Function_Method.MsgBox("Query: " + Query, this.Page, this);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    getPickingID.Text = textBoxBarcode.Text;
                    getConfirmDateTime.Text = today;
                    ///getConfirmTime.Text = time;
                    getConfirmBy.Text = GLOBAL.user_id;
                    textBoxBarcode.Text = "";
                    textBoxBarcode.Focus();
                    conn.Close();

                }


            }
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                Function_Method.MsgBox("Database error: " + sqlEx.Message, this.Page, this);
            }
            /*catch (FormatException formatEx)
            {
                // Handle format conversion exceptions
                Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
            }*/
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
            }
        }

        private void store_PickingID1(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    //Function_Method.MsgBox("time: " + time, this.Page, this);
                    string Query, query1;

                    //Function_Method.MsgBox("456", this.Page, this);
                    Query = "insert into lf_pod(pickingID) values(@D1)";

                    using (MySqlCommand cmd = new MySqlCommand(Query, conn))
                    {
                        //Function_Method.MsgBox("789", this.Page, this);

                        MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 10);
                        _D1.Value = id;
                        cmd.Parameters.Add(_D1);
                        //Function_Method.MsgBox("ID: " + _D1.Value, this.Page, this);
                        //MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 50);
                        //_D2.Value = today; //Convert.ToDateTime(today);
                        //Function_Method.MsgBox("Date: " + _D2.Value, this.Page, this);
                        //cmd.Parameters.Add(_D2);
                        //MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.Time);
                        //_D3.Value = Convert.ToDateTime(time);
                        //Function_Method.MsgBox("Time: " + time, this.Page, this);
                        //cmd.Parameters.Add(_D3);
                        //MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 50);
                        //_D4.Value = GLOBAL.user_id;
                        //cmd.Parameters.Add(_D4);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            /*
            catch (Exception ER_AU_02)
            {
                Function_Method.MsgBox("ER_AU_02: " + ER_AU_02.ToString(), this.Page, this);
            }*/
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                Function_Method.MsgBox("Database error: " + sqlEx.Message, this.Page, this);
            }
            /*catch (FormatException formatEx)
            {
                // Handle format conversion exceptions
                Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
            }*/
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
            }
        }

        private void store_PickingID(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();
                    string selectQuery = "SELECT * FROM lf_pod WHERE pickingID = @id";
                    string insertQuery = "insert into lf_pod(pickingID) values(@D1)";

                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        try
                        {
                            selectCmd.Parameters.AddWithValue("@id", id);

                            using (MySqlDataReader selectReader = selectCmd.ExecuteReader())
                            {
                                //Function_Method.MsgBox("123" + reader.GetValue(1).ToString(), this.Page, this);
                                if (!selectReader.HasRows)
                                {
                                    selectReader.Close();
                                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                                    {
                                        MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 10);
                                        _D1.Value = id;
                                        insertCmd.Parameters.Add(_D1);
                                        insertCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        catch (MySqlException sqlEx)
                        {
                            // Handle MySQL-specific exceptions
                            Function_Method.MsgBox("Database error: " + sqlEx.Message, this.Page, this);
                        }
                        /*catch (FormatException formatEx)
                        {
                            // Handle format conversion exceptions
                            Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
                        }*/
                        catch (InvalidOperationException invalidOpEx)
                        {
                            // Handle invalid operations
                            Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
                        }
                        catch (Exception ex)
                        {
                            // Handle all other exceptions
                            Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
                        }
                    }
                    // conn.Close();
                }
            }
            catch (MySqlException sqlEx)
            {
                // Handle MySQL-specific exceptions
                Function_Method.MsgBox("Database error: " + sqlEx.Message, this.Page, this);
            }
            /*catch (FormatException formatEx)
            {
                // Handle format conversion exceptions
                Function_Method.MsgBox("Format error: " + formatEx.Message, this.Page, this);
            }*/
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations
                Function_Method.MsgBox("Invalid operation: " + invalidOpEx.Message, this.Page, this);
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Function_Method.MsgBox("An error occurred: " + ex.Message, this.Page, this);
            }
            //cmd1.ExecuteNonQuery();
            //conn.Close();
        }

        /*
        catch (Exception ER_AU_02)
        {
            Function_Method.MsgBox("ER_AU_02: " + ER_AU_02.ToString(), this.Page, this);
        }*/


        private void f_call_ax(int PAGE_INDEX)
        {
            string fieldName;
            AxaptaObject axQuery;
            AxaptaObject axQueryRun;
            AxaptaObject axQueryDataSource;
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;

            string TableName = "LF_Picking";
            string createdDt = "";
            string date = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");

            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int tableId = DynAx.GetTableId(TableName);
                int pickIdFieldId = DynAx.GetFieldId(tableId, "PickId");
                int unpickFieldId = DynAx.GetFieldId(tableId, "Unpick");

                axQuery = DynAx.CreateAxaptaObject("Query");
                axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                //var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", DynAx.GetFieldId(tableId, fieldName));
                //qbr.Call("value", fieldValue);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", pickIdFieldId);//PickId
                qbr.Call("value", "*");

                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", unpickFieldId);//Unpick
                qbr1.Call("value", "0");

                axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                axQueryDataSource.Call("addSortField", pickIdFieldId, 1);//PickId, asc

                while ((bool)axQueryRun.Call("next"))
                {
                    DynRec = (AxaptaRecord)axQueryRun.Call("Get", 30588);
                    createdDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime")).ToString("dd/MM/yyyy");

                    if (DateTime.ParseExact(createdDt, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        string pickID = DynRec.get_Field("PickID").ToString();
                        store_PickingID(pickID);
                        //Function_Method.MsgBox("pickID: " + pickID, this.Page, this);
                    }

                    DynRec.Dispose();
                }
                axQueryRun.Dispose();
                axQueryDataSource.Dispose();
            }
            catch (Exception ER_CM_00)
            {
                Function_Method.MsgBox("ER_CM_00: " + ER_CM_00.ToString(), this.Page, this);
            }
            DynAx.Dispose();
        }

        protected async void btnConfirm_Click(object sender, EventArgs e)
        {
            logger.Info("Picking Confirmation: Confirm button clicked");

            var pickingId = textBoxBarcode.Text;

            // Check if picking id needs to be confirmed or unconfirmed
            var checkingResult = await GetConfirmStatusAsync(pickingId);

            if (!checkingResult.Success)
            {
                Function_Method.MsgBox(checkingResult.Message, this.Page, this);
                return;
            }

            // If confirmed is true, means already confirmed, then need to unconfirm. 
            // If confirmed is false, means not yet confirm, need to confirm.
            bool confirmed = checkingResult.Result;

            if (confirmed)
            {
                string confirmedValue = Request.Form["__CONFIRMEDACTION"];

                if (confirmedValue != "true")
                {
                    string script = string.Format(@"
                        if (confirm('This picking is already confirmed. Do you want to unconfirm it?')) {{
                            var input = document.createElement('input');
                            input.type = 'hidden';
                            input.name = '__CONFIRMEDACTION';
                            input.value = 'true';
                            document.forms[0].appendChild(input);
                            __doPostBack('{0}', '');
                        }} else {{
                            // User clicked Cancel - clear the textbox
                            document.getElementById('{1}').value = '';
                            document.getElementById('{1}').focus();
                        }}
                    ", btnConfirm.UniqueID, textBoxBarcode.ClientID);

                    ClientScript.RegisterStartupScript(this.GetType(), "confirmDialog", script, true);
                    return;
                }
                //// Check if user already confirmed the dialog
                //string confirmedValue = Request.Form["__CONFIRMEDACTION"];

                //if (confirmedValue != "true")
                //{
                //    // Show confirmation dialog and resubmit if user confirms
                //    string script = @"
                //        if (confirm('This picking is already confirmed. Do you want to unconfirm it?')) {
                //            var form = document.forms[0];
                //            var input = document.createElement('input');
                //            input.type = 'hidden';
                //            input.name = '__CONFIRMEDACTION';
                //            input.value = 'true';
                //            form.appendChild(input);
                //            " + Page.ClientScript.GetPostBackEventReference(btnConfirm, "") + @";
                //        }
                //    ";
                //    ClientScript.RegisterStartupScript(this.GetType(), "confirmDialog", script, true);
                //    return;
                //}
            }


            // Get the list of invoice for the picking id
            var invoiceResult = await GetInvoiceFromGatepassAsync(pickingId);

            if (!invoiceResult.Success)
            {
                Function_Method.MsgBox(invoiceResult.Message, this.Page, this);
                return;
            }

            // Get the list of items for the invoice in invoiceList
            var itemResult = await GetItemsFromCustInvoiceTransAsync(invoiceResult.Result);

            if (!itemResult.Success)
            {
                Function_Method.MsgBox(itemResult.Message, this.Page, this);
                return;
            }

            // Update LF_WMSInventLocationQty
            var updateLocationQtyResult = await UpdateLF_WMSInventLocationQtyAsync(itemResult.Result, confirmed);

            if (!updateLocationQtyResult.Success)
            {
                Function_Method.MsgBox(updateLocationQtyResult.Message, this.Page, this);
                return;
            }


            // Update Picking Id on Mysql
            var updateDateTime = new DateTime();
            updateDateTime = DateTime.Now;
            var updatePickingIdMySqlResult = await UpdatePickingIdStatusAsync(pickingId, updateDateTime, confirmed);

            if (!updatePickingIdMySqlResult.Success)
            {
                Function_Method.MsgBox(updatePickingIdMySqlResult.Message, this.Page, this);
                //return;
            }

            // Update Picking Id on Axapta
            var updatePickingIdAxaptaResult = await UpdatePickingIdStatusAxaptaAsync(pickingId, updateDateTime, confirmed);

            if (!updatePickingIdAxaptaResult.Success)
            {
                Function_Method.MsgBox(updatePickingIdAxaptaResult.Message, this.Page, this);
            }

            //var count = 1;
            //foreach (var item in itemResult.Result)
            //{
            //    logger.Info(count);
            //    logger.Info($"Invoice Id: {item.InvoiceId}");
            //    logger.Info($"Item: {item.ItemId}");
            //    logger.Info($"InventDimId: {item.InventDimId}");
            //    logger.Info($"Location: {item.Location}");
            //    logger.Info($"Qty: {item.Qty}");
            //    logger.Info($"Unit: {item.Unit}");

            //    count++;
            //}
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            //update_PickingIDWithChecking(textBoxBarcode.Text); //update_PickingID();
            //syncToAx();
        }

        /*public static string get_table()
        {
            // from dotnet


            // to ax
            
            string pod_gps = ""; // coordinate
            string pod_date = ""; // gps_date
            string pod_time = ""; // gps_time
            string pod_veriby = ""; // status_by
            string pod_veridate = ""; // status_date
            string pod_veritime = ""; // status_date (retrieve time)
            string lfi_invreceived = ""; // status
            

            //Json Response
            string json_status = "";
            string json_status_msg = "";
            string no_of_records = "";
            string table = "";
            List<PickingObject> picking_list = new List<PickingObject>();

            //Function_Method_Sync.AddSyncLog(get_log_file_name(), "Sync started: " + DateTime.Now.ToString("HH:mm:ss"));

            int count = 0;

            MySqlConnection conn = null;

            try
            {
                conn = Function_Method_Sync.getMySqlConnection(GLOBAL.connStr);

                string sql = "SELECT * " +
                                    "FROM lf_pod " +
                                    "WHERE confirmBy != '' " +
                                    "LIMIT 50";

                MySqlCommand mysql_command = new MySqlCommand(sql, conn);

                conn.Open();
                MySqlDataReader reader = mysql_command.ExecuteReader();

                table = "<table id='sync_table' class='datatable display stripe hover table dataTable dtr-inline' style='width:100%'>" +
                    "<thead><tr>" +
                    "<th>No</th>" +
                    "<th>Picking Id</th>" +
                    "<th>Confirm Date</th>" +
                    "<th>Confirm Time</th>" +
                    "<th>Confirm By</th>" +                  
                    "</tr></thead>" +
                    "<tbody>";

                while (reader.Read())
                {
                    string pickingID = reader["pickingID"].ToString();
                    string confirmDate = reader["confirmDate"].ToString();
                    string confirmTime = reader["confirmTime"].ToString();
                    string confirmBy = reader["confirmBy"].ToString();

                    PickingObject picking_ob = new PickingObject();
                    picking_ob.pickingID = pickingID;
                    picking_ob.confirmDate = confirmDate;
                    picking_ob.confirmTime = confirmTime;
                    picking_ob.confirmBy = confirmBy;

                    picking_list.Add(picking_ob);
                }

                picking_list = sync_picking(picking_list);
                //invoice_list = update_sync_status(conn, invoice_list);
                
                foreach (PickingObject picking in picking_list)
                {
                    int index_of_slash = picking.invoice_id.IndexOf('/');

                    string status_label = "";
                    if (picking.sync_status == "success")
                    {
                        status_label = "<span class='success_label'>" + picking.sync_msg + "</span>";
                    }
                    else
                    {
                        status_label = "<span class='failed_label'>" + picking.sync_msg + "</span>";
                    }

                    string remove_flag_label = "";
                    if (picking.remove_flag_status == "success")
                    {
                        remove_flag_label = "<span class='success_label'>" + picking.remove_flag_msg + "</span>";
                    }
                    else
                    {
                        remove_flag_label = "<span class='failed_label'>" + picking.remove_flag_msg + "</span>";
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

                //Function_Method_Sync.AddSyncLog(get_log_file_name(), "No. of rows to be synced: " + no_of_records);

                conn.Close();
            }
            catch (MySqlException ex)
            {
                json_status = "failed";
                json_status_msg = "Mysql Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                //Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + json_status_msg);
            }
            catch (Exception ex)
            {
                json_status = "failed";
                json_status_msg = "Exception: " + ex.Message + Environment.NewLine +
                                         "StackTrace: " + ex.StackTrace + Environment.NewLine +
                                         "Source: " + ex.Source + Environment.NewLine +
                                         "InnerException: " + (ex.InnerException != null ? ex.InnerException.Message : "N/A");
                //Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + json_status_msg);

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
                //status = json_status,
                //status_msg = json_status_msg,
                //no_of_records = no_of_records, // Replace with the actual number of records
                //table_string = table,
                picking_list = picking_list
            };

            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }*/

        /*private static List<PickingObject> sync_picking(List<PickingObject> picking)
        {
            lock (lock_object)
            {
                Axapta dynax = Function_Method_Sync.getDynAxapta(GLOBAL_Sync.ax_user_id, GLOBAL_Sync.ax_domain_name, GLOBAL_Sync.ax_proxy_user_name, GLOBAL_Sync.ax_proxy_password,
                    GLOBAL_Sync.ax_company, GLOBAL_Sync.ax_language, GLOBAL_Sync.ax_object_server);

                // AX fields
                string picking_id;
                string confirm_date;
                string confirm_time;
                string confirm_by;
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

                    foreach (PickingObject pick in picking)
                    {
                        // prepare values for AX fields
                        picking_id = pick.pickingID;
                        confirm_date = pick.confirmDate;
                        confirm_time = pick.confirmTime;
                        confirm_by = pick.confirmBy;

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

                        using (AxaptaRecord dynrec = dynax.CreateAxaptaRecord("LF_Picking"))
                        {
                            dynrec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "PickingID", picking_id));
                            if (dynrec.Found)
                            {
                                dynrec.set_Field("confirmDate", confirm_date);
                                dynrec.set_Field("confirmTime", confirm_time);
                                dynrec.set_Field("confirmBy", confirm_by);
                                
                                dynrec.Call("Update");
                            }
                            dynrec.Dispose();

                            //invoice.sync_status = "success";
                            //invoice.sync_msg = "Sync to Ax";

                            //Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Success]Update to LF_Gatepass: invoice_id: " + invoice_id);
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
                    //Function_Method_Sync.AddSyncLog(get_log_file_name(), "[XppException] " + exception_msg);

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
                    //Function_Method_Sync.AddSyncLog(get_log_file_name(), "[Exception] " + exception_msg);

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
            return picking;
        }*/

        /*public List<Picking> GetDataFromDatabase()
        {
            List<Picking> results = new List<Picking>();

            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                conn.Open();
                string query = "SELECT * FROM LF_Picking";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new Picking
                        {
                            pickingID = reader.GetString("PickID"),
                            confirmDate = reader.GetString("confirmDate"),
                            confirmTime = reader.GetString("confirmTime"),
                            confirmBy = reader.GetString("confirmBy")
                        });
                    }
                }
            }

            return results;
        }*/

        private void syncToAx()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();
                    string selectQuery = "SELECT * FROM lf_pod";
                    string updateQuery = "update lf_pod SET confirmDate=@D2,confirmTime=@D3,confirmBy=@D4 where pickingID=@D1";
                    Axapta DynAx = Function_Method.GlobalAxapta();
                    AxaptaRecord DynRec = null;

                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        try
                        {
                            using (MySqlDataReader selectReader = selectCmd.ExecuteReader())
                            {
                                // Check if there is at least one row with a matching user_id
                                bool needsUpdate = false;
                                while (selectReader.Read())
                                {
                                    string columnValue = selectReader.GetString("confirmBy"); selectReader.GetString("pickingID");
                                    if (!string.IsNullOrEmpty(columnValue))
                                    {
                                        needsUpdate = true;
                                    }
                                }
                                selectReader.Close();
                                if (needsUpdate)
                                {
                                    DynRec = DynAx.CreateAxaptaRecord("LF_Picking");
                                    DynAx.TTSBegin();

                                    ///String query = string.Format("select forupdate * from %1 where %1.PickId == '{0}'", id);
                                    //DynRec.ExecuteStmt(query);

                                }
                            }
                        }
                        catch (Exception E)
                        {

                        }
                    }
                }
            }
            catch (Exception E)
            {

            }
        }
    }
}
