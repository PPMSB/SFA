using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DotNet.CampaignModel
{
    public class CampaignModel
    {
        public class CampaignDocumentModel
        {
            //[Key]
            //public int ID { get; set; }
            public string CampaignID { get; set; }
            public int SequenceNo { get; set; }
            public string CustomerAccount { get; set; }
            public string WorkshopName { get; set; }
            public string Salesman { get; set; }
            public string SalesmanID { get; set; }
            public string CampaignTerms { get; set; }
            public DateTime CampaignStartDate { get; set; }
            public DateTime CampaignEndDate { get; set; }
            public double PastYearSales { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            public DateTime UpdatedDateTime { get; set; }
            public string UpdatedBy { get; set; }
            public DocumentStatus Status { get; set; }
            public int FileID { get; set; }
            public DateTime DocumentRecDate { get; set; }

        }

        public class CampaignDocumentViewModel
        {
            //[Key]
            public int ID { get; set; }
            public string CampaignID { get; set; }
            public int SequenceNo { get; set; }
            public string Target { get; set; }
            public string CustomerAccount { get; set; }
            public string WorkshopName { get; set; }
            public string Salesman { get; set; }
            public string SalesmanID { get; set; }
            public string CampaignTerms { get; set; }
            public DateTime CampaignStartDate { get; set; }
            public DateTime CampaignEndDate { get; set; }
            public double PastYearSales { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            public DateTime UpdatedDateTime { get; set; }
            public string UpdatedBy { get; set; }
            public DocumentStatus Status { get; set; }
            public int FileID { get; set; }
            public DateTime DocumentRecDate { get; set; }
            public string RejectRemarks { get; set; }
            public bool IsOriReceive { get; set; }

        }

        public class CampaignTargetHeaderModel
        {
            [Key]
            public string TargetID { get; set; }
            public string CampaignID { get; set; }
            public string CustomerAccount { get; set; }
            public double TargetAmount { get; set; }

        }

        public class CampaignTargetPercentModel
        {
            [Key]
            public string ID { get; set; }
            public string RefTargetID { get; set; }
            public string CampaignID { get; set; }
            public string CustomerAccount { get; set; }
            public string Product { get; set; }
            public double Percent { get; set; }
        }

        public class CampaignTargetPercentGridViewModel
        {
            public string TargetAmount { get; set; }
            public string ProductA { get; set; }
            public string ProductB { get; set; }
            public string ProductC { get; set; }
            public string ProductD { get; set; }
            public string ProductE { get; set; }

        }

        public class CustomerTargetModel
        {
            public string CustomerAccount { get; set; }
            public double CampaignTarget { get; set; }
            public double SecondLevelTarget { get; set; }
        }

        public class CompiledModel
        {
            public CampaignDocumentModel cdm { get; set; }
            public List<CampaignTargetHeaderModel> cthm { get; set; }
            public List<CampaignTargetPercentModel> ctpm { get; set; }
        }


        public class FileLibraryModel
        {
            public int FileID { get; set; }
            public string FileName { get; set; }
            public int FileSize { get; set; }
            public string FileType { get; set; }
            public string UploadBy { get; set; }
            public string UploadDate { get; set; }
            public byte[] FileContent { get; set; }
            public string FileLocation { get; set; }

        }

        public class MSBCampaignDetailModel
        {
            public string CampaignID { get; set; }
            public long TargetRefRecId { get; set; }
            public string CustAccount { get; set; }
            public DateTime DateStart { get; set; }
            public string MSB_ComType { get; set; }
            public DateTime DateEnd { get; set; }
            public DateTime GrowthDateEnd { get; set; }
            public double PYSales { get; set; }
            public string CustName { get; set; }
        }

        public enum DocumentStatus
        {
            Empty = 0,
            Approved = 1,
            Canceled = 2,
            Uploaded = 3,
            Renewal = 4,
            Rejected = 5,
        }
    }

}