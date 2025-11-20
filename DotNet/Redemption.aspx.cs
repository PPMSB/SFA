using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DotNet
{
    public partial class Redemption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Function_Method.isPBM = false;//dont bcc to Saziela
            Function_Method.isWarranty = true;//dont bcc to Keegan
            Check_session();
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

                ClearParameters();
                Session["data_passing"] = "";//in case forget reset
                Session["flag_temp"] = 0;
                Check_DataRequest();

                #region Neil Changes
                if (lblGetRedempID.Text != "")
                {

                    string redemption_Id = lblGetRedempID.Text;
                    double preposted_ap, deducted_ap;
                    GetApValues(redemption_Id, out preposted_ap, out deducted_ap);
                    string script = $@"
        var preposted_ap_value = {preposted_ap.ToString(System.Globalization.CultureInfo.InvariantCulture)};
        var deducted_ap_value = {deducted_ap.ToString(System.Globalization.CultureInfo.InvariantCulture)};
    ";
                    ClientScript.RegisterStartupScript(this.GetType(), "PassApValues", script, true);
                }
                #endregion
            }
        }

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        private void Check_session()
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
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                var getManagerGM = Redemption_Get_Details.RedempApprovalInUseManagerGM(DynAx);
                string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);
                //var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(txtRM.Text));

                if (UserName.Equals(getManagerGM.Item3.ToString()))
                {
                    img1.Visible = false;
                    Button_Redemption_section.Visible = false;

                    Session["flag_temp"] = 4;
                    redemption_Overview(0);
                }

                string temp1 = GLOBAL.data_passing.ToString();
                ddlPointCategory.Items.AddRange(Redemption_Get_Details.get_PointCategory(DynAx).ToArray());
                if (temp1 != "")//data receive
                {
                    if (temp1.Length >= 6)//correct size
                    {
                        var splitTemp1 = temp1.Split('_');
                        if (temp1.Substring(0, 6) == "@RDCM_")//Data receive from CustomerMaster> REDEMPTION
                        {
                            TextBox_Account.Text = temp1.Substring(6);
                            validate();
                            Alt_Addr_function();
                            BtnUpdateDoc.Visible = false;
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemption_section'); ", true);//go to new_applicant_section
                        }
                        else
                        {
                            List<ListItem> cndnPurpose = new List<ListItem>();
                            cndnPurpose = Redemption_Get_Details.get_CNDNPurpose(DynAx);
                            if (cndnPurpose.Count > 1)
                            {
                                ddlCnCategory.Items.AddRange(cndnPurpose.ToArray());
                            }

                            Button_CheckAcc.Visible = false;
                            Button_CheckOutStanding.Visible = true;

                            Reloading_data(DynAx, splitTemp1[1]);

                            lblGetRedempID.Text = splitTemp1[1];

                            var split = Label_Salesman.Text.Split('(', ')');
                            hdsalemanID.Value = split[1];
                            var getAdminRemarks = Redemption_Get_Details.get_gridViewDataAdmin(DynAx, lblGetRedempID.Text);

                            string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, hdsalemanID.Value);//salesapprovalgroupid
                            logger.Info($"Get HOD: {getHod} for Redemption ID: {lblGetRedempID.Text}");
                            var hodSplit = getHod.Split('_');

                            var getHODLvl = Redemption_Get_Details.get_gridViewData2(DynAx, lblGetRedempID.Text);

                            var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(txtRM.Text));
                            //Neil - Solve non-breaking spaces issue when filtering
                            string GLOBAL_logined_user_name = GLOBAL.logined_user_name;
                            GLOBAL_logined_user_name = GLOBAL_logined_user_name.Replace('\u00A0', ' ');

                            UserName = UserName.Replace('\u00A0', ' '); // Store UserName for later use

                            #region CheckNextApprover
                            string GetNextApprover = getAdminRemarks.Item7.ToString(); 
                            bool isCorrectNextApprover = false;

                            if (GetNextApprover.Contains(GLOBAL_logined_user_name) || GLOBAL_logined_user_name.Contains(GetNextApprover))
                            {
                                isCorrectNextApprover = true;
                            }
                            #endregion
                            //=========================================================
                            if (splitTemp1[2] == "Awaiting HOD")
                            {
                                if (hodSplit.Length > 0 && GLOBAL_logined_user_name == UserName &&
                                    getHODLvl.Item6 == "1" || getHODLvl.Item6 == "0" && isCorrectNextApprover == true)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    hod_section.Visible = true;
                                    BtnUpdateDoc.Visible = false;
                                    admin_section.Visible = false;
                                    adminMng_section.Visible = false;
                                    operationMng_section.Visible = false;
                                    Button_Submit.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    Button_Hold.Visible = false;
                                    dvHOD2.Visible = true;
                                }
                                else if (hodSplit.Length > 1 && GLOBAL_logined_user_name == UserName &&
                                    getHODLvl.Item6 == "2" && isCorrectNextApprover == true)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    hod_section.Visible = true;
                                    dvHOD1.Disabled = true;
                                    dvHOD2.Visible = false;
                                    BtnUpdateDoc.Visible = false;
                                    admin_section.Visible = false;
                                    adminMng_section.Visible = false;
                                    operationMng_section.Visible = false;
                                    Button_Submit.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    Button_Hold.Visible = false;
                                }
                                else if (hodSplit.Length > 2 && GLOBAL_logined_user_name == UserName && getHODLvl.Item6 == "3" &&
                                    isCorrectNextApprover == true)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    dvHOD1.Disabled = true;
                                    dvHOD2.Disabled = true;
                                    dvHOD3.Visible = true;
                                    BtnUpdateDoc.Visible = false;
                                    admin_section.Visible = false;
                                    adminMng_section.Visible = false;
                                    operationMng_section.Visible = false;
                                    Button_Submit.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    Button_Hold.Visible = false;
                                }
                                else
                                {
                                    hideButton();
                                }
                                hod_section.Visible = true;
                                divForSalesman.Visible = true;
                            }
                            else if (splitTemp1[2] == "Awaiting Sales Admin")
                            {
                                if (GLOBAL.logined_user_name == amount.SalesAdmin1 || GLOBAL.logined_user_name == amount.SalesAdmin2 || GLOBAL.logined_user_name == amount.SalesAdmin3)
                                {
                                    BtnUpdateDoc.Visible = true;
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    BtnUpdateDoc.Visible = true;
                                    admin_section.Visible = true;
                                    adminMng_section.Visible = false;
                                    operationMng_section.Visible = false;
                                    Button_Submit.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    BtnAmend.Visible = true;
                                    Button_Print.Visible = true;
                                }
                                else
                                {
                                    hideButton();
                                }
                                hod_section.Visible = true;
                                dvHOD2.Visible = true;
                                divForSalesman.Visible = true;
                            }
                            else if (splitTemp1[2] == "Awaiting Sales Admin Manager")
                            {
                                if (GLOBAL.logined_user_name == amount.SAManager1 || GLOBAL.logined_user_name == amount.SAManager2 || GLOBAL.logined_user_name == amount.SAManager3)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    adminMng_section.Visible = true;
                                    Button_Submit.Visible = false;
                                    operationMng_section.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    BtnAmend.Visible = true;
                                    BtnReverse.Visible = true;
                                    Button_Print.Visible = true;
                                }
                                else
                                {
                                    hideButton();
                                }
                                admin_section.Visible = true;
                                hod_section.Visible = true;
                                divForSalesman.Visible = true;
                            }
                            else if (splitTemp1[2] == "Awaiting Credit Control Manager")
                            {
                                if (GLOBAL.logined_user_name == amount.CcManager1 || GLOBAL.logined_user_name == amount.CcManager2 || GLOBAL.logined_user_name == amount.CcManager3)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    adminMng_section.Visible = false;
                                    Button_Submit.Visible = false;
                                    operationMng_section.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    ccMng_section.Visible = true;
                                }
                                else
                                {
                                    hideButton();
                                }
                                admin_section.Visible = true;
                                hod_section.Visible = true;
                                divForSalesman.Visible = true;
                            }
                            else if (splitTemp1[2] == "Awaiting Operation Manager")
                            {
                                if (GLOBAL.logined_user_name == amount.OManager1 || GLOBAL.logined_user_name == amount.OManager2 || GLOBAL.user_id == amount.OManager3)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    operationMng_section.Visible = true;
                                    Button_Reject.Visible = true;
                                    BtnUpdateDoc.Visible = false;
                                    Button_Submit.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                    BtnAmend.Visible = true;
                                }
                                else
                                {
                                    hideButton();
                                }
                                admin_section.Visible = true;
                                hod_section.Visible = true;
                                adminMng_section.Visible = true;
                                divForSalesman.Visible = true;
                            }
                            else if (splitTemp1[2] == "Awaiting General Manager")
                            {
                                if (GLOBAL.logined_user_name == amount.GM1 || GLOBAL.logined_user_name == amount.GM2)
                                {
                                    Button_Approve.Visible = true;//proceed
                                    Button_Reject.Visible = true;
                                    BtnUpdateDoc.Visible = false;
                                    Button_Submit.Visible = false;
                                    Button_CheckAcc.Visible = false;
                                }
                                else
                                {
                                    hideButton();
                                }
                                admin_section.Visible = true;
                                hod_section.Visible = true;
                                adminMng_section.Visible = true;
                                operationMng_section.Visible = true;
                                divForSalesman.Visible = true;
                            }
                            else
                            {//approve/reject/submit
                                admin_section.Visible = true;
                                hod_section.Visible = true;
                                dvHOD2.Visible = true;
                                dvHOD3.Visible = true;
                                adminMng_section.Visible = true;
                                operationMng_section.Visible = true;
                                if (!string.IsNullOrEmpty(txtCcMng.Text))
                                {
                                    ccMng_section.Visible = true;
                                }
                                hideButton();
                            }

                            if (GLOBAL.user_id == "foozm")//Superadmin
                            {
                                BtnAmend.Visible = true;
                                hod_section.Visible = true;
                                admin_section.Visible = true;
                                adminMng_section.Visible = true;
                                operationMng_section.Visible = true;
                                cbAnP.Enabled = true;
                                Button_Approve.Visible = true;
                                Button_AdminApprove.Visible = true;
                                Button_Reject.Visible = true;
                                Button_Hold.Visible = true;
                                Button_Print.Visible = true;
                                divForSalesman.Visible = true;
                                divForApproval.Visible = true;
                                //enableItemPoint();
                            }
                            pnlInvoice.Visible = true;

                            lbl_Status.Text = splitTemp1[2];
                            if (lbl_Status.Text != "" || lbl_Status.Text == "Approved")
                            {
                                DisableCusDetail();
                                Button_Print.Visible = true;
                            }
                            else
                            {
                                hdStatus.Value = "New";
                                lbl_Status.Text = hdStatus.Value;
                                BtnUpdateDoc.Visible = false;
                            }
                            #region 19/9/2025- Request From Keegan - If Category contain start with "Dxxx", CNTYPE is Point Payment Contra
                            if (lbl_Status.Text != "Approved")
                            {
                                string text = ddlPointCategory.SelectedItem.Text;
                                if (!string.IsNullOrEmpty(text) && text[0] == 'D')
                                {
                                    ddlCNType.SelectedValue = "4";
                                    ddlCNType.Enabled = false;
                                }
                            }

                            #endregion
                            GetRedempIdImage();
                        }
                    }
                    Session["data_passing"] = "";
                    Accordion_Redemption.Visible = true;
                    div_Overview.Attributes.Add("style", "display:none");
                    if (lblGetRedempID.Text == "")
                    {
                        Button_Redemption_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                        Button_Redemp_Overview.Attributes.Add("style", "background-color:transparent");
                    }
                    else
                    {
                        Button_Redemption_section.Attributes.Add("style", "background-color:transparent");
                        Button_Redemp_Overview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                    }
                    Button_Signboard_section.Attributes.Add("style", "background-color:transparent");
                    new_applicant_section.Attributes.Add("style", "display:initial");
                }
                else
                {
                    var getHodId = Quotation_Get_Sales_Quotation.get_Empl_Id(DynAx, Session["user_id"].ToString() + "@posim.com.my");//salesapprovalgroupid

                    if (!string.IsNullOrEmpty(getHodId.Item1.ToString()))
                    {
                        Session["hod"] = UserName;
                        Button_Redemp_Enquiries_Click(null, EventArgs.Empty);
                        Button_Redemp_Overview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                        Button_Redemp_Enquiries.Attributes.Add("style", "background-color:transparent");
                    }
                    else
                    {
                        Session["hod"] = "";
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemption_section'); ", true);
                    }
                }
            }
            catch (Exception ER_RD_19)
            {
                Function_Method.MsgBox("ER_RD_19: " + ER_RD_19.Message, this.Page, this);
            }
            DynAx.Dispose();
        }

        protected void Button_Redemption_section_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_Account.Text))
            {
                Button_CheckOutStanding.Visible = false;
                BtnUpdateDoc.Visible = false;
                Button_Submit.Visible = true;
                //Button_Email.Visible = true;
                Button_CustomerMasterList.Enabled = true;
            }

            if (lbl_Status.Text != "New")
            {
                Response.Redirect("Redemption.aspx", true);
                return;
            }
            Accordion_Redemption.Visible = true;
            div_Overview.Attributes.Add("style", "display:none");
            Button_Redemption_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_Signboard_section.Attributes.Add("style", "background-color:transparent");
            Button_Redemp_Overview.Attributes.Add("style", "background-color:transparent");
            Button_Redemp_Enquiries.Attributes.Add("style", "background-color:transparent");
            new_applicant_section.Attributes.Add("style", "display:initial");
        }

        protected void Button_Signboard_section_Click(object sender, EventArgs e)
        {
            div_Overview.Attributes.Add("style", "display:none");
            Button_Signboard_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_Redemption_section.Attributes.Add("style", "background-color:transparent");
            Button_Redemp_Overview.Attributes.Add("style", "background-color:transparent");
        }

        protected void Button_Redemp_Overview_Click(object sender, EventArgs e)
        {
            Accordion_Redemption.Text = "Overview";
            Button_Submit.Visible = false;
            Button_Email.Visible = false;
            Button_CreateVoucher.Visible = false;
            ClearParameters();

            Button_Redemption_section.Attributes.Add("style", "background-color:transparent");
            Button_Redemp_Overview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_Signboard_section.Attributes.Add("style", "background-color:transparent");
            new_applicant_section.Attributes.Add("style", "display:none");
            div_Overview.Attributes.Add("style", "display:initial");
            Button_Redemp_Enquiries.Attributes.Add("style", "background-color:transparent");

            dvOverView.Visible = true;
            redemption_Overview(0);
            dvEnquiry.Visible = false;
        }

        private void Get_data_load_GridRedemption(Axapta DynAx, string redemption_Id)
        {
            gvItemPoint.DataSource = null;
            gvItemPoint.DataBind();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Items", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));
            dt.Columns.Add(new DataColumn("Points", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));

            for (int i = 0; i < 3; i++)
            {
                var row = dt.NewRow();
                row["No."] = i + 1;
                row["Items"] = string.Empty;
                row["ItemCode"] = string.Empty;
                row["Quantity"] = string.Empty;
                row["UnitPrice"] = string.Empty;
                row["Total"] = string.Empty;
                row["Points"] = string.Empty;
                row["InvoiceNo"] = string.Empty;
                row["InvoiceDate"] = string.Empty;
                dt.Rows.Add(row);
            }

            ViewState["CurrentTable"] = dt;
            gvItemPoint.DataSource = dt;
            gvItemPoint.DataBind();

            for (int i = 0; i < gvItemPoint.Rows.Count; i++)
            {
                var getItems = Redemption_Get_Details.get_gridViewData(DynAx, redemption_Id);
                var getItems2 = Redemption_Get_Details.get_gridViewData2(DynAx, redemption_Id);
                var getItemsInvoice = Redemption_Get_Details.get_gridViewDataInvoice(DynAx, redemption_Id);

                if (gvItemPoint.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox item = (TextBox)gvItemPoint.Rows[i].Cells[1].FindControl("txtItems");
                    TextBox itemCode = (TextBox)gvItemPoint.Rows[i].Cells[2].FindControl("txtItemCode");
                    TextBox qty = (TextBox)gvItemPoint.Rows[i].Cells[3].FindControl("txtQty");
                    TextBox unitPrice = (TextBox)gvItemPoint.Rows[i].Cells[4].FindControl("txtUnitPrice");
                    TextBox total = (TextBox)gvItemPoint.Rows[i].Cells[5].FindControl("txtTotal");
                    TextBox points = (TextBox)gvItemPoint.Rows[i].Cells[6].FindControl("txtPoints");
                    TextBox invoice = (TextBox)gvItemPoint.Rows[i].Cells[7].FindControl("txtInvoiceNo");
                    TextBox invoiceDt = (TextBox)gvItemPoint.Rows[i].Cells[8].FindControl("txtInvoiceDate");

                    if (i == 0)
                    {
                        item.Text = getItems.Item1.ToString();//item
                        qty.Text = getItems.Item4.ToString();//quantity
                        unitPrice.Text = getItems.Item7.ToString("#,###,###,##0.00");//Amt1
                        total.Text = getItems.Rest.Item3.ToString("#,###,###,##0.00");//TAmt1
                        points.Text = getItems2.Item1.ToString("#,###,###,##0.00");//PtsVal1
                        itemCode.Text = getItemsInvoice.Item7.Item1;
                        invoice.Text = getItemsInvoice.Item1.ToString();//invoice
                        invoiceDt.Text = getItemsInvoice.Item4;//invoicedt
                    }
                    else if (i == 1)
                    {
                        item.Text = getItems.Item2.ToString();
                        qty.Text = getItems.Item5.ToString();
                        unitPrice.Text = getItems.Rest.Item1.ToString("#,###,###,##0.00");
                        total.Text = getItems.Rest.Item4.ToString("#,###,###,##0.00");
                        points.Text = getItems2.Item2.ToString("#,###,###,##0.00");
                        itemCode.Text = getItemsInvoice.Item7.Item2;
                        invoice.Text = getItemsInvoice.Item2.ToString();
                        invoiceDt.Text = getItemsInvoice.Item5;
                    }
                    else if (i == 2)
                    {
                        item.Text = getItems.Item3.ToString();
                        qty.Text = getItems.Item6.ToString();
                        unitPrice.Text = getItems.Rest.Item2.ToString("#,###,###,##0.00");
                        total.Text = getItems.Rest.Item5.ToString("#,###,###,##0.00");
                        points.Text = getItems2.Item3.ToString("#,###,###,##0.00");
                        itemCode.Text = getItemsInvoice.Item7.Item3;
                        invoice.Text = getItemsInvoice.Item3.ToString();
                        invoiceDt.Text = getItemsInvoice.Item6;
                    }

                    //if (itemCode.Text == "" && !isItemRemoved)
                    //{
                    //    ddlRedempType.Items.RemoveAt(2);//sales order
                    //    isItemRemoved = true;
                    //}
                }
                txtRM.Text = getItems2.Item4.ToString("#,###,###,##0.00");
                txtPts.Text = getItems2.Item5.ToString("#,###,###,##0.00");
            }
        }

        private void Reloading_data(Axapta DynAx, string redemption_Id)
        {
            hdRedemID.Value = redemption_Id;
            var getRedemp = Redemption_Get_Details.get_redemption(DynAx, redemption_Id);
            var getStat = Redemption_Get_Details.get_redempStat(DynAx, redemption_Id);
            var getAdminStat = Redemption_Get_Details.get_redempStat2(DynAx, redemption_Id);
            var getInvoice = Redemption_Get_Details.get_gridViewDataInvoice(DynAx, redemption_Id);
            var getAdminRemarks = Redemption_Get_Details.get_gridViewDataAdmin(DynAx, redemption_Id);
            TextBox_Account.Text = getRedemp.CustAcc;
            var getLatestTransDate = Redemption_Get_Details.get_latestInvoiceTrans(DynAx, TextBox_Account.Text);
            var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, TextBox_Account.Text);

            validate();//auto fill up [TextBox_Account, Label_CustName,Label_Address] in Customer accordion

            Textbox_TelNo.Text = getRedemp.CustPhone;
            Textbox_ContactPerson.Text = getRedemp.CustContact;
            txtDeliveryAddr.Text = getRedemp.CustAddr;
            txtBeneficiaryName.Text = getRedemp.benefitName;

            if (getRedemp.interestWaiver == 1)
            {
                cbInterestWaive.Checked = true;
            }
            else
            {
                cbInterestWaive.Checked = false;
            }

            // Jerry 2024-11-15 Fixed Beneficiary Name radio buttons
            //benefitName, benefitIc, benefitTaxNo, 
            //LF_WebRedemp Benefit_Name (5), Benefit_IC (6), Benefit_Tax_No (7)

            //if (!string.IsNullOrEmpty(getRedemp.Item7) && string.IsNullOrEmpty(getRedemp.Item6))//no tax number || no ic
            //{
            //    rblBeneficiaryName.SelectedValue = "1";//company
            //}
            //else if (string.IsNullOrEmpty(getRedemp.Item5) && string.IsNullOrEmpty(getRedemp.Item7))
            //{
            //    rblBeneficiaryName.SelectedValue = "2";//Other
            //}
            //else
            //{
            //    rblBeneficiaryName.SelectedValue = "0";//Owner
            //}

            var accname = Redemption_Get_Details.get_accName(DynAx, getRedemp.CustAcc);

            if (!string.IsNullOrEmpty(getRedemp.benefitName) && string.IsNullOrEmpty(getRedemp.benefitIc) && string.IsNullOrEmpty(getRedemp.benefitTaxNo))//has name, but no tax number & no ic
            {
                // TODO check if Item5 = accname, if yes, selectedvalue = 1, if no, = 2
                if (accname == getRedemp.benefitName)
                {
                    rblBeneficiaryName.SelectedValue = "1";//company
                }
                else
                {
                    rblBeneficiaryName.SelectedValue = "2";//other
                }
            }
            else
            {
                rblBeneficiaryName.SelectedValue = "0";//Owner
            }

            // Jerry 2024-11-15 end

            txtIc.Text = getRedemp.benefitIc;
            if (getRedemp.reason != "")
            {
                specialReason.Visible = true;
                txtReason.Text = getRedemp.reason;
            }

            //upload_section.Visible = false;
            display_section.Visible = true;
            var invoiceNo1 = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, getInvoice.Item1);
            if (invoiceNo1.Item1 != "")
            {
                btnGetInvoice1.Text = invoiceNo1.Item1;
            }
            else
            {
                lblInvoice1.Visible = false;
                btnGetInvoice1.Visible = false;
            }

            var invoiceNo2 = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, getInvoice.Item2);
            if (invoiceNo2.Item1 != "")
            {
                btnGetInvoice2.Text = invoiceNo2.Item1;
            }
            else
            {
                lblInvoice2.Visible = false;
                btnGetInvoice2.Visible = false;
            }

            var invoiceNo3 = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, getInvoice.Item3);
            if (invoiceNo3.Item1 != "")
            {
                btnGetInvoice3.Text = invoiceNo3.Item1;
            }
            else
            {
                lblInvoice3.Visible = false;
                btnGetInvoice3.Visible = false;
            }

            string deliveryType = getStat.Item2;
            //string paymentType = getStat.Item3;
            string bankType = getStat.Item4;

            switch (deliveryType)
            {
                case "Customer/Workshop":
                    ddlDelivery.SelectedValue = "1";
                    break;
                case "PPM Warehouse/Salesman Deliver Via S/O":
                    ddlDelivery.SelectedValue = "2";
                    break;
                default:
                    ddlDelivery.SelectedValue = "0";
                    break;
            }

            switch (bankType)
            {
                case "1":
                    rblBank.SelectedValue = "1";
                    break;
                case "2":
                    rblBank.SelectedValue = "2";
                    break;
                default:
                    rblBank.SelectedValue = "0";
                    break;
            }

            Get_data_load_GridRedemption(DynAx, redemption_Id);

            //--------------------------------------------------------HOD------------------------------------------------
            txtHod1.Text = getStat.Item6;
            txtHod2.Text = getAdminStat.Rest.Item1;
            txtHod3.Text = getAdminStat.Rest.Item2;
            lblHODgetLastInvoice.Text = getLatestTransDate.Item1 + " " + getLatestTransDate.Item2;
            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("PointBalance"))
            {
                DynAx.TTSBegin();
                DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", "AccountNum", TextBox_Account.Text));

                var LPCF = DynRec.get_Field("TPBalance");
                var APCF = DynRec.Call("getAddBalance");
                var LPBal = DynRec.Call("getTotBalance", get_Point.Item2.ToString());
                var APBal = DynRec.Call("getTotAddBalance", get_Point.Item2.ToString());

                // Jerry 2024-12-19 Recalculation for Preposted, Deducted and Balance A&P and Loyalty points

                

                string lp = Convert.ToDouble(LPCF).ToString("#,###,###,##0.00");
                string ap = Convert.ToDouble(APCF).ToString("#,###,###,##0.00");
                string lpBal = Convert.ToDouble(LPBal).ToString("#,###,###,##0.00");
                string apBal = Convert.ToDouble(APBal).ToString("#,###,###,##0.00");

                // Total redeemed point
                double redeemed_point = Convert.ToDouble(txtPts.Text.Replace(",", ""));

                // Preposted A&P and Loyalty points
                double preposted_ap = Convert.ToDouble(ap);
                double preposted_lp = Convert.ToDouble(lp);

                // Deducted A&P points
                double deducted_ap = !string.IsNullOrEmpty(getAdminRemarks.Item1?.ToString())
                                        ? Convert.ToDouble(getAdminRemarks.Item1.ToString())
                                        : 0;

                // Deducted Loyalty points (redeemed points deduct A&P points first, balance deduct by lp
                double deducted_lp = redeemed_point - deducted_ap;


                // Assign deducted A&P points values to both checkbox and textbox
                if (deducted_ap > 0)
                {
                    cbAnP.Checked = true;
                    cbAnP.Text = getAdminRemarks.Item1;
                    txtAnP.Visible = false;
                    txtAnP.Text = getAdminRemarks.Item1;
                } else
                {
                    cbAnP.Text = "";
                }

                // HOD Remarks - "Balance A&P Point" - PrePosted/Opening
                lblHODaP.Text = preposted_ap.ToString("#,###,###,##0.00");

                // Sales Admin - "Balance A&P Point" - Balance/Closing
                lblBalanceAP.Text = (preposted_ap - deducted_ap).ToString("#,###,###,##0.00");

                // Sales Admin - "PrePosted A&P Point" - PrePosted/Opening
                lblPrePostedAP.Text = preposted_ap.ToString("#,###,###,##0.00");

                // Sales Admin - "Balance Loyalty Point" = Balance/Closing
                lblLoyaltyPoint.Text = (preposted_lp - deducted_lp).ToString("#,###,###,##0.00");

                // Sales Admin - "PrePosted Loyalty Point" - PrePosted/Opening
                lblPrepostedLoyalty.Text = preposted_lp.ToString("#,###,###,##0.00");


                var LPoint = getAdminRemarks.Item2.Split('(');
                if (!string.IsNullOrEmpty(LPoint[0]))
                {

                    lblPrepostedLoyalty.Text = lblPrepostedLoyalty.Text + " (" + LPoint[1] + ")";
                }

                if (!string.IsNullOrEmpty(getAdminRemarks.Item3))
                {

                    lblPrePostedAP.Text = lblPrePostedAP.Text + " (" + getAdminRemarks.Item4 + " " + getAdminRemarks.Item5 + ")";
                }


                /*
                cbAnP.Text = APBal.ToString();
                decimal balanceValue;
                if (decimal.TryParse(Convert.ToString(APCF), out balanceValue))
                {
                    string BalanceAP = balanceValue.ToString("#,###,###,##0.00");
                    lblBalanceAP.Text = BalanceAP;
                    lblHODaP.Text = BalanceAP;
                }
                */

                //if (!string.IsNullOrEmpty(getAdminRemarks.Item1.ToString()))
                //{
                // Jerry 2024-11-25 Set value to txtAnP as value of AnP will be capture from this textbox on Approve/Update, and show the correct LP balance 

                //cbAnP.Checked = true;
                //cbAnP.Text = getAdminRemarks.Item1;
                //txtAnP.Visible = false;

                //lblLoyaltyPoint.Text = Convert.ToDouble(LPBal).ToString("#,###,###,##0.00");

                //double totalPrepostedAP = Convert.ToDouble(lblHODaP.Text) - Convert.ToDouble(getAdminRemarks.Item1);
                //lblBalanceAP.Text = totalPrepostedAP.ToString("#,###,###,##0.00");


                //cbAnP.Checked = true;
                //cbAnP.Text = getAdminRemarks.Item1;
                //txtAnP.Visible = false;
                //txtAnP.Text = getAdminRemarks.Item1;

                //var total_redemption_points = Convert.ToDouble(txtPts.Text.Replace(",", ""));
                //var balance_loyalty_points = Convert.ToDouble(LPBal) - total_redemption_points;
                //if (balance_loyalty_points < 0)
                //    lblLoyaltyPoint.Text = "0.00";
                //else 
                //    lblLoyaltyPoint.Text = balance_loyalty_points.ToString("#,###,###,##0.00");

                //double totalPrepostedAP = Convert.ToDouble(lblHODaP.Text) - Convert.ToDouble(getAdminRemarks.Item1);
                //lblBalanceAP.Text = totalPrepostedAP.ToString("#,###,###,##0.00");

                // Jerry 2024-11-25 End
                //}
                //else
                //{
                //    double balance = Convert.ToDouble(Label_Point.Text) - Convert.ToDouble(txtPts.Text);
                //    if (balance < 0)
                //    {
                //        lblLoyaltyPoint.Text = "0.00";
                //    }
                //    else
                //    {
                //        lblLoyaltyPoint.Text = balance.ToString("#,###,###,##0.00");
                //    }

                //    DivBeforeSubmit.Visible = true;
                //    cbAnP.Text = "";
                //}

                //var LPoint = getAdminRemarks.Item2.Split('(');
                //if (!string.IsNullOrEmpty(LPoint[0]))
                //{
                //    double loyaltyPoint = Convert.ToDouble(LPCF);
                //    lblPrepostedLoyalty.Text = loyaltyPoint.ToString("#,###,###,##0.00") + " (" + LPoint[1] + ")";
                //}
                //else
                //{
                //    lblPrepostedLoyalty.Text = Label_Point.Text;
                //}

                // Jerry 2024-12-19 Recalculation for Preposted, Deducted and Balance A&P and Loyalty points - END
            }

            // Jerry 2024-12-19 Recalculation for Preposted, Deducted and Balance A&P and Loyalty points
            //if (!string.IsNullOrEmpty(getAdminRemarks.Item3))
            //{
            //    double apPoint = Convert.ToDouble(getAdminRemarks.Item3);
            //    lblPrePostedAP.Text = apPoint.ToString("#,###,###,##0.00") + " (" + getAdminRemarks.Item4 + " " + getAdminRemarks.Item5 + ")";
            //}
            //else
            //{
            //    lblPrePostedAP.Text = lblHODaP.Text;
            //}
            // Jerry 2024-12-19 Recalculation for Preposted, Deducted and Balance A&P and Loyalty points - END

            //----------------------------------------------------------Admin---------------------------------------------
            lblInvoiceTrans.Text = getLatestTransDate.Item1 + " " + getLatestTransDate.Item2;
            var getEOLEORBDP = Redemption_Get_Details.getEORdetails(DynAx, TextBox_Account.Text);
            //double eol = getEOLEORBDP.Item5 + getEOLEORBDP.Item6 / getEOLEORBDP.Item7;
            double eor = getEOLEORBDP.Rest.Item1 * 100 / getEOLEORBDP.Item7;

            // Jerry 2024-11-18 update NaN to N/A
            //lblEOR.Text = eor.ToString("#,###,###,##0.00");            
            //lblBDP.Text = "NaN";
            if (double.IsNaN(eor))
            {
                lblEOR.Text = "N/A"; // or whatever default text you want to show
            }
            else
            {
                lblEOR.Text = eor.ToString("#,###,###,##0.00");
            }
            lblBDP.Text = "N/A";
            // Jerry 2024-11-18 End

            //lblEOL.Text = eol.ToString();
            var getPOdetails = Redemption_Get_Details.GetPurchaseOrderDetails(DynAx, redemption_Id);
            var getJournalId = Redemption_Get_Details.getPPMTP_JournalId(DynAx, TextBox_Account.Text, redemption_Id);

            if (getJournalId == "")
            {
                getJournalId = Redemption_Get_Details.getPPMTP_JournalId_AfterPosting(DynAx, TextBox_Account.Text, redemption_Id);
            }

            // Jerry 2024-11-15 Fixed Beneficiary Name radio buttons
            // rblBeneficiaryName.SelectedValue = getStat.Item4;
            // Jerry 2024-11-15 End

            ddlRedempType.SelectedValue = getAdminRemarks.Item6;
            if (ddlRedempType.SelectedValue == "1")
            {
                lblRedempTypeNo.Text = "Purchase Order No.: ";
                vendor_section.Visible = true;
                lblGetVendor.Text = getPOdetails.Item3.ToString();
                lblGetRedempTypeNo.Text = getPOdetails.Item1.ToString();
            }
            else if (ddlRedempType.SelectedValue == "2")
            {
                string soID = Redemption_Get_Details.GetSoID(DynAx, redemption_Id);
                lblRedempTypeNo.Text = "Sales Order No.: ";
                lblGetRedempTypeNo.Text = soID;
            }
            else if (ddlRedempType.SelectedValue == "3")
            {
                lblRedempTypeNo.Text = "Credit Note No.: ";
                //lblGetRedempType.Text;
            }

            if (getJournalId != "")
            {
                lblJournalId.Visible = true;
                lblGetJournalID.Visible = true;
                lblGetJournalID.Text = getJournalId;
            }
            ddlCnCategory.SelectedItem.Text = getAdminStat.Rest.Item3;
            //else
            //{
            //    lblCnCategory.Visible = false;
            //    ddlCnCategory.Visible = false;
            //}
            ddlCNType.SelectedValue = getAdminStat.Item3;
            lblGetProcessStat.Text = getStat.Item1;

            DdlSalesmanRemark.SelectedValue = "3";
            txtRemark.Text = getStat.Item5;
            txtHeader.Text = getAdminStat.Item1;
            txtCNreason.Text = getAdminStat.Item2;
            ddlLedgerAcc.SelectedItem.Text = getAdminStat.Item4;
            ddlPointCategory.SelectedItem.Text = getAdminStat.Item5;
            txtAdmin.Text = getStat.Item7;

            //var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, TextBox_Account.Text);
            //string custClass = tuple_getCustInfo.Item6.Trim();
            //if (custClass.Contains("CC01") || custClass.Contains("CC02") || custClass.Contains("CC03"))
            //{
            //    ddlLedgerAcc.Visible = false;
            //}
            //-------------------------------------------Admin Manager-----------------------------------------------
            txtAdminMng.Text = getStat.Rest.Item2;
            if (getAdminStat.Item6.ToString() != "0")
            {
                cbSpecialApproval.Checked = true;
            }

            //------------------------------------------Operation Manager---------------------------------------------------
            txtOperationMng.Text = getStat.Rest.Item3;
            if (getAdminStat.Item7.ToString() != "0")
            {
                cbGMApproval.Checked = true;
            }

            lblProcessDate.Visible = true;
            lblProcessStatus.Visible = true;
            lblGetProcessDt.Text = getStat.Rest.Item1;

            //------------------------------------------Credit Control Manager---------------------------------------------------
            txtCcMng.Text = getStat.Rest.Item4;
        }

        //private void Reloading_data2(Axapta DynAx, string redemption_Id)
        //{
        //    hdRedemID.Value = redemption_Id;
        //    var getRedemp = Redemption_Get_Details.get_redemption(DynAx, redemption_Id);
        //    var getStat = Redemption_Get_Details.get_redempStat(DynAx, redemption_Id);
        //    var getAdminStat = Redemption_Get_Details.get_redempStat2(DynAx, redemption_Id);
        //    var getInvoice = Redemption_Get_Details.get_gridViewDataInvoice(DynAx, redemption_Id);
        //    var getAdminRemarks = Redemption_Get_Details.get_gridViewDataAdmin(DynAx, redemption_Id);
        //    TextBox_Account.Text = getRedemp.Item1;
        //    var getLatestTransDate = Redemption_Get_Details.get_latestInvoiceTrans(DynAx, TextBox_Account.Text);
        //    var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, TextBox_Account.Text);

        //    validate();//auto fill up [TextBox_Account, Label_CustName,Label_Address] in Customer accordion

        //    Textbox_TelNo.Text = getRedemp.Item2;
        //    Textbox_ContactPerson.Text = getRedemp.Item3;
        //    txtDeliveryAddr.Text = getRedemp.Item4;
        //    txtBeneficiaryName.Text = getRedemp.Item5;

        //    if (getRedemp.Rest.Item2 == 1)
        //    {
        //        cbInterestWaive.Checked = true;
        //    }
        //    else
        //    {
        //        cbInterestWaive.Checked = false;
        //    }

        //    // Jerry 2024-11-15 Fixed Beneficiary Name radio buttons
        //    //benefitName, benefitIc, benefitTaxNo, 
        //    //LF_WebRedemp Benefit_Name (5), Benefit_IC (6), Benefit_Tax_No (7)

        //    //if (!string.IsNullOrEmpty(getRedemp.Item7) && string.IsNullOrEmpty(getRedemp.Item6))//no tax number || no ic
        //    //{
        //    //    rblBeneficiaryName.SelectedValue = "1";//company
        //    //}
        //    //else if (string.IsNullOrEmpty(getRedemp.Item5) && string.IsNullOrEmpty(getRedemp.Item7))
        //    //{
        //    //    rblBeneficiaryName.SelectedValue = "2";//Other
        //    //}
        //    //else
        //    //{
        //    //    rblBeneficiaryName.SelectedValue = "0";//Owner
        //    //}

        //    var accname = Redemption_Get_Details.get_accName(DynAx, getRedemp.Item1);

        //    if (!string.IsNullOrEmpty(getRedemp.Item5) && string.IsNullOrEmpty(getRedemp.Item6) && string.IsNullOrEmpty(getRedemp.Item7))//has name, but no tax number & no ic
        //    {
        //        // TODO check if Item5 = accname, if yes, selectedvalue = 1, if no, = 2
        //        if (accname == getRedemp.Item5)
        //        {
        //            rblBeneficiaryName.SelectedValue = "1";//company
        //        }
        //        else
        //        {
        //            rblBeneficiaryName.SelectedValue = "2";//other
        //        }
        //    }
        //    else
        //    {
        //        rblBeneficiaryName.SelectedValue = "0";//Owner
        //    }

        //    // Jerry 2024-11-15 end

        //    txtIc.Text = getRedemp.Item6;
        //    if (getRedemp.Rest.Item1 != "")
        //    {
        //        specialReason.Visible = true;
        //        txtReason.Text = getRedemp.Rest.Item1;
        //    }

        //    //upload_section.Visible = false;
        //    display_section.Visible = true;
        //    var invoiceNo1 = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, getInvoice.Item1);
        //    if (invoiceNo1.Item1 != "")
        //    {
        //        btnGetInvoice1.Text = invoiceNo1.Item1;
        //    }
        //    else
        //    {
        //        lblInvoice1.Visible = false;
        //        btnGetInvoice1.Visible = false;
        //    }

        //    var invoiceNo2 = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, getInvoice.Item2);
        //    if (invoiceNo2.Item1 != "")
        //    {
        //        btnGetInvoice2.Text = invoiceNo2.Item1;
        //    }
        //    else
        //    {
        //        lblInvoice2.Visible = false;
        //        btnGetInvoice2.Visible = false;
        //    }

        //    var invoiceNo3 = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, getInvoice.Item3);
        //    if (invoiceNo3.Item1 != "")
        //    {
        //        btnGetInvoice3.Text = invoiceNo3.Item1;
        //    }
        //    else
        //    {
        //        lblInvoice3.Visible = false;
        //        btnGetInvoice3.Visible = false;
        //    }

        //    string deliveryType = getStat.Item2;
        //    //string paymentType = getStat.Item3;
        //    string bankType = getStat.Item4;

        //    switch (deliveryType)
        //    {
        //        case "Customer/Workshop":
        //            ddlDelivery.SelectedValue = "1";
        //            break;
        //        case "PPM Warehouse/Salesman Deliver Via S/O":
        //            ddlDelivery.SelectedValue = "2";
        //            break;
        //        default:
        //            ddlDelivery.SelectedValue = "0";
        //            break;
        //    }

        //    switch (bankType)
        //    {
        //        case "1":
        //            rblBank.SelectedValue = "1";
        //            break;
        //        case "2":
        //            rblBank.SelectedValue = "2";
        //            break;
        //        default:
        //            rblBank.SelectedValue = "0";
        //            break;
        //    }

        //    Get_data_load_GridRedemption(DynAx, redemption_Id);

        //    //--------------------------------------------------------HOD------------------------------------------------
        //    txtHod1.Text = getStat.Item6;
        //    txtHod2.Text = getAdminStat.Rest.Item1;
        //    txtHod3.Text = getAdminStat.Rest.Item2;
        //    lblHODgetLastInvoice.Text = getLatestTransDate.Item1 + " " + getLatestTransDate.Item2;
        //    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("PointBalance"))
        //    {
        //        DynAx.TTSBegin();
        //        DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", "AccountNum", TextBox_Account.Text));

        //        var LPCF = DynRec.get_Field("TPBalance");
        //        var APCF = DynRec.Call("getAddBalance");
        //        var LPBal = DynRec.Call("getTotBalance", get_Point.Item2.ToString());
        //        var APBal = DynRec.Call("getTotAddBalance", get_Point.Item2.ToString());

        //        //lblBalanceAP.Text = BalanceAP.ToString("#,###,###,##0.00");
        //        cbAnP.Text = APBal.ToString();
        //        decimal balanceValue;
        //        if (decimal.TryParse(Convert.ToString(APCF), out balanceValue))
        //        {
        //            string BalanceAP = balanceValue.ToString("#,###,###,##0.00");
        //            lblBalanceAP.Text = BalanceAP;
        //            lblHODaP.Text = BalanceAP;
        //        }

        //        if (!string.IsNullOrEmpty(getAdminRemarks.Item1.ToString()))
        //        {
        //            // Jerry 2024-11-25 Set value to txtAnP as value of AnP will be capture from this textbox on Approve/Update, and show the correct LP balance 

        //            //cbAnP.Checked = true;
        //            //cbAnP.Text = getAdminRemarks.Item1;
        //            //txtAnP.Visible = false;

        //            //lblLoyaltyPoint.Text = Convert.ToDouble(LPBal).ToString("#,###,###,##0.00");

        //            //double totalPrepostedAP = Convert.ToDouble(lblHODaP.Text) - Convert.ToDouble(getAdminRemarks.Item1);
        //            //lblBalanceAP.Text = totalPrepostedAP.ToString("#,###,###,##0.00");


        //            cbAnP.Checked = true;
        //            cbAnP.Text = getAdminRemarks.Item1;
        //            txtAnP.Visible = false;
        //            txtAnP.Text = getAdminRemarks.Item1;

        //            var total_redemption_points = Convert.ToDouble(txtPts.Text.Replace(",", ""));
        //            var balance_loyalty_points = Convert.ToDouble(LPBal) - total_redemption_points;
        //            if (balance_loyalty_points < 0)
        //                lblLoyaltyPoint.Text = "0.00";
        //            else 
        //                lblLoyaltyPoint.Text = balance_loyalty_points.ToString("#,###,###,##0.00");

        //            double totalPrepostedAP = Convert.ToDouble(lblHODaP.Text) - Convert.ToDouble(getAdminRemarks.Item1);
        //            lblBalanceAP.Text = totalPrepostedAP.ToString("#,###,###,##0.00");

        //            // Jerry 2024-11-25 End
        //        }
        //        else
        //        {
        //            double balance = Convert.ToDouble(Label_Point.Text) - Convert.ToDouble(txtPts.Text);
        //            if (balance < 0)
        //            {
        //                lblLoyaltyPoint.Text = "0.00";
        //            }
        //            else
        //            {
        //                lblLoyaltyPoint.Text = balance.ToString("#,###,###,##0.00");
        //            }

        //            DivBeforeSubmit.Visible = true;
        //            cbAnP.Text = "";
        //        }

        //        var LPoint = getAdminRemarks.Item2.Split('(');
        //        if (!string.IsNullOrEmpty(LPoint[0]))
        //        {
        //            double loyaltyPoint = Convert.ToDouble(LPCF);
        //            lblPrepostedLoyalty.Text = loyaltyPoint.ToString("#,###,###,##0.00") + " (" + LPoint[1] + ")";
        //        }
        //        else
        //        {
        //            lblPrepostedLoyalty.Text = Label_Point.Text;
        //        }
        //    }


        //    if (!string.IsNullOrEmpty(getAdminRemarks.Item3))
        //    {
        //        double apPoint = Convert.ToDouble(getAdminRemarks.Item3);
        //        lblPrePostedAP.Text = apPoint.ToString("#,###,###,##0.00") + " (" + getAdminRemarks.Item4 + " " + getAdminRemarks.Item5 + ")";
        //    }
        //    else
        //    {
        //        lblPrePostedAP.Text = lblHODaP.Text;
        //    }

        //    //----------------------------------------------------------Admin---------------------------------------------
        //    lblInvoiceTrans.Text = getLatestTransDate.Item1 + " " + getLatestTransDate.Item2;
        //    var getEOLEORBDP = Redemption_Get_Details.getEORdetails(DynAx, TextBox_Account.Text);
        //    //double eol = getEOLEORBDP.Item5 + getEOLEORBDP.Item6 / getEOLEORBDP.Item7;
        //    double eor = getEOLEORBDP.Rest.Item1 * 100 / getEOLEORBDP.Item7;

        //    // Jerry 2024-11-18 update NaN to N/A
        //    //lblEOR.Text = eor.ToString("#,###,###,##0.00");            
        //    //lblBDP.Text = "NaN";
        //    if (double.IsNaN(eor))
        //    {
        //        lblEOR.Text = "N/A"; // or whatever default text you want to show
        //    }
        //    else
        //    {
        //        lblEOR.Text = eor.ToString("#,###,###,##0.00");
        //    }
        //    lblBDP.Text = "N/A";
        //    // Jerry 2024-11-18 End

        //    //lblEOL.Text = eol.ToString();
        //    var getPOdetails = Redemption_Get_Details.GetPurchaseOrderDetails(DynAx, redemption_Id);
        //    var getJournalId = Redemption_Get_Details.getPPMTP_JournalId(DynAx, TextBox_Account.Text, redemption_Id);

        //    // Jerry 2024-11-15 Fixed Beneficiary Name radio buttons
        //    // rblBeneficiaryName.SelectedValue = getStat.Item4;
        //    // Jerry 2024-11-15 End

        //    ddlRedempType.SelectedValue = getAdminRemarks.Item6;
        //    if (ddlRedempType.SelectedValue == "1")
        //    {
        //        lblRedempTypeNo.Text = "Purchase Order No.: ";
        //        vendor_section.Visible = true;
        //        lblGetVendor.Text = getPOdetails.Item3.ToString();
        //        lblGetRedempTypeNo.Text = getPOdetails.Item1.ToString();
        //    }
        //    else if (ddlRedempType.SelectedValue == "2")
        //    {
        //        string soID = Redemption_Get_Details.GetSoID(DynAx, redemption_Id);
        //        lblRedempTypeNo.Text = "Sales Order No.: ";
        //        lblGetRedempTypeNo.Text = soID;
        //    }
        //    else if (ddlRedempType.SelectedValue == "3")
        //    {
        //        lblRedempTypeNo.Text = "Credit Note No.: ";
        //        //lblGetRedempType.Text;
        //    }

        //    if (getJournalId != "")
        //    {
        //        lblJournalId.Visible = true;
        //        lblGetJournalID.Visible = true;
        //        lblGetJournalID.Text = getJournalId;
        //    }
        //    ddlCnCategory.SelectedItem.Text = getAdminStat.Rest.Item3;
        //    //else
        //    //{
        //    //    lblCnCategory.Visible = false;
        //    //    ddlCnCategory.Visible = false;
        //    //}
        //    ddlCNType.SelectedValue = getAdminStat.Item3;
        //    lblGetProcessStat.Text = getStat.Item1;

        //    DdlSalesmanRemark.SelectedValue = "3";
        //    txtRemark.Text = getStat.Item5;
        //    txtHeader.Text = getAdminStat.Item1;
        //    txtCNreason.Text = getAdminStat.Item2;
        //    ddlLedgerAcc.SelectedItem.Text = getAdminStat.Item4;
        //    ddlPointCategory.SelectedItem.Text = getAdminStat.Item5;
        //    txtAdmin.Text = getStat.Item7;

        //    //var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, TextBox_Account.Text);
        //    //string custClass = tuple_getCustInfo.Item6.Trim();
        //    //if (custClass.Contains("CC01") || custClass.Contains("CC02") || custClass.Contains("CC03"))
        //    //{
        //    //    ddlLedgerAcc.Visible = false;
        //    //}
        //    //-------------------------------------------Admin Manager-----------------------------------------------
        //    txtAdminMng.Text = getStat.Rest.Item2;
        //    if (getAdminStat.Item6.ToString() != "0")
        //    {
        //        cbSpecialApproval.Checked = true;
        //    }

        //    //------------------------------------------Operation Manager---------------------------------------------------
        //    txtOperationMng.Text = getStat.Rest.Item3;
        //    if (getAdminStat.Item7.ToString() != "0")
        //    {
        //        cbGMApproval.Checked = true;
        //    }

        //    lblProcessDate.Visible = true;
        //    lblProcessStatus.Visible = true;
        //    lblGetProcessDt.Text = getStat.Rest.Item1;

        //    //------------------------------------------Credit Control Manager---------------------------------------------------
        //    txtCcMng.Text = getStat.Rest.Item4;
        //}

        private void ClearParameters()
        {
            btnDisplay.Visible = false;
            lblDisplay.Visible = false;
            Button_AdminApprove.Visible = false;
            Button_Approve.Visible = false;
            Button_Reject.Visible = false;
            Button_Hold.Visible = false;
            BtnAmend.Visible = false;
            display_section.Visible = false;
            hdStatus.Value = "";
            initialize_GridViewItemPoint();
        }

        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            // Jerry 2024-12-19 Prevent submission if Customer Account number is empty
            if (String.IsNullOrEmpty(TextBox_Account.Text))
            {
                Function_Method.MsgBox("Please enter customer account number", this.Page, this);
                return;
            }
            // Jerry 2024-12-19 Prevent submission if Customer Account number is empty - END

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebRedemp"))
                {
                    Function_Method.isVPPPCampaign = false;
                    DynAx.TTSBegin();
                    var Email = Redemption_Get_Details.getRedempApprovalEmail();
                    string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);

                    if (Save_LF_WebRedemp(DynRec, 1, "SB: by " + UserName + " on " + DateTime.Now + "<br/>"))
                    {
                        DynRec.Call("insert");
                    DynAx.TTSCommit();

                    hdRedemID.Value = DynRec.get_Field("Rdemt_ID").ToString();
                    DynAx.TTSAbort();

                    // Jerry 2024-12-18 update pic as long as fileUpload is not empty
                    Function_Method.UserLog(GLOBAL.user_id + " check if fileUpload not empty.");
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        // Check for the specific files by their names
                        HttpPostedFile fileUpload = Request.Files["fileUpload"];

                        // Check if any of the files are not null and have content
                        if (fileUpload != null && fileUpload.ContentLength > 0)
                        {
                            // At least one file is uploaded and has content
                            Function_Method.UserLog(GLOBAL.user_id + " uploading image");
                            UploadPic();
                            Function_Method.UserLog(GLOBAL.user_id + " upload image done.");
                        }
                    }
                    // Jerry 2024-12-18 update pic as long as fileUpload is not empty - END

                    if (lbl_Status.Text == "New")
                    {
                        // Jerry 2024-12-18 update pic as long as fileUpload is not empty
                        // UploadPic();
                        // Jerry 2024-12-18 update pic as long as fileUpload is not empty - END
                        lbl_Status.Text = "Awaiting HOD";
                    }
                    hdStatus.Value = lbl_Status.Text;
                    Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Submitted Redemption Application (" + hdStatus.Value + " " + hdRedemID.Value + ")",
                        Email.Item1, Email.Item2 + "," + Email.Item3, RedempEmailContent("Submitted"));//email to hod
                    }
                    else
                    {
                        Function_Method.MsgBox("Insufficient Points Balance.", this.Page, this);
                    }
                }
            }
            catch (Exception ER_RD_01)
            {
                Function_Method.MsgBox("ER_RD_01: " + ER_RD_01.ToString(), this.Page, this);
                return;
            }
            finally
            {
                DynAx.Dispose();
                Function_Method.MsgBox("Redemption ID: " + hdRedemID.Value.ToString() + " have been submitted.", this.Page, this);
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview section
            }
        }

        protected void UploadPic()
        {
            HttpPostedFile file = Request.Files["fileUpload"];

            try
            {
                // Jerry 2024-12-18 update pic as long as fileUpload is not empty
                //if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(lnkBtn1.Text))
                if (file != null && file.ContentLength > 0)
                // Jerry 2024-12-18 update pic as long as fileUpload is not empty - END
                {
                    //Jerry 2024-11-04 Use Customer Account No. as folder
                    //string path = @"e:/Redemption/" + Label_Salesman.Text + "/" + Label_CustName.Text;
                    string path = @"e:/Redemption/" + Label_Salesman.Text + "/" + TextBox_Account.Text; 
                    if (!Directory.Exists(@"e:/Redemption/" + Label_Salesman.Text))
                    {
                        Directory.CreateDirectory(@"e:/Redemption/" + Label_Salesman.Text);
                        Directory.CreateDirectory(path);
                        logger.Info("Created directory: {0}", path);
                    }
                    else
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            logger.Info("Created directory: {0}", path);
                        }
                    }

                    MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                    string query = "insert into redemp_tbl(redemp_ID,salesman_name, customer_name, pictures) values (@c1,@s1,@c2,@p1)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFile currentFile = Request.Files[i];
                        string filename = currentFile.FileName;
                        
                        // Jerry 2024-12-23 Sanitize file name
                        filename = Function_Method.SanitizeFilename(filename);
                        // Jerry 2024-12-23 Sanitize file name - END 

                        string filePath = (path + "/" + filename);

                        // Log the file path
                        logger.Info("Saving file to: {0}", filePath);
                        if (Path.GetExtension(filename).ToLower() == ".pdf")
                        {
                            file.SaveAs(filePath);
                            logger.Info("PDF file saved: {0}", filePath);
                        }
                        else
                        {
                            Function_Method.ImgCompress(file, filePath);//compress image and save into E drive
                            System.Drawing.Image img = System.Drawing.Image.FromStream(currentFile.InputStream);
                            img.Save(filePath);
                            logger.Info("Image file compressed and saved: {0}", filePath);

                        }
                        MySqlParameter _C1 = new MySqlParameter("@c1", MySqlDbType.VarChar, 0);
                        _C1.Value = hdRedemID.Value;
                        cmd.Parameters.Add(_C1);

                        MySqlParameter _S1 = new MySqlParameter("@s1", MySqlDbType.VarChar, 0);
                        _S1.Value = Label_Salesman.Text;
                        cmd.Parameters.Add(_S1);

                        MySqlParameter _C2 = new MySqlParameter("@c2", MySqlDbType.VarChar, 0);
                        _C2.Value = Label_CustName.Text;
                        cmd.Parameters.Add(_C2);

                        MySqlParameter _p1 = new MySqlParameter("@p1", MySqlDbType.VarChar, 0);
                        _p1.Value = path + "/" + filename;
                        cmd.Parameters.Add(_p1);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        logger.Info("Database record inserted for: {0}", filePath);

                        conn.Close();
                        cmd.Parameters.Clear();
                        Function_Method.UserLog(hdRedemID.Value + " image uploaded.");

                    }
                    Function_Method.AddLog("Redemption Images uploaded.");
                }
            }
            catch (Exception ER_RD_05)
            {
                logger.Error(ER_RD_05, "Upload error occurred while uploading redemption images.");
                Function_Method.MsgBox("ER_RD_05: " + ER_RD_05.ToString(), this.Page, this);
                Function_Method.AddLog("Redemption Images upload error>" + ER_RD_05);
                throw;
            }
        }

        protected void BtnAmend_Click(object sender, EventArgs e)
        {
            Function_Method.isVPPPCampaign = false;
            var Email = Redemption_Get_Details.getRedempApprovalEmail();

            string UserName = Function_Method.GetLoginedUserFullName(Session["user_id"].ToString());
            if (cbInterestWaive.Checked)
            {
                UpdRedemption("RV: by " + UserName + " on " + DateTime.Now + "<br/>", 8, 0, "reverse");//docStatus Awaiting CC manager
            }
            else
            {
                UpdRedemption("RV: by " + UserName + " on " + DateTime.Now + "<br/>", 1, 0, "reverse");//docStatus Awaiting HOD
            }
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Reversed Redemption Application (" + hdStatus.Value + " " + hdRedemID.Value + ")",
                Email.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("Reversed"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }

        protected void Button_CheckOutStanding_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = TextBox_Account.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Redemption_OutBalance.aspx','_blank')", true);//_blank
        }

        protected void Button_Email_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = TextBox_Account.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('EmailApplicant.aspx','_new')", true);//_blank
        }

        protected void Button_Approve_Click(object sender, EventArgs e)
        {
            // Jerry 2024-12-09 Check for sufficient lp and ap before allow proceed
            decimal lp_point = 0, redeem_point = 0, anp_point = 0, anp_point2 = 0;

            if (!string.IsNullOrWhiteSpace(Label_Point.Text))
                lp_point = Convert.ToDecimal(Label_Point.Text);

            if (!string.IsNullOrWhiteSpace(txtPts.Text))
                redeem_point = Convert.ToDecimal(txtPts.Text);

            if (!string.IsNullOrWhiteSpace(txtAnP.Text))
                anp_point = Convert.ToDecimal(txtAnP.Text);

            if (anp_point == 0 && !string.IsNullOrWhiteSpace(cbAnP.Text))
                anp_point = Convert.ToDecimal(cbAnP.Text);

            if (lp_point < 0)
                lp_point = 0;

            if (redeem_point > lp_point && anp_point == 0)
            {
                Function_Method.MsgBox("Insufficient Loyalty Points, please use A&P Point for the balance.", this.Page, this);
                return;
            }

            if (redeem_point > (anp_point + lp_point))
            {
                Function_Method.MsgBox("Insufficient Points, unable to approve.", this.Page, this);
                return;
            }
            // Jerry 2024-12-09 Check for sufficient lp and ap before allow proceed - END
            if (!PointCalculation(lblGetRedempID.Text))
            {
                return;
            }
            // Jerry 2024-12-09 Stop reading user data from GLOBAL
            var login_user_id = Session["user_id"].ToString();

            //string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);
            string UserName = Function_Method.GetLoginedUserFullName(login_user_id);
            // Jerry 2024-12-09 Stop reading user data from GLOBAL - END

            int index = UserName.IndexOf('(');
            if (index != -1)
            {
                UserName = UserName.Substring(0, index).Trim();
            }

            Axapta DynAx = Function_Method.GlobalAxapta();//base class
            var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(txtRM.Text));

            try
            {
                if (lbl_Status.Text == "Awaiting HOD")//yongwc 29/10/24 - 1
                {
                    string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, hdsalemanID.Value);//salesapprovalgroupid
                    var hodSplit = getHod.Split('_');
                    string getReportHOD = "---";

                    if (getHod == "")
                    {
                        var tuple_GroupId_ReportingTo = EOR_GET_NewApplicant.Check_User_GroupId_ReportingTo(DynAx, hdsalemanID.Value);
                        string ReportTo = tuple_GroupId_ReportingTo.Item2;// to find GroupId

                        getReportHOD = EOR_GET_NewApplicant.get_User_Id(DynAx, ReportTo);

                    }
                    int docstat = 2;
                    // Jerry 2024-12-09 Stop reading user data from GLOBAL
                    //if (GLOBAL.user_id == hodSplit[0])//kennychuah
                    if (getHod != "")
                    {

                        if (login_user_id == hodSplit[0])//kennychuah
                                                         // Jerry 2024-12-09 Stop reading user data from GLOBAL - END
                        {
                            hdStatus.Value = "Awaiting HOD";

                            UpdRedemption("RC: by " + UserName + " on " + DateTime.Now + "<br/>", docstat, 0, "approved");//HOD1 approve > awaiting sales admin
                        }
                        // Jerry 2024-12-09 Stop reading user data from GLOBAL
                        //else if (GLOBAL.user_id == hodSplit[1])
                        else if (login_user_id == hodSplit[1])
                        // Jerry 2024-12-09 Stop reading user data from GLOBAL - END
                        {
                            hdStatus.Value = "Awaiting HOD";
                            UpdRedemption("RC: by " + UserName + " on " + DateTime.Now + "<br/>", 1, 0, "approved");//awaiting HOD2
                        }
                        // Jerry 2024-12-09 Stop reading user data from GLOBAL
                        //else if (GLOBAL.user_id == hodSplit[2])
                        else if (login_user_id == hodSplit[2])
                        // Jerry 2024-12-09 Stop reading user data from GLOBAL - END
                        {
                            hdStatus.Value = "Awaiting HOD";
                            UpdRedemption("RC: by " + UserName + " on " + DateTime.Now + "<br/>", 1, 1, "approved");//awaiting HOD3
                        }
                        else
                        {
                            if (cbInterestWaive.Checked)
                            {
                                hdStatus.Value = "Awaiting Credit Control Manager";
                                docstat = 8;
                            }
                            else
                            {
                                hdStatus.Value = "Awaiting Sales Admin";
                                docstat = 2;
                            }
                            UpdRedemption("RC: by " + UserName + " on " + DateTime.Now + "<br/>", docstat, 0, "approved");
                            switch (hdStatus.Value)
                            {
                                case "Awaiting Sales Admin":
                                    btnListSalesAdmin_Click(sender, e);
                                    break;
                                case "Awaiting Credit Control Manager":
                                    btnListCreditControl_Click(sender, e);
                                    break;
                            }
                        }
                    }
                    else if (login_user_id == getReportHOD)
                    {
                        if (cbInterestWaive.Checked)
                        {
                            hdStatus.Value = "Awaiting Credit Control Manager";
                            docstat = 8;
                        }
                        else
                        {
                            hdStatus.Value = "Awaiting Sales Admin";
                            docstat = 2;
                        }
                        UpdRedemption("RC: by " + UserName + " on " + DateTime.Now + "<br/>", docstat, 0, "approved");
                        switch (hdStatus.Value)
                        {
                            case "Awaiting Sales Admin":
                                btnListSalesAdmin_Click(sender, e);
                                break;
                            case "Awaiting Credit Control Manager":
                                btnListCreditControl_Click(sender, e);
                                break;
                        }

                    }

                }
                else if (lbl_Status.Text == "Awaiting Sales Admin")
                {
                    hdStatus.Value = "Awaiting Sales Admin Manager";
                    UpdRedemption("AC: by " + UserName + " on " + DateTime.Now + "<br/>", 3, 0, hdStatus.Value);//awaiting sales admin manager
                }
                else if (lbl_Status.Text == "Awaiting Sales Admin Manager")
                {
                    if (cbSpecialApproval.Checked || Convert.ToDouble(txtRM.Text) >= 5000)
                    {
                        hdStatus.Value = "Awaiting General Manager";
                        UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 4, 0, hdStatus.Value);//awaiting gm
                    }
                    else if (amount.OManager1 != "" || amount.OManager2 != "" || amount.OManager3 != "")
                    {
                        hdStatus.Value = "Awaiting Operation Manager";
                        UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 7, 0, hdStatus.Value);//awaiting operation manager
                    }
                    else// if redemption approval is empty, approve directly
                    {
                        hdStatus.Value = "Approved";
                        UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 5, 0, hdStatus.Value);//approve
                    }
                }
                else if (lbl_Status.Text == "Awaiting Operation Manager")
                {
                    if (cbGMApproval.Checked)
                    {
                        hdStatus.Value = "Awaiting General Manager";
                        UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 4, 0, hdStatus.Value);
                    }
                    else
                    {
                        hdStatus.Value = "Approved";
                        UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 5, 0, hdStatus.Value);
                    }
                }
                else if (lbl_Status.Text == "Awaiting Credit Control Manager")
                {
                    hdStatus.Value = "Awaiting Sales Admin";
                    UpdRedemption("RC: by " + UserName + " on " + DateTime.Now + "<br/>", 2, 0, hdStatus.Value);
                }
                else// awaiting gm
                {
                    hdStatus.Value = "Approved";
                    UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 5, 0, hdStatus.Value);
                }
            }
            catch (Exception rd_update)
            {
                Function_Method.MsgBox("User name not in approval list.\n" + rd_update, this.Page, this);
                throw;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        /*
        protected void Button_CreateVoucher_Click(object sender, EventArgs e)
        {
            var Email = Redemption_Get_Details.getRedempApprovalEmail();
            string UserName = Function_Method.GetLoginedUserFullName(Email.Item1);

            UpdRedemption("VRV: by " + " on " + DateTime.Now, 0, 0, "created voucher");
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Completed Redemption Application (Awaiting VRV Journal " + hdStatus.Value + " " + hdRedemID.Value + ")",
                Email.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("approved"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }
        */
        protected void Button_CreateVoucher_Click(object sender, EventArgs e)
        {
            //2025-03-21 KX Get salesman email and send ---------------------------------------
            Function_Method.isVPPPCampaign = false;

            var tuple_SalesManEmail = GetSalesManHODEmail();
            //-------------------------------------------------------------------------

            var Email = Redemption_Get_Details.getRedempApprovalEmail();
            string UserName = Function_Method.GetLoginedUserFullName(Email.Item1);

            UpdRedemption("VRV: by " + " on " + DateTime.Now, 0, 0, "created voucher");
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Completed Redemption Application (Awaiting VRV Journal " + hdStatus.Value + " " + hdRedemID.Value + ")",
                tuple_SalesManEmail.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6 + "," + tuple_SalesManEmail.Item2, RedempEmailContent("Approved"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }
        /*
        protected void Button_CreateVoucher_Click(object sender, EventArgs e)
        {
            //2025-03-21 KX Get salesman email and send ---------------------------------------
            Axapta DynAx = Function_Method.GlobalAxapta();
            var salesman = Label_Salesman.Text.Split('(', ')');
            int tableId = DynAx.GetTableId("EmplTable");
            string CustAcc = TextBox_Account.Text.Trim();
            string Receiver = "";

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId);
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//CustAccount
            qbr1.Call("value", salesman[1].ToString());
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                Receiver = DynRec1.get_Field("Del_Email").ToString();

                DynRec1.Dispose();
            }
            //-------------------------------------------------------------------------

            var Email = Redemption_Get_Details.getRedempApprovalEmail();
            string UserName = Function_Method.GetLoginedUserFullName(Email.Item1);

            UpdRedemption("VRV: by " + " on " + DateTime.Now, 0, 0, "created voucher");
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Completed Redemption Application (Awaiting VRV Journal " + hdStatus.Value + " " + hdRedemID.Value + ")",
                Receiver, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("Approved"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }*/


        protected void BtnUpdateDoc_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            Function_Method.isVPPPCampaign = false;

            var Email = Redemption_Get_Details.getRedempApprovalEmail();
            string UserName = Function_Method.GetLoginedUserFullName(Email.Item1);

            Function_Method.AddLog("Redemption uploading images");
            logger.Info($"User  {GLOBAL.user_id} is attempting to upload images.");

            // Jerry 2024-12-18 update pic as long as fileUpload is not empty
            Function_Method.UserLog(GLOBAL.user_id + " check if fileUpload not empty.");
            if (Request.Files != null && Request.Files.Count > 0)
            {
                // Check for the specific files by their names
                HttpPostedFile fileUpload = Request.Files["fileUpload"];

                // Check if any of the files are not null and have content
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    // At least one file is uploaded and has content
                    Function_Method.UserLog(GLOBAL.user_id + " uploading image");
                    UploadPic();
                    Function_Method.UserLog(GLOBAL.user_id + " upload image done.");
                    logger.Info($"User  {GLOBAL.user_id} uploaded an image successfully.");
                }
                else
                {
                    logger.Warn($"User  {GLOBAL.user_id} attempted to upload an empty file.");
                }
            }
            //if (btnDisplay.Visible != false)
            //{
            //    UploadPic();
            //}
            // Jerry 2024-12-18 update pic as long as fileUpload is not empty - END

            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebRedemp"))
            {
                DynAx.TTSBegin();
                DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Rdemt_ID", hdRedemID.Value));
                if (DynRec.Found)
                {
                    var getStat = Redemption_Get_Details.get_redempStat(DynAx, lblGetRedempID.Text);

                    // Jerry 2024-12-10 Get docstatus for redemption
                    int docstatus = Redemption_Get_Details.get_docstatus(DynAx, lblGetRedempID.Text);
                    // Jerry 2024-12-10 Get docstatus for redemption - END

                    // Jerry 2024-12-10 Save with the correct docstatus
                    //Save_LF_WebRedemp(DynRec, 0, getStat.Item1 + "DU: by " + Session["user_id"].ToString() + " on " + DateTime.Now + "<br/>");
                    Save_LF_WebRedemp(DynRec, docstatus, getStat.Item1 + "DU: by " + Session["user_id"].ToString() + " on " + DateTime.Now + "<br/>", true);
                    // Jerry 2024-12-10 Save with the correct docstatus - END
                    logger.Info($"User  {GLOBAL.user_id} updated redemption application with ID: {hdRedemID.Value} and status: {getStat.Item1}");

                    DynRec.Call("update");
                    DynAx.TTSCommit();

                    hdRedemID.Value = DynRec.get_Field("Rdemt_ID").ToString();
                    logger.Info($"Redemption ID updated to: {hdRedemID.Value}");
                    DynAx.TTSAbort();

                    hdStatus.Value = lbl_Status.Text;
                    Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Updated Redemption Application (" + hdStatus.Value + " " + hdRedemID.Value + ")",
                        Email.Item1, Email.Item2 + "," + Email.Item3, RedempEmailContent("Submitted"));//email to hod
                    logger.Info($"Email sent to HOD regarding updated redemption application: {hdRedemID.Value}");
                    Reloading_data(DynAx, hdRedemID.Value);
                }
                DynAx.Dispose();
            }
        }

        protected void Button_Hold_Click(object sender, EventArgs e)
        {
            var Email = Redemption_Get_Details.getRedempApprovalEmail();
            string UserName = Function_Method.GetLoginedUserFullName(Email.Item1);
            Function_Method.isVPPPCampaign = false;

            EnableItemPoint();
            UpdRedemption("Reviewed Hold: by " + UserName + " on " + DateTime.Now, 0, 0, "Hold");//update status to approved
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Completed Redemption Application", Email.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("hold"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }

        protected void Button_AdminApprove_Click(object sender, EventArgs e)
        {
            if (hdSubmitReason.Value != "")
            {
                double validateSpecialApproval = double.Parse(lblLoyaltyPoint.Text) - double.Parse(lblBalanceAP.Text) - double.Parse(txtRM.Text);
                if (validateSpecialApproval < 0)
                {
                    Function_Method.MsgBox("Total point must be positive to proceed.", this.Page, this);
                    return;
                }
                else
                {
                    var Email = Redemption_Get_Details.getRedempApprovalEmail();
                    string UserName = Function_Method.GetLoginedUserFullName(Email.Item1);

                    UpdRedemption("AP: by " + UserName + " on " + DateTime.Now + "<br/>", 5, 0, "approve");//update status to approved

                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
                }
            }
        }

        protected void btnGetInvoice1_Click(object sender, EventArgs e)
        {
            if (btnGetInvoice1.Text != "")
            {
                getInvoiceData(btnGetInvoice1.Text);
            }
        }

        protected void btnGetInvoice2_Click(object sender, EventArgs e)
        {
            if (btnGetInvoice2.Text != "")
            {
                getInvoiceData(btnGetInvoice2.Text);
            }
        }

        protected void btnGetInvoice3_Click(object sender, EventArgs e)
        {
            if (btnGetInvoice3.Text != "")
            {
                getInvoiceData(btnGetInvoice3.Text);
            }
        }

        protected string RedempEmailContent(string status)
        {
            var salesman = Label_Salesman.Text.Split('(', ')');
            string removeAwaiting = status;

            if (status.Contains("Awaiting Sales Admin Manager") || status.Contains("Awaiting General Manager"))
            {
                removeAwaiting = "This redemption have been Approved.";
            }
            else
            {
                //removeAwaiting = "The next approval is" + status.Replace("Awaiting", "");
                removeAwaiting = "This redemption have been " + status.Replace("Awaiting", "");
            }

            string emailContent = "Please be informed that the following Redemption Application details:" +
                                  "\r\n\r\nApplicant Name: " + salesman[2] +
                                  "\r\nCustomer: " + Label_CustName.Text +
                                  "\r\nAmount: " + txtRM.Text +
                                  "\r\nPoints: " + txtPts.Text +
                                  "\r\nRedemption Form No.: " + lblGetRedempID.Text +
                                  "\r\n\r\n" + removeAwaiting + "." +
                                  "\r\n\r\n\r\nThank You.";
            return emailContent;
        }

        protected void getInvoiceData(string invoiceID)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            var getDataPassing = Redemption_Get_Details.getInvoice(DynAx, TextBox_Account.Text, invoiceID);
            Session["data_passing"] = "_PAIN@" + getDataPassing.Item1 + "|" + TextBox_Account.Text + "|" + getDataPassing.Item2 + "|" +
                                                Convert.ToDateTime(getDataPassing.Item3).ToString("dd/MM/yyyy") + "|" + Convert.ToDateTime(getDataPassing.Item4).ToString("dd/MM/yyyy");

            string strUserAgent = Request.UserAgent.ToString().ToLower();
            if (strUserAgent != null)
            {
                if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("mobile"))
                {
                    if (strUserAgent.Contains("iphone"))//default iphone block pop out
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_self')", true);//_blank
                    }
                    else if (strUserAgent.Contains("blackberry") || strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") || strUserAgent.Contains("palm"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
                    }
                }
                else// not mobile
                {//Function_Method.MsgBox("2  " + strUserAgent, this.Page, this);
                    if (strUserAgent.Contains("ipad"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_self')", true);//_blank
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Invoice.aspx','_newtab')", true);//_blank
            }
            DynAx.Dispose();
        }

        /*
        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);
            var Email = Redemption_Get_Details.getRedempApprovalEmail();

            hdStatus.Value = "Rejected";
            UpdRedemption("RJ: by " + UserName + " on " + DateTime.Now + "<br/>", 6, 0, "rejected");//reject
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Rejected Redemption Application", Email.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("Reject"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }
        */

        /*
        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            //2025-03-21 KX Get salesman email and send ---------------------------------------
            Axapta DynAx = Function_Method.GlobalAxapta();
            var salesman = Label_Salesman.Text.Split('(', ')');
            int tableId = DynAx.GetTableId("EmplTable");
            string CustAcc = TextBox_Account.Text.Trim();
            string Receiver = "";

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId);
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//CustAccount
            qbr1.Call("value", salesman[1].ToString());
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                Receiver = DynRec1.get_Field("Del_Email").ToString();

                DynRec1.Dispose();
            }
            //------------------------------------------------------------------------------------
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);
            var Email = Redemption_Get_Details.getRedempApprovalEmail();

            hdStatus.Value = "Rejected";
            UpdRedemption("RJ: by " + UserName + " on " + DateTime.Now + "<br/>", 6, 0, "rejected");//reject
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Rejected Redemption Application", Receiver, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("Rejected"));
            //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }
        */

        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            //2025-03-21 KX Get salesman email and send ---------------------------------------
            var tuple_SalesManEmail = GetSalesManHODEmail();
            //================================================================================
            Function_Method.isVPPPCampaign = false;
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);
            var Email = Redemption_Get_Details.getRedempApprovalEmail();

            hdStatus.Value = "Rejected";
            UpdRedemption("RJ: by " + UserName + " on " + DateTime.Now + "<br/>", 6, 0, "rejected");//reject
            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Rejected Redemption Application", tuple_SalesManEmail.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6 + "," + tuple_SalesManEmail.Item2/* + Email.Item1*/, RedempEmailContent("Rejected"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section
        }

        protected Tuple<string, string> GetSalesManHODEmail()
        {
            //2025-03-21 KX Get salesman email and send ---------------------------------------
            Axapta DynAx = Function_Method.GlobalAxapta();
            var salesman = Label_Salesman.Text.Split('(', ')');
            int tableId = DynAx.GetTableId("EmplTable");
            string CustAcc = TextBox_Account.Text.Trim();
            string Receiver = "";
            string HodID = "";
            string Hod = "";

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", tableId);
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//CustAccount
            qbr1.Call("value", salesman[1].ToString());
            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", tableId);
                Receiver = DynRec1.get_Field("LF_EmpEmailID").ToString();
                HodID = DynRec1.get_Field("ReportTo").ToString();
                DynRec1.Dispose();
            }

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", tableId);
            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 1);//CustAccount
            qbr2.Call("value", HodID);
            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);

            if ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun2.Call("Get", tableId);
                Hod = DynRec1.get_Field("LF_EmpEmailID").ToString();
                DynRec1.Dispose();
            }

            return new Tuple<string, string>(Receiver, Hod);
        }

        protected void ddlRedempType_TextChanged(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            if (ddlRedempType.SelectedValue == "1")//purchase order
            {
                var getVendor = Redemption_Get_Details.GetVendDetails(DynAx, TextBox_Account.Text);

                vendor_section.Visible = true;
                if (getVendor.Item1.ToString() != "")
                {
                    lblGetVendor.Text = getVendor.Item1.ToString();
                    hdVendorGrp.Value = getVendor.Item3.ToString();
                }
                else
                {
                    Function_Method.MsgBox("Vendor account not found.", this.Page, this);
                }
            }
            DynAx.Dispose();
        }

        protected void cbAnP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAnP.Checked)
            {
                txtAnP.Visible = true;
            }
        }

        protected void Button_Print_Click(object sender, EventArgs e)
        {
            Session["data_passing"] = lblGetRedempID.Text + "|" + TextBox_Account.Text;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('PrintingLayout.aspx','_newtab')", true);
        }

        protected void BtnRedemptionId_Click(object sender, EventArgs e)
        {
            try
            {
                string selected_Redemption = "";
                Button Button_SalesId = sender as Button;
                GridViewRow row = (GridViewRow)Button_SalesId.NamingContainer;
                int index = row.RowIndex;

                if (Button_SalesId != null)
                {
                    selected_Redemption = Button_SalesId.Text;
                    hdStatus.Value = Gv_Overview.Rows[index].Cells[7].Text;

                    Session["data_passing"] = "@RDRD_" + selected_Redemption + "_" + hdStatus.Value;
                    //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview section

                    Response.Redirect("Redemption.aspx", false);

                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Function_Method.UserLog(ex.ToString());

                throw;
            }
        }

        protected void Gv_Overview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Redemption")
            {
                string id = e.CommandArgument.ToString();
                Button Button_RdId = e.CommandSource as Button;
                if (Button_RdId != null)
                {
                    GridViewRow row = (GridViewRow)Button_RdId.NamingContainer;
                    int index = row.RowIndex;

                    hdStatus.Value = Gv_Overview.Rows[index].Cells[7].Text;
                    string HodApprover = Gv_Overview.Rows[index].Cells[6].Text;
                    Session["data_passing"] = "@RDRD_" + id + "_" + hdStatus.Value + "_" + HodApprover;

                    Response.Redirect("Redemption.aspx", false);
                }
            }
            else
            {
                string id = e.CommandArgument.ToString();
                Button Button_RdId = e.CommandSource as Button;
                if (Button_RdId != null)
                {
                    GridViewRow row = (GridViewRow)Button_RdId.NamingContainer;
                    int index = row.RowIndex;

                    hdStatus.Value = Gv_OverviewSearch.Rows[index].Cells[7].Text;
                    string HodApprover = Gv_Overview.Rows[index].Cells[6].Text;
                    Session["data_passing"] = "@RDRD_" + id + "_" + hdStatus.Value + "_" + HodApprover;

                    Response.Redirect("Redemption.aspx", false);
                }
            }
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSort.SelectedValue == "0")
            {
                Session["flag_temp"] = 0;
            }
            else if (ddlSort.SelectedValue == "1")
            {
                Session["flag_temp"] = 1;
            }
            else if (ddlSort.SelectedValue == "2")
            {
                Session["flag_temp"] = 2;
            }
            else if (ddlSort.SelectedValue == "3")
            {
                Session["flag_temp"] = 3;
            }
            else if (ddlSort.SelectedValue == "4")
            {
                Session["flag_temp"] = 4;
            }
            else if (ddlSort.SelectedValue == "5")
            {
                Session["flag_temp"] = 5;
            }
            else if (ddlSort.SelectedValue == "6")
            {
                Session["flag_temp"] = 6;
            }
            else if (ddlSort.SelectedValue == "7")
            {
                Session["flag_temp"] = 7;
            }
            else
            {
                Session["flag_temp"] = 8;
            }

            redemption_Overview(0);
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                Gv_Overview.Visible = false;
                Gv_OverviewSearch.Visible = true;
                redemption_OverviewTxtSearch(0, txtSearch.Text);
            }
            else
            {
                Gv_Overview.Visible = true;
                Gv_OverviewSearch.Visible = false;
                redemption_Overview(0);
            }
        }

        // Desmond 2024-12-23 Add Search by HOD for Sales Admin only
        protected void btnSearchHOD_Click(object sender, EventArgs e)
        {

            if (txtSearchHOD.Text != "")
            {
                // Jerry 2025-01-24 Test
                //Gv_Overview.Visible = false;
                //Gv_OverviewSearch.Visible = true;
                Gv_Overview.Visible = false;
                Gv_OverviewSearch.Visible = true;
                redemption_OverviewTxtSearchHOD(0, txtSearchHOD.Text);
            }
            else
            {
                Gv_Overview.Visible = true;
                Gv_OverviewSearch.Visible = false;
                redemption_Overview(0);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "ResetButtonText",
        "document.getElementById('" + btnSearchHOD.ClientID + "').value='Search';" +
        "document.getElementById('loading').style.display='none';", true);
        }
        // Desmond 2024-12-23 Add Search by HOD for Sales Admin only - END

        protected void Button_Redemp_Enquiries_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            Button_Redemption_section.Attributes.Add("style", "background-color:transparent");
            Button_Redemp_Overview.Attributes.Add("style", "background-color:transparent");
            Button_Signboard_section.Attributes.Add("style", "background-color:transparent");
            new_applicant_section.Attributes.Add("style", "display:none");
            div_Overview.Attributes.Add("style", "display:initial");
            Button_Redemp_Enquiries.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            dvEnquiry.Visible = true;
            dvOverView.Visible = false;

            string[] arr_NA_HOD = new string[10];

            // Desmond 2024-12-23 Get HOD method
            string UserName = getHODUserName(DynAx);

            if (!string.IsNullOrEmpty(UserName))
            {
                //UserName = getHODUserName(DynAx);
                divForApproval.Visible = true;

                btnListHOD_Click(sender, e);
            }
            /*
            string userid = Session["user_id"].ToString();
            var getHodId = Quotation_Get_Sales_Quotation.get_Empl_Id(DynAx, Session["user_id"].ToString() + "@posim.com.my");//salesapprovalgroupid
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);

            if (!string.IsNullOrEmpty(getHodId.Item1.ToString()))
            {
                Session["hod"] = UserName;
                divForApproval.Visible = true;

                btnListHOD_Click(sender, e);
            }
            */
            // Desmond 2024-12-23 Get HOD method - END

            string getApprover = Redemption_Get_Details.getRedempApprovalAccess(DynAx, UserName);
            if (getApprover != "")
            {
                divForApproval.Visible = true;
                if (getApprover == "SalesAdmin1" || getApprover == "SalesAdmin2" || getApprover == "SalesAdmin3")
                {
                    btnListSalesAdmin_Click(sender, e);
                }
                else if (getApprover == "SalesAdminManager1" || getApprover == "SalesAdminManager2" || getApprover == "SalesAdminManager3")
                {
                    btnListSAManager_Click(sender, e);
                }
                else if (getApprover == "CreditControlManager1" || getApprover == "CreditControlManager2" || getApprover == "CreditControlManager")
                {
                    btnListCreditControl_Click(sender, e);
                }
                else if (getApprover == "OperationManager1" || getApprover == "OperationManager2" || getApprover == "OperationManager3")
                {
                    btnListOperation_Click(sender, e);
                }
                else
                {
                    btnListGM_Click(sender, e);
                }
            }
            divForSalesman.Visible = true;

            DynAx.Dispose();
        }

        protected void btnListHOD_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:#f58345");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["flag_temp"] = 1;
            // Desmond 2024-12-23 Get HOD method
            Axapta DynAx = Function_Method.GlobalAxapta();
            getHODUserName(DynAx);
            DynAx.Dispose();
            // Desmond 2024-12-23 Get HOD method - END
            redemption_Overview(0);
        }

        protected void btnListSalesAdmin_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:#f58345");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["hod"] = "";
            Session["flag_temp"] = 2;
            redemption_Overview(0);
        }

        protected void btnListSAManager_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:#f58345");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["hod"] = "";
            Session["flag_temp"] = 3;
            redemption_Overview(0);
        }

        protected void btnListCreditControl_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:#f58345");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["hod"] = "";
            Session["flag_temp"] = 8;
            redemption_Overview(0);
        }

        protected void btnListOperation_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:#f58345");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["hod"] = "";
            Session["flag_temp"] = 7;
            redemption_Overview(0);
        }

        protected void btnListGM_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:#f58345");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["hod"] = "";
            Session["flag_temp"] = 4;
            redemption_Overview(0);
        }

        protected void btnListApproved_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:#f58345");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            Session["hod"] = "";
            Session["flag_temp"] = 5;
            redemption_Overview(0);
        }

        protected void btnListReject_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:#f58345");
            btnListAll.Attributes.Add("style", "background-color:transparent");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["hod"] = "";
            Session["flag_temp"] = 6;
            redemption_Overview(0);
        }

        protected void btnListAll_Click(object sender, EventArgs e)
        {
            btnListHOD.Attributes.Add("style", "background-color:transparent");
            btnListSalesAdmin.Attributes.Add("style", "background-color:transparent");
            btnListSAManager.Attributes.Add("style", "background-color:transparent");
            btnListCreditControl.Attributes.Add("style", "background-color:transparent");
            btnListOperation.Attributes.Add("style", "background-color:transparent");
            btnListGM.Attributes.Add("style", "background-color:transparent");
            btnListApproved.Attributes.Add("style", "background-color:transparent");
            btnListReject.Attributes.Add("style", "background-color:transparent");
            btnListAll.Attributes.Add("style", "background-color:#f58345");

            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD
            lblSearchHOD.Visible = false;
            txtSearchHOD.Visible = false;
            btnSearchHOD.Visible = false;
            //Desmond 2024-12-23 Hide txtSearchHOD and btnSearchHOD -END

            Session["flag_temp"] = 0;
            Session["hod"] = "";
            /*
            // Desmond 2024-12-23 Get HOD method
            //Session["hod"] = "";
            Axapta DynAx = Function_Method.GlobalAxapta();
            getHODUserName(DynAx);
            DynAx.Dispose();
            */
            // Desmond 2024-12-23 Get HOD method - END

            redemption_Overview(0);
        }

        protected void BtnReverse_Click(object sender, EventArgs e)
        {
            Function_Method.isVPPPCampaign = false;

            var Email = Redemption_Get_Details.getRedempApprovalEmail();

            string UserName = Function_Method.GetLoginedUserFullName(Session["user_id"].ToString());

            UpdRedemption("RV: by " + UserName + " on " + DateTime.Now + "<br/>", 2, 0, "reverse");//docStatus Awaiting sales admin

            Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Reversed Redemption Application (" + hdStatus.Value + " " + hdRedemID.Value + ")",
                Email.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent("Reversed"));
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview_section

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

        protected void Button_Alt_Delivery_Addr_Click(object sender, EventArgs e)
        {
            GridView_AltAddress.Visible = true;

            Button_Delivery_Addr.Text = "Hide Alt. Addr.";

            if (TextBox_Account.Text == "")
            {
                Function_Method.MsgBox("There is no customer account number.", this.Page, this);
                Button_Delivery_Addr.Text = "Alt. Addr.";
                return;
            }

            //Add to Grid, GridView_AltAddress
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Alternate Address") });

            int Counter = Convert.ToInt32(hidden_alt_address_counter.Text);

            string[] arr_alt_address = hidden_alt_address.Text.Split('|');
            for (int i = 0; i < Counter; i++)
            {
                dt.Rows.Add(arr_alt_address[i]);
            }

            GridView_AltAddress.DataSource = dt;
            GridView_AltAddress.DataBind();
        }

        public void Alt_Addr_function()
        {
            Button_Delivery_Addr.Visible = false;
            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";

            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                var tuple_get_AltAddress = SFA_GET_SALES_HEADER.get_AltAddress(DynAx, TextBox_Account.Text);
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
                    if (AltAddress[0] == txtDeliveryAddr.Text)//same as primary, no alt address
                    {
                        return;
                    }
                    Button_Delivery_Addr.Visible = false;
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
                Button_Delivery_Addr.Visible = true;
            }
            catch (Exception ER_RD_12)
            {
                Function_Method.MsgBox("ER_RD_12: " + ER_RD_12.ToString(), this.Page, this);
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

                        txtDeliveryAddr.Text = selected_address;

                        //need to update hidden field

                        string temp_RecID_List = hidden_alt_address_RecId.Text;
                        string[] arr_temp_RecID_List = temp_RecID_List.Split('|');
                        var tuple_get_AltAddress_info = SFA_GET_SALES_HEADER.get_AltAddress_info(DynAx, arr_temp_RecID_List[counter]);

                        DynAx.Logoff();

                        GridView_AltAddress.Visible = false;
                        Button_Delivery_Addr.Text = "Alt. Addr.";
                        //row.BackColor = Color.FromName("#ff8000");
                    }
                }
                counter = counter + 1;
            }
        }

        // Desmond 2024-12-23 Get HOD method
        protected string getHODUserName(Axapta DynAx)
        {
            string userid = Session["user_id"].ToString();
            var getHodId = Quotation_Get_Sales_Quotation.get_Empl_Id(DynAx, Session["user_id"].ToString() + "@posim.com.my");//salesapprovalgroupid
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);

            if (!string.IsNullOrEmpty(getHodId.Item1.ToString()))
            {
                Session["hod"] = UserName;
                return UserName;
            }
            return "";
        }
        // Desmond 2024-12-23 Get HOD method - END

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Neil Fine-tuning
        
        private void GetApValues(string redemption_Id, out double preposted_ap, out double deducted_ap)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, TextBox_Account.Text);
            var getAdminRemarks = Redemption_Get_Details.get_gridViewDataAdmin(DynAx, redemption_Id);
            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("PointBalance"))
            {
                DynAx.TTSBegin();
                DynRec.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", "AccountNum", TextBox_Account.Text));
                var APCF = DynRec.Call("getAddBalance");
                preposted_ap = Convert.ToDouble(APCF);
                deducted_ap = !string.IsNullOrEmpty(getAdminRemarks.Item1?.ToString())
                                        ? Convert.ToDouble(getAdminRemarks.Item1.ToString())
                                        : 0;
            }
        }

        #endregion
    }
}