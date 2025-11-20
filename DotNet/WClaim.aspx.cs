using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;

namespace DotNet
{
    public partial class WClaim : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            Function_Method.isWarranty = true;//not to bcc Keegan

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

                clear_parameter_NewApplication_Customer();
                clear_parameter_NewApplication_BatteryDetails();
                clear_parameter_NewApplication_ItemDetails();
                clear_parameter_NewApplication_BatchItemDetails();
                //clear_parameter_NewApplication_TransportationArrangement();
                DropDownList();

                Session["data_passing"] = "";//in case forget reset
                Session["flag_temp"] = 0;

                Check_DataRequest();

                if (hidden_Label_PreviousStatus.Text == "" || hidden_Label_PreviousStatus.Text == "Draft")
                {
                    Button_SaveDraft.Visible = true;
                    Button_Reject.Visible = false;
                }
                else
                {
                    Button_SaveDraft.Visible = false;
                    Button_Submit.Text = "Proceed";
                }

                #region Neil - For Edit Battery / Oil Item on submission process
                string claimText = Session["ClaimText"] as string;
                if (claimText == "Awaiting Invoice Chk" || claimText == "Draft")
                {
                    string previousPage = Session["PreviousPage"] as string;


                    if (previousPage == "Battery.aspx")
                    {
                        string claimNum = Session["ClaimNum"] as string;

                        if (!string.IsNullOrEmpty(claimNum) && !string.IsNullOrEmpty(claimText))

                        {
                            Check_Item_From_SelectItem(claimNum);
                            lblClaimNum.Text = claimNum;
                            lblClaimText.Text = claimText;
                            hidden_Label_PreviousStatus.Text = claimText;
                            hidden_LabelClaimNumber.Text = claimNum;
                            lblAcc.Visible = true;
                            // Optionally clear session if no longer
                            fileUpload.Visible = false;
                            fileUpload2.Visible = false;
                            fileUpload3.Visible = false;
                            fileUpload4.Visible = false;
                            lblimg.Visible = false;
                            lblimg2.Visible = false;
                            lblimg3.Visible = false;
                            lblimg4.Visible = false;
                            getClaimImage();
                            Btn_Battery_Item_Save.Visible = true; 
                            Btn_Item_Save.Visible = true;
                            Session.Remove("ClaimNum");
                            Session.Remove("ClaimText");
                            Session.Remove("PreviousPage");
                        }
                    }
                }
                #endregion
            }

            labelAccount.Visible = false;
            //ClientScript.RegisterStartupScript(GetType(), "BackButtonScript", "<script type=\"text/javascript\">history.pushState(null, null, location.href);window.onpopstate = function () {history.go(1);};</script>");
            //avoid user click back button
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
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void Button_new_applicant_section_Click(object sender, EventArgs e)
        {
            btnAmend.Visible = false;
            button_section.Visible = true;
            if (string.IsNullOrEmpty(Label_Account.Text))
            {
                Button_Submit.Visible = false;
                Button_SaveDraft.Visible = false;
                Button_CustomerMasterList.Enabled = true;
            }
            else
            {//to postback 
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('WClaimTag2'); ", true);
                Response.Redirect("WClaim.aspx", true);
                return;
            }

            cusButton.Visible = true;
            Button_Approve.Visible = false;
            overview_section.Visible = false;
            divProcessStat.Visible = false;
            Button_Reject.Visible = false;
            upload_section.Visible = false;
            report_section.Visible = false;
            divApproverRemark.Visible = false;
            new_applicant_sectionTransportationArrangement.Visible = false;
            divInvoiceChkRemark.Visible = false;
            divGoodsReceiveRemark.Visible = false;
            divInspectionRemark.Visible = false;
            divVerficationRemark.Visible = false;
            new_applicant_section.Attributes.Add("style", "display:initial");
            Button_new_applicant_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            btnOverview.Attributes.Add("style", "background-color:transparent");
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:none");
            Button_report_section.Attributes.Add("style", "background-color:transparent");
        }

        protected void btnOverview_Click(object sender, EventArgs e)
        {
            clear_parameter_NewApplication_Customer();
            clear_parameter_NewApplication_BatteryDetails();
            clear_parameter_NewApplication_ItemDetails();
            clear_parameter_NewApplication_BatchItemDetails();
            clear_parameter_NewApplication_TransportationArrangement();
            new_applicant_section.Attributes.Add("style", "display:none");
            Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:initial");
            btnOverview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            enquiries_section.Attributes.Add("style", "display:none");
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            Button_report_section.Attributes.Add("style", "background-color:transparent");
            //btnListRejected.Attributes.Add("style", "background-color:transparent");
            report_section.Visible = false;

            Button_Overview_accordion.Text = "List All";
            //Session["flag_temp"] = 0; // List all
            lblSearch.Visible = true;
            DropDownList_Search_Overview.Visible = true;
            lblStatus.Visible = true;
            ddlStatus.Visible = true;
            ImageButton5.Visible = true;
            overview_section.Visible = true;
            //btnListRejected.Visible = true;
            //lblText.Text = "Number/Name:";
            ddlStatus.SelectedItem.Value = "0";

            if (GLOBAL.user_id == "foozm")
            {
                divForSalesman.Visible = true;
                divGoodsReceiveRemark.Visible = true;
                divInspectionRemark.Visible = true;
                divInvoiceChkRemark.Visible = true;
                divVerficationRemark.Visible = true;
            }

            //btnListAll.Attributes.Add("style", "background-color:#f58345");
            //f_Button_ListAll();
            btnListAll_Click(sender, e);
        }

        protected void Button_enquiries_section_Click(object sender, EventArgs e)
        {
            new_applicant_section.Attributes.Add("style", "display:none");
            report_section.Visible = false;
            overview_section.Attributes.Add("style", "display:none");
            btnOverview.Attributes.Add("style", "background-color:transparent");
            warranty_section.Visible = false;
            Button_report_section.Attributes.Add("style", "background-color:transparent");
            enquiries_section.Attributes.Add("style", "display:initial");
            Button_enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
        }

        protected void Button_report_section_Click(object sender, EventArgs e)
        {
            JobDaysTaken_section.Attributes.Add("style", "display:none");
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            BatteryQuery_section.Attributes.Add("style", "display:none");
            report_section.Visible = true;
            Button_report_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            new_applicant_section.Attributes.Add("style", "display:none");
            Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:none");
            btnOverview.Attributes.Add("style", "background-color:transparent");
            generalreport_section.Visible = false;
            warranty_section.Visible = false;
        }

        private void Check_DataRequest()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            try
            {
                btnDisplay.Visible = false;
                btnInspectDisplay.Visible = false;
                overview_section.Visible = false;

                bool getWarrantyAccess = WClaim_GET_NewApplicant.getWarrantyApprovalAccess(DynAx, GLOBAL.user_id);
                if (getWarrantyAccess || GLOBAL.user_authority_lvl == 1)
                {//for last approval admin to access only
                    EnableEdit();
                    btnListReject.Visible = true;
                    btnListApproval.Visible = true;
                    divForSalesman.Visible = true;
                    //btnListSalesOrder.Visible = true;
                    GridViewOverviewList.Visible = false;
                    Button_report_section.Visible = true;
                    btnAmend.Visible = true;
                    img2.Visible = true;
                }
                else
                {
                    divForSalesman.Visible = true;
                    Button_report_section.Visible = false;
                    img2.Visible = false;
                }

                string temp1 = GLOBAL.data_passing.ToString();
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        //string test = temp1.Substring(0, 6);
                        if (temp1.Substring(0, 6) == "@WCCM_")//Data receive from CustomerMaster> WC
                        {
                            Label_Account.Text = temp1.Substring(6);
                            validate();
                            labelAccount.Visible = true;
                        }
                        if (temp1.Substring(0, 6) == "@WCBA_")//Data receive from Battery> WC
                        {
                            string result = temp1.Substring(6);// AccNo + "|" + WarrantyType +"|" +DATA //
                            string[] arr_raw_result = result.Split('|');
                            string AccNo = arr_raw_result[1];
                            string Warranty = arr_raw_result[3];
                            string subClaimType = arr_raw_result[4];
                            string SerialNo = arr_raw_result[5];
                            string InvoiceId = arr_raw_result[6];
                            string ItemId = arr_raw_result[7];
                            string ItemName = arr_raw_result[8];
                            string Unit = arr_raw_result[9];
                            string inventLocationId = arr_raw_result[10];
                            // Jerry 20251119 - Add batch number
                            string BatchNumber = arr_raw_result[11];
                            // Jerry 20251119 End
                            Label_Account.Text = AccNo;
                            validate();
                            ddlClaimType.SelectedValue = subClaimType;
                            if (subClaimType == "1") //Battery
                            {
                                rblTransport.SelectedValue = arr_raw_result[2];
                                rblWarranty.SelectedValue = Warranty;
                                //lbltype.Visible = true;
                                //ddlClaimType.Visible = true;
                                divProductType.Visible = true;
                                Label_ItemID.Text = ItemId;
                                Label_BatteryName.Text = ItemName;
                                TextBox_SerialNo.Text = SerialNo;
                                // Jerry 20251119 - Add batch number
                                TextBox_BatchNo.Text = BatchNumber;
                                // Jerry 20251119 End
                                TextBox_CustomerDO.Text = InvoiceId;
                                TextBox_Quantity.Text = "1";
                                TextBox_Quantity.ReadOnly = true;// for battery one serial number only can claim 1 pc
                                string[] arr_raw_Unit = Unit.Split('/');
                                int counter = arr_raw_Unit.Count();

                                DropDownList_Unit.Items.Clear();
                                List<ListItem> items = new List<ListItem>();

                                for (int i = 0; i < counter; i++)
                                {
                                    items.Add(new ListItem(arr_raw_Unit[i]));
                                }
                                DropDownList_Unit.Items.AddRange(items.ToArray());
                            }

                            BatteryButtonChanged();
                            hidden_inventLocationId.Text = inventLocationId;
                            foreach (ListItem item in ddlWarehouse.Items)
                            {
                                if (item.Text == inventLocationId)
                                {
                                    ddlWarehouse.SelectedValue = inventLocationId;
                                    break;
                                }
                            }
                        }
                        if (temp1.Substring(0, 6) == "@WCWC_")//Data receive from WC> WC
                        {
                            string result = temp1.Substring(6);
                            string[] arr_raw_result = result.Split('|');
                            string selected_Id = arr_raw_result[0];
                            string ClaimStatus = arr_raw_result[1];
                            string CustomerAcc = arr_raw_result[2];
                            string ClaimType = arr_raw_result[3];
                            string subClaimType = arr_raw_result[4];
                            string TransRequired = arr_raw_result[5];
                            string warehouse = arr_raw_result[6];
                            if (warehouse.Contains("&amp;"))
                            {
                                warehouse = warehouse.Replace("&amp;", "&");
                            }
                            Label_Account.Text = CustomerAcc;
                            lblAcc.Visible = true;
                            lblClaimNum.Text = selected_Id;
                            lblClaimText.Text = ClaimStatus;
                            validate();
                            hidden_LabelClaimNumber.Text = selected_Id;
                            hidden_Label_PreviousStatus.Text = ClaimStatus;
                            hidden_Label_NextStatus.Text = "";
                            //lbltype.Visible = true;
                            //ddlClaimType.Visible = true;
                            divProductType.Visible = true;
                            ddlClaimType.SelectedValue = subClaimType;

                            string claimType = "";
                            if (ClaimType == "1")
                            {
                                claimType = "Batch Return";
                            }
                            else
                            {
                                claimType = ddlClaimType.SelectedItem.Text;
                            }

                            var getWarrantyApproverUser4 = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, claimType, warehouse); // check for approver
                            var getWarrantyApproverUser5 = WClaim_GET_NewApplicant.getWarrantyApprovalUser2(DynAx, claimType, warehouse);
                            var getWarrantyApproverUser6 = WClaim_GET_NewApplicant.getWarrantyApprovalUser3(DynAx, claimType, warehouse);
                            if (ClaimStatus == "Awaiting Invoice Chk")//Invoice Check
                            {
                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item3 || GLOBAL.user_id == getWarrantyApproverUser6.Item1 ||
                                    GLOBAL.user_id == getWarrantyApproverUser6.Item2 || GLOBAL.user_id == getWarrantyApproverUser6.Item3)
                                {
                                    EnableEdit();
                                    divInvoiceChkRemark.Visible = true;
                                    rblWarranty.Enabled = true;
                                    ddlClaimType.Enabled = true;
                                    ddlWarehouse.Enabled = true;
                                    Button_Revert.Visible = true;
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (ClaimStatus == "Awaiting Transporter")//transporter
                            {
                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item1 || GLOBAL.user_id == getWarrantyApproverUser5.Item1 ||
                                    GLOBAL.user_id == getWarrantyApproverUser5.Item2 || GLOBAL.user_id == getWarrantyApproverUser5.Item3)
                                {
                                    EnableEdit();
                                    divInvoiceChkRemark.Visible = true;
                                    txtInvoiceChkRemark.ReadOnly = true;
                                    rblWarranty.Enabled = false;
                                    ddlClaimType.Enabled = false;
                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    //divCustChop.Attributes.Add("style", "display:block");
                                    //signWarehouse_section.Attributes.Add("style", "display:block");
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (ClaimStatus == "Awaiting GoodsReceive")
                            {
                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item2 || GLOBAL.user_id == getWarrantyApproverUser5.Item4 ||
                                    GLOBAL.user_id == getWarrantyApproverUser5.Item5 || GLOBAL.user_id == getWarrantyApproverUser5.Item6)//GoodsReceive
                                {
                                    EnableEdit();
                                    divGoodsReceiveRemark.Visible = true;

                                    divInvoiceChkRemark.Visible = true;
                                    txtInvoiceChkRemark.ReadOnly = true;

                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    txtTransportationRemark.ReadOnly = true;
                                    rblWarranty.Enabled = false;
                                    ddlClaimType.Enabled = false;
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (ClaimStatus == "Awaiting Inspection")//inspector
                            {
                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item4 || GLOBAL.user_id == getWarrantyApproverUser6.Item4 ||
                                    GLOBAL.user_id == getWarrantyApproverUser6.Item5 || GLOBAL.user_id == getWarrantyApproverUser6.Item6)
                                {
                                    divInvoiceChkRemark.Visible = true;
                                    txtInvoiceChkRemark.ReadOnly = true;

                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    divGoodsReceiveRemark.Visible = true;
                                    txtGoodReceiveRemark.ReadOnly = true;

                                    divInspectionRemark.Visible = true;
                                    Button_Reject.Visible = true;
                                    Button_Submit.Visible = true;
                                    //Neil - Chun Yew - 2025-10-09 - Request Battery not allow to appear create return button before or equal "Awaiting Inspection" status
                                    if (subClaimType != "1")//Battery
                                    {
                                        Button_CreateReturn.Visible = true;
                                    }
                                    else { Button_CreateReturn.Visible = false; }
                                    Button_Revert.Visible = true;
                                    rblWarranty.Enabled = false;
                                    ddlClaimType.Enabled = false;
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (ClaimStatus == "Awaiting Verified")//verifier
                            {
                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item5 || GLOBAL.user_id == getWarrantyApproverUser6.Item7 ||
                                    GLOBAL.user_id == getWarrantyApproverUser6.Rest.Item1 || GLOBAL.user_id == getWarrantyApproverUser6.Rest.Item2)
                                {
                                    divInvoiceChkRemark.Visible = true;
                                    txtInvoiceChkRemark.ReadOnly = true;

                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    txtTransportationRemark.ReadOnly = true;

                                    divGoodsReceiveRemark.Visible = true;
                                    txtGoodReceiveRemark.ReadOnly = true;

                                    divInspectionRemark.Visible = true;
                                    txtInspectionRemark.ReadOnly = true;

                                    divVerficationRemark.Visible = true;
                                    Button_Submit.Visible = true;
                                    Button_Revert.Visible = true;
                                    rblWarranty.Enabled = false;
                                    ddlClaimType.Enabled = false;
                                    if (!WClaim_GET_NewApplicant.isSoCreated(DynAx, lblClaimNum.Text))
                                    {
                                        Button_Reject.Visible = true;
                                    }
                                    //Neil - Chun Yew - 2025-10-09 - Request Battery not allow to appear create return button before or equal "Awaiting Inspection" status
                                    if (subClaimType == "1")//Battery
                                    {Button_CreateReturn.Visible = true;}
                                    else { Button_CreateReturn.Visible = false; }
                                }
                                else
                                {
                                    disableEdit();
                                    Button_Submit.Visible = false;
                                    Button_Reject.Visible = false;
                                }
                            }
                            else if (ClaimStatus == "Awaiting Approved")
                            {
                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item6 || GLOBAL.user_id == getWarrantyApproverUser4.Rest.Item1 ||
                                    GLOBAL.user_id == getWarrantyApproverUser4.Rest.Item2 || GLOBAL.user_id == getWarrantyApproverUser4.Rest.Item3)
                                {
                                    Button_Submit.Visible = false;
                                    Button_Approve.Visible = true;
                                    Button_Revert.Visible = true;
                                    btnAmend.Visible = true;
                                    img2.Visible = true;

                                    divInvoiceChkRemark.Visible = true;
                                    txtInvoiceChkRemark.ReadOnly = true;

                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    txtTransportationRemark.ReadOnly = true;

                                    divGoodsReceiveRemark.Visible = true;
                                    txtGoodReceiveRemark.ReadOnly = true;

                                    divInspectionRemark.Visible = true;
                                    txtInspectionRemark.ReadOnly = true;

                                    divVerficationRemark.Visible = true;
                                    txtVerificationRemark.ReadOnly = true;

                                    divApproverRemark.Visible = true;
                                    rblWarranty.Enabled = false;
                                    ddlClaimType.Enabled = false;
                                    if (!WClaim_GET_NewApplicant.isSoCreated(DynAx, lblClaimNum.Text))
                                    {
                                        Button_Reject.Visible = true;
                                    }
                                }
                                else
                                {
                                    disableEdit();
                                    Button_Submit.Visible = false;
                                    Button_Reject.Visible = false;
                                }
                            }
                            else if (ClaimStatus == "Draft")
                            {
                                EnableEdit();
                                Button_Cancel_Draft.Visible = true;
                            }
                            else if (ClaimStatus == "Approved")
                            {
                                disableEdit();

                                if (GLOBAL.user_id == getWarrantyApproverUser4.Item6 || GLOBAL.user_id == getWarrantyApproverUser4.Rest.Item1 ||
                                    GLOBAL.user_id == getWarrantyApproverUser4.Rest.Item2 || GLOBAL.user_id == getWarrantyApproverUser4.Rest.Item3)
                                {
                                    divInvoiceChkRemark.Visible = true;
                                    txtInvoiceChkRemark.ReadOnly = true;

                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    txtTransportationRemark.ReadOnly = true;

                                    divGoodsReceiveRemark.Visible = true;
                                    txtGoodReceiveRemark.ReadOnly = true;

                                    divInspectionRemark.Visible = true;
                                    txtInspectionRemark.ReadOnly = true;

                                    divVerficationRemark.Visible = true;
                                    txtVerificationRemark.ReadOnly = true;

                                    divApproverRemark.Visible = true;
                                    Button_CreateReturn.Visible = true;
                                }
                                else
                                {
                                    Button_Submit.Visible = false;
                                    Button_Reject.Visible = false;
                                    Button_Approve.Visible = false;
                                    Button_CreateReturn.Visible = false;
                                    divInvoiceChkRemark.Visible = true;
                                    new_applicant_sectionTransportationArrangement.Visible = true;
                                    divGoodsReceiveRemark.Visible = true;
                                    divInspectionRemark.Visible = true;
                                    divVerficationRemark.Visible = true;
                                    divApproverRemark.Visible = true;
                                }
                                btnAmend.Visible = false;
                            }
                            else if (ClaimStatus == "Rejected")
                            {
                                disableEdit();
                                Button_Approve.Visible = false;
                                Button_CreateReturn.Visible = false;
                                btnAmend.Visible = false;
                                Button_Submit.Visible = false;
                                Button_Reject.Visible = false;

                                divInvoiceChkRemark.Visible = true;
                                txtInvoiceChkRemark.ReadOnly = true;

                                new_applicant_sectionTransportationArrangement.Visible = true;
                                txtTransportationRemark.ReadOnly = true;

                                divGoodsReceiveRemark.Visible = true;
                                txtGoodReceiveRemark.ReadOnly = true;

                                divInspectionRemark.Visible = true;
                                txtInspectionRemark.ReadOnly = true;

                                divVerficationRemark.Visible = true;
                                txtVerificationRemark.ReadOnly = true;

                                divApproverRemark.Visible = true;
                            }
                            else
                            {
                                disableEdit();
                            }

                            if (ClaimType == "1")
                            {
                                rblWarranty.SelectedValue = "1";
                                if (subClaimType != "1")
                                {
                                    get_data_load_LF_WarrantyLine_Batch(DynAx, selected_Id, subClaimType);
                                }
                                else
                                {
                                    get_data_load_LF_WarrantyLine(DynAx, selected_Id, subClaimType);
                                }
                            }
                            else//warranty
                            {
                                rblWarranty.SelectedValue = "2";
                                if (subClaimType == "3")
                                {
                                    get_data_load_LF_WarrantyLine_Batch(DynAx, selected_Id, subClaimType);
                                }
                                else
                                {
                                    get_data_load_LF_WarrantyLine(DynAx, selected_Id, subClaimType);
                                }
                            }

                            string TransportationReason = get_data_load_LF_WarrantyTable(DynAx, selected_Id, ClaimType);
                            if (TransRequired == "Yes")
                            {
                                rblTransport.SelectedValue = "1";
                                divTransportArr.Visible = true;
                                txtTransportationRemark.Text = TransportationReason;
                            }
                            else
                            {
                                rblTransport.SelectedValue = "0";
                            }
                            string WarrantyProcessStatus = WClaim_GET_NewApplicant.getWarrantyProcessStatus(DynAx, selected_Id);
                            var split = WarrantyProcessStatus.Split(' ');
                            if (ClaimStatus == "Draft")//to avoid other user submit
                            {
                                if (split[2] != GLOBAL.user_id)
                                {
                                    Button_Submit.Visible = false;
                                    Button_Reject.Visible = false;
                                    btnAmend.Visible = false;
                                }
                            }
                            load_LF_WarrantyBattery(DynAx, selected_Id);
                            lblGetProcessStat.Text = WarrantyProcessStatus;
                            //Chun Yew - Neil - 2025-10-09 - Disable Create Return button if Sales Order created
                            if (lblGetProcessStat.Text.Contains("Sales Order"))
                            {
                                Button_CreateReturn.Visible = false;
                            }
                            divProcessStat.Visible = true;
                            cusButton.Visible = false;
                            if (ClaimStatus == "Awaiting Invoice Chk" || ClaimStatus == "Draft")
                            {

                                if (subClaimType == "1")//Battery
                                { Button_BatteryList.Visible = true; }
                                else { Button_BatteryList.Visible = false; }

                                if (subClaimType == "2") { Button_Oil.Visible = true; }
                                else { Button_Oil.Visible = false; }
                            }
                            else { Button_BatteryList.Visible = false; Button_Oil.Visible = false; }
                            fileUpload.Visible = false;
                            fileUpload2.Visible = false;
                            fileUpload3.Visible = false;
                            fileUpload4.Visible = false;
                            lblimg.Visible = false;
                            lblimg2.Visible = false;
                            lblimg3.Visible = false;
                            lblimg4.Visible = false;
                            getClaimImage();
                            getBatInspectionImage();
                        }
                        if (temp1.Substring(0, 6) == "@RPWC_")//Data receive from WC> WC
                        {
                            string result = temp1.Substring(6);
                            string[] arr_raw_result = result.Split('|');
                            string selected_Id = arr_raw_result[0];
                            string ClaimStatus = arr_raw_result[1];
                            string CustomerAcc = arr_raw_result[2];
                            string ClaimType = arr_raw_result[3];
                            string TransRequired = arr_raw_result[4];
                            Label_Account.Text = CustomerAcc;
                            lblClaimText.Text = ClaimStatus;
                            validate();
                            hidden_LabelClaimNumber.Text = selected_Id;
                            hidden_Label_PreviousStatus.Text = ClaimStatus;
                            hidden_Label_NextStatus.Text = "";

                            if (ClaimStatus == "Approved")
                            {
                                disableEdit();
                                Button_Approve.Visible = false;
                                Button_Submit.Visible = false;
                                Button_Reject.Visible = false;
                            }
                            else if (ClaimStatus == "Awaiting Approved")
                            {
                                disableEdit();
                                Button_Submit.Visible = false;
                                Button_Reject.Visible = false;
                            }
                            else if (ClaimStatus == "Rejected")
                            {
                                disableEdit();
                                Button_Submit.Visible = false;
                                Button_Reject.Visible = false;
                            }

                            if (ClaimType == "Batch")
                            {
                                get_data_load_LF_WarrantyLine_Batch(DynAx, selected_Id, ClaimType);
                            }
                            else
                            {
                                get_data_load_LF_WarrantyLine(DynAx, selected_Id, ClaimType);
                            }
                            load_LF_WarrantyBattery(DynAx, selected_Id);

                            string TransportationReason = get_data_load_LF_WarrantyTable(DynAx, selected_Id, ClaimType);
                            if (TransRequired == "Yes")
                            {
                                rblTransport.SelectedValue = "1";
                            }
                            else
                            {
                                rblTransport.SelectedValue = "0";
                            }
                            Button_CreateReturn.Visible = false;
                            btnAmend.Visible = false;
                            lblDisplay.Visible = true;
                            labelAccount.Visible = true;
                            cusButton.Visible = false;
                            fileUpload.Visible = false;
                            fileUpload2.Visible = false;
                            fileUpload3.Visible = false;
                            fileUpload4.Visible = false;
                            lblimg.Visible = false;
                            lblimg2.Visible = false;
                            lblimg3.Visible = false;
                            lblimg4.Visible = false;
                        }
                        if (temp1.Substring(0, 5) == "@WCO_")
                        {
                            string result = temp1.Substring(5);
                            var split = result.Split('|');

                            Label_Account.Text = split[0];
                            validate();
                            rblTransport.SelectedValue = split[1];
                            rblWarranty.SelectedValue = split[2];
                            //lbltype.Visible = true;
                            //ddlClaimType.Visible = true;
                            divProductType.Visible = true;
                            ddlClaimType.SelectedValue = split[3];
                            Label_ItemID_Item.Text = split[4];
                            Label_ItemName_Item.Text = split[5];
                            AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

                            string itemunit = SFA_GET_SALES_ORDER.get_SLUnit(DynAx, Label_ItemID_Item.Text);//fieldValue=Itemid
                            string itemUnitStr = domComSalesLine.Call("getValidUnits", Label_ItemID_Item.Text).ToString();//fieldValue=Itemid
                            string[] arr_itemUnitStr = itemUnitStr.Split(new char[] { ',', '=' });
                            int counter_arr_itemUnitStr = arr_itemUnitStr.Count();
                            List<string> List_itemUnit = new List<string>();

                            for (int i = 0; i < arr_itemUnitStr.Length; i += 2)
                            {
                                string key = arr_itemUnitStr[i].Trim();

                                List_itemUnit.Add(key);//default unit
                            }

                            foreach (string item in List_itemUnit)
                            {
                                DropDownList_Unit_Items.Items.Add(new ListItem(item));
                            }

                            foreach (ListItem item in ddlWarehouse.Items)
                            {
                                if (item.Text == split[7])
                                {
                                    item.Selected = true;
                                    break;
                                }
                            }
                            BatteryButtonChanged();
                        }
                    }
                    if (GLOBAL.user_id == GLOBAL.AdminID)
                    {
                        divInvoiceChkRemark.Visible = true;

                        new_applicant_sectionTransportationArrangement.Visible = true;
                        divGoodsReceiveRemark.Visible = true;
                        divInspectionRemark.Visible = true;
                        divVerficationRemark.Visible = true;
                        divApproverRemark.Visible = true;

                        Button_Reject.Visible = true;
                        Button_Approve.Visible = true;
                        Button_Revert.Visible = true;
                        Button_CreateReturn.Visible = true;
                    }

                    //string getUserName = WClaim_GET_NewApplicant.CheckUserName(DynAx, GLOBAL.logined_user_name);
                    //lblGetName.Text = Session["user_id"].ToString();
                    //lblGetDate.Text = DateTime.Now.ToString();
                    lblName.Text = Session["user_id"].ToString();
                    labelDate.Text = DateTime.Now.ToString();
                    Session["data_passing"] = "";
                    new_applicant_section.Attributes.Add("style", "display:initial");
                    Button_new_applicant_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    btnOverview.Attributes.Add("style", "background-color:transparent");
                    overview_section.Attributes.Add("style", "display:none");
                    Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
                    enquiries_section.Attributes.Add("style", "display:none");
                    report_section.Visible = false;
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);
                }
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ER_WC_00)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - X++ Exception:{ER_WC_00.Message}");
                //return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ER_WC_00)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - LogonFailedException:{ER_WC_00.Message}");
                //return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (UnauthorizedAccessException ER_WC_00)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - UnauthorizedAccessException:{ER_WC_00.Message}");
                //return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.SessionTerminatedException ER_WC_00)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - Session terminated.:{ER_WC_00.Message}");
                /*DynAx = Function_Method.GlobalAxapta();*/ // Re-establish the session
                //return "";
            }
            catch (Exception ER_WC_00)
            {
                Function_Method.MsgBox("ER_WC_00: " + ER_WC_00.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private void Check_Item_From_SelectItem(string claim)
        {
            string ItemName = ""; string ItemId = ""; string SerialNo = ""; string CustDONo = "";
            string Quantity = ""; string Unit = ""; string ReturnReasonBattery = ""; string ReturnReasonOther = "";
            string OtherReason = "";
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            int LF_WarrantyLine = DynAx.GetTableIdWithLock("LF_WarrantyLine");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            int claimNumber = DynAx.GetFieldId(LF_WarrantyLine, "ClaimNumber");
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimNumber);//Claim Number
            qbr1.Call("value", claim);


            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);

                SerialNo = DynRec1.get_Field("SerialNumber").ToString();
                CustDONo = DynRec1.get_Field("CustDO").ToString();
                Quantity = DynRec1.get_Field("Qty").ToString();
                Unit = DynRec1.get_Field("UnitID").ToString();
                txtBatteryVol.Text = DynRec1.get_Field("BatteryVoltage").ToString();
                ReturnReasonBattery = DynRec1.get_Field("ReturnReasonBattery").ToString();
                ReturnReasonOther = DynRec1.get_Field("ReturnReasonOther").ToString();
                OtherReason = DynRec1.get_Field("OtherReason").ToString();
                DynRec1.Dispose();
            }

            if (SerialNo != "")//battery
            {
                //RadioButtonWarranty1.Checked = true;
                //Label_BatteryName.Text = ItemName;
                //Label_ItemID.Text = ItemId;
                TextBox_SerialNo.Text = SerialNo;
                TextBox_Quantity.Text = Quantity;
                TextBox_CustomerDO.Text = CustDONo;

                //DropDownList_ReasonReturn_Battery.SelectedItem.Text = ReturnReasonBattery;
                if (ReturnReasonBattery == "Others")
                {
                    foreach (ListItem item in DropDownList_ReasonReturn_Battery.Items)
                    {
                        if (item.Text == ReturnReasonBattery)
                        {
                            item.Selected = true;
                            break;
                        }
                    }

                    TextBox_ReasonReturn_Battery_Other.Visible = true;
                    TextBox_ReasonReturn_Battery_Other.Text = OtherReason;
                }
                else
                {
                    foreach (ListItem item in DropDownList_ReasonReturn_Battery.Items)
                    {
                        if (item.Text == ReturnReasonBattery)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
                BatteryButtonChanged();

                if (ItemId != "")
                {
                    string BatteryUnit = WClaim_GET_NewApplicant.get_BatteryUnit(DynAx, ItemId);
                    string[] arr_raw_Unit = BatteryUnit.Split('/');
                    int counter = arr_raw_Unit.Count();

                    DropDownList_Unit.Items.Clear();
                    List<ListItem> items = new List<ListItem>();

                    for (int i = 0; i < counter; i++)
                    {
                        items.Add(new ListItem(arr_raw_Unit[i]));
                    }
                    DropDownList_Unit.Items.AddRange(items.ToArray());
                    DropDownList_Unit.SelectedItem.Text = Unit;
                }

                //if (!string.IsNullOrEmpty(OtherReason))
                //{
                //    TextBox_ReasonReturn_Battery_Other.Visible = true;
                //    TextBox_ReasonReturn_Battery_Other.Text = OtherReason;
                //}
            }
            else //lubricant
            {
                //RadioButtonWarranty2.Checked = true;
                //Label_ItemName_Item.Text = ItemName;
                //Label_ItemID_Item.Text = ItemId;
                TextBox_Quantity_Item.Text = Quantity;
                TextBox_CustomerDO_Item.Text = CustDONo;

                //DropDownList_ReasonReturn_Item.SelectedItem.Text = ReturnReasonOther;
                if (!string.IsNullOrEmpty(OtherReason))
                {
                    TextBox_ReasonReturn_Item.Visible = true;
                    TextBox_ReasonReturn_Item.Text = OtherReason;
                }
                else
                {
                    foreach (ListItem item in DropDownList_ReasonReturn_Item.Items)
                    {
                        if (item.Text == ReturnReasonOther)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
                BatteryButtonChanged();

                if (ItemId != "")
                {
                    string BatteryUnit = WClaim_GET_NewApplicant.get_BatteryUnit(DynAx, ItemId);
                    string[] arr_raw_Unit = BatteryUnit.Split('/');
                    int counter = arr_raw_Unit.Count();

                    DropDownList_Unit.Items.Clear();
                    List<ListItem> items = new List<ListItem>();

                    for (int i = 0; i < counter; i++)
                    {
                        items.Add(new ListItem(arr_raw_Unit[i]));
                    }
                    DropDownList_Unit_Items.Items.AddRange(items.ToArray());
                    DropDownList_Unit_Items.SelectedItem.Text = Unit;
                }
            }

        }

        private string get_data_load_LF_WarrantyTable(Axapta DynAx, string selected_Id, string ClaimType)
        {
            //Jerry 2024-11-04 Avoid hardcode
            //int LF_WarrantyTable = 50773;
            //int LF_WarrantyTable = 30661; //live
            int LF_WarrantyTable = DynAx.GetTableIdWithLock("LF_WarrantyTable");

            string TransportationReason = ""; string InventLocationID = "";
            string ApprovalRemark = ""; string RejectRemark = "";
            string InvoiceRemark = ""; string GoodsReceiveRemark = "";
            string InspectionRemark = ""; string VerifyRemark = "";
            string WarrantlyAddress = "";

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WarrantyTable);

            //Jerry 2024-11-04 Avoid hardcode
            //var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30001);//claimNumber
            int claimNumber = DynAx.GetFieldId(LF_WarrantyTable, "ClaimNumber");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", claimNumber);

            qbr6.Call("value", selected_Id);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WarrantyTable);
                TransportationReason = DynRec6.get_Field("TransportationReason").ToString();
                InventLocationID = DynRec6.get_Field("InventLocationID").ToString();
                ApprovalRemark = DynRec6.get_Field("ApprovalRemark").ToString();
                RejectRemark = DynRec6.get_Field("RejectReason").ToString();
                InvoiceRemark = DynRec6.get_Field("InvoiceChkRemark").ToString();
                GoodsReceiveRemark = DynRec6.get_Field("GoodsRcRemark").ToString();
                InspectionRemark = DynRec6.get_Field("InspectionRemark").ToString();
                VerifyRemark = DynRec6.get_Field("VerifiedRemark").ToString();
                int cnrequired = Convert.ToInt16(DynRec6.get_Field("CnRequired"));
                //19/8/2025 Neil changes: Address must follow the warrantly record stored, as Alt address changed
                WarrantlyAddress = DynRec6.get_Field("Address").ToString();
                Label_Address.Text = WarrantlyAddress;
                ///------------------------
                rblCNrequired.SelectedValue = cnrequired.ToString();
                ddlWarehouse.SelectedItem.Text = InventLocationID;
                ddlWarehouse.SelectedItem.Value = InventLocationID;
                txtGoodReceiveRemark.Text = GoodsReceiveRemark;
                txtInvoiceChkRemark.Text = InvoiceRemark;
                txtInspectionRemark.Text = InspectionRemark;
                txtVerificationRemark.Text = VerifyRemark;
                txtApproverRemark.Text = ApprovalRemark;
                if (RejectRemark != "")
                {
                    divRejectSection.Visible = true;
                    txtRejectedRemark.Text = RejectRemark;
                }
                DynRec6.Dispose();
            }
            return TransportationReason;
        }

        private void get_data_load_LF_WarrantyLine_Batch(Axapta DynAx, string selected_Id, string ClaimType)
        {
            GridView_BatchItem.DataSource = null;
            GridView_BatchItem.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;
            List<ListItem> List_Description = new List<ListItem>();
            List<ListItem> List_ItemID = new List<ListItem>();
            List<ListItem> List_Unit = new List<ListItem>();
            List<ListItem> List_Price = new List<ListItem>();
            List<ListItem> List_Qty = new List<ListItem>();
            List<ListItem> List_CustomerDO = new List<ListItem>();

            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemID", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            //dt.Columns.Add(new DataColumn("Price", typeof(string)));
            dt.Columns.Add(new DataColumn("CustomerDO", typeof(string)));

            // Instantiate your DropDownList control
            DropDownList ddlReasonReturn = new DropDownList();
            string ReturnReasonBatch = "";
            string otherReason = "";
            string returnReasonOther = "";
            //Jerry 2024-11-04 Avoid hardcode
            //int LF_WarrantyLine = 50772;
            //int LF_WarrantyLine = 30660; //live
            int LF_WarrantyLine = DynAx.GetTableIdWithLock("LF_WarrantyLine");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            //Jerry 2024-11-04 Avoid hardcode
            //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//Claim Number
            int claimNumber = DynAx.GetFieldId(LF_WarrantyLine, "ClaimNumber");
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimNumber);//Claim Number
            qbr1.Call("value", selected_Id);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            int count = 0;
            while ((bool)axQueryRun1.Call("next"))
            {
                count = count + 1;
                dr = dt.NewRow();

                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);
                string ItemID = DynRec1.get_Field("ItemID").ToString();
                string New_QTY = DynRec1.get_Field("Qty").ToString();
                string UnitID = DynRec1.get_Field("UnitID").ToString();
                //string Price = DynRec1.get_Field("UnitPrice").ToString();
                string Description = DynRec1.get_Field("Name").ToString();
                string CustomerDO = DynRec1.get_Field("CustDO").ToString();
                string temp_ReturnReasonOther = DynRec1.get_Field("ReturnReasonOther").ToString();
                string temp_ReturnReasonBatch = DynRec1.get_Field("ReturnReasonBatch").ToString();
                otherReason = DynRec1.get_Field("OtherReason").ToString();

                dr["No."] = count;
                dr["Description"] = string.Empty;
                dr["ItemID"] = string.Empty;
                dr["Qty"] = string.Empty;
                dr["Unit"] = string.Empty;
                //dr["Price"] = string.Empty;
                dr["CustomerDO"] = string.Empty;

                List_Qty.Add(new ListItem(New_QTY));
                List_ItemID.Add(new ListItem(ItemID));
                List_Unit.Add(new ListItem(UnitID));
                //List_Price.Add(new ListItem(Price));
                List_Description.Add(new ListItem(Description));
                List_CustomerDO.Add(new ListItem(CustomerDO));

                if (temp_ReturnReasonBatch != "") ReturnReasonBatch = temp_ReturnReasonBatch;

                if (temp_ReturnReasonOther != "") returnReasonOther = temp_ReturnReasonOther;

                dt.Rows.Add(dr);
                DynRec1.Dispose();
            }

            GridView_BatchItem.DataSource = dt;
            GridView_BatchItem.DataBind();

            ViewState["CurrentTable"] = GridView_BatchItem.DataSource;

            for (int i = 0; i < count; i++)
            {
                if (GridView_BatchItem.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox box1 = (TextBox)GridView_BatchItem.Rows[i].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                    Label lblItemID = (Label)GridView_BatchItem.Rows[i].Cells[2].FindControl("lblItemID");
                    TextBox box2 = (TextBox)GridView_BatchItem.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                    RadioButton rbBtl = (RadioButton)GridView_BatchItem.Rows[i].Cells[4].FindControl("rbBtl");
                    RadioButton rbAdd = (RadioButton)GridView_BatchItem.Rows[i].Cells[4].FindControl("rbAdd");
                    //TextBox price = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_Price");
                    TextBox box3 = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

                    box1.Text = List_Description[i].ToString();
                    lblItemID.Text = List_ItemID[i].ToString();
                    box2.Text = List_Qty[i].ToString();
                    if (List_Unit[i].ToString() != "PC" && List_Unit[i].ToString() != "BT")
                    {
                        rbBtl.Visible = false;
                        rbAdd.Checked = true;
                        rbAdd.Text = List_Unit[i].ToString();
                    }
                    else if (List_Unit[i].ToString() == "PC")
                    {
                        rbBtl.Text = List_Unit[i].ToString();
                        rbBtl.Checked = true;
                        rbAdd.Visible = false;
                    }
                    else
                    {
                        rbBtl.Checked = true;
                        rbBtl.Text = List_Unit[i].ToString();
                        rbAdd.Visible = false;
                    }
                    //price.Text = List_Price[i].ToString();
                    box3.Text = List_CustomerDO[i].ToString();
                }
            }

            BatteryButtonChanged();

            if (ReturnReasonBatch != "")
            {
                DropDownList_ReasonReturn_BatchItemDetails.SelectedItem.Text = ReturnReasonBatch;

                switch (ReturnReasonBatch)
                {
                    case "Shop Close":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "1";
                        break;

                    case "Customer Wrong Order":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "2";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    case "Customer Double Order":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "3";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    case "Salesperson Wrong Order Item":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "4";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    case "Salesperson Double Order":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "5";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    case "Sales Admin Wrongly Key In":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "6";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    case "Customer Reject":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "7";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    case "Wrong Item Delivered By Warehouse":
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "8";
                        txtbatchReason.Text = otherReason.ToString();
                        break;

                    default:
                        DropDownList_ReasonReturn_BatchItemDetails.SelectedValue = "99";
                        break;
                }

                rblbatch.Visible = true;
                DropDownList_ReasonReturn_BatchItemDetails_SelectedIndexChanged(ddlReasonReturn, EventArgs.Empty);
                if (ReturnReasonBatch == "Others")
                {
                    rblbatch.Visible = false;
                    txtbatchReason.Visible = true;
                    txtbatchReason.Text = otherReason.ToString();
                }
            }

            if (returnReasonOther != "")
            {
                int desiredIndex = -1;

                for (int i = 0; i < DropDownList_ReasonReturn_OtherProducts.Items.Count; i++)
                {
                    if (DropDownList_ReasonReturn_OtherProducts.Items[i].Text == returnReasonOther)
                    {
                        desiredIndex = i;
                        break;
                    }
                }

                // Set the selected index to trigger the SelectedIndexChanged event
                DropDownList_ReasonReturn_OtherProducts.SelectedIndex = desiredIndex;
                DropDownList_ReasonReturn_OtherProducts_SelectedIndexChanged(ddlReasonReturn, EventArgs.Empty);
            }

            if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "7")
            {
                txtbatchReason.Text = otherReason;
            }

            foreach (ListItem item in rblbatch.Items)
            {
                if (item.Text == otherReason)
                {
                    item.Selected = true;
                    txtbatchReason.Visible = false;
                    break;
                }
            }

            if (!rblbatch.Items.Cast<ListItem>().Any(item => item.Selected))
            {
                rblbatch.SelectedIndex = 0;
                // Handle the case when no item is selected based on your requirements.
            }
        }

        private void get_data_load_LF_WarrantyLine(Axapta DynAx, string selected_Id, string ClaimType)
        {
            string ItemName = ""; string ItemId = ""; string SerialNo = ""; string CustDONo = "";
            string Quantity = ""; string Unit = ""; string ReturnReasonBattery = ""; string ReturnReasonOther = "";
            string OtherReason = "";
            //Jerry 20251119 - Add batch number
            string BatchNumber = "";
            //Jerry 20251119 End

            //Jerry 2024-11-04 Avoid hardcode
            //int LF_WarrantyLine = 50772;
            //int LF_WarrantyLine = 30660; //live
            int LF_WarrantyLine = DynAx.GetTableIdWithLock("LF_WarrantyLine");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            //Jerry 2024-11-04 Avoid hardcode
            //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//Claim Number
            int claimNumber = DynAx.GetFieldId(LF_WarrantyLine, "ClaimNumber");
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimNumber);//Claim Number
            qbr1.Call("value", selected_Id);


            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);
                ItemName = DynRec1.get_Field("Name").ToString();
                ItemId = DynRec1.get_Field("ItemID").ToString();
                SerialNo = DynRec1.get_Field("SerialNumber").ToString();
                CustDONo = DynRec1.get_Field("CustDO").ToString();
                Quantity = DynRec1.get_Field("Qty").ToString();
                Unit = DynRec1.get_Field("UnitID").ToString();
                txtBatteryVol.Text = DynRec1.get_Field("BatteryVoltage").ToString();
                ReturnReasonBattery = DynRec1.get_Field("ReturnReasonBattery").ToString();
                ReturnReasonOther = DynRec1.get_Field("ReturnReasonOther").ToString();
                OtherReason = DynRec1.get_Field("OtherReason").ToString();
                // Jerry 20251119 - Add batch number
                BatchNumber = DynRec1.get_Field("BatchNumber").ToString();
                // Jerry 20251119 End
                DynRec1.Dispose();
            }

            if (ClaimType == "1")//battery
            {
                //RadioButtonWarranty1.Checked = true;
                Label_BatteryName.Text = ItemName;
                Label_ItemID.Text = ItemId;
                TextBox_SerialNo.Text = SerialNo;
                // Jerry 20251119 - Add batch number
                TextBox_BatchNo.Text = BatchNumber;
                // Jerry 20251119 End
                TextBox_Quantity.Text = Quantity;
                TextBox_CustomerDO.Text = CustDONo;

                //DropDownList_ReasonReturn_Battery.SelectedItem.Text = ReturnReasonBattery;
                if (ReturnReasonBattery == "Others")
                {
                    foreach (ListItem item in DropDownList_ReasonReturn_Battery.Items)
                    {
                        if (item.Text == ReturnReasonBattery)
                        {
                            item.Selected = true;
                            break;
                        }
                    }

                    TextBox_ReasonReturn_Battery_Other.Visible = true;
                    TextBox_ReasonReturn_Battery_Other.Text = OtherReason;
                }
                else
                {
                    foreach (ListItem item in DropDownList_ReasonReturn_Battery.Items)
                    {
                        if (item.Text == ReturnReasonBattery)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
                BatteryButtonChanged();

                if (ItemId != "")
                {
                    string BatteryUnit = WClaim_GET_NewApplicant.get_BatteryUnit(DynAx, ItemId);
                    string[] arr_raw_Unit = BatteryUnit.Split('/');
                    int counter = arr_raw_Unit.Count();

                    DropDownList_Unit.Items.Clear();
                    List<ListItem> items = new List<ListItem>();

                    for (int i = 0; i < counter; i++)
                    {
                        items.Add(new ListItem(arr_raw_Unit[i]));
                    }
                    DropDownList_Unit.Items.AddRange(items.ToArray());
                    DropDownList_Unit.SelectedItem.Text = Unit;
                }

                //if (!string.IsNullOrEmpty(OtherReason))
                //{
                //    TextBox_ReasonReturn_Battery_Other.Visible = true;
                //    TextBox_ReasonReturn_Battery_Other.Text = OtherReason;
                //}
            }
            else if (ClaimType == "2")//lubricant
            {
                //RadioButtonWarranty2.Checked = true;
                Label_ItemName_Item.Text = ItemName;
                Label_ItemID_Item.Text = ItemId;
                TextBox_Quantity_Item.Text = Quantity;
                TextBox_CustomerDO_Item.Text = CustDONo;

                //DropDownList_ReasonReturn_Item.SelectedItem.Text = ReturnReasonOther;
                if (!string.IsNullOrEmpty(OtherReason))
                {
                    TextBox_ReasonReturn_Item.Visible = true;
                    TextBox_ReasonReturn_Item.Text = OtherReason;
                }
                else
                {
                    foreach (ListItem item in DropDownList_ReasonReturn_Item.Items)
                    {
                        if (item.Text == ReturnReasonOther)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
                BatteryButtonChanged();

                if (ItemId != "")
                {
                    string BatteryUnit = WClaim_GET_NewApplicant.get_BatteryUnit(DynAx, ItemId);
                    string[] arr_raw_Unit = BatteryUnit.Split('/');
                    int counter = arr_raw_Unit.Count();

                    DropDownList_Unit.Items.Clear();
                    List<ListItem> items = new List<ListItem>();

                    for (int i = 0; i < counter; i++)
                    {
                        items.Add(new ListItem(arr_raw_Unit[i]));
                    }
                    DropDownList_Unit_Items.Items.AddRange(items.ToArray());
                    DropDownList_Unit_Items.SelectedItem.Text = Unit;
                }
            }
        }

        private void Dropdown_Transporter_Receiver(Axapta DynAx, string ClaimType)
        {
            string TransportArr1 = ""; string TransportArr2 = ""; string TransportArr3 = ""; string TransportArr4 = "";
            string GoodsRec1 = ""; string GoodsRec2 = ""; string GoodsRec3 = ""; string GoodsRec4 = "";

            var tuple_getWarrantyApprovalGroup = WClaim_GET_NewApplicant.getWarrantyApprovalGroup(DynAx, ClaimType, "");
            string TransportArr = tuple_getWarrantyApprovalGroup.Item1;
            string GoodsRec = tuple_getWarrantyApprovalGroup.Item2;
            string[] arr_TransportArr = TransportArr.Split('|');
            string[] arr_GoodsRec = GoodsRec.Split('|');
            if (TransportArr != "")
            {
                TransportArr1 = arr_TransportArr[0];
                TransportArr2 = arr_TransportArr[1];
                TransportArr3 = arr_TransportArr[2];
                TransportArr4 = arr_TransportArr[3];
            }

            if (GoodsRec != "")
            {
                GoodsRec1 = arr_GoodsRec[0];
                GoodsRec2 = arr_GoodsRec[1];
                GoodsRec3 = arr_GoodsRec[2];
                GoodsRec4 = arr_GoodsRec[3];
            }

            if (TransportArr1 != "") TransportArr1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr1);
            if (TransportArr2 != "") TransportArr1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr2);
            if (TransportArr3 != "") TransportArr1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr3);
            if (TransportArr4 != "") TransportArr1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr4);

            if (GoodsRec1 != "") GoodsRec1 = WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec1);
            if (GoodsRec2 != "") GoodsRec2 = WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec2);
            if (GoodsRec3 != "") GoodsRec3 = WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec3);
            if (GoodsRec4 != "") GoodsRec4 = WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec4);

            //DropDownList_TransportationCoordinator.Items.Clear();
            List<ListItem> items_TransportationCoordinator = new List<ListItem>();
            items_TransportationCoordinator.Add(new ListItem("-- SELECT --", ""));
            if (TransportArr1 != "") items_TransportationCoordinator.Add(new ListItem(TransportArr1, "1"));
            if (TransportArr2 != "") items_TransportationCoordinator.Add(new ListItem(TransportArr2, "2"));
            if (TransportArr3 != "") items_TransportationCoordinator.Add(new ListItem(TransportArr3, "3"));
            if (TransportArr4 != "") items_TransportationCoordinator.Add(new ListItem(TransportArr4, "4"));
            //DropDownList_TransportationCoordinator.Items.AddRange(items_TransportationCoordinator.ToArray());

            //DropDownList_GoodReceivePerson.Items.Clear();
            List<ListItem> items_GoodReceivePerson = new List<ListItem>();
            items_GoodReceivePerson.Add(new ListItem("-- SELECT --", ""));
            if (GoodsRec1 != "") items_GoodReceivePerson.Add(new ListItem(GoodsRec1, "1"));
            if (GoodsRec2 != "") items_GoodReceivePerson.Add(new ListItem(GoodsRec2, "2"));
            if (GoodsRec3 != "") items_GoodReceivePerson.Add(new ListItem(GoodsRec3, "3"));
            if (GoodsRec4 != "") items_GoodReceivePerson.Add(new ListItem(GoodsRec4, "4"));
            //DropDownList_GoodReceivePerson.Items.AddRange(items_GoodReceivePerson.ToArray());
        }

        protected void Button_Approve_Click(object sender, EventArgs e)
        {
            string PreviousStatus = hidden_Label_PreviousStatus.Text;
            string ClaimStatus = "";
            string WarrantyProcessStatus = "";
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                ClaimStatus = "Approved";

                Submit_Process();

                string SalesmanEmail = SalesReport_Get_Budget.getEmail(DynAx, hdSalesmanId.Value);
                if (SalesmanEmail.StartsWith("@"))//head office, no validate user email
                {
                    SalesmanEmail = Session["user_id"].ToString() + "@posim.com.my";
                }
                var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, hdSalesmanId.Value);
                string HODemail = SalesReport_Get_Budget.getEmail(DynAx, tuple_GroupId_ReportingTo.Item2);

                getDetailsAndSendemail(PreviousStatus, WarrantyProcessStatus, ClaimStatus, SalesmanEmail + "," + HODemail, txtApproverRemark.Text);

                clear_parameter_NewApplication_Customer();
                clear_parameter_NewApplication_BatteryDetails();
                clear_parameter_NewApplication_ItemDetails();
                clear_parameter_NewApplication_BatchItemDetails();
                clear_parameter_NewApplication_TransportationArrangement();

                Button back = new Button();

                btnOverview_Click(back, EventArgs.Empty);
                btnListApproval_Click(back, EventArgs.Empty);
            }
            catch (Exception ER_WC_10)
            {
                Function_Method.UserLog(ER_WC_10.ToString());
                Function_Method.MsgBox("ER_WC_10: " + ER_WC_10.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            GLOBAL.user_id = Session["user_id"].ToString();

            string ClaimNumber = hidden_LabelClaimNumber.Text;
            string PreviousStatus = hidden_Label_PreviousStatus.Text;

            string ClaimStatus = "Rejected";
            string Process_Status = "";
            string WarrantyProcessStatus = "";
            string RejectReason = hfRejectReason.Value;

            var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, hdSalesmanId.Value);
            string HODemail = SalesReport_Get_Budget.getEmail(DynAx, tuple_GroupId_ReportingTo.Item2);
            if (ClaimNumber != "")
            {
                try
                {
                    string SalesmanEmail = SalesReport_Get_Budget.getEmail(DynAx, hdSalesmanId.Value);
                    if (SalesmanEmail.StartsWith("@"))//head office, no validate user email
                    {
                        SalesmanEmail = Session["user_id"].ToString() + "@posim.com.my";
                    }
                    WarrantyProcessStatus = WClaim_GET_NewApplicant.getWarrantyProcessStatus(DynAx, ClaimNumber);
                    if (PreviousStatus == "Awaiting Transporter")
                    {
                        Process_Status = WarrantyProcessStatus + "Transporter reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Awaiting GoodsReceive")
                    {
                        Process_Status = WarrantyProcessStatus + "Goods reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Awaiting Invoice Chk")
                    {
                        Process_Status = WarrantyProcessStatus + "Invoice reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Awaiting Inspection")
                    {
                        Process_Status = WarrantyProcessStatus + "Verification reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Awaiting Verified")
                    {
                        Process_Status = WarrantyProcessStatus + "Inspection reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Awaiting Approved")
                    {
                        Process_Status = WarrantyProcessStatus + "Verified reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Approved")
                    {
                        Process_Status = WarrantyProcessStatus + "Reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    Function_Method.UserLog(Process_Status);
                    logger.Info($"Claim {ClaimNumber} rejected. Process Status: {Process_Status}");
                    getDetailsAndSendemail(PreviousStatus, Process_Status, ClaimStatus, SalesmanEmail + "," + HODemail + ",tancy1@posim.com.my", "\nReject Reason: " + RejectReason);

                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyTable"))
                    {
                        DynAx.TTSBegin();

                        if (ClaimNumber != "")
                        {
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", ClaimNumber));
                            if (DynRec.Found)
                            {
                                //DynRec.set_Field("InventLocationId", "HO");

                                if (RejectReason != "") DynRec.set_Field("RejectReason", RejectReason);
                                if (ClaimStatus != "") DynRec.set_Field("ClaimStatus", ClaimStatus);
                                if (Process_Status != "") DynRec.set_Field("ProcessStatus", Process_Status);

                                DynRec.Call("Update");
                                logger.Info($"Claim {ClaimNumber} updated with status: {ClaimStatus} and process status: {Process_Status}");
                            }
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                    Function_Method.MsgBox("Claim Number: " + ClaimNumber + " have been rejected.", this.Page, this);

                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('btnOverview'); ", true);//go to overview
                }
                catch (Exception ER_WC_15)
                {
                    logger.Error($"Error rejecting claim {ClaimNumber}: {ER_WC_15}");
                    Function_Method.MsgBox("ER_WC_15: " + ER_WC_15.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Dispose();
                }
            }
        }

        protected void Button_SaveDraft_Click(object sender, EventArgs e)
        {
            hidden_Label_NextStatus.Text = "Draft";//save as draft
            submit();
        }

        //Jerry 2024-11-05 use unique_key in place of claim_number if claim_number not yet generated
        //protected void uploadPic()
        protected void uploadPic(string unique_key)
        {
            HttpFileCollection files = Request.Files;
            string filename = "";
            string key = hidden_LabelClaimNumber.Text;

            // Jerry 2024-11-05 If no claim number, save with unique_key. replace unique key later when claim number is generated
            if (String.IsNullOrEmpty(key))
            {
                key = unique_key;
            }
            // Jerry 2024-11-05 end

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    if (file.ContentLength > 0)
                    {
                        //string path = @"e:/Warranty/" + Label_Salesman.Text + "/" + Label_CustName.Text;
                        string path = @"e:/Warranty/" + Label_Salesman.Text + "/" + Label_CustAcc.Text; 
                        if (!Directory.Exists(@"e:/Warranty/" + Label_Salesman.Text))
                        {
                            Directory.CreateDirectory(@"e:/Warranty/" + Label_Salesman.Text);
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                        }

                        filename = file.FileName;

                        // Jerry 2024-12-23 Sanitize file name
                        filename = Function_Method.SanitizeFilename(filename);
                        // Jerry 2024-12-23 Sanitize file name - END

                        string filePath = (path + "/" + filename);
                        Function_Method.UserLog(filePath);

                        Function_Method.UserLog(path);

                        logger.Info("File path: {0}", filePath); //For N Log
                        logger.Info("Directory path: {0}", path);

                        if (Path.GetExtension(filename).ToLower() == ".pdf")
                        {
                            file.SaveAs(filePath);
                            logger.Info("PDF file saved: {0}", filePath);
                        }
                        else
                        {
                            Function_Method.ImgCompress(file, filePath);//compress image and save into E drive
                            logger.Info("{0} image uploaded.", hidden_LabelClaimNumber.Text);
                            Function_Method.UserLog(hidden_LabelClaimNumber.Text + " image uploaded.");
                        }

                        using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                        {
                            string query = "insert into warranty_pic_tbl(claim_num, salesman, customer_name, pictures) values (@c1, @s1, @c2, @p1)";
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            //Jerry 2024-11-05 use unique_key
                            //cmd.Parameters.AddWithValue("@c1", hidden_LabelClaimNumber.Text);
                            cmd.Parameters.AddWithValue("@c1", key);
                            cmd.Parameters.AddWithValue("@s1", Label_Salesman.Text);
                            cmd.Parameters.AddWithValue("@c2", Label_CustName.Text);
                            cmd.Parameters.AddWithValue("@p1", filePath);
                            logger.Info("Database record inserted for {0}", filePath);
                            Function_Method.UserLog(hidden_LabelClaimNumber.Text + " filepath: " + filePath);

                            conn.Open();
                            cmd.ExecuteNonQuery();

                            conn.Close();
                            cmd.Parameters.Clear();
                        }
                        
                    }
                    else
                    {
                        Function_Method.UserLog(filename);
                        logger.Warn("Empty file received: {0}", filename);
                    }
                    upload_section.Visible = false;
                }

                if (string.IsNullOrEmpty(filename))
                {
                    Function_Method.MsgBox("Upload Image failed, please resubmit again. ", this.Page, this);
                }
            }
            catch (Exception ER_WC_22)
            {
                logger.Error(ER_WC_22, "Upload error");
                Function_Method.MsgBox("Upload error: " + ER_WC_22.ToString(), this.Page, this);
                Function_Method.UserLog("Upload error: " + ER_WC_22.ToString());
            }
        }

        // Jerry 2024-11-05 update unique_key with claim_number
        protected void updatePic(string unique_key, string claim_number)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string query = "update warranty_pic_tbl set claim_num = @c1 where claim_num = @c2; ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@c1", claim_number);
                    cmd.Parameters.AddWithValue("@c2", unique_key);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ER_WC_22)
            {
                Function_Method.MsgBox("Upload error: " + ER_WC_22.ToString(), this.Page, this);
                Function_Method.UserLog("Upload error: " + ER_WC_22.ToString());
            }
        }

        protected string uploadInspectImg()
        {
            string fileLocation = "";
            string fullPath = "";
            if (InspectFile.PostedFile != null && InspectFile.PostedFile.ContentLength > 0)
            {
                HttpPostedFile postedFile = InspectFile.PostedFile;
                string fileName = Path.GetFileName(InspectFile.PostedFile.FileName);
                
                // Jerry 2024-12-23 Sanitize file name
                fileName = Function_Method.SanitizeFilename(fileName);
                // Jerry 2024-12-23 Sanitize file name - END

                fileLocation = @"e:/Warranty/Battery/" + Label_CustAcc.Text;
                fullPath = fileLocation + "/" + fileName;

                if (!Directory.Exists(fileLocation))
                {
                    Directory.CreateDirectory(fileLocation);
                }

                //postedFile.SaveAs(fullPath);

                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string query = "insert into warranty_bat_tbl(claim_num, cust_acc, imgpath) values (@c1, @s1, @p1)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    conn.Open();

                    cmd.Parameters.AddWithValue("@c1", hidden_LabelClaimNumber.Text);
                    cmd.Parameters.AddWithValue("@s1", Label_CustAcc.Text);
                    cmd.Parameters.AddWithValue("@p1", fullPath);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();
                }

                Function_Method.ImgCompress(postedFile, fullPath);//compress image and save into E drive
            }
            return fullPath;
        }

        protected void Button_CreateReturn_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            string ClaimNumber = hidden_LabelClaimNumber.Text;
            string PreviousStatus = hidden_Label_PreviousStatus.Text;
            string WarrantyProcessStatus = ""; string ClaimStatus = "";
            string Process_Status = ""; string CreateSalesDate = "";

            WarrantyProcessStatus = WClaim_GET_NewApplicant.getWarrantyProcessStatus(DynAx, ClaimNumber);
            //ClaimStatus = "Sales Order";
            Process_Status = WarrantyProcessStatus + "Sales order created by " + Session["user_id"].ToString() + " on " + DateTime.Now + "<br/>";
            CreateSalesDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var todayDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (!WClaim_GET_NewApplicant.isSoCreated(DynAx, ClaimNumber))
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyTable"))
                {
                    DynAx.TTSBegin();

                    if (hidden_LabelClaimNumber.Text != "")
                    {
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", hidden_LabelClaimNumber.Text));
                        if (DynRec.Found)
                        {
                            //if (ClaimStatus != "") DynRec.set_Field("ClaimStatus", ClaimStatus);
                            if (Process_Status != "") DynRec.set_Field("ProcessStatus", Process_Status);
                            if (CreateSalesDate != "") DynRec.set_Field("SOCreatedDate", CreateSalesDate);
                            DynRec.Call("Update");
                            logger.Info($"Claim {ClaimNumber} updated with sales date created: {CreateSalesDate} and process status: {Process_Status}");
                        }
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }

                AxaptaObject axSalesLine = DynAx.CreateAxaptaObject("AxSalesLine");

                if (Label_Account.Text == "")
                {
                    Function_Method.MsgBox("There is no data.", this.Page, this);
                }

                try
                {
                    string cust_Name = SFA_GET_SALES_HEADER.get_customer_acc(DynAx, Label_Account.Text);

                    if (Label_CustName.Text == cust_Name)
                    {
                        AxaptaObject axSalesTable = DynAx.CreateAxaptaObject("AxSalesTable");
                        axSalesTable.Call("parmDeliveryName", cust_Name);
                        axSalesTable.Call("parmDeliveryStreet", Label_Address.Text);
                        axSalesTable.Call("parmCustAccount", Label_Account.Text);
                        axSalesTable.Call("parmInvoiceAccount", Label_Account.Text);
                        axSalesTable.Call("parmCurrencyCode", "MYR");
                        axSalesTable.Call("parmDocumentStatus", 5);//packing slip
                        axSalesTable.Call("parmReturnStatus", 2);//open
                        axSalesTable.Call("parmSalesStatus", 2);//Delivered
                        axSalesTable.Call("parmReturnReasonCodeId", "GoodsRtn");
                        axSalesTable.Call("SuffixCode", "/RT");
                        axSalesTable.Call("parmCustGroup", "TDE");
                        axSalesTable.Call("parmReturnDeadLine", todayDate);
                        axSalesTable.Call("parmCustomerRef", hidden_LabelClaimNumber.Text);
                        axSalesTable.Call("parmSalesType", 4);// return item
                        axSalesTable.Call("save");
                        var salesID = axSalesTable.Call("parmSalesID");
                        hdSoNumber.Value = salesID.ToString();

                        logger.Info($"Sales order created for Claim {ClaimNumber} with Sales ID: {salesID} updated SalesStatus : 2 = Delivered, ReturnStatus: 2 = Open");

                        if (ddlClaimType.SelectedValue == "1")//battery
                        {
                            if (PreProcess_Warranty_SaveSalesOrder(Label_ItemID.Text))
                            {
                                SaveSalesLine(axSalesLine, Label_ItemID.Text);
                                axSalesLine.Call("parmSalesId", salesID.ToString());
                                axSalesLine.Call("parmItemId", Label_ItemID.Text);
                                axSalesLine.Call("parmName", Label_BatteryName.Text);
                                axSalesLine.Call("SuffixCode", "/RT");
                                axSalesLine.Call("parmSalesUnit", DropDownList_Unit.SelectedItem.Text);
                                axSalesLine.Call("parmExpectedRetQty", "-" + TextBox_Quantity.Text);
                                axSalesLine.Call("parmSalesQty", "-" + TextBox_Quantity.Text);
                                axSalesLine.Call("parmShippingDateRequested", todayDate);
                                axSalesLine.Call("parmSalesStatus", 2); //1= Backorder //2 = Delivered
                                axSalesLine.Call("parmReturnStatus", 2);//1= Awaiting //2 = Registered //4 = Received
                                axSalesLine.Call("LF_SerialNumber", TextBox_SerialNo.Text);
                                axSalesLine.Call("parmReturnDeadLine", todayDate);
                                axSalesLine.Call("CNInvId", TextBox_CustomerDO.Text);//for draft cn purposes
                                axSalesLine.Call("dosave");
                            }
                        }
                        //lubricant=============================================================
                        else if (ddlClaimType.SelectedValue == "2")
                        {
                            if (PreProcess_Warranty_SaveSalesOrder(Label_ItemID_Item.Text))
                            {
                                SaveSalesLine(axSalesLine, Label_ItemName_Item.Text);
                                axSalesLine.Call("parmSalesId", salesID.ToString());
                                axSalesLine.Call("parmItemId", Label_ItemID_Item.Text);
                                axSalesLine.Call("parmName", Label_ItemName_Item.Text);
                                axSalesLine.Call("SuffixCode", "/RT");
                                axSalesLine.Call("parmSalesUnit", DropDownList_Unit_Items.SelectedItem.Text);
                                axSalesLine.Call("parmExpectedRetQty", "-" + TextBox_Quantity_Item.Text);
                                axSalesLine.Call("parmSalesQty", "-" + TextBox_Quantity_Item.Text);
                                axSalesLine.Call("parmShippingDateRequested", todayDate);
                                axSalesLine.Call("parmSalesStatus", 2); //1= Backorder //2 = Delivered
                                axSalesLine.Call("parmReturnStatus", 2);//1= Awaiting //2 = Registered //4 = Received
                                //axSalesLine.Call("LF_SerialNumber", TextBox_SerialNo.Text);
                                axSalesLine.Call("parmReturnDeadLine", todayDate);
                                axSalesLine.Call("CNInvId", TextBox_CustomerDO_Item.Text);//for draft cn purposes
                                axSalesLine.Call("dosave");
                            }
                        }
                        //batch return & warranty other products=============================================================
                        else
                        {
                            var tuple_get_Batch_Item = get_Batch_Item();//get item id
                            //string[] Description_Batch = tuple_get_Batch_Item.Item1;
                            //string[] New_QTY_Batch = tuple_get_Batch_Item.Item2;
                            string[] custDo = tuple_get_Batch_Item.Item4;
                            string[] ItemId_Batch = tuple_get_Batch_Item.Item5;
                            string[] SalesUnit = tuple_get_Batch_Item.Item7;
                            TextBox_CustomerDO.Text = custDo[0];
                            foreach (GridViewRow row in GridView_BatchItem.Rows)
                            {
                                TextBox box1 = (TextBox)GridView_BatchItem.Rows[row.RowIndex].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                                string itemName = box1.Text;
                                string temp_ItemId = WClaim_GET_NewApplicant.ItemNameGetItemId(DynAx, itemName);

                                TextBox qty = (TextBox)GridView_BatchItem.Rows[row.RowIndex].Cells[3].FindControl("TextBox_New_QTY");
                                //TextBox price = (TextBox)GridView_BatchItem.Rows[row.RowIndex].Cells[5].FindControl("TextBox_Price");
                                int itemQty = Convert.ToInt16("-" + qty.Text);
                                //double itemPrice = Convert.ToDouble(price.Text);
                                if (PreProcess_Warranty_SaveSalesOrder(temp_ItemId))
                                {
                                    string itemID = ItemId_Batch[row.RowIndex].ToString();
                                    string _CustAccount = Label_Account.Text;
                                    string _SalesUnit = SalesUnit[row.RowIndex];

                                    var tuple_getMPP_MPG = SFA_GET_SALES_ORDER_2.get_MPP_MPG(DynAx, salesID.ToString());
                                    string LF_MixPricePromo = tuple_getMPP_MPG.Item1;
                                    string LF_MixPriceGroup = tuple_getMPP_MPG.Item2;

                                    string axTaxItemGroup = SFA_GET_SALES_ORDER_2.get_TaxItemGroup(DynAx, itemID);
                                    string LFUOM = SFA_GET_SALES_ORDER_2.get_UOM(DynAx, _SalesUnit);

                                    AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");
                                    string itemPrice = domComSalesLine.Call("getSalesPrice", itemID, _SalesUnit, todayDate, 1, Label_Account.Text, salesID).ToString();//fieldValue=Itemid

                                    if (itemQty != 0)//New Qty
                                    {
                                        AxaptaObject axBatchSalesLine = DynAx.CreateAxaptaObject("AxSalesLine");
                                        axBatchSalesLine.Call("InitSalesLine", salesID.ToString());

                                        SaveSalesLine(axBatchSalesLine, itemID);
                                        axBatchSalesLine.Call("parmReceiptDateRequested", todayDate);
                                        axBatchSalesLine.Call("parmShippingDateRequested", todayDate);
                                        axBatchSalesLine.Call("parmReturnDeadLine", todayDate);
                                        axBatchSalesLine.Call("parmConfirmedDlv", todayDate);
                                        axBatchSalesLine.Call("parmItemId", itemID);
                                        axBatchSalesLine.Call("parmName", itemName);
                                        axBatchSalesLine.Call("parmSalesUnit", _SalesUnit);
                                        axBatchSalesLine.Call("parmSalesQty", itemQty);
                                        axBatchSalesLine.Call("parmSalesPrice", itemPrice);
                                        axBatchSalesLine.Call("suffixCode", "/RT");
                                        axBatchSalesLine.Call("parmExpectedRetQty", itemQty);
                                        axBatchSalesLine.Call("parmSalesId", salesID.ToString());
                                        axBatchSalesLine.Call("parmSalesStatus", 2);//1= Backorder //2 = Delivered
                                        axBatchSalesLine.Call("parmReturnStatus", 2);//1= Awaiting //2= Registered //4 = Received
                                        axBatchSalesLine.Call("parmMixPriceGroup", LF_MixPriceGroup);
                                        axBatchSalesLine.Call("parmUOM", LFUOM);
                                        axBatchSalesLine.Call("parmMixPricePromo", LF_MixPricePromo);
                                        axBatchSalesLine.Call("CNInvId", custDo[row.RowIndex]);//for draft cn purposes
                                        //SalesStatus -None 0 -Backorder 1 - Delivered 2 - Invoiced 3 - Canceled 4 - Partial Delivered 5
                                        //ReturnStatus - None 0 - Awaiting 1 - Registered 2 - Quarantine 3 - Received 4 - Invoiced 5 - Canceled 6
                                        logger.Info($"Sales ID: {salesID} for {itemName} updated SalesStatus : 2 = Delivered, ReturnStatus : 2 = Registered");
                                        if (axTaxItemGroup == "OS")
                                        {
                                            axBatchSalesLine.Call("parmTaxGroup", axTaxItemGroup);
                                        }
                                        if (hdInventId.Value != "")
                                        {
                                            axBatchSalesLine.Call("parmInventDimId", hdInventId.Value);
                                        }

                                        string poCost = ""; string manualCost = "";
                                        if (GLOBAL.user_company == "PBM")
                                        {
                                            if (poCost != "")
                                            {
                                                axBatchSalesLine.Call("POCost", poCost);
                                            }
                                            else
                                            {
                                                axBatchSalesLine.Call("POCost", 0);
                                            }
                                            if (manualCost != "")
                                            {
                                                axBatchSalesLine.Call("manualCost", manualCost);
                                            }
                                            else
                                            {
                                                axBatchSalesLine.Call("manualCost", 0);
                                            }
                                        }
                                        axBatchSalesLine.Call("dosave");
                                        axBatchSalesLine.Call("CurrentRecord");
                                        //axBatchSalesLine.Call("CurrentRecordId");//comment for temporary
                                    }
                                }
                            }
                        }

                        //update warranty table
                        var rmaID = WClaim_GET_NewApplicant.get_RMAid(DynAx, salesID.ToString());
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyTable"))
                        {
                            DynAx.TTSBegin();

                            if (hidden_LabelClaimNumber.Text != "")
                            {
                                DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", hidden_LabelClaimNumber.Text));
                                if (DynRec.Found)
                                {
                                    DynRec.set_Field("RMAID", rmaID.Item1);
                                    DynRec.set_Field("RMADate", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                    DynRec.Call("Update");
                                    logger.Info($"Warranty table updated for Claim {ClaimNumber} with RMA ID: {rmaID.Item1}");
                                }
                            }
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                    }
                    Function_Method.MsgBox("Warranty: " + ClaimNumber + ". Sales order created.", this.Page, this);
                    logger.Info($"Sales order created for Warranty: {ClaimNumber}.");

                    Button toApproval = new Button();
                    // Call the SelectedIndexChanged event handler method

                    btnOverview_Click(toApproval, EventArgs.Empty);
                    btnListInspection_Click(toApproval, EventArgs.Empty);
                }
                catch (Exception ER_WC_20)
                {
                    logger.Error($"Error creating return for Claim {ClaimNumber}: {ER_WC_20}");
                    Function_Method.MsgBox("ER_WC_20: " + ER_WC_20.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Dispose();
                }
            }
            else
            {
                Function_Method.MsgBox("Warranty for " + ClaimNumber + " found in sales order.\nPlease check.", this.Page, this);
            }
        }

        protected void SaveSalesLine(AxaptaObject axSalesLine, string ItemID)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            try
            {
                string inventDim = string.Empty;
                AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

                inventDim = ddlWarehouse.SelectedItem.Text;
                string inventDimId = WClaim_GET_NewApplicant.getInventDimID_InventDimLocation(DynAx, inventDim); 
                string inventDimId_old = domComSalesLine.Call("getInventDimId", ItemID, "", "", "", inventDim, "").ToString();

                if (hdInventId.Value != "")  //Makesure PreProcess_Warranty_SaveSalesOrder already get the Invent ID
                {
                    if (hdInventId.Value != inventDimId)
                    {
                        inventDimId = hdInventId.Value;
                    }
                }

                if (inventDimId != "")
                {
                    axSalesLine.Call("parmInventDimId", inventDimId);
                }
                axSalesLine.Call("suffixCode", "");
                axSalesLine.Call("parmCurrencyCode", "MYR");
            }
            catch (Exception ER_WC_16)
            {
                Function_Method.MsgBox("ER_WC_16: " + ER_WC_16.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void disableEdit()
        {
            if (GLOBAL.user_id != GLOBAL.AdminID)
            {
                Button_Submit.Visible = false;
                Button_Reject.Visible = false;
                Label_Account.Enabled = false;
                ddlWarehouse.Enabled = false;
                rblWarranty.Enabled = false;
                //lbltype.Visible = false;
                ddlClaimType.Enabled = false;
                rblbatch.Enabled = false;
                TextBox_CustomerDO_Item.Enabled = false;
                TextBox_ReasonReturn_Item.Enabled = false;

                //battery&others
                Button_BatteryList.Enabled = false;
                TextBox_SerialNo.Enabled = false;
                TextBox_CustomerDO.Enabled = false;
                TextBox_CustomerDO_Item.Enabled = false;
                TextBox_Quantity.Enabled = false;
                TextBox_Quantity_Item.Enabled = false;
                DropDownList_ReasonReturn_Battery.Enabled = false;
                DropDownList_ReasonReturn_OtherProducts.Enabled = false;
                DropDownList_Unit.Enabled = false;
                DropDownList_Unit_Items.Enabled = false;
                DropDownList_ReasonReturn_Item.Enabled = false;
                TextBox_ReasonReturn_Battery_Other.Enabled = false;

                //batch item
                DropDownList_ReasonReturn_BatchItemDetails.Enabled = false;
                Button_AddBatchItem.Enabled = false;
                GridView_BatchItem.Enabled = false;

                rblTransport.Enabled = false;
                //DropDownList_TransportationCoordinator.Enabled = false;
                //DropDownList_GoodReceivePerson.Enabled = false;
                Button_AddBatchItem.Enabled = false;
                Button_CustomerMasterList.Enabled = false;
                txtbatchReason.Enabled = false;

                new_applicant_sectionTransportationArrangement.Visible = false;
            }
        }

        protected void EnableEdit()
        {
            Button_Submit.Visible = true;
            Label_Account.Enabled = true;
            ddlWarehouse.Enabled = true;
            rblWarranty.Enabled = true;
            rblbatch.Enabled = true;
            TextBox_CustomerDO_Item.Enabled = true;
            TextBox_ReasonReturn_Item.Enabled = true;

            //battery&Others
            Button_BatteryList.Enabled = true;
            TextBox_SerialNo.ReadOnly = false;
            TextBox_CustomerDO_Item.ReadOnly = false;
            TextBox_Quantity.ReadOnly = false;
            TextBox_Quantity_Item.ReadOnly = false;
            DropDownList_ReasonReturn_Battery.Enabled = true;
            DropDownList_Unit.Enabled = true;
            DropDownList_Unit_Items.Enabled = true;
            DropDownList_ReasonReturn_Item.Enabled = true;
            TextBox_ReasonReturn_Battery_Other.Enabled = true;

            //batch item
            DropDownList_ReasonReturn_BatchItemDetails.Enabled = true;
            Button_AddBatchItem.Enabled = true;
            GridView_BatchItem.Enabled = true;
            rblTransport.Enabled = true;
            //DropDownList_TransportationCoordinator.Enabled = true;
            //DropDownList_GoodReceivePerson.Enabled = true;
            Button_AddBatchItem.Enabled = true;
            Button_CustomerMasterList.Enabled = true;
            txtbatchReason.Enabled = true;

            Accordion_TransportationArrangement.Visible = true;
            new_applicant_sectionTransportationArrangement.Visible = true;
            divForApproval.Visible = true;
        }

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            string saleStatus = "";
            var validateSalesStatus = WClaim_GET_NewApplicant.get_RMAid(DynAx, hdSoNumber.Value);
            if (validateSalesStatus.Item2.ToString() == "3")
            {
                EnableEdit();
                hidden_Label_PreviousStatus.Text = "";
                lblClaimText.Text = "Draft";
                btnAmend.Visible = false;
            }
            else
            {
                if (validateSalesStatus.Item2 == "3")
                {
                    saleStatus = "Invoiced";
                }
                else if (validateSalesStatus.Item2 == "4")
                {
                    saleStatus = "Canceled";
                }
                else if (validateSalesStatus.Item2 == "5")
                {
                    saleStatus = "Partial delivered";
                }
                Function_Method.MsgBox("Failed to amend due to sales status: " + saleStatus, this.Page, this);
                //0 none //1 backorder //2 delivered //3 invoiced //4 canceled //5 partialdelivered
            }
            DynAx.Dispose();
        }

        protected void rbAdd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton select = (RadioButton)sender;
            GridViewRow row = (GridViewRow)select.Parent.Parent;
            int index = row.RowIndex;

            foreach (GridViewRow rw in GridView_BatchItem.Rows)
            {
                RadioButton radioBtl = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbBtl");
                RadioButton radioAdd = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbAdd");
                if (select.Checked)
                {
                    radioAdd.Checked = true;
                    radioBtl.Checked = false;
                }
                else
                {
                    radioBtl.Checked = true;
                    radioAdd.Checked = false;
                }
            }
        }

        protected void rbBtl_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton select = (RadioButton)sender;
            GridViewRow row = (GridViewRow)select.Parent.Parent;
            int index = row.RowIndex;

            foreach (GridViewRow rw in GridView_BatchItem.Rows)
            {
                RadioButton radioBtl = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbBtl");
                RadioButton radioAdd = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbAdd");
                if (select.Checked)
                {
                    radioBtl.Checked = true;
                    radioAdd.Checked = false;
                }
                else
                {
                    radioAdd.Checked = true;
                    radioBtl.Checked = false;
                }
            }
        }

        protected void DropDownList_ReasonReturn_BatchItemDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblbatch.Items.Clear();
            txtbatchReason.Visible = false;
            rblbatch.Visible = true;
            upload_section.Visible = true;

            if (DropDownList_ReasonReturn_BatchItemDetails.SelectedItem.Value == "0")
            {
                rblbatch.Visible = false;
                upload_section.Visible = false;
                txtbatchReason.Visible = false;
            }
            else if (DropDownList_ReasonReturn_BatchItemDetails.SelectedItem.Value != "1")
            {
                rblbatch.Visible = false;
                txtbatchReason.Visible = true;
            }
        }

        protected void DropDownList_ReasonReturn_OtherProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblbatch.Items.Clear();
            txtbatchReason.Visible = false;
            rblbatch.Visible = true;
            upload_section.Visible = true;

            if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "0")
            {
                rblbatch.Visible = false;
                upload_section.Visible = false;
            }
            else if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "1")
            {
                rblbatch.Items.Add("Bulging");
                rblbatch.Items.Add("Groove Cracked");
                rblbatch.Items.Add("Uneven Wear");
                rblbatch.Items.Add("Ply Seperation");
                rblbatch.Items.Add("Open Splice");
                rblbatch.Items.Add("Side Cracked");
                rblbatch.Items.Add("Exposed Bead Wire");
                rblbatch.Items.Add("Air Leak");
            }
            else if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "2")
            {
                rblbatch.Items.Add("Shrieking Sound");
                rblbatch.Items.Add("Chattering");
                rblbatch.Items.Add("Line Defect");
                rblbatch.Items.Add("Smearing");
                rblbatch.Items.Add("Juddering");
            }
            else if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "3")
            {
                rblbatch.Items.Add("Carbon Fouling");
                rblbatch.Items.Add("Cracked Shell");
                rblbatch.Items.Add("Oil Fouling");
                rblbatch.Items.Add("Short Circuit");
                rblbatch.Items.Add("Loss Terminal");
                rblbatch.Items.Add("Ash Deposits");
            }
            else if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "4")
            {
                rblbatch.Items.Add("No Pin Lock Provided Inside");
                rblbatch.Items.Add("Broken Joint/Splice");
                rblbatch.Items.Add("Uneven Chain");
            }
            else if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "5")
            {
                rblbatch.Items.Add("Linner Seperation");
                rblbatch.Items.Add("Burned");
                rblbatch.Items.Add("Shrieking Sound");
                rblbatch.Items.Add("Uneven Contact Area/Wear");
            }
            else if (DropDownList_ReasonReturn_OtherProducts.SelectedItem.Value == "6")
            {
                rblbatch.Items.Add("O-ring Problem");
            }
            else
            {
                rblbatch.Visible = false;
                txtbatchReason.Visible = true;
            }
        }

        protected void rblWarranty_SelectedIndexChanged(object sender, EventArgs e)
        {
            BatteryButtonChanged();

            string selectedDate = SelectedDateHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                // Set the datepicker's value to the selected date
                txtCollectionDt.Text = selectedDate;
            }
            else
            {
                string collectionDate = Request.Form[txtCollectionDt.UniqueID];

                txtCollectionDt.Text = collectionDate;
            }
        }

        private void DropDownList()
        {
            try
            {
                List<ListItem> warehouse = new List<ListItem>();
                warehouse = WClaim_GET_NewApplicant.getWarrantyWarehouse();
                ddlWarehouse.Items.AddRange(warehouse.ToArray());
            }

            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ER_WC_DropDownList)
            {
                logger.Error($"Error in DropDownList - X++ Exception:{ER_WC_DropDownList.Message}");
                //return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ER_WC_DropDownList)
            {
                logger.Error($"Error in DropDownList - LogonFailedException:{ER_WC_DropDownList.Message}");
                //return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (UnauthorizedAccessException ER_WC_DropDownList)
            {
                logger.Error($"Error in DropDownList - UnauthorizedAccessException:{ER_WC_DropDownList.Message}");
                //return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.SessionTerminatedException ER_WC_DropDownList)
            {
                logger.Error($"Error in DropDownList - Session terminated.:{ER_WC_DropDownList.Message}");
                /*DynAx = Function_Method.GlobalAxapta();*/ // Re-establish the session
                //return "";
            }
            catch (Exception ER_WC_DropDownList)
            {
                Function_Method.MsgBox("ER_WC_DropDownList: " + ER_WC_DropDownList.ToString(), this.Page, this);
            }
        }

        protected void rblTransport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTransport.SelectedValue == "1")
            {
                Accordion_TransportationArrangement.Visible = true;
                new_applicant_sectionTransportationArrangement.Visible = true;
            }
            else
            {
                Accordion_TransportationArrangement.Visible = false;
                new_applicant_sectionTransportationArrangement.Visible = false;
            }
        }
        protected void ActionButtons_Click(object sender, EventArgs e)
        {
            List<string> s = new List<string>();
            if (sender as Button != null)
            {
                s = (sender as Button).Text.Split('(').ToList();
            }
            else
            {
                s.Add(sender.ToString());
            }
            transparentButtonAttribute();
            switch (s[0].Trim())
            {
                case "Overview":
                case "Pending":
                    btnListAll.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "0";

                    break;
                case "Draft":
                    btnListDraft.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "1";

                    break;
                case "Approved":
                    btnListApproved.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "2";

                    break;
                case "Rejected":
                    btnListReject.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "3";

                    break;
                case "Approval":
                    btnListApproval.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "4";

                    break;
                case "Transporter":
                    btnListTransporter.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "5";

                    break;
                case "Goods Receive":
                    btnListGoodsReceived.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "6";

                    break;
                case "Inspection":
                    btnListInspection.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "7";

                    break;
                case "Invoice Check":
                    btnListInvoiceChk.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "8";

                    break;
                case "Verification":
                    btnListVerify.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "9";

                    break;
                case "Sales Order":
                    //btnListSalesOrder.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "10";

                    break;

            }
            f_Button_ListAll();
        }
        protected void transparentButtonAttribute()
        {
            //03/03/25 KX If any extra action button, just add inside

            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            imgbtnExport.Visible = false;
        }
        protected void btnListDraft_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#f58345");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "1";
            GridViewOverviewList.Visible = true;
            f_Button_ListAll();
        }

        protected void btnListInvoiceChk_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#f58345");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "8";
            f_Button_ListAll();
        }

        protected void btnListTransporter_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#f58345");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "5";
            f_Button_ListAll();
        }

        protected void btnListVerify_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#f58345");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "9";
            f_Button_ListAll();
        }

        protected void btnListGoodsReceived_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#f58345");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "6";
            f_Button_ListAll();
        }

        protected void btnListInspection_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#f58345");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "7";
            f_Button_ListAll();
        }

        protected void btnListApproval_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#f58345");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "4";
            f_Button_ListAll();
        }

        protected void btnListApproved_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#f58345");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "2";
            f_Button_ListAll();
        }

        protected void btnListReject_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#f58345");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "3";
            f_Button_ListAll();
        }

        protected void btnListAll_Click(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#f58345");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "0";
            f_Button_ListAll();
        }

        protected void btnListSalesOrder_Click(object sender, EventArgs e)
        {
            //btnListSalesOrder.Attributes.Add("style", "background-color:#f58345");
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "10";
            f_Button_ListAll();
        }

        protected void txtboxListAll_TextChanged(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListInvoiceChk.Attributes.Add("style", "background-color:#transparent");
            btnListTransporter.Attributes.Add("style", "background-color:#transparent");
            btnListVerify.Attributes.Add("style", "background-color:#transparent");
            btnListGoodsReceived.Attributes.Add("style", "background-color:#transparent");
            btnListInspection.Attributes.Add("style", "background-color:#transparent");
            btnListApproval.Attributes.Add("style", "background-color:#transparent");
            WClaim_Search(0, "");
        }

        protected void lnkBtn1_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = hdFilePath.Value + "/" + lnkBtn1.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('DownloadFile.aspx','_newtab')", true);//_blank
        }

        protected void lnkBtn2_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = hdFilePath.Value + "/" + lnkBtn2.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('DownloadFile.aspx','_newtab')", true);//_blank
        }

        protected void lnkBtn3_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = hdFilePath.Value + "/" + lnkBtn3.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('DownloadFile.aspx','_newtab')", true);//_blank
        }

        protected void lnkBtn4_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = hdFilePath.Value + "/" + lnkBtn4.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('DownloadFile.aspx','_newtab')", true);//_blank
        }

        private void load_LF_WarrantyBattery(Axapta DynAx, string claimID)
        {
            //Jerry 2024-11-05 Avoid hardcode
            //int LF_WarrantyBattery = 30691; //live
            int LF_WarrantyBattery = DynAx.GetTableIdWithLock("LF_WarrantyBattery");

            string Cca = ""; string LoadTest = "";
            string TestResult = ""; string MagicEye = "";
            string ImgPath = "";

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WarrantyBattery);

            //Jerry 2024-11-05 Avoid hardcode
            //var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30001);//claimNumber
            int cn1 = DynAx.GetFieldId(LF_WarrantyBattery, "ClaimNumber");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

            qbr6.Call("value", claimID);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WarrantyBattery);
                Cca = DynRec6.get_Field("CCA").ToString();
                LoadTest = DynRec6.get_Field("LoadTest").ToString();
                TestResult = DynRec6.get_Field("TestResult").ToString();
                MagicEye = DynRec6.get_Field("MagicEye").ToString();
                ImgPath = DynRec6.get_Field("ImgPath").ToString();

                ddlLoadTest.SelectedValue = LoadTest;
                ddlTestResult.SelectedValue = TestResult;
                ddlMagicEye.Text = MagicEye;
                txtCca.Text = Cca;
                DynRec6.Dispose();
            }
        }

        private void Alt_Addr_function()
        {
            Btn_Delivery_Addr.Visible = false;
            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";

            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                var tuple_get_AltAddress = SFA_GET_SALES_HEADER.get_AltAddress(DynAx, Label_CustAcc.Text);
                if (tuple_get_AltAddress == null)
                {
                    return;
                }
                string[] AltAddress = tuple_get_AltAddress.Item1;
                string[] AltAddressRecId = tuple_get_AltAddress.Item2;
                string[] DefaultAddress = tuple_get_AltAddress.Item4;
                int Counter = tuple_get_AltAddress.Item3;

                if (Counter == 1)//only one data
                {
                    if (AltAddress[0] == Label_Address.Text)//same as primary, no alt address
                    {
                        return;
                    }
                    Btn_Delivery_Addr.Visible = false;
                }
                int temp_count = 0;
                for (int i = 0; i < Counter; i++)
                {
                    if (AltAddress[i] == hidden_Street.Text)
                    {
                        //skip
                    }
                    else
                    {
                        if (temp_count == 0)//first count
                        {
                            hidden_alt_address_RecId.Text = AltAddressRecId[i];
                            hidden_alt_address.Text = AltAddress[i];
                        }
                        else
                        {
                            hidden_alt_address_RecId.Text = hidden_alt_address_RecId.Text + "|" + AltAddressRecId[i];
                            hidden_alt_address.Text = hidden_alt_address.Text + "|" + AltAddress[i];
                        }
                        temp_count = temp_count + 1;
                    }
                }

                hidden_alt_address_counter.Text = temp_count.ToString();
                Btn_Delivery_Addr.Visible = true;
            }
            catch (Exception ER_WC_12)
            {
                Function_Method.MsgBox("ER_WC_12: " + ER_WC_12.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void CheckBox_Changed2(object sender, EventArgs e)
        {
            int counter = 0;
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            foreach (GridViewRow row in GridView_AltAddress.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox_c = (row.Cells[0].FindControl("chkRow2") as CheckBox);

                    if (CheckBox_c.Checked)
                    {
                        string selected_address = row.Cells[1].Text;

                        Label_Address.Text = selected_address;

                        //need to update hidden field

                        string temp_RecID_List = hidden_alt_address_RecId.Text;
                        string[] arr_temp_RecID_List = temp_RecID_List.Split('|');
                        var tuple_get_AltAddress_info = SFA_GET_SALES_HEADER.get_AltAddress_info(DynAx, arr_temp_RecID_List[counter]);

                        #region Update Alternate Address Straight when select Checkbox - Renny 2025-08-19
                        if (lblClaimText.Text == "Awaiting Transporter" || lblClaimText.Text == "Awaiting Invoice Chk" || lblClaimText.Text == "Draft")
                        {
                            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyTable"))
                            {
                                DynAx.TTSBegin();

                                string ClaimNumber = hidden_LabelClaimNumber.Text;

                                if (ClaimNumber != "")
                                {
                                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", ClaimNumber));
                                    if (DynRec.Found)
                                    {
                                        //DynRec.set_Field("InventLocationId", "HO");

                                        if (selected_address != "") DynRec.set_Field("Address", selected_address);

                                        DynRec.Call("Update");
                                        logger.Info($"Claim {ClaimNumber} updated with Address: {selected_address}. Modified by {GLOBAL.logined_user_name}");
                                    }
                                }
                                DynAx.TTSCommit();
                                DynAx.TTSAbort();
                            }
                        }
                        else
                        {
                            Function_Method.MsgBox("Address changes are not permitted after the transporter has approved the warranty claim.", this.Page, this);
                        }
                        #endregion
                        DynAx.Logoff();

                        GridView_AltAddress.Visible = false;
                        Btn_Delivery_Addr.Text = "Alt. Addr.";
                        //row.BackColor = Color.FromName("#ff8000");
                    }
                }
                counter = counter + 1;
            }
        }

        protected void Btn_Delivery_Addr_Click(object sender, EventArgs e)
        {
            GridView_AltAddress.Visible = true;

            Btn_Delivery_Addr.Text = "Hide Alt. Addr.";

            if (Label_CustAcc.Text == "")
            {
                Function_Method.MsgBox("There is no customer account number.", this.Page, this);
                Btn_Delivery_Addr.Text = "Alt. Addr.";
                return;
            }

            int Counter = 0;
            //Add to Grid, GridView_AltAddress
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Alternate Address") });

            if (hidden_alt_address_counter.Text != "")
            {
                Counter = Convert.ToInt32(hidden_alt_address_counter.Text);

                string[] arr_alt_address = hidden_alt_address.Text.Split('|');
                for (int i = 0; i < Counter; i++)
                {
                    dt.Rows.Add(arr_alt_address[i]);
                }

                GridView_AltAddress.DataSource = dt;
                GridView_AltAddress.DataBind();
            }

        }

        #region report 
        protected void btnJobTaken_Click(object sender, EventArgs e)
        {
            btnJobTaken.Attributes.Add("style", "background-color:#f58345");
            btnProductionChargingCode.Attributes.Add("style", "background-color:transparent");
            btnBatteryQueryReport.Attributes.Add("style", "background-color:transparent");
            btnBatteryStatisticReport.Attributes.Add("style", "background-color:transparent");

            btnTitle.Text = "Job Days Taken";
            lblWarrantyType.Visible = true;
            ddlWarranty.Visible = true;
            btnGenReport.Visible = true;
            generalreport_section.Visible = true;
            warranty_section.Visible = true;
            dateRange_section.Visible = true;

            BatteryQuery_section.Attributes.Add("style", "display:none");
            BatteryStatistic_section.Attributes.Add("style", "display:none");
            QueryReport_section.Attributes.Add("style", "display:none");
        }

        protected void btnProductionChargingCode_Click(object sender, EventArgs e)
        {
            btnProductionChargingCode.Attributes.Add("style", "background-color:#f58345");
            btnJobTaken.Attributes.Add("style", "background-color:transparent");
            btnBatteryQueryReport.Attributes.Add("style", "background-color:transparent");
            btnBatteryStatisticReport.Attributes.Add("style", "background-color:transparent");

            btnTitle.Text = "Production/Charging Code";
            lblWarrantyType.Visible = false;
            ddlWarranty.Visible = false;
            btnGenReport.Visible = false;
            JobDaysTaken_section.Attributes.Add("style", "display:none");
            BatteryQuery_section.Attributes.Add("style", "display:none");
            BatteryStatistic_section.Attributes.Add("style", "display:none");
            dateRange_section.Attributes.Add("style", "display:none");

            generalreport_section.Attributes.Add("style", "display:initial");

            btnGenReport_Click(null, null);
        }

        protected void btnBatteryQueryReport_Click(object sender, EventArgs e)
        {
            btnBatteryQueryReport.Attributes.Add("style", "background-color:#f58345");
            btnProductionChargingCode.Attributes.Add("style", "background-color:transparent");
            btnJobTaken.Attributes.Add("style", "background-color:transparent");
            btnBatteryStatisticReport.Attributes.Add("style", "background-color:transparent");

            btnTitle.Text = "Battery Query Report";
            lblWarrantyType.Visible = false;
            ddlWarranty.Visible = false;
            JobDaysTaken_section.Attributes.Add("style", "display:none");
            BatteryStatistic_section.Attributes.Add("style", "display:none");
            QueryReport_section.Attributes.Add("style", "display:none");

            btnGenReport.Visible = true;
            generalreport_section.Attributes.Add("style", "display:initial");
            dateRange_section.Attributes.Add("style", "display:initial");
        }

        protected void btnBatteryStatisticReport_Click(object sender, EventArgs e)
        {
            btnBatteryStatisticReport.Attributes.Add("style", "background-color:#f58345");
            btnProductionChargingCode.Attributes.Add("style", "background-color:transparent");
            btnJobTaken.Attributes.Add("style", "background-color:transparent");
            btnBatteryQueryReport.Attributes.Add("style", "background-color:transparent");

            btnTitle.Text = "Battery Statistic Report";
            lblWarrantyType.Visible = false;
            ddlWarranty.Visible = false;
            dateRange_section.Visible = true;

            JobDaysTaken_section.Attributes.Add("style", "display:none");
            BatteryQuery_section.Attributes.Add("style", "display:none");
            QueryReport_section.Attributes.Add("style", "display:none");

            btnGenReport.Visible = true;
            generalreport_section.Attributes.Add("style", "display:initial");
            dateRange_section.Attributes.Add("style", "display:initial");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            if (btnTitle.Text == "Job Days Taken")
            {
                JobDaysTaken_section.Attributes.Add("style", "display:initial");
                JobDaysTakenReport(0);
                export.Visible = true;
            }
            else if (btnTitle.Text == "Production/Charging Code")
            {
                QueryReport_section.Attributes.Add("style", "display:initial");
                export.Visible = false;
                queryReport_Overview();
            }
            else if (btnTitle.Text == "Battery Query Report")
            {
                BatteryQuery_section.Attributes.Add("style", "display:initial");
                BatteryQueryReport();
                export.Visible = true;
            }
            else
            {
                BatteryStatistic_section.Attributes.Add("style", "display:initial");
                BatteryStatisticReport(0);
                export.Visible = true;
            }
        }

        protected void gvJobsTaken_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvJobsTaken.PageIndex = e.NewPageIndex;
            gvJobsTaken.DataBind();
        }
        #endregion
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected void Button_Revert_Click(object sender, EventArgs e)
        {
            hidden_Label_NextStatus.Text = "Revert";
            submit();
        }

        protected void Button_BatteryInfo_Click(object sender, EventArgs e)
        {
            string claimNo = Server.UrlEncode(lblClaimNum.Text);
            string url = $"Warranty/WClaim_BatteryList.aspx?ClaimNo={claimNo}";
            string features = "menubar=1,width=800,height=600";
            string script = $"window.open('{url}', '_blank', '{features}');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", script, true);
            GlobalHelper.RedirectToNewTab(url, "_blank", features);
        }

        protected void Button_ItemChanges_Click(object sender, EventArgs e)
        {

            try
            {
                string ClaimStatus = lblClaimText.Text;
                string ClaimType = "";
                int subClaimType = 0;//1: Battery 2.Oil 3.Other Products
                string CustAccount = Label_Account.Text;
                string ClaimNumber = lblClaimNum.Text;
                string SerialNumber = TextBox_SerialNo.Text;// only battery have
                string CustDO = TextBox_CustomerDO.Text; int Qty = 0; string Unit = "";
                string ItemID = ""; string ItemName = ""; string ReturnReasonBattery = "";
                string ReturnReasonOther = ""; string ReturnReasonBatch = "";
                string OtherReason = ""; string Price = "";
                bool CheckWarrantyLine = false;
                if (TextBox_Quantity.Text != "") Qty = Convert.ToInt32(TextBox_Quantity.Text);
                if (DropDownList_Unit.Text != "") Unit = DropDownList_Unit.SelectedItem.Text;

                if (rblWarranty.SelectedValue == "1")//batch
                {
                    ClaimType = "Batch";

                    CheckWarrantyLine = ProcessBatchItemReturn(ReturnReasonBatch, OtherReason, CheckWarrantyLine, ClaimType, CustAccount,
                                         ClaimStatus, ClaimNumber, SerialNumber, ReturnReasonBattery, ReturnReasonOther);
                }
                else
                {

                    ClaimType = "Warranty";
                    if (Accordion_BatteryDetails.Visible == true)
                    {
                        subClaimType = 1;//battery

                        SerialNumber = TextBox_SerialNo.Text;// only battery have
                        CustDO = TextBox_CustomerDO.Text;

                        if (TextBox_Quantity.Text != "") Qty = Convert.ToInt32(TextBox_Quantity.Text);
                        if (DropDownList_Unit.Text != "") Unit = DropDownList_Unit.SelectedItem.Text;
                        ItemID = Label_ItemID.Text;
                        ItemName = Label_BatteryName.Text;
                        if (DropDownList_ReasonReturn_Battery.SelectedValue != "0")
                        {
                            ReturnReasonBattery = DropDownList_ReasonReturn_Battery.SelectedItem.ToString();
                            if (DropDownList_ReasonReturn_Battery.SelectedItem.Text == "Others")
                            {
                                OtherReason = TextBox_ReasonReturn_Battery_Other.Text;
                            }
                        }
                        CheckWarrantyLine = Save_LF_WarrantyLine(ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                        CustDO, Qty, Unit, ItemID, ItemName, ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, Price);
                    }
                    else if (Accordion_ItemDetails.Visible == true)
                    {
                        subClaimType = 2;//lubricant
                        if (TextBox_Quantity_Item.Text != "") Qty = Convert.ToInt32(TextBox_Quantity_Item.Text);
                        if (DropDownList_Unit_Items.Text != "") Unit = DropDownList_Unit_Items.SelectedItem.Text;
                        ItemID = Label_ItemID_Item.Text;
                        ItemName = Label_ItemName_Item.Text;
                        CustDO = TextBox_CustomerDO_Item.Text;
                        //Price = TextBox_LubricantPrice.Text;
                        if (DropDownList_ReasonReturn_Item.SelectedValue != "0")
                        {
                            ReturnReasonOther = DropDownList_ReasonReturn_Item.SelectedItem.ToString();
                            if (DropDownList_ReasonReturn_Item.SelectedItem.Text == "Others")
                            {
                                OtherReason = TextBox_ReasonReturn_Item.Text;
                            }
                        }
                        CheckWarrantyLine = Save_LF_WarrantyLine(ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                            CustDO, Qty, Unit, ItemID, ItemName, ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, Price);

                    }
                    else if (Accordion_BatchItemDetails.Visible == true)
                    {
                        subClaimType = 3;
                        CheckWarrantyLine = ProcessBatchItemReturn(ReturnReasonBatch, OtherReason, CheckWarrantyLine, ClaimType, CustAccount,
                                            ClaimStatus, ClaimNumber, SerialNumber, ReturnReasonBattery, ReturnReasonOther);
                    }

                }

                Function_Method.UserLog(GLOBAL.user_id + " warranty line " + ClaimNumber + ItemName + " ID: " + ItemID + " " +
                                        SerialNumber + " " + ReturnReasonBattery + OtherReason);
                Function_Method.MsgBox("Save changes successfull to" + ItemName + ". ", this.Page, this);
                if (CheckWarrantyLine == false)
                {
                    Function_Method.MsgBox("Fail to create warranty line. Please create again.", this.Page, this);
                    //return false;
                }
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Fail to save. Button_ItemChanges_Click" + ex, Page, this);
                //return false;
            }

        }
        protected void Button_Cancel_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                string ClaimNumber = lblClaimNum.Text;
                string WarrantyProcessStatus = "";
                string Process_Status = "";

                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyTable"))
                {
                    DynAx.TTSBegin();

                    if (ClaimNumber != "")
                    {
                        WarrantyProcessStatus = WClaim_GET_NewApplicant.getWarrantyProcessStatus(DynAx, ClaimNumber);

                        Process_Status = WarrantyProcessStatus + "Cancel by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";


                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", ClaimNumber));
                        if (DynRec.Found)
                        {
                            //DynRec.set_Field("InventLocationId", "HO");

                            //if (RejectReason != "") DynRec.set_Field("RejectReason", RejectReason);
                            DynRec.set_Field("ClaimStatus", "Cancel");
                            if (Process_Status != "") DynRec.set_Field("ProcessStatus", Process_Status);

                            DynRec.Call("Update");
                            logger.Info($"Claim {ClaimNumber} updated with status: Cancel. ");
                        }
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
                Button back = new Button();
                btnOverview_Click(back, EventArgs.Empty);
                ActionButtons_Click(back.Text = "Draft", EventArgs.Empty);
            }
            catch (Exception ER_WC_Cancel)
            {
                Function_Method.UserLog(ER_WC_Cancel.ToString());
                Function_Method.MsgBox("ER_WC_Cancel: " + ER_WC_Cancel.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }
    }
}