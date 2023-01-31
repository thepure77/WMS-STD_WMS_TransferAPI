using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    [Table("_Prepare_Imports_step")]
    public partial class _Prepare_Imports_step
    {
        [Key]
        public int RowIndex { get; set; }

        public Guid Import_Index { get; set; }
        public Guid? import_userindex { get; set; }


        public DateTime? Run_Date { get; set; }

        public string Import_Type { get; set; }

        public string Import_Message { get; set; }

        public string Import_File_Name { get; set; }

        public int Import_Status { get; set; }

        public string Import_By { get; set; }

    }
}
