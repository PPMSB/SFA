using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNet.Visitor_Model;
using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using Microsoft.Dynamics.BusinessConnectorNet;
using MySql.Data.MySqlClient;
using ZXing;
using OfficeOpenXml;

using static DotNet.Visitor_MainMenu;
using static DotNet.Visitor_Model.AppointmentModel;
using NLog.Filters;
using System.Data.SqlClient;
using System.Web.DynamicData;
using System.Web.UI.HtmlControls;
using System.Collections.Concurrent;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DotNet
{
    public partial class Visitor_History : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            UserRoleType UserRole = (UserRoleType)Convert.ToInt32(context.Session["UserRole"]);
            int Type = (string.IsNullOrEmpty(CurrentSelectedType.Text) ? 0 : Int32.Parse(CurrentSelectedType.Text));
            NavigationItemsModel m = new NavigationItemsModel();
            m.NewAppointment = NewAppointment;
            m.NewAppointmentTag = null;
            m.AppointmentHistory = AppointmentHistory;
            m.AppointmentHistoryTag = null;
            m.SFA = SFA;
            m.SFATag = null;
            if (UserRole == UserRoleType.Guest)
            {
                Function_Method.MsgBox("No Permission to access.", this.Page, this);

                Response.Redirect("Visitor_Login.aspx");
            }

            if (UserRole == UserRoleType.Security && Type != 0)
            {
                Function_Method.MsgBox("No Permission to access.", this.Page, this);

                Response.Redirect("Visitor_MainMenu.aspx");
            }
            else
            {
                dateRange_section.Visible = false;
                SearchWrapper.Visible = false;

                if (UserRole != UserRoleType.Security)
                {
                    Visitor_CheckSession.CheckSession(this.Page, this);
                    Visitor_CheckSession.TimeOutRedirect(this.Page, this);
                }

                Function_Method.LoadVisitorSelectionMenu(UserRole, m);

                if (!IsPostBack)
                {
                    DataTable dt = new DataTable();
                    Timer1.Enabled = true;

                    gvAppointmentReport.DataSource = dt;
                    gvAppointmentReport.DataBind();

                    Button btn = new Button();
                    (btn as Button).Text = "0";

                    GetAppointmentStatusList();
                    ViewAppointmentHistory(btn, e);
                }

                CheckUserRoleActions();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //Allows for printing
        }
        private void GetAppointmentStatusList()
        {
            ddlStatus.Items.Clear();
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("All", ""));

            foreach (var item in Enum.GetValues(typeof(AppointmentStatus)))
            {
                items.Add(new ListItem(item.ToString(), ((int)Enum.Parse(typeof(AppointmentStatus), item.ToString())).ToString()));
            }

            ddlStatus.Items.AddRange(items.ToArray());

        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            Button s = new Button();
            if (!string.IsNullOrEmpty(CurrentSelectedType.Text))
            {
                s.Text = CurrentSelectedType.Text;

                ViewAppointmentHistory(s, e);
            }
        }

        protected void ViewAppointmentHistory(object sender, EventArgs e)
        {
            string s = (sender as Button).Text.Trim();

            dateRange_section.Visible = false;
            btnTodayReport.Attributes.Add("style", "background-color:transparent");
            btnTomorrowReport.Attributes.Add("style", "background-color:transparent");
            btnPastReport.Attributes.Add("style", "background-color:transparent");
            //AwareColorSection.Visible = false;
            SearchWrapper.Visible = false;
            btnGenReport.Visible = false;
            export.Visible = false;

            int Type = (int)Enum.Parse(typeof(HistoryType), s);
            CurrentSelectedType.Text = Type.ToString();

            switch (Type)
            {
                case 0:
                    btnTodayReport.Attributes.Add("style", "background-color:#f58345");
                    AwareColorSection.Visible = true;
                    btnTitle.Text = "Today";
                    GenerateReport(Type, "");
                    break;

                case 1:
                    btnTomorrowReport.Attributes.Add("style", "background-color:#f58345");

                    btnTitle.Text = "Upcoming";
                    GenerateReport(Type, "");

                    break;

                case 2:
                    dateRange_section.Visible = true;
                    btnPastReport.Attributes.Add("style", "background-color:#f58345");
                    btnGenReport.Visible = true;
                    //SearchWrapper.Visible = true;

                    btnTitle.Text = "Past";
                    gvAppointmentReport.DataSource = null;
                    gvAppointmentReport.DataBind();

                    break;

            }

        }

        protected void CheckUserRoleActions()
        {
            btnTodayReport.Visible = false;
            btnTomorrowReport.Visible = false;
            btnPastReport.Visible = false;
            var context = HttpContext.Current;

            UserRoleType UserRole = (UserRoleType)Convert.ToInt32(context.Session["UserRole"]);
            switch (UserRole)
            {
                case UserRoleType.HR:
                    btnTodayReport.Visible = true;
                    btnTomorrowReport.Visible = true;
                    btnPastReport.Visible = true;

                    //check role, hr can view all users

                    break;

                case UserRoleType.Staff:
                    btnTodayReport.Visible = true;
                    btnTomorrowReport.Visible = true;
                    btnPastReport.Visible = true;

                    //check role, staff can view their own records

                    break;

                case UserRoleType.Security:
                    btnTodayReport.Visible = true;

                    break;
            }
        }

        protected void GenerateHistory(object sender, EventArgs e)
        {
            string Filtered = TxtSearchAppointment.Text.Trim();
            dateRange_section.Visible = true;

            GenerateReport(2, Filtered, 0);

            if ((UserRoleType)Convert.ToInt32(Session["UserRole"]) == UserRoleType.HR)
            {
                export.Visible = true;
            }

            gvAppointmentReport.PageIndex = 0;
            gvAppointmentReport.DataBind();
        }

        protected void GenerateReport(int Type, string Filtered, int PageIndex = 0)
        {
            UserRoleType UserRole = (UserRoleType)Convert.ToInt32(Session["UserRole"]);
            bool CanViewAll = false;
            switch (Type)
            {
                case 0:
                    if (UserRole == UserRoleType.HR || UserRole == UserRoleType.Security)
                    {
                        CanViewAll = true;
                    }
                    GenerateHistoryGridView(Type, CanViewAll, PageIndex, Filtered);
                    break;

                case 1:
                    if (UserRole == UserRoleType.HR)
                    {
                        CanViewAll = true;
                    }
                    GenerateHistoryGridView(Type, CanViewAll, PageIndex, Filtered);

                    break;

                case 2:
                    if (UserRole == UserRoleType.HR)
                    {
                        CanViewAll = true;
                    }
                    GenerateHistoryGridView(Type, CanViewAll, PageIndex, Filtered);

                    break;

            }
            CurrentSelectedType.Text = Type.ToString();
        }

        protected void datagrid_PageIndexChanging_AppointmentHistory(object sender, GridViewPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentSelectedType.Text))
            {
                int Type = Int32.Parse(CurrentSelectedType.Text);

                GenerateReport(Type, "", e.NewPageIndex);

                gvAppointmentReport.PageIndex = e.NewPageIndex;
                gvAppointmentReport.DataBind();
            }
        }

        public void GenerateHistoryGridView(int Type, bool CanViewAll, int PageIndex, string Filtered)
        {
            var context = HttpContext.Current;

            gvAppointmentReport.DataSource = null;
            gvAppointmentReport.DataBind();
            // Log on to Microsoft Dynamics AX.
            Axapta DynAx = Function_Method.GlobalAxapta();

            DataTable dt = new DataTable();
            List<string> N = new List<string>{ "BtnAction", "No.", "AppointmentID" , "VisitorName" , "VisitorCompany" , "LPGCompany" , "Status", "NumberOfVisitors" , "StartDateTime",
                "EndDateTime", "ActualStartDateTime", "ActualEndDateTime", "LocationName", "VehiclePlateNo", "PersonToMeet", "Purpose", "UserName", "CreatedDateTime", "RowColor"};

            for (int i = 0; i < N.Count(); i++)
            {
                dt.Columns.Add(new DataColumn(N[i], typeof(string)));
            }
            //===========================================
            DataRow row;
            int Count = 0;
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
            string format = DateTime.Now.ToString("yyyyMM");

            string AppointmentID = format + 1;
            try
            {
                DateTime FilteredStartDateTime = DateTime.Now;
                DateTime FilteredEndDateTime = FilteredStartDateTime;
                string CountQuery = GetAppointmentQuery(Type, CanViewAll, Filtered, PageIndex, out FilteredStartDateTime, out FilteredEndDateTime, true);

                string Query = GetAppointmentQuery(Type, CanViewAll, Filtered, PageIndex, out FilteredStartDateTime, out FilteredEndDateTime, false);
                DateTime StartDateToday = DateTime.Now.Date;
                DateTime EndDateToday = StartDateToday.AddDays(1).AddSeconds(-1);
                DateTime DateTimeNow = DateTime.Now.AddHours(-1);

                MySqlCommand cmd = new MySqlCommand(Query, conn);
                MySqlCommand cmdCount = new MySqlCommand(CountQuery, conn);

                Dictionary<string, object> ParamDict = new Dictionary<string, object>
                    {
                        { "@p0", StartDateToday },
                        { "@p1", EndDateToday },
                        { "@p2", DateTimeNow },
                        { "@p3", context.Session["UserName"].ToString() },
                        { "@p4", FilteredStartDateTime },
                        { "@p5", FilteredEndDateTime },
                        { "@p6", DateTime.MinValue },
                        { "@p7", Filtered + "%" },
                        { "@p8", StartDateToday.AddDays(1) },
                        { "@p9", ddlStatus.SelectedValue },

                    };
                GlobalHelper.PumpParamQuery(cmd, ParamDict);
                GlobalHelper.PumpParamQuery(cmdCount, ParamDict);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            row = dt.NewRow();

                            row["AppointmentID"] = reader.GetString("AppointmentID");
                            row["VisitorName"] = reader.GetString("VisitorName");
                            row["VisitorCompany"] = reader.GetString("VisitorCompany");
                            row["LPGCompany"] = reader.GetString("LPGCompany");
                            row["Status"] = Enum.GetName(typeof(AppointmentStatus), Int32.Parse(reader.GetString("Status")));

                            row["NumberOfVisitors"] = reader.GetString("NumberOfVisitors");
                            row["StartDateTime"] = DateTime.Parse(reader.GetString("StartDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat);
                            row["EndDateTime"] = DateTime.Parse(reader.GetString("EndDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat);
                            row["ActualStartDateTime"] = (reader.GetString("ActualStartDateTime") == DateTime.MinValue.ToString() ? "" : DateTime.Parse(reader.GetString("ActualStartDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat));
                            row["ActualEndDateTime"] = (reader.GetString("ActualEndDateTime") == DateTime.MinValue.ToString() ? "" : DateTime.Parse(reader.GetString("ActualEndDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat));
                            row["LocationName"] = reader.GetString("LocationName");
                            row["VehiclePlateNo"] = reader.GetString("VehiclePlateNo");
                            row["PersonToMeet"] = reader.GetString("PersonToMeet");
                            row["Purpose"] = reader.GetString("Purpose");
                            row["UserName"] = reader.GetString("UserName");
                            row["CreatedDateTime"] = DateTime.Parse(reader.GetString("CreatedDateTime")).ToString(GLOBAL.gDisplayDateTimeWithoutSecondsFormat);

                            row["RowColor"] = "";

                            if ((DateTime.Parse(reader.GetString("ActualEndDateTime")) == DateTime.MinValue &&
                                DateTime.Parse(reader.GetString("StartDateTime")) >= DateTimeNow))
                            {
                                row["RowColor"] = "yellow";
                            }
                            else if (DateTime.Parse(reader.GetString("ActualEndDateTime")) != DateTime.MinValue)
                            {
                                row["RowColor"] = "#40E0D0";
                            }

                            dt.Rows.Add(row);
                        }
                    }
                }

                using (MySqlDataReader reader = cmdCount.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            Count = Int32.Parse(reader.GetString("Count"));
                        }
                    }
                }
                conn.Close();

                //gvAppointmentReport.PagerSettings.Visible = true;

                gvAppointmentReport.VirtualItemCount = Count;

                gvAppointmentReport.DataSource = dt;
                gvAppointmentReport.DataBind();

                if (Type != 0 || (UserRoleType)Convert.ToInt32(context.Session["UserRole"]) == UserRoleType.Staff)
                {
                    if (Type == 1)
                    {
                        gvAppointmentReport.Columns[1].Visible = true;

                    }
                    else
                    {
                        gvAppointmentReport.Columns[1].Visible = false;
                    }
                }
                else
                {
                    gvAppointmentReport.Columns[1].Visible = true;
                }

                if (dt.Rows.Count <= 0)
                {
                    AppointmentHistoryLabel.Attributes.Add("style", "color:red; font-weight: bolder");
                    AppointmentHistoryLabel.Visible = true;
                }
                else
                {
                    AppointmentHistoryLabel.Visible = false;
                }
            }
            catch (Exception ER_VH_00)
            {
                Function_Method.MsgBox("ER_VH_00: " + ER_VH_00.ToString(), this.Page, this);
            }
        }

        protected void gvAppointmentReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var context = HttpContext.Current;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = e.Row.DataItem as DataRowView;
                Button checkOutButton = (Button)e.Row.FindControl("Button_CheckOut");
                Button checkInButton = (Button)e.Row.FindControl("Button_CheckIn");
                Button cancelButton = (Button)e.Row.FindControl("Button_Cancel");
                
                if (row != null)
                {
                    string RowColor = row["RowColor"].ToString();

                    //e.Row.BackColor = System.Drawing.Color.FromName(RowColor);

                    if (row["ActualStartDateTime"].ToString() == "")
                    {
                        checkInButton.Visible = true;
                        checkOutButton.Visible = false;
                        cancelButton.Visible = true;
                    }
                    if (row["ActualEndDateTime"].ToString() == "" && row["ActualStartDateTime"].ToString() != "")
                    {
                        checkInButton.Visible = false;
                        checkOutButton.Visible = true;
                        cancelButton.Visible = false;
                    }
                    if (RowColor == "#40E0D0" || row["Status"].ToString() == AppointmentStatus.Canceled.ToString())
                    {
                        checkInButton.Visible = false;
                        checkOutButton.Visible = false;
                        cancelButton.Visible = false;
                    }
                    if ((UserRoleType)Convert.ToInt32(context.Session["UserRole"]) == UserRoleType.Security)
                    {
                        cancelButton.Visible = false;
                    }
                    if (CurrentSelectedType.Text == "1")
                    {
                        checkOutButton.Visible = false;
                        checkInButton.Visible = false;
                        cancelButton.Visible = true;
                    }

                }
            }
        }

        private string GetAppointmentQuery(int Type, bool CanViewAll, string Filtered, int PageIndex, out DateTime FilteredStartDateTime, out DateTime FilteredEndDateTime, bool IsCount, bool IsExportExcel = false)
        {
            string SQL = "select ";
            if (IsCount)
            {
                SQL += " count(1) Count ";
            }
            else
            {
                SQL += " *, t2.LocationName ";
            }

            SQL += " from visitor_staffappointment t1 inner join visitor_availablelocation t2 on t2.LocationID = t1.LocationID where ";

            if (Type == 0)
            {
                SQL += "StartDateTime >= @p0 and StartDateTime <= @p1 ";
            }
            else if (Type == 1)
            {
                SQL += "StartDateTime >= @p8 and ActualEndDateTime = @p6 and Status = 0 ";
            }
            else if (Type == 2)
            {
                SQL += "StartDateTime <= @p2 and StartDateTime >= @p4 and StartDateTime <= @p5 ";
                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                {
                    SQL += " and Status = @p9 ";
                }
            }

            if (!CanViewAll)
            {
                SQL += "and UserName = @p3 ";
            }

            if (!string.IsNullOrEmpty(Filtered))
            {
                SQL += "and AppointmentID like @p7 ";
            }

            if (Type == 2)
            {
                SQL += "order by EndDateTime desc ";
            }
            else
            {
                SQL += "order by case when StartDateTime >= @p2 and ActualEndDateTime = @p6 and Status != 3 then 0 else 1 end, StartDateTime asc ";
            }

            if (!IsCount)
            {
                //if (!IsExportExcel)
                //{
                //    SQL += "limit 20 offset " + (PageIndex * 20);
                //}
            }

            if (Type != 2)
            {
                FilteredStartDateTime = DateTime.Now;
                FilteredEndDateTime = FilteredStartDateTime.AddDays(1).AddSeconds(-1);
            }
            else
            {
                FilteredStartDateTime = DateTime.Parse(Request.Form[txtFromDate.UniqueID]);
                FilteredEndDateTime = DateTime.Parse(Request.Form[txtToDate.UniqueID]).AddDays(1).AddSeconds(-1);
            }

            return SQL;
        }

        protected void ActionButton(object sender, EventArgs e)
        {
            string AppointmentID = (sender as Button).CommandArgument.ToString().Trim();
            AppointmentStatus Status = AppointmentStatus.Awaiting;

            if ((sender as Button).Text == "Check In")
            {
                Status = AppointmentStatus.CheckedIn;
                PerformActionBasedOnStatus(AppointmentID, Status);
            }
            else if ((sender as Button).Text == "Check Out")
            {
                Status = AppointmentStatus.CheckedOut;
                PerformActionBasedOnStatus(AppointmentID, Status);
            }
            else if ((sender as Button).Text == "Cancel")
            {
                Status = AppointmentStatus.Canceled;
                PerformActionBasedOnStatus(AppointmentID, Status);
            }

            GenerateReport(Int32.Parse(CurrentSelectedType.Text), "");

            Function_Method.MsgBox("Update Successfully.", this.Page, this);

        }

        private void PerformActionBasedOnStatus(string AppointmentID, AppointmentStatus Status)
        {
            MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);

            string SQL = "update visitor_staffappointment set ";
            string Condition = "";
            if (Status == AppointmentStatus.CheckedIn)
            {
                Status = AppointmentStatus.CheckedIn;
                SQL += "ActualStartDateTime = @p1, ";
                Condition = "and ActualStartDateTime = @p2 ";
            }
            else if (Status == AppointmentStatus.CheckedOut)
            {
                Status = AppointmentStatus.CheckedOut;
                SQL += "ActualEndDateTime = @p1, ";
                Condition = "and ActualEndDateTime = @p2 ";
            }
            //else if (Status == AppointmentStatus.Cancelled)
            //{

            //}

            SQL += "Status = @p3 where AppointmentID = @p0 " + Condition;


            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            Dictionary<string, object> ParamDict = new Dictionary<string, object>
            {
                { "@p0", AppointmentID },
                { "@p1", DateTime.Now },
                { "@p2", DateTime.MinValue },
                { "@p3", Status },
            };

            conn.Open();
            GlobalHelper.PumpParamQuery(cmd, ParamDict);

            cmd.ExecuteNonQuery();

            conn.Close();
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            GridView gv = new GridView();
            gv = gvAppointmentReport;
            string FileName = " Appointment Report (" + ddlStatus.SelectedItem.Text + " Status) - " + DateTime.Now + ".xls";

            GlobalHelper.ExportFromGridView(gv, FileName);

            //Response.Clear();

            //DataTable dt = ConvertIntoDataTable();
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Use NonCommercial for non-commercial use

            //using (ExcelPackage excelPackage = new ExcelPackage())
            //{
            //    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Appointment Report");

            //    // Load the datatable into the worksheet, starting from cell A1
            //    worksheet.Cells["A1"].LoadFromDataTable(dt, true);

            //    // Auto-fit the columns for all cells
            //    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            //    // Create a memory stream to write to Excel
            //    using (MemoryStream memoryStream = new MemoryStream())
            //    {
            //        excelPackage.SaveAs(memoryStream);
            //        memoryStream.Position = 0;
            //        string fileName = "Appointment Report - " + DateTime.Now + ".xlsx";

            //        // Clear response stream and write the file to output
            //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            //        memoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        Response.End();
            //    }
            //}

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Charset = "";
            //string fileName = "Appointment Report - " + DateTime.Now + ".xlsx";

            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");

            //using (StringWriter sw = new StringWriter())
            //{
            //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            //    {
            //        // Start table
            //        hw.RenderBeginTag(HtmlTextWriterTag.Table); // <table>

            //        // Add headers
            //        hw.RenderBeginTag(HtmlTextWriterTag.Thead); // <thead>
            //        hw.RenderBeginTag(HtmlTextWriterTag.Tr);    // <tr>
            //        foreach (DataColumn column in dt.Columns)
            //        {
            //            hw.RenderBeginTag(HtmlTextWriterTag.Th); // <th>
            //            hw.Write(column.ColumnName);
            //            hw.RenderEndTag();                       // </th>
            //        }
            //        hw.RenderEndTag();                           // </tr>
            //        hw.RenderEndTag();                           // </thead>

            //        // Add rows
            //        hw.RenderBeginTag(HtmlTextWriterTag.Tbody); // <tbody>
            //        foreach (DataRow row in dt.Rows)
            //        {
            //            hw.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            //            foreach (var item in row.ItemArray)
            //            {
            //                hw.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
            //                hw.Write(item.ToString());
            //                hw.RenderEndTag();                       // </td>
            //            }
            //            hw.RenderEndTag();                           // </tr>
            //        }
            //        hw.RenderEndTag();                              // </tbody>

            //        hw.RenderEndTag();                              // </table>

            //        // Write the output to the response
            //        Response.Write(sw.ToString());
            //        Response.Flush();
            //        Response.End(); // Send the response
        }

        //Can remove
        //private DataTable ConvertIntoDataTable()
        //{
        //    MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
        //    DataTable tb = new DataTable();

        //    string format = DateTime.Now.ToString("yyyyMM");
        //    string Filtered = TxtSearchAppointment.Text.Trim();

        //    string AppointmentID = format + 1;
        //    DateTime FilteredStartDateTime = DateTime.Now;
        //    DateTime FilteredEndDateTime = FilteredStartDateTime;

        //    string Query = GetAppointmentQuery(2, true, Filtered, 0, out FilteredStartDateTime, out FilteredEndDateTime, false, true);
        //    DateTime StartDateToday = DateTime.Now.Date;
        //    DateTime EndDateToday = StartDateToday.AddDays(1).AddSeconds(-1);
        //    DateTime DateTimeNow = DateTime.Now.AddMinutes(-30);

        //    MySqlCommand cmd = new MySqlCommand(Query, conn);

        //    Dictionary<string, object> ParamDict = new Dictionary<string, object>
        //            {
        //                { "@p0", StartDateToday },
        //                { "@p1", EndDateToday },
        //                { "@p2", DateTimeNow },
        //                { "@p3", Session["UserName"].ToString() },
        //                { "@p4", FilteredStartDateTime },
        //                { "@p5", FilteredEndDateTime },
        //                { "@p6", DateTime.MinValue },
        //                { "@p7", Filtered + "%" },
        //                { "@p8", StartDateToday.AddDays(1) },
        //                { "@p9", ddlStatus.SelectedValue },

        //            };
        //    GlobalHelper.PumpParamQuery(cmd, ParamDict);

        //    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //    adapter.Fill(tb);

        //    return tb;
        //}

        public enum HistoryType
        {
            Today = 0,
            Upcoming = 1,
            Past = 2,
        }
    }
}