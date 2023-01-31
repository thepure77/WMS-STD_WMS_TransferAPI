using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TransferBusiness.Transfer;

namespace TransferBusiness.Transfer
{
    public class TransferViewModel
    {
        [Key]
        public Guid? Tag_Index { get; set; }

        public Guid? TagItemIndex { get; set; }

        public Guid? Tag_Index_From { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }

        [StringLength(50)]
        public string LpnNo { get; set; }

        [StringLength(50)]
        public string TagOutNo { get; set; }

        public Guid? GoodsIssueIndex { get; set; }

        public Guid? GoodsIssueItemIndex { get; set; }

        public Guid? GoodsReceiveIndex { get; set; }

        public Guid? GoodsReceiveItemIndex { get; set; }

        public Guid? GoodsReceiveItemLocationIndex { get; set; }

        public Guid? ProductIndex { get; set; }

        [StringLength(200)]
        public string TagNoFrom { get; set; }        

        [StringLength(50)]
        public string TagNoNew { get; set; }

        [StringLength(50)]
        public string Pallet_No { get; set; }

        public Guid? Pallet_Index { get; set; }

        public Guid? LocationIndex { get; set; }

        [StringLength(200)]
        public string TagRef_No1 { get; set; }

        [StringLength(200)]
        public string TagRef_No2 { get; set; }

        [StringLength(200)]
        public string TagRef_No3 { get; set; }

        [StringLength(200)]
        public string TagRef_No4 { get; set; }

        [StringLength(200)]
        public string TagRef_No5 { get; set; }

        public int? Tag_Status { get; set; }

        public int? PrintNumber { get; set; }

        [StringLength(200)]
        public string GoodsReceiveNo { get; set; }

        [StringLength(50)]
        public string LocationId { get; set; }

        [StringLength(200)]
        public string LocationName { get; set; }

        public Guid? LocationConfirm_Index { get; set; }

        [StringLength(50)]
        public string LocationConfirm_Id { get; set; }

        [StringLength(200)]
        public string LocationConfirm_Name { get; set; }

        [StringLength(50)]
        public string ProductId { get; set; }

        [StringLength(200)]
        public string ProductName { get; set; }

        [StringLength(200)]
        public string ProductSecondName { get; set; }

        [StringLength(200)]
        public string ProductThirdName { get; set; }

        [StringLength(200)]
        public string ProductLot { get; set; }


        public Guid? WareHouseIndex { get; set; }

        [StringLength(50)]
        public string WareHouseId { get; set; }

        [StringLength(200)]
        public string WareHouseName { get; set; }

        public Guid? ownerIndex { get; set; }

        [StringLength(50)]
        public string ownerId { get; set; }

        [StringLength(200)]
        public string ownerName { get; set; }

        public Guid? itemStatusIndex { get; set; }

        public Guid BinBalance_Index { get; set; }

        public decimal? BalanceQty_Begin { get; set; }

        public decimal? BalanceQty_Balance { get; set; }

        [StringLength(50)]
        public string ItemStatusId { get; set; }

        [StringLength(200)]
        public string ItemStatusTo { get; set; }

        public Guid? ItemStatusIndex_From { get; set; }

        [StringLength(200)]
        public string ItemStatusName_From { get; set; }

        [StringLength(200)]
        public string ItemStatusId_From { get; set; }    
           
        public Guid? ItemStatusIndex_To { get; set; }

        [StringLength(50)]
        public string ItemStatusId_To { get; set; }

        [StringLength(200)]
        public string ItemStatusName_To { get; set; }


        public string ExpireDate { get; set; }

        public string MFGDate { get; set; }

        public decimal? BinBalanceQtyBal { get; set; }

        public decimal? BinBalanceQtyReserve { get; set; }

        public decimal? BinBalanceRatio { get; set; }

        public decimal? Ratio { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Volume { get; set; }

        public decimal? BinBalanceWeightBal { get; set; }

        public decimal? TotalQty { get; set; }

        public decimal? BinBalanceVolumeBal { get; set; }

        public decimal? TagOutStatus { get; set; }

        public decimal? BinBalanceWeightBegin { get; set; }

        public decimal? BinBalanceQtyBegin { get; set; }

        public decimal? BinBalanceVolumeBegin { get; set; }

        public Guid? ProductConversionIndex { get; set; }

        [StringLength(50)]
        public string ProductConversionId { get; set; }

        [StringLength(200)]
        public string ProductConversionName { get; set; }

        [StringLength(200)]
        public string ProductConversionBarcode { get; set; }

        [StringLength(200)]
        public string UDF1 { get; set; }

        [StringLength(200)]
        public string UDF2 { get; set; }

        [StringLength(200)]
        public string UDF3 { get; set; }

        [StringLength(200)]
        public string UDF4 { get; set; }

        [StringLength(200)]
        public string UDF5 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime GoodsReceiveDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? MFG_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? EXP_Date { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public Guid? tagOutItemIndex { get; set; }

        [StringLength(50)]
        public string LocationNew { get; set; }

        public long? RowIndex { get; set; }
        public Guid? taskItemIndex { get; set; }



    }
    public class actionResultTransferViewModel
    {
        public string msgResult { get; set; }
        public IList<TransferViewModel> Formart { get; set; }

        public IList<TransferCartonViewModel> CheckData { get; set; }
        public IList<TransferCartonViewModel> itemsUse { get; set; }
        public IList<TransferViewModel> itemsLPN { get; set; }
        public IList<TransferViewModel> CheckSearchLPN { get; set; }
        public IList<GroupViewModel> itemsGroup { get; set; }
        public IList<SumQtyBinbalanceViewModel> SumQtyLoc { get; set; }
        public IList<SumQtyBinbalanceViewModel> SumQtyLPN { get; set; }
    }
}
