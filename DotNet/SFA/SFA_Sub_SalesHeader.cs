using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class SFA : System.Web.UI.Page
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
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                logger.Info($"Attempting to update Sales Order for Customer Account: {temp_cust_acc_no}");

                string customer_name = SFA_GET_SALES_HEADER.get_customer_acc(DynAx, temp_cust_acc_no);//to double check if the account no is correct with the customer name
                if (temp_cust_name == customer_name)
                {//correct data
                    logger.Info($"Customer name verified: {customer_name}");

                    string SalesId = hidden_Label_SO_No.Text;
                    using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("SalesTable"))
                    {
                        DynAx.TTSBegin();
                        DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "SalesId", SalesId));
                        if (DynRec.Found)
                        {
                            string deliverydate = Request.Form[txtDeliveryDate.UniqueID];
                            var temp_deliveryDate = DateTime.ParseExact(deliverydate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            DynRec.set_Field("DeliveryDate", temp_deliveryDate);
                             DynRec.set_Field("CUSTOMERREF", TextBox_References.Text);
                            DynRec.set_Field("NOTES", TextBox_Notes.Text);
                            DynRec.set_Field("DeliveryStreet", hidden_Street.Text);
                            DynRec.set_Field("DeliveryAddress", hidden_Street.Text);
                            DynRec.set_Field("DeliveryZipCode", hidden_ZipCode.Text);
                            DynRec.set_Field("DeliveryCity", hidden_City.Text);
                            DynRec.set_Field("DeliveryState", hidden_State.Text);
                            DynRec.set_Field("DeliveryCountryRegionId", hidden_Country.Text);
                            //
                            DynRec.Call("Update");

                            Function_Method.MsgBox("Update successfully, Sales ID: " + SalesId + " .", this.Page, this);
                        }
                        else
                        {
                            Function_Method.MsgBox("Fail to update, Sales ID: " + SalesId + " .", this.Page, this);
                            logger.Warn($"SalesId: {SalesId} not found for update.");
                        }

                        DynAx.TTSCommit();
                        DynAx.TTSAbort();
                    }
                }
                else
                {//wrong data
                    Function_Method.MsgBox("Customer Account no. in Sales Header have been edited and data is not match with Customer Name. Please check.", this.Page, this);
                    logger.Warn($"Customer Account no. {temp_cust_acc_no} does not match with Customer Name: {temp_cust_name}.");
                }
            }
            catch (Exception ER_SF_29)
            {
                DynAx.TTSAbort();
                logger.Error($"Error in Button_UpdateHeader_Click: {ER_SF_29.Message}");
                Function_Method.MsgBox("ER_SF_29: " + ER_SF_29.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
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
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                logger.Info($"Attempting to save Sales Order for Customer Account: {temp_cust_acc_no}");
                string customer_name = SFA_GET_SALES_HEADER.get_customer_acc(DynAx, temp_cust_acc_no);//to double check if the account no is correct with the customer name
                if (temp_cust_name == customer_name)
                {//correct data
                    logger.Info($"Customer name verified: {customer_name}");
                    AxaptaObject axSalesTable = DynAx.CreateAxaptaObject("AxSalesTable");
                    //
                    string deliverydate = Request.Form[txtDeliveryDate.UniqueID];
                    var DeliveryDate = Function_Method.get_correct_date(GLOBAL.system_checking, deliverydate, false);
                    var temp_deliveryDate = DateTime.ParseExact(DeliveryDate, "dd/MM/yyyy", null);// in lotus use cdat
                    axSalesTable.Call("parmDeliveryDate", temp_deliveryDate);
                    logger.Info($"Delivery date set to: {temp_deliveryDate}");
                    //
                    string temp_Receiver_Name = Label_Receiver_Name.Text;
                    if (temp_Receiver_Name == "")
                    {
                        temp_Receiver_Name = Label_CustName.Text;
                    }
                    axSalesTable.Call("parmDeliveryName", temp_Receiver_Name);
                    logger.Info($"Delivery name set to: {temp_Receiver_Name}");
                    //
                    axSalesTable.Call("parmDeliveryStreet", hidden_Street.Text);
                    axSalesTable.Call("parmDeliveryZipCode", hidden_ZipCode.Text);
                    axSalesTable.Call("parmDeliveryCity", hidden_City.Text);
                    axSalesTable.Call("parmDeliveryState", hidden_State.Text);
                    axSalesTable.Call("parmDeliveryCountryRegionId", hidden_Country.Text);
                    //
                    string temp_PaymTermIdWithName = Label_Payment_Terms.Text.Trim();// temp_PaymTermId + " | " + PaymTermName;
                    string[] arr_temp_PaymTermIdWithName = temp_PaymTermIdWithName.Split('|');
                    axSalesTable.Call("parmPayment", arr_temp_PaymTermIdWithName[0]);
                    logger.Info($"Payment term set to: {arr_temp_PaymTermIdWithName[0]}");
                    //
                    string temp_SuffixCodeWithName = Label_Suffix_Code.Text.Trim();
                    string[] arr_temp_SuffixCodeWithName = temp_SuffixCodeWithName.Split('|');//temp_SuffixCode + " | " + temp_SuffixCodeName;
                    axSalesTable.Call("suffixCode", arr_temp_SuffixCodeWithName[0]);
                    logger.Info($"Suffix code set to: {arr_temp_SuffixCodeWithName[0]}");
                    //
                    axSalesTable.Call("parmCustAccount", temp_cust_acc_no);
                    logger.Info($"Customer account set to: {temp_cust_acc_no}");
                    //
                    if (TextBox_References.Text != "")
                    {
                        axSalesTable.Call("parmCustomerRef", TextBox_References.Text);
                        logger.Info($"Customer reference set to: {TextBox_References.Text}");
                    }
                    // 
                    if (TextBox_Notes.Text != "")
                    {
                        axSalesTable.Call("Notes", TextBox_Notes.Text);
                        logger.Info($"Notes set to: {TextBox_Notes.Text}");
                    }

                    //
                    axSalesTable.Call("save");
                    var temp_newSalesID = axSalesTable.Call("parmSalesID");
                    string newSalesID = temp_newSalesID.ToString();
                    /*un-used
                    axDeliveryDate = axSalesTable.Call("parmReceiptDateRequested", Cdat(deliveryDate)) 
                    axDeliveryDate = axSalesTable.Call("parmShippingDateRequested", Cdat(deliveryDate)) 
                    axSalesStatus = axSalesTable.Call("parmSalesStatus", 1) 
            
                    axSubmitted = axSalesTable.Call("Submitted", Submitted)
                    axPoNo = axSalesTable.call("parmPurchOrderFormNum", poNo)
                    axReturnItemNum = axSalesTable.call("parmReturnItemNum", returnItemNum)
                    */
                    hidden_Label_SO_No.Text = newSalesID;

                    Function_Method.MsgBox("Sales Order: " + newSalesID, this.Page, this);
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:GoToTab('Button_sales_order_section'); ", true);//go to Overall_sales_order_section
                }
                else
                {//wrong data
                    Function_Method.MsgBox("Customer Account no. in Sales Header have been edited and data is not match with Customer Name. Please check.", this.Page, this);
                    logger.Warn($"Customer Account no. {temp_cust_acc_no} does not match with Customer Name: {temp_cust_name}.");
                }
            }
            catch (Exception ER_SF_00)
            {
                logger.Error($"Error in Button_SaveHeader_Click: {ER_SF_00.Message}");
                Function_Method.MsgBox("ER_SF_00: " + ER_SF_00.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        //start of accordion customer
        protected void CheckAccInList(object sender, EventArgs e)
        {
            Session["data_passing"] = "_SFCM@";//SFA > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }
        protected void CheckAcc(object sender, EventArgs e)
        {
            validate();
            Alt_Addr_function();

            Axapta DynAx = new Axapta();

            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            try
            {
                string temp_RecID_List = hidden_alt_address_RecId.Text;
                string[] arr_temp_RecID_List = temp_RecID_List.Split('|');
                var tuple_get_AltAddress_info = SFA_GET_SALES_HEADER.get_Default_Address(DynAx);

                if (!string.IsNullOrEmpty(tuple_get_AltAddress_info.Item1))
                {
                    hidden_Street.Text = tuple_get_AltAddress_info.Item1;
                    hidden_ZipCode.Text = tuple_get_AltAddress_info.Item2;
                    hidden_City.Text = tuple_get_AltAddress_info.Item3;
                    hidden_State.Text = tuple_get_AltAddress_info.Item4;
                    hidden_Country.Text = tuple_get_AltAddress_info.Item5;
                }

                DynAx.Logoff();
            }
            catch (Exception)
            {

                throw;
            }
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
            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

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
                    //
                    //Delivery accordion
                    txtDeliveryDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                    string PaymTermName = SFA_GET_SALES_HEADER.get_PaymTermName(DynAx, temp_PaymTermId);
                    Label_Payment_Terms.Text = temp_PaymTermId + " | " + PaymTermName;

                    string temp_CustomerClass = DynRec.get_Field("CustomerClass").ToString();
                    var temp_SuffixCode = SFA_GET_SALES_HEADER.get_SuffixCode(DynAx, temp_CustomerClass);
                    string temp_SuffixCodeName = SFA_GET_SALES_HEADER.get_SuffixCodeName(DynAx, temp_SuffixCode.Item1.ToString());
                    Label_Suffix_Code.Text = temp_SuffixCode.Item1 + " | " + temp_SuffixCodeName;
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
            catch (Exception ER_SF_01)
            {
                Function_Method.MsgBox("ER_SF_01: " + ER_SF_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }

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
                Alt_Addr_function();

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

                GridView_AltAddress.DataSource = dt;
                GridView_AltAddress.DataBind();
            }
        }

        private void Alt_Addr_function()
        {
            Button_Delivery_Addr.Visible = false;
            hidden_alt_address_RecId.Text = ""; hidden_alt_address.Text = ""; hidden_alt_address_counter.Text = "";

            Axapta DynAx = new Axapta();

            try
            {
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                var tuple_get_AltAddress = SFA_GET_SALES_HEADER.get_AltAddress(DynAx, TextBox_Account.Text);
                if (tuple_get_AltAddress == null)
                {
                    return;
                }
                string[] AltAddress = tuple_get_AltAddress.Item1;
                string[] AltAddressRecId = tuple_get_AltAddress.Item2;
                string[] DefaultAddress = tuple_get_AltAddress.Item4;
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
            catch (Exception ER_SF_02)
            {
                Function_Method.MsgBox("ER_SF_02: " + ER_SF_02.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }
        protected void CheckBox_Changed2(object sender, EventArgs e)
        {
            int counter = 0;
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

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
        //end of sales header section
    }
}