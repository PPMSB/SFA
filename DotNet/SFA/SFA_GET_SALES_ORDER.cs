using Microsoft.Dynamics.BusinessConnectorNet;
using System;

namespace DotNet
{
    public class SFA_GET_SALES_ORDER
    {
        public static string get_CustAcc(Axapta DynAx, string temp_SalesId)
        {
            string CustAcc = "";

            int SalesTable = 366;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesTable);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//SalesId
            qbr.Call("value", temp_SalesId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesTable);
                CustAcc = DynRec.get_Field("CustAccount").ToString();
                DynRec.Dispose();
            }

            return CustAcc;
        }

        public static Tuple<string, string, string, string, string, string> reload_from_SalesTable(Axapta DynAx, string temp_SalesId)
        {
            //Customer accordion-1           
            string temp_References = "";

            //Delivery accordion-3
            string temp_DeliveryDate = "";
            string temp_Receiver_Name = "";
            string temp_Delivery_Addr = "";//"-same as primary address-";
            string Status = "";
            //SO accordion-1
            string temp_Notes = "";

            int SalesTable = 366;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", SalesTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//SalesId
            qbr1.Call("value", temp_SalesId);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", SalesTable);

                temp_References = DynRec1.get_Field("CustomerRef").ToString();
                temp_Receiver_Name = DynRec1.get_Field("DeliveryName").ToString();
                temp_DeliveryDate = DynRec1.get_Field("DeliveryDate").ToString();
                temp_Delivery_Addr = DynRec1.get_Field("DeliveryAddress").ToString();
                temp_Notes = DynRec1.get_Field("Notes").ToString();
                Status = DynRec1.get_Field("SalesStatus").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string>(temp_References, temp_DeliveryDate, temp_Receiver_Name, temp_Delivery_Addr, temp_Notes, Status);

        }

        public static Tuple<string, string, string, string, string, string, string> reload_from_SalesTableSet2(Axapta DynAx, string temp_SalesId)
        {
            //Delivery accordion-5
            string temp_hidden_Street = "";
            string temp_hidden_ZipCode = "";
            string temp_hidden_City = "";
            string temp_hidden_State = "";
            string temp_hidden_Country = "";
            string AutoPost = "";
            string temp_ResetCampaignID = "";
            int SalesTable = 366;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", SalesTable);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//SalesId
            qbr2.Call("value", temp_SalesId);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", SalesTable);

                temp_hidden_Street = DynRec2.get_Field("DeliveryStreet").ToString();
                temp_hidden_ZipCode = DynRec2.get_Field("DeliveryZipCode").ToString();
                temp_hidden_City = DynRec2.get_Field("DeliveryCity").ToString();
                temp_hidden_State = DynRec2.get_Field("DeliveryState").ToString();
                temp_hidden_Country = DynRec2.get_Field("DeliveryCountryRegionId").ToString();
                AutoPost = DynRec2.get_Field("LF_AutoPost").ToString();
                temp_ResetCampaignID = DynRec2.get_Field("LF_CampaignID").ToString();

                DynRec2.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string>(temp_hidden_Street, temp_hidden_ZipCode, temp_hidden_City, temp_hidden_State, temp_hidden_Country, AutoPost, temp_ResetCampaignID);
        }


        public static string get_SLUnit(Axapta DynAx, string temp_ItemId)
        {
            string SLUnit = "";

            /*ModuleType"   
            0 = Invent 
            1 = Purch
            2 = Sales 
            3 = SmmQuotation*/
            int InventTableModule = 176; int ModuleType = 2;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", InventTableModule);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 1);//ItemId
            qbr3.Call("value", temp_ItemId);

            var qbr4 = (AxaptaObject)axQueryDataSource3.Call("addRange", ModuleType);//ModuleType
            qbr4.Call("value", "Sales order");

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", InventTableModule);

                string temp_unit = DynRec3.get_Field("UnitId").ToString();
                if (temp_unit != "0")
                {
                    SLUnit = temp_unit;
                }

                DynRec3.Dispose();
            }

            return SLUnit;
        }

        public static string get_sanitize(string myStr)
        {
            string sanitize_str = "";
            int length_myStr = myStr.Length;
            if (length_myStr > 0)
            {
                if (myStr.Substring(0, 1) == ",")
                {
                    myStr = myStr.Substring(1);//remove "," at first substring if have
                }
            }
            length_myStr = myStr.Length;
            if (length_myStr > 0)
            {
                if (myStr.Substring(length_myStr - 1, 1) == ",")
                {
                    myStr = myStr.Substring(0, length_myStr - 1);//remove "," at last substring if have
                }
            }

            sanitize_str = myStr.Replace("=", " | ");
            return sanitize_str;
        }

        public static string get_SLUnitName(Axapta DynAx, string itemunit)
        {
            string SLUnitName = "";
            int Unit = 485;
            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", Unit);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 1);//itemunit
            qbr5.Call("value", itemunit);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", Unit);
                SLUnitName = DynRec5.get_Field("Txt").ToString();
                DynRec5.Dispose();
            }
            return SLUnitName;
        }

        public static string get_MultipleQty(Axapta DynAx, string itemId)
        {
            string MultipleQty = "";
            int inventItemSalesSetup = 1764;
            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", inventItemSalesSetup);

            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 1);//ItemId
            qbr6.Call("value", itemId);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun5.Call("Get", inventItemSalesSetup);
                string temp_MultipleQty = DynRec6.get_Field("MultipleQty").ToString();
                if (temp_MultipleQty != "0")
                {
                    MultipleQty = temp_MultipleQty;
                }

                DynRec6.Dispose();
            }
            return MultipleQty;
        }

        public static string get_LowestQty(Axapta DynAx, string itemId)
        {
            string LowestQty = "0";
            int inventItemSalesSetup = 1764;
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", inventItemSalesSetup);

            var qbr7 = (AxaptaObject)axQueryDataSource7.Call("addRange", 1);//ItemId
            qbr7.Call("value", itemId);

            AxaptaObject axQueryRun7 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);
            if ((bool)axQueryRun7.Call("next"))
            {
                AxaptaRecord DynRec7 = (AxaptaRecord)axQueryRun7.Call("Get", inventItemSalesSetup);
                LowestQty = DynRec7.get_Field("LowestQty").ToString();

                DynRec7.Dispose();
            }
            return LowestQty;
        }

        public static Tuple<string, string, string, int, int, int> get_SaleslineDet(Axapta DynAx, string temp_SalesId)
        {
            int tableid;
            tableid = DynAx.GetTableId("SalesLine");
            string itemName = ""; string itemId = ""; string unit = "";
            int dlvQty = 0; int rmainderQty = 0; int qty = 0;

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", tableid);

            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 1);//SalesId
            qbr6.Call("value", temp_SalesId);

            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun6.Call("Get", tableid);
                itemName = DynRec.get_Field("Name").ToString();
                itemId = DynRec.get_Field("ItemId").ToString();
                unit = DynRec.get_Field("SalesUnit").ToString();
                qty = Convert.ToInt32(DynRec.get_Field("SalesQty"));
                dlvQty = Convert.ToInt32(DynRec.get_Field("RemainSalesPhysical"));

                AxaptaObject SalesLinetype = DynAx.CreateAxaptaObject("SalesLinetype");
                var temp_DeliveredInTotal = SalesLinetype.Call("deliveredInTotal");
                rmainderQty = Convert.ToInt32(temp_DeliveredInTotal);
                DynRec.Dispose();
            }

            return new Tuple<string, string, string, int, int, int>(itemName, itemId, unit, qty, dlvQty, rmainderQty);
        }
    }
}