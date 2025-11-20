using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNetSync.lhdn.EInvoiceLoginPage;

namespace DotNetSync.lhdn
{
    public partial class EInvoiceMaster : System.Web.UI.MasterPage
    {
        public static int SUBMISSION_NEW = 0;
        public static int SUBMISSION_UUID_FAILED = 2;
        public static int SUBMISSION_LONG_ID_PENDING = 3;
        public static int SUBMISSION_LONG_ID_FAILED = 4;
        public static int SUBMISSION_COMPLETED = 5;

        public static Constants constants = new Constants();
        public static List<string> access_companies = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["einvoice_user_name"] = "Jerry Yong";
            //Session["einvoice_user_id"] = "yongwc@lionpb.com.my";

            // Get the current page's file name
            string currentPage = System.IO.Path.GetFileName(Request.Path);

            // Check if the current page is the one that should skip session validation
            if (currentPage.Equals("PageToSkip.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return; 
            }

            // Check if the current page is the one that should skip session validation
            if (currentPage.Equals("EInvoiceLoginPage.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return; 
            }

            // Perform session validation for other pages
            if (Session["einvoice_user_name"] == null)
            {
                Response.Redirect("EInvoiceLoginPage.aspx");
            }

            access_companies = Session["access_companies"] as List<string>;
        }
    }
}