using ActiveDs;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using static DotNet.Redemption_Get_Details;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DotNet
{
    public partial class Redemption : System.Web.UI.Page
    {
        private void clear_parameter_NewApplication_CustomerInfo()
        {
            // <!--Customer Info ////////////////////////////////////////////////////////////////////////////////////-->
            TextBox_Account.Text = "";
            Label_HQ.Text = "";
            Label_Salesman.Text = "";
            Label_CustName.Text = "";
            Textbox_TelNo.Text = "";
            Textbox_ContactPerson.Text = "";
            Label_Class.Text = "";
        }

        //CustInfo //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void CheckAcc(object sender, EventArgs e)
        {
            validate();
        }

        private void validate()
        {
            string CustAcc = TextBox_Account.Text.Trim();
            clear_parameter_NewApplication_CustomerInfo();
            TextBox_Account.Text = CustAcc;//after clear all, rewrite back

            if (CustAcc == "")
            {
                Function_Method.MsgBox("There is no account number", this.Page, this);
                return;
            }
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string getWarning = "";
                string emplID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, CustAcc);
                string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, emplID);//salesapprovalgroupid
                var splitHod = getHod.Split('_');
                var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                var tuple_getGracePeriod = SFA_GET_SALES_HEADER.get_SuffixCode(DynAx, tuple_getCustInfo.Item6.ToString());
                string CustName = tuple_getCustInfo.Item1;
                string temp_EmplId = tuple_getCustInfo.Item3;
                string temp_EmpName = tuple_getCustInfo.Item4;
                string BranchID = tuple_getCustInfo.Item5;//dimension
                string CustomerClass = tuple_getCustInfo.Item6.Trim();

                if (splitHod[0] != "" && hdRedemID.Value == "")
                {
                    if (Session["user_id"].ToString() == splitHod[0] || splitHod.Length > 1 && Session["user_id"].ToString() == splitHod[1] ||
                        splitHod.Length > 2 && Session["user_id"].ToString() == splitHod[2])//HOD, 6months, 180days
                    {
                        // Jerry 2024-12-04 Allow HOD to create redemption for customer with up to 365 days of inactivities
                        // getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, CustAcc, tuple_getGracePeriod.Item3);
                        getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, CustAcc, tuple_getGracePeriod.Item3, true);
                        // Jerry 2024-12-04 End

                        if (getWarning != "")
                        {
                            Function_Method.MsgBox(getWarning, this.Page, this);
                            TextBox_Account.Text = "";
                        }
                    }
                    else if (Session["user_id"].ToString() != splitHod[0] || Session["user_id"].ToString() != splitHod[1] ||
                       Session["user_id"].ToString() != splitHod[2])//salesman can create new redemption within 3months from last active invoice, 90days
                    {
                        // Jerry 2024-12-04 Allow HOD to create redemption for customer with up to 365 days of inactivities
                        // getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, CustAcc, tuple_getGracePeriod.Item2);
                        getWarning = Redemption_Get_Details.getCustOutstanding(DynAx, CustAcc, tuple_getGracePeriod.Item2, false);
                        // Jerry 2024-12-04 End

                        if (getWarning != "")
                        {
                            Function_Method.MsgBox(getWarning, this.Page, this);
                            TextBox_Account.Text = "";
                        }
                    }
                }
                else if (getHod == "") //Neil - Redemption requires salesman must have HOD approval group before proceed.
                {
                    Function_Method.MsgBox("The salesman " + temp_EmpName + " (" + temp_EmplId + ") does not found any HOD level by his following Approval Group. Please contact IT to verify this employees.", this.Page, this);
                    TextBox_Account.Text = "";
                }

                if (CustName == "")
                {
                    Function_Method.MsgBox("No data found", this.Page, this);
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Overview'); ", true);//go to overview section
                }
                else
                {
                    string ClassDesc = EOR_GET_NewApplicant.getClassDetails(DynAx, CustomerClass);
                    var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, CustAcc);
                    Label_HQ.Text = BranchID;
                    Label_Salesman.Text = "(" + temp_EmplId + ") " + temp_EmpName;
                    Label_CustName.Text = CustName;
                    Label_Class.Text = ClassDesc;
                    var getPoint = EOR_GET_NewApplicant.getEORClassSetup(DynAx, CustomerClass);
                    hdpoint.Value = getPoint.Item2;
                    hidden_customer_class.Text = CustomerClass;
                    Label_Point.Text = get_Point.Item1.ToString("#,###,###,##0.00");
                    txtDeliveryAddr.Text = tuple_getCustInfo.Item2;
                    lblBeneficiaryName.Text = CustName;

                    var tuple_getCustInfo_2 = EOR_GET_NewApplicant.getCustInfo_2(DynAx, CustAcc);
                    //string CustomerContactId = tuple_getCustInfo_2.Item1;
                    string CustTelNo = tuple_getCustInfo_2.Item2;
                    string ContactName = EOR_GET_NewApplicant.getContactPersonName(DynAx, CustAcc);
                    Textbox_ContactPerson.Text = ContactName;
                    if (Textbox_ContactPerson.Text == "") Textbox_ContactPerson.Text = CustName;

                    Textbox_TelNo.Text = CustTelNo;
                }
            }
            catch (Exception ER_RD_00)
            {
                Function_Method.MsgBox("ER_RD_00: " + ER_RD_00.ToString(), this.Page, this);
            }
        }

        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_RDCM@";//REDEMPTION > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        private void initialize_GridViewItemPoint()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Items", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));
            dt.Columns.Add(new DataColumn("Points", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));

            for (int i = 0; i < 3; i++)
            {
                dr = dt.NewRow();
                dr["No."] = i + 1;
                dr["Items"] = string.Empty;
                dr["Quantity"] = string.Empty;
                dr["UnitPrice"] = string.Empty;
                dr["Total"] = string.Empty;
                dr["Points"] = string.Empty;
                dr["ItemCode"] = string.Empty;
                dr["InvoiceNo"] = string.Empty;
                dr["InvoiceDate"] = string.Empty;
                dt.Rows.Add(dr);
            }
            ViewState["CurrentTable"] = dt;
            gvItemPoint.DataSource = dt;
            gvItemPoint.DataBind();
        }

        private bool ApproverUpdate(AxaptaRecord DynRec, int docStat, string ProcessStat, string hodLvl, string Status)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            string inventDimId = "";
            string recid = "";
            string LedgerNum = "";
            string PurchId = "";
            var salesID = "";

            try
            {
                var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(txtRM.Text));

                if (docStat != 0)
                {
                    DynRec.set_Field("DocStatus", docStat);
                    //0 = Reverse //1 = HOD, 2 = Sales Admin, 3 = sales admin mgr, 4 = gm, 5 = approve, 6 = reject, 7 = operation mgr, 8 = credit control mgr
                }

                if (docStat == 1)
                {
                    string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, hdsalemanID.Value);//salesapprovalgroupid
                    var hodSplit = getHod.Split('_');
                    string UserName = "";
                    if (hodLvl == "0")
                    {
                        //UserName = Function_Method.GetLoginedUserFullName(hodSplit[0]);
                        string RevertedHOD = hodSplit[0];

                        if (Status.ToLower().Trim() == "reverse")
                        {
                            RevertedHOD = (!string.IsNullOrEmpty(hodSplit[1]) ? hodSplit[1] : hodSplit[0]);
                        }

                        UserName = Function_Method.GetLoginedUserFullName(RevertedHOD);

                        DynRec.set_Field("NextApprover", UserName);

                        if (docStat != 0)//save & update record
                        {
                            var getUser = Redemption_Get_Details.RedempApprovalInUse(DynAx);//ONLY SELECTED user can go throu

                            if (cbInterestWaive.Checked)
                            {
                                DynRec.set_Field("InterestWaiver", 1);
                            }
                            else
                            {
                                DynRec.set_Field("InterestWaiver", 0);
                            }
                        }

                    }
                    else if (hodLvl == "1")
                    {
                        UserName = Function_Method.GetLoginedUserFullName(hodSplit[1]);

                        DynRec.set_Field("NextApprover", UserName);

                        var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, TextBox_Account.Text);

                        using (AxaptaRecord DynRec1 = DynAx.CreateAxaptaRecord("PointBalance"))
                        {
                            DynAx.TTSBegin();
                            DynRec1.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", "AccountNum", TextBox_Account.Text));

                            var LPCF = DynRec1.get_Field("TPBalance");
                            var APCF = DynRec1.Call("getAddBalance");
                            var LPBal = DynRec1.Call("getTotBalance", get_Point.Item2.ToString());
                            var APBal = DynRec1.Call("getTotAddBalance", get_Point.Item2.ToString());

                            double lp = Convert.ToDouble(LPCF);
                            double ap = Convert.ToDouble(APCF);

                            DynRec.set_Field("CreatedLP", lp);
                            DynRec.set_Field("CreatedAP", ap);
                        }
                    }
                    else if (docStat == 2)
                    {
                        DynRec.set_Field("NextApprover", amount.SalesAdmin1);
                        DynRec.set_Field("SO_InvDate2", DateTime.Now.ToString());
                        DynRec.set_Field("SO_InvDate1", DateTime.Now.ToString("dd/MM/yyyy"));
                        DynRec.set_Field("SO_Inv1", lblBalanceAP.Text);
                        DynRec.set_Field("TP_DocNo", lblLoyaltyPoint.Text + "(" + DateTime.Now + ")");

                    }
                    else/* if (hodSplit.Length >= 3 && GLOBAL.user_id == hodSplit[2])*/
                    {
                        UserName = Function_Method.GetLoginedUserFullName(hodSplit[2]);

                        DynRec.set_Field("NextApprover", UserName);
                    }
                }
                else if (docStat == 2)
                {
                    DynRec.set_Field("NextApprover", amount.SalesAdmin1);
                    DynRec.set_Field("SO_InvDate2", DateTime.Now.ToString());
                    DynRec.set_Field("SO_InvDate1", DateTime.Now.ToString("dd/MM/yyyy"));
                    DynRec.set_Field("SO_Inv1", lblBalanceAP.Text);
                    DynRec.set_Field("TP_DocNo", lblLoyaltyPoint.Text + "(" + DateTime.Now + ")");
                }
                else if (docStat == 3)
                {
                    DynRec.set_Field("NextApprover", amount.SAManager1);
                }
                else if (docStat == 4)
                {
                    DynRec.set_Field("NextApprover", amount.GM1);
                }
                else if (docStat == 5)
                {
                    DynRec.set_Field("NextApprover", "");
                }
                else if (docStat == 7)
                {
                    DynRec.set_Field("NextApprover", amount.OManager1);
                }
                else if (docStat == 8)
                {
                    DynRec.set_Field("NextApprover", amount.CcManager1);
                }

                if (txtRemark.Text != "") DynRec.set_Field("Remarks_Sales", txtRemark.Text);

                // Jerry 2024-12-31 Skip check if cbAnP is checked. Update HODAnP as long as txtAnP has value
                // if (cbAnP.Checked) DynRec.set_Field("HODAnP", txtAnP.Text);

                double valueAnP;

                // Validate if the textbox value is not empty, can be parsed to a double, and is not 0
                if (!string.IsNullOrWhiteSpace(txtAnP.Text) &&
                    double.TryParse(txtAnP.Text, out valueAnP) &&
                    valueAnP != 0)
                {
                    string valueAnP_s = valueAnP.ToString();
                    DynRec.set_Field("HODAnP", valueAnP_s); // Update the HODAnP field
                }
                // Jerry 2024-12-31 Skip check if cbAnP is checked. Update HODAnP as long as txtAnP has value - END

                if (txtHod1.Text != "") DynRec.set_Field("Remarks_HOD", txtHod1.Text);
                if (txtHod2.Text != "") DynRec.set_Field("Remarks_HOD2", txtHod2.Text);
                if (txtHod3.Text != "") DynRec.set_Field("Remarks_HOD3", txtHod3.Text);
                if (txtAdmin.Text != "") DynRec.set_Field("Remarks_Admin", txtAdmin.Text);
                if (txtCcMng.Text != "") DynRec.set_Field("Remarks_CreditControlMgr", txtCcMng.Text);
                if (ddlPointCategory.SelectedItem.ToString() != "-- SELECT --") DynRec.set_Field("Point_Ctgry_New", ddlPointCategory.SelectedItem.Text);
                if (txtHeader.Text != "") DynRec.set_Field("Header", txtHeader.Text);
                if (ddlCNType.SelectedItem.ToString() != "-- SELECT --") DynRec.set_Field("CNType", Convert.ToInt16(ddlCNType.SelectedValue));
                if (txtCNreason.Text != "") DynRec.set_Field("CNReason", txtCNreason.Text);
                if (ddlRedempType.SelectedItem.ToString() != "-- SELECT --") DynRec.set_Field("RedemptItemType", Convert.ToInt16(ddlRedempType.SelectedValue));

                if (ddlLedgerAcc.SelectedItem.ToString() != "")
                {
                    LedgerNum = ddlLedgerAcc.SelectedItem.Text.Substring(0, 8);
                    if (ddlLedgerAcc.SelectedItem.ToString() != "-- SELECT --") DynRec.set_Field("LedgerAcc", LedgerNum);
                }

                if (txtAdminMng.Text != "") DynRec.set_Field("Remarks_AdminMgr", txtAdminMng.Text);

                if (cbSpecialApproval.Checked) DynRec.set_Field("SpecialApprove", 1);
                else { DynRec.set_Field("SpecialApprove", 0); }

                if (txtOperationMng.Text != "") DynRec.set_Field("OperationMgr_Reason", txtOperationMng.Text);

                if (cbGMApproval.Checked) DynRec.set_Field("SpecialGM", 1);
                else { DynRec.set_Field("SpecialGM", 0); }

                if (hdSubmitReason.Value != "")
                {
                    DynRec.set_Field("Special_Approve_Reason", hdSubmitReason.Value);
                }
                int hodlevel = Convert.ToInt16(DynRec.get_Field("HODLevel"));
                DynRec.set_Field("HODLevel", hodLvl);
                DynRec.set_Field("AppliedDate", DateTime.Today);
                DynRec.set_Field("ProcessDate", DateTime.Today);
                DynRec.set_Field("ProcessStatus", ProcessStat);

                if (ddlRedempType.SelectedValue == "3" || ddlRedempType.SelectedValue == "4")
                {
                    DynRec.set_Field("PurposeCode", ddlCnCategory.SelectedItem.Value);
                }

                if (hdStatus.Value != "Rejected")
                {
                    // Jerry 2024-11-01 Insert into PPMTP_tmpUsedPts & PPMTP_tmpUsedPts_Acc after final approval
                    //if (docStat == 3 || cbSpecialApproval.Checked)//after sa manager approve
                    if (docStat == 5)
                    {
                        var getPaymentTerm = Redemption_Get_Details.get_rd_CreditInfo2(DynAx, TextBox_Account.Text);
                        var getZipCode = EOR_GET_NewApplicant.getCustInfo_2(DynAx, TextBox_Account.Text);
                        var todayDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //string temp_SuffixCode = SFA_GET_SALES_HEADER.get_SuffixCode(DynAx, Label_Class.Text);
                        var getWarehouse = EOR_GET_NewApplicant.getCustInfo(DynAx, TextBox_Account.Text);

                        double usedPoint = 0;
                        double anp_point = 0; // Jerry 2024-11-25 To update in PPMTP_tmpUsedPts
                        string journalId = "";

                        if (cbAnP.Checked)
                        {
                            if (txtAnP.Text == "")
                            {
                                //Jerry 2024-11-07 Fixed the invalid string error due to empty space between '-' and the number
                                //usedPoint = Convert.ToDouble("-" + cbAnP.Text);
                                string input = "-" + cbAnP.Text.Trim();
                                input = input.Replace(" ", "").Replace(",", "");
                                usedPoint = Convert.ToDouble(input);
                                //Jerry ends
                            }
                            else
                            {
                                // Jerry 2024-11-25 To update in PPMTP_tmpUsedPts (seperate LP and AP deduction)
                                //if (double.TryParse(txtAnP.Text, out usedPoint))
                                //{
                                //    usedPoint = -usedPoint;
                                //}

                                double total_redeem = Convert.ToDouble(txtPts.Text.Replace(",", ""));
                                anp_point = Convert.ToDouble(txtAnP.Text.Replace(",", ""));
                                usedPoint = total_redeem - anp_point;
                                anp_point = anp_point * -1;
                                usedPoint = usedPoint * -1;
                                // Jerry 2024-11-25 End
                            }
                        }
                        else
                        {
                            double lp = Convert.ToDouble(Label_Point.Text);
                            if (lp - Convert.ToDouble(txtPts.Text) < 0)
                            {
                                // Jerry 2024-11-20 Temporary allow negative Loyalty Point to go through
                                //lblRedWarning.Focus();
                                //lblRedWarning.Visible = true;
                                //lblRedWarning.Text = "Loyalty point not enough! Please use A&P Point!";
                                //return false;
                                usedPoint = Convert.ToDouble("-" + txtPts.Text);
                                // Jerry 2024-11-20 End
                            }
                            else
                            {
                                usedPoint = Convert.ToDouble("-" + txtPts.Text);
                            }
                        }

                        string journalid = "";

                        using (DynRec = DynAx.CreateAxaptaRecord("PPMTP_tmpUsedPts"))
                        {
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "PPMTP_RedempNo", lblGetRedempID.Text));
                            DynAx.TTSBegin();

                            if (DynRec.Found)
                            {
                                journalid = DynRec.get_Field("PPMTP_JournalId").ToString();
                                DynRec.set_Field("PPMTP_RedempNo", lblGetRedempID.Text);
                                //Jerry 2024-11-01 Insert after final approval, LF_RedemptionStatus set to 2 (Approved)
                                //DynRec.set_Field("LF_RedemptionStatus", 1);
                                DynRec.set_Field("LF_RedemptionStatus", 2);
                                if (ddlPointCategory.SelectedItem.Text != "")
                                {
                                    string pointCategory = ddlPointCategory.SelectedItem.Text.Substring(0, 5);
                                    DynRec.set_Field("LF_PointCategoryUsed", pointCategory);
                                }
                                DynRec.set_Field("PPMTP_DocDate", DateTime.Today);
                                DynRec.set_Field("PPMTP_CustAcc", TextBox_Account.Text);
                                // Jerry 2024-11-25 Save both LP and AP
                                //if (cbAnP.Checked)
                                //{
                                //    DynRec.set_Field("PPMTP_AddUsedPt", usedPoint);
                                //    DynRec.set_Field("PPMTP_UsedPt", 0.00);
                                //}
                                //else
                                //{
                                //    DynRec.set_Field("PPMTP_UsedPt", usedPoint);
                                //}
                                DynRec.set_Field("PPMTP_AddUsedPt", anp_point);
                                DynRec.set_Field("PPMTP_UsedPt", usedPoint);
                                // Jerry 2024-11-25 End
                                DynRec.set_Field("PPMTP_Salesman", hdsalemanID.Value);
                                DynRec.set_Field("Type", "PPMTP");
                                DynRec.set_Field("PPMTP_Remark", txtAdmin.Text);
                                DynRec.set_Field("PPMTP_Dimension", getWarehouse.Item5);

                                // Jerry 2024-12-05 Control Sign
                                DynRec.set_Field("PPMTPControlSign", 6);
                                // Jerry 2024-12-05 End

                                if (ddlRedempType.SelectedValue == "2")
                                {
                                    DynRec.set_Field("SalesId", salesID);
                                }

                                AxaptaObject webredempt = DynAx.CreateAxaptaObject("WebRedempt");
                                journalId = webredempt.Call("newJournalId").ToString();

                                DynRec.set_Field("PPMTP_journalId", journalId);
                                DynRec.Call("Update");
                            }
                            else
                            {
                                DynRec.set_Field("PPMTP_RedempNo", lblGetRedempID.Text);
                                //Jerry 2024-11-01 Insert after final approval, LF_RedemptionStatus set to 2 (Approved)
                                //DynRec.set_Field("LF_RedemptionStatus", 1);
                                DynRec.set_Field("LF_RedemptionStatus", 2);
                                if (ddlPointCategory.SelectedItem.Text != "")
                                {
                                    string pointCategory = ddlPointCategory.SelectedItem.Text.Substring(0, 5);
                                    DynRec.set_Field("LF_PointCategoryUsed", pointCategory);
                                }
                                DynRec.set_Field("PPMTP_DocDate", DateTime.Today);
                                DynRec.set_Field("PPMTP_CustAcc", TextBox_Account.Text);
                                // Jerry 2024-11-25 Save both LP and AP
                                //if (cbAnP.Checked)
                                //{
                                //    DynRec.set_Field("PPMTP_AddUsedPt", usedPoint);
                                //    DynRec.set_Field("PPMTP_UsedPt", 0.00);
                                //}
                                //else
                                //{
                                //    DynRec.set_Field("PPMTP_UsedPt", usedPoint);
                                //}
                                DynRec.set_Field("PPMTP_AddUsedPt", anp_point);
                                DynRec.set_Field("PPMTP_UsedPt", usedPoint);
                                // Jerry 2024-11-25 End
                                DynRec.set_Field("PPMTP_Salesman", hdsalemanID.Value);
                                DynRec.set_Field("Type", "PPMTP");
                                DynRec.set_Field("PPMTP_Remark", txtAdmin.Text);
                                DynRec.set_Field("PPMTP_Dimension", getWarehouse.Item5);
                                DynRec.set_Field("Post", 1);

                                // Jerry 2024-12-05 Control Sign
                                DynRec.set_Field("PPMTPControlSign", 6);
                                // Jerry 2024-12-05 End

                                if (ddlRedempType.SelectedValue == "2")
                                {
                                    DynRec.set_Field("SalesId", salesID);
                                }

                                AxaptaObject webredempt = DynAx.CreateAxaptaObject("WebRedempt");
                                journalId = webredempt.Call("newJournalId").ToString();

                                DynRec.set_Field("PPMTP_journalId", journalId);
                                lblGetJournalID.Text = journalId;
                                DynRec.Call("Insert");
                            }
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }

                        // PPMTP_tmpUsedPts_Acc
                        using (DynRec = DynAx.CreateAxaptaRecord("PPMTP_tmpUsedPts_Acc"))
                        {
                            DynAx.TTSBegin();
                            DynRec.set_Field("PPMTP_RedempNo", lblGetRedempID.Text);
                            if (ddlPointCategory.SelectedItem.Text != "")
                            {
                                string pointCategory = ddlPointCategory.SelectedItem.Text.Substring(0, 5);
                                DynRec.set_Field("LF_PointCategoryUsed", pointCategory);
                            }
                            DynRec.set_Field("PPMTP_DocDate", DateTime.Today);
                            DynRec.set_Field("PPMTP_CustAcc", TextBox_Account.Text);
                            // Jerry 2024-11-25 Save both LP and AP
                            //if (cbAnP.Checked)
                            //{
                            //    DynRec.set_Field("PPMTP_AddUsedPt", usedPoint);
                            //    DynRec.set_Field("PPMTP_UsedPt", 0.00);
                            //}
                            //else
                            //{
                            //    DynRec.set_Field("PPMTP_UsedPt", usedPoint);
                            //}
                            DynRec.set_Field("PPMTP_AddUsedPt", anp_point);
                            DynRec.set_Field("PPMTP_UsedPt", usedPoint);
                            // Jerry 2024-11-25 End
                            DynRec.set_Field("PPMTP_Salesman", hdsalemanID.Value);
                            DynRec.set_Field("Type", "PPMTP");
                            DynRec.set_Field("PPMTP_Remark", txtAdmin.Text);
                            DynRec.set_Field("PPMTP_Dimension", getWarehouse.Item5);
                            DynRec.set_Field("Post", 1);

                            if (ddlRedempType.SelectedValue == "2")
                            {
                                DynRec.set_Field("SalesId", salesID);
                            }

                            DynRec.set_Field("PPMTP_journalId", journalId);
                            lblGetJournalID.Text = journalId;
                            DynRec.Call("Insert");

                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }

                        //PO CN SO
                        //Jerry 2024-12-06 Temporary disabled until PurchLine ready
                        //if (ddlRedempType.SelectedValue == "1")// PO 
                        if (ddlRedempType.SelectedValue == "11111")// PO 
                        {
                            AxaptaObject axPurchTable = DynAx.CreateAxaptaObject("AxPurchTable");
                            AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

                            axPurchTable.Call("parmPurchName", Label_CustName.Text);
                            axPurchTable.Call("parmOrderAccount", lblGetVendor.Text);//vendor account
                            axPurchTable.Call("parmInvoiceAccount", lblGetVendor.Text);
                            axPurchTable.Call("parmVendGroup", hdVendorGrp.Value);

                            axPurchTable.Call("parmDeliveryDate", DateTime.Today);
                            axPurchTable.Call("parmCurrencyCode", "MYR");
                            axPurchTable.Call("SuffixCode", "/PT");
                            axPurchTable.Call("parmPayment", getPaymentTerm.Item5.ToString());
                            axPurchTable.Call("parmDeliveryAddress", txtDeliveryAddr.Text);
                            axPurchTable.Call("parmPurchaseType", 3);
                            axPurchTable.Call("parmVendorRef", lblGetJournalID.Text + " " + lblGetRedempID.Text);

                            var split = txtDeliveryAddr.Text.Split(',');
                            axPurchTable.Call("parmDeliveryZipCode", getZipCode.Item4.ToString());
                            axPurchTable.Call("parmDlvCountryRegionId", "MY");
                            axPurchTable.Call("parmDlvState", getZipCode.Item6.ToString());
                            axPurchTable.Call("parmDeliveryName", Label_CustName.Text);
                            axPurchTable.Call("parmLanguageId", "En-us");
                            axPurchTable.Call("parmDeliveryCity", getZipCode.Item5.ToString());
                            axPurchTable.Call("parmDeliveryStreet", txtDeliveryAddr.Text);

                            axPurchTable.Call("save");

                            // Jerry 2024-12-05 Update PurchId with suffix /PT
                            //PurchId = axPurchTable.Call("parmPurchId").ToString();

                            PurchId = axPurchTable.Call("parmPurchId").ToString() + "/PT";
                            axPurchTable.Call("parmPurchId", PurchId);
                            axPurchTable.Call("save");
                            // Jerry 2024-12-05 End


                            // Jerry 2024-12-05 Update PPMTP_tmpUsedPts with PurchId
                            using (DynRec = DynAx.CreateAxaptaRecord("PPMTP_tmpUsedPts"))
                            {
                                DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "PPMTP_RedempNo", lblGetRedempID.Text));
                                DynAx.TTSBegin();

                                if (DynRec.Found)
                                {
                                    DynRec.set_Field("PurchId", PurchId);
                                    DynRec.Call("Update");
                                }

                                DynAx.TTSCommit();
                                DynAx.TTSAbort();
                            }
                            // Jerry 2024-12-05 End


                            //for (int i = 0; i < 3; i++)
                            //{
                            //    if (gvItemPoint.Rows[i].RowType == DataControlRowType.DataRow)
                            //    {
                            //        TextBox itemNameTextBox = (TextBox)gvItemPoint.Rows[i].Cells[1].FindControl("txtItems");
                            //        TextBox itemCodeTextBox = (TextBox)gvItemPoint.Rows[i].Cells[2].FindControl("txtItemCode");
                            //        TextBox quantityTextBox = (TextBox)gvItemPoint.Rows[i].Cells[3].FindControl("txtQty");
                            //        TextBox unitPriceTextBox = (TextBox)gvItemPoint.Rows[i].Cells[4].FindControl("txtUnitPrice");
                            //        TextBox totalTextBox = (TextBox)gvItemPoint.Rows[i].Cells[5].FindControl("txtTotal");
                            //        TextBox pointTextBox = (TextBox)gvItemPoint.Rows[i].Cells[6].FindControl("txtPoints");
                            //        TextBox invoiceNoTextBox = (TextBox)gvItemPoint.Rows[i].Cells[7].FindControl("txtInvoiceNo");
                            //        TextBox invoiceDateTextBox = (TextBox)gvItemPoint.Rows[i].Cells[8].FindControl("txtInvoiceDate");

                            //        if (!string.IsNullOrEmpty(itemCodeTextBox.Text) && !string.IsNullOrEmpty(itemNameTextBox.Text))
                            //        {
                            //            AxaptaObject axPurchLine = DynAx.CreateAxaptaObject("AxPurchLine");

                            //            if (invoiceDateTextBox.Text == "")
                            //            {
                            //                invoiceDateTextBox.Text = DateTime.Today.Date.ToString();
                            //            }//if got item, no date, assign today 
                            //            var purchid = axPurchLine.Call("InitPurchLine", PurchId);

                            //            axPurchLine.Call("parmPurchId", PurchId + "/PT");
                            //            axPurchLine.Call("parmItemId", itemCodeTextBox.Text);
                            //            axPurchLine.Call("ItemName", itemNameTextBox.Text);
                            //            axPurchLine.Call("parmLineNum", i + 1);
                            //            axPurchLine.Call("parmCurrencyCode", "MYR");
                            //            string itemUnitStr = domComSalesLine.Call("getValidUnits", itemCodeTextBox.Text).ToString();//fieldValue=Itemid
                            //            string[] arr_itemUnitStr = itemUnitStr.Split(new char[] { ',', '=' });

                            //            axPurchLine.Call("parmPurchUnit", arr_itemUnitStr[2]);
                            //            axPurchLine.Call("parmPurchQty", quantityTextBox.Text);
                            //            axPurchLine.Call("parmQtyOrdered", quantityTextBox.Text);
                            //            axPurchLine.Call("parmPurchPrice", unitPriceTextBox.Text);
                            //            axPurchLine.Call("parmShippingDateRequested", todayDate);
                            //            axPurchLine.Call("dosave");
                            //        }
                            //    }
                            //}
                        }
                        else if (ddlRedempType.SelectedValue == "2")// SO
                        {
                            AxaptaObject axSalesTable = DynAx.CreateAxaptaObject("AxSalesTable");
                            AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

                            axSalesTable.Call("parmCustAccount", TextBox_Account.Text);
                            axSalesTable.Call("parmSalesType", 3);// sales order
                            axSalesTable.Call("parmCustomerRef", lblGetRedempID.Text);
                            axSalesTable.Call("suffixCode", "/INV");
                            axSalesTable.Call("save");

                            salesID = axSalesTable.Call("parmSalesID").ToString();
                            for (int i = 0; i < 3; i++)
                            {
                                if (gvItemPoint.Rows[i].RowType == DataControlRowType.DataRow)
                                {
                                    TextBox itemNameTextBox = (TextBox)gvItemPoint.Rows[i].Cells[1].FindControl("txtItems");
                                    TextBox itemCodeTextBox = (TextBox)gvItemPoint.Rows[i].Cells[2].FindControl("txtItemCode");
                                    TextBox quantityTextBox = (TextBox)gvItemPoint.Rows[i].Cells[3].FindControl("txtQty");
                                    TextBox unitPriceTextBox = (TextBox)gvItemPoint.Rows[i].Cells[4].FindControl("txtUnitPrice");
                                    TextBox totalTextBox = (TextBox)gvItemPoint.Rows[i].Cells[5].FindControl("txtTotal");
                                    TextBox pointTextBox = (TextBox)gvItemPoint.Rows[i].Cells[6].FindControl("txtPoints");
                                    TextBox invoiceNoTextBox = (TextBox)gvItemPoint.Rows[i].Cells[7].FindControl("txtInvoiceNo");
                                    TextBox invoiceDateTextBox = (TextBox)gvItemPoint.Rows[i].Cells[8].FindControl("txtInvoiceDate");

                                    if (itemNameTextBox.Text != "" && itemCodeTextBox.Text != "")
                                    {
                                        AxaptaObject axSalesLine = DynAx.CreateAxaptaObject("AxSalesLine");
                                        axSalesLine.Call("InitSalesLine", salesID);

                                        if (invoiceDateTextBox.Text == "")
                                        {
                                            invoiceDateTextBox.Text = DateTime.Today.Date.ToString();
                                        }//if got item, no date, assign today date

                                        if (getWarehouse.Item5.ToString() == "")//default "MP" if no warehouse from custable
                                        {
                                            inventDimId = domComSalesLine.Call("getInventDimId", itemCodeTextBox.Text, "", "", "", "MP", "").ToString();
                                        }
                                        else
                                        {
                                            inventDimId = domComSalesLine.Call("getInventDimId", itemCodeTextBox.Text, "", "", "", getWarehouse.Item5, "").ToString();
                                        }

                                        if (inventDimId == "")
                                        {
                                            Function_Method.MsgBox("No warehouse. Please check again.", this.Page, this);
                                            Function_Method.AddLog("Customer Account:" + TextBox_Account.Text + "\nInventDimID: " + inventDimId + "\nLine 209 Redemption_Sub_NewApplicant");
                                            return false;
                                        }
                                        else
                                        {
                                            axSalesLine.Call("parmInventDimId", inventDimId);
                                            hidden_inventDim.Value = inventDimId;
                                        }

                                        axSalesLine.Call("parmItemId", itemCodeTextBox.Text);
                                        axSalesLine.Call("parmName", itemNameTextBox.Text);

                                        string itemUnitStr = domComSalesLine.Call("getValidUnits", itemCodeTextBox.Text).ToString();//fieldValue=Itemid
                                        string[] arr_itemUnitStr = itemUnitStr.Split(new char[] { ',', '=' });

                                        axSalesLine.Call("SuffixCode", "/INV");
                                        //axSalesLine.Call("parmSalesPrice", decimal.Parse(unitPriceTextBox.Text));
                                        axSalesLine.Call("parmSalesUnit", arr_itemUnitStr[2]);
                                        axSalesLine.Call("parmExpectedRetQty", quantityTextBox.Text);
                                        axSalesLine.Call("parmSalesQty", quantityTextBox.Text);
                                        axSalesLine.Call("parmQtyOrdered", quantityTextBox.Text);
                                        axSalesLine.Call("parmShippingDateRequested", todayDate);
                                        axSalesLine.Call("parmReturnDeadLine", todayDate);
                                        axSalesLine.Call("CNInvId", invoiceNoTextBox.Text);//for draft cn purposes
                                        axSalesLine.Call("dosave");
                                    }
                                    else
                                    {
                                        Function_Method.AddLog(lblGetRedempID.Text + " item name: " + itemNameTextBox.Text + " item code: " + itemCodeTextBox.Text);
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (ddlRedempType.SelectedValue == "3")//CN
                        {
                            using (DynRec = DynAx.CreateAxaptaRecord("CustInvoiceTable"))
                            {
                                DynRec.set_Field("OrderAccount", TextBox_Account.Text);
                                DynRec.set_Field("InvoiceAccount", TextBox_Account.Text);
                                DynRec.set_Field("Name", lblCustName.Text);
                                DynRec.set_Field("Address", txtDeliveryAddr.Text);
                                DynRec.set_Field("ZipCode", getZipCode.Item4.ToString());
                                DynRec.set_Field("CountryRegionId", "MY");
                                DynRec.set_Field("Street", txtDeliveryAddr.Text);
                                DynRec.set_Field("LanguageId", "EN-US");
                                DynRec.set_Field("State", getZipCode.Item6.ToString());
                                DynRec.set_Field("City", getZipCode.Item5.ToString());
                                DynRec.set_Field("InvoiceDate", todayDate);
                                DynRec.set_Field("LF_CNDNPurposeCode", ddlCnCategory.SelectedItem.Text);
                                DynRec.set_Field("Payment", getPaymentTerm.Item5.ToString());
                                DynRec.set_Field("DebitCreditType", 1);
                                DynRec.set_Field("LF_JournalId", lblGetJournalID.Text);
                                DynRec.set_Field("MSB_CNType", Convert.ToInt16(ddlCNType.SelectedValue));
                                DynRec.set_Field("CurrencyCode", "MYR");
                                DynRec.Call("Insert");

                                recid = DynRec.get_Field("RecId").ToString();
                            }

                            using (DynRec = DynAx.CreateAxaptaRecord("CustInvoiceLine"))
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (gvItemPoint.Rows[i].RowType == DataControlRowType.DataRow)
                                    {
                                        TextBox itemNameTextBox = (TextBox)gvItemPoint.Rows[i].Cells[1].FindControl("txtItems");
                                        TextBox itemCodeTextBox = (TextBox)gvItemPoint.Rows[i].Cells[2].FindControl("txtItemCode");
                                        TextBox quantityTextBox = (TextBox)gvItemPoint.Rows[i].Cells[3].FindControl("txtQty");
                                        TextBox unitPriceTextBox = (TextBox)gvItemPoint.Rows[i].Cells[4].FindControl("txtUnitPrice");
                                        TextBox totalTextBox = (TextBox)gvItemPoint.Rows[i].Cells[5].FindControl("txtTotal");
                                        TextBox pointTextBox = (TextBox)gvItemPoint.Rows[i].Cells[6].FindControl("txtPoints");
                                        TextBox invoiceNoTextBox = (TextBox)gvItemPoint.Rows[i].Cells[7].FindControl("txtInvoiceNo");
                                        TextBox invoiceDateTextBox = (TextBox)gvItemPoint.Rows[i].Cells[8].FindControl("txtInvoiceDate");
                                        if (itemNameTextBox.Text != "")
                                        {
                                            DynRec.set_Field("ParentRecId", Int64.Parse(recid));
                                            DynRec.set_Field("LineNum", Convert.ToDouble(i));
                                            DynRec.set_Field("Description", itemNameTextBox.Text);
                                            DynRec.set_Field("AmountCur", Convert.ToDouble("-" + totalTextBox.Text));
                                            DynRec.set_Field("AmountCurCN", Convert.ToDouble(totalTextBox.Text));
                                            DynRec.set_Field("LF_InvoiceID", invoiceNoTextBox.Text);
                                            if (itemCodeTextBox.Text != "")
                                            {
                                                DynRec.set_Field("LF_ItemId", itemCodeTextBox.Text);
                                            }
                                            DynRec.set_Field("LedgerAccount", LedgerNum);
                                            DynRec.set_Field("CN_Reason", txtCNreason.Text);
                                            DynRec.set_Field("HeaderParticulars", txtHeader.Text);
                                            DynRec.Call("Insert");
                                        }
                                    }
                                }
                            }
                        }

                        // TO DO: Insert JournalId back to LF_WebRedemp
                        /*
                        using (AxaptaRecord DynRec2 = DynAx.CreateAxaptaRecord("LF_WebRedemp"))
                        {
                            DynAx.TTSBegin();

                            //update redemption details RC/DU
                            DynRec2.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Rdemt_ID", hdRedemID.Value));
                            if (DynRec2.Found)
                            {
                                DynRec2.set_Field("LF_JournalID", journalid);
                            }
                        }
                        */
                    }
                }
                return true;
            }
            catch (Exception ER_RD_04)
            {
                Function_Method.MsgBox("ER_RD_04: " + ER_RD_04.Message, this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private bool Save_LF_WebRedemp(AxaptaRecord DynRec, int docStat, string ProcessStat)
        {
            return Save_LF_WebRedemp(DynRec, docStat, ProcessStat, false);
        }

        private bool Save_LF_WebRedemp(AxaptaRecord DynRec, int docStat, string ProcessStat, bool IsUpdate)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                double lp = 0;
                double ap = 0;

                using (AxaptaRecord DynRec1 = DynAx.CreateAxaptaRecord("PointBalance"))
                {
                    DynAx.TTSBegin();
                    DynRec1.ExecuteStmt(string.Format("select * from %1 where %1.{0} == '{1}'", "AccountNum", TextBox_Account.Text));

                    var LPCF = DynRec1.get_Field("TPBalance");
                    var APCF = DynRec1.Call("getAddBalance");

                    lp = Convert.ToDouble(LPCF);
                    ap = Convert.ToDouble(APCF);
                }


                var split = Label_Salesman.Text.Split('(', ')');
                hdsalemanID.Value = split[1];
                string getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, hdsalemanID.Value);//salesapprovalgroupid

                string[] arr_NA_HOD = getHod.Split('_');
                int count_arr_NA_HOD = arr_NA_HOD.Count();
                string UserName = "";

                //Jerry 2024-11-19 Check if interest waiver is checked, then docStat needs to update to 8 (awaiting CC Manager)
                //Jerry 2024-11-08 Add docStat
                //DynRec.set_Field("DocStatus", docStat);
                //Jerry 2024-11-08 End
                //Jerry 2024-11-19 End
                // Jerry 2024-11-07 Attempt to save InterestWaiver
                if (cbInterestWaive.Checked)
                {
                    DynRec.set_Field("InterestWaiver", 1);
                    //Jerry 2024-11-19 if interest waiver is checked, docStat set to 8
                    DynRec.set_Field("DocStatus", 8);
                    //Jerry 2024-11-19 end
                }
                else
                {
                    DynRec.set_Field("InterestWaiver", 0);
                    //Jerry 2024-11-19 if interest waiver is not checked, docStat follow parameter
                    DynRec.set_Field("DocStatus", docStat);
                    //Jerry 2024-11-19 end
                }
                // Jerry 2024-11-07 end

                //Cannot be updated
                if (!IsUpdate)
                {
                    DynRec.set_Field("HODLevel", count_arr_NA_HOD.ToString());

                    var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(txtRM.Text));

                    if (count_arr_NA_HOD == 3 && !cbInterestWaive.Checked)
                    {
                        if (!string.IsNullOrEmpty(arr_NA_HOD[2].ToString()))
                        {
                            UserName = Function_Method.GetLoginedUserFullName(arr_NA_HOD[2]);
                            DynRec.set_Field("NextApprover", UserName);
                        }
                    }
                    else if (count_arr_NA_HOD == 2 && !cbInterestWaive.Checked)
                    {
                        if (!string.IsNullOrEmpty(arr_NA_HOD[1].ToString()))
                        {
                            UserName = Function_Method.GetLoginedUserFullName(arr_NA_HOD[1]);
                            DynRec.set_Field("NextApprover", UserName);
                        }
                    }
                    else if (cbInterestWaive.Checked)
                    {
                        if (!string.IsNullOrEmpty(amount.CcManager1))
                        {
                            //UserName = Function_Method.GetLoginedUserFullName(amount.CcManager1);
                            DynRec.set_Field("NextApprover", amount.CcManager1);
                        }
                    }
                    else /*if (hodLevel == "1" || hodLevel == "0")*/
                    {
                        if (count_arr_NA_HOD > 0 && !string.IsNullOrEmpty(arr_NA_HOD[0].ToString()))
                        {
                            UserName = Function_Method.GetLoginedUserFullName(arr_NA_HOD[0]);
                            DynRec.set_Field("NextApprover", UserName);
                        }
                    }
                }
                // if (cbAnP.Checked) DynRec.set_Field("HODAnP", txtAnP.Text);
                if (Label_CustName.Text != "") DynRec.set_Field("CustAccount", TextBox_Account.Text);
                if (Textbox_TelNo.Text != "") DynRec.set_Field("CustPhone", Textbox_TelNo.Text);
                if (Textbox_ContactPerson.Text != "") DynRec.set_Field("CustContact", Textbox_ContactPerson.Text);
                if (txtDeliveryAddr.Text != "") DynRec.set_Field("Del_Addr", txtDeliveryAddr.Text);
                if (txtIc.Text != "") DynRec.set_Field("Benefit_IC", txtIc.Text);
                //if (txtTax.Text != "") DynRec.set_Field("Benefit_Tax_No", txtTax.Text);
                var emplid = Label_Salesman.Text.Split('(', ')');
                DynRec.set_Field("EmplName", emplid[2].TrimStart());

                if (txtBeneficiaryName.Text != "") DynRec.set_Field("Benefit_Name", txtBeneficiaryName.Text);
                else DynRec.set_Field("Benefit_Name", lblBeneficiaryName.Text);

                //double AnP = 0;
                //try
                //{
                //    if (cbAnP.Checked == true)
                //        AnP = Double.Parse(txtAnP.Text);
                //}
                //catch
                //{

                //}
                //double deductedPoint = (lp + (cbAnP.Checked == true ? AnP : 0));

                for (int i = 0; i < 3; i++)
                {
                    if (gvItemPoint.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox itemNameTextBox = (TextBox)gvItemPoint.Rows[i].Cells[1].FindControl("txtItems");
                        TextBox itemCodeTextBox = (TextBox)gvItemPoint.Rows[i].Cells[2].FindControl("txtItemCode");
                        TextBox quantityTextBox = (TextBox)gvItemPoint.Rows[i].Cells[3].FindControl("txtQty");
                        TextBox unitPriceTextBox = (TextBox)gvItemPoint.Rows[i].Cells[4].FindControl("txtUnitPrice");
                        TextBox totalTextBox = (TextBox)gvItemPoint.Rows[i].Cells[5].FindControl("txtTotal");
                        TextBox pointTextBox = (TextBox)gvItemPoint.Rows[i].Cells[6].FindControl("txtPoints");
                        TextBox invoiceNoTextBox = (TextBox)gvItemPoint.Rows[i].Cells[7].FindControl("txtInvoiceNo");
                        TextBox invoiceDateTextBox = (TextBox)gvItemPoint.Rows[i].Cells[8].FindControl("txtInvoiceDate");

                        if (itemNameTextBox.Text != "")
                        {
                            if (invoiceDateTextBox.Text == "")
                            {
                                invoiceDateTextBox.Text = DateTime.Today.Date.ToString();
                            }
                            SetItemFields(DynRec, i, itemNameTextBox, quantityTextBox, unitPriceTextBox, totalTextBox, pointTextBox, invoiceNoTextBox, invoiceDateTextBox, itemCodeTextBox);
                        }
                        //if (!string.IsNullOrEmpty(pointTextBox.Text))
                        //{
                        //    deductedPoint = deductedPoint - Double.Parse(pointTextBox.Text);

                        //    if (deductedPoint < 0)
                        //    {
                        //        return false;
                        //    }
                        //}

                    }
                }
                DynRec.set_Field("AttachmentID", "Posted");
                if (txtRM.Text != "") DynRec.set_Field("Rdempt_Amt", Convert.ToDouble(txtRM.Text));
                if (txtPts.Text != "") DynRec.set_Field("Rdempt_Point", Convert.ToDouble(txtPts.Text));

                int bank = 0;
                if (rblBank.SelectedItem.Value == "0")
                { bank = 0; }
                else if (rblBank.SelectedItem.Value == "1")
                { bank = 1; }
                else
                { bank = 2; }

                DynRec.set_Field("Benefit_Bank", bank);
                DynRec.set_Field("Remarks_Sales", txtRemark.Text);
                DynRec.set_Field("Del_To", ddlDelivery.SelectedItem.ToString());
                //DynRec.set_Field("Del_Person", ddlPayment.SelectedItem.ToString());

                DynRec.set_Field("AppliedDate", DateTime.Today);
                DynRec.set_Field("ProcessDate", DateTime.Today);
                DynRec.set_Field("ProcessStatus", ProcessStat);

                return true;
            }
            catch (Exception ER_RD_03)
            {
                Function_Method.MsgBox("ER_RD_03: " + ER_RD_03.Message, this.Page, this);
                throw;

            }
        }

        private void SetItemFields(AxaptaRecord DynRec, int index, TextBox itemNameTextBox, TextBox quantityTextBox,
            TextBox unitPriceTextBox, TextBox totalPrice, TextBox itemPoint, TextBox invoiceNumTextBox, TextBox invoiceDtTextBox, TextBox itemCodeTextBox)
        {
            index = index + 1;
            string itemName = itemNameTextBox.Text;
            int quantity = Convert.ToInt16(quantityTextBox.Text);
            double unitPrice = Convert.ToDouble(unitPriceTextBox.Text);
            double totalValue = Convert.ToDouble(totalPrice.Text);
            double pointValue = Convert.ToDouble(itemPoint.Text);

            string invoiceDt = invoiceDtTextBox.Text;
            //if (string.IsNullOrEmpty(invoiceDt))
            //{
            //    return;
            //}

            DynRec.set_Field("Item" + index, itemName);
            DynRec.set_Field("Qty" + index, quantity);
            DynRec.set_Field("Amt" + index, unitPrice);
            DynRec.set_Field("LF_Item" + index + "_InvNo", invoiceNumTextBox.Text);
            DynRec.set_Field("LF_Item" + index + "_InvDate", Convert.ToDateTime(invoiceDt));

            if (itemCodeTextBox.Text != "")
            {
                DynRec.set_Field("ItemCode" + index, itemCodeTextBox.Text);
            }

            if (!double.IsNaN(totalValue))
            {
                DynRec.set_Field("TAmt" + index, totalValue);
            }

            if (!double.IsNaN(pointValue))
            {
                DynRec.set_Field("PtsVal" + index, pointValue);
            }
        }

        private void UpdRedemption(string ProcessStat, int docStatus, int hodLvl, string status)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            string UserName = Function_Method.GetLoginedUserFullName(GLOBAL.user_id);
            int index = UserName.IndexOf('(');
            if (index != -1)
            {
                UserName = UserName.Substring(0, index).Trim();
            }
            var Email = Redemption_Get_Details.getRedempApprovalEmail();

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebRedemp"))
                {
                    DynAx.TTSBegin();

                    //update redemption details RC/DU
                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "Rdemt_ID", hdRedemID.Value));
                    if (DynRec.Found)
                    {
                        var getPreviousProcessStat = Redemption_Get_Details.get_redempStat(DynAx, hdRedemID.Value);
                        if (ApproverUpdate(DynRec, docStatus, getPreviousProcessStat.Item1 + ProcessStat, hodLvl.ToString(), status))
                        {
                            DynRec.Call("Update");
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                            hdStatus.Value = lbl_Status.Text;
                            //Function_Method.SendMail(Session["user_id"].ToString(), UserName, "Application (" + hdStatus.Value + " " + hdRedemID.Value + ")",
                            //Email.Item1, Email.Item4 + "," + Email.Item5 + "," + Email.Item6, RedempEmailContent(hdStatus.Value));
                            Function_Method.MsgBox("Redemption ID: " + hdRedemID.Value.ToString() + " have been " + status + ".", this.Page, this);
                            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_Redemp_Enquiries'); ", true);//go to overview section

                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ER_RD_02)
            {
                Function_Method.MsgBox("ER_RD_02: " + ER_RD_02.Message, this.Page, this);
            }
            DynAx.Dispose();

        }

        public void hideButton()
        {
            Button_Submit.Visible = false;
            Button_Approve.Visible = false;
            Button_Reject.Visible = false;
            Button_AdminApprove.Visible = false;
            Button_Hold.Visible = false;
            BtnAmend.Visible = false;
            Button_CustomerMasterList.Enabled = false;
            BtnUpdateDoc.Visible = false;
            Button_CheckAcc.Visible = false;
            Button_CheckOutStanding.Visible = false;
        }

        protected void TextBox_DescriptionBatchItem_Changed(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            //get the the id that call the function
            TextBox TextBox_DescriptionBatchItem = sender as TextBox;

            string ClientID = TextBox_DescriptionBatchItem.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)TextBox_DescriptionBatchItem.NamingContainer;
            int index = row.RowIndex;
            //
            TextBox txtItem = (TextBox)gvItemPoint.Rows[index].Cells[1].FindControl("txtItems");
            TextBox txtItemCode = (TextBox)gvItemPoint.Rows[index].Cells[2].FindControl("txtItemCode");
            TextBox txtQty = (TextBox)gvItemPoint.Rows[index].Cells[3].FindControl("txtQty");

            try
            {
                //get the the id that call the function
                string Description_toSearch = txtItem.Text.Trim();

                if (Description_toSearch != "")
                {
                    DropDownList DropDownList_SearchBatchItem = (DropDownList)gvItemPoint.Rows[index].Cells[1].FindControl("DropDownList_SearchBatchItem");

                    DropDownList_SearchBatchItem.Items.Clear();
                    Description_toSearch = "*" + Description_toSearch + "*";
                    List<ListItem> List_BatchItem = EOR_GET_NewApplicant.get_SearchEquipment(DynAx, Description_toSearch, "");
                    if (List_BatchItem.Count > 1)
                    {
                        DropDownList_SearchBatchItem.Items.AddRange(List_BatchItem.ToArray());
                        DropDownList_SearchBatchItem.Visible = true;
                        txtItem.Visible = false;
                    }
                }
            }
            catch (Exception ER_RD_09)
            {
                Function_Method.MsgBox("ER_RD_09: " + ER_RD_09.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
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

            TextBox txtItems = (TextBox)gvItemPoint.Rows[index].Cells[1].FindControl("txtItems");
            TextBox txtItemCode = (TextBox)gvItemPoint.Rows[index].Cells[2].FindControl("txtItemCode");

            string BatchItemId_RecId = DropDownList_SearchBatchItem.SelectedItem.Value;
            string[] arr_BatchItemId_RecId = BatchItemId_RecId.Split('|');
            string ItemId = arr_BatchItemId_RecId[0];

            if (ItemId != "")//Only selected
            {
                txtItems.Text = DropDownList_SearchBatchItem.SelectedItem.ToString();
                txtItemCode.Text = ItemId;
                txtItems.Visible = true;
                DropDownList_SearchBatchItem.Items.Clear();
                DropDownList_SearchBatchItem.Visible = false;
            }

            DynAx.Dispose();
        }

        #region PointCalculation
        private bool PointCalculation(string redemption_Id)
        {
            bool isValid = true;
            Axapta DynAx = Function_Method.GlobalAxapta();
            var get_Point = EOR_GET_NewApplicant.getPointBalance(DynAx, TextBox_Account.Text);
            var getAdminRemarks = Redemption_Get_Details.get_gridViewDataAdmin(DynAx, redemption_Id);
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

                #region Validation

                //lblBalanceAP.Text = (preposted_ap - deducted_ap).ToString("#,###,###,##0.00"); // is to check Balance A&P Point: not in negatif
                if (deducted_lp < 0)
                {
                    if ((preposted_ap - deducted_ap) < 0) //(Table:PointBalance Field: getAddBalance) - (Table:LF_WebRedemp Field: HODAnP)
                    {
                        Function_Method.MsgBox("Balance A&P Point shows negative, please verify the balances.", this.Page, this);
                        isValid = false;
                    }
                }
                else
                {
                    //if ((preposted_ap - deducted_ap) < 0) //(Table:PointBalance Field: getAddBalance) - (Table:LF_WebRedemp Field: HODAnP)
                    //{
                    //    // Prompt the user for confirmation
                    //    var result = Function_Method.Confirm("Balance A&P Point shows negative. Are you sure you want to proceed?", this.Page, this);

                    //    if (result == "Yes")
                    //    {
                    //        // Proceed with the operation
                    //    }
                    //    else
                    //    {
                    //        // User chose not to proceed
                    //        isValid = false;
                    //    }
                    //}
                }
                #endregion
                return isValid;
            }
        }
        #endregion
    }
}