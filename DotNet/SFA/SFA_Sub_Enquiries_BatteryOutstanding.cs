using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        protected void Button_BatteryOutstanding_Click(object sender, EventArgs e)
        {
            if (BatteryOutstanding() == false)
            {
                return;
            }
            Button_BatteryOutstanding.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_SalesmanTotal.Attributes.Add("style", "background-color:transparent");
            Button_QTDSalesmanInvPerform.Attributes.Add("style", "background-color:transparent");
            QTDSmenInvoicePerformance_section.Visible = false;

            SalesmanTotal_section.Visible = false; Button_enquiries_section_accordion.Text = "Battery Outstanding (Showing top-20 data)";
            BatteryOutstanding_section.Visible = true;
        }

        private bool BatteryOutstanding()
        {
            //GridView_BatteryOutstanding.DataSource = null;
            //GridView_BatteryOutstanding.DataBind();

            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                var tuple_get_BatteryOutstandingList = SFA_GET_Enquiries_BatteryOutstanding.get_BatteryOutstandingList(DynAx, GLOBAL.system_checking);
                List<ListItem> List_CUSTACCOUNT = tuple_get_BatteryOutstandingList.Item1;
                List<ListItem> List_SERIALNUM = tuple_get_BatteryOutstandingList.Item2;
                List<ListItem> List_INVOICEID = tuple_get_BatteryOutstandingList.Item3;
                List<ListItem> List_INVOICEDATE = tuple_get_BatteryOutstandingList.Item4;
                List<ListItem> List_CustName = tuple_get_BatteryOutstandingList.Item5;

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("No.", typeof(string)));
                dt.Columns.Add(new DataColumn("Serial No.", typeof(string)));
                dt.Columns.Add(new DataColumn("Customer", typeof(string)));
                dt.Columns.Add(new DataColumn("Invoice ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Invoice Date", typeof(string)));

                DataRow row;
                int count = 0;
                for (int i = 0; i < 20; i++)//only show top 20
                {
                    count = count + 1;
                    row = dt.NewRow();

                    row["No."] = count.ToString();
                    row["Serial No."] = List_SERIALNUM[i];
                    row["Customer"] = "(" + List_CUSTACCOUNT[i] + ")  " + List_CustName[i];
                    row["Invoice ID"] = List_INVOICEID[i];
                    row["Invoice Date"] = List_INVOICEDATE[i];
                    dt.Rows.Add(row);
                }
                GridView_BatteryOutstanding.DataSource = dt;
                GridView_BatteryOutstanding.DataBind();
                if (GridView_BatteryOutstanding.Rows.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ER_SF_28)
            {
                Function_Method.MsgBox("ER_SF_28: " + ER_SF_28.ToString(), this.Page, this);
                return false;
            }
        }
    }
}