using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{

    public partial class im_TransferStockAdjustmentItem
    {
        [Key]
        public Guid? StockAdjustmentItemItem_Index { get; set; }

        public Guid StockAdjustment_Index { get; set; }

        public Guid? GoodsReceive_Index { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? GoodsReceiveItemLocation_Index { get; set; }

        [StringLength(50)]
        public string LineNum { get; set; }

        public Guid? TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }

        public Guid? Tag_Index_To { get; set; }

        public Guid? Product_Index { get; set; }

        public Guid? Product_Index_To { get; set; }

        [StringLength(50)]
        public string Product_Lot { get; set; }

        [StringLength(50)]
        public string Product_Lot_To { get; set; }

        public Guid? ItemStatus_Index { get; set; }

        public Guid? ItemStatus_Index_To { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        public Guid? Owner_Index { get; set; }

        public Guid? Owner_Index_To { get; set; }

        public Guid? Location_Index { get; set; }

        public Guid? Location_Index_To { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GoodsReceive_EXP_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GoodsReceive_EXP_Date_To { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Volume { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        public Guid? Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

    }
}
