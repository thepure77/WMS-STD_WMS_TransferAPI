using DataAccess;
using TransferBusiness.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Comone.Utils;
using TransferBusiness.Library;
using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.ConfigModel;
using TransferDataAccess.Models;
using planGIBusiness.AutoNumber;
using MasterDataBusiness.ViewModels;

namespace TransferBusiness.Transfer
{
    public class TransferPalletService
    {
        private TransferDbContext db;
        private BinbalanceDbContext dbbin;
        private string _defaultStagingLocation = new AppSettingConfig().GetUrl("defaultStagingLocation");

        public TransferPalletService()
        {
            db = new TransferDbContext();
            dbbin = new BinbalanceDbContext();
        }

        public TransferPalletService(TransferDbContext db , BinbalanceDbContext dbbin)
        {
            this.db = db;
            this.dbbin = dbbin;
        }

        //public actionResultTransferViewModel GroupProduct(listTransferItem data)
        //{
        //    try
        //    {
        //        using (var context = new TransferDbContext())
        //        {
        //            var result = new List<TransferViewModel>();
        //            var resultNew = new List<GroupViewModel>();
        //            var actionResultLPN = new actionResultTransferViewModel();

        //            var group = data.listTransferItemViewModel.GroupBy(c => new { c.ProductName, c.ProductId, c.BalanceQty_Balance }).Select(c => new { c.Key.ProductName, c.Key.ProductId, c.Key.BalanceQty_Balance, }).ToList();
        //            if (group.Count > 0)
        //            {
        //                foreach (var item in group)
        //                {
        //                    var resultItem = new GroupViewModel();

        //                    resultItem.ProductId = item.ProductId;
        //                    resultItem.ProductName = item.ProductName;
        //                    resultItem.Qty = item.BalanceQty_Balance;
        //                    resultNew.Add(resultItem);
        //                }
        //            }


        //            actionResultLPN.itemsGroup = resultNew.ToList();
        //            actionResultLPN.itemsLPN = result.ToList();

