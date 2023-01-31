using TransferBusiness.Transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransferBusiness.Transfer
{
    public partial class listTransferItem
    {
        public List<TransferViewModel> listTransferItemViewModel { get; set; }
    }

    public class actionResultCHeckTagViewModel
    {
        public IList<TransferViewModel> Formart { get; set; }
        public IList<TransferViewModel> itemsCheckTagNo { get; set; }
        public IList<TransferViewModel> itemsCheckTagNoQtyNotZero { get; set; }
    }
}
