using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNet.CampaignModel.CampaignModel;
using static DotNet.Visitor_MainMenu;
using static DotNet.Visitor_Model.AppointmentModel;
using GLOBAL_VAR;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Web.UI.WebControls.Button;
using System.Web.Services;
using System.IO;
using SelectPdf;
using static System.Net.WebRequestMethods;
using System.Data.SqlClient;
using NPOI.SS.Formula.Functions;
using iText.StyledXmlParser.Jsoup.Nodes;
using System.Web.Services.Description;
using NPOI.HSSF.Record;
using NLog;
namespace DotNet
{
    public partial class Campaign_SubmitForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Campaign_CheckSession.SystemCheckSession(this.Page, this);

            if (GLOBAL.CampaignReport == false)
            {
                export.Visible = false;
                dateRange_section.Visible = false;
            }
            if (!IsPostBack)
            {
                LoadDropdown();
                if (Request.QueryString["download"] == "true")
                {
                    MonthDateWrapper.Visible = true;
                    filter_section.Visible = false;
                }
                else
                {
                    MonthDateWrapper.Visible = false;
                    filter_section.Visible = true;
                }
                RerenderGridView();

            }
        }

        private void LoadDropdown()
        {
            ddlCampaignList.Items.Clear();
            ddlStatus.Items.Clear();

            List<ListItem> Items = new List<ListItem>();
            Items.Add(new ListItem("All", ""));
            Axapta DynAx = Function_Method.GlobalAxapta();
            int CampaignTable = DynAx.GetTableIdWithLock("MSB_CampaignMaster");
            string CampaignID = "";
            string CampaignDesc = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CampaignTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CampaignTable);
                DateTime CurrentDateTime = DateTime.Now;
                DateTime CampaignEndDateTime = DateTime.Parse(DynRec.get_Field("EndDate").ToString());
                CampaignID = DynRec.get_Field("MSB_CampaignID").ToString();
                //CampaignDesc = DynRec.get_Field("CampaignDesc").ToString();
                CampaignDesc = DynRec.get_Field("MSB_CampaignID").ToString();

                if (CurrentDateTime < CampaignEndDateTime)
                {
                    Items.Add(new ListItem(CampaignDesc, CampaignID));
                }
                DynRec.Dispose();
            }
            axQueryRun.Dispose();
            axQueryDataSource.Dispose();
            axQuery.Dispose();

            DynAx.Dispose();
            ddlCampaignList.Items.AddRange(Items.ToArray());

            Items = new List<ListItem>();

            Items.Add(new ListItem("All", "-1"));

            List<DocumentStatus> DocStatuses = new List<DocumentStatus>();

            foreach (DocumentStatus item in Enum.GetValues(typeof(DocumentStatus)))
            {
                DocStatuses.Add(item);
            }

            DocumentStatus[] Order =
            {
            DocumentStatus.Empty,
            DocumentStatus.Renewal,
            DocumentStatus.Uploaded,
            DocumentStatus.Rejected,
            DocumentStatus.Approved,
            DocumentStatus.Canceled
            };

            List<DocumentStatus> OrderedStatuses = DocStatuses.OrderBy(status => Array.IndexOf(Order, status)).ToList();

            foreach (var item in OrderedStatuses)
            {
                if (item.ToString() == "Renewal")
                {
                    Items.Add(new ListItem("Renewal - Pending Chop Sign", ((int)Enum.Parse(typeof(DocumentStatus), item.ToString())).ToString()));

                    continue;
                }
                Items.Add(new ListItem((item.ToString() == "Empty" ? "New" : item.ToString()), ((int)Enum.Parse(typeof(DocumentStatus), item.ToString())).ToString()));
            }

            ddlStatus.Items.AddRange(Items.ToArray());
            if (GLOBAL.CampaignReport)
            {
                ddlStatus.SelectedValue = "3";
            }
            else
            {
                ddlStatus.SelectedValue = "0";
            }

            ddlStatus.DataBind();

            ddlDownloadStatus.SelectedIndex = 1;
        }

        protected void FilterRecords(object sender, EventArgs e)
        {
            RerenderGridView();
        }

        private void RerenderGridView()
        {
            RerenderGridView(0);
        }

        private void RerenderGridView(int PageIndex)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["status"]))
            {
                ddlStatus.ClearSelection(); ddlStatus.Items.FindByText(Request.QueryString["status"]).Selected = true;
            }
            string UserName = Session["user_id"].ToString();
            string CampaignID = ddlCampaignList.SelectedItem.Value;
            string CustAccount = txtCustomerAccount.Text.Trim();
            int Status = Int32.Parse(ddlStatus.SelectedItem.Value);
            DateTime StartDate = DateTime.Now.AddMonths(-15).Date;
            DateTime EndDate = DateTime.Now.AddYears(1).AddMonths(6);
            try
            {
                if (!GLOBAL.CampaignReport && Request.QueryString["download"] != "true")
                {
                    dateRange_section.Visible = false;
                }
                else if (Request.QueryString["download"] == "true")
                {
                    StartDate = DateTime.Parse(txtMonthDate.Text);
                    EndDate = StartDate.AddMonths(6).AddSeconds(-1);
                }
                else
                {
                    StartDate = DateTime.Parse(txtFromDate.Text).Date;
                    EndDate = DateTime.Parse(txtToDate.Text).Date.AddDays(1).AddSeconds(-1);
                }

            }
            catch
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                EndDate = DateTime.Now.AddMonths(1).AddSeconds(-1);
            }

            //InformationWrapper.Visible = true;

            GenerateHistoryGridView(UserName, CampaignID, Status, CustAccount, StartDate, EndDate, PageIndex);
        }

        protected void ActionButton(object sender, EventArgs e)
        {
            // Determine the type of the sender and cast accordingly
            string commandArgument = string.Empty;
            string buttonText = string.Empty;
            logger.Info("Action Button - Upload Campaign Start");
            if (sender is Button)
            {
                Button button = (Button)sender; // Cast sender to Button
                commandArgument = button.CommandArgument;
                buttonText = button.Text;
            }
            else if (sender is LinkButton)
            {
                LinkButton linkButton = (LinkButton)sender; // Cast sender to LinkButton
                commandArgument = linkButton.CommandArgument;
                buttonText = linkButton.Text;
            }
            else
            {
                return; // If sender is neither Button nor LinkButton, exit the method
            }
            logger.Info("Action Button  - Document Status");
            DocumentStatus Status = DocumentStatus.Uploaded;
            List<string> parameters = commandArgument.ToString().Split('|').ToList();

            if (buttonText == "Approve")
            {
                Status = DocumentStatus.Approved;
            }
            else if (buttonText == "Cancel")
            {
                Status = DocumentStatus.Canceled;
            }
            else if (buttonText == "Reject")
            {
                RecordID.Value = commandArgument;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none",
"<script>$('#myModal').show();</script>", false);

                return;
            }
            else if (buttonText == "Delete")
            {
                string CampaignID = parameters[0].ToString();
                string CustAccount = parameters[1].ToString();
                logger.Info("Action Button  - Delete New Campaign");
                DeleteNewCampaignDocument(CampaignID, CustAccount);

                Function_Method.MsgBox("Delete Successfully.", this.Page, this);

                RerenderGridView();

                return;
            }
            else if (buttonText == "OK")
            {
                Status = DocumentStatus.Rejected;
            }
            else if (buttonText == "Download")
            {
                string url = $"Campaign_DocumentPDF.aspx?tempCamp={parameters[0]}&tempCust={parameters[1]}";
                GlobalHelper.RedirectToNewTab(url, "_blank", "menubar=1000,width=1000,height=1000");

                return;
            }
            else
            {
                return;
            }
            logger.Info("Action Button  - Move Axapta stores to 1st then phpMySql");
            ///Move Axapta stores to 1st then phpMySql
            if (Status == DocumentStatus.Approved || Status == DocumentStatus.Canceled)
            {
                if (!SaveIntoMSBCampaignDetails(commandArgument, Status))
                {
                    // Log and exit if Axapta save failed
                    logger.Error("Failed to save to Axapta - aborting entire operation");
                    return;
                }
            }
            //////////////////////////////////////////
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();
            logger.Info("Action Button  - mySql Start");
            string UserName = GLOBAL.user_id;
            string TableName = "campaign_document";
            string RejectRemarks = txtRejectReason.Text;
            List<string> ColumnList = new List<string> { "UpdatedDateTime", "UpdatedBy", "Status", "DocumentRecDate", "RejectRemarks" };
            List<object> ObjectList = new List<object> { DateTime.Now, UserName, Status, DateTime.Now, RejectRemarks };
            Dictionary<string, object> ParamDict = GlobalHelper.ConvertModelColumnsIntoDictionary(ColumnList, ObjectList);
            if (buttonText != "OK")
            {
                ParamDict.Add("@c1", commandArgument);
            }
            else
            {
                ParamDict.Add("@c1", RecordID.Value);
            }
            string Condition = " where ID = @c1";
            MySqlCommand cmd = GlobalHelper.UpdateQuery(TableName, ColumnList, ParamDict, Condition);

            cmd.ExecuteNonQuery();
            conn.Close();


            //IF WANT TO UPDATE AXAPTA 
            //Axapta DynAx = Function_Method.GlobalAxapta();

            //using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord(""/*If approve, submit document column table, If Cancel, */))
            //{
            //    DynAx.TTSBegin();

            //    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "EmplId", "0000"));
            //    if (DynRec.Found)
            //    {
            //        //if (ClaimStatus != "") DynRec.set_Field("ClaimStatus", ClaimStatus);
            //        DynRec.set_Field("Del_UserId", "Testing");
            //        DynRec.set_Field("DEL_Phone", "0123456789");
            //        DynRec.Call("Update");
            //    }
            //    DynAx.TTSCommit();
            //    DynAx.TTSAbort();
            //}

            Function_Method.MsgBox(Status.ToString() + " Successfully.", this.Page, this);

            RerenderGridView();
        }

        private void DeleteNewCampaignDocument(string CampaignID, string CustAccount)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "delete from campaign_document where CampaignID = @p0 and CustomerAccount = @p1";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            cmd.Parameters.AddWithValue("@p0", CampaignID);
            cmd.Parameters.AddWithValue("@p1", CustAccount);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private string GetDocumentRecordsQuery(string CampaignID, int Status, string CustAccount, DateTime StartDate, DateTime EndDate, int PageIndex, bool IsCount)
        {
            string SQL = "select ";

            if (IsCount)
            {
                SQL += "count(1) Count ";
            }
            else
            {
                SQL += "t1.*, IFNULL(t2.FileName, '') FileName, t3.Target ";
            }
            SQL += " from campaign_document t1 left join file_library t2 on t2.FileID = t1.FileID " +
                " left join (select max(TargetAmount) Target, CampaignID, CustomerAccount from campaign_targetheader group by CampaignID, CustomerAccount) t3 on t3.CampaignID = t1.CampaignID and t3.CustomerAccount = t1.CustomerAccount";

            if (!string.IsNullOrEmpty(CampaignID) || Status >= 0 || !string.IsNullOrEmpty(CustAccount) || StartDate != DateTime.MinValue)
            {
                List<string> Condition = new List<string>();
                SQL += " where ";
                if (!string.IsNullOrEmpty(CampaignID))
                {
                    Condition.Add(" t1.CampaignID = @p1 ");
                }
                if (Status >= 0)
                {
                    if (Request.QueryString["download"] != "true")
                    {
                        Condition.Add(" t1.Status = @p2 ");
                    }
                }
                if (!string.IsNullOrEmpty(CustAccount))
                {
                    Condition.Add(" t1.CustomerAccount like @p3 ");
                }

                if (GLOBAL.CampaignReport == false)
                {
                    if (Status == 4 || Status == -1)
                    {

                        Condition.Add(" ((t1.CreatedBy = @p0 or t1.Salesman like @p7 )) OR t1.Status = '4' ");
                    }
                    else
                    {
                        Condition.Add(" ((t1.CreatedBy = @p0 or t1.Salesman like @p7 )) ");
                    }
                }

                if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
                {
                    if (Request.QueryString["download"] == "true")
                    {
                        Condition.Add(" t1.CampaignStartDate >= @p4 and t1.CampaignStartDate <= @p5 ");

                        if (ddlDownloadStatus.SelectedValue == "1")
                        {
                            Condition.Add(" t1.Status != '1' ");
                        }
                    }
                    else
                    {
                        if (GLOBAL.CampaignReport == true)
                        {
                            Condition.Add(" t1.UpdatedDateTime >= @p4 and t1.UpdatedDateTime <= @p5 ");
                        }
                        else
                        {
                            Condition.Add(" t1.CampaignEndDate >= @p4 and t1.CampaignEndDate <= @p5 ");
                        }
                    }
                }

                SQL += string.Join("and", Condition);
            }
            else
            {
                if (GLOBAL.CampaignReport == false)
                {
                    SQL += " where ( (t1.CreatedBy = @p0 or t1.Salesman like @p7) " + (Request.QueryString["download"] == "true" ? " and t1.CampaignEndDate <= @p6 " : "") + " ) OR t1.Status = '4' ";
                }
                else
                {
                    // If GLOBAL.CampaignReport == true and no filters, just show Status=4 as well
                    SQL += " where t1.Status = '4' ";
                }
            }

            if (GLOBAL.CampaignReport == false)
            {
                SQL += " Order by t1.CampaignEndDate desc ";
            }
            else
            {
                SQL += " Order by t1.UpdatedDateTime desc ";
            }
            //if (!IsCount)
            //{
            //    SQL += " limit 20 offset " + PageIndex;
            //}

            return SQL;
        }

        protected void datagrid_PageIndexChanging_DocumentRecords(object sender, GridViewPageEventArgs e)
        {
            int PageIndex = e.NewPageIndex * 20;

            RerenderGridView(PageIndex);
        }

        public void GenerateHistoryGridView(string UserName, string CampaignID, int Status, string CustAccount, DateTime StartDate, DateTime EndDate)
        {
            GenerateHistoryGridView(UserName, CampaignID, Status, CustAccount, StartDate, EndDate, 0);
        }

        public void GenerateHistoryGridView(string UserName, string CampaignID, int Status, string CustAccount, DateTime StartDate, DateTime EndDate, int PageIndex)
        {
            var context = HttpContext.Current;
            CampaignDocumentViewModel m = new CampaignDocumentViewModel();
            gvDocumentRecords.DataSource = null;
            gvDocumentRecords.DataBind();
            // Log on to Microsoft Dynamics AX.
            Axapta DynAx = Function_Method.GlobalAxapta();

            DataTable dt = new DataTable();
            List<string> N = new List<string> { "No.", "Action", "FileURL" };
            N.AddRange(GlobalHelper.GetColumnsByModel(m));
            N.Add("FileName");

            for (int i = 0; i < N.Count(); i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }
            //===========================================
            DataRow row;
            int Count = 0;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {
                #region CheckHOD
                #region Get EmplTable
                ////var EmplResult = Function_Method.GetEmplTable_LF_EmplName_withDynAx(DynAx, GLOBAL.user_id);//(EmplID, LF_EmplName, ReportTo)

                #endregion

                #region GetHOD_From_LF_SalesApprovalLevel

                //string HOD_1 = "", HOD_2 = "", HOD_3 = "", HOD_4 = "";
                //string ReportTo = "";

                //bool isNotEmpty = !string.IsNullOrEmpty(EmplResult.Item1) ||
                // !string.IsNullOrEmpty(EmplResult.Item2) ||
                // !string.IsNullOrEmpty(EmplResult.Item3);

                
                //if (isNotEmpty)
                //{
                //    string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, EmplResult.Item1);
                //    string[] arr_NA_HOD = getHod.Split('_');
                //    int count_arr_NA_HOD = arr_NA_HOD.Count();
                //    #region CheckAllHODName
                //    if (count_arr_NA_HOD > 0)
                //    {
                //        HOD_1 = Function_Method.GetLoginedUserFullName(arr_NA_HOD[0])?.Split('(')[0] ?? arr_NA_HOD[0]?.Split('(')[0];
                //        ReportTo = (arr_NA_HOD[0])?.Split('(')[0];
                //        if (count_arr_NA_HOD > 1)
                //        {
                //            HOD_2 = Function_Method.GetLoginedUserFullName(arr_NA_HOD[1])?.Split('(')[0] ?? arr_NA_HOD[1]?.Split('(')[0];
                //            ReportTo = (arr_NA_HOD[1])?.Split('(')[0];

                //        }
                //        if (count_arr_NA_HOD > 2)
                //        {
                //            HOD_3 = Function_Method.GetLoginedUserFullName(arr_NA_HOD[2])?.Split('(')[0] ?? arr_NA_HOD[2]?.Split('(')[0];
                //            ReportTo = (arr_NA_HOD[2])?.Split('(')[0];

                //        }
                //        if (count_arr_NA_HOD > 3)
                //        {
                //            HOD_4 = Function_Method.GetLoginedUserFullName(arr_NA_HOD[3])?.Split('(')[0] ?? arr_NA_HOD[3]?.Split('(')[0];
                //            ReportTo = (arr_NA_HOD[3])?.Split('(')[0];
                //        }
                //    }
                //}

                #endregion


                #endregion
                

                DateTime FilteredStartDateTime = DateTime.Now;
                DateTime FilteredEndDateTime = FilteredStartDateTime;
                string CountQuery = GetDocumentRecordsQuery(CampaignID, Status, CustAccount, StartDate, EndDate, PageIndex, true);

                string Query = GetDocumentRecordsQuery(CampaignID, Status, CustAccount, StartDate, EndDate, PageIndex, false);
                DateTime StartDateToday = DateTime.Now.Date;
                DateTime EndDateToday = StartDateToday.AddDays(1).AddSeconds(-1);
                DateTime DateTimeNow = DateTime.Now.AddHours(-1);

                MySqlCommand cmd = new MySqlCommand(Query, conn);
                MySqlCommand cmdCount = new MySqlCommand(CountQuery, conn);

                Dictionary<string, object> ParamDict = new Dictionary<string, object>
                    {
                        { "@p0", UserName },
                        { "@p1", CampaignID },
                        { "@p2", Status },
                        { "@p3", "%" + CustAccount + "%" },
                        { "@p4", StartDate },
                        { "@p5", EndDate },
                        { "@p6", DateTimeNow },
                        { "@p7", GLOBAL.logined_user_name.Split('(')[0] },
                         //{ "@p8", ReportTo },
                    };
                GlobalHelper.PumpParamQuery(cmd, ParamDict);
                GlobalHelper.PumpParamQuery(cmdCount, ParamDict);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            string createdBy = reader.GetString("CreatedBy").Trim();
                            bool addRow = true;
                            string SalesmanName = "";

                            string checkuserID = Session["user_id"].ToString();
                            var adminUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    { "phuasp", "tanwl", "tseec", "foozm" };

                            if (!adminUsers.Contains(checkuserID))
                            {
                                if (createdBy != Session["user_id"].ToString())
                                {
                                    string CustAcc = reader.GetString("CustomerAccount").Trim();
                                    string SalesmanID = "";
                                    string UserID = "";
                                    #region Axapta check
                                    int CustTable = DynAx.GetTableIdWithLock("CustTable");

                                    AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
                                    AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

                                    int AccountNumField = DynAx.GetFieldId(CustTable, "AccountNum");
                                    var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", AccountNumField);

                                    qbr4.Call("value", CustAcc);
                                    AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);

                                    if ((bool)axQueryRun4.Call("next"))
                                    {
                                        AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                                        //CustName = DynRec4.get_Field("Name").ToString();
                                        SalesmanID = DynRec4.get_Field("EmplId").ToString();

                                        DynRec4.Dispose();
                                    }
                                    else  //Since this customer not exist, then this campaign no need visible to user.
                                    {
                                        addRow = false;
                                    }

                                    axQuery4.Dispose();
                                    axQueryDataSource4.Dispose();
                                    axQueryRun4.Dispose();

                                    if (SalesmanID != "")
                                    {

                                        int EmplTable = DynAx.GetTableIdWithLock("EmplTable");

                                        AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
                                        AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", EmplTable);

                                        int EmplIDField = DynAx.GetFieldId(CustTable, "EmplId");
                                        var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 1);
                                        qbr5.Call("value", SalesmanID);
                                        AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);

                                        if ((bool)axQueryRun5.Call("next"))
                                        {
                                            AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", EmplTable);
                                            //SalesmanName = DynRec5.get_Field("DEL_Name").ToString();
                                            string EmplEmail = DynRec5.get_Field("LF_EmpEmailID").ToString();
                                            SalesmanName = DynRec5.get_Field("LF_EmplName").ToString();
                                            SalesmanName = (string.IsNullOrEmpty(SalesmanName) ? DynRec5.get_Field("Del_Name").ToString() : SalesmanName);
                                            //SalesmanName = DynRec5.get_Field("Del_Name").ToString();
                                            DynRec5.Dispose();

                                            UserID = EmplEmail.Split('@')[0].ToString();
                                        }
                                        axQuery5.Dispose();
                                        axQueryDataSource5.Dispose();
                                        axQueryRun5.Dispose();
                                    }
                                    #endregion
                                    if (UserID != Session["user_id"].ToString())
                                    {
                                        addRow = false;
                                    }

                                }
                            }
                            if (addRow)
                            {
                                #region dt Row
                                row = dt.NewRow();

                                row["ID"] = reader.GetString("ID");
                                row["CampaignID"] = reader.GetString("CampaignID");
                                row["SequenceNo"] = reader.GetString("SequenceNo");
                                row["CustomerAccount"] = reader.GetString("CustomerAccount");
                                row["WorkshopName"] = reader.GetString("WorkshopName");

                                row["RejectRemarks"] = reader.GetString("RejectRemarks");
                                //row["Salesman"] = reader.GetString("Salesman");
                                row["Salesman"] = (string.IsNullOrEmpty(SalesmanName) ? reader.GetString("Salesman") : SalesmanName);
                                row["Target"] = Double.Parse(reader.GetString("Target")).ToString("N");
                                //row["SalesmanID"] = DateTime.Parse(reader.GetString("StartDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat);
                                //row["EndDateTime"] = DateTime.Parse(reader.GetString("EndDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat);
                                row["CampaignStartDate"] = (reader.GetString("CampaignStartDate") == DateTime.MinValue.ToString() ? "" : DateTime.Parse(reader.GetString("CampaignStartDate")).ToString(GLOBAL.gDisplayDateFormat));
                                row["CampaignEndDate"] = (reader.GetString("CampaignEndDate") == DateTime.MinValue.ToString() ? "" : DateTime.Parse(reader.GetString("CampaignEndDate")).ToString(GLOBAL.gDisplayDateFormat));
                                row["PastYearSales"] = Double.Parse(reader.GetString("PastYearSales")).ToString("N");
                                row["CreatedDateTime"] = DateTime.Parse(reader.GetString("CreatedDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat);
                                row["CreatedBy"] = reader.GetString("CreatedBy");
                                row["UpdatedDateTime"] = (reader.GetString("UpdatedDateTime") == DateTime.MinValue.ToString() ? "" : DateTime.Parse(reader.GetString("UpdatedDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat));
                                row["UpdatedBy"] = reader.GetString("UpdatedBy");
                                row["FileURL"] = "Handler/ImageHandler.ashx?id=" + reader.GetString("FileID");
                                row["DocumentRecDate"] = (reader.GetString("DocumentRecDate") == DateTime.MinValue.ToString() ? "" : DateTime.Parse(reader.GetString("DocumentRecDate")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat));
                                string strStatus = Enum.GetName(typeof(DocumentStatus), Int32.Parse(reader.GetString("Status")));

                                if (reader.GetString("Status") == "0")
                                {
                                    row["Status"] = "New";
                                }
                                else if (reader.GetString("Status") == "4")
                                {
                                    row["Status"] = "Renewal - Pending Chop";
                                }
                                else
                                {
                                    row["Status"] = strStatus;

                                }

                                row["FileID"] = reader.GetString("FileID");
                                //row["FileURL"] = "ImageHandler.ashx?id=" + reader.GetString("FileID");
                                row["FileName"] = reader.GetString("FileName");
                                row["IsOriReceive"] = reader.GetString("IsOriReceive");

                                dt.Rows.Add(row);
                                #endregion
                            }
                        }
                    }
                }

                using (MySqlDataReader reader = cmdCount.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            Count = Int32.Parse(reader.GetValue(0).ToString());
                        }
                    }
                }
                conn.Close();

                if (Count > 0)
                {
                    imgbtnExport.Visible = true;
                }
                else
                {
                    imgbtnExport.Visible = false;
                }

                gvDocumentRecords.PagerSettings.Visible = true;

                gvDocumentRecords.VirtualItemCount = Count;
                //Open if Page index enable
                //gvDocumentRecords.PageIndex = PageIndex;
                gvDocumentRecords.DataSource = dt;
                gvDocumentRecords.DataBind();
            }
            catch (Exception ER_CSF_00)
            {
                Function_Method.MsgBox("ER_CSF_00: " + ER_CSF_00.ToString(), this.Page, this);
            }
        }

        protected void gvDocumentRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var context = HttpContext.Current;

            foreach (DataControlField column in gvDocumentRecords.Columns)
            {
                if (column.HeaderText == "Ori Receive")
                {
                    column.Visible = true;

                    if (GLOBAL.CampaignReport == false)
                    {
                        column.Visible = false;
                        break;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = e.Row.DataItem as DataRowView;
                var Hyperlink = e.Row.FindControl("HyperLinkView");
                var UploadAction = e.Row.FindControl("lblUpload");
                var OriRecAction = e.Row.FindControl("lblOriRec");
                var BtnActionWrapper = e.Row.FindControl("ActionWrapper");
                Button ApproveAction = (Button)e.Row.FindControl("Button_Approve");
                Button CancelAction = (Button)e.Row.FindControl("Button_Cancel");
                Button RejectAction = (Button)e.Row.FindControl("Button_Reject");
                Button DeleteAction = (Button)e.Row.FindControl("Button_Delete");

                var DownloadWrapper = e.Row.FindControl("DownloadWrapper");

                UploadAction.Visible = false;
                ApproveAction.Visible = false;
                CancelAction.Visible = false;
                RejectAction.Visible = false;
                DeleteAction.Visible = false;
                OriRecAction.Visible = false;
                BtnActionWrapper.Visible = true;
                DownloadWrapper.Visible = false;//revert back to false
                if (row != null)
                {
                    //string RowColor = row["RowColor"].ToString();

                    ////e.Row.BackColor = System.Drawing.Color.FromName(RowColor);

                    if (row["FileName"].ToString() == "")
                    {
                        Hyperlink.Visible = false;
                    }

                    if (GLOBAL.CampaignReport == false)
                    {
                        DownloadWrapper.Visible = true;
                        if (Request.QueryString["download"] != "true")
                        {
                            UploadAction.Visible = true;
                            if (row["Status"].ToString() == DocumentStatus.Approved.ToString() || row["Status"].ToString() == DocumentStatus.Canceled.ToString())
                            {
                                UploadAction.Visible = false;
                            }
                        }

                        if (row["Status"].ToString() == "New")
                        {
                            DeleteAction.Visible = true;
                        }
                    }
                    else
                    {
                        OriRecAction.Visible = true;
                        RejectAction.Visible = true;
                        ApproveAction.Visible = true;
                        CancelAction.Visible = true;

                        if (row["Status"].ToString() == DocumentStatus.Approved.ToString() || row["Status"].ToString() == DocumentStatus.Canceled.ToString())
                        {
                            BtnActionWrapper.Visible = false;
                        }

                        if (row["Status"].ToString() == "New")
                        {
                            DeleteAction.Visible = true;
                        }
                    }

                }
            }
        }


        private bool SaveIntoMSBCampaignDetails(string ID, DocumentStatus Status)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            MSBCampaignDetailModel m = new MSBCampaignDetailModel();
            try
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();
                string SQL = "select t1.*, max(t2.TargetID) TargetID from campaign_document t1 left join campaign_targetheader t2 on t2.CampaignID = t1.CampaignID and t2.CustomerAccount = t1.CustomerAccount where t1.ID = @p0";

                MySqlCommand cmd1 = new MySqlCommand(SQL, conn);
                cmd1.Parameters.AddWithValue("@p0", ID);

                MySqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        long TargetResult = 0;
                        long.TryParse(reader.GetString("TargetID"), out TargetResult);
                        m.CampaignID = reader.GetString("CampaignID");
                        m.CustAccount = reader.GetString("CustomerAccount");
                        m.DateStart = DateTime.Parse(reader.GetString("CampaignStartDate"));
                        m.MSB_ComType = "Volume";
                        m.DateEnd = DateTime.Parse(reader.GetString("CampaignEndDate"));
                        m.GrowthDateEnd = DateTime.Parse(reader.GetString("CampaignEndDate"));
                        m.CustName = reader.GetString("WorkshopName");
                        m.TargetRefRecId = TargetResult;
                        m.PYSales = Double.Parse(reader.GetString("PastYearSales"));
                    }
                }

                conn.Close();

                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("MSB_CampaignDetail"))
                {
                    DynAx.TTSBegin();

                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}' && %1.{2} == '{3}'", "CampaignID", m.CampaignID, "CustAccount", m.CustAccount));
                    if (DynRec.Found)
                    {
                        if (Status == DocumentStatus.Approved)
                        {
                            Save_MSB_CampaignDetail_Parameter(DynRec, m);
                            DynRec.Call("Update");
                            logger.Info($"Updated MSB_CampaignDetail for CampaignID: {m.CampaignID}, CustAccount: {m.CustAccount}");
                        }
                        else if (Status == DocumentStatus.Canceled)
                        {

                            Cancel_MSB_CampaignDetail_Parameter(DynRec);
                            logger.Info($"Canceled MSB_CampaignDetail for CampaignID: {m.CampaignID}, CustAccount: {m.CustAccount}");
                        }
                    }
                    else
                    {
                        Save_MSB_CampaignDetail_Parameter(DynRec, m);
                        DynRec.Call("Insert");
                        logger.Info($"Inserted MSB_CampaignDetail for CampaignID: {m.CampaignID}, CustAccount: {m.CustAccount}");
                    }

                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }

                DynAx.Dispose();
                return true;
            }
            catch (Exception ER_CSF_11)
            {
                logger.Error($"Error saving MSB_CampaignDetail for ID: {ID}. Exception: {ER_CSF_11}");
                Function_Method.MsgBox("ER_CSF_11: " + ER_CSF_11.ToString(), this.Page, this);
                Function_Method.UserLog(ER_CSF_11.ToString());
                return false;
            }
        }

        private void Save_MSB_CampaignDetail_Parameter(AxaptaRecord DynRec, MSBCampaignDetailModel m)
        {
            if (m.CampaignID != "") DynRec.set_Field("CampaignID", m.CampaignID);
            if (m.CustAccount != "") DynRec.set_Field("CustAccount", m.CustAccount);
            DynRec.set_Field("DateStart", m.DateStart);
            //DynRec.set_Field("MSB_ComType", m.MSB_ComType);
            DynRec.set_Field("GrowthDateEnd", m.GrowthDateEnd);
            DynRec.set_Field("DateEnd", m.DateEnd);
            DynRec.set_Field("CustName", m.CustName);
            DynRec.set_Field("TargetRefRecId", m.TargetRefRecId);
            DynRec.set_Field("PYSales", m.PYSales);
            DynRec.set_Field("DocumentRecDate", DateTime.Now);

        }

        private void Cancel_MSB_CampaignDetail_Parameter(AxaptaRecord DynRec)
        {
            DynRec.set_Field("Cancelled", 1);
            DynRec.set_Field("DateCancelled", DateTime.Now);
            DynRec.set_Field("DateEnd", DateTime.Now);
            DynRec.set_Field("GrowthDateEnd", DateTime.Now);
            DynRec.set_Field("ContractEnd", 1);

            DynRec.Call("Update");
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            GridView gv = new GridView();

            GridView ExportGridView = new GridView();
            gv = gvDocumentRecords;


            string FileName = " VPPP Campaign Report( Status: " + ddlStatus.SelectedItem.Text + ") - " + DateTime.Now + ".xls";

            GlobalHelper.ExportFromGridView(gv, FileName);
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            //Allows for printing
        }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}