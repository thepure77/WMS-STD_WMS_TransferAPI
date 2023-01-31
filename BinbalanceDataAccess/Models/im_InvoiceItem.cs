using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class im_InvoiceItem
    {
        [Key]
        public Guid InvoiceItem_Index { get; set; }

        public Guid Invoice_Index { get; set; }

        public int? Item_Seq { get; set; }

        public Guid? ServiceCharge_Index { get; set; }

        public string ServiceCharge_Id { get; set; }


        public string ServiceCharge_Name { get; set; }


        public decimal? Qty { get; set; }


        public decimal? Weight { get; set; }


        public decimal? Volume { get; set; }


        public decimal? RT { get; set; }

        public decimal UnitCharge { get; set; }


        public decimal? Rate { get; set; }


        public decimal? Amount { get; set; }

        public Guid? Currency_Index { get; set; }

        public Guid? Memo_Index { get; set; }

        [StringLength(50)]
        public string Memo_No { get; set; }


        public string DocumentRef_No1 { get; set; }


        public string DocumentRef_No2 { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string DocumentRef_No4 { get; set; }


        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }


        public string Document_Remark { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

    }
}
