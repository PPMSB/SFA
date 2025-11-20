using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZXing;
using DotNet.Visitor_Model;
using System.Runtime.Remoting.Contexts;
using MySql.Data.MySqlClient;
using System.Web.Services;
using static DotNet.Visitor_Model.AppointmentModel;
using System.Configuration;

namespace DotNet
{
    public partial class Visitor_MainMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserRoleType UserRole = (UserRoleType)Convert.ToInt32(Session["UserRole"]);

            NavigationItemsModel m = new NavigationItemsModel();
            m.NewAppointment = null;
            m.NewAppointmentTag = NewAppointmentTag;
            m.AppointmentHistory = null;
            m.AppointmentHistoryTag = AppointmentHistoryTag;
            m.SFA = null;
            m.SFATag = SFATag;

            if (UserRole == UserRoleType.Guest)
            {
                Function_Method.MsgBox("No Permission to access.", this.Page, this);

                Response.Redirect("Visitor_Login.aspx");
            }

            if (Request.QueryString["security"] == "true")
            {
                Visitor_Login.system_time_format();

                Visitor_CheckSession.AssignSecuritySession(this.Page, this);
            }
            //else
            //{
            //    Visitor_CheckSession.CheckSession(this.Page, this);
            //    Visitor_CheckSession.TimeOutRedirect(this.Page, this);
            //}

            if (!IsPostBack)
            {
                Function_Method.LoadVisitorSelectionMenu(UserRole, m);
            }
        }

        [WebMethod]
        public static void RedirectToMainMenu(string UserName)
        {
            try
            {
                var context = HttpContext.Current;

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

                string Query = "select * from visitor_userlogin where UserName=@D1 limit 1";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);

                _D1.Value = UserName;
                cmd.Parameters.Add(_D1);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            if (reader.GetValue(0).ToString().Trim() == UserName)//first level security at mySQL
                            {
                                //password match
                                //pass information to main page==================================

                                context.Session["UserName"] = reader.GetValue(0).ToString();
                                context.Session["UserFullName"] = reader.GetValue(1).ToString();
                                context.Session["UserRole"] = reader.GetInt32(2);
                                context.Session["UserCompany"] = reader.GetValue(3).ToString();
                                context.Session["switch_Company"] = "PPM";
                                conn.Close();
                                //context.Response.Redirect("Visitor_MainMenu.aspx");
                                return;
                            }
                        }
                    }

                    UserLoginModel m = new UserLoginModel();
                    string TableName = "visitor_userlogin";
                    List<object> ObjectList = StoreAllVariables();
                    List<string> ColumnList = GlobalHelper.GetColumnsByModel(m);
                    Dictionary<string, object> ParamDict = GlobalHelper.ConvertModelColumnsIntoDictionary(ColumnList, ObjectList);

                    GlobalHelper.InsertQuery(TableName, ColumnList, ParamDict);

                    context.Session["UserName"] = context.Session["user_id"].ToString();
                    context.Session["UserFullName"] = context.Session["logined_user_name"].ToString();
                    context.Session["UserRole"] = (int)UserRoleType.Staff;
                    context.Session["UserCompany"] = context.Session["user_company"].ToString();
                    context.Session["switch_Company"] = "PPM";
                    conn.Close();
                    //Function_Method.MsgBox("Not a Registered System Database User.", this.Page, this);
                }
            }
            catch (Exception LoginError)
            {
                //Function_Method.MsgBox(LoginError.Message, this.Page, this);
            }
        }

        private static List<object> StoreAllVariables()
        {
            var context = HttpContext.Current;

            List<object> ObjectList = new List<object>();
            //Must order in sequence based on model sequence
            ObjectList.Add(context.Session["user_id"].ToString());
            ObjectList.Add(context.Session["logined_user_name"].ToString());
            ObjectList.Add((int)UserRoleType.Staff);
            ObjectList.Add(context.Session["user_company"].ToString());
            ObjectList.Add(DateTime.Now);
            ObjectList.Add(context.Session["user_id"].ToString());
            ObjectList.Add(DateTime.MinValue);
            ObjectList.Add("");
            ObjectList.Add(context.Session["axPWD"]);
            ObjectList.Add(false);

            return ObjectList;
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

        public enum UserRoleType
        {
            HR = 1,
            Staff = 2,
            Security = 3,
            Guest = 4,
        }
    }
}