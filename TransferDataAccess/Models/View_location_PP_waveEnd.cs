using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
 

    public partial class View_location_PP_waveEnd
    {
        [Key]
        public long RowIndex { get; set; }
        public string Location_ID_X { get; set; }
        public Guid? BinBalance_Index { get; set; }
        public string Tag_No { get; set; }
        public Guid? TagItem_Index { get; set; }
        public Guid? Tag_Index { get; set; }
        public Guid? Location_Index { get; set; }
        public string Location_Id { get; set; }
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
        public Guid? GoodsReceive_Index { get; set; }
        public Guid? GoodsReceiveItem_Index { get; set; }
        public Guid? GoodsReceiveItemLocation_Index { get; set; }
        
        
        
        
        
        
        
       

    }
}
