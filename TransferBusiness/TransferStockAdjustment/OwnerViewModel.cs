using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{

    public class OwnerViewModel
    {
        public Guid OwnerIndex { get; set; }


        [StringLength(50)]
        public string OwnerId { get; set; }


        [StringLength(200)]
        public string OwnerName { get; set; }

        [StringLength(200)]
        public string OwnerAddress { get; set; }

        [StringLength(200)]
        public string OwnerTypeName { get; set; }

        [StringLength(200)]
        public string OwnerTaxID { get; set; }

        [StringLength(200)]
        public string OwnerEmail { get; set; }

        [StringLength(200)]
        public string OwnerFax { get; set; }

        [StringLength(200)]
        public string OwnerTel { get; set; }

        [StringLength(200)]
        public string OwnerMobile { get; set; }

        [StringLength(200)]
        public string OwnerBarcode { get; set; }

        [StringLength(200)]
        public string ContactPerson { get; set; }

        [StringLength(200)]
        public string ContactPerson2 { get; set; }

        [StringLength(200)]
        public string ContactPerson3 { get; set; }

        [StringLength(200)]
        public string ContactTel { get; set; }

        [StringLength(200)]
        public string ContactTel2 { get; set; }

        [StringLength(200)]
        public string ContactTel3 { get; set; }

        [StringLength(200)]
        public string ContactEmail { get; set; }

        [StringLength(200)]
        public string ContactEmail2 { get; set; }

        [StringLength(200)]
        public string ContactEmail3 { get; set; }

        [StringLength(200)]
        public string SubDistrictName { get; set; }

        [StringLength(200)]
        public string DistrictName { get; set; }

        [StringLength(200)]
        public string ProvinceName { get; set; }

        [StringLength(200)]
        public string PostCodeName { get; set; }

        [StringLength(200)]
        public string CountryName { get; set; }

        public Guid OwnerTypeIndex { get; set; }

        public Guid? SubDistrictIndex { get; set; }

        public Guid? DistrictIndex { get; set; }

        public Guid? ProvinceIndex { get; set; }

        public Guid? CountryIndex { get; set; }

        public Guid? PostCodeIndex { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? StatusId { get; set; }


        [StringLength(200)]
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [StringLength(200)]
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        [StringLength(200)]
        public string CancelBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CancelDate { get; set; }
    }
}
