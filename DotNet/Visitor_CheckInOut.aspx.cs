using GLOBAL_FUNCTION;
using GLOBAL_VAR;
using MySql.Data.MySqlClient;
using NLog.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DotNet.Visitor_Model.AppointmentModel;

namespace DotNet
{
    public partial class Visitor_CheckInOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CheckAppointmentID(object sender, EventArgs e)
        {
            string AppointmentID = txtAppointmentID.Text.Trim();
            //string FullName = txtFullName.Text.Trim();

            if (!string.IsNullOrEmpty(AppointmentID))
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();

                string SQL = "select * from visitor_staffappointment where AppointmentID = @p0";

                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                cmd.Parameters.AddWithValue("@p0", AppointmentID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0) != DBNull.Value)
                        {
                            if (DateTime.Parse(reader.GetValue(8).ToString()) == DateTime.MinValue && reader.GetValue(14).ToString() != "3") //Cancel status
                            {
                                HideWrapper(true, true);
                            }
                            else if (DateTime.Parse(reader.GetValue(8).ToString()) != DateTime.MinValue && DateTime.Parse(reader.GetValue(9).ToString()) == DateTime.MinValue)
                            {
                                HideWrapper(true, false);
                            }
                            else if (reader.GetValue(14).ToString() == "3")
                            {
                                Function_Method.MsgBox("This appointment has been canceled.", this.Page, this);

                                HideWrapper(false, false);
                            }
                            else if (reader.GetValue(14).ToString() == "2")
                            {
                                Function_Method.MsgBox("This appointment has been checked out.", this.Page, this);

                                HideWrapper(false, false);
                            }
                            //else
                            //{
                            //    HideWrapper(true, false, true);
                            //}
                            conn.Close();

                            return;
                        }
                    }
                    Function_Method.MsgBox("Appointment Not Found.", this.Page, this);

                }
                conn.Close();

                return;
            }
            Function_Method.MsgBox("Please key in your Appointment ID.", this.Page, this);

        }

        private void HideWrapper(bool IsExist, bool IsCheckIn, bool IsCompleted = false)
        {
            CheckAppointmentWrapper.Visible = false;
            PerformActionWrapper.Visible = false;
            BtnCancelAppointment.Visible = false;
            BtnCheckInOut.Text = "Check In";
            CompletedWrapper.Visible = false;

            if (IsCompleted)
            {
                CompletedWrapper.Visible = true;

                return;
            }

            if (IsExist)
            {
                PerformActionWrapper.Visible = true;
            }

            if (IsCheckIn)
            {
                BtnCheckInOut.Text = "Check In";
                BtnCancelAppointment.Visible = true;
            }
            else
            {
                BtnCheckInOut.Text = "Check Out";
            }

            if (IsExist == false && IsCheckIn == false)
            {
                txtAppointmentID.Text = "";
                //txtFullName.Text = "";
                CheckAppointmentWrapper.Visible = true;
            }
        }

        protected void ActionButton_Click(object sender, EventArgs e)
        {
            string AppointmentID = txtAppointmentID.Text.Trim();
            string CurrentAction = "";
            if (!string.IsNullOrEmpty(AppointmentID))
            {
                MySqlConnection conn = new MySqlConnection(GLOBAL.connStr);
                conn.Open();
                string SQL = "";
                string TableName = "visitor_staffappointment";
                List<string> ColumnList = new List<string>();
                Dictionary<string, object> ParamDict = new Dictionary<string, object>();
                string Condition = "where AppointmentID = @c1";
                AppointmentStatus Status = AppointmentStatus.Awaiting;
                if ((sender as Button).Text == "Yes")
                {
                    ColumnList.Add("Status");
                    Status = AppointmentStatus.Canceled;
                    CurrentAction = "Canceled";

                    HideWrapper(false, false);
                }
                else
                {
                    if (BtnCheckInOut.Text == "Check In")
                    {
                        ColumnList.Add("Status");
                        ColumnList.Add("ActualStartDateTime");
                        Status = AppointmentStatus.CheckedIn;
                        CurrentAction = "Checked In";

                        HideWrapper(true, false);
                    }
                    else if (BtnCheckInOut.Text == "Check Out")
                    {
                        ColumnList.Add("Status");
                        ColumnList.Add("ActualEndDateTime");
                        Status = AppointmentStatus.CheckedOut;
                        CurrentAction = "Checked Out";

                        HideWrapper(true, false, true);
                    }
                }
                List<object> ObjectList = new List<object> { (int)Status, DateTime.Now.ToString(GLOBAL.gDisplayDateTimeFormat) };
                ParamDict = GlobalHelper.ConvertModelColumnsIntoDictionary(ColumnList, ObjectList);

                MySqlCommand cmd = GlobalHelper.UpdateQuery(TableName, ColumnList, ParamDict, Condition);
                cmd.Parameters.AddWithValue("@c1", AppointmentID);

                cmd.ExecuteNonQuery();
                conn.Close();
                Function_Method.MsgBox(CurrentAction + " Successfully.", this.Page, this);

            }
            else
            {
                Function_Method.MsgBox("Appointment not found, please try again.", this.Page, this);
            }
        }
    }
}