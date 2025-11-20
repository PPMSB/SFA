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
using System.IO;

namespace DotNet
{
    public partial class PDF : System.Web.UI.Page
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


                string strUserAgent = Request.UserAgent.ToString().ToLower();
                HtmlLink htmllnkStyleSheet = new HtmlLink();
                htmllnkStyleSheet = Page.FindControl("lnkStyleSheet") as HtmlLink;
                if (strUserAgent != null)
                {

                    if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("mobile"))
                    {
                        htmllnkStyleSheet.Href = "../Styles/InvoiceDesign_M.css"; b_print.Visible = false;
                    }
                    else
                    {
                        htmllnkStyleSheet.Href = "../Styles/InvoiceDesign.css"; b_print.Visible = true;
                    }
                }
                else
                {
                    htmllnkStyleSheet.Href = "../Styles/InvoiceDesign.css"; b_print.Visible = true;
                }

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
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

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

                            string InvoiceNo = arr_invoice_selected[0];                                                     //sec1: Invoice No.
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

                            string custinvoicejour_refnum = get_CustInvoiceJour_details3(DynAx, InvoiceNo);
                            if (custinvoicejour_refnum == "2")
                            {
                                Label_FormType.Text = "Credit Note";
                                Label7.Visible = false;
                                tblinvoicefooter.Attributes.Add("hidden", "hidden");
                                //table_invoicecompanyname.Attributes.Add("hidden", "hidden");
                                InvoiceForDN.Attributes.Add("style", "display:none");
                                InvoiceForCN.Attributes.Add("style", "display:initial");
                                tblInvoiceNo.Attributes.Add("hidden", "hidden");
                                tblInvoice.Attributes.Add("hidden", "hidden");
                                tblHeaderCustomerInfo.Attributes.Add("style", "display:none");

                                string ParentRecId = get_CustInvoiceTable(DynAx, InvoiceNo);
                                string CNDNPurposeCode = get_WritingInvoice(DynAx, InvoiceNo);
                                var tuple_get_CustInvoiceLine = get_CustInvoiceLine(DynAx, ParentRecId);
                                string Particulars = tuple_get_CustInvoiceLine.Item1;
                                string HeaderParticulars = tuple_get_CustInvoiceLine.Item2;
                                Particulars = DetectNewLine(Particulars);
                                if (HeaderParticulars != "") Label_CN_particulars_header.Text = HeaderParticulars + "<br>" + "<br>";
                                Label_CN_particulars.Text = Particulars;//Particulars;

                                var tuple_get_CustInvoiceJour_details2 = get_CustInvoiceJour_details4(DynAx, InvoiceNo);
                                string NumberSequenceGroup_CustInvoiceJour = tuple_get_CustInvoiceJour_details2.Item1;
                                DataTable dt = new DataTable();
                                dt.Columns.Add(new DataColumn("INVOICE", typeof(string)));
                                dt.Columns.Add(new DataColumn("DATED", typeof(string)));
                                dt.Columns.Add(new DataColumn("AMOUNT (RM)", typeof(string)));
                                GridViewCN.DataSource = dt;
                                GridViewCN.DataBind();
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
                                table_signature.Attributes.Add("hidden", "hidden");
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

                                var tuple_get_CustInvoiceJour_details2 = get_CustInvoiceJour_details2(DynAx, InvoiceNo);
                                string NumberSequenceGroup_CustInvoiceJour = tuple_get_CustInvoiceJour_details2.Item1;


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
                                    dt.Rows.Add(ItemName[p], InventLocationID[p], InvoiceQty[p] + " " + SalesUnit[p], ItemPrice[p], InvoiceAmount[p]);

                                }

                                int row_DiscPercent = 0;
                                int new_row_count = count2;
                                if (LessTotalDiscount != "" && LessTotalDiscount != "0" && LessTotalDiscount != "0.00")
                                {
                                    if (LessTotalDiscount.Substring(0, 1) != "-")
                                    {
                                        LessTotalDiscount = "-" + LessTotalDiscount;//add negative if dint have
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
                                Label_rule.Text = "Any complaint on goods delivered will have to be recorded on this D/O. No claim will be entertained if this D/O is signed with goods receive in good order and condition. Received from " + Company_Name + " ,the above goods in good order and condition.";
                            }

                            if (DueDate != "")
                            {
                                Label_DuePayment.Text = "* This invoice is due for payment on " + DueDate + " *";
                            }

                            if (InvoiceAmountTotal != "")
                            {
                                if (InvoiceAmountTotal.Substring(0, 1) == "-")
                                {
                                    InvoiceAmountTotal = InvoiceAmountTotal.Substring(1);
                                }
                                Label_TotalAmount.Text = "Total" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + InvoiceAmountTotal;
                                Label_CreditNoteAmount.Text = "RM" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + InvoiceAmountTotal;
                                Label_CN_Total.Text = InvoiceAmountTotal;
                                Label_rm.Text = "RINGGIT MALAYSIA: " + NumberToWords.ConvertAmount(Convert.ToDouble(InvoiceAmountTotal));

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
            finally
            {
                DynAx.Logoff();
            }

        }
        private string DetectNewLine(string raw_text)
        {
            string text = "";
            text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");

            return text;
        }
    }
}