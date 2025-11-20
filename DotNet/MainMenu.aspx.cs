using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using System;
using System.Net.Mail;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.UI;

namespace DotNet
{
    public partial class MainMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                string temp_logined_user_name = GLOBAL.logined_user_name;
                if (temp_logined_user_name.Length > 25)
                {
                    String[] arrtemp = temp_logined_user_name.Substring(25).Split(' ');
                    temp_logined_user_name = temp_logined_user_name.Substring(0, 25) + arrtemp[0] + "...";
                }

                Label1.Text = "Hi, " + temp_logined_user_name;
                Label3.Text = "Currently accessing: " + GLOBAL.switch_Company;
                //this.Form.Target = "_blank";//enable it if you want to open in new tab

                if ((Session["user_authority_lvl_Red"].ToString() != Session["user_authority_lvl"].ToString()) ||
                    (Session["logined_user_name_Red"].ToString() != Session["logined_user_name"].ToString()))//not same
                {
                    if (Session["user_authority_lvl_Red"].ToString() == "1")//SuperAdmin
                    {
                        Label_Red.Visible = true;
                        Label_Red.Text = "SuperAdmin: " + Session["logined_user_name_Red"].ToString();
                    }
                    else
                    {
                        Label_Red.Visible = false;
                    }
                }
                else
                {
                    Label_Red.Visible = false;
                }
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                                CustomerMasterTag, CustomerMasterTag2,
                                SFATag, SFATag2,
                                SalesQuotation, SalesQuotation2,
                                PaymentTag, PaymentTag2,
                                RedemptionTag, RedemptionTag2,
                                InventoryMasterTag, InventoryMasterTag2,
                                EORTag, EORTag2,
                                CheckInTag, null,
                                WClaimTag, WClaimTag2,
                                EventBudgetTag, EventBudgetTag2,
                                NewCustomerTag, NewCustomerTag2,
                                MapTag, MapTag2, RocTinTag, RocTinTag2,
                                 NewProduct2
                                );
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
                Session["data_passing"] = "";
                GLOBAL.data_passing = Session["data_passing"].ToString();
            }
            catch
            {
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void testing()
        {
            string smtpServer = "10.1.1.199";
            int port = 25; // Use the appropriate port number for your SMTP server
            string BCCEmailDeveloper = ConfigurationManager.AppSettings["BCCEmailDeveloper"].ToString();
            try
            {
                using (TcpClient client = new TcpClient(smtpServer, port))
                {
                    Console.WriteLine("Connected to SMTP server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }

            try
            {
                // Set up the SMTP client
                SmtpClient client = new SmtpClient("10.1.1.199")
                {
                    Port = 25,
                    Credentials = new NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword),
                };

                // Create the email message
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(BCCEmailDeveloper),
                    Subject = "Test Email",
                    Body = "This is a test email to check SMTP functionality.",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(BCCEmailDeveloper);

                // Send the email
                client.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP-specific exceptions
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"General Error: {ex.Message}");
            }
            //Function_Method.SendMail("tanhl", GLOBAL.user_id, "Testing", "chongwf@posim.com.my", "", "Testing");
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            testing();
        }

        //JJ below for testing if user dont have userid in license then auto load
        protected void btnLicenseRenew_Click(object sender, EventArgs e)
        {
            string userPwd = GLOBAL.axPWD;
            string userId = GLOBAL.user_id;
            string name = GLOBAL.logined_user_name;
            string companyPrefix = GLOBAL.user_company;
            string roleId = "3"; // Default RoleID for Employee

            if (userId.ToLower() == "foozm" && userPwd.ToLower() == "Posim#123456789")
            {
                Response.Redirect("http://sfa.posim.com.my/LicenseSystem/Login.aspx?uid=" + Server.UrlEncode(userId) + "&pwd=" + Server.UrlEncode(userPwd), false);
                return;
            }

          
            string connStr = ConfigurationManager.ConnectionStrings["LOF_renew"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string checkQuery = "SELECT COUNT(*) FROM UserTable WHERE UserID = @UserID";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    // 1. Insert user into UserTable
                    string insertQuery = @"
                        INSERT INTO UserTable 
                        (UserID, Name, Email, PhoneNumber, Status, RoleID, DepartmentID, DateCreated, CompanyPrefix)
                        VALUES 
                        (@UserID, @Name, NULL, NULL, 'Active', @RoleID, NULL, @DateCreated, @CompanyPrefix)";

                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@UserID", userId);
                    insertCmd.Parameters.AddWithValue("@Name", name);
                    insertCmd.Parameters.AddWithValue("@RoleID", roleId);
                    insertCmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@CompanyPrefix", companyPrefix);
                    insertCmd.ExecuteNonQuery();

                    // 2. Find CompanyID using the CompanyPrefix
                    string companyIdQuery = "SELECT CompanyID FROM CompanyTable WHERE CompanyPrefix = @CompanyPrefix LIMIT 1";
                    MySqlCommand getCompanyIdCmd = new MySqlCommand(companyIdQuery, conn);
                    getCompanyIdCmd.Parameters.AddWithValue("@CompanyPrefix", companyPrefix);
                    object result = getCompanyIdCmd.ExecuteScalar();

                    if (result != null)
                    {
                        string companyId = result.ToString();


                        // 3. Auto insert into UserCompanyAccessTable
                        string accessInsertQuery = @"
                            INSERT INTO UserCompanyAccessTable (UserID, CompanyID) 
                            VALUES (@UserID, @CompanyID)";

                        MySqlCommand insertAccessCmd = new MySqlCommand(accessInsertQuery, conn);
                        insertAccessCmd.Parameters.AddWithValue("@UserID", userId);
                        insertAccessCmd.Parameters.AddWithValue("@CompanyID", companyId);
                        insertAccessCmd.ExecuteNonQuery();
                    }

                    conn.Close();

                    //string script = $"alert('You have been auto-registered in the License System. Need to ask admin to update company access, email, phone number, role ID, and department.'); window.location.href='http://sfa.posim.com.my/LicenseSystem/Login.aspx?uid={Server.UrlEncode(userId)}';";
                    string script = $"alert('You have been auto-registered in the License System. Need to ask admin to update company access, email, phone number, role ID, and department.'); window.location.href='http://sfa.posim.com.my/LicenseSystem/Login.aspx?uid={Server.UrlEncode(userId)}&pwd={Server.UrlEncode(userPwd)}';";

                    ScriptManager.RegisterStartupScript(this, GetType(), "AutoRegister", script, true);
                }
                else
                {
                    conn.Close();
                    //Response.Redirect("http://sfa.posim.com.my/LicenseSystem/Login.aspx?uid=" + Server.UrlEncode(userId), false);
                    Response.Redirect("http://sfa.posim.com.my/LicenseSystem/Login.aspx?uid=" + Server.UrlEncode(userId) + "&pwd=" + Server.UrlEncode(userPwd), false);
                }
            }
        }
    }
}