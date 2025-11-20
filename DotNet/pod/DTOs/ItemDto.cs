using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNet.pod.DTOs
{
    public class ItemDto
    {
        public string ItemId {  get; set; }
        public string InvoiceId { get; set; }
        public string InventDimId {  get; set; }
        public double Qty { get; set; }
        public string Unit { get; set; }
        public string Location { get; set; }
    }
}