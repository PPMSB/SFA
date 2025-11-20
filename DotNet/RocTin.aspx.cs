using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Query.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;

namespace DotNet
{
    public partial class RocTin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dvEnquiries.Visible = false;
                btnOverview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
                TextBox_Account.Attributes.Add("autocomplete", "off");
                clear_variable();
                gvRocTin.PageIndex = 0;
                f_call_ax(0);

                string viewParam = Request.QueryString["View"]; // Use this value to adjust behavior

                if (viewParam == "Update") 
                {

                }
                else
                {
                    txtNewRoc.Enabled = false;
                    txtGetSSTNo.Enabled = false;
                    txtTIN.Enabled = false;
                    txtGetEmail.Enabled = false;
                    btnUpd.Visible = false;
                    btnCancel.Text = "Back";
                }
            }
        }

        protected void CheckAcc(object sender, EventArgs e)
        {
            clear_variable();
            gvRocTin.PageIndex = 0;
            f_call_ax(0);
        }

        private void f_call_ax(int PAGE_INDEX)
        {
            try
            {
                dvButtonUpd.Visible = false;
                using (Axapta DynAx = new Axapta())
                {
                    // Initialize AX connection
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                        new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    // Setup query
                    string fieldName = DropDownList1.SelectedItem.Text == "Account No." ? "AccountNum" : "Name";
                    int tableId = DynAx.GetTableId("CustTable");
                    string empID = "";
                    var axQuery = DynAx.CreateAxaptaObject("Query");
                    var axDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

                    // Add query ranges
                    if (dvEnquiries.Visible && !string.IsNullOrEmpty(empID) && !empID.Contains("-1"))
                    {
                        ((AxaptaObject)axDataSource.Call("addRange", 30033))
                            .Call("value", $"{empID},{empID}-1,{empID}-2");
                    }
                    else
                    {
                        ((AxaptaObject)axDataSource.Call("addRange", DynAx.GetFieldId(tableId, fieldName)))
                            .Call("value", $"*{TextBox_Account.Text.Trim()}*");
                    }
                    // Add standard customer group filters
                    new[] { "TDI", "TDE", "TDO" }
                        .ToList()
                        .ForEach(val => ((AxaptaObject)axDataSource.Call("addRange", 7))
                            .Call("value", val));
                    axDataSource.Call("addSortField", 2, 0); // Sort by Customer Name
                                                             // Process data
                    var axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
                    var dt = CreateVendorDataTable(12, new[] { "Account", "Customer Name", "Phone", "Mobile Phone",
                "ROC", "New ROC", "SST No", "TIN", "Email", "Address", "Salesman ID", "Updated" });

                    var pagingResult = Function_Method.paging_grid(PAGE_INDEX);
                    int startA = pagingResult[0];
                    int endA = pagingResult[1];
                    int countA = 0;
                    while ((bool)axQueryRun.Call("next") && countA <= endA)
                    {
                        using (var DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId))
                        {
                            countA++;
                            if (countA >= startA && countA <= endA)
                            {
                                var row = dt.NewRow();
                                string accountNum = DynRec.get_Field("AccountNum").ToString();
                                var partyTIN = GetLF_WebCustomerTIN(DynAx, accountNum);

                                row["No"] = countA;
                                row["Account"] = accountNum;
                                row["Customer Name"] = DynRec.get_Field("Name").ToString();
                                row["Phone"] = DynRec.get_Field("Phone").ToString();
                                row["Mobile Phone"] = DynRec.get_Field("CellularPhone").ToString();
                                row["ROC"] = GetOldROC(DynAx, DynRec.get_Field("PartyId").ToString()).Item1;
                                row["New ROC"] = partyTIN.Item2;
                                row["SST No"] = DynRec.get_Field("LF_SST_RegisterNo").ToString();
                                row["TIN"] = DynRec.get_Field("LF_TIN").ToString();
                                row["Email"] = DynRec.get_Field("Email").ToString();
                                row["Address"] = DynRec.get_Field("Address").ToString();
                                row["Salesman ID"] = DynRec.get_Field("EmplId").ToString();
                                row["Updated"] = (!string.IsNullOrEmpty(partyTIN.Item3?.ToString()) &&
                                                 !string.IsNullOrEmpty(partyTIN.Item5?.ToString())) ? "Yes" : "No";
                                dt.Rows.Add(row);
                            }
                        }
                    }
                    gvRocTin.VirtualItemCount = countA;
                    gvRocTin.DataSource = dt;
                    gvRocTin.DataBind();
                }
            }
            catch (Exception ex)
            {
                Function_Method.MsgBox($"ER_CM_00: {ex.Message}", this.Page, this);
            }

        }

        private DataTable CreateVendorDataTable(int data_count, string[] N)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("No", typeof(string)));
            for (int i = 0; i < data_count; i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }

            return dt;
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (dvEnquiries.Visible)
            {
                var split = DdlRoc.SelectedItem.ToString().Split('(', ')');
                string SalesmanID = split[1];
                f_call_ax(e.NewPageIndex);
            }
            else
            {
                f_call_ax(e.NewPageIndex);
            }
            gvRocTin.PageIndex = e.NewPageIndex;
            gvRocTin.DataBind();
        }

        protected void gvRocTin_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the GridView that triggered the event
                GridView gridView = (GridView)sender;
                // Get the selected row
                GridViewRow selectedRow = gridView.SelectedRow;
                // Extract the account number from the CommandArgument of the button or directly from the row
                LinkButton linkButton = (LinkButton)selectedRow.FindControl("LinkButton_Account");
                string accountNumber = linkButton != null ? linkButton.CommandArgument : string.Empty; // Assuming "Account" is in column 1

                // Initialize Axapta connection
                using (Axapta DynAx = new Axapta())
                {
                    DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName,
                    GLOBAL.ProxyPassword, GLOBAL.DomainName), GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
                    // Hide/Show Panels
                    dvEnquiries.Visible = false;
                    dvOverview.Visible = false;
                    dvUpdate.Visible = true;
                    dvButtonUpd.Visible = true;
                    // Set values from row to labels/textboxes
                    lblGetCustAcc.Text = accountNumber; // Assuming "Account" is in column 1
                    lblGetCustName.Text = selectedRow.Cells[2].Text;
                    lblGetPhone.Text = selectedRow.Cells[3].Text;
                    // Handle empty values (&nbsp;) safely
                    txtNewRoc.Text = string.IsNullOrWhiteSpace(selectedRow.Cells[6].Text.Trim().Replace("&nbsp;", "")) ? "" : selectedRow.Cells[6].Text;
                    txtGetSSTNo.Text = selectedRow.Cells[7].Text == "&nbsp;" ? "" : selectedRow.Cells[7].Text;
                    txtTIN.Text = selectedRow.Cells[8].Text == "&nbsp;" ? "" : selectedRow.Cells[8].Text;
                    txtGetEmail.Text = selectedRow.Cells[9].Text == "&nbsp;" ? "" : selectedRow.Cells[9].Text;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show a message)
                Function_Method.MsgBox($"Error: {ex.Message}", this.Page, this);
            }

        }

        protected void btnUpd_Click(object sender, EventArgs e)
        {
            Axapta DynAx = new Axapta();
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            try
            {
                if ((txtGetEmail.Text != "" && txtTIN.Text != "") || txtSalesmandRemark.Text != "")
                {
                    //string[] UpdateDuplicatedCust = getMultipleAcc(DynAx, lblGetCustAcc.Text);
                    //foreach (var item in UpdateDuplicatedCust)
                    //{
                    //if (item != null)
                    //{
                    //email is mandatory || if no email and tin, then can save for remark
                    if (!LblValidEmail.Visible || (txtGetEmail.Text == "" && txtTIN.Text == ""))
                    {
                        using (AxaptaRecord DynRec = DynAx.CreateAxaptaRecord("CustTable"))
                        {
                            DynAx.TTSBegin();
                            DynRec.ExecuteStmt(string.Format("select forupdate * from %1 where %1.{0} == '{1}'", "AccountNum", lblGetCustAcc.Text));
                            if (DynRec.Found)
                            {
                                if (lblGetCustAcc.Text.Contains("&amp;"))
                                {
                                    lblGetCustAcc.Text = lblGetCustAcc.Text.Replace("&amp;", "&");
                                }
                                DynRec.set_Field("AccountNum", lblGetCustAcc.Text);
                                DynRec.set_Field("Name", lblGetCustName.Text);
                                DynRec.set_Field("LF_CusteInveMail", txtGetEmail.Text);

                                DynRec.set_Field("LF_SST_RegisterNo", txtGetSSTNo.Text);
                                //DynRec.set_Field("RocNoNew", txtNewRoc.Text);
                                DynRec.set_Field("LF_TIN", txtTIN.Text.ToUpper().Replace(" ", string.Empty));

                                //if (txtSalesmandRemark.Text != "") { DynRec.set_Field("SalesmanRemark", txtSalesmandRemark.Text); }

                                DynRec.Call("update");
                            }
                            else
                            {
                                DynRec.set_Field("AccountNum", lblGetCustAcc.Text);
                                DynRec.set_Field("Name", lblGetCustName.Text);
                                DynRec.set_Field("LF_CusteInveMail", txtGetEmail.Text);

                                DynRec.set_Field("LF_SST_RegisterNo", txtGetSSTNo.Text);
                                //DynRec.set_Field("RocNoNew", txtNewRoc.Text);
                                DynRec.set_Field("LF_TIN", txtTIN.Text.ToUpper().Replace(" ", string.Empty));
                                //if (txtSalesmandRemark.Text != "") { DynRec.set_Field("SalesmanRemark", txtSalesmandRemark.Text); }

                                DynRec.Call("insert");
                            }

                            DynAx.TTSCommit();
                            DynAx.TTSAbort();
                        }
                    }
                    //}
                    //}
                    dvEnquiries.Visible = false;
                    dvUpdate.Visible = false;
                    dvOverview.Visible = true;
                    dvButtonUpd.Visible = false;
                    f_call_ax(0);
                }
                else
                {
                    if (txtGetEmail.Text == "")
                    {
                        LblValidEmail.Text = "Email required!";
                        LblValidEmail.Visible = true;
                    }

                    if (txtTIN.Text == "")
                    {
                        lblValidateTin.Visible = true;
                    }
                }
            }
            catch (Exception ER_Invoice_01)
            {
                DynAx.TTSAbort();
                Function_Method.MsgBox("ER_Invoice_01: " + ER_Invoice_01.ToString(), this.Page, this);
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("RocTin.aspx?View=All");
        }

        public static Tuple<string, string, string, string, string, string, string> GetLF_WebCustomerTIN(Axapta DynAx, string CustAcc)
        {
            string TableName = "LF_WebCustomerTIN";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "CustAccount");
            string sstno = ""; string rocnew = ""; string TinNo = "";
            string M100T1 = ""; string M100T2 = ""; string email = ""; string salesmanRemark = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//CustAccount
            qbr1.Call("value", CustAcc);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                salesmanRemark = DynRec.get_Field("SalesmanRemark").ToString();
                sstno = DynRec.get_Field("SSTno").ToString();
                rocnew = DynRec.get_Field("RocNoNew").ToString();
                TinNo = DynRec.get_Field("TINno").ToString();
                M100T1 = DynRec.get_Field("M100").ToString();
                M100T2 = DynRec.get_Field("M100T2").ToString();
                email = DynRec.get_Field("CusteInvoiceEmail").ToString();
            }
            return new Tuple<string, string, string, string, string, string, string>
                (sstno, rocnew, TinNo, M100T1, email, M100T2, salesmanRemark);
        }

        public static Tuple<string, string> GetOldROC(Axapta DynAx, string partyID)
        {
            string TableName = "DirOrganizationDetail";

            int tableId = DynAx.GetTableId(TableName);
            int fieldId = DynAx.GetFieldId(tableId, "PartyId");
            string oldROC = ""; string newROC = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", tableId);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", fieldId);//PartyId
            qbr1.Call("value", partyID);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", tableId);

                oldROC = DynRec.get_Field("OrgNumber").ToString();
                newROC = DynRec.get_Field("ROC_No_New").ToString();
            }
            return new Tuple<string, string>(oldROC, newROC);
        }

        public static string[] getMultipleAcc(Axapta DynAx, string CustAcc)
        {
            string[] accNum = new string[5]; int count = 0;
            int CustTable = 77;
            int LFI_AccountNum3_Group = DynAx.GetFieldId(CustTable, "LFI_AccountNum3_Group");
            AxaptaObject axQuery4 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource4 = (AxaptaObject)axQuery4.Call("addDataSource", CustTable);

            var qbr4 = (AxaptaObject)axQueryDataSource4.Call("addRange", LFI_AccountNum3_Group);
            qbr4.Call("value", CustAcc);
            AxaptaObject axQueryRun4 = DynAx.CreateAxaptaObject("QueryRun", axQuery4);
            while ((bool)axQueryRun4.Call("next"))
            {
                AxaptaRecord DynRec4 = (AxaptaRecord)axQueryRun4.Call("Get", CustTable);
                accNum[count] = DynRec4.get_Field("AccountNum").ToString();
                count++;
                DynRec4.Dispose();
            }

            if (accNum[0] == null)
            {
                accNum[0] = CustAcc;
            }
            return accNum;
        }

        protected void Button_enquiries_section_Click(object sender, EventArgs e)
        {
            dvOverview.Visible = true;
            dvSearchCust.Visible = false;
            dvUpdate.Visible = false;
            dvEnquiries.Visible = true;
            Button_enquiries_section.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            btnOverview.Attributes.Add("style", "background-color:transparent");
            lblTotalCust.Text = "";
            lblTotalUpdate.Text = "";

            Axapta DynAx = Function_Method.GlobalAxapta();
            GLOBAL.Company = GLOBAL.switch_Company;

            DdlRoc.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            items = SFA_GET_Enquiries_SalesmanTotal.getSalesman(DynAx);
            if (items.Count > 1)
            {
                DdlRoc.Items.AddRange(items.ToArray());
            }
            else
            {
                Function_Method.MsgBox("There is no salesman available.", this.Page, this);
            }
            DdlRoc.SelectedIndex = 0;
            gvRocTin.PageIndex = 0;
            clear_variable();
            //f_call_ax(0);

            DynAx.Logoff();
        }

        protected void btnOverview_Click(object sender, EventArgs e)
        {
            dvEnquiries.Visible = false;
            dvOverview.Visible = true;
            dvSearchCust.Visible = true;
            btnOverview.Attributes.Add("style", GLOBAL_VAR.GLOBAL.Button_Selected_color);
            Button_enquiries_section.Attributes.Add("style", "background-color:transparent");
            clear_variable();

            f_call_ax(0);
        }

        protected void DdlRoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected item is valid
            if (DdlRoc.SelectedIndex == 0)
            {
                // Optionally, you can clear the labels or set them to default values
                lblTotalCust.Text = "0";
                lblTotalOnly.Text = "0";
                lblTotalUpdate.Text = "0";
                gvRocTin.Visible = false;
                return; // Exit the method early
            }
            Axapta DynAx = new Axapta();
            DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);
            int count = 0; int countTIN = 0;
            int totalCust = 0;

            string SalesmanID = "";

            var split1 = DdlRoc.SelectedItem.Text.Split('(', ')');
            SalesmanID = split1[1];

            //var empID = getEmplTableEmpID(DynAx, SalesmanID);

            //for (int i = 0; i < empID.Count; i++)
            //{
            var GetCustAcc = getEmpID(DynAx, SalesmanID);

            for (int j = 0; j < GetCustAcc.Item1.Length; j++)
            {
                if (!string.IsNullOrEmpty(GetCustAcc.Item1[j]))
                {
                    var activeCustomer = getActiveCustomer(DynAx, GetCustAcc.Item1[j]);

                    if (activeCustomer)
                    {
                        totalCust++;
                        var getTotalUpdatedTINnEmail = get_EinvoiceEmailTin(DynAx, GetCustAcc.Item1[j]);

                        if (getTotalUpdatedTINnEmail.Item1.ToString() != "" && getTotalUpdatedTINnEmail.Item2.ToString() != "")
                        {
                            count++;
                        }

                        if (getTotalUpdatedTINnEmail.Item2.ToString() != "")
                        {
                            countTIN++;
                        }
                    }
                }
            }

            lblTotalCust.Text = totalCust.ToString();
            lblTotalOnly.Text = countTIN.ToString();
            lblTotalUpdate.Text = count.ToString();
            //}

            gvRocTin.PageIndex = 0;
            clear_variable();
            //f_call_ax(0, SalesmanID);// comment for temp
            //TIN_Report(0, SalesmanID);
            gvRocTin.Visible = false;
        }

        private void clear_variable()
        {
            gvRocTin.DataSource = null;
            gvRocTin.DataBind();
        }

        protected void txtGetEmail_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGetEmail.Text))
            {
                txtGetEmail.Focus();
                LblValidEmail.Text = "Email required!";
                LblValidEmail.Visible = true;
            }
            else
            {
                LblValidEmail.Visible = false;
            }

            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
    RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!regex.IsMatch(txtGetEmail.Text))
            {
                txtGetEmail.Focus();
                LblValidEmail.Text = "Email not valid!";
                LblValidEmail.Visible = true;
            }
            else
            {
                LblValidEmail.Visible = false;
            }
        }

        public static Tuple<string, string, string> GetEmpID(Axapta DynAx, string temp_EmplEmail)
        {
            string temp_EmpName = ""; string temp_EmpEmail = "";
            string emplID = "";

            string tablename = "EmplTable";
            string fieldname = "LF_EmpEmailID";
            int tableid = DynAx.GetTableId(tablename);
            int fieldid = DynAx.GetFieldId(tableid, fieldname);

            AxaptaObject axQuery2 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource2 = (AxaptaObject)axQuery2.Call("addDataSource", tableid);

            var qbr2 = (AxaptaObject)axQueryDataSource2.Call("addRange", fieldid);//LF_EmplEmailID
            qbr2.Call("value", temp_EmplEmail);

            var qbr1_2 = (AxaptaObject)axQueryDataSource2.Call("addRange", 30009);
            qbr1_2.Call("value", "0");// 'LF_StopEMail YesNo

            AxaptaObject axQueryRun2 = DynAx.CreateAxaptaObject("QueryRun", axQuery2);
            while ((bool)axQueryRun2.Call("next"))
            {
                AxaptaRecord DynRec2 = (AxaptaRecord)axQueryRun2.Call("Get", tableid);

                emplID = DynRec2.get_Field("EmplId").ToString();
                temp_EmpName = DynRec2.get_Field("Del_Name").ToString();

                DynRec2.Dispose();
            }
            return new Tuple<string, string, string>
                (temp_EmpName, temp_EmpEmail, emplID);
        }

        public static Tuple<List<ListItem>, string> getEmplTableEmpIDName(Axapta DynAx)
        {
            List<ListItem> List_Salesman = new List<ListItem>();
            string getSalesmanId = "";
            string salesmanName = "";
            int emplTable = 103;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", emplTable);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", emplTable);
                getSalesmanId = DynRec1.get_Field("EmplId").ToString();
                salesmanName = DynRec1.get_Field("DEL_Name").ToString();
                if (getSalesmanId.Contains("H") || getSalesmanId.StartsWith("0"))
                {
                    List_Salesman.Add(new ListItem(getSalesmanId));
                }
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource1.Dispose();
            return new Tuple<List<ListItem>, string>(List_Salesman, salesmanName);
        }

        public static Tuple<string, string> get_EinvoiceEmailTin(Axapta DynAx, string custAcc)
        {
            int CustTable = DynAx.GetTableId("CustTable");
            string LF_CusteInveMail = ""; string LF_TIN = "";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//CustAcc
            qbr1.Call("value", custAcc);

            var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);//CustGroup
            qbr4_2.Call("value", "TDI");

            var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_3.Call("value", "TDE");

            var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_4.Call("value", "TDO");

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);

                LF_CusteInveMail = DynRec.get_Field("LF_CusteInveMail").ToString();
                LF_TIN = DynRec.get_Field("LF_TIN").ToString();

            }
            return new Tuple<string, string>(LF_CusteInveMail, LF_TIN);
        }

        public static Tuple<string[], string, int> getEmpID(Axapta DynAx, string temp_EmpId)
        {
            string[] temp_Cust_Account = new string[1000];
            string temp_Emp_Id = "";
            int count = 0;

            int CustTable = 77;
            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery1.Call("addDataSource", CustTable);

            var qbr1 = (AxaptaObject)axQueryDataSource.Call("addRange", 30033);//EmplId
            qbr1.Call("value", temp_EmpId);

            if (!temp_EmpId.Contains("-1"))
            {
                var qbr2 = (AxaptaObject)axQueryDataSource.Call("addRange", 30033);//EmplId
                qbr2.Call("value", temp_EmpId + "," + temp_EmpId + "-1," + temp_EmpId + "-2");
            }

            var qbr4_2 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);//CustGroup
            qbr4_2.Call("value", "TDI");

            var qbr4_3 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_3.Call("value", "TDE");

            var qbr4_4 = (AxaptaObject)axQueryDataSource.Call("addRange", 7);
            qbr4_4.Call("value", "TDO");

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", CustTable);

                temp_Cust_Account[count] = DynRec1.get_Field("AccountNum").ToString();
                temp_Emp_Id = DynRec1.get_Field("EmplId").ToString();
                count++;
                DynRec1.Dispose();
            }
            axQuery1.Dispose();
            axQueryDataSource.Dispose();
            axQueryRun1.Dispose();
            return new Tuple<string[], string, int>(temp_Cust_Account, temp_Emp_Id, count);
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";

            string FileName = "";
            GridView gv = new GridView();
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            var split = DdlRoc.SelectedItem.ToString().Split('(', ')');
            string SalesmanID = split[1];

            FileName = SalesmanID + ".xls";
            gv = gvRocTin;

            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gv.GridLines = GridLines.Both;
            gv.HeaderStyle.Font.Bold = true;
            gv.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.Flush();
            Response.End();
        }

        protected void txtTIN_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTIN.Text))
            {
                lblValidateTin.Visible = true;
            }
            else
            {
                lblValidateTin.Visible = false;
            }
        }

        public static bool getActiveCustomer(Axapta DynAx, string temp_CustAcc)
        {
            int CustTrans = 78; string DueDate = "";
            bool isActive = false;
            var var_TransDate = DateTime.Now;
            DateTime todayDt = DateTime.Today;
            List<DateTime> listTransDt = new List<DateTime>();
            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTrans);

            var qbr_12 = (AxaptaObject)axQueryDataSource.Call("addRange", 4);
            qbr_12.Call("value", "*/INV");

            if (temp_CustAcc != "")
            {
                var qbr_0_1 = (AxaptaObject)axQueryDataSource.Call("addRange", 1);//ACCOUNTNUM
                qbr_0_1.Call("value", temp_CustAcc);
            }

            DateTime lastyear = new DateTime(2023, 1, 1);
            string var_Qdate = lastyear.ToString("dd/MM/yyyy") + ".." + todayDt.ToString("dd/MM/yyyy");

            var qbr = (AxaptaObject)axQueryDataSource.Call("addRange", 2);
            qbr.Call("value", var_Qdate);

            axQueryDataSource.Call("addSortField", 2, 1);//TransId, descending
            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);
            // Loop through the set of retrieved records.

            if ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTrans);

                string temp_TransDate = DynRec.get_Field("TransDate").ToString();
                string temp_DueDate = DynRec.get_Field("DueDate").ToString();
                string TransDate = "";
                if (temp_TransDate != "")
                {
                    string[] arr_temp_TransDate = temp_TransDate.Split(' ');//date + " " + time;
                    string Raw_TransDate = arr_temp_TransDate[0];
                    TransDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_TransDate, true);
                }
                if (temp_DueDate != "")
                {
                    string[] arr_temp_DueDate = temp_DueDate.Split(' ');//date + " " + time;
                    string Raw_DueDate = arr_temp_DueDate[0];
                    DueDate = Function_Method.get_correct_date(GLOBAL.system_checking, Raw_DueDate, true);
                }
                isActive = true;
                DynRec.Dispose();
            }


            // Sort the list by transaction date in descending order
            return isActive;
        }

    }
}