using Microsoft.Dynamics.BusinessConnectorNet;
using System;
namespace DotNet
{
    public class SFA_GET_SALES_ORDER_2
    {
        public static string get_validateSubmitted(Axapta DynAx, string temp_SalesId)
        {
            string validateSubmitted = "";
            int SalesTable = 366;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesTable);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//SalesId
            qbr.Call("value", temp_SalesId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesTable);
                validateSubmitted = DynRec.get_Field("LFI_Submitted").ToString();
                DynRec.Dispose();
            }
            return validateSubmitted;
        }

        public static Tuple<string[], int, string[]> get_Empl_Id(Axapta DynAx, string userid)
        {
            string[] Empl_Id = new string[10];
            string[] dimension = new string[10];
            int EmplTable = 103;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 12);//del_userId
            qbr1.Call("value", userid);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            int counterA = 0;
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                Empl_Id[counterA] = DynRec1.get_Field("Emplid").ToString();
                dimension[counterA] = DynRec1.get_Field("Dimension").ToString();
                counterA = counterA + 1;
                DynRec1.Dispose();
            }
            return new Tuple<string[], int, string[]>(Empl_Id, counterA, dimension);
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
                MPP = DynRec4.get_Field("LF_MixPricePromo").ToString();
                MPG = DynRec4.get_Field("LF_MixPriceGroup").ToString();
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

        public static string get_LFI_SalesStatus(Axapta DynAx, string temp_SalesId, string temp_SalesStatus)
        {
            string LFI_SalesStatus = temp_SalesStatus;
            if (temp_SalesStatus == "1")//Open order
            {
                int SalesLine = 359;
                AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

                AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", SalesLine);

                var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 1);//SalesId
                qbr6.Call("value", temp_SalesId);

                AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);
                while ((bool)axQueryRun6.Call("next"))
                {
                    AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", SalesLine);

                    string inventTransId = DynRec6.get_Field("inventTransId").ToString();
                    var totalDelivered = domComSalesLine.Call("getDeliveredInTotal", inventTransId);

                    int int_totalDelivered = Convert.ToInt32(totalDelivered);
                    if (int_totalDelivered > 0)//partial delivered
                    {
                        LFI_SalesStatus = "5";
                        DynRec6.Dispose();
                        goto DONE_get_LFI_SalesStatus;
                    }
                    DynRec6.Dispose();
                }
            }
            else
            {
                LFI_SalesStatus = temp_SalesStatus;
            }

        DONE_get_LFI_SalesStatus:
            return LFI_SalesStatus;
        }

        public static string get_delete_saleline(Axapta DynAx, string RecId)
        {
            string error = "";
            try
            {
                AxaptaRecord MySLRec = DynAx.CreateAxaptaRecord("SalesLine");

                DynAx.TTSBegin();
                MySLRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RecId", RecId));

                DynAx.TTSCommit(); DynAx.TTSAbort();
                return error;
            }
            catch (Exception)
            {
                DynAx.TTSAbort();
                error = RecId + " failed to delete. ";
                return error;
            }
        }

        public static string get_SalesStatus(Axapta DynAx, string SalesId)
        {
            string SalesStatus = "";
            int SalesTable = 366;
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", SalesTable);

            var qbr7 = (AxaptaObject)axQueryDataSource7.Call("addRange", 1);//SalesId
            qbr7.Call("value", SalesId);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", SalesTable);
                SalesStatus = DynRec5.get_Field("SalesStatus").ToString();
                DynRec5.Dispose();
            }
            return SalesStatus;
        }

        public static Tuple<string, string, string, string, string, string, string> get_CustInvoiceJour_details(Axapta DynAx, string InvoiceId)
        {
            string DelName = "";
            string DelAddress = "";
            string InvoiceAcc = "";
            string createdDt = "";
            string LF_WithAltAddress = "";
            string LessTotalDiscount = "";
            string InvoiceAmountTotal = "";

            int CustInvoiceJour = 62;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustInvoiceJour);
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 21);
            qbr1.Call("value", InvoiceId);//invoice Id
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustInvoiceJour);

                InvoiceAcc = DynRec1.get_Field("InvoiceAccount").ToString();
                createdDt = DynRec1.get_Field("createdDateTime").ToString();
                LF_WithAltAddress = DynRec1.get_Field("LF_WithAltAddress").ToString();//1: got alt address take de
                if (LF_WithAltAddress == "1")
                {
                    DelName = DynRec1.get_Field("DeliveryName").ToString();
                    DelAddress = DynRec1.get_Field("DELIVERYADDRESS").ToString();
                }
                double double_LessTotalDiscount = Convert.ToDouble(DynRec1.get_Field("EndDisc").ToString());
                LessTotalDiscount = double_LessTotalDiscount.ToString("#,###,###,##0.00");

                double double_InvoiceAmountTotal = Convert.ToDouble(DynRec1.get_Field("InvoiceAmount").ToString());
                InvoiceAmountTotal = double_InvoiceAmountTotal.ToString("#,###,###,##0.00");

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string>
                (DelName, DelAddress, InvoiceAcc, createdDt, LF_WithAltAddress, LessTotalDiscount, InvoiceAmountTotal);
        }

    }
}