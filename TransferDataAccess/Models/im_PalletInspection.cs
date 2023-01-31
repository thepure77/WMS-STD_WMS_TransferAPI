using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferDataAccess.Models
{
 

    public partial class im_PalletInspection
    {
        [Key]
        public Guid PalletInspection_Index { get; set; }

        public string Tag_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }
       
        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }
        

    }
}
