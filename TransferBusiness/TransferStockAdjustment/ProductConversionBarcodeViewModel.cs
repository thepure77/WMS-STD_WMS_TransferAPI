using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{

    public class ProductConversionBarcodeViewModel
    {

        [Key]
        public Guid ProductConversionBarcodeIndex { get; set; }

        public Guid ProductConversionIndex { get; set; }

        public Guid ProductIndex { get; set; }

        public Guid OwnerIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductConversionBarcodeId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductConversionBarcode { get; set; }

        public int IsActive { get; set; }

        public int IsDelete { get; set; }

        public int IsSystem { get; set; }

        public int StatusId { get; set; }

        [Required]
        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

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
