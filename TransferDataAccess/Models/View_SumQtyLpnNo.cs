using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class View_SumQtyLpnNo
    {
        [Key]
        public long? RowIndex { get; set; }

        public string Tag_No { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public Guid Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public Guid Owner_Index { get; set; }

        public string Location_Id { get; set; }

        public string Location_Name { get; set; }

        public Guid Location_Index { get; set; }

        public decimal? BinBalance_QtyReserve { get; set; }
    }
}
