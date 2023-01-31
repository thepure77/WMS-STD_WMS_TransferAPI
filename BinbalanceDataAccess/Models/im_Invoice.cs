using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class im_Invoice
    {
        [Key]
        public Guid Invoice_Index { get; set; }

        public string Invoice_No { get; set; }

        public DateTime? Invoice_Date { get; set; }

        public DateTime? Start_Date { get; set; }

        public DateTime? End_Date { get; set; }

        public DateTime? Due_Date { get; set; }

        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public Guid DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public string Tax_No { get; set; }

        public string Credit_Term { get; set; }

        public Guid? Currency_Index { get; set; }

        public decimal? Exchange_Rate { get; set; }

        public string PaymentMethod_Index { get; set; }


        public string Payment_Ref { get; set; }

        public DateTime? FullPaid_Date { get; set; }

        public decimal? Amount { get; set; }


        public decimal? Discount_Percent { get; set; }


        public decimal? Discount_Amt { get; set; }


        public decimal? Total_Amt { get; set; }


        public decimal? VAT_Percent { get; set; }


        public decimal? VAT { get; set; }


        public decimal? Net_Amt { get; set; }


        public string Billing_Address { get; set; }


        public string Billing_Tel { get; set; }


        public string Billing_Fax { get; set; }


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


        public string Confirm_By { get; set; }

        public DateTime? Confirm_Date { get; set; }
        public decimal? AmountOther { get; set; }
        public decimal? AmountStorage { get; set; }
    }
}
