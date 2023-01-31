using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace  TransferBusiness.Transfer
{


    public partial class ReasonCodeViewModel
    {
        [Key]
        public Guid ReasonCodeIndex { get; set; }

        [StringLength(50)]
        public string ReasonCodeId { get; set; }

        [StringLength(200)]
        public string ReasonCodeName { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? StatusId { get; set; }

        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        [StringLength(200)]
        public string CancelBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CancelDate { get; set; }

        public Guid? ProcessIndex { get; set; }
    }
}
