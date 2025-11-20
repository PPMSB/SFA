using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Battery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();

                Check_DataRequest();
                #region Neil - For Edit Battery / Oil Item on submission process
                string claimText = Session["ClaimText"] as string;
                if (claimText == "Awaiting Invoice Chk" || claimText == "Draft")
                {

                    string claimNum = Session["ClaimNum"] as string;

                    if (!string.IsNullOrEmpty(claimNum) && !string.IsNullOrEmpty(claimText))
                    {
                        hidden_LabelClaimNumber.Text = claimNum;
                        hidden_Label_PreviousStatus.Text = claimText;
                        // Optionally clear session if no longer needed
                        Session.Remove("ClaimNum");
                        Session.Remove("ClaimText");
                    }
                }
                #endregion
            }
        }

        private void Check_DataRequest()
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    if ((temp1.Substring(0, 6) == "_WCBA@"))
                    {
                        GridView1.Columns[1].Visible = true;//Invoice button
                        //GridView1.Columns[2].Visible = false;//Invoice label
                        string AccNo_WarrantyType = temp1.Substring(6);//"_WCBA@" + AccNo + "|" + Warranty

                        string[] arr_raw_AccNo_WarrantyType = AccNo_WarrantyType.Split('|');
                        string AccNo = arr_raw_AccNo_WarrantyType[0];
                        string WarrantyType = arr_raw_AccNo_WarrantyType[1];
                        hidden_CustAcc.Text = AccNo;
                        hidden_WarrantyType.Text = WarrantyType;

                        //first time, reload gridview with Search ALL approach
                        GridView1.PageIndex = 0;
                        f_call_ax(0);
                    }
                    else if ((temp1.Substring(0, 5) == "_WCO@"))
                    {
                        getGvOil(0);
                    }
                    else
                    {//default
                        GridView1.Columns[1].Visible = false;
                        GridView1.Columns[2].Visible = true;
                    }
                }
            }
            else
            {
                GridView1.Columns[1].Visible = false;
                GridView1.Columns[2].Visible = true;
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
            hidden_CustAcc.Text = "";
            hidden_WarrantyType.Text = "";
        }

        protected void CheckAcc(object sender, EventArgs e)
        {
            if (HeaderTitle.InnerText != "Battery")
            {
                gvOil.PageIndex = 0;
                getGvOil(0);
            }
            else
            {
                GridView1.PageIndex = 0;
                f_call_ax(0);
            }
        }

        //=====================================================================================
        private void f_call_ax(int PAGE_INDEX)
        {
            // Log on to Microsoft Dynamics AX.
            Axapta DynAx = Function_Method.GlobalAxapta();
            GridView1.DataSource = null;
            GridView1.DataBind();

            try
            {
                int LF_AutoSerialTrans = 30179;
                string FindItemId = TextBox_BatteryID.Text;
                string CustAccNo = hidden_CustAcc.Text;
                string Serial = TextBox_SerialNo.Text;
                string FindItemName = TextBox_BatteryName.Text;

                AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", LF_AutoSerialTrans);
                if (FindItemName != "")
                {
                    var temp_ItemId = WClaim_GET_NewApplicant.getItemId(DynAx, FindItemName);
                    if (FindItemId == "")
                    {
                        FindItemId = temp_ItemId.Item1;
                    }
                }
                if (FindItemId != "")
                {
                    var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30005);//itemid
                    qbr1.Call("value", "*" + FindItemId + "*");
                }
                if (CustAccNo != "")
                {
                    var qbr1_2 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30004);
                    qbr1_2.Call("value", CustAccNo);
                }
                if (Serial != "")
                {
                    var qbr1_3 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30012);
                    qbr1_3.Call("value", "*" + Serial + "*");
                }

                //var qbr1_6 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30025);//WarrantyPeriod
                //qbr1_6.Call("value", "1..13");

                axQueryDataSource1.Call("addSortField", 30005, 1);//itemid , dsc

                AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                //====================================================================================
                DataTable dt = new DataTable(); DataRow row;
                dt.Columns.Add(new DataColumn("No", typeof(string)));
                dt.Columns.Add(new DataColumn("SerialNo", typeof(string)));
                // Jerry 20251119 - Add batchNumber
                dt.Columns.Add(new DataColumn("BatchNumber", typeof(string)));
                // Jerry 20251119 End
                dt.Columns.Add(new DataColumn("InvoiceId", typeof(string)));
                dt.Columns.Add(new DataColumn("CustomerAcc", typeof(string)));
                dt.Columns.Add(new DataColumn("CustomerName", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemId", typeof(string)));
                dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                dt.Columns.Add(new DataColumn("inventLocationId", typeof(string)));

                //====================================================================================
                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //====================================================================================
                while ((bool)axQueryRun1.Call("next"))
                {
                    AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", LF_AutoSerialTrans);
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();
                        row["No"] = countA;
                        string Account = DynRec1.get_Field("CustAccount").ToString();
                        string SerialNum = DynRec1.get_Field("SerialNum").ToString();
                        // Jerry 20251119 - Add batch number
                        string BatchNo = DynRec1.get_Field("LF_BatchNo").ToString();
                        // Jerry 20251119 End
                        string InvoiceId = DynRec1.get_Field("InvoiceId").ToString();
                        string ItemId = DynRec1.get_Field("ItemId").ToString();

                        string InventDimId = DynRec1.get_Field("InventDimId").ToString();
                        //====================================
                        int CustInvoiceJour = 62;
                        AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
                        AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", CustInvoiceJour);

                        var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 21);
                        qbr2.Call("value", InvoiceId);

                        axQueryDataSource2.Call("addSortField", 6, 1);//Invoice Date , dsc
                        AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
                        if ((bool)axQueryRun2.Call("next"))
                        {
                            AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", CustInvoiceJour);
                            //have result. Can show account, serial num and invoice id

                            string ItemName = SFA_GET_PROMO_LAYER2.getItemName(DynAx, ItemId);
                            string CustName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, Account);
                            row["InvoiceId"] = InvoiceId;
                            row["SerialNo"] = SerialNum;
                            // Jerry 20251119 - Add batch number
                            row["BatchNumber"] = BatchNo;
                            // Jerry 20251119 End
                            row["CustomerAcc"] = Account;
                            row["CustomerName"] = CustName;
                            row["ItemId"] = ItemId;
                            row["ItemName"] = ItemName;

                            string BatteryUnit = WClaim_GET_NewApplicant.get_BatteryUnit(DynAx, ItemId);
                            row["Unit"] = BatteryUnit;

                            //string inventLocationId = DynRec2.get_Field("inventLocationId").ToString();//not data in the table
                            string inventLocationId = get_InventDim2(DynAx, InventDimId);

                            row["inventLocationId"] = inventLocationId;
                            dt.Rows.Add(row);

                            DynRec2.Dispose();
                        }

                        // Advance to the next row.
                        //DynRec.Next();
                        DynRec1.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }
            //====================================================================================

            // Log off from Microsoft Dynamics AX.
            FINISH:
                GridView1.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ER_BA_00)
            {
                Function_Method.MsgBox("ER_BA_00: " + ER_BA_00.ToString(), this.Page, this);
            }
        }

        private string get_InventDim2(Axapta DynAx, string InventDimId)
        {
            string InventLocationID = "";
            int InventDim = 698;
            AxaptaObject axQuery1_13 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1_13 = (AxaptaObject)axQuery1_13.Call("addDataSource", InventDim);

            var qbr13 = (AxaptaObject)axQueryDataSource1_13.Call("addRange", 1);
            qbr13.Call("value", InventDimId);//Invoice Id
            AxaptaObject axQueryRun1_13 = DynAx.CreateAxaptaObject("QueryRun", axQuery1_13);
            if ((bool)axQueryRun1_13.Call("next"))
            {
                AxaptaRecord DynRec1_13 = (AxaptaRecord)axQueryRun1_13.Call("Get", InventDim);

                InventLocationID = DynRec1_13.get_Field("InventLocationID").ToString();
                DynRec1_13.Dispose();
            }
            return InventLocationID;
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
            catch (Exception ER_BA_01)
            {
                Function_Method.MsgBox("ER_BA_01: " + ER_BA_01.ToString(), this.Page, this);
            }
        }

        protected void Button_SerialNo_Click(object sender, EventArgs e)
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    if (temp1.Substring(0, 6) == "_WCBA@")//Request from WClaim > Battery
                    {
                        string AccNo_WarrantyType = temp1.Substring(6);//"_WCBA@" + AccNo + "|" + Warranty

                        string[] arr_raw_AccNo_WarrantyType = AccNo_WarrantyType.Split('|');
                        string AccNo = arr_raw_AccNo_WarrantyType[0];
                        string WarrantyType = arr_raw_AccNo_WarrantyType[1];
                        string subClaimType = arr_raw_AccNo_WarrantyType[2];
                        string transport = arr_raw_AccNo_WarrantyType[3];
                        string SerialNo = "";
                        // Jerry 20251119 - add batchNumber
                        string BatchNumber = "";
                        // Jerry 20251119 End

                        Button Button_SerialNo = sender as Button;
                        GridViewRow row = (GridViewRow)Button_SerialNo.NamingContainer;
                        int index = row.RowIndex;

                        if (Button_SerialNo != null)
                        {
                            SerialNo = Button_SerialNo.Text;
                        }
                        // Jerry 20251119 - add batch number
                        BatchNumber = GridView1.Rows[index].Cells[2].Text;

                        string InvoiceId = GridView1.Rows[index].Cells[3].Text;
                        string ItemId = GridView1.Rows[index].Cells[6].Text;
                        string ItemName = GridView1.Rows[index].Cells[7].Text;
                        string Unit = GridView1.Rows[index].Cells[8].Text;

                        //string InvoiceId = GridView1.Rows[index].Cells[2].Text;
                        //string ItemId = GridView1.Rows[index].Cells[5].Text;
                        //string ItemName = GridView1.Rows[index].Cells[6].Text;
                        //string Unit = GridView1.Rows[index].Cells[7].Text;
                        // Jerry 20251119 End
                        string inventLocationId = arr_raw_AccNo_WarrantyType[4];

                        // Jerry 20251119 - Add batch number
                        string DATA = SerialNo + "|" + InvoiceId + "|" + ItemId + "|" + ItemName + "|" + Unit + "|" + inventLocationId + "|" + BatchNumber;
                        // Jerry 20251119 End
                        Session["data_passing"] = "@WCBA_|" + AccNo + "|" + transport + "|" + WarrantyType + "|" + subClaimType + "|" + DATA;
                        
                        if (hidden_Label_PreviousStatus.Text == "Awaiting Invoice Chk" || hidden_Label_PreviousStatus.Text == "Draft")
                        {
                            Session["ClaimNum"] = hidden_LabelClaimNumber.Text;
                            Session["ClaimText"] = hidden_Label_PreviousStatus.Text;

                            Session["PreviousPage"] = "Battery.aspx";
                        }
                        Response.Redirect("WClaim.aspx");
                    }
                }
            }
        }

        private void getGvOil(int PAGE_INDEX)
        {
            // Log on to Microsoft Dynamics AX.
            Axapta DynAx = Function_Method.GlobalAxapta();
            gvOil.DataSource = null;
            gvOil.DataBind();

            HeaderTitle.InnerText = "Lubricant";
            imgHeader.Src = "RESOURCES/Lubricant.png";

            try
            {
                int InventTable = 175;

                AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", InventTable);

                if (TextBox_BatteryName.Text != "")
                {
                    int itemnameFieldID = DynAx.GetFieldId(InventTable, "ItemName");
                    
                    string updatedText = TextBox_BatteryName.Text.Replace(" ", "*");

                    var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", itemnameFieldID);//itemname
                    qbr1.Call("value", updatedText + "*");
                }

                if (TextBox_BatteryID.Text != "")
                {
                    var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 2);//itemid
                    qbr1.Call("value", "*" + TextBox_BatteryID.Text + "*");
                }

                var qbr1_6 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30015);//NewItemType
                qbr1_6.Call("value", "LUBE");

                var qbr1_7 = (AxaptaObject)axQueryDataSource1.Call("addRange", 30012);//LFI_Status
                qbr1_7.Call("value", "Active");
                //var SortItem = WClaim_GET_NewApplicant.get_CustInvoiceTrans_details(DynAx, CustAcc);

                axQueryDataSource1.Call("addSortField", 2, 1);//itemid , dsc

                AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

                DataTable dt = new DataTable(); DataRow row;
                dt.Columns.Add(new DataColumn("No", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemId", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
                dt.Columns.Add(new DataColumn("Unit", typeof(string)));

                int countA = 0;

                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //====================================================================================
                //for (int i = 0; i < SortItem.Item1; i++)
                while ((bool)axQueryRun1.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun1.Call("Get", InventTable);

                    //var qbr = (AxaptaObject)axQueryDataSource1.Call("addRange", 2);//itemid
                    //qbr.Call("value", SortItem.Item2[i]);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();
                        row["No"] = countA;
                        row["ItemId"] = DynRec.get_Field("ItemId").ToString();
                        row["ItemName"] = DynRec.get_Field("ItemName").ToString();
                        row["Unit"] = DynRec.get_Field("QpuItem").ToString();

                        dt.Rows.Add(row);

                        // Advance to the next row.

                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }
            //====================================================================================

            // Log off from Microsoft Dynamics AX.
            FINISH:
                gvOil.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                gvOil.DataSource = dt;
                gvOil.DataBind();
            }
            catch (Exception ER_BA_03)
            {
                Function_Method.MsgBox("ER_BA_03: " + ER_BA_03.ToString(), this.Page, this);
            }
        }

        protected void Button_ItemId_Click(object sender, EventArgs e)
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    if (temp1.Substring(0, 5) == "_WCO@")//Request from WClaim > Oil
                    {
                        var splitTemp1 = temp1.Split('|', '@');
                        string itemId = "";
                        Button Button_ItemId = sender as Button;
                        //
                        GridViewRow row = (GridViewRow)Button_ItemId.NamingContainer;
                        int index = row.RowIndex;

                        itemId = Button_ItemId.Text;
                        string itemName = gvOil.Rows[index].Cells[2].Text;
                        string unit = gvOil.Rows[index].Cells[3].Text;

                        Session["data_passing"] = "@WCO_" + splitTemp1[1] + "|" + splitTemp1[4] + "|" + splitTemp1[2] + "|" + splitTemp1[3] + "|" + itemId + "|" + itemName + "|" + unit + "|" + splitTemp1[5];
                        
                        if (hidden_Label_PreviousStatus.Text == "Awaiting Invoice Chk" || hidden_Label_PreviousStatus.Text == "Draft")
                        {
                            Session["ClaimNum"] = hidden_LabelClaimNumber.Text;
                            Session["ClaimText"] = hidden_Label_PreviousStatus.Text;

                            Session["PreviousPage"] = "Battery.aspx";
                        }

                        Response.Redirect("WClaim.aspx");
                    }
                }
            }
        }

        protected void gvOil_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            getGvOil(e.NewPageIndex);
            gvOil.PageIndex = e.NewPageIndex;
            gvOil.DataBind();
        }
    }
}