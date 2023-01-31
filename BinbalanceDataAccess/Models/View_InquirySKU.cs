using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{
    public partial class View_InquirySKU
    {
        [Key]
        public long RowIndex { get; set; }

        public Guid? Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string Product_SecondName { get; set; }

        public string Location_Name { get; set; }

        public string GoodsReceive_No { get; set; }

        public string ReceivingRef { get; set; }

        public DateTime? GoodsReceive_Date { get; set; }

        public string Tag_No { get; set; }

        public string ItemStatus_Name { get; set; }

        public DateTime? MFG_Date { get; set; }

        public DateTime? EXP_Date { get; set; }

        public string ProductConversion_Name { get; set; }

        public decimal StockONHand { get; set; }

        public decimal StockAllocated { get; set; }

        public decimal StockAvailable { get; set; }

    }
}
