using AspNetCore.Reporting;
using Comone.Utils;
using DataAccess;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using GRBusiness.Reports;
using InterfaceBusiness;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using planGIBusiness.AutoNumber;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TransferBusiness.ConfigModel;
using TransferBusiness.GoodIssue;
using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.Library;
using TransferBusiness.Libs;
using TransferBusiness.Transfer.Report;
using TransferDataAccess.Models;
using static TransferBusiness.Transfer.SearchGTModel;

namespace TransferBusiness.Transfer
{
    public class TransferBypassService
    {
        #region TransferBypassService
        private TransferDbContext db;

        private BinbalanceDbContext db2;

        private InboundDbContext dbInbound;

        public TransferBypassService()
        {
            db = new TransferDbContext();
            db2 = new BinbalanceDbContext();
            dbInbound = new InboundDbContext();
        }

        public TransferBypassService(TransferDbContext db)
        {
            this.db = db;

        }
        #endregion
        
        #region filter
        public TransferBypassViewModel filter()
        {
            try
            {

                TransferBypassViewModel transferBypassViewModel = new TransferBypassViewModel();
                try
                {

                    var partial = db.View_location_PP_waveEnd.OrderBy(c => Convert.ToInt64(c.Location_ID_X)).ToList();
                    foreach (var item in partial)
                    {
                        TransferBypassViewModel viewModel = new TransferBypassViewModel();

                        viewModel.BinBalance_Index = item.BinBalance_Index;
                        viewModel.Tag_No = item.Tag_No;
                        viewModel.Tag_Index = item.Tag_Index;
                        viewModel.TagItem_Index = item.TagItem_Index;
                        viewModel.Location_Index = item.Location_Index;
                        viewModel.Location_Id = item.Location_Id;
                        viewModel.Location_ID_X = item.Location_ID_X;
                        viewModel.Location_Name = item.Location_Name;
                        viewModel.Product_Index = item.Product_Index;
                        viewModel.Product_Id = item.Product_Id;
                        viewModel.Product_Name = item.Product_Name;
                        viewModel.ItemStatus_Id = item.ItemStatus_Id;
                        viewModel.ItemStatus_Index = item.ItemStatus_Index;
                        viewModel.ItemStatus_Name = item.ItemStatus_Name;
                        viewModel.Product_Lot = item.Product_Lot;
                        viewModel.UOM = item.UOM;
                        viewModel.Qty = item.Qty;
                        viewModel.BinBalance_QtyBal = item.BinBalance_QtyBal;
                        viewModel.SALE_ProductConversion_Index = item.SALE_ProductConversion_Index;
                        viewModel.SALE_ProductConversion_Id = item.SALE_ProductConversion_Id;
                        viewModel.SALE_ProductConversion_Name = item.SALE_ProductConversion_Name;
                        viewModel.SALE_ProductConversion_Ratio = item.SALE_ProductConversion_Ratio;
                        viewModel.ERP_Location = item.ERP_Location;
                        viewModel.goodsReceive_Index = item.GoodsReceive_Index.ToString();
                        viewModel.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                        viewModel.GoodsReceiveItemLocation_Index = item.GoodsReceiveItemLocation_Index;

                        transferBypassViewModel.ItemModel.Add(viewModel);
                    }

                    return transferBypassViewModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region getlocation
        public List<location_ToModel> getlocation(string jsonData)
        {
            try
            {
                TransferBypassViewModel Models = JsonConvert.DeserializeObject<TransferBypassViewModel>(jsonData);
                List<location_ToModel> location_To = new List<location_ToModel>();
                try
                {
                    var Littag = "";
                    foreach(var item in Models.ItemModel)
                    {
                        if (!string.IsNullOrEmpty(item.Tag_No))
                        {
                            Littag += ",";
                        }

                        Littag += item.Tag_No;
                    }
                    var TagNo = new SqlParameter("@TagNo", Littag);
                    var xx = db.TBL_IF_WMS_PP_RETRIEVAL.FromSql("sp_PP_RETRIEVAL @TagNo", TagNo).ToList();
                    if (xx.Count > 0)
                    {
                        return location_To;
                    }
                    else {

                        for (int i = 0; i < Models.ItemModel.Count(); i++)
                        {
                            location_ToModel model = new location_ToModel();
                            model.location_Index_To = Guid.Parse("35832da6-1a1f-4f7a-9122-721238d179f0");
                            model.location_Id_To = "Pond";
                            model.location_Name_To = "Pond";
                            location_To.Add(model);
                        }

                        //var partial = db.View_CheckLocation_Ongroud.Take(Models.ItemModel.Count()).ToList().OrderBy(c => c.Location_Name).ToList();
                        ////var partial = db.View_CheckLocation_Ongroud.Take(1).ToList().OrderBy(c => c.Location_Name).ToList();
                        //foreach (var item in partial)
                        //{
                        //    location_ToModel model = new location_ToModel();
                        //    model.location_Index_To = item.Location_Index;
                        //    model.location_Id_To = item.Location_Id;
                        //    model.location_Name_To = item.Location_Name;

                        //    location_To.Add(model);
                        //}
                        //if (partial.Count < Models.ItemModel.Count())
                        //{
                        //    for (int i = partial.Count; i < Models.ItemModel.Count(); i++)
                        //    {
                        //        location_ToModel model = new location_ToModel();
                        //        model.location_Index_To = Guid.Parse("46AA3038-A9D7-4069-8945-E5EE0F2189A1");
                        //        model.location_Id_To = "GT-Bulk-01";
                        //        model.location_Name_To = "GT-Bulk-01";
                        //        location_To.Add(model);
                        //    }


                        //}
                    }
                    

                    return location_To;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ConfirmBypass
        public Result ConfirmBypass(string jsonData)
        {
            logtxt log = new logtxt();
            Result result = new Result();
            log.DataLogLines("ConfirmBypass", "ConfirmBypass" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now);
            
            bool is_success = true;
            Guid Tranfer_index = Guid.NewGuid();
            
            
            try
            {
                TransferBypassViewModel Models = JsonConvert.DeserializeObject<TransferBypassViewModel>(jsonData);
                Result Create_H = CreateGoodsTransferHeader(Tranfer_index, Models.Create_by);
                if (Create_H.resultIsUse == true)
                {
                    foreach (var item in Models.ItemModel)
                    {
                        Result Create_I = CreateGoodsTransferItem(item, Tranfer_index, Models.Create_by);
                        if (Create_I.resultIsUse == false)
                        {
                            is_success = false;
                            break;
                        }
                    }

                    if (!is_success)
                    {
                        GoodsTransferService transferService = new GoodsTransferService();
                        List<IM_GoodsTransferItem> goodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == Tranfer_index && c.Document_Status == 0).ToList();
                        if (goodsTransferItem.Count() > 0)
                        {
                            foreach (var item in goodsTransferItem)
                            {
                                item.Document_Status = -1;
                                item.Cancel_By = "System";
                                item.Cancel_Date = DateTime.Now;
                            }

                            var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                            try
                            {
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception exy)
                            {
                                transaction.Rollback();
                                throw exy;
                            }

                            result.resultIsUse = false;
                            result.resultMsg = "กรุณาติดต่อ Admin หรือลองใหม่ในภายหลัง  เลขอ้างอิง :" + Tranfer_index;
                            return result;
                        }
                        else {
                            result.resultIsUse = false;
                            result.resultMsg = "ไม่สามารถทำการสร้าง คำสั่ง Bypass ได้กรุณาติดต่อ Admin เลขอ้างอิง : "+ Tranfer_index;
                            return result;
                        }
                        

                    }
                    else
                    {
                        var move_task = movelocationbypass(Tranfer_index, Models.Create_by);
                        if (!move_task.resultIsUse)
                        {
                            result.resultIsUse = false;
                            result.resultMsg = "กรุณาติดต่อ Admin หรือลองใหม่ในภายหลัง เลขอ้างอิง :" + Tranfer_index;
                            return result;
                        }
                        else
                        {
                            var SendBypass = new { docNo = Create_H.No };
                            var result_bypass = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("SendBypassAuto"), SendBypass.sJson());
                            if (result_bypass != "create_success")
                            {
                                result.resultIsUse = false;
                                result.resultMsg = result_bypass.ToString();
                                return result;
                            }
                            else {
                                result.resultIsUse = true;
                                result.resultMsg = "ส่งคำสั่ง Bypass partial เรียบร้อยแล้ว เลขที่โอนย้าย : "+Create_H.No;
                                return result;
                            }
                           
                        }
                    }

                }
                else
                {
                    result.resultIsUse = false;
                    result.resultMsg = Create_H.resultMsg;
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            } 
        }
        #endregion

        #region CreateGoodsTransferHeader
        public Result CreateGoodsTransferHeader(Guid index, string Confirm_By)
        {
            var olog = new logtxt();
            String GoodsTransferNo = "";
            Guid GoodsTransfer_Index = Guid.Empty;

            Result result_data = new Result();
            logtxt log = new logtxt();
            log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now);
            try
            {
                var filterModel = new DocumentTypeViewModel();
                var result = new List<GenDocumentTypeViewModel>();
                log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "1 : " + DateTime.Now);
                filterModel.process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");
                filterModel.documentType_Index = Guid.Parse("773DF9A5-83FC-4964-BECC-CAB7A0F482C7");
                result = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "2 : " + DateTime.Now);
                var genDoc = new AutoNumberService();
                DateTime DocumentDate = DateTime.Now;
                GoodsTransferNo = genDoc.genAutoDocmentNumber(result, DocumentDate);
                log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "3 : " + DateTime.Now);
                IM_GoodsTransfer itemHeader = new IM_GoodsTransfer();

                itemHeader.GoodsTransfer_Index = index;
                itemHeader.GoodsTransfer_No = GoodsTransferNo;
                itemHeader.GoodsTransfer_Date = DateTime.Now;
                itemHeader.GoodsTransfer_Time = null;
                itemHeader.GoodsTransfer_Doc_Date = DateTime.Now;
                itemHeader.GoodsTransfer_Doc_Time = null;
                itemHeader.Owner_Index = Guid.Parse("02B31868-9D3D-448E-B023-05C121A424F4");
                itemHeader.Owner_Id = "3419";
                itemHeader.Owner_Name = "Amazon";
                itemHeader.DocumentType_Index = Guid.Parse("773DF9A5-83FC-4964-BECC-CAB7A0F482C7");
                itemHeader.DocumentType_Id = "TF80";
                itemHeader.DocumentType_Name = "Transfer ByPass Sorter";
                itemHeader.DocumentRef_No2 = null;
                itemHeader.DocumentRef_No3 = null;
                itemHeader.DocumentRef_No4 = null;
                itemHeader.DocumentRef_Remark = null;
                itemHeader.Document_Status = 1;
                itemHeader.Create_By = Confirm_By;
                itemHeader.Create_Date = DateTime.Now;
                db.IM_GoodsTransfer.Add(itemHeader);

                log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "4 : " + DateTime.Now);
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "5 : " + DateTime.Now);
                }
                catch (Exception exy)
                {
                    transaction.Rollback();
                    result_data.resultIsUse = false;
                    result_data.resultMsg = exy.Message;
                    log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "6 : " + DateTime.Now);
                    return result_data;
                }

