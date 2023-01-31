using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace TransferDataAccess.Models
{

    public partial class View_TaskTransfer
    {
        [Key]
        public Guid TaskTransfer_Index { get; set; }
        public string TaskTransfer_No { get; set; }

        public string Ref_Document_No { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public string UserAssign { get; set; }
        public string Create_By { get; set; }
        public DateTime Create_Date { get; set; }
        public string update_By { get; set; }

    }
}
