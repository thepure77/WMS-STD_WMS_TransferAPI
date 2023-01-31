using System;
using System.Collections.Generic;
using System.Text;
using TransferBusiness.ConfigModel;

namespace TransferBusiness.Transfer.Report
{
    public class ReportGoodsTransferViewModel
    {
        public string customer_Code { get; set; }
        public string documentRef_Remark { get; set; }
        public string goodsTransfer_No { get; set; }
        public string goodsTransfer_Date { get; set; }
        public string goodsTransfer_Time { get; set; }
        public string goodsReceive_No { get; set; }
        public string tag_No { get; set; }
        public string product_name { get; set; }
        public decimal qty { get; set; }
        public string ProductConversion_Name { get; set; }
        public decimal Weight { get; set; }
        public string itemStatus_Name { get; set; }
        public string itemStatus_Name_To { get; set; }
        public string location_Name { get; set; }
        public string location_Name_To { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public string mat_Doc { get; set; }
        public string fi_Doc { get; set; }
    }
}
