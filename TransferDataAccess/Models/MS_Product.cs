using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class MS_Product
    {
        [Key]
        public Guid Product_Index { get; set; }

        [StringLength(50)]
        public string Product_Id { get; set; }

        [StringLength(200)]
        public string Product_Name { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        [StringLength(200)]
        public string Product_SecondName { get; set; }

        [StringLength(200)]
        public string Product_ThirdName { get; set; }

        public Guid ProductCategory_Index { get; set; }

        public Guid ProductType_Index { get; set; }

        public Guid ProductSubType_Index { get; set; }

        public Guid ProductConversion_Index { get; set; }

        public int? ProductItemLife_Y { get; set; }

        public int? ProductItemLife_M { get; set; }

        public int? ProductItemLife_D { get; set; }

        [StringLength(500)]
        public string ProductImage_Path { get; set; }

        public int? IsLot { get; set; }

        public int? IsExpDate { get; set; }
        public int? IsMfgDate { get; set; }

        public int? IsCatchWeight { get; set; }

        public int? IsPack { get; set; }

        public int? IsSerial { get; set; }

        public int IsActive { get; set; }

        public int IsDelete { get; set; }

        public int IsSystem { get; set; }

        public int Status_Id { get; set; }

        [Required]
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
