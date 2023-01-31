using System;
using System.Collections.Generic;
using System.Text;

namespace TransferBusiness.ConfigModel
{
    public class ProductConversionViewModelDoc : Pagination
    {
        public Guid productConversion_Index { get; set; }

        public Guid product_Index { get; set; }


        public string productConversion_Id { get; set; }


        public string productConversion_Name { get; set; }


        public decimal? productconversion_Ratio { get; set; }


        public decimal? productconversion_Weight { get; set; }


        public decimal? productconversion_Width { get; set; }


        public decimal? productconversion_Length { get; set; }


        public decimal? productconversion_Height { get; set; }


        public decimal? productconversion_VolumeRatio { get; set; }


        public decimal? productconversion_Volume { get; set; }

        public int isActive { get; set; }

        public int isDelete { get; set; }

        public int issystem { get; set; }

        public int status_id { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }
    }
    public class actionResultProductConversionViewModel
    {
        public IList<ProductConversionViewModelDoc> items { get; set; }
        public Pagination pagination { get; set; }
    }

}
