using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
    {
        //*start of FV*/

        protected void Button_SaveFavourite_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {

                string temp_cust_name = Label_CustName.Text;
                string temp_cust_acc_no = TextBox_Account.Text.Trim();
                string customer_name = SFA_GET_SALES_HEADER.get_customer_acc(DynAx, temp_cust_acc_no);//to double check if the account no is correct with the customer name because Textbox can be edit

                if (temp_cust_name == customer_name)//do checking and if matched only proceed
                {
                    foreach (GridViewRow row in GridView_FVOrder.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            TextBox temp_New_Qty = (row.Cells[7].FindControl("TextBox_New_QTY") as TextBox);
                            string New_Qty = temp_New_Qty.Text;
                            if (New_Qty != "")//New Qty
                            {
                                string _CustAccount = temp_cust_acc_no;
                                string _SalesId = hidden_Label_SO_No.Text;
                                int int_SalesId = Convert.ToInt32(_SalesId);
                                string _ItemId = row.Cells[1].Text;
                                string _Qty = New_Qty;
                                DropDownList temp_DropDownList = (row.Cells[6].FindControl("DropDownList_FV_Order") as DropDownList);
                                string _SalesUnit = temp_DropDownList.Text;
                                string _LF_UOM = "";

                                AxaptaObject DomCOMSalesLine = DynAx.CreateAxaptaObject("DOMCOMSalesLine");

                                DomCOMSalesLine.Call("InsertNewOrders", _CustAccount, _SalesId, _ItemId, _Qty, _SalesUnit, _LF_UOM);
                            }
                        }
                    }
                    Function_Method.MsgBox("Favourite order saved.", this.Page, this);

                    sales_order_section_general.Visible = true; Sales_order_section.Visible = false; FV_order_section.Visible = false;
                    sales_line_view();

                }
                else
                {
                    Function_Method.MsgBox("Customer account no. in Sales Header and data is not match. Please check the input for Customer acc no. at the Sales Header section.", this.Page, this);

                }
            }
            catch (Exception ER_SF_05)
            {
                Function_Method.MsgBox("ER_SF_05: " + ER_SF_05.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void Button_CancelFavourite_Click(object sender, EventArgs e)
        {
            sales_order_section_general.Visible = true; Sales_order_section.Visible = false; FV_order_section.Visible = false;
            sales_line_view();
        }

        private void GridView_FVOrder_AdditonalData()
        {

            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                foreach (GridViewRow row in GridView_FVOrder.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {

                        DropDownList temp_DropDownList = (row.Cells[6].FindControl("DropDownList_FV_Order") as DropDownList);
                        temp_DropDownList.Items.Clear();
                        string temp_Unit = row.Cells[4].Text;
                        string temp_ItemId = row.Cells[1].Text;

                        List<ListItem> List_NewUnit = SFA_GET_FV_ORDER.get_NewUnit(DynAx, temp_Unit, temp_ItemId);
                        temp_DropDownList.Items.AddRange(List_NewUnit.ToArray());
                    }
                }
            }
            catch (Exception ER_SF_04)
            {
                Function_Method.MsgBox("ER_SF_04: " + ER_SF_04.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        private void FV_order()
        {
            //
            sales_order_section_general.Visible = false; Sales_order_section.Visible = false;
            FV_order_section.Visible = true;
            GridView_FVOrder.DataSource = null; GridView_FVOrder.DataBind();

            //
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);


            string ItemId, ItemName, SalesPrice, Unit, Quantity, InvDate = "";
            //FOCITEM
            try
            {
                int LF_CustFavourOrders = 30254;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", LF_CustFavourOrders);

                var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 30001);//CustAcc
                qbr1.Call("value", TextBox_Account.Text);
                axQueryDataSource.Call("addSortField", 30002, 1);//invoice date, dsc
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

                //===========================================
                DataTable dt = new DataTable();
                int data_count = 6;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Item Id"; N[2] = "Item Name";
                N[3] = "Invoice Date"; N[4] = "Unit"; N[5] = "Quantity";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================

                DataRow row;
                int countA = 0;
                while ((bool)axQueryRun.Call("next"))
                {
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", LF_CustFavourOrders);
                    countA = countA + 1;

                    ItemId = DynRec.get_Field("ItemId").ToString();
                    ItemName = DynRec.get_Field("Name").ToString();
                    string temp_InvDate = DynRec.get_Field("InvoiceDate").ToString();

                    if (temp_InvDate != "")
                    {
                        string[] arr_temp_InvDate = temp_InvDate.Split(' ');//date + " " + time;
                        string Raw_InvDate = arr_temp_InvDate[0];
                        InvDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_InvDate, true);

                    }

                    SalesPrice = DynRec.get_Field("SalesPrice").ToString();
                    Unit = DynRec.get_Field("SalesUnit").ToString();
                    Quantity = DynRec.get_Field("Qty").ToString();
                    //FOCITEM= DynRec.get_Field("FOC_Item").ToString();

                    row = dt.NewRow();
                    //transfer to grid
                    row["No."] = countA;
                    row["Item Id"] = ItemId;
                    row["Item Name"] = ItemName;
                    row["Invoice Date"] = InvDate;
                    row["Unit"] = Unit;
                    row["Quantity"] = Quantity;
                    dt.Rows.Add(row);
                    DynRec.Dispose();
                }
                if (countA == 0)//grid empty
                {
                    //close FV order
                    sales_order_section_general.Visible = true; Sales_order_section.Visible = false; FV_order_section.Visible = false;
                    sales_line_view();
                    Function_Method.MsgBox("There is no Favourite Order for this customer.", this.Page, this);
                    return;
                }
                GridView_FVOrder.DataSource = dt;
                GridView_FVOrder.DataBind();
                GridView_FVOrder_AdditonalData();
            }
            catch (Exception ER_SF_03)
            {
                Function_Method.MsgBox("ER_SF_03: " + ER_SF_03.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }


        }
        //end of FV
    }
}