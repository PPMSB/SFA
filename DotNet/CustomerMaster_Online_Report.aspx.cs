using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class CustomerMaster_Online_Report : System.Web.UI.Page
    {
        public string fromDate;
        public string toDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            
            if (!IsPostBack)
            {
                clear_variable();
                clear_variable_SalesmanTotal();
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
                
                //first time, reload gridview with Search ALL approach
                GridView1.PageIndex = 0;
                //f_call_ax(0);
                Dropdown_SalesManList();
            }
            
        }

        #region Page_Load
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
            //f_call_ax(0);
        }

        private void clear_variable()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        private void clear_variable_SalesmanTotal()
        {
            // Declare variables to hold the dates
            string fromDate;
            string toDate;
            // Retrieve dates from cookies if they exist
            if (Request.Cookies["CustomerMaster_Online_Report"] != null)
            {
                fromDate = Request.Cookies["CustomerMaster_Online_Report"]["StartDate"];
                toDate = Request.Cookies["CustomerMaster_Online_Report"]["EndDate"];
            }
            else
            {
                // Set default values if no cookies are found
                // Set start date to first day of current month
                DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                fromDate = firstDayOfMonth.ToString("yyyy-MM-dd"); // Format for HTML5 date input
                                                                   // Set end date to last day of current month
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                toDate = lastDayOfMonth.ToString("yyyy-MM-dd"); // Format for HTML5 date input
            }

            txtStartDate.Value = fromDate; // Assuming txtStartDate is an <input type="date">
            txtEndDate.Value = toDate;     // Assuming txtEndDate is an <input type="date">
        }
        #endregion
        private void f_call_ax(int PAGE_INDEX)
        {

            Axapta DynAx = new Axapta();
            string getEmplId = DropDownList2.SelectedItem.Value.ToString();
            string temp_EmpID_1 = getEmplId + "-1";
            string temp_EmpID_LEGAL = getEmplId + "-LEGAL";
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                var getCustAcc = getAllCustomer(DynAx, getEmplId);

                // Get the dictionary of customer accounts (account number -> customer name)
                Dictionary<string, string> custAccDict = getCustAcc.Item1;
                List<string> custAccList = custAccDict.Keys.ToList(); // Get account numbers only

                var getCustAcc_1 = getAllCustomer(DynAx, temp_EmpID_1);

                Dictionary<string, string> custAccDict_1 = getCustAcc_1.Item1;
                List<string> custAccList_1 = custAccDict_1.Keys.ToList(); // Get account numbers only

                var getCustAcc_LEGAL = getAllCustomer(DynAx, temp_EmpID_LEGAL);

                Dictionary<string, string> custAccDict_LEGAL = getCustAcc_LEGAL.Item1;
                List<string> custAccList_LEGAL = custAccDict_LEGAL.Keys.ToList(); // Get account numbers only

                //int LF_TyreProvision = 30719; // LF_TyreProvision table ID
                //int CustAccount = 30001;
                int LF_TyreProvision = DynAx.GetTableIdWithLock("LF_TyreProvBalance"); // LF_TyreProvision table ID
                
                int CustAccount = DynAx.GetFieldIdWithLock(LF_TyreProvision, "CustAccount");
                int ProcessDate = DynAx.GetFieldIdWithLock(LF_TyreProvision, "ProcessDate");
                int StartDateField = DynAx.GetFieldIdWithLock(LF_TyreProvision, "StartDate");
                int EndDateField = DynAx.GetFieldIdWithLock(LF_TyreProvision, "EndDate");

                GridView1.DataSource = null;
                GridView1.DataBind();
                GridView1.PageIndex = 0;

                // Prepare the DataTable with the required columns
                DataTable dt = new DataTable();
                int data_count = 11;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Salesman ID"; N[2] = "Customer Code";
                N[3] = "Customer Name"; N[4] = "Start Date"; N[5] = "End Date";
                N[6] = "Open Point"; N[7] = "Earn Point"; N[8] = "Used Point";
                N[9] = "Adjust Point"; N[10] = "Balance";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }

                int countA = 0;
                List<double> totalBalance = new List<double>();

                string startDate = txtStartDate.Value;
                string endDate = txtEndDate.Value;

                // Declare the variables as nullable DateTime
                var var_StartQDate = (DateTime?)null;
                var var_EndQDate = (DateTime?)null;
                // Check if the dates are not null or empty
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    // Parse the dates using the correct format
                    var_StartQDate = DateTime.ParseExact(startDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    var_EndQDate = DateTime.ParseExact(endDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                    // Now you can use var_StartQDate and var_EndQDate as needed
                }
                var var_Qdate = var_StartQDate + ".." + var_EndQDate;

                #region custAccList merge
                foreach (string custAcc in custAccList) // Loop through each customer account
                {
                    if (!string.IsNullOrEmpty(custAcc))
                    {
                        // Get customer name from the dictionary
                        string temp_Cust_Name = custAccDict.ContainsKey(custAcc) ? custAccDict[custAcc] : "Unknown Customer";

                        AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_TyreProvision);

                        // Set the range for the current `custAcc`
                        var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", CustAccount); // ACCOUNTNUM
                        qbr_0_1.Call("value", custAcc);

                        var qbr0_2 = (AxaptaObject)axQueryDataSource.Call("addRange", StartDateField);//StartDate
                        qbr0_2.Call("value", var_StartQDate + "..");

                        var qbr0_3 = (AxaptaObject)axQueryDataSource.Call("addRange", EndDateField);//EndDate
                        qbr0_3.Call("value", ".." + var_EndQDate);

                        axQueryDataSource.Call("addSortField", ProcessDate, 0); // ProcessDate, descending
                        AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                        // Loop through the records for the current `custAcc`
                        while ((bool)axQueryRun.Call("next"))
                        {
                            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_TyreProvision);

                            string StartDate = DynRec.get_Field("StartDate").ToString() != "" ? DateTime.Parse(DynRec.get_Field("StartDate").ToString()).ToString(GLOBAL.gDisplayDateFormat) : "";
                            string EndDate = DynRec.get_Field("EndDate").ToString() != "" ? DateTime.Parse(DynRec.get_Field("EndDate").ToString()).ToString(GLOBAL.gDisplayDateFormat) : "";

                            //double OpenPoint = Convert.ToDouble(DynRec.get_Field("PointOpen").ToString());
                            //double EarnPoint = Convert.ToDouble(DynRec.get_Field("PointEarn").ToString());
                            //double UsedPoint = Convert.ToDouble(DynRec.get_Field("PointUsed").ToString());
                            //double AdjustPoint = Convert.ToDouble(DynRec.get_Field("PointAdj").ToString());
                            //double Balance = Convert.ToDouble(DynRec.get_Field("PointBalance").ToString());
                            //Neil - Request from Keegan - Change collumn get data - 15/10/2025
                            double OpenPoint = Convert.ToDouble(DynRec.get_Field("ValueOpen").ToString());
                            double EarnPoint = Convert.ToDouble(DynRec.get_Field("ValueEarn").ToString());
                            double UsedPoint = Convert.ToDouble(DynRec.get_Field("ValueUsed").ToString());
                            double AdjustPoint = Convert.ToDouble(DynRec.get_Field("ValueAdj").ToString());
                            double Balance = Convert.ToDouble(DynRec.get_Field("ValueBalance").ToString());
                            totalBalance.Add(Balance);

                            countA++;
                            DataRow row = dt.NewRow();
                            row["No."] = countA;
                            row["Salesman ID"] = "(" + getEmplId + ") " + EOR_GET_NewApplicant.getEmpName(DynAx, getEmplId);
                            row["Customer Code"] = custAcc;
                            row["Customer Name"] = temp_Cust_Name;

                            row["Start Date"] = StartDate;
                            row["End Date"] = EndDate;
                            row["Open Point"] = OpenPoint.ToString("#,###,###,##0.00");
                            row["Earn Point"] = EarnPoint.ToString("#,###,###,##0.00");
                            row["Used Point"] = UsedPoint.ToString("#,###,###,##0.00");
                            row["Adjust Point"] = AdjustPoint.ToString("#,###,###,##0.00");
                            row["Balance"] = Balance.ToString("#,###,###,##0.00");

                            dt.Rows.Add(row);
                        }
                    }
                }
                #endregion

                #region custAccList_1 merge
                foreach (string custAcc in custAccList_1) // Loop through each customer account
                {
                    if (!string.IsNullOrEmpty(custAcc))
                    {
                        // Get customer name from the dictionary
                        string temp_Cust_Name = custAccDict_1.ContainsKey(custAcc) ? custAccDict_1[custAcc] : "Unknown Customer";

                        AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_TyreProvision);

                        // Set the range for the current `custAcc`
                        var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", CustAccount); // ACCOUNTNUM
                        qbr_0_1.Call("value", custAcc);

                        var qbr0_2 = (AxaptaObject)axQueryDataSource.Call("addRange", StartDateField);//StartDate
                        qbr0_2.Call("value", var_StartQDate + "..");

                        var qbr0_3 = (AxaptaObject)axQueryDataSource.Call("addRange", EndDateField);//EndDate
                        qbr0_3.Call("value", ".." + var_EndQDate);

                        axQueryDataSource.Call("addSortField", ProcessDate, 0); // ProcessDate, descending
                        AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                        // Loop through the records for the current `custAcc`
                        while ((bool)axQueryRun.Call("next"))
                        {
                            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_TyreProvision);

                            string StartDate = DynRec.get_Field("StartDate").ToString() != "" ? DateTime.Parse(DynRec.get_Field("StartDate").ToString()).ToString(GLOBAL.gDisplayDateFormat) : "";
                            string EndDate = DynRec.get_Field("EndDate").ToString() != "" ? DateTime.Parse(DynRec.get_Field("EndDate").ToString()).ToString(GLOBAL.gDisplayDateFormat) : "";

                            double OpenPoint = Convert.ToDouble(DynRec.get_Field("PointOpen").ToString());
                            double EarnPoint = Convert.ToDouble(DynRec.get_Field("PointEarn").ToString());
                            double UsedPoint = Convert.ToDouble(DynRec.get_Field("PointUsed").ToString());
                            double AdjustPoint = Convert.ToDouble(DynRec.get_Field("PointAdj").ToString());
                            double Balance = Convert.ToDouble(DynRec.get_Field("PointBalance").ToString());
                            totalBalance.Add(Balance);

                            countA++;
                            DataRow row = dt.NewRow();
                            row["No."] = countA;
                            row["Salesman ID"] = "(" + temp_EmpID_1 + ") " + EOR_GET_NewApplicant.getEmpName(DynAx, temp_EmpID_1);
                            row["Customer Code"] = custAcc;
                            row["Customer Name"] = temp_Cust_Name;

                            row["Start Date"] = StartDate;
                            row["End Date"] = EndDate;
                            row["Open Point"] = OpenPoint.ToString("#,###,###,##0.00");
                            row["Earn Point"] = EarnPoint.ToString("#,###,###,##0.00");
                            row["Used Point"] = UsedPoint.ToString("#,###,###,##0.00");
                            row["Adjust Point"] = AdjustPoint.ToString("#,###,###,##0.00");
                            row["Balance"] = Balance.ToString("#,###,###,##0.00");

                            dt.Rows.Add(row);
                        }
                    }
                }
                #endregion

                #region custAccList_LEGAL merge
                foreach (string custAcc in custAccList_LEGAL) // Loop through each customer account
                {
                    if (!string.IsNullOrEmpty(custAcc))
                    {
                        // Get customer name from the dictionary
                        string temp_Cust_Name = custAccDict_LEGAL.ContainsKey(custAcc) ? custAccDict_LEGAL[custAcc] : "Unknown Customer";

                        AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_TyreProvision);

                        // Set the range for the current `custAcc`
                        var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", CustAccount); // ACCOUNTNUM
                        qbr_0_1.Call("value", custAcc);

                        var qbr0_2 = (AxaptaObject)axQueryDataSource.Call("addRange", StartDateField);//StartDate
                        qbr0_2.Call("value", var_StartQDate + "..");

                        var qbr0_3 = (AxaptaObject)axQueryDataSource.Call("addRange", EndDateField);//EndDate
                        qbr0_3.Call("value", ".." + var_EndQDate);

                        axQueryDataSource.Call("addSortField", ProcessDate, 0); // ProcessDate, descending
                        AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                        // Loop through the records for the current `custAcc`
                        while ((bool)axQueryRun.Call("next"))
                        {
                            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_TyreProvision);

                            string StartDate = DynRec.get_Field("StartDate").ToString() != "" ? DateTime.Parse(DynRec.get_Field("StartDate").ToString()).ToString(GLOBAL.gDisplayDateFormat) : "";
                            string EndDate = DynRec.get_Field("EndDate").ToString() != "" ? DateTime.Parse(DynRec.get_Field("EndDate").ToString()).ToString(GLOBAL.gDisplayDateFormat) : "";

                            double OpenPoint = Convert.ToDouble(DynRec.get_Field("PointOpen").ToString());
                            double EarnPoint = Convert.ToDouble(DynRec.get_Field("PointEarn").ToString());
                            double UsedPoint = Convert.ToDouble(DynRec.get_Field("PointUsed").ToString());
                            double AdjustPoint = Convert.ToDouble(DynRec.get_Field("PointAdj").ToString());
                            double Balance = Convert.ToDouble(DynRec.get_Field("PointBalance").ToString());
                            totalBalance.Add(Balance);

                            countA++;
                            DataRow row = dt.NewRow();
                            row["No."] = countA;
                            row["Salesman ID"] = "(" + temp_EmpID_LEGAL + ") " + EOR_GET_NewApplicant.getEmpName(DynAx, temp_EmpID_LEGAL);
                            row["Customer Code"] = custAcc;
                            row["Customer Name"] = temp_Cust_Name;

                            row["Start Date"] = StartDate;
                            row["End Date"] = EndDate;
                            row["Open Point"] = OpenPoint.ToString("#,###,###,##0.00");
                            row["Earn Point"] = EarnPoint.ToString("#,###,###,##0.00");
                            row["Used Point"] = UsedPoint.ToString("#,###,###,##0.00");
                            row["Adjust Point"] = AdjustPoint.ToString("#,###,###,##0.00");
                            row["Balance"] = Balance.ToString("#,###,###,##0.00");

                            dt.Rows.Add(row);
                        }
                    }
                }
                #endregion

                #region Calculate total balance
                double totalBalanceSum = totalBalance.Sum();
                // Add a total balance row
                if (totalBalanceSum > 0)
                {
                    DataRow totalRow = dt.NewRow();
                    totalRow["No."] = "Total"; // You can customize this as needed
                    totalRow["Salesman ID"] = ""; // Leave blank or customize
                    totalRow["Customer Code"] = ""; // Leave blank or customize
                    totalRow["Customer Name"] = ""; // Leave blank or customize
                    totalRow["Start Date"] = ""; // Leave blank or customize
                    totalRow["End Date"] = ""; // Leave blank or customize
                    totalRow["Open Point"] = ""; // Leave blank or customize
                    totalRow["Earn Point"] = ""; // Leave blank or customize
                    totalRow["Used Point"] = ""; // Leave blank or customize
                    totalRow["Adjust Point"] = ""; // Leave blank or customize
                    totalRow["Balance"] = totalBalanceSum.ToString("#,###,###,##0.00"); // Set total balance
                    dt.Rows.Add(totalRow); // Add the total row to the DataTable
                }
                #endregion

                // Set the VirtualItemCount and bind the DataTable to the GridView
                GridView1.VirtualItemCount = countA;
                GridView1.DataSource = dt;
                GridView1.DataBind();

            }
            catch (Exception ex)

            {
                // Log the exception or handle it
                Console.WriteLine(ex.Message); // Example logging
            }

            finally
            {
                DynAx.Logoff();
            }
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

        #region Button
        protected void button_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            AxaptaObject Cust_CreditExposure = DynAx.CreateAxaptaObject("Cust_CreditExposure");
            var recid = Cust_CreditExposure.Call("WebCustCreditExposure", "02085190-T");
        }

        protected void Button_Search_ProvisionReport_Click(object sender, EventArgs e)
        {
            // Create or retrieve the cookie
            HttpCookie dateCookie = new HttpCookie("CustomerMaster_Online_Report"); // SomePage eg: Quotation, Redemption, etc

            // Set the cookie values
            dateCookie["StartDate"] = txtStartDate.Value;
            dateCookie["EndDate"] = txtEndDate.Value;

            // Set the cookie expiration (optional)
            dateCookie.Expires = DateTime.Now.AddDays(2); // Set cookie to expire in 20 days
            Response.Cookies.Add(dateCookie); // Add the cookie to the response

            //HttpContext.Current.Response.Cookies.Add(dateCookie);
            // Call your existing method
            f_call_ax(0);

        }

        protected void Button_Online_Report_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerMaster.aspx");
        }
        #endregion

        #region DropDown
        protected void OnSelectedIndexChanged_DropDownList2(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedItem.Value != "")//Only selected
            {
                string SelectedItem = DropDownList2.SelectedItem.Text;
                string getDescription = SelectedItem;
                //ImageButton8.Visible = true;
                btnGetReport_1.Visible= true;
            }
            else
            {
                //ImageButton8.Visible = false;
                btnGetReport_1.Visible = false;
            }

        }

        private bool Dropdown_SalesManList()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            DropDownList2.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            try
            {
                items = SFA_GET_Enquiries_QTDSmenInvoicePerform.getSalesman(DynAx);
                if (items.Count > 1)
                {
                    DropDownList2.Items.AddRange(items.ToArray());
                    return true;
                }
                else
                {
                    Function_Method.MsgBox("There is no Salesman available.", this.Page, this);
                    return false;
                }
            }
            catch (Exception ER_SF_26)
            {
                Function_Method.MsgBox("ER_SF_26: " + ER_SF_26.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }
        #endregion

        #region GetAxapata_Data
        public static Tuple<Dictionary<string, string>, string, int> getAllCustomer(Axapta DynAx, string temp_EmpId)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();

            Dictionary<string, string> temp_Cust_Account = new Dictionary<string, string>();
            string temp_Emp_Id = "";
            int count = 0;

            int CustTable = 77;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", CustTable);

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30033); // EmplId
            qbr2.Call("value", temp_EmpId);

            var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7); // CustGroup
            qbr4_2.Call("value", "TDI");

            var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_3.Call("value", "TDE");

            var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_4.Call("value", "TDO");

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                if (temp_Cust_Account.Count >= 500)
                {
                    break;
                }
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTable);

                temp_Cust_Account.Add(DynRec1.get_Field("AccountNum").ToString(), DynRec1.get_Field("Name").ToString());
                temp_Emp_Id = DynRec1.get_Field("EmplId").ToString();

                count++;
                DynRec1.Dispose();
            }

            axQuery1.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun1.Dispose();

            return new Tuple<Dictionary<string, string>, string, int>(temp_Cust_Account, temp_Emp_Id, count);
        }
        #endregion
    }
}