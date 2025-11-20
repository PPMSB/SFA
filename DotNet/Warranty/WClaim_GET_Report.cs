using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;

namespace DotNet
{
    public class WClaim_GET_Report
    {
        public static Tuple<string, string, int, string, string> getItem(Axapta DynAx, string ClaimNumber)
        {
            string Description = ""; string itemId = ""; int Qty = 0; string custDO = "";
            string serialNumber = "";
            int LF_WarrantyLine = DynAx.GetTableId("LF_WarrantyLine"); 
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//Claim Number
            qbr1.Call("value", ClaimNumber);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);
                Description = DynRec1.get_Field("Name").ToString();
                itemId = DynRec1.get_Field("ItemID").ToString();
                Qty = Convert.ToInt16(DynRec1.get_Field("Qty"));
                custDO = DynRec1.get_Field("CustDO").ToString();
                custDO = DynRec1.get_Field("SerialNumber").ToString();
                DynRec1.Dispose();
            }
            return new Tuple<string, string, int, string, string>(Description, itemId, Qty, custDO, serialNumber);
        }

        public static List<string> getItemList(Axapta DynAx, string ClaimNumber)
        {
            List<string> Description = new List<string>();
            int LF_WarrantyLine = DynAx.GetTableId("LF_WarrantyLine");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//Claim Number
            qbr1.Call("value", ClaimNumber);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);
                Description.Add(DynRec1.get_Field("Name").ToString());
                DynRec1.Dispose();
            }
            return Description;
        }
        public static int getTotalSold(Axapta DynAx, string ItemId, string fromdt, string todt)
        {
            int totalSold = 0; int tempTotalSold = 0;
            int InventTrans = 177;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", InventTrans);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);
            qbr.Call("value", ItemId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);//DatePhysical
            qbr1.Call("value", fromdt + ".." + todt);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", InventTrans);
                totalSold = Convert.ToInt16(DynRec.get_Field("Qty"));
                tempTotalSold = tempTotalSold + totalSold;
            }
            return tempTotalSold;
        }

        public static Tuple<string, int> getTotalClaim(Axapta DynAx, string temp_ItemID)
        {
            int LF_WarrantyLine = DynAx.GetTableId("LF_WarrantyLine"); 
            int ItemID = DynAx.GetFieldId(LF_WarrantyLine, "ItemID");
            string itemId = ""; int Qty = 0; int tempQty = 0;

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", ItemID);//Item ID
            qbr1.Call("value", temp_ItemID);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);
                itemId = DynRec1.get_Field("ItemID").ToString();
                Qty = Convert.ToInt16(DynRec1.get_Field("Qty"));
                tempQty = tempQty + Qty;
            }
            return new Tuple<string, int>(itemId, tempQty);
        }

        public static string get_rmaCreatedDate(Axapta DynAx, string warrantyID)
        {
            string SalesStatus = ""; string date = "";
            int SalesTable = 366;
            AxaptaObject axQuery7 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource7 = (AxaptaObject)axQuery7.Call("addDataSource", SalesTable);

            var qbr7 = (AxaptaObject)axQueryDataSource7.Call("addRange", 1);//SalesId
            qbr7.Call("value", warrantyID);

            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery7);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", SalesTable);
                SalesStatus = DynRec5.get_Field("CustomerRef").ToString();
                date = Convert.ToDateTime(DynRec5.get_Field("createdDateTime")).ToString("dd/MM/yyyy");
                DynRec5.Dispose();
            }
            return date;
        }

        public static Tuple<string, string> get_SalesId(Axapta DynAx, string temp_RmaId)
        {
            string salesStatus = "";
            string createdDt = "";
            int SalesTable = 366;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesTable);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 58);//ReturnItemNum
            qbr.Call("value", temp_RmaId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if (temp_RmaId != "")
            {
                if ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesTable);
                    var getCreatedDt = get_CustInvoiceJour_details(DynAx, DynRec.get_Field("SalesId").ToString());
                    createdDt = getCreatedDt.Item4;
                    salesStatus = DynRec.get_Field("SalesStatus").ToString();
                    DynRec.Dispose();
                }
            }

            return new Tuple<string, string>(createdDt, salesStatus);
        }

        public static Tuple<string, string, string, string, string, string, string> get_CustInvoiceJour_details(Axapta DynAx, string SalesId)
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
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 3);
            qbr1.Call("value", SalesId);//sales Id
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
            return new Tuple<string, string, string, string, string, string, string>(DelName, DelAddress, InvoiceAcc, createdDt, LF_WithAltAddress, LessTotalDiscount, InvoiceAmountTotal);
        }

        public static Tuple<string, int> getTotalClaimStatus(Axapta DynAx, string claimStat)
        {
            string claimNum = ""; int count = 0;
            string TableName = "LF_WarrantyTable";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "ClaimStatus");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", tableId);

            var pending = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//claimstat
            pending.Call("value", claimStat);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                claimNum = DynRec1.get_Field("ClaimNumber").ToString();
                count++;
            }

            return new Tuple<string, int>(claimNum, count);
        }
    }
}