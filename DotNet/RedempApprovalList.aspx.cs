using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.DynamicData;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class RedempApprovalList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                //TextBox_Account.Attributes.Add("onkeypress", "return controlEnter('" + Button1.ClientID + "', event)");
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
                //Check_DataRequest();
                DropDownList();
                OverviewApproval();
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
            }
            catch
            {
                Session.Clear();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void clear_variable()
        {
            gvApproval.DataSource = null;
            gvApproval.DataBind();
        }

        private void OverviewApproval()
        {
            gvApproval.DataSource = null;
            gvApproval.DataBind();
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string TableName = "LF_WebRedemp_AppGrp";

                int tableId = DynAx.GetTableId(TableName);

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                DataTable dt = new DataTable();
                int data_count = 19;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "chkSelect"; N[2] = "Edit"; N[3] = "AmountRange"; N[4] = "SalesAdmin1"; N[5] = "SalesAdmin2";
                N[6] = "SalesAdmin3"; N[7] = "SalesAdminMng1"; N[8] = "SalesAdminMng2"; N[9] = "SalesAdminMng3"; N[10] = "CreditControlMng1";
                N[11] = "CreditControlMng2"; N[12] = "CreditControlMng3"; N[13] = "OpMng1"; N[14] = "OpMng2"; N[15] = "OpMng3";
                N[16] = "GM1"; N[17] = "GM2"; N[18] = "hdRecId";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }

                axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //axQueryDataSource.Call("addSortField", 50041, 1);//Amount From, asc
                //===========================================
                DataRow row;
                int countA = 0;
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                    countA = countA + 1;
                    if (countA > 0)
                    {
                        row = dt.NewRow();
                        row["No."] = countA;

                        row["AmountRange"] = Convert.ToDouble(DynRec.get_Field("AmountFrom")) + " - " + Convert.ToDouble(DynRec.get_Field("AmountTo"));

                        row["SalesAdmin1"] = DynRec.get_Field("SalesAdmin1").ToString();
                        row["SalesAdmin2"] = DynRec.get_Field("SalesAdmin2").ToString();
                        row["SalesAdmin3"] = DynRec.get_Field("SalesAdmin3").ToString();

                        row["SalesAdminMng1"] = DynRec.get_Field("SalesAdminManager1").ToString();
                        row["SalesAdminMng2"] = DynRec.get_Field("SalesAdminManager2").ToString();
                        row["SalesAdminMng3"] = DynRec.get_Field("SalesAdminManager3").ToString();

                        row["CreditControlMng1"] = DynRec.get_Field("CreditControlManager1").ToString();
                        row["CreditControlMng2"] = DynRec.get_Field("CreditControlManager2").ToString();
                        row["CreditControlMng3"] = DynRec.get_Field("CreditControlManager3").ToString();

                        row["OpMng1"] = DynRec.get_Field("OperationManager1").ToString();
                        row["OpMng2"] = DynRec.get_Field("OperationManager2").ToString();
                        row["OpMng3"] = DynRec.get_Field("OperationManager3").ToString();

                        row["GM1"] = DynRec.get_Field("GeneralManager1").ToString();
                        row["GM2"] = DynRec.get_Field("GeneralManager2").ToString();

                        row["hdRecId"] = DynRec.get_Field("RecId").ToString();

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                }

                // Log off from Microsoft Dynamics AX.
                DynAx.Logoff();
                //Data-Binding with our GRID

                gvApproval.DataSource = dt;
                gvApproval.DataBind();

                gvApproval.UseAccessibleHeader = true;
                gvApproval.HeaderRow.TableSection = TableRowSection.TableHeader;

                gvApproval.Columns[18].Visible = false;

                if (countA == 0)
                {
                    btnDelete.Visible = false;
                }
            }
            catch (Exception ER_RD_11)
            {
                Function_Method.MsgBox("ER_RD_11: " + ER_RD_11.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            clear_variable();
            OverviewApproval();
            gvApproval.PageIndex = e.NewPageIndex;
            gvApproval.DataBind();
        }

        protected void Button_Account_Click(object sender, EventArgs e)
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    //string test = temp1.Substring(0, 6);
                    if (temp1.Substring(0, 6) == "_SFCM@")//Request from SFA > CustomerMaster
                    {
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@SFCM_" + selected_Account;
                        Response.Redirect("SFA.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_PACM@")//Request from Payment > CustomerMaster
                    {
                        string Journal_Num = temp1.Substring(6);//_PACM@ + 8 digit Journal Num

                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@PACM_" + Journal_Num + "|" + selected_Account;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_EOCM@")//Request from EOR > CustomerMaster
                    {
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@EOCM_" + selected_Account;
                        Response.Redirect("EOR.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_PAC2@")//Request from Payment > CustomerMaster
                    {
                        string info = temp1.Substring(6);//_PACM@
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@PAC2_" + info + "|" + selected_Account;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_WCCM@")//Request from WClaim > CustomerMaster
                    {
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@WCCM_" + selected_Account;
                        Response.Redirect("WClaim.aspx");
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            HideListView();
            btnSaveGroup.Visible = true;
            btnUpd.Visible = false;
        }

        protected void gvApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edt")
            {
                HideListView();
                LinkButton lnkBtn = (LinkButton)e.CommandSource;
                GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;
                string ID = gvApproval.DataKeys[myRow.RowIndex].Value.ToString();
                int index = Convert.ToInt32(ID);
                GridViewRow row = gvApproval.Rows[index - 1];

                var split = row.Cells[3].Text.Split('-', ' ');
                txtAmountFrom.Text = split[0];
                txtAmountTo.Text = split[3];
                DropDownList();

                if (row.Cells[4].Text != "&nbsp;") ddlSalesAdmin1.SelectedValue = row.Cells[4].Text;
                if (row.Cells[5].Text != "&nbsp;") ddlSalesAdmin2.SelectedValue = row.Cells[5].Text;
                if (row.Cells[6].Text != "&nbsp;") ddlSalesAdmin3.SelectedValue = row.Cells[6].Text;

                if (row.Cells[7].Text != "&nbsp;") ddlSalesAdminManager1.SelectedValue = row.Cells[7].Text;
                if (row.Cells[8].Text != "&nbsp;") ddlSalesAdminManager2.SelectedValue = row.Cells[8].Text;
                if (row.Cells[9].Text != "&nbsp;") ddlSalesAdminManager3.SelectedValue = row.Cells[9].Text;

                if (row.Cells[10].Text != "&nbsp;") ddlCcMng1.SelectedValue = row.Cells[10].Text;
                if (row.Cells[11].Text != "&nbsp;") ddlCcMng2.SelectedValue = row.Cells[11].Text;
                if (row.Cells[12].Text != "&nbsp;") ddlCcMng3.SelectedValue = row.Cells[12].Text;

                if (row.Cells[13].Text != "&nbsp;") ddlOperationMng1.SelectedValue = row.Cells[13].Text;
                if (row.Cells[14].Text != "&nbsp;") ddlOperationMng2.SelectedValue = row.Cells[14].Text;
                if (row.Cells[15].Text != "&nbsp;") ddlOperationMng3.SelectedValue = row.Cells[15].Text;

                if (row.Cells[16].Text != "&nbsp;") ddlGM1.SelectedValue = row.Cells[16].Text;
                if (row.Cells[17].Text != "&nbsp;") ddlGM2.SelectedValue = row.Cells[17].Text;

                hdRecId.Value = gvApproval.DataKeys[index - 1]["hdRecId"].ToString();
            }
        }

        protected void btnUpd_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebRedemp_AppGrp"))
                {
                    DynAx.TTSBegin();
                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", hdRecId.Value));

                    if (DynRec.Found)
                    {
                        if (!isAmountExist(txtAmountFrom.Text, txtAmountTo.Text))
                        {
                            if (txtAmountFrom.Text != "") DynRec.set_Field("AmountFrom", Convert.ToInt16(txtAmountFrom.Text));
                            if (txtAmountTo.Text != "") DynRec.set_Field("AmountTo", Convert.ToInt32(txtAmountTo.Text));
                        }

                        Dictionary<DropDownList, string> dropdownFields = new Dictionary<DropDownList, string>()
                        {
                            {ddlSalesAdmin1, "SalesAdmin1"},
                            {ddlSalesAdmin2, "SalesAdmin2"},
                            {ddlSalesAdmin3, "SalesAdmin3"},
                            {ddlSalesAdminManager1, "SalesAdminManager1"},
                            {ddlSalesAdminManager2, "SalesAdminManager2"},
                            {ddlSalesAdminManager3, "SalesAdminManager3"},
                            {ddlCcMng1, "CreditControlManager1"},
                            {ddlCcMng2, "CreditControlManager2"},
                            {ddlCcMng3, "CreditControlManager3"},
                            {ddlOperationMng1, "OperationManager1"},
                            {ddlOperationMng2, "OperationManager2"},
                            {ddlOperationMng3, "OperationManager3"},
                            {ddlGM1, "GeneralManager1" },
                            {ddlGM2, "GeneralManager2" },
                        };

                        foreach (var pair in dropdownFields)
                        {
                            DropDownList ddl = pair.Key;
                            string fieldName = pair.Value;

                            if (ddl.SelectedItem.Text == "--SELECT--")
                            {
                                ddl.SelectedItem.Text = "";
                            }

                            DynRec.set_Field(fieldName, ddl.SelectedItem.Text);
                            DynRec.Call("update");
                        }
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }

                OverviewApproval();
                ShowListview();
            }
            catch (Exception ER_RD_20)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_RD_20: " + ER_RD_20.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OverviewApproval();
            ShowListview();
        }

        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                if (!isAmountExist(txtAmountFrom.Text, txtAmountTo.Text))
                {
                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebRedemp_AppGrp"))
                    {
                        DynAx.TTSBegin();

                        if (txtAmountFrom.Text != "") DynRec.set_Field("AmountFrom", Convert.ToInt32(txtAmountFrom.Text));
                        if (txtAmountTo.Text != "") DynRec.set_Field("AmountTo", Convert.ToInt32(txtAmountTo.Text));

                        if (ddlSalesAdmin1.SelectedIndex != 0) DynRec.set_Field("SalesAdmin1", ddlSalesAdmin1.SelectedItem.Value);
                        if (ddlSalesAdmin2.SelectedIndex != 0) DynRec.set_Field("SalesAdmin2", ddlSalesAdmin2.SelectedItem.Value);
                        if (ddlSalesAdmin3.SelectedIndex != 0) DynRec.set_Field("SalesAdmin3", ddlSalesAdmin3.SelectedItem.Value);

                        if (ddlSalesAdminManager1.SelectedIndex != 0) DynRec.set_Field("SalesAdminManager1", ddlSalesAdminManager1.SelectedItem.Value);
                        if (ddlSalesAdminManager2.SelectedIndex != 0) DynRec.set_Field("SalesAdminManager2", ddlSalesAdminManager2.SelectedItem.Value);
                        if (ddlSalesAdminManager3.SelectedIndex != 0) DynRec.set_Field("SalesAdminManager3", ddlSalesAdminManager3.SelectedItem.Value);

                        if (ddlCcMng1.SelectedIndex != 0) DynRec.set_Field("CreditControlManager1", ddlCcMng1.SelectedItem.Value);
                        if (ddlCcMng2.SelectedIndex != 0) DynRec.set_Field("CreditControlManager2", ddlCcMng2.SelectedItem.Value);
                        if (ddlCcMng3.SelectedIndex != 0) DynRec.set_Field("CreditControlManager3", ddlCcMng3.SelectedItem.Value);

                        if (ddlOperationMng1.SelectedIndex != 0) DynRec.set_Field("OperationManager1", ddlOperationMng1.SelectedItem.Value);
                        if (ddlOperationMng2.SelectedIndex != 0) DynRec.set_Field("OperationManager2", ddlOperationMng2.SelectedItem.Value);
                        if (ddlOperationMng3.SelectedIndex != 0) DynRec.set_Field("OperationManager3", ddlOperationMng3.SelectedItem.Value);

                        if (ddlGM1.SelectedIndex != 0) DynRec.set_Field("GeneralManager1", ddlGM1.SelectedItem.Value);
                        if (ddlGM2.SelectedIndex != 0) DynRec.set_Field("GeneralManager2", ddlGM2.SelectedItem.Value);

                        DynRec.Call("insert");
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                    Response.Redirect("RedempApprovalList.aspx");
                }
                else
                {
                    Function_Method.MsgBox("Range amount exceed!", this.Page, this);
                }
            }
            catch (Exception ER_RD_21)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_RD_21: " + ER_RD_21.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void ShowListview()
        {
            Accordion_ApprovalInfo.Visible = true;
            divApproval.Visible = true;
            btnAdd.Visible = true;
            btnDelete.Visible = true;

            btnSaveGroup.Visible = false;
            btnUpd.Visible = false;
            btnCancel.Visible = false;
            btnEdit.Visible = false;
            divEdit.Visible = false;
        }

        protected void HideListView()
        {
            Accordion_ApprovalInfo.Visible = false;
            divApproval.Visible = false;
            btnAdd.Visible = false;
            btnDelete.Visible = false;

            btnEdit.Visible = true;
            divEdit.Visible = true;
            btnUpd.Visible = true;
            btnCancel.Visible = true;
        }

        private void DropDownList()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                // Fetch the user names from AX
                List<string> userNames = GetAxaptaUserNames(DynAx);

                // Insert "--SELECT--" at the top and user names below for each DropDownList
                AddItemsToDropDownLists(userNames);
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Error: " + ex.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose(); // Always dispose of Axapta object
            }
        }

        // Method to fetch user names from Axapta
        private List<string> GetAxaptaUserNames(Axapta DynAx)
        {

            List<string> userNames = new List<string>();
            int userInfo = 65531;

            int sortname = DynAx.GetFieldId(userInfo, "name");
            // Create Axapta query
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");

            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", userInfo); // Adjust with actual table name
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            axQueryDataSource.Call("addSortField", sortname, 1);//, asc

            // Iterate over query results
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", userInfo);
                string name = DynRec.get_Field("name").ToString(); // Replace "name" with actual field
                userNames.Add(name);
            }

            return userNames;
        }

        private void AddItemsToDropDownLists(List<string> userNames)
        {
            // List of DropDownLists to update
            DropDownList[] dropDownLists = { ddlSalesAdmin1, ddlSalesAdmin2, ddlSalesAdmin3,
                                     ddlSalesAdminManager1, ddlSalesAdminManager2, ddlSalesAdminManager3,
                                     ddlCcMng1, ddlCcMng2, ddlCcMng3,
                                     ddlOperationMng1, ddlOperationMng2, ddlOperationMng3,
                                     ddlGM1, ddlGM2 };

            // Add "--SELECT--" and user names to each DropDownList
            foreach (DropDownList ddl in dropDownLists)
            {
                ddl.Items.Clear(); // Clear existing items
                ddl.Items.Insert(0, new ListItem("--SELECT--", "")); // Insert "--SELECT--"

                // Add user names
                foreach (string name in userNames)
                {
                    ddl.Items.Add(new ListItem(name, name));
                }
            }
        }


        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;

            int count = 0;
            foreach (GridViewRow gvRow in gvApproval.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    hdRecId.Value = row.Cells[18].Text;
                    count++;
                }
            }

            // If more than one row is checked, display an error message
            if (count > 1)
            {
                chk.Checked = false;
                return;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            foreach (GridViewRow row in gvApproval.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox_selection = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (CheckBox_selection.Checked)
                    {
                        string recId = gvApproval.DataKeys[row.RowIndex].Values["hdRecId"].ToString();
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebRedemp_AppGrp"))
                        {
                            DynAx.TTSBegin();
                            DynRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RecId", recId));
                        }
                    }
                }
            }
            DynAx.TTSCommit();
            DynAx.TTSAbort();
            DynAx.Dispose();
            Response.Redirect("RedempApprovalList.aspx");
        }

        protected bool isAmountExist(string newFrom, string newTo)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            bool isUnique = true;

            string tableName = "LF_WebRedemp_AppGrp";

            int tableId = DynAx.GetTableId(tableName);

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                string amountFromAx = dynRec.get_Field("AmountFrom").ToString();
                string amountToAx = dynRec.get_Field("AmountTo").ToString();

                if ((double.Parse(newFrom) >= double.Parse(amountFromAx) && double.Parse(newTo) <= double.Parse(amountToAx)) ||
                    (double.Parse(amountFromAx) <= double.Parse(newFrom) && double.Parse(amountToAx) <= double.Parse(newTo)))
                {
                    isUnique = false;
                    break;
                }
            }
            DynAx.Dispose();
            return isUnique;
        }

    }
}