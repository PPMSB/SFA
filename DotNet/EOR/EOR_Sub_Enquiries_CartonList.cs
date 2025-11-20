using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class EOR : System.Web.UI.Page
    {
        protected void Button_EORCartonList_Click(object sender, EventArgs e)
        {
            f_Button_EORCartonList();
        }
        private void f_Button_EORCartonList()
        {
            GridViewCartonListing.PageIndex = 0;
            EORCartonList_section.Visible = true;
            Div_GridViewCartonListing.Visible = true;
            Div_NewCartonListing.Visible = false;

            Button_enquiries_section_accordion.Text = "EOR Carton List";
            SearchCarton_Bar.Visible = true;
            carton_listing(0, "");
        }

        //protected void CheckBox_div_Searchable_Carton(object sender, EventArgs e)
        //{
        //    if (CheckBox_div_Searchable_ID_Carton.Checked)
        //    {
        //        div_Searchable_CartonList.Visible = true;
        //        CheckBox_div_Searchable_ID_Carton.Text = "Hide Search Bar";
        //    }
        //    else
        //    {
        //        div_Searchable_CartonList.Visible = false;
        //        CheckBox_div_Searchable_ID_Carton.Text = "Show Search Bar";

        //    }

        //}

        protected void Button_Search_Carton_Click(object sender, ImageClickEventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            switch (DropDownList1.SelectedItem.Text)
            {
                case "Item Id":
                    fieldName = "ItemId";//Item Id
                    break;
                case "Deposit":
                    fieldName = "Deposit";//Deposit
                    break;

                default:
                    fieldName = "";
                    break;
            }

            carton_listing(0, fieldName);
        }

        private void carton_listing(int PAGE_INDEX, string fieldName)
        {

            Button_Update.Enabled = false;
            GridViewCartonListing.DataSource = null; GridViewCartonListing.DataBind();
            //
            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                int tableId = DynAx.GetTableId("LF_InventTable_EOR");
                AxaptaObject axQuery_2 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource_2 = (AxaptaObject)axQuery_2.Call("addDataSource", tableId);

                string temp_SearchValue = "*" + TextBox_SearchCarton.Text.Trim() + "*";
                if (fieldName != "" && temp_SearchValue != "")
                {
                    string fieldValue = "*" + temp_SearchValue + "*";
                    var qbr_2 = (AxaptaObject)axQueryDataSource_2.Call("addRange", DynAx.GetFieldId(tableId, fieldName));
                    qbr_2.Call("value", fieldValue);
                }
                axQueryDataSource_2.Call("addSortField", DynAx.GetFieldId(tableId, "RecId"), 1);//RecId, descending
                AxaptaObject axQueryRun_2 = DynAx.CreateAxaptaObject("QueryRun", axQuery_2);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Item Id"; N[2] = "Item GroupId";
                N[3] = "Item Name"; N[4] = "Sman From"; N[5] = "Sman To";
                N[6] = "Effective Date"; N[7] = "CTN_Dep"; N[8] = "CTN_NoDep";
                N[9] = "Deposit"; N[10] = "Contract Duration"; N[11] = "RecId";

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

                while ((bool)axQueryRun_2.Call("next"))
                {
                    AxaptaRecord DynRec_2 = (AxaptaRecord)axQueryRun_2.Call("Get", tableId);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string temp_ItemId = DynRec_2.get_Field("ItemId").ToString();
                        row["Item Id"] = temp_ItemId;
                        string temp_ItemGroupId = DynRec_2.get_Field("ItemGroupId").ToString();
                        if (temp_ItemGroupId == "")
                        {
                            var tuple_getItemGroupId_RecId = EOR_GET_NewApplicant.getItemGroupId_RecId(DynAx, temp_ItemId);
                            temp_ItemGroupId = tuple_getItemGroupId_RecId.Item1;
                        }
                        row["Item GroupId"] = temp_ItemGroupId;
                        row["Item Name"] = SFA_GET_PROMO_LAYER2.getItemName(DynAx, temp_ItemId);
                        row["Sman To"] = DynRec_2.get_Field("SmanTo").ToString();
                        row["Sman From"] = DynRec_2.get_Field("SmanFrom").ToString();
                        string temp_EffectiveDate = DynRec_2.get_Field("EffectiveDate").ToString();
                        string[] arr_temp_EffectiveDate = temp_EffectiveDate.Split(' ');//date + " " + time;
                        string Raw_temp_EffectiveDate = arr_temp_EffectiveDate[0];
                        row["Effective Date"] = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_temp_EffectiveDate, true);

                        row["CTN_Dep"] = DynRec_2.get_Field("CTN_Dep").ToString();
                        row["CTN_NoDep"] = DynRec_2.get_Field("CTN_NoDep").ToString();
                        int val = Convert.ToInt16(DynRec_2.get_Field("Deposit").ToString());
                        row["Deposit"] = val.ToString("N2");
                        row["Contract Duration"] = DynRec_2.get_Field("Period").ToString();
                        row["RecId"] = DynRec_2.get_Field("RecId").ToString();
                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec_2.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH_2;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH_2:
                GridViewCartonListing.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                GridViewCartonListing.DataSource = dt;
                GridViewCartonListing.DataBind();
            }
            catch (Exception ER_EO_04)
            {
                Function_Method.MsgBox("ER_EO_04: " + ER_EO_04.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void datagrid_PageIndexChanging_carton(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (TextBox_SearchCarton.Text == "")
                {
                    carton_listing(e.NewPageIndex, "");
                }
                else
                {
                    string fieldName = "";
                    switch (DropDownList_Search.SelectedItem.Text)
                    {
                        case "Item Id":
                            fieldName = "ItemId";//Item Id
                            break;
                        case "Deposit":
                            fieldName = "Deposit";//Deposit
                            break;

                        default:
                            fieldName = "";
                            break;
                    }

                    carton_listing(e.NewPageIndex, fieldName);
                }

                GridViewCartonListing.PageIndex = e.NewPageIndex;
                GridViewCartonListing.DataBind();
            }
            catch (Exception ER_EO_06)
            {
                Function_Method.MsgBox("ER_EO_06: " + ER_EO_06.ToString(), this.Page, this);
            }
        }

        protected void CheckBoxSelect_Changed(object sender, EventArgs e)
        {
            try
            {
                //get the the id that call the function
                CheckBox CheckBox_selection1 = sender as CheckBox;

                string ClientID = CheckBox_selection1.ClientID;

                string[] arr_ClientID = ClientID.Split('_');
                int arr_count = arr_ClientID.Count();
                int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);
                //
                int row_count = GridViewCartonListing.Rows.Count;
                int count_selection = 0;
                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridViewCartonListing.Rows[i].Cells[0].FindControl("chkRowSelect") as CheckBox);

                    if (GridViewCartonListing.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (i != ClientRow)//allow only one selection
                        {
                            CheckBox_c.Checked = false;
                        }
                        if (CheckBox_c.Checked)//highlight
                        {
                            GridViewCartonListing.Rows[i].BackColor = Color.FromName("#ff8000");
                            count_selection = count_selection + 1;
                        }
                    }
                }
                if (count_selection != 0)
                {
                    Button_Update.Enabled = true;
                }
                else
                {
                    Button_Update.Enabled = false;
                }
            }
            catch (Exception ER_EO_07)
            {
                Function_Method.MsgBox("ER_EO_07: " + ER_EO_07.ToString(), this.Page, this);
            }
        }

        protected void CreateSelect(object sender, EventArgs e)
        {
            try
            {
                clear_variable_CartonListNewUpdate();
                f_Button_EORCartonNew("0");//0:create, 1: Update
            }
            catch (Exception ER_EO_09)
            {
                Function_Method.MsgBox("ER_EO_09: " + ER_EO_09.ToString(), this.Page, this);
            }
        }

        protected void UpdateSelect(object sender, EventArgs e)
        {
            clear_variable_CartonListNewUpdate();
            try
            {
                int row_count = GridViewCartonListing.Rows.Count;

                string[] arr_selection = new string[row_count];
                for (int i = 0; i < row_count; i++)
                {
                    CheckBox CheckBox_c = (GridViewCartonListing.Rows[i].Cells[0].FindControl("chkRowSelect") as CheckBox);

                    if (GridViewCartonListing.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBox_c.Checked)//highlight
                        {
                            Label_ItemID.Text = GridViewCartonListing.Rows[i].Cells[2].Text;
                            //Item Item = GridViewCartonListing.Rows[i].Cells[3].Text
                            Label_ItemName.Text = GridViewCartonListing.Rows[i].Cells[4].Text;
                            string temp = GridViewCartonListing.Rows[i].Cells[5].Text;
                            DropDownList_SmanFrom.SelectedValue = GridViewCartonListing.Rows[i].Cells[5].Text;
                            DropDownList_SmanTo.SelectedValue = GridViewCartonListing.Rows[i].Cells[6].Text;
                            Label_EffectiveDate.Text = GridViewCartonListing.Rows[i].Cells[7].Text;
                            TextBox_CartonWithDeposit.Text = GridViewCartonListing.Rows[i].Cells[8].Text;
                            TextBox_CartonWithoutDeposit.Text = GridViewCartonListing.Rows[i].Cells[9].Text;
                            TextBox_DepositRequired.Text = GridViewCartonListing.Rows[i].Cells[10].Text;
                            DropDownList_ContractDuration.SelectedValue = GridViewCartonListing.Rows[i].Cells[11].Text;
                            hidden_Label_CartonRecId.Text = GridViewCartonListing.Rows[i].Cells[12].Text;
                            f_Button_EORCartonNew("1");//0:create, 1: Update
                        }
                    }
                }
            }
            catch (Exception ER_EO_08)
            {
                Function_Method.MsgBox("ER_EO_08: " + ER_EO_08.ToString(), this.Page, this);
            }
        }
        //=============================================================================================================
        private void clear_variable_CartonListNewUpdate()
        {
            Label_ItemID.Text = "";
            Label_ItemName.Text = "";
            Label_EffectiveDate.Text = "";
            TextBox_CartonWithDeposit.Text = "";
            TextBox_CartonWithoutDeposit.Text = "";
            TextBox_DepositRequired.Text = "";
            Dropdown();
        }
        //=================================================================================================================
        protected void CheckItemInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_EOIM@";//EOR > InventoryMaster
            Response.Redirect("InventoryMaster.aspx");
        }

        private void f_Button_EORCartonNew(string Update_Create)
        {
            SearchCarton_Bar.Visible = false;
            EORCartonList_section.Visible = true;
            Div_GridViewCartonListing.Visible = false;
            Div_NewCartonListing.Visible = true;

            Button_enquiries_section_accordion.Text = "EOR Carton List";

            if (Update_Create == "0")
            {
                Button_CreateNewList.Text = "Create";
            }
            else
            {
                Button_CreateNewList.Text = "Update";
            }
        }
        protected void Button_CreateNewList_Click(object sender, EventArgs e)//renamed as Add Sales Line
        {
            if (Label_ItemID.Text.Trim() == "")//cannt be empty mandatory
            {
                Function_Method.MsgBox("Please check all mandatory fields is not blank", this.Page, this);
                return;
            }

            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            string Item_Id = "";
            if (Label_ItemID.Text.Trim() != "") { Item_Id = Label_ItemID.Text.Trim(); }

            string Item_Name = "";
            if (Label_ItemName.Text.Trim() != "") { Item_Name = Label_ItemName.Text.Trim(); }

            string Contract_DurationRequired = "";
            Contract_DurationRequired = DropDownList_ContractDuration.SelectedValue;

            string SmanTo = "";
            SmanTo = DropDownList_SmanTo.SelectedValue;

            string SmanFrom = "";
            SmanFrom = DropDownList_SmanFrom.SelectedValue;

            string Deposit_Required = "";
            if (TextBox_DepositRequired.Text.Trim() != "") { Deposit_Required = TextBox_DepositRequired.Text.Trim(); }

            string CTN_NoDep = "";
            if (TextBox_CartonWithoutDeposit.Text.Trim() != "") { CTN_NoDep = TextBox_CartonWithoutDeposit.Text.Trim(); }

            string CTN_Dep = "";
            if (TextBox_CartonWithDeposit.Text.Trim() != "") { CTN_Dep = TextBox_CartonWithDeposit.Text.Trim(); }

            string EffectiveDate = "";
            if (Label_EffectiveDate.Text.Trim() != "") { EffectiveDate = Label_EffectiveDate.Text.Trim(); }

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_InventTable_EOR"))
                {
                    if (Button_CreateNewList.Text == "Create")
                    {
                        DynAx.TTSBegin();
                        CreateNewList_field(DynAx, DynRec, Item_Id, SmanFrom, SmanTo, Contract_DurationRequired, Deposit_Required, CTN_NoDep, CTN_Dep, EffectiveDate);

                        DynRec.Call("insert");
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                        logger.Info($"Successfully created new record for Item_Id: {Item_Id}, SmanFrom: {SmanFrom}, SmanTo: {SmanTo}");

                        Function_Method.MsgBox("Successfully to create.", this.Page, this);
                    }
                    else//Update
                    {
                        DynAx.TTSBegin();
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", hidden_Label_CartonRecId.Text));

                        if (DynRec.Found)
                        {
                            CreateNewList_field(DynAx, DynRec, Item_Id, SmanFrom, SmanTo, Contract_DurationRequired, Deposit_Required, CTN_NoDep, CTN_Dep, EffectiveDate);
                            DynRec.Call("Update");
                            logger.Info($"Successfully updated record for RecId: {hidden_Label_CartonRecId.Text}");

                            Function_Method.MsgBox("Successfully to update", this.Page, this);
                        }
                        else
                        {
                            Function_Method.MsgBox("Fail to update", this.Page, this);
                        }
                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }

                    clear_variable_CartonListNewUpdate();
                    f_Button_EORCartonList();
                }
            }
            catch (Exception ER_EO_10)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_EO_10: " + ER_EO_10.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void CreateNewList_field(Axapta DynAx, AxaptaRecord DynRec, string Item_Id, string SmanFrom, string SmanTo, string Contract_DurationRequired, string Deposit_Required, string CTN_NoDep, string CTN_Dep, string EffectiveDate)
        {
            DynRec.set_Field("ItemId", Item_Id);
            DynRec.set_Field("SmanFrom", SmanFrom);
            DynRec.set_Field("SmanTo", SmanTo);

            var tuple_getItemGroupId_RecId = EOR_GET_NewApplicant.getItemGroupId_RecId(DynAx, Item_Id);
            string temp_ItemGroupId = tuple_getItemGroupId_RecId.Item1;
            var temp_RefRecId = Convert.ToInt64(tuple_getItemGroupId_RecId.Item2);

            DynRec.set_Field("ItemGroupId", temp_ItemGroupId);
            DynRec.set_Field("RefRecId", temp_RefRecId);

            double double_Contract_DurationRequired = Convert.ToDouble(Contract_DurationRequired);
            DynRec.set_Field("Period", double_Contract_DurationRequired);
            if (Deposit_Required == "") { Deposit_Required = "0"; }
            double double_Deposit_Required = Convert.ToDouble(Deposit_Required);
            DynRec.set_Field("Deposit", double_Deposit_Required);
            var double_CTN_NoDep = Convert.ToDouble(CTN_NoDep);
            DynRec.set_Field("CTN_NoDep", double_CTN_NoDep);
            var double_CTN_Dep = Convert.ToDouble(CTN_Dep);
            DynRec.set_Field("CTN_Dep", double_CTN_Dep);
            var temp_EffectiveDate = DateTime.ParseExact(EffectiveDate, "dd/MM/yyyy", null);// in lotus use cdat
            DynRec.set_Field("EffectiveDate", temp_EffectiveDate);
        }
        protected void Button_SalesmanTotal_StartDate_Click(object sender, ImageClickEventArgs e)
        {
            if (Calendar_EffectiveDate.Visible == false)
            {
                Calendar_EffectiveDate.Visible = true;
            }
            else
            {
                Calendar_EffectiveDate.Visible = false;
            }
        }

        protected void Calendar_EffectiveDate_SelectionChanged(object sender, EventArgs e)
        {
            string EffectiveDate = Calendar_EffectiveDate.SelectedDate.ToShortDateString();
            EffectiveDate = Function_Method.get_correct_date(GLOBAL.system_checking, EffectiveDate, true);
            Label_EffectiveDate.Text = EffectiveDate;
            Calendar_EffectiveDate.Visible = false;
        }
        private bool Dropdown()
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            DropDownList_SmanTo.Items.Clear(); DropDownList_SmanFrom.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            try
            {
                items = SFA_GET_Enquiries_SalesmanTotal.getSalesman(DynAx);
                if (items.Count > 1)
                {
                    DropDownList_SmanTo.Items.AddRange(items.ToArray());
                    DropDownList_SmanFrom.Items.AddRange(items.ToArray());
                    return true;
                }
                else
                {
                    Function_Method.MsgBox("There is no Salesman available.", this.Page, this);
                    return false;
                }
            }
            catch (Exception ER_EO_17)
            {
                Function_Method.MsgBox("ER_EO_17: " + ER_EO_17.ToString(), this.Page, this);
                return false;
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void OnSelectedIndexChanged_DropDownList_SmanTo(object sender, EventArgs e)
        {


        }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}