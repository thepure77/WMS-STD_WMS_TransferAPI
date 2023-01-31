using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{
    public partial class View_InquirySKU_AllocatedBy
    {
        [Key]
        public long RowIndex { get; set; }

        public string GoodsIssue_No { get; set; }

        public string Ref_Document_No { get; set; }

        public DateTime? PlanGoodsIssue_Due_Date { get; set; }

        public string Route_Name { get; set; }

        public string Round_Name { get; set; }

        public string Product_Id { get; set; }

        public decimal BinCardReserve_QtyBal { get; set; }

    }
}
