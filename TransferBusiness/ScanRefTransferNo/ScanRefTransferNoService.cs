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
using TransferBusiness.GoodIssue;

namespace TransferBusiness.Transfer
{
    public class ScanRefTransferNoService
    {
        #region TransferDbContext
        private TransferDbContext db;

        public ScanRefTransferNoService()
        {
            db = new TransferDbContext();
        }

        public ScanRefTransferNoService(TransferDbContext db)
        {
            this.db = db;
        }
        #endregion

        #region FilterScanTransfer
        public List<listTaskViewModel> FilterScanTransfer(ScanRefTransferNoViewModel model)
        {
            try
            {
                var items = new List<listTaskViewModel>();

                if (!string.IsNullOrEmpty(model.goodsTransfer_No) && (!string.IsNullOrEmpty(model.username)))
                {
                    items = (from gt in db.IM_GoodsTransfer
                             join tgti in db.im_TaskTransferItem on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                             join tgt in db.im_TaskTransfer on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                             where gt.GoodsTransfer_No == model.goodsTransfer_No && gt.Document_Status == 2 && tgt.UserAssign == model.username && gt.DocumentType_Index != new Guid("9540E2AE-10A1-44CC-9FDF-53FF9AAB8D07")
                             group gt by new { gt.GoodsTransfer_Index, gt.GoodsTransfer_No, tgt.TaskTransfer_No, tgt.TaskTransfer_Index, gt.GoodsTransfer_Date } into g
                             select new listTaskViewModel
                             {
                                 goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                                 goodsTransfer_No = g.Key.GoodsTransfer_No,
                                 goodsTransfer_Date = g.Key.GoodsTransfer_Date.ToString(),
                                 taskTransfer_No = g.Key.TaskTransfer_No,
                                 taskTransfer_Index = g.Key.TaskTransfer_Index
                             }).ToList();
                } else {
                    items = (from gt in db.IM_GoodsTransfer
                             join tgti in db.im_TaskTransferItem on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                             join tgt in db.im_TaskTransfer on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                             where gt.Document_Status == 2 && tgt.UserAssign == model.username && gt.DocumentType_Index != new Guid("9540E2AE-10A1-44CC-9FDF-53FF9AAB8D07")
                             group gt by new { gt.GoodsTransfer_Index, gt.GoodsTransfer_No, tgt.TaskTransfer_No, tgt.TaskTransfer_Index, gt.GoodsTransfer_Date } into g
                             select new listTaskViewModel
                             {
                                 goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                                 goodsTransfer_No = g.Key.GoodsTransfer_No,
                                 goodsTransfer_Date = g.Key.GoodsTransfer_Date.ToString(),
                                 taskTransfer_No = g.Key.TaskTransfer_No,
                                 taskTransfer_Index = g.Key.TaskTransfer_Index
                             }).ToList();
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

        #region FilterGoodsTransfer
        public List<listTaskViewModel> FilterGoodsTransfer(ScanRefTransferNoViewModel model)
        {
            try
            {
                var items = new List<listTaskViewModel>();
                items = (from gt in db.IM_GoodsTransfer
                         join tgti in db.im_TaskTransferItem on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                         join tgt in db.im_TaskTransfer on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                         where gt.Document_Status == 2 && tgti.Document_Status == 0 && gt.DocumentType_Index != new Guid("9540E2AE-10A1-44CC-9FDF-53FF9AAB8D07")
                         group gt by new { gt.GoodsTransfer_Index, gt.GoodsTransfer_No, gt.GoodsTransfer_Date, tgt.TaskTransfer_No, tgt.TaskTransfer_Index
                         , gt.DocumentType_Name, tgti.Location_Name, tgti.Location_Name_To, tgti.UDF_5 } into g
                         select new listTaskViewModel
                         {
                             goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                             goodsTransfer_No = g.Key.GoodsTransfer_No,
                             goodsTransfer_Date = g.Key.GoodsTransfer_Date.toString(),
                             taskTransfer_No = g.Key.TaskTransfer_No,
                             taskTransfer_Index = g.Key.TaskTransfer_Index,
                             documentType_Name = g.Key.DocumentType_Name,
                             location_Name = g.Key.Location_Name,
                             location_Name_To = g.Key.Location_Name_To,
                             udf_5 = g.Key.UDF_5
                         }).ToList();
                if (!string.IsNullOrEmpty(model.key))
                {
                    items = items.Where(c => c.goodsTransfer_No == model.key).ToList();
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

        #region FilterGoodsTransferReplenish
        public List<listTaskViewModel> FilterGoodsTransferReplenish(ScanRefTransferNoViewModel model)
        {
            try
            {
                var items = new List<listTaskViewModel>();
                items = (from gt in db.IM_GoodsTransfer.Where(c => c.DocumentType_Index == Guid.Parse("47BF1845-33D1-47FE-B38D-7BDDF0E48A7E") ||
                            c.DocumentType_Index == Guid.Parse("9056FF09-29DF-4BBA-8FC5-6C524387F995") ||
                            c.DocumentType_Index == Guid.Parse("773DF9A5-83FC-4964-BECC-CAB7A0F482C7") ||
                            c.DocumentType_Index == Guid.Parse("D61AB6E6-FFB7-47B9-A2D3-CD4AF77E98C5")
                         )
                         join tgti in db.im_TaskTransferItem on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                         join tgt in db.im_TaskTransfer on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                         where gt.Document_Status == 2 && tgti.Document_Status == 0 && gt.DocumentType_Index != new Guid("9540E2AE-10A1-44CC-9FDF-53FF9AAB8D07")
                         group gt by new
                         {
                             gt.GoodsTransfer_Index,
                             gt.GoodsTransfer_No,
                             gt.GoodsTransfer_Date,
                             tgt.TaskTransfer_No,
                             tgt.TaskTransfer_Index,
                             gt.DocumentType_Name,
                             tgti.Location_Name,
                             tgti.Location_Name_To,
                             tgti.UDF_5,
                             tgti.Product_Id,
                             tgti.Product_Name,
                             tgti.ProductConversion_Name,
                             tgti.Qty,
                             tgti.Tag_No
                         } into g
                         select new listTaskViewModel
                         {
                             goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                             goodsTransfer_No = g.Key.GoodsTransfer_No,
                             goodsTransfer_Date = g.Key.GoodsTransfer_Date.toString(),
                             taskTransfer_No = g.Key.TaskTransfer_No,
                             taskTransfer_Index = g.Key.TaskTransfer_Index,
                             documentType_Name = g.Key.DocumentType_Name,
                             location_Name = g.Key.Location_Name,
                             location_Name_To = g.Key.Location_Name_To,
                             udf_5 = g.Key.UDF_5,
                             product_Id = g.Key.Product_Id,
                             product_Name = g.Key.Product_Name,
                             productConversion_Name = g.Key.ProductConversion_Name,
                             qty = g.Key.Qty,
                             tag_No = g.Key.Tag_No
                         }).ToList();
                if (!string.IsNullOrEmpty(model.key))
                {
                    items = items.Where(c => c.goodsTransfer_No == model.key || c.tag_No.Contains(model.key)).ToList();
                }
                return items.OrderBy(o => o.location_Name_To).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

        #region FilterGoodsTransferUnPack
        public List<listTaskViewModel> FilterGoodsTransferUnPack(ScanRefTransferNoViewModel model)
        {
            try
            {
                var items = new List<listTaskViewModel>();
                items = (from gt in db.IM_GoodsTransfer.Where(c => c.DocumentType_Index == Guid.Parse("DBF6FCD1-D881-4E63-8307-9A118B93D899"))
                         join tgti in db.im_TaskTransferItem.Where(c => c.Is_unpack == null) on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                         join tgt in db.im_TaskTransfer on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                         where gt.Document_Status == 3 && gt.DocumentType_Index != new Guid("9540E2AE-10A1-44CC-9FDF-53FF9AAB8D07")
                         group gt by new {
                             gt.GoodsTransfer_Index,
                             gt.GoodsTransfer_No,
                             gt.GoodsTransfer_Date,
                             tgti.Binbalance_index,
                             tgti.TaskTransferItem_Index,
                             tgti.ProductConversion_Index,
                             tgti.ProductConversion_Id,
                             tgti.ProductConversion_Name,
                             tgti.Qty } into g
                         select new listTaskViewModel
                         {
                             goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                             goodsTransfer_No = g.Key.GoodsTransfer_No,
                             goodsTransfer_Date = g.Key.GoodsTransfer_Date.toString(),
                             binbalance_index = g.Key.Binbalance_index,
                             taskTransfer_Index = g.Key.TaskTransferItem_Index,
                             productConversion_Index = g.Key.ProductConversion_Index.ToString(),
                             productConversion_Id = g.Key.ProductConversion_Id,
                             productConversion_Name = g.Key.ProductConversion_Name,
                             qty = g.Key.Qty

                         }).ToList();
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

        #region ScanTransfer
        public GoodsTransferViewModel ScanTransfer(string transferNo, string user)
        {
            try
            {
                //var result = new GoodsTransferViewModel();

                //var queryResult = db.IM_GoodsTransfer.FirstOrDefault(f => f.GoodsTransfer_No == transferNo && f.Document_Status == 0);

                var queryResult = (from gt in db.IM_GoodsTransfer
                                   join tgti in db.im_TaskTransferItem on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                                   join tgt in db.im_TaskTransfer on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                                   where gt.GoodsTransfer_No == transferNo && gt.Document_Status == 2 && gt.DocumentType_Index != new Guid("9540E2AE-10A1-44CC-9FDF-53FF9AAB8D07")
                                   group gt by new { gt.GoodsTransfer_Index, gt.GoodsTransfer_No } into g
                                   select new GoodsTransferViewModel
                                   {
                                       goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                                       goodsTransfer_No = g.Key.GoodsTransfer_No
                                   }).FirstOrDefault();

                //if (queryResult != null)
                //{
                //    result.goodsTransfer_Index = queryResult.GoodsTransfer_Index;
                //    result.goodsTransfer_No = queryResult.GoodsTransfer_No;
                //    result.document_Status = queryResult.Document_Status;
                //}

                return queryResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

        #region GetTransferItem
        public List<GoodsTransferItemViewModel> GetTransferItem(Guid transferNo, Guid tasktransfer_Index)
        {
            try
            {
                var result = new List<GoodsTransferItemViewModel>();

                var queryResult = db.IM_GoodsTransferItem.Where(f => f.GoodsTransfer_Index == transferNo && f.Document_Status == 0).ToList();
                var querytask = db.im_TaskTransferItem.FirstOrDefault(f => f.TaskTransfer_Index == tasktransfer_Index && f.Document_Status == 0);

                if (queryResult.Count() > 0)
                {
                    foreach (var q in queryResult.Where(c => c.GoodsTransferItem_Index == querytask.Ref_DocumentItem_Index))
                    {
                        var r = new GoodsTransferItemViewModel();
                        r.goodsTransferItem_Index = q.GoodsTransferItem_Index;
                        r.goodsTransfer_Index = q.GoodsTransfer_Index;
                        r.goodsReceive_Index = q.GoodsReceive_Index;
                        r.goodsReceiveItem_Index = q.GoodsReceiveItem_Index;
                        r.goodsReceiveItemLocation_Index = q.GoodsReceiveItemLocation_Index;
                        r.lineNum = q.LineNum;
                        r.tagItem_Index = q.TagItem_Index;
                        r.tag_Index = q.Tag_Index;
                        r.tag_No = q.Tag_No;
                        r.tag_Index_To = q.Tag_Index_To;
                        r.product_Index = q.Product_Index;
                        r.product_Index_To = q.Product_Index_To;
                        r.product_Lot = q.Product_Lot;
                        r.product_Lot_To = q.Product_Lot_To;
                        r.itemStatus_Index = q.ItemStatus_Index;
                        r.itemStatus_Index_To = q.ItemStatus_Index_To;
                        r.productConversion_Index = q.ProductConversion_Index;
                        r.productConversion_Id = q.ProductConversion_Id;
                        r.productConversion_Name = q.ProductConversion_Name;
                        r.owner_Index = q.Owner_Index;
                        r.owner_Index_To = q.Owner_Index_To;
                        r.location_Index = querytask.Location_Index;
                        r.location_Index_To = querytask.Location_Index_To;
                        r.goodsReceive_EXP_Date = q.GoodsReceive_EXP_Date.toString();
                        r.goodsReceive_EXP_Date_To = q.GoodsReceive_EXP_Date_To.toString();
                        r.qty = q.Qty;
                        r.ratio = q.Ratio;
                        r.totalQty = q.TotalQty;
                        r.weight = q.Weight;
                        r.volume = q.Volume;
                        r.documentRef_No1 = q.DocumentRef_No1;
                        r.documentRef_No2 = q.DocumentRef_No2;
                        r.documentRef_No3 = q.DocumentRef_No3;
                        r.documentRef_No4 = q.DocumentRef_No4;
                        r.documentRef_No5 = q.DocumentRef_No5;
                        r.document_Status = q.Document_Status;
                        r.udf_1 = q.UDF_1;
                        r.udf_2 = q.UDF_2;
                        r.udf_3 = q.UDF_3;
                        r.udf_4 = q.UDF_4;
                        r.udf_5 = q.UDF_5;
                        r.ref_Process_Index = q.Ref_Process_Index;
                        r.ref_Document_No = q.Ref_Document_No;
                        r.ref_Document_Index = q.Ref_Document_Index;
                        r.ref_DocumentItem_Index = q.Ref_DocumentItem_Index;
                        r.tag_No_To = q.Tag_No_To;
                        r.product_Id = q.Product_Id;
                        r.product_Name = q.Product_Name;
                        r.product_SecondName = q.Product_SecondName;
                        r.product_ThirdName = q.Product_ThirdName;
                        r.product_Id_To = q.Product_Id_To;
                        r.product_Name_To = q.Product_Name_To;
                        r.product_SecondName_To = q.Product_SecondName_To;
                        r.product_ThirdName_To = q.Product_ThirdName_To;
                        r.itemStatus_Id = q.ItemStatus_Id;
                        r.itemStatus_Name = q.ItemStatus_Name;
                        r.itemStatus_Id_To = q.ItemStatus_Id_To;
                        r.itemStatus_Name_To = q.ItemStatus_Name_To;
                        r.owner_Id = q.Owner_Id;
                        r.owner_Name = q.Owner_Name;
                        r.owner_Id_To = q.Owner_Id_To;
                        r.owner_Name_To = q.Owner_Name_To;
                        r.location_Id = querytask.Location_Id;
                        r.location_Name = querytask.Location_Name;
                        r.location_Id_To = querytask.Location_Id_To;
                        r.location_Name_To = querytask.Location_Name_To;
                        result.Add(r);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

        #region Confirm
        public string Confirm(ScanRefTransferNoViewModel model)
        {
            String State = "Start " + model.sJson();
            String msglog = "";
            var olog = new logtxt();
            var _tag_Index = "";
            try
            {
                var result = "";
                var process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");
                State = "SELECT IM_GoodsTransferItem";
                var goodsTransfer = db.IM_GoodsTransfer.FirstOrDefault(f => f.GoodsTransfer_Index == model.goodsTransfer_Index);
                var GTI = db.IM_GoodsTransferItem.FirstOrDefault(f => f.GoodsTransferItem_Index == model.goodsTransferItem_Index && f.GoodsTransfer_Index == model.goodsTransfer_Index && f.Document_Status == 0);
                var TGTI = db.im_TaskTransferItem.FirstOrDefault(f => f.Ref_DocumentItem_Index == model.goodsTransferItem_Index && f.Ref_Document_Index == model.goodsTransfer_Index && f.Document_Status == 0);
                if (GTI == null && TGTI == null)
                {
                    return "ไม่พบเลขที่ใบโอนย้ายที่ค้นหา";
                }
                else
                {
                    GTI.Document_Status = 1;
                    GTI.Update_Date = DateTime.Now;
                    GTI.Update_By = model.username;

                    TGTI.Document_Status = 1;
                    TGTI.Update_Date = DateTime.Now;
                    TGTI.Update_By = model.username;


                    #region Get default Location
                    var LocationViewModel = new { location_Name = "SA1011" };
                    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), LocationViewModel.sJson());
                    var DataLocation = GetLocation.FirstOrDefault();
                    #endregion

                    #region update binbalance
                    var modelTag = new { ref_Document_Index = GTI.GoodsTransfer_Index, ref_DocumentItem_Index = GTI.GoodsTransferItem_Index };
                    var dataBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalanceFromReserve"), modelTag.sJson()).Where(c => c.binBalance_QtyReserve > 0).FirstOrDefault();

                    var dataBinbalanceTo = new BinBalanceViewModel();
                    dataBinbalanceTo = null;
                    if (!string.IsNullOrEmpty(GTI.Tag_No_To))
                    {
                        var modelTagNew = new { Tag_No = GTI.Tag_No_To };
                        dataBinbalanceTo = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), modelTagNew.sJson()).FirstOrDefault();
                    }

                    if ((GTI.Location_Id != GTI.Location_Id_To) || (GTI.ItemStatus_Id != GTI.ItemStatus_Id_To) || (GTI.Tag_No != GTI.Tag_No_To))
                    {

                        // Has Old Tag
                        if (dataBinbalanceTo != null)
                        {
                            var resultTag = new LPNViewModel();
                            var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_Index = GTI.Tag_Index_To } };
                            var tag = new DocumentViewModel();
                            tag.listDocumentViewModel = listTag;
                            resultTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), tag.sJson()).FirstOrDefault();
                            _tag_Index = resultTag.tag_Index.ToString();
                        }

                        if (_tag_Index == "")
                        {
                            //if (!string.IsNullOrEmpty(GTI.Tag_No_To))
                            if (dataBinbalance.binBalance_QtyBal != (GTI.Qty * GTI.Ratio))
                            {
                                GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                                grModel.goodsReceive_Index = GTI.GoodsReceive_Index;
                                grModel.owner_Index = Guid.Parse("8B8B6203-A634-4769-A247-C0346350A963");
                                grModel.tag_No = GTI.Tag_No_To;
                                grModel.create_By = model.username;
                                if (dataBinbalance.binBalance_QtyBal != (GTI.Qty * GTI.Ratio))
                                {
                                    _tag_Index = utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTag"), grModel.sJson());
                                }
                            }
                        }

                        // If tag_Index
                        if (!string.IsNullOrEmpty(_tag_Index))
                        {
                            #region Check Binbalance binBalance_QtyReserve
                            var binBalanceReserve = new
                            {
                                binbalance_Index = dataBinbalance.binBalance_Index
                            };
                            var dataBinbalanceReserve = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), binBalanceReserve.sJson());
                            #endregion

                            var TagItemIndex = new Guid();

                            var resultNewTag = new LPNViewModel();
                            var listNewTag = new List<DocumentViewModel> { new DocumentViewModel { document_Index = new Guid(_tag_Index) } };
                            var newTag = new DocumentViewModel();
                            newTag.listDocumentViewModel = listNewTag;
                            resultNewTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), newTag.sJson()).FirstOrDefault();

                            var dataSplit = false;



                            if (dataBinbalance.binBalance_QtyBal != (GTI.Qty * GTI.Ratio) && dataBinbalanceReserve.binBalance_QtyBal != dataBinbalanceReserve.binBalance_QtyReserve)
                            {
                                GoodsReceiveTagItemViewModel item = new GoodsReceiveTagItemViewModel();

                                item.tag_Index = new Guid(_tag_Index.ToString());
                                item.tagItem_Index = Guid.NewGuid();
                                item.tag_No = resultNewTag.tag_No;
                                item.goodsReceive_Index = dataBinbalance.goodsReceive_Index;
                                item.goodsReceiveItem_Index = new Guid(dataBinbalance.goodsReceiveItem_Index.ToString());
                                item.process_Index = process_Index;
                                item.product_Index = dataBinbalance.product_Index;
                                item.product_Id = dataBinbalance.product_Id;
                                item.product_Name = dataBinbalance.product_Name;
                                item.product_SecondName = dataBinbalance.product_SecondName;
                                item.product_ThirdName = dataBinbalance.product_ThirdName;
                                item.product_Lot = dataBinbalance.product_Lot;
                                item.itemStatus_Index = dataBinbalance.itemStatus_Index;
                                item.itemStatus_Id = dataBinbalance.itemStatus_Id;
                                item.itemStatus_Name = dataBinbalance.itemStatus_Name;
                                item.qty = GTI.Qty;
                                item.productConversion_Ratio = Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                item.totalQty = Convert.ToDecimal(GTI.Qty) * Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
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
                                item.tag_Status = 1;
                                //item.udf_1 = GTI.udf_1;
                                //item.udf_2 = GTI.udf_2;
                                //item.udf_3 = GTI.udf_3;
                                //item.udf_4 = GTI.udf_4;
                                //item.udf_5 = GTI.udf_5;
                                item.create_By = model.username;
                                item.create_Date = DateTime.Now.toString();
                                item.erp_Location = dataBinbalance.ERP_Location;
                                item.suggest_Location_Index = dataBinbalance.location_Index;
                                item.suggest_Location_Id = dataBinbalance.location_Id;
                                item.suggest_Location_Name = dataBinbalance.location_Name;

                                dataSplit = true;
                                TagItemIndex = item.tagItem_Index;
                                utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTagItem"), item.sJson());
                            }


                            var binCardModel = new
                            {
                                isSplit = dataSplit,
                                binBalance_Index = dataBinbalance.binBalance_Index,
                                process_Index = process_Index,
                                documentType_Index = goodsTransfer.DocumentType_Index,
                                documentType_Id = goodsTransfer.DocumentType_Id,
                                documentType_Name = goodsTransfer.DocumentType_Name,
                                //goodsIssue_Date = data.create_Date,
                                tagItem_Index = TagItemIndex,
                                tag_Index = dataBinbalance.tag_Index,
                                tag_No = dataBinbalance.tag_No,
                                tag_Index_To = _tag_Index,
                                tag_No_To = resultNewTag.tag_No,
                                product_Index = dataBinbalance.product_Index,
                                product_Id = dataBinbalance.product_Id,
                                product_Name = dataBinbalance.product_Name,
                                product_SecondName = dataBinbalance.product_SecondName,
                                product_ThirdName = dataBinbalance.product_ThirdName,
                                product_Lot = dataBinbalance.product_Lot,
                                itemStatus_Index = dataBinbalance.itemStatus_Index,
                                itemStatus_Id = dataBinbalance.itemStatus_Id,
                                itemStatus_Name = dataBinbalance.itemStatus_Name,
                                itemStatus_Index_To = GTI.ItemStatus_Index_To,
                                itemStatus_Id_To = GTI.ItemStatus_Id_To,
                                itemStatus_Name_To = GTI.ItemStatus_Name_To,
                                productConversion_Index = dataBinbalance.productConversion_Index,
                                productConversion_Id = dataBinbalance.productConversion_Id,
                                productConversion_Name = dataBinbalance.productConversion_Name,
                                owner_Index = dataBinbalance.owner_Index,
                                owner_Id = dataBinbalance.owner_Id,
                                owner_Name = dataBinbalance.owner_Name,
                                location_Index = dataBinbalance.location_Index,
                                location_Id = dataBinbalance.location_Id,
                                location_Name = dataBinbalance.location_Name,
                                //location_Index_To = dataBinbalanceTo == null ? DataLocation.location_Index : dataBinbalanceTo.location_Index,
                                //location_Id_To = dataBinbalanceTo == null ? DataLocation.location_Id : dataBinbalanceTo.location_Id,
                                //location_Name_To = dataBinbalanceTo == null ? DataLocation.location_Name : dataBinbalanceTo.location_Name,
                                location_Index_To = GTI.Location_Index_To,
                                location_Id_To = GTI.Location_Id_To,
                                location_Name_To = GTI.Location_Name_To,
                                mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                picking_Qty = (GTI.Qty * GTI.Ratio),
                                picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                picking_TotalQty = (GTI.Qty * GTI.Ratio) * (Decimal)dataBinbalance.binBalance_Ratio,
                                Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                ref_Document_No = model.goodsTransfer_No,
                                //ref_Document_Index = GoodsTransfer.GoodsTransfer_Index,
                                ref_Document_Index = GTI.GoodsTransfer_Index,
                                ref_DocumentItem_Index = GTI.GoodsTransferItem_Index,
                                userName = model.username,
                                erp_Location = GTI.ERP_Location,
                                erp_Location_To = GTI.ERP_Location_To,
                                isTransfer = true
                            };
                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransfer"), binCardModel.sJson());
                            if (CreateTagHeader != null)
                            {
                                TGTI.Binbalance_index = Guid.Parse(CreateTagHeader);
                            }


                        }
                        else
                        {

                            var binCardModel = new
                            {
                                binBalance_Index = dataBinbalance.binBalance_Index,
                                process_Index = process_Index,
                                documentType_Index = goodsTransfer.DocumentType_Index,
                                documentType_Id = goodsTransfer.DocumentType_Id,
                                documentType_Name = goodsTransfer.DocumentType_Name,
                                //goodsIssue_Date = itemHeader.Create_Date,
                                tagItem_Index = dataBinbalance.tagItem_Index,
                                tag_Index = dataBinbalance.tag_Index,
                                tag_No = dataBinbalance.tag_No,
                                tag_Index_To = dataBinbalance.tag_Index,
                                tag_No_To = dataBinbalance.tag_No,
                                product_Index = dataBinbalance.product_Index,
                                product_Id = dataBinbalance.product_Id,
                                product_Name = dataBinbalance.product_Name,
                                product_SecondName = dataBinbalance.product_SecondName,
                                product_ThirdName = dataBinbalance.product_ThirdName,
                                product_Lot = dataBinbalance.product_Lot,
                                itemStatus_Index = dataBinbalance.itemStatus_Index,
                                itemStatus_Id = dataBinbalance.itemStatus_Id,
                                itemStatus_Name = dataBinbalance.itemStatus_Name,
                                itemStatus_Index_To = GTI.ItemStatus_Index_To,
                                itemStatus_Id_To = GTI.ItemStatus_Id_To,
                                itemStatus_Name_To = GTI.ItemStatus_Name_To,
                                productConversion_Index = dataBinbalance.productConversion_Index,
                                productConversion_Id = dataBinbalance.productConversion_Id,
                                productConversion_Name = dataBinbalance.productConversion_Name,
                                owner_Index = dataBinbalance.owner_Index,
                                owner_Id = dataBinbalance.owner_Id,
                                owner_Name = dataBinbalance.owner_Name,
                                location_Index = dataBinbalance.location_Index,
                                location_Id = dataBinbalance.location_Id,
                                location_Name = dataBinbalance.location_Name,
                                location_Index_To = GTI.Location_Index_To,
                                location_Id_To = GTI.Location_Id_To,
                                location_Name_To = GTI.Location_Name_To,
                                mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                picking_Qty = (GTI.Qty * GTI.Ratio),
                                picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                picking_TotalQty = (GTI.Qty * GTI.Ratio) * (Decimal)dataBinbalance.binBalance_Ratio,
                                Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                ref_Document_No = model.goodsTransfer_No,
                                ref_Document_Index = model.goodsTransfer_Index,
                                ref_DocumentItem_Index = model.goodsTransferItem_Index,
                                userName = model.username,
                                erp_Location = GTI.ERP_Location,
                                erp_Location_To = GTI.ERP_Location_To,
                                isTransfer = true
                            };
                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransfer"), binCardModel.sJson());
                            if (CreateTagHeader != null)
                            {
                                TGTI.Binbalance_index = Guid.Parse(CreateTagHeader);
                            }
                        }
                    }
                    #endregion



                    result = "โอนย้ายตำแหน่งสำเร็จ";

                    State = "SELECT IM_GoodsTransferItem";
                    var chkGTI = db.IM_GoodsTransferItem.Where(f => f.GoodsTransfer_Index == model.goodsTransfer_Index && f.Document_Status == 0).Count();
                    if (chkGTI == 1)
                    {
                        var GT = db.IM_GoodsTransfer.Find(model.goodsTransfer_Index);
                        var TGT = db.im_TaskTransfer.Find(TGTI.TaskTransfer_Index);
                        if (GT != null && TGT != null)
                        {
                            GT.Document_Status = 3;
                            GT.Update_By = model.username;
                            GT.Update_Date = DateTime.Now;

                            TGT.Document_Status = 2;
                            TGT.Update_By = model.username;
                            TGT.Update_Date = DateTime.Now;
                            result = "โอนย้ายเอกสารสำเร็จ";
                        }
                    }
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        State = "Save";
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("ScanRefTransferNo", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanRefTransferNo", msglog);
                return ex.Message;
            }
        }
        #endregion

        #region ConfirmTaskTransfer
        public string ConfirmTaskTransfer(ScanRefTransferNoViewModel model)
        {
            String State = "Start " + model.sJson();
            String msglog = "";
            var olog = new logtxt();
            var _tag_Index = "";
            try
            {
                var result = "";
                var process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");
                State = "SELECT IM_GoodsTransferItem";
                var goodsTransfer = db.IM_GoodsTransfer.FirstOrDefault(f => f.GoodsTransfer_Index == model.goodsTransfer_Index);
                var GTI = db.IM_GoodsTransferItem.FirstOrDefault(f => f.GoodsTransferItem_Index == model.goodsTransferItem_Index && f.GoodsTransfer_Index == model.goodsTransfer_Index && f.Document_Status == 0);
                var TGTI = db.im_TaskTransferItem.FirstOrDefault(f => f.Ref_DocumentItem_Index == model.goodsTransferItem_Index && f.Ref_Document_Index == model.goodsTransfer_Index && f.Document_Status == 0);
                if (GTI == null && TGTI == null)
                {
                    return "ไม่พบเลขที่ใบโอนย้ายที่ค้นหา";
                }
                else
                {
                    GTI.Document_Status = 1;
                    GTI.Update_Date = DateTime.Now;
                    GTI.Update_By = model.username;

                    TGTI.Document_Status = 1;
                    TGTI.Update_Date = DateTime.Now;
                    TGTI.Update_By = model.username;


                    #region Get default Location
                    var LocationViewModel = new { location_Name = "SA1011" };
                    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("GetLocation"), LocationViewModel.sJson());
                    var DataLocation = GetLocation.FirstOrDefault();
                    #endregion

                    #region update binbalance
                    var modelTag = new { ref_Document_Index = GTI.GoodsTransfer_Index, ref_DocumentItem_Index = GTI.GoodsTransferItem_Index };
                    var dataBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalanceFromReserve"), modelTag.sJson()).Where(c => c.binBalance_QtyReserve > 0).FirstOrDefault();

                    var dataBinbalanceTo = new BinBalanceViewModel();
                    dataBinbalanceTo = null;
                    if (!string.IsNullOrEmpty(GTI.Tag_No_To))
                    {
                        var modelTagNew = new { Tag_No = GTI.Tag_No_To };
                        dataBinbalanceTo = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), modelTagNew.sJson()).FirstOrDefault();
                    }

                    //if ((GTI.Location_Id != GTI.Location_Id_To) || (GTI.ItemStatus_Id != GTI.ItemStatus_Id_To) || (GTI.Tag_No != GTI.Tag_No_To) || GTI.ERP_Location != GTI.ERP_Location_To)
                    if ((GTI.Location_Id != GTI.Location_Id_To) || (GTI.ItemStatus_Id != GTI.ItemStatus_Id_To) || (GTI.Tag_No != GTI.Tag_No_To) || GTI.ERP_Location != GTI.ERP_Location_To || (GTI.GoodsReceive_MFG_Date != GTI.GoodsReceive_MFG_Date_To) || (GTI.GoodsReceive_EXP_Date != GTI.GoodsReceive_EXP_Date_To))
                    {

                        // Has Old Tag
                        if (dataBinbalanceTo != null)
                        {
                            var resultTag = new LPNViewModel();
                            var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_Index = GTI.Tag_Index_To } };
                            var tag = new DocumentViewModel();
                            tag.listDocumentViewModel = listTag;
                            resultTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), tag.sJson()).FirstOrDefault();
                            _tag_Index = resultTag.tag_Index.ToString();
                        }

                        if (_tag_Index == "")
                        {
                            //if (!string.IsNullOrEmpty(GTI.Tag_No_To))
                            if (dataBinbalance.binBalance_QtyBal != (GTI.Qty * GTI.Ratio))
                            {
                                GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                                grModel.goodsReceive_Index = GTI.GoodsReceive_Index;
                                grModel.owner_Index = Guid.Parse("8B8B6203-A634-4769-A247-C0346350A963");
                                grModel.tag_No = GTI.Tag_No_To;
                                grModel.create_By = model.username;
                                if (dataBinbalance.binBalance_QtyBal != (GTI.Qty * GTI.Ratio))
                                {
                                    _tag_Index = utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTag"), grModel.sJson());
                                }
                            }
                        }

                        // If tag_Index
                        if (!string.IsNullOrEmpty(_tag_Index))
                        {
                            #region Check Binbalance binBalance_QtyReserve
                            var binBalanceReserve = new
                            {
                                binbalance_Index = dataBinbalance.binBalance_Index
                            };
                            var dataBinbalanceReserve = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), binBalanceReserve.sJson());
                            #endregion

                            //var TagItemIndex = new Guid();
                            var TagItemIndex = Guid.NewGuid();

                            var resultNewTag = new LPNViewModel();
                            var listNewTag = new List<DocumentViewModel> { new DocumentViewModel { document_Index = new Guid(_tag_Index) } };
                            var newTag = new DocumentViewModel();
                            newTag.listDocumentViewModel = listNewTag;
                            resultNewTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), newTag.sJson()).FirstOrDefault();