        //            return actionResultLPN;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<GoodsTransferItemViewModel> FilterTransferItem(GoodsTransferViewModel model)
        {
            try
            {
                var result = new List<GoodsTransferItemViewModel>();

                using (var context = new TransferDbContext())
                {
                    //Guid GoodsTransferIndex = new Guid(id);

                    //var queryResult = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransferIndex && c.Document_Status != -1).ToList();
                    var queryResult = (from gti in db.IM_GoodsTransferItem
                                       join gt in db.IM_GoodsTransfer
                                       on gti.GoodsTransfer_Index equals gt.GoodsTransfer_Index
                                       select new
                                       {
                                           gti.GoodsTransferItem_Index,
                                           gt.GoodsTransfer_Index,
                                           gt.GoodsTransfer_No,
                                           gti.Tag_Index,
                                           gti.Tag_No,
                                           gti.Product_Index,
                                           gti.Product_Id,
                                           gti.Product_Name,
                                           gti.Qty,
                                           gti.ProductConversion_Index,
                                           gti.ProductConversion_Id,
                                           gti.ProductConversion_Name,
                                           gt.Document_Status
                                       }).Distinct().Where(c => c.GoodsTransfer_No == model.goodsTransfer_No && c.Document_Status != -1).ToList().OrderBy(c => c.GoodsTransfer_No);


                    foreach (var data in queryResult)
                    {
                        var item = new GoodsTransferItemViewModel();

                        item.goodsTransferItem_Index = data.GoodsTransferItem_Index;
                        item.goodsTransfer_Index = data.GoodsTransfer_Index;

                        item.product_Index = data.Product_Index;
                        item.product_Id = data.Product_Id;
                        item.product_Name = data.Product_Name;
                        //item.location_Index = data.Location_Index;
                        //item.location_Id = data.Location_Id;
                        //item.location_Name = data.Location_Name;
                        //item.location_Index_To = data.Location_Index_To;
                        //item.location_Id_To = data.Location_Id_To;
                        //item.location_Name_To = data.Location_Name_To;
                        //item.itemStatus_Index = data.ItemStatus_Index;
                        //item.itemStatus_Id = data.ItemStatus_Id;
                        //item.itemStatus_Name = data.ItemStatus_Name;
                        //item.itemStatus_Index_To = data.ItemStatus_Index_To;
                        //item.itemStatus_Id_To = data.ItemStatus_Id_To;
                        //item.itemStatus_Name_To = data.ItemStatus_Name_To;

                        item.tag_Index = data.Tag_Index;
                        item.tag_No = data.Tag_No;
                        item.qty = data.Qty;
                        item.pick = data.Qty;

                        item.productConversion_Index = data.ProductConversion_Index;
                        item.productConversion_Id = data.ProductConversion_Id;
                        item.productConversion_Name = data.ProductConversion_Name;

                        result.Add(item);
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BinBalanceViewModel> ScanLpnNo(TransferViewModel data)
        {
            try
            {

                var result = new List<BinBalanceViewModel>();
                var lstBinBalance = new List<BinBalanceViewModel>();

                var resBinBalance = new BinBalanceViewModel();
                //resBinBalance.owner_Index = new Guid(data.ownerIndex.ToString());
                resBinBalance.tag_No = data.LpnNo;

                lstBinBalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), resBinBalance.sJson());

                //using (var context = new TransferDbContext())
                //{
                //    string SqlWhere = "";
                //    if (data.LpnNo != null)
                //    {
                //        SqlWhere = " and Tag_No = N'" + data.LpnNo + "'" +
                //        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                //        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                //        " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve = 0 ";
                //    }
                //    else
                //    {
                //        SqlWhere = " and Location_Name = N'" + "00000000000000" + "'";
                //    }

                //    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                //    var result = new List<TransferViewModel>();
                //    var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).Where(c => c.Location_Name != null).ToList();
                //    if (queryResult.Count > 0)
                //    {
                //        var group = queryResult.GroupBy(c => new { c.Product_Name, c.Product_Id, c.Location_Index, c.Location_Id, c.Location_Name })
                //              .Select(c => new { c.Key.Product_Name, c.Key.Product_Id, c.Key.Location_Index, c.Key.Location_Id, c.Key.Location_Name })
                //              .ToList();
                //        Guid? locationIndex = new Guid();
                //        var locationName = "";
                //        var locationId = "";



                //        foreach (var item in queryResult)
                //        {
                //            var resultItem = new TransferViewModel();
                //            resultItem.Tag_No = item.Tag_No;
                //            resultItem.Tag_Index = item.Tag_Index;
                //            //resultItem.ExpireDate = item.GoodsReceive_EXP_Date.ToString();
                //            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                //            resultItem.UDF1 = item.UDF_1;
                //            resultItem.UDF2 = item.UDF_2;
                //            resultItem.UDF3 = item.UDF_3;
                //            resultItem.BinBalance_Index = item.BinBalance_Index;
                //            resultItem.BalanceQty_Begin = item.BinBalance_QtyBegin;
                //            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal;

                //            foreach (var item1 in group.Where(c => c.Location_Name != null))
                //            {
                //                locationIndex = item1.Location_Index;
                //                locationId = item1.Location_Id;
                //                locationName = item1.Location_Name;
                //                resultItem.LocationIndex = locationIndex;
                //                resultItem.LocationId = locationId;
                //                resultItem.LocationName = locationName;
                //                resultItem.ProductIndex = item.Product_Index;
                //                resultItem.ProductSecondName = item.Product_SecondName;
                //                resultItem.ProductThirdName = item.Product_ThirdName;
                //                resultItem.ProductId = item.Product_Id;
                //                resultItem.ProductName = item.Product_Name;
                //                resultItem.ItemStatusName_From = item.ItemStatus_Name;
                //                resultItem.ItemStatusId_From = item.ItemStatus_Id;
                //                resultItem.ItemStatusIndex_From = item.ItemStatus_Index;
                //                resultItem.ProductConversionName = item.ProductConversion_Name;
                //                resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal;
                //            }

                //            if (group.Count == 1)
                //            {
                //                resultItem.LocationId = locationId;
                //                resultItem.LocationIndex = locationIndex;
                //                resultItem.LocationName = locationName;
                //                resultItem.ProductId = item.Product_Id;
                //                resultItem.ProductIndex = item.Product_Index;
                //                resultItem.ProductIndex = item.Product_Index;
                //                resultItem.ProductSecondName = item.Product_SecondName;
                //                resultItem.ProductThirdName = item.Product_ThirdName;
                //                resultItem.ItemStatusName_From = item.ItemStatus_Name;
                //                resultItem.ItemStatusId_From = item.ItemStatus_Id;
                //                resultItem.ItemStatusIndex_From = item.ItemStatus_Index;
                //                resultItem.ProductConversionName = item.ProductConversion_Name;
                //            }

                //            resultItem.Create_Date = item.Create_Date.GetValueOrDefault();
                //            resultItem.Create_By = item.Create_By;
                //            resultItem.Update_Date = item.Update_Date.GetValueOrDefault();
                //            resultItem.Update_By = item.Update_By;
                //            result.Add(resultItem);
                //        }

                return lstBinBalance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BinBalanceViewModel> ScanLpnNoStatusloaction(TransferViewModel data)
        {
            try
            {

                var result = new List<BinBalanceViewModel>();
                var lstBinBalance = new List<BinBalanceViewModel>();

                var resBinBalance = new BinBalanceViewModel();
                //resBinBalance.owner_Index = new Guid(data.ownerIndex.ToString());
                resBinBalance.tag_No = data.LpnNo;

                lstBinBalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), resBinBalance.sJson());

                foreach (var item in lstBinBalance)
                {
                    if (item.binBalance_QtyReserve != 0)
                    {
                        var lstBinBalance_null = new List<BinBalanceViewModel>();
                        return lstBinBalance_null;
                    }
                }
                return lstBinBalance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean ScanLocation(TransferViewModel data)
        {
            try
            {

                //LocationViewModel DataLocation = new LocationViewModel();

                #region Get Tag
                var resultTag = new LPNViewModel();
                var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_No = data.TagNoNew } };
                var tag = new DocumentViewModel();
                tag.listDocumentViewModel = listTag;
                resultTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), tag.sJson()).FirstOrDefault();
                #endregion

                //var binBalance = new BinBalanceViewModel();
                //var resBinBalance = new BinBalanceViewModel();
                ////resBinBalance.owner_Index = new Guid(data.ownerIndex.ToString());
                //resBinBalance.tag_No = data.TagNoNew;

                //binBalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), resBinBalance.sJson()).FirstOrDefault();

                //if (binBalance != null)
                //{
                //    #region Get Location
                //    var LocationViewModel = new LocationViewModel();
                //    LocationViewModel.location_Index = binBalance.location_Index;
                //    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), LocationViewModel.sJson());
                //    DataLocation = GetLocation.FirstOrDefault();
                //    #endregion
                //}

                if (resultTag != null)
                {
                    return true;
                }
                else
                {
                    return false;
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

                    //if (data.lpnNo != "")
                    //{
                    //    SqlWhere = " and Tag_No = N'" + data.lpnNo + "'" +
                    //  " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                    //  " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                    //  " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve = 0 ";
                    //}
                    //else
                    //{
                    //    SqlWhere += " and Tag_No = '" + "00000000000000" + "'";
                    //}
                    //var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    //var queryResult1 = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).Where(c => c.Tag_No != null).ToList();

                    var queryResult1 = dbbin.wm_BinBalance.Where(c => c.Tag_No == data.Tag_No && ((c.BinBalance_QtyBal - c.BinBalance_QtyReserve) > 0) && (c.BinBalance_QtyBal > 0) && (c.BinBalance_QtyReserve == 0)).ToList();

                    var resultLPN = new List<SumQtyBinbalanceViewModel>();

                    var groupLPN = queryResult1.GroupBy(c => new { c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();

                    if (queryResult1.Count > 0)
                    {
                        foreach (var item in queryResult1.GroupBy(c => new { c.Product_Name, c.Product_SecondName, c.Product_ThirdName }).ToList())
                        {
                            var resultItem1 = new SumQtyBinbalanceViewModel();
                            var sum = item.Sum(c => c.BinBalance_QtyBal);
                            resultItem1.ProductName = item.Key.Product_Name;
                            resultItem1.ProductSecondName = item.Key.Product_SecondName;
                            resultItem1.ProductThirdName = item.Key.Product_ThirdName;
                            resultItem1.BinBalanceQtyBal = sum;
                            var MaxQty = item.Max(c => c.BinBalance_QtyBal);
                            var ProductConversion = item.Where(c => c.BinBalance_QtyBal == MaxQty).Select(c => c.ProductConversion_Name).FirstOrDefault();
                            resultItem1.productConversionName = ProductConversion;
                            resultLPN.Add(resultItem1);
                        }
                    }

                    actionResultSumQty.SumQtyLPN = resultLPN.ToList();

                    return actionResultSumQty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Confirm(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            //String GoodsTransferNo = "";
            //Guid GoodsTransfer_Index = Guid.Empty;
            //var result = false;

            String GoodsTransferNo = "";
            Guid GoodsTransfer_Index = Guid.Empty;

            try
            {

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    var GoodsTransfer = new GoodsTransferViewModel();

                    var filterModel = new DocumentTypeViewModel();
                    var result = new List<GenDocumentTypeViewModel>();

                    filterModel.process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");
                    filterModel.documentType_Index = data.documentType_Index;
                    ////GetConfig
                    //result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());
                    result = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                    //DataTable resultDocumentType = CreateDataTable(result);

                    //var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                    //DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                    //DocumentType.Value = resultDocumentType;

                    //var DocumentType_Index = new SqlParameter("@DocumentType_Index", data.documentType_Index.ToString());
                    //var DocDate = new SqlParameter("@DocDate", data.goodsTransfer_Date.toDate());
                    //var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    //resultParameter.Size = 2000; // some meaningfull value
                    //resultParameter.Direction = ParameterDirection.Output;
                    //db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                    //GoodsTransferNo = resultParameter.Value.ToString();

                    var genDoc = new AutoNumberService();
                    //string DocNo = "";
                    DateTime DocumentDate = (DateTime)data.goodsTransfer_Date.toDate();
                    GoodsTransferNo = genDoc.genAutoDocmentNumber(result, DocumentDate);


                    State = "CreateGoodsTransferHeader";
                    IM_GoodsTransfer itemHeader = new IM_GoodsTransfer();

                    GoodsTransfer_Index = Guid.NewGuid();
                    itemHeader.GoodsTransfer_Index = GoodsTransfer_Index;
                    itemHeader.GoodsTransfer_No = GoodsTransferNo;
                    itemHeader.GoodsTransfer_Date = data.goodsTransfer_Date.toDateDefault(); //Convert.ToDateTime(data.goodsTransfer_Date.toDateTimeString());
                    itemHeader.GoodsTransfer_Time = data.goodsTransfer_Time;
                    itemHeader.Owner_Index = data.owner_Index;
                    itemHeader.Owner_Id = data.owner_Id;
                    itemHeader.Owner_Name = data.owner_Name;
                    itemHeader.DocumentType_Index = data.documentType_Index;
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;

                    itemHeader.Document_Status = -2;

                    itemHeader.Create_By = data.create_By;
                    itemHeader.Create_Date = DateTime.Now;

                    db.IM_GoodsTransfer.Add(itemHeader);

                    data.goodsTransfer_Index = GoodsTransfer_Index;
                    data.goodsTransfer_No = GoodsTransferNo;

                    BinBalanceViewModel modelTagNew = new BinBalanceViewModel();
                    modelTagNew.tag_No = data.tagNoNew;
                    var dataBinbalanceTransfer = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), modelTagNew.sJson()).FirstOrDefault(); ;

                    #region Get default Location
                    //LocationViewModel DataLocation = new LocationViewModel();
                    //var LocationViewModel = new LocationViewModel();
                    //LocationViewModel.location_Name = "SP001";
                    var LocationViewModel = new { location_Name = _defaultStagingLocation };
                    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), LocationViewModel.sJson());
                    var DataLocation = GetLocation.FirstOrDefault();
                    #endregion

                    Guid tagOld_Index = Guid.Empty;
                    string _tag_Index = "";
                    if (data.lstPickProduct.Count > 0)
                    {
                        foreach (var itemDetails in data.lstPickProduct)
                        {
                            Guid GoodsTransferItem_Index = Guid.Empty;
                            GoodsTransferItem_Index = Guid.NewGuid();
                            itemDetails.isuse = GoodsTransfer_Index.ToString();
                            var resultBinbalance = new actionResultPickbinbalanceViewModel();
                            var dataBinbalance = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), itemDetails.sJson());
                            if (!dataBinbalance.resultIsUse)
                            {
                                resultBinbalance.resultMsg = "";
                                resultBinbalance.resultIsUse = false;
                                return false;
                            }
                            else
                            {
                                var resIsUse = utils.SendDataApi<Boolean>(new AppSettingConfig().GetUrl("updateIsuseBinbalance"), itemDetails.sJson());

                                var TransferQty = dataBinbalance.binBalance_QtyBal - dataBinbalance.binBalance_QtyReserve; //Qty to transfer

                                if (TransferQty < itemDetails.pick)
                                {
                                    return false;
                                }

                                if (resIsUse)
                                {
                                    if (!string.IsNullOrEmpty(itemDetails.unit?.productConversion_Index.ToString()))
                                    {
                                        itemDetails.productConversion_Ratio = itemDetails.unit.productconversion_Ratio;
                                    }

                                    var GoodsTransferItem = new IM_GoodsTransferItem();
                                    GoodsTransferItem.GoodsTransferItem_Index = GoodsTransferItem_Index;
                                    GoodsTransferItem.GoodsTransfer_Index = data.goodsTransfer_Index;
                                    GoodsTransferItem.TagItem_Index = dataBinbalance.tagItem_Index;
                                    GoodsTransferItem.Tag_Index = dataBinbalance.tag_Index;
                                    GoodsTransferItem.Tag_No = dataBinbalance.tag_No;

                                    GoodsTransferItem.Product_Index = dataBinbalance.product_Index;
                                    GoodsTransferItem.Product_Id = dataBinbalance.product_Id;
                                    GoodsTransferItem.Product_Name = dataBinbalance.product_Name;
                                    GoodsTransferItem.Product_SecondName = dataBinbalance.product_SecondName;
                                    GoodsTransferItem.Product_ThirdName = dataBinbalance.product_ThirdName;
                                    GoodsTransferItem.Product_Index_To = dataBinbalance.product_Index;
                                    GoodsTransferItem.Product_Id_To = dataBinbalance.product_Id;
                                    GoodsTransferItem.Product_Name_To = dataBinbalance.product_Name;
                                    GoodsTransferItem.Product_SecondName_To = dataBinbalance.product_SecondName;
                                    GoodsTransferItem.Product_ThirdName_To = dataBinbalance.product_ThirdName;
                                    GoodsTransferItem.Product_Lot = dataBinbalance.product_Lot;
                                    GoodsTransferItem.ItemStatus_Index = dataBinbalance.itemStatus_Index;
                                    GoodsTransferItem.ItemStatus_Id = dataBinbalance.itemStatus_Id;
                                    GoodsTransferItem.ItemStatus_Name = dataBinbalance.itemStatus_Name;
                                    GoodsTransferItem.ItemStatus_Index_To = dataBinbalance.itemStatus_Index;
                                    GoodsTransferItem.ItemStatus_Id_To = dataBinbalance.itemStatus_Id;
                                    GoodsTransferItem.ItemStatus_Name_To = dataBinbalance.itemStatus_Name;
                                    GoodsTransferItem.Location_Index = dataBinbalance.location_Index;
                                    GoodsTransferItem.Location_Id = dataBinbalance.location_Id;
                                    GoodsTransferItem.Location_Name = dataBinbalance.location_Name;

                                    if (dataBinbalanceTransfer != null)
                                    {
                                        GoodsTransferItem.Tag_Index_To = dataBinbalanceTransfer.tag_Index;
                                        GoodsTransferItem.Tag_No_To = dataBinbalanceTransfer.tag_No;
                                        GoodsTransferItem.Location_Index_To = dataBinbalanceTransfer.location_Index;
                                        GoodsTransferItem.Location_Id_To = dataBinbalanceTransfer.location_Id;
                                        GoodsTransferItem.Location_Name_To = dataBinbalanceTransfer.location_Name;
                                    }
                                    else
                                    {
                                        GoodsTransferItem.Location_Index_To = DataLocation.location_Index;
                                        GoodsTransferItem.Location_Id_To = DataLocation.location_Id;
                                        GoodsTransferItem.Location_Name_To = DataLocation.location_Name;
                                    }

                                    GoodsTransferItem.Owner_Index = data.owner_Index;
                                    GoodsTransferItem.Owner_Id = data.owner_Id;
                                    GoodsTransferItem.Owner_Name = data.owner_Name;
                                    //GoodsIssueItemLocation.Qty = (Decimal)QtyBal / (Decimal)model.productConversion_Ratio;
                                    //GoodsIssueItemLocation.Ratio = (Decimal)model.productConversion_Ratio;
                                    //GoodsIssueItemLocation.TotalQty = (Decimal)QtyBal;
                                    GoodsTransferItem.Qty = (Decimal)itemDetails.pick;
                                    GoodsTransferItem.Ratio = (Decimal)dataBinbalance.binBalance_Ratio;
                                    GoodsTransferItem.TotalQty = (Decimal)itemDetails.pick * (Decimal)dataBinbalance.binBalance_Ratio;
                                    GoodsTransferItem.ProductConversion_Index = (Guid)dataBinbalance.productConversion_Index;
                                    GoodsTransferItem.ProductConversion_Id = dataBinbalance.productConversion_Id;
                                    GoodsTransferItem.ProductConversion_Name = dataBinbalance.productConversion_Name;
                                    GoodsTransferItem.GoodsReceive_EXP_Date = dataBinbalance.goodsReceive_EXP_Date;
                                    GoodsTransferItem.GoodsReceive_EXP_Date_To = dataBinbalance.goodsReceive_EXP_Date;
                                    GoodsTransferItem.Weight = (Decimal)dataBinbalance.binBalance_WeightBal;

                                    GoodsTransferItem.Volume = (Decimal)dataBinbalance.binBalance_VolumeBal;

                                    GoodsTransferItem.Document_Status = -2;

                                    //GoodsTransferItem.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process

                                    GoodsTransferItem.GoodsReceiveItem_Index = dataBinbalance.goodsReceiveItem_Index;
                                    GoodsTransferItem.GoodsReceive_Index = dataBinbalance.goodsReceive_Index;
                                    GoodsTransferItem.Create_By = data.create_By;
                                    GoodsTransferItem.Create_Date = DateTime.Now;

                                    db.IM_GoodsTransferItem.Add(GoodsTransferItem);

                                    PickbinbalanceViewModel pickBinBalance = new PickbinbalanceViewModel();
                                    pickBinBalance.goodsTransfer_Index = GoodsTransferItem.GoodsTransfer_Index.ToString();
                                    pickBinBalance.goodsTransferItem_Index = GoodsTransferItem.GoodsTransferItem_Index.ToString();
                                    pickBinBalance.goodsTransfer_No = GoodsTransferNo;
                                    pickBinBalance.process_Index = "CE757517-EBBC-4BEA-93CC-F7E139AE422C";
                                    //model.GIIL = GoodsIssueItemLocation;
                                    pickBinBalance.goodsReceive_Index = dataBinbalance.goodsReceive_Index.ToString();
                                    pickBinBalance.goodsReceive_No = dataBinbalance.goodsReceive_No;
                                    pickBinBalance.goodsReceive_date = dataBinbalance.goodsReceive_Date.toString();

                                    pickBinBalance.binbalance_Index = itemDetails.binbalance_Index;
                                    pickBinBalance.pick = itemDetails.pick;
                                    pickBinBalance.productConversion_Ratio = (Decimal)dataBinbalance.binBalance_Ratio;
                                    pickBinBalance.create_By = itemDetails.create_By;

                                    var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve"), pickBinBalance.sJson());
                                    if (insetBinRe.resultIsUse)
                                    {
                                        itemDetails.binCardReserve_Index = insetBinRe.items?.binCardReserve_Index;
                                        //model.binCard_Index = insetBinRe.items?.binCard_Index;
                                        //var update_gti = db.IM_GoodsTransferItem.Find(GoodsTransferItem.GoodsTransferItem_Index);
                                        //update_gti.Document_Status = 0;
                                        var update_gt = db.IM_GoodsTransfer.Find(GoodsTransfer_Index);
                                        update_gt.Owner_Index = dataBinbalance.owner_Index;
                                        update_gt.Owner_Id = dataBinbalance.owner_Id;
                                        update_gt.Owner_Name = dataBinbalance.owner_Name;
                                        update_gt.Document_Status = 3;
                                        //db.SaveChanges();
                                    }
                                    else
                                    {
                                        msglog = State + " ex Rollback " + insetBinRe.resultMsg.ToString();
                                        olog.logging("Pick Transfer Pallet", msglog);
                                        itemDetails.isActive = true;
                                        utils.SendDataApi<Boolean>(new AppSettingConfig().GetUrl("updateIsuseBinbalance"), itemDetails.sJson());
                                        throw new Exception();
                                    }


                                    //if (BinBalanceResult.BinBalance_QtyBal == BinCardReserve.BinCardReserve_QtyBal && BinBalanceResult.BinBalance_QtyBal == GoodsTransferItem.TotalQty && BinBalanceResult.BinBalance_QtyBal == BinBalanceResult.BinBalance_QtyReserve)
                                    //{

                                    //}

                                    if (dataBinbalanceTransfer != null)
                                    {
                                        var resultTag = new LPNViewModel();
                                        var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_Index = dataBinbalanceTransfer.tag_Index } };
                                        var tag = new DocumentViewModel();
                                        tag.listDocumentViewModel = listTag;
                                        resultTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), tag.sJson()).FirstOrDefault();
                                        _tag_Index = resultTag.tag_Index.ToString();
                                    }

                                    if(_tag_Index == "")
                                    {
                                        GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                                        grModel.goodsReceive_Index = dataBinbalance.goodsReceive_Index;
                                        grModel.owner_Index = dataBinbalance.owner_Index;
                                        grModel.tag_No = data.tagNoNew;
                                        grModel.create_By = itemDetails.create_By;
                                        _tag_Index = utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTag"), grModel.sJson());
                                    }

                                    // If tag_Index
                                    if(!string.IsNullOrEmpty(_tag_Index))
                                    {
                                        #region Check Binbalance binBalance_QtyReserve
                                        var binBalanceReserve = new
                                        {
                                            binbalance_Index = itemDetails.binbalance_Index
                                        };
                                        var dataBinbalanceReserve = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), binBalanceReserve.sJson());
                                        #endregion

