using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SalesQuotation : System.Web.UI.Page
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
            sales_order_section_general.Visible = true; Sales_order_section.Visible = false; FV_order_section.Visible = false;
            clear_sales_line(); Label_SQ_No.Text = "SQ number : " + hidden_Label_SQ_No.Text;//12 words + hidden
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
                if (initialize_SaveSalesQuotationLine() == true)
                {
                    sales_order_section_general.Visible = true;
                    Sales_order_section.Visible = false;
                    FV_order_section.Visible = false;
                    clear_sales_line();
                    Label_SQ_No.Text = "SQ number : " + hidden_Label_SQ_No.Text;//12 words + hidden
                    quotation_line_view();
                    Function_Method.MsgBox("Sales quotation line saved.", this.Page, this);
                }
            }
        }

        private bool initialize_SaveSalesQuotationLine()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesQuotationLine"))
                {
                    DynAx.TTSBegin();

                    string DeliveryDate = Request.Form[txtDlvDate.UniqueID];
                    //string CustAcc = TextBox_Account.Text.Trim();
                    //var getLineNumber = DynRec.Call("nextLineNum");
                    //string lineNumber = DynRec.get_Field("LineNum").ToString();
                    string ItemId = TextBox_Id.Text.Trim();
                    string SQ_number = hidden_Label_SQ_No.Text;

                    string selected_item_unit = hidden_selected_item_unit.Text;
                    //var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //var DeliveryDate = Function_Method.get_correct_date(GLOBAL.system_checking, txtDlvDate.Text, false);
                    var temp_deliveryDate = DateTime.ParseExact(DeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string itemQuantity = TextBox_SLQuantity.Text;
                    string itemPrice = Label_Price.Text;

                    string inventDimId = hidden_inventDim.Text;
                    //
                    string axTaxItemGroup = Quotation_Get_Sales_Quotation.get_TaxItemGroup(DynAx, ItemId);
                    //
                    string LFUOM = Quotation_Get_Sales_Quotation.get_UOM(DynAx, selected_item_unit);
                    //====================================================================
                    DynRec.set_Field("QuotationId", SQ_number);
                    DynRec.set_Field("Name", Label_IdName.Text);
                    DynRec.set_Field("ConfirmedDlv", temp_deliveryDate);
                    DynRec.set_Field("ShippingDateRequested", temp_deliveryDate);
                    DynRec.set_Field("CurrencyCode", "MYR");
                    DynRec.set_Field("ItemId", ItemId);
                    DynRec.set_Field("PriceUnit", decimal.Parse(itemQuantity));
                    DynRec.set_Field("QtyOrdered", decimal.Parse(itemQuantity));
                    DynRec.set_Field("SalesUnit", selected_item_unit);
                    DynRec.set_Field("SalesQty", decimal.Parse(itemQuantity));
                    DynRec.set_Field("SalesPrice", decimal.Parse(itemPrice));

                    if (axTaxItemGroup == "OS")
                    {
                        DynRec.set_Field("TaxGroup", axTaxItemGroup);
                    }
                    if (inventDimId != "")
                    {
                        DynRec.set_Field("InventDimId", inventDimId);
                    }

                    int QuotationLineNum = Quotation_Get_Sales_Quotation.get_QuotationIsExist(DynAx, TextBox_Account.Text);
                    if (QuotationLineNum == 0 || QuotationLineNum > 0)
                    {
                        QuotationLineNum++;
                        DynRec.set_Field("LineNum", decimal.Parse(QuotationLineNum.ToString()));
                    }
                    //====================================================================
                    DynRec.Call("insert");//pending
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                    return true;
                }
            }
            catch (Exception ER_SQ_13)
            {
                Function_Method.MsgBox("ER_SQ_13: " + ER_SQ_13.ToString(), this.Page, this);
                return false;
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
                string CustAcc = TextBox_Account.Text.Trim();
                string ItemId = TextBox_Id.Text.Trim();
                string SO_number = hidden_Label_SQ_No.Text;
                string temp_selected_item_unit_name = DropDownList_Unit.Text;
                string[] arr_temp_selected_item_unit_name = temp_selected_item_unit_name.Split('|');
                string selected_item_unit = arr_temp_selected_item_unit_name[0].Trim();
                hidden_selected_item_unit.Text = selected_item_unit;
                var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //====================================================================
                AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");
                //====================================================================
                if (hidden_configIdStr.Text == "" || hidden_sizeStr.Text == "" || hidden_colorStr.Text == "" || Label_Warehouse.Text == "")
                {
                    string inventDimId = domComSalesLine.Call("getInventDimId", ItemId, hidden_configIdStr.Text, hidden_colorStr.Text, hidden_sizeStr.Text, Label_Warehouse.Text).ToString();

                    if (inventDimId == "")
                    {
                        Function_Method.MsgBox("Sales Quotation Line unable to be save. Please check again.", this.Page, this);
                        return false;
                    }
                    else
                    {
                        hidden_inventDim.Text = inventDimId;
                    }
                }

                if (hidden_Lowest_Qty.Text != "")
                {
                    int int_Lowest_Qty = Convert.ToInt32(hidden_Lowest_Qty.Text);
                    int int_SLQuantity = Convert.ToInt32(TextBox_SLQuantity.Text);

                    if (int_SLQuantity < int_Lowest_Qty)
                    {
                        TextBox_SLQuantity.Text = hidden_Lowest_Qty.Text;//auto adjust
                    }
                }

                string itemPrice = domComSalesLine.Call("getSalesPrice", ItemId, selected_item_unit, temp_Today, TextBox_SLQuantity.Text, CustAcc, SO_number).ToString();
                Label_Price.Text = itemPrice;
                return true;
            }
            catch (Exception ER_SF_08)
            {
                Function_Method.MsgBox("ER_SF_08: " + ER_SF_08.ToString(), this.Page, this);
                return false;
            }
        }

        protected void CheckIdInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_SQIM@" + hidden_Label_SQ_No.Text;//SQ > InventoryMaster
            Response.Redirect("InventoryMaster.aspx");
        }

        protected void CheckId(object sender, EventArgs e)
        {
            validate_SL();
        }

        private void validate_SL()
        {
            string fieldValue = TextBox_Id.Text.Trim();
            string SQ_number = hidden_Label_SQ_No.Text;

            //clear_sales_line();

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
                    string warehouse = domComSalesLine.Call("getDefaultWarehouse", fieldValue, SQ_number).ToString();//fieldValue=Itemid
                    string[] arr_warehouse = warehouse.Split(',');
                    Label_Warehouse.Text = arr_warehouse[0];
                    //itemPrice
                    string itemPrice = "";
                    string CustAcc = SFA_GET_SALES_ORDER.get_CustAcc(DynAx, SQ_number);
                    var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    itemPrice = domComSalesLine.Call("getSalesPrice", fieldValue, itemunit, temp_Today, 1, CustAcc, SQ_number).ToString();//fieldValue=Itemid
                    Label_Price.Text = itemPrice;

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
                    //

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
            catch (Exception ER_SQ_01)
            {
                Function_Method.MsgBox("ER_SQ_01: " + ER_SQ_01.ToString(), this.Page, this);
            }
        }
        //=========================================================================================================
        //end of sales order

        protected bool save_QuotationLinetoSalesLine()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                //string CustAcc = TextBox_Account.Text.Trim();
                string ItemId = TextBox_Id.Text.Trim();
                string SQ_number = hidden_Label_SQ_No.Text;
                string deliverydate = Request.Form[txtDlvDate.UniqueID];

                string selected_item_unit = hidden_selected_item_unit.Text;
                //var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var DeliveryDate = Function_Method.get_correct_date(GLOBAL.system_checking, deliverydate, false);
                var temp_deliveryDate = DateTime.ParseExact(DeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string itemQuantity = TextBox_SLQuantity.Text;
                string itemPrice = Label_Price.Text;

                //string LinePercent = Label_Disc_pct.Text;
                //string LineDisc = Label_Discount.Text;

                //string MultiLnPercent = Label_MultilineDisc_pct.Text;
                //string MultiLnDisc = Label_MultilineDiscount.Text;

                string inventDimId = hidden_inventDim.Text;
                //
                string axTaxItemGroup = SFA_GET_SALES_ORDER_2.get_TaxItemGroup(DynAx, ItemId);
                //
                string LF_MixPriceGroup = ""; string LF_MixPricePromo = "";
                var tuple_getMPP_MPG = SFA_GET_SALES_ORDER_2.get_MPP_MPG(DynAx, SQ_number);
                LF_MixPricePromo = tuple_getMPP_MPG.Item1;
                LF_MixPriceGroup = tuple_getMPP_MPG.Item2;
                //
                string LFUOM = SFA_GET_SALES_ORDER_2.get_UOM(DynAx, selected_item_unit);
                //====================================================================
                AxaptaObject axSalesLine = DynAx.CreateAxaptaObject("AxSalesQuotationLine");

                int QuotationLineNum = Quotation_Get_Sales_Quotation.get_QuotationIsExist(DynAx, TextBox_Account.Text);
                if (QuotationLineNum >= 0)
                {
                    QuotationLineNum++;
                    axSalesLine.Call("parmLineNum", decimal.Parse(QuotationLineNum.ToString()));
                }

                axSalesLine.Call("parmReceiptDateRequested", DateTime.Now);
                axSalesLine.Call("parmShippingDateRequested", DateTime.Now);
                axSalesLine.Call("parmConfirmedDlv", DateTime.Now);
                axSalesLine.Call("parmItemId", ItemId);
                axSalesLine.Call("parmSalesUnit", selected_item_unit);
                axSalesLine.Call("parmSalesQty", decimal.Parse(itemQuantity));
                axSalesLine.Call("parmSalesPrice", decimal.Parse(itemPrice));
                axSalesLine.Call("parmLinePercent", txt_Disc_pct.Text);
                axSalesLine.Call("parmMultiLnPercent", txt_MultilineDisc_pct.Text);
                axSalesLine.Call("parmMultiLnDisc", txt_MultilineDiscount.Text);
                axSalesLine.Call("parmCurrencyCode", "MYR");

                //var tuple_SalesQuotationTable = Quotation_Get_Sales_Quotation.reload_from_SalesQuotationTable(DynAx, SO_number);
                //axSalesLine.Call("parmCustAccount", tuple_SalesQuotationTable.Item2);
                //DynRec.set_Field("MixPriceGroup", LF_MixPriceGroup);
                //DynRec.set_Field("UOM", LFUOM);
                //DynRec.set_Field("MixPricePromo", LF_MixPricePromo);

                if (axTaxItemGroup == "OS")
                {
                    axSalesLine.Call("parmTaxGroup", axTaxItemGroup);
                }
                if (inventDimId != "")
                {
                    axSalesLine.Call("parmInventDimId", inventDimId);
                }
                axSalesLine.Call("newSalesQuotationLine");

                return true;
            }
            catch (Exception ER_SQ_09)
            {
                Function_Method.MsgBox("ER_SQ_09: " + ER_SQ_09.ToString(), this.Page, this);
                return false;
            }
        }
    }
}