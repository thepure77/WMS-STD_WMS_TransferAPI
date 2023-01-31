using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class ms_ProductConversionBarcode
    {
        [Key]
        public Guid ProductConversionBarcode_Index { get; set; }

        public Guid ProductConversion_Index { get; set; }

        public Guid Product_Index { get; set; }

        public Guid Owner_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductConversionBarcode_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductConversionBarcode { get; set; }

        public int IsActive { get; set; }

        public int IsDelete { get; set; }

        public int IsSystem { get; set; }

        public int Status_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Create_Date { get; set; }

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
