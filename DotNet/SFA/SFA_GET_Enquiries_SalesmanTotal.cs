using GLOBAL_FUNCTION;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace DotNet
{
    public class SFA_GET_Enquiries_SalesmanTotal
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
            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            axQueryRun1.Dispose();
            return List_Salesman;
        }

        public static Tuple<double, string, string, int> ITotal_s(Axapta DynAx, string CustAcc, string StartQDate, string EndQDate)
        {
            string invoice = ""; int count = 0;
            string TableRun = ""; string TableSumTax = "";
            double ITotal = 0;

            int CustTrans = 78;
            try
            {
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

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

                var qbr2_5 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);// MSB_CNTYPE
                qbr2_5.Call("value", "0");

                var qbr2_6 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
                qbr2_6.Call("value", "1");

                var qbr2_7 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
                qbr2_7.Call("value", "2");

                var qbr2_8 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
                qbr2_8.Call("value", "3");

                var qbr2_9 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30025);
                qbr2_9.Call("value", "4");

                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                while ((bool)axQueryRun2.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CustTrans);
                    invoice = DynRec2.get_Field("invoice").ToString();
                    //Debug.WriteLine(invoice);
                    if (invoice != "")
                    {
                        int CustInvoiceJour = 62;
                        AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustInvoiceJour);

                        var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 21);//invoiceid
                        qbr3.Call("value", invoice);

                        var qbr3_1 = (AxaptaObject)axQueryDataSource3.Call("addRange", 2);//RefNum
                        qbr3_1.Call("value", "0");

                        var qbr3_2 = (AxaptaObject)axQueryDataSource3.Call("addRange", 2);
                        qbr3_2.Call("value", "2");

                        AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
                        if ((bool)axQueryRun3.Call("next"))
                        {
                            AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustInvoiceJour);
                            var length_str_invoice = invoice.Split('/');

                            if (length_str_invoice.Length == 2)//let no /suffix code total up
                            {
                                string invoice_code3 = length_str_invoice[1];
                                if ((invoice_code3 == "cn") || (invoice_code3 == "CN"))
                                {
                                    string MSB_CNType = DynRec3.get_Field("MSB_CNType").ToString();
                                    if ((MSB_CNType != "0") && (MSB_CNType != "1") && (MSB_CNType != "2") && (MSB_CNType != "3"))
                                    {
                                        //skip
                                        DynRec3.Dispose();

                                        goto SKIP;
                                    }
                                }
                                if ((invoice_code3 == "dn") || (invoice_code3 == "DN"))
                                {
                                    string MSB_DNType = DynRec3.get_Field("MSB_DNType").ToString();
                                    if ((MSB_DNType != "0") && (MSB_DNType != "1") && (MSB_DNType != "2") && (MSB_DNType != "3"))
                                    {
                                        //skip
                                        DynRec3.Dispose();

                                        goto SKIP;
                                    }
                                }
                                if ((invoice_code3 == "ER") || (invoice_code3 == "er") || (invoice_code3 == "WSDN"))
                                {
                                    //skip
                                    DynRec3.Dispose();

                                    goto SKIP;
                                }
                                //if ((invoice_code3 == "FINV") || (invoice_code3 == "finv"))
                                //{
                                //    //skip
                                //    DynRec3.Dispose();

                                //    goto SKIP;
                                //}//Sazila
                            }

                            double SalesBalance = Convert.ToDouble(DynRec3.get_Field("SalesBalance"));
                            double EndDisc = Convert.ToDouble(DynRec3.get_Field("EndDisc"));

                            double InvoiceAmount = SalesBalance - EndDisc;
                            double SumTax = Convert.ToDouble(DynRec3.get_Field("SumTax"));
                            double Amount = InvoiceAmount - SumTax;

                            ITotal = ITotal + Amount;

                            count++;
                            DynRec3.Dispose();
                        }
                        axQuery3.Dispose();
                        axQueryDataSource3.Dispose();
                        axQueryRun3.Dispose();
                    }
                SKIP:
                    qbr2.Dispose();
                    qbr2_2.Dispose();
                    qbr2_3.Dispose();
                    qbr2_4.Dispose();
                    qbr2_5.Dispose();
                    axQueryDataSource2.Dispose();
                    DynRec2.Dispose();
                }
                axQuery.Dispose();
                axQueryRun2.Dispose();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
            return new Tuple<double, string, string, int>(ITotal, TableRun, TableSumTax, count);
        }

        public static Tuple<double, string, string, int> get_ITotal_TableRun_s(Axapta DynAx, string SalesmanID, string startDate, string endDate)
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

            var qbr1 = (AxaptaObject)axQueryDataSource4.Call("addRange", 30033);//SalesmanID
            qbr1.Call("value", SalesmanID);

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

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            while ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                CustAcc = DynRec4.get_Field("AccountNum").ToString();

                temp_ITotal = 0;
                temp_TableRun = ""; temp_TableSumTax = "";
                if (CustAcc != "")
                {
                    //count1++;
                    var tuple_getTotal_func = ITotal_s(DynAx, CustAcc, startDate, endDate);
                    temp_ITotal = tuple_getTotal_func.Item1;
                    if (temp_ITotal != 0)
                    {
                        count1++ /*= count1 + tuple_getTotal_func.Item4*/;
                        //Debug.WriteLine("1 " + CustAcc + " " + temp_ITotal + " " + count1);
                    }
                    temp_TableRun = tuple_getTotal_func.Item2;
                    temp_TableSumTax = tuple_getTotal_func.Item3;
                }
                //temp_count = temp_count + count1;

                ITotal = ITotal + temp_ITotal;
                //TableRun = TableRun + temp_TableRun;
                //TableSumTax = TableSumTax + temp_TableSumTax;
                DynRec4.Dispose();
            }
            axQuery4.Dispose();
            axQueryDataSource4.Dispose();
            axQueryRun4.Dispose();
            axQuery.Dispose();
            return new Tuple<double, string, string, int>(ITotal, TableRun, TableSumTax, count1);
        }

        public static Tuple<double, int> getTotal(Axapta DynAx, string SalesmanID, string startDate, string endDate)
        {
            double Total = 0;
            string CustAcc = "";
            double temp_Total = 0;
            int totalAcc = 0;

            //SalesmanID = SalesmanID.Replace("-1", "");

            int CustTable = 77;

            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", CustTable);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 30033);
            qbr5.Call("value", SalesmanID);

            var qbr5_1 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_1.Call("value", "TDI");

            var qbr5_2 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_2.Call("value", "TDE");

            var qbr5_3 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_3.Call("value", "TDO");

            var qbr5_4 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);//CustGroup
            qbr5_4.Call("value", "!=TDP");

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            while ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", CustTable);
                CustAcc = DynRec5.get_Field("AccountNum").ToString();

                if (CustAcc != "")
                {
                    var tuple_getTotal_func = ITotal_s(DynAx, CustAcc, startDate, endDate);
                    temp_Total = tuple_getTotal_func.Item1;

                    Total = Total + temp_Total;
                    if (temp_Total != 0)
                    {
                        totalAcc++;
                        //Debug.WriteLine("2 " + CustAcc + " " + temp_Total + " " + totalAcc);
                    }
                    DynRec5.Dispose();
                }
            }
            axQuery5.Dispose();
            axQueryDataSource5.Dispose();
            qbr5 = null;
            qbr5_1 = null;
            return new Tuple<double, int>(Total, totalAcc);
        }

        public static Tuple<double, string, int> get_All_ITotal_TableRun_s(Axapta DynAx, string startDate, string endDate)
        {
            double ITotal = 0;
            string CustAcc = "";
            //string TableRun = ""; 
            string TableSumTax = "";

            double temp_ITotal = 0;
            //string temp_TableRun = "";
            //string temp_TableSumTax = "";

            int CustTable = 77;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

            var qbr4_1 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);//CustGroup
            qbr4_1.Call("value", "!=TDP");

            var qbr4_2 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);//CustGroup
            qbr4_2.Call("value", "!=OD");

            var qbr4_3 = (AxaptaObject)axQueryDataSource4.Call("addRange", 7);//CustGroup
            qbr4_3.Call("value", "!=sub");

            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            int count1 = 0;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            while ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                CustAcc = DynRec4.get_Field("AccountNum").ToString();

                temp_ITotal = 0;
                //temp_TableRun = ""; temp_TableSumTax = "";
                if (CustAcc != "")
                {
                    //count1++;
                    var tuple_getTotal_func = CustInvoiceTotal(DynAx, CustAcc, startDate, endDate);
                    temp_ITotal = tuple_getTotal_func.Item1;
                    if (temp_ITotal != 0)
                    {
                        count1++ /*= count1 + tuple_getTotal_func.Item4*/;
                    }
                    //temp_TableRun = tuple_getTotal_func.Item2;
                    //temp_TableSumTax = tuple_getTotal_func.Item3;
                }
                //temp_count = temp_count + count1;

                ITotal += temp_ITotal;
                //Debug.WriteLine(CustAcc + " " + temp_ITotal);

                //TableRun = TableRun + temp_TableRun;
                //TableSumTax = TableSumTax + temp_TableSumTax;
                DynRec4.Dispose();
            }
            axQuery4.Dispose();
            axQueryDataSource4.Dispose();
            axQueryRun4.Dispose();
            axQuery.Dispose();
            return new Tuple<double, string, int>(ITotal, TableSumTax, count1);
        }

        public static Tuple<double, int> getAllTotal(Axapta DynAx, string startDate, string endDate)
        {
            double Total = 0;
            string CustAcc = "";
            double temp_Total = 0;
            int totalAcc = 0;

            //SalesmanID = SalesmanID.Replace("-1", "");

            int CustTable = 77;

            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", CustTable);

            var qbr5_1 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_1.Call("value", "TDI");

            var qbr5_2 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_2.Call("value", "TDE");

            var qbr5_3 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);
            qbr5_3.Call("value", "TDO");

            var qbr5_4 = (AxaptaObject)axQueryDataSource5.Call("addRange", 7);//CustGroup
            qbr5_4.Call("value", "!=TDP");

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            while ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", CustTable);
                CustAcc = DynRec5.get_Field("AccountNum").ToString();

                if (!string.IsNullOrEmpty(CustAcc))
                {
                    var tuple_getTotal_func = ITotal_s(DynAx, CustAcc, startDate, endDate);
                    temp_Total = tuple_getTotal_func.Item1;

                    Total += temp_Total;
                    if (temp_Total != 0)
                    {
                        totalAcc++;
                        Debug.WriteLine("YTD " + CustAcc + " " + temp_Total + " " + totalAcc);
                    }
                    DynRec5.Dispose();
                }
            }
            axQuery5.Dispose();
            axQueryDataSource5.Dispose();
            qbr5_1 = null;
            return new Tuple<double, int>(Total, totalAcc);
        }

        public static Tuple<double, string, string, int> CustInvoiceTotal(Axapta DynAx, string CustAcc, string StartQDate, string EndQDate)
        {
            string invoice = "";
            string TableRun = "";
            string TableSumTax = "";
            double ITotal = 0;
            int count = 0;

            int CustInvoiceJour = 62;
            AxaptaObject axQueryDataSource = null;
            AxaptaObject qbr2_2 = null;
            AxaptaObject qbr2_3 = null;
            AxaptaObject qbr2_4 = null;

            try
            {
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustInvoiceJour);

                var qbr2_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 5);
                qbr2_1.Call("value", CustAcc);

                var var_StartQDate = DateTime.ParseExact(StartQDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var var_EndQDate = DateTime.ParseExact(EndQDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var var_Qdate = var_StartQDate + ".." + var_EndQDate + "*";

                qbr2_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30008);//LFI_invoicedate
                qbr2_2.Call("value", var_Qdate);

                qbr2_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 21);//invoiceid
                qbr2_3.Call("value", " !*ER*");

                qbr2_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 22);//ledgervoucher
                qbr2_4.Call("value", "!=*OR");

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun.Call("Get", CustInvoiceJour);

                    var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", 21); // invoiceid
                    invoice = DynRec2.get_Field("InvoiceId").ToString();
                    if (!string.IsNullOrEmpty(invoice))
                    {
                        //if (invoice == "80481232/INV")
                        //{
                        //    string invcodate = DynRec2.get_Field("LFI_InvoiceDate").ToString();
                        //    string lv = DynRec2.get_Field("LedgerVoucher").ToString();
                        //}
                        //AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                        //if ((bool)axQueryRun3.Call("next"))
                        //{
                        //AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustInvoiceJour);

                        var length_str_invoice = invoice.Split('/');
                        if (length_str_invoice.Length == 2)
                        {
                            string invoice_code3 = length_str_invoice[1];
                            if (invoice_code3.Equals("cn", StringComparison.OrdinalIgnoreCase) || invoice_code3.Equals("dn", StringComparison.OrdinalIgnoreCase))
                            {
                                string MSB_CNType = DynRec2.get_Field("MSB_CNType").ToString();
                                if (!IsValidMSBType(MSB_CNType))
                                {
                                    DynRec2.Dispose();
                                    goto SKIP;
                                }
                            }

                            if (invoice_code3.Equals("ER", StringComparison.OrdinalIgnoreCase) || invoice_code3.Equals("WSDN", StringComparison.OrdinalIgnoreCase))
                            {
                                DynRec2.Dispose();
                                goto SKIP;
                            }
                        }

                        double InvoiceAmount = Convert.ToDouble(DynRec2.get_Field("InvoiceAmount"));
                        double SumTax = Convert.ToDouble(DynRec2.get_Field("SumTax"));
                        double Amount = InvoiceAmount - SumTax;

                        ITotal += Amount;
                        count++;
                        //Debug.WriteLine(invoice + " " + InvoiceAmount);

                        DynRec2.Dispose();
                        //}
                        //axQueryRun3.Dispose();
                    }
                SKIP:
                    DynRec2.Dispose();
                }

                return new Tuple<double, string, string, int>(ITotal, TableRun, TableSumTax, count);
            }
            catch (Exception ex)
            {
                Function_Method.AddLog(ex.ToString());
                throw;
            }
            finally
            {
                if (qbr2_2 != null)
                    qbr2_2.Dispose();
                if (qbr2_3 != null)
                    qbr2_3.Dispose();
                if (qbr2_4 != null)
                    qbr2_4.Dispose();
                if (axQueryDataSource != null)
                    axQueryDataSource.Dispose();
            }
        }

        private static bool IsValidMSBType(string msbType)
        {
            string[] validTypes = { "0", "1", "2", "3", "4" };
            return validTypes.Contains(msbType);
        }

        private Tuple<string, string, string, string, string> getlf_gatepass(string SalesId)
        {
            string FixUserName = " ";
            string InvoiceDate = ""; string GatePass = ""; string GatePassDate = ""; string AccountNum = "";
            string AccountName = ""; string TransporterCode = "";

            Axapta DynAx = new Axapta();
            try
            {
                Function_Method.GlobalAxapta();

                int LF_GatePass = 40313;
                AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_GatePass);

                var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 40003);//InvoiceId
                qbr1.Call("value", SalesId);
                axQueryDataSource1.Call("addSortField", 40003, 1);//InvoiceId, desc

                AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                if ((bool)axQueryRun1.Call("next"))
                {
                    AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_GatePass);
                    InvoiceDate = DynRec1.get_Field("InvoiceDate").ToString();
                    GatePass = DynRec1.get_Field("GatePass").ToString();
                    GatePassDate = DynRec1.get_Field("GatePassDate").ToString();
                    AccountNum = DynRec1.get_Field("AccountNum").ToString();
                    AccountName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, AccountNum);
                    TransporterCode = DynRec1.get_Field("TransporterCode").ToString();
                    string TransporterName = DynRec1.get_Field("TransporterName").ToString();
                    DynRec1.Dispose();
                    return new Tuple<string, string, string, string, string>(InvoiceDate, GatePass, GatePassDate, AccountName, TransporterCode);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

    }
}