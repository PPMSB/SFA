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
    public partial class Visitor_CheckInOutQRCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string qrCodeImageUrl = Function_Method.GenerateQRCode(GLOBAL.ProductionWebsite + "Visitor_CheckInOut.aspx");
            QRCodeImage.ImageUrl = qrCodeImageUrl;
        }

        protected void RedirectSecurityToSystem(object sender, EventArgs e)
        {
            Response.Redirect("Visitor_CheckInOut.aspx");
        }
    }
}