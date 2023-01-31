using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace TransferBusiness.Transfer
{
    public class TransferItemService
    {
        public actionResultTransferViewModel CheckBinBalance(TransferViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {

                    var result = new List<TransferViewModel>();
                    //var resultNew = new List<GroupViewModel>();
                    var resultNew = new List<TransferViewModel>();
                    var actionResultLPN = new actionResultTransferViewModel();

                    string SqlWhere = "";
                    string SqlWhere1 = "";
                    string SqlWhere2 = "";
                    if (data.LocationName != null)
                    {
                        SqlWhere = " and Location_Name = N'" + data.LocationName + "'" +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                        " and Tag_No = '" + data.LocationName + "'" +
                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                        " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   ";
                    }
                    else
                    {
                        SqlWhere += " and Location_Name = N'" + "00000000000000" + "'";
                    }


                    if (data.ProductConversionBarcode != "" && data.LocationName != "")
                    {
                        SqlWhere1 = " and Location_Name = N'" + data.LocationName + "'" +
                       " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = '" + data.ProductConversionBarcode + "') " +
                       " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                       " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                       " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   ";
                    }
                    else
                    {
                        SqlWhere1 = " and Location_Name = N'" + data.LocationName + "'" +
                       " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = '" + data.ProductConversionBarcode + "') ";
                    }

                    // Find Barcode LPN
                    if (data.LpnNo != null && data.LpnNo != "")
                    {
                        SqlWhere = " and Tag_No = N'" + data.LpnNo + "'" +
                        " and Location_Name != '" + data.LpnNo + "'" +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                        " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   ";
                    }

                    if (data.ProductConversionBarcode != "" && data.LocationName != null && data.LpnNo != null && data.LpnNo != "")
                    {
                        SqlWhere1 = " and Location_Name = N'" + data.LocationName + "'" +
                       " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = '" + data.ProductConversionBarcode + "') " +
                       " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                       " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                       " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   ";
                    }
                    else
                    {
                        SqlWhere1 = SqlWhere1;
                    }

                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);

                    var queryResult1 = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere1", strwhere1).ToList();
                    var queryResult2 = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).Where(c => c.Location_Name != null).ToList();

                    if (queryResult1.Count > 0)
                    {
                        foreach (var item in queryResult1)
                        {
                            var resultItem = new TransferViewModel();
                            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.ownerIndex = item.Owner_Index;
                            resultItem.ownerId = item.Owner_Id;
                            resultItem.ownerName = item.Owner_Name;
                            resultItem.Tag_No = item.Tag_No;
                            resultItem.TagItemIndex = item.TagItem_Index;
                            resultItem.Tag_Index = item.Tag_Index;
                            resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                            resultItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
                            resultItem.GoodsReceiveItemLocationIndex = item.GoodsReceiveItemLocation_Index;
                            resultItem.GoodsReceiveNo = item.GoodsReceive_No;
                            resultItem.GoodsReceiveDate = item.GoodsReceive_Date;
                            resultItem.BinBalance_Index = item.BinBalance_Index;
                            resultItem.LocationIndex = item.Location_Index;
                            resultItem.LocationId = item.Location_Id;
                            resultItem.LocationName = item.Location_Name;
                            resultItem.ProductIndex = item.Product_Index;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ProductSecondName = item.Product_SecondName;
                            resultItem.ProductThirdName = item.Product_ThirdName;
                            resultItem.ProductLot = item.Product_Lot;
                            resultItem.ItemStatusId = item.ItemStatus_Id;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionIndex = item.ProductConversion_Index;

                            string TagSqlWhere = "";
                            if (item.Tag_Index != null)
                            {
                                TagSqlWhere = " and Tag_Index = '" + item.Tag_Index.ToString() + "'";

                            }
                            var Tagstrwhere = new SqlParameter("@Tagstrwhere", TagSqlWhere);
                            var checkStatus = context.wm_Tag.FromSql("sp_GetTag @Tagstrwhere", Tagstrwhere).FirstOrDefault();
                            if (checkStatus != null)
                            {
                                resultItem.Tag_Status = checkStatus.Tag_Status;
                            }
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.UDF4 = item.UDF_4;
                            resultItem.UDF5 = item.UDF_5;
                            resultItem.BinBalanceRatio = item.BinBalance_Ratio;
                            resultItem.TotalQty = item.BinBalance_QtyBal;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal - item.BinBalance_QtyReserve;
                            resultItem.BinBalanceQtyReserve = item.BinBalance_QtyReserve;
                            resultItem.BinBalanceWeightBal = item.BinBalance_WeightBal;
                            resultItem.BinBalanceVolumeBal = item.BinBalance_VolumeBal;
                            resultItem.BinBalanceWeightBegin = item.BinBalance_WeightBegin;
                            resultItem.BinBalanceQtyBegin = item.BinBalance_QtyBegin;
                            resultItem.BinBalanceVolumeBegin = item.BinBalance_VolumeBegin;


                            resultItem.MFG_Date = item.GoodsReceive_MFG_Date;
                            resultItem.EXP_Date = item.GoodsReceive_EXP_Date;
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_By = item.Update_By;

                            resultNew.Add(resultItem);
                        }
                    }
                    else if (data.ProductConversionBarcode != "" && data.LpnNo != "" && data.LocationName != "" && queryResult1.Count == 0)
                    {
                        foreach (var item in queryResult1)
                        {
                            var resultItem = new TransferViewModel();
                            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.ownerIndex = item.Owner_Index;
                            resultItem.ownerId = item.Owner_Id;
                            resultItem.ownerName = item.Owner_Name;
                            resultItem.Tag_No = item.Tag_No;
                            resultItem.TagItemIndex = item.TagItem_Index;
                            resultItem.Tag_Index = item.Tag_Index;
                            resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                            resultItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
                            resultItem.GoodsReceiveItemLocationIndex = item.GoodsReceiveItemLocation_Index;
                            resultItem.GoodsReceiveNo = item.GoodsReceive_No;
                            resultItem.GoodsReceiveDate = item.GoodsReceive_Date;
                            resultItem.BinBalance_Index = item.BinBalance_Index;
                            resultItem.LocationIndex = item.Location_Index;
                            resultItem.LocationId = item.Location_Id;
                            resultItem.LocationName = item.Location_Name;
                            resultItem.ProductIndex = item.Product_Index;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ProductSecondName = item.Product_SecondName;
                            resultItem.ProductThirdName = item.Product_ThirdName;
                            resultItem.ProductLot = item.Product_Lot;
                            resultItem.ItemStatusId = item.ItemStatus_Id;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionIndex = item.ProductConversion_Index;

                            string TagSqlWhere = "";
                            if (item.Tag_Index != null)
                            {
                                TagSqlWhere = " and Tag_Index = '" + item.Tag_Index.ToString() + "'";

                            }
                            var Tagstrwhere = new SqlParameter("@Tagstrwhere", TagSqlWhere);
                            var checkStatus = context.wm_Tag.FromSql("sp_GetTag @Tagstrwhere", Tagstrwhere).FirstOrDefault();
                            if (checkStatus != null)
                            {
                                resultItem.Tag_Status = checkStatus.Tag_Status;
                            }
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.UDF4 = item.UDF_4;
                            resultItem.UDF5 = item.UDF_5;
                            resultItem.BinBalanceRatio = item.BinBalance_Ratio;
                            resultItem.TotalQty = item.BinBalance_QtyBal;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal - item.BinBalance_QtyReserve;
                            resultItem.BinBalanceQtyReserve = item.BinBalance_QtyReserve;
                            resultItem.BinBalanceWeightBal = item.BinBalance_WeightBal;
                            resultItem.BinBalanceVolumeBal = item.BinBalance_VolumeBal;
                            resultItem.BinBalanceWeightBegin = item.BinBalance_WeightBegin;
                            resultItem.BinBalanceQtyBegin = item.BinBalance_QtyBegin;
                            resultItem.BinBalanceVolumeBegin = item.BinBalance_VolumeBegin;


                            resultItem.MFG_Date = item.GoodsReceive_MFG_Date;
                            resultItem.EXP_Date = item.GoodsReceive_EXP_Date;
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_By = item.Update_By;
                            resultNew.Add(resultItem);
                        }
                    }
                    else if (data.ProductConversionBarcode == "" && data.LocationName != null && data.LpnNo == "")
                    {
                        foreach (var item in queryResult2)
                        {
                            var resultItem = new TransferViewModel();
                            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.ownerIndex = item.Owner_Index;
                            resultItem.ownerId = item.Owner_Id;
                            resultItem.ownerName = item.Owner_Name;
                            resultItem.Tag_No = item.Tag_No;
                            resultItem.TagItemIndex = item.TagItem_Index;
                            resultItem.Tag_Index = item.Tag_Index;
                            resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                            resultItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
                            resultItem.GoodsReceiveItemLocationIndex = item.GoodsReceiveItemLocation_Index;
                            resultItem.GoodsReceiveNo = item.GoodsReceive_No;
                            resultItem.GoodsReceiveDate = item.GoodsReceive_Date;
                            resultItem.BinBalance_Index = item.BinBalance_Index;
                            resultItem.LocationIndex = item.Location_Index;
                            resultItem.LocationId = item.Location_Id;
                            resultItem.LocationName = item.Location_Name;
                            resultItem.ProductIndex = item.Product_Index;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ProductSecondName = item.Product_SecondName;
                            resultItem.ProductThirdName = item.Product_ThirdName;
                            resultItem.ProductLot = item.Product_Lot;
                            resultItem.ItemStatusId = item.ItemStatus_Id;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionIndex = item.ProductConversion_Index;

                            string TagSqlWhere = "";
                            if (item.Tag_Index != null)
                            {
                                TagSqlWhere = " and Tag_Index = '" + item.Tag_Index.ToString() + "'";

                            }
                            var Tagstrwhere = new SqlParameter("@Tagstrwhere", TagSqlWhere);
                            var checkStatus = context.wm_Tag.FromSql("sp_GetTag @Tagstrwhere", Tagstrwhere).FirstOrDefault();
                            if (checkStatus != null)
                            {
                                resultItem.Tag_Status = checkStatus.Tag_Status;
                            }
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.UDF4 = item.UDF_4;
                            resultItem.UDF5 = item.UDF_5;
                            resultItem.BinBalanceRatio = item.BinBalance_Ratio;
                            resultItem.TotalQty = item.BinBalance_QtyBal;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal - item.BinBalance_QtyReserve;
                            resultItem.BinBalanceQtyReserve = item.BinBalance_QtyReserve;
                            resultItem.BinBalanceWeightBal = item.BinBalance_WeightBal;
                            resultItem.BinBalanceVolumeBal = item.BinBalance_VolumeBal;
                            resultItem.BinBalanceWeightBegin = item.BinBalance_WeightBegin;
                            resultItem.BinBalanceQtyBegin = item.BinBalance_QtyBegin;
                            resultItem.BinBalanceVolumeBegin = item.BinBalance_VolumeBegin;


                            resultItem.MFG_Date = item.GoodsReceive_MFG_Date;
                            resultItem.EXP_Date = item.GoodsReceive_EXP_Date;
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_By = item.Update_By;
                            resultNew.Add(resultItem);
                        }
                    }
                    else if (data.ProductConversionBarcode == "" && data.LpnNo != "" && data.LocationName != "")
                    {
                        foreach (var item in queryResult2)
                        {
                            var resultItem = new TransferViewModel();
                            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.ownerIndex = item.Owner_Index;
                            resultItem.ownerId = item.Owner_Id;
                            resultItem.ownerName = item.Owner_Name;
                            resultItem.Tag_No = item.Tag_No;
                            resultItem.TagItemIndex = item.TagItem_Index;
                            resultItem.Tag_Index = item.Tag_Index;
                            resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                            resultItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
                            resultItem.GoodsReceiveItemLocationIndex = item.GoodsReceiveItemLocation_Index;
                            resultItem.GoodsReceiveNo = item.GoodsReceive_No;
                            resultItem.GoodsReceiveDate = item.GoodsReceive_Date;
                            resultItem.BinBalance_Index = item.BinBalance_Index;
                            resultItem.LocationIndex = item.Location_Index;
                            resultItem.LocationId = item.Location_Id;
                            resultItem.LocationName = item.Location_Name;
                            resultItem.ProductIndex = item.Product_Index;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ProductSecondName = item.Product_SecondName;
                            resultItem.ProductThirdName = item.Product_ThirdName;
                            resultItem.ProductLot = item.Product_Lot;
                            resultItem.ItemStatusId = item.ItemStatus_Id;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionIndex = item.ProductConversion_Index;

                            string TagSqlWhere = "";
                            if (item.Tag_Index != null)
                            {
                                TagSqlWhere = " and Tag_Index = '" + item.Tag_Index.ToString() + "'";

                            }
                            var Tagstrwhere = new SqlParameter("@Tagstrwhere", TagSqlWhere);
                            var checkStatus = context.wm_Tag.FromSql("sp_GetTag @Tagstrwhere", Tagstrwhere).FirstOrDefault();
                            if (checkStatus != null)
                            {
                                resultItem.Tag_Status = checkStatus.Tag_Status;
                            }
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.UDF4 = item.UDF_4;
                            resultItem.UDF5 = item.UDF_5;
                            resultItem.BinBalanceRatio = item.BinBalance_Ratio;
                            resultItem.TotalQty = item.BinBalance_QtyBal;
                            resultItem.BinBalanceQtyBal = item.BinBalance_QtyBal - item.BinBalance_QtyReserve;
                            resultItem.BinBalanceQtyReserve = item.BinBalance_QtyReserve;
                            resultItem.BinBalanceWeightBal = item.BinBalance_WeightBal;
                            resultItem.BinBalanceVolumeBal = item.BinBalance_VolumeBal;
                            resultItem.BinBalanceWeightBegin = item.BinBalance_WeightBegin;
                            resultItem.BinBalanceQtyBegin = item.BinBalance_QtyBegin;
                            resultItem.BinBalanceVolumeBegin = item.BinBalance_VolumeBegin;


                            resultItem.MFG_Date = item.GoodsReceive_MFG_Date;
                            resultItem.EXP_Date = item.GoodsReceive_EXP_Date;
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_By = item.Update_By;
                            resultNew.Add(resultItem);
                        }
                    }

                    actionResultLPN.itemsLPN = resultNew.ToList();
                    //actionResultLPN.itemsGroup = resultNew.ToList();
                    //actionResultLPN.itemsLPN = result.ToList();

                    return actionResultLPN;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultCHeckTagViewModel ScanTagNo(listTransferItem data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var result = new List<TransferViewModel>();
                    var resultCheckNotZero = new List<TransferViewModel>();
                    var actionResultCheckTag = new actionResultCHeckTagViewModel();
                    var resultItems = new TransferViewModel();
                    var results = new List<TransferViewModel>();




                    foreach (var newItem in data.listTransferItemViewModel)
                    {
                        string SqlWhere = "";
                        string SqlWhere1 = "";
                        if (newItem.TagNoNew != null)
                        {
                            SqlWhere = " and Tag_No = N'" + newItem.TagNoNew + "'" +
                            " and Convert(Nvarchar(50),Owner_Index) = N'" + newItem.ownerIndex + "'" +
                            " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + newItem.WareHouseIndex + "') " +
                            " and BinBalance_QtyBal = 0 ";

                        }

                        if (newItem.TagNoNew != null)
                        {
                            SqlWhere1 = " and Tag_No = N'" + newItem.TagNoNew + "'" +
                            " and Convert(Nvarchar(50),Owner_Index) = N'" + newItem.ownerIndex + "'" +
                            " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + newItem.WareHouseIndex + "') " +
                            " and BinBalance_QtyReserve > 0 ";

                        }

                        string SqlTagNo = " and Convert(Nvarchar(50),Tag_No) = N'" + newItem.TagNoNew + "' ";
                        var strwhereTagNo = new SqlParameter("@strwhere", SqlTagNo);
                        var TagNo = context.wm_Tag.FromSql("sp_GetTag @strwhere", strwhereTagNo).FirstOrDefault();


                        string pstring = " and Tag_No ='" + newItem.TagNoNew + "'";
                        //var swhere = new SqlParameter("@strwhere", SqlWhere);
                        var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
                        var ColumnName2 = new SqlParameter("@ColumnName2", "Format_Text");
                        var ColumnName3 = new SqlParameter("@ColumnName3", "Format_Date");
                        var ColumnName4 = new SqlParameter("@ColumnName4", "Format_Running");
                        var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        var TableName = new SqlParameter("@TableName", "ms_DocumentType");
                        var Where = new SqlParameter("@Where", " where DocumentType_Index = 'CB8FEA3E-0683-44B8-A05F-BDB358ABF8D0'");
                        var DataDocumentType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                        var FormatLPN = DataDocumentType.dataincolumn2 + DataDocumentType.dataincolumn3 + DataDocumentType.dataincolumn4;
                        var FormatLPNNum = FormatLPN.Length;
                        int LPNNum = newItem.TagNoNew.Length;

                        if (TagNo != null)
                        {

                            if (LPNNum == FormatLPNNum)
                            {
                                var FormatText = DataDocumentType.dataincolumn2.Length;
                                var FormatDate = DataDocumentType.dataincolumn3.Length;
                                var FormatRunning = DataDocumentType.dataincolumn4.Length;
                                var LPNText = newItem.TagNoNew.Substring(0, FormatText);
                                var LPNDate = newItem.TagNoNew.Substring(FormatText, FormatDate);
                                var LPNRunning = newItem.TagNoNew.Substring((FormatDate + FormatText), FormatRunning);
                                var chekNumeric = newItem.TagNoNew.Substring(FormatText, (FormatRunning + FormatDate));
                                var isNumeric = int.TryParse(chekNumeric, out int n);

                                //เช๊ค Format_Text
                                if (LPNText.Length != FormatText)
                                {
                                    resultItems.Tag_No = "false";
                                    results.Add(resultItems);
                                }
                                //เช๊ค Format_Date
                                else if (LPNDate.Length != FormatDate)
                                {
                                    resultItems.Tag_No = "false";
                                    results.Add(resultItems);
                                }
                                //เช๊ค Format_Running
                                else if (LPNRunning.Length != FormatRunning)
                                {
                                    resultItems.Tag_No = "false";
                                    results.Add(resultItems);
                                }
                                //เช๊ค 3 ตัวหน้าตรงกับ Format_Text หรือป่าว
                                else if (LPNText != DataDocumentType.dataincolumn2)
                                {
                                    resultItems.Tag_No = "false";
                                    results.Add(resultItems);
                                }
                                //เช๊ค Formate_Date && Formate_Running เป็นตัวเลขหรือป่าว
                                else if (isNumeric != true)
                                {
                                    resultItems.Tag_No = "false";
                                    results.Add(resultItems);
                                }
                                else
                                {
                                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                                    var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                                    var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();
                                    var queryResult1 = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();

                                    if (queryResult.Count > 0)
                                    {
                                        foreach (var item in queryResult)
                                        {
                                            var resultItem = new TransferViewModel();
                                            resultItem.Tag_Index = item.Tag_Index;
                                            resultItem.Tag_No = item.Tag_No;
                                            resultItem.TagItemIndex = item.TagItem_Index;
                                            resultItem.BinBalance_Index = item.BinBalance_Index;
                                            resultItem.Create_By = item.Create_By;
                                            resultItem.Update_By = item.Update_By;
                                            result.Add(resultItem);

                                        }
                                    }
                                    if (queryResult1.Count > 0)
                                    {
                                        foreach (var item in queryResult1)
                                        {
                                            var resultItem1 = new TransferViewModel();
                                            resultItem1.Tag_Index = item.Tag_Index;
                                            resultItem1.Tag_No = item.Tag_No;
                                            resultItem1.TagItemIndex = item.TagItem_Index;
                                            resultItem1.BinBalance_Index = item.BinBalance_Index;
                                            resultItem1.Create_By = item.Create_By;
                                            resultItem1.Update_By = item.Update_By;
                                            resultCheckNotZero.Add(resultItem1);

                                        }
                                    }

                                    resultItems.Tag_No = "true";
                                    results.Add(resultItems);

                                }

                            }

                            else
                            {
                                var strwhere = new SqlParameter("@strwhere", SqlWhere);
                                var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                                var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();
                                var queryResult1 = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();

                                if (queryResult.Count > 0)
                                {
                                    foreach (var item in queryResult)
                                    {
                                        var resultItem = new TransferViewModel();
                                        resultItem.Tag_Index = item.Tag_Index;
                                        resultItem.Tag_No = item.Tag_No;
                                        resultItem.TagItemIndex = item.TagItem_Index;
                                        resultItem.BinBalance_Index = item.BinBalance_Index;
                                        resultItem.Create_By = item.Create_By;
                                        resultItem.Update_By = item.Update_By;
                                        result.Add(resultItem);

                                    }
                                }
                                if (queryResult1.Count > 0)
                                {
                                    foreach (var item in queryResult1)
                                    {
                                        var resultItem1 = new TransferViewModel();
                                        resultItem1.Tag_Index = item.Tag_Index;
                                        resultItem1.Tag_No = item.Tag_No;
                                        resultItem1.TagItemIndex = item.TagItem_Index;
                                        resultItem1.BinBalance_Index = item.BinBalance_Index;
                                        resultItem1.Create_By = item.Create_By;
                                        resultItem1.Update_By = item.Update_By;
                                        resultCheckNotZero.Add(resultItem1);

                                    }
                                }

                                resultItems.Tag_No = "true";
                                results.Add(resultItems);
                            }

                        }
                        else
                        {
                            resultItems.Tag_No = "false";
                            results.Add(resultItems);
                        }


                    }

                    actionResultCheckTag.itemsCheckTagNo = result.ToList();
                    actionResultCheckTag.itemsCheckTagNoQtyNotZero = resultCheckNotZero.ToList();
                    actionResultCheckTag.Formart = results.ToList();

                    return actionResultCheckTag;

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
                       " and Location_Name in (select Location_Name from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') ";
                    }
                    else
                    {
                        SqlWhere += " and Location_Name = '" + "00000000000000" + "'";
                    }

                    if (data.lpnNo != "" && data.lpnNo != null)
                    {
                        SqlWhere1 = " and Location_Name = N'" + data.LocationName + "'" +
                      " and Tag_No = N'" + data.lpnNo + "'" +
                      " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                      " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                      " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";
                    }
                    else
                    {
                        SqlWhere1 += " and Tag_No = '" + "00000000000000" + "'";
                    }

                    if (data.productConversionBarcode != "" && data.lpnNo == null && data.LocationName != "" && data.LocationName != null)
                    {
                        SqlWhere2 = " and Location_Name = N'" + data.LocationName + "'" +
                       " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.productConversionBarcode + "') " +
                       " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                       " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                       " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";

                    }
                    else
                    {
                        SqlWhere2 += " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.productConversionBarcode + "') ";
                    }

                    //if (data.productConversionBarcode != "" && data.productConversionBarcode != null)
                    //{
                    //    //var findBarcode = context.ms_ProductConversionBarcode.FromSql("sp_GetProductConversionBarcode").Where(c => c.ProductConversionBarcode_Id == data.productConversionBarcodeId).FirstOrDefault();

                    //    SqlWhere2 = " and Product_Index in (select Product_Index from ms_ProductConversionBarcode where ProductConversionBarcode = N'" + data.productConversionBarcode + "') " +
                    //  " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                    //  " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                    //  " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";


                    //}
                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                    var strwhere2 = new SqlParameter("@strwhere2", SqlWhere2);
                    var queryResult = context.View_SumQtyBinbalance.FromSql("sp_GetViewSumQtyBinbalance @strwhere", strwhere).Where(c => c.Location_Name != null).ToList();
                    var queryResult1 = context.View_BinBalance.FromSql("sp_GetBinBalanceTransfer @strwhere1", strwhere1).Where(c => c.Tag_No != null).ToList();
                    var queryResult2 = context.View_BinBalance.FromSql("sp_GetBinBalanceTransfer @strwhere2", strwhere2).ToList();

                    var resultLoc = new List<SumQtyBinbalanceViewModel>();
                    var resultLPN = new List<SumQtyBinbalanceViewModel>();

                    var groupLoc = queryResult.GroupBy(c => new { c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();
                    var groupLPN = queryResult1.GroupBy(c => new { c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();
                    var groupBarcodeLoc = queryResult2.GroupBy(c => new { c.Product_Name, c.BinBalance_QtyBal, }).Select(c => new { c.Key.Product_Name, c.Key.BinBalance_QtyBal }).ToList();
                    if (queryResult.Count > 0)
                    {
                        foreach (var item in queryResult.GroupBy(c => c.Product_Name).ToList())
                        {
                            var resultItem = new SumQtyBinbalanceViewModel();
                            var sum = item.Sum(c => c.BinBalance_QtyBal);
                            resultItem.ProductName = item.Key;
                            resultItem.BinBalanceQtyBal = sum;
                            var MaxQty = item.Max(c => c.BinBalance_QtyBal);
                            var ProductConversion = item.Where(c => c.BinBalance_QtyBal == MaxQty).Select(c => c.ProductConversion_Name).FirstOrDefault();
                            resultItem.productConversionName = ProductConversion;
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
                            resultItem2.productConversionName = item.FirstOrDefault().ProductConversion_Name;
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
        public actionResultTransferViewModel CheckProductList(string ProductName)
        {
            try
            {
                using (var context = new TransferDbContext())
                {

                    var resultNew = new List<GroupViewModel>();
                    string SqlWhere = "";

                    if (ProductName != null)
                    {
                        SqlWhere = " and Product_Name = N'" + ProductName + "'";

                    }
                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var queryResult = context.View_BinBalance.FromSql("sp_GetBinBalanceTransfer @strwhere", strwhere).ToList();
                    var group = queryResult.GroupBy(c => new
                    {
                        c.Tag_Index,
                        c.Tag_No,
                        c.Product_Name,
                        c.Product_Id,
                        c.Location_Index,
                        c.Location_Id,
                        c.Location_Name,
                        c.ItemStatus_Index,
                        c.ItemStatus_Id,
                        c.ItemStatus_Name,
                        c.ProductConversion_Index,
                        c.ProductConversion_Id,
                        c.ProductConversion_Name,
                        c.Owner_Index,
                        c.Owner_Id,
                        c.Owner_Name,
                        c.GoodsReceive_EXP_Date,
                        c.UDF_1,
                        c.UDF_2,
                        c.UDF_3,
                        c.BinBalance_QtyBal
                    }).Select(c => new
                    {
                        c.Key.Tag_No,
                        c.Key.Product_Name,
                        c.Key.Product_Id,
                        c.Key.Location_Index,
                        c.Key.Location_Id,
                        c.Key.Location_Name,
                        c.Key.ItemStatus_Index,
                        c.Key.ItemStatus_Id,
                        c.Key.ItemStatus_Name,
                        c.Key.Tag_Index,
                        c.Key.ProductConversion_Index,
                        c.Key.ProductConversion_Id,
                        c.Key.ProductConversion_Name,
                        c.Key.Owner_Index,
                        c.Key.Owner_Id,
                        c.Key.Owner_Name,
                        c.Key.GoodsReceive_EXP_Date,
                        c.Key.UDF_1,
                        c.Key.UDF_2,
                        c.Key.UDF_3,
                        c.Key.BinBalance_QtyBal,
                    }).ToList();

                    if (group.Count > 0)
                    {
                        foreach (var item in group)
                        {
                            var resultItem = new GroupViewModel();
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.Tag_No = item.Tag_No;
                            resultItem.Tag_Index = item.Tag_Index;
                            resultItem.LocationIndex = item.Location_Index;
                            resultItem.LocationId = item.Location_Id;
                            resultItem.LocationName = item.Location_Name;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.itemStatusId = item.ItemStatus_Id;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.ItemStatusFrom = item.ItemStatus_Name;
                            resultItem.ProductConversionName = item.ProductConversion_Name;

                            string SqlWhere1 = "";
                            if (item.Tag_Index != null)
                            {
                                SqlWhere1 = " and Tag_Index = '" + item.Tag_Index.ToString() + "'";

                            }
                            var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                            var checkStatus = context.wm_Tag.FromSql("sp_GetTag @strwhere1", strwhere1).FirstOrDefault();
                            if (checkStatus != null)
                            {
                                resultItem.Tag_Status = checkStatus.Tag_Status;
                            }
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.Qty = item.BinBalance_QtyBal;
                            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.GoodsReceive_EXP_Date);
                            resultNew.Add(resultItem);
                        }
                    }

                    var actionResultLPN = new actionResultTransferViewModel();
                    actionResultLPN.itemsGroup = resultNew.ToList();

                    return actionResultLPN;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultTransferViewModel ScanTagNoReserve(TransferViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var actionResult = new actionResultTransferViewModel();
                    var result = new List<TransferViewModel>();
                    var result1 = new List<TransferViewModel>();
                    var resultItems = new TransferViewModel();
                    var results = new List<TransferViewModel>();

                    string SqlWhere = "";

                    string SqlWhere1 = "";

                    if (data.LpnNo != null && data.LpnNo != "")
                    {
                        SqlWhere1 = " and Tag_No = N'" + data.LpnNo + "'";
                    }
                    var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                    var chkData = context.wm_Tag.FromSql("sp_GetTag @strwhere1", strwhere1).Where(c => c.Tag_No != null).FirstOrDefault();


                    if (data.LpnNo != null)
                    {
                        SqlWhere = " and Tag_No = N'" + data.LpnNo + "'" +
                        " and Location_Name != '" + data.LpnNo + "'" +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                        " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve = 0 ";
                    }
                    else
                    {
                        SqlWhere = " and Location_Name = N'" + "00000000000000" + "'";
                    }

                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).Where(c => c.Tag_No != null).ToList();


                    string SqlTagNo = " and Convert(Nvarchar(50),Tag_No) = N'" + data.LpnNo + "' ";
                    var strwhereTagNo = new SqlParameter("@strwhere", SqlTagNo);
                    var TagNo = context.wm_Tag.FromSql("sp_GetTag @strwhere", strwhereTagNo).FirstOrDefault();


                    string pstring = " and Tag_No ='" + data.LpnNo + "'";
                    //var swhere = new SqlParameter("@strwhere", SqlWhere);
                    var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
                    var ColumnName2 = new SqlParameter("@ColumnName2", "Format_Text");
                    var ColumnName3 = new SqlParameter("@ColumnName3", "Format_Date");
                    var ColumnName4 = new SqlParameter("@ColumnName4", "Format_Running");
                    var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                    var TableName = new SqlParameter("@TableName", "ms_DocumentType");
                    var Where = new SqlParameter("@Where", " where DocumentType_Index = 'CB8FEA3E-0683-44B8-A05F-BDB358ABF8D0'");
                    var DataDocumentType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                    var FormatLPN = DataDocumentType.dataincolumn2 + DataDocumentType.dataincolumn3 + DataDocumentType.dataincolumn4;
                    var FormatLPNNum = FormatLPN.Length;
                    int LPNNum = data.LpnNo.Length;

                    if (TagNo != null)
                    {
                        if (LPNNum == FormatLPNNum)
                        {
                            var FormatText = DataDocumentType.dataincolumn2.Length;
                            var FormatDate = DataDocumentType.dataincolumn3.Length;
                            var FormatRunning = DataDocumentType.dataincolumn4.Length;
                            var LPNText = data.LpnNo.Substring(0, FormatText);
                            var LPNDate = data.LpnNo.Substring(FormatText, FormatDate);
                            var LPNRunning = data.LpnNo.Substring((FormatDate + FormatText), FormatRunning);
                            var chekNumeric = data.LpnNo.Substring(FormatText, (FormatRunning + FormatDate));
                            var isNumeric = int.TryParse(chekNumeric, out int n);

                            //เช๊ค Format_Text
                            if (LPNText.Length != FormatText)
                            {
                                resultItems.Tag_No = "false";
                                results.Add(resultItems);
                            }
                            //เช๊ค Format_Date
                            else if (LPNDate.Length != FormatDate)
                            {
                                resultItems.Tag_No = "false";
                                results.Add(resultItems);
                            }
                            //เช๊ค Format_Running
                            else if (LPNRunning.Length != FormatRunning)
                            {
                                resultItems.Tag_No = "false";
                                results.Add(resultItems);
                            }
                            //เช๊ค 3 ตัวหน้าตรงกับ Format_Text หรือป่าว
                            else if (LPNText != DataDocumentType.dataincolumn2)
                            {
                                resultItems.Tag_No = "false";
                                results.Add(resultItems);
                            }
                            //เช๊ค Formate_Date && Formate_Running เป็นตัวเลขหรือป่าว
                            else if (isNumeric != true)
                            {
                                resultItems.Tag_No = "false";
                                results.Add(resultItems);
                            }
                            else
                            {
                                if (queryResult.Count > 0)
                                {
                                    foreach (var item in queryResult)
                                    {
                                        var resultItem = new TransferViewModel();
                                        resultItem.ownerIndex = item.Owner_Index;
                                        resultItem.ownerId = item.Owner_Id;
                                        resultItem.ownerName = item.Owner_Name;
                                        resultItem.Tag_No = item.Tag_No;
                                        resultItem.Tag_Index = item.Tag_Index;
                                        resultItem.BinBalance_Index = item.BinBalance_Index;
                                        resultItem.Create_By = item.Create_By;
                                        resultItem.Update_By = item.Update_By;
                                        result.Add(resultItem);

                                    }
                                }
                                resultItems.Tag_No = "true";
                                results.Add(resultItems);
                            }
                        }
                        else
                        {
                            if (queryResult.Count > 0)
                            {
                                foreach (var item in queryResult)
                                {
                                    var resultItem = new TransferViewModel();
                                    resultItem.ownerIndex = item.Owner_Index;
                                    resultItem.ownerId = item.Owner_Id;
                                    resultItem.ownerName = item.Owner_Name;
                                    resultItem.Tag_No = item.Tag_No;
                                    resultItem.Tag_Index = item.Tag_Index;
                                    resultItem.BinBalance_Index = item.BinBalance_Index;
                                    resultItem.Create_By = item.Create_By;
                                    resultItem.Update_By = item.Update_By;
                                    result.Add(resultItem);

                                }
                            }

                            resultItems.Tag_No = "true";
                            results.Add(resultItems);
                        }
                    }

                    else
                    {
                        resultItems.Tag_No = "false";
                        results.Add(resultItems);
                    }

                    if (chkData != null)
                    {
                        var resultItem = new TransferViewModel();
                        resultItem.Tag_No = chkData.Tag_No;
                        resultItem.Tag_Index = chkData.Tag_Index;
                        result1.Add(resultItem);
                    }
                    actionResult.itemsLPN = result.ToList();
                    actionResult.CheckSearchLPN = result1.ToList();
                    actionResult.Formart = results.ToList();

                    return actionResult;
                    //return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public Boolean SaveDataRelocation(listTransferItem dataList)
        //{
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    var SqlClearError = new List<string>();

        //    Boolean IsError = false;

        //    Boolean IsBinBalanceTransfer = false;
        //    Boolean IsBinBalance = false;


        //    Guid pBinCardReserveNew = Guid.NewGuid();

        //    Guid GoodTransfer_Index = Guid.NewGuid();

        //    Guid GoodTransferItem_Index = Guid.NewGuid();

        //    Guid pBinBalance_Index = Guid.NewGuid();

        //    Guid pBinCard_IndexTo = Guid.NewGuid();

        //    Guid pBinCard_IndexFrom = Guid.NewGuid();

        //    Guid Tag_Index = Guid.NewGuid();

        //    Guid TagItem_Index = Guid.NewGuid();

        //    decimal? IsBinBalance_QtyReserve = 0;
        //    decimal? IsBinBalance_WeightReserve = 0;
        //    decimal? IsBinBalance_VolumeReserve = 0;
        //    String IsBinBalance_Index = "";

        //    decimal? bcBinBalance_QtyReserve = 0;
        //    decimal? bcBinBalance_WeightReserve = 0;
        //    decimal? bcBinBalance_VolumeReserve = 0;
        //    String bcBinBalance_Index = "";
        //    String bcLocation_Index = "";
        //    String bcLocation_Id = "";
        //    String bcLocation_Name = "";
        //    String bcTag_Index = "";
        //    String bcTag_No = "";
        //    String bcCreate_By = "";

        //    String TagItemIndex = "";

        //    try
        //    {

        //        using (var context = new TransferDbContext())
        //        {

        //            var listTagData = new List<TagViewModel>();
        //            var listTagItem = new List<TagItemViewModel>();
        //            var listGoodsTransfer = new List<GoodsTransferViewModel>();
        //            var listGoodsTransferItem = new List<GoodsTransferItemViewModel>();
        //            var listBinBalance = new List<BinBalanceViewModel>();
        //            var listBinCard = new List<BinCardViewModel>();
        //            var listBinCardReserve = new List<BinCardReserveModel>();
        //            var TagData = new TagViewModel();
        //            //-------------------- Transfer TagItem --------------------//
        //            foreach (var data in dataList.listTransferItemViewModel)
        //            {
        //                // Set Parameter 

        //                //-------------------- Transfer from DC (ASN) --------------------//
        //                Guid Process_Index = new Guid("408FD0AF-1592-4FA7-8BA0-03F6C3215D41");
        //                Guid DocType_Index = new Guid("063BC3F4-A8F5-48EA-8B36-ECCEAD297484");
        //                var TransferDocDate = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
        //                //var oTransferDocDate = String.Format("{0:dd/MM/yyyy}", DateTime.Now);



        //                DateTime oTransferDocDate = DateTime.Now;
        //                var GoodsTransferNo = "";
        //                var IsUse = new SqlParameter("@IsUse", Guid.NewGuid().ToString());



        //                var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
        //                var ColumnName2 = new SqlParameter("@ColumnName2", "DocumentType_Id");
        //                var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Name");
        //                var ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                var ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                var TableName = new SqlParameter("@TableName", "ms_DocumentType");
        //                var Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),DocumentType_Index)  ='" + DocType_Index.ToString() + "'");
        //                var DataDocumentType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();




        //                // Set Document No
        //                var DocumentType_Index = new SqlParameter("@DocumentType_Index", DocType_Index.ToString());
        //                var DocDate = new SqlParameter("@DocDate", TransferDocDate.ToString());
        //                var resultDocNoParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultDocNoParameter.Size = 2000; // some meaningfull value
        //                resultDocNoParameter.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultDocNoParameter);
        //                //var result = resultParameter.Value;
        //                GoodsTransferNo = resultDocNoParameter.Value.ToString();

        //                //Find new Location and new Tag From NewLPN
        //                string SqlWhereTag = " and Tag_No = N'" + data.TagNoNew + "'";
        //                var strwhereTag = new SqlParameter("@strwhere", SqlWhereTag);

        //                var newData = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhereTag).FirstOrDefault();
        //                Guid Location_Index_To;
        //                String Location_Id_To;
        //                String Location_Name_To;


        //                //var GoodsReceive_Index = new SqlParameter("@GoodsReceive_Index", data.GoodsReceiveIndex.ToString());
        //                //var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                //resultParameter.Size = 2000; // some meaningfull value
        //                //resultParameter.Direction = ParameterDirection.Output;
        //                //context.Database.ExecuteSqlCommand("EXEC sp_GetLocationConfirmGR @GoodsReceive_Index  ,@txtReturn OUTPUT", GoodsReceive_Index, resultParameter);
        //                //var SuggestLocation = resultParameter.Value.ToString();

        //                var WareHouse_Index = new SqlParameter("@WareHouse_Index", data.WareHouseIndex.ToString());
        //                DocumentType_Index = new SqlParameter("@DocumentType_Index", DocType_Index.ToString());
        //                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultParameter.Size = 2000; // some meaningfull value
        //                resultParameter.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_GetTranferWareHouse @WareHouse_Index,@DocumentType_Index  ,@txtReturn OUTPUT", WareHouse_Index, DocumentType_Index, resultParameter);

        //                var SuggestLocation = resultParameter.Value.ToString();

        //                ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //                ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
        //                ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
        //                ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                TableName = new SqlParameter("@TableName", "ms_Location");
        //                Where = new SqlParameter("@Where", " Where Location_Name  ='" + SuggestLocation.ToString() + "'");
        //                var DataLocationWH = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

        //                if (newData != null)
        //                {
        //                    Location_Index_To = (Guid)newData.Location_Index;
        //                    Location_Id_To = newData.Location_Id;
        //                    Location_Name_To = newData.Location_Name;
        //                }
        //                else
        //                {
        //                    //Location_Index_To = new Guid("5d30facb-ed0f-480a-a26d-f6b35308ee05");
        //                    //Location_Id_To = "7";
        //                    //Location_Name_To = SuggestLocation;
        //                    Location_Index_To = new Guid(DataLocationWH[0].dataincolumn1);
        //                    Location_Id_To = DataLocationWH[0].dataincolumn2;
        //                    Location_Name_To = DataLocationWH[0].dataincolumn3;

        //                }




        //                // Create HeaderTransfer 
        //                ////-------------------- GR GoodTransfer --------------------
        //                var GoodsTransfer = new GoodsTransferViewModel();

        //                GoodsTransfer.GoodsTransferIndex = GoodTransfer_Index;
        //                GoodsTransfer.OwnerIndex = data.ownerIndex;// new Guid(DataOwner[0].dataincolumn1);  //item.Owner_Index;
        //                GoodsTransfer.DocumentTypeIndex = DocType_Index;  //new Guid(DataDocumentType[0].dataincolumn1); ;
        //                GoodsTransfer.GoodsTransferNo = GoodsTransferNo;
        //                GoodsTransfer.GoodsTransferDate = oTransferDocDate;
        //                GoodsTransfer.DocumentRefNo1 = "";
        //                GoodsTransfer.DocumentRefNo2 = "";
        //                GoodsTransfer.DocumentRefNo3 = "";
        //                GoodsTransfer.DocumentRefNo4 = "";
        //                GoodsTransfer.DocumentRefNo5 = "";
        //                GoodsTransfer.DocumentStatus = 0;
        //                GoodsTransfer.UDF1 = "";
        //                GoodsTransfer.UDF2 = "";
        //                GoodsTransfer.UDF3 = "";
        //                GoodsTransfer.UDF4 = "";
        //                GoodsTransfer.UDF5 = "";
        //                GoodsTransfer.DocumentPriorityStatus = 0;
        //                GoodsTransfer.CreateBy = data.Update_By;
        //                //GoodsTransfer.CreateDate = DateTime.Now;

        //                //string pstring = " and BinBalance_Index = N'" + dataList.BinBalance_Index + "'";
        //                //var strwhere = new SqlParameter("@strwhere", pstring);

        //                //var queryResult = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();

        //                // Set Transfer Item
        //                string SqlWhere = " and BinBalance_Index = N'" + data.BinBalance_Index + "'" +
        //                                    " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "' " +
        //                                    " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50),Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
        //                                    " and (BinBalance_QtyBal - BinBalance_QtyReserve)  > 0  and BinBalance_QtyBal > 0   and BinBalance_QtyReserve >= 0 ";

        //                var strwhere = new SqlParameter("@strwhere", SqlWhere);
        //                var queryBalance = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).Where(c => c.Location_Name != null).FirstOrDefault();


        //                int iRows = 0;
        //                if (queryBalance != null)
        //                {
        //                    var newtransaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {

        //                        // Lock STOCK Balance 
        //                        String SqlcmdUpd = " Update [dbo].[wm_BinBalance] Set  IsUse =  @IsUse  where  isnull(IsUse,'') = ''  " + SqlWhere;
        //                        context.Database.ExecuteSqlCommand(SqlcmdUpd, IsUse);
        //                        iRows = iRows + 1;


        //                        //var TagItem = new TagItemViewModel();                                
        //                        //var BinBalance = new BinBalanceViewModel();
        //                        //var BinCard = new BinCardViewModel();
        //                        //var BinCardReserve = new BinCardReserveModel();


        //                        var GoodsTransferItem = new GoodsTransferItemViewModel();

        //                        var BinCardReserve = new BinCardReserveModel();

        //                        var TransferQty = queryBalance.BinBalance_QtyBal - queryBalance.BinBalance_QtyReserve;

        //                        if (TransferQty < data.TotalQty)
        //                        {
        //                            return false;
        //                        }
        //                        TransferQty = data.TotalQty;
        //                        ////-------------------- GR GoodTransferItem --------------------

        //                        GoodsTransferItem.GoodsTransferItemIndex = GoodTransferItem_Index;
        //                        GoodsTransferItem.GoodsTransferIndex = GoodsTransfer.GoodsTransferIndex;
        //                        GoodsTransferItem.GoodsReceiveIndex = queryBalance.GoodsReceive_Index;
        //                        GoodsTransferItem.GoodsReceiveItemIndex = queryBalance.GoodsReceiveItem_Index;
        //                        GoodsTransferItem.GoodsReceiveItemLocationIndex = queryBalance.GoodsReceiveItemLocation_Index;
        //                        GoodsTransferItem.LineNum = iRows.ToString();
        //                        GoodsTransferItem.TagItemIndex = queryBalance.TagItem_Index;
        //                        GoodsTransferItem.TagIndex = queryBalance.Tag_Index;
        //                        GoodsTransferItem.TagIndexTo = queryBalance.Tag_Index;
        //                        GoodsTransferItem.ProductIndex = queryBalance.Product_Index;
        //                        GoodsTransferItem.ProductIndexTo = queryBalance.Product_Index;
        //                        GoodsTransferItem.ProductLot = queryBalance.Product_Lot;
        //                        GoodsTransferItem.ProductLotTo = queryBalance.Product_Lot;
        //                        GoodsTransferItem.ItemStatusIndex = queryBalance.ItemStatus_Index;
        //                        GoodsTransferItem.ItemStatusIndexTo = queryBalance.ItemStatus_Index;
        //                        GoodsTransferItem.ProductConversionIndex = queryBalance.ProductConversion_Index;
        //                        GoodsTransferItem.OwnerIndex = data.ownerIndex;//data.Owner_Index;
        //                        GoodsTransferItem.OwnerIndexTo = data.ownerIndex;
        //                        GoodsTransferItem.LocationIndex = data.LocationIndex;

        //                        GoodsTransferItem.LocationIndexTo = Location_Index_To;


        //                        GoodsTransferItem.GoodsReceiveEXPDate = queryBalance.GoodsReceive_EXP_Date;
        //                        GoodsTransferItem.GoodsReceiveEXPDateTo = queryBalance.GoodsReceive_EXP_Date;
        //                        GoodsTransferItem.Qty = TransferQty;
        //                        GoodsTransferItem.TotalQty = TransferQty;
        //                        GoodsTransferItem.Weight = queryBalance.BinBalance_WeightBal;
        //                        GoodsTransferItem.Volume = queryBalance.BinBalance_VolumeBal;

        //                        //GoodsTransferItem.RefProcessIndex = itemList.Ref_Process_Index;
        //                        //GoodsTransferItem.RefDocumentNo = itemList.Ref_Document_No;
        //                        //GoodsTransferItem.RefDocumentIndex = itemList.Ref_Document_Index;
        //                        //GoodsTransferItem.RefDocumentItemIndex = itemList.Ref_DocumentItem_Index;
        //                        GoodsTransferItem.CreateBy = data.Update_By;
        //                        //GoodsTransferItem.CreateDate = DateTime.Now;
        //                        listGoodsTransferItem.Add(GoodsTransferItem);


        //                        // ADD Bin Reserve
        //                        BinCardReserve.BinCardReserve_Index = pBinCardReserveNew;
        //                        BinCardReserve.BinBalance_Index = data.BinBalance_Index;
        //                        BinCardReserve.Process_Index = Process_Index;  // GI Process
        //                        BinCardReserve.GoodsReceive_Index = queryBalance.GoodsReceive_Index;
        //                        BinCardReserve.GoodsReceive_No = queryBalance.GoodsReceive_No;
        //                        BinCardReserve.GoodsReceive_Date = queryBalance.GoodsReceive_Date;
        //                        BinCardReserve.GoodsReceiveItem_Index = queryBalance.GoodsReceiveItem_Index;
        //                        BinCardReserve.TagItem_Index = queryBalance.TagItem_Index;
        //                        BinCardReserve.Tag_Index = queryBalance.Tag_Index;
        //                        BinCardReserve.Tag_No = queryBalance.Tag_No;
        //                        BinCardReserve.Product_Index = queryBalance.Product_Index;
        //                        BinCardReserve.Product_Id = queryBalance.Product_Id;
        //                        BinCardReserve.Product_Name = queryBalance.Product_Name;
        //                        BinCardReserve.Product_SecondName = queryBalance.Product_SecondName;
        //                        BinCardReserve.Product_ThirdName = queryBalance.Product_ThirdName;
        //                        BinCardReserve.Product_Lot = queryBalance.Product_Lot;
        //                        BinCardReserve.ItemStatus_Index = queryBalance.ItemStatus_Index;
        //                        BinCardReserve.ItemStatus_Id = queryBalance.ItemStatus_Id;
        //                        BinCardReserve.ItemStatus_Name = queryBalance.ItemStatus_Name;
        //                        BinCardReserve.MFG_Date = queryBalance.GoodsReceive_MFG_Date;
        //                        BinCardReserve.EXP_Date = queryBalance.GoodsReceive_EXP_Date;
        //                        BinCardReserve.ProductConversion_Index = queryBalance.ProductConversion_Index;
        //                        BinCardReserve.ProductConversion_Id = queryBalance.ProductConversion_Id;
        //                        BinCardReserve.ProductConversion_Name = queryBalance.ProductConversion_Name;
        //                        BinCardReserve.Owner_Index = data.ownerIndex;
        //                        BinCardReserve.Owner_Id = data.ownerId;
        //                        BinCardReserve.Owner_Name = data.ownerName;
        //                        BinCardReserve.Location_Index = data.LocationIndex;
        //                        BinCardReserve.Location_Id = data.LocationId;
        //                        BinCardReserve.Location_Name = data.LocationName;
        //                        BinCardReserve.BinCardReserve_QtyBal = TransferQty;
        //                        if (data.BinBalanceWeightBegin == 0)
        //                        {
        //                            BinCardReserve.BinCardReserve_WeightBal = 0;
        //                        }
        //                        else
        //                        {
        //                            if (queryBalance.BinBalance_WeightBegin == 0)
        //                            {
        //                                BinCardReserve.BinCardReserve_WeightBal = 0;
        //                            }
        //                            else
        //                            {
        //                                BinCardReserve.BinCardReserve_WeightBal = TransferQty * (queryBalance.BinBalance_QtyBegin / queryBalance.BinBalance_WeightBegin);
        //                            }


        //                        }
        //                        if (data.BinBalanceVolumeBegin == 0)
        //                        {
        //                            BinCardReserve.BinCardReserve_VolumeBal = 0;
        //                        }
        //                        else
        //                        {
        //                            if (queryBalance.BinBalance_VolumeBegin == 0)
        //                            {
        //                                BinCardReserve.BinCardReserve_VolumeBal = 0;
        //                            }
        //                            else
        //                            {
        //                                BinCardReserve.BinCardReserve_VolumeBal = TransferQty * (queryBalance.BinBalance_QtyBegin / queryBalance.BinBalance_VolumeBegin);
        //                            }


        //                        }

        //                        BinCardReserve.Ref_Document_No = GoodsTransferNo;
        //                        BinCardReserve.Ref_Document_Index = GoodsTransferItem.GoodsTransferIndex;
        //                        BinCardReserve.Ref_DocumentItem_Index = GoodsTransferItem.GoodsTransferItemIndex;

        //                        BinCardReserve.Ref_Wave_Index = IsUse.Value.ToString();
        //                        BinCardReserve.Create_By = data.Update_By;
        //                        //      BinCardReserve.Create_Date =  data.Create_Date;

        //                        listBinCardReserve.Add(BinCardReserve);

        //                        var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", TransferQty);
        //                        var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserve.BinCardReserve_WeightBal);
        //                        var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserve.BinCardReserve_VolumeBal);
        //                        var BinBalance_Index = new SqlParameter("@BinBalance_Index", data.BinBalance_Index);

        //                        IsBinBalance_QtyReserve = queryBalance.BinBalance_QtyReserve;
        //                        IsBinBalance_WeightReserve = queryBalance.BinBalance_WeightReserve;
        //                        IsBinBalance_VolumeReserve = queryBalance.BinBalance_VolumeReserve;
        //                        IsBinBalance_Index = data.BinBalance_Index.ToString();

        //                        String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                  "     BinBalance_QtyReserve  =  BinBalance_QtyReserve + @BinBalance_QtyReserve " +
        //                                                  "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve +  @BinBalance_WeightReserve " +
        //                                                  "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve + @BinBalance_VolumeReserve " +
        //                                                    "  where  BinBalance_Index = @BinBalance_Index  ";
        //                        context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);


        //                        if (queryBalance != null)
        //                        {
        //                            // Clear LOCK
        //                            String SqlcmdUpF = " Update [dbo].[wm_BinBalance] Set  IsUse = '' where  isnull(IsUse,'') = @IsUse";
        //                            context.Database.ExecuteSqlCommand(SqlcmdUpF, IsUse);
        //                        }

        //                        newtransaction.Commit();
        //                    } // Try Transacation
        //                    catch (Exception ex)
        //                    {
        //                        newtransaction.Rollback();
        //                        throw ex;
        //                    }

        //                    listGoodsTransfer.Add(GoodsTransfer);
        //                    DataTable dtGoodsTransfer = CreateDataTable(listGoodsTransfer);
        //                    DataTable dtGoodsTransferItem = CreateDataTable(listGoodsTransferItem);
        //                    DataTable dtBinCardReserve = CreateDataTable(listBinCardReserve);

        //                    ////  Save Transfer  and  BinCardReserve

        //                    var pGoodsTransfer = new SqlParameter("GoodsTransfer", SqlDbType.Structured);
        //                    pGoodsTransfer.TypeName = "[dbo].[im_GoodsTransferData]";
        //                    pGoodsTransfer.Value = dtGoodsTransfer;

        //                    var pGoodsTransferItem = new SqlParameter("GoodsTransferItem", SqlDbType.Structured);
        //                    pGoodsTransferItem.TypeName = "[dbo].[im_GoodsTransferItemData]";
        //                    pGoodsTransferItem.Value = dtGoodsTransferItem;

        //                    var pBinCardReserve = new SqlParameter("BinCardReserve", SqlDbType.Structured);
        //                    pBinCardReserve.TypeName = "[dbo].[wm_BinCardReserveData]";
        //                    pBinCardReserve.Value = dtBinCardReserve;

        //                    var transactionx = context.Database.BeginTransaction(IsolationLevel.Serializable);

        //                    try
        //                    {
        //                        // ADD DATA To Stroe
        //                        var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_TranferConfirm @GoodsTransfer, @GoodsTransferItem ,@BinCardReserve", pGoodsTransfer, pGoodsTransferItem, pBinCardReserve);

        //                        transactionx.Commit();
        //                        IsError = true;
        //                        IsBinBalance = true;

        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transactionx.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("Save_TranferConfirm", msglog);
        //                        throw exy;

        //                    }
        //                    //if (rowsAffected.ToString() != "0")
        //                    //{
        //                    // GetDataTransfer  For Confirm

        //                    string SqlTFWhere = " and Convert(Nvarchar(50),GoodsTransfer_Index)   = N'" + GoodTransfer_Index.ToString() + "'";
        //                    var strtfwhere = new SqlParameter("@strwhere", SqlTFWhere);

        //                    string SqlTFWhere1 = " and Convert(Nvarchar(50),GoodsTransferItem_Index)   = N'" + GoodTransferItem_Index.ToString() + "'";
        //                    var strtfwhere1 = new SqlParameter("@strwhere1", SqlTFWhere1);

        //                    var TransferResult = context.IM_GoodsTransfer.FromSql("sp_GetGoodsTransfer @strwhere", strtfwhere).ToList();
        //                    var TransferItemResult = context.IM_GoodsTransferItem.FromSql("sp_GetGoodsTransferItem @strwhere", strtfwhere).ToList();

        //                    string SqlBinCardReserveWhere = " and Convert(Nvarchar(50),Process_Index) = N'" + Process_Index.ToString() + "'" +
        //                                                    " and Convert(Nvarchar(50),Ref_Document_Index) = N'" + GoodTransfer_Index.ToString() + "'";

        //                    var strBinCardReservewhere = new SqlParameter("@strwhere", SqlBinCardReserveWhere);
        //                    var BinCardReserveResult = context.WM_BinCardReserve.FromSql("sp_GetBinCardReserve @strwhere", strBinCardReservewhere).ToList();

        //                    if (TransferResult.Count != 1 || TransferItemResult.Count < 1 || BinCardReserveResult.Count < 1)
        //                    {
        //                        // Error 
        //                        return false;

        //                    }
        //                    // Add TAG ITEM


        //                    var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {
        //                        foreach (var BinCardReserveItem in BinCardReserveResult)
        //                        {
        //                            var BinBalance = new BinBalanceViewModel();
        //                            //   var BinCard = new BinCardViewModel();


        //                            //Select Data from Balance
        //                            string SqlBinBalanceWhere = " and Convert(Nvarchar(50), BinBalance_Index) = N'" + BinCardReserveItem.BinBalance_Index.ToString() + "'";
        //                            var strBinBalancewhere = new SqlParameter("@strwhere", SqlBinBalanceWhere);
        //                            var BinBalanceResult = context.wm_BinBalance2.FromSql("sp_GetBinBalance @strwhere", strBinBalancewhere).FirstOrDefault();


        //                            bcBinBalance_QtyReserve = BinBalanceResult.BinBalance_QtyReserve;
        //                            bcBinBalance_WeightReserve = BinBalanceResult.BinBalance_QtyReserve;
        //                            bcBinBalance_VolumeReserve = BinBalanceResult.BinBalance_VolumeReserve;
        //                            bcBinBalance_Index = BinBalanceResult.BinBalance_Index.ToString();
        //                            bcLocation_Index = BinBalanceResult.Location_Index.ToString();
        //                            bcLocation_Id = BinBalanceResult.Location_Id;
        //                            bcLocation_Name = BinBalanceResult.Location_Name;
        //                            bcTag_Index = BinBalanceResult.Tag_Index.ToString();
        //                            bcTag_No = BinBalanceResult.Tag_No;
        //                            bcCreate_By = BinBalanceResult.Create_By;

        //                            // Select By Line Item 
        //                            var TransferItemResultSelect = TransferItemResult.Where(c => c.GoodsTransferItem_Index == BinCardReserveItem.Ref_DocumentItem_Index).FirstOrDefault();

        //                            // Select Desc ItemStatus TO
        //                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
        //                            ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
        //                            ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
        //                            ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                            ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                            TableName = new SqlParameter("@TableName", "ms_ItemStatus");
        //                            Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),ItemStatus_Index)  ='" + TransferItemResultSelect.ItemStatus_Index_To.ToString() + "'");
        //                            var DatItemStatusTo = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();


        //                            // Select Desc ItemStatus TO
        //                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //                            ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
        //                            ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
        //                            ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                            ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                            TableName = new SqlParameter("@TableName", "ms_Location");
        //                            Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),Location_Index)  ='" + TransferItemResultSelect.Location_Index_To.ToString() + "'");
        //                            var DataLocationTo = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();


        //                            if (BinBalanceResult.BinBalance_QtyBal == BinCardReserveItem.BinCardReserve_QtyBal && BinBalanceResult.BinBalance_QtyBal == TransferItemResultSelect.TotalQty && BinBalanceResult.BinBalance_QtyBal == BinBalanceResult.BinBalance_QtyReserve)
        //                            {
        //                                // Update Binbalance  :  QtyBal and  Itemstatus
        //                                TagItemIndex = BinBalanceResult.TagItem_Index.ToString();

        //                                string SqlTagWhere = " and Convert(Nvarchar(50), Tag_No) = N'" + data.TagNoNew + "' ";
        //                                var strTagwhere = new SqlParameter("@strwhere", SqlTagWhere);
        //                                var Tag = context.wm_Tag.FromSql("sp_GetTag @strwhere ", strTagwhere).FirstOrDefault();
        //                                if (Tag == null)
        //                                {
        //                                    var Tag_Index1 = new SqlParameter("Tag_Index", Tag_Index);
        //                                    var Tag_No1 = new SqlParameter("Tag_No", data.TagNoNew);
        //                                    var Pallet_No = new SqlParameter("Pallet_No", "");
        //                                    var Pallet_Index = new SqlParameter("Pallet_Index", Guid.NewGuid());
        //                                    //    Pallet_Index.SqlValue = Tag_Index;
        //                                    int tagStatus = 1;
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
        //                                    var Create_By = new SqlParameter("Create_By", data.Create_By);
        //                                    var Create_Date = new SqlParameter("Create_Date", DateTime.Now.Date);
        //                                    var Update_By = new SqlParameter("Update_By", "");
        //                                    var Update_Date = new SqlParameter("Update_Date", DateTime.Now.Date);
        //                                    var Cancel_By = new SqlParameter("Cancel_By", "");
        //                                    var Cancel_Date = new SqlParameter("Cancel_Date", DateTime.Now.Date);
        //                                    var rowsAffectedTag = context.Database.ExecuteSqlCommand("sp_Save_wm_Tag  @Tag_Index,@Tag_No,@Pallet_No,@Pallet_Index,@TagRef_No1,@TagRef_No2,@TagRef_No3,@TagRef_No4,@TagRef_No5,@Tag_Status,@UDF_1,@UDF_2,@UDF_3,@UDF_4,@UDF_5,@Create_By,@Create_Date,@Update_By,@Update_Date,@Cancel_By,@Cancel_Date ", Tag_Index1, Tag_No1, Pallet_No, Pallet_Index, TagRef_No1, TagRef_No2, TagRef_No3, TagRef_No4, TagRef_No5, Tag_Status, UDF_1, UDF_2, UDF_3, UDF_4, UDF_5, Create_By, Create_Date, Update_By, Update_Date, Cancel_By, Cancel_Date);

        //                                    //TagData = new TagViewModel();
        //                                    //TagData.TagIndex = Tag_Index;
        //                                    //TagData.TagNo = data.TagNoNew;
        //                                    //TagData.PalletNo = "";
        //                                    //TagData.PalletIndex = Guid.NewGuid();
        //                                    //TagData.TagRefNo1 = "";
        //                                    //TagData.TagRefNo2 = "";
        //                                    //TagData.TagRefNo3 = "";
        //                                    //TagData.TagRefNo4 = "";
        //                                    //TagData.TagRefNo5 = "";
        //                                    //TagData.TagStatus = 1;
        //                                    //TagData.UDF1 = "";
        //                                    //TagData.UDF2 = "";
        //                                    //TagData.UDF3 = "";
        //                                    //TagData.UDF4 = "";
        //                                    //TagData.UDF5 = "";
        //                                    //TagData.CreateBy = data.Create_By;


        //                                }
        //                                else
        //                                {
        //                                    Tag_Index = Tag.Tag_Index;
        //                                }


        //                                var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", BinCardReserveItem.BinCardReserve_QtyBal);
        //                                var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserveItem.BinCardReserve_WeightBal);
        //                                var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserveItem.BinCardReserve_VolumeBal);
        //                                var BinBalance_Index = new SqlParameter("@BinBalance_Index", BinCardReserveItem.BinBalance_Index);
        //                                //var ItemStatus_Index = new SqlParameter("@ItemStatus_Index", TransferItemResultSelect.ItemStatus_Index_To);
        //                                //var ItemStatus_Id = new SqlParameter("@ItemStatus_Id", DatItemStatusTo[0].dataincolumn2);
        //                                //var ItemStatus_Name = new SqlParameter("@ItemStatus_Name", DatItemStatusTo[0].dataincolumn3);
        //                                var Location_Index = new SqlParameter("@Location_Index", TransferItemResultSelect.Location_Index_To);
        //                                var Location_Id = new SqlParameter("@Location_Id", DataLocationTo[0].dataincolumn2);
        //                                var Location_Name = new SqlParameter("@Location_Name", DataLocationTo[0].dataincolumn3);
        //                                var pTag_Index = new SqlParameter("@Tag_Index", Tag_Index);
        //                                var pTag_No = new SqlParameter("@Tag_No", data.TagNoNew);
        //                                var pCreate_By = new SqlParameter("@Create_By", data.Create_By);

        //                                try
        //                                {
        //                                    String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                               "  BinBalance_QtyReserve  = BinBalance_QtyReserve - @BinBalance_QtyReserve " +
        //                                                               "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  @BinBalance_WeightReserve " +
        //                                                               "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - @BinBalance_VolumeReserve " +
        //                                                               " ,Location_Index  =  @Location_Index " +
        //                                                               " ,Location_Id    =   @Location_Id " +
        //                                                               " ,Location_Name  =   @Location_Name  " +
        //                                                               " ,Tag_Index  =   @Tag_Index  " +
        //                                                               " ,Tag_No  =   @Tag_No  " +
        //                                                               " ,Create_By  =   @Create_By  " +
        //                                                               "  where  BinBalance_Index = @BinBalance_Index  ";
        //                                    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, Location_Index, Location_Id, Location_Name, pTag_Index, pTag_No, pCreate_By, BinBalance_Index);

        //                                    IsError = true;
        //                                    //transaction.Commit();

        //                                }
        //                                catch (Exception exy)
        //                                {
        //                                    transaction.Rollback();
        //                                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                                    olog.logging("Save_TranferConfirm", msglog);
        //                                    throw exy;
        //                                }
        //                            }
        //                            else  // Split Line Binbalance 
        //                            {


        //                                var BinBalance_QtyReserve = new SqlParameter("@BinBalance_QtyReserve", BinCardReserveItem.BinCardReserve_QtyBal);
        //                                var BinBalance_WeightReserve = new SqlParameter("@BinBalance_WeightReserve", BinCardReserveItem.BinCardReserve_WeightBal);
        //                                var BinBalance_VolumeReserve = new SqlParameter("@BinBalance_VolumeReserve", BinCardReserveItem.BinCardReserve_VolumeBal);
        //                                var BinBalance_Index = new SqlParameter("@BinBalance_Index", BinCardReserveItem.BinBalance_Index);


        //                                try
        //                                {

        //                                    String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                                                "  BinBalance_QtyBal  = BinBalance_QtyBal - @BinBalance_QtyReserve " +
        //                                                                "  ,BinBalance_WeightBal  = BinBalance_WeightBal -  @BinBalance_WeightReserve " +
        //                                                                "  ,BinBalance_VolumeBal  =  BinBalance_VolumeBal - @BinBalance_VolumeReserve " +
        //                                                                "  ,BinBalance_QtyReserve  = BinBalance_QtyReserve - @BinBalance_QtyReserve " +
        //                                                                "  ,BinBalance_WeightReserve  = BinBalance_WeightReserve -  @BinBalance_WeightReserve " +
        //                                                                "  ,BinBalance_VolumeReserve  =  BinBalance_VolumeReserve - @BinBalance_VolumeReserve " +
        //                                                                "  where  BinBalance_Index = @BinBalance_Index  ";
        //                                    context.Database.ExecuteSqlCommand(SqlcmdUpdReserve, BinBalance_QtyReserve, BinBalance_WeightReserve, BinBalance_VolumeReserve, BinBalance_Index);

        //                                    IsError = true;

        //                                    //transaction.Commit();

        //                                }
        //                                catch (Exception exy)
        //                                {
        //                                    transaction.Rollback();
        //                                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                                    olog.logging("Save_TranferConfirm", msglog);
        //                                    throw exy;
        //                                }


        //                                string SqlTagWhere = " and Convert(Nvarchar(50), Tag_No) = N'" + data.TagNoNew + "' ";
        //                                var strTagwhere = new SqlParameter("@strwhere", SqlTagWhere);
        //                                var Tag = context.wm_Tag.FromSql("sp_GetTag @strwhere ", strTagwhere).FirstOrDefault();

        //                                //string SqlTagItemWhere = " and Convert(Nvarchar(50), TagItem_Index) = N'" + BinCardReserveItem.TagItem_Index.ToString() + "' ";
        //                                //var strTagItemwhere = new SqlParameter("@strwhere", SqlTagItemWhere);
        //                                //var itemsTag = context.wm_TagItem.FromSql("sp_GetTagItem @strwhere ", strTagItemwhere).FirstOrDefault();

        //                                //Check Case New LPN 
        //                                // var isNewLPN = TransferItemResult.FirstOrDefault().Location_Index_To.ToString() == "5d30facb-ed0f-480a-a26d-f6b35308ee05";
        //                                bool isNewLPN;
        //                                if (Tag == null)
        //                                {
        //                                    isNewLPN = true;
        //                                }
        //                                else
        //                                {
        //                                    isNewLPN = false;
        //                                }

        //                                if (isNewLPN == true)
        //                                {
        //                                    //Create New TAG / New LPN
        //                                    TagData = new TagViewModel();
        //                                    TagData.TagIndex = Tag_Index;
        //                                    TagData.TagNo = data.TagNoNew;
        //                                    TagData.PalletNo = "";
        //                                    TagData.PalletIndex = Guid.NewGuid();
        //                                    TagData.TagRefNo1 = "";
        //                                    TagData.TagRefNo2 = "";
        //                                    TagData.TagRefNo3 = "";
        //                                    TagData.TagRefNo4 = "";
        //                                    TagData.TagRefNo5 = "";
        //                                    TagData.TagStatus = 1;
        //                                    TagData.UDF1 = "";
        //                                    TagData.UDF2 = "";
        //                                    TagData.UDF3 = "";
        //                                    TagData.UDF4 = "";
        //                                    TagData.UDF5 = "";
        //                                    TagData.CreateBy = data.Create_By;


        //                                    var TagItem = new TagItemViewModel();
        //                                    TagItem.TagItemIndex = TagItem_Index;
        //                                    TagItem.TagIndex = Tag_Index;
        //                                    TagItem.TagNo = data.TagNoNew;
        //                                    TagItem.GoodsReceiveIndex = data.GoodsReceiveIndex;
        //                                    TagItem.GoodsReceiveItemIndex = data.GoodsReceiveItemIndex;
        //                                    TagItem.ProductIndex = data.ProductIndex;
        //                                    TagItem.ProductId = data.ProductId;
        //                                    TagItem.ProductName = data.ProductName;
        //                                    TagItem.ProductSecondName = data.ProductSecondName;
        //                                    TagItem.ProductThirdName = data.ProductThirdName;
        //                                    TagItem.ProductLot = data.ProductLot;
        //                                    TagItem.ItemStatusIndex = data.ItemStatusIndex_From;
        //                                    TagItem.ItemStatusId = data.ItemStatusId;
        //                                    TagItem.ItemStatusName = data.ItemStatusName_From;
        //                                    TagItem.Qty = (BinCardReserveItem.BinCardReserve_QtyBal / data.BinBalanceRatio);
        //                                    TagItem.Ratio = data.BinBalanceRatio;
        //                                    TagItem.TotalQty = BinCardReserveItem.BinCardReserve_QtyBal;
        //                                    TagItem.ProductConversionIndex = data.ProductConversionIndex;
        //                                    TagItem.ProductConversionId = data.ProductConversionId;
        //                                    TagItem.ProductConversionName = data.ProductConversionName;
        //                                    TagItem.Weight = data.BinBalanceWeightBal;
        //                                    TagItem.Volume = data.BinBalanceVolumeBal;
        //                                    TagItem.MFGDate = data.MFG_Date;
        //                                    TagItem.EXPDate = data.EXP_Date;
        //                                    TagItem.TagRefNo1 = data.TagRef_No1;
        //                                    TagItem.TagRefNo2 = data.TagRef_No2;
        //                                    TagItem.TagRefNo3 = data.TagRef_No3;
        //                                    TagItem.TagRefNo4 = data.TagRef_No4;
        //                                    TagItem.TagRefNo5 = data.TagRef_No5;
        //                                    TagItem.TagStatus = 1;
        //                                    TagItem.UDF1 = data.UDF1;
        //                                    TagItem.UDF2 = data.UDF2;
        //                                    TagItem.UDF3 = data.UDF3;
        //                                    TagItem.UDF4 = data.UDF4;
        //                                    TagItem.UDF5 = data.UDF5;
        //                                    TagItem.CreateBy = data.Update_By;
        //                                    // TagItem.CreateDate = DateTime.Now;

        //                                    listTagItem.Add(TagItem);
        //                                }
        //                                else
        //                                {
        //                                    // CASE OLD TAG / LPN
        //                                    Tag_Index = Tag.Tag_Index;

        //                                    TagData = new TagViewModel();
        //                                    TagData.TagIndex = Tag_Index;
        //                                    TagData.TagNo = Tag.Tag_No;
        //                                    TagData.PalletNo = Tag.Pallet_No;
        //                                    TagData.PalletIndex = Tag.Pallet_Index;
        //                                    TagData.TagRefNo1 = Tag.TagRef_No1;
        //                                    TagData.TagRefNo2 = Tag.TagRef_No2;
        //                                    TagData.TagRefNo3 = Tag.TagRef_No3;
        //                                    TagData.TagRefNo4 = Tag.TagRef_No4;
        //                                    TagData.TagRefNo5 = Tag.TagRef_No5;
        //                                    TagData.TagStatus = Tag.Tag_Status;
        //                                    TagData.UDF1 = Tag.UDF_1;
        //                                    TagData.UDF2 = Tag.UDF_2;
        //                                    TagData.UDF3 = Tag.UDF_3;
        //                                    TagData.UDF4 = Tag.UDF_4;
        //                                    TagData.UDF5 = Tag.UDF_5;
        //                                    TagData.CreateBy = data.Create_By;

        //                                    var TagItem = new TagItemViewModel();

        //                                    TagItem.TagItemIndex = TagItem_Index;
        //                                    TagItem.TagIndex = Tag.Tag_Index;
        //                                    TagItem.TagNo = Tag.Tag_No;
        //                                    TagItem.GoodsReceiveIndex = data.GoodsReceiveIndex;
        //                                    TagItem.GoodsReceiveItemIndex = data.GoodsReceiveItemIndex;
        //                                    TagItem.ProductIndex = data.ProductIndex;
        //                                    TagItem.ProductId = data.ProductId;
        //                                    TagItem.ProductName = data.ProductName;
        //                                    TagItem.ProductSecondName = data.ProductSecondName;
        //                                    TagItem.ProductThirdName = data.ProductThirdName;
        //                                    TagItem.ProductLot = data.ProductLot;
        //                                    TagItem.ItemStatusIndex = data.ItemStatusIndex_From;
        //                                    TagItem.ItemStatusId = data.ItemStatusId;
        //                                    TagItem.ItemStatusName = data.ItemStatusName_From;
        //                                    TagItem.Qty = (BinCardReserveItem.BinCardReserve_QtyBal / data.BinBalanceRatio);
        //                                    TagItem.Ratio = data.BinBalanceRatio;
        //                                    TagItem.TotalQty = BinCardReserveItem.BinCardReserve_QtyBal;
        //                                    TagItem.ProductConversionIndex = data.ProductConversionIndex;
        //                                    TagItem.ProductConversionId = data.ProductConversionId;
        //                                    TagItem.ProductConversionName = data.ProductConversionName;
        //                                    TagItem.Weight = data.BinBalanceWeightBal;
        //                                    TagItem.Volume = data.BinBalanceVolumeBal;
        //                                    TagItem.MFGDate = data.MFG_Date;
        //                                    TagItem.EXPDate = data.EXP_Date;
        //                                    TagItem.TagRefNo1 = data.TagRef_No1;
        //                                    TagItem.TagRefNo2 = data.TagRef_No2;
        //                                    TagItem.TagRefNo3 = data.TagRef_No3;
        //                                    TagItem.TagRefNo4 = data.TagRef_No4;
        //                                    TagItem.TagRefNo5 = data.TagRef_No5;
        //                                    TagItem.TagStatus = data.Tag_Status;
        //                                    TagItem.UDF1 = data.UDF1;
        //                                    TagItem.UDF2 = data.UDF2;
        //                                    TagItem.UDF3 = data.UDF3;
        //                                    TagItem.UDF4 = data.UDF4;
        //                                    TagItem.UDF5 = data.UDF5;
        //                                    TagItem.CreateBy = data.Update_By;
        //                                    // TagItem.CreateDate = DateTime.Now;

        //                                    listTagItem.Add(TagItem);
        //                                }

        //                                // Update Old Line Binbalance 

        //                                BinBalance.BinBalance_Index = pBinBalance_Index;
        //                                BinBalance.Owner_Index = new Guid(BinCardReserveItem.Owner_Index.ToString());//item.Owner_Index;
        //                                BinBalance.Owner_Id = BinCardReserveItem.Owner_Id;//item.Owner_Id;
        //                                BinBalance.Owner_Name = BinCardReserveItem.Owner_Name; // item.Owner_Name;


        //                                BinBalance.LocationIndex = new Guid(DataLocationTo[0].dataincolumn1);   //BinCardReserveItem.Location_Index;
        //                                BinBalance.LocationId = DataLocationTo[0].dataincolumn2; // BinCardReserveItem.Location_Id;
        //                                BinBalance.LocationName = DataLocationTo[0].dataincolumn3;//BinCardReserveItem.Location_Name;



        //                                BinBalance.GoodsReceive_Index = new Guid(BinCardReserveItem.GoodsReceive_Index.ToString());
        //                                BinBalance.GoodsReceive_No = BinCardReserveItem.GoodsReceive_No; //item.GoodsReceive_No;  
        //                                BinBalance.GoodsReceive_Date = BinCardReserveItem.GoodsReceive_Date;  //item.GoodsReceive_Date;
        //                                BinBalance.GoodsReceiveItem_Index = new Guid(BinCardReserveItem.GoodsReceiveItem_Index.ToString());
        //                                BinBalance.GoodsReceiveItemLocation_Index = new Guid(TransferItemResultSelect.GoodsReceiveItemLocation_Index.ToString());//item.GoodsReceiveItemLocation_Index;
        //                                BinBalance.TagItem_Index = TagItem_Index;

        //                                TagItemIndex = BinBalance.TagItem_Index.ToString();

        //                                BinBalance.Tag_Index = Tag_Index;
        //                                BinBalance.Tag_No = data.TagNoNew;




        //                                BinBalance.Product_Index = new Guid(BinCardReserveItem.Product_Index.ToString());
        //                                BinBalance.Product_Id = BinCardReserveItem.Product_Id;
        //                                BinBalance.Product_Name = BinCardReserveItem.Product_Name;
        //                                BinBalance.Product_SecondName = BinCardReserveItem.Product_SecondName;
        //                                BinBalance.Product_ThirdName = BinCardReserveItem.Product_ThirdName;
        //                                BinBalance.Product_Lot = BinCardReserveItem.Product_Lot;
        //                                BinBalance.ItemStatus_Index = TransferItemResultSelect.ItemStatus_Index_To;
        //                                BinBalance.ItemStatus_Id = DatItemStatusTo[0].dataincolumn2;
        //                                BinBalance.ItemStatus_Name = DatItemStatusTo[0].dataincolumn3;
        //                                BinBalance.GoodsReceive_MFG_Date = BinCardReserveItem.MFG_Date;
        //                                BinBalance.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;

        //                                BinBalance.GoodsReceive_ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                                BinBalance.GoodsReceive_ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                                BinBalance.GoodsReceive_ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;
        //                                BinBalance.BinBalance_Ratio = BinBalanceResult.BinBalance_Ratio;
        //                                BinBalance.BinBalance_QtyBegin = BinCardReserveItem.BinCardReserve_QtyBal;
        //                                BinBalance.BinBalance_WeightBegin = BinCardReserveItem.BinCardReserve_WeightBal;
        //                                BinBalance.BinBalance_VolumeBegin = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                                BinBalance.BinBalance_QtyBal = BinCardReserveItem.BinCardReserve_QtyBal;
        //                                BinBalance.BinBalance_WeightBal = BinCardReserveItem.BinCardReserve_WeightBal;
        //                                BinBalance.BinBalance_VolumeBal = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                                BinBalance.BinBalance_QtyReserve = 0;
        //                                BinBalance.BinBalance_WeightReserve = 0;
        //                                BinBalance.BinBalance_VolumeReserve = 0;
        //                                BinBalance.ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                                BinBalance.ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                                BinBalance.ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;
        //                                BinBalance.UDF_1 = BinBalanceResult.UDF_1;
        //                                BinBalance.UDF_2 = BinBalanceResult.UDF_2;
        //                                BinBalance.UDF_3 = BinBalanceResult.UDF_3;
        //                                BinBalance.UDF_4 = BinBalanceResult.UDF_4;
        //                                BinBalance.UDF_5 = BinBalanceResult.UDF_5;
        //                                BinBalance.Create_By = data.Update_By;
        //                                BinBalance.Update_By = data.Update_By;
        //                                listBinBalance.Add(BinBalance);

        //                            }

        //                            ////--------------------Bin Card FROM--------------------
        //                            var BinCard = new BinCardViewModel();
        //                            BinCard.BinCard_Index = pBinCard_IndexFrom;
        //                            BinCard.Process_Index = Process_Index;//BinCardReserveItem.Process_Index;

        //                            BinCard.DocumentType_Index = DocType_Index; //BinCardReserveItem.DocumentType_Index;
        //                            BinCard.DocumentType_Id = DataDocumentType[0].dataincolumn2;//BinCardReserveItem.DocumentType_Id;
        //                            BinCard.DocumentType_Name = DataDocumentType[0].dataincolumn3;//BinCardReserveItem.DocumentType_Name;
        //                            BinCard.GoodsReceive_Index = BinCardReserveItem.GoodsReceive_Index;
        //                            BinCard.GoodsReceiveItem_Index = BinCardReserveItem.GoodsReceiveItem_Index;
        //                            BinCard.GoodsReceiveItemLocation_Index = TransferItemResultSelect.GoodsReceiveItemLocation_Index;//BinCardReserveItem.GoodsReceiveItemLocation_Index;
        //                            BinCard.BinCard_No = TransferResult[0].GoodsTransfer_No; //BinCardReserveItem.BinCard_No;
        //                            BinCard.BinCard_Date = TransferResult[0].GoodsTransfer_Date; //BinCardReserveItem.BinCard_Date;
        //                            BinCard.TagItem_Index = BinCardReserveItem.TagItem_Index;
        //                            BinCard.Tag_Index = BinCardReserveItem.Tag_Index;
        //                            BinCard.Tag_No = BinBalanceResult.Tag_No;
        //                            BinCard.Tag_Index_To = Tag_Index; //BinCardReserveItem.Tag_Index_To;
        //                            BinCard.Tag_No_To = data.TagNoNew;  //  ไม่ได้เปลี่ยนTAG
        //                            BinCard.Product_Index = BinCardReserveItem.Product_Index;
        //                            BinCard.Product_Id = BinCardReserveItem.Product_Id;
        //                            BinCard.Product_Name = BinCardReserveItem.Product_Name;
        //                            BinCard.Product_SecondName = BinCardReserveItem.Product_SecondName;
        //                            BinCard.Product_ThirdName = BinCardReserveItem.Product_ThirdName;
        //                            BinCard.Product_Index_To = BinCardReserveItem.Product_Index; //BinCardReserveItem.Product_Index_To;
        //                            BinCard.Product_Id_To = BinCardReserveItem.Product_Id;
        //                            BinCard.Product_Name_To = BinCardReserveItem.Product_Name;
        //                            BinCard.Product_SecondName_To = BinCardReserveItem.Product_SecondName;
        //                            BinCard.Product_ThirdName_To = BinCardReserveItem.Product_ThirdName;
        //                            BinCard.Product_Lot = BinCardReserveItem.Product_Lot;
        //                            BinCard.Product_Lot_To = BinCardReserveItem.Product_Lot;
        //                            BinCard.ItemStatus_Index = BinCardReserveItem.ItemStatus_Index;
        //                            BinCard.ItemStatus_Id = BinCardReserveItem.ItemStatus_Id;
        //                            BinCard.ItemStatus_Name = BinCardReserveItem.ItemStatus_Name;

        //                            BinCard.ItemStatus_Index_To = TransferItemResultSelect.ItemStatus_Index_To;
        //                            BinCard.ItemStatus_Id_To = DatItemStatusTo[0].dataincolumn2;
        //                            BinCard.ItemStatus_Name_To = DatItemStatusTo[0].dataincolumn3;

        //                            BinCard.ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                            BinCard.ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                            BinCard.ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;

        //                            BinCard.Owner_Index = BinCardReserveItem.Owner_Index;//BinCardReserveItem.Owner_Index;
        //                            BinCard.Owner_Id = BinCardReserveItem.Owner_Id;//BinCardReserveItem.Owner_Id;
        //                            BinCard.Owner_Name = BinCardReserveItem.Owner_Name; // BinCardReserveItem.Owner_Name;
        //                            BinCard.Owner_Index_To = BinCardReserveItem.Owner_Index;
        //                            BinCard.Owner_Id_To = BinCardReserveItem.Owner_Id;
        //                            BinCard.Owner_Name_To = BinCardReserveItem.Owner_Name;
        //                            BinCard.Location_Index = BinCardReserveItem.Location_Index;//BinCardReserveItem.Location_Index;
        //                            BinCard.Location_Id = BinCardReserveItem.Location_Id; //BinCardReserveItem.Location_Id;
        //                            BinCard.Location_Name = BinCardReserveItem.Location_Name;//BinCardReserveItem.Location_Name;

        //                            BinCard.Location_Index_To = new Guid(DataLocationTo[0].dataincolumn1);   //BinCardReserveItem.Location_Index;
        //                            BinCard.Location_Id_To = DataLocationTo[0].dataincolumn2; // BinCardReserveItem.Location_Id;
        //                            BinCard.Location_Name_To = DataLocationTo[0].dataincolumn3;//BinCardReserveItem.Location_Name;


        //                            BinCard.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;
        //                            BinCard.GoodsReceive_EXP_Date_To = BinCardReserveItem.EXP_Date;
        //                            BinCard.BinCard_QtyIn = 0;
        //                            BinCard.BinCard_QtyOut = BinCardReserveItem.BinCardReserve_QtyBal;
        //                            BinCard.BinCard_QtySign = BinCardReserveItem.BinCardReserve_QtyBal * -1;
        //                            BinCard.BinCard_WeightIn = 0;
        //                            BinCard.BinCard_WeightOut = BinCardReserveItem.BinCardReserve_WeightBal;

        //                            if (BinCardReserveItem.BinCardReserve_WeightBal == 0)
        //                            {
        //                                BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal;
        //                            }
        //                            else
        //                            {
        //                                BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal * -1;
        //                            }

        //                            BinCard.BinCard_VolumeIn = 0;
        //                            BinCard.BinCard_VolumeOut = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                            if (BinCardReserveItem.BinCardReserve_VolumeBal == 0)
        //                            {
        //                                BinCard.BinCard_VolumeSign = 0;
        //                            }
        //                            else
        //                            {
        //                                BinCard.BinCard_VolumeSign = BinCardReserveItem.BinCardReserve_VolumeBal * -1;

        //                            }
        //                            BinCard.Ref_Document_No = TransferResult[0].GoodsTransfer_No;
        //                            BinCard.Ref_Document_Index = BinCardReserveItem.Ref_Document_Index; //tem.Ref_Document_Index;
        //                            BinCard.Ref_DocumentItem_Index = BinCardReserveItem.Ref_DocumentItem_Index;
        //                            BinCard.Create_By = data.Update_By;
        //                            //BinCard.Create_Date = BinCardReserveItem.CreateDate;

        //                            listBinCard.Add(BinCard);


        //                            ////------------------------------------------------




        //                            ////--------------------Bin Card TO--------------------
        //                            BinCard = new BinCardViewModel();

        //                            BinCard.BinCard_Index = pBinCard_IndexTo;
        //                            BinCard.Process_Index = Process_Index;//BinCardReserveItem.Process_Index;
        //                            BinCard.DocumentType_Index = DocType_Index; //BinCardReserveItem.DocumentType_Index;
        //                            BinCard.DocumentType_Id = DataDocumentType[0].dataincolumn2;//BinCardReserveItem.DocumentType_Id;
        //                            BinCard.DocumentType_Name = DataDocumentType[0].dataincolumn3;//BinCardReserveItem.DocumentType_Name;
        //                            BinCard.GoodsReceive_Index = BinCardReserveItem.GoodsReceive_Index;
        //                            BinCard.GoodsReceiveItem_Index = BinCardReserveItem.GoodsReceiveItem_Index;
        //                            BinCard.GoodsReceiveItemLocation_Index = TransferItemResultSelect.GoodsReceiveItemLocation_Index;//BinCardReserveItem.GoodsReceiveItemLocation_Index;
        //                            BinCard.BinCard_No = TransferResult[0].GoodsTransfer_No; //BinCardReserveItem.BinCard_No;
        //                            BinCard.BinCard_Date = TransferResult[0].GoodsTransfer_Date; //BinCardReserveItem.BinCard_Date;
        //                            BinCard.TagItem_Index = new Guid(TagItemIndex);
        //                            //Tag LPN Old
        //                            BinCard.Tag_Index = Tag_Index;
        //                            BinCard.Tag_No = data.TagNoNew;
        //                            //Tag LPN New
        //                            BinCard.Tag_Index_To = Tag_Index; //Tag LPN New;
        //                            BinCard.Tag_No_To = data.TagNoNew;  //  ไม่ได้เปลี่ยนTAG

        //                            BinCard.Product_Index = BinCardReserveItem.Product_Index;
        //                            BinCard.Product_Id = BinCardReserveItem.Product_Id;
        //                            BinCard.Product_Name = BinCardReserveItem.Product_Name;
        //                            BinCard.Product_SecondName = BinCardReserveItem.Product_SecondName;
        //                            BinCard.Product_ThirdName = BinCardReserveItem.Product_ThirdName;

        //                            BinCard.Product_Index_To = BinCardReserveItem.Product_Index; //BinCardReserveItem.Product_Index_To;
        //                            BinCard.Product_Id_To = BinCardReserveItem.Product_Id;
        //                            BinCard.Product_Name_To = BinCardReserveItem.Product_Name;
        //                            BinCard.Product_SecondName_To = BinCardReserveItem.Product_SecondName;
        //                            BinCard.Product_ThirdName_To = BinCardReserveItem.Product_ThirdName;
        //                            BinCard.Product_Lot = BinCardReserveItem.Product_Lot;
        //                            BinCard.Product_Lot_To = BinCardReserveItem.Product_Lot;
        //                            BinCard.ItemStatus_Index = TransferItemResultSelect.ItemStatus_Index_To;
        //                            BinCard.ItemStatus_Id = DatItemStatusTo[0].dataincolumn2;
        //                            BinCard.ItemStatus_Name = DatItemStatusTo[0].dataincolumn3;

        //                            // Null ฝั่ง From
        //                            BinCard.ItemStatus_Index_To = TransferItemResultSelect.ItemStatus_Index_To;
        //                            BinCard.ItemStatus_Id_To = DatItemStatusTo[0].dataincolumn2;
        //                            BinCard.ItemStatus_Name_To = DatItemStatusTo[0].dataincolumn3;

        //                            BinCard.ProductConversion_Index = BinCardReserveItem.ProductConversion_Index;
        //                            BinCard.ProductConversion_Id = BinCardReserveItem.ProductConversion_Id;
        //                            BinCard.ProductConversion_Name = BinCardReserveItem.ProductConversion_Name;

        //                            BinCard.Owner_Index = BinCardReserveItem.Owner_Index;//BinCardReserveItem.Owner_Index;
        //                            BinCard.Owner_Id = BinCardReserveItem.Owner_Id;//BinCardReserveItem.Owner_Id;
        //                            BinCard.Owner_Name = BinCardReserveItem.Owner_Name; // BinCardReserveItem.Owner_Name;

        //                            BinCard.Owner_Index_To = BinCardReserveItem.Owner_Index;
        //                            BinCard.Owner_Id_To = BinCardReserveItem.Owner_Id;
        //                            BinCard.Owner_Name_To = BinCardReserveItem.Owner_Name;

        //                            //if (newData != null)
        //                            //{
        //                            //    BinCard.Owner_Index_To = newData.Owner_Index;
        //                            //    BinCard.Owner_Id_To = newData.Owner_Id;
        //                            //    BinCard.Owner_Name_To = newData.Owner_Name;
        //                            //}
        //                            //else
        //                            //{
        //                            //    BinCard.Owner_Index_To = BinCardReserveItem.Owner_Index;
        //                            //    BinCard.Owner_Id_To = BinCardReserveItem.Owner_Id;
        //                            //    BinCard.Owner_Name_To = BinCardReserveItem.Owner_Name;
        //                            //}



        //                            BinCard.Location_Index = new Guid(DataLocationTo[0].dataincolumn1);
        //                            BinCard.Location_Id = DataLocationTo[0].dataincolumn2;
        //                            BinCard.Location_Name = DataLocationTo[0].dataincolumn3;

        //                            BinCard.Location_Index_To = new Guid(DataLocationTo[0].dataincolumn1);
        //                            BinCard.Location_Id_To = DataLocationTo[0].dataincolumn2;
        //                            BinCard.Location_Name_To = DataLocationTo[0].dataincolumn3;

        //                            BinCard.GoodsReceive_EXP_Date = BinCardReserveItem.EXP_Date;
        //                            BinCard.GoodsReceive_EXP_Date_To = BinCardReserveItem.EXP_Date;
        //                            BinCard.BinCard_QtyIn = BinCardReserveItem.BinCardReserve_QtyBal;
        //                            BinCard.BinCard_QtyOut = 0;
        //                            BinCard.BinCard_QtySign = BinCardReserveItem.BinCardReserve_QtyBal;
        //                            BinCard.BinCard_WeightIn = BinCardReserveItem.BinCardReserve_WeightBal;
        //                            BinCard.BinCard_WeightOut = 0;
        //                            BinCard.BinCard_WeightSign = BinCardReserveItem.BinCardReserve_WeightBal;
        //                            BinCard.BinCard_VolumeIn = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                            BinCard.BinCard_VolumeOut = 0;
        //                            BinCard.BinCard_VolumeSign = BinCardReserveItem.BinCardReserve_VolumeBal;
        //                            BinCard.Ref_Document_No = TransferResult[0].GoodsTransfer_No;
        //                            BinCard.Ref_Document_Index = BinCardReserveItem.Ref_Document_Index; //tem.Ref_Document_Index;
        //                            BinCard.Ref_DocumentItem_Index = BinCardReserveItem.Ref_DocumentItem_Index;
        //                            BinCard.Create_By = data.Update_By;

        //                            //BinCard.Create_Date = BinCardReserveItem.CreateDate;

        //                            listBinCard.Add(BinCard);

        //                            ////------------------------------------------------


        //                        }

        //                        transaction.Commit();
        //                    }// Try Transacation
        //                    catch (Exception ex)
        //                    {
        //                        transaction.Rollback();
        //                        throw ex;
        //                        // Clear Reserve
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

        //                    var transaction2 = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {
        //                        //// Add Bincacd binbalance TAG  To Stroe 
        //                        var rowsAffected1 = context.Database.ExecuteSqlCommand("sp_Save_BinBalanceTransfer @BinBalance,@BinCard,@Tag,@TagItem", pBinBalance, pBinCard, pTag, pTagItem);

        //                        transaction2.Commit();
        //                        IsError = true;
        //                        IsBinBalanceTransfer = true;
        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transaction2.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("TransferRelocation", msglog);
        //                        throw exy;
        //                    }

        //                    //}
        //                    //else
        //                    //{
        //                    //    // Clear Reserve

        //                    //}

        //                }
        //                else
        //                {
        //                    return false;
        //                }

        //            }

        //        }// END Transaction 

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        using (var context = new TransferDbContext())
        //        {

        //            if (IsError == true && IsBinBalanceTransfer == true)
        //            {
        //                foreach (String sql in SqlClearError)
        //                {
        //                    context.Database.ExecuteSqlCommand(sql);
        //                }
        //            }

        //            else if (IsBinBalanceTransfer == false || IsBinBalance == false)
        //            {

        //                String SqlCmd = "";

        //                // delete BinBalance

        //                //SqlCmd = " Delete from wm_BinCardReserve where Convert(Varchar(200),BinCardReserve_Index)  ='" + pBinCardReserveNew.ToString() + "'";
        //                //context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from im_GoodsTransferItem where Convert(Varchar(200),GoodsTransfer_Index)  ='" + GoodTransfer_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from im_GoodsTransferItem where Convert(Varchar(200),GoodsTransfer_Index)  ='" + GoodTransfer_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);


        //                String SqlcmdUpdReserve = " Update [dbo].[wm_BinBalance]  SET " +
        //                                           "   BinBalance_QtyReserve  =  " + IsBinBalance_QtyReserve +
        //                                           "  ,BinBalance_WeightReserve  =  " + IsBinBalance_WeightReserve +
        //                                           "  ,BinBalance_VolumeReserve  =  " + IsBinBalance_VolumeReserve +
        //                                           " where Convert(Varchar(200),BinBalance_Index) ='" + IsBinBalance + "'";
        //                context.Database.ExecuteSqlCommand(SqlcmdUpdReserve);



        //                String SqlcmdUpdReserve2 = " Update [dbo].[wm_BinBalance]  SET " +
        //                                           "  BinBalance_QtyReserve  = " + bcBinBalance_QtyReserve +
        //                                           "  ,BinBalance_WeightReserve  = " + bcBinBalance_WeightReserve +
        //                                           "  ,BinBalance_VolumeReserve  = " + bcBinBalance_VolumeReserve +
        //                                           " ,Location_Index =  '" + bcLocation_Index.ToString() + "'" +
        //                                           " ,Location_Id = '" + bcLocation_Name + "'" +
        //                                           " ,Tag_Index  = '" + bcTag_Index.ToString() + "'" +
        //                                           " ,Tag_No  =  '" + bcTag_No + "'" +
        //                                           " ,Create_By  =   '" + bcCreate_By + "'" +
        //                                           " where Convert(Varchar(200),BinBalance_Index) ='" + bcBinBalance_Index + "'";
        //                context.Database.ExecuteSqlCommand(SqlcmdUpdReserve2);


        //                // delete BinBalanceTransfer

        //                //SqlCmd = " Delete from wm_BinBalance where Convert(Varchar(200),BinBalance_Index)  ='" + pBinBalance_Index.ToString() + "'";
        //                //context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from wm_BinCardReserve where Convert(Varchar(200),BinCardReserve_Index)  ='" + pBinCard_IndexTo.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from wm_BinCardReserve where Convert(Varchar(200),BinCardReserve_Index)  ='" + pBinCard_IndexFrom.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                //SqlCmd = " Delete from wm_Tag where Convert(Varchar(200),Tag_Index)  ='" + Tag_Index.ToString() + "'";
        //                //context.Database.ExecuteSqlCommand(SqlCmd);

        //                //SqlCmd = " Delete from wm_TagItem where Convert(Varchar(200),TagItem_Index)  ='" + TagItem_Index.ToString() + "'";
        //                //context.Database.ExecuteSqlCommand(SqlCmd);                       
        //            }
        //        }

        //        throw ex;
        //    }
        //}
        //public Boolean SaveData(TransferViewModel data)
        //{
        //    try
        //    {
        //        using (var context = new TransferDbContext())
        //        {

        //            //foreach (var data in dataList.listTransferItemViewModel)
        //            //{


        //            string SqlTGRI = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + data.GoodsReceiveItemIndex + "' ";
        //            var strwhereGRI = new SqlParameter("@strwhere", SqlTGRI);
        //            var GRI = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhereGRI).FirstOrDefault();


        //            string SqlTGr = " and Convert(Nvarchar(50),GoodsReceive_Index) = N'" + data.GoodsReceiveIndex + "' ";
        //            var strwhereGr = new SqlParameter("@strwhere", SqlTGr);
        //            var Gr = context.IM_GoodsReceives.FromSql("sp_GetGoodsReceiveStock @strwhere", strwhereGr).FirstOrDefault();

        //            string pstringTaskItemQ = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //            var strwhereTaskItemQ = new SqlParameter("@strwhere", pstringTaskItemQ);
        //            var TaskItemQ = context.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereTaskItemQ).FirstOrDefault();

        //            //Find new Location and new Tag From NewLPN
        //            string SqlWhereTag = " and Tag_No = N'" + data.TagNoNew + "'";
        //            var strwhereTag = new SqlParameter("@strwhere", SqlWhereTag);

        //            string SqlnewData = " and Convert(Nvarchar(50),Tag_No) = N'" + data.TagNoNew + "' ";
        //            var strwherenewData = new SqlParameter("@strwhere", SqlnewData);
        //            var newData = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwherenewData).FirstOrDefault();

        //            string pstringViewPlanCarton = " and TagOut_No = N'" + data.TagOutNo + "'";
        //            var whereViewPlanCarton = new SqlParameter("@strwhere", pstringViewPlanCarton);
        //            var ViewPlanCarton = context.View_PlanCarton.FromSql("sp_GetViewPlanCarton @strwhere", whereViewPlanCarton).FirstOrDefault();


        //            Guid Location_Index_To;
        //            String Location_Id_To;
        //            String Location_Name_To;



        //            if (newData != null)
        //            {
        //                // Check LocationStagging  
        //                var locationStagging = "14C5F85D-137D-470E-8C70-C1E535005DC3";
        //                var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //                var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),LocationType_Index)");
        //                var ColumnName3 = new SqlParameter("@ColumnName3", "Location_Id");
        //                var ColumnName4 = new SqlParameter("@ColumnName4", "Location_Name");
        //                var ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                var TableName = new SqlParameter("@TableName", "ms_Location");
        //                var Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),Location_Index)  ='" + newData.Location_Index.ToString() + "'");
        //                var DataLocationType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();


        //                if (DataLocationType.dataincolumn2.ToString() == locationStagging)
        //                {
        //                    Location_Index_To = (Guid)newData.Location_Index;
        //                    Location_Id_To = newData.Location_Id;
        //                    Location_Name_To = newData.Location_Name;

        //                    var document_status = 0;


        //                    GoodsReceiveViewModel GR = new GoodsReceiveViewModel();

        //                    //----Set Header------
        //                    var GRHeader = new GoodsReceiveViewModel();
        //                    var Documen_tPriorityStatus = 0;
        //                    var putaway_Status = 0;
        //                    var a = "";


        //                    var Goods_ReceiveRemark = "";

        //                    GRHeader.GoodsReceiveIndex = Guid.NewGuid();
        //                    GRHeader.OwnerIndex = Gr.Owner_Index; ;
        //                    GRHeader.OwnerId = Gr.Owner_Id;
        //                    GRHeader.OwnerName = Gr.Owner_Name;
        //                    GRHeader.DocumentTypeIndex = new Guid("88F043C7-F6CF-4285-B3AA-3090D7D8E664");

        //                    String Sql = " and  Convert(Nvarchar(200) ,DocumentType_Index ) = N'" + GRHeader.DocumentTypeIndex + "'  ";
        //                    var GRDocwhere = new SqlParameter("@strwhere", Sql);
        //                    var GRDoc = context.MS_DocumentType.FromSql("sp_GetDocumentType @strwhere ", GRDocwhere).FirstOrDefault();

        //                    GRHeader.DocumentTypeId = GRDoc.DocumentType_Id;
        //                    GRHeader.DocumentTypeName = GRDoc.DocumentType_Name;

        //                    var DocumentType_Index = new SqlParameter("@DocumentType_Index", GRHeader.DocumentTypeIndex.ToString());
        //                    var DocDate = new SqlParameter("@DocDate", DateTime.Now);
        //                    var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                    resultParameter.Size = 2000; // some meaningfull value
        //                    resultParameter.Direction = ParameterDirection.Output;
        //                    context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
        //                    //var result = resultParameter.Value;
        //                    GR.GoodsReceiveNo = resultParameter.Value.ToString();

        //                    GRHeader.GoodsReceiveNo = GR.GoodsReceiveNo;
        //                    GRHeader.GoodsReceiveDate = DateTime.Now;
        //                    GRHeader.DocumentRefNo1 = Gr.DocumentRef_No1;
        //                    GRHeader.DocumentRefNo2 = Gr.DocumentRef_No2;
        //                    GRHeader.DocumentRefNo3 = Gr.DocumentRef_No3;
        //                    GRHeader.DocumentRefNo4 = Gr.DocumentRef_No4;
        //                    GRHeader.DocumentRefNo5 = Gr.DocumentRef_No5;
        //                    GRHeader.DocumentStatus = document_status;
        //                    GRHeader.UDF1 = Gr.UDF_1;
        //                    GRHeader.UDF2 = Gr.UDF_2;
        //                    GRHeader.UDF3 = Gr.UDF_3;
        //                    GRHeader.UDF4 = Gr.UDF_4;
        //                    GRHeader.UDF5 = Gr.UDF_5;
        //                    GRHeader.DocumentPriorityStatus = Documen_tPriorityStatus;
        //                    GRHeader.DocumentRemark = Gr.Document_Remark;
        //                    GRHeader.Create_By = data.Update_By;
        //                    GRHeader.Create_Date = DateTime.Now;
        //                    GRHeader.WarehouseIndex = Gr.Warehouse_Index;
        //                    GRHeader.WarehouseId = Gr.Warehouse_Id;
        //                    GRHeader.WarehouseName = Gr.Warehouse_Name;
        //                    GRHeader.WarehouseIndexTo = Gr.Warehouse_Index;
        //                    GRHeader.WarehouseIdTo = Gr.Warehouse_Id;
        //                    GRHeader.WarehouseNameTo = Gr.Warehouse_Name;
        //                    GRHeader.PutawayStatus = putaway_Status;
        //                    GRHeader.DockDoorIndex = Gr.DockDoor_Index;
        //                    GRHeader.DockDoorId = Gr.DockDoor_Id;
        //                    GRHeader.DockDoorName = Gr.DockDoor_Name;
        //                    GRHeader.VehicleTypeIndex = Gr.VehicleType_Index;
        //                    GRHeader.VehicleTypeId = Gr.VehicleType_Id;
        //                    GRHeader.VehicleTypeName = Gr.VehicleType_Name;
        //                    GRHeader.ContainerTypeIndex = Gr.ContainerType_Index;
        //                    GRHeader.ContainerTypeId = Gr.ContainerType_Id;
        //                    GRHeader.ContainerTypeName = Gr.ContainerType_Name;

        //                    //----Set Detail-----
        //                    var GRDetail = new List<GoodsReceiveItemViewModel>();
        //                    int addNumber = 0;
        //                    int refDocLineNum = 0;
        //                    addNumber++;

        //                    var GRItem = new GoodsReceiveItemViewModel();

        //                    // Gen Index for line item

        //                    GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                    // Index From Header
        //                    GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;

        //                    GRItem.LineNum = addNumber.ToString(); ;

        //                    GRItem.ProductIndex = GRI.Product_Index;
        //                    GRItem.ProductId = GRI.Product_Id;
        //                    GRItem.ProductName = GRI.Product_Name;
        //                    GRItem.ProductSecondName = GRI.Product_SecondName;
        //                    GRItem.ProductThirdName = GRI.Product_ThirdName;
        //                    if (data.ProductLot != "")
        //                    {
        //                        GRItem.ProductLot = GRI.Product_Lot;
        //                    }
        //                    else
        //                    {
        //                        GRItem.ProductLot = "";
        //                    }
        //                    GRItem.ItemStatusIndex = GRI.ItemStatus_Index;
        //                    GRItem.ItemStatusId = GRI.ItemStatus_Id;
        //                    GRItem.ItemStatusName = GRI.ItemStatus_Name;
        //                    GRItem.qty = data.Qty;
        //                    GRItem.ratio = GRI.Ratio;

        //                    GRItem.TotalQty = GRItem.qty * GRItem.ratio;

        //                    GRItem.UDF1 = data.UDF1;
        //                    GRItem.ProductConversionIndex = GRI.ProductConversion_Index;
        //                    GRItem.ProductConversionId = GRI.ProductConversion_Id;
        //                    GRItem.ProductConversionName = GRI.ProductConversion_Name;
        //                    GRItem.MFGDate = GRI.MFG_Date;
        //                    GRItem.EXPDate = GRI.EXP_Date;

        //                    GRItem.UnitWeight = GRI.UnitWeight;

        //                    GRItem.Weight = GRI.Weight;

        //                    GRItem.UnitWidth = GRI.UnitWidth;

        //                    GRItem.UnitLength = GRI.UnitLength;

        //                    GRItem.UnitHeight = GRI.UnitHeight;

        //                    GRItem.UnitVolume = GRI.UnitVolume;

        //                    GRItem.Volume = GRI.Volume;

        //                    GRItem.UnitPrice = GRI.UnitPrice;

        //                    GRItem.Price = GRI.Price;

        //                    GRItem.RefDocumentNo = GRI.Ref_Document_No;

        //                    GRItem.RefDocumentLineNum = GRI.Ref_Document_LineNum;

        //                    GRItem.RefDocumentIndex = GRI.Ref_Document_Index;

        //                    GRItem.RefProcessIndex = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                    GRItem.RefDocumentItemIndex = GRI.Ref_DocumentItem_Index;
        //                    GRItem.DocumentRefNo1 = GRI.DocumentRef_No1;
        //                    GRItem.DocumentRefNo2 = GRI.DocumentRef_No2;
        //                    GRItem.DocumentRefNo3 = GRI.DocumentRef_No3;
        //                    GRItem.DocumentRefNo4 = GRI.DocumentRef_No4;
        //                    GRItem.DocumentRefNo5 = GRI.DocumentRef_No5;
        //                    GRItem.DocumentStatus = document_status;
        //                    GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                    GRItem.UDF2 = (String.IsNullOrEmpty(data.UDF2.ToString()) == true ? "" : data.UDF2);
        //                    GRItem.UDF3 = data.UDF3;
        //                    GRItem.UDF4 = data.UDF4;
        //                    GRItem.UDF5 = data.UDF5;
        //                    GRItem.GoodsReceiveRemark = "";
        //                    GRItem.GoodsReceiveDockDoor = "";
        //                    GRItem.Create_Date = DateTime.Now;
        //                    GRDetail.Add(GRItem);

        //                    var GRHeaderlist = new List<GoodsReceiveViewModel>();
        //                    GRHeaderlist.Add(GRHeader);


        //                    //-- SAVE STORE PROC ----//

        //                    DataTable CGRHeader = CreateDataTable(GRHeaderlist);
        //                    DataTable CGRDetail = CreateDataTable(GRDetail);


        //                    if (CGRHeader.Columns.Contains("listGoodsReceiveItemViewModels"))
        //                    {
        //                        CGRHeader.Columns.Remove("listGoodsReceiveItemViewModels");
        //                    }

        //                    var GoodsReceive = new SqlParameter("GoodsReceive", SqlDbType.Structured);
        //                    GoodsReceive.TypeName = "[dbo].[im_GoodsReceiveData]";
        //                    GoodsReceive.Value = CGRHeader;


        //                    var GoodsReceiveItem = new SqlParameter("GoodsReceiveItem", SqlDbType.Structured);
        //                    GoodsReceiveItem.TypeName = "[dbo].[im_GoodsReceiveItemData]";
        //                    GoodsReceiveItem.Value = CGRDetail;


        //                    var commandText2 = "EXEC sp_Save_im_GoodsReceive @GoodsReceive,@GoodsReceiveItem";
        //                    var rowsAffected2 = context.Database.ExecuteSqlCommand(commandText2, GoodsReceive, GoodsReceiveItem);





        //                    string pstring = " and BinBalance_Index = N'" + newData.BinBalance_Index + "'";
        //                    var strwhere = new SqlParameter("@strwhere", pstring);

        //                    var queryResults = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhere).ToList();

        //                    var listBinBalance = new List<BinBalanceViewModel>();
        //                    var listBinCard = new List<BinCardViewModel>();
        //                    var GRLocation = new List<GoodsReceiveItemLocationViewModel>();

        //                    foreach (var item in queryResults)
        //                    {

        //                        string pstringTaskItem = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                        var strwhereTaskItem = new SqlParameter("@strwhere", pstringTaskItem);
        //                        var TaskItem = context.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereTaskItem).FirstOrDefault();

        //                        var GRLocationResult = new GoodsReceiveItemLocationViewModel();

        //                        var GoodsReceiveItemLocationIndex = Guid.NewGuid();

        //                        GRLocationResult.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                        GRLocationResult.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                        GRLocationResult.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocationIndex;
        //                        GRLocationResult.TagItem_Index = newData.TagItem_Index;
        //                        GRLocationResult.Tag_Index = newData.Tag_Index;
        //                        GRLocationResult.Tag_No = newData.Tag_No;
        //                        GRLocationResult.Product_Index = TaskItem.Product_Index;
        //                        GRLocationResult.Product_Name = TaskItem.Product_Name;
        //                        GRLocationResult.Product_Id = TaskItem.Product_Id;
        //                        GRLocationResult.Product_Name = TaskItem.Product_Name;
        //                        GRLocationResult.Product_SecondName = TaskItem.Product_SecondName;
        //                        GRLocationResult.Product_ThirdName = TaskItem.Product_ThirdName;
        //                        GRLocationResult.Product_Lot = TaskItem.Product_Lot;
        //                        GRLocationResult.ItemStatus_Index = GRItem.ItemStatusIndex;
        //                        GRLocationResult.ItemStatus_Id = GRItem.ItemStatusId;
        //                        GRLocationResult.ItemStatus_Name = GRItem.ItemStatusName;
        //                        GRLocationResult.ProductConversion_Index = GRItem.ProductConversionIndex;
        //                        GRLocationResult.ProductConversion_Id = GRItem.ProductConversionId;
        //                        GRLocationResult.ProductConversion_Name = GRItem.ProductConversionName;
        //                        GRLocationResult.MFG_Date = GRItem.MFGDate;
        //                        GRLocationResult.EXP_Date = GRItem.EXPDate;
        //                        GRLocationResult.UnitWeight = TaskItem.UnitWeight;
        //                        GRLocationResult.Weight = TaskItem.Weight;
        //                        GRLocationResult.UnitWidth = TaskItem.UnitWidth;
        //                        GRLocationResult.UnitLength = TaskItem.UnitLength;
        //                        GRLocationResult.UnitHeight = TaskItem.UnitHeight;
        //                        GRLocationResult.UnitVolume = TaskItem.UnitVolume;
        //                        GRLocationResult.Volume = TaskItem.Volume;
        //                        GRLocationResult.UnitPrice = TaskItem.UnitPrice;
        //                        GRLocationResult.Price = TaskItem.Price;
        //                        GRLocationResult.Owner_Index = data.ownerIndex;
        //                        GRLocationResult.Owner_Id = data.ownerId;
        //                        GRLocationResult.Owner_Name = data.ownerName;
        //                        GRLocationResult.Location_Index = Location_Index_To;
        //                        GRLocationResult.Location_Id = Location_Id_To;
        //                        GRLocationResult.Location_Name = Location_Name_To;
        //                        GRLocationResult.Qty = data.Qty;
        //                        GRLocationResult.TotalQty = GRItem.TotalQty;
        //                        GRLocationResult.Ratio = TaskItem.Ratio;
        //                        GRLocationResult.UDF_1 = GRItem.UDF1;
        //                        GRLocationResult.UDF_2 = GRItem.UDF2;
        //                        GRLocationResult.UDF_3 = GRItem.UDF3;
        //                        GRLocationResult.UDF_4 = GRItem.UDF4;
        //                        GRLocationResult.UDF_5 = GRItem.UDF5;
        //                        GRLocationResult.Create_By = GRItem.Create_By;
        //                        GRLocationResult.Create_Date = GRItem.Create_Date;
        //                        GRLocationResult.Putaway_Status = 0;
        //                        GRLocationResult.Putaway_By = "";
        //                        GRLocation.Add(GRLocationResult);

        //                        var BinBalance = new BinBalanceViewModel();
        //                        ////--------------------Bin Balance --------------------

        //                        var BinBalance_Index = Guid.NewGuid();
        //                        BinBalance.BinBalance_Index = BinBalance_Index;

        //                        BinBalance.Owner_Index = GRHeader.OwnerIndex;
        //                        BinBalance.Owner_Id = GRHeader.OwnerId;
        //                        BinBalance.Owner_Name = GRHeader.OwnerName;

        //                        BinBalance.LocationIndex = Location_Index_To;
        //                        BinBalance.LocationId = Location_Id_To;
        //                        BinBalance.LocationName = Location_Name_To;

        //                        BinBalance.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                        BinBalance.GoodsReceive_No = GRHeader.GoodsReceiveNo;
        //                        BinBalance.GoodsReceive_Date = GRHeader.GoodsReceiveDate;
        //                        BinBalance.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                        BinBalance.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                        BinBalance.TagItem_Index = newData.TagItem_Index;
        //                        BinBalance.Tag_Index = newData.Tag_Index;
        //                        BinBalance.Tag_No = newData.Tag_No;

        //                        // Find Product  

        //                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Product_Index)");
        //                        ColumnName2 = new SqlParameter("@ColumnName2", "Product_Id");
        //                        ColumnName3 = new SqlParameter("@ColumnName3", "Product_Name");
        //                        ColumnName4 = new SqlParameter("@ColumnName4", "Product_SecondName");
        //                        ColumnName5 = new SqlParameter("@ColumnName5", "Product_ThirdName");
        //                        TableName = new SqlParameter("@TableName", "ms_Product");
        //                        Where = new SqlParameter("@Where", " Where Product_Id  ='" + data.ProductId + "'");
        //                        var DataProduct = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();
        //                        if (DataProduct != null)
        //                        {
        //                            BinBalance.Product_Index = new Guid(DataProduct.dataincolumn1);
        //                            BinBalance.Product_Id = DataProduct.dataincolumn2;
        //                            BinBalance.Product_Name = DataProduct.dataincolumn3;
        //                            BinBalance.Product_SecondName = DataProduct.dataincolumn4;
        //                            BinBalance.Product_ThirdName = DataProduct.dataincolumn5;
        //                        }
        //                        else
        //                        {
        //                            BinBalance.Product_Index = item.Product_Index;
        //                            BinBalance.Product_Id = item.Product_Id;
        //                            BinBalance.Product_Name = item.Product_Name;
        //                            BinBalance.Product_SecondName = item.Product_SecondName;
        //                            BinBalance.Product_ThirdName = item.Product_ThirdName;
        //                        }


        //                        BinBalance.Product_Lot = item.Product_Lot;

        //                        if (data.ItemStatusIndex_From != null)
        //                        {
        //                            BinBalance.ItemStatus_Index = data.ItemStatusIndex_From;
        //                            BinBalance.ItemStatus_Id = data.ItemStatusId_From;
        //                            BinBalance.ItemStatus_Name = data.ItemStatusName_From;
        //                        }
        //                        else
        //                        {
        //                            BinBalance.ItemStatus_Index = item.ItemStatus_Index;
        //                            BinBalance.ItemStatus_Id = item.ItemStatus_Id;
        //                            BinBalance.ItemStatus_Name = item.ItemStatus_Name;
        //                        }


        //                        BinBalance.GoodsReceive_MFG_Date = item.GoodsReceive_MFG_Date;
        //                        BinBalance.GoodsReceive_EXP_Date = item.GoodsReceive_EXP_Date;
        //                        BinBalance.GoodsReceive_ProductConversion_Index = TaskItem.ProductConversion_Index;
        //                        BinBalance.GoodsReceive_ProductConversion_Id = TaskItem.ProductConversion_Id;
        //                        BinBalance.GoodsReceive_ProductConversion_Name = TaskItem.ProductConversion_Name;
        //                        BinBalance.BinBalance_Ratio = TaskItem.Ratio;
        //                        BinBalance.BinBalance_QtyBegin = data.Qty;
        //                        BinBalance.BinBalance_WeightBegin = TaskItem.Weight;
        //                        BinBalance.BinBalance_VolumeBegin = TaskItem.Volume;
        //                        BinBalance.BinBalance_QtyBal = data.Qty;
        //                        BinBalance.BinBalance_WeightBal = TaskItem.Weight;
        //                        BinBalance.BinBalance_VolumeBal = TaskItem.Volume;
        //                        BinBalance.BinBalance_QtyReserve = 0;
        //                        BinBalance.BinBalance_WeightReserve = 0;
        //                        BinBalance.BinBalance_VolumeReserve = 0;
        //                        BinBalance.ProductConversion_Index = TaskItem.ProductConversion_Index;
        //                        BinBalance.ProductConversion_Id = TaskItem.ProductConversion_Id;
        //                        BinBalance.ProductConversion_Name = TaskItem.ProductConversion_Name;
        //                        BinBalance.UDF_1 = GRItem.UDF1;
        //                        BinBalance.UDF_2 = GRItem.UDF2;
        //                        BinBalance.UDF_3 = GRItem.UDF3;
        //                        BinBalance.UDF_4 = GRItem.UDF4;
        //                        BinBalance.UDF_5 = GRItem.UDF5;
        //                        BinBalance.Create_By = GRHeader.Create_By;
        //                        BinBalance.Create_Date = GRItem.Create_Date;
        //                        BinBalance.Update_By = GRItem.Update_By;
        //                        BinBalance.Update_Date = GRItem.Update_Date;
        //                        BinBalance.Cancel_By = GRItem.Cancel_By;
        //                        BinBalance.Cancel_Date = GRItem.Cancel_Date;


        //                        listBinBalance.Add(BinBalance);

        //                        ////--------------------Bin Card --------------------
        //                        var BinCard = new BinCardViewModel();

        //                        BinCard.BinCard_Index = Guid.NewGuid();
        //                        BinCard.Process_Index = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                        // fix DocumentType GR
        //                        BinCard.DocumentType_Index = GRHeader.DocumentTypeIndex;
        //                        BinCard.DocumentType_Id = GRHeader.DocumentTypeId;
        //                        BinCard.DocumentType_Name = GRHeader.DocumentTypeName;
        //                        BinCard.GoodsReceive_Index = GR.GoodsReceiveIndex;
        //                        BinCard.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                        BinCard.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                        BinCard.BinCard_No = GRHeader.GoodsReceiveNo;
        //                        BinCard.BinCard_Date = GRHeader.GoodsReceiveDate;
        //                        BinCard.TagItem_Index = newData.TagItem_Index;
        //                        BinCard.Tag_Index = newData.Tag_Index;
        //                        BinCard.Tag_No = newData.Tag_No;
        //                        BinCard.Tag_Index_To = newData.Tag_Index;
        //                        BinCard.Tag_No_To = newData.Tag_No;
        //                        BinCard.Product_Index = TaskItem.Product_Index;
        //                        BinCard.Product_Id = TaskItem.Product_Id;
        //                        BinCard.Product_Name = TaskItem.Product_Name;
        //                        BinCard.Product_SecondName = TaskItem.Product_SecondName;
        //                        BinCard.Product_ThirdName = TaskItem.Product_ThirdName;
        //                        BinCard.Product_Index_To = TaskItem.Product_Index;
        //                        BinCard.Product_Id_To = TaskItem.Product_Id;
        //                        BinCard.Product_Name_To = TaskItem.Product_Name;
        //                        BinCard.Product_SecondName_To = TaskItem.Product_SecondName;
        //                        BinCard.Product_ThirdName_To = TaskItem.Product_ThirdName;
        //                        BinCard.Product_Lot = TaskItem.Product_Lot;
        //                        BinCard.Product_Lot_To = GRItem.ProductLot;
        //                        BinCard.ItemStatus_Index = GRItem.ItemStatusIndex;
        //                        BinCard.ItemStatus_Id = GRItem.ItemStatusId;
        //                        BinCard.ItemStatus_Name = GRItem.ItemStatusName;
        //                        BinCard.ItemStatus_Index_To = GRItem.ItemStatusIndex;
        //                        BinCard.ItemStatus_Id_To = GRItem.ItemStatusId;
        //                        BinCard.ItemStatus_Name_To = GRItem.ItemStatusName;
        //                        BinCard.ProductConversion_Index = TaskItem.ProductConversion_Index;
        //                        BinCard.ProductConversion_Id = TaskItem.ProductConversion_Id;
        //                        BinCard.ProductConversion_Name = TaskItem.ProductConversion_Name;
        //                        BinCard.Owner_Index = GRHeader.OwnerIndex;
        //                        BinCard.Owner_Id = GRHeader.OwnerId;
        //                        BinCard.Owner_Name = GRHeader.OwnerName;

        //                        BinCard.Owner_Index_To = GRHeader.OwnerIndex;
        //                        BinCard.Owner_Id_To = GRHeader.OwnerId;
        //                        BinCard.Owner_Name_To = GRHeader.OwnerName;

        //                        BinCard.Location_Index = item.Location_Index;
        //                        BinCard.Location_Id = item.Location_Id;
        //                        BinCard.Location_Name = item.Location_Name;

        //                        BinCard.Location_Index_To = Location_Index_To;
        //                        BinCard.Location_Id_To = Location_Id_To;
        //                        BinCard.Location_Name_To = Location_Name_To;

        //                        BinCard.GoodsReceive_EXP_Date = GRItem.EXPDate;
        //                        BinCard.GoodsReceive_EXP_Date_To = GRItem.EXPDate;
        //                        BinCard.BinCard_QtyIn = data.Qty;
        //                        BinCard.BinCard_QtyOut = 0;
        //                        BinCard.BinCard_QtySign = data.Qty;
        //                        BinCard.BinCard_WeightIn = GRItem.Weight;
        //                        BinCard.BinCard_WeightOut = 0;
        //                        BinCard.BinCard_WeightSign = TaskItem.Weight;
        //                        BinCard.BinCard_VolumeIn = TaskItem.Volume;
        //                        BinCard.BinCard_VolumeOut = 0;
        //                        BinCard.BinCard_VolumeSign = TaskItem.Volume;
        //                        BinCard.Ref_Document_No = GRHeader.GoodsReceiveNo;
        //                        BinCard.Ref_Document_Index = GRHeader.GoodsReceiveIndex;
        //                        BinCard.Ref_DocumentItem_Index = GRItem.GoodsReceiveItemIndex;
        //                        BinCard.Create_By = GRItem.Create_By;
        //                        BinCard.Create_Date = GRItem.Create_Date;

        //                        listBinCard.Add(BinCard);
        //                    }

        //                    DataTable CGRLocation = CreateDataTable(GRLocation);
        //                    DataTable dtBinBalance = CreateDataTable(listBinBalance);
        //                    DataTable dtBinCard = CreateDataTable(listBinCard);

        //                    var GoodsReceiveItemLocation = new SqlParameter("GoodsReceiveItemLocation", SqlDbType.Structured);
        //                    GoodsReceiveItemLocation.TypeName = "[dbo].[im_GoodsReceiveItemLocationData]";
        //                    GoodsReceiveItemLocation.Value = CGRLocation;

        //                    var pBinBalance = new SqlParameter("BinBalance", SqlDbType.Structured);
        //                    pBinBalance.TypeName = "[dbo].[wm_BinBalanceData]";
        //                    pBinBalance.Value = dtBinBalance;

        //                    var pBinCard = new SqlParameter("BinCard", SqlDbType.Structured);
        //                    pBinCard.TypeName = "[dbo].[wm_BinCardData]";
        //                    pBinCard.Value = dtBinCard;
        //                    var rowsAffected3 = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", GoodsReceiveItemLocation, pBinBalance, pBinCard);

        //                    var contextM = new TransferDbContext();

        //                    string pstringsplitQty = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                    var strwheresplitQty = new SqlParameter("@strwhere", pstringsplitQty);
        //                    var querysplitQty = contextM.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwheresplitQty).FirstOrDefault();

        //                    if (querysplitQty.splitQty == null)
        //                    {
        //                        querysplitQty.splitQty = 0;
        //                    }

        //                    String SqlQty = " Update im_TaskItem set " +
        //                                     " splitQty  =  '" + (data.Qty + querysplitQty.splitQty) + "'" +
        //                                     " ,Qty  =  '" + (querysplitQty.Qty - data.Qty) + "'" +
        //                                     " where Convert(Varchar(200),TaskItem_Index) ='" + data.taskItemIndex + "'";
        //                    var row = context.Database.ExecuteSqlCommand(SqlQty);

        //                    var contextO = new TransferDbContext();

        //                    string pstringQty = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                    var strwhereQty = new SqlParameter("@strwhere", pstringQty);
        //                    var queryQty = contextO.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereQty).FirstOrDefault();

        //                    if (queryQty.Qty == 0)
        //                    {
        //                        String SqlcmdTagItem = " Update im_TaskItem set " +
        //                                                                     " Document_Status = -1 " +
        //                                                                     " where Convert(Varchar(200),TaskItem_Index) ='" + data.taskItemIndex + "'";
        //                        var row2 = context.Database.ExecuteSqlCommand(SqlcmdTagItem);
        //                    }


        //                    var contextT = new TransferDbContext();

        //                    //var CheckTaskItem = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem").Where(c => c.TaskItem_Index == data.taskItemIndex).FirstOrDefault();

        //                    string pstringCheckTaskItem = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                    var strwhereCheckTaskItem = new SqlParameter("@strwhere", pstringCheckTaskItem);
        //                    var CheckTaskItem = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereCheckTaskItem).FirstOrDefault();

        //                    //var CheckTask = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem").Where(c => c.Task_Index == CheckTaskItem.Task_Index && c.Document_Status == 1).ToList();

        //                    string pstringCheckTask = " and Task_Index = N'" + CheckTaskItem.Task_Index + "'" +
        //                                              " and Document_Status == 1";
        //                    var strwhereCheckTask = new SqlParameter("@strwhere", pstringCheckTaskItem);
        //                    var CheckTask = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereCheckTaskItem).ToList();


        //                    string GIstr = " and GoodsIssue_No = N'" + querysplitQty.Ref_Document_No + "'";
        //                    var GIwhere = new SqlParameter("@strwhere", GIstr);
        //                    var GI = contextT.IM_GoodsIssue.FromSql("sp_GetGoodIssue @strwhere", GIwhere).FirstOrDefault();

        //                    string TagOutstr = " and GoodsIssue_Index = N'" + GI.GoodsIssue_Index + "'";
        //                    var TagOutwhere = new SqlParameter("@strwhere", TagOutstr);
        //                    var TagOut = contextT.wm_TagOutItem.FromSql("sp_GetTagOutItem @strwhere", TagOutwhere).FirstOrDefault();

        //                    if (CheckTask.Count == 0)
        //                    {
        //                        String SqlcmdTagItem = " Update wm_TagOut set " +
        //                                                                     " TagOut_Status = -1 " +
        //                                                                     " where Convert(Varchar(200),TagOut_Index) ='" + TagOut.TagOut_Index + "'";
        //                        var row2 = context.Database.ExecuteSqlCommand(SqlcmdTagItem);
        //                    }

        //                }
        //                else
        //                {
        //                    return false;
        //                }


        //            }

        //            else
        //            {

        //                Guid Process_Index = new Guid("408FD0AF-1592-4FA7-8BA0-03F6C3215D41");
        //                Guid DocType_Index = new Guid("063BC3F4-A8F5-48EA-8B36-ECCEAD297484");

        //                var TransferDocDate = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
        //                //var oTransferDocDate = String.Format("{0:dd/MM/yyyy}", DateTime.Now);



        //                DateTime oTransferDocDate = DateTime.Now;
        //                var GoodsTransferNo = "";
        //                var IsUse = new SqlParameter("@IsUse", Guid.NewGuid().ToString());

        //                var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
        //                var ColumnName2 = new SqlParameter("@ColumnName2", "DocumentType_Id");
        //                var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Name");
        //                var ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                var ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                var TableName = new SqlParameter("@TableName", "ms_DocumentType");
        //                var Where = new SqlParameter("@Where", " Where Convert(Nvarchar(50),DocumentType_Index)  ='" + DocType_Index.ToString() + "'");
        //                var DataDocumentType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();
        //                //Location_Index_To = new Guid("5d30facb-ed0f-480a-a26d-f6b35308ee05");
        //                //Location_Id_To = "7";
        //                //Location_Name_To = "Location ST4";


        //                // Set Document No
        //                var DocumentType_Index = new SqlParameter("@DocumentType_Index", DocType_Index.ToString());
        //                var DocDate = new SqlParameter("@DocDate", TransferDocDate.ToString());
        //                var resultDocNoParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultDocNoParameter.Size = 2000; // some meaningfull value
        //                resultDocNoParameter.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultDocNoParameter);
        //                //var result = resultParameter.Value;
        //                GoodsTransferNo = resultDocNoParameter.Value.ToString();

        //                //Find new Location and new Tag From NewLPN
        //                string SqlWhereTags = " and Tag_No = N'" + data.TagNoNew + "'";
        //                var strwhereTags = new SqlParameter("@strwhere", SqlWhereTags);

        //                var WareHouse_Index = new SqlParameter("@WareHouse_Index", data.WareHouseIndex.ToString());
        //                DocumentType_Index = new SqlParameter("@DocumentType_Index", DocType_Index.ToString());
        //                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultParameter.Size = 2000; // some meaningfull value
        //                resultParameter.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_GetTranferWareHouse @WareHouse_Index,@DocumentType_Index  ,@txtReturn OUTPUT", WareHouse_Index, DocumentType_Index, resultParameter);

        //                var SuggestLocation = resultParameter.Value.ToString();

        //                ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
        //                ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
        //                ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
        //                ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                TableName = new SqlParameter("@TableName", "ms_Location");
        //                Where = new SqlParameter("@Where", " Where Location_Name  ='" + SuggestLocation.ToString() + "'");
        //                var DataLocationWH = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();


        //                Location_Index_To = new Guid(DataLocationWH[0].dataincolumn1);
        //                Location_Id_To = DataLocationWH[0].dataincolumn2;
        //                Location_Name_To = DataLocationWH[0].dataincolumn3;

        //                var document_status = 0;


        //                GoodsReceiveViewModel GR = new GoodsReceiveViewModel();

        //                //----Set Header------
        //                var GRHeader = new GoodsReceiveViewModel();
        //                var Documen_tPriorityStatus = 0;
        //                var putaway_Status = 0;
        //                var a = "";


        //                var Goods_ReceiveRemark = "";

        //                GRHeader.GoodsReceiveIndex = Guid.NewGuid();
        //                GRHeader.OwnerIndex = Gr.Owner_Index; ;
        //                GRHeader.OwnerId = Gr.Owner_Id;
        //                GRHeader.OwnerName = Gr.Owner_Name;
        //                GRHeader.DocumentTypeIndex = new Guid("88F043C7-F6CF-4285-B3AA-3090D7D8E664");

        //                String Sql = " and  Convert(Nvarchar(200) ,DocumentType_Index ) = N'" + GRHeader.DocumentTypeIndex + "'  ";
        //                var GRDocwhere = new SqlParameter("@strwhere", Sql);
        //                var GRDoc = context.MS_DocumentType.FromSql("sp_GetDocumentType @strwhere ", GRDocwhere).FirstOrDefault();

        //                GRHeader.DocumentTypeId = GRDoc.DocumentType_Id;
        //                GRHeader.DocumentTypeName = GRDoc.DocumentType_Name;

        //                var DocumentType_Indexs = new SqlParameter("@DocumentType_Index", GRHeader.DocumentTypeIndex.ToString());
        //                var DocDates = new SqlParameter("@DocDate", DateTime.Now);
        //                var resultParameters = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultParameters.Size = 2000; // some meaningfull value
        //                resultParameters.Direction = ParameterDirection.Output;
        //                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Indexs, DocDates, resultParameter);
        //                //var result = resultParameter.Value;
        //                GR.GoodsReceiveNo = resultParameter.Value.ToString();

        //                GRHeader.GoodsReceiveNo = GR.GoodsReceiveNo;
        //                GRHeader.GoodsReceiveDate = DateTime.Now;
        //                GRHeader.DocumentRefNo1 = Gr.DocumentRef_No1;
        //                GRHeader.DocumentRefNo2 = Gr.DocumentRef_No2;
        //                GRHeader.DocumentRefNo3 = Gr.DocumentRef_No3;
        //                GRHeader.DocumentRefNo4 = Gr.DocumentRef_No4;
        //                GRHeader.DocumentRefNo5 = Gr.DocumentRef_No5;
        //                GRHeader.DocumentStatus = document_status;
        //                GRHeader.UDF1 = Gr.UDF_1;
        //                GRHeader.UDF2 = Gr.UDF_2;
        //                GRHeader.UDF3 = Gr.UDF_3;
        //                GRHeader.UDF4 = Gr.UDF_4;
        //                GRHeader.UDF5 = Gr.UDF_5;
        //                GRHeader.DocumentPriorityStatus = Documen_tPriorityStatus;
        //                GRHeader.DocumentRemark = Gr.Document_Remark;
        //                GRHeader.Create_By = data.Update_By;
        //                GRHeader.Create_Date = DateTime.Now;
        //                GRHeader.WarehouseIndex = Gr.Warehouse_Index;
        //                GRHeader.WarehouseId = Gr.Warehouse_Id;
        //                GRHeader.WarehouseName = Gr.Warehouse_Name;
        //                GRHeader.WarehouseIndexTo = Gr.Warehouse_Index_To;
        //                GRHeader.WarehouseIdTo = Gr.Warehouse_Id_To;
        //                GRHeader.WarehouseNameTo = Gr.Warehouse_Name_To;
        //                GRHeader.PutawayStatus = putaway_Status;
        //                GRHeader.DockDoorIndex = Gr.DockDoor_Index;
        //                GRHeader.DockDoorId = Gr.DockDoor_Id;
        //                GRHeader.DockDoorName = Gr.DockDoor_Name;
        //                GRHeader.VehicleTypeIndex = Gr.VehicleType_Index;
        //                GRHeader.VehicleTypeId = Gr.VehicleType_Id;
        //                GRHeader.VehicleTypeName = Gr.VehicleType_Name;
        //                GRHeader.ContainerTypeIndex = Gr.ContainerType_Index;
        //                GRHeader.ContainerTypeId = Gr.ContainerType_Id;
        //                GRHeader.ContainerTypeName = Gr.ContainerType_Name;

        //                //----Set Detail-----
        //                var GRDetail = new List<GoodsReceiveItemViewModel>();
        //                int addNumber = 0;
        //                int refDocLineNum = 0;
        //                addNumber++;

        //                var GRItem = new GoodsReceiveItemViewModel();

        //                // Gen Index for line item

        //                GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                // Index From Header
        //                GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;

        //                GRItem.LineNum = addNumber.ToString();

        //                GRItem.ProductIndex = GRI.Product_Index;
        //                GRItem.ProductId = GRI.Product_Id;
        //                GRItem.ProductName = GRI.Product_Name;
        //                GRItem.ProductSecondName = GRI.Product_SecondName;
        //                GRItem.ProductThirdName = GRI.Product_ThirdName;
        //                if (data.ProductLot != "" && data.ProductLot != null)
        //                {
        //                    GRItem.ProductLot = GRI.Product_Lot;
        //                }
        //                else
        //                {
        //                    GRItem.ProductLot = "";
        //                }
        //                GRItem.ItemStatusIndex = GRI.ItemStatus_Index;
        //                GRItem.ItemStatusId = GRI.ItemStatus_Id;
        //                GRItem.ItemStatusName = GRI.ItemStatus_Name;
        //                GRItem.qty = data.Qty;
        //                GRItem.ratio = GRI.Ratio;

        //                GRItem.TotalQty = GRItem.qty * GRItem.ratio;

        //                GRItem.UDF1 = data.UDF1;
        //                GRItem.ProductConversionIndex = GRI.ProductConversion_Index;
        //                GRItem.ProductConversionId = GRI.ProductConversion_Id;
        //                GRItem.ProductConversionName = GRI.ProductConversion_Name;
        //                GRItem.MFGDate = GRI.MFG_Date;
        //                GRItem.EXPDate = GRI.EXP_Date;

        //                GRItem.UnitWeight = GRI.UnitWeight;

        //                GRItem.Weight = GRI.Weight;

        //                GRItem.UnitWidth = GRI.UnitWidth;

        //                GRItem.UnitLength = GRI.UnitLength;

        //                GRItem.UnitHeight = GRI.UnitHeight;

        //                GRItem.UnitVolume = GRI.UnitVolume;

        //                GRItem.Volume = GRI.Volume;

        //                GRItem.UnitPrice = GRI.UnitPrice;

        //                GRItem.Price = GRI.Price;

        //                GRItem.RefDocumentNo = GRI.Ref_Document_No;

        //                GRItem.RefDocumentLineNum = GRI.Ref_Document_LineNum;

        //                GRItem.RefDocumentIndex = GRI.Ref_Document_Index;

        //                GRItem.RefProcessIndex = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                GRItem.RefDocumentItemIndex = GRI.Ref_DocumentItem_Index;
        //                GRItem.DocumentRefNo1 = GRI.DocumentRef_No1;
        //                GRItem.DocumentRefNo2 = GRI.DocumentRef_No2;
        //                GRItem.DocumentRefNo3 = GRI.DocumentRef_No3;
        //                GRItem.DocumentRefNo4 = GRI.DocumentRef_No4;
        //                GRItem.DocumentRefNo5 = GRI.DocumentRef_No5;
        //                GRItem.DocumentStatus = document_status;
        //                GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                GRItem.UDF2 = (String.IsNullOrEmpty(data.UDF2.ToString()) == true ? "" : data.UDF2);
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


        //                var commandText2 = "EXEC sp_Save_im_GoodsReceive @GoodsReceive,@GoodsReceiveItem";
        //                var rowsAffected2 = context.Database.ExecuteSqlCommand(commandText2, GoodsReceive, GoodsReceiveItem);


        //                //TAG
        //                string SqlTagWhere = " and Convert(Nvarchar(50), Tag_No) = N'" + data.TagNoNew + "' ";
        //                var strTagwhere = new SqlParameter("@strwhere", SqlTagWhere);
        //                var oTag = context.wm_Tag.FromSql("sp_GetTag @strwhere ", strTagwhere).FirstOrDefault();
        //                bool isNewLPN;
        //                var PalletIndex = Guid.NewGuid();
        //                var TagIndex = Guid.NewGuid();
        //                int tagStatus = 1;
        //                var TagHeader = new TagViewModel();
        //                var TagDetail = new TagItemViewModel();


        //                if (oTag == null)
        //                {
        //                    isNewLPN = true;
        //                }
        //                else
        //                {
        //                    isNewLPN = false;
        //                }

        //                if (isNewLPN == true)
        //                {
        //                    //Create New TAG / New LPN

        //                    TagHeader.TagIndex = TagIndex;
        //                    TagHeader.TagNo = data.TagNoNew;
        //                    TagHeader.PalletIndex = PalletIndex;
        //                    TagHeader.TagRefNo1 = data.UDF1;
        //                    TagHeader.TagRefNo2 = data.UDF2;
        //                    TagHeader.TagRefNo3 = data.UDF3;
        //                    TagHeader.TagRefNo4 = data.UDF4;
        //                    TagHeader.TagRefNo5 = data.UDF5;
        //                    TagHeader.TagStatus = tagStatus;
        //                    TagHeader.UDF1 = data.UDF1;
        //                    TagHeader.UDF2 = data.UDF2;
        //                    TagHeader.UDF3 = data.UDF3;
        //                    TagHeader.UDF4 = data.UDF4;
        //                    TagHeader.UDF5 = data.UDF5;
        //                    TagHeader.CreateBy = data.Update_By;
        //                    TagHeader.CreateDate = DateTime.Today;
        //                    TagHeader.TagStatus = 1;

        //                    TagDetail = new TagItemViewModel();

        //                    TagDetail.TagIndex = TagHeader.TagIndex;
        //                    TagDetail.TagItemIndex = Guid.NewGuid();
        //                    TagDetail.TagNo = data.TagNoNew;
        //                    TagDetail.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;
        //                    TagDetail.GoodsReceiveItemIndex = GRItem.GoodsReceiveItemIndex;
        //                    TagDetail.ProductIndex = GRItem.ProductIndex;
        //                    TagDetail.ProductName = GRItem.ProductName;
        //                    TagDetail.ProductId = GRItem.ProductId;
        //                    TagDetail.ProductSecondName = GRItem.ProductSecondName;
        //                    TagDetail.ProductThirdName = GRItem.ProductThirdName;
        //                    TagDetail.ProductLot = GRItem.ProductLot;
        //                    TagDetail.ItemStatusIndex = GRItem.ItemStatusIndex;
        //                    TagDetail.ItemStatusId = GRItem.ItemStatusId;
        //                    TagDetail.ItemStatusName = GRItem.ItemStatusName;
        //                    TagDetail.Qty = GRItem.qty;
        //                    TagDetail.TotalQty = GRItem.TotalQty;
        //                    TagDetail.Ratio = GRItem.ratio;
        //                    TagDetail.ProductConversionIndex = GRItem.ProductConversionIndex;
        //                    TagDetail.ProductConversionId = GRItem.ProductConversionId;
        //                    TagDetail.ProductConversionName = GRItem.ProductConversionName;
        //                    TagDetail.Volume = GRItem.Volume;
        //                    TagDetail.Weight = GRItem.Weight;
        //                    TagDetail.MFGDate = GRItem.MFGDate;
        //                    TagDetail.EXPDate = GRItem.EXPDate;
        //                    TagDetail.TagRefNo1 = "";
        //                    TagDetail.TagRefNo2 = "";
        //                    TagDetail.TagRefNo3 = "";
        //                    TagDetail.TagRefNo4 = "";
        //                    TagDetail.TagRefNo5 = "";
        //                    TagDetail.TagStatus = 1;
        //                    TagDetail.CreateBy = data.Create_By;
        //                    TagDetail.CreateDate = DateTime.Today;

        //                }
        //                else
        //                {
        //                    // CASE OLD TAG / LPN

        //                    TagHeader = new TagViewModel();
        //                    TagIndex = oTag.Tag_Index;
        //                    TagHeader.TagIndex = oTag.Tag_Index;
        //                    TagHeader.TagNo = oTag.Tag_No;
        //                    TagHeader.PalletNo = oTag.Pallet_No;
        //                    TagHeader.PalletIndex = oTag.Pallet_Index;
        //                    TagHeader.TagRefNo1 = oTag.TagRef_No1;
        //                    TagHeader.TagRefNo2 = oTag.TagRef_No2;
        //                    TagHeader.TagRefNo3 = oTag.TagRef_No3;
        //                    TagHeader.TagRefNo4 = oTag.TagRef_No4;
        //                    TagHeader.TagRefNo5 = oTag.TagRef_No5;
        //                    TagHeader.TagStatus = 1;
        //                    TagHeader.UDF1 = oTag.UDF_1;
        //                    TagHeader.UDF2 = oTag.UDF_2;
        //                    TagHeader.UDF3 = oTag.UDF_3;
        //                    TagHeader.UDF4 = oTag.UDF_4;
        //                    TagHeader.UDF5 = oTag.UDF_5;
        //                    TagHeader.CreateBy = oTag.Create_By;

        //                    TagDetail = new TagItemViewModel();

        //                    TagDetail.TagIndex = TagHeader.TagIndex;
        //                    TagDetail.TagItemIndex = Guid.NewGuid();
        //                    TagDetail.TagNo = data.TagNoNew;
        //                    TagDetail.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;
        //                    TagDetail.GoodsReceiveItemIndex = GRItem.GoodsReceiveItemIndex;
        //                    TagDetail.ProductIndex = GRItem.ProductIndex;
        //                    TagDetail.ProductName = GRItem.ProductName;
        //                    TagDetail.ProductId = GRItem.ProductId;
        //                    TagDetail.ProductSecondName = GRItem.ProductSecondName;
        //                    TagDetail.ProductThirdName = GRItem.ProductThirdName;
        //                    TagDetail.ProductLot = GRItem.ProductLot;
        //                    TagDetail.ItemStatusIndex = GRItem.ItemStatusIndex;
        //                    TagDetail.ItemStatusId = GRItem.ItemStatusId;
        //                    TagDetail.ItemStatusName = GRItem.ItemStatusName;
        //                    TagDetail.Qty = GRItem.qty;
        //                    TagDetail.TotalQty = GRItem.TotalQty;
        //                    TagDetail.Ratio = GRItem.ratio;
        //                    TagDetail.ProductConversionIndex = GRItem.ProductConversionIndex;
        //                    TagDetail.ProductConversionId = GRItem.ProductConversionId;
        //                    TagDetail.ProductConversionName = GRItem.ProductConversionName;
        //                    TagDetail.Volume = GRItem.Volume;
        //                    TagDetail.Weight = GRItem.Weight;
        //                    TagDetail.MFGDate = GRItem.MFGDate;
        //                    TagDetail.EXPDate = GRItem.EXPDate;
        //                    TagDetail.TagRefNo1 = "";
        //                    TagDetail.TagRefNo2 = "";
        //                    TagDetail.TagRefNo3 = "";
        //                    TagDetail.TagRefNo4 = "";
        //                    TagDetail.TagRefNo5 = "";
        //                    TagDetail.TagStatus = 1;
        //                    TagDetail.CreateBy = data.Create_By;
        //                    TagDetail.CreateDate = DateTime.Today;
        //                }

        //                var THeaderlist = new List<TagViewModel>();
        //                THeaderlist.Add(TagHeader);

        //                var TDetallist = new List<TagItemViewModel>();
        //                TDetallist.Add(TagDetail);

        //                DataTable THeader = CreateDataTable(THeaderlist);
        //                DataTable TDetail = CreateDataTable(TDetallist);


        //                var Tag = new SqlParameter("Tag", SqlDbType.Structured);
        //                Tag.TypeName = "[dbo].[wm_TagTransferData]";
        //                Tag.Value = THeader;


        //                var TagItem = new SqlParameter("TagItems", SqlDbType.Structured);
        //                TagItem.TypeName = "[dbo].[wm_TagItemData]";
        //                TagItem.Value = TDetail;

        //                var Tag_Index = new SqlParameter("Tag_Index", TagIndex);
        //                //var commandText1 = "EXEC sp_Save_NewLpn @Tag,@TagItems";
        //                var commandText1 = "EXEC sp_Save_NewLpnTag @Tag,@TagItems,@Tag_Index";
        //                var rowsAffected1 = context.Database.ExecuteSqlCommand(commandText1, Tag, TagItem, Tag_Index);


        //                string pstring = " and GoodsReceiveItem_Index = N'" + data.GoodsReceiveItemIndex + "'";
        //                var strwhere = new SqlParameter("@strwhere", pstring);
        //                var queryResults = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();

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
        //                    GRLocationResult.TagItem_Index = TagDetail.TagItemIndex;
        //                    GRLocationResult.Tag_Index = TagHeader.TagIndex;
        //                    GRLocationResult.Tag_No = TagHeader.TagNo;
        //                    GRLocationResult.Product_Index = GRItem.ProductIndex;
        //                    GRLocationResult.Product_Name = GRItem.ProductName;
        //                    GRLocationResult.Product_Id = GRItem.ProductId;
        //                    GRLocationResult.Product_Name = GRItem.ProductName;
        //                    GRLocationResult.Product_SecondName = GRItem.ProductSecondName;
        //                    GRLocationResult.Product_ThirdName = GRItem.ProductThirdName;
        //                    GRLocationResult.Product_Lot = GRItem.ProductLot;
        //                    GRLocationResult.ItemStatus_Index = GRItem.ItemStatusIndex;
        //                    GRLocationResult.ItemStatus_Id = GRItem.ItemStatusId;
        //                    GRLocationResult.ItemStatus_Name = GRItem.ItemStatusName;
        //                    GRLocationResult.ProductConversion_Index = GRItem.ProductConversionIndex;
        //                    GRLocationResult.ProductConversion_Id = GRItem.ProductConversionId;
        //                    GRLocationResult.ProductConversion_Name = GRItem.ProductConversionName;
        //                    GRLocationResult.MFG_Date = GRItem.MFGDate;
        //                    GRLocationResult.EXP_Date = GRItem.EXPDate;
        //                    GRLocationResult.UnitWeight = GRItem.UnitWeight;
        //                    GRLocationResult.Weight = GRItem.Weight;
        //                    GRLocationResult.UnitWidth = GRItem.UnitWidth;
        //                    GRLocationResult.UnitLength = GRItem.UnitLength;
        //                    GRLocationResult.UnitHeight = GRItem.UnitHeight;
        //                    GRLocationResult.UnitVolume = GRItem.UnitVolume;
        //                    GRLocationResult.Volume = GRItem.Volume;
        //                    GRLocationResult.UnitPrice = GRItem.UnitPrice;
        //                    GRLocationResult.Price = GRItem.Price;
        //                    GRLocationResult.Owner_Index = GRHeader.OwnerIndex;
        //                    GRLocationResult.Owner_Id = GRHeader.OwnerId;
        //                    GRLocationResult.Owner_Name = GRHeader.OwnerName;
        //                    GRLocationResult.Location_Index = Location_Index_To;
        //                    GRLocationResult.Location_Id = Location_Id_To;
        //                    GRLocationResult.Location_Name = Location_Name_To;
        //                    GRLocationResult.Qty = data.Qty;
        //                    GRLocationResult.TotalQty = GRItem.TotalQty;
        //                    GRLocationResult.Ratio = GRItem.ratio;
        //                    GRLocationResult.UDF_1 = GRItem.UDF1;
        //                    GRLocationResult.UDF_2 = GRItem.UDF2;
        //                    GRLocationResult.UDF_3 = GRItem.UDF3;
        //                    GRLocationResult.UDF_4 = GRItem.UDF4;
        //                    GRLocationResult.UDF_5 = GRItem.UDF5;
        //                    GRLocationResult.Create_By = GRItem.Create_By;
        //                    GRLocationResult.Create_Date = GRItem.Create_Date;
        //                    GRLocationResult.Putaway_Status = 0;
        //                    GRLocationResult.Putaway_By = "";
        //                    GRLocation.Add(GRLocationResult);

        //                    var BinBalance = new BinBalanceViewModel();
        //                    ////--------------------Bin Balance --------------------

        //                    var BinBalance_Index = Guid.NewGuid();
        //                    BinBalance.BinBalance_Index = BinBalance_Index;

        //                    BinBalance.Owner_Index = GRHeader.OwnerIndex;
        //                    BinBalance.Owner_Id = GRHeader.OwnerId;
        //                    BinBalance.Owner_Name = GRHeader.OwnerName;

        //                    BinBalance.LocationIndex = Location_Index_To;
        //                    BinBalance.LocationId = Location_Id_To;
        //                    BinBalance.LocationName = Location_Name_To;

        //                    BinBalance.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                    BinBalance.GoodsReceive_No = GRHeader.GoodsReceiveNo;
        //                    BinBalance.GoodsReceive_Date = GRHeader.GoodsReceiveDate;
        //                    BinBalance.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    BinBalance.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                    BinBalance.TagItem_Index = TagDetail.TagItemIndex;
        //                    BinBalance.Tag_Index = TagHeader.TagIndex;
        //                    BinBalance.Tag_No = TagHeader.TagNo;
        //                    BinBalance.Product_Index = item.Product_Index;
        //                    BinBalance.Product_Id = item.Product_Id;
        //                    BinBalance.Product_Name = item.Product_Name;
        //                    BinBalance.Product_SecondName = item.Product_SecondName;
        //                    BinBalance.Product_ThirdName = item.Product_ThirdName;
        //                    BinBalance.Product_Lot = item.Product_Lot;
        //                    BinBalance.ItemStatus_Index = item.ItemStatus_Index;
        //                    BinBalance.ItemStatus_Id = item.ItemStatus_Id;
        //                    BinBalance.ItemStatus_Name = item.ItemStatus_Name;
        //                    BinBalance.GoodsReceive_MFG_Date = item.MFG_Date;
        //                    BinBalance.GoodsReceive_EXP_Date = item.EXP_Date;
        //                    BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
        //                    BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
        //                    BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
        //                    BinBalance.BinBalance_Ratio = item.Ratio;
        //                    BinBalance.BinBalance_QtyBegin = data.Qty;
        //                    BinBalance.BinBalance_WeightBegin = TaskItemQ.Weight;
        //                    BinBalance.BinBalance_VolumeBegin = TaskItemQ.Volume;
        //                    BinBalance.BinBalance_QtyBal = data.Qty;
        //                    BinBalance.BinBalance_WeightBal = TaskItemQ.Weight;
        //                    BinBalance.BinBalance_VolumeBal = TaskItemQ.Volume;
        //                    BinBalance.BinBalance_QtyReserve = 0;
        //                    BinBalance.BinBalance_WeightReserve = 0;
        //                    BinBalance.BinBalance_VolumeReserve = 0;
        //                    BinBalance.ProductConversion_Index = GRItem.ProductConversionIndex;
        //                    BinBalance.ProductConversion_Id = GRItem.ProductConversionId;
        //                    BinBalance.ProductConversion_Name = GRItem.ProductConversionName;
        //                    BinBalance.UDF_1 = GRItem.UDF1;
        //                    BinBalance.UDF_2 = GRItem.UDF2;
        //                    BinBalance.UDF_3 = GRItem.UDF3;
        //                    BinBalance.UDF_4 = GRItem.UDF4;
        //                    BinBalance.UDF_5 = GRItem.UDF5;
        //                    BinBalance.Create_By = GRHeader.Create_By;
        //                    BinBalance.Create_Date = GRItem.Create_Date;
        //                    BinBalance.Update_By = GRItem.Update_By;
        //                    BinBalance.Update_Date = GRItem.Update_Date;
        //                    BinBalance.Cancel_By = GRItem.Cancel_By;
        //                    BinBalance.Cancel_Date = GRItem.Cancel_Date;


        //                    listBinBalance.Add(BinBalance);

        //                    ////--------------------Bin Card --------------------
        //                    var BinCard = new BinCardViewModel();

        //                    BinCard.BinCard_Index = Guid.NewGuid();
        //                    BinCard.Process_Index = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                    // fix DocumentType GR
        //                    BinCard.DocumentType_Index = GRHeader.DocumentTypeIndex;
        //                    BinCard.DocumentType_Id = GRHeader.DocumentTypeId;
        //                    BinCard.DocumentType_Name = GRHeader.DocumentTypeName;
        //                    BinCard.GoodsReceive_Index = GR.GoodsReceiveIndex;
        //                    BinCard.GoodsReceiveItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    BinCard.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                    BinCard.BinCard_No = GRHeader.GoodsReceiveNo;
        //                    BinCard.BinCard_Date = GRHeader.GoodsReceiveDate;
        //                    BinCard.TagItem_Index = TagDetail.TagItemIndex;
        //                    BinCard.Tag_Index = TagHeader.TagIndex;
        //                    BinCard.Tag_No = TagHeader.TagNo;
        //                    BinCard.Tag_Index_To = TagHeader.TagIndex;
        //                    BinCard.Tag_No_To = TagDetail.TagNo;
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

        //                    BinCard.Location_Index = Location_Index_To;
        //                    BinCard.Location_Id = Location_Id_To;
        //                    BinCard.Location_Name = Location_Name_To;

        //                    BinCard.Location_Index_To = Location_Index_To;
        //                    BinCard.Location_Id_To = Location_Id_To;
        //                    BinCard.Location_Name_To = Location_Name_To;

        //                    BinCard.GoodsReceive_EXP_Date = GRItem.EXPDate;
        //                    BinCard.GoodsReceive_EXP_Date_To = GRItem.EXPDate;
        //                    BinCard.BinCard_QtyIn = data.Qty;
        //                    BinCard.BinCard_QtyOut = 0;
        //                    BinCard.BinCard_QtySign = data.Qty;
        //                    BinCard.BinCard_WeightIn = GRItem.Weight;
        //                    BinCard.BinCard_WeightOut = 0;
        //                    BinCard.BinCard_WeightSign = GRItem.Weight;
        //                    BinCard.BinCard_VolumeIn = GRItem.Volume;
        //                    BinCard.BinCard_VolumeOut = 0;
        //                    BinCard.BinCard_VolumeSign = GRItem.Volume;
        //                    BinCard.Ref_Document_No = GRHeader.GoodsReceiveNo;
        //                    BinCard.Ref_Document_Index = GRHeader.GoodsReceiveIndex;
        //                    BinCard.Ref_DocumentItem_Index = GRItem.GoodsReceiveItemIndex;
        //                    BinCard.Create_By = GRItem.Create_By;
        //                    BinCard.Create_Date = GRItem.Create_Date;

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
        //                var rowsAffected3 = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", GoodsReceiveItemLocation, pBinBalance, pBinCard);

        //                var contextM = new TransferDbContext();

        //                string pstringsplitQty = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                var strwheresplitQty = new SqlParameter("@strwhere", pstringsplitQty);
        //                var querysplitQty = contextM.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwheresplitQty).FirstOrDefault();

        //                if (querysplitQty.splitQty == null)
        //                {
        //                    querysplitQty.splitQty = 0;
        //                }

        //                String SqlQty = " Update im_TaskItem set " +
        //                                 " splitQty  =  '" + (data.Qty + querysplitQty.splitQty) + "'" +
        //                                 " ,Qty  =  '" + (querysplitQty.Qty - data.Qty) + "'" +
        //                                 " where Convert(Varchar(200),TaskItem_Index) ='" + data.taskItemIndex + "'";
        //                var row = context.Database.ExecuteSqlCommand(SqlQty);

        //                var contextO = new TransferDbContext();

        //                string pstringQty = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                var strwhereQty = new SqlParameter("@strwhere", pstringQty);
        //                var queryQty = contextO.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereQty).FirstOrDefault();

        //                if (queryQty.Qty == 0)
        //                {
        //                    String SqlcmdTagItem = " Update im_TaskItem set " +
        //                                                                 " Document_Status = -1 " +
        //                                                                 " where Convert(Varchar(200),TaskItem_Index) ='" + data.taskItemIndex + "'";
        //                    var row2 = context.Database.ExecuteSqlCommand(SqlcmdTagItem);
        //                }

        //                var contextT = new TransferDbContext();

        //                //var CheckTaskItem = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem").Where(c => c.TaskItem_Index == data.taskItemIndex).FirstOrDefault();

        //                //var CheckTask = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem").Where(c => c.Task_Index == CheckTaskItem.Task_Index && c.Document_Status == 1).ToList();


        //                string pstringCheckTaskItem = " and TaskItem_Index = N'" + data.taskItemIndex + "'";
        //                var strwhereCheckTaskItem = new SqlParameter("@strwhere", pstringCheckTaskItem);
        //                var CheckTaskItem = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereCheckTaskItem).FirstOrDefault();

        //                //var CheckTask = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem").Where(c => c.Task_Index == CheckTaskItem.Task_Index && c.Document_Status == 1).ToList();

        //                string pstringCheckTask = " and Task_Index = N'" + CheckTaskItem.Task_Index + "'" +
        //                                          " and Document_Status == 1";
        //                var strwhereCheckTask = new SqlParameter("@strwhere", pstringCheckTaskItem);
        //                var CheckTask = contextT.im_TaskListItem.FromSql("sp_GetTaskListItem @strwhere", strwhereCheckTaskItem).ToList();


        //                string GIstr = " and GoodsIssue_No = N'" + querysplitQty.Ref_Document_No + "'";
        //                var GIwhere = new SqlParameter("@strwhere", GIstr);
        //                var GI = contextT.IM_GoodsIssue.FromSql("sp_GetGoodIssue @strwhere", GIwhere).FirstOrDefault();

        //                string TagOutstr = " and GoodsIssue_Index = N'" + GI.GoodsIssue_Index + "'";
        //                var TagOutwhere = new SqlParameter("@strwhere", TagOutstr);
        //                var TagOut = contextT.wm_TagOutItem.FromSql("sp_GetTagOutItem @strwhere", TagOutwhere).FirstOrDefault();

        //                if (CheckTask.Count == 0)
        //                {
        //                    String SqlcmdTagItem = " Update wm_TagOut set " +
        //                                                                 " TagOut_Status = -1 " +
        //                                                                 " where Convert(Varchar(200),TagOut_Index) ='" + TagOut.TagOut_Index + "'";
        //                    var row2 = context.Database.ExecuteSqlCommand(SqlcmdTagItem);
        //                }

        //            }
        //            //}

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
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
