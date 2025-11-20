using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using iText.Layout;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySqlX.XDevAPI.Relational;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        protected void Button_QTDSalesmanInvPerform_Click(object sender, EventArgs e)
        {
            if(f_Button_EnquiriesPerformance() == false)
            {
                return;
            }
            PopulateYearDropdown();
            PopulateQuarterDropdown(DateTime.Now.Year); // Populate quarters for the current year by default
                                                        // Set the selected index to the first item (0-based index)
            if (ddlQuarter.Items.Count > 0)
            {
                ddlQuarter.SelectedIndex = 0; // Select the first quarter
                ddlQuarter_SelectedIndexChanged(null, null); // Optionally trigger the selected index change event
            }
        }

        private bool f_Button_EnquiriesPerformance()
        {
            if (Dropdown_EnquiriesPerform() == false)//no salesman
            {
                return false;
            }

            Button_SalesmanTotal.Attributes.Add("style", "background-color:transparent");
            SalesmanTotal_section.Visible = false;
            Button_BatteryOutstanding.Attributes.Add("style", "background-color:transparent");
            BatteryOutstanding_section.Visible = false;
            Button_QTDSalesmanInvPerform.Attributes.Add("style", "background-color:#f58345");
            QTDSmenInvoicePerformance_section.Visible = true;

            Button_enquiries_section_accordion.Text = "Quatery Salesman Invoice Performance";
            clear_variable_EnquiriesPerformance();

            //ImageButton1.Visible = false;
            btnGetReport_1.Visible = false;
            return true;
        }

        private void clear_variable_EnquiriesPerformance()
        {
           
        }

        private bool Dropdown_EnquiriesPerform()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            ddlSmenInvoicePerformance.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            try
            {
                items = SFA_GET_Enquiries_QTDSmenInvoicePerform.getSalesman(DynAx);
                if (items.Count > 1)
                {
                    ddlSmenInvoicePerformance.Items.AddRange(items.ToArray());
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

        private void PopulateYearDropdown()
        {
            ddlYear.Items.Clear();
            int currentYear = DateTime.Now.Year;
            // Loop through the last 1 years (including the current year)
            for (int year = currentYear; year >= currentYear - 1; year--)
            {
                ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }

        private void PopulateQuarterDropdown(int selectedYear)
        {
            // Clear existing items
            ddlQuarter.Items.Clear();
            // Loop through quarters
            for (int quarter = 1; quarter <= 4; quarter++)
            {
                // Create the quarter text
                string quarterText = GetQuarterText(selectedYear, quarter);
                ListItem item = new ListItem(quarterText, $"{selectedYear}-{quarter}");
                ddlQuarter.Items.Add(item);
            }
        }

        private string GetQuarterText(int year, int quarter)
        {
            string startMonth = "";
            string endMonth = "";

            switch (quarter)
            {
                case 1:
                    startMonth = "January";
                    endMonth = "March";
                    break;
                case 2:
                    startMonth = "April";
                    endMonth = "June";
                    break;
                case 3:
                    startMonth = "July";
                    endMonth = "September";
                    break;
                case 4:
                    startMonth = "October";
                    endMonth = "December";
                    break;
            }

            return $"Q{quarter} - {startMonth} to {endMonth}";
        }


        private bool QTDSalesmanInvPerform()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            try
            {
                return true;
            }
            catch(Exception ER_SF_29) 
            {
                Function_Method.MsgBox("ER_SF_29: " + ER_SF_29.ToString(), this.Page, this);
                return false;
            }
        }

        protected void OnSelectedIndexChanged_ddlSmenInvoicePerformance(object sender, EventArgs e)
        {
            if (ddlSmenInvoicePerformance.SelectedItem.Value != "")//Only selected
            {
                string SelectedItem = ddlSmenInvoicePerformance.SelectedItem.Text;
                //string[] arr_SelectedItem = SelectedItem.Split(')');
                //int temp_length_arr_JournalName = arr_SelectedItem[0].Length + 1;// plus 1 ")"
                //string getDescription = SelectedItem.Substring(temp_length_arr_JournalName);//getDescription
                string getDescription = SelectedItem;

                btnGetReport_1.Visible = true;
            }
            else
            {
                //ImageButton1.Visible = false;
                btnGetReport_1.Visible = false;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedYear = int.Parse(ddlYear.SelectedValue);
            PopulateQuarterDropdown(selectedYear); // Populate quarters based on the selected year
            ddlQuarter_SelectedIndexChanged(sender, e); // Pass the sender and event argsddlQuarter_SelectedIndexChanged();
        }
        protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected value from the quarter dropdown
            string selectedValue = ddlQuarter.SelectedValue; // e.g., "2025-2"
            string[] parts = selectedValue.Split('-');
            int year = int.Parse(parts[0]);
            int quarter = int.Parse(parts[1]);
            // Calculate the first and last days of the quarter
            DateTime firstDayOfQuarter = new DateTime(year, (quarter - 1) * 3 + 1, 1); // First month of the quarter
            DateTime lastDayOfQuarter = new DateTime(year, quarter * 3, DateTime.DaysInMonth(year, quarter * 3)); // Last month of the quarter
                                                                                                                  // Update the labels with the calculated dates
            lblQuarterStartDate.Text = firstDayOfQuarter.ToString("dd/MM/yyyy");
            lblQuarterEndDate.Text = lastDayOfQuarter.ToString("dd/MM/yyyy");
            // Optionally, you can set the hidden field value if needed
            hdnSelectedQuarterRange.Value = $"{firstDayOfQuarter:dd/MM/yyyy} - {lastDayOfQuarter:dd/MM/yyyy}";
        }
        protected void Button_Generate_SmenInvoicePerformance_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            try
            {
                DateTime now = DateTime.Now;
                string SalesmanID = ddlSmenInvoicePerformance.SelectedItem.Value.ToString();
                QuaterSalesReport(DynAx, SalesmanID, now);
            }
            catch (Exception ER_SF_29)
            {
                Function_Method.MsgBox("ER_SF_29: " + ER_SF_29.ToString(), this.Page, this);

            }
        }

        protected void QuaterSalesReport(Axapta DynAx, string SalesmanID, DateTime now)
        {

            #region GetDateTime
            var firstDayOfMonth = lblQuarterStartDate.Text;
            var lastDayOfMonth = lblQuarterEndDate.Text;

            // Assuming lblQuarterStartDate and lblQuarterEndDate are already set
            DateTime firstDayOfQuarter = DateTime.ParseExact(lblQuarterStartDate.Text, "dd/MM/yyyy", null);
            DateTime lastDayOfQuarter = DateTime.ParseExact(lblQuarterEndDate.Text, "dd/MM/yyyy", null);

            // Get the first day of the first month
            var firstMonthLastDay = new DateTime(firstDayOfQuarter.Year, firstDayOfQuarter.Month, DateTime.DaysInMonth(firstDayOfQuarter.Year, firstDayOfQuarter.Month));

            // Get the first day of the second month
            var secondMonthFirstDay = new DateTime(firstDayOfQuarter.Year, firstDayOfQuarter.Month + 1, 1);
            // Get the last day of the second month
            var secondMonthLastDay = new DateTime(firstDayOfQuarter.Year, firstDayOfQuarter.Month + 1, DateTime.DaysInMonth(firstDayOfQuarter.Year, firstDayOfQuarter.Month + 1));

            var thirdMonthFirstDay = new DateTime(firstDayOfQuarter.Year, firstDayOfQuarter.Month + 2, 1);

            // Get the last day of the third month
            var thirdMonthLastDay = new DateTime(firstDayOfQuarter.Year, firstDayOfQuarter.Month + 2, DateTime.DaysInMonth(firstDayOfQuarter.Year, firstDayOfQuarter.Month + 2));

            // Format the last days as strings
            var firstMonthLastDayString = firstMonthLastDay.ToString("dd/MM/yyyy");
            var secondMonthLastDayString = secondMonthLastDay.ToString("dd/MM/yyyy");
            var thirdMonthLastDayString = thirdMonthLastDay.ToString("dd/MM/yyyy");

            // Get the year from the quarter dates (using first day as reference)
            int currentYear = firstDayOfQuarter.Year;
            int previousYear = currentYear - 1;
            // Create dates for Jan 1st and Dec 31st of previous year
            DateTime jan1stPreviousYear = new DateTime(previousYear, 1, 1);
            DateTime dec31stPreviousYear = new DateTime(previousYear, 12, 31);
            // Format as strings if needed
            string jan1stFormatted = jan1stPreviousYear.ToString("dd/MM/yyyy");
            string dec31stFormatted = dec31stPreviousYear.ToString("dd/MM/yyyy");

            #endregion

            lblDescribesQtyInvPerf.Text = "Following describes the Sales Report for the following Sales ID on ";
            lblDateQtyInvPerf.Text = firstDayOfMonth + " to " + lastDayOfMonth;

            try
            {
                DataTable dt = new DataTable();
                int data_count = 13;
                string[] N = new string[data_count];

                N[0] = "Sales ID"; N[1] = "Sman Name"; N[2] = "Actual Total (QTD)";
                N[3] = "Target"; N[4] = "Invoiced Sales (#Customer)";
                N[5] = "Budget"; N[6] = "Balance vs T1"; N[7] = "Balance vs T2";
                N[8] = "Actual (firstM)"; N[9] = "Actual (seconM)"; N[10] = "Actual (thirdM)";
                N[11] = "Actual Last Year Sales"; N[12] = "Category";


                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }

                var getEmplDetails = SalesReport_Get_Budget.getEmpDetails(DynAx, SalesmanID);

                var getEmail = SalesReport_Get_Budget.getEmpDetails2(DynAx, getEmplDetails.Item6);

                List<double> totalInvoicedSales = new List<double>();
                List<double> totalInvoicedSalesFirst = new List<double>();
                List<double> totalInvoicedSalesSecond = new List<double>();
                List<double> totalInvoicedSalesThird = new List<double>();
                List<int> SumMonthlyBudget = new List<int>();
                List<double> totalLastYearSales = new List<double>();

                DataRow row;

                string thirdmonthlyPeriodName = SalesReport_Get_Budget.getStartDateToDate(DynAx, thirdMonthLastDayString);
                string secondmonthlyPeriodName = SalesReport_Get_Budget.getStartDateToDate(DynAx, secondMonthLastDayString);
                string firstmonthlyPeriodName = SalesReport_Get_Budget.getStartDateToDate(DynAx, firstMonthLastDayString);

                bool AllowDisplay = false; // IF found 1002-2 or 1002-3

                for (int i = 0; i < getEmail.Item4; i++)//count salesman email
                {
                    string empId = getEmail.Item3[i]; 

                    // Check if the employee ID starts with "1002" or "1003"
                    if (empId.StartsWith("1002") || empId.StartsWith("1003"))
                    {
                        // Allow these IDs to proceed
                        AllowDisplay = true;
                    }
                    else
                    {
                        // For other IDs, skip if they end with "-2", "-3", or "-4"
                        if (empId.EndsWith("-2") || empId.EndsWith("-3") || empId.EndsWith("-4"))
                        {
                            continue; // Skip this iteration
                        }
                    }

                    var tuple_get_firstMonthTotal = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s
                    (DynAx, getEmail.Item3[i], firstDayOfMonth, firstMonthLastDayString);

                    var tuple_get_SeconMonthTotal = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s
                        (DynAx, getEmail.Item3[i], secondMonthFirstDay.ToString("dd/MM/yyyy"), secondMonthLastDayString);

                    var tuple_get_ThirdMonthTotal = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s
                        (DynAx, getEmail.Item3[i], thirdMonthFirstDay.ToString("dd/MM/yyyy"), thirdMonthLastDayString);


                    var firstmonthlyBudget = SalesReport_Get_Budget.getPeriodName(DynAx, firstmonthlyPeriodName, getEmail.Item3[i]);
                    var secondmonthlyBudget = SalesReport_Get_Budget.getPeriodName(DynAx, secondmonthlyPeriodName, getEmail.Item3[i]);
                    var thirdmonthlyBudget = SalesReport_Get_Budget.getPeriodName(DynAx, thirdmonthlyPeriodName, getEmail.Item3[i]);

                    int totalmonthlyBudget = Convert.ToInt32(firstmonthlyBudget.Item2) + Convert.ToInt32(secondmonthlyBudget.Item2) + Convert.ToInt32(thirdmonthlyBudget.Item2);


                    ///QTD Total
                    var tuple_get_QuaterRangeTotal = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s
                       (DynAx, getEmail.Item3[i], firstDayOfMonth, lastDayOfMonth);

                    ///Last Year
                    var tuple_get_LastYearTotal = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s
                        (DynAx, getEmail.Item3[i], jan1stFormatted, dec31stFormatted);


                    var getCustAcc = Payment_GET_Ledger_Journal_Table.get_CustTable(DynAx, getEmail.Item3[i]);

                    row = dt.NewRow();
                    row["Sales ID"] = getEmail.Item3[i];

                    string originalName = getEmplDetails.Item1;
                    //string cleaned = Regex.Replace(originalName, @"\s*\(TRANSFER\)", "", RegexOptions.IgnoreCase);
                    //string cleanedName = Regex.Replace(cleaned, @"\s{2,}", " ").Trim();

                    row["Sman Name"] = originalName;
                    row["Actual Total (QTD)"] = "<br>" + tuple_get_QuaterRangeTotal.Item1.ToString("#,###,###,##0") + "<br> <br> ";
                    row["Target"] = "<br>" + totalmonthlyBudget.ToString("#,###,###,##0") + "<br> <br>";

                    row["Actual (firstM)"] = "<br>" + tuple_get_firstMonthTotal.Item1.ToString("#,###,###,##0") + "<br> <br>";
                    row["Actual (seconM)"] = "<br>" + tuple_get_SeconMonthTotal.Item1.ToString("#,###,###,##0") + "<br> <br>";
                    row["Actual (thirdM)"] = "<br>" + tuple_get_ThirdMonthTotal.Item1.ToString("#,###,###,##0") + "<br> <br>";

                    SumMonthlyBudget.Add(Convert.ToInt32(totalmonthlyBudget));

                    totalInvoicedSales.Add(tuple_get_QuaterRangeTotal.Item1);
                    totalInvoicedSalesFirst.Add(tuple_get_firstMonthTotal.Item1);
                    totalInvoicedSalesSecond.Add(tuple_get_SeconMonthTotal.Item1);
                    totalInvoicedSalesThird.Add(tuple_get_ThirdMonthTotal.Item1);

                    totalLastYearSales.Add(tuple_get_LastYearTotal.Item1);

                    dt.Rows.Add(row);
                }

                row = dt.NewRow();

                row["Sales ID"] = "Total";

                //int getInvoiceSales = totalInvoicedSales.Where((value, index) => index != 2).Sum();
                #region Calculation


                int getSumMonthlyBudget = SumMonthlyBudget.Sum();

                double getInvoicedSalesFirstDecimal = totalInvoicedSalesFirst.Sum();
                double getInvoicedSalesSecondDecimal = totalInvoicedSalesSecond.Sum();
                double getInvoicedSalesThirdDecimal = totalInvoicedSalesThird.Sum();

                int getInvoicedSalesFirst = Convert.ToInt32(getInvoicedSalesFirstDecimal);
                int getInvoicedSalesSecond = Convert.ToInt32(getInvoicedSalesSecondDecimal);
                int getInvoicedSalesThird = Convert.ToInt32(getInvoicedSalesThirdDecimal);


                double getInvoiceSalesDecimal = 0.0;
                //double getLastYearSalesDecimal = 0.0;
                getInvoiceSalesDecimal = totalInvoicedSales.Sum();
               

                int getInvoiceSales = Convert.ToInt32(getInvoiceSalesDecimal);
                //int getLastYearSales = Convert.ToInt32(getLastYearSalesDecimal);
                #region LastYearSales


                double getLastYearSales = SFA_GET_Enquiries_QTDSmenInvoicePerform.getLF_PPM_SalesInc_Master(DynAx, previousYear + "-" + previousYear, getEmplDetails.Item6);

                #endregion
                #endregion
                #region GetCategory
                // Call the method to get sales data
                var salesData = SFA_GET_Enquiries_QTDSmenInvoicePerform.getLF_PPM_SalesInvCategory(DynAx, currentYear + "-" + currentYear);
                // Initialize the category variable
                string category = "A"; // Default category
                                       // Check the ValueFrom from the sales data to determine the category
                foreach (var item in salesData)
                {
                    double valueFrom = item.Item1; // ValueFrom
                    string currentCategory = item.Item2; // Category
                                                         // Determine the category based on getLastYearSales
                    if (getLastYearSales > valueFrom)
                    {
                        category = currentCategory; // Assign the category if the condition is met
                    }
                }

                row["Category"] = "<br>" + category + "<br> <br>";

                #endregion
                row["Sman Name"] = "<br><span style='color:red; font-weight:bold;'>Please refer to Sales Admin for FINAL Value Confirmation</span><br><br>";
                row["Actual Total (QTD)"] = "<br>" + getInvoiceSales.ToString("#,###,###,##0") + "**<br> <br>";
                row["Target"] = "<br>" + getSumMonthlyBudget.ToString("#,###,###,##0") + "**<br> <br>";

                //< span style = 'color: red;' > (Exclude - 2) </ span >
                #region Balance T1
                int getBalanceT1 = getInvoiceSales - getSumMonthlyBudget;
                row["Balance vs T1"] = "<br>" +
                    (getBalanceT1 < 0
                        ? $"<span style='color: red;'>({getBalanceT1.ToString("#,###,###,##0").Replace("-", "")})</span>"
                        : getBalanceT1.ToString("#,###,###,##0")) +
                    "<br> <br>";
                #endregion
                #region Balance T2

                double getLastYearActualQuarterSalesDecimal = getLastYearSales / 12 * 3;
                int get90PercentTarget = getSumMonthlyBudget * 9 / 10;

                // Use the ternary operator to assign T2
                double T2 = (get90PercentTarget > getLastYearActualQuarterSalesDecimal) ? get90PercentTarget : getLastYearActualQuarterSalesDecimal;
                double getBalanceT2Decimal = (category != "A") ? (getInvoiceSalesDecimal - T2) : 0;
                int getBalanceT2 = Convert.ToInt32(getBalanceT2Decimal);

                // Format the output for Balance vs T2
                row["Balance vs T2"] = "<br>" +
                    (getBalanceT2 < 0
                        ? $"<span style='color: red;'>({getBalanceT2.ToString("#,###,###,##0").Replace("-", "")})</span>"
                        : getBalanceT2.ToString("#,###,###,##0")) +
                    "<br> <br>";

                #endregion

                row["Actual Last Year Sales"] = "<br>" + getLastYearSales.ToString("#,###,###,##0") + "<br> <br>";
                //row["Balance vs T2"] = ;


                row["Actual (firstM)"] = "<br>" + getInvoicedSalesFirst.ToString("#,###,###,##0") + "<br> <br>";
                row["Actual (seconM)"] = "<br>" + getInvoicedSalesSecond.ToString("#,###,###,##0") + "<br> <br>";
                row["Actual (thirdM)"] = "<br>" + getInvoicedSalesThird.ToString("#,###,###,##0") + "<br> <br>";

                dt.Rows.Add(row);
                gvReport.DataSource = dt;
                gvReport.DataBind();

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void gvReport_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DateTime startDate = DateTime.Parse(lblQuarterStartDate.Text);
                DateTime endDate = DateTime.Parse(lblQuarterEndDate.Text);
                // Check if the dates correspond to the second quarter
                if (startDate.Month == 4 && endDate.Month == 6)
                {
                    e.Row.Cells[6].Text = "Actual (April)";  // Actual (firstM)
                    e.Row.Cells[7].Text = "Actual (May)";    // Actual (seconM)
                    e.Row.Cells[8].Text = "Actual (June)";   // Actual (thirdM)
                }else if (startDate.Month == 1 && endDate.Month == 3)
                {
                    e.Row.Cells[6].Text = "Actual (Jan)";  // Actual (firstM)
                    e.Row.Cells[7].Text = "Actual (Feb)";    // Actual (seconM)
                    e.Row.Cells[8].Text = "Actual (March)";   // Actual (thirdM)
                }
                else if (startDate.Month == 7 && endDate.Month == 9)
                {
                    e.Row.Cells[6].Text = "Actual (July)";  // Actual (firstM)
                    e.Row.Cells[7].Text = "Actual (Aug)";    // Actual (seconM)
                    e.Row.Cells[8].Text = "Actual (Sept)";   // Actual (thirdM)
                }
                else 
                {
                    e.Row.Cells[6].Text = "Actual (Oct)";  // Actual (firstM)
                    e.Row.Cells[7].Text = "Actual (Nov)";    // Actual (seconM)
                    e.Row.Cells[8].Text = "Actual (Dec)";   // Actual (thirdM)
                }
                // You can add more conditions for other quarters if needed
            }
        }

    }
}
