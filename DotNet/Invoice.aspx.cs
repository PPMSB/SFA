using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Invoice : System.Web.UI.Page
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
                            string invoice_selected = temp1.Substring(6);
                            string[] arr_invoice_selected = invoice_selected.Split('|');

                            string InvoiceNo = arr_invoice_selected[0];
                            var getuuidLongid = Payment_GET_Overview.getLF_eInvoiceGov(DynAx, InvoiceNo);
                            var getuuidLongidStatus5 = Payment_GET_Overview.getLF_eInvoiceGov_LFStatusIs5(DynAx, InvoiceNo);
                            //16.6.2025 -Status must be "5". Request from Foo Zhi Ming
                            if (getuuidLongid.Item1 != "" && getuuidLongidStatus5.Item1 != "")
                            {
                                string URL = "https://myinvois.hasil.gov.my/" + getuuidLongid.Item1 + "/share/" + getuuidLongid.Item2;
                                try
                                {
                                    string qrCodeImageUrl = Function_Method.GenerateQRCode(URL);
                                    QRCodeImage.ImageUrl = qrCodeImageUrl;
                                }
                                catch (Exception ex)
                                {
                                    Function_Method.MsgBox("ER_IN_CHECKING: " + URL, this.Page, this);
                                }
                            }
                            else
                            {
                                QRCodeImage.Visible = false;
                            }
                            Label_getTinNo.Text = getuuidLongid.Item3;
                            Label_getSstNo.Text = getuuidLongid.Item4;
                            //sec1: Invoice No.
                            string AccountNo = arr_invoice_selected[1];                                                     //sec2: Account No.
                            string SalesmanNo = arr_invoice_selected[2];                                                    //sec2: Salesman No.
                            string InvoiceDate = arr_invoice_selected[3];                                                   //sec2: Date
                            string DueDate = arr_invoice_selected[4];                                                       //sec3: Due

                            var tuple_get_CustInvoiceJour_details = get_CustInvoiceJour_details(DynAx, InvoiceNo);
                            string DelName = tuple_get_CustInvoiceJour_details.Item1;
                            string DelAddress = tuple_get_CustInvoiceJour_details.Item2;
                            string SalesId = tuple_get_CustInvoiceJour_details.Item3;                                       //sec2: Sales Order No.   
                            string Payment = tuple_get_CustInvoiceJour_details.Item4;                                       //sec2: Terms
                            string LF_WithAltAddress = tuple_get_CustInvoiceJour_details.Item5;
                            string LessTotalDiscount = tuple_get_CustInvoiceJour_details.Item6;                             //sec3: Less Total Disc
                            string InvoiceAmountTotal = tuple_get_CustInvoiceJour_details.Item7;                            //sec3: Total 

                            if (InvoiceAmountTotal != "")
                            {
                                // 20251113 Jerry - Show negative value - start
                                //if (InvoiceAmountTotal.Substring(0, 1) == "-")
                                //{
                                //    InvoiceAmountTotal = InvoiceAmountTotal.Substring(1);
                                //}
                                // 20251113 Jerry - Show negative value - end
                            }

                            if (LF_WithAltAddress != "1")                                                                     //sec1: Delivery
                            {//same as customer address
                                LF_WithAltAddress = "SAME AS CUSTOMER ADDRESS";
                            }
                            else
                            {
                                LF_WithAltAddress = DelAddress;
                            }

                            var tuple_get_Custtable_details = get_Custtable_details(DynAx, AccountNo);
                            string CustAddress = tuple_get_Custtable_details.Item1;                                         //sec1: Customer Address
                            string CustTelNo = tuple_get_Custtable_details.Item2;                                           //sec1: TEL
                            string CustCellPhone = tuple_get_Custtable_details.Item3;                                       //sec1: HP
                            string CustomerName = tuple_get_Custtable_details.Item4;                                        //sec1: Name

                            /*-------------------------------------------------------------------------------------------------------------*/
                            var tuple_get_SalesTable = get_SalesTable(DynAx, SalesId);
                            string DiscPercent = tuple_get_SalesTable.Item1;                                                //sec3: Label Less Total Disc
                            string CreatedBy = tuple_get_SalesTable.Item2;                                                  //sec4: CreatedBy
                            string OrderNo = tuple_get_SalesTable.Item3;

                            string selected_company = "ppm";//default first , ppm
                            var tuple_get_Company_details = get_Company_details(DynAx, selected_company);

                            string Company_Name = tuple_get_Company_details.Item1;
                            string Company_Address = tuple_get_Company_details.Item2;
                            string Company_Tel = tuple_get_Company_details.Item3;
                            string Company_TeleFax = tuple_get_Company_details.Item4;
                            string Company_OrgId = tuple_get_Company_details.Item5;
                            string Company_OrgIdNew = tuple_get_Company_details.Item6;
                            //--Section Company name--
                            Label_CompanyName.Text = Company_Name + Company_OrgIdNew + Company_OrgId;
                            Label_CompanyAddress.Text = Company_Address;
                            if (Company_Tel != "") Label_CompanyTel.Text = "Tel : " + Company_Tel;
                            if (Company_TeleFax != "") Label_CompanyTelFax.Text = "Fax : " + Company_TeleFax;
                            Label_POAddress.Text = "P.O. Box 7695, 40724 Shah Alam, Selangor Darul Ehsan, Malaysia";// fixed in Axapta
                            //--Section 1--
                            Label_CustomerName.Text = CustomerName;
                            lblCust.Text = CustomerName;
                            Label_CustomerAddress.Text = CustAddress;
                            lblCusAddress.Text = CustAddress;
                            if (CustCellPhone != "") Label_CustomerHp.Text = "HP : " + CustCellPhone;
                            if (CustTelNo != "") Label_CustomerTel.Text = "TEL : " + CustTelNo;
                            if (CustCellPhone != "") lblCustHp.Text = "HP : " + CustCellPhone;
                            if (CustTelNo != "") lblCustTel.Text = "TEL : " + CustTelNo;
                            lblSalesman.Text = SalesmanNo;

                            Label_InvoiceNo.Text = InvoiceNo;
                            if (InvoiceNo.EndsWith("INV"))
                            {
                                Label_FormType.Text = "DO / INVOICE";
                                Label1.Text = "INVOICE NO: ";
                            }
                            else if (InvoiceNo.EndsWith("DN"))
                            {
                                Label_FormType.Text = "DEBIT NOTE";
                                Label1.Text = "DEBIT NOTE NO: ";
                            }
                            else if (InvoiceNo.EndsWith("CN"))
                            {
                                Label_FormType.Text = "CREDIT NOTE";
                                Label1.Text = "CREDIT NOTE NO: ";
                            }
                            else
                            {
                                // Default case (optional)
                                Label_FormType.Text = "DOCUMENT";
                                Label1.Text = "DOCUMENT NO: ";
                            }
                            Label_DeliveryAddress.Text = LF_WithAltAddress;
                            lblInvoiceNo.Text = InvoiceNo;
                            lblInvoiceDt.Text = InvoiceDate;
                            lblInvoiceAccNo.Text = AccountNo;

                            //--Section 2--
                            Label_AccountNo.Text = AccountNo;
                            Label_OrderNo.Text = OrderNo;
                            Label_SalesOrderNo.Text = SalesId;
                            Label_Terms.Text = Payment;
                            Label_SalesmanNo.Text = SalesmanNo;
                            Label_InvoiceDate.Text = InvoiceDate;
                            //
                            lblLatePayment.Text = "Late payment charges will be levied on overdue account at 1.5% p.m.";
                            lblDiscrepancy.Text = "Any discrepancy regarding this invoice will not be entertained after 7 days from date of this invoice.";
                            var ParentRecId = get_CustInvoiceTable(DynAx, InvoiceNo);//get custinvoicetable msbdebitcredittype
                            var custinvoicejour_cn = get_CustInvoiceJour_details3(DynAx, InvoiceNo);
                            if (ParentRecId.Item2.ToString() == "1")//credit note
                            {
                                lblDiscrepancy.Visible = true;
                                Label_FormType.Text = "Credit Note";
                                //Label7.Visible = false;
                                tblinvoicefooter.Attributes.Add("hidden", "hidden");
                                //table_invoicecompanyname.Attributes.Add("hidden", "hidden");
                                InvoiceForDN.Attributes.Add("style", "display:none");
                                InvoiceForCN.Attributes.Add("style", "display:initial");
                                tblInvoiceNo.Attributes.Add("hidden", "hidden");
                                tblInvoice.Attributes.Add("hidden", "hidden");
                                tblHeaderCustomerInfo.Attributes.Add("style", "display:none");

                                var tuple_get_CustInvoiceLine2 = get_CustInvoiceLine(DynAx, ParentRecId.Item1);
                                string HeaderParticulars = "";

                                string Particulars = tuple_get_CustInvoiceLine2.Item1;
                                HeaderParticulars = tuple_get_CustInvoiceLine2.Item2;

                                var tuple_get_CustInvoiceTrans_details4 = get_CustInvoiceTrans_details4(DynAx, InvoiceNo);
                                string[] Name = tuple_get_CustInvoiceTrans_details4.Item2;
                                string[] Date = tuple_get_CustInvoiceTrans_details4.Item3;
                                string[] InvoiceId = tuple_get_CustInvoiceTrans_details4.Item4;
                                string[] totalUnit = tuple_get_CustInvoiceTrans_details4.Item5;
                                string[] unitId = tuple_get_CustInvoiceTrans_details4.Item6;
                                string[] Amount = tuple_get_CustInvoiceTrans_details4.Item7;
                                string[] LineAmount = tuple_get_CustInvoiceTrans_details4.Rest.Item1;
                                int count = tuple_get_CustInvoiceTrans_details4.Item1;

                                if (string.IsNullOrEmpty(ParentRecId.Item1.ToString()))//to cater no recid
                                {
                                    var get_ParticularHeaderCustRef = get_CustInvoiceJour_details4(DynAx, InvoiceNo);
                                    Label_CN_particulars_header.Text = "BEING GOODS RETURNED - " + get_ParticularHeaderCustRef.Item4;
                                    DataTable dt = new DataTable();
                                    dt.Columns.Add(new DataColumn("Name", typeof(string)));
                                    dt.Columns.Add(new DataColumn("TotalUnit", typeof(string)));
                                    dt.Columns.Add(new DataColumn("UnitId", typeof(string)));
                                    dt.Columns.Add(new DataColumn("SalesPrice", typeof(string)));
                                    dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));

                                    for (int i = 0; i < count; i++)
                                    {
                                        dt.Rows.Add(Name[i], totalUnit[i], unitId[i] + " @ ", "RM " + Amount[i], " = RM " + LineAmount[i]);
                                        lblGetSubTotal.Text = LineAmount[i];
                                    }
                                    GridViewCNReturn.DataSource = dt;
                                    GridViewCNReturn.DataBind();

                                    lblSubTotal.Visible = true;
                                    lblGetSubTotal.Visible = true;

                                    lblGetDiscount.Text = get_ParticularHeaderCustRef.Item5 + ".00 %";
                                    lbldiscount.Visible = true;
                                    lblGetDiscount.Visible = true;

                                    lblAfterDisc.Text = get_ParticularHeaderCustRef.Item6;
                                    lblAfterDisc.Visible = true;
                                    Label_CN_particulars.Visible = false;
                                }
                                else
                                {
                                    string[] InvoiceID2 = tuple_get_CustInvoiceLine2.Item3;
                                    string[] TotalDisc2 = tuple_get_CustInvoiceLine2.Item4;

                                    // normal CN || VPPP || PointContra
                                    if (custinvoicejour_cn.Item2 == "1" || custinvoicejour_cn.Item2 == "5" || custinvoicejour_cn.Item2 == "4")
                                    {
                                        Particulars = DetectNewLine(Particulars);
                                        if (HeaderParticulars != "") Label_CN_particulars.Text = HeaderParticulars + " " + InvoiceID2[0] + "<br>" + "<br>";
                                        string rmaCN = "INVOICE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                            "DATED&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                            "QUANTITY<br>------------&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-----------&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;---------------";
                                        if (Particulars != "")
                                        {
                                            if (Particulars != rmaCN)
                                            {
                                                Label_CN_particulars_header.Text = Particulars;
                                                goto CN;// to remove another particular with INVOICE DATED AMOUNT TOTAL DISCOUNT below  
                                            }
                                            else
                                            {
                                                Label_CN_particulars_header.Text = Label_CN_particulars.Text;
                                                Label_CN_particulars.Text = "";
                                            }
                                        }
                                        else
                                        {
                                            if (LineAmount[0] != "")//Updated for 068831cn with account no. 02204370 3/5/23
                                            {
                                                string particular = noHeaderCN(LineAmount[0], LineAmount[1], LineAmount[2], LineAmount[3], InvoiceAmountTotal);
                                                Label_CN_particulars_header.Text = particular;
                                            }
                                            goto CN;
                                        }//Particulars;
                                    }
                                }

                                if (custinvoicejour_cn.Item1 != "ERCN")
                                {
                                    var tuple_get_CustInvoiceLine = Payment_GET_JournalLine_SelectJournal.get_MultipleCustInvoiceLine(DynAx, ParentRecId.Item1);
                                    if (tuple_get_CustInvoiceLine2.Item7 == "")
                                    {
                                        if (tuple_get_CustInvoiceLine2.Item3[0] != "")
                                        {
                                            HeaderParticulars = tuple_get_CustInvoiceLine2.Item2 + " INVOICE " + tuple_get_CustInvoiceLine2.Item3[0] + " DATED " + InvoiceDate;
                                            Label_CN_particulars_header.Text = HeaderParticulars;
                                        }
                                    }

                                    var tuple_get_CustInvoiceTrans_details3 = get_CustInvoiceTrans_details3(DynAx, InvoiceNo);
                                    string[] ItemId = tuple_get_CustInvoiceTrans_details3.Item2;
                                    if (custinvoicejour_cn.Item2 != "4")//1,2,3
                                    {
                                        if (!string.IsNullOrEmpty(ItemId[0]) && !string.IsNullOrEmpty(ParentRecId.Item1))
                                        {
                                            DataTable dt = new DataTable();
                                            dt.Columns.Add(new DataColumn("INVOICE", typeof(string)));
                                            dt.Columns.Add(new DataColumn("DATED", typeof(string)));
                                            dt.Columns.Add(new DataColumn("AMOUNT", typeof(string)));
                                            dt.Columns.Add(new DataColumn("TOTAL DISCOUNT", typeof(string)));

                                            int count1 = tuple_get_CustInvoiceLine.Item5;
                                            HashSet<string> uniqueInvoiceIDs = new HashSet<string>();

                                            for (int i = 0; i < count1; i++)
                                            {
                                                string InvoiceID = tuple_get_CustInvoiceLine.Item3[i];
                                                string TotalDisc = tuple_get_CustInvoiceLine.Item4[i];
                                                string invoiceDate = ""; string invoiceAmount = "";

                                                if (!uniqueInvoiceIDs.Contains(InvoiceID))
                                                {
                                                    uniqueInvoiceIDs.Add(InvoiceID);
                                                    var tuple_get_CustInvoiceJourAmount = get_Custtrans(DynAx, AccountNo, InvoiceID);
                                                    invoiceAmount = tuple_get_CustInvoiceJourAmount.Item1;
                                                    invoiceDate = tuple_get_CustInvoiceJourAmount.Item2;
                                                    dt.Rows.Add(InvoiceID, invoiceDate, invoiceAmount, TotalDisc + "%");//Updated for rma 069011/CN with account no. 02022091 16/5/23 
                                                }
                                            }

                                            GridViewCN.DataSource = dt;
                                            GridViewCN.DataBind();
                                        }
                                        else
                                        {
                                            var get_ParticularHeaderCustRef = get_CustInvoiceJour_details4(DynAx, InvoiceNo);
                                            Label_CN_particulars_header.Text = "BEING GOODS RETURNED - " + get_ParticularHeaderCustRef.Item4;
                                            //var tuple_get_CustInvoiceTrans_details4 = get_CustInvoiceTrans_details4(DynAx, InvoiceNo);
                                            //string[] Name = tuple_get_CustInvoiceTrans_details4.Item2;
                                            //string[] Date = tuple_get_CustInvoiceTrans_details4.Item3;
                                            //string[] InvoiceId = tuple_get_CustInvoiceTrans_details4.Item4;
                                            //string[] totalUnit = tuple_get_CustInvoiceTrans_details4.Item5;
                                            //string[] unitId = tuple_get_CustInvoiceTrans_details4.Item6;
                                            //string[] Amount = tuple_get_CustInvoiceTrans_details4.Item7;
                                            //string[] LineAmount = tuple_get_CustInvoiceTrans_details4.Rest.Item1;

                                            //int count = tuple_get_CustInvoiceTrans_details4.Item1;
                                            DataTable dt = new DataTable();
                                            dt.Columns.Add(new DataColumn("Name", typeof(string)));
                                            dt.Columns.Add(new DataColumn("TotalUnit", typeof(string)));
                                            dt.Columns.Add(new DataColumn("UnitId", typeof(string)));
                                            dt.Columns.Add(new DataColumn("SalesPrice", typeof(string)));
                                            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));

                                            for (int i = 0; i < count; i++)
                                            {
                                                dt.Rows.Add(Name[i], totalUnit[i], unitId[i] + " @ ", "RM " + Amount[i], " = RM " + LineAmount[i]);
                                            }
                                            GridViewCNReturn.DataSource = dt;
                                            GridViewCNReturn.DataBind();
                                            Label_CN_particulars.Visible = false;
                                        }
                                    }
                                }
                                else
                                {
                                    var tuple_get_CustTrans = get_Custtrans(DynAx, AccountNo, InvoiceNo);
                                    InvoiceAmountTotal = tuple_get_CustTrans.Item1;
                                }
                            CN://use invoice particular header
                                divCreditNote.Attributes.Add("style", "display:initial");
                                Label_TotalAmount.Visible = false;
                                TotalInvoiceAmount.Visible = false;
                            }
                            else
                            {
                                InvoiceForDN.Attributes.Add("style", "display:initial");
                                InvoiceForCN.Attributes.Add("style", "display:none");
                                Label_CN_Total.Visible = false;
                                LabelCN_author.Visible = false;
                                tblCreditNote.Attributes.Add("hidden", "hidden");
                                TotalCreditNoteAmount.Attributes.Add("hidden", "hidden");
                                //table_companyname.Attributes.Add("hidden", "hidden");
                                //table_signature.Attributes.Add("hidden", "hidden");
                                table_particular.Attributes.Add("hidden", "hidden");

                                Label_rm.Visible = false;
                                lblAccNo.Visible = false;
                                lblDate.Visible = false;
                                lblAccNo.Visible = false;
                                //default
                                var tuple_get_CustInvoiceTrans_details1 = get_CustInvoiceTrans_details1(DynAx, InvoiceNo);

                                int count1 = tuple_get_CustInvoiceTrans_details1.Item1;
                                string[] ItemName = tuple_get_CustInvoiceTrans_details1.Item2;                                 //sec3: Description
                                string[] InventDimId = tuple_get_CustInvoiceTrans_details1.Item3;
                                string[] InvoiceQty = tuple_get_CustInvoiceTrans_details1.Item4;                                //sec3: Qty./Vol.
                                string[] SalesUnit = tuple_get_CustInvoiceTrans_details1.Item5;                                 //sec3: SalesUnit
                                string[] InventLocationID = get_InventDim(DynAx, count1, InventDimId);                          //sec3: Location

                                var tuple_get_CustInvoiceTrans_details2 = get_CustInvoiceTrans_details2(DynAx, InvoiceNo);
                                int count2 = tuple_get_CustInvoiceTrans_details1.Item1;
                                string[] ItemPrice = tuple_get_CustInvoiceTrans_details2.Item2;                                     //sec3: Unit Price
                                string[] InvoiceAmount = tuple_get_CustInvoiceTrans_details2.Item3;                                 //sec3: Amount RM
                                string[] str_discount = tuple_get_CustInvoiceTrans_details2.Item4;                                  //sec3: label Less Discount
                                string[] gs_discount = tuple_get_CustInvoiceTrans_details2.Item5;                                   //sec3: label Less Discount
                                string[] str_discount_pctg = tuple_get_CustInvoiceTrans_details2.Item6;                             //sec3: Less Discount Pct
                                string[] gs_discount_pctg = tuple_get_CustInvoiceTrans_details2.Item7;                              //sec3: Less Discount Pct

                                /*-------------------------------------------------------------------------------------------------------------*/

                                //var tuple_get_CustInvoiceJour_details2 = get_CustInvoiceJour_details2(DynAx, InvoiceNo);
                                //string NumberSequenceGroup_CustInvoiceJour = tuple_get_CustInvoiceJour_details2.Item1;

                                var tuple_get_CustInvoiceTrans_details3 = get_CustInvoiceTrans_details3(DynAx, InvoiceNo);
                                int count3 = tuple_get_CustInvoiceTrans_details3.Item1;
                                string[] ItemId = tuple_get_CustInvoiceTrans_details3.Item2;
                                string[] InventTransId = tuple_get_CustInvoiceTrans_details3.Item3;
                                string[] NumberSequenceGroup_CustInvoiceTrans = tuple_get_CustInvoiceTrans_details3.Item4;

                                var tuple_get_SalesLine2 = get_SalesLine2(DynAx, count3, ItemId, SalesId, InventTransId);
                                string[] ItemBOMId = tuple_get_SalesLine2.Item1;

                                var tuple_get_BOMVersion = get_BOMVersion(DynAx, count3, ItemId, ItemBOMId);
                                string[] BOMId = tuple_get_BOMVersion.Item1;

                                var tuple_get_BOM = get_BOM(DynAx, count3, BOMId, InvoiceQty);
                                string[] is_itemNameBOM = tuple_get_BOM.Item1;
                                string[] UnitId = tuple_get_BOM.Item2;
                                string[] TotalBomQty = tuple_get_BOM.Item3;
                                string[] InventDimId_BOM = tuple_get_BOM.Item4;

                                //--Section3--
                                DataTable dt = new DataTable();
                                dt.Columns.Add(new DataColumn("Description", typeof(string)));
                                dt.Columns.Add(new DataColumn("Location", typeof(string)));
                                dt.Columns.Add(new DataColumn("Qty./Vol.", typeof(string)));
                                dt.Columns.Add(new DataColumn("Unit Price", typeof(string)));
                                dt.Columns.Add(new DataColumn("Amount RM", typeof(string)));

                                for (int p = 0; p < count2; p++)
                                {
                                    if (ParentRecId.Item2.ToString() == "0")
                                    {
                                        //var ParentRecId = get_CustInvoiceTable(DynAx, InvoiceNo);
                                        var tuple_get_CustInvoiceLine2 = get_CustInvoiceLine(DynAx, ParentRecId.Item1);
                                        dt.Rows.Add(tuple_get_CustInvoiceLine2.Item2, InventLocationID[p], tuple_get_CustInvoiceLine2.Rest.Item1[p] + " " +
                                            SalesUnit[p], tuple_get_CustInvoiceLine2.Rest.Item2[p].ToString("#,###,###,##0.00"), tuple_get_CustInvoiceLine2.Rest.Item3[p].ToString("#,###,###,##0.00"));
                                    }
                                    else
                                    {
                                        dt.Rows.Add(ItemName[p], InventLocationID[p], InvoiceQty[p] + " " + SalesUnit[p], ItemPrice[p], InvoiceAmount[p]);
                                    }
                                    var getDiscount = get_MarkupTrans(DynAx, tuple_get_CustInvoiceTrans_details1.Item6[p]);
                                    string[] DiscountLabel = getDiscount.Item2;
                                    string[] CalculatedAmount = getDiscount.Item3;
                                    string formattedAmount = CalculatedAmount[p] ?? "0"; // Default to "0" if CalculatedAmount[p] is null

                                    if (getDiscount.Item1 == 0)
                                    {
                                        continue;
                                    }

                                    for (int j = 0; j < getDiscount.Item1; j++)
                                    {
                                        decimal amount = 0;
                                        bool success = false;

                                        // Check if CalculatedAmount[j] is not null before parsing
                                        if (CalculatedAmount[j] != null)
                                        {
                                            try
                                            {
                                                amount = decimal.Parse(CalculatedAmount[j]);
                                                success = true;
                                            }
                                            catch (FormatException)
                                            {
                                                // Handle parsing failure if necessary
                                            }
                                        }

                                        // Format the amount based on whether it's a discount or not
                                        if (success)
                                        {
                                            if (DiscountLabel[j].Contains("Less"))
                                            {
                                                formattedAmount = "-" + amount.ToString("#,###,###,##0.00");
                                            }
                                            else
                                            {
                                                formattedAmount = amount.ToString("#,###,###,##0.00");
                                            }
                                        }
                                        else
                                        {
                                            // If parsing failed, set formattedAmount to "0" or handle as needed
                                            formattedAmount = "0.00"; // or any default value you want to use
                                        }

                                        // Ensure InvoiceQty[p] is valid before parsing
                                        int invoiceQty = 1; // Default value
                                        if (InvoiceQty[p] != null)
                                        {
                                            invoiceQty = Int32.Parse(InvoiceQty[p]);
                                        }

                                        // Add the row to the DataTable
                                        dt.Rows.Add(
                                            DiscountLabel[j] + " @ RM" +
                                            (Double.Parse(formattedAmount.Replace("-", "")) / invoiceQty).ToString("N") +
                                            " per " + SalesUnit[p],
                                            "",
                                            "",
                                            "",
                                            formattedAmount
                                        );
                                    }

                                }

                                //for (int i = 0; i < tuple_get_CustInvoiceTrans_details1.Item1; i++)
                                //{
                                //    var getDiscount = get_MarkupTrans(DynAx, tuple_get_CustInvoiceTrans_details1.Item6[i]);
                                //    string[] DiscountLabel = getDiscount.Item2;
                                //    string[] CalculatedAmount = getDiscount.Item3;

                                //    for (int j = 0; j < getDiscount.Item1; j++)
                                //    {
                                //        string formattedAmount = CalculatedAmount[j];

                                //        if (DiscountLabel[j].Contains("Less"))
                                //        {
                                //            decimal amount = 0;
                                //            bool success = false;

                                //            try
                                //            {
                                //                amount = decimal.Parse(CalculatedAmount[j]);
                                //                success = true;
                                //            }
                                //            catch (FormatException)
                                //            {
                                //                // Handle parsing failure if necessary
                                //            }

                                //            if (success && amount >= 0)
                                //            {
                                //                formattedAmount = "-" + amount.ToString("#,###,###,##0.00");
                                //            }
                                //        }
                                //        else
                                //        {
                                //            decimal amount = 0;
                                //            bool success = false;

                                //            try
                                //            {
                                //                amount = decimal.Parse(CalculatedAmount[j]);
                                //                success = true;
                                //            }
                                //            catch (FormatException)
                                //            {
                                //                // Handle parsing failure if necessary
                                //            }

                                //            if (success)
                                //            {
                                //                formattedAmount = amount.ToString("#,###,###,##0.00");
                                //            }
                                //        }

                                //        dt.Rows.Add(DiscountLabel[j], "", "", "", formattedAmount);
                                //    }
                                //}

                                int row_DiscPercent = 0;
                                int new_row_count = count2;
                                if (LessTotalDiscount != "" && LessTotalDiscount != "0" && LessTotalDiscount != "0.00")
                                {
                                    if (LessTotalDiscount.Substring(0, 1) != "-")
                                    {
                                        
                                        LessTotalDiscount = "-" + LessTotalDiscount;//add negative if dint have
                                        
                                    }
                                    if (InvoiceNo == "80717681/INV") // skip for unique case
                                    {
                                        LessTotalDiscount = LessTotalDiscount.Replace("-", "");
                                    }
                                    dt.Rows.Add("Less Total Discount: " + DiscPercent + " %", "", "", "", LessTotalDiscount);
                                    row_DiscPercent = new_row_count;
                                    new_row_count = new_row_count + 1;
                                }

                                GridView_FormList.DataSource = dt;
                                GridView_FormList.DataBind();

                                //GridView_RowDataBound_(count2, gs_discount, str_discount, gs_discount_pctg, str_discount_pctg, row_DiscPercent);
                                GridView_RowDataBound_(DynAx, count2, gs_discount, str_discount, gs_discount_pctg, str_discount_pctg, row_DiscPercent,
                                    is_itemNameBOM, UnitId, TotalBomQty, InventDimId_BOM);
                                //Label_rule.Text = "Any complaint on goods delivered will have to be recorded on this D/O. No claim will be entertained if this D/O is signed with goods receive in good order and condition. Received from " + Company_Name + " ,the above goods in good order and condition.";
                            }

                            if (DueDate != "")
                            {
                                Label_DuePayment.Text = "* This invoice is due for payment on " + DueDate + " *";
                            }

                            if (InvoiceAmountTotal != "")
                            {
                                Label_TotalAmount.Text = "Total" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + InvoiceAmountTotal;
                                Label_CreditNoteAmount.Text = "RM" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + InvoiceAmountTotal;
                                Label_CN_Total.Text = InvoiceAmountTotal;
                                Label_rm.Text = "RINGGIT MALAYSIA: " + NumberToWords.ConvertAmount(Math.Abs(Convert.ToDouble(InvoiceAmountTotal)));
                            }

                            Label_author.Text = GLOBAL.user_id + "(" + DateTime.Now.ToString("dd/MM/yy|hh:mm:ss") + ")" + "(" + CreatedBy + ")  ";
                            LabelCN_author.Text = GLOBAL.user_id + "(" + DateTime.Now.ToString("dd/MM/yy|hh:mm:ss") + ")" + "(" + CreatedBy + ")  ";
                        }
                    }
                    Session["data_passing"] = "";
                }
            }
            catch (Exception ER_IN_00)
            {
                Function_Method.MsgBox("ER_IN_00: " + ER_IN_00.ToString(), this.Page, this);
            }
        }
        private string DetectNewLine(string raw_text)
        {
            string text = "";
            text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
            return text;
        }

        private void GridView_RowDataBound_(Axapta DynAx, int count2, string[] gs_discount, string[] str_discount, string[] gs_discount_pctg, string[] str_discount_pctg, int row_DiscPercent, string[] is_itemNameBOM, string[] UnitId, string[] TotalBomQty, string[] InventDimId_BOM)
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

                    if (gs_discount[i] != "" && str_discount[i] != "")
                    {
                        temp_Description = temp_Description + "<br>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Less Discount: " + gs_discount[i];
                        temp_Amount = temp_Amount + "<br>" + "-" + str_discount[i];

                        temp_Location = temp_Location + "<br>" + "<br>";
                        temp_QTY = temp_QTY + "<br>" + "<br>";
                        temp_UnitPrice = temp_UnitPrice + "<br>" + "<br>";
                    }
                    if (gs_discount_pctg[i] != "" && str_discount_pctg[i] != "")
                    {
                        temp_Description = temp_Description + "<br>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Less Discount Pct: " + gs_discount_pctg[i];
                        temp_Amount = temp_Amount + "<br>" + "-" + str_discount_pctg[i];

                        temp_Location = temp_Location + "<br>";
                        temp_QTY = temp_QTY + "<br>" + "<br>";
                        temp_UnitPrice = temp_UnitPrice + "<br>" + "<br>";
                    }
                    /*-------------------------------------------------------------------------------------------------------------*/
                    //Start of BOM item
                    if (is_itemNameBOM[i] != null)
                    {
                        string[] arr_is_itemNameBOM = is_itemNameBOM[i].Split('|');
                        string[] arr_UnitId = UnitId[i].Split('|');
                        string[] arr_TotalBomQty = TotalBomQty[i].Split('|');
                        string[] arr_InventDimId_BOM = InventDimId_BOM[i].Split('|');
                        //tuning----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                    }
                    //End of BOM item

                    /*-------------------------------------------------------------------------------------------------------------*/
                    /*-------------------------------------------------------------------------------------------------------------*/
                    GridView_FormList.Rows[i].Cells[0].Text = temp_Description;
                    GridView_FormList.Rows[i].Cells[1].Text = temp_Location;
                    GridView_FormList.Rows[i].Cells[2].Text = temp_QTY;
                    GridView_FormList.Rows[i].Cells[3].Text = temp_UnitPrice;
                    GridView_FormList.Rows[i].Cells[4].Text = temp_Amount;
                    /*-------------------------------------------------------------------------------------------------------------*/
                }
            }
            if (row_DiscPercent != 0) //DiscPercent
            {
                string temp = GridView_FormList.Rows[row_DiscPercent].Cells[0].Text;
                temp = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + temp;//add tab
                GridView_FormList.Rows[row_DiscPercent].Cells[0].Text = temp;
            }
            //GridView_FormList.Rows[0].Font.Bold = true;
        }

        private void clear_variable()
        {
            Label_CompanyName.Text = "";
            Label_CompanyAddress.Text = "";
            Label_POAddress.Text = "";
            //--Section 1--
            Label_CustomerName.Text = "";
            Label_CustomerAddress.Text = "";
            Label_CustomerHp.Text = "";
            Label_CustomerTel.Text = "";
            Label_InvoiceNo.Text = "";
            Label_DeliveryAddress.Text = "";

            //--Section 2--
            Label_AccountNo.Text = "";
            Label_OrderNo.Text = "";
            Label_SalesOrderNo.Text = "";
            Label_Terms.Text = "";
            Label_SalesmanNo.Text = "";
            Label_InvoiceDate.Text = "";

            //--Section 3--
            GridView_FormList.DataSource = null;
            GridView_FormList.DataBind();

            Label_author.Text = "";
            //Label_rule.Text = "";

            Label_DuePayment.Text = "";
            Label_TotalAmount.Text = "";
            //--
            Label_CN_particulars.Text = "";
            Label_CN_particulars_header.Text = "";
        }
        /*-------------------------------------------------------------------------------------------------------------*/

        private Tuple<string[]> get_SalesLine2(Axapta DynAx, int count3, string[] ItemId, string SalesId, string[] InventTransId)
        {
            string[] ItemBOMId = new string[count3];

            int SalesLine = 359;
            for (int q = 0; q < count3; q++)
            {
                AxaptaObject axQuery1_10 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource1_10 = (AxaptaObject)axQuery1_10.Call("addDataSource", SalesLine);

                string test = ItemId[q];
                var qbr1_10 = (AxaptaObject)axQueryDataSource1_10.Call("addRange", 3);
                qbr1_10.Call("value", ItemId[q]);//ItemId

                var qbr1_10_2 = (AxaptaObject)axQueryDataSource1_10.Call("addRange", 1);
                qbr1_10_2.Call("value", SalesId);//SalesId

                var qbr1_10_3 = (AxaptaObject)axQueryDataSource1_10.Call("addRange", 26);
                qbr1_10_3.Call("value", InventTransId[q]);//InventTransId            

                AxaptaObject axQueryRun1_10 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_10);
                if ((bool)axQueryRun1_10.Call("next"))
                {
                    AxaptaRecord DynRec1_10 = (AxaptaRecord)axQueryRun1_10.Call("Get", SalesLine);
                    ItemBOMId[q] = DynRec1_10.get_Field("ItemBOMId").ToString();
                    DynRec1_10.Dispose();
                }
            }
            return new Tuple<string[]>(ItemBOMId);
        }

        /*-------------------------------------------------------------------------------------------------------------*/
        private Tuple<string[]> get_BOMVersion(Axapta DynAx, int count3, string[] ItemId, string[] ItemBOMId)
        {
            string[] BOMId = new string[count3];

            int BOMVersion = 27;
            for (int q = 0; q < count3; q++)
            {
                if (ItemBOMId[q] != null && ItemId[q] != null)
                {
                    AxaptaObject axQuery1_11 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource1_11 = (AxaptaObject)axQuery1_11.Call("addDataSource", BOMVersion);

                    var qbr1_11 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 3);
                    qbr1_11.Call("value", ItemId[q]);//ItemId

                    var qbr1_11_2 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 6);
                    qbr1_11_2.Call("value", "1");//ACTIVE=yes

                    var qbr1_11_3 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 30001);
                    qbr1_11_3.Call("value", "1");//LF_PRINTDETAIL=true

                    var qbr1_11_4 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 7);
                    qbr1_11_4.Call("value", "1");//Approved= yes

                    var qbr1_11_5 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 4);
                    qbr1_11_5.Call("value", ItemBOMId[q]);//BOMId

                    axQueryDataSource1_11.Call("addSortField", 65534, 1);//RecId, descending

                    AxaptaObject axQueryRun1_11 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_11);
                    while ((bool)axQueryRun1_11.Call("next"))
                    {
                        AxaptaRecord DynRec1_11 = (AxaptaRecord)axQueryRun1_11.Call("Get", BOMVersion);
                        BOMId[q] = DynRec1_11.get_Field("BOMId").ToString();
                        //string RecId = DynRec1_11.get_Field("RecId").ToString();
                        DynRec1_11.Dispose();
                    }
                }
            }
            return new Tuple<string[]>(BOMId);
        }

        private Tuple<string[], string[], string[], string[]> get_BOM(Axapta DynAx, int count3, string[] BOMId, string[] InvoiceQty)
        {
            string[] is_itemNameBOM = new string[count3];
            string[] UnitId = new string[count3];
            string[] TotalBomQty = new string[count3];
            string[] InventDImId_BOM = new string[count3];
            int BOM = 18;
            for (int q = 0; q < count3; q++)
            {
                if (BOMId[q] != null)
                {
                    int counter = 0;

                    AxaptaObject axQuery1_12 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource1_12 = (AxaptaObject)axQuery1_12.Call("addDataSource", BOM);

                    var qbr1_12 = (AxaptaObject)axQueryDataSource1_12.Call("addRange", 23);
                    qbr1_12.Call("value", BOMId[q]);//BOMId

                    axQueryDataSource1_12.Call("addSortField", 1, 0);//LineNum, ascending
                    AxaptaObject axQueryRun1_12 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_12);
                    UnitId[q] = ""; is_itemNameBOM[q] = "";
                    while ((bool)axQueryRun1_12.Call("next"))
                    {
                        AxaptaRecord DynRec1_12 = (AxaptaRecord)axQueryRun1_12.Call("Get", BOM);

                        counter = counter + 1;
                        UnitId[q] = UnitId[q] + "|" + DynRec1_12.get_Field("UnitId").ToString();
                        is_itemNameBOM[q] = is_itemNameBOM[q] + "|" + DynRec1_12.get_Field("ItemId").ToString();

                        double temp_double_InvoiceQty = 0;
                        if (InvoiceQty[q] != "") temp_double_InvoiceQty = Convert.ToDouble(InvoiceQty[q]);

                        double temp_double_BOMQty = Convert.ToDouble(DynRec1_12.get_Field("BomQty").ToString());
                        double temp_double_total_BOMQty = temp_double_BOMQty * temp_double_InvoiceQty;
                        string str_TotalBomQty = temp_double_total_BOMQty.ToString();
                        TotalBomQty[q] = TotalBomQty[q] + "|" + str_TotalBomQty;

                        InventDImId_BOM[q] = InventDImId_BOM[q] + "|" + DynRec1_12.get_Field("INVENTDIMID").ToString();

                        DynRec1_12.Dispose();
                    }

                    is_itemNameBOM[q] = counter + is_itemNameBOM[q];
                    UnitId[q] = counter + UnitId[q];
                    TotalBomQty[q] = counter + TotalBomQty[q];

                    InventDImId_BOM[q] = counter + InventDImId_BOM[q];
                }
            }
            return new Tuple<string[], string[], string[], string[]>(is_itemNameBOM, UnitId, TotalBomQty, InventDImId_BOM);
        }

        /*-------------------------------------------------------------------------------------------------------------*/
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
                Company_OrgId = " (" + DynRec1_7.get_Field("LF_CompBRN").ToString() + ")";//OrgId

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

        private Tuple<string, string, string> get_SalesTable(Axapta DynAx, string SalesId)
        {
            string DiscPercent = ""; string CreatedBy = ""; string OrderNo = "";
            int SalesTable = 366;
            AxaptaObject axQuery1_5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_5 = (AxaptaObject)axQuery1_5.Call("addDataSource", SalesTable);
            var qbr1_5 = (AxaptaObject)axQueryDataSource1_5.Call("addRange", 1);
            qbr1_5.Call("value", SalesId);//Invoice Id
            AxaptaObject axQueryRun1_5 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_5);
            if ((bool)axQueryRun1_5.Call("next"))
            {
                AxaptaRecord DynRec1_5 = (AxaptaRecord)axQueryRun1_5.Call("Get", SalesTable);

                DiscPercent = DynRec1_5.get_Field("DiscPercent").ToString();
                double temp_double_DiscPercent = 0;
                if (DiscPercent != "") temp_double_DiscPercent = Convert.ToDouble(DiscPercent);

                DiscPercent = temp_double_DiscPercent.ToString("#,###,###,##0.00");
                CreatedBy = DynRec1_5.get_Field("CreatedBy").ToString();
                OrderNo = DynRec1_5.get_Field("PURCHORDERFORMNUM").ToString();

                DynRec1_5.Dispose();
            }
            return new Tuple<string, string, string>(DiscPercent, CreatedBy, OrderNo);
        }

        private Tuple<int, string[], string[], string[], string[], string[]> get_CustInvoiceTrans_details1(Axapta DynAx, string InvoiceNo)
        {
            int count1 = 0;
            string[] ItemName = new string[50];
            string[] InventDimId = new string[50];
            string[] InvoiceQty = new string[50];
            string[] SalesUnit = new string[50];
            string[] recId = new string[50];
            int CustInvoiceTrans = 64;
            AxaptaObject axQuery1_4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_4 = (AxaptaObject)axQuery1_4.Call("addDataSource", CustInvoiceTrans);
            var qbr1_4 = (AxaptaObject)axQueryDataSource1_4.Call("addRange", 1);
            qbr1_4.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun1_4 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_4);
            while ((bool)axQueryRun1_4.Call("next"))
            {
                AxaptaRecord DynRec1_4 = (AxaptaRecord)axQueryRun1_4.Call("Get", CustInvoiceTrans);

                ItemName[count1] = DynRec1_4.get_Field("Name").ToString();
                InventDimId[count1] = DynRec1_4.get_Field("InventDimId").ToString();
                InvoiceQty[count1] = DynRec1_4.get_Field("Qty").ToString();
                SalesUnit[count1] = DynRec1_4.get_Field("UNITIDTXT").ToString();
                recId[count1] = DynRec1_4.get_Field("RecId").ToString();
                /*===================================================================================================*/
                count1 = count1 + 1;

                DynRec1_4.Dispose();
            }
            return new Tuple<int, string[], string[], string[], string[], string[]>(count1, ItemName, InventDimId, InvoiceQty, SalesUnit, recId);
        }

        private Tuple<int, string[], string[], string[], string[], string[], string[]> get_CustInvoiceTrans_details2(Axapta DynAx, string InvoiceNo)
        {
            int count = 0;

            string[] ItemPrice = new string[50];
            string[] InvoiceQty = new string[50];
            string[] InvoiceAmount = new string[50];//Unit Price x InvoiceQty
            string[] str_discount = new string[50];
            string[] gs_discount = new string[50];
            string[] str_discount_pctg = new string[50];
            string[] gs_discount_pctg = new string[50];

            int CustInvoiceTrans = 64;
            AxaptaObject axQuery1_3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_3 = (AxaptaObject)axQuery1_3.Call("addDataSource", CustInvoiceTrans);
            var qbr1_3 = (AxaptaObject)axQueryDataSource1_3.Call("addRange", 1);
            qbr1_3.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun1_3 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_3);
            while ((bool)axQueryRun1_3.Call("next"))
            {
                AxaptaRecord DynRec1_3 = (AxaptaRecord)axQueryRun1_3.Call("Get", CustInvoiceTrans);

                /*===================================================================================================*/
                double temp_double_ItemPrice = 0; double temp_double_InvoiceQty = 0;
                double temp_double_InvoiceAmount = 0; string temp_SalesPrice = "";
                string temp_LineAmount = "";

                string temp_LineDisc = ""; double temp_double_LineDisc = 0;
                string temp_MultiLnDisc = ""; double temp_double_MultiLnDisc = 0;
                double temp_double_gr_discount = 0;

                string temp_LinePercent = ""; double temp_double_LinePercent = 0;
                string temp_MultiLnPercent = ""; double temp_double_MultiLnPercent = 0;
                double temp_double_gr_discount_pctg = 0; double temp_double_gr_discount_pctg1 = 0;
                /*===================================================================================================*/

                InvoiceQty[count] = DynRec1_3.get_Field("Qty").ToString();
                if (InvoiceQty[count] != "") temp_double_InvoiceQty = Convert.ToDouble(InvoiceQty[count]);

                temp_SalesPrice = DynRec1_3.get_Field("SalesPrice").ToString();

                if (temp_SalesPrice == "" || temp_SalesPrice == "0" || temp_SalesPrice == "0.00")
                {
                    temp_LineAmount = DynRec1_3.get_Field("LineAmount").ToString();//check if SalesPrice is zero
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

                /*===================================================================================================*/
                //Line Discount
                temp_LineDisc = DynRec1_3.get_Field("LineDisc").ToString();
                if (temp_LineDisc != "") temp_double_LineDisc = Convert.ToDouble(temp_LineDisc);

                //MultiLine Discount
                temp_MultiLnDisc = DynRec1_3.get_Field("MultiLnDisc").ToString();
                if (temp_MultiLnDisc != "") temp_double_MultiLnDisc = Convert.ToDouble(temp_MultiLnDisc);

                if (temp_double_LineDisc > 0)
                {
                    temp_double_gr_discount = temp_double_LineDisc * temp_double_InvoiceQty;
                }

                if (temp_double_MultiLnDisc > 0)
                {
                    temp_double_gr_discount = temp_double_gr_discount + (temp_double_MultiLnDisc * temp_double_InvoiceQty);
                }
                if (temp_double_gr_discount != 0)
                {
                    gs_discount[count] = temp_double_LineDisc.ToString("#,###,###,##0.00") + " / " + temp_double_MultiLnDisc.ToString("#,###,###,##0.00");
                    str_discount[count] = temp_double_gr_discount.ToString("#,###,###,##0.00");
                }
                else
                {
                    gs_discount[count] = "";
                    str_discount[count] = "";
                }
                /*===================================================================================================*/
                //line Percentage
                temp_LinePercent = DynRec1_3.get_Field("LinePercent").ToString();
                if (temp_LinePercent != "") temp_double_LinePercent = Convert.ToDouble(temp_LinePercent);
                //MultiLine Percentage
                temp_MultiLnPercent = DynRec1_3.get_Field("MultiLnPercent").ToString();
                if (temp_MultiLnPercent != "") temp_double_MultiLnPercent = Convert.ToDouble(temp_MultiLnPercent);

                //discount amount
                if (temp_double_LinePercent > 0)
                {
                    temp_double_gr_discount_pctg = ((temp_double_ItemPrice * temp_double_InvoiceQty) - temp_double_gr_discount) * (temp_double_LinePercent / 100);
                    temp_double_gr_discount_pctg1 = temp_double_gr_discount_pctg;
                }

                if (temp_double_MultiLnPercent > 0)
                {
                    temp_double_gr_discount_pctg = ((temp_double_ItemPrice * temp_double_InvoiceQty) - temp_double_gr_discount) * (temp_double_MultiLnPercent / 100);
                    temp_double_gr_discount_pctg = temp_double_gr_discount_pctg + temp_double_gr_discount_pctg1;

                }
                if (temp_double_gr_discount_pctg != 0)
                {
                    gs_discount_pctg[count] = temp_double_LinePercent.ToString("#,###,###,##0.00") + " / " + temp_double_MultiLnPercent.ToString("#,###,###,##0.00");
                    str_discount_pctg[count] = temp_double_gr_discount_pctg.ToString("#,###,###,##0.00");
                }
                else
                {
                    gs_discount_pctg[count] = "";
                    str_discount_pctg[count] = "";
                }
                /*===================================================================================================*/
                count = count + 1;
                DynRec1_3.Dispose();
            }

            return new Tuple<int, string[], string[], string[], string[], string[], string[]>(count, ItemPrice, InvoiceAmount, str_discount, gs_discount, str_discount_pctg, gs_discount_pctg);
        }

        private Tuple<int, string[], string[], string[]> get_CustInvoiceTrans_details3(Axapta DynAx, string InvoiceNo)
        {
            int count2 = 0;

            string[] ItemId = new string[50];
            string[] InventTransId = new string[50];
            string[] NumberSequenceGroup = new string[50];
            string[] InvoiceDateRaw = new string[50];

            int CustInvoiceTrans = 64;
            AxaptaObject axQuery1_9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_9 = (AxaptaObject)axQuery1_9.Call("addDataSource", CustInvoiceTrans);
            var qbr1_9 = (AxaptaObject)axQueryDataSource1_9.Call("addRange", 1);
            qbr1_9.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun1_9 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_9);
            while ((bool)axQueryRun1_9.Call("next"))
            {
                AxaptaRecord DynRec1_9 = (AxaptaRecord)axQueryRun1_9.Call("Get", CustInvoiceTrans);

                ItemId[count2] = DynRec1_9.get_Field("InvoiceId").ToString();
                InventTransId[count2] = DynRec1_9.get_Field("InventTransId").ToString();
                NumberSequenceGroup[count2] = DynRec1_9.get_Field("NumberSequenceGroup").ToString();
                InvoiceDateRaw[count2] = DynRec1_9.get_Field("InvoiceDate").ToString();
                /*===================================================================================================*/
                count2 = count2 + 1;

                DynRec1_9.Dispose();
            }
            return new Tuple<int, string[], string[], string[]>
                (count2, ItemId, InventTransId, NumberSequenceGroup);
        }

        private Tuple<int, string[], string[], string[], string[], string[], string[], Tuple<string[]>> get_CustInvoiceTrans_details4(Axapta DynAx, string InvoiceNo)
        {
            int count2 = 0;

            string[] Name = new string[50];
            string[] InvoiceDateRaw = new string[50];
            string[] InvoiceId = new string[50];
            string[] Qty = new string[50];
            string[] UnitId = new string[50];
            string[] SalesPrice = new string[50];
            string[] LineAmount = new string[50];

            int CustInvoiceTrans = 64;
            AxaptaObject axQuery1_9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_9 = (AxaptaObject)axQuery1_9.Call("addDataSource", CustInvoiceTrans);
            var qbr1_9 = (AxaptaObject)axQueryDataSource1_9.Call("addRange", 1);
            qbr1_9.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun1_9 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_9);
            while ((bool)axQueryRun1_9.Call("next"))
            {
                AxaptaRecord DynRec1_9 = (AxaptaRecord)axQueryRun1_9.Call("Get", CustInvoiceTrans);

                Name[count2] = DynRec1_9.get_Field("Name").ToString();
                DateTime dateTime = Convert.ToDateTime(DynRec1_9.get_Field("InvoiceDate").ToString());
                InvoiceDateRaw[count2] = dateTime.ToString("dd/MM/yyyy");
                InvoiceId[count2] = DynRec1_9.get_Field("InvoiceId").ToString();
                Qty[count2] = DynRec1_9.get_Field("Qty").ToString();
                UnitId[count2] = DynRec1_9.get_Field("UnitIDTxt").ToString();

                double salesPrice = Convert.ToDouble(DynRec1_9.get_Field("SalesPrice").ToString());
                SalesPrice[count2] = salesPrice.ToString("#,###,###,##0.00");

                string amount = DynRec1_9.get_Field("LineAmount").ToString();
                amount = amount.Replace("-", "");
                double lineAmount = Convert.ToDouble(amount);

                LineAmount[count2] = lineAmount.ToString("#,###,###,##0.00");
                /*===================================================================================================*/
                count2 = count2 + 1;

                DynRec1_9.Dispose();
            }
            return new Tuple<int, string[], string[], string[], string[], string[], string[], Tuple<string[]>>
                (count2, Name, InvoiceDateRaw, InvoiceId, Qty, UnitId, SalesPrice, Tuple.Create(LineAmount));
        }

        private Tuple<string, string, string, string, string, string, string> get_CustInvoiceJour_details(Axapta DynAx, string InvoiceNo)
        {
            string DelName = "";
            string DelAddress = "";
            string SalesId = "";
            string Payment = "";
            string LF_WithAltAddress = "";
            string LessTotalDiscount = "";
            string InvoiceAmountTotal = "";

            int CustInvoiceJour = 62;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustInvoiceJour);
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 21);
            qbr1.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustInvoiceJour);

                SalesId = DynRec1.get_Field("SALESID").ToString();
                Payment = DynRec1.get_Field("PAYMENT").ToString();
                LF_WithAltAddress = DynRec1.get_Field("LF_WithAltAddress").ToString();//1: got alt address take de
                if (LF_WithAltAddress == "1")
                {
                    DelName = DynRec1.get_Field("DeliveryName").ToString();
                    DelAddress = DynRec1.get_Field("DELIVERYADDRESS").ToString();
                }
                double double_LessTotalDiscount = Convert.ToDouble(DynRec1.get_Field("EndDisc").ToString());
                LessTotalDiscount = double_LessTotalDiscount.ToString("#,###,###,##0.00");

                double double_InvoiceAmountTotal = Convert.ToDouble(DynRec1.get_Field("InvoiceAmount").ToString());
                InvoiceAmountTotal = double_InvoiceAmountTotal.ToString("#,###,###,##0.00");

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string>(DelName, DelAddress, SalesId, Payment, LF_WithAltAddress, LessTotalDiscount, InvoiceAmountTotal);
        }

        private Tuple<string, string> get_CustInvoiceJour_details3(Axapta DynAx, string InvoiceNo)
        {
            string custinvoicejour_refnum = "";
            string invoiceid = "";
            string cntype = "";
            string cn = "";
            int CustInvoiceJour = 62;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", CustInvoiceJour);
            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 21);
            qbr2.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CustInvoiceJour);

                custinvoicejour_refnum = DynRec2.get_Field("RefNum").ToString();
                invoiceid = DynRec2.get_Field("InvoiceId").ToString();
                cntype = DynRec2.get_Field("MSB_CNType").ToString();
                var split = invoiceid.Split('/');
                cn = split[1];

                DynRec2.Dispose();
            }
            return new Tuple<string, string>(cn, cntype);
        }

        private Tuple<string, string, string, string, string, string> get_CustInvoiceJour_details4(Axapta DynAx, string InvoiceAcc)
        {
            string InvoiceId = "";
            string InvoiceDate = "";
            string InvoiceAmount = "";
            string CustomerRef = "";
            string lf_discpercent = "";
            string enddisc = "";

            int CustInvoiceJour = 62;
            AxaptaObject axQuery1_8 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_8 = (AxaptaObject)axQuery1_8.Call("addDataSource", CustInvoiceJour);
            var qbr1_8 = (AxaptaObject)axQueryDataSource1_8.Call("addRange", 21);
            qbr1_8.Call("value", InvoiceAcc);//Invoice Id
            AxaptaObject axQueryRun1_8 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_8);
            if ((bool)axQueryRun1_8.Call("next"))
            {
                AxaptaRecord DynRec1_8 = (AxaptaRecord)axQueryRun1_8.Call("Get", CustInvoiceJour);

                InvoiceId = DynRec1_8.get_Field("InvoiceId").ToString();
                InvoiceDate = Convert.ToDateTime(DynRec1_8.get_Field("InvoiceDate")).ToString("dd/MM/yyyy");
                double double_InvoiceAmountTotal = Convert.ToDouble(DynRec1_8.get_Field("InvoiceAmount").ToString());
                InvoiceAmount = double_InvoiceAmountTotal.ToString("#,###,###,##0.00");
                CustomerRef = DynRec1_8.get_Field("CustomerRef").ToString();
                double doubleDisc = Convert.ToDouble(DynRec1_8.get_Field("enddisc").ToString());
                enddisc = doubleDisc.ToString("#,###,###,##0.00");
                lf_discpercent = DynRec1_8.get_Field("lf_discpercent").ToString();

                DynRec1_8.Dispose();
            }
            return new Tuple<string, string, string, string, string, string>(InvoiceId, InvoiceDate, InvoiceAmount, CustomerRef, lf_discpercent, enddisc);
        }

        private Tuple<string, string, string, string> get_Custtable_details(Axapta DynAx, string AccountNo)
        {
            int CustTable = 77;
            AxaptaObject axQuery1_2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_2 = (AxaptaObject)axQuery1_2.Call("addDataSource", CustTable);
            var qbr1_2 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 1);//CustAccount
            qbr1_2.Call("value", AccountNo);
            AxaptaObject axQueryRun1_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_2);

            string CustAddress = ""; string CustTelNo = ""; string CustCellPhone = ""; string CustomerName = "";

            if ((bool)axQueryRun1_2.Call("next"))
            {
                AxaptaRecord DynRec1_2 = (AxaptaRecord)axQueryRun1_2.Call("Get", CustTable);
                CustAddress = DynRec1_2.get_Field("ADDRESS").ToString();
                CustTelNo = DynRec1_2.get_Field("Phone").ToString();
                CustCellPhone = DynRec1_2.get_Field("CellularPhone").ToString();
                CustomerName = DynRec1_2.get_Field("Name").ToString();
                DynRec1_2.Dispose();
            }
            return new Tuple<string, string, string, string>(CustAddress, CustTelNo, CustCellPhone, CustomerName);
        }

        private Tuple<string, string> get_CustInvoiceTable(Axapta DynAx, string InvoiceNo)
        {
            string ParentRecId = "";
            string debitCreditType = "";

            int CustInvoiceTable = 1209;
            AxaptaObject axQuery1_12 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_12 = (AxaptaObject)axQuery1_12.Call("addDataSource", CustInvoiceTable);

            var qbr1_12 = (AxaptaObject)axQueryDataSource1_12.Call("addRange", 32);
            qbr1_12.Call("value", InvoiceNo);//InvoiceNo

            AxaptaObject axQueryRun1_12 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_12);
            if ((bool)axQueryRun1_12.Call("next"))
            {
                AxaptaRecord DynRec1_12 = (AxaptaRecord)axQueryRun1_12.Call("Get", CustInvoiceTable);
                ParentRecId = DynRec1_12.get_Field("RecId").ToString();
                debitCreditType = DynRec1_12.get_Field("DebitCreditType").ToString();

                DynRec1_12.Dispose();
            }
            return new Tuple<string, string>(ParentRecId, debitCreditType);
        }

        private Tuple<string, string, string[], string[], int, string, string, Tuple<double[], double[], double[]>> get_CustInvoiceLine(Axapta DynAx, string ParentRecId)
        {
            int count = 0;
            string Particulars = "";
            string HeaderParticulars = "";
            string reason = "";
            string[] LF_InvoiceId = new string[50];
            string[] LF_TotalDiscount = new string[50];
            string LF_CustInvoiceTransRecId = "";
            double[] lf_Qty = new double[10];
            double[] AmountCur = new double[10];
            double[] totalAmount = new double[10];

            int CustInvoiceLine = 63;

            AxaptaObject axQuery1_11 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_11 = (AxaptaObject)axQuery1_11.Call("addDataSource", CustInvoiceLine);

            axQueryDataSource1_11.Call("addSortField", 2, 0);//LineNum, ascending
            //to avoid getting empty header and particular 3/5/23

            var qbr1_11 = (AxaptaObject)axQueryDataSource1_11.Call("addRange", 1);
            qbr1_11.Call("value", ParentRecId);//ParentRecId

            AxaptaObject axQueryRun1_11 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_11);
            if ((bool)axQueryRun1_11.Call("next"))
            {
                AxaptaRecord DynRec1_11 = (AxaptaRecord)axQueryRun1_11.Call("Get", CustInvoiceLine);
                Particulars = DynRec1_11.get_Field("Particulars").ToString();
                HeaderParticulars = DynRec1_11.get_Field("HeaderParticulars").ToString();
                LF_InvoiceId[count] = DynRec1_11.get_Field("LF_InvoiceID").ToString();
                LF_TotalDiscount[count] = Convert.ToDouble(DynRec1_11.get_Field("LF_TotalDisc")).ToString("f1");
                LF_CustInvoiceTransRecId = DynRec1_11.get_Field("LF_CustInvoiceTransRecId").ToString();
                reason = DynRec1_11.get_Field("CN_Reason").ToString();
                lf_Qty[count] = Convert.ToDouble(DynRec1_11.get_Field("LF_Qty"));
                AmountCur[count] = Convert.ToDouble(DynRec1_11.get_Field("AmountCur"));

                totalAmount[count] = lf_Qty[count] * AmountCur[count];
                if (totalAmount[count] == 0)
                {
                    totalAmount[count] = AmountCur[count];
                }
                DynRec1_11.Dispose();
                count = count + 1;
            }
            return new Tuple<string, string, string[], string[], int, string, string, Tuple<double[], double[], double[]>>
                (Particulars, HeaderParticulars, LF_InvoiceId, LF_TotalDiscount, count, LF_CustInvoiceTransRecId, reason, Tuple.Create(lf_Qty, AmountCur, totalAmount));
        }

        private Tuple<string, string> get_Custtrans(Axapta DynAx, string AccountNo, string InvoiceNo)
        {
            int CustTrans = 78;
            AxaptaObject axQuery1_2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_2 = (AxaptaObject)axQuery1_2.Call("addDataSource", CustTrans);

            var qbr1_2 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 1);//CustAccount
            qbr1_2.Call("value", AccountNo);

            var qbr1_3 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 4);//Invoice
            qbr1_3.Call("value", InvoiceNo);

            AxaptaObject axQueryRun1_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_2);

            string AmountCur = ""; string TransDate = "";

            if ((bool)axQueryRun1_2.Call("next"))
            {
                AxaptaRecord DynRec1_2 = (AxaptaRecord)axQueryRun1_2.Call("Get", CustTrans);
                AmountCur = Convert.ToDouble(DynRec1_2.get_Field("AmountCur")).ToString("#,###,###,##0.00");
                TransDate = Convert.ToDateTime(DynRec1_2.get_Field("TransDate")).ToString("dd/MM/yyyy");
                DynRec1_2.Dispose();
            }
            return new Tuple<string, string>(AmountCur, TransDate);
        }

        private string noHeaderCN(string a, string b, string c, string d, string total)
        {
            string CNmanually = "";
            if (!string.IsNullOrEmpty(d))
            {
                CNmanually = "<br>A&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + a + "\r\n" +
                    "<br>B&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + b + "\r\n" +
                    "<br>C&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + c + "\r\n" +
                    "<br>D&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + d + "\r\n" +
                    "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--------------------\r\n" +
                    "<br>TOTAL&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;" + total + "\r\n" +
                    "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--------------------";
            }
            else
            {
                CNmanually = "<br>A&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + a + "\r\n" +
                    "<br>B&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + b + "\r\n" +
                    "<br>C&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + c + "\r\n" +
                    "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--------------------\r\n" +
                    "<br>TOTAL&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;RM&nbsp;&nbsp;&nbsp;" + total + "\r\n" +
                    "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--------------------";
            }
            return CNmanually;
        }

        class NumberToWords
        {
            public static String ConvertAmount(double amount)
            {
                try
                {
                    Int64 amount_int = (Int64)amount;
                    Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                    if (amount_dec == 0)
                    {
                        return Convert(amount_int) + " Only.";
                    }
                    else
                    {
                        return Convert(amount_int) + " and " + Convert(amount_dec) + " Cents Only.";
                    }
                }
                catch (Exception)
                {
                    // TODO: handle exception  
                }
                return "";
            }

            public static String Convert(Int64 i)
            {
                String[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                    "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                String[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (i < 20)
                {
                    return units[i];
                }
                if (i < 100)
                {
                    return tens[i / 10] + ((i % 10 > 0) ? " " + Convert(i % 10) : "");
                }
                if (i < 1000)
                {
                    return units[i / 100] + " Hundred"
                            + ((i % 100 > 0) ? " " + Convert(i % 100) : "");
                }
                if (i < 100000)
                {
                    return Convert(i / 1000) + " Thousand "
                    + ((i % 1000 > 0) ? " " + Convert(i % 1000) : "");
                }
                if (i < 10000000)
                {
                    return Convert(i / 100000) + " Hundred Thousand "
                            + ((i % 100000 > 0) ? " " + Convert(i % 100000) : "");
                }
                if (i < 1000000000)
                {
                    return Convert(i / 10000000) + " Million "
                            + ((i % 10000000 > 0) ? " " + Convert(i % 10000000) : "");
                }
                return Convert(i / 1000000000) + " Billion "
                        + ((i % 1000000000 > 0) ? " " + Convert(i % 1000000000) : "");
            }
        }

        private Tuple<int, string[], string[]> get_MarkupTrans(Axapta DynAx, string recId)
        {

            int count1 = 0;
            string[] Txt = new string[50];
            string[] CalculatedAmount = new string[50];

            int MarkupTrans = 230;

            try
            {
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", MarkupTrans);
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 2);
                qbr.Call("value", recId);//Rec Id

                var transtableid = DynAx.GetFieldId(MarkupTrans, "TransTableId");
                var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", transtableid);
                var hardCodeValue = "64";
                qbr2.Call("value", hardCodeValue);


                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", MarkupTrans);

                    Txt[count1] = DynRec.get_Field("Txt").ToString();
                    CalculatedAmount[count1] = DynRec.get_Field("CalculatedAmount").ToString();
                    count1 = count1 + 1;

                    DynRec.Dispose();
                }
            }
            catch (Exception ex)
            {
                return new Tuple<int, string[], string[]>(count1, Txt, CalculatedAmount);
            }
            return new Tuple<int, string[], string[]>(count1, Txt, CalculatedAmount);
        }
    }
}