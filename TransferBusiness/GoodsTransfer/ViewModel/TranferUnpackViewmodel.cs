using System;
using System.Collections.Generic;
using System.Text;
using TransferBusiness.ConfigModel;

namespace TransferBusiness.GoodsTransfer.ViewModel
{
    public class TranferUnpackViewmodel : Result
    {
        public Guid? location_Index { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }

    }
}


