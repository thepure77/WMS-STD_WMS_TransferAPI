using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class View_BinBalance
    {
        [Key]
        public long Row_Index { get; set; }

        public Guid Owner_Index { get; set; }

        public Guid Tag_Index { get; set; }

        public Guid BinBalance_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }

        [StringLength(200)]
        public string Owner_Name { get; set; }
       
        public Guid Location_Index { get; set; }

        [StringLength(50)]
        public string Location_Id { get; set; }

        [StringLength(200)]
        public string Location_Name { get; set; }      

        public Guid Product_Index { get; set; }

        [StringLength(50)]
        public string Product_Id { get; set; }

        [StringLength(200)]
        public string Product_Name { get; set; }

        public Guid ItemStatus_Index { get; set; }

        [StringLength(50)]
        public string ItemStatus_Id { get; set; }

        [StringLength(200)]
        public string ItemStatus_Name { get; set; }

        public Guid ProductConversion_Index { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? GoodsReceive_EXP_Date { get; set; }

        public string Create_By { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public decimal? BinBalance_QtyReserve { get; set; }

        public decimal? BinBalance_Ratio { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        

    }
}
