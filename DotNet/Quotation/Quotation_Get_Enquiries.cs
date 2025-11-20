using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DotNet
{
    public class Quotation_Get_Enquiries
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

                List_Salesman.Add(new ListItem("(" + getSalesmanId + ") " + getSalesmanName, getSalesmanId));
                DynRec1.Dispose();
            }
            return List_Salesman;
        }

        public static Tuple<double, string, string> ITotal_s(Axapta DynAx, string CustAcc, string StartQDate, string EndQDate)
        {
            string invoice = "";
            string TableRun = ""; string TableSumTax = "";
            double ITotal = 0;

            int CustTrans = 78;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", CustTrans);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);
            qbr2.Call("value", CustAcc);

            var var_StartQDate = DateTime.ParseExact(StartQDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);// in lotus use cdat
            var var_EndQDate = DateTime.ParseExact(EndQDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);// in lotus use cdat

            var var_Qdate = var_StartQDate + ".." + var_EndQDate;
            var qbr2_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 2);
            qbr2_2.Call("value", var_Qdate);

            var qbr2_3 = (AxaptaObject)axQueryDataSource2.Call("addRange", 4);
            qbr2_3.Call("value", " !*ER*");

            var qbr2_4 = (AxaptaObject)axQueryDataSource2.Call("addRange", 3);
            qbr2_4.Call("value", " !=*OR");

            var qbr2_5 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
            qbr2_5.Call("value", "0");

            var qbr2_6 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
            qbr2_6.Call("value", "1");

            var qbr2_7 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
            qbr2_7.Call("value", "2");

            var qbr2_8 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
            qbr2_8.Call("value", "3");

            var qbr2_9 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
            qbr2_9.Call("value", "4");

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            while ((bool)axQueryRun2.Call("next"))
            {

                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CustTrans);
                invoice = DynRec2.get_Field("invoice").ToString();
                if (invoice != "")
                {
                    int CustInvoiceJour = 62;
                    AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustInvoiceJour);

                    var qbr3 = (AxaptaObject)axQueryDataSource2.Call("addRange", 21);
                    qbr3.Call("value", invoice);

                    var qbr3_1 = (AxaptaObject)axQueryDataSource2.Call("addRange", 2);
                    qbr3_1.Call("value", "0");

                    var qbr3_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 2);
                    qbr3_2.Call("value", "2");
                    AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
                    if ((bool)axQueryRun3.Call("next"))
                    {
                        AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustInvoiceJour);
                        int length_str_invoice = invoice.Length;
                        string invoice_code3 = invoice.Substring(length_str_invoice - 3);

                        if ((invoice_code3 == "/cn") || (invoice_code3 == "/CN"))
                        {
                            string MSB_CNType = DynRec3.get_Field("MSB_CNType").ToString();
                            if ((MSB_CNType != "0") && (MSB_CNType != "1") && (MSB_CNType != "2") && (MSB_CNType != "3"))
                            {
                                //skip
                                goto SKIP;
                            }
                        }
                        if ((invoice_code3 == "/dn") || (invoice_code3 == "/DN"))
                        {
                            string MSB_DNType = DynRec3.get_Field("MSB_DNType").ToString();
                            if ((MSB_DNType != "0") && (MSB_DNType != "1") && (MSB_DNType != "2") && (MSB_DNType != "3"))
                            {
                                //skip
                                goto SKIP;
                            }
                        }
                        string invoice_code2 = invoice.Substring(length_str_invoice - 2);
                        if ((invoice_code3 == "ER") || (invoice_code3 == "er"))
                        {
                            //skip
                            goto SKIP;
                        }

                        string invoice_code4 = invoice.Substring(length_str_invoice - 4);
                        if ((invoice_code3 == "FINV") || (invoice_code3 == "finv"))
                        {
                            //skip
                            goto SKIP;
                        }

                        double InvoiceAmount = Convert.ToDouble(DynRec3.get_Field("InvoiceAmount"));
                        double SumTax = Convert.ToDouble(DynRec3.get_Field("SumTax"));
                        double Amount = InvoiceAmount - SumTax;
                        TableRun = TableRun + "\n" + invoice + "_" + InvoiceAmount.ToString();
                        TableSumTax = TableSumTax + "\n" + invoice + "_" + SumTax.ToString();
                        ITotal = ITotal + Amount;
                        DynRec3.Dispose();
                    }
                }

            SKIP:

                DynRec2.Dispose();
            }
            return new Tuple<double, string, string>(ITotal, TableRun, TableSumTax);
        }

        public static Tuple<double, string, string> get_ITotal_TableRun_s(Axapta DynAx, string SalesmanID, string startDate, string endDate)
        {
            double ITotal = 0;
            string CustAcc = "";
            string TableRun = ""; string TableSumTax = "";

            double temp_ITotal = 0;
            string temp_TableRun = "";
            string temp_TableSumTax = "";

            int CustTable = 77;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 30033);//SalesmanID
            qbr4.Call("value", SalesmanID);

            var qbr4_1 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);//CustGroup
            qbr4_1.Call("value", "!=TDP");

            var qbr4_2 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);
            qbr4_2.Call("value", "TDI");

            var qbr4_3 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);
            qbr4_3.Call("value", "TDE");

            var qbr4_4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);
            qbr4_4.Call("value", "TDO");

            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            int count1 = 0;
            while ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                CustAcc = DynRec4.get_Field("AccountNum").ToString();

                temp_ITotal = 0;
                temp_TableRun = ""; temp_TableSumTax = "";
                if (CustAcc != "")
                {
                    var tuple_getTotal_func = ITotal_s(DynAx, CustAcc, startDate, endDate);
                    temp_ITotal = tuple_getTotal_func.Item1;
                    temp_TableRun = tuple_getTotal_func.Item2;
                    temp_TableSumTax = tuple_getTotal_func.Item3;
                }
                ITotal = ITotal + temp_ITotal;
                TableRun = TableRun + temp_TableRun;
                TableSumTax = TableSumTax + temp_TableSumTax;
                count1 = count1 + 1;
                DynRec4.Dispose();
            }

            return new Tuple<double, string, string>(ITotal, TableRun, TableSumTax);
        }

        public static double getTotal(Axapta DynAx, string SalesmanID, string startDate, string endDate)
        {
            double Total = 0;
            string CustAcc = "";

            double temp_Total = 0;

            SalesmanID = SalesmanID.Replace("-1", "");

            int CustTable = 77;

            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", CustTable);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 30033);
            qbr5.Call("value", SalesmanID + "*");

            var qbr5_1 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_1.Call("value", "TDI");

            var qbr5_2 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_2.Call("value", "TDE");

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            while ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", CustTable);
                CustAcc = DynRec5.get_Field("AccountNum").ToString();
                temp_Total = 0;

                if (CustAcc != "")
                {
                    var tuple_getTotal_func = ITotal_s(DynAx, CustAcc, startDate, endDate);
                    temp_Total = tuple_getTotal_func.Item1;

                    Total = Total + temp_Total;

                    DynRec5.Dispose();
                }
            }
            return Total;
        }

        public static Tuple<string, string, string> getEmpDetails(Axapta DynAx, string temp_EmplId)
        {
            string temp_EmpName = ""; string temp_EmpContact = ""; string temp_EmpEmail = "";
            if (temp_EmplId != "")
            {
                int EmplTable = 103;
                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", EmplTable);

                var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//temp_EmplId
                qbr2.Call("value", temp_EmplId);
                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                if ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", EmplTable);
                    temp_EmpName = DynRec2.get_Field("Del_Name").ToString();
                    temp_EmpContact = DynRec2.get_Field("DEL_Phone").ToString();
                    temp_EmpEmail = DynRec2.get_Field("DEL_Email").ToString();
                    DynRec2.Dispose();
                }
            }
            return new Tuple<string, string, string>(temp_EmpName, temp_EmpContact, temp_EmpEmail);
        }

    }
}