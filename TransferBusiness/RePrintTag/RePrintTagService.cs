using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TransferBusiness.Library;
using TransferBusiness.ConfigModel;
using Comone.Utils;
using TransferDataAccess.Models;
using BinBalanceDataAccess.Models;
//using GRBusiness.LPNItem;

namespace TransferBusiness.Transfer
{
    public class RePrintTagService
    {
        private TransferDbContext db;
        private BinbalanceDbContext dbBinBalance;

        public RePrintTagService()
        {
            db = new TransferDbContext();
            dbBinBalance = new BinbalanceDbContext();
        }

        public RePrintTagService(TransferDbContext db)
        {
            this.db = db;
        }

        public List<RePrintTagViewModel> RePrintTagSearch(RePrintTagSearchViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = new List<RePrintTagViewModel>();

                //var queryResult = db.IM_GoodsTransferItem.Where(c => c.Document_Status != -1).AsQueryable();
                var queryResult = db.im_TaskTransferItem.Where(c => c.Document_Status != -1).AsQueryable();
                var queryBinBalanceResult = dbBinBalance.wm_BinBalance.Where(c => (c.BinBalance_QtyBal - c.BinBalance_QtyReserve) >= 0 && (c.BinBalance_QtyBal > 0) && (c.BinBalance_QtyReserve >= 0)).AsQueryable();

                if (!string.IsNullOrEmpty(data.goodsTransfer_Index))
                {
                    try
                    {
                        Guid GT_Index = new Guid(data.goodsTransfer_Index);
                        //queryResult = queryResult.Where(c => c.GoodsTransfer_Index == GT_Index);
                        queryResult = queryResult.Where(c => c.Ref_Document_Index == GT_Index);
                    }
                    catch
                    {
                        var query = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_No == data.goodsTransfer_No && c.Document_Status != -1).FirstOrDefault();
                        //queryResult = queryResult.Where(c => c.GoodsTransfer_Index == query.GoodsTransfer_Index);
                        queryResult = queryResult.Where(c => c.Ref_Document_Index == query.GoodsTransfer_Index);
                    }
                }

                if (!string.IsNullOrEmpty(data.tag_No))
                {
                    //queryResult = queryResult.Where(c => c.Tag_No == data.tag_No || c.Tag_No_To == data.tag_No);
                    queryBinBalanceResult = queryBinBalanceResult.Where(c => c.Tag_No == data.tag_No);
                }

                //if (data.product_Index != new Guid("00000000-0000-0000-0000-000000000000") && data.product_Index != null)
                //{
                //    queryResult = queryResult.Where(c => c.Product_Index == data.product_Index || c.Product_Index_To == data.product_Index);
                //}

                if (data.location_Index != new Guid("00000000-0000-0000-0000-000000000000") && data.location_Index != null)
                {
                    //queryResult = queryResult.Where(c => c.Location_Index == data.location_Index || c.Location_Index_To == data.location_Index);
                    queryBinBalanceResult = queryBinBalanceResult.Where(c => c.Location_Index == data.location_Index);
                }

                List<IM_GoodsTransferItem> queryResultItem = new List<IM_GoodsTransferItem>();
                List<im_TaskTransferItem> queryResultTaskItem = new List<im_TaskTransferItem>();
                List<wm_BinBalance> queryResultItemBinBalance = new List<wm_BinBalance>();

                if (string.IsNullOrEmpty(data.tag_No) && (data.location_Index == new Guid("00000000-0000-0000-0000-000000000000") || data.location_Index == null))
                {
                    queryResultTaskItem = queryResult.ToList();
                }
                else
                {
                    queryResultItemBinBalance = queryBinBalanceResult.ToList();
                }

                var filterModel = new ProcessStatusViewModel();
                filterModel.process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");

                var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());

