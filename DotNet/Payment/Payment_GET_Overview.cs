using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
namespace DotNet
{
    public class Payment_GET_Overview
    {
        public static string Customer(Axapta DynAx, string JournalNum)
        {
            string Customer = "";

            int LedgerJournalTrans = 212;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LedgerJournalTrans);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//JournalNum
            qbr2.Call("value", JournalNum);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            int count = 0;
            string temp_Customer = "";

            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LedgerJournalTrans);
                string temp_CustAccNo = DynRec2.get_Field("AccountNum").ToString();

                if (count == 0)//first count
                {
                    temp_Customer = "(" + temp_CustAccNo + ")" + CustName(DynAx, temp_CustAccNo);
                }
                else
                {
                    temp_Customer = temp_Customer + "<br>" + "(" + temp_CustAccNo + ")" + CustName(DynAx, temp_CustAccNo);
                }
                count = count + 1;
                DynRec2.Dispose();
            }
            Customer = temp_Customer;
            return Customer;
        }

        public static string CustName(Axapta DynAx, string Cust_Acc)
        {
            string Cust_Name = "";

            int CustTable = 77;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustTable);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 1);//JournalNum
            qbr3.Call("value", Cust_Acc);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustTable);
                Cust_Name = DynRec3.get_Field("Name").ToString();
                DynRec3.Dispose();
            }
            return Cust_Name;
        }

        public static Tuple<List<string>, string, int> getAllCustomer(Axapta DynAx, string temp_EmpId)
        {
            List<string> temp_Cust_Account = new List<string>(); // Kai Xuan 2025-03-10 Change from string[500] to ArrayList
            string temp_Emp_Id = "";
            int count = 0;

            int CustTable = 77;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", CustTable);

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30033);//EmplId
            qbr2.Call("value", temp_EmpId);

            var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);//CustGroup
            qbr4_2.Call("value", "TDI");

            var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_3.Call("value", "TDE");

            var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_4.Call("value", "TDO");

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                if (temp_Cust_Account.Count > 500)
                {
                    break; // Kai Xuan 2025-03-10 Limit temp_Cust_Account to 500 records
                }
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTable);

                temp_Cust_Account.Add(DynRec1.get_Field("AccountNum").ToString());
                temp_Emp_Id = DynRec1.get_Field("EmplId").ToString();

                count++;
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun1.Dispose();
            return new Tuple<List<string>, string, int>(temp_Cust_Account, temp_Emp_Id, count);

        }

        public static Tuple<string, string, string, string> getLF_eInvoiceGov(Axapta DynAx, string temp_InvoiceID)
        {
            string LF_UUID = "";
            string LF_LONGID = "";
            string TIN = "";
            string SST = "";

            int LF_EinvoiceGov = DynAx.GetTableId("LF_EinvoiceGov");
            int InvoiceId = DynAx.GetFieldId(LF_EinvoiceGov, "InvoiceId");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", LF_EinvoiceGov);

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", InvoiceId);//InvoiceID
            qbr2.Call("value", temp_InvoiceID);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_EinvoiceGov);

                LF_UUID = DynRec1.get_Field("LF_UUID").ToString();
                LF_LONGID = DynRec1.get_Field("LF_LONGID").ToString();
                TIN = DynRec1.get_Field("BuyerTIN").ToString();
                SST = DynRec1.get_Field("BuyerSST").ToString();
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun1.Dispose();
            return new Tuple<string, string, string, string>(LF_UUID, LF_LONGID, TIN, SST);
        }

        public static Tuple<string, string, string, string> getLF_eInvoiceGov_LFStatusIs5(Axapta DynAx, string temp_InvoiceID)
        {
            string LF_UUID = "";
            string LF_LONGID = "";
            string TIN = "";
            string SST = "";

            int LF_EinvoiceGov = DynAx.GetTableId("LF_EinvoiceGov");
            int InvoiceId = DynAx.GetFieldId(LF_EinvoiceGov, "InvoiceId");
            int LF_Status = DynAx.GetFieldId(LF_EinvoiceGov, "LF_Status");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", LF_EinvoiceGov);

            var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", InvoiceId);//InvoiceID
            qbr2.Call("value", temp_InvoiceID);

            var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", LF_Status);//16.6.2025 -Status must be "5". Request from Foo Zhi Ming
            qbr3.Call("value", "5");

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_EinvoiceGov);

                LF_UUID = DynRec1.get_Field("LF_UUID").ToString();
                LF_LONGID = DynRec1.get_Field("LF_LONGID").ToString();
                TIN = DynRec1.get_Field("BuyerTIN").ToString();
                SST = DynRec1.get_Field("BuyerSST").ToString();
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun1.Dispose();
            return new Tuple<string, string, string, string>(LF_UUID, LF_LONGID, TIN, SST);
        }
    }
}