using BinbalanceBusiness.PickBinbalance.ViewModels;
using BinbalanceBusiness;
using BinBalanceDataAccess.Models;
using Business.Library;
using DataAccess;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static BinbalanceBusiness.PickBinbalance.ViewModels.FilterPickbinbalanceViewModel;
using ProductViewModel = MasterDataBusiness.ViewModels.ProductViewModel;
using Comone.Utils;
using BinBalanceBusiness.ViewModels;
using BinBalanceBusiness;

namespace BinbalanceBusiness.PickBinbalance
{
    public class PickBinbalance
    {
        private BinbalanceDbContext db;

        public PickBinbalance()
        {
            db = new BinbalanceDbContext();
        }
        public PickBinbalance(BinbalanceDbContext db)
        {
            this.db = db;
        }

        public actionResultPickbinbalanceViewModel Filter(FilterPickbinbalanceViewModel model)
        {
            var resul = new actionResultPickbinbalanceViewModel();
            var items = new List<PickbinbalanceViewModel>();
            var olog = new logtxt();
            try
            {
                var query = db.wm_BinBalance.AsQueryable();
                olog.logging("FilterPickbinbalance","getConfigFromBase | Config_LocationType_PA/PB/VC");
                var LocationIndex = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_LocationType_PA/PB/VC" }.sJson()).Split(',').Select(s => s == null || s == string.Empty ? (Guid?)null : Guid.Parse(s)).ToList();
                olog.logging("FilterPickbinbalance", "LocationConfig | " + LocationIndex.ToString());
                var location = utils.SendDataApi<List<locationViewModel>>(new AppSettingConfig().GetUrl("LocationConfig"), new { }.sJson()).Where(c => LocationIndex.Contains(c.locationType_Index)).ToList();


                query = query.Where(c => (c.BinBalance_QtyBal - c.BinBalance_QtyReserve) > 0);

                query = query.Where(c => location.Select(s => s.location_Index).Contains(c.Location_Index));

                query = query.Where(c => c.Owner_Index == model.owner_Index);

                #region filter data

                #region filter tab 1
                if (!string.IsNullOrEmpty(model.dropdownProductType?.productType_Index.ToString()))
                {
                    object product_type = new { productType_Index = model.dropdownProductType.productType_Index };
                    olog.logging("FilterPickbinbalance", "GetProductOfType | " + product_type.ToString());
                    var itemsproduct = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("GetProductOfType"), product_type.sJson());
                    query = query.Where(c => itemsproduct.Select(s => s.product_Index).Contains(c.Product_Index));
                }

                if (!string.IsNullOrEmpty(model.product_Index))
                {
                    query = query.Where(c => model.product_Index.Contains(c.Product_Index.ToString()));
                }
                else if (!string.IsNullOrEmpty(model.product_Id))
                {
                    query = query.Where(c => model.product_Id.Contains(c.Product_Id));
                }
                else if (!string.IsNullOrEmpty(model.product_Name))
                {
                    query = query.Where(c => model.product_Name.Contains(c.Product_Name));
                }

                if (!string.IsNullOrEmpty(model.goodsReceive_No))
                {
                    query = query.Where(c => model.goodsReceive_No.Contains(c.GoodsReceive_No));
                }

                if (!string.IsNullOrEmpty(model.dropdownItemStatus?.itemStatus_Index.ToString()))
                {
                    query = query.Where(c => c.ItemStatus_Index == model.dropdownItemStatus.itemStatus_Index.sParse<Guid>());
                }

                if (!string.IsNullOrEmpty(model.product_Lot))
                {
                    //query = query.Where(c => model.product_Lot.Contains(c.Product_Lot));
                    query = query.Where(c => c.Product_Lot == model.product_Lot);
                }

                if (!string.IsNullOrEmpty(model.tag_Index))
                {
                    try
                    {
                        query = query.Where(c => model.tag_Index.Contains(c.Tag_Index.ToString()));
                    }
                    catch
                    {
                        query = query.Where(c => model.tag_No.Contains(c.Tag_No));
                    }
                }

                if (!string.IsNullOrEmpty(model.tag_No))
                {
                    query = query.Where(c => model.tag_No.Contains(c.Tag_No));
                }
                if (!string.IsNullOrEmpty(model.erp_Location))
                {
                    query = query.Where(c => model.erp_Location.Contains(c.ERP_Location));
                }
                #endregion

                #region filter tab 2
                if (!string.IsNullOrEmpty(model.goodsReceive_Date) && !string.IsNullOrEmpty(model.goodsReceive_Date_To))
                {
                    query = query.Where(c => c.GoodsReceive_Date >= model.goodsReceive_Date.toBetweenDate().start && c.GoodsReceive_Date <= model.goodsReceive_Date_To.toBetweenDate().end);
                }
                if (!string.IsNullOrEmpty(model.mfg_Date) && !string.IsNullOrEmpty(model.mfg_Date_To))
                {
                    query = query.Where(c => c.GoodsReceive_MFG_Date >= model.mfg_Date.toBetweenDate().start && c.GoodsReceive_MFG_Date <= model.mfg_Date_To.toBetweenDate().end);
                }
                if (!string.IsNullOrEmpty(model.exp_Date) && !string.IsNullOrEmpty(model.exp_Date_To))
                {
                    query = query.Where(c => c.GoodsReceive_EXP_Date >= model.exp_Date.toBetweenDate().start && c.GoodsReceive_EXP_Date <= model.exp_Date_To.toBetweenDate().end);
                }
                #endregion

                object warehouse_Index = new { };
                olog.logging("FilterPickbinbalance", "locationFilter | " + warehouse_Index.ToString());
                var warehouseLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("locationFilter"), warehouse_Index.sJson());
                #region filter tab 3
                if (!string.IsNullOrEmpty(model.dropdownWarehouse?.warehouse_Index.ToString()))
                {
                    //object warehouse_Index = new { warehouse_Index = model.dropdownWarehouse?.warehouse_Index };
                    //var warehouse = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("locationFilter"), warehouse_Index.sJson());
                    var wahereWarehouse = warehouseLocation.Where(c => c.warehouse_Index == model.dropdownWarehouse?.warehouse_Index.ToString()).ToList();
                    query = query.Where(c => wahereWarehouse.Select(s => s.location_Index).Contains(c.Location_Index));
                }
                object zone_Index = new { };
                olog.logging("FilterPickbinbalance", "zoneLocationFilter | " + zone_Index.ToString());
                var zoneLocation = utils.SendDataApi<List<BinBalanceBusiness.ViewModels.ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("zoneLocationFilter"), zone_Index.sJson());
                if (!string.IsNullOrEmpty(model.dropdownZone?.zone_Index.ToString()))
                {
                    //object zone_Index = new { zone_Index = model.dropdownZone?.zone_Index };
                    //var zone = utils.SendDataApi<List<BinBalanceBusiness.ViewModels.ZoneLocationViewModel>>(new AppSettingConfig().GetUrl("zoneLocationFilter"), zone_Index.sJson());
                    var whereZone = zoneLocation.Where(c => c.Zone_Index == model.dropdownZone?.zone_Index).ToList();
                    query = query.Where(c => whereZone.Select(s => s.Location_Index).Contains(c.Location_Index));
                }
                if (!string.IsNullOrEmpty(model.location_Index.ToString()))
                {
                    query = query.Where(c => c.Location_Index == model.location_Index);
                }

