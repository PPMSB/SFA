using System;
using System.Web;
using static DotNet.Visitor_MainMenu;

namespace GLOBAL_VAR
{
    public static class GLOBAL
    {
        //for testing
        public static bool debug = true;// true=testing, false=released
        public static string software_version = "v127";
        //module======================================================
        public static int no_of_module = 11;
        public static String[] module_name = {
            "Customer",
            "Sales",
            "Payment",
            "Redemption",
            "Inventory",
            "EOR",
            "Check In",
            "WClaim",
            "Quotation",
            "Event Budget",
            "New Product Request"
        };

        //mySQL======================================================
        public static string mySqlPW = "posim@dotnet";
        public static string connStr = "server=localhost;user=root;database=dotnet;port=3307;password=" + mySqlPW;
        public static string ProductionWebsite = "https://sfa.posim.com.my/";
        //external mySQL [not using]======================================================
        public static string EXTmySqlPW = "posimadmin@2018";
        public static string EXTmySqlServer = "103.233.1.123";
        public static string EXTconnStr = "server=" + EXTmySqlServer + ";user=hirevadmin;database=hirevadm_db_dev;password=" + mySqlPW;
        public static string gDisplayDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static string gDisplayDateTimeWithoutSecondsFormat = "yyyy-MM-dd hh:mm tt";
        public static string gDisplayDateFormat = "dd/MM/yyyy";
        public static string gDisplayDateAxaptaFormat = "d/M/yyyy";
        //proxy======================================================
        public static string ProxyUserName = "axbcproxy";
        public static string ProxyPassword = "aos20@9";

        //login info, Table:user [use session to pass data]=========
        public static string logined_user_name;
        //public static string user_id;

        public static string user_id
        {
            get
            {
                return (string)HttpContext.Current.Session["user_id"];

            }
            set
            {
                HttpContext.Current.Session["user_id"] = value;
            }
        }

        public static UserRoleType UserRole
        {
            get
            {
                return (UserRoleType)HttpContext.Current.Session["UserRole"];
            }
            set
            {
                HttpContext.Current.Session["UserRole"] = value;
            }
        }
        public static int user_authority_lvl;
        public static int page_access_authority;
        public static string user_company;
        public static string switch_Company;//for user that able to switch company
        public static bool CampaignReport
        {
            get
            {
                return (bool)HttpContext.Current.Session["CampaignReport"];
            }
            set
            {
                HttpContext.Current.Session["CampaignReport"] = value;
            }
        }
        public static int module_access_authority;

        //Get Current Directory
        public static string BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        public static string LogFilePath = "C:\\Users\\Administrator\\Desktop\\publishDotNet\\LogFile\\";

        public static string DomainPath;

        //system checking
        public static int system_checking;
        /*  0x01: system_time_format  | 1:correct time format |0:Not correct time format
            0x02:                                                                             
        */

        public static string data_passing;
        /*
          2 bits: module name
          1 bits: splitter
          xx bits: data
         */

        //axapta biz connector========================================
        public static string AdminID = "foozm";//equivalent to user_id //admindotnet
        public static string AdminID2 = "foozm";//admin id2

        public static string DomainName = "lionpb";
        public static string Language = "EN-US";
        public static string AxaptaPort = "2725";///new: 2723 old:2726 LIVE:2713 //newer restore testing sazila: 2725
        public static string ObjectServer = "AOS2009:" + AxaptaPort;
        public static string Company = null;
        public static string axPWD;

        //external IP Address
        public static string externalServerIP = "https://sfa.posim.com.my/";
            ////"https://202.188.142.200";

        //version control
        public static string version_control = AxaptaPort + "_" + software_version;

        // ConversionData as switcher for access control  
        public static int[] ConversionData = new int[31]
        {0x01,0x02,0x04,0x08,
         0x10,0x20,0x40,0x80,
         0x100,0x200,0x400,0x800,
         0x1000,0x2000,0x4000,0x8000,
         0x10000,0x20000,0x40000,0x80000,
         0x100000,0x200000,0x400000,0x800000,
         0x1000000,0x2000000,0x4000000,0x8000000,
         0x10000000,0x20000000,0x40000000
         };


        // Color Code=======================================================
        public static string Button_Selected_color = "background-color:#fbb58f";

        /*not using  for encryption                                                                
        //encryption============================================================
        public static string key_user = "HIREVHEBAT2020";
        */
    }
}

/*
 *module as of 18/02/2021
    0x01: Customer
    0x02: Sales Automation
    0x04: PaymentTag
    0x06: Redemption  
    0x08: InventoryMaster
    0x10: EnS  
    0x12: CheckIn              

 *Page Access Control (not fixed, as it follow axapta), sort by RecID Asc
 0x01: DAT
 0x02: PPP
 0x04: OTO
 0x08: PCG

 0x10: PPM
 0x20: LFI
 0x40: EMS
 0x80: PBM

0x100: ETY
0x200: TTY
0x400:
0x800:
*/