using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public class TransferCartonViewModel
    {
        public Guid TagOutItemIndex { get; set; }

        public Guid? TagOutIndex { get; set; }

        [StringLength(50)]
        public string TagOutNo { get; set; }

        public Guid? GoodsIssueIndex { get; set; }

        public Guid? GoodsIssueItemIndex { get; set; }

        public Guid? GoodsIssueItemLocationIndex { get; set; }
      
        public Guid? ProductIndex { get; set; }

        [StringLength(50)]
        public string ProductId { get; set; }
     
        [StringLength(200)]
        public string ProductName { get; set; }

        public Guid? locationIndex { get; set; }

        [StringLength(50)]
        public string locationId { get; set; }

        [StringLength(200)]
        public string locationName { get; set; }

        public Guid? ItemStatusIndex { get; set; }

        [StringLength(50)]
        public string ItemStatusId { get; set; }

        [StringLength(200)]
        public string ItemStatusName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }      

        public Guid? ProductConversionIndex { get; set; }

        [StringLength(50)]
        public string ProductConversionId { get; set; }

        [StringLength(200)]
        public string ProductConversionName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MFGDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EXPDate { get; set; }

        public int? TagOutStatus { get; set; }

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

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public Guid TaskItemIndex { get; set; }

    }
}
