using DotNet.SFAModel;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.DynamicData;
using System.Web.Http.Results;
using System.Web.SessionState;
using System.Web.UI.WebControls;



namespace DotNet
{
    public class Redemption_Get_Details
    {
        public static RedemptionModel get_redemption(Axapta DynAx, string redemptionID)
        {
            RedemptionModel m = new RedemptionModel();
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");
            string CustAcc = ""; string CustPhone = ""; string CustContact = "";
            string CustAddr = ""; string benefitName = ""; string benefitIc = "";
            string benefitTaxNo = ""; string reason = ""; int interestWaiver = 0;
            string time = ""; string date = ""; string ap = ""; string lpDate = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                m.CustAcc = DynRec.get_Field("CustAccount").ToString();
                m.CustPhone = DynRec.get_Field("CustPhone").ToString();
                m.CustContact = DynRec.get_Field("CustContact").ToString();
                m.CustAddr = DynRec.get_Field("Del_Addr").ToString();
                m.benefitName = DynRec.get_Field("Benefit_Name").ToString();
                m.benefitIc = DynRec.get_Field("Benefit_IC").ToString();
                m.benefitTaxNo = DynRec.get_Field("Benefit_Tax_No").ToString();
                m.reason = DynRec.get_Field("Remarks_AdminMgr").ToString();
                m.interestWaiver = Convert.ToInt16(DynRec.get_Field("InterestWaiver"));
                m.time = DynRec.get_Field("SO_InvDate2").ToString();
                m.date = DynRec.get_Field("SO_InvDate1").ToString();
                m.ap = DynRec.get_Field("SO_Inv1").ToString();
                m.lpDate = DynRec.get_Field("TP_DocNo").ToString();
                m.CreatedAP = DynRec.get_Field("CreatedAP").ToString();
                m.CreatedLP = DynRec.get_Field("CreatedLP").ToString();

            }
            return m;
        }

