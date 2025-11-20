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
    public partial class QuotationLayout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();

            if (!IsPostBack)
            {
                clear_variable();
                Session["flag_temp"] = 0;
                Check_DataRequest();
                Session["data_passing"] = "";//in case forget reset

                //show print button for user to download using mobile
                //string strUserAgent = Request.UserAgent.ToString().ToLower();
                //HtmlLink htmllnkStyleSheet = new HtmlLink();
                //htmllnkStyleSheet = Page.FindControl("lnkStyleSheet") as HtmlLink;
                //if (strUserAgent != null)
                //{

                //    if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("mobile"))
                //    {
                //        htmllnkStyleSheet.Href = "../Styles/InvoiceDesign_M.css"; b_print.Visible = false; /*lnkView.Visible = false;*/
                //    }
                //    else
                //    {
                //        htmllnkStyleSheet.Href = "../Styles/InvoiceDesign.css"; b_print.Visible = true; /*lnkView.Visible = true;*/
                //    }
                //}
                //else
                //{
                //    htmllnkStyleSheet.Href = "../Styles/InvoiceDesign.css"; b_print.Visible = true; /*lnkView.Visible = true; */
                //}

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
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        if (temp1.Substring(0, 6) == "_PAIN@")//Data request from PA> Invoice
                        {

                            //_PAIN@" + InvoiceNo + "|" + CustomerName + "|" + AccountNo + "|" + SalesmanNo + "|" + InvoiceDate + "|" + DueDate;

                            string invoice_selected = temp1.Substring(6);
                            string[] arr_invoice_selected = invoice_selected.Split('|');

                            string QuotationID = arr_invoice_selected[0];                                                     //sec1: Invoice No.
                            string AccountNo = arr_invoice_selected[1];                                                     //sec2: Account No.
                            /*                            string SalesmanNo = arr_invoice_selected[2]; */                                                   //sec2: Salesman No.
                            string DeliveryAddress = arr_invoice_selected[3];                                                   //sec2: Date
                            string InvoiceDate = arr_invoice_selected[4];                                                       //sec3: Due

                            //var tuple_get_CustInvoiceJour_details = get_CustInvoiceJour_details(DynAx, InvoiceNo);
                            //string DelName = tuple_get_CustInvoiceJour_details.Item1;
                            //string DelAddress = tuple_get_CustInvoiceJour_details.Item2;
                            //string SalesId = tuple_get_CustInvoiceJour_details.Item3;                                       //sec2: Sales Order No.   
                            //string Payment = tuple_get_CustInvoiceJour_details.Item4;                                       //sec2: Terms
                            string LF_WithAltAddress = "0";
                            //string LessTotalDiscount = tuple_get_CustInvoiceJour_details.Item6;                             //sec3: Less Total Disc

                            if (LF_WithAltAddress != "1")                                                                     //sec1: Delivery
                            {//same as customer address
                                LF_WithAltAddress = "SAME AS CUSTOMER ADDRESS";
                            }
                            else
                            {
                                //LF_WithAltAddress = DelAddress;
                            }

                            var tuple_get_SalesQuotationtable_details = get_SalesQuotationtable_details(DynAx, AccountNo);
                            string CustAddress = DeliveryAddress;                                  //sec1: Customer Address
                            //string CustTelNo = tuple_get_Custtable_details.Item2; 
                            //sec1: TEL
                            string CustomerName = tuple_get_SalesQuotationtable_details.Item1;                                        //sec1: Name

                            CustAddress = tuple_get_SalesQuotationtable_details.Item2;                                        //sec1: Name
                            string SalesmanNo = tuple_get_SalesQuotationtable_details.Item3;

                            var tuple_get_Salesman_details = Quotation_Get_Enquiries.getEmpDetails(DynAx, SalesmanNo);
                            string SalesmanContact = tuple_get_Salesman_details.Item2;                                    //sec1: HP
                            string SalesmanEmail = tuple_get_Salesman_details.Item3;

                            /*-------------------------------------------------------------------------------------------------------------*/
                            //var tuple_get_SalesQuotationTable = get_SalesQuotationTable(DynAx, InvoiceNo);
                            //string CreatedBy = tuple_get_SalesQuotationTable.Item2;                                                  //sec4: CreatedBy
                            //string OrderNo = tuple_get_SalesQuotationTable.Item3;

                            string selected_company = "ppm";//default first , ppm

                            var tuple_get_Company_details = get_Company_details(DynAx, selected_company);

                            string Company_Name = tuple_get_Company_details.Item1;
                            string Company_Address = tuple_get_Company_details.Item2;
                            string Company_Tel = tuple_get_Company_details.Item3;
                            string Company_TeleFax = tuple_get_Company_details.Item4;
                            string Company_OrgId = tuple_get_Company_details.Item5;
                            string Company_OrgIdNew = tuple_get_Company_details.Item6;
                            //--Section Company name--
                            lblTnc.Text = "This quotation is subject to our standard 'Supply Agreement', Quoted price is subject to change without prior notice." + " <br>" +
                                "Validity: 30Days" + "<br>" +
                                "We hope our offer meets your requirement and we look foward to receiving your confirmed order soon." + "<br>" +
                                "For further enquiries, please do not hesitate to contact me at " + SalesmanContact + " or email at " + SalesmanEmail + " .<br><br>";
                            lblPayment30.Text = "Entitled additional 3% Rebate if payment  made within 30 days of invoiced date.";
                            lblPayment60.Text = "Entitled additional 2% Rebate if payment  made within 60 days of invoiced date.";
                            Label_CustomerName.Text = CustomerName;
                            Label_CustomerAddress.Text = CustAddress;
                            //if (CustCellPhone != "") Label_CustomerHp.Text = "HP : " + CustCellPhone;
                            //if (CustTelNo != "") Label_CustomerTel.Text = "TEL : " + CustTelNo;
                            //if (CustCellPhone != "") lblCustHp.Text = "HP : " + CustCellPhone;
                            //if (CustTelNo != "") lblCustTel.Text = "TEL : " + CustTelNo;

                            Label_InvoiceNo.Text = QuotationID;
                            Label_Date.Text = DateTime.Today.ToString("dd MMMM yyyy");
                            Label_Enquiry.Text = "Thank you for your enquiry, we are pleased to submit our quotation for your kind consideration as below:<br>";

                            InvoiceForDN.Attributes.Add("style", "display:initial");
                            InvoiceForCN.Attributes.Add("style", "display:none");
                            Label_CN_Total.Visible = false;
                            //LabelCN_author.Visible = false;
                            //table_companyname.Attributes.Add("hidden", "hidden");
                            //table_particular.Attributes.Add("hidden", "hidden");

                            //default
                            var tuple_get_SalesQuotation_details = get_SalesQuotationLine_details(DynAx, QuotationID);
                            int count1 = tuple_get_SalesQuotation_details.Item1;
                            string[] ItemName = tuple_get_SalesQuotation_details.Item2;                                 //sec3: Description
                            string[] ItemPrice = tuple_get_SalesQuotation_details.Item3;                                     //sec3: Unit Price
                            string[] InvoiceAmount = tuple_get_SalesQuotation_details.Item4;                                 //sec3: Amount RM
                            string[] InventDimId = tuple_get_SalesQuotation_details.Item5;
                            string[] InvoiceQty = tuple_get_SalesQuotation_details.Item6;                                //sec3: Qty./Vol.
                            string[] SalesUnit = tuple_get_SalesQuotation_details.Item7;                                 //sec3: SalesUnit
                            string[] InventLocationID = get_InventDim(DynAx, count1, InventDimId);                          //sec3: Location
                            string[] InvoiceAmountTotal = tuple_get_SalesQuotation_details.Item4;                            //sec3: Total 
                            //--Section3--
                            DataTable dt = new DataTable();
                            dt.Columns.Add(new DataColumn("Description", typeof(string)));
                            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                            dt.Columns.Add(new DataColumn("Amount RM", typeof(string)));

                            for (int p = 0; p < count1; p++)
                            {
                                var split = ItemName[p].Split(' ');
                                int lastIndex = ItemName[p].LastIndexOf(split.Last());
                                string removeUnit = ItemName[p].Remove(lastIndex);
                                dt.Rows.Add(removeUnit, split.Last(), InvoiceAmount[p]);
                            }

                            int new_row_count = count1;
                            string invoiceAmountTotal = tuple_get_SalesQuotation_details.Rest.Item1.ToString();
                            GridView_FormList.DataSource = dt;
                            GridView_FormList.DataBind();

                            if (invoiceAmountTotal != "")
                            {
                                Label_CN_Total.Text = invoiceAmountTotal;
                            }

                            Label_author.Text = GLOBAL.user_id + "(" + DateTime.Now.ToString("dd/MM/yy|hh:mm:ss") + ")";
                        }
                    }
                    Session["data_passing"] = "";
                }

            }
            catch (Exception ER_QL_00)
            {
                Function_Method.MsgBox("ER_QL_00: " + ER_QL_00.ToString(), this.Page, this);
            }
        }

        private void GridView_RowDataBound_(Axapta DynAx, int count2, string[] is_itemNameBOM, string[] UnitId, string[] TotalBomQty, string[] InventDimId_BOM)
        {
            for (int i = 0; i < count2; i++)
            {
                if (GridView_FormList.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    string temp_Description = GridView_FormList.Rows[i].Cells[0].Text;
                    string temp_Location = GridView_FormList.Rows[i].Cells[1].Text;
                    string temp_QTY = GridView_FormList.Rows[i].Cells[2].Text;
                    string temp_UnitPrice = GridView_FormList.Rows[i].Cells[3].Text;
                    string temp_Amount = GridView_FormList.Rows[i].Cells[4].Text;

                    //if (gs_discount[i] != "" && str_discount[i] != "")
                    //{
                    //    temp_Description = temp_Description + "<br>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Less Discount: " + gs_discount[i];
                    //    temp_Amount = temp_Amount + "<br>" + "-" + str_discount[i];

                    //    temp_Location = temp_Location + "<br>" + "<br>";
                    //    temp_QTY = temp_QTY + "<br>" + "<br>";
                    //    temp_UnitPrice = temp_UnitPrice + "<br>" + "<br>";
                    //}
                    //if (gs_discount_pctg[i] != "" && str_discount_pctg[i] != "")
                    //{
                    //    temp_Description = temp_Description + "<br>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Less Discount Pct: " + gs_discount_pctg[i];
                    //    temp_Amount = temp_Amount + "<br>" + "-" + str_discount_pctg[i];

                    //    temp_Location = temp_Location + "<br>";
                    //    temp_QTY = temp_QTY + "<br>" + "<br>";
                    //    temp_UnitPrice = temp_UnitPrice + "<br>" + "<br>";
                    //}
                    /*-------------------------------------------------------------------------------------------------------------*/
                    //Start of BOM item
                    if (is_itemNameBOM[i] != null)
                    {

                        string[] arr_is_itemNameBOM = is_itemNameBOM[i].Split('|');
                        string[] arr_UnitId = UnitId[i].Split('|');
                        string[] arr_TotalBomQty = TotalBomQty[i].Split('|');
                        string[] arr_InventDimId_BOM = InventDimId_BOM[i].Split('|');
                        //tuning------------------------------------------------------------------------------------------------------
                        int limit_tuning = 55; //default follow pc//max length one line is 55 words. +1 because add one extra space
                        string strUserAgent = Request.UserAgent.ToString().ToLower();
                        if (strUserAgent != null)
                        {
                            if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("mobile"))
                            {
                                limit_tuning = 45;
                            }
                            else// pc
                            {
                                limit_tuning = 55;
                            }
                        }
                        int length_description = temp_Description.Length + 10;//+10= package + spacing
                        int tuning_space_description = (length_description / limit_tuning) + 1;
                        temp_Description = temp_Description + "&nbsp;&nbsp;" + "<u>" + "Package" + "</u>";
                        int test2 = temp_Description.Length;
                        temp_Description = temp_Description + "<br>";
                        temp_Description = temp_Description + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "------------------------------------------";
                        for (int temp_spacing = 0; temp_spacing < tuning_space_description; temp_spacing++)
                        {
                            temp_Location = temp_Location + "<br>";
                            temp_QTY = temp_QTY + "<br>";
                            temp_UnitPrice = temp_UnitPrice + "<br>";
                            temp_Amount = temp_Amount + "<br>";
                        }
                        temp_Location = temp_Location + "-----";
                        temp_QTY = temp_QTY + "-----";
                        //----------------------------------------------------------------------------------------------------------------------------
                        int is_itemNameBOM_count = Convert.ToInt32(arr_is_itemNameBOM[0]);
                        for (int temp_count1 = 0; temp_count1 < is_itemNameBOM_count; temp_count1++)
                        {
                            string BOM_ItemName = arr_is_itemNameBOM[temp_count1 + 1];
                            string BOM_UnitId = arr_UnitId[temp_count1 + 1];
                            string BOM_TotalBomQty = arr_TotalBomQty[temp_count1 + 1];

                            string BOM_InventDimId_BOM = arr_InventDimId_BOM[temp_count1 + 1];
                            string InventLocationID_BOM = get_InventDim2(DynAx, BOM_InventDimId_BOM);

                            temp_Description = temp_Description + "<br>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + BOM_ItemName;
                            temp_Description = temp_Description + "&nbsp;&nbsp;" + "1X" + BOM_TotalBomQty + BOM_UnitId;
                            temp_Location = temp_Location + "<br>" + InventLocationID_BOM;
                            temp_QTY = temp_QTY + "<br>" + BOM_TotalBomQty + " " + BOM_UnitId;

                            temp_Amount = temp_Amount + "<br>";
                            temp_UnitPrice = temp_UnitPrice + "<br>";
                        }
                        //tuning
                        temp_Description = temp_Description + "<br>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "------------------------------------------";
                        temp_Location = temp_Location + "<br>" + "-----";
                        temp_QTY = temp_QTY + "<br>" + "-----";
                        //
                    }
                    //End of BOM item

                    GridView_FormList.Rows[i].Cells[0].Text = temp_Description;
                    GridView_FormList.Rows[i].Cells[1].Text = temp_Location;
                    GridView_FormList.Rows[i].Cells[2].Text = temp_QTY;
                    GridView_FormList.Rows[i].Cells[3].Text = temp_UnitPrice;
                    GridView_FormList.Rows[i].Cells[4].Text = temp_Amount;
                    /*-------------------------------------------------------------------------------------------------------------*/
                }
            }
            //if (row_DiscPercent != 0) //DiscPercent
            //{
            //    string temp = GridView_FormList.Rows[row_DiscPercent].Cells[0].Text;
            //    temp = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + temp;//add tab
            //    GridView_FormList.Rows[row_DiscPercent].Cells[0].Text = temp;
            //}
            //GridView_FormList.Rows[0].Font.Bold = true;

        }

        private void clear_variable()
        {
            //--Section 1--
            Label_CustomerName.Text = "";
            Label_CustomerAddress.Text = "";
            Label_CustomerHp.Text = "";
            Label_CustomerTel.Text = "";
            Label_InvoiceNo.Text = "";
            //Label_DeliveryAddress.Text = "";

            //--Section 3--
            GridView_FormList.DataSource = null;
            GridView_FormList.DataBind();

            Label_author.Text = "";
            Label_rule.Text = "";

            //--
            Label_CN_particulars.Text = "";
            Label_CN_particulars_header.Text = "";

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


        protected void View(object sender, EventArgs e)
        {
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            string pageurl = ResolveUrl("~/PDF/QuotationLayout.pdf");
            //ltEmbed.Text = string.Format(embed, ResolveUrl("~/PDF/QuotationLayout.pdf"));
            //Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl(pageurl)));
            Response.Redirect(pageurl);
        }
    }
}