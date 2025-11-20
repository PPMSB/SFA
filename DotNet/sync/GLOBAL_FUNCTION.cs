using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using GLOBAL_VAR;
using EncryptStringSample;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

using System.DirectoryServices;
using System.IO;
using DotNetSync;
using System.Net.Sockets;

namespace GLOBAL_FUNCTION
{
    public class Function_Method_Sync
    {
        public static bool isPBM;

        public static Axapta global_dynax = null;
        public static Axapta sub_query_dynax = null;

        //public static MySqlConnection conn = new MySqlConnection(GLOBAL_Sync.connStr);
        public static MySqlConnection mysql_conn = null;

        private static object lockObject = new object();
        private static object subQueryLockObject = new object();

        public static Axapta getDynAxapta(string user_id, string domain_name, string proxy_user_name, string proxy_password,
                                            string company, string language, string object_server)
        {
            try
            {
                /*
                if (global_dynax == null)
                {
                    global_dynax = new Axapta();

                   
                    global_dynax.LogonAs(user_id, domain_name,
                        new System.Net.NetworkCredential(proxy_user_name, proxy_password, domain_name),
                        company, language, object_server, null);
                    
                }
                */
                // Check if the Axapta instance is null or the connection is dead
                if (global_dynax == null || !IsConnectionAlive(global_dynax))
                {
                    lock (lockObject)
                    {
                        // Double-check locking to ensure thread safety
                        if (global_dynax == null || !IsConnectionAlive(global_dynax))
                        {
                            // Create a new Axapta instance
                            global_dynax = new Axapta();

                            // Logon to the Axapta instance
                            global_dynax.LogonAs(user_id, domain_name,
                                new System.Net.NetworkCredential(proxy_user_name, proxy_password, domain_name),
                                company, language, object_server, null);
                        }
                    }
                }
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
            {
                throw ex;
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return global_dynax;
        }



        public static Axapta getSubQueryDynAxapta(string user_id, string domain_name, string proxy_user_name, string proxy_password,
                                            string company, string language, string object_server)
        {
            try
            {
                // Check if the Axapta instance is null or the connection is dead
                if (sub_query_dynax == null || !IsConnectionAlive(sub_query_dynax))
                {
                    lock (subQueryLockObject)
                    {
                        // Double-check locking to ensure thread safety
                        if (sub_query_dynax == null || !IsConnectionAlive(sub_query_dynax))
                        {
                            // Create a new Axapta instance
                            sub_query_dynax = new Axapta();

                            // Logon to the Axapta instance
                            sub_query_dynax.LogonAs(user_id, domain_name,
                                new System.Net.NetworkCredential(proxy_user_name, proxy_password, domain_name),
                                company, language, object_server, null);
                        }
                    }
                }
                /*
                if (sub_query_dynax == null)
                {
                    sub_query_dynax = new Axapta();
                    sub_query_dynax.LogonAs(user_id, domain_name,
                        new System.Net.NetworkCredential(proxy_user_name, proxy_password, domain_name),
                        company, language, object_server, null);
                }*/
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ex)
            {
                throw ex;
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sub_query_dynax;
        }

        // Helper method to check if the connection is alive
        private static bool IsConnectionAlive(Axapta axapta)
        {
            try
            {
                // Perform a simple operation to check if the connection is alive
                axapta.CallStaticClassMethod("Global", "sessionAlive");
                return true;
            }
            catch
            {
                // Connection is dead
                return false;
            }
        }

        public static MySqlConnection getMySqlConnection(string conn_string)
        {
            /*
            if(mysql_conn == null)
            {
                mysql_conn = new MySqlConnection(conn_string);
            }
            return mysql_conn;
            */
            return new MySqlConnection(conn_string);
        }

        public static void AddSyncLog(string filename, string text)
        {
            //using (StreamWriter writer = new StreamWriter("C:\\Users\\Administrator\\Desktop\\publishDotNet\\sync_logs\\" + filename, true))
            //using (StreamWriter writer = new StreamWriter("C:\\Users\\yongwc\\Desktop\\sync_logs\\" + filename, true))
            using (StreamWriter writer = new StreamWriter(GLOBAL_Sync.sync_logs_location + filename, true))
            {
                writer.WriteLine("[" + DateTime.Now + "] " + text);
            }
        }

        
    }
}

public static class AxaptaExtensionsSync
{

    private static readonly object lock_object = new object();
    public static int GetFieldIdWithLock(this Axapta ax, int tableId, string fieldName)
    {
        lock (lock_object)
        {
            try
            {
                using (var dict = ax.CreateAxaptaObject("DictTable", tableId))
                {
                    return (int)dict.Call("fieldName2Id", fieldName);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1; // or another appropriate error value
            }
        }
    }

    public static int GetTableIdWithLock(this Axapta ax, string table)
    {
        lock (lock_object)
        {
            try
            {
                return (int)ax.CallStaticClassMethod("Global", "tableName2Id", table);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1; // or another appropriate error value
            }
        }
    }
}



/*
ExecuteReader to query the database.Results are usually returned in a MySqlDataReader object, created by ExecuteReader.

ExecuteNonQuery to insert, update, and delete data.

ExecuteScalar to return a single value.
*/
