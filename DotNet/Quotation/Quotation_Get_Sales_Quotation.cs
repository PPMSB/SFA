using Microsoft.Dynamics.BusinessConnectorNet;
using System;

namespace DotNet
{
    public class Quotation_Get_Sales_Quotation
    {
        public static Tuple<string, string, string, string, string, string> reload_from_SalesQuotationTable(Axapta DynAx, string temp_SQId)
        {
            string temp_QuotationName = "";
            string temp_DeliveryDate = "";
            string temp_Cust_Account = "";
            string temp_Delivery_Addr = "";//"-same as primary address-";
            string temp_Emp_Id = "";

            //SO accordion-1
            string temp_QuotationID = "";

            int SalesQuotationTable = 1967;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", SalesQuotationTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//SalesId
            qbr1.Call("value", temp_SQId);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", SalesQuotationTable);

                temp_QuotationName = DynRec1.get_Field("QuotationName").ToString();
                temp_Cust_Account = DynRec1.get_Field("CustAccount").ToString();
                temp_DeliveryDate = DynRec1.get_Field("DeliveryDate").ToString();
                temp_Delivery_Addr = DynRec1.get_Field("DeliveryAddress").ToString();
                temp_QuotationID = DynRec1.get_Field("QuotationId").ToString();
                temp_Emp_Id = DynRec1.get_Field("SalesResponsible").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string>
                (temp_QuotationName, temp_Cust_Account, temp_DeliveryDate, temp_Delivery_Addr, temp_QuotationID, temp_Emp_Id);
        }

        public static Tuple<string, string, string, string, string, string> reload_from_SalesTableSet2(Axapta DynAx, string temp_SalesId)
        {
            //Delivery accordion-5
            string temp_hidden_Street = "";
            string temp_hidden_ZipCode = "";
            string temp_hidden_City = "";
            string temp_hidden_State = "";
            string temp_hidden_Country = "";
            string temp_hidden_QuotationId = "";

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
                temp_hidden_QuotationId = DynRec2.get_Field("QuotationId").ToString();

                DynRec2.Dispose();
            }
            return new Tuple<string, string, string, string, string, string>
                (temp_hidden_Street, temp_hidden_ZipCode, temp_hidden_City, temp_hidden_State, temp_hidden_Country, temp_hidden_QuotationId);
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

        public static string get_validateSubmitted(Axapta DynAx, string temp_SalesId)
        {
            string validateSubmitted = "";
            int SalesQuotationTable = 1967;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesQuotationTable);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//SalesId
            qbr.Call("value", temp_SalesId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesQuotationTable);
                validateSubmitted = DynRec.get_Field("LFI_Submitted").ToString();
                DynRec.Dispose();
            }
            return validateSubmitted;
        }

        public static Tuple<string> get_Empl_Id(Axapta DynAx, string userid)
        {
            string Empl_Id = "";
            int EmplTable = 103;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 19);//del_email
            qbr1.Call("value", userid);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                Empl_Id = DynRec1.get_Field("Emplid").ToString();
                DynRec1.Dispose();
            }
            return new Tuple<string>(Empl_Id);
        }

        public static string get_TaxItemGroup(Axapta DynAx, string ItemId)
        {
            string TaxItemGroup = "";
            int InventTableModule = 176;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", InventTableModule);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//ItemId
            qbr2.Call("value", ItemId);

            var qbr3 = (AxaptaObject)axQueryDataSource2.Call("addRange", 2);//ModuleType
            qbr3.Call("value", "Sales order");

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", InventTableModule);
                TaxItemGroup = DynRec2.get_Field("TaxItemGroupId").ToString();
                DynRec2.Dispose();
            }
            return TaxItemGroup;
        }

        public static Tuple<string, string> get_MPP_MPG(Axapta DynAx, string ItemId)
        {
            string MPP = "";
            string MPG = "";
            int InventTable = 175;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", InventTable);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 2);//ItemId
            qbr4.Call("value", ItemId);
            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            if ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", InventTable);
                MPP = DynRec4.get_Field("LFI_MixPricePromo").ToString();
                MPG = DynRec4.get_Field("LFI_MixPriceGroup").ToString();
                DynRec4.Dispose();
            }
            return new Tuple<string, string>(MPP, MPG);
        }

        public static string get_UOM(Axapta DynAx, string itemunit)
        {
            string UOM = "";
            int UnitTxt = 1196;
            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", UnitTxt);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 1);//itemunit
            qbr5.Call("value", itemunit);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", UnitTxt);
                UOM = DynRec5.get_Field("UnitIdTxt").ToString();
                DynRec5.Dispose();
            }
            return UOM;
        }

        public static string get_AxEnumSalesStatus(string temp_SalesStatus)
        {
            string AxEnumSalesStatus = "";
            switch (temp_SalesStatus)
            {
                case "0":
                    AxEnumSalesStatus = "";
                    break;
                case "1":
                    AxEnumSalesStatus = "Open Order";
                    break;
                case "2":
                    AxEnumSalesStatus = "Delivered";
                    break;
                case "3":
                    AxEnumSalesStatus = "Invoiced";
                    break;
                case "4":
                    AxEnumSalesStatus = "Cancelled";
                    break;
                case "5":
                    AxEnumSalesStatus = "Partially Delivered";
                    break;
                default:
                    AxEnumSalesStatus = "";
                    break;
            }

            return AxEnumSalesStatus;
        }

        public static string get_delete_saleline(Axapta DynAx, string RecId)
        {
            string error = "";
            try
            {
                AxaptaRecord MySLRec = DynAx.CreateAxaptaRecord("SalesQuotationLine");

                DynAx.TTSBegin();
                MySLRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RecId", RecId));

                DynAx.TTSCommit();
                DynAx.TTSAbort();
                return error;
            }
            catch (Exception)
            {
                DynAx.TTSAbort();
                error = RecId + " failed to delete. ";
                return error;
            }
        }

        public static string get_QuotationStatus(Axapta DynAx, string QuotationId)
        {
            string QuotationStatus = "";
            int SalesQuotationTable = 1967;
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", SalesQuotationTable);

            var qbr7 = (AxaptaObject)axQueryDataSource7.Call("addRange", 1);//QuotationId
            qbr7.Call("value", QuotationId);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", SalesQuotationTable);
                QuotationStatus = DynRec5.get_Field("QuotationStatus").ToString();
                DynRec5.Dispose();
            }
            return QuotationStatus;
        }

        public static int get_QuotationIsExist(Axapta DynAx, string CustAccount)
        {
            string[] lineNum = new string[100];
            int count = 0;
            int salesQuotationLine = 1962;
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", salesQuotationLine);

            var qbr7 = (AxaptaObject)axQueryDataSource7.Call("addRange", 56);//CustAccount
            qbr7.Call("value", CustAccount);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);
            AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", salesQuotationLine);

            while ((bool)axQueryRun5.Call("next"))
            {
                count++;
            }
            return count;
        }
    }
}