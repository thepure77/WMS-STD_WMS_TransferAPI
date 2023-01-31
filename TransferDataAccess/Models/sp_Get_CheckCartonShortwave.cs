using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace GIDataAccess.Models
{

    public partial class sp_Get_CheckCartonShortwave
    {
        [Key]
        public Guid PlanGoodsIssueItem_Index { get; set; }

        public string PlanGoodsIssue_No { get; set; }

        public Guid Zone_Index { get; set; }
        
    }
}
