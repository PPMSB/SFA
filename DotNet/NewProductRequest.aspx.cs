using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using Microsoft.SqlServer.Server;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Input;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static ZXing.QrCode.Internal.Mode;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using GLOBAL_FUNCTION;
using System.Net;
using System.Net.Mail;

namespace DotNet
{
    public partial class NewProductRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                check_session();
                TimeOutRedirect();
                GenerateFormNumber(sender, e);
                PopulateSalesmanName();

                string salesmanName = Session["SalesmanName"] as string;
                string Status = Session["Status"] as string;
                string serialNum = Session["SerialNo"] as string;

                DateTime currentDateTime = DateTime.Now;
                requestDate.Text = currentDateTime.ToString("yyyy-MM-dd h:mm tt");

                string checkStatus = status.Text;
                if (status.Text == "")
                {
                    status.Text = "Draft";
                }

                //salesmanName.Text = salesmanName;
                //status.Text = status;

                if (serialNo.Text == "")
                {
                    serialNo.Text = serialNum;
                }

                LoadApprovalUser();

                //PopulateDropDownList(ddlCheck, new[] { "Ho Sau Ching" });
                //PopulateDropDownList(ddlQC, new[] { "Loh Boon Kian" });
                //PopulateDropDownList(ddlProduct, new[] { "Kenny Chuah Yew Siang" });
                //PopulateDropDownList(ddlPurchase, new[] { "Jerry Yong Wee Chong" });
                //PopulateDropDownList(ddlWarehouse, new[] { "Phua BE" });
                //PopulateDropDownList(ddlIT, new[] { "Keegan" });

                //LoginAccess();

