using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace BinBalanceBusiness.ViewModels
{
    public class ZoneLocationViewModel
    {
        [StringLength(200)]
        public string Zone_Name { get; set; }

        [StringLength(200)]
        public string Location_Name { get; set; }

        [StringLength(200)]
        public string ZoneLocation_Id { get; set; }

        public Guid ZoneLocation_Index { get; set; }

        public Guid? Zone_Index { get; set; }

        public Guid? Location_Index { get; set; }
    }
}
