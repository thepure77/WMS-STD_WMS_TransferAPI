using DataAccess;
using  TransferBusiness.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Comone.Utils;


namespace TransferBusiness.Transfer
{
    public class TransferStatusGradeService
    {
        public List<TransferViewModel> ScanBarcode(SumQtyBinbalanceViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    string SqlWhere = "";

                    if (data.Tag_No != null && data.Tag_No != "")
                    {
                        SqlWhere = " and Tag_No = N'" + data.Tag_No + "'" +
                      " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                      " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                      " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";
                    }                   

                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var queryResult = context.View_BinBalance.FromSql("sp_GetBinBalanceTransfer @strwhere ", strwhere).ToList();

                    var group = queryResult.GroupBy(c => new { c.Product_Name, c.Product_Id, c.Location_Index, c.Location_Id, c.Location_Name })
                              .Select(c => new { c.Key.Product_Name, c.Key.Product_Id, c.Key.Location_Index, c.Key.Location_Id, c.Key.Location_Name })
                              .ToList();
                    Guid? locationIndex = new Guid();
                    var locationName = "";
                    var locationId = "";


                    var result = new List<TransferViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new TransferViewModel();
                        resultItem.Tag_No = item.Tag_No;
                        resultItem.Tag_Index = item.Tag_Index;
                        resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                        //resultItem.ExpireDate = item.GoodsReceive_EXP_Date.ToString();
                        resultItem.UDF1 = item.UDF_1;
                        resultItem.UDF2 = item.UDF_2;
                        resultItem.UDF3 = item.UDF_3;
                        resultItem.BinBalance_Index = item.BinBalance_Index;
                        //resultItem.BalanceQty_Begin = item.BinBalance_QtyBegin;
                        resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal;

                        foreach (var item1 in group.Where(c => c.Location_Name != null))
                        {
                            locationIndex = item1.Location_Index;
                            locationId = item1.Location_Id;
                            locationName = item1.Location_Name;
                            resultItem.LocationIndex = locationIndex;
                            resultItem.LocationId = locationId;
                            resultItem.LocationName = locationName;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ItemStatusId_From = item.ItemStatus_Id;
                            resultItem.ItemStatusIndex_From = item.ItemStatus_Index;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal;
                        }

                        if (group.Count == 1)
                        {
                            resultItem.LocationId = locationId;
                            resultItem.LocationIndex = locationIndex;
                            resultItem.LocationName = locationName;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ItemStatusId_From = item.ItemStatus_Id;
                            resultItem.ItemStatusIndex_From = item.ItemStatus_Index;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                        }
                        
                        resultItem.Update_By = item.Update_By;
                        result.Add(resultItem);
                    }


                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultTransferViewModel SumQty(SumQtyBinbalanceViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var actionResultSumQty = new actionResultTransferViewModel();

                    string SqlWhere = "";
                    string SqlWhere1 = "";
                    string SqlWhere2 = "";
                    if (data.LocationName != "" && data.productConversionBarcode == "")
                    {
                        SqlWhere = " and Location_Name = N'" + data.LocationName + "'" +
                       " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                       " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                       " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";
                    }

                    if (data.Tag_No != "" && data.Tag_No != null)
                    {
                        SqlWhere1 = " and Location_Name = N'" + data.LocationName + "'" +
                      " and Tag_No = N'" + data.Tag_No + "'" +
                      " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                      " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                      " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";
                    }

                    if (data.productConversionBarcode != "" && data.Tag_No == null && data.LocationName != "" && data.LocationName != null)
                    {
                        SqlWhere2 = " and ProductConversionBarcode = N'" + data.productConversionBarcode + "'" +
                      " and Location_Name = N'" + data.LocationName + "'" +
                      " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                      " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                      " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";
                    }
                    else
                    {
                        SqlWhere2 += " and ProductConversionBarcode = '" + "00000000000000" + "'";
                    }
                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                    var strwhere2 = new SqlParameter("@strwhere2", SqlWhere2);
                    var queryResult = context.View_SumQtyLocation.FromSql("sp_GetSumQtyLocation @strwhere", strwhere).Where(c => c.Location_Name != null).ToList();
                    var queryResult1 = context.View_BinBalance.FromSql("sp_GetBinBalanceTransfer @strwhere1", strwhere1).Where(c => c.Tag_No != null).ToList();
                    var queryResult2 = context.View_SumQtyBarcodeLocation.FromSql("sp_GetSumQtyBarcodeLocation @strwhere2", strwhere2).Where(c => c.ProductConversionBarcode != null).ToList();

                    var resultLoc = new List<SumQtyBinbalanceViewModel>();
                    var resultLPN = new List<SumQtyBinbalanceViewModel>();

                    var groupLoc = queryResult.GroupBy(c => new { c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();
                    var groupLPN = queryResult1.GroupBy(c => new { c.ProductConversion_Name, c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.ProductConversion_Name, c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();
                    var groupBarcodeLoc = queryResult2.GroupBy(c => new { c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();
                    if (queryResult.Count > 0)
                    {
                        foreach (var item in queryResult.GroupBy(c => c.Product_Name).ToList())
                        {
                            var resultItem = new SumQtyBinbalanceViewModel();
                            var sum = item.Sum(c => c.BinBalance_QtyBal);
                            resultItem.ProductName = item.Key;
                            resultItem.BinBalanceQtyBal = sum;
                            resultLoc.Add(resultItem);
                        }
                    }

                    if (queryResult1.Count > 0)
                    {
                        foreach (var item in queryResult1.GroupBy(c => c.Product_Name).ToList())
                        {
                            var resultItem1 = new SumQtyBinbalanceViewModel();
                            var sum = item.Sum(c => c.BinBalance_QtyBal);
                            resultItem1.ProductName = item.Key;
                            resultItem1.BinBalanceQtyBal = sum;
                            resultItem1.productConversionName = item.FirstOrDefault().ProductConversion_Name;
                            resultLPN.Add(resultItem1);
                        }
                    }

                    if (queryResult2.Count > 0)
                    {
                        foreach (var item in queryResult2.GroupBy(c => c.Product_Name).ToList())
                        {
                            var resultItem2 = new SumQtyBinbalanceViewModel();
                            var sum = item.Sum(c => c.BinBalance_QtyBal);
                            resultItem2.ProductName = item.Key;
                            resultItem2.BinBalanceQtyBal = sum;
                            resultLoc.Add(resultItem2);
                        }
                    }

                    actionResultSumQty.SumQtyLoc = resultLoc.ToList();
                    actionResultSumQty.SumQtyLPN = resultLPN.ToList();

                    return actionResultSumQty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public Boolean ConfirmScan(TransferViewModel data)
        //{

        //    var olog = new logtxt();
        //    bool isinnsert = false;
        //    bool isBinBalanceQtyReserve = false;
        //    String GoodsTransferIndex = "";
        //    String GoodsTransferItemIndex = "'x'";
        //    String BinCardReserveIndex = "'x'";
        //    var rbbinbalance = new List<BinBalanceViewModel>();
        //    try
        //    {
        //        using (var context = new TransferDbContext())
        //        {



        //            // Set Parameter 
        //            Guid GoodTransfer_Index = Guid.NewGuid();

        //            //-------------------- Transfer from DC (ASN) --------------------//
        //            Guid Process_Index = new Guid("408FD0AF-1592-4FA7-8BA0-03F6C3215D41");
        //            Guid DocType_Index = new Guid("063BC3F4-A8F5-48EA-8B36-ECCEAD297484");
        //            var TransferDocDate = String.Format("{0:MM/dd/yyyy}", DateTime.Now);

        //            DateTime oTransferDocDate = DateTime.Now;
        //            var GoodsTransferNo = "";
        //            var IsUse = new SqlParameter("@IsUse", Guid.NewGuid().ToString());


        //            var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
        //            var ColumnName2 = new SqlParameter("@ColumnName2", "DocumentType_Id");
        //            var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Name");
        //            var ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //            var ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //            var TableName = new SqlParameter("@TableName", "ms_DocumentType");
        //            var Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),DocumentType_Index)  ='" + DocType_Index.ToString() + "'");
        //            var DataDocumentType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();


        //            // Set Document No
        //            var DocumentType_Index = new SqlParameter("@DocumentType_Index", DocType_Index.ToString());
        //            var DocDate = new SqlParameter("@DocDate", TransferDocDate.ToString());
        //            var resultDocNoParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //            resultDocNoParameter.Size = 2000; // some meaningfull value
        //            resultDocNoParameter.Direction = ParameterDirection.Output;
        //            context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultDocNoParameter);
        //            //var result = resultParameter.Value;
        //            GoodsTransferNo = resultDocNoParameter.Value.ToString();



        //            var listGoodsTransfer = new List<GoodsTransferViewModel>();
        //            var listGoodsTransferItem = new List<GoodsTransferItemViewModel>();
        //            var listBinBalance = new List<BinBalanceViewModel>();
        //            var listBinCard = new List<BinCardViewModel>();
        //            var ListBinCardReserve = new List<BinCardReserveModel>();
        //            var listTagItem = new List<TagItemViewModel>();
        //            var listTagData = new List<TagViewModel>();
        //            var TagData = new TagViewModel();

        //            // Create HeaderTransfer 
        //            ////-------------------- GR GoodTransfer --------------------
        //            var GoodsTransfer = new GoodsTransferViewModel();
        //            GoodsTransferIndex = GoodTransfer_Index.ToString();
        //            GoodsTransfer.GoodsTransferIndex = GoodTransfer_Index;
        //            GoodsTransfer.OwnerIndex = data.ownerIndex;// new Guid(DataOwner[0].dataincolumn1);  //item.Owner_Index;
        //            GoodsTransfer.DocumentTypeIndex = DocType_Index;  //new Guid(DataDocumentType[0].dataincolumn1); ;
        //            GoodsTransfer.GoodsTransferNo = GoodsTransferNo;
        //            GoodsTransfer.GoodsTransferDate = oTransferDocDate;
        //            GoodsTransfer.DocumentRefNo1 = "";
        //            GoodsTransfer.DocumentRefNo2 = "";
        //            GoodsTransfer.DocumentRefNo3 = "";
        //            GoodsTransfer.DocumentRefNo4 = "";
        //            GoodsTransfer.DocumentRefNo5 = "";
        //            GoodsTransfer.DocumentStatus = 0;
        //            GoodsTransfer.UDF1 = "";
        //            GoodsTransfer.UDF2 = "";
        //            GoodsTransfer.UDF3 = "";
        //            GoodsTransfer.UDF4 = "";
        //            GoodsTransfer.UDF5 = "";
        //            GoodsTransfer.DocumentPriorityStatus = 0;
        //            GoodsTransfer.CreateBy = data.Update_By;
        //            GoodsTransfer.CreateDate = DateTime.Now;




        //            // Set Transfer Item
        //            string SqlWhere = " and Tag_No = N'" + data.Tag_No + "'" +
        //                               " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "' " +
        //                               " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50),Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
        //                               " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";

        //            var strwhere = new SqlParameter("@strwhere", SqlWhere);

        //            var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).Where(c => c.Location_Name != null).ToList();


        //            int iRows = 0;
        //            var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //            try
        //            {

        //                foreach (var item in queryResult)
        //                {

        //                    // Lock STOCK Balance 
        //                    String SqlcmdUpd = " Update [dbo].[wm_BinBalance] Set  IsUse =  @IsUse  where  isnull(IsUse,'') = ''  " + SqlWhere;
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpd, IsUse);


        //                    iRows = iRows + 1;
        //                    var GoodsTransferItem = new GoodsTransferItemViewModel();

        //                    var BinCardReserve = new BinCardReserveModel();

        //                    var rbbinbalanceDetal = new BinBalanceViewModel();

        //                    var TransferQty = item.BinBalance_QtyBal - item.BinBalance_QtyReserve;

        //                    ////-------------------- GR GoodTransferItem --------------------
        //                    Guid GoodTransferItem_Index = Guid.NewGuid();
        //                    GoodsTransferItemIndex += ",'" + GoodTransferItem_Index + "'";
        //                    GoodsTransferItem.GoodsTransferItemIndex = GoodTransferItem_Index;
        //                    GoodsTransferItem.GoodsTransferIndex = GoodsTransfer.GoodsTransferIndex;
        //                    GoodsTransferItem.GoodsReceiveIndex = item.GoodsReceive_Index;
        //                    GoodsTransferItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
        //                    GoodsTransferItem.GoodsReceiveItemLocationIndex = item.GoodsReceiveItemLocation_Index;
        //                    GoodsTransferItem.LineNum = iRows.ToString();
        //                    GoodsTransferItem.TagItemIndex = item.TagItem_Index;
        //                    GoodsTransferItem.TagIndex = item.Tag_Index;
        //                    GoodsTransferItem.TagIndexTo = item.Tag_Index;
        //                    GoodsTransferItem.ProductIndex = item.Product_Index;
        //                    GoodsTransferItem.ProductIndexTo = item.Product_Index;
        //                    GoodsTransferItem.ProductLot = item.Product_Lot;
        //                    GoodsTransferItem.ProductLotTo = item.Product_Lot;
        //                    GoodsTransferItem.ItemStatusIndex = item.ItemStatus_Index;
        //                    GoodsTransferItem.ItemStatusIndexTo = data.ItemStatusIndex_To;
        //                    GoodsTransferItem.ProductConversionIndex = item.ProductConversion_Index;
        //                    GoodsTransferItem.OwnerIndex = item.Owner_Index;//item.Owner_Index;
        //                    GoodsTransferItem.OwnerIndexTo = item.Owner_Index;
        //                    GoodsTransferItem.LocationIndex = item.Location_Index;
        //                    GoodsTransferItem.LocationIndexTo = item.Location_Index;
        //                    GoodsTransferItem.GoodsReceiveEXPDate = item.GoodsReceive_EXP_Date;
        //                    GoodsTransferItem.GoodsReceiveEXPDateTo = item.GoodsReceive_EXP_Date;
        //                    GoodsTransferItem.Qty = TransferQty;
        //                    GoodsTransferItem.TotalQty = TransferQty;
        //                    GoodsTransferItem.Weight = item.BinBalance_WeightBal;
        //                    GoodsTransferItem.Volume = item.BinBalance_VolumeBal;

        //                    //GoodsTransferItem.RefProcessIndex = itemList.Ref_Process_Index;
        //                    //GoodsTransferItem.RefDocumentNo = itemList.Ref_Document_No;
        //                    //GoodsTransferItem.RefDocumentIndex = itemList.Ref_Document_Index;
        //                    //GoodsTransferItem.RefDocumentItemIndex = itemList.Ref_DocumentItem_Index;
        //                    GoodsTransferItem.CreateBy = data.Update_By;
        //                    GoodsTransferItem.CreateDate = DateTime.Now;
        //                    //GoodsTransferItem.UpdateBy = data.Update_By;
        //                    //GoodsTransferItem.UpdateDate = DateTime.Now;
        //                    //GoodsTransferItem.CancelBy = "";
        //                    //GoodsTransferItem.CancelDate = data.Cancel_Date;
        //                    listGoodsTransferItem.Add(GoodsTransferItem);



        //                    // ADD Bin Reserve
        //                    BinCardReserve.BinCardReserve_Index = Guid.NewGuid();
        //                    BinCardReserveIndex += ",'" + BinCardReserve.BinCardReserve_Index + "'";
        //                    BinCardReserve.BinBalance_Index = item.BinBalance_Index;
        //                    BinCardReserve.Process_Index = Process_Index;  // GI Process
        //                    BinCardReserve.GoodsReceive_Index = item.GoodsReceive_Index;
        //                    BinCardReserve.GoodsReceive_No = item.GoodsReceive_No;
        //                    BinCardReserve.GoodsReceive_Date = item.GoodsReceive_Date;
        //                    BinCardReserve.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //                    BinCardReserve.TagItem_Index = item.TagItem_Index;
        //                    BinCardReserve.Tag_Index = item.Tag_Index;
        //                    BinCardReserve.Tag_No = item.Tag_No;
        //                    BinCardReserve.Product_Index = item.Product_Index;
        //                    BinCardReserve.Product_Id = item.Product_Id;
        //                    BinCardReserve.Product_Name = item.Product_Name;
        //                    BinCardReserve.Product_SecondName = item.Product_SecondName;
        //                    BinCardReserve.Product_ThirdName = item.Product_ThirdName;
        //                    BinCardReserve.Product_Lot = item.Product_Lot;
        //                    BinCardReserve.ItemStatus_Index = item.ItemStatus_Index;
        //                    BinCardReserve.ItemStatus_Id = item.ItemStatus_Id;
        //                    BinCardReserve.ItemStatus_Name = item.ItemStatus_Name;
        //                    BinCardReserve.MFG_Date = item.GoodsReceive_MFG_Date;
        //                    BinCardReserve.EXP_Date = item.GoodsReceive_EXP_Date;
        //                    BinCardReserve.ProductConversion_Index = item.ProductConversion_Index;
        //                    BinCardReserve.ProductConversion_Id = item.ProductConversion_Id;
        //                    BinCardReserve.ProductConversion_Name = item.ProductConversion_Name;
        //                    BinCardReserve.Owner_Index = item.Owner_Index;
        //                    BinCardReserve.Owner_Id = item.Owner_Id;
        //                    BinCardReserve.Owner_Name = item.Owner_Name;
        //                    BinCardReserve.Location_Index = item.Location_Index;
        //                    BinCardReserve.Location_Id = item.Location_Id;
        //                    BinCardReserve.Location_Name = item.Location_Name;
        //                    BinCardReserve.BinCardReserve_QtyBal = TransferQty;
        //                    if (item.BinBalance_WeightBegin == 0)
        //                    {
        //                        BinCardReserve.BinCardReserve_WeightBal = 0;
        //                    }
        //                    else
        //                    {
        //                        BinCardReserve.BinCardReserve_WeightBal = TransferQty * (item.BinBalance_QtyBegin / item.BinBalance_WeightBegin);

        //                    }
        //                    if (item.BinBalance_VolumeBegin == 0)
        //                    {
        //                        BinCardReserve.BinCardReserve_VolumeBal = 0;
        //                    }
        //                    else
        //                    {
        //                        BinCardReserve.BinCardReserve_VolumeBal = TransferQty * (item.BinBalance_QtyBegin / item.BinBalance_VolumeBegin);

        //                    }

        //                    BinCardReserve.Ref_Document_No = GoodsTransferNo;
        //                    BinCardReserve.Ref_Document_Index = GoodsTransferItem.GoodsTransferIndex;
        //                    BinCardReserve.Ref_DocumentItem_Index = GoodsTransferItem.GoodsTransferItemIndex;

        //                    BinCardReserve.Ref_Wave_Index = IsUse.Value.ToString();
        //                    BinCardReserve.Create_By = item.Update_By;
        //                    //      BinCardReserve.Create_Date =  item.Create_Date;

        //                    ListBinCardReserve.Add(BinCardReserve);

        //                    rbbinbalanceDetal.BinBalance_QtyReserve = TransferQty;
        //                    rbbinbalanceDetal.BinBalance_WeightReserve = BinCardReserve.BinCardReserve_WeightBal;
        //                    rbbinbalanceDetal.BinBalance_VolumeReserve = BinCardReserve.BinCardReserve_VolumeBal;
        //                    rbbinbalanceDetal.BinBalance_Index = item.BinBalance_Index;
        //                    rbbinbalance.Add(rbbinbalanceDetal);

        //                    var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", TransferQty);
        //                    var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserve.BinCardReserve_WeightBal);
        //                    var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserve.BinCardReserve_VolumeBal);
        //                    var BinBalance_Index = new SqlParameter("@BinBalance_Index", item.BinBalance_Index);
        //                    String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                             "     BinBalance_QtyReserve  =  BinBalance_QtyReserve + @BinBalance_QtyReserve " +
        //                                            "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve +  @BinBalance_WeightReserve " +
        //                                            "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve + @BinBalance_VolumeReserve " +
        //                                             "  where  BinBalance_Index = @BinBalance_Index  ";
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);
        //                    isBinBalanceQtyReserve = true;
        //                    olog.logging("Grade Adjustment", " Update [dbo].[wm_BinBalance]  SET " +
        //                                             "  BinBalance_QtyReserve  =  BinBalance_QtyReserve +" + TransferQty +
        //                                            "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve +" + BinCardReserve.BinCardReserve_WeightBal +
        //                                            "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve +" + BinCardReserve.BinCardReserve_VolumeBal +
        //                                             "  where  BinBalance_Index = " + item.BinBalance_Index + ");");



        //                }


        //                if (queryResult.Count > 0)
        //                {
        //                    // Clear LOCK
        //                    String SqlcmdUpF = " Update [dbo].[wm_BinBalance] Set  IsUse = '' where  isnull(IsUse,'') = @IsUse";
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpF, IsUse);
        //                }

        //                transaction.Commit();

        //            } // Try Transacation
        //            catch (Exception ex)
        //            {
        //                olog.logging("Grade Adjustment", " ex Rollback " + ex.Message.ToString());
        //                transaction.Rollback();
        //                throw ex;
        //            }

        //            listGoodsTransfer.Add(GoodsTransfer);
        //            DataTable dtGoodsTransfer = CreateDataTable(listGoodsTransfer);
        //            DataTable dtGoodsTransferItem = CreateDataTable(listGoodsTransferItem);
        //            DataTable dtBinCardReserve = CreateDataTable(ListBinCardReserve);

        //            ////  Save Transfer  and  BinCardReserve

        //            var pGoodsTransfer = new SqlParameter("GoodsTransfer", SqlDbType.Structured);
        //            pGoodsTransfer.TypeName = "[dbo].[im_GoodsTransferData]";
        //            pGoodsTransfer.Value = dtGoodsTransfer;

        //            var pGoodsTransferItem = new SqlParameter("GoodsTransferItem", SqlDbType.Structured);
        //            pGoodsTransferItem.TypeName = "[dbo].[im_GoodsTransferItemData]";
        //            pGoodsTransferItem.Value = dtGoodsTransferItem;

        //            var pBinCardReserve = new SqlParameter("BinCardReserve", SqlDbType.Structured);
        //            pBinCardReserve.TypeName = "[dbo].[wm_BinCardReserveData]";
        //            pBinCardReserve.Value = dtBinCardReserve;

        //            // ADD DATA To Stroe

        //            var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_TranferConfirm @GoodsTransfer, @GoodsTransferItem ,@BinCardReserve", pGoodsTransfer, pGoodsTransferItem, pBinCardReserve);
        //            isinnsert = true;
        //            olog.logging("Grade Adjustment", " insert im_GoodsTransfer  im_GoodsTransferItem  wm_BinCardReserve ");
        //            if (rowsAffected.ToString() != "0")
        //            {
                        

        //                // GetDataTransfer  For Confirm

        //                string SqlTFWhere = " and Convert(Nvarchar(50),GoodsTransfer_Index)   = N'" + GoodTransfer_Index.ToString() + "'";
        //                var strtfwhere = new SqlParameter("@strwhere", SqlTFWhere);

        //                var TransferResult = context.IM_GoodsTransfer.FromSql("sp_GetGoodsTransfer @strwhere", strtfwhere).ToList();
        //                var TransferItemResult = context.IM_GoodsTransferItem.FromSql("sp_GetGoodsTransferItem @strwhere", strtfwhere).ToList();

        //                string SqlBinCardReserveWhere = " and Convert(Nvarchar(50),Process_Index) = N'" + Process_Index .ToString() + "'" +
        //                                                " and Convert(Nvarchar(50),Ref_Document_Index) = N'" + GoodTransfer_Index.ToString() + "'";
        //                var strBinCardReservewhere = new SqlParameter("@strwhere", SqlBinCardReserveWhere);
        //                var BinCardReserveResult = context.WM_BinCardReserve.FromSql("sp_GetBinCardReserve @strwhere", strBinCardReservewhere).ToList();

        //                if (TransferResult.Count != 1 || TransferItemResult.Count < 1  || BinCardReserveResult.Count < 1) {
        //                    // Error 
        //                    return false;

        //                }

        //                transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    foreach (var BinCardReserveItem in BinCardReserveResult)
        //                    {
        //                        var BinBalance = new BinBalanceViewModel();
        //                        //   var BinCard = new BinCardViewModel();


        //                        //Select Data from Balance
        //                        string SqlBinBalanceWhere = " and Convert(Nvarchar(50), BinBalance_Index) = N'" + BinCardReserveItem.BinBalance_Index.ToString() + "'";
        //                        var strBinBalancewhere = new SqlParameter("@strwhere", SqlBinBalanceWhere);
        //                        var BinBalanceResult = context.wm_BinBalance2.FromSql("sp_GetBinBalance @strwhere", strBinBalancewhere).FirstOrDefault();

        //                        // Select By Line Item 
        //                        var TransferItemResultSelect = TransferItemResult.Where(c => c.GoodsTransferItem_Index == BinCardReserveItem.Ref_DocumentItem_Index).FirstOrDefault();

        //                        // Select Desc ItemStatus TO
        //                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
        //                        ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
        //                        ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
        //                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                        TableName = new SqlParameter("@TableName", "ms_ItemStatus");
        //                        Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),ItemStatus_Index)  ='" + TransferItemResultSelect.ItemStatus_Index_To.ToString() + "'");
        //                        var DatItemStatusTo = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

        //                        // Select Desc ItemStatus From
        //                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
        //                        ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
        //                        ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
        //                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                        TableName = new SqlParameter("@TableName", "ms_ItemStatus");
        //                        Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),ItemStatus_Index)  ='" + TransferItemResultSelect.ItemStatus_Index.ToString() + "'");
        //                        var DatItemStatusFrom = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

        //                        if (BinBalanceResult.BinBalance_QtyBal == BinCardReserveItem.BinCardReserve_QtyBal && BinBalanceResult.BinBalance_QtyBal == TransferItemResultSelect.TotalQty && BinBalanceResult.BinBalance_QtyBal == BinBalanceResult.BinBalance_QtyReserve)
        //                        {
        //                            // Update Binbalance  :  QtyBal and  Itemstatus


        //                            var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", BinCardReserveItem.BinCardReserve_QtyBal);
        //                            var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserveItem.BinCardReserve_WeightBal);
        //                            var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserveItem.BinCardReserve_VolumeBal);
        //                            var BinBalance_Index = new SqlParameter("@BinBalance_Index", BinCardReserveItem.BinBalance_Index);
        //                            var ItemStatus_Index = new SqlParameter("@ItemStatus_Index", TransferItemResultSelect.ItemStatus_Index_To);
        //                            var ItemStatus_Id = new SqlParameter("@ItemStatus_Id", DatItemStatusTo[0].dataincolumn2);
        //                            var ItemStatus_Name = new SqlParameter("@ItemStatus_Name", DatItemStatusTo[0].dataincolumn3);
        //                            String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                     "  BinBalance_QtyReserve  = BinBalance_QtyReserve - @BinBalance_QtyReserve " +
        //                                                     "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  @BinBalance_WeightReserve " +
        //                                                     "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - @BinBalance_VolumeReserve " +
        //                                                      " ,ItemStatus_Index  =  @ItemStatus_Index " +
        //                                                      " ,ItemStatus_Id    =   @ItemStatus_Id " +
        //                                                      " ,ItemStatus_Name  =   @ItemStatus_Name  " +
        //                                                     "  where  BinBalance_Index = @BinBalance_Index  ";
        //                            context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, ItemStatus_Index, ItemStatus_Id, ItemStatus_Name, BinBalance_Index);
        //                            olog.logging("Grade Adjustment", " Update [dbo].[wm_BinBalance]  SET " +
        //                                                   "  BinBalance_QtyReserve  = BinBalance_QtyReserve - "+ BinCardReserveItem.BinCardReserve_QtyBal +
        //                                                     "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  " + BinCardReserveItem.BinCardReserve_WeightBal +
        //                                                     "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - " + BinCardReserveItem.BinCardReserve_VolumeBal +
        //                                                      " ,ItemStatus_Index  = " + TransferItemResultSelect.ItemStatus_Index_To +
        //                                                      " ,ItemStatus_Id    =  " + DatItemStatusTo[0].dataincolumn2 +
        //                                                      " ,ItemStatus_Name  =  " + DatItemStatusTo[0].dataincolumn3 +
        //                                                    "  where  BinBalance_Index = " + BinCardReserveItem.BinBalance_Index);


        //                        }
        //                        else  // Split Line Binbalance 
        //                        {


        //                            var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", BinCardReserveItem.BinCardReserve_QtyBal);
        //                            var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserveItem.BinCardReserve_WeightBal);
        //                            var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserveItem.BinCardReserve_VolumeBal);
        //                            var BinBalance_Index = new SqlParameter("@BinBalance_Index", BinCardReserveItem.BinBalance_Index);
        //                            var ItemStatus_Index = new SqlParameter("@ItemStatus_Index", TransferItemResultSelect.ItemStatus_Index_To);
        //                            var ItemStatus_Id = new SqlParameter("@ItemStatus_Id", DatItemStatusTo[0].dataincolumn2);
        //                            var ItemStatus_Name = new SqlParameter("@ItemStatus_Name", DatItemStatusTo[0].dataincolumn3);
        //                            String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                     "  BinBalance_QtyBal  = BinBalance_QtyBal - @BinBalance_QtyReserve " +
        //                                                     "  ,BinBalance_WeightBal  = BinBalance_WeightBal -  @BinBalance_WeightReserve " +
        //                                                     "  ,BinBalance_VolumeBal  =  BinBalance_VolumeBal - @BinBalance_VolumeReserve " +
        //                                                     "  ,BinBalance_QtyReserve  = BinBalance_QtyReserve - @BinBalance_QtyReserve " +
        //                                                     "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  @BinBalance_WeightReserve " +
        //                                                     "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - @BinBalance_VolumeReserve " +
        //                                                     "  ,ItemStatus_Index  =  @ItemStatus_Index " +
        //                                                     "  ,ItemStatus_Id    =   @ItemStatus_Id " +
        //                                                     "  ,ItemStatus_Name  =   @ItemStatus_Name  " +
        //                                                     "  where  BinBalance_Index = @BinBalance_Index  ";
        //                            context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, ItemStatus_Index, ItemStatus_Id, ItemStatus_Name, BinBalance_Index);
        //                            olog.logging("Grade Adjustment", " Update [dbo].[wm_BinBalance]  SET " +
        //                                                     "  BinBalance_QtyBal  = BinBalance_QtyBal - " + BinCardReserveItem.BinCardReserve_QtyBal  +
        //                                                     "  ,BinBalance_WeightBal  = BinBalance_WeightBal - " + BinCardReserveItem.BinCardReserve_WeightBal +
        //                                                     "  ,BinBalance_VolumeBal  =  BinBalance_VolumeBal - " + BinCardReserveItem.BinCardReserve_VolumeBal +
        //                                                     "  ,BinBalance_QtyReserve  = BinBalance_QtyReserve - " + BinCardReserveItem.BinCardReserve_QtyBal +
        //                                                     "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve - " + BinCardReserveItem.BinCardReserve_WeightBal +
        //                                                     "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - " + BinCardReserveItem.BinCardReserve_VolumeBal +
        //                                                     "  ,ItemStatus_Index  = " + TransferItemResultSelect.ItemStatus_Index_To  +
        //                                                     "  ,ItemStatus_Id    =  " + DatItemStatusTo[0].dataincolumn2  +
        //                                                     "  ,ItemStatus_Name  =  " + DatItemStatusTo[0].dataincolumn3   +
        //                                                     "  where  BinBalance_Index = " + BinCardReserveItem.BinBalance_Index);

        //                            // Add TAG ITEM
        //                            var TagItem_Index = Guid.NewGuid();
        //                            var Tag_Index = Guid.NewGuid();
        //                            //  var Tag_Index = Guid.NewGuid();
        //                            string SqlTagWhere = " and Convert(Nvarchar(50), Tag_Index) = N'" + BinCardReserveItem.Tag_Index.ToString() + "' ";
        //                            var strTagwhere = new SqlParameter("@strwhere", SqlTagWhere);
        //                            var Tag = context.wm_Tag.FromSql("sp_GetTag @strwhere ", strTagwhere).FirstOrDefault();

        //                            string SqlTagItemWhere = " and Convert(Nvarchar(50), TagItem_Index) = N'" + BinCardReserveItem.TagItem_Index.ToString() + "' ";
        //                            var strTagItemwhere = new SqlParameter("@strwhere", SqlTagItemWhere);
        //                            var itemsTag = context.wm_TagItem.FromSql("sp_GetTagItem @strwhere ", strTagItemwhere).FirstOrDefault();

        //                            // CASE OLD TAG / LPN
        //                            if (Tag != null)
        //                            {
        //                                TagData = new TagViewModel();
        //                                TagData.TagIndex = Tag.Tag_Index;
        //                                TagData.TagNo = Tag.Tag_No;
        //                                TagData.PalletNo = Tag.Pallet_No;
        //                                TagData.PalletIndex = Tag.Pallet_Index;
        //                                TagData.TagRefNo1 = Tag.TagRef_No1;
        //                                TagData.TagRefNo2 = Tag.TagRef_No2;
        //                                TagData.TagRefNo3 = Tag.TagRef_No3;
        //                                TagData.TagRefNo4 = Tag.TagRef_No4;
        //                                TagData.TagRefNo5 = Tag.TagRef_No5;
        //                                TagData.TagStatus = Tag.Tag_Status;
        //                                TagData.UDF1 = Tag.UDF_1;
        //                                TagData.UDF2 = Tag.UDF_2;
        //                                TagData.UDF3 = Tag.UDF_3;
        //                                TagData.UDF4 = Tag.UDF_4;
        //                                TagData.UDF5 = Tag.UDF_5;
        //                                TagData.CreateBy = Tag.Create_By;
        //                                //    TagData.CreateDate = DateTime.Now;


        //                            }
        //                            //else

        //                            //{
        //                            //    //Create New TAG / New LPN
        //                            //    TagData = new TagViewModel();
        //                            //    TagData.TagIndex = Tag_Index;
        //                            //    TagData.TagNo = BinCardReserveItem.Tag_No;
        //                            //    TagData.PalletNo = "";
        //                            //    TagData.PalletIndex = new Guid();
        //                            //    TagData.TagRefNo1 = "";
        //                            //    TagData.TagRefNo2 = "";
        //                            //    TagData.TagRefNo3 = "";
        //                            //    TagData.TagRefNo4 = "";
        //                            //    TagData.TagRefNo5 = "";
        //                            //    TagData.TagStatus = 0;
        //                            //    TagData.UDF1 = "";
        //                            //    TagData.UDF2 = "";
        //                            //    TagData.UDF3 = "";
        //                            //    TagData.UDF4 = "";
        //                            //    TagData.UDF5 = "";
        //                            //    TagData.CreateBy = Tag.Create_By;


        //                            //}



        //                            if (itemsTag != null)
        //                            {
        //                                var TagItem = new TagItemViewModel();

        //                                TagItem.TagItemIndex = TagItem_Index;
        //                                TagItem.TagIndex = itemsTag.Tag_Index;
        //                                TagItem.TagNo = itemsTag.Tag_No;
        //                                TagItem.GoodsReceiveIndex = itemsTag.GoodsReceive_Index;
        //                                TagItem.GoodsReceiveItemIndex = itemsTag.GoodsReceiveItem_Index;
        //                                TagItem.ProductIndex = itemsTag.Product_Index;
        //                                TagItem.ProductId = itemsTag.Product_Id;
        //                                TagItem.ProductName = itemsTag.Product_Name;
        //                                TagItem.ProductSecondName = itemsTag.Product_SecondName;
        //                                TagItem.ProductThirdName = itemsTag.Product_ThirdName;
        //                                TagItem.ProductLot = itemsTag.Product_Lot;
        //                                TagItem.ItemStatusIndex = itemsTag.ItemStatus_Index;
        //                                TagItem.ItemStatusId = itemsTag.ItemStatus_Id;
        //                                TagItem.ItemStatusName = itemsTag.ItemStatus_Name;
        //                                TagItem.Qty = (BinCardReserveItem.BinCardReserve_QtyBal / itemsTag.Ratio);
        //                                TagItem.Ratio = itemsTag.Ratio;
        //                                TagItem.TotalQty = BinCardReserveItem.BinCardReserve_QtyBal;
        //                                TagItem.ProductConversionIndex = itemsTag.ProductConversion_Index;
        //                                TagItem.ProductConversionId = itemsTag.ProductConversion_Id;
        //                                TagItem.ProductConversionName = itemsTag.ProductConversion_Name;
        //                                TagItem.Weight = itemsTag.Weight;
        //                                TagItem.Volume = itemsTag.Volume;
        //                                TagItem.MFGDate = itemsTag.MFG_Date;
        //                                TagItem.EXPDate = itemsTag.EXP_Date;
        //                                TagItem.TagRefNo1 = itemsTag.TagRef_No1;
        //                                TagItem.TagRefNo2 = itemsTag.TagRef_No2;
        //                                TagItem.TagRefNo3 = itemsTag.TagRef_No3;
        //                                TagItem.TagRefNo4 = itemsTag.TagRef_No4;
        //                                TagItem.TagRefNo5 = itemsTag.TagRef_No5;
        //                                TagItem.TagStatus = itemsTag.Tag_Status;
        //                                TagItem.UDF1 = itemsTag.UDF_1;
        //                                TagItem.UDF2 = itemsTag.UDF_2;
        //                                TagItem.UDF3 = itemsTag.UDF_3;
        //                                TagItem.UDF4 = itemsTag.UDF_4;
        //                                TagItem.UDF5 = itemsTag.UDF_5;
        //                                TagItem.CreateBy = data.Update_By;
        //                                // TagItem.CreateDate = DateTime.Now;

        //                                listTagItem.Add(TagItem);
        //                            }


        //                            // Add New Line Binbalance 

        //                            Guid pBinBalance_Index = Guid.NewGuid();

        //                            BinBalance.BinBalance_Index = pBinBalance_Index;
        //                            BinBalance.Owner_Index = new Guid(BinCardReserveItem.Owner_Index.ToString());//item.Owner_Index;
        //                            BinBalance.Owner_Id = BinCardReserveItem.Owner_Id;//item.Owner_Id;
        //                            BinBalance.Owner_Name = BinCardReserveItem.Owner_Name; // item.Owner_Name;
        //                            BinBalance.LocationIndex = new Guid(BinCardReserveItem.Location_Index.ToString());//item.Location_Index;
        //                            BinBalance.LocationId = BinCardReserveItem.Location_Id; //item.Location_Id;
        //                            BinBalance.LocationName = BinCardReserveItem.Location_Name;//item.Location_Name;
        //                            BinBalance.GoodsReceive_Index = new Guid(BinCardReserveItem.GoodsReceive_Index.ToString());
        //                            BinBalance.GoodsReceive_No = BinCardReserveItem.GoodsReceive_No; //item.GoodsReceive_No;  
        //                            BinBalance.GoodsReceive_Date = BinCardReserveItem.GoodsReceive_Date;  //item.GoodsReceive_Date;
        //                            BinBalance.GoodsReceiveItem_Index = new Guid(BinCardReserveItem.GoodsReceiveItem_Index.ToString());
        //                            BinBalance.GoodsReceiveItemLocation_Index = new Guid(TransferItemResultSelect.GoodsReceiveItemLocation_Index.ToString());//item.GoodsReceiveItemLocation_Index;
        //                            BinBalance.TagItem_Index = TagItem_Index;
        //                            if (itemsTag != null)
        //                            {
        //                                BinBalance.Tag_Index = new Guid(itemsTag.Tag_Index.ToString());
        //                                BinBalance.Tag_No = itemsTag.Tag_No;
        //                            }
                                    
        //                            BinBalance.Product_Index = new Guid(BinCardReserveItem.Product_Index.ToString());
        //                            BinBalance.Product_Id = BinCardReserveItem.Product_Id;
        //                            BinBalance.Product_Name = BinCardReserveItem.Product_Name;
        //                            BinBalance.Product_SecondName = BinCardReserveItem.Product_SecondName;
        //                            BinBalance.Product_ThirdName = BinCardReserveItem.Product_ThirdName;
        //                            BinBalance.Product_Lot = BinCardReserveItem.Product_Lot;
        //                            BinBalance.ItemStatus_Index = TransferItemResultSelect.ItemStatus_Index_To;
        //                            BinBalance.ItemStatus_Id = DatItemStatusTo[0].dataincolumn2;
        //                            BinBalance.ItemStatus_Name = DatItemStatusTo[0].dataincolumn3;
        //                            BinBalance.GoodsReceive_MFG_Date = BinCardReserveItem.MFG_Date;
        //                            BinBalance.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;

        //                            BinBalance.GoodsReceive_ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                            BinBalance.GoodsReceive_ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                            BinBalance.GoodsReceive_ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;
        //                            BinBalance.BinBalance_Ratio = BinBalanceResult.BinBalance_Ratio;
        //                            BinBalance.BinBalance_QtyBegin = BinCardReserveItem.BinCardReserve_QtyBal;
        //                            BinBalance.BinBalance_WeightBegin = BinCardReserveItem.BinCardReserve_WeightBal;
        //                            BinBalance.BinBalance_VolumeBegin = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                            BinBalance.BinBalance_QtyBal = BinCardReserveItem.BinCardReserve_QtyBal;
        //                            BinBalance.BinBalance_WeightBal = BinCardReserveItem.BinCardReserve_WeightBal;
        //                            BinBalance.BinBalance_VolumeBal = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                            BinBalance.BinBalance_QtyReserve = 0;
        //                            BinBalance.BinBalance_WeightReserve = 0;
        //                            BinBalance.BinBalance_VolumeReserve = 0;
        //                            BinBalance.ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                            BinBalance.ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                            BinBalance.ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;
        //                            BinBalance.UDF_1 = BinBalanceResult.UDF_1;
        //                            BinBalance.UDF_2 = BinBalanceResult.UDF_2;
        //                            BinBalance.UDF_3 = BinBalanceResult.UDF_3;
        //                            BinBalance.UDF_4 = BinBalanceResult.UDF_4;
        //                            BinBalance.UDF_5 = BinBalanceResult.UDF_5;
        //                            BinBalance.Create_By = data.Update_By;

        //                            listBinBalance.Add(BinBalance);

        //                        }

        //                        ////--------------------Bin Card FROM--------------------
        //                        var BinCard = new BinCardViewModel();
        //                        BinCard.BinCard_Index = Guid.NewGuid();
        //                        BinCard.Process_Index = Process_Index;//BinCardReserveItem.Process_Index;
        //                        BinCard.DocumentType_Index = DocType_Index; //BinCardReserveItem.DocumentType_Index;
        //                        BinCard.DocumentType_Id = DataDocumentType[0].dataincolumn2;//BinCardReserveItem.DocumentType_Id;
        //                        BinCard.DocumentType_Name = DataDocumentType[0].dataincolumn3;//BinCardReserveItem.DocumentType_Name;
        //                        BinCard.GoodsReceive_Index = BinCardReserveItem.GoodsReceive_Index;
        //                        BinCard.GoodsReceiveItem_Index = BinCardReserveItem.GoodsReceiveItem_Index;
        //                        BinCard.GoodsReceiveItemLocation_Index = TransferItemResultSelect.GoodsReceiveItemLocation_Index;//BinCardReserveItem.GoodsReceiveItemLocation_Index;
        //                        BinCard.BinCard_No = TransferResult[0].GoodsTransfer_No; //BinCardReserveItem.BinCard_No;
        //                        BinCard.BinCard_Date = TransferResult[0].GoodsTransfer_Date; //BinCardReserveItem.BinCard_Date;
        //                        BinCard.TagItem_Index = BinCardReserveItem.TagItem_Index;
        //                        BinCard.Tag_Index = BinCardReserveItem.Tag_Index;
        //                        BinCard.Tag_No = BinBalanceResult.Tag_No;
        //                        BinCard.Tag_Index_To = BinCardReserveItem.Tag_Index; //BinCardReserveItem.Tag_Index_To;
        //                        BinCard.Tag_No_To = BinBalanceResult.Tag_No;  //  ไม่ได้เปลี่ยนTAG
        //                        BinCard.Product_Index = BinCardReserveItem.Product_Index;
        //                        BinCard.Product_Id = BinCardReserveItem.Product_Id;
        //                        BinCard.Product_Name = BinCardReserveItem.Product_Name;
        //                        BinCard.Product_SecondName = BinCardReserveItem.Product_SecondName;
        //                        BinCard.Product_ThirdName = BinCardReserveItem.Product_ThirdName;
        //                        BinCard.Product_Index_To = BinCardReserveItem.Product_Index; //BinCardReserveItem.Product_Index_To;
        //                        BinCard.Product_Id_To = BinCardReserveItem.Product_Id;
        //                        BinCard.Product_Name_To = BinCardReserveItem.Product_Name;
        //                        BinCard.Product_SecondName_To = BinCardReserveItem.Product_SecondName;
        //                        BinCard.Product_ThirdName_To = BinCardReserveItem.Product_ThirdName;
        //                        BinCard.Product_Lot = BinCardReserveItem.Product_Lot;
        //                        BinCard.Product_Lot_To = BinCardReserveItem.Product_Lot;
        //                        BinCard.ItemStatus_Index = TransferItemResultSelect.ItemStatus_Index;
        //                        BinCard.ItemStatus_Id = DatItemStatusFrom[0].dataincolumn2;
        //                        BinCard.ItemStatus_Name = DatItemStatusFrom[0].dataincolumn3;

        //                        BinCard.ItemStatus_Index_To = TransferItemResultSelect.ItemStatus_Index;
        //                        BinCard.ItemStatus_Id_To = DatItemStatusFrom[0].dataincolumn2;
        //                        BinCard.ItemStatus_Name_To = DatItemStatusFrom[0].dataincolumn3;

        //                        BinCard.ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                        BinCard.ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                        BinCard.ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;

        //                        BinCard.Owner_Index = BinCardReserveItem.Owner_Index;//BinCardReserveItem.Owner_Index;
        //                        BinCard.Owner_Id = BinCardReserveItem.Owner_Id;//BinCardReserveItem.Owner_Id;
        //                        BinCard.Owner_Name = BinCardReserveItem.Owner_Name; // BinCardReserveItem.Owner_Name;

        //                        BinCard.Owner_Index_To = BinCardReserveItem.Owner_Index;
        //                        BinCard.Owner_Id_To = BinCardReserveItem.Owner_Id;
        //                        BinCard.Owner_Name_To = BinCardReserveItem.Owner_Name;


        //                        BinCard.Location_Index = BinCardReserveItem.Location_Index;//BinCardReserveItem.Location_Index;
        //                        BinCard.Location_Id = BinCardReserveItem.Location_Id; //BinCardReserveItem.Location_Id;
        //                        BinCard.Location_Name = BinCardReserveItem.Location_Name;//BinCardReserveItem.Location_Name;

        //                        BinCard.Location_Index_To = BinCardReserveItem.Location_Index;
        //                        BinCard.Location_Id_To = BinCardReserveItem.Location_Id;
        //                        BinCard.Location_Name_To = BinCardReserveItem.Location_Name;


        //                        BinCard.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;
        //                        BinCard.GoodsReceive_EXP_Date_To = BinCardReserveItem.EXP_Date;
        //                        BinCard.BinCard_QtyIn = 0;
        //                        BinCard.BinCard_QtyOut = BinCardReserveItem.BinCardReserve_QtyBal;
        //                        BinCard.BinCard_QtySign = BinCardReserveItem.BinCardReserve_QtyBal * -1;
        //                        BinCard.BinCard_WeightIn = 0;
        //                        BinCard.BinCard_WeightOut = BinCardReserveItem.BinCardReserve_WeightBal;

        //                        if (BinCardReserveItem.BinCardReserve_WeightBal == 0)
        //                        {
        //                            BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal;
        //                        }
        //                        else
        //                        {
        //                            BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal * -1;
        //                        }

        //                        BinCard.BinCard_VolumeIn = 0;
        //                        BinCard.BinCard_VolumeOut = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                        if (BinCardReserveItem.BinCardReserve_VolumeBal == 0)
        //                        {
        //                            BinCard.BinCard_VolumeSign = 0;
        //                        }
        //                        else
        //                        {
        //                            BinCard.BinCard_VolumeSign = BinCardReserveItem.BinCardReserve_VolumeBal * -1;

        //                        }
        //                        BinCard.Ref_Document_No = TransferResult[0].GoodsTransfer_No;
        //                        BinCard.Ref_Document_Index = BinCardReserveItem.Ref_Document_Index; //tem.Ref_Document_Index;
        //                        BinCard.Ref_DocumentItem_Index = BinCardReserveItem.Ref_DocumentItem_Index;
        //                        BinCard.Create_By = data.Update_By;
        //                        //BinCard.Create_Date = BinCardReserveItem.CreateDate;

        //                        listBinCard.Add(BinCard);


        //                        ////------------------------------------------------




        //                        ////--------------------Bin Card TO--------------------
        //                        BinCard = new BinCardViewModel();

        //                        BinCard.BinCard_Index = Guid.NewGuid();
        //                        BinCard.Process_Index = Process_Index;//BinCardReserveItem.Process_Index;
        //                        BinCard.DocumentType_Index = DocType_Index; //BinCardReserveItem.DocumentType_Index;
        //                        BinCard.DocumentType_Id = DataDocumentType[0].dataincolumn2;//BinCardReserveItem.DocumentType_Id;
        //                        BinCard.DocumentType_Name = DataDocumentType[0].dataincolumn3;//BinCardReserveItem.DocumentType_Name;
        //                        BinCard.GoodsReceive_Index = BinCardReserveItem.GoodsReceive_Index;
        //                        BinCard.GoodsReceiveItem_Index = BinCardReserveItem.GoodsReceiveItem_Index;
        //                        BinCard.GoodsReceiveItemLocation_Index = TransferItemResultSelect.GoodsReceiveItemLocation_Index;//BinCardReserveItem.GoodsReceiveItemLocation_Index;
        //                        BinCard.BinCard_No = TransferResult[0].GoodsTransfer_No; //BinCardReserveItem.BinCard_No;
        //                        BinCard.BinCard_Date = TransferResult[0].GoodsTransfer_Date; //BinCardReserveItem.BinCard_Date;
        //                        BinCard.TagItem_Index = BinCardReserveItem.TagItem_Index;
        //                        BinCard.Tag_Index = BinCardReserveItem.Tag_Index;
        //                        BinCard.Tag_No = BinBalanceResult.Tag_No;
        //                        BinCard.Tag_Index_To = BinCardReserveItem.Tag_Index; //BinCardReserveItem.Tag_Index_To;
        //                        BinCard.Tag_No_To = BinBalanceResult.Tag_No;  //  ไม่ได้เปลี่ยนTAG
        //                        BinCard.Product_Index = BinCardReserveItem.Product_Index;
        //                        BinCard.Product_Id = BinCardReserveItem.Product_Id;
        //                        BinCard.Product_Name = BinCardReserveItem.Product_Name;
        //                        BinCard.Product_SecondName = BinCardReserveItem.Product_SecondName;
        //                        BinCard.Product_ThirdName = BinCardReserveItem.Product_ThirdName;
        //                        BinCard.Product_Index_To = BinCardReserveItem.Product_Index; //BinCardReserveItem.Product_Index_To;
        //                        BinCard.Product_Id_To = BinCardReserveItem.Product_Id;
        //                        BinCard.Product_Name_To = BinCardReserveItem.Product_Name;
        //                        BinCard.Product_SecondName_To = BinCardReserveItem.Product_SecondName;
        //                        BinCard.Product_ThirdName_To = BinCardReserveItem.Product_ThirdName;
        //                        BinCard.Product_Lot = BinCardReserveItem.Product_Lot;
        //                        BinCard.Product_Lot_To = BinCardReserveItem.Product_Lot;

        //                        BinCard.ItemStatus_Index = TransferItemResultSelect.ItemStatus_Index_To;
        //                        BinCard.ItemStatus_Id = DatItemStatusTo[0].dataincolumn2;
        //                        BinCard.ItemStatus_Name = DatItemStatusTo[0].dataincolumn3;

        //                        // Null ฝั่ง From
        //                        BinCard.ItemStatus_Index_To = TransferItemResultSelect.ItemStatus_Index_To;
        //                        BinCard.ItemStatus_Id_To = DatItemStatusTo[0].dataincolumn2;
        //                        BinCard.ItemStatus_Name_To = DatItemStatusTo[0].dataincolumn3;

        //                        BinCard.ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                        BinCard.ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                        BinCard.ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;

        //                        BinCard.Owner_Index = BinCardReserveItem.Owner_Index;//BinCardReserveItem.Owner_Index;
        //                        BinCard.Owner_Id = BinCardReserveItem.Owner_Id;//BinCardReserveItem.Owner_Id;
        //                        BinCard.Owner_Name = BinCardReserveItem.Owner_Name; // BinCardReserveItem.Owner_Name;

        //                        BinCard.Owner_Index_To = BinCardReserveItem.Owner_Index;
        //                        BinCard.Owner_Id_To = BinCardReserveItem.Owner_Id;
        //                        BinCard.Owner_Name_To = BinCardReserveItem.Owner_Name;


        //                        BinCard.Location_Index = BinCardReserveItem.Location_Index;//BinCardReserveItem.Location_Index;
        //                        BinCard.Location_Id = BinCardReserveItem.Location_Id; //BinCardReserveItem.Location_Id;
        //                        BinCard.Location_Name = BinCardReserveItem.Location_Name;//BinCardReserveItem.Location_Name;

        //                        BinCard.Location_Index_To = BinCardReserveItem.Location_Index;
        //                        BinCard.Location_Id_To = BinCardReserveItem.Location_Id;
        //                        BinCard.Location_Name_To = BinCardReserveItem.Location_Name;


        //                        BinCard.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;
        //                        BinCard.GoodsReceive_EXP_Date_To = BinCardReserveItem.EXP_Date;
        //                        BinCard.BinCard_QtyIn = BinCardReserveItem.BinCardReserve_QtyBal;
        //                        BinCard.BinCard_QtyOut = 0;
        //                        BinCard.BinCard_QtySign = BinCardReserveItem.BinCardReserve_QtyBal;
        //                        BinCard.BinCard_WeightIn = BinCardReserveItem.BinCardReserve_WeightBal;
        //                        BinCard.BinCard_WeightOut = 0;
        //                        BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal;
        //                        BinCard.BinCard_VolumeIn = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                        BinCard.BinCard_VolumeOut = 0;
        //                        BinCard.BinCard_VolumeSign = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                        BinCard.Ref_Document_No = TransferResult[0].GoodsTransfer_No;
        //                        BinCard.Ref_Document_Index = BinCardReserveItem.Ref_Document_Index; //tem.Ref_Document_Index;
        //                        BinCard.Ref_DocumentItem_Index = BinCardReserveItem.Ref_DocumentItem_Index;
        //                        BinCard.Create_By = data.Update_By;
        //                        //BinCard.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;
        //                        //BinCard.GoodsReceive_EXP_Date_To = BinCardReserveItem.EXP_Date;
        //                        //BinCard.BinCard_QtyIn = 0;
        //                        //BinCard.BinCard_QtyOut = BinCardReserveItem.BinCardReserve_QtyBal;
        //                        //BinCard.BinCard_QtySign = BinCardReserveItem.BinCardReserve_QtyBal;
        //                        //BinCard.BinCard_WeightIn = 0;
        //                        //BinCard.BinCard_WeightOut = BinCardReserveItem.BinCardReserve_WeightBal;
        //                        //BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal;
        //                        //BinCard.BinCard_VolumeIn = 0;
        //                        //BinCard.BinCard_VolumeOut = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                        //BinCard.BinCard_VolumeSign = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                        //BinCard.Ref_Document_No = TransferResult[0].GoodsTransfer_No;
        //                        //BinCard.Ref_Document_Index = BinCardReserveItem.Ref_Document_Index; //tem.Ref_Document_Index;
        //                        //BinCard.Ref_DocumentItem_Index = BinCardReserveItem.Ref_DocumentItem_Index;
        //                        //BinCard.Create_By = data.Update_By;

        //                        //BinCard.Create_Date = BinCardReserveItem.CreateDate;

        //                        listBinCard.Add(BinCard);

        //                        ////------------------------------------------------


        //                    }

        //                    // Add  Bincacd BinBalance TAG  To DATATABLE 

        //                    listTagData.Add(TagData);
        //                    DataTable dtTag = CreateDataTable(listTagData);
        //                    DataTable dtTagItem = CreateDataTable(listTagItem);
        //                    DataTable dtBinBalance = CreateDataTable(listBinBalance);
        //                    DataTable dtBinCard = CreateDataTable(listBinCard);


        //                    var pBinBalance = new SqlParameter("BinBalance", SqlDbType.Structured);
        //                    pBinBalance.TypeName = "[dbo].[wm_BinBalanceTransferData]";
        //                    pBinBalance.Value = dtBinBalance;

        //                    var pBinCard = new SqlParameter("BinCard", SqlDbType.Structured);
        //                    pBinCard.TypeName = "[dbo].[wm_BinCardData]";
        //                    pBinCard.Value = dtBinCard;


        //                    var pTag = new SqlParameter("Tag", SqlDbType.Structured);
        //                    pTag.TypeName = "[dbo].[wm_TagTransferData]";
        //                    pTag.Value = dtTag;

        //                    var pTagItem = new SqlParameter("TagItem", SqlDbType.Structured);
        //                    pTagItem.TypeName = "[dbo].[wm_TagItemTransferData]";
        //                    pTagItem.Value = dtTagItem;
        //                    var rowsAffected1 = context.Database.ExecuteSqlCommand("sp_Save_BinBalanceTransfer @BinBalance,@BinCard,@Tag,@TagItem", pBinBalance, pBinCard, pTag, pTagItem);
        //                    olog.logging("Grade Adjustment", "insert BinCard");
        //                    transaction.Commit();

        //                    if (rowsAffected.ToString() != "0")
        //                    {
        //                        return true;
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }

        //                }// Try Transacation
        //                catch (Exception ex)
        //                {
        //                    olog.logging("Grade Adjustment", " ex Rollback " + ex.Message.ToString());
        //                    transaction.Rollback();
        //                    throw ex;
        //                    // Clear Reserve


        //                }


        //                //// Add  Bincacd BinBalance TAG  To DATATABLE 

        //                //listTagData.Add(TagData);
        //                //DataTable dtTag = CreateDataTable(listTagData);
        //                //DataTable dtTagItem = CreateDataTable(listTagItem);
        //                //DataTable dtBinBalance = CreateDataTable(listBinBalance);
        //                //DataTable dtBinCard = CreateDataTable(listBinCard);
                

        //                //var pBinBalance = new SqlParameter("BinBalance", SqlDbType.Structured);
        //                //pBinBalance.TypeName = "[dbo].[wm_BinBalanceTransferData]";
        //                //pBinBalance.Value = dtBinBalance;

        //                //var pBinCard = new SqlParameter("BinCard", SqlDbType.Structured);
        //                //pBinCard.TypeName = "[dbo].[wm_BinCardData]";
        //                //pBinCard.Value = dtBinCard;


        //                //var pTag = new SqlParameter("Tag", SqlDbType.Structured);
        //                //pTag.TypeName = "[dbo].[wm_TagTransferData]";
        //                //pTag.Value = dtTag;

        //                //var pTagItem = new SqlParameter("TagItem", SqlDbType.Structured);
        //                //pTagItem.TypeName = "[dbo].[wm_TagItemTransferData]";
        //                //pTagItem.Value = dtTagItem;

        //                //var transactionGrade = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //                //try
        //                //{
        //                //    var rowsAffected1 = context.Database.ExecuteSqlCommand("sp_Save_BinBalanceTransfer @BinBalance,@BinCard,@Tag,@TagItem", pBinBalance, pBinCard, pTag, pTagItem);
        //                //    transactionGrade.Commit();

        //                //}
        //                //catch (Exception exy)
        //                //{
        //                //    transactionGrade.Rollback();
        //                //    throw exy;
        //                //}
        //                ////// Add Bincacd binbalance TAG  To Stroe 
        //                //if (rowsAffected.ToString() != "0")
        //                //{
        //                //    return true;
        //                //}
        //                //else
        //                //{
        //                //    return false;
        //                //}





        //            }
        //            else
        //            {
        //                // Clear Reserve



        //            }




        //            return true;

                    

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        olog.logging("Pallet Relocation", " ex Rollback " + ex.Message.ToString());
        //        using (var context = new TransferDbContext())
        //        {
        //            var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //            try
        //            {
        //                if (isinnsert)
        //                {

        //                    BinCardReserveIndex = BinCardReserveIndex.Replace("'x',", "");
        //                    if (GoodsTransferIndex != "")
        //                    {
        //                        String SqlCmd3 = "";
        //                        SqlCmd3 = " Delete from wm_BinCardReserve where Convert(Varchar(200), BinCardReserve_Index) in (" + BinCardReserveIndex + ")";
        //                        context.Database.ExecuteSqlCommand(SqlCmd3);
        //                        olog.logging("Pallet Relocation", " Rollback Delete from wm_BinCardReserve where Convert(Varchar(200), BinCardReserve_Index) in (" + BinCardReserveIndex + ")");

        //                    }
        //                    GoodsTransferItemIndex = GoodsTransferItemIndex.Replace("'x',", "");
        //                    if (GoodsTransferItemIndex != "")
        //                    {
        //                        String SqlCmd2 = "";
        //                        SqlCmd2 = " Delete from im_GoodsTransferItem where Convert(Varchar(200), GoodsTransferItem_Index) in (" + GoodsTransferItemIndex + ")";
        //                        context.Database.ExecuteSqlCommand(SqlCmd2);
        //                        olog.logging("Pallet Relocation", " Rollback Delete from im_GoodsTransferItem where Convert(Varchar(200), GoodsTransferItem_Index) in (" + GoodsTransferItemIndex + ")");

        //                    }
        //                    if (GoodsTransferIndex != "")
        //                    {
        //                        String SqlCmd = "";
        //                        SqlCmd = " Delete from im_GoodsTransfer where Convert(Varchar(200), GoodsTransfer_Index)  ='" + GoodsTransferIndex + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);
        //                        olog.logging("Pallet Relocation", " Rollback Delete from im_GoodsTransfer where Convert(Varchar(200), GoodsTransfer_Index) ='" + GoodsTransferIndex + "'");
        //                    }
        //                }
        //                if (isBinBalanceQtyReserve)
        //                {
        //                    if (rbbinbalance.Count > 0)
        //                    {
        //                        foreach (var i in rbbinbalance)
        //                        {
        //                            var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", i.BinBalance_QtyReserve);
        //                            var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", i.BinBalance_WeightReserve);
        //                            var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", i.BinBalance_VolumeReserve);
        //                            var BinBalance_Index = new SqlParameter("@BinBalance_Index", i.BinBalance_Index);
        //                            String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                     "  BinBalance_QtyReserve  =  BinBalance_QtyReserve - @BinBalance_QtyReserve " +
        //                                                    "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  @BinBalance_WeightReserve " +
        //                                                    "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - @BinBalance_VolumeReserve " +
        //                                                     "  where  BinBalance_Index = @BinBalance_Index  ";
        //                            context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);
        //                            olog.logging("Pallet Relocation", " Rollback Update [dbo].[wm_BinBalance]  SET " +
        //                                           "  BinBalance_QtyReserve  =  BinBalance_QtyReserve -" + i.BinBalance_QtyReserve +
        //                                          "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -" + i.BinBalance_WeightReserve +
        //                                          "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve -" + i.BinBalance_VolumeReserve +
        //                                           "  where  BinBalance_Index = " + i.BinBalance_Index + ");");
        //                        }
        //                    }
        //                }
        //                transaction.Commit();
        //            }
        //            catch (Exception exy)
        //            {
        //                olog.logging("Pallet Relocation", " exy Rollback " + exy.Message.ToString());
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //        throw ex;
        //    }
        //}

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

    }
}
