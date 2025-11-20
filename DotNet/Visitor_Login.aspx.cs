using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Visitor_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GLOBAL.system_checking = 0;//test();
            if (Request.QueryString["changedpassword"] == "true")
            {
                Function_Method.MsgBox("Password have been reset.", this.Page, this);
            }

            if (!IsPostBack)
            {
                //initialize======================================================
                Session.Abandon();
                Session.Clear();
                Response.Cookies.Clear();
                Label1.Text = GLOBAL.version_control;
                TextBox1.Attributes.Add("autocomplete", "off");

                //================================================================
                //this.Form.Target = "_blank";//enable it if you want to open in new tab
                //SetFocus(TextBox1);
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

                if (CheckMYSQLConn(tempUserName, tempaxPWD) == true)
                {
                    system_time_format();
                    Response.Redirect("Visitor_MainMenu.aspx");
                }
            }
        }
        private bool CheckMYSQLConn(string tempUserName, string tempaxPWD)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

                string Query = "select * from visitor_userlogin where UserName=@D1 limit 1";

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
                            if (reader.GetValue(0).ToString().Trim() == tempUserName && reader.GetValue(8).ToString().Trim() == tempaxPWD)//first level security at mySQL
                            {
                                //password match
                                //pass information to main page==================================

                                Session["UserPassword"] = tempaxPWD;
                                Session["UserName"] = reader.GetValue(0).ToString();
                                Session["UserFullName"] = reader.GetValue(1).ToString();
                                Session["UserRole"] = reader.GetInt32(2);
                                Session["UserCompany"] = reader.GetValue(3).ToString();
                                Session["switch_Company"] = "PPM";
                                conn.Close();
                                return true;
                            }
                            else if (reader.GetValue(0).ToString().Trim() == tempUserName && reader.GetValue(8).ToString().Trim() != tempaxPWD)
                            {
                                Function_Method.MsgBox("Incorrect Password, Please Try Again.", this.Page, this);
                                return false;
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

        public static void system_time_format()
        {
            var context = HttpContext.Current; // Get the current HTTP context

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
            context.Session["system_checking"] = GLOBAL.system_checking.ToString();
        }

    }
}