                            var dataSplit = false;

                            //&& dataBinbalanceReserve.binBalance_QtyBal != dataBinbalanceReserve.binBalance_QtyReserve

                            if (dataBinbalance.binBalance_QtyBal != (GTI.Qty * GTI.Ratio))
                            {
                                GoodsReceiveTagItemViewModel item = new GoodsReceiveTagItemViewModel();

                                item.tag_Index = new Guid(_tag_Index.ToString());
                                item.tagItem_Index = Guid.NewGuid();
                                item.tag_No = resultNewTag.tag_No;
                                item.goodsReceive_Index = dataBinbalance.goodsReceive_Index;
                                item.goodsReceiveItem_Index = new Guid(dataBinbalance.goodsReceiveItem_Index.ToString());
                                item.process_Index = process_Index;
                                item.product_Index = dataBinbalance.product_Index;
                                item.product_Id = dataBinbalance.product_Id;
                                item.product_Name = dataBinbalance.product_Name;
                                item.product_SecondName = dataBinbalance.product_SecondName;
                                item.product_ThirdName = dataBinbalance.product_ThirdName;
                                item.product_Lot = dataBinbalance.product_Lot;
                                item.itemStatus_Index = dataBinbalance.itemStatus_Index;
                                item.itemStatus_Id = dataBinbalance.itemStatus_Id;
                                item.itemStatus_Name = dataBinbalance.itemStatus_Name;
                                item.qty = GTI.Qty;
                                item.productConversion_Ratio = Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                item.totalQty = Convert.ToDecimal(GTI.Qty) * Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                item.productConversion_Index = new Guid(dataBinbalance.productConversion_Index.ToString());
                                item.productConversion_Id = dataBinbalance.productConversion_Id;
                                item.productConversion_Name = dataBinbalance.productConversion_Name;
                                item.weight = dataBinbalance.binBalance_WeightBal;
                                item.volume = dataBinbalance.binBalance_VolumeBal;
                                //item.mfg_Date = dataBinbalance.goodsReceive_MFG_Date.toString();
                                //item.exp_Date = dataBinbalance.goodsReceive_EXP_Date.toString();
                                item.mfg_Date = GTI.GoodsReceive_MFG_Date.toString();
                                item.exp_Date = GTI.GoodsReceive_EXP_Date.toString();
                                item.tagRef_No1 = "";
                                item.tagRef_No2 = "";
                                item.tagRef_No3 = "";
                                item.tagRef_No4 = "";
                                item.tagRef_No5 = "";
                                item.tag_Status = 1;
                                //item.udf_1 = GTI.udf_1;
                                //item.udf_2 = GTI.udf_2;
                                //item.udf_3 = GTI.udf_3;
                                //item.udf_4 = GTI.udf_4;
                                //item.udf_5 = GTI.udf_5;
                                item.create_By = model.username;
                                item.create_Date = DateTime.Now.toString();
                                item.erp_Location = dataBinbalance.ERP_Location;
                                item.suggest_Location_Index = GTI.Location_Index_To;
                                item.suggest_Location_Id = GTI.Location_Id_To;
                                item.suggest_Location_Name = GTI.Location_Name_To;

                                dataSplit = true;
                                TagItemIndex = item.tagItem_Index;
                                utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTagItem"), item.sJson());
                            }