                                        if (dataBinbalance.binBalance_QtyBal == itemDetails.pick && dataBinbalanceReserve.binBalance_QtyBal == dataBinbalanceReserve.binBalance_QtyReserve)
                                        {
                                            GoodsReceiveTagItemViewModel item = new GoodsReceiveTagItemViewModel();

                                            item.tag_Index = new Guid(_tag_Index.ToString());
                                            item.tag_No = data.tagNoNew;
                                            item.goodsReceive_Index = dataBinbalance.goodsReceive_Index;
                                            item.goodsReceiveItem_Index = new Guid(dataBinbalance.goodsReceiveItem_Index.ToString());
                                            item.process_Index = filterModel.process_Index;
                                            item.product_Index = dataBinbalance.product_Index;
                                            item.product_Id = dataBinbalance.product_Id;
                                            item.product_Name = dataBinbalance.product_Name;
                                            item.product_SecondName = dataBinbalance.product_SecondName;
                                            item.product_ThirdName = dataBinbalance.product_ThirdName;
                                            item.product_Lot = dataBinbalance.product_Lot;
                                            item.itemStatus_Index = dataBinbalance.itemStatus_Index;
                                            item.itemStatus_Id = dataBinbalance.itemStatus_Id;
                                            item.itemStatus_Name = dataBinbalance.itemStatus_Name;
                                            item.qty = itemDetails.pick;
                                            item.productConversion_Ratio = Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                            item.totalQty = Convert.ToDecimal(itemDetails.pick) * Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                            item.productConversion_Index = new Guid(dataBinbalance.productConversion_Index.ToString());
                                            item.productConversion_Id = dataBinbalance.productConversion_Id;
                                            item.productConversion_Name = dataBinbalance.productConversion_Name;
                                            item.weight = dataBinbalance.binBalance_WeightBal;
                                            item.volume = dataBinbalance.binBalance_VolumeBal;
                                            item.mfg_Date = dataBinbalance.goodsReceive_MFG_Date.toString();
                                            item.exp_Date = dataBinbalance.goodsReceive_EXP_Date.toString();
                                            item.tagRef_No1 = "";
                                            item.tagRef_No2 = "";
                                            item.tagRef_No3 = "";
                                            item.tagRef_No4 = "";
                                            item.tagRef_No5 = "";
                                            item.tag_Status = 0;
                                            item.udf_1 = data.udf_1;
                                            item.udf_2 = data.udf_2;
                                            item.udf_3 = data.udf_3;
                                            item.udf_4 = data.udf_4;
                                            item.udf_5 = data.udf_5;
                                            item.create_By = data.create_By;
                                            item.create_Date = DateTime.Now.toString();

                                            utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTagItem"), item.sJson());
                                        }
                                        var binCardModel = new
                                        {
                                            binBalance_Index = dataBinbalance.binBalance_Index,
                                            process_Index = filterModel.process_Index,
                                            documentType_Index = "9056FF09-29DF-4BBA-8FC5-6C524387F993",
                                            documentType_Id = "TF01",
                                            documentType_Name = "โอนย้ายสถานะทั่วไป",
                                            goodsIssue_Date = itemHeader.Create_Date,
                                            //tagItem_Index = ,
                                            tag_Index = dataBinbalance.tag_Index,
                                            tag_No = dataBinbalance.tag_No,
                                            tag_Index_To = _tag_Index,
                                            tag_No_To = data.tagNoNew,
                                            product_Index = dataBinbalance.product_Index,
                                            product_Id = dataBinbalance.product_Id,
                                            product_Name = dataBinbalance.product_Name,
                                            //product_SecondName = ,
                                            //product_ThirdName = ,
                                            product_Lot = dataBinbalance.product_Lot,
                                            itemStatus_Index = dataBinbalance.itemStatus_Index,
                                            itemStatus_Id = dataBinbalance.itemStatus_Id,
                                            itemStatus_Name = dataBinbalance.itemStatus_Name,
                                            itemStatus_Index_To = dataBinbalance.itemStatus_Index,
                                            itemStatus_Id_To = dataBinbalance.itemStatus_Id,
                                            itemStatus_Name_To = dataBinbalance.itemStatus_Name,
                                            productConversion_Index = dataBinbalance.productConversion_Index,
                                            productConversion_Id = dataBinbalance.productConversion_Id,
                                            productConversion_Name = dataBinbalance.productConversion_Name,
                                            owner_Index = dataBinbalance.owner_Index,
                                            owner_Id = dataBinbalance.owner_Id,
                                            owner_Name = dataBinbalance.owner_Name,
                                            location_Index = dataBinbalance.location_Index,
                                            location_Id = dataBinbalance.location_Id,
                                            location_Name = dataBinbalance.location_Name,
                                            location_Index_To = dataBinbalanceTransfer == null ? DataLocation.location_Index : dataBinbalanceTransfer.location_Index,
                                            location_Id_To = dataBinbalanceTransfer == null ? DataLocation.location_Id : dataBinbalanceTransfer.location_Id,
                                            location_Name_To = dataBinbalanceTransfer == null ? DataLocation.location_Name : dataBinbalanceTransfer.location_Name,
                                            mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                            exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                            picking_Qty = (Decimal)itemDetails.pick,
                                            picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                            picking_TotalQty = (Decimal)itemDetails.pick * (Decimal)dataBinbalance.binBalance_Ratio,
                                            Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)itemDetails.pick,
                                            Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)itemDetails.pick,
                                            ref_Document_No = GoodsTransferNo,
                                            ref_Document_Index = GoodsTransfer_Index,
                                            ref_DocumentItem_Index = GoodsTransferItem_Index,
                                            userName = data.create_By,
                                            isTransfer = true
                                        };
                                        var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("insertBincard"), binCardModel.sJson());


                                    }

                                    var update_gti = db.IM_GoodsTransferItem.Find(GoodsTransferItem.GoodsTransferItem_Index);
                                    update_gti.Tag_Index_To = new Guid(_tag_Index);
                                    update_gti.Tag_No_To = data.tagNoNew;
                                    update_gti.Document_Status = 0;

                                    itemDetails.isActive = true;
                                    utils.SendDataApi<Boolean>(new AppSettingConfig().GetUrl("updateIsuseBinbalance"), itemDetails.sJson());
                                }


                            }
                        }
                    }






                    //////// Comment 
                    //// Create GoodsTransfer Header
                    //GoodsTransferService goodsTransferService = new GoodsTransferService();
                    //GoodsTransferViewModel modelGT = new GoodsTransferViewModel();
                    //modelGT.goodsTransfer_Index = Guid.Empty;
                    ////modelGT.owner_Index = data.owner_Index;
                    ////modelGT.owner_Id = data.owner_Id;
                    ////modelGT.owner_Name = data.owner_Name;
                    ////modelGT.documentType_Index = data.documentType_Index;
                    ////modelGT.documentType_Id = data.documentType_Id;
                    ////modelGT.documentType_Name = data.documentType_Name;
                    //modelGT.goodsTransfer_Date = data.goodsTransfer_Date;
                    //modelGT.goodsTransfer_Time = data.goodsTransfer_Time;
                    //modelGT.create_By = data.create_By;
                    //modelGT = goodsTransferService.CreateGoodsTransferHeader(modelGT);

                    //// Create GoodsTransfer Item Pick
                    //List<PickbinbalanceViewModel> lstGoodsTransferItem = new List<PickbinbalanceViewModel>();
                    //if (data.lstPickProduct.Count > 0)
                    //{

                    //    foreach (var item in lstGoodsTransferItem)
                    //    {
                    //        PickbinbalanceViewModel modelPickBinbalance = new PickbinbalanceViewModel();
                    //        modelPickBinbalance.binbalance_Index = item.binbalance_Index;
                    //        modelPickBinbalance.goodsTransfer_Index = modelGT.goodsTransfer_Index.ToString();
                    //        modelPickBinbalance.goodsTransferItem_Index = Guid.NewGuid().ToString();
                    //        modelPickBinbalance.pick = item.qty;
                    //        modelPickBinbalance.Process_Index = item.Process_Index;
                    //        modelPickBinbalance.create_By = data.create_By;
                    //        goodsTransferService.pickProduct(modelPickBinbalance);
                    //    }
                    //}
                    //////// End Comment 


                    db.SaveChanges();
                    transaction.Commit();

                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("CreateGoodsTransferHeader", msglog);
                    transaction.Rollback();
                    throw exy;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        //public Boolean Confirm(TransferViewModel data)
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

        //            //Find new Location
        //            //var newData = context.wm_BinBalance.FromSql("sp_GetBinBalance").Where(c => c.Location_Name == data.LocationNew).FirstOrDefault();

        //            // Select Desc location TO
        //            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //            ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
        //            ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
        //            ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //            ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //            TableName = new SqlParameter("@TableName", "ms_Location");
        //            Where = new SqlParameter("@Where", " Where Location_Name  ='" + data.LocationNew.ToString() + "'");
        //            var newLocationTo = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

        //            Guid Location_Index_To;
        //            String Location_Id_To;
        //            String Location_Name_To;

        //            if (newLocationTo != null)
        //            {
        //                Location_Index_To = new Guid(newLocationTo[0].dataincolumn1.ToString());
        //                Location_Id_To = newLocationTo[0].dataincolumn2;
        //                Location_Name_To = newLocationTo[0].dataincolumn3;
        //            }
        //            else
        //            {
        //                Location_Index_To = new Guid("5d30facb-ed0f-480a-a26d-f6b35308ee05");
        //                Location_Id_To = "7";
        //                Location_Name_To = "Location ST4";

        //            }

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
        //            string SqlWhere = " and Tag_No = N'" + data.LpnNo + "'" +
        //                               " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "' " +
        //                               " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50),Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
        //                               " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve = 0 ";

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
        //                    GoodsTransferItem.ItemStatusIndexTo = item.ItemStatus_Index;
        //                    GoodsTransferItem.ProductConversionIndex = item.ProductConversion_Index;
        //                    GoodsTransferItem.OwnerIndex = item.Owner_Index;//item.Owner_Index;
        //                    GoodsTransferItem.OwnerIndexTo = item.Owner_Index;
        //                    GoodsTransferItem.LocationIndex = item.Location_Index;

        //                    GoodsTransferItem.LocationIndexTo = Location_Index_To;

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
        //                        if (item.BinBalance_WeightBegin == 0)
        //                        {
        //                            BinCardReserve.BinCardReserve_WeightBal = 0;
        //                        }
        //                        else
        //                        {
        //                            BinCardReserve.BinCardReserve_WeightBal = TransferQty * (item.BinBalance_QtyBegin / item.BinBalance_WeightBegin);
        //                        }


        //                    }
        //                    if (item.BinBalance_VolumeBegin == 0)
        //                    {
        //                        BinCardReserve.BinCardReserve_VolumeBal = 0;
        //                    }
        //                    else
        //                    {
        //                        if (item.BinBalance_VolumeBegin == 0)
        //                        {
        //                            BinCardReserve.BinCardReserve_VolumeBal = 0;
        //                        }
        //                        else
        //                        {
        //                            BinCardReserve.BinCardReserve_VolumeBal = TransferQty * (item.BinBalance_QtyBegin / item.BinBalance_VolumeBegin);
        //                        }


        //                    }

        //                    BinCardReserve.Ref_Document_No = GoodsTransferNo;
        //                    BinCardReserve.Ref_Document_Index = GoodsTransferItem.GoodsTransferIndex;
        //                    BinCardReserve.Ref_DocumentItem_Index = GoodsTransferItem.GoodsTransferItemIndex;

        //                    BinCardReserve.Ref_Wave_Index = IsUse.Value.ToString();
        //                    BinCardReserve.Create_By = data.Update_By;

        //                    BinCardReserve.BinCardReserve_Status = 0;
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
        //                                             "  BinBalance_QtyReserve  =  BinBalance_QtyReserve + @BinBalance_QtyReserve " +
        //                                            "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve +  @BinBalance_WeightReserve " +
        //                                            "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve + @BinBalance_VolumeReserve " +
        //                                             "  where  BinBalance_Index = @BinBalance_Index  ";
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);
        //                    isBinBalanceQtyReserve = true;
        //                    olog.logging("Pallet Relocation", " Update [dbo].[wm_BinBalance]  SET " +
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
        //                olog.logging("Pallet Relocation", " ex Rollback " + ex.Message.ToString());
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
        //            olog.logging("Pallet Relocation", " insert im_GoodsTransfer  im_GoodsTransferItem  wm_BinCardReserve ");

        //            if (rowsAffected.ToString() != "0")
        //            {
        //                // Add TAG ITEM
        //                var TagItem_Index = Guid.NewGuid();
        //                var Tag_Index = Guid.NewGuid();
        //                var Tag_No = data.LocationNew;

        //                // GetDataTransfer  For Confirm

        //                string SqlTFWhere = " and Convert(Nvarchar(50),GoodsTransfer_Index)   = N'" + GoodTransfer_Index.ToString() + "'";
        //                var strtfwhere = new SqlParameter("@strwhere", SqlTFWhere);

        //                var TransferResult = context.IM_GoodsTransfer.FromSql("sp_GetGoodsTransfer @strwhere", strtfwhere).ToList();
        //                var TransferItemResult = context.IM_GoodsTransferItem.FromSql("sp_GetGoodsTransferItem @strwhere", strtfwhere).ToList();

        //                string SqlBinCardReserveWhere = " and Convert(Nvarchar(50),Process_Index) = N'" + Process_Index.ToString() + "'" +
        //                                                " and Convert(Nvarchar(50),Ref_Document_Index) = N'" + GoodTransfer_Index.ToString() + "'";
        //                var strBinCardReservewhere = new SqlParameter("@strwhere", SqlBinCardReserveWhere);
        //                var BinCardReserveResult = context.WM_BinCardReserve.FromSql("sp_GetBinCardReserve @strwhere", strBinCardReservewhere).ToList();

        //                if (TransferResult.Count != 1 || TransferItemResult.Count < 1 || BinCardReserveResult.Count < 1)
        //                {
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

        //                        // Select Desc ItemStatus FROM
        //                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
        //                        ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
        //                        ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
        //                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                        TableName = new SqlParameter("@TableName", "ms_ItemStatus");
        //                        Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),ItemStatus_Index)  ='" + TransferItemResultSelect.ItemStatus_Index.ToString() + "'");
        //                        var DatItemStatusFrom = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

        //                        // Select Desc location TO
        //                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //                        ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
        //                        ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
        //                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                        TableName = new SqlParameter("@TableName", "ms_Location");
        //                        Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),Location_Index)  ='" + TransferItemResultSelect.Location_Index_To.ToString() + "'");
        //                        var DataLocationTo = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();


        //                        // Get Owner Location
        //                        var conX = new TransferDbContext();

        //                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //                        ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
        //                        ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
        //                        ColumnName4 = new SqlParameter("@ColumnName4", "Convert(Nvarchar(50),LocationType_Index)");
        //                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                        TableName = new SqlParameter("@TableName", "ms_Location");
        //                        Where = new SqlParameter("@Where", "Where Location_Name ='" + data.LocationNew + "'");
        //                        var DataLocation = conX.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();



        //                        if (BinBalanceResult.BinBalance_QtyBal == BinCardReserveItem.BinCardReserve_QtyBal && BinBalanceResult.BinBalance_QtyBal == TransferItemResultSelect.TotalQty && BinBalanceResult.BinBalance_QtyBal == BinBalanceResult.BinBalance_QtyReserve)
        //                        {
        //                            // Update Binbalance  :  QtyBal and  Itemstatus
        //                            var conDB = new TransferDbContext();
        //                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),LocationType_Index)");
        //                            ColumnName2 = new SqlParameter("@ColumnName2", "LocationType_Id");
        //                            ColumnName3 = new SqlParameter("@ColumnName3", "LocationType_Name");
        //                            ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                            ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                            TableName = new SqlParameter("@TableName", "ms_LocationType");
        //                            Where = new SqlParameter("@Where", "Where LocationType_Index = N'" + DataLocation[0].dataincolumn4.ToString() + "'");
        //                            var DataLocationType = conDB.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

        //                            if (DataLocationType[0].dataincolumn3 == "Active")
        //                            {

        //                                string SqlTagWhere = " and Convert(Nvarchar(50), Tag_No) = N'" + data.LocationNew + "' ";
        //                                var strTagwhere = new SqlParameter("@strwhere", SqlTagWhere);
        //                                var Tag = context.wm_Tag.FromSql("sp_GetTag @strwhere ", strTagwhere).FirstOrDefault();

        //                                if (Tag == null)
        //                                {


        //                                    Tag_Index = Guid.NewGuid();
        //                                    Tag_No = data.LocationNew;

        //                                    var Tag_Index1 = new SqlParameter("Tag_Index", Tag_Index);
        //                                    var Tag_No1 = new SqlParameter("Tag_No", data.LocationNew);
        //                                    var Pallet_No = new SqlParameter("Pallet_No", "");
        //                                    var Pallet_Index = new SqlParameter("Pallet_Index", Guid.NewGuid());
        //                                    //    Pallet_Index.SqlValue = Tag_Index;
        //                                    int tagStatus = 0;
        //                                    var TagRef_No1 = new SqlParameter("TagRef_No1", "");
        //                                    var TagRef_No2 = new SqlParameter("TagRef_No2", "");
        //                                    var TagRef_No3 = new SqlParameter("TagRef_No3", "");
        //                                    var TagRef_No4 = new SqlParameter("TagRef_No4", "");
        //                                    var TagRef_No5 = new SqlParameter("TagRef_No5", "");
        //                                    var Tag_Status = new SqlParameter("Tag_Status", tagStatus);
        //                                    var UDF_1 = new SqlParameter("UDF_1", "");
        //                                    var UDF_2 = new SqlParameter("UDF_2", "");
        //                                    var UDF_3 = new SqlParameter("UDF_3", "");
        //                                    var UDF_4 = new SqlParameter("UDF_4", "");
        //                                    var UDF_5 = new SqlParameter("UDF_5", "");
        //                                    var Create_By = new SqlParameter("Create_By", "");
        //                                    var Create_Date = new SqlParameter("Create_Date", DateTime.Now.Date);
        //                                    var Update_By = new SqlParameter("Update_By", "");
        //                                    var Update_Date = new SqlParameter("Update_Date", DateTime.Now.Date);
        //                                    var Cancel_By = new SqlParameter("Cancel_By", "");
        //                                    var Cancel_Date = new SqlParameter("Cancel_Date", DateTime.Now.Date);
        //                                    var rowsAffectedTag = context.Database.ExecuteSqlCommand("sp_Save_wm_Tag  @Tag_Index,@Tag_No,@Pallet_No,@Pallet_Index,@TagRef_No1,@TagRef_No2,@TagRef_No3,@TagRef_No4,@TagRef_No5,@Tag_Status,@UDF_1,@UDF_2,@UDF_3,@UDF_4,@UDF_5,@Create_By,@Create_Date,@Update_By,@Update_Date,@Cancel_By,@Cancel_Date ", Tag_Index1, Tag_No1, Pallet_No, Pallet_Index, TagRef_No1, TagRef_No2, TagRef_No3, TagRef_No4, TagRef_No5, Tag_Status, UDF_1, UDF_2, UDF_3, UDF_4, UDF_5, Create_By, Create_Date, Update_By, Update_Date, Cancel_By, Cancel_Date);
        //                                }
        //                                else
        //                                {
        //                                    Tag_Index = Tag.Tag_Index;
        //                                    Tag_No = Tag.Tag_No;
        //                                }

        //                            }
        //                            else
        //                            {
        //                                Tag_Index = (Guid)BinCardReserveItem.Tag_Index;
        //                                Tag_No = BinCardReserveItem.Tag_No;
        //                            }

        //                            var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", BinCardReserveItem.BinCardReserve_QtyBal);
        //                            var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserveItem.BinCardReserve_WeightBal);
        //                            var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserveItem.BinCardReserve_VolumeBal);
        //                            var BinBalance_Index = new SqlParameter("@BinBalance_Index", BinCardReserveItem.BinBalance_Index);
        //                            //var ItemStatus_Index = new SqlParameter("@ItemStatus_Index", TransferItemResultSelect.ItemStatus_Index_To);
        //                            //var ItemStatus_Id = new SqlParameter("@ItemStatus_Id", DatItemStatusTo[0].dataincolumn2);
        //                            //var ItemStatus_Name = new SqlParameter("@ItemStatus_Name", DatItemStatusTo[0].dataincolumn3);
        //                            var Location_Index = new SqlParameter("@Location_Index", TransferItemResultSelect.Location_Index_To);
        //                            var Location_Id = new SqlParameter("@Location_Id", DataLocationTo[0].dataincolumn2);
        //                            var Location_Name = new SqlParameter("@Location_Name", DataLocationTo[0].dataincolumn3);
        //                            var pTag_Index = new SqlParameter("@Tag_Index", Tag_Index);
        //                            var pTag_No = new SqlParameter("@Tag_No", Tag_No);
        //                            var pUpdate_by = new SqlParameter("@Update_by", data.Update_By);
        //                            String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                        "  BinBalance_QtyReserve  = BinBalance_QtyReserve - @BinBalance_QtyReserve " +
        //                                                        "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  @BinBalance_WeightReserve " +
        //                                                        "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - @BinBalance_VolumeReserve " +
        //                                                        " ,Location_Index  =  @Location_Index " +
        //                                                        " ,Location_Id    =   @Location_Id " +
        //                                                        " ,Location_Name  =   @Location_Name  " +
        //                                                        " ,Update_by  =   @Update_by  " +
        //                                                        " ,Update_date  =   GETDATE()  " +
        //                                                        " ,Tag_Index  =   @Tag_Index  " +
        //                                                        " ,Tag_No  =   @Tag_No  " +
        //                                                        "  where  BinBalance_Index = @BinBalance_Index  ";
        //                            context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, Location_Index, Location_Id, Location_Name, pTag_Index, pTag_No, BinBalance_Index, pUpdate_by);

        //                            olog.logging("Pallet Relocation", " Update [dbo].[wm_BinBalance]  SET " +
        //                                                        "  BinBalance_QtyReserve  = BinBalance_QtyReserve - " + BinCardReserveItem.BinCardReserve_QtyBal +
        //                                                        "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  " + BinCardReserveItem.BinCardReserve_WeightBal +
        //                                                        "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - " + BinCardReserveItem.BinCardReserve_VolumeBal +
        //                                                        " ,Location_Index  =  " + TransferItemResultSelect.Location_Index_To +
        //                                                        " ,Location_Id    =   " + DataLocationTo[0].dataincolumn2 +
        //                                                        " ,Location_Name  =   " + DataLocationTo[0].dataincolumn3 +
        //                                                        " ,Update_by  =   " + data.Update_By +
        //                                                        " ,Update_date  =   GETDATE() " +
        //                                                        " ,Tag_Index  =   " + Tag_Index +
        //                                                        " ,Tag_No  =   " + Tag_No +
        //                                                        "  where  BinBalance_Index = " + BinCardReserveItem.BinBalance_Index);

        //                            var RefDocumentItemIndex = new SqlParameter("@Ref_DocumentItem_Index", BinCardReserveItem.Ref_DocumentItem_Index);

        //                            String SqlcmdBinCardReserve = " Update [dbo].[wm_BinCardReserve]  SET " +
        //                                 " Tag_Index  =   @Tag_Index  " +
        //                                 " ,Tag_No  =   @Tag_No  " +
        //                                 " ,BinCardReserve_Status = 2" +
        //                                 "  where  BinBalance_Index = @BinBalance_Index and  Ref_DocumentItem_Index = @Ref_DocumentItem_Index";
        //                            context.Database.ExecuteSqlCommand(SqlcmdBinCardReserve, BinBalance_Index, pTag_Index, pTag_No, RefDocumentItemIndex);


        //                            olog.logging("Pallet Relocation", " Update [dbo].[wm_BinCardReserve]  SET " +
        //                                                        " Tag_Index  =   " + Tag_Index +
        //                                                        " ,Tag_No  =   " + Tag_No +
        //                                                        " ,BinCardReserve_Status = 2" +
        //                                                        "  where  BinBalance_Index = " + BinCardReserveItem.BinBalance_Index + "and  Ref_DocumentItem_Index = " + BinCardReserveItem.Ref_DocumentItem_Index);

        //                            var tagindexupdate = new SqlParameter("@Tag_indexUpdate", BinCardReserveItem.Tag_Index);

        //                            String SqlcmdGoodsTransfer = " Update [dbo].[im_GoodsTransferItem]  SET " +
        //                                                        " Tag_Index  =   @Tag_Index  " +
        //                                                        "  where  Tag_Index = @Tag_indexUpdate and GoodsTransferItem_Index = @Ref_DocumentItem_Index ";
        //                            context.Database.ExecuteSqlCommand(SqlcmdGoodsTransfer, tagindexupdate, pTag_Index, RefDocumentItemIndex);

        //                            olog.logging("Pallet Relocation", " Update [dbo].[im_GoodsTransferItem]  SET " +
        //                                                      " Tag_Index  =   " + Tag_Index +
        //                                                      "  where  Tag_Index = " + BinCardReserveItem.Tag_Index + "and GoodsTransferItem_Index = " + BinCardReserveItem.Ref_DocumentItem_Index);



        //                        }
        //                        else
        //                        {

        //                            // error
        //                            return false;

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
        //                        BinCard.Tag_Index_To = Tag_Index; // BinCardReserveItem.TagItem_Index; //BinCardReserveItem.Tag_Index_To;
        //                        BinCard.Tag_No_To = Tag_No;  //BinBalanceResult.Tag_No;  //  ไม่ได้เปลี่ยนTAG
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

        //                        BinCard.Location_Index = new Guid(newLocationTo[0].dataincolumn1.ToString());//BinCardReserveItem.Location_Index;
        //                        BinCard.Location_Id = newLocationTo[0].dataincolumn2; //BinCardReserveItem.Location_Id;
        //                        BinCard.Location_Name = newLocationTo[0].dataincolumn3;//BinCardReserveItem.Location_Name;

        //                        BinCard.Location_Index_To = new Guid(newLocationTo[0].dataincolumn1.ToString());
        //                        BinCard.Location_Id_To = newLocationTo[0].dataincolumn2;
        //                        BinCard.Location_Name_To = newLocationTo[0].dataincolumn3;


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
        //                        BinCard.TagItem_Index = TagItem_Index;

        //                        BinCard.Tag_Index = Tag_Index;
        //                        BinCard.Tag_No = Tag_No; //data.LpnNo;
        //                        BinCard.Tag_Index_To = Tag_Index; //BinCardReserveItem.Tag_Index_To;
        //                        BinCard.Tag_No_To = Tag_No; //data.LpnNo;  //  ไม่ได้เปลี่ยนTAG

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


        //                        BinCard.Location_Index = new Guid(DataLocationTo[0].dataincolumn1);
        //                        BinCard.Location_Id = DataLocationTo[0].dataincolumn2;
        //                        BinCard.Location_Name = DataLocationTo[0].dataincolumn3;

        //                        BinCard.Location_Index_To = new Guid(DataLocationTo[0].dataincolumn1);
        //                        BinCard.Location_Id_To = DataLocationTo[0].dataincolumn2;
        //                        BinCard.Location_Name_To = DataLocationTo[0].dataincolumn3;



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


        //                    //// Add Bincacd binbalance TAG  To Stroe 
        //                    var rowsAffected1 = context.Database.ExecuteSqlCommand("sp_Save_BinBalanceTransfer @BinBalance,@BinCard,@Tag,@TagItem", pBinBalance, pBinCard, pTag, pTagItem);
        //                    olog.logging("Pallet Relocation", "insert BinCard");

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
        //                    olog.logging("Pallet Relocation", " ex Rollback " + ex.Message.ToString());
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


        //                ////// Add Bincacd binbalance TAG  To Stroe 
        //                //var rowsAffected1 = context.Database.ExecuteSqlCommand("sp_Save_BinBalanceTransfer @BinBalance,@BinCard,@Tag,@TagItem", pBinBalance, pBinCard, pTag, pTagItem);
        //                //olog.logging("Pallet Relocation", "insert BinCard");
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
