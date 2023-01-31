using Comone.Utils;
using DataAccess;
using GIBusiness.GoodIssue;
using GIBusiness.PlanGoodIssue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace TransferBusiness.Transfer
{
    public class TransferStockAdjustmentService
    {
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

        public List<BinBalanceDocViewModel> ScanLocation(BinBalanceDocViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    string SqlWhere = "";
                    if (data.LocationName != "" && data.LocationName != null)
                    {
                        SqlWhere = " and Location_Name = N'" + data.LocationName + "'" +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.OwnerIndex + "'" +
                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                        " and BinBalance_QtyBal > 0 and BinBalance_QtyReserve = 0 ";
                    }

                    //string pstring = " and Location_Name = N'" + LocationName + "'";
                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).OrderBy(o => o.GoodsReceive_EXP_Date).ToList();

                    var result = new List<BinBalanceDocViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new BinBalanceDocViewModel();
                        resultItem.BinBalance_Index = item.BinBalance_Index;
                        resultItem.Owner_Index = item.Owner_Index;
                        resultItem.Owner_Id = item.Owner_Id;
                        resultItem.Owner_Name = item.Owner_Name;
                        resultItem.Product_Index = item.Product_Index;
                        resultItem.Product_Id = item.Product_Id;
                        resultItem.Product_Name = item.Product_Name;
                        resultItem.BinBalance_QtyBal = item.BinBalance_QtyBal;
                        resultItem.ProductConversion_Index = item.ProductConversion_Index;
                        resultItem.ProductConversion_Id = item.ProductConversion_Id;
                        resultItem.ProductConversion_Name = item.ProductConversion_Name;
                        if (item.ProductConversion_Name != null)
                        {
                            //Find RatioConvertion
                            var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ProductConversion_Ratio)");
                            var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),ProductConversion_VolumeRatio)");
                            var ColumnName3 = new SqlParameter("@ColumnName3", "Convert(Nvarchar(50),ProductConversion_Volume)");
                            var ColumnName4 = new SqlParameter("@ColumnName4", "Convert(Nvarchar(50),ProductConversion_Weight)");
                            var ColumnName5 = new SqlParameter("@ColumnName5", "ProductConversion_Name");
                            var TableName = new SqlParameter("@TableName", "ms_ProductConversion");
                            var Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),ProductConversion_Index)  ='" + item.ProductConversion_Index.ToString() + "'");
                            var strProductConvertion = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

                            resultItem.ProductConversion_Ratio = strProductConvertion[0].dataincolumn1.sParse<decimal>();
                        }
                        resultItem.LocationIndex = item.Location_Index;
                        resultItem.LocationId = item.Location_Id;
                        resultItem.LocationName = item.Location_Name;
                        resultItem.ItemStatus_Index = item.ItemStatus_Index;
                        resultItem.ItemStatus_Id = item.ItemStatus_Id;
                        resultItem.ItemStatus_Name = item.ItemStatus_Name;
                        resultItem.GoodsReceive_EXP_Date = item.GoodsReceive_EXP_Date.HasValue ? item.GoodsReceive_EXP_Date.Value.toString() : "";
                        resultItem.UDF_1 = item.UDF_1;
                        resultItem.UDF_2 = item.UDF_2;
                        resultItem.UDF_3 = item.UDF_3;

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

        public List<SumQtyBinbalanceViewModel> SumQty(SumQtyBinbalanceViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    if (data.productConversionBarcode == null)
                    {
                        //var queryResult = context.View_SumQtyBinbalance.FromSql("sp_GetViewSumQtyBinbalance").Where(c => c.Location_Name == data.LocationName && c.Owner_Index == data.ownerIndex && c.Warehouse_Index == data.WareHouseIndex).ToList();
                        string SqlWhere = "";

                        SqlWhere = " and Location_Name = N'" + data.LocationName + "'" +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                        //" and Convert(Nvarchar(50),Warehouse_Index)  = N'" + data.WareHouseIndex + "'";
                        " and Warehouse_Index  = '" + data.WareHouseIndex + "'";


                        var strwhere = new SqlParameter("@strwhere", SqlWhere);
                        //var Product = context.ms_ProductConversionBarcode.FromSql("sp_GetProductConversionBarcode").Where(c => c.ProductConversionBarcode == BarCode).FirstOrDefault();

                        var queryResult = context.View_SumQtyBinbalance.FromSql("sp_GetViewSumQtyBinbalance @strwhere", strwhere).ToList();

                        var result = new List<SumQtyBinbalanceViewModel>();
                        foreach (var item in queryResult)
                        {
                            var resultItem = new SumQtyBinbalanceViewModel();
                            resultItem.ProductName = item.Product_Name;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal;
                            resultItem.productConversionName = item.ProductConversion_Name;
                            result.Add(resultItem);
                        }

                        return result;
                    }

                    else
                    {
                        string SqlWhere = "";
                        if (data.productConversionBarcode != "" && data.productConversionBarcode != null)
                        {
                            SqlWhere = " and Location_Name = N'" + data.LocationName + "'" +
                            " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.productConversionBarcode + "') " +
                            " and ProductConversion_Index in (select ProductConversion_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.productConversionBarcode + "') " +
                            " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                            " and Warehouse_Index  = '" + data.WareHouseIndex + "'";
                        }

                        var strwhere = new SqlParameter("@strwhere", SqlWhere);
                        //var Product = context.ms_ProductConversionBarcode.FromSql("sp_GetProductConversionBarcode").Where(c => c.ProductConversionBarcode == BarCode).FirstOrDefault();

                        var queryResult = context.View_SumQtyBinbalance.FromSql("sp_GetViewSumQtyBinbalance @strwhere", strwhere).ToList();
                        //var Barcode = context.ms_ProductConversionBarcode.FromSql("sp_GetProductConversionBarcode").Where(c => c.ProductConversionBarcode == data.productConversionBarcode).FirstOrDefault();

                        //var queryResult = context.View_SumQtyBinbalance.FromSql("sp_GetViewSumQtyBinbalance").Where(c => c.Location_Name == data.LocationName && c.Product_Index == Barcode.Product_Index && c.Owner_Index == data.ownerIndex && c.Warehouse_Index == data.WareHouseIndex).ToList();

                        var result = new List<SumQtyBinbalanceViewModel>();
                        foreach (var item in queryResult)
                        {
                            var resultItem = new SumQtyBinbalanceViewModel();
                            resultItem.ProductName = item.Product_Name;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal;
                            resultItem.productConversionName = item.ProductConversion_Name;
                            result.Add(resultItem);
                        }

                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<OwnerViewModel> filterOwner()
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var queryResult = context.MS_Owner.FromSql("sp_GetOwner").Where(c => c.Owner_Index == new Guid("8B8B6203-A634-4769-A247-C0346350A963")).ToList();
                    var result = new List<OwnerViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new OwnerViewModel();

                        resultItem.OwnerIndex = item.Owner_Index;
                        resultItem.OwnerId = item.Owner_Id;
                        resultItem.OwnerName = item.Owner_Name;
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

        public List<WarehouseViewModel> filterWarehouse()
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    
                    var strtfwhere = new SqlParameter("@strwhere", "and Warehouse_Index = '72885519-D256-4AAD-9C37-A783B90E1DF6'");
                    var queryResult = context.MS_Warehouse.FromSql("sp_GetTransferWarehouse @strwhere", strtfwhere).ToList();
                    var result = new List<WarehouseViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new WarehouseViewModel();

                        resultItem.WarehouseIndex = item.Warehouse_Index;
                        resultItem.WarehouseId = item.Warehouse_Id;
                        resultItem.WarehouseName = item.Warehouse_Name;
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

        //public String SaveChanges(TransferStockAdjustmentDocViewModel data)
        //{
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    var SqlClearReserve = new List<string>();

        //    var SqlClearBal = new List<string>();


        //    Boolean IsReserve = false;
        //    Boolean IsAdjust= false;
        //    Boolean IsGr = false;
        //    Boolean IsTag = false;
        //    Boolean IsBin = false;
        //    Boolean IsGi = false;

        //    Guid pStockAdjustmentIndex = Guid.NewGuid();
        //    Guid pGoodsReceiveIndex = Guid.NewGuid();
        //    Guid pTagItemIndex = Guid.NewGuid();
        //    Guid pBinBalance_Index = Guid.NewGuid();
        //    Guid pBinCard_Index = Guid.NewGuid();
        //    Guid pGoodsIssueIndex = Guid.NewGuid();
        //    Guid pBinCardRe_Index = Guid.NewGuid();


        //    //decimal? bBinBalance_QtyBal = 0;
        //    //decimal? bBinBalance_QtyReserve = 0;
        //    //decimal? bBinBalance_WeightReserve = 0;
        //    //decimal? bBinBalance_VolumeReserve = 0;
        //    //decimal? bBinBalance_WeightBal = 0;
        //    //decimal? bBinBalance_VolumeBal = 0;
        //    //String bBinBalance_Index = "";

        //    try
        //    {


        //        using (var context = new TransferDbContext())
        //        {
        //            if (data.StockAdjustmentIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //            {
        //                data.StockAdjustmentIndex = pStockAdjustmentIndex;
        //            }
        //            if (data.StockAdjustmentNo == null)
        //            {
        //                var DocumentType_Index = new SqlParameter("@DocumentType_Index", new Guid("D945100E-C1C4-4D9C-BEA4-86BD55CE61FF"));

        //                var DocDate = new SqlParameter("@DocDate", DateTime.Now);

        //                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultParameter.Size = 2000; // some meaningfull value
        //                resultParameter.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
        //                //var result = resultParameter.Value;
        //                data.StockAdjustmentNo = resultParameter.Value.ToString();
        //            }

        //            //----Set Header------
        //            var itemHeader = new TransferStockAdjustmentViewModel();
        //            var document_status = 0;



        //            itemHeader.StockAdjustmentIndex = data.StockAdjustmentIndex;
        //            itemHeader.StockAdjustmentNo = data.StockAdjustmentNo;
        //            itemHeader.OwnerIndex = data.OwnerIndex;
        //            itemHeader.StockAdjustmentDate = DateTime.Now;
        //            itemHeader.DocumentTypeIndex = new Guid("D945100E-C1C4-4D9C-BEA4-86BD55CE61FF");
        //            itemHeader.DocumentRefNo1 = data.DocumentRefNo1;
        //            itemHeader.DocumentRefNo2 = data.DocumentRefNo2;
        //            itemHeader.DocumentRefNo3 = data.DocumentRefNo3;
        //            itemHeader.DocumentRefNo4 = data.DocumentRefNo4;
        //            itemHeader.DocumentRefNo5 = data.DocumentRefNo5;
        //            itemHeader.DocumentStatus = document_status;
        //            itemHeader.UDF1 = data.UDF1;
        //            itemHeader.UDF2 = data.UDF2;
        //            itemHeader.UDF3 = data.UDF3;
        //            itemHeader.UDF4 = data.UDF4;
        //            itemHeader.UDF5 = data.UDF5;
        //            itemHeader.DocumentPriorityStatus = data.DocumentPriorityStatus;
        //            itemHeader.CreateBy = data.CreateBy;
        //            itemHeader.CreateDate = DateTime.Now;
        //            itemHeader.ReasonCodeIndex = data.ReasonCodeIndex;
        //            itemHeader.ReasonCodeId = data.ReasonCodeId;
        //            itemHeader.ReasonCodeName = data.ReasonCodeName;


        //            //----Set Detail-----

        //            var itemDetail = new List<TransferStockAdjustmentItemViewModel>();

        //            string SqlWhereBalance = " and BinBalance_Index = N'" + data.BinBalanceIndex + "'" +
        //                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.OwnerIndex + "' " +
        //                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50),Warehouse_Index) = N'" + data.WarehouseIndex + "') " +
        //                        " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";

        //            var strwhereBinBalance = new SqlParameter("@strwhere", SqlWhereBalance);

        //            var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhereBinBalance).FirstOrDefault();


        //            string SqlWhereWare = " and Convert(Nvarchar(50),Warehouse_Index) = N'" + data.WarehouseIndex + "' ";
        //            var strwhereWarehouse = new SqlParameter("@strwhere", SqlWhereWare);
        //            var queryWarehouse = context.MS_Warehouse.FromSql("sp_GetWarehouse @strwhere", strwhereWarehouse).FirstOrDefault();

        //            //var queryWarehouse = context.MS_Warehouse.FromSql("sp_GetWarehouse").Where(c => c.Warehouse_Index == data.WarehouseIndex).FirstOrDefault();

        //            if (queryResult == null)
        //            {
        //                return "Fail No Data";
        //            }
        //            var resultItem = new TransferStockAdjustmentItemViewModel();
        //            // Gen Index for line item
        //            if (data.StockAdjustmentItemItemIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //            {
        //                data.StockAdjustmentItemItemIndex = Guid.NewGuid();
        //            }

        //            // Index From Header
        //            resultItem.StockAdjustmentIndex = data.StockAdjustmentIndex;
        //            resultItem.StockAdjustmentItemItemIndex = data.StockAdjustmentItemItemIndex;
        //            resultItem.GoodsReceiveIndex = queryResult.GoodsReceive_Index;
        //            resultItem.GoodsReceiveItemIndex = queryResult.GoodsReceiveItem_Index;
        //            resultItem.GoodsReceiveItemLocationIndex = queryResult.GoodsReceiveItemLocation_Index;
        //            resultItem.TagIndex = queryResult.Tag_Index;
        //            resultItem.TagIndexTo = queryResult.Tag_Index;
        //            resultItem.TagItemIndex = queryResult.TagItem_Index;



        //            resultItem.ProductIndex = queryResult.Product_Index;
        //            resultItem.ProductIndexTo = queryResult.Product_Index;
        //            if (queryResult.Product_Lot != null)
        //            {
        //                resultItem.ProductLot = queryResult.Product_Lot;
        //                resultItem.ProductLotTo = queryResult.Product_Lot;

        //            }
        //            else
        //            {
        //                resultItem.ProductLot = "";
        //                resultItem.ProductLotTo = "";
        //            }
        //            resultItem.ItemStatusIndex = queryResult.ItemStatus_Index;
        //            resultItem.ItemStatusIndexTo = queryResult.ItemStatus_Index;
        //            resultItem.ProductConversionIndex = queryResult.ProductConversion_Index;
        //            resultItem.OwnerIndex = queryResult.Owner_Index;
        //            resultItem.OwnerIndexTo = queryResult.Owner_Index;
        //            resultItem.LocationIndex = queryResult.Location_Index;
        //            resultItem.LocationIndexTo = queryResult.Location_Index;
        //            resultItem.GoodsReceiveEXPDate = queryResult.GoodsReceive_EXP_Date;
        //            resultItem.GoodsReceiveEXPDateTo = queryResult.GoodsReceive_EXP_Date;

        //            //if (data.Qty == 0)
        //            //{
        //            //    resultItem.Qty = 0;
        //            //}

        //            if (data.BinBalanceQtyBal > data.Qty)
        //            {
        //                resultItem.Qty = data.BinBalanceQtyBal - data.Qty;
        //            }
        //            else
        //            {
        //                resultItem.Qty = data.Qty - data.BinBalanceQtyBal;
        //            }



        //            resultItem.TotalQty = resultItem.Qty; // queryResult.BinBalance_Ratio * resultItem.Qty;
        //                                                  //     resultItem.Weight = queryResult.BinBalance_WeightBal;

        //            if (queryResult.BinBalance_WeightBegin == 0)
        //            {
        //                resultItem.Weight = 0;
        //            }
        //            else
        //            { 
        //                resultItem.Weight = resultItem.Qty * (queryResult.BinBalance_WeightBegin / queryResult.BinBalance_QtyBegin);
        //            }

        //            // resultItem.Volume = queryResult.BinBalance_VolumeBal;
        //            if (queryResult.BinBalance_VolumeBegin == 0)
        //            {
        //                resultItem.Volume = 0;
        //            }
        //            else
        //            {                       
        //                resultItem.Volume = resultItem.Qty * (queryResult.BinBalance_VolumeBegin / queryResult.BinBalance_QtyBegin);
        //            }
        //            resultItem.RefProcessIndex = data.RefProcessIndex;
        //            resultItem.RefDocumentIndex = data.RefDocumentIndex;
        //            resultItem.RefDocumentNo = data.RefDocumentNo;
        //            resultItem.RefDocumentItemIndex = data.RefDocumentItemIndex;
        //            resultItem.CreateBy = itemHeader.CreateBy;
        //            resultItem.CreateDate = DateTime.Now;
        //            itemDetail.Add(resultItem);




        //            //}

        //            var itemHeaderlist = new List<TransferStockAdjustmentViewModel>();
        //            itemHeaderlist.Add(itemHeader);

        //            //-- SAVE STORE PROC ----//

        //            DataTable dtHeader = CreateDataTable(itemHeaderlist);
        //            DataTable dtDetail = CreateDataTable(itemDetail);


        //            if (dtHeader.Columns.Contains("listTransferStockAdjustmentItemViewModel"))
        //            {
        //                dtHeader.Columns.Remove("listTransferStockAdjustmentItemViewModel");
        //            }

        //            var TransferStockAdjustment = new SqlParameter("TransferStockAdjustment", SqlDbType.Structured);
        //            TransferStockAdjustment.TypeName = "[dbo].[im_TransferStockAdjustmentData]";
        //            TransferStockAdjustment.Value = dtHeader;


        //            var TransferStockAdjustmentItem = new SqlParameter("TransferStockAdjustmentItem", SqlDbType.Structured);
        //            TransferStockAdjustmentItem.TypeName = "[dbo].[im_TransferStockAdjustmentItemData]";
        //            TransferStockAdjustmentItem.Value = dtDetail;

        //            var transactionAdjustment = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //            try
        //            {
        //                var commandText = "EXEC sp_Save_im_TransferStockAdjustment @TransferStockAdjustment,@TransferStockAdjustmentItem";
        //                var rowsAffected = context.Database.ExecuteSqlCommand(commandText, TransferStockAdjustment, TransferStockAdjustmentItem);
        //                transactionAdjustment.Commit();
        //                IsAdjust = true;

        //            }
        //            catch (Exception exy)
        //            {
        //                transactionAdjustment.Rollback();
        //                msglog = State + " ex Rollback " + exy.Message.ToString();
        //                olog.logging("TransferStockAdjustment", msglog);
        //                throw exy;
        //            }

        //            //---------------------------//

        //            if (data.BinBalanceQtyBal < data.Qty)
        //            {
        //                //GoodsReceiveViewModel GR = new GoodsReceiveViewModel();

        //                String SqlBI = " and  Convert(Nvarchar(200) ,BinBalance_Index ) = N'" + data.BinBalanceIndex + "'  ";
        //                var whereBI = new SqlParameter("@strwhere", SqlBI);
        //                var BI = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere ", whereBI).FirstOrDefault();


        //                String SqlGR = " and  Convert(Nvarchar(200) ,GoodsReceive_Index ) = N'" + BI.GoodsReceive_Index + "'  ";
        //                var whereGR = new SqlParameter("@strwhere", SqlGR);
        //                var GR = context.IM_GoodsReceives.FromSql("sp_GetGoodsReceiveStock @strwhere ", whereGR).FirstOrDefault();

        //                //if (GR.GoodsReceiveIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                //{
        //                //    GR.GoodsReceiveIndex = Guid.NewGuid();
        //                //}

        //                //----Set Header------
        //                var GRHeader = new GoodsReceiveViewModel();
        //                var Documen_tPriorityStatus = 0;
        //                var putaway_Status = 0;
        //                var a = "";


        //                var Goods_ReceiveRemark = "";

        //                GRHeader.GoodsReceiveIndex = pGoodsReceiveIndex;
        //                GRHeader.OwnerIndex = data.OwnerIndex; ;
        //                GRHeader.OwnerId = queryResult.Owner_Id;
        //                GRHeader.OwnerName = queryResult.Owner_Name;
        //                GRHeader.DocumentTypeIndex = new Guid("c3aaf53b-dae8-4f64-9354-c7fd65b04059");

        //                String Sql = " and  Convert(Nvarchar(200) ,DocumentType_Index ) = N'" + GRHeader.DocumentTypeIndex + "'  ";
        //                var GRDocwhere = new SqlParameter("@strwhere", Sql);
        //                var GRDoc = context.MS_DocumentType.FromSql("sp_GetDocumentType @strwhere ", GRDocwhere).FirstOrDefault();

        //                GRHeader.DocumentTypeId = GRDoc.DocumentType_Id;
        //                GRHeader.DocumentTypeName = GRDoc.DocumentType_Name;

        //                //if (GR.GoodsReceiveNo == null)
        //                //{
        //                var DocumentType_Index = new SqlParameter("@DocumentType_Index", GRHeader.DocumentTypeIndex.ToString());
        //                var DocDate = new SqlParameter("@DocDate", DateTime.Now);
        //                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultParameter.Size = 2000; // some meaningfull value
        //                resultParameter.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
        //                //var result = resultParameter.Value;
        //                var GoodsReceiveNo = resultParameter.Value.ToString();
        //                //}

        //                GRHeader.GoodsReceiveNo = GoodsReceiveNo;
        //                GRHeader.GoodsReceiveDate = DateTime.Now;
        //                GRHeader.DocumentRefNo1 = GR.DocumentRef_No1;
        //                GRHeader.DocumentRefNo2 = GR.DocumentRef_No2;
        //                GRHeader.DocumentRefNo3 = GR.DocumentRef_No3;
        //                GRHeader.DocumentRefNo4 = GR.DocumentRef_No4;
        //                GRHeader.DocumentRefNo5 = GR.DocumentRef_No5;
        //                GRHeader.DocumentStatus = 3; // 3 เท่ากับ รับสินค้าเรียบร้อย
        //                GRHeader.UDF1 = data.UDF1;
        //                GRHeader.UDF2 = data.UDF2;
        //                GRHeader.UDF3 = data.UDF3;
        //                GRHeader.UDF4 = data.UDF4;
        //                GRHeader.UDF5 = data.UDF5;
        //                GRHeader.DocumentPriorityStatus = Documen_tPriorityStatus;
        //                GRHeader.DocumentRemark = GR.Document_Remark;
        //                GRHeader.WarehouseIndex = data.WarehouseIndex;
        //                GRHeader.WarehouseId = queryWarehouse.Warehouse_Id;
        //                GRHeader.WarehouseName = queryWarehouse.Warehouse_Name;
        //                GRHeader.WarehouseIndexTo = queryWarehouse.Warehouse_Index;
        //                GRHeader.WarehouseIdTo = queryWarehouse.Warehouse_Id;
        //                GRHeader.WarehouseNameTo = queryWarehouse.Warehouse_Name;
        //                GRHeader.PutawayStatus = putaway_Status;
        //                GRHeader.DockDoorIndex = GR.DockDoor_Index;
        //                GRHeader.DockDoorId = GR.DockDoor_Id;
        //                GRHeader.DockDoorName = GR.DockDoor_Name;
        //                GRHeader.VehicleTypeIndex = GR.VehicleType_Index;
        //                GRHeader.VehicleTypeId = GR.VehicleType_Id;
        //                GRHeader.VehicleTypeName = GR.VehicleType_Name;
        //                GRHeader.ContainerTypeIndex = GR.ContainerType_Index;
        //                GRHeader.ContainerTypeId = GR.ContainerType_Id;
        //                GRHeader.ContainerTypeName = GR.ContainerType_Name;
        //                GRHeader.Create_By = itemHeader.CreateBy;
        //                GRHeader.Create_Date = DateTime.Now;


        //                //----Set Detail-----
        //                var GRDetail = new List<GoodsReceiveItemViewModel>();
        //                int addNumber = 0;
        //                int refDocLineNum = 0;
        //                addNumber++;
        //                var GRItem = new GoodsReceiveItemViewModel();

        //                // Gen Index for line item
        //                //if (data.GoodsReceiveItemIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                //{
        //                //    data.GoodsReceiveItemIndex = Guid.NewGuid();
        //                //}
        //                GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                // Index From Header
        //                GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;
        //                if (data.LineNum == null)
        //                {
        //                    resultItem.LineNum = addNumber.ToString();
        //                }
        //                else
        //                {
        //                    resultItem.LineNum = data.LineNum;
        //                }

        //                GRItem.ProductIndex = queryResult.Product_Index;
        //                GRItem.ProductId = queryResult.Product_Id;
        //                GRItem.ProductName = queryResult.Product_Name;
        //                GRItem.ProductSecondName = queryResult.Product_SecondName;
        //                GRItem.ProductThirdName = queryResult.Product_ThirdName;
        //                if (data.ProductLot != "")
        //                {
        //                    GRItem.ProductLot = queryResult.Product_Lot;
        //                }
        //                else
        //                {
        //                    GRItem.ProductLot = "";
        //                }
        //                GRItem.ItemStatusIndex = queryResult.ItemStatus_Index;
        //                GRItem.ItemStatusId = queryResult.ItemStatus_Id;
        //                GRItem.ItemStatusName = queryResult.ItemStatus_Name;
        //                GRItem.qty = resultItem.Qty;
        //                GRItem.ratio = queryResult.BinBalance_Ratio;

        //                var totalQty = GRItem.qty * queryResult.BinBalance_Ratio;
        //                GRItem.TotalQty = totalQty;

        //                GRItem.UDF1 = data.UDF1;
        //                GRItem.ProductConversionIndex = queryResult.ProductConversion_Index;
        //                GRItem.ProductConversionId = queryResult.ProductConversion_Id;
        //                GRItem.ProductConversionName = queryResult.ProductConversion_Name;
        //                GRItem.MFGDate = queryResult.GoodsReceive_MFG_Date;
        //                GRItem.EXPDate = queryResult.GoodsReceive_EXP_Date;

        //                string SqlProductConversion = " and Convert(Nvarchar(50),ProductConversion_Index) = N'" + queryResult.ProductConversion_Index + "' ";
        //                var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();

        //                if (data.UnitWeight != null)
        //                {
        //                    GRItem.UnitWeight = ProductConversion.ProductConversion_Weight;
        //                }
        //                else
        //                {
        //                    GRItem.UnitWeight = 0;
        //                }

        //                if (data.Weight != null)
        //                {
        //                    GRItem.Weight = data.Weight;
        //                }
        //                else
        //                {
        //                    GRItem.Weight = resultItem.Qty * GRItem.UnitWeight;
        //                }

        //                if (data.UnitWidth != null)
        //                {
        //                    GRItem.UnitWidth = data.UnitWidth;
        //                }
        //                else
        //                {
        //                    GRItem.UnitWidth = 0;
        //                }

        //                if (data.UnitLength != null)
        //                {
        //                    GRItem.UnitLength = data.UnitLength;
        //                }
        //                else
        //                {
        //                    GRItem.UnitLength = 0;
        //                }

        //                if (data.UnitHeight != null)
        //                {
        //                    GRItem.UnitHeight = data.UnitHeight;
        //                }
        //                else
        //                {
        //                    GRItem.UnitHeight = 0;
        //                }

        //                if (data.UnitVolume != null)
        //                {
        //                    GRItem.UnitVolume = data.UnitVolume;
        //                }
        //                else
        //                {
        //                    GRItem.UnitVolume = ProductConversion.ProductConversion_Volume;
        //                }

        //                if (data.Volume != null)
        //                {
        //                    GRItem.Volume = data.Volume;
        //                }
        //                else
        //                {
        //                    GRItem.Volume = resultItem.Qty * GRItem.UnitVolume;
        //                }

        //                if (data.UnitPrice != null)
        //                {
        //                    GRItem.UnitPrice = data.UnitPrice;
        //                }
        //                else
        //                {
        //                    GRItem.UnitPrice = 0;
        //                }

        //                if (data.Price != null)
        //                {
        //                    GRItem.Price = data.Price;
        //                }
        //                else
        //                {
        //                    GRItem.Price = 0;
        //                }

        //                var contextT = new TransferDbContext();

        //                string SqlstockItem = " and Convert(Nvarchar(50),StockAdjustment_Index) = N'" + data.StockAdjustmentIndex + "' ";
        //                var strwherestockItem = new SqlParameter("@strwhere", SqlstockItem);
        //                var stockItem = contextT.im_TransferStockAdjustmentItem.FromSql("sp_GetStockAdjustmentItemItem @strwhere", strwherestockItem).FirstOrDefault();

        //                //var stockItem = contextT.im_TransferStockAdjustmentItem.FromSql("sp_GetStockAdjustmentItemItem").Where(c => c.StockAdjustment_Index == data.StockAdjustmentIndex).FirstOrDefault();

        //                GRItem.RefDocumentNo = data.StockAdjustmentNo;
        //                if (data.RefDocumentLineNum == null)
        //                {
        //                    GRItem.RefDocumentLineNum = refDocLineNum.ToString();
        //                }
        //                else
        //                {
        //                    GRItem.RefDocumentLineNum = data.RefDocumentLineNum;
        //                }
        //                GRItem.RefDocumentIndex = itemHeader.StockAdjustmentIndex;

        //                GRItem.RefProcessIndex = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                GRItem.RefDocumentItemIndex = stockItem.StockAdjustmentItemItem_Index;
        //                GRItem.DocumentRefNo1 = data.DocumentRefNo1;
        //                GRItem.DocumentRefNo2 = data.DocumentRefNo2;
        //                GRItem.DocumentRefNo3 = data.DocumentRefNo3;
        //                GRItem.DocumentRefNo4 = data.DocumentRefNo4;
        //                GRItem.DocumentRefNo5 = data.DocumentRefNo5;
        //                GRItem.DocumentStatus = document_status;
        //                GRItem.UDF1 = data.UDF1;
        //                GRItem.UDF2 = data.UDF2;
        //                GRItem.UDF3 = data.UDF3;
        //                GRItem.UDF4 = data.UDF4;
        //                GRItem.UDF5 = data.UDF5;
        //                GRItem.GoodsReceiveRemark = "";
        //                GRItem.GoodsReceiveDockDoor = "";
        //                GRItem.Create_Date = DateTime.Now;
        //                GRDetail.Add(GRItem);



        //                var GRHeaderlist = new List<GoodsReceiveViewModel>();
        //                GRHeaderlist.Add(GRHeader);


        //                //-- SAVE STORE PROC ----//

        //                DataTable CGRHeader = CreateDataTable(GRHeaderlist);
        //                DataTable CGRDetail = CreateDataTable(GRDetail);


        //                if (CGRHeader.Columns.Contains("listGoodsReceiveItemViewModels"))
        //                {
        //                    CGRHeader.Columns.Remove("listGoodsReceiveItemViewModels");
        //                }

        //                var GoodsReceive = new SqlParameter("GoodsReceive", SqlDbType.Structured);
        //                GoodsReceive.TypeName = "[dbo].[im_GoodsReceiveData]";
        //                GoodsReceive.Value = CGRHeader;


        //                var GoodsReceiveItem = new SqlParameter("GoodsReceiveItem", SqlDbType.Structured);
        //                GoodsReceiveItem.TypeName = "[dbo].[im_GoodsReceiveItemData]";
        //                GoodsReceiveItem.Value = CGRDetail;



        //                var transactionGR = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //                try
        //                {

        //                    var commandText2 = "EXEC sp_Save_im_GoodsReceive @GoodsReceive,@GoodsReceiveItem";
        //                    var rowsAffected2 = context.Database.ExecuteSqlCommand(commandText2, GoodsReceive, GoodsReceiveItem);
        //                    a = rowsAffected2.ToString();


        //                    String Sqlupdate = " Update im_GoodsReceive set " +
        //                                             " Document_Status = 1 " +
        //                                             " where Convert(Varchar(200),GoodsReceive_Index) ='" + GR.GoodsReceive_Index + "'";
        //                    var row = context.Database.ExecuteSqlCommand(Sqlupdate);
        //                    transactionGR.Commit();
        //                    IsGr = true;

        //                }
        //                catch (Exception exy)
        //                {
        //                    transactionGR.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("TransferStockAdjustment", msglog);
        //                    throw exy;
        //                }


        //                //---------------- Create TagItem ----------------//

        //                var listTagItem = new List<TagItemViewModel>();
        //                var TagItem = new TagItemViewModel();

        //                var TagItemIndex = pTagItemIndex;

        //                TagItem.TagItemIndex = TagItemIndex;
        //                TagItem.TagIndex = queryResult.Tag_Index;
        //                TagItem.TagNo = queryResult.Tag_No;
        //                TagItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;
        //                TagItem.GoodsReceiveItemIndex = GRItem.GoodsReceiveItemIndex;
        //                TagItem.ProductIndex = GRItem.ProductIndex;
        //                TagItem.ProductId = GRItem.ProductId;
        //                TagItem.ProductName = GRItem.ProductName;
        //                TagItem.ProductLot = GRItem.ProductLot;
        //                TagItem.ProductSecondName = GRItem.ProductSecondName;
        //                TagItem.ProductThirdName = GRItem.ProductThirdName;
        //                TagItem.ItemStatusIndex = GRItem.ItemStatusIndex;
        //                TagItem.ItemStatusId = GRItem.ItemStatusId;
        //                TagItem.ItemStatusName = GRItem.ItemStatusName;
        //                TagItem.Qty = GRItem.qty;
        //                TagItem.Ratio = GRItem.ratio;
        //                TagItem.TotalQty = GRItem.TotalQty;
        //                TagItem.ProductConversionIndex = GRItem.ProductConversionIndex;
        //                TagItem.ProductConversionId = GRItem.ProductConversionId;
        //                TagItem.ProductConversionName = GRItem.ProductConversionName;
        //                TagItem.Weight = GRItem.Weight;
        //                TagItem.Volume = GRItem.Volume;
        //                TagItem.MFGDate = GRItem.MFGDate;
        //                TagItem.EXPDate = GRItem.EXPDate;
        //                TagItem.TagRefNo1 = "";
        //                TagItem.TagRefNo2 = "";
        //                TagItem.TagRefNo3 = "";
        //                TagItem.TagRefNo4 = "";
        //                TagItem.TagRefNo5 = "";
        //                TagItem.TagStatus = 2;
        //                TagItem.UDF1 = GRItem.UDF1;
        //                TagItem.UDF2 = GRItem.UDF2;
        //                TagItem.UDF3 = GRItem.UDF3;
        //                TagItem.UDF4 = GRItem.UDF4;
        //                TagItem.UDF5 = GRItem.UDF5;
        //                TagItem.CreateBy = itemHeader.CreateBy;
        //                TagItem.CreateDate = DateTime.Today;
        //                listTagItem.Add(TagItem);

        //                DataTable CTag = CreateDataTable(listTagItem);

        //                var ResultsTagItem = new SqlParameter("TagItem", SqlDbType.Structured);
        //                ResultsTagItem.TypeName = "[dbo].[wm_TransferTagItemData]";
        //                ResultsTagItem.Value = CTag;

        //                var transactionTag = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    var commandTextTagItem = "EXEC sp_Save_im_TransferTagItem @TagItem";
        //                    var rowsAffectedTagItem = context.Database.ExecuteSqlCommand(commandTextTagItem, ResultsTagItem);
        //                    transactionTag.Commit();

        //                    IsTag = true;

        //                }
        //                catch (Exception exy)
        //                {
        //                    transactionTag.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("TransferRelocation", msglog);
        //                    throw exy;
        //                }




        //                //-------// GR Location -------//

        //                string pstring = " and BinBalance_Index = N'" + queryResult.BinBalance_Index + "'";
        //                var strwhere = new SqlParameter("@strwhere", pstring);

        //                var queryResults = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();

        //                var listBinBalance = new List<BinBalanceViewModel>();
        //                var listBinCard = new List<BinCardViewModel>();
        //                var GRLocation = new List<GoodsReceiveItemLocationViewModel>();

        //                foreach (var item in queryResults)
        //                {

        //                    var GRLocationResult = new GoodsReceiveItemLocationViewModel();

        //                    var GoodsReceiveItemLocationIndex = Guid.NewGuid();

        //                    GRLocationResult.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                    GRLocationResult.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    GRLocationResult.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocationIndex;
        //                    GRLocationResult.TagItem_Index = TagItem.TagItemIndex;
        //                    GRLocationResult.Tag_Index = TagItem.TagIndex;
        //                    GRLocationResult.Tag_No = TagItem.TagNo;
        //                    GRLocationResult.Product_Index = queryResult.Product_Index;
        //                    GRLocationResult.Product_Name = queryResult.Product_Name;
        //                    GRLocationResult.Product_Id = queryResult.Product_Id;
        //                    GRLocationResult.Product_Name = queryResult.Product_Name;
        //                    GRLocationResult.Product_SecondName = queryResult.Product_SecondName;
        //                    GRLocationResult.Product_ThirdName = queryResult.Product_ThirdName;
        //                    GRLocationResult.Product_Lot = queryResult.Product_Lot;
        //                    GRLocationResult.ItemStatus_Index = queryResult.ItemStatus_Index;
        //                    GRLocationResult.ItemStatus_Id = queryResult.ItemStatus_Id;
        //                    GRLocationResult.ItemStatus_Name = queryResult.ItemStatus_Name;
        //                    GRLocationResult.ProductConversion_Index = queryResult.ProductConversion_Index;
        //                    GRLocationResult.ProductConversion_Id = queryResult.ProductConversion_Id;
        //                    GRLocationResult.ProductConversion_Name = queryResult.ProductConversion_Name;
        //                    GRLocationResult.MFG_Date = queryResult.GoodsReceive_MFG_Date;
        //                    GRLocationResult.EXP_Date = queryResult.GoodsReceive_EXP_Date;
        //                    GRLocationResult.UnitWeight = GRItem.UnitWeight;
        //                    GRLocationResult.Weight = GRItem.Weight;
        //                    GRLocationResult.UnitWidth = GRItem.UnitWidth;
        //                    GRLocationResult.UnitLength = GRItem.UnitLength;
        //                    GRLocationResult.UnitHeight = GRItem.UnitHeight;
        //                    GRLocationResult.UnitVolume = GRItem.UnitVolume;
        //                    GRLocationResult.Volume = GRItem.Volume;
        //                    GRLocationResult.UnitPrice = GRItem.UnitPrice;
        //                    GRLocationResult.Price = GRItem.Price;
        //                    GRLocationResult.Owner_Index = queryResult.Owner_Index;
        //                    GRLocationResult.Owner_Id = queryResult.Owner_Id;
        //                    GRLocationResult.Owner_Name = queryResult.Owner_Name;
        //                    GRLocationResult.Location_Index = queryResult.Location_Index;
        //                    GRLocationResult.Location_Id = queryResult.Location_Id;
        //                    GRLocationResult.Location_Name = queryResult.Location_Name;
        //                    GRLocationResult.Qty = GRItem.qty;
        //                    GRLocationResult.TotalQty = GRLocationResult.Qty * queryResult.BinBalance_Ratio;
        //                    GRLocationResult.Ratio = queryResult.BinBalance_Ratio;
        //                    GRLocationResult.UDF_1 = GRItem.UDF1;
        //                    GRLocationResult.UDF_2 = GRItem.UDF2;
        //                    GRLocationResult.UDF_3 = GRItem.UDF3;
        //                    GRLocationResult.UDF_4 = GRItem.UDF4;
        //                    GRLocationResult.UDF_5 = GRItem.UDF5;
        //                    GRLocationResult.Create_By = itemHeader.CreateBy;
        //                    GRLocationResult.Create_Date = GRItem.Create_Date;
        //                    GRLocationResult.Putaway_Status = 0;
        //                    GRLocationResult.Putaway_By = "";
        //                    GRLocation.Add(GRLocationResult);



        //                    var BinBalance = new BinBalanceViewModel();
        //                    ////--------------------Bin Balance --------------------

        //                    var BinBalance_Index = pBinBalance_Index;
        //                    BinBalance.BinBalance_Index = BinBalance_Index;

        //                    BinBalance.Owner_Index = GRHeader.OwnerIndex;
        //                    BinBalance.Owner_Id = GRHeader.OwnerId;
        //                    BinBalance.Owner_Name = GRHeader.OwnerName;

        //                    BinBalance.LocationIndex = queryResult.Location_Index;
        //                    BinBalance.LocationId = queryResult.Location_Id;
        //                    BinBalance.LocationName = queryResult.Location_Name;

        //                    BinBalance.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                    BinBalance.GoodsReceive_No = GRHeader.GoodsReceiveNo;
        //                    BinBalance.GoodsReceive_Date = GRHeader.GoodsReceiveDate;
        //                    BinBalance.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    BinBalance.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                    BinBalance.TagItem_Index = TagItem.TagItemIndex;
        //                    BinBalance.Tag_Index = item.Tag_Index;
        //                    BinBalance.Tag_No = item.Tag_No;
        //                    BinBalance.Product_Index = item.Product_Index;
        //                    BinBalance.Product_Id = item.Product_Id;
        //                    BinBalance.Product_Name = item.Product_Name;
        //                    BinBalance.Product_SecondName = item.Product_SecondName;
        //                    BinBalance.Product_ThirdName = item.Product_ThirdName;
        //                    BinBalance.Product_Lot = item.Product_Lot;
        //                    BinBalance.ItemStatus_Index = item.ItemStatus_Index;
        //                    BinBalance.ItemStatus_Id = item.ItemStatus_Id;
        //                    BinBalance.ItemStatus_Name = item.ItemStatus_Name;
        //                    BinBalance.GoodsReceive_MFG_Date = item.GoodsReceive_MFG_Date;
        //                    BinBalance.GoodsReceive_EXP_Date = item.GoodsReceive_EXP_Date;
        //                    BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
        //                    BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
        //                    BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
        //                    BinBalance.BinBalance_Ratio = item.BinBalance_Ratio;
        //                    BinBalance.BinBalance_QtyBegin = (data.Qty - data.BinBalanceQtyBal);
        //                    BinBalance.BinBalance_WeightBegin = GRItem.Weight;
        //                    BinBalance.BinBalance_VolumeBegin = GRItem.Volume;
        //                    BinBalance.BinBalance_QtyBal = (data.Qty - data.BinBalanceQtyBal);
        //                    BinBalance.BinBalance_WeightBal = GRItem.Weight;
        //                    BinBalance.BinBalance_VolumeBal = GRItem.Volume;
        //                    BinBalance.BinBalance_QtyReserve = 0;
        //                    BinBalance.BinBalance_WeightReserve = resultItem.Weight;
        //                    BinBalance.BinBalance_VolumeReserve = resultItem.Volume;
        //                    BinBalance.ProductConversion_Index = GRItem.ProductConversionIndex;
        //                    BinBalance.ProductConversion_Id = GRItem.ProductConversionId;
        //                    BinBalance.ProductConversion_Name = GRItem.ProductConversionName;
        //                    BinBalance.UDF_1 = item.UDF_1;
        //                    BinBalance.UDF_2 = item.UDF_2;
        //                    BinBalance.UDF_3 = item.UDF_3;
        //                    BinBalance.UDF_4 = item.UDF_4;
        //                    BinBalance.UDF_5 = item.UDF_5;
        //                    BinBalance.Create_By = itemHeader.CreateBy;
        //                    BinBalance.Create_Date = GRItem.Create_Date;
        //                    BinBalance.Update_By = GRItem.Update_By;
        //                    BinBalance.Update_Date = GRItem.Update_Date;
        //                    BinBalance.Cancel_By = GRItem.Cancel_By;
        //                    BinBalance.Cancel_Date = GRItem.Cancel_Date;


        //                    listBinBalance.Add(BinBalance);

        //                    ////--------------------Bin Card --------------------
        //                    var BinCard = new BinCardViewModel();

        //                    BinCard.BinCard_Index = pBinCard_Index;
        //                    BinCard.Process_Index = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                    // fix DocumentType GR
        //                    BinCard.DocumentType_Index = GRHeader.DocumentTypeIndex;
        //                    BinCard.DocumentType_Id = GRHeader.DocumentTypeId;
        //                    BinCard.DocumentType_Name = GRHeader.DocumentTypeName;
        //                    BinCard.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                    BinCard.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    BinCard.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                    BinCard.BinCard_No = GRHeader.GoodsReceiveNo;
        //                    BinCard.BinCard_Date = GRHeader.GoodsReceiveDate;
        //                    BinCard.TagItem_Index = TagItem.TagItemIndex;
        //                    BinCard.Tag_Index = item.Tag_Index;
        //                    BinCard.Tag_No = item.Tag_No;
        //                    BinCard.Tag_Index_To = item.Tag_Index;
        //                    BinCard.Tag_No_To = item.Tag_No;
        //                    BinCard.Product_Index = GRItem.ProductIndex;
        //                    BinCard.Product_Id = GRItem.ProductId;
        //                    BinCard.Product_Name = GRItem.ProductName;
        //                    BinCard.Product_SecondName = GRItem.ProductSecondName;
        //                    BinCard.Product_ThirdName = GRItem.ProductThirdName;
        //                    BinCard.Product_Index_To = GRItem.ProductIndex;
        //                    BinCard.Product_Id_To = GRItem.ProductId;
        //                    BinCard.Product_Name_To = GRItem.ProductName;
        //                    BinCard.Product_SecondName_To = GRItem.ProductSecondName;
        //                    BinCard.Product_ThirdName_To = GRItem.ProductThirdName;
        //                    BinCard.Product_Lot = GRItem.ProductLot;
        //                    BinCard.Product_Lot_To = GRItem.ProductLot;
        //                    BinCard.ItemStatus_Index = GRItem.ItemStatusIndex;
        //                    BinCard.ItemStatus_Id = GRItem.ItemStatusId;
        //                    BinCard.ItemStatus_Name = GRItem.ItemStatusName;
        //                    BinCard.ItemStatus_Index_To = GRItem.ItemStatusIndex;
        //                    BinCard.ItemStatus_Id_To = GRItem.ItemStatusId;
        //                    BinCard.ItemStatus_Name_To = GRItem.ItemStatusName;
        //                    BinCard.ProductConversion_Index = GRItem.ProductConversionIndex;
        //                    BinCard.ProductConversion_Id = GRItem.ProductConversionId;
        //                    BinCard.ProductConversion_Name = GRItem.ProductConversionName;
        //                    BinCard.Owner_Index = GRHeader.OwnerIndex;
        //                    BinCard.Owner_Id = GRHeader.OwnerId;
        //                    BinCard.Owner_Name = GRHeader.OwnerName;

        //                    BinCard.Owner_Index_To = GRHeader.OwnerIndex;
        //                    BinCard.Owner_Id_To = GRHeader.OwnerId;
        //                    BinCard.Owner_Name_To = GRHeader.OwnerName;

        //                    BinCard.Location_Index = queryResult.Location_Index;
        //                    BinCard.Location_Id = queryResult.Location_Id;
        //                    BinCard.Location_Name = queryResult.Location_Name;

        //                    BinCard.Location_Index_To = queryResult.Location_Index;
        //                    BinCard.Location_Id_To = queryResult.Location_Id;
        //                    BinCard.Location_Name_To = queryResult.Location_Name;

        //                    BinCard.GoodsReceive_EXP_Date = GRItem.EXPDate;
        //                    BinCard.GoodsReceive_EXP_Date_To = GRItem.EXPDate;
        //                    BinCard.BinCard_QtyIn = GRItem.qty;
        //                    BinCard.BinCard_QtyOut = 0;
        //                    BinCard.BinCard_QtySign = GRItem.qty;
        //                    BinCard.BinCard_WeightIn = GRItem.Weight;
        //                    BinCard.BinCard_WeightOut = 0;
        //                    BinCard.BinCard_WeightSign = GRItem.Weight;
        //                    BinCard.BinCard_VolumeIn = GRItem.Volume;
        //                    BinCard.BinCard_VolumeOut = 0;
        //                    BinCard.BinCard_VolumeSign = GRItem.Volume;
        //                    BinCard.Ref_Document_No = GRHeader.GoodsReceiveNo;
        //                    BinCard.Ref_Document_Index = GRHeader.GoodsReceiveIndex;
        //                    BinCard.Ref_DocumentItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    BinCard.Create_By = GRHeader.Create_By;
        //                    BinCard.Create_Date = GRHeader.Create_Date;

        //                    listBinCard.Add(BinCard);
        //                }
        //                DataTable CGRLocation = CreateDataTable(GRLocation);
        //                DataTable dtBinBalance = CreateDataTable(listBinBalance);
        //                DataTable dtBinCard = CreateDataTable(listBinCard);

        //                var GoodsReceiveItemLocation = new SqlParameter("GoodsReceiveItemLocation", SqlDbType.Structured);
        //                GoodsReceiveItemLocation.TypeName = "[dbo].[im_GoodsReceiveItemLocationData]";
        //                GoodsReceiveItemLocation.Value = CGRLocation;

        //                var pBinBalance = new SqlParameter("BinBalance", SqlDbType.Structured);
        //                pBinBalance.TypeName = "[dbo].[wm_BinBalanceData]";
        //                pBinBalance.Value = dtBinBalance;

        //                var pBinCard = new SqlParameter("BinCard", SqlDbType.Structured);
        //                pBinCard.TypeName = "[dbo].[wm_BinCardData]";
        //                pBinCard.Value = dtBinCard;



        //                var transactionBin = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    var rowsAffected3 = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", GoodsReceiveItemLocation, pBinBalance, pBinCard);

        //                    transactionBin.Commit();

        //                    IsBin = true;

        //                }
        //                catch (Exception exy)
        //                {
        //                    transactionBin.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("TransferRelocation", msglog);
        //                    throw exy;
        //                }
        //            }

        //            else
        //            {

        //                GoodIssueViewModel GI = new GoodIssueViewModel();

        //                if (GI.GoodsIssueIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                {
        //                    GI.GoodsIssueIndex = pGoodsIssueIndex;
        //                }

        //                //----Set Header------
        //                var GIHeader = new GoodIssueViewModel();
        //                var Documen_tPriorityStatus = 0;
        //                var putaway_Status = 0;
        //                var a = "";


        //                var Goods_ReceiveRemark = "";

        //                GIHeader.GoodsIssueIndex = GI.GoodsIssueIndex;
        //                GIHeader.OwnerIndex = queryResult.Owner_Index; ;
        //                GIHeader.OwnerId = queryResult.Owner_Id;
        //                GIHeader.OwnerName = queryResult.Owner_Name;
        //                GIHeader.DocumentTypeIndex = new Guid("20f58c63-15e9-47cb-acc8-3896756a94a6");

        //                String Sql = " and  Convert(Nvarchar(200) ,DocumentType_Index ) = N'" + GIHeader.DocumentTypeIndex + "'  ";
        //                var GIDocwhere = new SqlParameter("@strwhere", Sql);
        //                var GIDoc = context.MS_DocumentType.FromSql("sp_GetDocumentType @strwhere ", GIDocwhere).FirstOrDefault();

        //                GIHeader.DocumentTypeId = GIDoc.DocumentType_Id;
        //                GIHeader.DocumentTypeName = GIDoc.DocumentType_Name;

        //                if (GI.GoodsIssueNo == null)
        //                {
        //                    var DocumentType_Index = new SqlParameter("@DocumentType_Index", GIHeader.DocumentTypeIndex.ToString());
        //                    var DocDate = new SqlParameter("@DocDate", DateTime.Now);
        //                    var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                    resultParameter.Size = 2000; // some meaningfull value
        //                    resultParameter.Direction = ParameterDirection.Output;
        //                    context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
        //                    GI.GoodsIssueNo = resultParameter.Value.ToString();
        //                }

        //                GIHeader.GoodsIssueNo = GI.GoodsIssueNo;
        //                GIHeader.GoodsIssueDate = DateTime.Now;
        //                GIHeader.DocumentRefNo1 = GI.DocumentRefNo1;
        //                GIHeader.DocumentRefNo2 = GI.DocumentRefNo2;
        //                GIHeader.DocumentRefNo3 = GI.DocumentRefNo3;
        //                GIHeader.DocumentRefNo4 = GI.DocumentRefNo4;
        //                GIHeader.DocumentRefNo5 = GI.DocumentRefNo5;
        //                GIHeader.DocumentStatus = 6; // 6 เท่ากับ เบิก เสร็จสิ้น
        //                GIHeader.UDF1 = data.UDF1;
        //                GIHeader.UDF2 = data.UDF2;
        //                GIHeader.UDF3 = data.UDF3;
        //                GIHeader.UDF4 = data.UDF4;
        //                GIHeader.UDF5 = data.UDF5;
        //                GIHeader.DocumentPriorityStatus = Documen_tPriorityStatus;
        //                GIHeader.DocumentRemark = GI.DocumentRemark;
        //                GIHeader.CreateBy = itemHeader.CreateBy;
        //                GIHeader.CreateDate = DateTime.Now;
        //                GIHeader.WarehouseIndex = data.WarehouseIndex;
        //                GIHeader.WarehouseId = queryWarehouse.Warehouse_Id;
        //                GIHeader.WarehouseName = queryWarehouse.Warehouse_Name;

        //                decimal ratio = 1;

        //                //----Set Detail-----
        //                var GIDetail = new List<GoodIssueViewModelItem>();
        //                int addNumber = 0;
        //                int refDocLineNum = 0;

        //                addNumber++;
        //                var GIItem = new GoodIssueViewModelItem();

        //                // Gen Index for line item
        //                var GoodsIssueItemIndex = Guid.NewGuid();
        //                GIItem.GoodsIssueItemIndex = GoodsIssueItemIndex;

        //                // Index From Header
        //                GIItem.GoodsIssueIndex = GI.GoodsIssueIndex;
        //                if (data.LineNum == null)
        //                {
        //                    resultItem.LineNum = addNumber.ToString();
        //                }
        //                else
        //                {
        //                    resultItem.LineNum = data.LineNum;
        //                }

        //                GIItem.ProductIndex = queryResult.Product_Index;
        //                GIItem.ProductId = queryResult.Product_Id;
        //                GIItem.ProductName = queryResult.Product_Name;
        //                GIItem.ProductSecondName = queryResult.Product_SecondName;
        //                GIItem.ProductThirdName = queryResult.Product_ThirdName;
        //                if (data.ProductLot != "")
        //                {
        //                    GIItem.ProductLot = queryResult.Product_Lot;
        //                }
        //                else
        //                {
        //                    GIItem.ProductLot = "";
        //                }
        //                //string SqlGLI = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + queryResult.GoodsReceiveItem_Index + "' ";
        //                //var strwhereGLI = new SqlParameter("@strwhere", SqlGLI);
        //                //var GLI = context.IM_GoodsIssueItemLocation.FromSql("sp_GetGoodsIssueItemLocation @strwhere", strwhereGLI).FirstOrDefault();

        //                string SqlGEI = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + queryResult.GoodsReceiveItem_Index + "' ";
        //                var strwhereGEI = new SqlParameter("@strwhere", SqlGEI);
        //                var GEI = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhereGEI).FirstOrDefault();

        //                GIItem.ItemStatusIndex = GEI.ItemStatus_Index;
        //                GIItem.ItemStatusId = GEI.ItemStatus_Id;
        //                GIItem.ItemStatusName = GEI.ItemStatus_Name;

        //                //GIItem.Qty = resultItem.Qty;
        //                //GIItem.Ratio = data.ratio;
        //                //GIItem.QtyPlan = resultItem.Qty;

        //                //var totalQty = GIItem.Qty * queryResult.BinBalance_Ratio;
        //                // GIItem.TotalQty = totalQty;

        //                GIItem.Qty = resultItem.Qty / ratio;
        //                GIItem.Ratio = ratio;
        //                GIItem.QtyPlan = resultItem.Qty / ratio;

        //                var totalQty = resultItem.TotalQty;

        //                GIItem.TotalQty = resultItem.TotalQty;


        //                GIItem.UDF1 = data.UDF1;
        //                GIItem.ProductConversionIndex = queryResult.ProductConversion_Index;
        //                GIItem.ProductConversionId = queryResult.ProductConversion_Id;
        //                GIItem.ProductConversionName = queryResult.ProductConversion_Name;
        //                GIItem.MFGDate = queryResult.GoodsReceive_MFG_Date;
        //                GIItem.EXPDate = queryResult.GoodsReceive_EXP_Date;
        //                GIItem.GoodsReceiveItemIndex = queryResult.GoodsReceiveItem_Index;

        //                string SqlProductConversion = " and Convert(Nvarchar(50),ProductConversion_Index) = N'" + queryResult.ProductConversion_Index + "' ";
        //                var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();

        //                if (data.UnitWeight != null)
        //                {
        //                    GIItem.UnitWeight = data.UnitWeight;
        //                }
        //                else
        //                {
        //                    GIItem.UnitWeight = ProductConversion.ProductConversion_Weight;
        //                }

        //                if (data.Weight != null)
        //                {
        //                    GIItem.Weight = data.Weight;
        //                }
        //                else
        //                {
        //                    GIItem.Weight = resultItem.Qty * GIItem.UnitWeight;
        //                }

        //                if (data.UnitWidth != null)
        //                {
        //                    GIItem.UnitWidth = data.UnitWidth;
        //                }
        //                else
        //                {
        //                    GIItem.UnitWidth = 0;
        //                }

        //                if (data.UnitLength != null)
        //                {
        //                    GIItem.UnitLength = data.UnitLength;
        //                }
        //                else
        //                {
        //                    GIItem.UnitLength = 0;
        //                }

        //                if (data.UnitHeight != null)
        //                {
        //                    GIItem.UnitHeight = data.UnitHeight;
        //                }
        //                else
        //                {
        //                    GIItem.UnitHeight = 0;
        //                }

        //                if (data.UnitVolume != null)
        //                {
        //                    GIItem.UnitVolume = data.UnitVolume;
        //                }
        //                else
        //                {
        //                    GIItem.UnitVolume = ProductConversion.ProductConversion_Volume;
        //                }

        //                if (data.Volume != null)
        //                {
        //                    GIItem.Volume = data.Volume;
        //                }
        //                else
        //                {
        //                    GIItem.Volume = resultItem.Qty * GIItem.UnitVolume;
        //                }

        //                if (data.UnitPrice != null)
        //                {
        //                    GIItem.UnitPrice = data.UnitPrice;
        //                }
        //                else
        //                {
        //                    GIItem.UnitPrice = 0;
        //                }

        //                if (data.Price != null)
        //                {
        //                    GIItem.Price = data.Price;
        //                }
        //                else
        //                {
        //                    GIItem.Price = 0;
        //                }

        //                string SqlstockItem = " and Convert(Nvarchar(50),StockAdjustment_Index) = N'" + data.StockAdjustmentIndex + "' ";
        //                var strwherestockItem = new SqlParameter("@strwhere", SqlstockItem);
        //                var stockItem = context.im_TransferStockAdjustmentItem.FromSql("sp_GetStockAdjustmentItemItem @strwhere", strwherestockItem).FirstOrDefault();
        //                //var stockItem = context.im_TransferStockAdjustmentItem.FromSql("sp_GetStockAdjustmentItemItem").Where(c => c.StockAdjustment_Index == data.StockAdjustmentIndex).FirstOrDefault();

        //                GIItem.RefDocumentNo = data.StockAdjustmentNo;
        //                if (data.RefDocumentLineNum == null)
        //                {
        //                    GIItem.RefDocumentLineNum = refDocLineNum.ToString();
        //                }
        //                else
        //                {
        //                    GIItem.RefDocumentLineNum = data.RefDocumentLineNum;
        //                }

        //                GIItem.RefProcessIndex = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                GIItem.RefDocumentIndex = itemHeader.StockAdjustmentIndex;
        //                GIItem.RefDocumentItemIndex = stockItem.StockAdjustmentItemItem_Index;

        //                GIItem.DocumentRefNo1 = data.DocumentRefNo1;
        //                GIItem.DocumentRefNo2 = data.DocumentRefNo2;
        //                GIItem.DocumentRefNo3 = data.DocumentRefNo3;
        //                GIItem.DocumentRefNo4 = data.DocumentRefNo4;
        //                GIItem.DocumentRefNo5 = data.DocumentRefNo5;
        //                GIItem.DocumentStatus = document_status;
        //                GIItem.UDF1 = data.UDF1;
        //                GIItem.UDF2 = data.UDF2;
        //                GIItem.UDF3 = data.UDF3;
        //                GIItem.UDF4 = data.UDF4;
        //                GIItem.UDF5 = data.UDF5;
        //                GIItem.CreateDate = DateTime.Now;
        //                GIDetail.Add(GIItem);


        //                //SET GI Location 

        //                var GILocation = new List<GoodsIssueItemLocationModel>();

        //                var GILocationResult = new GoodsIssueItemLocationModel();

        //                GILocationResult.GoodsIssue_Index = GIHeader.GoodsIssueIndex;
        //                GILocationResult.GoodsIssueItem_Index = GIItem.GoodsIssueItemIndex;

        //                var GoodsIssueItemLocation_Index = Guid.NewGuid();

        //                GILocationResult.GoodsIssueItemLocation_Index = GoodsIssueItemLocation_Index;
        //                GILocationResult.TagItem_Index = queryResult.TagItem_Index;
        //                GILocationResult.Tag_Index = queryResult.Tag_Index;
        //                GILocationResult.Tag_No = queryResult.Tag_No;
        //                GILocationResult.Product_Index = queryResult.Product_Index;
        //                GILocationResult.Product_Id = queryResult.Product_Id;
        //                GILocationResult.Product_Name = queryResult.Product_Name;
        //                GILocationResult.Product_SecondName = queryResult.Product_SecondName;
        //                GILocationResult.Product_ThirdName = queryResult.Product_ThirdName;
        //                GILocationResult.Product_Lot = queryResult.Product_Lot;
        //                GILocationResult.ItemStatus_Index = queryResult.ItemStatus_Index;
        //                GILocationResult.ItemStatus_Id = queryResult.ItemStatus_Id;
        //                GILocationResult.ItemStatus_Name = queryResult.ItemStatus_Name;
        //                GILocationResult.Location_Index = queryResult.Location_Index;
        //                GILocationResult.Location_Id = queryResult.Location_Id;
        //                GILocationResult.Location_Name = queryResult.Location_Name;
        //                GILocationResult.Qty = (Decimal)GIItem.Qty;
        //                GILocationResult.Ratio = (Decimal)GIItem.Ratio;//queryResult.BinBalance_Ratio;
        //                GILocationResult.TotalQty = (Decimal)GIItem.TotalQty; // queryResult.BinBalance_QtyBal;
        //                GILocationResult.ProductConversion_Index = (Guid)queryResult.ProductConversion_Index;
        //                GILocationResult.ProductConversion_Id = queryResult.ProductConversion_Id;
        //                GILocationResult.ProductConversion_Name = queryResult.ProductConversion_Name;
        //                GILocationResult.MFG_Date = queryResult.GoodsReceive_MFG_Date;
        //                GILocationResult.EXP_Date = queryResult.GoodsReceive_EXP_Date;
        //                GILocationResult.Weight = (Decimal)GIItem.Weight;//queryResult.BinBalance_WeightBal;
        //                GILocationResult.Volume = (Decimal)GIItem.Volume; //queryResult.BinBalance_VolumeBal;
        //                GILocationResult.DocumentRef_No1 = itemHeader.DocumentRefNo1;
        //                GILocationResult.DocumentRef_No2 = itemHeader.DocumentRefNo2;
        //                GILocationResult.DocumentRef_No3 = itemHeader.DocumentRefNo3;
        //                GILocationResult.DocumentRef_No4 = itemHeader.DocumentRefNo4;
        //                GILocationResult.DocumentRef_No5 = itemHeader.DocumentRefNo5;
        //                GILocationResult.Document_Status = itemHeader.DocumentStatus;
        //                GILocationResult.UDF_1 = itemHeader.UDF1;
        //                GILocationResult.UDF_2 = itemHeader.UDF2;
        //                GILocationResult.UDF_3 = itemHeader.UDF3;
        //                GILocationResult.UDF_4 = itemHeader.UDF4;
        //                GILocationResult.UDF_5 = itemHeader.UDF5;
        //                GILocationResult.Ref_Process_Index = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                GILocationResult.Ref_Document_No = itemHeader.StockAdjustmentNo;
        //                GILocationResult.Ref_Document_Index = (Guid)itemHeader.StockAdjustmentIndex;
        //                GILocationResult.Ref_DocumentItem_Index = resultItem.StockAdjustmentItemItemIndex;
        //                GILocationResult.GoodsReceiveItem_Index = queryResult.GoodsReceiveItem_Index;
        //                GILocationResult.Create_By = queryResult.Create_By;

        //                GILocation.Add(GILocationResult);

        //                var GIHeaderlist = new List<GoodIssueViewModel>();
        //                GIHeaderlist.Add(GIHeader);

        //                //----------------พี่ก้อยบอกว่า Adjust ไม่ต้องลง----------------//

        //                //---------------- พี่คิวบอกว่า ต้องลง 02/08/2019 ----------------//

        //                // Add BinCardReserve

        //                var ListBinCardReserve = new List<BinCardReserveModel>();

        //                var BinCardReserve = new BinCardReserveModel();

        //                //var BinCardReserve_Index = Guid.NewGuid();
        //                BinCardReserve.BinCardReserve_Index = pBinCardRe_Index;
        //                BinCardReserve.BinBalance_Index = queryResult.BinBalance_Index;
        //                BinCardReserve.Process_Index = new Guid("19CCA9B3-B4D2-4BC5-87F7-DE64C1E75CBF");  // GI Process
        //                BinCardReserve.GoodsReceive_Index = queryResult.GoodsReceive_Index;
        //                BinCardReserve.GoodsReceive_No = queryResult.GoodsReceive_No;
        //                BinCardReserve.GoodsReceive_Date = queryResult.GoodsReceive_Date;
        //                BinCardReserve.GoodsReceiveItem_Index = queryResult.GoodsReceiveItem_Index;
        //                BinCardReserve.TagItem_Index = queryResult.TagItem_Index;
        //                BinCardReserve.Tag_Index = queryResult.Tag_Index;
        //                BinCardReserve.Tag_No = queryResult.Tag_No;
        //                BinCardReserve.Product_Index = queryResult.Product_Index;
        //                BinCardReserve.Product_Id = queryResult.Product_Id;
        //                BinCardReserve.Product_Name = queryResult.Product_Name;
        //                BinCardReserve.Product_SecondName = queryResult.Product_SecondName;
        //                BinCardReserve.Product_ThirdName = queryResult.Product_ThirdName;
        //                BinCardReserve.Product_Lot = queryResult.Product_Lot;
        //                BinCardReserve.ItemStatus_Index = queryResult.ItemStatus_Index;
        //                BinCardReserve.ItemStatus_Id = queryResult.ItemStatus_Id;
        //                BinCardReserve.ItemStatus_Name = queryResult.ItemStatus_Name;
        //                BinCardReserve.MFG_Date = queryResult.GoodsReceive_MFG_Date;
        //                BinCardReserve.EXP_Date = queryResult.GoodsReceive_EXP_Date;
        //                BinCardReserve.ProductConversion_Index = queryResult.ProductConversion_Index;
        //                BinCardReserve.ProductConversion_Id = queryResult.ProductConversion_Id;
        //                BinCardReserve.ProductConversion_Name = queryResult.ProductConversion_Name;
        //                BinCardReserve.Owner_Index = queryResult.Owner_Index;
        //                BinCardReserve.Owner_Id = queryResult.Owner_Id;
        //                BinCardReserve.Owner_Name = queryResult.Owner_Name;
        //                BinCardReserve.Location_Index = queryResult.Location_Index;
        //                BinCardReserve.Location_Id = queryResult.Location_Id;
        //                BinCardReserve.Location_Name = queryResult.Location_Name;
        //                BinCardReserve.BinCardReserve_QtyBal = GILocationResult.TotalQty;
        //                BinCardReserve.BinCardReserve_WeightBal = GILocationResult.Weight;
        //                BinCardReserve.BinCardReserve_VolumeBal = GILocationResult.Volume;
        //                BinCardReserve.Ref_Document_No = GIHeader.GoodsIssueNo;
        //                BinCardReserve.Ref_Document_Index = GIHeader.GoodsIssueIndex;
        //                BinCardReserve.Ref_DocumentItem_Index = GILocationResult.GoodsIssueItemLocation_Index;
        //                BinCardReserve.Create_By = itemHeader.CreateBy;
        //                ListBinCardReserve.Add(BinCardReserve);



        //                DataTable dtBinCardReserve = CreateDataTable(ListBinCardReserve);

        //                var pBinCardReserve = new SqlParameter("BinCardReserve", SqlDbType.Structured);
        //                pBinCardReserve.TypeName = "[dbo].[wm_BinCardReserveData]";
        //                pBinCardReserve.Value = dtBinCardReserve;

        //                var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //                try
        //                {
        //                    var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", GILocationResult.TotalQty);
        //                    var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", GILocationResult.Weight);
        //                    var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", GILocationResult.Volume);
        //                    var BinBalance_Index = new SqlParameter("@BinBalance_Index", queryResult.BinBalance_Index);
        //                    String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                             "     BinBalance_QtyReserve  = BinBalance_QtyReserve +  @BinBalance_QtyReserve " +
        //                                             "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve +  @BinBalance_WeightReserve " +
        //                                             "  ,BinBalance_VolumeReserve  = BinBalance_VolumeReserve+  @BinBalance_VolumeReserve " +
        //                                             " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);



        //                    IsReserve = true;
        //                    //SqlClearReserve
        //                    String SqlcmdClearReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                             "     BinBalance_QtyReserve  = BinBalance_QtyReserve - " + GILocationResult.TotalQty + "" +
        //                                             "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve - " + GILocationResult.Weight + "" +
        //                                             "  ,BinBalance_VolumeReserve  = BinBalance_VolumeReserve - " + GILocationResult.Volume + "" +
        //                                             " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";

        //                    SqlClearReserve.Add(SqlcmdClearReserve);

        //                    var BinCardReserves = "EXEC sp_Save_BinCardReserve @BinCardReserve";
        //                    var rowsBinCardReserveAffected = context.Database.ExecuteSqlCommand(BinCardReserves, pBinCardReserve);

        //                    transaction.Commit();

        //                }

        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    throw ex;
        //                }

                       

        //                //-- SAVE STORE PROC ----//

        //                DataTable CGIHeader = CreateDataTable(GIHeaderlist);
        //                DataTable CGIDetail = CreateDataTable(GIDetail);
        //                DataTable CGILocation = CreateDataTable(GILocation);


        //                var GoodsIssue = new SqlParameter("GoodsIssue", SqlDbType.Structured);
        //                GoodsIssue.TypeName = "[dbo].[im_GoodsIssueData]";
        //                GoodsIssue.Value = CGIHeader;


        //                var GoodsIssueItem = new SqlParameter("GoodsIssueItem", SqlDbType.Structured);
        //                GoodsIssueItem.TypeName = "[dbo].[im_GoodsIssueItemData]";
        //                GoodsIssueItem.Value = CGIDetail;

        //                var GoodsIssueItemLocation = new SqlParameter("GoodsIssueItemLocation", SqlDbType.Structured);
        //                GoodsIssueItemLocation.TypeName = "[dbo].[im_GoodsIssueItemLocationData]";
        //                GoodsIssueItemLocation.Value = CGILocation;

        //                var transactionGi = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //                try
        //                {
        //                    var commandText2 = "EXEC sp_Save_im_GoodsIssue @GoodsIssue,@GoodsIssueItem,@GoodsIssueItemLocation";
        //                    var rowsAffected2 = context.Database.ExecuteSqlCommand(commandText2, GoodsIssue, GoodsIssueItem, GoodsIssueItemLocation);
        //                    a = rowsAffected2.ToString();
        //                    transactionGi.Commit();

        //                    IsGi = true;

        //                }
        //                catch (Exception exy)
        //                {
        //                    transactionGi.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("TransferAdjust", msglog);
        //                    throw exy;
        //                }


        //                string pstring = " and BinBalance_Index = N'" + queryResult.BinBalance_Index + "'";
        //                var strwhere = new SqlParameter("@strwhere", pstring);

        //                var queryResults = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();

        //                var listBinBalance = new List<BinBalanceViewModel>();
        //                var listBinCard = new List<BinCardViewModel>();
        //                foreach (var item in queryResults)
        //                {
        //                    string SqlGRItem = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + queryResult.GoodsReceiveItem_Index.ToString() + "' ";
        //                    var strwhereGRItem = new SqlParameter("@strwhere", SqlGRItem);
        //                    var queryGRItem = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhereGRItem).FirstOrDefault();

        //                    //var queryGRItem = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem").Where(c => c.GoodsReceiveItem_Index == queryResult.GoodsReceiveItem_Index).FirstOrDefault();


        //                    string SqlGR = " and Convert(Nvarchar(50),GoodsReceive_Index) = N'" + queryGRItem.GoodsReceive_Index + "' ";
        //                    var strwhereGR = new SqlParameter("@strwhere", SqlGR);
        //                    var queryGR = context.IM_GoodsReceives.FromSql("sp_GetGoodsReceiveStock @strwhere", strwhereGR).FirstOrDefault();

        //                    //var queryGR = context.IM_GoodsReceives.FromSql("sp_GetGoodsReceive").Where(c => c.GoodsReceive_Index == queryGRItem.GoodsReceive_Index).FirstOrDefault();

        //                    string SqlGRLocation = " and Convert(Nvarchar(50),TagItem_Index) = N'" + queryResult.TagItem_Index + "' ";
        //                    var strwhereGRLocation = new SqlParameter("@strwhere", SqlGRLocation);
        //                    var queryGRLocation = context.IM_GoodsReceiveItemLocation.FromSql("sp_GetGoodsReceiveItemLocation @strwhere", strwhereGRLocation).FirstOrDefault();

        //                    //var queryGRLocation = context.IM_GoodsReceiveItemLocation.FromSql("sp_GetGoodsReceiveItemLocation").Where(c => c.TagItem_Index == queryResult.TagItem_Index).FirstOrDefault();


        //                    //       var BinBalance = new BinBalanceViewModel();
        //                    ////--------------------Bin Balance --------------------

        //                    //      var BinIndex = Guid.NewGuid();


        //                    //BinBalance.BinBalance_Index = BinIndex;

        //                    //BinBalance.Owner_Index = queryResult.Owner_Index;
        //                    //BinBalance.Owner_Id = queryResult.Owner_Id;
        //                    //BinBalance.Owner_Name = queryResult.Owner_Name;

        //                    //BinBalance.LocationIndex = queryResult.Location_Index;
        //                    //BinBalance.LocationId = queryResult.Location_Id;
        //                    //BinBalance.LocationName = queryResult.Location_Name;


        //                    //BinBalance.GoodsReceive_Index = queryGRItem.GoodsReceive_Index;
        //                    //BinBalance.GoodsReceive_No = queryGR.GoodsReceive_No;
        //                    //BinBalance.GoodsReceive_Date = queryGR.GoodsReceive_Date;
        //                    //BinBalance.GoodsReceiveItem_Index = queryGRItem.GoodsReceiveItem_Index;
        //                    //BinBalance.GoodsReceiveItemLocation_Index = queryGRLocation.GoodsReceiveItemLocation_Index;
        //                    //BinBalance.TagItem_Index = item.TagItem_Index;
        //                    //BinBalance.Tag_Index = new Guid(item.Tag_Index.ToString());
        //                    //BinBalance.Tag_No = item.Tag_No;
        //                    //BinBalance.Product_Index = item.Product_Index;
        //                    //BinBalance.Product_Id = item.Product_Id;
        //                    //BinBalance.Product_Name = item.Product_Name;
        //                    //BinBalance.Product_SecondName = item.Product_SecondName;
        //                    //BinBalance.Product_ThirdName = item.Product_ThirdName;
        //                    //BinBalance.Product_Lot = item.Product_Lot;
        //                    //BinBalance.ItemStatus_Index = item.ItemStatus_Index;
        //                    //BinBalance.ItemStatus_Id = item.ItemStatus_Id;
        //                    //BinBalance.ItemStatus_Name = item.ItemStatus_Name;
        //                    //BinBalance.GoodsReceive_MFG_Date = item.GoodsReceive_MFG_Date;
        //                    //BinBalance.GoodsReceive_EXP_Date = item.GoodsReceive_EXP_Date;
        //                    //BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
        //                    //BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
        //                    //BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
        //                    //BinBalance.BinBalance_Ratio = item.BinBalance_Ratio;
        //                    //BinBalance.BinBalance_QtyBegin = resultItem.Qty;
        //                    //BinBalance.BinBalance_WeightBegin = GIItem.Weight;
        //                    //BinBalance.BinBalance_VolumeBegin = GIItem.Volume;
        //                    //BinBalance.BinBalance_QtyBal = resultItem.Qty;
        //                    //BinBalance.BinBalance_WeightBal = GIItem.Weight;
        //                    //BinBalance.BinBalance_VolumeBal = GIItem.Volume;
        //                    //BinBalance.BinBalance_QtyReserve = 0;
        //                    //BinBalance.BinBalance_WeightReserve = 0;
        //                    //BinBalance.BinBalance_VolumeReserve = 0;
        //                    //BinBalance.ProductConversion_Index = GIItem.ProductConversionIndex;
        //                    //BinBalance.ProductConversion_Id = GIItem.ProductConversionId;
        //                    //BinBalance.ProductConversion_Name = GIItem.ProductConversionName;
        //                    //BinBalance.UDF_1 = GIItem.UDF1;
        //                    //BinBalance.UDF_2 = GIItem.UDF2;
        //                    //BinBalance.UDF_3 = GIItem.UDF3;
        //                    //BinBalance.UDF_4 = GIItem.UDF4;
        //                    //BinBalance.UDF_5 = GIItem.UDF5;
        //                    //BinBalance.Create_By = itemHeader.CreateBy;
        //                    //BinBalance.Create_Date = GIItem.CreateDate;
        //                    //BinBalance.Update_By = GIItem.UpdateBy;
        //                    //BinBalance.Update_Date = GIItem.UpdateDate;
        //                    //BinBalance.Cancel_By = GIItem.CancelBy;
        //                    //BinBalance.Cancel_Date = GIItem.CancelDate;


        //                    //listBinBalance.Add(BinBalance);


        //                    ////--------------------Bin Card --------------------
        //                    var BinCard = new BinCardViewModel();

        //                    BinCard.BinCard_Index = pBinCard_Index;
        //                    BinCard.Process_Index = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");

        //                    //ตอนนี้ DocumentType fix เป็นของGI อยู่
        //                    BinCard.DocumentType_Index = GIHeader.DocumentTypeIndex;
        //                    BinCard.DocumentType_Id = GIHeader.DocumentTypeId;
        //                    BinCard.DocumentType_Name = GIHeader.DocumentTypeName;
        //                    BinCard.GoodsReceive_Index = queryGR.GoodsReceive_Index;
        //                    BinCard.GoodsReceiveItem_Index = queryGRItem.GoodsReceiveItem_Index;
        //                    BinCard.GoodsReceiveItemLocation_Index = queryGRLocation.GoodsReceiveItemLocation_Index;
        //                    BinCard.BinCard_No = GI.GoodsIssueNo;
        //                    BinCard.BinCard_Date = DateTime.Now; ;
        //                    BinCard.TagItem_Index = item.TagItem_Index;
        //                    BinCard.Tag_Index = item.Tag_Index;
        //                    BinCard.Tag_No = item.Tag_No;
        //                    BinCard.Tag_Index_To = item.TagItem_Index;
        //                    BinCard.Tag_No_To = item.Tag_No;
        //                    BinCard.Product_Index = GIItem.ProductIndex;
        //                    BinCard.Product_Id = GIItem.ProductId;
        //                    BinCard.Product_Name = GIItem.ProductName;
        //                    BinCard.Product_SecondName = GIItem.ProductSecondName;
        //                    BinCard.Product_ThirdName = GIItem.ProductThirdName;
        //                    BinCard.Product_Index_To = GIItem.ProductIndex;
        //                    BinCard.Product_Id_To = GIItem.ProductId;
        //                    BinCard.Product_Name_To = GIItem.ProductName;
        //                    BinCard.Product_SecondName_To = GIItem.ProductSecondName;
        //                    BinCard.Product_ThirdName_To = GIItem.ProductThirdName;
        //                    BinCard.Product_Lot = GIItem.ProductLot;
        //                    BinCard.Product_Lot_To = GIItem.ProductLot;
        //                    BinCard.ItemStatus_Index = GIItem.ItemStatusIndex;
        //                    BinCard.ItemStatus_Id = GIItem.ItemStatusId;
        //                    BinCard.ItemStatus_Name = GIItem.ItemStatusName;
        //                    BinCard.ItemStatus_Index_To = GIItem.ItemStatusIndex;
        //                    BinCard.ItemStatus_Id_To = GIItem.ItemStatusId;
        //                    BinCard.ItemStatus_Name_To = GIItem.ItemStatusName;
        //                    BinCard.ProductConversion_Index = GIItem.ProductConversionIndex;
        //                    BinCard.ProductConversion_Id = GIItem.ProductConversionId;
        //                    BinCard.ProductConversion_Name = GIItem.ProductConversionName;
        //                    BinCard.Owner_Index = queryResult.Owner_Index;
        //                    BinCard.Owner_Id = queryResult.Owner_Id;
        //                    BinCard.Owner_Name = queryResult.Owner_Name;

        //                    BinCard.Owner_Index_To = queryResult.Owner_Index;
        //                    BinCard.Owner_Id_To = queryResult.Owner_Id;
        //                    BinCard.Owner_Name_To = queryResult.Owner_Name;

        //                    BinCard.Location_Index = queryResult.Location_Index;
        //                    BinCard.Location_Id = queryResult.Location_Id;
        //                    BinCard.Location_Name = queryResult.Location_Name;

        //                    BinCard.Location_Index_To = queryResult.Location_Index;
        //                    BinCard.Location_Id_To = queryResult.Location_Id;
        //                    BinCard.Location_Name_To = queryResult.Location_Name;

        //                    BinCard.GoodsReceive_EXP_Date = GIItem.EXPDate;
        //                    BinCard.GoodsReceive_EXP_Date_To = GIItem.EXPDate;
        //                    BinCard.BinCard_QtyIn = 0;
        //                    BinCard.BinCard_QtyOut = GIItem.TotalQty;
        //                    BinCard.BinCard_QtySign = GIItem.TotalQty * -1;
        //                    BinCard.BinCard_WeightIn = 0;
        //                    BinCard.BinCard_WeightOut = GIItem.Weight;
        //                    BinCard.BinCard_WeightSign = GIItem.Weight;
        //                    BinCard.BinCard_VolumeIn = 0;
        //                    BinCard.BinCard_VolumeOut = GIItem.Volume;
        //                    BinCard.BinCard_VolumeSign = GIItem.Volume;
        //                    BinCard.Ref_Document_No = GI.GoodsIssueNo;
        //                    BinCard.Ref_Document_Index = GI.GoodsIssueIndex;
        //                    BinCard.Ref_DocumentItem_Index = GIItem.GoodsIssueItemIndex;
        //                    BinCard.Create_By = itemHeader.CreateBy;
        //                    BinCard.Create_Date = GIItem.CreateDate;

        //                    listBinCard.Add(BinCard);
        //                }

        //                DataTable dtBinBalance = CreateDataTable(listBinBalance);
        //                DataTable dtBinCard = CreateDataTable(listBinCard);

        //                var pBinBalance = new SqlParameter("BinBalance", SqlDbType.Structured);
        //                pBinBalance.TypeName = "[dbo].[wm_BinBalanceData]";
        //                pBinBalance.Value = dtBinBalance;

        //                var pBinCard = new SqlParameter("BinCard", SqlDbType.Structured);
        //                pBinCard.TypeName = "[dbo].[wm_BinCardData]";
        //                pBinCard.Value = dtBinCard;

        //                var transactionBinCard = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    var rowsAffected3 = context.Database.ExecuteSqlCommand("sp_Save_BinBalance @BinBalance,@BinCard", pBinBalance, pBinCard);
        //                    transactionBinCard.Commit();

        //                    IsBin = true;

        //                }
        //                catch (Exception exy)
        //                {
        //                    transactionBinCard.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("TransferAdjust", msglog);
        //                    throw exy;
        //                }


        //                // Open Transaction
        //                var transaction2 = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    // Update QtyReserve In BinCardReserve

        //                    var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", GIItem.TotalQty);
        //                    var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", GIItem.Weight);
        //                    var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", GIItem.Volume);
        //                    var BinBalance_Index = new SqlParameter("@BinBalance_Index", queryResult.BinBalance_Index);

        //                    var UnitWeight = queryResult.BinBalance_WeightBal / queryResult.BinBalance_QtyBal;
        //                    var UnitVolume = queryResult.BinBalance_VolumeBal / queryResult.BinBalance_QtyBal;

        //                    var Weight = resultItem.Qty * UnitWeight;
        //                    var Volume = resultItem.Qty * UnitVolume;
        //                    //var UnitWeight = queryResult.BinBalance_WeightBal / queryResult.BinBalance_QtyBal;
        //                    //var UnitVolume = queryResult.BinBalance_VolumeBal / queryResult.BinBalance_QtyBal;


        //                    //if (data.Qty == 0)
        //                    //{
        //                    //    String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                    //                        " BinBalance_QtyReserve  = 0 " +
        //                    //                        " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    //    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);


        //                    //    IsReserve = true;
        //                    //    //SqlClearReserve
        //                    //    String SqlcClearReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                    //                             "     BinBalance_QtyReserve  = BinBalance_QtyReserve = " + GIItem.TotalQty + "" +
        //                    //                             " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";

        //                    //    SqlClearReserve.Add(SqlcClearReserve);
        //                    //}
        //                    //else
        //                    //{

        //                    //bBinBalance_QtyBal = GIItem.TotalQty;
        //                    //bBinBalance_QtyReserve = GIItem.TotalQty;
        //                    //bBinBalance_WeightReserve = GIItem.Weight;
        //                    //bBinBalance_VolumeReserve = GIItem.Volume;
        //                    //bBinBalance_WeightBal = queryResult.BinBalance_WeightBal; ;
        //                    //bBinBalance_VolumeBal = queryResult.BinBalance_VolumeBal;
        //                    //bBinBalance_Index = queryResult.BinBalance_Index.ToString();


        //                    String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                              "     BinBalance_QtyBal  =  BinBalance_QtyBal - @BinBalance_QtyReserve" +
        //                                              "    , BinBalance_QtyReserve  = BinBalance_QtyReserve -  @BinBalance_QtyReserve " +
        //                                              "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve- @BinBalance_WeightReserve " +
        //                                              "  ,BinBalance_VolumeReserve  = BinBalance_VolumeReserve -  @BinBalance_VolumeReserve " +
        //                                              " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);



        //                    String SqlcmdUpdBal = " Update [dbo].[wm_BinBalance]  SET " +
        //                                              "   BinBalance_WeightBal = " + Weight + "" +
        //                                              "  ,BinBalance_VolumeBal  = " + Volume + ""+
        //                                              " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    context.Database.ExecuteSqlCommand(SqlcmdUpdBal, UnitWeight, UnitVolume, BinBalance_Index);

        //                    String SqlcmdCardReserve = " Update [dbo].[wm_BinCardReserve]  SET " +
        //                                               "     BinCardReserve_Status  =  2" +
        //                                               " where Convert(Varchar(200),BinCardReserve_Index) ='" + pBinCardRe_Index + "'";
        //                    context.Database.ExecuteSqlCommand(SqlcmdCardReserve);


        //                    IsReserve = true;
        //                    ////SqlClearReserve
        //                    //String SqlcClearReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                    //                    "     BinBalance_QtyBal  =  BinBalance_QtyBal + " + GIItem.TotalQty + "" +
        //                    //                    "    , BinBalance_QtyReserve  = BinBalance_QtyReserve + " + GIItem.TotalQty + "" +
        //                    //                    "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve + " + GIItem.Weight + "" +
        //                    //                    "  ,BinBalance_VolumeReserve  = BinBalance_VolumeReserve + " + GIItem.Volume + "" +
        //                    //                    " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    //SqlClearReserve.Add(SqlcClearReserve);

        //                    //String SqlcClearReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                    //                          "   BinBalance_QtyBal  =  BinBalance_QtyBal + " + queryResult.BinBalance_QtyBal + "" +
        //                    //                          "  ,BinBalance_QtyReserve  = BinBalance_QtyReserve + " + queryResult.BinBalance_QtyReserve + "" +
        //                    //                          "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve + " + queryResult.BinBalance_WeightReserve + "" +
        //                    //                          "  ,BinBalance_VolumeReserve  = BinBalance_VolumeReserve + " + queryResult.BinBalance_VolumeReserve + "" +
        //                    //                          " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    //SqlClearReserve.Add(SqlcClearReserve);


        //                    //String SqlcClearBal = " Update [dbo].[wm_BinBalance]  SET " +
        //                    //                    "  BinBalance_WeightBal  = BinBalance_WeightBal - " + Weight + "" +
        //                    //                    "  ,BinBalance_VolumeBal  =  BinBalance_VolumeBal - " + Volume + "" +
        //                    //                    " where Convert(Varchar(200),BinBalance_Index) ='" + queryResult.BinBalance_Index + "'";
        //                    //SqlClearBal.Add(SqlcClearBal);

        //                    //}

        //                    transaction2.Commit();

        //                }
        //                catch (Exception ex)
        //                {

        //                    transaction2.Rollback();
        //                    throw ex;
        //                }


        //            }

        //            return "Success";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        using (var context = new TransferDbContext())
        //        {
                   
        //            if (data.BinBalanceQtyBal < data.Qty)
        //            {
        //                if (IsAdjust == false || IsGr == false || IsTag == false || IsBin == false)
        //                {
        //                    String SqlCmd = "";

        //                    SqlCmd = " Delete from im_TransferStockAdjustment where Convert(Varchar(200),StockAdjustment_Index)  ='" + pStockAdjustmentIndex.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from im_TransferStockAdjustmentItem where Convert(Varchar(200),StockAdjustment_Index)  ='" + pStockAdjustmentIndex.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from im_GoodsReceive where Convert(Varchar(200),GoodsReceive_Index)  ='" + pGoodsReceiveIndex.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from im_GoodsReceiveItem where Convert(Varchar(200),GoodsReceive_Index)  ='" + pGoodsReceiveIndex.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from im_GoodsReceiveItemLocation where Convert(Varchar(200),GoodsReceive_Index)  ='" + pGoodsReceiveIndex.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from wm_TagItem where Convert(Varchar(200),TagItem_Index)  ='" + pTagItemIndex.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from wm_BinBalance where Convert(Varchar(200),BinBalance_Index)  ='" + pBinBalance_Index.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);

        //                    SqlCmd = " Delete from wm_BinCard where Convert(Varchar(200),BinCard_Index)  ='" + pBinCard_Index.ToString() + "'";
        //                    context.Database.ExecuteSqlCommand(SqlCmd);
        //                }
        //            }

        //            else if(IsAdjust == false || IsGi == false || IsBin == false)
        //            {

        //                using (var context2 = new TransferDbContext())
        //                {
        //                    var transactionIsReserve = context2.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {

        //                        String SqlCmd = "";

        //                        SqlCmd = " Delete from im_TransferStockAdjustment where Convert(Varchar(200),StockAdjustment_Index)  ='" + pStockAdjustmentIndex.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        SqlCmd = " Delete from im_TransferStockAdjustmentItem where Convert(Varchar(200),StockAdjustment_Index)  ='" + pStockAdjustmentIndex.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        SqlCmd = " Delete from im_GoodsIssue where Convert(Varchar(200),GoodsIssue_Index)  ='" + pGoodsIssueIndex.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        SqlCmd = " Delete from im_GoodsIssueItem where Convert(Varchar(200),GoodsIssue_Index)  ='" + pGoodsIssueIndex.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        SqlCmd = " Delete from im_GoodsIssueItemLocation where Convert(Varchar(200),GoodsIssue_Index)  ='" + pGoodsIssueIndex.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        SqlCmd = " Delete from wm_BinCard where Convert(Varchar(200),BinCard_Index)  ='" + pBinCard_Index.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        SqlCmd = " Delete from wm_BinCardReserve where Convert(Varchar(200),BinCardReserve_Index)  ='" + pBinCardRe_Index.ToString() + "'";
        //                        context.Database.ExecuteSqlCommand(SqlCmd);

        //                        foreach (String sql in SqlClearReserve)
        //                        {
        //                            context.Database.ExecuteSqlCommand(sql);
        //                        }

        //                        transactionIsReserve.Commit();

        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transactionIsReserve.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("TransferAdjust", msglog);
        //                        throw exy;
        //                    }
        //                }

                        
        //            }
        //        }


        //        throw ex;
        //    }
        //}

        public List<BinBalanceDocViewModel> ScanBarCode(BinBalanceDocViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    string SqlWhere = "";
                    if (data.ProductConversionBarcode != "" && data.ProductConversionBarcode != null)
                    {
                        SqlWhere = " and Location_Name = N'" + data.LocationName + "'" +
                        " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.ProductConversionBarcode + "') " +
                        " and ProductConversion_Index in (select ProductConversion_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.ProductConversionBarcode + "') " +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.OwnerIndex + "'" +
                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                        " and BinBalance_QtyBal > 0   and BinBalance_QtyReserve = 0 ";
                    }

                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).OrderBy(o => o.GoodsReceive_EXP_Date).ToList();


                    //var Product = context.ms_ProductConversionBarcode.FromSql("sp_GetProductConversionBarcode").Where(c => c.ProductConversionBarcode == data.ProductConversionBarcode).FirstOrDefault();

                    //var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance").Where(c => c.Product_Index == Product.Product_Index
                    //&& c.ProductConversion_Index == Product.ProductConversion_Index
                    //&& c.Location_Name == data.LocationName && c.Owner_Index == data.OwnerIndex
                    //&& c.BinBalance_QtyBal > 0 && c.BinBalance_QtyReserve == 0).OrderBy(o => o.GoodsReceive_EXP_Date).ToList();

                    var result = new List<BinBalanceDocViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new BinBalanceDocViewModel();
                        resultItem.BinBalance_Index = item.BinBalance_Index;
                        resultItem.Owner_Index = item.Owner_Index;
                        resultItem.Owner_Id = item.Owner_Id;
                        resultItem.Owner_Name = item.Owner_Name;
                        resultItem.Product_Index = item.Product_Index;
                        resultItem.Product_Id = item.Product_Id;
                        resultItem.Product_Name = item.Product_Name;
                        resultItem.BinBalance_QtyBal = item.BinBalance_QtyBal;
                        resultItem.ProductConversion_Index = item.ProductConversion_Index;
                        resultItem.ProductConversion_Id = item.ProductConversion_Id;
                        resultItem.ProductConversion_Name = item.ProductConversion_Name;

                        var SqlqueryRatio = " and ProductConversion_Index = N'" + item.ProductConversion_Index + "'";
                        var wherequeryRatio = new SqlParameter("@strwhere", SqlqueryRatio);
                        var Ratio = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", wherequeryRatio).FirstOrDefault();

                        resultItem.ProductConversion_Ratio = Ratio.ProductConversion_Ratio;
                        resultItem.LocationIndex = item.Location_Index;
                        resultItem.LocationId = item.Location_Id;
                        resultItem.LocationName = item.Location_Name;
                        resultItem.ItemStatus_Index = item.ItemStatus_Index;
                        resultItem.ItemStatus_Id = item.ItemStatus_Id;
                        resultItem.ItemStatus_Name = item.ItemStatus_Name;
                        resultItem.GoodsReceive_EXP_Date = item.GoodsReceive_EXP_Date.HasValue ? item.GoodsReceive_EXP_Date.Value.toString() : "";
                        resultItem.UDF_1 = item.UDF_1;
                        resultItem.UDF_2 = item.UDF_2;
                        resultItem.UDF_3 = item.UDF_3;

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

        public List<ReasonCodeViewModel> filterReasonCode()
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var queryResult = context.ms_ReasonCode.FromSql("sp_GetReasonCode").Where(c => c.Process_Index == new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740")).ToList();
                    var result = new List<ReasonCodeViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new ReasonCodeViewModel();

                        resultItem.ReasonCodeIndex = item.ReasonCode_Index;
                        resultItem.ReasonCodeId = item.ReasonCode_Id;
                        resultItem.ReasonCodeName = item.ReasonCode_Name;
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


        public String CheckLocation(BinBalanceDocViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {

                    string SqlWhere = "";

                    string SqlCheckLocation = " and Convert(Nvarchar(50),Location_Name) = N'" + data.LocationName + "' "+
                                              " and LocationType_Index = 'F9EDDAEC-A893-4F63-A700-526C69CC08C0'";
                    var strwhereCheckLocation = new SqlParameter("@strwhere", SqlCheckLocation);
                    var CheckLocation = context.MS_Location.FromSql("sp_GetLocation @strwhere", strwhereCheckLocation).FirstOrDefault();



                    if (CheckLocation != null)
                    {
                        return "true";
                    }

                    else
                    {
                        return "Location นี้ไม่มีในระบบ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
