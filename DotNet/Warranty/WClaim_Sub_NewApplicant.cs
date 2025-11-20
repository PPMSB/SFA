using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace DotNet
{
    public partial class WClaim : System.Web.UI.Page
    {
        private void clear_parameter_NewApplication_Customer()
        {
            Label_Address.Text = "";
            hidden_inventLocationId.Text = "";
            // <!--Customer ////////////////////////////////////////////////////////////////////////////////////-->
            Label_Account.Text = "";
            Label_CustName.Text = "";
            Label_Salesman.Text = "";

            div_CustInfoExtra.Visible = false;
            Show_Hide_NewApplication_CustomerInfo(true);

            hidden_Label_NextStatus.Text = "";
            hidden_Label_PreviousStatus.Text = "";
            hidden_LabelClaimNumber.Text = "";
        }

        private void clear_parameter_NewApplication_BatteryDetails()
        {
            Label_BatteryName.Text = "";
            Label_ItemID.Text = "";
            TextBox_SerialNo.Text = "";
            TextBox_Quantity.Text = "";
            DropDownList_Unit.Items.Clear();
            TextBox_CustomerDO.Text = "";

            DropDownList_ReasonReturn_Battery.ClearSelection();
            TextBox_ReasonReturn_Battery_Other.Text = "";
            TextBox_ReasonReturn_Battery_Other.Visible = false;
            Accordion_BatteryDetails.Visible = false;
            new_applicant_section_BatteryDetails.Attributes.Add("style", "display:none");
        }

        private void clear_parameter_NewApplication_ItemDetails()
        {
            Label_ItemName_Item.Text = "";
            Label_ItemID_Item.Text = "";
            TextBox_Quantity_Item.Text = "";
            DropDownList_Unit_Items.Items.Clear();
            TextBox_CustomerDO_Item.Text = "";
            DropDownList_ReasonReturn_Item.ClearSelection();
            TextBox_ReasonReturn_Item.Text = "";
            TextBox_ReasonReturn_Item.Visible = false;
            Accordion_ItemDetails.Visible = false;
            new_applicant_section_ItemDetails.Attributes.Add("style", "display:none");
        }

        private void clear_parameter_NewApplication_BatchItemDetails()
        {
            DropDownList_ReasonReturn_BatchItemDetails.ClearSelection();
            initialize_GridView_BatchItem();
            Accordion_BatchItemDetails.Visible = false;
            new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:none");
        }

        private void clear_parameter_NewApplication_TransportationArrangement()
        {
            Accordion_TransportationArrangement.Visible = false;
            new_applicant_sectionTransportationArrangement.Visible = false;
        }

        //CustInfo //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void CheckAcc(object sender, EventArgs e)
        {
            validate();
        }

        private void validate()
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string CustAcc = Label_Account.Text.Trim();
            clear_parameter_NewApplication_Customer();
            clear_parameter_NewApplication_BatteryDetails();
            clear_parameter_NewApplication_ItemDetails();
            clear_parameter_NewApplication_BatchItemDetails();
            //clear_parameter_NewApplication_TransportationArrangement();
            Label_Account.Text = CustAcc;//after clear all, rewrite back

            if (CustAcc == "")
            {
                Function_Method.MsgBox("There is no account number", this.Page, this);
                return;
            }

            try
            {
                var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                string CustName = tuple_getCustInfo.Item1;
                string Address = tuple_getCustInfo.Item2;
                string temp_EmplId = tuple_getCustInfo.Item3;
                string temp_EmpName = tuple_getCustInfo.Item4;
                //string BranchID = tuple_getCustInfo.Item5;//dimension
                //string CustomerClass = tuple_getCustInfo.Item6.Trim();
                //string getOpeningAccDate = tuple_getCustInfo.Item7;
                if (CustName == "")
                {
                    upImg.Visible = false;
                    Function_Method.MsgBox("No data found", this.Page, this);
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_new_applicant_section'); ", true);
                    return;
                }
                else
                {
                    Label_Salesman.Text = "(" + temp_EmplId + ") " + temp_EmpName;
                    hdSalesmanId.Value = temp_EmplId;
                    Label_CustAcc.Text = CustAcc;
                    Label_CustName.Text = CustName;
                    Label_Address.Text = Address;
                    //Button_Delivery_Addr.Visible = false;
                    div_CustInfoExtra.Visible = true;

                    Alt_Addr_function();

                }
            }
            catch (Exception ER_WC_01)
            {
                Function_Method.MsgBox("ER_WC_01: " + ER_WC_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_WCCM@";//WClaim > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        protected void RadioButtonChanged_Warranty(object sender, EventArgs e)
        {
            BatteryButtonChanged();
        }

        private void BatteryButtonChanged()
        {
            string ClaimType = "";
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                switch (rblWarranty.SelectedValue)
                {
                    case "1":
                        //lbltype.Visible = false;
                        //ddlClaimType.Visible = false;
                        divProductType.Visible = false;
                        ClaimType = "Batch";
                        switch (rblWarranty.SelectedValue == "1")
                        {
                            default://oil/other product
                                    //subClaimType = Convert.ToInt32(ddlClaimType.SelectedValue);
                                    //rfvBattery.Enabled = false;
                                TextBox_CustomerDO_Item.Enabled = false;
                                //rfvReasonReturn_Item.Enabled = false;
                                rfvClaim.Enabled = false;

                                var getWarehousePIC = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, rblWarranty.SelectedItem.Text, "");
                                int selectedIndex = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(getWarehousePIC.Item7.ToString()));
                                //if (ddlWarehouse.SelectedItem.Text == "HO" || ddlWarehouse.SelectedItem.Text == "MP")
                                //{
                                //    ddlWarehouse.SelectedIndex = selectedIndex;

                                //}

                                new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:initial");
                                Accordion_BatchItemDetails.Visible = true;

                                new_applicant_section_BatteryDetails.Attributes.Add("style", "display:none");
                                Accordion_BatteryDetails.Visible = false;

                                new_applicant_section_ItemDetails.Attributes.Add("style", "display:none");
                                Accordion_ItemDetails.Visible = false;
                                break;
                        }
                        break;

                    case "2":
                        //lbltype.Visible = true;
                        //ddlClaimType.Visible = true;
                        divProductType.Visible = true;
                        upload_section.Visible = false;
                        txtbatchReason.Visible = false;

                        ClaimType = "Warranty";
                        switch (ddlClaimType.SelectedValue)
                        {
                            case "1"://battery

                                var getWarehousePIC1 = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, "Battery", "");
                                int selectedIndex1 = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(getWarehousePIC1.Item7.ToString()));
                                if (ddlWarehouse.SelectedItem.Text == hidden_inventLocationId.Text)
                                {
                                    ddlWarehouse.SelectedIndex = selectedIndex1;
                                }
                                //rfvCustDo.Enabled = false;
                                new_applicant_section_BatteryDetails.Attributes.Add("style", "display:initial");
                                Accordion_BatteryDetails.Visible = true;

                                new_applicant_section_ItemDetails.Attributes.Add("style", "display:none");
                                Accordion_ItemDetails.Visible = false;

                                new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:none");
                                Accordion_BatchItemDetails.Visible = false;
                                divCustChop.Attributes["class"] = "column print-show";
                                break;

                            case "2"://lubricant

                                var getWarehousePIC2 = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, "Lubricant", "");
                                int selectedIndex2 = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(getWarehousePIC2.Item7.ToString()));
                                //if (ddlWarehouse.SelectedItem.Text == "HO" || ddlWarehouse.SelectedItem.Text == "MP")
                                //{

                                //    ddlWarehouse.SelectedIndex = selectedIndex2;
                                //}
                                new_applicant_section_ItemDetails.Attributes.Add("style", "display:initial");
                                Accordion_ItemDetails.Visible = true;

                                new_applicant_section_BatteryDetails.Attributes.Add("style", "display:none");
                                Accordion_BatteryDetails.Visible = false;

                                new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:none");
                                Accordion_BatchItemDetails.Visible = false;
                                break;

                            case "3":

                                var getWarehousePIC3 = WClaim_GET_NewApplicant.getWarrantyApprovalUser(DynAx, "Other Products", "");
                                int selectedIndex3 = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(getWarehousePIC3.Item7.ToString()));
                                //if (ddlWarehouse.SelectedItem.Text == "HO" || ddlWarehouse.SelectedItem.Text == "MP")
                                //{
                                //    ddlWarehouse.SelectedIndex = selectedIndex3;
                                //}
                                new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:initial");
                                Accordion_BatchItemDetails.Visible = true;

                                new_applicant_section_BatteryDetails.Attributes.Add("style", "display:none");
                                Accordion_BatteryDetails.Visible = false;

                                new_applicant_section_ItemDetails.Attributes.Add("style", "display:none");
                                Accordion_ItemDetails.Visible = false;
                                DropDownList_ReasonReturn_OtherProducts.Visible = true;
                                DropDownList_ReasonReturn_BatchItemDetails.Visible = false;
                                break;

                            default:
                                new_applicant_section_BatteryDetails.Attributes.Add("style", "display:none");
                                Accordion_BatteryDetails.Visible = false;

                                new_applicant_section_ItemDetails.Attributes.Add("style", "display:none");
                                Accordion_ItemDetails.Visible = false;

                                new_applicant_section_BatchItemDetails.Attributes.Add("style", "display:none");
                                Accordion_BatchItemDetails.Visible = false;
                                break;
                        }
                        break;
                }
                Dropdown_Transporter_Receiver(DynAx, ClaimType);

            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ER_WC_BatteryButtonChanged)
            {
                logger.Error($"Error in BatteryButtonChanged - X++ Exception:{ER_WC_BatteryButtonChanged.Message}");
                //return false;
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ER_WC_BatteryButtonChanged)
            {
                logger.Error($"Error in BatteryButtonChanged - LogonFailedException:{ER_WC_BatteryButtonChanged.Message}");
                //return false;
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (UnauthorizedAccessException ER_WC_BatteryButtonChanged)
            {
                logger.Error($"Error in BatteryButtonChanged - UnauthorizedAccessException:{ER_WC_BatteryButtonChanged.Message}");
                //return false;
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.SessionTerminatedException ER_WC_BatteryButtonChanged)
            {
                logger.Error($"Error in BatteryButtonChanged - Session terminated.:{ER_WC_BatteryButtonChanged.Message}");
                /*DynAx = Function_Method.GlobalAxapta();*/ // Re-establish the session
                //return false;
            }
            catch (Exception ER_WC_BatteryButtonChanged)
            {
                logger.Error($"Error in BatteryButtonChanged for ClaimNumber:  {ER_WC_BatteryButtonChanged}");
                Function_Method.MsgBox("ER_WC_21: " + ER_WC_BatteryButtonChanged.ToString(), this.Page, this);
                //return false;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private void Show_Hide_NewApplication_CustomerInfo(bool paramater)
        {
            if (paramater == true)//show
            {
                new_applicant_section_CustomerInfo.Attributes.Add("style", "display:initial");
            }
            else
            {
                new_applicant_section_CustomerInfo.Attributes.Add("style", "display:none");
            }
        }

        //BatteryDetails //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void OnSelectedIndexChanged_DropDownList_ReasonReturn_Battery(object sender, EventArgs e)
        {
            upload_section.Visible = true;

            if (DropDownList_ReasonReturn_Battery.SelectedItem.Value == "99")//Others
            {
                TextBox_ReasonReturn_Battery_Other.Visible = true;
            }
            else
            {
                TextBox_ReasonReturn_Battery_Other.Visible = false;
            }
        }

        protected void CheckBatteryInList(object sender, EventArgs e)
        {
            string AccNo = Label_Account.Text;
            string claimNum = lblClaimNum.Text;    // Assuming lblClaimNum is accessible here
            string claimText = lblClaimText.Text;  // Assuming lblClaimText is accessible here
            bool getCheckItemExist = false;
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                if (ddlClaimType.SelectedValue == "1")//battery only
                {
                    getCheckItemExist = WClaim_GET_NewApplicant.getCheckItemExist(DynAx, AccNo);
                    if (getCheckItemExist == false)
                    {
                        Function_Method.MsgBox("There is no data to search for this customer", this.Page, this);
                        return;
                    }
                    else
                    {
                        Session["data_passing"] = "_WCBA@" + AccNo + "|" + rblWarranty.SelectedValue + "|" + "1" + "|" + rblTransport.SelectedValue + "|" + ddlWarehouse.SelectedValue;//Battery
                    }
                }
                else
                {
                    if (ddlClaimType.SelectedValue != "2")//batch
                    {
                        Session["data_passing"] = "_WCBA@" + AccNo + "|" + rblWarranty.SelectedValue + "|" + "1" + "|" + rblTransport.SelectedValue + "|" + ddlWarehouse.SelectedValue;//Battery
                    }
                    else// warranty
                    {
                        Session["data_passing"] = "_WCO@" + AccNo + "|" + rblWarranty.SelectedValue + "|" + "2" + "|" + rblTransport.SelectedValue + "|" + ddlWarehouse.SelectedValue;//Oil
                    }
                }

                // Add claimNum and claimText to session 
                if (claimText == "Awaiting Invoice Chk" || claimText == "Draft")
                {
                    Session["ClaimNum"] = claimNum;
                    Session["ClaimText"] = claimText;
                }
            }
            catch (Exception ER_WC_02)
            {
                Function_Method.MsgBox("ER_WC_02: " + ER_WC_02.ToString(), this.Page, this);
                return;
            }
            finally
            {
                DynAx.Dispose();
            }

            Response.Redirect("Battery.aspx");
        }
        //Other Item//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        protected void OnSelectedIndexChanged_DropDownList_ReasonReturn_Item(object sender, EventArgs e)
        {
            upload_section.Visible = true;

            if (DropDownList_ReasonReturn_Item.SelectedItem.Value == "99")//Others
            {
                TextBox_ReasonReturn_Item.Visible = true;
            }
            else
            {
                TextBox_ReasonReturn_Item.Visible = false;
            }
        }

        //BatchItemDetails //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        //GridView_Equipment
        private void initialize_GridView_BatchItem()
        {
            GridView_BatchItem.Columns[5].Visible = true;//RecId
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemID", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("CustomerDO", typeof(string)));
            dr = dt.NewRow();

            dr["No."] = 1;
            dr["ItemID"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["Qty"] = string.Empty;
            dr["Unit"] = string.Empty;
            dr["CustomerDO"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
            GridView_BatchItem.DataSource = dt;
            GridView_BatchItem.DataBind();
        }

        protected void Button_AddBatchItem_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //Extract the  values

                        TextBox box1 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        Label lblItemID = (Label)GridView_BatchItem.Rows[rowIndex].Cells[2].FindControl("lblItemID");
                        TextBox box2 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbBtl");
                        RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbAdd");
                        TextBox box3 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");
                        DropDownList ddl = (DropDownList)GridView_BatchItem.Rows[rowIndex].Cells[5].FindControl("DropDownList_CustDO");

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["No."] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Description"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["ItemID"] = lblItemID.Text;
                        dtCurrentTable.Rows[i - 1]["Qty"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Unit"] = radiobutton1.Checked.ToString() + "|" + radiobutton1.Text + "|" + radiobutton2.Checked.ToString() + "|" + radiobutton2.Text;
                        if (box3.Text == "")
                        {
                            dtCurrentTable.Rows[i - 1]["CustomerDO"] = ddl.Text;
                        }
                        else
                        {
                            dtCurrentTable.Rows[i - 1]["CustomerDO"] = box3.Text;
                        }


                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView_BatchItem.DataSource = dtCurrentTable;
                    GridView_BatchItem.DataBind();
                }
                else
                {
                    initialize_GridView_BatchItem();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        Label lblItemID = (Label)GridView_BatchItem.Rows[rowIndex].Cells[2].FindControl("lblItemID");
                        TextBox box2 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbBtl");
                        RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[rowIndex].Cells[4].FindControl("rbAdd");
                        TextBox box3 = (TextBox)GridView_BatchItem.Rows[rowIndex].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

                        lblItemID.Text = dt.Rows[i]["ItemID"].ToString();
                        box1.Text = dt.Rows[i]["Description"].ToString();
                        box2.Text = dt.Rows[i]["Qty"].ToString();
                        string Unit = dt.Rows[i]["Unit"].ToString();

                        if (!string.IsNullOrEmpty(Unit))
                        {
                            string[] split = Unit.Split('|');
                            if (split[0] == "True")
                            {
                                radiobutton1.Checked = true;
                            }
                            else
                            {
                                radiobutton2.Checked = true;
                            }
                            radiobutton1.Text = split[1];
                            radiobutton2.Text = split[3];
                        }
                        box3.Text = dt.Rows[i]["CustomerDO"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        protected void GridView_BatchItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                dt.Rows.RemoveAt(e.RowIndex);

                //=====================================================================
                //to set back row number record textbox data
                int row_count = dt.Rows.Count;
                for (int i = 0; i < row_count; i++)
                {
                    int currentNo = Convert.ToInt32(dt.Rows[i]["No."]);

                    TextBox box1 = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                    Label box2 = (Label)GridView_BatchItem.Rows[currentNo - 1].Cells[2].FindControl("lblItemID");
                    TextBox box3 = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[3].FindControl("TextBox_New_QTY");
                    RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[currentNo - 1].Cells[4].FindControl("rbBtl");
                    RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[currentNo - 1].Cells[4].FindControl("rbAdd");
                    TextBox CustDo = (TextBox)GridView_BatchItem.Rows[currentNo - 1].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

                    dt.Rows[i]["Description"] = box1.Text;
                    dt.Rows[i]["ItemID"] = box2.Text;
                    dt.Rows[i]["Qty"] = box3.Text;

                    if (radiobutton1.Checked)
                    {
                        radiobutton1.Checked = true;
                        dt.Rows[i]["Unit"] = radiobutton1.Text;
                    }
                    else
                    {
                        radiobutton2.Checked = true;
                        dt.Rows[i]["Unit"] = radiobutton2.Text;
                    }
                    dt.Rows[i]["CustomerDO"] = CustDo.Text;
                    dt.Rows[i]["No."] = (i + 1).ToString();
                }
                //=====================================================================

                GridView_BatchItem.DataSource = dt;
                GridView_BatchItem.DataBind();
                ViewState["CurrentTable"] = dt;
                //to set back data for textbox for viewing
                for (int i = 0; i < row_count; i++)
                {
                    TextBox box1 = (TextBox)GridView_BatchItem.Rows[i].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                    Label box2 = (Label)GridView_BatchItem.Rows[i].Cells[2].FindControl("lblItemID");
                    TextBox box3 = (TextBox)GridView_BatchItem.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                    RadioButton radiobutton1 = (RadioButton)GridView_BatchItem.Rows[i].Cells[4].FindControl("rbBtl");
                    RadioButton radiobutton2 = (RadioButton)GridView_BatchItem.Rows[i].Cells[4].FindControl("rbAdd");
                    TextBox CustDo = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

                    box1.Text = dt.Rows[i]["Description"].ToString();
                    box2.Text = dt.Rows[i]["ItemID"].ToString();
                    box3.Text = dt.Rows[i]["Qty"].ToString();
                    if (radiobutton1.Checked)
                    {
                        radiobutton1.Checked = true;
                        radiobutton1.Text = dt.Rows[i]["Unit"].ToString();
                    }
                    else
                    {
                        radiobutton2.Checked = true;
                        radiobutton2.Text = dt.Rows[i]["Unit"].ToString();
                    }
                    CustDo.Text = dt.Rows[i]["CustomerDO"].ToString();
                }
            }

        }

        protected void TextBox_DescriptionBatchItem_Changed(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            //get the the id that call the function
            TextBox TextBox_DescriptionBatchItem = sender as TextBox;

            string ClientID = TextBox_DescriptionBatchItem.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)TextBox_DescriptionBatchItem.NamingContainer;
            int index = row.RowIndex;
            //
            TextBox box1 = (TextBox)GridView_BatchItem.Rows[index].Cells[3].FindControl("TextBox_New_QTY");
            TextBox box2 = (TextBox)GridView_BatchItem.Rows[index].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");
            Label lblItemID = (Label)GridView_BatchItem.Rows[index].Cells[2].FindControl("lblItemID");

            //clear when Description change
            box1.Text = "";
            box2.Text = "";
            lblItemID.Text = "";

            try
            {
                //get the the id that call the function
                TextBox Description = (TextBox)GridView_BatchItem.Rows[index].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                string Description_toSearch = Description.Text.Trim();

                if (Description_toSearch != "")
                {
                    DropDownList DropDownList_SearchBatchItem = (DropDownList)GridView_BatchItem.Rows[index].Cells[1].FindControl("DropDownList_SearchBatchItem");

                    DropDownList_SearchBatchItem.Items.Clear();
                    List<ListItem> List_BatchItem = EOR_GET_NewApplicant.get_SearchEquipment(DynAx, Description_toSearch, "");
                    if (List_BatchItem.Count > 1)
                    {
                        DropDownList_SearchBatchItem.Items.AddRange(List_BatchItem.ToArray());
                        DropDownList_SearchBatchItem.Visible = true;
                        Description.Visible = false;
                    }
                    else
                    {
                        Description.Text = "";
                        Description.Focus();
                        Description.Attributes["placeholder"] = "Item not found!";
                    }
                }
            }
            catch (Exception ER_WC_09)
            {
                Function_Method.MsgBox("ER_WC_09: " + ER_WC_09.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        protected void OnSelectedIndexChanged_DropDownList_SearchBatchItem(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            //get the the id that call the function
            DropDownList DropDownList_SearchBatchItem = sender as DropDownList;

            string ClientID = DropDownList_SearchBatchItem.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)DropDownList_SearchBatchItem.NamingContainer;
            int index = row.RowIndex;

            TextBox box2 = (TextBox)GridView_BatchItem.Rows[index].Cells[1].FindControl("TextBox_DescriptionBatchItem");
            Label lblItemID = (Label)GridView_BatchItem.Rows[index].Cells[2].FindControl("lblItemID");
            RadioButton btl = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbBtl");
            RadioButton add = (RadioButton)GridView_BatchItem.Rows[index].Cells[4].FindControl("rbAdd");
            HiddenField hdrecid = (HiddenField)GridView_BatchItem.Rows[index].FindControl("hd_RecIdBatchItem");
            DropDownList ddlinvoiceid = (DropDownList)GridView_BatchItem.Rows[index].Cells[5].FindControl("DropDownList_CustDO");
            TextBox txtInvoiceId = (TextBox)GridView_BatchItem.Rows[index].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

            string BatchItemId_RecId = DropDownList_SearchBatchItem.SelectedItem.Value;
            string[] arr_BatchItemId_RecId = BatchItemId_RecId.Split('|');
            string ItemId = arr_BatchItemId_RecId[0];
            string RecId = arr_BatchItemId_RecId[1];

            AxaptaObject domComSalesLine = DynAx.CreateAxaptaObject("DomComSalesLine");

            string itemunit = SFA_GET_SALES_ORDER.get_SLUnit(DynAx, ItemId);//fieldValue=Itemid
            string itemUnitStr = domComSalesLine.Call("getValidUnits", ItemId).ToString();//fieldValue=Itemid
            string[] arr_itemUnitStr = itemUnitStr.Split(new char[] { ',', '=' });

            for (int i = 0; i < arr_itemUnitStr.Length; i += 2)
            {
                btl.Text = arr_itemUnitStr[0].Trim();
                if (arr_itemUnitStr[2] != "")
                {
                    add.Text = arr_itemUnitStr[2].Trim();
                    add.Visible = true;
                }
            }

            if (ItemId != "")//Only selected
            {
                box2.Text = DropDownList_SearchBatchItem.SelectedItem.ToString();
                lblItemID.Text = ItemId;
                List<ListItem> getInvoiceId = WClaim_GET_NewApplicant.get_CustInvoiceTrans_details(DynAx, ItemId, Label_CustAcc.Text);
                if (getInvoiceId.Count > 1)
                {
                    ddlinvoiceid.Items.AddRange(getInvoiceId.ToArray());
                    ddlinvoiceid.Visible = true;
                    //txtInvoiceId.Visible = false;
                }

                if (getInvoiceId.Count == 0 || ddlinvoiceid.Items.Count == 0)
                {
                    ddlinvoiceid.Visible = false;
                    txtInvoiceId.Visible = true;
                    txtInvoiceId.Attributes["placeholder"] = "Invoice not found! Please insert.";
                }
                hdrecid.Value = RecId;
                DropDownList_SearchBatchItem.Items.Clear();
                DropDownList_SearchBatchItem.Visible = false;
                box2.Visible = true;
            }

            string selectedDate = SelectedDateHiddenField.Value;
            if (!string.IsNullOrEmpty(selectedDate))
            {
                // Set the datepicker's value to the selected date
                txtCollectionDt.Text = selectedDate;
            }
            else
            {
                string collectionDate = Request.Form[txtCollectionDt.UniqueID];

                txtCollectionDt.Text = collectionDate;
            }
            DynAx.Dispose();
        }

        protected void DropDownList_CustDO_SelectedIndexChanged(object sender, EventArgs e)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            DropDownList DropDownList_SearchBatchItem = sender as DropDownList;
            string ClientID = DropDownList_SearchBatchItem.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)DropDownList_SearchBatchItem.NamingContainer;
            int index = row.RowIndex;

            DropDownList invoiceid = (DropDownList)GridView_BatchItem.Rows[index].Cells[4].FindControl("DropDownList_CustDO");
            TextBox txtInvoiceId = (TextBox)GridView_BatchItem.Rows[index].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");

            var invoiceOnly = invoiceid.Text.Split(' ');
            var salesUnit = WClaim_GET_NewApplicant.get_CustInvoiceSalesUnit(DynAx, invoiceOnly[0]);

            if (string.IsNullOrEmpty(DropDownList_SearchBatchItem.SelectedValue))
            {
                // Show an error message or handle the error
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select customer DO');", true);
                return;
            }
            else
            {
                txtInvoiceId.Text = DropDownList_SearchBatchItem.SelectedValue.ToString();
                DropDownList_SearchBatchItem.Visible = false;
            }
        }
        //TransportationArrangement //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            divInvoiceChkRemark.Visible = false;
            divInspectionRemark.Visible = false;
            divGoodsReceiveRemark.Visible = false;
            divVerficationRemark.Visible = false;
            divApproverRemark.Visible = false;
            Button_Submit.Visible = false;//avoid user click multiple times
            submit();
            rfvProductImg.Text = "";
            btnDisplay.Visible = false;
            Button_Submit.Visible = true;
        }

        private void submit()
        {
            if (check_parameter_Item())
            {
                string validationMessage;
                // Neil - 2025-8-8 makesure at least 2 image submit before proceed any new submission
                if (ValidateFileUploadChecking(out validationMessage))
                {
                    if (Submit_Process())
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "Javascript:GoToTab('btnOverview');", true);
                    }
                    else
                    {
                        Function_Method.MsgBox("Please fill in the blanks and try again.", this.Page, this);
                    }

                }
                else
                {
                    // Show the specific validation message for file uploads
                    Function_Method.MsgBox(validationMessage, this.Page, this);
                }
                
            }
            else
            {
                Function_Method.MsgBox("Empty field in Item Details section.", this.Page, this);
            }
        }

        private bool check_parameter_Item()
        {
            bool checkParam = true;

            if (ddlWarehouse.SelectedItem.Text == "-- SELECT --")
            {
                ddlWarehouse.BorderColor = Color.Red;
                checkParam = false;
                return checkParam;
            }

            if (Accordion_BatchItemDetails.Visible == true)
            {
                if (btnDisplay.Visible == false)
                {
                    if (pdf_section.Visible != true)
                    {
                        if (fileUpload.PostedFile.ContentLength > 0 && fileUpload2.PostedFile.ContentLength > 0)
                        {
                            // A file has been uploaded, and it's not empty
                            // You can process the uploaded file here
                            checkParam = true;
                        }
                        else
                        {
                            rfvProductImg.Validate();
                            RequiredFieldValidator1.Validate();
                            checkParam = false;
                        }
                    }
                }
            }
            //else if (ddlWarehouse.Text == "")
            //{
            //    checkParam = false;
            //    ddlWarehouse.BorderColor = Color.Red;
            //}
            else if (Accordion_ItemDetails.Visible == true)//lubricant
            {
                if (TextBox_CustomerDO_Item.Text == "" || TextBox_Quantity_Item.Text == "" ||
                    DropDownList_ReasonReturn_Item.SelectedValue == "0" || Label_ItemName_Item.Text == "")
                {
                    if (TextBox_Quantity_Item.Text == "") TextBox_Quantity_Item.BorderColor = Color.Red;
                    if (TextBox_CustomerDO_Item.Text == "") TextBox_CustomerDO_Item.BorderColor = Color.Red;
                    if (DropDownList_ReasonReturn_Item.SelectedValue == "0") DropDownList_ReasonReturn_Item.BorderColor = Color.Red;
                    if (Label_ItemName_Item.Text == "") Label_ItemName_Item.BorderColor = Color.Red;
                    checkParam = false;
                }
                else { checkParam = true; }
            }
            else //(Accordion_BatteryDetails.Visible == true)battery
            {
                if (TextBox_SerialNo.Text == "" || TextBox_Quantity.Text == "" || Label_BatteryName.Text == "" ||
                    TextBox_CustomerDO.Text == "" || DropDownList_ReasonReturn_Battery.SelectedItem.Text == "")
                {
                    if (Label_BatteryName.Text == "") Label_BatteryName.BorderColor = Color.Red;
                    if (TextBox_SerialNo.Text == "") TextBox_SerialNo.BorderColor = Color.Red;
                    if (TextBox_Quantity.Text == "") TextBox_Quantity.BorderColor = Color.Red;
                    if (TextBox_CustomerDO.Text == "") TextBox_CustomerDO.BorderColor = Color.Red;
                    if (DropDownList_ReasonReturn_Battery.SelectedItem.Text == "") DropDownList_ReasonReturn_Battery.BorderColor = Color.Red;
                    checkParam = false;
                }
                else { checkParam = true; }
            }
            return checkParam;
        }

        private bool ValidateFileUploadChecking(out string message)
        {
            // Check for the specific files by their names
            HttpPostedFile fileUpload = Request.Files["fileUpload"];
            HttpPostedFile fileUpload2 = Request.Files["fileUpload2"];
            HttpPostedFile fileUpload3 = Request.Files["fileUpload3"];
            HttpPostedFile fileUpload4 = Request.Files["fileUpload4"];

            // Initialize a counter for uploaded files
            int uploadedFileCount = 0;

            // Check each file and increment the counter if it has content
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                uploadedFileCount++;
            }
            if (fileUpload2 != null && fileUpload2.ContentLength > 0)
            {
                uploadedFileCount++;
            }
            if (fileUpload3 != null && fileUpload3.ContentLength > 0)
            {
                uploadedFileCount++;
            }
            if (fileUpload4 != null && fileUpload4.ContentLength > 0)
            {
                uploadedFileCount++;
            }

            // Check if at least 2 files are uploaded
            if (upload_section.Visible == true && btnDisplay.Visible == false)
            {
                if (uploadedFileCount >= 2)
                {
                    message = string.Empty; // No message needed if validation passes
                    return true;
                }
                else
                {
                    message = "Please must submit atleast 2 upload image evidences for the warrantly claim."; // Specific message for validation failure
                    return false;
                }
            }
            else
            {
                message = string.Empty; // No message needed if validation passes
                return true;
            }

        }

        private bool Submit_Process()
        {
            string ClaimType = "";//1.Warranty 2.Batch Return
            int subClaimType = 0;//1: Battery 2.Oil 3.Other Products
            string CustAccount = Label_Account.Text;

            //Jerry 2024-11-05 unique_key to be used as temp claim_number
            string unique_key = CustAccount + DateTime.Now.ToString("yyyyMMddHHmmss");

            string inventLocationId = "";
            string Address = Label_Address.Text;
            string Transport = ""; string TransportationReason = "";
            Axapta DynAx = Function_Method.GlobalAxapta();

            inventLocationId = ddlWarehouse.SelectedItem.Text;
            hidden_inventLocationId.Text = inventLocationId;
            Transport = rblTransport.SelectedValue;

            // Jerry 2024-12-18 update pic as long as at least one of fileUpload, fileUpload2, fileUpload3, fileUpload4 is not empty
            Function_Method.UserLog(GLOBAL.user_id + " check if fileUpload, fileUpload2, fileUpload3, fileUpload4 not empty.");
            if (Request.Files != null && Request.Files.Count > 0)
            {
                // Check for the specific files by their names
                HttpPostedFile fileUpload = Request.Files["fileUpload"];
                HttpPostedFile fileUpload2 = Request.Files["fileUpload2"];
                HttpPostedFile fileUpload3 = Request.Files["fileUpload3"];
                HttpPostedFile fileUpload4 = Request.Files["fileUpload4"];

                // Check if any of the files are not null and have content
                if ((fileUpload != null && fileUpload.ContentLength > 0) ||
                    (fileUpload2 != null && fileUpload2.ContentLength > 0) ||
                    (fileUpload3 != null && fileUpload3.ContentLength > 0) ||
                    (fileUpload4 != null && fileUpload4.ContentLength > 0))
                {
                    // At least one file is uploaded and has content
                    Function_Method.UserLog(GLOBAL.user_id + " uploading image");
                    uploadPic(unique_key);
                    Function_Method.UserLog(GLOBAL.user_id + " upload image done.");
                }
            }
            // Jerry 2024-12-18 update pic as long as at least one of fileUpload, fileUpload2, fileUpload3, fileUpload4 is not empty - END

            if (lblClaimText.Text == "New" || lblClaimText.Text == "Draft")//make sure every new warranty will upload
            {

                // Jerry 2024-12-18 update pic as long as at least one of fileUpload, fileUpload2, fileUpload3, fileUpload4 is not empty
                // Function_Method.UserLog(GLOBAL.user_id + " uploading image");
                // Jerry 2024-11-05 update pic with unique_key if claim number not generated yet
                // uploadPic(unique_key);
                // uploadPic();
                // Jerry 2024-11-05 end
                //Function_Method.UserLog(GLOBAL.user_id + " upload image done.");
                // Jerry 2024-12-18 update pic as long as at least one of fileUpload, fileUpload2, fileUpload3, fileUpload4 is not empty - END

                fileUpload.Visible = false;
                fileUpload2.Visible = false;
                fileUpload3.Visible = false;
                fileUpload4.Visible = false;
                lblimg.Visible = false;
                lblimg2.Visible = false;
                lblimg3.Visible = false;
                lblimg4.Visible = false;
            }

            if (rblWarranty.SelectedValue == "1")//batch
            {
                var tuple_get_Batch_Item = get_Batch_Item();//get item id
                string[] ItemId = tuple_get_Batch_Item.Item1;
                string[] DONumber = tuple_get_Batch_Item.Item4;
                ClaimType = "Batch";

                if (string.IsNullOrEmpty(ItemId[0]) || string.IsNullOrEmpty(DONumber[0]))//if item id & invoice id empty, return false
                {
                    return false;
                }
            }
            else//warranty
            {
                ClaimType = "Warranty";

                if (Accordion_BatteryDetails.Visible == true)
                {
                    subClaimType = 1;//battery
                }
                else if (Accordion_ItemDetails.Visible == true)
                {
                    subClaimType = 2;//lubricant
                }
                else if (Accordion_BatchItemDetails.Visible == true)
                {
                    var tuple_get_Batch_Item = get_Batch_Item();//get item id
                    string[] ItemId = tuple_get_Batch_Item.Item1;
                    string[] DONumber = tuple_get_Batch_Item.Item4;

                    subClaimType = 3;//others product

                    if (string.IsNullOrEmpty(ItemId[0]))//if item id empty & invoice id, return false
                    {
                        return false;
                    }
                }
            }

            try
            {
                if (ClaimType != "")
                {
                    string ClaimStatus = ""; string Process_Status = "";
                    string PreviousStatus = hidden_Label_PreviousStatus.Text;
                    string NextStatus = hidden_Label_NextStatus.Text;
                    string currentDt = DateTime.Now.ToString();
                    string WarrantyProcessStatus = ""; string TransportationDate = ""; string GoodReceiveDate = "";
                    string InvoiceCheckedDate = ""; string InspectionDate = ""; string VerifiedDate = ""; string ApprovedDate = "";
                    if (hidden_LabelClaimNumber.Text != "")
                    {
                        WarrantyProcessStatus = WClaim_GET_NewApplicant.getWarrantyProcessStatus(DynAx, hidden_LabelClaimNumber.Text);
                    }

                    if (PreviousStatus == "" && NextStatus == "Draft")//new  draft
                    {
                        ClaimStatus = "Draft";
                        Process_Status = "Draft by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                    }
                    else if (PreviousStatus == "Draft" && NextStatus == "Draft")// draft
                    {
                        ClaimStatus = "Draft";
                        Process_Status = WarrantyProcessStatus + "Draft Update by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                    }
                    else if (PreviousStatus == "Draft")
                    {
                        ClaimStatus = "Awaiting Invoice Chk";
                        Process_Status = WarrantyProcessStatus + "Submitted by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                    }

                    // Check if the revert action is triggered
                    if (NextStatus == "Revert")
                    {
                        ClaimStatus = "Draft";
                        PreviousStatus = "";
                        hidden_Label_PreviousStatus.Text = "";
                        Process_Status = WarrantyProcessStatus + "Revert to Draft by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                    }

                    if (PreviousStatus == "" && NextStatus == "")//new submit
                    {
                        ClaimStatus = "Awaiting Invoice Chk";
                        Process_Status = "Submitted by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                    }
                    else if (PreviousStatus == "Awaiting Invoice Chk")
                    {
                        if (Transport == "1")
                        {
                            ClaimStatus = "Awaiting Transporter";
                            if (txtTransportationRemark.Text != "")
                            {
                                TransportationReason = txtTransportationRemark.Text;
                            }
                        }
                        else
                        {
                            ClaimStatus = "Awaiting GoodsReceive";
                        }
                        Process_Status = WarrantyProcessStatus + "Invoice checked by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                        InvoiceCheckedDate = currentDt;
                    }
                    else if (PreviousStatus == "Awaiting Transporter")
                    {
                        ClaimStatus = "Awaiting GoodsReceive";
                        if (rblTransport.SelectedValue != "0")
                        {
                            Process_Status = WarrantyProcessStatus + "Transporter arranged by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                            TransportationDate = currentDt;
                        }
                        else
                        {
                            Process_Status = "";
                            TransportationDate = "";
                        }
                    }
                    else if (PreviousStatus == "Awaiting GoodsReceive")
                    {
                        ClaimStatus = "Awaiting Inspection";
                        Process_Status = WarrantyProcessStatus + "Goods received by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                        GoodReceiveDate = currentDt;
                    }
                    else if (PreviousStatus == "Awaiting Inspection")
                    {
                        ClaimStatus = "Awaiting Verified";
                        Process_Status = WarrantyProcessStatus + "Inspected by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                        InspectionDate = currentDt;

                        string getfilepath = uploadInspectImg();
                        if (subClaimType == 1)
                        {
                            Save_LF_WarrantyBattery(DynAx, lblClaimNum.Text, txtCca.Text, ddlLoadTest.SelectedValue, ddlTestResult.SelectedValue, ddlMagicEye.SelectedValue, getfilepath);
                        }
                    }
                    else if (PreviousStatus == "Awaiting Verified")
                    {
                        ClaimStatus = "Awaiting Approved";
                        Process_Status = WarrantyProcessStatus + "Verified by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                        VerifiedDate = currentDt;
                    }
                    else if (PreviousStatus == "Awaiting Approved")
                    {
                        ClaimStatus = "Approved";
                        Process_Status = WarrantyProcessStatus + "Approved by " + Session["user_id"].ToString() + " on " + currentDt + "<br/>";
                        ApprovedDate = currentDt;
                    }

                    string ClaimNumber = Save_LF_WarrantyTable(ClaimStatus, ClaimType, subClaimType, CustAccount, inventLocationId, Address,
                                                               Transport, TransportationReason, Process_Status, TransportationDate,
                                                               GoodReceiveDate, InvoiceCheckedDate, InspectionDate, VerifiedDate, ApprovedDate);

                    if (ClaimNumber == "")
                    {
                        Function_Method.MsgBox("Fail to create warranty header. Due to Axapta submission issue. ", this.Page, this);
                        return false;
                    }

                    Function_Method.UserLog(GLOBAL.user_id + " status " + lblClaimText.Text + " " + lblClaimNum.Text  + " warehouse selected: " + inventLocationId);

                    //Jerry 2024-11-05 update the uploaded image unique_key with claim_number
                    updatePic(unique_key, ClaimNumber);
                    //Jerry 2024-11-05 end

                    if (ClaimNumber != "")
                    {
                        string SerialNumber = ""; string CustDO = ""; string Unit = "";
                        string ItemID = ""; string ItemName = "";
                        int Qty = 0; string Price = "";
                        string ReturnReasonBattery = ""; string ReturnReasonOther = "";
                        string ReturnReasonBatch = ""; string OtherReason = "";
                        bool CheckWarrantyLine = false;

                        if (rblWarranty.SelectedValue == "1")//batch return
                        {
                            CheckWarrantyLine = ProcessBatchItemReturn(ReturnReasonBatch, OtherReason, CheckWarrantyLine, ClaimType, CustAccount,
                                 ClaimStatus, ClaimNumber, SerialNumber, ReturnReasonBattery, ReturnReasonOther);
                        }
                        else
                        {
                            if (subClaimType == 1)//battery
                            {
                                SerialNumber = TextBox_SerialNo.Text;// only battery have
                                CustDO = TextBox_CustomerDO.Text;

                                if (TextBox_Quantity.Text != "") Qty = Convert.ToInt32(TextBox_Quantity.Text);
                                if (DropDownList_Unit.Text != "") Unit = DropDownList_Unit.SelectedItem.Text;
                                ItemID = Label_ItemID.Text;
                                ItemName = Label_BatteryName.Text;

                                if (DropDownList_ReasonReturn_Battery.SelectedValue != "0")
                                {
                                    ReturnReasonBattery = DropDownList_ReasonReturn_Battery.SelectedItem.ToString();
                                    if (DropDownList_ReasonReturn_Battery.SelectedItem.Text == "Others")
                                    {
                                        OtherReason = TextBox_ReasonReturn_Battery_Other.Text;
                                    }
                                }
                                CheckWarrantyLine = Save_LF_WarrantyLine(ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                                CustDO, Qty, Unit, ItemID, ItemName, ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, Price);
                            }
                            else if (subClaimType == 2)//lubricant
                            {
                                if (TextBox_Quantity_Item.Text != "") Qty = Convert.ToInt32(TextBox_Quantity_Item.Text);
                                if (DropDownList_Unit_Items.Text != "") Unit = DropDownList_Unit_Items.SelectedItem.Text;
                                ItemID = Label_ItemID_Item.Text;
                                ItemName = Label_ItemName_Item.Text;
                                CustDO = TextBox_CustomerDO_Item.Text;
                                //Price = TextBox_LubricantPrice.Text;
                                if (DropDownList_ReasonReturn_Item.SelectedValue != "0")
                                {
                                    ReturnReasonOther = DropDownList_ReasonReturn_Item.SelectedItem.ToString();
                                    if (DropDownList_ReasonReturn_Item.SelectedItem.Text == "Others")
                                    {
                                        OtherReason = TextBox_ReasonReturn_Item.Text;
                                    }
                                }
                                CheckWarrantyLine = Save_LF_WarrantyLine(ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                                    CustDO, Qty, Unit, ItemID, ItemName, ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, Price);
                            }
                            else
                            {
                                CheckWarrantyLine = ProcessBatchItemReturn(ReturnReasonBatch, OtherReason, CheckWarrantyLine, ClaimType, CustAccount,
                                     ClaimStatus, ClaimNumber, SerialNumber, ReturnReasonBattery, ReturnReasonOther);
                            }                              

                            Function_Method.UserLog(GLOBAL.user_id + " warranty line " + ClaimNumber + ItemName + " ID: " + ItemID + " " +
                                    SerialNumber + " " + ReturnReasonBattery + OtherReason);
                        }

                        if (CheckWarrantyLine == false)
                        {
                            Function_Method.MsgBox("Fail to create warranty line. Please create again.", this.Page, this);
                            return false;
                        }
                        else
                        {
                            if (txtGetTransportName.Text != "")
                            {
                                string collectionDate = Request.Form[txtCollectionDt.UniqueID];
                                DateTime date = DateTime.ParseExact(collectionDate, "dd/MM/yyyy", null);

                                bool transport = Save_LF_WebClaim_Transport(DynAx, ClaimNumber, txtGetTransportName.Text, txtVehicle.Text, txtGetDriver.Text,
                                    txtGetStreet.Text, txtGetState.Text, date, txtGetPostcode.Text,
                                    txtGetCollectionPoint.Text, txtApproverRemark.Text, Convert.ToInt32(rblCNrequired.SelectedValue));
                            }

                            lblDisplay.Visible = false;
                            divProcessStat.Visible = false;

                            if (hidden_Label_PreviousStatus.Text != "")
                            {
                                Function_Method.MsgBox("Updated. ClaimNumber: " + ClaimNumber + " .", this.Page, this);
                            }
                            else
                            {
                                string isDraft = "";
                                if (ClaimStatus == "Draft")
                                {
                                    isDraft = "Save as Draft.";
                                }
                                else
                                {
                                    isDraft = "Submitted";
                                    //Process_Status = WarrantyProcessStatus + "Reject by " + GLOBAL.user_id + " on " + DateTime.Now + "<br/>";
                                    //getDetailsAndSendemail(PreviousStatus, WarrantyProcessStatus, ClaimStatus);
                                }
                                Function_Method.MsgBox(isDraft + " ClaimNumber: " + ClaimNumber + " .", this.Page, this);
                            }
                        }
                    }
                    else//fail to create header
                    {
                        Function_Method.MsgBox("Fail to create warranty header.", this.Page, this);
                        return false;
                    }
                }
                else
                {
                    Function_Method.MsgBox("Please select warranty type.", this.Page, this);
                }
            }
            catch (Exception e)
            {
                Function_Method.MsgBox("Fail to save. " + e, Page, this);
                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
            return true;
        }

        private Tuple<string[], string[], int, string[], string[], string[], string[]> get_Batch_Item()
        {
            int row_count_BatchItem = GridView_BatchItem.Rows.Count;
            string[] New_QTY = new string[row_count_BatchItem];
            string[] Description = new string[row_count_BatchItem];
            string[] CustDO = new string[row_count_BatchItem];
            string[] RecId = new string[row_count_BatchItem];
            string[] Batch_ItemId = new string[row_count_BatchItem];
            string[] Unit = new string[row_count_BatchItem];
            //string[] Price = new string[row_count_BatchItem];

            if (row_count_BatchItem >= 1)
            {
                for (int i = 0; i < row_count_BatchItem; i++)
                {
                    if (GridView_BatchItem.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox box1 = (TextBox)GridView_BatchItem.Rows[i].Cells[1].FindControl("TextBox_DescriptionBatchItem");
                        TextBox Qty = (TextBox)GridView_BatchItem.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                        RadioButton radioBtl = (RadioButton)GridView_BatchItem.Rows[i].Cells[4].FindControl("rbBtl");
                        RadioButton radioAdd = (RadioButton)GridView_BatchItem.Rows[i].Cells[4].FindControl("rbAdd");
                        //TextBox getPrice = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_Price");
                        TextBox box3 = (TextBox)GridView_BatchItem.Rows[i].Cells[5].FindControl("TextBox_CustomerDO_BatchItem");
                        DropDownList dropdownlist = (DropDownList)GridView_BatchItem.Rows[i].Cells[5].FindControl("DropDownList_CustDO");
                        HiddenField box4 = (HiddenField)GridView_BatchItem.Rows[i].FindControl("hd_RecIdBatchItem");

                        Description[i] = ""; New_QTY[i] = "";
                        CustDO[i] = ""; RecId[i] = ""; Batch_ItemId[i] = "";

                        if (box1.Text.ToString() != "")
                        {
                            Description[i] = box1.Text.ToString();
                        }

                        if (Qty.Text.ToString() != "")
                        {
                            New_QTY[i] = Qty.Text.ToString();
                        }

                        if (box3.Text.ToString() != "")
                        {
                            CustDO[i] = box3.Text.ToString();
                        }
                        else
                        {
                            //CustDO[i] = dropdownlist.SelectedItem.Text;
                            if (dropdownlist.SelectedItem != null)
                            {
                                CustDO[i] = dropdownlist.SelectedItem.Text;
                            }
                            else
                            {
                                CustDO[i] = "";
                            }
                        }

                        if (box4.Value.ToString() != "")
                        {
                            RecId[i] = box4.Value.ToString();
                        }

                        if (radioBtl.Checked)
                        {
                            Unit[i] = radioBtl.Text;
                        }
                        else
                        {
                            Unit[i] = radioAdd.Text;
                        }
                    }
                }
                Batch_ItemId = get_Batch_ItemId(Description, RecId, row_count_BatchItem);
            }
            return new Tuple<string[], string[], int, string[], string[], string[], string[]>
                (Description, New_QTY, row_count_BatchItem, CustDO, Batch_ItemId, RecId, Unit);
        }

        private string[] get_Batch_ItemId(string[] Description, string[] RecId, int row_count_BatchItem)
        {
            string[] ItemId = new string[row_count_BatchItem];
            Axapta DynAx = Function_Method.GlobalAxapta();
            for (int i = 0; i < row_count_BatchItem; i++)
            {
                if (Description[i] != null || Description[i] != "")
                {
                    int InventTable = 175;

                    AxaptaObject axQuery1_2 = DynAx.CreateAxaptaObject("Query");
                    AxaptaObject axQueryDataSource1_2 = (AxaptaObject)axQuery1_2.Call("addDataSource", InventTable);
                    if (RecId[i] == "")
                    {
                        var qbr1_2_1 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 3);//ItemName
                        qbr1_2_1.Call("value", Description[i]);
                    }
                    else
                    {
                        var qbr1_2_2 = (AxaptaObject)axQueryDataSource1_2.Call("addRange", 65534);//RecId
                        qbr1_2_2.Call("value", RecId[i]);
                    }

                    AxaptaObject axQueryRun1_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_2);

                    if ((bool)axQueryRun1_2.Call("next"))
                    {
                        AxaptaRecord DynRec1_2 = (AxaptaRecord)axQueryRun1_2.Call("Get", InventTable);
                        ItemId[i] = DynRec1_2.get_Field("ItemId").ToString();
                        RecId[i] = DynRec1_2.get_Field("RecId").ToString();
                        DynRec1_2.Dispose();
                    }
                }
            }
            DynAx.Dispose();
            return ItemId;
        }

        //batch
        protected bool Save_LF_WarrantyLine_Batch(string ClaimStatus, string ClaimType, string CustAccount, string ClaimNumber,
            string SerialNumber, string[] CustDO, string[] Qty, string[] Unit, string[] ItemID, string[] ItemName, string ReturnReasonBattery,
            string ReturnReasonOther, string ReturnReasonBatch, string OtherReason, int row_count_BatchItem, string[] recid)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            logger.Info($"Starting to save warranty line batch for ClaimNumber: {ClaimNumber}, ClaimStatus: {ClaimStatus}");
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyLine"))
                {
                    if (hidden_Label_PreviousStatus.Text != "")
                    {
                        reinsert_routine_batch(DynAx, DynRec, ClaimNumber, row_count_BatchItem);
                        DynAx.TTSBegin();
                        for (int i = 0; i < row_count_BatchItem; i++)
                        {
                            if (ItemName[i] != "")
                            {
                                logger.Info($"Reinserting routine batch for ClaimNumber: {ClaimNumber}");
                                
                                Save_LF_WarrantyLine_Parameter(DynRec, ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                                    CustDO[i], Qty[i], Unit[i], ItemID[i], ItemName[i], ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason);
                                DynRec.Call("insert");
                                logger.Info($"Updated {ClaimNumber} {ItemName[i]} {ItemID[i]} {Qty[i]} {Unit[i]} {CustDO[i]} by {GLOBAL.user_id}");
                                Function_Method.UserLog
                                    ("Updated " + ClaimNumber + " " + ItemName[i] + " " + ItemID[i] + " " + Qty[i] + " " + Unit[i] + " " + CustDO[i] + " by " + GLOBAL.user_id);
                                
                            }
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                    else
                    {
                        DynAx.TTSBegin();
                        for (int i = 0; i < row_count_BatchItem; i++)
                        {
                            if (ItemName[i] != "")
                            {
                                
                                Save_LF_WarrantyLine_Parameter(DynRec, ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                                    CustDO[i], Qty[i], Unit[i], ItemID[i], ItemName[i], ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason);
                                Function_Method.UserLog
                                    ("Submitted " + ClaimNumber + " " + ItemName[i] + " " + ItemID[i] + " " + Qty[i] + " " + Unit[i] + " " + CustDO[i] + " by " + GLOBAL.user_id);
                                DynRec.Call("insert");
                                logger.Info($"Submitted {ClaimNumber} {ItemName[i]} {ItemID[i]} {Qty[i]} {Unit[i]} {CustDO[i]} by {GLOBAL.user_id}");
                                
                            }
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                    return true;
                }
            }
            catch (Exception ER_WC_03)
            {
                string message = "ER_WC_03: " + ER_WC_03.ToString();
                // Check if the exception is of type NotLoggedOnException
                if (ER_WC_03 is Microsoft.Dynamics.BusinessConnectorNet.NotLoggedOnException)
                {
                    message = "Your connection to Axapta has been interrupted. Please attempt to log in again to proceed. ";
                }
                Function_Method.MsgBox(message, this.Page, this);
                logger.Error($"Error in Save_LF_WarrantyLine_Batch for ClaimNumber: {ClaimNumber}: {ER_WC_03.Message}");

                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        //battery
        private bool Save_LF_WarrantyLine(string ClaimStatus, string ClaimType, string CustAccount, string ClaimNumber, string SerialNumber,
            string CustDO, int Qty, string Unit, string ItemID, string ItemName, string ReturnReasonBattery,
            string ReturnReasonOther, string ReturnReasonBatch, string OtherReason, string Price, string batchNumber = null)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();

            string str_Qty = Qty.ToString();
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyLine"))
                {
                    DynAx.TTSBegin();

                    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber", ClaimNumber));
                    if (DynRec.Found)
                    {
                        Save_LF_WarrantyLine_Parameter(DynRec, ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                            CustDO, str_Qty, Unit, ItemID, ItemName, ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, batchNumber);
                        DynRec.Call("Update");
                        logger.Info($"Updated warranty line for ClaimNumber: {ClaimNumber} with ItemID: {ItemID}");
                    }
                    else
                    {
                        Save_LF_WarrantyLine_Parameter(DynRec, ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                            CustDO, str_Qty, Unit, ItemID, ItemName, ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, batchNumber);
                        DynRec.Call("insert");
                        logger.Info($"Inserted new warranty line for ClaimNumber: {ClaimNumber} with ItemID: {ItemID}");
                    }
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                    return true;
                }
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ER_WC_21)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - X++ Exception:{ER_WC_21.Message}");
                return false;
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ER_WC_21)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - LogonFailedException:{ER_WC_21.Message}");
                return false;
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (UnauthorizedAccessException ER_WC_21)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - UnauthorizedAccessException:{ER_WC_21.Message}");
                return false;
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.SessionTerminatedException ER_WC_21)
            {
                logger.Error($"Error in Save_LF_WarrantyLine - Session terminated.:{ER_WC_21.Message}");
                /*DynAx = Function_Method.GlobalAxapta();*/ // Re-establish the session
                return false;
            }
            catch (Exception ER_WC_21)
            {
                logger.Error($"Error in Save_LF_WarrantyLine for ClaimNumber: {ClaimNumber}: {ER_WC_21}");
                Function_Method.MsgBox("ER_WC_21: " + ER_WC_21.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        public string Save_LF_WarrantyTable(string ClaimStatus, string ClaimType, int subClaimType, string CustAccount,
                                             string inventLocationId, string Address, string Transport, string TransportationReason,
                                             string Process_Status, string TransportationDate, string GoodReceiveDate, string InvoiceCheckedDate,
                                             string InspectionDate, string VerifiedDate, string ApprovedDate)
        {
            string ClaimNumber = "";
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyTable"))
                {
                    DynAx.TTSBegin();

                    if (hidden_LabelClaimNumber.Text != "")
                    {
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber",
                            hidden_LabelClaimNumber.Text));
                        if (DynRec.Found)
                        {
                            Save_LF_WarrantyTable_Parameter(DynRec, ClaimStatus, ClaimType, subClaimType, CustAccount, inventLocationId, Address,
                                                          Transport, TransportationReason, Process_Status, TransportationDate, GoodReceiveDate,
                                                          InvoiceCheckedDate, InspectionDate, VerifiedDate, ApprovedDate);
                            DynRec.Call("Update");
                            logger.Info($"Updated ClaimNumber: {hidden_LabelClaimNumber.Text} with status: {ClaimStatus}");
                        }
                    }
                    else
                    {
                        Save_LF_WarrantyTable_Parameter(DynRec, ClaimStatus, ClaimType, subClaimType, CustAccount, inventLocationId, Address,
                                                        Transport, TransportationReason, Process_Status, TransportationDate, GoodReceiveDate,
                                                        InvoiceCheckedDate, InspectionDate, VerifiedDate, ApprovedDate);
                        DynRec.Call("insert");
                        logger.Info($"Inserted new claim with status: {ClaimStatus}");
                    }

                    ClaimNumber = DynRec.get_Field("ClaimNumber").ToString();
                    DynAx.TTSCommit();
                    DynAx.TTSAbort();

                    if (hidden_LabelClaimNumber.Text == "")
                    {
                        string format = DateTime.Now.ToString("yyyyMM");
                        ClaimNumber = format + "/" + ClaimNumber;

                        DynAx.TTSBegin();
                        DynRec.set_Field("ClaimNumber", ClaimNumber);

                        DynRec.Call("Update");
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();

                        hidden_LabelClaimNumber.Text = ClaimNumber;
                        logger.Info($"ClaimNumber formatted and updated: {ClaimNumber}");
                    }

                    return ClaimNumber;
                }
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.XppException ER_WC_11)
            {
                logger.Error($"Error in Save_LF_WarrantyTable - X++ Exception:{ER_WC_11.Message}");
                return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.LogonFailedException ER_WC_11)
            {
                logger.Error($"Error in Save_LF_WarrantyTable - LogonFailedException:{ER_WC_11.Message}");
                return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (UnauthorizedAccessException ER_WC_11)
            {
                logger.Error($"Error in Save_LF_WarrantyTable - UnauthorizedAccessException:{ER_WC_11.Message}");
                return "";
                //ExceptionLogger.LogException(logger, ex, statusMessage);
            }
            catch (Microsoft.Dynamics.BusinessConnectorNet.SessionTerminatedException ER_WC_11)
            {
                logger.Error($"Error in Save_LF_WarrantyTable - Session terminated.:{ER_WC_11.Message}");
                /*DynAx = Function_Method.GlobalAxapta();*/ // Re-establish the session
                return "";
            }
            catch (Exception ER_WC_11)
            {
                logger.Error($"Error in Save_LF_WarrantyTable: {ER_WC_11}");
                Function_Method.MsgBox("ER_WC_11: " + ER_WC_11.ToString(), this.Page, this);
                Function_Method.UserLog(ER_WC_11.ToString());
                return "";
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private void Save_LF_WarrantyLine_Parameter(AxaptaRecord DynRec, string ClaimStatus, string ClaimType, string CustAccount,
                                                    string ClaimNumber, string SerialNumber, string CustDO, string Qty, string Unit,
                                                    string ItemID, string ItemName, string ReturnReasonBattery, string ReturnReasonOther,
                                                    string ReturnReasonBatch, string OtherReason, string batchNumber = null)
        {

            if (ClaimNumber != "") DynRec.set_Field("ClaimNumber", ClaimNumber);
            if (ItemName != "") DynRec.set_Field("Name", ItemName);
            if (ItemID != "") DynRec.set_Field("ItemID", ItemID);
            //system update only if any update from qty and unitid
            if (Qty != "")
            {
                int int_Qty = Convert.ToInt32(Qty);
                DynRec.set_Field("Qty", int_Qty);
            }

            if (Unit != "") { DynRec.set_Field("UnitID", Unit); }

            if (!string.IsNullOrEmpty(txtBatteryVol.Text)) DynRec.set_Field("BatteryVoltage", txtBatteryVol.Text);
            if (!string.IsNullOrEmpty(SerialNumber)) DynRec.set_Field("SerialNumber", SerialNumber);
            if (CustDO != "") DynRec.set_Field("CustDO", CustDO);
            if (!string.IsNullOrEmpty(ReturnReasonBattery)) DynRec.set_Field("ReturnReasonBattery", ReturnReasonBattery);
            if (!string.IsNullOrEmpty(ReturnReasonOther)) DynRec.set_Field("ReturnReasonOther", ReturnReasonOther);
            if (!string.IsNullOrEmpty(ReturnReasonBatch)) DynRec.set_Field("ReturnReasonBatch", ReturnReasonBatch);
            if (!string.IsNullOrEmpty(OtherReason)) DynRec.set_Field("OtherReason", OtherReason);
            // Jerry 20251119 - Add batch number
            if (!string.IsNullOrEmpty(batchNumber)) DynRec.set_Field("BatchNumber", batchNumber);
            // Jerry 20251119 End
        }

        private void Save_LF_WarrantyTable_Parameter(AxaptaRecord DynRec, string ClaimStatus, string ClaimType, int subClaimType, string CustAccount,
                                                     string inventLocationId, string Address, string Transport,
                                                     string TransportationReason, string Process_Status, string TransportationDate,
                                                     string GoodReceiveDate, string InvoiceCheckedDate, string InspectionDate,
                                                     string VerifiedDate, string ApprovedDate)
        {
            if (ClaimType != "") DynRec.set_Field("ClaimType", ClaimType);
            if (subClaimType != 0) DynRec.set_Field("SubClaimType", subClaimType);
            if (CustAccount != "") DynRec.set_Field("CustAccount", CustAccount);

            if (inventLocationId != "")
            {
                if (inventLocationId == "&nbsp")
                {
                    inventLocationId = "";
                }
                DynRec.set_Field("InventLocationId", inventLocationId);
            }

            if (Address != "") DynRec.set_Field("Address", Address);
            if (Transport != "")
            {
                var var_Transport = Convert.ToInt16(Transport);
                DynRec.set_Field("TransportRequired", var_Transport);
            }

            int CNrequired = Convert.ToInt16(rblCNrequired.SelectedValue);
            DynRec.set_Field("CnRequired", CNrequired);

            if (ClaimStatus != "") DynRec.set_Field("ClaimStatus", ClaimStatus);
            if (Process_Status != "") DynRec.set_Field("ProcessStatus", Process_Status);
            if (TransportationDate != "") DynRec.set_Field("TransArrangeDate", TransportationDate);
            if (GoodReceiveDate != "") DynRec.set_Field("GoodsReceivedDate", GoodReceiveDate);
            if (InvoiceCheckedDate != "") DynRec.set_Field("InvoiceCheckedDate", InvoiceCheckedDate);
            if (InspectionDate != "") DynRec.set_Field("InspectionDate", InspectionDate);
            if (VerifiedDate != "") DynRec.set_Field("VerifiedDate", VerifiedDate);
            if (ApprovedDate != "") DynRec.set_Field("ApprovedDate", ApprovedDate);
            if (txtInvoiceChkRemark.Text != "") DynRec.set_Field("InvoiceChkRemark", txtInvoiceChkRemark.Text);
            if (txtTransportationRemark.Text != "") DynRec.set_Field("TransportationReason", TransportationReason);
            if (txtGoodReceiveRemark.Text != "") DynRec.set_Field("GoodsRcRemark", txtGoodReceiveRemark.Text);
            if (txtInspectionRemark.Text != "") DynRec.set_Field("InspectionRemark", txtInspectionRemark.Text);
            if (txtVerificationRemark.Text != "") DynRec.set_Field("VerifiedRemark", txtVerificationRemark.Text);
            if (txtApproverRemark.Text != "") DynRec.set_Field("ApprovalRemark", txtApproverRemark.Text);
            /* DynRec.set_Field("RMADate", "");
            DynRec.set_Field("CNDate", "");
            */
        }

        private bool ProcessBatchItemReturn(string ReturnReasonBatch, string OtherReason, bool CheckWarrantyLine, string ClaimType,
            string CustAccount, string ClaimStatus, string ClaimNumber, string SerialNumber, string ReturnReasonBattery, string ReturnReasonOther)
        {
            if (rblWarranty.SelectedValue == "1")//batch return
            {
                ReturnReasonBatch = DropDownList_ReasonReturn_BatchItemDetails.SelectedItem.ToString();
            }
            else
            {
                ReturnReasonOther = DropDownList_ReasonReturn_OtherProducts.SelectedItem.ToString();
            }

            if (!string.IsNullOrEmpty(txtbatchReason.Text))
            {
                OtherReason = txtbatchReason.Text;
            }
            else
            {
                if (!string.IsNullOrEmpty(rblbatch.SelectedValue.ToString()))
                {
                    OtherReason = rblbatch.SelectedValue.ToString();
                }
                else
                {
                    OtherReason = ReturnReasonBatch;
                }
            }

            //check GridView_BatchItem
            var tuple_get_Batch_Item = get_Batch_Item();//get item id
            string[] Description_Batch = tuple_get_Batch_Item.Item1;
            string[] New_QTY_Batch = tuple_get_Batch_Item.Item2;
            int row_count_BatchItem = tuple_get_Batch_Item.Item3;
            string[] CustDO_Batch = tuple_get_Batch_Item.Item4;
            string[] ItemId_Batch = tuple_get_Batch_Item.Item5;
            string[] recid = tuple_get_Batch_Item.Item6;
            string[] Unit_Batch = tuple_get_Batch_Item.Item7;

            if (!string.IsNullOrEmpty(ItemId_Batch[0]))
            {
                CheckWarrantyLine = Save_LF_WarrantyLine_Batch(ClaimStatus, ClaimType, CustAccount, ClaimNumber, SerialNumber,
                    CustDO_Batch, New_QTY_Batch, Unit_Batch, ItemId_Batch, Description_Batch,
                    ReturnReasonBattery, ReturnReasonOther, ReturnReasonBatch, OtherReason, row_count_BatchItem, recid);
            }
            else
            {
                CheckWarrantyLine = false;
            }
            return CheckWarrantyLine;
        }

        public void getDetailsAndSendemail(string Process_Status, string WarrantyProcessStatus, string ClaimStatus, string SalesmanEmail, string Remark)
        {
            Function_Method.isVPPPCampaign = false;
            string Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Process_Status = WarrantyProcessStatus + ClaimStatus + " by " + Session["user_id"].ToString() + " on " + Date;
            string MailSubject = "Warranty " + ClaimStatus + ". Warranty ID: " + hidden_LabelClaimNumber.Text;
            string MailTo = Session["user_id"].ToString() + "@posim.com.my";
            string MailCC = SalesmanEmail;
            string SendMsg = "Warranty " + "(" + hidden_LabelClaimNumber.Text + ") " + " have been " + Process_Status + "." + "\n" + "\n" +
                             "Customer Acc No: " + Label_Account.Text + "\n" +
                             "Customer Name: " + Label_CustName.Text + "\n" +
                             "Warranty Id: " + hidden_LabelClaimNumber.Text + "\n" +
                             "Status: " + ClaimStatus + "\n" +
                             "Remark: " + Remark;
            Function_Method.SendMail(Session["user_id"].ToString(), Session["user_id"].ToString(), MailSubject, MailTo, MailCC, SendMsg);
        }

        private bool Save_LF_WebClaim_Transport(Axapta DynAx, string ClaimNumber, string TransportName, string VehicleNo,
                             string DriverResponsible, string Street, string State, DateTime CollectionDate,
                             string Postcode, string CollectionPoint, string ApproverRemark, int CNRequired)
        {
            try
            {
                if (lblClaimText.Text != "New" || lblClaimText.Text != "Draft" || lblClaimText.Text != "")//for submit only
                {
                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebClaim_Transport"))
                    {
                        DynAx.TTSBegin();

                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ClaimNumber",
                            ClaimNumber));
                        if (string.IsNullOrEmpty(Postcode))
                        {
                            Postcode = "0";
                        }
                        if (DynRec.Found)
                        {
                            Save_LF_WebClaim_Transport_Parameter(DynRec, ClaimNumber, TransportName, VehicleNo,
                                                         DriverResponsible, Street, State, CollectionDate,
                                                        Convert.ToInt32(Postcode), CollectionPoint, ApproverRemark, CNRequired);
                            DynRec.Call("Update");
                            logger.Info($"Updated transport details for ClaimNumber: {ClaimNumber}");
                        }
                        else
                        {
                            Save_LF_WebClaim_Transport_Parameter(DynRec, ClaimNumber, TransportName, VehicleNo,
                                                             DriverResponsible, Street, State, CollectionDate,
                                                             Convert.ToInt32(Postcode), CollectionPoint, ApproverRemark, CNRequired);
                            DynRec.Call("insert");
                            logger.Info($"Inserted new transport details for ClaimNumber: {ClaimNumber}");
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                }
                return true;
            }
            catch (Exception ER_WC_19)
            {
                logger.Error($"Error in Save_LF_WebClaim_Transport for ClaimNumber: {ClaimNumber}: {ER_WC_19}");
                Function_Method.MsgBox("ER_WC_19: " + ER_WC_19.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Dispose();
            }
        }

        private void Save_LF_WebClaim_Transport_Parameter(AxaptaRecord DynRec, string ClaimNumber, string TransportName, string VehicleNo,
                             string DriverResponsible, string Street, string State, DateTime CollectionDate,
                             int Postcode, string CollectionPoint, string ApproverRemark, int CNRequired)
        {
            if (ClaimNumber != "") DynRec.set_Field("ClaimNumber", ClaimNumber);
            if (TransportName != "") DynRec.set_Field("TransporterName", TransportName);
            if (VehicleNo != "") DynRec.set_Field("VehicleNo", VehicleNo);
            if (DriverResponsible != "") DynRec.set_Field("DriverResponsible", DriverResponsible);
            if (Street != "") DynRec.set_Field("Street", Street);
            if (State != "") DynRec.set_Field("State", State);

            DynRec.set_Field("CollectionDate", CollectionDate);

            if (Postcode != 0) DynRec.set_Field("Postcode", Postcode);
            if (CollectionPoint != "") DynRec.set_Field("CollectionPoint", CollectionPoint);

            if (ApproverRemark != "") DynRec.set_Field("ApproverRemark", ApproverRemark);
            //DynRec.set_Field("CNrequired", CNRequired);
        }

        public static Tuple<string, string, int> getWarrantyLineUnitQty(Axapta DynAx, string itemID)
        {
            string ItemID = "";
            string UnitID = "";
            int Qty = 0;

            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyLine"))
            {
                DynAx.TTSBegin();
                DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "ItemID", itemID));
                if (DynRec.Found)
                {
                    ItemID = DynRec.get_Field("ItemID").ToString();
                    UnitID = DynRec.get_Field("UnitID").ToString();
                    Qty = Convert.ToInt16(DynRec.get_Field("Qty"));
                }
                DynAx.TTSAbort();
            }

            return new Tuple<string, string, int>(ItemID, UnitID, Qty);
        }

        private void reinsert_routine_batch(Axapta DynAx, AxaptaRecord DynRec, string ClaimNumber, int row_count_BatchItem)
        {
            for (int j = 0; j < row_count_BatchItem; j++)
            {
                DynAx.TTSBegin();
                DynRec.ExecuteStmt(string.Format("delete_from %1 where %1.{0} == '{1}'", "ClaimNumber", ClaimNumber));

                DynAx.TTSCommit();
                DynAx.TTSAbort();
            }
        }

        public void Save_LF_WarrantyBattery(Axapta DynAx, string ClaimID, string CCA, string LoadTest,
                                     string TestResult, string MagicEye, string ImgPath)
        {
            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WarrantyBattery"))
                {
                    DynAx.TTSBegin();

                    if (ClaimID != "") DynRec.set_Field("ClaimNumber", ClaimID);
                    if (CCA != "") DynRec.set_Field("CCA", CCA);
                    if (LoadTest != "") DynRec.set_Field("LoadTest", LoadTest);
                    if (TestResult != "") DynRec.set_Field("TestResult", TestResult);
                    if (MagicEye != "") DynRec.set_Field("MagicEye", MagicEye);
                    if (ImgPath != "") DynRec.set_Field("ImgPath", ImgPath);

                    DynRec.Call("insert");

                    DynAx.TTSCommit();
                    DynAx.TTSAbort();
                }
            }
            catch (Exception ER_WB_01)
            {
                Function_Method.MsgBox("ER_WB_01: " + ER_WB_01.ToString(), this.Page, this);
            }
        }

        //private void Alt_Addr_function()
        //{
        //    Button_Delivery_Addr.Visible = false;
        //    hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";

        //    Axapta DynAx = Function_Method.GlobalAxapta();

        //    try
        //    {
        //        GLOBAL.Company = GLOBAL.switch_Company;
        //        DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
        //            (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //        var tuple_get_AltAddress = SFA_GET_SALES_HEADER.get_AltAddress(DynAx, Label_Account.Text);
        //        if (tuple_get_AltAddress == null)
        //        {
        //            return;
        //        }
        //        string[] AltAddress = tuple_get_AltAddress.Item1;
        //        string[] AltAddressRecId = tuple_get_AltAddress.Item2;
        //        string[] DefaultAddress = tuple_get_AltAddress.Item4;
        //        int Counter = tuple_get_AltAddress.Item3;

        //        if (Counter == 1)//only one data
        //        {
        //            if (AltAddress[0] == Label_Address.Text)//same as primary, no alt address
        //            {
        //                return;
        //            }
        //        }
        //        int temp_count = 0;
        //        for (int i = 0; i < Counter; i++)
        //        {
        //            if (AltAddress[i] == hidden_Street.Text)
        //            {
        //                //skip
        //            }
        //            else
        //            {
        //                if (temp_count == 0)//first count
        //                {
        //                    hidden_alt_address_RecId.Text = AltAddressRecId[i];
        //                    hidden_alt_address.Text = AltAddress[i];
        //                }
        //                else
        //                {
        //                    hidden_alt_address_RecId.Text = hidden_alt_address_RecId.Text + "|" + AltAddressRecId[i];
        //                    hidden_alt_address.Text = hidden_alt_address.Text + "|" + AltAddress[i];
        //                }
        //                temp_count = temp_count + 1;
        //            }
        //        }

        //        hidden_alt_address_counter.Text = temp_count.ToString();
        //        Button_Delivery_Addr.Visible = true;
        //    }
        //    catch (Exception ER_SF_02)
        //    {
        //        Function_Method.MsgBox("ER_SF_02: " + ER_SF_02.ToString(), this.Page, this);
        //    }
        //    finally
        //    {
        //        DynAx.Logoff();
        //    }
        //}

        //protected void CheckBox_Changed2(object sender, EventArgs e)
        //{
        //    int counter = 0;
        //    Axapta DynAx = new Axapta();
        //    GLOBAL.Company = GLOBAL.switch_Company;
        //    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
        //        (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

        //    foreach (GridViewRow row in GridView_AltAddress.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            CheckBox CheckBox_c = (row.Cells[0].FindControl("chkRow2") as CheckBox);

        //            if (CheckBox_c.Checked)
        //            {
        //                string selected_address = row.Cells[1].Text;

        //                if (selected_address == Label_Address.Text)//added as a precaution
        //                {
        //                    Label_Delivery_Addr.Text = "-same as primary address-";
        //                }
        //                else
        //                {
        //                    Label_Delivery_Addr.Text = selected_address;
        //                }
        //                //need to update hidden field

        //                string temp_RecID_List = hidden_alt_address_RecId.Text;
        //                string[] arr_temp_RecID_List = temp_RecID_List.Split('|');
        //                var tuple_get_AltAddress_info = SFA_GET_SALES_HEADER.get_AltAddress_info(DynAx, arr_temp_RecID_List[counter]);

        //                hidden_Street.Text = tuple_get_AltAddress_info.Item1;
        //                //hidden_ZipCode.Text = tuple_get_AltAddress_info.Item2;
        //                //hidden_City.Text = tuple_get_AltAddress_info.Item3;
        //                //hidden_State.Text = tuple_get_AltAddress_info.Item4;
        //                //hidden_Country.Text = tuple_get_AltAddress_info.Item5;

        //                DynAx.Logoff();

        //                GridView_AltAddress.Visible = false;
        //                Button_Delivery_Addr.Text = "Alt. Addr.";
        //                //row.BackColor = Color.FromName("#ff8000");
        //            }
        //        }
        //        counter = counter + 1;
        //    }
        //}

    }
}