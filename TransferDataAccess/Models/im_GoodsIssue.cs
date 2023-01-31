using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class im_GoodsIssue
    {


        [Key]
        public Guid GoodsIssue_Index { get; set; }

        public Guid Owner_Index { get; set; }

        
        [StringLength(50)]
        public string Owner_Id { get; set; }

        
        [StringLength(50)]
        public string Owner_Name { get; set; }

        public Guid? DocumentType_Index { get; set; }

        
        [StringLength(50)]
        public string DocumentType_Id { get; set; }

        
        [StringLength(200)]
        public string DocumentType_Name { get; set; }

        
        [StringLength(50)]
        public string GoodsIssue_No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime GoodsIssue_Date { get; set; }

        [StringLength(200)]
        public string DocumentRef_No1 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No2 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No3 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No4 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No5 { get; set; }

        [StringLength(200)]
        public string Document_Remark { get; set; }

        public int? Document_Status { get; set; }

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

        public int? DocumentPriority_Status { get; set; }

        public int? Picking_Status { get; set; }

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

        public Guid? Warehouse_Index { get; set; }

        [StringLength(50)]
        public string Warehouse_Id { get; set; }

        [StringLength(200)]
        public string Warehouse_Name { get; set; }


    }
}
