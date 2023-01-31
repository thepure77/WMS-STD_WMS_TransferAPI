using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class im_Memo
    {
        [Key]
        public Guid Memo_Index { get; set; }

        public string Memo_No { get; set; }

        public DateTime? Memo_Date { get; set; }

        public Guid? Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public string DocumentRef_No1 { get; set; }

        public string DocumentRef_No2 { get; set; }

        public string DocumentRef_No3 { get; set; }

        public string DocumentRef_No4 { get; set; }

        public string DocumentRef_No5 { get; set; }

        public int Document_Status { get; set; }

        
        public string Document_Remark { get; set; }

        
        public string UDF_1 { get; set; }

        
        public string UDF_2 { get; set; }

        
        public string UDF_3 { get; set; }

        
        public string UDF_4 { get; set; }

        
        public string UDF_5 { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        
        public string Ref_Document_No { get; set; }

        
        public string Ref_Document_LineNum { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        
        public decimal? Total_Amount { get; set; }

        
        public string Create_By { get; set; }

        
        public DateTime? Create_Date { get; set; }
        
        
        public string Update_By { get; set; }
        
        
        public DateTime? Update_Date { get; set; }

        
        public string Cancel_By { get; set; }

        
        public DateTime? Cancel_Date { get; set; }

    }
}
