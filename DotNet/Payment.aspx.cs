using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
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
                clear_Ledger_Journal_Table();
                clear_add_payment_line();
                Session["flag_temp"] = 0;
                Check_DataRequest();
                Session["data_passing"] = "";//in case forget reset                                
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
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string temp1 = GLOBAL.data_passing.ToString();
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        if (temp1.Substring(0, 6) == "@PACM_")//Data receive from CustomerMaster> PA
                        {
                            string JournalNum_selected_ItemId = temp1.Substring(6);
                            string[] arr_JournalNum_selected_ItemId = JournalNum_selected_ItemId.Split('|');
                            string JournalNum = arr_JournalNum_selected_ItemId[0];
                            string selected_ItemId = arr_JournalNum_selected_ItemId[1];

                            hidden_Label_Journal_No.Text = JournalNum;
                            TextBox_CustomerAcc.Text = selected_ItemId;
                            hidden_TextBox_CustomerAcc.Text = selected_ItemId;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalLine_section'); ", true);//go to JournalLine section
                            AddPaymentLine();
                            validate_JL();
                        }
                        else if (temp1.Substring(0, 6) == "@PAP2_" || temp1.Substring(0, 6) == "@PAPA_")//refresh after submit/ post or Data request from Payment> Payment
                        {
                            string JournalNum = temp1.Substring(6);
                            hidden_Label_Journal_No.Text = JournalNum;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalLine_section'); ", true);//go to JournalLine section
                        }
                        else if (temp1.Substring(0, 6) == "@PAC2_" || temp1.Substring(0, 6) == "@PAC3_")//Data request from CustomerMaster> Payment invoice or CustomerMaster> Payment statement
                        {
                            Session["flag_temp"] = "";
                            Session["flag_temp"] = temp1;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Enquiries_section'); ", true);//go to Enquiries section  
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalTable_section'); ", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalTable_section'); ", true);
                    }
                    Session["data_passing"] = "";
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalTable_section'); ", true);
                }
            }
            catch (Exception ER_PA_00)
            {
                Function_Method.MsgBox("ER_PA_00: " + ER_PA_00.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void clear_Ledger_Journal_Table()
        {
            TextBox_Description.Visible = false; TextBox_Description.Text = "";
            hidden_Label_Journal_No.Text = "";
            Button_SaveNewEntry.Text = "SAVE";
            Dropdown();
        }

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void Button_JournalTable_section_Click(object sender, EventArgs e)
        {
            JournalTable_section.Attributes.Add("style", "display:initial"); Button_JournalTable_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            JournalLine_section.Attributes.Add("style", "display:none"); Button_JournalLine_section.Attributes.Add("style", "background-color:transparent");
            Overview_section.Attributes.Add("style", "display:none"); Button_Overview_section.Attributes.Add("style", "background-color:transparent");
            Enquiries_section.Attributes.Add("style", "display:none"); Button_Enquiries_section.Attributes.Add("style", "background-color:transparent");
            btnCheckInvoice.Visible = false;
            btnCheckStatement.Visible = false;
        }

        protected void Button_JournalLine_section_Click(object sender, EventArgs e)
        {
            if (hidden_Label_Journal_No.Text == "")
            {
                Function_Method.MsgBox("There is no Journal Number.", this.Page, this);
                return;
            }
            else
            {
                Label_Journal_No.Text = "Journal number : " + hidden_Label_Journal_No.Text;//17 words + hidden
            }

            //=========================================================================================================================================================
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                var tuple_get_JournalPosted_JournalCCReceived = Payment_GET_JournalLine_AddLine.get_JournalPosted_JournalCCReceived(DynAx, hidden_Label_Journal_No.Text);
                string Journal_Posted = tuple_get_JournalPosted_JournalCCReceived.Item1;
                string Journal_CCReceived = tuple_get_JournalPosted_JournalCCReceived.Item2;
                bool lock_edit = false;
                if (Journal_Posted == "1")//Payment has been Posted. No changes allowed.
                {
                    lock_edit = true;
                }

                if (Journal_CCReceived == "1")//Payment has been Received. No changes allowed.
                {
                    lock_edit = true;
                }

                TextBox_Description.Visible = true;
                TextBox_Description.Text = Payment_GET_Ledger_Journal_Table.getJournalDescriptionExisting(DynAx, hidden_Label_Journal_No.Text);
                DropDownList2.SelectedValue = Payment_GET_JournalLine_SelectJournal_Transfer.getJournalNameByJournalNum(DynAx, hidden_Label_Journal_No.Text);
                if (lock_edit == true)
                {
                    //lock the textbox and button from editting in Sales Header
                    Button_SaveNewEntry.Enabled = false;
                    TextBox_Description.ReadOnly = true; TextBox_Description.Attributes.Add("style", "background-color:#ffdf87");
                    DropDownList2.Enabled = false; DropDownList2.Attributes.Add("style", "background-color:#ffdf87");
                }
                else
                {
                    Button_SaveNewEntry.Enabled = true; Button_SaveNewEntry.Text = "Update (J/N: " + hidden_Label_Journal_No.Text + " )";
                    //unlock the textbox and button from editting in Sales Header
                    TextBox_Description.ReadOnly = false; TextBox_Description.Attributes.Add("style", "background-color: white");
                    DropDownList2.Enabled = true; DropDownList2.Attributes.Add("style", "background-color: white");
                }
                journal_line_view();//run grid journal_line_view
                JournalTable_section.Attributes.Add("style", "display:none"); Button_JournalTable_section.Attributes.Add("style", "background-color:transparent");
                JournalLine_section.Attributes.Add("style", "display:initial"); Button_JournalLine_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                Overview_section.Attributes.Add("style", "display:none"); Button_Overview_section.Attributes.Add("style", "background-color:transparent");
                Enquiries_section.Attributes.Add("style", "display:none"); Button_Enquiries_section.Attributes.Add("style", "background-color:transparent");
            }
            catch (Exception ER_PA_10)
            {
                Function_Method.MsgBox("ER_PA_10: " + ER_PA_10.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_Overview_section_Click(object sender, EventArgs e)
        {
            JournalTable_section.Attributes.Add("style", "display:none"); Button_JournalTable_section.Attributes.Add("style", "background-color:transparent");
            JournalLine_section.Attributes.Add("style", "display:none"); Button_JournalLine_section.Attributes.Add("style", "background-color:transparent");
            Overview_section.Attributes.Add("style", "display:initial"); Button_Overview_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Enquiries_section.Attributes.Add("style", "display:none"); Button_Enquiries_section.Attributes.Add("style", "background-color:transparent");
            btnCheckInvoice.Visible = false;
            btnCheckStatement.Visible = false;
            //first, go to ListOutStanding
            f_Button_ListOutStanding();
        }

        protected void Button_Enquiries_section_Click(object sender, EventArgs e)
        {
            JournalTable_section.Attributes.Add("style", "display:none"); Button_JournalTable_section.Attributes.Add("style", "background-color:transparent");
            JournalLine_section.Attributes.Add("style", "display:none"); Button_JournalLine_section.Attributes.Add("style", "background-color:transparent");
            Overview_section.Attributes.Add("style", "display:none"); Button_Overview_section.Attributes.Add("style", "background-color:transparent");
            Enquiries_section.Attributes.Add("style", "display:none"); Button_Enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            btnCheckInvoice.Visible = true;
            btnCheckStatement.Visible = true;
            btnCheckInvDue.Visible = true;
            //btnCheckInvToBeDue.Visible = true;
            //first, go to Enquiries
            //f_Button_Enquiries_Click();
            string temp1 = Session["flag_temp"].ToString();
            if (temp1.Length >= 6)
            {
                //if (!Enquiries_section_general.Visible)
                //{
                //    btnCheckInvDue_Click(btnCheckInvDue, EventArgs.Empty);
                //}
                if (temp1.Substring(0, 6) == "@PAC2_")
                {
                    btnCheckInvoice_Click(btnCheckInvoice, EventArgs.Empty);
                }
                else
                {
                    btnCheckStatement_Click(btnCheckStatement, EventArgs.Empty);
                }
            }
        }

        protected void Dropdown()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            DropDownList2.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            try
            {
                items = Payment_GET_Ledger_Journal_Table.getJournalNameList(DynAx);
                if (items.Count > 1)
                {
                    DropDownList2.Items.AddRange(items.ToArray());
                }
                else
                {
                    Function_Method.MsgBox("There is no Journal available.", this.Page, this);
                }
            }
            catch (Exception ER_PA_01)
            {
                Function_Method.MsgBox("ER_PA_01: " + ER_PA_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void OnSelectedIndexChanged_DropDownList2(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedItem.Value != "")//Only selected
            {
                Button_SaveNewEntry.Visible = true;
                string SelectedItem = DropDownList2.SelectedItem.Text;

                string SelectedValue = DropDownList2.SelectedValue.ToString();//Journal Name
                string JournalNameBracket = "(" + SelectedValue + ") ";

                string getDescription = SelectedItem.Replace(JournalNameBracket, "");//getDescription
                TextBox_Description.Visible = true; TextBox_Description.Text = getDescription;
            }
            else
            {
                Button_SaveNewEntry.Visible = false;
                TextBox_Description.Visible = false; TextBox_Description.Text = "";
            }
        }

        protected void Button_SaveNewEntry_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                if (DropDownList2.SelectedValue != "")//Only selected
                {
                    string SelectedValue = DropDownList2.SelectedValue.ToString();

                    string JournalName = SelectedValue;
                    string Description = TextBox_Description.Text;

                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LedgerJournalTable"))
                    {
                        DynAx.TTSBegin();
                        string temp_logined_user_name = GLOBAL.logined_user_name;
                        if (temp_logined_user_name.Length > 40)
                        {
                            temp_logined_user_name = temp_logined_user_name.Substring(0, 38) + "..";
                        }

                        if (Button_SaveNewEntry.Text == "SAVE" || hidden_Label_Journal_No.Text == "")
                        {
                            DynRec.set_Field("LFI_Web", temp_logined_user_name);
                            DynRec.set_Field("JournalName", JournalName);
                            DynRec.set_Field("Name", Description);
                            DynRec.set_Field("PostDateChq", 1);
                            DynRec.Call("insert");
                        }
                        else//Update
                        {
                            //DynRec.ExecuteStmt("select * from %1 where %1.{0} == {1}", "JOURNALNUM", hidden_Label_Journal_No.Text);
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "JOURNALNUM", hidden_Label_Journal_No.Text));

                            if (DynRec.Found)
                            {
                                DynRec.set_Field("LFI_Web", temp_logined_user_name);
                                DynRec.set_Field("JournalName", JournalName);
                                DynRec.set_Field("Name", Description);
                                DynRec.set_Field("PostDateChq", 1);
                                DynRec.Call("Update");
                            }
                        }

                        DynAx.TTSCommit();
                        string JournalNum = DynRec.get_Field("JournalNum").ToString();
                        DynAx.TTSAbort();
                        hidden_Label_Journal_No.Text = JournalNum;
                        Function_Method.MsgBox("Journal Number: " + JournalNum, this.Page, this);
                        //
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalLine_section'); ", true);
                        //go to JournalLine_section
                    }
                }
            }
            catch (Exception ER_PA_04)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_PA_04: " + ER_PA_04.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_DeleteDuplicate_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string msg = "";
                string Journal_Num = hidden_Label_Journal_No.Text;
                if (Journal_Num == "")
                {
                    msg = "There is no journal number selected.";
                }
                else
                {
                    msg = Payment_GET_Ledger_Journal_Table.get_delete_duplicate_JN(DynAx, GLOBAL.user_id, hidden_Label_Journal_No.Text);
                }
                Function_Method.MsgBox(msg, this.Page, this);
            }
            catch (Exception ER_PA_12)
            {
                Function_Method.MsgBox("ER_PA_12: " + ER_PA_12.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_Delete_PaymentLine_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string error = "";
                foreach (GridViewRow row in GridView_Line_View.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox CheckBox_selection = (row.Cells[0].FindControl("CheckBox_ToDeleteByOne") as CheckBox);
                        if (CheckBox_selection.Checked)
                        {
                            GridView_Line_View.Columns[6].Visible = true;//Hidden RecID
                            GridView_Line_View.Columns[7].Visible = true;//Hidden Alter

                            string RecId = row.Cells[6].Text;
                            string allow_alter = row.Cells[7].Text;

                            error += get_delete_PaymentLine(DynAx, RecId);
                            /*
                            if (allow_alter == "1")//can be updated
                            {
                                error += get_delete_PaymentLine(DynAx, RecId);
                            }
                            else
                            {
                                error += "Cannot delete Payment Line " + RecId + " that has been created before.";
                            }*/
                        }
                    }
                }
                if (error != "")
                {
                    Function_Method.MsgBox(error, this.Page, this);
                }
                journal_line_view();
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox(ex.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private string get_delete_PaymentLine(Axapta DynAx, string RecId)
        {
            string error = "";
            try
            {
                AxaptaRecord MyRec = DynAx.CreateAxaptaRecord("LedgerJournalTrans");

                DynAx.TTSBegin();
                MyRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RecId", RecId));

                DynAx.TTSCommit(); DynAx.TTSAbort();
                return error;
            }
            catch (Exception)
            {
                DynAx.TTSAbort();
                error = RecId + " failed to delete. ";
                return error;
            }
        }

        protected void CheckBox_Changed_ToDeleteAll(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView_Line_View.HeaderRow.FindControl("CheckBox_ToDeleteAll");

            foreach (GridViewRow row in GridView_Line_View.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("CheckBox_ToDeleteByOne");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                    //row.BackColor = Color.FromName("#ff8000");
                    Button_Delete_PaymentLine.Visible = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                    Button_Delete_PaymentLine.Visible = false;
                }
            }
        }

        protected void CheckBox_Changed_ToDeleteByOne(object sender, EventArgs e)
        {
            int row_count = GridView_Line_View.Rows.Count;

            int count_checked = 0;
            for (int i = 0; i < row_count; i++)
            {
                CheckBox CheckBox_selection = (GridView_Line_View.Rows[i].Cells[0].FindControl("CheckBox_ToDeleteByOne") as CheckBox);
                if (GridView_Line_View.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (CheckBox_selection.Checked)//highlight
                    {
                        count_checked = count_checked + 1;
                        GridView_Line_View.Rows[i].BackColor = System.Drawing.Color.FromName("#ff8000");
                    }
                }
            }

            CheckBox ChkBoxHeader = (CheckBox)GridView_Line_View.HeaderRow.FindControl("CheckBox_ToDeleteAll");
            if (count_checked == row_count)
            {
                ChkBoxHeader.Checked = true;
            }
            else
            {
                ChkBoxHeader.Checked = false;
            }

            if (count_checked != 0)
            {
                Button_Delete_PaymentLine.Visible = true;
            }
            else
            {
                Button_Delete_PaymentLine.Visible = false;
            }
        }

        private void journal_line_view()
        {
            Button_Journal_Line.Visible = true;
            //GridView_Line_View.Columns[13].Visible = true; GridView_Line_View.Columns[14].Visible = true;

            GridView_Line_View.DataSource = null; GridView_Line_View.DataBind();
            //
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                int LedgerJournalTrans = 212;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LedgerJournalTrans);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//JOURNALNUM

                qbr.Call("value", hidden_Label_Journal_No.Text);
                axQueryDataSource.Call("addSortField", 65534, 1);//RecId, dsc

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 7;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Trans. Date"; N[2] = "Account";
                N[3] = "Txt."; N[4] = "Credit";
                N[5] = "Hidden_RecId"; N[6] = "Hidden_allow_alter";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================

                DataRow row;
                int countA = 0;

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LedgerJournalTrans);
                    countA = countA + 1;

                    row = dt.NewRow();
                    //transfer to grid
                    row["No."] = countA.ToString();

                    //string LineNum = DynRec.get_Field("LineNum").ToString();
                    string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                    string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                    string Raw_DeliveryDate = arr_temp_TransDate[0];
                    row["Trans. Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DeliveryDate, true);

                    string temp_AccountNum = DynRec.get_Field("AccountNum").ToString();
                    string temp_CustName = Payment_GET_Overview.CustName(DynAx, temp_AccountNum);
                    row["Account"] = "(" + DynRec.get_Field("AccountNum").ToString() + ") " + temp_CustName;
                    row["Txt."] = DynRec.get_Field("Txt").ToString();

                    row["Credit"] = DynRec.get_Field("AmountCurCredit").ToString();

                    row["Hidden_RecId"] = DynRec.get_Field("RecId").ToString();

                    row["Hidden_allow_alter"] = "";

                    //row["Hidden_allow_alter"] = DynRec.get_Field("LineAmount").ToString();

                    dt.Rows.Add(row);
                    DynRec.Dispose();
                }
                if (countA == 0)//grid empty
                {
                    //close grid
                    Button_Journal_Line.Visible = false;
                    return;
                }

                GridView_Line_View.DataSource = dt;
                GridView_Line_View.DataBind();

                GridView_Line_View.Columns[6].Visible = false;//Hidden RecID
                GridView_Line_View.Columns[7].Visible = false;//Hidden Alter
            }
            catch (Exception ER_PA_02)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_PA_02: " + ER_PA_02.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_AddPaymentLine_Click(object sender, ImageClickEventArgs e)
        {
            AddPaymentLine();
        }

        protected void Button_SelectInvoice_Click(object sender, EventArgs e)
        {
            string error_msg = "";

            if (TextBox_Amount.Text == "")
            {
                error_msg = "Amount";
            }

            string chequeDate = Request.Form[txtChequeDate.UniqueID];
            hdChequeDt.Value = chequeDate;
            if (chequeDate == "")
            {
                if (error_msg != "")
                {
                    error_msg = error_msg + " , " + "Cheque Date";
                }
                else
                {
                    error_msg = "Cheque Date";
                }
            }

            string receivedDate = Request.Form[txtReceivedDate.UniqueID];
            hdReceivedDt.Value = receivedDate;
            if (receivedDate == "")
            {
                if (error_msg != "")
                {
                    error_msg = error_msg + " , " + "Received Date";
                }
                else
                {
                    error_msg = "Received Date";
                }
            }

            if (DropDownList_BankCode.SelectedValue == "")
            {
                if (error_msg != "")
                {
                    error_msg = error_msg + " , " + "Bank Code";
                }
                else
                {
                    error_msg = "Bank Code";
                }
            }

            if (error_msg == "")
            {
                SelectInvoice();
            }
            else
            {
                Function_Method.MsgBox(error_msg + " field need to keyin.", this.Page, this);
            }
        }

        private void clear_add_payment_line()
        {
            AddLine_section_details1.Visible = true; AddLine_section_details2.Visible = false;
            Button_CancelJournalLine.Enabled = true; Button_SaveJournalLine.Enabled = false;
            Label_Journal_No.Text = "";
            GridView_Line_View.DataSource = null; GridView_Line_View.DataBind();

            TextBox_CustomerAcc.Text = "";
            TextBox_Amount.Text = "";
            TextBox_Other.Text = ""; 
            TextBox_Other.Visible = false;

            TextBox_Cheque_Number.Text = "";
            TextBox_Payment_Reference.Text = "";
            TextBox_Txt.Text = "";
            TextBox_Settle_Invoice.Text = "";
            TextBox_Settle_Amount.Text = "";

            txtReceivedDate.Text = "";
            txtChequeDate.Text = "";

            DropDownList_Cheque_Number.Items.Clear();
            DropDownList_BankCode.Items.Clear();
            DropDownList_BankBranch.Items.Clear(); DropDownList_BankBranch.Visible = true;
            DropDownList_Account_Type.Items.Clear(); DropDownList_Account_Type.Enabled = true;

            hidden_TextBox_CustomerAcc.Text = "";
            hidden_Label_Journal_No.Text = "";
            hidden_TransactionDate.Text = "";


        }

        private void ClearFilterSection()
        {
            TextBox_SearchEnquiries.Text = "";
            DropDownList_Salesman.ClearSelection(); ;
            //DropDownList_DayInvoice.ClearSelection();
            ddlCustomerAcc.ClearSelection(); ;
            txtCustAcc.Text = "";
            rblInvoice.ClearSelection();
            Session["data_passing"] = "";
        }


        protected void btnCheckInvoice_Click(object sender, EventArgs e)
        {
            ClearFilterSection();
            divDays.Visible = true;
            TextBox_SearchEnquiries.Visible = true;
            labeltext.Visible = true;
            Button_FindList.Visible = true;
            gvInvoiceDue.Visible = false;
            rblInvoice.Visible = false;

            btnCheckInvDue.Attributes.Add("style", "background-color:transparent");
            btnCheckInvoice.Attributes.Add("style", "background-color:#f58345");
            btnCheckStatement.Attributes.Add("style", "background-color:transparent");
            JournalTable_section.Attributes.Add("style", "display:none");
            JournalLine_section.Attributes.Add("style", "display:none");
            Overview_section.Attributes.Add("style", "display:none");
            Enquiries_section.Attributes.Add("style", "display:initial"); Button_Enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Enquiries_section_general.Visible = false;
            upInvoice.Visible = true;
            Enquiries_section_Statement.Visible = false;
            upStatement.Visible = false;

            f_Button_Enquiries_Click();
        }

        protected void btnCheckStatement_Click(object sender, EventArgs e)
        {
            //ClearFilterSection();
            btnCheckInvDue.Attributes.Add("style", "background-color:transparent");
            btnCheckStatement.Attributes.Add("style", "background-color:#f58345");
            btnCheckInvoice.Attributes.Add("style", "background-color:transparent");
            JournalTable_section.Attributes.Add("style", "display:none");
            JournalLine_section.Attributes.Add("style", "display:none");
            Overview_section.Attributes.Add("style", "display:none");
            Enquiries_section.Attributes.Add("style", "display:initial"); Button_Enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Enquiries_section_general.Visible = false;
            upInvoice.Visible = false;
            upStatement.Visible = true;
            Enquiries_section_Statement.Visible = false;
            gvInvoiceDue.Visible = false;
            rblInvoice.Visible = false;

            showStatement();
        }

        protected void btnCheckInvDue_Click(object sender, EventArgs e)
        {
            ClearFilterSection();
            Axapta DynAx = Function_Method.GlobalAxapta();
            divDays.Visible = false;
            TextBox_SearchEnquiries.Visible = false;
            labeltext.Visible = false;
            Button_FindList.Visible = false;
            gvInvoiceDue.Visible = true;
            rblInvoice.Visible = true;

            btnCheckInvDue.Attributes.Add("style", "background-color:#f58345");
            btnCheckStatement.Attributes.Add("style", "background-color:#transparent");
            btnCheckInvoice.Attributes.Add("style", "background-color:transparent");
            JournalTable_section.Attributes.Add("style", "display:none");
            JournalLine_section.Attributes.Add("style", "display:none");
            Overview_section.Attributes.Add("style", "display:none");
            Enquiries_section.Attributes.Add("style", "display:initial"); Button_Enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            upStatement.Visible = false;
            Enquiries_section_Statement.Visible = false;
            Enquiries_section_general.Visible = false;
            f_Dropdown_Salesman();
            Session["data_passing"] = "InvDue";
        }
    }
}