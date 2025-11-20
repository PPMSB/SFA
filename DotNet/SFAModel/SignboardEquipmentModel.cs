using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNet.SFAModel
{
    public class SignboardEquipmentModel
    {
        public int AppType { get; set; }
        public string CustAccount { get; set; }
        public string CustContact { get; set; }
        public string CustPhone { get; set; }
        public string DelPerson { get; set; }
        public string Remarks { get; set; }
        public string DeliveryTo { get; set; }
        public string Address { get; set; }
        public string AppliedBy { get; set; }
        public DateTime AppliedDate { get; set; }
        public string AppliedTime { get; set; }
        public string FormType { get; set; }
        public string DepositType { get; set; }
        public int RequestSign { get; set; }
        public string MapLocation { get; set; }
        public string MapRemark { get; set; }
        public string ServiceType { get; set; }
        public string ShopFacility { get; set; }
        public string OwnerExp { get; set; }
        public string ShopSize { get; set; }
        public string Worker { get; set; }
        public string ShopStatus { get; set; }
        public string YearEstablish { get; set; }
        public int MapTraffic { get; set; }
        public int MapSCVisible { get; set; }
        public int ImgInd { get; set; }
        public List<string> ItemDescription { get; set; }
        public List<int> ItemQty { get; set; }
        public List<string> ItemSize { get; set; }
        public int ItemCount { get; set; }
        public string ProcessStatus { get; set; }
        public int DocStatus { get; set; }
        public string NextApprover { get; set; }
        public string HODLevel { get; set; }
        public double Cost { get; set; }
        public string SubDBWorkshopName { get; set; }
        public string EmplName { get; set; }
    }

    public class SignboardEquipmentAppGroupModel
    {
        public string Type { get; set; }
        public string Company { get; set; }
        public string HOD { get; set; }
        public string HOD_2 { get; set; }
        public string HOD_3 { get; set; }
        public string Approval { get; set; }
        public string SalesAdmin { get; set; }
        public string AltSalesAdmin { get; set; }
        public string SalesAdminManager { get; set; }
        public string GM {  get; set; }
        public string HOD_4 { get; set; }
    }

    public class SignboardEquipment_Cust_AgingTableModel
    {
        /// <summary>
        /// Bank corporate guarantee amount
        /// </summary>
        public string BankCorpGuaranteeAmt { get; set; }
        /// <summary>
        /// Expiry date of bank corporate guarantee
        /// </summary>
        public string BankCorpExpiryDate { get; set; }
        /// <summary>
        /// Collection amount in the last month
        /// </summary>
        public string LastMonthCollection { get; set; }
        /// <summary>
        /// Collection amount in the current month
        /// </summary>
        public string CurrentMonthCollection { get; set; }
        /// Average payment days (first metric)
        /// </summary>
        public double AvgPaymDays { get; set; }
        /// <summary>
        /// Average payment days (second metric)
        /// </summary>
        public double AvgPaymDays2 { get; set; }
        /// <summary>
        /// Average monthly sales amount
        /// </summary>
        public double AvgMonthlySales { get; set; }
        /// <summary>
        /// Last returned check amount
        /// </summary>
        public string LastReturnCheqAmt { get; set; }
        /// <summary>
        /// Total amount of post-dated checks
        /// </summary>
        public double PostDatedChqTotal { get; set; }
        /// <summary>
        /// Flag indicating whether credit applies
        /// </summary>
        public bool ApplyCredit { get; set; }
        /// <summary>
        /// Overdue interest amount
        /// </summary>
        public double OverDueInterest { get; set; }

    }
}