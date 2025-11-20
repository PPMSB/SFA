using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Web.UI.HtmlControls;

//<meta http-equiv="refresh" content="900"/>
namespace DotNet
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check_session();
            ///TimeOutRedirect();
            if (!IsPostBack)
            {/*
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                    null, CustomerMasterTag2,
                    null, SFATag2,
                    null, PaymentTag2,
                    null, RedemptionTag2,
                    null, InventoryMasterTag2,

                    null, EORTag2,
                    null, null,
                    null, null,
                    null, null,
                    null, null
                    );*/
            }
            //sync_lf_gatepass_toPHP();
            sync_lf_gatepass_fromAxaptatoMySQL_master();
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
                Response.Redirect("LoginPage.aspx");
            }
        }
        protected void Button_Confirm_click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('LoginPage.aspx','_newtab');", true);//_blank //_self
        }

        private void sync_lf_gatepass_toPHP()
        {
            string getParamater = Request.QueryString["invoiceid"];
            if (getParamater == null || getParamater == "")
            {
                return;
            }
            //string getParamater = "80589130/PKG";
            string invoiceid = getParamater;
            string invoicedate = ""; string accountnum = "";
            string gatepass = ""; string transportercode = "";
            string gatepassdate = ""; string accountname = "";

            //=====================================================================
            try
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                string Query = "select * from lf_gatepass_test where invoiceid=@D1 limit 1";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = invoiceid;
                cmd.Parameters.Add(_D1);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            //invoiceid = DateTime.Now.ToString();//testing
                            invoicedate = reader.GetValue(1).ToString();
                            gatepass = reader.GetValue(2).ToString();
                            gatepassdate = reader.GetValue(3).ToString();
                            accountnum = reader.GetValue(4).ToString();
                            accountname = reader.GetValue(5).ToString();
                            transportercode = reader.GetValue(6).ToString();
                            conn.Close();
                        }
                    }
                    conn.Close();
                }
            }
            catch
            {
                //Function_Method.MsgBox("Error. Please contact IT department.", this.Page, this);
            }
            //=====================================================================
            //string passing_data = "http://www.hi-rev.com.my/0/sync/sync_lf_gatepass.php?invoiceid="
            string passing_data = "http://www.hi-rev.com.my/0/sync/sync_lf_gatepass_test.php?invoiceid="
                    + invoiceid
                    + "&invoicedate=" + invoicedate
                    + "&accountnum=" + accountnum
                    + "&gatepass=" + gatepass
                    + "&transportercode=" + transportercode
                    + "&gatepassdate=" + gatepassdate
                    + "&accountname=" + accountname;
            Response.Redirect(passing_data);
            //=====================================================================
        }

        //
        private void sync_lf_gatepass_fromAxaptatoMySQL_master()
        {
            //string FixUserName = "sazila";
            string FixUserName = "netuserid";
            Axapta DynAx = new Axapta(); int count = 0;
            try
            {
                DynAx.LogonAs(FixUserName, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int LF_GatePass = 40313;
                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_GatePass);
                /*
                var qbr1 = (AxaptaObject)axQueryDataSource2.Call("addRange", 40003);//InvoiceId
                qbr1.Call("value", "A0007025/INV");
                */
                axQueryDataSource2.Call("addSortField", 40003, 1);//InvoiceId, desc

                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                //if ((bool)axQueryRun2.Call("next"))
                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_GatePass);

                    string InvoiceId = DynRec2.get_Field("InvoiceId").ToString();
                    string InvoiceDate = DynRec2.get_Field("InvoiceDate").ToString();
                    string GatePass = DynRec2.get_Field("GatePass").ToString();

                    string GatePassDate = DynRec2.get_Field("GatePassDate").ToString();
                    if (GatePassDate == "01/01/1900 12:00:00 AM")
                    {
                        GatePassDate = "";
                    }
                    string AccountNum = DynRec2.get_Field("AccountNum").ToString();
                    string AccountName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, AccountNum);
                    string TransporterCode = DynRec2.get_Field("TransporterCode").ToString();
                    //string TransporterName = DynRec1.get_Field("TransporterName").ToString();
                    store_toLocalMySQL(InvoiceId, InvoiceDate, GatePass, GatePassDate, AccountNum, AccountName, TransporterCode);
                    count = count + 1;
                    //insert data to mysql
                    DynRec2.Dispose();
                }
            }
            catch (Exception)
            {
                //Function_Method.MsgBox(count.ToString(), this.Page, this);

            }
        }

        private void store_toLocalMySQL(string InvoiceId, string InvoiceDate, string GatePass, string GatePassDate,
            string AccountNum, string AccountName, string TransporterCode)
        {
            try
            {
                int existing_record;
                existing_record = 0;//insert

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                string Query;
                if (existing_record == 1)
                {
                    Query = "update lf_gatepass_out SET invoicedate=@D2,gatepass=@D3,gatepassdate=@D4,accountnum=@D5,accountname=@D6, transportercode=@D7,sync_out=@D8 where invoiceid=@D1";
                }
                else
                {
                    Query = "insert into lf_gatepass_out(invoiceid,invoicedate,gatepass,gatepassdate,accountnum,accountname, transportercode,sync_out) values(@D1,@D2,@D3,@D4,@D5,@D6,@D7,@D8)";

                }
                MySqlCommand cmd = new MySqlCommand(Query, conn);

                //1:invoiceid
                MySqlParameter _D1 = new MySqlParameter("@D1", MySqlDbType.VarChar, 0);
                _D1.Value = InvoiceId;
                cmd.Parameters.Add(_D1);

                //2:invoicedate,   
                MySqlParameter _D2 = new MySqlParameter("@D2", MySqlDbType.VarChar, 0);
                _D2.Value = InvoiceDate;
                cmd.Parameters.Add(_D2);

                //3:gatepass,
                MySqlParameter _D3 = new MySqlParameter("@D3", MySqlDbType.VarChar, 0);
                _D3.Value = GatePass;
                cmd.Parameters.Add(_D3);

                //4:gatepassdate,
                MySqlParameter _D4 = new MySqlParameter("@D4", MySqlDbType.VarChar, 0);
                _D4.Value = GatePassDate;
                cmd.Parameters.Add(_D4);

                //5:accountnum,
                MySqlParameter _D5 = new MySqlParameter("@D5", MySqlDbType.VarChar, 0);
                _D5.Value = AccountNum;
                cmd.Parameters.Add(_D5);

                //6:accountnumame 
                MySqlParameter _D6 = new MySqlParameter("@D6", MySqlDbType.VarChar, 0);
                _D6.Value = AccountName;
                cmd.Parameters.Add(_D6);

                //6:transportercode,
                MySqlParameter _D7 = new MySqlParameter("@D7", MySqlDbType.VarChar, 0);
                _D7.Value = TransporterCode;
                cmd.Parameters.Add(_D7);

                //7:sync_out
                MySqlParameter _D8 = new MySqlParameter("@D8", MySqlDbType.Int32, 0);
                _D8.Value = 1;
                cmd.Parameters.Add(_D8);

                conn.Open();
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception)
            {


            }
        }



        //not used
        private void sync_employee()
        {
            string emp_name = "Training User1";

            string payrol_com = "352 Lion Forest Industries Berhad";

            string department = "12 Sales";

            string group_code = "TAX08";

            string hrisid = "1";

            string Branch = "1";

            string LoginID = "1";
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "MyFunction()", true);
            string temp = "http://www.hi-rev.com.my/0/sync/sync_employee.php?name="
                + emp_name
                + "&payrol_com=" + payrol_com
                + "&department=" + department
                + "&group_code=" + group_code
                + "&hrisid=" + hrisid
                + "&branch=" + Branch
                + "&LoginID=" + LoginID;

            Response.Redirect(temp);

            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Func()", true
        }
    }
}