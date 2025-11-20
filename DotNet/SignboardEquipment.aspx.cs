using DotNet.SFAModel;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using iText.StyledXmlParser.Jsoup.Nodes;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using NLog;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cms;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;

using Control = System.Web.UI.Control;

namespace DotNet
{
    public partial class SignboardEquipment : System.Web.UI.Page
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

                //clear_parameter_NewApplication_Customer();
                //clear_parameter_NewApplication_BatteryDetails();
                //clear_parameter_NewApplication_ItemDetails();
                //clear_parameter_NewApplication_BatchItemDetails();
                //clear_parameter_NewApplication_TransportationArrangement();
                //DropDownList();

                Session["data_passing"] = "";//in case forget reset
                Session["flag_temp"] = 0;

                LoadIssueToDropDown();
                Check_DataRequest();
                Button_Submit.Visible = false;

                if (hidden_Label_PreviousStatus.Text == "")
                {
                    //Button_SaveDraft.Visible = true;
                    Button_Submit.Visible = true;
                    Button_Reject.Visible = false;
                    Button_Approve.Visible = false;
                    Button_Proceed.Visible = false;
                    display_section.Visible = false;
                }
                else
                {
                    //Button_SaveDraft.Visible = false;
                    divProcessStat.Visible = true;
                    //Button_Submit.Text = "Proceed";
                }
                if (hidden_LabelEquipID.Text == "")
                {
                    btnUpload.Visible = false;
                }
            }

            labelAccount.Visible = false;
            //ClientScript.RegisterStartupScript(GetType(), "BackButtonScript", "<script type=\"text/javascript\">history.pushState(null, null, location.href);window.onpopstate = function () {history.go(1);};</script>");
            //avoid user click back button
        }

        private void LoadIssueToDropDown(bool forEditing = false, string currentEditingValue = "")
        {
            // Clear existing items
            ddlIssueTo.Items.Clear();

            // Add default item
            ddlIssueTo.Items.Add(new ListItem("-- N/A --", "N/A"));

            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();

                    // Base query for active items (Status = 1)
                    string SQL = "SELECT IssueTo FROM signboardequipment_issueto WHERE Status = 1";

                    // If editing, include the specific record being edited
                    if (forEditing && !string.IsNullOrEmpty(currentEditingValue))
                    {
                        SQL = $@"SELECT IssueTo FROM signboardequipment_issueto 
                        WHERE Status = 1 
                        UNION
                        SELECT IssueTo FROM signboardequipment_issueto 
                        WHERE IssueTo = @currentValue";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(SQL, conn))
                    {
                        if (forEditing && !string.IsNullOrEmpty(currentEditingValue))
                        {
                            cmd.Parameters.AddWithValue("@currentValue", currentEditingValue);
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    string issueToValue = reader.GetString("IssueTo");
                                    ddlIssueTo.Items.Add(new ListItem(issueToValue, issueToValue));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Error loading dropdown: " + ex.Message, this.Page, this);
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
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void Button_new_applicant_section_Click(object sender, EventArgs e)
        {
            //btnAmend.Visible = false;
            button_section.Visible = true;
            if (string.IsNullOrEmpty(Label_Account.Text))
            {
                Button_Submit.Visible = false;
                //Button_SaveDraft.Visible = false;
                Button_CustomerMasterList.Enabled = true;
            }
            else
            {//to postback 
             //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('WClaimTag2'); ", true);
                Response.Redirect("SignboardEquipment.aspx", true);
                return;
            }

            cusButton.Visible = true;
            Button_Approve.Visible = false;
            overview_section.Visible = false;
            enquiries_section.Visible = false;
            Button_Reject.Visible = false;
            new_applicant_sectionDeliveryDetails.Visible = false;
            divCommentFromSalesPerson.Visible = false;
            new_applicant_section.Attributes.Add("style", "display:initial");
            Button_new_applicant_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            btnOverview.Attributes.Add("style", "background-color:transparent");
            //Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            //Button_report_section.Attributes.Add("style", "background-color:transparent");
        }

        protected void btnOverview_Click(object sender, EventArgs e)
        {
            
            new_applicant_section.Attributes.Add("style", "display:none");
            Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:initial");
            enquiries_section.Attributes.Add("style", "display:none");
            btnOverview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            //Button_report_section.Attributes.Add("style", "background-color:transparent");
            //btnListRejected.Attributes.Add("style", "background-color:transparent");
            Label_Account.Text = "l";
            Label_CustAcc.Text = "";
            Button_Overview_accordion.Text = "List All";
            //Session["flag_temp"] = 0; // List all
            lblSearch.Visible = true;
            DropDownList_Search_Overview.Visible = true;
            lblStatus.Visible = true;
            ddlStatus.Visible = true;
            ImageButton5.Visible = true;
            overview_section.Visible = true;
            enquiries_section.Visible = false;
            divForApproval.Visible = true;
            //lblText.Text = "Number/Name:";
            ddlStatus.SelectedItem.Value = "0";

            if (GLOBAL.user_id == GLOBAL.AdminID)
            {
                divForSalesman.Visible = true;
                divForApproval.Visible = true;
                divCommentFromSalesPerson.Visible = true;
            }

            if (!CheckSalesAdmin_BySQL()) 
            {
                btnListAll.Visible = false;
                divPendingButton.Visible = false;

            }

            //btnListAll.Attributes.Add("style", "background-color:#f58345");
           

            if (btnListAll.Visible == false) 
            {
                string userRole = GetUserRole(Session["logined_user_name"].ToString().Trim().Split('(')[0]);

                switch (userRole)
                {

                    case null:
                        btnListHOD.Attributes.Add("style", "background-color:#f58345");
                        ddlStatus.SelectedItem.Value = "4";

                        break;
                    //case "SalesAdmin":
                    //    btnListSalesAdmin.Attributes.Add("style", "background-color:#f58345");
                    //    ddlStatus.SelectedItem.Value = "5";

                    //break;
                    case "Approval":
                        btnListAnP.Attributes.Add("style", "background-color:#f58345");
                        ddlStatus.SelectedItem.Value = "6";

                        break;
                    case "SalesAdminManager":
                        btnListSalesAdminMgr.Attributes.Add("style", "background-color:#f58345");
                        ddlStatus.SelectedItem.Value = "7";

                        break;
                    case "GM":
                        btnListGM.Attributes.Add("style", "background-color:#f58345");
                        ddlStatus.SelectedItem.Value = "8";

                        break;
                }
            }
            f_Button_ListAll();
            //btnListAll_Click(sender, e);
        }

        protected void Button_enquiries_section_Click(object sender, EventArgs e)
        {
            new_applicant_section.Attributes.Add("style", "display:none");
            overview_section.Attributes.Add("style", "display:none");
            enquiries_section.Attributes.Add("style", "display:block");
            BindGrid_IssueTo();
            enquiries_section.Visible = true;
            overview_section.Visible = false;
            new_applicant_section.Visible = false;
        }

        protected void Button_report_section_Click(object sender, EventArgs e)
        {
            //Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            //Button_report_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            new_applicant_section.Attributes.Add("style", "display:none");
            Button_new_applicant_section.Attributes.Add("style", "background-color:transparent");
            overview_section.Attributes.Add("style", "display:none");
            //btnOverview.Attributes.Add("style", "background-color:transparent");
        }

        private void Check_DataRequest()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            try
            {
                overview_section.Visible = false;
                enquiries_section.Visible = false;

                bool getWarrantyAccess = WClaim_GET_NewApplicant.getWarrantyApprovalAccess(DynAx, GLOBAL.user_id);
                if (getWarrantyAccess || GLOBAL.user_authority_lvl == 1)
                {//for last approval admin to access only
                    EnableEdit();
                    btnListReject.Visible = true;
                    btnListHOD.Visible = true;
                    divForSalesman.Visible = true;
                    divForApproval.Visible = true;
                    //btnListSalesOrder.Visible = true;
                    GridViewOverviewList.Visible = false;
                    //Button_report_section.Visible = true;
                    //img2.Visible = true;
                }
                else
                {
                    divForSalesman.Visible = true;
                    //Button_report_section.Visible = false;
                    //img2.Visible = false;
                }

                string temp1 = GLOBAL.data_passing.ToString();
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        //string test = temp1.Substring(0, 6);
                        if (temp1.Substring(0, 6) == "@SECM_")//Data receive from CustomerMaster> SE
                        {
                            Label_Account.Text = temp1.Substring(6);
                            validate();
                            labelAccount.Visible = true;
                        }
                        if (temp1.Substring(0, 6) == "@SESE_")
                        {
                            string HOD = "";
                            string ReportTo = "";
                            string result = temp1.Substring(6);
                            string[] arr_raw_result = result.Split('|');
                            string selected_Id = arr_raw_result[0];
                            string DocStatus = arr_raw_result[1];
                            string CustomerAcc = arr_raw_result[2];
                            string DepositType = arr_raw_result[3];
                            string NextApproval = arr_raw_result[4];
                            hidden_LabelNextApprover.Text = NextApproval;
                            //Customer Information
                            string WareHouse = ""; string OpeningAccDate = ""; string CustName = ""; string CustPhone = "";
                            string CustContact = ""; string SalesmanID = ""; string SalesmanName = ""; string CustGroup = "";
                            //EOR/Signage/Supercese Case
                            string FormType = ""; string AppType = ""; string DepositAmount = "";
                            //Delivery Details
                            string DelTo = ""; string DeliveryAddress = ""; string RemarksSales = ""; string IssueTo = "";
                            //Signage License Status
                            int RequestSign = 0;
                            //Location Map
                            string MapDesc = ""; string TrafficDensity = ""; string MapRemark = ""; string SignboardVisibility = "";
                            int ImgInd = 0;
                            //Comment from Sales Person
                            string TypeServiceCenter = ""; string WorkshopFacilities = ""; string OwnerExperience = ""; string WorkshopSizeType = "";
                            string NumberOfMechanics = ""; string YearOfEstablishment = ""; string WorkshopStatus = "";

                            string ProcessStatus = ""; string AppliedDate = "";
                            //All Remarks // Purpose For labeling all remarks 10/6/2025
                            string SalesAdminRemarks = ""; string HODRemarks = ""; string HODRemarks2 = ""; string HODRemarks3 = ""; string ANPRemarks = "";
                            string AdminMgrRemarks = ""; string GMRemarks = ""; string RemarksDisplay = ""; string HODRemarks4 = "";

                            //Sub Dealer or Branch  
                            string SubDBWorkshopName = "";

                            int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");

                            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
                            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WebEquipment);

                            int cn1 = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
                            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

                            qbr6.Call("value", selected_Id);
                            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

                            if ((bool)axQueryRun6.Call("next"))
                            {
                                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WebEquipment);
                                CustPhone = DynRec6.get_Field("CustPhone").ToString();
                                CustContact = DynRec6.get_Field("CustContact").ToString();
                                RequestSign = (string.IsNullOrEmpty(DynRec6.get_Field("RequestSign").ToString()) ? 0 : Int32.Parse(DynRec6.get_Field("RequestSign").ToString()));
                                FormType = DynRec6.get_Field("FormType").ToString();
                                AppType = DynRec6.get_Field("AppType").ToString();
                                DelTo = DynRec6.get_Field("Del_To").ToString();
                                DeliveryAddress = DynRec6.get_Field("Del_Addr").ToString();
                                RemarksSales = DynRec6.get_Field("Remarks_Sales").ToString();
                                TypeServiceCenter = DynRec6.get_Field("ServiceType").ToString();
                                WorkshopFacilities = DynRec6.get_Field("ShopFacility").ToString();
                                OwnerExperience = DynRec6.get_Field("OwnerExp").ToString();
                                WorkshopSizeType = DynRec6.get_Field("ShopSize").ToString();
                                NumberOfMechanics = DynRec6.get_Field("Worker").ToString();
                                YearOfEstablishment = DynRec6.get_Field("YearEstablish").ToString();
                                WorkshopStatus = DynRec6.get_Field("ShopStatus").ToString();
                                MapDesc = DynRec6.get_Field("MapLocation").ToString();
                                TrafficDensity = DynRec6.get_Field("MapTraffic").ToString();
                                MapRemark = DynRec6.get_Field("MapRemark").ToString();
                                SubDBWorkshopName = DynRec6.get_Field("Branch_WorkshopName").ToString();
                                SignboardVisibility = DynRec6.get_Field("MapSCVisible").ToString();
                                ImgInd = Int32.Parse(DynRec6.get_Field("Img_Ind").ToString());
                                ProcessStatus = DynRec6.get_Field("ProcessStatus").ToString();
                                IssueTo = DynRec6.get_Field("Del_Person").ToString();
                                string appliedDateString = DynRec6.get_Field("AppliedDate").ToString();
                                DateTime appliedDate;
                                if (DateTime.TryParseExact(appliedDateString, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out appliedDate))
                                {
                                    AppliedDate = appliedDate.ToString("dd/MM/yyyy");
                                }

                                //Might change
                                DepositAmount = DynRec6.get_Field("Cost").ToString();

                                #region Remarks   

                                // Set fields
                                SalesAdminRemarks = DynRec6.get_Field("Remarks_Admin").ToString();
                                HODRemarks = DynRec6.get_Field("Remarks_HOD").ToString();
                                HODRemarks2 = DynRec6.get_Field("Remarks_HOD_2").ToString(); 
                                HODRemarks3 = DynRec6.get_Field("Remarks_HOD_3").ToString();
                                HODRemarks4 = DynRec6.get_Field("Remarks_HOD_4").ToString();
                                ANPRemarks = DynRec6.get_Field("RemarksAnP").ToString();
                                AdminMgrRemarks = DynRec6.get_Field("Remarks_AdminMgr").ToString();
                                GMRemarks = DynRec6.get_Field("Remarks_GM").ToString();

                                var remarksDictionary = new Dictionary<string, string>
                                {
                                    { "PR Remarks", SalesAdminRemarks },{ "HOD Remarks", HODRemarks },{ "2nd HOD Remarks", HODRemarks2 },
                                    { "3rd HOD Remarks", HODRemarks3},{ "4th HOD Remarks", HODRemarks4},
                                    { "ANP Remarks", ANPRemarks },
                                    { "VR Remarks", AdminMgrRemarks },{ "GM Remarks", GMRemarks }};

                                // Build RemarksDisplay
                                if (!string.IsNullOrEmpty(RemarksSales))
                                {
                                    RemarksDisplay += $"SB Remarks: {RemarksSales} \n"; // No </br> for SalesManRemarks
                                }
                                foreach (var remark in remarksDictionary)
                                {
                                    if (!string.IsNullOrEmpty(remark.Value))
                                    {
                                        RemarksDisplay += $"</br> {remark.Key}: {remark.Value} \n";
                                    }
                                }
                                lblAllRemarks.Text = RemarksDisplay;
                                divAllRemarks.Visible = true;
                                #endregion

                                DynRec6.Dispose();
                            }
                            axQueryRun6.Dispose();
                            axQueryDataSource6.Dispose();

                            int CustTable = DynAx.GetTableIdWithLock("CustTable");

                            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

                            int custAcc = DynAx.GetFieldId(CustTable, "AccountNum");
                            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", custAcc);

                            qbr.Call("value", CustomerAcc);
                            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                            if ((bool)axQueryRun.Call("next"))
                            {
                                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                                WareHouse = DynRec.get_Field("Dimension").ToString();
                                OpeningAccDate = DynRec.get_Field("OpeningAccDate").ToString();
                                CustName = DynRec.get_Field("Name").ToString();
                                SalesmanID = DynRec.get_Field("EmplId").ToString();
                                CustGroup = DynRec.get_Field("CustGroup").ToString();

                                DynRec.Dispose();
                            }
                            axQueryRun.Dispose();
                            axQueryDataSource.Dispose();

                            int EmplTable = DynAx.GetTableIdWithLock("EmplTable");

                            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

                            int emplID = DynAx.GetFieldId(EmplTable, "EmplId");
                            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", emplID);

                            qbr1.Call("value", SalesmanID);
                            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                            if ((bool)axQueryRun1.Call("next"))
                            {
                                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                                SalesmanName = DynRec1.get_Field("LF_EmplName").ToString();
                                SalesmanName = (string.IsNullOrEmpty(SalesmanName) ? DynRec1.get_Field("Del_Name").ToString() : SalesmanName);
                                ReportTo = DynRec1.get_Field("ReportTo").ToString();

                                DynRec1.Dispose();
                            }
                            axQueryRun1.Dispose();
                            axQueryDataSource1.Dispose();


                            if (ImgInd == 1)
                            {
                                cbPhoneSubmission.Enabled = true;
                            }

                            //Customer Information
                            lblClaimText.Text = DocStatus; Label_Salesman.Text = SalesmanName; rblRequestSign.SelectedValue = RequestSign.ToString();
                            lblAccOpenedDate.Text = OpeningAccDate; Label_CustName.Text = CustName; Label_CustAcc.Text = CustomerAcc;
                            lblAcc.Visible = true; lblEquipID.Text = selected_Id; lblClaimText.Text = DocStatus; lblWarehouse.Text = WareHouse;
                            lblTelNo.Text = CustPhone; lblContactPerson.Text = CustContact; lblCustClass.Text = CustGroup; hdSalesmanId.Value = SalesmanID; lblSub.Visible = true; lblSubDate.Text = AppliedDate;

                            hidden_LabelEquipID.Text = selected_Id;
                            hidden_Label_PreviousStatus.Text = DocStatus;
                            hidden_Label_NextStatus.Text = "";

                            //EOR/Signage
                            TextBox_TotalCost.Text = DepositAmount;

                            //Delivery Details
                            //txtRemarks.Text = RemarksSales;   //12.6.2025 - must let approver type their own remarks
                            txtDeliveryAddress.Text = DeliveryAddress;

                            //Map Location
                            #region Map Location
                            txtMapDescription.Text = MapDesc;
                            DivDisplayMapA.Visible = MapDesc.Contains("A");
                            DivDisplayMapB.Visible = MapDesc.Contains("B");
                            DivDisplayMapC.Visible = MapDesc.Contains("C");
                            txtTrafficDensity.Text = TrafficDensity;
                            txtMapRemark.Text = MapRemark;
                            txtSignboardVisibility.Text = SignboardVisibility;
                            if (ImgInd == 1)
                            {
                                cbPhoneSubmission.Checked = true;
                            }

                            if (TrafficDensity == "1")
                            {
                                textTrafficDensity.Text = "Low";
                            }
                            else if (TrafficDensity == "2")
                            {
                                textTrafficDensity.Text = "Fair";
                            }
                            else if (TrafficDensity == "3")
                            {
                                textTrafficDensity.Text = "High";
                            }

                            if (SignboardVisibility == "1")
                            {
                                textSignboardVisibility.Text = "Low";
                            }
                            else if (SignboardVisibility == "2")
                            {
                                textSignboardVisibility.Text = "Fair";
                            }
                            else if (SignboardVisibility == "3")
                            {
                                textSignboardVisibility.Text = "High";
                            }
                            #endregion
                            //Comment from salesman
                            txtTypeServiceCenter.Text = TypeServiceCenter; txtWorkshopFacilities.Text = WorkshopFacilities; txtOwnerExperience.Text = OwnerExperience;
                            txtWorkshopSizeType.Text = WorkshopSizeType; txtNumberOfMechanics.Text = NumberOfMechanics; txtYearOfEstablishment.Text = YearOfEstablishment;

                            lblGetProcessStat.Text = ProcessStatus.Replace(Environment.NewLine, "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>");

                            //SubDealer or Branch 
                            txtSBWorkshopName.Text = SubDBWorkshopName;
                            //Dropdown and Radio
                            rblApplicationType.SelectedValue = AppType;
                            ddlRequestType.ClearSelection(); var item = ddlRequestType.Items.FindByText(FormType);
                            ddlRequestType.SelectedIndex = (FormType == "--- Please Select --") ? 0 : (item != null ? ddlRequestType.Items.IndexOf(item) : -1);
                            ddlItemDeliveryTo.ClearSelection(); ddlItemDeliveryTo.Items.FindByText(DelTo).Selected = true;
                            rblWorkshopStatus.ClearSelection(); rblWorkshopStatus.Items.FindByText(WorkshopStatus).Selected = true;

                            //Temporary put try catch since boss didnt give me full data, so some of the items might not be found
                            try
                            {
                                ddlIssueTo.ClearSelection();
                                LoadIssueToDropDown(true, IssueTo);
                                ddlIssueTo.Items.FindByText(IssueTo).Selected = true;
                            }
                            catch
                            {

                            }
                            //validate();
                            //lbltype.Visible = true;
                            //ddlClaimType.Visible = true;
                            //divProductType.Visible = true;
                            //ddlClaimType.SelectedValue = subClaimType;

                            string Type = "";
                            if (DepositType == "1")
                            {
                                Type = "Signboard";
                            }
                            else
                            {
                                Type = "Equipment";
                            }
                            SignboardEquipmentAppGroupModel m = getSignboardEquipmentApprovalUser(DynAx, Type, GLOBAL.user_company.ToUpper().Trim());
                            // Reason is to pass value in rblES before go through disableEdit()
                            rblES.SelectedValue = (DepositType == "1") ? "1" : "2";
                            ////////////////////--------------------------
                            string ProcessedUserName = Regex.Replace(Session["logined_user_name"].ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
                            string ProcessedNewApproval = Regex.Replace(NextApproval, @"\s+", "").ToLower().Trim();
                            if (DocStatus == "AwaitingHOD")//Invoice Check
                            {
                                if (ProcessedNewApproval.Contains(ProcessedUserName) || ProcessedUserName.Contains(ProcessedNewApproval))
                                {
                                    EnableEdit();
                                    Button_Approve.Visible = true;
                                    Button_Print.Visible = true;
                                    rblES.Enabled = true;
                                    //ddlClaimType.Enabled = true;
                                    upload_section.Visible = false;
                                }
                                else if (ProcessedUserName == "kennychuahyewsiang")
                                {
                                    string approvalLower = ProcessedNewApproval?.ToLowerInvariant() ?? "";
                                    bool isMatch = approvalLower.Contains("kennychuah") || approvalLower.Contains("kennychuahys") ||
    ProcessedNewApproval.Contains(ProcessedUserName) || ProcessedUserName.Contains(ProcessedNewApproval);

                                    if (isMatch)
                                    {
                                        EnableEdit();
                                        Button_Approve.Visible = true;
                                        Button_Print.Visible = true;

                                        rblES.Enabled = true;
                                        //ddlClaimType.Enabled = true;
                                        upload_section.Visible = false;
                                    }
                                    else
                                    {
                                        disableEdit();
                                    }
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (DocStatus == "AwaitingSalesAdmin")
                            {
                                if (ProcessedUserName == Regex.Replace(m.AltSalesAdmin, @"\s+", "").ToLower().Trim() || ProcessedNewApproval.Contains(ProcessedUserName) || ProcessedUserName.Contains(ProcessedNewApproval))
                                {
                                    EnableEdit();
                                    rblES.Enabled = true;
                                    Button_Proceed.Visible = true;
                                    //ddlClaimType.Enabled = false;
                                    Button_Approve.Visible = true;
                                    Button_Print.Visible = true;
                                    new_applicant_sectionDeliveryDetails.Visible = true;
                                    //divCustChop.Attributes.Add("style", "display:block");
                                    //signWarehouse_section.Attributes.Add("style", "display:block");
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (DocStatus == "AwaitingAnP")
                            {
                                if (ProcessedNewApproval.Contains(ProcessedUserName) || ProcessedUserName.Contains(ProcessedNewApproval))
                                {
                                    EnableEdit();

                                    new_applicant_sectionDeliveryDetails.Visible = true;
                                    rblES.Enabled = true;
                                    Button_Approve.Visible = true;
                                    Button_Print.Visible = true;
                                    //ddlClaimType.Enabled = false;
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (DocStatus == "AwaitingSalesAdminMgr")
                            {
                                if (ProcessedNewApproval.Contains(ProcessedUserName) || ProcessedUserName.Contains(ProcessedNewApproval))
                                {

                                    new_applicant_sectionDeliveryDetails.Visible = true;

                                    divCommentFromSalesPerson.Visible = true;
                                    Button_Reject.Visible = true;
                                    Button_Submit.Visible = true;
                                    Button_Approve.Visible = true;
                                    Button_Print.Visible = true;
                                    //Button_CreateReturn.Visible = true;
                                    rblES.Enabled = true;
                                    //ddlClaimType.Enabled = false;
                                }
                                else
                                {
                                    disableEdit();
                                }
                            }
                            else if (DocStatus == "AwaitingGM")
                            {
                                if (ProcessedNewApproval.Contains(ProcessedUserName) || ProcessedUserName.Contains(ProcessedNewApproval))
                                {
                                    new_applicant_sectionDeliveryDetails.Visible = true;

                                    divCommentFromSalesPerson.Visible = true;
                                    Button_Approve.Visible = true;
                                    Button_Print.Visible = true;
                                    Button_Submit.Visible = true;
                                    rblES.Enabled = true;
                                    // ddlClaimType.Enabled = false;
                                    if (!isSoCreated(DynAx, selected_Id))
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
                            else if (DocStatus == "Draft")
                            {
                                EnableEdit();
                            }
                            else if (DocStatus == "Approved")
                            {
                                disableEdit();

                                new_applicant_sectionDeliveryDetails.Visible = true;

                                divCommentFromSalesPerson.Visible = true;

                                Button_Submit.Visible = false;
                                Button_Reject.Visible = false;
                                Button_Approve.Visible = false;
                                Button_Print.Visible = true;
                                Button_Proceed.Visible = false;
                                divMainRemarks.Visible = false;
                            }
                            else if (DocStatus == "Rejected")
                            {
                                disableEdit();
                                Button_Approve.Visible = false;
                                Button_Submit.Visible = false;
                                Button_Reject.Visible = false;
                                new_applicant_sectionDeliveryDetails.Visible = true;

                                divCommentFromSalesPerson.Visible = true;
                                divMainRemarks.Visible = false;
                            }
                            else
                            {
                                disableEdit();
                            }

                            if (DepositType == "1")
                            {
                                rblES.SelectedValue = "1";
                                get_data_load_LF_SignboardEquipment_Batch(DynAx, selected_Id, DepositType);

                            }
                            else
                            {
                                rblES.SelectedValue = "2";
                                //if (subClaimType == "3")
                                //{
                                //    get_data_load_LF_SignboardEquipment_Batch(DynAx, selected_Id, subClaimType);
                                //}
                                //else
                                //{
                                //    get_data_load_LF_SignboardEquipment(DynAx, selected_Id, subClaimType);
                                //}
                            }
                            SectionVisibility();
                            string SignboardEquipmentProcessStatus = getSignboardEquipmentProcessStatus(DynAx, selected_Id);
                            var split = SignboardEquipmentProcessStatus.Split(' ');
                            if (DocStatus == "Draft")//to avoid other user submit
                            {
                                if (split[2] != GLOBAL.user_id)
                                {
                                    Button_Submit.Visible = false;
                                    Button_Reject.Visible = false;
                                    //btnAmend.Visible = false;
                                }
                            }
                            //load_LF_WarrantyBattery(DynAx, selected_Id);
                            cusButton.Visible = false;
                            //getClaimImage();
                            //getBatInspectionImage();
                            DateTime appliedDate2;
                            if (DateTime.TryParseExact(AppliedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out appliedDate2))
                            {
                                GetPastMonthsPurchaseRecord(CustomerAcc, appliedDate2);
                            }
                        }
                    }
                    if (GLOBAL.user_id == GLOBAL.AdminID)
                    {
                        Button_Reject.Visible = true;
                        Button_Approve.Visible = true;
                        Button_Print.Visible = true;
                    }
                    new_applicant_sectionDeliveryDetails.Visible = true;
                    divCommentFromSalesPerson.Visible = true;
                    //string getUserName = WClaim_GET_NewApplicant.CheckUserName(DynAx, GLOBAL.logined_user_name);
                    lblGetName.Text = Session["user_id"].ToString();
                    lblGetDate.Text = DateTime.Now.ToString();
                    Session["data_passing"] = "";
                    new_applicant_section.Attributes.Add("style", "display:initial");
                    Button_new_applicant_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    //btnOverview.Attributes.Add("style", "background-color:transparent");
                    overview_section.Attributes.Add("style", "display:none");
                    enquiries_section.Attributes.Add("style", "display:none");
                    //Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
                    div_CustInfoExtra.Visible = true;

                    // Define an array of statuses that require GetReferIdImage
                    string[] statuses = { "AwaitingSalesAdmin", "AwaitingHOD", "AwaitingAnP", "AwaitingSalesAdminMgr", "AwaitingGM", "Approved" };
                    // Check if the current status is in the array
                    if (statuses.Contains(hidden_Label_PreviousStatus.Text))
                    {
                        GetReferIdImage(false);
                    }
                    else
                    {
                       //upload_section.Visible = false;
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);
                }
            }
            catch (Exception ER_SE_00)
            {
                Function_Method.MsgBox("ER_SE_00: " + ER_SE_00.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        public SignboardEquipmentAppGroupModel getSignboardEquipmentApprovalUser(Axapta DynAx, string Type, string Company)
        {
            SignboardEquipmentAppGroupModel m = new SignboardEquipmentAppGroupModel();

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "select * from signboardequipmentappgroup where Type = @p0 and Company = @p1";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", Type);
            cmd.Parameters.AddWithValue("@p1", Company);

            MySqlDataReader reader = cmd.ExecuteReader();

            if ((bool)reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    m.Type = reader.GetString("Type");
                    m.Company = reader.GetString("Company");
                    m.Approval = reader.GetString("Approval");
                    m.SalesAdmin = reader.GetString("SalesAdmin");
                    m.AltSalesAdmin = reader.GetString("AltSalesAdmin");
                    m.SalesAdminManager = reader.GetString("SalesAdminManager");
                    m.GM = reader.GetString("GM");
                }
            }
            conn.Close();
            return m;
        }

        public static bool isSoCreated(Axapta DynAx, string EuipID)
        {
            AxaptaRecord DynRec;
            string TableName = "LF_WebEquipment";
            string fieldName = ("Equip_ID");
            string fieldName1 = ("GMDate");
            //string fieldValue = ("your_search_criteria_here");

            // Define the record
            DynRec = DynAx.CreateAxaptaRecord(TableName);
            DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}' && %1.{2} >= '{3}'", fieldName, EuipID, fieldName1, "01/01/1900"));
            // Check if the query returned any data.
            if (DynRec.Found)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string getSignboardEquipmentProcessStatus(Axapta DynAx, string EquipmentID)
        {
            //Jerry 2024-11-05 Avoid hardcode
            //int LF_WarrantyTable = 50773;
            //int LF_WarrantyTable = 30661; //live
            int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");

            string ProcessStatus = "";

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WebEquipment);

            //Jerry 2024-11-05 Avoid hardcode
            //var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", 30001);//claimNumber
            int cn1 = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

            qbr6.Call("value", EquipmentID);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WebEquipment);
                ProcessStatus = DynRec6.get_Field("ProcessStatus").ToString();

                DynRec6.Dispose();
            }
            return ProcessStatus;
        }

        private void validate()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string CustAcc = Label_Account.Text.Trim();
            clear_parameter_NewApplication_Customer();
            clear_parameter_NewApplication_ItemDetails();
            clear_parameter_NewApplication_BatchItemDetails();
            //clear_parameter_NewApplication_TransportationArrangement();
            Label_Account.Text = CustAcc;//after clear all, rewrite back

            if (CustAcc == "")
            {
                Function_Method.MsgBox("There is no account number", this.Page, this);
                return;
            }

            try
            {
                var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                string CustName = tuple_getCustInfo.Item1;
                string Address = tuple_getCustInfo.Item2;
                string temp_EmplId = tuple_getCustInfo.Item3;
                string temp_EmpName = tuple_getCustInfo.Item4;
                string Warehouse = tuple_getCustInfo.Item5;
                string CustGroup = tuple_getCustInfo.Item6;
                string AccOpenDate = tuple_getCustInfo.Item7;

                //string BranchID = tuple_getCustInfo.Item5;//dimension
                //string CustomerClass = tuple_getCustInfo.Item6.Trim();
                //string getOpeningAccDate = tuple_getCustInfo.Item7;
                if (CustName == "")
                {
                    upImg.Visible = false;
                    Function_Method.MsgBox("No data found", this.Page, this);
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);
                    return;
                }
                else
                {
                    Label_Salesman.Text = temp_EmpName + " (" + temp_EmplId + ")";
                    hdSalesmanId.Value = temp_EmplId;
                    Label_CustAcc.Text = CustAcc;
                    Label_CustName.Text = CustName;
                    lblWarehouse.Text = Warehouse;
                    lblCustClass.Text = CustGroup;
                    lblAccOpenedDate.Text = AccOpenDate;
                    //Button_Delivery_Addr.Visible = false;
                    div_CustInfoExtra.Visible = true;
                    txtDeliveryAddress.Text = Address;

                    
                    GetPastMonthsPurchaseRecord(CustAcc, DateTime.Now);

                }
            }
            catch (Exception ER_SE_01)
            {
                Function_Method.MsgBox("ER_SE_01: " + ER_SE_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private void clear_parameter_NewApplication_Customer()
        {
            hidden_inventLocationId.Text = "";
            // <!--Customer ////////////////////////////////////////////////////////////////////////////////////-->
            Label_Account.Text = "";
            Label_CustName.Text = "";
            Label_Salesman.Text = "";

            div_CustInfoExtra.Visible = false;

            hidden_Label_NextStatus.Text = "";
            hidden_Label_PreviousStatus.Text = "";
            hidden_LabelEquipID.Text = "";
        }

        private void clear_parameter_NewApplication_ItemDetails()
        {
            //Accordion_SignboardDetails.Visible = false;
            //new_applicant_section_ItemDetails.Attributes.Add("style", "display:none");
        }

        private void clear_parameter_NewApplication_BatchItemDetails()
        {
            initialize_GridView_BatchItem();
            //Accordion_BatchItemDetails.Visible = false;
            //new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:none");
        }

        private void clear_parameter_NewApplication_TransportationArrangement()
        {
            Accordion_DeliveryDetails.Visible = false;
            new_applicant_sectionDeliveryDetails.Visible = false;
        }

        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_SECM@";//SignboardEquipment > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        private void get_data_load_LF_SignboardEquipment_Batch(Axapta DynAx, string selected_Id, string DepositType)
        {
            GridView_BatchItem.DataSource = null;
            GridView_BatchItem.DataBind();

            DataTable dt = new DataTable();
            DataRow dr = null;
            List<ListItem> List_Description = new List<ListItem>();
            List<ListItem> List_ItemID = new List<ListItem>();
            List<ListItem> List_Qty = new List<ListItem>();
            List<ListItem> List_Size = new List<ListItem>();

            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemID", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("Size", typeof(string)));

            // Instantiate your DropDownList control
            DropDownList ddlReasonReturn = new DropDownList();
            string ReturnReasonBatch = "";
            string otherReason = "";
            string returnReasonOther = "";
            //Jerry 2024-11-04 Avoid hardcode
            //int LF_WarrantyLine = 50772;
            //int LF_WarrantyLine = 30660; //live
            int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WebEquipment);

            //Jerry 2024-11-04 Avoid hardcode
            //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30001);//Claim Number
            int claimNumber = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimNumber);//Claim Number
            qbr1.Call("value", selected_Id);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            int count = 0;
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WebEquipment);

                for (int i = 1; 1 <= 3; i++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(DynRec1.get_Field("Item" + i).ToString()))
                        {
                            break;
                        }

                    }
                    catch
                    {
                        break;
                    }
                    count = count + 1;
                    dr = dt.NewRow();

                    string New_QTY = DynRec1.get_Field("Qty" + i).ToString();
                    string Size = DynRec1.get_Field("Size" + i).ToString();
                    //string Price = DynRec1.get_Field("UnitPrice").ToString();
                    string Description = DynRec1.get_Field("Item" + i).ToString();

                    dr["No."] = count;
                    dr["Description"] = string.Empty;
                    dr["ItemID"] = string.Empty;
                    dr["Qty"] = string.Empty;
                    dr["Size"] = string.Empty;

                    List_Qty.Add(new ListItem(New_QTY));
                    //List_ItemID.Add(new ListItem(ItemID));
                    List_Size.Add(new ListItem(Size));
                    //List_Price.Add(new ListItem(Price));
                    List_Description.Add(new ListItem(Description));

                    dt.Rows.Add(dr);
                }

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
                    //TextBox price = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_Price");
                    TextBox box3 = (TextBox)GridView_BatchItem.Rows[i].Cells[4].FindControl("TextBox_New_Size");

                    box1.Text = List_Description[i].ToString();
                    //lblItemID.Text = List_ItemID[i].ToString();
                    box2.Text = List_Qty[i].ToString();

                    //price.Text = List_Price[i].ToString();
                    box3.Text = List_Size[i].ToString();
                }
            }
        }

        protected void TextBox_DescriptionBatchItem_Changed(object sender, EventArgs e)
        {

        }

        protected void OnSelectedIndexChanged_DropDownList_SearchBatchItem(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            //get the the id that call the function
            DropDownList DropDownList_SearchBatchItem = sender as DropDownList;

            string ClientID = DropDownList_SearchBatchItem.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)DropDownList_SearchBatchItem.NamingContainer;
            int index = row.RowIndex;

            TextBox box2 = (TextBox)GridView_BatchItem.Rows[index].Cells[1].FindControl("TextBox_DescriptionBatchItem");
            Label lblItemID = (Label)GridView_BatchItem.Rows[index].Cells[2].FindControl("lblItemID");
            //RadioButton btl = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbBtl");
            //RadioButton add = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbAdd");
            HiddenField hdrecid = (HiddenField)GridView_BatchItem.Rows[index].FindControl("hd_RecIdBatchItem");
            //DropDownList ddlinvoiceid = (DropDownList)GridView_BatchItem.Rows[index].Cells[5].FindControl("DropDownList_CustDO");
            //TextBox txtInvoiceId = (TextBox)GridView_BatchItem.Rows[index].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

            string BatchItemId_RecId = DropDownList_SearchBatchItem.SelectedItem.Value;
            string[] arr_BatchItemId_RecId = BatchItemId_RecId.Split('|');
            string ItemId = arr_BatchItemId_RecId[0];
            string RecId = arr_BatchItemId_RecId[1];

            AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

            string itemunit = SFA_GET_SALES_ORDER.get_SLUnit(DynAx, ItemId);//fieldValue=Itemid
            string itemUnitStr = domComSalesLine.Call("getValidUnits", ItemId).ToString();//fieldValue=Itemid
            string[] arr_itemUnitStr = itemUnitStr.Split(new char[] { ',', '=' });


            if (ItemId != "")//Only selected
            {
                box2.Text = DropDownList_SearchBatchItem.SelectedItem.ToString();
                lblItemID.Text = ItemId;
                List<ListItem> getInvoiceId = WClaim_GET_NewApplicant.get_CustInvoiceTrans_details(DynAx, ItemId, Label_CustAcc.Text);
                //if (getInvoiceId.Count > 1)
                //{
                //    ddlinvoiceid.Items.AddRange(getInvoiceId.ToArray());
                //    ddlinvoiceid.Visible = true;
                //    //txtInvoiceId.Visible = false;
                //}

                //if (getInvoiceId.Count == 0 || ddlinvoiceid.Items.Count == 0)
                //{
                //    ddlinvoiceid.Visible = false;
                //    txtInvoiceId.Visible = true;
                //    txtInvoiceId.Attributes["placeholder"] = "Invoice not found! Please insert.";
                //}
                hdrecid.Value = RecId;
                DropDownList_SearchBatchItem.Items.Clear();
                DropDownList_SearchBatchItem.Visible = false;
                box2.Visible = true;
            }

            DynAx.Dispose();
        }


        protected void Button_AddBatchItem_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //Extract the  values

                        if (dtCurrentTable.Rows.Count == 3)
                        {
                            Function_Method.MsgBox("Not more than 3 items.", this.Page, this);

                            return;
                        }
                        TextBox box1 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        Label lblItemID = (Label)GridView_BatchItem.Rows[rowIndex].Cells[2].FindControl("lblItemID");
                        TextBox box2 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box3 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("TextBox_New_Size");
                        //RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbBtl");
                        //RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbAdd");
                        //TextBox box3 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");
                        //DropDownList ddl = (DropDownList)GridView_BatchItem.Rows[rowIndex].Cells[5].FindControl("DropDownList_CustDO");

                        // 2025/03/04 KX Validation for row inputs 
                        if (string.IsNullOrEmpty(box1.Text) || string.IsNullOrEmpty(box2.Text))
                        {
                            Function_Method.MsgBox("Please finish the inputs from the previous row before moving on to the next row. ", this.Page, this);

                            return;
                        }

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["No."] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Description"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["ItemID"] = lblItemID.Text;
                        dtCurrentTable.Rows[i - 1]["Qty"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Size"] = box3.Text;

                        //dtCurrentTable.Rows[i - 1]["Unit"] = radiobutton1.Checked.ToString() + "|" + radiobutton1.Text + "|" + radiobutton2.Checked.ToString() + "|" + radiobutton2.Text;
                        //if (box3.Text == "")
                        //{
                        //    dtCurrentTable.Rows[i - 1]["CustomerDO"] = ddl.Text;
                        //}
                        //else
                        //{
                        //    dtCurrentTable.Rows[i - 1]["CustomerDO"] = box3.Text;
                        //}


                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView_BatchItem.DataSource = dtCurrentTable;
                    GridView_BatchItem.DataBind();
                }
                else
                {
                    initialize_GridView_BatchItem();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        Label lblItemID = (Label)GridView_BatchItem.Rows[rowIndex].Cells[2].FindControl("lblItemID");
                        TextBox box2 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box3 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[3].FindControl("TextBox_New_Size");
                        //RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbBtl");
                        //RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbAdd");
                        //TextBox box3 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

                        lblItemID.Text = dt.Rows[i]["ItemID"].ToString();
                        box1.Text = dt.Rows[i]["Description"].ToString();
                        box2.Text = dt.Rows[i]["Qty"].ToString();
                        box3.Text = dt.Rows[i]["Size"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        private void initialize_GridView_BatchItem()
        {
            GridView_BatchItem.Columns[4].Visible = true;//RecId
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemID", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("Size", typeof(string)));
            //dt.Columns.Add(new DataColumn("CustomerDO", typeof(string)));
            dr = dt.NewRow();

            dr["No."] = 1;
            dr["ItemID"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["Qty"] = string.Empty;
            dr["Size"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
            GridView_BatchItem.DataSource = dt;
            GridView_BatchItem.DataBind();
        }

        protected void GridView_BatchItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                dt.Rows.RemoveAt(e.RowIndex);

                //=====================================================================
                //to set back row number record textbox data
                int row_count = dt.Rows.Count;
                for (int i = 0; i < row_count; i++)
                {
                    int currentNo = Convert.ToInt32(dt.Rows[i]["No."]);

                    TextBox box1 = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                    Label box2 = (Label)GridView_BatchItem.Rows[currentNo - 1].Cells[2].FindControl("lblItemID");
                    TextBox box3 = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[3].FindControl("TextBox_New_QTY");
                    TextBox box4 = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[4].FindControl("TextBox_New_Size");

                    //RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[currentNo - 1].Cells[4].FindControl("rbBtl");
                    //RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[currentNo - 1].Cells[4].FindControl("rbAdd");
                    //TextBox CustDo = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

                    dt.Rows[i]["Description"] = box1.Text;
                    dt.Rows[i]["ItemID"] = box2.Text;
                    dt.Rows[i]["Qty"] = box3.Text;
                    dt.Rows[i]["Size"] = box4.Text;
                    //if (radiobutton1.Checked)
                    //{
                    //    radiobutton1.Checked = true;
                    //    dt.Rows[i]["Unit"] = radiobutton1.Text;
                    //}
                    //else
                    //{
                    //    radiobutton2.Checked = true;
                    //    dt.Rows[i]["Unit"] = radiobutton2.Text;
                    //}
                    //dt.Rows[i]["CustomerDO"] = CustDo.Text;
                    dt.Rows[i]["No."] = (i + 1).ToString();
                }
                //=====================================================================

                GridView_BatchItem.DataSource = dt;
                GridView_BatchItem.DataBind();
                ViewState["CurrentTable"] = dt;
                //to set back data for textbox for viewing
                for (int i = 0; i < row_count; i++)
                {
                    TextBox box1 = (TextBox)GridView_BatchItem.Rows[i].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                    Label box2 = (Label)GridView_BatchItem.Rows[i].Cells[2].FindControl("lblItemID");
                    TextBox box3 = (TextBox)GridView_BatchItem.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                    TextBox box4 = (TextBox)GridView_BatchItem.Rows[i].Cells[3].FindControl("TextBox_New_Size");

                    box1.Text = dt.Rows[i]["Description"].ToString();
                    box2.Text = dt.Rows[i]["ItemID"].ToString();
                    box3.Text = dt.Rows[i]["Qty"].ToString();
                    box4.Text = dt.Rows[i]["Size"].ToString();

                }
            }

        }
        public void getDetailsAndSendemail(string Process_Status, string ProcessStatus, string DocStatus, string SalesmanEmail)
        {
            string Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Process_Status = ProcessStatus + DocStatus + " by " + Session["user_id"].ToString() + " on " + Date;
            string MailSubject = "Signboard/Equipment " + DocStatus + ". Signboard/Equipment ID: " + hidden_LabelEquipID.Text;
            string MailTo = Session["user_id"].ToString() + "@posim.com.my";
            string MailCC = SalesmanEmail;
            string SendMsg = "Signboard/Equipment " + "(" + hidden_LabelEquipID.Text + ") " + " have been " + Process_Status + "." + "\n" + "\n" +
                             "Customer Acc No: " + Label_CustAcc.Text + "\n" +
                             "Customer Name: " + Label_CustName.Text + "\n" +
                             "Signboard/Equipment Id: " + hidden_LabelEquipID.Text + "\n" +
                             "Status: " + DocStatus;
            Function_Method.SendMail(Session["user_id"].ToString(), Session["user_id"].ToString(), MailSubject, MailTo, MailCC, SendMsg);
        }

        protected void Button_Approve_Click(object sender, EventArgs e)
        {// Usage:    
            string message; // Declare the variable to receive the out parameter
            bool isValid = CheckValidation(out message);
            isValid = CheckValidation_SalesAdmin(out message); // Deconstruct the tuple
            if (isValid) 
            {
                string PreviousStatus = hidden_Label_PreviousStatus.Text;
            string DocStatus = "";
            string ProcessStatus = "";
            Axapta DynAx = Function_Method.GlobalAxapta();
            Button back = new Button();

            Button_Approve.Visible = false;
                Button_Print.Visible = true;
                Button_Reject.Visible = false;

            disableEdit();
            if (Submit_Process())
            {
                try
                {
                    DocStatus = "Approved";

                    //Submit_Process();

                    string SalesmanEmail = SalesReport_Get_Budget.getEmail(DynAx, hdSalesmanId.Value);
                    if (SalesmanEmail.StartsWith("@"))//head office, no validate user email
                    {
                        SalesmanEmail = Session["user_id"].ToString() + "@posim.com.my";
                    }
                    var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, hdSalesmanId.Value);
                    string HODemail = SalesReport_Get_Budget.getEmail(DynAx, tuple_GroupId_ReportingTo.Item2);

                    getDetailsAndSendemail(PreviousStatus, ProcessStatus, DocStatus, SalesmanEmail + "," + HODemail);


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

            btnOverview_Click(back, EventArgs.Empty);
            }
            else
            {
                Function_Method.MsgBox("Still have some fields need to fulfill. " + message, this.Page, this);

            }
        }

        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            disableEdit();
            Button_Proceed.Visible = false;
            Button_Approve.Visible = false;
            Button_Reject.Visible = false;

            Axapta DynAx = Function_Method.GlobalAxapta();
            GLOBAL.user_id = Session["user_id"].ToString();

            string EquipID = hidden_LabelEquipID.Text;
            string PreviousStatus = hidden_Label_PreviousStatus.Text;

            int DocStatus = 7;//Rejected
            string Process_Status = "";
            string SEProcessStatus = "";
            string RejectReason = hfRejectReason.Value;
            string txtDocStatus = "";
            string ProcessStatus = "";
            var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, hdSalesmanId.Value);
            string HODemail = SalesReport_Get_Budget.getEmail(DynAx, tuple_GroupId_ReportingTo.Item2);
            if (EquipID != "")
            {
                try
                {
                    txtDocStatus = "Rejected";
                    string SalesmanEmail = SalesReport_Get_Budget.getEmail(DynAx, hdSalesmanId.Value);
                    if (SalesmanEmail.StartsWith("@"))//head office, no validate user email
                    {
                        SalesmanEmail = Session["user_id"].ToString() + "@posim.com.my";
                    }
                    SEProcessStatus = getSignboardEquipmentProcessStatus(DynAx, EquipID);
                    if (PreviousStatus == "AwaitingHOD")
                    {
                        Process_Status = SEProcessStatus + "</br> HOD: reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "AwaitingAnP")
                    {
                        Process_Status = SEProcessStatus + "</br> AnP: reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "AwaitingSalesAdmin")
                    {
                        Process_Status = SEProcessStatus + "</br> Sales Admin: reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "AwaitingSalesAdminMgr")
                    {
                        Process_Status = SEProcessStatus + "</br> Sales Admin Manager: reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "AwaitingGM")
                    {
                        Process_Status = SEProcessStatus + "</br> GM: reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    else if (PreviousStatus == "Approved")
                    {
                        Process_Status = SEProcessStatus + "</br> Reject by " + GLOBAL.user_id + " on " + DateTime.Now + "\n";
                    }
                    Function_Method.UserLog(Process_Status);
                    logger.Info($"Process Status for Equipment ID {EquipID}: {Process_Status}");
                    //getDetailsAndSendemail(PreviousStatus, Process_Status, ClaimStatus, SalesmanEmail + "," + HODemail + ",tancy1@posim.com.my", "\nReject Reason: " + RejectReason);

                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                    {
                        DynAx.TTSBegin();

                        if (EquipID != "")
                        {
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Equip_ID", EquipID));
                            if (DynRec.Found)
                            {
                                //DynRec.set_Field("InventLocationId", "HO");

                                //if (RejectReason != "") DynRec.set_Field("RejectReason", RejectReason);
                                DynRec.set_Field("DocStatus", DocStatus);
                                if (Process_Status != "") DynRec.set_Field("ProcessStatus", Process_Status);

                                DynRec.Call("Update");
                                logger.Info($"Updated DocStatus for Equipment ID: {EquipID} to {DocStatus}");
                            }
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }

                    getDetailsAndSendemail(PreviousStatus, ProcessStatus, txtDocStatus, SalesmanEmail + "," + HODemail);

                    Function_Method.MsgBox("Equipment ID: " + EquipID + " have been rejected.", this.Page, this);
                    logger.Info($"Equipment ID: {EquipID} has been rejected successfully.");

                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('btnOverview'); ", true);//go to overview
                }
                catch (Exception ER_SE_15)
                {
                    logger.Error($"Error rejecting Equipment ID {EquipID}: {ER_SE_15}");
                    Function_Method.MsgBox("ER_SE_15: " + ER_SE_15.ToString(), this.Page, this);
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
            //submit();
        }

        protected void Button_CreateReturn_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class

            string ClaimNumber = hidden_LabelEquipID.Text;
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

                    if (hidden_LabelEquipID.Text != "")
                    {
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", hidden_LabelEquipID.Text));
                        if (DynRec.Found)
                        {
                            //if (ClaimStatus != "") DynRec.set_Field("ClaimStatus", ClaimStatus);
                            if (Process_Status != "") DynRec.set_Field("ProcessStatus", Process_Status);
                            if (CreateSalesDate != "") DynRec.set_Field("SOCreatedDate", CreateSalesDate);
                            DynRec.Call("Update");
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
                        axSalesTable.Call("parmCustomerRef", hidden_LabelEquipID.Text);
                        axSalesTable.Call("parmSalesType", 4);// return item
                        axSalesTable.Call("save");
                        var salesID = axSalesTable.Call("parmSalesID");
                        hdSoNumber.Value = salesID.ToString();

                        //if (ddlClaimType.SelectedValue == "1")//battery
                        //{
                        //    //if (PreProcess_Warranty_SaveSalesOrder(Label_ItemID.Text))
                        //    //{
                        //    //    SaveSalesLine(axSalesLine, Label_ItemID.Text);
                        //    //    axSalesLine.Call("parmSalesId", salesID.ToString());
                        //    //    axSalesLine.Call("parmItemId", Label_ItemID.Text);
                        //    //    axSalesLine.Call("parmName", Label_BatteryName.Text);
                        //    //    axSalesLine.Call("SuffixCode", "/RT");
                        //    //    axSalesLine.Call("parmSalesUnit", DropDownList_Unit.SelectedItem.Text);
                        //    //    axSalesLine.Call("parmExpectedRetQty", "-" + TextBox_Quantity.Text);
                        //    //    axSalesLine.Call("parmSalesQty", "-" + TextBox_Quantity.Text);
                        //    //    axSalesLine.Call("parmShippingDateRequested", todayDate);
                        //    //    axSalesLine.Call("LF_SerialNumber", TextBox_SerialNo.Text);
                        //    //    axSalesLine.Call("parmReturnDeadLine", todayDate);
                        //    //    axSalesLine.Call("CNInvId", TextBox_CustomerDO.Text);//for draft cn purposes
                        //    //    axSalesLine.Call("dosave");
                        //    //}
                        //}
                        ////lubricant=============================================================
                        //else if (ddlClaimType.SelectedValue == "2")
                        //{
                        //    //if (PreProcess_Warranty_SaveSalesOrder(Label_ItemID_Item.Text))
                        //    //{
                        //    //    SaveSalesLine(axSalesLine, Label_ItemName_Item.Text);
                        //    //    axSalesLine.Call("parmSalesId", salesID.ToString());
                        //    //    axSalesLine.Call("parmItemId", Label_ItemID_Item.Text);
                        //    //    axSalesLine.Call("parmName", Label_ItemName_Item.Text);
                        //    //    axSalesLine.Call("SuffixCode", "/RT");
                        //    //    axSalesLine.Call("parmSalesUnit", DropDownList_Unit_Items.SelectedItem.Text);
                        //    //    axSalesLine.Call("parmExpectedRetQty", "-" + TextBox_Quantity_Item.Text);
                        //    //    axSalesLine.Call("parmSalesQty", "-" + TextBox_Quantity_Item.Text);
                        //    //    axSalesLine.Call("parmShippingDateRequested", todayDate);
                        //    //    //axSalesLine.Call("LF_SerialNumber", TextBox_SerialNo.Text);
                        //    //    axSalesLine.Call("parmReturnDeadLine", todayDate);
                        //    //    axSalesLine.Call("CNInvId", TextBox_CustomerDO_Item.Text);//for draft cn purposes
                        //    //    axSalesLine.Call("dosave");
                        //    //}
                        //}
                        //batch return & warranty other products=============================================================
                        //else
                        //{
                        //    //var tuple_get_Batch_Item = get_Batch_Item();//get item id
                        //    //                                            //string[] Description_Batch = tuple_get_Batch_Item.Item1;
                        //    //                                            //string[] New_QTY_Batch = tuple_get_Batch_Item.Item2;
                        //    //string[] custDo = tuple_get_Batch_Item.Item4;
                        //    //string[] ItemId_Batch = tuple_get_Batch_Item.Item5;
                        //    //string[] SalesUnit = tuple_get_Batch_Item.Item7;
                        //    //TextBox_CustomerDO.Text = custDo[0];
                        //    //foreach (GridViewRow row in GridView_BatchItem.Rows)
                        //    //{
                        //    //    TextBox box1 = (TextBox)GridView_BatchItem.Rows[row.RowIndex].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        //    //    string itemName = box1.Text;
                        //    //    string temp_ItemId = WClaim_GET_NewApplicant.ItemNameGetItemId(DynAx, itemName);

                        //    //    TextBox qty = (TextBox)GridView_BatchItem.Rows[row.RowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        //    //    //TextBox price = (TextBox)GridView_BatchItem.Rows[row.RowIndex].Cells[5].FindControl("TextBox_Price");
                        //    //    int itemQty = Convert.ToInt16("-" + qty.Text);
                        //    //    //double itemPrice = Convert.ToDouble(price.Text);
                        //    //    if (PreProcess_Warranty_SaveSalesOrder(temp_ItemId))
                        //    //    {
                        //    //        string itemID = ItemId_Batch[row.RowIndex].ToString();
                        //    //        string _CustAccount = Label_Account.Text;
                        //    //        string _SalesUnit = SalesUnit[row.RowIndex];

                        //    //        var tuple_getMPP_MPG = SFA_GET_SALES_ORDER_2.get_MPP_MPG(DynAx, salesID.ToString());
                        //    //        string LF_MixPricePromo = tuple_getMPP_MPG.Item1;
                        //    //        string LF_MixPriceGroup = tuple_getMPP_MPG.Item2;

                        //    //        string axTaxItemGroup = SFA_GET_SALES_ORDER_2.get_TaxItemGroup(DynAx, itemID);
                        //    //        string LFUOM = SFA_GET_SALES_ORDER_2.get_UOM(DynAx, _SalesUnit);

                        //    //        AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");
                        //    //        string itemPrice = domComSalesLine.Call("getSalesPrice", itemID, _SalesUnit, todayDate, 1, Label_Account.Text, salesID).ToString();//fieldValue=Itemid

                        //    //        if (itemQty != 0)//New Qty
                        //    //        {
                        //    //            AxaptaObject axBatchSalesLine = DynAx.CreateAxaptaObject("AxSalesLine");
                        //    //            axBatchSalesLine.Call("InitSalesLine", salesID.ToString());

                        //    //            SaveSalesLine(axBatchSalesLine, itemID);
                        //    //            axBatchSalesLine.Call("parmReceiptDateRequested", todayDate);
                        //    //            axBatchSalesLine.Call("parmShippingDateRequested", todayDate);
                        //    //            axBatchSalesLine.Call("parmReturnDeadLine", todayDate);
                        //    //            axBatchSalesLine.Call("parmConfirmedDlv", todayDate);
                        //    //            axBatchSalesLine.Call("parmItemId", itemID);
                        //    //            axBatchSalesLine.Call("parmName", itemName);
                        //    //            axBatchSalesLine.Call("parmSalesUnit", _SalesUnit);
                        //    //            axBatchSalesLine.Call("parmSalesQty", itemQty);
                        //    //            axBatchSalesLine.Call("parmSalesPrice", itemPrice);
                        //    //            axBatchSalesLine.Call("suffixCode", "/RT");
                        //    //            axBatchSalesLine.Call("parmExpectedRetQty", itemQty);
                        //    //            axBatchSalesLine.Call("parmSalesId", salesID.ToString());
                        //    //            axBatchSalesLine.Call("parmSalesStatus", 2);//Delivered
                        //    //            axBatchSalesLine.Call("parmMixPriceGroup", LF_MixPriceGroup);
                        //    //            axBatchSalesLine.Call("parmUOM", LFUOM);
                        //    //            axBatchSalesLine.Call("parmMixPricePromo", LF_MixPricePromo);
                        //    //            axBatchSalesLine.Call("CNInvId", custDo[row.RowIndex]);//for draft cn purposes

                        //    //            if (axTaxItemGroup == "OS")
                        //    //            {
                        //    //                axBatchSalesLine.Call("parmTaxGroup", axTaxItemGroup);
                        //    //            }
                        //    //            if (hdInventId.Value != "")
                        //    //            {
                        //    //                axBatchSalesLine.Call("parmInventDimId", hdInventId.Value);
                        //    //            }

                        //    //            string poCost = ""; string manualCost = "";
                        //    //            if (GLOBAL.user_company == "PBM")
                        //    //            {
                        //    //                if (poCost != "")
                        //    //                {
                        //    //                    axBatchSalesLine.Call("POCost", poCost);
                        //    //                }
                        //    //                else
                        //    //                {
                        //    //                    axBatchSalesLine.Call("POCost", 0);
                        //    //                }
                        //    //                if (manualCost != "")
                        //    //                {
                        //    //                    axBatchSalesLine.Call("manualCost", manualCost);
                        //    //                }
                        //    //                else
                        //    //                {
                        //    //                    axBatchSalesLine.Call("manualCost", 0);
                        //    //                }
                        //    //            }
                        //    //            axBatchSalesLine.Call("dosave");
                        //    //            axBatchSalesLine.Call("CurrentRecord");
                        //    //            //axBatchSalesLine.Call("CurrentRecordId");//comment for temporary
                        //    //        }
                        //    //    }
                        //    //}
                        //}

                        //update warranty table
                        var rmaID = WClaim_GET_NewApplicant.get_RMAid(DynAx, salesID.ToString());
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                        {
                            DynAx.TTSBegin();

                            if (hidden_LabelEquipID.Text != "")
                            {
                                DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", hidden_LabelEquipID.Text));
                                if (DynRec.Found)
                                {
                                    DynRec.set_Field("RMAID", rmaID.Item1);
                                    DynRec.set_Field("RMADate", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                    DynRec.Call("Update");
                                }
                            }
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                    }
                    Function_Method.MsgBox("Warranty: " + ClaimNumber + ". Sales order created.", this.Page, this);

                    Button toApproval = new Button();
                    // Call the SelectedIndexChanged event handler method

                    btnOverview_Click(toApproval, EventArgs.Empty);
                    ActionButtons_Click(toApproval.Text = "Inspection", EventArgs.Empty);
                }
                catch (Exception ER_SE_20)
                {
                    Function_Method.MsgBox("ER_SE_20: " + ER_SE_20.ToString(), this.Page, this);
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


        protected void Button_Search_Overview_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            switch (DropDownList_Search_Overview.SelectedItem.Text.Trim())
            {
                case "Claim Number":
                    fieldName = "ClaimNumber";
                    break;
                case "Customer Account No.":
                    fieldName = "CustAccount";//CUSTACCOUNT
                    break;
                case "Customer Name":
                    fieldName = "CustName";
                    break;
                default:
                    fieldName = "";
                    break;
            }

            SignboardEquipment_Overview(0, fieldName);
        }

        protected void Button_Print_Click(object sender, EventArgs e)
        {
            string url = $"SignBoardForm.aspx?tempSignboardNo={lblEquipID.Text}";
            GlobalHelper.RedirectToNewTab(url, "_blank", "menubar=1000,width=1000,height=1000");
        }
        protected void SaveSalesLine(AxaptaObject axSalesLine, string ItemID)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();//base class


        }

        protected void disableEdit()
        {
            if (GLOBAL.user_id != GLOBAL.AdminID)
            { 
                bool check = false;
                Axapta DynAx = Function_Method.GlobalAxapta();
                string ProcessedUserName = Regex.Replace(Session["logined_user_name"].ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
                string SalesAdminName = Regex.Replace(getSignboardEquipmentSalesAdminName(DynAx, hidden_LabelEquipID.Text).ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
                #region Check SalesAdmin

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();

                string SQL = "select * from signboardequipmentappgroup where Type = @p0 and Company = @p1";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                cmd.Parameters.AddWithValue("@p0", rblES.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@p1", Session["user_company"].ToString());

                MySqlDataReader reader = cmd.ExecuteReader();

                if ((bool)reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        SalesAdminName = Regex.Replace(reader.GetString("SalesAdmin"), @"\s+", "").ToLower().Trim().Split('(')[0];
                    }
                }
                conn.Close();
                #endregion

                if (ProcessedUserName == SalesAdminName)
                {
                    check = true;
                }
                Button_Submit.Visible = false;
                Button_Reject.Visible = false;
                Button_Print.Visible = true;
                if (!check)
                {
                    Button_Proceed.Visible = false;
                    Label_Account.Enabled = false;
                    rblES.Enabled = false;
                    //batch item
                    Button_AddBatchItem.Enabled = false;
                    GridView_BatchItem.Enabled = false;

                    //DropDownList_TransportationCoordinator.Enabled = false;
                    //DropDownList_GoodReceivePerson.Enabled = false;
                    Button_AddBatchItem.Enabled = false;
                    Button_CustomerMasterList.Enabled = false;

                    new_applicant_sectionDeliveryDetails.Visible = false;
                    lblTelNo.Enabled = false;
                    lblContactPerson.Enabled = false;
                    ddlRequestType.Enabled = false;
                    ddlItemDeliveryTo.Enabled = false;
                    txtDeliveryAddress.Enabled = false;
                    ddlIssueTo.Enabled = false;
                    txtRemarks.Enabled = false;
                    rblRequestSign.Enabled = false;
                    txtMapDescription.Enabled = false;
                    textTrafficDensity.Enabled = false;
                    txtMapRemark.Enabled = false;
                    textSignboardVisibility.Enabled = false;
                    cbPhoneSubmission.Enabled = false;
                    txtTypeServiceCenter.Enabled = false;
                    txtWorkshopFacilities.Enabled = false;
                    txtWorkshopSizeType.Enabled = false;
                    txtOwnerExperience.Enabled = false;
                    rblWorkshopStatus.Enabled = false;
                    txtNumberOfMechanics.Enabled = false;
                    txtYearOfEstablishment.Enabled = false;
                    rblApplicationType.Enabled = false;
                    divMainRemarks.Visible = false;
                    txtSBWorkshopName.Enabled = false;
                    TextBox_TotalCost.Enabled = false;
                    //txtSubDBRemark.Enabled = false;
                    //txtSubDBAddress.Enabled = false;
                    //rblSubDBType.Enabled = false;
                    //upload_section.Visible = false;
                }
                DynAx.Dispose();
            }
        }

        protected void EnableEdit()
        {
            bool check = false;
            Axapta DynAx = Function_Method.GlobalAxapta();
            string ProcessedUserName = Regex.Replace(Session["logined_user_name"].ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
            string SalesAdminName = Regex.Replace(getSignboardEquipmentSalesAdminName(DynAx, hidden_LabelEquipID.Text).ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];


            #region Check SalesAdmin

            if (rblES.SelectedItem != null)
            {

                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();

                string SQL = "select * from signboardequipmentappgroup where Type = @p0 and Company = @p1";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                cmd.Parameters.AddWithValue("@p0", rblES.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@p1", Session["user_company"].ToString());

                MySqlDataReader reader = cmd.ExecuteReader();

                if ((bool)reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        SalesAdminName = Regex.Replace(reader.GetString("SalesAdmin"), @"\s+", "").ToLower().Trim().Split('(')[0];
                    }
                }
                conn.Close();

                if (ProcessedUserName == SalesAdminName)
                {
                    check = true;
                }
                if (!check)
                {
                    Button_Proceed.Visible = false;
                }
            }
            #endregion

            Button_Submit.Visible = true;
            Label_Account.Enabled = true;
            rblES.Enabled = true;
            //batch item
            Button_AddBatchItem.Enabled = true;
            GridView_BatchItem.Enabled = true;
            //DropDownList_TransportationCoordinator.Enabled = true;
            //DropDownList_GoodReceivePerson.Enabled = true;
            Button_AddBatchItem.Enabled = true;
            Button_CustomerMasterList.Enabled = true;

            Accordion_DeliveryDetails.Visible = true;
            new_applicant_sectionDeliveryDetails.Visible = true;
            divForApproval.Visible = true;

            lblTelNo.Enabled = true;
            lblContactPerson.Enabled = true;
            ddlRequestType.Enabled = true;
            ddlItemDeliveryTo.Enabled = true;
            txtDeliveryAddress.Enabled = true;
            ddlIssueTo.Enabled = true;
            txtRemarks.Enabled = true;
            rblRequestSign.Enabled = true;
            txtMapDescription.Enabled = true;
            textTrafficDensity.Enabled = true;
            txtMapRemark.Enabled = true;
            textSignboardVisibility.Enabled = true;
            cbPhoneSubmission.Enabled = true;
            txtTypeServiceCenter.Enabled = true;
            txtWorkshopFacilities.Enabled = true;
            txtWorkshopSizeType.Enabled = true;
            txtOwnerExperience.Enabled = true;
            rblWorkshopStatus.Enabled = true;
            txtNumberOfMechanics.Enabled = true;
            txtYearOfEstablishment.Enabled = true;
            txtSBWorkshopName.Enabled = true;
            rblApplicationType.Enabled = true;
            TextBox_TotalCost.Enabled = true;
            DynAx.Dispose();

        }

        private void HideRepeaterButtons(Repeater repeater, string buttonId)
        {
            if (repeater != null && repeater.Items.Count > 0)
            {
                foreach (RepeaterItem item in repeater.Items)
                {
                    Button btn = (Button)item.FindControl(buttonId);
                    if (btn != null)
                    {
                        btn.Visible = false;
                    }
                }
            }
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
                //btnAmend.Visible = false;
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

        }

        protected void DropDownList_ReasonReturn_OtherProducts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void rblES_SelectedIndexChanged(object sender, EventArgs e)
        {
            SectionVisibility();
        }

        private void SectionVisibility()
        {
            DetailsWrapper.Visible = false;
            SignboardWrapper.Visible = false;
            SignboardApplicationType.Visible = false;
            EquipmentContractDuration.Visible = false;
            SignageLicenseStatusWrapper.Visible = false;
            PastMonthPurchaseRecordWrapper.Visible = false;
            LocationMapWrapper.Visible = false;
            lblDepositforEOR.Text = rblES.SelectedItem.Text;

            if (rblES.SelectedValue == "2")
            {
                EquipmentContractDuration.Visible = true;
                DetailsWrapper.Visible = true;
                PastMonthPurchaseRecordWrapper.Visible = true;
            }
            else if (rblES.SelectedValue == "1")
            {
                SignboardWrapper.Visible = true;
                SignboardApplicationType.Visible = true;
                DetailsWrapper.Visible = true;
                SignageLicenseStatusWrapper.Visible = true;
                PastMonthPurchaseRecordWrapper.Visible = true;
                LocationMapWrapper.Visible = true;
            }
        }

        private void DropDownList()
        {
            List<ListItem> warehouse = new List<ListItem>();
            warehouse = WClaim_GET_NewApplicant.getWarrantyWarehouse();
        }

        protected void rblTransport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rblTransport.SelectedValue == "1")
            //{
            //    Accordion_TransportationArrangement.Visible = true;
            //    new_applicant_sectionTransportationArrangement.Visible = true;
            //}
            //else
            //{
            //    Accordion_TransportationArrangement.Visible = false;
            //    new_applicant_sectionTransportationArrangement.Visible = false;
            //}
        }


        //03/03/2025 KX Reduce redundant coding
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
                    GridViewOverviewList.Visible = true;

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
                case "Awaiting HOD":
                    btnListHOD.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "4";

                    break;
                case "Awaiting Sales Admin":
                    btnListSalesAdmin.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "5";

                    break;
                case "Awaiting AnP":
                    btnListAnP.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "6";

                    break;
                case "Awaiting Sales Admin Mgr":
                    btnListSalesAdminMgr.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "7";

                    break;
                case "Awaiting GM":
                    btnListGM.Attributes.Add("style", "background-color:#f58345");
                    ddlStatus.SelectedItem.Value = "8";

                    break;
                case "Check Last Outstanding Invoice":
                    PumpModalData();

                    return;
            }

            if (s[0].Trim() != "Check Last Outstanding Invoice")
            {
                f_Button_ListAll();
            }
        }

        private void PumpModalData()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            
            #region Custable
            string TableName = "CustTable";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "AccountNum");

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var pending = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//claimstat
            pending.Call("value", Label_CustAcc.Text);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                modalSalesmanCode.Text = DynRec1.get_Field("EmplName").ToString() + " (" + DynRec1.get_Field("EmplId").ToString() + ")";
                modalCustomerCode.Text = DynRec1.get_Field("Name").ToString() + " (" + DynRec1.get_Field("AccountNum").ToString() + ")";
                modalCustClass.Text = DynRec1.get_Field("CustomerClass").ToString();
                modalContactPerson.Text = DynRec1.get_Field("ContactPersonId").ToString();
                modalCustGroup.Text = DynRec1.get_Field("CustomerMainGroup").ToString();
                modalTelephone.Text = DynRec1.get_Field("Phone").ToString();
                modalCustType.Text = DynRec1.get_Field("CustomerType").ToString();
                var AccOpenDate = DynRec1.get_Field("OpeningAccDate");
                modalAccOpenDate.Text = AccOpenDate != null ?
                Convert.ToDateTime(AccOpenDate).ToString("dd/MM/yyyy") : "";
                modalCreditLimit.Text = "RM " + DynRec1.get_Field("CreditMax").ToString();
                var IncorpDate = DynRec1.get_Field("IncorpDate");
                modalIncorporationDate.Text = IncorpDate != null ?
                Convert.ToDateTime(IncorpDate).ToString("dd/MM/yyyy") : "";
            }
            #endregion

            GridView1.PageIndex = 0;
            GridView1.Visible = true;

            SignboardEquipment_Details(0);

            #region LF_EOR_Master
            int LF_EOR_Master = DynAx.GetTableId("LF_EOR_Master");
            int AccountNum = DynAx.GetFieldId(tableId, "AccountNum");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_EOR_Master);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", AccountNum);
            qbr1.Call("value", Label_CustAcc.Text);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_EOR_Master);

                var custExpiryDate = DynRec1.get_Field("EXPDATE");
                modalExpiry.Text = custExpiryDate != null ?
                Convert.ToDateTime(custExpiryDate).ToString("dd/MM/yyyy") : "";

                var commenceDate = DynRec1.get_Field("COMDATE");
                modalCommence.Text = commenceDate != null ?
                Convert.ToDateTime(commenceDate).ToString("dd/MM/yyyy") : "";

                modalTargetCarton.Text = DynRec1.get_Field("TGTCTN").ToString();
                modalEORTarget.Text = DynRec1.get_Field("GrandTargetCarton").ToString();
            }
            #endregion

            #region CustAgingTable
            var custAging = GetCustAging(DynAx, Label_CustAcc.Text);
            modalAvgPayment6Mth.Text = Convert.ToDouble(custAging.AvgPaymDays.ToString("#,###,###,##0.00")) + "(" + Convert.ToDouble(custAging.AvgPaymDays2.ToString("#,###,###,##0.00")) + ")";
            //modalAvrgMonthlySales6Mth.Text = custAging.AvgMonthlySales.ToString("#,###,###,##0.00");
            modalApprovalForCredit.Text = custAging.ApplyCredit.ToString();
            modalBankGuarantee.Text = custAging.BankCorpGuaranteeAmt;
            modalBankExpiryDate.Text = custAging.BankCorpExpiryDate;
            modalPostedDatedChequeTotal.Text = custAging.PostDatedChqTotal.ToString();
            modalLastReturnCheque.Text = custAging.LastReturnCheqAmt;
            modalOverdueInterest.Text = custAging.OverDueInterest.ToString();
            modalLastMonthCollection.Text = custAging.LastMonthCollection;
            modalCurrentMonthCollection.Text = custAging.CurrentMonthCollection;
            #endregion
            DynAx.Dispose();
        }

        protected void transparentButtonAttribute()
        {
            //03/03/25 KX If any extra action button, just add inside

            //btnListSalesOrder.Attributes.Add("style", "background-color:#transparent");
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListHOD.Attributes.Add("style", "background-color:#transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:#transparent");
            btnListAnP.Attributes.Add("style", "background-color:#transparent");
            btnListSalesAdminMgr.Attributes.Add("style", "background-color:#transparent");
            btnListGM.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
        }

        protected void btnListSalesOrder_Click(object sender, EventArgs e)
        {
            //btnListSalesOrder.Attributes.Add("style", "background-color:#f58345");
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListHOD.Attributes.Add("style", "background-color:#transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:#transparent");
            btnListAnP.Attributes.Add("style", "background-color:#transparent");
            btnListSalesAdminMgr.Attributes.Add("style", "background-color:#transparent");
            btnListGM.Attributes.Add("style", "background-color:#transparent");
            ddlStatus.SelectedItem.Value = "10";
            //f_Button_ListAll();
        }

        protected void txtboxListAll_TextChanged(object sender, EventArgs e)
        {
            btnListAll.Attributes.Add("style", "background-color:#transparent");
            btnListReject.Attributes.Add("style", "background-color:#transparent");
            btnListApproved.Attributes.Add("style", "background-color:#transparent");
            btnListDraft.Attributes.Add("style", "background-color:#transparent");
            btnListHOD.Attributes.Add("style", "background-color:#transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:#transparent");
            btnListAnP.Attributes.Add("style", "background-color:#transparent");
            btnListSalesAdminMgr.Attributes.Add("style", "background-color:#transparent");
            btnListGM.Attributes.Add("style", "background-color:#transparent");
            SignboardEquipment_Overview(0, "");
        }


        #region report 

        protected void ReportActionButtons_Click(object sender, EventArgs e)
        {
            string s = (sender as Button).Text.Trim();


        }
        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta(); 
            // Usage:
            string message; // Declare the variable to receive the out parameter
            bool isValid = CheckValidation(out message);
            if (isValid)
            {
                disableEdit();
                Button_Submit.Visible = false;
                Submit_Process();

                Function_Method.MsgBox("Submitted Successfully.", this.Page, this);

                f_Button_ListAll();
            }
            else
            {
                Function_Method.MsgBox("Still have some fields need to fulfill. " + message, this.Page, this);
            }
            DynAx.Dispose();
        }

        protected void Button_Proceed_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            // Usage:
            string message = ""; // Declare the variable to receive the out parameter
            //bool isValid = CheckValidation(out message); 
            bool isValid = true;
            if (isValid)
            {
                disableEdit();
                hidden_Label_PreviousStatus.Text = "Update";
                if (hidden_LabelNextApprover.Text != "")
                {
                    Submit_Process();
                }
                Function_Method.MsgBox("Updated Successfully.", this.Page, this);

                f_Button_ListAll();
            }
            else
            {
                Function_Method.MsgBox("Still have some fields need to fulfill. " + message, this.Page, this);
            }
            DynAx.Dispose();
        }

        #region Validation
        private bool CheckValidation(out string message)
        {
            bool isValidate = true;
            message = "";
            // Reset all input controls font weight to normal before validating
            //Neil Fix
            if (rblApplicationType.SelectedIndex < 0 && rblES.SelectedIndex == 1)
            {
                isValidate = false;
                message = "Please select the Application Type.";
            } //Application Type

            if (rblRequestSign.SelectedIndex < 0)
            {
                isValidate = false;
                message = "Please select the Signage License Status.";
            }  //Signage License Status

            if (rblWorkshopStatus.SelectedIndex < 0)
            {
                isValidate = false;
                message = "Please select the Workshop Status.";
            } //Workshop Status

            if (string.IsNullOrEmpty(ddlItemDeliveryTo.Text))
            {
                isValidate = false;
                message = "Please select the Item Delivery To.";
            } //Item Delivery To

            if (string.IsNullOrEmpty(txtDeliveryAddress.Text))
            {
                isValidate = false;
                message = "Please fill in the Delivery Address.";
            } //txtDeliveryAddress

            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                isValidate = false;
                message = "Please fill in the Remarks.";
            }

            if (string.IsNullOrEmpty(txtMapDescription.Text) && rblES.SelectedIndex == 1)
            {
                isValidate = false;
                message = "Please fill in the Map Description.";
            }

            if (string.IsNullOrEmpty(txtWorkshopFacilities.Text))
            {
                isValidate = false;
                message = "Please fill in the Workshop Facilities.";
            }
            if (string.IsNullOrEmpty(txtOwnerExperience.Text))
            {
                isValidate = false;
                message = "Please fill in the Owner Experience.";
            }
            if (string.IsNullOrEmpty(txtWorkshopSizeType.Text))
            {
                isValidate = false;
                message = "Please fill in the Workshop Size Type.";
            }
            if (string.IsNullOrEmpty(txtNumberOfMechanics.Text))
            {
                isValidate = false;
                message = "Please fill in the Number Of Mechanics.";
            }
            if (string.IsNullOrEmpty(txtYearOfEstablishment.Text))
            {
                isValidate = false;
                message = "Please fill in the Year Of Establishment.";
            }
            if (string.IsNullOrEmpty(ddlRequestType.Text) || ddlRequestType.Text == "--Please Select--")
            {
                isValidate = false;
                message = "Please select the Request Type.";
            }

            return isValidate;
        }

        private bool CheckValidation_SalesAdmin(out string message)
        {
            bool isValidate = true;
            message = "";
            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                isValidate = false;
                message = "Please fill in the Remarks.";
            }

            if (ddlStatus.SelectedItem.Value == "2")
            { //only sales Admin makesure must submit image 
                // Check if files have been uploaded
                HttpFileCollection uploadedFiles = Request.Files; // Get the uploaded files
                bool filesUploaded = false;
                // Check if any files are uploaded
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    if (uploadedFiles[i].ContentLength > 0) // Check if the file has content
                    {
                        filesUploaded = true;
                        break; // Exit the loop if at least one file is found
                    }
                }


                if (!pdf_section.Visible && !display_section.Visible)
                {
                    if (!filesUploaded)
                    {
                        isValidate = false;
                        message = "Please upload files evidences. (.pdf/.jpeg/.png/.jpg)";

                    }
                }
            }
            return isValidate;
        }
        #endregion

        protected void datagrid_PageIndexChanging_Overview(object sender, GridViewPageEventArgs e)
        {
            SignboardEquipment_Overview(e.NewPageIndex, "");

            if (!string.IsNullOrEmpty(Label_CustAcc.Text.Trim()))
            {
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataBind();
            }

            GridViewOverviewList.PageIndex = e.NewPageIndex;
            GridViewOverviewList.DataBind();
        }

        private void SignboardEquipment_Details(int PAGE_INDEX)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Button_Overview_accordion.Visible = true;
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int CustTransOpen = DynAx.GetTableIdWithLock("CustTransOpen");

                int custAccount = DynAx.GetFieldId(CustTransOpen, "AccountNum");
                string EquipID = "";

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTransOpen);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", custAccount);//claimstat
                qbr.Call("value", Label_CustAcc.Text);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 8;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Voucher"; N[2] = "InvoiceNo"; N[3] = "Date";
                N[4] = "AmountCurrency"; N[5] = "Currency"; N[6] = "DueDate"; N[7] = "TotalOutStandingDay";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTransOpen);

                    /////This section is get from Redemption_Outbalance.aspx.cs line 244
                    #region Temporary_Data
                    string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                    string temp_DueDate = DynRec.get_Field("DueDate").ToString();
                    string TransDate = ""; string DueDate = "";
                    if (temp_TransDate != "")
                    {
                        string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                        string Raw_TransDate = arr_temp_TransDate[0];
                        TransDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_TransDate, true);
                    }
                    if (temp_DueDate != "")
                    {
                        string[] arr_temp_DueDate = temp_DueDate.Split(' ');//date + " " + time;
                        string Raw_DueDate = arr_temp_DueDate[0];
                        DueDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DueDate, true);
                    }

                    string TodayDate = DateTime.Now.ToString("dd/MM/yyyy");//default
                    TodayDate = Function_Method.get_correct_date(GLOBAL.system_checking, TodayDate, false);
                    //==========================================================================
                    //for extra filter purpose since CustTrans do not have Axapta control authorization

                    string temp_Cust_Acc = DynRec.get_Field("AccountNum").ToString().Trim();
                    string temp_Cust_Name = SFA_GET_Enquiries_BatteryOutstanding.getCust(DynAx, temp_Cust_Acc);
                    //==========================================================================
                    string str_AmountCur = DynRec.get_Field("AmountCur").ToString();
                    #endregion

                    countA = countA + 1;
                    //if (countA >= startA && countA <= endA)
                    //{
                    row = dt.NewRow();

                    string InvoiceNo = "";
                    string Voucher = "";
                    //string TransDate = (DateTime.Parse(DynRec.get_Field("TransDate").ToString()) == new DateTime(1900, 1, 1) ? "" : DynRec.get_Field("TransDate").ToString());
                    //string DueDate = (DateTime.Parse(DynRec.get_Field("DueDate").ToString()) == new DateTime(1900, 1, 1) ? "" : DynRec.get_Field("DueDate").ToString());
                    string AmountCurrency = DynRec.get_Field("AmountCur").ToString();
                    string Currency = "MYR";
                    string TotalOutStandingDay = "";

                    if (DateTime.Parse(DynRec.get_Field("TransDate").ToString()) != new DateTime(1900, 1, 1))
                    {
                        TotalOutStandingDay = (DateTime.Now.Date - DateTime.Parse(DynRec.get_Field("TransDate").ToString())).Days.ToString();
                    }

                    int CustTrans = DynAx.GetTableIdWithLock("CustTrans");

                    AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", CustTrans);

                    int RecID = DynAx.GetFieldId(CustTrans, "RecId");
                    var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", RecID);

                    qbr1.Call("value", DynRec.get_Field("RefRecId").ToString());
                    AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                    if ((bool)axQueryRun1.Call("next"))
                    {
                        AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTrans);
                        InvoiceNo = DynRec1.get_Field("Invoice").ToString();
                        Voucher = DynRec1.get_Field("Voucher").ToString(); ;

                        DynRec1.Dispose();
                    }
                    axQueryRun1.Dispose();
                    axQueryDataSource1.Dispose();

                    row["Voucher"] = Voucher;
                    row["InvoiceNo"] = InvoiceNo;
                    row["Date"] = TransDate;
                    row["AmountCurrency"] = AmountCurrency;
                    row["Currency"] = Currency;
                    row["DueDate"] = DueDate;
                    #region Total Outstanding Day

                    var var_TransDate = DateTime.ParseExact(TransDate, "dd/MM/yyyy", null);

                    var today = DateTime.Now.ToString("dd/MM/yyyy");
                    var var_DueDate = DateTime.ParseExact(DueDate, "dd/MM/yyyy", null);

                    double diff90 = (Convert.ToDateTime(today) - var_DueDate).TotalDays;
                    if (diff90 > 90)
                    {
                        Function_Method.MsgBox("Warning! There no Active transaction over the last 3 months for this Customer Account Number", this.Page, this);
                        return;
                    }

                    double diff2 = (var_TransDate - var_DueDate).TotalDays;

                    row["TotalOutStandingDay"] = diff2;
                    if (diff2 > 60)
                    {
                        lblDays.Text = diff2.ToString() + " days";
                    }
                    #endregion

                    dt.Rows.Add(row);
                    DynRec.Dispose();
                }
                //if (countA > endA)
                //{
                //    goto FINISH;//speed up process
                //}
                //}

                // Log off from Microsoft Dynamics AX.
                //FINISH:
                GridView1.PagerSettings.Visible = false;

                GridView1.VirtualItemCount = countA;

                GridView1.DataSource = dt;
                GridView1.DataBind();

            }
            catch (Exception ER_SE_13)
            {
                string message = "ER_SE_13: " + ER_SE_13.ToString();
                // Check if the exception is of type NotLoggedOnException
                if (ER_SE_13 is Microsoft.Dynamics.BusinessConnectorNet.NotLoggedOnException)
                {
                    message = "Your connection to Axapta has been interrupted. Please attempt to log in again to proceed.";
                }
                Function_Method.MsgBox(message, this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }


        private void SignboardEquipment_Overview(int PAGE_INDEX, string fieldName)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Button_Overview_accordion.Visible = true;
            Axapta DynAx = Function_Method.GlobalAxapta();

            CheckAndSetVisibility(DynAx, "Draft", btnListDraft);
            CheckAndSetVisibility(DynAx, "Awaiting HOD", btnListHOD);
            CheckAndSetVisibility(DynAx, "Awaiting Sales Admin", btnListSalesAdmin);
            CheckAndSetVisibility(DynAx, "AwaitingAnP", btnListAnP);
            CheckAndSetVisibility(DynAx, "Awaiting Sales Admin Mgr", btnListSalesAdminMgr);
            CheckAndSetVisibility(DynAx, "Awaiting GM", btnListGM);
            CheckAndSetVisibility(DynAx, "Approved", btnListApproved);
            CheckAndSetVisibility(DynAx, "Rejected", btnListReject);
            //CheckAndSetVisibility(DynAx, "Sales Order", btnListSalesOrder);

            try
            {
                int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");

                int claimStat = DynAx.GetFieldId(LF_WebEquipment, "DocStatus");
                int equipID = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
                int appliedDate = DynAx.GetFieldId(LF_WebEquipment, "AppliedDate");
                int custAccount = DynAx.GetFieldId(LF_WebEquipment, "CustAccount");
                string EquipID = "";

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WebEquipment);

                #region FilterAxapta
                string input = TextBox_Search_Overview.Text;
                if (input.EndsWith("/ES", StringComparison.OrdinalIgnoreCase))
                {
                    var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", equipID);

                    qbr1.Call("value", input);
                }
                else if (!string.IsNullOrEmpty(input) && input.All(char.IsDigit))
                {
                    var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", custAccount);

                    qbr2.Call("value", input);
                }
                else if (!string.IsNullOrEmpty(input))
                {
                    input = GlobalHelper.GET_CustID_From_CustName(DynAx, input);
                    var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", custAccount);

                    qbr2.Call("value", input);
                }

                #endregion

                if (string.IsNullOrEmpty(TextBox_Search_Overview.Text))
                {
                    switch (Convert.ToInt16(ddlStatus.SelectedItem.Value))
                    {
                        case 1:
                            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat); // claimstatus 
                            qbr.Call("value", "Draft");
                            break;

                        case 2:
                            var reject = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            reject.Call("value", "Approved");
                            break;

                        case 3:
                            var pending = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending.Call("value", "Rejected");
                            break;

                        case 4:
                            var pending1 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending1.Call("value", "Awaiting HOD");
                            break;

                        case 5:
                            var pending2 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending2.Call("value", "Awaiting Sales Admin");
                            break;

                        case 6:
                            var pending3 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending3.Call("value", "AwaitingAnP");
                            break;

                        case 7:
                            var pending4 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending4.Call("value", "Awaiting Sales Admin Mgr");
                            break;

                        case 8:
                            var pending5 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending5.Call("value", "Awaiting GM");
                            break;

                        default:
                            var pending8 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                            pending8.Call("value", "!Approved,!Rejected,!Draft");
                            break;
                    }
                }
                //if (!string.IsNullOrEmpty(Label_CustAcc.Text.Trim()))
                //{
                //    GridView1.DataSource = null;
                //    GridView1.DataBind();

                //    var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", custAccount);
                //    qbr.Call("value", Label_CustAcc.Text);
                //}

                axQueryDataSource.Call("addSortField", equipID, 1);
                axQueryDataSource.Call("addSortField", appliedDate, 1);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 16;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "EquipID"; N[2] = "CustomerAccount"; N[3] = "CustomerName";
                N[4] = "FormType"; N[5] = "DepositType"; N[6] = "DocStatus"; N[7] = "AppliedDate"; N[8] = "NextApprover"; N[9] = "AdminDate";
                N[10] = "AnPDate"; N[11] = "ManagerDate"; N[12] = "GMDate"; N[13] = "ProcessStatus"; N[14] = "AppliedBy"; N[15] = "EmplName";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WebEquipment);
                    string temp_DraftCN = DynRec.get_Field("DocStatus").ToString();
                    countA = countA + 1;
                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        string temp_CustAcc = DynRec.get_Field("CustAccount").ToString();
                        var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, temp_CustAcc);
                        string CustName = tuple_getCustInfo.Item1;
                        string SalesmanID = "";
                        string ReportTo = "";
                        if (CustName != "")
                        {
                            EquipID = DynRec.get_Field("Equip_ID").ToString();

                            if (CustName.ToLower().Contains(TextBox_Search_Overview.Text.ToLower()) ||
    TextBox_Search_Overview.Text == temp_CustAcc || TextBox_Search_Overview.Text == EquipID)
                            {
                                row["EquipID"] = EquipID;
                                row["CustomerAccount"] = temp_CustAcc;
                                row["CustomerName"] = CustName;
                                row["FormType"] = DynRec.get_Field("FormType").ToString();

                                string DocStatus = Enum.GetName(typeof(DocStatus), Int32.Parse(DynRec.get_Field("DocStatus").ToString()));

                                row["DocStatus"] = DocStatus;
                                string processStatus = DynRec.get_Field("ProcessStatus").ToString();


                                row["DepositType"] = DynRec.get_Field("DepositType").ToString();
                                row["ProcessStatus"] = processStatus;

                                int CustTable = DynAx.GetTableIdWithLock("CustTable");

                                AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                                AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", CustTable);

                                int custAcc = DynAx.GetFieldId(CustTable, "AccountNum");
                                var qbr = (AxaptaObject)axQueryDataSource2.Call("addRange", custAcc);

                                qbr.Call("value", temp_CustAcc);
                                AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

                                if ((bool)axQueryRun2.Call("next"))
                                {
                                    AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CustTable);
                                    SalesmanID = DynRec2.get_Field("EmplId").ToString();

                                    DynRec2.Dispose();
                                }
                                axQueryRun2.Dispose();
                                axQueryDataSource2.Dispose();

                                //int EmplTable = DynAx.GetTableIdWithLock("EmplTable");

                                //AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                                //AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

                                //int emplID = DynAx.GetFieldId(EmplTable, "EmplId");
                                //var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", emplID);

                                //qbr1.Call("value", SalesmanID);
                                //AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                                //if ((bool)axQueryRun1.Call("next"))
                                //{
                                //    AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                                //    ReportTo = DynRec1.get_Field("ReportTo").ToString();

                                //    DynRec1.Dispose();
                                //}
                                //axQueryRun1.Dispose();
                                //axQueryDataSource1.Dispose();

                                string temp_Img_Ind = DynRec.get_Field("Img_Ind").ToString();

                                if (temp_Img_Ind == "1")
                                {
                                    //row["Transport Required"] = "Yes";
                                    //if (processStatus == "Awaiting Transporter")
                                    //{
                                    //}
                                }
                                row["NextApprover"] = DynRec.get_Field("NextApprover").ToString();
                                row["AppliedBy"] = DynRec.get_Field("AppliedBy").ToString();
                                row["EmplName"] = DynRec.get_Field("EmplName").ToString();
                                #region AppliedDatenTime  
                                string createdDT = DynRec.get_Field("AppliedDate").ToString();
                                string appliedTime = DynRec.get_Field("AppliedTime").ToString();
                                DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                                if (parsedDateTime.Hour >= 12)
                                {
                                    parsedDateTime = parsedDateTime.AddHours(-12);
                                    string amOrPm = "PM";
                                    string formattedResultTime = parsedDateTime.ToString("dd/MM/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);

                                    // Check if AppliedTime exists and is not "0"
                                    if (!string.IsNullOrEmpty(appliedTime) && appliedTime != "0")
                                    {
                                        // Convert appliedTime from seconds to TimeSpan
                                        int totalSeconds;
                                        if (int.TryParse(appliedTime, out totalSeconds))
                                        {
                                            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
                                            // Combine date and time
                                            formattedResultTime = $"{formattedResultTime.Split(' ')[0]} {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2} AM"; // Assuming AM for simplicity
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid time format for AppliedTime.");
                                        }
                                    }

                                    row["AppliedDate"] = formattedResultTime;
                                }
                                else
                                {
                                    // Check if AppliedTime exists and is not "0"
                                    if (!string.IsNullOrEmpty(appliedTime) && appliedTime != "0")
                                    {
                                        int totalSeconds;
                                        if (int.TryParse(appliedTime, out totalSeconds))
                                        {
                                            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
                                            row["AppliedDate"] = parsedDateTime.ToString("dd/MM/yyyy") + $" {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2} AM"; // Assuming AM for simplicity
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid time format for AppliedTime.");
                                        }
                                    }
                                    else
                                    {
                                        row["AppliedDate"] = parsedDateTime;
                                    }
                                }
                                #endregion

                                row["AdminDate"] = (DateTime.Parse(DynRec.get_Field("AdminDate").ToString()) == new DateTime(1900, 1, 1) ? "" : DynRec.get_Field("AdminDate"));
                                row["AnPDate"] = (DateTime.Parse(DynRec.get_Field("AnPDate").ToString()) == new DateTime(1900, 1, 1) ? "" : DynRec.get_Field("AnPDate"));
                                row["ManagerDate"] = (DateTime.Parse(DynRec.get_Field("ManagerDate").ToString()) == new DateTime(1900, 1, 1) ? "" : DynRec.get_Field("ManagerDate"));
                                row["GMDate"] = (DateTime.Parse(DynRec.get_Field("GMDate").ToString()) == new DateTime(1900, 1, 1) ? "" : DynRec.get_Field("GMDate"));
                                dt.Rows.Add(row);
                                // Advance to the next row.
                                DynRec.Dispose();

                            }
                        }
                    }
                    //if (countA > endA)
                    //{
                    //    goto FINISH;//speed up process
                    //}
                }

                // Log off from Microsoft Dynamics AX.
                //FINISH:
                GridViewOverviewList.PagerSettings.Visible = true;

                GridViewOverviewList.VirtualItemCount = countA;

                GridViewOverviewList.DataSource = dt;
                //LblPendingBadge.Text = countA.ToString();
                GridViewOverviewList.DataBind();

                //if (!string.IsNullOrEmpty(Label_CustAcc.Text.Trim()))
                //{
                //    GridView1.PagerSettings.Visible = true;
                //    GridView1.VirtualItemCount = countA;
                //    GridView1.DataSource = dt;
                //    GridView1.DataBind();
                //}
            }
            catch (Exception ER_SE_13)
            {
                string message = "ER_SE_13: " + ER_SE_13.ToString();
                // Check if the exception is of type NotLoggedOnException
                if (ER_SE_13 is Microsoft.Dynamics.BusinessConnectorNet.NotLoggedOnException)
                {
                    message = "Your connection to Axapta has been interrupted. Please attempt to log in again to proceed.";
                }
                Function_Method.MsgBox(message, this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void CheckAndSetVisibility(Axapta DynAx, string status, Button button)
        {
            var totalClaim = getTotalClaimStatus(DynAx, status);
            string total = totalClaim.Item2.ToString();
            if (status == "Awaiting HOD")
            {
                if (total != "0")
                {
                    button.Text = "Awaiting HOD " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting Sales Admin")
            {
                if (total != "0")
                {
                    button.Text = "Awaiting Sales Admin " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "AwaitingAnP")
            {
                if (total != "0")
                {
                    button.Text = "Awaiting AnP " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting Sales Admin Mgr")
            {
                if (total != "0")
                {
                    button.Text = "Awaiting Sales Admin Mgr " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting GM")
            {
                if (total != "0")
                {
                    ////button.Text = "Awaiting GM " + "(" + totalClaim.Item2.ToString() + ")";
                    button.Text = "Awaiting GM ";
                }
            }
            else if (status == "Draft")
            {
                if (total != "0")
                {
                    //button.Text = "Draft " + "(" + totalClaim.Item2.ToString() + ")";
                    button.Text = "Draft ";
                }
            }
            else if (status == "Approved")
            {
                if (total != "0")
                {
                    //button.Text = "Approved " + "(" + totalClaim.Item2.ToString() + ")";
                    //Keegan - Request Do hide the total ie comment in the program 30/9/2025
                    button.Text = "Approved";
                }
            }
            else if (status == "Rejected")
            {
                if (total != "0")
                {
                    //button.Text = "Rejected " + "(" + totalClaim.Item2.ToString() + ")";
                    button.Text = "Rejected ";
                }
            }
        }

        private void f_Button_ListAll()
        {
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;
            GridViewOverviewList.Columns[2].Visible = true;

            GridViewOverviewList.Visible = true;

            //if (!string.IsNullOrEmpty(Label_CustAcc.Text.Trim()))
            //{
            //    GridView1.PageIndex = 0;
            //    GridView1.Columns[1].Visible = true;
            //    GridView1.Columns[2].Visible = true;

            //    GridView1.Visible = true;
            //}

            if (!CheckSalesAdmin_BySQL())
            {
                btnListAll.Visible = false;
                divPendingButton.Visible = false;
            }
            SignboardEquipment_Overview(0, "");
            TextBox_Search_Overview.Text = "";
        }

        public static Tuple<string, int> getTotalClaimStatus(Axapta DynAx, string claimStat)
        {
            string claimNum = ""; int count = 0;
            string TableName = "LF_WebEquipment";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "DocStatus");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", tableId);

            var pending = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//claimstat
            pending.Call("value", claimStat);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                claimNum = DynRec1.get_Field("Equip_ID").ToString();
                count++;
            }

            return new Tuple<string, int>(claimNum, count);
        }


        protected void Button_EquipId_Click(object sender, EventArgs e)
        {
            try
            {
                string selected_Id = "";
                Button Button_ClaimId = sender as Button;
                if (Button_ClaimId != null)
                {
                    selected_Id = Button_ClaimId.Text;
                    hidden_LabelEquipID.Text = selected_Id;
                    string ClientID = Button_ClaimId.ClientID;
                    string[] arr_ClientID = ClientID.Split('_');
                    int arr_count = arr_ClientID.Count();
                    //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                    GridViewRow row = (GridViewRow)Button_ClaimId.NamingContainer;
                    
                    Label lblCustomerAccount = (Label)row.FindControl("Label_CustomerAccount");
                    string CustomerAcc = lblCustomerAccount != null ? lblCustomerAccount.Text : string.Empty;

                    Label hidden_inventLocationId = (Label)row.FindControl("hidden_inventLocationId");
                    Label lblNextApprover = (Label)row.FindControl("Label_NextApprover");
                    if (hidden_inventLocationId != null)
                    {
                        hidden_inventLocationId.Text = lblNextApprover.Text;
                    }

                    Label lblDepositType = (Label)row.FindControl("Label_DepositType");
                    string DepositType = "";
                    if (lblDepositType.Text == "Signboard")
                    {
                        DepositType = "1";
                    }
                    else
                    {
                        DepositType = "2";
                    }


                    Label lblDocStatus = (Label)row.FindControl("Label_DocStatus");
                    string DocStatus = lblDocStatus != null ? lblDocStatus.Text : string.Empty;

                    string NextApproval = lblNextApprover.Text.Replace("&#160;", " ");

                    string Parameter = "@SESE_" + selected_Id + "|" + DocStatus + "|" + CustomerAcc + "|" +
                                                        DepositType + "|" + NextApproval;
                    Session["data_passing"] = Parameter;
                    //Response.Redirect("SignboardEquipment.aspx", true);
                    // Use JavaScript to open a new tab  
                    string url = "SignboardEquipment.aspx";
                    string script = $"window.open('{url}', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenNewTab", script, true);
                }
            }
            catch (Exception ER_SE_04)
            {
                Function_Method.MsgBox("ER_SE_04: " + ER_SE_04.ToString(), this.Page, this);
            }
        }

        private Tuple<List<string>, List<int>, int, List<string>, List<string>> get_Batch_Item()
        {
            int row_count_BatchItem = GridView_BatchItem.Rows.Count;
            List<int> New_QTY = new List<int>();
            List<string> Description = new List<string>();
            List<string> EuipSize = new List<string>();
            List<string> RecId = new List<string>();
            List<string> Batch_ItemId = new List<string>();
            //string[] Price = new string[row_count_BatchItem];

            if (row_count_BatchItem >= 1)
            {
                for (int i = 0; i < row_count_BatchItem; i++)
                {
                    if (GridView_BatchItem.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox box1 = (TextBox)GridView_BatchItem.Rows[i].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        TextBox Qty = (TextBox)GridView_BatchItem.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                        //TextBox getPrice = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_Price");
                        TextBox Size = (TextBox)GridView_BatchItem.Rows[i].Cells[4].FindControl("TextBox_New_Size");

                        if (box1.Text.ToString() != "")
                        {
                            Description.Add(box1.Text.ToString());
                        }

                        if (Qty.Text.ToString() != "")
                        {
                            New_QTY.Add(Int32.Parse(Qty.Text));
                        }

                        if (Size.Text.ToString() != "")
                        {
                            EuipSize.Add(Size.Text.ToString());
                        }
                    }
                }
                //Batch_ItemId = get_Batch_ItemId(Description, RecId, row_count_BatchItem);
            }
            return new Tuple<List<string>, List<int>, int, List<string>, List<string>>
                (Description, New_QTY, row_count_BatchItem, EuipSize, Batch_ItemId);
        }

        private bool Submit_Process()
        {
            bool IsApproved = false;
            Axapta DynAx = Function_Method.GlobalAxapta();

            SignboardEquipmentModel m = new SignboardEquipmentModel();

            string userid = Session["user_id"].ToString();
            var getHodId = Quotation_Get_Sales_Quotation.get_Empl_Id(DynAx, Session["user_id"].ToString() + "@posim.com.my");//salesapprovalgroupid
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);

            if (!string.IsNullOrEmpty(getHodId.Item1.ToString()))
            {
                Session["hod"] = UserName;
            }


            SignboardEquipmentAppGroupModel vm = new SignboardEquipmentAppGroupModel();

            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            conn.Open();

            string SQL = "select * from signboardequipmentappgroup where Type = @p0 and Company = @p1";

            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@p0", rblES.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@p1", Session["user_company"].ToString());

            MySqlDataReader reader = cmd.ExecuteReader();

            if ((bool)reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    vm.Type = reader.GetString("Type");
                    vm.Company = reader.GetString("Company");
                    vm.Approval = reader.GetString("Approval");
                    vm.SalesAdmin = reader.GetString("SalesAdmin");
                    vm.AltSalesAdmin = reader.GetString("AltSalesAdmin");
                    vm.SalesAdminManager = reader.GetString("SalesAdminManager");
                    vm.GM = reader.GetString("GM");
                }
            }
            conn.Close();

            string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, hdSalesmanId.Value);
            string[] arr_NA_HOD = getHod.Split('_');
            int count_arr_NA_HOD = arr_NA_HOD.Count();
            string Approver = "";

            double parsedValue; // Declare the variable before the TryParse call
            if (string.IsNullOrWhiteSpace(TextBox_TotalCost.Text) || !Double.TryParse(TextBox_TotalCost.Text, out parsedValue))
            {
                m.Cost = 0.0; // Assign 0.0 if the text box is empty or the input is not a valid double
            }
            else
            {
                m.Cost = parsedValue; // Assign the parsed value if it's valid
            }
            m.AppType = (string.IsNullOrEmpty(rblApplicationType.SelectedValue) ? 0 : Int32.Parse(rblApplicationType.SelectedValue));
            m.CustAccount = Label_CustAcc.Text;
            m.CustPhone = lblTelNo.Text;
            m.CustContact = lblContactPerson.Text;
            m.DelPerson = ddlIssueTo.SelectedItem.Text;
            m.Remarks = txtRemarks.Text;
            m.DeliveryTo = ddlItemDeliveryTo.SelectedItem.Text;
            m.Address = txtDeliveryAddress.Text;
            m.AppliedBy = GLOBAL.logined_user_name;
            m.EmplName = Label_Salesman.Text;
            m.AppliedDate = DateTime.Now;
            m.AppliedTime = DateTime.Now.ToString("hh:mm:ss");
            m.FormType = ddlRequestType.SelectedItem.Text;
            m.DepositType = rblES.SelectedItem.Text;
            m.RequestSign = (string.IsNullOrEmpty(rblRequestSign.SelectedValue) ? 0 : Int32.Parse(rblRequestSign.SelectedValue));
            m.MapLocation = txtMapDescription.Text;
            m.MapRemark = txtMapRemark.Text;
            m.ServiceType = txtTypeServiceCenter.Text;
            m.ShopFacility = txtWorkshopFacilities.Text;
            m.OwnerExp = txtOwnerExperience.Text;
            m.ShopSize = txtWorkshopSizeType.Text;
            m.Worker = txtNumberOfMechanics.Text;
            m.SubDBWorkshopName = txtSBWorkshopName.Text;
            m.ShopStatus = rblWorkshopStatus.SelectedItem.Text;
            m.YearEstablish = txtYearOfEstablishment.Text;
            m.MapTraffic = (string.IsNullOrEmpty(txtTrafficDensity.Text) ? 0 : Int32.Parse(txtTrafficDensity.Text));
            m.MapSCVisible = (string.IsNullOrEmpty(txtSignboardVisibility.Text) ? 0 : Int32.Parse(txtSignboardVisibility.Text));
            m.ImgInd = (cbPhoneSubmission.Checked ? 1 : 0);
            m.ItemDescription = new List<string>();
            m.ItemQty = new List<int>();
            m.ItemSize = new List<string>();
            m.ItemCount = 0;
            m.NextApprover = "";
            int DocStatus = 0;

            if (hidden_Label_PreviousStatus.Text != "Update")
            {
                switch (lblClaimText.Text)
                {
                    case "New":
                        DocStatus = 2;  // Change to SalesAdmin first then HOD - 10/6/2025
                        m.NextApprover = vm.SalesAdmin;
                        break;

                    case "AwaitingHOD":
                        #region CheckAllHODName
                        // Reverse Population: vm.HOD = highest/David (first), vm.HOD_4 = lowest/Alice (last)
                        // Array remains low-to-high; we map in reverse for vm properties
                        if (count_arr_NA_HOD > 0)
                        {
                            int hodLevelCount = Math.Min(count_arr_NA_HOD, 4);  // Cap at 4
                            for (int arrayIndex = 0; arrayIndex < hodLevelCount; arrayIndex++)
                            {
                                // Fetch from array in reverse: arrayIndex=0 gets arr[max-1] (highest)
                                string hodId = arr_NA_HOD[hodLevelCount - 1 - arrayIndex];
                                string hodName = Function_Method.GetLoginedUserFullName(hodId)?.Split('(')[0] ?? hodId?.Split('(')[0] ?? "";

                                // vmLevel: 1=highest (David), 4=lowest (Alice)
                                int vmLevel = arrayIndex + 1;  // Starts at 1 for highest

                                switch (vmLevel)
                                {
                                    case 1: vm.HOD = hodName; break;      // Highest: David (arr[3])
                                    case 2: vm.HOD_2 = hodName; break;    // Charlie (arr[2])
                                    case 3: vm.HOD_3 = hodName; break;    // Bob (arr[1])
                                    case 4: vm.HOD_4 = hodName; break;    // Lowest: Alice (arr[0])
                                }
                            }
                        }
                        #endregion

                        #region CheckNextApprover
                        // Progression: Forward in vm order (HOD → HOD_2 → HOD_3 → HOD_4 = high → low)
                        int currentArrayIndex = -1;
                        for (int i = 0; i < count_arr_NA_HOD; i++)
                        {
                            if (GLOBAL.user_id == arr_NA_HOD[i])  // Find position in original array
                            {
                                currentArrayIndex = i;
                                break;
                            }
                        }

                        string nextApprover = "";
                        int maxLevels = Math.Min(count_arr_NA_HOD, 4);
                        if (currentArrayIndex >= 0)
                        {
                            // Map array index to vm level: Higher array index = lower vm level (David arr[3] = vm 1)
                            int currentVmLevel = maxLevels - currentArrayIndex;  // e.g., David (3) → 4-3=1

                            if (currentVmLevel < maxLevels)  // Not last (HOD_4); has next lower level
                            {
                                int nextVmLevel = currentVmLevel + 1;  // Forward: 1→2 (David→Charlie)
                                DocStatus = 1;  // Approved, forward to next (lower)

                                switch (nextVmLevel)
                                {
                                    case 2: nextApprover = vm.HOD_2 ?? ""; break;
                                    case 3: nextApprover = vm.HOD_3 ?? ""; break;
                                    case 4: nextApprover = vm.HOD_4 ?? ""; break;
                                    default: nextApprover = ""; break;
                                }
                            }
                            else
                            {
                                // Last level (HOD_4/Alice): End HOD chain
                                DocStatus = 3;  // Awaiting final or completed
                                nextApprover = vm.Approval ?? "";
                            }
                        }
                        else
                        {
                            // User not in HOD array: Default
                            DocStatus = 3;
                            nextApprover = vm.Approval ?? "";
                        }

                        m.NextApprover = !string.IsNullOrEmpty(nextApprover) ? nextApprover : vm.Approval ?? "";
                        #endregion
                        break;

                    case "AwaitingSalesAdmin":
                        DocStatus = 1;
                        #region CheckHOD
                        // Reverse Population: Same as above (vm.HOD = highest/David first)
                        if (count_arr_NA_HOD > 0)
                        {
                            int hodLevelCount = Math.Min(count_arr_NA_HOD, 4);
                            for (int arrayIndex = 0; arrayIndex < hodLevelCount; arrayIndex++)
                            {
                                string hodId = arr_NA_HOD[hodLevelCount - 1 - arrayIndex];
                                string hodName = Function_Method.GetLoginedUserFullName(hodId)?.Split('(')[0] ?? hodId?.Split('(')[0] ?? "";

                                int vmLevel = arrayIndex + 1;

                                switch (vmLevel)
                                {
                                    case 1: vm.HOD = hodName; break;
                                    case 2: vm.HOD_2 = hodName; break;
                                    case 3: vm.HOD_3 = hodName; break;
                                    case 4: vm.HOD_4 = hodName; break;
                                }
                            }

                            // Initial Assignment: Start HOD chain with highest (vm.HOD = David)
                            string initialApprover = vm.HOD ?? "";
                            if (!string.IsNullOrEmpty(initialApprover))
                            {
                                Approver = initialApprover;  // Now highest!
                                m.NextApprover = Approver;
                            }
                            else
                            {
                                // Fallback if no valid HOD
                                Approver = vm.Approval ?? "";
                                m.NextApprover = Approver;
                            }
                        }
                        else
                        {
                            // No HODs: Direct fallback
                            Approver = vm.Approval ?? "";
                            m.NextApprover = Approver;
                        }
                        #endregion
                        uploadPic(hidden_LabelEquipID.Text);
                        break;

                    case "AwaitingAnP":
                        DocStatus = 4;
                        m.NextApprover = vm.SalesAdminManager;
                        uploadPic(hidden_LabelEquipID.Text);
                        break;

                    case "AwaitingSalesAdminMgr":
                        DocStatus = 5;
                        m.NextApprover = vm.GM;
                        uploadPic(hidden_LabelEquipID.Text);
                        break;

                    case "AwaitingGM":
                        DocStatus = 6;
                        m.NextApprover = vm.GM;
                        uploadPic(hidden_LabelEquipID.Text);
                        break;

                    default:
                        // Handle any other cases if necessary
                        break;
                }
            }
            else
            {
                switch (lblClaimText.Text)
                {
                    case "AwaitingHOD":
                        DocStatus = 1;
                        break;
                    case "AwaitingSalesAdmin":
                        DocStatus = 2;
                        break;
                    case "AwaitingAnP":
                        DocStatus = 3;
                        break;
                    case "AwaitingSalesAdminMgr":
                        DocStatus = 4;
                        break;
                    case "AwaitingGM":
                        DocStatus = 5;
                        break;
                    default:
                        // Handle any other cases if necessary
                        DocStatus = 0; // or any default value
                        break;
                }

                m.NextApprover = hidden_LabelNextApprover.Text;
                //2 - sales admin //1-hod // 3-anp //4-sales manage//5-GM //6-approve
            }
            m.DocStatus = DocStatus;
            m.HODLevel = count_arr_NA_HOD.ToString();


            if (rblES.SelectedValue == "1")//batch
            {
                var tuple_get_Batch_Item = get_Batch_Item();
                m.ItemDescription = tuple_get_Batch_Item.Item1;
                m.ItemQty = tuple_get_Batch_Item.Item2;
                m.ItemCount = tuple_get_Batch_Item.Item3;
                m.ItemSize = tuple_get_Batch_Item.Item4;
                if (string.IsNullOrEmpty(m.ItemDescription[0]))//if item id & invoice id empty, return false
                {
                    return false;
                }
            }

            try
            {
                #region ProcessStatus
                m.ProcessStatus = "";
                string PreviousStatus = hidden_Label_PreviousStatus.Text;
                string NextStatus = hidden_Label_NextStatus.Text;
                string currentDt = DateTime.Now.ToString();
                string EquipmentProcessStatus = "";
                string SalesManName = GLOBAL.logined_user_name;// no more using Session["user_id"].ToString()
                if (hidden_LabelEquipID.Text != "")
                {
                    EquipmentProcessStatus = getEquipmentProcessStatus(DynAx, hidden_LabelEquipID.Text);

                    // Normalize: Ensure EquipmentProcessStatus ends with <br/> if not empty and doesn't already
                    if (!string.IsNullOrEmpty(EquipmentProcessStatus))
                    {
                        bool endsWithBr = EquipmentProcessStatus.EndsWith("<br/>", StringComparison.OrdinalIgnoreCase) ||
                                          EquipmentProcessStatus.EndsWith("<br />", StringComparison.OrdinalIgnoreCase) ||
                                          EquipmentProcessStatus.EndsWith(Environment.NewLine) ||
                                          EquipmentProcessStatus.EndsWith("\n") ||
                                          EquipmentProcessStatus.EndsWith("\r") ||
                                          EquipmentProcessStatus.EndsWith("<br>", StringComparison.OrdinalIgnoreCase);

                        if (!endsWithBr)
                        {
                            EquipmentProcessStatus += "<br/>";
                        }
                    }
                    // If empty, leave as-is (handled in branches below)
                }

                if (PreviousStatus == "" && NextStatus == "Draft")//new  draft
                {
                    m.ProcessStatus = "Draft by " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "Draft" && NextStatus == "Draft")// draft
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Draft Update by " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "Draft")
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Submitted by " + SalesManName + " on " + currentDt + "<br/>";
                }

                if (PreviousStatus == "" && NextStatus == "")//new submit
                {
                    m.ProcessStatus = "Submitted by " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "AwaitingHOD")
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Verified by HOD: " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "AwaitingSalesAdmin")
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Verified by Sales Admin: " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "AwaitingAnP")
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Verified by AnP: " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "AwaitingSalesAdminMgr")
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Verified by Sales Admin Mgr: " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "AwaitingGM")
                {
                    IsApproved = true;
                    m.ProcessStatus = EquipmentProcessStatus + "Verified by GM: " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "Approved")
                {
                    IsApproved = true;
                    m.ProcessStatus = EquipmentProcessStatus + "Approved by " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "Rejected")
                {
                    m.ProcessStatus = EquipmentProcessStatus + "Rejected by " + SalesManName + " on " + currentDt + "<br/>";
                }
                else if (PreviousStatus == "Update")
                {
                    m.ProcessStatus = EquipmentProcessStatus;
                }

                #endregion

                string ClaimNumber = Save_LF_WebEquipment(m, vm);

                if (hidden_LabelEquipID.Text != "" && lblClaimText.Text == "New")
                {
                    uploadPic(hidden_LabelEquipID.Text);
                }

                //Function_Method.UserLog(GLOBAL.user_id + " status " + lblClaimText.Text + " " + lblClaimNum.Text + " warehouse selected: " + inventLocationId);

            }
            catch (Exception e)
            {
                Function_Method.MsgBox("Fail to save. " + e, Page, this);
                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
            return IsApproved;
        }


        public string Save_LF_WebEquipment(SignboardEquipmentModel m, SignboardEquipmentAppGroupModel vm)
        {
            string EquipID = "";
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                {
                    DynAx.TTSBegin();

                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Equip_ID",
        hidden_LabelEquipID.Text));
                    Save_LF_WebEquipmentTable_Parameter(DynRec, m, vm);
                    if (hidden_LabelEquipID.Text != "")
                    {

                        if (DynRec.Found)
                        {
                            DynRec.Call("Update");
                            logger.Info($"Updated equipment with Equip_ID: {hidden_LabelEquipID.Text}");
                        }
                    }
                    else
                    {
                        DynRec.Call("insert");
                        logger.Info($"Inserted new equipment with Equip_ID: {EquipID}");
                    }

                    EquipID = DynRec.get_Field("Equip_ID").ToString();
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();

                    if (hidden_LabelEquipID.Text == "")
                    {
                        hidden_LabelEquipID.Text = EquipID;
                    }

                    return EquipID;
                }
            }
            catch (Exception ER_SE_11)
            {
                logger.Error($"Error in Save_LF_WebEquipment: {ER_SE_11}");
                Function_Method.MsgBox("ER_SE_11: " + ER_SE_11.ToString(), this.Page, this);
                Function_Method.UserLog(ER_SE_11.ToString());
                return "";
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        public static string getEquipmentProcessStatus(Axapta DynAx, string EquipID)
        {
            int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");

            string ProcessStatus = "";

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WebEquipment);

            int cn1 = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

            qbr6.Call("value", EquipID);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WebEquipment);
                ProcessStatus = DynRec6.get_Field("ProcessStatus").ToString();

                DynRec6.Dispose();
            }
            return ProcessStatus;
        }

        private void Save_LF_WebEquipmentTable_Parameter(AxaptaRecord DynRec, SignboardEquipmentModel m, SignboardEquipmentAppGroupModel vm)
        {
            DynRec.set_Field("CustAccount", m.CustAccount);
            DynRec.set_Field("CustContact", m.CustContact);
            DynRec.set_Field("CustPhone", m.CustPhone);
            DynRec.set_Field("Del_To", m.DeliveryTo);
            DynRec.set_Field("Del_Addr", m.Address);
            DynRec.set_Field("Del_Person", m.DelPerson);
            //DynRec.set_Field("Remarks_Sales", m.Remarks);
            DynRec.set_Field("ProcessStatus", m.ProcessStatus);
            DynRec.set_Field("FormType", m.FormType);
            DynRec.set_Field("DepositType", m.DepositType);
            DynRec.set_Field("RequestSign", m.RequestSign.ToString());
            DynRec.set_Field("MapLocation", m.MapLocation);
            DynRec.set_Field("MapRemark", m.MapRemark);
            DynRec.set_Field("DocStatus", m.DocStatus);
            if (m.DocStatus == 2)
            {
                DynRec.set_Field("AppliedBy", m.AppliedBy);
                DynRec.set_Field("EmplName", m.EmplName);
            }
            DynRec.set_Field("ServiceType", m.ServiceType);
            DynRec.set_Field("ShopFacility", m.ShopFacility);
            DynRec.set_Field("OwnerExp", m.OwnerExp);
            DynRec.set_Field("ShopSize", m.ShopSize);
            DynRec.set_Field("Worker", m.Worker);
            DynRec.set_Field("ShopStatus", m.ShopStatus);
            DynRec.set_Field("YearEstablish", m.YearEstablish);
            DynRec.set_Field("MapTraffic", m.MapTraffic);
            DynRec.set_Field("MapSCVisible", m.MapSCVisible);
            DynRec.set_Field("Img_Ind", m.ImgInd);
            DynRec.set_Field("AppType", m.AppType);
            DynRec.set_Field("NextApprover", m.NextApprover);
            DynRec.set_Field("HODLevel", m.HODLevel);
            DynRec.set_Field("Branch_WorkshopName", m.SubDBWorkshopName);
            DynRec.set_Field("Cost", m.Cost);

            if (m.DocStatus == (int)DocStatus.AwaitingHOD)
            {
                DynRec.set_Field("ProcessDate", m.AppliedDate);

                DynRec.set_Field("NA_HOD", (string.IsNullOrEmpty(vm.HOD) ? "" : vm.HOD));
                DynRec.set_Field("NA_HOD_2", (string.IsNullOrEmpty(vm.HOD_2) ? "" : vm.HOD_2));
                DynRec.set_Field("NA_HOD_3", (string.IsNullOrEmpty(vm.HOD_3) ? "" : vm.HOD_3));
                DynRec.set_Field("NA_HOD_4", (string.IsNullOrEmpty(vm.HOD_4) ? "" : vm.HOD_4));
                DynRec.set_Field("NA_Admin", (string.IsNullOrEmpty(vm.SalesAdmin) ? "" : vm.SalesAdmin));
                DynRec.set_Field("NA_Manager", (string.IsNullOrEmpty(vm.SalesAdminManager) ? "" : vm.SalesAdminManager));
                DynRec.set_Field("NA_GM", (string.IsNullOrEmpty(vm.GM) ? "" : vm.GM));
                
                #region HOD_Level
                if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD_2) //When Marketing Admin approve - next is HOD level 1
                {
                    DynRec.set_Field("Remarks_HOD_2", m.Remarks);
                    DynRec.set_Field("HODDate", m.AppliedDate);
                }
                else if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD) //When HOD 1 approve but status still AwaitingHOD
                {
                    DynRec.set_Field("Remarks_HOD", m.Remarks);
                    DynRec.set_Field("HODDate", m.AppliedDate);
                }
                else if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD_3) //When HOD 3 approve but status still AwaitingHOD
                {
                    DynRec.set_Field("Remarks_HOD_3", m.Remarks);
                    DynRec.set_Field("HODDate_3", m.AppliedDate);
                }
                else if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD_4) //When HOD 4 approve but status still AwaitingHOD
                {
                    DynRec.set_Field("Remarks_HOD_4", m.Remarks);
                    DynRec.set_Field("HODDate_4", m.AppliedDate);
                }
                else //When HOD 2 approve but status still AwaitingHOD, HOD 3 must in status AwaitingAnP
                {
                    DynRec.set_Field("Remarks_Admin", m.Remarks);
                    DynRec.set_Field("AdminDate", m.AppliedDate);

                }
                #endregion
            }
            else if (m.DocStatus == (int)DocStatus.AwaitingSalesAdmin)
            {
                DynRec.set_Field("NA_HOD", (string.IsNullOrEmpty(vm.HOD) ? "" : vm.HOD));
                DynRec.set_Field("NA_HOD_2", (string.IsNullOrEmpty(vm.HOD) ? "" : vm.HOD_2));
                DynRec.set_Field("NA_HOD_3", (string.IsNullOrEmpty(vm.HOD) ? "" : vm.HOD_3));
                DynRec.set_Field("NA_HOD_4", (string.IsNullOrEmpty(vm.HOD_4) ? "" : vm.HOD_4));
                DynRec.set_Field("NA_Admin", (string.IsNullOrEmpty(vm.SalesAdmin) ? "" : vm.SalesAdmin));
                DynRec.set_Field("Remarks_Sales", m.Remarks);
                DynRec.set_Field("ProcessDate", m.AppliedDate);
                DynRec.set_Field("AppliedDate", m.AppliedDate);
                #region AppliedTime
                // Assuming m.AppliedTime is in the format "hh:mm:ss"
                string appliedTimeString = m.AppliedTime; // e.g., "05:23:35"
                                                          // Parse the time string into a TimeSpan
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(appliedTimeString, out timeSpan))
                {
                    // Get the total seconds
                    int totalSeconds = (int)timeSpan.TotalSeconds;
                    // Save the total seconds to DynRec
                    DynRec.set_Field("AppliedTime", totalSeconds);
                }
                else
                {
                    Console.WriteLine("Invalid time format for AppliedTime.");
                }
                #endregion
            }
            else if (m.DocStatus == (int)DocStatus.AwaitingAnP)
            {
                #region HOD_Level

                if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD) //When HOD 1 approve
                {
                    DynRec.set_Field("Remarks_HOD", m.Remarks);
                    DynRec.set_Field("HODDate", m.AppliedDate);
                }
                else if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD_2)//When HOD 2 approve 
                {
                    DynRec.set_Field("Remarks_HOD_2", m.Remarks);
                    DynRec.set_Field("HODDate_2", m.AppliedDate);
                }
                else if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD_3)//When HOD 3 approve 
                {
                    DynRec.set_Field("Remarks_HOD_3", m.Remarks);
                    DynRec.set_Field("HODDate_3", m.AppliedDate);
                }
                else if (Function_Method.GetLoginedUserFullName(GLOBAL.user_id).Split('(')[0] == vm.HOD_4)//When HOD 4 approve 
                {
                    DynRec.set_Field("Remarks_HOD_4", m.Remarks);
                    DynRec.set_Field("HODDate_4", m.AppliedDate);
                }
                #endregion
            }
            else if (m.DocStatus == (int)DocStatus.AwaitingSalesAdminMgr)
            {
                DynRec.set_Field("RemarksAnP", m.Remarks);
                DynRec.set_Field("AnPDate", m.AppliedDate);
            }
            else if (m.DocStatus == (int)DocStatus.AwaitingGM)
            {
                DynRec.set_Field("Remarks_AdminMgr", m.Remarks);

                DynRec.set_Field("ManagerDate", m.AppliedDate);
            }
            else
            {
                DynRec.set_Field("Remarks_GM", m.Remarks);

                DynRec.set_Field("GMDate", m.AppliedDate);
            }
            //DynRec.set_Field("AppliedTime", DateTime.Now);
            for (int i = 0; i < m.ItemCount; i++)
            {
                try
                {
                    DynRec.set_Field("Item" + (i + 1), m.ItemDescription[i]);
                    DynRec.set_Field("Qty" + (i + 1), m.ItemQty[i]);
                    DynRec.set_Field("Size" + (i + 1), m.ItemSize[i]);
                }
                catch
                {
                    break;
                }
            }
            /* DynRec.set_Field("RMADate", "");
            DynRec.set_Field("CNDate", "");
            */
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //ExportGridToExcel();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            GridViewPageEventArgs Event = null;
            //if (btnTitle.Text == "Job Days Taken")
            //{
            //    JobDaysTaken_section.Attributes.Add("style", "display:initial");
            //    //JobDaysTakenReport(0, Event);
            //    export.Visible = true;
            //}
            //else if (btnTitle.Text == "Production/Charging Code")
            //{
            //    QueryReport_section.Attributes.Add("style", "display:initial");
            //    export.Visible = false;
            //    //queryReport_Overview(0, Event);
            //}
            //else if (btnTitle.Text == "Battery Query Report")
            //{
            //    BatteryQuery_section.Attributes.Add("style", "display:initial");
            //    //BatteryQueryReport(0, Event);
            //    export.Visible = true;
            //}
            //else
            //{
            //    BatteryStatistic_section.Attributes.Add("style", "display:initial");
            //    //BatteryStatisticReport(0, Event);
            //    export.Visible = true;
            //}
        }

        protected void gvJobsTaken_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
        #endregion

        public enum DocStatus
        {
            Draft = 0,
            AwaitingHOD = 1,
            AwaitingSalesAdmin = 2,
            AwaitingAnP = 3,
            AwaitingSalesAdminMgr = 4,
            AwaitingGM = 5,
            Approved = 6,
            Rejected = 7,
        }

        #region FileUpload
        protected void uploadPic(string unique_key)
        {
            HttpFileCollection files = Request.Files;
            string filename = "";
            string key = hidden_LabelEquipID.Text;

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
                        string path = @"e:/SignboardEquipment/" + Label_Salesman.Text + "/" + Label_CustAcc.Text;

                        if (!Directory.Exists(@"e:/SignboardEquipment/" + Label_Salesman.Text))
                        {

                            Directory.CreateDirectory(@"e:/SignboardEquipment/" + Label_Salesman.Text);
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
                        string fileType = System.IO.Path.GetExtension(filename).ToLower() == ".pdf" ? "PDF" : "Image";
                        if (Path.GetExtension(filename).ToLower() == ".pdf")
                        {
                            file.SaveAs(filePath);
                            logger.Info("PDF file saved: {0}", filePath);
                        }
                        else
                        {

                            Function_Method.ImgCompress(file, filePath);//compress image and save into E drive
                            Function_Method.UserLog(hidden_LabelEquipID.Text + " file uploaded.");
                            logger.Info("{0} image uploaded.", hidden_LabelEquipID.Text);
                        }

                        using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                        {
                            string query = "insert into signboardequipment_filetbl(ReferID, SalesAdmin, Salesman, Customer, FileLocation, createDateTime, createBy, FileType) values (@c1, @sA1, @s1, @c2, @p1, @createDateTime, @createBy, @fileType)";
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            //Jerry 2024-11-05 use unique_key
                            //cmd.Parameters.AddWithValue("@c1", hidden_LabelClaimNumber.Text);
                            cmd.Parameters.AddWithValue("@c1", key);
                            cmd.Parameters.AddWithValue("@sA1", GLOBAL.logined_user_name);
                            cmd.Parameters.AddWithValue("@s1", Label_Salesman.Text);
                            cmd.Parameters.AddWithValue("@c2", Label_CustName.Text);
                            cmd.Parameters.AddWithValue("@p1", filePath);
                            cmd.Parameters.AddWithValue("@createDateTime", DateTime.Now); // Set current date and time
                            cmd.Parameters.AddWithValue("@createBy", GLOBAL.logined_user_name); // Set the user who uploaded
                            cmd.Parameters.AddWithValue("@fileType", fileType); // Set the file type
                            Function_Method.UserLog(hidden_LabelEquipID.Text + " filepath: " + filePath);
                            logger.Info("Database record inserted for {0}", filePath);

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
                }
            }
            catch (Exception ER_SE_22)
            {
                logger.Error(ER_SE_22, "Upload error");
                Function_Method.MsgBox("Upload error: " + ER_SE_22.ToString(), this.Page, this);
                Function_Method.UserLog("Upload error: " + ER_SE_22.ToString());
            }
        }

        protected void updatePic(string unique_key, string claim_number)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string query = "update signboardequipment_filetbl set ReferID = @c1 where ReferID = @c2; ";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@c1", claim_number);
                    cmd.Parameters.AddWithValue("@c2", unique_key);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ER_SE_22)
            {
                Function_Method.MsgBox("Upload error: " + ER_SE_22.ToString(), this.Page, this);
                Function_Method.UserLog("Upload error: " + ER_SE_22.ToString());
            }
        }

        private void GetReferIdImage(bool RefreshPage)
        {
            int count = 0;
            string ReferID = hidden_LabelEquipID.Text;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "select ReferID, Salesman, SalesAdmin, Customer, FileLocation from signboardequipment_filetbl where " +
                "ReferID=@c1";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            MySqlParameter _C1 = new MySqlParameter("@c1", MySqlDbType.VarChar, 0);
            _C1.Value = ReferID;
            cmd.Parameters.Add(_C1);

            conn.Open();

            List<ListItem> files = new List<ListItem>();
            List<ImageItem> images = new List<ImageItem>();
            List<ListItem> pdfFiles = new List<ListItem>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == ReferID)
                    {
                        string img1 = reader.GetValue(4).ToString();

                        #region Previous File
                        // Retrieve Salesman, SalesAdmin, and Customer values
                        string salesman = reader.GetValue(1).ToString();
                        string salesAdmin = reader.GetValue(2).ToString();
                        string customer = reader.GetValue(3).ToString();
                        // Check if Salesman, SalesAdmin, and Customer are empty
                        if (string.IsNullOrEmpty(salesman) && string.IsNullOrEmpty(salesAdmin) && string.IsNullOrEmpty(customer))
                        {
                            // Construct the link for the file
                            string link = "https://www.hi-rev.com.my/ES/uploads/" + img1;
                            files.Add(new ListItem(img1, link)); // Add the link to the files list
                            hdFilePath.Value = link;

                            string getFileExtension = System.IO.Path.GetExtension(link);

                            if (getFileExtension == ".pdf")
                            {
                                pdfFiles.Add(new ListItem(img1, link));
                            }
                            else
                            {
                                images.Add(new ImageItem(img1, link)); // Assuming ImageItem is defined elsewhere
                            }
                        }
                        else
                        {
                            string[] arr_pathSplit = img1.Split('/');
                            //var filePath = "http://10.1.1.167/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3] + "/" + arr_pathSplit[4];//
                            var filePath = GLOBAL.externalServerIP + "/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3] + "/" + arr_pathSplit[4]; //for external live
                            files.Add(new ListItem(arr_pathSplit[4], filePath));
                            hdFilePath.Value = "_SEPDF@" + arr_pathSplit[0] + "/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3];

                            string getFileExtension = System.IO.Path.GetExtension(filePath);

                            if (getFileExtension == ".pdf")
                            {
                                pdfFiles.Add(new ListItem(arr_pathSplit[4], filePath));
                            }
                            else
                            {
                                images.Add(new ImageItem(arr_pathSplit[4], filePath)); // Assuming ImageItem is defined elsewhere
                            }

                        }
                        #endregion
                        upload_section.Visible = true;
                        count++;
                    }
                }
            }

            if (pdfFiles.Count > 0)
            {
                pdf_section.Visible = true;
                // Bind pdfFiles to a repeater or similar control
                pdfRepeater.DataSource = pdfFiles; // Assuming you have a repeater for PDFs
                pdfRepeater.DataBind();
            }
            if (images.Count > 0)
            {
                display_section.Visible = true;
                // Bind images to a repeater or similar control
                imageRepeater.DataSource = images; // Assuming you have a repeater for images
                imageRepeater.DataBind();
            }
            if (hidden_Label_PreviousStatus.Text == "AwaitingHOD") { upload_section.Visible = false; }

            #region - Neil 25-9-2025 - Button Delete image pdf not visible for non sales admin user
            Axapta DynAx = Function_Method.GlobalAxapta();
            string ProcessedUserName = Regex.Replace(Session["logined_user_name"].ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
            string SalesAdminName = Regex.Replace(getSignboardEquipmentSalesAdminName(DynAx, hidden_LabelEquipID.Text).ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
            if (ProcessedUserName != SalesAdminName)
            {
                // FIXED: Hide PDF delete buttons (dynamic, no error)
                HideRepeaterButtons(pdfRepeater, "btnDeletePdf");
                // FIXED: Hide image delete buttons (tries Repeater first, then standalone fallback)
                HideRepeaterButtons(imageRepeater, "btnDeleteDisplayIMG");
            }
            DynAx.Dispose();
            #endregion

            if (RefreshPage)
            {
                // Refresh the page after processing
                ClientScript.RegisterStartupScript(this.GetType(), "RefreshPage", "window.location.reload();", true);
            }

        }
        public class ImageItem
        {
            public string ImageName { get; set; }
            public string ImageUrl { get; set; }
            public string Value // Add this property
            {
                get { return ImageUrl; } // Assuming you want Value to represent the image URL
            }
            // Constructor that takes only one argument
            public ImageItem(string imageName, string imageUrl)
            {
                ImageName = imageName;
                ImageUrl = imageUrl;
            }
        }

        private bool CheckSalesAdmin()
        {
            bool check = true;
            Axapta DynAx = Function_Method.GlobalAxapta();
            string ProcessedUserName = Regex.Replace(Session["logined_user_name"].ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
            string SalesAdminName = Regex.Replace(getSignboardEquipmentSalesAdminName(DynAx, hidden_LabelEquipID.Text).ToString(), @"\s+", "").ToLower().Trim().Split('(')[0];
            if (ProcessedUserName != SalesAdminName)
            {
                Function_Method.MsgBox("Only Sales Admin have permission to delete submission file/image.", this.Page, this);
                check = false;
            }
            DynAx.Dispose();
            return check;
        }
        private bool CheckSalesAdmin_BySQL()
        {
            bool check = false;

            // Check if the session variable is null or empty
            if (Session["logined_user_name"] == null)
            {
                return check; // Return false if the session variable is not set
            }

            string processedUserName = Session["logined_user_name"].ToString().Trim().Split('(')[0];

            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                conn.Open();

                string sql = "SELECT * FROM signboardequipmentappgroup WHERE SalesAdmin = @SalesAdmin";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SalesAdmin", processedUserName);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check if any rows were returned
                        check = reader.Read(); // This will return true if there is at least one row
                    }
                }
            }

            return check;
        }

        private string GetUserRole(string userName)
        {
            string role = null; // Initialize role as null

            // Sanitize the input to avoid SQL injection
            string processedUserName = userName.Trim();

            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                conn.Open();

                // SQL query to check the role based on multiple fields
                string sql = @"
            SELECT SalesAdmin, SalesAdminManager, Approval, GM 
            FROM signboardequipmentappgroup 
            WHERE SalesAdmin = @SalesAdmin 
               OR SalesAdminManager = @SalesAdminManager 
               OR Approval = @Approval 
               OR GM = @GM";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Set parameters
                    cmd.Parameters.AddWithValue("@SalesAdmin", processedUserName);
                    cmd.Parameters.AddWithValue("@SalesAdminManager", processedUserName);
                    cmd.Parameters.AddWithValue("@Approval", processedUserName);
                    cmd.Parameters.AddWithValue("@GM", processedUserName);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Check which role the user belongs to
                            if (reader["SalesAdmin"].ToString().Equals(userName, StringComparison.OrdinalIgnoreCase))
                            {
                                role = "SalesAdmin";
                            }
                            else if (reader["SalesAdminManager"].ToString().Equals(userName, StringComparison.OrdinalIgnoreCase))
                            {
                                role = "SalesAdminManager";
                            }
                            else if (reader["Approval"].ToString().Equals(userName, StringComparison.OrdinalIgnoreCase))
                            {
                                role = "Approval";
                            }
                            else if (reader["GM"].ToString().Equals(userName, StringComparison.OrdinalIgnoreCase))
                            {
                                role = "GM";
                            }
                        }
                    }
                }
            }

            return role; // Return the role or null if not found
        }

        public static string getSignboardEquipmentSalesAdminName(Axapta DynAx, string EquipmentID)
        {

            int LF_WebEquipment = DynAx.GetTableIdWithLock("LF_WebEquipment");

            string SalesAdmin = "";

            AxaptaObject axQuery6 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource6 = (AxaptaObject)axQuery6.Call("addDataSource", LF_WebEquipment);

            int cn1 = DynAx.GetFieldId(LF_WebEquipment, "Equip_ID");
            var qbr6 = (AxaptaObject)axQueryDataSource6.Call("addRange", cn1);

            qbr6.Call("value", EquipmentID);
            AxaptaObject axQueryRun6 = DynAx.CreateAxaptaObject("QueryRun", axQuery6);

            if ((bool)axQueryRun6.Call("next"))
            {
                AxaptaRecord DynRec6 = (AxaptaRecord)axQueryRun6.Call("Get", LF_WebEquipment);
                SalesAdmin = DynRec6.get_Field("NA_Admin").ToString();

                DynRec6.Dispose();
            }
            return SalesAdmin;
        }
        protected void DeleteImage(object sender, CommandEventArgs e)
        {
            if (CheckSalesAdmin())
            {
                string imageName = e.CommandArgument.ToString();
                DeleteImageFromDatabase(imageName, hidden_LabelEquipID.Text);

                // Refresh the displayed images after deletion
                GetReferIdImage(true);
            }
        }

        private void DeleteImageFromDatabase(string imageId, string refID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string query = "DELETE FROM signboardequipment_filetbl WHERE FileLocation LIKE @imageId AND ReferID = @refID ;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@imageId", "%/" + imageId);
                    cmd.Parameters.AddWithValue("@refID", refID);


                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    GlobalHelper.LogToDatabase("Signboard.DeleteImageFromDatabase", $"{query}", null, GLOBAL.user_id);
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        Function_Method.MsgBox("No matching file found to delete", this.Page, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Delete error: " + ex.ToString(), this.Page, this);
                Function_Method.UserLog("Delete error: " + ex.ToString());
            }
        }
        protected void DeletePdf(object sender, CommandEventArgs e)
        {
            if (CheckSalesAdmin())
            {
                string pdfName = e.CommandArgument.ToString();
                DeletePdfFromDatabase(pdfName, hidden_LabelEquipID.Text);

                // Refresh the displayed PDFs after deletion
                GetReferIdImage(true);
            }
        }
        private void DeletePdfFromDatabase(string pdfName, string refID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    string query = "DELETE FROM signboardequipment_filetbl WHERE FileLocation LIKE @pdfName  AND ReferID = @refID ;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pdfName", "%/" + pdfName);
                    cmd.Parameters.AddWithValue("@refID", refID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    GlobalHelper.LogToDatabase("Signboard.DeletePdfFromDatabase", $"{query}", null, GLOBAL.user_id);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Delete error: " + ex.ToString(), this.Page, this);
                Function_Method.UserLog("Delete error: " + ex.ToString());
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpFileCollection files = Request.Files; // Get the uploaded files
            if (files.Count > 0)
            {
                string uniqueKey = hidden_LabelEquipID.Text;
                uploadPic(uniqueKey);

                // Refresh the displayed PDFs after deletion
                GetReferIdImage(false);

            }
            else
            {
                // Handle the case where no files were uploaded
                // You might want to show a message to the user
                Function_Method.MsgBox("No files were uploaded. Please select files to upload.", this.Page, this);
            }
        }

        protected void YourSubmitButton_Click(object sender, EventArgs e)
        {
            // Get the selected value from the RadioButtonList
            string trafficDensityValue = rblModalTrafficDensity.SelectedValue; // This will give you the selected value (1, 2, or 3)
            string signboardVisibilityValue = rblModalSignboardVisibility.SelectedValue; // Similarly for signboard visibility
                                                                                         // If you want to display the text corresponding to the selected value
            string trafficDensityText = rblModalTrafficDensity.SelectedItem.Text; // This will give you the text (Low, Fair, High)
            string signboardVisibilityText = rblModalSignboardVisibility.SelectedItem.Text; // Similarly for signboard visibility
                                                                                            // Example of assigning to a TextBox or Label
            txtTrafficDensity.Text = trafficDensityValue; // Assuming txtTrafficDensity is a TextBox
            txtSignboardVisibility.Text = signboardVisibilityValue; // Assuming txtSignboardVisibility is a TextBox
            textSignboardVisibility.Text = signboardVisibilityText;
            textTrafficDensity.Text = trafficDensityText;
            // You can also use the values as needed
            // For example, you can log them or use them in further processing
        }

        #endregion

        #region GetPastMonthsPurchaseRecord
        private void GetPastMonthsPurchaseRecord(string custAcc, DateTime AppliedDate)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                int CustTrans = DynAx.GetTableIdWithLock("CustTrans");

                int custAccount = DynAx.GetFieldId(CustTrans, "AccountNum");
                int TransDate = DynAx.GetFieldId(CustTrans, "TransDate");
                int AmountCur = DynAx.GetFieldId(CustTrans, "AmountCur");
                int TransType = DynAx.GetFieldId(CustTrans, "TransType");


                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

                // Add range for AccountNum
                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", custAccount);//claimstat
                qbr.Call("value", custAcc);

                // Add range for TransType
                var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", TransType);//claimstat
                qbr2.Call("value", "Sales Order");

                // Calculate the date 6 months ago
                DateTime sixMonthsAgo = AppliedDate.AddMonths(-6);//DateTime.Now.AddMonths(-6);
                string formattedDate = sixMonthsAgo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // Format as needed

                // Add range for TransDate for the last 6 months
                var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", TransDate);
                qbr3.Call("value", formattedDate);
                qbr3.Call("value", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)); // End date is today


                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                // Prepare the list to hold records
                List<CustTransRecord> records = new List<CustTransRecord>();

                while ((bool)axQueryRun.Call("next"))
                {
                    #region RetrieveData
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);
                    // Retrieve field values from the current record

                    object transDateObj = DynRec.get_Field("TransDate").ToString();
                    DateTime transDate;
                    // Check if the retrieved object is of type DateTime
                    if (transDateObj is DateTime)
                    {
                        transDate = (DateTime)transDateObj; // Cast to DateTime
                    }
                    else if (transDateObj is string)
                    {
                        string dateString = (string)transDateObj; // Cast to string
                        if (DateTime.TryParse(dateString, out transDate))
                        {
                            // Successfully parsed from string
                        }
                        else
                        {
                            transDate = DateTime.MinValue; // Handle the error as needed
                                                           // Optionally log an error or throw an exception
                        }
                    }
                    else
                    {
                        transDate = DateTime.MinValue; // Handle the error as needed
                                                       // Optionally log an error or throw an exception
                    }

                    object amountCurObj = DynRec.get_Field("AmountCur").ToString();
                    double amountCur;
                    // Check if the retrieved object is of type double
                    if (amountCurObj is double)
                    {
                        amountCur = (double)amountCurObj; // Cast to double
                    }
                    else if (amountCurObj is string)
                    {
                        string s = (string)amountCurObj; // Cast to string
                        double parsed; // Declare the variable before using it
                        if (double.TryParse(s, out parsed))
                        {
                            amountCur = parsed; // Successfully parsed from string
                        }
                        else
                        {
                            amountCur = 0; // Handle the error as needed
                                           // Optionally log an error or throw an exception
                        }
                    }
                    else
                    {
                        amountCur = 0; // Handle the error as needed
                                       // Optionally log an error or throw an exception
                    }
                    
                    // Create a new record object
                    CustTransRecord record = new CustTransRecord
                    {

                        TransDate = transDate,
                        AmountCur = amountCur
                    };
                    // Add to the list
                    records.Add(record);
                    #endregion

                }
                // Now summarize the records for the past 6 months
                var summaries = GetMonthlySummaries(records, 6);
                // Display results in the label with line breaks
                var htmlTable = BuildHtmlTable(summaries, 3);
                lblPastRecord.Text = htmlTable;
            }
            catch (Exception e)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine(e.Message);
            }
            finally
            {
                DynAx.Dispose();
            }
        }
        public class CustTransRecord
        {
            public DateTime TransDate { get; set; }
            public double AmountCur { get; set; }
            // Add other fields as needed
        }

        private List<string> GetMonthlySummaries(List<CustTransRecord> records, int monthsBack)
        {
            DateTime now = DateTime.Now;
            DateTime startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1 - monthsBack);
            // Prepare a list of months to cover
            List<DateTime> monthList = new List<DateTime>();
            for (int i = 0; i < monthsBack; i++)
            {
                monthList.Add(startMonth.AddMonths(i));
            }
            // Group records by year and month
            var grouped = records
                .Where(r => r.TransDate >= startMonth)
                .GroupBy(r => new { r.TransDate.Year, r.TransDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(x => x.AmountCur)
                })
                .ToList();

            #region For Outstanding Record
            double totalAmountCur = grouped.Sum(record => record.TotalAmount);
            double AvgrMonthlySales6Mth = totalAmountCur / 6;

            modalAvrgMonthlySales6Mth.Text = AvgrMonthlySales6Mth.ToString("#,###,###,##0.00");
            #endregion
            // Prepare the result list
            List<string> result = new List<string>();
            List<string> tempRow = new List<string>(); // Temporary list for current row
            foreach (var m in monthList)
            {
                var monthSummary = grouped.FirstOrDefault(g => g.Year == m.Year && g.Month == m.Month);
                double total = monthSummary?.TotalAmount ?? 0.0;
                string formattedMonth = m.ToString("MMM yyyy", CultureInfo.InvariantCulture);
                result.Add($"{formattedMonth} - RM {total:F2}");

                // If we have 3 items in the current row, join them and add to the result
                if (tempRow.Count == 3)
                {
                    result.Add(string.Join(" | ", tempRow)); // Use a separator of your choice
                    tempRow.Clear(); // Clear the temporary row for the next set
                }
            }

            // If there are any remaining items in tempRow, add them as well
            if (tempRow.Count > 0)
            {
                result.Add(string.Join(" | ", tempRow));
            }
            // Reverse the result to show the latest month first (optional)
            result.Reverse();
            return result;
        }

        private string BuildHtmlTable(List<string> summaries, int columnsPerRow = 3)
        {
            var table = new System.Text.StringBuilder();
            table.Append("<table style='border-collapse: collapse;'>");
            for (int i = 0; i < summaries.Count; i++)
            {
                if (i % columnsPerRow == 0)
                    table.Append("<tr>");
                table.AppendFormat("<td style='padding: 8px 20px; border: 1px solid #ccc;'>{0}</td>", summaries[i]);
                if (i % columnsPerRow == columnsPerRow - 1)
                    table.Append("</tr>");
            }
            // Close last row if not completed
            if (summaries.Count % columnsPerRow != 0)
                table.Append("</tr>");
            table.Append("</table>");
            return table.ToString();
        }
        #endregion
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static SignboardEquipment_Cust_AgingTableModel GetCustAging(Axapta dynAx, string customerAcc)
        {
            var result = new SignboardEquipment_Cust_AgingTableModel();

            int custAgingTable = 40056;//Cust_AgingTable
            using (AxaptaObject axQuery = dynAx.CreateAxaptaObject("Query"))
            {
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", custAgingTable);
                var qbr1_7 = (AxaptaObject)axQueryDataSource.Call("addRange", 40008);//CustAccount
                qbr1_7.Call("value", customerAcc);
                axQueryDataSource.Call("addSortField", 65534, 1); // RecId, descending

                using (AxaptaObject axQueryRun = dynAx.CreateAxaptaObject("QueryRun", axQuery))
                {
                    if ((bool)axQueryRun.Call("next"))
                    {
                        using (AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", custAgingTable))
                        {
                            // Map all fields from the record to your model
                            result.BankCorpGuaranteeAmt = dynRec.get_Field("BankCorpGuaranteeAmt")?.ToString() ?? "";
                            var expiryDate = dynRec.get_Field("BankCorpExpiryDate");

                            result.BankCorpExpiryDate = expiryDate != null ?
                            Convert.ToDateTime(expiryDate).ToString("dd/MM/yyyy") : "";

                            result.LastMonthCollection = dynRec.get_Field("LastMonthCollection")?.ToString() ?? "";
                            result.CurrentMonthCollection = dynRec.get_Field("CurrentMonthCollection")?.ToString() ?? "";

                            double avgPaymDays; // Declare the variable first
                            double.TryParse(dynRec.get_Field("AvgPaymDays")?.ToString(), out avgPaymDays);
                            result.AvgPaymDays = avgPaymDays; // Assign the value (will be 0 if parsing fails)

                            double avgPaymDays2;
                            double.TryParse(dynRec.get_Field("AvgPaymDays2")?.ToString(), out avgPaymDays2);
                            result.AvgPaymDays2 = avgPaymDays2;

                            double avgMonthlySales;
                            double.TryParse(dynRec.get_Field("AvgMonthlySales")?.ToString(), out avgMonthlySales);
                            result.AvgMonthlySales = avgMonthlySales;

                            result.LastReturnCheqAmt = dynRec.get_Field("LastReturnCheqAmt")?.ToString() ?? "";

                            double postDatedChqTotal;
                            double.TryParse(dynRec.get_Field("PostDatedChqTotal")?.ToString(), out postDatedChqTotal);
                            result.PostDatedChqTotal = postDatedChqTotal;

                            bool applyCredit;
                            bool.TryParse(dynRec.get_Field("ApplyCredit")?.ToString(), out applyCredit);
                            result.ApplyCredit = applyCredit;

                            double overDueInterest;
                            double.TryParse(dynRec.get_Field("OverDueInterest")?.ToString(), out overDueInterest);
                            result.OverDueInterest = overDueInterest;
                        }
                    }
                }
            }
            return result;
        }

        #region Enquiries
        
        protected void dtgvIssueTo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvIssueTo.PageIndex = e.NewPageIndex;
            BindGrid_IssueTo();
        }
        private void BindGrid_IssueTo()
        {
            using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
            {
                conn.Open();
                string SQL = " SELECT ID, Type, IssueTo, CASE WHEN Status = 1 THEN 'Active' WHEN Status = 0 THEN 'Disable' END AS Status " +
                    " FROM signboardequipment_issueto; ";
                using (MySqlCommand cmd = new MySqlCommand(SQL, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        gvIssueTo.DataSource = dt;
                        gvIssueTo.DataBind();
                    }
                }
                conn.Close();
            }
        }

        protected void gvIssueTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvIssueTo.SelectedValue != null)
            {
                int id = Convert.ToInt32(gvIssueTo.SelectedValue);
                DataTable dt = new DataTable();
                string SQL = "SELECT ID, IssueTo, Status FROM signboardequipment_issueto WHERE ID = @ID";

                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                using (MySqlCommand cmd = new MySqlCommand(SQL, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    hdnID.Value = dt.Rows[0]["ID"].ToString();
                    txtIssueTo.Text = dt.Rows[0]["IssueTo"].ToString();
                    ddlStatus_IssueTo.ClearSelection();
                    ddlStatus_IssueTo.SelectedValue = dt.Rows[0]["Status"].ToString();

                    gvIssueTo.Visible = false;              // Hide GridView
                    divIssueTo_details.Visible = true;      // Show edit form
                    // Update the UpdatePanels
                    UpdatePanel3.Update(); // Update the grid panel
                    UpdatePanel4.Update(); // Update the details panel
                }
            }
        }
        protected void btnCancel_IssueTo_Click(object sender, EventArgs e)
        {
            gvIssueTo.Visible = true;              // Hide GridView
            divIssueTo_details.Visible = false;      // Show edit form
                                                     // Update the UpdatePanels
            UpdatePanel3.Update(); // Update the grid panel
            UpdatePanel4.Update(); // Update the details panel
        }
        // Add New button click handler
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            // Clear form for new entry
            txtIssueTo.Text = string.Empty;
            ddlStatus_IssueTo.SelectedValue = "1"; // Default to Active
            hdnID.Value = "AUTO INCREMENT"; // Indicates new record

            // Hide grid and show form
            gvIssueTo.Visible = false;
            divIssueTo_details.Visible = true;

            // Update panels
            UpdatePanel3.Update();
            UpdatePanel4.Update();
        }
        protected void btnSave_IssueTo_Click(object sender, EventArgs e)
        {
            // Determine the status number based on the selected status text
            string statusNo = ddlStatus_IssueTo.SelectedItem.Text == "Disable" ? "0" : "1";

            try
            {
                // Use 'using' statements for proper resource management
                using (MySqlConnection conn = new MySqlConnection(GLOBAL.connStr))
                {
                    conn.Open();
                    string SQL;

                    // Check if we are adding a new item or updating an existing one
                    if (hdnID.Value == "AUTO INCREMENT") // Assuming "0" indicates a new record
                    {
                        SQL = "INSERT INTO signboardequipment_issueto (IssueTo, Type, Status, CreatedDateTime, CreatedBy) " +
                              "VALUES (@IssueTo, 'Equipment', @Status, NOW(), @CurrentUser )";
                    }
                    else // Update existing record
                    {
                        SQL = "UPDATE signboardequipment_issueto SET IssueTo = @IssueTo, Status = @Status, " +
                              "UpdatedDateTime = NOW(), UpdatedBy = @CurrentUser  WHERE ID = @ID";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(SQL, conn))
                    {
                        // Create a dictionary for parameters
                        Dictionary<string, object> ParamDict = new Dictionary<string, object>
                {
                    { "@IssueTo", txtIssueTo.Text.Trim() }, // Trim to remove any leading/trailing spaces
                    { "@Status", statusNo },
                    { "@CurrentUser", GLOBAL.logined_user_name } // Current user identifier
                };

                        // Add ID parameter only if updating an existing record
                        if (hdnID.Value != "AUTO INCREMENT")
                        {
                            ParamDict.Add("@ID", hdnID.Value);
                        }

                        // Use a helper method to add parameters
                        GlobalHelper.PumpParamQuery(cmd, ParamDict);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Log the action after successful execution
                        GlobalHelper.LogToDatabase("SignboardEnquirs.SaveIssueTo", SQL, null, "system");
                    }
                }

                // Refresh the GridView and return to the grid view
                BindGrid_IssueTo();
                btnCancel_IssueTo_Click(sender, e);
            }
            catch (Exception ex)
            {
                // Handle the exception and provide a meaningful message
                Function_Method.MsgBox("Error saving item: " + ex.Message, this.Page, this);
            }
        }
        #endregion
    }
}