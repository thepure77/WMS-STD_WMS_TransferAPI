using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TransferBusiness.GoodsTransfer.ViewModel;

namespace TransferBusiness.Transfer
{
    public partial class TransferBypassViewModel
    {
        public TransferBypassViewModel()
        {
            ItemModel = new List<TransferBypassViewModel>();
        }

        public Guid? BinBalance_Index { get; set; }
        public string Tag_No { get; set; }
        public Guid? TagItem_Index { get; set; }
        public Guid? Tag_Index { get; set; }
        public Guid? Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_ID_X { get; set; }
        public string Location_Name { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public string ItemStatus_Id { get; set; }
        public string ItemStatus_Name { get; set; }
        public string Product_Lot { get; set; }
        public string UOM { get; set; }
        public decimal? Qty { get; set; }
        public decimal? BinBalance_QtyBal { get; set; }
        public Guid? SALE_ProductConversion_Index { get; set; }
        public string SALE_ProductConversion_Id { get; set; }
        public string SALE_ProductConversion_Name { get; set; }
        public decimal? SALE_ProductConversion_Ratio { get; set; }
        public string ERP_Location { get; set; }
        //public Guid? GoodsReceive_Index { get; set; }
        public Guid? GoodsReceiveItem_Index { get; set; }
        public Guid? GoodsReceiveItemLocation_Index { get; set; }
        public string Create_by { get; set; }


        #region Send to Binbalance
        public string goodsTransfer_Index { get; set; }
        public string goodsTransferItem_Index { get; set; }
        public string goodsTransfer_No { get; set; }
        public string process_Index { get; set; }
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string goodsReceive_date { get; set; }
        public string erp_Location { get; set; }

        public Guid? location_Index_To { get; set; }
        public string location_Id_To { get; set; }
        public string location_Name_To { get; set; }
        #endregion


        public List<TransferBypassViewModel> ItemModel { get; set; }


    }

    public class View_TaskInsertBinCardViewModel
    {
        public Guid? taskitem_Index { get; set; }
        public Guid? task_Index { get; set; }
        public string task_No { get; set; }
        public Guid? ref_Document_Index { get; set; }
        public Guid? ref_DocumentItem_Index { get; set; }
        public string ref_Document_No { get; set; }
        public Guid? tagOutItem_Index { get; set; }
        public Guid? tagOut_Index { get; set; }
        public string tagOut_No { get; set; }
        public DateTime? goodsIssue_Date { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public Guid? tagItem_Index { get; set; }
        public Guid? tag_Index { get; set; }
        public string tag_No { get; set; }
        public Guid? tag_Index_To { get; set; }
        public string tag_No_To { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public string product_Lot { get; set; }
        public Guid? itemStatus_Index { get; set; }
        public string itemStatus_Id { get; set; }
        public string itemStatus_Name { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public DateTime? exp_Date { get; set; }
        public DateTime? mfg_Date { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public decimal? picking_Qty { get; set; }
        public decimal? picking_Ratio { get; set; }
        public decimal? picking_TotalQty { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public Guid? binBalance_Index { get; set; }
        public int? gIILStatus { get; set; }
        public int? gLStatus { get; set; }
        public int? taskItemStatus { get; set; }
        public int? pickStatus { get; set; }
        public Guid? process_Index { get; set; }

        public Guid? location_Index_To { get; set; }
        public string location_Id_To { get; set; }
        public string location_Name_To { get; set; }

        public Guid? itemStatus_Index_To { get; set; }
        public string itemStatus_Id_To { get; set; }
        public string itemStatus_Name_To { get; set; }

        public string userName { get; set; }
        public bool isTransfer { get; set; }
        public bool isScanSplit { get; set; }
        public bool isScanPick { get; set; }
        public bool isScanToDock { get; set; }
    }

    public class location_ToModel
    {
        
        public Guid? location_Index_To { get; set; }
        public string location_Id_To { get; set; }
        public string location_Name_To { get; set; }
    }

}
