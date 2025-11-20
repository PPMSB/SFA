using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using QRCoder;
using System;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Drawing; 
using System.Drawing.Imaging; 
using System.IO;
using System.Windows.Forms;
using System.Linq;
using DotNet.Visitor_Model;
using static DotNet.Visitor_MainMenu;
using System.Configuration;
using System.Text.RegularExpressions;
using NLog;

namespace GLOBAL_FUNCTION
{
    public class Function_Method
    {
        public static bool isPBM;
        public static bool isWarranty;
        public static bool isPoonshReport;
        public static bool isVPPPCampaign = false;

        public static Axapta globalAxapta = null;

        // Jerry 2024-12-23 Sanitize file name
        public static string SanitizeFilename(string filename)
        {
            // List of invalid characters in Windows filenames
            char[] invalidChars = Path.GetInvalidFileNameChars();

            // Replace each invalid character with an empty string
            foreach (char invalidChar in invalidChars)
            {
                filename = filename.Replace(invalidChar.ToString(), string.Empty);
            }

            // Optionally, you can also check for reserved filenames (e.g., "CON", "PRN", etc.)
            string[] reservedFilenames = new[] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

            if (reservedFilenames.Contains(Path.GetFileNameWithoutExtension(filename).ToUpper()))
            {
                filename = "_" + filename; // Prepend an underscore if filename is reserved
            }

            return filename;
        }
        // Jerry 2024-12-23 Sanitize file name - END

        public static string get_correct_date(int system_checking_date, string raw_date, bool check_system)
        {
            string correct_date = raw_date;
            string[] arr_raw_date = raw_date.Split('/');//  MM/dd/yyyy

            if (arr_raw_date[0].Length < 2)
            {
                arr_raw_date[0] = "0" + arr_raw_date[0];
            }
            if (arr_raw_date[1].Length < 2)
            {
                arr_raw_date[1] = "0" + arr_raw_date[1];
            }
            if (check_system == true)
            {
                if ((system_checking_date & 0x01) != 0)//correct format
                {
                    correct_date = arr_raw_date[0] + "/" + arr_raw_date[1] + "/" + arr_raw_date[2];
                }
                else
                {
                    correct_date = arr_raw_date[1] + "/" + arr_raw_date[0] + "/" + arr_raw_date[2];
                }
            }
            else
            {
                correct_date = arr_raw_date[0] + "/" + arr_raw_date[1] + "/" + arr_raw_date[2];
            }
            return correct_date;
        }

        public static void MsgBox(String ex, Page pg, Object obj)
        {
            string show_messsage = ex;

            /*
            show_messsage = "";
            if (ex.Length>150)//too long message
            {
                string temp = ex.Substring(150);
                String[] arrtemp = temp.Split(' ');
                show_messsage = ex.Substring(0,150) + arrtemp[0] + " ......";
            }
            else
            {
                show_messsage = ex;
            }*/

            show_messsage = show_messsage.Replace("\r", "").Replace("\n", "").Replace("'", "").Replace("(", "").Replace(")", "").Replace("\\", " ");
            show_messsage = "alert('" + show_messsage + "');";
            ScriptManager.RegisterClientScriptBlock(pg, pg.GetType(), "alertscript", show_messsage, true);

            //If you have update panel use ScriptManager.RegisterClientScriptBlock as above
        }

        public static int[] paging_grid(int PAGE_INDEX)
        {
            /*
            Page INDEX      START       END
            0               1           20 
            1               21          40  
            2               41          60 
            */

            const int paging_size = 20;

            //int current_page_index = Convert.ToInt32(ViewState["currentpage"]);

            int temp_count = 0;
            for (int i = 0; i < PAGE_INDEX; i++)
            {
                temp_count += paging_size;
            }
            int startA = temp_count + 1;
            //int endA = temp_count + paging_size
            int endA = temp_count + paging_size;//add additional buffer according to paging_size

            int[] start_end_A = { startA, endA };
            return start_end_A;
        }

        public static void SendMail(string UserId, string UserName, string Subject, string SendMailTo, string MailCC, string Sendmsg)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("10.1.1.199");

            string AliasEmail = "@posim.com.my";
            string UserEmail = UserId + AliasEmail;

            mail.From = new MailAddress(UserEmail);
            string BCCEmailDeveloper = ConfigurationManager.AppSettings["BCCEmailDeveloper"].ToString();

            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword);

