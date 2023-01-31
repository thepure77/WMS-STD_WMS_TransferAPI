using System;
using System.Collections.Generic;
using System.Text;

namespace TransferBusiness.Transfer
{
    public partial class SearchGTModel : Pagination
    {
        public SearchGTModel()
        {
            sort = new List<sortViewModel>();

            status = new List<statusViewModel>();

        }

        public Guid? goodsTransfer_Index { get; set; }
        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }

        public Guid documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string goodsTransfer_No { get; set; }
        public string goodsTransfer_Date { get; set; }
        public string goodsTransfer_Date_To { get; set; }
        public string documentRef_No1 { get; set; }
        public string documentRef_No2 { get; set; }
        public string documentRef_No3 { get; set; }
        public string documentRef_No4 { get; set; }
        public string documentRef_No5 { get; set; }
        public int? document_Status { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public int? documentPriority_Status { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string create_Date_To { get; set; }
        public string update_By { get; set; }
        public string update_Date { get; set; }
        public string cancel_By { get; set; }
        public string cancel_Date { get; set; }
        public string processStatus_Name { get; set; }
        public string column_Name { get; set; }
        public string order_by { get; set; }
        public string key { get; set; }
        public bool advanceSearch { get; set; }
        public decimal? qty { get; set; }
        public int? wcs_Status { get; set; }
        public string wcs_Date { get; set; }

        public List<sortViewModel> sort { get; set; }
        public List<statusViewModel> status { get; set; }

        public class actionResultGTViewModel
        {
            public IList<SearchGTModel> lstGoodsTranfer { get; set; }
            public Pagination pagination { get; set; }
        }

        public class sortViewModel
        {
            public string value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class statusViewModel
        {
            public int value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class SortModel
        {
            public string ColId { get; set; }
            public string Sort { get; set; }

            public string PairAsSqlExpression
            {
                get
                {
                    return $"{ColId} {Sort}";
                }
            }
        }

        public class StatusModel
        {
            public string Name { get; set; }
        }

    }
}
