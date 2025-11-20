using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {
        //start of OverviewSL section
        protected void Button_ListOutStanding_Click(object sender, EventArgs e)
        {
            f_Button_ListOutStanding();
        }

        private void f_Button_ListOutStanding()
        {
            Session["flag_temp"] = 0;//List Outstanding
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//Journal Number button
            GridViewOverviewList.Columns[2].Visible = false;//Journal Number label
            Button_ListOutStanding.Attributes.Add("style", "background-color:#f58345");
            Button_ListAll.Attributes.Add("style", "background-color:#transparent");
            Button_ListAutoSettlement.Attributes.Add("style", "background-color:#transparent"); upStatement.Visible = false;
            Overview(0, ""); Overview_section_general.Visible = true; Button_Overview_accordion.Text = "List Outstanding";
        }

        protected void Button_ListAll_Click(object sender, EventArgs e)
        {
            Session["flag_temp"] = 2;//List All 
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//Journal Number button
            GridViewOverviewList.Columns[2].Visible = false;//Journal Number label
            Button_ListAll.Attributes.Add("style", "background-color:#f58345");
            Button_ListOutStanding.Attributes.Add("style", "background-color:#transparent");
            Button_ListAutoSettlement.Attributes.Add("style", "background-color:#transparent");
            Overview(0, ""); Overview_section_general.Visible = true; Button_Overview_accordion.Text = "List All";
        }

        protected void Button_ListAutoSettlement_Click(object sender, EventArgs e)
        {
            Session["flag_temp"] = 1;//List AutoSettlement
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//Journal Number button
            GridViewOverviewList.Columns[2].Visible = false;//Journal Number label
            Button_ListAutoSettlement.Attributes.Add("style", "background-color:#f58345");
            Button_ListOutStanding.Attributes.Add("style", "background-color:#transparent");
            Button_ListAll.Attributes.Add("style", "background-color:#transparent");
            Overview(0, ""); Overview_section_general.Visible = true; Button_Overview_accordion.Text = "List Auto Settlement";
        }

        protected void Button_Journal_Number_Click(object sender, EventArgs e)
        {
            string selected_Journal_Number = "";
            Button Button_Journal_Number = sender as Button;
            if (Button_Journal_Number != null)
            {
                selected_Journal_Number = Button_Journal_Number.Text;

                Axapta DynAx = new Axapta();
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                try
                {
                    Session["data_passing"] = "@PAPA_" + selected_Journal_Number;//Payment > Payment
                    Response.Redirect("Payment.aspx");
                }
                catch (Exception ER_PA_07)
                {
                    Function_Method.MsgBox("ER_PA_07: " + ER_PA_07.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Logoff();
                }
            }
        }

        private void Overview(int PAGE_INDEX, string JournalNum)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int LEDGERJOURNALTABLE = 211;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LEDGERJOURNALTABLE);

                switch (Convert.ToInt32(Session["flag_temp"]))
                {
                    case 0:// List Outstanding
                        var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 5);//JOURNALTYPE
                        qbr_0_1.Call("value", "7");//Only show Journal of type [Customer Payment]

                        var qbr_0_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 18);//POSTED
                        qbr_0_2.Call("value", "0");//Field: Posted = 18, Enum 0 = Not Posted

                        break;

                    case 1:// List AutoSettlement
                        var qbr_1_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 5);//JOURNALTYPE
                        qbr_1_1.Call("value", "7");//Only show Journal of type [Customer Payment]

                        var qbr_1_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 18);//POSTED
                        qbr_1_2.Call("value", "0");//Field: Posted = 18, Enum 0 = Not Posted

                        var qbr_1_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 17);//JOURNALNAME
                        qbr_1_3.Call("value", "c*");//Field: Posted = 18, Enum 0 = Not Posted
                        break;

                    case 2:// List All
                        var qbr_2_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 5);//JOURNALTYPE
                        qbr_2_1.Call("value", "7");//Only show Journal of type [Customer Payment]
                        break;
                    default://List Outstanding
                        var qbr_3_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 5);//JOURNALTYPE
                        qbr_3_1.Call("value", "7");//Only show Journal of type [Customer Payment]

                        var qbr_2_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 18);//POSTED
                        qbr_2_2.Call("value", "0");//Field: Posted = 18, Enum 0 = Not Posted
                        break;
                }
                if (JournalNum != "")
                {
                    var qbr_2_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//Journal Number
                    qbr_2_3.Call("value", "*"+JournalNum);
                }

                axQueryDataSource.Call("addSortField", 1, 1);//Journal Num, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================

                DataTable dt = new DataTable();
                int data_count = 8;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Journal Number"; N[2] = "Name";
                N[3] = "Posted"; N[4] = "Post Dated Chq."; N[5] = "CC Received";
                N[6] = "Description"; N[7] = "Customer";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LEDGERJOURNALTABLE);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string temp_JournalNum = DynRec.get_Field("JournalNum").ToString();
                        row["Journal Number"] = temp_JournalNum;
                        row["Name"] = DynRec.get_Field("JournalName").ToString();

                        string temp_Posted = DynRec.get_Field("Posted").ToString();
                        if (temp_Posted == "1")
                        {
                            row["Posted"] = "Yes";
                        }
                        else
                        {
                            row["Posted"] = "No";
                        }

                        string temp_PostDateChq = DynRec.get_Field("PostDateChq").ToString();
                        if (temp_PostDateChq == "1")
                        {
                            row["Post Dated Chq."] = "Yes";
                        }
                        else
                        {
                            row["Post Dated Chq."] = "No";
                        }

                        string temp_CCReceived = DynRec.get_Field("CCReceived").ToString();
                        if (temp_CCReceived == "1")
                        {
                            row["CC Received"] = "Yes";
                        }
                        else
                        {
                            row["CC Received"] = "No";
                        }

                        row["Description"] = DynRec.get_Field("Name").ToString();

                        row["Customer"] = Payment_GET_Overview.Customer(DynAx, temp_JournalNum);

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridViewOverviewList.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                GridViewOverviewList.DataSource = dt;
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_PA_09)
            {
                Function_Method.MsgBox("ER_PA_09: " + ER_PA_09.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Overview(e.NewPageIndex, "");
            GridViewOverviewList.PageIndex = e.NewPageIndex;
            GridViewOverviewList.DataBind();
        }

        protected void TextBox_Search_Overview_TextChanged(object sender, EventArgs e)
        {
            if (TextBox_Search_Overview.Text != "")
            {
                Overview(0, TextBox_Search_Overview.Text);
            }
            else
            {
                Overview(0, "");
            }
        }
        //end of OverviewSL section
    }
}