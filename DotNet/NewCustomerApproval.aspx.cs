using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class NewCustomerApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                BindGridViewData();
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
                Session.Abandon();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void BindGridViewData()
        {
            try
            {
                // Connect to MySQL database
                using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                {
                    connection.Open();

                    // Create SQL query to retrieve data from newcust_department_access table
                    string query = "SELECT * FROM newcust_department_access";

                    // Execute query and fill the DataTable
                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }

                    // Bind data to the GridView
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}<br />");
                Response.Write($"Stack Trace: {ex.StackTrace}<br />");
                Response.Write($"Inner Exception: {ex.InnerException?.Message}<br />");
                Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Hide GridView and show DropDownList controls
            GridView1.Visible = false;
            ddlCreditControlAdmin.Visible = true;
            ddlCreditControlManager.Visible = true;
            lblCreditControlAdmin.Visible = true;
            lblCreditControlManager.Visible = true;

            // Populate DropDownList controls with data from user_tbl
            PopulateDropDownList(ddlCreditControlManager);
            PopulateDropDownList(ddlCreditControlAdmin);

            // Show Save button and hide Add button
            btnSaveGroup.Visible = true;
            btnAdd.Visible = false;
            btnCancel.Visible = true;


            // Additional logic for the "Add" button click event
            // ...
        }


        private void PopulateDropDownList(DropDownList ddl)
        {
            try
            {
                // Connect to MySQL database
                using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                {
                    connection.Open();

                    // Create SQL query to retrieve data from user_tbl
                    string query = "SELECT user_id, logined_user_name FROM user_tbl";

                    // Execute query and populate the DropDownList
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string userId = reader["user_id"].ToString();
                                string userName = reader["logined_user_name"].ToString();

                                // Use userId as the value for the DropDownList
                                ddl.Items.Add(new ListItem(userName, userId));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                // You might want to log the exception or display an error message
                Response.Write("An error occurred: " + ex.Message);
            }
        }


        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            // Capture selected values from DropDownList controls
            string selectedCreditControlAdmin = ddlCreditControlAdmin.SelectedValue;
            string selectedCreditControlManager = ddlCreditControlManager.SelectedValue;

            // Perform save operation with the selected values
            SaveData(selectedCreditControlAdmin, selectedCreditControlManager);

            // Additional logic for the save button click event
            // ...

            // Show Add button and hide Save button
            btnAdd.Visible = true;
            btnSaveGroup.Visible = false;
            btnCancel.Visible = false;



            // Reset DropDownList controls and GridView visibility
            lblCreditControlManager.Visible = false;
            lblCreditControlAdmin.Visible = false;
            ddlCreditControlManager.Visible = false;
            ddlCreditControlAdmin.Visible = false;
            GridView1.Visible = true;
            BindGridViewData();
        }

        private void SaveData(string ccAdmin, string ccManager)
        {
            try
            {
                // Connect to MySQL database
                using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                {
                    connection.Open();

                    // Create SQL query to insert data into the newcust_department_access table
                    string query = "INSERT INTO newcust_department_access (CreditControlManager, CreditControlAdmin) " +
                                   "VALUES (@CCManager, @CCAdmin)";

                    // Use a parameterized query to avoid SQL injection
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CCManager", ccManager);
                        cmd.Parameters.AddWithValue("@CCAdmin", ccAdmin);

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        // Axapta HOD Recommendation

                        Axapta DynAxDepAss = new Axapta();

                        DynAxDepAss.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                        new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                        AxaptaRecord DynRecDepAss = DynAxDepAss.CreateAxaptaRecord("LF_WebNewCustDepartAccess");

                        string DepAssAdm = ddlCreditControlAdmin.SelectedValue;
                        string DepAssMng = ddlCreditControlAdmin.SelectedValue;


                        DynRecDepAss.set_Field("CreditControlAdmin", DepAssAdm);
                        DynRecDepAss.set_Field("CreditControlManager", DepAssMng);

                        DynRecDepAss.Call("insert");


                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessMessage", "showSuccessMessage();", true);


                        BindGridViewData();
                    }
                }

                // Optionally, display a success message or perform additional actions
                Response.Write("Data saved successfully!");
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                // You might want to log the exception or display an error message
                Response.Write("An error occurred: " + ex.Message);
            }
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete button click logic here
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Show GridView and hide DropDownList controls
            GridView1.Visible = true;
            ddlCreditControlAdmin.Visible = false;
            ddlCreditControlManager.Visible = false;
            lblCreditControlManager.Visible = false;
            lblCreditControlAdmin.Visible = false;

            // Show Add button and hide Save, Cancel buttons
            btnAdd.Visible = true;
            btnSaveGroup.Visible = false;
            btnCancel.Visible = false;
            BindGridViewData();

            // Additional logic for the "Cancel" button click event
            // ...
        }

    }
}