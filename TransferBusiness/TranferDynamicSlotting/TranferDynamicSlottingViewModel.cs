using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TransferBusiness.GoodsTransfer.ViewModel;

namespace TransferBusiness.Transfer
{
    public partial class TranferDynamicSlottingViewModel : Result
    {

        public Guid? DynamicSlotting_Index { get; set; }

        public string DynamicSlotting_Id { get; set; }

        public string DynamicSlotting_Remark { get; set; }

        public string crane_Name { get; set; }

        public Guid? Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public Guid? Zoneputaway_Index { get; set; }

        public string Zoneputaway_Id { get; set; }

        public string Zoneputaway_Name { get; set; }

        public string Create_By { get; set; }

        public string Update_By { get; set; }

    }
    
}
