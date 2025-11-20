using DotNet.Visitor_Model;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNet.Visitor_MainMenu;

namespace DotNet
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            UserRoleType UserRole = (UserRoleType)Convert.ToInt32(Session["UserRole"]);
            NavigationItemsModel m = new NavigationItemsModel();
            m.NewAppointment = NewAppointment;
            m.NewAppointmentTag = null;
            m.AppointmentHistory = AppointmentHistory;
            m.AppointmentHistoryTag = null;
            m.SFA = SFA;
            m.SFATag = null;
            Visitor_CheckSession.CheckSession(this.Page, this);

            GLOBAL.logined_user_name = context.Session["UserFullName"].ToString();
            GLOBAL.switch_Company = context.Session["switch_Company"].ToString();

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

                Function_Method.LoadVisitorSelectionMenu(UserRole, m);
            }

            //if (UserRole == UserRoleType.Security)
            //{
            //    Function_Method.MsgBox("No Permission to access.", this.Page, this);

            //    Response.Redirect("Visitor_MainMenu.aspx");
            //}
            if (Request.QueryString["guest"] == "true")
            {
                Function_Method.LoadVisitorSelectionMenu(UserRole, m);

            }
            else
            {
                Visitor_CheckSession.CheckSession(this.Page, this);
                Visitor_CheckSession.TimeOutRedirect(this.Page, this);
                Function_Method.LoadVisitorSelectionMenu(UserRole, m);

            }


        }
    }
}