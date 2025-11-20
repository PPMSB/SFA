using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText;
//using iText.Html2pdf;
using SelectPdf;
//using Syncfusion.HtmlConverter;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static DotNet.CampaignModel.CampaignModel;
using PdfPageSize = SelectPdf.PdfPageSize;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

namespace DotNet
{
    public partial class Campaign_DocumentPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["tempCamp"]))
                {
                    lblCampaignID.Text = Request.QueryString["tempCamp"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["tempCust"]))
                {
                    lblCustomerAccount.Text = Request.QueryString["tempCust"];
                }
            }
            catch
            {
                
            }
            
            BindDocumentData();
            BindGrid();
            UpdateProgress.Visible = false;
            GenerateReport();
            //btnGeneratePDF_Click(sender, e);
        }

        private void BindGrid()
        {
            List<CampaignTargetPercentGridViewModel> mList = new List<CampaignTargetPercentGridViewModel>();

            MySqlConnection conn = new MySqlConnection(GLOBAL_VAR.GLOBAL.connStr);
            string CampaignID = lblCampaignID.Text;
            string CustomerAccount = lblCustomerAccount.Text;
            conn.Open();
            string SQL = "select t1.TargetAmount, t2.Product, t2.Percent, t3.CampaignTerms from campaign_targetheader t1 inner join campaign_targetpercent t2 on t2.RefTargetID = t1.TargetID and t1.CampaignID = t2.CampaignID and t1.CustomerAccount = t2.CustomerAccount " +
                "inner join campaign_document t3 on t3.CampaignID = t1.CampaignID and t3.CustomerAccount = t1.CustomerAccount where t1.CampaignID = @p0 and t1.CustomerAccount = @p1 group by t2.RefTargetID, t2.Product order by t1.TargetAmount desc, t2.Product asc LIMIT 10;";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            Dictionary<string, object> ParamDict = new Dictionary<string, object>
            {
                { "@p0", CampaignID },
                { "@p1", CustomerAccount }
            };

            GlobalHelper.PumpParamQuery(cmd, ParamDict);

            bool DataExisted = false;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                CampaignTargetPercentGridViewModel m = new CampaignTargetPercentGridViewModel();
                string CurrentAmountReading = "";

                int i = 0;
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        string CurrentAmount = reader.GetValue(0).ToString();
                        if (CurrentAmountReading != CurrentAmount)
                        {
                            i++;

                            CurrentAmountReading = CurrentAmount;
                            m = new CampaignTargetPercentGridViewModel();

                            m.TargetAmount = reader.GetValue(3).ToString() + " Target " + i + " : <br/>RM" + Double.Parse(CurrentAmount).ToString("N") +
                                (i == 2 ? "<br/><span style='font-size:9px'>*Refer to Term & Condition No.5</span>" : "");
                        }
                        if (reader.GetValue(1).ToString() == "Product A")
                        {
                            m.ProductA = "<u>" + reader.GetValue(2).ToString() + "%</u>";
                        }
                        else if (reader.GetValue(1).ToString() == "Product B")
                        {
                            m.ProductB = "<u>" + reader.GetValue(2).ToString() + "%</u>";
                        }
                        else if (reader.GetValue(1).ToString() == "Product C")
                        {
                            m.ProductC = "<u>" + reader.GetValue(2).ToString() + "%</u>";
                        }
                        else if (reader.GetValue(1).ToString() == "Product D")
                        {
                            m.ProductD = "<u>" + reader.GetValue(2).ToString() + "%</u>";
                        }
                        else if (reader.GetValue(1).ToString() == "Product E")
                        {
                            m.ProductE = "<u>" + reader.GetValue(2).ToString() + "%</u>";

                            mList.Add(m);
                        }
                        DataExisted = true;
                    }

                }
            }

            if (!DataExisted)
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                for (var i = 1; i <= 2; i++)
                {
                    mList.Add(PrintDummyTargetPercentageTable(reader, i));
                }
            }
            conn.Close();
            tbProductPercentage.DataSource = mList;
            tbProductPercentage.DataBind();
        }

        private CampaignTargetPercentGridViewModel PrintDummyTargetPercentageTable(MySqlDataReader reader, int i)
        {

            CampaignTargetPercentGridViewModel m = new CampaignTargetPercentGridViewModel();

            m.TargetAmount = (i == 2 ? "One-level-down Target" : "Sales Target") + ": <br/>RM" + "<u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u>" +
                (i == 2 ? "<br/><span style='font-size:9px'>*Refer to Term & Condition No.5</span>" : "");

            m.ProductA = "<u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;%</u>";
            m.ProductB = "<u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;%</u>";
            m.ProductC = "<u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;%</u>";
            m.ProductD = "<u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;%</u>";
            m.ProductE = "<u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;%</u>";

            return m;
        }

        private void BindDocumentData()
        {
            //ExistedCampaignWrapper.Visible = false;
            //NewCampaignWrapper.Visible = false;
            SalesRecordWrapper.Visible = true;

            MySqlConnection conn = new MySqlConnection(GLOBAL_VAR.GLOBAL.connStr);
            conn.Open();
            string CampaignID = lblCampaignID.Text;
            string CustomerAccount = lblCustomerAccount.Text;

            string SQL = "select t1.CampaignID, t1.SequenceNo, t1.CustomerAccount, t1.WorkshopName, t1.Salesman, t1.CampaignTerms, t1.CampaignStartDate, t1.CampaignEndDate, t1.PastYearSales, t2.TargetAmount from campaign_document t1 " +
                "left join campaign_targetheader t2 on t2.CampaignID = t1.CampaignID and t2.CustomerAccount = t1.CustomerAccount where t1.CampaignID = @p0 and t1.CustomerAccount = @p1 order by t2.TargetAmount desc limit 1";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            Dictionary<string, object> ParamDict = new Dictionary<string, object>
            {
                { "@p0", CampaignID },
                { "@p1", CustomerAccount }
            };

            GlobalHelper.PumpParamQuery(cmd, ParamDict);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        string Target1 = "";
                        try
                        {
                            Target1 = Double.Parse(reader.GetValue(9).ToString()).ToString("N2");
                            ExistedCampaignWrapper.Visible = true;
                        }
                        catch
                        {
                            //NewCampaignWrapper.Visible = true;
                            SalesRecordWrapper.Visible = false;
                            Target1 = "_____________________";
                        }

                        lblCampaignID.Text = reader.GetValue(0).ToString();
                        lblSequenceNo.Text = reader.GetValue(1).ToString();
                        lblCustomerAccount.Text = reader.GetValue(2).ToString();
                        lblWorkshopName.Text = reader.GetValue(3).ToString();
                        lblSalesman.Text = reader.GetValue(4).ToString();
                        lblFirstTarget1.InnerText = "RM" + Target1;
                        lblTerms.InnerText = reader.GetValue(5).ToString();
                        lblTerms1.InnerText = reader.GetValue(5).ToString();
                        lblTerms2.InnerText = reader.GetValue(5).ToString();
                        lblTerms3.InnerText = reader.GetValue(5).ToString();
                        lblTerms4.InnerText = reader.GetValue(5).ToString();
                        spanCampaignStartDate.InnerText = DateTime.Parse(reader.GetValue(6).ToString()).ToString(GLOBAL_VAR.GLOBAL.gDisplayDateFormat);
                        spanCampaignEndDate.InnerText = DateTime.Parse(reader.GetValue(7).ToString()).ToString(GLOBAL_VAR.GLOBAL.gDisplayDateFormat);
                        lblSalesRecord.InnerText = Double.Parse(reader.GetValue(8).ToString()).ToString("N2");
                    }
                }
            }

            conn.Close();
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

            // save pdf document
            doc.Save(Response, true, DateTime.Now.ToString("dd/MM/yyyy") + " - Campaign: " + lblCampaignID.Text + " - Customer Account: " + lblCustomerAccount.Text + ".pdf");

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