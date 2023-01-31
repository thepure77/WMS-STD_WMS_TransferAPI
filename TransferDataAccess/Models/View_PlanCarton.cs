using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class View_PlanCarton
    {
        [Key]
        public string TagOut_No { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public string Ref_PlanGoodsIssue_No { get; set; }
        public Guid PlanGoodsIssue_Index { get; set; }

        

    }
}
