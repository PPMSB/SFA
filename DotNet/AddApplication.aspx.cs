using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DotNet
{
    public partial class AddApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string salesmanName = Session["SalesmanName"] as string;
                    string status = Session["Status"] as string;
                    string salesmanCode = Session["SalesmanCode"] as string;
                    string formNo = Session["FormNo"] as string;
                    string coordinate = Session["Coordinate"] as string;
                    string custName = Session["CustName"] as string;
                    string newCustID = Session["NewCustID"] as string;


                    // ... (rest of your code)

                    salesmanNameTextBox.Text = salesmanName;
                    txtStatus.Text = status;
                    salesmanCodeTextBox.Text = salesmanCode;
                    txtFormNo.Text = formNo;
                    txtCoordinate.Text = coordinate;
                    txtCustName.Text = custName;

                    check_session();
                    TimeOutRedirect();
                    PopulateCustomerDescriptionDropDown();
                    PopulateCreditTermDropDown();

                    if (!string.IsNullOrEmpty(Request.QueryString["NewCustID"]))
                    {
                        string newCustIDFromQueryString = Request.QueryString["NewCustID"];
                        System.Diagnostics.Debug.WriteLine($"NewCustIDFromQueryString: {newCustIDFromQueryString}");

                        // Call LoadDataForDraft with the retrieved newCustID
                        LoadDataForDraft(newCustIDFromQueryString);
                    }

                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    System.Diagnostics.Debug.WriteLine($"Error in Page_Load: {ex.Message}");
                }
            }
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
                        salesmanNameTextBox.Text = detailsReader["SalesmanName"].ToString();
                        HiddenNewCustID.Text = detailsReader["NewCustID"].ToString();
                        txtStatus.Text = detailsReader["status"].ToString();
                        salesmanCodeTextBox.Text = detailsReader["EmplID"].ToString();
                        txtFormNo.Text = detailsReader["FormNo"].ToString();
                        txtSalesmansMark.Text = detailsReader["SalesmanMark"].ToString();
                        txtCustName.Text = detailsReader["CustName"].ToString();
                        customerAddress.Text = detailsReader["CustAddress"].ToString();
                        province.Text = detailsReader["CustProvince"].ToString();
                        postalCode.Text = detailsReader["CustPostal"].ToString();
                        city.Text = detailsReader["CustCity"].ToString();
                        rocRob.Text = detailsReader["CustRegister"].ToString();
                        creditLimit.Text = detailsReader["CreditLimit"].ToString();
                        txtCoordinate.Text = detailsReader["Coordinate"].ToString();
                        DropDownListCreditTerm.SelectedValue = detailsReader["CreditTerm"].ToString();
                        DropDownListCustomerDescription.SelectedValue = detailsReader["CustClass"].ToString();


                        bool isDiscountChecked = Convert.ToInt32(detailsReader["Discount"]) == 1;
                        chkDiscount.Checked = isDiscountChecked;

                        string customerType = detailsReader["CustType"].ToString();
                        ListItem selectedItem = customerTypeRadioButtonList.Items.FindByText(customerType);

                        if (selectedItem != null)
                        {
                            selectedItem.Selected = true;
                        }

                        string selectedCategory = detailsReader["CustCategory"].ToString();
                        ListItem selectedItem1 = CustCategoryDropDownList.Items.FindByText(selectedCategory);

                        if (selectedItem1 != null)
                        {
                            selectedItem1.Selected = true;
                        }

                        bool isVpppYes = Convert.ToInt32(detailsReader["CustVPPP"]) == 1;
                        vpppYes.Checked = isVpppYes;
                        vpppNo.Checked = !isVpppYes; // Set vpppNo to checked if isVpppYes is false

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
                        txtCustomerName.Text = declarationReader["Name"].ToString();
                        CustomerYearsKnown.Text = declarationReader["CustYearsKnown"].ToString();
                        txtIntroducedBy.Text = declarationReader["CustRecommender"].ToString();
                        txtCompanyName.Text = declarationReader["RecommenderComp"].ToString();
                        txtWorkTitle.Text = declarationReader["CustWork"].ToString();
                        txtWorkYears.Text = declarationReader["CustYears"].ToString();
                        txtWorkCompany.Text = declarationReader["CustComp"].ToString();
                        txtReasonForLeaving.Text = declarationReader["CustLeaving"].ToString();
                        txtDaysOpenPerWeek.Text = declarationReader["CustWorkDays"].ToString();

                        txtHobbiesInterests.Text = declarationReader["CustHobbies"].ToString();
                        txtRentalInstallment.Text = declarationReader["ShopRental"].ToString();
                        txtOwnsHouseNo.Text = declarationReader["CustOwnNo"].ToString();
                        txtOwnsHouseType.Text = declarationReader["CustOwnType"].ToString();
                        txtOwnsHouseLocation.Text = declarationReader["CustOwnLocation"].ToString();
                        txtFinacialBy.Text = declarationReader["CustFinacial"].ToString();
                        txtOtherAssets.Text = declarationReader["CustAssets"].ToString();
                        txtPersonName.Text = declarationReader["NameOfCust"].ToString();


                        //radiobutton part
                        bool isPersonallyYes = Convert.ToInt32(declarationReader["CustPersonally"]) == 1;
                        radYes.Checked = isPersonallyYes;
                        radNo.Checked = !isPersonallyYes;

                        bool isWorkedInOtherTradeYes = Convert.ToInt32(declarationReader["CustOtherTrade"]) == 1;
                        radWorkedInOtherTradeYes.Checked = isWorkedInOtherTradeYes;
                        radWorkedInOtherTradeNo.Checked = !isWorkedInOtherTradeYes;

                        bool isStockMarketYes = Convert.ToInt32(declarationReader["CustStockMarket"]) == 1;
                        radStockMarketYes.Checked = isStockMarketYes;
                        radStockMarketNo.Checked = !isStockMarketYes;

                        bool isBorrowingGamblingYes = Convert.ToInt32(declarationReader["CustBorrowing"]) == 1;
                        radBorrowingGamblingYes.Checked = isBorrowingGamblingYes;
                        radBorrowingGamblingNo.Checked = !isBorrowingGamblingYes;

                        bool isPartnersGetAlongWellYes = Convert.ToInt32(declarationReader["CustPartners"]) == 1;
                        radPartnersGetAlongWellYes.Checked = isPartnersGetAlongWellYes;
                        radPartnersGetAlongWellNo.Checked = !isPartnersGetAlongWellYes;

                        bool isMaritalProblemsYes = Convert.ToInt32(declarationReader["CustMarital"]) == 1;
                        radMaritalProblemsYes.Checked = isMaritalProblemsYes;
                        radMaritalProblemsNo.Checked = !isMaritalProblemsYes;

                        bool isKeepPromisesYes = Convert.ToInt32(declarationReader["CustPromise"]) == 1;
                        radKeepPromisesYes.Checked = isKeepPromisesYes;
                        radKeepPromisesNo.Checked = !isKeepPromisesYes;

                        bool isChequesDishonoredYes = Convert.ToInt32(declarationReader["CustCheque"]) == 1;
                        radChequesDishonoredYes.Checked = isChequesDishonoredYes;
                        radChequesDishonoredNo.Checked = !isChequesDishonoredYes;

                        bool isMainRoadYes = Convert.ToInt32(declarationReader["ShopMainRoad"]) == 1;
                        radMainRoadYes.Checked = isMainRoadYes;
                        radMainRoadNo.Checked = !isMainRoadYes;

                        bool isDifficultToFindYes = Convert.ToInt32(declarationReader["ShopHardFind"]) == 1;
                        radDifficultToFindYes.Checked = isDifficultToFindYes;
                        radDifficultToFindNo.Checked = !isDifficultToFindYes;

                        bool isGoodsMovingFastYes = Convert.ToInt32(declarationReader["ShopMoving"]) == 1;
                        radGoodsMovingFastYes.Checked = isGoodsMovingFastYes;
                        radGoodsMovingFastNo.Checked = !isGoodsMovingFastYes;

                        bool isBelongsToCustomerYes = Convert.ToInt32(declarationReader["ShopBelong"]) == 1;
                        radBelongsToCustomerYes.Checked = isBelongsToCustomerYes;
                        radBelongsToCustomerNo.Checked = !isBelongsToCustomerYes;

                        //checkbox part
                        string selectedRelationship = declarationReader["CustRelationShip"].ToString();
                        switch (selectedRelationship)
                        {
                            case "Classmate":
                                chkClassmate.Checked = true;
                                break;
                            case "Neighbor":
                                chkNeighbor.Checked = true;
                                break;
                            case "Personal Friend":
                                chkPersonalFriend.Checked = true;
                                break;
                            case "Others":
                                chkOthers.Checked = true;
                                break;

                        }

                        string selectedCheckboxes = declarationReader["CustFind"].ToString();

                        // Split the comma-separated string into individual values
                        string[] checkboxValues = selectedCheckboxes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Check the appropriate checkboxes based on the retrieved values
                        foreach (string checkboxValue in checkboxValues)
                        {
                            switch (checkboxValue.Trim())
                            {
                                case "Aggressive":
                                    chkAggressive.Checked = true;
                                    break;
                                case "Organized":
                                    chkOrganized.Checked = true;
                                    break;
                                case "Unable to express himself":
                                    chkUnableToExpress.Checked = true;
                                    break;
                                case "Capable":
                                    chkCapable.Checked = true;
                                    break;
                                case "Quiet":
                                    chkQuiet.Checked = true;
                                    break;
                                case "Unorganized":
                                    chkUnorganized.Checked = true;
                                    break;
                                case "Eloquent":
                                    chkEloquent.Checked = true;
                                    break;
                                case "Skillful":
                                    chkSkillful.Checked = true;
                                    break;
                                case "Unskillful":
                                    chkUnskillful.Checked = true;
                                    break;
                            }
                        }

                        string shopBusyValue = declarationReader["ShopBusy"].ToString();

                        // Check the appropriate checkbox based on the retrieved value
                        switch (shopBusyValue.Trim())
                        {
                            case "Always":
                                chkAlwaysBusy.Checked = true;
                                break;
                            case "Usually":
                                chkUsuallyBusy.Checked = true;
                                break;
                            case "Sometimes":
                                chkSometimesBusy.Checked = true;
                                break;
                            case "Never":
                                chkNeverBusy.Checked = true;
                                break;
                                // Add additional cases as needed
                        }

                        string considerationValue = declarationReader["OtherComments"].ToString();

                        // Check the appropriate radio button based on the retrieved value
                        switch (considerationValue.Trim())
                        {
                            case "Very Good":
                                rdoVeryGood.Checked = true;
                                break;
                            case "Good":
                                rdoGood.Checked = true;
                                break;
                            case "Average":
                                rdoAverage.Checked = true;
                                break;
                                // Add additional cases as needed
                        }

                        string workHoursAM = declarationReader["CustWorkHoursAM"].ToString();
                        string workHoursPM = declarationReader["CustWorkHoursPM"].ToString();

                        TimeSpan startTimeAM = TimeSpan.Parse(workHoursAM);
                        TimeSpan endTimePM = TimeSpan.Parse(workHoursPM);

                        txtStartHourAM.Value = startTimeAM.ToString("hh\\:mm");
                        txtEndHourPM.Value = endTimePM.ToString("hh\\:mm");

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
                DropDownListCustomerDescription.DataSource = customerDescriptionList;
                DropDownListCustomerDescription.DataBind();
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
                DropDownListCreditTerm.DataSource = creditTermList;
                DropDownListCreditTerm.DataBind();
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

        private string GetSelectedRelationship()
        {
            if (chkClassmate.Checked)
                return chkClassmate.Text;

            if (chkNeighbor.Checked)
                return chkNeighbor.Text;

            if (chkPersonalFriend.Checked)
                return chkPersonalFriend.Text;

            if (chkOthers.Checked)
                return chkOthers.Text;

            return string.Empty; 
        }

        private string GetSelectedCheckboxes()
        {
            List<string> selectedValues = new List<string>();

            if (chkAggressive.Checked) selectedValues.Add(chkAggressive.Text);
            if (chkOrganized.Checked) selectedValues.Add(chkOrganized.Text);
            if (chkUnableToExpress.Checked) selectedValues.Add(chkUnableToExpress.Text);
            if (chkCapable.Checked) selectedValues.Add(chkCapable.Text);
            if (chkQuiet.Checked) selectedValues.Add(chkQuiet.Text);
            if (chkUnorganized.Checked) selectedValues.Add(chkUnorganized.Text);
            if (chkEloquent.Checked) selectedValues.Add(chkEloquent.Text);
            if (chkSkillful.Checked) selectedValues.Add(chkSkillful.Text);
            if (chkUnskillful.Checked) selectedValues.Add(chkUnskillful.Text);

            return string.Join(", ", selectedValues);
        }

        private string GetSelectedShopBusyValue()
        {
            if (chkAlwaysBusy.Checked) return chkAlwaysBusy.Text;
            if (chkUsuallyBusy.Checked) return chkUsuallyBusy.Text;
            if (chkSometimesBusy.Checked) return chkSometimesBusy.Text;
            if (chkNeverBusy.Checked) return chkNeverBusy.Text;

            return string.Empty;
        }

        private string GetSelectedConsideration()
        {
            if (rdoVeryGood.Checked)
            {
                return "Very Good";
            }
            else if (rdoGood.Checked)
            {
                return "Good";
            }
            else if (rdoAverage.Checked)
            {
                return "Average";
            }
            else
            {
                return string.Empty; 
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (AreAllFieldsFilled())
            {
                MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
                try
                {
                    string query = @"UPDATE newcust_details SET " +
                    "SalesmanName = @SalesName, SalesmanMark = @SalesMark, status = 'NEW', " +
                   " FormNo = @FN, CustCategory = @CCategory, CustAddress = @CAddress, " +
                   " CustProvince = @CProvince, CustPostal = @CPostal, CustCity = @CCity, " +
                   " CustTerritory = @CTerritory, CustRegister = @CRegis, CustClass = @CClass, " +
                   " CreditTerm = @CTerm, Discount = @Disc, CreditLimit = @CLimit, " +
                   " CustVPPP = @CVPPP, CustType = @CType " +
                   " WHERE NewCustID = @NewCustID ;" +

                    "UPDATE newcust_salesmandeclaration SET " +
                     "Name = @SDName, CustPersonally = @SDPersonally, " +
                     "CustYearsKnown = @SDYearsKnown, CustRelationShip = @SDRelationShip, " +
                     "CustRecommender = @SDRecommender, RecommenderComp = @SDCompany, " +
                     "CustWork = @SDWork, CustYears = @SDYears, CustComp = @SDCustCompany, " +
                     "CustLeaving = @SDLeaving, CustOtherTrade = @SDTrade, CustWorkDays = @SDDays, " +
                     "CustBorrowing = @SDBorrowing, CustPartners = @SDPartners, " +
                     "CustMarital = @SDMarital, CustPromise = @SDPromise, CustCheque = @SDCheque, CustStockMarket = @SDStockMarket, " +
                     "CustFind = @SDFind, CustHobbies = @SDHobbies, ShopMainRoad = @SDMainRoad, " +
                     "ShopHardFind = @SDHardFind, ShopBusy = @SDBusy, ShopMoving = @SDMoving, " +
                     "ShopBelong = @SDBelong, ShopRental = @SDRental, CustOwnNo = @SDOwnNo, " +
                     "CustOwnType = @SDOwnType, CustOwnLocation = @SDOwnLocation, " +
                     "CustFinacial = @SDFinacial, CustAssets = @SDAssests, OtherComments = @SDComments," +
                     "CustWorkHoursAM = @SDHoursAM, CustWorkHoursPM = @SDHoursPM, NameOfCust = @SDNameCust " +
                     "WHERE NewCustID = @NewCustID; " +


                    "UPDATE newcust_guarantor SET " +
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
                    "WHERE NewCustID = @NewCustID ;";



                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@NewCustID", HiddenNewCustID.Text.Trim());
                    command.Parameters.AddWithValue("@SalesName", salesmanNameTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@CustName", txtCustName.Text.Trim());
                    command.Parameters.AddWithValue("@SalesMark", txtSalesmansMark.Text.Trim());
                    command.Parameters.AddWithValue("@FN", txtFormNo.Text.Trim());
                    command.Parameters.AddWithValue("@CAddress", customerAddress.Text.Trim());
                    command.Parameters.AddWithValue("@CProvince", province.Text.Trim());
                    command.Parameters.AddWithValue("@CPostal", postalCode.Text.Trim());
                    command.Parameters.AddWithValue("@CCity", city.Text.Trim());
                    command.Parameters.AddWithValue("@CRegis", rocRob.Text.Trim());
                    command.Parameters.AddWithValue("@CClass", DropDownListCustomerDescription.SelectedValue);
                    command.Parameters.AddWithValue("@CTerm", DropDownListCreditTerm.SelectedValue);
                    command.Parameters.AddWithValue("@CLimit", creditLimit.Text.Trim());
                    command.Parameters.AddWithValue("@CVPPP", vpppYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@CTerritory", territory.Text.Trim());
                    command.Parameters.AddWithValue("@Disc", chkDiscount.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@CCategory", CustCategoryDropDownList.SelectedItem.Text);
                    command.Parameters.AddWithValue("@SDName", txtCustomerName.Text.Trim());
                    command.Parameters.AddWithValue("@SDPersonally", radYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDYearsKnown", CustomerYearsKnown.Text.Trim());
                    command.Parameters.AddWithValue("@SDRecommender", txtIntroducedBy.Text.Trim());
                    command.Parameters.AddWithValue("@SDCompany", txtCompanyName.Text.Trim());
                    command.Parameters.AddWithValue("@SDRelationShip", GetSelectedRelationship());
                    command.Parameters.AddWithValue("@SDWork", txtWorkTitle.Text.Trim());
                    command.Parameters.AddWithValue("@SDYears", txtWorkYears.Text.Trim());
                    command.Parameters.AddWithValue("@SDCustCompany", txtWorkCompany.Text.Trim());
                    command.Parameters.AddWithValue("@SDLeaving", txtReasonForLeaving.Text.Trim());
                    command.Parameters.AddWithValue("@SDTrade", radWorkedInOtherTradeYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDDays", txtDaysOpenPerWeek.Text.Trim());
                    command.Parameters.AddWithValue("@SDStockMarket", radStockMarketYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDBorrowing", radBorrowingGamblingYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDPartners", radPartnersGetAlongWellYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDMarital", radMaritalProblemsYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDPromise", radKeepPromisesYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDCheque", radChequesDishonoredYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDHobbies", txtHobbiesInterests.Text.Trim());
                    command.Parameters.AddWithValue("@SDMainRoad", radMainRoadYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDHardFind", radDifficultToFindYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDMoving", radGoodsMovingFastYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDBelong", radBelongsToCustomerYes.Checked ? 1 : 0);
                    command.Parameters.AddWithValue("@SDRental", txtRentalInstallment.Text.Trim());
                    command.Parameters.AddWithValue("@SDOwnNo", txtOwnsHouseNo.Text.Trim());
                    command.Parameters.AddWithValue("@SDOwnType", txtOwnsHouseType.Text.Trim());
                    command.Parameters.AddWithValue("@SDOwnLocation", txtOwnsHouseLocation.Text.Trim());
                    command.Parameters.AddWithValue("@SDFinacial", txtFinacialBy.Text.Trim());
                    command.Parameters.AddWithValue("@SDAssests", txtOtherAssets.Text.Trim());
                    command.Parameters.AddWithValue("@SDNameCust", txtPersonName.Text.Trim());


                    string considerationValue = GetSelectedConsideration();
                    command.Parameters.AddWithValue("@SDComments", considerationValue);


                    string shopBusyValue = GetSelectedShopBusyValue();
                    command.Parameters.AddWithValue("@SDBusy", shopBusyValue);

                    string selectedCheckboxes = GetSelectedCheckboxes();
                    command.Parameters.AddWithValue("@SDFind", selectedCheckboxes);
                    command.Parameters.AddWithValue("@CType", customerTypeRadioButtonList.SelectedItem.Text);

                    command.Parameters.AddWithValue("@SDHoursAM", txtStartHourAM.Value);
                    command.Parameters.AddWithValue("@SDHoursPM", txtEndHourPM.Value);

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

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();


                    Response.Redirect("NewCustomer.aspx");
                }
                catch (Exception ex)
                {
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

        private bool IsAtLeastOneCheckboxChecked(params CheckBox[] checkboxes)
        {
            foreach (CheckBox checkbox in checkboxes)
            {
                if (checkbox.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        private bool AreAllFieldsFilled()
        {

            //text not null
            if (string.IsNullOrWhiteSpace(txtCustName.Text) ||
                string.IsNullOrWhiteSpace(salesmanNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(txtFormNo.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                string.IsNullOrWhiteSpace(salesmanCodeTextBox.Text) ||
                string.IsNullOrWhiteSpace(txtSalesmansMark.Text) ||
                string.IsNullOrWhiteSpace(txtCustName.Text) ||
                string.IsNullOrWhiteSpace(customerAddress.Text) ||
                string.IsNullOrWhiteSpace(province.Text) ||
                string.IsNullOrWhiteSpace(postalCode.Text) ||
                string.IsNullOrWhiteSpace(city.Text) ||
                string.IsNullOrWhiteSpace(rocRob.Text) ||
                string.IsNullOrWhiteSpace(txtCoordinate.Text) ||
                //string.IsNullOrWhiteSpace(territory.Text) ||
                string.IsNullOrWhiteSpace(creditLimit.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(CustomerYearsKnown.Text) ||
                string.IsNullOrWhiteSpace(txtIntroducedBy.Text) ||
                string.IsNullOrWhiteSpace(txtWorkTitle.Text) ||
                string.IsNullOrWhiteSpace(txtWorkYears.Text) ||
                string.IsNullOrWhiteSpace(txtWorkCompany.Text) ||
                string.IsNullOrWhiteSpace(txtReasonForLeaving.Text) ||
                string.IsNullOrWhiteSpace(txtDaysOpenPerWeek.Text) ||
                string.IsNullOrWhiteSpace(txtHobbiesInterests.Text) ||
                string.IsNullOrWhiteSpace(txtRentalInstallment.Text) ||
                string.IsNullOrWhiteSpace(txtOwnsHouseNo.Text) ||
                string.IsNullOrWhiteSpace(txtOwnsHouseType.Text) ||
                string.IsNullOrWhiteSpace(txtOwnsHouseLocation.Text) ||
                string.IsNullOrWhiteSpace(txtFinacialBy.Text) ||
                string.IsNullOrWhiteSpace(txtOtherAssets.Text)
               )
            {
                return false;
            }

            //radiobutton not null

            if (CustCategoryDropDownList.SelectedIndex == -1)
            {
                return false;
            }

            if (!vpppYes.Checked && !vpppNo.Checked)
            {
                return false;
            }

            if (customerTypeRadioButtonList.SelectedIndex == -1)
            {
                return false;
            }

            if (!radWorkedInOtherTradeYes.Checked && !radWorkedInOtherTradeNo.Checked)
            {
                return false;
            }

            if (!radStockMarketYes.Checked && !radStockMarketNo.Checked)
            {
                return false;
            }

            if (!radBorrowingGamblingYes.Checked && !radBorrowingGamblingNo.Checked)
            {
                return false;
            }

            if (!radPartnersGetAlongWellYes.Checked && !radPartnersGetAlongWellNo.Checked)
            {
                return false;
            }

            if (!radMaritalProblemsYes.Checked && !radMaritalProblemsNo.Checked)
            {
                return false;
            }

            if (!radKeepPromisesYes.Checked && !radKeepPromisesNo.Checked)
            {
                return false;
            }

            if (!radChequesDishonoredYes.Checked && !radChequesDishonoredNo.Checked)
            {
                return false;
            }

            if (!radMainRoadYes.Checked && !radMainRoadNo.Checked)
            {
                return false;
            }

            if (!radDifficultToFindYes.Checked && !radDifficultToFindNo.Checked)
            {
                return false;
            }

            if (!radGoodsMovingFastYes.Checked && !radGoodsMovingFastNo.Checked)
            {
                return false;
            }

            if (!radBelongsToCustomerYes.Checked && !radBelongsToCustomerNo.Checked)
            {
                return false;
            }

            if (!radBelongsToCustomerYes.Checked && !radBelongsToCustomerNo.Checked)
            {
                return false;
            }

            if (!rdoVeryGood.Checked && !rdoGood.Checked && !rdoAverage.Checked)
            {
                return false;
            }

            if (!IsAtLeastOneCheckboxChecked(chkClassmate, chkNeighbor, chkPersonalFriend, chkOthers))
            {
                return false; // Return false if none of the checkboxes is checked
            }

            if (!IsAtLeastOneCheckboxChecked(chkAggressive, chkOrganized, chkUnableToExpress,
               chkCapable, chkQuiet, chkUnorganized,
               chkEloquent, chkSkillful, chkUnskillful))
            {
                return false; // Return false if none of the checkboxes is checked
            }

            if (!IsAtLeastOneCheckboxChecked(chkAlwaysBusy, chkUsuallyBusy, chkSometimesBusy, chkNeverBusy))
            {
                return false; // Return false if none of the checkboxes is checked
            }

            if (DropDownListCustomerDescription.SelectedItem.Text == "-- SELECT --")
            {
                return false;
            }

            if (DropDownListCreditTerm.SelectedItem.Text == "-- SELECT --")
            {
                return false;
            }

            return true;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Save data directly to SQL table without validation checks
            SaveToDatabase();
        }

        // Helper method to save data to the SQL table
        private void SaveToDatabase()
        {
            MySqlConnection connection = new MySqlConnection(GLOBAL.connStr);
            try
            {
                string query = @"UPDATE newcust_details SET " +
                                  "SalesmanName = @SalesName, SalesmanMark = @SalesMark, status = @Status, " +
                                 " FormNo = @FN, CustCategory = @CCategory, CustAddress = @CAddress, " +
                                 " CustProvince = @CProvince, CustPostal = @CPostal, CustCity = @CCity, " +
                                 " CustTerritory = @CTerritory, CustRegister = @CRegis, CustClass = @CClass, " +
                                 " CreditTerm = @CTerm, Discount = @Disc, CreditLimit = @CLimit, " +
                                 " CustVPPP = @CVPPP, CustType = @CType " +
                                 " WHERE NewCustID = @NewCustID ;" +

                                  "UPDATE newcust_salesmandeclaration SET " +
                                   "Name = @SDName, CustPersonally = @SDPersonally, " +
                                   "CustYearsKnown = @SDYearsKnown, CustRelationShip = @SDRelationShip, " +
                                   "CustRecommender = @SDRecommender, RecommenderComp = @SDCompany, " +
                                   "CustWork = @SDWork, CustYears = @SDYears, CustComp = @SDCustCompany, " +
                                   "CustLeaving = @SDLeaving, CustOtherTrade = @SDTrade, CustWorkDays = @SDDays, " +
                                   "CustBorrowing = @SDBorrowing, CustPartners = @SDPartners, " +
                                   "CustMarital = @SDMarital, CustPromise = @SDPromise, CustCheque = @SDCheque, CustStockMarket = @SDStockMarket, " +
                                   "CustFind = @SDFind, CustHobbies = @SDHobbies, ShopMainRoad = @SDMainRoad, " +
                                   "ShopHardFind = @SDHardFind, ShopBusy = @SDBusy, ShopMoving = @SDMoving, " +
                                   "ShopBelong = @SDBelong, ShopRental = @SDRental, CustOwnNo = @SDOwnNo, " +
                                   "CustOwnType = @SDOwnType, CustOwnLocation = @SDOwnLocation, " +
                                   "CustFinacial = @SDFinacial, CustAssets = @SDAssests, OtherComments = @SDComments," +
                                   "CustWorkHoursAM = @SDHoursAM, CustWorkHoursPM = @SDHoursPM, NameOfCust = @SDNameCust " +
                                   "WHERE NewCustID = @NewCustID ; " +

                                     "UPDATE newcust_guarantor SET " +
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
                                     "WHERE NewCustID = @NewCustID ;";






                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@NewCustID", HiddenNewCustID.Text.Trim());
                command.Parameters.AddWithValue("@SalesName", salesmanNameTextBox.Text.Trim());
                command.Parameters.AddWithValue("@SalesMark", txtSalesmansMark.Text.Trim());
                command.Parameters.AddWithValue("@status", txtStatus.Text.Trim());
                command.Parameters.AddWithValue("@FN", txtFormNo.Text.Trim());
                command.Parameters.AddWithValue("@CAddress", customerAddress.Text.Trim());
                command.Parameters.AddWithValue("@CProvince", province.Text.Trim());
                command.Parameters.AddWithValue("@CPostal", postalCode.Text.Trim());
                command.Parameters.AddWithValue("@CCity", city.Text.Trim());
                command.Parameters.AddWithValue("@CRegis", rocRob.Text.Trim());
                command.Parameters.AddWithValue("@CClass", DropDownListCustomerDescription.SelectedValue);
                command.Parameters.AddWithValue("@CTerm", DropDownListCreditTerm.SelectedValue);
                command.Parameters.AddWithValue("@CLimit", creditLimit.Text.Trim());
                command.Parameters.AddWithValue("@CVPPP", vpppYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@CTerritory", territory.Text.Trim());
                command.Parameters.AddWithValue("@Disc", chkDiscount.Checked ? 1 : 0);
              
                string selectedCategory = CustCategoryDropDownList.SelectedItem?.Text;
                command.Parameters.AddWithValue("@CCategory", selectedCategory ?? DBNull.Value.ToString());

                command.Parameters.AddWithValue("@SDName", txtCustomerName.Text.Trim());
                command.Parameters.AddWithValue("@SDPersonally", radYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDYearsKnown", CustomerYearsKnown.Text.Trim());
                command.Parameters.AddWithValue("@SDRecommender", txtIntroducedBy.Text.Trim());
                command.Parameters.AddWithValue("@SDCompany", txtCompanyName.Text.Trim());
                command.Parameters.AddWithValue("@SDRelationShip", GetSelectedRelationship());
                command.Parameters.AddWithValue("@SDWork", txtWorkTitle.Text.Trim());
                command.Parameters.AddWithValue("@SDYears", txtWorkYears.Text.Trim());
                command.Parameters.AddWithValue("@SDCustCompany", txtWorkCompany.Text.Trim());
                command.Parameters.AddWithValue("@SDLeaving", txtReasonForLeaving.Text.Trim());
                command.Parameters.AddWithValue("@SDTrade", radWorkedInOtherTradeYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDDays", txtDaysOpenPerWeek.Text.Trim());
                command.Parameters.AddWithValue("@SDStockMarket", radStockMarketYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDBorrowing", radBorrowingGamblingYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDPartners", radPartnersGetAlongWellYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDMarital", radMaritalProblemsYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDPromise", radKeepPromisesYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDCheque", radChequesDishonoredYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDHobbies", txtHobbiesInterests.Text.Trim());
                command.Parameters.AddWithValue("@SDMainRoad", radMainRoadYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDHardFind", radDifficultToFindYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDMoving", radGoodsMovingFastYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDBelong", radBelongsToCustomerYes.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@SDRental", txtRentalInstallment.Text.Trim());
                command.Parameters.AddWithValue("@SDOwnNo", txtOwnsHouseNo.Text.Trim());
                command.Parameters.AddWithValue("@SDOwnType", txtOwnsHouseType.Text.Trim());
                command.Parameters.AddWithValue("@SDOwnLocation", txtOwnsHouseLocation.Text.Trim());
                command.Parameters.AddWithValue("@SDFinacial", txtFinacialBy.Text.Trim());
                command.Parameters.AddWithValue("@SDAssests", txtOtherAssets.Text.Trim());
                command.Parameters.AddWithValue("@SDNameCust", txtPersonName.Text.Trim());
                string considerationValue = GetSelectedConsideration();
                command.Parameters.AddWithValue("@SDComments", considerationValue);


                string shopBusyValue = GetSelectedShopBusyValue();
                command.Parameters.AddWithValue("@SDBusy", shopBusyValue);

                string selectedCheckboxes = GetSelectedCheckboxes();
                command.Parameters.AddWithValue("@SDFind", selectedCheckboxes);


                string selectedCustomerType = customerTypeRadioButtonList.SelectedItem?.Text;
                command.Parameters.AddWithValue("@CType", selectedCustomerType ?? DBNull.Value.ToString());


                command.Parameters.AddWithValue("@SDHoursAM", txtStartHourAM.Value);
                command.Parameters.AddWithValue("@SDHoursPM", txtEndHourPM.Value);


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