using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class TBL_IF_WMS_PP_RETRIEVAL
    {
        [Key]
        public string TransactionID { get; set; }

        public short OrderType { get; set; }

        public string WaveNo { get; set; }

        public int TotalLineNo { get; set; }

        public int LineNo { get; set; }

        public string PalletID { get; set; }

        public short SourceStation { get; set; }

        public short Status { get; set; }

        public short Priority { get; set; }

        public string SKU { get; set; }

        public short RobotGroup { get; set; }

        public int IssueQuantity { get; set; }

        public short IsFullPalletOut { get; set; }

        public short? WCSErrorCode { get; set; }

        public string RouteID { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int DeliveryRnd { get; set; }

        public string IssueType { get; set; }

        public short? WMSDestStation { get; set; }

        public DateTime UpdateDT { get; set; }

        public string UpdateBy { get; set; }
    }
}
