using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.GoodIssue
{


    public class TaskfilterViewModel 
    {
        public TaskfilterViewModel()
        {
            listTaskViewModel = new List<listTaskViewModel>();

        }

        public Guid? taskTransfer_Index { get; set; }


        public string taskTransfer_No { get; set; }

        public string goodsTransfer_No { get; set; }

        public Guid? goodsTransfer_Index { get; set; }
        public string userAssign { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string create_Time { get; set; }
        public string update_By { get; set; }



        public List<listTaskViewModel> listTaskViewModel { get; set; }

    }

}