                            var binCardModel = new
                            {
                                isSplit = dataSplit,
                                binBalance_Index = dataBinbalance.binBalance_Index,
                                process_Index = process_Index,
                                documentType_Index = goodsTransfer.DocumentType_Index,
                                documentType_Id = goodsTransfer.DocumentType_Id,
                                documentType_Name = goodsTransfer.DocumentType_Name,
                                //goodsIssue_Date = data.create_Date,
                                tagItem_Index = TagItemIndex,
                                tag_Index = dataBinbalance.tag_Index,
                                tag_No = dataBinbalance.tag_No,
                                tag_Index_To = _tag_Index,
                                tag_No_To = resultNewTag.tag_No,
                                product_Index = dataBinbalance.product_Index,
                                product_Id = dataBinbalance.product_Id,
                                product_Name = dataBinbalance.product_Name,
                                product_SecondName = dataBinbalance.product_SecondName,
                                product_ThirdName = dataBinbalance.product_ThirdName,
                                product_Lot = dataBinbalance.product_Lot,
                                product_Lot_To = GTI.Product_Lot_To,
                                itemStatus_Index = dataBinbalance.itemStatus_Index,
                                itemStatus_Id = dataBinbalance.itemStatus_Id,
                                itemStatus_Name = dataBinbalance.itemStatus_Name,
                                itemStatus_Index_To = GTI.ItemStatus_Index_To,
                                itemStatus_Id_To = GTI.ItemStatus_Id_To,
                                itemStatus_Name_To = GTI.ItemStatus_Name_To,
                                productConversion_Index = dataBinbalance.productConversion_Index,
                                productConversion_Id = dataBinbalance.productConversion_Id,
                                productConversion_Name = dataBinbalance.productConversion_Name,
                                owner_Index = dataBinbalance.owner_Index,
                                owner_Id = dataBinbalance.owner_Id,
                                owner_Name = dataBinbalance.owner_Name,
                                location_Index = dataBinbalance.location_Index,
                                location_Id = dataBinbalance.location_Id,
                                location_Name = dataBinbalance.location_Name,
                                //location_Index_To = dataBinbalanceTo == null ? DataLocation.location_Index : dataBinbalanceTo.location_Index,
                                //location_Id_To = dataBinbalanceTo == null ? DataLocation.location_Id : dataBinbalanceTo.location_Id,
                                //location_Name_To = dataBinbalanceTo == null ? DataLocation.location_Name : dataBinbalanceTo.location_Name,
                                location_Index_To = GTI.Location_Index_To,
                                location_Id_To = GTI.Location_Id_To,
                                location_Name_To = GTI.Location_Name_To,
                                //mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                //exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                mfg_Date = GTI.GoodsReceive_MFG_Date,
                                mfg_Date_To = GTI.GoodsReceive_MFG_Date_To,
                                exp_Date = GTI.GoodsReceive_EXP_Date,
                                exp_Date_To = GTI.GoodsReceive_EXP_Date_To,
                                picking_Qty = (GTI.Qty * GTI.Ratio),
                                picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                picking_TotalQty = (GTI.Qty * GTI.Ratio) * (Decimal)dataBinbalance.binBalance_Ratio,
                                Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                ref_Document_No = model.goodsTransfer_No,
                                //ref_Document_Index = GoodsTransfer.GoodsTransfer_Index,
                                ref_Document_Index = GTI.GoodsTransfer_Index,
                                ref_DocumentItem_Index = GTI.GoodsTransferItem_Index,
                                userName = model.username,
                                erp_Location = GTI.ERP_Location,
                                erp_Location_To = GTI.ERP_Location_To,
                                isTransfer = true,
                                goodsReceiveItemLocation_Index = GTI.GoodsReceiveItemLocation_Index,
                                goodsReceiveItem_Index = GTI.GoodsReceiveItem_Index
                            };
                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransferNotCreateTagItem"), binCardModel.sJson());
                            if (!string.IsNullOrEmpty(CreateTagHeader))
                            {
                                TGTI.Binbalance_index = Guid.Parse(CreateTagHeader);
                            }
                            else {
                                return "โอนย้ายไม่สำเร็จ";
                            }


                        }
                        else
                        {

                            var binCardModel = new
                            {
                                binBalance_Index = dataBinbalance.binBalance_Index,
                                process_Index = process_Index,
                                documentType_Index = goodsTransfer.DocumentType_Index,
                                documentType_Id = goodsTransfer.DocumentType_Id,
                                documentType_Name = goodsTransfer.DocumentType_Name,
                                //goodsIssue_Date = itemHeader.Create_Date,
                                tagItem_Index = dataBinbalance.tagItem_Index,
                                tag_Index = dataBinbalance.tag_Index,
                                tag_No = dataBinbalance.tag_No,
                                tag_Index_To = dataBinbalance.tag_Index,
                                tag_No_To = dataBinbalance.tag_No,
                                product_Index = dataBinbalance.product_Index,
                                product_Id = dataBinbalance.product_Id,
                                product_Name = dataBinbalance.product_Name,
                                product_SecondName = dataBinbalance.product_SecondName,
                                product_ThirdName = dataBinbalance.product_ThirdName,
                                product_Lot = dataBinbalance.product_Lot,
                                product_Lot_To = GTI.Product_Lot_To,
                                itemStatus_Index = dataBinbalance.itemStatus_Index,
                                itemStatus_Id = dataBinbalance.itemStatus_Id,
                                itemStatus_Name = dataBinbalance.itemStatus_Name,
                                itemStatus_Index_To = GTI.ItemStatus_Index_To,
                                itemStatus_Id_To = GTI.ItemStatus_Id_To,
                                itemStatus_Name_To = GTI.ItemStatus_Name_To,
                                productConversion_Index = dataBinbalance.productConversion_Index,
                                productConversion_Id = dataBinbalance.productConversion_Id,
                                productConversion_Name = dataBinbalance.productConversion_Name,
                                owner_Index = dataBinbalance.owner_Index,
                                owner_Id = dataBinbalance.owner_Id,
                                owner_Name = dataBinbalance.owner_Name,
                                location_Index = dataBinbalance.location_Index,
                                location_Id = dataBinbalance.location_Id,
                                location_Name = dataBinbalance.location_Name,
                                location_Index_To = GTI.Location_Index_To,
                                location_Id_To = GTI.Location_Id_To,
                                location_Name_To = GTI.Location_Name_To,
                                //mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                //exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                mfg_Date = GTI.GoodsReceive_MFG_Date,
                                mfg_Date_To = GTI.GoodsReceive_MFG_Date_To,
                                exp_Date = GTI.GoodsReceive_EXP_Date,
                                exp_Date_To = GTI.GoodsReceive_EXP_Date_To,
                                picking_Qty = (GTI.Qty * GTI.Ratio),
                                picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                picking_TotalQty = (GTI.Qty * GTI.Ratio) * (Decimal)dataBinbalance.binBalance_Ratio,
                                Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)GTI.Qty,
                                ref_Document_No = model.goodsTransfer_No,
                                ref_Document_Index = model.goodsTransfer_Index,
                                ref_DocumentItem_Index = model.goodsTransferItem_Index,
                                userName = model.username,
                                erp_Location = GTI.ERP_Location,
                                erp_Location_To = GTI.ERP_Location_To,
                                isTransfer = true,
                                goodsReceiveItemLocation_Index = GTI.GoodsReceiveItemLocation_Index,
                                goodsReceiveItem_Index = GTI.GoodsReceiveItem_Index
                            };
                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransferNotCreateTagItem"), binCardModel.sJson());
                            if (CreateTagHeader != null)
                            {
                                TGTI.Binbalance_index = dataBinbalance.binBalance_Index;
                                //TGTI.Binbalance_index = Guid.Parse(CreateTagHeader);
                            }
                            //if (!string.IsNullOrEmpty(CreateTagHeader))
                            //{
                            //    TGTI.Binbalance_index = Guid.Parse(CreateTagHeader);
                            //}
                            else
                            {
                                return "โอนย้ายไม่สำเร็จ";
                            }
                        }
                    }
                    #endregion



                    result = "โอนย้ายตำแหน่งสำเร็จ";

                    State = "SELECT IM_GoodsTransferItem";
                    var chkGTI = db.IM_GoodsTransferItem.Where(f => f.GoodsTransfer_Index == model.goodsTransfer_Index && f.Document_Status == 0).Count();
                    if (chkGTI == 1)
                    {
                        var GT = db.IM_GoodsTransfer.Find(model.goodsTransfer_Index);
                        var TGT = db.im_TaskTransfer.Find(TGTI.TaskTransfer_Index);
                        if (GT != null && TGT != null)
                        {
                            GT.Document_Status = 3;
                            GT.Update_By = model.username;
                            GT.Update_Date = DateTime.Now;

                            TGT.Document_Status = 2;
                            TGT.Update_By = model.username;
                            TGT.Update_Date = DateTime.Now;
                            result = "โอนย้ายเอกสารสำเร็จ";
                        }
                    } else
                    {
                        var TGT = db.im_TaskTransfer.Find(TGTI.TaskTransfer_Index);
                        if (TGT != null)
                        {
                            TGT.Document_Status = 2;
                            TGT.Update_By = model.username;
                            TGT.Update_Date = DateTime.Now;
                            result = "โอนย้ายเอกสารสำเร็จ";
                        }
                    }
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        State = "Save";
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("ScanRefTransferNo", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("ScanRefTransferNo", msglog);
                return ex.Message;
            }
        }
        #endregion

        #region Bypass_confirmTaskTransfer
        public Result Bypass_confirmTaskTransfer(ConfirmTranferViewModel model)
        {
            Result result = new Result();
            var olog = new logtxt();
            olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_"+ model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "model : " + model.sJson() +"_"+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            try
            {
                olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "find tranfer : "+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                IM_GoodsTransfer goodsTransfer = db.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == model.goodsTransfer_Index && c.Document_Status == 2);
                if (goodsTransfer != null)
                {
                    olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "find tranfer item: " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    List<IM_GoodsTransferItem> goodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == model.goodsTransfer_Index && c.Document_Status == 0).ToList();
                    foreach (var item in goodsTransferItem)
                    {
                        
                        ScanRefTransferNoViewModel scanRef = new ScanRefTransferNoViewModel();
                        scanRef.goodsTransfer_No = model.goodsTransfer_No;
                        scanRef.goodsTransfer_Index = model.goodsTransfer_Index;
                        scanRef.goodsTransfer_Index = model.goodsTransfer_Index;
                        scanRef.goodsTransferItem_Index = item.GoodsTransferItem_Index;
                        scanRef.goodsTransferItem_Index = item.GoodsTransferItem_Index;
                        scanRef.username = model.update_By;
                        olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "Save tranfer item: " + scanRef.sJson() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        var result_scan = ConfirmTaskTransfer(scanRef);
                        olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "Save tranfer item Result: " + result_scan.sJson() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        if (result_scan != "โอนย้ายเอกสารสำเร็จ")
                        {
                            result.resultIsUse = false;
                            result.resultMsg = "ไม่สามารถทำการยืนยันเอกสารต่อได้";
                            break;
                        }
                        else {
                            result.resultIsUse = true;
                        }
                    }

                }
                else {
                    result.resultIsUse = false;
                    result.resultMsg = "สถานะของเอกสาร ไม่ถูกต้อง";
                    olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "Error find tranfer: " + result.resultMsg + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                }
               
                return result;
            }
            catch (Exception ex)
            {
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                olog.DataLogLines("Bypass_confirmTaskTransfer", "Bypass_confirmTaskTransfer_" + model.goodsTransfer_No + "_" + DateTime.Now.ToString("yyyy-MM-dd"), "Error : " + result.resultMsg + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                return result;
            }
        }
        #endregion

        #region UpdateRePutaway
        public string UpdateRePutaway(ScanRefTransferNoViewModel model)
        {
            String State = "Start " + model.sJson();
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = "";
                var _modelTaskItem = db.im_TaskTransferItem.Where(c => c.Ref_DocumentItem_Index == model.goodsTransferItem_Index && c.Ref_Document_Index == model.goodsTransfer_Index).FirstOrDefault();
                if(_modelTaskItem == null)
                {
                    return result = "ไม่พบ Task";
                }

                var transferModel = new
                {
                    docNo = _modelTaskItem.Ref_Document_No,
                    palletID = _modelTaskItem.Tag_No
                };
                var _sentPalletInspection = utils.SendDataApi<dynamic>(new AppSettingConfig().GetUrl("SentPalletInspection"), transferModel.sJson());

                _modelTaskItem.UDF_5 = "Re-Putaway";
                _modelTaskItem.Update_By = model.username;
                _modelTaskItem.Update_Date = DateTime.Now;
                result = "โอนย้ายเอกสารสำเร็จ";

                var transaction = db.Database.BeginTransaction();
                try
                {
                    State = "Save";
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("UpdateRePutaway", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                return result;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("UpdateRePutaway", msglog);
                return ex.Message;
            }
        }
        #endregion
    }
}
