using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class ms_ReasonCode
    {
        [Key]
        public Guid ReasonCode_Index { get; set; }

        [StringLength(50)]
        public string ReasonCode_Id { get; set; }

        [StringLength(200)]
        public string ReasonCode_Name { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public Guid? Process_Index { get; set; }
    }
}
