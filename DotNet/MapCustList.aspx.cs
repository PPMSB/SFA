using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class MapCustList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Map_Overview();
        }

        private void Map_Overview()
        {
            AxaptaObject axQuery;
            AxaptaObject axQueryRun;
            AxaptaObject axQueryDataSource;
            Axapta DynAx = new Axapta();
            AxaptaRecord DynRec;

            string state = Session["data_passing"].ToString();
            string TableName = "CustTable";

            int data_count = 5;
            string[] F = new string[data_count];
            F[0] = "Name"; F[1] = "Address"; F[2] = "GPS"; F[3] = "CustomerMainGroup"; F[4] = "EmplId";

            string[] N = new string[data_count];
            N[0] = "Customer Name"; N[1] = "Address"; N[2] = "GPS Coordinate"; N[3] = "Customer Group"; N[4] = "Salesman ID";

            // Output variables for calls to AxRecord.get_Field
            object[] O = new object[data_count];
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int tableId = DynAx.GetTableId(TableName);

                axQuery = DynAx.CreateAxaptaObject("Query");
                axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                if (state != "-- SELECT --")
                {
                    var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", DynAx.GetFieldId(tableId, "State"));
                    qbr.Call("value", state);
                }

                var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);//CustGroup
                qbr4_2.Call("value", "TDI");

                var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
                qbr4_3.Call("value", "TDE");

                var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
                qbr4_4.Call("value", "TDO");

                axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                axQueryDataSource.Call("addSortField", 2, 0);//Customer Name, asc

                DataTable dt = CreateVendorDataTable(data_count, N);
                DataRow row;
                //===========================================
                int countA = 0;

                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                    countA = countA + 1;

                    row = dt.NewRow();
                    for (int i = 0; i < data_count; i++)
                    {
                        O[i] = DynRec.get_Field(F[i]);
                        row[N[i]] = O[i].ToString().Trim();
                    }

                    dt.Rows.Add(row);
                    DynRec.Dispose();
                }
                gv_Customer.VirtualItemCount = countA;

                gv_Customer.DataSource = dt;
                gv_Customer.DataBind();
            }
            catch (Exception ER_CM_00)
            {
                Function_Method.MsgBox("ER_CM_00: " + ER_CM_00.ToString(), this.Page, this);
            }
            DynAx.Dispose();

        }

        private DataTable CreateVendorDataTable(int data_count, string[] N)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("No", typeof(string)));
            for (int i = 0; i < data_count; i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }

            return dt;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        private void Export()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";

            string selectedState = Session["data_passing"].ToString();
            if (selectedState == "-- SELECT --")
            {
                selectedState = "All Customer";
            }
            string FileName = GLOBAL.logined_user_name + selectedState + ".xls";
            GridView gv = gv_Customer;

            using (StringWriter strwritter = new StringWriter())
            using (HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter))
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);

                gv.GridLines = GridLines.Both;
                gv.HeaderStyle.Font.Bold = true;
                gv.RenderControl(htmltextwrtter);

                Response.Write(strwritter.ToString());
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        protected void button_Click(object sender, EventArgs e)
        {
            Export();
        }
    }
}