using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace DotNet
{
    public class Payment_GET_Ledger_Journal_Table
    {
        //=============================================================================================================
        public static List<ListItem> getJournalNameList(Axapta DynAx)
        {
            List<ListItem> List_Journal_Name_Desc = new List<ListItem>();
            string getJournalName = "";
            string getDescription = "";

            int LedgerJournalName = 210;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LedgerJournalName);
            /*
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 9);
            qbr1.Call("value", "7");

            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);
            qbr1_2.Call("value", "!cia*");
            */
            axQueryDataSource1.Call("addSortField", 1, 0);//Journal Name, asc

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            List_Journal_Name_Desc.Add(new ListItem("-- SELECT --", ""));

            while ((bool)axQueryRun1.Call("next"))
            {

                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LedgerJournalName);
                getJournalName = DynRec1.get_Field("JournalName").ToString();
                getDescription = DynRec1.get_Field("Name").ToString();

                List_Journal_Name_Desc.Add(new ListItem("(" + getJournalName + ") " + getDescription, getJournalName));
                DynRec1.Dispose();
            }
            return List_Journal_Name_Desc;
        }

        public static string getJournalDescriptionExisting(Axapta DynAx, string JournalNum)
        {
            string getDescription = "";

            int LedgerJournalTable = 211;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LedgerJournalTable);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);
            qbr2.Call("value", JournalNum);


            axQueryDataSource2.Call("addSortField", 1, 1);//Journal Number, asc

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            if ((bool)axQueryRun2.Call("next"))
            {

                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LedgerJournalTable);

                getDescription = DynRec2.get_Field("Name").ToString();
                DynRec2.Dispose();
            }
            return getDescription;
        }

        public static string get_delete_duplicate_JN(Axapta DynAx, string UserId, string JournalNum)
        {
            string msg = "";
            int NumberSequenceList = 271;
            string getRecId = "";
            try
            {

                AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", NumberSequenceList);

                var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 4);
                qbr3.Call("value", UserId);

                var qbr3_1 = (AxaptaObject)axQueryDataSource3.Call("addRange", 1);
                qbr3_1.Call("value", JournalNum);

                var qbr3_2 = (AxaptaObject)axQueryDataSource3.Call("addRange", 12);//STATUS
                qbr3_2.Call("value", "Free");

                axQueryDataSource3.Call("addSortField", 61440, 1);//MODIFIEDDATETIME, desc

                AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
                if ((bool)axQueryRun3.Call("next"))
                {

                    AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", NumberSequenceList);

                    getRecId = DynRec3.get_Field("RECID").ToString();
                    DynRec3.Dispose();
                }
                else//nothing found
                {
                    msg = "There is no duplicate found.";
                    return msg;
                }

                AxaptaRecord MySLRec = DynAx.CreateAxaptaRecord("NumberSequenceList");
                DynAx.TTSBegin();
                MySLRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RecId", getRecId));

                DynAx.TTSCommit(); DynAx.TTSAbort();

                return msg;
            }
            catch (Exception)
            {
                DynAx.TTSAbort();
                msg = "Journal number, " + JournalNum + " with RecId, " + getRecId + " failed to clear duplicate. ";
                return msg;
            }
        }

        public static Tuple<string[], string, int> get_CustTable(Axapta DynAx, string temp_EmpId)
        {
            string[] temp_Cust_Account = new string[1000];
            string temp_Emp_Id = "";
            int count = 0;

            int CustTable = 77;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30033);//EmplId
            qbr1.Call("value", temp_EmpId);

            if (!temp_EmpId.Contains("-1"))
            {
                var qbr2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30033);//EmplId
                qbr2.Call("value", temp_EmpId);
            }

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTable);

                temp_Cust_Account[count] = DynRec1.get_Field("AccountNum").ToString();
                temp_Emp_Id = DynRec1.get_Field("EmplId").ToString();
                count++;
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            axQueryRun1.Dispose();
            return new Tuple<string[], string, int>(temp_Cust_Account, temp_Emp_Id, count);
        }
    }
}