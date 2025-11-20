using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class ApprovalList : System.Web.UI.Page
    {
        //Axapta DynAx = Function_Method.GlobalAxapta();

        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                //TextBox_Account.Attributes.Add("onkeypress", "return controlEnter('" + Button1.ClientID + "', event)");
                Function_Method.LoadSelectionMenu(GLOBAL.module_access_authority,
                    null, CustomerMasterTag2,
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
                    null, null, null, null,
                    NewProduct2
                    );
                Check_DataRequest();
                DropDownList();
                DropDownList_Search();
                //first time, reload gridview with Search ALL approach
                OverviewApproval();
            }
        }

        private void Check_DataRequest()
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    if ((temp1.Substring(0, 6) == "_SFCM@") || (temp1.Substring(0, 6) == "_PACM@") || (temp1.Substring(0, 6) == "_EOCM@") || (temp1.Substring(0, 6) == "_PAC2@") || (temp1.Substring(0, 6) == "_WCCM@"))
                    {
                        gvApproval.Columns[1].Visible = true;//Customer Acc button
                        gvApproval.Columns[2].Visible = false;//Customer Acc label

                        if (temp1.Substring(0, 6) == "_PAC2@")//if triggered from invoice hide Salesman Id (request by Keegan)
                        {
                            gvApproval.Columns[6].Visible = false;//Employee ID
                        }
                        else
                        {
                            gvApproval.Columns[6].Visible = true;//Employee ID
                        }
                    }
                    else
                    {//default
                        gvApproval.Columns[1].Visible = false;//Customer Acc button
                        gvApproval.Columns[2].Visible = true;//Customer Acc label
                    }
                }
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
                Session.Clear();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void clear_variable()
        {
            ddlWarehouse.SelectedItem.Value = "0";
            ddlWarranty.ClearSelection();
            ddlTransport1.ClearSelection();
            ddlTransport2.ClearSelection();
            ddlTransport3.ClearSelection();
            ddlTransport4.ClearSelection();

            ddlGoodReceive1.ClearSelection();
            ddlGoodReceive2.ClearSelection();
            ddlGoodReceive3.ClearSelection();
            ddlGoodReceive4.ClearSelection();

            ddlInvoiceCheck1.ClearSelection();
            ddlInvoiceCheck2.ClearSelection();
            ddlInvoiceCheck3.ClearSelection();
            ddlInvoiceCheck4.ClearSelection();

            ddlInspector1.ClearSelection();
            ddlInspector2.ClearSelection();
            ddlInspector3.ClearSelection();
            ddlInspector4.ClearSelection();

            ddlVerifier1.ClearSelection();
            ddlVerifier2.ClearSelection();
            ddlVerifier3.ClearSelection();
            ddlVerifier4.ClearSelection();

            ddlApprover1.ClearSelection();
            ddlApprover2.ClearSelection();
            ddlApprover3.ClearSelection();
            ddlApprover4.ClearSelection();
        }

        private void OverviewApproval()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                string tablename = "LF_Warranty_AppGrp";
                int tableid = DynAx.GetTableId(tablename);
                int fieldid = DynAx.GetFieldId(tableid, "RECID");
                //int LF_Warranty_AppGrp = 30659; // live
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableid);

                if (ddlWarehouseSearch.Text != "-- SELECT --")
                {
                    int LocationId = DynAx.GetFieldId(tableid, "InventLocationID");
                    var qbr1_2 = (AxaptaObject)axQueryDataSource.Call("addRange", LocationId);//InventLocationId
                    qbr1_2.Call("value", ddlWarehouseSearch.Text);
                }

                axQueryDataSource.Call("addSortField", fieldid, 1);//RECID, must desc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "chkSelect"; N[1] = "No."; N[2] = "Edit"; N[3] = "Location"; N[4] = "WarrantyType"; N[5] = "InvCheck"; N[6] = "Transportation";
                N[7] = "GoodsReceive"; N[8] = "Inspection"; N[9] = "Verification"; N[10] = "Approval"; N[11] = "hdRecId";
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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableid);

                    countA = countA + 1;
                    if (countA > 0)
                    {
                        row = dt.NewRow();
                        row["No."] = countA;
                        row["Location"] = DynRec.get_Field("InventLocationID").ToString();
                        row["WarrantyType"] = DynRec.get_Field("ClaimType").ToString();

                        row["Transportation"] = DynRec.get_Field("TransportArr1").ToString() + "<br>" + DynRec.get_Field("TransportArr2").ToString()
                            + "<br>" + DynRec.get_Field("TransportArr3").ToString() + "<br>" + DynRec.get_Field("TransportArr4").ToString();

                        row["GoodsReceive"] = DynRec.get_Field("GoodsRec1").ToString() + "<br>" + DynRec.get_Field("GoodsRec2").ToString()
                            + "<br>" + DynRec.get_Field("GoodsRec3").ToString() + "<br>" + DynRec.get_Field("GoodsRec4").ToString();

                        row["InvCheck"] = DynRec.get_Field("InvChecked1").ToString() + "<br>" + DynRec.get_Field("InvChecked2").ToString()
                            + "<br>" + DynRec.get_Field("InvChecked3").ToString() + "<br>" + DynRec.get_Field("InvChecked4").ToString();

                        row["Inspection"] = DynRec.get_Field("Inspector1").ToString() + "<br>" + DynRec.get_Field("Inspector2").ToString()
                             + "<br>" + DynRec.get_Field("Inspector3").ToString() + "<br>" + DynRec.get_Field("Inspector4").ToString();

                        row["Verification"] = DynRec.get_Field("Verifier1").ToString() + "<br>" + DynRec.get_Field("Verifier2").ToString()
                             + "<br>" + DynRec.get_Field("Verifier3").ToString() + "<br>" + DynRec.get_Field("Verifier4").ToString();

                        row["Approval"] = DynRec.get_Field("Approver1").ToString() + "<br>" + DynRec.get_Field("Approver2").ToString()
                             + "<br>" + DynRec.get_Field("Approver3").ToString() + "<br>" + DynRec.get_Field("Approver4").ToString();

                        row["hdRecId"] = DynRec.get_Field("RecId").ToString();

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                }

                //Data-Binding with our GRID

                gvApproval.DataSource = dt;
                gvApproval.DataBind();
                gvApproval.Columns[11].Visible = false;
            }
            catch (Exception ER_AP_11)
            {
                Function_Method.MsgBox("ER_AP_11: " + ER_AP_11.ToString(), this.Page, this);
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApproval.DataKeyNames = new string[] { "hdRecId" };
            clear_variable();
            OverviewApproval();
            gvApproval.PageIndex = e.NewPageIndex;
            gvApproval.DataBind();
        }

        protected void Button_Account_Click(object sender, EventArgs e)
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    //string test = temp1.Substring(0, 6);
                    if (temp1.Substring(0, 6) == "_SFCM@")//Request from SFA > CustomerMaster
                    {
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@SFCM_" + selected_Account;
                        Response.Redirect("SFA.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_PACM@")//Request from Payment > CustomerMaster
                    {
                        string Journal_Num = temp1.Substring(6);//_PACM@ + 8 digit Journal Num

                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@PACM_" + Journal_Num + "|" + selected_Account;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_EOCM@")//Request from EOR > CustomerMaster
                    {
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@EOCM_" + selected_Account;
                        Response.Redirect("EOR.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_PAC2@")//Request from Payment > CustomerMaster
                    {
                        string info = temp1.Substring(6);//_PACM@
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@PAC2_" + info + "|" + selected_Account;
                        Response.Redirect("Payment.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_WCCM@")//Request from WClaim > CustomerMaster
                    {
                        string selected_Account = "";
                        Button ButtonAccount = sender as Button;
                        if (ButtonAccount != null)
                        {
                            selected_Account = ButtonAccount.Text;
                        }
                        Session["data_passing"] = "@WCCM_" + selected_Account;
                        Response.Redirect("WClaim.aspx");
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            clear_variable();
            HideListView();

            //ddlWarehouse.Items.AddRange(WClaim_GET_NewApplicant.get_Warehouse(DynAx).ToArray());
            btnSaveGroup.Text = "Save";
            btnSaveGroup.Visible = true;
            btnDelete.Visible = false;
            btnUpd.Visible = false;
        }

        protected void gvApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edt")
            {
                HideListView();

                LinkButton lnkBtn = (LinkButton)e.CommandSource;
                GridViewRow myRow = (GridViewRow)lnkBtn.NamingContainer;
                Axapta DynAx = Function_Method.GlobalAxapta();

                int rowIndex = myRow.RowIndex; // This gives you the row index on the current page

                GridViewRow row = gvApproval.Rows[rowIndex];

                string[] delim = { "<br>" };

                ddlWarehouse.SelectedItem.Text = row.Cells[3].Text;

                if (row.Cells[4].Text == "Batch Return")
                {
                    ddlWarranty.SelectedValue = "1";
                }
                else if (row.Cells[4].Text == "Battery")
                {
                    ddlWarranty.SelectedValue = "2";
                }
                else if (row.Cells[4].Text == "Other Products")
                {
                    ddlWarranty.SelectedValue = "3";
                }
                else
                {
                    ddlWarranty.SelectedValue = "4";
                }

                List<DropDownList> dropdownLists = new List<DropDownList>()
                {
                ddlInvoiceCheck1, ddlInvoiceCheck2, ddlInvoiceCheck3, ddlInvoiceCheck4,
                ddlTransport1, ddlTransport2, ddlTransport3, ddlTransport4,
                ddlGoodReceive1, ddlGoodReceive2, ddlGoodReceive3, ddlGoodReceive4,
                ddlInspector1, ddlInspector2, ddlInspector3, ddlInspector4,
                ddlVerifier1, ddlVerifier2, ddlVerifier3, ddlVerifier4,
                ddlApprover1, ddlApprover2, ddlApprover3, ddlApprover4
                };

                AddDefaultSelectItemToDropDowns(dropdownLists);

                var getApproval1 = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, row.Cells[4].Text, row.Cells[3].Text);
                var getApproval2 = WClaim_GET_NewApplicant.getWarrantyApprovalUser2(DynAx, row.Cells[4].Text, row.Cells[3].Text);
                var getApproval3 = WClaim_GET_NewApplicant.getWarrantyApprovalUser3(DynAx, row.Cells[4].Text, row.Cells[3].Text);

                SetSelectedValue(ddlTransport1, getApproval1.Item1);
                SetSelectedValue(ddlTransport2, getApproval2.Item1);
                SetSelectedValue(ddlTransport3, getApproval2.Item2);
                SetSelectedValue(ddlTransport4, getApproval2.Item3);

                SetSelectedValue(ddlGoodReceive1, getApproval1.Item2);
                SetSelectedValue(ddlGoodReceive2, getApproval2.Item4);
                SetSelectedValue(ddlGoodReceive3, getApproval2.Item5);
                SetSelectedValue(ddlGoodReceive4, getApproval2.Item6);

                SetSelectedValue(ddlInvoiceCheck1, getApproval1.Item3);
                SetSelectedValue(ddlInvoiceCheck2, getApproval3.Item1);
                SetSelectedValue(ddlInvoiceCheck3, getApproval3.Item2);
                SetSelectedValue(ddlInvoiceCheck4, getApproval3.Item3);

                SetSelectedValue(ddlInspector1, getApproval1.Item4);
                SetSelectedValue(ddlInspector2, getApproval3.Item4);
                SetSelectedValue(ddlInspector3, getApproval3.Item5);
                SetSelectedValue(ddlInspector4, getApproval3.Item6);

                SetSelectedValue(ddlVerifier1, getApproval1.Item5);
                SetSelectedValue(ddlVerifier2, getApproval3.Item7);
                SetSelectedValue(ddlVerifier3, getApproval3.Rest.Item1);
                SetSelectedValue(ddlVerifier4, getApproval3.Rest.Item2);

                SetSelectedValue(ddlApprover1, getApproval1.Item6);
                SetSelectedValue(ddlApprover2, getApproval1.Rest.Item1);
                SetSelectedValue(ddlApprover3, getApproval1.Rest.Item2);
                SetSelectedValue(ddlApprover4, getApproval1.Rest.Item3);

                //hdRecId.Value = row.Cells[11].Text;
                hdRecId.Value = gvApproval.DataKeys[rowIndex]["hdRecId"].ToString();
            }
        }

        protected void btnUpd_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Warranty_AppGrp"))
                {
                    DynAx.TTSBegin();
                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", hdRecId.Value));

                    if (DynRec.Found)
                    {
                        // Define an array of dropdown lists and their corresponding field names
                        Dictionary<DropDownList, string> dropdownFields = new Dictionary<DropDownList, string>()
                        {
                            { ddlWarehouse, "InventLocationID" },
                            { ddlWarranty, "ClaimType" },
                            { ddlInvoiceCheck1, "InvChecked1" },
                            { ddlInvoiceCheck2, "InvChecked2" },
                            { ddlInvoiceCheck3, "InvChecked3" },
                            { ddlInvoiceCheck4, "InvChecked4" },
                            { ddlTransport1, "TransportArr1" },
                            { ddlTransport2, "TransportArr2" },
                            { ddlTransport3, "TransportArr3" },
                            { ddlTransport4, "TransportArr4" },
                            { ddlGoodReceive1, "GoodsRec1" },
                            { ddlGoodReceive2, "GoodsRec2" },
                            { ddlGoodReceive3, "GoodsRec3" },
                            { ddlGoodReceive4, "GoodsRec4" },
                            { ddlInspector1, "Inspector1" },
                            { ddlInspector2, "Inspector2" },
                            { ddlInspector3, "Inspector3" },
                            { ddlInspector4, "Inspector4" },
                            { ddlVerifier1, "Verifier1" },
                            { ddlVerifier2, "Verifier2" },
                            { ddlVerifier3, "Verifier3" },
                            { ddlVerifier4, "Verifier4" },
                            { ddlApprover1, "Approver1" },
                            { ddlApprover2, "Approver2" },
                            { ddlApprover3, "Approver3" },
                            { ddlApprover4, "Approver4" }
                        };

                        // Iterate through each dropdown list and its corresponding field
                        foreach (var pair in dropdownFields)
                        {
                            DropDownList ddl = pair.Key;
                            string fieldName = pair.Value;

                            // Check if the selected value is "0"
                            if (ddl.SelectedItem.Text == "-- SELECT --")
                            {
                                // Set the dropdown list with an empty string
                                ddl.SelectedItem.Text = "";
                            }

                            // Set the field in the DynRec object with the selected item's text
                            DynRec.set_Field(fieldName, ddl.SelectedItem.Text);
                            DynRec.Call("update");
                        }
                    }
                    // Log the successful update
                    logger.Info($"Record updated successfully for RecId: {hdRecId.Value}");

                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
                //Response.Redirect("ApprovalList.aspx", true);
                ShowListview();
                int currentPageIndex = gvApproval.PageIndex;
                gvApproval.PageIndex = currentPageIndex;
                gvApproval.DataBind();
                OverviewApproval();
            }
            catch (Exception ER_AP_19)
            {
                DynAx.TTSAbort();
                logger.Error($"Error updating record for RecId: {hdRecId.Value}. Exception: {ER_AP_19}");
                Function_Method.MsgBox("ER_AP_19: " + ER_AP_19.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int currentPageIndex = gvApproval.PageIndex;
            gvApproval.PageIndex = currentPageIndex;
            gvApproval.DataBind();
            OverviewApproval();

            ShowListview();
        }

        protected void btnSaveGroup_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Warranty_AppGrp"))
                {
                    DynAx.TTSBegin();

                    if (ddlWarehouse.SelectedValue != "")
                    {
                        DynRec.set_Field("InventLocationID", ddlWarehouse.SelectedItem.Text);
                        DynRec.set_Field("ClaimType", ddlWarranty.SelectedItem.Text);

                        if (ddlInvoiceCheck1.SelectedIndex != 0) DynRec.set_Field("InvChecked1", ddlInvoiceCheck1.SelectedItem.Text);
                        if (ddlInvoiceCheck2.SelectedIndex != 0) DynRec.set_Field("InvChecked2", ddlInvoiceCheck2.SelectedItem.Text);
                        if (ddlInvoiceCheck3.SelectedIndex != 0) DynRec.set_Field("InvChecked3", ddlInvoiceCheck3.SelectedItem.Text);
                        if (ddlInvoiceCheck4.SelectedIndex != 0) DynRec.set_Field("InvChecked4", ddlInvoiceCheck4.SelectedItem.Text);

                        if (ddlTransport1.SelectedIndex != 0) DynRec.set_Field("TransportArr1", ddlTransport1.SelectedItem.Text);
                        if (ddlTransport2.SelectedIndex != 0) DynRec.set_Field("TransportArr2", ddlTransport2.SelectedItem.Text);
                        if (ddlTransport3.SelectedIndex != 0) DynRec.set_Field("TransportArr3", ddlTransport3.SelectedItem.Text);
                        if (ddlTransport4.SelectedIndex != 0) DynRec.set_Field("TransportArr4", ddlTransport4.SelectedItem.Text);

                        if (ddlGoodReceive1.SelectedIndex != 0) DynRec.set_Field("GoodsRec1", ddlGoodReceive1.SelectedItem.Text);
                        if (ddlGoodReceive2.SelectedIndex != 0) DynRec.set_Field("GoodsRec2", ddlGoodReceive2.SelectedItem.Text);
                        if (ddlGoodReceive3.SelectedIndex != 0) DynRec.set_Field("GoodsRec3", ddlGoodReceive3.SelectedItem.Text);
                        if (ddlGoodReceive4.SelectedIndex != 0) DynRec.set_Field("GoodsRec4", ddlGoodReceive4.SelectedItem.Text);

                        if (ddlInspector1.SelectedIndex != 0) DynRec.set_Field("Inspector1", ddlInspector1.SelectedItem.Text);
                        if (ddlInspector2.SelectedIndex != 0) DynRec.set_Field("Inspector2", ddlInspector2.SelectedItem.Text);
                        if (ddlInspector3.SelectedIndex != 0) DynRec.set_Field("Inspector3", ddlInspector3.SelectedItem.Text);
                        if (ddlInspector4.SelectedIndex != 0) DynRec.set_Field("Inspector4", ddlInspector4.SelectedItem.Text);

                        if (ddlVerifier1.SelectedIndex != 0) DynRec.set_Field("Verifier1", ddlVerifier1.SelectedItem.Text);
                        if (ddlVerifier2.SelectedIndex != 0) DynRec.set_Field("Verifier2", ddlVerifier2.SelectedItem.Text);
                        if (ddlVerifier3.SelectedIndex != 0) DynRec.set_Field("Verifier3", ddlVerifier3.SelectedItem.Text);
                        if (ddlVerifier4.SelectedIndex != 0) DynRec.set_Field("Verifier4", ddlVerifier4.SelectedItem.Text);

                        if (ddlApprover1.SelectedIndex != 0) DynRec.set_Field("Approver1", ddlApprover1.SelectedItem.Text);
                        if (ddlApprover2.SelectedIndex != 0) DynRec.set_Field("Approver2", ddlApprover2.SelectedItem.Text);
                        if (ddlApprover3.SelectedIndex != 0) DynRec.set_Field("Approver3", ddlApprover3.SelectedItem.Text);
                        if (ddlApprover4.SelectedIndex != 0) DynRec.set_Field("Approver4", ddlApprover4.SelectedItem.Text);

                        DynRec.Call("insert");
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
                //Response.Redirect("ApprovalList.aspx", true);
                ShowListview();
                int currentPageIndex = gvApproval.PageIndex;
                gvApproval.PageIndex = currentPageIndex;
                gvApproval.DataBind();
                OverviewApproval();
            }
            catch (Exception ER_AP_20)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_AP_20: " + ER_AP_20.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void ShowListview()
        {
            Accordion_ApprovalInfo.Text = "Approval Info";
            divApproval.Visible = true;
            btnAdd.Visible = true;
            btnDelete.Visible = true;
            ddlWarehouseSearch.Visible = true;

            btnSaveGroup.Visible = false;
            btnUpd.Visible = false;
            btnCancel.Visible = false;
            divEdit.Visible = false;
        }

        protected void HideListView()
        {
            Accordion_ApprovalInfo.Text = "Approval Edit";
            divApproval.Visible = false;

            btnAdd.Visible = false;
            btnDelete.Visible = false;
            ddlWarehouseSearch.Visible = false;

            divEdit.Visible = true;
            btnSaveGroup.Text = "Save New";
            btnSaveGroup.Visible = true;
            btnUpd.Visible = true;
            btnCancel.Visible = true;
        }

        private void DropDownList()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            List<ListItem> items = new List<ListItem>();
            try
            {
                ddlWarehouse.Items.AddRange(WClaim_GET_NewApplicant.get_Warehouse(DynAx).ToArray());

                DotNetUser();
            }
            catch (Exception ER_AP_01)
            {
                Function_Method.MsgBox("ER_AP_01: " + ER_AP_01.ToString(), this.Page, this);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            foreach (GridViewRow row in gvApproval.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox_selection = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (CheckBox_selection.Checked)
                    {
                        string recId = gvApproval.DataKeys[row.RowIndex].Values["hdRecId"].ToString();
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Warranty_AppGrp"))
                        {
                            DynAx.TTSBegin();
                            DynRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == {1}", "RecId", recId));
                        }
                    }
                }
            }
            DynAx.TTSCommit();
            DynAx.TTSAbort();
            DynAx.Dispose();
            Response.Redirect("ApprovalList.aspx");
        }

        private void DotNetUser()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string query = "SELECT user_id, page_access_authority FROM user_tbl ORDER BY user_id ASC";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            DataSet ds = new DataSet();

            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                da.Fill(ds);

                List<DropDownList> dropdownLists = new List<DropDownList>()
                {
                    ddlInvoiceCheck1, ddlInvoiceCheck2, ddlInvoiceCheck3, ddlInvoiceCheck4,
                    ddlTransport1, ddlTransport2, ddlTransport3, ddlTransport4,
                    ddlGoodReceive1, ddlGoodReceive2, ddlGoodReceive3, ddlGoodReceive4,
                    ddlInspector1, ddlInspector2, ddlInspector3, ddlInspector4,
                    ddlVerifier1, ddlVerifier2, ddlVerifier3, ddlVerifier4,
                    ddlApprover1, ddlApprover2, ddlApprover3, ddlApprover4
                };

                ClearAndAddSelectOption(dropdownLists);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string userId = row["user_id"].ToString();
                    string pageAccessAuthority = row["page_access_authority"].ToString();

                    ListItem item = new ListItem(userId);

                    AddItemToDropdownLists(dropdownLists, item);
                }
            }

            conn.Close();
        }

        private void ClearAndAddSelectOption(List<DropDownList> dropdownLists)
        {
            foreach (var ddl in dropdownLists)
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem("-- SELECT --", ""));
            }
        }

        private void AddItemToDropdownLists(List<DropDownList> dropdownLists, ListItem item)
        {
            foreach (var ddl in dropdownLists)
            {
                ddl.Items.Add(item);
            }
        }

        private void AddDefaultSelectItemToDropDowns(List<DropDownList> dropdownLists)
        {
            foreach (var ddl in dropdownLists)
            {
                ddl.Items.Insert(0, new ListItem("-- SELECT --", ""));
            }
        }

        void SetSelectedValue(DropDownList ddl, string getApprovalValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(getApprovalValue))
                {
                    ddl.SelectedValue = getApprovalValue;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Handle the exception, e.g., set to a default value or log the error
                ddlTransport2.SelectedIndex = 0; // Optional: set to the first item or a default item
            }
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private void DropDownList_Search()
        {
            List<ListItem> warehouse = new List<ListItem>();
            warehouse = WClaim_GET_NewApplicant.getWarrantyWarehouse();
            // Add "--All" item at the beginning of the list
            //warehouse.Insert(0, new ListItem("--All", ""));
            ddlWarehouseSearch.Items.AddRange(warehouse.ToArray());
        }
        protected void LocationChanged_Warranty(object sender, EventArgs e)
        {
            OverviewApproval();
        }
    }
}