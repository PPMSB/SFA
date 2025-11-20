using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class Payment : System.Web.UI.Page
    {
        private void showStatement()
        {
            gvStatement.DataSource = null;
            gvStatement.DataBind();
            txtCustAcc.Text = "";
            string temp1 = Session["flag_temp"].ToString();
            if (temp1.Length >= 6)//correct size
            {
                if (temp1.Substring(0, 6) == "@PAC3_")
                {
                    string raw_data = temp1.Substring(6);
                    string[] arr_raw_data = raw_data.Split('|');
                    string selected_Account = arr_raw_data[1];

                    txtCustAcc.Text = selected_Account;

                    Session["flag_temp"] = 0;
                }
                OverviewSL(0, txtCustAcc.Text);
            }
        }

        protected void txtCustAcc_Changed(object sender, EventArgs e)
        {
            string temp_CustAcc = txtCustAcc.Text.Trim();
            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                string temp_CustName = Payment_GET_JournalLine_AddLine.get_CustName(DynAx, temp_CustAcc);
                if (temp_CustName == "" && txtCustAcc.Text == "")
                {
                    Function_Method.MsgBox("Invalid Customer Account", this.Page, this);
                    btnSearch.Visible = false;
                }
                else
                {
                    btnSearch.Visible = true;
                }
            }
            catch (Exception ER_PA_16)
            {
                Function_Method.MsgBox("ER_PA_16: " + ER_PA_16.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void CheckAccInList3(object sender, EventArgs e)
        {
            string SalesmanId = ddlCustomerAcc.SelectedValue.ToString();

            Session["data_passing"] = "_PAC3@" + SalesmanId;//Payment > CustomerMaster
            Response.Redirect("CustomerMaster.aspx");
        }

        private void OverviewStatement(int PAGE_INDEX, string custAccount)
        {
            gvStatement.DataSource = null;
            gvStatement.DataBind();

            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                DataTable dt = new DataTable();
                int data_count = 3;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Statement"; N[2] = "Description";
                //N[3] = "Account No."; N[4] = "Invoice"; N[5] = "Invoice Date";
                //N[6] = "Due Date"; N[7] = "Invoice Amount"; N[8] = "Outstanding Amount";
                //N[9] = "Current Payment"; N[10] = "Balance Outstanding";

                for (int i = 0; i < data_count; i++)
                {
                    dt.Columns.Add(new DataColumn(N[i], typeof(string)));
                }
                //===========================================
                DataRow row;
                int countA = 0;
                string year = DateTime.Now.Year.ToString();
                int startA = Function_Method.paging_grid(PAGE_INDEX)[0];
                int endA = Function_Method.paging_grid(PAGE_INDEX)[1];
                //===========================================
                // Loop through the set of retrieved records.
                string strUserAgent = Request.UserAgent.ToLower();
                bool isAppleDevice = strUserAgent.Contains("iphone") || strUserAgent.Contains("ipad");
                //===========================================
                //Display link if is apple user
                countA = 1;
                if (countA >= startA && countA <= endA)
                {
                    if (!Directory.Exists("e:/DN-Statement/" + year))
                    {
                        Directory.CreateDirectory("e:/DN-Statement/" + year);
                    }
                    string[] filePaths = Directory.GetFiles("e:/DN-Statement/" + year + "/");//get current year statement
                    foreach (string filePath in filePaths)
                    {
                        row = dt.NewRow();
                        //row["No."] = countA;
                        string filename = Path.GetFileName(filePath);
                        string fileWithoutEx = Path.GetFileNameWithoutExtension(filePath);
                        var fileSplit = fileWithoutEx.Split('_');
                        string date = fileSplit[0].ToString();
                        int month = Convert.ToInt32(date.Substring(4, 2));
                        System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                        string strMonthName = mfi.GetMonthName(month).ToString();
                        if (fileSplit[1].ToString() == custAccount) //if userid correct then show salesman statement
                        {
                            // Generate the download URL
                            string dataPassing = "_CUSTACC@" + fileSplit[0] + "|" + fileSplit[1];
                            string downloadUrl = $"DownloadFile.aspx?file={HttpUtility.UrlEncode(dataPassing)}";
                            // Set the Statement field to the download URL
                            row["Statement"] = $"<a href=\"{downloadUrl}\" target=\"_blank\" class=\"apple-link\">{filename}</a>"; // Set the URL directly

                            row["Description"] = strMonthName + " " + year;
                            countA++;
                            dt.Rows.Add(row);
                        }
                    }

                    string pastYear = DateTime.Now.AddYears(-1).ToString("yyyy");
                    string[] filePathsPastYear = Directory.GetFiles("e:/DN-Statement/" + pastYear + "/");//get past year statement
                    foreach (string filePath in filePathsPastYear.Reverse())//reverse data
                    {
                        row = dt.NewRow();
                        //row["No."] = countA;
                        string filename = Path.GetFileName(filePath);
                        string fileWithoutEx = Path.GetFileNameWithoutExtension(filePath);
                        var fileSplit = fileWithoutEx.Split('_');
                        string date = fileSplit[0].ToString();
                        int month = Convert.ToInt32(date.Substring(4, 2));
                        System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                        string strMonthName = mfi.GetMonthName(month).ToString();
                        if (countA <= 18) //ONLY show 18 months of statement BEFORE current year
                        {
                            if (fileSplit[1].ToString() == custAccount) //if userid correct then show salesman statement
                            {
                                // Generate the download URL
                                string dataPassing = "_CUSTACC@" + fileSplit[0] + "|" + fileSplit[1];
                                string downloadUrl = $"DownloadFile.aspx?file={HttpUtility.UrlEncode(dataPassing)}";
                                // Always create a clickable link
                                row["Statement"] = $"<a href=\"{downloadUrl}\" target=\"_blank\" class=\"apple-link\">{filename}</a>";

                                row["Description"] = strMonthName + " " + pastYear;
                                countA++;
                                dt.Rows.Add(row);
                            }
                        }
                    }

                    var dataView = new DataView(dt);
                    dataView.Sort = "Statement DESC";
                    dt = dataView.ToTable();
                    if (dt.Rows.Count.ToString() == "0")
                    {
                        ErrMsg.Visible = true;
                        ErrMsg.Text = "No statement found.";
                    }
                    //dt = dt.DefaultView.ToTable();
                }
                gvStatement.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID
                gvStatement.DataSource = dt;
                gvStatement.DataBind();
            }
            catch (Exception ER_PA_14)
            {
                Function_Method.MsgBox("ER_PA_14: " + ER_PA_14.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void gvStatement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            OverviewStatement(e.NewPageIndex, "");
            gvStatement.PageIndex = e.NewPageIndex;
            gvStatement.DataBind();
        }

        protected void Button_Statement_Click(object sender, EventArgs e)
        {
            Button Button_Statement = sender as Button;
            if (Button_Statement == null) return;
            string custAccount = Button_Statement.Text;
            var fileNameSplit = custAccount.Split('_', '.');
            if (fileNameSplit.Length < 2)
            {
                // Handle error: Invalid format
                return;
            }
            string timestamp = DateTime.Now.Ticks.ToString();
            string dataPassing = Request.Browser.IsMobileDevice
                ? "_CUSTACC@" + fileNameSplit[0] + "|" + fileNameSplit[1] + "|" + timestamp
                : "_CUSTACC@" + fileNameSplit[0] + "|" + fileNameSplit[1];
            string strUserAgent = Request.UserAgent.ToLower();
            bool isAppleDevice = strUserAgent.Contains("iphone") || strUserAgent.Contains("ipad");
            string target = isAppleDevice ? "_self" : "_blank";
            string script = $"window.open('DownloadFile.aspx?file={HttpUtility.UrlEncode(dataPassing)}', '{target}');";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "openDownload", script, true);
        }

        protected void Button_Search_Click(object sender, EventArgs e)//renamed as Add Sales Line
        {
            string fieldName = "";
            switch (ddlCustomerAcc.SelectedItem.Text)
            {
                case "Account No.":
                    fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                    break;
                case "Customer Name":
                    fieldName = "CUSTNAME";//CUSTNAME
                    break;

                default:
                    fieldName = "";
                    break;
            }
            Session["flag_temp"] = 2;//List All 
            customer_Section.Visible = true;
            gvCustomerList.Visible = true;
            ErrMsg.Visible = false;
            Enquiries_section_Statement.Visible = false;
            OverviewSL(0, fieldName);
        }

        private void OverviewSL(int PAGE_INDEX, string fieldName)
        {
            gvCustomerList.DataSource = null;
            gvCustomerList.DataBind();

            Axapta DynAx = new Axapta();
            try
            {
                // Log on to Microsoft Dynamics AX.
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                    (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                int CustTable = 77;

                AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
                AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

                string temp_SearchValue = "*" + txtCustAcc.Text.Trim() + "*";
                if (fieldName != "" && temp_SearchValue != "")
                {
                    if (fieldName == "CUSTNAME")
                    {
                        var qbr3_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 2);
                        qbr3_1.Call("value", temp_SearchValue);
                    }
                    if (fieldName == "CUSTACCOUNT")
                    {
                        var qbr3_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);
                        qbr3_2.Call("value", temp_SearchValue);
                    }
                }
                axQueryDataSource.Call("addSortField", 1, 1);//SalesId, descending
                AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                //===========================================
                DataTable dt = new DataTable();
                int data_count = 7;
                string[] N = new string[data_count];
                N[0] = "No."; N[1] = "Customer Account"; N[2] = "Customer";
                N[3] = "Address"; N[4] = "Phone"; N[5] = "Salesman"; N[6] = "Remark";
                //N[6] = "LFI Status"; N[7] = "Submitted"; 
                //N[9] = "Remark";
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
                    AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);

                    countA = countA + 1;

                    if (countA >= startA && countA <= endA)
                    {
                        row = dt.NewRow();

                        row["No."] = countA;
                        string temp_SalesId = DynRec.get_Field("EmplId").ToString();
                        //row["Sales Id"] = temp_SalesId;
                        row["Customer Account"] = DynRec.get_Field("AccountNum").ToString();
                        row["Customer"] = DynRec.get_Field("Name").ToString();
                        row["Address"] = DynRec.get_Field("Address").ToString();
                        row["Phone"] = DynRec.get_Field("Phone").ToString();
                        row["Salesman"] = DynRec.get_Field("EmplName").ToString();
                        row["Remark"] = DynRec.get_Field("Remark").ToString();

                        dt.Rows.Add(row);
                        // Advance to the next row.
                        DynRec.Dispose();
                    }
                    if (countA > endA)
                    {
                        goto FINISH;//speed up process
                    }
                }

            // Log off from Microsoft Dynamics AX.
            FINISH:
                gvCustomerList.VirtualItemCount = countA;
                DynAx.Logoff();
                //Data-Binding with our GRID

                gvCustomerList.DataSource = dt;
                gvCustomerList.DataBind();
            }
            catch (Exception ER_PA_17)
            {
                Function_Method.MsgBox("ER_PA_17: " + ER_PA_17.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void gvCustomerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (txtCustAcc.Text == "")
                {
                    OverviewSL(e.NewPageIndex, "");
                }
                else
                {
                    string fieldName = "";
                    switch (ddlCustomerAcc.SelectedItem.Text)
                    {
                        case "Customer Account No.":
                            fieldName = "CUSTACCOUNT";//CUSTACCOUNT
                            break;
                        case "Customer Name":
                            fieldName = "CUSTNAME";//CUSTNAME
                            break;

                        default:
                            fieldName = "";
                            break;
                    }
                    OverviewSL(e.NewPageIndex, fieldName);
                }
                gvCustomerList.PageIndex = e.NewPageIndex;
                gvCustomerList.DataBind();
            }
            catch (Exception ER_PA_18)
            {
                Function_Method.MsgBox("ER_PA_18: " + ER_PA_18.ToString(), this.Page, this);
            }
        }

        protected void btnCusAccount_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            GLOBAL.Company = GLOBAL.switch_Company;
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential
                (GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

            Button CustAccount = sender as Button;
            string temp_CustAcc = CustAccount.Text;

            gvStatement.DataSource = null;
            gvStatement.DataBind();

            if (temp_CustAcc != "")
            {//check validity
                string CustName = Payment_GET_Overview.CustName(DynAx, temp_CustAcc);
                if (CustName == "")
                {
                    txtCustAcc.Text = "";
                    Function_Method.MsgBox("Wrong Customer Account Number", this.Page, this);
                }
            }
            DynAx.Logoff();
            OverviewStatement(0, temp_CustAcc);
            customer_Section.Visible = false;
            Enquiries_section_Statement.Visible = true;
        }
    }
}