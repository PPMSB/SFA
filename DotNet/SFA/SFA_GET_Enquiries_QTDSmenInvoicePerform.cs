using GLOBAL_FUNCTION;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Diagnostics;


namespace DotNet
{
    public class SFA_GET_Enquiries_QTDSmenInvoicePerform
    {
        public static List<ListItem> getSalesman(Axapta DynAx)
        {
            List<ListItem> List_Salesman = new List<ListItem>();
            string getSalesmanId = "";
            string getSalesmanName = "";

            int emplTable = 103;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", emplTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30004);
            qbr1.Call("value", "0");//'Field Id 30004 = EmplStatus; 0 = Active

            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30009);
            qbr1_2.Call("value", "0");// 'LF_StopEMail YesNo

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            List_Salesman.Add(new ListItem("-- SELECT --", ""));

            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", emplTable);
                getSalesmanId = DynRec1.get_Field("emplId").ToString();
                getSalesmanName = DynRec1.get_Field("DEL_NAME").ToString();

                string cleanName = getSalesmanName.Contains('(')
                ? getSalesmanName.Split('(')[0].Trim()
                : getSalesmanName.Trim();

                if (getSalesmanId == "0301-1" || getSalesmanId == "6021-1")
                {
                    List_Salesman.Add(new ListItem(cleanName + "(" + getSalesmanId + ")", getSalesmanId));
                }
                else
                {
                    if (!(getSalesmanId.EndsWith("-1") || getSalesmanId.EndsWith("-2") || getSalesmanId.EndsWith("-3") || getSalesmanId.EndsWith("-4")))
                    {
                        // Add if it does NOT end with -1, -2, -3, or -4
                        List_Salesman.Add(new ListItem(cleanName + "(" + getSalesmanId + ")", getSalesmanId));
                    }
                    else if (getSalesmanId.StartsWith("1002") || getSalesmanId.StartsWith("1003"))
                    {
                        // Add if it starts with 1002 or 1003 regardless of suffix
                        List_Salesman.Add(new ListItem(cleanName + "(" + getSalesmanId + ")", getSalesmanId));
                    }
                }
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            axQueryRun1.Dispose();
            return List_Salesman;
        }
        public static List<Tuple<double, string>> getLF_PPM_SalesInvCategory(Axapta DynAx, string YearRange)
        {
            // List to store all ValueFrom and Category pairs
            List<Tuple<double, string>> results = new List<Tuple<double, string>>();

            int LF_PPMSalesInc_Criteria = DynAx.GetTableIdWithLock("LF_PPMSalesInc_Criteria");
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_PPMSalesInc_Criteria);

            int PeriodName = DynAx.GetFieldId(LF_PPMSalesInc_Criteria, "PeriodName");
            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", PeriodName);

            qbr1.Call("value", YearRange);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_PPMSalesInc_Criteria);
                double valueFrom = Convert.ToDouble(DynRec.get_Field("ValueFrom"));
                string category = DynRec.get_Field("Category").ToString();

                // Add the pair to the list
                results.Add(new Tuple<double, string>(valueFrom, category));
            }

            // If less than 3 entries exist, this method still returns whatever it finds.
            return results;
        }

        public static Int32 getLF_PPM_SalesInc_Master(Axapta DynAx, string YearRange, string email)
        {
            int ActualSales = 0;

            int LF_PPMSalesInc_Master = DynAx.GetTableIdWithLock("LF_PPMSalesInc_Master");
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_PPMSalesInc_Master);

            int PeriodName = DynAx.GetFieldId(LF_PPMSalesInc_Master, "PeriodName");
            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", PeriodName);

            qbr1.Call("value", YearRange);

            int LF_EmpEMailID = DynAx.GetFieldId(LF_PPMSalesInc_Master, "LF_EmpEMailID");
            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", LF_EmpEMailID);

            qbr2.Call("value", email);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_PPMSalesInc_Master);
                ActualSales = Convert.ToInt32(DynRec.get_Field("ActualSales"));
            }
            return ActualSales;
        }


    }
}
