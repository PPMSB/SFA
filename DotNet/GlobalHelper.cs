using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using static DotNet.Visitor_MainMenu;
using static DotNet.Visitor_Model.AppointmentModel;
using System.Web.Services;
using System.Reflection;
using static DotNet.CampaignModel.CampaignModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO.Compression;
using Org.BouncyCastle.Asn1.Ocsp;
using DotNet.Visitor_Model;
using NLog;
using System.Text.RegularExpressions;

namespace DotNet
{
    public class GlobalHelper
    {
        ////Example of Inserting record ==================================================
        //string TableName = "TableName";
        //List<string> ColumnList = new List<string> { "columnName", "columnName"... };
        //Dictionary<string, object> ParamDict = new Dictionary<string, object>
        //        {
        //            { "@p0", paramValue },
        //            { "@p1", paramValue },
        //            { "@p2", paramValue },
        //            ...
        //        };
        //GlobalHelper.InsertQuery(TableName, ColumnList, ParamDict);
        //    //=================================================================================
        public static void InsertQuery(string TableName, List<string> ColumnList, Dictionary<string, object> ParamDict)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            List<string> ParamList = new List<string>();
            for (var i = 0; i < ColumnList.Count(); i++)
            {
                ParamList.Add("@p" + i);
            }
            string Query = "insert into " + TableName + "(" + String.Join(",", ColumnList) + ") values (" + String.Join(",", ParamList) + ")";

            MySqlCommand cmd = new MySqlCommand(Query, conn);

            foreach (var param in ParamDict)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            try
            {
                cmd.ExecuteNonQuery();
                //LogToDatabase("GlobalHelper.InsertQuery", $"Inserted into {TableName}: {String.Join(", ", ParamDict.Select(p => $"{p.Key}={p.Value}"))}", null, GLOBAL.user_id);
            }
            catch (Exception ex)
            {
                //LogToDatabase("ERROR", $"Failed to insert into {TableName}: {ex.Message}", ex.ToString(), GLOBAL.user_id);
                throw; // Optionally rethrow the exception
            }
            finally
            {
                conn.Close();
            }
        }


        public static void BulkInsertQuery(string TableName, List<string> ColumnList, Dictionary<string, object> ParamDict, int itemCount)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string CombinedParam = "";
            int count = 0;

            for (var ic = 1; ic <= itemCount; ic++)
            {
                List<string> ParamList = new List<string>();

                for (var i = 0; i < ColumnList.Count(); i++)
                {
                    ParamList.Add("@p" + count);

                    count++;
                }

                if (ic > 1)
                {
                    CombinedParam += ",";
                }
                CombinedParam += "(" + String.Join(",", ParamList) + ")";
            }

            string Query = "insert into " + TableName + "(" + String.Join(",", ColumnList) + ") values " + CombinedParam;

            MySqlCommand cmd = new MySqlCommand(Query, conn);

            foreach (var param in ParamDict)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        ////Example of Updating record==============================================
        //string Variable = "abctesting";
        //string TableName = "TableName";
        //string Condition = "where condition = @c1;
        //List<string> ColumnList = new List<string> { "ColumnName", "ColumnName" };
        //Dictionary<string, object> ParamDict = new Dictionary<string, object>
        //{
        //    { "@p0", "paramValue" },
        //    { "@p1", "paramValue" },
        //      ...
        //};
        //GlobalHelper.UpdateQuery(TableName, ColumnList, ParamDict, Condition);
        ////=========================================================================
        public static MySqlCommand UpdateQuery(string TableName, List<string> ColumnList, Dictionary<string, object> ParamDict, string Condition)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            List<string> ParamList = new List<string>();

            string Query = "update " + TableName + " set ";

            for (var i = 0; i < ColumnList.Count(); i++)
            {
                ParamList.Add(ColumnList[i] + " = @p" + i);
            }

            Query += string.Join(",", ParamList) + " ";

            Query += Condition;

            MySqlCommand cmd = new MySqlCommand(Query, conn);

