using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransferDataAccess.Models
{
    public partial class View_CartonList
    {
        [Key]

        public long? Row_Index { get; set; }

        public Guid GoodsIssue_Index { get; set; }

        public Guid BinBalance_Index { get; set; }

        public Guid GoodsIssueItem_Index { get; set; }

        public Guid Owner_Index { get; set; }


        public string Owner_Id { get; set; }


        public string Owner_Name { get; set; }

        public Guid? TagOut_Index { get; set; }

        public int? TagOut_Status { get; set; }

        public string TagOut_No { get; set; }

        //public Guid? Location_Confirm_Index { get; set; }

        //public string Location_Confirm_Id { get; set; }

        //public string Location_Confirm_Name { get; set; }

        public DateTime? MFG_Date { get; set; }


        public DateTime? EXP_Date { get; set; }


        public decimal TotalQty { get; set; }


        public decimal Qty { get; set; }


        public Guid Product_Index { get; set; }

 
        public string Product_Id { get; set; }


        public string Product_Name { get; set; }

        public Guid? ItemStatus_Index { get; set; }


        public string ItemStatus_Id { get; set; }


        public string ItemStatus_Name { get; set; }


        public Guid ProductConversion_Index { get; set; }


        public string ProductConversion_Id { get; set; }


        public string ProductConversion_Name { get; set; }


        public Guid GoodsReceive_Index { get; set; }


        public Guid GoodsReceiveItem_Index { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public decimal? BinBalance_QtyReserve { get; set; }

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

        [StringLength(200)]
        public string Create_By { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }
    }
}
