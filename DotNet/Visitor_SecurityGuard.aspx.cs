using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Visitor_SecurityGuard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string qrCodeImageUrl = Function_Method.GenerateQRCode(GLOBAL.ProductionWebsite + "Visitor_MainMenu.aspx?security=true");
            QRCodeImage.ImageUrl = qrCodeImageUrl;
        }

        protected void RedirectSecurityToSystem(object sender, EventArgs e)
        {
            Response.Redirect("Visitor_MainMenu.aspx?security=true");
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string fileExtension = Path.GetExtension(FileUpload1.FileName).ToLower();
                    using (var stream = FileUpload1.PostedFile.InputStream)
                    {
                        List<TestingModel> products = GlobalHelper.ImportExcel<TestingModel>(stream, fileExtension);
                    }
                }
                catch (Exception ex)
                {
                    Function_Method.MsgBox("Incorrect Excel format: " + ex.Message, this.Page, this);
                }
            }
            else
            {
                Function_Method.MsgBox("Please upload an excel file.", this.Page, this);
            }
        }


        public class TestingModel
        {
            public string Name { get; set; }
            public string Phone {  get; set; }
            public string Address { get; set; }
        }

    }

}