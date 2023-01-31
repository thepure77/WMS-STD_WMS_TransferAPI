using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MasterDataDataAccess.Models
{
    public partial class View_LocatinSelectiveOnGroundExport
    {
        [Key]
        public Guid Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }

        public int? isActive { get; set; }
        public int? isDelete { get; set; }

        public Guid? LocationType_Index { get; set; }
        public string LocationType_Id { get; set; }
        public string LocationType_Name { get; set; }
    }
}
