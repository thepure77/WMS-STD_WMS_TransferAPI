using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{

    public class SumQtyBinbalanceViewModel
    {
        [Key]

        public long? RowIndex { get; set; }

        public string LocationName { get; set; }

        public decimal? BinBalanceQtyBal { get; set; }

        public Guid ProductIndex { get; set; }

        public Guid ownerIndex { get; set; }

        public Guid WareHouseIndex { get; set; }

        public string ProductId { get; set; }

        public string TagOutNo { get; set; }

        public string ProductName { get; set; }
        public string ProductSecondName { get; set; }
        public string ProductThirdName { get; set; }
        public string Tag_No { get; set; }
        public string lpnNo { get; set; }

        public string productConversionBarcode { get; set; }
        public string productConversionBarcodeId { get; set; }

        public string productConversionName{ get; set; }


    }
}
