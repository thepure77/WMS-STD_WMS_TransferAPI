using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public class TagViewModel
    {
        [Key]
        public Guid? TagIndex { get; set; }

        [StringLength(50)]
        public string TagNo { get; set; }

        [StringLength(50)]
        public string PalletNo { get; set; }

        public Guid? PalletIndex { get; set; }

        [StringLength(200)]
        public string TagRefNo1 { get; set; }

        [StringLength(200)]
        public string TagRefNo2 { get; set; }

        [StringLength(200)]
        public string TagRefNo3 { get; set; }

        [StringLength(200)]
        public string TagRefNo4 { get; set; }

        [StringLength(200)]
        public string TagRefNo5 { get; set; }

        public int? TagStatus { get; set; }

        [StringLength(200)]
        public string UDF1 { get; set; }

        [StringLength(200)]
        public string UDF2 { get; set; }

        [StringLength(200)]
        public string UDF3 { get; set; }

        [StringLength(200)]
        public string UDF4 { get; set; }

        [StringLength(200)]
        public string UDF5 { get; set; }

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
    }
}
