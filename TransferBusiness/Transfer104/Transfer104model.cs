using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TransferBusiness.Transfer;

namespace TransferBusiness.Transfer
{
    public class Transfer104model : Result
    {

        public Transfer104model()
        {
            listTransferItemViewModel = new List<Transfer104model>();
        }

        public Guid? BinBalance_Index { get; set; }

        public string Tag_No { get; set; }


        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string ProductConversion_Name { get; set; }

        public Guid? Location_Index { get; set; }

        public string Location_Id { get; set; }

        public string Location_Name { get; set; }

        public decimal? BinBalance_QtyBegin { get; set; }

        public decimal? BinBalance_QtyBal { get; set; }

        public decimal? BinBalance_QtyReserve { get; set; }

        public Guid? GoodsReceiveItemLocation_Index { get; set; }

        public string crane { get; set; }

        public string location { get; set; }

        public List<Transfer104model> listTransferItemViewModel { get; set; }
    }
   
    public class statusmodel : Result
    {
        public string status { get; set; }

        public messagemodel message { get; set; }
    }

    public class messagemodel
    {
        public string description { get; set; }
    }
}
