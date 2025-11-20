using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class EOR : System.Web.UI.Page
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

                clear_parameter_NewApplication_CustomerInfo(); Show_Hide_NewApplication_CustomerInfo(true);
                Accordion_CustInfo.Visible = true;
                clear_parameter_NewApplication_EORPart();
                Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
                Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);

                Session["data_passing"] = "";//in case forget reset
                Session["flag_temp"] = 0;

                Check_DataRequest();

                //====================================================================================================================================
                if (hidden_Label_PreviousDraftStatus.Text == "1" || hidden_Label_PreviousDraftStatus.Text == "")//1: update, previous is draft
                {
                    Button_SaveDraft.Visible = true;
                }
                else if (hidden_Label_PreviousDraftStatus.Text == "0")//0: update, previous not draft
                {
                    Button_SaveDraft.Visible = false;
                }

                //====================================================================================================================================
                //
                string DocStatus = hidden_DocStatus.Text;
                if (DocStatus != "")
                {
                    switch (DocStatus)
                    {
                        case "0"://draft
                            Button_Reject.Visible = false;
                            Button_RejectToDraft.Visible = false;
                            Button_Submit.Text = "Submit";
                            Button_EORAdmin.Visible = false;
                            ButtonAdd_Equipment.Enabled = true;
                            break;

                        case "1": // Awaiting HOD
                            Button_Reject.Visible = false;
                            Button_RejectToDraft.Visible = true;
                            Button_Submit.Text = "Edit and Approve";
                            Button_EORAdmin.Visible = false;
                            ButtonAdd_Equipment.Enabled = true;
                            break;

                        case "2": // Awaiting Sales Admin
                            Button_Reject.Visible = true;
                            Button_RejectToDraft.Visible = true;
                            Button_Submit.Text = "Edit and Approve";
                            Button_EORAdmin.Visible = false;
                            break;

                        case "3": // Awaiting Sales Admin Manager//only GM and Manager can straight reject
                            Button_Reject.Visible = true;
                            Button_RejectToDraft.Visible = true;
                            Button_Submit.Text = "Edit and Approve";
                            Button_EORAdmin.Visible = true;
                            ButtonAdd_Equipment.Enabled = false;

                            Button_CalculateEquipmentAdmin.Enabled = false;
                            Button_Save_Admin.Enabled = false;
                            break;

                        case "4": // /Awaiting GM //only GM and Manager can straight reject
                            Button_Reject.Visible = false;
                            Button_RejectToDraft.Visible = true;
                            Button_Submit.Text = "Edit and Final Approve";
                            Button_EORAdmin.Visible = true;
                            ButtonAdd_Equipment.Enabled = false;

                            Button_CalculateEquipmentAdmin.Enabled = false;
                            Button_Save_Admin.Enabled = false;
                            break;

                        default:
                            Button_Reject.Visible = false;
                            Button_RejectToDraft.Visible = false;
                            Button_Submit.Text = "Submit";
                            Button_EORAdmin.Visible = false;
                            ButtonAdd_Equipment.Enabled = false;
                            break;
                    }
                }
                else
                {
                    Button_Reject.Visible = false;
                    Button_RejectToDraft.Visible = false;
                    Button_Submit.Text = "Submit";
                    Button_EORAdmin.Visible = false;
                    ButtonAdd_Equipment.Enabled = true;
                }
            }
        }
        //====================================================================================================
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
                //
            }
            catch
            {
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
                    if (temp1.Length >= 6)//correct size
                    {
                        //string test = temp1.Substring(0, 6);
                        if (temp1.Substring(0, 6) == "@EOCM_")//Data receive from CustomerMaster> EOR
                        {
                            TextBox_Account.Text = temp1.Substring(6);
                            validate();
                            Alt_Addr_function();
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);//go to new_applicant_section
                        }

                        if (temp1.Substring(0, 6) == "@EOIM_")//Data receive from InventoryrMaster> EOR
                        {
                            Session["flag_temp"] = temp1;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_enquiries_section'); ", true);//go to enquiries_section
                        }
                        if (temp1.Substring(0, 6) == "@EOEO_")//Data receive from EOR> EOR from overview section
                        {
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);//go to new_applicant_section

                            string temp2 = temp1.Substring(6);
                            string[] arr_temp2 = temp2.Split('|');
                            string temp_EquipmentId = arr_temp2[0];
                            string Status = arr_temp2[1];

                            if (Status == "Draft" || Status == "0")
                            {
                                hidden_Label_PreviousDraftStatus.Text = "1";//to update, last state is draft
                            }
                            else
                            {
                                hidden_Label_PreviousDraftStatus.Text = "0";//to update, last state not draft
                            }

                            string EquipmentRecId = get_data_load(DynAx, temp_EquipmentId);

                            if (temp_EquipmentId != "")
                            {
                                get_data_load_GridEquipment(DynAx, EquipmentRecId);
                                get_data_load_ProposedProduct(DynAx, EquipmentRecId);
                                get_data_load_SalesAdmin(DynAx, EquipmentRecId);
                                hidden_Label_EquipmentRecId_Update.Text = EquipmentRecId;
                                hidden_Label_EquipmentRecId.Text = "";
                            }
                            else
                            {
                                clear_parameter_NewApplication_CustomerInfo();
                                clear_parameter_NewApplication_EORPart();
                            }
                            //=====================================================================================================================================================================

                            var tuple_Check_ApprovalListFromLF_WebEquipment = EOR_GET_NewApplicant.Check_ApprovalListFromLF_WebEquipment(DynAx, temp_EquipmentId);
                            string NA_HOD = tuple_Check_ApprovalListFromLF_WebEquipment.Item1;
                            string NA_Admin = tuple_Check_ApprovalListFromLF_WebEquipment.Item2;
                            string NA_Manager = tuple_Check_ApprovalListFromLF_WebEquipment.Item3;
                            string NA_GM = tuple_Check_ApprovalListFromLF_WebEquipment.Item4;

                            string NextApproval = tuple_Check_ApprovalListFromLF_WebEquipment.Item5;
                            string NextApprovalAlt = tuple_Check_ApprovalListFromLF_WebEquipment.Item6;
                            string DocStatus = tuple_Check_ApprovalListFromLF_WebEquipment.Item7;

                            hidden_NA_HOD.Text = NA_HOD;
                            hidden_NA_Admin.Text = NA_Admin;
                            hidden_NA_Manager.Text = NA_Manager;
                            hidden_NA_GM.Text = NA_GM;
                            hidden_NextApproval.Text = NextApproval;
                            hidden_NextApprovalAlt.Text = NextApprovalAlt;
                            hidden_DocStatus.Text = DocStatus;

                            getEorPicture(Label_Salesman.Text, Label_CustName.Text);
                        }

                        if (temp1.Substring(0, 6) == "@EOE2_")//Data receive from EOR> EOR from New Applicant: Admin Submit section
                        {
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_overview_section'); ", true);//go to new_applicant_section
                        }

                    }
                    Session["data_passing"] = "";
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);
                }
            }
            catch (Exception ER_EO_22)
            {
                Function_Method.MsgBox("ER_EO_22: " + ER_EO_22.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }

        }
        //====================================================================================================

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void Button_new_applicant_section_Click(object sender, EventArgs e)
        {
            new_applicant_section.Attributes.Add("style", "display:initial"); Button_new_applicant_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            draft_section.Attributes.Add("style", "display:none"); Button_draft_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:none"); Button_overview_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
        }
        protected void Button_draft_section_Click(object sender, EventArgs e)
        {
            clear_parameter_NewApplication_CustomerInfo(); Show_Hide_NewApplication_CustomerInfo(true);
            Accordion_CustInfo.Visible = true;
            clear_parameter_NewApplication_EORPart();
            Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
            Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);


            new_applicant_section.Attributes.Add("style", "display:none"); Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            draft_section.Attributes.Add("style", "display:initial"); Button_draft_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            overview_section.Attributes.Add("style", "display:none"); Button_overview_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            performance_listing(0, "");

        }
        protected void Button_overview_section_Click(object sender, EventArgs e)
        {
            clear_parameter_NewApplication_CustomerInfo(); Show_Hide_NewApplication_CustomerInfo(true);
            Accordion_CustInfo.Visible = true;
            clear_parameter_NewApplication_EORPart();
            Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
            Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);

            new_applicant_section.Attributes.Add("style", "display:none"); Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            draft_section.Attributes.Add("style", "display:none"); Button_draft_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:initial"); Button_overview_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            enquiries_section.Attributes.Add("style", "display:none"); Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            f_Button_ListAll();
        }
        protected void Button_enquiries_section_Click(object sender, EventArgs e)
        {
            clear_parameter_NewApplication_CustomerInfo(); Show_Hide_NewApplication_CustomerInfo(true);
            Accordion_CustInfo.Visible = true;
            clear_parameter_NewApplication_EORPart();
            Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
            Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);

            new_applicant_section.Attributes.Add("style", "display:none"); Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            draft_section.Attributes.Add("style", "display:none"); Button_draft_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:none"); Button_overview_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:initial"); Button_enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);

            string temp1 = Session["flag_temp"].ToString();
            if (temp1.Length >= 6)//correct size
            {
                if (temp1.Substring(0, 6) == "@EOIM_")// go to CartonListing creating carton listing
                {
                    string temp1_selected_ItemId = temp1.Substring(6);
                    string[] arr_temp1_selected_ItemId = temp1_selected_ItemId.Split('|');
                    string ItemId = arr_temp1_selected_ItemId[0];
                    string ItemName = arr_temp1_selected_ItemId[1];
                    clear_variable_CartonListNewUpdate();
                    Label_ItemID.Text = ItemId;
                    Label_ItemName.Text = ItemName;

                    f_Button_EORCartonNew("0");//0: create, 1:update
                    Session["data_passing"] = 0;
                }
            }
            else//go to CartonListing showing grid
            {
                f_Button_EORCartonList();
            }
        }

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void Button_EORAdmin_Click(object sender, EventArgs e)
        {
            EOR_Admin();
            GetRecalculate_Admin();
        }
        private void EOR_Admin()
        {
            if (Accordion_EORPartAdmin.Visible == false)
            {
                Accordion_EORPartAdmin.Visible = true;
                Button_EORAdmin.Attributes.Add("style", "background-color:#cc5500");
                new_applicant_section_EORPartAdmin.Attributes.Add("style", "display:initial");
            }
            else
            {
                Accordion_EORPartAdmin.Visible = false;
                Button_EORAdmin.Attributes.Add("style", "color: ");
                new_applicant_section_EORPartAdmin.Attributes.Add("style", "display:none");
            }
            //Accordion_EORPart2.Visible = true; Show_Hide_NewApplication_EORPart2(true);
        }

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            submit();
        }

        private void submit()
        {
            //if (hidden_Label_SaveDraft.Text == "")//if save as draft mode dont need check
            //{
            //    string er1 = check_customer_info_forSubmit();
            //    if (er1 != "")
            //    {
            //        Function_Method.MsgBox("Error in customer info section: " + er1, this.Page, this);
            //        return;
            //    }

            //    string er2 = check_EOR_part1_forSubmit();
            //    if (er2 != "")
            //    {

            //        Function_Method.MsgBox("Error in EOR part 1 section: " + er2, this.Page, this);
            //        return;
            //    }
            //}

            string temp_EquipmentId = hidden_Label_EquipmentRecId.Text;
            string temp_EquipmentId_Update = hidden_Label_EquipmentRecId_Update.Text;

            bool Edit_EOR; string Equip_Id;
            if (hidden_Label_EquipmentRecId_Update.Text != "")//to update
            {
                Edit_EOR = true;
                Equip_Id = store_EOR(Edit_EOR, hidden_Label_EquipmentRecId_Update.Text);
                if (Equip_Id != "")
                {
                    store_EOR_WebEquipment_Item(Edit_EOR, hidden_Label_EquipmentRecId_Update.Text);
                    store_EOR_Proposed_Item(Edit_EOR, hidden_Label_EquipmentRecId_Update.Text);
                }
            }
            else //to insert
            {
                Edit_EOR = false;
                Equip_Id = store_EOR(Edit_EOR, "");
                if (Equip_Id != "")
                {
                    store_EOR_WebEquipment_Item(Edit_EOR, hidden_Label_EquipmentRecId.Text);
                    store_EOR_Proposed_Item(Edit_EOR, hidden_Label_EquipmentRecId.Text);
                }
            }

            //check picture upload 
            string uploadSuccess = UploadPicture();
            if (uploadSuccess != "")
            {
                return;
            }
            //==========================================================================================================================================
            //to update process status
            Axapta DynAx = new Axapta();

            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                {
                    if (Equip_Id != "")
                    {
                        string WebEquipmentProcessStatus = EOR_GET_NewApplicant.getWebEquipmentProcessStatus(DynAx, Equip_Id);
                        string temp_WebEquipmentProcessStatus = "";

                        if (Edit_EOR == false)
                        {
                            var today_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            if (hidden_Label_SaveDraft.Text == "1")
                            {
                                temp_WebEquipmentProcessStatus = WebEquipmentProcessStatus + "DF: " + GLOBAL.user_id + " on " + today_date + ".";
                            }
                            else
                            {
                                temp_WebEquipmentProcessStatus = WebEquipmentProcessStatus + "SB: " + GLOBAL.user_id + " on " + today_date + ".";
                            }

                            DynAx.TTSBegin();

                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Equip_Id", Equip_Id));
                            if (DynRec.Found)
                            {
                                DynRec.set_Field("ProcessStatus", temp_WebEquipmentProcessStatus);//rejected

                                DynRec.Call("Update");
                            }
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();

                        }
                        else if (Edit_EOR == true)
                        {
                            var today_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                            string CurrentEORStatus = hidden_DocStatus.Text;
                            string ShortCode = "";//default update / approve
                            if (CurrentEORStatus == "0")//"Draft")
                            {
                                if (hidden_Label_SaveDraft.Text == "1")
                                {
                                    ShortCode = "DU";
                                }
                                else
                                {
                                    ShortCode = "SB";
                                }
                            }
                            else if (CurrentEORStatus == "1")// "Awaiting HOD")
                            {
                                string ShortAbbr = "";
                                string[] arr_NA_HOD = hidden_NA_HOD.Text.Split('_');
                                int count_arr_NA_HOD = arr_NA_HOD.Count();
                                for (int i = 0; i < count_arr_NA_HOD; i++)
                                {
                                    if (GLOBAL.user_id == arr_NA_HOD[i])
                                    {
                                        ShortAbbr = (i + 1).ToString();
                                    }
                                }

                                ShortCode = "H" + ShortAbbr;
                            }
                            else if (CurrentEORStatus == "2")//"Awaiting Sales Admin")
                            {
                                ShortCode = "AD";
                            }
                            else if (CurrentEORStatus == "3")//"Awaiting Sales Admin Manager")
                            {
                                ShortCode = "AM";
                            }
                            else if (CurrentEORStatus == "4")//"Awaiting GM")
                            {
                                ShortCode = "GM";
                            }
                            temp_WebEquipmentProcessStatus = WebEquipmentProcessStatus + ShortCode + ": " + GLOBAL.user_id + " on " + today_date + ".";

                            DynAx.TTSBegin();
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Equip_Id", Equip_Id));
                            if (DynRec.Found)
                            {
                                DynRec.set_Field("ProcessStatus", temp_WebEquipmentProcessStatus);//rejected

                                DynRec.Call("Update");
                            }
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                    }
                }
                //==========================================================================================================================================

                if (Edit_EOR == false && Equip_Id != "")
                {
                    Function_Method.MsgBox("EOR application is created. Equipment ID: " + Equip_Id, this.Page, this);
                }
                else if (Edit_EOR == true && Equip_Id != "")//Update
                {
                    Function_Method.MsgBox("EOR application is updated. Equipment ID: " + Equip_Id, this.Page, this);
                }
                else
                {
                    Function_Method.MsgBox("EOR application failed", this.Page, this);
                }

                //==========================================================================================================================================
                string status = hidden_DocStatus.Text;
                if (status == "2")//Sales Admin
                {
                    Button_Submit.Visible = false;

                    Button_EORAdmin.Visible = true;
                }
                else
                {
                    clear_parameter_NewApplication_CustomerInfo();
                    clear_parameter_NewApplication_EORPart();
                    //==========================================================================================================================================
                    clear_parameter_NewApplication_CustomerInfo();
                    clear_parameter_NewApplication_EORPart();

                    Show_Hide_NewApplication_CustomerInfo(true);
                    Accordion_CustInfo.Visible = true;
                    Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
                    Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_overview_section'); ", true);//go to overview_section
                }
            }
            catch (Exception ER_EO_18)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_18: " + ER_EO_18.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }
        protected void Button_SaveDraft_Click(object sender, EventArgs e)
        {
            hidden_Label_SaveDraft.Text = "1";
            submit();
        }

        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();

            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                {
                    string Equip_Id = "";
                    string EquipmentRecId_Update = hidden_Label_EquipmentRecId_Update.Text;
                    if (EquipmentRecId_Update != "")
                    {
                        Equip_Id = EOR_GET_NewApplicant.get_WebEquipmentId(DynAx, EquipmentRecId_Update);
                    }
                    else
                    {
                        return;
                    }

                    if (Equip_Id != "")
                    {
                        string WebEquipmentProcessStatus = EOR_GET_NewApplicant.getWebEquipmentProcessStatus(DynAx, Equip_Id);
                        string temp_WebEquipmentProcessStatus = "";
                        var today_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        temp_WebEquipmentProcessStatus = WebEquipmentProcessStatus + "RJ: " + GLOBAL.user_id + " on " + today_date + ".";

                        DynAx.TTSBegin();
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Equip_Id", Equip_Id));
                        if (DynRec.Found)
                        {
                            DynRec.set_Field("ProcessStatus", temp_WebEquipmentProcessStatus);//rejected
                            DynRec.set_Field("DocStatus", 6);//rejected

                            DynRec.set_Field("NextApprover", "");
                            DynRec.set_Field("NextApproverAlt", "");
                            DynRec.Call("Update");
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();

                        //email notification//----------------------------------------------------------------------------------------
                        string AppliedById = "";
                        if (TextBox_Requested.Text != "")
                        {
                            AppliedById = WClaim_GET_NewApplicant.CheckUserId(DynAx, TextBox_Requested.Text);
                        }

                        string MailSubject = "Rejected. EOR: " + Equip_Id;
                        string MailTo = AppliedById + "@posim.com.my";

                        //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                        string SendMsg = "EOR form " + "(" + Equip_Id + ") " + "has been rejected." + "\n" + "\n" +
                               "Customer Acc No: " + TextBox_Account.Text + "\n" +
                            "Customer Name: " + Label_CustName.Text + "\n" +
                            "EOR Id: " + Equip_Id + "\n" +
                            "Status: " + "Rejected";
                        Function_Method.SendMail(GLOBAL.user_id, GLOBAL.logined_user_name, MailSubject, MailTo, "", SendMsg);
                        //----------------------------------------------------------------------------------------------------------------
                    }
                }

                //==========================================================================================================================================

                clear_parameter_NewApplication_CustomerInfo();
                clear_parameter_NewApplication_EORPart();

                Show_Hide_NewApplication_CustomerInfo(true);
                Accordion_CustInfo.Visible = true;
                Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
                Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_overview_section'); ", true);//go to overview_section
            }
            catch (Exception ER_EO_21)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_21: " + ER_EO_21.ToString(), this.Page, this);
            }
        }

        protected void Button_RejectToDraft_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();

            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string Equip_Id = "";
                string EquipmentRecId_Update = hidden_Label_EquipmentRecId_Update.Text;
                if (EquipmentRecId_Update != "")
                {
                    Equip_Id = EOR_GET_NewApplicant.get_WebEquipmentId(DynAx, EquipmentRecId_Update);
                }
                else
                {
                    return;
                }

                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                {
                    if (Equip_Id != "")
                    {
                        string WebEquipmentProcessStatus = EOR_GET_NewApplicant.getWebEquipmentProcessStatus(DynAx, Equip_Id);
                        var today_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        WebEquipmentProcessStatus = WebEquipmentProcessStatus + "RD: " + GLOBAL.user_id + " on " + today_date + ".";
                        DynAx.TTSBegin();
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Equip_Id", Equip_Id));
                        if (DynRec.Found)
                        {
                            DynRec.set_Field("ProcessStatus", WebEquipmentProcessStatus);
                            DynRec.set_Field("DocStatus", 0);//rejected to draft

                            DynRec.set_Field("NextApprover", "");
                            DynRec.set_Field("NextApproverAlt", "");

                            DynRec.Call("Update");
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();

                        //email notification//----------------------------------------------------------------------------------------
                        string AppliedById = "";
                        if (TextBox_Requested.Text != "")
                        {
                            AppliedById = WClaim_GET_NewApplicant.CheckUserId(DynAx, TextBox_Requested.Text);
                        }

                        string MailSubject = "Rejected to Draft. EOR: " + Equip_Id;
                        string MailTo = AppliedById + "@posim.com.my";

                        //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                        string SendMsg = "EOR form " + "(" + Equip_Id + ") " + "has been rejected." + "\n" + "\n" +
                               "Customer Acc No: " + TextBox_Account.Text + "\n" +
                            "Customer Name: " + Label_CustName.Text + "\n" +
                            "EOR Id: " + Equip_Id + "\n" +
                            "Status: " + "Rejected to Draft";

                        Function_Method.SendMail(GLOBAL.user_id, GLOBAL.logined_user_name, MailSubject, MailTo, "", SendMsg);
                        //----------------------------------------------------------------------------------------------------------------
                    }
                }

                //==========================================================================================================================================

                clear_parameter_NewApplication_CustomerInfo();
                clear_parameter_NewApplication_EORPart();

                Show_Hide_NewApplication_CustomerInfo(true);
                Accordion_CustInfo.Visible = true;
                Accordion_EORPart1.Visible = false; Show_Hide_NewApplication_EORPart1(false);
                Accordion_EORPart2.Visible = false; Show_Hide_NewApplication_EORPart2(false);
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_overview_section'); ", true);//go to overview_section
            }
            catch (Exception ER_EO_21)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_21: " + ER_EO_21.ToString(), this.Page, this);
            }
        }

        private string check_customer_info_forSubmit()
        {
            string error = "";
            // <!--Customer Info ////////////////////////////////////////////////////////////////////////////////////-->
            if (TextBox_Account.Text == "")
            {
                error = "Account is empty";
            }

            if (rblContractDuration.SelectedValue == "")
            {
                if (error == "")
                {
                    error = "Duration of Contract is empty";
                }
                else
                {
                    error += "Duration of Contract is empty";
                }
            }

            if (rblRequestType.SelectedValue == "")
            {
                if (error == "")
                {
                    error = "Request type is empty";
                }
                else
                {
                    error += "Request type are empty";
                }
            }
            return error;
        }

        private string check_EOR_part1_forSubmit()
        {
            string error = "";

            int row_count = GridView_Equipment.Rows.Count;
            if (row_count < 1)
            {
                error = "No equipment added";
            }
            else
            {
                //
                for (int i = 0; i < row_count; i++)
                {
                    if (GridView_Equipment.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox box1 = (GridView_Equipment.Rows[i].Cells[3].FindControl("TextBox_New_QTY") as TextBox);
                        TextBox box2 = (GridView_Equipment.Rows[i].Cells[1].FindControl("TextBox_Description") as TextBox);
                        TextBox box3 = (TextBox)GridView_Equipment.Rows[i].Cells[4].FindControl("TextBox_DepositEquipment");

                        if (box2.Text.ToString() == "" || box1.Text.ToString() == "" || box3.Text.ToString() == "")
                        {
                            error = "There is empty field in equipment gridview";
                            goto SKIP;
                        }
                    }
                }
            SKIP:;
            }
            return error;
        }
        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        private void store_EOR_Proposed_Item(bool Edit_EOR, string Equipment_RECID)
        {
            Axapta DynAx = new Axapta();

            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            DataTable dt = (DataTable)ViewState["CurrentTable_ProposedProduct"];
            int temp_row_count = dt.Rows.Count;
            try
            {
                var tuple_get_Proposed_Item = get_Proposed_Item(DynAx);
                string[] Proposed_Description = tuple_get_Proposed_Item.Item1;

                string[] Proposed_New_QTY = tuple_get_Proposed_Item.Item2;
                int row_count_ProposedItem = tuple_get_Proposed_Item.Item3;
                string[] Proposed_ItemId = tuple_get_Proposed_Item.Item4;

                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment_Propose"))
                {
                    if (row_count_ProposedItem >= 1)
                    {
                        if (Edit_EOR == false)//insert
                        {
                            DynAx.TTSBegin();
                            Insert_Updata_Proposed_Item(DynRec, row_count_ProposedItem, Equipment_RECID, Proposed_New_QTY, Proposed_Description, Proposed_ItemId, Edit_EOR);
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                        else//update (delete and insert, cant use update since admin want to have delete and insert option)
                        {
                            reinsert_routine(DynAx, DynRec, Equipment_RECID, row_count_ProposedItem);
                            DynAx.TTSBegin();

                            Insert_Updata_Proposed_Item(DynRec, row_count_ProposedItem, Equipment_RECID, Proposed_New_QTY, Proposed_Description, Proposed_ItemId, Edit_EOR);
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                    }
                }
            }
            catch (Exception ER_EO_16)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_16: " + ER_EO_16.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void Insert_Updata_Proposed_Item(AxaptaRecord DynRec, int row_count_WebEquipmentItem, string Equipment_RECID, string[] Equipment_New_QTY, string[] Equipment_Description, string[] Equipment_ItemId, bool Edit_EOR)
        {
            for (int j = 0; j < row_count_WebEquipmentItem; j++)
            {
                var int_Equipment_RECID = Convert.ToInt64(Equipment_RECID);
                DynRec.set_Field("RefRecId", int_Equipment_RECID);
                DynRec.set_Field("Quantity", Equipment_New_QTY[j]);

                DynRec.set_Field("ItemName", Equipment_Description[j]);
                if (Equipment_ItemId[j] != null)//in case no id
                {
                    DynRec.set_Field("ItemId", Equipment_ItemId[j]);
                }
                DynRec.Call("insert");
            }
        }

        private Tuple<string[], string[], int, string[]> get_Proposed_Item(Axapta DynAx)
        {
            int row_count_ProposedItem = GridView_ProposedProduct.Rows.Count;
            string[] Proposed_New_QTY = new string[row_count_ProposedItem];
            string[] Proposed_Description = new string[row_count_ProposedItem];
            string[] Proposed_ItemId = new string[row_count_ProposedItem];

            if (row_count_ProposedItem >= 1)
            {
                for (int i = 0; i < row_count_ProposedItem; i++)
                {
                    if (GridView_ProposedProduct.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox box1 = (GridView_ProposedProduct.Rows[i].Cells[3].FindControl("TextBox_New_QTY") as TextBox);
                        TextBox box2 = (GridView_ProposedProduct.Rows[i].Cells[1].FindControl("TextBox_Description") as TextBox);

                        if (box2.Text.ToString() != "" && box1.Text.ToString() != "")
                        {
                            Proposed_Description[i] = box2.Text.ToString();
                            Proposed_New_QTY[i] = box1.Text.ToString();
                        }
                        else
                        {
                            Proposed_Description[i] = "";
                            Proposed_New_QTY[i] = "";
                        }
                    }
                }
                Proposed_ItemId = get_WebEquipment_ItemId(DynAx, Proposed_Description, row_count_ProposedItem);// can reuse code get_WebEquipment_ItemId 
            }
            return new Tuple<string[], string[], int, string[]>(Proposed_Description, Proposed_New_QTY, row_count_ProposedItem, Proposed_ItemId);
        }
        //=============================================================================================================================
        private void store_EOR_WebEquipment_Item(bool Edit_EOR, string Equipment_RECID)
        {
            Axapta DynAx = new Axapta();

            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                var tuple_get_WebEquipment_Item = get_WebEquipment_Item(DynAx);
                string[] Equipment_Description = tuple_get_WebEquipment_Item.Item1;

                string[] Equipment_New_QTY = tuple_get_WebEquipment_Item.Item2;
                int row_count_WebEquipmentItem = tuple_get_WebEquipment_Item.Item3;
                string[] Equipment_ItemId = tuple_get_WebEquipment_Item.Item4;

                string[] Equipment_Deposit = tuple_get_WebEquipment_Item.Item5;
                string[] Equipment_Carton = tuple_get_WebEquipment_Item.Item6;

                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment_Item"))
                {
                    if (row_count_WebEquipmentItem >= 1)
                    {
                        if (Edit_EOR == false)//insert
                        {
                            DynAx.TTSBegin();
                            Insert_Updata_EOR_WebEquipment_Item(DynRec, row_count_WebEquipmentItem, Equipment_RECID, Equipment_New_QTY, Equipment_Description, Equipment_ItemId, Edit_EOR, Equipment_Deposit, Equipment_Carton);
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                        else//update (delte and insert, cant use update since admin want to have delete and insert option)
                        {
                            if (ButtonAdd_Equipment.Enabled == true)//add is allowed
                            {
                                reinsert_routine(DynAx, DynRec, Equipment_RECID, row_count_WebEquipmentItem);

                                DynAx.TTSBegin();
                                Insert_Updata_EOR_WebEquipment_Item(DynRec, row_count_WebEquipmentItem, Equipment_RECID, Equipment_New_QTY, Equipment_Description, Equipment_ItemId, Edit_EOR, Equipment_Deposit, Equipment_Carton);
                                DynAx.TTSCommit();
                                DynAx.TTSAbort();
                            }
                            else
                            {
                                DynAx.TTSBegin();
                                string temp_EquipmentId_Update = hidden_Label_EquipmentRecId_Update.Text;
                                DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RefRecId", temp_EquipmentId_Update));
                                if (DynRec.Found)
                                {
                                    Insert_Updata_EOR_WebEquipment_Item(DynRec, row_count_WebEquipmentItem, Equipment_RECID, Equipment_New_QTY, Equipment_Description, Equipment_ItemId, Edit_EOR, Equipment_Deposit, Equipment_Carton);
                                }
                                DynAx.TTSCommit();
                                DynAx.TTSAbort();
                            }
                        }
                    }
                }
            }
            catch (Exception ER_EO_16)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_16: " + ER_EO_16.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void reinsert_routine(Axapta DynAx, AxaptaRecord DynRec, string Equipment_RECID, int row_count_WebEquipmentItem)
        {
            //int temp = row_count_WebEquipmentItem;
            for (int j = 0; j < 50; j++)
            {
                DynAx.TTSBegin();
                DynRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RefRecId", Equipment_RECID));

                DynAx.TTSCommit(); DynAx.TTSAbort();
            }
        }

        private void Insert_Updata_EOR_WebEquipment_Item(AxaptaRecord DynRec, int row_count_WebEquipmentItem, string Equipment_RECID, string[] Equipment_New_QTY, string[] Equipment_Description, string[] Equipment_ItemId, bool Edit_EOR, string[] Equipment_Deposit, string[] Equipment_Carton)
        {
            for (int j = 0; j < row_count_WebEquipmentItem; j++)
            {
                var int_Equipment_RECID = Convert.ToInt64(Equipment_RECID);
                DynRec.set_Field("RefRecId", int_Equipment_RECID);
                DynRec.set_Field("Quantity", Equipment_New_QTY[j]);

                DynRec.set_Field("ItemName", Equipment_Description[j]);

                if (Equipment_Carton[j] == "" || Equipment_Carton[j] == null)
                {
                    Equipment_Carton[j] = "";
                }

                DynRec.set_Field("ManualCarton", Equipment_Carton[j]);

                if (Equipment_Deposit[j] == "" || Equipment_Deposit[j] == null)
                {
                    Equipment_Deposit[j] = "0";
                }

                var int_Equipment_Deposit = Convert.ToDouble(Equipment_Deposit[j]);
                DynRec.set_Field("Deposit", int_Equipment_Deposit);

                if (Equipment_ItemId[j] != null)//in case no id
                {
                    DynRec.set_Field("ItemId", Equipment_ItemId[j]);
                }
                if (ButtonAdd_Equipment.Enabled == true)//add is allowed
                {
                    DynRec.Call("insert");
                }
                else
                {
                    DynRec.Call("Update");
                }
            }
        }

        private string[] get_WebEquipment_ItemId(Axapta DynAx, string[] Equipment_Description, int row_count_WebEquipmentItem)
        {
            string[] Equipment_ItemId = new string[row_count_WebEquipmentItem];

            for (int i = 0; i < row_count_WebEquipmentItem; i++)
            {
                if (Equipment_Description[i] != null || Equipment_Description[i] != "")
                {
                    int InventTable = 175;

                    AxaptaObject axQuery1_2 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource1_2 = (AxaptaObject)axQuery1_2.Call("addDataSource", InventTable);
                    var qbr1_2 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 3);//Equipment ItemName
                    qbr1_2.Call("value", Equipment_Description[i]);
                    AxaptaObject axQueryRun1_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_2);

                    if ((bool)axQueryRun1_2.Call("next"))
                    {
                        AxaptaRecord DynRec1_2 = (AxaptaRecord)axQueryRun1_2.Call("Get", InventTable);
                        Equipment_ItemId[i] = DynRec1_2.get_Field("ItemId").ToString();
                        DynRec1_2.Dispose();
                    }
                }
            }
            return Equipment_ItemId;
        }

        private Tuple<string[], string[], int, string[], string[], string[]> get_WebEquipment_Item(Axapta DynAx)
        {
            int row_count_WebEquipmentItem = GridView_Equipment.Rows.Count;
            string[] Equipment_New_QTY = new string[row_count_WebEquipmentItem];
            string[] Equipment_Description = new string[row_count_WebEquipmentItem];
            string[] Equipment_ItemId = new string[row_count_WebEquipmentItem];

            string[] Equipment_Deposit = new string[row_count_WebEquipmentItem];
            string[] Equipment_Carton = new string[row_count_WebEquipmentItem];
            string[] Equipment_RecId = new string[row_count_WebEquipmentItem];
            if (row_count_WebEquipmentItem >= 1)
            {
                for (int i = 0; i < row_count_WebEquipmentItem; i++)
                {
                    if (GridView_Equipment.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox box1 = (TextBox)GridView_Equipment.Rows[i].Cells[2].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_Equipment.Rows[i].Cells[1].FindControl("TextBox_Description");

                        TextBox box3 = (TextBox)GridView_Equipment.Rows[i].Cells[4].FindControl("TextBox_DepositEquipment");
                        Label box4 = (Label)GridView_Equipment.Rows[i].Cells[5].FindControl("Label_CartonEquipment");
                        //Label box5 = (Label)GridView_Equipment.Rows[i].Cells[6].FindControl("Label_RecIdEquipment");

                        Equipment_Description[i] = ""; Equipment_New_QTY[i] = "";
                        Equipment_Deposit[i] = ""; Equipment_Carton[i] = ""; Equipment_RecId[i] = "";

                        if (box2.Text.ToString() != "")
                        {
                            Equipment_Description[i] = box2.Text.ToString();
                        }
                        if (box1.Text.ToString() != "")
                        {
                            Equipment_New_QTY[i] = box1.Text.ToString();
                        }
                        if (box3.Text.ToString() != "")
                        {
                            Equipment_Deposit[i] = box3.Text.ToString();
                        }
                        if (box4.Text.ToString() != "")
                        {
                            Equipment_Carton[i] = box4.Text.ToString();
                        }
                        /*
                        if (box5.Text.ToString() != "" && box5.Text.ToString() != "")
                        {
                            Equipment_RecId[i] = box5.Text.ToString();
                        }*/
                    }
                }
                Equipment_ItemId = get_WebEquipment_ItemId(DynAx, Equipment_Description, row_count_WebEquipmentItem);
            }
            return new Tuple<string[], string[], int, string[], string[], string[]>(Equipment_Description, Equipment_New_QTY, row_count_WebEquipmentItem, Equipment_ItemId,
            Equipment_Deposit, Equipment_Carton);
        }
        //=============================================================================================================================

        private string store_EOR(bool Edit_EOR, string Equip_RecId)//if Edit EOR is true then it is to update
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            string Equip_Id = "";
            try
            {
                string customer_acc = TextBox_Account.Text.Trim();
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                {
                    DynAx.TTSBegin();

                    if (customer_acc != "" && Edit_EOR == false)
                    {
                        Insert_Updata_Data(DynRec);
                        Insert_Updata_EORApprovalDuringFirstInsert(DynAx, DynRec);
                        DynRec.Call("insert");
                        Equip_Id = DynRec.get_Field("EQUIP_ID").ToString();
                        Equip_RecId = DynRec.get_Field("RecId").ToString();
                        hidden_Label_EquipmentRecId.Text = Equip_RecId;//Equipment_RECID
                        hidden_Label_EquipmentRecId_Update.Text = "";
                        //----------------------------------------------------------------------------------------------------------------
                        string NextApprovalUserName = hidden_NextApproval.Text;

                        if (NextApprovalUserName != "")
                        {
                            string MailSubject = "Pending approval for EOR: " + Equip_Id;
                            string NextApprovalId = WClaim_GET_NewApplicant.CheckUserId(DynAx, NextApprovalUserName);
                            string MailCC = "";
                            //hidden_NextApprovalAlt.Text
                            string MailTo = NextApprovalId + "@posim.com.my";
                            string Status = hidden_DocStatus.Text;
                            switch (Status)
                            {
                                case "0":
                                    Status = "Draft";
                                    break;
                                case "1":
                                    Status = "Awaiting HOD";
                                    break;
                                case "2":
                                    Status = "Awaiting SalesAdmin";
                                    break;
                                case "3":
                                    Status = "Awaiting SalesAdmin Manager";
                                    break;
                                case "4":
                                    Status = "Awaiting GM";
                                    break;
                                case "5":
                                    Status = "Approved";
                                    break;
                                case "6":
                                    Status = "Rejected";
                                    break;
                                default:
                                    Status = "";
                                    break;
                            }

                            //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                            string SendMsg = "EOR form " + "(" + Equip_Id + ") " + "is pending for approval." + "\n" + "\n" +
                                   "Customer Acc No: " + TextBox_Account.Text + "\n" +
                                "Customer Name: " + Label_CustName.Text + "\n" +
                                "EOR Id: " + Equip_Id + "\n" +
                                "Status: " + Status;

                            Function_Method.SendMail(GLOBAL.user_id, GLOBAL.logined_user_name, MailSubject, MailTo, MailCC, SendMsg);

                        }
                        //----------------------------------------------------------------------------------------------------------------
                    }
                    else if (customer_acc != "" && Edit_EOR == true)//Update
                    {
                        Equip_Id = "";
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", Equip_RecId));
                        if (DynRec.Found)
                        {
                            Insert_Updata_Data(DynRec);
                            Equip_Id = DynRec.get_Field("EQUIP_ID").ToString();
                            string temp = hidden_Label_SaveDraft.Text;
                            if ((hidden_DocStatus.Text == "2") || (hidden_Label_SaveDraft.Text == "1"))//do not update approval level for SalesAdmin, will update only submit the admin for calculation
                            {
                                hidden_Label_EquipmentRecId_Update.Text = Equip_RecId;//Equipment_RECID
                            }
                            else
                            {
                                bool OKtoGo = Insert_Updata_EORApprovalDuringUpdate(DynAx, DynRec, Equip_Id);
                                hidden_Label_EquipmentRecId.Text = "";
                                if (OKtoGo == true)
                                {
                                    DynRec.Call("Update");
                                    hidden_Label_EquipmentRecId_Update.Text = Equip_RecId;//Equipment_RECID
                                    //----------------------------------------------------------------------------------------------------------------
                                    string NextApprovalUserName = hidden_NextApproval.Text;
                                    string NextApprovalAltUserName = hidden_NextApprovalAlt.Text;
                                    if (NextApprovalUserName != "")
                                    {
                                        string MailSubject = "Pending approval for EOR: " + Equip_Id;
                                        string NextApprovalId = WClaim_GET_NewApplicant.CheckUserId(DynAx, NextApprovalUserName);
                                        string MailCC = "";
                                        //hidden_NextApprovalAlt.Text
                                        string MailTo = NextApprovalId + "@posim.com.my";
                                        if (NextApprovalAltUserName != "")
                                        {
                                            MailTo = MailTo + WClaim_GET_NewApplicant.CheckUserId(DynAx, NextApprovalAltUserName) + "@posim.com.my";
                                        }
                                        string Status = hidden_DocStatus.Text;
                                        switch (Status)
                                        {
                                            case "0":
                                                Status = "Draft";
                                                break;
                                            case "1":
                                                Status = "Awaiting HOD";
                                                break;
                                            case "2":
                                                Status = "Awaiting SalesAdmin";
                                                break;
                                            case "3":
                                                Status = "Awaiting SalesAdmin Manager";
                                                break;
                                            case "4":
                                                Status = "Awaiting GM";
                                                break;
                                            case "5":
                                                Status = "Approved";
                                                break;
                                            case "6":
                                                Status = "Rejected";
                                                break;
                                            default:
                                                Status = "";
                                                break;
                                        }

                                        //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                        string SendMsg = "EOR form " + "(" + Equip_Id + ") " + "is pending for approval." + "\n" + "\n" +
                                               "Customer Acc No: " + TextBox_Account.Text + "\n" +
                                            "Customer Name: " + Label_CustName.Text + "\n" +
                                            "EOR Id: " + Equip_Id + "\n" +
                                            "Status: " + Status;
                                        Function_Method.SendMail(GLOBAL.user_id, GLOBAL.logined_user_name, MailSubject, MailTo, MailCC, SendMsg);

                                    }
                                    //----------------------------------------------------------------------------------------------------------------
                                }
                            }
                        }
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                    return Equip_Id;
                    //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_JournalLine_section'); ", true);
                }
            }
            catch (Exception ER_EO_15)
            {
                Function_Method.MsgBox("ER_EO_15: " + ER_EO_15.ToString(), this.Page, this); return "";
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private bool Insert_Updata_EORApprovalDuringUpdate(Axapta DynAx, AxaptaRecord DynRec, string EQUIP_ID)
        {
            //====================================================================
            string NA_HOD = hidden_NA_HOD.Text;
            string NA_Admin = hidden_NA_Admin.Text;
            string NA_Manager = hidden_NA_Manager.Text;
            string NA_GM = hidden_NA_GM.Text;
            string NextApproval = hidden_NextApproval.Text;
            string NextApprovalAlt = hidden_NextApprovalAlt.Text;
            string DocStatus = hidden_DocStatus.Text;

            string CurrentLogOnUserName = GLOBAL.logined_user_name;
            //====================================================================
            bool OKtoGo = false;
            string UpdatedDocStatus = ""; string UpdatedNextApproval = ""; string UpdatedNextApprovalAlt = "";

            if ((CurrentLogOnUserName == NextApproval) || (CurrentLogOnUserName == NextApprovalAlt) && (DocStatus != "6") || (GLOBAL.user_id == GLOBAL.AdminID) || DocStatus == "0")
            {
                OKtoGo = true;
                if (DocStatus == "0")// Draft 
                {
                    UpdatedDocStatus = "1"; //"Awaiting HOD"

                    string[] arr_NA_HOD = NA_HOD.Split('_');
                    int count_arr_NA_HOD = arr_NA_HOD.Count();
                    string NA_HOD_lvl1 = ""; string UserName_NA_HOD_lvl1 = "";
                    if (count_arr_NA_HOD > 0)
                    {
                        NA_HOD_lvl1 = arr_NA_HOD[0];
                        UserName_NA_HOD_lvl1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, NA_HOD_lvl1);
                    }

                    UpdatedNextApproval = UserName_NA_HOD_lvl1;
                    UpdatedNextApprovalAlt = "";

                }

                else if (DocStatus == "1")// Awaiting HOD
                {
                    string[] arr_NA_Admin = NA_Admin.Split('_');
                    int count_arr_NA_Admin = arr_NA_Admin.Count();
                    string temp_arr_NA_Admin = ""; string temp_arr_NA_Admin_Alt = "";
                    if (count_arr_NA_Admin > 1)
                    {
                        temp_arr_NA_Admin = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_Admin[0]);
                        temp_arr_NA_Admin_Alt = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_Admin[1]);
                    }
                    else if (count_arr_NA_Admin > 0)
                    {
                        temp_arr_NA_Admin = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_Admin[0]);
                    }
                    //
                    //2 scenario if there is more than one level of HOD

                    string[] arr_NA_HOD = NA_HOD.Split('_');
                    int count_arr_NA_HOD = arr_NA_HOD.Count();
                    string NA_HOD_lvl1 = ""; string NA_HOD_lvl2 = ""; string NA_HOD_lvl3 = ""; string NA_HOD_lvl4 = "";
                    //
                    //
                    if (count_arr_NA_HOD == 1)
                    {
                        NA_HOD_lvl1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[0]);

                        if (CurrentLogOnUserName == NA_HOD_lvl1)
                        {
                            UpdatedDocStatus = "2"; //"Awaiting sales Admin
                            UpdatedNextApproval = temp_arr_NA_Admin;
                            UpdatedNextApprovalAlt = temp_arr_NA_Admin_Alt;
                        }
                    }
                    else if (count_arr_NA_HOD == 2)
                    {
                        NA_HOD_lvl1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[0]);
                        NA_HOD_lvl2 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[1]);

                        if (CurrentLogOnUserName == NA_HOD_lvl2)
                        {
                            UpdatedDocStatus = "2"; //"Awaiting sales Admin
                            UpdatedNextApproval = temp_arr_NA_Admin;
                            UpdatedNextApprovalAlt = temp_arr_NA_Admin_Alt;
                        }
                        else if (CurrentLogOnUserName == NA_HOD_lvl1)
                        {
                            UpdatedDocStatus = "1"; //"2nd level HOD
                            UpdatedNextApproval = NA_HOD_lvl2;
                            UpdatedNextApprovalAlt = "";
                        }
                    }
                    else if (count_arr_NA_HOD == 3)
                    {
                        NA_HOD_lvl1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[0]);
                        NA_HOD_lvl2 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[1]);
                        NA_HOD_lvl3 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[2]);

                        if (CurrentLogOnUserName == NA_HOD_lvl3)
                        {
                            UpdatedDocStatus = "2"; //"Awaiting sales Admin
                            UpdatedNextApproval = temp_arr_NA_Admin;
                            UpdatedNextApprovalAlt = temp_arr_NA_Admin_Alt;
                        }
                        else if (CurrentLogOnUserName == NA_HOD_lvl2)
                        {
                            UpdatedDocStatus = "1";
                            UpdatedNextApproval = NA_HOD_lvl3;
                            UpdatedNextApprovalAlt = "";
                        }
                        else if (CurrentLogOnUserName == NA_HOD_lvl1)
                        {
                            UpdatedDocStatus = "1";
                            UpdatedNextApproval = NA_HOD_lvl2;
                            UpdatedNextApprovalAlt = "";
                        }
                    }
                    else if (count_arr_NA_HOD == 4)
                    {
                        NA_HOD_lvl1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[0]);
                        NA_HOD_lvl2 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[1]);
                        NA_HOD_lvl3 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[2]);
                        NA_HOD_lvl4 = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_HOD[3]);

                        if (CurrentLogOnUserName == NA_HOD_lvl4)
                        {
                            UpdatedDocStatus = "2"; //"Awaiting sales Admin
                            UpdatedNextApproval = temp_arr_NA_Admin;
                            UpdatedNextApprovalAlt = temp_arr_NA_Admin_Alt;
                        }
                        else if (CurrentLogOnUserName == NA_HOD_lvl3)
                        {
                            UpdatedDocStatus = "1";
                            UpdatedNextApproval = NA_HOD_lvl4;
                            UpdatedNextApprovalAlt = "";
                        }
                        else if (CurrentLogOnUserName == NA_HOD_lvl2)
                        {
                            UpdatedDocStatus = "1";
                            UpdatedNextApproval = NA_HOD_lvl3;
                            UpdatedNextApprovalAlt = "";
                        }
                        else if (CurrentLogOnUserName == NA_HOD_lvl1)
                        {
                            UpdatedDocStatus = "1";
                            UpdatedNextApproval = NA_HOD_lvl2;
                            UpdatedNextApprovalAlt = "";
                        }
                    }
                }
                else if (DocStatus == "2")//Awaiting Sales Admin
                {
                    UpdatedDocStatus = "3";//Awaiting Sales Admin Manager

                    string[] arr_NA_Manager = NA_Manager.Split('_');

                    int count_arr_NA_Manager = arr_NA_Manager.Count();

                    if (count_arr_NA_Manager > 1)
                    {
                        UpdatedNextApproval = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_Manager[0]);
                        UpdatedNextApprovalAlt = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_Manager[1]);
                    }
                    else if (count_arr_NA_Manager > 0)
                    {
                        UpdatedNextApproval = WClaim_GET_NewApplicant.CheckUserName(DynAx, arr_NA_Manager[0]);
                    }
                }
                else if (DocStatus == "3")//Awaiting Sales Admin Manager
                {
                    UpdatedDocStatus = "4";//Awaiting GM

                    UpdatedNextApproval = WClaim_GET_NewApplicant.CheckUserName(DynAx, NA_GM);
                    UpdatedNextApprovalAlt = "";
                }
                else if (DocStatus == "4")//Awaiting GM
                {
                    UpdatedDocStatus = "5";//Approved
                    UpdatedNextApproval = "";
                    UpdatedNextApprovalAlt = "";
                }
                var int_UpdatedDocStatus = Convert.ToInt32(UpdatedDocStatus);
                DynRec.set_Field("DocStatus", int_UpdatedDocStatus);
                DynRec.set_Field("NextApprover", UpdatedNextApproval);
                DynRec.set_Field("NextApproverAlt", UpdatedNextApprovalAlt);

            }
            return OKtoGo;
        }

        private void Insert_Updata_EORApprovalDuringFirstInsert(Axapta DynAx, AxaptaRecord DynRec)//if Edit EOR is true then it is in edit mode
        {
            string raw_Salesman = Label_Salesman.Text;
            string SalesmanNo = "";
            if (raw_Salesman != "")
            {
                string[] arr_temp_SalesmanNo_Name = raw_Salesman.Split(')');
                SalesmanNo = arr_temp_SalesmanNo_Name[0].Substring(1);//use salesman No
            }
            //var tuple_Check_LF_EOR_Approval = EOR_GET_NewApplicant.Check_LF_EOR_Approval(DynAx, GLOBAL.user_company);//dont have this table  in test and live
            //string NA_Admin = tuple_Check_LF_EOR_Approval.Item1;
            //string NA_Manager = tuple_Check_LF_EOR_Approval.Item2;
            //string NA_GM = tuple_Check_LF_EOR_Approval.Item3;

            string NA_HOD = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, SalesmanNo);

            string[] arr_NA_HOD = NA_HOD.Split('_');
            int count_arr_NA_HOD = arr_NA_HOD.Count();
            string NA_HOD_lvl1 = "";
            if (count_arr_NA_HOD > 0)
            {
                NA_HOD_lvl1 = arr_NA_HOD[0];
            }

            if (hidden_Label_SaveDraft.Text == "")
            {
                //DynRec.set_Field("NA_Admin", NA_Admin);
                //DynRec.set_Field("NA_Manager", NA_Manager);
                //DynRec.set_Field("NA_GM", NA_GM);

                string UserName_NA_HOD_lvl1 = "";
                if (NA_HOD_lvl1 != "") UserName_NA_HOD_lvl1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, NA_HOD_lvl1);
                DynRec.set_Field("NextApprover", UserName_NA_HOD_lvl1);
                DynRec.set_Field("NextApproverAlt", "");
                DynRec.set_Field("DocStatus", 1);//"Awaiting HOD", refer LFWebRedempApprovalStatus enum
                DynRec.set_Field("NA_HOD", NA_HOD);
                hidden_NA_HOD.Text = NA_HOD;
                //hidden_NA_Admin.Text = NA_Admin;
                //hidden_NA_Manager.Text = NA_Manager;
                //hidden_NA_GM.Text = NA_GM;
                hidden_NextApproval.Text = UserName_NA_HOD_lvl1;
                hidden_NextApprovalAlt.Text = "";
                hidden_DocStatus.Text = "1";
                //
            }
            else//save as draft
            {
                DynRec.set_Field("DocStatus", 0);//draft
                //DynRec.set_Field("NA_Admin", NA_Admin);
                //DynRec.set_Field("NA_Manager", NA_Manager);
                //DynRec.set_Field("NA_GM", NA_GM);
                DynRec.set_Field("NA_HOD", NA_HOD);
                hidden_NA_HOD.Text = NA_HOD;
                //hidden_NA_Admin.Text = NA_Admin;
                //hidden_NA_Manager.Text = NA_Manager;
                //hidden_NA_GM.Text = NA_GM;
                hidden_NextApproval.Text = "";
                hidden_NextApprovalAlt.Text = "";
                hidden_DocStatus.Text = "0";
            }
        }

        private void Insert_Updata_Data(AxaptaRecord DynRec)//if Edit EOR is true then it is in edit mode
        {
            string customer_acc = TextBox_Account.Text.Trim();
            //Type -------------------------------------------->
            DynRec.set_Field("DepositType", "EOR");
            //===========================================Cust Info ===============================================
            //customer_acc    -------------------------------------------->
            DynRec.set_Field("CustAccount", customer_acc);
            //customer_tel_no
            DynRec.set_Field("CustPhone", Label_TelNo.Text.Trim());
            //eor_contact_person -------------------------------------------->
            DynRec.set_Field("CustContact", Label_ContactPerson.Text.Trim());
            DynRec.set_Field("Del_Person", Label_CustName.Text.Trim());

            DynRec.set_Field("Del_Addr", Label_Address.Text.Trim());
            //request_type    -------------------------------------------->
            string temp_FormType = "";
            if (rblRequestType.SelectedValue == "1") { temp_FormType = "New"; }
            else if (rblRequestType.SelectedValue == "2") { temp_FormType = "Existing"; }
            else if (rblRequestType.SelectedValue == "3") { temp_FormType = "Branch"; }
            DynRec.set_Field("FormType", temp_FormType);
            //duration_of_contract----------------- ----------------------->
            string temp_duration_of_contract = "";
            if (rblContractDuration.SelectedValue == "1") { temp_duration_of_contract = "12"; }
            else if (rblContractDuration.SelectedValue == "2") { temp_duration_of_contract = "24"; }
            else if (rblContractDuration.SelectedValue == "3") { temp_duration_of_contract = "36"; }
            else if (rblContractDuration.SelectedValue == "4") { temp_duration_of_contract = "48"; }
            else if (rblContractDuration.SelectedValue == "5") { temp_duration_of_contract = "60"; }
            DynRec.set_Field("Period", temp_duration_of_contract);

            DynRec.set_Field("EmplName", Label_Salesman.Text.Trim());
            //Remark SalesMan HOD
            DynRec.set_Field("REMARKS_HOD", TextBox_RemarksSalesHOD.Text.Trim());
            //=============================================== Comment from Salesman ================================
            //owner_experience -------------------------------------------->
            if (DropDownList_Experience.SelectedItem.Text == "")
            {
                DynRec.set_Field("OwnerExp", "");
            }
            else
            {
                DynRec.set_Field("OwnerExp", DropDownList_Experience.SelectedItem.Text);
            }

            //types_of_service_centre-------------------------------------------->
            DynRec.set_Field("ServiceType", TextBox_ServiceCentre.Text.Trim());
            //workshop_facilities-------------------------------------------->
            DynRec.set_Field("ShopFacility", TextBox_Facilities.Text.Trim());
            //workshop_size-------------------------------------------->
            DynRec.set_Field("ShopSize", TextBox_WorkshopSize.Text.Trim());
            //no_of_mechanics-------------------------------------------->
            DynRec.set_Field("Worker", TextBox_Mechanics.Text.Trim());
            //workshop_status-------------------------------------------->
            DynRec.set_Field("ShopStatus", TextBox_WorkshopStatus.Text.Trim());
            //years_of_establishment-------------------------------------------->
            DynRec.set_Field("YearEstablish", TextBox_Establishment.Text.Trim());

            if (hidden_Label_EquipmentRecId_Update.Text != "")//to update
            {

            }
            else
            {
                // first applied by-------------------------------------------- >
                DynRec.set_Field("AppliedBy", TextBox_Requested.Text.Trim());
                //Applied Date
                var today_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");// in lotus use cdat
                var temp_today_date = DateTime.ParseExact(today_date, "dd/MM/yyyy HH:mm:ss", null);// in lotus use cdat
                DynRec.set_Field("AppliedDate", temp_today_date);
            }

            //Applied Time
            //var temp_today_time = DateTime.ParseExact(today_date, "dd/MM/yyyy HH:mm:ss", null);// in lotus use cdat
            //DynRec.set_Field("AppliedTime", temp_today_time);
        }

        private string get_data_load(Axapta DynAx, string temp_EquipmentId)
        {
            string FormType = ""; string duration_of_contract = ""; string EquipmentRecId = "";

            string customer_acc = ""; string DepositType = "";
            int LF_WebEquipment = 30346;
            AxaptaObject axQuery13 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource13 = (AxaptaObject)axQuery13.Call("addDataSource", LF_WebEquipment);

            var qbr13 = (AxaptaObject)axQueryDataSource13.Call("addRange", 30001);//RecId
            qbr13.Call("value", temp_EquipmentId);

            AxaptaObject axQueryRun13 = DynAx.CreateAxaptaObject("QueryRun", axQuery13);
            if ((bool)axQueryRun13.Call("next"))
            {
                AxaptaRecord DynRec13 = (AxaptaRecord)axQueryRun13.Call("Get", LF_WebEquipment);
                customer_acc = DynRec13.get_Field("CustAccount").ToString();
                TextBox_Account.Text = customer_acc;
                validate();

                DepositType = DynRec13.get_Field("DepositType").ToString();

                //request_type    -------------------------------------------->
                FormType = DynRec13.get_Field("FormType").ToString();
                //RadioButton13.Checked = false; RadioButton14.Checked = false; RadioButton15.Checked = false;
                if (FormType == "New")
                {
                    rblRequestType.SelectedIndex = 1;
                }
                else if (FormType == "Existing")
                {
                    rblRequestType.SelectedIndex = 2;
                }
                else if (FormType == "Branch")
                {
                    rblRequestType.SelectedIndex = 3;
                }
                //duration_of_contract----------------- ----------------------->
                duration_of_contract = DynRec13.get_Field("Period").ToString();
                //RadioButton2.Checked = false; RadioButton10.Checked = false; RadioButton11.Checked = false; RadioButton12.Checked = false;
                if (duration_of_contract == "12")
                {
                    rblContractDuration.SelectedIndex = 1;
                }
                else if (duration_of_contract == "24")
                {
                    rblContractDuration.SelectedIndex = 2;
                }
                else if (duration_of_contract == "36")
                {
                    rblContractDuration.SelectedIndex = 3;
                }
                else if (duration_of_contract == "48")
                {
                    rblContractDuration.SelectedIndex = 4;
                }
                else if (duration_of_contract == "60")
                {
                    rblContractDuration.SelectedIndex = 5;
                }
                Show_Hide_NewApplication_EORPart1(true);
                //===============================================
                //Remark SalesMan HOD
                TextBox_RemarksSalesHOD.Text = DynRec13.get_Field("REMARKS_HOD").ToString();
                //=============================================== Comment from Salesman ================================
                //owner_experience -------------------------------------------->
                DropDownList_Experience.ClearSelection();
                DropDownList_Experience.SelectedItem.Text = DynRec13.get_Field("OwnerExp").ToString();
                //types_of_service_centre-------------------------------------------->
                TextBox_ServiceCentre.Text = DynRec13.get_Field("ServiceType").ToString();
                //workshop_facilities-------------------------------------------->
                TextBox_Facilities.Text = DynRec13.get_Field("ShopFacility").ToString();
                //workshop_size-------------------------------------------->
                TextBox_WorkshopSize.Text = DynRec13.get_Field("ShopSize").ToString();
                //no_of_mechanics-------------------------------------------->
                TextBox_Mechanics.Text = DynRec13.get_Field("Worker").ToString();
                //workshop_status-------------------------------------------->
                TextBox_WorkshopStatus.Text = DynRec13.get_Field("ShopStatus").ToString();
                //years_of_establishment-------------------------------------------->
                TextBox_Establishment.Text = DynRec13.get_Field("YearEstablish").ToString();
                //requested_by-------------------------------------------->
                TextBox_Requested.Text = DynRec13.get_Field("AppliedBy").ToString();
                EquipmentRecId = DynRec13.get_Field("RecId").ToString();

                string MonthlyTarget = DynRec13.get_Field("Amount").ToString();
                if (MonthlyTarget != "" || MonthlyTarget != "&nbsp;") Label_MonthlyTarget.Text = MonthlyTarget;
                string Remarks_Admin = DynRec13.get_Field("Remarks_Admin").ToString();
                if (Remarks_Admin != "" || Remarks_Admin != "&nbsp;") TextBox_RemarksAdmin.Text = Remarks_Admin;
                DynRec13.Dispose();
            }
            return EquipmentRecId;
        }

        private void get_data_load_SalesAdmin(Axapta DynAx, string temp_EquipmentId)
        {
            GridView_EquipmentAdmin.DataSource = null;
            GridView_EquipmentAdmin.DataBind();

            int LF_WebEquipment_Item = 30558;

            AxaptaObject axQuery16 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource16 = (AxaptaObject)axQuery16.Call("addDataSource", LF_WebEquipment_Item);

            var qbr16 = (AxaptaObject)axQueryDataSource16.Call("addRange", 30001);//RefRecId
            qbr16.Call("value", temp_EquipmentId);

            AxaptaObject axQueryRun16 = DynAx.CreateAxaptaObject("QueryRun", axQuery16);

            List<ListItem> List_Equipment_New_QTY = new List<ListItem>();
            List<ListItem> List_Equipment_Description = new List<ListItem>();
            List<ListItem> List_Equipment_Charges = new List<ListItem>();
            List<ListItem> List_Equipment_Amount = new List<ListItem>();
            List<ListItem> List_Equipment_TotalAmount = new List<ListItem>();
            List<ListItem> List_Equipment_ManualCarton = new List<ListItem>();

            int count = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));

            dt.Columns.Add(new DataColumn("Types", typeof(string)));
            dt.Columns.Add(new DataColumn("Equipment_Cost", typeof(string)));
            dt.Columns.Add(new DataColumn("Equipment_Handling", typeof(string)));
            dt.Columns.Add(new DataColumn("Carton", typeof(string)));

            dr = dt.NewRow();

            while ((bool)axQueryRun16.Call("next"))
            {
                count = count + 1;
                dr = dt.NewRow();

                AxaptaRecord DynRec16 = (AxaptaRecord)axQueryRun16.Call("Get", LF_WebEquipment_Item);
                string Equipment_New_QTY = DynRec16.get_Field("Quantity").ToString();
                string Equipment_Description = DynRec16.get_Field("ItemName").ToString();
                string Charges = DynRec16.get_Field("Charges").ToString();
                string Amount = DynRec16.get_Field("Amount").ToString();
                string TotalAmount = DynRec16.get_Field("TotalAmount").ToString();
                string ManualCarton = DynRec16.get_Field("ManualCarton").ToString();

                dr["No."] = count;
                dr["Description"] = string.Empty;
                dr["Qty"] = string.Empty;
                dr["Types"] = string.Empty;
                dr["Equipment_Cost"] = string.Empty;
                dr["Equipment_Handling"] = string.Empty;
                dr["Carton"] = string.Empty;

                List_Equipment_New_QTY.Add(new ListItem(Equipment_New_QTY));
                List_Equipment_Description.Add(new ListItem(Equipment_Description));
                List_Equipment_Charges.Add(new ListItem(Charges));
                List_Equipment_Amount.Add(new ListItem(Amount));
                List_Equipment_TotalAmount.Add(new ListItem(TotalAmount));
                List_Equipment_ManualCarton.Add(new ListItem(ManualCarton));

                dt.Rows.Add(dr);
                DynRec16.Dispose();
            }
            GridView_EquipmentAdmin.DataSource = dt;
            GridView_EquipmentAdmin.DataBind();

            for (int i = 0; i < count; i++)
            {
                if (GridView_Equipment.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox rbox1 = (GridView_EquipmentAdmin.Rows[i].Cells[1].FindControl("TextBox_Description_R") as TextBox);
                    TextBox rbox2 = (GridView_EquipmentAdmin.Rows[i].Cells[2].FindControl("TextBox_New_QTY_R") as TextBox);
                    DropDownList DropDownList_NormalItem = (GridView_EquipmentAdmin.Rows[i].Cells[3].FindControl("DropDownList_NormalItem") as DropDownList);
                    TextBox rbox3 = (GridView_EquipmentAdmin.Rows[i].Cells[4].FindControl("TextBox_Equipment_Cost_R") as TextBox);
                    TextBox rbox4 = (GridView_EquipmentAdmin.Rows[i].Cells[5].FindControl("TextBox_Equipment_Handling_R") as TextBox);
                    Label rbox5 = (GridView_EquipmentAdmin.Rows[i].Cells[6].FindControl("Label_CartonEquipment_R") as Label);

                    string temp_List_Equipment_Description = List_Equipment_Description[i].ToString();
                    string temp_List_Equipment_New_QTY = List_Equipment_New_QTY[i].ToString();
                    string temp_List_Equipment_Charges = List_Equipment_Charges[i].ToString();
                    string temp_List_Equipment_Amount = List_Equipment_Amount[i].ToString();
                    string temp_List_Equipment_TotalAmount = List_Equipment_TotalAmount[i].ToString();
                    string temp_List_Equipment_ManualCarton = List_Equipment_ManualCarton[i].ToString();
                    if (temp_List_Equipment_Description != "") rbox1.Text = temp_List_Equipment_Description;
                    if (temp_List_Equipment_New_QTY != "") rbox2.Text = temp_List_Equipment_New_QTY;
                    if (temp_List_Equipment_Charges != "")
                    {
                        //DropDownList_NormalItem.Visible = true;
                        DropDownList_NormalItem.SelectedValue = temp_List_Equipment_Charges;
                    }
                    else
                    {
                        //DropDownList_NormalItem.Visible = false;
                        DropDownList_NormalItem.SelectedValue = "99";
                    }
                    rbox3.Visible = true; rbox4.Visible = true;
                    rbox4.Text = temp_List_Equipment_TotalAmount;
                    rbox5.Text = temp_List_Equipment_ManualCarton;
                    rbox3.Text = temp_List_Equipment_Amount;
                }
            }
        }

        private void get_data_load_GridEquipment(Axapta DynAx, string temp_EquipmentId)
        {
            GridView_Equipment.DataSource = null;
            GridView_Equipment.DataBind();

            int LF_WebEquipment_Item = 30558;
            AxaptaObject axQuery16 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource16 = (AxaptaObject)axQuery16.Call("addDataSource", LF_WebEquipment_Item);

            var qbr16 = (AxaptaObject)axQueryDataSource16.Call("addRange", 30001);//RefRecId
            qbr16.Call("value", temp_EquipmentId);

            AxaptaObject axQueryRun16 = DynAx.CreateAxaptaObject("QueryRun", axQuery16);

            List<ListItem> List_Equipment_New_QTY = new List<ListItem>();
            List<ListItem> List_Equipment_Description = new List<ListItem>();
            List<ListItem> List_Equipment_Deposit = new List<ListItem>();
            List<ListItem> List_Equipment_Carton = new List<ListItem>();
            List<ListItem> List_Equipment_RecId = new List<ListItem>();
            //List<ListItem> List_Equipment_ItemId = new List<ListItem>();

            int count = 0;

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("Deposit", typeof(string)));
            dt.Columns.Add(new DataColumn("Carton", typeof(string)));
            dt.Columns.Add(new DataColumn("RecId", typeof(string)));
            dr = dt.NewRow();

            while ((bool)axQueryRun16.Call("next"))
            {
                count = count + 1;
                dr = dt.NewRow();

                AxaptaRecord DynRec16 = (AxaptaRecord)axQueryRun16.Call("Get", LF_WebEquipment_Item);
                string Equipment_New_QTY = DynRec16.get_Field("Quantity").ToString();
                string Equipment_Description = DynRec16.get_Field("ItemName").ToString();
                string Equipment_Deposit = DynRec16.get_Field("Deposit").ToString();
                string Equipment_Carton = DynRec16.get_Field("ManualCarton").ToString();
                string Equipment_RecId = DynRec16.get_Field("ItemId").ToString();

                dr["No."] = count;
                dr["Description"] = string.Empty;
                dr["Qty"] = string.Empty;
                dr["Deposit"] = string.Empty;
                dr["Carton"] = string.Empty;
                dr["RecId"] = string.Empty;

                List_Equipment_New_QTY.Add(new ListItem(Equipment_New_QTY));
                List_Equipment_Description.Add(new ListItem(Equipment_Description));
                List_Equipment_Deposit.Add(new ListItem(Equipment_Deposit));
                List_Equipment_Carton.Add(new ListItem(Equipment_Carton));
                List_Equipment_RecId.Add(new ListItem(Equipment_RecId));

                dt.Rows.Add(dr);
                DynRec16.Dispose();
            }
            GridView_Equipment.DataSource = dt;
            GridView_Equipment.DataBind();

            ViewState["CurrentTable"] = GridView_Equipment.DataSource;

            for (int i = 0; i < count; i++)
            {
                if (GridView_Equipment.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox box1 = (TextBox)GridView_Equipment.Rows[i].Cells[2].FindControl("TextBox_New_QTY");
                    DropDownList ddl = (DropDownList)GridView_Equipment.Rows[i].Cells[1].FindControl("DropDownList_SearchEquipment");
                    TextBox box3 = (TextBox)GridView_Equipment.Rows[i].Cells[4].FindControl("TextBox_DepositEquipment");
                    Label box4 = (Label)GridView_Equipment.Rows[i].Cells[5].FindControl("Label_CartonEquipment");
                    Label box5 = (Label)GridView_Equipment.Rows[i].Cells[6].FindControl("Label_RecIdEquipment");

                    box1.Text = List_Equipment_New_QTY[i].ToString();
                    ddl.SelectedValue = List_Equipment_Description[i].ToString();
                    box3.Text = List_Equipment_Deposit[i].ToString();
                    box4.Text = List_Equipment_Carton[i].ToString();
                    box5.Text = List_Equipment_RecId[i].ToString();
                }
            }
        }

        private void get_data_load_ProposedProduct(Axapta DynAx, string temp_EquipmentId)
        {
            GridView_ProposedProduct.DataSource = null;
            GridView_ProposedProduct.DataBind();

            int LF_WebEquipment_Propose = 30560;
            AxaptaObject axQuery15 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource15 = (AxaptaObject)axQuery15.Call("addDataSource", LF_WebEquipment_Propose);

            var qbr15 = (AxaptaObject)axQueryDataSource15.Call("addRange", 30001);//RefRecId
            qbr15.Call("value", temp_EquipmentId);

            AxaptaObject axQueryRun15 = DynAx.CreateAxaptaObject("QueryRun", axQuery15);

            List<ListItem> List_Equipment_New_QTY = new List<ListItem>();
            List<ListItem> List_Equipment_Description = new List<ListItem>();
            //List<ListItem> List_Equipment_ItemId = new List<ListItem>();

            int count = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));

            while ((bool)axQueryRun15.Call("next"))
            {
                count = count + 1;
                dr = dt.NewRow();


                AxaptaRecord DynRec15 = (AxaptaRecord)axQueryRun15.Call("Get", LF_WebEquipment_Propose);
                string Equipment_New_QTY = DynRec15.get_Field("Quantity").ToString();
                string Equipment_Description = DynRec15.get_Field("ItemName").ToString();
                //string Equipment_ItemId = DynRec15.get_Field("ItemId").ToString();

                dr["No."] = count;
                dr["Description"] = string.Empty;
                dr["Qty"] = string.Empty;

                List_Equipment_New_QTY.Add(new ListItem(Equipment_New_QTY));
                List_Equipment_Description.Add(new ListItem(Equipment_Description));
                //List_Equipment_ItemId.Add(new ListItem(Equipment_ItemId));
                dt.Rows.Add(dr);
                DynRec15.Dispose();
            }
            GridView_ProposedProduct.DataSource = dt;
            GridView_ProposedProduct.DataBind();

            ViewState["CurrentTable_ProposedProduct"] = GridView_ProposedProduct.DataSource;

            for (int i = 0; i < count; i++)
            {
                if (GridView_ProposedProduct.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox box1 = (GridView_ProposedProduct.Rows[i].Cells[3].FindControl("TextBox_New_QTY") as TextBox);
                    TextBox box2 = (GridView_ProposedProduct.Rows[i].Cells[1].FindControl("TextBox_Description") as TextBox);

                    box1.Text = List_Equipment_New_QTY[i].ToString();
                    box2.Text = List_Equipment_Description[i].ToString();

                }
            }
        }

        //protected void GridView_Equipment_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    Axapta DynAx = new Axapta();
        //    GLOBAL.Company = GLOBAL.switch_Company;
        //    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DropDownList ddlSearchEquipment = (e.Row.FindControl("DropDownList_SearchEquipment") as DropDownList);
        //        ddlSearchEquipment.DataSource = EOR_GET_NewApplicant.get_SearchEquipment(DynAx, "", "");
        //        ddlSearchEquipment.DataBind();
        //    }
        //}
    }
}