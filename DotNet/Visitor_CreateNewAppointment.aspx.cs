using DotNet.Visitor_Model;
//using DotNetSync.lhdn.Models;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNet.Visitor_MainMenu;
using static DotNet.Visitor_Model.AppointmentModel;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DotNet
{
    public partial class Visitor_CreateNewAppointment : System.Web.UI.Page
    {
        private readonly object _lock = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            UserRoleType UserRole = (UserRoleType)Convert.ToInt32(Session["UserRole"]);
            NavigationItemsModel m = new NavigationItemsModel();
            m.NewAppointment = NewAppointment;
            m.NewAppointmentTag = null;
            m.AppointmentHistory = AppointmentHistory;
            m.AppointmentHistoryTag = null;
            m.SFA = SFA;
            m.SFATag = null;
            if (UserRole == UserRoleType.Security)
            {
                Function_Method.MsgBox("No Permission to access.", this.Page, this);

                Response.Redirect("Visitor_MainMenu.aspx");
            }
            else
            {
                if (Request.QueryString["guest"] == "true")
                {
                    Function_Method.LoadVisitorSelectionMenu(UserRole, m);

                    if (!IsPostBack)
                    {
                        // Bind your dropdown list here
                        LPGCompaniesDropdown();
                    }
                }
                else
                {
                    Visitor_CheckSession.CheckSession(this.Page, this);
                    Visitor_CheckSession.TimeOutRedirect(this.Page, this);
                    Function_Method.LoadVisitorSelectionMenu(UserRole, m);

                    if (!IsPostBack)
                    {
                        // Bind your dropdown list here
                        LPGCompaniesDropdown();
                    }
                }
            }
        }

        protected void BackToMainMenu(object sender, EventArgs e)
        {
            Response.Redirect("Visitor_MainMenu.aspx");

        }

        protected void CreateNewAppointment(object sender, EventArgs e)
        {
            try
            {
                if (CheckTextEmpty())
                {
                    Function_Method.MsgBox("Please complete all the inputs.", this.Page, this);

                    return;
                }
                lock (_lock)
                {
                    MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                    conn.Open();
                    string SQL = CheckRoomAvailabilityQuery(true);

                    DateTime StartDateTime = DateTime.Parse(Request.Form[txtFromDate.UniqueID]);
                    DateTime EndDateTime = DateTime.Parse(Request.Form[txtFromDate.UniqueID]).AddMinutes(Int32.Parse(ddlTimeDuration.SelectedValue));
                    List<string> ColumnList1 = new List<string> { "", "", "" };
                    List<object> ParamValue = new List<object> { StartDateTime, EndDateTime, ddlAvailableLocation.SelectedValue };
                    var ParamDict1 = GlobalHelper.ConvertModelColumnsIntoDictionary(ColumnList1, ParamValue);

                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    GlobalHelper.PumpParamQuery(cmd, ParamDict1);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(0) != DBNull.Value)
                            {
                                if (reader.GetValue(5).ToString() == "0")
                                {
                                    Function_Method.MsgBox("Current location are not available. Please select other location.", this.Page, this);

                                    return;
                                }
                            }

                        }
                    }

                    VisitorAppointmentModel m = new VisitorAppointmentModel();
                    string NewAppointmentID = GenerateNewAppointmentID();
                    string TableName = "visitor_staffappointment";
                    List<object> ObjectList = StoreAllVariables(NewAppointmentID);
                    List<string> ColumnList = GlobalHelper.GetColumnsByModel(m);
                    Dictionary<string, object> ParamDict = GlobalHelper.ConvertModelColumnsIntoDictionary(ColumnList, ObjectList);

                    GlobalHelper.InsertQuery(TableName, ColumnList, ParamDict);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none",
"<script>$('#ReceiptModal').show();</script>", false);

                    GenerateReport(NewAppointmentID);

                    //GenerateReport(BtnGenReport, EventArgs.Empty);
                    //Function_Method.MsgBox("New Update to MySQL Success.", this.Page, this);
                    //Response.Redirect("Visitor_CreateNewAppointment.aspx");
                }
            }
            catch (Exception ER_AU_02)
            {
                Function_Method.MsgBox("Please Try Again.", this.Page, this);
            }
        }

        private List<object> StoreAllVariables(string NewAppointmentID)
        {
            List<object> ObjectList = new List<object>();
            var context = HttpContext.Current;
            //Must order in sequence based on model sequence
            ObjectList.Add(NewAppointmentID);
            ObjectList.Add(context.Session["UserName"].ToString());
            ObjectList.Add(VisitorName.Text);
            ObjectList.Add(VisitorCompany.Text);
            ObjectList.Add(ddlLPGCompany.SelectedValue.ToString());
            ObjectList.Add(NumberOfVisitors.Text);
            ObjectList.Add(DateTime.Parse(Request.Form[txtFromDate.UniqueID]).ToString(GLOBAL.gDisplayDateTimeFormat));
            ObjectList.Add(DateTime.Parse(Request.Form[txtFromDate.UniqueID]).AddMinutes(Int32.Parse(ddlTimeDuration.SelectedValue)).ToString(GLOBAL.gDisplayDateTimeFormat));
            ObjectList.Add(DateTime.MinValue.ToString(GLOBAL.gDisplayDateTimeFormat));
            ObjectList.Add(DateTime.MinValue.ToString(GLOBAL.gDisplayDateTimeFormat));
            ObjectList.Add(VehiclePlateNo.Text);
            ObjectList.Add(PersonToMeet.Text);
            ObjectList.Add(Purpose.Text);
            ObjectList.Add(DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat));
            ObjectList.Add(AppointmentStatus.Awaiting);
            ObjectList.Add(ddlAvailableLocation.SelectedItem.Value);

            return ObjectList;
        }

        protected void GenerateReport(string AppointmentID)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            try
            {

                string Query = "select * from visitor_staffappointment where AppointmentID = @p0";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                MySqlParameter ParamAppointmentID = new MySqlParameter("@p0", MySqlDbType.VarChar, 0);
                ParamAppointmentID.Value = AppointmentID;
                cmd.Parameters.Add(ParamAppointmentID);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            string SelectedLocation = ddlAvailableLocation.SelectedItem.Text.Split('-')[0].Trim();

                            Label_SerialNumber.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            Label_FullName.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            Label_VisitorCompany.Text = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            Label_LPGCompany.Text = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            Label_Visitors.Text = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            Label_DateVisit.Text = reader.IsDBNull(6) ? "" : reader.GetString(6) + " - " +
                                                  (reader.IsDBNull(7) ? "" : reader.GetString(7));
                            Label_VehiclePlateNo.Text = reader.IsDBNull(10) ? "" : reader.GetString(10);
                            Label_PersonToMeet.Text = reader.IsDBNull(11) ? "" : reader.GetString(11);
                            Label_Purpose.Text = reader.IsDBNull(12) ? "" : reader.GetString(12);
                            Label_Location.Text = string.IsNullOrEmpty(SelectedLocation) ? "" : SelectedLocation;

                        }
                    }
                }
                conn.Close();



            }
            catch (Exception ex)
            {
                Function_Method.MsgBox("ERROR: " + ex.ToString(), this.Page, this);
            }
        }

        protected void TimeDurationDropdown(object sender, EventArgs e)
        {
            ddlTimeDuration.Items.Clear();
            List<ListItem> items = new List<ListItem>();

            items.Add(new ListItem("-- SELECT --", ""));
            items.Add(new ListItem("30 minutes", "30"));
            items.Add(new ListItem("1 hour", "60"));
            items.Add(new ListItem("1 hour 30 minutes", "90"));
            items.Add(new ListItem("2 hours", "120"));
            items.Add(new ListItem("2 hours 30 minutes", "150"));
            items.Add(new ListItem("3 hours", "180"));

            ddlTimeDuration.Items.AddRange(items.ToArray());
        }

        protected void AvailableLocationDropdown(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(GLOBAL.connStr))
            {
                if (string.IsNullOrEmpty(Request.Form[txtFromDate.UniqueID]) || string.IsNullOrEmpty(ddlTimeDuration.SelectedValue))
                {
                    Function_Method.MsgBox("Please complete Date and Time input field.", this.Page, this);

                    return;
                }
                ddlAvailableLocation.Items.Clear();
                List<ListItem> items = new List<ListItem>();

                items.Add(new ListItem("-- SELECT --", ""));

                string SQL = CheckRoomAvailabilityQuery(false);
                MySqlCommand cmd = new MySqlCommand(SQL, connection);

                List<string> ColumnList = new List<string> { "", "" };

                DateTime StartDateTime = DateTime.Parse(Request.Form[txtFromDate.UniqueID]);
                DateTime EndDateTime = DateTime.Parse(Request.Form[txtFromDate.UniqueID]).AddMinutes(Int32.Parse(ddlTimeDuration.SelectedValue));
                List<object> ParamValue = new List<object> { StartDateTime, EndDateTime };
                var ParamDict = GlobalHelper.ConvertModelColumnsIntoDictionary(ColumnList, ParamValue);

                GlobalHelper.PumpParamQuery(cmd, ParamDict);
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            if (reader.GetValue(5).ToString() == "0")
                            {
                                continue;
                            }
                            items.Add(new ListItem(reader.GetValue(1).ToString() + " - " + reader.GetValue(5).ToString() + " slot(s) left.", reader.GetValue(0).ToString()));
                        }
                    }
                }
                connection.Close();
                ddlAvailableLocation.Items.AddRange(items.ToArray());

            }
        }


        private string CheckRoomAvailabilityQuery(bool IsChecking)
        {
            string SQL = "select t1.*, ifnull(t2.RoomUsed, 0) RoomUsed, IFNULL((t1.AvailableSlot - t2.RoomUsed), t1.AvailableSlot) LeftSlot from visitor_availablelocation t1 " +
    "left join (select count(1) RoomUsed, LocationID from visitor_staffappointment where " +
    "((Status != 3 and @p0 BETWEEN StartDateTime and CASE WHEN ActualEndDateTime > '1900-01-01' THEN ActualEndDateTime ELSE EndDateTime END) or (Status != 3 and @p1 between StartDateTime and " +
    "CASE WHEN ActualEndDateTime > '1900-01-01' THEN ActualEndDateTime ELSE EndDateTime END)) group by LocationID) " +
    "t2 on t2.LocationID = t1.LocationID ";

            if (IsChecking)
            {
                SQL += "where t1.LocationID = @p2";
            }
            return SQL;
        }

        private void LPGCompaniesDropdown()
        {
            try
            {
                ddlTimeDuration.Items.Clear();
                List<ListItem> items = new List<ListItem>();

                items.Add(new ListItem("-- SELECT --", ""));
                items.Add(new ListItem("30 minutes", "30"));
                items.Add(new ListItem("1 hour", "60"));
                items.Add(new ListItem("1 hour 30 minutes", "90"));
                items.Add(new ListItem("2 hours", "120"));
                items.Add(new ListItem("2 hours 30 minutes", "150"));
                items.Add(new ListItem("3 hours", "180"));

                ddlTimeDuration.Items.AddRange(items.ToArray());

                items = new List<ListItem>();

                ddlLPGCompany.Items.Clear();
                items = new List<ListItem>();
                items.Add(new ListItem("-- SELECT --", ""));
                string UserId = (string.IsNullOrEmpty(Session["UserName"].ToString()) ? "foozm" : Session["UserName"].ToString().Trim());
                string TableName = "DataArea";

                Axapta DynAx = new Axapta();
                AxaptaRecord DynRec;
                int data_count = 2;
                string[] F = new string[data_count];
                F[0] = "id";
                F[1] = "name";

                string[] N = new string[data_count];
                N[0] = "Company Accounts";
                N[1] = "Name of Company Accounts";

                // Output variables for calls to AxRecord.get_Field
                object[] O = new object[data_count];

                try
                {
                    // Log on to Microsoft Dynamics AX.
                    GLOBAL.Company = null;

                    DynAx.LogonAs(UserId, GLOBAL.DomainName, new System.Net.NetworkCredential(GLOBAL.ProxyUserName, GLOBAL.ProxyPassword, GLOBAL.DomainName),
                        GLOBAL.switch_Company, GLOBAL.Language, GLOBAL.ObjectServer, null);

                    DynRec = DynAx.CreateAxaptaRecord(TableName);

                    DynRec.ExecuteStmt("select * from %1 ORDER BY RECID ASC");

                    while (DynRec.Found)
                    {
                        List<string> Companies = new List<string>();
                        for (int i = 0; i < data_count; i++)
                        {
                            O[i] = DynRec.get_Field(F[i]);

                            Companies.Add(O[i].ToString());
                        }
                        items.Add(new ListItem(Companies[1], Companies[0]));

                        // Advance to the next row.
                        DynRec.Next();

                    }
                    // Dispose of the AxaptaRecord object.

                    DynRec.Dispose();

                    // Log off from Microsoft Dynamics AX.

                    DynAx.Logoff();
                    //Data-Binding with our GRID

                    ddlLPGCompany.Items.AddRange(items.ToArray());
                }
                catch (Exception ER_AU_03)
                {
                    Function_Method.MsgBox("ER_AU_03: " + ER_AU_03.ToString(), this.Page, this);
                }
                finally
                {
                    DynAx.Dispose();
                }
            }
            catch
            {
                Response.Redirect("Visitor_Login.aspx");
            }
        }


        protected string GenerateNewAppointmentID()
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string format = DateTime.Now.ToString("yyMM");

            string AppointmentID = format + 1;
            try
            {

                string Query = "select IFNULL(AppointmentID, 1)AppointmentID from visitor_staffappointment order by CreatedDateTime desc limit 1";

                MySqlCommand cmd = new MySqlCommand(Query, conn);

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            int intAppointmentID = Int32.Parse(reader.GetString("AppointmentID")) + 1;

                            AppointmentID = intAppointmentID.ToString();
                        }
                    }
                }
                conn.Close();
                return AppointmentID;
            }
            catch (Exception e)
            {
                Function_Method.MsgBox("ERROR: " + e.ToString(), this.Page, this);
                return AppointmentID;
            }
        }
        private void clear_variable()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private bool CheckTextEmpty()
        {
            List<string> CheckEmptyValue = new List<string>();
            CheckEmptyValue.Add(VisitorName.Text);
            CheckEmptyValue.Add(VisitorCompany.Text);
            CheckEmptyValue.Add(ddlLPGCompany.SelectedItem.Value);
            CheckEmptyValue.Add(VehiclePlateNo.Text);
            CheckEmptyValue.Add(PersonToMeet.Text);
            CheckEmptyValue.Add(Purpose.Text);
            CheckEmptyValue.Add(ddlTimeDuration.SelectedItem.Value);
            try
            {
                CheckEmptyValue.Add(ddlAvailableLocation.SelectedItem.Value);

                if (Int32.Parse(NumberOfVisitors.Text.ToString()) <= 0)
                {
                    CheckEmptyValue.Add("");
                }
                else
                {
                    CheckEmptyValue.Add(NumberOfVisitors.Text);
                }
            }
            catch
            {
                CheckEmptyValue.Add("");
            }


            bool IsNullOrEmpty = false;
            foreach (string item in CheckEmptyValue)
            {
                if (string.IsNullOrEmpty(item.Trim()))
                {
                    IsNullOrEmpty = true;

                    break;
                }
            }
            return IsNullOrEmpty;
        }
    }
}