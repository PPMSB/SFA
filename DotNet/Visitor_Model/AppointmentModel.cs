using GLOBAL_VAR;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static DotNet.Visitor_MainMenu;

namespace DotNet.Visitor_Model
{
    public class AppointmentModel
    {
        public class VisitorAppointmentModel
        {
            [Key]
            public string AppointmentID { get; set; }
            public string UserName { get; set; }
            public string VisitorName { get; set; }
            public string VisitorCompany { get; set; }
            public string LPGCompany { get; set; }
            public int NumberOfVisitors { get; set; }
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public DateTime ActualStartDateTime { get; set; }
            public DateTime ActualEndDateTime { get; set; }
            public string VehiclePlateNo { get; set; }
            public string PersonToMeet { get; set; }
            public string Purpose { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public AppointmentStatus Status { get; set; }
            public int LocationID { get; set; }

        }

        //public class VisitorAppointmentHistoryGridColumnModel
        //{
        //    public string No { get; set; }
        //    public string BtnAction { get; set; }
        //    public string AppointmentID { get; set; }
        //    public string UserName { get; set; }
        //    public string VisitorName { get; set; }
        //    public string VisitorCompany { get; set; }
        //    public string LPGCompany { get; set; }
        //    public string NumberOfVisitors { get; set; }
        //    public string StartDateTime { get; set; }
        //    public string EndDateTime { get; set; }
        //    public string ActualStartDateTime { get; set; }
        //    public string ActualEndDateTime { get; set; }
        //    public string VehiclePlateNo { get; set; }
        //    public string PersonToMeet { get; set; }
        //    public string Purpose { get; set; }
        //    public string CreatedDateTime { get; set; }
        //    public string Status { get; set; }
        //    public string LocationName { get; set; }
        //    public string RowColor { get; set; }

        //}

        public class Appointment
        {
            public int AppointmentID { get; set; }
            public string AppointmentTitle { get; set; }
            public DateTime Date { get; set; }
            public AppointmentStatus Status { get; set; }
        }

        public enum AppointmentStatus
        {
            Awaiting = 0,
            [Display(Name = "Checked In")]
            CheckedIn = 1,
            [Display(Name = "Checked Out")]
            CheckedOut = 2,
            Canceled = 3,
        }
    }
}