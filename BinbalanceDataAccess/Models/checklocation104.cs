using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class checklocation104
    {
        [Key]
        public Guid BinBalance_Index { get; set; }

        public string Tag_No { get; set; }

        public string Product_Id { get; set; }
        
        public string Product_Name { get; set; }

        public string ProductConversion_Name { get; set; }

        public Guid Location_Index { get; set; }
        
        public string Location_Id { get; set; }
        
        public string Location_Name { get; set; }

        public decimal? BinBalance_QtyBegin { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public decimal? BinBalance_QtyReserve { get; set; }

        public Guid GoodsReceiveItemLocation_Index { get; set; }
    }
}
