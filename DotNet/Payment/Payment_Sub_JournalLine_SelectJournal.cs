using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {
        private void SelectInvoice()
        {
            JournalLine_section_general.Visible = false; AddLine_section.Visible = false; SelectInvoice_section.Visible = true;

            Initialize_SelectInvoice();
        }

        protected void Button_Cancel_SelectInvoice_Click(object sender, EventArgs e)
        {
            JournalLine_section_general.Visible = false; AddLine_section.Visible = true; SelectInvoice_section.Visible = false;
        }

        private void Initialize_SelectInvoice()
        {
            //reset first
            GridView_SelectInvoice.DataSource = null;
            GridView_SelectInvoice.DataBind();
            //
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                GridView_SelectInvoice.Columns[10].Visible = true;
                GridView_SelectInvoice.Columns[11].Visible = true;
                GridView_SelectInvoice.Columns[12].Visible = true;
                GridView_SelectInvoice.Columns[13].Visible = true;

                int CustTrans = 78;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);
                //==========================================================================================================
                string CustomerAcc = hidden_TextBox_CustomerAcc.Text;
                string JournalNum = hidden_Label_Journal_No.Text;
                //==========================================================================================================
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//Customer Acc Number
                qbr.Call("value", CustomerAcc);

                var qbr_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_2.Call("value", "!*_065");

                var qbr_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_3.Call("value", "!*/CN");

                var qbr_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_4.Call("value", "!*/ERDN");

                var qbr_5 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_5.Call("value", "!*/OR");

                var qbr_6 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_6.Call("value", "!*/DN");

                var qbr_7 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_7.Call("value", "!*/IDV");

                var qbr_8 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_8.Call("value", "!*/IDN");

                var qbr_10 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_10.Call("value", "!*_100");

                var qbr_11 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_11.Call("value", "!*JV_");

                var qbr_12 = (AxaptaObject)axQueryDataSource.Call("addRange", 3);
                qbr_12.Call("value", "!*/ERINV");
                //==========================================================================================================
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                DataTable dt = Header_Transfer_GridView_SelectInvoice();

                while ((bool)axQueryRun.Call("next"))//use if only one record
                {
                    //======================================================================================

                    bool Disable_CheckBox_FullPayment = false;
                    bool Disable_TextBox_PartialPayment = false;
                    bool Disable_CheckBox_InUse = false; bool Tick_CheckBox_InUse = false;

                    string str_invoice = "";
                    string str_voucher = "";
                    string TransDate = "";
                    string DueDate = "";
                    //======================================================================================
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);
                    //======================================================================================
                    string RecordId = DynRec.get_Field("RECID").ToString();
                    double double_AmountCur = Convert.ToDouble(DynRec.get_Field("AmountCur"));

                    string SettleAmount = DynRec.get_Field("SettleAmountCur").ToString();
                    /*
                    if(SettleAmount.Substring(0,1)=="-")
                    {
                        SettleAmount=SettleAmount.Substring(1);//Remove negative
                    }
                    */
                    double double_SettleAmount = Convert.ToDouble(SettleAmount);
                    double RoundUp_SettleAmount = Math.Round(double_SettleAmount, 2); //round up 2 digit

                    double double_Balance = double_AmountCur - double_SettleAmount;
                    //RoundUp_Balance = Math.Round(Balance, 2); //round up 2decimal
                    string str_RoundUp_Balance = double_Balance.ToString("#,###,###,##0.00");

                    var tuple_MarkRecord_MarkRecordA_currbal = Payment_GET_JournalLine_SelectJournal.getMarkStatus(DynAx, RecordId, CustomerAcc, JournalNum);
                    string MarkRecord = tuple_MarkRecord_MarkRecordA_currbal.Item1;
                    string MarkRecordA = tuple_MarkRecord_MarkRecordA_currbal.Item2;
                    double currbal = tuple_MarkRecord_MarkRecordA_currbal.Item3;
                    double RoundUp_currbal = Math.Round(currbal, 2); //round up 2 digit

                    if (RoundUp_currbal != 0 || RoundUp_currbal != 0.00)
                    {
                        str_invoice = DynRec.get_Field("Invoice").ToString();
                        str_voucher = DynRec.get_Field("Voucher").ToString();
                        if (str_invoice == "")
                        {
                            str_invoice = str_voucher;
                        }

                        string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                        string temp_DueDate = DynRec.get_Field("DueDate").ToString();

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
                        //======================================================================================    
                        if (MarkRecord == "In Use")
                        {
                            Disable_CheckBox_FullPayment = true;
                            Disable_CheckBox_InUse = true; Tick_CheckBox_InUse = true;
                        }
                        else
                        {
                            Disable_CheckBox_InUse = true; Tick_CheckBox_InUse = false;

                            if (str_invoice.Substring(0, 2) == "JV")
                            {
                                Disable_CheckBox_FullPayment = true;
                            }
                            else
                            {
                                Disable_CheckBox_FullPayment = false;
                            }
                        }
                        //======================================================================================
                        double TotalBalance = double_Balance - currbal;
                        string str_TotalBalance = TotalBalance.ToString();

                        if (str_TotalBalance == "0" || str_TotalBalance == "0.00" || str_TotalBalance == "0.0")
                        {
                            Disable_TextBox_PartialPayment = true;
                        }
                        else
                        {
                            if (str_invoice.Substring(0, 2) == "JV")
                            {
                                Disable_TextBox_PartialPayment = true;
                            }
                            else
                            {
                                int length_str_invoice = str_invoice.Length;
                                if (str_invoice.Substring(length_str_invoice - 4) == "ERDN")
                                {
                                    Disable_TextBox_PartialPayment = true;
                                }
                                else
                                {
                                    Disable_TextBox_PartialPayment = false;
                                }
                            }
                        }
                        string CustTransRecId = Payment_GET_JournalLine_SelectJournal.get_RecIds(DynAx, RecordId);
                        string str_RoundUp_AmountCur = double_AmountCur.ToString("#,###,###,##0.00");
                        string str_currbal = currbal.ToString("#,###,###,##0.00");
                        //======================================================================================
                        //======================================================================================
                        //Transfer to Grid only when Balance is not zero

                        dt = Transfer_GridView_SelectInvoice(dt,
                        Disable_CheckBox_FullPayment, Disable_TextBox_PartialPayment, Disable_CheckBox_InUse, Tick_CheckBox_InUse,
                        str_invoice, str_RoundUp_AmountCur, str_RoundUp_Balance, str_currbal,
                        TransDate, DueDate, CustTransRecId
                        );
                        //======================================================================================
                    }
                }
                GridView_SelectInvoice.DataSource = dt;
                GridView_SelectInvoice.DataBind();
                CheckVisiblity_GridView_SelectInvoice();

                GridView_SelectInvoice.Columns[10].Visible = false;//Hidden_Disable_CheckBox_FullPayment
                GridView_SelectInvoice.Columns[11].Visible = false;//Hidden_Disable_TextBox_PartialPayment
                GridView_SelectInvoice.Columns[12].Visible = false;//Hidden_Disable_CheckBox_InUse	
                GridView_SelectInvoice.Columns[13].Visible = false;//Hidden_Tick_CheckBox_InUse
            }
            catch (Exception ER_PA_05)
            {
                Function_Method.MsgBox("ER_PA_05: " + ER_PA_05.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private DataTable Transfer_GridView_SelectInvoice(
            DataTable dt,
            bool Disable_CheckBox_FullPayment, bool Disable_TextBox_PartialPayment, bool Disable_CheckBox_InUse, bool Tick_CheckBox_InUse,
            string str_invoice, string str_RoundUp_AmountCur, string str_RoundUp_Balance, string str_currbal,
            string TransDate, string DueDate, string CustTransRecId)
        {
            DataRow row;
            row = dt.NewRow();
            row["Invoice"] = str_invoice;
            row["Invoice Amount"] = str_RoundUp_AmountCur;
            row["Balance Settlement"] = str_RoundUp_Balance;
            row["In Payment"] = str_currbal;
            row["Date"] = TransDate;
            row["Due Date"] = DueDate;
            row["CustToRecId"] = CustTransRecId;

            row["Hidden_Disable_CheckBox_FullPayment"] = Disable_CheckBox_FullPayment.ToString();
            row["Hidden_Disable_TextBox_PartialPayment"] = Disable_TextBox_PartialPayment.ToString();
            row["Hidden_Disable_CheckBox_InUse"] = Disable_CheckBox_InUse.ToString();
            row["Hidden_Tick_CheckBox_InUse"] = Tick_CheckBox_InUse.ToString();

            dt.Rows.Add(row);

            return dt;
        }

        private void CheckVisiblity_GridView_SelectInvoice()
        {

            foreach (GridViewRow row in GridView_SelectInvoice.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string Hidden_Disable_CheckBox_FullPayment = row.Cells[10].Text;
                    CheckBox C_CheckBox_FullPayment = (row.Cells[0].FindControl("CheckBox_FullPayment") as CheckBox);
                    if (Hidden_Disable_CheckBox_FullPayment == "True")
                    {
                        C_CheckBox_FullPayment.Enabled = false;
                        C_CheckBox_FullPayment.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    }
                    else
                    {
                        C_CheckBox_FullPayment.Enabled = true;
                        C_CheckBox_FullPayment.Attributes.Add("style", "background-color:#45A29E");
                    }

                    string Hidden_Disable_TextBox_PartialPayment = row.Cells[11].Text;
                    TextBox T_TextBox_Partial_PaymentQTY = (row.Cells[1].FindControl("TextBox_Partial_PaymentQTY") as TextBox);
                    if (Hidden_Disable_TextBox_PartialPayment == "True")
                    {
                        T_TextBox_Partial_PaymentQTY.Enabled = false;
                        //T_TextBox_Partial_PaymentQTY.Visible = false;
                        T_TextBox_Partial_PaymentQTY.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    }
                    else
                    {
                        T_TextBox_Partial_PaymentQTY.Enabled = true;

                        T_TextBox_Partial_PaymentQTY.Attributes.Add("style", "background-color: ");
                    }

                    string Hidden_Tick_CheckBox_InUse = row.Cells[13].Text;
                    string Hidden_Disable_CheckBox_InUse = row.Cells[12].Text;
                    CheckBox C_CheckBox_InUse = (row.Cells[2].FindControl("CheckBox_InUse") as CheckBox);

                    if (Hidden_Tick_CheckBox_InUse == "True")
                    {
                        C_CheckBox_InUse.Checked = true;
                    }
                    else
                    {
                        C_CheckBox_InUse.Checked = false;
                    }

                    if (Hidden_Disable_CheckBox_InUse == "True")
                    {
                        C_CheckBox_InUse.Enabled = false;
                        C_CheckBox_InUse.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    }
                    else
                    {
                        C_CheckBox_InUse.Enabled = true;
                        C_CheckBox_InUse.Attributes.Add("style", "background-color:#45A29E");
                    }
                }
            }
        }

        private DataTable Header_Transfer_GridView_SelectInvoice()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Invoice", typeof(string)));
            dt.Columns.Add(new DataColumn("Invoice Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Balance Settlement", typeof(string)));
            dt.Columns.Add(new DataColumn("In Payment", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Due Date", typeof(string)));
            dt.Columns.Add(new DataColumn("CustToRecId", typeof(string)));

            dt.Columns.Add(new DataColumn("Hidden_Disable_CheckBox_FullPayment", typeof(string)));
            dt.Columns.Add(new DataColumn("Hidden_Disable_TextBox_PartialPayment", typeof(string)));
            dt.Columns.Add(new DataColumn("Hidden_Disable_CheckBox_InUse", typeof(string)));
            dt.Columns.Add(new DataColumn("Hidden_Tick_CheckBox_InUse", typeof(string)));
            return dt;
        }

    }
}