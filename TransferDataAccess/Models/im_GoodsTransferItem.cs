using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
 

    public partial class IM_GoodsTransferItem
    {
        [Key]
        public Guid GoodsTransferItem_Index { get; set; }
        public Guid GoodsTransfer_Index { get; set; }
        public Guid? GoodsReceive_Index { get; set; }
        public Guid? GoodsReceiveItem_Index { get; set; }
        public Guid? GoodsReceiveItemLocation_Index { get; set; }
        public string LineNum { get; set; }
        public Guid? TagItem_Index { get; set; }
        public Guid? Tag_Index { get; set; }
        public string Tag_No { get; set; }
        public Guid? Tag_Index_To { get; set; }
        public Guid? Product_Index { get; set; }
        public Guid? Product_Index_To { get; set; }
        public string Product_Lot { get; set; }
        public string Product_Lot_To { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public Guid? ItemStatus_Index_To { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public Guid? Owner_Index { get; set; }
        public Guid? Owner_Index_To { get; set; }
        public Guid? Location_Index { get; set; }
        public Guid? Location_Index_To { get; set; }
        public DateTime? GoodsReceive_MFG_Date { get; set; }
        public DateTime? GoodsReceive_MFG_Date_To { get; set; }
        public DateTime? GoodsReceive_EXP_Date { get; set; }
        public DateTime? GoodsReceive_EXP_Date_To { get; set; }
        public decimal Qty { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalQty { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
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
        public Guid? Ref_Document_Index { get; set; }
        public Guid? Ref_DocumentItem_Index { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Cancel_By { get; set; }
        public DateTime? Cancel_Date { get; set; }
        public string Tag_No_To { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_SecondName { get; set; }
        public string Product_ThirdName { get; set; }
        public string Product_Id_To { get; set; }
        public string Product_Name_To { get; set; }
        public string Product_SecondName_To { get; set; }
        public string Product_ThirdName_To { get; set; }
        public string ItemStatus_Id { get; set; }
        public string ItemStatus_Name { get; set; }
        public string ItemStatus_Id_To { get; set; }
        public string ItemStatus_Name_To { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public string Owner_Id_To { get; set; }
        public string Owner_Name_To { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public string Location_Id_To { get; set; }
        public string Location_Name_To { get; set; }
        public string Mat_Doc { get; set; }
        public string FI_Doc { get; set; }
        public string ERP_Location { get; set; }
        public string ERP_Location_To { get; set; }

    }
}
