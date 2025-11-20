using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNet.SFAModel
{
    public class RedemptionModel
    {
        public string CustAcc { get; set; }
        public string CustPhone { get; set; }
        public string CustContact { get; set; }
        public string CustAddr { get; set; }
        public string benefitName { get; set; }
        public string benefitIc { get; set; }
        public string benefitTaxNo { get; set; }
        public string reason { get; set; }
        public int interestWaiver { get; set; }
        public string time { get; set; }
        public string date { get; set; }
        public string ap { get; set; }
        public string lpDate { get; set; }
        public string CreatedAP { get; set; }
        public string CreatedLP { get; set; }
    }
}