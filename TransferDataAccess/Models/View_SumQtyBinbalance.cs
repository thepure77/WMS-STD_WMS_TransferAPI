using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{


    public partial class View_SumQtyBinbalance
    {
        [Key]
        public long? RowIndex { get; set; }

        public string Location_Name { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public Guid? Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }


        public Guid Warehouse_Index { get; set; }

        public string Warehouse_Id { get; set; }

        public string Warehouse_Name { get; set; }

        public Guid Owner_Index { get; set; }
        public Guid ProductConversion_Index { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }
    }
}
