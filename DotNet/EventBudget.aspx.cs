using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Web.DynamicData;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace DotNet
{
    public partial class EventBudget : System.Web.UI.Page
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

                        if (temp1.Substring(0, 6) == "@EBCM_")//Data receive from CustomerMaster> Event Budget
                        {
                            var split = temp1.Substring(6).Split('|');
                            TextBox_Account.Text = split[0];
                            if (!string.IsNullOrEmpty(split[1]))
                            {
                                Label_CustName.Text = split[1];
                            }
                        }
                    }
                    validate(DynAx);
                    btnForm_Click(btnForm, EventArgs.Empty);

                    Session["data_passing"] = "";
                }
                else
                {
                    btnList_Click(btnList, EventArgs.Empty);
                }

                divNewSelection.Visible = true;
                Button_eb_header_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            }
            catch (Exception ER_EB_07)
            {
                Function_Method.MsgBox("ER_EB_07: " + ER_EB_07.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private void reloading_data(Axapta DynAx, string SO_Id)
        {
            TextBox_Account.Text = SFA_GET_SALES_ORDER.get_CustAcc(DynAx, SO_Id);

            hidden_Label_SO_No.Text = SO_Id;
            //Some variable data need to derived from SalesTable manually
            var tuple_reload = SFA_GET_SALES_ORDER.reload_from_SalesTable(DynAx, SO_Id);

            //Customer accordion
            //Delivery accordion
            string temp_DeliveryDate = tuple_reload.Item2;
            string[] arr_temp_DeliveryDate = temp_DeliveryDate.Split(' ');//date + " " + time;
            string Raw_DeliveryDate = arr_temp_DeliveryDate[0];

            var tuple_reload_set2 = SFA_GET_SALES_ORDER.reload_from_SalesTableSet2(DynAx, SO_Id);

        }

        protected void eb_header_section_Click(object sender, EventArgs e)
        {
            eb_header_section.Attributes.Add("style", "display:initial"); Button_eb_header_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_sales_order_section.Attributes.Add("style", "background-color:transparent");
            divNewList_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
        }

        protected void sales_order_section_Click(object sender, EventArgs e)
        {
            if (hidden_Label_SO_No.Text == "")
            {
                Button_SaveHeader.Visible = true;
                TextBox_Account.Enabled = true;
                Button_CustomerMasterList.Enabled = true;
                Button_CheckAcc.Enabled = true;
                Function_Method.MsgBox("There is no Sales Order Number.", this.Page, this);
                return;
            }
            else
            {
                Button_SaveHeader.Visible = false;
                TextBox_Account.Enabled = false;
                Button_CustomerMasterList.Enabled = false;
                Button_CheckAcc.Enabled = false;
                Label_SO_No.Text = "SO number : " + hidden_Label_SO_No.Text;//12 words + hidden
            }

            eb_header_section.Attributes.Add("style", "display:none"); Button_eb_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:initial"); Button_sales_order_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            divNewList_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
        }

        protected void confirmation_section_Click(object sender, EventArgs e)
        {
            eb_header_section.Attributes.Add("style", "display:none"); Button_eb_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_sales_order_section.Attributes.Add("style", "background-color:transparent");
            divNewList_section.Attributes.Add("style", "display:initial"); Button_confirmation_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            //first, go to ListOutStanding
            Button callBtn = new Button();
            Button_ListAll_Click(callBtn, EventArgs.Empty);
        }

        protected void enquiries_section_Click(object sender, EventArgs e)
        {
            eb_header_section.Attributes.Add("style", "display:none"); Button_eb_header_section.Attributes.Add("style", "background-color:transparent");
            Overall_sales_order_section.Attributes.Add("style", "display:none"); Button_sales_order_section.Attributes.Add("style", "background-color:transparent");
            divNewList_section.Attributes.Add("style", "display:none"); Button_confirmation_section.Attributes.Add("style", "background-color:transparent");
            Button_enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
        }

        //start of sales line section
        //start of sales line view
        protected void Button_Submit_SalesOrderLine_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            string SO_number = hidden_Label_SO_No.Text;
            if (SO_number == "")
            {
                Function_Method.MsgBox("There is no SO number", this.Page, this);
                return;
            }
            try
            {
                if (Post_Confirm_SO() == true)
                {
                    string SO_no = hidden_Label_SO_No.Text;
                    Session["data_passing"] = "@SFS2_" + SO_no;//reload
                    Response.Redirect("SFA.aspx");
                }
            }
            catch (Exception ER_SF_10)
            {
                Function_Method.MsgBox("ER_SF_10: " + ER_SF_10.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
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
            finally
            {
                DynAx.Logoff();
            }
        }

        //end of sales line view     

        //end of sales line section

        //start of OverviewSL section

        protected void Button_ListAll_Click(object sender, EventArgs e)
        {
            Session["flag_temp"] = 2;//List All 
            GridEventBudgetList.PageIndex = 0;
            OverviewSL(0); TextBox_Search.Text = "";
            //CheckBox_div_Searchable_ID.Visible = true;
            divNewList_section.Visible = true;
            Button_Overview_accordion.Text = "List All";
        }

        protected void Button_Search_Click(object sender, ImageClickEventArgs e)
        {
            OverviewSL(0);
        }

        private void OverviewSL(int PAGE_INDEX)
        {
            gvNewParticipation.DataSource = null;
            gvNewParticipation.DataBind();

            // Log on to Microsoft Dynamics AX.
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int LF_WebParticipant = DynAx.GetTableId("LF_WebParticipant");

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WebParticipant);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                if (!string.IsNullOrEmpty(ddlEventCode.SelectedValue))
                {
                    var qbr9 = (AxaptaObject)axQueryDataSource.Call("addRange", 30004);//EventNumber
                    qbr9.Call("value", ddlEventCode.SelectedItem.Text);
                }

                DataTable dt = new DataTable();
                int data_count = 7;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Customer Name"; N[2] = "Account No."; N[3] = "Loyalty Point";
                N[4] = "Pax Registered"; N[5] = "Total Rooms"; N[6] = "Petrol/Allowance";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }

                DataRow row;
                int countA = 0;

                //int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                //int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WebParticipant);

                    countA = countA + 1;

                    //if (countA >= startA && countA <= endA)
                    //{
                    row = dt.NewRow();

                    row["No."] = countA;

                    row["Customer Name"] = DynRec.get_Field("CustName").ToString();
                    row["Account No."] = DynRec.get_Field("CustAccount").ToString();
                    row["Loyalty Point"] = DynRec.get_Field("LoyaltyPoint").ToString();
                    row["Pax Registered"] = DynRec.get_Field("NoOfParticipant").ToString();
                    row["Total Rooms"] = DynRec.get_Field("NumOfRoom").ToString();
                    bool isPetroAllowance = Convert.ToBoolean(DynRec.get_Field("PetroAllowance"));
                    row["Petrol/Allowance"] = isPetroAllowance;

                    dt.Rows.Add(row);
                    // Advance to the next row.
                    DynRec.Dispose();
                    //}
                    //if (countA > endA)
                    //{
                    //    goto FINISH;//speed up process
                    //}
                }

                //FINISH:
                gvNewParticipation.VirtualItemCount = countA;

                gvNewParticipation.DataSource = dt;
                gvNewParticipation.DataBind();
            }
            catch (Exception ER_EB_11)
            {
                Function_Method.MsgBox("ER_EB_11: " + ER_EB_11.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            OverviewSL(e.NewPageIndex);

            gvNewParticipation.PageIndex = e.NewPageIndex;
            gvNewParticipation.DataBind();
        }

        protected void validate(Axapta DynAx)
        {
            try
            {
                if (TextBox_Account.Text != "")
                {
                    var getSalesmanDetails = SFA_GET_Enquiries_BatteryOutstanding.getEmplId(DynAx, TextBox_Account.Text);
                    hidden_salesmanId.Text = getSalesmanDetails.Item1;
                    Label_Salesman.Text = getSalesmanDetails.Item2;

                    var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, hidden_salesmanId.Text);
                    var getHOD = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, tuple_GroupId_ReportingTo.Item2);
                    Label_SalesmanHod.Text = getHOD.Item3;

                    string getEmail = SalesReport_Get_Budget.getEmail(DynAx, getSalesmanDetails.Item1);
                    if (string.IsNullOrEmpty(getEmail))
                    {
                        getEmail = Session["user_id"].ToString() + "@posim.com.my";
                    }
                    var getEventCode = EventBudget_GET_NewApplicant.getEvent_EmpID(DynAx, getEmail);
                    List<ListItem> eventCodeList = getEventCode.Item1;
                    foreach (var item in eventCodeList)
                    {
                        ddlEventCode.Items.Add(item);
                    }

                    var participantDetails = EventBudget_GET_NewApplicant.getParticipant(DynAx, hidden_salesmanId.Text);
                    Label_LatestInvoice.Text = participantDetails.Item2;
                    TextBox_GoldPendant.Text = participantDetails.Item3;
                    TextBox_VoucherCheque.Text = participantDetails.Item4;
                    Label_Rp.Text = participantDetails.Item5;
                    TextBox_NoVegetarian.Text = participantDetails.Item6;

                    if (ddlEventCode.SelectedItem != null)
                    {
                        var getEventDetails = EventBudget_GET_NewApplicant.getWebEvent(DynAx, ddlEventCode.SelectedItem.Text);
                        string dt = Convert.ToDateTime(getEventDetails.Item2).ToString("dd/MM/yyyy");

                        Label_EventDate.Text = dt;
                        Label_Location.Text = getEventDetails.Item1.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Account not exist.", this.Page, this);
                return;
            }
        }

        protected void Button_SaveHeader_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebParticipant"))
                {
                    DynAx.TTSBegin();

                    if (TextBox_Account.Text != "")
                    {
                        DynRec.set_Field("EventNumber", ddlEventCode.SelectedItem.Text);
                        //DynRec.set_Field("EventDate", Label_EventDate.Text);

                        DynRec.set_Field("NoOfParticipant", Convert.ToInt16(TextBox_TotalParticipant.Text));
                        DynRec.set_Field("CustAccount", TextBox_Account.Text);
                        DynRec.set_Field("EmplID", hidden_salesmanId.Text);
                        DynRec.set_Field("ProcessStatus", "Submit by " + Label_Salesman.Text + " " + DateTime.Now + "<br/>");

                        DynRec.set_Field("LastInvoiceID", Label_LatestInvoice.Text);
                        DynRec.set_Field("GoldPendant", TextBox_GoldPendant.Text);
                        DynRec.set_Field("VoucherCheque", TextBox_VoucherCheque.Text);
                        DynRec.set_Field("CustName", Label_CustName.Text);

                        DynRec.Call("insert");
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
                Response.Redirect("EventBudget.aspx", true);
            }
            catch (Exception ER_EB_20)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EB_20: " + ER_EB_20.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_EventNum_Click(object sender, EventArgs e)
        {
            string selected_Event_Id = "";
            Button Button_EventNum = sender as Button;
            if (Button_EventNum != null)
            {
                selected_Event_Id = Button_EventNum.Text;

                Axapta DynAx = Function_Method.GlobalAxapta();

                try
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
                    }
                    var getDetails = EventBudget_GET_NewApplicant.getParticipantDet(DynAx, hidden_salesmanId.Text);
                    Session["data_passing"] = "@EBCM_" + getDetails.Item2 + "|" + getDetails.Item1 + "|" + hidden_salesmanId.Text;//EB > EB
                    Response.Redirect("EventBudget.aspx");
                }
                catch (Exception ER_EB_12)
                {
                    Function_Method.MsgBox("ER_EB_12: " + ER_EB_12.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Logoff();
                }
            }
        }

        protected void ddlEventCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            validate(DynAx);
        }

        protected void gvNewParticipation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the current data item
                DataRowView drv = (DataRowView)e.Row.DataItem;

                // Check the PetroAllowance field and set the CheckBox accordingly
                bool isPetroAllowance = Convert.ToBoolean(drv["Petrol/Allowance"]);
                CheckBox chkPetrolAllowance = (CheckBox)e.Row.FindControl("chkPetrolAllowance");
                chkPetrolAllowance.Text = "";
                if (isPetroAllowance)
                {
                    chkPetrolAllowance.Checked = true;
                }
                else
                {
                    chkPetrolAllowance.Checked = false;
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainMenu.aspx", false);
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            btnList.Attributes.Add("style", "background-color:#f58345");
            btnForm.Attributes.Add("style", "background-color:transparent");

            divNewList_section.Visible = true;
            eb_header_section.Visible = false;
            OverviewSL(0);
        }

        protected void btnForm_Click(object sender, EventArgs e)
        {
            btnForm.Attributes.Add("style", "background-color:#f58345");
            btnList.Attributes.Add("style", "background-color:transparent");

            eb_header_section.Attributes.Add("style", "display:initial");
            eb_header_section.Visible = true;
            divNewList_section.Visible = false;
        }

        [WebMethod]
        public static string UpdateRecord(int pax, int totalRoom, string chkAllowance, string eventCode, string acc, string custName)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebParticipant"))
            {
                DynAx.TTSBegin();

                if (!string.IsNullOrEmpty(eventCode))
                {
                    DynRec.set_Field("EventNumber", eventCode);
                    //DynRec.set_Field("EventDate", Label_EventDate.Text);

                    DynRec.set_Field("NoOfParticipant", pax);
                    DynRec.set_Field("CustAccount", acc);
                    DynRec.set_Field("NumOfRoom", totalRoom);
                    //DynRec.set_Field("EmplID", );
                    //DynRec.set_Field("ProcessStatus", "Submit by " + Label_Salesman.Text + " " + DateTime.Now + "<br/>");                         
                    DynRec.set_Field("CustName", custName);

                    DynRec.Call("insert");
                }
                DynAx.TTSCommit();
                DynAx.TTSAbort();
            }

            DynAx.Logoff();

            return "Success";
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
