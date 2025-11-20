using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DotNet
{
    public partial class NewProductHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            check_session();
            TimeOutRedirect();


            if (!IsPostBack)
            {
                PopulateDropDownLists();
                try
                {
                    string newCustID = Session["serialNo"] as string;
                    LoadDataFromDatabase();
                }
                catch
                {

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

        protected void DoNPRF(object sender, EventArgs e)
        {
            Response.Redirect("NewProductRequest.aspx");
        }

        protected void HomeMenu(object sender, EventArgs e)
        {
            Response.Redirect("MainMenu.aspx");
        }

        private void LoadDataFromDatabase()
        {
            string UserName = GLOBAL.logined_user_name;

            int index = UserName.IndexOf(" (");

            string fullName = (index >= 0) ? UserName.Substring(0, index) : UserName;

            string Status = "Waiting " + fullName;

            string connectionString = GLOBAL.connStr;

            string query = "SELECT serialNo, requestBy, date, status, name FROM newproductrequest_info WHERE SUBSTRING_INDEX(requestBy, ' (', 1) = @name OR LOWER(status) LIKE CONCAT('%', LOWER(@name), '%') ORDER BY date DESC, time DESC";

            string adminQuery = "SELECT serialNo, requestBy, date, status, name FROM newproductrequest_info ORDER BY date DESC, time DESC";


            MySqlConnection connectionUsr = new MySqlConnection(connectionString);

            // new product approval user list
            string usrQuery = $"SELECT * FROM newproductrequest_approval";

            MySqlCommand usrCommand = new MySqlCommand(usrQuery, connectionUsr);

            connectionUsr.Open();
            using (MySqlDataReader usrReader = usrCommand.ExecuteReader())
            {
                if (usrReader.Read())
                {
                    // Populate controls on your form with data from newproductrequest_info
                    string check = usrReader["CheckUser"].ToString();
                    string qc = usrReader["QC"].ToString();
                    string product = usrReader["Production"].ToString();
                    string purchase = usrReader["Purchase"].ToString();
                    string warehouse = usrReader["Warehouse"].ToString();
                    string anp = usrReader["Marketing"].ToString();
                    string it = usrReader["IT"].ToString();
                    string approve = usrReader["Approve"].ToString();
                    string admin = usrReader["Admin"].ToString();

                    PopulateTextBoxes(check, ddlCheck1, ddlCheck2);
                    PopulateTextBoxes(qc, ddlQC1, ddlQC2);
                    PopulateTextBoxes(product, ddlProduct1, ddlProduct2);
                    PopulateTextBoxes(purchase, ddlPurchase1, ddlPurchase2);
                    PopulateTextBoxes(warehouse, ddlWare1, ddlWare2);
                    PopulateTextBoxes(anp, ddlMarket1, ddlMarket2);
                    PopulateTextBoxes(it, ddlIT1, ddlIT2);
                    PopulateTextBoxes(approve, ddlApprove1, ddlApprove2);
                    PopulateTextBoxes(admin, ddlAdmin1, ddlAdmin2);
                }
            }

            if (UserName == ddlAdmin1.SelectedValue || UserName == ddlAdmin2.SelectedValue || ddlAdmin1.SelectedValue == "")
            {
                btnAddUser.Visible = true;
                btnViewAll.Visible = true;
                btnFindStatus.Visible = true;
                btnSave.Visible = true;
                btnClose.Visible = true;
                btnViewUser.Visible = false;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(adminQuery, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();

                            connection.Open();
                            adapter.Fill(dataTable);

                            // If results are found, bind to GridView
                            GridView1.DataSource = dataTable;
                            GridView1.DataBind();
                        }
                    }
                }
            }
            else
            {
                readOnly();
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", fullName);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();

                        connection.Open();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            // If results are found, bind to GridView
                            GridView1.DataSource = dataTable;
                            GridView1.DataBind();
                            readOnly();
                        }
                        else
                        {
                            // No results found, handle accordingly
                            // For example, log a message or notify the user
                            // GridView1.DataSource = null;
                            // GridView1.DataBind();
                            // Or show a message to the user


                            if (UserName != "Admin DotNet" && UserName != ddlAdmin1.SelectedValue && UserName != ddlAdmin2.SelectedValue)
                            {
                                string script = "alert('Dont have your record / Nothing waiting for you to review.');";
                                ClientScript.RegisterStartupScript(this.GetType(), "NoRecordsMessage", script, true);
                            }
                        }
                    }
                }
            }


        }

        private void UpdateStatus(string newStatus, string statusFilter)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query = "UPDATE newproductrequest_info SET status = @newStatus WHERE status LIKE CONCAT(@statusFilter, '%')";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@newStatus", newStatus.Trim());
                cmd.Parameters.AddWithValue("@statusFilter", statusFilter);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
        }

        private void readOnly()
        {
            ddlCheck1.Enabled = false;
            ddlCheck2.Enabled = false;
            ddlQC1.Enabled = false;
            ddlQC2.Enabled = false;
            ddlProduct1.Enabled = false;
            ddlProduct2.Enabled = false;
            ddlPurchase1.Enabled = false;
            ddlPurchase2.Enabled = false;
            ddlWare1.Enabled = false;
            ddlWare2.Enabled = false;
            ddlMarket1.Enabled = false;
            ddlMarket2.Enabled = false;
            ddlIT1.Enabled = false;
            ddlIT2.Enabled = false;
            ddlApprove1.Enabled = false;
            ddlApprove2.Enabled = false;
            ddlAdmin1.Enabled = false;
            ddlAdmin2.Enabled = false;
        }

        private void PopulateTextBoxes(string data, System.Web.UI.WebControls.DropDownList ddl1, System.Web.UI.WebControls.DropDownList ddl2)
        {
            if (!string.IsNullOrEmpty(data))
            {
                string[] parts = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    ddl1.SelectedValue = parts[0].Trim();
                }

                if (parts.Length > 1)
                {
                    ddl2.SelectedValue = parts[1].Trim();
                }
            }
        }

        protected void GoToRequestForm(object sender, EventArgs e)
        {
            // Get the newCustID from the command argument
            System.Web.UI.WebControls.Button btnRequestForm = (System.Web.UI.WebControls.Button)sender;
            string serialNum = btnRequestForm.CommandArgument;

            // Check the status for the given newCustID
            string status = GetStatusForserialNo(serialNum);

            // Redirect based on the status
            Response.Redirect("NewProductRequest.aspx?serialNo=" + serialNum);
        }

        private string GetStatusForserialNo(string serialNum)
        {
            // Query the database to get the status for the given newCustID
            string connectionString = GLOBAL.connStr;
            string query = $"SELECT status FROM newproductrequest_info WHERE serialNo = '{serialNum}'";

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

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < GridView1.Rows.Count)
            {
                // Get the NewCustID from the row being deleted
                string serialNum = GridView1.DataKeys[e.RowIndex].Value.ToString();

                // Delete the row from the database
                DeleteRowFromDatabase(serialNum);



                // Rebind the GridView to show the updated data
                LoadDataFromDatabase();
            }
        }

        private void DeleteRowFromDatabase(string serialNum)
        {

            string connectionString = GLOBAL.connStr;
            string query = $"DELETE FROM newproductrequest_info WHERE serialNo = '{serialNum}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            string queryDetail = $"DELETE FROM newproductrequest_detail WHERE serialNo = '{serialNum}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(queryDetail, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            string queryReview = $"DELETE FROM newproductrequest_review WHERE serialNo = '{serialNum}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(queryReview, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

        }

        protected void btnFindStatus_Click(object sender, EventArgs e)
        {
            string UserName = GLOBAL.logined_user_name;

            int index = UserName.IndexOf(" (");

            string fullName = (index >= 0) ? UserName.Substring(0, index) : UserName;

            //string Status = "Waiting Approve: *";

            // Only show data with status "Waiting Approve:"
            FilterDataByStatus("Waiting Approve: ");
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void PopulateDropDownLists()
        {
            List<string> words = GetWordsFromDatabase();

            // Bind the data to all dropdown lists
            BindDataToDropDownList(ddlCheck1, words);
            BindDataToDropDownList(ddlCheck2, words);
            BindDataToDropDownList(ddlQC1, words);
            BindDataToDropDownList(ddlQC2, words);
            BindDataToDropDownList(ddlProduct1, words);
            BindDataToDropDownList(ddlProduct2, words);
            BindDataToDropDownList(ddlPurchase1, words);
            BindDataToDropDownList(ddlPurchase2, words);
            BindDataToDropDownList(ddlWare1, words);
            BindDataToDropDownList(ddlWare2, words);
            BindDataToDropDownList(ddlMarket1, words);
            BindDataToDropDownList(ddlMarket2, words);
            BindDataToDropDownList(ddlIT1, words);
            BindDataToDropDownList(ddlIT2, words);
            BindDataToDropDownList(ddlApprove1, words);
            BindDataToDropDownList(ddlApprove2, words);
            BindDataToDropDownList(ddlAdmin1, words);
            BindDataToDropDownList(ddlAdmin2, words);
        }

        private List<string> GetWordsFromDatabase()
        {
            List<string> words = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                string query = "SELECT logined_user_name FROM user_tbl";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fullName = reader.GetString("logined_user_name");
                        string nameWithoutParenthesis = GetNameBeforeParenthesis(fullName);
                        words.Add(nameWithoutParenthesis);
                        //words.Add(reader.GetString("logined_user_name"));
                    }
                }
            }
            return words;
        }

        private void BindDataToDropDownList(DropDownList ddl, List<string> words)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            foreach (string word in words)
            {
                ddl.Items.Add(new ListItem(word, word));
            }
        }

        private string GetNameBeforeParenthesis(string fullName)
        {
            int index = fullName.IndexOf('(');
            if (index > -1)
            {
                return fullName.Substring(0, index).Trim(); // Extract and trim name before '('
            }
            return fullName; // Return the full name if no '(' is found
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {

                string checkUser;
                string check1 = ddlCheck1.SelectedItem.Text;
                string qcUser;
                string productUser;
                string purchaseUser;
                string wareUser;
                string marketUser;
                string itUser;
                string approveUser;
                string adminUser;
                string emptyBlank = "";


                if (ddlCheck1.SelectedItem.Text != "")
                {
                    if (ddlCheck2.SelectedItem.Text != "")
                    {
                        checkUser = ddlCheck1.SelectedItem.Text + ", " + ddlCheck2.SelectedItem.Text;
                    }
                    else
                    {
                        checkUser = ddlCheck1.SelectedItem.Text;
                    }
                }
                else
                {
                    checkUser = emptyBlank;
                }

                if (ddlQC1.SelectedItem.Text != "")
                {
                    if (ddlQC2.SelectedItem.Text != "")
                    {
                        qcUser = ddlQC1.SelectedItem.Text + ", " + ddlQC2.SelectedItem.Text;
                    }
                    else
                    {
                        qcUser = ddlQC1.SelectedItem.Text;
                    }
                }
                else
                {
                    qcUser = emptyBlank;
                }

                if (ddlProduct1.SelectedItem.Text != "")
                {
                    if (ddlProduct2.SelectedItem.Text != "")
                    {
                        productUser = ddlProduct1.SelectedItem.Text + ", " + ddlProduct2.SelectedItem.Text;
                    }
                    else
                    {
                        productUser = ddlProduct1.SelectedItem.Text;
                    }
                }
                else
                {
                    productUser = emptyBlank;
                }

                if (ddlPurchase1.SelectedItem.Text != "")
                {
                    if (ddlPurchase2.SelectedItem.Text != "")
                    {
                        purchaseUser = ddlPurchase1.SelectedItem.Text + ", " + ddlPurchase2.SelectedItem.Text;
                    }
                    else
                    {
                        purchaseUser = ddlPurchase1.SelectedItem.Text;
                    }
                }
                else
                {
                    purchaseUser = emptyBlank;
                }

                if (ddlWare1.SelectedItem.Text != "")
                {
                    if (ddlWare2.SelectedItem.Text != "")
                    {
                        wareUser = ddlWare1.SelectedItem.Text + ", " + ddlWare2.SelectedItem.Text;
                    }
                    else
                    {
                        wareUser = ddlWare1.SelectedItem.Text;
                    }
                }
                else
                {
                    wareUser = emptyBlank;
                }

                if (ddlMarket1.SelectedItem.Text != "")
                {
                    if (ddlMarket2.SelectedItem.Text != "")
                    {
                        marketUser = ddlMarket1.SelectedItem.Text + ", " + ddlMarket2.SelectedItem.Text;
                    }
                    else
                    {
                        marketUser = ddlMarket1.SelectedItem.Text;
                    }
                }
                else
                {
                    marketUser = emptyBlank;
                }

                if (ddlIT1.SelectedItem.Text != "")
                {
                    if (ddlIT2.SelectedItem.Text != "")
                    {
                        itUser = ddlIT1.SelectedItem.Text + ", " + ddlIT2.SelectedItem.Text;
                    }
                    else
                    {
                        itUser = ddlIT1.SelectedItem.Text;
                    }
                }
                else
                {
                    itUser = emptyBlank;
                }

                if (ddlApprove1.SelectedItem.Text != "")
                {
                    if (ddlApprove2.SelectedItem.Text != "")
                    {
                        approveUser = ddlApprove1.SelectedItem.Text + ", " + ddlApprove2.SelectedItem.Text;
                    }
                    else
                    {
                        approveUser = ddlApprove1.SelectedItem.Text;
                    }
                }
                else
                {
                    approveUser = emptyBlank;
                }

                if (ddlAdmin1.SelectedItem.Text != "")
                {
                    if (ddlAdmin2.SelectedItem.Text != "")
                    {
                        adminUser = ddlAdmin1.SelectedItem.Text + ", " + ddlAdmin2.SelectedItem.Text;
                    }
                    else
                    {
                        adminUser = ddlAdmin1.SelectedItem.Text;
                    }
                }
                else
                {
                    adminUser = emptyBlank;
                }

                connection.Open();

                string selectQuery = "SELECT * FROM newproductrequest_approval";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                MySqlDataReader reader = selectCmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    // Insert new row
                    string insertQuery = "INSERT INTO newproductrequest_approval (CheckUser, QC, Production, Purchase, Warehouse, Marketing, IT, Approve, Admin) VALUES (@CheckUser, @QC, @Production, @Purchase, @Warehouse, @Marketing, @IT, @Approve, @Admin)";

                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@CheckUser", checkUser);
                    insertCmd.Parameters.AddWithValue("@QC", qcUser);
                    insertCmd.Parameters.AddWithValue("@Production", productUser);
                    insertCmd.Parameters.AddWithValue("@Purchase", purchaseUser);
                    insertCmd.Parameters.AddWithValue("@Warehouse", wareUser);
                    insertCmd.Parameters.AddWithValue("@Marketing", marketUser);
                    insertCmd.Parameters.AddWithValue("@IT", itUser);
                    insertCmd.Parameters.AddWithValue("@Approve", approveUser);
                    insertCmd.Parameters.AddWithValue("@Admin", adminUser);

                    insertCmd.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    // Update existing row
                    string updateQuery = "UPDATE newproductrequest_approval " +
                                         "SET CheckUser = @CheckUser, QC = @QC, Production = @Production, " +
                                         "Purchase = @Purchase, Warehouse = @Warehouse, Marketing = @Marketing, IT = @IT, Approve = @Approve, Admin = @Admin";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@CheckUser", checkUser);
                    updateCmd.Parameters.AddWithValue("@QC", qcUser);
                    updateCmd.Parameters.AddWithValue("@Production", productUser);
                    updateCmd.Parameters.AddWithValue("@Purchase", purchaseUser);
                    updateCmd.Parameters.AddWithValue("@Warehouse", wareUser);
                    updateCmd.Parameters.AddWithValue("@Marketing", marketUser);
                    updateCmd.Parameters.AddWithValue("@IT", itUser);
                    updateCmd.Parameters.AddWithValue("@Approve", approveUser);
                    updateCmd.Parameters.AddWithValue("@Admin", adminUser);

                    updateCmd.ExecuteNonQuery();

                    string updchck = "Waiting Checker: " + checkUser;
                    string updqc = "Waiting QC: " + qcUser;
                    string updproduct = "Waiting Production: " + productUser;
                    string updpurchase = "Waiting Purchase: " + purchaseUser;
                    string updware = "Waiting Warehouse: " + wareUser;
                    string updmkt = "Waiting Marketing: " + marketUser;
                    string updit = "Waiting IT: " + itUser;
                    string updapprove = "Waiting Approve: " + approveUser;

                    UpdateStatus(updchck, "Waiting Checker: ");
                    UpdateStatus(updqc, "Waiting QC: ");
                    UpdateStatus(updproduct, "Waiting Production: ");
                    UpdateStatus(updpurchase, "Waiting Purchase: ");
                    UpdateStatus(updware, "Waiting Warehouse: ");
                    UpdateStatus(updmkt, "Waiting Marketing: ");
                    UpdateStatus(updit, "Waiting IT: ");
                    UpdateStatus(updapprove, "Waiting Approve: ");

                    Response.Redirect("NewProductHomePage.aspx");
                }



            }
            catch (Exception ex)
            {

            }
        }

        private void FilterDataByStatus(string status)
        {
            string connectionString = GLOBAL.connStr;

            // Modify the query to filter data by status
            string query = "SELECT serialNo, requestBy, date, status, name FROM newproductrequest_info WHERE status LIKE @Status";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", "%" + status + "%");

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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            LoadDataFromDatabase(); // Call the method to load data based on the new page index

        }

    }



}