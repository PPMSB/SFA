using GLOBAL_VAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DotNet.Visitor_MainMenu;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using ZXing;

namespace DotNet
{
    public class Campaign_CheckSession : System.Web.UI.Page
    {
        public static void SystemCheckSession(Page CurrentPage, object Page)
        {
            var context = HttpContext.Current; // Get the current HTTP context

            try
            {
                //context.Session["UserName"] = null;
                string.IsNullOrEmpty(context.Session["user_id"].ToString().Trim());

                GLOBAL.axPWD = context.Session["axPWD"].ToString();
                GLOBAL.logined_user_name = context.Session["logined_user_name"].ToString();
                GLOBAL.user_authority_lvl = Convert.ToInt32(context.Session["user_authority_lvl"]);
                GLOBAL.page_access_authority = Convert.ToInt32(context.Session["page_access_authority"]);
                GLOBAL.user_company = context.Session["user_company"].ToString();
                GLOBAL.module_access_authority = Convert.ToInt32(context.Session["module_access_authority"]);
                GLOBAL.switch_Company = context.Session["switch_Company"].ToString();
                GLOBAL.system_checking = Convert.ToInt32(context.Session["system_checking"]);
                context.Session["data_passing"] = "";
                GLOBAL.data_passing = context.Session["data_passing"].ToString();

                GLOBAL.CampaignReport = false;
                context.Session["CampaignReport"] = false;
                List<string> EPUsers = GiveExtraPermission();
                if (EPUsers.Contains(context.Session["user_id"].ToString().Trim()))
                {
                    context.Session["CampaignReport"] = true;
                    GLOBAL.CampaignReport = true;
                }

            }
            catch
            {
                context.Response.Redirect("LoginPage.aspx");
            }
        }

        public static List<string> GiveExtraPermission()
        {
            List<string> EPUsers = new List<string> { "phuasp", "tseec", "tanwl", "foozm" };

            return EPUsers;
        }

        public static void TimeOutRedirect(Page CurrentPage, object Page)
        {
            var context = HttpContext.Current;
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(context.Session.Timeout * 60) + ";url=LoginPage.aspx";

            CurrentPage.Header.Controls.Add(meta);
        }
    }
}