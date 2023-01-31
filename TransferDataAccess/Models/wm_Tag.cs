using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
    public partial class WM_Tag
    {
        [Key]
        public Guid Tag_Index { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }

        [StringLength(50)]
        public string Pallet_No { get; set; }

        public Guid Pallet_Index { get; set; }

        [StringLength(200)]
        public string TagRef_No1 { get; set; }

        [StringLength(200)]
        public string TagRef_No2 { get; set; }

        [StringLength(200)]
        public string TagRef_No3 { get; set; }

        [StringLength(200)]
        public string TagRef_No4 { get; set; }

        [StringLength(200)]
        public string TagRef_No5 { get; set; }

        public int? Tag_Status { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

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
    }
}
