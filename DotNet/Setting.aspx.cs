using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace DotNet
{
    public partial class Setting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                LoadSelectionMenu_Setting();
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                   null, null,
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
                /*
                if (GLOBAL.debug == true)
                {
                    AddUserTag.Visible = true;
                }   */
            }

            if (GLOBAL.user_authority_lvl == 1)
            {
                admin.Visible = true;
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
                //
            }
            catch
            {
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void LoadSelectionMenu_Setting()
        {
            Axapta DynAx = new Axapta();
            // Log on to Microsoft Dynamics AX.
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            //check user authority
            if ((GLOBAL.user_authority_lvl == 1) || (GLOBAL.user_authority_lvl == 2))//SuperAdmin, Admin
            {
                AddUserTag.Visible = true;
                AddUserTagRed.Visible = false;
                ApprovalGroup.Visible = true;
                RedempApprovalGroup.Visible = true;
                SignEquipApprovalGroup.Visible = true;
                admin.Visible = true;
            }
            else//Basic
            {
                AddUserTag.Visible = false;
                ApprovalGroup.Visible = false;
                RedempApprovalGroup.Visible = false;
                SignEquipApprovalGroup.Visible = false;
                //Check got second authentification, this when Superadmin switch user=================================
                int user_authority_lvl_Red = Convert.ToInt32(Session["user_authority_lvl_Red"]);
                if ((user_authority_lvl_Red == 1) || (user_authority_lvl_Red == 2))//SuperAdmin, Admin
                {
                    AddUserTagRed.Visible = true;
                }
                else
                {
                    AddUserTagRed.Visible = false;
                }
                //==================================================================================================
                var getWarrantyApproverUser = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, "", ""); // check user HOD or Salesman
                if (GLOBAL.user_id == getWarrantyApproverUser.Item6 || GLOBAL.user_id == getWarrantyApproverUser.Rest.Item1 ||
                    GLOBAL.user_id == getWarrantyApproverUser.Rest.Item2 || GLOBAL.user_id == getWarrantyApproverUser.Rest.Item3 || GLOBAL.user_id == GLOBAL.AdminID)
                {
                    ApprovalGroup.Visible = true;
                }

                var getRedempApproverUser = Redemption_Get_Details.RedempApprovalInUse(DynAx);//ONLY SELECTED user can go throu
                if (getRedempApproverUser.Item4.Contains(GLOBAL.user_id) )
                {
                    RedempApprovalGroup.Visible = true;
                }
            }
            for (int i = 0; i < GLOBAL.no_of_module; i++)
            {
                if ((GLOBAL.module_access_authority & GLOBAL.ConversionData[i]) != 0)
                {
                    if (i == 0)
                    {
                        CustomerMasterTag2.Visible = true;
                    }
                    if (i == 1)
                    {
                        SFATag2.Visible = true;
                    }
                }
            }
        }

        protected void Button_Report_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReport.aspx", true);
        }

        protected void ButtonReport_Group_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReportGroup.aspx", true);
        }

        protected void ButtonReport_PBM_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReport_PBM.aspx", true);
        }

        protected void ButtonReportGroup_PBM_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReportGroup_PBM.aspx", true);
        }

        protected void ButtonMonthlyReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReportGroupMonthly.aspx", true);
        }

        protected void ButtonYTDReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReport_OtherDiv.aspx", true);
        }

        protected void ButtonMonthlyReport_PBM_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesReportGroupMonthly_PBM.aspx", true);
        }

        protected void Button_PoonshReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("WeeklySalesReportGroupForPoonsh.aspx", true);
        }
    }
}