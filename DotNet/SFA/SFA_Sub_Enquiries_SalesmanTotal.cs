using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        protected void Button_SalesmanTotal_Click(object sender, EventArgs e)
        {
            if (f_Button_SalesmanTotal() == false)
            {
                return;
            }
        }
        private bool f_Button_SalesmanTotal()
        {
            if (Dropdown() == false)//no salesman
            {
                return false;
            }
            SalesmanTotal_section.Visible = true;
            BatteryOutstanding_section.Visible = false;
            Button_enquiries_section_accordion.Text = "Salesman Total";
            clear_variable_SalesmanTotal();
            Button_SalesmanTotal.Attributes.Add("style", "background-color:#f58345");
            Button_BatteryOutstanding.Attributes.Add("style", "background-color:transparent");
            Button_QTDSalesmanInvPerform.Attributes.Add("style", "background-color:transparent");
            QTDSmenInvoicePerformance_section.Visible = false;

            ImageButton8.Visible = false;
            return true;
        }

        //protected void Button_SalesmanTotal_StartDate_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (Calendar_SalesmanTotal_StartDate.Visible == false)
        //    {
        //        Calendar_SalesmanTotal_StartDate.Visible = true;
        //    }
        //    else
        //    {
        //        Calendar_SalesmanTotal_StartDate.Visible = false;
        //    }
        //}

        //protected void Calendar_SalesmanTotal_StartDate_SelectionChanged(object sender, EventArgs e)
        //{
        //    string startDate = Calendar_SalesmanTotal_StartDate.SelectedDate.ToString("dd/MM/yyyy");
        //    startDate = Function_Method.get_correct_date(GLOBAL.system_checking, startDate, true);
        //    Label_SalesmanTotal_StartDate.Text = startDate;
        //    Calendar_SalesmanTotal_StartDate.Visible = false;
        //}

        //protected void Button_SalesmanTotal_EndDate_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (Calendar_SalesmanTotal_EndDate.Visible == false)
        //    {
        //        Calendar_SalesmanTotal_EndDate.Visible = true;
        //    }
        //    else
        //    {
        //        Calendar_SalesmanTotal_EndDate.Visible = false;
        //    }
        //}

        //protected void Calendar_SalesmanTotal_EndDate_SelectionChanged(object sender, EventArgs e)
        //{
        //    string endDate = Calendar_SalesmanTotal_EndDate.SelectedDate.ToShortDateString();
        //    endDate = Function_Method.get_correct_date(GLOBAL.system_checking, endDate, true);
        //    Label_SalesmanTotal_EndDate.Text = endDate;
        //    Calendar_SalesmanTotal_EndDate.Visible = false;
        //}
        //=========================================================================================================================//

        private void clear_variable_SalesmanTotal()
        {
            //Label1
            Label_CurrentSalesIDTotal.Text = "";
            Label_SalesmanTotal.Text = "";

            txtStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");//default
            txtEndDate.Text = DateTime.Now.ToString("dd/MM/yyyy"); //default

            hidden_Label_TableRun.Text = "";
            hidden_Label_Test.Text = "";
        }
        //=========================================================================================================================//
        private bool Dropdown()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            DropDownList2.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            try
            {
                items = SFA_GET_Enquiries_SalesmanTotal.getSalesman(DynAx);
                if (items.Count > 1)
                {
                    DropDownList2.Items.AddRange(items.ToArray());
                    return true;
                }
                else
                {
                    Function_Method.MsgBox("There is no Salesman available.", this.Page, this);
                    return false;
                }
            }
            catch (Exception ER_SF_26)
            {
                Function_Method.MsgBox("ER_SF_26: " + ER_SF_26.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void OnSelectedIndexChanged_DropDownList2(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedItem.Value != "")//Only selected
            {
                string SelectedItem = DropDownList2.SelectedItem.Text;
                string[] arr_SelectedItem = SelectedItem.Split(')');
                int temp_length_arr_JournalName = arr_SelectedItem[0].Length + 1;// plus 1 ")"
                string getDescription = SelectedItem.Substring(temp_length_arr_JournalName);//getDescription

                ImageButton8.Visible = true;
            }
            else
            {
                ImageButton8.Visible = false;
            }
        }
        //=========================================================================================================================//
        protected void Button_Search_SalesmanTotal_Click(object sender, ImageClickEventArgs e)
        {
            if (preProcess_SalesManTotalClick() == true)
            {
                CheckSalesmanTotal();
            }
        }

        private void CheckSalesmanTotal()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            try
            {
                string startDate = Request.Form[txtStartDate.UniqueID];
                string endDate = Request.Form[txtEndDate.UniqueID];
                string SalesmanID = DropDownList2.SelectedItem.Value.ToString();
                double totalAmount1 = 0;

                var tuple_get_ITotal_TableRun = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s(DynAx, SalesmanID, startDate, endDate);
                var tuple_get_ITotal_TableRun1 = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s(DynAx, SalesmanID + "-1", startDate, endDate);
                var tuple_get_ITotal_TableRun2 = SFA_GET_Enquiries_SalesmanTotal.get_ITotal_TableRun_s(DynAx, SalesmanID + "-2", startDate, endDate);

                double iTotal = tuple_get_ITotal_TableRun.Item1;
                string TableRun = tuple_get_ITotal_TableRun.Item2;
                string TableSumTax = tuple_get_ITotal_TableRun.Item3;

                double iTotal1 = tuple_get_ITotal_TableRun1.Item1;
                string TableRun1 = tuple_get_ITotal_TableRun1.Item2;
                string TableSumTax1 = tuple_get_ITotal_TableRun1.Item3;

                double iTotal2 = tuple_get_ITotal_TableRun2.Item1;
                string TableRun2 = tuple_get_ITotal_TableRun2.Item2;
                string TableSumTax2 = tuple_get_ITotal_TableRun2.Item3;

                Label_CurrentSalesIDTotal.Text = Math.Round(iTotal, 2).ToString("#,###,###,##0.00"); //round up 2 digit
                Label_CurrentSalesIDTotal1.Text = Math.Round(iTotal1, 2).ToString("#,###,###,##0.00"); //round up 2 digit
                Label_CurrentSalesIDTotal2.Text = Math.Round(iTotal2, 2).ToString("#,###,###,##0.00"); //round up 2 digit

                //=========================================================================================================
                var get_Total = SFA_GET_Enquiries_SalesmanTotal.getTotal(DynAx, SalesmanID, startDate, endDate);
                double totalAmount = get_Total.Item1;
                Label_SalesmanTotal.Text = Math.Round(totalAmount, 2).ToString("#,###,###,##0.00"); //round up 2 digit
                var get_Total1 = SFA_GET_Enquiries_SalesmanTotal.getTotal(DynAx, SalesmanID + "-1", startDate, endDate);

                if (!SalesmanID.Contains("-"))
                {
                    totalAmount1 = get_Total1.Item1;
                    Label_SalesmanTotal1.Text = Math.Round(totalAmount1, 2).ToString("#,###,###,##0.00"); //round up 2 digit

                    var get_Total2 = SFA_GET_Enquiries_SalesmanTotal.getTotal(DynAx, SalesmanID + "-2", startDate, endDate);
                    double totalAmount2 = get_Total2.Item1;
                    Label_SalesmanTotal2.Text = Math.Round(totalAmount2, 2).ToString("#,###,###,##0.00"); //round up 2 digit
                }
                Label_SalesmanSubTotal.Text = Math.Round(totalAmount + totalAmount1, 2).ToString("#,###,###,##0.00");
                txtStartDate.Text = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
                txtEndDate.Text = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");
            }
            catch (Exception ER_SF_27)
            {
                Function_Method.MsgBox("ER_SF_27: " + ER_SF_27.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private bool preProcess_SalesManTotalClick()
        {
            string startDate = Request.Form[txtStartDate.UniqueID];
            string endDate = Request.Form[txtEndDate.UniqueID];
            //startDate = Function_Method.get_correct_date(GLOBAL.system_checking, startDate, false);
            var var_StartQDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);// in lotus use cdat
            //endDate = Function_Method.get_correct_date(GLOBAL.system_checking, endDate, false);
            var var_EndQDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null); // in lotus use cdat
            int res = DateTime.Compare(var_StartQDate, var_EndQDate);

            if (res > 0)//Start date > EndDate
            {
                Function_Method.MsgBox("Invalid Date Range!", this.Page, this);
                return false;
            }

            return true;
        }
        //=========================================================================================================================//
    }
}