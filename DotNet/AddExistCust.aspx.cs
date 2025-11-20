using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing.QrCode.Internal;

namespace DotNet
{
    public partial class AddExistCust : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Other form field values
            string salesmanName = SalesmanName.Text;
            string company = "Posim Petroleum Marketing Sdn Bhd";
            string Branch = "Shah Alam";
            string status = "New";
            string FormNumber = "SYSTEM";
            string code = TextBox1.Text;
            string custName = CustName.Text;

            // Always set documentType to 0
            int documentType = 0;

            DateTime currentDateTime = DateTime.Now;
            TimeSpan currentTime = currentDateTime.TimeOfDay;

            string query = "INSERT INTO newcust_details (SalesmanName, Company, Branch, Status, FormNo, Code, CustName, createdDt, createdTime, DocumentType) " +
                           "VALUES (@SalesmanName, @Company, @Branch, @Status, @FormNumber, @Code, @CustName, @CreatedDt, @CreatedTime, @DocumentType)";

            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@SalesmanName", salesmanName);
                    command.Parameters.AddWithValue("@Company", company);
                    command.Parameters.AddWithValue("@Branch", Branch);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@FormNumber", FormNumber);
                    command.Parameters.AddWithValue("@Code", code);
                    command.Parameters.AddWithValue("@CustName", custName);
                    command.Parameters.AddWithValue("@CreatedDt", currentDateTime.Date);
                    command.Parameters.AddWithValue("@CreatedTime", currentTime);
                    command.Parameters.AddWithValue("@DocumentType", documentType);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            output.InnerText = "Form submitted successfully!";
                        }
                        else
                        {
                            output.InnerText = "Error: Form submission failed.";
                        }
                    }
                    catch (Exception ex)
                    {
                        output.InnerText = "Error: " + ex.Message;
                    }
                }
            }
        }



    }
}