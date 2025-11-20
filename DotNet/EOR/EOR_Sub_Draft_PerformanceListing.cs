using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class EOR : System.Web.UI.Page
    {
        //protected void CheckBox_div_Searchable(object sender, EventArgs e)
        //{
        //    if (CheckBox_div_Searchable_ID.Checked)
        //    {
        //        div_Searchable.Visible = true;
        //        CheckBox_div_Searchable_ID.Text = "Hide Search Bar";
        //    }
        //    else
        //    {
        //        div_Searchable.Visible = false;
        //        CheckBox_div_Searchable_ID.Text = "Show Search Bar";

        //    }

        //}

        protected void Button_Search_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {

            string fieldName = "";
            switch (DropDownList_Search.SelectedItem.Text)
            {
                case "Customer Account No.":
                    fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                    break;
                case "Customer Name":
                    fieldName = "CUSTNAME";//CUSTNAME
                    break;

                default:
                    fieldName = "";
                    break;
            }

            performance_listing(0, fieldName);
        }

        private void performance_listing(int PAGE_INDEX, string fieldName)
        {
            GridViewPerformanceListing.DataSource = null; GridViewPerformanceListing.DataBind();
            //
            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                int LF_EOR_MASTER = 30207;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_EOR_MASTER);

                string temp_SearchValue = "*" + TextBox_Search.Text.Trim() + "*";
                if (fieldName != "" && temp_SearchValue != "")
                {
                    if (fieldName == "CUSTACCOUNT")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30002);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                    if (fieldName == "CUSTNAME")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30001);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                }
                axQueryDataSource.Call("addSortField", 30001, 0);//CustName, ascending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 11;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Account"; N[2] = "Customer Name";
                N[3] = "Start Date"; N[4] = "Expiry Date"; N[5] = "Cur Act";
                N[6] = "Cur Tgt"; N[7] = "CTGTCTN"; N[8] = "CCNCTN";
                N[9] = "CORCTN"; N[10] = "Performance";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_EOR_MASTER);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();


                        row["No."] = countA;
                        row["Account"] = DynRec.get_Field("CUSTACCOUNT").ToString();
                        row["Customer Name"] = DynRec.get_Field("CUSTNAME").ToString();

                        string temp_StartDate = DynRec.get_Field("ComDate").ToString();
                        string[] arr_temp_StartDate = temp_StartDate.Split(' ');//date + " " + time;
                        string Raw_StartDate = arr_temp_StartDate[0];
                        row["Start Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_StartDate, true);

                        string temp_ExpDate = DynRec.get_Field("EXPDATE").ToString();
                        string[] arr_temp_ExpDate = temp_ExpDate.Split(' ');//date + " " + time;
                        string Raw_ExpDate = arr_temp_ExpDate[0];
                        row["Expiry Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_ExpDate, true);

                        double double_ACTCTN = Convert.ToDouble(DynRec.get_Field("ACTCTN").ToString());
                        row["Cur Act"] = double_ACTCTN.ToString("#,###,###,##0.00");
                        double double_TGTCTN = Convert.ToDouble(DynRec.get_Field("TGTCTN").ToString());
                        row["Cur Tgt"] = double_TGTCTN.ToString("#,###,###,##0.00");

                        string str_CTGTCTN = DynRec.get_Field("CTGTCTN").ToString();
                        double double_CTGTCTN = Convert.ToDouble(DynRec.get_Field("CTGTCTN").ToString());//A
                        double double_CCNCTN = Convert.ToDouble(DynRec.get_Field("CCNCTN").ToString());//B
                        double double_CORCTN = Convert.ToDouble(DynRec.get_Field("CORCTN").ToString());//C
                        row["CTGTCTN"] = double_CTGTCTN.ToString("#,###,###,##0.00");
                        row["CCNCTN"] = double_CCNCTN.ToString("#,###,###,##0.00");
                        row["CORCTN"] = double_CORCTN.ToString("#,###,###,##0.00");
                        //
                        string performance = "";
                        if (str_CTGTCTN != "" || double_CTGTCTN == 0)
                        {

                            //B+C
                            double B_plus_C = double_CCNCTN + double_CORCTN;
                            //Ax 100
                            double A_multiply_100 = double_CTGTCTN * 100;
                            //(B+C)/(A*100)
                            double double_performance = B_plus_C / A_multiply_100;
                            performance = double_performance.ToString("#,###,###,##0.00");
                        }

                        row["Performance"] = performance;

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
                GridViewPerformanceListing.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                GridViewPerformanceListing.DataSource = dt;
                GridViewPerformanceListing.DataBind();


            }
            catch (Exception ER_EO_04)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_04: " + ER_EO_04.ToString(), this.Page, this);

            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (TextBox_Search.Text == "")
                {
                    performance_listing(e.NewPageIndex, "");
                }
                else
                {
                    string fieldName = "";
                    switch (DropDownList_Search.SelectedItem.Text)
                    {

                        case "Customer Account No.":
                            fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                            break;
                        case "Customer Name":
                            fieldName = "CUSTNAME";//CUSTNAME
                            break;


                        default:
                            fieldName = "";
                            break;
                    }

                    performance_listing(e.NewPageIndex, fieldName);
                }

                GridViewPerformanceListing.PageIndex = e.NewPageIndex;
                GridViewPerformanceListing.DataBind();
            }
            catch (Exception ER_EO_05)
            {
                Function_Method.MsgBox("ER_EO_05: " + ER_EO_05.ToString(), this.Page, this);
            }
        }
    }
}