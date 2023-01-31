using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.GoodIssue
{

    public partial class listGoodsTransferViewModel
    {
        public Guid? goodsTransfer_Index { get; set; }

        public string goodsTransfer_No { get; set; }


        public string goodsTransfer_Date { get; set; }


        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }

        public int? document_Status { get; set; }


        public string planGoodsIssue_No { get; set; }


        public Guid? documentType_Index { get; set; }


        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }

        public decimal? weight { get; set; }

        public decimal? qty { get; set; }

        public string create_By { get; set; }
        public string document_Remark { get; set; }


        public string processStatus_Name { get; set; }


    }
}
