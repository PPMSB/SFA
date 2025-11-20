using DotNet.SFAModel;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static DotNet.SignboardEquipment;

namespace DotNet
{
    public partial class WClaim_BatteryList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ClaimNo"]))
                {
                    string encodedClaimNo = Request.QueryString["ClaimNo"];
                    string claimNo = Server.UrlDecode(encodedClaimNo);
                    Session["data_passing"] = claimNo;
 
                    check_session();
                    TimeOutRedirect();
                    if (!IsPostBack)
                    {
                        clear_variable();

                        Check_DataRequest();
                        get_WClaim_Battery(claimNo);
                        //first time, reload gridview with Search ALL approach
                        GridView1.PageIndex = 0;
                        //GridView1.Columns[5].Visible = false;//hide on-order (request by Keegan 15/12/2020)
                        txtLF_Bat_IR.Text = "0.00";
                        txtLF_Bat_SOC.Text = "0.00";
                        txtLF_Bat_SOH.Text = "0.00";
                        txtLF_Bat_CCA.Text = "0.00";
                        txtLF_Bat_Vol.Text = "0.00";
                        f_call_ax(0);
                    }
                }
            }
            catch(Exception WC_BI_01)
            {
                Function_Method.MsgBox("WC_BI_01: " + WC_BI_01.ToString(), this.Page, this);
            }

            
        }

        private List<BatteryInfo> batteryList = new List<BatteryInfo>();
        private void Check_DataRequest()
        {
            string temp1 = GLOBAL.data_passing.ToString();
            //if (temp1 != "")//request from other module
            //{
            //    if (temp1.Length >= 6)
            //    {

            //        if (temp1.Substring(0, 6) == "_SFIM@")//Request from SFA > InventoryMaster
            //        {
            //            GridView1.Columns[1].Visible = true;//Inventory Id button
            //            GridView1.Columns[2].Visible = false;//Inventory Id label
            //        }

            //        if (temp1.Substring(0, 6) == "_EOIM@")//Request from EOR > InventoryMaster
            //        {
            //            GridView1.Columns[1].Visible = true;//Inventory Id button
            //            GridView1.Columns[2].Visible = false;//Inventory Id label
            //        }

            //        if (temp1.Substring(0, 6) == "_SQIM@")//Request from SQ > InventoryMaster
            //        {
            //            GridView1.Columns[1].Visible = true;//Inventory Id button
            //            GridView1.Columns[2].Visible = false;//Inventory Id label
            //        }
            //    }
            //}
            //else
            //{
            //    GridView1.Columns[1].Visible = false;// Inventory Id button
            //    GridView1.Columns[2].Visible = true;//Inventory Id label
            //}
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

        protected void CheckItem(object sender, EventArgs e)
        {
            clear_variable();
            GridView1.PageIndex = 0;
            f_call_ax(0);
        }

        private void clear_variable()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
        //=====================================================================================
        private void f_call_ax(int PAGE_INDEX)
        {
            batteryList.Clear();
            Axapta DynAx = Function_Method.GlobalAxapta();

                string fieldValue = Session["data_passing"].ToString();
            try
            {
                int LF_Battery_Info_TableID = DynAx.GetTableIdWithLock("LF_Battery_Info");
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_Battery_Info_TableID);

                int RecId = DynAx.GetFieldId(LF_Battery_Info_TableID, "RecId");
                int ClaimReference = DynAx.GetFieldId(LF_Battery_Info_TableID, "ClaimReference");
                int ModifiedDateTime = DynAx.GetFieldId(LF_Battery_Info_TableID, "modifiedDateTime");

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", ClaimReference);
                qbr.Call("value", fieldValue);
          
                
                axQueryDataSource.Call("addSortField", ModifiedDateTime, 1);//*fieldId, desc

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 12;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "InvoiceId"; N[2] = "SerialNumberId";

                N[3] = "BatchNumber"; N[4] = "LF_Bat_Vol"; N[5] = "LF_Bat_CCA";
                N[6] = "LF_Bat_SOH"; N[7] = "LF_Bat_SOC"; N[8] = "LF_Bat_IR"; 
    
                N[9] = "modifiedDateTime"; N[10] = "RecId"; N[11] = "Remarks";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_Battery_Info_TableID);
                    //string temp_LFI_WEBONLY = DynRec.get_Field("LFI_WEBONLY").ToString();
                    //for EOR, dont check LFI_WebOnly =====================================================================================================
                    string temp = Session["data_passing"].ToString();
                   
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        
                        row["InvoiceId"] = DynRec.get_Field("InvoiceId").ToString();
                        row["SerialNumberId"] = DynRec.get_Field("SerialNumberId").ToString();

                        row["LF_Bat_IR"] = DynRec.get_Field("LF_Bat_IR").ToString();
                        row["LF_Bat_SOC"] = DynRec.get_Field("LF_Bat_SOC").ToString();
                        row["LF_Bat_SOH"] = DynRec.get_Field("LF_Bat_SOH").ToString();
                        row["LF_Bat_CCA"] = DynRec.get_Field("LF_Bat_CCA").ToString();
                        row["LF_Bat_Vol"] = DynRec.get_Field("LF_Bat_Vol").ToString();
                        row["BatchNumber"] = DynRec.get_Field("BatchNumber").ToString();
                        row["modifiedDateTime"] = DynRec.get_Field("modifiedDateTime").ToString();
                        row["RecId"] = DynRec.get_Field("RecId").ToString();
                        row["Remarks"] = DynRec.get_Field("Remarks").ToString();

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                END: if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridView1.VirtualItemCount = countA;
                //Data-Binding with our GRID

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ER_CM_00)
            {
                Function_Method.MsgBox("ER_IM_00: " + ER_CM_00.ToString(), this.Page, this);
            }
        }
        //=====================================================================================
        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                clear_variable();
                f_call_ax(e.NewPageIndex);
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataBind();
            }
            catch (Exception ER_CM_01)
            {
                Function_Method.MsgBox("ER_IM_01: " + ER_CM_01.ToString(), this.Page, this);
            }
        }

        #region get_Data from AX
        private void get_WClaim_Battery(string WClaimID)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            string SerialNo = ""; string InvoiceId = ""; string Status = "";
            #region LF_WarrantyTable
            int LF_WarrantyTable = DynAx.GetTableIdWithLock("LF_WarrantyTable");

            AxaptaObject axQuery1_1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_1 = (AxaptaObject)axQuery1_1.Call("addDataSource", LF_WarrantyTable);


            int claimNumber_1 = DynAx.GetFieldId(LF_WarrantyTable, "ClaimNumber");
            var qbr1_1 = (AxaptaObject)axQueryDataSource1_1.Call("addRange", claimNumber_1);
            qbr1_1.Call("value", WClaimID);

            AxaptaObject axQueryRun1_1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_1);

            if ((bool)axQueryRun1_1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1_1.Call("Get", LF_WarrantyTable);
                Status = DynRec1.get_Field("ClaimStatus").ToString();
            }
            #endregion
            #region LF_WarrantyLine
            int LF_WarrantyLine = DynAx.GetTableIdWithLock("LF_WarrantyLine");

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_WarrantyLine);

            
            int claimNumber = DynAx.GetFieldId(LF_WarrantyLine, "ClaimNumber");
            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", claimNumber);
            qbr1.Call("value", WClaimID);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_WarrantyLine);
                SerialNo = DynRec1.get_Field("SerialNumber").ToString();
                InvoiceId= DynRec1.get_Field("CustDO").ToString();
                //Neil - Changes only store xxxxxxx/INV without behind Date - 21_10_2025
                int spaceIndex = InvoiceId.IndexOf(' ');
                InvoiceId = (spaceIndex > 0) ? InvoiceId.Substring(0, spaceIndex) : InvoiceId;
            }
            #endregion

            

            txtSerialNumberId.Text = SerialNo;
            txtInvoiceId.Text = InvoiceId;
            txtHiddenStatus.Text = Status;
        }
        #endregion

        //protected void Button_ItemId_Click(object sender, EventArgs e)
        //{
        //    string temp1 = GLOBAL.data_passing.ToString();
        //    if (temp1 != "")//request from other module
        //    {
        //        if (temp1.Length >= 6)
        //        {


        //        }
        //    }
        //}

        // Insert new record
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {

                #region Validation
                // Validate: at least one of the five decimal fields must have a non-zero value
                double ir = Convert.ToDouble(txtLF_Bat_IR.Text);
                double soc = Convert.ToDouble(txtLF_Bat_SOC.Text);
                double soh = Convert.ToDouble(txtLF_Bat_SOH.Text);
                double cca = Convert.ToDouble(txtLF_Bat_CCA.Text);
                double vol = Convert.ToDouble(txtLF_Bat_Vol.Text);


                if (ir == 0 && soc == 0 && soh == 0 && cca == 0 && vol == 0)
                {
                    // Show error message and stop processing
                    Function_Method.MsgBox("Please enter a non-zero value in at least one of the fields: IR, SOC, SOH, CCA, or Vol.", this.Page, this);
                    return;
                }

                #endregion

                BatteryInfo newItem = new BatteryInfo
                {
                    InvoiceId = txtInvoiceId.Text.Trim(),
                    SerialNumberId = txtSerialNumberId.Text.Trim(),
                    LF_Bat_IR = ir,LF_Bat_SOC = soc,LF_Bat_SOH = soh,
                    LF_Bat_CCA = cca,LF_Bat_Vol = vol,BatchNumber = txtBatch_No.Text.Trim(),

                    Remark = txtHiddenStatus.Text.Trim()
                };

                // TODO: Validate inputs here

                // TODO: Insert into your data source (replace with your actual insert logic)
                InsertBatteryInfo(newItem);
                string fieldValue = Session["data_passing"].ToString();
                // Clear inputs
                //txtInvoiceId.Text = "";
                //txtSerialNumberId.Text = "";
                txtLF_Bat_IR.Text = "0.00";
                txtLF_Bat_SOC.Text = "0.00";
                txtLF_Bat_SOH.Text = "0.00";
                txtLF_Bat_CCA.Text = "0.00";
                txtLF_Bat_Vol.Text = "0.00";
                txtBatch_No.Text = "";
                txtHiddenRef.Text = "";
                get_WClaim_Battery(fieldValue);
                // Reload grid first page
                GridView1.PageIndex = 0;
                clear_variable();
                f_call_ax(0);
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("Insert failed: " + ex.Message, this.Page, this);
            }
        }

        

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected row
            GridViewRow row = GridView1.SelectedRow;

            // Assuming your BoundFields are in the order you defined, get cell values
            txtInvoiceId.Text = row.Cells[2].Text; // InvoiceId (adjust index if needed)
            txtSerialNumberId.Text = row.Cells[3].Text;
            txtBatch_No.Text = row.Cells[4].Text;
            txtLF_Bat_Vol.Text = row.Cells[5].Text;
            txtLF_Bat_CCA.Text = row.Cells[6].Text;
            txtLF_Bat_SOH.Text = row.Cells[7].Text;
            txtLF_Bat_SOC.Text = row.Cells[8].Text;
            txtLF_Bat_IR.Text = row.Cells[9].Text;
           
            //txtHiddenRef.Text = row.Cells[11].Text;//hidden reference for update purpose only
            txtHiddenRef.Text = GridView1.DataKeys[row.RowIndex]["RecId"].ToString();
        }


        private void InsertBatteryInfo(BatteryInfo newItem)
        {
            Axapta DynAx = Function_Method.GlobalAxapta();
            string fieldValue = Session["data_passing"].ToString();
            using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("LF_Battery_Info"))
            {

                //if (txtHiddenRef.Text !="") {
                //    DynAx.TTSBegin();
                //    // Check if record exists

                //    DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == {1}", "RecId", Convert.ToInt64(txtHiddenRef.Text)));
                //    if (DynRec.Found)
                //    {
                //        // Update existing record
                //        Save_LF_Battery_Info(DynRec, newItem);
                //        DynRec.Call("Update");
                //    }

                //    DynAx.TTSCommit();
                //}
                //else
                //{
                    DynAx.TTSBegin();
                    // Insert new record
                    DynRec.set_Field("InvoiceId", newItem.InvoiceId);
                    DynRec.set_Field("SerialNumberId", newItem.SerialNumberId);
                    DynRec.set_Field("ClaimReference", fieldValue);
                    Save_LF_Battery_Info(DynRec, newItem);
                    DynRec.Call("insert");
                    DynAx.TTSCommit();
                //}
                   
            }
        }

        private void Save_LF_Battery_Info(AxaptaRecord DynRec,BatteryInfo newItem)
        {          
                   // Update existing record
              DynRec.set_Field("LF_Bat_IR", newItem.LF_Bat_IR);
              DynRec.set_Field("LF_Bat_SOC", newItem.LF_Bat_SOC);
              DynRec.set_Field("LF_Bat_SOH", newItem.LF_Bat_SOH);
              DynRec.set_Field("LF_Bat_CCA", newItem.LF_Bat_CCA);
              DynRec.set_Field("LF_Bat_Vol", newItem.LF_Bat_Vol);
            DynRec.set_Field("BatchNumber", newItem.BatchNumber);
            DynRec.set_Field("Remarks", newItem.Remark);

        }
        //private void UpdateBatteryInfo(BatteryInfo newItem)
        //{
        //    // TODO: Implement your update logic here
        //    // Example: call AX or database update
        //}


    }
}