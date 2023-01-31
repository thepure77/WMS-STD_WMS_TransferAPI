using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.ConfigModel
{
    public class GoodsReceiveTagItemViewModel
    {
        [Key]
        public Guid tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }

        [StringLength(50)]
        public string tag_No { get; set; }

        public Guid goodsReceive_Index { get; set; }

        public Guid? goodsReceiveItem_Index { get; set; }

        public Guid? process_Index { get; set; }

        public Guid product_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string product_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string product_Name { get; set; }

        [StringLength(200)]
        public string product_SecondName { get; set; }

        [StringLength(200)]
        public string product_ThirdName { get; set; }

        [StringLength(50)]
        public string product_Lot { get; set; }

        public Guid itemStatus_Index { get; set; }

        [StringLength(50)]
        public string itemStatus_Id { get; set; }

        [StringLength(200)]
        public string itemStatus_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal productConversion_Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal totalQty { get; set; }

        public Guid productConversion_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string productConversion_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string productConversion_Name { get; set; }


        public decimal? weight { get; set; }


        public decimal? unitWeight { get; set; }


        public decimal? netWeight { get; set; }

        public Guid? weight_Index { get; set; }


        public string weight_Id { get; set; }


        public string weight_Name { get; set; }


        public decimal? weightRatio { get; set; }


        public decimal? unitGrsWeight { get; set; }


        public decimal? grsWeight { get; set; }

        public Guid? grsWeight_Index { get; set; }


        public string grsWeight_Id { get; set; }


        public string grsWeight_Name { get; set; }


        public decimal? grsWeightRatio { get; set; }


        public decimal? unitWidth { get; set; }


        public decimal? width { get; set; }

        public Guid? width_Index { get; set; }


        public string width_Id { get; set; }


        public string width_Name { get; set; }


        public decimal? widthRatio { get; set; }


        public decimal? unitLength { get; set; }


        public decimal? length { get; set; }

        public Guid? length_Index { get; set; }


        public string length_Id { get; set; }


        public string length_Name { get; set; }


        public decimal? lengthRatio { get; set; }


        public decimal? unitHeight { get; set; }


        public decimal? height { get; set; }

        public Guid? height_Index { get; set; }


        public string height_Id { get; set; }


        public string height_Name { get; set; }


        public decimal? heightRatio { get; set; }


        public decimal? unitVolume { get; set; }


        public decimal? volume { get; set; }

        public Guid? volume_Index { get; set; }


        public string volume_Id { get; set; }


        public string volume_Name { get; set; }


        public decimal? volumeRatio { get; set; }


        public decimal? unitPrice { get; set; }


        public decimal? price { get; set; }


        public decimal? totalPrice { get; set; }

        public Guid? currency_Index { get; set; }


        public string currency_Id { get; set; }


        public string currency_Name { get; set; }



        [Column(TypeName = "date")]
        public string mfg_Date { get; set; }

        [Column(TypeName = "date")]
        public string exp_Date { get; set; }

        [Column(TypeName = "date")]
        public string exp_Date_To { get; set; }

        [Column(TypeName = "date")]
        public string mfg_Date_To { get; set; }

        [StringLength(200)]
        public string tagRef_No1 { get; set; }

        [StringLength(200)]
        public string tagRef_No2 { get; set; }

        [StringLength(200)]
        public string tagRef_No3 { get; set; }

        [StringLength(200)]
        public string tagRef_No4 { get; set; }

        [StringLength(200)]
        public string tagRef_No5 { get; set; }

        public int? tag_Status { get; set; }

        [StringLength(200)]
        public string udf_1 { get; set; }

        [StringLength(200)]
        public string udf_2 { get; set; }

        [StringLength(200)]
        public string udf_3 { get; set; }

        [StringLength(200)]
        public string udf_4 { get; set; }

        [StringLength(200)]
        public string udf_5 { get; set; }

        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string cancel_Date { get; set; }

        public Guid planGoodsReceive_Index { get; set; }

        public Guid planGoodsReceiveItem_Index { get; set; }

        public Guid owner_Index { get; set; }

        [StringLength(50)]
        public string owner_Id { get; set; }

        [StringLength(50)]
        public string owner_Name { get; set; }

        public Guid documentType_Index { get; set; }

        [StringLength(50)]
        public string documentType_Id { get; set; }

        [StringLength(200)]
        public string documentType_Name { get; set; }

        [StringLength(50)]
        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }



        [StringLength(200)]
        public string documentRef_No1 { get; set; }

        [StringLength(200)]
        public string documentRef_No2 { get; set; }

        [StringLength(200)]
        public string documentRef_No3 { get; set; }

        [StringLength(200)]
        public string documentRef_No4 { get; set; }

        [StringLength(200)]
        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }

        [StringLength(200)]
        public string document_Remark { get; set; }

        [StringLength(200)]
        public string udf1 { get; set; }

        [StringLength(200)]
        public string udf2 { get; set; }

        [StringLength(200)]
        public string udf3 { get; set; }

        [StringLength(200)]
        public string udf4 { get; set; }

        [StringLength(200)]
        public string udf5 { get; set; }

        public int? documentPriority_Status { get; set; }

        public int? putaway_Status { get; set; }

        public Guid? warehouse_Index { get; set; }

        [StringLength(50)]
        public string warehouse_Id { get; set; }

        [StringLength(200)]
        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public Guid? dockDoor_Index { get; set; }

        [StringLength(50)]
        public string dockDoor_Id { get; set; }

        [StringLength(200)]
        public string dockDoor_Name { get; set; }

        public Guid? vehicleType_Index { get; set; }

        [StringLength(50)]
        public string vehicleType_Id { get; set; }

        [StringLength(200)]
        public string vehicleType_Name { get; set; }

        public Guid? containerType_Index { get; set; }

        [StringLength(50)]
        public string containerType_Id { get; set; }

        [StringLength(200)]
        public string containerType_Name { get; set; }

        public string planGoodsReceive_Date { get; set; }

        public QtyGenTagViewModel qtyGenTag { get; set; }

        public Guid? suggest_Location_Index { get; set; }


        public string suggest_Location_Id { get; set; }


        public string suggest_Location_Name { get; set; }
        public string erp_Location { get; set; }

        
    }

    public class QtyGenTagViewModel
    {
        public decimal qty { get; set; }

        public decimal volume { get; set; }

        public decimal weight { get; set; }

        public decimal qtyPerTag { get; set; }

        public decimal qtyOfTag { get; set; }
    }
}
