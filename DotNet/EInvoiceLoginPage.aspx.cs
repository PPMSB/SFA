using DotNetSync.lhdn.Controllers;
using DotNetSync.lhdn.Models;
using DotNetSync.lhdn.Utilities;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNetSync.SyncGatepassAxToDotNetV2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DotNetSync.lhdn
{
    public partial class EInvoiceLoginPage : System.Web.UI.Page
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            string password = EncryptionHelper.Encrypt("12345678");
            Console.WriteLine(password);
            string decrypted_password = EncryptionHelper.Decrypt(password);
            Console.WriteLine(decrypted_password);
        }

        public class LoginResponse
        {
            public string status { get; set; }
            public string status_msg { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string login(string user_id, string password)
        {
            string status = "";
            string status_msg = "";

            (bool mysql_login_success, status, status_msg, string user_full_name, string user_company, List<string> user_access_companies) = 
                login_mysql(user_id, password);
            
            if (mysql_login_success)
            {
                (bool axapta_login_success, status, status_msg) = login_axapta(user_id, password, user_company);

                if (axapta_login_success)
                {
                    HttpContext.Current.Session["einvoice_user_name"] = user_full_name;
                    HttpContext.Current.Session["einvoice_user_id"] = user_id;
                    HttpContext.Current.Session["access_companies"] = user_access_companies;
                }
            }
            

            LoginResponse response = new LoginResponse
            {
                status = status,
                status_msg = status_msg
            };

            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }

        private static (bool, string, string, string, string, List<string>) login_mysql(string user_id, string password)
        {
            string status = "";
            string status_msg = "";

            string user_full_name = "";
            string user_company = ""; // company which staff under
            List<string> user_access_companies = new List<string>(); // companies that staff has permission to access

            try
            {
                using (MySqlConnection conn = MysqlController.get_connection())
                {
                    conn.Open();

                    string sql = "SELECT * FROM user_table WHERE user_id = @user_id";

                    using (MySqlCommand mysql_command = new MySqlCommand(sql, conn))
                    {
                        mysql_command.Parameters.Clear();
                        mysql_command.Parameters.AddWithValue("@user_id", user_id);

                        MySqlDataReader reader = mysql_command.ExecuteReader();

                        if (reader.Read())
                        {
                            user_full_name = reader["user_full_name"].ToString();
                            user_company = reader["company"].ToString();
                            //string user_password = reader["password"].ToString();
                            //string user_company = reader["company"].ToString();
                            //string encrypted_pw = EncryptionHelper.Decrypt(user_password);
                        }
                        else
                        {
                            return (false, "failed", "User id not found, please contact administrator.", "", "", user_access_companies);
                        }
                    }

                    sql = "SELECT * FROM user_access_company WHERE user_id = @user_id";

                    using (MySqlCommand mysql_command = new MySqlCommand(sql, conn))
                    {
                        mysql_command.Parameters.Clear();
                        mysql_command.Parameters.AddWithValue("@user_id", user_id);

                        MySqlDataReader reader = mysql_command.ExecuteReader();

                        while (reader.Read())
                        {
                            string company = reader["company"].ToString();
                            //string user_password = reader["password"].ToString();
                            //string user_company = reader["company"].ToString();
                            //string encrypted_pw = EncryptionHelper.Decrypt(user_password);
                            user_access_companies.Add(company);
                        }

                        if (user_access_companies.Count < 1)
                        {
                            return (false, "failed", "Invalid user permission, please contact administrator.", "", "", null);
                        }
                        return (true, "success", "success", user_full_name, user_company, user_access_companies);
                    }
                }
            }
            catch (MySqlException ex)
            {
                status = "failed";
                status_msg = "Mysql Login MySqlException: " + ex.Message;

                // Handle specific MySQL error codes
                if (ex.Number == 1062) // MySQL error code for duplicate entry
                {
                    // Handle duplicate entry error
                }
                else if (ex.Number == 1452) // MySQL error code for foreign key violation
                {
                    // Handle foreign key violation error
                }
                logger.Error($"Exception: {status_msg}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                return (false, status, status_msg, "", "", null);

            }
            catch (Exception ex)
            {
                status = "failed";
                status_msg = "Mysql Login Exception: " + ex.Message;

                logger.Error($"Exception: {status_msg}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                return (false, status, status_msg, "", "", null);
            }
        }

        private static (bool, string, string) login_axapta(string user_id, string password, string company_short_name)
        {
            string status = "";
            string status_msg = "";

            try
            {
                var dynax = AxController.axapta_user_login(user_id, password, company_short_name);
                if (dynax != null)
                {
                    return (true, "success", "success");
                }

                return (false, "failed", "Axapta login failed");
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
            {
                // Handle X++ exceptions from Business Connector
                status = "failed";
                status_msg = "Axapta Login X++ Exception: " + ex.Message;

                logger.Error($"XppException: {status_msg}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }

                return (false, status, status_msg);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
            {
                status = "failed";
                status_msg = "Axapta Login LogonFailedException: " + ex.Message;

                logger.Error($"LogonFailedException: {status_msg}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }

                return (false, status, status_msg);
            }
            catch (Exception ex)
            {
                status = "failed";
                status_msg = "Axapta Login Exception: " + ex.Message;

                logger.Error($"Exception: {status_msg}");
                logger.Error($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    logger.Error($"Inner Exception: {ex.InnerException.Message}");
                    logger.Error($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }

                return (false, status, status_msg);
            }
            finally
            {
                //dynax.Logoff();
                AxController.axapta_user_logoff();
            }
        }

        [WebMethod(EnableSession = true)]
        public static string logout()
        {
            // Clear or abandon the session
            HttpContext.Current.Session.Abandon();

            // Construct the response
            LoginResponse response = new LoginResponse
            {
                status = "success",
                status_msg = "success"
            };

            // Serialize the response object to JSON
            string json_response = JsonConvert.SerializeObject(response);

            return json_response;
        }

    }
}