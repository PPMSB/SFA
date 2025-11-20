using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using GLOBAL_VAR;
using EncryptStringSample;
using GLOBAL_FUNCTION;
using System.Web.UI.HtmlControls;
using MySqlX.XDevAPI.Relational;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.DynamicData;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;




namespace DotNet
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            check_session();
            TimeOutRedirect();

            if (!IsPostBack)
            {
                try
                {
                    string newCustID = Session["NewCustID"] as string;
                    LoadDataFromDatabase();
                }
                catch
                {

                }
            }
            
        }

        protected void GoToCompanyReport(object sender, EventArgs e)
        {
            // Get the newCustID from the command argument
            Button btnCompanyReport = (Button)sender;
            string newCustID = btnCompanyReport.CommandArgument;

            // Check the status for the given newCustID
            string status = GetStatusForNewCustID(newCustID);

            // Redirect based on the status
            if (status == "DRAFT")

            {
                // If status is DRAFT, redirect to AddApplication.aspx
                Response.Redirect("AddApplication.aspx?NewCustID=" + newCustID);
            }
            else if (status == "AWAITING HOD1" || status == "AWAITING HOD2" || status == "AWAITING CREDIT CONTROL" || status == "HODREJECT" || status == "CCREJECT" || status == "AWAITING DOCUMENT" || status == "KIV" || status == "END FOR APPROVAL" || status == "AWAITING CREDIT CONTROL MANAGER")
            {
                // If status is New or Approved, redirect to CompanyReport.aspx
                Response.Redirect("CompanyReport.aspx?NewCustID=" + newCustID);
            }
            // Add more conditions if needed for other status values
        }

        private string GetStatusForNewCustID(string newCustID)
        {
            // Query the database to get the status for the given newCustID
            string connectionString = GLOBAL.connStr;
            string query = $"SELECT status FROM newcust_details WHERE NewCustID = '{newCustID}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        // If result is not null, return the status as a string
                        return result.ToString();
                    }
                }
            }

            // Return an empty string or handle the case where status is not found
            return string.Empty;
        }

        

        // View Draft
        protected void RButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton1.Checked || RButton6.Checked)
            {
                // Only show data with status "DRAFT"
                FilterDataByStatus("DRAFT");
            }
        }

        // View All
        protected void RButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton2.Checked || RButton7.Checked)
            {
                // Show all data
                LoadDataFromDatabase();
            }
        }

        // View New
        protected void RButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton4.Checked || RButton8.Checked)
            {
                // Only show data with status "NEW"
                FilterDataByStatus("NEW");
            }
        }

        // View Awaiting Document
        protected void RButtonAwait_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton10.Checked)
            {
                // Only show data with status "AWAIT"
                FilterDataByStatus("AWAITING DOCUMENT");
            }
        }
        
        // View KIV
        protected void RButtonKIV_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton12.Checked)
            {
                // Only show data with status "KIV"
                FilterDataByStatus("KIV");
            }
        }

        // View End for Approval 
        protected void RButtonEndApprove_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton17.Checked)
            {
                // Only show data with status "END FOR APPROVAL"
                FilterDataByStatus("END FOR APPROVAL");
            }
        }

        // View HOD Reject
        protected void RButtonHODReject_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton18.Checked)
            {
                // Only show data with status "HODREJECT"
                FilterDataByStatus("HODREJECT");
            }
        }

        // View CC Reject
        protected void RButtonCCReject_CheckedChanged(object sender, EventArgs e)
        {
            if (RButton19.Checked)
            {
                // Only show data with status "CCREJECT"
                FilterDataByStatus("CCREJECT");
            }
        }


        private void FilterDataByStatus(string status)
        {
            string connectionString = GLOBAL.connStr;

            // Modify the query to filter data by status
            string query = $"SELECT NewCustID, EmplID, SalesmanName, custName, FormNo, status, coordinate FROM newcust_details WHERE status = '{status}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();

                        connection.Open();
                        adapter.Fill(dataTable);

                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                }
            }
        }

        private void LoadDataFromDatabase()
        {

            string connectionString = GLOBAL.connStr;

            string query = "SELECT NewCustID, EmplID,SalesmanName, custName, FormNo, status, coordinate FROM newcust_details ORDER BY createdDt DESC";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {

                        DataTable dataTable = new DataTable();

                        connection.Open();

                        adapter.Fill(dataTable);

                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                }
            }
        }


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < GridView1.Rows.Count)
            {
                // Get the NewCustID from the row being deleted
                string NewCustID = GridView1.DataKeys[e.RowIndex].Value.ToString();

                // Delete the row from the database
                DeleteRowFromDatabase(NewCustID);



                // Rebind the GridView to show the updated data
                LoadDataFromDatabase();
            }
        }

        private void DeleteRowFromDatabase(string NewCustID)
        {
            
            string connectionString = GLOBAL.connStr;
            string query = $"DELETE FROM newcust_details WHERE NewCustID = '{NewCustID}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
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
               // load session user
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
                Session.Abandon();
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            LoadDataFromDatabase(); // Call the method to load data based on the new page index
            
        }


        protected void DoGPS(object sender, EventArgs e)
        {
            Response.Redirect("DraftCustomerGPS.aspx");
        }

        protected void DoExistCustomer(object sender, EventArgs e)
        {
            Response.Redirect("AddExistCust.aspx");
        }

        protected void DoCompanyReport(object sender, EventArgs e)
        {
            Response.Redirect("CompanyReport.aspx");
        }

        protected void DoAccApplication(object sender, EventArgs e)
        {
            Response.Redirect("AddApplication.aspx");
        }

        

        private bool Check_Admin_Logging(string tempUserName, string tempaxPWD)
        {
            if (tempUserName == "administrator" && tempaxPWD == "administrator123")

            {
                Session["axPWD"] = tempaxPWD;
                Session["logined_user_name"] = "Admin DotNet";
                Session["user_id"] = tempUserName;
                Session["user_authority_lvl"] = 1;
                Session["page_access_authority"] = 0xFFF;
                Session["user_company"] = "PPM";
                Session["module_access_authority"] = 0xFF;
                Session["switch_Company"] = "PPM";
                Session["flag_temp"] = 0;
                Session["system_checking"] = 0;
                Session["data_passing"] = "";
                Session["user_authority_lvl_Red"] = Convert.ToInt32(Session["user_authority_lvl"]);
                Session["logined_user_name_Red"] = Session["logined_user_name"].ToString();
                return true;
            } 
            else
            {
                return false;
            }

        }

       
    }
   
    
}