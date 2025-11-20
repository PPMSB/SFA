using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DotNet
{
    public partial class PrintingLayout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();

            if (!IsPostBack)
            {
                Session["flag_temp"] = 0;
                Check_DataRequest();
                Session["data_passing"] = "";//in case forget reset
            }
        }
        private void check_session()
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
                Response.Redirect("LoginPage.aspx");
            }
        }
        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        private void Check_DataRequest()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string temp1 = GLOBAL.data_passing.ToString();
                var split = temp1.Split('|');
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        string tmp = Session["data_passing"].ToString();
                        var getRedemp = Redemption_Get_Details.get_redemption(DynAx, split[0]);
                        var getRedempInfo = Redemption_Get_Details.get_rd_CreditInfo(DynAx, split[1]);
                        var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, split[1]);
                        var getStat = Redemption_Get_Details.get_redempStat(DynAx, split[0]);
                        var getAdminStat = Redemption_Get_Details.get_redempStat2(DynAx, split[0]);
                        var getInvoice = Redemption_Get_Details.get_gridViewDataInvoice(DynAx, split[0]);
                        var getAdminRemarks = Redemption_Get_Details.get_gridViewDataAdmin(DynAx, split[0]);
                        var getLatestTransDate = Redemption_Get_Details.get_latestInvoiceTrans(DynAx, split[1]);
                        var getItems = Redemption_Get_Details.get_gridViewData(DynAx, split[0]);
                        var getItems2 = Redemption_Get_Details.get_gridViewData2(DynAx, split[0]);
                        var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, split[1]);
                        string getJournalId = Redemption_Get_Details.getPPMTP_JournalId(DynAx, split[1], split[0]);

                        if (getJournalId == "")
                        {
                            getJournalId = Redemption_Get_Details.getPPMTP_JournalId_AfterPosting(DynAx, split[1], split[0]);
                        }

                        string CustomerClass = tuple_getCustInfo.Item6.Trim();
                        string ClassDesc = EOR_GET_NewApplicant.getClassDetails(DynAx, CustomerClass);

                        Label_CustName.Text = getRedempInfo.Item1;
                        Label_Salesman.Text = getRedempInfo.Item7;
                        Label_Class.Text = ClassDesc;
                        Label_HqBr.Text = tuple_getCustInfo.Item5;
                        Label_AccNo.Text = getRedemp.CustAcc;
                        Label_HpNo.Text = getRedemp.CustPhone;
                        Label_AppNo.Text = tmp.Substring(0, 10);
                        Label_ContactNo.Text = getRedemp.CustContact;

                        string submitDt = "";

                        if (!string.IsNullOrEmpty(getInvoice.Item7.Item4))
                        {
                            submitDt = Convert.ToDateTime(getInvoice.Item7.Item4).ToString("dd/MM/yyyy");
                            Label_SubmissionDt.Text = submitDt;
                        }

                        switch (getAdminRemarks.Item6)
                        {
                            case "1":
                                Label_RedempType.Text = "Purchase Order";
                                break;
                            case "2":
                                Label_RedempType.Text = "Sales Order";
                                break;
                            case "3":
                                Label_RedempType.Text = "Credit Note";
                                break;
                            default:
                                Label_RedempType.Text = "Customer";
                                break;
                        }

                        Label_Loyalty.Text = get_Point.Item1.ToString("#,###,###,##0.00") + "(" + submitDt + ")";

                        double totalPoints = Convert.ToDouble(getItems2.Item1 + getItems2.Item2 + getItems2.Item3);
                        List<ItemData> itemList = new List<ItemData>
                        {
                            new ItemData { No = "1", Item = getItems.Item1, Quantity = Convert.ToInt16(getItems.Item4), Amount = getItems.Rest.Item3,
                                PointsValue = getItems2.Item1.ToString("#,###,###,##0.00"), InvoiceNo = getInvoice.Item1, InvoiceDate = getInvoice.Item4.ToString() },

                            new ItemData { No = "2", Item = getItems.Item2.ToString(), Quantity = Convert.ToInt16(getItems.Item5), Amount = getItems.Rest.Item4,
                                PointsValue = getItems2.Item2.ToString("#,###,###,##0.00"), InvoiceNo = getInvoice.Item2, InvoiceDate = getInvoice.Item5.ToString() },

                            new ItemData { No = "3", Item = getItems.Item3.ToString(), Quantity = Convert.ToInt16(getItems.Item6), Amount = getItems.Rest.Item5,
                                PointsValue = getItems2.Item3.ToString("#,###,###,##0.00"), InvoiceNo = getInvoice.Item3, InvoiceDate = getInvoice.Item6.ToString() },

                            new ItemData { No = "", Item = "TOTAL REDEMPTION", Quantity = Convert.ToInt16(getItems.Item4) + Convert.ToInt16(getItems.Item5) + Convert.ToInt16(getItems.Item6), Amount = getItems.Rest.Item3 + getItems.Rest.Item4 + getItems.Rest.Item5,
                                PointsValue = totalPoints.ToString("#,###,###,##0.00"), InvoiceNo = "", InvoiceDate = "" }
                        };

                        foreach (var item in itemList)
                        {
                            // Create a new row
                            TableRow row = new TableRow();

                            // Create cells and add them to the row
                            row.Cells.Add(CreateCell(item.No.ToString()));
                            row.Cells.Add(CreateCell(item.Item));
                            row.Cells.Add(CreateCell(item.Quantity.ToString()));
                            row.Cells.Add(CreateCell(item.Amount.ToString("F2"))); // Formats decimal to two decimal places
                            row.Cells.Add(CreateCell(item.PointsValue.ToString()));
                            row.Cells.Add(CreateCell(item.InvoiceNo));
                            row.Cells.Add(CreateCell(item.InvoiceDate));

                            // Add the row to the table
                            table_particular.Rows.Add(row);
                        }

                        Lbl_deliverTo.Text = getStat.Item2;

                        if (getRedemp.benefitIc == "")
                        {
                            Lbl_BeneIC.Text = "-";
                        }
                        else
                        {
                            Lbl_BeneIC.Text = getRedemp.benefitIc;
                        }

                        if (getStat.Item4 == "")
                        {
                            Lbl_BeneBankAcc.Text = "-";
                        }
                        else
                        {
                            string beneBank = getStat.Item4;
                            switch (getStat.Item4)
                            {
                                // Jerry 2024-11-14 Wrong value assigned
                                //case "1":
                                //    beneBank = "Company";
                                //    break;
                                //case "2":
                                //    beneBank = "None";
                                //    break;
                                //default:
                                //    beneBank = "Owner";
                                //    break;
                                case "1":
                                    beneBank = "Company";
                                    break;
                                case "2":
                                    beneBank = "Owner";
                                    break;
                                default:
                                    beneBank = "None";
                                    break;
                                // Jerry 2024-11-14 End
                            }


                            Lbl_BeneBankAcc.Text = beneBank;
                        }
                        Lbl_BeneName.Text = getRedemp.benefitName;
                        Lbl_Remarks.Text = getStat.Item5;
                        Lbl_Address.Text = getRedemp.CustAddr;
                        Lbl_LastInvoice.Text = getLatestTransDate.Item1 + " " + getLatestTransDate.Item2;
                        Lbl_HODRemark.Text = getStat.Item6;
                        Lbl_ProcessDt.Text = getStat.Rest.Item1;
                        Lbl_ProcessStat.Text = getStat.Item1;
                        Lbl_JournalRemark.Text = getStat.Item7;
                        Lbl_Journal.Text = getJournalId;

                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("PointBalance"))
                        {
                            DynAx.TTSBegin();
                            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", "AccountNum", split[1]));

                            var LPCF = DynRec.get_Field("TPBalance");
                            var APCF = DynRec.Call("getAddBalance");
                            var LPBal = DynRec.Call("getTotBalance", get_Point.Item2.ToString());
                            var APBal = DynRec.Call("getTotAddBalance", get_Point.Item2.ToString());

                            string lp = Convert.ToDouble(LPCF).ToString("#,###,###,##0.00");
                            string ap = Convert.ToDouble(APCF).ToString("#,###,###,##0.00");
                            string lpBal = Convert.ToDouble(LPBal).ToString("#,###,###,##0.00");
                            string apBal = Convert.ToDouble(APBal).ToString("#,###,###,##0.00");

                            double ApDeduct = Convert.ToDouble(APBal) - Convert.ToDouble(ap);

                            LiteralPreposted.Text = "LP: " + (getRedemp.CreatedLP == "0" ? lp : getRedemp.CreatedLP) + " (" + getRedemp.date.ToString() + " " + getRedemp.time.ToString() + ")" + "<br/>" +
                        "AP: " + (getRedemp.CreatedLP == "0" ? ap : getRedemp.CreatedAP) + " (" + getRedemp.date.ToString() + " " + getRedemp.time.ToString() + ")" + "<br/>";

                            // Jerry 2024-12-19 Recalculation of Preposted, Deducted and Balance A&P and Loyalty points
                            // if A&P point is used (full or partial)
                            // Total redeemed point
                            double redeemed_point = Convert.ToDouble(getItems2.Item5);

                            // Preposted A&P and Loyalty points
                            double preposted_ap = Convert.ToDouble(ap);
                            double preposted_lp = Convert.ToDouble(lp);

                            // Deducted A&P points
                            double deducted_ap = !string.IsNullOrEmpty(getAdminRemarks.Item1?.ToString())
                                                    ? Convert.ToDouble(getAdminRemarks.Item1.ToString())
                                                    : 0;

                            // Deducted Loyalty points (redeemed points deduct A&P points first, balance deduct by lp
                            double deducted_lp = redeemed_point - deducted_ap;

                            // Balance of A&P and Loyalty points after deduction
                            double lp_new_balance = preposted_lp - deducted_lp;
                            double ap_new_balance = preposted_ap - deducted_ap;
                            double old_lp_new_balance = double.Parse(getRedemp.CreatedLP) - deducted_lp;
                            double old_ap_new_balance = double.Parse(getRedemp.CreatedAP) - deducted_ap;

                            LiteralLp.Text = lp_new_balance.ToString("#,###,###,##0.00");
                            LiteraldeductLp.Text = (-deducted_lp).ToString("#,###,###,##0.00");

                            LiteralAp.Text = ap_new_balance.ToString("#,###,###,##0.00");
                            LiteralDeductAp.Text = (-deducted_ap).ToString("#,###,###,##0.00");
                            if (getRedemp.CreatedLP != "0")
                            {
                                LiteralLp.Text = old_lp_new_balance.ToString("#,###,###,##0.00");
                                LiteraldeductLp.Text = (-deducted_lp).ToString("#,###,###,##0.00");

                                LiteralAp.Text = old_ap_new_balance.ToString("#,###,###,##0.00");
                                LiteralDeductAp.Text = (-deducted_ap).ToString("#,###,###,##0.00");
                            }
                            else
                            {
                                LiteralLp.Text = lp_new_balance.ToString("#,###,###,##0.00");
                                LiteraldeductLp.Text = (-deducted_lp).ToString("#,###,###,##0.00");

                                LiteralAp.Text = ap_new_balance.ToString("#,###,###,##0.00");
                                LiteralDeductAp.Text = (-deducted_ap).ToString("#,###,###,##0.00");
                            }


                            /*
                            if (!string.IsNullOrEmpty(getAdminRemarks.Item1.ToString())) // getAdminRemarks.Item1 = HODAnP
                            {
                                double totalDeductAP = Convert.ToDouble(ap) - Convert.ToDouble(getItems2.Item5); // getItems2.Item5 = Rdempt_Point
                                LiteralAp.Text = totalDeductAP.ToString("#,###,###,##0.00") + "(" + DateTime.Now + ")";
                                LiteralLp.Text = lp + "(" + DateTime.Now + ")";

                                // Jerry 2024-11-28 Show the correct LP: LP = Rdempt_Point - HODAnP
                                //LiteraldeductLp.Text = "0";

                                double deducted_AP = Convert.ToDouble(getAdminRemarks.Item1);

                                LiteraldeductLp.Text = (deducted_AP - Convert.ToDouble(getItems2.Item5)).ToString("#,###,###,##0.00");

                                //LiteralDeductAp.Text = "-" + getItems2.Item5.ToString("#,###,###,##0.00");

                                LiteralDeductAp.Text = "-" + deducted_AP.ToString("#,###,###,##0.00");

                                // Jerry 2024-11-28 End

                            }
                            else
                            {
                                double totalDeductLP = Convert.ToDouble(lp) - Convert.ToDouble(getItems2.Item5);
                                LiteralLp.Text = totalDeductLP.ToString("#,###,###,##0.00") + "(" + DateTime.Now + ")";
                                LiteralAp.Text = ap + "(" + DateTime.Now + ")";
                                LiteralDeductAp.Text = "0";
                                LiteraldeductLp.Text = "-" + getItems2.Item5.ToString("#,###,###,##0.00");
                            }
                            */
                            // Jerry 2024-12-19 Recalculation of Preposted, Deducted and Balance A&P and Loyalty points - END
                        }

                        Lbl_Printedby.Text = GLOBAL.user_id + " - on " + DateTime.Now;
                    }
                    Session["data_passing"] = "";
                }

            }
            catch (Exception ER_QL_00)
            {
                Function_Method.MsgBox("ER_QL_00: " + ER_QL_00.ToString(), this.Page, this);
            }
        }

        private TableCell CreateCell(string text)
        {
            TableCell cell = new TableCell();
            cell.Text = text;
            return cell;
        }

        // Example class to represent the data
        public class ItemData
        {
            public string No { get; set; }
            public string Item { get; set; }
            public int Quantity { get; set; }
            public double Amount { get; set; }
            public string PointsValue { get; set; }
            public string InvoiceNo { get; set; }
            public string InvoiceDate { get; set; }
        }



        private Tuple<string, string, string, string, string, string> get_Company_details(Axapta DynAx, string dataAreaId)
        {
            string Company_Name = ""; string Company_Address = ""; string Company_Tel = ""; string Company_TeleFax = "";
            string Company_OrgId = ""; string Company_OrgIdNew = "";
            int CompanyInfo = 41;
            AxaptaObject axQuery1_7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_7 = (AxaptaObject)axQuery1_7.Call("addDataSource", CompanyInfo);

            var qbr1_7 = (AxaptaObject)axQueryDataSource1_7.Call("addRange", 61448);
            qbr1_7.Call("value", dataAreaId);//dataAreaId
            AxaptaObject axQueryRun1_7 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_7);
            if ((bool)axQueryRun1_7.Call("next"))
            {
                AxaptaRecord DynRec1_7 = (AxaptaRecord)axQueryRun1_7.Call("Get", CompanyInfo);

                Company_Name = DynRec1_7.get_Field("Name").ToString();
                Company_Address = DynRec1_7.get_Field("Address").ToString();

                Company_Tel = DynRec1_7.get_Field("Phone").ToString();
                Company_TeleFax = DynRec1_7.get_Field("TeleFax").ToString();
                Company_OrgId = " (" + DynRec1_7.get_Field("CoRegNum").ToString() + ")";//OrgId

                string temp_Company_OrgIdNew = DynRec1_7.get_Field("OrgId_New").ToString();
                if (temp_Company_OrgIdNew != "")
                {
                    //Company_OrgIdNew = " (" + temp_Company_OrgIdNew + ")";
                    Company_OrgIdNew = " " + temp_Company_OrgIdNew;
                }

                DynRec1_7.Dispose();
            }
            return new Tuple<string, string, string, string, string, string>(Company_Name, Company_Address, Company_Tel, Company_TeleFax, Company_OrgId, Company_OrgIdNew);
        }

        private string[] get_InventDim(Axapta DynAx, int count1, string[] InventDimId)
        {
            string[] InventLocationID = new string[count1];
            int InventDim = 698;
            for (int i = 0; i < count1; i++)
            {
                AxaptaObject axQuery1_6 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource1_6 = (AxaptaObject)axQuery1_6.Call("addDataSource", InventDim);

                var qbr1_5 = (AxaptaObject)axQueryDataSource1_6.Call("addRange", 1);
                qbr1_5.Call("value", InventDimId[i]);//Invoice Id
                AxaptaObject axQueryRun1_6 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_6);
                if ((bool)axQueryRun1_6.Call("next"))
                {
                    AxaptaRecord DynRec1_6 = (AxaptaRecord)axQueryRun1_6.Call("Get", InventDim);
                    InventLocationID[i] = DynRec1_6.get_Field("InventLocationID").ToString();
                    DynRec1_6.Dispose();
                }
            }

            return InventLocationID;
        }

        private string get_InventDim2(Axapta DynAx, string InventDimId)
        {
            string InventLocationID = "";
            int InventDim = 698;
            AxaptaObject axQuery1_13 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_13 = (AxaptaObject)axQuery1_13.Call("addDataSource", InventDim);

            var qbr13 = (AxaptaObject)axQueryDataSource1_13.Call("addRange", 1);
            qbr13.Call("value", InventDimId);//Invoice Id
            AxaptaObject axQueryRun1_13 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_13);
            if ((bool)axQueryRun1_13.Call("next"))
            {
                AxaptaRecord DynRec1_13 = (AxaptaRecord)axQueryRun1_13.Call("Get", InventDim);
                InventLocationID = DynRec1_13.get_Field("InventLocationID").ToString();
                DynRec1_13.Dispose();
            }
            return InventLocationID;
        }

        private Tuple<int, string[], string[], string[], string[], string[], string[], Tuple<double>> get_SalesQuotationLine_details(Axapta DynAx, string QuotationID)
        {
            int count = 0;
            string[] ItemName = new string[50];
            string[] ItemPrice = new string[50];
            string[] InventDimId = new string[50];
            string[] InvoiceQty = new string[50];
            string[] InvoiceAmount = new string[50];//Unit Price x InvoiceQty
            string[] SalesUnit = new string[50];

            List<double> list = new List<double>();
            double subTotal = 0;

            double temp_double_InvoiceAmount = 0; string temp_SalesPrice = "";
            string temp_LineAmount = "";
            double temp_double_ItemPrice = 0; double temp_double_InvoiceQty = 0;

            int SalesQuotationLine = 1962;
            AxaptaObject axQuery1_4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_4 = (AxaptaObject)axQuery1_4.Call("addDataSource", SalesQuotationLine);
            var qbr1_4 = (AxaptaObject)axQueryDataSource1_4.Call("addRange", 2);
            qbr1_4.Call("value", QuotationID);//Invoice Id
            AxaptaObject axQueryRun1_4 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_4);
            while ((bool)axQueryRun1_4.Call("next"))
            {
                AxaptaRecord DynRec1_4 = (AxaptaRecord)axQueryRun1_4.Call("Get", SalesQuotationLine);

                ItemName[count] = DynRec1_4.get_Field("Name").ToString();
                InventDimId[count] = DynRec1_4.get_Field("InventDimId").ToString();
                InvoiceQty[count] = DynRec1_4.get_Field("SalesQty").ToString();
                temp_double_InvoiceQty = Convert.ToDouble(InvoiceQty[count]);

                SalesUnit[count] = DynRec1_4.get_Field("SalesUnit").ToString();
                temp_SalesPrice = DynRec1_4.get_Field("SalesPrice").ToString();

                if (temp_SalesPrice == "" || temp_SalesPrice == "0" || temp_SalesPrice == "0.00")
                {
                    temp_LineAmount = DynRec1_4.get_Field("LineAmount").ToString();//check if SalesPrice is zero
                    ItemPrice[count] = temp_LineAmount;
                }
                else
                {
                    ItemPrice[count] = temp_SalesPrice;
                }
                if (ItemPrice[count] != "")
                {
                    temp_double_ItemPrice = Convert.ToDouble(ItemPrice[count]);
                    ItemPrice[count] = temp_double_ItemPrice.ToString("#,###,###,##0.00");
                }
                temp_double_InvoiceAmount = temp_double_ItemPrice * temp_double_InvoiceQty;
                InvoiceAmount[count] = temp_double_InvoiceAmount.ToString("#,###,###,##0.00");
                list.Add(temp_double_InvoiceAmount);
                subTotal = list.Sum();
                //subTotal[count] = sum;
                /*===================================================================================================*/
                count = count + 1;

                DynRec1_4.Dispose();
            }
            return new Tuple<int, string[], string[], string[], string[], string[], string[], Tuple<double>>
                (count, ItemName, ItemPrice, InvoiceAmount, InventDimId, InvoiceQty, SalesUnit, Tuple.Create(subTotal));
        }

        private Tuple<string, string, string> get_SalesQuotationtable_details(Axapta DynAx, string AccountNo)
        {
            int SalesQuotationTable = 1967;
            AxaptaObject axQuery1_2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_2 = (AxaptaObject)axQuery1_2.Call("addDataSource", SalesQuotationTable);

            var qbr1_2 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 46);//CustAccount
            qbr1_2.Call("value", AccountNo);

            AxaptaObject axQueryRun1_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_2);

            string CustAddress = ""; string CustName = ""; string SalesResponsible = "";

            if ((bool)axQueryRun1_2.Call("next"))
            {
                AxaptaRecord DynRec1_2 = (AxaptaRecord)axQueryRun1_2.Call("Get", SalesQuotationTable);
                CustName = DynRec1_2.get_Field("QuotationName").ToString();
                CustAddress = DynRec1_2.get_Field("DeliveryAddress").ToString();
                SalesResponsible = DynRec1_2.get_Field("SalesResponsible").ToString();
                DynRec1_2.Dispose();
            }
            return new Tuple<string, string, string>(CustName, CustAddress, SalesResponsible);
        }
    }
}