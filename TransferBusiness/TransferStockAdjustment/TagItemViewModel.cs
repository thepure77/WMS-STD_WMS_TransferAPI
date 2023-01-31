
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{

    public partial class TagItemViewModel
    {

        [Key]
        public Guid TagItemIndex { get; set; }

        public Guid? TagIndex { get; set; }

        [StringLength(50)]
        public string TagNo { get; set; }

        public Guid? GoodsReceiveIndex { get; set; }

        public Guid? GoodsReceiveItemIndex { get; set; }

        public Guid? ProductIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [StringLength(200)]
        public string ProductSecondName { get; set; }

        [StringLength(200)]
        public string ProductThirdName { get; set; }

        [StringLength(50)]
        public string ProductLot { get; set; }

        public Guid? ItemStatusIndex { get; set; }

        [StringLength(50)]
        public string ItemStatusId { get; set; }

        [StringLength(200)]
        public string ItemStatusName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        public Guid? ProductConversionIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductConversionId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductConversionName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Volume { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFGDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXPDate { get; set; }

        [StringLength(200)]
        public string TagRefNo1 { get; set; }

        [StringLength(200)]
        public string TagRefNo2 { get; set; }

        [StringLength(200)]
        public string TagRefNo3 { get; set; }

        [StringLength(200)]
        public string TagRefNo4 { get; set; }

        [StringLength(200)]
        public string TagRefNo5 { get; set; }

        public int? TagStatus { get; set; }

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

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
    }
}