            foreach (var param in ParamDict)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
            return cmd;
            //cmd.ExecuteNonQuery();
            //conn.Close();
        }

        public static void PumpParamQuery(MySqlCommand cmd, Dictionary<string, object> ParamDict)
        {
            foreach (var param in ParamDict)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
        }

        public static List<string> GetColumnsByModel<T>(T m)
        {
            List<string> ColumnList = new List<string>();
            Type mType = m.GetType();

            foreach (var item in mType.GetProperties())
            {
                var propertyInfo = mType.GetProperty(item.Name);

                ColumnList.Add(item.Name);
            }

            return ColumnList;
        }

        public static Dictionary<string, object> ConvertModelColumnsIntoDictionary(List<string> ColumnList, List<object> ObjectList)
        {
            Dictionary<string, object> ParamDict = new Dictionary<string, object>();

            for (var i = 0; i < ColumnList.Count(); i++)
            {
                ParamDict.Add("@p" + i, ObjectList[i]);
            }

            return ParamDict;
        }


        //Export Excel
        public static void ExportFromGridView(GridView gv, string FileName)
        {
            var context = HttpContext.Current;

            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.Charset = "";

            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gv.GridLines = GridLines.Both;
            gv.HeaderStyle.Font.Bold = true;
            gv.RenderControl(htmltextwrtter);
            context.Response.Write(strwritter.ToString());
            context.Response.Flush();
            context.Response.End();
        }

        public static Dictionary<string, object> ConvertModelValuesIntoDictionary<T>(T model)
        {
            Dictionary<string, object> ParamDict = new Dictionary<string, object>();
            Type type = model.GetType();

            int i = 0;
            foreach (PropertyInfo item in type.GetProperties())
            {
                ParamDict.Add("@p" + i, item.GetValue(model));

                i++;
            }

            return ParamDict;
        }

        public static Dictionary<string, object> ConvertModelListValuesIntoDictionary<T>(List<T> model)
        {
            Dictionary<string, object> ParamDict = new Dictionary<string, object>();

            int i = 0;

            foreach (var item in model)
            {
                Type type = item.GetType();

                foreach (PropertyInfo value in type.GetProperties())
                {
                    ParamDict.Add("@p" + i, value.GetValue(item));

                    i++;
                }
            }

            return ParamDict;
        }

        public static int SaveFile(string UserName, int fileno, HttpPostedFile file)
        {
            return SaveFile(UserName, fileno, file, "", "", "");
        }

        public static int SaveFile(string UserName, int fileno, HttpPostedFile file, string CampaignID, string CustAcc, string FileName)
        {
            if (file != null)
            {
                string FileLocation = SaveIntoLocalLibrary(CampaignID, CustAcc, FileName);

                if (string.IsNullOrEmpty(FileLocation))
                {
                    return 0;
                }
                int FileNo = SaveFileToLibrary(UserName, file.InputStream, file.FileName, file.ContentType, fileno, FileLocation);
                return FileNo;
            }
            return 0;
        }
        public static int SaveFileToLibrary(string UserName, System.IO.Stream FileStream, string FileName, string FileType, int FileNo, string FileLocation)
        {
            byte[] FileBytes;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                FileStream.Position = 0;
                FileStream.CopyTo(ms);
                FileBytes = ms.ToArray();
            }
            return SaveFileToLibrary(UserName, FileBytes, FileName, FileType, FileNo, FileLocation);
        }

        public static int SaveFileToLibrary(string UserName, byte[] FileBytes, string FileName, string FileType, int FileNo, string FileLocation)
        {
            var context = HttpContext.Current;

            int FileSize = FileBytes.Length;
            FileLibraryModel m = new FileLibraryModel();
            m.FileName = FileName;
            m.FileSize = FileSize;
            m.FileType = FileType;
            m.UploadBy = UserName;
            m.UploadDate = DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat);
            m.FileContent = new byte[0];//Compress(FileBytes);
            m.FileLocation = FileLocation;

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();
            string TableName = "file_library";

            List<string> ColumnList = GetColumnsByModel(m);
            if (FileNo == 0)
            {
                FileNo = 1;
                Dictionary<string, object> ParamDict = ConvertModelValuesIntoDictionary(m);

                InsertQuery(TableName, ColumnList, ParamDict);

                string SQL = "select max(FileID) LatestID from file_library";
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        FileNo = Int32.Parse(reader.GetValue(0).ToString());
                    }
                }
                //return m.FileID;
            }
            else
            {
                m.FileID = FileNo;
                Dictionary<string, object> ParamDict = ConvertModelValuesIntoDictionary(m);

                string Condition = " where FileID = @c1";
                MySqlCommand cmd = UpdateQuery(TableName, ColumnList, ParamDict, Condition);
                cmd.Parameters.AddWithValue("@c1", FileNo);

                cmd.ExecuteNonQuery();
            }
            conn.Close();
            return FileNo;
        }

        private static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return compressedStream.ToArray();
            }
        }

        private static string SaveIntoLocalLibrary(string Value1, string Value2, string FileName)
        {
            string FileLocation = "";
            var context = HttpContext.Current;

            HttpFileCollection files = context.Request.Files;
            string filename = "";

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    if (file.ContentLength > 0)
                    {
                        //string path = @"e:/Warranty/" + Label_Salesman.Text + "/" + Label_CustName.Text;
                        string path = @"e:/" + FileName + "/" + Value1 + " - " + Value2 + " - " + FileName;
                        //string path = GLOBAL.BaseDirectory + "VPPPCampaign/" + Label_Salesman.Text + "/" + Label_CustAcc.Text;
                        if (!Directory.Exists(@"e:/" + FileName + "/" + Value2))
                        {
                            //Directory.CreateDirectory(@"e:/Warranty/" + Label_Salesman.Text);
                            Directory.CreateDirectory(@"e:/" + FileName + "/" + Value2);
                            Directory.CreateDirectory(path);
                            logger.Info("Created directory: {0}", path);
                        }
                        else
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                                logger.Info("Created directory: {0}", path);
                            }
                        }
                        filename = file.FileName;

                        // Jerry 2024-12-23 Sanitize file name
                        filename = Function_Method.SanitizeFilename(filename);
                        // Jerry 2024-12-23 Sanitize file name - END

                        string filePath = (path + "/" + filename);

                        // Log the file path
                        logger.Info("Saving file to: {0}", filePath);

                        
                        string ext = Path.GetExtension(filename).ToLower();
                        if (ext == ".pdf")
                        {
                            file.SaveAs(filePath);
                            logger.Info("PDF file saved: {0}", filePath);
                        }
                        else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".gif" || ext == ".tiff")
                        {
                            Function_Method.ImgCompress(file, filePath);
                            logger.Info("Image file compressed and saved: {0}", filePath);
                        }
                        else
                        {
                            logger.Warn("Unsupported file type: {0}", ext);
                            throw new InvalidOperationException("Unsupported file type.");
                        }

                        FileLocation = filePath;
                    }
                    else
                    {
                        Function_Method.UserLog(filename);
                        logger.Warn("Empty file received: {0}", filename);
                    }
                }

                return FileLocation;
            }
            catch (Exception ER_WC_22)
            {
                logger.Error(ER_WC_22, "Upload error");
                Function_Method.UserLog("Upload error: " + ER_WC_22.ToString());

                return FileLocation;
            }
        }


        //Import Excel
        public static List<T> ImportExcel<T>(Stream stream, string fileExtension) where T : new()
        {
            IWorkbook workbook;
            List<T> mList = new List<T>();

            if (fileExtension == ".xls")
            {
                workbook = new HSSFWorkbook(stream);
            }
            else
            {
                workbook = new XSSFWorkbook(stream);
            }


            ISheet sheet = workbook.GetSheetAt(0);

            IRow headerRow = sheet.GetRow(0);
            var columnMappings = new Dictionary<string, int>();
            for (int col = 0; col < headerRow.LastCellNum; col++)
            {
                var cell = headerRow.GetCell(col);
                if (cell != null)
                {
                    string columnName = cell.ToString().Trim();
                    columnMappings[columnName] = col;
                }
            }

            int rowCount = sheet.LastRowNum;

            for (int row = 1; row <= rowCount; row++)
            {
                var currentRow = sheet.GetRow(row);
                if (currentRow == null) continue;

                T dataItem = new T();

                foreach (var mapping in columnMappings)
                {
                    var propertyInfo = typeof(T).GetProperty(mapping.Key, BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        var cellValue = GetCellValue(currentRow.GetCell(mapping.Value));
                        if (cellValue != null)
                        {
                            var convertedValue = Convert.ChangeType(cellValue, propertyInfo.PropertyType);
                            propertyInfo.SetValue(dataItem, convertedValue);
                        }
                    }
                }

                mList.Add(dataItem);
            }

            return mList;
        }


        private static object GetCellValue(ICell cell)
        {
            if (cell == null) return null;

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                default:
                    return null;
            }
        }


        public static void RedirectToNewTab(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                System.Web.UI.Page page = (System.Web.UI.Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(System.Web.UI.Page),
                    "Redirect",
                    script,
                    true);
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region "Get_Data_From_Axapta"
        public static string GET_CustomerName_From_CustID(string AccountNum_Value, string UserID)
        {
            //GLOBAL.user_id = GLOBAL.AdminID;
            string customername = "";
            Axapta DynAx = Function_Method.GlobalAxapta_RequireUserID(UserID);
            int CustTable = DynAx.GetTableIdWithLock("CustTable");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            int AccountNum = DynAx.GetFieldId(CustTable, "AccountNum");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", AccountNum);
            qbr.Call("value", AccountNum_Value);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                customername = DynRec.get_Field("Name").ToString();
                DynRec.Dispose();
            }
            axQuery.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun.Dispose();

            return customername;
        }

        public static string GET_CustID_From_CustName(Axapta DynAx, string CustName)
        {
            string result = "";
            List<string> accountNums = new List<string>();
            //Axapta DynAx = Function_Method.GlobalAxapta_RequireUserID(UserID);
            int CustTable = DynAx.GetTableIdWithLock("CustTable");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            string replace_CustName = Regex.Replace(CustName, @"\s", "*"); // Handle single quotes in the name


            int CustNameField = DynAx.GetFieldId(CustTable, "Name");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", CustNameField);
            qbr.Call("value", "*" + replace_CustName + "*");

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                accountNums.Add(DynRec.get_Field("AccountNum").ToString());
                DynRec.Dispose();
            }

            result = string.Join(",", accountNums);

            axQuery.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun.Dispose();

            return result;
        }

        public static UserMainMenuModel UserMainMenuModelData(string UserName)
        {
            UserMainMenuModel m = new UserMainMenuModel();
            var context = HttpContext.Current;
            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                string Query = "SELECT * FROM user_tbl WHERE user_id=@D1 LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(Query, conn))
                {
                    cmd.Parameters.Add(new MySqlParameter("@D1", MySqlDbType.VarChar) { Value = UserName });
                    try
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && reader.GetValue(0) != DBNull.Value)
                            {
                                if (reader.GetValue(2).ToString().Trim() == UserName) // First level security at MySQL
                                {
                                    m.axPWD = reader.GetValue(8).ToString();
                                    m.logined_user_name = reader.GetValue(1).ToString();
                                    m.user_id = reader.GetValue(2).ToString();
                                    m.user_authority_lvl = reader.GetInt32(3);
                                    m.page_access_authority = reader.GetInt32(4);
                                    m.user_company = reader.GetValue(5).ToString();
                                    m.module_access_authority = reader.GetInt32(6);
                                    m.switch_Company = m.user_company;
                                    m.flag_temp = 0;
                                    m.system_checking = 0;
                                    m.data_passing = "";
                                    m.user_authority_lvl_Red = reader.GetInt32(3);
                                    m.logined_user_name_Red = reader.GetValue(1).ToString();
                                    return m;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (consider using a logging framework)
                        // Function_Method.MsgBox(LoginError.Message, this.Page, this);
                    }
                }
            }
            return new UserMainMenuModel(); // Return a new instance if no data is found
        }
        #endregion

        public static void LogToDatabase(string logLevel, string message, string exception, string userId)
        {
            using (MySqlConnection logConn = new MySqlConnection(GLOBAL.connStr))
            {
                logConn.Open();
                string logQuery = "INSERT INTO ApplicationLogs (LogDate, LogLevel, Message, Exception, UserId) VALUES (@LogDate, @LogLevel, @Message, @Exception, @UserID)";
                using (MySqlCommand logCmd = new MySqlCommand(logQuery, logConn))
                {
                    logCmd.Parameters.AddWithValue("@LogDate", DateTime.Now); // Storing the current date and time
                    logCmd.Parameters.AddWithValue("@LogLevel", logLevel);
                    logCmd.Parameters.AddWithValue("@Message", message);
                    logCmd.Parameters.AddWithValue("@Exception", exception);
                    logCmd.Parameters.AddWithValue("@UserID", userId);
                    logCmd.ExecuteNonQuery();
                }
            }
        }
    }
}