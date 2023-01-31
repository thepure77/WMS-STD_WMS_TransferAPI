using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace BinBalanceBusiness.ViewModels
{
    public class LocationViewModel
    {
        public Guid location_Index { get; set; }

        public string warehouse_Index { get; set; }

        public string room_Index { get; set; }

        public string locationType_Index { get; set; }

        public string location_Id { get; set; }

        public string locationType_Name { get; set; }

        public string location_Name { get; set; }

        public string warehouse_Name { get; set; }

        public string room_Name { get; set; }

        public string locationAisle_Name { get; set; }

        public string locationAisle_Index { get; set; }

        public int? location_Bay { get; set; }

        public int? location_Depth { get; set; }

        public int? location_Level { get; set; }

        public decimal? max_Qty { get; set; }

        public decimal? max_Weight { get; set; }

        public decimal? max_Volume { get; set; }

        public decimal? max_Pallet { get; set; }

        public int? putAway_Seq { get; set; }

        public int? picking_Seq { get; set; }

        public int? isActive { get; set; }

        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }


        public string create_By { get; set; }

        public string create_Date { get; set; }

        public string update_By { get; set; }

        public string update_Date { get; set; }

        public string cancel_By { get; set; }

        public string cancel_Date { get; set; }
        public int? blockPut { get; set; }
        public int? blockPick { get; set; }
    }
}
