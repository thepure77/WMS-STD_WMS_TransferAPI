using System;
using System.Collections.Generic;
using System.Text;
using TransferBusiness.ConfigModel;

namespace TransferBusiness.Transfer.Report
{
    public class TagPutawayTransferViewModel
    {
        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string warehouse_Id { get; set; }

        public string location_Id { get; set; }

        public string location_Id_To { get; set; }
        
        public string suggest_Location_Name { get; set; }

        public string date_Print { get; set; }

        public string planGoodsReceive_No { get; set; }

        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }

        public string owner_Name { get; set; }

        public string productConversion_Name { get; set; }

        public string ref_no1 { get; set; }

        public string ref_no2 { get; set; }
        public string ref_no3 { get; set; }

        public string tag_No { get; set; }
        public string tag_No_To { get; set; }

        public string tag_NoBarcode { get; set; }

        public decimal? qty { get; set; }

        public bool checkQuery { get; set; }

        public Guid? productConversion_Index { get; set; }
        public Guid? goodsReceiveItem_Index { get; set; }
        public Guid? goodsReceive_Index { get; set; }
        public Guid? tag_Index { get; set; }
        public Guid? goodsTransferItem_Index { get; set; }
        public Guid? goodsTransfer_Index { get; set; }
        public string goodsTransfer_No { get; set; }

        public List<TagPutawayTransferViewModel> listTagPutawayTransferViewModel { get; set; }
    }
}
