using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public  class View_BinbalanceProductTypeViewModel
    {
        public Guid product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }

        public Guid? productType_Index { get; set; }
        public string productType_Id { get; set; }
        public string productType_Name { get; set; }
    }
}
