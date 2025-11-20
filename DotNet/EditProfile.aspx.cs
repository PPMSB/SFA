using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class EditProfile : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                    null, CustomerMasterTag2,
                    null, SFATag2,
                    null, SalesQuotation2,
                    null, PaymentTag2,
                    null, RedemptionTag2,
                    null, InventoryMasterTag2,

                    null, EORTag2,
                    null, null,
                    null, WClaimTag2,
                    null, EventBudgetTag2,
                    null, null,
                    null, null, null, RocTinTag2,
                    NewProduct2
                    );
            }
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        private void check_session()
        {
            try
            {
                //load session user
                GLOBAL.user_id = Session["user_id"].ToString();
                GLOBAL.axPWD = Session["axPWD"].ToString();
                GLOBAL.logined_user_name = Session["logined_user_name"].ToString();
                GLOBAL.user_authority_lvl = Convert.ToInt32(Session["user_authority_lvl"]);
                GLOBAL.page_access_authority = Convert.ToInt32(Session["page_access_authority"]);
                GLOBAL.user_company = Session["user_company"].ToString();
                GLOBAL.module_access_authority = Convert.ToInt32(Session["module_access_authority"]);
                GLOBAL.switch_Company = Session["switch_Company"].ToString();
                GLOBAL.system_checking = Convert.ToInt32(Session["system_checking"]);
                GLOBAL.data_passing = Session["data_passing"].ToString();
                //
            }
            catch
            {
                Session.Clear();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void clear_variable()
        {
            Label_UserName.Text = "";
            initialize();
        }

        protected void initialize()
        {
            TextBox_UserId.Text = GLOBAL.user_id;
            Label_UserName.Text = GLOBAL.logined_user_name;
            Button_Save.Visible = false;

            Label3.Text = GLOBAL.switch_Company;

            show_company_access_list();

            hidden_module_access_authority.Text = GLOBAL.module_access_authority.ToString();//load current user module access //Convert.ToInt32(Session["module_access_authority"]);
            hidden_user_authority_lvl.Text = GLOBAL.user_authority_lvl.ToString();
            //check user authority
            if (GLOBAL.user_authority_lvl == 1)//Superadmin
            {//only Superadmin allow to use other UserId

                TextBox_UserId.ReadOnly = false; /*TextBox_UserId.Attributes.Add("style", "background-color:#ffaec8");*/
            }
            else //Admin and Basic not allowed
            {
                TextBox_UserId.ReadOnly = true; TextBox_UserId.Attributes.Add("style", "background-color:#f58345");
                //Check got second authentification, this when Superadmin switch user=================================
                int user_authority_lvl_Red = Convert.ToInt32(Session["user_authority_lvl_Red"]);
                if ((user_authority_lvl_Red == 1))//SuperAdmin
                {
                    TextBox_UserId.ReadOnly = false; /*TextBox_UserId.Attributes.Add("style", "background-color:#ffaec8");*/
                }
                //==================================================================================================
            }
        }

        protected void CheckNewID(object sender, EventArgs e)
        {
            string UserId = TextBox_UserId.Text.Trim();
            if (UserId == "")
            {
                Function_Method.MsgBox("Textbox for User Id is empty", this.Page, this);
            }
            show_company_access_list();
            ShowHide_Button_Save();
        }

        protected void DropDownList_SwitchCompany_Change(object sender, EventArgs e)
        {
            ShowHide_Button_Save();
        }

        protected void ShowHide_Button_Save()
        {
            Button_Save.Visible = false;
            string UserId = TextBox_UserId.Text.Trim();
            if (UserId == "" || DropDownList_SwitchCompany.SelectedValue == "")
            {
                Button_Save.Visible = false;
            }
            else
            {
                if (GLOBAL.user_id != UserId || DropDownList_SwitchCompany.SelectedValue != Label3.Text)
                {
                    Button_Save.Visible = true;
                }
            }
        }

        protected void show_company_access_list()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string UserId = TextBox_UserId.Text.Trim();
            try
            {
                string Query = "select page_access_authority,user_authority_lvl,module_access_authority,logined_user_name,user_company from user_tbl where user_id=@D1 order by user_key DESC limit 1";
                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = UserId;
                cmd.Parameters.Add(_D1);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            hidden_module_access_authority.Text = reader.GetInt32("module_access_authority").ToString();
                            hidden_user_authority_lvl.Text = reader.GetInt32("user_authority_lvl").ToString();

                            Label_UserName.Text = reader.GetString("logined_user_name");
                            DropDownList_SwitchCompany.Items.Clear();
                            //DropDownList_SwitchCompany.ClearSelection();
                            int temp_page_access_authority = reader.GetInt32("page_access_authority");

                            var tuple_get_UpToDateCompany = get_UpToDateCompany();
                            List<ListItem> List_UpToDateCompany = tuple_get_UpToDateCompany.Item1;
                            List<ListItem> List_SwitchCompany = new List<ListItem>();
                            int count_company = tuple_get_UpToDateCompany.Item2;

                            if (count_company != 0)
                            {
                                for (int k = 0; k < count_company; k++)
                                {
                                    if ((temp_page_access_authority & GLOBAL.ConversionData[k]) != 0)
                                    {
                                        string CompanyNameId = List_UpToDateCompany[k].Text;
                                        string CompanyId = List_UpToDateCompany[k].Value;
                                        List_SwitchCompany.Add(new ListItem(CompanyNameId, CompanyId));
                                    }
                                }
                                DropDownList_SwitchCompany.Items.AddRange(List_SwitchCompany.ToArray());
                                DropDownList_SwitchCompany.SelectedValue = Label3.Text;
                            }
                        }
                    }
                    else
                    {
                        Function_Method.MsgBox("There is no data found in MySQL database for this User Id: " + UserId + ". Please add in MySql first.", this.Page, this);
                        TextBox_UserId.Text = "";
                    }
                }
            }
            catch (Exception ER_EP_01)
            {
                Function_Method.MsgBox("ER_EP_01: " + ER_EP_01.ToString(), this.Page, this);
            }
        }

        private Tuple<List<ListItem>, int> get_UpToDateCompany()
        {
            List<ListItem> List_UpToDateCompany = new List<ListItem>();
            int count = 0;
            string TableName = "DataArea";
            AxaptaRecord DynRec2;
            Axapta DynAx = new Axapta();

            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = null;
                DynAx.LogonAs(TextBox_UserId.Text.Trim(), GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Define the record
                DynRec2 = DynAx.CreateAxaptaRecord(TableName);
                DynRec2.ExecuteStmt("select * from %1 ORDER BY RECID ASC");//must asc

                while (DynRec2.Found)
                {
                    count = count + 1;
                    // Retrieve the record data for the
                    // specified fields.
                    string CompanyId = DynRec2.get_Field("id").ToString();
                    string CompanyName = DynRec2.get_Field("name").ToString();

                    string CompanyDetail = "(" + CompanyId + ")  " + CompanyName;
                    List_UpToDateCompany.Add(new ListItem(CompanyDetail, CompanyId));
                    // Advance to the next row.
                    DynRec2.Next();
                }
                // Dispose of the AxaptaRecord object.
                DynRec2.Dispose();

                // Log off from Microsoft Dynamics AX.
                DynAx.Logoff();

            }
            catch (Exception ER_EP_02)
            {
                Function_Method.MsgBox("ER_EP_02: " + ER_EP_02.ToString(), this.Page, this);
            }
            return new Tuple<List<ListItem>, int>(List_UpToDateCompany, count);
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            string UserId = TextBox_UserId.Text.Trim();
            string message = "";
            if (UserId != "" && DropDownList_SwitchCompany.SelectedItem.Value != "")// user id is not empty
            {
                String str = DropDownList_SwitchCompany.SelectedItem.Value;
                string switched_company = str;

                if (switched_company != GLOBAL.switch_Company)
                {
                    Session["switch_Company"] = switched_company;
                    Label3.Text = switched_company;
                    message = "Switched Company. ";
                }

                //==================================================================================================
                int user_authority_lvl_Red = Convert.ToInt32(Session["user_authority_lvl_Red"]);

                if (GLOBAL.user_id != UserId)
                {
                    if ((GLOBAL.user_authority_lvl == 1) || (user_authority_lvl_Red == 1))//Superadmin
                    {//only Superadmin allow to use other UserId

                        //Update to Session to use others id

                        Session["user_id"] = UserId;
                        Session["logined_user_name"] = Label_UserName.Text;
                        Session["module_access_authority"] = Convert.ToInt32(hidden_module_access_authority.Text);
                        Session["user_authority_lvl"] = Convert.ToInt32(hidden_user_authority_lvl.Text);
                        message = message + " Switched User Id. ";
                    }
                    else //Admin and Basic not allowed to switch
                    {
                        //do nothing
                    }
                }
                //==================================================================================================
                if (message != "")
                {
                    Button_Save.Visible = false;
                    Response.Redirect("MainMenu.aspx", true);
                    Function_Method.MsgBox(message, this.Page, this);
                }
            }
        }
    }
}