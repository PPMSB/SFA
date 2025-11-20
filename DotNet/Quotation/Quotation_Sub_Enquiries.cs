using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SalesQuotation : System.Web.UI.Page
    {
        Axapta DynAx = Function_Method.GlobalAxapta();
        private void f_Button_Enquiries_Click()
        {
            Enquiries_section_general.Visible = false;
            GridEnquiriesList.DataSource = null;
            GridEnquiriesList.DataBind();
            TextBox_SearchEnquiries.Text = "";
            DropDownList_DayInvoice.ClearSelection();
            f_Dropdown_Salesman();
            //GridEnquiriesList.Columns[5].Visible = false;//hide Invoice non button
            string temp1 = Session["flag_temp"].ToString();
            if (temp1.Length >= 6)//correct size
            {
                if (temp1.Substring(0, 6) == "@PAC2_")
                {
                    string raw_data = temp1.Substring(6);
                    string[] arr_raw_data = raw_data.Split('|');
                    string SalesmanId = arr_raw_data[0];
                    string DayInvoice = arr_raw_data[1];
                    string selected_Account = arr_raw_data[2];

                    DropDownList_Salesman.SelectedValue = SalesmanId;
                    DropDownList_DayInvoice.SelectedValue = DayInvoice;
                    TextBox_SearchEnquiries.Text = selected_Account;

                    Session["flag_temp"] = 0;
                }
            }
        }

        protected void CheckEnquiries(object sender, EventArgs e)
        {
            GridEnquiriesList.DataSource = null;
            GridEnquiriesList.DataBind();
            GridEnquiriesList.PageIndex = 0;

            string temp_CustAcc = TextBox_SearchEnquiries.Text.Trim();
            if (temp_CustAcc != "")
            {//check validity
                string CustName = Payment_GET_Overview.CustName(DynAx, temp_CustAcc);
                if (CustName == "")
                {
                    TextBox_SearchEnquiries.Text = "";
                    Function_Method.MsgBox("Wrong Customer Account Number", this.Page, this);
                }
            }
            OverviewEnquiries(0); Enquiries_section_general.Visible = true; //Button_Enquiries_accordion.Text = "Customer Invoices";
        }

        private bool f_Dropdown_Salesman()
        {
            DropDownList_Salesman.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            items = Quotation_Get_Enquiries.getSalesman(DynAx);
            if (items.Count > 1)
            {
                DropDownList_Salesman.Items.AddRange(items.ToArray());
                return true;
            }
            else
            {
                Function_Method.MsgBox("There is no Salesman available.", this.Page, this);
                return false;
            }
        }

        protected void CheckAccInList2(object sender, EventArgs e)
        {

            string SalesmanId = DropDownList_Salesman.SelectedValue.ToString();
            string DayInvoice = DropDownList_DayInvoice.SelectedValue.ToString();

            Session["data_passing"] = "_SQCM@" + SalesmanId + "|" + DayInvoice;//Quotation > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }


        private void OverviewEnquiries(int PAGE_INDEX)
        {
            GridEnquiriesList.DataSource = null;
            GridEnquiriesList.DataBind();

            try
            {
                int SalesQuotationTable = 1967;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesQuotationTable);

                string temp_CustAcc = TextBox_SearchEnquiries.Text.Trim();
                if (temp_CustAcc != "")
                {
                    var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 46);//CustAccount
                    qbr_0_1.Call("value", temp_CustAcc);
                }
                string temp_SalesId = DropDownList_Salesman.SelectedValue;
                if (temp_SalesId != "")
                {
                    var qbr_0_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 21);//SALESRESPONSIBLE
                    qbr_0_2.Call("value", temp_SalesId);
                }
                string DayInvoice = DropDownList_DayInvoice.SelectedValue;
                //
                axQueryDataSource.Call("addSortField", 2, 1);//TransId, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================

                DataTable dt = new DataTable();
                int data_count = 6;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Quotation ID"; N[2] = "Shipping Date Requested";
                N[3] = "Account No."; N[4] = "Account Name"; N[5] = "Delivery Address";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.


                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesQuotationTable);
                    //==========================================================================
                    //string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                    //string temp_DueDate = DynRec.get_Field("DueDate").ToString();
                    //string TransDate = ""; string DueDate = "";
                    //if (temp_TransDate != "")
                    //{
                    //    string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                    //    string Raw_TransDate = arr_temp_TransDate[0];
                    //    TransDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_TransDate, true);
                    //}
                    //if (temp_DueDate != "")
                    //{
                    //    string[] arr_temp_DueDate = temp_DueDate.Split(' ');//date + " " + time;
                    //    string Raw_DueDate = arr_temp_DueDate[0];
                    //    DueDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DueDate, true);
                    //}

                    //string TodayDate = DateTime.Now.ToString("dd/MM/yyyy");//default
                    //TodayDate = Function_Method.get_correct_date(GLOBAL.system_checking, TodayDate, false);

                    //if (DayInvoice != "")
                    //{

                    //    var var_TodayDate = DateTime.ParseExact(TodayDate, "dd/MM/yyyy", null);
                    //    var var_TransDate = DateTime.ParseExact(TransDate, "dd/MM/yyyy", null);

                    //    double diff2 = (var_TodayDate - var_TransDate).TotalDays;
                    //    int int_DayInvoice = Convert.ToInt32(DayInvoice);
                    //    if (diff2 > int_DayInvoice)//var_TransDate > var_TodayDate
                    //    {
                    //        goto FINISH;//dont ned use subroutine SKIP since trans TransDate arrange in Desc;
                    //    }
                    //}


                    //==========================================================================
                    //for extra filter purpose since CustTrans do not have Axapta control authorization
                    //string temp_SalesmanID = DynRec.get_Field("SalesResponsible").ToString();
                    //string temp_SalesmanName = EOR_GET_NewApplicant.getEmpName(DynAx, temp_SalesmanID).Trim();

                    string temp_Cust_Acc = DynRec.get_Field("CustAccount").ToString().Trim();
                    string temp_Cust_Name = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, temp_Cust_Acc);
                    string temp_ShippingDate = DynRec.get_Field("ShippingDateRequested").ToString();

                    //if (temp_SalesmanName == "" || temp_Cust_Name == "")
                    //{
                    //    goto SKIP;
                    //}
                    //==========================================================================

                    //string str_AmountCur = DynRec.get_Field("AmountCur").ToString();

                    //double double_AmountCur = Convert.ToDouble(str_AmountCur);

                    //string SettleAmount = DynRec.get_Field("SettleAmountCur").ToString();

                    //double double_SettleAmount = Convert.ToDouble(SettleAmount);
                    //double double_Balance = double_AmountCur - double_SettleAmount;
                    //if (DayInvoice == "" || DayInvoice == "365")
                    //{
                    //    //do nothing
                    //}
                    //else
                    //{
                    //    if (double_Balance == 0 || double_Balance == 0.00)
                    //    {
                    //        goto SKIP;
                    //    }
                    //}

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        row["Quotation ID"] = DynRec.get_Field("QuotationId").ToString();
                        string[] arr_temp_ShippingDate = temp_ShippingDate.Split(' ');//date + " " + time;
                        string Raw_ShippingDate = arr_temp_ShippingDate[0];
                        row["Shipping Date Requested"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_ShippingDate, true);

                        row["Account No."] = temp_Cust_Acc;
                        row["Account Name"] = temp_Cust_Name;
                        row["Delivery Address"] = DynRec.get_Field("DeliveryAddress").ToString();
                        //string temp_invoice = DynRec.get_Field("Invoice").ToString().Trim();
                        //string temp_voucher = DynRec.get_Field("Voucher").ToString().Trim();
                        //if (temp_invoice == "")
                        //{
                        //    temp_invoice = temp_voucher;
                        //}
                        //row["Invoice"] = temp_invoice;

                        //row["Invoice Date"] = TransDate;
                        //row["Due Date"] = DueDate;
                        //==========================================================================

                        //row["Invoice Amount"] = double_AmountCur.ToString("#,###,###,##0.00");
                        //row["Outstanding Amount"] = double_Balance.ToString("#,###,###,##0.00");

                        string RecordId = DynRec.get_Field("RECID").ToString();
                        //var tuple_MarkRecord_MarkRecordA_currbal = Payment_GET_JournalLine_SelectJournal.getMarkStatus(DynAx, RecordId, temp_Cust_Acc, "");
                        //string MarkRecord = tuple_MarkRecord_MarkRecordA_currbal.Item1;
                        //string MarkRecordA = tuple_MarkRecord_MarkRecordA_currbal.Item2;
                        //double double_currbal = tuple_MarkRecord_MarkRecordA_currbal.Item3;
                        //row["Current Payment"] = double_currbal.ToString("#,###,###,##0.00");

                        //double double_OutstandingBalance = 0;
                        //if (double_currbal == 0)
                        //{
                        //    double_OutstandingBalance = double_Balance;
                        //}r
                        //else
                        //{
                        //    double_OutstandingBalance = double_Balance - double_currbal;
                        //}

                        //row["Balance "] = double_OutstandingBalance.ToString("#,###,###,##0.00");

                        //==========================================================================

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    //SKIP:
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }

                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridEnquiriesList.VirtualItemCount = countA;
                //Data-Binding with our GRID

                GridEnquiriesList.DataSource = dt;
                GridEnquiriesList.DataBind();
            }
            catch (Exception ER_PA_14)
            {
                Function_Method.MsgBox("ER_PA_14: " + ER_PA_14.ToString(), this.Page, this);
            }
        }

        protected void Button_Quotation_Click(object sender, EventArgs e)
        {
            string QuotationNo = hidden_Label_SQ_No.Text; string AccountNo = "";//string CustomerName = "";
            string SalesmanNo = ""; string ShippingDate = ""; string DeliveryAddress = "";

            //Button Button_Quotation = sender as Button;
            //if (Button_Quotation == null) return;
            var tuple_QuotationDetail = Quotation_Get_Sales_Quotation.reload_from_SalesQuotationTable(DynAx, QuotationNo);
            AccountNo = tuple_QuotationDetail.Item2;
            SalesmanNo = tuple_QuotationDetail.Item6;
            ShippingDate = tuple_QuotationDetail.Item3;
            DeliveryAddress = tuple_QuotationDetail.Item4;
            //QuotationNo = Button_Quotation.Text;
            //get the the id that call the function
            //string ClientID = Button_Invoice.ClientID;
            //GridViewRow gvr = Button_Quotation.Parent.Parent as GridViewRow;
            //int accNo = Convert.ToInt32(GridEnquiriesList.DataKeys[gvr.RowIndex].Values[0]);
            //GridViewRow row = (GridViewRow)Button_Quotation.NamingContainer;

            //string accno = GridViewOverviewList.DataKeys[row.RowIndex].Values["Customer Account"].ToString();
            //AccountNo = accno.ToString();

            //string[] arr_ClientID = ClientID.Split('_');
            //int arr_count=arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count-1]);
            //
            //int row_count = GridEnquiriesList.Rows.Count;
            //CustomerName = GridEnquiriesList.Rows[ClientRow].Cells[2].Text.Replace("&amp;", "&");

            //string temp_SalesmanNo_Name = GridEnquiriesList.Rows[3].Cells[1].Text;
            //string[] arr_temp_SalesmanNo_Name = temp_SalesmanNo_Name.Split(')');
            //SalesmanNo = arr_temp_SalesmanNo_Name[0].Substring(1);

            //ShippingDate = row.Cells[2].Text;
            //DeliveryAddress = row.Cells[5].Text; ;

            Session["data_passing"] = "_PAIN@" + QuotationNo + "|" + AccountNo + "|" + SalesmanNo + "|" + DeliveryAddress + "|" + ShippingDate;
            //Response.Redirect("Invoice.aspx");

            string strUserAgent = Request.UserAgent.ToString().ToLower();
            if (strUserAgent != null)
            {
                if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("mobile"))
                {
                    if (strUserAgent.Contains("iphone"))//default iphone block pop out
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('QuotationLayout.aspx','_self')", true);//_blank
                    }
                    else if (strUserAgent.Contains("blackberry") || strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") || strUserAgent.Contains("palm"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('QuotationLayout.aspx','_newtab')", true);//_blank
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('QuotationLayout.aspx','_newtab')", true);//_blank
                    }
                }
                else// not mobile
                {//Function_Method.MsgBox("2  " + strUserAgent, this.Page, this);
                    if (strUserAgent.Contains("ipad"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('QuotationLayout.aspx','_self')", true);//_blank
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('QuotationLayout.aspx','_newtab')", true);//_blank
                    }
                }
            }
            else
            {
                //Function_Method.MsgBox("3  " + strUserAgent, this.Page, this);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('QuotationLayout.aspx','_newtab')", true);//_blank
            }
        }

        protected void datagrid_PageIndexChanging_enquiries(object sender, GridViewPageEventArgs e)
        {
            OverviewEnquiries(e.NewPageIndex);
            GridEnquiriesList.PageIndex = e.NewPageIndex;
            GridEnquiriesList.DataBind();
        }

        protected void OnSelectedIndexChanged_DropDownList_DayInvoice(object sender, EventArgs e)
        {
            if (DropDownList_DayInvoice.SelectedItem.Value == "365" && TextBox_SearchEnquiries.Text.Trim() == "")//Only 1 Year, add retriction
            {
                ImageButton5.Visible = false;
                Function_Method.MsgBox("Customer Account field must not empty", this.Page, this);
            }
            else
            {
                ImageButton5.Visible = true;
            }
        }

        protected void TextBox_SearchEnquiries_Changed(object sender, EventArgs e)
        {
            string temp_CustAcc = TextBox_SearchEnquiries.Text.Trim();
            if (temp_CustAcc == "")
            {
                if (DropDownList_DayInvoice.SelectedItem.Value == "365")//Only 1 Year, add retriction
                {
                    ImageButton5.Visible = false;
                    Function_Method.MsgBox("Customer Account field must not empty", this.Page, this);
                    return;
                }
            }
            else
            {
                try
                {
                    string temp_CustName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, temp_CustAcc);
                    if (temp_CustName == "")
                    {
                        Function_Method.MsgBox("Invalid Customer Account", this.Page, this);
                        ImageButton5.Visible = false;
                    }
                    else
                    {
                        ImageButton5.Visible = true;
                    }
                }
                catch (Exception ER_PA_16)
                {
                    Function_Method.MsgBox("ER_PA_16: " + ER_PA_16.ToString(), this.Page, this);
                }
            }
        }
    }
}