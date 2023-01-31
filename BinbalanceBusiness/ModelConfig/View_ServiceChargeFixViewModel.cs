using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public  class View_ServiceChargeFixViewModel
    {
        public long? rowIndex { get; set; }

        public Guid? serviceCharge_Index { get; set; }

        public string serviceCharge_Id { get; set; }
        public string serviceCharge_Name { get; set; }
        public Guid? owner_Index { get; set; }

        public string owner_Id { get; set; }
        public string owner_Name { get; set; }

        public decimal? rate { get; set; }

        

    }
}
