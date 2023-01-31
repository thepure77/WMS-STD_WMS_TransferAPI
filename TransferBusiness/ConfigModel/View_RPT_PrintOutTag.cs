using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.ConfigModel
{

    public class View_RPT_PrintOutTag
    {
        public long? Row_Index { get; set; }
        public Guid? GoodsReceive_Index { get; set; }
        public Guid? GoodsReceiveItem_Index { get; set; }
        public string GoodsReceive_No { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public Guid? Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }
        public Guid? Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Tag_No { get; set; }
        public decimal? Qty { get; set; }
        public DateTime? GoodsReceive_Date { get; set; }
        public string Ref_Document_No { get; set; }

    }
}
