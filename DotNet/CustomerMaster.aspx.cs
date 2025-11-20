using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using NLog.Time;
using System;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.EnterpriseServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class CustomerMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();
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
                SetFocus(TextBox_Account);
                TextBox_Account.Attributes.Add("autocomplete", "off");
                Check_DataRequest();

                //first time, reload gridview with Search ALL approach
                GridView1.PageIndex = 0;
                f_call_ax(0);
            }
        }

        private void Check_DataRequest()
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    string prefix = temp1.Substring(0, 6);
                    switch (prefix)
                    {
                        case "_SFCM@":
                        case "_PACM@":
                        case "_SQCM@":
                        case "_RDCM@":
                        case "_EOCM@":
                        case "_PAC2@":
                        case "_PAC3@":
                        case "_WCCM@":
                        case "_EBCM@":
                        case "_CACM@":
                        case "_ICACM":
                        case "_SECM@":
                            GridView1.Columns[1].Visible = true; // Customer Acc button
                            GridView1.Columns[2].Visible = false; // Customer Acc label

                            if (prefix == "_PAC2@" || prefix == "_SQCM@" || prefix == "_PAC3@")
                            {
                                GridView1.Columns[4].Visible = false; // Employee ID
                            }
                            else
                            {
                                GridView1.Columns[4].Visible = true; // Employee ID
                            }
                            break;


                        default: // Default case
                            GridView1.Columns[1].Visible = false; // Customer Acc button
                            GridView1.Columns[2].Visible = true; // Customer Acc label
                            GridView1.Columns[7].Visible = false; // Point
                            break;
                    }
                }
            }
            else
            {
                GridView1.Columns[1].Visible = false;//Customer Acc button
                GridView1.Columns[2].Visible = true;//Customer Acc label 
                GridView1.Columns[7].Visible = false; // Point
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

        protected void CheckAcc(object sender, EventArgs e)
        {
            clear_variable();
            GridView1.PageIndex = 0;
            f_call_ax(0);
        }

        private void clear_variable()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        private void f_call_ax(int PAGE_INDEX)
        {
            string fieldName;
            AxaptaObject axQuery;
            AxaptaObject axQueryRun;
            AxaptaObject axQueryDataSource;
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;

            switch (DropDownList1.SelectedItem.Text)
            {
                case "Account No.":
                    fieldName = "AccountNum";
                    break;
                case "Customer Name":
                    fieldName = "Name";
                    break;

                default:
                    fieldName = "AccountNum";
                    break;
            }

            string TableName = "CustTable";
            string AccountNum = "";
            //string fieldName = ("AccountNum");
            string fieldValue = "*" + TextBox_Account.Text.Trim() + "*";

            //field names for calls to AxRecord.get_field
            //int[] numbers = { 4, 5, 6, 1, 2, 3, -2, -1, 0 };

            int data_count = 11;
            string[] F = new string[data_count];
            F[0] = "AccountNum"; F[1] = "Name"; F[2] = "City";
            F[3] = "State"; F[4] = "EmplId"; F[5] = "LPPoint"; F[6] = "Phone";
            F[7] = "TeleFax"; F[8] = "CellularPhone"; F[9] = "Email"; F[10] = "Address";

            string[] N = new string[data_count];
            N[0] = "Account"; N[1] = "Customer Name"; N[2] = "City";
            N[3] = "State"; N[4] = "Employee ID"; N[5] = "LPPoint"; N[6] = "Phone";
            N[7] = "TeleFax"; N[8] = "Cellular Phone"; N[9] = "Email"; N[10] = "Address";

            // Output variables for calls to AxRecord.get_Field
            object[] O = new object[data_count];
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int tableId = DynAx.GetTableId(TableName);

                axQuery = DynAx.CreateAxaptaObject("Query");
                axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", DynAx.GetFieldId(tableId, fieldName));
                qbr.Call("value", fieldValue);

                var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);//CustGroup
                qbr4_2.Call("value", "TDI");

                var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
                qbr4_3.Call("value", "TDE");

                var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
                qbr4_4.Call("value", "TDO");

                axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                axQueryDataSource.Call("addSortField", 2, 0);//Customer Name, asc

                DataTable dt = CreateVendorDataTable(data_count, N);
                DataRow row;
                //===========================================
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();
                        row["No"] = countA;
                        for (int i = 0; i < data_count; i++)
                        {
                            AccountNum = DynRec.get_Field("AccountNum").ToString();
                            if (i == 5)
                            {
                                var getTpBalance = EOR_GET_NewApplicant.getPointBalance(DynAx, AccountNum);
                                double adjPts = EOR_GET_NewApplicant.getPPMTP_AdjPts(DynAx, AccountNum);
                                double usedPts = EOR_GET_NewApplicant.getPPMTP_UsedPt(DynAx, AccountNum);
                                double getTotal = getTpBalance.Item1 + adjPts + usedPts;
                                row[N[i]] = getTotal.ToString("#,###,###,##0.00");
                            }
                            else
                            {
                                O[i] = DynRec.get_Field(F[i]);
                                row[N[i]] = O[i].ToString().Trim();
                            }
                        }

                        dt.Rows.Add(row);

                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            FINISH:
                GridView1.VirtualItemCount = countA;

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ER_CM_00)
            {
                Function_Method.MsgBox("ER_CM_00: " + ER_CM_00.ToString(), this.Page, this);
            }
            DynAx.Dispose();
        }

        private DataTable CreateVendorDataTable(int data_count, string[] N)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("No", typeof(string)));
            for (int i = 0; i < data_count; i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }

            return dt;
        }
        //=====================================================================================
        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                clear_variable();
                f_call_ax(e.NewPageIndex);
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataBind();
            }
            catch (Exception ER_CM_01)
            {
                Function_Method.MsgBox("ER_CM_01: " + ER_CM_01.ToString(), this.Page, this);
            }
        }

        protected void Button_Account_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    Button ButtonAccount = sender as Button;

                    //string test = temp1.Substring(0, 6);
                    if (temp1.Substring(0, 6) == "_SFCM@")//Request from SFA > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Button_Cust_Campaign.Enabled = false;
                        Session["data_passing"] = "@SFCM_" + selected_Account;
                        Response.Redirect("SFA.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_PACM@")//Request from Payment > CustomerMaster
                    {
                        string Journal_Num = temp1.Substring(6);//_PACM@ + 8 digit Journal Num

                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Button_Cust_Campaign.Enabled = false;
                        Session["data_passing"] = "@PACM_" + Journal_Num + "|" + selected_Account;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_EOCM@")//Request from EOR > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Button_Cust_Campaign.Enabled = false;
                        Session["data_passing"] = "@EOCM_" + selected_Account;
                        Response.Redirect("EOR.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_RDCM@")//Request from Redemption > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Button_Cust_Campaign.Enabled = false;

                        Session["data_passing"] = "@RDCM_" + selected_Account;

                        string getWarning = "";
                        string emplID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, selected_Account);
                        string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, emplID);//salesapprovalgroupid
                        var splitHod = getHod.Split('_');
                            var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, selected_Account);
                        var tuple_getGracePeriod = SFA_GET_SALES_HEADER.get_SuffixCode(DynAx, tuple_getCustInfo.Item6.ToString());

                        // Jerry 2024-11-21 Allow Sales Admin to bypass inactivity checking
                        //var sales_admin = Redemption_Get_Details.getSalesAdmin(DynAx);
                        // Jerry 2024-11-21 End

                        if (splitHod[0] != "")
                        {
                            // Jerry 2024-11-21 Allow Sales Admin to bypass inactivity checking
                            //string logined_user_name = Session["logined_user_name"].ToString();
                            //if (sales_admin.Contains(logined_user_name))
                            //{
                            //    Response.Redirect("Redemption.aspx");
                            //}
                            // Jerry 2024-11-21 End

                            if (Session["user_id"].ToString() == splitHod[0] || splitHod.Length > 1 && Session["user_id"].ToString() == splitHod[1] ||
                                splitHod.Length > 2 && Session["user_id"].ToString() == splitHod[2])//HOD, 9months, 270days
                            {
                                // Jerry 2024-12-04 Allow HOD to create redemption for customer with up to 365 days of inactivities
                                //getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, selected_Account, tuple_getGracePeriod.Item3);
                                getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, selected_Account, tuple_getGracePeriod.Item3, true);
                                // Jerry 2024-12-04 End

                                if (getWarning != "")
                                {
                                    //panelNormal.Visible = false;
                                    Function_Method.MsgBox(getWarning, this.Page, this);
                                    ShowMessageAndRedirect(getWarning, "CustomerMaster.aspx");
                                }
                                else
                                {
                                    Response.Redirect("Redemption.aspx");
                                }
                            }
                            else if (Session["user_id"].ToString() != splitHod[0] || Session["user_id"].ToString() != splitHod[1] ||
                               Session["user_id"].ToString() != splitHod[2])//salesman can create new redemption within 3months from last active invoice, 90days
                            {
                                // Jerry 2024-12-04 Allow HOD to create redemption for customer with up to 365 days of inactivities
                                //getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, selected_Account, tuple_getGracePeriod.Item2);
                                getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, selected_Account, tuple_getGracePeriod.Item2, false);
                                // Jerry 2024-12-04 End

                                if (getWarning != "")
                                {
                                    //panelNormal.Visible = false;
                                    Function_Method.MsgBox(getWarning, this.Page, this);
                                    ShowMessageAndRedirect(getWarning, "CustomerMaster.aspx");
                                }
                                else
                                {
                                    Response.Redirect("Redemption.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("Redemption.aspx");
                            }
                        }
                        else
                        {
                            Function_Method.MsgBox("No hod. Please contact administrator.", this.Page, this);
                            Session["data_passing"] = "_RDCM@";
                            Response.Redirect("CustomerMaster.aspx");
                        }
                    }

                    if (temp1.Substring(0, 6) == "_PAC2@")//Request from Payment Invoices > CustomerMaster
                    {
                        string info = temp1.Substring(6);//_PACM@
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@PAC2_" + info + "|" + selected_Account;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_PAC3@")//Request from Payment Statement > CustomerMaster
                    {
                        string info = temp1.Substring(6);//_PACM@
                        string selected_Account = ""; bool statement = true;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@PAC3_" + info + "|" + selected_Account + "|" + statement;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_WCCM@")//Request from WClaim > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@WCCM_" + selected_Account;
                        Response.Redirect("WClaim.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_SQCM@")//Request from SQuotation > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@SQCM_" + selected_Account;
                        Response.Redirect("SalesQuotation.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_EBCM@")//Request from EventBudget > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        GridViewRow row = (GridViewRow)ButtonAccount.NamingContainer;
                        string customername = row.Cells[3].Text;
                        string emplid = row.Cells[6].Text;
                        Session["data_passing"] = "@EBCM_" + selected_Account + "|" + customername + "|" + emplid;
                        Response.Redirect("EventBudget.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_CACM@")//Request from Campaign > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@CACM_" + selected_Account;
                        Response.Redirect("Campaign.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_ICACM")//Request from IncentiveCampaign > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "ICACM_" + selected_Account;
                        Response.Redirect("Campaign_CreateNewOffer.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_SECM@")//Request from SignboardEquipment > CustomerMaster
                    {
                        string selected_Account = "";
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@SECM_" + selected_Account;
                        Response.Redirect("SignboardEquipment.aspx");
                    }
                }
            }
        }

        protected void button_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            AxaptaObject Cust_CreditExposure = DynAx.CreateAxaptaObject("Cust_CreditExposure");
            var recid = Cust_CreditExposure.Call("WebCustCreditExposure", "02085190-T");
        }

        public void ShowMessageAndRedirect(string message, string redirectUrl)
        {
            // Set the session variable
            Session["data_passing"] = "_RDCM@";

            // Prepare the message for the alert box
            message = message.Replace("\r", "").Replace("\n", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace("\\", " ");

            // Create the script to show the alert and redirect
            string script = $"alert('{message}'); window.location='{redirectUrl}';";

            // Register the script on the client side
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alertAndRedirect", script, true);
        }

        protected void Button_Cust_Enquiries_Click(object sender, EventArgs e)
        {
            Button_Cust_Enquiries.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            GridView1.Columns[7].Visible = true; // Point
            GridView1.Columns[8].Visible = false; // Point
            GridView1.Columns[9].Visible = false; // Point
            GridView1.Columns[10].Visible = false; // Point
            GridView1.Columns[11].Visible = false; // Point
            GridView1.Columns[12].Visible = false; // Point
            f_call_ax(0);

        }

        protected void Button_Cust_Campaign_Click(object sender, EventArgs e)
        {
            Response.Redirect("Campaign.aspx");
        }
        protected void Button_ROC_N_TIN_Click(object sender, EventArgs e)
        {
            Response.Redirect("RocTin.aspx?View=All");
        }

        protected void Button_Online_Report_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerMaster_Online_Report.aspx");
        }
    }
}