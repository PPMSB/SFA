using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DotNet
{
    public class EventBudget_GET_NewApplicant
    {
        public static Tuple<List<ListItem>, string, string> getEvent_EmpID(Axapta DynAx, string Email)
        {
            string[] EventNumber = new string[100];
            string LF_EmpEmailID = "";
            string EmplName = "";
            int count = 0;
            List<ListItem> eventCode = new List<ListItem>();
            int LF_WebEvent_EmpID = 30507;

            AxaptaObject axQuery9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource9 = (AxaptaObject)axQuery9.Call("addDataSource", LF_WebEvent_EmpID);

            var qbr9 = (AxaptaObject)axQueryDataSource9.Call("addRange", 30002);//Email
            qbr9.Call("value", Email);

            AxaptaObject axQueryRun9 = DynAx.CreateAxaptaObject("QueryRun", axQuery9);

            while ((bool)axQueryRun9.Call("next"))
            {
                AxaptaRecord DynRec9 = (AxaptaRecord)axQueryRun9.Call("Get", LF_WebEvent_EmpID);

                EventNumber[count] = (DynRec9.get_Field("EventNumber").ToString().Trim());
                eventCode.Add(new ListItem(EventNumber[count]));

                LF_EmpEmailID = DynRec9.get_Field("LF_EmpEMailID").ToString().Trim();
                EmplName = DynRec9.get_Field("EmplName").ToString().Trim();

                count = count + 1;
                DynRec9.Dispose();
            }

            return new Tuple<List<ListItem>, string, string>(eventCode, LF_EmpEmailID, EmplName);
        }

        public static Tuple<string, string, string, string, string> getParticipantDet(Axapta DynAx, string EmplID)
        {
            string custname = "";
            string custacc = "";
            string NoofParticipant = "";
            string loyaltyPoint = "";
            string ApPoint = "";

            int LF_WebParticipant = 30366;

            AxaptaObject axQuery9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource9 = (AxaptaObject)axQuery9.Call("addDataSource", LF_WebParticipant);

            var qbr9 = (AxaptaObject)axQueryDataSource9.Call("addRange", 30008);//EmplID
            qbr9.Call("value", EmplID);

            AxaptaObject axQueryRun9 = DynAx.CreateAxaptaObject("QueryRun", axQuery9);

            if ((bool)axQueryRun9.Call("next"))
            {
                AxaptaRecord DynRec9 = (AxaptaRecord)axQueryRun9.Call("Get", LF_WebParticipant);

                custname = DynRec9.get_Field("CustName").ToString();
                custacc = DynRec9.get_Field("CustAccount").ToString();

                NoofParticipant = DynRec9.get_Field("NoOfParticipant").ToString().Trim();
                loyaltyPoint = DynRec9.get_Field("LoyaltyPoint").ToString().Trim();

                ApPoint = DynRec9.get_Field("APPoint").ToString().Trim();

                DynRec9.Dispose();
            }

            return new Tuple<string, string, string, string, string>(custname, custacc, NoofParticipant,
                                                                            loyaltyPoint, ApPoint);
        }

        public static Tuple<string, string, string, string, string, string> getParticipant(Axapta DynAx, string EmplID)
        {
            string eventDt = "";
            string lastInvoiceId = "";
            string goldPendant = "";
            string voucherCheque = "";
            string numOfRoom = "";
            string vegePax = "";

            int LF_WebParticipant = DynAx.GetTableId("LF_WebParticipant");
            AxaptaObject axQuery9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource9 = (AxaptaObject)axQuery9.Call("addDataSource", LF_WebParticipant);

            var qbr9 = (AxaptaObject)axQueryDataSource9.Call("addRange", 30008);//EmplID
            qbr9.Call("value", EmplID);

            AxaptaObject axQueryRun9 = DynAx.CreateAxaptaObject("QueryRun", axQuery9);

            if ((bool)axQueryRun9.Call("next"))
            {
                AxaptaRecord DynRec9 = (AxaptaRecord)axQueryRun9.Call("Get", LF_WebParticipant);

                eventDt = DynRec9.get_Field("EventDate").ToString();
                lastInvoiceId = DynRec9.get_Field("LastInvoiceID").ToString();

                goldPendant = DynRec9.get_Field("GoldPendant").ToString().Trim();
                voucherCheque = DynRec9.get_Field("VoucherCheque").ToString().Trim();

                numOfRoom = DynRec9.get_Field("NumOfRoom").ToString().Trim();
                vegePax = DynRec9.get_Field("VegePax").ToString().Trim();

                DynRec9.Dispose();
            }

            return new Tuple<string, string, string, string, string, string>(eventDt, lastInvoiceId, goldPendant,
                                                                            voucherCheque, numOfRoom, vegePax);
        }

        public static Tuple< string, string> getWebEvent(Axapta DynAx, string EventNumber)
        {
            string EventLocation = "";
            string EventDate = "";
            int count = 0;
            List<ListItem> eventCode = new List<ListItem>();
            int LF_WebEvent_EmpID = DynAx.GetTableId("LF_WebEvent");
            int fieldId = DynAx.GetFieldId(LF_WebEvent_EmpID, "EventNumber");
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WebEvent_EmpID);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//eventlocation
            qbr1.Call("value", EventNumber);

            AxaptaObject axQueryRun9 = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            while ((bool)axQueryRun9.Call("next"))
            {
                AxaptaRecord DynRec9 = (AxaptaRecord)axQueryRun9.Call("Get", LF_WebEvent_EmpID);

                EventLocation = DynRec9.get_Field("EventLocation").ToString().Trim();
                EventDate = DynRec9.get_Field("EventDate").ToString().Trim();

                count = count + 1;
                DynRec9.Dispose();
            }

            return new Tuple<string, string>(EventLocation, EventDate);
        }
    }
}