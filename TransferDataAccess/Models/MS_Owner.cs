using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{

    public partial class MS_Owner
    {
        public MS_Owner()
        {

        }

        [Key]
        public Guid Owner_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(200)]
        public string Owner_Name { get; set; }

        [StringLength(200)]
        public string Owner_Address { get; set; }

        public Guid OwnerType_Index { get; set; }

        public Guid? SubDistrict_Index { get; set; }

        public Guid? District_Index { get; set; }

        public Guid? Province_Index { get; set; }

        public Guid? Country_Index { get; set; }

        public Guid? Postcode_Index { get; set; }

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

    }
}
