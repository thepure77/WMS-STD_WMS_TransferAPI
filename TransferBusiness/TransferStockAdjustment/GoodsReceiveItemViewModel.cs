using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransferBusiness.Transfer
{
    public class GoodsReceiveItemViewModel
    {
        [Key]
        public Guid goodsReceiveItem_Index { get; set; }

        public Guid? goodsReceive_Index { get; set; }

        [StringLength(50)]
        public string lineNum { get; set; }

        public Guid? product_Index { get; set; }

        [StringLength(50)]
        public string product_Id { get; set; }

        [StringLength(200)]
        public string product_Name { get; set; }

        [StringLength(200)]
        public string productSecond_Name { get; set; }

        [StringLength(200)]
        public string productThird_Name { get; set; }

        [StringLength(50)]
        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }

        [StringLength(50)]
        public string itemStatus_Id { get; set; }

        [StringLength(200)]
        public string itemStatus_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal qtyPlan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? total_Qty { get; set; }

        public Guid? productConversion_Index { get; set; }

       
        [StringLength(50)]
        public string productConversion_Id { get; set; }

        [StringLength(200)]
        public string productConversion_Name { get; set; }

        public Guid pallet_Index { get; set; }

        [Column(TypeName = "date")]
        public DateTime? mfg_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? exp_Date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitWeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitWidth { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitLength { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? nitHeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? nitVolume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? volume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitPrice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price { get; set; }
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
        public string udf_1 { get; set; }

        [StringLength(200)]
        public string udf_2 { get; set; }

        [StringLength(200)]
        public string udf_3 { get; set; }

        [StringLength(200)]
        public string udf_4 { get; set; }

        [StringLength(200)]
        public string udf_5 { get; set; }

        public Guid ref_Process_Index { get; set; }

        public string ref_Document_No { get; set; }

        public string ref_Document_LineNum { get; set; }
        
        public Guid? ref_Document_Index { get; set; }

        public Guid? ref_DocumentItem_Index { get; set; }


        [StringLength(200)]
        public string goodsReceive_Remark { get; set; }

        [StringLength(200)]
        public string goodsReceive_DockDoor { get; set; }

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

        

    }
}
