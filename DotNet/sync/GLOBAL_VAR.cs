using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Dynamics.BusinessConnectorNet;

namespace GLOBAL_VAR
{
    public static class GLOBAL_Sync
    {        
        public static string mysql_conn_string = "server=localhost;user=hirevadm_sync;database=sync_db;port=3307;password=sync@Hirevadmin#1";        

        //public static string ax_user_id = "yongwc";
        public static string ax_domain_name = "LIONPB";
        public static string ax_proxy_user_name = "axbcproxy";
        public static string ax_proxy_password = "aos20@9";
        public static string ax_company = "PPM";
        public static string ax_language = "EN-US";
        //public static string ax_object_server = "AOS2009:2725";

        public static string ax_user_id = "itsyn";
        public static string ax_object_server = "AOS2009:2713";

        public static string sync_logs_location = "C:\\Users\\Administrator\\Desktop\\publishDotNet\\sync_logs\\";
    }
}

