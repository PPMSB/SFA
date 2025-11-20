using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {

        private void f_Button_Enquiries_Click()
        {
            GridEnquiriesList.DataSource = null;
            GridEnquiriesList.DataBind();
            TextBox_SearchEnquiries.Text = "";
            DropDownList_DayInvoice.ClearSelection();
            f_Dropdown_Salesman();
            GridEnquiriesList.Columns[5].Visible = false;//hide Invoice non button
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
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            GridEnquiriesList.DataSource = null;
            GridEnquiriesList.DataBind();
            GridEnquiriesList.PageIndex = 0;
            gvInvoiceDue.DataSource = null;
            gvInvoiceDue.DataBind();
            gvInvoiceDue.PageIndex = 0;

            if (Session["data_passing"].ToString() != "InvDue")
            {
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
                OverviewEnquiries(0);
                Enquiries_section_general.Visible = true;
            }
            else
            {

                string getEmplId = DropDownList_Salesman.SelectedItem.ToString().Substring(1, 6).Replace(")", "").Trim();

                var getCustAcc = Payment_GET_Overview.getAllCustomer(DynAx, getEmplId);
                List<string> custAccList = getCustAcc.Item1.ToList(); // Convert to List<string>
                OverviewInvoiceDue(0, getEmplId, custAccList);

                Enquiries_section_general.Visible = false;
            }
            DynAx.Logoff();
        }

        private void GenerateInvoiceEnquires(Axapta DynAx)
        {
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
            OverviewEnquiries(0);
            Enquiries_section_general.Visible = true;
        }

        private bool f_Dropdown_Salesman()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            DropDownList_Salesman.Items.Clear();
            List<ListItem> items = new List<ListItem>();
            try
            {
                items = SFA_GET_Enquiries_SalesmanTotal.getSalesman(DynAx);
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
            catch (Exception ER_PA_15)
            {
                Function_Method.MsgBox("ER_PA_15: " + ER_PA_15.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void CheckAccInList2(object sender, EventArgs e)
        {
            string SalesmanId = DropDownList_Salesman.SelectedValue.ToString();
            string DayInvoice = DropDownList_DayInvoice.SelectedValue.ToString();

            Session["data_passing"] = "_PAC2@" + SalesmanId + "|" + DayInvoice;//Payment > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        private void OverviewEnquiries(int PAGE_INDEX)
        {
            GridEnquiriesList.DataSource = null;
            GridEnquiriesList.DataBind();

            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int CustTrans = 78;
                string emplid = "";
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

                //Neil -Request By Renny - Invoice Cant visit, must remove this filter -1/10/2025
                //var qbr_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                //qbr_1.Call("value", "!*_047");

                var qbr_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_2.Call("value", "!*_065");

                var qbr_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_3.Call("value", "!*/CN");

                var qbr_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_4.Call("value", "!*/ERDN");

                var qbr_5 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_5.Call("value", "!*/OR*");

                var qbr_6 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_6.Call("value", "!*/DN");

                var qbr_7 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_7.Call("value", "!*/IDV");

                var qbr_8 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_8.Call("value", "!*/IDN");

                var qbr_9 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_9.Call("value", "!*/FINV");

                var qbr_10 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_10.Call("value", "!*_100");

                var qbr_11 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_11.Call("value", "!*JV_");

                var qbr_12 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_12.Call("value", "!*/ERINV");

                var qbr_13 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_13.Call("value", "!*/ERCN");

                //exclude all so that can get invoice in payment
                string temp_CustAcc = TextBox_SearchEnquiries.Text.Trim();
                if (temp_CustAcc != "")
                {
                    var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//ACCOUNTNUM
                    qbr_0_1.Call("value", temp_CustAcc);
                }

                string temp_SalesId = DropDownList_Salesman.SelectedValue;
                if (temp_SalesId != "")
                {
                    var qbr_0_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 30005);//SALESRESPONSIBLE
                    qbr_0_3.Call("value", temp_SalesId);

                    var getEmpID = Payment_GET_Overview.getAllCustomer(DynAx, temp_SalesId);
                    emplid = getEmpID.Item2.ToString();
                }

                string DayInvoice = DropDownList_DayInvoice.SelectedValue;
                if (DayInvoice.ToString() == "1")// show all invoice
                {
                    DayInvoice = "";
                }

                //Neil Change
                string temp_Invoice = txtInvoiceSearch.Text.Trim();
                if (temp_Invoice != "")
                {
                    int invoice_fieldid = DynAx.GetFieldIdWithLock(CustTrans, "Invoice");
                    var qbr_0_2 = (AxaptaObject)axQueryDataSource.Call("addRange", invoice_fieldid);//INVOICE
                    qbr_0_2.Call("value", "*" + temp_Invoice + "*");
                }

                axQueryDataSource.Call("addSortField", 2, 1);//TransId, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Salesman ID"; N[2] = "Account Name";
                N[3] = "Account No."; N[4] = "Invoice"; N[5] = "Invoice Date";
                N[6] = "Due Date"; N[7] = "Invoice Amount"; N[8] = "Outstanding Amount";
                N[9] = "Current Payment"; N[10] = "Balance Outstanding"; N[11] = "Cheque Date";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }

                DataRow row;
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);

                    string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                    string temp_DueDate = DynRec.get_Field("DueDate").ToString();
                    string TransDate = ""; string DueDate = "";
                    if (temp_TransDate != "")
                    {
                        string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                        string Raw_TransDate = arr_temp_TransDate[0];
                        TransDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_TransDate, true);
                    }
                    if (temp_DueDate != "")
                    {
                        string[] arr_temp_DueDate = temp_DueDate.Split(' ');//date + " " + time;
                        string Raw_DueDate = arr_temp_DueDate[0];
                        DueDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DueDate, true);
                    }

                    string TodayDate = DateTime.Now.ToString("dd/MM/yyyy");//default
                    TodayDate = Function_Method.get_correct_date(GLOBAL.system_checking, TodayDate, false);

                    if (DayInvoice != "")
                    {
                        var var_TodayDate = DateTime.ParseExact(TodayDate, "dd/MM/yyyy", null);
                        var var_TransDate = DateTime.ParseExact(TransDate, "dd/MM/yyyy", null);

                        double diff2 = (var_TodayDate - var_TransDate).TotalDays;
                        int int_DayInvoice = Convert.ToInt32(DayInvoice);
                        if (diff2 > int_DayInvoice)//var_TransDate > var_TodayDate
                        {
                            goto FINISH;//dont ned use subroutine SKIP since trans TransDate arrange in Desc;
                        }
                    }
                    string temp_SalesmanID = DynRec.get_Field("SalesmanId").ToString();
                    string salesmanID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, temp_CustAcc);
                    var getEmpID = Payment_GET_Overview.getAllCustomer(DynAx, salesmanID);
                    if (temp_SalesmanID == "")
                    {
                        temp_SalesmanID = emplid;
                    }

                    if (emplid == "")
                    {
                        temp_SalesmanID = getEmpID.Item2.ToString();
                    }

                    string temp_SalesmanName = EOR_GET_NewApplicant.getEmpName(DynAx, temp_SalesmanID).Trim();

                    string temp_Cust_Acc = DynRec.get_Field("AccountNum").ToString().Trim();
                    string temp_Cust_Name = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, temp_Cust_Acc);

                    string str_AmountCur = DynRec.get_Field("AmountCur").ToString();
                    double double_AmountCur = Convert.ToDouble(str_AmountCur);

                    string SettleAmount = DynRec.get_Field("SettleAmountCur").ToString();

                    double double_SettleAmount = Convert.ToDouble(SettleAmount);
                    double double_Balance = double_AmountCur - double_SettleAmount;
                    if (DayInvoice == "" || DayInvoice == "365")
                    {
                        //do nothing
                    }
                    else
                    {
                        if (double_Balance == 0 || double_Balance == 0.00)
                        {
                            goto SKIP;
                        }
                    }

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;

                        row["Salesman ID"] = "(" + temp_SalesmanID + ") " + temp_SalesmanName;
                        row["Account No."] = temp_Cust_Acc;
                        row["Account Name"] = temp_Cust_Name;

                        string temp_invoice = DynRec.get_Field("Invoice").ToString().Trim();
                        string temp_voucher = DynRec.get_Field("Voucher").ToString().Trim();
                        if (temp_invoice == "")
                        {
                            temp_invoice = temp_voucher;
                        }
                        row["Invoice"] = temp_invoice;
                        row["Invoice Date"] = TransDate;
                        row["Due Date"] = DueDate;
                        row["Invoice Amount"] = double_AmountCur.ToString("#,###,###,##0.00");
                        row["Outstanding Amount"] = double_Balance.ToString("#,###,###,##0.00");

                        string RecordId = DynRec.get_Field("RECID").ToString();
                        var tuple_MarkRecord_MarkRecordA_currbal = Payment_GET_JournalLine_SelectJournal.getMarkStatus(DynAx, RecordId, temp_Cust_Acc, "");
                        //string MarkRecord = tuple_MarkRecord_MarkRecordA_currbal.Item1;
                        //string MarkRecordA = tuple_MarkRecord_MarkRecordA_currbal.Item2;
                        double double_currbal = tuple_MarkRecord_MarkRecordA_currbal.Item3;

                        row["Current Payment"] = double_currbal.ToString("#,###,###,##0.00");

                        double double_OutstandingBalance = 0;
                        if (double_currbal == 0)
                        {
                            double_OutstandingBalance = double_Balance;
                        }
                        else
                        {
                            double_OutstandingBalance = double_Balance - double_currbal;
                        }

                        row["Balance Outstanding"] = double_OutstandingBalance.ToString("#,###,###,##0.00");
                        row["Cheque Date"] = tuple_MarkRecord_MarkRecordA_currbal.Item4;
                        //==========================================================================

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                SKIP:
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridEnquiriesList.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID
                GridEnquiriesList.Columns[11].Visible = false;//hide for temporary

                GridEnquiriesList.DataSource = dt;
                GridEnquiriesList.DataBind();
            }
            catch (Exception ER_PA_08)
            {
                Function_Method.MsgBox("ER_PA_08: " + ER_PA_08.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_Invoice_Click(object sender, EventArgs e)
        {
            string InvoiceNo = ""; string AccountNo = "";//string CustomerName = "";
            string SalesmanNo = ""; string InvoiceDate = ""; string DueDate = "";

            Button Button_Invoice = sender as Button;
            if (Button_Invoice == null) return;

            InvoiceNo = Button_Invoice.Text;
            //get the the id that call the function
            //string ClientID = Button_Invoice.ClientID;
            //string[] arr_ClientID = ClientID.Split('_');
            //int arr_count=arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count-1]);
            //
            GridViewRow row = (GridViewRow)(sender as Button).NamingContainer;

            //int row_count = GridEnquiriesList.Rows.Count;
            //CustomerName = GridEnquiriesList.Rows[ClientRow].Cells[2].Text.Replace("&amp;", "&");
            AccountNo = row.Cells[3].Text;

            string temp_SalesmanNo_Name = row.Cells[1].Text;
            string[] arr_temp_SalesmanNo_Name = temp_SalesmanNo_Name.Split(')');
            SalesmanNo = arr_temp_SalesmanNo_Name[0].Substring(1);

            GridView gridView = (GridView)row.NamingContainer;
            if (gridView.HeaderRow.Cells[5].Text == "Invoice")
            {
                InvoiceDate = row.Cells[6].Text;
                DueDate = row.Cells[7].Text;

            }
            else
            {
            InvoiceDate = row.Cells[5].Text;
            DueDate = row.Cells[6].Text;

            }

            Session["data_passing"] = "_PAIN@" + InvoiceNo + "|" + AccountNo + "|" + SalesmanNo + "|" + InvoiceDate + "|" + DueDate;
            //Response.Redirect("Invoice.aspx");

            string strUserAgent = Request.UserAgent.ToString().ToLower();
            if (strUserAgent != null)
            {
                if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("mobile"))
                {
                    //Function_Method.MsgBox("1  " + strUserAgent, this.Page, this);

                    if (strUserAgent.Contains("iphone"))//default iphone block pop out
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_self')", true);//_blank
                    }
                    else if (strUserAgent.Contains("blackberry") || strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") || strUserAgent.Contains("palm"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
                    }
                }
                else// not mobile
                {//Function_Method.MsgBox("2  " + strUserAgent, this.Page, this);
                    if (strUserAgent.Contains("ipad"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_self')", true);//_blank
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
                    }
                }
            }
            else
            {
                //Function_Method.MsgBox("3  " + strUserAgent, this.Page, this);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
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
                btnGetInvoice.Visible = false;
                Function_Method.MsgBox("Customer Account field must not empty", this.Page, this);
            }
            else
            {
                btnGetInvoice.Visible = true;
            }
        }

        protected void TextBox_SearchEnquiries_Changed(object sender, EventArgs e)
        {
            string temp_CustAcc = TextBox_SearchEnquiries.Text.Trim();
            if (temp_CustAcc == "")
            {
                if (DropDownList_DayInvoice.SelectedItem.Value == "365")//Only 1 Year, add retriction
                {
                    btnGetInvoice.Visible = false;
                    Function_Method.MsgBox("Customer Account field must not empty", this.Page, this);
                    return;
                }
            }
            else
            {
                Axapta DynAx = new Axapta();
                try
                {
                    // Log on to Microsoft Dynamics AX.
                    GLOBAL.Company = GLOBAL.switch_Company;
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    string temp_CustName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, temp_CustAcc);
                    if (temp_CustName == "")
                    {
                        Function_Method.MsgBox("Invalid Customer Account", this.Page, this);
                        btnGetInvoice.Visible = false;
                    }
                    else
                    {
                        btnGetInvoice.Visible = true;
                    }
                }
                catch (Exception ER_PA_16)
                {
                    Function_Method.MsgBox("ER_PA_16: " + ER_PA_16.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Logoff();
                }
            }
        }
    }
}