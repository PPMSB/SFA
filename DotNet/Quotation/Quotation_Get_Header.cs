using Microsoft.Dynamics.BusinessConnectorNet;
using System;

namespace DotNet
{
    public class Quotation_Get_Header
    {
        //All the "get" method for Sales Header here==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        /*
        get_PaymTermName
        get_SuffixCode
        get_AltAddress
        get_AltAddress_info
        get_customer_acc
        */
        public static string get_PaymTermName(Axapta DynAx, string temp_PaymTermId)
        {
            string PaymTermName = "";
            int PAYMTERM = 276;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", PAYMTERM);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//PAYMTERMID
            qbr1.Call("value", temp_PaymTermId);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", PAYMTERM);
                PaymTermName = DynRec1.get_Field("DESCRIPTION").ToString();
                DynRec1.Dispose();
            }
            return PaymTermName;
        }

        public static Tuple<string[], string[], int> get_AltAddress(Axapta DynAx, string temp_CustomeNo)
        {

            int CustTable = 77;
            int DirPartyAddressRelationShip = 2596;
            int DirPartyAddressRelationShipMapping = 1066;
            int Address = 1;

            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", CustTable);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 1);//ACCOUNTNUM
            qbr3.Call("value", temp_CustomeNo);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            string PartyId = "";
            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", CustTable);
                PartyId = DynRec3.get_Field("PartyId").ToString();
                DynRec3.Dispose();
            }
            //
            string[] DirPtAddrRelRecId = new string[20];
            int counter = 0;
            if (PartyId != "")
            {
                AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", DirPartyAddressRelationShip);

                var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 1);//PartyId
                qbr4.Call("value", PartyId);

                AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);

                while ((bool)axQueryRun4.Call("next"))
                {
                    counter = counter + 1;
                    AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", DirPartyAddressRelationShip);

                    DirPtAddrRelRecId[counter - 1] = DynRec4.get_Field("RecId").ToString();
                    DynRec4.Dispose();
                }
            }
            else
            {
                return null;//no alternate address since no PartyId
            }
            //
            string[] AddressRecId = new string[counter];
            for (int i = 0; i < counter; i++)
            {
                string temp_DirPtAddrRelRecId = DirPtAddrRelRecId[i];

                AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", DirPartyAddressRelationShipMapping);

                var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 10);//DirPtAddrRelRecId
                qbr5.Call("value", temp_DirPtAddrRelRecId);

                AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);


                if ((bool)axQueryRun5.Call("next"))
                {

                    AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", DirPartyAddressRelationShipMapping);
                    string RefCompanyId = DynRec5.get_Field("RefCompanyId").ToString();

                    if (RefCompanyId != "")
                    {
                        AddressRecId[i] = DynRec5.get_Field("AddressRecId").ToString();
                    }
                    else
                    {
                        AddressRecId[i] = "";
                    }
                    DynRec5.Dispose();
                }
            }
            //
            string[] AltAddress = new string[counter];
            for (int i = 0; i < counter; i++)
            {
                string temp_AddressRecId = AddressRecId[i];

                AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", Address);

                var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 65534);//RecId
                qbr6.Call("value", temp_AddressRecId);

                AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);


                if ((bool)axQueryRun6.Call("next"))
                {

                    AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", Address);

                    AltAddress[i] = DynRec6.get_Field("Address").ToString();

                    DynRec6.Dispose();
                }
            }
            return new Tuple<string[], string[], int>(AltAddress, AddressRecId, counter);
        }

        public static Tuple<string, string, string, string, string> get_AltAddress_info(Axapta DynAx, string temp_AddressRecID)
        {
            string temp_hidden_Street = ""; string temp_hidden_ZipCode = ""; string temp_hidden_City = "";
            string temp_hidden_State = ""; string temp_hidden_Country = "";
            int Address = 1;

            AxaptaObject axQuery8 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource8 = (AxaptaObject)axQuery8.Call("addDataSource", Address);

            var qbr8 = (AxaptaObject)axQueryDataSource8.Call("addRange", 65534);//RecId
            qbr8.Call("value", temp_AddressRecID);

            AxaptaObject axQueryRun8 = DynAx.CreateAxaptaObject("QueryRun", axQuery8);
            if ((bool)axQueryRun8.Call("next"))
            {
                AxaptaRecord DynRec8 = (AxaptaRecord)axQueryRun8.Call("Get", Address);

                temp_hidden_Street = DynRec8.get_Field("Street").ToString();
                temp_hidden_ZipCode = DynRec8.get_Field("ZipCode").ToString();
                temp_hidden_City = DynRec8.get_Field("City").ToString();
                temp_hidden_State = DynRec8.get_Field("State").ToString();
                temp_hidden_Country = DynRec8.get_Field("CountryRegionId").ToString();
                DynRec8.Dispose();
            }
            return new Tuple<string, string, string, string, string>(temp_hidden_Street, temp_hidden_ZipCode, temp_hidden_City, temp_hidden_State, temp_hidden_Country);
        }

        public static string get_customer_acc(Axapta DynAx, string temp_CusId)
        {
            string customer_name = "";

            int CustTable = 77;
            AxaptaObject axQuery9 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource9 = (AxaptaObject)axQuery9.Call("addDataSource", CustTable);

            var qbr9 = (AxaptaObject)axQueryDataSource9.Call("addRange", 1);//Customer Name
            qbr9.Call("value", temp_CusId);

            AxaptaObject axQueryRun9 = DynAx.CreateAxaptaObject("QueryRun", axQuery9);
            if ((bool)axQueryRun9.Call("next"))//use if only one record
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun9.Call("Get", CustTable);
                customer_name = DynRec.get_Field("Name").ToString();
            }
            return customer_name;
        }
    }
}