using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class SignboardEquipment_ApprovalList: System.Web.UI.Page
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
                    null, null, null, null,
                    NewProduct2
                    );
                Check_DataRequest();
                PopulateAllDropdowns();
                //first time, reload gridview with Search ALL approach
                OverviewApproval();
            }
        }

        private void Check_DataRequest()
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    if ((temp1.Substring(0, 6) == "_SFCM@") || (temp1.Substring(0, 6) == "_PACM@") || (temp1.Substring(0, 6) == "_EOCM@") || (temp1.Substring(0, 6) == "_PAC2@") || (temp1.Substring(0, 6) == "_WCCM@"))
                    {
                        gvApproval.Columns[1].Visible = true;//Customer Acc button
                        gvApproval.Columns[2].Visible = false;//Customer Acc label

                        if (temp1.Substring(0, 6) == "_PAC2@")//if triggered from invoice hide Salesman Id (request by Keegan)
                        {
                            gvApproval.Columns[6].Visible = false;//Employee ID
                        }
                        else
                        {
                            gvApproval.Columns[6].Visible = true;//Employee ID
                        }
                    }
                    else
                    {//default
                        gvApproval.Columns[1].Visible = false;//Customer Acc button
                        gvApproval.Columns[2].Visible = true;//Customer Acc label
                    }
                }
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
                Session.Clear();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void clear_variable()
        {
           ddlApproval.ClearSelection();
            ddlAltSalesAdmin.ClearSelection();
            ddlSalesAdmin.ClearSelection();
            ddlSalesAdminManager.ClearSelection(); 
            ddlType.ClearSelection();
            ddlCompany.ClearSelection();
            ddlGM.ClearSelection();
        }

        private void OverviewApproval()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

            try
            {
                conn.Open();
                string query = "SELECT * FROM signboardequipmentappgroup";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                DataTable displayTable = new DataTable();
                // Define column names
                string[] columnNames = { "chkSelect", "ID", "Type", "Company", "Approval", "SalesAdmin", "AltSalesAdmin", "SalesAdminManager", "GM" };
                // Add columns to displayTable
                foreach (string columnName in columnNames)
                {
                    if (!displayTable.Columns.Contains(columnName))
                    {
                        displayTable.Columns.Add(new DataColumn(columnName, typeof(string)));
                    }
                }

                // Loop through the set of retrieved records.
                int countA = 1; // Start from 1 for numbering
                foreach (DataRow dataRow in dt.Rows)
                {
                    DataRow newRow = displayTable.NewRow();
                    newRow["chkSelect"] = false; // Assuming you want a checkbox for selection
                    newRow["ID"] = dataRow["ID"].ToString();
                    newRow["Type"] = dataRow["Type"].ToString();
                    newRow["Company"] = dataRow["Company"].ToString();
                    newRow["Approval"] = dataRow["Approval"].ToString();
                    newRow["SalesAdmin"] = dataRow["SalesAdmin"].ToString();
                    newRow["AltSalesAdmin"] = dataRow["AltSalesAdmin"].ToString();
                    newRow["SalesAdminManager"] = dataRow["SalesAdminManager"].ToString();
                    newRow["GM"] = dataRow["GM"].ToString();

                    displayTable.Rows.Add(newRow);
                    countA++;
                }
                // Bind the display table to the GridView
                gvApproval.DataSource = displayTable;
                gvApproval.DataBind();
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Error: " + ex.Message, this.Page, this);
            }
            finally
            {
                conn.Close();
            }
        }


        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApproval.DataKeyNames = new string[] { "ID" };
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

       
        protected void gvApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edt")
            {
                HideListView();
                LinkButton lnkBtn = (LinkButton)e.CommandSource;
                GridViewRow myRow = (GridViewRow)lnkBtn.NamingContainer;
                int rowIndex = myRow.RowIndex; // This gives you the row index on the current page
                                               // Get the DropDownList controls

                GridViewRow row = gvApproval.Rows[rowIndex];

                //DropDownList ddlApproval = (DropDownList)myRow.FindControl("ddlApproval");
                //DropDownList ddlSalesAdmin = (DropDownList)myRow.FindControl("ddlSalesAdmin");
                //DropDownList ddlAltSalesAdmin = (DropDownList)myRow.FindControl("ddlAltSalesAdmin");
                //DropDownList ddlSalesAdminManager = (DropDownList)myRow.FindControl("ddlSalesAdminManager");
                
                // Set the selected values based on the current row
                ddlApproval.SelectedItem.Text = row.Cells[5].Text;
                ddlSalesAdmin.SelectedItem.Text = row.Cells[6].Text;
                ddlAltSalesAdmin.SelectedItem.Text = row.Cells[7].Text;
                ddlSalesAdminManager.SelectedItem.Text = row.Cells[8].Text;
                ddlGM.SelectedItem.Text = row.Cells[9].Text;

                ddlCompany.SelectedItem.Text = row.Cells[4].Text;
                ddlType.SelectedItem.Text = row.Cells[3].Text;
                // Set the hidden field value if needed
                hdRecId.Value = gvApproval.DataKeys[rowIndex]["ID"].ToString();
            }
        }
        // Method to populate DropDownLists (you need to implement this)
        #region Dropdownlist EmplTable
        private List<ListItem> GetEmployeeListFromAxapta()
        {
            List<ListItem> employees = new List<ListItem>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string query = "SELECT * FROM user_tbl ORDER BY logined_user_name ASC ";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        DataSet ds = new DataSet();

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                            Axapta DynAx = Function_Method.GlobalAxapta(); // base class

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                string userId = row["user_id"].ToString();
                                string user_email = userId + "@posim.com.my"; // Assuming email format is

                                #region Axapta Site
                                int EmplTable = DynAx.GetTableIdWithLock("EmplTable");
                                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                                AxaptaObject axDataSource = (AxaptaObject)axQuery.Call("addDataSource", EmplTable);
                                int LF_EmpEMailID = DynAx.GetFieldId(EmplTable, "LF_EmpEMailID");
                                var qbr = (AxaptaObject)axDataSource.Call("addRange", LF_EmpEMailID);
                                qbr.Call("value", user_email);

                                using (AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery))
                                {
                                    if ((bool)axQueryRun.Call("next"))
                                    {
                                        using (AxaptaRecord record = (AxaptaRecord)axQueryRun.Call("Get", EmplTable))
                                        {
                                            string emplId = record.get_Field("EmplId").ToString();
                                            string emplName = record.get_Field("LF_EmplName")?.ToString();

                                            // Check if LF_EmplName is null, empty, or whitespace
                                            if (string.IsNullOrWhiteSpace(emplName))
                                            {
                                                emplName = record.get_Field("Del_Name")?.ToString();
                                            }
                                            if (string.IsNullOrWhiteSpace(emplName) && row.Table.Columns.Contains("logined_user_name"))
                                            {
                                                emplName = row["logined_user_name"].ToString();
                                            }
                                            // Remove trailing information in parentheses
                                            if (!string.IsNullOrWhiteSpace(emplName))
                                            {
                                                emplName = Regex.Replace(emplName, @"\s*\(.*\)", ""); // Removes (2w), (4w), etc.
                                            }
                                            if (!string.IsNullOrWhiteSpace(emplName))
                                            {
                                                string[] words = emplName.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                                var titleCasedWords = words.Select(word =>
                                                    word.Length > 0 ? char.ToUpper(word[0]) + word.Substring(1).ToLowerInvariant() : word
                                                );
                                                emplName = string.Join(" ", titleCasedWords);
                                            }
                                            employees.Add(new ListItem(emplName, emplId));
                                        }
                                    }
                                }
                                #endregion
                            }
                            DynAx.Dispose();
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"GetEmployeeListFromAxapta:{ex}");
                // Optionally rethrow or handle the exception as needed
            }

            return employees;
        }

        private void PopulateAllDropdowns()
        {
            // Get data once
            List<ListItem> employeeList = GetEmployeeListFromAxapta();

            // Define dropdowns to populate
            List<DropDownList> dropdowns = new List<DropDownList>
    {
        ddlApproval,
        ddlSalesAdmin,
        ddlAltSalesAdmin,
        ddlSalesAdminManager,
        ddlGM // Even if disabled
    };
            // Populate all dropdowns
            foreach (var ddl in dropdowns)
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem("-- SELECT --", ""));
                ddl.Items.AddRange(employeeList.ToArray());
            }

            // Disable GM dropdown (if needed)
            //ddlGM.Enabled = false;
        }
        #endregion
        protected void btnUpd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hdRecId.Value))
            {
                // Show error if no record ID is found
                ScriptManager.RegisterStartupScript(this, GetType(), "NoID", "alert('No record selected');", true);
                return;
            }

            try
            {
                // Get the record ID from hidden field
                int recId = int.Parse(hdRecId.Value);

                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string updateQuery = @"
                UPDATE signboardequipmentappgroup 
                SET Approval = @Approval,
                    SalesAdmin = @SalesAdmin,
                    AltSalesAdmin = @AltSalesAdmin,
                    SalesAdminManager = @SalesAdminManager
                WHERE ID = @ID";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn)) // Use MySqlCommand instead of SqlCommand
                    {
                        // Add parameters - use empty string if no selection
                        cmd.Parameters.AddWithValue("@Approval", ddlApproval.SelectedItem.Text ?? "");
                        cmd.Parameters.AddWithValue("@SalesAdmin", ddlSalesAdmin.SelectedItem.Text ?? "");
                        cmd.Parameters.AddWithValue("@AltSalesAdmin", ddlAltSalesAdmin.SelectedItem.Text ?? "");
                        cmd.Parameters.AddWithValue("@SalesAdminManager", ddlSalesAdminManager.SelectedItem.Text ?? "");
                        cmd.Parameters.AddWithValue("@ID", recId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSuccess", "alert('Update successful');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateFailed", "alert('No records were updated');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error and show message
                logger.Error($"Error updating signboardequipmentappgroup: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateError", $"alert('Error updating: {ex.Message}');", true);
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int currentPageIndex = gvApproval.PageIndex;
            gvApproval.PageIndex = currentPageIndex;
            gvApproval.DataBind();
            OverviewApproval();

            ShowListview();
        }

        //protected void btnSaveGroup_Click(object sender, EventArgs e)
        //{
        //    //Axapta DynAx = Function_Method.GlobalAxapta();
        //    //try
        //    //{
        //    //    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Warranty_AppGrp"))
        //    //    {
        //    //        DynAx.TTSBegin();

        //    //        if (ddlWarehouse.SelectedValue != "")
        //    //        {
        //    //            DynRec.set_Field("InventLocationID", ddlWarehouse.SelectedItem.Text);
        //    //            DynRec.set_Field("ClaimType", ddlWarranty.SelectedItem.Text);

        //    //            if (ddlInvoiceCheck1.SelectedIndex != 0) DynRec.set_Field("InvChecked1", ddlInvoiceCheck1.SelectedItem.Text);
        //    //            if (ddlInvoiceCheck2.SelectedIndex != 0) DynRec.set_Field("InvChecked2", ddlInvoiceCheck2.SelectedItem.Text);
        //    //            if (ddlInvoiceCheck3.SelectedIndex != 0) DynRec.set_Field("InvChecked3", ddlInvoiceCheck3.SelectedItem.Text);
        //    //            if (ddlInvoiceCheck4.SelectedIndex != 0) DynRec.set_Field("InvChecked4", ddlInvoiceCheck4.SelectedItem.Text);

        //    //            if (ddlTransport1.SelectedIndex != 0) DynRec.set_Field("TransportArr1", ddlTransport1.SelectedItem.Text);
        //    //            if (ddlTransport2.SelectedIndex != 0) DynRec.set_Field("TransportArr2", ddlTransport2.SelectedItem.Text);
        //    //            if (ddlTransport3.SelectedIndex != 0) DynRec.set_Field("TransportArr3", ddlTransport3.SelectedItem.Text);
        //    //            if (ddlTransport4.SelectedIndex != 0) DynRec.set_Field("TransportArr4", ddlTransport4.SelectedItem.Text);

        //    //            if (ddlGoodReceive1.SelectedIndex != 0) DynRec.set_Field("GoodsRec1", ddlGoodReceive1.SelectedItem.Text);
        //    //            if (ddlGoodReceive2.SelectedIndex != 0) DynRec.set_Field("GoodsRec2", ddlGoodReceive2.SelectedItem.Text);
        //    //            if (ddlGoodReceive3.SelectedIndex != 0) DynRec.set_Field("GoodsRec3", ddlGoodReceive3.SelectedItem.Text);
        //    //            if (ddlGoodReceive4.SelectedIndex != 0) DynRec.set_Field("GoodsRec4", ddlGoodReceive4.SelectedItem.Text);

        //    //            if (ddlInspector1.SelectedIndex != 0) DynRec.set_Field("Inspector1", ddlInspector1.SelectedItem.Text);
        //    //            if (ddlInspector2.SelectedIndex != 0) DynRec.set_Field("Inspector2", ddlInspector2.SelectedItem.Text);
        //    //            if (ddlInspector3.SelectedIndex != 0) DynRec.set_Field("Inspector3", ddlInspector3.SelectedItem.Text);
        //    //            if (ddlInspector4.SelectedIndex != 0) DynRec.set_Field("Inspector4", ddlInspector4.SelectedItem.Text);

        //    //            if (ddlVerifier1.SelectedIndex != 0) DynRec.set_Field("Verifier1", ddlVerifier1.SelectedItem.Text);
        //    //            if (ddlVerifier2.SelectedIndex != 0) DynRec.set_Field("Verifier2", ddlVerifier2.SelectedItem.Text);
        //    //            if (ddlVerifier3.SelectedIndex != 0) DynRec.set_Field("Verifier3", ddlVerifier3.SelectedItem.Text);
        //    //            if (ddlVerifier4.SelectedIndex != 0) DynRec.set_Field("Verifier4", ddlVerifier4.SelectedItem.Text);

        //    //            if (ddlApprover1.SelectedIndex != 0) DynRec.set_Field("Approver1", ddlApprover1.SelectedItem.Text);
        //    //            if (ddlApprover2.SelectedIndex != 0) DynRec.set_Field("Approver2", ddlApprover2.SelectedItem.Text);
        //    //            if (ddlApprover3.SelectedIndex != 0) DynRec.set_Field("Approver3", ddlApprover3.SelectedItem.Text);
        //    //            if (ddlApprover4.SelectedIndex != 0) DynRec.set_Field("Approver4", ddlApprover4.SelectedItem.Text);

        //    //            DynRec.Call("insert");
        //    //        }
        //    //        DynAx.TTSCommit();
        //    //        DynAx.TTSAbort();
        //    //    }
        //    //    //Response.Redirect("ApprovalList.aspx", true);
        //    //    ShowListview();
        //    //    int currentPageIndex = gvApproval.PageIndex;
        //    //    gvApproval.PageIndex = currentPageIndex;
        //    //    gvApproval.DataBind();
        //    //    OverviewApproval();
        //    //}
        //    //catch (Exception ER_AP_20)
        //    //{
        //    //    DynAx.TTSAbort();
        //    //    Function_Method.MsgBox("ER_AP_20: " + ER_AP_20.ToString(), this.Page, this);
        //    //}
        //    //finally
        //    //{
        //    //    DynAx.Dispose();
        //    //}
        //}

        protected void ShowListview()
        {
            Accordion_ApprovalInfo.Text = "Approval Info";
            divApproval.Visible = true;
            //btnAdd.Visible = true;
            //btnDelete.Visible = true;

            //btnSaveGroup.Visible = false;
            btnUpd.Visible = false;
            btnCancel.Visible = false;
            divEdit.Visible = false;
        }

        protected void HideListView()
        {
            Accordion_ApprovalInfo.Text = "Approval Edit";
            divApproval.Visible = false;

            //btnAdd.Visible = false;
            //btnDelete.Visible = false;

            divEdit.Visible = true;
            //btnSaveGroup.Text = "Save New";
            //btnSaveGroup.Visible = true;
            btnUpd.Visible = true;
            btnCancel.Visible = true;
        }

        private void DotNetUser()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "SELECT user_id, page_access_authority FROM user_tbl ORDER BY user_id ASC";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            DataSet ds = new DataSet();

            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                da.Fill(ds);

                List<DropDownList> dropdownLists = new List<DropDownList>()
                {
                    ddlType,ddlCompany,ddlSalesAdmin,ddlApproval,ddlAltSalesAdmin,ddlSalesAdminManager,ddlGM
                };

                ClearAndAddSelectOption(dropdownLists);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string userId = row["user_id"].ToString();
                    string pageAccessAuthority = row["page_access_authority"].ToString();

                    ListItem item = new ListItem(userId);

                    AddItemToDropdownLists(dropdownLists, item);
                }
            }

            conn.Close();
        }

        private void ClearAndAddSelectOption(List<DropDownList> dropdownLists)
        {
            foreach (var ddl in dropdownLists)
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem("-- SELECT --", ""));
            }
        }

        private void AddItemToDropdownLists(List<DropDownList> dropdownLists, ListItem item)
        {
            foreach (var ddl in dropdownLists)
            {
                ddl.Items.Add(item);
            }
        }

        private void AddDefaultSelectItemToDropDowns(List<DropDownList> dropdownLists)
        {
            foreach (var ddl in dropdownLists)
            {
                ddl.Items.Insert(0, new ListItem("-- SELECT --", ""));
            }
        }

        void SetSelectedValue(DropDownList ddl, string getApprovalValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(getApprovalValue))
                {
                    ddl.SelectedValue = getApprovalValue;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Handle the exception, e.g., set to a default value or log the error
                //ddlTransport2.SelectedIndex = 0; // Optional: set to the first item or a default item
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}