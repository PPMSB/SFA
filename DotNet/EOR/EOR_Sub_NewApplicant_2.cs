using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class EOR : System.Web.UI.Page
    {
        private void clear_parameter_NewApplication_CustomerInfo()
        {
            // <!--Customer Info ////////////////////////////////////////////////////////////////////////////////////-->

            TextBox_Account.Text = "";
            Label_HQ.Text = "";
            Label_Salesman.Text = "";
            Label_Address.Text = "";

            Label_CustName.Text = "";
            Label_TelNo.Text = "";
            Label_ContactPerson.Text = "";
            Label_Class.Text = "";
            Label_AccOpenedDate.Text = "";

            //RadioButton1.Checked = false;
            //RadioButton2.Checked = false;
            //RadioButton10.Checked = false;
            //RadioButton11.Checked = false;
            //RadioButton12.Checked = false;
            hidden_duration_year_month.Text = "";

            hidden_customer_class.Text = "";
            hidden_duration_rate.Text = "";

            //RadioButton13.Checked = false;
            //RadioButton14.Checked = false;
            //RadioButton15.Checked = false;
            hidden_customer_type.Text = "";

            div_CustInfoExtra.Visible = false;

            hidden_Label_EquipmentRecId.Text = "";
            hidden_Label_EquipmentRecId_Update.Text = "";
            hidden_Label_SaveDraft.Text = "";

            hidden_NA_HOD.Text = "";
            hidden_NA_Admin.Text = "";
            hidden_NA_Manager.Text = "";
            hidden_NA_GM.Text = "";
            hidden_NextApproval.Text = "";
            hidden_NextApprovalAlt.Text = "";
            hidden_DocStatus.Text = "";
            ButtonAdd_Equipment.Enabled = true;

            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";
            hidden_Street.Text = ""; hidden_ZipCode.Text = ""; hidden_City.Text = "";
            hidden_State.Text = ""; hidden_Country.Text = "";

            GridView_AltAddress.DataSource = null; GridView_AltAddress.DataBind(); GridView_AltAddress.Visible = false;
            Button_Delivery_Addr.Visible = false;

            TextBox_Requested.Enabled = false;
        }

        private void clear_parameter_NewApplication_EORPart()
        {
            //Part1

            TextBox_ServiceCentre.Text = "";
            TextBox_Facilities.Text = "";

            TextBox_WorkshopSize.Text = "";
            TextBox_Mechanics.Text = "";
            TextBox_WorkshopStatus.Text = "";
            TextBox_Establishment.Text = "";

            TextBox_RemarksSalesHOD.Text = "";
            DropDownList_Experience.ClearSelection();

            if (TextBox_Requested.ReadOnly == false)
            {
                TextBox_Requested.Text = GLOBAL.logined_user_name;
            }

            GridView_Equipment.DataSource = null;
            GridView_Equipment.DataBind();
            initiallize_GridView_Equipment();

            GridView_ProposedProduct.DataSource = null;
            GridView_ProposedProduct.DataBind();
            initiallize_GridView_ProposedProduct();

            //Part2
            TextBox_DateApproval.Text = "";
            TextBox_RemarksApproval.Text = "";
            TextBox_Approved.Text = "";
            GridView_Past_MonthRecords.DataSource = null;
            GridView_Past_MonthRecords.DataBind();

            //Admin

            GridView_EquipmentAdmin.DataSource = null;
            GridView_EquipmentAdmin.DataBind();

            hidden_EORCarton.Text = "";
            hidden_EORPoint.Text = "";
            Label_MonthlyTarget.Text = "";

            TextBox_RemarksAdmin.Text = "";
            Label_DurationContract_Admin.Text = "";
        }

        //===========================================================================
        protected void HideAccordion_CustInfo(object sender, EventArgs e)
        {
            if (Accordion_CustInfo.Text == "Customer Info (maximized)")
            {
                Show_Hide_NewApplication_CustomerInfo(false);

            }
            else if (Accordion_CustInfo.Text == "Customer Info (minimized)")
            {
                Show_Hide_NewApplication_CustomerInfo(true);
            }
        }

        private void Show_Hide_NewApplication_CustomerInfo(bool paramater)
        {
            if (paramater == true)//show
            {
                new_applicant_section_CustomerInfo.Attributes.Add("style", "display:initial");
                Accordion_CustInfo.Text = "Customer Info (maximized)";
            }
            else
            {
                new_applicant_section_CustomerInfo.Attributes.Add("style", "display:none");
                Accordion_CustInfo.Text = "Customer Info (minimized)";
            }
        }
        //===========================================================================
        protected void Hide_Accordion_EORPart1(object sender, EventArgs e)
        {
            if (Accordion_EORPart1.Text == "EOR:Part 1 (maximized)")
            {
                Show_Hide_NewApplication_EORPart1(false);

            }
            else if (Accordion_EORPart1.Text == "EOR:Part 1 (minimized)")
            {
                Show_Hide_NewApplication_EORPart1(true);
            }

        }

        private void Show_Hide_NewApplication_EORPart1(bool paramater)
        {
            if (paramater == true)
            {
                new_applicant_section_EORPart1.Attributes.Add("style", "display:initial");

                Accordion_EORPart1.Text = "EOR:Part 1 (maximized)";

                if (GridView_Past_MonthRecords.Rows.Count == 0)//if already exist, dont need (faster execution time) because during validate wil reset
                {
                    Past_MonthRecords();
                }
            }
            else
            {
                new_applicant_section_EORPart1.Attributes.Add("style", "display:none");

                Accordion_EORPart1.Text = "EOR:Part 1 (minimized)";
            }
        }
        //===========================================================================
        protected void Hide_Accordion_EORPart2(object sender, EventArgs e)
        {
            if (Accordion_EORPart2.Text == "Approval Control (maximized)")
            {
                Show_Hide_NewApplication_EORPart2(false);

            }
            else if (Accordion_EORPart2.Text == "Approval Control (minimized)")
            {
                Show_Hide_NewApplication_EORPart2(true);
            }

        }
        private void Show_Hide_NewApplication_EORPart2(bool paramater)
        {
            if (paramater == true)
            {
                new_applicant_section_EORPart2.Attributes.Add("style", "display:initial");

                Accordion_EORPart2.Text = "Approval Control (maximized)";
            }
            else
            {
                new_applicant_section_EORPart2.Attributes.Add("style", "display:none");

                Accordion_EORPart2.Text = "Approval Control (minimized)";
            }
        }
        //===========================================================================

        //CustInfo //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void CheckAcc(object sender, EventArgs e)
        {
            validate();
            Alt_Addr_function();
        }

        private void validate()
        {
            string CustAcc = TextBox_Account.Text.Trim();
            clear_parameter_NewApplication_CustomerInfo(); Show_Hide_NewApplication_CustomerInfo(true);
            clear_parameter_NewApplication_EORPart(); Show_Hide_NewApplication_EORPart1(false);
            Accordion_EORPart1.Visible = false;
            TextBox_Account.Text = CustAcc;//after clear all, rewrite back

            if (CustAcc == "")
            {
                Function_Method.MsgBox("There is no account number", this.Page, this);
                return;
            }
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                var tuple_getCustInfo = EOR_GET_NewApplicant.getCustInfo(DynAx, CustAcc);
                string CustName = tuple_getCustInfo.Item1;
                string Address = tuple_getCustInfo.Item2;
                string temp_EmplId = tuple_getCustInfo.Item3;
                string temp_EmpName = tuple_getCustInfo.Item4;
                string BranchID = tuple_getCustInfo.Item5;//dimension
                string CustomerClass = tuple_getCustInfo.Item6.Trim();
                string getOpeningAccDate = tuple_getCustInfo.Item7;
                if (CustName == "")
                {
                    Function_Method.MsgBox("No data found", this.Page, this);
                    return;
                }
                else
                {
                    string ClassDesc = EOR_GET_NewApplicant.getClassDetails(DynAx, CustomerClass);

                    Label_HQ.Text = BranchID;
                    Label_Salesman.Text = "(" + temp_EmplId + ") " + temp_EmpName;
                    Label_Address.Text = Address;
                    Label_CustName.Text = CustName;
                    Label_AccOpenedDate.Text = getOpeningAccDate;

                    Label_Class.Text = CustomerClass + " (" + ClassDesc + ")";
                    hidden_customer_class.Text = CustomerClass;

                    var tuple_getCustInfo_2 = EOR_GET_NewApplicant.getCustInfo_2(DynAx, CustAcc);
                    string CustomerContactId = tuple_getCustInfo_2.Item1;
                    string CustTelNo = tuple_getCustInfo_2.Item2;
                    if (CustomerContactId != "")
                    {
                        string ContactName = EOR_GET_NewApplicant.getContactPersonName(DynAx, CustomerClass);
                        Label_ContactPerson.Text = ContactName;
                    }
                    if (Label_ContactPerson.Text == "") Label_ContactPerson.Text = CustName;

                    Label_TelNo.Text = CustTelNo;

                    string CustStreet = ""; string CustZipCode = ""; string CustCity = ""; string CustState = ""; string CustCountry = "";
                    hidden_Street.Text = CustStreet;
                    hidden_ZipCode.Text = CustZipCode;
                    hidden_City.Text = CustCity;
                    hidden_State.Text = CustState;
                    hidden_Country.Text = CustCountry;

                    div_CustInfoExtra.Visible = true;
                }
                DynAx.Logoff();
            }
            catch (Exception ER_EO_00)
            {
                Function_Method.MsgBox("ER_EO_00: " + ER_EO_00.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_EOCM@";//EOR > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        //===========================================================================
        private void duration_rate()
        {
            if (rblContractDuration.SelectedValue == "1")//Duration 1 year
            {
                hidden_duration_year_month.Text = "12";
                hidden_duration_rate.Text = "1.05";// 5 percent
            }
            if (rblContractDuration.SelectedValue == "2")//Duration 2 years
            {
                hidden_duration_year_month.Text = "24";
                hidden_duration_rate.Text = "1.10";// 10 percent
            }
            if (rblContractDuration.SelectedValue == "3")//Duration 3 years
            {
                hidden_duration_year_month.Text = "36";
                hidden_duration_rate.Text = "1.15";// 15 percent
            }
            if (rblContractDuration.SelectedValue == "4")//Duration 4 years
            {
                hidden_duration_year_month.Text = "48";
                hidden_duration_rate.Text = "1.20";// 20 percent
            }
            if (rblContractDuration.SelectedValue == "5")//Duration 5 years
            {
                hidden_duration_year_month.Text = "60";
                hidden_duration_rate.Text = "1.25";// 25 percent
            }
        }

        protected void RadioButtonChanged_duration(object sender, EventArgs e)
        {
            duration_rate();
            if (hidden_customer_type.Text == "")
            {
                //wait for type;
            }
            else
            {
                //clear_parameter_NewApplication_EORPart();
                Accordion_EORPart1.Visible = true;
                Show_Hide_NewApplication_EORPart1(true);
            }

        }

        protected void RadioButtonChanged_CustType(object sender, EventArgs e)
        {
            if (rblRequestType.SelectedValue == "1")//New
            {
                hidden_customer_type.Text = "New";

            }
            if (rblRequestType.SelectedValue == "2")//Existing
            {
                hidden_customer_type.Text = "Existing";
            }
            if (rblRequestType.SelectedValue == "3")//Branch
            {
                hidden_customer_type.Text = "Branch";
            }
            if (hidden_duration_year_month.Text == "")
            {
                //wait for duration_year_month;
            }
            else
            {
                //clear_parameter_NewApplication_EORPart();
                Accordion_EORPart1.Visible = true;
                Show_Hide_NewApplication_EORPart1(true);
            }
        }

        //EOR Part 1 //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        //GridView_Equipment
        private void initiallize_GridView_Equipment()
        {
            GridView_Equipment.Columns[6].Visible = true;//RecId
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("Deposit", typeof(string)));
            dt.Columns.Add(new DataColumn("Carton", typeof(string)));
            dt.Columns.Add(new DataColumn("RecId", typeof(string)));
            dr = dt.NewRow();

            dr["No."] = 1;
            dr["Description"] = string.Empty;
            dr["Qty"] = string.Empty;
            dr["Deposit"] = string.Empty;
            dr["Carton"] = string.Empty;
            dr["RecId"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;
            GridView_Equipment.DataSource = dt;
            GridView_Equipment.DataBind();
            GridView_Equipment.Columns[6].Visible = false;//RecId
        }


        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            GridView_Equipment.Columns[6].Visible = true;//RecId
            AddNewRowToGrid();
            GridView_Equipment.Columns[6].Visible = false;//RecId
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
                        TextBox box1 = (TextBox)GridView_Equipment.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_Equipment.Rows[rowIndex].Cells[1].FindControl("TextBox_Description");

                        TextBox box3 = (TextBox)GridView_Equipment.Rows[rowIndex].Cells[4].FindControl("TextBox_DepositEquipment");
                        Label box4 = (Label)GridView_Equipment.Rows[rowIndex].Cells[5].FindControl("Label_CartonEquipment");
                        Label box5 = (Label)GridView_Equipment.Rows[rowIndex].Cells[6].FindControl("Label_RecIdEquipment");

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["No."] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Description"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Qty"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Deposit"] = box3.Text;
                        dtCurrentTable.Rows[i - 1]["Carton"] = box4.Text;
                        dtCurrentTable.Rows[i - 1]["RecId"] = box5.Text;

                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView_Equipment.DataSource = dtCurrentTable;
                    GridView_Equipment.DataBind();
                }
                else
                {
                    initiallize_GridView_Equipment();
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

                        TextBox box1 = (TextBox)GridView_Equipment.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_Equipment.Rows[rowIndex].Cells[1].FindControl("TextBox_Description");

                        TextBox box3 = (TextBox)GridView_Equipment.Rows[rowIndex].Cells[4].FindControl("TextBox_DepositEquipment");
                        Label box4 = (Label)GridView_Equipment.Rows[rowIndex].Cells[5].FindControl("Label_CartonEquipment");
                        Label box5 = (Label)GridView_Equipment.Rows[rowIndex].Cells[6].FindControl("Label_RecIdEquipment");

                        box1.Text = dt.Rows[i]["Qty"].ToString();
                        box2.Text = dt.Rows[i]["Description"].ToString();

                        box3.Text = dt.Rows[i]["Deposit"].ToString();
                        box4.Text = dt.Rows[i]["Carton"].ToString();
                        box5.Text = dt.Rows[i]["RecId"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        protected void TextBox_DescriptionEquipment_Changed(object sender, EventArgs e)
        {
            //get the the id that call the function
            TextBox TextBox_DescriptionEquipment = sender as TextBox;

            string ClientID = TextBox_DescriptionEquipment.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            GridViewRow row = (GridViewRow)TextBox_DescriptionEquipment.NamingContainer;
            int index = row.RowIndex;
            //int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            //GridViewRow row = (GridViewRow)(sender as TextBox).NamingContainer;

            //string index = GridView_Equipment.DataKeys[row.RowIndex].Values[0].ToString();
            //
            TextBox box1 = (TextBox)GridView_Equipment.Rows[index].Cells[3].FindControl("TextBox_New_QTY");
            //TextBox box2 = (TextBox)GridView_Equipment.Rows[ClientRow].Cells[1].FindControl("TextBox_Description");
            TextBox box3 = (TextBox)GridView_Equipment.Rows[index].Cells[4].FindControl("TextBox_DepositEquipment");
            Label box5 = (Label)GridView_Equipment.Rows[index].Cells[6].FindControl("Label_RecIdEquipment");
            Label box4 = (Label)GridView_Equipment.Rows[index].Cells[5].FindControl("Label_CartonEquipment");

            //clear when Description change
            box1.Text = "";//TextBox_New_QTY
            box3.Text = "";//TextBox_DepositEquipment
            box4.Text = "";//Label_CartonEquipment
            box5.Text = "";//Label_RecIdEquipment
        }
        protected void TextBox_DepositEquipment_Changed(object sender, EventArgs e)
        {
            //get the the id that call the function
            TextBox TextBox_DepositEquipment = sender as TextBox;

            string ClientID = TextBox_DepositEquipment.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            //int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)TextBox_DepositEquipment.NamingContainer;
            int index = row.RowIndex;
            //
            TextBox box1 = (TextBox)GridView_Equipment.Rows[index].Cells[3].FindControl("TextBox_New_QTY");
            TextBox box2 = (TextBox)GridView_Equipment.Rows[index].Cells[1].FindControl("TextBox_Description");
            TextBox box3 = (TextBox)GridView_Equipment.Rows[index].Cells[4].FindControl("TextBox_DepositEquipment");
            Label box5 = (Label)GridView_Equipment.Rows[index].Cells[6].FindControl("Label_RecIdEquipment");
            Label box4 = (Label)GridView_Equipment.Rows[index].Cells[5].FindControl("Label_CartonEquipment");

            //clear when Description change
            box4.Text = "";//Label_CartonEquipment
            //
            string EquipmentRecId = box5.Text;
            if (EquipmentRecId != "")//Only selected
            {
                //to get propose carton and deposit
                string raw_Salesman = Label_Salesman.Text;
                string SalesmanNo = "";
                if (raw_Salesman != "" && EquipmentRecId != "")
                {
                    string[] arr_temp_SalesmanNo_Name = raw_Salesman.Split(')');
                    SalesmanNo = arr_temp_SalesmanNo_Name[0].Substring(1);

                    Axapta DynAx = new Axapta();
                    GLOBAL.Company = GLOBAL.switch_Company;
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    var tuple_getItemInfo_from_RecId = EOR_GET_NewApplicant.getItemInfo_from_RecId(DynAx, EquipmentRecId);
                    string EquipmentItemId = tuple_getItemInfo_from_RecId.Item1;
                    string ContractDuration = hidden_duration_year_month.Text;
                    var tuple_Check_EOR_Carton_List_Auto = EOR_GET_NewApplicant.Check_EOR_Carton_List_Auto(DynAx, EquipmentRecId, SalesmanNo, EquipmentItemId, ContractDuration);
                    string Suggest_CartonDep = tuple_Check_EOR_Carton_List_Auto.Item1;
                    string Suggest_CartonNoDep = tuple_Check_EOR_Carton_List_Auto.Item2;
                    string Suggest_WithDeposit = tuple_Check_EOR_Carton_List_Auto.Item3;
                    if (box3.Text == Suggest_WithDeposit)
                    {
                        box4.Text = Suggest_CartonDep;
                    }
                    else
                    {
                        box4.Text = Suggest_CartonNoDep;
                    }
                }
            }
        }

        protected void Button_SearchEquipment_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                //get the the id that call the function
                Button Button_SearchEquipment = sender as Button;

                string ClientID = Button_SearchEquipment.ClientID;

                string[] arr_ClientID = ClientID.Split('_');
                //int arr_count = arr_ClientID.Count();
                //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                GridViewRow row = (GridViewRow)Button_SearchEquipment.NamingContainer;
                int index = row.RowIndex;
                //
                TextBox box2 = (TextBox)GridView_Equipment.Rows[index].Cells[1].FindControl("TextBox_Description");
                string Description_toSearch = box2.Text.Trim();
                //if (Description_toSearch != "")
                //{
                DropDownList Dropdown_SearchEquipment = (DropDownList)GridView_Equipment.Rows[index].Cells[1].FindControl("DropDownList_SearchEquipment");
                Dropdown_SearchEquipment.Visible = true;
                box2.Visible = false;

                Dropdown_SearchEquipment.Items.Clear();
                Description_toSearch = "*" + Description_toSearch + "*";
                List<ListItem> List_SearchEquipment = EOR_GET_NewApplicant.get_SearchEquipment(DynAx, Description_toSearch, "");
                if (List_SearchEquipment.Count > 1)
                {
                    Dropdown_SearchEquipment.Items.AddRange(List_SearchEquipment.ToArray());
                }
                else
                {
                    Dropdown_SearchEquipment.Visible = false;
                    box2.Visible = true;
                }

                //}
            }
            catch (Exception ER_EO_11)
            {
                Function_Method.MsgBox("ER_EO_11: " + ER_EO_11.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void OnSelectedIndexChanged_DropDownList_SearchEquipment(object sender, EventArgs e)
        {
            //get the the id that call the function
            DropDownList Dropdown_SearchEquipment = sender as DropDownList;

            string ClientID = Dropdown_SearchEquipment.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            //int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
            GridViewRow row = (GridViewRow)Dropdown_SearchEquipment.NamingContainer;
            int index = row.RowIndex;
            //
            TextBox box2 = (TextBox)GridView_Equipment.Rows[index].Cells[1].FindControl("TextBox_Description");

            string EquipmentItemId_RecId = Dropdown_SearchEquipment.SelectedItem.Value;
            string[] arr_EquipmentItemId_RecId = EquipmentItemId_RecId.Split('|');
            string EquipmentItemId = arr_EquipmentItemId_RecId[0];
            string EquipmentRecId = arr_EquipmentItemId_RecId[1];
            if (EquipmentItemId != "")//Only selected
            {
                box2.Text = Dropdown_SearchEquipment.SelectedItem.ToString();
                Dropdown_SearchEquipment.Items.Clear();
                Dropdown_SearchEquipment.Visible = false;
                box2.Visible = true;
                //
                Label box5 = (Label)GridView_Equipment.Rows[index].Cells[5].FindControl("Label_RecIdEquipment");
                box5.Text = EquipmentRecId;

                //to get propose carton and deposit
                string raw_Salesman = Label_Salesman.Text;
                string SalesmanNo = "";
                if (raw_Salesman != "" && EquipmentRecId != "")
                {
                    string[] arr_temp_SalesmanNo_Name = raw_Salesman.Split(')');
                    SalesmanNo = arr_temp_SalesmanNo_Name[0].Substring(1);

                    Axapta DynAx = new Axapta();
                    GLOBAL.Company = GLOBAL.switch_Company;
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    string ContractDuration = hidden_duration_year_month.Text;
                    var tuple_Check_EOR_Carton_List_Auto = EOR_GET_NewApplicant.Check_EOR_Carton_List_Auto(DynAx, EquipmentRecId, SalesmanNo, EquipmentItemId, ContractDuration);
                    string Suggest_Carton = tuple_Check_EOR_Carton_List_Auto.Item1;
                    string Suggest_Deposit = tuple_Check_EOR_Carton_List_Auto.Item3;

                    TextBox box3 = (TextBox)GridView_Equipment.Rows[index].Cells[4].FindControl("TextBox_DepositEquipment");
                    Label box4 = (Label)GridView_Equipment.Rows[index].Cells[5].FindControl("Label_CartonEquipment");

                    box3.Text = Suggest_Deposit;
                    box4.Text = Suggest_Carton;
                }
            }
            else
            {
                //do nothing
            }
        }

        protected void GridView_Equipment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if ((hidden_DocStatus.Text == "0") || (hidden_DocStatus.Text == "1") || (hidden_DocStatus.Text == "2"))//draft, HOD, and SalesAdmin can delete to prevent loss of data in equipment cost , carton, .. keyin by EOR Admin
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

                        TextBox box1 = (TextBox)GridView_Equipment.Rows[currentNo - 1].Cells[2].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_Equipment.Rows[currentNo - 1].Cells[1].FindControl("TextBox_Description");

                        TextBox box3 = (TextBox)GridView_Equipment.Rows[currentNo - 1].Cells[4].FindControl("TextBox_DepositEquipment");
                        Label box4 = (Label)GridView_Equipment.Rows[currentNo - 1].Cells[5].FindControl("Label_CartonEquipment");
                        Label box5 = (Label)GridView_Equipment.Rows[currentNo - 1].Cells[6].FindControl("Label_RecIdEquipment");

                        dt.Rows[i]["Qty"] = box1.Text;
                        dt.Rows[i]["Description"] = box2.Text;
                        dt.Rows[i]["Deposit"] = box3.Text;
                        dt.Rows[i]["Carton"] = box4.Text;
                        dt.Rows[i]["RecId"] = box5.Text;
                        dt.Rows[i]["No."] = (i + 1).ToString();
                    }
                    //=====================================================================

                    GridView_Equipment.DataSource = dt;
                    GridView_Equipment.DataBind();
                    ViewState["CurrentTable"] = dt;
                    //to set back data for textbox for viewing
                    for (int i = 0; i < row_count; i++)
                    {
                        TextBox box1 = (TextBox)GridView_Equipment.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_Equipment.Rows[i].Cells[1].FindControl("TextBox_Description");
                        TextBox box3 = (TextBox)GridView_Equipment.Rows[i].Cells[4].FindControl("TextBox_DepositEquipment");
                        Label box4 = (Label)GridView_Equipment.Rows[i].Cells[5].FindControl("Label_CartonEquipment");
                        Label box5 = (Label)GridView_Equipment.Rows[i].Cells[6].FindControl("Label_RecIdEquipment");

                        box1.Text = dt.Rows[i]["Qty"].ToString();
                        box2.Text = dt.Rows[i]["Description"].ToString();

                        box3.Text = dt.Rows[i]["Deposit"].ToString();
                        box4.Text = dt.Rows[i]["Carton"].ToString();
                        box5.Text = dt.Rows[i]["RecId"].ToString();
                    }
                }
            }
            else
            {
                Function_Method.MsgBox("Only during draft, Salesman HOD and EOR admin allowed to delete.", this.Page, this);
            }
        }
        // ==================================================================================================================================================================================================================
        //GridView_ProposedProduct
        // ==================================================================================================================================================================================================================
        protected void Button_SearchProposedProduct_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                //get the the id that call the function
                Button Button_SearchProposedProduct = sender as Button;

                string ClientID = Button_SearchProposedProduct.ClientID;

                string[] arr_ClientID = ClientID.Split('_');
                //int arr_count = arr_ClientID.Count();
                //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                GridViewRow row = (GridViewRow)Button_SearchProposedProduct.NamingContainer;
                int index = row.RowIndex;
                //
                TextBox box2 = (TextBox)GridView_ProposedProduct.Rows[index].Cells[1].FindControl("TextBox_Description");
                string Description_toSearch = box2.Text.Trim();
                if (Description_toSearch != "")
                {
                    DropDownList DropDownList_SearchProposedProduct = (DropDownList)GridView_ProposedProduct.Rows[index].Cells[1].FindControl("DropDownList_SearchProposedProduct");
                    DropDownList_SearchProposedProduct.Visible = true;
                    box2.Visible = false;

                    DropDownList_SearchProposedProduct.Items.Clear();
                    Description_toSearch = "*" + Description_toSearch + "*";
                    List<ListItem> List_SearchEquipment = EOR_GET_NewApplicant.get_SearchEquipment(DynAx, Description_toSearch, "LUBE");
                    int counter_list = List_SearchEquipment.Count();
                    if (counter_list > 1)
                    {
                        DropDownList_SearchProposedProduct.Items.AddRange(List_SearchEquipment.ToArray());
                    }
                    else
                    {
                        DropDownList_SearchProposedProduct.Visible = false;
                        box2.Visible = true;
                    }
                }
            }
            catch (Exception ER_EO_12)
            {
                Function_Method.MsgBox("ER_EO_12: " + ER_EO_12.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void OnSelectedIndexChanged_DropDownList_SearchProposedProduct(object sender, EventArgs e)
        {
            //get the the id that call the function
            DropDownList DropDownList_SearchProposedProduct = sender as DropDownList;

            string ClientID = DropDownList_SearchProposedProduct.ClientID;

            string[] arr_ClientID = ClientID.Split('_');
            //int arr_count = arr_ClientID.Count();
            //int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);

            GridViewRow row = (GridViewRow)DropDownList_SearchProposedProduct.NamingContainer;
            int index = row.RowIndex;
            //
            TextBox box2 = (TextBox)GridView_ProposedProduct.Rows[index].Cells[1].FindControl("TextBox_Description");

            if (DropDownList_SearchProposedProduct.SelectedItem.Value != "")//Only selected
            {
                box2.Text = DropDownList_SearchProposedProduct.SelectedItem.ToString();
                DropDownList_SearchProposedProduct.Items.Clear();
                DropDownList_SearchProposedProduct.Visible = false;
                box2.Visible = true;
            }
            else
            {
                //do nothing
            }
        }

        private void initiallize_GridView_ProposedProduct()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));

            dr = dt.NewRow();

            dr["No."] = 1;
            dr["Description"] = string.Empty;
            dr["Qty"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CurrentTable_ProposedProduct"] = dt;
            GridView_ProposedProduct.DataSource = dt;
            GridView_ProposedProduct.DataBind();
        }

        protected void ButtonAdd_ProposedProduct_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid_ProposedProduct();
        }

        private void AddNewRowToGrid_ProposedProduct()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable_ProposedProduct"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable_ProposedProduct"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //Extract the  values
                        TextBox box1 = (TextBox)GridView_ProposedProduct.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_ProposedProduct.Rows[rowIndex].Cells[1].FindControl("TextBox_Description");

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["No."] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Description"] = box2.Text;
                        dtCurrentTable.Rows[i - 1]["Qty"] = box1.Text;

                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable_ProposedProduct"] = dtCurrentTable;
                    GridView_ProposedProduct.DataSource = dtCurrentTable;
                    GridView_ProposedProduct.DataBind();
                }
                else
                {
                    initiallize_GridView_ProposedProduct();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks

            SetPreviousData_ProposedProduct();
        }

        private void SetPreviousData_ProposedProduct()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable_ProposedProduct"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable_ProposedProduct"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)GridView_ProposedProduct.Rows[rowIndex].Cells[3].FindControl("TextBox_New_QTY");
                        TextBox box2 = (TextBox)GridView_ProposedProduct.Rows[rowIndex].Cells[1].FindControl("TextBox_Description");

                        box1.Text = dt.Rows[i]["Qty"].ToString();
                        box2.Text = dt.Rows[i]["Description"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        protected void GridView_ProposedProduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["CurrentTable_ProposedProduct"] != null)
            {
                /*
                DataTable dt = (DataTable)ViewState["CurrentTable_ProposedProduct"];
                dt.Rows.RemoveAt(e.RowIndex);
                GridView_ProposedProduct.DataSource = dt;
                GridView_ProposedProduct.DataBind();
                */

                DataTable dt = (DataTable)ViewState["CurrentTable_ProposedProduct"];
                dt.Rows.RemoveAt(e.RowIndex);

                //=====================================================================
                //to set back row number record textbox data
                int row_count = dt.Rows.Count;
                for (int i = 0; i < row_count; i++)
                {
                    int currentNo = Convert.ToInt32(dt.Rows[i]["No."]);

                    TextBox box1 = (TextBox)GridView_ProposedProduct.Rows[currentNo - 1].Cells[3].FindControl("TextBox_New_QTY");
                    TextBox box2 = (TextBox)GridView_ProposedProduct.Rows[currentNo - 1].Cells[1].FindControl("TextBox_Description");

                    dt.Rows[i]["Qty"] = box1.Text;
                    dt.Rows[i]["Description"] = box2.Text;
                    dt.Rows[i]["No."] = (i + 1).ToString();
                }
                //=====================================================================
                GridView_ProposedProduct.DataSource = dt;
                GridView_ProposedProduct.DataBind();
                ViewState["CurrentTable"] = dt;
                //to set back data for textbox for viewing
                for (int i = 0; i < row_count; i++)
                {
                    TextBox box1 = (TextBox)GridView_ProposedProduct.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                    TextBox box2 = (TextBox)GridView_ProposedProduct.Rows[i].Cells[1].FindControl("TextBox_Description");
                    box1.Text = dt.Rows[i]["Qty"].ToString();
                    box2.Text = dt.Rows[i]["Description"].ToString();
                }
            }
        }

        //Past Months Records
        private void Past_MonthRecords()
        {
            string CustAcc = TextBox_Account.Text.Trim();
            if (CustAcc == "")
            {
                return;
            }
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                var tuple_AmountMST_TransDate_Invoice = EOR_GET_NewApplicant.getAmountMST_TransDate_Invoice(DynAx, CustAcc, GLOBAL.system_checking);
                double[] arr_AmountMST = tuple_AmountMST_TransDate_Invoice.Item1;
                string[] arr_TransDate = tuple_AmountMST_TransDate_Invoice.Item2;
                int Count = tuple_AmountMST_TransDate_Invoice.Item3;
                int[] int_transaction_number = tuple_AmountMST_TransDate_Invoice.Item4;
                if (Count == 0)// no records
                {
                    //Function_Method.MsgBox("There is no past 6 month transaction records for " + CustAcc + " .", this.Page, this);//dont need to show msg
                    return;
                }
                GridView_Past_MonthRecords.DataSource = null;
                GridView_Past_MonthRecords.DataBind();

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("No.", typeof(string)));
                dt.Columns.Add(new DataColumn("Trans Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Debit Amount (RM)", typeof(string)));
                dt.Columns.Add(new DataColumn("No. of Transaction", typeof(string)));
                //
                DataRow row;
                int countA = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (arr_TransDate[i] != "")
                    {
                        countA = countA + 1;
                        row = dt.NewRow();
                        row["No."] = countA;
                        row["Trans Date"] = arr_TransDate[i];
                        row["Debit Amount (RM)"] = arr_AmountMST[i].ToString("#,###,###,##0.00");
                        row["No. of Transaction"] = int_transaction_number[i].ToString();
                        dt.Rows.Add(row);
                    }
                }
                GridView_Past_MonthRecords.DataSource = dt;
                GridView_Past_MonthRecords.DataBind();
            }
            catch (Exception ER_EO_02)
            {
                Function_Method.MsgBox("ER_EO_02: " + ER_EO_02.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        //EOR Part 2 //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        //EOR Part Admin //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void Recalculate_Admin(object sender, EventArgs e)//refresh
        {
            GetRecalculate_Admin();
        }
        private void GetRecalculate_Admin()
        {
            GetGridSelectedEORClass();
            //Function_Method.MsgBox(hidden_EORCarton.Text +"  @  " + hidden_EORPoint.Text, this.Page, this);
            if (hidden_EORCarton.Text == "" || hidden_EORPoint.Text == "")
            {
                Function_Method.MsgBox("There is no data for selected class", this.Page, this);
                return;
            }
            if (hidden_Label_EquipmentRecId_Update.Text == "")//first time insert
            {
                TransferInfoGridEquipment();
            }

            if (hidden_duration_year_month.Text == "")
            {
                duration_rate();
            }
            Label_DurationContract_Admin.Text = hidden_duration_year_month.Text;
        }

        // ===========================================================================================================================================================
        protected void Save_Admin(object sender, EventArgs e)//Save
        {
            string temp_EquipmentId = hidden_Label_EquipmentRecId.Text;
            string temp_EquipmentId_Update = hidden_Label_EquipmentRecId_Update.Text;

            if (temp_EquipmentId_Update != "")//to update
            {
                var tuple_GridView_EquipmentAdmin_ToBeUpdate = Tuple_GridView_EquipmentAdmin_ToBeUpdate();
                if (tuple_GridView_EquipmentAdmin_ToBeUpdate != null)
                {
                    int row_count = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item1;
                    string[] Desc_Admin = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item2;
                    string[] Qty_Admin = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item3;
                    string[] NormalItem_Admin = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item4;
                    string[] EquipCost_Admin = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item5;
                    string[] EquipQtyCost_Admin = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item6;
                    string[] Carton_Admin = tuple_GridView_EquipmentAdmin_ToBeUpdate.Item7;

                    update_EOR_WebEquipment_Item_Admin(temp_EquipmentId_Update, row_count, Desc_Admin, Qty_Admin, NormalItem_Admin, EquipCost_Admin, EquipQtyCost_Admin, Carton_Admin);

                    string Remark_Admin = ""; string Total_Monthly_Target = "";
                    if (Label_MonthlyTarget.Text != "")
                    {
                        Total_Monthly_Target = Label_MonthlyTarget.Text;
                    }
                    else
                    {
                        Function_Method.MsgBox("Error update LF_WebEquipment_Item.", this.Page, this);
                        return;
                    }
                    if (TextBox_RemarksAdmin.Text != "")
                    {
                        Remark_Admin = TextBox_RemarksAdmin.Text;
                    }

                    update_EOR_WebEquipment_Admin(temp_EquipmentId_Update, Remark_Admin, Total_Monthly_Target);

                    //update next approval
                    Axapta DynAx = new Axapta();
                    GLOBAL.Company = GLOBAL.switch_Company;
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    string Equip_Id = "";
                    try
                    {
                        string customer_acc = TextBox_Account.Text.Trim();
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                        {
                            DynAx.TTSBegin();
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", temp_EquipmentId_Update));
                            if (DynRec.Found)
                            {
                                Equip_Id = DynRec.get_Field("EQUIP_ID").ToString();
                                bool OKtoGo = Insert_Updata_EORApprovalDuringUpdate(DynAx, DynRec, Equip_Id);

                                if (OKtoGo == true)
                                {
                                    DynRec.Call("Update");
                                }
                            }
                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                    }
                    catch (Exception ER_EO_19)
                    {
                        Function_Method.MsgBox("ER_EO_19: " + ER_EO_19.ToString(), this.Page, this);
                        return;
                    }
                    finally
                    {
                        DynAx.Logoff();
                    }
                    Session["data_passing"] = "@EOE2_";
                    Response.Redirect("EOR.aspx");
                }
            }
            else //to insert
            {
                //do nothing
            }
        }

        private void update_EOR_WebEquipment_Admin(string temp_EquipmentId_Update, string Remark_Admin, string Total_Monthly_Target)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                using (AxaptaRecord DynRec1 = DynAx.CreateAxaptaRecord("LF_WebEquipment"))
                {
                    DynAx.TTSBegin();
                    DynRec1.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", temp_EquipmentId_Update));
                    if (DynRec1.Found)
                    {
                        DynRec1.set_Field("Remarks_Admin", Remark_Admin);

                        double double_Total_Monthly_Target = Convert.ToDouble(Total_Monthly_Target);
                        DynRec1.set_Field("Amount", double_Total_Monthly_Target);

                        DynRec1.Call("Update");
                    }
                    DynAx.TTSCommit(); DynAx.TTSAbort();
                }
            }
            catch (Exception ER_EO_16)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_16: " + ER_EO_16.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private Tuple<int, string[], string[], string[], string[], string[], string[]> Tuple_GridView_EquipmentAdmin_ToBeUpdate()
        {
            int row_count = GridView_EquipmentAdmin.Rows.Count;
            if (row_count < 1)
            {
                return null;
            }

            string[] Desc_Admin = new string[row_count];
            string[] Qty_Admin = new string[row_count];
            string[] NormalItem_Admin = new string[row_count];
            string[] EquipCost_Admin = new string[row_count];
            string[] EquipQtyCost_Admin = new string[row_count];
            string[] Carton_Admin = new string[row_count];

            for (int i = 0; i < row_count; i++)
            {
                if (GridView_EquipmentAdmin.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox rbox1 = (GridView_EquipmentAdmin.Rows[i].Cells[1].FindControl("TextBox_Description_R") as TextBox);
                    TextBox rbox2 = (GridView_EquipmentAdmin.Rows[i].Cells[2].FindControl("TextBox_New_QTY_R") as TextBox);
                    DropDownList DropDownList_NormalItem = (GridView_EquipmentAdmin.Rows[i].Cells[3].FindControl("DropDownList_NormalItem") as DropDownList);
                    TextBox rbox3 = (GridView_EquipmentAdmin.Rows[i].Cells[4].FindControl("TextBox_Equipment_Cost_R") as TextBox);
                    TextBox rbox4 = (GridView_EquipmentAdmin.Rows[i].Cells[5].FindControl("TextBox_Equipment_Handling_R") as TextBox);
                    Label rbox5 = (GridView_EquipmentAdmin.Rows[i].Cells[6].FindControl("Label_CartonEquipment_R") as Label);

                    if (DropDownList_NormalItem.Visible == false)//already have carton
                    {
                        Desc_Admin[i] = ""; Qty_Admin[i] = "";
                        NormalItem_Admin[i] = ""; EquipCost_Admin[i] = "";
                        EquipQtyCost_Admin[i] = ""; Carton_Admin[i] = "";
                    }
                    else //dint have carton
                    {
                        Desc_Admin[i] = rbox1.Text; Qty_Admin[i] = rbox2.Text;
                        NormalItem_Admin[i] = DropDownList_NormalItem.SelectedValue;
                        EquipCost_Admin[i] = rbox3.Text;
                        EquipQtyCost_Admin[i] = rbox4.Text; Carton_Admin[i] = rbox5.Text;
                    }
                }
            }
            return new Tuple<int, string[], string[], string[], string[], string[], string[]>(row_count, Desc_Admin, Qty_Admin, NormalItem_Admin, EquipCost_Admin, EquipQtyCost_Admin, Carton_Admin);
        }

        private void update_EOR_WebEquipment_Item_Admin(string temp_EquipmentId_Update, int row_count, string[] Desc_Admin, string[] Qty_Admin, string[] NormalItem_Admin,
            string[] EquipCost_Admin, string[] EquipQtyCost_Admin, string[] Carton_Admin)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_WebEquipment_Item"))
                {
                    if (row_count >= 1)
                    {
                        DynAx.TTSBegin();

                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RefRecId", temp_EquipmentId_Update));
                        int count = 0;
                        while (DynRec.Found)
                        {
                            Update_EOR_WebEquipment_Item_Admin(DynRec, count, Desc_Admin, Qty_Admin, NormalItem_Admin, EquipCost_Admin, EquipQtyCost_Admin, Carton_Admin);
                            ButtonAdd_Equipment.Enabled = false;//hide button add Equipment.
                            count = count + 1;
                            DynRec.Next();
                        }
                        DynAx.TTSCommit(); DynAx.TTSAbort();
                    }
                }
            }
            catch (Exception ER_EO_16)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_16: " + ER_EO_16.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void Update_EOR_WebEquipment_Item_Admin(AxaptaRecord DynRec, int count, string[] Desc_Admin, string[] Qty_Admin, string[] NormalItem_Admin,
            string[] EquipCost_Admin, string[] EquipQtyCost_Admin, string[] Carton_Admin)
        {
            //string temp_EquipmentId = DynRec.get_Field("ItemID").ToString();
            string temp_EquipmentName = DynRec.get_Field("ItemName").ToString();

            DynRec.set_Field("Quantity", Qty_Admin[count]);
            DynRec.set_Field("Charges", NormalItem_Admin[count]);
            DynRec.set_Field("Amount", EquipCost_Admin[count]);
            DynRec.set_Field("TotalAmount", EquipQtyCost_Admin[count]);
            DynRec.set_Field("ManualCarton", Carton_Admin[count]);
            DynRec.Call("Update");
        }
        // ===========================================================================================================================================================

        private void GetGridSelectedEORClass()
        {
            string temp_CustClass = hidden_customer_class.Text;
            Axapta DynAx = new Axapta();
            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                if (temp_CustClass != "")
                {
                    string ClassDesc = EOR_GET_NewApplicant.getClassDetails(DynAx, temp_CustClass);
                    var tuple_getEORClassSetup = EOR_GET_NewApplicant.getEORClassSetup(DynAx, temp_CustClass);
                    string EOR_CartonValue = tuple_getEORClassSetup.Item1;
                    string EOR_PointValue = tuple_getEORClassSetup.Item2;
                    string EOR_ApprovedBy = tuple_getEORClassSetup.Item3;

                    hidden_EORCarton.Text = EOR_CartonValue;
                    hidden_EORPoint.Text = EOR_PointValue;
                }
            }
            catch (Exception ER_EO_01)
            {
                Function_Method.MsgBox("ER_EO_01: " + ER_EO_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void TransferInfoGridEquipment()
        {
            int row_count = GridView_Equipment.Rows.Count;
            if (row_count < 1)
            {
                Function_Method.MsgBox("There is no equipment filled by Salesman.", this.Page, this);
                return;
            }

            GridView_EquipmentAdmin.DataSource = null;
            GridView_EquipmentAdmin.DataBind();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Qty", typeof(string)));

            dt.Columns.Add(new DataColumn("Types", typeof(string)));
            dt.Columns.Add(new DataColumn("Equipment_Cost", typeof(string)));
            dt.Columns.Add(new DataColumn("Equipment_Handling", typeof(string)));
            dt.Columns.Add(new DataColumn("Carton", typeof(string)));
            //
            DataRow row;
            int count = 0;
            for (int i = 0; i < row_count; i++)
            {
                if (GridView_Equipment.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox box1 = (TextBox)GridView_Equipment.Rows[i].Cells[3].FindControl("TextBox_New_QTY");
                    TextBox box2 = (TextBox)GridView_Equipment.Rows[i].Cells[1].FindControl("TextBox_Description");
                    TextBox box3 = (TextBox)GridView_Equipment.Rows[i].Cells[4].FindControl("TextBox_DepositEquipment");
                    Label box4 = (Label)GridView_Equipment.Rows[i].Cells[5].FindControl("Label_CartonEquipment");
                    Label box5 = (Label)GridView_Equipment.Rows[i].Cells[6].FindControl("Label_RecIdEquipment");

                    if (box2.Text.ToString() == "" || box1.Text.ToString() == "")
                    {
                        goto NEXT;
                    }
                    count = count + 1;
                    row = dt.NewRow();
                    row["No."] = count;
                    row["Description"] = box2.Text;
                    row["Qty"] = box1.Text;
                    row["Equipment_Cost"] = "";
                    row["Equipment_Handling"] = "";
                    if (box4.Text != "")
                    {
                        row["Carton"] = box4.Text;
                    }
                    else
                    {
                        row["Carton"] = "";
                    }
                    // 
                    dt.Rows.Add(row);
                NEXT:;
                }
            }
            GridView_EquipmentAdmin.DataSource = dt;
            GridView_EquipmentAdmin.DataBind();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox rbox1 = (GridView_EquipmentAdmin.Rows[i].Cells[1].FindControl("TextBox_Description_R") as TextBox);
                TextBox rbox2 = (GridView_EquipmentAdmin.Rows[i].Cells[2].FindControl("TextBox_New_QTY_R") as TextBox);
                DropDownList DropDownList_NormalItem = (GridView_EquipmentAdmin.Rows[i].Cells[3].FindControl("DropDownList_NormalItem") as DropDownList);
                TextBox rbox3 = (GridView_EquipmentAdmin.Rows[i].Cells[4].FindControl("TextBox_Equipment_Cost_R") as TextBox);
                TextBox rbox4 = (GridView_EquipmentAdmin.Rows[i].Cells[5].FindControl("TextBox_Equipment_Handling_R") as TextBox);
                Label rbox5 = (GridView_EquipmentAdmin.Rows[i].Cells[6].FindControl("Label_CartonEquipment_R") as Label);

                rbox2.Text = dt.Rows[i]["Qty"].ToString();
                rbox1.Text = dt.Rows[i]["Description"].ToString();
                rbox5.Text = dt.Rows[i]["Carton"].ToString();

                if (rbox5.Text == "" || rbox5.Text == "0")//carton not found
                {
                    // rbox3.Visible = true; rbox4.Visible = true;
                    //DropDownList_NormalItem.Visible = true;
                }
                else
                {
                    //rbox3.Visible = false; rbox4.Visible = false;
                    //DropDownList_NormalItem.Visible = false;
                    DropDownList_NormalItem.SelectedValue = "99";
                }
            }
        }

        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==
        protected void Button_CalculateEquipmentAdmin_click(object sender, EventArgs e)
        {
            double HandlingRateNormalItem = 1.05;// Normal: 1.05 ; Non Normal: 1.15
            double HandlingRateNotNormalItem = 1.15;// Normal: 1.05 ; Non Normal: 1.15

            int row_count = GridView_EquipmentAdmin.Rows.Count;
            if (row_count < 1)
            {
                return;
            }
            for (int i = 0; i < row_count; i++)
            {
                if (GridView_EquipmentAdmin.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox rbox2 = (GridView_EquipmentAdmin.Rows[i].Cells[2].FindControl("TextBox_New_QTY_R") as TextBox);
                    DropDownList DropDownList_NormalItem = (GridView_EquipmentAdmin.Rows[i].Cells[3].FindControl("DropDownList_NormalItem") as DropDownList);
                    TextBox rbox3 = (GridView_EquipmentAdmin.Rows[i].Cells[4].FindControl("TextBox_Equipment_Cost_R") as TextBox);
                    TextBox rbox4 = (GridView_EquipmentAdmin.Rows[i].Cells[5].FindControl("TextBox_Equipment_Handling_R") as TextBox);
                    Label rbox5 = (GridView_EquipmentAdmin.Rows[i].Cells[6].FindControl("Label_CartonEquipment_R") as Label);
                    double temp_SelectedRate = 0;

                    if (rbox5.Text == "0" || rbox5.Text == "" || DropDownList_NormalItem.Visible == true)//carton not found
                    {
                        if (DropDownList_NormalItem.SelectedValue == "0")//normal
                        {
                            temp_SelectedRate = HandlingRateNormalItem;
                        }
                        else if (DropDownList_NormalItem.SelectedValue == "1")
                        {
                            temp_SelectedRate = HandlingRateNotNormalItem;
                        }
                        double EquipmentCost = 0; double EquipmentQty = 0; double temp_CalculatedEquipmentHandlingCost = 0;
                        if (rbox3.Text.ToString() == "")
                        {
                            //do nothing
                        }
                        else
                        {
                            EquipmentCost = Convert.ToDouble(rbox3.Text);//Equipment Cost
                            EquipmentQty = Convert.ToDouble(rbox2.Text);// QTY
                            temp_CalculatedEquipmentHandlingCost = EquipmentCost * EquipmentQty * temp_SelectedRate;
                        }
                        rbox4.Text = temp_CalculatedEquipmentHandlingCost.ToString("#,###,###,##0.00");//TextBox_Equipment_Handling_R
                        double temp_Equipment_PriceWithHandling = Convert.ToDouble(rbox4.Text);
                        string str_MonthlyTarget = Formulae_MonthlyTarget(temp_Equipment_PriceWithHandling);
                        if (str_MonthlyTarget != null)
                        {
                            rbox5.Text = str_MonthlyTarget;
                        }
                    }
                }
            }
            Formulae_TotalEquipment_Carton();
        }

        private void Formulae_TotalEquipment_Carton()
        {
            int row_count = GridView_EquipmentAdmin.Rows.Count;

            double Equipment_Carton = 0;

            for (int i = 0; i < row_count; i++)
            {
                if (GridView_EquipmentAdmin.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    Label rbox5 = (GridView_EquipmentAdmin.Rows[i].Cells[6].FindControl("Label_CartonEquipment_R") as Label);
                    try
                    {
                        if (rbox5.Text != "")
                        {
                            double temp_Equipment_Carton = Convert.ToDouble(rbox5.Text);

                            Equipment_Carton += temp_Equipment_Carton;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            Label_MonthlyTarget.Text = Equipment_Carton.ToString("#,###,###,##0.00");
        }

        private string Formulae_MonthlyTarget(double Equipment_PriceWithHandling)
        {
            if ((hidden_duration_year_month.Text == "") || (hidden_duration_rate.Text == "") || (hidden_EORCarton.Text == "") || (Equipment_PriceWithHandling == 0))
            {
                return null;
            }

            double EOR_ClassRate = Convert.ToDouble(hidden_EORCarton.Text);//refer: EOR class table rate
            double EOR_duration = Convert.ToDouble(hidden_duration_year_month.Text);
            double EOR_durationRate = Convert.ToDouble(hidden_duration_rate.Text);

            double MonthlyTarget = Equipment_PriceWithHandling * EOR_durationRate / EOR_ClassRate / EOR_duration;
            string str_MonthlyTarget = MonthlyTarget.ToString("#,###,###,##0.00");
            return str_MonthlyTarget;
        }

        protected void Button_Alt_Delivery_Addr_Click(object sender, EventArgs e)
        {
            if (GridView_AltAddress.Visible == true)
            {
                GridView_AltAddress.Visible = false;
                Button_Delivery_Addr.Text = "Alt. Addr.";
            }
            else
            {
                GridView_AltAddress.Visible = true;
                Alt_Addr_function();// so that i will refresh when user plannign to change selection again

                Button_Delivery_Addr.Text = "Hide Alt. Addr.";

                if (TextBox_Account.Text == "")
                {
                    Function_Method.MsgBox("There is no customer account number.", this.Page, this);
                    Button_Delivery_Addr.Text = "Alt. Addr.";
                    return;
                }

                //Add to Grid, GridView_AltAddress
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Alt. Address") });

                int Counter = Convert.ToInt32(hidden_alt_address_counter.Text);

                string[] arr_alt_address = hidden_alt_address.Text.Split('|');
                for (int i = 0; i < Counter; i++)
                {
                    dt.Rows.Add(arr_alt_address[i]);
                }
                //this.GridView_AltAddress.Columns[3].Visible = false;
                //GridView_AltAddress.Columns[3].Visible = false;//Hide RecId
                GridView_AltAddress.DataSource = dt;
                GridView_AltAddress.DataBind();
            }
        }

        private void Alt_Addr_function()
        {
            Button_Delivery_Addr.Visible = false;
            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";
            //
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);


                var tuple_get_AltAddress = SFA_GET_SALES_HEADER.get_AltAddress(DynAx, TextBox_Account.Text);
                if (tuple_get_AltAddress == null)
                {
                    return;
                }
                string[] AltAddress = tuple_get_AltAddress.Item1;
                string[] AltAddressRecId = tuple_get_AltAddress.Item2;
                int Counter = tuple_get_AltAddress.Item3;

                if (Counter == 1)//only one data
                {
                    if (AltAddress[0] == Label_Address.Text)//same as primary, no alt address
                    {
                        return;
                    }
                }
                int temp_count = 0;
                for (int i = 0; i < Counter; i++)
                {
                    if (AltAddress[i] == hidden_Street.Text)
                    {
                        //skip
                    }
                    else
                    {
                        if (temp_count == 0)//first count
                        {
                            hidden_alt_address_RecId.Text = AltAddressRecId[i];
                            hidden_alt_address.Text = AltAddress[i];
                        }
                        else
                        {
                            hidden_alt_address_RecId.Text = hidden_alt_address_RecId.Text + "|" + AltAddressRecId[i];
                            hidden_alt_address.Text = hidden_alt_address.Text + "|" + AltAddress[i];
                        }
                        temp_count = temp_count + 1;
                    }
                }
                hidden_alt_address_counter.Text = temp_count.ToString();
                Button_Delivery_Addr.Visible = true;
            }
            catch (Exception ER_EO_23)
            {
                Function_Method.MsgBox("ER_EO_23: " + ER_EO_23.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void CheckBox_Changed2(object sender, EventArgs e)
        {
            int counter = 0;
            foreach (GridViewRow row in GridView_AltAddress.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox_c = (row.Cells[0].FindControl("chkRow2") as CheckBox);
                    if (CheckBox_c.Checked)
                    {
                        string selected_address = row.Cells[1].Text;

                        if (selected_address == Label_Address.Text)//added as a precaution
                        {
                            //do nothing
                        }
                        else
                        {
                            Label_Address.Text = selected_address;
                        }
                        //need to update hidden field
                        Axapta DynAx = new Axapta();
                        GLOBAL.Company = GLOBAL.switch_Company;
                        DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);


                        string temp_RecID_List = hidden_alt_address_RecId.Text;
                        string[] arr_temp_RecID_List = temp_RecID_List.Split('|');
                        var tuple_get_AltAddress_info = SFA_GET_SALES_HEADER.get_AltAddress_info(DynAx, arr_temp_RecID_List[counter]);

                        hidden_Street.Text = tuple_get_AltAddress_info.Item1;
                        hidden_ZipCode.Text = tuple_get_AltAddress_info.Item2;
                        hidden_City.Text = tuple_get_AltAddress_info.Item3;
                        hidden_State.Text = tuple_get_AltAddress_info.Item4;
                        hidden_Country.Text = tuple_get_AltAddress_info.Item5;

                        DynAx.Logoff();

                        GridView_AltAddress.Visible = false;
                        Button_Delivery_Addr.Text = "Alt. Addr.";
                        //row.BackColor = Color.FromName("#ff8000");
                    }
                }
                counter = counter + 1;
            }
        }
    }
}