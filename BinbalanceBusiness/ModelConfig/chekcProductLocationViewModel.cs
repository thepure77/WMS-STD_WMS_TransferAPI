using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public  class chekcProductLocationViewModel
    {
        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }
        public string tagNo_New { get; set; }
        public decimal? qty { get; set; }
        public string location_Name_To { get; set; }

        

    }
}
