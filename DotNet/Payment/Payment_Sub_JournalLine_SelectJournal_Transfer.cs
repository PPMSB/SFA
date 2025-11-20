using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Button_Transfer_SelectInvoice_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string error = preprocess(DynAx);
                if (error != "")
                {
                    Function_Method.MsgBox(error, this.Page, this);
                    return;
                }
                DynAx.TTSBegin();
                //=========================================================================================================================================
                //from GridView_SelectInvoice
                var tuple_get_JournalPosted_JournalCCReceived = GetDataFromGridView_SelectInvoice();
                int arr_Counter = tuple_get_JournalPosted_JournalCCReceived.Item1;
                string[] arr_Payment = tuple_get_JournalPosted_JournalCCReceived.Item2;
                string[] arr_Invoice = tuple_get_JournalPosted_JournalCCReceived.Item3;
                string[] arr_InvoiceAmount = tuple_get_JournalPosted_JournalCCReceived.Item4;
                string[] arr_BalanceSettlement = tuple_get_JournalPosted_JournalCCReceived.Item5;
                string[] arr_CurrBal = tuple_get_JournalPosted_JournalCCReceived.Item6;
                string[] arr_CustToRecId = tuple_get_JournalPosted_JournalCCReceived.Item7;
                //=========================================================================================================================================
                //from New Payment Line form
                string tempTransDate = hidden_TransactionDate.Text;

                string Journal_No = hidden_Label_Journal_No.Text;
                string temp_CustomerAcc = hidden_TextBox_CustomerAcc.Text;
                string AmountCurCredit = TextBox_Amount.Text;
                string BankBranch = DropDownList_BankBranch.Text;
                string OtherBankBranch = TextBox_Other.Text;
                string temp_PaymReference = TextBox_Payment_Reference.Text;
                string MSBReceivedDate = hdReceivedDt.Value;
                string MSBChequeDate = hdChequeDt.Value;

                string Txt = TextBox_Txt.Text;
                string SettleInv = TextBox_Settle_Invoice.Text;

                //=========================================================================================================================================
                //default
                string Mode = "new";
                string BankDepositVoucher = "Yes";

                //using class
                AxaptaObject domComCustPayment = DynAx.CreateAxaptaObject("DomComCustPayment");
                string temp_JournalName = Payment_GET_JournalLine_SelectJournal_Transfer.getJournalNameByJournalNum(DynAx, Journal_No);
                //=========================================================================================================================================

                AxaptaRecord DynRec2;
                DynRec2 = DynAx.CreateAxaptaRecord("LedgerJournalTrans");

                if (Mode == "new")
                {
                    DynRec2.set_Field("JournalNum", Journal_No);
                    DynRec2.set_Field("AccountNum", temp_CustomerAcc);

                    var tuple_get_OffsetAccountType_OffsetAccount = Payment_GET_JournalLine_SelectJournal_Transfer.get_OffsetAccountType_OffsetAccount(DynAx, Journal_No);
                    string OffsetAccountType = tuple_get_OffsetAccountType_OffsetAccount.Item1;//Payment_GET_JournalLine_SelectJournal_Transfer.getAxEnumLedgerJournalACType
                    int int_OffsetAccountType = Convert.ToInt32(OffsetAccountType);
                    string OffsetAccount = tuple_get_OffsetAccountType_OffsetAccount.Item2;
                    DynRec2.set_Field("OffsetAccountType", int_OffsetAccountType);
                    DynRec2.set_Field("OffsetAccount", OffsetAccount);

                    string Dimension = domComCustPayment.Call("getDimBranch", temp_CustomerAcc).ToString();
                    string InterCoDimension = Dimension;
                    DynRec2.set_Field("Dimension[1]", Dimension);
                    DynRec2.set_Field("InterCoDimension[1]", InterCoDimension);

                    DynRec2.set_Field("AccountType", 1);//default 1
                    DynRec2.set_Field("Approved", 1);//default 1
                    DynRec2.set_Field("ApprovedBy", GLOBAL.user_id);//default 1
                    DynRec2.set_Field("PostDateChq", 1);//default 1
                    DynRec2.set_Field("TransactionType", 15);//default 15

                    string Payment = domComCustPayment.Call("getPayment", temp_CustomerAcc).ToString();
                    DynRec2.set_Field("Payment", Payment);//note: in testing this have this field

                    string PostingProfile = domComCustPayment.Call("getPostingProfile").ToString();
                    DynRec2.set_Field("PostingProfile", PostingProfile);

                    string Voucher = domComCustPayment.Call("getVoucher", temp_JournalName).ToString();
                    DynRec2.set_Field("Voucher", Voucher);
                }

                string Emplid = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, temp_CustomerAcc);
                if (Emplid == "0303" || Emplid == "0303-1")
                {
                    string CustVal = temp_CustomerAcc.Substring(0, 8);
                    int Length_CustVal = CustVal.Length;
                    CustVal = CustVal.Substring(Length_CustVal - 1, Length_CustVal);

                    if (CustVal == "9")
                    {
                        DynRec2.set_Field("Prepayment", 1);
                    }
                }
                tempTransDate = Function_Method.get_correct_date(GLOBAL.system_checking, tempTransDate, false);
                var var_tempTransDate = DateTime.ParseExact(tempTransDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DynRec2.set_Field("TransDate", var_tempTransDate);
                DynRec2.set_Field("DocumentDate", var_tempTransDate);

                if (BankDepositVoucher == "Yes")
                {
                    DynRec2.set_Field("BankDepositVoucher", 1);//default BankDepositVoucher == "Yes"
                }
                else
                {
                    DynRec2.set_Field("BankDepositVoucher", 0);
                }
                //=========================================================================================================================================
                DynRec2.set_Field("PaymReference", temp_PaymReference);
                //=========================================================================================================================================
                string CurrencyCode = domComCustPayment.Call("getCurrencyCode").ToString();
                DynRec2.set_Field("CurrencyCode", CurrencyCode);
                //=========================================================================================================================================
                double d_AmountCur = Convert.ToDouble(AmountCurCredit);
                double RoundUp_d_AmountCur = Math.Round(d_AmountCur, 2); //round up 2 digit

                DynRec2.set_Field("AmountCurCredit", RoundUp_d_AmountCur);
                //=========================================================================================================================================
                MSBChequeDate = Function_Method.get_correct_date(GLOBAL.system_checking, MSBChequeDate, false);
                var var_MSBChequeDate = DateTime.ParseExact(MSBChequeDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                MSBReceivedDate = Function_Method.get_correct_date(GLOBAL.system_checking, MSBReceivedDate, false);
                var var_MSBReceivedDate = DateTime.ParseExact(MSBReceivedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                DynRec2.set_Field("MSBChequeDate", var_MSBChequeDate);
                DynRec2.set_Field("MSBReceivedDate", var_MSBReceivedDate);
                //=========================================================================================================================================
                string BankTransTypeFirstData = Payment_GET_JournalLine_SelectJournal_Transfer.get_AxBankTransTypeFirstData(DynAx);
                DynRec2.set_Field("BankTransType", BankTransTypeFirstData);
                //=========================================================================================================================================
                string AddInv = "";
                for (int i = 0; i < arr_Counter; i++)
                {
                    if (i == 0)
                    {
                        AddInv = arr_Invoice[i];
                    }
                    else
                    {
                        AddInv = AddInv + "," + arr_Invoice[i];
                    }
                }
                if (AddInv == "")
                {
                    DynRec2.set_Field("LFI_SettleInv", "(AutoSettle)" + SettleInv);
                    DynRec2.set_Field("Txt", Txt);
                }
                else
                {
                    DynRec2.set_Field("Txt", Txt + "(Partial: " + AddInv + ")");
                    if (SettleInv == "" || SettleInv == "0")
                    {
                        DynRec2.set_Field("LFI_SettleInv", AddInv);
                    }
                    else
                    {
                        DynRec2.set_Field("LFI_SettleInv", "(AutoSettle)" + SettleInv + ", (Partial)" + AddInv);
                    }
                }
                //=========================================================================================================================================
                if (BankBranch == "" && OtherBankBranch == "")
                {
                    //do nothing
                }
                else
                {
                    if (OtherBankBranch != "")
                    {
                        DynRec2.set_Field("LF_NormalBankBranch", BankBranch);
                        DynRec2.set_Field("LF_OtherBankBranch", OtherBankBranch);
                        domComCustPayment.Call("InsertNewBankBranch", temp_CustomerAcc, OtherBankBranch, temp_PaymReference).ToString();
                    }
                    else
                    {
                        DynRec2.set_Field("LF_NormalBankBranch", BankBranch);
                        DynRec2.set_Field("LF_OtherBankBranch", OtherBankBranch);
                        domComCustPayment.Call("UpdateDefaultBankBranch", temp_CustomerAcc, BankBranch).ToString();
                    }
                }
                //=========================================================================================================================================
                tempTransDate = Function_Method.get_correct_date(GLOBAL.system_checking, tempTransDate, false);
                var var_DocumentDate = DateTime.ParseExact(tempTransDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var var_TransDate = var_DocumentDate;

                var due = domComCustPayment.Call("getDueDate", var_DocumentDate, var_TransDate);
                var exchRate = domComCustPayment.Call("getExchRate", CurrencyCode, var_TransDate);

                DynRec2.set_Field("Due", due);
                DynRec2.set_Field("ExchRate", exchRate);
                //=========================================================================================================================================
                DynRec2.set_Field("SettleVoucher", 2);//getSettlementType
                //=========================================================================================================================================
                string NewInvoice = "";
                if (SettleInv == "" || SettleInv == "0")
                {
                    NewInvoice = AddInv;
                }
                else
                {
                    NewInvoice = SettleInv;
                }
                //=========================================================================================================================================
                int temp_counter_payment = 0;

                for (int i = 0; i < arr_Counter; i++)
                {
                    if (arr_Payment[i] != "0" || arr_Payment[i] != "")
                    {
                        temp_counter_payment = temp_counter_payment + 1;
                    }
                }
                if (temp_counter_payment == 0 || temp_counter_payment == 1)
                {
                    DynRec2.set_Field("MarkedInvoice", NewInvoice);
                }
                else
                {
                    DynRec2.set_Field("MarkedInvoice", "*");
                }
                //=========================================================================================================================================
                if (Mode == "new")
                {
                    DynRec2.Call("insert");
                }
                else
                {
                    DynRec2.Call("Update");
                }
                string RecordIds = DynRec2.get_Field("RecId").ToString();
                Int64 int_RecordIds = Convert.ToInt64(RecordIds);

                //=========================================================================================================================================
                string LineNum = DynRec2.get_Field("LineNum").ToString();
                double double_LineNum = Convert.ToDouble(LineNum);
                string linenumrec = LineNum + " Line Number";

                //=========================================================================================================================================
                string feedbback_error = UpdateSpecTrans(DynAx, temp_CustomerAcc, arr_Counter, arr_Payment, arr_CustToRecId, CurrencyCode, int_RecordIds, double_LineNum);
                if (feedbback_error != "")
                {
                    Function_Method.MsgBox("ER_PA_06: " + feedbback_error.ToString(), this.Page, this);
                }
                //=========================================================================================================================================
                DynAx.TTSCommit(); DynAx.TTSAbort();
                string msg = feedbback_error;

                DynRec2.Dispose();

                if (msg == "")//no error
                {
                    string Journal_Num = hidden_Label_Journal_No.Text;
                    Session["data_passing"] = "@PAP2_" + Journal_Num;//reload
                    Response.Redirect("Payment.aspx");
                }
            }
            catch (Exception ER_PA_11)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_PA_11: " + ER_PA_11.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private string UpdateSpecTrans(Axapta DynAx, string CustAccNo, int arr_Counter, string[] arr_Payment, string[] arr_CustToRecId, string CurrencyCode, Int64 int_RecordIds, double double_LineNum)
        {
            string feedback = "";
            int CustTable = 77;
            AxaptaObject axQuery02 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource02 = (AxaptaObject)axQuery02.Call("addDataSource", CustTable);

            var qbr02 = (AxaptaObject)axQueryDataSource02.Call("addRange", 1);//CustAccNo
            qbr02.Call("value", CustAccNo);

            AxaptaObject axQueryRun02 = DynAx.CreateAxaptaObject("QueryRun", axQuery02);
            string CustTableRecId = "";
            if ((bool)axQueryRun02.Call("next"))//use if only one record
            {
                AxaptaRecord DynRec02 = (AxaptaRecord)axQueryRun02.Call("Get", CustTable);
                CustTableRecId = DynRec02.get_Field("RecId").ToString();

                DynRec02.Dispose();
            }
            for (int i = 0; i < arr_Counter; i++)
            {
                if (arr_Payment[i] != "" || arr_Payment[i] != "0")
                {
                    string temp_AmountSettle = arr_Payment[i];
                    string temp_AmountSettle_Sign = temp_AmountSettle.Substring(0, 1); // in lotus called SpecAmount
                    int CustTransOpen = 865;
                    string temp_CustToRecId = arr_CustToRecId[i];
                    AxaptaObject SpecTransManager = DynAx.CreateAxaptaObject("SpecTransManager");

                    SpecTransManager.Call("insertWEB", GLOBAL.user_company, CustTransOpen, temp_CustToRecId, temp_AmountSettle, CurrencyCode);
                    //object realLineRecA = DynAx.CallStaticRecordMethod("SpecTrans", "findByRef", GLOBAL.user_company, CustTransOpen, temp_CustToRecId, true);
                    //string str_realLineRecA = realLineRecA.ToString();
                    //=========================================================================================================================================                  
                    AxaptaRecord DynRec03 = DynAx.CreateAxaptaRecord("SpecTrans");
                    DynRec03.ExecuteStmt(string.Format("select forupdate %1 where %1.{0} == {1}", "REFRECID", temp_CustToRecId));

                    //=========================================================================================================================================
                    if (DynRec03.Found)
                    {
                        int SpecTID = 212;
                        DynRec03.set_Field("SpecTableId", SpecTID);// SpecTId="212"
                        DynRec03.set_Field("SpecRecId", int_RecordIds);
                        DynRec03.set_Field("LineNum", double_LineNum);

                        Int64 int_CustTableRecId = 0;
                        if (temp_AmountSettle_Sign == "-")
                        {
                            SpecTID = 77;
                            DynRec03.set_Field("SpecTableId", 77);// SpecTId="77"
                            int_CustTableRecId = Convert.ToInt64(CustTableRecId);
                            DynRec03.set_Field("SpecRecId", int_CustTableRecId);// SpecTId="77"
                        }
                        DynRec03.Update();

                        Update_LF_SpecTrans(DynAx, temp_CustToRecId, temp_AmountSettle, int_RecordIds, SpecTID);

                        if (temp_AmountSettle_Sign != "-")
                        {
                            SpecTransLine(DynAx, temp_CustToRecId, temp_AmountSettle, int_RecordIds, double_LineNum, CurrencyCode, int_CustTableRecId);
                        }
                    }
                    else
                    {
                        feedback = "Line Not Found. ";
                    }
                    DynRec03.Dispose();
                }
            }
            return feedback;
        }

        private void SpecTransLine(Axapta DynAx, string temp_CustToRecId, string temp_AmountSettle, Int64 int_RecordIds, double LineNum, string CurrencyCode, Int64 int_CustTableRecId)
        {
            AxaptaRecord DynRec05 = DynAx.CreateAxaptaRecord("SpecTrans");
            int CustTransOpen = 865;
            int SpecTID = 77;

            DynRec05.set_Field("SpecTableId", SpecTID);
            DynRec05.set_Field("LineNum", LineNum);
            DynRec05.set_Field("Code", CurrencyCode);
            double double_temp_AmountSettle = Convert.ToDouble(temp_AmountSettle);
            DynRec05.set_Field("Balance01", double_temp_AmountSettle);
            DynRec05.set_Field("RefTableId", CustTransOpen);
            Int64 int_temp_CustToRecId = Convert.ToInt64(temp_CustToRecId);
            DynRec05.set_Field("RefRecId", int_temp_CustToRecId);
            DynRec05.set_Field("SpecRecId", int_CustTableRecId);
            DynRec05.set_Field("SpecCompany", GLOBAL.user_company);
            DynRec05.set_Field("RefCompany", GLOBAL.user_company);
            DynRec05.Insert();
            DynRec05.Dispose();
            Update_LF_SpecTrans(DynAx, temp_CustToRecId, temp_AmountSettle, int_RecordIds, SpecTID);
        }

        private void Update_LF_SpecTrans(Axapta DynAx, string temp_CustToRecId, string temp_AmountSettle, Int64 RecordIds, int SpecTID)
        {
            int SpecTrans = 417;

            AxaptaObject axQuery04 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource04 = (AxaptaObject)axQuery04.Call("addDataSource", SpecTrans);

            var qbr04 = (AxaptaObject)axQueryDataSource04.Call("addRange", 7);//temp_CustToRecId
            qbr04.Call("value", temp_CustToRecId);

            var qbr04_1 = (AxaptaObject)axQueryDataSource04.Call("addRange", 1);//SpecTId
            string str_SpecTID = SpecTID.ToString();
            qbr04_1.Call("value", str_SpecTID);

            var qbr04_2 = (AxaptaObject)axQueryDataSource04.Call("addRange", 4);//temp_AmountSettle
            qbr04_2.Call("value", temp_AmountSettle);

            AxaptaObject axQueryRun04 = DynAx.CreateAxaptaObject("QueryRun", axQuery04);
            if ((bool)axQueryRun04.Call("next"))
            {
                AxaptaRecord DynRec04 = (AxaptaRecord)axQueryRun04.Call("Get", SpecTrans);
                string SpecTransRecId = DynRec04.get_Field("RecId").ToString();

                AxaptaRecord DynRec04_2 = DynAx.CreateAxaptaRecord("LF_SpecTrans");
                Int64 int_SpecTransRecId = Convert.ToInt64(SpecTransRecId);
                DynRec04_2.set_Field("SpecTransRecID", int_SpecTransRecId);
                DynRec04_2.set_Field("SourceRecID", RecordIds);
                var today = DateTime.Now.ToString("dd/MM/yyyy");

                var var_today = DateTime.ParseExact(today, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DynRec04_2.set_Field("MarkDate", var_today);
                double double_temp_AmountSettle = Convert.ToDouble(temp_AmountSettle);
                DynRec04_2.set_Field("MarkAmt", double_temp_AmountSettle);
                Int64 int_temp_CustToRecId = Convert.ToInt64(temp_CustToRecId);
                DynRec04_2.set_Field("CustTransOpenRecId", int_temp_CustToRecId);
                DynRec04_2.Insert();

                DynRec04_2.Dispose();
                DynRec04.Dispose();
            }
        }
        private string preprocess(Axapta DynAx)
        {
            string error = "";
            //=========================================================================================================================================
            string Journal_No = hidden_Label_Journal_No.Text;
            string temp_CustomerAcc = hidden_TextBox_CustomerAcc.Text;
            //=========================================================================================================================================
            if (Journal_No == "")
            {
                error = "No Journal Number found. ";
            }

            if (temp_CustomerAcc == "")
            {
                error = error + "No Customer Account Number found. ";
            }
            //=========================================================================================================================================
            string temp_DocumentDate = hidden_TransactionDate.Text;
            string temp_PaymReference = TextBox_Payment_Reference.Text;
            if (temp_DocumentDate != "" && temp_PaymReference != "")
            {
                string DuplicatePaymRef = Payment_GET_JournalLine_SelectJournal_Transfer.get_DuplicatePaymRef(DynAx, temp_PaymReference, temp_DocumentDate);
                if (DuplicatePaymRef == "1")
                {
                    error = error + "Duplicate cheque number. ";
                }
            }
            else
            {
                if (temp_DocumentDate == "")
                {
                    error = error + "DocumentDate is empty. ";
                }
                if (temp_PaymReference == "")
                {
                    error = error + "PaymReference is empty. ";
                }
            }
            //=========================================================================================================================================
            string MSBChequeDate = hdChequeDt.Value;
            string MSBReceivedDate = hdReceivedDt.Value;
            if (MSBChequeDate == "")
            {
                error = error + "Cheque Date is empty. ";
            }

            if (MSBReceivedDate == "")
            {
                error = error + "Received Date is empty. ";
            }
            //=========================================================================================================================================
            string JournalPosted = Payment_GET_JournalLine_SelectJournal_Transfer.get_JournalPosted(DynAx, Journal_No);
            if (JournalPosted == "1")
            {
                error = error + "This Journal has been Posted. Please reload your screen. ";
            }
            return error;
        }

        private Tuple<int, string[], string[], string[], string[], string[], string[]> GetDataFromGridView_SelectInvoice()
        {
            int row_count = GridView_SelectInvoice.Rows.Count;
            string[] arr_Payment = new string[row_count];//full or partial
            string[] arr_Invoice = new string[row_count];//invoice
            string[] arr_InvoiceAmount = new string[row_count];//invoice amount

            string[] arr_BalanceSettlement = new string[row_count];//BalanceSettlement
            string[] arr_CurrBal = new string[row_count];//in payment
            string[] arr_CustToRecId = new string[row_count];//CustToRecId

            int count = 0;
            foreach (GridViewRow row in GridView_SelectInvoice.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    arr_Payment[count] = "0";

                    CheckBox C_CheckBox_FullPayment = (row.Cells[0].FindControl("CheckBox_FullPayment") as CheckBox);
                    TextBox T_TextBox_Partial_PaymentQTY = (row.Cells[1].FindControl("TextBox_Partial_PaymentQTY") as TextBox);

                    if (C_CheckBox_FullPayment.Checked && T_TextBox_Partial_PaymentQTY.Text == "")//full payment & Partial Payment blank
                    {
                        arr_Payment[count] = row.Cells[5].Text;//Balance settlement
                    }
                    if (T_TextBox_Partial_PaymentQTY.Text != "")//Partial Payment not blank
                    {
                        arr_Payment[count] = T_TextBox_Partial_PaymentQTY.Text;
                    }

                    arr_Invoice[count] = row.Cells[3].Text;
                    arr_InvoiceAmount[count] = row.Cells[4].Text;
                    arr_BalanceSettlement[count] = row.Cells[5].Text;
                    arr_CurrBal[count] = row.Cells[6].Text;
                    if (row.Cells[9].Text == "&nbsp;")
                    {
                        arr_CustToRecId[count] = "";
                    }
                    else
                    {
                        arr_CustToRecId[count] = row.Cells[9].Text;
                    }
                }
                count = count + 1;
            }
            return new Tuple<int, string[], string[], string[], string[], string[], string[]>(row_count, arr_Payment, arr_Invoice, arr_InvoiceAmount, arr_BalanceSettlement, arr_CurrBal, arr_CustToRecId);
        }

    }
}








