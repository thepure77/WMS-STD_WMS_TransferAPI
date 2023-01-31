using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class View_CalMemo
    {
        [Key]

        public long? RowIndex { get; set; }
        public Guid Memo_Index { get; set; }

        public string Memo_No { get; set; }

        public DateTime Memo_Date { get; set; }

        public decimal? MemoAmount { get; set; }

    }
}
