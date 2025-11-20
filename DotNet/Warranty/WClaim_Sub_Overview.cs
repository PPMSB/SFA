using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;

namespace DotNet
{
    public partial class WClaim : Page
    {
        protected void Button_ListAll_Click(object sender, EventArgs e)
        {
            f_Button_ListAll();
        }

        private void f_Button_ListAll()
        {
            GridViewOverviewList.PageIndex = 0;
            GridViewOverviewList.Columns[1].Visible = true;//ClaimNumber button
            GridViewOverviewList.Columns[2].Visible = false;//ClaimNumber label
            //GridViewOverviewList.Columns[2].Visible = false;//Equipment Id label
            //ddlStatus.SelectedItem.Value = Session["flag_temp"].ToString();

            GridViewOverviewList.Visible = true;

            WClaim_Overview(0, "");
            TextBox_Search_Overview.Text = "";
        }

        protected void Button_Search_Overview_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            switch (DropDownList_Search_Overview.SelectedItem.Text.Trim())
            {
                case "Claim Number":
                    fieldName = "ClaimNumber";
                    break;
                case "Customer Account No.":
                    fieldName = "CustAccount";//CUSTACCOUNT
                    break;
                case "Customer Name":
                    fieldName = "CustName";
                    break;
                default:
                    fieldName = "";
                    break;
            }

            WClaim_Overview(0, fieldName);
        }

