using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class IM_GoodsReceiveItem
    {
        //[Key]
        //public Guid GoodsReceiveItem_Index { get; set; }

        //public Guid GoodsReceive_Index { get; set; }

        //[StringLength(50)]
        //public string LineNum { get; set; }

        //public Guid Product_Index { get; set; }

        //[StringLength(50)]
        //public string Product_Id { get; set; }

        //[StringLength(200)]
        //public string Product_Name { get; set; }

        //[StringLength(200)]
        //public string Product_SecondName { get; set; }

        //[StringLength(200)]
        //public string Product_ThirdName { get; set; }

        //[StringLength(50)]
        //public string Product_Lot { get; set; }

        //public Guid ItemStatus_Index { get; set; }

        //[StringLength(50)]
        //public string ItemStatus_Id { get; set; }

        //[StringLength(200)]
        //public string ItemStatus_Name { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal QtyPlan { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal Qty { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal Ratio { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal TotalQty { get; set; }

        //public Guid ProductConversion_Index { get; set; }

        //[StringLength(50)]
        //public string ProductConversion_Id { get; set; }

        //[StringLength(200)]
        //public string ProductConversion_Name { get; set; }

        //public Guid Pallet_Index { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? MFG_Date { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? EXP_Date { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? UnitWeight { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? Weight { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? UnitWidth { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? UnitLength { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? UnitHeight { get; set; }

        //[Column(TypeName = "numeric")]  
        //public decimal? UnitVolume { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? Volume { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? UnitPrice { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? Price { get; set; }

        //[StringLength(200)]
        //public string DocumentRef_No1 { get; set; }

        //[StringLength(200)]
        //public string DocumentRef_No2 { get; set; }

        //[StringLength(200)]
        //public string DocumentRef_No3 { get; set; }

        //[StringLength(200)]
        //public string DocumentRef_No4 { get; set; }

        //[StringLength(200)]
        //public string DocumentRef_No5 { get; set; }

        //public int? Document_Status { get; set; }

        //[StringLength(200)]
        //public string UDF_1 { get; set; }

        //[StringLength(200)]
        //public string UDF_2 { get; set; }

        //[StringLength(200)]
        //public string UDF_3 { get; set; }

        //[StringLength(200)]
        //public string UDF_4 { get; set; }

        //[StringLength(200)]
        //public string UDF_5 { get; set; }

        //public Guid? Ref_Process_Index { get; set; }

        //[StringLength(50)]
        //public string Ref_Document_No { get; set; }

        //[StringLength(50)]
        //public string Ref_Document_LineNum { get; set; }

        //public Guid? Ref_Document_Index { get; set; }

        //public Guid? Ref_DocumentItem_Index { get; set; }

        //[StringLength(200)]
        //public string GoodsReceive_Remark { get; set; }

        //[StringLength(200)]
        //public string GoodsReceive_DockDoor { get; set; }

        //[StringLength(200)]
        //public string Create_By { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? Create_Date { get; set; }

        //[StringLength(200)]
        //public string Update_By { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? Update_Date { get; set; }

        //[StringLength(200)]
        //public string Cancel_By { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? Cancel_Date { get; set; }

        [Key]
        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? GoodsReceive_Index { get; set; }


        public string LineNum { get; set; }

        public Guid Product_Index { get; set; }


        public string Product_Id { get; set; }


        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }


        public string Product_Lot { get; set; }

        public Guid? ItemStatus_Index { get; set; }


        public string ItemStatus_Id { get; set; }


        public string ItemStatus_Name { get; set; }


        public decimal? QtyPlan { get; set; }


        public decimal? Qty { get; set; }


        public decimal? Ratio { get; set; }


        public decimal? TotalQty { get; set; }

        public Guid ProductConversion_Index { get; set; }


        public string ProductConversion_Id { get; set; }


        public string ProductConversion_Name { get; set; }

        public Guid? Pallet_Index { get; set; }

        public DateTime? MFG_Date { get; set; }

        public DateTime? EXP_Date { get; set; }


        public string DocumentRef_No1 { get; set; }


        public string DocumentRef_No2 { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string DocumentRef_No4 { get; set; }


        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }

        public Guid? Ref_Process_Index { get; set; }


        public string Ref_Document_No { get; set; }


        public string Ref_Document_LineNum { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }


        public string GoodsReceive_Remark { get; set; }


        public string GoodsReceive_DockDoor { get; set; }


        public decimal? UnitWeight { get; set; }


        public decimal? Weight { get; set; }


        public decimal? NetWeight { get; set; }

        public Guid? Weight_Index { get; set; }


        public string Weight_Id { get; set; }


        public string Weight_Name { get; set; }


        public decimal? WeightRatio { get; set; }


        public decimal? UnitGrsWeight { get; set; }


        public decimal? GrsWeight { get; set; }

        public Guid? GrsWeight_Index { get; set; }


        public string GrsWeight_Id { get; set; }


        public string GrsWeight_Name { get; set; }


        public decimal? GrsWeightRatio { get; set; }


        public decimal? UnitWidth { get; set; }


        public decimal? Width { get; set; }

        public Guid? Width_Index { get; set; }


        public string Width_Id { get; set; }


        public string Width_Name { get; set; }


        public decimal? WidthRatio { get; set; }


        public decimal? UnitLength { get; set; }


        public decimal? Length { get; set; }

        public Guid? Length_Index { get; set; }


        public string Length_Id { get; set; }


        public string Length_Name { get; set; }


        public decimal? LengthRatio { get; set; }


        public decimal? UnitHeight { get; set; }


        public decimal? Height { get; set; }

        public Guid? Height_Index { get; set; }


        public string Height_Id { get; set; }


        public string Height_Name { get; set; }


        public decimal? HeightRatio { get; set; }


        public decimal? UnitVolume { get; set; }


        public decimal? Volume { get; set; }

        public Guid? Volume_Index { get; set; }


        public string Volume_Id { get; set; }


        public string Volume_Name { get; set; }


        public decimal? VolumeRatio { get; set; }


        public decimal? UnitPrice { get; set; }


        public decimal? Price { get; set; }


        public decimal? TotalPrice { get; set; }

        public Guid? Currency_Index { get; set; }


        public string Currency_Id { get; set; }


        public string Currency_Name { get; set; }


        public string Ref_Code1 { get; set; }


        public string Ref_Code2 { get; set; }


        public string Ref_Code3 { get; set; }


        public string Ref_Code4 { get; set; }


        public string Ref_Code5 { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public string Invoice_No { get; set; }

        public string Declaration_No { get; set; }
        public string HS_Code { get; set; }
        public string Conutry_of_Origin { get; set; }

        public decimal? Tax1 { get; set; }
        public Guid? Tax1_Currency_Index { get; set; }
        public string Tax1_Currency_Id { get; set; }
        public string Tax1_Currency_Name { get; set; }

        public decimal? Tax2 { get; set; }
        public Guid? Tax2_Currency_Index { get; set; }
        public string Tax2_Currency_Id { get; set; }
        public string Tax2_Currency_Name { get; set; }

        public decimal? Tax3 { get; set; }
        public Guid? Tax3_Currency_Index { get; set; }
        public string Tax3_Currency_Id { get; set; }
        public string Tax3_Currency_Name { get; set; }

        public decimal? Tax4 { get; set; }
        public Guid? Tax4_Currency_Index { get; set; }
        public string Tax4_Currency_Id { get; set; }
        public string Tax4_Currency_Name { get; set; }

        public decimal? Tax5 { get; set; }
        public Guid? Tax5_Currency_Index { get; set; }
        public string Tax5_Currency_Id { get; set; }
        public string Tax5_Currency_Name { get; set; }
        public string ERP_Location { get; set; }
    }
}
