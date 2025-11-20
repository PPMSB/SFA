using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.DirectoryServices;
using System.Dynamic;
using System.IO;
using System.Web;

namespace DotNet
{
    public partial class Login_Page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GLOBAL.system_checking = 0;//test();
            if (!IsPostBack)
            {
                //initialize======================================================
                //Session.Clear();
                Session.Clear();
                Label1.Text = GLOBAL.version_control;
                TextBox1.Attributes.Add("autocomplete", "off");

                //================================================================
                //this.Form.Target = "_blank";//enable it if you want to open in new tab
                //SetFocus(TextBox1);
                // ✅ Auto-login from License System
                string uid = Request.QueryString["uid"];
                string pwd = Request.QueryString["pwd"];
                if (!string.IsNullOrEmpty(uid))
                {
                    uid = uid.Trim().ToLower();

                    if (uid == "foozm" && pwd == "Posim#123456789") // ✅ Check if hardcoded admin
                    {
                        // hardcode admin auto login
                        Check_Admin_Logging(uid, pwd);
                        system_time_format();
                        Response.Redirect("MainMenu.aspx", false);
                    }
                    else
                    {
                        // user auto-login
                        string tempaxPWD = pwd;
                        if (CheckMYSQLConn(uid, tempaxPWD))
                        {
                            system_time_format();
                            Response.Redirect("MainMenu.aspx", false);
                        }
                        else
                        {
                            Function_Method.MsgBox("Auto-login failed. Please login manually.", this.Page, this);
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(uid))
                //{
                //    uid = uid.Trim().ToLower();

                //    if (uid == " ") // ✅ Check if hardcoded admin
                //    {
                //        // hardcode admin auto login
                //        string tempaxPWD = "123";
                //        Check_Admin_Logging(uid, tempaxPWD);
                //        system_time_format();
                //        Response.Redirect("MainMenu.aspx", false);
                //    }
                //    else
                //    {
                //        // user auto-login
                //        string tempaxPWD = "";
                //        if (CheckMYSQLConn(uid, tempaxPWD))
                //        {
                //            system_time_format();
                //            Response.Redirect("MainMenu.aspx", false);
                //        }
                //        else
                //        {
                //            Function_Method.MsgBox("Auto-login failed. Please login manually.", this.Page, this);
                //        }
                //    }
                //}

            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string tempUserName;
            string tempaxPWD;

            tempUserName = TextBox1.Text.Trim().ToLower().ToString();
            tempaxPWD = TextBox2.Text.Trim().ToString();

            if (tempUserName != "" && tempaxPWD != "")
            {
                if (Check_Admin_Logging(tempUserName, tempaxPWD) == true)
                {
                    system_time_format();
                    Response.Redirect("MainMenu.aspx");
                }

                if (CheckMYSQLConn(tempUserName, tempaxPWD) == true)
                {
                    //if (CheckAxaptaConnStatus(tempUserName, tempaxPWD) == true)
                    //{
                        system_time_format();
                        Response.Redirect("MainMenu.aspx");
                    //}
                    //else
                    //{
                    //    Session.Clear();
                    //    Function_Method.MsgBox("Incorrect Username/Password!", this.Page, this);
                    //}
                }
            }
        }

        private bool Check_Admin_Logging(string tempUserName, string tempaxPWD)
        {
            if (tempUserName == GLOBAL.AdminID && tempaxPWD == "Posim#123456789")
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

        private bool CheckMYSQLConn(string tempUserName, string tempaxPWD)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

                string Query = "select * from user_tbl where user_id=@D1 limit 1";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);

                _D1.Value = tempUserName;
                cmd.Parameters.Add(_D1);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            if (reader.GetValue(2).ToString().Trim() == tempUserName)//first level security at mySQL
                            {
                                if (CheckAxaptaConnStatus(tempUserName, tempaxPWD))
                                {
                                    UserPasswordStatus Status = GetUser();

                                    if (Status == UserPasswordStatus.Change)
                                    {
                                        Response.Redirect("ChangePassword.aspx?expired=true");
                                        return false;
                                    }
                                    else if (Status == UserPasswordStatus.Expired)
                                    {
                                        Function_Method.MsgBox("Password has been expired, please contact IT Department to renew password.", this.Page, this);

                                        return false;
                                    }
                                    else if (Status == UserPasswordStatus.NotFound)
                                    {
                                        Function_Method.MsgBox("User not found.", this.Page, this);

                                        return false;
                                    }
                                    else if (Status == UserPasswordStatus.SystemErr)
                                    {
                                        Function_Method.MsgBox("Server down.", this.Page, this);

                                        return false;
                                    }

                                    //password match
                                    //pass information to main page==================================

                                    Session["axPWD"] = tempaxPWD;
                                    Session["logined_user_name"] = reader.GetValue(1).ToString();
                                    Session["user_id"] = reader.GetValue(2).ToString();
                                    Session["user_authority_lvl"] = reader.GetInt32(3);
                                    Session["page_access_authority"] = reader.GetInt32(4);
                                    Session["user_company"] = reader.GetValue(5).ToString();
                                    Session["module_access_authority"] = reader.GetInt32(6);

                                    Session["switch_Company"] = reader.GetValue(5).ToString();
                                    Session["flag_temp"] = 0;
                                    Session["system_checking"] = 0;
                                    Session["data_passing"] = "";
                                    Session["user_authority_lvl_Red"] = Convert.ToInt32(Session["user_authority_lvl"]);
                                    Session["logined_user_name_Red"] = Session["logined_user_name"].ToString();
                                    conn.Close();
                                    return true;
                                }
                                else
                                {
                                    Function_Method.MsgBox("Incorrect Password.", this.Page, this);
                                }
                                //password match
                                //pass information to main page==================================

                                //Session["axPWD"] = tempaxPWD;
                                //Session["logined_user_name"] = reader.GetValue(1).ToString();
                                //Session["user_id"] = reader.GetValue(2).ToString();
                                //Session["user_authority_lvl"] = reader.GetInt32(3);
                                //Session["page_access_authority"] = reader.GetInt32(4);
                                //Session["user_company"] = reader.GetValue(5).ToString();
                                //Session["module_access_authority"] = reader.GetInt32(6);

                                //Session["switch_Company"] = reader.GetValue(5).ToString();
                                //Session["flag_temp"] = 0;
                                //Session["system_checking"] = 0;
                                //Session["data_passing"] = "";
                                //Session["user_authority_lvl_Red"] = Convert.ToInt32(Session["user_authority_lvl"]);
                                //Session["logined_user_name_Red"] = Session["logined_user_name"].ToString();
                                //conn.Close();
                                //return true;
                            }
                        }
                    }
                    conn.Close();
                    Function_Method.MsgBox("Not a Registered System Database User.", this.Page, this);
                    return false;
                }
            }
            catch (Exception LoginError)
            {
                Function_Method.MsgBox(LoginError.Message, this.Page, this);
                return false;
            }
        }

        private UserPasswordStatus GetUser()
        {
            UserPasswordStatus Status = UserPasswordStatus.OK;
            try
            {
                string UserId = TextBox1.Text.Trim();

                /*
                 * 2025-08-20 Jerry - If running on VS, domainPath will be null as local user won't be able to connect to ActiveDirectory.
                                      Returns UserPasswordStatus.OK to skip checking and continue the login process.
                 */
                string domainPath = ChangePassword.GetCurrentDomainPath();
                if (string.IsNullOrEmpty(domainPath))
                {
                    //domainPath = "LDAP://LIONPB.com.my";
                    return Status;
                }

                DirectoryEntry de = new DirectoryEntry(domainPath);



                DirectorySearcher deSearch = new DirectorySearcher();
                deSearch.SearchRoot = de;

                deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserId + "))";
                deSearch.SearchScope = SearchScope.Subtree;
                deSearch.PropertiesToLoad.Add("msDS-UserPasswordExpiryTimeComputed");
                SearchResult results = deSearch.FindOne();

                if(results == null)
                {
                    Status = UserPasswordStatus.NotFound;
                }

                if (!(results == null))
                {
                    de = new DirectoryEntry(results.Path, UserId, TextBox2.Text, AuthenticationTypes.Secure);
                    //de = new DirectoryEntry(results.Path);
                    DateTime ExpiredDate = DateTimePropertyFromLong(results, "msDS-UserPasswordExpiryTimeComputed");

                    DateTime DateNow = DateTime.Now.Date;

                    if ((ExpiredDate - DateNow).TotalDays <= 14 && (ExpiredDate - DateNow).TotalDays > 0)
                    {
                        Status = UserPasswordStatus.Change;

                        return Status;
                    }
                    else if ((ExpiredDate - DateNow).TotalDays < 0)
                    {
                        Status = UserPasswordStatus.Expired;
                    }

                    return Status;
                }
                else
                {
                    Status = UserPasswordStatus.NotFound;

                    return Status;
                }
            }
            catch
            {
                Status = UserPasswordStatus.SystemErr;

                return Status;
            }
        }


        public static DateTime DateTimePropertyFromLong(SearchResult sr, string propName)
        {
            if (!sr.Properties.Contains(propName)) return DateTime.MinValue;
            var value = (long)sr.Properties[propName][0];
            return value == long.MaxValue ? DateTime.MinValue : DateTime.FromFileTimeUtc(value);
        }



        private enum UserPasswordStatus
        {
            OK = 1,
            Change = 2,
            Expired = 3,
            NotFound = 4,
            SystemErr = 5

        }

        private bool CheckAxaptaConnStatus(string tempUserName, string tempaxPWD)
        {
            Axapta DynAx = new Axapta();
            try
            {
                //DynAx.LogonAs(tempUserName, GLOBAL.DomainName, new System.Net.NetworkCredential("axbcproxy", "aos20@9", GLOBAL.DomainName), GLOBAL.Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(tempUserName, GLOBAL.DomainName, new System.Net.NetworkCredential(tempUserName, tempaxPWD, GLOBAL.DomainName), GLOBAL.Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                
                return true;
            }
            catch (Exception ex)
            {
                //Function_Method.MsgBox(ex.Message, this.Page, this);
                return false;
            }
        }

        private void system_time_format()
        {
            GLOBAL.system_checking = 0;
            string correct_time_format = DateTime.Now.ToString("dd MM yyyy");
            string[] arr_correct_time_format = correct_time_format.Split(' ');
            string correct_day = arr_correct_time_format[0];
            string correct_month = arr_correct_time_format[1];

            string check_time_format = DateTime.Now.ToString();//19/8/2020 2:06:54 PM
            string temp_check_time_format = check_time_format.Replace("/", " ");
            string[] arr_temp_check_time_format = temp_check_time_format.Split(' ');
            string check_day = arr_temp_check_time_format[0];
            string check_month = arr_temp_check_time_format[1];

            if (check_month.Length < 2)
            {
                check_month = "0" + check_month;
            }
            if (check_day.Length < 2)
            {
                check_day = "0" + check_day;
            }
            if ((check_month == correct_month) && (check_day == correct_day))
            {
                GLOBAL.system_checking = 0x01;
            }
            else
            {
                GLOBAL.system_checking = 0x00;
            }
            Session["system_checking"] = GLOBAL.system_checking.ToString();
        }

    }
}



