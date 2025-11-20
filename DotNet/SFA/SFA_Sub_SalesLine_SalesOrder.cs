using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        //start of sales order
        private void Sales_order()
        {
            sales_order_section_general.Visible = false;
            Sales_order_section.Visible = true;
            FV_order_section.Visible = false;
        }

        protected void Button_CancelSalesOrder_Click(object sender, EventArgs e)
        {
            sales_order_section_general.Visible = true;
            Sales_order_section.Visible = false;
            FV_order_section.Visible = false;

            clear_sales_line(); Label_SO_No.Text = "SO number : " + hidden_Label_SO_No.Text;//12 words + hidden
            sales_line_view();
        }

        protected void Button_SaveSalesOrder_Click(object sender, EventArgs e)
        {
            if (TextBox_SLQuantity.Text == "")
            {
                Function_Method.MsgBox("There is no quantity", this.Page, this);
                return;
            }
            if (PreProcess_SaveSalesOrder() == true)
            {
                if (initialize_SaveSalesOrder() == true)
                {
                    sales_order_section_general.Visible = true;
                    Sales_order_section.Visible = false;
                    FV_order_section.Visible = false;
                    clear_sales_line();
                    Label_SO_No.Text = "SO number : " + hidden_Label_SO_No.Text;//12 words + hidden
                    sales_line_view();
                    Function_Method.MsgBox("Sales order saved.", this.Page, this);
                }
            }
        }

        private bool initialize_SaveSalesOrder()
        {
            Axapta DynAx = new Axapta();
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                //====================================================================
                //string CustAcc = TextBox_Account.Text.Trim();
                string ItemId = TextBox_Id.Text.Trim();
                string SO_number = hidden_Label_SO_No.Text;
                string deliverydate = Request.Form[txtDeliveryDate.UniqueID];

                if (deliverydate == "")  
                {//Some variable data need to derived from SalesTable manually
                    var tuple_reload = SFA_GET_SALES_ORDER.reload_from_SalesTable(DynAx, SO_number);
                    //Delivery accordion
                    string temp_DeliveryDate = tuple_reload.Item2;
                    string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
                    deliverydate = arr_temp_DeliveryDate[0];
                }

                string selected_item_unit = hidden_selected_item_unit.Text;
                //var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var DeliveryDate = Function_Method.get_correct_date(GLOBAL.system_checking, deliverydate, false);
                var temp_deliveryDate = DateTime.ParseExact(DeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string itemQuantity = TextBox_SLQuantity.Text;
                string itemPrice = Label_Price.Text;

                string temp_SuffixCodeWithName = Label_Suffix_Code.Text.Trim();
                string[] arr_temp_SuffixCodeWithName = temp_SuffixCodeWithName.Split('|');//temp_SuffixCode + " | " + temp_SuffixCodeName;
                string Suffix_Code = arr_temp_SuffixCodeWithName[0];

                string LinePercent = Label_Disc_pct.Text;
                string LineDisc = Label_Discount.Text;

                string MultiLnPercent = Label_MultilineDisc_pct.Text;
                string MultiLnDisc = Label_MultilineDiscount.Text;

                string inventDimId = hidden_inventDim.Text;
                //
                string axTaxItemGroup = SFA_GET_SALES_ORDER_2.get_TaxItemGroup(DynAx, ItemId);
                //
                string LF_MixPriceGroup = ""; string LF_MixPricePromo = "";
                var tuple_getMPP_MPG = SFA_GET_SALES_ORDER_2.get_MPP_MPG(DynAx, SO_number);
                LF_MixPricePromo = tuple_getMPP_MPG.Item1;
                LF_MixPriceGroup = tuple_getMPP_MPG.Item2;
                //
                string LFUOM = SFA_GET_SALES_ORDER_2.get_UOM(DynAx, selected_item_unit);
                //====================================================================
                AxaptaObject axSalesLine = DynAx.CreateAxaptaObject("AxSalesLine");
                var salesid = axSalesLine.Call("InitSalesLine", SO_number);

                axSalesLine.Call("parmReceiptDateRequested", temp_deliveryDate);
                axSalesLine.Call("parmShippingDateRequested", temp_deliveryDate);
                axSalesLine.Call("parmConfirmedDlv", temp_deliveryDate);
                axSalesLine.Call("parmItemId", ItemId);
                axSalesLine.Call("parmSalesUnit", selected_item_unit);
                axSalesLine.Call("parmSalesQty", itemQuantity);
                axSalesLine.Call("parmSalesPrice", itemPrice);
                axSalesLine.Call("suffixCode", Suffix_Code);
                axSalesLine.Call("parmLinePercent", LinePercent);
                axSalesLine.Call("parmLineDisc", LineDisc);
                axSalesLine.Call("parmMultiLnPercent", MultiLnPercent);
                axSalesLine.Call("parmMultiLnDisc", MultiLnDisc);
                axSalesLine.Call("parmMixPriceGroup", LF_MixPriceGroup);
                axSalesLine.Call("parmUOM", LFUOM);
                axSalesLine.Call("parmMixPricePromo", LF_MixPricePromo);

                if (axTaxItemGroup == "OS")
                {
                    axSalesLine.Call("parmTaxGroup", axTaxItemGroup);
                }
                if (inventDimId != "")
                {
                    axSalesLine.Call("parmInventDimId", inventDimId);
                }

                string poCost = ""; string manualCost = "";
                if (GLOBAL.user_company == "PBM")
                {
                    if (poCost != "")
                    {
                        axSalesLine.Call("POCost", poCost);
                    }
                    else
                    {
                        axSalesLine.Call("POCost", 0);
                    }
                    if (manualCost != "")
                    {
                        axSalesLine.Call("manualCost", manualCost);
                    }
                    else
                    {
                        axSalesLine.Call("manualCost", 0);
                    }
                }

                //====================================================================
                axSalesLine.Call("dosave");

                var SalesLineRec = axSalesLine.Call("CurrentRecord");
                var RecIDSL = axSalesLine.Call("CurrentRecordId");
                return true;
            }
            catch (Exception ER_SF_09)
            {
                Function_Method.MsgBox("ER_SF_09: " + ER_SF_09.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private bool PreProcess_SaveSalesOrder()
        {
            Axapta DynAx = new Axapta();
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                //====================================================================
                string inventDimId = "";
                string CustAcc = TextBox_Account.Text.Trim();
                string ItemId = TextBox_Id.Text.Trim();
                string SO_number = hidden_Label_SO_No.Text;
                string temp_selected_item_unit_name = DropDownList_Unit.Text;
                string[] arr_temp_selected_item_unit_name = temp_selected_item_unit_name.Split('|');
                string selected_item_unit = arr_temp_selected_item_unit_name[0].Trim();
                hidden_selected_item_unit.Text = selected_item_unit;
                var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //====================================================================
                string validateSubmitted = SFA_GET_SALES_ORDER_2.get_validateSubmitted(DynAx, SO_number);
                if (validateSubmitted == "1")//already submitted
                {
                    if (GLOBAL.user_authority_lvl != 1)//not system Admin
                    {
                        //check for HOD
                        var tupple_get_Empl_Id = SFA_GET_SALES_ORDER_2.get_Empl_Id(DynAx, GLOBAL.user_id);
                        string[] Empl_Id = tupple_get_Empl_Id.Item1;
                        int counter = tupple_get_Empl_Id.Item2;

                        int temp_flag_HOD = 0;
                        for (int j = 0; j < counter; j++)
                        {
                            if (Empl_Id[j].Substring(0, 1) == "H")
                            {//HOD
                                temp_flag_HOD = 1;
                            }
                        }
                        if (temp_flag_HOD == 0)
                        {
                            Function_Method.MsgBox("This SO has been submitted and confirmed. Please contact admin for details.", this.Page, this);
                            return false;
                        }
                        else
                        {
                            Function_Method.MsgBox("User is HOD. SO can be resubmit by HOD.", this.Page, this);
                        }
                    }
                }
                //====================================================================
                AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");
                //====================================================================
                if (hidden_configIdStr.Text == "" || hidden_sizeStr.Text == "" || hidden_colorStr.Text == "" || Label_Warehouse.Text == "")
                {
                    inventDimId = domComSalesLine.Call("getInventDimId", ItemId, hidden_configIdStr.Text, hidden_colorStr.Text, hidden_sizeStr.Text, Label_Warehouse.Text).ToString();

                    if (inventDimId == "")
                    {
                        Function_Method.MsgBox("Sales Line unable to be save. Please check the details again.", this.Page, this);
                        return false;
                    }
                    else
                    {
                        hidden_inventDim.Text = inventDimId;
                    }
                }

                //====================================================================
                //====================================================================
                if (hidden_Lowest_Qty.Text != "")
                {
                    int int_Lowest_Qty = Convert.ToInt32(hidden_Lowest_Qty.Text);
                    int int_SLQuantity = Convert.ToInt32(TextBox_SLQuantity.Text);

                    if (int_SLQuantity < int_Lowest_Qty)
                    {
                        TextBox_SLQuantity.Text = hidden_Lowest_Qty.Text;//auto adjust
                    }
                }

                //====================================================================
                //if (GLOBAL.user_company != "PBM")//not PBM
                //{

                string itemPrice = domComSalesLine.Call("getSalesPrice", ItemId, selected_item_unit, temp_Today, TextBox_SLQuantity.Text, CustAcc, SO_number).ToString();
                Label_Price.Text = itemPrice;
                //}
                //discount amount
                string lineDiscAmt = "";
                lineDiscAmt = domComSalesLine.Call("getLineDiscAmt", ItemId, selected_item_unit, temp_Today, TextBox_SLQuantity.Text, CustAcc, SO_number).ToString();
                if (lineDiscAmt == "")
                {
                    Label_Discount.Text = "0";
                }
                else
                {
                    Label_Discount.Text = lineDiscAmt;
                }
                //
                string lineDiscPct = "";
                lineDiscPct = domComSalesLine.Call("getLineDiscPct", ItemId, selected_item_unit, temp_Today, TextBox_SLQuantity.Text, CustAcc, SO_number).ToString();
                if (lineDiscPct == "")
                {
                    Label_Disc_pct.Text = "0";
                }
                else
                {
                    Label_Disc_pct.Text = lineDiscPct;
                }

                //====================================================================

                return true;
            }
            catch (Exception ER_SF_08)
            {
                Function_Method.MsgBox("ER_SF_08: " + ER_SF_08.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        //=========================================================================================================
        protected void CheckIdInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_SFIM@" + hidden_Label_SO_No.Text;//SFA > InventoryMaster
            //Response.Redirect("InventoryMaster.aspx");

            string url = "InventoryMaster.aspx";
            //string script = "window.open('" + url + "', '_blank');";
            string script = "window.open('" + url + "', 'InventoryMasterPopup', 'width=1000,height=800,resizable=yes,scrollbars=yes');";

            // Register the script to open the new tab

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
        }

        protected void CheckId(object sender, EventArgs e)
        {
            validate_SL();
        }

        private void validate_SL()
        {
            string fieldValue = TextBox_Id.Text.Trim();
            string SO_number = hidden_Label_SO_No.Text;

            clear_sales_line(); TextBox_Id.Text = fieldValue;

            if (fieldValue == "")
            {
                Function_Method.MsgBox("There is no item id", this.Page, this);
                return;
            }
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int InventTable = 175;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", InventTable);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 2);//ItemId
                qbr.Call("value", fieldValue);
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                if ((bool)axQueryRun.Call("next"))//use if only one record
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", InventTable);

                    Label_IdName.Text = DynRec.get_Field("ItemName").ToString();

                    string itemunit = SFA_GET_SALES_ORDER.get_SLUnit(DynAx, fieldValue);//fieldValue=Itemid
                    string itemunit_ItemName = itemunit + " | " + SFA_GET_SALES_ORDER.get_SLUnitName(DynAx, itemunit);//fieldValue=Itemid
                    //
                    AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");
                    string itemUnitStr = domComSalesLine.Call("getValidUnits", fieldValue).ToString();//fieldValue=Itemid
                    itemUnitStr = SFA_GET_SALES_ORDER.get_sanitize(itemUnitStr);

                    string[] arr_itemUnitStr = itemUnitStr.Split(',');
                    int counter_arr_itemUnitStr = arr_itemUnitStr.Count();

                    List<ListItem> List_itemUnit = new List<ListItem>();
                    List_itemUnit.Add(new ListItem(itemunit_ItemName));//default unit

                    for (int j = 0; j < counter_arr_itemUnitStr; j++)
                    {
                        int counter_list = List_itemUnit.Count();
                        for (int i = 0; i < counter_list; i++)
                        {
                            if (arr_itemUnitStr[j] != List_itemUnit[i].Text)
                            {
                                List_itemUnit.Add(new ListItem(arr_itemUnitStr[j]));
                            }
                            else//exit loop
                            {
                                i = counter_list;
                            }
                        }
                    }
                    DropDownList_Unit.Items.AddRange(List_itemUnit.ToArray());
                    //Label_Warehouse
                    string warehouse = domComSalesLine.Call("getDefaultWarehouse", fieldValue, SO_number).ToString();//fieldValue=Itemid
                    string[] arr_warehouse = warehouse.Split(',');
                    Label_Warehouse.Text = arr_warehouse[0];
                    //itemPrice
                    string itemPrice = "";
                    string CustAcc = SFA_GET_SALES_ORDER.get_CustAcc(DynAx, SO_number);
                    var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    itemPrice = domComSalesLine.Call("getSalesPrice", fieldValue, itemunit, temp_Today, 1, CustAcc, SO_number).ToString();//fieldValue=Itemid
                    Label_Price.Text = itemPrice;
                    //discount amount
                    string lineDiscAmt = "";
                    lineDiscAmt = domComSalesLine.Call("getLineDiscAmt", fieldValue, itemunit, temp_Today, 1, CustAcc, SO_number).ToString();//fieldValue=Itemid
                    if (lineDiscAmt == "")
                    {
                        Label_Discount.Text = "0";
                    }
                    else
                    {
                        Label_Discount.Text = lineDiscAmt;
                    }
                    //
                    string lineDiscPct = "";
                    lineDiscPct = domComSalesLine.Call("getLineDiscPct", fieldValue, itemunit, temp_Today, 1, CustAcc, SO_number).ToString();//fieldValue=Itemid
                    if (lineDiscPct == "")
                    {
                        Label_Disc_pct.Text = "0";
                    }
                    else
                    {
                        Label_Disc_pct.Text = lineDiscPct;
                    }
                    //
                    Label_MultilineDisc_pct.Text = "0";
                    Label_MultilineDiscount.Text = "0";
                    //
                    string itemIncQty = SFA_GET_SALES_ORDER.get_MultipleQty(DynAx, fieldValue);//fieldValue=Itemid
                    hidden_itemIncQty.Text = itemIncQty;
                    //
                    string configIdStr = "";
                    configIdStr = domComSalesLine.Call("getConfigId", fieldValue).ToString();//fieldValue=Itemid
                    configIdStr = SFA_GET_SALES_ORDER.get_sanitize(configIdStr);
                    hidden_configIdStr.Text = configIdStr;
                    //
                    string sizeStr = "";
                    sizeStr = domComSalesLine.Call("getSizeId", fieldValue).ToString();//fieldValue=Itemid
                    sizeStr = SFA_GET_SALES_ORDER.get_sanitize(sizeStr);
                    hidden_sizeStr.Text = sizeStr;
                    //
                    string colorStr = "";
                    colorStr = domComSalesLine.Call("getColorId", fieldValue).ToString();//fieldValue=Itemid
                    colorStr = SFA_GET_SALES_ORDER.get_sanitize(colorStr);
                    hidden_colorStr.Text = colorStr;
                    //min Qty
                    string LQ = "0";//Lowest Qty

                    if (itemunit == "PC")
                    {
                        LQ = SFA_GET_SALES_ORDER.get_LowestQty(DynAx, fieldValue);//fieldValue=Itemid
                    }
                    hidden_Lowest_Qty.Text = LQ;

                    // record exist
                    /*
                    checkun2.Visible = true;
                    shwimg2.ImageUrl = "RESOURCES/ok.png";
                    lblmsg2.Text = "Item Id Exist.";
                    */
                    //

                    DynRec.Dispose();
                }
                else
                {
                    checkun2.Visible = true;
                    shwimg2.ImageUrl = "RESOURCES/not_ok.png";
                    lblmsg2.Text = "Item Id Not Exist..!!";
                }
            }
            catch (Exception ER_SF_01)
            {
                Function_Method.MsgBox("ER_SF_01: " + ER_SF_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }
        //=========================================================================================================
        //end of sales order
    }
}