using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferBusiness.Transfer;

namespace InterfaceBusiness
{
    public class TransferRequestViewModel
    {
        public TransferRequestViewModel()
        {
            Detail = new List<TransferRequestDetail>();
        }
        //public string PstngDate { get; set; }
        public string DocDate { get; set; }
        //public string RefDocNo { get; set; }
        public string GrNo { get; set; }
        public string HeaderTxt { get; set; }
        public string GmCode { get; set; }
        public List<TransferRequestDetail> Detail { get; set; }
        public AuthenticationModel AuthSAP { get; set; }

    }

    public class TransferRequestDetail
    {
        public string Material { get; set; }
        public string Plant { get; set; }
        public string StgeLoc { get; set; }
        public string Batch { get; set; }
        public string MoveType { get; set; }
        public decimal EntryQnt { get; set; }
        public string EntryUom { get; set; }
        public string MovePlant { get; set; }
        public string MoveStloc { get; set; }
        public string StckType { get; set; }
    }
}
