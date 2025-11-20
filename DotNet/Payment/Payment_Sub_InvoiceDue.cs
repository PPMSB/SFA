using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {
        private void OverviewInvoiceDue(int PAGE_INDEX, string empID, List<string> custAccList)
        {
            gvInvoiceDue.DataSource = null;
            gvInvoiceDue.DataBind();
            int currentYear = DateTime.Now.Year;
            DateTime TodayDate = DateTime.Now;
            DateTime ninetyDaysAgo = TodayDate.AddDays(-90);
            DateTime backDt = TodayDate.AddDays(-3);
            DateTime sevenDaysAfter = TodayDate.AddDays(7);

            Axapta DynAx = new Axapta();
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int CustTrans = 78;

                // Prepare the DataTable with the required columns
                DataTable dt = new DataTable();
                int data_count = 11;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Salesman ID"; N[2] = "Account Name";
                N[3] = "Account No."; N[4] = "Invoice"; N[5] = "Invoice Date";
                N[6] = "Due Date"; N[7] = "Invoice Amount"; N[8] = "Outstanding Amount";
                N[9] = "Current Payment"; N[10] = "Balance Outstanding";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                //int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                int countA = 0;
                foreach (string custAcc in custAccList) // Loop through each customer account
                {
                    if (!string.IsNullOrEmpty(custAcc))
                    {
                        AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

                        // Set the range for the current `custAcc`
                        var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1); // ACCOUNTNUM
                        qbr_0_1.Call("value", custAcc);

                        var qbr_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);
                        qbr_4.Call("value", "*/INV");

                        if (rblInvoice.SelectedValue.ToString() == "0")
                        {
                            var qbr_5 = (AxaptaObject)axQueryDataSource.Call("addRange", 14);//duedate
                            qbr_5.Call("value", ninetyDaysAgo + ".." + TodayDate);
                        }

                        axQueryDataSource.Call("addSortField", 2, 0); // TransId, descending
                        AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                        // Loop through the records for the current `custAcc`
                        while ((bool)axQueryRun.Call("next"))
                        {
                            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);

                            string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                            string temp_DueDate = DynRec.get_Field("DueDate").ToString();
                            string TransDate = temp_TransDate != "" ? DateTime.Parse(temp_TransDate).ToString(GLOBAL.gDisplayDateFormat) : "";
                            string DueDate = temp_DueDate != "" ? DateTime.Parse(temp_DueDate).ToString(GLOBAL.gDisplayDateFormat) : "";
                            string temp_SalesmanID = DynRec.get_Field("SalesmanId").ToString();
                            string temp_Cust_Acc = DynRec.get_Field("AccountNum").ToString().Trim();
                            string temp_Cust_Name = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, temp_Cust_Acc);
                            string temp_invoice = DynRec.get_Field("Invoice").ToString().Trim();
                            string temp_voucher = DynRec.get_Field("Voucher").ToString().Trim();
                            string invoiceNumber = !string.IsNullOrEmpty(temp_invoice) ? temp_invoice : temp_voucher;
                            double double_AmountCur = Convert.ToDouble(DynRec.get_Field("AmountCur").ToString());
                            double double_SettleAmount = Convert.ToDouble(DynRec.get_Field("SettleAmountCur").ToString());

                            var tuple_MarkRecord_MarkRecordA_currbal = Payment_GET_JournalLine_SelectJournal.getMarkStatus(DynAx, DynRec.get_Field("RECID").ToString(), temp_Cust_Acc, "");
                            double double_Balance = double_AmountCur - double_SettleAmount;
                            double double_currbal = tuple_MarkRecord_MarkRecordA_currbal.Item3;

                            string outstandingBalance = (double_Balance - double_currbal).ToString("#,###,###,##0.00");

                            if (rblInvoice.SelectedValue.ToString() == "0" && Convert.ToDouble(outstandingBalance) > 0)//invoice due
                            {
                                countA++;
                                DataRow row = dt.NewRow();
                                row["No."] = countA;
                                row["Salesman ID"] = "(" + empID + ") " + EOR_GET_NewApplicant.getEmpName(DynAx, empID);
                                row["Account No."] = temp_Cust_Acc;
                                row["Account Name"] = temp_Cust_Name;
                                row["Invoice"] = invoiceNumber;
                                row["Invoice Date"] = TransDate;
                                row["Due Date"] = DueDate;
                                row["Invoice Amount"] = double_AmountCur.ToString("#,###,###,##0.00");
                                row["Outstanding Amount"] = double_Balance.ToString("#,###,###,##0.00");
                                row["Current Payment"] = double_currbal.ToString("#,###,###,##0.00");
                                row["Balance Outstanding"] = outstandingBalance;

                                dt.Rows.Add(row);
                            }
                            else if (Convert.ToDouble(outstandingBalance) > 0 && Convert.ToDateTime(temp_DueDate) >= backDt && Convert.ToDateTime(temp_DueDate) <= sevenDaysAfter)// invoice to be due
                            {
                                countA++;
                                DataRow row = dt.NewRow();
                                row["No."] = countA;
                                row["Salesman ID"] = "(" + empID + ") " + EOR_GET_NewApplicant.getEmpName(DynAx, empID);
                                row["Account No."] = temp_Cust_Acc;
                                row["Account Name"] = temp_Cust_Name;
                                row["Invoice"] = invoiceNumber;
                                row["Invoice Date"] = TransDate;
                                row["Due Date"] = DueDate;
                                row["Invoice Amount"] = double_AmountCur.ToString("#,###,###,##0.00");
                                row["Outstanding Amount"] = double_Balance.ToString("#,###,###,##0.00");
                                row["Current Payment"] = double_currbal.ToString("#,###,###,##0.00");
                                row["Balance Outstanding"] = outstandingBalance;

                                dt.Rows.Add(row);
                            }
                        }
                    }
                }

                // Set the VirtualItemCount and bind the DataTable to the GridView
                gvInvoiceDue.VirtualItemCount = countA;
                gvInvoiceDue.DataSource = dt;
                gvInvoiceDue.DataBind();
                DivInvoiceDue.Visible = true;
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

        protected void gvInvoiceDue_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string getEmplId = DropDownList_Salesman.SelectedItem.ToString().Substring(1, 6);

            var getCustAcc = Payment_GET_Overview.getAllCustomer(DynAx, getEmplId);
            List<string> custAccList = getCustAcc.Item1.ToList(); // Convert to List<string>
            OverviewInvoiceDue(e.NewPageIndex, getEmplId, custAccList);
            gvInvoiceDue.PageIndex = e.NewPageIndex;
            gvInvoiceDue.DataBind();
        }
    }
}