using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransferBusiness.Transfer
{

    public class TransferStockAdjustmentDocViewModel
    {
        [Key]
        public Guid StockAdjustmentIndex { get; set; }

        public Guid OwnerIndex { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }


        public Guid? DocumentTypeIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string StockAdjustmentNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime StockAdjustmentDate { get; set; }

        [StringLength(200)]
        public string DocumentRefNo1 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo2 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo3 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo4 { get; set; }

        [StringLength(200)]
        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }

        [StringLength(200)]
        public string UDF1 { get; set; }

        [StringLength(200)]
        public string UDF2 { get; set; }

        [StringLength(200)]
        public string UDF3 { get; set; }

        [StringLength(200)]
        public string UDF4 { get; set; }

        [StringLength(200)]
        public string UDF5 { get; set; }

        public int? DocumentPriorityStatus { get; set; }

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
        public Guid BinBalanceIndex { get; set; }
        public decimal? BinBalanceQtyBal { get; set; }
        public decimal? Qty { get; set; }
        public Guid WarehouseIndex { get; set; }

        public Guid StockAdjustmentItemItemIndex { get; set; }
        public Guid RefProcessIndex { get; set; }
        public Guid RefDocumentIndex { get; set; }
        public Guid? RefDocumentNo { get; set; }
        public Guid RefDocumentItemIndex { get; set; }
        public Guid GoodsReceiveItemIndex { get; set; }
        public Guid GoodsReceiveIndex { get; set; }
        public string LineNum { get; set; }
        public string ProductLot { get; set; }
        public decimal ratio { get; set; }
        public decimal? UnitWeight { get; set; }

        public decimal? Weight { get; set; }

        public decimal? UnitWidth { get; set; }

        public decimal? UnitLength { get; set; }

        public decimal? UnitHeight { get; set; }

        public decimal? UnitVolume { get; set; }

        public decimal? Volume { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Price { get; set; }

        public string RefDocumentLineNum { get; set; }
        public Guid ReasonCodeIndex { get; set; }

        public string ReasonCodeId { get; set; }

        public string ReasonCodeName { get; set; }
        public string tagNoNew { get; set; }

        public string tagOutNo { get; set; }


        public List<TransferStockAdjustmentItemViewModel> listTransferStockAdjustmentItemViewModel { get; set; }



    }
}
