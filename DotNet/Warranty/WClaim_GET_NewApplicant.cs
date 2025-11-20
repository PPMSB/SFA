using GLOBAL_FUNCTION;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static DotNetSync.SyncGatepassAxToDotNetV2;
namespace DotNet
{
    public class WClaim_GET_NewApplicant
    {
        //=============================================================================================================
        public static Tuple<string, string> getItemId(Axapta DynAx, string ItemId)
        {
            string getItemId = ""; string itemName = "";
            int InventTable = DynAx.GetTableIdWithLock("InventTable");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", InventTable);

            int ItemNameField = DynAx.GetFieldId(InventTable, "ItemName");

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", ItemNameField);
            qbr1.Call("value", ItemId);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", InventTable);
                getItemId = DynRec1.get_Field("ItemId").ToString();
                itemName = DynRec1.get_Field("ItemName").ToString();
                DynRec1.Dispose();
            }
            return new Tuple<string, string>(getItemId, itemName);
        }

        public static string get_BatteryUnit(Axapta DynAx, string temp_ItemId)
        {
            string Unit = "";

            /*ModuleType"   
            0 = Inventory 
            1 = Purch
            2 = Sales 
            3 = SmmQuotation*/
            int InventTableModule = 176; int ModuleType = 2;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", InventTableModule);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 1);//ItemId
            qbr3.Call("value", temp_ItemId);

            //var qbr4 = (AxaptaObject)axQueryDataSource3.Call("addRange", ModuleType);//ModuleType
            //qbr4.Call("value", "Inventory");

            var qbr5 = (AxaptaObject)axQueryDataSource3.Call("addRange", ModuleType);//ModuleType
            qbr5.Call("value", "Sales");

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            while ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", InventTableModule);

                string temp_unit = DynRec3.get_Field("UnitId").ToString();
                if (temp_unit != "0")
                {
                    if (Unit != "")
                    {
                        string[] arr_raw_Unit = Unit.Split('/');
                        int counter = arr_raw_Unit.Count();
                        for (int i = 0; i < counter; i++)
                        {
                            if (temp_unit != arr_raw_Unit[i])
                            {
                                Unit = Unit + "/" + temp_unit;
                            }
                        }
                    }
                    else
                    {
                        Unit = temp_unit;
                    }
                }
                DynRec3.Dispose();
            }
            return Unit;
        }

        public static string getWarehouse(Axapta DynAx, string CustAcc)
        {
            string getWarehouse = "";

            int CustTable = 77;
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 1);//custAcc
            qbr4.Call("value", CustAcc);
            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            if ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                getWarehouse = DynRec4.get_Field("InventLocation").ToString();
                DynRec4.Dispose();
            }
            return getWarehouse;
        }

        public static bool getCheckItemExist(Axapta DynAx, string CustAcc)
        {
            bool getCheckItemExist = false;

            int LF_AutoSerialTrans = 30179;
            AxaptaObject axQuery5 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource5 = (AxaptaObject)axQuery5.Call("addDataSource", LF_AutoSerialTrans);

            var qbr5 = (AxaptaObject)axQueryDataSource5.Call("addRange", 30004);
            qbr5.Call("value", CustAcc);
            AxaptaObject axQueryRun5 = DynAx.CreateAxaptaObject("QueryRun", axQuery5);
            if ((bool)axQueryRun5.Call("next"))
            {
                AxaptaRecord DynRec5 = (AxaptaRecord)axQueryRun5.Call("Get", LF_AutoSerialTrans);
                getCheckItemExist = true;
                DynRec5.Dispose();
            }
            return getCheckItemExist;
        }

        public static string getWarrantyProcessStatus(Axapta DynAx, string claimNumber)
        {
            //Jerry 2024-11-05 Avoid hardcode
            //int LF_WarrantyTable = 50773;
            //int LF_WarrantyTable = 30661; //live
            int LF_WarrantyTable = DynAx.GetTableIdWithLock("LF_WarrantyTable");

            string ProcessStatus = "";

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WarrantyTable);

            //Jerry 2024-11-05 Avoid hardcode
            //var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30001);//claimNumber
            int cn1 = DynAx.GetFieldId(LF_WarrantyTable, "ClaimNumber");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

            qbr6.Call("value", claimNumber);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WarrantyTable);
                ProcessStatus = DynRec6.get_Field("ProcessStatus").ToString();

                DynRec6.Dispose();
            }
            return ProcessStatus;
        }

        public static string CheckUserName(Axapta DynAx, string UserId)
        {
            string UserName = UserId;
            AxaptaRecord DynRec;
            string TableName = "UserInfo";
            string fieldName = ("networkAlias");
            //string fieldValue = ("your_search_criteria_here");

            // Define the record
            DynRec = DynAx.CreateAxaptaRecord(TableName);
            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", fieldName, UserId));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                UserName = (string)DynRec.get_Field("name");
            }
            DynRec.Dispose();
            return UserName;
        }

        public static string CheckUserId(Axapta DynAx, string UserName)
        {
            string UserId = "";
            AxaptaRecord DynRec;
            string TableName = "UserInfo";
            string fieldName = ("name");
            //string fieldValue = ("your_search_criteria_here");

            // Define the record
            DynRec = DynAx.CreateAxaptaRecord(TableName);
            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", fieldName, UserName));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                UserId = (string)DynRec.get_Field("networkAlias");
            }
            DynRec.Dispose();
            return UserId;
        }

        public static Tuple<string, string> getWarrantyApprovalGroup(Axapta DynAx, string ClaimType, string inventLocationId)
        {
            /* Jerry 2024-11-04 Avoid hardcode
            //int LF_Warranty_AppGrp = 50771; // test
            int LF_Warranty_AppGrp = 30659; // live
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_Warranty_AppGrp);
            string TransportArr = ""; string GoodsRec = "";
            if (inventLocationId != "")
            {
                //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 5004);//test
                var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//live
                qbr1.Call("value", inventLocationId);
            }

            //var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 50003);//test
            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30002);//live
            qbr1_2.Call("value", ClaimType);
            */

            //Jerry 2024-11-04 Avoid hardcode
            //int LF_Warranty_AppGrp = 50771; // test
            //int LF_Warranty_AppGrp = 30659; // live
            int LF_Warranty_AppGrp = DynAx.GetTableIdWithLock("LF_Warranty_AppGrp");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_Warranty_AppGrp);
            string TransportArr = ""; string GoodsRec = "";
            if (inventLocationId != "")
            {
                //Jerry 2024-11-04 Avoid hardcode
                //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 5004);//test
                //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//live

                // Jerry 2024-12-13 Re-assign location id
                if (inventLocationId == "L&R")
                {
                    inventLocationId = "LnR";
                }
                if (inventLocationId == "A&P")
                {
                    inventLocationId = "AnP";
                }
                // Jerry 2024-12-13 Re-assign location id - END

                int inventLocationID = DynAx.GetFieldId(LF_Warranty_AppGrp, "InventLocationID");
                var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", inventLocationID);

                qbr1.Call("value", inventLocationId);
            }
            //Jerry 2024-11-04 Avoid hardcode
            //var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 50003);//test
            //var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30002);//live
            int claimType = DynAx.GetFieldId(LF_Warranty_AppGrp, "ClaimType");
            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimType);
            qbr1_2.Call("value", ClaimType);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_Warranty_AppGrp);
                TransportArr = DynRec1.get_Field("TransportArr1").ToString();
                TransportArr = TransportArr + "|" + DynRec1.get_Field("TransportArr2").ToString();
                TransportArr = TransportArr + "|" + DynRec1.get_Field("TransportArr3").ToString();
                TransportArr = TransportArr + "|" + DynRec1.get_Field("TransportArr4").ToString();

                GoodsRec = DynRec1.get_Field("GoodsRec1").ToString();
                GoodsRec = GoodsRec + "|" + DynRec1.get_Field("GoodsRec2").ToString();
                GoodsRec = GoodsRec + "|" + DynRec1.get_Field("GoodsRec3").ToString();
                GoodsRec = GoodsRec + "|" + DynRec1.get_Field("GoodsRec4").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string>(TransportArr, GoodsRec);
        }

        public static string getSuffixCode(Axapta DynAx, string CustAcc)
        {
            string getSuffixCode = "";

            //Jerry 2024-11-04 Avoid hardcode
            //int CustTable = 77;
            int CustTable = DynAx.GetTableIdWithLock("CustTable");

            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", 1);
            qbr4.Call("value", CustAcc);
            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            if ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                getSuffixCode = DynRec4.get_Field("CustomerClass").ToString();
                DynRec4.Dispose();
            }
            return getSuffixCode;
        }

        public static string ItemNameGetItemId(Axapta DynAx, string ItemName)
        {
            string getItemId = "";

            //Jerry 2024-11-04 Avoid hardcode
            //int InventTable = 175;
            int InventTable = DynAx.GetTableIdWithLock("InventTable");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", InventTable);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 3);//ItemName
            qbr1.Call("value", ItemName + "*");

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", InventTable);
                getItemId = DynRec1.get_Field("ItemId").ToString();

                DynRec1.Dispose();
            }
            return getItemId;
        }

        public static Tuple<string, string> get_RMAid(Axapta DynAx, string temp_SalesId)
        {
            string RMAid = ""; string salesStatus = "";

            //Jerry 2024-11-04 Avoid hardcode
            //int SalesTable = 366;
            int SalesTable = DynAx.GetTableIdWithLock("SalesTable");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesTable);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//SalesId
            qbr.Call("value", temp_SalesId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesTable);
                RMAid = DynRec.get_Field("ReturnItemNum").ToString();
                salesStatus = DynRec.get_Field("SalesStatus").ToString();
                DynRec.Dispose();
            }
            return new Tuple<string, string>(RMAid, salesStatus);
        }

        public static List<ListItem> get_Warehouse(Axapta DynAx)
        {
            List<ListItem> list = new List<ListItem>();
            string InventLocationID = "";

            //Jerry 2024-11-04 Avoid hardcode
            //int InventLocation = 158;
            int InventLocation = DynAx.GetTableIdWithLock("InventLocation");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", InventLocation);

            list.Add(new ListItem("-- SELECT --", ""));
            AxaptaObject axQueryRun1_13 = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            while ((bool)axQueryRun1_13.Call("next"))
            {
                AxaptaRecord DynRec1_13 = (AxaptaRecord)axQueryRun1_13.Call("Get", InventLocation);
                InventLocationID = DynRec1_13.get_Field("InventLocationID").ToString();
                list.Add(new ListItem(InventLocationID));
                DynRec1_13.Dispose();
            }
            return list;
        }

        public static Tuple<string, string, string, string, string, string, string,
            Tuple<string, string, string, string>> getWarrantyApprovalUser(Axapta DynAx, string ClaimType, string inventLocationId)
        {
            int tableId = DynAx.GetTableId("LF_Warranty_AppGrp");
            int fieldClaimTypeId = DynAx.GetFieldId(tableId, "ClaimType");
            int fieldinventLocationIdId = DynAx.GetFieldId(tableId, "inventLocationId");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId);
            string warehouse = ""; string TransportArr = ""; string GoodsRec = ""; string InvChecked = "";
            string Inspector = ""; string Verifier = ""; string Verifier2 = ""; string Approver1 = "";
            string Approver2 = ""; string Approver3 = ""; string Approver4 = "";

            if (inventLocationId != "" && inventLocationId != "-- SELECT --")
            {
                var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", fieldinventLocationIdId);//inventLocationId

                // Jerry 2024-12-13 Re-assign location id
                if (inventLocationId == "L&R")
                {
                    inventLocationId = "LnR";
                }
                if (inventLocationId == "A&P")
                {
                    inventLocationId = "AnP";
                }
                // Jerry 2024-12-13 Re-assign location id - END
                qbr1.Call("value", inventLocationId);
            }

            if (ClaimType != "-- SELECT --")
            {
                var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", fieldClaimTypeId);//ClaimType
                qbr1_2.Call("value", "*" + ClaimType + "*");
            }

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                warehouse = DynRec1.get_Field("InventLocationID").ToString();
                TransportArr = DynRec1.get_Field("TransportArr1").ToString();
                GoodsRec = DynRec1.get_Field("GoodsRec1").ToString();
                InvChecked = DynRec1.get_Field("InvChecked1").ToString();
                Inspector = DynRec1.get_Field("Inspector1").ToString();
                Verifier = DynRec1.get_Field("Verifier1").ToString();
                Verifier2 = DynRec1.get_Field("Verifier2").ToString();
                Approver1 = DynRec1.get_Field("Approver1").ToString();
                Approver2 = DynRec1.get_Field("Approver2").ToString();
                Approver3 = DynRec1.get_Field("Approver3").ToString().ToLower();
                Approver4 = DynRec1.get_Field("Approver4").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string, Tuple<string, string, string, string>>
                (TransportArr, GoodsRec, InvChecked, Inspector, Verifier, Approver1, warehouse,
                new Tuple<string, string, string, string>(Approver2, Approver3, Approver4, Verifier2));
        }

        public static List<ListItem> getWarrantyWarehouse()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class
            
            List<ListItem> list = new List<ListItem>();
            string InventLocationID = "";
            //Jerry 2024-11-04 Avoid using hardcode
            //int LF_Warranty_AppGrp = 30659;
            int LF_Warranty_AppGrp = DynAx.GetTableIdWithLock("LF_Warranty_AppGrp");
            int warehouse = DynAx.GetFieldId(LF_Warranty_AppGrp, "InventLocationID");
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_Warranty_AppGrp);

            axQueryDataSource.Call("addSortField", warehouse, 0);//RECID, must asc

            list.Add(new ListItem("-- SELECT --", ""));
            AxaptaObject axQueryRun1_13 = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            HashSet<string> uniqueInventLocationIDs = new HashSet<string>();
            while ((bool)axQueryRun1_13.Call("next"))
            {
                AxaptaRecord DynRec1_13 = (AxaptaRecord)axQueryRun1_13.Call("Get", LF_Warranty_AppGrp);

                InventLocationID = DynRec1_13.get_Field("InventLocationID").ToString();
                if (!uniqueInventLocationIDs.Contains(InventLocationID))
                {
                    list.Add(new ListItem(InventLocationID));
                    uniqueInventLocationIDs.Add(InventLocationID);
                }
                DynRec1_13.Dispose();
            }
            DynAx.Logoff();
            return list;
        }

        public static Tuple<string, string, string, string, string, string> getWarrantyApprovalUser2(Axapta DynAx, string ClaimType, string inventLocationId)
        {
            string tablename = "LF_Warranty_AppGrp";
            int tableid = DynAx.GetTableId(tablename);
            int InventLocationID = DynAx.GetFieldId(tableid, "InventLocationID");
            int ClaimTypeID = DynAx.GetFieldId(tableid, "ClaimType");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableid);
            string TransportArr2 = ""; string TransportArr3 = ""; string TransportArr4 = "";
            string GoodsRec2 = ""; string GoodsRec3 = ""; string GoodsRec4 = "";

            if (inventLocationId != "" && inventLocationId != "-- SELECT --")
            {
                //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 50004);//test

                var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", InventLocationID);//inventLocationId
                qbr1.Call("value", inventLocationId);
            }

            //var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 50003);//test

            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", ClaimTypeID);//ClaimType
            qbr1_2.Call("value", ClaimType);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableid);
                TransportArr2 = DynRec1.get_Field("TransportArr2").ToString();
                TransportArr3 = DynRec1.get_Field("TransportArr3").ToString();
                TransportArr4 = DynRec1.get_Field("TransportArr4").ToString();
                GoodsRec2 = DynRec1.get_Field("GoodsRec2").ToString();
                GoodsRec3 = DynRec1.get_Field("GoodsRec3").ToString();
                GoodsRec4 = DynRec1.get_Field("GoodsRec4").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string>
                (TransportArr2, TransportArr3, TransportArr4, GoodsRec2, GoodsRec3, GoodsRec4);
        }

        public static Tuple<string, string, string, string, string, string, string, Tuple<string, string>> getWarrantyApprovalUser3(Axapta DynAx, string ClaimType, string inventLocationId)
        {
            string tablename = "LF_Warranty_AppGrp";
            int tableid = DynAx.GetTableId(tablename);
            int inventlocationid = DynAx.GetFieldId(tableid, "InventLocationID");
            int claimtypeid = DynAx.GetFieldId(tableid, "ClaimType");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableid);
            string InvChecked2 = ""; string InvChecked3 = ""; string InvChecked4 = "";
            string Inspector2 = ""; string Inspector3 = ""; string Inspector4 = "";
            string Verifier2 = ""; string Verifier3 = ""; string Verifier4 = "";

            if (inventLocationId != "" && inventLocationId != "-- SELECT --")
            {
                //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 50004);//test

                var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", inventlocationid);//inventLocationId
                qbr1.Call("value", inventLocationId);
            }

            //var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 50003);//test

            var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimtypeid);//ClaimType
            qbr1_2.Call("value", ClaimType);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableid);
                InvChecked2 = DynRec1.get_Field("InvChecked2").ToString();
                InvChecked3 = DynRec1.get_Field("InvChecked3").ToString();
                InvChecked4 = DynRec1.get_Field("InvChecked4").ToString();
                Inspector2 = DynRec1.get_Field("Inspector2").ToString();
                Inspector3 = DynRec1.get_Field("Inspector3").ToString();
                Inspector4 = DynRec1.get_Field("Inspector4").ToString();
                Verifier2 = DynRec1.get_Field("Verifier2").ToString();
                Verifier3 = DynRec1.get_Field("Verifier3").ToString();
                Verifier4 = DynRec1.get_Field("Verifier4").ToString();

                DynRec1.Dispose();
            }
            return new Tuple<string, string, string, string, string, string, string, Tuple<string, string>>
                (InvChecked2, InvChecked3, InvChecked4, Inspector2, Inspector3, Inspector4, Verifier2,
                new Tuple<string, string>(Verifier3, Verifier4));
        }

        public static bool isSoCreated(Axapta DynAx, string CustomerRef)
        {
            AxaptaRecord DynRec;
            string TableName = "SalesTable";
            string fieldName = ("CustomerRef");
            //string fieldValue = ("your_search_criteria_here");

            // Define the record
            DynRec = DynAx.CreateAxaptaRecord(TableName);
            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", fieldName, CustomerRef));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool getWarrantyApprovalAccess(Axapta DynAx, string loginID)
        {
            int LF_Warranty_AppGrp = 30659; // live

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_Warranty_AppGrp);
            HashSet<string> getUniq = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<ListItem> List_Approval = new List<ListItem>();

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_Warranty_AppGrp);
                getUniq.Add(DynRec1.get_Field("TransportArr1").ToString());
                getUniq.Add(DynRec1.get_Field("TransportArr2").ToString());
                getUniq.Add(DynRec1.get_Field("TransportArr3").ToString());
                getUniq.Add(DynRec1.get_Field("TransportArr4").ToString());

                getUniq.Add(DynRec1.get_Field("GoodsRec1").ToString());
                getUniq.Add(DynRec1.get_Field("GoodsRec2").ToString());
                getUniq.Add(DynRec1.get_Field("GoodsRec3").ToString());
                getUniq.Add(DynRec1.get_Field("GoodsRec4").ToString());

                getUniq.Add(DynRec1.get_Field("InvChecked1").ToString());
                getUniq.Add(DynRec1.get_Field("InvChecked2").ToString());
                getUniq.Add(DynRec1.get_Field("InvChecked3").ToString());
                getUniq.Add(DynRec1.get_Field("InvChecked4").ToString());

                getUniq.Add(DynRec1.get_Field("Inspector1").ToString());
                getUniq.Add(DynRec1.get_Field("Inspector2").ToString());
                getUniq.Add(DynRec1.get_Field("Inspector3").ToString());
                getUniq.Add(DynRec1.get_Field("Inspector4").ToString());

                getUniq.Add(DynRec1.get_Field("Verifier1").ToString());
                getUniq.Add(DynRec1.get_Field("Verifier2").ToString());
                getUniq.Add(DynRec1.get_Field("Verifier3").ToString());
                getUniq.Add(DynRec1.get_Field("Verifier4").ToString());

                getUniq.Add(DynRec1.get_Field("Approver1").ToString());
                getUniq.Add(DynRec1.get_Field("Approver2").ToString());
                getUniq.Add(DynRec1.get_Field("Approver3").ToString());
                getUniq.Add(DynRec1.get_Field("Approver4").ToString());
                DynRec1.Dispose();
            }

            if (!getUniq.Contains(loginID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<ListItem> get_CustInvoiceTrans_details(Axapta DynAx, string ItemID, string custAcc)
        {
            List<ListItem> List_InvoiceID = new List<ListItem>();

            int count = 0;
            string invoiceDt = "";
            string[] InvoiceId = new string[100];
            string[] InvoiceDateRaw = new string[100];
            bool IsRMA = false;

            DateTime todayDt = DateTime.Today;
            DateTime twoYearsFromToday = todayDt.AddYears(-1);//temp one year
            string var_Qdate = twoYearsFromToday.ToString("dd/MM/yyyy") + ".." + todayDt.ToString("dd/MM/yyyy");

            string table = "CustInvoiceTrans";
            int tableid = DynAx.GetTableId(table);
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableid);

            // Setting up ranges for query
            AxaptaObject qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 6);
            qbr.Call("value", ItemID);

            AxaptaObject qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30005);
            qbr2.Call("value", custAcc);

            AxaptaObject qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 2);
            qbr1.Call("value", var_Qdate);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            List_InvoiceID.Add(new ListItem("-- SELECT --", ""));

            while ((bool)axQueryRun.Call("next"))
            {
                if (count == 30)
                {
                    return List_InvoiceID;
                }
                else
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableid);
                    InvoiceId[count] = DynRec.get_Field("InvoiceId").ToString();
                    var invoiceDateField = DynRec.get_Field("InvoiceDate");

                    if (invoiceDateField != null)
                    {
                        invoiceDt = Convert.ToDateTime(invoiceDateField).ToString("dd/MM/yyyy");
                    }
                    InvoiceDateRaw[count] = invoiceDt;

                    //// Check for RMA
                    //AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                    //AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", 359);//salesline

                    //AxaptaObject qbr3 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30017);//CNInvId
                    //qbr3.Call("value", "*" + InvoiceId[count] + "*");

                    //AxaptaObject qbr4 = (AxaptaObject)axQueryDataSource2.Call("addRange", 4);//SalesStatus
                    //qbr4.Call("value", "!=4");

                    //AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                    //if ((bool)axQueryRun2.Call("next"))
                    //{
                    //    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", 359);
                    //    string InvID = DynRec2.get_Field("CNInvId").ToString();
                    //    string salestatus = DynRec2.get_Field("SalesStatus").ToString();
                    //}
                    List_InvoiceID.Add(new ListItem(InvoiceId[count] + " " + InvoiceDateRaw[count]));

                    count++;

                }

            }
            return List_InvoiceID;
        }

        public static Tuple<string, string, string> get_CustInvoiceSalesUnit(Axapta DynAx, string InvoiceNo)
        {
            string ItemName = "";
            string InvoiceQty = "";
            string SalesUnit = "";

            int CustInvoiceTrans = 64;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustInvoiceTrans);
            var qbr1_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);
            qbr1_4.Call("value", InvoiceNo);//Invoice Id
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustInvoiceTrans);

                ItemName = DynRec.get_Field("Name").ToString();
                InvoiceQty = DynRec.get_Field("Qty").ToString();
                SalesUnit = DynRec.get_Field("UNITIDTXT").ToString();

                DynRec.Dispose();
            }
            return new Tuple<string, string, string>(ItemName, InvoiceQty, SalesUnit);
        }

        public static string getInventDimID_InventDimLocation(Axapta DynAx, string InventDimLocationID)
        {
            string InventDimID = "";

            int InventDim = 698;
            AxaptaObject axQuery1_13 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_13 = (AxaptaObject)axQuery1_13.Call("addDataSource", InventDim);

            int InventDimLocationIDField = DynAx.GetFieldId(InventDim, "InventLocationId");
            var qbr13 = (AxaptaObject)axQueryDataSource1_13.Call("addRange", InventDimLocationIDField);
            qbr13.Call("value", InventDimLocationID);//InventDimLocation

            AxaptaObject axQueryRun1_13 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_13);
            if ((bool)axQueryRun1_13.Call("next"))
            {
                AxaptaRecord DynRec1_13 = (AxaptaRecord)axQueryRun1_13.Call("Get", InventDim);

                InventDimID = DynRec1_13.get_Field("InventDimId").ToString();
                DynRec1_13.Dispose();
            }

            return InventDimID;
        }
    }
}