        private void WClaim_Overview(int PAGE_INDEX, string fieldName)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Button_Overview_accordion.Visible = true;
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
            CheckAndSetVisibility(DynAx, "Draft", btnListDraft);
            CheckAndSetVisibility(DynAx, "Awaiting Invoice Chk", btnListInvoiceChk);
            CheckAndSetVisibility(DynAx, "Awaiting Transporter", btnListTransporter);
            CheckAndSetVisibility(DynAx, "Awaiting GoodsReceive", btnListGoodsReceived);
            CheckAndSetVisibility(DynAx, "Awaiting Inspection", btnListInspection);
            CheckAndSetVisibility(DynAx, "Awaiting Verified", btnListVerify);
            CheckAndSetVisibility(DynAx, "Awaiting Approved", btnListApproval);
            CheckAndSetVisibility(DynAx, "Approved", btnListApproved);
            CheckAndSetVisibility(DynAx, "Rejected", btnListReject);
            //CheckAndSetVisibility(DynAx, "Sales Order", btnListSalesOrder);

            
                // Jerry 2024-11-04 Avoid using harcode
                int LF_WarrantyTable = DynAx.GetTableIdWithLock("LF_WarrantyTable");
                //int LF_WarrantyTable = 30661; //live
                //int LF_WarrantyTable = 50773; //test
                //int claimStat = 30006;
                int claimStat = DynAx.GetFieldId(LF_WarrantyTable, "ClaimStatus");
                int claimNumber = DynAx.GetFieldId(LF_WarrantyTable, "ClaimNumber");
                int invoiceCheckedDate = DynAx.GetFieldId(LF_WarrantyTable, "InvoiceCheckedDate");
                string claimID = ""; string location = "";

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WarrantyTable);

                switch (Convert.ToInt16(ddlStatus.SelectedItem.Value))
                {
                    case 1:
                        var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat); // claimstatus 
                        qbr.Call("value", "Draft");
                        break;

                    case 2:
                        var reject = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        reject.Call("value", "Approved");
                        break;

                    case 3:
                        var pending = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending.Call("value", "Rejected");
                        break;

                    case 4:
                        var pending1 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending1.Call("value", "Awaiting Approved");
                        break;

                    case 5:
                        var pending2 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending2.Call("value", "Awaiting Transporter");
                        break;

                    case 6:
                        var pending3 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending3.Call("value", "Awaiting GoodsReceive");
                        break;

                    case 7:
                        var pending4 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending4.Call("value", "Awaiting Inspection");
                        break;

                    case 8:
                        var pending5 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending5.Call("value", "Awaiting Invoice Chk");
                        break;

                    case 9:
                        var pending6 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending6.Call("value", "Awaiting Verified");
                        break;

                    case 10:
                        var pending7 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending7.Call("value", "Sales Order");
                        break;

                    default:
                        var pending8 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                        pending8.Call("value", "!Approved,!Sales Order,!Rejected,!Draft,!Cancel");
                        break;
                }

                //Jerry 2024-11-04 Avoid hardcode
                //axQueryDataSource.Call("addSortField", "30001", 1); // warrantynum desc
                //axQueryDataSource.Call("addSortField", "30011", 1);// invoicecheckeddate desc
                axQueryDataSource.Call("addSortField", claimNumber, 1);
                axQueryDataSource.Call("addSortField", invoiceCheckedDate, 1);

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 18;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Claim Number"; N[2] = "Customer Account"; N[3] = "Customer Name";
                N[4] = "Type"; N[5] = "Claim Type"; N[6] = "Claim Status"; N[7] = "WH"; N[8] = "Next Approver"; N[9] = "Transport Required";
                N[10] = "Submitted Date"; N[11] = "Invoice Checked Date"; N[12] = "Transport Arrange Date"; N[13] = "Goods Received Date"; N[14] = "Inspection Date";
                N[15] = "Verified Date"; N[16] = "Approved Date"; N[17] = "CN Date";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WarrantyTable);
                    string temp_DraftCN = DynRec.get_Field("ClaimStatus").ToString();
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        string temp_CustAcc = DynRec.get_Field("CustAccount").ToString();
                        var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, temp_CustAcc);
                        string CustName = tuple_getCustInfo.Item1;
                        if (CustName != "")
                        {
                            claimID = DynRec.get_Field("ClaimNumber").ToString();

                            row["Claim Number"] = claimID;
                            row["Customer Account"] = temp_CustAcc;
                            row["Customer Name"] = CustName;
                            row["Type"] = DynRec.get_Field("ClaimType").ToString();
                            string subclaimtype = "";
                            string claimStatus = DynRec.get_Field("ClaimStatus").ToString();
                            if (DynRec.get_Field("ClaimType").ToString() == "Batch")
                            {
                                subclaimtype = "Batch";
                            }
                            else
                            {
                                if (DynRec.get_Field("SubClaimType").ToString() == "1")
                                {
                                    subclaimtype = "Battery";
                                }
                                else if (DynRec.get_Field("SubClaimType").ToString() == "2")
                                {
                                    subclaimtype = "Lubricant";
                                }
                                else
                                {
                                    subclaimtype = "Other Products";
                                }
                            }

                            row["Claim Type"] = subclaimtype;
                            row["Claim Status"] = claimStatus;

                            location = DynRec.get_Field("InventLocationID").ToString();
                            var nextPersonInCharge = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, subclaimtype, location);
                            string temp_Trans_Required = DynRec.get_Field("TransportRequired").ToString();

                            if (temp_Trans_Required == "1")//need transport
                            {
                                row["Transport Required"] = "Yes";
                                if (claimStatus == "Awaiting Transporter")
                                {
                                    row["Next Approver"] = nextPersonInCharge.Item1.ToString();
                                }
                            }
                            else
                            {
                                row["Transport Required"] = "No";
                            }
                            string createdDT = DynRec.get_Field("createdDateTime").ToString();
                            DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                            if (parsedDateTime.Hour >= 12)
                            {
                                parsedDateTime = parsedDateTime.AddHours(-12);
                                // Set "PM"
                                string amOrPm = "PM";
                                // If the resultTime is exactly 12:00 PM, it should stay "PM"
                                //if (parsedDateTime.Hour < 12)
                                //{
                                //    amOrPm = "AM";
                                //}
                                string formattedResultTime = parsedDateTime.ToString("dd/MM/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);
                                row["Submitted Date"] = formattedResultTime;
                            }
                            else
                            {
                                row["Submitted Date"] = parsedDateTime;
                            }

                            if (claimStatus == "Awaiting GoodsReceive")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item2.ToString();
                            }
                            if (claimStatus == "Awaiting Invoice Chk")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item3.ToString();
                            }
                            if (claimStatus == "Awaiting Inspection")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item4.ToString();
                            }
                            if (claimStatus == "Awaiting Verified")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item5.ToString();
                            }
                            if (claimStatus == "Awaiting Approved")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item6.ToString();
                            }
                            row["Transport Arrange Date"] = DynRec.get_Field("TransArrangeDate");
                            row["Goods Received Date"] = DynRec.get_Field("GoodsReceivedDate");
                            row["Invoice Checked Date"] = DynRec.get_Field("InvoiceCheckedDate");
                            row["Inspection Date"] = DynRec.get_Field("InspectionDate");
                            row["Verified Date"] = DynRec.get_Field("VerifiedDate").ToString();
                            row["Approved Date"] = DynRec.get_Field("ApprovedDate").ToString();
                            row["WH"] = DynRec.get_Field("InventLocationID").ToString();
                            row["CN Date"] = WClaim_GET_Report.get_rmaCreatedDate(DynAx, claimID);
                            dt.Rows.Add(row);
                            // Advance to the next row.
                            DynRec.Dispose();

                        }
                    }
                    //if (countA > endA)
                    //{
                    //    goto FINISH;//speed up process
                    //}
                }

                // Log off from Microsoft Dynamics AX.
                //FINISH:
                GridViewOverviewList.PagerSettings.Visible = true;

                GridViewOverviewList.VirtualItemCount = countA;

                GridViewOverviewList.DataSource = dt;
                //LblPendingBadge.Text = countA.ToString();
                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_WC_13)
            {
                string message = "ER_WC_13: " + ER_WC_13.ToString();
                // Check if the exception is of type NotLoggedOnException
                if (ER_WC_13 is Microsoft.Dynamics.BusinessConnectorNet.NotLoggedOnException)
                {
                    message = "Your connection to Axapta has been interrupted. Please attempt to log in again to proceed.";
                }
                Function_Method.MsgBox(message, this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void datagrid_PageIndexChanging_Overview(object sender, GridViewPageEventArgs e)
        {
            WClaim_Overview(e.NewPageIndex, "");

            GridViewOverviewList.PageIndex = e.NewPageIndex;
            GridViewOverviewList.DataBind();
        }

        private void WClaim_Search(int PAGE_INDEX, string fieldName)
        {
            GridViewOverviewList.DataSource = null;
            GridViewOverviewList.DataBind();

            Button_Overview_accordion.Visible = true;
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int LF_WarrantyTable = 30661; //live
                //int LF_WarrantyTable = 50773; //test
                int claimStat = 30006;
                string claimID = ""; string location = "";

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_WarrantyTable);
                var pending9 = (AxaptaObject)axQueryDataSource.Call("addRange", claimStat);
                pending9.Call("value", "!Draft");

                axQueryDataSource.Call("addSortField", "30001", 1); // warrantynum desc
                axQueryDataSource.Call("addSortField", "30011", 1);// invoicecheckeddate desc

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 18;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Claim Number"; N[2] = "Customer Account"; N[3] = "Customer Name";
                N[4] = "Type"; N[5] = "Claim Type"; N[6] = "Claim Status"; N[7] = "WH"; N[8] = "Next Approver"; N[9] = "Transport Required";
                N[10] = "Submitted Date"; N[11] = "Invoice Checked Date"; N[12] = "Transport Arrange Date"; N[13] = "Goods Received Date"; N[14] = "Inspection Date";
                N[15] = "Verified Date"; N[16] = "Approved Date"; N[17] = "CN Date";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;

                //===========================================
                // Loop through the set of retrieved records.

                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_WarrantyTable);
                    string temp_DraftCN = DynRec.get_Field("ClaimStatus").ToString();
                    countA = countA + 1;

                    row = dt.NewRow();

                    string temp_CustAcc = DynRec.get_Field("CustAccount").ToString();
                    var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, temp_CustAcc);
                    string CustName = tuple_getCustInfo.Item1;
                    if (CustName != "")
                    {
                        claimID = DynRec.get_Field("ClaimNumber").ToString();

                        row["Claim Number"] = claimID;
                        row["Customer Account"] = temp_CustAcc;
                        if (CustName.ToLower().Contains(TextBox_Search_Overview.Text.ToLower()) ||
                            TextBox_Search_Overview.Text == temp_CustAcc || TextBox_Search_Overview.Text == claimID)
                        {
                            row["Customer Name"] = CustName;
                            row["Type"] = DynRec.get_Field("ClaimType").ToString();
                            string subclaimtype = "";
                            string claimStatus = DynRec.get_Field("ClaimStatus").ToString();
                            if (DynRec.get_Field("ClaimType").ToString() == "Batch")
                            {
                                subclaimtype = "Batch";
                            }
                            else
                            {
                                if (DynRec.get_Field("SubClaimType").ToString() == "1")
                                {
                                    subclaimtype = "Battery";
                                }
                                else if (DynRec.get_Field("SubClaimType").ToString() == "2")
                                {
                                    subclaimtype = "Lubricant";
                                }
                                else
                                {
                                    subclaimtype = "Other Products";
                                }
                            }

                            row["Claim Type"] = subclaimtype;
                            row["Claim Status"] = claimStatus;

                            location = DynRec.get_Field("InventLocationID").ToString();
                            var nextPersonInCharge = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, subclaimtype, location);
                            string temp_Trans_Required = DynRec.get_Field("TransportRequired").ToString();

                            if (temp_Trans_Required == "1")//need transport
                            {
                                row["Transport Required"] = "Yes";
                                if (claimStatus == "Awaiting Transporter")
                                {
                                    row["Next Approver"] = nextPersonInCharge.Item1.ToString();
                                }
                            }
                            else
                            {
                                row["Transport Required"] = "No";
                            }
                            string createdDT = DynRec.get_Field("createdDateTime").ToString();
                            DateTime parsedDateTime = DateTime.Parse(createdDT).AddHours(8);
                            if (parsedDateTime.Hour >= 12)
                            {
                                parsedDateTime = parsedDateTime.AddHours(-12);
                                // Set "PM"
                                string amOrPm = "PM";
                                // If the resultTime is exactly 12:00 PM, it should stay "PM"
                                if (parsedDateTime.Hour == 0)
                                {
                                    amOrPm = "AM";
                                }
                                string formattedResultTime = parsedDateTime.ToString("dd/M/yyyy h:mm:ss tt").Replace("AM", amOrPm).Replace("PM", amOrPm);
                                row["Submitted Date"] = formattedResultTime;
                            }
                            else
                            {
                                row["Submitted Date"] = parsedDateTime;
                            }

                            if (claimStatus == "Awaiting GoodsReceive")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item2.ToString();
                            }
                            if (claimStatus == "Awaiting Invoice Chk")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item3.ToString();
                            }
                            if (claimStatus == "Awaiting Inspection")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item4.ToString();
                            }
                            if (claimStatus == "Awaiting Verified")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item5.ToString();
                            }
                            if (claimStatus == "Awaiting Approved")
                            {
                                row["Next Approver"] = nextPersonInCharge.Item6.ToString();
                            }
                            row["Transport Arrange Date"] = DynRec.get_Field("TransArrangeDate");
                            row["Goods Received Date"] = DynRec.get_Field("GoodsReceivedDate");
                            row["Invoice Checked Date"] = DynRec.get_Field("InvoiceCheckedDate");
                            row["Inspection Date"] = DynRec.get_Field("InspectionDate");
                            row["Verified Date"] = DynRec.get_Field("VerifiedDate").ToString();
                            row["Approved Date"] = DynRec.get_Field("ApprovedDate").ToString();
                            row["WH"] = DynRec.get_Field("InventLocationID").ToString();
                            row["CN Date"] = WClaim_GET_Report.get_rmaCreatedDate(DynAx, claimID);
                            dt.Rows.Add(row);
                            // Advance to the next row.
                            DynRec.Dispose();
                        }
                    }
                }

                GridViewOverviewList.VirtualItemCount = countA;

                GridViewOverviewList.PagerSettings.Visible = false;
                GridViewOverviewList.DataSource = dt;

                GridViewOverviewList.DataBind();
            }
            catch (Exception ER_WC_13)
            {
                string message = "ER_WC_13: " + ER_WC_13.ToString();
                // Check if the exception is of type NotLoggedOnException
                if (ER_WC_13 is Microsoft.Dynamics.BusinessConnectorNet.NotLoggedOnException)
                {
                    message = "Your connection to Axapta has been interrupted. Please attempt to log in again to proceed.";
                }
                Function_Method.MsgBox(message, this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        //end of Overview section
        protected void Button_ClaimId_Click(object sender, EventArgs e)
        {
            try
            {
                string selected_Id = "";
                Button Button_ClaimId = sender as Button;
                if (Button_ClaimId != null)
                {
                    selected_Id = Button_ClaimId.Text;
                    hidden_LabelClaimNumber.Text = selected_Id;
                    string ClientID = Button_ClaimId.ClientID;
                    string[] arr_ClientID = ClientID.Split('_');
                    int arr_count = arr_ClientID.Count();
                    //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                    GridViewRow row = (GridViewRow)Button_ClaimId.NamingContainer;
                    int index = row.RowIndex;
                    //
                    string CustomerAcc = GridViewOverviewList.Rows[index].Cells[3].Text;
                    hidden_inventLocationId.Text = GridViewOverviewList.Rows[index].Cells[8].Text;

                    string ClaimType = "";
                    if (GridViewOverviewList.Rows[index].Cells[5].Text == "Batch")
                    {
                        ClaimType = "1";
                    }
                    else
                    {
                        ClaimType = "2";
                    }

                    string subClaimType = "";
                    if (GridViewOverviewList.Rows[index].Cells[6].Text == "Battery")
                    {
                        subClaimType = "1";
                    }
                    else if (GridViewOverviewList.Rows[index].Cells[6].Text == "Lubricant")
                    {
                        subClaimType = "2";
                    }
                    else
                    {
                        subClaimType = "3";
                    }
                    string ClaimStatus = GridViewOverviewList.Rows[index].Cells[7].Text;
                    string warehouse = GridViewOverviewList.Rows[index].Cells[8].Text;
                    string TransRequired = GridViewOverviewList.Rows[index].Cells[10].Text;

                    Session["data_passing"] = "@WCWC_" + selected_Id + "|" + ClaimStatus + "|" + CustomerAcc + "|" + ClaimType + "|" +
                                                        subClaimType + "|" + TransRequired + "|" + warehouse;//WC>WC
                    Response.Redirect("WClaim.aspx", true);
                }
            }
            catch (Exception ER_WC_04)
            {
                Function_Method.MsgBox("ER_WC_04: " + ER_WC_04.ToString(), this.Page, this);
            }
        }

        private void getClaimImage()
        {
            int count = 0;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "select claim_num, salesman, customer_name, pictures from warranty_pic_tbl where claim_num=@c1";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            MySqlParameter _C1 = new MySqlParameter("@c1", MySqlDbType.VarChar, 0);
            _C1.Value = hidden_LabelClaimNumber.Text;
            cmd.Parameters.Add(_C1);

            conn.Open();

            List<ListItem> files = new List<ListItem>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    //if (!string.IsNullOrEmpty(reader.GetValue(3).ToString()))
                    //if (!reader.IsDBNull(3)) // Check if the image path is not null or empty
                    //{
                    //    rfvProductImg.Visible = false;
                    //    lblDisplay.Visible = true;
                    //    btnDisplay.Visible = true;
                    //    fileUpload.Visible = false;
                    //    upload_section.Visible = true;
                    //}

                    if (reader.GetValue(0).ToString() == hidden_LabelClaimNumber.Text)
                    {
                        string img1 = reader.GetValue(3).ToString();
                        string[] arr_pathSplit = img1.Split('/');

                        //var filePath = "http://10.1.1.167/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3] + "/" + arr_pathSplit[4];// for local testing
                        var filePath = GLOBAL.externalServerIP + "/Images/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/"
                            + arr_pathSplit[3] + "/" + arr_pathSplit[4]; //for external live
                        hdFilePath.Value = "_WAPDF@" + arr_pathSplit[0] + "/" + arr_pathSplit[1] + "/" + arr_pathSplit[2] + "/" + arr_pathSplit[3];
                        files.Add(new ListItem(arr_pathSplit[4], filePath));

                        string getFileExtension = Path.GetExtension(filePath);
                        upload_section.Visible = true;

                        if (getFileExtension == ".pdf")
                        {
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
                            else if (count == 2)
                            {
                                lnkBtn3.Text = arr_pathSplit[4];
                                pdf_section.Visible = true;
                            }
                            else
                            {
                                lnkBtn4.Text = arr_pathSplit[4];
                                pdf_section.Visible = true;
                            }
                        }

                        count++;
                    }
                }
            }

            if (count > 0) // At least one image is uploaded
            {
                rfvProductImg.Visible = false;
                lblDisplay.Visible = true;
                btnDisplay.Visible = true;
                fileUpload.Visible = false;
            }
            else
            {
                lblDisplay.Visible = false;
                btnDisplay.Visible = false;

                fileUpload.Visible = true;
                fileUpload2.Visible = true;
                fileUpload3.Visible = true;
                fileUpload4.Visible = true;
                lblimg.Visible = true;
                lblimg2.Visible = true;
                lblimg3.Visible = true;
                lblimg4.Visible = true;
                rfvProductImg.Visible = true;
                fileUpload.Visible = true;
            }

            repeater.DataSource = files;
            repeater.DataBind();
        }

        public bool PreProcess_Warranty_SaveSalesOrder(string itemId)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string CustAcc = Label_Account.Text.Trim();
                string ItemId = itemId;
                string SO_number = hdSoNumber.Value;
                string warehouse = "";
                //var tuple_get_Batch_Item = get_Batch_Item();//get item id
                //string[] SalesUnit = tuple_get_Batch_Item.Item7;
                var temp_Today = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //====================================================================
                string validateSubmitted = SFA_GET_SALES_ORDER_2.get_validateSubmitted(DynAx, SO_number);
                if (validateSubmitted == "1")//already submitted
                {
                    if (GLOBAL.user_authority_lvl != 1)//not system Admin
                    {
                        //check for HOD
                        var tupple_get_Empl_Id = SFA_GET_SALES_ORDER_2.get_Empl_Id(DynAx, GLOBAL.user_id);
                        string[] Empl_Id = tupple_get_Empl_Id.Item1;
                        int counter = tupple_get_Empl_Id.Item2;

                        int temp_flag_HOD = 0;
                        for (int j = 0; j < counter; j++)
                        {
                            if (Empl_Id[j].Substring(0, 1) == "H")
                            {//HOD
                                temp_flag_HOD = 1;
                            }
                        }

                        if (temp_flag_HOD == 0)
                        {
                            Function_Method.MsgBox("This warranty has been submitted and confirmed. Please contact admin for details.", this.Page, this);
                            return false;
                        }
                        else
                        {
                            Function_Method.MsgBox("User is HOD. Warranty claim can be resubmit by HOD.", this.Page, this);
                        }
                    }
                }
                AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

                warehouse = ddlWarehouse.SelectedItem.Text;

                string inventDimId = WClaim_GET_NewApplicant.getInventDimID_InventDimLocation(DynAx, warehouse);
                string inventDimId_old = domComSalesLine.Call("getInventDimId", ItemId, "", "", "", warehouse).ToString();

                if (inventDimId == "")
                {
                    Function_Method.MsgBox("Warranty claim unable to be save. Please check the details again.", this.Page, this);
                    return false;
                }
                else
                {
                    hdInventId.Value = inventDimId;
                }
                return true;
            }
            catch (Exception ER_WC_17)
            {
                Function_Method.MsgBox("ER_WC_17: " + ER_WC_17.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void Button_QueryClaimId_Click(object sender, EventArgs e)
        {
            string TransportArr1 = ""; string TransportArr2 = ""; string TransportArr3 = ""; string TransportArr4 = "";
            string GoodsRec1 = ""; string GoodsRec2 = ""; string GoodsRec3 = ""; string GoodsRec4 = "";
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                string selected_Id = "";
                Button Button_ClaimId = sender as Button;
                if (Button_ClaimId != null)
                {
                    selected_Id = Button_ClaimId.Text;
                    hidden_LabelClaimNumber.Text = selected_Id;
                    //
                    string ClientID = Button_ClaimId.ClientID;
                    string[] arr_ClientID = ClientID.Split('_');
                    int arr_count = arr_ClientID.Count();
                    //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                    GridViewRow row = (GridViewRow)Button_ClaimId.NamingContainer;
                    int index = row.RowIndex;
                    //
                    string CustomerAcc = gvQueryReport.Rows[index].Cells[3].Text;

                    string ClaimType = gvQueryReport.Rows[index].Cells[5].Text;
                    string ClaimStatus = gvQueryReport.Rows[index].Cells[6].Text;
                    string TransRequired = gvQueryReport.Rows[index].Cells[7].Text;

                    //====================================================================
                    string inventLocationId = WClaim_GET_NewApplicant.getWarehouse(DynAx, CustomerAcc);
                    var tuple_getWarrantyApprovalGroup = WClaim_GET_NewApplicant.getWarrantyApprovalGroup(DynAx, ClaimType, inventLocationId);
                    string TransportArr = tuple_getWarrantyApprovalGroup.Item1;
                    string GoodsRec = tuple_getWarrantyApprovalGroup.Item2;
                    string[] arr_TransportArr = TransportArr.Split('|');
                    string[] arr_GoodsRec = GoodsRec.Split('|');

                    if (!string.IsNullOrEmpty(TransportArr))
                    {
                        TransportArr1 = arr_TransportArr[0];
                        TransportArr2 = arr_TransportArr[1];
                        TransportArr3 = arr_TransportArr[2];
                        TransportArr4 = arr_TransportArr[3];
                    }

                    if (!string.IsNullOrEmpty(GoodsRec))
                    {
                        GoodsRec1 = arr_GoodsRec[0];
                        GoodsRec2 = arr_GoodsRec[1];
                        GoodsRec3 = arr_GoodsRec[2];
                        GoodsRec4 = arr_GoodsRec[3];
                    }

                    string next_approval_GoodsRec = "";
                    if (GoodsRec1 != "") next_approval_GoodsRec = WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec1);
                    if (GoodsRec2 != "") next_approval_GoodsRec = next_approval_GoodsRec + "/" + WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec2);
                    if (GoodsRec3 != "") next_approval_GoodsRec = next_approval_GoodsRec + "/" + WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec3);
                    if (GoodsRec4 != "") next_approval_GoodsRec = next_approval_GoodsRec + "/" + WClaim_GET_NewApplicant.CheckUserName(DynAx, GoodsRec4);

                    string next_approval_TransportArr = "";
                    if (TransportArr1 != "") next_approval_TransportArr = WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr1);
                    if (TransportArr2 != "") next_approval_TransportArr = next_approval_TransportArr + "/" + WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr2);
                    if (TransportArr3 != "") next_approval_TransportArr = next_approval_TransportArr + "/" + WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr3);
                    if (TransportArr4 != "") next_approval_TransportArr = next_approval_TransportArr + "/" + WClaim_GET_NewApplicant.CheckUserName(DynAx, TransportArr4);

                    //string user_id = GLOBAL.user_id;
                    //Session["data_passing"] = "@RPWC_" + selected_Id + "|" + ClaimStatus + "|" + CustomerAcc + "|" + ClaimType + "|" + TransRequired;//WC>WC
                    Session["data_passing"] = "@WCWC_" + selected_Id + "|" + ClaimStatus + "|" + CustomerAcc + "|" + ClaimType + "|" +
                                    "Batch" + "|" + TransRequired;//WC>WC
                    Response.Redirect("WClaim.aspx", false);
                }
            }
            catch (Exception ER_WC_06)
            {
                Function_Method.MsgBox("ER_WC_06: " + ER_WC_06.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void CheckAndSetVisibility(Axapta DynAx, string status, Button button)
        {
            var totalClaim = WClaim_GET_Report.getTotalClaimStatus(DynAx, status);
            string total = totalClaim.Item2.ToString();
            if (status == "Awaiting Invoice Chk")
            {
                if (total != "0")
                {
                    button.Text = "Invoice Check " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting Transporter")
            {
                if (total != "0")
                {
                    button.Text = "Transportation " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting GoodsReceive")
            {
                if (total != "0")
                {
                    button.Text = "Goods Receive " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting Inspection")
            {
                if (total != "0")
                {
                    button.Text = "Inspection " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting Verified")
            {
                if (total != "0")
                {
                    button.Text = "Verification " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Awaiting Approved")
            {
                if (total != "0")
                {
                    button.Text = "Approval " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Draft")
            {
                if (total != "0")
                {
                    button.Text = "Draft " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Approved")
            {
                if (total != "0")
                {
                    //button.Text = "Approved " + "(" + totalClaim.Item2.ToString() + ")";
                    //Keegan - Request Do hide the total ie comment in the program 30/9/2025
                    button.Text = "Approved";
                }
            }
            else if (status == "Rejected")
            {
                if (total != "0")
                {
                    button.Text = "Rejected " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
            else if (status == "Sales Order")
            {
                if (total != "0")
                {
                    button.Text = "Sales Order " + "(" + totalClaim.Item2.ToString() + ")";
                }
            }
        }

        private void getBatInspectionImage()
        {
            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                connection.Open();

                string query = "select claim_num, cust_acc, imgpath from warranty_bat_tbl where claim_num=@c1";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@c1", hidden_LabelClaimNumber.Text);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var externalIP = reader["imgpath"].ToString().Split('/');
                            ImgInspect.ImageUrl = GLOBAL.externalServerIP + "/Images/" + externalIP[1] + "/" + externalIP[2] + "/" + externalIP[3] + "/" + externalIP[4];
                        }
                    }

                    if (!string.IsNullOrEmpty(ImgInspect.ImageUrl))
                    {
                        btnInspectDisplay.Visible = true;
                        InspectFile.Visible = false;
                    }
                    else
                    {
                        btnInspectDisplay.Visible = false;
                        InspectFile.Visible = true;
                    }
                }
            }
        }

    }
}