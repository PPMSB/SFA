using DotNet.SFAModel;
using GLOBAL_FUNCTION;
using iText;
//using iText.Html2pdf;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using NPOI.SS.Formula.Functions;
using SelectPdf;
//using Syncfusion.HtmlConverter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;
using static DotNet.CampaignModel.CampaignModel;
using static DotNet.SignboardEquipment;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using PdfPageSize = SelectPdf.PdfPageSize;

namespace DotNet
{
    public partial class SignBoardForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["tempSignboardNo"]))
                {
                    Label_AppNo.Text = Request.QueryString["tempSignboardNo"];

                    LoadSignboardDetails(Request.QueryString["tempSignboardNo"]);
                }
            }
            catch
            {
                
            }
            UpdateProgress.Visible = false;
            //GenerateReport();
        }

        private void LoadSignboardDetails(string tempSignboardNo)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            //Customer Information
            string CustomerAcc = ""; string ReportTo = "";
            string WareHouse = ""; string OpeningAccDate = ""; string CustName = ""; string CustPhone = "";
            string CustContact = ""; string SalesmanID = ""; string SalesmanName = ""; string CustGroup = "";
            //EOR/Signage/Supercese Case
            string FormType = ""; string FormTypeDesc = ""; string AppType = ""; string DepositAmount = ""; string DepositType = "";
            //Items Details
            string ItemDesc1 = ""; string ItemDesc2 = ""; string ItemDesc3 = "";
            string Qty1 = ""; string Qty2 = ""; string Qty3 = "";
            string Size1 = ""; string Size2 = ""; string Size3 = "";

            //Delivery Details
            string DelTo = ""; string DeliveryAddress = ""; string RemarksSales = ""; string IssueTo = "";
            //Signage License Status
            int RequestSign = 0;
            //Location Map
            string MapDesc = ""; string TrafficDensity = ""; string MapRemark = ""; string SignboardVisibility = "";
            int ImgInd = 0;
            //Comment from Sales Person
            string TypeServiceCenter = ""; string WorkshopFacilities = ""; string OwnerExperience = ""; string WorkshopSizeType = "";
            string NumberOfMechanics = ""; string YearOfEstablishment = ""; string WorkshopStatus = "";

            string ProcessStatus = ""; string AppliedDate = "";string ProcessDate = "";string DocStatus = "";

            //All Remarks // Purpose For labeling all remarks 10/6/2025
            string SalesAdminRemarks = ""; string HODRemarks = ""; string HODRemarks2 = ""; string HODRemarks3 = ""; string ANPRemarks = "";
            string AdminMgrRemarks = ""; string GMRemarks = ""; string RemarksDisplay = ""; string HODRemarks4 = "";

            //Sub Dealer or Branch  
            string SubDBWorkshopName = "";

            #region LF_WebEquipment

            int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WebEquipment);

            int cn1 = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

            qbr6.Call("value", tempSignboardNo);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WebEquipment);
                CustomerAcc = DynRec6.get_Field("CustAccount").ToString();  
                CustPhone = DynRec6.get_Field("CustPhone").ToString();
                CustContact = DynRec6.get_Field("CustContact").ToString();
                RequestSign = (string.IsNullOrEmpty(DynRec6.get_Field("RequestSign").ToString()) ? 0 : Int32.Parse(DynRec6.get_Field("RequestSign").ToString()));
                FormType = DynRec6.get_Field("FormType").ToString();
                AppType = DynRec6.get_Field("AppType").ToString();
                DepositType = DynRec6.get_Field("DepositType").ToString();
                DelTo = DynRec6.get_Field("Del_To").ToString();
                DeliveryAddress = DynRec6.get_Field("Del_Addr").ToString();
                RemarksSales = DynRec6.get_Field("Remarks_Sales").ToString();
                TypeServiceCenter = DynRec6.get_Field("ServiceType").ToString();
                WorkshopFacilities = DynRec6.get_Field("ShopFacility").ToString();
                OwnerExperience = DynRec6.get_Field("OwnerExp").ToString();
                WorkshopSizeType = DynRec6.get_Field("ShopSize").ToString();
                NumberOfMechanics = DynRec6.get_Field("Worker").ToString();
                YearOfEstablishment = DynRec6.get_Field("YearEstablish").ToString();
                WorkshopStatus = DynRec6.get_Field("ShopStatus").ToString();
                MapDesc = DynRec6.get_Field("MapLocation").ToString();
                TrafficDensity = DynRec6.get_Field("MapTraffic").ToString();
                MapRemark = DynRec6.get_Field("MapRemark").ToString();
                SubDBWorkshopName = DynRec6.get_Field("Branch_WorkshopName").ToString();
                SignboardVisibility = DynRec6.get_Field("MapSCVisible").ToString();
                ImgInd = Int32.Parse(DynRec6.get_Field("Img_Ind").ToString());
                ProcessStatus = DynRec6.get_Field("ProcessStatus").ToString();
                IssueTo = DynRec6.get_Field("Del_Person").ToString();
                string appliedDateString = DynRec6.get_Field("AppliedDate").ToString();
                DateTime appliedDate;
                if (DateTime.TryParseExact(appliedDateString, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out appliedDate))
                {
                    AppliedDate = appliedDate.ToString("dd/MM/yyyy");
                }
                string processDateString = DynRec6.get_Field("ProcessDate").ToString();
                DateTime processDate;
                if (DateTime.TryParseExact(processDateString, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out processDate)) 
                {
                    ProcessDate = processDate.ToString("dd/MM/yyyy");
                }

                //Might change
                DepositAmount = DynRec6.get_Field("Cost").ToString();

                ItemDesc1 = DynRec6.get_Field("Item1").ToString();
                ItemDesc2 = DynRec6.get_Field("Item2").ToString();
                ItemDesc3 = DynRec6.get_Field("Item3").ToString();
                Qty1 = DynRec6.get_Field("Qty1").ToString();
                Qty2 = DynRec6.get_Field("Qty2").ToString();
                Qty3 = DynRec6.get_Field("Qty3").ToString();
                Size1 = DynRec6.get_Field("Size1").ToString();
                Size2 = DynRec6.get_Field("Size2").ToString();
                Size3 = DynRec6.get_Field("Size3").ToString();

                DocStatus = DynRec6.get_Field("DocStatus").ToString();

                #region Remarks   

                // Set fields
                SalesAdminRemarks = DynRec6.get_Field("Remarks_Admin").ToString();
                HODRemarks = DynRec6.get_Field("Remarks_HOD").ToString();
                HODRemarks2 = DynRec6.get_Field("Remarks_HOD_2").ToString();
                HODRemarks3 = DynRec6.get_Field("Remarks_HOD_3").ToString();
                HODRemarks4 = DynRec6.get_Field("Remarks_HOD_4").ToString();
                ANPRemarks = DynRec6.get_Field("RemarksAnP").ToString();
                AdminMgrRemarks = DynRec6.get_Field("Remarks_AdminMgr").ToString();
                GMRemarks = DynRec6.get_Field("Remarks_GM").ToString();

                var remarksDictionary = new Dictionary<string, string>
                                {
                                    { "HOD Remarks", HODRemarks },{ "2nd HOD Remarks", HODRemarks2 },
                                    { "3rd HOD Remarks", HODRemarks3},{ "4th HOD Remarks", HODRemarks4},
                                    };

                // Build RemarksDisplay
                if (!string.IsNullOrEmpty(RemarksSales))
                {
                    RemarksDisplay += $"SB Remarks: {RemarksSales} \n"; // No </br> for SalesManRemarks
                }
                foreach (var remark in remarksDictionary)
                {
                    if (!string.IsNullOrEmpty(remark.Value))
                    {
                        RemarksDisplay += $"</br> {remark.Key}: {remark.Value} \n";
                    }
                }
                lblAllRemarks.Text = RemarksDisplay;
                //divAllRemarks.Visible = true;
                #endregion

                DynRec6.Dispose();
            }
            axQueryRun6.Dispose();
            axQueryDataSource6.Dispose();
            #endregion
            #region CustTable
            int CustTable = DynAx.GetTableIdWithLock("CustTable");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            int custAcc = DynAx.GetFieldId(CustTable, "AccountNum");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", custAcc);

            qbr.Call("value", CustomerAcc);
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                WareHouse = DynRec.get_Field("Dimension").ToString();
                OpeningAccDate = DynRec.get_Field("OpeningAccDate").ToString();
                CustName = DynRec.get_Field("Name").ToString();
                SalesmanID = DynRec.get_Field("EmplId").ToString();
                CustGroup = DynRec.get_Field("CustGroup").ToString();

                DynRec.Dispose();
            }
            axQueryRun.Dispose();
            axQueryDataSource.Dispose();
            #endregion
            #region EmplTable
            int EmplTable = DynAx.GetTableIdWithLock("EmplTable");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

            int emplID = DynAx.GetFieldId(EmplTable, "EmplId");
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", emplID);

            qbr1.Call("value", SalesmanID);
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                SalesmanName = DynRec1.get_Field("LF_EmplName").ToString();
                SalesmanName = (string.IsNullOrEmpty(SalesmanName) ? DynRec1.get_Field("Del_Name").ToString() : SalesmanName);
                ReportTo = DynRec1.get_Field("ReportTo").ToString();

                DynRec1.Dispose();
            }
            axQueryRun1.Dispose();
            axQueryDataSource1.Dispose();
            #endregion

            // Implement your logic to load signboard details from the database
            // For demonstration, we'll just set some dummy data
            Label_AppNo.Text = tempSignboardNo;
            Label_SubDate.Text = AppliedDate;
            lblCustName.Text = CustName;
            lblCustAcc.Text = CustomerAcc;
            lblCustTelNo.Text = CustPhone;
            lblCustContactPerson.Text = CustContact;
            if (FormType == "1"){FormTypeDesc = "New";}
            else if (FormType == "2"){FormTypeDesc = "Branch";}
            else if (FormType == "3"){FormTypeDesc = "Existing";}
            else if (FormType == "4"){FormTypeDesc = "Dealer";}
            else {FormTypeDesc = "N/A";}
            lblRequestType.Text = FormTypeDesc;
            lblHQBR.Text = WareHouse;
            lblClass.Text = CustGroup;
            lblSalesman.Text = SalesmanName + " (" + SalesmanID + ")";
            lblDepositAmt.Text = "0";
            lblDepositEORSC.Text = DepositType;

            lblItem1.Text = ItemDesc1;
            lblItem2.Text = ItemDesc2;
            lblItem3.Text = ItemDesc3;
            lblQty1.Text = Qty1;
            lblQty2.Text = Qty2;
            lblQty3.Text = Qty3;
            lblSize1.Text = Size1;
            lblSize2.Text = Size2;
            lblSize3.Text = Size3;
            lblTotalCost.Text = DepositAmount;
            lblItemDeliver.Text = DelTo;
            lblAddress.Text = DeliveryAddress;
            lblRemarks.Text = RemarksSales;
            lblSupplier.Text = IssueTo;
            lblBeneficiaryBank.Text = "Owner - ";
            rblRequestSign.SelectedValue = RequestSign.ToString();
            lblSBWorkshopName.Text = SubDBWorkshopName;

            DateTime appliedDate2;
            if (DateTime.TryParseExact(AppliedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out appliedDate2))
            {
                GetPastMonthsPurchaseRecord(CustomerAcc, appliedDate2);
            }

            
            DivDisplayMapA.Visible = MapDesc.Contains("A");
            DivDisplayMapB.Visible = MapDesc.Contains("B");
            DivDisplayMapC.Visible = MapDesc.Contains("C");
            lblMapDesc.Text = MapDesc;
            lblTrafficDensity.Text = TrafficDensity;
            lblMapRemark.Text = MapRemark;
            lblSignVisibility.Text = SignboardVisibility;


            lblTypeOfServiceCenter.Text = TypeServiceCenter;
            lblOwnerExperience.Text = OwnerExperience;
            lblNumberOfMechanics.Text = NumberOfMechanics;
            lblYearOfEstablishment.Text = YearOfEstablishment;

            lblWorkshopFacilities.Text = WorkshopFacilities;
            lblWorkshopSizeType.Text = WorkshopSizeType;
            lblWorkshopStatus.Text = WorkshopStatus;

            lblCustClass.Text = CustGroup;
            lblEOLPerformance.Text = "Grade: N/A";
            lblEORPerformance.Text = "Grade: N/A";

            lblGetProcessStat.Text = ProcessStatus.Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>");
            lblGetProcessDate.Text = ProcessDate;

            //if (DocStatus == "") {
            //divLabelDraft.Visible = false;
            //}



            // Add more fields as necessary
        }

        private void GetPastMonthsPurchaseRecord(string custAcc, DateTime AppliedDate)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int CustTrans = DynAx.GetTableIdWithLock("CustTrans");

                int custAccount = DynAx.GetFieldId(CustTrans, "AccountNum");
                int TransDate = DynAx.GetFieldId(CustTrans, "TransDate");
                int AmountCur = DynAx.GetFieldId(CustTrans, "AmountCur");
                int TransType = DynAx.GetFieldId(CustTrans, "TransType");


                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

                // Add range for AccountNum
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", custAccount);//claimstat
                qbr.Call("value", custAcc);

                // Add range for TransType
                var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", TransType);//claimstat
                qbr2.Call("value", "Sales Order");

                // Calculate the date 6 months ago
                DateTime sixMonthsAgo = AppliedDate.AddMonths(-6);//DateTime.Now.AddMonths(-6);
                string formattedDate = sixMonthsAgo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // Format as needed

                // Add range for TransDate for the last 6 months
                var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", TransDate);
                qbr3.Call("value", formattedDate);
                qbr3.Call("value", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)); // End date is today


                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                // Prepare the list to hold records
                List<CustTransRecord> records = new List<CustTransRecord>();

                while ((bool)axQueryRun.Call("next"))
                {
                    #region RetrieveData
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);
                    // Retrieve field values from the current record

                    object transDateObj = DynRec.get_Field("TransDate").ToString();
                    DateTime transDate;
                    // Check if the retrieved object is of type DateTime
                    if (transDateObj is DateTime)
                    {
                        transDate = (DateTime)transDateObj; // Cast to DateTime
                    }
                    else if (transDateObj is string)
                    {
                        string dateString = (string)transDateObj; // Cast to string
                        if (DateTime.TryParse(dateString, out transDate))
                        {
                            // Successfully parsed from string
                        }
                        else
                        {
                            transDate = DateTime.MinValue; // Handle the error as needed
                                                           // Optionally log an error or throw an exception
                        }
                    }
                    else
                    {
                        transDate = DateTime.MinValue; // Handle the error as needed
                                                       // Optionally log an error or throw an exception
                    }

                    object amountCurObj = DynRec.get_Field("AmountCur").ToString();
                    double amountCur;
                    // Check if the retrieved object is of type double
                    if (amountCurObj is double)
                    {
                        amountCur = (double)amountCurObj; // Cast to double
                    }
                    else if (amountCurObj is string)
                    {
                        string s = (string)amountCurObj; // Cast to string
                        double parsed; // Declare the variable before using it
                        if (double.TryParse(s, out parsed))
                        {
                            amountCur = parsed; // Successfully parsed from string
                        }
                        else
                        {
                            amountCur = 0; // Handle the error as needed
                                           // Optionally log an error or throw an exception
                        }
                    }
                    else
                    {
                        amountCur = 0; // Handle the error as needed
                                       // Optionally log an error or throw an exception
                    }
                    // Create a new record object

                    CustTransRecord record = new CustTransRecord
                    {

                        TransDate = transDate,
                        AmountCur = amountCur
                    };
                    // Add to the list
                    records.Add(record);
                    #endregion

                }

                // Now summarize the records for the past 6 months
                var summaries = GetMonthlySummaries(records, 6);
                // Display results in the label with line breaks
                var htmlTable = BuildHtmlTable(summaries, 3);
                lblPastRecord.Text = htmlTable;
                // Assuming records is your list of CustTransRecord

            }
            catch (Exception e)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine(e.Message);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private List<string> GetMonthlySummaries(List<CustTransRecord> records, int monthsBack)
        {
            DateTime now = DateTime.Now;
            DateTime startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1 - monthsBack);
            // Prepare a list of months to cover
            List<DateTime> monthList = new List<DateTime>();
            for (int i = 0; i < monthsBack; i++)
            {
                monthList.Add(startMonth.AddMonths(i));
            }
            // Group records by year and month
            var grouped = records
                .Where(r => r.TransDate >= startMonth)
                .GroupBy(r => new { r.TransDate.Year, r.TransDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(x => x.AmountCur)
                })
                .ToList();

            #region For Outstanding Record
            double totalAmountCur = grouped.Sum(record => record.TotalAmount);
            double AvgrMonthlySales6Mth = totalAmountCur / 6;

            lblAvgSalesMonth.Text = "Average monthly sales (6 mth) : RM" + AvgrMonthlySales6Mth.ToString("#,###,###,##0.00");
            #endregion

            // Prepare the result list
            List<string> result = new List<string>();
            List<string> tempRow = new List<string>(); // Temporary list for current row
            foreach (var m in monthList)
            {
                var monthSummary = grouped.FirstOrDefault(g => g.Year == m.Year && g.Month == m.Month);
                double total = monthSummary?.TotalAmount ?? 0.0;
                string formattedMonth = m.ToString("MMM yyyy", CultureInfo.InvariantCulture);
                result.Add($"{formattedMonth} - RM {total:F2}");

                // If we have 3 items in the current row, join them and add to the result
                if (tempRow.Count == 3)
                {
                    result.Add(string.Join(" | ", tempRow)); // Use a separator of your choice
                    tempRow.Clear(); // Clear the temporary row for the next set
                }
            }

            // If there are any remaining items in tempRow, add them as well
            if (tempRow.Count > 0)
            {
                result.Add(string.Join(" | ", tempRow));
            }
            // Reverse the result to show the latest month first (optional)
            result.Reverse();
            return result;
        }

        private string BuildHtmlTable(List<string> summaries, int columnsPerRow = 3)
        {
            var table = new System.Text.StringBuilder();
            table.Append("<table style='border-collapse: collapse;'>");
            for (int i = 0; i < summaries.Count; i++)
            {
                if (i % columnsPerRow == 0)
                    table.Append("<tr>");
                table.AppendFormat("<td style='padding: 8px 20px; border: 1px solid #ccc;'>{0}</td>", summaries[i]);
                if (i % columnsPerRow == columnsPerRow - 1)
                    table.Append("</tr>");
            }
            // Close last row if not completed
            if (summaries.Count % columnsPerRow != 0)
                table.Append("</tr>");
            table.Append("</table>");
            return table.ToString();
        }

        private void GenerateReport()
        {
            string htmlString = RenderControlToString(pnlContent);
            //string htmlString = "<html><body><h1>Hello World!</h1></body></html>";
            string baseUrl = "";
            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 850;

            int webPageHeight = 1100;
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms);
                byte[] pdfBytes = ms.ToArray();

                string filename = " - ES Report: " + Label_AppNo.Text + ".pdf";
             
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", $"attachment; filename=\"{filename}\"");
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.End();
            }

            // close pdf document
            doc.Close();
        }
        protected void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            string htmlString = RenderControlToString(pnlContent);
            //string htmlString = "<html><body><h1>Hello World!</h1></body></html>";
            string baseUrl = "";
            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 850;
            //try
            //{
            //    webPageWidth = Convert.ToInt32(TxtWidth.Text);
            //}
            //catch { }

            int webPageHeight = 1100;
            //try
            //{
            //    webPageHeight = Convert.ToInt32(TxtHeight.Text);
            //}
            //catch { }

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

            // save pdf document
            doc.Save(Response, false, "Sample.pdf");

            // close pdf document
            doc.Close();
            // Render the HTML content from the panel
            //string htmlContent = RenderControlToString(pnlContent);
            //string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
            //// Convert the HTML to PDF
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    // Use iText7 to convert HTML to PDF
            //    HtmlConverter.ConvertToPdf(htmlContent, stream);

            //    // Send the PDF back to the browser
            //    Response.Clear();
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-disposition", "attachment; filename=Sample.pdf");
            //    Response.BinaryWrite(stream.ToArray());
            //    Response.End();
            //}
        }

        private string RenderControlToString(Control control)
        {
            // Create a StringWriter to hold the rendered output
            using (StringWriter stringWriter = new StringWriter())
            {
                // Create a HtmlTextWriter to write the control content to the StringWriter
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    // Render the control to the HtmlTextWriter
                    control.RenderControl(htmlWriter);
                    return stringWriter.ToString(); // Get the rendered HTML as string
                }
            }
        }
    }
}