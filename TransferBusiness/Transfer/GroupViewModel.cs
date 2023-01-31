using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TransferBusiness.Transfer
{
    public partial class GroupViewModel
    {
        [Key]
        public Guid Tag_Index { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }

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

        [StringLength(50)]
        public string LocationId { get; set; }

        [StringLength(200)]
        public string LocationName { get; set; }

        [StringLength(50)]
        public string ProductId { get; set; }

        [StringLength(200)]
        public string ProductName { get; set; }

        public Guid? itemStatusIndex { get; set; }

        public Guid BinBalance_Index { get; set; }

        public decimal? BalanceQty_Begin { get; set; }

        public decimal? BalanceQty_Balance { get; set; }

        public Guid? GoodsReceive_Index { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        [StringLength(50)]
        public string ItemStatusFrom { get; set; }

        [StringLength(50)]
        public string itemStatusId { get; set; }

        public Guid? ItemStatusIndex_From { get; set; }

        [StringLength(50)]
        public string ItemStatusId_From { get; set; }

        [StringLength(200)]
        public string ItemStatusName_From { get; set; }

        public Guid? OwnerIndex { get; set; }

        public Guid? TagOutItemIndex { get; set; }

        [StringLength(50)]
        public string OwnerId { get; set; }

        [StringLength(200)]
        public string OwnerName { get; set; }

        [StringLength(200)]
        public string ItemStatusTo { get; set; }

        public string ExpireDate { get; set; }

        public decimal? Qty { get; set; }

        public decimal? BinBalanceRatio { get; set; }

        public decimal? BinBalanceQtyReserve { get; set; }

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

        public long? RowIndex { get; set; }

        public Guid? GoodsIssueIndex { get; set; }

        public Guid? GoodsIssueItemIndex { get; set; }

        public Guid? GoodsReceiveIndex { get; set; }

        public Guid? GoodsReceiveItemIndex { get; set; }

        public Guid? GoodsReceiveItemLocationIndex { get; set; }

        public Guid BinBalanceIndex { get; set; }

        public Guid TaskItemIndex { get; set; }
    }
}
