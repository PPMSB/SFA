using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using iText.StyledXmlParser.Jsoup.Helper;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Campaign_MainMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Campaign_CheckSession.SystemCheckSession(this.Page, this);

        }

        public void DownloadInformation(object sender, EventArgs e)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                using (ZipArchive archive = new ZipArchive(memStream, ZipArchiveMode.Create, true, null))
                {
                    string path = HttpContext.Current.Server.MapPath("~/RESOURCES/VPPPCampaign");

                    foreach (string docFile in Directory.GetFiles(path))
                    {
                        string fileName = Path.GetFileName(docFile);
                        ZipArchiveEntry entry = archive.CreateEntry(fileName);

                        using (BinaryWriter writer = new BinaryWriter(entry.Open()))
                        {
                            writer.Write(File.ReadAllBytes(docFile));
                        }
                    }
                }

                SendZipToClient(memStream);
            }
            catch (Exception LoginError)
            {
                Function_Method.MsgBox(LoginError.Message, this.Page, this);
            }
        }

        private void SendZipToClient(MemoryStream memStream)
        {
            var context = HttpContext.Current;

            memStream.Seek(0, SeekOrigin.Begin);

            if (memStream != null && memStream.Length > 0)
            {
                context.Response.Clear();
                context.Response.AddHeader("content-disposition", "attachment; filename=VPPPCampaign.zip");
                context.Response.ContentType = "application/zip";
                context.Response.BinaryWrite(memStream.ToArray());
                context.Response.End();
            }
        }
    }
}