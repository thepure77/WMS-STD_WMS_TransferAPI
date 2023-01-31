using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public partial class getTranferLocationViewModel :Result
    {
        public getTranferLocationViewModel()
        {
            locationmodel = new List<locationmodel>();
        }


        public List<locationmodel> locationmodel { get; set; }
    }

    public class locationmodel
    {
        public Guid? location_index { get; set; }

        public string location_name { get; set; }

        public string location_id { get; set; }

        
    }
}
