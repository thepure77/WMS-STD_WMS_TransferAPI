using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace BinBalanceBusiness.ViewModels
{

    public class ItemListViewModel
    {
        public Guid? index { get; set; }
        public Guid? sub_index { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string key2 { get; set; }
        public string key3 { get; set; }
        public string key4 { get; set; }
        public string key5 { get; set; }
        public int chk { get; set; }
    }
}
