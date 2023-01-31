using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{

    public class TransferStockAdjustmentViewModel
    {
        [Key]
        public Guid StockAdjustmentIndex { get; set; }

        public Guid? OwnerIndex { get; set; }

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

        public Guid? ReasonCodeIndex { get; set; }

        public string ReasonCodeId { get; set; }

        public string ReasonCodeName { get; set; }
        public List<TransferStockAdjustmentItemViewModel> listTransferStockAdjustmentItemViewModel { get; set; }



    }
}
