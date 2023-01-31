using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace TransferDataAccess.Models
{

    public partial class View_GoodsTransfer
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid GoodsTransfer_Index { get; set; }


        public string GoodsTransfer_No { get; set; }

        public DateTime? GoodsTransfer_Date { get; set; }

        public Guid? Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public int? Document_Status { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Qty { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }
    }
}