                foreach (var items in queryResultItem)
                {
                    var model = new RePrintTagViewModel();
                    model.goodsTransferItem_Index = items.GoodsTransferItem_Index;
                    model.goodsTransfer_Index = items.GoodsTransfer_Index;
                    model.goodsReceiveItem_Index = items.GoodsReceiveItem_Index;
                    model.goodsReceive_Index = items.GoodsReceive_Index;
                    model.tagItem_Index = items.TagItem_Index;
                    model.tag_Index = items.Tag_Index;
                    model.tag_No = items.Tag_No;
                    model.tag_Index_To = items.Tag_Index_To;
                    model.tag_No_To = items.Tag_No_To;
                    model.goodsReceive_Index = items.GoodsReceive_Index;
                    model.goodsReceiveItem_Index = items.GoodsReceiveItem_Index;
                    model.product_Index = items.Product_Index;
                    model.product_Id = items.Product_Id;
                    model.product_Name = items.Product_Name;
                    model.product_SecondName = items.Product_SecondName;
                    model.product_ThirdName = items.Product_ThirdName;
                    model.product_Lot = items.Product_Lot;
                    model.itemStatus_Index = items.ItemStatus_Index;
                    model.itemStatus_Id = items.ItemStatus_Id;
                    model.itemStatus_Name = items.ItemStatus_Name;
                    model.itemStatus_Index_To = items.ItemStatus_Index_To;
                    model.itemStatus_Id_To = items.ItemStatus_Id_To;
                    model.itemStatus_Name_To = items.ItemStatus_Name_To;
                    model.location_Index = items.Location_Index;
                    model.location_Id = items.Location_Id;
                    model.location_Name = items.Location_Name;
                    model.location_Index_To = items.Location_Index_To;
                    model.location_Id_To = items.Location_Id_To;
                    model.location_Name_To = items.Location_Name_To;
                    model.qty = string.Format(String.Format("{0:N0}", items.Qty));
                    model.ratio = items.Ratio;
                    model.totalQty = items.TotalQty;
                    model.productConversion_Index = items.ProductConversion_Index;
                    model.productConversion_Id = items.ProductConversion_Id;
                    model.productConversion_Name = items.ProductConversion_Name;
                    model.weight = string.Format(String.Format("{0:N3}", items.Weight));
                    model.volume = string.Format(String.Format("{0:N3}", items.Volume));
                    model.uDF_1 = items.UDF_1;
                    model.uDF_2 = items.UDF_2;
                    model.uDF_3 = items.UDF_3;
                    model.uDF_4 = items.UDF_4;
                    model.uDF_5 = items.UDF_5;
                    model.create_By = items.Create_By;
                    model.create_Date = items.Create_Date;
                    model.update_By = items.Update_By;
                    model.update_Date = items.Update_Date;
                    model.cancel_By = items.Cancel_By;
                    model.cancel_Date = items.Cancel_Date;


                    result.Add(model);
                }

                foreach (var items in queryResultTaskItem)
                {
                    var model = new RePrintTagViewModel();

                    var dataBinBalance = dbBinBalance.wm_BinBalance.Where(c => c.BinBalance_QtyBal - c.BinBalance_QtyReserve > 0 && c.BinBalance_Index == items.Binbalance_index).FirstOrDefault();

                    if(dataBinBalance == null)
                    {
                        continue;
                    }

                    //model.goodsTransferItem_Index = items.GoodsTransferItem_Index;
                    //model.goodsTransfer_Index = items.GoodsTransfer_Index;
                    model.goodsReceiveItem_Index = dataBinBalance.GoodsReceiveItem_Index;
                    model.goodsReceive_Index = dataBinBalance.GoodsReceive_Index;
                    model.tagItem_Index = dataBinBalance.TagItem_Index;
                    model.tag_Index = dataBinBalance.Tag_Index;
                    model.tag_No = dataBinBalance.Tag_No;
                    //model.tag_Index_To = items.Tag_Index_To;
                    //model.tag_No_To = items.Tag_No_To;
                    model.goodsReceive_Index = dataBinBalance.GoodsReceive_Index;
                    model.goodsReceiveItem_Index = dataBinBalance.GoodsReceiveItem_Index;
                    model.product_Index = dataBinBalance.Product_Index;
                    model.product_Id = dataBinBalance.Product_Id;
                    model.product_Name = dataBinBalance.Product_Name;
                    model.product_SecondName = dataBinBalance.Product_SecondName;
                    model.product_ThirdName = dataBinBalance.Product_ThirdName;
                    model.product_Lot = dataBinBalance.Product_Lot;
                    model.itemStatus_Index = dataBinBalance.ItemStatus_Index;
                    model.itemStatus_Id = dataBinBalance.ItemStatus_Id;
                    model.itemStatus_Name = dataBinBalance.ItemStatus_Name;
                    //model.itemStatus_Index_To = items.ItemStatus_Index_To;
                    //model.itemStatus_Id_To = items.ItemStatus_Id_To;
                    //model.itemStatus_Name_To = items.ItemStatus_Name_To;
                    model.location_Index = dataBinBalance.Location_Index;
                    model.location_Id = dataBinBalance.Location_Id;
                    model.location_Name = dataBinBalance.Location_Name;
                    //model.location_Index_To = items.Location_Index_To;
                    //model.location_Id_To = items.Location_Id_To;
                    //model.location_Name_To = items.Location_Name_To;
                    model.qty = string.Format(String.Format("{0:N2}", dataBinBalance.BinBalance_QtyBal));
                    model.ratio = dataBinBalance.BinBalance_Ratio;
                    model.totalQty = dataBinBalance.BinBalance_QtyBal;
                    model.productConversion_Index = dataBinBalance.ProductConversion_Index;
                    model.productConversion_Id = dataBinBalance.ProductConversion_Id;
                    model.productConversion_Name = dataBinBalance.ProductConversion_Name;
                    model.weight = string.Format(String.Format("{0:N3}", dataBinBalance.BinBalance_WeightBal));
                    model.volume = string.Format(String.Format("{0:N3}", dataBinBalance.BinBalance_VolumeBal));
                    model.uDF_1 = dataBinBalance.UDF_1;
                    model.uDF_2 = dataBinBalance.UDF_2;
                    model.uDF_3 = dataBinBalance.UDF_3;
                    model.uDF_4 = dataBinBalance.UDF_4;
                    model.uDF_5 = dataBinBalance.UDF_5;
                    model.create_By = dataBinBalance.Create_By;
                    model.create_Date = dataBinBalance.Create_Date;
                    model.update_By = dataBinBalance.Update_By;
                    model.update_Date = dataBinBalance.Update_Date;
                    model.cancel_By = dataBinBalance.Cancel_By;
                    model.cancel_Date = dataBinBalance.Cancel_Date;


                    result.Add(model);
                }

