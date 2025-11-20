using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.UI.WebControls;
using static DotNet.CampaignModel.CampaignModel;

namespace DotNet.API
{
    public class APIProcessor
    {
        public static void NotifySalesmanCustomerReturnForm()
        {
            Function_Method.isWarranty = true;
            Function_Method.isPBM = false;
            Function_Method.isPoonshReport = false;

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            DateTime CurrentFourMonthsAgo = DateTime.Now.AddMonths(-4).Date;
            Dictionary<string, List<string>> CustomerDict = new Dictionary<string, List<string>>();
            conn.Open();

            string SQL = "select WorkshopName, CreatedDateTime, SalesmanID from campaign_document where CampaignStartDate >= @p0 and CampaignStartDate <= @p1 and (Status = 0 or Status = 4)";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            cmd.Parameters.AddWithValue("@p0", CurrentFourMonthsAgo);
            cmd.Parameters.AddWithValue("@p1", DateTime.Now.Date);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (!CustomerDict.ContainsKey(reader.GetString("SalesmanID")))
                {
                    CustomerDict[reader.GetString("SalesmanID")] = new List<string>();
                }

                if (!string.IsNullOrEmpty(reader.GetString("SalesmanID")))
                {
                    CustomerDict[reader.GetString("SalesmanID")].Add(reader.GetString("WorkshopName"));
                }
            }

            conn.Close();

            PrepareEmailContent(CustomerDict);
        }

        private static void PrepareEmailContent(Dictionary<string, List<string>> CustomerDict)
        {
            Axapta DynAx = GlobalAxapta();
            string ReportTo = "";
            string Salesman = "";
            string Content = "";
            string HOD = "";

            foreach (var item in CustomerDict.Keys)

            DynAx.Dispose();
        }

        protected static string CampaignEmailContent(string Customer)
        {
            var CustomerList = Customer.Split(',');
            string emailContent = "Please be informed that the following Customer did not return the VPPP Campaign Form yet:";
            for (var i = 0; i < CustomerList.Count(); i++)
            {
                emailContent += "\r\n\r\nApplicant Name " + (i + 1) + ": " + CustomerList[i];
            }
            //emailContent += "\r\n\r\n\r\nThank You.";

            return emailContent;
        }

        public static void GetEmailInfo(string receiver, string hod2, string hod3, string hod4, string groupHOD, string Sendmsg, string Subject)
        {

            //Sendmsg = GetGridviewData(gvReportGroup);

            string MailTo = receiver;//recipient
            Function_Method.SendMail("ppm_vppp", "Administrator", Subject, MailTo, hod4 + ",", Sendmsg);
            SalesReport.Flag = true;
        }


        public static HashSet<string> GetAvailableCompanyList()
        {
            DateTime CurrentDate = DateTime.Now.Date;
            DateTime EndDate = CurrentDate.AddYears(-1).Date;

            HashSet<string> FileList = new HashSet<string>();
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "select CampaignID from campaign_document where CreatedDateTime <= @p0 and CreatedDateTime >= @p1 group by CampaignID";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", CurrentDate);
            cmd.Parameters.AddWithValue("@p1", EndDate);

            MySqlDataReader reader = cmd.ExecuteReader();
            HashSet<string> AvailableCompanyList = new HashSet<string>();
            while ((bool)reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    AvailableCompanyList.Add(reader.GetValue(0).ToString());
                }
            }
            conn.Close();

