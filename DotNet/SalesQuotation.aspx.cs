using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class SalesQuotation : System.Web.UI.Page
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
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                string temp1 = GLOBAL.data_passing.ToString();
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        //string test = temp1.Substring(0, 6);

                        if (temp1.Substring(0, 6) == "@SFCM_")//Data receive from CustomerMaster> SFA
                        {
                            string temp3 = temp1.Substring(6);
                            TextBox_Account.Text = temp1.Substring(6);
                            //validate(); Alt_Addr_function();
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_header_section'); ", true);//go to Overall_sales_order_section
                        }
                        else if (temp1.Substring(0, 6) == "@SQIM_")//Data receive from InventoryMaster> SFA
                        {
                            string SQ_Id_selected_ItemId = temp1.Substring(6);
                            string[] arr_SQ_Id_selected_ItemId = SQ_Id_selected_ItemId.Split('|');
                            string SQ_Id = arr_SQ_Id_selected_ItemId[0];
                            string selected_ItemId = arr_SQ_Id_selected_ItemId[1];
                            /*start of reloading data*/
                            reloading_data(DynAx, SQ_Id);
                            /*end of reloading data*/
                            TextBox_Id.Text = selected_ItemId;
                            validate_SL();
                            Sales_order();
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_quotation_section'); ", true);//go to Overall_sales_order_section                                                       
                        }
                        else if (temp1.Substring(0, 4) == "@SQ_")//Data request from SFA> SQ
                        {
                            string SQ_Id = temp1.Substring(4);
                            /*start of reloading data*/
                            reloading_data(DynAx, SQ_Id);
                            /*end of reloading data*/

                            sales_order_section_general.Visible = true; Sales_order_section.Visible = false; FV_order_section.Visible = false;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_quotation_section'); ", true);//go to Overall_sales_order_section
                        }
                        else if (temp1.Substring(0, 6) == "@SQCM_")//Data receive from CustomerMaster> SQuotation
                        {
                            string temp3 = temp1.Substring(6);
                            TextBox_Account.Text = temp1.Substring(6);
                            lblQuotationStat.Text = "Created";
                            validate();
                            Alt_Addr_function();
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_header_section'); ", true);//go to Overall_sales_order_section
                        }
                        else if (temp1.Substring(0, 6) == "@SQS2_")//refresh after submit/ post
                        {
                            string SQ_Id = temp1.Substring(6);
                            Function_Method.MsgBox("SQ number: " + SQ_Id + " successfully submitted.", this.Page, this);
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_confirmation_section'); ", true);//go to overview section
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
            catch (Exception ER_SQ_07)
            {
                Function_Method.MsgBox("ER_SQ_07: " + ER_SQ_07.ToString(), this.Page, this);
            }
        }

        private void reloading_data(Axapta DynAx, string SQ_Id)
        {
            //validate();//auto fill up [TextBox_Account, Label_CustName,Label_Address] in Customer accordion

            hidden_Label_SQ_No.Text = SQ_Id;
            //Some variable data need to derived from SalesQuotationTable manually
            var tuple_reload = Quotation_Get_Sales_Quotation.reload_from_SalesQuotationTable(DynAx, SQ_Id);

            //Customer accordion
            //TextBox_References.Text = tuple_reload.Item1;
            //Delivery accordion
            TextBox_Account.Text = tuple_reload.Item2;
            string temp_DeliveryDate = tuple_reload.Item3;
            string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
            string Raw_DeliveryDate = arr_temp_DeliveryDate[0];
            txtDlvDate.Text = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DeliveryDate, true);

            //Label_Receiver_Name.Text = tuple_reload.Item3;

            string temp_Delivery_Addr = tuple_reload.Item4;
            if (temp_Delivery_Addr == Label_Address.Text)
            {
                Label_Delivery_Addr.Text = "-same as primary address-";
            }
            else
            {
                Label_Delivery_Addr.Text = temp_Delivery_Addr;
            }
            quotation_line_view();

        }
        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void sales_header_section_Click(object sender, EventArgs e)
        {
            sales_header_section.Attributes.Add("style", "display:initial"); Button_sales_header_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_quotation_section.Attributes.Add("style", "background-color:transparent");
            confirmation_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            quotation_line_view();//run grid sales line view
        }

        protected void quotation_section_Click(object sender, EventArgs e)
        {
            //if (hidden_Label_SQ_No.Text == "")
            //{
            //    /*Button_UpdateHeader.Visible = false; Button_SaveHeader.Visible = true; TextBox_Account.Enabled = true; Button_CustomerMasterList.Enabled = true; Button_CheckAcc.Enabled = true;*/
            //    Function_Method.MsgBox("No quotation number.", this.Page, this);
            //    return;
            //}
            //else
            //{
            //    /*Button_UpdateHeader.Visible = true; Button_SaveHeader.Visible = false; TextBox_Account.Enabled = false; Button_CustomerMasterList.Enabled = false; Button_CheckAcc.Enabled = false;*/
            Label_SQ_No.Text = "SQ number : " + hidden_Label_SQ_No.Text;
            //}

            /*
            //lock the textbox and button from editting in Sales Header
            TextBox_Account.ReadOnly = true; TextBox_Account.Attributes.Add("style", "background-color:#ffdf87");
            TextBox_References.ReadOnly = true; TextBox_References.Attributes.Add("style", "background-color:#ffdf87");

            Button_CustomerMasterList.Enabled = false;
            TextBox_Notes.ReadOnly = true; TextBox_Notes.Attributes.Add("style", "background-color:#ffdf87");
            Button_Delivery_Addr.Enabled = false;
            */
            sales_header_section.Attributes.Add("style", "display:none"); Button_sales_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:initial"); Button_quotation_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            confirmation_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
        }

        protected void confirmation_section_Click(object sender, EventArgs e)
        {
            sales_header_section.Attributes.Add("style", "display:none"); Button_sales_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_quotation_section.Attributes.Add("style", "background-color:transparent");
            confirmation_section.Attributes.Add("style", "display:initial"); Button_confirmation_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            //first, go to ListOutStanding
            quotation_line_view();
            f_Button_ListOutStanding();
        }

        protected void enquiries_section_Click(object sender, EventArgs e)
        {
            sales_header_section.Attributes.Add("style", "display:none"); Button_sales_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_quotation_section.Attributes.Add("style", "background-color:transparent");
            confirmation_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:initial"); Button_enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            //first, go to Enquiries
            f_Button_Enquiries_Click();
        }

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        private void clear_sales_header()
        {
            //Customer
            //TextBox_Account.Text = "";
            Label_CustName.Text = "";//Name
            Label_Address.Text = "";//Address
            //TextBox_References.Text = "";

            //Delivery
            txtDlvDate.Text = ""; Label_Receiver_Name.Text = ""; Label_Delivery_Addr.Text = "";

            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";
            hidden_Street.Text = ""; hidden_ZipCode.Text = ""; hidden_City.Text = "";
            hidden_State.Text = ""; hidden_Country.Text = "";

            GridView_AltAddress.DataSource = null; GridView_AltAddress.DataBind(); GridView_AltAddress.Visible = false;

            //Sales Order
            //Label_Payment_Terms.Text = "";
            //Label_Suffix_Code.Text = "";
            //TextBox_Notes.Text = "";
            hidden_Label_SQ_No.Text = "";

            //Button_Delivery_Addr.Visible = false;
            checkun.Visible = false;
        }

        private void clear_sales_line()
        {
            Label_SQ_No.Text = "";
            GridView_FVOrder.DataSource = null; GridView_FVOrder.DataBind(); //GridView_FVOrder.Visible = false;

            checkun2.Visible = false;
            TextBox_Id.Text = "";
            Label_IdName.Text = "";
            Label_Warehouse.Text = "";
            DropDownList_Unit.Items.Clear();
            TextBox_SLQuantity.Text = "";
            Label_Price.Text = "";

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
        protected void Button_Submit_SalesQuotationLine_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string SQ_number = hidden_Label_SQ_No.Text;
            if (SQ_number == "")
            {
                Function_Method.MsgBox("There is no SQ number", this.Page, this);
                return;
            }
            try
            {
                if (Post_Confirm_SQ() == true)
                {
                    string SO_no = hidden_Label_SQ_No.Text;
                    Session["data_passing"] = "@SQS2_" + SO_no;//reload
                    Response.Redirect("SalesQuotation.aspx", false);
                }
            }
            catch (Exception ER_SQ_10)
            {
                Function_Method.MsgBox("ER_SQ_10: " + ER_SQ_10.ToString(), this.Page, this);
            }
        }

        private bool Post_Confirm_SQ()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string quotationid = hidden_Label_SQ_No.Text;
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesQuotationTable"))
                {
                    DynAx.TTSBegin();
                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "QuotationId", quotationid));
                    if (DynRec.Found)
                    {
                        //DynRec.set_Field("CUSTOMERREF", TextBox_References.Text);
                        //DynRec.set_Field("NOTES", TextBox_Notes.Text);
                        DynRec.set_Field("DeliveryStreet", hidden_Street.Text);
                        DynRec.set_Field("DeliveryAddress", hidden_Street.Text);
                        DynRec.set_Field("DeliveryZipCode", hidden_ZipCode.Text);
                        DynRec.set_Field("DeliveryCity", hidden_City.Text);
                        DynRec.set_Field("DeliveryState", hidden_State.Text);
                        DynRec.set_Field("DeliveryCountryRegionId", hidden_Country.Text);
                        //
                        DynRec.Call("Update");
                    }
                    else
                    {
                        Function_Method.MsgBox("Fail to submit, Quotation ID: " + quotationid + " not found.", this.Page, this);
                    }

                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }

                updateSalesQuotationStatus(hidden_Label_SQ_No.Text);
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_quotation_section'); ", true);//go to Overall_sales_order_section
            }
            catch (Exception ER_SQ_00)
            {
                Function_Method.MsgBox("ER_SQ_00: " + ER_SQ_00.ToString(), this.Page, this);
                return false;
            }
            return true;
        }

        protected void Button_Delete_SalesOrderLine_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string error = "";
                foreach (GridViewRow row in GridView_Sales_Quotation_Line_View.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox CheckBox_selection = (row.Cells[0].FindControl("CheckBox_ToDeleteByOne") as CheckBox);
                        if (CheckBox_selection.Checked)
                        {
                            GridView_Sales_Quotation_Line_View.Columns[12].Visible = true;
                            GridView_Sales_Quotation_Line_View.Columns[13].Visible = true;
                            string SalesLine_RecId = row.Cells[12].Text;
                            string allow_alter = row.Cells[13].Text;
                            if (allow_alter == "1")//can be updated
                            {
                                error += Quotation_Get_Sales_Quotation.get_delete_saleline(DynAx, SalesLine_RecId);
                            }
                            else
                            {
                                error += "Cannot delete quotation " + SalesLine_RecId + " that has been created.";
                            }
                        }
                    }
                }
                if (error != "")
                {
                    Function_Method.MsgBox(error, this.Page, this);
                }
                quotation_line_view();
            }
            catch (Exception ER_SQ_23)
            {
                Function_Method.MsgBox("ER_SQ_23: " + ER_SQ_23.ToString(), this.Page, this);
            }
        }

        protected void CheckBox_Changed_ToDeleteAll(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView_Sales_Quotation_Line_View.HeaderRow.FindControl("CheckBox_ToDeleteAll");

            foreach (GridViewRow row in GridView_Sales_Quotation_Line_View.Rows)
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
            int row_count = GridView_Sales_Quotation_Line_View.Rows.Count;
            int count_checked = 0;
            for (int i = 0; i < row_count; i++)
            {
                CheckBox CheckBox_selection = (GridView_Sales_Quotation_Line_View.Rows[i].Cells[0].FindControl("CheckBox_ToDeleteByOne") as CheckBox);
                if (GridView_Sales_Quotation_Line_View.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    if (CheckBox_selection.Checked)//highlight
                    {
                        count_checked = count_checked + 1;
                        GridView_Sales_Quotation_Line_View.Rows[i].BackColor = Color.FromName("#ff8000");
                    }
                }
            }

            CheckBox ChkBoxHeader = (CheckBox)GridView_Sales_Quotation_Line_View.HeaderRow.FindControl("CheckBox_ToDeleteAll");
            if (count_checked == row_count)
            {
                ChkBoxHeader.Checked = true;
            }
            else
            {
                ChkBoxHeader.Checked = false;
            }

            if (count_checked != 0)
            {
                Button_Delete_SalesOrderLine.Visible = true;
            }
            else
            {
                Button_Delete_SalesOrderLine.Visible = false;
            }
        }

        private void quotation_line_view()
        {
            Button_Sales_Order_Line.Visible = true; 
            Button_Submit_SalesQuotationLine.Visible = true; 
            Button_Delete_SalesOrderLine.Visible = false;

            GridView_Sales_Quotation_Line_View.DataSource = null; 
            GridView_Sales_Quotation_Line_View.DataBind();

            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int SalesQuotationLine = 1962;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesQuotationLine);

                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 2);//QuotationId
                qbr1.Call("value", hidden_Label_SQ_No.Text);
                //axQueryDataSource.Call("addSortField", 1, 0);//Line Number, asc

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 13;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Item Id";
                N[2] = "Item Name"; N[3] = "Unit"; N[4] = "Quantity";

                N[5] = "Price Each"; N[6] = "Net Amount"; N[7] = "Delivery Date";
                N[8] = "Invoice Quantity"; N[9] = "Delivery Quantity"; N[10] = "Delivery Remainder";
                N[11] = "Hidden_RecId"; N[12] = "Hidden_allow_alter";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================

                DataRow row;
                int countA = 0;
                //for checking sales status, so that the saleline can be delete or update
                string temp_QuotationStatus = Quotation_Get_Sales_Quotation.get_QuotationStatus(DynAx, hidden_Label_SQ_No.Text);
                //
                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", SalesQuotationLine);
                    countA = countA + 1;

                    row = dt.NewRow();
                    //transfer to grid
                    row["No."] = countA.ToString();
                    row["Item Id"] = DynRec.get_Field("ItemId").ToString();
                    row["Item Name"] = DynRec.get_Field("Name").ToString();
                    row["Unit"] = DynRec.get_Field("SalesUnit").ToString();
                    row["Quantity"] = DynRec.get_Field("SalesQty").ToString();
                    row["Price Each"] = DynRec.get_Field("SalesPrice").ToString();
                    row["Net Amount"] = DynRec.get_Field("LineAmount").ToString();

                    string temp_InvDate = DynRec.get_Field("ConfirmedDlv").ToString();
                    string InvDate = "";
                    if (temp_InvDate != "")
                    {
                        string[] arr_temp_InvDate = temp_InvDate.Split(' ');//date + " " + time;
                        string Raw_InvDate = arr_temp_InvDate[0];
                        InvDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_InvDate, true);
                    }
                    row["Delivery Date"] = InvDate;
                    //
                    string temp_INVENTTRANSID = DynRec.get_Field("InventTransId").ToString();

                    row["Hidden_RecId"] = DynRec.get_Field("RecId").ToString();

                    if (temp_QuotationStatus == "0")//created
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
                }
                if (countA == 0)//grid empty
                {
                    //close grid
                    Button_Sales_Order_Line.Visible = false; Button_Submit_SalesQuotationLine.Visible = false;
                    return;
                }
                GridView_Sales_Quotation_Line_View.DataSource = dt;
                GridView_Sales_Quotation_Line_View.DataBind();
            }
            catch (Exception ER_SQ_06)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_SQ_06: " + ER_SQ_06.ToString(), this.Page, this);
            }
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
            OverviewQL(0, ""); TextBox_Search.Text = "";
            confirmation_section_general.Visible = true; Button_Overview_accordion.Text = "List Outstanding";
        }

        protected void Button_QuotationID_Click(object sender, EventArgs e)
        {
            string selected_Quotation_Id = "";
            Button Button_QuotationId = sender as Button;
            if (Button_QuotationId != null)
            {
                selected_Quotation_Id = Button_QuotationId.Text;

                Axapta DynAx = Function_Method.GlobalAxapta();

                try
                {
                    //string validateSubmitted = Quotation_Get_Sales_Quotation.get_validateSubmitted(DynAx, selected_Quotation_Id);

                    //====================================================================

                    //if (validateSubmitted == "1")//already submitted
                    //{
                    //    if (    (GLOBAL.user_authority_lvl != 1) && (GLOBAL.user_authority_lvl != 2)    )//not superadmin or admin
                    //    {
                    //        var tupple_get_Empl_Id = Quotation_Get_Sales_Quotation.get_Empl_Id(DynAx, GLOBAL.user_id);

                    //        string[] Empl_Id = tupple_get_Empl_Id.Item1;
                    //        int counter = tupple_get_Empl_Id.Item2;

                    //        int temp_flag_HOD = 0;
                    //        for (int j = 0; j < counter; j++)
                    //        {
                    //            if (Empl_Id[j].Substring(0, 1) == "H")
                    //            {//HOD
                    //                temp_flag_HOD = 1;
                    //            }
                    //        }
                    //        if (temp_flag_HOD == 0)
                    //        {
                    //            Function_Method.MsgBox("This SQ has been submitted and confirmed. Please contact admin for details.", this.Page, this);
                    //            return;
                    //        }
                    //    }
                    //}                   

                    Session["data_passing"] = "@SQ_" + selected_Quotation_Id;//SFA > SFA
                    Response.Redirect("SalesQuotation.aspx", false);
                }
                catch (Exception ER_SQ_12)
                {
                    Function_Method.MsgBox("ER_SQ_12: " + ER_SQ_12.ToString(), this.Page, this);
                }
            }
        }

        protected void Button_Search_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            switch (DropDownList_Search.SelectedItem.Text)
            {
                case "Quotation No.":
                    fieldName = "QUOTATIONID";//SALESID
                    break;
                case "Customer Account No.":
                    fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                    break;
                case "Workshop Name":
                    fieldName = "CUSTNAME";//CUSTACCOUNT
                    break;

                default:
                    fieldName = "";
                    break;
            }
            errMsg.Visible = false;

            OverviewQL(0, fieldName);
        }

        private void OverviewQL(int PAGE_INDEX, string fieldName)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int SalesQuotationTable = 1967;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", SalesQuotationTable);

                string temp_SearchValue = "*" + TextBox_Search.Text.Trim() + "*";

                if (fieldName != "" && temp_SearchValue != "")
                {
                    if (fieldName == "QUOTATIONID")
                    {
                        var qbr3_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);
                        qbr3_1.Call("value", temp_SearchValue);

                    }
                    else if (fieldName == "CUSTACCOUNT")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                    else if (fieldName == "CUSTNAME")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 2);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                }
                axQueryDataSource.Call("addSortField", 1, 1);//QuotationId, ascending
                AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 9;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Quotation Id"; N[2] = "Workshop Name"; N[3] = "Salesman";
                N[4] = "Customer Account"; N[5] = "Delivery Date"; N[6] = "Payment"; N[7] = "Date Created";
                N[8] = "Status";
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

                while ((bool)axQueryRun1.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun1.Call("Get", SalesQuotationTable);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        row["Quotation ID"] = DynRec.get_Field("QuotationId").ToString();
                        row["Workshop Name"] = DynRec.get_Field("QuotationName").ToString();
                        string temp_SalesmanID = DynRec.get_Field("SalesResponsible").ToString();
                        row["Salesman"] = EOR_GET_NewApplicant.getEmpName(DynAx, temp_SalesmanID).Trim();

                        row["Customer Account"] = DynRec.get_Field("CUSTACCOUNT").ToString();
                        string temp_DeliveryDate = DynRec.get_Field("DeliveryDate").ToString();
                        string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
                        string Raw_DeliveryDate = arr_temp_DeliveryDate[0];
                        row["Delivery Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DeliveryDate, true);

                        string temp_PaymTermId = DynRec.get_Field("Payment").ToString();
                        string PaymTermName = Quotation_Get_Header.get_PaymTermName(DynAx, temp_PaymTermId);
                        row["Payment"] = temp_PaymTermId + " (" + PaymTermName + ")";

                        string temp_DateCreated = DynRec.get_Field("createdDateTime").ToString();
                        string[] arr_temp_CreatedDate = temp_DateCreated.Split(' ');
                        string Raw_DateCreated = arr_temp_CreatedDate[0];
                        row["Date Created"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DateCreated, true);

                        //row["Status"] = DynRec.get_Field("QuotationStatus").ToString();
                        string tempStatus = DynRec.get_Field("QuotationStatus").ToString();
                        if (tempStatus == "0")
                        {
                            row["Status"] = "Created";
                        }
                        else
                        {
                            row["Status"] = "Confirmed";
                        }
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

                if (dt.Rows.Count.ToString() == "0")
                {
                    errMsg.Visible = true;
                    errMsg.Text = "No quotation found.";
                }
            }
            catch (Exception ER_SQ_11)
            {
                Function_Method.MsgBox("ER_SQ_11: " + ER_SQ_11.ToString(), this.Page, this);
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (TextBox_Search.Text == "")
                {
                    OverviewQL(e.NewPageIndex, "");
                }
                else
                {
                    string fieldName = "";
                    switch (DropDownList_Search.SelectedItem.Text)
                    {
                        case "Quotation No.":
                            fieldName = "QUOTATIONID";//QUOTATIONID
                            break;
                        case "Customer Account No.":
                            fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                            break;
                        default:
                            fieldName = "";
                            break;
                    }

                    OverviewQL(e.NewPageIndex, fieldName);
                }

                GridViewOverviewList.PageIndex = e.NewPageIndex;
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_SQ_25)
            {
                Function_Method.MsgBox("ER_SQ_25: " + ER_SQ_25.ToString(), this.Page, this);
            }
        }
        //end of OverviewSL section

        //public static Tuple<string, string> getAllContact(Axapta DynAx)
        //{
        //    string ContactName = ""; string PhoneNo = "";
        //    int CONTACTPERSON = 520;
        //    List<string> listContactName = new List<string>();
        //    List<string> listPhoneNo = new List<string>();

        //    AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
        //    AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CONTACTPERSON);
        //    AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

        //    while ((bool)axQueryRun.Call("next"))//use if only one record
        //    {
        //        AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CONTACTPERSON);

        //        ContactName = DynRec.get_Field("Name").ToString();
        //        PhoneNo = DynRec.get_Field("Phone").ToString();

        //        listContactName.Add(ContactName);
        //        listPhoneNo.Add(PhoneNo);               

        //        DynRec.Dispose();
        //    }
        //    return new Tuple<string, string>
        //        (ContactName, PhoneNo);
        //}

        protected void updateSalesQuotationStatus(string QuotationId)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesQuotationTable"))
                {
                    DynAx.TTSBegin();
                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "QuotationId", QuotationId));
                    if (DynRec.Found)
                    {
                        //DynRec.set_Field("CUSTOMERREF", TextBox_References.Text);
                        //DynRec.set_Field("NOTES", TextBox_Notes.Text);
                        //DynRec.set_Field("ConfirmDate", DateTime.Today.ToString("dd/MM/yyyy"));

                        DynRec.set_Field("QuotationStatus", 2);
                        DynRec.set_Field("ReasonId", "Success");
                        DynRec.set_Field("DeliveryCity", hidden_City.Text);
                        DynRec.set_Field("DeliveryState", hidden_State.Text);
                        DynRec.set_Field("DeliveryCountryRegionId", hidden_Country.Text);
                        //
                        DynRec.Call("Update");

                        //Function_Method.MsgBox("Update successfully, Quotation ID: " + QuotationId + " .", this.Page, this);
                    }
                    else
                    {
                        Function_Method.MsgBox("Fail to update, Quotation ID: " + QuotationId + " .", this.Page, this);
                    }

                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
            }
            catch (Exception ER_SQ_29)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_SQ_29: " + ER_SQ_29.ToString(), this.Page, this);
            }
        }

        protected void chkboxlistTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkboxlistTerms.SelectedValue == "1")
            {

            }
            else if (chkboxlistTerms.SelectedValue == "1" && chkboxlistTerms.SelectedValue == "2")
            {

            }
            else
            {

            }
        }
    }
}