                foreach (var items in queryResultItemBinBalance)
                {
                    var model = new RePrintTagViewModel();
                    //model.goodsTransferItem_Index = items.GoodsTransferItem_Index;
                    //model.goodsTransfer_Index = items.GoodsTransfer_Index;
                    model.goodsReceiveItem_Index = items.GoodsReceiveItem_Index;
                    model.goodsReceive_Index = items.GoodsReceive_Index;
                    model.tagItem_Index = items.TagItem_Index;
                    model.tag_Index = items.Tag_Index;
                    model.tag_No = items.Tag_No;
                    //model.tag_Index_To = items.Tag_Index_To;
                    //model.tag_No_To = items.Tag_No_To;
                    model.goodsReceive_Index = items.GoodsReceive_Index;
                    model.goodsReceiveItem_Index = items.GoodsReceiveItem_Index;
                    model.product_Index = items.Product_Index;
                    model.product_Id = items.Product_Id;
                    model.product_Name = items.Product_Name;
                    model.product_SecondName = items.Product_SecondName;
                    model.product_ThirdName = items.Product_ThirdName;
                    model.product_Lot = items.Product_Lot;
                    model.itemStatus_Index = items.ItemStatus_Index;
                    model.itemStatus_Id = items.ItemStatus_Id;
                    model.itemStatus_Name = items.ItemStatus_Name;
                    //model.itemStatus_Index_To = items.ItemStatus_Index_To;
                    //model.itemStatus_Id_To = items.ItemStatus_Id_To;
                    //model.itemStatus_Name_To = items.ItemStatus_Name_To;
                    model.location_Index = items.Location_Index;
                    model.location_Id = items.Location_Id;
                    model.location_Name = items.Location_Name;
                    //model.location_Index_To = items.Location_Index_To;
                    //model.location_Id_To = items.Location_Id_To;
                    //model.location_Name_To = items.Location_Name_To;
                    model.qty = string.Format(String.Format("{0:N0}", items.BinBalance_QtyBal));
                    model.ratio = items.BinBalance_Ratio;
                    model.totalQty = items.BinBalance_QtyBal;
                    model.productConversion_Index = items.ProductConversion_Index;
                    model.productConversion_Id = items.ProductConversion_Id;
                    model.productConversion_Name = items.ProductConversion_Name;
                    model.weight = string.Format(String.Format("{0:N3}", items.BinBalance_WeightBal));
                    model.volume = string.Format(String.Format("{0:N3}", items.BinBalance_VolumeBal));
                    model.uDF_1 = items.UDF_1;
                    model.uDF_2 = items.UDF_2;
                    model.uDF_3 = items.UDF_3;
                    model.uDF_4 = items.UDF_4;
                    model.uDF_5 = items.UDF_5;
                    model.create_By = items.Create_By;
                    model.create_Date = items.Create_Date;
                    model.update_By = items.Update_By;
                    model.update_Date = items.Update_Date;
                    model.cancel_By = items.Cancel_By;
                    model.cancel_Date = items.Cancel_Date;


                    result.Add(model);
                }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoTagFilter(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (data.key == "-")
                {
                    var query1 = db.IM_GoodsTransferItem.Where(c => c.Document_Status != -1 && !(c.Tag_No == null || c.Tag_No == "")).Select(s => new ItemListViewModel
                    {
                        index = s.Tag_Index,
                        name = s.Tag_No,
                        key = s.Tag_No
                    }).Distinct();

                    var query2 = db.IM_GoodsTransferItem.Where(c => c.Document_Status != -1 && !(c.Tag_No_To == null || c.Tag_No_To == "")).Select(s => new ItemListViewModel
                    {
                        index = s.Tag_Index_To,
                        name = s.Tag_No_To,
                        key = s.Tag_No_To
                    }).Distinct();

                    var query = query1.Union(query2);

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    //var query1 = db.IM_GoodsTransferItem.Where(c => c.Tag_No.Contains(data.key) && c.Document_Status != -1 && !(c.Tag_No == null || c.Tag_No == "")).Select(s => new ItemListViewModel
                    //{
                    //    index = s.Tag_Index,
                    //    name = s.Tag_No,
                    //    key = s.Tag_No
                    //}).Distinct();

                    //var query2 = db.IM_GoodsTransferItem.Where(c => c.Tag_No_To.Contains(data.key) && c.Document_Status != -1 && !(c.Tag_No_To == null || c.Tag_No_To == "")).Select(s => new ItemListViewModel
                    //{
                    //    index = s.Tag_Index_To,
                    //    name = s.Tag_No_To,
                    //    key = s.Tag_No_To
                    //}).Distinct();

                    var query3 = dbBinBalance.wm_BinBalance.Where(c => c.Tag_No.Contains(data.key) && (c.BinBalance_QtyBal - c.BinBalance_QtyReserve >= 0) && (c.BinBalance_QtyBal > 0) && (c.BinBalance_QtyReserve >= 0)).Select(s => new ItemListViewModel
                    {
                        index = s.Tag_Index,
                        name = s.Tag_No,
                        key = s.Tag_No
                    }).Distinct();

                    var query = query3;

                    //items = query.OrderBy(c => c.name).Take(10).ToList();
                    items = query.Take(10).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }
        public List<ItemListViewModel> autoLocationFilter(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (data.key == "-")
                {
                    var query1 = db.IM_GoodsTransferItem.Where(c => c.Document_Status != -1).Select(s => new ItemListViewModel
                    {
                        index = s.Location_Index,
                        name = s.Location_Name,
                        key = s.Location_Name
                    }).Distinct();

                    var query2 = db.IM_GoodsTransferItem.Where(c => c.Document_Status != -1).Select(s => new ItemListViewModel
                    {
                        index = s.Location_Index_To,
                        name = s.Location_Name_To,
                        key = s.Location_Name_To
                    }).Distinct();

                    var query = query1.Union(query2);

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    //var query1 = db.IM_GoodsTransferItem.Where(c => c.Location_Name.Contains(data.key) && c.Document_Status != -1).Select(s => new ItemListViewModel
                    //{
                    //    index = s.Location_Index,
                    //    name = s.Location_Name,
                    //    key = s.Location_Name
                    //}).Distinct();

                    //var query2 = db.IM_GoodsTransferItem.Where(c => c.Location_Name_To.Contains(data.key) && c.Document_Status != -1).Select(s => new ItemListViewModel
                    //{
                    //    index = s.Location_Index_To,
                    //    name = s.Location_Name_To,
                    //    key = s.Location_Name_To
                    //}).Distinct();

                    //var query = query1.Union(query2);

                    var query3 = dbBinBalance.wm_BinBalance.Where(c => c.Location_Name.Contains(data.key) && (c.BinBalance_QtyBal - c.BinBalance_QtyReserve > 0)).Select(s => new ItemListViewModel
                    {
                        index = s.Location_Index,
                        name = s.Location_Name,
                        key = s.Location_Name
                    }).Distinct();

                    var query = query3;

                    items = query.Take(10).ToList();

                    //items = query.OrderBy(c => c.name).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }

        public List<ItemListViewModel> autoGoodsTransferFilter(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (data.key == "-")
                {
                    var query1 = db.IM_GoodsTransfer.Where(c => c.Document_Status != -1 && !(c.GoodsTransfer_No == null || c.GoodsTransfer_No == "")).Select(s => new ItemListViewModel
                    {
                        index = s.GoodsTransfer_Index,
                        name = s.GoodsTransfer_No,
                        key = s.GoodsTransfer_No
                    }).Distinct();

                    items = query1.OrderBy(c => c.key).Take(10).ToList();
                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    var query1 = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_No.Contains(data.key) && c.Document_Status != -1 && !(c.GoodsTransfer_No == null || c.GoodsTransfer_No == "")).Select(s => new ItemListViewModel
                    {
                        index = s.GoodsTransfer_Index,
                        name = s.GoodsTransfer_No,
                        key = s.GoodsTransfer_No
                    }).Distinct();


                    items = query1.OrderBy(c => c.key).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }

    }
}
