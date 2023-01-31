using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{

    public partial class tmp_SuggestLocationCheckTF
    {

        [Key]
        public Guid Temp_Index { get; set; }

        public string PalletID { get; set; }

        public Guid Location_Index { get; set; }

        public string Location_Id { get; set; }

        public string Location_Name { get; set; }

        public decimal QtyBal { get; set; }

        public decimal QtyReserve { get; set; }

        public Guid Location_Index_To { get; set; }

        public string Location_Id_To { get; set; }

        public string Location_Name_To { get; set; }

        public Guid BinBalance_Index { get; set; }

        public Guid GoodsReceiveItemLocation_Index { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public int? IsComplete { get; set; }

    }
}
