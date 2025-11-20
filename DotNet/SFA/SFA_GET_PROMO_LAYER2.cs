using Microsoft.Dynamics.BusinessConnectorNet;
using System;
namespace DotNet
{
    public class SFA_GET_PROMO_LAYER2
    {
        public static Tuple<string, string> getPromotionId_getstmres(Axapta DynAx, string SO_number, string camp_id, string camp_type)
        {
            string getPromotionId = "0";
            string getstmres = "0";
            AxaptaObject LF_MixPriceGrp = DynAx.CreateAxaptaObject("LF_MixPriceGrp");
            //getPromotionId = LF_MixPriceGrp.Call("getPromotionType_Web", SO_number, camp_id, camp_type).ToString();
            getPromotionId = LF_MixPriceGrp.Call("getPromotionType_Web_02", SO_number, camp_id, camp_type).ToString();
            getstmres = LF_MixPriceGrp.Call("getCompl_Section3", SO_number, camp_id, camp_type).ToString();
            return new Tuple<string, string>(getPromotionId, getstmres);
        }

        //=============================================================================================================
        public static Tuple<int, string[], string[], string[], string[], string[], string[]> GeneralSplit_TotalItem_id_level_qty_vtype_unitid_section(string getPromotionId)
        {
            int TotalItem = 0;

            string[] arr_getPromotionId = getPromotionId.Split(new string[] { "@@" }, StringSplitOptions.None);
            TotalItem = Convert.ToInt32(arr_getPromotionId[0]);

            string[] DataList = new string[TotalItem];
            string[] id = new string[TotalItem];        //Item Id
            string[] level = new string[TotalItem];     //Item level for Promo Type 6
            string[] qty = new string[TotalItem];       //Quantity Entitled
            string[] vtype = new string[TotalItem];     //S: Sum F:Fixed
            string[] unitid = new string[TotalItem];    //Unit ID
            string[] section = new string[TotalItem];   //Section

            for (int i = 0; i < TotalItem; i++)
            {
                DataList[i] = arr_getPromotionId[i + 1];
                string[] arr_DataList = DataList[i].Split(new string[] { "@%" }, StringSplitOptions.None);
                id[i] = arr_DataList[0];
                level[i] = arr_DataList[1];
                qty[i] = arr_DataList[2];
                vtype[i] = arr_DataList[3];
                unitid[i] = arr_DataList[4];
                section[i] = arr_DataList[5];
            }
            return new Tuple<int, string[], string[], string[], string[], string[], string[]>(TotalItem, id, level, qty, vtype, unitid, section);
        }
        //=============================================================================================================
        public static string getItemNameFOC(Axapta DynAx, string Campid, string ItemId)
        {
            string getItemNameFOC = "";
            int LF_MixPriceGrp = 30405;
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_MixPriceGrp);

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 30001);//MIXPriceGroupID
            qbr.Call("value", Campid);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_MixPriceGrp);
                string temp_FOC_ItemID1 = DynRec.get_Field("FOC_ItemID1").ToString();
                string temp_FOC_ItemID2 = DynRec.get_Field("FOC_ItemID2").ToString();
                string temp_FOC_ItemID3 = DynRec.get_Field("FOC_ItemID3").ToString();
                string temp_FOC_ItemID4 = DynRec.get_Field("FOC_ItemID4").ToString();
                string temp_FOC_ItemID5 = DynRec.get_Field("FOC_ItemID5").ToString();
                if (ItemId == temp_FOC_ItemID1)
                {
                    getItemNameFOC = DynRec.get_Field("FOC_ItemName1").ToString();
                }
                if (ItemId == temp_FOC_ItemID2)
                {
                    getItemNameFOC = DynRec.get_Field("FOC_ItemName2").ToString();
                }
                if (ItemId == temp_FOC_ItemID3)
                {
                    getItemNameFOC = DynRec.get_Field("FOC_ItemName3").ToString();
                }
                if (ItemId == temp_FOC_ItemID4)
                {
                    getItemNameFOC = DynRec.get_Field("FOC_ItemName4").ToString();
                }
                if (ItemId == temp_FOC_ItemID5)
                {
                    getItemNameFOC = DynRec.get_Field("FOC_ItemName5").ToString() + " (" + DynRec.get_Field("Foc5_Txt").ToString() + ")";
                }
                DynRec.Dispose();
            }
            if (getItemNameFOC == "")//no item name
            {
                getItemNameFOC = ItemId;
            }
            return getItemNameFOC;
        }
        //=============================================================================================================
        public static string getItemNameOption(Axapta DynAx, string Campid, string ItemId)
        {
            string getItemNameOption = "";
            int LF_MixPriceGrp = 30405;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_MixPriceGrp);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//MIXPriceGroupID
            qbr1.Call("value", Campid);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_MixPriceGrp);
                string MixPriceGroupOption = DynRec1.get_Field("MixPriceGroupOption").ToString();

                if (MixPriceGroupOption != "")
                {
                    getItemNameOption = getItemNameFOC(DynAx, MixPriceGroupOption, ItemId);

                    if (getItemNameOption == "")
                    {
                        getItemNameOption = getItemNameFOC(DynAx, Campid, ItemId);
                    }

                }
                DynRec1.Dispose();
            }
            return getItemNameOption;
        }
        //=============================================================================================================

        public static string getitemvol(Axapta DynAx, string Campid, string ItemId)
        {
            string getitemvol = "";
            int LF_MixPriceGrp = 30405;
            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", LF_MixPriceGrp);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30001);//MIXPriceGroupID
            qbr2.Call("value", Campid);

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", LF_MixPriceGrp);

                string temp_FOC_ItemID1 = DynRec2.get_Field("FOC_ItemID1").ToString();
                string temp_FOC_ItemID2 = DynRec2.get_Field("FOC_ItemID2").ToString();
                string temp_FOC_ItemID3 = DynRec2.get_Field("FOC_ItemID3").ToString();
                string temp_FOC_ItemID4 = DynRec2.get_Field("FOC_ItemID4").ToString();
                string temp_FOC_ItemID5 = DynRec2.get_Field("FOC_ItemID5").ToString();
                if (ItemId == temp_FOC_ItemID1)
                {
                    getitemvol = DynRec2.get_Field("FOC_Qty1").ToString();
                }
                if (ItemId == temp_FOC_ItemID2)
                {
                    getitemvol = DynRec2.get_Field("FOC_Qty2").ToString();
                }
                if (ItemId == temp_FOC_ItemID3)
                {
                    getitemvol = DynRec2.get_Field("FOC_Qty3").ToString();
                }
                if (ItemId == temp_FOC_ItemID4)
                {
                    getitemvol = DynRec2.get_Field("FOC_Qty4").ToString();
                }
                if (ItemId == temp_FOC_ItemID5)
                {
                    getitemvol = DynRec2.get_Field("FOC_Qty5").ToString();
                }

                DynRec2.Dispose();
            }
            return getitemvol;
        }
        //=============================================================================================================
        public static string getVolQty(Axapta DynAx, string Campid, string ItemId)
        {
            string getVolQty = "";
            string temp_getVolQty2 = getitemvol(DynAx, Campid, ItemId);
            if (temp_getVolQty2 != "")
            {
                decimal temp_getVolQty = decimal.Parse(temp_getVolQty2);
                if (temp_getVolQty < 1)
                {
                    getVolQty = "";
                }
                else
                {
                    getVolQty = temp_getVolQty.ToString();
                }
            }

            return getVolQty;
        }
        //=============================================================================================================
        public static string GetPromoPrice(Axapta DynAx, string Sales_Order, string cam, string ItemId)
        {
            AxaptaObject LF_MixPriceGrp = DynAx.CreateAxaptaObject("LF_MixPriceGrp");
            string GetPromoPrice = LF_MixPriceGrp.Call("getPricePromo", Sales_Order, cam, ItemId).ToString();
            return GetPromoPrice;
        }
        //=============================================================================================================
        public static string getItemNamev3(Axapta DynAx, string ItemId)
        {
            AxaptaObject LF_MixPriceGrp = DynAx.CreateAxaptaObject("LF_MixPriceGrp");
            string getItemNamev3 = LF_MixPriceGrp.Call("getItemName", ItemId).ToString();
            return getItemNamev3;
        }
        //=============================================================================================================
        public static string getItemName(Axapta DynAx, string ItemId)
        {
            string getItemName = "";
            int InventTable = 175;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", InventTable);

            var qbr3 = (AxaptaObject)axQueryDataSource3.Call("addRange", 2);//ItemId
            qbr3.Call("value", ItemId);

            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);
            if ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", InventTable);
                getItemName = DynRec3.get_Field("ItemName").ToString();


                DynRec3.Dispose();
            }

            if (getItemName == "")
            {
                getItemName = getItemNamev3(DynAx, ItemId);
            }
            return getItemName;
        }
        //=============================================================================================================
        public static string getOptionCamp(Axapta DynAx, string Campid)
        {
            string getOptionCamp = "";
            int LF_MixPriceGrp = 30405;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_MixPriceGrp);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//MIXPriceGroupID
            qbr1.Call("value", Campid);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_MixPriceGrp);
                string MixPriceGroupOption = DynRec1.get_Field("MixPriceGroupOption").ToString();

                getOptionCamp = MixPriceGroupOption;
                DynRec1.Dispose();
            }
            return getOptionCamp;
        }
        //=============================================================================================================
        //=============================================================================================================

        //=============================================================================================================
    }
}