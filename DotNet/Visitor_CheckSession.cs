using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using static DotNet.Visitor_MainMenu;

namespace DotNet
{
    public class Visitor_CheckSession : System.Web.UI.Page
    {

        public static void CheckSession(Page CurrentPage, object Page)
        {
            var context = HttpContext.Current; // Get the current HTTP context

            try
            {
                //context.Session["UserName"] = null;
                string.IsNullOrEmpty(context.Session["UserName"].ToString().Trim());

                GLOBAL.UserRole = (UserRoleType)Convert.ToInt32(context.Session["UserRole"]);
                GLOBAL.user_company = context.Session["UserCompany"].ToString();
                GLOBAL.user_id = context.Session["UserName"].ToString();
                GLOBAL.logined_user_name = context.Session["UserFullName"].ToString();
                GLOBAL.switch_Company = context.Session["switch_Company"].ToString();
            }
            catch
            {
                context.Response.Redirect("LoginPage.aspx");
            }
        }

        public static void AssignSecuritySession(Page CurrentPage, object Page)
        {
            var context = HttpContext.Current;

            context.Session["UserRole"] = "3";
            context.Session["UserPassword"] = "";
            context.Session["UserName"] = "Security";
            context.Session["UserFullName"] = "Security";
            context.Session["UserCompany"] = "";
            context.Session["switch_Company"] = "PPM";

            context.Response.Redirect("Visitor_MainMenu.aspx");
        }

        public static void TimeOutRedirect(Page CurrentPage, object Page)
        {
            var context = HttpContext.Current;
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(context.Session.Timeout * 60) + ";url=LoginPage.aspx";

            CurrentPage.Header.Controls.Add(meta);
        }


        public static void Wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                return;
            }
        }

        //public static void CheckAccess(int UserRole)
        //{
        //    if (UserRole == 3)
        //    {

        //    }
        //}

        ////Hardcode first
        //public class AccessModel
        //{
        //    public string Name { get; set; }
        //}
    }
}