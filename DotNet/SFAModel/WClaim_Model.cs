namespace DotNet.SFAModel
{
    public class BatteryInfo
    {
         // Optional, for numbering rows
        public string InvoiceId { get; set; }
        public string SerialNumberId { get; set; }
        public double LF_Bat_IR { get; set; }
        public double LF_Bat_SOC { get; set; }
        public double LF_Bat_SOH { get; set; }
        public double LF_Bat_CCA { get; set; }
        public double LF_Bat_Vol { get; set; }
        public string BatchNumber { get; set; }
        public string Remark { get; set; }
    }

    public class SalesLineStatus {
        public const int parmSalesStatus = 2;

        public const int parmReturnStatus = 2;

    }
}
