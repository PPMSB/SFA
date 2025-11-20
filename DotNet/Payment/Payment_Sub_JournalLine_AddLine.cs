using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {
        //start of sales order
        private void AddPaymentLine()
        {
            JournalLine_section_general.Visible = false; AddLine_section.Visible = true; SelectInvoice_section.Visible = false;
        }

        protected void Button_CancelJournalLine_Click(object sender, EventArgs e)
        {

            JournalLine_section_general.Visible = true; AddLine_section.Visible = false; SelectInvoice_section.Visible = false;

            string temp_Label_Journal_No = hidden_Label_Journal_No.Text;
            clear_add_payment_line();
            Label_Journal_No.Text = "Journal number : " + temp_Label_Journal_No; hidden_Label_Journal_No.Text = temp_Label_Journal_No;//17 words + hidden

            journal_line_view();

        }

        protected void Button_SaveJournalLine_Click(object sender, EventArgs e)
        {
            string temp_Label_Journal_No = hidden_Label_Journal_No.Text;
            clear_add_payment_line();
            Label_Journal_No.Text = "Journal number : " + temp_Label_Journal_No; hidden_Label_Journal_No.Text = temp_Label_Journal_No;//17 words + hidden

        }

        //=========================================================================================================
        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_PACM@" + hidden_Label_Journal_No.Text;//SFA > InventoryMaster
            Response.Redirect("CustomerMaster.aspx");
        }
        protected void CheckAcc(object sender, EventArgs e)
        {
            validate_JL();
        }

        private void validate_JL()
        {

            string CustomerAcc = TextBox_CustomerAcc.Text.Trim();

            string temp_Label_Journal_No = hidden_Label_Journal_No.Text;
            clear_add_payment_line(); TextBox_CustomerAcc.Text = CustomerAcc; hidden_TextBox_CustomerAcc.Text = CustomerAcc;//rewrite back after clear_add_payment_line is called
            Label_Journal_No.Text = "Journal number : " + temp_Label_Journal_No; hidden_Label_Journal_No.Text = temp_Label_Journal_No;//17 words + hidden

            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);


                //=========================================================================================
                if (CustomerAcc == "")
                {
                    Function_Method.MsgBox("There is no Customer Account.", this.Page, this);
                    return;
                }
                else
                {
                    string temp_CustName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, CustomerAcc);
                    if (temp_CustName == "")
                    {
                        Function_Method.MsgBox("Customer number not found in database. Please try again.", this.Page, this);
                        return;
                    }
                    else
                    {
                        Label_CustomerName.Text = temp_CustName;
                    }

                }
                //=========================================================================================
                var tuple_get_JournalPosted_JournalCCReceived = Payment_GET_JournalLine_AddLine.get_JournalPosted_JournalCCReceived(DynAx, temp_Label_Journal_No);

                string Journal_Posted = tuple_get_JournalPosted_JournalCCReceived.Item1;
                string Journal_CCReceived = tuple_get_JournalPosted_JournalCCReceived.Item2;

                if (Journal_Posted == "1")
                {
                    Function_Method.MsgBox("Payment has been Posted. No changes allowed.", this.Page, this);
                    return;
                }

                if (Journal_CCReceived == "1")
                {

                    Function_Method.MsgBox("Payment has been Received. No changes allowed.", this.Page, this);
                    return;
                }
                AddLine_section_details1.Visible = false;
                AddLine_section_details2.Visible = true;
                Button_CancelJournalLine.Enabled = true; Button_SaveJournalLine.Enabled = true;

                //store Customer Account number
                hidden_TextBox_CustomerAcc.Text = CustomerAcc;
                //======================================================================================
                List<ListItem> List_BankCode = new List<ListItem>();
                List_BankCode = Payment_GET_JournalLine_AddLine.get_AxBankcodes(DynAx);
                if (List_BankCode.Count > 1)
                {
                    DropDownList_BankCode.Items.AddRange(List_BankCode.ToArray());
                }
                //======================================================================================
                List<ListItem> List_BankBranchList = new List<ListItem>();
                List_BankBranchList = Payment_GET_JournalLine_AddLine.get_Branch(DynAx, CustomerAcc);
                if (List_BankBranchList.Count > 1)
                {
                    DropDownList_BankBranch.Items.AddRange(List_BankBranchList.ToArray());
                }
                //======================================================================================
                List<ListItem> List_Cheque_Number = new List<ListItem>();
                List_Cheque_Number = Payment_GET_JournalLine_AddLine.get_CountDown();
                DropDownList_Cheque_Number.Items.AddRange(List_Cheque_Number.ToArray());
                //======================================================================================
                //set to default-> Account_Type= BANK
                List<ListItem> List_Account_Type = new List<ListItem>();
                List_Account_Type.Add(new ListItem("BANK", "6"));
                DropDownList_Account_Type.Items.AddRange(List_Account_Type.ToArray());
                DropDownList_Account_Type.Enabled = false;
                //======================================================================================
                //update hidden field
                hidden_TransactionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                /*
                List<ListItem> temp_List_AxCurrencies = new List<ListItem>();
                temp_List_AxCurrencies = Payment_GET_JournalLine_AddLine.get_AxCurrencies(DynAx);
                    
                List<ListItem> temp_AxBankTransType = new List<ListItem>();
                temp_AxBankTransType = Payment_GET_JournalLine_AddLine.get_AxBankTransType(DynAx);

                List<ListItem> temp_get_AxDimension = new List<ListItem>();
                temp_get_AxDimension = Payment_GET_JournalLine_AddLine.get_AxDimension(DynAx, "0");
                */
                //======================================================================================

            }

            catch (Exception ER_PA_03)
            {
                Function_Method.MsgBox("ER_PA_03: " + ER_PA_03.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }

        }
        //======================================================================================================================
        protected void TextBox_Cheque_Number_Changed(object sender, EventArgs e)
        {
            UpdateTextBox_PaymentReference();
        }

        protected void DropDownList_Cheque_Number_Changed(object sender, EventArgs e)
        {
            UpdateTextBox_PaymentReference();
        }

        protected void DropDownList_BankCode_Changed(object sender, EventArgs e)
        {
            UpdateTextBox_PaymentReference();
        }

        private void UpdateTextBox_PaymentReference()
        {
            string ChequeNumber = TextBox_Cheque_Number.Text;

            string BankCode = "";

            if (DropDownList_BankCode.SelectedValue == "")//no value selected
            {
                BankCode = "";
            }
            else
            {
                string SelectedItem = DropDownList_BankCode.SelectedItem.Text;
                string[] arr_SelectedItem = SelectedItem.Split(')');
                BankCode = arr_SelectedItem[0].Substring(1);//minus 1 "(" at the front
            }


            string DropDown_Cheque_Number = DropDownList_Cheque_Number.Text;

            TextBox_Payment_Reference.Text = BankCode + " " + ChequeNumber + " " + DropDown_Cheque_Number;
        }
        //======================================================================================================================
        private void validate_JL_ExistingJournal()
        {
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);


                int LedgerJournalTable = 211;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LedgerJournalTable);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//Journal Number
                qbr.Call("value", hidden_Label_Journal_No.Text);
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                if ((bool)axQueryRun.Call("next"))//use if only one record
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LedgerJournalTable);
                    //======================================================================================
                    string OffsetAccountType = DynRec.get_Field("OffsetAccountType").ToString();
                    if (OffsetAccountType == "")
                    {
                        Function_Method.MsgBox("Offset Account Type cannot be determined", this.Page, this);
                    }
                    string OffsetAccount = DynRec.get_Field("OffsetAccount").ToString();
                    if (OffsetAccount == "")
                    {
                        Function_Method.MsgBox("Offset Account cannot be determined.", this.Page, this);
                    }
                    //======================================================================================
                }

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
        protected void CheckBox_Changed_OtherBankBranch(object sender, EventArgs e)
        {

            if (CheckBox_OtherBankBranch.Checked)
            {
                TextBox_Other.Visible = true;
                DropDownList_BankBranch.Visible = false;
            }
            else
            {
                TextBox_Other.Visible = false;
                DropDownList_BankBranch.Visible = true;
            }
        }

        protected void Button_ChequeDate_Click(object sender, ImageClickEventArgs e)
        {
            //if (Calendar1.Visible == false)
            //{
            //    Calendar1.Visible = true;
            //}
            //else
            //{
            //    Calendar1.Visible = false;
            //}
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            //default
            //string temp = Calendar1.SelectedDate.ToShortDateString();
            //temp = Function_Method.get_correct_date(GLOBAL.system_checking, temp, true);
            //Label_ChequeDate.Text = temp;
            //Calendar1.Visible = false;

        }

        protected void Button_ReceivedDate_Click(object sender, ImageClickEventArgs e)
        {
            //if (Calendar2.Visible == false)
            //{
            //    Calendar2.Visible = true;
            //}
            //else
            //{
            //    Calendar2.Visible = false;
            //}
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            //default
            //string temp = Calendar2.SelectedDate.ToShortDateString();
            //temp = Function_Method.get_correct_date(GLOBAL.system_checking, temp, true);
            //Label_ReceivedDate.Text = temp;
            //Calendar2.Visible = false;

        }

        //=========================================================================================================
        //end of AddLine

    }
}