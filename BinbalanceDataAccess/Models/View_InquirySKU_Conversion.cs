using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{
    public partial class View_InquirySKU_Conversion
    {
        [Key]
        public long RowIndex { get; set; }

        public string Product_Id { get; set; }

        public string ProductConversionBarcode { get; set; }

        public string Product_Name { get; set; }

        public string Product_SecondName { get; set; }

        public string Product_ThirdName { get; set; }

        public string SKUConversionName { get; set; }

        public string ProductConversion_Name { get; set; }

        public decimal? ProductConversion_Ratio { get; set; }

    }
}
