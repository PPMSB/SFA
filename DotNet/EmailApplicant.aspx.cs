using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace DotNet
{
    public partial class EmailApplicant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();

            DropDownList();
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

        private void TimeOutRedirect()
        {
            HtmlMeta meta = new HtmlMeta();

            meta.HttpEquiv = "Refresh";

            meta.Content = Convert.ToString(Session.Timeout * 60) + ";url=LoginPage.aspx";

            this.Page.Header.Controls.Add(meta);
        }

        private void DropDownList()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            ddlApplicant.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            try
            {
                items = SFA_GET_Enquiries_SalesmanTotal.getSalesman(DynAx);
                if (items.Count > 1)
                {
                    ddlApplicant.Items.AddRange(items.ToArray());
                    ddlApplicant2.Items.AddRange(items.ToArray());
                }
                else
                {
                    Function_Method.MsgBox("There is no Salesman available.", this.Page, this);
                }
            }
            catch (Exception ER_EM_01)
            {
                Function_Method.MsgBox("ER_EM_01: " + ER_EM_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void buttonSend_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string temp1 = GLOBAL.data_passing.ToString();
                var getNameEmail = Redemption_Get_Details.get_rd_CreditInfo2(DynAx, temp1);

                var cc1 = ddlApplicant.SelectedValue.Split('(', ')');
                var cc2 = ddlApplicant2.SelectedValue.Split('(', ')');
                var getSalesmanEmail = Quotation_Get_Enquiries.getEmpDetails(DynAx, cc1.ToString());
                var getSalesmanEmail2 = Quotation_Get_Enquiries.getEmpDetails(DynAx, cc2.ToString());

                if (getNameEmail.Item4 != "")
                {
                    Function_Method.SendMail("", getNameEmail.Item3, txtSubject.Text, getNameEmail.Item4, getSalesmanEmail.Item3 + "," + getSalesmanEmail2.Item3, txtContent.Text);
                }
                else
                {
                    Function_Method.MsgBox("No email address with this customer.", this.Page, this);
                }
                Session["data_passing"] = "";
            }
            catch (Exception ER_EM_02)
            {
                Function_Method.MsgBox("ER_EM_02: " + ER_EM_02.ToString(), this.Page, this);
                throw;
            }
            finally
            {
                DynAx.Logoff();
            }
        }
    }
}