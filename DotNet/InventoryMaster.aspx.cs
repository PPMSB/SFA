using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class InventoryMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                clear_variable();
                //TextBox_Account.Attributes.Add("onkeypress", "return controlEnter('" + Button1.ClientID + "', event)");
                SetFocus(TextBox_Account);
                TextBox_Account.Attributes.Add("autocomplete", "off");

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
                   null, null, null, RocTinTag2,
                   NewProduct2
                   );
                Check_DataRequest();

                //first time, reload gridview with Search ALL approach
                GridView1.PageIndex = 0;
                GridView1.Columns[5].Visible = false;//hide on-order (request by Keegan 15/12/2020)
                f_call_ax(0);
            }
        }
        private void Check_DataRequest()
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {

                    if (temp1.Substring(0, 6) == "_SFIM@")//Request from SFA > InventoryMaster
                    {
                        GridView1.Columns[1].Visible = true;//Inventory Id button
                        GridView1.Columns[2].Visible = false;//Inventory Id label
                    }

                    if (temp1.Substring(0, 6) == "_EOIM@")//Request from EOR > InventoryMaster
                    {
                        GridView1.Columns[1].Visible = true;//Inventory Id button
                        GridView1.Columns[2].Visible = false;//Inventory Id label
                    }

                    if (temp1.Substring(0, 6) == "_SQIM@")//Request from SQ > InventoryMaster
                    {
                        GridView1.Columns[1].Visible = true;//Inventory Id button
                        GridView1.Columns[2].Visible = false;//Inventory Id label
                    }
                }
            }
            else
            {
                GridView1.Columns[1].Visible = false;// Inventory Id button
                GridView1.Columns[2].Visible = true;//Inventory Id label
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
            Axapta DynAx = Function_Method.GlobalAxapta();

            //var tuple_get_Empl_Id = SFA_GET_SALES_ORDER_2.get_Empl_Id(DynAx, GLOBAL.user_id);
            //user inventory only can view item base on thier own dimension
            GetInventDimLocation(DynAx);
            int fieldId;
            switch (DropDownList1.SelectedItem.Text)
            {
                case "Item Name":
                    fieldId = 3;//ItemName
                    break;
                case "Item Id.":
                    fieldId = 2;//ItemId
                    break;

                default:
                    fieldId = 3;//ItemName
                    break;
            }
            string fieldValue = "*" + TextBox_Account.Text.Trim() + "*";

            try
            {
                int InventTable = 175;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", InventTable);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);
                qbr.Call("value", fieldValue);

                //if (!string.IsNullOrEmpty(tuple_get_Empl_Id.Item3[0]))
                //{
                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", "21");//dimension
                if (ddlLocation.SelectedItem.Value == "")
                {
                    qbr1.Call("value", "HO");
                }
                else
                {
                    qbr1.Call("value", ddlLocation.SelectedItem.Text);
                }
                //}

                axQueryDataSource.Call("addSortField", fieldId, 0);//*fieldId, asc

                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 5;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Item Id"; N[2] = "Item Name";
                N[3] = "On-hand"; N[4] = "On-order";

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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", InventTable);
                    string temp_LFI_WEBONLY = DynRec.get_Field("LFI_WEBONLY").ToString();
                    //for EOR, dont check LFI_WebOnly =====================================================================================================
                    string temp = Session["data_passing"].ToString();
                    if (temp != "")
                    {
                        if (temp.Length >= 6)
                        {
                            if (temp.Substring(0, 6) == "_EOIM@")//Request from EOR > InventoryMaster
                            {
                                temp_LFI_WEBONLY = "1";//do not check
                            }
                        }
                    }
                    //======================================================================================================================================
                    if (temp_LFI_WEBONLY == "0")
                    {
                        goto END;
                    }
                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string ItemId = DynRec.get_Field("ItemId").ToString();

                        row["Item Id"] = ItemId;
                        row["Item Name"] = DynRec.get_Field("ItemName").ToString();

                        var tuple_onhand_onorder = get_tuple_onhand_onorder(DynAx, ItemId);
                        row["On-hand"] = tuple_onhand_onorder.Item1.ToString();
                        row["On-order"] = tuple_onhand_onorder.Item2.ToString();

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

        private Tuple<int, int> get_tuple_onhand_onorder(Axapta DynAx, string ItemId)
        {
            int InventSum = 174;
            int onhand = 0;
            int onorder = 0;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");

            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", InventSum);

            var qbr1 = (AxaptaObject)axQueryDataSource1.Call("addRange", 1);//ItemId
            qbr1.Call("value", ItemId);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            // Loop through the set of retrieved records.
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun.Call("Get", InventSum);

                onhand += Convert.ToInt32(DynRec1.get_Field("AvailPhysical").ToString());
                onorder += Convert.ToInt32(DynRec1.get_Field("OnOrder").ToString());
                // Advance to the next row.
                DynRec1.Dispose();
            }
            return new Tuple<int, int>(onhand, onorder);
        }

        private List<ListItem> get_InventDimLocation(Axapta DynAx)
        {
            List<ListItem> List_Location = new List<ListItem>();
            HashSet<string> uniqueInventDimIds = new HashSet<string>();

            int InventDim = 698;
            AxaptaObject axQuery3 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource3 = (AxaptaObject)axQuery3.Call("addDataSource", InventDim);
            AxaptaObject axQueryRun3 = DynAx.CreateAxaptaObject("QueryRun", axQuery3);

            List_Location.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun3.Call("next"))
            {
                AxaptaRecord DynRec3 = (AxaptaRecord)axQueryRun3.Call("Get", InventDim);
                string temp_InventDimId = DynRec3.get_Field("inventLocationId").ToString();

                if (!uniqueInventDimIds.Contains(temp_InventDimId))
                {
                    uniqueInventDimIds.Add(temp_InventDimId);
                    List_Location.Add(new ListItem(temp_InventDimId));
                }

                DynRec3.Dispose();
            }

            return List_Location;
        }


        protected void Button_ItemId_Click(object sender, EventArgs e)
        {
            string temp1 = GLOBAL.data_passing.ToString();
            if (temp1 != "")//request from other module
            {
                if (temp1.Length >= 6)
                {
                    if (temp1.Substring(0, 6) == "_SFIM@")//Request from SFA > InventoryMaster
                    {
                        string SO_Id = temp1.Substring(6);//_SFIM@ + Item_Id

                        string selected_ItemId = "";
                        Button ButtonItemId = sender as Button;
                        if (ButtonItemId != null)
                        {
                            selected_ItemId = ButtonItemId.Text;
                        }
                        Session["data_passing"] = "@SFIM_" + SO_Id + "|" + selected_ItemId;
                        //Response.Redirect("SFA.aspx");
                        string dataToSendBack = "@SFIM_" + SO_Id + "|" + selected_ItemId;
                        string script = $@"
                     if (window.opener && !window.opener.closed) {{
                        window.opener.receiveDataFromInventoryMaster('{dataToSendBack}');
                        window.opener.focus();
                        window.close();
                     }}";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ReturnToOpener", script, true);
                    }

                    if (temp1.Substring(0, 6) == "_EOIM@")//Request from EOR > InventoryMaster
                    {
                        string selected_ItemId = ""; string ItemName = "";
                        Button ButtonItemId = sender as Button;
                        if (ButtonItemId != null)
                        {
                            string ClientID = ButtonItemId.ClientID;

                            string[] arr_ClientID = ClientID.Split('_');
                            int arr_count = arr_ClientID.Count();
                            int ClientRow = Convert.ToInt32(arr_ClientID[arr_count - 1]);

                            ItemName = GridView1.Rows[ClientRow].Cells[3].Text;
                            selected_ItemId = ButtonItemId.Text;
                        }
                        Session["data_passing"] = "@EOIM_" + selected_ItemId + "|" + ItemName;
                        Response.Redirect("EOR.aspx");
                    }

                    if (temp1.Substring(0, 6) == "_SQIM@")//Request from SFA > InventoryMaster
                    {
                        string SO_Id = temp1.Substring(6);//_SFIM@ + Item_Id

                        string selected_ItemId = "";
                        Button ButtonItemId = sender as Button;
                        if (ButtonItemId != null)
                        {
                            selected_ItemId = ButtonItemId.Text;
                        }
                        Session["data_passing"] = "@SQIM_" + SO_Id + "|" + selected_ItemId;
                        Response.Redirect("SalesQuotation.aspx");
                    }
                }
            }
        }

        private void GetInventDimLocation(Axapta DynAx)
        {
            List<ListItem> inventDimLocation = new List<ListItem>();
            inventDimLocation = get_InventDimLocation(DynAx);
            ddlLocation.Items.AddRange(inventDimLocation.ToArray());
        }
    }
}