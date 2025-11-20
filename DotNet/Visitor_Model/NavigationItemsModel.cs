using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace DotNet.Visitor_Model
{
    public class NavigationItemsModel
    {
        public HtmlAnchor NewAppointment {  get; set; }
        public HtmlAnchor NewAppointmentTag { get; set; }
        public HtmlAnchor AppointmentHistory { get; set; }
        public HtmlAnchor AppointmentHistoryTag { get; set; }
        public HtmlAnchor SFA { get; set; }
        public HtmlGenericControl SFATag { get; set; }

    }
}