                result_data.resultIsUse = true;
                result_data.No = GoodsTransferNo;
                return result_data;

            }
            catch (Exception ex)
            {
                result_data.resultIsUse = false;
                result_data.resultMsg = ex.Message;

                return result_data;
            }

        }
        #endregion

        #region CreateGoodsTransferItem
        public Result CreateGoodsTransferItem(TransferBypassViewModel model,Guid Tranfer_index,string Create_by)
        {
            var olog = new logtxt();
            try
            {
                var result = new actionResultPickbinbalanceViewModel();
                var dataBinbalance = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), model.sJson());
                if (!dataBinbalance.resultIsUse)
                {
                    result.resultMsg = "";
                    result.resultIsUse = false;
                    return result;
                }
                else
                {
                    var GoodsTransferItem = new IM_GoodsTransferItem();
                    GoodsTransferItem.GoodsTransferItem_Index = Guid.NewGuid();
                    GoodsTransferItem.GoodsTransfer_Index = Tranfer_index;
                    GoodsTransferItem.TagItem_Index = dataBinbalance.tagItem_Index;
                    GoodsTransferItem.Tag_Index = dataBinbalance.tag_Index;
                    GoodsTransferItem.Tag_No = dataBinbalance.tag_No;
                    GoodsTransferItem.Product_Index = dataBinbalance.product_Index;
                    GoodsTransferItem.Product_Id = dataBinbalance.product_Id;
                    GoodsTransferItem.Product_Name = dataBinbalance.product_Name;
                    GoodsTransferItem.Product_SecondName = dataBinbalance.product_SecondName;
                    GoodsTransferItem.Product_ThirdName = dataBinbalance.product_ThirdName;
                    GoodsTransferItem.Product_Lot = dataBinbalance.product_Lot;
                    GoodsTransferItem.Product_Lot_To = dataBinbalance.product_Lot;
                    GoodsTransferItem.ItemStatus_Index = dataBinbalance.itemStatus_Index;
                    GoodsTransferItem.ItemStatus_Id = dataBinbalance.itemStatus_Id;
                    GoodsTransferItem.ItemStatus_Name = dataBinbalance.itemStatus_Name;
                    GoodsTransferItem.ItemStatus_Index_To = dataBinbalance.itemStatus_Index;
                    GoodsTransferItem.ItemStatus_Id_To = dataBinbalance.itemStatus_Id;
                    GoodsTransferItem.ItemStatus_Name_To = dataBinbalance.itemStatus_Name;
                    GoodsTransferItem.Location_Index = dataBinbalance.location_Index;
                    GoodsTransferItem.Location_Id = dataBinbalance.location_Id;
                    GoodsTransferItem.Location_Name = dataBinbalance.location_Name;
                    GoodsTransferItem.Location_Index_To = model.location_Index_To;
                    GoodsTransferItem.Location_Id_To = model.location_Id_To;
                    GoodsTransferItem.Location_Name_To = model.location_Name_To;
                    GoodsTransferItem.Qty = (dataBinbalance.binBalance_QtyBal / model.SALE_ProductConversion_Ratio).GetValueOrDefault();
                    GoodsTransferItem.Ratio = model.SALE_ProductConversion_Ratio;
                    GoodsTransferItem.TotalQty = dataBinbalance.binBalance_QtyBal;
                    GoodsTransferItem.ProductConversion_Index = model.SALE_ProductConversion_Index;
                    GoodsTransferItem.ProductConversion_Id = model.SALE_ProductConversion_Id;
                    GoodsTransferItem.ProductConversion_Name = model.SALE_ProductConversion_Name;
                    GoodsTransferItem.GoodsReceive_MFG_Date = dataBinbalance.goodsReceive_MFG_Date;
                    GoodsTransferItem.GoodsReceive_MFG_Date_To = dataBinbalance.goodsReceive_MFG_Date;
                    GoodsTransferItem.GoodsReceive_EXP_Date = dataBinbalance.goodsReceive_EXP_Date;
                    GoodsTransferItem.GoodsReceive_EXP_Date_To = dataBinbalance.goodsReceive_EXP_Date;
                    GoodsTransferItem.DocumentRef_No1 = dataBinbalance.binBalance_Index.ToString();

                    if (dataBinbalance.binBalance_WeightBegin != 0)
                    {
                        var unitWeight = dataBinbalance.binBalance_WeightBegin / dataBinbalance.binBalance_QtyBegin;

                        var WeightReserve = (dataBinbalance.binBalance_QtyBal * unitWeight);
                        GoodsTransferItem.Weight = WeightReserve;

                    }

                    if (dataBinbalance.binBalance_VolumeBegin != 0)
                    {
                        var unitVol = dataBinbalance.binBalance_VolumeBegin / dataBinbalance.binBalance_QtyBegin;

                        var VolReserve = (dataBinbalance.binBalance_QtyBal * unitVol);
                        GoodsTransferItem.Volume = VolReserve;
                    }
                    GoodsTransferItem.Document_Status = 0;

                    GoodsTransferItem.GoodsReceiveItem_Index = dataBinbalance.goodsReceiveItem_Index;
                    GoodsTransferItem.GoodsReceive_Index = dataBinbalance.goodsReceive_Index;
                    GoodsTransferItem.GoodsReceiveItemLocation_Index = dataBinbalance.goodsReceiveItemLocation_Index;

                    GoodsTransferItem.ERP_Location = dataBinbalance.ERP_Location;
                    GoodsTransferItem.ERP_Location_To = dataBinbalance.ERP_Location;
                    GoodsTransferItem.Create_By = Create_by;
                    GoodsTransferItem.Create_Date = DateTime.Now;

                    db.IM_GoodsTransferItem.Add(GoodsTransferItem);

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exy)
                    {
                        transaction.Rollback();
                        result.resultIsUse = false;
                        result.resultMsg = exy.Message;
                        return result;
                    }

                    
                    result.resultIsUse = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new actionResultPickbinbalanceViewModel();
                result.resultMsg = ex.Message;
                result.resultIsUse = false;
                return result;
            }
        }
        #endregion

        #region movelocationbypass
        public Result movelocationbypass (Guid Tranfer_index, string Create_by)
        {
            Result result = new Result();
            var olog = new logtxt();
            try
            {
                IM_GoodsTransfer goodsTransfer = db.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == Tranfer_index);
                if (goodsTransfer != null)
                {
                    List<IM_GoodsTransferItem> goodsTransferItems = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == Tranfer_index).ToList();
                    if (goodsTransferItems.Count() <= 0)
                    {
                        result.resultMsg = "กรุณาติดต่อ Admin เลขอ้างอิง : " + Tranfer_index;
                        result.resultIsUse = false;
                        return result;
                    }
                    else {
                        foreach (var item in goodsTransferItems)
                        {
                            var View_TaskInsertBinCard = new View_TaskInsertBinCardViewModel
                            {
                                binBalance_Index = Guid.Parse(item.DocumentRef_No1),
                                process_Index = Guid.Parse("CE757517-EBBC-4BEA-93CC-F7E139AE422C"),
                                documentType_Index = goodsTransfer.DocumentType_Index,
                                documentType_Id = goodsTransfer.DocumentType_Id,
                                documentType_Name = goodsTransfer.DocumentType_Name,
                                ref_Document_No = goodsTransfer.GoodsTransfer_No,
                                tagItem_Index = item.TagItem_Index,
                                tag_Index_To = item.Tag_Index,
                                tag_No_To = item.Tag_No,
                                product_Index = item.Product_Index,
                                product_Id = item.Product_Id,
                                product_Name = item.Product_Name,
                                product_SecondName = item.Product_Name,
                                product_ThirdName = item.Product_Name,
                                product_Lot = item.Product_Lot,
                                itemStatus_Index = item.ItemStatus_Index,
                                itemStatus_Id = item.ItemStatus_Id,
                                itemStatus_Name = item.ItemStatus_Name,
                                itemStatus_Index_To = item.ItemStatus_Index_To,
                                itemStatus_Id_To = item.ItemStatus_Id_To,
                                itemStatus_Name_To = item.ItemStatus_Name_To,
                                productConversion_Index = item.ProductConversion_Index,
                                productConversion_Id = item.ProductConversion_Id,
                                productConversion_Name = item.ProductConversion_Name,
                                owner_Index = Guid.Parse("02B31868-9D3D-448E-B023-05C121A424F4"),
                                owner_Id = "3419",
                                owner_Name = "ศูนย์กระจายสินค้าคาเฟ่อเมซอน",
                                location_Index = item.Location_Index,
                                location_Id = item.Location_Id,
                                location_Name = item.Location_Name,
                                location_Index_To = item.Location_Index_To,
                                location_Id_To = item.Location_Id_To,
                                location_Name_To = item.Location_Name_To,
                                picking_TotalQty = item.TotalQty,
                                ref_Document_Index = item.GoodsTransfer_Index,
                                ref_DocumentItem_Index = item.GoodsTransferItem_Index,
                                userName = Create_by,
                                isScanPick = true
                            };
                            var Bincard = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardBy_Scanpick"), View_TaskInsertBinCard.sJson());
                            if (!string.IsNullOrEmpty(Bincard))
                            {
                                item.DocumentRef_No2 = Bincard;
                                item.Document_Status = 1;
                                item.Update_By = Create_by;
                                item.Update_Date = DateTime.Now;
                            }
                        }
                        

                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception exy)
                        {
                            result.resultMsg = "กรุณาติดต่อ Admin เลขอ้างอิง : " + Tranfer_index+" : MSG : "+ exy.Message;
                            result.resultIsUse = false;
                            return result;
                        }

                        List<IM_GoodsTransferItem> goodsTransferItems_C = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == Tranfer_index && c.Document_Status != 1).ToList();
                        if (goodsTransferItems_C.Count == 0)
                        {
                            goodsTransfer.Document_Status = 3;
                            goodsTransfer.Update_By = Create_by;
                            goodsTransfer.Update_Date = DateTime.Now;

                            var transactionX = db.Database.BeginTransaction(IsolationLevel.Serializable);
                            try
                            {
                                db.SaveChanges();
                                transactionX.Commit();
                            }
                            catch (Exception exy)
                            {
                                result.resultMsg = "กรุณาติดต่อ Admin เลขอ้างอิง : " + Tranfer_index + " : MSG : " + exy.Message;
                                result.resultIsUse = false;
                                return result;
                            }
                        }

                        result.resultIsUse = true;
                    }
                    
                }
                else {
                    result.resultMsg = "กรุณาติดต่อ Admin เลขอ้างอิง : "+ Tranfer_index;
                    result.resultIsUse = false;
                    return result;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                result.resultMsg = ex.Message;
                result.resultIsUse = false;
                return result;
            }
        }
        #endregion

    }

}
