using GLOBAL_FUNCTION;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.IO;
using System.Web;


namespace DotNet
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Check_DataRequest();
            }
        }

        private void Check_DataRequest()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            string custAccount = ""; string statementDate = "";

            try
            {
                string temp1 = Request.QueryString["file"] ?? Session["data_passing"]?.ToString();
                if (!string.IsNullOrEmpty(temp1))//data receive
                {
                    string[] filePaths = new string[50];
                    string[] split = new string[10];
                    string selectedFile = "";
                    if (temp1.Substring(0, 9) == "_CUSTACC@")
                    {
                        split = temp1.Split('@', '|');
                        statementDate = split[1].ToString();
                        custAccount = split[2].ToString();

                        filePaths = Directory.GetFiles("e:/DN-Statement/" + statementDate.Substring(0, 4) + "/");
                        selectedFile = statementDate + "_" + custAccount + ".pdf";
                    }
                    else
                    {
                        split = temp1.Split('@', '/');
                        filePaths = Directory.GetFiles(split[1] + "/" + split[2] + "/" + split[3] + "/" + split[4]);
                        selectedFile = Path.GetFileName(split[5].ToString());
                    }

                    foreach (string filePath in filePaths)
                    {                        
                        string fileName = Path.GetFileName(filePath.ToString());
                        if (fileName == selectedFile)
                        {
                            // Set cache control headers to prevent caching
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetNoStore();
                            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                            Response.AddHeader("Pragma", "no-cache");
                            Response.AddHeader("Expires", "0");
                            //string path = Server.MapPath("~/PDF/");
                            Response.ContentType = "application/pdf";
                            string downloadName = fileName;
                                                           // Add timestamp to break cache
                            if (Request.Browser.IsMobileDevice)
                            {
                                downloadName = Path.GetFileNameWithoutExtension(fileName) +
                                              "_" + DateTime.Now.Ticks +
                                              Path.GetExtension(fileName);
                            }

                            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
                            Response.Clear();
                            Response.WriteFile(filePath);
                            Response.Flush();
                            Response.End();
                            return;
                        }
                    }
                    Session["data_passing"] = "";
                }
            }
            catch (Exception ER_DL_00)
            {
                Function_Method.MsgBox(ER_DL_00.ToString() + ": Failed to retrieve PDF.", this.Page, this);
            }
        }
    }
}