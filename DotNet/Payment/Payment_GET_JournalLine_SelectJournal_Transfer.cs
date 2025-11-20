using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;


namespace DotNet
{
    public class Payment_GET_JournalLine_SelectJournal_Transfer
    {
        public static Tuple<string, string> get_OffsetAccountType_OffsetAccount(Axapta DynAx, string JournalNum)
        {
            string OffsetAccountType = "";
            string OffsetAccount = "";
            int LedgerJournalTable = 211;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LedgerJournalTable);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//JournalNum
            qbr2.Call("value", JournalNum);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LedgerJournalTable);
                OffsetAccountType = DynRec2.get_Field("OffsetAccountType").ToString();
                OffsetAccount = DynRec2.get_Field("OffsetAccount").ToString();

                DynRec2.Dispose();
            }
            return new Tuple<string, string>(OffsetAccountType, OffsetAccount);
        }

        public static string get_PostDateChq(Axapta DynAx, string JournalNum)
        {
            string PostDateChq = "";

            int LedgerJournalTable = 211;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LedgerJournalTable);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//JournalNum
            qbr2.Call("value", JournalNum);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LedgerJournalTable);
                PostDateChq = DynRec2.get_Field("OffsetAccountType").ToString();


                DynRec2.Dispose();
            }
            return PostDateChq;
        }

        public static string get_emplid(Axapta DynAx, string CustAccNum)
        {
            string emplid = "";

            int CustTable = 77;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustTable);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 1);//CustAccNum
            qbr3.Call("value", CustAccNum);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustTable);
                emplid = DynRec3.get_Field("Emplid").ToString();
                DynRec3.Dispose();
            }
            return emplid;
        }

        public static string get_AxBankTransTypeFirstData(Axapta DynAx)
        {
            string BankTransTypeFirstData = "";

            int BankTransTypeTable = 12;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", BankTransTypeTable);

            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);

            if ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", BankTransTypeTable);
                BankTransTypeFirstData = DynRec4.get_Field("BankTransType").ToString();

                DynRec4.Dispose();
            }
            return BankTransTypeFirstData;
        }

        public static string get_DuplicatePaymRef(Axapta DynAx, string PaymReference, string temp_DocumentDate)
        {
            string DuplicatePaymRef = "";

            AxaptaObject domComCustPayment = DynAx.CreateAxaptaObject("DomComCustPayment");
            temp_DocumentDate = Function_Method.get_correct_date(GLOBAL.system_checking, temp_DocumentDate, false);
            var var_temp_DocumentDate = DateTime.ParseExact(temp_DocumentDate, "dd/MM/yyyy", null);// in lotus use cdat

            DuplicatePaymRef = domComCustPayment.Call("chkDuplicateChqNum", PaymReference, var_temp_DocumentDate).ToString();

            return DuplicatePaymRef;
        }

        public static string get_JournalPosted(Axapta DynAx, string JournalNum)
        {
            string JournalPosted = "";

            int LedgerJournalTable = 211;
            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", LedgerJournalTable);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 1);//JournalNum
            qbr5.Call("value", JournalNum);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);

            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", LedgerJournalTable);
                JournalPosted = DynRec5.get_Field("Posted").ToString();

                DynRec5.Dispose();
            }
            return JournalPosted;
        }

        public static string getJournalNameByJournalNum(Axapta DynAx, string JournalNum)
        {
            string JournalName = "";

            int LedgerJournalTable = 211;
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LedgerJournalTable);

            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 1);//JournalNum
            qbr6.Call("value", JournalNum);

            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LedgerJournalTable);
                JournalName = DynRec6.get_Field("JournalName").ToString();

                DynRec6.Dispose();
            }
            return JournalName;
        }

        public static string getAxEnumLedgerJournalACType(string OffsetAccountType)
        {
            string AxEnumLedgerJournalACType = "";
            switch (OffsetAccountType)
            {
                case "0":
                    AxEnumLedgerJournalACType = "Ledger";
                    break;
                case "1":
                    AxEnumLedgerJournalACType = "Customer";
                    break;
                case "2":
                    AxEnumLedgerJournalACType = "Vendor";
                    break;
                case "3":
                    AxEnumLedgerJournalACType = "Project";
                    break;
                case "5":
                    AxEnumLedgerJournalACType = "Fixed assets";
                    break;
                case "6":
                    AxEnumLedgerJournalACType = "Bank";
                    break;

                case "7":
                    AxEnumLedgerJournalACType = "Outdated";
                    break;

                default:
                    AxEnumLedgerJournalACType = "";
                    break;
            }

            return AxEnumLedgerJournalACType;
        }

        public static string getSettlementType(int SettleVoucher)
        {
            string SettlementType = "";
            switch (SettleVoucher)
            {
                case 0:
                    SettlementType = "None";
                    break;
                case 1:
                    SettlementType = "OpenTransact";
                    break;
                case 2:
                    SettlementType = "SelectedTransact";
                    break;

                default:
                    SettlementType = "";
                    break;
            }

            return SettlementType;

        }
    }
}