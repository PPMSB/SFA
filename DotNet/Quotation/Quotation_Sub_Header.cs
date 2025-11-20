using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class SalesQuotation : System.Web.UI.Page
    {
        //start of sales header section
        protected void Button_UpdateHeader_Click(object sender, EventArgs e)
        {
            string temp_cust_name = Label_CustName.Text;
            string temp_cust_acc_no = TextBox_Account.Text.Trim();
            if (temp_cust_name == "")
            {
                Function_Method.MsgBox("There is no data.", this.Page, this);
                return;
            }
            Axapta DynAx = Function_Method.GlobalAxapta();
            try
            {
                string customer_name = Quotation_Get_Header.get_customer_acc(DynAx, temp_cust_acc_no);//to double check if the account no is correct with the customer name
                if (temp_cust_name == customer_name)
                {//correct data

                    string QuotationId = hidden_Label_SQ_No.Text;
                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesQuotationTable"))
                    {
                        DynAx.TTSBegin();
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "QuotationId", QuotationId));
                        if (DynRec.Found)
                        {
                            //DynRec.set_Field("CUSTOMERREF", TextBox_References.Text);
                            //DynRec.set_Field("NOTES", TextBox_Notes.Text);
                            DynRec.set_Field("DeliveryStreet", hidden_Street.Text);
                            DynRec.set_Field("DeliveryAddress", hidden_Street.Text);
                            DynRec.set_Field("DeliveryZipCode", hidden_ZipCode.Text);
                            DynRec.set_Field("DeliveryCity", hidden_City.Text);
                            DynRec.set_Field("DeliveryState", hidden_State.Text);
                            DynRec.set_Field("DeliveryCountryRegionId", hidden_Country.Text);
                            //
                            DynRec.Call("Update");

                            Function_Method.MsgBox("Update successfully, Quotation ID: " + QuotationId + " .", this.Page, this);
                        }
                        else
                        {
                            Function_Method.MsgBox("Fail to update, Quotation ID: " + QuotationId + " .", this.Page, this);
                        }

                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                }
                else
                {//wrong data
                    Function_Method.MsgBox("Customer Account no. in Quotation Header have been edited and data is not match with Customer Name. Please check.", this.Page, this);
                }
            }
            catch (Exception ER_SQ_03)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_SQ_03: " + ER_SQ_03.ToString(), this.Page, this);
            }
        }
        //==//==//==//==//==//==//==//==//==//==//==//==//==//==//==//==

        //start of sales header section
        protected void Button_SaveHeader_Click(object sender, EventArgs e)
        {
            string temp_cust_name = Label_CustName.Text;
            string temp_cust_acc_no = TextBox_Account.Text.Trim();
            if (temp_cust_name == "")
            {
                Function_Method.MsgBox("There is no data.", this.Page, this);
                return;
            }

            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesQuotationTable"))
                {
                    DynAx.TTSBegin();
                    string customer_name = Quotation_Get_Header.get_customer_acc(DynAx, temp_cust_acc_no);//to double check if the account no is correct with the customer name
                    if (temp_cust_name == customer_name)
                    {//correct data
                        var id = DynRec.Call("setQuotationId");
                        string quotationid = DynRec.get_Field("QuotationId").ToString();
                        hidden_QuotationID.Text = quotationid;
                        DynRec.set_Field("QuotationName", Label_CustName.Text);
                        DynRec.set_Field("CustAccount", temp_cust_acc_no);
                        DynRec.set_Field("InvoiceAccount", temp_cust_acc_no);
                        DynRec.set_Field("DeliveryAddress", Label_Address.Text);
                        DynRec.set_Field("SalesResponsible", lblSalesResponsible.Text);
                        string deliverydate = Request.Form[txtDlvDate.UniqueID];
                        var DeliveryDate = Function_Method.get_correct_date(GLOBAL.system_checking, deliverydate, false);
                        var temp_deliveryDate = DateTime.ParseExact(DeliveryDate, "dd/MM/yyyy", null);// in lotus use cdat
                        DynRec.set_Field("DeliveryDate", temp_deliveryDate);
                        DynRec.set_Field("QuotationFollowUpDate", temp_deliveryDate);
                        DynRec.set_Field("ShippingDateRequested", temp_deliveryDate);//mandatory field
                        DynRec.set_Field("DeliveryStreet", hidden_Street.Text);
                        DynRec.set_Field("CurrencyCode", "MYR");//mandatory field
                        DynRec.set_Field("LanguageId", "en-my");//mandatory field
                        DynRec.set_Field("DeliveryZipCode", hidden_ZipCode.Text);
                        DynRec.set_Field("DeliveryCity", hidden_City.Text);
                        DynRec.set_Field("DeliveryState", hidden_State.Text);
                        DynRec.set_Field("DeliveryCounty", hidden_Country.Text);

                        hidden_EmployeeID.Text = lblSalesResponsible.Text;
                        //string temp_PaymTermIdWithName = Label_Payment_Terms.Text.Trim();// temp_PaymTermId + " | " + PaymTermName;
                        //string[] arr_temp_PaymTermIdWithName = temp_PaymTermIdWithName.Split('|');
                        //DynRec.set_Field("Payment", arr_temp_PaymTermIdWithName[0]);

                        DynRec.Call("insert");
                        DynAx.TTSCommit();

                        Label_SQ_No.Text = quotationid;
                        Function_Method.MsgBox("Quotation: " + quotationid, this.Page, this);
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_quotation_section'); ", true);//go to Overall_sales_order_section                                                                                                                                                //
                    }
                    else
                    {//wrong data
                        Function_Method.MsgBox("Customer Account no. in Quotation Header not match with Customer Name.", this.Page, this);
                    }
                }
                DynAx.TTSCommit();
                DynAx.TTSAbort();
            }
            catch (Exception ER_SQ_04)
            {
                Function_Method.MsgBox("ER_SQ_04: " + ER_SQ_04.ToString(), this.Page, this);
            }
        }

        //start of accordion customer
        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_SQCM@";//SFA > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }
        protected void CheckAcc(object sender, EventArgs e)
        {
            validate();
            Alt_Addr_function();
        }

        private void validate()
        {
            string fieldValue = TextBox_Account.Text.Trim();
            clear_sales_header(); clear_sales_line(); TextBox_Account.Text = fieldValue;//after clear all, rewrite back

            if (fieldValue == "")
            {
                Function_Method.MsgBox("There is no account number", this.Page, this);
                return;
            }
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                int CustTable = 77;
                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

                var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//AccountNum
                qbr.Call("value", fieldValue);
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                if ((bool)axQueryRun.Call("next"))//use if only one record
                {
                    //Customer accordion
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                    Label_CustName.Text = DynRec.get_Field("Name").ToString();
                    Label_Address.Text = DynRec.get_Field("Address").ToString();
                    lblSalesResponsible.Text = DynRec.get_Field("EmplId").ToString();

                    //
                    //Delivery accordion
                    txtDlvDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    Label_Receiver_Name.Text = Label_CustName.Text;
                    Label_Delivery_Addr.Text = "-same as primary address-";

                    hidden_Street.Text = DynRec.get_Field("Street").ToString();
                    hidden_ZipCode.Text = DynRec.get_Field("Zipcode").ToString();
                    hidden_City.Text = DynRec.get_Field("City").ToString();
                    hidden_State.Text = DynRec.get_Field("State").ToString();
                    hidden_Country.Text = DynRec.get_Field("CountryRegionId").ToString();
                    //
                    //Sales Order accordion
                    string temp_PaymTermId = DynRec.get_Field("PaymTermId").ToString();
                    string PaymTermName = Quotation_Get_Header.get_PaymTermName(DynAx, temp_PaymTermId);
                    //Label_Payment_Terms.Text = temp_PaymTermId + " | " + PaymTermName;

                    // record exist
                    /*
                    checkun.Visible = true;
                    
                    shwimg.ImageUrl = "RESOURCES/ok.png";
                    lblmsg.Text = "Account Exist.";
                    */
                    //
                    checkun.Visible = false;//true dont need show
                    DynRec.Dispose();
                }
                else
                {
                    checkun.Visible = true;
                    shwimg.ImageUrl = "RESOURCES/not_ok.png";
                    lblmsg.Text = "Account Not Exist..!!";
                }
            }
            catch (Exception ER_SQ_01)
            {
                Function_Method.MsgBox("ER_SQ_01: " + ER_SQ_01.ToString(), this.Page, this);
            }
        }
        //start of delivery

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
            Axapta DynAx = Function_Method.GlobalAxapta();

            try
            {
                var tuple_get_AltAddress = Quotation_Get_Header.get_AltAddress(DynAx, TextBox_Account.Text);
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
                            Button_Delivery_Addr.Visible = true;
                        }
                        temp_count = temp_count + 1;
                    }
                }
                hidden_alt_address_counter.Text = temp_count.ToString();
            }
            catch (Exception ER_SQ_02)
            {
                Function_Method.MsgBox("ER_SQ_02: " + ER_SQ_02.ToString(), this.Page, this);
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
                            Label_Delivery_Addr.Text = "-same as primary address-";
                        }
                        else
                        {
                            Label_Delivery_Addr.Text = selected_address;
                        }
                        //need to update hidden field
                        Axapta DynAx = Function_Method.GlobalAxapta();

                        string temp_RecID_List = hidden_alt_address_RecId.Text;
                        string[] arr_temp_RecID_List = temp_RecID_List.Split('|');
                        var tuple_get_AltAddress_info = Quotation_Get_Header.get_AltAddress_info(DynAx, arr_temp_RecID_List[counter]);

                        hidden_Street.Text = tuple_get_AltAddress_info.Item1;
                        hidden_ZipCode.Text = tuple_get_AltAddress_info.Item2;
                        hidden_City.Text = tuple_get_AltAddress_info.Item3;
                        hidden_State.Text = tuple_get_AltAddress_info.Item4;
                        hidden_Country.Text = tuple_get_AltAddress_info.Item5;

                        GridView_AltAddress.Visible = false;
                        Button_Delivery_Addr.Text = "Alt. Addr.";
                        //row.BackColor = Color.FromName("#ff8000");
                    }
                }
                counter = counter + 1;
            }
        }
        //end of sales header section

    }
}