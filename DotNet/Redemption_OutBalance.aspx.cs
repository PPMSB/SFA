using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Redemption_OutBalance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();

            if (!IsPostBack)
            {
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                    null, CustomerMasterTag2,
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

                Session["data_passing"] = "";//in case forget reset
                Session["flag_temp"] = 0;
                Check_DataRequest();
            }
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
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

        private void Check_DataRequest()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string temp1 = GLOBAL.data_passing.ToString();
                if (temp1 != "")//data receive
                {
                    reloading_data(DynAx, temp1);
                    Session["data_passing"] = "";
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemption_section'); ", true);
                }
            }
            catch (Exception ER_EO_22)
            {
                Function_Method.MsgBox("ER_RD_22: " + ER_EO_22.Message, this.Page, this);
                return;
            }
        }

        private void reloading_data(Axapta DynAx, string custAcc)
        {
            var getRedemp = Redemption_Get_Details.get_rd_CreditInfo(DynAx, custAcc);
            var getRedemp2 = Redemption_Get_Details.get_rd_CreditInfo2(DynAx, custAcc);
            var custAging = Redemption_Get_Details.get_CustAging(DynAx, custAcc);
            var getEOR = Redemption_Get_Details.getEORdetails(DynAx, custAcc);

            lblSalesmanName.Text = getRedemp.Item7;
            lblCustName.Text = getRedemp.Item1;
            lblTelNo.Text = getRedemp.Item2;
            txtGetCreditLimit.Text = "RM " + getRedemp2.Item1;
            LabelCustClass.Text = EOR_GET_NewApplicant.getClassDetails(DynAx, getRedemp.Item3);
            Label_CustType.Text = Redemption_Get_Details.getCustomerType(DynAx, getRedemp.Item4);
            lblGetIncorporationDate.Text = getRedemp.Item5;
            lblGetAccOpenDt.Text = getRedemp.Item6;
            Label_CustGroup.Text = Redemption_Get_Details.getCustomerMainGroup(DynAx, getRedemp2.Item2);

            Label_EORTarget.Text = getEOR.Item1;
            lblGetTargetCarton.Text = getEOR.Item2;
            Label_Expiry.Text = getEOR.Item3;
            lblGetCommence.Text = getEOR.Item4;

            lblGetAveragePayDay.Text = Convert.ToDouble(custAging.Item5.ToString("#,###,###,##0.00")) + "(" + Convert.ToDouble(custAging.Item6.ToString("#,###,###,##0.00")) + ")";
            lblGetAverageMonthlySales.Text = custAging.Item7.Item1.ToString("#,###,###,##0.00");
            lblGetApprovalCredit.Text = custAging.Item7.Item4.ToString();
            lblGetBankGuarantee.Text = custAging.Item1;
            lblGetBankExpiry.Text = custAging.Item2;
            lblGetChequeTotal.Text = custAging.Item7.Item3.ToString();
            lblGetLastCheque.Text = custAging.Item7.Item2;
            lblGetOverdue.Text = custAging.Item7.Item5.ToString();
            lblGetLastMonth.Text = custAging.Item3;
            lblGetCurrentMonth.Text = custAging.Item4;

            OverviewCustOutstanding(0, custAcc);
            //hdRedemID.Value = redemption_Id;
            //Label_Receiver_Name.Text = tuple_reload.Item3;

            //string temp_Delivery_Addr = tuple_reload.Item4;
            //if (temp_Delivery_Addr == Label_Address.Text)
            //{
            //    Label_Delivery_Addr.Text = "-same as primary address-";
            //}
            //else
            //{
            //    Label_Delivery_Addr.Text = temp_Delivery_Addr;
            //}
            //TextBox_Notes.Text = tuple_reload.Item5;

            //var tuple_reload_set2 = SFA_GET_SALES_ORDER.reload_from_SalesTableSet2(DynAx, SO_Id);
            //hidden_Street.Text = tuple_reload_set2.Item1;
            //hidden_ZipCode.Text = tuple_reload_set2.Item2;
            //hidden_City.Text = tuple_reload_set2.Item3;
            //hidden_State.Text = tuple_reload_set2.Item4;
            //hidden_Country.Text = tuple_reload_set2.Item5;

            //Alt_Addr_function();
        }

        private void OverviewCustOutstanding(int PAGE_INDEX, string temp_CustAcc)
        {
            gvCustCredit.DataSource = null;
            gvCustCredit.DataBind();

            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int CustTrans = 78;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

                var qbr_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_1.Call("value", "*_047");

                var qbr_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_2.Call("value", "*_065");

                var qbr_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_3.Call("value", "*/CN");

                var qbr_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_4.Call("value", "*/ERDN");

                var qbr_5 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_5.Call("value", "*/OR");

                var qbr_6 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_6.Call("value", "*/DN");

                var qbr_7 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_7.Call("value", "*/IDV");

                var qbr_8 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_8.Call("value", "*/IDN");

                var qbr_9 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_9.Call("value", "*/FINV");

                var qbr_10 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_10.Call("value", "*_100");

                var qbr_11 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_11.Call("value", "*JV_");

                var qbr_12 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_12.Call("value", "*/ERINV");

                //
                if (temp_CustAcc != "")
                {
                    var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//ACCOUNTNUM
                    qbr_0_1.Call("value", temp_CustAcc);
                }

                axQueryDataSource.Call("addSortField", 2, 1);//TransId, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================

                DataTable dt = new DataTable();
                int data_count = 8;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Voucher"; N[2] = "Invoice No.";
                N[3] = "Date"; N[4] = "Amount Currency"; N[5] = "Currency"; N[6] = "Due Date";
                N[7] = "Total Outstanding Day";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);
                    //==========================================================================
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

                    //==========================================================================
                    //for extra filter purpose since CustTrans do not have Axapta control authorization

                    string temp_Cust_Acc = DynRec.get_Field("AccountNum").ToString().Trim();
                    string temp_Cust_Name = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, temp_Cust_Acc);
                    //==========================================================================

                    string str_AmountCur = DynRec.get_Field("AmountCur").ToString();
                    int amountMST = Convert.ToInt32(DynRec.get_Field("AmountMST"));
                    int settleAmountMst = Convert.ToInt32(DynRec.get_Field("SettleAmountMST"));
                    double double_AmountCur = Convert.ToDouble(str_AmountCur);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();
                        if (amountMST - settleAmountMst != 0)
                        {
                            Debug.WriteLine(amountMST + settleAmountMst);
                            row["No."] = countA;

                            row["Voucher"] = DynRec.get_Field("Voucher").ToString().Trim();
                            row["Invoice No."] = DynRec.get_Field("Invoice").ToString().Trim();
                            row["Date"] = TransDate;
                            row["Amount Currency"] = double_AmountCur;
                            row["Currency"] = DynRec.get_Field("CurrencyCode").ToString();
                            row["Due Date"] = DueDate;

                            var var_TransDate = DateTime.ParseExact(TransDate, "dd/MM/yyyy", null);

                            var today = DateTime.Now.ToString("dd/MM/yyyy");
                            var var_DueDate = DateTime.ParseExact(DueDate, "dd/MM/yyyy", null);

                            double diff90 = (Convert.ToDateTime(today) - var_DueDate).TotalDays;
                            if (diff90 > 90)
                            {
                                Function_Method.MsgBox("Warning! There no Active transaction over the last 3 months for this Customer Account Number", this.Page, this);
                                return;
                            }

                            double diff2 = (var_TransDate - var_DueDate).TotalDays;

                            row["Total Outstanding Day"] = diff2;
                            if (diff2 > 60)
                            {
                                lblDays.Text = diff2.ToString() + " days";
                            }
                            dt.Rows.Add(row);
                        }
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                gvCustCredit.VirtualItemCount = countA;
                //Data-Binding with our GRID

                gvCustCredit.DataSource = dt;
                gvCustCredit.DataBind();
            }
            catch (Exception ER_RO_01)
            {
                Function_Method.MsgBox("ER_RO_01: " + ER_RO_01.ToString(), this.Page, this);
            }
        }

        protected void gvCustCredit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            OverviewCustOutstanding(e.NewPageIndex, GLOBAL.data_passing.ToString());
            gvCustCredit.PageIndex = e.NewPageIndex;
            gvCustCredit.DataBind();
        }
    }
}