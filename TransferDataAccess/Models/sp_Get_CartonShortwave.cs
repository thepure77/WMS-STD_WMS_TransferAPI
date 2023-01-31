using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_Get_CartonShortwave
    {
        [Key]
        public Guid PlanGoodsIssueItem_Index { get; set; }
       
        public Guid? Product_Index { get; set; }

        
        public string Product_Id { get; set; }

        
        public string Product_Name { get; set; }

        public string Product_SecondName { get; set; }

        
        public Guid? ProductConversion_Index { get; set; }

        
        public string ProductConversion_Id { get; set; }

        
        public string ProductConversion_Name { get; set; }
        public DateTime? MFG_Date { get; set; }

        public DateTime? EXP_Date { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public string ItemStatus_Id { get; set; }
        public string ItemStatus_Name { get; set; }


        public decimal? Picking_Qty { get; set; }
        public decimal? Picking_Ratio { get; set; }
        public decimal? Picking_TotalQty { get; set; }
        public string Product_ThirdName { get; set; }

        public string Product_Lot { get; set; }

        public decimal? UnitWeight { get; set; }


        public decimal? Weight { get; set; }


        public decimal? UnitWidth { get; set; }


        public decimal? UnitLength { get; set; }


        public decimal? UnitHeight { get; set; }


        public decimal? UnitVolume { get; set; }


        public decimal? Volume { get; set; }
        public decimal? UnitPrice { get; set; }


        public decimal? Price { get; set; }



    }
}
