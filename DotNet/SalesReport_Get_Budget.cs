using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.WebControls;

namespace DotNet
{
    public class SalesReport_Get_Budget
    {
        public static DateTime now = DateTime.Now;
        public static string autoGen = "This is an auto generated mail.";

        public static List<ListItem> getSalesman(Axapta DynAx)
        {
            List<ListItem> List_Salesman = new List<ListItem>();
            string getSalesmanId = ""; string getSalesmanName = "";

            int emplTable = 103;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", emplTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30004);
            qbr1.Call("value", "0");//'Field Id 30004 = EmplStatus; 0 = Active

            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30009);
            qbr1_2.Call("value", "0");// 'LF_StopEMail YesNo

            axQueryDataSource1.Call("addSortField", 1, 0);//LF_EmplId, asc

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", emplTable);
                getSalesmanId = DynRec1.get_Field("emplId").ToString();

                if (!getSalesmanId.StartsWith("H"))
                {
                    getSalesmanName = DynRec1.get_Field("DEL_NAME").ToString();
                    List_Salesman.Add(new ListItem("(" + getSalesmanId + ") " + getSalesmanName, getSalesmanId));
                }
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            return List_Salesman;
        }

        public static Tuple<string, int, string, string[], int, string, string> getEmpDetails(Axapta DynAx, string temp_EmplId)
        {
            string temp_EmpName = ""; int countEmail = 0; string temp_EmpEmail = "";
            int count = 0; string[] emplID = new string[10]; string LF_EmpEmailID = ""; string LF_SalesApprovalGroup = "";
            if (temp_EmplId != "")
            {
                int EmplTable = 103;
                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

                var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//EmplId
                qbr2.Call("value", temp_EmplId);
                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                    if (!DynRec2.get_Field("EmplId").ToString().StartsWith("H"))
                    {
                        emplID[count] = DynRec2.get_Field("EmplId").ToString();
                        temp_EmpName = DynRec2.get_Field("Del_Name").ToString();
                        temp_EmpEmail = DynRec2.get_Field("DEL_Email").ToString();
                        LF_EmpEmailID = DynRec2.get_Field("LF_EmpEMailID").ToString();
                        LF_SalesApprovalGroup = DynRec2.get_Field("LF_SalesApprovalGroup").ToString();
                        count++;
                    }
                    DynRec2.Dispose();
                }
            }
            return new Tuple<string, int, string, string[], int, string, string>
                (temp_EmpName, countEmail, temp_EmpEmail, emplID, count, LF_EmpEmailID, LF_SalesApprovalGroup);
        }

        public static Tuple<string[], string, string, string[], int, string[], string> getEmpDetailsFilterSalesAppGrp(Axapta DynAx, string LF_SalesApprovalGroup)
        {
            string[] temp_EmpName = new string[100]; string temp_EmpContact = ""; string temp_EmpEmail = ""; string HOD = "";
            int count = 0; string[] emplID = new string[500]; string[] LF_EmpEmailID = new string[500];

            int EmplTable = 103;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30012);//LF_SalesApprovalGroup
            qbr2.Call("value", LF_SalesApprovalGroup);

            var qbr1_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30009);
            qbr1_2.Call("value", "0");// 'LF_StopEMail YesNo

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                if (!DynRec2.get_Field("EmplId").ToString().StartsWith("H"))
                {
                    emplID[count] = DynRec2.get_Field("EmplId").ToString();
                    temp_EmpName[count] = DynRec2.get_Field("Del_Name").ToString();
                    temp_EmpContact = DynRec2.get_Field("DEL_Phone").ToString();
                    temp_EmpEmail = DynRec2.get_Field("DEL_Email").ToString();
                    LF_EmpEmailID[count] = DynRec2.get_Field("LF_EmpEMailID").ToString();
                    count++;
                }
                else
                {
                    HOD = DynRec2.get_Field("EmplId").ToString() + " " + DynRec2.get_Field("Del_Name").ToString();
                }
                DynRec2.Dispose();
            }
            return new Tuple<string[], string, string, string[], int, string[], string>
                (temp_EmpName, temp_EmpContact, temp_EmpEmail, emplID, count, LF_EmpEmailID, HOD);
        }

        public static Tuple<double, double> getPeriodName(Axapta DynAx, string PeriodName, string temp_EmplId)
        {
            double MonthlyBudget = 0;
            double totalBudget = 0; string period;
            int LF_PPMSalesBudget = 30378;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_PPMSalesBudget);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30002);//EmplId
            qbr2.Call("value", temp_EmplId);

            var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 30001); //PeriodName
            qbr.Call("value", PeriodName);

            if (PeriodName != "")//avoid getting wrong records from table
            {
                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_PPMSalesBudget);
                    period = DynRec2.get_Field("PeriodName").ToString();
                    MonthlyBudget = Convert.ToDouble(DynRec2.get_Field("MonthlyBudget"));
                    //Debug.WriteLine(temp_EmplId + " " + period + " " + MonthlyBudget);
                    totalBudget = totalBudget + Convert.ToDouble(MonthlyBudget);

                    DynRec2.Dispose();
                }
            }
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            return new Tuple<double, double>(MonthlyBudget, totalBudget);
        }

        public static string getStartDateToDate(Axapta DynAx, string toDate)
        {
            string PeriodName = "";
            int LF_PeriodTable = 30238;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_PeriodTable);

            //var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30002);//startDate
            //qbr2.Call("value", startDate);

            var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 30003);//toDate
            qbr.Call("value", toDate);

            var qbr1 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30004);//Yearly
            qbr1.Call("value", "0");//not yearly

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_PeriodTable);
                PeriodName = DynRec2.get_Field("PeriodName").ToString();
                //Debug.WriteLine(PeriodName);
                DynRec2.Dispose();
            }
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            axQueryRun2.Dispose();
            return PeriodName;
        }

        public static double getPeriodNameYearly(Axapta DynAx, string startPeriodName, string endPeriodName, string temp_EmplId)
        {
            double MonthlyBudget = 0;
            double totalBudget = 0;
            string currentYear = DateTime.Now.Year.ToString();
            string currentMonth = DateTime.Now.Month.ToString();

            int LF_PPMSalesBudget = 30378;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_PPMSalesBudget);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30002);//EmplId
            qbr2.Call("value", temp_EmplId);

            //var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 30001); //PeriodName
            //qbr.Call("value", startPeriodName + ".." + endPeriodName);
            //if (!string.IsNullOrEmpty(PeriodName))
            //{
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_PPMSalesBudget);
                string periodname = DynRec2.get_Field("PeriodName").ToString();
                var month = periodname.Split('-');
                MonthlyBudget = Convert.ToDouble(DynRec2.get_Field("MonthlyBudget"));
                if (periodname.Contains(currentYear))
                {
                    if (Convert.ToInt16(currentMonth) >= Convert.ToInt16(month[2]))
                    {
                        totalBudget = totalBudget + Convert.ToDouble(MonthlyBudget);
                    }
                }

                DynRec2.Dispose();
            }
            //}
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            return totalBudget;
        }

        public static Tuple<string, string, string> getStartDateToDateYearly(Axapta DynAx, string toDate)
        {
            string PeriodName = "";
            DateTime startDate = new DateTime(); DateTime ToDate = new DateTime();
            string fromDate = ""; string fieldToDate = "";
            int LF_PeriodTable = 30238;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_PeriodTable);

            //var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30002);//startDate
            //qbr2.Call("value", startDate);

            var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 30003);//toDate
            qbr.Call("value", toDate);

            var qbr1 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30004);//Yearly
            qbr1.Call("value", "1");//yearly

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_PeriodTable);
                startDate = Convert.ToDateTime(DynRec2.get_Field("StartDate").ToString());
                fromDate = startDate.ToString("dd/MM/yyyy");

                ToDate = Convert.ToDateTime(DynRec2.get_Field("ToDate").ToString());
                fieldToDate = ToDate.ToString("dd/MM/yyyy");

                PeriodName = DynRec2.get_Field("PeriodName").ToString();
                DynRec2.Dispose();
            }
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            axQueryRun2.Dispose();
            return new Tuple<string, string, string>(PeriodName, fromDate, fieldToDate);
        }

        public static Tuple<double, int> getSalesOrder(Axapta DynAx, string temp_CustAccount, string firstDayOfYear, string currentDt)
        {
            double invoiceAmount = 0; int count = 0;
            string createdDt = "";

            int SalesLine = 359;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesLine);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 28);//CustAccount
            qbr.Call("value", temp_CustAccount);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);//SalesStatus
            qbr1.Call("value", "1");//backorder, label as open order

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 35);//SalesType
            qbr2.Call("value", "3");//sales, label as sales order

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesLine);
                createdDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime")).ToString("dd/MM/yyyy");
                if (DateTime.ParseExact(createdDt, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(firstDayOfYear, "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(createdDt, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(currentDt, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    string SalesPrice = DynRec.get_Field("LineAmount").ToString();
                    string salesID = DynRec.get_Field("SalesId").ToString();
                    //temp_InvoiceAmount = getInvoiceAmount(DynAx, SalesPrice);
                    if (SalesPrice != "0")
                    {
                        invoiceAmount = invoiceAmount + Convert.ToDouble(SalesPrice);

                        count = count + 1;
                        Debug.WriteLine(count + " " + temp_CustAccount + " " + salesID + " " + SalesPrice);
                    }
                }

                DynRec.Dispose();
            }
            axQueryRun.Dispose();
            axQueryDataSource.Dispose();
            return new Tuple<double, int>(invoiceAmount, count);
        }

        public static Tuple<string, string, string, string, int, string[]> GetGroupCode(Axapta DynAx)
        {
            int LF_SalesApprovalLevel = 30370;
            string EMailLvl1 = ""; string EMailLvl2 = ""; string EMailLvl3 = ""; string EMailLvl4 = "";
            string[] GroupCode = new string[100]; int count = 0;
            AxaptaObject axQuery16 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource16 = (AxaptaObject)axQuery16.Call("addDataSource", LF_SalesApprovalLevel);

            axQueryDataSource16.Call("addSortField", 30001, 0);//GroupId, ascending

            AxaptaObject axQueryRun16 = DynAx.CreateAxaptaObject("QueryRun", axQuery16);
            while ((bool)axQueryRun16.Call("next"))
            {
                AxaptaRecord DynRec16 = (AxaptaRecord)axQueryRun16.Call("Get", LF_SalesApprovalLevel);
                EMailLvl1 = DynRec16.get_Field("EMailLvl1").ToString();
                EMailLvl2 = DynRec16.get_Field("EMailLvl2").ToString();
                EMailLvl3 = DynRec16.get_Field("EMailLvl3").ToString();
                EMailLvl4 = DynRec16.get_Field("EMailLvl4").ToString();
                GroupCode[count] = DynRec16.get_Field("GroupCode").ToString();
                count++;
                DynRec16.Dispose();
            }
            return new Tuple<string, string, string, string, int, string[]>
                (EMailLvl1, EMailLvl2, EMailLvl3, EMailLvl4, count, GroupCode);
        }

        public static Tuple<string, string, string[], int, string[], string> getEmpDetails2(Axapta DynAx, string temp_EmplEmail)
        {
            string temp_EmpName = ""; string temp_EmpEmail = "";
            int count = 0; string[] emplID = new string[10]; string[] LF_EmpEmailID = new string[10]; string LF_SalesApprovalGroup = "";
            if (temp_EmplEmail != "")
            {
                int EmplTable = 103;
                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

                var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30011);//LF_EmplEmail
                qbr2.Call("value", temp_EmplEmail);

                var qbr1_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30009);
                qbr1_2.Call("value", "0");// 'LF_StopEMail YesNo

                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                    if (!DynRec2.get_Field("EmplId").ToString().StartsWith("H"))
                    {
                        emplID[count] = DynRec2.get_Field("EmplId").ToString();
                        temp_EmpName = DynRec2.get_Field("Del_Name").ToString();
                        temp_EmpEmail = DynRec2.get_Field("DEL_Email").ToString();
                        LF_EmpEmailID[count] = DynRec2.get_Field("LF_EmpEMailID").ToString();
                        count++;
                    }
                    DynRec2.Dispose();
                }
            }
            return new Tuple<string, string, string[], int, string[], string>
                (temp_EmpName, temp_EmpEmail, emplID, count, LF_EmpEmailID, LF_SalesApprovalGroup);
        }

        public static double getBatterySerialNumber(Axapta DynAx, string custDO)
        {
            double serialNumber = 0;
            var getItemID = WClaim_GET_Report.getItem(DynAx, custDO);
            int LF_AutoSerialTrans = 30179;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_AutoSerialTrans);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30001);//invoice ID
            qbr2.Call("value", getItemID.Item4);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_AutoSerialTrans);
                serialNumber = Convert.ToDouble(DynRec2.get_Field("SerialNum").ToString());

                DynRec2.Dispose();
            }
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            return serialNumber;
        }

        public static Tuple<string, string, string, string> get_HODemplIdLvl(Axapta DynAx, string GroupId)
        {
            int LF_SalesApprovalLevel = 30370;
            string EmplIdLvl1 = ""; string EmplIdLvl2 = ""; string EmplIdLvl3 = ""; string EmplIdLvl4 = "";

            AxaptaObject axQuery16 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource16 = (AxaptaObject)axQuery16.Call("addDataSource", LF_SalesApprovalLevel);

            var qbr16 = (AxaptaObject)axQueryDataSource16.Call("addRange", 30001);//GroupId
            qbr16.Call("value", GroupId);
            AxaptaObject axQueryRun16 = DynAx.CreateAxaptaObject("QueryRun", axQuery16);
            if (GroupId != "")
            {
                if ((bool)axQueryRun16.Call("next"))
                {
                    AxaptaRecord DynRec16 = (AxaptaRecord)axQueryRun16.Call("Get", LF_SalesApprovalLevel);
                    EmplIdLvl1 = getEmail(DynAx, DynRec16.get_Field("EmplIdLvl1").ToString());
                    EmplIdLvl2 = getEmail(DynAx, DynRec16.get_Field("EmplIdLvl2").ToString());
                    EmplIdLvl3 = getEmail(DynAx, DynRec16.get_Field("EmplIdLvl3").ToString());
                    EmplIdLvl4 = getEmail(DynAx, DynRec16.get_Field("EmplIdLvl4").ToString());
                    DynRec16.Dispose();
                }
            }
            return new Tuple<string, string, string, string>(EmplIdLvl1, EmplIdLvl2, EmplIdLvl3, EmplIdLvl4);
        }

        public static string getEmail(Axapta DynAx, string temp_EmplId)
        {
            string LF_EmpEmailID = "";

            int EmplTable = 103;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", EmplTable);

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//EmplId
            qbr2.Call("value", temp_EmplId);
            if (temp_EmplId != "")//to cater PPM "" emplid with invalid email address
            {
                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                if ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                    LF_EmpEmailID = DynRec2.get_Field("LF_EmpEMailID").ToString();
                    DynRec2.Dispose();
                }
            }
            return LF_EmpEmailID;
        }

        public static string getSkipHODstatus(Axapta DynAx, string emplid)
        {
            string LF_SkipHOD = "";

            int EmplTable = 103;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//emplid
            qbr2.Call("value", emplid);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);

                LF_SkipHOD = DynRec2.get_Field("LF_SkipHOD").ToString();

                DynRec2.Dispose();
            }
            return LF_SkipHOD;
        }

        public static Tuple<string[], int, string[], string[], int, string[], string[]> getEmpDetailsOtherDiv(Axapta DynAx)
        {
            string[] temp_EmpName = new string[100];
            int count = 0;
            string[] emplID = new string[100];
            string[] LF_EmpEmailID = new string[100];
            string[] temp_EmpEmail = new string[100];
            string[] ReportTo = new string[100];

            int EmplTable = 103;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

            //var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//EmplId
            //qbr2.Call("value", temp_EmplId);

            //var qbr3 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30019);//LF_Sales
            int LF_SalesField = DynAx.GetFieldIdWithLock(EmplTable, "LF_Sales");
            var qbr3 = (AxaptaObject)axQueryDataSource2.Call("addRange", LF_SalesField);//LF_Sales
            qbr3.Call("value", "1");

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                //if (!DynRec2.get_Field("EmplId").ToString().StartsWith("H"))
                //{
                emplID[count] = DynRec2.get_Field("EmplId").ToString();
                temp_EmpName[count] = DynRec2.get_Field("Del_Name").ToString();
                temp_EmpEmail[count] = DynRec2.get_Field("DEL_Email").ToString();
                LF_EmpEmailID[count] = DynRec2.get_Field("LF_EmpEMailID").ToString();
                ReportTo[count] = DynRec2.get_Field("ReportTo").ToString();
                count++;
                //}
                DynRec2.Dispose();
            }
            return new Tuple<string[], int, string[], string[], int, string[], string[]>
                (temp_EmpName, count, temp_EmpEmail, emplID, count, LF_EmpEmailID, ReportTo);
        }

        public static Tuple<List<string>, string, int> GetCustTable(Axapta DynAx)
        {
            List<string> temp_Cust_Account = new List<string>();
            string temp_Emp_Id = "";
            int count = 0;

            int CustTable = 77;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustTable);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTable);

                temp_Cust_Account.Add(DynRec1.get_Field("AccountNum").ToString());
                temp_Emp_Id = DynRec1.get_Field("EmplId").ToString();
                count++;
                DynRec1.Dispose();
            }

            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            axQueryRun1.Dispose();
            return Tuple.Create(temp_Cust_Account, temp_Emp_Id, count);
        }

        public static Tuple<double, int, string> GetSalesOrder_OtherDiv(Axapta DynAx, string temp_CustAccount, string firstDayOfMonth, string currentDt)
        {
            double invoiceAmount = 0;
            int count = 0;
            string createdDt = "";

            DateTime startDate = DateTime.ParseExact(firstDayOfMonth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(currentDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            int SalesLine = 359;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesLine);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 28);//CustAccount
            qbr.Call("value", temp_CustAccount);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);//SalesStatus
                                                                           //qbr1.Call("value", "1");//backorder, label as open order
            qbr1.Call("value", "1");//not cancelled

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesLine);
                string SalesPrice = DynRec.get_Field("LineAmount").ToString();
                string salesid = DynRec.get_Field("SalesId").ToString();
                createdDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime")).ToString("dd/MM/yyyy");
                DateTime createdDate = DateTime.ParseExact(createdDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (createdDate >= startDate && createdDate <= endDate)
                {
                    if (!string.IsNullOrEmpty(SalesPrice))
                    {
                        invoiceAmount += Convert.ToDouble(SalesPrice);
                        count = count + 1;
                        //Debug.WriteLine(temp_CustAccount + " " + salesid + " " + createdDt + " " + SalesPrice);
                    }
                }

                DynRec.Dispose();
            }
            axQueryRun.Dispose();
            axQueryDataSource.Dispose();
            return new Tuple<double, int, string>(invoiceAmount, count, createdDt);
        }

        public static double getPeriodNameYearly_OtherDiv(Axapta DynAx, string temp_EmplId)
        {
            double MonthlyBudget = 0;
            double totalBudget = 0;
            string currentYear = DateTime.Now.Year.ToString();

            int LF_SalesBudget = 50168;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_SalesBudget);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 50006);//EmplId
            qbr2.Call("value", temp_EmplId);

            var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 50001); //PeriodName
            qbr.Call("value", currentYear + "-" + currentYear);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_SalesBudget);
                string periodname = DynRec2.get_Field("PeriodName").ToString();
                var month = periodname.Split('-');
                MonthlyBudget = Convert.ToDouble(DynRec2.get_Field("MonthlyBudget"));

                totalBudget = totalBudget + Convert.ToDouble(MonthlyBudget);

                DynRec2.Dispose();
            }
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            return totalBudget;
        }

        public static Tuple<string, string, string[], int, string, string> getEmpDetailsOtherDiv(Axapta DynAx, string temp_EmplId)
        {
            string temp_EmpName = ""; string temp_EmpEmail = "";
            int count = 0; string[] emplID = new string[10]; string LF_EmpEmailID = ""; string LF_SalesApprovalGroup = "";
            if (temp_EmplId != "")
            {
                int EmplTable = 103;
                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

                var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//EmplId
                qbr2.Call("value", temp_EmplId);
                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                    emplID[count] = DynRec2.get_Field("EmplId").ToString();
                    temp_EmpName = DynRec2.get_Field("Del_Name").ToString();
                    temp_EmpEmail = DynRec2.get_Field("DEL_Email").ToString();
                    LF_EmpEmailID = DynRec2.get_Field("LF_EmpEMailID").ToString();
                    LF_SalesApprovalGroup = DynRec2.get_Field("LF_SalesApprovalGroup").ToString();
                    count++;
                    DynRec2.Dispose();
                }
            }
            return new Tuple<string, string, string[], int, string, string>
                (temp_EmpName, temp_EmpEmail, emplID, count, LF_EmpEmailID, LF_SalesApprovalGroup);
        }

        public static Tuple<double, string, double, int, string> getAllSalesLineAmount(Axapta DynAx, string temp_CustAccount, string firstDayOfMonth, string currentDt)
        {
            int qtyOrdered = 0;
            double SalesPrice = 0; double lineAmount = 0;
            string SalesStatus = ""; string createdDt = "";
            double total = 0; double Mvalue = 0;
            string Txt = ""; string markupcategory = "";
            double currValue = 0;

            int SalesLine = 359;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesLine);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 28);//CustAccount
            qbr.Call("value", temp_CustAccount);

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 35);//SalesType
            qbr2.Call("value", "3");//sales, label as sales order

            var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);//SalesStatus
            qbr3.Call("value", "!=4");

            DateTime startDate = DateTime.ParseExact(firstDayOfMonth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(currentDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesLine);
                SalesPrice = Convert.ToDouble(DynRec.get_Field("SalesPrice").ToString());
                SalesStatus = DynRec.get_Field("SalesStatus").ToString();
                lineAmount = Convert.ToDouble(DynRec.get_Field("LineAmount"));
                qtyOrdered = Convert.ToInt32(DynRec.get_Field("QtyOrdered"));
                createdDt = Convert.ToDateTime(DynRec.get_Field("createdDateTime")).ToString("dd/MM/yyyy");//createddate between currentdate to the next month
                DateTime createdDate = DateTime.ParseExact(createdDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (createdDate >= startDate && createdDate <= endDate)
                {
                    int MarkupTrans = 230;
                    AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery.Call("addDataSource", MarkupTrans);
                    var qbr4 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);
                    qbr4.Call("value", "359");//transtable Id

                    AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun1.Call("Get", MarkupTrans);

                    Mvalue = Convert.ToDouble(DynRec2.get_Field("Value"));
                    Txt = DynRec2.get_Field("Txt").ToString();
                    markupcategory = DynRec2.get_Field("MarkupCategory").ToString();

                    if (markupcategory == "0")
                    {
                        if (Txt.Contains("add"))
                        {
                            currValue = currValue + Mvalue;
                        }
                        else
                        {
                            currValue = currValue - Mvalue;
                        }
                    }
                    else if (markupcategory == "1")
                    {
                        if (Txt.Contains("add"))
                        {
                            currValue = currValue + (Mvalue * qtyOrdered);
                        }
                        else
                        {
                            currValue = currValue - (Mvalue * qtyOrdered);
                        }
                    }
                    else if (markupcategory == "2")
                    {
                        if (Txt.Contains("add"))
                        {
                            currValue = currValue + (Mvalue / 100) * lineAmount;
                        }
                        else
                        {
                            currValue = currValue - (Mvalue / 100) * lineAmount;
                        }
                    }
                }

                total = lineAmount + currValue;

                DynRec.Dispose();
            }
            axQueryRun.Dispose();
            axQueryDataSource.Dispose();
            return new Tuple<double, string, double, int, string>
                (SalesPrice, SalesStatus, total, qtyOrdered, createdDt);
        }

        public static double getPeriodNameYearlyForMonthlyReport(Axapta DynAx, string startPeriodName, string endPeriodName, string temp_EmplId)
        {
            double MonthlyBudget = 0;
            double totalBudget = 0;
            string currentYear = DateTime.Now.Year.ToString();
            string currentMonth = DateTime.Now.Month.ToString();
            int oneMonthBefore = Convert.ToInt32(currentMonth) - 1;

            int LF_PPMSalesBudget = 30378;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_PPMSalesBudget);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30002);//EmplId
            qbr2.Call("value", temp_EmplId);

            //var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", 30001); //PeriodName
            //qbr.Call("value", startPeriodName + ".." + endPeriodName);
            //if (!string.IsNullOrEmpty(PeriodName))
            //{
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_PPMSalesBudget);
                string periodname = DynRec2.get_Field("PeriodName").ToString();
                var month = periodname.Split('-');
                MonthlyBudget = Convert.ToDouble(DynRec2.get_Field("MonthlyBudget"));
                if (periodname.Contains(currentYear))
                {
                    if (oneMonthBefore >= Convert.ToInt16(month[2]))
                    {
                        totalBudget = totalBudget + Convert.ToDouble(MonthlyBudget);
                    }
                }

                DynRec2.Dispose();
            }
            //}
            axQuery2.Dispose();
            axQueryDataSource2.Dispose();
            return totalBudget;
        }
    }
}