        // Jerry 2024-11-14 Get customer name
        public static string get_accName(Axapta DynAx, string accNo)
        {
            string TableName = "CustTable";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "AccountNum");
            string AccName = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//AccountNum
            qbr1.Call("value", accNo);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                AccName = DynRec.get_Field("Name").ToString();

            }
            return AccName;
        }

        public static List<ListItem> get_CNDNPurpose(Axapta DynAx)
        {
            string TableName = "LF_CNDNPurpose";
            List<ListItem> List_lf_cndnPurpose = new List<ListItem>();

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Code");
            string Code = ""; string Name = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
            qbr1.Call("value", "*C301*");

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                Code = DynRec.get_Field("Code").ToString();
                Name = DynRec.get_Field("Name").ToString();
                List_lf_cndnPurpose.Add(new ListItem(Code + " " + Name));
                DynRec.Dispose();
            }
            return List_lf_cndnPurpose;
        }

        public static string getEmpName(Axapta DynAx, string temp_EmplId)
        {
            if (temp_EmplId == "")
            {
                return "";
            }

            string temp_EmpName = "";
            //int EmplTable = 103;
            int tableId = DynAx.GetTableId("EmplTable");
            int fieldId = DynAx.GetFieldId(tableId, "EmplId");

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", tableId);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", fieldId);//temp_EmplId
            qbr2.Call("value", temp_EmplId);
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", tableId);
                temp_EmpName = DynRec2.get_Field("Del_Name").ToString();
                DynRec2.Dispose();
            }
            return temp_EmpName;
        }

        public static Tuple<string, string, string, string, string, string, string> get_rd_CreditInfo(Axapta DynAx, string custAcc)
        {
            int CustTable = 77;
            string EmplName = ""; string openAccDt = "";
            string CustName = ""; string CustPhone = "";
            string CustClass = ""; string custType = ""; string incorpDt = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//CustAcc
            qbr1.Call("value", custAcc);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);

                // Jerry 2025-02-19 Get the correct salesman name
                //EmplName = DynRec.get_Field("EmplName").ToString();
                string temp_EmplId = DynRec.get_Field("EmplId").ToString();
                EmplName = getEmpName(DynAx, temp_EmplId);
                // Jerry 2025-02-19 Get the correct salesman name - END

                CustName = DynRec.get_Field("Name").ToString();
                CustPhone = DynRec.get_Field("Phone").ToString();
                CustClass = DynRec.get_Field("CustomerClass").ToString();
                custType = DynRec.get_Field("CustomerType").ToString();
                incorpDt = Convert.ToDateTime(DynRec.get_Field("IncorpDate")).ToString("dd/MM/yyyy");
                openAccDt = Convert.ToDateTime(DynRec.get_Field("OpeningAccDate")).ToString("dd/MM/yyyy");
            }
            return new Tuple<string, string, string, string, string, string, string>
                (CustName, CustPhone, CustClass, custType, incorpDt, openAccDt, EmplName);
        }

        public static Tuple<string, string, string, string, string> get_rd_CreditInfo2(Axapta DynAx, string custAcc)
        {
            int CustTable = 77;
            string creditLimit = ""; string custMainGroup = ""; string Name = ""; string Email = "";
            string paymentId = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//CustAcc
            qbr1.Call("value", custAcc);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);

                creditLimit = DynRec.get_Field("CreditMax").ToString();
                custMainGroup = DynRec.get_Field("CustomerMainGroup").ToString();
                Name = DynRec.get_Field("Name").ToString();
                Email = DynRec.get_Field("Email").ToString();
                paymentId = DynRec.get_Field("PaymTermId").ToString();
            }
            return new Tuple<string, string, string, string, string>
                (creditLimit, custMainGroup, Name, Email, paymentId);
        }

        // Jerry 2024-12-10 Get docstatus for redemption
        public static int get_docstatus(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");

            int docstatus = 0;


            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                // Retrieve the DOCSTATUS field
                object docstatusField = DynRec.get_Field("DOCSTATUS");

                // Attempt to convert docstatus to an integer safely
                if (docstatusField is int)
                {
                    docstatus = (int)docstatusField;
                }
                else if (int.TryParse(docstatusField?.ToString(), out docstatus))
                {
                    // Successfully converted to int
                }
                else
                {
                    docstatus = 2;
                }

            }
            return docstatus;
        }
        // Jerry 2024-12-10 Get docstatus for redemption - END

        public static Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string>> get_redempStat(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");
            string ProcessStat = ""; string Del_To = ""; string Del_Person = "";
            string Benefit_Bank = ""; string Remarks_Sales = ""; string Remarks_HOD = "";
            string Remarks_Admin = ""; string ProcessDate = ""; string Remarks_AdminMgr = ""; string Remarks_OperationMgr = "";
            string Remarks_CreditControlMgr = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                ProcessStat = DynRec.get_Field("ProcessStatus").ToString();
                Del_To = DynRec.get_Field("Del_To").ToString();
                Del_Person = DynRec.get_Field("Del_Person").ToString();
                Benefit_Bank = DynRec.get_Field("Benefit_Bank").ToString();
                Remarks_Sales = DynRec.get_Field("Remarks_Sales").ToString();
                Remarks_HOD = DynRec.get_Field("Remarks_HOD").ToString();
                Remarks_Admin = DynRec.get_Field("Remarks_Admin").ToString();
                Remarks_AdminMgr = DynRec.get_Field("Remarks_AdminMgr").ToString();
                Remarks_OperationMgr = DynRec.get_Field("OperationMgr_Reason").ToString();
                Remarks_CreditControlMgr = DynRec.get_Field("Remarks_CreditControlMgr").ToString();
                ProcessDate = Convert.ToDateTime(DynRec.get_Field("ProcessDate")).ToString("dd/MM/yyyy");
            }
            return new Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string>>
                (ProcessStat, Del_To, Del_Person, Benefit_Bank, Remarks_Sales, Remarks_HOD, Remarks_Admin,
                new Tuple<string, string, string, string>(ProcessDate, Remarks_AdminMgr, Remarks_OperationMgr, Remarks_CreditControlMgr));
        }

        public static Tuple<string, string, string, string, string, string, string, 
            Tuple<string, string, string>> get_redempStat2(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");
            string Header = ""; string cnreason = ""; string cntype = "";
            string ledgeracc = ""; string pointctgry = ""; string specialApprove = ""; string specialGM = "";
            string remark_hod2 = ""; string remark_hod3 = ""; string cndnPurpose = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                Header = DynRec.get_Field("Header").ToString();
                cnreason = DynRec.get_Field("CNReason").ToString();
                cntype = DynRec.get_Field("CNType").ToString();
                ledgeracc = DynRec.get_Field("LedgerAcc").ToString();
                pointctgry = DynRec.get_Field("Point_Ctgry_New").ToString();
                specialApprove = DynRec.get_Field("SpecialApprove").ToString();
                specialGM = DynRec.get_Field("SpecialGM").ToString();
                remark_hod2 = DynRec.get_Field("Remarks_HOD2").ToString();
                remark_hod3 = DynRec.get_Field("Remarks_HOD3").ToString();
                cndnPurpose = DynRec.get_Field("PurposeCode").ToString();
            }
            return new Tuple<string, string, string, string, string, string, string, Tuple<string, string, string>>
                (Header, cnreason, cntype, ledgeracc, pointctgry, specialApprove, specialGM,
                new Tuple<string, string, string>(remark_hod2, remark_hod3, cndnPurpose));
        }

        public static Tuple<string, string, string, string, string, string, double, Tuple<double, double, double, double, double>> get_gridViewData(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");
            string Item1 = ""; string Item2 = ""; string Item3 = "";
            string Qty1 = ""; string Qty2 = ""; string Qty3 = "";
            double Amt1 = 0; double Amt2 = 0; double Amt3 = 0;
            double TAmt1 = 0; double TAmt2 = 0; double TAmt3 = 0;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                Item1 = DynRec.get_Field("Item1").ToString();
                Item2 = DynRec.get_Field("Item2").ToString();
                Item3 = DynRec.get_Field("Item3").ToString();
                Qty1 = DynRec.get_Field("Qty1").ToString();
                Qty2 = DynRec.get_Field("Qty2").ToString();
                Qty3 = DynRec.get_Field("Qty3").ToString();
                Amt1 = Convert.ToDouble(DynRec.get_Field("Amt1"));
                Amt2 = Convert.ToDouble(DynRec.get_Field("Amt2"));
                Amt3 = Convert.ToDouble(DynRec.get_Field("Amt3"));
                TAmt1 = Convert.ToDouble(DynRec.get_Field("TAmt1"));
                TAmt2 = Convert.ToDouble(DynRec.get_Field("TAmt2"));
                TAmt3 = Convert.ToDouble(DynRec.get_Field("TAmt3"));
            }
            return new Tuple<string, string, string, string, string, string, double, Tuple<double, double, double, double, double>>
                (Item1, Item2, Item3, Qty1, Qty2, Qty3, Amt1, new Tuple<double, double, double, double, double>(Amt2, Amt3, TAmt1, TAmt2, TAmt3));
        }

        public static Tuple<double, double, double, double, double, string> get_gridViewData2(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");
            double PtsVal1 = 0; double PtsVal2 = 0; double PtsVal3 = 0;
            double Rdempt_Amt = 0; double Rdempt_Point = 0;
            string hodLvl = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                PtsVal1 = Convert.ToDouble(DynRec.get_Field("PtsVal1"));
                PtsVal2 = Convert.ToDouble(DynRec.get_Field("PtsVal2"));
                PtsVal3 = Convert.ToDouble(DynRec.get_Field("PtsVal3"));
                Rdempt_Amt = Convert.ToDouble(DynRec.get_Field("Rdempt_Amt"));
                Rdempt_Point = Convert.ToDouble(DynRec.get_Field("Rdempt_Point"));
                hodLvl = DynRec.get_Field("HodLevel").ToString();
            }
            return new Tuple<double, double, double, double, double, string>
                (PtsVal1, PtsVal2, PtsVal3, Rdempt_Amt, Rdempt_Point, hodLvl);
        }

        public static Tuple<string, string, string, string, string, string, 
            Tuple<string, string, string, string>> get_gridViewDataInvoice(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");

            string LF_Item1_InvNo = ""; string LF_Item2_InvNo = ""; string LF_Item3_InvNo = "";
            string LF_Item_Code1 = ""; string LF_Item_Code2 = ""; string LF_Item_Code3 = "";
            string LF_Item1_InvDate = ""; string LF_Item2_InvDate = ""; string LF_Item3_InvDate = "";
            string createdDt = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                LF_Item1_InvNo = DynRec.get_Field("LF_Item1_InvNo").ToString();
                LF_Item2_InvNo = DynRec.get_Field("LF_Item2_InvNo").ToString();
                LF_Item3_InvNo = DynRec.get_Field("LF_Item3_InvNo").ToString();
                LF_Item_Code1 = DynRec.get_Field("ItemCode1").ToString();
                LF_Item_Code2 = DynRec.get_Field("ItemCode2").ToString();
                LF_Item_Code3 = DynRec.get_Field("ItemCode3").ToString();
                createdDt = DynRec.get_Field("CreatedDateTime").ToString();

                if (DynRec.get_Field("LF_Item1_InvDate").ToString() != "1/1/1900 12:00:00 AM")
                {
                    LF_Item1_InvDate = Convert.ToDateTime(DynRec.get_Field("LF_Item1_InvDate")).ToString("dd/MM/yyyy");
                }
                if (DynRec.get_Field("LF_Item2_InvDate").ToString() != "1/1/1900 12:00:00 AM")
                {
                    LF_Item2_InvDate = Convert.ToDateTime(DynRec.get_Field("LF_Item2_InvDate")).ToString("dd/MM/yyyy");
                }
                if (DynRec.get_Field("LF_Item3_InvDate").ToString() != "1/1/1900 12:00:00 AM")
                {
                    LF_Item3_InvDate = Convert.ToDateTime(DynRec.get_Field("LF_Item3_InvDate")).ToString("dd/MM/yyyy");
                }
            }
            return new Tuple<string, string, string, string, string, string, Tuple<string, string, string, string>>
                (LF_Item1_InvNo, LF_Item2_InvNo, LF_Item3_InvNo, LF_Item1_InvDate, LF_Item2_InvDate, LF_Item3_InvDate,
                new Tuple<string, string, string, string>(LF_Item_Code1, LF_Item_Code2, LF_Item_Code3, createdDt));
        }

        public static List<ListItem> getAxUser(Axapta DynAx)
        {
            List<ListItem> List_Salesman = new List<ListItem>();
            string getSalesmanId = "";
            //string getSalesmanName = "";

            int SysUserInfo = 956;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", SysUserInfo);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            List_Salesman.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", SysUserInfo);
                getSalesmanId = DynRec1.get_Field("Id").ToString();

                List_Salesman.Add(new ListItem(getSalesmanId));
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            axQueryRun1.Dispose();
            return List_Salesman;
        }

        public static Tuple<string[], string[], string[], string[], string[], string[], Tuple<string[], string[], string[]>> RedempApprovalInUse(Axapta DynAx)//filter InUse
        {
            AxaptaObject axQuery;
            AxaptaObject axQueryDataSource;

            string TableName = "LF_WebRedemp_AppGrp";
            int tableId = DynAx.GetTableId(TableName);

            axQuery = DynAx.CreateAxaptaObject("Query");
            axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);
            int count = 0;
            string[] hod1 = new string[3]; string[] hod2 = new string[3]; string[] hod3 = new string[3];
            string[] salesAdmin1 = new string[3]; string[] salesAdmin2 = new string[3]; string[] salesAdmin3 = new string[10];
            string[] salesAdmMng1 = new string[3]; string[] salesAdmMng2 = new string[3]; string[] salesAdmMng3 = new string[10];

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                salesAdmin1[count] = DynRec.get_Field("SalesAdmin1").ToString();
                salesAdmin2[count] = DynRec.get_Field("SalesAdmin2").ToString();
                salesAdmin3[count] = DynRec.get_Field("SalesAdmin3").ToString();

                salesAdmMng1[count] = DynRec.get_Field("SalesAdminManager1").ToString();
                salesAdmMng2[count] = DynRec.get_Field("SalesAdminManager2").ToString();
                salesAdmMng3[count] = DynRec.get_Field("SalesAdminManager3").ToString();
                count++;
                DynRec.Dispose();
            }
            
            return new Tuple<string[], string[], string[], string[], string[], string[], Tuple<string[], string[], string[]>>
                (hod1, hod2, hod3, salesAdmin1, salesAdmin2, salesAdmin3, Tuple.Create(salesAdmMng1, salesAdmMng2, salesAdmMng3));
        }

        public static Tuple<string, string, string, string> RedempApprovalInUseManagerGM(Axapta DynAx)//filter InUse
        {
            AxaptaObject axQuery;
            AxaptaObject axQueryDataSource;

            string TableName = "LF_WebRedemp_AppGrp";

            int tableId = DynAx.GetTableId(TableName);

            axQuery = DynAx.CreateAxaptaObject("Query");
            axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);
            string om1 = ""; string om2 = ""; string gm1 = "";
            string gm2 = "";

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                om1 = DynRec.get_Field("OperationManager1").ToString();
                om2 = DynRec.get_Field("OperationManager2").ToString();

                gm1 = DynRec.get_Field("GeneralManager1").ToString();
                gm2 = DynRec.get_Field("GeneralManager2").ToString();

                DynRec.Dispose();
            }
            return new Tuple<string, string, string, string>(om1, om2, gm1, gm2);
        }

        public static Tuple<string, string, string, string, string, string, string> get_gridViewDataAdmin(Axapta DynAx, string redemptionID)
        {
            string TableName = "LF_WebRedemp";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "Rdemt_ID");
            string HODAnP = ""; string TP_DocNo = ""; string SO_Inv1 = "";
            string SO_InvDate1 = ""; string SO_InvDate2 = ""; string rdempItemType = "";
            string NextApprover = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//Rdemt_ID
            qbr1.Call("value", redemptionID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                HODAnP = DynRec.get_Field("HODAnP").ToString();
                TP_DocNo = DynRec.get_Field("TP_DocNo").ToString();
                SO_Inv1 = DynRec.get_Field("SO_Inv1").ToString();
                SO_InvDate1 = DynRec.get_Field("SO_InvDate1").ToString();
                SO_InvDate2 = DynRec.get_Field("SO_InvDate2").ToString();
                rdempItemType = DynRec.get_Field("RedemptItemType").ToString();
                NextApprover = DynRec.get_Field("NextApprover").ToString();
                NextApprover = NextApprover.Replace('\u00A0', ' ');
            }
            return new Tuple<string, string, string, string, string, string, string>
                (HODAnP, TP_DocNo, SO_Inv1, SO_InvDate1, SO_InvDate2, rdempItemType, NextApprover);
        }

        public static Tuple<string, string, string, string> getInvoice(Axapta DynAx, string AccNo, string invoiceID)
        {
            string invoice = ""; string salesmanID = ""; string InvoiceDate = ""; string DueDate = "";
            int CustTrans = 78;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 4);//account
            qbr.Call("value", AccNo);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);//invoice
            qbr1.Call("value", invoiceID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);

                invoice = DynRec.get_Field("Invoice").ToString();
                salesmanID = DynRec.get_Field("SalesResponsible").ToString();
                InvoiceDate = DynRec.get_Field("TransDate").ToString();
                DueDate = DynRec.get_Field("DueDate").ToString();
            }

            return new Tuple<string, string, string, string>(invoice, salesmanID, InvoiceDate, DueDate);
        }

        public static string getAxUserEmail(Axapta DynAx, string id)
        {
            int SysUserInfo = 956;
            string userEmail = "";

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", SysUserInfo);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 2);//id
            qbr.Call("value", id);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", SysUserInfo);
                userEmail = DynRec1.get_Field("Email").ToString();

                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun1.Dispose();
            return userEmail;
        }

        public static Tuple<string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string>> getRedempApprovalEmail()//filter InUse
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            AxaptaObject axQuery;
            AxaptaObject axQueryDataSource;
            string TableName = "LF_WebRedemp_AppGrp";

            int tableId = DynAx.GetTableId(TableName);

            string hod1 = ""; string hod2 = ""; string hod3 = "";
            string salesAdmin1 = ""; string salesAdmin2 = ""; string salesAdmin3 = "";
            string salesAdmMng1 = ""; string salesAdmMng2 = ""; string salesAdmMng3 = "";
            string OpMngr1 = ""; string OpMngr2 = ""; string GM1 = ""; string GM2 = "";

            axQuery = DynAx.CreateAxaptaObject("Query");
            axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                salesAdmin1 = getAxUserEmail(DynAx, DynRec.get_Field("SalesAdmin1").ToString());
                salesAdmin2 = getAxUserEmail(DynAx, DynRec.get_Field("SalesAdmin2").ToString());
                salesAdmin3 = getAxUserEmail(DynAx, DynRec.get_Field("SalesAdmin3").ToString());

                salesAdmMng1 = getAxUserEmail(DynAx, DynRec.get_Field("SalesAdminManager1").ToString());
                salesAdmMng2 = getAxUserEmail(DynAx, DynRec.get_Field("SalesAdminManager2").ToString());
                salesAdmMng3 = getAxUserEmail(DynAx, DynRec.get_Field("SalesAdminManager3").ToString());
                OpMngr1 = getAxUserEmail(DynAx, DynRec.get_Field("OperationManager1").ToString());
                OpMngr2 = getAxUserEmail(DynAx, DynRec.get_Field("OperationManager2").ToString());
                GM1 = getAxUserEmail(DynAx, DynRec.get_Field("GeneralManager1").ToString());
                GM2 = getAxUserEmail(DynAx, DynRec.get_Field("GeneralManager2").ToString());
            }
            DynAx.Dispose();
            return new Tuple<string, string, string, string, string, string, Tuple<string, string, string, string, string, string, string>>
                (hod1, hod2, hod3, salesAdmin1, salesAdmin2, salesAdmin3, Tuple.Create(salesAdmMng1, salesAdmMng2, salesAdmMng3, OpMngr1, OpMngr2, GM1, GM2));
        }

        public static string getCustomerType(Axapta DynAx, string CustomerTypeCode)
        {
            if (CustomerTypeCode == "")
            {
                return "";
            }
            string ClassDesc = "";
            int MSBCustomerType = 30005;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", MSBCustomerType);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 30001);//CustomerTypeCode
            qbr3.Call("value", CustomerTypeCode);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", MSBCustomerType);
                ClassDesc = DynRec3.get_Field("TypeDesc").ToString();
                DynRec3.Dispose();
            }
            return ClassDesc;
        }

        public static string getCustomerMainGroup(Axapta DynAx, string CustomerMainGroup)
        {
            if (CustomerMainGroup == "")
            {
                return "";
            }
            string ClassDesc = "";
            int MSBCustomerMainGroup = 30004;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", MSBCustomerMainGroup);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 30002);//CustomerMainGroup
            qbr3.Call("value", CustomerMainGroup);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", MSBCustomerMainGroup);
                ClassDesc = DynRec3.get_Field("MainGroupDesc").ToString();
                DynRec3.Dispose();
            }
            return ClassDesc;
        }

        public static Tuple<string, string, string, string, double, double, Tuple<double, string, double, bool, double>> get_CustAging(Axapta DynAx, string customerAcc)
        {
            string BankCorpGuaranteeAmt = ""; string BankCorpExpiryDate = ""; string LastMonthCollection = "";
            string CurrentMonthCollection = ""; double AvgPaymDays = 0; double AvgPaymDays2 = 0; double AvgMonthlySales = 0;
            string LastReturnCheqAmt = ""; double PostDatedChqTotal = 0; bool ApplyCredit = false; double OverDueInterest = 0;
            int Cust_AgingTable = 40056;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", Cust_AgingTable);

            var qbr1_7 = (AxaptaObject)axQueryDataSource.Call("addRange", 40008);
            qbr1_7.Call("value", customerAcc);

            axQueryDataSource.Call("addSortField", 65534, 1);//RecId, descending
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", Cust_AgingTable);

                BankCorpGuaranteeAmt = DynRec.get_Field("BankCorpGuaranteeAmt").ToString();
                BankCorpExpiryDate = Convert.ToDateTime(DynRec.get_Field("BankCorpExpiryDate")).ToString("dd/MM/yyyy");
                LastMonthCollection = DynRec.get_Field("LastMonthCollection").ToString();
                CurrentMonthCollection = DynRec.get_Field("CurrentMonthCollection").ToString();
                AvgPaymDays = Convert.ToDouble(DynRec.get_Field("AvgPaymDays"));
                AvgPaymDays2 = Convert.ToDouble(DynRec.get_Field("AvgPaymDays2"));
                AvgMonthlySales = Convert.ToDouble(DynRec.get_Field("AvgMonthlySales"));
                LastReturnCheqAmt = DynRec.get_Field("LastReturnCheqAmt").ToString();
                PostDatedChqTotal = Convert.ToDouble(DynRec.get_Field("PostDatedChqTotal"));
                ApplyCredit = Convert.ToBoolean(DynRec.get_Field("ApplyCredit"));
                OverDueInterest = Convert.ToDouble(DynRec.get_Field("OverDueInterest"));

                DynRec.Dispose();
            }
            return new Tuple<string, string, string, string, double, double, Tuple<double, string, double, bool, double>>
                (BankCorpGuaranteeAmt, BankCorpExpiryDate, LastMonthCollection, CurrentMonthCollection, AvgPaymDays, AvgPaymDays2,
                new Tuple<double, string, double, bool, double>(AvgMonthlySales, LastReturnCheqAmt, PostDatedChqTotal, ApplyCredit, OverDueInterest));
        }

        public static Tuple<string, string, string, string, double, double, double, Tuple<double>> getEORdetails(Axapta DynAx, string AccNo)
        {
            string EorTarget = ""; string TargetCarton = ""; string Expiry = ""; string Commence = "";
            double CCNCTN = 0; double CORCTN = 0; double CTGTCTN = 0; double CACTCTN = 0;
            int LF_EOR_MASTER = 30207;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_EOR_MASTER);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 30002);//CustAcc
            qbr.Call("value", AccNo);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_EOR_MASTER);

                EorTarget = DynRec.get_Field("GrandTargetCarton").ToString();
                TargetCarton = DynRec.get_Field("TGTCTN").ToString();
                Expiry = DynRec.get_Field("EXPDATE").ToString();
                Commence = DynRec.get_Field("COMDATE").ToString();
                CCNCTN = Convert.ToDouble(DynRec.get_Field("CCNCTN"));
                CORCTN = Convert.ToDouble(DynRec.get_Field("CORCTN"));
                CTGTCTN = Convert.ToDouble(DynRec.get_Field("CTGTCTN"));
                CACTCTN = Convert.ToDouble(DynRec.get_Field("CACTCTN"));
            }

            return new Tuple<string, string, string, string, double, double, double, Tuple<double>>(EorTarget, TargetCarton, Expiry, Commence, CCNCTN, CORCTN, CTGTCTN, new Tuple<double>(CACTCTN));
        }

        // Jerry 2024-12-04 Allow HOD to create redemption for customer with up to 365 days of inactivities
        //public static string getCustOutstanding(Axapta DynAx, string temp_CustAcc, int days)
        public static string getCustOutstanding(Axapta DynAx, string temp_CustAcc, int grace_days, bool isHoD = false)
        // Jerry 2024-12-04 End
        {
            int CustTrans = 78; string DueDate = "";
            string warning = "";
            var today = DateTime.Now.ToString("dd/MM/yyyy"); ;
            var var_TransDate = DateTime.Now;
            DateTime latestDate = DateTime.Now;
            List<DateTime> listTransDt = new List<DateTime>();
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

            var qbr_12 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);
            qbr_12.Call("value", "*/INV");

            if (temp_CustAcc != "")
            {
                var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//ACCOUNTNUM
                qbr_0_1.Call("value", temp_CustAcc);
            }

            axQueryDataSource.Call("addSortField", 2, 1);//TransId, descending
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            // Loop through the set of retrieved records.

            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);
                //==========================================================================
                string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                string temp_DueDate = DynRec.get_Field("DueDate").ToString();
                string TransDate = "";
                if (temp_TransDate != "")
                {
                    string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                    string Raw_TransDate = arr_temp_TransDate[0];
                    TransDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_TransDate, true);
                }
                if (temp_DueDate != "")
                {
                    string[] arr_temp_DueDate = temp_DueDate.Split(' ');//date + " " + time;
                    string Raw_DueDate = arr_temp_DueDate[0];
                    DueDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DueDate, true);
                }

                //==========================================================================
                //for extra filter purpose since CustTrans do not have Axapta control authorization

                string temp_Cust_Acc = DynRec.get_Field("AccountNum").ToString().Trim();
                string temp_Cust_Name = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, temp_Cust_Acc);
                //==========================================================================

                string str_AmountCur = DynRec.get_Field("AmountCur").ToString();

                var_TransDate = DateTime.ParseExact(TransDate, "dd/MM/yyyy", null);

                listTransDt.Add(var_TransDate);
                listTransDt.Sort();
                latestDate = listTransDt.LastOrDefault();

                DynRec.Dispose();
            }

            if (latestDate.ToString() != "")
            {
                // Jerry 2024-11-29 Allow Sales Admin to bypass inactivity checking
                var sales_admin = Redemption_Get_Details.getSalesAdmin(DynAx);
                
                //get logined_user_name to check if logined_user_name is sales admin
                string logined_user_name = "";
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    logined_user_name = HttpContext.Current.Session["logined_user_name"] as string;
                }                

                // if logined_user_name is sales admin, return empty string to allow creation of the redemption
                if (sales_admin.Contains(logined_user_name))
                {
                    return "";
                }
                // Jerry 2024-11-29 End




                // Jerry 2024-12-04 Allow HOD to create redemption for customer with up to 360 days of inactivities

                //diff90 = (Convert.ToDateTime(today) - Convert.ToDateTime(latestDate)).TotalDays;
                double inactive_days = (Convert.ToDateTime(today) - Convert.ToDateTime(latestDate)).TotalDays;

                if (isHoD && inactive_days <= 360)
                {
                    return "";
                }

                // Jerry 2024-12-04 End


                if (inactive_days > grace_days)
                {
                    // Jerry 2024-12-09 Warning based on 3,6 & 12 months of inactivity
                    /*
                    if (grace_days == 93)
                    {
                        warning = "Warning! No active transaction over the last 3 months for this customer." + " Latest invoice date: " + latestDate;
                    }
                    else//185days from last invoice date
                    {
                        warning = "Warning! No active transaction over the last 6 months for this customer.";
                    }
                    */
                    if (inactive_days > 360)
                    {
                        warning = "Warning! No active transaction over at least 12 months for this customer.";
                    }
                    else if (inactive_days > 180)
                    {
                        warning = "Warning! No active transaction over the last 6 months for this customer.";
                    }
                    else if (inactive_days > 90)
                    {
                        warning = "Warning! No active transaction over the last 3 months for this customer. Latest invoice date: " + latestDate;
                    }

                    // Jerry 2024-12-09 Warning based on 3,6 & 12 months of inactivity - END
                }
            }
            else
            {
                warning = "Warning! TransDate not found.";
            }

            // Sort the list by transaction date in descending order
            return warning;
        }

        public static Tuple<string, string> get_latestInvoiceTrans(Axapta DynAx, string AccountNum)
        {
            string TransDate = "";
            string InvoiceNo = "";
            string TableName = "CustTrans";
            int tableId = DynAx.GetTableId(TableName);
            List<Tuple<string, string>> listTransDt = new List<Tuple<string, string>>();
            listTransDt.Clear();

            AxaptaObject axQuery1_8 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_8 = (AxaptaObject)axQuery1_8.Call("addDataSource", tableId);

            int currentYear = DateTime.Now.Year;
            DateTime lastYear = new DateTime(currentYear - 1, 1, 1);
            DateTime currentDt = DateTime.Now;

            var var_Qdate = lastYear.ToString("dd/MM/yyyy") + ".." + currentDt.ToString("dd/MM/yyyy");
            var qbr2_2 = (AxaptaObject)axQueryDataSource1_8.Call("addRange", 2);
            qbr2_2.Call("value", var_Qdate);

            var qbr1_8 = (AxaptaObject)axQueryDataSource1_8.Call("addRange", 1); //accountno
            qbr1_8.Call("value", AccountNum);

            AxaptaObject axQueryRun1_8 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_8);
            while ((bool)axQueryRun1_8.Call("next"))
            {
                AxaptaRecord DynRec1_8 = (AxaptaRecord)axQueryRun1_8.Call("Get", tableId);
                // Check for null values and convert to an empty string if null
                var invoiceFieldValue = DynRec1_8.get_Field("Invoice");
                InvoiceNo = invoiceFieldValue != null ? invoiceFieldValue.ToString() : "";
                var transDateFieldValue = DynRec1_8.get_Field("TransDate");
                TransDate = transDateFieldValue != null ? Convert.ToDateTime(transDateFieldValue).ToString("dd/MM/yyyy") : "";

                listTransDt.Add(new Tuple<string, string>(InvoiceNo, TransDate));
                DynRec1_8.Dispose();
            }

            // Sort the list by transaction date in descending order
            listTransDt.Sort((x, y) => DateTime.ParseExact(y.Item2, "dd/MM/yyyy", null)
                                             .CompareTo(DateTime.ParseExact(x.Item2, "dd/MM/yyyy", null)));

            // Get the latest transaction date and invoice number
            Tuple<string, string> latestTrans = listTransDt.FirstOrDefault();

            // Return an "empty" tuple if listTransDt is empty
            return listTransDt.FirstOrDefault() ?? new Tuple<string, string>("", "");
        }

        public static List<ListItem> get_PointCategory(Axapta DynAx)
        {
            List<ListItem> list = new List<ListItem>();
            string PointCategory = ""; string PointDesc = "";
            string tablename = "LF_PointCategory";
            string fieldname = "LP_Used";

            int tableid = DynAx.GetTableId(tablename);
            int fieldid = DynAx.GetFieldId(tableid, fieldname);
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableid);

            var filter = (AxaptaObject)axQueryDataSource.Call("addRange", fieldid);
            filter.Call("Value", "1");

            list.Add(new ListItem("-- SELECT --", ""));
            AxaptaObject axQueryRun1_13 = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun1_13.Call("next"))
            {
                AxaptaRecord DynRec1_13 = (AxaptaRecord)axQueryRun1_13.Call("Get", tableid);
                PointCategory = DynRec1_13.get_Field("PointCategory").ToString();
                PointDesc = DynRec1_13.get_Field("PointDesc").ToString();
                list.Add(new ListItem(PointCategory + " " + PointDesc));
                DynRec1_13.Dispose();
            }
            return list;
        }

        public class AmountData
        {
            public string Hod1 { get; set; }
            public string Hod2 { get; set; }
            public string Hod3 { get; set; }
            public string SalesAdmin1 { get; set; }
            public string SalesAdmin2 { get; set; }
            public string SalesAdmin3 { get; set; }
            public string SAManager1 { get; set; }
            public string SAManager2 { get; set; }
            public string SAManager3 { get; set; }
            public string CcManager1 { get; set; }
            public string CcManager2 { get; set; }
            public string CcManager3 { get; set; }
            public string OManager1 { get; set; }
            public string OManager2 { get; set; }
            public string OManager3 { get; set; }
            public string GM1 { get; set; }
            public string GM2 { get; set; }
        }

        // Jerry 2024-11-21 Get list of SalesAdmin
        public static List<string> getSalesAdmin(Axapta DynAx)
        {
            var sales_admin = new List<string>();

            int tableId = DynAx.GetTableId("LF_WebRedemp_AppGrp");
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            DynAx.CreateAxaptaObject("QueryRun", axQuery);
            AxaptaObject AxQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                string sa = DynRec.get_Field("SalesAdmin1").ToString();
                sales_admin.Add(sa);
            }

            return sales_admin;
        }
        // Jerry 2024-11-21 End

        public static AmountData GetAmount(Axapta DynAx, double amount)
        {
            AmountData data = new AmountData();

            double AmountFromAx = 0; double AmountToAx = 0;

            int tableId = DynAx.GetTableId("LF_WebRedemp_AppGrp");
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            DynAx.CreateAxaptaObject("QueryRun", axQuery);
            AxaptaObject AxQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                AmountFromAx = Convert.ToDouble(DynRec.get_Field("AmountFrom"));
                AmountToAx = Convert.ToDouble(DynRec.get_Field("AmountTo"));
                if (amount >= AmountFromAx && amount <= AmountToAx)
                {
                    data.SalesAdmin1 = DynRec.get_Field("SalesAdmin1").ToString();
                    data.SalesAdmin2 = DynRec.get_Field("SalesAdmin2").ToString();
                    data.SalesAdmin3 = DynRec.get_Field("SalesAdmin3").ToString();
                    data.SAManager1 = DynRec.get_Field("SalesAdminManager1").ToString();
                    data.SAManager2 = DynRec.get_Field("SalesAdminManager2").ToString();
                    data.SAManager3 = DynRec.get_Field("SalesAdminManager3").ToString();
                    data.CcManager1 = DynRec.get_Field("CreditControlManager1").ToString();
                    data.CcManager2 = DynRec.get_Field("CreditControlManager2").ToString();
                    data.CcManager3 = DynRec.get_Field("CreditControlManager3").ToString();
                    data.OManager1 = DynRec.get_Field("OperationManager1").ToString();
                    data.OManager2 = DynRec.get_Field("OperationManager2").ToString();
                    data.OManager3 = DynRec.get_Field("OperationManager3").ToString();
                    data.GM1 = DynRec.get_Field("GeneralManager1").ToString();
                    data.GM2 = DynRec.get_Field("GeneralManager2").ToString();
                    break; // Exit the loop since we found a match
                }
            }
            return data;
        }

        public static string GetSoID(Axapta DynAx, string CustomerRef)
        {
            AxaptaRecord DynRec;
            string TableName = "SalesTable";
            string fieldName = ("CustomerRef");
            string soID = "";

            // Define the record
            DynRec = DynAx.CreateAxaptaRecord(TableName);
            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", fieldName, CustomerRef));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                soID = DynRec.get_Field("SalesId").ToString();
            }
            return soID;
        }

        public static Tuple<string, string, string, string, double, double, double> GetVendDetails(Axapta DynAx, string AccNo)
        {
            string AccountNum = ""; string Name = ""; string VendGroup = ""; string Commence = "";
            double CCNCTN = 0; double CORCTN = 0; double CTGTCTN = 0;
            string tablename = "VendTable";
            string fieldname = "CustAccount";

            int tableid = DynAx.GetTableId(tablename);
            int fieldid = DynAx.GetFieldId(tableid, fieldname);

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableid);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", fieldid);//CustAcc
            qbr.Call("value", AccNo);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableid);

                AccountNum = DynRec.get_Field("AccountNum").ToString();
                Name = DynRec.get_Field("Name").ToString();
                VendGroup = DynRec.get_Field("VendGroup").ToString();
                //Commence = DynRec.get_Field("COMDATE").ToString();
                //CCNCTN = Convert.ToDouble(DynRec.get_Field("CCNCTN"));
                //CORCTN = Convert.ToDouble(DynRec.get_Field("CORCTN"));
                //CTGTCTN = Convert.ToDouble(DynRec.get_Field("CTGTCTN"));
            }

            return new Tuple<string, string, string, string, double, double, double>(AccountNum, Name, VendGroup, Commence, CCNCTN, CORCTN, CTGTCTN);
        }

        public static Tuple<string, string, string, string, double, double, double> GetPurchaseOrderDetails(Axapta DynAx, string vendorRef)
        {
            string PurchId = ""; string PurchName = ""; string InvoiceAccount = ""; string Commence = "";
            double CCNCTN = 0; double CORCTN = 0; double CTGTCTN = 0;
            string tablename = "PurchTable";
            string fieldname = "VendorRef";

            int tableid = DynAx.GetTableId(tablename);
            int fieldid = DynAx.GetFieldId(tableid, fieldname);

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableid);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", fieldid);//vendorref
            qbr.Call("value", vendorRef);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableid);

                PurchId = DynRec.get_Field("PurchId").ToString();
                PurchName = DynRec.get_Field("PurchName").ToString();
                InvoiceAccount = DynRec.get_Field("InvoiceAccount").ToString();
                //Commence = DynRec.get_Field("COMDATE").ToString();
                //CCNCTN = Convert.ToDouble(DynRec.get_Field("CCNCTN"));
                //CORCTN = Convert.ToDouble(DynRec.get_Field("CORCTN"));
                //CTGTCTN = Convert.ToDouble(DynRec.get_Field("CTGTCTN"));
            }

            return new Tuple<string, string, string, string, double, double, double>(PurchId, PurchName, InvoiceAccount, Commence, CCNCTN, CORCTN, CTGTCTN);
        }

        public static Tuple<string, string, string, string, string, string[]> getCustInfo(Axapta DynAx, string CustName)
        {
            //int CustTable = 77;
            int tableId = DynAx.GetTableId("CustTable");
            int Name = DynAx.GetFieldId(tableId, "Name");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", Name);//CustName
            qbr1.Call("value", "*" + CustName + "*");
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            int count = 0;
            string Address = "";
            string temp_EmplId = "";
            string temp_EmpName = "";
            string BranchID = "";
            string CustomerClass = "";
            string[] AccountNum = new string[500]; 
            for (int i = 0; i < AccountNum.Length; i++)
            {
                AccountNum[i] = "0";
            }

            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                Address = DynRec1.get_Field("Address").ToString();
                temp_EmplId = DynRec1.get_Field("EmplId").ToString();

                BranchID = DynRec1.get_Field("Dimension").ToString();
                string accNumber = DynRec1.get_Field("AccountNum").ToString();
                if (accNumber != "")
                {
                    AccountNum[count] = accNumber;
                }
                else
                {
                    AccountNum[count] = "0";
                }
                count++;
                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string[]>
                (Address, temp_EmplId, temp_EmpName, BranchID, CustomerClass, AccountNum);
        }

        public static string getRedempApprovalAccess(Axapta DynAx, string loginID)
        {
            int LF_WebRedemp_AppGrp = DynAx.GetTableId("LF_WebRedemp_AppGrp");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WebRedemp_AppGrp);
            HashSet<string> getUniq = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<ListItem> List_Approval = new List<ListItem>();

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            // Variable to store the field name
            string matchedField = string.Empty;

            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WebRedemp_AppGrp);

                // Check each field for the loginID and log the field name if matched
                if (loginID.Equals(DynRec1.get_Field("SalesAdmin1").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "SalesAdmin1";
                else if (loginID.Equals(DynRec1.get_Field("SalesAdmin2").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "SalesAdmin2";
                else if (loginID.Equals(DynRec1.get_Field("SalesAdmin3").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "SalesAdmin3";

                else if (loginID.Equals(DynRec1.get_Field("SalesAdminManager1").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "SalesAdminManager1";
                else if (loginID.Equals(DynRec1.get_Field("SalesAdminManager2").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "SalesAdminManager2";
                else if (loginID.Equals(DynRec1.get_Field("SalesAdminManager3").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "SalesAdminManager3";

                else if (loginID.Equals(DynRec1.get_Field("CreditControlManager1").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "CreditControlManager1";
                else if (loginID.Equals(DynRec1.get_Field("CreditControlManager2").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "CreditControlManager2";
                else if (loginID.Equals(DynRec1.get_Field("CreditControlManager3").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "CreditControlManager3";

                else if (loginID.Equals(DynRec1.get_Field("OperationManager1").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "OperationManager1";
                else if (loginID.Equals(DynRec1.get_Field("OperationManager2").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "OperationManager2";
                else if (loginID.Equals(DynRec1.get_Field("OperationManager3").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "OperationManager3";

                else if (loginID.Equals(DynRec1.get_Field("GeneralManager1").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "GeneralManager1";
                else if (loginID.Equals(DynRec1.get_Field("GeneralManager2").ToString(), StringComparison.OrdinalIgnoreCase))
                    matchedField = "GeneralManager2";

                // If a match is found, return the field name
                if (!string.IsNullOrEmpty(matchedField))
                {
                    DynRec1.Dispose();
                    return matchedField;
                }

                DynRec1.Dispose();
            }

            // If no match is found, return an empty string or an appropriate message
            return string.Empty;
        }

        public static string getPPMTP_JournalId(Axapta DynAx, string CustAcc, string RedempId)
        {
            string PPMTP_JournalId = "";

            int PPMTP_tmpUsedPts = DynAx.GetTableId("PPMTP_tmpUsedPts");
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", PPMTP_tmpUsedPts);

            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 40004);//AccountNum
            qbr6.Call("value", CustAcc);

            var qbr7 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30001);//ppmtp_redempno
            qbr7.Call("value", RedempId);

            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", PPMTP_tmpUsedPts);
                PPMTP_JournalId = DynRec6.get_Field("PPMTP_JournalId").ToString();
                DynRec6.Dispose();
            }
            return PPMTP_JournalId;
        }

        public static string getPPMTP_JournalId_AfterPosting(Axapta DynAx, string CustAcc, string RedempId)
        {
            string PPMTP_JournalId = "";

            int PPMTP_UsedPts = DynAx.GetTableId("PPMTP_UsedPts");
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", PPMTP_UsedPts);

            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 40004);//AccountNum
            qbr6.Call("value", CustAcc);

            var qbr7 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30001);//ppmtp_redempno
            qbr7.Call("value", RedempId);

            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", PPMTP_UsedPts);
                PPMTP_JournalId = DynRec6.get_Field("PPMTP_JournalId").ToString();
                DynRec6.Dispose();
            }
            return PPMTP_JournalId;
        }
    }
}