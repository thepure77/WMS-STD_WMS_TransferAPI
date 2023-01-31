using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TransferDataAccess.Models
{
    public class View_GTProcessStatus
    {
        [Key]
        public Guid GoodsTransfer_Index { get; set; }
        public Guid Owner_Index { get; set; }
        public Guid DocumentType_Index { get; set; }
        public string DocumentType_Id { get; set; }
        public string DocumentType_Name { get; set; }
        public string GoodsTransfer_No { get; set; }
        public DateTime? GoodsTransfer_Date { get; set; }
        public string DocumentRef_No1 { get; set; }
        public string DocumentRef_No2 { get; set; }
        public string DocumentRef_No3 { get; set; }
        public string DocumentRef_No4 { get; set; }
        public string DocumentRef_No5 { get; set; }
        public int? Document_Status { get; set; }
        public string UDF_1 { get; set; }
        public string UDF_2 { get; set; }
        public string UDF_3 { get; set; }
        public string UDF_4 { get; set; }
        public string UDF_5 { get; set; }
        public int? DocumentPriority_Status { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Cancel_By { get; set; }
        public DateTime? Cancel_Date { get; set; }
        public int? WCS_Status { get; set; }
        public DateTime? WCS_Date { get; set; }
    }
}
