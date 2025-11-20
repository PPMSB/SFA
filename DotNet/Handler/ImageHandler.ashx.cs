using GLOBAL_VAR;
using iText.IO.Image;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace DotNet
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (var connection = new MySqlConnection(GLOBAL.connStr))
            {
                string query = "SELECT FileContent, FileType, FileSize, FileLocation FROM file_library WHERE FileID = @p0";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@p0", context.Request.QueryString["id"]);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] imageData = (byte[])reader["FileContent"];
                            byte[] decompressedData = DecompressImage(imageData);
                            int FileSize = Int32.Parse(reader["FileSize"].ToString());
                            string folderPath = reader["FileLocation"].ToString();

                            if (!string.IsNullOrEmpty(folderPath))
                            {
                                try
                                {
                                    byte[] imageBytes = File.ReadAllBytes(folderPath);

                                    string extension = Path.GetExtension(folderPath).ToLower();

                                    context.Response.ContentType = reader["FileType"].ToString();
                                    context.Response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
                                    context.Response.Flush();
                                    context.Response.End();
                                }
                                catch (Exception ex)
                                {
                                    context.Response.StatusCode = 500;
                                    context.Response.StatusDescription = "Internal Server Error";
                                }
                            }
                            else
                            {
                                context.Response.StatusCode = 404; // Not Found
                                context.Response.StatusDescription = "Image not found";
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 404; // Not Found
                        }
                    }
                }
            }
        }

        private byte[] DecompressImage(byte[] compressedData)
        {
            try
            {
                using (var inputStream = new MemoryStream(compressedData))
                using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                using (var outputStream = new MemoryStream())
                {
                    gzipStream.CopyTo(outputStream);
                    return outputStream.ToArray();
                }
            }
            catch
            {
                return compressedData;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }

}