                #endregion

                #endregion

                int count = query.Count();

                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);
                }

                var dataList = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();

                olog.logging("FilterPickbinbalance", "dropdownWarehouse");
                var warehouse = utils.SendDataApi<List<WarehouseViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), model.sJson());
                olog.logging("FilterPickbinbalance", "dropdownZone");
                var zone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("dropdownZone"), new { }.sJson());
                olog.logging("FilterPickbinbalance", "dropdownProductConversionV2");
                var productconversion = utils.SendDataApi<List<ProductConversionViewModel>>(new AppSettingConfig().GetUrl("dropdownProductConversionV2"), new { }.sJson());

                foreach (var d in dataList)
                {
                    var saleUnit = productconversion.Where(c => c.product_Index == d.Product_Index && c.sale_UNIT == 1 ).Select(c => c.productConversion_Name);
                   
                    var item = new PickbinbalanceViewModel
                    {
                        
                        binbalance_Index = d?.BinBalance_Index.ToString(),
                        goodsReceive_Index = d?.GoodsReceive_Index.ToString(),
                        goodsReceive_No = d?.GoodsReceive_No,
                        tag_Index = d?.Tag_Index.ToString(),
                        tag_No = d?.Tag_No,
                        product_Index = d?.Product_Index.ToString(),
                        product_Lot = d?.Product_Lot,
                        //documentType_Index = d.documentType_Index,
                        //documentType_Id    = d.documentType_Id   ,
                        //documentType_Name  = d.documentType_Name ,
                        product_Id = d?.Product_Id,
                        product_Name = d?.Product_Name,
                        qty = d?.BinBalance_QtyBal - d?.BinBalance_QtyReserve,
                        weight = d?.BinBalance_WeightBal - d?.BinBalance_WeightReserve,

                        binBalance_UnitWeightBal = d?.BinBalance_UnitWeightBal,
                        binBalance_UnitWeightBal_Index = d?.BinBalance_UnitWeightBal_Index,
                        binBalance_UnitWeightBal_Id = d?.BinBalance_UnitWeightBal_Id,
                        binBalance_UnitWeightBal_Name = d?.BinBalance_UnitWeightBal_Name,
                        binBalance_UnitWeightBalRatio = d?.BinBalance_UnitWeightBalRatio,
                        binBalance_UnitNetWeightBal = d?.BinBalance_UnitNetWeightBal,
                        binBalance_UnitNetWeightBal_Index = d?.BinBalance_UnitNetWeightBal_Index,
                        binBalance_UnitNetWeightBal_Id = d?.BinBalance_UnitNetWeightBal_Id,
                        binBalance_UnitNetWeightBal_Name = d?.BinBalance_UnitNetWeightBal_Name,
                        binBalance_UnitNetWeightBalRatio = d?.BinBalance_UnitNetWeightBalRatio,
                        binBalance_UnitGrsWeightBal = d?.BinBalance_UnitGrsWeightBal,
                        binBalance_UnitGrsWeightBal_Index = d?.BinBalance_UnitGrsWeightBal_Index,
                        binBalance_UnitGrsWeightBal_Id = d?.BinBalance_UnitGrsWeightBal_Id,
                        binBalance_UnitGrsWeightBal_Name = d?.BinBalance_UnitGrsWeightBal_Name,
                        binBalance_UnitGrsWeightBalRatio = d?.BinBalance_UnitGrsWeightBalRatio,
                        binBalance_UnitWidthBal = d?.BinBalance_UnitWidthBal,
                        binBalance_UnitWidthBal_Index = d?.BinBalance_UnitWidthBal_Index,
                        binBalance_UnitWidthBal_Id = d?.BinBalance_UnitWidthBal_Id,
                        binBalance_UnitWidthBal_Name = d?.BinBalance_UnitWidthBal_Name,
                        binBalance_UnitWidthBalRatio = d?.BinBalance_UnitWidthBalRatio,
                        binBalance_UnitLengthBal = d?.BinBalance_UnitLengthBal,
                        binBalance_UnitLengthBal_Index = d?.BinBalance_UnitLengthBal_Index,
                        binBalance_UnitLengthBal_Id = d?.BinBalance_UnitLengthBal_Id,
                        binBalance_UnitLengthBal_Name = d?.BinBalance_UnitLengthBal_Name,
                        binBalance_UnitLengthBalRatio = d?.BinBalance_UnitLengthBalRatio,
                        binBalance_UnitHeightBal = d?.BinBalance_UnitHeightBal,
                        binBalance_UnitHeightBal_Index = d?.BinBalance_UnitHeightBal_Index,
                        binBalance_UnitHeightBal_Id = d?.BinBalance_UnitHeightBal_Id,
                        binBalance_UnitHeightBal_Name = d?.BinBalance_UnitHeightBal_Name,
                        binBalance_UnitHeightBalRatio = d?.BinBalance_UnitHeightBalRatio,

                        productConversion_Index = d?.ProductConversion_Index.ToString(),
                        productConversion_Id = d?.ProductConversion_Id,
                        productConversion_Name = d?.ProductConversion_Name,
                        productConversion_Ratio = d?.BinBalance_Ratio,
                        status_Index = d?.ItemStatus_Index.ToString(),
                        status_Id = d?.ItemStatus_Id,
                        status_Name = d?.ItemStatus_Name,
                        location_Index = d?.Location_Index.ToString(),
                        location_Id = d?.Location_Id,
                        location_Name = d?.Location_Name,
                        goodsReceive_Date = d?.GoodsReceive_Date != null ? d?.GoodsReceive_Date.toString() : "",
                        goodsReceive_MFG_Date = d?.GoodsReceive_MFG_Date != null ? d?.GoodsReceive_MFG_Date.toString() : "",
                        goodsReceive_EXP_Date = d?.GoodsReceive_EXP_Date != null ? d?.GoodsReceive_EXP_Date.toString() : "",
                        //warehouse_Index = warehouse.FirstOrDefault(f => f.warehouse_Index == new Guid(warehouseLocation.FirstOrDefault(wf => wf.location_Index == d?.Location_Index)?.warehouse_Index))?.warehouse_Index.ToString(),
                        //warehouse_Id = warehouse.FirstOrDefault(f => f.warehouse_Index == new Guid(warehouseLocation.FirstOrDefault(wf => wf.location_Index == d?.Location_Index)?.warehouse_Index))?.warehouse_Id,
                        //warehouse_Name = warehouse.FirstOrDefault(f => f.warehouse_Index == new Guid(warehouseLocation.FirstOrDefault(wf => wf.location_Index == d?.Location_Index)?.warehouse_Index))?.warehouse_Name,
                        zone_Index = zone?.FirstOrDefault(f => f.zone_Index == zoneLocation?.FirstOrDefault(zf => zf.Location_Index == d?.Location_Index)?.Zone_Index)?.zone_Index.ToString(),
                        zone_Id = zone?.FirstOrDefault(f => f.zone_Index == zoneLocation?.FirstOrDefault(zf => zf.Location_Index == d?.Location_Index)?.Zone_Index)?.zone_Id,
                        zone_Name = zone?.FirstOrDefault(f => f.zone_Index == zoneLocation?.FirstOrDefault(zf => zf.Location_Index == d?.Location_Index)?.Zone_Index)?.zone_Name,
                        sale_unit = (saleUnit.FirstOrDefault() == null ) ? null : saleUnit.FirstOrDefault(),
                        Erp_location = d.ERP_Location,
                    };
                    items.Add(item);
                }

                resul.items = items;
                resul.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return resul;
            }
            catch (Exception ex)
            {
                olog.logging("FilterPickbinbalance", "resultMsg" + ex.Message);
                resul.resultMsg = ex.Message;
                return resul;
            }
        }

    }
}

