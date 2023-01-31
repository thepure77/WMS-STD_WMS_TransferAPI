using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.ConfigModel
{
    public class ProcessStatusViewModel
    {
        public Guid processStatus_Index { get; set; }
        public string processStatus_Id { get; set; }
        public Guid process_Index { get; set; }

        [StringLength(50)]
        public string processStatus_Name { get; set; }

        public int? isActive { get; set; }
        public int? isDelete { get; set; }

        public int? isSystem { get; set; }

        public int? status_Id { get; set; }

        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? cancel_Date { get; set; }

        public int chk { get; set; }

        public string key { get; set; }

        public string name { get; set; }
    }
}
