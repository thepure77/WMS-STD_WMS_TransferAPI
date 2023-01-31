﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public class GoodsReceiveViewModel
    {
        [Key]
        public Guid? goodsReceive_Index { get; set; }

        public Guid? owner_Index { get; set; }

        [StringLength(50)]
        public string owner_Id { get; set; }

        [StringLength(50)]
        public string owner_Name { get; set; }

        public Guid? documentType_Index { get; set; }

        [StringLength(50)]
        public string documentType_Id { get; set; }

        [StringLength(200)]
        public string documentType_Name { get; set; }

        [StringLength(50)]
        public string goodsReceive_No { get; set; }

        public DateTime? goodsReceive_Date { get; set; }
       


        [StringLength(200)]
        public string documentRef_No1 { get; set; }

        [StringLength(200)]
        public string documentRef_No2 { get; set; }

        [StringLength(200)]
        public string documentRef_No3 { get; set; }

        [StringLength(200)]
        public string documentRef_No4 { get; set; }

        [StringLength(200)]
        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }

        [StringLength(200)]
        public string document_Remark { get; set; }

        [StringLength(200)]
        public string udf_1 { get; set; }

        [StringLength(200)]
        public string udf_2 { get; set; }

        [StringLength(200)]
        public string udf_3 { get; set; }

        [StringLength(200)]
        public string udf_4 { get; set; }

        [StringLength(200)]
        public string udf_5 { get; set; }

        public int? document_Priority_Status { get; set; }

        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? cancel_Date { get; set; }

        public int? putaway_Status { get; set; }

        public Guid? warehouse_Index { get; set; }

        [StringLength(50)]
        public string warehouse_Id { get; set; }

        [StringLength(200)]
        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public Guid? dockDoor_Index { get; set; }

        [StringLength(50)]
        public string dockDoor_Id { get; set; }

        [StringLength(200)]
        public string dockDoor_Name { get; set; }

        public Guid? vehicleType_Index { get; set; }

        [StringLength(50)]
        public string vehicleType_Id { get; set; }

        [StringLength(200)]
        public string vehicleType_Name { get; set; }

        public Guid? containerType_Index { get; set; }

        [StringLength(50)]
        public string containerType_Id { get; set; }

        [StringLength(200)]
        public string containerType_Name { get; set; }

        public string tag_No { get; set; }

        public List<GoodsReceiveItemViewModel> listGoodsReceiveItemViewModels { get; set; }

        public List<GoodsReceiveItemViewModel> listPlanGoodsReceiveItemViewModel { get; set; }
    }

    public class GoodsReceiveViewModelV2
    {
        [Key]
        public Guid? goodsReceive_Index { get; set; }

        public Guid owner_Index { get; set; }


        public string owner_Id { get; set; }


        public string owner_Name { get; set; }

        public Guid? documentType_Index { get; set; }


        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }


        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }

        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string document_Remark { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string uDF_5 { get; set; }

        public int? documentPriority_Status { get; set; }


        public string create_By { get; set; }


        public string create_Date { get; set; }


        public string update_By { get; set; }


        public string update_Date { get; set; }


        public string cancel_By { get; set; }


        public string cancel_Date { get; set; }

        public int? putaway_Status { get; set; }

        public Guid? warehouse_Index { get; set; }


        public string warehouse_Id { get; set; }


        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public Guid? dockDoor_Index { get; set; }


        public string dockDoor_Id { get; set; }


        public string dockDoor_Name { get; set; }

        public Guid? vehicleType_Index { get; set; }


        public string vehicleType_Id { get; set; }


        public string vehicleType_Name { get; set; }

        public Guid? containerType_Index { get; set; }


        public string containerType_Id { get; set; }


        public string containerType_Name { get; set; }

        public string key { get; set; }

        public string name { get; set; }

        public string userAssign { get; set; }
        public string invoice_No { get; set; }
        public Guid? vendor_Index { get; set; }


        public string vendor_Id { get; set; }


        public string vendor_Name { get; set; }

        public Guid? whOwner_Index { get; set; }


        public string whOwner_Id { get; set; }


        public string whOwner_Name { get; set; }

        //Add new
        public string processStatus_Name { get; set; }
        public string tag_no { get; set; }
    }
}
