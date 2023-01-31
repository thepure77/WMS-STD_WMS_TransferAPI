using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class View_aging
    {
        [Key]

        public long? RowIndex { get; set; }

        public Guid Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }


        public string Owner_Name { get; set; }

        public decimal? date_1 { get; set; }

        public decimal? date_2 { get; set; }

        public decimal? date_3 { get; set; }

        public decimal? date_4 { get; set; }

        public decimal? sumQtyBalance { get; set; }

        public int? age { get; set; }

        public decimal? average { get; set; }
    }
}