                if (!string.IsNullOrEmpty(Request.QueryString["serialNo"]))
                {
                    string newCustIDFromQueryString = Request.QueryString["serialNo"];

                    LoadDataForDraft(newCustIDFromQueryString);
                    disableCheckBoxNbtn();
                    CheckUser();
                }

            }

        }

        private void BindDropDownList(DropDownList ddl, string data)
        {
            ddl.Items.Clear();

            if (!string.IsNullOrEmpty(data))
            {
                if (data.Contains(","))
                {
                    string[] values = data.Split(',');

                    foreach (string value in values)
                    {
                        ddl.Items.Add(new ListItem(value.Trim()));
                    }
                }
                else
                {
                    ddl.Items.Add(new ListItem(data.Trim()));
                }
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
            }
            catch
            {
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void disableCheckBoxNbtn()

        {
            string Status = status.Text;

            if (Status != "Draft")
            {
                productName.ReadOnly = true;
                productApp.ReadOnly = true;
                productPerform.ReadOnly = true;
                colorBottle.ReadOnly = true;
                oriBottle.ReadOnly = true;
                logoBottle.ReadOnly = true;
                colourCapLip.ReadOnly = true;
                oriCapLip.ReadOnly = true;
                logoCapLip.ReadOnly = true;
                colorInduction.ReadOnly = true;
                oriInduction.ReadOnly = true;
                logoInduction.ReadOnly = true;
                colorDrumPail.ReadOnly = true;
                oriDrumPail.ReadOnly = true;
                logoDrumPail.ReadOnly = true;
                colorLabel.ReadOnly = true;
                oriLabel.ReadOnly = true;
                logoLabel.ReadOnly = true;
                colorSticker.ReadOnly = true;
                oriSticker.ReadOnly = true;
                logoSticker.ReadOnly = true;
                colorOpp.ReadOnly = true;
                oriOpp.ReadOnly = true;
                logoOpp.ReadOnly = true;
                colorOthers.ReadOnly = true;
                oriOthers.ReadOnly = true;
                logoOthers.ReadOnly = true;
                coolantColour.ReadOnly = true;
                apiDate.ReadOnly = true;
                apiOthers.ReadOnly = true;
                EDDate.ReadOnly = true;
                EDOthers.ReadOnly = true;
                APPDate.ReadOnly = true;
                APPOthers.ReadOnly = true;
                packSize.ReadOnly = true;
                litreX.ReadOnly = true;
                launchDate.ReadOnly = true;
                qty1Product.ReadOnly = true;
                stockCode.ReadOnly = true;
                remarks.ReadOnly = true;

                PPMYes.Enabled = false;
                btnDraft.Visible = false;
                btnRequest.Visible = false;
            }
        }

        public bool IsUserInWaitList(string waitList, string userName)
        {
            // Split the waitList by ':' and ',' and remove empty entries
            string[] parts = waitList.Split(new[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Check if there are parts after the ':' indicating user names
            if (parts.Length > 1)
            {
                // Loop through the names part (skipping the first part which is the role)
                for (int i = 1; i < parts.Length; i++)
                {
                    if (parts[i].Trim().Equals(userName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // check status with username
        private void CheckUser()
        {
            string Status = status.Text;
            string UserName = GLOBAL.logined_user_name;

            string[] parts = Status.Split(new[] { ' ' }, 2);
            string name = parts.Length > 1 ? parts[1] : string.Empty;

            int index = UserName.IndexOf(" (");
            string fullName = (index >= 0) ? UserName.Substring(0, index) : UserName;

            string waitCheck = "Waiting Checker: " + checkerText.Text;
            string waitQC = "Waiting QC:" + qcText.Text;
            string waitProduct = "Waiting Production: " + productText.Text;
            string waitPurchase = "Waiting Purchase: " + purchaseText.Text;
            string waitWare = "Waiting Warehouse: " + wareText.Text;
            string waitAnP = "Waiting Marketing: " + marketText.Text;
            string waitIT = "Waiting IT: " + itText.Text;
            string waitApprove = "Waiting Approve:" + approveText.Text; //ddlApprove.SelectedValue;
            string waitAdmin = "Admin:" + adminUser.Text;

            // Approve
            if (IsUserInWaitList(waitCheck, fullName) && Status.Contains("Checker"))
            {
                tblReview.Visible = true;
                ddlCheck.Visible = true;
                //addCheckUser.Visible = true;
                //btnCheck.Visible = true;
                Check.Visible = true;
                btnApprove.Visible = true;
                tblReview.Visible = true;
                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (IsUserInWaitList(waitQC, fullName) && Status.Contains("QC"))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                ddlQC.Visible = true;
                //addQCUser.Visible = true;
                //btnQC.Visible = true;
                reviewQC.Visible = true;

                if (checkUser.Text.Trim() != "")
                {
                    btnAuthor.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (IsUserInWaitList(waitProduct, fullName) && Status.Contains("Production"))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                ddlProduct.Visible = true;
                //addProductUser.Visible = true;
                //btnProduct.Visible = true;

                if (qcUser.Text.Trim() != "")
                {
                    btnAuthor.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (IsUserInWaitList(waitPurchase, fullName) && Status.Contains("Purchase"))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                productUser.Visible = true;
                reviewPurchase.Visible = true;
                ddlPurchase.Visible = true;
                //addPurchaseUser.Visible = true;
                //btnPurchase.Visible = true;

                if (productUser.Text.Trim() != "")
                {
                    btnAuthor.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (IsUserInWaitList(waitWare, fullName) && Status.Contains("Warehouse"))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                productUser.Visible = true;
                reviewPurchase.Visible = true;
                purchaseUser.Visible = true;
                rowWare.Visible = true;
                ddlWarehouse.Visible = true;
                //addWarehouseUser.Visible = true;
                //btnWarehouse.Visible = true;

                if (purchaseUser.Text.Trim() != "")
                {
                    btnAcknow.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (IsUserInWaitList(waitAnP, fullName) && Status.Contains("Marketing"))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                productUser.Visible = true;
                reviewPurchase.Visible = true;
                purchaseUser.Visible = true;
                rowWare.Visible = true;
                wareUser.Visible = true;
                rowAP.Visible = true;
                ddlAnP.Visible = true;
                //addAnPUser.Visible = true;
                //btnAnP.Visible = true;

                if (wareUser.Text.Trim() != "")
                {
                    btnAcknow.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (IsUserInWaitList(waitIT, fullName) && Status.Contains("IT"))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                productUser.Visible = true;
                reviewPurchase.Visible = true;
                purchaseUser.Visible = true;
                rowWare.Visible = true;
                wareUser.Visible = true;
                rowAP.Visible = true;
                anpUser.Visible = true;
                rowIT.Visible = true;
                ddlIT.Visible = true;
                btnPrint.Visible = true;
                rowApprove.Visible = true;
                approveDate.Visible = false;
                //addITUser.Visible = true;
                //btnIT.Visible = true;

                if (anpUser.Text.Trim() != "")
                {
                    btnAcknow.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if ((IsUserInWaitList(waitApprove, fullName) && Status.Contains("Approve:")) || ((IsUserInWaitList(waitAdmin, fullName) || UserName == "Admin DotNet") && Status.Contains("Approve:")))
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                productUser.Visible = true;
                reviewPurchase.Visible = true;
                purchaseUser.Visible = true;
                rowWare.Visible = true;
                wareUser.Visible = true;
                rowAP.Visible = true;
                anpUser.Visible = true;
                rowIT.Visible = true;
                itUser.Visible = true;
                rowApprove.Visible = true;
                ddlApprove.Visible = true;
                btnPrint.Visible = true;

                if (itUser.Text.Trim() != "")
                {
                    btnApprove.Visible = true;
                    btnReject.Visible = true;
                }

                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }
            else if (Status == "Approve" || Status == "Reject")
            {
                tblReview.Visible = true;
                Check.Visible = true;
                checkUser.Visible = true;
                reviewTag.Visible = true;
                reviewQC.Visible = true;
                qcUser.Visible = true;
                reviewProduct.Visible = true;
                productUser.Visible = true;
                reviewPurchase.Visible = true;
                purchaseUser.Visible = true;
                rowWare.Visible = true;
                wareUser.Visible = true;
                rowAP.Visible = true;
                anpUser.Visible = true;
                rowIT.Visible = true;
                itUser.Visible = true;
                rowApprove.Visible = true;
                approveUser.Visible = true;
                btnPrint.Visible = false;
                btnApprove.Visible = false;
                btnReject.Visible = false;
                btnRequest.Visible = false;
                btnDraft.Visible = false;
            }

            //bool chckUsr = true;

            //if (IsUserInWaitList(name, fullName))
            //{
            //    //btnAcknow.Visible = false;
            //    //btnApprove.Visible = false;
            //    //btnReject.Visible=false;
            //    //btnAuthor.Visible = false;

            //    // Approve
            //    if (chckUsr = IsUserInWaitList(waitCheck, fullName))
            //    {
            //        checkUser.Visible = true;
            //        ddlCheck.Visible = false;
            //        addCheckUser.Visible = false;
            //        btnCheck.Visible = false;
            //        btnApprove.Visible = false;
            //    }
            //    else if (chckUsr = IsUserInWaitList(waitQC, fullName))
            //    {
            //        qcUser.Visible = true;
            //        ddlQC.Visible = false;
            //        addQCUser.Visible = false;
            //        btnQC.Visible = false;
            //        btnAuthor.Visible = false;
            //    }
            //    else if (IsUserInWaitList(waitProduct, fullName))
            //    {
            //        productUser.Visible = true;
            //        ddlProduct.Visible = false;
            //        addProductUser.Visible = false;
            //        btnProduct.Visible = false;
            //        btnAuthor.Visible = false;
            //    }
            //    else if (IsUserInWaitList(waitPurchase, fullName))
            //    {
            //        purchaseUser.Visible = true;
            //        ddlPurchase.Visible = false;
            //        addPurchaseUser.Visible = false;
            //        btnPurchase.Visible = false;
            //        btnAuthor.Visible = false;
            //    }
            //    else if (IsUserInWaitList(waitWare, fullName))
            //    {
            //        wareUser.Visible = true;
            //        ddlWarehouse.Visible = false;
            //        addWarehouseUser.Visible = false;
            //        btnWarehouse.Visible = false;
            //        btnAcknow.Visible = false;
            //    }
            //    else if (IsUserInWaitList(waitAnP, fullName))
            //    {
            //        anpUser.Visible = true;
            //        ddlAnP.Visible = false;
            //        addAnPUser.Visible = false;
            //        btnAnP.Visible = false;
            //        btnAcknow.Visible = false;
            //    }
            //    else if (IsUserInWaitList(waitIT, fullName))
            //    {
            //        itUser.Visible = true;
            //        ddlIT.Visible = false;
            //        addITUser.Visible = false;
            //        btnIT.Visible = false;
            //        btnAcknow.Visible = false;
            //    }
            //    else if (fullName.Equals(waitApprove))
            //    {
            //        approveUser.Visible = true;
            //        ddlApprove.Visible = false;
            //        btnApprove.Visible = false;
            //        btnReject.Visible = false;
            //    }

            //}

        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            // Email to checker
            string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

            string checkUser = ddlCheck.SelectedValue;

            using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                {
                    cmd.Parameters.AddWithValue("@username", "%" + checkUser + "%");

                    try
                    {
                        emailConnection.Open();
                        object result = cmd.ExecuteScalar();

                        string checkUsr = result.ToString();

                        string MailSubject = "New Product Request - Waiting for You to Review";
                        string MailTo = "urTestingEmail@gmail.com";// checkUsr + "@posim.com.my";

                        //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                        string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                            "Form Created Time: " + requestDate.Text + "\n" +
                            "Request By: " + salesmanName.Text + "\n" +
                            "\nWaiting for you to review and approve.";
                        Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                    }
                    catch { }
                }

            }

        }

        private void PopulateSalesmanName()
        {
            Axapta DynAx = new Axapta();

#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call your method to get the salesman name based on user id
                string SalesmanName = GLOBAL.logined_user_name;

                // Display the salesman name in the TextBox
                salesmanName.Text = SalesmanName;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log or display an error message
            }
            finally
            {
                // Close the Axapta connection
                DynAx.Logoff();
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }

        public string GetAxSalesmanNameByUserId(Axapta DynAx, string userId)
        {
            // Assuming your salesmen table in Axapta is named "EmplTable"
            int EmplTable = 103; // Replace with the actual table number

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            if ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);

                // Assuming DEL_UserId is a field in your salesmen table
                string temp_SalesmanName = DynRec1.get_Field("DEL_Name").ToString();

                // Log or output the values for debugging
                System.Diagnostics.Debug.WriteLine($"User ID: {userId}, Salesman Name: {temp_SalesmanName}");

                // Compare the user id with DEL_UserId and return the corresponding name
                if (userId == GLOBAL.user_id)
                {
                    // Dispose of the current record
                    DynRec1.Dispose();
                    return temp_SalesmanName;
                }

                // Dispose of the current record
                DynRec1.Dispose();
            }

            return "-- Not Found --"; // Return a default value if user id is not found
        }

        protected void GenerateFormNumber(object sender, EventArgs e)
        {
            // Generate the form number in the format "yymm-four any digit"
            string FormNumber = $"{DateTime.Now.ToString("yyMM")}-{DateTime.Now.ToString("ddHHmm")}";   //GenerateRandomDigits(4)}";

            // Set the generated form number to the Form_Number TextBox
            serialNo.Text = FormNumber;
        }

        private string GenerateRandomDigits(int digitCount)
        {
            Random random = new Random();
            StringBuilder digits = new StringBuilder();

            for (int i = 0; i < digitCount; i++)
            {
                digits.Append(random.Next(10));
            }

            return digits.ToString();
        }

        private void PopulateDropDownList(DropDownList ddl, string[] items)
        {
            ddl.DataSource = items;
            ddl.DataBind();
        }

        private void LoadDataForDraft(string serialNumber)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                // new product info
                string infoQuery = $"SELECT * FROM newproductrequest_info WHERE serialNo = @serialNo";
                MySqlCommand infoCommand = new MySqlCommand(infoQuery, connection);
                infoCommand.Parameters.AddWithValue("@serialNo", serialNumber);

                connection.Open();
                using (MySqlDataReader infoReader = infoCommand.ExecuteReader())
                {
                    if (infoReader.Read())
                    {
                        // Populate controls on your form with data from newproductrequest_info
                        serialNo.Text = infoReader["serialNo"].ToString();
                        salesmanName.Text = infoReader["requestBy"].ToString();
                        requestDate.Text = infoReader["date"].ToString();
                        status.Text = infoReader["status"].ToString();
                        productName.Text = infoReader["name"].ToString();
                        productApp.Text = infoReader["application"].ToString();
                        productPerform.Text = infoReader["performance"].ToString();
                    }
                }

                // new product detail
                string detailQuery = $"SELECT * FROM newproductrequest_detail WHERE serialNo = @serialNo";
                MySqlCommand detailCommand = new MySqlCommand(detailQuery, connection);
                detailCommand.Parameters.AddWithValue("@serialNo", serialNumber);

                using (MySqlDataReader detailReader = detailCommand.ExecuteReader())
                {
                    if (detailReader.Read())
                    {
                        // Populate controls on your form with data from newproductrequest_info
                        colorBottle.Text = detailReader["bottleColour"].ToString();
                        oriBottle.Text = detailReader["bottleMould"].ToString();
                        logoBottle.Text = detailReader["bottleLogo"].ToString();

                        string caplipValue = detailReader["CapLip"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (caplipValue == "Cap")
                            {
                                radCap.Checked = true;
                                radLid.Checked = false;
                            }
                            else if (caplipValue == "Lid")
                            {
                                radCap.Checked = false;
                                radLid.Checked = true;
                            }
                        }
                        else
                        {
                            radCap.Visible = false;
                            radLid.Visible = false;
                            caplidText.Visible = true;
                            caplidText.Text = caplipValue;
                        }

                        colourCapLip.Text = detailReader["CapLipColour"].ToString();
                        oriCapLip.Text = detailReader["CapLipMould"].ToString();
                        logoCapLip.Text = detailReader["CapLipLogo"].ToString();

                        string InducTabValue = detailReader["InducTab"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (InducTabValue == "Induction")
                            {
                                radInduc.Checked = true;
                                radTab.Checked = false;
                            }
                            else if (InducTabValue == "Tab Seal")
                            {
                                radInduc.Checked = false;
                                radTab.Checked = true;
                            }
                        }
                        else
                        {
                            radInduc.Visible = false;
                            radTab.Visible = false;
                            inductabText.Visible = true;
                            inductabText.Text = InducTabValue;
                        }

                        colorInduction.Text = detailReader["InducTabColour"].ToString();
                        oriInduction.Text = detailReader["InducTabMould"].ToString();
                        logoInduction.Text = detailReader["InducTabLogo"].ToString();

                        string DrumPailValue = detailReader["DrumPail"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (DrumPailValue == "Drum")
                            {
                                radDrum.Checked = true;
                                radPail.Checked = false;
                            }
                            else if (DrumPailValue == "Pail")
                            {
                                radDrum.Checked = false;
                                radPail.Checked = true;
                            }
                        }
                        else
                        {
                            radDrum.Visible = false;
                            radPail.Visible = false;
                            drumpailText.Visible = true;
                            drumpailText.Text = DrumPailValue;
                        }

                        colorDrumPail.Text = detailReader["DrumPailColour"].ToString();
                        oriDrumPail.Text = detailReader["DrumPailMould"].ToString();
                        logoDrumPail.Text = detailReader["DrumPailLogo"].ToString();

                        colorLabel.Text = detailReader["labelColour"].ToString();
                        oriLabel.Text = detailReader["labelMould"].ToString();
                        logoLabel.Text = detailReader["labelLogo"].ToString();
                        colorSticker.Text = detailReader["stickerColour"].ToString();
                        oriSticker.Text = detailReader["stickerMould"].ToString();
                        logoSticker.Text = detailReader["stickerLogo"].ToString();
                        colorOpp.Text = detailReader["oppColour"].ToString();
                        oriOpp.Text = detailReader["oppMould"].ToString();
                        logoOpp.Text = detailReader["oppLogo"].ToString();
                        colorOthers.Text = detailReader["otherColour"].ToString();
                        oriOthers.Text = detailReader["otherMould"].ToString();
                        logoOthers.Text = detailReader["otherLogo"].ToString();
                        coolantColour.Text = detailReader["coolColour"].ToString();

                        string coolValue = detailReader["coolFragrance"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (coolValue == "Yes")
                            {
                                radCoolYes.Checked = true;
                                radCoolNo.Checked = false;
                            }
                            else if (coolValue == "No")
                            {
                                radCoolYes.Checked = false;
                                radCoolNo.Checked = true;
                            }
                        }
                        else
                        {
                            radCoolYes.Visible = false;
                            radCoolNo.Visible = false;
                            coolText.Visible = true;
                            coolText.Text = coolValue;
                        }


                        string apiValue = detailReader["apiRegister"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (apiValue == "Yes")
                            {
                                apiYes.Checked = true;
                                apiNo.Checked = false;
                            }
                            else if (apiValue == "No")
                            {
                                apiYes.Checked = false;
                                apiNo.Checked = true;
                            }
                        }
                        else
                        {
                            apiYes.Visible = false;
                            apiNo.Visible = false;
                            apiText.Visible = true;
                            apiText.Text = apiValue;
                        }

                        apiDate.Text = detailReader["apiDate"].ToString();
                        apiOthers.Text = detailReader["apiOther"].ToString();

                        string EDValue = detailReader["formEDRegister"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (EDValue == "Yes")
                            {
                                EDYes.Checked = true;
                                EDNo.Checked = false;
                            }
                            else if (EDValue == "No")
                            {
                                EDYes.Checked = false;
                                EDNo.Checked = true;
                            }
                        }
                        else
                        {
                            EDYes.Visible = false;
                            EDNo.Visible = false;
                            edText.Visible = true;
                            edText.Text = EDValue;
                        }

                        EDDate.Text = detailReader["formEDDate"].ToString();
                        EDOthers.Text = detailReader["formEDOther"].ToString();

                        string webValue = detailReader["webRegister"].ToString();
                        if (status.Text == "Draft")
                        {
                            if (webValue == "Yes")
                            {
                                APPYes.Checked = true;
                                APPNo.Checked = false;
                            }
                            else if (webValue == "No")
                            {
                                APPYes.Checked = false;
                                APPNo.Checked = true;
                            }
                        }
                        else
                        {
                            APPYes.Visible = false;
                            APPNo.Visible = false;
                            appText.Visible = true;
                            appText.Text = webValue;
                        }

                        APPDate.Text = detailReader["webDate"].ToString();
                        APPOthers.Text = detailReader["webOther"].ToString();
                        packSize.Text = detailReader["packSize"].ToString();
                        litreX.Text = detailReader["packLitre"].ToString();
                        launchDate.Text = detailReader["productLaunchDate"].ToString();
                        qty1Product.Text = detailReader["qty1Production"].ToString();
                        stockCode.Text = detailReader["stockCode"].ToString();

                        bool isPPMChecked = Convert.ToInt32(detailReader["PPMUser"]) == 1;
                        PPMYes.Checked = isPPMChecked;
                        remarks.Text = detailReader["remark"].ToString();
                    }
                }



                // new product review
                string reviewQuery = $"SELECT * FROM newproductrequest_review WHERE serialNo = @serialNo";
                MySqlCommand reviewCommand = new MySqlCommand(reviewQuery, connection);
                reviewCommand.Parameters.AddWithValue("@serialNo", serialNumber);

                using (MySqlDataReader reviewReader = reviewCommand.ExecuteReader())
                {
                    if (reviewReader.Read())
                    {
                        // Populate controls on your form with data from newproductrequest_reviewReader
                        checkUser.Text = reviewReader["checkedBy"].ToString();
                        checkSign.Text = reviewReader["checkAcknow"].ToString();
                        checkDate.Text = reviewReader["checkDate"].ToString();
                        checkRemark.Text = reviewReader["checkRemark"].ToString();
                        qcUser.Text = reviewReader["QCName"].ToString();
                        qcSign.Text = reviewReader["QCAcknow"].ToString();
                        qcDate.Text = reviewReader["QCDate"].ToString();
                        qcRemark.Text = reviewReader["QCRemark"].ToString();
                        productUser.Text = reviewReader["ProductName"].ToString();
                        productSign.Text = reviewReader["ProductAcknow"].ToString();
                        productDate.Text = reviewReader["ProductDate"].ToString();
                        productRemark.Text = reviewReader["ProductRemark"].ToString();
                        purchaseUser.Text = reviewReader["PurchaseName"].ToString();
                        purchaseSign.Text = reviewReader["PurchaseAcknow"].ToString();
                        purchaseDate.Text = reviewReader["PurchaseDate"].ToString();
                        purchaseRemark.Text = reviewReader["PurchaseRemark"].ToString();
                        wareUser.Text = reviewReader["WarehouseName"].ToString();
                        wareSign.Text = reviewReader["WarehouseAcknow"].ToString();
                        wareDate.Text = reviewReader["WarehouseDate"].ToString();
                        wareRemark.Text = reviewReader["WarehouseRemark"].ToString();
                        anpUser.Text = reviewReader["AnPName"].ToString();
                        anpSign.Text = reviewReader["AnPAcknow"].ToString();
                        anpDate.Text = reviewReader["AnPDate"].ToString();
                        anpRemark.Text = reviewReader["AnPRemark"].ToString();
                        itUser.Text = reviewReader["ITName"].ToString();
                        itSign.Text = reviewReader["ITAcknow"].ToString();
                        itDate.Text = reviewReader["ITDate"].ToString();
                        itRemark.Text = reviewReader["ITRemark"].ToString();
                        approveUser.Text = reviewReader["ApproveName"].ToString();
                        approveSign.Text = reviewReader["ApproveAcknow"].ToString();
                        approveDate.Text = reviewReader["ApproveDate"].ToString();
                        approveRemark.Text = reviewReader["ApproveRemark"].ToString();

                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadApprovalUser()
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                connection.Open();
                // new product approval user
                string userQuery = $"SELECT * FROM newproductrequest_approval";
                MySqlCommand userCommand = new MySqlCommand(userQuery, connection);

                using (MySqlDataReader userReader = userCommand.ExecuteReader())
                {
                    if (userReader.Read())
                    {
                        // Populate controls on your form with data from newproductrequest_approval
                        checkerText.Text = userReader["CheckUser"].ToString();
                        qcText.Text = userReader["QC"].ToString();
                        productText.Text = userReader["Production"].ToString();
                        purchaseText.Text = userReader["Purchase"].ToString();
                        wareText.Text = userReader["Warehouse"].ToString();
                        marketText.Text = userReader["Marketing"].ToString();
                        itText.Text = userReader["IT"].ToString();
                        approveText.Text = userReader["Approve"].ToString();
                        adminUser.Text = userReader["Admin"].ToString();

                        BindDropDownList(ddlCheck, checkerText.Text);
                        BindDropDownList(ddlQC, qcText.Text);
                        BindDropDownList(ddlProduct, productText.Text);
                        BindDropDownList(ddlPurchase, purchaseText.Text);
                        BindDropDownList(ddlWarehouse, wareText.Text);
                        BindDropDownList(ddlAnP, marketText.Text);
                        BindDropDownList(ddlIT, itText.Text);
                        BindDropDownList(ddlApprove, approveText.Text);
                    }
                }
            }
            catch (Exception e) { }
        }

        protected void btnAddCheck_Click(object sender, EventArgs e)
        {
            AddNewName(addCheckUser, ddlCheck);
        }

        protected void btnAddQC_Click(object sender, EventArgs e)
        {
            AddNewName(addQCUser, ddlQC);
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)

        {
            AddNewName(addProductUser, ddlProduct);
        }

        protected void btnAddPurchase_Click(object sender, EventArgs e)
        {
            AddNewName(addPurchaseUser, ddlPurchase);
        }

        protected void btnAddWarehouse_Click(object sender, EventArgs e)
        {
            AddNewName(addWarehouseUser, ddlWarehouse);

        }

        protected void btnAddAnP_Click(object sender, EventArgs e)
        {
            AddNewName(addAnPUser, ddlAnP);

        }

        protected void btnAddIT_Click(object sender, EventArgs e)
        {
            AddNewName(addITUser, ddlIT);

        }

        private void AddNewName(System.Web.UI.WebControls.TextBox addUser, DropDownList ddl)
        {
            if (!string.IsNullOrWhiteSpace(addUser.Text))
            {
                ddl.Items.Add(new ListItem(addUser.Text));
                addUser.Text = string.Empty;
            }
        }

        protected void btnDraft_Click(object sender, EventArgs e)
        {
            string emptyBlank = "";
            string serialNum = serialNo.Text.Trim();

            if (!string.IsNullOrEmpty(serialNum))
            {
                string findSerial = "SELECT * FROM newproductrequest_info WHERE serialNo = @serialNo";

                using (MySqlConnection connectionNum = new MySqlConnection(GLOBAL.connStr))
                {
                    MySqlCommand commandNo = new MySqlCommand(findSerial, connectionNum);
                    commandNo.Parameters.AddWithValue("@serialNo", serialNum);

                    try
                    {
                        connectionNum.Open();
                        MySqlDataReader ReaderNum = commandNo.ExecuteReader();

                        if (ReaderNum.HasRows)
                        {
                            // exist

                            string query =
                            @"UPDATE newproductrequest_info SET " +
                            "name = @name, application = @application, performance = @performance WHERE serialNo = @serialNo;" +

                            "UPDATE newproductrequest_detail SET " +
                            "bottleColour = @bottleColour, bottleMould = @bottleMould, bottleLogo = @bottleLogo, CapLip = @CapLip, CapLipColour = @CapLipColour, CapLipMould = @CapLipMould, CapLipLogo = @CapLipLogo, InducTab = @InducTab, InducTabColour = @InducTabColour, InducTabMould = @InducTabMould, InducTabLogo = @InducTabLogo, DrumPail = @DrumPail, DrumPailColour = @DrumPailColour, DrumPailMould = @DrumPailMould, DrumPailLogo = @DrumPailLogo, labelColour = @labelColour, labelMould = @labelMould, labelLogo = @labelLogo,  stickerColour = @stickerColour, stickerMould = @stickerMould, stickerLogo = @stickerLogo, oppColour = @oppColour, oppMould = @oppMould, oppLogo = @oppLogo, otherColour = @otherColour, otherMould = @otherMould, otherLogo = @otherLogo, coolColour = @coolColour, coolFragrance = @coolFragrance, apiRegister = @apiRegister, apiDate = @apiDate, apiOther = @apiOther, formEDRegister = @formEDRegister, formEDDate = @formEDDate, formEDOther = @formEDOther, webRegister = @webRegister, webDate = @webDate, webOther = @webOther, packSize = @packSize, packLitre = @packLitre, productLaunchDate = @productLaunchDate, qty1Production = @qty1Production, stockCode = @stockCode, PPMUser = @PPMUser, remark = @remark " +
                            "WHERE serialNo = @serialNo;";

                            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                            {
                                using (MySqlCommand command = new MySqlCommand(query, connection))
                                {
                                    // use for three table
                                    command.Parameters.AddWithValue("@serialNo", serialNo.Text);
                                    command.Parameters.AddWithValue("@requestBy", salesmanName.Text);


                                    command.Parameters.AddWithValue("@status", "Draft");
                                    command.Parameters.AddWithValue("@name", productName.Text);
                                    command.Parameters.AddWithValue("@application", productApp.Text);
                                    command.Parameters.AddWithValue("@performance", productPerform.Text);


                                    command.Parameters.AddWithValue("@bottleColour", colorBottle.Text);
                                    command.Parameters.AddWithValue("@bottleMould", oriBottle.Text);
                                    command.Parameters.AddWithValue("@bottleLogo", logoBottle.Text);

                                    if (!radCap.Checked && !radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", emptyBlank);
                                    }
                                    else if (radCap.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Cap");
                                    }
                                    else if (radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Lid");
                                    }

                                    command.Parameters.AddWithValue("@CapLipColour", colourCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipMould", oriCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipLogo", logoCapLip.Text);

                                    if (!radInduc.Checked && !radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", emptyBlank);
                                    }
                                    else if (radInduc.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Induction");
                                    }
                                    else if (radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Tab Seal");
                                    }

                                    command.Parameters.AddWithValue("@InducTabColour", colorInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabMould", oriInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabLogo", logoInduction.Text);

                                    if (!radDrum.Checked && !radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", emptyBlank);
                                    }
                                    else if (radDrum.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Drum");
                                    }
                                    else if (radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Pail");
                                    }

                                    command.Parameters.AddWithValue("@DrumPailColour", colorDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailMould", oriDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailLogo", logoDrumPail.Text);
                                    command.Parameters.AddWithValue("@labelColour", colorLabel.Text);
                                    command.Parameters.AddWithValue("@labelMould", oriLabel.Text);
                                    command.Parameters.AddWithValue("@labelLogo", logoLabel.Text);
                                    command.Parameters.AddWithValue("@stickerColour", colorSticker.Text);
                                    command.Parameters.AddWithValue("@stickerMould", oriSticker.Text);
                                    command.Parameters.AddWithValue("@stickerLogo", logoSticker.Text);
                                    command.Parameters.AddWithValue("@oppColour", colorOpp.Text);
                                    command.Parameters.AddWithValue("@oppMould", oriOpp.Text);
                                    command.Parameters.AddWithValue("@oppLogo", logoOpp.Text);
                                    command.Parameters.AddWithValue("@otherColour", colorOthers.Text);
                                    command.Parameters.AddWithValue("@otherMould", oriOthers.Text);
                                    command.Parameters.AddWithValue("@otherLogo", logoOthers.Text);
                                    command.Parameters.AddWithValue("@coolColour", coolantColour.Text);

                                    if (!radCoolYes.Checked && !radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", emptyBlank);
                                    }
                                    else if (radCoolYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "Yes");
                                    }
                                    else if (radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "No");
                                    }


                                    if (!apiYes.Checked && !apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", emptyBlank);
                                    }
                                    else if (apiYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "Yes");
                                    }
                                    else if (apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@apiDate", apiDate.Text);
                                    command.Parameters.AddWithValue("@apiOther", apiOthers.Text);

                                    if (!EDYes.Checked && !EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", emptyBlank);
                                    }
                                    else if (EDYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "Yes");
                                    }
                                    else if (EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@formEDDate", EDDate.Text);
                                    command.Parameters.AddWithValue("@formEDOther", EDOthers.Text);

                                    if (!APPYes.Checked && !APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", emptyBlank);
                                    }
                                    else if (APPYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "Yes");
                                    }
                                    else if (APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@webDate", APPDate.Text);
                                    command.Parameters.AddWithValue("@webOther", APPOthers.Text);
                                    command.Parameters.AddWithValue("@packSize", packSize.Text);
                                    command.Parameters.AddWithValue("@packLitre", litreX.Text);
                                    command.Parameters.AddWithValue("@productLaunchDate", launchDate.Text);
                                    command.Parameters.AddWithValue("@qty1Production", qty1Product.Text);
                                    command.Parameters.AddWithValue("@stockCode", stockCode.Text);
                                    command.Parameters.AddWithValue("@PPMUser", PPMYes.Checked ? 1 : 0);
                                    command.Parameters.AddWithValue("@remark", remarks.Text);

                                    connection.Open();
                                    command.ExecuteNonQuery();

                                    Response.Redirect("NewProductHomePage.aspx", false);
                                }

                            }

                        }
                        else
                        {
                            // not exist   

                            DateTime currentDateTime = DateTime.Now;
                            TimeSpan currentTime = currentDateTime.TimeOfDay;

                            string query = @"
                            INSERT INTO `newproductrequest_info`
                            (`serialNo`, `requestBy`, `date`, `time`, `status`, `name`, `application`, `performance`) VALUES (@serialNo, @requestBy, @date, @time, @status, @name, @application, @performance);

                            INSERT INTO `newproductrequest_detail`
                            (`serialNo`, `requestBy`, `bottleColour`, `bottleMould`, `bottleLogo`, `CapLip`, `CapLipColour`, `CapLipMould`, `CapLipLogo`, `InducTab`, `InducTabColour`, `InducTabMould`, `InducTabLogo`, `DrumPail`, `DrumPailColour`, `DrumPailMould`, `DrumPailLogo`, `labelColour`, `labelMould`, `labelLogo`, `stickerColour`, `stickerMould`, `stickerLogo`, `oppColour`, `oppMould`, `oppLogo`, `otherColour`, `otherMould`, `otherLogo`, `coolColour`, `coolFragrance`, `apiRegister`, `apiDate`, `apiOther`, `formEDRegister`, `formEDDate`, `formEDOther`, `webRegister`, `webDate`, `webOther`, `packSize`, `packLitre`, `productLaunchDate`, `qty1Production`, `stockCode`, `PPMUser`, `remark`) VALUES (@serialNo, @requestBy, @bottleColour, @bottleMould, @bottleLogo, @CapLip, @CapLipColour, @CapLipMould, @CapLipLogo, @InducTab, @InducTabColour, @InducTabMould, @InducTabLogo, @DrumPail, @DrumPailColour, @DrumPailMould, @DrumPailLogo, @labelColour, @labelMould, @labelLogo, @stickerColour, @stickerMould, @stickerLogo, @oppColour, @oppMould, @oppLogo, @otherColour, @otherMould, @otherLogo, @coolColour, @coolFragrance, @apiRegister, @apiDate, @apiOther, @formEDRegister, @formEDDate, @formEDOther, @webRegister, @webDate, @webOther, @packSize, @packLitre, @productLaunchDate, @qty1Production, @stockCode, @PPMUser, @remark);

                            INSERT INTO `newproductrequest_review`
                            (`serialNo`, `requestBy`) VALUES (@serialNo, @requestBy);";

                            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                            {
                                using (MySqlCommand command = new MySqlCommand(query, connection))
                                {
                                    // use for three table
                                    command.Parameters.AddWithValue("@serialNo", serialNo.Text);
                                    command.Parameters.AddWithValue("@requestBy", salesmanName.Text);

                                    command.Parameters.AddWithValue("@date", requestDate.Text); //currentDateTime.ToString("yyyy-MM-dd"));
                                    command.Parameters.AddWithValue("@time", currentTime);
                                    command.Parameters.AddWithValue("@status", "Draft");
                                    command.Parameters.AddWithValue("@name", productName.Text);
                                    command.Parameters.AddWithValue("@application", productApp.Text);
                                    command.Parameters.AddWithValue("@performance", productPerform.Text);


                                    command.Parameters.AddWithValue("@bottleColour", colorBottle.Text);
                                    command.Parameters.AddWithValue("@bottleMould", oriBottle.Text);
                                    command.Parameters.AddWithValue("@bottleLogo", logoBottle.Text);

                                    if (!radCap.Checked && !radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", emptyBlank);
                                    }
                                    else if (radCap.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Cap");
                                    }
                                    else if (radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Lid");
                                    }

                                    command.Parameters.AddWithValue("@CapLipColour", colourCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipMould", oriCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipLogo", logoCapLip.Text);

                                    if (!radInduc.Checked && !radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", emptyBlank);
                                    }
                                    else if (radInduc.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Induction");
                                    }
                                    else if (radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Tab Seal");
                                    }

                                    command.Parameters.AddWithValue("@InducTabColour", colorInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabMould", oriInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabLogo", logoInduction.Text);

                                    if (!radDrum.Checked && !radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", emptyBlank);
                                    }
                                    else if (radDrum.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Drum");
                                    }
                                    else if (radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Pail");
                                    }

                                    command.Parameters.AddWithValue("@DrumPailColour", colorDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailMould", oriDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailLogo", logoDrumPail.Text);
                                    command.Parameters.AddWithValue("@labelColour", colorLabel.Text);
                                    command.Parameters.AddWithValue("@labelMould", oriLabel.Text);
                                    command.Parameters.AddWithValue("@labelLogo", logoLabel.Text);
                                    command.Parameters.AddWithValue("@stickerColour", colorSticker.Text);
                                    command.Parameters.AddWithValue("@stickerMould", oriSticker.Text);
                                    command.Parameters.AddWithValue("@stickerLogo", logoSticker.Text);
                                    command.Parameters.AddWithValue("@oppColour", colorOpp.Text);
                                    command.Parameters.AddWithValue("@oppMould", oriOpp.Text);
                                    command.Parameters.AddWithValue("@oppLogo", logoOpp.Text);
                                    command.Parameters.AddWithValue("@otherColour", colorOthers.Text);
                                    command.Parameters.AddWithValue("@otherMould", oriOthers.Text);
                                    command.Parameters.AddWithValue("@otherLogo", logoOthers.Text);
                                    command.Parameters.AddWithValue("@coolColour", coolantColour.Text);

                                    if (!radCoolYes.Checked && !radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", emptyBlank);
                                    }
                                    else if (radCoolYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "Yes");
                                    }
                                    else if (radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "No");
                                    }


                                    if (!apiYes.Checked && !apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", emptyBlank);
                                    }
                                    else if (apiYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "Yes");
                                    }
                                    else if (apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@apiDate", apiDate.Text);
                                    command.Parameters.AddWithValue("@apiOther", apiOthers.Text);

                                    if (!EDYes.Checked && !EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", emptyBlank);
                                    }
                                    else if (EDYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "Yes");
                                    }
                                    else if (EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@formEDDate", EDDate.Text);
                                    command.Parameters.AddWithValue("@formEDOther", EDOthers.Text);

                                    if (!APPYes.Checked && !APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", emptyBlank);
                                    }
                                    else if (APPYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "Yes");
                                    }
                                    else if (APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@webDate", APPDate.Text);
                                    command.Parameters.AddWithValue("@webOther", APPOthers.Text);
                                    command.Parameters.AddWithValue("@packSize", packSize.Text);
                                    command.Parameters.AddWithValue("@packLitre", litreX.Text);
                                    command.Parameters.AddWithValue("@productLaunchDate", launchDate.Text);
                                    command.Parameters.AddWithValue("@qty1Production", qty1Product.Text);
                                    command.Parameters.AddWithValue("@stockCode", stockCode.Text);
                                    command.Parameters.AddWithValue("@PPMUser", PPMYes.Checked ? 1 : 0);
                                    command.Parameters.AddWithValue("@remark", remarks.Text);

                                    connection.Open();
                                    command.ExecuteNonQuery();

                                    Response.Redirect("NewProductHomePage.aspx", false);
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                }
            }


        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            string emptyBlank = "";
            string serialNum = serialNo.Text.Trim();

            if (!string.IsNullOrEmpty(serialNum))
            {
                string findSerial = "SELECT * FROM newproductrequest_info WHERE serialNo = @serialNo";

                using (MySqlConnection connectionNum = new MySqlConnection(GLOBAL.connStr))
                {
                    MySqlCommand commandNo = new MySqlCommand(findSerial, connectionNum);
                    commandNo.Parameters.AddWithValue("@serialNo", serialNum);

                    try
                    {
                        connectionNum.Open();
                        MySqlDataReader ReaderNum = commandNo.ExecuteReader();

                        if (ReaderNum.HasRows)
                        {
                            // exist

                            string query =
                            @"UPDATE newproductrequest_info SET " +
                            "name = @name, application = @application, performance = @performance, status = @status WHERE serialNo = @serialNo;" +

                            "UPDATE newproductrequest_detail SET " +
                            "bottleColour = @bottleColour, bottleMould = @bottleMould, bottleLogo = @bottleLogo, CapLip = @CapLip, CapLipColour = @CapLipColour, CapLipMould = @CapLipMould, CapLipLogo = @CapLipLogo, InducTab = @InducTab, InducTabColour = @InducTabColour, InducTabMould = @InducTabMould, InducTabLogo = @InducTabLogo, DrumPail = @DrumPail, DrumPailColour = @DrumPailColour, DrumPailMould = @DrumPailMould, DrumPailLogo = @DrumPailLogo, labelColour = @labelColour, labelMould = @labelMould, labelLogo = @labelLogo,  stickerColour = @stickerColour, stickerMould = @stickerMould, stickerLogo = @stickerLogo, oppColour = @oppColour, oppMould = @oppMould, oppLogo = @oppLogo, otherColour = @otherColour, otherMould = @otherMould, otherLogo = @otherLogo, coolColour = @coolColour, coolFragrance = @coolFragrance, apiRegister = @apiRegister, apiDate = @apiDate, apiOther = @apiOther, formEDRegister = @formEDRegister, formEDDate = @formEDDate, formEDOther = @formEDOther, webRegister = @webRegister, webDate = @webDate, webOther = @webOther, packSize = @packSize, packLitre = @packLitre, productLaunchDate = @productLaunchDate, qty1Production = @qty1Production, stockCode = @stockCode, PPMUser = @PPMUser, remark = @remark " +
                            "WHERE serialNo = @serialNo;";

                            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                            {
                                using (MySqlCommand command = new MySqlCommand(query, connection))
                                {
                                    // use for three table
                                    command.Parameters.AddWithValue("@serialNo", serialNo.Text);
                                    command.Parameters.AddWithValue("@requestBy", salesmanName.Text);

                                    string waitCheck = checkerText.Text; //ddlCheck.SelectedValue;
                                    string statusCheck = "Waiting Checker: " + waitCheck;

                                    command.Parameters.AddWithValue("@status", statusCheck);
                                    command.Parameters.AddWithValue("@name", productName.Text);
                                    command.Parameters.AddWithValue("@application", productApp.Text);
                                    command.Parameters.AddWithValue("@performance", productPerform.Text);


                                    command.Parameters.AddWithValue("@bottleColour", colorBottle.Text);
                                    command.Parameters.AddWithValue("@bottleMould", oriBottle.Text);
                                    command.Parameters.AddWithValue("@bottleLogo", logoBottle.Text);

                                    if (!radCap.Checked && !radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Cap / Lid");
                                    }
                                    else if (radCap.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Cap");
                                    }
                                    else if (radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Lid");
                                    }

                                    command.Parameters.AddWithValue("@CapLipColour", colourCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipMould", oriCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipLogo", logoCapLip.Text);

                                    if (!radInduc.Checked && !radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Induction / Tab Seal");
                                    }
                                    else if (radInduc.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Induction");
                                    }
                                    else if (radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Tab Seal");
                                    }

                                    command.Parameters.AddWithValue("@InducTabColour", colorInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabMould", oriInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabLogo", logoInduction.Text);

                                    if (!radDrum.Checked && !radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Drum / Pail");
                                    }
                                    else if (radDrum.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Drum");
                                    }
                                    else if (radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Pail");
                                    }

                                    command.Parameters.AddWithValue("@DrumPailColour", colorDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailMould", oriDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailLogo", logoDrumPail.Text);
                                    command.Parameters.AddWithValue("@labelColour", colorLabel.Text);
                                    command.Parameters.AddWithValue("@labelMould", oriLabel.Text);
                                    command.Parameters.AddWithValue("@labelLogo", logoLabel.Text);
                                    command.Parameters.AddWithValue("@stickerColour", colorSticker.Text);
                                    command.Parameters.AddWithValue("@stickerMould", oriSticker.Text);
                                    command.Parameters.AddWithValue("@stickerLogo", logoSticker.Text);
                                    command.Parameters.AddWithValue("@oppColour", colorOpp.Text);
                                    command.Parameters.AddWithValue("@oppMould", oriOpp.Text);
                                    command.Parameters.AddWithValue("@oppLogo", logoOpp.Text);
                                    command.Parameters.AddWithValue("@otherColour", colorOthers.Text);
                                    command.Parameters.AddWithValue("@otherMould", oriOthers.Text);
                                    command.Parameters.AddWithValue("@otherLogo", logoOthers.Text);
                                    command.Parameters.AddWithValue("@coolColour", coolantColour.Text);

                                    if (!radCoolYes.Checked && !radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", emptyBlank);
                                    }
                                    else if (radCoolYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "Yes");
                                    }
                                    else if (radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "No");
                                    }


                                    if (!apiYes.Checked && !apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", emptyBlank);
                                    }
                                    else if (apiYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "Yes");
                                    }
                                    else if (apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@apiDate", apiDate.Text);
                                    command.Parameters.AddWithValue("@apiOther", apiOthers.Text);

                                    if (!EDYes.Checked && !EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", emptyBlank);
                                    }
                                    else if (EDYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "Yes");
                                    }
                                    else if (EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@formEDDate", EDDate.Text);
                                    command.Parameters.AddWithValue("@formEDOther", EDOthers.Text);

                                    if (!APPYes.Checked && !APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", emptyBlank);
                                    }
                                    else if (APPYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "Yes");
                                    }
                                    else if (APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@webDate", APPDate.Text);
                                    command.Parameters.AddWithValue("@webOther", APPOthers.Text);
                                    command.Parameters.AddWithValue("@packSize", packSize.Text);
                                    command.Parameters.AddWithValue("@packLitre", litreX.Text);
                                    command.Parameters.AddWithValue("@productLaunchDate", launchDate.Text);
                                    command.Parameters.AddWithValue("@qty1Production", qty1Product.Text);
                                    command.Parameters.AddWithValue("@stockCode", stockCode.Text);
                                    command.Parameters.AddWithValue("@PPMUser", PPMYes.Checked ? 1 : 0);
                                    command.Parameters.AddWithValue("@remark", remarks.Text);

                                    connection.Open();
                                    command.ExecuteNonQuery();

                                    // Email to checker
                                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                                    string checkUser = ddlCheck.SelectedValue;

                                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                                    {
                                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                                        {
                                            cmd.Parameters.AddWithValue("@username", "%" + checkUser + "%");

                                            try
                                            {
                                                emailConnection.Open();
                                                object result = cmd.ExecuteScalar();

                                                string checkUsr = result.ToString();

                                                string MailSubject = "New Product Request - Waiting for You to Review";
                                                string MailTo = checkUsr + "@posim.com.my";

                                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                                    "Form Created Time: " + requestDate.Text + "\n" +
                                                    "Request By: " + salesmanName.Text + "\n" +
                                                    "\nWaiting for you to review and approve.";
                                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                                            }
                                            catch { }
                                        }

                                    }

                                    Response.Redirect("NewProductHomePage.aspx", false);
                                }

                            }

                        }
                        else
                        {
                            // not exist   

                            DateTime currentDateTime = DateTime.Now;
                            TimeSpan currentTime = currentDateTime.TimeOfDay;

                            string query = @"
                            INSERT INTO `newproductrequest_info`
                            (`serialNo`, `requestBy`, `date`, `time`, `status`, `name`, `application`, `performance`) VALUES (@serialNo, @requestBy, @date, @time, @status, @name, @application, @performance);

                            INSERT INTO `newproductrequest_detail`
                            (`serialNo`, `requestBy`, `bottleColour`, `bottleMould`, `bottleLogo`, `CapLip`, `CapLipColour`, `CapLipMould`, `CapLipLogo`, `InducTab`, `InducTabColour`, `InducTabMould`, `InducTabLogo`, `DrumPail`, `DrumPailColour`, `DrumPailMould`, `DrumPailLogo`, `labelColour`, `labelMould`, `labelLogo`, `stickerColour`, `stickerMould`, `stickerLogo`, `oppColour`, `oppMould`, `oppLogo`, `otherColour`, `otherMould`, `otherLogo`, `coolColour`, `coolFragrance`, `apiRegister`, `apiDate`, `apiOther`, `formEDRegister`, `formEDDate`, `formEDOther`, `webRegister`, `webDate`, `webOther`, `packSize`, `packLitre`, `productLaunchDate`, `qty1Production`, `stockCode`, `PPMUser`, `remark`) VALUES (@serialNo, @requestBy, @bottleColour, @bottleMould, @bottleLogo, @CapLip, @CapLipColour, @CapLipMould, @CapLipLogo, @InducTab, @InducTabColour, @InducTabMould, @InducTabLogo, @DrumPail, @DrumPailColour, @DrumPailMould, @DrumPailLogo, @labelColour, @labelMould, @labelLogo, @stickerColour, @stickerMould, @stickerLogo, @oppColour, @oppMould, @oppLogo, @otherColour, @otherMould, @otherLogo, @coolColour, @coolFragrance, @apiRegister, @apiDate, @apiOther, @formEDRegister, @formEDDate, @formEDOther, @webRegister, @webDate, @webOther, @packSize, @packLitre, @productLaunchDate, @qty1Production, @stockCode, @PPMUser, @remark);

                            INSERT INTO `newproductrequest_review`
                            (`serialNo`, `requestBy`) VALUES (@serialNo, @requestBy);";

                            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                            {
                                using (MySqlCommand command = new MySqlCommand(query, connection))
                                {
                                    // use for three table
                                    command.Parameters.AddWithValue("@serialNo", serialNo.Text);
                                    command.Parameters.AddWithValue("@requestBy", salesmanName.Text);

                                    command.Parameters.AddWithValue("@date", requestDate.Text); //currentDateTime.ToString("yyyy-MM-dd"));
                                    command.Parameters.AddWithValue("@time", currentTime);

                                    string waitCheck = checkerText.Text; //ddlCheck.SelectedValue;
                                    string statusCheck = "Waiting Checker: " + waitCheck;

                                    command.Parameters.AddWithValue("@status", statusCheck);
                                    command.Parameters.AddWithValue("@name", productName.Text);
                                    command.Parameters.AddWithValue("@application", productApp.Text);
                                    command.Parameters.AddWithValue("@performance", productPerform.Text);


                                    command.Parameters.AddWithValue("@bottleColour", colorBottle.Text);
                                    command.Parameters.AddWithValue("@bottleMould", oriBottle.Text);
                                    command.Parameters.AddWithValue("@bottleLogo", logoBottle.Text);

                                    if (!radCap.Checked && !radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Cap / Lid");
                                    }
                                    else if (radCap.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Cap");
                                    }
                                    else if (radLid.Checked)
                                    {
                                        command.Parameters.AddWithValue("@CapLip", "Lid");
                                    }

                                    command.Parameters.AddWithValue("@CapLipColour", colourCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipMould", oriCapLip.Text);
                                    command.Parameters.AddWithValue("@CapLipLogo", logoCapLip.Text);

                                    if (!radInduc.Checked && !radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Induction / Tab Seal");
                                    }
                                    else if (radInduc.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Induction");
                                    }
                                    else if (radTab.Checked)
                                    {
                                        command.Parameters.AddWithValue("@InducTab", "Tab Seal");
                                    }

                                    command.Parameters.AddWithValue("@InducTabColour", colorInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabMould", oriInduction.Text);
                                    command.Parameters.AddWithValue("@InducTabLogo", logoInduction.Text);

                                    if (!radDrum.Checked && !radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Drum / Pail");
                                    }
                                    else if (radDrum.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Drum");
                                    }
                                    else if (radPail.Checked)
                                    {
                                        command.Parameters.AddWithValue("@DrumPail", "Pail");
                                    }

                                    command.Parameters.AddWithValue("@DrumPailColour", colorDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailMould", oriDrumPail.Text);
                                    command.Parameters.AddWithValue("@DrumPailLogo", logoDrumPail.Text);
                                    command.Parameters.AddWithValue("@labelColour", colorLabel.Text);
                                    command.Parameters.AddWithValue("@labelMould", oriLabel.Text);
                                    command.Parameters.AddWithValue("@labelLogo", logoLabel.Text);
                                    command.Parameters.AddWithValue("@stickerColour", colorSticker.Text);
                                    command.Parameters.AddWithValue("@stickerMould", oriSticker.Text);
                                    command.Parameters.AddWithValue("@stickerLogo", logoSticker.Text);
                                    command.Parameters.AddWithValue("@oppColour", colorOpp.Text);
                                    command.Parameters.AddWithValue("@oppMould", oriOpp.Text);
                                    command.Parameters.AddWithValue("@oppLogo", logoOpp.Text);
                                    command.Parameters.AddWithValue("@otherColour", colorOthers.Text);
                                    command.Parameters.AddWithValue("@otherMould", oriOthers.Text);
                                    command.Parameters.AddWithValue("@otherLogo", logoOthers.Text);
                                    command.Parameters.AddWithValue("@coolColour", coolantColour.Text);

                                    if (!radCoolYes.Checked && !radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", emptyBlank);
                                    }
                                    else if (radCoolYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "Yes");
                                    }
                                    else if (radCoolNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@coolFragrance", "No");
                                    }


                                    if (!apiYes.Checked && !apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", emptyBlank);
                                    }
                                    else if (apiYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "Yes");
                                    }
                                    else if (apiNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@apiRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@apiDate", apiDate.Text);
                                    command.Parameters.AddWithValue("@apiOther", apiOthers.Text);

                                    if (!EDYes.Checked && !EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", emptyBlank);
                                    }
                                    else if (EDYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "Yes");
                                    }
                                    else if (EDNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@formEDRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@formEDDate", EDDate.Text);
                                    command.Parameters.AddWithValue("@formEDOther", EDOthers.Text);

                                    if (!APPYes.Checked && !APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", emptyBlank);
                                    }
                                    else if (APPYes.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "Yes");
                                    }
                                    else if (APPNo.Checked)
                                    {
                                        command.Parameters.AddWithValue("@webRegister", "No");
                                    }

                                    command.Parameters.AddWithValue("@webDate", APPDate.Text);
                                    command.Parameters.AddWithValue("@webOther", APPOthers.Text);
                                    command.Parameters.AddWithValue("@packSize", packSize.Text);
                                    command.Parameters.AddWithValue("@packLitre", litreX.Text);
                                    command.Parameters.AddWithValue("@productLaunchDate", launchDate.Text);
                                    command.Parameters.AddWithValue("@qty1Production", qty1Product.Text);
                                    command.Parameters.AddWithValue("@stockCode", stockCode.Text);
                                    command.Parameters.AddWithValue("@PPMUser", PPMYes.Checked ? 1 : 0);
                                    command.Parameters.AddWithValue("@remark", remarks.Text);

                                    connection.Open();
                                    command.ExecuteNonQuery();

                                    // Email to checker
                                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                                    string checkUser = ddlCheck.SelectedValue;

                                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                                    {
                                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                                        {
                                            cmd.Parameters.AddWithValue("@username", "%" + checkUser + "%");

                                            try
                                            {
                                                emailConnection.Open();
                                                object result = cmd.ExecuteScalar();

                                                string checkUsr = result.ToString();

                                                string MailSubject = "New Product Request - Waiting for You to Review";
                                                string MailTo = checkUsr + "@posim.com.my";

                                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                                    "Form Created Time: " + requestDate.Text + "\n" +
                                                    "Request By: " + salesmanName.Text + "\n\n" +
                                                    "\nWaiting for you to review and approve.";
                                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                                            }
                                            catch { }
                                        }

                                    }

                                    Response.Redirect("NewProductHomePage.aspx", false);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        protected void btnAuthor_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query =
                    @"UPDATE newproductrequest_info SET " +
                    "status = @status WHERE serialNo = @serialNo;" +

                    "UPDATE newproductrequest_review SET " +
                    "QCName = @QCName, QCAcknow = @QCAcknow, QCDate = @QCDate, QCRemark = @QCRemark, ProductName = @ProductName, ProductAcknow = @ProductAcknow, ProductDate = @ProductDate, ProductRemark = @ProductRemark, PurchaseName = @PurchaseName, PurchaseAcknow = @PurchaseAcknow, PurchaseDate = @PurchaseDate, PurchaseRemark = @PurchaseRemark WHERE serialNo = @serialNo;";

                MySqlCommand command = new MySqlCommand(query, connection);

                //string emptyBlank = "";
                command.Parameters.AddWithValue("@serialNo", serialNo.Text);

                if (qcUser.Text == "")
                {
                    string waitProduct = productText.Text; //ddlProduct.SelectedValue;
                    string statusProduct = "Waiting Production: " + waitProduct;
                    command.Parameters.AddWithValue("@status", statusProduct);
                    command.Parameters.AddWithValue("@QCName", ddlQC.SelectedValue);
                    command.Parameters.AddWithValue("@QCAcknow", qcSign.Text);
                    command.Parameters.AddWithValue("@QCDate", qcDate.Text);
                    command.Parameters.AddWithValue("@QCRemark", qcRemark.Text);

                    // Email to product
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string productUser = ddlProduct.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + productUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string productUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Authorise";
                                string MailTo = productUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and authorise.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@QCName", qcUser.Text);
                    command.Parameters.AddWithValue("@QCAcknow", qcSign.Text);
                    command.Parameters.AddWithValue("@QCDate", qcDate.Text);
                    command.Parameters.AddWithValue("@QCRemark", qcRemark.Text);
                }

                if (qcUser.Text != "" && productUser.Text == "")
                {
                    string waitPurchase = purchaseText.Text; //ddlPurchase.SelectedValue;
                    string statusPurchase = "Waiting Purchase: " + waitPurchase;
                    command.Parameters.AddWithValue("@status", statusPurchase);
                    command.Parameters.AddWithValue("@ProductName", ddlProduct.SelectedValue);
                    command.Parameters.AddWithValue("@ProductAcknow", productSign.Text);
                    command.Parameters.AddWithValue("@ProductDate", productDate.Text);
                    command.Parameters.AddWithValue("@ProductRemark", productRemark.Text);

                    // Email to purchase
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string purchaseUser = ddlPurchase.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + purchaseUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string purchaseUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Authorise";
                                string MailTo = purchaseUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and authorise.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@ProductName", productUser.Text);
                    command.Parameters.AddWithValue("@ProductAcknow", productSign.Text);
                    command.Parameters.AddWithValue("@ProductDate", productDate.Text);
                    command.Parameters.AddWithValue("@ProductRemark", productRemark.Text);
                }

                if (productUser.Text != "" && purchaseUser.Text == "")
                {
                    string waitWare = wareText.Text; //ddlWarehouse.SelectedValue;
                    string statusWare = "Waiting Warehouse: " + waitWare;
                    command.Parameters.AddWithValue("@status", statusWare);
                    command.Parameters.AddWithValue("@PurchaseName", ddlPurchase.SelectedValue);
                    command.Parameters.AddWithValue("@PurchaseAcknow", purchaseSign.Text);
                    command.Parameters.AddWithValue("@PurchaseDate", purchaseDate.Text);
                    command.Parameters.AddWithValue("@PurchaseRemark", purchaseRemark.Text);

                    // Email to warehouse
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string wareUser = ddlWarehouse.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + wareUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string wareUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Acknowledge";
                                string MailTo = wareUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and acknowledge.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@PurchaseName", purchaseUser.Text);
                    command.Parameters.AddWithValue("@PurchaseAcknow", purchaseSign.Text);
                    command.Parameters.AddWithValue("@PurchaseDate", purchaseDate.Text);
                    command.Parameters.AddWithValue("@PurchaseRemark", purchaseRemark.Text);
                }

                connection.Open();
                command.ExecuteNonQuery();

                Response.Redirect("NewProductHomePage.aspx", false);

            }
            catch (Exception ex)
            { }

        }

        protected void btnAcknow_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query =
                    @"UPDATE newproductrequest_info SET " +
                    "status = @status WHERE serialNo = @serialNo;" +

                    "UPDATE newproductrequest_review SET " +
                    "WarehouseName = @WarehouseName, WarehouseAcknow = @WarehouseAcknow, WarehouseDate = @WarehouseDate, WarehouseRemark = @WarehouseRemark, AnPName = @AnPName, AnPAcknow = @AnPAcknow, AnpDate = @AnpDate, AnPRemark = @AnPRemark, ITName = @ITName, ITAcknow = @ITAcknow, ITDate = @ITDate, ITRemark = @ITRemark WHERE serialNo = @serialNo;";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@serialNo", serialNo.Text);

                if (wareUser.Text == "")
                {
                    string waitAnP = marketText.Text; //ddlAnP.SelectedValue;
                    string statusAnP = "Waiting Marketing: " + waitAnP;
                    command.Parameters.AddWithValue("@status", statusAnP);
                    command.Parameters.AddWithValue("@WarehouseName", ddlWarehouse.SelectedValue);
                    command.Parameters.AddWithValue("@WarehouseAcknow", wareSign.Text);
                    command.Parameters.AddWithValue("@WarehouseDate", wareDate.Text);
                    command.Parameters.AddWithValue("@WarehouseRemark", wareRemark.Text);

                    // Email to marketing
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string anpUser = ddlAnP.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + anpUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string anpUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Acknowledge";
                                string MailTo = anpUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and acknowledge.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@WarehouseName", wareUser.Text);
                    command.Parameters.AddWithValue("@WarehouseAcknow", wareSign.Text);
                    command.Parameters.AddWithValue("@WarehouseDate", wareDate.Text);
                    command.Parameters.AddWithValue("@WarehouseRemark", wareRemark.Text);
                }

                if (wareUser.Text != "" && anpUser.Text == "")
                {
                    string waitIT = itText.Text; //ddlIT.SelectedValue;
                    string statusIT = "Waiting IT: " + waitIT;
                    command.Parameters.AddWithValue("@status", statusIT);
                    command.Parameters.AddWithValue("@AnPName", ddlAnP.SelectedValue);
                    command.Parameters.AddWithValue("@AnPAcknow", anpSign.Text);
                    command.Parameters.AddWithValue("@AnpDate", anpDate.Text);
                    command.Parameters.AddWithValue("@AnPRemark", anpRemark.Text);

                    // Email to IT
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string itUser = ddlIT.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + itUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string itUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Acknowledge";
                                string MailTo = itUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and acknowledge.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@AnPName", anpUser.Text);
                    command.Parameters.AddWithValue("@AnPAcknow", anpSign.Text);
                    command.Parameters.AddWithValue("@AnpDate", anpDate.Text);
                    command.Parameters.AddWithValue("@AnPRemark", anpRemark.Text);
                }

                if (anpUser.Text != "" && itUser.Text == "")
                {
                    string waitApprove = ddlApprove.SelectedValue;
                    string statusApprove = "Waiting Approve: " + waitApprove;
                    command.Parameters.AddWithValue("@status", statusApprove);
                    command.Parameters.AddWithValue("@ITName", ddlIT.SelectedValue);
                    command.Parameters.AddWithValue("@ITAcknow", itSign.Text);
                    command.Parameters.AddWithValue("@ITDate", itDate.Text);
                    command.Parameters.AddWithValue("@ITRemark", itRemark.Text);

                    // Email to product
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string approveUser = ddlApprove.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + approveUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string approveUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Approve";
                                string MailTo = approveUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and approve.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@ITName", itUser.Text);
                    command.Parameters.AddWithValue("@ITAcknow", itSign.Text);
                    command.Parameters.AddWithValue("@ITDate", itDate.Text);
                    command.Parameters.AddWithValue("@ITRemark", itRemark.Text);
                }

                connection.Open();
                command.ExecuteNonQuery();

                Response.Redirect("NewProductHomePage.aspx", false);

            }
            catch (Exception ex)
            { }



        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query =
                @"UPDATE newproductrequest_info SET " +
                "status = @status WHERE serialNo = @serialNo;" +

                "UPDATE newproductrequest_review SET " +
                "checkedBy = @checkedBy, checkAcknow = @checkAcknow, checkDate = @checkDate, checkRemark = @checkRemark, ApproveName = @ApproveName, ApproveAcknow = @ApproveAcknow, ApproveDate = @ApproveDate, ApproveRemark = @ApproveRemark WHERE serialNo = @serialNo;";

                MySqlCommand command = new MySqlCommand(query, connection);

                string emptyBlank = "";
                string checkusr = checkUser.Text;
                command.Parameters.AddWithValue("@serialNo", serialNo.Text);

                if (checkusr == "")
                {

                    string waitQC = qcText.Text; //ddlQC.SelectedValue;
                    string statusQC = "Waiting QC: " + waitQC;
                    command.Parameters.AddWithValue("@status", statusQC);
                    command.Parameters.AddWithValue("@checkedBy", ddlCheck.SelectedValue);
                    command.Parameters.AddWithValue("@checkAcknow", checkSign.Text);
                    command.Parameters.AddWithValue("@checkDate", checkDate.Text);
                    command.Parameters.AddWithValue("@checkRemark", checkRemark.Text);

                    // Email to QC
                    string emailQuery = "SELECT user_id FROM user_tbl WHERE logined_user_name LIKE @username";

                    string qcUser = ddlQC.SelectedValue;

                    using (MySqlConnection emailConnection = new MySqlConnection(GLOBAL.connStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(emailQuery, emailConnection))
                        {
                            cmd.Parameters.AddWithValue("@username", "%" + qcUser + "%");

                            try
                            {
                                emailConnection.Open();
                                object result = cmd.ExecuteScalar();

                                string qcUsr = result.ToString();

                                string MailSubject = "New Product Request - Waiting for You to Review";
                                string MailTo = qcUsr + "@posim.com.my";

                                //  text = raw_text.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                                string SendMsg = "Form Number: " + serialNo.Text + "\n" +
                                    "Form Created Time: " + requestDate.Text + "\n" +
                                    "Request By: " + salesmanName.Text + "\n" +
                                    "\nWaiting for you to review and authorise.";
                                Function_Method.SendMail("mailadmin", "administrator", MailSubject, MailTo, "", SendMsg);
                            }
                            catch { }
                        }

                    }

                }
                else
                {
                    command.Parameters.AddWithValue("@checkedBy", checkUser.Text);
                    command.Parameters.AddWithValue("@checkAcknow", checkSign.Text);
                    command.Parameters.AddWithValue("@checkDate", checkDate.Text);
                    command.Parameters.AddWithValue("@checkRemark", checkRemark.Text);
                }

                if (itUser.Text != "")
                {
                    command.Parameters.AddWithValue("@status", "Approve");
                    command.Parameters.AddWithValue("@ApproveName", ddlApprove.SelectedValue);
                    command.Parameters.AddWithValue("@ApproveAcknow", approveSign.Text);
                    command.Parameters.AddWithValue("@ApproveDate", approveDate.Text);
                    command.Parameters.AddWithValue("@ApproveRemark", approveRemark.Text);
                }
                else
                {
                    command.Parameters.AddWithValue("@ApproveName", emptyBlank);
                    command.Parameters.AddWithValue("@ApproveAcknow", approveSign.Text);
                    command.Parameters.AddWithValue("@ApproveDate", approveDate.Text);
                    command.Parameters.AddWithValue("@ApproveRemark", approveRemark.Text);
                }

                connection.Open();
                command.ExecuteNonQuery();

                Response.Redirect("NewProductHomePage.aspx", false);


            }
            catch (Exception ex)
            { }



        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query =
                @"UPDATE newproductrequest_info SET " +
                "status = @status WHERE serialNo = @serialNo;" +

                "UPDATE newproductrequest_review SET " +
                "ApproveName = @ApproveName, ApproveKnow = @ApproveAcknow, ApproveDate = @ApproveDate, ApproveRemark = @ApproveRemark WHERE serialNo = @serialNo;";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@serialNo", serialNo.Text);

                if (itUser.Text != "")
                {
                    command.Parameters.AddWithValue("@status", "Reject");
                    command.Parameters.AddWithValue("@ApproveName", ddlApprove.SelectedValue);
                    command.Parameters.AddWithValue("@ApproveAcknow", approveSign.Text);
                    command.Parameters.AddWithValue("@ApproveDate", approveDate.Text);
                    command.Parameters.AddWithValue("@ApproveRemark", approveRemark.Text);
                }
                connection.Open();
                command.ExecuteNonQuery();

                Response.Redirect("NewProductHomePage.aspx", false);

            }
            catch (Exception ex)
            { }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewProductHomePage.aspx");
        }

    }

}