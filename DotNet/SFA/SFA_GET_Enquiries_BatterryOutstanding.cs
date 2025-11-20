using GLOBAL_FUNCTION;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DotNet
{
    public class SFA_GET_Enquiries_BatteryOutstanding
    {
        public static Tuple<string, string> getEmplId(Axapta DynAx, string CustAcc)
        {
            string getEmplId = ""; string getSalesman = "";

            int CustTable = 77;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);
            qbr1.Call("value", CustAcc);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTable);
                getEmplId = DynRec1.get_Field("EmplId").ToString();
                getSalesman = DynRec1.get_Field("EmplName").ToString();
                string companyName = DynRec1.get_Field("Name").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string>(getEmplId, getSalesman);
        }

        public static string getCust(Axapta DynAx, string CustAcc)
        {
            string getCust = "";

            int CustTable = 77;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 1);
            qbr4.Call("value", CustAcc);
            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);

            if ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);

                getCust = DynRec4.get_Field("Name").ToString();
                DynRec4.Dispose();
            }
            return getCust;
        }

        public static Tuple<List<ListItem>, List<ListItem>, List<ListItem>, List<ListItem>, List<ListItem>> get_BatteryOutstandingList(Axapta DynAx, int system_checking)
        {
            string CUSTACCOUNT = "";
            string SERIALNUM = "";
            string INVOICEID = "";
            string INVOICEDATE = "";
            string DATAAREAID = "";
            string CustName = "";

            List<ListItem> List_CUSTACCOUNT = new List<ListItem>();
            List<ListItem> List_SERIALNUM = new List<ListItem>();
            List<ListItem> List_INVOICEID = new List<ListItem>();
            List<ListItem> List_INVOICEDATE = new List<ListItem>();
            List<ListItem> List_CustName = new List<ListItem>();

            int LF_AUTOSERIALTRANS = 30179;
            int LF_GATEPASS = 40313;

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_AUTOSERIALTRANS);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30011);//UPDATEDDATE
            qbr2.Call("value", "");

            var qbr2_1 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30014);//LF_REBATEDATE
            qbr2_1.Call("value", "");

            /*
            var qbr2_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 61448);//DATAAREAID
            qbr2_2.Call("value", "ppm");
            */
            axQueryDataSource2.Call("addSortField", 30003, 1);//SalesID , dsc

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            int count = 0;
            while ((bool)axQueryRun2.Call("next"))
            {
                CUSTACCOUNT = "";
                SERIALNUM = "";
                INVOICEID = "";
                INVOICEDATE = "";
                DATAAREAID = "";

                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_AUTOSERIALTRANS);

                DATAAREAID = DynRec2.get_Field("DATAAREAID").ToString();
                INVOICEID = DynRec2.get_Field("INVOICEID").ToString();

                AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", LF_GATEPASS);

                var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 40017);//UPDATEDDATE
                qbr3.Call("value", "1");

                var qbr3_1 = (AxaptaObject)axQueryDataSource3.Call("addRange", 61448);//DATAAREAID
                qbr3_1.Call("value", DATAAREAID);

                var qbr3_2 = (AxaptaObject)axQueryDataSource3.Call("addRange", 40003);//INVOICEID
                qbr3_2.Call("value", INVOICEID);

                axQueryDataSource3.Call("addSortField", 40004, 1);//SalesID , dsc

                AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
                while ((bool)axQueryRun3.Call("next"))
                {
                    //reset
                    CUSTACCOUNT = "";
                    SERIALNUM = "";
                    INVOICEID = "";
                    INVOICEDATE = "";
                    CustName = "";
                    //
                    AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", LF_GATEPASS);
                    INVOICEDATE = DynRec3.get_Field("INVOICEDATE").ToString();

                    string[] arr_temp_INVOICEDATE = INVOICEDATE.Split(' ');//date + " " + time;
                    string Raw_INVOICEDATE = arr_temp_INVOICEDATE[0];
                    INVOICEDATE = Function_Method.get_correct_date(system_checking, Raw_INVOICEDATE, true);
                    var var_INVOICEDATE = DateTime.ParseExact(INVOICEDATE, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);// in lotus use cdat

                    string TodayDate = DateTime.Now.ToString("dd/MM/yyyy");//default
                    TodayDate = Function_Method.get_correct_date(system_checking, TodayDate, false);

                    var var_TodayDate = DateTime.ParseExact(TodayDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);// in lotus use cdat

                    double diff2 = (var_TodayDate - var_INVOICEDATE).TotalDays;

                    if (diff2 > 180)//var_INVOICEDATE > var_TodayDate
                    {
                        count = count + 1;
                        if (count > 30)
                        {
                            goto END;
                        }
                        CUSTACCOUNT = DynRec2.get_Field("CUSTACCOUNT").ToString();
                        SERIALNUM = DynRec2.get_Field("SERIALNUM").ToString();
                        INVOICEID = DynRec3.get_Field("INVOICEID").ToString();

                        CustName = getCust(DynAx, CUSTACCOUNT);

                        List_CUSTACCOUNT.Add(new ListItem(CUSTACCOUNT, count.ToString()));
                        List_SERIALNUM.Add(new ListItem(SERIALNUM, count.ToString()));
                        List_INVOICEID.Add(new ListItem(INVOICEID, count.ToString()));
                        List_INVOICEDATE.Add(new ListItem(INVOICEDATE, count.ToString()));
                        List_CustName.Add(new ListItem(CustName, count.ToString()));
                    }
                    DynRec3.Dispose();
                }
                DynRec2.Dispose();
            }
        END:
            return new Tuple<List<ListItem>, List<ListItem>, List<ListItem>, List<ListItem>, List<ListItem>>(List_CUSTACCOUNT, List_SERIALNUM, List_INVOICEID, List_INVOICEDATE, List_CustName);

        }
    }
}