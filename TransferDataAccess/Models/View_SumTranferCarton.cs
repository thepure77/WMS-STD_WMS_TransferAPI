using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{


    public partial class View_SumTranferCarton
    {
        [Key]

        public Guid Product_Index { get; set; }
        public Guid GoodsIssue_Index { get; set; }


        public decimal? Picking_Qty { get; set; }

        public DateTime? MFG_Date { get; set; }

        public DateTime? EXP_Date { get; set; }

        public Guid ProductConversion_Index { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }



        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string Location_Id { get; set; }

        public string Location_Name { get; set; }

        public Guid Location_Index { get; set; }
        public string UDF_1 { get; set; }

        public string UDF_2 { get; set; }

        public string UDF_3 { get; set; }

        public string UDF_4 { get; set; }

        public string UDF_5 { get; set; }
        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }
        public string TagOutPick_No { get; set; }
        public string ProductConversionBarcode { get; set; }

        

    }
}
