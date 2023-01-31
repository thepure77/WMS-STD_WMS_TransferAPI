using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public class View_LocatinCyclecountViewModel
    {

        public Guid? location_Index { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }

        public Guid? zone_Index { get; set; }

        public string zone_Id { get; set; }

        public string zone_Name { get; set; }

        public Guid? locationType_Index { get; set; }

        public string locationType_Id { get; set; }

        public string locationType_Name { get; set; }

        public Guid? warehouse_Index { get; set; }

        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }

    }
}
