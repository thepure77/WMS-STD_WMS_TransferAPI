using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace BinBalanceBusiness.ViewModels
{
    public class ProductViewModel 
    {

        public Guid product_Index { get; set; }

        [StringLength(50)]
        public string product_Id { get; set; }

        [StringLength(50)]
        public string productConversion_Id { get; set; }

        [StringLength(200)]
        public string product_Name { get; set; }

        [StringLength(200)]
        public string product_SecondName { get; set; }


        [StringLength(200)]
        public string product_ThirdName { get; set; }

        [StringLength(200)]
        public string productCategory_Name { get; set; }

        [StringLength(200)]
        public string productType_Name { get; set; }

        [StringLength(200)]
        public string productSubType_Name { get; set; }

        [StringLength(200)]
        public string productConversion_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? productConversion_Ratio { get; set; }

        public decimal? productConversion_Weight { get; set; }

        public decimal? productConversion_Width { get; set; }

        public decimal? productConversion_Length { get; set; }

        public decimal? productConversion_Height { get; set; }

        public decimal? productConversion_VolumeRatio { get; set; }

        public decimal? productConversion_Volume { get; set; }

        public Guid productCategory_Index { get; set; }

        public Guid productType_Index { get; set; }

        public Guid productSubType_Index { get; set; }

        public Guid productConversion_Index { get; set; }

        public int? productItemLife_Y { get; set; }

        public int? productItemLife_M { get; set; }

        public int? productItemLife_D { get; set; }

        [StringLength(500)]
        public string productImage_Path { get; set; }

        public int? isLot { get; set; }

        public int? isExpDate { get; set; }
        public int? isMfgDate { get; set; }

        public int? isCatchWeight { get; set; }
        public int? isPack { get; set; }

        public int? isSerial { get; set; }

        public int isActive { get; set; }

        public int isDelete { get; set; }

        public int isSystem { get; set; }

        public int status_Id { get; set; }


        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? cancel_Date { get; set; }

        public Guid? owner_Index { get; set; }

        public string key { get; set; }

        public string name { get; set; }

    }
}
