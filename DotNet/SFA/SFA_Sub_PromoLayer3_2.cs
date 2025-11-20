using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Linq;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        private string promo_layer3()
        {
            try
            {
                //==============================================================================================================================================
                if (Section1.Visible == true)//got section 1
                {
                    if (GridView_PromoSection1_sum.Visible == true)
                    {
                        string[] arr_hidden_ItemId_Sum_Section1 = hidden_ItemId_Sum_Section1.Text.Split('|');
                        //int arr_hidden_ItemId_Sum_Section1_count = arr_hidden_ItemId_Sum_Section1.Count();

                        string[] arr_hidden_Qty_Sum_Section1 = hidden_Qty_Sum_Section1.Text.Split('|');
                        int arr_hidden_Qty_Sum_Section1_count = arr_hidden_Qty_Sum_Section1.Count();

                        for (int i = 0; i < arr_hidden_Qty_Sum_Section1_count; i++)
                        {
                            string FOC_Qty = arr_hidden_Qty_Sum_Section1[i];
                            string FOC_Id = "";
                            if ((FOC_Qty != "") && (FOC_Qty != "0"))//not empty or zero
                            {
                                FOC_Id = arr_hidden_ItemId_Sum_Section1[i];

                                string error = promo_layer3_CallClass(FOC_Id, FOC_Qty, 0);
                                if (error != "")
                                {
                                    return error;
                                }
                            }
                        }
                    }
                    if (GridView_PromoSection1_fixed.Visible == true)
                    {
                        string arr_hidden_ItemId_fixed_Section1 = hidden_ItemId_fixed_Section1.Text;

                        string[] arr_hidden_Qty_fixed_Section1 = hidden_Qty_fixed_Section1.Text.Split('|');
                        int arr_hidden_Qty_fixed_Section1_count = arr_hidden_Qty_fixed_Section1.Count();

                        for (int i = 0; i < arr_hidden_Qty_fixed_Section1_count; i++)
                        {
                            string FOC_Qty = arr_hidden_Qty_fixed_Section1[i];
                            string FOC_Id = "";
                            if ((FOC_Qty != "") && (FOC_Qty != "0"))//not empty or zero
                            {
                                FOC_Id = arr_hidden_ItemId_fixed_Section1;
                                string error = promo_layer3_CallClass(FOC_Id, FOC_Qty, 0);
                                if (error != "")
                                {
                                    return error;
                                }
                            }
                        }
                    }
                }
                //==============================================================================================================================================
                if (Section2.Visible == true)//got section 2
                {
                    if (GridView_PromoSection2_sum.Visible == true)
                    {
                        string arr_hidden_ItemId_Sum_Section2 = hidden_ItemId_Sum_Section2.Text;

                        string[] arr_hidden_Qty_Sum_Section2 = hidden_Qty_Sum_Section2.Text.Split('|');
                        int arr_hidden_Qty_Sum_Section2_count = arr_hidden_Qty_Sum_Section2.Count();

                        for (int i = 0; i < arr_hidden_Qty_Sum_Section2_count; i++)
                        {
                            string FOC_Qty = arr_hidden_Qty_Sum_Section2[i];
                            string FOC_Id = "";
                            if ((FOC_Qty != "") && (FOC_Qty != "0"))//not empty or zero
                            {
                                FOC_Id = arr_hidden_ItemId_Sum_Section2;

                                string error = promo_layer3_CallClass(FOC_Id, FOC_Qty, 1);
                                if (error != "")
                                {
                                    return error;
                                }
                            }
                        }
                    }
                    if (GridView_PromoSection2_fixed.Visible == true)
                    {
                        string[] arr_hidden_ItemId_fixed_Section2 = hidden_ItemId_fixed_Section2.Text.Split('|');

                        string[] arr_hidden_Qty_fixed_Section2 = hidden_Qty_fixed_Section2.Text.Split('|');
                        int arr_hidden_Qty_fixed_Section2_count = arr_hidden_Qty_fixed_Section2.Count();

                        for (int i = 0; i < arr_hidden_Qty_fixed_Section2_count; i++)
                        {
                            string FOC_Qty = arr_hidden_Qty_fixed_Section2[i];
                            string FOC_Id = "";
                            if ((FOC_Qty != "") && (FOC_Qty != "0"))//not empty or zero
                            {
                                FOC_Id = arr_hidden_ItemId_fixed_Section2[i];

                                string error = promo_layer3_CallClass(FOC_Id, FOC_Qty, 1);
                                if (error != "")
                                {
                                    return error;
                                }
                            }
                        }
                    }
                }
                //==============================================================================================================================================
                if (Section3.Visible == true)//got section 3
                {
                    if (GridView_PromoSection3_sum.Visible == true)
                    {
                        string[] arr_hidden_ItemId_Sum_Section3 = hidden_ItemId_Sum_Section3.Text.Split('|');

                        string[] arr_hidden_Qty_Sum_Section3 = hidden_Qty_Sum_Section3.Text.Split('|');
                        int arr_hidden_Qty_Sum_Section3_count = arr_hidden_Qty_Sum_Section3.Count();

                        for (int i = 0; i < arr_hidden_Qty_Sum_Section3_count; i++)
                        {
                            string FOC_Qty = arr_hidden_Qty_Sum_Section3[i];
                            string FOC_Id = "";

                            if ((FOC_Qty != "") && (FOC_Qty != "0"))//not empty or zero
                            {
                                FOC_Id = arr_hidden_ItemId_Sum_Section3[i];

                                string error = promo_layer3_CallClass(FOC_Id, FOC_Qty, 2);
                                if (error != "")
                                {
                                    return error;
                                }
                            }
                        }
                    }
                    if (GridView_PromoSection3_fixed.Visible == true)
                    {
                        string[] arr_hidden_ItemId_fixed_Section3 = hidden_ItemId_fixed_Section3.Text.Split('|');

                        string[] arr_hidden_Qty_fixed_Section3 = hidden_Qty_fixed_Section3.Text.Split('|');
                        int arr_hidden_Qty_fixed_Section3_count = arr_hidden_Qty_fixed_Section3.Count();

                        for (int i = 0; i < arr_hidden_Qty_fixed_Section3_count; i++)
                        {
                            string FOC_Qty = arr_hidden_Qty_fixed_Section3[i];
                            string FOC_Id = "";
                            if ((FOC_Qty != "") && (FOC_Qty != "0"))//not empty or zero
                            {
                                FOC_Id = arr_hidden_ItemId_fixed_Section3[i];

                                string error = promo_layer3_CallClass(FOC_Id, FOC_Qty, 2);
                                if (error != "")
                                {
                                    return error;
                                }
                            }
                        }
                    }
                }
                //==============================================================================================================================================

                return "";
            }
            catch (Exception ER_SF_22)
            {
                string error = "ER_SF_22: " + ER_SF_22.ToString();
                return error;
            }
        }
        private string promo_layer3_CallClass(string FOC_Id, string FOC_Qty, int Section)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string MXPriceGroup = hidden_layer2_camp_id.Text;
                string CampaignType = hidden_layer2_camp_type.Text;
                string so_number = hidden_layer2_SO_number.Text;

                AxaptaObject DomComSalesTable = DynAx.CreateAxaptaObject("DomComSalesTable");
                AxaptaObject class_LF_MixPriceGroup = DynAx.CreateAxaptaObject("LF_MixPriceGrp");
                string b_MXPriceGroup = SFA_GET_PROMO_LAYER2.getOptionCamp(DynAx, MXPriceGroup);

                string str_result1 = "";
                if (CampaignType == "8")
                {
                    var result1 = class_LF_MixPriceGroup.Call("InsertNewFOCItem_Web_08", so_number, FOC_Id, FOC_Qty);
                    str_result1 = "1";
                }
                else if (CampaignType == "2")
                {
                    var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, b_MXPriceGroup, FOC_Id, FOC_Qty, "2");
                    str_result1 = result1.ToString();
                }
                else if (CampaignType == "6")
                {
                    if (b_MXPriceGroup != "" && Section.ToString() == "1")//second section only
                    {
                        var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, b_MXPriceGroup, FOC_Id, FOC_Qty, "1");
                        str_result1 = result1.ToString();
                        //var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web_02", so_number, MXPriceGroup, FOC_Id, FOC_Qty, "0");
                    }
                    else// other section (1st section)
                    {
                        var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, MXPriceGroup, FOC_Id, FOC_Qty, Section.ToString());
                        str_result1 = result1.ToString();
                    }
                }
                else if (CampaignType == "7")// campaign 7, the section 1 and 2 axapta parameter _campaignIdSecond is 0
                {
                    string temp_section = Section.ToString();
                    if (temp_section == "0" || temp_section == "1")//first and second section only
                    {
                        string result1;
                        if (temp_section == "1")//in case user select 2 foc items
                        {
                            result1 = (string)class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, MXPriceGroup, FOC_Id, FOC_Qty, "2");
                            str_result1 = result1.ToString();
                        }
                        else
                        {
                            result1 = (string)class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, MXPriceGroup, FOC_Id, FOC_Qty, "0");
                            str_result1 = result1.ToString();
                        }
                        //var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web_02", so_number, MXPriceGroup, FOC_Id, FOC_Qty, "0");
                    }
                    else if (temp_section == "2")
                    {
                        var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, b_MXPriceGroup, FOC_Id, FOC_Qty, "1");
                        str_result1 = result1.ToString();
                    }
                    else//other section
                    {
                        var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, MXPriceGroup, FOC_Id, FOC_Qty, temp_section);
                        str_result1 = result1.ToString();
                    }
                }
                else// in case unhande case
                {
                    if (b_MXPriceGroup == "") b_MXPriceGroup = MXPriceGroup.ToString();//from chong campaign testing

                    var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web", so_number, b_MXPriceGroup, FOC_Id, FOC_Qty, Section.ToString());
                    str_result1 = result1.ToString();
                    //var result1 = class_LF_MixPriceGroup.Call("getFOCItem_Web_02", so_number, MXPriceGroup, FOC_Id, FOC_Qty, "0");                  
                }

                if (str_result1 != "1")
                {
                    return "ER_SF_17: Fail to add promo item. Id: " + FOC_Id + " || Qty: " + FOC_Qty;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ER_SF_18)
            {
                return "ER_SF_18: Id: " + FOC_Id + " || Qty: " + FOC_Qty + " ||| " + ER_SF_18.ToString();
            }
            finally
            {
                DynAx.Logoff();
            }
        }

    }
}