            return AvailableCompanyList;
        }

        public static HashSet<string> GetRemoveFileID(HashSet<string> CompanyList)
        {
            HashSet<string> FileList = new HashSet<string>();
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "select FileID from campaign_document where CampaignID not in (" + string.Join(", ", CompanyList) + ")";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            while ((bool)reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    FileList.Add(reader.GetValue(0).ToString());
                }
            }
            conn.Close();

            return FileList;
        }

        public static void RemoveFile(HashSet<string> FileList)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "delete from file_library where FileID in (" + string.Join(",", FileList) + ")";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", "(" + string.Join(",", FileList) + ")");

            cmd.ExecuteNonQuery();
            //GlobalHelper.LogToDatabase("APIProcessor.RemoveFile", $"{SQL}", null, GLOBAL.user_id);
            conn.Close();
        }


        public static void FetchCampaignDataFromAxaptaDocument()
        {
            Axapta DynAx = GlobalAxapta();
            Function_Method.isWarranty = true;
            Function_Method.isPBM = false;
            Function_Method.isPoonshReport = false;

            ConvertAndInsert(DynAx);
            DynAx.Dispose();
        }

        public static Dictionary<string, string> GetAllRegisteredCustomer()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();
            DateTime MinDateTime = DateTime.Now.AddYears(-1).AddMonths(-6);
            Dictionary<string, string> CampaignCustomer = new Dictionary<string, string>();

            string SQL = "select CampaignID, CustomerAccount from campaign_document where CampaignEndDate >= @p0 and Status != @p1";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", MinDateTime);
            cmd.Parameters.AddWithValue("@p1", DocumentStatus.Canceled);

            MySqlDataReader reader = cmd.ExecuteReader();

            while ((bool)reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    if (!CampaignCustomer.ContainsKey(reader.GetValue(1).ToString() + "-" + reader.GetValue(0).ToString()))
                    {
                        CampaignCustomer.Add(reader.GetValue(1).ToString() + "-" + reader.GetValue(0).ToString(), reader.GetValue(0).ToString());
                    }

                }
            }

            conn.Close();

            return CampaignCustomer;
        }
        private static void ConvertAndInsert(Axapta DynAx)
        {
            HashSet<string> m = ConvertIntoDocumentModel(DynAx);
            string MailSubject = "Renewal VPPP Campaign Customers";
            string HOD = "";
            string Salesman = "";
            string ReportTo = "";

            string msgContent = "VPPP Renewal customer for month " + DateTime.Now.ToString(GLOBAL.gDisplayDateFormat) + " is ready to download. Please keep track.";

            foreach (var item in m)
            {
                int EmplTable = DynAx.GetTableIdWithLock("Empltable");
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", EmplTable);

                int EmplID = DynAx.GetFieldIdWithLock(EmplTable, "EmplId");
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", EmplID);
                qbr.Call("value", item);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                if ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", EmplTable);
                    ReportTo = DynRec.get_Field("ReportTo").ToString();
                    Salesman = DynRec.get_Field("LF_EmpEmailID").ToString();

                    AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

                    var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", EmplID);
                    qbr1.Call("value", ReportTo);

                    AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
                    if ((bool)axQueryRun1.Call("next"))
                    {
                        AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);

                        HOD = DynRec1.get_Field("LF_EmpEmailID").ToString();
                    }

                    DynRec.Dispose();

                    axQuery1.Dispose();
                    axQueryDataSource1.Dispose();
                    axQueryRun1.Dispose();
                }
                axQuery.Dispose();
                axQueryDataSource.Dispose();
                axQueryRun.Dispose();

                DynAx.Dispose();

                if (!CheckEmailSent("Renewal Vppp - " + Salesman,"Month"))
                {
                    InsertSQLEmail("Renewal Vppp - " + Salesman, true);
                    GetEmailInfo(Salesman, "", "", HOD, "", msgContent, MailSubject);
                }
                
                //GetEmailInfo(item, );
            }
        }

        public static void Insert(CompiledModel m)
        {
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
                Function_Method.WriteLog(ex.ToString(), DateTime.Now.ToString(GLOBAL.gDisplayDateFormat) + " - Migration of VPPP Campaign");
            }
        }

        public static HashSet<string> ConvertIntoDocumentModel(Axapta DynAx)
        {
            HashSet<string> SalesmanList = new HashSet<string>();
            CompiledModel m = new CompiledModel();

            var CampaignCustomer = GetAllRegisteredCustomer();
            Dictionary<string, string> CampaignList = new Dictionary<string, string>();

            int CampaignTable = DynAx.GetTableIdWithLock("MSB_CampaignMaster");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CampaignTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CampaignTable);
                DateTime CurrentDateTime = DateTime.Now;
                DateTime CampaignEndDateTime = DateTime.Parse(DynRec.get_Field("EndDate").ToString());
                string CampaignID = DynRec.get_Field("MSB_CampaignID").ToString();

                if (CurrentDateTime < CampaignEndDateTime)
                {
                    CampaignList.Add(CampaignID, DynRec.get_Field("Terms").ToString());
                }
                DynRec.Dispose();
            }

            axQuery.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun.Dispose();

            foreach (var campaign in CampaignList)
            {
                string CampaignID = campaign.Key;
                string CampaignTerms = campaign.Value;
                int SequenceNo = 1;

                List<CustomerTargetModel> CustomerList = new List<CustomerTargetModel>();
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();
                string SQL = "select max(SequenceNo) SequenceNo from campaign_document where CampaignID = @p0";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                cmd.Parameters.AddWithValue("@p0", CampaignID);

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

                int MSB_CampaignDetail = DynAx.GetTableIdWithLock("MSB_CampaignDetail");

                AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", MSB_CampaignDetail);

                int CampaignIDField = DynAx.GetFieldId(MSB_CampaignDetail, "CampaignID");

                var qbr = (AxaptaObject)axQueryDataSource3.Call("addRange", CampaignIDField);

                qbr.Call("value", CampaignID);

                int ContractEndField = DynAx.GetFieldId(MSB_CampaignDetail, "ContractEnd");
                var qbr_1 = (AxaptaObject)axQueryDataSource3.Call("addRange", ContractEndField);

                qbr_1.Call("value", "0");
                AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

                while ((bool)axQueryRun3.Call("next"))
                {
                    AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", MSB_CampaignDetail);

                    CustomerTargetModel ctm = new CustomerTargetModel();
                    ctm.CustomerAccount = DynRec3.get_Field("CustAccount").ToString();
                    string TargetRefRecId = DynRec3.get_Field("TargetRefRecId").ToString();

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

                        ctm.CampaignTarget = double.Parse(DynRec6.get_Field("Target").ToString());
                        ctm.SecondLevelTarget = double.Parse(DynRec6.get_Field("SecondLevelTarget").ToString());

                        DynRec6.Dispose();
                    }

                    CustomerList.Add(ctm);

                    DynRec3.Dispose();
                }

                axQuery3.Dispose();
                axQueryDataSource3.Dispose();
                axQueryRun3.Dispose();

                foreach (var customer in CustomerList)
                {
                    List<double> TargetList = new List<double>();


                    int IsUserExist = CampaignCustomer.Where(x => x.Key.Split('-')[0].ToString().Trim() == customer.CustomerAccount && x.Value == CampaignID).ToList().Count();
                    if (IsUserExist < 1)
                    {
                        string CustName = "";
                        string SalesmanID = "";
                        string SalesmanName = "";
                        string UserID = "";
                        string AccountNum = customer.CustomerAccount;

                        int CustTable = DynAx.GetTableIdWithLock("CustTable");

                        AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

                        int AccountNumField = DynAx.GetFieldId(CustTable, "AccountNum");
                        var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", AccountNumField);

                        qbr4.Call("value", AccountNum);
                        AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);

                        if ((bool)axQueryRun4.Call("next"))
                        {
                            AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                            CustName = DynRec4.get_Field("Name").ToString();
                            SalesmanID = DynRec4.get_Field("EmplId").ToString();

                            DynRec4.Dispose();
                        }

                        axQuery4.Dispose();
                        axQueryDataSource4.Dispose();
                        axQueryRun4.Dispose();

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
                            SalesmanName = DynRec5.get_Field("DEL_Name").ToString();
                            string EmplEmail = DynRec5.get_Field("LF_EmpEmailID").ToString();

                            DynRec5.Dispose();

                            UserID = EmplEmail.Split('@')[0].ToString();
                        }
                        axQuery5.Dispose();
                        axQueryDataSource5.Dispose();
                        axQueryRun5.Dispose();

                        CampaignDocumentModel DocumentModel = new CampaignDocumentModel();

                        DocumentModel.CampaignID = CampaignID;
                        DocumentModel.SequenceNo = SequenceNo;
                        DocumentModel.CustomerAccount = customer.CustomerAccount;
                        DocumentModel.WorkshopName = CustName;
                        DocumentModel.Salesman = SalesmanName;
                        DocumentModel.SalesmanID = SalesmanID;
                        DocumentModel.CampaignTerms = CampaignTerms;
                        //DocumentModel.CampaignStartDate = DateTime.Parse(DynRec.get_Field("StartDate").ToString());
                        //DocumentModel.CampaignEndDate = DateTime.Parse(DynRec.get_Field("EndDate").ToString());
                        DocumentModel.CreatedDateTime = DateTime.Now;
                        DocumentModel.CreatedBy = UserID;
                        DocumentModel.FileID = 0;

                        TargetList.Add(customer.CampaignTarget);
                        TargetList.Add(customer.SecondLevelTarget);

                        int CampaignDetails = DynAx.GetTableIdWithLock("MSB_CampaignDetail");

                        AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", CampaignDetails);

                        int CampaignIDField2 = DynAx.GetFieldId(CampaignDetails, "CampaignID");
                        var qbr2_1 = (AxaptaObject)axQueryDataSource2.Call("addRange", CampaignIDField2);

                        qbr2_1.Call("value", CampaignID);

                        int CustAccountField2 = DynAx.GetFieldId(CampaignDetails, "CustAccount");
                        var qbr2_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", CustAccountField2);

                        qbr2_2.Call("value", customer.CustomerAccount);

                        int ContractEndField1 = DynAx.GetFieldId(CampaignDetails, "ContractEnd");
                        var qbr2_3 = (AxaptaObject)axQueryDataSource2.Call("addRange", ContractEndField1);

                        qbr2_3.Call("value", "0");
                        AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

                        if ((bool)axQueryRun2.Call("next"))
                        {
                            AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CampaignDetails);

                            DocumentModel.CampaignStartDate = DateTime.Parse(DynRec2.get_Field("DateStart").ToString());
                            DocumentModel.CampaignEndDate = DateTime.Parse(DynRec2.get_Field("DateEnd").ToString());
                            DocumentModel.PastYearSales = Double.Parse(Double.Parse(DynRec2.get_Field("PYSales").ToString()).ToString("N2"));

                            DateTime DocumentDateTime = DateTime.MinValue;
                            DocumentStatus Status = DocumentStatus.Renewal;
                            string ModifiedBy = "";
                            DateTime ModifiedDateTime = DateTime.MinValue;
                            if (DynRec2.get_Field("DocumentRecDate").ToString() != "1/1/1900 12:00:00 AM")
                            {
                                DocumentDateTime = DateTime.Parse(DynRec2.get_Field("DocumentRecDate").ToString());
                                Status = DocumentStatus.Approved;
                                ModifiedBy = DynRec2.get_Field("modifiedBy").ToString();
                                ModifiedDateTime = DateTime.Parse(DynRec2.get_Field("ModifiedDateTime").ToString());
                            }
                            if (DynRec2.get_Field("Cancelled").ToString() == "1")
                            {
                                Status = DocumentStatus.Canceled;
                                ModifiedBy = DynRec2.get_Field("modifiedBy").ToString();
                                ModifiedDateTime = DateTime.Parse(DynRec2.get_Field("ModifiedDateTime").ToString());
                            }
                            DocumentModel.Status = Status;
                            DocumentModel.UpdatedDateTime = ModifiedDateTime;
                            DocumentModel.UpdatedBy = ModifiedBy;
                            DocumentModel.DocumentRecDate = DocumentDateTime;

                            DynRec2.Dispose();
                        }

                        axQuery2.Dispose();
                        axQueryDataSource2.Dispose();
                        axQueryRun2.Dispose();

                        m.cdm = DocumentModel;
                        ConvertIntoTargetHeaderModelByCampaignIDAndTargets(DynAx, m, CampaignID, customer.CustomerAccount, TargetList);

                        Insert(m);
                        SalesmanList.Add(m.cdm.SalesmanID);
                        SequenceNo++;
                    }
                }
            }
            return SalesmanList;
        }

        private static string GetCurrentDomainPath()
        {
            // First, try the fallback using Environment.UserDomainName
            string primaryDnsSuffix = Environment.UserDomainName;

            if (!string.IsNullOrEmpty(primaryDnsSuffix) && !string.Equals(primaryDnsSuffix, Environment.MachineName, StringComparison.OrdinalIgnoreCase))
            {
                Function_Method.AddLog("Using fallback for domain path.");
                return "LDAP://" + primaryDnsSuffix;  // e.g., "LDAP://lionpb.com.my"
            }
            // If fallback fails, proceed to try the LDAP query
            try
            {
                using (DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE"))
                {
                    if (de.Properties["defaultNamingContext"].Count > 0)
                    {
                        Function_Method.AddLog("Successfully retrieved domain path via LDAP.");
                        return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
                    }
                    else
                    {
                        Function_Method.AddLog("defaultNamingContext property not found.");
                        return null; // Or another appropriate value indicating failure
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException comEx)
            {
                Function_Method.AddLog($"COMException: {comEx.Message} (Error Code: {comEx.ErrorCode})");
                return null; // Or another appropriate value indicating failure
            }
            catch (Exception ex)
            {
                Function_Method.AddLog($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return null; // Or another appropriate value indicating failure
            }
            // If everything fails, return null
            Function_Method.AddLog("Failed to get domain path via both fallback and LDAP.");
            return null;
        }

        private static DirectorySearcher BuildUserSearcher(DirectoryEntry de, string[] F, int data_count)
        {
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);
            for (int k = 0; k < data_count; k++)
            {
                ds.PropertiesToLoad.Add(F[k]);
            }
            //ds.PropertiesToLoad.Add("distinguishedName");//Distinguished Name
            return ds;
        }

        private static void ConvertIntoTargetHeaderModelByCampaignIDAndTargets(Axapta DynAx, CompiledModel m, string CampaignID, string CustAcc, List<double> Targets)
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
            axQuery.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun.Dispose();

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

            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            axQueryRun1.Dispose();

            m.cthm = HeaderModel;
            m.ctpm = PercentModel;
        }


        public static Axapta GlobalAxapta()
        {
            var globalAxapta = new Axapta();
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;

                // Use a lock to ensure thread safety
                lock (globalAxapta)
                {
                    // Log in only if not already logged in
                    globalAxapta.LogonAs(GLOBAL.AdminID, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    //var session = globalAxapta.Session();
                }
            }
            catch (Exception ex)
            {
            }

            return globalAxapta;
        }

        public static void InsertSQLEmail(string emplid, Boolean Flag)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "insert into email_tbl(emp_ID,is_Send,send_DateTime) values (@v1,@v2,@v3)";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            MySqlParameter _v1 = new MySqlParameter("@v1", MySqlDbType.VarChar, 0);
            _v1.Value = emplid;
            cmd.Parameters.Add(_v1);

            MySqlParameter _v2 = new MySqlParameter("@v2", MySqlDbType.VarChar, 0);
            _v2.Value = Flag.ToString();
            cmd.Parameters.Add(_v2);

            MySqlParameter _v3 = new MySqlParameter("@v3", MySqlDbType.VarChar, 0);
            _v3.Value = DateTime.Now.ToString();
            cmd.Parameters.Add(_v3);

            conn.Open();
            cmd.ExecuteNonQuery();

            conn.Close();
            cmd.Parameters.Clear();
            conn.Dispose();
        }

        public static Boolean CheckEmailSent(string emp_ID, string filterBy)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();
            bool IsExist = false;
            #region DateTime Now
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth;
            DateTime lastDayOfMonth;

            firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            lastDayOfMonth = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);

            #endregion

            string SQL = "SELECT * FROM `email_tbl` WHERE `emp_ID` = @p0 ";
            switch (filterBy)
            {
                case "Month":
                    SQL += " AND DATE(STR_TO_DATE(`send_DateTime`, '%d/%m/%Y %h:%i:%s %p')) >= @firstDayofMonth " +
                           " AND DATE(STR_TO_DATE(`send_DateTime`, '%d/%m/%Y %h:%i:%s %p')) <= @lastDayofMonth ";
                    break;
                case "Day":
                    SQL += " AND DATE(STR_TO_DATE(`send_DateTime`, '%d/%m/%Y %h:%i:%s %p')) = CURDATE()";
                    break;
            }

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", emp_ID);

            switch (filterBy)
            {
                case "Month":
                    cmd.Parameters.AddWithValue("@firstDayofMonth", firstDayOfMonth.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@lastDayofMonth", lastDayOfMonth.ToString("yyyy-MM-dd"));
                    break;
                //case "Day":
                //    cmd.Parameters.AddWithValue("@p1", DateTime.Now.ToString("yyyy-MM-dd"));
                //    break;
            }

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
    }
}