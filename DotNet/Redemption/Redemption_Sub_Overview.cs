using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DotNet
{
    public partial class Redemption : System.Web.UI.Page
    {
        public void redemption_Overview(int PAGE_INDEX)
        {
            Gv_Overview.DataSource = null;
            Gv_Overview.DataBind();
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                string TableName = "LF_WebRedemp";
                int tableId = DynAx.GetTableId(TableName);
                int fieldId = DynAx.GetFieldId(tableId, "DocStatus");
                int Rdemt_ID = DynAx.GetFieldId(tableId, "Rdemt_ID");
                int NextApprover = DynAx.GetFieldId(tableId, "NextApprover");

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                switch (Convert.ToInt32(Session["flag_temp"]))
                {
                    //0=draft; 1=awaiting hod; 2=awaiting sales admin; 3=awaiting sales admin manager 4=awaiting gm
                    //5=approved 6=reject 7=awaiting operation manager
                    case 1:
                        var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr1.Call("value", "1");
                        break;
                    case 2:
                        var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr2.Call("value", "2");
                        break;
                    case 3:
                        var qbr3 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr3.Call("value", "3");
                        break;
                    case 4:
                        var qbr4 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr4.Call("value", "4");
                        break;
                    case 5:
                        var qbr5 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr5.Call("value", "5");
                        break;
                    case 6:
                        var qbr6 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr6.Call("value", "6");
                        break;
                    case 7:
                        var qbr7 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr7.Call("value", "7");
                        break;
                    case 8:
                        var qbr8 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr8.Call("value", "8");
                        break;
                    default:
                        var qbr9 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                        qbr9.Call("value", "!5,!6");
                        break;
                }

                // Jerry 2024-11-20 check if Session["hod"] exists before access
                //if (Session["hod"].ToString() != "")
                //{
                //    string hod = Session["hod"].ToString();
                //    var qbr10 = (AxaptaObject)axQueryDataSource.Call("addRange", NextApprover);//nextapprover
                //    qbr10.Call("value", hod);
                //}
                if (Session["hod"] != null)
                {
                    string hod = Session["hod"].ToString();
                    //Neil - To Solve filtering both normal spaces and non-breaking spaces
                    hod = hod.Replace('\u00A0', ' '); // convert non-breaking spaces to normal space
                    hod = hod.Replace(" ", "*");
                    // Find the index of the last opening parenthesis
                    int lastIndex = hod.LastIndexOf('(');
                    // If the parenthesis is found, remove everything from that index onward
                    if (lastIndex != -1)
                    {
                        hod = hod.Substring(0, lastIndex);
                    }
                    ///================
                    if (!string.IsNullOrEmpty(hod))
                    {
                        var qbr10 = (AxaptaObject)axQueryDataSource.Call("addRange", NextApprover);
                        qbr10.Call("value", "*" + hod + "*");
                    }
                }
                // Jerry 2024-11-20 End

                var qbr11 = (AxaptaObject)axQueryDataSource.Call("addRange", 40008);
                qbr11.Call("value", "*" + txtSearch.Text + "*");

                //Desmond 2024 2024-12-23 Add Search by HOD for Approved List, use by Sales Admin
                if ((Session["flag_temp"].ToString() == "5") && GLOBAL.user_id == "anis")
                {
                    lblSearch.Visible = true;
                    txtSearchHOD.Visible = true;
                    btnSearchHOD.Visible = true;
                }
                //Desmond 2024 2024-12-23 Add Search by HOD for Approved List, use by Sales Admin - END

                if (ddlSort.SelectedItem.Text == "Approved" || ddlSort.SelectedItem.Text == "Rejected" ||
                    Session["flag_temp"].ToString() != "5" || Session["flag_temp"].ToString() != "6")
                {
                    axQueryDataSource.Call("addSortField", Rdemt_ID, 1); // applieddate desc
                }
                else
                {
                    axQueryDataSource.Call("addSortField", Rdemt_ID, 0); // applieddate asc
                }

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 9; int startA = 0; int endA = 0;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Redemption ID"; N[2] = "Name"; N[3] = "Customer Account";
                N[4] = "Sales Rep"; N[5] = "Amount"; N[6] = "Approval"; N[7] = "Status"; N[8] = "Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                string name = ""; string emplID = "";
                string getHod = "";

                startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                    // Jerry 2025-02-06 Solve the issue where each page display limited number of records
                    //countA++;
                    //if (countA >= startA && countA <= endA)
                    //{
                    // Jerry 2025-02-06 Solve the issue where each page display limited number of records - END

                    string CustAcc = DynRec.get_Field("CustAccount").ToString();
                    var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                    name = tuple_getCustInfo.Item1.ToString();
                    row = dt.NewRow();

                    if (name.Contains("&amp;"))
                    {
                        name = name.Replace("&amp;", "");
                    }

                    // Jerry 2025-02-06 Solve the issue where each page display limited number of records
                    //string rdID = DynRec.get_Field("Rdemt_ID").ToString();
                        
                    //row["Redemption ID"] = rdID;

                    
                    //emplID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, CustAcc);
                    //getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, emplID);//salesapprovalgroupid

                    //var hodSplit = getHod.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                    //string redempAmt = DynRec.get_Field("Rdempt_Amt").ToString();
                    //var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(redempAmt));
                    // Jerry 2025-02-06 Solve the issue where each page display limited number of records - END 

                    if (name != "")
                    {
                        // Jerry 2025-02-06 Solve the issue where each page display limited number of records
                        countA++;

                        if (countA < startA) continue;
                        if (countA > endA) break;

                        string rdID = DynRec.get_Field("Rdemt_ID").ToString();

                        row["Redemption ID"] = rdID;


                        emplID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, CustAcc);
                        getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, emplID);//salesapprovalgroupid

                        var hodSplit = getHod.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                        string redempAmt = DynRec.get_Field("Rdempt_Amt").ToString();
                        var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(redempAmt));
                        // Jerry 2025-02-06 Solve the issue where each page display limited number of records - END

                        row["No."] = countA;
                        row["Name"] = name;
                        row["Customer Account"] = CustAcc;

                        row["Sales Rep"] = DynRec.get_Field("EmplName").ToString();
                        double redempAmount = Convert.ToDouble(DynRec.get_Field("Rdempt_Amt"));
                        row["Amount"] = redempAmount.ToString("#,###,###,##0.00");
                        int status = Convert.ToInt16(DynRec.get_Field("DocStatus"));
                        string hodLevel = DynRec.get_Field("HODLevel").ToString();
                        if (status == 1)
                        {
                            row["Status"] = "Awaiting HOD";
                        }
                        else if (status == 2)
                        {
                            row["Status"] = "Awaiting Sales Admin";
                        }
                        else if (status == 3)
                        {
                            row["Status"] = "Awaiting Sales Admin Manager";
                        }
                        else if (status == 4)
                        {
                            row["Status"] = "Awaiting General Manager";
                        }
                        else if (status == 5)
                        { row["Status"] = "Approved"; }
                        else if (status == 6)
                        { row["Status"] = "Rejected"; }
                        else if (status == 7)
                        {
                            row["Status"] = "Awaiting Operation Manager";
                        }
                        else
                        {
                            row["Status"] = "Awaiting Credit Control Manager";
                        }
                        string tmp_NextApprover = DynRec.get_Field("NextApprover").ToString();
                        row["Approval"] = tmp_NextApprover;
                        row["Date"] = Convert.ToDateTime(DynRec.get_Field("AppliedDate")).ToString("dd/MM/yyyy");

                        var getDataPassing = GLOBAL.data_passing.Split('_');
                        //if (tmp_NextApprover == getDataPassing[3])
                        //{
                        dt.Rows.Add(row);
                        //}

                    }
                    // Jerry 2025-02-06 Solve the issue where each page display limited number of records
                    //}

                    //if (countA > endA)
                    //{
                    //    goto FINISH;//speed up process
                    //}
                    // Jerry 2025-02-06 Solve the issue where each page display limited number of records - END


                }

            //FINISH:
                Gv_Overview.VirtualItemCount = countA;

                Gv_Overview.DataSource = dt;
                Gv_Overview.DataBind();
            }
            catch (Exception ER_RD_13)
            {
                Function_Method.MsgBox("ER_RD_13: " + ER_RD_13.Message, this.Page, this);
            }
            DynAx.Dispose();
        }

        protected void Gv_Overview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            redemption_Overview(e.NewPageIndex);
            Gv_Overview.PageIndex = e.NewPageIndex;
            Gv_Overview.DataBind();
        }

        protected void Gv_OverviewSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Jerry 2025-02-05 Fixed bug where page 2 show array index out of bound
            //redemption_OverviewTxtSearch(e.NewPageIndex, txtSearch.Text);
            //redemption_OverviewTxtSearch(e.NewPageIndex, txtSearchHOD.Text);
            redemption_OverviewTxtSearchHOD(e.NewPageIndex, txtSearchHOD.Text);
            Gv_OverviewSearch.PageIndex = e.NewPageIndex;
            Gv_OverviewSearch.DataBind();
            // Jerry 2025-02-05 Fixed bug where page 2 show array index out of bound - END
        }

        private void GetRedempIdImage()
        {
            int count = 0;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "select redemp_ID, salesman_name, customer_name, pictures from redemp_tbl where " +
                "redemp_ID=@c1";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            MySqlParameter _C1 = new MySqlParameter("@c1", MySqlDbType.VarChar, 0);
            _C1.Value = hdRedemID.Value;
            cmd.Parameters.Add(_C1);

            conn.Open();

            List<ListItem> files = new List<ListItem>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == hdRedemID.Value)
                    {
                        string img1 = reader.GetValue(3).ToString();
                        string[] arr_pathSplit = img1.Split('/');

                        //var filePath = "http://10.1.1.167/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3] + "/" + arr_pathSplit[4];// for local testing
                        var filePath = GLOBAL.externalServerIP + "/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/"
                            + arr_pathSplit[3] + "/" + arr_pathSplit[4]; //for external live
                        files.Add(new ListItem(arr_pathSplit[4], filePath));
                        hdFilePath.Value = "_RDPDF@" + arr_pathSplit[0] + "/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3];

                        string getFileExtension = Path.GetExtension(filePath);
                        upload_section.Visible = true;

                        if (getFileExtension == ".pdf")
                        {
                            btnDisplay.Visible = false;
                            lblDisplay.Visible = false;
                            lblDoc.Visible = false;

                            if (count == 0)
                            {
                                lnkBtn1.Text = arr_pathSplit[4];
                                pdf_section.Visible = true;
                            }
                            else if (count == 1)
                            {
                                lnkBtn2.Text = arr_pathSplit[4];
                                pdf_section.Visible = true;
                            }
                            else 
                            {
                                lnkBtn3.Text = arr_pathSplit[4];
                                pdf_section.Visible = true;
                            }
                        }
                        else
                        {
                            display_section.Visible = true;
                            btnDisplay.Visible = true;
                        }
                            upload_section.Visible = false;

                        count++;
                    }
                }
            }

            repeater.DataSource = files;
            repeater.DataBind();
        }

        protected void Gv_Overview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime dateApplied = DateTime.Parse(e.Row.Cells[8].Text);
                string status = e.Row.Cells[7].Text;
                DateTime now = DateTime.Now;
                int diff = (int)(now - dateApplied).TotalDays;
                if (status != "Approved" && status != "Rejected")
                {
                    if (diff > 3)
                    {
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Firebrick;
                    }
                }
            }
        }

        protected void Gv_OverviewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime dateApplied = DateTime.Parse(e.Row.Cells[8].Text);
                string status = e.Row.Cells[7].Text;
                DateTime now = DateTime.Now;
                int diff = (int)(now - dateApplied).TotalDays;
                if (status != "Approved" && status != "Reject")
                {
                    if (diff > 3)
                    {
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Firebrick;
                    }
                }
            }
        }

        protected void DisableCusDetail()
        {
            Button_CustomerMasterList.Visible = false;
            //Button_CheckAcc.Visible = false;
            TextBox_Account.Enabled = false;
            Textbox_TelNo.Enabled = false;
            Textbox_ContactPerson.Enabled = false;
            //txtGetInvoiceNo.Enabled = false;
        }

        protected void DisableItemPoint()
        {
            gvItemPoint.Enabled = false;
            txtPts.Enabled = false;
            txtGST.Enabled = false;
            BtnUpdateDoc.Enabled = false;
            //======================================
            ddlDelivery.Enabled = false;
            txtDeliveryAddr.Enabled = false;
            //ddlPayment.Enabled = false;
            txtRemark.Enabled = false;
            rblBeneficiaryName.Enabled = false;
            txtBeneficiaryName.Enabled = false;
            txtIc.Enabled = false;
            rblBank.Enabled = false;
        }

        protected void EnableItemPoint()
        {
            gvItemPoint.Enabled = true;
            txtPts.Enabled = true;
            txtGST.Enabled = true;
            BtnUpdateDoc.Enabled = true;
            //======================================
            ddlDelivery.Enabled = true;
            txtDeliveryAddr.Enabled = true;
            //ddlPayment.Enabled = true;
            txtRemark.Enabled = true;
            rblBeneficiaryName.Enabled = true;
            txtBeneficiaryName.Enabled = true;
            txtIc.Enabled = true;
            rblBank.Enabled = true;
        }

        public void redemption_OverviewTxtSearch(int PAGE_INDEX, string txtSearch)
        {
            Gv_OverviewSearch.DataSource = null;
            Gv_OverviewSearch.DataBind();
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string TableName = "LF_WebRedemp";
                int tableId = DynAx.GetTableId(TableName);
                int fieldId = DynAx.GetFieldId(tableId, "DocStatus");
                int Rdemt_ID = DynAx.GetFieldId(tableId, "Rdemt_ID");

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                var getCustAcc = Redemption_Get_Details.getCustInfo(DynAx, txtSearch);

                int currentYear = DateTime.Now.Year;
                DateTime lastYear = new DateTime(currentYear - 1, 1, 1);
                DateTime firstDayOfYear = new DateTime(currentYear, 1, 1);
                DateTime currentDt = DateTime.Now;

                if (txtSearch != "")
                {
                    var var_Qdate = lastYear.ToString("dd/MM/yyyy") + ".." + currentDt.ToString("dd/MM/yyyy");
                    var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 40045);//applied date
                    qbr.Call("value", var_Qdate);
                }

                if (ddlSort.SelectedItem.Text == "Approved" || ddlSort.SelectedItem.Text == "Rejected" ||
                    Session["flag_temp"].ToString() != "5" || Session["flag_temp"].ToString() != "6")
                {
                    axQueryDataSource.Call("addSortField", Rdemt_ID, 1); // applieddate desc
                }
                else
                {
                    axQueryDataSource.Call("addSortField", Rdemt_ID, 0); // applieddate asc
                }
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 9; int startA = 0; int endA = 0;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Redemption ID"; N[2] = "Name"; N[3] = "Customer Account";
                N[4] = "Sales Rep"; N[5] = "Amount"; N[6] = "Approval"; N[7] = "Status"; N[8] = "Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                string name = ""; string emplID = "";
                string getHod = "";

                startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);
                    countA = countA + 1;

                    row = dt.NewRow();

                    string rdID = DynRec.get_Field("Rdemt_ID").ToString();
                    row["Redemption ID"] = rdID;

                    string CustAcc = DynRec.get_Field("CustAccount").ToString();
                    var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                    name = tuple_getCustInfo.Item1.ToString();
                    if (name.ToLower().Contains(txtSearch) || rdID.ToLower() == txtSearch.ToLower() || CustAcc == txtSearch)
                    {
                        if (name.Contains("&amp;"))
                        {
                            name = name.Replace("&amp;", "");
                        }
                        emplID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, CustAcc);
                        getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, emplID);//salesapprovalgroupid

                        var hodSplit = getHod.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                        string redempAmt = DynRec.get_Field("Rdempt_Amt").ToString();
                        var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(redempAmt));

                        row["No."] = countA;
                        row["Name"] = name;
                        row["Customer Account"] = CustAcc;

                        row["Sales Rep"] = DynRec.get_Field("EmplName").ToString();
                        double redempAmount = Convert.ToDouble(DynRec.get_Field("Rdempt_Amt"));
                        row["Amount"] = redempAmount.ToString("#,###,###,##0.00");
                        int status = Convert.ToInt16(DynRec.get_Field("DocStatus"));
                        string temp_approvalName = DynRec.get_Field("NextApprover").ToString();

                        if (status == 1)
                        {
                            row["Status"] = "Awaiting HOD";
                        }
                        else if (status == 2)
                        {
                            row["Status"] = "Awaiting Sales Admin";
                        }
                        else if (status == 3)
                        {
                            row["Status"] = "Awaiting Sales Admin Manager";
                        }
                        else if (status == 4)
                        {
                            row["Status"] = "Awaiting General Manager";
                        }
                        else if (status == 5)
                        { row["Status"] = "Approved"; }
                        else if (status == 6)
                        { row["Status"] = "Rejected"; }
                        else
                        {
                            row["Status"] = "Awaiting Operation Manager";
                        }
                        row["Approval"] = temp_approvalName;

                        row["Date"] = Convert.ToDateTime(DynRec.get_Field("AppliedDate")).ToString("dd/MM/yyyy");

                        dt.Rows.Add(row);

                        // Advance to the next row.
                        DynRec.Dispose();

                    }
                }

                Gv_OverviewSearch.VirtualItemCount = countA;

                Gv_OverviewSearch.DataSource = dt;
                Gv_OverviewSearch.DataBind();
            }
            catch (Exception ER_RD_14)
            {
                Function_Method.MsgBox("ER_RD_14: " + ER_RD_14.ToString(), this.Page, this);
            }
            DynAx.Dispose();
        }

        //Desmond 2024 2024-12-23 Search by HOD for Approved redemption, use by Sales Admin
        public void redemption_OverviewTxtSearchHOD(int PAGE_INDEX, string txtSearch)
        {
            Gv_OverviewSearch.DataSource = null;
            Gv_OverviewSearch.DataBind();
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                string TableName = "LF_WebRedemp";
                int tableId = DynAx.GetTableId(TableName);
                int fieldId = DynAx.GetFieldId(tableId, "DocStatus");
                int Rdemt_ID = DynAx.GetFieldId(tableId, "Rdemt_ID");

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                var qbrApprove = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                qbrApprove.Call("value", "5");

                var getCustAcc = Redemption_Get_Details.getCustInfo(DynAx, txtSearch);

                int currentYear = DateTime.Now.Year;
                DateTime lastYear = new DateTime(currentYear - 1, 1, 1);
                DateTime firstDayOfYear = new DateTime(currentYear, 1, 1);
                DateTime currentDt = DateTime.Now;

                if (txtSearch != "")
                {
                    var var_Qdate = lastYear.ToString("dd/MM/yyyy") + ".." + currentDt.ToString("dd/MM/yyyy");
                    var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 40045);//applied date
                    qbr.Call("value", var_Qdate);
                }

                if (ddlSort.SelectedItem.Text == "Approved" || ddlSort.SelectedItem.Text == "Rejected" ||
                    Session["flag_temp"].ToString() != "5" || Session["flag_temp"].ToString() != "6")
                {
                    axQueryDataSource.Call("addSortField", Rdemt_ID, 1); // applieddate desc
                }
                else
                {
                    axQueryDataSource.Call("addSortField", Rdemt_ID, 0); // applieddate asc
                }
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 9; int startA = 0; int endA = 0;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Redemption ID"; N[2] = "Name"; N[3] = "Customer Account";
                N[4] = "Sales Rep"; N[5] = "Amount"; N[6] = "Approval"; N[7] = "Status"; N[8] = "Date";
                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                string name = ""; string emplID = "";
                string getHod = "";

                startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                endA = Function_Method.paging_grid(PAGE_INDEX)[1];

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                    // Jerry 2025-2-06 Display page 2 of HOD search result
                    //countA = countA + 1; 
                    //row = dt.NewRow();

                    //string rdID = DynRec.get_Field("Rdemt_ID").ToString();
                    //row["Redemption ID"] = rdID;

                    //string CustAcc = DynRec.get_Field("CustAccount").ToString();
                    //var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                    //name = tuple_getCustInfo.Item1.ToString();                    
                    // Jerry 2025-2-06 Display page 2 of HOD search result - END

                    string processStatus = DynRec.get_Field("ProcessStatus").ToString();

                    if (processStatus.ToLower().Contains(txtSearch))
                    {
                        // Jerry 2025-02-06 Display page 2 of HOD search result
                        countA++;
                        if (countA < startA)
                        {
                            DynRec.Dispose();
                            continue;
                        }
                        if (countA > endA)
                        {
                            DynRec.Dispose();
                            break;
                        }

                        row = dt.NewRow();

                        string rdID = DynRec.get_Field("Rdemt_ID").ToString();
                        row["Redemption ID"] = rdID;

                        string CustAcc = DynRec.get_Field("CustAccount").ToString();
                        var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                        name = tuple_getCustInfo.Item1.ToString();

                        // Jerry 2025-02-06 Display page 2 of HOD search result - END


                        if (name.Contains("&amp;"))
                        {
                            name = name.Replace("&amp;", "");
                        }
                        emplID = Payment_GET_JournalLine_SelectJournal_Transfer.get_emplid(DynAx, CustAcc);
                        getHod = EOR_GET_NewApplicant.get_NA_HODbyLevel(DynAx, emplID);//salesapprovalgroupid

                        var hodSplit = getHod.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                        string redempAmt = DynRec.get_Field("Rdempt_Amt").ToString();
                        var amount = Redemption_Get_Details.GetAmount(DynAx, Convert.ToDouble(redempAmt));

                        row["No."] = countA;
                        row["Name"] = name;
                        row["Customer Account"] = CustAcc;

                        row["Sales Rep"] = DynRec.get_Field("EmplName").ToString();
                        double redempAmount = Convert.ToDouble(DynRec.get_Field("Rdempt_Amt"));
                        row["Amount"] = redempAmount.ToString("#,###,###,##0.00");
                        int status = Convert.ToInt16(DynRec.get_Field("DocStatus"));
                        string temp_approvalName = DynRec.get_Field("NextApprover").ToString();

                        if (status == 1)
                        {
                            row["Status"] = "Awaiting HOD";
                        }
                        else if (status == 2)
                        {
                            row["Status"] = "Awaiting Sales Admin";
                        }
                        else if (status == 3)
                        {
                            row["Status"] = "Awaiting Sales Admin Manager";
                        }
                        else if (status == 4)
                        {
                            row["Status"] = "Awaiting General Manager";
                        }
                        else if (status == 5)
                        { row["Status"] = "Approved"; }
                        else if (status == 6)
                        { row["Status"] = "Rejected"; }
                        else
                        {
                            row["Status"] = "Awaiting Operation Manager";
                        }
                        row["Approval"] = temp_approvalName;

                        row["Date"] = Convert.ToDateTime(DynRec.get_Field("AppliedDate")).ToString("dd/MM/yyyy");

                        dt.Rows.Add(row);

                        // Advance to the next row.
                        DynRec.Dispose();

                    }
                }

                Gv_OverviewSearch.VirtualItemCount = countA;

                Gv_OverviewSearch.DataSource = dt;
                Gv_OverviewSearch.DataBind();
            }
            catch (Exception ER_RD_14)
            {
                Function_Method.MsgBox("ER_RD_14: " + ER_RD_14.ToString(), this.Page, this);
            }
            DynAx.Dispose();
        }
        //Desmond 2024 2024-12-23 Search by HOD for Approved redemption, use by Sales Admin - END

    }
}