            try
            {
                
                if (GLOBAL.debug == true)
                {
                    //Subject = "[IT Testing] " + Subject;
                    //SendMailTo = DeveloperEmail;
                    //MailCC = DeveloperEmail;
                }

                string MailTo = UserEmail;//default
                if (SendMailTo != "") MailTo = /*MailTo + "," +*/ SendMailTo;

                mail.To.Add(MailTo);

                if (MailCC != null)
                {
                    if (MailCC.Contains(","))
                    {
                        var split = MailCC.Split(',');
                        foreach (var item in split)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                mail.CC.Add(item.ToString());
                            }
                        }
                    }
                    else
                    {
                        mail.CC.Add(MailCC);
                    }
                }


                mail.Subject = Subject;

                if (!isWarranty)//only get daily and monthly report
                {
                    mail.Bcc.Add("tseec@posim.com.my");
                    mail.Bcc.Add(BCCEmailDeveloper);
                }

                if (isPBM)
                {
                    mail.Bcc.Add("tseec@posim.com.my");
                    mail.Bcc.Add(BCCEmailDeveloper);
                }

                if (isPoonshReport)
                {
                    mail.Bcc.Add("tseec@posim.com.my");
                    mail.Bcc.Add(BCCEmailDeveloper);
                }
                if (isVPPPCampaign)
                {
                    mail.Bcc.Add("hr_campaign@posim.com.my");
                }

                string Company_EtnicCode = "ACCOUNTABLE . COMPETENT . TRANSPARENT";
                if (Sendmsg != "")
                {
                    if (Subject.Contains("Sales Report"))
                    {
                        mail.IsBodyHtml = true;
                        Sendmsg = Sendmsg + "<br><br>" + "Thank you." + "<br>" +
                            Company_EtnicCode + "<br>" + UserName + "<br>" + "http://sfa.posim.com.my/";
                    }
                    else
                    {
                        Sendmsg = Sendmsg + "\n" + "\n" +
                                "Thank you." + "\n" + "\n" + Company_EtnicCode + "\n" + "\n" +
                                UserName + "\n" + "\n" + "http://sfa.posim.com.my/";
                    }
                    mail.Body = Sendmsg;
                }

                SmtpServer.Send(mail);

                SmtpServer.Dispose();
            }
            catch (Exception ex)
            {
                AddLog("> Email failed to send. " + ex.Message);
                throw;
            }
        }

        public static void WriteLog(string msg, string _LogFileName)
        {
            string strLogFileName = _LogFileName + ".txt";
            string FullPath = Path.Combine(GLOBAL.LogFilePath, strLogFileName);

            try
            {
                if (!Directory.Exists(GLOBAL.LogFilePath))
                    Directory.CreateDirectory(GLOBAL.LogFilePath);

                using (StreamWriter sw = new StreamWriter(FullPath, true))
                {
                    string TimeStamp = DateTime.Now.ToString("[HH:mm:ss] ");
                    sw.WriteLine(TimeStamp + msg);
                }
            }
            catch
            {

            }
        }
        /*//functional but not using because found faster methods
        public static string GetPropertyValue_LargeInteger(DirectoryEntry de, string ID, string propertyName)
        {
            string ret = string.Empty;
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + ID + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if (!(results == null))
            {
                de = new DirectoryEntry(results.Path);
               
                LargeInteger largeInt = (LargeInteger)de.Properties[propertyName][0];
                Int64 liTicks = largeInt.HighPart * 0x100000000 + largeInt.LowPart;
                DateTime? dTemp = null;
                if (DateTime.MaxValue.Ticks >= liTicks && DateTime.MinValue.Ticks <= liTicks)
                {
                    dTemp = DateTime.FromFileTime(liTicks);
                }
                ret = dTemp.ToString();
            }
            return ret;
        }*/
        public static DateTime? DateTimePropertyFromLong(SearchResult sr, string propertyName)
        {
            if (!sr.Properties.Contains(propertyName)) return null;
            var value = (long)sr.Properties[propertyName][0];
            return value == long.MaxValue ? (DateTime?)null : DateTime.FromFileTime(value);
        }

        public static void LoadSelectionMenu(int temp_module_access_authority,
            HtmlAnchor CustomerMasterTag, HtmlAnchor CustomerMasterTag2,
            HtmlAnchor SFATag, HtmlAnchor SFATag2,
            HtmlAnchor SalesQuotation, HtmlAnchor SalesQuotation2,
            HtmlAnchor PaymentTag, HtmlAnchor PaymentTag2,
            HtmlAnchor RedemptionTag, HtmlAnchor RedemptionTag2,
            HtmlAnchor InventoryMasterTag, HtmlAnchor InventoryMasterTag2,
            HtmlAnchor EORTag, HtmlAnchor EORTag2,
            HtmlAnchor CheckInTag, HtmlAnchor CheckInTag2,
            HtmlAnchor WClaimTag, HtmlAnchor WClaimTag2,
            HtmlAnchor EventBudgetTag, HtmlAnchor EventBudgetTag2,
            HtmlAnchor NewCustomerTag, HtmlAnchor NewCustomerTag2,
            HtmlAnchor MapTag, HtmlAnchor MapTag2, HtmlAnchor RocTinTag, HtmlAnchor RocTinTag2,
            HtmlAnchor NewProduct2
            )
        {

            for (int i = 0; i < GLOBAL.no_of_module; i++)
            {
                if ((temp_module_access_authority & GLOBAL.ConversionData[i]) != 0 || temp_module_access_authority == 255)//255 is admin
                {
                    if (i == 0)
                    {
                        if (CustomerMasterTag != null)
                        {
                            CustomerMasterTag.Visible = true;
                        }
                        if (CustomerMasterTag2 != null)
                        {
                            CustomerMasterTag2.Visible = true;
                        }
                    }
                    if (i == 1)
                    {
                        if (SFATag != null)
                        {
                            SFATag.Visible = true;
                        }
                        if (SFATag2 != null)
                        {
                            SFATag2.Visible = true;
                        }

                        if (SalesQuotation != null)
                        {
                            SalesQuotation.Visible = true;
                        }
                        if (SalesQuotation2 != null)
                        {
                            SalesQuotation2.Visible = true;
                        }

                    }
                    if (i == 2)
                    {
                        if (PaymentTag != null)
                        {
                            PaymentTag.Visible = true;
                        }
                        if (PaymentTag2 != null)
                        {
                            PaymentTag2.Visible = true;
                        }
                    }
                    if (i == 3)
                    {
                        if (RedemptionTag != null)
                        {
                            RedemptionTag.Visible = true;
                        }
                        if (RedemptionTag2 != null)
                        {
                            RedemptionTag2.Visible = true;
                        }
                    }
                    if (i == 4)
                    {
                        if (InventoryMasterTag != null)
                        {
                            InventoryMasterTag.Visible = true;
                        }
                        if (InventoryMasterTag2 != null)
                        {
                            InventoryMasterTag2.Visible = true;
                        }
                    }
                    if (i == 5)
                    {
                        if (EORTag != null)
                        {
                            EORTag.Visible = true;
                        }
                        if (EORTag2 != null)
                        {
                            EORTag2.Visible = true;
                        }                       

                    }
                    if (i == 6)
                    {
                        if (CheckInTag != null)
                        {
                            CheckInTag.Visible = true;
                        }

                        if (NewCustomerTag != null)
                        {
                            NewCustomerTag.Visible = true;
                        }
                        if (NewCustomerTag != null)
                        {
                            NewCustomerTag2.Visible = true;
                        }
                    }
                    if (i == 7)
                    {
                        if (WClaimTag != null)
                        {
                            WClaimTag.Visible = true;
                        }
                        if (WClaimTag2 != null)
                        {
                            WClaimTag2.Visible = true;
                        }
                        //if (RocTinTag != null)
                        //{
                        //    RocTinTag2.Visible = true;
                        //}
                        //if (RocTinTag2 != null)
                        //{
                        //    RocTinTag2.Visible = true;
                        //}                    
                        if (MapTag != null)
                        {
                            MapTag.Visible = true;
                        }
                        if (MapTag != null)
                        {
                            MapTag2.Visible = true;
                        }                       
                    }
                    if (i == 9)
                    {
                        if (EventBudgetTag != null)
                        {
                            EventBudgetTag.Visible = true;
                        }
                        if (EventBudgetTag != null)
                        {
                            EventBudgetTag2.Visible = true;
                        }
                    }
                    if (i == 10)
                    {
                        if (NewProduct2 != null)
                        {
                            NewProduct2.Visible = true;
                        }
                    }
                }
            }
        }

        public static void LoadVisitorSelectionMenu(UserRoleType UserRole, NavigationItemsModel m)
        {
            Type mType = m.GetType();

            bool CheckRole = (UserRole != UserRoleType.Security ? true : false);

            foreach (var item in mType.GetProperties())
            {
                var propertyInfo = mType.GetProperty(item.Name);

                if (((item.Name == "NewAppointment" || item.Name == "NewAppointmentTag") && CheckRole) || item.Name != "NewAppointment" && item.Name != "NewAppointmentTag")
                {
                    if (propertyInfo != null && propertyInfo.PropertyType == typeof(HtmlAnchor))
                    {
                        if ((propertyInfo.GetValue(m) as HtmlAnchor) != null)
                        {
                            (propertyInfo.GetValue(m) as HtmlAnchor).Visible = true;

                        }
                    }
                }

                if ((item.Name != "NewAppointment" || item.Name == "NewAppointmentTag") && UserRole == UserRoleType.Guest)
                {
                    if (propertyInfo != null && propertyInfo.PropertyType == typeof(HtmlAnchor))
                    {
                        if ((propertyInfo.GetValue(m) as HtmlAnchor) != null)
                        {
                            (propertyInfo.GetValue(m) as HtmlAnchor).Visible = false;

                        }
                    }
                }


            }


        }
        public static void AddLog(string text)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\Administrator\\Desktop\\publishDotNet\\Weekly.log", true))
            {
                writer.WriteLine(DateTime.Now + " " + text);
            }
        }

        public static string GetLoginedUserFullName(string userId)
        {
            string loginedUserName = null;

            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                conn.Open();

                string query = "select logined_user_name from user_tbl where user_id = @UserId";

                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        loginedUserName = result.ToString();
                    }
                }
            }
            return loginedUserName;
        }
        public static Tuple<string, string, string> GetEmplTable_LF_EmplName_withDynAx(Axapta DynAx, string userId)
        { 
            string LF_EmplName = null;
            string EmplID = null;
            string ReportTo = null; // Added for ReportTo field
            //Axapta DynAx = GlobalAxapta();
            try
            {
                int EmplTable = DynAx.GetTableIdWithLock("Empltable");
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", EmplTable);

                string user_email = userId + "@posim.com.my";
                int LF_EmpEMailID = DynAx.GetFieldId(EmplTable, "LF_EmpEMailID");
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", LF_EmpEMailID);
                qbr.Call("value", user_email);

                using (AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery))
                {
                    if ((bool)axQueryRun.Call("next"))
                    {
                        using (AxaptaRecord record = (AxaptaRecord)axQueryRun.Call("Get", EmplTable))
                        {
                            EmplID = record.get_Field("EmplId").ToString();
                            ReportTo = record.get_Field("ReportTo").ToString(); // Get ReportTo field
                            LF_EmplName = record.get_Field("LF_EmplName")?.ToString();

                            // Check if LF_EmplName is null, empty, or whitespace
                            if (string.IsNullOrWhiteSpace(LF_EmplName))
                            {
                                LF_EmplName = record.get_Field("Del_Name")?.ToString();
                            }
                            //if (string.IsNullOrWhiteSpace(emplName) && row.Table.Columns.Contains("logined_user_name"))
                            //{
                            //    emplName = row["logined_user_name"].ToString();
                            //}

                            // Remove trailing information in parentheses
                            if (!string.IsNullOrWhiteSpace(LF_EmplName))
                            {
                                LF_EmplName = Regex.Replace(LF_EmplName, @"\s*\(.*\)", ""); // Removes (2w), (4w), etc.
                            }
                        }
                    }
                }
                //DynAx.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error($"GetEmplTable_LF_EmplName:{ex}");
                // Optionally rethrow or handle the exception as needed
            }
            return new Tuple<string, string, string>(EmplID, LF_EmplName, ReportTo);
        }
        public static Axapta GlobalAxapta()
        {
            try
            {
                globalAxapta = new Axapta();
                GLOBAL.Company = GLOBAL.switch_Company;

                // Use a lock to ensure thread safety
                lock (globalAxapta)
                {
                    // Log in only if not already logged in
                    globalAxapta.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    //var session = globalAxapta.Session();
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error 414:  {ex.Message}");
                globalAxapta = null; // Reset the globalAxapta instance on exception
            }

            return globalAxapta;
        }

        public static Axapta GlobalAxapta_RequireUserID(string UserID)
        {    //Purpose for create another function is to prevent HTTP Global.VAR passing data - 30/5/2025
            try
            {
                globalAxapta = new Axapta();
                GLOBAL.Company = GLOBAL.switch_Company;

                // Use a lock to ensure thread safety
                lock (globalAxapta)
                {

                    // Log in only if not already logged in
                    globalAxapta.LogonAs(UserID, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    //var session = globalAxapta.Session();
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error 414:  {ex.Message}");
                globalAxapta = null; // Reset the globalAxapta instance on exception
            }

            return globalAxapta;
        }
        public static void ImgCompress(HttpPostedFile file, string filePath)
        {
            // Perform image compression
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                memoryStream.Position = 0; // Reset the position to the beginning of the stream
                try
                {
                    #region CompressFunction
                    // Perform image compression
                    using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(memoryStream))
                    {
                        // Calculate the desired target file size in bytes
                        long targetFileSize = 1 * 1024 * 1024; // 1MB (in bytes)
                        long currentFileSize = file.ContentLength;

                        // Determine the target image quality based on the desired file size
                        if (currentFileSize > targetFileSize)
                        {
                            double scaleFactor = Math.Sqrt((double)targetFileSize / (double)currentFileSize);
                            int targetWidth = (int)(originalImage.Width * scaleFactor);
                            int targetHeight = (int)(originalImage.Height * scaleFactor);

                            using (System.Drawing.Image compressedImage = new Bitmap(targetWidth, targetHeight))
                            {
                                using (Graphics graphics = Graphics.FromImage(compressedImage))
                                {
                                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);

                                    // Save the compressed image to the file system as JPEG format
                                    compressedImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                        }
                        else
                        {
                            // Save the original image as it is since its size is already below the target size
                            originalImage.Save(filePath);
                        }
                    }
                    #endregion
                }
                catch (OutOfMemoryException ex)
                {
                    // Log additional details about the image
                    throw new InvalidOperationException($"Out of memory while processing the image. File size: {file.ContentLength} bytes, File name: {file.FileName}", ex);
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw new InvalidOperationException("An error occurred while processing the image.", ex);
                }
            }
        }

        public static void UserLog(string text)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\Administrator\\Desktop\\publishDotNet\\Activity.log", true))
            {
                writer.WriteLine(DateTime.Now + " " + text);
            }
        }

        public static string GenerateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // Generate the QR code image
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Save the image to memory stream
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    // Convert the image to a base64 string
                    string base64String = Convert.ToBase64String(ms.ToArray());

                    // Return the base64 image string as a data URL
                    return "data:image/png;base64," + base64String;
                }
            }
        }

        public static string Confirm(string message, Page page, object sender)
        {
            // This is a placeholder for your confirmation dialog logic.
            // You can use JavaScript or any other method to show a confirmation dialog.

            // Example using JavaScript (this will need to be adapted to your environment):
            string script = $"if(confirm('{message}')) {{ return 'Yes'; }} else {{ return 'No'; }}";
            // Execute the script and capture the result (this is pseudo-code)
            // You will need to implement the actual dialog logic based on your UI framework.

            // For now, let's assume the user always selects "No" for demonstration purposes.
            return "No"; // Replace this with actual user response handling.
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}

public static class AxaptaExtensions
{
    public static int GetFieldId(this Axapta ax, int tableId, string fieldName)
    {
        using (var dict = ax.CreateAxaptaObject("DictTable", tableId))
        {
            return (int)dict.Call("fieldName2Id", fieldName);
        }
    }

    public static int GetTableId(this Axapta ax, string table)
    {
        return (int)ax.CallStaticClassMethod("Global", "tableName2Id", table);
    }
}

public static class ADExtensionMethods
{
    public static string GetPropertyValue(this SearchResult sr, string propertyName)
    {
        string ret = string.Empty;

        if (sr.Properties[propertyName].Count > 0)
            ret = sr.Properties[propertyName][0].ToString();
        return ret;
    }
}

/*
ExecuteReader to query the database.Results are usually returned in a MySqlDataReader object, created by ExecuteReader.

ExecuteNonQuery to insert, update, and delete data.

ExecuteScalar to return a single value.
*/
