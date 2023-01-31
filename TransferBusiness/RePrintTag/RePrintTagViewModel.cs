using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public partial class RePrintTagViewModel
    {
        public Guid? goodsTransferItem_Index { get; set; }
        public Guid? goodsTransfer_Index { get; set; }
        public string GoodsTransfer_No { get; set; }
        public Guid? tagItem_Index { get; set; }
        public Guid? tag_Index { get; set; }
        public string tag_No { get; set; }
        public Guid? tag_Index_To { get; set; }
        public string tag_No_To { get; set; }

        public Guid? goodsReceive_Index { get; set; }

        public Guid? goodsReceiveItem_Index { get; set; }

        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }
        public Guid? itemStatus_Index_To { get; set; }

        public string itemStatus_Id_To { get; set; }

        public string itemStatus_Name_To { get; set; }

        public Guid? location_Index { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }
        public Guid? location_Index_To { get; set; }

        public string location_Id_To { get; set; }

        public string location_Name_To { get; set; }

        public string qty { get; set; }

        public decimal? ratio { get; set; }

        public decimal? totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public string weight { get; set; }

        public string volume { get; set; }

        public DateTime? mFG_Date { get; set; }

        public DateTime? eXP_Date { get; set; }

        public string tagRef_No1 { get; set; }

        public string tagRef_No2 { get; set; }

        public string tagRef_No3 { get; set; }

        public string tagRef_No4 { get; set; }

        public string tagRef_No5 { get; set; }

        public int? tag_Status { get; set; }

        public string uDF_1 { get; set; }

        public string uDF_2 { get; set; }

        public string uDF_3 { get; set; }

        public string uDF_4 { get; set; }

        public string uDF_5 { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string putaway_By { get; set; }
        public string putaway_Status { get; set; }

    }

    public partial class RePrintTagSearchViewModel
    {
        public Guid? tag_Index { get; set; }
        public string tag_No { get; set; }
        public Guid? location_Index { get; set; }
        public string location_Name { get; set; }
        public string goodsTransfer_Index { get; set; }
        public string goodsTransfer_No { get; set; }
        public Guid? goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }

    }
}
