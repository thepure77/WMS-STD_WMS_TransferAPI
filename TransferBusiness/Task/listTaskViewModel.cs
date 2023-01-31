using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.GoodIssue
{

    public partial class listTaskViewModel
    {

        public listTaskViewModel()
        {
            items = new List<listTaskViewModel>();
        }

        public Guid? taskTransfer_Index { get; set; }
        public Guid? taskTransferItem_Index { get; set; }
        public Guid? binbalance_index { get; set; }

        public string taskTransfer_No { get; set; }

        public string update_By { get; set; }

        public Guid? goodsTransfer_Index { get; set; }

        public string document_Status { get; set; }
        public string goodsTransfer_No { get; set; }
        public string userAssign { get; set; }

        public string goodsTransfer_Date { get; set; }

        public string productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? qty { get; set; }

        public string documentType_Name { get; set; }
        public string location_Name { get; set; }
        public string location_Name_To { get; set; }
        public string udf_5 { get; set; }

        public string product_Id { get; set; }
        public string product_Name { get; set; }

        public string tag_No { get; set; }

        public List<listTaskViewModel> items { get; set; }
    }
}
