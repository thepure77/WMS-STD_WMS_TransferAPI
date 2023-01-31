using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class View_MEMO
    {
        [Key]

        public long? RowIndex { get; set; }
        public Guid Memo_Index { get; set; }

        public string Memo_No { get; set; }

        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public DateTime Memo_Date { get; set; }

        public Guid? Ref_Process_Index { get; set; }

        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        public Guid ServiceCharge_Index { get; set; }


        public string ServiceCharge_Id { get; set; }

        public string ServiceCharge_Name { get; set; }

        public decimal UnitCharge { get; set; }

        public decimal? Rate { get; set; }

        public decimal Qty { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Volume { get; set; }

        public decimal? Pallet { get; set; }

        public decimal? RT { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Minimumrate { get; set; }
    }
}
