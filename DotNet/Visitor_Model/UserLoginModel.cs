using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DotNet.Visitor_MainMenu;

namespace DotNet.Visitor_Model
{
    public class UserLoginModel
    {
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public UserRoleType UserRole { get; set; }
        public string UserCompany { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UserPassword { get; set; }
        public bool IsUpdatedPassword { get; set; }
    }

    public class UserMainMenuModel
    {
        public string axPWD { get; set; }
        public string logined_user_name { get; set; }
        public string user_id { get; set; }
        public int user_authority_lvl { get; set; }
        public int page_access_authority { get; set; }
        public string user_company { get; set; }
        public int module_access_authority { get; set; }
        public string switch_Company { get; set; }
        public int flag_temp { get; set; }
        public int system_checking { get; set; }
        public string data_passing { get; set; }
        public int user_authority_lvl_Red { get; set; }
        public string logined_user_name_Red { get; set; }
    }
}