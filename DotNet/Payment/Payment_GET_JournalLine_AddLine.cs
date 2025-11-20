using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DotNet
{
    public class Payment_GET_JournalLine_AddLine
    {
        public static List<ListItem> get_AxCurrencies(Axapta DynAx)
        {
            List<ListItem> List_Currency = new List<ListItem>();

            int CurrencyTable = 47;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CurrencyTable);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            List_Currency.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CurrencyTable);
                string temp_CurrencyCode = DynRec1.get_Field("CurrencyCode").ToString();
                string temp_Txt = DynRec1.get_Field("Txt").ToString();

                List_Currency.Add(new ListItem("(" + temp_CurrencyCode + ") " + temp_Txt, temp_Txt));

                DynRec1.Dispose();
            }
            return List_Currency;
        }

        public static List<ListItem> get_AxDimension(Axapta DynAx, string sysDimension)
        {
            List<ListItem> List_Dimension = new List<ListItem>();

            int Dimensions = 91;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", Dimensions);

            var qbr_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 3);//sysDimension
            qbr_2.Call("value", sysDimension);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            List_Dimension.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", Dimensions);
                string temp_Num = DynRec2.get_Field("Num").ToString();
                string temp_Description = DynRec2.get_Field("Description").ToString();

                List_Dimension.Add(new ListItem("(" + temp_Num + ") " + temp_Description, temp_Description));

                DynRec2.Dispose();
            }
            return List_Dimension;
        }

        public static List<ListItem> get_AxBankTransType(Axapta DynAx)
        {
            List<ListItem> List_Bank = new List<ListItem>();

            int BankTransTypeTable = 12;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", BankTransTypeTable);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

            List_Bank.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", BankTransTypeTable);
                string temp_bankTransType = DynRec3.get_Field("BankTransType").ToString();
                string temp_bankTransName = DynRec3.get_Field("Name").ToString();

                List_Bank.Add(new ListItem("(" + temp_bankTransType + ") " + temp_bankTransName, temp_bankTransName));
                DynRec3.Dispose();
            }
            return List_Bank;
        }

        public static List<ListItem> get_AxBankcodes(Axapta DynAx)
        {
            List<ListItem> List_BankCode = new List<ListItem>();

            int BankCodeforCheque = 40266;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", BankCodeforCheque);

            var qbr_4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 40003);
            qbr_4.Call("value", "1");

            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);

            List_BankCode.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", BankCodeforCheque);
                string temp_BankCode = DynRec4.get_Field("BankCode").ToString();
                string temp_Description = DynRec4.get_Field("Description").ToString();
                List_BankCode.Add(new ListItem("(" + temp_BankCode + ") " + temp_Description, temp_Description));

                DynRec4.Dispose();
            }
            return List_BankCode;
        }

        public static List<ListItem> get_Branch(Axapta DynAx, string CustomerAccountB)
        {
            List<ListItem> List_BankBranchList = new List<ListItem>();

            int CustBankAccount = 50;
            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", CustBankAccount);

            var qbr_5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 25);
            qbr_5.Call("value", CustomerAccountB);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);

            int count = 0;
            while ((bool)axQueryRun5.Call("next"))
            {
                count = count + 1;
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", CustBankAccount);
                string temp_BankBranch = DynRec5.get_Field("Name").ToString();

                List_BankBranchList.Add(new ListItem(temp_BankBranch, count.ToString()));
                DynRec5.Dispose();
            }
            return List_BankBranchList;
        }

        public static string get_CustName(Axapta DynAx, string CustomerAccountB)
        {
            string CustName = "";

            int CustTable = 77;
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", CustTable);

            var qbr_6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 1);
            qbr_6.Call("value", CustomerAccountB);

            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", CustTable);
                CustName = DynRec6.get_Field("Name").ToString();
                DynRec6.Dispose();
            }
            else
            {
                CustName = "";
            }
            return CustName;
        }

        public static Tuple<string, string> get_JournalPosted_JournalCCReceived(Axapta DynAx, string JournalNum)
        {
            string Journal_Posted = "";
            string Journal_CCReceived = "";


            int LedgerJournalTable = 211;
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", LedgerJournalTable);

            var qbr = (AxaptaObject)axQueryDataSource7.Call("addRange", 1);//Journal Number
            qbr.Call("value", JournalNum);
            AxaptaObject axQueryRun7 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);

            if ((bool)axQueryRun7.Call("next"))//use if only one record
            {
                AxaptaRecord DynRec7 = (AxaptaRecord)axQueryRun7.Call("Get", LedgerJournalTable);

                Journal_Posted = DynRec7.get_Field("Posted").ToString();
                Journal_CCReceived = DynRec7.get_Field("CCReceived").ToString();

                DynRec7.Dispose();
            }
            return new Tuple<string, string>(Journal_Posted, Journal_CCReceived);
        }

        public static List<ListItem> get_CountDown()
        {
            List<ListItem> List_CountDown = new List<ListItem>();
            int counter = 0;

            List_CountDown.Add(new ListItem("-", ""));
            for (int i = 0; i < 30; i++)
            {
                counter = counter + 1;
                List_CountDown.Add(new ListItem(counter.ToString()));
            }

            return List_CountDown;
        }

        public static string Base_Enum_LedgerJournalACTypeForPaymProposal(int OffsetAccType)
        {
            string Base_Enum_OffsetAccType = "";

            switch (OffsetAccType)
            {
                case 0:// Ledger
                    Base_Enum_OffsetAccType = "Ledger";
                    break;
                case 6:// Bank
                    Base_Enum_OffsetAccType = "Bank";
                    break;
                case 250:// Bank
                    Base_Enum_OffsetAccType = "None";
                    break;
                default:
                    Base_Enum_OffsetAccType = "";
                    break;
            }
            return Base_Enum_OffsetAccType;
        }

        public static string get_CustAccount(Axapta DynAx, string EmplId)
        {
            string CustAccount = "";

            int CustTable = 77;
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", CustTable);

            var qbr_6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30033);
            qbr_6.Call("value", EmplId);

            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", CustTable);
                CustAccount = DynRec6.get_Field("AccountNum").ToString();
                DynRec6.Dispose();
            }
            else
            {
                CustAccount = "";
            }
            return CustAccount;
        }

    }
}