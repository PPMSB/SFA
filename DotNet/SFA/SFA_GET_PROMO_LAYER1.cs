using Microsoft.Dynamics.BusinessConnectorNet;
using System;

namespace DotNet
{
    public class SFA_GET_PROMO_LAYER1
    {
        public static string getCampaignId(Axapta DynAx, string Sales_order, string user_email)
        {
            string getCampaignId = "0";

            AxaptaObject LF_MixPriceGrp = DynAx.CreateAxaptaObject("LF_MixPriceGrp");

            getCampaignId = LF_MixPriceGrp.Call("getCampaignId_Web_02", Sales_order, user_email).ToString();
            //getCampaignId = LF_MixPriceGrp.Call("getCampaignId_Web", Sales_order, user_email).ToString();

            return getCampaignId;
        }
        //=============================================================================================================
        public static Tuple<int, string[], string[]> GeneralSplit_TotalCount_id_type(string getCampaignId)
        {
            int TotalCount = 0;

            string[] arr_getCampaignId = getCampaignId.Split(new string[] { "@@" }, StringSplitOptions.None);
            TotalCount = Convert.ToInt32(arr_getCampaignId[0]);

            string[] DataList = new string[TotalCount];
            string[] id = new string[TotalCount];   //subset of DataList
            string[] type = new string[TotalCount]; //subset of DataList

            for (int i = 0; i < TotalCount; i++)
            {
                DataList[i] = arr_getCampaignId[i + 1];
                string[] arr_DataList = DataList[i].Split(new string[] { "@%" }, StringSplitOptions.None);
                id[i] = arr_DataList[0];
                type[i] = arr_DataList[1];
            }
            return new Tuple<int, string[], string[]>(TotalCount, id, type);
        }
        //=============================================================================================================
        public static string getdetailname(Axapta DynAx, string FOCID)
        {
            string getdetailname = "";

            int LF_MixPriceGroup = 30405;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_MixPriceGroup);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//FOCID
            qbr1.Call("value", FOCID);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_MixPriceGroup);
                getdetailname = DynRec1.get_Field("Description").ToString();

                DynRec1.Dispose();
            }
            return getdetailname;
        }
    }
}