using ActiveDs;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class CompanyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            check_session();
            TimeOutRedirect();
            if (!IsPostBack)
            {
                string salesmanName = Session["SalesmanName"] as string;
                string status = Session["Status"] as string;
                string salesmanCode = Session["SalesmanCode"] as string;
                string formNo = Session["FormNo"] as string;
                string coordinate = Session["Coordinate"] as string;
                string custName = Session["CustName"] as string;
                string newCustID = Session["NewCustID"] as string;

                txtSalesmanName.Text = salesmanName;
                txtStatus.Text = status;
                txtSalesmanCode.Text = salesmanCode;
                txtFormNo.Text = formNo;
                txtCoordinate.Text = coordinate;
                txtCustomerName.Text = custName;

                PopulateCustomerTypeDropDown();
                PopulateCustomerMainGroupDropDown();
                PopulateCreditTermDropDown();
                PopulateCustomerDescriptionDropDown();
                PopulateTerritoryDropDown();
                PopulateInventLocationDropDown();
                PopulateStateIdDropDown();
                CheckUserAccess();
                CheckCreditControlAcess();
                LoginAccess();

                if (!string.IsNullOrEmpty(Request.QueryString["NewCustID"]))
                {
                    string newCustIDFromQueryString = Request.QueryString["NewCustID"];
                    // Log the custNameFromQueryString for debugging
                    System.Diagnostics.Debug.WriteLine($"NewCustIDFromQueryString: {newCustIDFromQueryString}");

                    LoadDataForDraft(newCustIDFromQueryString);
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
                Session.Abandon();
                Response.Redirect("LoginPage.aspx");
            }
        }

        private void CheckUserAccess()
        {
            // Assuming you have a MySqlConnection configured appropriately
            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                connection.Open();

                // Replace 'your_query' with the correct query to fetch data from your database
                string query = "SELECT * FROM newcust_department_access " +
                               "WHERE Hod1 = @userId OR Hod2 = @userId OR Hod3 = @userId";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Set parameter to the global user_id
                    cmd.Parameters.AddWithValue("@userId", GLOBAL.user_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check if there is at least one row with a matching user_id
                        if (reader.HasRows)
                        {
                            // Set the visibility of the table to true
                            tblHODRecommendation.Visible = true;
                        }
                        else
                        {
                            // Set the visibility of the table to false
                            tblHODRecommendation.Visible = false;
                            tblCreditControlDocumentation.Visible = false;
                            tblCreditControlAccStatus.Visible = false;
                        }
                    }
                }
            }
        }      
        
        private void CheckCreditControlAcess()
        {
            // Assuming you have a MySqlConnection configured appropriately
            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                connection.Open();

                // Replace 'your_query' with the correct query to fetch data from your database
                string query = "SELECT * FROM newcust_department_access " +
                               "WHERE CreditControl = @userId";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Set parameter to the global user_id
                    cmd.Parameters.AddWithValue("@userId", GLOBAL.user_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check if there is at least one row with a matching user_id
                        if (reader.HasRows)
                        {
                            // Set the visibility of the table to true
                            tblHODRecommendation.Visible = true;
                            tblCreditControlDocumentation.Visible = true;
                            tblCreditControlAccStatus.Visible = true;
                        }
                        else
                        {
                            // Set the visibility of the table to false
                            tblHODRecommendation.Visible = false;
                            tblCreditControlDocumentation.Visible = false;
                            tblCreditControlAccStatus.Visible = false;
                        }
                    }
                }
            }
        }

        private void LoginAccess()
        {
            // Assuming you have a MySqlConnection configured appropriately
            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                connection.Open();

                // Replace 'your_query' with the correct parameterized query
                string query = "SELECT * FROM newcust_department_access " +
                               "WHERE CreditControl = @userId";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Set parameter to the global user_id
                    cmd.Parameters.AddWithValue("@userId", GLOBAL.user_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check if there is at least one row with a matching user_id
                        if (reader.HasRows)
                        {
                            // User has the right access; enable the checkboxes or take appropriate action
                            EnableCheckboxes();
                        }
                        else
                        {
                            // User doesn't have the right access; disable the checkboxes or take appropriate action
                            DisableCheckboxes();
                        }
                    }
                }
            }
        }

        private void EnableCheckboxes()
        {
            creditApplicationFormReceivedByCreditAdmin.Enabled = true;
            creditApplicationFormRequestNewDocument.Enabled = true;
            personalGuaranteeReceivedByCreditAdmin.Enabled = true;
            personalGuaranteeRequestNewDocument.Enabled = true;
            SalesmanRecommmendationFormByCreditAdmin.Enabled = true;
            SalesmanRecommmendationFormByNewDocument.Enabled = true;
            PhotocopyByCreditAdmin.Enabled = true;
            PhotocopyByNewDocument.Enabled = true;
            declarationFormReceivedByCreditAdmin.Enabled = true;
            declarationFormRequestNewDocument.Enabled = true;
            workshopPhotos1ReceivedByCreditAdmin.Enabled = true;
            workshopPhotos1RequestNewDocument.Enabled = true;
            workshopPhotos2ReceivedByCreditAdmin.Enabled = true;
            workshopPhotos2RequestNewDocument.Enabled = true;
            workshopPhotos3ReceivedByCreditAdmin.Enabled = true;
            workshopPhotos3RequestNewDocument.Enabled = true;
            workshopPhotos4ReceivedByCreditAdmin.Enabled = true;
            workshopPhotos4RequestNewDocument.Enabled = true;
            workshopPhotos5ReceivedByCreditAdmin.Enabled = true;
            workshopPhotos5RequestNewDocument.Enabled = true;
            PhoneReceivedByCreditAdmin.Enabled = true;
            PhoneRequestNewDocument.Enabled = true;
            EmailReceivedByCreditAdmin.Enabled = true;
            EmailRequestNewDocument.Enabled = true;
            registrationCertificateReceivedByCreditAdmin.Enabled = true;
            registrationCertificateRequestNewDocument.Enabled = true;
            businessLicenseReceivedByCreditAdmin.Enabled = true;
            businessLicenseRequestNewDocument.Enabled = true;
            financialReportReceivedByCreditAdmin.Enabled = true;
            financialReportRequestNewDocument.Enabled = true;
            bankStatementsReceivedByCreditAdmin.Enabled = true;
            bankStatementsRequestNewDocument.Enabled = true;
        }

        private void DisableCheckboxes()
        {
            creditApplicationFormReceivedByCreditAdmin.Enabled = false;
            creditApplicationFormRequestNewDocument.Enabled = false;
            personalGuaranteeReceivedByCreditAdmin.Enabled = false;
            personalGuaranteeRequestNewDocument.Enabled = false;
            SalesmanRecommmendationFormByCreditAdmin.Enabled = false;
            SalesmanRecommmendationFormByNewDocument.Enabled = false;
            PhotocopyByCreditAdmin.Enabled = false;
            PhotocopyByNewDocument.Enabled = false;
            declarationFormReceivedByCreditAdmin.Enabled = false;
            declarationFormRequestNewDocument.Enabled = false;
            workshopPhotos1ReceivedByCreditAdmin.Enabled = false;
            workshopPhotos1RequestNewDocument.Enabled = false;
            workshopPhotos2ReceivedByCreditAdmin.Enabled = false;
            workshopPhotos2RequestNewDocument.Enabled = false;
            workshopPhotos3ReceivedByCreditAdmin.Enabled = false;
            workshopPhotos3RequestNewDocument.Enabled = false;
            workshopPhotos4ReceivedByCreditAdmin.Enabled = false;
            workshopPhotos4RequestNewDocument.Enabled = false;
            workshopPhotos5ReceivedByCreditAdmin.Enabled = false;
            workshopPhotos5RequestNewDocument.Enabled = false;
            PhoneReceivedByCreditAdmin.Enabled = false;
            PhoneRequestNewDocument.Enabled = false;
            EmailReceivedByCreditAdmin.Enabled = false;
            EmailRequestNewDocument.Enabled = false;
            registrationCertificateReceivedByCreditAdmin.Enabled = false;
            registrationCertificateRequestNewDocument.Enabled = false;
            businessLicenseReceivedByCreditAdmin.Enabled = false;
            businessLicenseRequestNewDocument.Enabled = false;
            financialReportReceivedByCreditAdmin.Enabled = false;
            financialReportRequestNewDocument.Enabled = false;
            bankStatementsReceivedByCreditAdmin.Enabled = false;
            bankStatementsRequestNewDocument.Enabled = false;
        }



        private void LoadDataForDraft(string newCustID)
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string detailsQuery = $"SELECT * FROM newcust_details WHERE NewCustID = @NewCustID";
                MySqlCommand detailsCommand = new MySqlCommand(detailsQuery, connection);
                detailsCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                connection.Open();
                using (MySqlDataReader detailsReader = detailsCommand.ExecuteReader())
                {
                    if (detailsReader.Read())
                    {
                        // Populate controls on your form with data from newcust_details
                        HiddenNewCustID.Text = detailsReader["NewCustID"].ToString();
                        txtSalesmanName.Text = detailsReader["SalesmanName"].ToString();
                        txtStatus.Text = detailsReader["status"].ToString();
                        txtSalesmanCode.Text = detailsReader["EmplID"].ToString();
                        txtFormNo.Text = detailsReader["FormNo"].ToString();
                        txtTIN.Text = detailsReader["CustTIN"].ToString();
                        txtPhone.Text = detailsReader["CustPhone"].ToString();
                        ddlState.SelectedValue = detailsReader["State"].ToString();
                        txtSalesmanRemark.Text = detailsReader["SalesmanMark"].ToString();
                        TextBoxCustName.Text = detailsReader["CustName"].ToString();
                        txtCustomerAddress.Text = detailsReader["CustAddress"].ToString();
                        txtCustType.Text = detailsReader["CustType"].ToString();
                        txtPostalCode.Text = detailsReader["CustPostal"].ToString();
                        txtCity.Text = detailsReader["CustCity"].ToString();
                        txtRocRob.Text = detailsReader["CustRegister"].ToString();
                        txtCustomerAddress.Text = detailsReader["CustAddress"].ToString();
                        txtCreditTerm.Text = detailsReader["CreditTerm"].ToString();
                        txtCreditlimit.Text = detailsReader["CreditLimit"].ToString();
                        txtCoordinate.Text = detailsReader["Coordinate"].ToString();
                        txtCustomerCategory.Text = detailsReader["CustCategory"].ToString();
                        txtCustomerClass.Text = detailsReader["CustClass"].ToString();
                        txtCreditTermHOD1.Text = detailsReader["CreditTerm"].ToString();
                        txtCreditTermHOD2.Text = detailsReader["CreditTerm"].ToString();
                        txtCreditLimitHOD1.Text = detailsReader["CreditLimit"].ToString();
                        txtCreditLimitHOD2.Text = detailsReader["CreditLimit"].ToString();
                        customerNameTextBox.Text = detailsReader["CustName"].ToString();


                    }
                }

                string declarationQuery = $"SELECT * FROM newcust_salesmandeclaration WHERE NewCustID = @NewCustID";
                MySqlCommand declarationCommand = new MySqlCommand(declarationQuery, connection);
                declarationCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                using (MySqlDataReader declarationReader = declarationCommand.ExecuteReader())
                {
                    if (declarationReader.Read())
                    {
                        // Populate controls on your form with data from newcust_salesmandeclaration
                        txtCustomerName.Text = declarationReader["CustName"].ToString();
                        CustomerYearsKnown.Text = declarationReader["CustYearsKnown"].ToString();
                        txtIntroducedBy.Text = declarationReader["CustRecommender"].ToString();
                        txtCompanyName.Text = declarationReader["RecommenderComp"].ToString();
                        txtWorkTitle.Text = declarationReader["CustWork"].ToString();
                        txtWorkYears.Text = declarationReader["CustYears"].ToString();
                        txtWorkCompany.Text = declarationReader["CustComp"].ToString();
                        txtReasonForLeaving.Text = declarationReader["CustLeaving"].ToString();
                        txtDaysOpenPerWeek.Text = declarationReader["CustWorkDays"].ToString();
                        txtStartHour.Text = declarationReader["CustWorkHoursAM"].ToString();
                        txtEndHour.Text = declarationReader["CustWorkHoursPM"].ToString();
                        txtHobbiesInterests.Text = declarationReader["CustHobbies"].ToString();
                        txtRentalInstallment.Text = declarationReader["ShopRental"].ToString();
                        txtOwnsHouseNo.Text = declarationReader["CustOwnNo"].ToString();
                        txtOwnsHouseType.Text = declarationReader["CustOwnType"].ToString();
                        txtOwnsHouseLocation.Text = declarationReader["CustOwnLocation"].ToString();
                        txtFinacialBy.Text = declarationReader["CustFinacial"].ToString();
                        txtOtherAssets.Text = declarationReader["CustAssets"].ToString();
                        findCust.Text = declarationReader["CustFind"].ToString();
                        shopbusy.Text = declarationReader["ShopBusy"].ToString();
                        CustConsider.Text = declarationReader["OtherComments"].ToString();
                        txtPersonName.Text = declarationReader["NameOfCust"].ToString();
                        Relationship.Text = declarationReader["CustRelationShip"].ToString();


                        stockmarket.Text = (Convert.ToInt32(declarationReader["CustStockMarket"]) == 1) ? "Yes" : "No";
                        borrowing.Text = (Convert.ToInt32(declarationReader["CustBorrowing"]) == 1) ? "Yes" : "No";
                        alongwell.Text = (Convert.ToInt32(declarationReader["CustPartners"]) == 1) ? "Yes" : "No";
                        maritalproblems.Text = (Convert.ToInt32(declarationReader["CustMarital"]) == 1) ? "Yes" : "No";
                        promises.Text = (Convert.ToInt32(declarationReader["CustPromise"]) == 1) ? "Yes" : "No";
                        cheques.Text = (Convert.ToInt32(declarationReader["CustCheque"]) == 1) ? "Yes" : "No";
                        personally.Text = (Convert.ToInt32(declarationReader["CustPersonally"]) == 1) ? "Yes" : "No";
                        WorkedInOtherTrade.Text = (Convert.ToInt32(declarationReader["CustOtherTrade"]) == 1) ? "Yes" : "No";
                        MainRoad.Text = (Convert.ToInt32(declarationReader["ShopMainRoad"]) == 1) ? "Yes" : "No";
                        DifficultToFind.Text = (Convert.ToInt32(declarationReader["ShopHardFind"]) == 1) ? "Yes" : "No";
                        GoodsMovingFast.Text = (Convert.ToInt32(declarationReader["ShopMoving"]) == 1) ? "Yes" : "No";
                        BelongsToCustomer.Text = (Convert.ToInt32(declarationReader["ShopBelong"]) == 1) ? "Yes" : "No";


                    }
                }

                string guarantorQuery = $"SELECT * FROM newcust_guarantor WHERE NewCustID = @NewCustID";
                MySqlCommand guarantorCommand = new MySqlCommand(guarantorQuery, connection);
                guarantorCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                using (MySqlDataReader guarantorReader = guarantorCommand.ExecuteReader())
                {
                    if (guarantorReader.Read())
                    {
                        //Guarantor1
                        ddlTitle.SelectedValue = guarantorReader["Title"].ToString();
                        ddlType.SelectedValue = guarantorReader["CustType"].ToString();
                        txtName.Text = guarantorReader["NameCust"].ToString();
                        txtICNo.Text = guarantorReader["CustIC"].ToString();
                        txtAddress.Text = guarantorReader["CustAddress"].ToString();
                        txtOldIC.Text = guarantorReader["CustOldIC"].ToString();
                        txtBirthDate.Text = guarantorReader["CustBirthDate"].ToString();
                        TextBoxPhone1.Text = guarantorReader["CustPhone"].ToString();
                        txtMobilePhone1.Text = guarantorReader["CustMobilePhone"].ToString();
                        //Guarantor2
                        DropDownList1.SelectedValue = guarantorReader["Title2"].ToString();
                        DropDownList2.SelectedValue = guarantorReader["CustType2"].ToString();
                        TextBox1.Text = guarantorReader["NameCust2"].ToString();
                        TextBox2.Text = guarantorReader["CustBirthDate2"].ToString();
                        TextBox3.Text = guarantorReader["CustIC2"].ToString();
                        TextBox4.Text = guarantorReader["CustMobilePhone2"].ToString();
                        TextBox5.Text = guarantorReader["CustOldIC2"].ToString();
                        TextBox6.Text = guarantorReader["CustPhone2"].ToString();
                        TextBox7.Text = guarantorReader["CustAddress2"].ToString();
                        //Guarantor3
                        DropDownList3.SelectedValue = guarantorReader["Title3"].ToString();
                        DropDownList4.SelectedValue = guarantorReader["CustType3"].ToString();
                        TextBox8.Text = guarantorReader["NameCust3"].ToString();
                        TextBox9.Text = guarantorReader["CustBirthDate3"].ToString();
                        TextBox10.Text = guarantorReader["CustIC3"].ToString();
                        TextBox11.Text = guarantorReader["CustMobilePhone3"].ToString();
                        TextBox12.Text = guarantorReader["CustOldIC3"].ToString();
                        TextBox13.Text = guarantorReader["CustPhone3"].ToString();
                        TextBox14.Text = guarantorReader["CustAddress3"].ToString();
                    }
                }
                string contactDetailsQuery = $"SELECT * FROM newcust_contactdetails WHERE NewCustID = @NewCustID";
                MySqlCommand contactDetailsCommand = new MySqlCommand(contactDetailsQuery, connection);
                contactDetailsCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                using (MySqlDataReader contactDetailsReader = contactDetailsCommand.ExecuteReader())
                {
                    if (contactDetailsReader.Read())
                    {
                        // Contact Details 1
                        CustomerNametxt.Text = contactDetailsReader["NameCust"].ToString();
                        txtICIDNo.Text = contactDetailsReader["CustIC"].ToString();
                        txtEmail.Text = contactDetailsReader["CustEmail"].ToString();
                        Postaltxt.Text = contactDetailsReader["CustPostalCode"].ToString();
                        Addresstxt.Text = contactDetailsReader["CustAddress"].ToString();
                        Phonetxt.Text = contactDetailsReader["CustPhone"].ToString();
                        MobilePhonetxt.Text = contactDetailsReader["CustMobilePhone"].ToString();

                        //Contact Details 2
                        CustomerNametxt1.Text = contactDetailsReader["NameCust2"].ToString();
                        txtICIDNo1.Text = contactDetailsReader["CustIC2"].ToString();
                        txtEmail1.Text = contactDetailsReader["CustEmail2"].ToString();
                        Postaltxt1.Text = contactDetailsReader["CustPostalCode2"].ToString();
                        Addresstxt1.Text = contactDetailsReader["CustAddress2"].ToString();
                        Phonetxt1.Text = contactDetailsReader["CustPhone2"].ToString();
                        MobilePhonetxt1.Text = contactDetailsReader["CustMobilePhone2"].ToString();

                        //Contact Details 3 
                        CustomerNametxt2.Text = contactDetailsReader["NameCust3"].ToString();
                        txtICIDNo2.Text = contactDetailsReader["CustIC3"].ToString();
                        txtEmail2.Text = contactDetailsReader["CustEmail3"].ToString();
                        Postaltxt2.Text = contactDetailsReader["CustPostalCode3"].ToString();
                        Addresstxt2.Text = contactDetailsReader["CustAddress3"].ToString();
                        Phonetxt2.Text = contactDetailsReader["CustPhone3"].ToString();
                        MobilePhonetxt2.Text = contactDetailsReader["CustMobilePhone3"].ToString();
                    }
                }

                string documentChecklistQuery = $"SELECT * FROM newcust_document_checklist WHERE NewCustID = @NewCustID";
                MySqlCommand documentChecklistCommand = new MySqlCommand(documentChecklistQuery, connection);
                documentChecklistCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                using (MySqlDataReader documentChecklistReader = documentChecklistCommand.ExecuteReader())
                {
                    if (documentChecklistReader.Read())
                    {
                        // Populate controls on your form with data from newcust_document_checklist
                        string creditApplicationValue = documentChecklistReader["creditApplication"].ToString();

                        // Split the stored value by comma (assuming that it is stored as a comma-separated string)
                        string[] creditApplicationValues = creditApplicationValue.Split(',');

                        // Check the checkboxes based on the split values
                        creditApplicationFormReceivedByCreditAdmin.Checked = creditApplicationValues.Contains("Received By Credit Admin");
                        creditApplicationFormRequestNewDocument.Checked = creditApplicationValues.Contains("New Document Request");

                        string personalGuaranteeValue = documentChecklistReader["PersonalGuarantor"].ToString();

                        // Split the stored value by comma
                        string[] personalGuaranteeValues = personalGuaranteeValue.Split(',');

                        // Check the checkboxes based on the split values
                        personalGuaranteeReceivedByCreditAdmin.Checked = personalGuaranteeValues.Contains("Received By Credit Admin");
                        personalGuaranteeRequestNewDocument.Checked = personalGuaranteeValues.Contains("New Document Request");

                        string salesmanRecommendationFormValue = documentChecklistReader["RecommendationForm"].ToString();
                        string photocopyICValue = documentChecklistReader["PhotocopyIC"].ToString();

                        // Split the values by comma
                        string[] salesmanRecommendationFormValues = salesmanRecommendationFormValue.Split(',');
                        string[] photocopyICValues = photocopyICValue.Split(',');

                        // Check the checkboxes based on the split values
                        SalesmanRecommmendationFormByCreditAdmin.Checked = salesmanRecommendationFormValues.Contains("Received By Credit Admin");
                        SalesmanRecommmendationFormByNewDocument.Checked = salesmanRecommendationFormValues.Contains("New Document Request");

                        PhotocopyByCreditAdmin.Checked = photocopyICValues.Contains("Received By Credit Admin");
                        PhotocopyByNewDocument.Checked = photocopyICValues.Contains("New Document Request");

                        string declarationFormValue = documentChecklistReader["DeclarationForm"].ToString();
                        string workshopPhotos1Value = documentChecklistReader["FullViewFront"].ToString();

                        // Split the values by comma
                        string[] declarationFormValues = declarationFormValue.Split(',');
                        string[] workshopPhotos1Values = workshopPhotos1Value.Split(',');

                        // Check the checkboxes based on the split values
                        declarationFormReceivedByCreditAdmin.Checked = declarationFormValues.Contains("Received By Credit Admin");
                        declarationFormRequestNewDocument.Checked = declarationFormValues.Contains("New Document Request");

                        workshopPhotos1ReceivedByCreditAdmin.Checked = workshopPhotos1Values.Contains("Received By Credit Admin");
                        workshopPhotos1RequestNewDocument.Checked = workshopPhotos1Values.Contains("New Document Request");

                        string workshopPhotos2Value = documentChecklistReader["FullViewStreet"].ToString();
                        string workshopPhotos3Value = documentChecklistReader["FullViewRight"].ToString();

                        // Split the values by comma
                        string[] workshopPhotos2Values = workshopPhotos2Value.Split(',');
                        string[] workshopPhotos3Values = workshopPhotos3Value.Split(',');

                        // Check the checkboxes based on the split values
                        workshopPhotos2ReceivedByCreditAdmin.Checked = workshopPhotos2Values.Contains("Received By Credit Admin");
                        workshopPhotos2RequestNewDocument.Checked = workshopPhotos2Values.Contains("New Document Request");

                        workshopPhotos3ReceivedByCreditAdmin.Checked = workshopPhotos3Values.Contains("Received By Credit Admin");
                        workshopPhotos3RequestNewDocument.Checked = workshopPhotos3Values.Contains("New Document Request");

                        string workshopPhotos4Value = documentChecklistReader["InsideShop"].ToString();
                        string workshopPhotos5Value = documentChecklistReader["CompanySignBoard"].ToString();

                        // Split the values by comma
                        string[] workshopPhotos4Values = workshopPhotos4Value.Split(',');
                        string[] workshopPhotos5Values = workshopPhotos5Value.Split(',');

                        // Check the checkboxes based on the split values
                        workshopPhotos4ReceivedByCreditAdmin.Checked = workshopPhotos4Values.Contains("Received By Credit Admin");
                        workshopPhotos4RequestNewDocument.Checked = workshopPhotos4Values.Contains("New Document Request");

                        workshopPhotos5ReceivedByCreditAdmin.Checked = workshopPhotos5Values.Contains("Received By Credit Admin");
                        workshopPhotos5RequestNewDocument.Checked = workshopPhotos5Values.Contains("New Document Request");

                        string phoneValue = documentChecklistReader["HandPhoneNumber"].ToString();
                        string emailValue = documentChecklistReader["EmailAddress"].ToString();

                        // Split the values by comma
                        string[] phoneValues = phoneValue.Split(',');
                        string[] emailValues = emailValue.Split(',');

                        // Check the checkboxes based on the split values
                        PhoneReceivedByCreditAdmin.Checked = phoneValues.Contains("Received By Credit Admin");
                        PhoneRequestNewDocument.Checked = phoneValues.Contains("New Document Request");

                        EmailReceivedByCreditAdmin.Checked = emailValues.Contains("Received By Credit Admin");
                        EmailRequestNewDocument.Checked = emailValues.Contains("New Document Request");

                        string registrationCertificateValue = documentChecklistReader["BusinessRegistration"].ToString();
                        string businessLicenseValue = documentChecklistReader["BusinessAnnualLicense"].ToString();

                        // Split the values by comma
                        string[] registrationCertificateValues = registrationCertificateValue.Split(',');
                        string[] businessLicenseValues = businessLicenseValue.Split(',');

                        // Check the checkboxes based on the split values
                        registrationCertificateReceivedByCreditAdmin.Checked = registrationCertificateValues.Contains("Received By Credit Admin");
                        registrationCertificateRequestNewDocument.Checked = registrationCertificateValues.Contains("New Document Request");

                        businessLicenseReceivedByCreditAdmin.Checked = businessLicenseValues.Contains("Received By Credit Admin");
                        businessLicenseRequestNewDocument.Checked = businessLicenseValues.Contains("New Document Request");

                        string financialReportValue = documentChecklistReader["FinancialReport"].ToString();
                        string bankStatementsValue = documentChecklistReader["BankStatement"].ToString();

                        // Split the values by comma
                        string[] financialReportValues = financialReportValue.Split(',');
                        string[] bankStatementsValues = bankStatementsValue.Split(',');

                        // Check the checkboxes based on the split values
                        financialReportReceivedByCreditAdmin.Checked = financialReportValues.Contains("Received By Credit Admin");
                        financialReportRequestNewDocument.Checked = financialReportValues.Contains("New Document Request");

                        bankStatementsReceivedByCreditAdmin.Checked = bankStatementsValues.Contains("Received By Credit Admin");
                        bankStatementsRequestNewDocument.Checked = bankStatementsValues.Contains("New Document Request");
                    }
                }

                //HOD Recommendation
                string hodRecommendationQuery = "SELECT * FROM newcust_hodrecommendation WHERE NewCustID = @NewCustID";
                MySqlCommand hodRecommendationCommand = new MySqlCommand(hodRecommendationQuery, connection);
                hodRecommendationCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                using (MySqlDataReader hodRecommendationReader = hodRecommendationCommand.ExecuteReader())
                {
                    if (hodRecommendationReader.Read())
                    {
                        // Populate controls on your form with data from newcust_hodrecommendation
                        txtRemarksHOD1.Text = hodRecommendationReader["HODRemarks"].ToString();
                        txtRemarksHOD2.Text = hodRecommendationReader["AGMRemarks"].ToString();
                    }
                }

                //Documentation Status
                string documentationStatusQuery = "SELECT * FROM newcust_documentation_status WHERE NewCustID = @NewCustID";
                MySqlCommand documentationStatusCommand = new MySqlCommand(documentationStatusQuery, connection);
                documentationStatusCommand.Parameters.AddWithValue("@NewCustID", newCustID);

                using (MySqlDataReader documentationStatusReader = documentationStatusCommand.ExecuteReader())
                {
                    if (documentationStatusReader.Read())
                    {
                        // Populate controls on your form with data from newcust_documentation_status
                        CTOStxt.Text = documentationStatusReader["CTOS_Status"].ToString();
                        documentRemarksTextBox.Text = documentationStatusReader["DocumentRemarks"].ToString();
                        customerTypeTextBox.Text = documentationStatusReader["CustomerType"].ToString();
                        paidUpCapitalTextBox.Text = documentationStatusReader["PaidUpCapital"].ToString();

                        string documentValue = documentationStatusReader["Document"].ToString();
                        SetCheckboxStatusFromCombinedValue(documentValue);
                    }
                }

                //Customer Account Status
                string custAccStatusQuery = "SELECT * FROM newcust_custaccstatus WHERE NewCustID = @NewCustID;";
                MySqlCommand custAccStatusCommand = new MySqlCommand(custAccStatusQuery, connection);
                custAccStatusCommand.Parameters.AddWithValue("@NewCustID", newCustID);


                using (MySqlDataReader custAccStatusReader = custAccStatusCommand.ExecuteReader())
                {
                    if (custAccStatusReader.Read())
                    {
                        // Populate controls for newcust_custaccstatus
                        statusDropDownList.SelectedValue = custAccStatusReader["Status"].ToString();
                        customerTypeDropDown.SelectedValue = custAccStatusReader["CustomerType"].ToString();
                        warehouseDropDown.SelectedValue = custAccStatusReader["WareHouse"].ToString();
                        creditLimitTextBox.Text = custAccStatusReader["CreditLimit"].ToString();
                        creditTermDropDown.SelectedValue = custAccStatusReader["CreditTerm"].ToString();
                        customerAccountNoTextBox.Text = custAccStatusReader["CustomerAccNo"].ToString();
                        openingDateTextBox.Text = custAccStatusReader["OpeningAccDate"].ToString();
                        customerClassDropDown.SelectedValue = custAccStatusReader["CustomerClass"].ToString();
                        customerMainGroupDropDown.SelectedValue = custAccStatusReader["CustomerMainGroup"].ToString();
                        incorporationDateTextBox.Text = custAccStatusReader["IncorparationDate"].ToString();
                        rocNewTextBox.Text = custAccStatusReader["ROC_New"].ToString();
                        rocRobTextBox.Text = custAccStatusReader["ROC_ROB"].ToString();
                        DropDownListTerritory.SelectedValue = custAccStatusReader["Territory"].ToString();
                        custAccTextBox.Text = custAccStatusReader["CustAcc_Remarks"].ToString();
                        customerTypeDropDownList.SelectedValue = custAccStatusReader["TypeOfCustomer"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            finally
            {
                connection.Close();
            }
        }

        private void SetCheckboxStatusFromCombinedValue(string combinedValue)
        {
            // Split the combined values by comma
            string[] documentValues = combinedValue.Split(',');

            // Check the checkboxes based on the split values
            ctosCheckbox.Checked = documentValues.Contains("CTOS");
            cbmCheckbox.Checked = documentValues.Contains("CBM");
            photoCheckbox.Checked = documentValues.Contains("PHOTO");
            icCheckbox.Checked = documentValues.Contains("IC");
        }

        private void PopulateInventLocationDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get data from table 698
                List<ListItem> inventLocationList = GetAxInventLocationData(DynAx);

                // Bind the data to the DropDownList
                warehouseDropDown.DataSource = inventLocationList;
                warehouseDropDown.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxInventLocationData(Axapta DynAx)
        {
            List<ListItem> inventLocationList = new List<ListItem>();

            int inventLocationTable = 158;
            string inventLocationField = "InventLocationId";
            string nameField = "Name";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", inventLocationTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            inventLocationList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", inventLocationTable);
                string tempInventLocation = dynRec.get_Field(inventLocationField).ToString();
                string tempName = dynRec.get_Field(nameField).ToString();

                // Output to debug for verification
                System.Diagnostics.Debug.WriteLine($"Invent Location: {tempInventLocation}, Name: {tempName}");

                // Concatenate the fields if you want both in the ListItem text
                string listItemText = $"{tempInventLocation} - {tempName}";
                inventLocationList.Add(new ListItem(listItemText, tempInventLocation));

                dynRec.Dispose();
            }

            return inventLocationList;
        }


        private void PopulateTerritoryDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get territory data
                List<ListItem> territoryList = GetAxTerritoryData(DynAx);

                // Bind the data to the Territory DropDownList
                DropDownListTerritory.DataSource = territoryList;
                DropDownListTerritory.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxTerritoryData(Axapta DynAx)
        {
            List<ListItem> territoryList = new List<ListItem>();

            int territoryTable = 30012;
            string territoryField = "TerrDesc";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", territoryTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            territoryList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", territoryTable);
                string tempTerritoryDesc = dynRec.get_Field(territoryField).ToString();

                // Output to debug for verification
                System.Diagnostics.Debug.WriteLine($"Territory Description: {tempTerritoryDesc}");

                territoryList.Add(new ListItem(tempTerritoryDesc));

                dynRec.Dispose();
            }

            return territoryList;
        }


        private void PopulateCustomerTypeDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get customer types
                List<ListItem> customerTypeList = GetAxCustomerTypes(DynAx);

                // Bind the customer types to the DropDownList
                customerTypeDropDownList.DataSource = customerTypeList;
                customerTypeDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxCustomerTypes(Axapta DynAx)
        {
            List<ListItem> customerTypeList = new List<ListItem>();

            int customerTypeTable = 30005;
            string customerTypeField = "TypeDesc";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", customerTypeTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            customerTypeList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", customerTypeTable);
                string tempCustomerTypeDesc = dynRec.get_Field(customerTypeField).ToString();

                // Output to debug for verification
                System.Diagnostics.Debug.WriteLine($"Customer Type Description: {tempCustomerTypeDesc}");

                customerTypeList.Add(new ListItem(tempCustomerTypeDesc));

                dynRec.Dispose();
            }

            return customerTypeList;
        }

        private void PopulateCustomerMainGroupDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get customer main groups
                List<ListItem> customerMainGroupList = GetAxCustomerMainGroups(DynAx);

                // Bind the customer main groups to the DropDownList
                customerMainGroupDropDown.DataSource = customerMainGroupList;
                customerMainGroupDropDown.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxCustomerMainGroups(Axapta DynAx)
        {
            List<ListItem> customerMainGroupList = new List<ListItem>();

            int customerMainGroupTable = 30004;
            string customerMainGroupField = "MainGroupDesc";

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", customerMainGroupTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            customerMainGroupList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord dynRec = (AxaptaRecord)axQueryRun.Call("Get", customerMainGroupTable);
                string tempCustomerMainGroupDesc = dynRec.get_Field(customerMainGroupField).ToString();

                // Output to debug for verification
                System.Diagnostics.Debug.WriteLine($"Customer Main Group Description: {tempCustomerMainGroupDesc}");

                customerMainGroupList.Add(new ListItem(tempCustomerMainGroupDesc));

                dynRec.Dispose();
            }

            return customerMainGroupList;
        }

        private void PopulateCustomerDescriptionDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get customer descriptions
                List<ListItem> customerDescriptionList = get_AxCustomerDescriptions(DynAx);

                // Bind the customer descriptions to the dropdown list
                customerClassDropDown.DataSource = customerDescriptionList;
                customerClassDropDown.DataBind();
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
        }


        public List<ListItem> get_AxCustomerDescriptions(Axapta DynAx)
        {
            List<ListItem> customerDescriptionList = new List<ListItem>();

            // Assuming your customer table in Axapta is named "CustTable"
            int CustTable = 30003; // Replace with the correct table number

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CustTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            customerDescriptionList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CustTable);
                string temp_ClassDesc = DynRec.get_Field("ClassDesc").ToString();

                // Log or output the values for debugging
                System.Diagnostics.Debug.WriteLine($"Class Description: {temp_ClassDesc}");

                // Combine CustomerClass and ClassDesc and add to the list
                string combinedDescription = $" {temp_ClassDesc}";
                customerDescriptionList.Add(new ListItem(combinedDescription));

                // Dispose of the current record
                DynRec.Dispose();
            }

            return customerDescriptionList;
        }

        private void PopulateCreditTermDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get credit terms
                List<ListItem> creditTermList = get_AxCreditTerms(DynAx);

                // Bind the credit terms to the dropdown list
                creditTermDropDown.DataSource = creditTermList;
                creditTermDropDown.DataBind();
            }
            catch (Exception ex)
            {

            }
            finally
            {

                DynAx.Logoff();
            }
        }


        public List<ListItem> get_AxCreditTerms(Axapta DynAx)
        {
            List<ListItem> creditTermList = new List<ListItem>();

            int CreditTermTable = 276;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", CreditTermTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            creditTermList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", CreditTermTable);
                string temp_CreditTermDescription = DynRec.get_Field("Description").ToString();


                System.Diagnostics.Debug.WriteLine($"Credit Term Description: {temp_CreditTermDescription}");

                creditTermList.Add(new ListItem(temp_CreditTermDescription));

                DynRec.Dispose();
            }

            return creditTermList;
        }

        private void PopulateStateIdDropDown()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call the method to get state IDs
                List<ListItem> stateIdList = GetAxStateIds(DynAx);

                // Bind the state IDs to the dropdown list
                ddlState.DataSource = stateIdList;
                ddlState.DataBind();
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
            }
            finally
            {
                DynAx.Logoff();
            }
        }

        public List<ListItem> GetAxStateIds(Axapta DynAx)
        {
            List<ListItem> stateIdList = new List<ListItem>();

            int stateIdTable = 418;

            AxaptaObject axQuery = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource = (AxaptaObject)axQuery.Call("addDataSource", stateIdTable);

            AxaptaObject axQueryRun = DynAx.CreateAxaptaObject("QueryRun", axQuery);

            stateIdList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun.Call("next"))
            {
                AxaptaRecord DynRec = (AxaptaRecord)axQueryRun.Call("Get", stateIdTable);
                string temp_StateId = DynRec.get_Field("StateId").ToString();

                System.Diagnostics.Debug.WriteLine($"State ID: {temp_StateId}");

                stateIdList.Add(new ListItem(temp_StateId));

                DynRec.Dispose();
            }

            return stateIdList;
        }


        private string GetCombinedCheckboxStatus()
        {
            List<string> checkboxStatusList = new List<string>();

            if (ctosCheckbox.Checked)
            {
                checkboxStatusList.Add("CTOS");
            }

            if (cbmCheckbox.Checked)
            {
                checkboxStatusList.Add("CBM");
            }

            if (photoCheckbox.Checked)
            {
                checkboxStatusList.Add("PHOTO");
            }

            if (icCheckbox.Checked)
            {
                checkboxStatusList.Add("IC");
            }

            return string.Join(", ", checkboxStatusList);
        }

        protected void btnSubmit(object sender, EventArgs e)
        {
            if (AreAllFieldsFilled())
            {
                MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
                try
                {
                    string query = @"UPDATE newcust_guarantor SET " +
                                    "Title = @GTitle , NameCust = @GName , CustIC = @GIC, CustAddress = @GAddress, " +
                                    "CustOldIC = @GOldIC, CustType = @GType, CustBirthDate = @GBirthDate, CustPhone = @GPhone," +
                                    "CustMobilePhone = @GMobilePhone  WHERE NewCustID = @NewCustID;" +

                                    "UPDATE newcust_guarantor SET " +
                                    "Title2 = @GTitle2  , NameCust2 = @GName2 , CustIC2 = @GIC2, CustAddress2 = @GAddress2, " +
                                    "CustOldIC2 = @GOldIC2, CustType2 = @GType2, CustBirthDate2 = @GBirthDate2, CustPhone2 = @GPhone2," +
                                    "CustMobilePhone2 = @GMobilePhone2  WHERE NewCustID = @NewCustID;" +

                                    "UPDATE newcust_guarantor SET " +
                                    "Title3 = @GTitle3  , NameCust3 = @GName3 , CustIC3 = @GIC3, CustAddress3 = @GAddress3, " +
                                    "CustOldIC3 = @GOldIC3, CustType3 = @GType3, CustBirthDate3 = @GBirthDate3, CustPhone3 = @GPhone3," +
                                    "CustMobilePhone3 = @GMobilePhone3  WHERE NewCustID = @NewCustID; " +

                                    "UPDATE newcust_contactdetails SET " +
                                    "NameCust = @CDName , CustIC = @CDIC , CustEmail = @CDEmail, CustPostalCode = @CDPostal, " +
                                    "CustAddress = @CDAddress, CustPhone = @CDPhone, CustMobilePhone = @CDMobilePhone " +
                                    "WHERE NewCustID = @NewCustID ;" +

                                    "UPDATE newcust_contactdetails SET " +
                                    "NameCust2 = @CDName2 , CustIC2 = @CDIC2 , CustEmail2 = @CDEmail2, CustPostalCode2 = @CDPostal2, " +
                                    "CustAddress2 = @CDAddress2, CustPhone2 = @CDPhone2, CustMobilePhone2 = @CDMobilePhone2 " +
                                    "WHERE NewCustID = @NewCustID ;" +

                                    "UPDATE newcust_contactdetails SET " +
                                    "NameCust3 = @CDName3 , CustIC3 = @CDIC3 , CustEmail3 = @CDEmail3, CustPostalCode3 = @CDPostal3, " +
                                    "CustAddress3 = @CDAddress3, CustPhone3 = @CDPhone3, CustMobilePhone3 = @CDMobilePhone3 " +
                                    "WHERE NewCustID = @NewCustID ;" +

                                    "UPDATE newcust_document_checklist SET " +
                                    "creditApplication = @CLApplication, PersonalGuarantor = @CLGuarantor , RecommendationForm = @CLRecommendation, " +
                                    "PhotocopyIC = @CLPhotocopy, DeclarationForm = @CLDeclaration, FullViewFront = @CLFront, " +
                                    "FullViewStreet = @CLStreet, FullViewRight = @CLRight, InsideShop = @CLInside, CompanySignBoard = @CLSignBoard, " +
                                    "HandPhoneNumber = @CLHandPhone, EmailAddress = @CLEmail, BusinessRegistration = @CLBusiness, " +
                                    "BusinessAnnualLicense = @CLAnnualLicense, FinancialReport = @CLFinacial, BankStatement = @CLStatement WHERE NewCustID = @NewCustID; " +

                                    "UPDATE newcust_hodrecommendation SET " +
                                    "CreditTerm = @HDCreditTerm, CreditLimit = @HDCreditLimit, HODRemarks = @HOD , AGMRemarks = @AGM " +
                                    "WHERE NewCustID = @NewCustID ;  " +

                                    "UPDATE newcust_documentation_status SET " +
                                    "CTOS_Status = @DS_CTOS_Status, DocumentRemarks = @DSDocumentRemarks , CustomerType = @DSCustType, " +
                                    "PaidUpCapital = @DSPaidUp, Document = @DSDocument " +
                                    "WHERE NewCustID = @NewCustID; " +

                                    "UPDATE newcust_custaccstatus SET " +
                                    "Status = @CAStatus, CustomerType = @CAType, WareHouse = @CAWareHouse, CreditLimit = @CACreditLimit, CreditTerm = @CACreditTerm, " +
                                    "CustomerAccNo = @CACustAccNo, OpeningAccDate = @CAOpeningDate, CustomerClass = @CAClass, CustomerMainGroup = @CACustMainGroup, " +
                                    "IncorparationDate = @CAIncorparationDate, ROC_New = @CAROCNew, ROC_ROB = @CAROCROB, Territory = @CATerritory, CustAcc_Remarks = @CAAccRemarks, " +
                                    "TypeOfCustomer = @CATypeCust WHERE NewCustID = @NewCustID; " +

                                    "UPDATE newcust_details SET " +
                                    "CustTIN = @CTIN, CustPhone = @CPhone, State = @CState WHERE NewCustID = @NewCustID; ";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@NewCustID", HiddenNewCustID.Text.Trim());
                    //Customer Details
                    command.Parameters.AddWithValue("@CTIN", txtTIN.Text);
                    command.Parameters.AddWithValue("@CPhone", txtPhone.Text);
                    command.Parameters.AddWithValue("@CState", ddlState.SelectedValue);
                   
                    //GUARANTOR
                    command.Parameters.AddWithValue("@GTitle", ddlTitle.SelectedValue);
                    command.Parameters.AddWithValue("@GName", txtName.Text);
                    command.Parameters.AddWithValue("@GIC", txtICNo.Text);
                    command.Parameters.AddWithValue("@GAddress", txtAddress.Text);
                    command.Parameters.AddWithValue("@GOldIC", txtOldIC.Text);
                    command.Parameters.AddWithValue("@GType", ddlType.SelectedValue);
                    command.Parameters.AddWithValue("@GBirthDate", txtBirthDate.Text);
                    command.Parameters.AddWithValue("@GPhone", TextBoxPhone1.Text);
                    command.Parameters.AddWithValue("@GMobilePhone", txtMobilePhone1.Text);

                    //GUARANTOR2
                    command.Parameters.AddWithValue("@GTitle2", DropDownList1.SelectedValue);
                    command.Parameters.AddWithValue("@GName2", TextBox1.Text);
                    command.Parameters.AddWithValue("@GIC2", TextBox3.Text);
                    command.Parameters.AddWithValue("@GAddress2", TextBox7.Text);
                    command.Parameters.AddWithValue("@GOldIC2", TextBox5.Text);
                    command.Parameters.AddWithValue("@GType2", DropDownList2.SelectedValue);
                    command.Parameters.AddWithValue("@GBirthDate2", TextBox2.Text);
                    command.Parameters.AddWithValue("@GPhone2", TextBox6.Text);
                    command.Parameters.AddWithValue("@GMobilePhone2", TextBox4.Text);

                    //GUARANTOR3
                    command.Parameters.AddWithValue("@GTitle3", DropDownList3.SelectedValue);
                    command.Parameters.AddWithValue("@GName3", TextBox8.Text);
                    command.Parameters.AddWithValue("@GIC3", TextBox10.Text);
                    command.Parameters.AddWithValue("@GAddress3", TextBox14.Text);
                    command.Parameters.AddWithValue("@GOldIC3", TextBox12.Text);
                    command.Parameters.AddWithValue("@GType3", DropDownList4.SelectedValue);
                    command.Parameters.AddWithValue("@GBirthDate3", TextBox9.Text);
                    command.Parameters.AddWithValue("@GPhone3", TextBox13.Text);
                    command.Parameters.AddWithValue("@GMobilePhone3", TextBox11.Text);

                    //Contact Details
                    command.Parameters.AddWithValue("@CDName", CustomerNametxt.Text);
                    command.Parameters.AddWithValue("@CDIC", txtICIDNo.Text);
                    command.Parameters.AddWithValue("@CDEmail", txtEmail.Text);
                    command.Parameters.AddWithValue("@CDPostal", Postaltxt.Text);
                    command.Parameters.AddWithValue("@CDAddress", Addresstxt.Text);
                    command.Parameters.AddWithValue("@CDPhone", Phonetxt.Text);
                    command.Parameters.AddWithValue("@CDMobilePhone", MobilePhonetxt.Text);

                    //Contact Details2
                    command.Parameters.AddWithValue("@CDName2", CustomerNametxt1.Text);
                    command.Parameters.AddWithValue("@CDIC2", txtICIDNo1.Text);
                    command.Parameters.AddWithValue("@CDEmail2", txtEmail1.Text);
                    command.Parameters.AddWithValue("@CDPostal2", Postaltxt1.Text);
                    command.Parameters.AddWithValue("@CDAddress2", Addresstxt1.Text);
                    command.Parameters.AddWithValue("@CDPhone2", Phonetxt1.Text);
                    command.Parameters.AddWithValue("@CDMobilePhone2", MobilePhonetxt1.Text);

                    //Contact Details3
                    command.Parameters.AddWithValue("@CDName3", CustomerNametxt2.Text);
                    command.Parameters.AddWithValue("@CDIC3", txtICIDNo2.Text);
                    command.Parameters.AddWithValue("@CDEmail3", txtEmail2.Text);
                    command.Parameters.AddWithValue("@CDPostal3", Postaltxt2.Text);
                    command.Parameters.AddWithValue("@CDAddress3", Addresstxt2.Text);
                    command.Parameters.AddWithValue("@CDPhone3", Phonetxt2.Text);
                    command.Parameters.AddWithValue("@CDMobilePhone3", MobilePhonetxt2.Text);

                    //Document CheckList
                    string receivedByCreditAdmin = creditApplicationFormReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string requestNewDocument = creditApplicationFormRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLApplication", $"{receivedByCreditAdmin},{requestNewDocument}");

                    string personalGuaranteeReceivedByCreditAdmin1 = personalGuaranteeReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string personalGuaranteeRequestNewDocument1 = personalGuaranteeRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLGuarantor", $"{personalGuaranteeReceivedByCreditAdmin1},{personalGuaranteeRequestNewDocument1}");

                    string salesmanRecommendationReceivedByCreditAdmin = SalesmanRecommmendationFormByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string salesmanRecommendationRequestNewDocument = SalesmanRecommmendationFormByNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLRecommendation", $"{salesmanRecommendationReceivedByCreditAdmin},{salesmanRecommendationRequestNewDocument}");

                    string photocopyReceivedByCreditAdmin = PhotocopyByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string photocopyRequestNewDocument = PhotocopyByNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLPhotocopy", $"{photocopyReceivedByCreditAdmin},{photocopyRequestNewDocument}");

                    string declarationFormReceivedByCreditAdmin1 = declarationFormReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string declarationFormRequestNewDocument1 = declarationFormRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLDeclaration", $"{declarationFormReceivedByCreditAdmin1},{declarationFormRequestNewDocument1}");

                    string workshopPhotos1ReceivedByCreditAdmin1 = workshopPhotos1ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string workshopPhotos1RequestNewDocument1 = workshopPhotos1RequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLFront", $"{workshopPhotos1ReceivedByCreditAdmin1},{workshopPhotos1RequestNewDocument1}");

                    string workshopPhotos2ReceivedByCreditAdmin1 = workshopPhotos2ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string workshopPhotos2RequestNewDocument1 = workshopPhotos2RequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLStreet", $"{workshopPhotos2ReceivedByCreditAdmin1},{workshopPhotos2RequestNewDocument1}");

                    string workshopPhotos3ReceivedByCreditAdmin1 = workshopPhotos3ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string workshopPhotos3RequestNewDocument1 = workshopPhotos3RequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLRight", $"{workshopPhotos3ReceivedByCreditAdmin1},{workshopPhotos3RequestNewDocument1}");

                    string workshopPhotos4ReceivedByCreditAdmin1 = workshopPhotos4ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string workshopPhotos4RequestNewDocument1 = workshopPhotos4RequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLInside", $"{workshopPhotos4ReceivedByCreditAdmin1}, {workshopPhotos4RequestNewDocument1}");

                    string workshopPhotos5ReceivedByCreditAdmin1 = workshopPhotos5ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string workshopPhotos5RequestNewDocument1 = workshopPhotos5RequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLSignBoard", $"{workshopPhotos5ReceivedByCreditAdmin1},{workshopPhotos5RequestNewDocument1}");

                    string phoneReceivedByCreditAdmin = PhoneReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string phoneRequestNewDocument = PhoneRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLHandPhone", $"{phoneReceivedByCreditAdmin},{phoneRequestNewDocument}");

                    string emailReceivedByCreditAdmin = EmailReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string emailRequestNewDocument = EmailRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLEmail", $"{emailReceivedByCreditAdmin},{emailRequestNewDocument}");

                    string registrationCertificateReceivedByCreditAdmin1 = registrationCertificateReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string registrationCertificateRequestNewDocument1 = registrationCertificateRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLBusiness", $"{registrationCertificateReceivedByCreditAdmin1},{registrationCertificateRequestNewDocument1}");

                    string businessLicenseReceivedByCreditAdmin1 = businessLicenseReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string businessLicenseRequestNewDocument1 = businessLicenseRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLAnnualLicense", $"{businessLicenseReceivedByCreditAdmin1},{businessLicenseRequestNewDocument1}");

                    string financialReportReceivedByCreditAdmin1 = financialReportReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string financialReportRequestNewDocument1 = financialReportRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLFinacial", $"{financialReportReceivedByCreditAdmin1},{financialReportRequestNewDocument1}");

                    string bankStatementsReceivedByCreditAdmin1 = bankStatementsReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                    string bankStatementsRequestNewDocument1 = bankStatementsRequestNewDocument.Checked ? "New Document Request" : "";

                    command.Parameters.AddWithValue("@CLStatement", $"{bankStatementsReceivedByCreditAdmin1},{bankStatementsRequestNewDocument1}");

                    //HOD Recommendation
                    command.Parameters.AddWithValue("@HDCreditTerm", txtCreditTermHOD1.Text);
                    command.Parameters.AddWithValue("@HDCreditLimit", txtCreditLimitHOD1.Text);
                    command.Parameters.AddWithValue("@HOD", txtRemarksHOD1.Text);
                    command.Parameters.AddWithValue("@AGM", txtRemarksHOD2.Text);

                    //Documentation Status
                    command.Parameters.AddWithValue("@DS_CTOS_Status", CTOStxt.Text);
                    command.Parameters.AddWithValue("@DSDocumentRemarks", documentRemarksTextBox.Text);
                    command.Parameters.AddWithValue("@DSCustType", customerTypeTextBox.Text);
                    command.Parameters.AddWithValue("@DSPaidUp", paidUpCapitalTextBox.Text);
                    command.Parameters.AddWithValue("@DSDocument", GetCombinedCheckboxStatus());

                    //Customer Account Status
                    command.Parameters.AddWithValue("@CAStatus", statusDropDownList.SelectedValue);
                    command.Parameters.AddWithValue("@CAType", customerTypeDropDown.SelectedValue);
                    command.Parameters.AddWithValue("@CAWareHouse", warehouseDropDown.SelectedValue);
                    command.Parameters.AddWithValue("@CACreditLimit", creditLimitTextBox.Text);
                    command.Parameters.AddWithValue("@CACreditTerm", creditTermDropDown.SelectedValue);
                    command.Parameters.AddWithValue("@CACustAccNo", customerAccountNoTextBox.Text);
                    command.Parameters.AddWithValue("@CAOpeningDate", openingDateTextBox.Text);
                    command.Parameters.AddWithValue("@CAClass", customerClassDropDown.SelectedValue);
                    command.Parameters.AddWithValue("@CACustMainGroup", customerMainGroupDropDown.SelectedValue);
                    command.Parameters.AddWithValue("@CAIncorparationDate", incorporationDateTextBox.Text);
                    command.Parameters.AddWithValue("@CAROCNew", rocNewTextBox.Text);
                    command.Parameters.AddWithValue("@CAROCROB", rocRobTextBox.Text);
                    command.Parameters.AddWithValue("@CATerritory", DropDownListTerritory.SelectedValue);
                    command.Parameters.AddWithValue("@CAAccRemarks", custAccTextBox.Text);
                    command.Parameters.AddWithValue("@CATypeCust", customerTypeDropDownList.SelectedValue);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    Response.Redirect("NewCustomer.aspx");
                }

                catch (Exception ex)
                {
                    // Handle exceptions
                    Response.Write($"Error: {ex.Message}<br />");
                    Response.Write($"Stack Trace: {ex.StackTrace}<br />");
                    Response.Write($"Inner Exception: {ex.InnerException?.Message}<br />");
                }
            }
            else
            {
                // Display an error message indicating that all required fields must be filled
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "alert('Please Complete The Form For Submit');", true);
            }
        }

        private bool AreAllFieldsFilled()
        {

            //text not null
            if (
                //Salesman Part
                string.IsNullOrWhiteSpace(txtSalesmanName.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(txtSalesmanCode.Text) ||
                string.IsNullOrWhiteSpace(txtFormNo.Text) ||
                string.IsNullOrWhiteSpace(txtSalesmanRemark.Text) ||

                //Customer Information
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerCategory.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerAddress.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtPostalCode.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerClass.Text) ||
                string.IsNullOrWhiteSpace(txtCoordinate.Text) ||
                string.IsNullOrWhiteSpace(txtRocRob.Text) ||
                string.IsNullOrWhiteSpace(txtCreditTerm.Text) ||
                string.IsNullOrWhiteSpace(txtTIN.Text) ||
                string.IsNullOrWhiteSpace(txtCreditlimit.Text) ||
                string.IsNullOrWhiteSpace(txtCustType.Text) 

                //Customer Information - Guarantor
                //string.IsNullOrWhiteSpace(txtName.Text) ||
                //string.IsNullOrWhiteSpace(txtBirthDate.Text) ||
                //string.IsNullOrWhiteSpace(txtICNo.Text) ||
                //string.IsNullOrWhiteSpace(txtMobilePhone1.Text) ||
                //string.IsNullOrWhiteSpace(TextBoxPhone1.Text) ||
                //string.IsNullOrWhiteSpace(txtAddress.Text) ||

                ////Customer Information - Contact Details
                //string.IsNullOrWhiteSpace(CustomerNametxt.Text) ||
                //string.IsNullOrWhiteSpace(MobilePhonetxt.Text) ||
                //string.IsNullOrWhiteSpace(txtICIDNo.Text) ||
                //string.IsNullOrWhiteSpace(Phonetxt.Text) ||
                //string.IsNullOrWhiteSpace(txtEmail.Text) ||
                //string.IsNullOrWhiteSpace(Addresstxt.Text) ||
                //string.IsNullOrWhiteSpace(Postaltxt.Text) 

                //HOD Recommendation 
                //string.IsNullOrWhiteSpace(txtCreditTermHOD1.Text) ||
                //string.IsNullOrWhiteSpace(txtCreditLimitHOD1.Text) ||
                //string.IsNullOrWhiteSpace(txtRemarksHOD1.Text) ||
                //string.IsNullOrWhiteSpace(txtCreditTermHOD2.Text) ||
                //string.IsNullOrWhiteSpace(txtCreditLimitHOD2.Text) ||
                //string.IsNullOrWhiteSpace(txtRemarksHOD2.Text) ||

                //Document Status - Credit Control Status
                //string.IsNullOrWhiteSpace(CTOStxt.Text) ||
                //string.IsNullOrWhiteSpace(documentRemarksTextBox.Text) ||
                //string.IsNullOrWhiteSpace(txtRemarksHOD2.Text) ||
                //string.IsNullOrWhiteSpace(txtRemarksHOD2.Text) ||
                //string.IsNullOrWhiteSpace(txtRemarksHOD2.Text) 


               )
            {
                return false;
            } 

            if (ddlState.SelectedItem.Text == "-- SELECT --")
            {
                return false;
            }       
            
            if (ddlTitle.SelectedItem.Text == "-- SELECT --")
            {
                return false;
            }            
            
            if (ddlType.SelectedItem.Text == "Blank")
            {
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query = @"UPDATE newcust_guarantor SET " +
                                "Title = @GTitle , NameCust = @GName , CustIC = @GIC, CustAddress = @GAddress, " +
                                "CustOldIC = @GOldIC, CustType = @GType, CustBirthDate = @GBirthDate, CustPhone = @GPhone," +
                                "CustMobilePhone = @GMobilePhone  WHERE NewCustID = @NewCustID;" +

                                "UPDATE newcust_guarantor SET " +
                                "Title2 = @GTitle2  , NameCust2 = @GName2 , CustIC2 = @GIC2, CustAddress2 = @GAddress2, " +
                                "CustOldIC2 = @GOldIC2, CustType2 = @GType2, CustBirthDate2 = @GBirthDate2, CustPhone2 = @GPhone2," +
                                "CustMobilePhone2 = @GMobilePhone2  WHERE NewCustID = @NewCustID;" +

                                "UPDATE newcust_guarantor SET " +
                                "Title3 = @GTitle3  , NameCust3 = @GName3 , CustIC3 = @GIC3, CustAddress3 = @GAddress3, " +
                                "CustOldIC3 = @GOldIC3, CustType3 = @GType3, CustBirthDate3 = @GBirthDate3, CustPhone3 = @GPhone3," +
                                "CustMobilePhone3 = @GMobilePhone3  WHERE NewCustID = @NewCustID; " +

                                "UPDATE newcust_contactdetails SET " +
                                "NameCust = @CDName , CustIC = @CDIC , CustEmail = @CDEmail, CustPostalCode = @CDPostal, " +
                                "CustAddress = @CDAddress, CustPhone = @CDPhone, CustMobilePhone = @CDMobilePhone " +
                                "WHERE NewCustID = @NewCustID ;" +

                                "UPDATE newcust_contactdetails SET " +
                                "NameCust2 = @CDName2 , CustIC2 = @CDIC2 , CustEmail2 = @CDEmail2, CustPostalCode2 = @CDPostal2, " +
                                "CustAddress2 = @CDAddress2, CustPhone2 = @CDPhone2, CustMobilePhone2 = @CDMobilePhone2 " +
                                "WHERE NewCustID = @NewCustID ;" +

                                "UPDATE newcust_contactdetails SET " +
                                "NameCust3 = @CDName3 , CustIC3 = @CDIC3 , CustEmail3 = @CDEmail3, CustPostalCode3 = @CDPostal3, " +
                                "CustAddress3 = @CDAddress3, CustPhone3 = @CDPhone3, CustMobilePhone3 = @CDMobilePhone3 " +
                                "WHERE NewCustID = @NewCustID ;" +

                                "UPDATE newcust_document_checklist SET " +
                                "creditApplication = @CLApplication, PersonalGuarantor = @CLGuarantor , RecommendationForm = @CLRecommendation, " +
                                "PhotocopyIC = @CLPhotocopy, DeclarationForm = @CLDeclaration, FullViewFront = @CLFront, " +
                                "FullViewStreet = @CLStreet, FullViewRight = @CLRight, InsideShop = @CLInside, CompanySignBoard = @CLSignBoard, " +
                                "HandPhoneNumber = @CLHandPhone, EmailAddress = @CLEmail, BusinessRegistration = @CLBusiness, " +
                                "BusinessAnnualLicense = @CLAnnualLicense, FinancialReport = @CLFinacial, BankStatement = @CLStatement WHERE NewCustID = @NewCustID; " +

                                "UPDATE newcust_hodrecommendation SET " +
                                "CreditTerm = @HDCreditTerm, CreditLimit = @HDCreditLimit, HODRemarks = @HOD , AGMRemarks = @AGM " +
                                "WHERE NewCustID = @NewCustID ;  " +

                                "UPDATE newcust_documentation_status SET " +
                                "CTOS_Status = @DS_CTOS_Status, DocumentRemarks = @DSDocumentRemarks , CustomerType = @DSCustType, " +
                                "PaidUpCapital = @DSPaidUp, Document = @DSDocument " +
                                "WHERE NewCustID = @NewCustID; " +

                                "UPDATE newcust_custaccstatus SET " +
                                "Status = @CAStatus, CustomerType = @CAType, WareHouse = @CAWareHouse, CreditLimit = @CACreditLimit, CreditTerm = @CACreditTerm, " +
                                "CustomerAccNo = @CACustAccNo, OpeningAccDate = @CAOpeningDate, CustomerClass = @CAClass, CustomerMainGroup = @CACustMainGroup, " +
                                "IncorparationDate = @CAIncorparationDate, ROC_New = @CAROCNew, ROC_ROB = @CAROCROB, Territory = @CATerritory, CustAcc_Remarks = @CAAccRemarks, " +
                                "TypeOfCustomer = @CATypeCust WHERE NewCustID = @NewCustID; " +
   
                                "UPDATE newcust_details SET " +
                                "CustTIN = @CTIN, CustPhone = @CPhone, State = @CState WHERE NewCustID = @NewCustID; ";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@NewCustID", HiddenNewCustID.Text.Trim());
                //Customer Details
                command.Parameters.AddWithValue("@CTIN", txtTIN.Text);
                command.Parameters.AddWithValue("@CPhone", txtPhone.Text);
                command.Parameters.AddWithValue("@CState", ddlState.SelectedValue);
                //GUARANTOR
                command.Parameters.AddWithValue("@GTitle", ddlTitle.SelectedValue);
                command.Parameters.AddWithValue("@GName", txtName.Text);
                command.Parameters.AddWithValue("@GIC", txtICNo.Text);
                command.Parameters.AddWithValue("@GAddress", txtAddress.Text);
                command.Parameters.AddWithValue("@GOldIC", txtOldIC.Text);
                command.Parameters.AddWithValue("@GType", ddlType.SelectedValue);
                command.Parameters.AddWithValue("@GBirthDate", txtBirthDate.Text);
                command.Parameters.AddWithValue("@GPhone", TextBoxPhone1.Text);
                command.Parameters.AddWithValue("@GMobilePhone", txtMobilePhone1.Text);

                //GUARANTOR2
                command.Parameters.AddWithValue("@GTitle2", DropDownList1.SelectedValue);
                command.Parameters.AddWithValue("@GName2", TextBox1.Text);
                command.Parameters.AddWithValue("@GIC2", TextBox3.Text);
                command.Parameters.AddWithValue("@GAddress2", TextBox7.Text);
                command.Parameters.AddWithValue("@GOldIC2", TextBox5.Text);
                command.Parameters.AddWithValue("@GType2", DropDownList2.SelectedValue);
                command.Parameters.AddWithValue("@GBirthDate2", TextBox2.Text);
                command.Parameters.AddWithValue("@GPhone2", TextBox6.Text);
                command.Parameters.AddWithValue("@GMobilePhone2", TextBox4.Text);

                //GUARANTOR3
                command.Parameters.AddWithValue("@GTitle3", DropDownList3.SelectedValue);
                command.Parameters.AddWithValue("@GName3", TextBox8.Text);
                command.Parameters.AddWithValue("@GIC3", TextBox10.Text);
                command.Parameters.AddWithValue("@GAddress3", TextBox14.Text);
                command.Parameters.AddWithValue("@GOldIC3", TextBox12.Text);
                command.Parameters.AddWithValue("@GType3", DropDownList4.SelectedValue);
                command.Parameters.AddWithValue("@GBirthDate3", TextBox9.Text);
                command.Parameters.AddWithValue("@GPhone3", TextBox13.Text);
                command.Parameters.AddWithValue("@GMobilePhone3", TextBox11.Text);

                //Contact Details
                command.Parameters.AddWithValue("@CDName", CustomerNametxt.Text);
                command.Parameters.AddWithValue("@CDIC", txtICIDNo.Text);
                command.Parameters.AddWithValue("@CDEmail", txtEmail.Text);
                command.Parameters.AddWithValue("@CDPostal", Postaltxt.Text);
                command.Parameters.AddWithValue("@CDAddress", Addresstxt.Text);
                command.Parameters.AddWithValue("@CDPhone", Phonetxt.Text);
                command.Parameters.AddWithValue("@CDMobilePhone", MobilePhonetxt.Text);

                //Contact Details2
                command.Parameters.AddWithValue("@CDName2", CustomerNametxt1.Text);
                command.Parameters.AddWithValue("@CDIC2", txtICIDNo1.Text);
                command.Parameters.AddWithValue("@CDEmail2", txtEmail1.Text);
                command.Parameters.AddWithValue("@CDPostal2", Postaltxt1.Text);
                command.Parameters.AddWithValue("@CDAddress2", Addresstxt1.Text);
                command.Parameters.AddWithValue("@CDPhone2", Phonetxt1.Text);
                command.Parameters.AddWithValue("@CDMobilePhone2", MobilePhonetxt1.Text);

                //Contact Details3
                command.Parameters.AddWithValue("@CDName3", CustomerNametxt2.Text);
                command.Parameters.AddWithValue("@CDIC3", txtICIDNo2.Text);
                command.Parameters.AddWithValue("@CDEmail3", txtEmail2.Text);
                command.Parameters.AddWithValue("@CDPostal3", Postaltxt2.Text);
                command.Parameters.AddWithValue("@CDAddress3", Addresstxt2.Text);
                command.Parameters.AddWithValue("@CDPhone3", Phonetxt2.Text);
                command.Parameters.AddWithValue("@CDMobilePhone3", MobilePhonetxt2.Text);

                //Document CheckList
                string receivedByCreditAdmin = creditApplicationFormReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string requestNewDocument = creditApplicationFormRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLApplication", $"{receivedByCreditAdmin},{requestNewDocument}");

                string personalGuaranteeReceivedByCreditAdmin1 = personalGuaranteeReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string personalGuaranteeRequestNewDocument1 = personalGuaranteeRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLGuarantor", $"{personalGuaranteeReceivedByCreditAdmin1},{personalGuaranteeRequestNewDocument1}");

                string salesmanRecommendationReceivedByCreditAdmin = SalesmanRecommmendationFormByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string salesmanRecommendationRequestNewDocument = SalesmanRecommmendationFormByNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLRecommendation", $"{salesmanRecommendationReceivedByCreditAdmin},{salesmanRecommendationRequestNewDocument}");

                string photocopyReceivedByCreditAdmin = PhotocopyByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string photocopyRequestNewDocument = PhotocopyByNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLPhotocopy", $"{photocopyReceivedByCreditAdmin},{photocopyRequestNewDocument}");

                string declarationFormReceivedByCreditAdmin1 = declarationFormReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string declarationFormRequestNewDocument1 = declarationFormRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLDeclaration", $"{declarationFormReceivedByCreditAdmin1},{declarationFormRequestNewDocument1}");

                string workshopPhotos1ReceivedByCreditAdmin1 = workshopPhotos1ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string workshopPhotos1RequestNewDocument1 = workshopPhotos1RequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLFront", $"{workshopPhotos1ReceivedByCreditAdmin1},{workshopPhotos1RequestNewDocument1}");

                string workshopPhotos2ReceivedByCreditAdmin1 = workshopPhotos2ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string workshopPhotos2RequestNewDocument1 = workshopPhotos2RequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLStreet", $"{workshopPhotos2ReceivedByCreditAdmin1},{workshopPhotos2RequestNewDocument1}");

                string workshopPhotos3ReceivedByCreditAdmin1 = workshopPhotos3ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string workshopPhotos3RequestNewDocument1 = workshopPhotos3RequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLRight", $"{workshopPhotos3ReceivedByCreditAdmin1},{workshopPhotos3RequestNewDocument1}");

                string workshopPhotos4ReceivedByCreditAdmin1 = workshopPhotos4ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string workshopPhotos4RequestNewDocument1 = workshopPhotos4RequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLInside", $"{workshopPhotos4ReceivedByCreditAdmin1}, {workshopPhotos4RequestNewDocument1}");

                string workshopPhotos5ReceivedByCreditAdmin1 = workshopPhotos5ReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string workshopPhotos5RequestNewDocument1 = workshopPhotos5RequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLSignBoard", $"{workshopPhotos5ReceivedByCreditAdmin1},{workshopPhotos5RequestNewDocument1}");

                string phoneReceivedByCreditAdmin = PhoneReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string phoneRequestNewDocument = PhoneRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLHandPhone", $"{phoneReceivedByCreditAdmin},{phoneRequestNewDocument}");

                string emailReceivedByCreditAdmin = EmailReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string emailRequestNewDocument = EmailRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLEmail", $"{emailReceivedByCreditAdmin},{emailRequestNewDocument}");

                string registrationCertificateReceivedByCreditAdmin1 = registrationCertificateReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string registrationCertificateRequestNewDocument1 = registrationCertificateRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLBusiness", $"{registrationCertificateReceivedByCreditAdmin1},{registrationCertificateRequestNewDocument1}");

                string businessLicenseReceivedByCreditAdmin1 = businessLicenseReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string businessLicenseRequestNewDocument1 = businessLicenseRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLAnnualLicense", $"{businessLicenseReceivedByCreditAdmin1},{businessLicenseRequestNewDocument1}");

                string financialReportReceivedByCreditAdmin1 = financialReportReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string financialReportRequestNewDocument1 = financialReportRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLFinacial", $"{financialReportReceivedByCreditAdmin1},{financialReportRequestNewDocument1}");

                string bankStatementsReceivedByCreditAdmin1 = bankStatementsReceivedByCreditAdmin.Checked ? "Received By Credit Admin" : "";
                string bankStatementsRequestNewDocument1 = bankStatementsRequestNewDocument.Checked ? "New Document Request" : "";

                command.Parameters.AddWithValue("@CLStatement", $"{bankStatementsReceivedByCreditAdmin1},{bankStatementsRequestNewDocument1}");

                //HOD Recommendation
                command.Parameters.AddWithValue("@HDCreditTerm", txtCreditTermHOD1.Text);
                command.Parameters.AddWithValue("@HDCreditLimit", txtCreditLimitHOD1.Text);
                command.Parameters.AddWithValue("@HOD", txtRemarksHOD1.Text);
                command.Parameters.AddWithValue("@AGM", txtRemarksHOD2.Text);

                //Documentation Status
                command.Parameters.AddWithValue("@DS_CTOS_Status", CTOStxt.Text);
                command.Parameters.AddWithValue("@DSDocumentRemarks", documentRemarksTextBox.Text);
                command.Parameters.AddWithValue("@DSCustType", customerTypeTextBox.Text);
                command.Parameters.AddWithValue("@DSPaidUp", paidUpCapitalTextBox.Text);
                command.Parameters.AddWithValue("@DSDocument", GetCombinedCheckboxStatus());

                //Customer Account Status
                command.Parameters.AddWithValue("@CAStatus", statusDropDownList.SelectedValue);
                command.Parameters.AddWithValue("@CAType", customerTypeDropDown.SelectedValue);
                command.Parameters.AddWithValue("@CAWareHouse", warehouseDropDown.SelectedValue);
                command.Parameters.AddWithValue("@CACreditLimit", creditLimitTextBox.Text);
                command.Parameters.AddWithValue("@CACreditTerm", creditTermDropDown.SelectedValue);
                command.Parameters.AddWithValue("@CACustAccNo", customerAccountNoTextBox.Text);
                command.Parameters.AddWithValue("@CAOpeningDate", openingDateTextBox.Text);
                command.Parameters.AddWithValue("@CAClass", customerClassDropDown.SelectedValue);
                command.Parameters.AddWithValue("@CACustMainGroup", customerMainGroupDropDown.SelectedValue);
                command.Parameters.AddWithValue("@CAIncorparationDate", incorporationDateTextBox.Text);
                command.Parameters.AddWithValue("@CAROCNew", rocNewTextBox.Text);
                command.Parameters.AddWithValue("@CAROCROB", rocRobTextBox.Text);
                command.Parameters.AddWithValue("@CATerritory", DropDownListTerritory.SelectedValue);
                command.Parameters.AddWithValue("@CAAccRemarks", custAccTextBox.Text);
                command.Parameters.AddWithValue("@CATypeCust", customerTypeDropDownList.SelectedValue);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                Response.Redirect("NewCustomer.aspx");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write($"Error: {ex.Message}<br />");
                Response.Write($"Stack Trace: {ex.StackTrace}<br />");
                Response.Write($"Inner Exception: {ex.InnerException?.Message}<br />");
            }
        }
    }
}