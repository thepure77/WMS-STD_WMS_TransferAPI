using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public partial class ItemListViewModel
    {
        public Guid? index { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public int chk { get; set; }

    }
}
