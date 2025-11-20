using ActiveDs;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static QRCoder.PayloadGenerator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DotNet
{

    public partial class AddUser : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["LOF_renew"].ConnectionString;       //JJ 25/3
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
                null, null, null, null,
                    NewProduct2
                    );
                clear_variable();
                checkun.Visible = false;
                UserID.Attributes.Add("autocomplete", "off");
                TextBox_SearchAd.Attributes.Add("onkeypress", "return controlEnter('" + ImageButton2.ClientID + "', event)");
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_AddUser_section'); ", true);
            }

            refresh_hightlight_checkbox();
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
        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void Button_section_Click(object sender, EventArgs e)
        {
            string ButtonText = (sender as Button).Text.Trim();
            HideSection();
            switch (ButtonText)
            {
                case "Add/Update":
                    AddUser_section.Attributes.Add("style", "display:initial");
                    Button_AddUser_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    break;

                case "SFA User":
                    clear_variable();
                    UserID.Text = "";
                    BtnSFARegistration.Visible = true;
                    DotNetUser_section.Attributes.Add("style", "display:initial");
                    Button_DotNetUser_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    DotNetUser();

                    break;

                case "Visitor User":
                    clear_variable();
                    BtnVisitorRegistration.Visible = true;
                    //Button_VisitorUser_Section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    VisitorSection.Attributes.Add("style", "display:initial");
                    VisitorUser();

                    break;

                case "AD User":
                    clear_variable_ActiveDirectory();
                    clear_variable();
                    ActiveDirectory_section.Attributes.Add("style", "display:initial");
                    Button_ActiveDirectory_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    GetAdditionalUserInfo(0);

                    break;
            }
        }

        private void HideSection()
        {
            BtnSFARegistration.Enabled = false;
            BtnVisitorRegistration.Enabled = false;
            BtnLicenseRegistration.Enabled = false;
            AddUser_section.Attributes.Add("style", "display:none");
            Button_AddUser_section.Attributes.Add("style", "background-color:transparent");
            DotNetUser_section.Attributes.Add("style", "display:none");
            Button_DotNetUser_section.Attributes.Add("style", "background-color:transparent");
            ActiveDirectory_section.Attributes.Add("style", "display:none");
            Button_ActiveDirectory_section.Attributes.Add("style", "background-color:transparent");
            //Button_VisitorUser_Section.Attributes.Add("style", "background-color:transparent");
            VisitorSection.Attributes.Add("style", "display:none");

        }

        //protected void Button_AddUser_section_Click(object sender, EventArgs e)
        //{
        //    BtnSFARegistration.Enabled = false;
        //    AddUser_section.Attributes.Add("style", "display:initial"); Button_AddUser_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
        //    DotNetUser_section.Attributes.Add("style", "display:none"); Button_DotNetUser_section.Attributes.Add("style", "background-color:transparent");
        //    ActiveDirectory_section.Attributes.Add("style", "display:none"); Button_ActiveDirectory_section.Attributes.Add("style", "background-color:transparent");
        //}

        //protected void Button_DotNetUser_section_Click(object sender, EventArgs e)
        //{
        //    clear_variable(); TextBox1.Text = "";
        //    AddUser_section.Attributes.Add("style", "display:none"); Button_AddUser_section.Attributes.Add("style", "background-color:transparent");
        //    DotNetUser_section.Attributes.Add("style", "display:initial"); Button_DotNetUser_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
        //    ActiveDirectory_section.Attributes.Add("style", "display:none"); Button_ActiveDirectory_section.Attributes.Add("style", "background-color:transparent");
        //    DotNetUser();
        //}

        //protected void Button_ActiveDirectory_section_Click(object sender, EventArgs e)
        //{
        //    clear_variable_ActiveDirectory(); clear_variable(); TextBox1.Text = ""; BtnSFARegistration.Enabled = false; BtnSFARegistration.Text = "Add User";
        //    AddUser_section.Attributes.Add("style", "display:none"); Button_AddUser_section.Attributes.Add("style", "background-color:transparent");
        //    DotNetUser_section.Attributes.Add("style", "display:none"); Button_DotNetUser_section.Attributes.Add("style", "background-color:transparent");
        //    ActiveDirectory_section.Attributes.Add("style", "display:initial"); Button_ActiveDirectory_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
        //    GridView_ActiveDirectory.PageIndex = 0;
        //    GetAdditionalUserInfo(0);
        //}
        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        protected void RegistrationTypeChanged(object sender, EventArgs e)
        {
            string SelectedValue = ddlRegistrationType.SelectedValue;

            AuthorityLevelWrapper.Visible = false;
            UserRoleWrapper.Visible = false;
            FormWrapper.Visible = false;
            AccessingWrapper.Visible = false;
            LicenseWrapper.Visible = false;           //JJ 25/3
            BtnSFARegistration.Visible = false;
            BtnVisitorRegistration.Visible = false;
            //BtnCheckPassword.Visible = false;
            BtnLicenseRegistration.Visible = false;   //JJ 25/3
            BtnUpdateCompanyList.Visible = false;     //JJ 25/3
            if (SelectedValue != "0") FormWrapper.Visible = true;

            if (SelectedValue == "1")
            {
                AccessingWrapper.Visible = true;
                AuthorityLevelWrapper.Visible = true;
                BtnSFARegistration.Visible = true;

            }
            else if (SelectedValue == "2")
            {
                UserRoleWrapper.Visible = true;
                BtnVisitorRegistration.Visible = true;
                //BtnCheckPassword.Visible = true;
            }
            else if (SelectedValue == "3")  //JJ 25/3
            {
                LicenseWrapper.Visible = true;
                FormWrapper.Visible = true;
                BtnLicenseRegistration.Visible = true; 
                BtnUpdateCompanyList.Visible = true;

                LoadRoleDropdown();
                LoadDepartmentDropdown();
                LoadCompanyAccess();
            }

        }

        private void clear_variable()
        {
            UserName.Text = "";
            Company.Text = "";
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView2.DataSource = null;
            GridView2.DataBind();
            ddlAuthorityLevel.Items.Clear();
            ddlUserRole.Items.Clear();
            BtnSFARegistration.Text = "Add";
            BtnVisitorRegistration.Text = "Add";
            BtnLicenseRegistration.Text = "Add";             //JJ 25/3
            txtEmail.Text = "";                              //JJ 25/3
            txtPhone.Text = "";                              //JJ 25/3
            //ddlRole.SelectedIndex = 0;                       //JJ 25/3
            //ddlDepartment.SelectedIndex = 0;                 //JJ 25/3
            ddlRole.Items.Clear();
            ddlDepartment.Items.Clear();
            // Uncheck all checkboxes in Company Access
            foreach (ListItem item in chkCompanies.Items)    //JJ 25/3
            {
                item.Selected = false;                       //JJ 25/3
            }
        }

        private void Dropdown(int number)
        {
            ddlAuthorityLevel.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            items.Add(new ListItem("-- SELECT --", ""));
            //==================================================================================================
            int user_authority_lvl_Red = Convert.ToInt32(Session["user_authority_lvl_Red"]);

            if ((GLOBAL.user_authority_lvl == 1) || (user_authority_lvl_Red == 1))//Superadmin
            {//only Superadmin allow to add SuperAdmin

                if (number != 1) items.Add(new ListItem("1 -> SuperAdmin", "1"));
            }
            //==================================================================================================

            if (number != 2) items.Add(new ListItem("2 -> Admin", "2"));
            if (number != 3) items.Add(new ListItem("3 -> Basic User", "3"));
            //if (number != 3) items.Add(new ListItem("3 ->Basic User (view + add data)", "3"));
            //if (number != 4) items.Add(new ListItem("4 ->Basic User (view + add+ update data)", "4"));

            ddlAuthorityLevel.Items.AddRange(items.ToArray());
        }

        private void VisitorDropdown(int number)
        {
            List<ListItem> items = new List<ListItem>();
            int user_authority_lvl_Red = Convert.ToInt32(Session["user_authority_lvl_Red"]);

            ddlUserRole.Items.Clear();
            items = new List<ListItem>();

            items.Add(new ListItem("-- SELECT --", ""));
            //==================================================================================================

            if ((GLOBAL.user_authority_lvl == 1) || (user_authority_lvl_Red == 1))//Superadmin
            {//only Superadmin allow to add SuperAdmin

                if (number != 1) items.Add(new ListItem("HR", "1"));
            }
            //==================================================================================================

            if (number != 2) items.Add(new ListItem("Staff", "2"));

            ddlUserRole.Items.AddRange(items.ToArray());
        }

        //JJ 25/3
        private void LoadRoleDropdown()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT RoleID, RoleName FROM RoleTable WHERE Status = 'Active'";  
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                ddlRole.DataSource = cmd.ExecuteReader();
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataBind();

                // Add default "Select Role" option
                ddlRole.Items.Insert(0, new ListItem("-- SELECT --", "", true));
            }
        }

        //JJ 25/3
        private void LoadDepartmentDropdown()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT DepartmentID, DepartmentName FROM DepartmentTable WHERE Status = 'Active'"; 
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                ddlDepartment.DataSource = cmd.ExecuteReader();
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();

                // Add default "Select Department" option
                ddlDepartment.Items.Insert(0, new ListItem("-- SELECT --", "", true));
            }
        }

        //JJ 25/3
        private void LoadCompanyAccess()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                //string query = "SELECT CompanyID, CompanyName FROM CompanyTable";
                string query = "SELECT CompanyID, CompanyName FROM CompanyTable order by CompanyName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                chkCompanies.DataSource = cmd.ExecuteReader();
                chkCompanies.DataTextField = "CompanyName";
                chkCompanies.DataValueField = "CompanyID";
                chkCompanies.DataBind();
            }
        }

        protected void Save_UserRegistration_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            if (UserID.Text.Trim() != "" && ddlAuthorityLevel.SelectedItem.Value != "")// user id is not empty
            {
                try
                {
                    string ButtonID = (sender as Button).ID;
                    switch (ButtonID)
                    {
                        case "BtnSFARegistration":
                            InsertUpdateSFAUser();

                            break;

                        case "BtnVisitorRegistration":
                            InsertUpdateVisitorUser();

                            break;
                        case "BtnLicenseRegistration":  //JJ 25/3
                            InsertUpdateLicenseUser();
                            break;

                    }

                }
                catch (Exception ER_AU_00)
                {
                    Function_Method.MsgBox("ER_AU_00: " + ER_AU_00.ToString(), this.Page, this);
                }
            }
            else
            {
                Function_Method.MsgBox("ER_AU_04: TextBox or Selection Box is empty", this.Page, this);
            }
        }

        protected void InsertUpdateSFAUser()
        {
            //string[,] company_access= check_access(2, GridView1, "chkRow");//company access
            int company_access = check_access(GridView1, "chkRow");//company access
            Page.Form.Controls.Add(GridView1);
            //GridView1.
            int module_access = check_access(GridView2, "chkRow2");//module access

            int user_access = Int16.Parse(ddlAuthorityLevel.SelectedItem.Value);

            store_access(user_access, company_access, module_access);
        }

        protected void InsertUpdateVisitorUser()
        {
            int UserRole = Int16.Parse(ddlUserRole.SelectedItem.Value);

            InsertUpdateVisitorUser(UserRole);
        }

        // JJ 25/3
        protected void UpdateCompanyListBtn_Click(object sender, EventArgs e)
        {
            SyncCompanyFromAxToMySQL();
        }

        // JJ 25/3
        private void SyncCompanyFromAxToMySQL()
        {
            Axapta ax = new Axapta();
            try
            {
                ax.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                AxaptaRecord rec = ax.CreateAxaptaRecord("DataArea");
                rec.ExecuteStmt("SELECT * FROM %1 ORDER BY RECID ASC");

                while (rec.Found)
                {
                    string companyPrefix = rec.get_Field("id").ToString();
                    string companyName = rec.get_Field("name").ToString();

                    SaveCompanyToMySQL(companyName, companyPrefix);
                    rec.Next();
                }

                rec.Dispose();
                ax.Logoff();

                Function_Method.MsgBox("Company data synced from AX successfully.", this.Page, this);
                LoadCompanyAccess();

            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("AX Error: " + ex.Message, this.Page, this);
            }
            finally
            {
                ax.Dispose();
            }
        }

        //JJ 25/3
        private void SaveCompanyToMySQL(string companyName, string companyPrefix)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM CompanyTable WHERE CompanyPrefix = @CompanyPrefix";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@CompanyPrefix", companyPrefix);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                MySqlCommand cmd;
                if (exists > 0)
                {
                    string updateQuery = @"
                UPDATE CompanyTable 
                SET CompanyName = @CompanyName, CompanyPrefix = @CompanyPrefix 
                WHERE CompanyPrefix = @CompanyPrefix";
                    cmd = new MySqlCommand(updateQuery, conn);
                }
                else
                {
                    string insertQuery = @"
                INSERT INTO CompanyTable (CompanyName, CompanyPrefix) 
                VALUES (@CompanyName, @CompanyPrefix)";
                    cmd = new MySqlCommand(insertQuery, conn);
                }

                cmd.Parameters.AddWithValue("@CompanyName", companyName);
                cmd.Parameters.AddWithValue("@CompanyPrefix", companyPrefix);
                cmd.ExecuteNonQuery();
            }
        }
        //JJ 25/3
        protected void InsertUpdateLicenseUser()
        {
            string userID = UserID.Text.Trim();
            string name = UserName.Text.Trim();
            string company = Company.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string status = "Active";
            string roleID = ddlRole.SelectedValue;
            string departmentID = ddlDepartment.SelectedValue;
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int existing_record = BtnLicenseRegistration.Text == "Update" ? 1 : 0;    

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd;
                if (existing_record == 1)
                {
                    string updateQuery = @"
                UPDATE UserTable 
                SET Name = @Name, Email = @Email, PhoneNumber = @Phone, Status = @Status, 
                    RoleID = @RoleID, DepartmentID = @DepartmentID, LastModified = @ModifiedDate, CompanyPrefix = @CompanyPrefix 
                WHERE UserID = @UserID";
                    cmd = new MySqlCommand(updateQuery, conn);
                }
                else
                {
                    string insertQuery = @"
                INSERT INTO UserTable 
                    (UserID, Name, Email, PhoneNumber, Status, RoleID, DepartmentID, DateCreated, LastModified, CompanyPrefix)
                VALUES 
                    (@UserID, @Name, @Email, @Phone, @Status, @RoleID, @DepartmentID, @CreatedDate, @ModifiedDate, @CompanyPrefix)";
                    cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@CreatedDate", now);
                }

                // Shared parameters
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@RoleID", roleID);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                cmd.Parameters.AddWithValue("@ModifiedDate", now);
                cmd.Parameters.AddWithValue("@CompanyPrefix", company);

                cmd.ExecuteNonQuery();

                // Clear previous company access
                string deleteAccess = "DELETE FROM UserCompanyAccessTable WHERE UserID = @UserID";
                MySqlCommand delCmd = new MySqlCommand(deleteAccess, conn);
                delCmd.Parameters.AddWithValue("@UserID", userID);
                delCmd.ExecuteNonQuery();

                // Insert selected company access
                foreach (ListItem item in chkCompanies.Items)
                {
                    if (item.Selected)
                    {
                        string insertAccess = "INSERT INTO UserCompanyAccessTable (UserID, CompanyID) VALUES (@UserID, @CompanyID)";
                        MySqlCommand accessCmd = new MySqlCommand(insertAccess, conn);
                        accessCmd.Parameters.AddWithValue("@UserID", userID);
                        accessCmd.Parameters.AddWithValue("@CompanyID", item.Value);
                        accessCmd.ExecuteNonQuery();
                    }
                }

                string message = existing_record == 1 ? "User updated successfully." : "New user added successfully.";
                Function_Method.MsgBox(message, this.Page, this);
                clear_variable();
            }
        }

        /*====================================================================================================================*/
        protected void CheckID(object sender, EventArgs e)
        {
            clear_variable();
            checkun.Visible = false;

            string UserId = UserID.Text.Trim();
            if (UserId == "") { return; }

            //first check axapta
            if (CheckIdInAxapta(UserId) == true)
            {
                Dropdown(0);//initial dropdown
                VisitorDropdown(0);
                company_list();
                module_list();

                var tuple_CheckVisitorUserMySQL = CheckVisitorUserMySQL();
                bool enable_UpdateVisitorUser = tuple_CheckVisitorUserMySQL.Item1;
                bool enable_AddVisitorUserButton = tuple_CheckVisitorUserMySQL.Item2;
                CheckUserExist("Visitor", enable_UpdateVisitorUser, enable_AddVisitorUserButton);
                //license visitor
                var tuple_CheckLicenseUserMySQL = CheckLicenseUserMySQL();
                bool enable_UpdateLicenseUser = tuple_CheckLicenseUserMySQL.Item1;
                bool enable_AddLicenseUserButton = tuple_CheckLicenseUserMySQL.Item2;
                CheckUserExist("License", enable_UpdateLicenseUser, enable_AddLicenseUserButton);

                var tuple_CheckIdInMySQL = CheckIdInMySQL();
                bool enable_UpdateUser = tuple_CheckIdInMySQL.Item1;
                bool enable_AddUserButton = tuple_CheckIdInMySQL.Item2;
                //SFA
                CheckUserExist("SFA", enable_UpdateUser, enable_AddUserButton);

            }
            else
            {
                BtnSFARegistration.Enabled = false;
                BtnVisitorRegistration.Enabled = false;
                BtnLicenseRegistration.Enabled = false;    //JJ 28/3
                Function_Method.MsgBox("The User Id " + UserId + " is not registered in Axapta System. Axapta Port= " + GLOBAL.AxaptaPort + " .", this.Page, this);
            }
        }

        protected void CheckUserExist(string Section, bool enable_UpdateUser, bool enable_AddUserButton)
        {
            if (enable_UpdateUser == true && enable_AddUserButton == true)
            {
                //have record in MySQL
                if (Section == "SFA")
                {
                    BtnSFARegistration.Enabled = true;
                    BtnSFARegistration.Text = "Update";
                    refresh_hightlight_checkbox();
                }
                else if (Section == "Visitor")
                {
                    BtnVisitorRegistration.Enabled = true;
                    BtnVisitorRegistration.Text = "Update";
                }
                else if (Section == "License")          //JJ 25/3
                {
                    BtnLicenseRegistration.Enabled = true;
                    BtnLicenseRegistration.Text = "Update";
                    BtnUpdateCompanyList.Enabled = true;
                }
            }
            else if (enable_UpdateUser == false && enable_AddUserButton == true)
            {//do not have record in MySQL
                if (Section == "SFA")
                {
                    BtnSFARegistration.Enabled = true;
                }
                else if (Section == "Visitor")
                {
                    BtnVisitorRegistration.Enabled = true;
                }
                else if (Section == "License")     //JJ 25/3
                {
                    BtnLicenseRegistration.Enabled = true;
                    BtnUpdateCompanyList.Enabled= true;
                }
            }
            else if (enable_UpdateUser == false && enable_AddUserButton == false)
            {
                if (Section == "SFA")
                {
                    BtnSFARegistration.Enabled = false;
                }
                else if (Section == "Visitor")
                {
                    BtnVisitorRegistration.Enabled = false;
                }
                else if (Section == "License")       //JJ 25/3
                {
                    BtnLicenseRegistration.Enabled = false;
                }
            }
        }

        private Tuple<bool, bool> CheckIdInMySQL()
        {
            if (UserID.Text == "")//if textbox empty
            {
                return new Tuple<bool, bool>(false, false);
            }

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string Query = "select * from user_tbl where user_id=@D1";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = UserID.Text.Trim();
                cmd.Parameters.Add(_D1);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            UserName.Text = reader.GetValue(1).ToString();//logined_user_name
                                                                          //user_authority_lvl

                            var ff = ddlAuthorityLevel.Items[reader.GetInt16(3)];
                            Dropdown(Convert.ToInt32(ff.Value));
                            ddlAuthorityLevel.SelectedItem.Text = ff.Text;
                            ddlAuthorityLevel.SelectedItem.Value = ff.Value;

                            //page_access_authority
                            int temp_page_access = reader.GetInt32(4);
                            int row_count = GridView1.Rows.Count;

                            for (int i = 0; i < row_count; i++)
                            {
                                CheckBox CheckBox_c = (GridView1.Rows[i].Cells[0].FindControl("chkRow") as CheckBox);
                                if (GridView1.Rows[i].RowType == DataControlRowType.DataRow)
                                {
                                    if ((temp_page_access & GLOBAL.ConversionData[i]) != 0)
                                    {
                                        CheckBox_c.Checked = true;
                                    }
                                }
                            }

                            Company.Text = reader.GetValue(5).ToString();//user_company

                            //module_access_authority
                            int temp_module_access = reader.GetInt32(6);

                            int row_count2 = GridView2.Rows.Count;

                            for (int i = 0; i < row_count2; i++)
                            {
                                CheckBox CheckBox_c2 = (GridView2.Rows[i].Cells[0].FindControl("chkRow2") as CheckBox);
                                if (GridView2.Rows[i].RowType == DataControlRowType.DataRow)
                                {
                                    if ((temp_module_access & GLOBAL.ConversionData[i]) != 0)
                                    {
                                        CheckBox_c2.Checked = true;
                                    }
                                }
                            }
                            conn.Close();

                            return new Tuple<bool, bool>(true, true);
                        }
                    }
                }
                conn.Close();
                return new Tuple<bool, bool>(false, true);
            }
            catch (Exception ER_AU_04)
            {
                Function_Method.MsgBox("ER_AU_04: " + ER_AU_04.ToString(), this.Page, this);
                return new Tuple<bool, bool>(false, false);
            }
        }


        private Tuple<bool, bool> CheckVisitorUserMySQL()
        {
            if (UserID.Text == "")//if textbox empty
            {
                return new Tuple<bool, bool>(false, false);
            }

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string Query = "select * from Visitor_UserLogin where UserName=@D1";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = UserID.Text.Trim();
                cmd.Parameters.Add(_D1);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            UserName.Text = reader.GetValue(1).ToString();//logined_user_name

                            var ff = ddlUserRole.Items[reader.GetInt16(2)];
                            VisitorDropdown(Convert.ToInt32(ff.Value));
                            ddlUserRole.SelectedItem.Text = ff.Text;
                            ddlUserRole.SelectedItem.Value = ff.Value;

                            conn.Close();

                            return new Tuple<bool, bool>(true, true);
                        }
                    }
                }
                conn.Close();
                return new Tuple<bool, bool>(false, true);
            }
            catch (Exception ER_AU_04)
            {
                Function_Method.MsgBox("ER_AU_04: " + ER_AU_04.ToString(), this.Page, this);
                return new Tuple<bool, bool>(false, false);
            }
        }

        //JJ 25/3
        private Tuple<bool, bool> CheckLicenseUserMySQL()
        {
            if (UserID.Text == "") return new Tuple<bool, bool>(false, false);

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = "SELECT * FROM UserTable WHERE UserID = @UserID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", UserID.Text.Trim());
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserName.Text = reader["Name"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtPhone.Text = reader["PhoneNumber"].ToString();
                        Company.Text = reader["CompanyPrefix"].ToString();

                        string deptID = reader["DepartmentID"].ToString();
                        string roleID = reader["RoleID"].ToString();

                        ddlDepartment.SelectedValue = deptID;
                        ddlRole.SelectedValue = roleID;

                        reader.Close();

                        // Load and check company access
                        string accessQuery = "SELECT CompanyID FROM UserCompanyAccessTable WHERE UserID = @UserID";
                        MySqlCommand accessCmd = new MySqlCommand(accessQuery, conn);
                        accessCmd.Parameters.AddWithValue("@UserID", UserID.Text.Trim());
                        using (MySqlDataReader accessReader = accessCmd.ExecuteReader())
                        {
                            while (accessReader.Read())
                            {
                                string compID = accessReader["CompanyID"].ToString();
                                ListItem item = chkCompanies.Items.FindByValue(compID);
                                if (item != null) item.Selected = true;
                            }
                        }

                        return new Tuple<bool, bool>(true, true);
                    }
                }
            }

            return new Tuple<bool, bool>(false, true); // Not in MySQL, can add
        }

        private bool CheckIdInAxapta(string UserId)
        {
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;

            string TableName = "UserInfo";
            string fieldName = ("networkAlias");
            //string fieldValue = ("your_search_criteria_here");

            try
            {
                GLOBAL.Company = null;

                DynAx.LogonAs(UserId, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Define the record
                DynRec = DynAx.CreateAxaptaRecord(TableName);
                DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", fieldName, UserId));
                // Check if the query returned any data.
                if (DynRec.Found)
                {
                    checkun.Visible = false;//true dont need show
                    UserName.Text = (string)DynRec.get_Field("name").ToString().Split('(')[0];
                    Company.Text = (string)DynRec.get_Field("company");

                    //refresh_hightlight_checkbox();
                    return true;
                }
                else
                {
                    checkun.Visible = true;
                    shwimg.ImageUrl = "RESOURCES/not_ok.png";
                    lblmsg.Text = "ID Not Exist..!!";
                    clear_variable();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        /*====================================================================================================================*/
        protected void CheckBoxAll_Changed2(object sender, EventArgs e)
        {
            bool ChkBoxHeader_checked = selectAll_checkbox(GridView2, "chkAll2", "chkRow2");
            if (ChkBoxHeader_checked == true)
            {
                highlight_checkbox(GridView2, "chkRow2");
            }
            else
            {
                module_list();//for better interface
            }
        }

        protected void CheckBox_Changed2(object sender, EventArgs e)
        {
            checking_selectAll_checkbox(GridView2, "chkAll2", "chkRow2");

            refresh_hightlight_checkbox();
        }

        protected void CheckBoxAll_Changed(object sender, EventArgs e)
        {
            bool ChkBoxHeader_checked = selectAll_checkbox(GridView1, "chkAll", "chkRow");
            if (ChkBoxHeader_checked == true)
            {
                highlight_checkbox(GridView1, "chkRow");
            }
            else
            {
                company_list();//for better interface
            }
        }

        protected void CheckBox_Changed(object sender, EventArgs e)
        {
            checking_selectAll_checkbox(GridView1, "chkAll", "chkRow");
            refresh_hightlight_checkbox();
        }

        private void refresh_hightlight_checkbox()
        {
            highlight_checkbox(GridView1, "chkRow");
            highlight_checkbox(GridView2, "chkRow2");
        }
        //sharing function======================================
        private void checking_selectAll_checkbox(GridView Grid_View, string CheckBoxAll, string Check_Box)
        {
            bool check_selectAll = true;
            CheckBox ChkBoxHeader = (CheckBox)Grid_View.HeaderRow.FindControl(CheckBoxAll);

            foreach (GridViewRow row in Grid_View.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl(Check_Box);
                if (ChkBoxRows.Checked == false)
                {
                    check_selectAll = false;
                    goto end_checking;
                }
            }
        end_checking:
            if (check_selectAll == false)
            {
                ChkBoxHeader.Checked = false;
            }
            else
            {
                ChkBoxHeader.Checked = true;
            }
        }

        private bool selectAll_checkbox(GridView Grid_View, string CheckBoxAll, string Check_Box)
        {
            CheckBox ChkBoxHeader = (CheckBox)Grid_View.HeaderRow.FindControl(CheckBoxAll);

            foreach (GridViewRow row in Grid_View.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl(Check_Box);
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
            if (ChkBoxHeader.Checked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void highlight_checkbox(GridView Grid_View, string Check_Box)
        {
            foreach (GridViewRow row in Grid_View.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox_c = (row.Cells[0].FindControl(Check_Box) as CheckBox);
                    if (CheckBox_c.Checked)
                    {
                        row.BackColor = Color.FromName("#ff8000");
                    }
                }
            }
        }
        //end of sharing function======================================

        private int check_access(GridView Grid_View, string Check_Box)
        {
            int row_count = Grid_View.Rows.Count;
            int converted_data = 0x00;

            for (int i = 0; i < row_count; i++)
            {
                if (Grid_View.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox_c = (Grid_View.Rows[i].Cells[0].FindControl(Check_Box) as CheckBox);
                    if (CheckBox_c.Checked)
                    {
                        if (Grid_View.ID.ToString() == "GridView1")
                        {
                            Company.Text = Grid_View.Rows[i].Cells[1].Text;
                        }
                        //D[i, j] = GridView1.Rows[i].Cells[j].Text;
                        converted_data += GLOBAL.ConversionData[i];
                    }
                }
            }
            //return D;
            return converted_data;
        }

        private void InsertUpdateVisitorUser(int UserRole)
        {
            try
            {
                string tempUserPassword = string.Empty;
                string tempUserName = string.Empty;
                int existing_record;
                if (BtnVisitorRegistration.Text == "Update")
                {
                    existing_record = 1;
                }
                else
                {
                    existing_record = 0;
                }

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

                string Query;
                if (existing_record == 1)
                {
                    Query = "update visitor_userlogin SET UserName=@D1,UserFullName=@D2,UserRole=@D3,UserCompany=@D4,UpdatedDateTime=@D7,UpdatedBy=@D8 where UserName=@D1";

                }
                else
                {
                    Query = "insert into visitor_userlogin(UserName,UserFullName,UserRole,UserCompany,CreatedDateTime,CreatedBy,UpdatedDateTime,UpdatedBy,UserPassword,IsUpdatedPassword) values(@D1,@D2,@D3,@D4,@D5,@D6,@D7,@D8,@D9,@D10)";

                }
                MySqlCommand cmd = new MySqlCommand(Query, conn);


                MySqlParameter VisitorID = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                VisitorID.Value = UserID.Text;
                tempUserName = VisitorID.Value.ToString();

                MySqlParameter UserFullName = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                UserFullName.Value = UserName.Text.Trim();

                MySqlParameter VisitorRole = new MySqlParameter("@D3", MySqlDbType.Int32, 0);
                VisitorRole.Value = UserRole;


                MySqlParameter VisitorCompany = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                VisitorCompany.Value = Company.Text;

                MySqlParameter UpdatedDateTime = new MySqlParameter("@D7", MySqlDbType.VarChar, 0);
                //UpdatedDateTime.Value = DateTime.MinValue.ToString(GLOBAL.gDisplayDateTimeFormat);


                MySqlParameter UpdatedBy = new MySqlParameter("@D8", MySqlDbType.VarChar, 0);
                UpdatedBy.Value = string.Empty;

                if (existing_record == 0)
                {
                    MySqlParameter CreatedDateTime = new MySqlParameter("@D5", MySqlDbType.VarChar, 0);
                    //CreatedDateTime.Value = DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat);
                    cmd.Parameters.Add(CreatedDateTime);

                    MySqlParameter CreatedBy = new MySqlParameter("@D6", MySqlDbType.VarChar, 0);
                    CreatedBy.Value = Session["logined_user_name_Red"].ToString();
                    cmd.Parameters.Add(CreatedBy);

                    MySqlParameter Password = new MySqlParameter("@D9", MySqlDbType.VarChar, 0);
                    //Password.Value = GUIDHelper.GenerateCustomGuid("PW", 10);
                    tempUserPassword = Password.Value.ToString();
                    cmd.Parameters.Add(Password);

                    MySqlParameter IsUpdatedPassword = new MySqlParameter("@D10", MySqlDbType.VarChar, 0);
                    IsUpdatedPassword.Value = false;
                    cmd.Parameters.Add(IsUpdatedPassword);
                }
                else
                {
                    UpdatedDateTime = new MySqlParameter("@D7", MySqlDbType.VarChar, 0);
                    //UpdatedDateTime.Value = DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat);

                    UpdatedBy = new MySqlParameter("@D8", MySqlDbType.VarChar, 0);
                    UpdatedBy.Value = Session["logined_user_name_Red"].ToString();

                }

                cmd.Parameters.Add(VisitorID);
                cmd.Parameters.Add(UserFullName);
                cmd.Parameters.Add(VisitorRole);
                cmd.Parameters.Add(VisitorCompany);
                cmd.Parameters.Add(UpdatedDateTime);
                cmd.Parameters.Add(UpdatedBy);
                //              
                //MySqlDataReader MyReader;
                conn.Open();
                //MyReader = cmd.ExecuteReader();     // Here our query will be executed and data saved into the database.  
                cmd.ExecuteNonQuery();
                if (existing_record == 1)
                {
                    Function_Method.MsgBox("New Update to MySQL Success.", this.Page, this);
                }
                else
                {
                    Function_Method.MsgBox("New Data Added to MySQL Success. \nUser: " + tempUserName + "\nPassword: " + tempUserPassword, this.Page, this);
                }

                clear_variable(); BtnVisitorRegistration.Enabled = false; BtnVisitorRegistration.Text = "Add User";
                UserID.Text = "";

                conn.Close();
            }
            catch (Exception ER_AU_02)
            {
                Function_Method.MsgBox("ER_AU_02: " + ER_AU_02.ToString(), this.Page, this);
            }
        }

        protected void CheckUserPassword(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserID.Text.Trim()))
            {
                Function_Method.MsgBox("Empty Field!", this.Page, this);

                return;
            }
            else
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                try
                {
                    string Query = "select UserName, UserPassword, IsUpdatedPassword from visitor_userlogin where UserName = @p0";

                    MySqlCommand cmd = new MySqlCommand(Query, conn);
                    conn.Open();
                    MySqlParameter Param = new MySqlParameter("@p0", MySqlDbType.VarChar, 0);

                    Param.Value = UserID.Text;
                    cmd.Parameters.Add(Param);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string UserName = reader.GetString("UserName");
                            string Password = reader.GetString("UserPassword");
                            bool IsUpdatedPassword = reader.GetBoolean("IsUpdatedPassword");

                            if (IsUpdatedPassword)
                            {
                                Function_Method.MsgBox("Unable to check due to User changed password. ", this.Page, this);
                            }
                            else
                            {
                                Function_Method.MsgBox("User ID: " + UserName + " \nPassword: " + Password, this.Page, this);

                            }
                        }
                    }
                    Function_Method.MsgBox("User Not Exist.", this.Page, this);


                    conn.Close();
                }
                catch (Exception ER_AU_05)
                {
                    Function_Method.MsgBox("ER_AU_05: " + ER_AU_05.ToString(), this.Page, this);
                }
            }
        }

        private void store_access(int user_access, int company_access, int module_access)
        {
            try
            {
                int existing_record;
                if (BtnSFARegistration.Text == "Update")
                {
                    existing_record = 1;
                }
                else
                {
                    existing_record = 0;
                }

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

                string Query;
                if (existing_record == 1)
                {
                    Query = "update user_tbl SET logined_user_name=@D1,user_authority_lvl=@D3,page_access_authority=@D4,user_company=@D5,module_access_authority=@D6,Reserve1=@D7,Reserve2=@D8,modified_date=@D9,modified_by=@D10 where user_id=@D2";

                }
                else
                {
                    Query = "insert into user_tbl(logined_user_name,user_id,user_authority_lvl,page_access_authority,user_company,module_access_authority,Reserve1,Reserve2,created_date,created_by) values(@D1,@D2,@D3,@D4,@D5,@D6,@D7,@D8,@D9,@D10)";

                }
                MySqlCommand cmd = new MySqlCommand(Query, conn);

                //1:logined_user_name
                //2:user_id,
                //3:user_authority_lvl,
                //4:page_access_authority,
                //5:user_company,
                //6:module_access_authority
                //7:Reserve1
                //8:Reserve2

                //1:logined_user_name
                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = UserName.Text;
                cmd.Parameters.Add(_D1);
                //2:user_id,   
                MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                _D2.Value = UserID.Text.Trim();
                cmd.Parameters.Add(_D2);

                //3:user_authority_lvl,
                MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.Int32, 0);
                _D3.Value = user_access;
                cmd.Parameters.Add(_D3);

                //4:page_access_authority
                MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.Int32, 0);
                _D4.Value = Convert.ToInt32(company_access);
                cmd.Parameters.Add(_D4);

                //5:user_company,
                MySqlParameter _D5 = new MySqlParameter("@D5", MySqlDbType.VarChar, 0);
                _D5.Value = Company.Text;
                cmd.Parameters.Add(_D5);

                //6:module_access_authority
                MySqlParameter _D6 = new MySqlParameter("@D6", MySqlDbType.Int32, 0);
                _D6.Value = Convert.ToInt32(module_access);
                cmd.Parameters.Add(_D6);

                //7:Reserve 1,
                MySqlParameter _D7 = new MySqlParameter("@D7", MySqlDbType.VarChar, 0);
                _D7.Value = "";
                cmd.Parameters.Add(_D7);

                //8:Reserve 2,
                MySqlParameter _D8 = new MySqlParameter("@D8", MySqlDbType.VarChar, 0);
                _D8.Value = "";
                cmd.Parameters.Add(_D8);

                //9:date_created/modified
                MySqlParameter _D9 = new MySqlParameter("@D9", MySqlDbType.VarChar, 0);

                _D9.Value = DateTime.Now.ToString("yyyy-MM-dd");
                cmd.Parameters.Add(_D9);

                //10:created_by/ modified by,

                MySqlParameter _D10 = new MySqlParameter("@D10", MySqlDbType.VarChar, 0);

                _D10.Value = Session["logined_user_name_Red"].ToString();
                cmd.Parameters.Add(_D10);
                //              
                //MySqlDataReader MyReader;
                conn.Open();
                //MyReader = cmd.ExecuteReader();     // Here our query will be executed and data saved into the database.  
                cmd.ExecuteNonQuery();
                if (existing_record == 1)
                {
                    Function_Method.MsgBox("New Update to MySQL Success.", this.Page, this);
                    Check_Update_Current_User(UserID.Text.Trim(), user_access, company_access, module_access);
                }
                else
                {
                    Function_Method.MsgBox("New Data Added to MySQL Success.", this.Page, this);
                }

                clear_variable(); BtnSFARegistration.Enabled = false; BtnSFARegistration.Text = "Add User";
                UserID.Text = "";

                conn.Close();
            }
            catch (Exception ER_AU_02)
            {
                Function_Method.MsgBox("ER_AU_02: " + ER_AU_02.ToString(), this.Page, this);
            }
        }

        private void Check_Update_Current_User(string userid, int user_access, int company_access, int module_access)
        {
            if (GLOBAL.user_id == userid)
            {
                Session["user_authority_lvl"] = user_access;
                Session["page_access_authority"] = company_access;
                Session["module_access_authority"] = module_access;
            }
        }

        //initialize==============================================================================================

        private void company_list()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();

            string UserId = UserID.Text.Trim();
            string TableName = "DataArea";

            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;
            int data_count = 2;
            string[] F = new string[data_count];
            F[0] = "id";
            F[1] = "name";

            string[] N = new string[data_count];
            N[0] = "Company Accounts";
            N[1] = "Name of Company Accounts";

            // Output variables for calls to AxRecord.get_Field
            object[] O = new object[data_count];

            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = null;

                DynAx.LogonAs(UserId, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Define the record
                DynRec = DynAx.CreateAxaptaRecord(TableName);

                DynRec.ExecuteStmt("select * from %1 ORDER BY RECID ASC");

                DataTable dt = CreateDataTable(data_count, N);
                DataRow row;

                // Loop through the set of retrieved records.

                while (DynRec.Found)
                {
                    // Retrieve the record data for the
                    // specified fields.
                    row = dt.NewRow();

                    for (int i = 0; i < data_count; i++)
                    {
                        O[i] = DynRec.get_Field(F[i]);
                        row[N[i]] = O[i].ToString();
                    }

                    dt.Rows.Add(row);

                    // Advance to the next row.

                    DynRec.Next();

                }
                // Dispose of the AxaptaRecord object.

                DynRec.Dispose();

                // Log off from Microsoft Dynamics AX.

                DynAx.Logoff();
                //Data-Binding with our GRID

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ER_AU_03)
            {
                Function_Method.MsgBox("ER_AU_03: " + ER_AU_03.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private DataTable CreateDataTable(int data_count, string[] N)
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < data_count; i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }
            //dt.Columns.Add(new DataColumn("REF_NO", typeof(string)));

            return dt;
        }

        private void module_list()
        {
            GridView2.DataSource = null;
            GridView2.DataBind();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Module") });

            for (int i = 0; i < GLOBAL.no_of_module; i++)
            {
                dt.Rows.Add(GLOBAL.module_name[i]);
            }

            GridView2.DataSource = dt;
            GridView2.DataBind();
        }

        private void DotNetUser()
        {
            Button_Delete.Enabled = false;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {
                GridView3.DataSource = null;
                GridView3.DataBind();

                string Query = "select user_key,user_id,logined_user_name,user_company,user_authority_lvl,page_access_authority, " +
                    "module_access_authority,created_date,created_by,modified_date,modified_by from user_tbl order by user_key Desc";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                conn.Open();
                DataSet ds = new DataSet();

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                    GridView3.DataSource = ds;
                    GridView3.DataBind();

                    GridView3.UseAccessibleHeader = true;
                    GridView3.HeaderRow.TableSection = TableRowSection.TableHeader;

                    GridView_RowDataBound_();
                }

                conn.Close();
            }
            catch (Exception ER_AU_05)
            {
                Function_Method.MsgBox("ER_AU_05: " + ER_AU_05.ToString(), this.Page, this);
            }
        }

        private void VisitorUser()
        {
            Button_DeleteVisitorUser.Enabled = false;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {
                GridViewVisitor.DataSource = null;
                GridViewVisitor.DataBind();

                string Query = "select t1.UserName,t1.UserName, t1.UserFullName, t2.RoleName, t1.UserCompany, DATE_FORMAT(t1.CreatedDateTime, '%d/%m/%Y %H:%i:%s') as CreatedDateTime, " +
                    "t1.CreatedBy, DATE_FORMAT(t1.UpdatedDateTime, '%d/%m/%Y %H:%i:%s') as UpdatedDateTime, t1.UpdatedBy from visitor_userlogin t1 " +
                    "inner join visitor_userrole t2 on t2.RoleID = t1.UserRole order by t1.CreatedDateTime Desc";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                conn.Open();
                DataSet ds = new DataSet();

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                    GridViewVisitor.DataSource = ds;
                    GridViewVisitor.DataBind();

                    GridViewVisitor.UseAccessibleHeader = true;
                    GridViewVisitor.HeaderRow.TableSection = TableRowSection.TableHeader;

                    //GridViewVisitor_RowDataBound_();
                }

                conn.Close();
            }
            catch (Exception ER_AU_05)
            {
                Function_Method.MsgBox("ER_AU_05: " + ER_AU_05.ToString(), this.Page, this);
            }
        }


        protected void GridView_RowDataBound_Header(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Select";
                e.Row.Cells[1].Text = "No";
                e.Row.Cells[2].Text = "ID";
                e.Row.Cells[3].Text = "Full Name";
                e.Row.Cells[4].Text = "Default company";
                e.Row.Cells[5].Text = "User Access";
                e.Row.Cells[6].Text = "Company Access";
                e.Row.Cells[7].Text = "Module Access";

                e.Row.Cells[8].Text = "Created Date";
                e.Row.Cells[9].Text = "Created By ";
                e.Row.Cells[10].Text = "Modified Date";
                e.Row.Cells[11].Text = "Modified By";
            }
        }

        protected void GridView_RowDataBound_VisitorUsersHeader(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Select";
                e.Row.Cells[1].Text = "ID";
                e.Row.Cells[2].Text = "User Name";
                e.Row.Cells[3].Text = "Full Name";
                e.Row.Cells[4].Text = "User Role";
                e.Row.Cells[5].Text = "User Company";
                e.Row.Cells[6].Text = "Created Date";
                e.Row.Cells[7].Text = "Created By ";
                e.Row.Cells[8].Text = "Modified Date";
                e.Row.Cells[9].Text = "Modified By";
            }
        }

        private void GridViewVisitor_RowDataBound_()
        {
            int row_count = GridViewVisitor.Rows.Count;
            for (int i = 0; i < row_count; i++)
            {
                if (GridViewVisitor.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    //GridView3.Rows[i].Cells[1].Text = (i + 1).ToString();
                    //user_authority_lvl

                    string temp_list = "";
                    //page_access_authority 
                    int temp_page_access_authority = Convert.ToInt32(GridViewVisitor.Rows[i].Cells[6].Text);

                    var tuple_get_UpToDateCompany = get_UpToDateCompany();
                    List<ListItem> List_UpToDateCompany = tuple_get_UpToDateCompany.Item1;
                    int count_company = tuple_get_UpToDateCompany.Item2;

                    if (count_company != 0)
                    {
                        for (int k = 0; k < count_company; k++)
                        {
                            if ((temp_page_access_authority & GLOBAL.ConversionData[k]) != 0)
                            {
                                temp_list += List_UpToDateCompany[k] + "<br>";
                            }
                        }
                    }

                    GridViewVisitor.Rows[i].Cells[6].Text = temp_list;

                    temp_list = "";
                    //module_access_authority
                    int temp_module_access_authority = Convert.ToInt32(GridViewVisitor.Rows[i].Cells[7].Text);
                    for (int l = 0; l < GLOBAL.no_of_module; l++)
                    {
                        if ((temp_module_access_authority & GLOBAL.ConversionData[l]) != 0)
                        {
                            temp_list += GLOBAL.module_name[l] + "<br>";
                        }
                    }
                    GridViewVisitor.Rows[i].Cells[7].Text = temp_list;

                }
            }
        }

        private void GridView_RowDataBound_()
        {
            int row_count = GridView3.Rows.Count;
            for (int i = 0; i < row_count; i++)
            {
                if (GridView3.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    //GridView3.Rows[i].Cells[1].Text = (i + 1).ToString();
                    //user_authority_lvl
                    int temp_user_authority_lvl = Int16.Parse(GridView3.Rows[i].Cells[5].Text);
                    if (temp_user_authority_lvl == 1)
                    {
                        GridView3.Rows[i].Cells[5].Text = "SuperAdmin";
                    }
                    else if (temp_user_authority_lvl == 2)
                    {
                        GridView3.Rows[i].Cells[5].Text = "Admin";
                    }
                    else if (temp_user_authority_lvl == 3)
                    {
                        GridView3.Rows[i].Cells[5].Text = "Basic user";
                    }
                    else
                    {
                        GridView3.Rows[i].Cells[5].Text = "NOT FOUND";
                    }

                    string temp_list = "";
                    //page_access_authority 
                    int temp_page_access_authority = Convert.ToInt32(GridView3.Rows[i].Cells[6].Text);

                    var tuple_get_UpToDateCompany = get_UpToDateCompany();
                    List<ListItem> List_UpToDateCompany = tuple_get_UpToDateCompany.Item1;
                    int count_company = tuple_get_UpToDateCompany.Item2;

                    if (count_company != 0)
                    {
                        for (int k = 0; k < count_company; k++)
                        {
                            if ((temp_page_access_authority & GLOBAL.ConversionData[k]) != 0)
                            {
                                temp_list += List_UpToDateCompany[k] + "<br>";
                            }
                        }
                    }

                    GridView3.Rows[i].Cells[6].Text = temp_list;

                    temp_list = "";
                    //module_access_authority
                    int temp_module_access_authority = Convert.ToInt32(GridView3.Rows[i].Cells[7].Text);
                    for (int l = 0; l < GLOBAL.no_of_module; l++)
                    {
                        if ((temp_module_access_authority & GLOBAL.ConversionData[l]) != 0)
                        {
                            temp_list += GLOBAL.module_name[l] + "<br>";
                        }
                    }
                    GridView3.Rows[i].Cells[7].Text = temp_list;

                }
            }
        }

        private Tuple<List<ListItem>, int> get_UpToDateCompany()
        {
            List<ListItem> List_UpToDateCompany = new List<ListItem>();
            int count = 0;
            int DataArea = 65533;
            try
            {
                Axapta DynAx = new Axapta();
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = null;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", DataArea);

                axQueryDataSource2.Call("addSortField", 65534, 0);//RECID, must asc

                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", DataArea);
                    count = count + 1;
                    string CompanyId = DynRec2.get_Field("id").ToString();
                    string CompanyName = DynRec2.get_Field("name").ToString();

                    string CompanyDetail = "(" + CompanyId + ")  " + CompanyName;
                    List_UpToDateCompany.Add(new ListItem(CompanyDetail, count.ToString()));

                    DynRec2.Dispose();
                }

            }
            catch (Exception ER_AU_08)
            {
                Function_Method.MsgBox("ER_AU_08: " + ER_AU_08.ToString(), this.Page, this);
            }
            return new Tuple<List<ListItem>, int>(List_UpToDateCompany, count);
        }


        protected void DeleteSelectedVisitor(object sender, EventArgs e)
        {
            try
            {
                int row_count = GridViewVisitor.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridViewVisitor.Rows[i].Cells[0].FindControl("chkRowDelete") as CheckBox);

                    if (GridViewVisitor.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBox_c.Checked)//highlight
                        {
                            arr_selection[count_selection] = GridViewVisitor.Rows[i].Cells[1].Text;
                            count_selection = count_selection + 1;
                        }
                    }
                }

                for (int j = 0; j < count_selection; j++)
                {
                    string Query = "delete from visitor_userlogin where UserName='" + arr_selection[j] + "';";
                    MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                    MySqlCommand cmd = new MySqlCommand(Query, conn);
                    MySqlDataReader MyReader2;

                    conn.Open();
                    MyReader2 = cmd.ExecuteReader();
                    conn.Close();
                }
                Function_Method.MsgBox(count_selection + " data deleted.", this.Page, this);
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_DotNetUser_section'); ", true);
                VisitorUser();
            }
            catch (Exception ER_AU_06)
            {
                Function_Method.MsgBox("ER_AU_06: " + ER_AU_06.ToString(), this.Page, this);
            }
        }

        protected void DeleteSelect(object sender, EventArgs e)
        {
            try
            {
                int row_count = GridView3.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridView3.Rows[i].Cells[0].FindControl("chkRowDelete") as CheckBox);

                    if (GridView3.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBox_c.Checked)//highlight
                        {
                            arr_selection[count_selection] = GridView3.Rows[i].Cells[1].Text;
                            count_selection = count_selection + 1;
                        }
                    }
                }

                for (int j = 0; j < count_selection; j++)
                {
                    string Query = "delete from user_tbl where user_key='" + arr_selection[j] + "';";
                    MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                    MySqlCommand cmd = new MySqlCommand(Query, conn);
                    MySqlDataReader MyReader2;

                    conn.Open();
                    MyReader2 = cmd.ExecuteReader();
                    conn.Close();
                }
                Function_Method.MsgBox(count_selection + " data deleted.", this.Page, this);
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_DotNetUser_section'); ", true);
                DotNetUser();
            }
            catch (Exception ER_AU_06)
            {
                Function_Method.MsgBox("ER_AU_06: " + ER_AU_06.ToString(), this.Page, this);
            }
        }

        protected void CheckBoxVisitorUser_Changed_Delete(object sender, EventArgs e)
        {
            try
            {
                int row_count = GridViewVisitor.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridViewVisitor.Rows[i].Cells[0].FindControl("chkRowDelete") as CheckBox);

                    if (GridViewVisitor.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        /*
                        if (i != ClientRow)//allow only one selection
                        {
                            CheckBox_c.Checked = false;
                            temp_TextBox_PromoQTYSec.Text = "";
                        }
                        */

                        if (CheckBox_c.Checked)//highlight
                        {
                            GridViewVisitor.Rows[i].BackColor = Color.FromName("#ff8000");
                            count_selection = count_selection + 1;
                        }
                    }
                }
                if (count_selection != 0)
                {
                    Button_DeleteVisitorUser.Enabled = true;
                }
                else
                {
                    Button_DeleteVisitorUser.Enabled = false;
                }
            }
            catch (Exception ER_AU_07)
            {
                Function_Method.MsgBox("ER_AU_07: " + ER_AU_07.ToString(), this.Page, this);
            }
        }


        protected void CheckBox_Changed_Delete(object sender, EventArgs e)
        {
            try
            {
                int row_count = GridView3.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridView3.Rows[i].Cells[0].FindControl("chkRowDelete") as CheckBox);

                    if (GridView3.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        /*
                        if (i != ClientRow)//allow only one selection
                        {
                            CheckBox_c.Checked = false;
                            temp_TextBox_PromoQTYSec.Text = "";
                        }
                        */

                        if (CheckBox_c.Checked)//highlight
                        {
                            GridView3.Rows[i].BackColor = Color.FromName("#ff8000");
                            count_selection = count_selection + 1;
                        }
                    }
                }
                if (count_selection != 0)
                {
                    Button_Delete.Enabled = true;
                }
                else
                {
                    Button_Delete.Enabled = false;
                }
            }
            catch (Exception ER_AU_07)
            {
                Function_Method.MsgBox("ER_AU_07: " + ER_AU_07.ToString(), this.Page, this);
            }
        }

        protected void SearchAd(object sender, EventArgs e)
        {
            clear_variable_ActiveDirectory();
            GridView_ActiveDirectory.PageIndex = 0;
            GetAdditionalUserInfo(0);
        }

        private void clear_variable_ActiveDirectory()
        {
            GridView_ActiveDirectory.DataSource = null;
            GridView_ActiveDirectory.DataBind();
            DropDownListResetAdUser.ClearSelection();
            DropDownListResetAdUser.Visible = false;
            TextBox_CustomizedPwd.Visible = false; TextBox_CustomizedPwd.Text = "";
            Button_ExecuteReset.Visible = false;
        }

        private string GetCurrentDomainPath()
        {
            // First, try the fallback using Environment.UserDomainName
            string primaryDnsSuffix = Environment.UserDomainName;

            if (!string.IsNullOrEmpty(primaryDnsSuffix) && !string.Equals(primaryDnsSuffix, Environment.MachineName, StringComparison.OrdinalIgnoreCase))
            {
                Function_Method.AddLog("Using fallback for domain path.");
                return "LDAP://" + primaryDnsSuffix;  // e.g., "LDAP://lionpb.com.my"
            }
            // If fallback fails, proceed to try the LDAP query
            try
            {
                using (DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE"))
                {
                    if (de.Properties["defaultNamingContext"].Count > 0)
                    {
                        Function_Method.AddLog("Successfully retrieved domain path via LDAP.");
                        return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
                    }
                    else
                    {
                        Function_Method.AddLog("defaultNamingContext property not found.");
                        return null; // Or another appropriate value indicating failure
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                Function_Method.AddLog($"COMException: {comEx.Message} (Error Code: {comEx.ErrorCode})");
                return null; // Or another appropriate value indicating failure
            }
            catch (Exception ex)
            {
                Function_Method.AddLog($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return null; // Or another appropriate value indicating failure
            }
            // If everything fails, return null
            Function_Method.AddLog("Failed to get domain path via both fallback and LDAP.");
            return null;
        }

        private DirectorySearcher BuildUserSearcher(DirectoryEntry de, string[] F, int data_count)
        {
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);
            for (int k = 0; k < data_count; k++)
            {
                ds.PropertiesToLoad.Add(F[k]);
            }
            //ds.PropertiesToLoad.Add("distinguishedName");//Distinguished Name
            return ds;
        }

        private void GetAdditionalUserInfo(int PAGE_INDEX)
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            try
            {
                DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());

                int data_count = 7;
                string[] F = new string[data_count];//active directory field *
                F[0] = "SAMAccountName"; F[1] = "name"; F[2] = "userPrincipalName";
                F[3] = "pwdLastSet"; F[4] = "msDS-UserPasswordExpiryTimeComputed";//PasswordExpirationDate
                F[5] = "userAccountControl"; F[6] = "accountExpires";
                //MsDS-LockoutDuration

                ds = new DirectorySearcher(de);
                ds.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Ascending);

                //
                ds = BuildUserSearcher(de, F, data_count);

                string fieldName = ""; string SearchAd = "";
                SearchAd = TextBox_SearchAd.Text.Trim();

                switch (DropDownList1.SelectedItem.Text)
                {
                    case "ID":
                        fieldName = "SAMAccountName";
                        break;
                    case "Name":
                        fieldName = "name";
                        break;

                    default:
                        fieldName = "";
                        break;
                }

                if (fieldName != "" && SearchAd != "")
                {
                    ds.Filter = "(&(objectCategory=User)(objectClass=person)(" + fieldName + "=" + SearchAd + "*))";
                }
                else
                {
                    ds.Filter = "(&(objectCategory=User)(objectClass=person))";
                }

                results = ds.FindAll();

                string[] N = new string[data_count];
                N[0] = "Id"; N[1] = "Full Name"; N[2] = "User Domain";
                N[3] = "PWD Last Set"; N[4] = "PWD Expiration";
                N[5] = "Account Active";
                N[6] = "Account Expiration";
                DataTable dt = CreateVendorDataTable(data_count, N);
                DataRow row;
                //===========================================
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                string[] str_PropertyValue = new string[5];
                foreach (SearchResult sr in results)
                {
                    countA = countA + 1;
                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();
                        row["No"] = countA;
                        for (int j = 0; j < data_count; j++)
                        {
                            if (j == 3 || j == 4 || j == 6)
                            {
                                //string ID = sr.GetPropertyValue("SAMAccountName");
                                //string test1= Function_Method.GetPropertyValue_LargeInteger(de, ID, F[j]);//not using
                                row[N[j]] = Function_Method.DateTimePropertyFromLong(sr, F[j]).ToString();
                            }
                            else if (j == 5)
                            {
                                //int flags = (int)sr.Properties[F[j]].Value;
                                int flags = (int)sr.Properties[F[j]][0];
                                bool AccActive = !Convert.ToBoolean(flags & 0x0002);
                                if (AccActive == true)
                                {
                                    row[N[j]] = "Active";
                                }
                                else
                                {
                                    row[N[j]] = "Not Active";
                                }
                            }
                            else
                            {
                                row[N[j]] = sr.GetPropertyValue(F[j]);
                            }
                        }
                        dt.Rows.Add(row);
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }
            FINISH:;
                GridView_ActiveDirectory.VirtualItemCount = countA;
                GridView_ActiveDirectory.DataSource = dt;
                GridView_ActiveDirectory.DataBind();
            }
            catch (Exception ER_AU_11)
            {
                Function_Method.MsgBox("ER_AU_11: " + ER_AU_11.ToString(), this.Page, this);
            }
        }

        protected void datagrid_PageIndexChanging_ActiveDirectory(object sender, GridViewPageEventArgs e)
        {
            try
            {
                clear_variable_ActiveDirectory();
                GetAdditionalUserInfo(e.NewPageIndex);
                GridView_ActiveDirectory.PageIndex = e.NewPageIndex;
                GridView_ActiveDirectory.DataBind();
            }
            catch (Exception ER_AU_10)
            {
                Function_Method.MsgBox("ER_AU_10: " + ER_AU_10.ToString(), this.Page, this);
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
        //---------------------------------------------------------------
        protected void Button_ExecuteReset_Click(object sender, EventArgs e)
        {
            string ResetSetting = "";
            ResetSetting = DropDownListResetAdUser.SelectedItem.Value;
            if (ResetSetting == "") return;

            try
            {
                int row_count = GridView_ActiveDirectory.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridView_ActiveDirectory.Rows[i].Cells[0].FindControl("chkRowResetPwd") as CheckBox);

                    if (GridView_ActiveDirectory.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBox_c.Checked)//highlight
                        {
                            arr_selection[count_selection] = GridView_ActiveDirectory.Rows[i].Cells[2].Text;
                            count_selection = count_selection + 1;
                        }
                    }
                }

                string UserId_Resetted = "";
                string today = DateTime.Now.ToString("ddMMyyyy");
                string default_password = "Posim@" + today;
                string customized_password = TextBox_CustomizedPwd.Text;
                if (customized_password == "")
                {
                    customized_password = default_password;
                }

                for (int j = 0; j < count_selection; j++)
                {
                    DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
                    DirectorySearcher deSearch = new DirectorySearcher();
                    deSearch.SearchRoot = de;

                    deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + arr_selection[j] + "))";
                    deSearch.SearchScope = SearchScope.Subtree;
                    SearchResult results = deSearch.FindOne();
                    de = new DirectoryEntry(results.Path, GLOBAL.user_id, GLOBAL.axPWD, AuthenticationTypes.Secure);

                    if (de != null)
                    {
                        switch (ResetSetting)
                        {
                            case "1"://Reset with default Pwd
                                de.Invoke("SetPassword", new object[] { default_password });//default password reset
                                de.CommitChanges();

                                break;
                            case "2"://User Required Change PWD on Next Login
                                de.Properties["pwdLastSet"].Value = 0;
                                de.CommitChanges();
                                break;
                            case "3"://Reset with default PWD + User Required Change PWD on Next Login
                                de.Invoke("SetPassword", new object[] { default_password });//default password reset
                                de.CommitChanges();

                                de.Properties["pwdLastSet"].Value = 0;
                                de.CommitChanges();
                                break;
                            case "4"://Reset with customize Pwd
                                de.Invoke("SetPassword", new object[] { customized_password });//customized_password reset
                                de.CommitChanges();
                                break;
                            case "5"://Reset with customize Pwd + User Required Change PWD on Next Login
                                de.Invoke("SetPassword", new object[] { customized_password });//customized_password reset
                                de.Properties["pwdLastSet"].Value = 0;
                                de.CommitChanges();
                                break;
                            default:
                                //do nothing
                                break;
                        }
                        UserId_Resetted += arr_selection[j] + ", ";
                    }
                }
                if (ResetSetting == "1")
                    Function_Method.MsgBox(count_selection + " User Id: " + UserId_Resetted + "reset with default password, " + default_password, this.Page, this);
                else if (ResetSetting == "2")
                    Function_Method.MsgBox(count_selection + " User Id: " + UserId_Resetted + "will require to reset password on next login.", this.Page, this);
                else if (ResetSetting == "3")
                    Function_Method.MsgBox(count_selection + " User Id: " + UserId_Resetted + "reset with default password, " + default_password + ". *User need reset password.", this.Page, this);
                else if (ResetSetting == "4")
                    Function_Method.MsgBox(count_selection + " User Id: " + UserId_Resetted + "reset with customized password, " + customized_password, this.Page, this);
                else if (ResetSetting == "5")
                    Function_Method.MsgBox(count_selection + " User Id: " + UserId_Resetted + "reset with customized password, " + customized_password + ". *User need reset password.", this.Page, this);
                clear_variable_ActiveDirectory();
                GetAdditionalUserInfo(0);
            }
            catch (Exception innerexception)
            {
                Function_Method.MsgBox("ER_AU_12: " + innerexception.ToString(), this.Page, this);
            }
        }

        protected void OnSelectedIndexChanged_DropDownListResetAdUser(object sender, EventArgs e)
        {
            if (DropDownListResetAdUser.SelectedItem.Value != "")//Only selected
            {
                Button_ExecuteReset.Visible = true;
            }
            else
            {
                Button_ExecuteReset.Visible = false;
            }
            if (DropDownListResetAdUser.SelectedItem.Value == "4" || DropDownListResetAdUser.SelectedItem.Value == "5")
            {
                TextBox_CustomizedPwd.Visible = true; TextBox_CustomizedPwd.Text = "";
            }
            else
            {
                TextBox_CustomizedPwd.Visible = false;
            }
        }

        //---------------------------------------------------------------
        protected void CheckBox_ResetPwd(object sender, EventArgs e)
        {
            try
            {
                int row_count = GridView_ActiveDirectory.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridView_ActiveDirectory.Rows[i].Cells[0].FindControl("chkRowResetPwd") as CheckBox);

                    if (GridView_ActiveDirectory.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBox_c.Checked)//highlight
                        {
                            GridView_ActiveDirectory.Rows[i].BackColor = Color.FromName("#ff8000");
                            count_selection = count_selection + 1;
                        }
                    }
                }
                if (count_selection != 0)
                {
                    DropDownListResetAdUser.Visible = true;
                }
                else
                {
                    DropDownListResetAdUser.Visible = false; Button_ExecuteReset.Visible = false; TextBox_CustomizedPwd.Visible = false;
                }
            }
            catch (Exception ER_AU_07)
            {
                Function_Method.MsgBox("ER_AU_07: " + ER_AU_07.ToString(), this.Page, this);
            }
        }
        //==============================================================================================
        //==============================================================================================

    }

}

/*
 //finding existing records
    MySqlCommand cmdCount = new MySqlCommand("SELECT COUNT(*) FROM user_tbl where user_id=@temp", conn);
    MySqlParameter _temp = new MySqlParameter("@temp", MySqlDbType.VarChar, 0);
    _temp.Value = TextBox1.Text.Trim();
    cmdCount.Parameters.Add(_temp);

    conn.Open();
    object obj = cmdCount.ExecuteScalar();
                
    int counter;
    if (obj != null)
    {
        counter = Convert.ToInt32(obj);
    }
    else
    {
        counter = 0;
    }
     
    cmdCount.Dispose(); conn.Close();
    //
                
    string Query;
    if (counter > 0)
    {
            Query = "update user_tbl SET logined_user_name=@D1,user_authority_lvl=@D3,page_access_authority=@D4,user_company=@D5,module_access_authority=@D6,Reserve1=@D7,Reserve2=@D8 where user_id=@D2";
               
    }
    else
    {
            Query = "insert into user_tbl(logined_user_name,user_id,user_authority_lvl,page_access_authority,user_company,module_access_authority,Reserve1,Reserve2) values(@D1,@D2,@D3,@D4,@D5,@D6,@D7,@D8)";

    }
   */
