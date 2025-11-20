using GLOBAL_FUNCTION;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Dynamics.BusinessConnectorNet;
using ActiveDs;
using MySql.Data.MySqlClient;
using GLOBAL_VAR;
using SelectPdf;
using System.IO;
using DotNet.CampaignModel;
using static DotNet.CampaignModel.CampaignModel;
using iText.StyledXmlParser.Jsoup.Helper;
using iText.Commons.Actions.Sequence;
using System.Data;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace DotNet
{
    public partial class Campaign_CreateNewOffer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Campaign_CheckSession.SystemCheckSession(this.Page, this);

            if (!IsPostBack)
            {
                LoadAvailableCampaignDropdown();
                CreateFormWrapper.Visible = false;
                CustomerMasterWrapper.Visible = true;
                f_call_ax(0);

                // f_call_ax(0);

                //Check_DataRequest();
            }
        }

        private void LoadAvailableCampaignDropdown()
        {
            ddlTarget.Items.Clear();
            List<ListItem> Items = new List<ListItem>();
            Axapta DynAx = Function_Method.GlobalAxapta();
            int CampaignTable = DynAx.GetTableIdWithLock("MSB_CampaignMaster");
            string CampaignID = "";
            string CampaignDesc = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CampaignTable);

            int CampaignIDField = DynAx.GetFieldId(CampaignTable, "MSB_CampaignID");

            axQueryDataSource.Call("addSortField", CampaignIDField, 1);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CampaignTable);
                DateTime CurrentDateTime = DateTime.Now;
                DateTime CampaignEndDateTime = DateTime.Parse(DynRec.get_Field("EndDate").ToString());
                CampaignID = DynRec.get_Field("MSB_CampaignID").ToString();

                //if (CurrentDateTime < CampaignEndDateTime)
                //{
                //    Items.Add(new ListItem(CampaignDesc, CampaignID));
                //}
                ddlAvailableCampaign.Text = CampaignID;
                DynRec.Dispose();

                int MSB_CampaignTargetHeader = DynAx.GetTableIdWithLock("MSB_CampaignTargetHeader");
                AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", MSB_CampaignTargetHeader);

                int CampaignIDField1 = DynAx.GetFieldId(MSB_CampaignTargetHeader, "CampaignID");
                var qbr = (AxaptaObject)axQueryDataSource1.Call("addRange", CampaignIDField1);

                qbr.Call("value", CampaignID);
                AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                Items.Add(new ListItem("-- Please select a target amount --", ""));

                while ((bool)axQueryRun1.Call("next"))
                {
                    AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", MSB_CampaignTargetHeader);

                    Items.Add(new ListItem(Double.Parse(DynRec1.get_Field("Target").ToString()).ToString("N"), DynRec1.get_Field("RecId").ToString() + "-" + DynRec1.get_Field("Target").ToString() + "-" + DynRec1.get_Field("SecondLevelTarget").ToString()));

                    DynRec1.Dispose();
                }
                ddlTarget.Items.AddRange(Items.ToArray());

                axQuery.Dispose();
                axQueryDataSource.Dispose();
                axQuery1.Dispose();
                axQueryDataSource1.Dispose();

                DynAx.Dispose();
                //use campaign id to find target in MSB_CampaignTargetHeader and make it as drop down
            }

            //ddlAvailableCampaign.Items.AddRange(Items.ToArray());

        }

        protected void ShowFindCustomer(object sender, EventArgs e)
        {
            CreateFormWrapper.Visible = false;
            CustomerMasterWrapper.Visible = true;

            f_call_ax(0);
        }

        private void CheckIsUserParticipateCampaign()
        {
            string CampaignID = ddlAvailableCampaign.Text;
            string CustAccount = txtCustomerAccount.Text.Trim();
            ddlTarget.ClearSelection();
            DateWrapper.Visible = false;

            Axapta DynAx = Function_Method.GlobalAxapta();

            int CampaignDetails = DynAx.GetTableIdWithLock("MSB_CampaignDetail");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CampaignDetails);

            int CampaignIDField = DynAx.GetFieldId(CampaignDetails, "CampaignID");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", CampaignIDField);

            qbr.Call("value", CampaignID);

            int CustAccountField = DynAx.GetFieldId(CampaignDetails, "CustAccount");
            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", CustAccountField);

            qbr1.Call("value", CustAccount);
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            if (!(bool)axQueryRun.Call("next"))
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();

                string SQL = "select CustomerAccount, CampaignID from campaign_document where CustomerAccount = @p0 and CampaignID = @p1 AND status <> @p2";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                cmd.Parameters.AddWithValue("@p0", CustAccount);
                cmd.Parameters.AddWithValue("@p1", CampaignID);
                cmd.Parameters.AddWithValue("@p2", 5);//status reject is 5

                MySqlDataReader reader = cmd.ExecuteReader();
                if (!(bool)reader.Read())
                {
                    DateWrapper.Visible = true;
                }

                conn.Close();
            }
            else
            { 
                /// Neil 04/09/2025 - after campaign rejected but axapta still got record, here's to allow select target again
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CampaignDetails);
                object documentRecDateObj = DynRec.get_Field("DocumentRecDate");
                //CampaignID = DynRec.get_Field("MSB_CampaignID").ToString();

                if (documentRecDateObj == null || string.IsNullOrWhiteSpace(documentRecDateObj.ToString()))
                {
                    DateWrapper.Visible = true;
                    ddlAvailableCampaign.Text = CampaignID;
                }
                DynRec.Dispose();

            }

            DynAx.Dispose();
        }

        private void Check_DataRequest()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string RequestedData = Session["data_passing"].ToString().Trim();
            Session["data_passing"] = "";
            string CustName = "";
            string SalesmanID = "";

            if (RequestedData != "")
            {
                if (RequestedData.Length >= 6)
                {
                    if (RequestedData.Substring(0, 6) == "ICACM_")
                    {
                        string AccountNum = RequestedData.Substring(6);

                        int CustTable = DynAx.GetTableIdWithLock("CustTable");

                        AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

                        int AccountNumField = DynAx.GetFieldId(CustTable, "AccountNum");
                        var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", AccountNumField);

                        qbr.Call("value", AccountNum);
                        AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                        if ((bool)axQueryRun.Call("next"))
                        {
                            AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                            CustName = DynRec.get_Field("Name").ToString();
                            SalesmanID = DynRec.get_Field("EmplId").ToString();

                            txtCustName.Text = CustName;
                            txtCustomerAccount.Text = DynRec.get_Field("AccountNum").ToString();

                            DynRec.Dispose();
                        }

                        int EmplTable = DynAx.GetTableIdWithLock("EmplTable");

                        AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

                        int EmplIDField = DynAx.GetFieldId(CustTable, "EmplId");
                        var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);
                        qbr2.Call("value", SalesmanID);
                        AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

                        if ((bool)axQueryRun2.Call("next"))
                        {
                            AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                            txtSalesman.Text = DynRec1.get_Field("DEL_Name").ToString();
                            txtSalesmanID.Text = DynRec1.get_Field("EmplId").ToString();
                            //hidden txt store salesman id
                            DynRec1.Dispose();
                        }

                        InformationWrapper.Visible = true;
                    }
                }
            }

            DynAx.Dispose();
        }

        protected void CheckAccInList(object sender, EventArgs e)
        {
            f_call_ax(0);
            //Session["data_passing"] = "_ICACM";//Icentive Campaign > CustomerMaster
            //Response.Redirect("CustomerMaster.aspx");
        }

        protected void CheckAcc(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.PageIndex = 0;
            f_call_ax(0);
        }

        private void f_call_ax(int PAGE_INDEX)
        {
            string fieldName;
            AxaptaObject axQuery;
            AxaptaObject axQueryRun;
            AxaptaObject axQueryDataSource;
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;

            switch (DropDownList1.SelectedItem.Text)
            {
                case "Account No.":
                    fieldName = "AccountNum";
                    break;
                case "Customer Name":
                    fieldName = "Name";
                    break;
                case "Participated VPPP":
                    fieldName = "VPPP";
                    break;
                default:
                    fieldName = "AccountNum";
                    break;
            }

            string TableName = "CustTable";
            string AccountNum = "";
            //string fieldName = ("AccountNum");
            string fieldValue = "*" + TextBox_Account.Text.Trim() + "*";

            List<string> CustomerClass = GetAllCustomerClassStartWithAandB();
            Dictionary<string, string> AllCustomer = GetAllIncentiveCampaignCustomer(ddlAvailableCampaign.Text);
            int data_count = 13;
            string[] F = new string[data_count];
            F[0] = "AccountNum"; F[1] = "Name"; F[2] = "City";
            F[3] = "State"; F[4] = "EmplId"; F[5] = "LPPoint"; F[6] = "Phone";
            F[7] = "TeleFax"; F[8] = "CellularPhone"; F[9] = "Email"; F[10] = "Address";
            F[11] = "ParticipateVPPP"; F[12] = "CampaignStartDate";

            string[] N = new string[data_count];
            N[0] = "Account"; N[1] = "Customer Name"; N[2] = "City";
            N[3] = "State"; N[4] = "Employee ID"; N[5] = "LPPoint"; N[6] = "Phone";
            N[7] = "TeleFax"; N[8] = "Cellular Phone"; N[9] = "Email"; N[10] = "Address";
            N[11] = "Participate VPPP"; N[12] = "Campaign Start Date";

            // Output variables for calls to AxRecord.get_Field
            object[] O = new object[data_count];
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int tableId = DynAx.GetTableId(TableName);

                axQuery = DynAx.CreateAxaptaObject("Query");
                axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                if (fieldName != "VPPP")
                {
                    var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", DynAx.GetFieldId(tableId, fieldName));
                    qbr.Call("value", fieldValue);
                }

                var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);//CustGroup
                qbr4_2.Call("value", "TDI");

                var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
                qbr4_3.Call("value", "TDE");

                var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
                qbr4_4.Call("value", "TDO");

                axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                axQueryDataSource.Call("addSortField", 2, 0);//Customer Name, asc

                DataTable dt = CreateVendorDataTable(data_count, N);
                DataRow row;
                //===========================================
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                    AccountNum = DynRec.get_Field("AccountNum").ToString();

                    if (!CustomerClass.Contains(DynRec.get_Field("CustomerClass").ToString()))
                    {
                        continue;
                    }

                    if (fieldName == "VPPP")
                    {
                        if (fieldValue.ToLower() == "*yes*")
                        {
                            if (!AllCustomer.ContainsKey(AccountNum))
                            {
                                continue;
                            }
                        }

                        if (fieldValue.ToLower() == "*no*")
                        {
                            if (AllCustomer.ContainsKey(AccountNum))
                            {
                                continue;
                            }
                        }
                    }
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {

                        row = dt.NewRow();
                        row["No"] = countA;
                        for (int i = 0; i < data_count; i++)
                        {
                            if (i == 11)
                            {
                                row[N[i]] = (AllCustomer.ContainsKey(AccountNum) ? "Yes" : "No");
                            }
                            else if (i == 12)
                            {
                                row[N[i]] = (AllCustomer.ContainsKey(AccountNum) ? AllCustomer[AccountNum] : "");
                            }
                            else if (i == 5)
                            {
                                var getTpBalance = EOR_GET_NewApplicant.getPointBalance(DynAx, AccountNum);
                                double adjPts = EOR_GET_NewApplicant.getPPMTP_AdjPts(DynAx, AccountNum);
                                double usedPts = EOR_GET_NewApplicant.getPPMTP_UsedPt(DynAx, AccountNum);
                                double getTotal = getTpBalance.Item1 + adjPts + usedPts;
                                row[N[i]] = getTotal.ToString("#,###,###,##0.00");
                            }
                            else
                            {
                                O[i] = DynRec.get_Field(F[i]);
                                row[N[i]] = O[i].ToString().Trim();
                            }

                        }

                        dt.Rows.Add(row);

                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            FINISH:
                GridView1.VirtualItemCount = countA;

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ER_CM_00)
            {
                Function_Method.MsgBox("ER_CM_00: " + ER_CM_00.ToString(), this.Page, this);
            }
            DynAx.Dispose();
        }
        private DataTable CreateVendorDataTable(int data_count, string[] N)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("No", typeof(string)));
            for (int i = 0; i < data_count; i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }

            return dt;
        }


        private Dictionary<string, string> GetAllIncentiveCampaignCustomer(string CampaignID)
        {
            Dictionary<string, string> ParamDict = new Dictionary<string, string>();
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "select CustomerAccount, CampaignStartDate from campaign_document where CampaignEndDate >= @p0 AND status <> @p1";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", DateTime.Now);
            cmd.Parameters.AddWithValue("@p1", 5);//status reject is 5
            MySqlDataReader reader = cmd.ExecuteReader();

            while ((bool)reader.Read())
            {
                try
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        ParamDict.Add(reader.GetValue(0).ToString().Trim(), DateTime.Parse(reader.GetValue(1).ToString()).ToString(GLOBAL.gDisplayDateFormat).Trim());
                    }
                }
                catch
                {

                }
            }

            return ParamDict;
        }

        private List<string> GetAllCustomerClassStartWithAandB()
        {
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec = null;
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            List<string> CustomerClass = new List<string>();
            int MSBCustomerClassTable = DynAx.GetTableIdWithLock("MSBCustomerClass");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", MSBCustomerClassTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            while ((bool)axQueryRun.Call("next"))
            {
                DynRec = (AxaptaRecord)axQueryRun.Call("Get", MSBCustomerClassTable);
                string FirstLetterClassDesc = DynRec.get_Field("ClassDesc").ToString().Trim().Substring(0, 1);

                if (FirstLetterClassDesc == "A" || FirstLetterClassDesc == "B")
                {
                    CustomerClass.Add(DynRec.get_Field("CustomerClass").ToString().Trim());
                }
            }
            DynRec.Dispose();
            axQueryRun.Dispose();
            axQueryDataSource.Dispose();
            axQuery.Dispose();

            DynAx.Dispose();
            return CustomerClass;
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                f_call_ax(e.NewPageIndex);
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataBind();
            }
            catch (Exception ER_CM_01)
            {
                Function_Method.MsgBox("ER_CM_01: " + ER_CM_01.ToString(), this.Page, this);
            }
        }

        protected void Button_Account_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string CustName = "";
            string SalesmanID = "";
            Button ButtonAccount = sender as Button;

            string selected_Account = "";
            if (ButtonAccount != null)
            {
                selected_Account = ButtonAccount.Text;
            }

            string AccountNum = selected_Account;

            int CustTable = DynAx.GetTableIdWithLock("CustTable");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            int AccountNumField = DynAx.GetFieldId(CustTable, "AccountNum");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", AccountNumField);

            qbr.Call("value", AccountNum);
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                CustName = DynRec.get_Field("Name").ToString();
                SalesmanID = DynRec.get_Field("EmplId").ToString();

                txtCustName.Text = CustName;
                txtCustomerAccount.Text = DynRec.get_Field("AccountNum").ToString();

                DynRec.Dispose();
            }

            int EmplTable = DynAx.GetTableIdWithLock("EmplTable");

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

            int EmplIDField = DynAx.GetFieldId(CustTable, "EmplId");
            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);
            qbr2.Call("value", SalesmanID);
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                txtSalesman.Text = DynRec1.get_Field("DEL_Name").ToString();
                txtSalesmanID.Text = DynRec1.get_Field("EmplId").ToString();
                //hidden txt store salesman id
                DynRec1.Dispose();
            }

            HideElement();
            CreateFormWrapper.Visible = true;
            InformationWrapper.Visible = true;

            CheckIsUserParticipateCampaign();
        }

        private void HideElement()
        {
            CustomerMasterWrapper.Visible = false;
            CreateFormWrapper.Visible = false;
        }

        private void ClearVariables()
        {
            ddlTarget.ClearSelection();
            txtCustName.Text = "";
            txtCustomerAccount.Text = "";
            txtSalesman.Text = "";
            txtSalesmanID.Text = "";
        }

        protected void GenerateDocument(object sender, EventArgs e)
        {
            if (Validate())
            {
                Axapta DynAx = Function_Method.GlobalAxapta();

                if (ValidateUserDataError(DynAx))
                {
                    if (!CheckIsUserExist())
                    {
                        //Session["tempCamp"] = ddlAvailableCampaign.Text;
                        //Session["tempCust"] = txtCustomerAccount.Text;
                        string CampaignID = ddlAvailableCampaign.Text;
                        string CustAcc = txtCustomerAccount.Text;
                        Insert(DynAx);
                        ClearVariables();

                        UpdateProgress.Visible = false;
                        //GlobalHelper.RedirectToNewTab("Campaign_DocumentPDF.aspx", "_blank", "menubar=1000,width=1000,height=1000");
                        string url = $"Campaign_DocumentPDF.aspx?tempCamp={CampaignID}&tempCust={CustAcc}";
                        Response.Redirect(url, true);

                        return;
                    }

                    //DownloadPDF();
                }
                #region DownloadPDF
                if (CheckIsUserExist())
                {
                    string CampaignID = ddlAvailableCampaign.Text;
                    string CustAcc = txtCustomerAccount.Text;

                    string url = $"Campaign_DocumentPDF.aspx?tempCamp={CampaignID}&tempCust={CustAcc}";
                    Response.Redirect(url, true);
                }
                else
                {
                    Function_Method.MsgBox("Please fill in all the input.", this.Page, this);

                }
                #endregion

                DynAx.Dispose();
            }

            //Function_Method.MsgBox("Please fill in all the input.", this.Page, this);

        }

        private bool CheckIsUserExist()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();
            bool IsExist = false;
            string SQL = "select CampaignID, CustomerAccount from campaign_document where CampaignID = @p0 and CustomerAccount = @p1 AND status <> @p2";
            string CampaignID = ddlAvailableCampaign.Text;
            string CustAccount = txtCustomerAccount.Text.Trim();

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", CampaignID);
            cmd.Parameters.AddWithValue("@p1", CustAccount);
            cmd.Parameters.AddWithValue("@p2", 5);//status reject is 5

            MySqlDataReader reader = cmd.ExecuteReader();

            while ((bool)reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    IsExist = true;
                }
            }
            conn.Close();
            return IsExist;
        }

        private bool ValidateUserDataError(Axapta DynAx)
        {
            bool isValidate = true;
            string AccountNum = txtCustomerAccount.Text;

            int CustTable = DynAx.GetTableIdWithLock("CustTable");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            int AccountNumField = DynAx.GetFieldId(CustTable, "AccountNum");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", AccountNumField);

            qbr.Call("value", AccountNum);
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);

                if (txtCustName.Text.Trim() != DynRec.get_Field("Name").ToString().Trim())
                {
                    Function_Method.MsgBox("User Account not match.", this.Page, this);
                    DynRec.Dispose();

                    isValidate = false;
                }
                DynRec.Dispose();
            }
            else
            {
                Function_Method.MsgBox("User Account not found.", this.Page, this);

                isValidate = false;
            }

            return isValidate;
        }

        private void Insert(Axapta DynAx)
        {
            CompiledModel m = ConvertIntoDocumentModel(DynAx);

            try
            {
                //Insert campaign document
                string CampaignDocumentTable = "campaign_document";

                List<string> DocumentColumnList = GlobalHelper.GetColumnsByModel(m.cdm);
                Dictionary<string, object> DocumentParamDict = GlobalHelper.ConvertModelValuesIntoDictionary(m.cdm);

                GlobalHelper.InsertQuery(CampaignDocumentTable, DocumentColumnList, DocumentParamDict);

                //Insert Campaign Target Header
                string TargetHeaderTable = "campaign_targetheader";
                CampaignTargetHeaderModel HeaderModel = new CampaignTargetHeaderModel();
                List<string> TargetHeaderColumnList = GlobalHelper.GetColumnsByModel(HeaderModel);
                Dictionary<string, object> TargetHeaderParamDict = GlobalHelper.ConvertModelListValuesIntoDictionary(m.cthm);

                GlobalHelper.BulkInsertQuery(TargetHeaderTable, TargetHeaderColumnList, TargetHeaderParamDict, m.cthm.Count());

                //Insert Campaign Target Percent
                string TargetPercentTable = "campaign_targetpercent";
                CampaignTargetPercentModel PercentModel = new CampaignTargetPercentModel();
                List<string> TargetPercentColumnList = GlobalHelper.GetColumnsByModel(PercentModel);
                Dictionary<string, object> TargetPercentParamDict = GlobalHelper.ConvertModelListValuesIntoDictionary(m.ctpm);

                GlobalHelper.BulkInsertQuery(TargetPercentTable, TargetPercentColumnList, TargetPercentParamDict, m.ctpm.Count());
            }
            catch (Exception ex)
            {

            }
        }

        public CompiledModel ConvertIntoDocumentModel(Axapta DynAx)
        {
            CompiledModel m = new CompiledModel();

            CampaignDocumentModel DocumentModel = new CampaignDocumentModel();
            List<double> TargetList = new List<double>();

            string CampaignID = ddlAvailableCampaign.Text;
            string CustAccount = txtCustomerAccount.Text.Trim();
            string CustName = txtCustName.Text.Trim();
            string CampaignTerms = "";
            int CampaignMaster = DynAx.GetTableIdWithLock("MSB_CampaignMaster");
            int SequenceNo = 1;

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();
            string SQL = "select max(SequenceNo) SequenceNo from campaign_document where CampaignID = @p0 AND status <> @p1 ";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            cmd.Parameters.AddWithValue("@p0", CampaignID);
            cmd.Parameters.AddWithValue("@p1", 5);//status reject is 5

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        SequenceNo = Int32.Parse(reader.GetValue(0).ToString()) + 1;
                    }
                }
            }
            conn.Close();

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CampaignMaster);

            int CampaignIDField1 = DynAx.GetFieldId(CampaignMaster, "MSB_CampaignID");
            var qbr1_1 = (AxaptaObject)axQueryDataSource1.Call("addRange", CampaignIDField1);

            qbr1_1.Call("value", CampaignID);
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CampaignMaster);

                CampaignTerms = DynRec1.get_Field("Terms").ToString();

                DynRec1.Dispose();
            }

            
            #region Changes04/07/2025

            DocumentModel.CampaignID = CampaignID;
            DocumentModel.SequenceNo = SequenceNo;
            DocumentModel.CustomerAccount = CustAccount;
            DocumentModel.WorkshopName = txtCustName.Text.Trim();
            DocumentModel.Salesman = txtSalesman.Text.Trim();
            DocumentModel.SalesmanID = txtSalesmanID.Text;
            DocumentModel.CampaignTerms = CampaignTerms;

            DocumentModel.CreatedDateTime = DateTime.Now;
            DocumentModel.CreatedBy = GLOBAL.user_id;
            DocumentModel.UpdatedDateTime = DateTime.MinValue;
            DocumentModel.UpdatedBy = "";
            DocumentModel.Status = DocumentStatus.Empty;
            DocumentModel.FileID = 0;

            #region MSB_CampaignDetail
            int CampaignDetails = DynAx.GetTableIdWithLock("MSB_CampaignDetail");

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", CampaignDetails);

            int CampaignIDField2 = DynAx.GetFieldId(CampaignDetails, "CampaignID");
            var qbr2_1 = (AxaptaObject)axQueryDataSource2.Call("addRange", CampaignIDField2);

            qbr2_1.Call("value", CampaignID);

            int CustAccountField2 = DynAx.GetFieldId(CampaignDetails, "CustAccount");
            var qbr2_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", CustAccountField2);

            qbr2_2.Call("value", CustAccount);
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CampaignDetails);

                DocumentModel.CampaignStartDate = DateTime.Parse(DynRec2.get_Field("PYDateFrom").ToString());
                DocumentModel.CampaignEndDate = DateTime.Parse(DynRec2.get_Field("PYDateTo").ToString());
                DocumentModel.PastYearSales = Double.Parse(Double.Parse(DynRec2.get_Field("PYSales").ToString()).ToString("N2"));
                DocumentModel.DocumentRecDate = DateTime.Parse(DynRec2.get_Field("DocumentRecDate").ToString());

                #region MSB_CampaignTargetHeader    
                string TargetRefRecId = DynRec2.get_Field("TargetRefRecId").ToString();
                int MSB_CampaignTargetHeader = DynAx.GetTableIdWithLock("MSB_CampaignTargetHeader");

                AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", MSB_CampaignTargetHeader);

                int RecId = DynAx.GetFieldId(MSB_CampaignTargetHeader, "RecId");
                var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", RecId);

                qbr6.Call("value", TargetRefRecId);
                AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

                if ((bool)axQueryRun6.Call("next"))
                {
                    AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", MSB_CampaignTargetHeader);

                    TargetList.Add(double.Parse(DynRec6.get_Field("Target").ToString()));
                    TargetList.Add(double.Parse(DynRec6.get_Field("SecondLevelTarget").ToString()));
                    DynRec6.Dispose();
                }
                #endregion

                DynRec2.Dispose();
            }
            else
            {
                DateTime campaignStartDate;
                DateTime campaignEndDate;
                campaignStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                campaignEndDate = campaignStartDate.AddYears(1).AddDays(-1);

                // Assign the values to DocumentModel
                DocumentModel.CampaignStartDate = campaignStartDate;
                DocumentModel.CampaignEndDate = campaignEndDate;
            }
            #endregion
            m.cdm = DocumentModel;
            #endregion
            ConvertIntoTargetHeaderModelByCampaignIDAndTargets(DynAx, m, CampaignID, CustAccount, TargetList);


            return (m == null ? new CompiledModel() : m);
        }

        private void ConvertIntoTargetHeaderModelByCampaignIDAndTargets(Axapta DynAx, CompiledModel m, string CampaignID, string CustAcc, List<double> Targets)
        {
            List<CampaignTargetPercentModel> PercentModel = new List<CampaignTargetPercentModel>();

            List<string> RecIDList = new List<string>();
            List<string> ProductList = new List<string>();

            List<CampaignTargetHeaderModel> HeaderModel = new List<CampaignTargetHeaderModel>();
            List<double> TargetList = new List<double>();

            int TargetHeaderTable = DynAx.GetTableIdWithLock("MSB_CampaignTargetHeader");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", TargetHeaderTable);

            int CampaignIDField = DynAx.GetFieldId(TargetHeaderTable, "CampaignID");
            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", CampaignIDField);

            qbr.Call("value", CampaignID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", TargetHeaderTable);
                double Target = Double.Parse(DynRec.get_Field("Target").ToString());

                if (Targets.Contains(Target))
                {
                    CampaignTargetHeaderModel hm = new CampaignTargetHeaderModel();
                    hm.TargetID = DynRec.get_Field("RecId").ToString();
                    hm.CampaignID = CampaignID;
                    hm.CustomerAccount = CustAcc;
                    hm.TargetAmount = Target;

                    HeaderModel.Add(hm);

                    RecIDList.Add(DynRec.get_Field("RecId").ToString());
                }

                DynRec.Dispose();
            }

            if (HeaderModel.Count == 0)
            {
                string[] HeaderTarget = ddlTarget.SelectedValue.Split('-');
                for (int i = 1; i < HeaderTarget.Count(); i++)
                {
                    string TargetID = (i == 1 ? HeaderTarget[0] : (Int64.Parse(HeaderTarget[0]) - 1).ToString());

                    CampaignTargetHeaderModel hm = new CampaignTargetHeaderModel();
                    hm.TargetID = TargetID;
                    hm.CampaignID = CampaignID;
                    hm.CustomerAccount = CustAcc;
                    hm.TargetAmount = Double.Parse(HeaderTarget[i]);

                    HeaderModel.Add(hm);

                    RecIDList.Add(TargetID);
                }
            }

            int TargetPercentTable = DynAx.GetTableIdWithLock("MSB_CampaignTargets");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", TargetPercentTable);

            int CampaignIDField1 = DynAx.GetFieldId(TargetPercentTable, "CampaignID");
            var qbr1_1 = (AxaptaObject)axQueryDataSource1.Call("addRange", CampaignIDField1);

            qbr1_1.Call("value", CampaignID);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            while ((bool)axQueryRun1.Call("next"))
            {
                CampaignTargetPercentModel pm = new CampaignTargetPercentModel();
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun1.Call("Get", TargetPercentTable);
                string RefRecId = DynRec.get_Field("RefRecId").ToString();

                if (RecIDList.Contains(RefRecId))
                {
                    string ProductGroupAlphabet = DynRec.get_Field("MSB_CampaignItemGroup").ToString();
                    if (ProductGroupAlphabet == "1")
                    {
                        ProductGroupAlphabet = "A";
                    }
                    else if (ProductGroupAlphabet == "2")
                    {
                        ProductGroupAlphabet = "B";
                    }
                    else if (ProductGroupAlphabet == "3")
                    {
                        ProductGroupAlphabet = "C";
                    }
                    else if (ProductGroupAlphabet == "4")
                    {
                        ProductGroupAlphabet = "D";
                    }
                    else if (ProductGroupAlphabet == "5")
                    {
                        ProductGroupAlphabet = "E";
                    }
                    string Product = "Product " + ProductGroupAlphabet;
                    double Percent = Double.Parse(DynRec.get_Field("Percent").ToString());
                    string ID = DynRec.get_Field("RecId").ToString();

                    pm.ID = ID;
                    pm.RefTargetID = RefRecId;
                    pm.CampaignID = CampaignID;
                    pm.CustomerAccount = CustAcc;
                    pm.Product = Product;
                    pm.Percent = Percent;

                    PercentModel.Add(pm);
                }

                DynRec.Dispose();
            }

            m.cthm = HeaderModel;
            m.ctpm = PercentModel;
        }
        private bool Validate()
        {
            bool isValidate = true;

            if (string.IsNullOrEmpty(txtCustomerAccount.Text) || string.IsNullOrEmpty(ddlAvailableCampaign.Text))
            {
                isValidate = false;
            }

            if (DateWrapper.Visible == true && (string.IsNullOrEmpty(txtFromDate.Text) || string.IsNullOrEmpty(ddlTarget.SelectedValue)))
            {
                isValidate = false;
            }

            return isValidate;
        }
    }
}