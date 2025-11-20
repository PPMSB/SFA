using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNet.Visitor_MainMenu;
using static DotNet.Visitor_Model.AppointmentModel;

namespace DotNet
{
    public partial class Visitor_GuestAppointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string qrCodeImageUrl = Function_Method.GenerateQRCode(GLOBAL.ProductionWebsite + "Visitor_CreateNewAppointment.aspx?guest=true");
            QRCodeImage.ImageUrl = qrCodeImageUrl;

            //if (!IsPostBack)
            //{
            //    BindListView();
            //}
        }

        protected void RedirectUserToCreateNew(object sender, EventArgs e)
        {
            Session["UserName"] = "";
            Session["UserRole"] = UserRoleType.Guest;
            Session["UserCompany"] = "";
            Session["UserFullName"] = "Guest";
            Session["switch_Company"] = "";
            Visitor_Login.system_time_format();

            Response.Redirect("Visitor_CreateNewAppointment.aspx?guest=true");
        }

        [WebMethod]
        public static string TestingAjAX(string Name)
        {
            GetUserDataAsync(Name);
            return Name;
        }
        [WebMethod]
        private static async void GetUserDataAsync(string Name)
        {
            var context = HttpContext.Current;

            string apiUrl = "http://localhost:44381/Login.aspx/RedirectToMainMenu?UserName=John"; // Example API URL
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Make the request
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read response content as string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Here you can process the response body, e.g., parse JSON
                        context.Response.Write("<script>alert('User Data Retrieved: " + responseBody + "');</script>");
                    }
                    else
                    {
                        context.Response.Write("<script>alert('Error: Unable to retrieve user data.');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                context.Response.Write("<script>alert('Exception: " + ex.Message + "');</script>");
            }
        }

        //private List<Appointment> GetAppointments()
        //{
        //    return new List<Appointment>
        //{
        //    new Appointment { AppointmentID = 1, AppointmentTitle = "Doctor's Appointment", Date = DateTime.Now.AddDays(1), Status = AppointmentStatus.Awaiting },
        //    new Appointment { AppointmentID = 2, AppointmentTitle = "Dental Checkup", Date = DateTime.Now.AddDays(2), Status = AppointmentStatus.CheckedIn },
        //    new Appointment { AppointmentID = 3, AppointmentTitle = "Eye Exam", Date = DateTime.Now.AddDays(-1), Status = AppointmentStatus.Canceled }
        //};
        //}

        //private void BindListView()
        //{
        //    List<Appointment> appointments = GetAppointments();
        //    ListViewAppointments.DataSource = appointments;
        //    ListViewAppointments.DataBind();
        //}

        //protected void CheckOutAction(object sender, EventArgs e)
        //{
        //    Button button = (Button)sender;
        //    int appointmentId = Convert.ToInt32(button.CommandArgument);

        //    // Here you would implement your checkout logic, e.g., update the appointment status
        //    // For demonstration:
        //    // var appointment = ... (get appointment by id and update its status)

        //    // After the action, rebinding the ListView if necessary
        //    BindListView(); // Re-bind the list if you want to reflect changes immediately
        //}

        //protected void ListViewAppointments_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListViewItemType.DataItem)
        //    {
        //        // Get the current appointment
        //        var currentAppointment = (Appointment)e.Item.DataItem;

        //        // Manipulate displayed data if needed
        //        // Example: Show user-friendly status
        //        Label lblStatus = (Label)e.Item.FindControl("lblStatus");
        //        if (lblStatus != null)
        //        {
        //            //lblStatus.Text = currentAppointment.Status.GetDisplayName(); // Assuming GetDisplayName method is available
        //        }
        //    }
        //}
    }
}