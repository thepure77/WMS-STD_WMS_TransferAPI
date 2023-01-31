using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BinBalanceDataAccess.Models
{

    public partial class im_MemoItem
    {
        [Key]
        public Guid? Memo_Item_Index { get; set; }

        public Guid? Memo_Index { get; set; }

        public String Memo_No { get; set; }

        public Guid? ServiceCharge_Index { get; set; }

        
        public String ServiceCharge_Id { get; set; }

        
        public String ServiceCharge_Name { get; set; }
        
        
        public String LineNum { get; set; }

        
        public decimal? UnitCharge { get; set; }

        
        public decimal? Rate { get; set; }

        
        public decimal? Qty { get; set; }

        
        public decimal? Weight { get; set; }

        
        public decimal? Volume { get; set; }

        
        public decimal? Pallet { get; set; }

        
        public decimal? RT { get; set; }

        
        public decimal? Amount { get; set; }

        
        public decimal? Minimumrate { get; set; }

        
        public String DocumentRef_No1 { get; set; }

        
        public String DocumentRef_No2 { get; set; }

        
        public String DocumentRef_No3 { get; set; }

        
        public String DocumentRef_No4 { get; set; }

        
        public String DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }

        
        public String Document_Remark { get; set; }

        public Guid? Container_Index { get; set; }

        public Guid? VehicleType_Index { get; set; }

        
        public DateTime? Start_date { get; set; }

        
        public DateTime? End_date { get; set; }


        
        public String Create_By { get; set; }

        
        public DateTime? Create_Date { get; set; }

        
        public String Update_By { get; set; }

        
        public DateTime? Update_Date { get; set; }

        
        public String Cancel_By { get; set; }

        
        public DateTime? Cancel_Date { get; set; }

        
    }
}
