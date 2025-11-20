using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;

namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                    null, CustomerMasterTag2,
                    null, SFATag2,
                    null, SalesQuotation2,
                    null, PaymentTag2,
                    null, RedemptionTag2,
                    null, InventoryMasterTag2,

                    null, EORTag2,
                    null, null,
                    null, WClaimTag2,
                    null, EventBudgetTag2,
                    null, null,
                    null, null, null, RocTinTag2,
                    NewProduct2
                    );

                clear_sales_header();
                clear_sales_line();
                Check_DataRequest();
                Session["data_passing"] = "";//in case forget reset
                Session["flag_temp"] = 0;
            }
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        private void check_session()
        {
            try
            {
                //load session user
                GLOBAL.user_id = Session["user_id"].ToString();
                GLOBAL.axPWD = Session["axPWD"].ToString();
                GLOBAL.logined_user_name = Session["logined_user_name"].ToString();
                GLOBAL.user_authority_lvl = Convert.ToInt32(Session["user_authority_lvl"]);
                GLOBAL.page_access_authority = Convert.ToInt32(Session["page_access_authority"]);
                GLOBAL.user_company = Session["user_company"].ToString();
                GLOBAL.module_access_authority = Convert.ToInt32(Session["module_access_authority"]);
                GLOBAL.switch_Company = Session["switch_Company"].ToString();
                GLOBAL.system_checking = Convert.ToInt32(Session["system_checking"]);
                GLOBAL.data_passing = Session["data_passing"].ToString();
            }
            catch
            {
                Session.Clear();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void Check_DataRequest()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string temp1 = GLOBAL.data_passing.ToString();
                if (temp1 != "")//data receive
                {
                    //cbAutoPost.Visible = true;
                    if (temp1.Length >= 6)//correct size
                    {
                        //string test = temp1.Substring(0, 6);
                        Button_Submit_SalesOrderLine.Visible = true;
                        Button_UpdateHeader.Visible = true;
                        Button_SaveSalesOrder.Visible = true;
                        Button_SaveFavourite.Visible = true;
                        Button_Reset_CampaignID.Visible = true;
                        if (temp1.Substring(0, 6) == "@SFCM_")//Data receive from CustomerMaster> SFA
                        {
                            string temp3 = temp1.Substring(6).Split('|')[0];
                            TextBox_Account.Text = temp1.Substring(6);
                            validate();
                            Button_CheckAcc.Visible = false;
                            //cbAutoPost.Checked = true;
                            Alt_Addr_function();
                            Button_UpdateHeader.Visible = false;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_header_section'); ", true);//go to Overall_sales_order_section
                        }
                        else if (temp1.Substring(0, 6) == "@SFIM_")//Data receive from InventoryMaster> SFA
                        {
                            string SO_Id_selected_ItemId = temp1.Substring(6);
                            string[] arr_SO_Id_selected_ItemId = SO_Id_selected_ItemId.Split('|');
                            string SO_Id = arr_SO_Id_selected_ItemId[0];
                            string selected_ItemId = arr_SO_Id_selected_ItemId[1];

                            /*start of reloading data*/

                            reloading_data(DynAx, SO_Id);
                            /*end of reloading data*/
                            TextBox_Id.Text = selected_ItemId;

                            validate_SL();

                            Sales_order();
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_order_section'); ", true);//go to Overall_sales_order_section                             
                        }
                        else if (temp1.Substring(0, 6) == "@SFSF_")//Data request from SFA> SFA
                        {
                            string SO_Id = temp1.Substring(6).Split('|')[0];
                            /*start of reloading data*/
                            reloading_data(DynAx, SO_Id);
                            /*end of reloading data*/

                            //Remove these button when its ready
                            if (temp1.Substring(6).Split('|')[1] == "Yes")
                            {
                                Button_Submit_SalesOrderLine.Visible = false;
                                Button_UpdateHeader.Visible = false;
                                Button_SaveSalesOrder.Visible = false;
                                Button_SaveFavourite.Visible = false;
                                Button_Reset_CampaignID.Visible = false;
                            }

                            //-----------------------------------------
                            sales_order_section_general.Visible = true; Sales_order_section.Visible = false; FV_order_section.Visible = false;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_order_section'); ", true);//go to Overall_sales_order_section
                        }
                        else if (temp1.Substring(0, 6) == "@SFS2_")//refresh after submit/ post
                        {
                            string SO_Id = temp1.Substring(6).Split('|')[0];
                            Function_Method.MsgBox("SO number: " + SO_Id + " successfully submitted.", this.Page, this);
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_confirmation_section'); ", true);//go to overview section
                        }
                        else if (temp1.Substring(0, 6) == "@WASFA")//warranty to SFA
                        {
                            confirmation_section_Click(Button_confirmation_section, EventArgs.Empty);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_header_section'); ", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_header_section'); ", true);
                    }
                    Session["data_passing"] = "";
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_header_section'); ", true);
                }
            }
            catch (Exception ER_SF_07)
            {
                Function_Method.MsgBox("ER_SF_07: " + ER_SF_07.ToString(), this.Page, this);
            }
        }

        private void reloading_data(Axapta DynAx, string SO_Id)
        {
            TextBox_Account.Text = SFA_GET_SALES_ORDER.get_CustAcc(DynAx, SO_Id);
            validate();//auto fill up [TextBox_Account, Label_CustName,Label_Address] in Customer accordion

            hidden_Label_SO_No.Text = SO_Id;
            //Some variable data need to derived from SalesTable manually
            var tuple_reload = SFA_GET_SALES_ORDER.reload_from_SalesTable(DynAx, SO_Id);

            //Customer accordion
            TextBox_References.Text = tuple_reload.Item1;
            //Delivery accordion
            string temp_DeliveryDate = tuple_reload.Item2;
            string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
            string Raw_DeliveryDate = arr_temp_DeliveryDate[0];
            txtDeliveryDate.Text = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DeliveryDate, true);

            Label_Receiver_Name.Text = tuple_reload.Item3;

            string temp_Delivery_Addr = tuple_reload.Item4;
            if (temp_Delivery_Addr == Label_Address.Text)
            {
                Label_Delivery_Addr.Text = "-same as primary address-";
            }
            else
            {
                Label_Delivery_Addr.Text = temp_Delivery_Addr;
            }
            Label_SalesLine_Address.Text = temp_Delivery_Addr;
            TextBox_Notes.Text = tuple_reload.Item5;

            var tuple_reload_set2 = SFA_GET_SALES_ORDER.reload_from_SalesTableSet2(DynAx, SO_Id);
            hidden_Street.Text = tuple_reload_set2.Item1;
            hidden_ZipCode.Text = tuple_reload_set2.Item2;
            hidden_City.Text = tuple_reload_set2.Item3;
            hidden_State.Text = tuple_reload_set2.Item4;
            hidden_Country.Text = tuple_reload_set2.Item5;
            //cbAutoPost.Checked = (tuple_reload_set2.Item6 == "1" ? true : false);
            string hidden_LFCampaignID = tuple_reload_set2.Item7; 
            if (hidden_LFCampaignID == "")
            {
                Button_Reset_CampaignID.Visible = false; 
            }
            else
            {
                Button_Reset_CampaignID.Visible = true; 
            }

            string temp_LFI_SalesStatus = SFA_GET_SALES_ORDER_2.get_LFI_SalesStatus(DynAx, SO_Id, tuple_reload.Item6);
            string Status = SFA_GET_SALES_ORDER_2.get_AxEnumSalesStatus(temp_LFI_SalesStatus);

            if (Status != "Open Order")
            {
                Button_Submit_SalesOrderLine.Visible = false;
                Button_UpdateHeader.Visible = false;
                Button_SaveSalesOrder.Visible = false;
                Button_SaveFavourite.Visible = false;
                Button_Reset_CampaignID.Visible = false;
            }
            Alt_Addr_function();
        }

        #region ResetCampaignID
        private static void ResetCampaignId(Axapta DynAx, string salesId)
        {
            try
            {
                #region SalesTable_Reset
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesTable"))
                {
                    DynAx.TTSBegin();
                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1  where %1.{0} == '{1}'", "SalesId", salesId));
                    if (DynRec.Found)
                    {

                        string campaignId = DynRec.get_Field("LF_CampaignID").ToString();

                        ResetSalesLines_Promotion(DynAx, salesId);

                        // Update SalesTable - clear campaign ID
                        DynRec.set_Field("LF_CampaignID", "");
                        DynRec.set_Field("PRS_Blocked", 0);
                        DynRec.set_Field("PRS_Released", 0);
                        DynRec.set_Field("LFI_HQPosted", 0);
                        DynRec.set_Field("LFI_Submitted", 0);

                        DynRec.Call("update");
                        DynAx.TTSCommit();
                        logger.Info($"Reset Campaign ID in Sales ID : {salesId}");
                        DynAx.TTSAbort();
                    }
                    else
                    {
                        logger.Warn($"SalesId {salesId} not found in SalesTable.");
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                DynAx.TTSAbort();
                throw new Exception("Error resetting campaign ID: " + ex.Message);
            }
        }

        private static void ResetSalesLines_Promotion(Axapta DynAx, string salesId)
        {
            const int SalesLine = 359;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesLine);
            Dictionary<string, string> ItemIdList = new Dictionary<string, string>();

            // Add ranges
            AxaptaObject qbrSalesId = (AxaptaObject)axDataSource.Call("addRange", 1);
            qbrSalesId.Call("value", salesId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            // Start transaction
            try
            {
                // First pass: Collect records
                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("get", SalesLine);

                    string ItemID = DynRec.get_Field("ItemId").ToString();
                    string LFI_POInventRefId = DynRec.get_Field("LFI_POInventRefId").ToString();

                    // Store ItemID and LFI_POInventRefId in the dictionary
                    ItemIdList[ItemID] = LFI_POInventRefId;
                }

                // Second pass: Update records where LFI_POInventRefId is "SYS"
                foreach (var item in ItemIdList)
                {
                    string ItemID = item.Key;
                    string LFI_POInventRefId = item.Value;

                    if (LFI_POInventRefId == "SYS")
                    {
                        // Ensure the record is selected for update
                        AxaptaRecord rec = (AxaptaRecord)axQueryRun.Call("get", SalesLine);
                        rec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}' && %1.{2} == '{3}'", "SalesId", salesId, "ItemId", ItemID));

                        // Update fields
                        rec.set_Field("QtyOrdered", 0m);
                        rec.set_Field("RemainSalesPhysical", 0m);
                        rec.set_Field("SalesQty", 0m);
                        rec.set_Field("RemainInventPhysical", 0m);
                        rec.Call("update");

                        Console.WriteLine($"Done reset the campaign ID for item: {ItemID}");
                    }
                }

                // Commit transaction
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                throw new Exception("Error resetting sales lines: " + ex.Message);
            }
        }

        protected void Button_Reset_CampaignID_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            string SO_number = hidden_Label_SO_No.Text;
            ResetCampaignId(DynAx, SO_number);
            Function_Method.MsgBox("Campaign ID reset successfully.", this.Page, this);
        }
        #endregion

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void sales_header_section_Click(object sender, EventArgs e)
        {
            sales_header_section.Attributes.Add("style", "display:initial"); Button_sales_header_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_sales_order_section.Attributes.Add("style", "background-color:transparent");
            confirmation_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            TextBox_Account_TextChanged(TextBox_Account, EventArgs.Empty);
        }

        protected void sales_order_section_Click(object sender, EventArgs e)
        {
            if (hidden_Label_SO_No.Text == "")
            {
                Button_UpdateHeader.Visible = false;
                Button_SaveHeader.Visible = true;
                TextBox_Account.Enabled = true;
                Button_CustomerMasterList.Enabled = true;
                Button_CheckAcc.Enabled = true;
                Function_Method.MsgBox("There is no Sales Order Number.", this.Page, this);
                return;
            }
            else
            {
                //Button_UpdateHeader.Visible = true;
                Button_SaveHeader.Visible = false;
                TextBox_Account.Enabled = false;
                Button_CustomerMasterList.Enabled = false;
                Button_CheckAcc.Enabled = false;
                Label_SO_No.Text = "SO number : " + hidden_Label_SO_No.Text;//12 words + hidden
            }

            /*
            //lock the textbox and button from editting in Sales Header
            TextBox_Account.ReadOnly = true; TextBox_Account.Attributes.Add("style", "background-color:#ffdf87");
            TextBox_References.ReadOnly = true; TextBox_References.Attributes.Add("style", "background-color:#ffdf87");

            Button_CustomerMasterList.Enabled = false;
            TextBox_Notes.ReadOnly = true; TextBox_Notes.Attributes.Add("style", "background-color:#ffdf87");
            Button_Delivery_Addr.Enabled = false;
            */
            sales_header_section.Attributes.Add("style", "display:none"); Button_sales_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:initial"); Button_sales_order_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            confirmation_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            sales_line_view();//run grid sales line view
        }

        protected void confirmation_section_Click(object sender, EventArgs e)
        {
            sales_header_section.Attributes.Add("style", "display:none"); Button_sales_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_sales_order_section.Attributes.Add("style", "background-color:transparent");
            confirmation_section.Attributes.Add("style", "display:initial"); Button_confirmation_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            //first, go to ListOutStanding
            f_Button_ListOutStanding();
        }

        protected void enquiries_section_Click(object sender, EventArgs e)
        {
            if (f_Button_SalesmanTotal() == false) //first, go to SalesmanTotal
            {
                return;
            }
            sales_header_section.Attributes.Add("style", "display:none"); Button_sales_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_sales_order_section.Attributes.Add("style", "background-color:transparent");
            confirmation_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:initial"); Button_enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
        }

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        private void clear_sales_header()
        {
            //Customer
            TextBox_Account.Text = "";
            Label_CustName.Text = "";//Name
            Label_Address.Text = "";//Address
            TextBox_References.Text = "";

            //Delivery
            txtDeliveryDate.Text = ""; Label_Receiver_Name.Text = ""; Label_Delivery_Addr.Text = "";

            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";
            hidden_Street.Text = ""; hidden_ZipCode.Text = ""; hidden_City.Text = "";
            hidden_State.Text = ""; hidden_Country.Text = "";

            GridView_AltAddress.DataSource = null; GridView_AltAddress.DataBind(); GridView_AltAddress.Visible = false;

            //Sales Order
            Label_Payment_Terms.Text = "";
            Label_Suffix_Code.Text = "";
            TextBox_Notes.Text = "";
            hidden_Label_SO_No.Text = "";

            Button_Delivery_Addr.Visible = false;
            checkun.Visible = false;
        }

        private void clear_sales_line()
        {
            Label_SO_No.Text = "";
            GridView_FVOrder.DataSource = null; GridView_FVOrder.DataBind(); //GridView_FVOrder.Visible = false;

            checkun2.Visible = false;
            TextBox_Id.Text = "";
            Label_IdName.Text = "";
            Label_Warehouse.Text = "";
            DropDownList_Unit.Items.Clear();
            TextBox_SLQuantity.Text = "";
            Label_Price.Text = "";
            Label_Disc_pct.Text = "";
            Label_Discount.Text = "";
            Label_MultilineDisc_pct.Text = "";
            Label_MultilineDiscount.Text = "";

            hidden_itemIncQty.Text = "";
            hidden_configIdStr.Text = "";
            hidden_sizeStr.Text = "";
            hidden_colorStr.Text = "";
            hidden_Lowest_Qty.Text = "";

            hidden_selected_item_unit.Text = "";
            hidden_inventDim.Text = "";
        }

        //start of sales line section
        //start of sales line view
        protected void Button_Submit_SalesOrderLine_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            Button_Submit_SalesOrderLine.Enabled = false;//to prevent double entry

            string SO_number = hidden_Label_SO_No.Text;
            if (SO_number == "")
            {
                Function_Method.MsgBox("There is no SO number", this.Page, this);
                return;
            }
            try
            {
                if (promo_layer1(DynAx, SO_number) == false)//no promotion
                {
                    if (Post_Confirm_SO() == true)
                    {
                        string SO_no = hidden_Label_SO_No.Text;
                        Session["data_passing"] = "@SFS2_" + SO_no;//reload
                        Response.Redirect("SFA.aspx");
                    }
                }
                Button_Submit_SalesOrderLine.Enabled = true;
            }
            catch (Exception ER_SF_10)
            {
                Function_Method.MsgBox("ER_SF_10: " + ER_SF_10.ToString(), this.Page, this);
            }
        }

        private bool Post_Confirm_SO()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            string SO_number = hidden_Label_SO_No.Text;

            try
            {
                AxaptaObject domComSalesTable = DynAx.CreateAxaptaObject("domComSalesTable");
                domComSalesTable.Call("Update_Submitted", SO_number);
                domComSalesTable.Call("Post_Confirmation", SO_number);
                return true;
            }
            catch (Exception ER_SF_19)
            {
                Function_Method.MsgBox("ER_SF_19: " + ER_SF_19.ToString(), this.Page, this);
                return false;
            }
        }
        //Promotion
        protected void Button_Next_Promotion_Click(object sender, EventArgs e)
        {
            bool exception = false;
            bool post_confirm = false;
            try
            {
                if (Promotion_Layer1.Visible == true && Promotion_Layer2.Visible == false)//from Layer 1
                {
                    hidden_layer2_SO_number.Text = ""; hidden_layer2_camp_id.Text = ""; hidden_layer2_camp_type.Text = "";
                    var tuple_promo_layer2_checking = promo_layer2_checking();
                    bool condition = tuple_promo_layer2_checking.Item1;
                    hidden_layer2_SO_number.Text = tuple_promo_layer2_checking.Item2;
                    hidden_layer2_camp_id.Text = tuple_promo_layer2_checking.Item3;
                    hidden_layer2_camp_type.Text = tuple_promo_layer2_checking.Item4;
                    if (condition == false && hidden_layer2_SO_number.Text == "")
                    {
                        Function_Method.MsgBox("No promotion selected.", this.Page, this);
                    }
                    if (condition == false && hidden_layer2_SO_number.Text != "")//no promotion id found, stay same page
                    {
                        Function_Method.MsgBox("Selected promotion did not shows any results. Please check with admin.", this.Page, this);
                    }
                }
                else if (Promotion_Layer1.Visible == false && Promotion_Layer2.Visible == true)//from Layer 2
                {
                    if (promo_layer3_checking() == true)
                    {
                        //proceed next
                        string error_layer3 = promo_layer3();
                        if (error_layer3 != "")
                        {
                            exception = true;
                            Function_Method.MsgBox(error_layer3, this.Page, this);
                        }
                        else
                        {
                            if (Post_Confirm_SO() == true)
                            {
                                post_confirm = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ER_SF_14)
            {
                exception = true;
                Function_Method.MsgBox("ER_SF_14: " + ER_SF_14.ToString(), this.Page, this);
            }
            finally
            {
                if (!exception)
                {
                    if (post_confirm == true)
                    {
                        string SO_number = hidden_Label_SO_No.Text;
                        Session["data_passing"] = "@SFS2_" + SO_number;//reload
                        Response.Redirect("SFA.aspx");
                    }
                }
            }
        }

        protected void Button_Back_Promotion_Click(object sender, EventArgs e)
        {
            if (Promotion_Layer1.Visible == true && Promotion_Layer2.Visible == false)//from Layer 1
            {
                Promotion_Campaign_section.Visible = false; sales_order_section_general.Visible = true; Promotion_Layer1.Visible = false;
            }
            else if (Promotion_Layer2.Visible == true && Promotion_Layer1.Visible == false)//from Layer 2
            {
                Promotion_Layer1.Visible = true; Promotion_Layer2.Visible = false; //reset promo campaign after click back

                //clear checkbox====
                foreach (GridViewRow row in GridView_Promotion_Campaign.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox CheckBox_selection = (row.Cells[0].FindControl("chkRow_Promotion_Campaign") as CheckBox);
                        if (CheckBox_selection.Checked)
                        {
                            CheckBox_selection.Checked = false;
                        }
                    }
                }
            }
            else//default, from Layer 1
            {
                Promotion_Campaign_section.Visible = false; sales_order_section_general.Visible = true; Promotion_Layer1.Visible = false;
            }
        }

        protected void CheckBox_Promotion_Campaign(object sender, EventArgs e)
        {
            //get the the id that call the function
            CheckBox CheckBox_selection1 = sender as CheckBox;

            string ClientID = CheckBox_selection1.ClientID;

            //string[] arr_ClientID = ClientID.Split('_');
            //int arr_count=arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count-1]);
            //
            int row_count = GridView_Promotion_Campaign.Rows.Count;
            for (int i = 0; i < row_count; i++)
            {
                CheckBox CheckBox_selection = (GridView_Promotion_Campaign.Rows[i].Cells[0].FindControl("chkRow_Promotion_Campaign") as CheckBox);
                if (GridView_Promotion_Campaign.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (i > 1)//allow only one selection
                    {
                        CheckBox_selection.Checked = false;
                    }

                    if (CheckBox_selection.Checked)//highlight
                    {
                        //GridView_Promotion_Campaign.Rows[i].BackColor = Color.FromName("#ff8000");
                        //auto enter ========================================================
                        hidden_layer2_SO_number.Text = ""; hidden_layer2_camp_id.Text = ""; hidden_layer2_camp_type.Text = "";
                        var tuple_promo_layer2_checking = promo_layer2_checking();
                        bool condition = tuple_promo_layer2_checking.Item1;
                        hidden_layer2_SO_number.Text = tuple_promo_layer2_checking.Item2;
                        hidden_layer2_camp_id.Text = tuple_promo_layer2_checking.Item3;
                        hidden_layer2_camp_type.Text = tuple_promo_layer2_checking.Item4;
                        if (condition == false && hidden_layer2_SO_number.Text == "")
                        {
                            Function_Method.MsgBox("No promotion selected.", this.Page, this);
                        }
                        if (condition == false && hidden_layer2_SO_number.Text != "")//no promotion id found, stay same page
                        {
                            Function_Method.MsgBox("Selected promotion did not shows any results. Please check with admin.", this.Page, this);
                        }
                        //========================================================================
                    }
                }
            }
        }

        private bool promo_layer1(Axapta DynAx, string SO_number)
        {
            try
            {
                Promotion_Layer1.Visible = true; Promotion_Layer2.Visible = false;
                string CustAcc = SFA_GET_SALES_ORDER.get_CustAcc(DynAx, SO_number);//get custaccount
                var getEmplID = SFA_GET_Enquiries_BatteryOutstanding.getEmplId(DynAx, CustAcc);//get empl ID
                string EmpEmail = SalesReport_Get_Budget.getEmail(DynAx, getEmplID.Item1);
                //string user_email = GLOBAL.user_id + "@posim.com.my";

                string getCampaignId = "0";

                getCampaignId = SFA_GET_PROMO_LAYER1.getCampaignId(DynAx, SO_number, EmpEmail);
                if (string.IsNullOrEmpty(EmpEmail))
                {
                    Function_Method.MsgBox("This salesman not valid for any campaign.", this.Page, this);
                    return true;
                }//no email
                if (getCampaignId == "0")
                {
                    return false;//no campaign id
                }
                else
                {
                    Promotion_Campaign_section.Visible = true; sales_order_section_general.Visible = false;
                    GridView_Promotion_Campaign.DataSource = null;
                    GridView_Promotion_Campaign.DataBind();

                    var tuple_GeneralSplit_TotalCount_id_type = SFA_GET_PROMO_LAYER1.GeneralSplit_TotalCount_id_type(getCampaignId);
                    int TotalCount = tuple_GeneralSplit_TotalCount_id_type.Item1;
                    string[] camp_id = tuple_GeneralSplit_TotalCount_id_type.Item2;
                    string[] camp_type = tuple_GeneralSplit_TotalCount_id_type.Item3;

                    string[] detailname = new string[TotalCount];
                    for (int j = 0; j < TotalCount; j++)
                    {
                        detailname[j] = SFA_GET_PROMO_LAYER1.getdetailname(DynAx, camp_id[j]);
                    }

                    if (TotalCount == 0)
                    {
                        //no campaign
                        return false;
                    }

                    //show in table
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Campaign"), new DataColumn("Description"), new DataColumn("Type") });

                    for (int i = 0; i < TotalCount; i++)
                    {
                        dt.Rows.Add(camp_id[i], detailname[i], camp_type[i]);
                    }
                    GridView_Promotion_Campaign.Columns[3].Visible = true;//camp_type
                    GridView_Promotion_Campaign.DataSource = dt;
                    GridView_Promotion_Campaign.DataBind();
                    return true;
                }
            }
            catch (Exception ER_SF_16)
            {
                Function_Method.MsgBox("ER_SF_16: " + ER_SF_16.ToString(), this.Page, this);
                return false;
            }
        }

        //========================================================================================
        protected void Button_Delete_SalesOrderLine_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string error = "";
                foreach (GridViewRow row in GridView_Sales_Order_Line_View.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox CheckBox_selection = (row.Cells[0].FindControl("CheckBox_ToDeleteByOne") as CheckBox);
                        if (CheckBox_selection.Checked)
                        {
                            GridView_Sales_Order_Line_View.Columns[13].Visible = true;
                            GridView_Sales_Order_Line_View.Columns[14].Visible = true;
                            string SalesLine_RecId = row.Cells[13].Text;
                            string allow_alter = row.Cells[14].Text;
                            if (allow_alter == "1")//can be updated
                            {
                                error += SFA_GET_SALES_ORDER_2.get_delete_saleline(DynAx, SalesLine_RecId);
                            }
                            else
                            {
                                error += "Cannot delete Sales Line " + SalesLine_RecId + " that has been created before.";
                            }
                        }
                    }
                }
                if (error != "")
                {
                    Function_Method.MsgBox(error, this.Page, this);
                }
                sales_line_view();
            }
            catch (Exception ER_SF_23)
            {
                Function_Method.MsgBox("ER_SF_23: " + ER_SF_23.ToString(), this.Page, this);
            }
        }

        protected void CheckBox_Changed_ToDeleteAll(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView_Sales_Order_Line_View.HeaderRow.FindControl("CheckBox_ToDeleteAll");

            foreach (GridViewRow row in GridView_Sales_Order_Line_View.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("CheckBox_ToDeleteByOne");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                    //row.BackColor = Color.FromName("#ff8000");
                    Button_Delete_SalesOrderLine.Visible = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                    Button_Delete_SalesOrderLine.Visible = false;
                }
            }
        }

        protected void CheckBox_Changed_ToDeleteByOne(object sender, EventArgs e)
        {
            int row_count = GridView_Sales_Order_Line_View.Rows.Count;

            int count_checked = 0;
            for (int i = 0; i < row_count; i++)
            {
                CheckBox CheckBox_selection = (GridView_Sales_Order_Line_View.Rows[i].Cells[0].FindControl("CheckBox_ToDeleteByOne") as CheckBox);
                if (GridView_Sales_Order_Line_View.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (CheckBox_selection.Checked)//highlight
                    {
                        count_checked = count_checked + 1;
                        GridView_Sales_Order_Line_View.Rows[i].BackColor = Color.FromName("#ff8000");
                    }
                }
            }

            //
            CheckBox ChkBoxHeader = (CheckBox)GridView_Sales_Order_Line_View.HeaderRow.FindControl("CheckBox_ToDeleteAll");
            if (count_checked == row_count)
            {
                ChkBoxHeader.Checked = true;
            }
            else
            {
                ChkBoxHeader.Checked = false;
            }
            //
            if (count_checked != 0)
            {
                Button_Delete_SalesOrderLine.Visible = true;
            }
            else
            {
                Button_Delete_SalesOrderLine.Visible = false;
            }
        }

        private void sales_line_view()
        {

            Button_Delete_SalesOrderLine.Visible = false;
            GridView_Sales_Order_Line_View.Columns[13].Visible = true;
            GridView_Sales_Order_Line_View.Columns[14].Visible = true;

            GridView_Sales_Order_Line_View.DataSource = null;
            GridView_Sales_Order_Line_View.DataBind();
            //
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                int SalesLine = 359;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesLine);

                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//SalesId
                qbr1.Call("value", hidden_Label_SO_No.Text);
                axQueryDataSource.Call("addSortField", 2, 0);//Line Number, asc

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 14;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Misc Charge"; N[2] = "Item Id";
                N[3] = "Item Name"; N[4] = "Unit"; N[5] = "Quantity";

                N[6] = "Price Each"; N[7] = "Net Amount"; N[8] = "Dlv Date";
                N[9] = "Inv Qty"; N[10] = "Dlv Qty"; N[11] = "Dlv Remainder";
                N[12] = "Hidden_RecId"; N[13] = "Hidden_allow_alter";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================

                DataRow row;
                int countA = 0;
                //for checking sales status, so that the saleline can be delete or update
                string temp_SalesStatus = SFA_GET_SALES_ORDER_2.get_SalesStatus(DynAx, hidden_Label_SO_No.Text);
                //
                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesLine);
                    countA = countA + 1;

                    row = dt.NewRow();
                    //transfer to grid
                    row["No."] = countA.ToString();

                    string temp_Misc_Charge = DynRec.get_Field("LFI_MiscChargeIndicator").ToString();
                    if (temp_Misc_Charge == "0")
                    {
                        row["Misc Charge"] = "No";
                    }
                    else
                    {
                        row["Misc Charge"] = "Yes";
                    }

                    row["Item Id"] = DynRec.get_Field("ItemId").ToString();
                    row["Item Name"] = DynRec.get_Field("Name").ToString();
                    row["Unit"] = DynRec.get_Field("SalesUnit").ToString();
                    row["Quantity"] = DynRec.get_Field("SalesQty").ToString();

                    row["Price Each"] = DynRec.get_Field("SalesPrice").ToString();
                    row["Net Amount"] = DynRec.get_Field("LineAmount").ToString();
                    //
                    string temp_InvDate = DynRec.get_Field("ConfirmedDlv").ToString();
                    string InvDate = "";
                    if (temp_InvDate != "")
                    {
                        string[] arr_temp_InvDate = temp_InvDate.Split(' ');//date + " " + time;
                        string Raw_InvDate = arr_temp_InvDate[0];
                        InvDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_InvDate, true);
                    }
                    row["Dlv Date"] = InvDate;
                    //
                    string temp_INVENTTRANSID = DynRec.get_Field("INVENTTRANSID").ToString();

                    AxaptaObject SalesLinetype = DynAx.CreateAxaptaObject("SalesLinetype");

                    var temp_InvoicedInTotal = SalesLinetype.Call("invoicedInTotal");
                    var temp_DeliveredInTotal = SalesLinetype.Call("deliveredInTotal");
                    row["Inv Qty"] = temp_InvoicedInTotal.ToString();
                    row["Dlv Qty"] = temp_DeliveredInTotal.ToString();
                    //
                    row["Dlv Remainder"] = DynRec.get_Field("RemainSalesPhysical").ToString();
                    row["Hidden_RecId"] = DynRec.get_Field("RecId").ToString();

                    if (temp_SalesStatus == "5")//partially delivered
                    {
                        string temp_createdDateTime = DynRec.get_Field("createdDateTime").ToString();

                        string createdDate = "";
                        if (temp_createdDateTime != "")
                        {
                            string[] arr_temp_createdDateTime = temp_createdDateTime.Split(' ');//date + " " + time;
                            string Raw_temp_createdDateTime = arr_temp_createdDateTime[0];
                            createdDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_temp_createdDateTime, true);
                        }
                        string today_date = DateTime.Now.ToString("dd/MM/yyyy");
                        if (createdDate == today_date)
                        {
                            row["Hidden_allow_alter"] = "1";
                        }
                        else
                        {
                            row["Hidden_allow_alter"] = "0";
                        }
                    }
                    else
                    {
                        row["Hidden_allow_alter"] = "1";
                    }

                    dt.Rows.Add(row);
                    DynRec.Dispose();
                    Button_Sales_Order_Line.Visible = true; Button_Submit_SalesOrderLine.Visible = true;
                }
                if (countA == 0)//grid empty
                {
                    //close grid
                    Button_Sales_Order_Line.Visible = false; Button_Submit_SalesOrderLine.Visible = false;
                    return;
                }


                GridView_Sales_Order_Line_View.DataSource = dt;
                GridView_Sales_Order_Line_View.DataBind();
                GridView_Sales_Order_Line_View.Columns[13].Visible = false;
                GridView_Sales_Order_Line_View.Columns[14].Visible = false;
                GridView_Sales_Order_Line_View.Columns[0].Visible = true;

                if (Button_Submit_SalesOrderLine.Visible == false)
                {
                    GridView_Sales_Order_Line_View.Columns[0].Visible = false;
                }
            }
            catch (Exception ER_SF_06)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_SF_06: " + ER_SF_06.ToString(), this.Page, this);
            }
        }

        protected void Button_Favourite_Click(object sender, ImageClickEventArgs e)
        {
            FV_order();
        }

        protected void Button_SalesOrder_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            Sales_order();
        }
        //end of sales line view     

        //end of sales line section

        //start of OverviewSL section
        protected void Button_ListOutStanding_Click(object sender, EventArgs e)
        {
            f_Button_ListOutStanding();
        }

        private void f_Button_ListOutStanding()
        {
            Session["flag_temp"] = 0;//List Outstanding
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//Sales Id button
            GridViewOverviewList.Columns[2].Visible = false;//Sales Id label
            grdCharges.Visible = true;
            dvDeliveryTracking.Visible = false;
            OverviewSL(0, ""); TextBox_Search.Text = "";
            confirmation_section.Attributes.Add("style", "display:initial");
            Button_ListOutStanding.Attributes.Add("style", "background-color:#f58345");
            Button_ListAll.Attributes.Add("style", "background-color:#transparent");
            Button_ListOutStandingBlocked.Attributes.Add("style", "background-color:#transparent");
            Button_DeliverTrack.Attributes.Add("style", "background-color:#transparent");
            confirmation_section_general.Visible = true; Button_Overview_accordion.Text = "List Outstanding";
        }

        protected void Button_ListAll_Click(object sender, EventArgs e)
        {
            //Session["flag_temp"] = 2;//List All 
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//Sales Id button
            GridViewOverviewList.Columns[2].Visible = false;//Sales Id label
            grdCharges.Visible = true;
            dvDeliveryTracking.Visible = false;
            OverviewSL(0, ""); TextBox_Search.Text = "";
            confirmation_section.Attributes.Add("style", "display:initial");
            Button_ListAll.Attributes.Add("style", "background-color:#f58345");
            Button_ListOutStanding.Attributes.Add("style", "background-color:transparent");
            Button_ListOutStandingBlocked.Attributes.Add("style", "background-color:#transparent");
            Button_DeliverTrack.Attributes.Add("style", "background-color:#transparent");
            confirmation_section_general.Visible = true; Button_Overview_accordion.Text = "List All";
        }

        protected void Button_ListOutStandingBlocked_Click(object sender, EventArgs e)
        {
            Session["flag_temp"] = 1;//List Outstanding Blocked
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = false;//Sales Id button
            GridViewOverviewList.Columns[2].Visible = true;//Sales Id label
            grdCharges.Visible = true;
            dvDeliveryTracking.Visible = false;
            OverviewSL(0, ""); TextBox_Search.Text = "";
            confirmation_section.Attributes.Add("style", "display:initial");
            Button_ListOutStandingBlocked.Attributes.Add("style", "background-color:#f58345");
            Button_ListAll.Attributes.Add("style", "background-color:transparent");
            Button_ListOutStanding.Attributes.Add("style", "background-color:transparent");
            Button_DeliverTrack.Attributes.Add("style", "background-color:#transparent");
            confirmation_section_general.Visible = true; Button_Overview_accordion.Text = "List Outstanding Blocked";
        }

        protected void Button_SalesId_Click(object sender, EventArgs e)
        {
            string selected_SO_Id = "";
            Button Button_SalesId = sender as Button;
            if (Button_SalesId != null)
            {
                selected_SO_Id = Button_SalesId.Text;

                Axapta DynAx = new Axapta();
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                try
                {

                    string validateSubmitted = SFA_GET_SALES_ORDER_2.get_validateSubmitted(DynAx, selected_SO_Id);

                    if (validateSubmitted == "1")//already submitted
                    {
                        if ((GLOBAL.user_authority_lvl != 1) && (GLOBAL.user_authority_lvl != 2))//not superadmin or admin
                        {
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
                            //if (temp_flag_HOD == 0)//to let salesman see sales order details
                            //{
                            //    Function_Method.MsgBox("This SO has been submitted and confirmed. Please contact admin for details.", this.Page, this);
                            //    return;
                            //}
                        }
                    }

                    Session["data_passing"] = "@SFSF_" + selected_SO_Id + "|" + Button_SalesId.CommandArgument;//SFA > SFA
                    Response.Redirect("SFA.aspx");
                }
                catch (Exception ER_SF_12)
                {
                    Function_Method.MsgBox("ER_SF_12: " + ER_SF_12.ToString(), this.Page, this);
                }
            }
        }

        protected void Button_Search_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            // 2025-03-13 KX switch case from text to number
            switch (DropDownList_Search.SelectedItem.Value)
            {
                case "0":
                    fieldName = "CUSTNAME";//SALESNAME
                    break;
                case "1":
                    fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                    break;
                // 2025-03-13 KX Add one more filter option
                case "2":
                    fieldName = "SALESID";
                    break;
                default:
                    fieldName = "";
                    break;
            }
            if (gvDeliveryTracking.Visible == true)
            {
                deliveryTrackingOverview(0, fieldName);
            }
            else
            {
                OverviewSL(0, fieldName);
            }
        }

        private void OverviewSL(int PAGE_INDEX, string fieldName)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();
            bool LFI_AccountNum3_Group = false;

            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                int SalesTable = 366;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesTable);

                switch (Convert.ToInt32(Session["flag_temp"]))
                {
                    case 0:// List Outstanding
                        var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 33);//SalesStatus
                        qbr.Call("value", "1");
                        var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 33);
                        qbr1.Call("value", "2");
                        break;//1=Open Order; 2=Delivered; 3=Invoiced; 4:Cancelled
                    case 1:// List Outstanding Blocked SO
                        var qbr_ = (AxaptaObject)axQueryDataSource.Call("addRange", 33);
                        qbr_.Call("value", "1");
                        var qbr_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 33);
                        qbr_1.Call("value", "2");
                        var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30006);//PRS BLOCKED
                        qbr2.Call("value", "1");
                        break;//1=Open Order; 2=Delivered; 3=Invoiced; 4:Cancelled
                    default:
                        //List All
                        break;
                }
                string temp_SearchValue = "*" + TextBox_Search.Text.Trim() + "*";

                if (fieldName != "" && temp_SearchValue != "")
                {
                    if (fieldName == "CUSTNAME")//need to change to customer account
                    {
                        var qbr3_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 2);//sales name
                        qbr3_1.Call("value", "*" + temp_SearchValue + "*");

                    }
                    if (fieldName == "CUSTACCOUNT")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                    if (fieldName == "SALESID")
                    {
                        var qbr3_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);
                        qbr3_3.Call("value", "*" + temp_SearchValue + "*");
                    }
                }
                axQueryDataSource.Call("addSortField", 1, 1);//SalesId, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Sales Id"; N[2] = "Customer"; N[3] = "Customer Acc.";
                N[4] = "Dl. Date"; N[5] = "Payment"; N[6] = "Status";
                N[7] = "LFI Status"; N[8] = "Submitted"; N[9] = "Salesman";
                N[10] = "Reference"; N[11] = "AutoPost";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesTable);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string temp_SalesId = DynRec.get_Field("SalesId").ToString();
                        row["Sales Id"] = temp_SalesId;
                        row["Customer"] = DynRec.get_Field("SalesName").ToString();

                        string custAcc = DynRec.get_Field("CUSTACCOUNT").ToString();
                        row["Customer Acc."] = custAcc;

                        string temp_DeliveryDate = DynRec.get_Field("DeliveryDate").ToString();
                        string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
                        string Raw_DeliveryDate = arr_temp_DeliveryDate[0];
                        row["Dl. Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DeliveryDate, true);

                        string temp_PaymTermId = DynRec.get_Field("Payment").ToString();
                        string PaymTermName = SFA_GET_SALES_HEADER.get_PaymTermName(DynAx, temp_PaymTermId);
                        row["Payment"] = temp_PaymTermId + " (" + PaymTermName + ")";

                        string temp_SalesStatus = DynRec.get_Field("SalesStatus").ToString();
                        row["Status"] = SFA_GET_SALES_ORDER_2.get_AxEnumSalesStatus(temp_SalesStatus);

                        string temp_LFI_SalesStatus = SFA_GET_SALES_ORDER_2.get_LFI_SalesStatus(DynAx, temp_SalesId, temp_SalesStatus);
                        row["LFI Status"] = SFA_GET_SALES_ORDER_2.get_AxEnumSalesStatus(temp_LFI_SalesStatus);

                        string temp_LFI_Submitted = DynRec.get_Field("LFI_Submitted").ToString();
                        if (temp_LFI_Submitted == "1")
                        {
                            row["Submitted"] = "Yes";
                        }
                        else
                        {
                            if (temp_LFI_Submitted == "0")
                            {
                                row["Submitted"] = "No";
                            }
                            else
                            {
                                row["Submitted"] = temp_LFI_Submitted;
                            }
                        }

                        row["Salesman"] = DynRec.get_Field("SalesResponsible").ToString();
                        row["Reference"] = DynRec.get_Field("CustomerRef").ToString();
                        row["AutoPost"] = DynRec.get_Field("LF_AutoPost").ToString() == "1" ? "Yes" : "No";

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridViewOverviewList.VirtualItemCount = countA;
                //Data-Binding with our GRID

                GridViewOverviewList.DataSource = dt;
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_SF_11)
            {
                Function_Method.MsgBox("ER_SF_11: " + ER_SF_11.ToString(), this.Page, this);
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (TextBox_Search.Text == "")
                {
                    OverviewSL(e.NewPageIndex, "");
                }
                else
                {
                    string fieldName = "";
                    switch (DropDownList_Search.SelectedItem.Text)
                    {
                        case "Sales Order No.":
                            fieldName = "SALESID";//SALESID
                            break;
                        case "Customer Account No.":
                            fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                            break;

                        default:
                            fieldName = "";
                            break;
                    }

                    OverviewSL(e.NewPageIndex, fieldName);
                }

                GridViewOverviewList.PageIndex = e.NewPageIndex;
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_SF_25)
            {
                Function_Method.MsgBox("ER_SF_25: " + ER_SF_25.ToString(), this.Page, this);
            }
        }

        protected void TextBox_SLQuantity_TextChanged(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");
            string inventDimId = domComSalesLine.Call("getInventDimId", TextBox_Id.Text, hidden_configIdStr.Text, hidden_colorStr.Text, hidden_sizeStr.Text, Label_Warehouse.Text).ToString();

            int qtyInsufficient = Convert.ToInt32(domComSalesLine.Call("getQtyAvail", TextBox_Id.Text.Trim(), inventDimId, DropDownList_Unit.Text.Substring(0, 2), TextBox_SLQuantity.Text));
            //prompt message if stock less than salesman keyin
            hdnUserResponse.Value = qtyInsufficient.ToString();
            // Set the value of qtyInsufficient to the hidden field
            if (qtyInsufficient < Convert.ToInt32(TextBox_SLQuantity.Text))//prompt message if stock less than salesman keyin
            {
                Function_Method.MsgBox("Insufficient stock!", this.Page, this);
            }
        }

        protected void TextBox_Account_TextChanged(object sender, EventArgs e)
        {
            //Axapta DynAx = Function_Method.GlobalAxapta();

            //AxaptaObject domComSalesTable = DynAx.CreateAxaptaObject("DOMCOMSalesTable");
            //string isSubmitted = domComSalesTable.Call("checkEarlySO", TextBox_Account.Text.Trim()).ToString();
            //if (isSubmitted != "")
            //{
            //    string message = "Previous SO: " + isSubmitted + " already submitted < 36 hours! Do you want to continue?";
            //    string script = "<script type='text/javascript'>showModal('" + message + "');</script>";
            //    ClientScript.RegisterStartupScript(this.GetType(), "ShowModalScript", script);
            //}
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            // Handle the "Yes" button click here
            Button_SaveHeader_Click(Button_SaveHeader, EventArgs.Empty);
        }

        private decimal total = 0; // Initialize total outside the loop
        private int totalQty = 0;
        protected void GridView_Sales_Order_Line_View_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Calculate the total
                decimal netAmount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Net Amount"));
                int quantity = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Quantity"));

                total += netAmount; // Accumulate the total for each row
                totalQty += quantity;
            }

            Label_TotalQty.Text = $"Total Quantity: {totalQty}";
            Label_TotalAmount.Text = $"Total Amount: RM{total.ToString("0.00")}";
        }

        protected void Button_DeliveryCheck_Click(object sender, ImageClickEventArgs e)
        {
            dvSalesOrderLine.Attributes.Add("style", "display:none");
            dvGatePass.Attributes.Add("style", "display:initial");
            Button_DeliveryCheck.Visible = true;
            displayDeliveryOverview(0, hidden_Label_SO_No.Text);
        }

        protected void displayDeliveryOverview(int PAGE_INDEX, string SalesID)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int LF_GATEPASS = 40313;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_GATEPASS);

                var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 40004);//SALESID
                qbr3_2.Call("value", SalesID);

                axQueryDataSource.Call("addSortField", 40004, 1);//salesID, desc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Sales ID"; N[2] = "Date"; N[3] = "Invoice ID"; N[4] = "Invoice Date"; N[5] = "Voucher"; N[6] = "Currency";
                N[7] = "Invoice Amount"; N[8] = "Gate Pass"; N[9] = "Gate Pass Date"; N[10] = "Vehicle No."; N[11] = "Transporter Name";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                //===========================================
                // Loop through the set of retrieved records.
                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_GATEPASS);

                    countA = countA + 1;
                    if (countA > 0)
                    {
                        row = dt.NewRow();
                        row["No."] = countA;
                        row["Sales ID"] = DynRec.get_Field("SalesId").ToString();
                        row["Date"] = Convert.ToDateTime(DynRec.get_Field("POD_Date")).ToString("dd/MM/yyyy");
                        string invoiceid = DynRec.get_Field("InvoiceId").ToString();
                        row["Invoice ID"] = invoiceid;
                        row["Invoice Date"] = Convert.ToDateTime(DynRec.get_Field("InvoiceDate")).ToString("dd/MM/yyyy");
                        row["Voucher"] = DynRec.get_Field("LF_InventDimID").ToString();
                        row["Currency"] = "MYR";
                        var getInvoiceAmount = SFA_GET_SALES_ORDER_2.get_CustInvoiceJour_details(DynAx, invoiceid);
                        row["Invoice Amount"] = getInvoiceAmount.Item7;
                        row["Gate Pass"] = DynRec.get_Field("GatePass").ToString();
                        row["Gate Pass Date"] = Convert.ToDateTime(DynRec.get_Field("GatePassDate")).ToString("dd/MM/yyyy");
                        row["Vehicle No."] = DynRec.get_Field("VehicleNo").ToString();
                        row["Transporter Name"] = DynRec.get_Field("TransporterName").ToString();

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                }

                //Data-Binding with our GRID

                gvGatePass.DataSource = dt;
                gvGatePass.DataBind();
            }
            catch (Exception ER_GP_00)
            {
                Function_Method.MsgBox("ER_GP_00: " + ER_GP_00.ToString(), this.Page, this);
            }
        }

        protected void deliveryTrackingOverview(int PAGE_INDEX, string fieldName)
        {
            gvDeliveryTracking.DataSource = null;
            gvDeliveryTracking.DataBind();

            Axapta DynAx = Function_Method.GlobalAxapta();
            // 2025-03-13 KX 14 days record change to get 3 months record
            DateTime ThreeMonthsRecord = DateTime.Now.AddMonths(-3);
            DateTime dateTime = DateTime.Now;
            try
            {
                string TableName = "LF_Gatepass";

                int tableId = DynAx.GetTableId(TableName);
                int blockInvChk = DynAx.GetFieldId(tableId, "BlockInvCheck");
                int invoiceDt = DynAx.GetFieldId(tableId, "InvoiceDate");
                int GatePass = DynAx.GetFieldId(tableId, "GatePass");
                int SalesId = DynAx.GetFieldId(tableId, "SalesId");

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                var qbr3_3 = (AxaptaObject)axQueryDataSource.Call("addRange", blockInvChk);//blockInvCheck
                qbr3_3.Call("value", "No");

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", invoiceDt);//invoiceDate
                qbr.Call("value", ThreeMonthsRecord + ".." + dateTime);

                string temp_SearchValue = "*" + TextBox_Search.Text.Trim() + "*";

                if (fieldName == "CUSTACCOUNT" && temp_SearchValue != "")
                {
                    var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 40001);//AccountNum
                    qbr3_2.Call("value", temp_SearchValue);
                }
                // 2025-03-13 KX Add one more filter option
                else if (fieldName == "SALESID" && temp_SearchValue != "")
                {
                    var qbr3_4 = (AxaptaObject)axQueryDataSource.Call("addRange", SalesId);//Sales ID
                    qbr3_4.Call("value", temp_SearchValue);
                }

                axQueryDataSource.Call("addSortField", 40004, 1);//salesID, desc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Sales ID"; N[2] = "Salesman ID"; N[3] = "Customer"; N[4] = "Customer Acc."; N[5] = "Invoice ID"; N[6] = "Picking ID";
                N[7] = "Gate Pass"; N[8] = "Gate Pass Date"; N[9] = "Transporter Name"; N[10] = "Vehicle No."; N[11] = "Reversed Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.
                string gatePassDt = ""; string reverseDt = "";

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();
                        string invoicedt = DynRec.get_Field("InvoiceDate").ToString();

                        row["No."] = countA;
                        row["Sales ID"] = DynRec.get_Field("SalesId").ToString();
                        string CustAccount = DynRec.get_Field("AccountNum").ToString();
                        var getSalesmanID = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAccount);
                        row["Salesman ID"] = getSalesmanID.Item3;

                        row["Customer"] = getSalesmanID.Item1;
                        row["Customer Acc."] = CustAccount;
                        row["Invoice ID"] = DynRec.get_Field("InvoiceId").ToString();
                        row["Picking ID"] = DynRec.get_Field("PickID").ToString();
                        gatePassDt = DynRec.get_Field("GatePassDate").ToString();
                        reverseDt = DynRec.get_Field("ReversedDate").ToString();
                        row["Gate Pass"] = DynRec.get_Field("GatePass").ToString();
                        if (gatePassDt.Contains("1900"))
                        {
                            row["Gate Pass Date"] = "";
                        }
                        else
                        {
                            row["Gate Pass Date"] = Convert.ToDateTime(gatePassDt).ToString("dd/MM/yyyy");
                        }
                        row["Transporter Name"] = DynRec.get_Field("TransporterName").ToString();
                        row["Vehicle No."] = DynRec.get_Field("VehicleNo").ToString();
                        if (reverseDt.Contains("1900"))
                        {
                            row["Reversed Date"] = "";
                        }
                        else
                        {
                            row["Reversed Date"] = Convert.ToDateTime(reverseDt).ToString("dd/MM/yyyy");
                        }

                        if (fieldName == "CUSTNAME" && temp_SearchValue != "")
                        {
                            if (getSalesmanID.Item1.ToLower().Contains(TextBox_Search.Text))
                            {
                                dt.Rows.Add(row);
                            }
                        }
                        else
                        {
                            dt.Rows.Add(row);
                        }

                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                }
                gvDeliveryTracking.VirtualItemCount = countA;

                //Data-Binding with our GRID
                gvDeliveryTracking.DataSource = dt;
                gvDeliveryTracking.DataBind();
            }
            catch (Exception ER_DT_00)
            {
                Function_Method.MsgBox("ER_DT_00: " + ER_DT_00.ToString(), this.Page, this);
            }
        }

        protected void Button_DeliverTrack_Click(object sender, EventArgs e)
        {
            Button_Overview_accordion.Text = "Delivery Tracking";
            grdCharges.Visible = false;
            Button_Overview_accordion.Visible = false;
            dvDeliveryTracking.Visible = true;
            Button_DeliverTrack.Attributes.Add("style", "background-color:#f58345");
            Button_ListOutStandingBlocked.Attributes.Add("style", "background-color:transparent");
            Button_ListAll.Attributes.Add("style", "background-color:transparent");
            Button_ListOutStanding.Attributes.Add("style", "background-color:transparent");

            deliveryTrackingOverview(0, "");
        }

        protected void gvDeliveryTracking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            deliveryTrackingOverview(e.NewPageIndex, "");

            gvDeliveryTracking.PageIndex = e.NewPageIndex;
            gvDeliveryTracking.DataBind();
        }

        protected void Button_UnfulfillOrder_Click(object sender, EventArgs e)
        {
            Session["flag_temp"] = 2;//Cust acc group 3
            Button_DeliverTrack.Attributes.Add("style", "background-color:transparent");
            Button_ListOutStandingBlocked.Attributes.Add("style", "background-color:transparent");
            Button_ListAll.Attributes.Add("style", "background-color:transparent");
            Button_ListOutStanding.Attributes.Add("style", "background-color:transparent");
            Button_UnfulfillOrder.Attributes.Add("style", "background-color:#f58345");
            OverviewUnfulfill(0, "");
            DivUnfulfillOrder.Visible = true;
            dvDeliveryTracking.Visible = false;
            GridViewOverviewList.Visible = false;
        }

        private void OverviewUnfulfill(int PAGE_INDEX, string fieldName)
        {
            gvUnfulfillOrder.DataSource = null;
            gvUnfulfillOrder.DataBind();

            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int SalesTable = 366;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesTable);

                var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", 33);//SalesStatus !invoice
                qbr3.Call("value", "!3");

                string temp_SearchValue = "*" + TextBox_Search.Text.Trim() + "*";

                if (fieldName != "" && temp_SearchValue != "")
                {
                    if (fieldName == "CUSTNAME")//need to change to customer account
                    {
                        var qbr3_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 2);//sales name
                        qbr3_1.Call("value", "*" + temp_SearchValue + "*");

                    }
                    if (fieldName == "CUSTACCOUNT")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                }
                axQueryDataSource.Call("addSortField", 1, 1);//SalesId, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 14;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Sales Id"; N[2] = "Customer"; N[3] = "Customer Acc.";
                N[4] = "Dl. Date"; N[5] = "Item Name"; N[6] = "Item ID"; N[7] = "Unit"; N[8] = "Quantity"; N[9] = "Dlv Quantity";
                N[10] = "Dlv Remainder"; N[11] = "Status"; N[12] = "LFI Status"; N[13] = "Salesman";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesTable);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string temp_SalesId = DynRec.get_Field("SalesId").ToString();
                        row["Sales Id"] = temp_SalesId;
                        row["Customer"] = DynRec.get_Field("SalesName").ToString();

                        string custAcc = DynRec.get_Field("CUSTACCOUNT").ToString();
                        row["Customer Acc."] = custAcc;

                        string temp_DeliveryDate = DynRec.get_Field("DeliveryDate").ToString();
                        string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
                        string Raw_DeliveryDate = arr_temp_DeliveryDate[0];
                        row["Dl. Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DeliveryDate, true);

                        var getItemDetails = SFA_GET_SALES_ORDER.get_SaleslineDet(DynAx, temp_SalesId);
                        row["Item Name"] = getItemDetails.Item1;
                        row["Item ID"] = getItemDetails.Item2;
                        row["Unit"] = getItemDetails.Item3;
                        row["Quantity"] = getItemDetails.Item4;
                        row["Dlv Quantity"] = getItemDetails.Item5;
                        row["Dlv Remainder"] = getItemDetails.Item6;
                        string temp_SalesStatus = DynRec.get_Field("SalesStatus").ToString();

                        string temp_LFI_SalesStatus = SFA_GET_SALES_ORDER_2.get_LFI_SalesStatus(DynAx, temp_SalesId, temp_SalesStatus);
                        row["LFI Status"] = SFA_GET_SALES_ORDER_2.get_AxEnumSalesStatus(temp_LFI_SalesStatus);

                        row["Salesman"] = DynRec.get_Field("SalesResponsible").ToString();

                        dt.Rows.Add(row);

                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                gvUnfulfillOrder.VirtualItemCount = countA;
                //Data-Binding with our GRID

                gvUnfulfillOrder.DataSource = dt;
                gvUnfulfillOrder.DataBind();
            }
            catch (Exception ER_SF_12)
            {
                Function_Method.MsgBox("ER_SF_12: " + ER_SF_12.ToString(), this.Page, this);
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected void btnHiddenPostback_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            string dataPassing = hiddenDataPassing.Value; // e.g. "@SFIM_SOId|ItemId"

            if (!string.IsNullOrEmpty(dataPassing) && dataPassing.StartsWith("@SFIM_"))
            {
                string SO_Id_selected_ItemId = dataPassing.Substring(6);
                string[] parts = SO_Id_selected_ItemId.Split('|');
                if (parts.Length == 2)
                {
                    string SO_Id = parts[0];
                    string selected_ItemId = parts[1];



                    reloading_data(DynAx, SO_Id);
                    // Call your method here if needed
                    //CheckIsEditable(SO_Id);

                    // Set the TextBox text
                    TextBox_Id.Text = selected_ItemId;

                    validate_SL();

                    Sales_order();

                    // Register client script to trigger Button_TextBox_Id click
                    string script = $"document.getElementById('{Button_TextBox_Id.ClientID}').click();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "TriggerValidateButton", script, true);

                    // Clear hidden field
                    hiddenDataPassing.Value = "";
                }

            }
        }

    }
}
