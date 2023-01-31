using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{

    public class TransferStockAdjustmentItemViewModel
    {
        [Key]
        public Guid StockAdjustmentItemItemIndex { get; set; }

        public Guid StockAdjustmentIndex { get; set; }

        public Guid? GoodsReceiveIndex { get; set; }

        public Guid? GoodsReceiveItemIndex { get; set; }

        public Guid? GoodsReceiveItemLocationIndex { get; set; }

        [StringLength(50)]
        public string LineNum { get; set; }

        public Guid? TagItemIndex { get; set; }

        public Guid? TagIndex { get; set; }

        public Guid? TagIndexTo { get; set; }

        public Guid? ProductIndex { get; set; }

        public Guid? ProductIndexTo { get; set; }

        [StringLength(50)]
        public string ProductLot { get; set; }

        [StringLength(50)]
        public string ProductLotTo { get; set; }

        public Guid? ItemStatusIndex { get; set; }

        public Guid? ItemStatusIndexTo { get; set; }

        public Guid? ProductConversionIndex { get; set; }

        public Guid? OwnerIndex { get; set; }

        public Guid? OwnerIndexTo { get; set; }

        public Guid? LocationIndex { get; set; }

        public Guid? LocationIndexTo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GoodsReceiveEXPDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GoodsReceiveEXPDateTo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Volume { get; set; }

        public Guid? RefProcessIndex { get; set; }

        public Guid? RefDocumentNo { get; set; }

        public Guid? RefDocumentIndex { get; set; }

        public Guid? RefDocumentItemIndex { get; set; }

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        [StringLength(200)]
        public string CancelBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CancelDate { get; set; }


    }
}
