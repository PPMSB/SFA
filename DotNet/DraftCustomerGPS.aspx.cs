using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZXing.QrCode.Internal;

namespace DotNet
{
    public partial class DraftCustomerGPS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // This code will only execute on the initial page load
                check_session();
                TimeOutRedirect();
                //CheckAcc(sender, e);
                //Check_DataRequest();
                PopulateSalesmanDropDowns();

            }
        }


        protected void getLocationButton_Click(object sender, EventArgs e)
        {
            // Handle the button click event (if needed)
        }

        [System.Web.Services.WebMethod]
        public static void UpdateLocation(double latitude, double longitude)
        {
            // Process the location data on the server (C#)
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

        private bool Check_Admin_Logging(string tempUserName, string tempaxPWD)
        {
            if (tempUserName == "administrator" && tempaxPWD == "administrator123")

            {
                Session["axPWD"] = tempaxPWD;
                Session["logined_user_name"] = "Admin DotNet";
                Session["user_id"] = tempUserName;
                Session["user_authority_lvl"] = 1;
                Session["page_access_authority"] = 0xFFF;
                Session["user_company"] = "PPM";
                Session["module_access_authority"] = 0xFF;
                Session["switch_Company"] = "PPM";
                Session["flag_temp"] = 0;
                Session["system_checking"] = 0;
                Session["data_passing"] = "";
                Session["user_authority_lvl_Red"] = Convert.ToInt32(Session["user_authority_lvl"]);
                Session["logined_user_name_Red"] = Session["logined_user_name"].ToString();
                return true;
            }
            else
            {
                return false;
            }

        }


        private void PopulateSalesmanDropDowns()
        {
            Axapta DynAx = new Axapta();

            try
            {
                // Connect to Axapta using your existing connection code
                GLOBAL.Company = GLOBAL.switch_Company;
                DynAx.LogonAs(GLOBAL.user_id, GLOBAL.DomainName,
                    new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                    GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                // Call your method to get separate lists for salesman names and codes
                List<ListItem> salesmanNameList = get_AxSalesmenNames(DynAx);
                List<ListItem> salesmanCodeList = get_AxSalesmenCodes(DynAx);

                // Bind the lists to the corresponding dropdown list controls
                DropDownListSalesmanName.DataSource = salesmanNameList;
                DropDownListSalesmanName.DataBind();

                DropDownListSalesmanCode.DataSource = salesmanCodeList;
                DropDownListSalesmanCode.DataBind();
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

        public List<ListItem> get_AxSalesmenNames(Axapta DynAx)
        {
            List<ListItem> salesmanNameList = new List<ListItem>();

            // Assuming your salesmen table in Axapta is named "EmplTable"
            int EmplTable = 103; // Replace with the actual table number

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);


            salesmanNameList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                string temp_SalesmanName = DynRec1.get_Field("DEL_Name").ToString();

                // Log or output the values for debugging
                System.Diagnostics.Debug.WriteLine($"Salesman Name: {temp_SalesmanName}");

                // Add salesman name to the list
                salesmanNameList.Add(new ListItem(temp_SalesmanName));

                // Dispose of the current record
                DynRec1.Dispose();
            }
            return salesmanNameList;
        }


        public List<ListItem> get_AxSalesmenCodes(Axapta DynAx)
        {
            List<ListItem> salesmanCodeList = new List<ListItem>();

            int EmplTable = 103; // Replace with the actual table number

            AxaptaObject axQuery1 = DynAx.CreateAxaptaObject("Query");
            AxaptaObject axQueryDataSource1 = (AxaptaObject)axQuery1.Call("addDataSource", EmplTable);

            AxaptaObject axQueryRun1 = DynAx.CreateAxaptaObject("QueryRun", axQuery1);

            salesmanCodeList.Add(new ListItem("-- SELECT --", ""));
            while ((bool)axQueryRun1.Call("next"))
            {
                AxaptaRecord DynRec1 = (AxaptaRecord)axQueryRun1.Call("Get", EmplTable);
                string temp_SalesmanCode = DynRec1.get_Field("Emplid").ToString();
                string temp_SalesmanName = DynRec1.get_Field("DEL_Name").ToString();
                System.Diagnostics.Debug.WriteLine($"Salesman Name: {temp_SalesmanName}, Salesman Code: {temp_SalesmanCode}");

                // Display both name and code, but save only the code
                salesmanCodeList.Add(new ListItem($"({temp_SalesmanCode})", temp_SalesmanCode));

                DynRec1.Dispose();
            }
            return salesmanCodeList;
        }





        protected void submitFormButton_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedSalesmanCode = DropDownListSalesmanCode.SelectedValue;
                string selectedSalesmanName = DropDownListSalesmanName.SelectedItem.Text;
                string company = "Posim Petroleum Marketing Sdn Bhd";
                string status = "DRAFT";
                string FormNumber = "SYSTEM";
                string custName = TextBoxCustName.Text; // Use TextBoxCustName instead of Label_CustName
                int documentType;

                if (NewDoc.Checked)
                {
                    documentType = 1;
                }
                else
                {
                    documentType = 0;
                }

                string coordinate = Coordinate.Text;

                DateTime currentDateTime = DateTime.Now;
                TimeSpan currentTime = currentDateTime.TimeOfDay;

                string query = @"
                INSERT INTO `newcust_details`
                (`NewCustID`, `EmplID`, `SalesmanName`, `company`, `status`, `FormNo`, `custName`, `Coordinate`, `DocumentType`, `createdDt`, `createdTime`)
                VALUES (@NewCustID, @EmplID, @SalesmanName, @Company, @Status, @FormNumber, @CustName, @Coordinate, @DocumentType, @CreatedDt, @CreatedTime);

                INSERT INTO `newcust_salesmandeclaration` (`NewCustID`, `custName`) VALUES (@NewCustID, @CustName);
                INSERT INTO `newcust_guarantor` (`NewCustID`, `CustName`) VALUES (@NewCustID, @CustName);
                INSERT INTO `newcust_contactdetails` (`NewCustID`, `CustName`) VALUES (@NewCustID, @CustName);
                INSERT INTO `newcust_document_checklist` (`NewCustID`, `CustName`) VALUES (@NewCustID, @CustName);
                INSERT INTO `newcust_hodrecommendation` (`NewCustID`, `CustName`) VALUES (@NewCustID, @CustName);
                INSERT INTO `newcust_documentation_status` (`NewCustID`, `CustName`) VALUES (@NewCustID, @CustName);
                INSERT INTO `newcust_custaccstatus` (`NewCustID`, `CustName`) VALUES (@NewCustID, @CustName);          
            ";

                using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Company", company);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@FormNumber", FormNumber);
                        command.Parameters.AddWithValue("@CustName", custName);
                        command.Parameters.AddWithValue("@CreatedDt", currentDateTime.Date);
                        command.Parameters.AddWithValue("@CreatedTime", currentTime);
                        command.Parameters.AddWithValue("@DocumentType", documentType);
                        command.Parameters.AddWithValue("@Coordinate", coordinate);
                        command.Parameters.AddWithValue("@EmplID", selectedSalesmanCode);
                        command.Parameters.AddWithValue("@SalesmanName", selectedSalesmanName);

                        string newCustID = GenerateNewCustID();
                        command.Parameters.AddWithValue("@NewCustID", newCustID);


                        connection.Open();
                        command.ExecuteNonQuery();

                        // Store values in session variables
                        Session["SalesmanName"] = selectedSalesmanName;
                        Session["Status"] = status;
                        Session["SalesmanCode"] = selectedSalesmanCode;
                        Session["FormNo"] = FormNumber;
                        Session["Coordinate"] = coordinate;
                        Session["CustName"] = custName;
                        Session["NewCustID"] = newCustID;


                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessMessage", "showSuccessMessage();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display other exceptions
                output.InnerText = "Error: " + ex.Message;

                // Show error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showErrorMessage", "showErrorMessage();", true);
            }
         }


        private string GenerateNewCustID()
        {
            // Implement your logic to generate a unique NewCustID here
            // For example, you can use a combination of numbers and "CustNewID"
            Random random = new Random();
            int randomNumber = random.Next(10000, 99999); // Adjust the range as needed
            string newCustID = randomNumber.ToString() + "NCID";

            return newCustID;
        }

        private void RedirectAfterSuccess()
        {
            string redirectScript = @"
        var confirmation = confirm('Form submitted successfully! Click OK to proceed to AddApplication.aspx.');
        if (confirmation) {
            window.location.href = 'AddApplication.aspx';
        } else { 
            window.location.href = 'NewCustomer.aspx';
        }
    ";

            ScriptManager.RegisterStartupScript(this, GetType(), "redirectScript",  redirectScript, true);
        }

    }
}










