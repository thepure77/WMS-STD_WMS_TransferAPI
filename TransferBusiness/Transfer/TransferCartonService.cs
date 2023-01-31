using Comone.Utils;
using DataAccess;
using GIDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TransferDataAccess.Models;

namespace TransferBusiness.Transfer
{
    public class TransferCartonService
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

        public actionResultTransferViewModel ScanCartonNo(TransferViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var actionResult = new actionResultTransferViewModel();
                    var result = new List<TransferCartonViewModel>();
                    var result1 = new List<TransferCartonViewModel>();
                    string SqlWhere = "";
                    string SqlWhere1 = "";

                    if (data.TagOutNo != null && data.TagOutNo != "")
                    {
                        SqlWhere1 = " and TagOut_No = N'" + data.TagOutNo + "'" + " and TagOut_Status not in  (-1) ";
                    }
                    var strwhere1 = new SqlParameter("@strwhere1", SqlWhere1);
                    var chkData = context.wm_TagOut.FromSql("sp_GetTagOut @strwhere1", strwhere1).Where(c => c.TagOut_No != null).FirstOrDefault();


                    if (data.TagOutNo != null && data.TagOutNo != "")
                    {
                        SqlWhere = " and TagOut_No = N'" + data.TagOutNo + "'" +
                        " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                        " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') ";

                        //SqlWhere = " and TagOut_No = N'" + data.TagOutNo + "'";

                    }
                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var check = context.View_TranferCartonV2.FromSql("sp_GetViewTranferCartonV2 @strwhere", strwhere).Where(c => c.TagOut_No != null).FirstOrDefault();

                    if (check == null)
                    {
                        actionResult.itemsUse = result.ToList();
                        actionResult.CheckData = result1.ToList();

                        return actionResult;
                    }

                    var TagOut_No = new SqlParameter("@TagOut_No", data.TagOutNo);
                    var queryResult = context.Get_CartonRelocation.FromSql("EXEC sp_Get_CartonRelocation @TagOut_No", TagOut_No).ToList();

                    if (queryResult.Count > 0)
                    {
                        foreach (var item in queryResult)
                        {
                            var resultItem = new TransferCartonViewModel();

                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_SecondName;
                            resultItem.ProductIndex = item.Product_Index;
                            resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionIndex = item.ProductConversion_Index;
                            resultItem.ProductConversionName = item.ProductConversion_Name;


                            string Sqlview = " and Convert(Nvarchar(50),TagOut_No) = N'" + data.TagOutNo + "' ";
                            var strwhereview = new SqlParameter("@strwhere", Sqlview);
                            var view = context.View_TranferCartonV2.FromSql("sp_GetViewTranferCartonV2 @strwhere", strwhereview).FirstOrDefault();

                            resultItem.locationIndex = view.Location_Index;
                            resultItem.locationId = view.Location_Id;
                            resultItem.locationName = view.Location_Name;
                            resultItem.GoodsIssueIndex = view.GoodsIssue_Index;
                            resultItem.TagOutNo = view.TagOut_No;
                            result.Add(resultItem);

                        }
                    }
                    if (chkData != null)
                    {
                        var resultItem = new TransferCartonViewModel();
                        resultItem.TagOutIndex = chkData.TagOut_Index;
                        resultItem.TagOutNo = chkData.TagOut_No;
                        result1.Add(resultItem);
                    }
                    actionResult.itemsUse = result.ToList();
                    actionResult.CheckData = result1.ToList();

                    return actionResult;
                    //return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TransferViewModel> ScanTagNo(string TagNo)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var result = new List<TransferViewModel>();
                    var resultItem = new TransferViewModel();
                    string SqlWhere = "";
                    if (TagNo != null && TagNo != "")
                    {
                        SqlWhere = " and Tag_No = N'" + TagNo + "'" + " and Tag_Status <> -1 ";
                    }
                    //var strwhere = new SqlParameter("@strwhere", SqlWhere);

                    string pstring = " and Tag_No ='" + TagNo + "'";
                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
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
                    int LPNNum = TagNo.Length;

                    if (LPNNum == FormatLPNNum)
                    {
                        var FormatText = DataDocumentType.dataincolumn2.Length;
                        var FormatDate = DataDocumentType.dataincolumn3.Length;
                        var FormatRunning = DataDocumentType.dataincolumn4.Length;
                        var LPNText = TagNo.Substring(0, FormatText);
                        var LPNDate = TagNo.Substring(FormatText, FormatDate);
                        var LPNRunning = TagNo.Substring((FormatDate + FormatText), FormatRunning);
                        var chekNumeric = TagNo.Substring(FormatText, (FormatRunning + FormatDate));
                        var isNumeric = int.TryParse(chekNumeric, out int n);

                        //เช๊ค Format_Text
                        if (LPNText.Length != FormatText)
                        {
                            resultItem.Tag_No = "false";
                            result.Add(resultItem);
                        }
                        //เช๊ค Format_Date
                        else if (LPNDate.Length != FormatDate)
                        {
                            resultItem.Tag_No = "false";
                            result.Add(resultItem);
                        }
                        //เช๊ค Format_Running
                        else if (LPNRunning.Length != FormatRunning)
                        {
                            resultItem.Tag_No = "false";
                            result.Add(resultItem);
                        }
                        //เช๊ค 3 ตัวหน้าตรงกับ Format_Text หรือป่าว
                        else if (LPNText != DataDocumentType.dataincolumn2)
                        {
                            resultItem.Tag_No = "false";
                            result.Add(resultItem);
                        }
                        //เช๊ค Formate_Date && Formate_Running เป็นตัวเลขหรือป่าว
                        else if (isNumeric != true)
                        {
                            resultItem.Tag_No = "false";
                            result.Add(resultItem);
                        }

                        else
                        {
                            var queryResult = context.wm_TagItem.FromSql("sp_GetTagItem @strwhere", strwhere).ToList();
                            if (queryResult.Count > 0)
                            {
                                foreach (var item in queryResult)
                                {

                                    resultItem.Tag_Index = item.Tag_Index;
                                    resultItem.Tag_No = item.Tag_No;
                                    resultItem.Tag_Status = item.Tag_Status;
                                    resultItem.Create_By = item.Create_By;
                                    resultItem.Update_By = item.Update_By;
                                    result.Add(resultItem);

                                }
                            }
                            return result;
                        }
                    }
                    else
                    {
                        resultItem.Tag_No = "false";
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

                    //if (data.productConversionBarcode == null)
                    //{
                    //string SqlWhere = "";
                    //string SqlTatOutNo = " and TagOutPick_No in (select Ref_Document_No from wm_TagOut where TagOut_No = '" + data.TagOutNo + "' and TagOut_Status not in (0, -1))";
                    //var strwhereTatOutNo = new SqlParameter("@strwhere", SqlTatOutNo);
                    //var queryResult = context.View_SumTranferCarton.FromSql("sp_GetViewSumTranferCarton @strwhere", strwhereTatOutNo).ToList();

                    var TagOut_No = new SqlParameter("@TagOut_No", data.TagOutNo);

                    var queryResult = context.Get_CartonRelocation.FromSql("EXEC sp_Get_CartonRelocation @TagOut_No", TagOut_No)
                        .GroupBy(c => new { c.Product_SecondName, c.ProductConversion_Name, })
                        .Select(c => new { c.Key.Product_SecondName, c.Key.ProductConversion_Name, SumPickQty = c.Sum(s => s.Picking_Qty) }).ToList();


                    var CheckShortwave = context.sp_Get_CheckCartonShortwave.FromSql("EXEC sp_Get_CheckCartonShortwave @TagOut_No", TagOut_No).FirstOrDefault();

                    var PlanGoodsIssue_No = new SqlParameter("@PlanGoodsIssue_No", CheckShortwave.PlanGoodsIssue_No);
                    var Zone_Index = new SqlParameter("@Zone_Index", CheckShortwave.Zone_Index);

                    var queryResults = context.sp_Get_CartonShortwave.FromSql("EXEC sp_Get_CartonShortwave @PlanGoodsIssue_No,@Zone_Index", PlanGoodsIssue_No, Zone_Index)
                        .GroupBy(c => new { c.Product_SecondName, c.ProductConversion_Name, })
                        .Select(c => new { c.Key.Product_SecondName, c.Key.ProductConversion_Name, SumPickQty = c.Sum(s => s.Picking_Qty) }).ToList();


                    var result = new List<SumQtyBinbalanceViewModel>();
                    if (queryResult.Count > 0)
                    {
                        foreach (var item in queryResult.GroupBy(c => c.Product_SecondName).ToList())
                        {
                            var resultItem = new SumQtyBinbalanceViewModel();

                            resultItem.ProductName = item.FirstOrDefault().Product_SecondName;
                            resultItem.productConversionName = item.FirstOrDefault().ProductConversion_Name;
                            resultItem.BinBalanceQtyBal = item.FirstOrDefault().SumPickQty;

                            result.Add(resultItem);
                        }
                    }

                    if (queryResults.Count > 0)
                    {
                        foreach (var item in queryResults.GroupBy(c => c.Product_SecondName).ToList())
                        {
                            var resultItem = new SumQtyBinbalanceViewModel();

                            resultItem.ProductName = item.FirstOrDefault().Product_SecondName;
                            resultItem.productConversionName = item.FirstOrDefault().ProductConversion_Name;
                            resultItem.BinBalanceQtyBal = item.FirstOrDefault().SumPickQty;

                            result.Add(resultItem);
                        }
                    }



                    return result;
                    


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultTransferViewModel CheckCarton(TransferCartonViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var result = new List<TransferViewModel>();
                    var resultNew = new List<GroupViewModel>();
                    var actionResultLPN = new actionResultTransferViewModel();
                    string SqlWhere = "";
                    if (data.GoodsIssueIndex != null)
                    {
                        SqlWhere = "  and GoodsIssue_Index = '" + data.GoodsIssueIndex.ToString() + "'";

                    }

                    var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    var queryResult = context.IM_GoodsIssueItem.FromSql("sp_GetGoodsIssueItem @strwhere", strwhere).ToList();

                    var group = queryResult
                        .GroupBy(c => new
                        {
                            c.Product_Name,
                            c.Product_Id,
                            c.ItemStatus_Index,
                            c.ItemStatus_Id,
                            c.ItemStatus_Name,
                            c.ProductConversion_Index,
                            c.ProductConversion_Id,
                            c.ProductConversion_Name,
                            c.EXP_Date,
                            c.UDF_1,
                            c.UDF_2,
                            c.UDF_3,
                            c.Qty
                        })
                        .Select(c => new
                        {
                            c.Key.Product_Name,
                            c.Key.Product_Id,
                            c.Key.ItemStatus_Index,
                            c.Key.ItemStatus_Id,
                            c.Key.ItemStatus_Name,
                            c.Key.ProductConversion_Index,
                            c.Key.ProductConversion_Id,
                            c.Key.ProductConversion_Name,
                            c.Key.EXP_Date,
                            c.Key.UDF_1,
                            c.Key.UDF_2,
                            c.Key.UDF_3,
                            c.Key.Qty,
                        }).ToList();

                    //if (group.Count > 0)
                    //{
                    if (group.Count > 0)
                    {
                        foreach (var item in group)
                        {
                            var resultItem = new GroupViewModel();
                            resultItem.ExpireDate = String.Format("{0:dd/MM/yyyy}", item.EXP_Date);
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_Name;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.Qty = item.Qty;
                            resultNew.Add(resultItem);
                        }
                    }



                    actionResultLPN.itemsGroup = resultNew.ToList();

                    return actionResultLPN;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultTransferViewModel CheckCartonList(TransferViewModel data)
        {
            try
            {
                using (var context = new TransferDbContext())
                {
                    var result = new List<TransferViewModel>();
                    var resultNew = new List<GroupViewModel>();
                    var actionResultLPN = new actionResultTransferViewModel();

                    //string SqlWhere = " and GoodsIssue_Index = N'" + data.GoodsIssueIndex + "'  and TagOut_Status = 1 and TagOut_Status != -1";
                    string SqlWhere = "";
                    string SqlWhere1 = "";

                    //if (data.ProductConversionBarcode == null || data.ProductConversionBarcode == "")
                    //{
                    //    if (data.TagOutNo != null && data.TagOutNo != "")
                    //    {
                    //        //SqlWhere = " and TagOut_No = '" + data.TagOutNo + "'" +
                    //        //" and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'"+
                    //        //" and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') ";
                    //    }
                    //}

                    //else
                    //{
                    //    //SqlWhere = " and TagOut_No = '" + data.TagOutNo + "'" +
                    //    //    " and Convert(Nvarchar(50),Owner_Index) = N'" + data.ownerIndex + "'" +
                    //    //    " and Location_Index in (select Location_Index from ms_Location where Convert(Nvarchar(50), Warehouse_Index) = N'" + data.WareHouseIndex + "') " +
                    //    //    "and ProductConversionBarcode = '" + data.ProductConversionBarcode + "'";
                    //}


                    //var strwhere = new SqlParameter("@strwhere", SqlWhere);
                    //var queryResult = context.View_TranferCartonV2.FromSql("sp_GetViewTranferCartonV2 @strwhere", strwhere).Where(c => c.TagOut_No != null).ToList();

                    var TagOut_No = new SqlParameter("@TagOut_No", data.TagOutNo);

                    


                    var queryResult = context.Get_CartonRelocation.FromSql("EXEC sp_Get_CartonRelocation @TagOut_No", TagOut_No).ToList();

                    var CheckShortwave = context.sp_Get_CheckCartonShortwave.FromSql("EXEC sp_Get_CheckCartonShortwave @TagOut_No", TagOut_No).FirstOrDefault();

                    var PlanGoodsIssue_No = new SqlParameter("@PlanGoodsIssue_No", CheckShortwave.PlanGoodsIssue_No);
                    var Zone_Index = new SqlParameter("@Zone_Index", CheckShortwave.Zone_Index);


                    var queryResults = context.sp_Get_CartonShortwave.FromSql("EXEC sp_Get_CartonShortwave @PlanGoodsIssue_No,@Zone_Index", PlanGoodsIssue_No, Zone_Index).ToList();

                    if (data.TagOutNo != null)
                    {
                        foreach (var item in queryResult)
                        {
                            var resultItem = new GroupViewModel();

                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_SecondName;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.Qty = item.Picking_Qty;
                            resultItem.ExpireDate = item.EXP_Date.toString();
                            resultItem.UDF1 = item.UDF_1;
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.UDF4 = item.UDF_4;
                            resultItem.UDF5 = item.UDF_5;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.itemStatusId = item.ItemStatus_Id;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;

                            string Sqlview = " and Convert(Nvarchar(50),TagOut_No) = N'" + data.TagOutNo + "' ";
                            var strwhereview = new SqlParameter("@strwhere", Sqlview);
                            var view = context.View_TranferCartonV2.FromSql("sp_GetViewTranferCartonV2 @strwhere", strwhereview).FirstOrDefault();

                            resultItem.GoodsReceiveIndex = view.GoodsReceive_Index;
                            resultItem.GoodsReceiveItem_Index = view.GoodsReceiveItem_Index;
                            resultItem.TaskItemIndex = view.TaskItem_Index;
                            resultItem.GoodsIssueIndex = view.GoodsIssue_Index;
                            resultItem.LocationId = view.Location_Id;
                            resultItem.LocationIndex = view.Location_Index;
                            resultItem.LocationName = view.Location_Name;

                            resultNew.Add(resultItem);
                        }
                    }
                    else
                    {
                        var resultItem = new GroupViewModel();
                        resultNew.Add(resultItem);
                    }

                    if (queryResult.Count > 0)
                    {
                        foreach (var item in queryResult)
                        {
                            var resultItem = new TransferViewModel();
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_SecondName;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionIndex = item.ProductConversion_Index;
                            resultItem.Qty = item.Picking_Qty;
                            resultItem.ExpireDate = item.EXP_Date.toString();
                            resultItem.UDF2 = item.UDF_2;
                            resultItem.UDF3 = item.UDF_3;
                            resultItem.UDF4 = item.UDF_4;
                            resultItem.UDF5 = item.UDF_5;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            resultItem.ItemStatusId_To = item.ItemStatus_Id;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;

                            string Sqlview = " and Convert(Nvarchar(50),TagOut_No) = N'" + data.TagOutNo + "' ";
                            var strwhereview = new SqlParameter("@strwhere", Sqlview);
                            var view = context.View_TranferCartonV2.FromSql("sp_GetViewTranferCartonV2 @strwhere", strwhereview).FirstOrDefault();

                            resultItem.GoodsIssueIndex = view.GoodsIssue_Index;
                            resultItem.GoodsReceiveIndex = view.GoodsReceive_Index;
                            resultItem.GoodsReceiveItemIndex = view.GoodsReceiveItem_Index;
                            resultItem.taskItemIndex = view.TaskItem_Index;

                            result.Add(resultItem);
                        }
                    }

                    if (queryResults.Count > 0)
                    {
                        foreach (var item in queryResults)
                        {
                            var resultItem = new GroupViewModel();

                            //resultItem.ProductIndex = item.Product_Index;
                            resultItem.ProductId = item.Product_Id;
                            resultItem.ProductName = item.Product_SecondName;
                            //resultItem.ProductConversionName = item.ProductConversion_Index;
                            //resultItem.ProductConversionId = item.ProductConversion_Id;
                            resultItem.ProductConversionName = item.ProductConversion_Name;
                            resultItem.itemStatusIndex = item.ItemStatus_Index;
                            //resultItem.ItemStatusId = item.ItemStatus_Id;
                            resultItem.ItemStatusName_From = item.ItemStatus_Name;
                            resultItem.ExpireDate = item.EXP_Date.toString();
                            resultItem.Qty = item.Picking_Qty;
                            resultNew.Add(resultItem);

                            actionResultLPN.msgResult = "ShortWave :  Product ID " + item.Product_Id + " Qty " + item.Picking_Qty;


                        }
                    }

                    actionResultLPN.itemsGroup = resultNew.ToList();
                    actionResultLPN.itemsLPN = result.ToList();



                    return actionResultLPN;
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

        //    Boolean IsBi = false;

        //    Boolean IsTag = false;



        //    Guid Gr_Index = Guid.NewGuid();
        //    Guid Tag_Index = Guid.NewGuid();


        //    try
        //    {
        //        //using (var context = new TransferDbContext())
        //        //{

        //        var TagOut = new wm_TagOut();

        //        var View = new View_TranferCartonV2();

        //        var GRI = new IM_GoodsReceiveItem();

        //        var Gr = new IM_GoodsReceive();

        //        using (var context = new TransferDbContext())
        //        {

        //            string SqlTagOut = " and Convert(Nvarchar(50),TagOut_No) = N'" + data.tagOutNo + "' ";
        //            var strwhereTagOut = new SqlParameter("@strwhere", SqlTagOut);
        //            TagOut = context.wm_TagOut.FromSql("sp_GetTagOut @strwhere", strwhereTagOut).FirstOrDefault();
        //        }

        //        using (var context = new TransferDbContext())
        //        {

        //            string SqlView = " and Convert(Nvarchar(50),TagOut_No) = N'" + data.tagOutNo + "' ";
        //            var strwhereView = new SqlParameter("@strwhere", SqlView);
        //            View = context.View_TranferCartonV2.FromSql("sp_GetViewTranferCartonV2 @strwhere", strwhereView).FirstOrDefault();
        //        }
        //        using (var context = new TransferDbContext())
        //        {

        //            string SqlGRI = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + View.GoodsReceiveItem_Index + "' ";
        //            var strwhereGRI = new SqlParameter("@strwhere", SqlGRI);
        //            GRI = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhereGRI).FirstOrDefault();
        //        }
        //        using (var context = new TransferDbContext())
        //        {

        //            string SqlGr = " and Convert(Nvarchar(50),GoodsReceive_Index) = N'" + View.GoodsReceive_Index + "' ";
        //            var strwhereGr = new SqlParameter("@strwhere", SqlGr);
        //            Gr = context.IM_GoodsReceives.FromSql("sp_GetGoodsReceiveStock @strwhere", strwhereGr).FirstOrDefault();
        //        }
        //        using (var context = new TransferDbContext())
        //        {

        //            //Find new Location and new Tag From NewLPN
        //            string SqlWhereTag = " and Tag_No = N'" + data.tagNoNew + "'";
        //            var strwhereTag = new SqlParameter("@strwhere", SqlWhereTag);
        //            var newData = context.wm_BinBalance.FromSql("sp_GetBinBalance @strwhere", strwhereTag).FirstOrDefault();


        //            Guid Location_Index_To;
        //            String Location_Id_To;
        //            String Location_Name_To;

        //            string pstringViewPlanCarton = " and TagOut_No = N'" + data.tagOutNo + "'";
        //            var whereViewPlanCarton = new SqlParameter("@strwhere", pstringViewPlanCarton);
        //            var ViewPlanCarton = context.View_PlanCarton.FromSql("sp_GetViewPlanCarton @strwhere", whereViewPlanCarton).FirstOrDefault();

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

        //                    GRHeader.GoodsReceiveIndex = Gr_Index;
        //                    GRHeader.OwnerIndex = Gr.Owner_Index; ;
        //                    GRHeader.OwnerId = Gr.Owner_Id;
        //                    GRHeader.OwnerName = Gr.Owner_Name;
        //                    GRHeader.DocumentTypeIndex = new Guid("D6E29707-48EF-4EBA-B440-2D548D5926E7");

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
        //                    GRHeader.GoodsReceiveDate = DateTime.Today;
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
        //                    GRHeader.Create_By = data.UpdateBy;
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


        //                    var TagOut_No = new SqlParameter("@TagOut_No", data.tagOutNo);

        //                    var queryResult = context.Get_CartonRelocation.FromSql("EXEC sp_Get_CartonRelocation @TagOut_No", TagOut_No).ToList();


        //                    var CheckShortwave = context.sp_Get_CheckCartonShortwave.FromSql("EXEC sp_Get_CheckCartonShortwave @TagOut_No", TagOut_No).FirstOrDefault();

        //                    var PlanGoodsIssue_No = new SqlParameter("@PlanGoodsIssue_No", CheckShortwave.PlanGoodsIssue_No);
        //                    var Zone_Index = new SqlParameter("@Zone_Index", CheckShortwave.Zone_Index);


        //                    var Results = context.sp_Get_CartonShortwave.FromSql("EXEC sp_Get_CartonShortwave @PlanGoodsIssue_No,@Zone_Index", PlanGoodsIssue_No, Zone_Index).ToList();


        //                    var GRDetail = new List<GoodsReceiveItemViewModel>();
        //                    var listTagItem = new List<TagItemViewModel>();

        //                    foreach (var item in queryResult)
        //                    {

        //                        string SqlProductConversion = " and Convert(Nvarchar(50),Product_Index) = N'" + item.Product_Index + "'" +
        //                                                      "and ProductConversion_Ratio = 1";
        //                        var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                        var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();


        //                        //----Set Detail-----
        //                        int addNumber = 0;
        //                        int refDocLineNum = 0;
        //                        addNumber++;

        //                        var GRItem = new GoodsReceiveItemViewModel();

        //                        var TagItem = new TagItemViewModel();

        //                        // Gen Index for line item

        //                        GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                        // Index From Header
        //                        GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;

        //                        GRItem.LineNum = addNumber.ToString(); ;

        //                        GRItem.ProductIndex = item.Product_Index;
        //                        GRItem.ProductId = item.Product_Id;
        //                        GRItem.ProductName = item.Product_Name;
        //                        GRItem.ProductSecondName = item.Product_SecondName;
        //                        GRItem.ProductThirdName = item.Product_ThirdName;
        //                        if (data.ProductLot != "")
        //                        {
        //                            GRItem.ProductLot = item.Product_Lot;
        //                        }
        //                        else
        //                        {
        //                            GRItem.ProductLot = "";
        //                        }
        //                        GRItem.ItemStatusIndex = item.ItemStatus_Index;
        //                        GRItem.ItemStatusId = item.ItemStatus_Id;
        //                        GRItem.ItemStatusName = item.ItemStatus_Name;
        //                        GRItem.qty = item.Picking_Qty;
        //                        GRItem.ratio = 1;

        //                        GRItem.TotalQty = item.Picking_TotalQty;

        //                        GRItem.UDF1 = data.UDF1;

        //                        if (ProductConversion != null)
        //                        {
        //                            GRItem.ProductConversionIndex = ProductConversion.ProductConversion_Index;
        //                            GRItem.ProductConversionId = ProductConversion.ProductConversion_Id;
        //                            GRItem.ProductConversionName = ProductConversion.ProductConversion_Name;
        //                            GRItem.MFGDate = item.MFG_Date;
        //                        }
        //                        else
        //                        {
        //                            GRItem.ProductConversionIndex = item.ProductConversion_Index;
        //                            GRItem.ProductConversionId = item.ProductConversion_Id;
        //                            GRItem.ProductConversionName = item.ProductConversion_Name;
        //                            GRItem.MFGDate = item.MFG_Date;
        //                        }
                                
        //                        GRItem.EXPDate = item.EXP_Date;

        //                        GRItem.UnitWeight = item.UnitWeight;

        //                        GRItem.Weight = item.Weight;

        //                        GRItem.UnitWidth = item.UnitWidth;

        //                        GRItem.UnitLength = item.UnitLength;

        //                        GRItem.UnitHeight = item.UnitHeight;

        //                        GRItem.UnitVolume = item.UnitVolume;

        //                        GRItem.Volume = item.Volume;

        //                        GRItem.UnitPrice = item.UnitPrice;

        //                        GRItem.Price = item.Price;

        //                        GRItem.RefDocumentNo = ViewPlanCarton.PlanGoodsIssue_No;

        //                        GRItem.RefDocumentLineNum = "";

        //                        GRItem.RefDocumentIndex = ViewPlanCarton.PlanGoodsIssue_Index;

        //                        GRItem.RefProcessIndex = new Guid("E1BA271A-F6E6-459D-B3EA-E4ACC522BD84");
        //                        GRItem.RefDocumentItemIndex = new Guid("00000000-0000-0000-0000-000000000000");
        //                        GRItem.DocumentRefNo1 = item.DocumentRef_No1;
        //                        GRItem.DocumentRefNo2 = item.DocumentRef_No2;
        //                        GRItem.DocumentRefNo3 = item.DocumentRef_No3;
        //                        GRItem.DocumentRefNo4 = item.DocumentRef_No4;
        //                        GRItem.DocumentRefNo5 = item.DocumentRef_No5;
        //                        GRItem.DocumentStatus = document_status;
        //                        GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                        GRItem.UDF2 = item.UDF_2;
        //                        GRItem.UDF3 = item.UDF_3;
        //                        GRItem.UDF4 = item.UDF_4;
        //                        GRItem.UDF5 = item.UDF_5;
        //                        GRItem.GoodsReceiveRemark = "";
        //                        GRItem.GoodsReceiveDockDoor = "";
        //                        GRItem.Create_By = data.CreateBy;
        //                        GRItem.Create_Date = DateTime.Now;
        //                        GRDetail.Add(GRItem);

        //                        //---------------- Create TagItem ----------------//


        //                        var TagItemIndex = Guid.NewGuid(); ;

        //                        TagItem.TagItemIndex = TagItemIndex;
        //                        TagItem.TagIndex = newData.Tag_Index;
        //                        TagItem.TagNo = data.tagNoNew;
        //                        TagItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;
        //                        TagItem.GoodsReceiveItemIndex = GRItem.GoodsReceiveItemIndex;
        //                        TagItem.ProductIndex = GRItem.ProductIndex;
        //                        TagItem.ProductId = GRItem.ProductId;
        //                        TagItem.ProductName = GRItem.ProductName;
        //                        TagItem.ProductLot = GRItem.ProductLot;
        //                        TagItem.ProductSecondName = GRItem.ProductSecondName;
        //                        TagItem.ProductThirdName = GRItem.ProductThirdName;
        //                        TagItem.ItemStatusIndex = GRItem.ItemStatusIndex;
        //                        TagItem.ItemStatusId = GRItem.ItemStatusId;
        //                        TagItem.ItemStatusName = GRItem.ItemStatusName;
        //                        TagItem.Qty = GRItem.qty;
        //                        TagItem.Ratio = GRItem.ratio;
        //                        TagItem.TotalQty = GRItem.TotalQty;
        //                        TagItem.ProductConversionIndex = GRItem.ProductConversionIndex;
        //                        TagItem.ProductConversionId = GRItem.ProductConversionId;
        //                        TagItem.ProductConversionName = GRItem.ProductConversionName;
        //                        TagItem.Weight = GRItem.Weight;
        //                        TagItem.Volume = GRItem.Volume;
        //                        TagItem.MFGDate = GRItem.MFGDate;
        //                        TagItem.EXPDate = GRItem.EXPDate;
        //                        TagItem.TagRefNo1 = "";
        //                        TagItem.TagRefNo2 = "";
        //                        TagItem.TagRefNo3 = "";
        //                        TagItem.TagRefNo4 = "";
        //                        TagItem.TagRefNo5 = "";
        //                        TagItem.TagStatus = 2;
        //                        TagItem.UDF1 = GRItem.UDF1;
        //                        TagItem.UDF2 = GRItem.UDF2;
        //                        TagItem.UDF3 = GRItem.UDF3;
        //                        TagItem.UDF4 = GRItem.UDF4;
        //                        TagItem.UDF5 = GRItem.UDF5;
        //                        TagItem.CreateBy = data.CreateBy;
        //                        TagItem.CreateDate = DateTime.Today;
        //                        listTagItem.Add(TagItem);



        //                    }

        //                    if (Results.Count > 0)
        //                    {
        //                        foreach (var item in Results)
        //                        {
        //                            string SqlProductConversion = " and Convert(Nvarchar(50),Product_Index) = N'" + item.Product_Index + "'" +
        //                                                          "and ProductConversion_Ratio = 1";
        //                            var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                            var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();



        //                            //----Set Detail-----
        //                            int addNumber = 0;
        //                            int refDocLineNum = 0;
        //                            addNumber++;

        //                            var GRItem = new GoodsReceiveItemViewModel();

        //                            var TagItem = new TagItemViewModel();

        //                            // Gen Index for line item

        //                            GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                            // Index From Header
        //                            GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;

        //                            GRItem.LineNum = addNumber.ToString(); ;

        //                            GRItem.ProductIndex = item.Product_Index;
        //                            GRItem.ProductId = item.Product_Id;
        //                            GRItem.ProductName = item.Product_Name;
        //                            GRItem.ProductSecondName = item.Product_SecondName;
        //                            GRItem.ProductThirdName = item.Product_ThirdName;
        //                            if (data.ProductLot != "")
        //                            {
        //                                GRItem.ProductLot = item.Product_Lot;
        //                            }
        //                            else
        //                            {
        //                                GRItem.ProductLot = "";
        //                            }
        //                            GRItem.ItemStatusIndex = item.ItemStatus_Index;
        //                            GRItem.ItemStatusId = item.ItemStatus_Id;
        //                            GRItem.ItemStatusName = item.ItemStatus_Name;
        //                            GRItem.qty = item.Picking_TotalQty;
        //                            GRItem.ratio = 1;

        //                            GRItem.TotalQty = item.Picking_TotalQty;

        //                            GRItem.UDF1 = data.UDF1;
        //                            if (ProductConversion != null)
        //                            {
        //                                GRItem.ProductConversionIndex = ProductConversion.ProductConversion_Index;
        //                                GRItem.ProductConversionId = ProductConversion.ProductConversion_Id;
        //                                GRItem.ProductConversionName = ProductConversion.ProductConversion_Name;
        //                                GRItem.MFGDate = item.MFG_Date;
        //                            }
        //                            else
        //                            {
        //                                GRItem.ProductConversionIndex = item.ProductConversion_Index;
        //                                GRItem.ProductConversionId = item.ProductConversion_Id;
        //                                GRItem.ProductConversionName = item.ProductConversion_Name;
        //                                GRItem.MFGDate = item.MFG_Date;
        //                            }
        //                            GRItem.MFGDate = item.MFG_Date;
        //                            GRItem.EXPDate = item.EXP_Date;

        //                            GRItem.UnitWeight = item.UnitWeight;

        //                            GRItem.Weight = item.Weight;

        //                            GRItem.UnitWidth = item.UnitWidth;

        //                            GRItem.UnitLength = item.UnitLength;

        //                            GRItem.UnitHeight = item.UnitHeight;

        //                            GRItem.UnitVolume = item.UnitVolume;

        //                            GRItem.Volume = item.Volume;

        //                            GRItem.UnitPrice = item.UnitPrice;

        //                            GRItem.Price = item.Price;

        //                            GRItem.RefDocumentNo = ViewPlanCarton.PlanGoodsIssue_No;

        //                            GRItem.RefDocumentLineNum = "";

        //                            GRItem.RefDocumentIndex = ViewPlanCarton.PlanGoodsIssue_Index;

        //                            GRItem.RefProcessIndex = new Guid("E1BA271A-F6E6-459D-B3EA-E4ACC522BD84");
        //                            GRItem.RefDocumentItemIndex = new Guid("00000000-0000-0000-0000-000000000000");
        //                            GRItem.DocumentRefNo1 = "";
        //                            GRItem.DocumentRefNo2 = "";
        //                            GRItem.DocumentRefNo3 = "";
        //                            GRItem.DocumentRefNo4 = "";
        //                            GRItem.DocumentRefNo5 = "";
        //                            GRItem.DocumentStatus = document_status;
        //                            GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                            GRItem.UDF2 = "";
        //                            GRItem.UDF3 = "";
        //                            GRItem.UDF4 = "";
        //                            GRItem.UDF5 = "";
        //                            GRItem.GoodsReceiveRemark = "";
        //                            GRItem.GoodsReceiveDockDoor = "";
        //                            GRItem.Create_By = data.CreateBy;
        //                            GRItem.Create_Date = DateTime.Now;
        //                            GRDetail.Add(GRItem);

        //                            //---------------- Create TagItem ----------------//


        //                            var TagItemIndex = Guid.NewGuid(); ;

        //                            TagItem.TagItemIndex = TagItemIndex;
        //                            TagItem.TagIndex = newData.Tag_Index;
        //                            TagItem.TagNo = data.tagNoNew;
        //                            TagItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;
        //                            TagItem.GoodsReceiveItemIndex = GRItem.GoodsReceiveItemIndex;
        //                            TagItem.ProductIndex = GRItem.ProductIndex;
        //                            TagItem.ProductId = GRItem.ProductId;
        //                            TagItem.ProductName = GRItem.ProductName;
        //                            TagItem.ProductLot = GRItem.ProductLot;
        //                            TagItem.ProductSecondName = GRItem.ProductSecondName;
        //                            TagItem.ProductThirdName = GRItem.ProductThirdName;
        //                            TagItem.ItemStatusIndex = GRItem.ItemStatusIndex;
        //                            TagItem.ItemStatusId = GRItem.ItemStatusId;
        //                            TagItem.ItemStatusName = GRItem.ItemStatusName;
        //                            TagItem.Qty = GRItem.qty;
        //                            TagItem.Ratio = GRItem.ratio;
        //                            TagItem.TotalQty = GRItem.TotalQty;
        //                            TagItem.ProductConversionIndex = GRItem.ProductConversionIndex;
        //                            TagItem.ProductConversionId = GRItem.ProductConversionId;
        //                            TagItem.ProductConversionName = GRItem.ProductConversionName;
        //                            TagItem.Weight = GRItem.Weight;
        //                            TagItem.Volume = GRItem.Volume;
        //                            TagItem.MFGDate = GRItem.MFGDate;
        //                            TagItem.EXPDate = GRItem.EXPDate;
        //                            TagItem.TagRefNo1 = "";
        //                            TagItem.TagRefNo2 = "";
        //                            TagItem.TagRefNo3 = "";
        //                            TagItem.TagRefNo4 = "";
        //                            TagItem.TagRefNo5 = "";
        //                            TagItem.TagStatus = 2;
        //                            TagItem.UDF1 = GRItem.UDF1;
        //                            TagItem.UDF2 = GRItem.UDF2;
        //                            TagItem.UDF3 = GRItem.UDF3;
        //                            TagItem.UDF4 = GRItem.UDF4;
        //                            TagItem.UDF5 = GRItem.UDF5;
        //                            TagItem.CreateBy = data.CreateBy;
        //                            TagItem.CreateDate = DateTime.Today;
        //                            listTagItem.Add(TagItem);
        //                        }

        //                    }


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

        //                    DataTable CTag = CreateDataTable(listTagItem);

        //                    var ResultsTagItem = new SqlParameter("TagItem", SqlDbType.Structured);
        //                    ResultsTagItem.TypeName = "[dbo].[wm_TransferTagItemData]";
        //                    ResultsTagItem.Value = CTag;


        //                    var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {
        //                        var commandText2 = "EXEC sp_Save_im_GoodsReceive @GoodsReceive,@GoodsReceiveItem";
        //                        var rowsAffected2 = context.Database.ExecuteSqlCommand(commandText2, GoodsReceive, GoodsReceiveItem);

        //                        var commandTextTagItem = "EXEC sp_Save_im_TransferTagItem @TagItem";
        //                        var rowsAffectedTagItem = context.Database.ExecuteSqlCommand(commandTextTagItem, ResultsTagItem);

        //                        transaction.Commit();
        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transaction.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("CartonRelocation", msglog);
        //                        throw exy;
        //                    }

        //                    string pstring = " and GoodsReceive_Index = N'" + GRHeader.GoodsReceiveIndex + "'";
        //                    var strwhere = new SqlParameter("@strwhere", pstring);
        //                    var queryResults = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();

        //                    var listBinBalance = new List<BinBalanceViewModel>();
        //                    var listBinCard = new List<BinCardViewModel>();
        //                    var GRLocation = new List<GoodsReceiveItemLocationViewModel>();

        //                    foreach (var item in queryResults)
        //                    {

        //                        string SqlProductConversion = " and Convert(Nvarchar(50),ProductConversion_Index) = N'" + item.ProductConversion_Index + "' ";
        //                        var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                        var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();

        //                        string SqlTagItem = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + item.GoodsReceiveItem_Index + "' ";
        //                        var strwhereTagItem = new SqlParameter("@strwhere", SqlTagItem);
        //                        var Item = context.wm_TagItem.FromSql("sp_GetTagItem @strwhere", strwhereTagItem).FirstOrDefault();

        //                        var GRLocationResult = new GoodsReceiveItemLocationViewModel();

        //                        var GoodsReceiveItemLocationIndex = Guid.NewGuid();

        //                        GRLocationResult.GoodsReceive_Index = item.GoodsReceive_Index;
        //                        GRLocationResult.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //                        GRLocationResult.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocationIndex;
        //                        GRLocationResult.TagItem_Index = Item.TagItem_Index;
        //                        GRLocationResult.Tag_Index = Item.Tag_Index;
        //                        GRLocationResult.Tag_No = Item.Tag_No;
        //                        GRLocationResult.Product_Index = item.Product_Index;
        //                        GRLocationResult.Product_Name = item.Product_Name;
        //                        GRLocationResult.Product_Id = item.Product_Id;
        //                        GRLocationResult.Product_Name = item.Product_Name;
        //                        GRLocationResult.Product_SecondName = item.Product_SecondName;
        //                        GRLocationResult.Product_ThirdName = item.Product_ThirdName;
        //                        GRLocationResult.Product_Lot = item.Product_Lot;
        //                        GRLocationResult.ItemStatus_Index = item.ItemStatus_Index;
        //                        GRLocationResult.ItemStatus_Id = item.ItemStatus_Id;
        //                        GRLocationResult.ItemStatus_Name = item.ItemStatus_Name;
        //                        GRLocationResult.ProductConversion_Index = item.ProductConversion_Index;
        //                        GRLocationResult.ProductConversion_Id = item.ProductConversion_Id;
        //                        GRLocationResult.ProductConversion_Name = item.ProductConversion_Name;
        //                        GRLocationResult.MFG_Date = item.MFG_Date;
        //                        GRLocationResult.EXP_Date = item.EXP_Date;
        //                        GRLocationResult.UnitWeight = ProductConversion.ProductConversion_Weight;
        //                        GRLocationResult.Weight = item.Weight;
        //                        GRLocationResult.UnitWidth = ProductConversion.ProductConversion_Width;
        //                        GRLocationResult.UnitLength = ProductConversion.ProductConversion_Length;
        //                        GRLocationResult.UnitHeight = ProductConversion.ProductConversion_Height;
        //                        GRLocationResult.UnitVolume = ProductConversion.ProductConversion_Volume;
        //                        GRLocationResult.Volume = item.Volume;
        //                        GRLocationResult.UnitPrice = 0;
        //                        GRLocationResult.Price = 0;
        //                        GRLocationResult.Owner_Index = GRHeader.OwnerIndex;
        //                        GRLocationResult.Owner_Id = GRHeader.OwnerId;
        //                        GRLocationResult.Owner_Name = GRHeader.OwnerName;
        //                        GRLocationResult.Location_Index = Location_Index_To;
        //                        GRLocationResult.Location_Id = Location_Id_To;
        //                        GRLocationResult.Location_Name = Location_Name_To;
        //                        GRLocationResult.Qty = item.Qty;
        //                        GRLocationResult.TotalQty = item.TotalQty;
        //                        GRLocationResult.Ratio = item.Ratio;
        //                        GRLocationResult.UDF_1 = item.UDF_1;
        //                        GRLocationResult.UDF_2 = item.UDF_2;
        //                        GRLocationResult.UDF_3 = item.UDF_3;
        //                        GRLocationResult.UDF_4 = item.UDF_4;
        //                        GRLocationResult.UDF_5 = item.UDF_5;
        //                        GRLocationResult.Create_By = item.Create_By;
        //                        GRLocationResult.Create_Date = item.Create_Date;
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
        //                        BinBalance.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //                        BinBalance.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                        BinBalance.TagItem_Index = Item.TagItem_Index;
        //                        BinBalance.Tag_Index = Item.Tag_Index;
        //                        BinBalance.Tag_No = Item.Tag_No;
        //                        BinBalance.Product_Index = item.Product_Index;
        //                        BinBalance.Product_Id = item.Product_Id;
        //                        BinBalance.Product_Name = item.Product_Name;
        //                        BinBalance.Product_SecondName = item.Product_SecondName;
        //                        BinBalance.Product_ThirdName = item.Product_ThirdName;
        //                        BinBalance.Product_Lot = item.Product_Lot;
        //                        BinBalance.ItemStatus_Index = item.ItemStatus_Index;
        //                        BinBalance.ItemStatus_Id = item.ItemStatus_Id;
        //                        BinBalance.ItemStatus_Name = item.ItemStatus_Name;
        //                        BinBalance.GoodsReceive_MFG_Date = Item.MFG_Date;
        //                        BinBalance.GoodsReceive_EXP_Date = Item.EXP_Date;
        //                        BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
        //                        BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
        //                        BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
        //                        BinBalance.BinBalance_Ratio = item.Ratio;
        //                        BinBalance.BinBalance_QtyBegin = item.Qty;
        //                        BinBalance.BinBalance_WeightBegin = item.Weight;
        //                        BinBalance.BinBalance_VolumeBegin = item.Volume;
        //                        BinBalance.BinBalance_QtyBal = item.Qty;
        //                        BinBalance.BinBalance_WeightBal = item.Weight;
        //                        BinBalance.BinBalance_VolumeBal = item.Volume;
        //                        BinBalance.BinBalance_QtyReserve = 0;
        //                        BinBalance.BinBalance_WeightReserve = 0;
        //                        BinBalance.BinBalance_VolumeReserve = 0;
        //                        BinBalance.ProductConversion_Index = item.ProductConversion_Index;
        //                        BinBalance.ProductConversion_Id = item.ProductConversion_Id;
        //                        BinBalance.ProductConversion_Name = item.ProductConversion_Name;
        //                        BinBalance.UDF_1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                        if (item.UDF_2 == null)
        //                        {
        //                            BinBalance.UDF_2 = "";
        //                        }
        //                        else
        //                        {
        //                            BinBalance.UDF_2 = item.UDF_2;
        //                        }
        //                        if (item.UDF_3 == null)
        //                        {
        //                            BinBalance.UDF_3 = "";
        //                        }
        //                        else
        //                        {
        //                            BinBalance.UDF_3 = item.UDF_3;
        //                        }
        //                        if (item.UDF_4 == null)
        //                        {
        //                            BinBalance.UDF_4 = "";
        //                        }
        //                        else
        //                        {
        //                            BinBalance.UDF_4 = item.UDF_4;
        //                        }
        //                        if (item.UDF_5 == null)
        //                        {
        //                            BinBalance.UDF_5 = "";
        //                        }
        //                        else
        //                        {
        //                            BinBalance.UDF_5 = item.UDF_5;
        //                        }
        //                        BinBalance.Create_By = GRHeader.Create_By;
        //                        BinBalance.Create_Date = item.Create_Date;



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
        //                        BinCard.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //                        BinCard.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                        BinCard.BinCard_No = GRHeader.GoodsReceiveNo;
        //                        BinCard.BinCard_Date = GRHeader.GoodsReceiveDate;
        //                        BinCard.TagItem_Index = Item.TagItem_Index;
        //                        BinCard.Tag_Index = Item.Tag_Index;
        //                        BinCard.Tag_No = Item.Tag_No;
        //                        BinCard.Tag_Index_To = Item.Tag_Index;
        //                        BinCard.Tag_No_To = Item.Tag_No;
        //                        BinCard.Product_Index = item.Product_Index;
        //                        BinCard.Product_Id = item.Product_Id;
        //                        BinCard.Product_Name = item.Product_Name;
        //                        BinCard.Product_SecondName = item.Product_SecondName;
        //                        BinCard.Product_ThirdName = item.Product_ThirdName;
        //                        BinCard.Product_Index_To = item.Product_Index;
        //                        BinCard.Product_Id_To = item.Product_Id;
        //                        BinCard.Product_Name_To = item.Product_Name;
        //                        BinCard.Product_SecondName_To = item.Product_SecondName;
        //                        BinCard.Product_ThirdName_To = item.Product_ThirdName;
        //                        BinCard.Product_Lot = item.Product_Lot;
        //                        BinCard.Product_Lot_To = item.Product_Lot;
        //                        BinCard.ItemStatus_Index = item.ItemStatus_Index;
        //                        BinCard.ItemStatus_Id = item.ItemStatus_Id;
        //                        BinCard.ItemStatus_Name = item.ItemStatus_Name;
        //                        BinCard.ItemStatus_Index_To = item.ItemStatus_Index;
        //                        BinCard.ItemStatus_Id_To = item.ItemStatus_Id;
        //                        BinCard.ItemStatus_Name_To = item.ItemStatus_Name;
        //                        BinCard.ProductConversion_Index = item.ProductConversion_Index;
        //                        BinCard.ProductConversion_Id = item.ProductConversion_Id;
        //                        BinCard.ProductConversion_Name = item.ProductConversion_Name;
        //                        BinCard.Owner_Index = GRHeader.OwnerIndex;
        //                        BinCard.Owner_Id = GRHeader.OwnerId;
        //                        BinCard.Owner_Name = GRHeader.OwnerName;

        //                        BinCard.Owner_Index_To = GRHeader.OwnerIndex;
        //                        BinCard.Owner_Id_To = GRHeader.OwnerId;
        //                        BinCard.Owner_Name_To = GRHeader.OwnerName;

        //                        BinCard.Location_Index = Location_Index_To;
        //                        BinCard.Location_Id = Location_Id_To;
        //                        BinCard.Location_Name = Location_Name_To;

        //                        BinCard.Location_Index_To = Location_Index_To;
        //                        BinCard.Location_Id_To = Location_Id_To;
        //                        BinCard.Location_Name_To = Location_Name_To;

        //                        BinCard.GoodsReceive_EXP_Date = item.EXP_Date;
        //                        BinCard.GoodsReceive_EXP_Date_To = item.EXP_Date;
        //                        BinCard.BinCard_QtyIn = item.TotalQty;
        //                        BinCard.BinCard_QtyOut = 0;
        //                        BinCard.BinCard_QtySign = item.TotalQty;
        //                        BinCard.BinCard_WeightIn = item.Weight;
        //                        BinCard.BinCard_WeightOut = 0;
        //                        BinCard.BinCard_WeightSign = item.Weight;
        //                        BinCard.BinCard_VolumeIn = item.Volume;
        //                        BinCard.BinCard_VolumeOut = 0;
        //                        BinCard.BinCard_VolumeSign = item.Volume;
        //                        BinCard.Ref_Document_No = GRHeader.GoodsReceiveNo;
        //                        BinCard.Ref_Document_Index = GRHeader.GoodsReceiveIndex;
        //                        BinCard.Ref_DocumentItem_Index = item.GoodsReceiveItem_Index;
        //                        BinCard.Create_By = item.Create_By;
        //                        BinCard.Create_Date = item.Create_Date;

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


        //                    var transaction2 = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {
        //                        //// Add Bincacd binbalance TAG  To Stroe 
        //                        var rowsAffected3 = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", GoodsReceiveItemLocation, pBinBalance, pBinCard);

        //                        String SqlcmdTag = " Update wm_TagOut set " +
        //                      " TagOut_Status = -1 " +
        //                      " where Convert(Varchar(200),Ref_Document_No) ='" + TagOut.Ref_Document_No + "'";
        //                        var row = context.Database.ExecuteSqlCommand(SqlcmdTag);


        //                        //String SqlcmdTaskItem = " Update im_TaskItem set " +
        //                        //                     " Document_Status = -1 " +
        //                        //                     " where Convert(Varchar(200),Task_Index) ='" + View.Task_Index + "'";
        //                        //var row2 = context.Database.ExecuteSqlCommand(SqlcmdTaskItem);


        //                        transaction2.Commit();

        //                        IsBi = true;

        //                        return "Success";

        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transaction2.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("CartonRelocation", msglog);
        //                        throw exy;
        //                    }


        //                }
        //                else
        //                {
        //                    return "false";
        //                }

        //            }

        //            else
        //            {

        //                //Location_Index_To = new Guid("5d30facb-ed0f-480a-a26d-f6b35308ee05");
        //                //Location_Id_To = "7";
        //                //Location_Name_To = "Location ST4";
        //                string SqlTag = " and Tag_No = N'" + data.tagNoNew + "'";
        //                var whereTag = new SqlParameter("@strwhere", SqlTag);
        //                var checkTag = context.wm_Tag.FromSql("sp_GetTag @strwhere", whereTag).FirstOrDefault();

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
        //                string SqlWhereTags = " and Tag_No = N'" + data.tagNoNew + "'";
        //                var strwhereTags = new SqlParameter("@strwhere", SqlWhereTags);

        //                var WareHouse_Index = new SqlParameter("@WareHouse_Index", data.WarehouseIndex.ToString());
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

        //                GRHeader.GoodsReceiveIndex = Gr_Index;
        //                GRHeader.OwnerIndex = Gr.Owner_Index; ;
        //                GRHeader.OwnerId = Gr.Owner_Id;
        //                GRHeader.OwnerName = Gr.Owner_Name;
        //                GRHeader.DocumentTypeIndex = new Guid("D6E29707-48EF-4EBA-B440-2D548D5926E7");

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
        //                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Indexs, DocDates, resultParameters);
        //                var result = resultParameters.Value;


        //                GR.GoodsReceiveNo = resultParameters.Value.ToString();
        //                GRHeader.GoodsReceiveNo = GR.GoodsReceiveNo;
        //                GRHeader.GoodsReceiveDate = DateTime.Today;
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
        //                GRHeader.Create_By = data.UpdateBy;
        //                GRHeader.Create_Date = DateTime.Now;
        //                GRHeader.WarehouseIndex = Gr.Warehouse_Index;
        //                GRHeader.WarehouseId = Gr.Warehouse_Id;
        //                GRHeader.WarehouseName = Gr.Warehouse_Name;
        //                GRHeader.WarehouseIndexTo = Gr.Warehouse_Index;
        //                GRHeader.WarehouseIdTo = Gr.Warehouse_Id;
        //                GRHeader.WarehouseNameTo = Gr.Warehouse_Name;
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


        //                var TagOut_No = new SqlParameter("@TagOut_No", data.tagOutNo);

        //                var queryResult = context.Get_CartonRelocation.FromSql("EXEC sp_Get_CartonRelocation @TagOut_No", TagOut_No).ToList();

        //                var CheckShortwave = context.sp_Get_CheckCartonShortwave.FromSql("EXEC sp_Get_CheckCartonShortwave @TagOut_No", TagOut_No).FirstOrDefault();

        //                var PlanGoodsIssue_No = new SqlParameter("@PlanGoodsIssue_No", CheckShortwave.PlanGoodsIssue_No);
        //                var Zone_Index = new SqlParameter("@Zone_Index", CheckShortwave.Zone_Index);


        //                var Results = context.sp_Get_CartonShortwave.FromSql("EXEC sp_Get_CartonShortwave @PlanGoodsIssue_No,@Zone_Index", PlanGoodsIssue_No, Zone_Index).ToList();



        //                var GRDetail = new List<GoodsReceiveItemViewModel>();

        //                foreach (var item in queryResult)
        //                {
        //                    string SqlProductConversion = " and Convert(Nvarchar(50),Product_Index) = N'" + item.Product_Index + "'" +
        //                                                  " and ProductConversion_Ratio = 1";
        //                    var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                    var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();



        //                    //----Set Detail-----
        //                    int addNumber = 0;
        //                    int refDocLineNum = 0;
        //                    addNumber++;


        //                    var GRItem = new GoodsReceiveItemViewModel();


        //                    // Gen Index for line item

        //                    GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                    // Index From Header
        //                    GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;

        //                    GRItem.LineNum = addNumber.ToString(); ;

        //                    GRItem.ProductIndex = item.Product_Index;
        //                    GRItem.ProductId = item.Product_Id;
        //                    GRItem.ProductName = item.Product_Name;
        //                    GRItem.ProductSecondName = item.Product_SecondName;
        //                    GRItem.ProductThirdName = item.Product_ThirdName;
        //                    if (data.ProductLot != "")
        //                    {
        //                        GRItem.ProductLot = item.Product_Lot;
        //                    }
        //                    else
        //                    {
        //                        GRItem.ProductLot = "";
        //                    }
        //                    GRItem.ItemStatusIndex = item.ItemStatus_Index;
        //                    GRItem.ItemStatusId = item.ItemStatus_Id;
        //                    GRItem.ItemStatusName = item.ItemStatus_Name;
        //                    GRItem.qty = item.Picking_TotalQty;
        //                    GRItem.ratio = 1;

        //                    GRItem.TotalQty = item.Picking_TotalQty;

        //                    GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                    if (ProductConversion != null)
        //                    {
        //                        GRItem.ProductConversionIndex = ProductConversion.ProductConversion_Index;
        //                        GRItem.ProductConversionId = ProductConversion.ProductConversion_Id;
        //                        GRItem.ProductConversionName = ProductConversion.ProductConversion_Name;
        //                        GRItem.MFGDate = item.MFG_Date;
        //                    }
        //                    else
        //                    {
        //                        GRItem.ProductConversionIndex = item.ProductConversion_Index;
        //                        GRItem.ProductConversionId = item.ProductConversion_Id;
        //                        GRItem.ProductConversionName = item.ProductConversion_Name;
        //                        GRItem.MFGDate = item.MFG_Date;
        //                    }
        //                    GRItem.EXPDate = item.EXP_Date;

        //                    GRItem.UnitWeight = item.UnitWeight;

        //                    GRItem.Weight = item.Weight;

        //                    GRItem.UnitWidth = item.UnitWidth;

        //                    GRItem.UnitLength = item.UnitLength;

        //                    GRItem.UnitHeight = item.UnitHeight;

        //                    GRItem.UnitVolume = item.UnitVolume;

        //                    GRItem.Volume = item.Volume;

        //                    GRItem.UnitPrice = item.UnitPrice;

        //                    GRItem.Price = item.Price;


        //                    GRItem.RefDocumentNo = ViewPlanCarton.PlanGoodsIssue_No;

        //                    GRItem.RefDocumentLineNum = "";

        //                    GRItem.RefDocumentIndex = ViewPlanCarton.PlanGoodsIssue_Index;

        //                    GRItem.RefProcessIndex = new Guid("E1BA271A-F6E6-459D-B3EA-E4ACC522BD84");
        //                    GRItem.RefDocumentItemIndex = new Guid("00000000-0000-0000-0000-000000000000");
        //                    GRItem.DocumentRefNo1 = item.DocumentRef_No1;
        //                    GRItem.DocumentRefNo2 = item.DocumentRef_No2;
        //                    GRItem.DocumentRefNo3 = item.DocumentRef_No3;
        //                    GRItem.DocumentRefNo4 = item.DocumentRef_No4;
        //                    GRItem.DocumentRefNo5 = item.DocumentRef_No5;
        //                    GRItem.DocumentStatus = document_status;
        //                    GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                    if (data.UDF2 == null)
        //                    {
        //                        GRItem.UDF2 = "";
        //                    }
        //                    else
        //                    {
        //                        GRItem.UDF2 = data.UDF2;
        //                    }
        //                    if (data.UDF3 == null)
        //                    {
        //                        GRItem.UDF3 = "";
        //                    }
        //                    else
        //                    {
        //                        GRItem.UDF3 = item.UDF_3;
        //                    }
        //                    if (data.UDF4 == null)
        //                    {
        //                        GRItem.UDF4 = "";
        //                    }
        //                    else
        //                    {
        //                        GRItem.UDF4 = item.UDF_4;
        //                    }
        //                    if (data.UDF5 == null)
        //                    {
        //                        GRItem.UDF5 = "";
        //                    }
        //                    else
        //                    {
        //                        GRItem.UDF5 = item.UDF_5;
        //                    }
        //                    GRItem.GoodsReceiveRemark = "";
        //                    GRItem.GoodsReceiveDockDoor = "";
        //                    GRItem.Create_Date = DateTime.Now;
        //                    GRDetail.Add(GRItem);

        //                }

        //                if (Results.Count > 0)
        //                {
        //                    foreach (var item in Results)
        //                    {
        //                        string SqlProductConversion = " and Convert(Nvarchar(50),Product_Index) = N'" + item.Product_Index + "'" +
        //                               "and ProductConversion_Ratio = 1";
        //                        var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                        var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();



        //                        //----Set Detail-----
        //                        int addNumber = 0;
        //                        int refDocLineNum = 0;
        //                        addNumber++;


        //                        var GRItem = new GoodsReceiveItemViewModel();
        //                        // Gen Index for line item

        //                        GRItem.GoodsReceiveItemIndex = Guid.NewGuid();

        //                        // Index From Header
        //                        GRItem.GoodsReceiveIndex = GRHeader.GoodsReceiveIndex;

        //                        GRItem.LineNum = addNumber.ToString(); ;
        //                        GRItem.ItemStatusIndex = item.ItemStatus_Index;
        //                        GRItem.ItemStatusId = item.ItemStatus_Id;
        //                        GRItem.ItemStatusName = item.ItemStatus_Name;

        //                        GRItem.ProductIndex = item.Product_Index;
        //                        GRItem.ProductId = item.Product_Id;
        //                        GRItem.ProductName = item.Product_Name;
        //                        GRItem.ProductSecondName = item.Product_SecondName;
        //                        GRItem.ProductThirdName = item.Product_ThirdName;
        //                        if (data.ProductLot != "")
        //                        {
        //                            GRItem.ProductLot = item.Product_Lot;
        //                        }
        //                        else
        //                        {
        //                            GRItem.ProductLot = "";
        //                        }
        //                        if (ProductConversion != null)
        //                        {
        //                            GRItem.ProductConversionIndex = ProductConversion.ProductConversion_Index;
        //                            GRItem.ProductConversionId = ProductConversion.ProductConversion_Id;
        //                            GRItem.ProductConversionName = ProductConversion.ProductConversion_Name;
        //                            GRItem.MFGDate = item.MFG_Date;
        //                        }
        //                        else
        //                        {
        //                            GRItem.ProductConversionIndex = item.ProductConversion_Index;
        //                            GRItem.ProductConversionId = item.ProductConversion_Id;
        //                            GRItem.ProductConversionName = item.ProductConversion_Name;
        //                            GRItem.MFGDate = item.MFG_Date;
        //                        }
        //                        GRItem.qty = item.Picking_TotalQty;
        //                        GRItem.ratio = 1;

        //                        GRItem.TotalQty = item.Picking_TotalQty;

        //                        GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                        GRItem.ProductConversionIndex = item.ProductConversion_Index;
        //                        GRItem.ProductConversionId = item.ProductConversion_Id;
        //                        GRItem.ProductConversionName = item.ProductConversion_Name;
        //                        GRItem.MFGDate = item.MFG_Date;
        //                        GRItem.EXPDate = item.EXP_Date;

        //                        GRItem.UnitWeight = item.UnitWeight;

        //                        GRItem.Weight = item.Weight;

        //                        GRItem.UnitWidth = item.UnitWidth;

        //                        GRItem.UnitLength = item.UnitLength;

        //                        GRItem.UnitHeight = item.UnitHeight;

        //                        GRItem.UnitVolume = item.UnitVolume;

        //                        GRItem.Volume = item.Volume;

        //                        GRItem.UnitPrice = item.UnitPrice;

        //                        GRItem.Price = item.Price;


        //                        GRItem.RefDocumentNo = ViewPlanCarton.PlanGoodsIssue_No;

        //                        GRItem.RefDocumentLineNum = "";

        //                        GRItem.RefDocumentIndex = ViewPlanCarton.PlanGoodsIssue_Index;

        //                        GRItem.RefProcessIndex = new Guid("E1BA271A-F6E6-459D-B3EA-E4ACC522BD84");
        //                        GRItem.RefDocumentItemIndex = new Guid("00000000-0000-0000-0000-000000000000");
        //                        GRItem.DocumentRefNo1 = "";
        //                        GRItem.DocumentRefNo2 = "";
        //                        GRItem.DocumentRefNo3 = "";
        //                        GRItem.DocumentRefNo4 = "";
        //                        GRItem.DocumentRefNo5 = "";
        //                        GRItem.DocumentStatus = document_status;
        //                        GRItem.UDF1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                        GRItem.UDF2 = "";
        //                        GRItem.UDF3 = "";
        //                        GRItem.UDF4 = "";
        //                        GRItem.UDF5 = "";
        //                        GRItem.GoodsReceiveRemark = "";
        //                        GRItem.GoodsReceiveDockDoor = "";
        //                        GRItem.Create_Date = DateTime.Now;
        //                        GRDetail.Add(GRItem);

        //                    }

        //                }


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


        //                var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    var commandText2 = "EXEC sp_Save_im_GoodsReceive @GoodsReceive,@GoodsReceiveItem";
        //                    var rowsAffected2 = context.Database.ExecuteSqlCommand(commandText2, GoodsReceive, GoodsReceiveItem);

        //                    transaction.Commit();
        //                }
        //                catch (Exception exy)
        //                {
        //                    transaction.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("CartonRelocation", msglog);
        //                    throw exy;
        //                }

        //                String TagIndex = "";
        //                String TagNo = "";

        //                if (checkTag == null)
        //                {
        //                    //TAG
        //                    var TagHeader = new TagViewModel();



        //                    var PalletIndex = Guid.NewGuid();
        //                    TagHeader.TagIndex = Tag_Index;
        //                    TagHeader.TagNo = data.tagNoNew;
        //                    TagHeader.PalletIndex = PalletIndex;
        //                    TagHeader.TagRefNo1 = data.UDF1;
        //                    TagHeader.TagRefNo2 = data.UDF2;
        //                    TagHeader.TagRefNo3 = data.UDF3;
        //                    TagHeader.TagRefNo4 = data.UDF4;
        //                    TagHeader.TagRefNo5 = data.UDF5;
        //                    TagHeader.UDF1 = data.UDF1;
        //                    TagHeader.UDF2 = data.UDF2;
        //                    TagHeader.UDF3 = data.UDF3;
        //                    TagHeader.UDF4 = data.UDF4;
        //                    TagHeader.UDF5 = data.UDF5;
        //                    TagHeader.CreateBy = data.UpdateBy;
        //                    TagHeader.CreateDate = DateTime.Today;
        //                    TagHeader.TagStatus = 1;

        //                    TagIndex = TagHeader.TagIndex.ToString();
        //                    TagNo = data.tagNoNew;

        //                    string SqlGrItem = " and Convert(Nvarchar(50),GoodsReceive_Index) = N'" + GRHeader.GoodsReceiveIndex + "' ";
        //                    var strwhereGrItem = new SqlParameter("@strwhere", SqlGrItem);
        //                    var GrItem = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhereGrItem).ToList();

        //                    var TagItem = new List<TagItemViewModel>();


        //                    foreach (var item in GrItem)
        //                    {
        //                        //Detail

        //                        var TagDetail = new TagItemViewModel();

        //                        TagDetail.TagIndex = TagHeader.TagIndex;
        //                        TagDetail.TagItemIndex = Guid.NewGuid();
        //                        TagDetail.TagNo = data.tagNoNew;
        //                        TagDetail.GoodsReceiveIndex = item.GoodsReceive_Index;
        //                        TagDetail.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
        //                        TagDetail.ProductIndex = item.Product_Index;
        //                        TagDetail.ProductName = item.Product_Name;
        //                        TagDetail.ProductId = item.Product_Id;
        //                        TagDetail.ProductSecondName = item.Product_SecondName;
        //                        TagDetail.ProductThirdName = item.Product_ThirdName;
        //                        TagDetail.ProductLot = item.Product_Lot;
        //                        TagDetail.ItemStatusIndex = item.ItemStatus_Index;
        //                        TagDetail.ItemStatusId = item.ItemStatus_Id;
        //                        TagDetail.ItemStatusName = item.ItemStatus_Name;
        //                        TagDetail.Qty = item.Qty;
        //                        TagDetail.TotalQty = item.TotalQty;
        //                        TagDetail.Ratio = item.Ratio;
        //                        TagDetail.ProductConversionIndex = item.ProductConversion_Index;
        //                        TagDetail.ProductConversionId = item.ProductConversion_Id;
        //                        TagDetail.ProductConversionName = item.ProductConversion_Name;
        //                        TagDetail.Volume = item.Volume;
        //                        TagDetail.Weight = item.Weight;
        //                        TagDetail.MFGDate = item.MFG_Date;
        //                        TagDetail.EXPDate = item.EXP_Date;
        //                        TagDetail.TagRefNo1 = "";
        //                        TagDetail.TagRefNo2 = "";
        //                        TagDetail.TagRefNo3 = "";
        //                        TagDetail.TagRefNo4 = "";
        //                        TagDetail.TagRefNo5 = "";
        //                        TagDetail.TagStatus = 1;
        //                        TagDetail.CreateBy = item.Create_By;
        //                        TagDetail.CreateDate = DateTime.Today;
        //                        TagItem.Add(TagDetail);
        //                    }



        //                    var THeaderlist = new List<TagViewModel>();
        //                    THeaderlist.Add(TagHeader);



        //                    DataTable THeader = CreateDataTable(THeaderlist);
        //                    DataTable TDetail = CreateDataTable(TagItem);


        //                    var Tag = new SqlParameter("Tag", SqlDbType.Structured);
        //                    Tag.TypeName = "[dbo].[wm_TagTransferData]";
        //                    Tag.Value = THeader;


        //                    var TagItems = new SqlParameter("TagItems", SqlDbType.Structured);
        //                    TagItems.TypeName = "[dbo].[wm_TagItemData]";
        //                    TagItems.Value = TDetail;




        //                    var transaction2 = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {
        //                        var commandText1 = "EXEC sp_Save_NewLpn @Tag,@TagItems";
        //                        var rowsAffected1 = context.Database.ExecuteSqlCommand(commandText1, Tag, TagItems);

        //                        transaction2.Commit();
        //                        IsTag = true;

        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transaction2.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("TransferRelocation", msglog);
        //                        throw exy;
        //                    }
        //                }

        //                else
        //                {
        //                    string SqlGrItem = " and Convert(Nvarchar(50),GoodsReceive_Index) = N'" + GRHeader.GoodsReceiveIndex + "' ";
        //                    var strwhereGrItem = new SqlParameter("@strwhere", SqlGrItem);
        //                    var GrItem = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhereGrItem).ToList();

        //                    var TagItem = new List<TagItemViewModel>();

        //                    TagIndex = checkTag.Tag_Index.ToString();
        //                    TagNo = data.tagNoNew;

        //                    foreach (var item in GrItem)
        //                    {
        //                        //Detail

        //                        var TagDetail = new TagItemViewModel();

        //                        TagDetail.TagIndex = checkTag.Tag_Index;
        //                        TagDetail.TagItemIndex = Guid.NewGuid();
        //                        TagDetail.TagNo = data.tagNoNew;
        //                        TagDetail.GoodsReceiveIndex = item.GoodsReceive_Index;
        //                        TagDetail.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
        //                        TagDetail.ProductIndex = item.Product_Index;
        //                        TagDetail.ProductName = item.Product_Name;
        //                        TagDetail.ProductId = item.Product_Id;
        //                        TagDetail.ProductSecondName = item.Product_SecondName;
        //                        TagDetail.ProductThirdName = item.Product_ThirdName;
        //                        TagDetail.ProductLot = item.Product_Lot;
        //                        TagDetail.ItemStatusIndex = item.ItemStatus_Index;
        //                        TagDetail.ItemStatusId = item.ItemStatus_Id;
        //                        TagDetail.ItemStatusName = item.ItemStatus_Name;
        //                        TagDetail.Qty = item.Qty;
        //                        TagDetail.TotalQty = item.TotalQty;
        //                        TagDetail.Ratio = item.Ratio;
        //                        TagDetail.ProductConversionIndex = item.ProductConversion_Index;
        //                        TagDetail.ProductConversionId = item.ProductConversion_Id;
        //                        TagDetail.ProductConversionName = item.ProductConversion_Name;
        //                        TagDetail.Volume = item.Volume;
        //                        TagDetail.Weight = item.Weight;
        //                        TagDetail.MFGDate = item.MFG_Date;
        //                        TagDetail.EXPDate = item.EXP_Date;
        //                        TagDetail.TagRefNo1 = "";
        //                        TagDetail.TagRefNo2 = "";
        //                        TagDetail.TagRefNo3 = "";
        //                        TagDetail.TagRefNo4 = "";
        //                        TagDetail.TagRefNo5 = "";
        //                        TagDetail.TagStatus = 1;
        //                        TagDetail.CreateBy = item.Create_By;
        //                        TagDetail.CreateDate = DateTime.Today;
        //                        TagItem.Add(TagDetail);
        //                    }



        //                    DataTable CTag = CreateDataTable(TagItem);

        //                    var ResultsTagItem = new SqlParameter("TagItem", SqlDbType.Structured);
        //                    ResultsTagItem.TypeName = "[dbo].[wm_TransferTagItemData]";
        //                    ResultsTagItem.Value = CTag;

        //                    var transactionTag = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                    try
        //                    {
        //                        var commandTextTagItem = "EXEC sp_Save_im_TransferTagItem @TagItem";
        //                        var rowsAffectedTagItem = context.Database.ExecuteSqlCommand(commandTextTagItem, ResultsTagItem);
        //                        transactionTag.Commit();
        //                    }
        //                    catch (Exception exy)
        //                    {
        //                        transactionTag.Rollback();
        //                        msglog = State + " ex Rollback " + exy.Message.ToString();
        //                        olog.logging("TransferRelocation", msglog);
        //                        throw exy;
        //                    }
        //                }


        //                string pstring = " and GoodsReceive_Index = N'" + GRHeader.GoodsReceiveIndex + "'";
        //                var strwhere = new SqlParameter("@strwhere", pstring);

        //                var queryResults = context.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();

        //                var listBinBalance = new List<BinBalanceViewModel>();
        //                var listBinCard = new List<BinCardViewModel>();
        //                var GRLocation = new List<GoodsReceiveItemLocationViewModel>();

        //                foreach (var item in queryResults)
        //                {

        //                    string SqlProductConversion = " and Convert(Nvarchar(50),ProductConversion_Index) = N'" + item.ProductConversion_Index + "' ";
        //                    var strwhereProductConversion = new SqlParameter("@strwhere", SqlProductConversion);
        //                    var ProductConversion = context.ms_ProductConversion.FromSql("sp_GetProductConversion @strwhere", strwhereProductConversion).FirstOrDefault();

        //                    var GRLocationResult = new GoodsReceiveItemLocationViewModel();

        //                    var GoodsReceiveItemLocationIndex = Guid.NewGuid();

        //                    GRLocationResult.GoodsReceive_Index = item.GoodsReceive_Index;
        //                    GRLocationResult.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;

        //                    GRLocationResult.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocationIndex;

        //                    string SqlTagItem = " and Convert(Nvarchar(50),GoodsReceiveItem_Index) = N'" + item.GoodsReceiveItem_Index + "' ";
        //                    var strwhereTagItem = new SqlParameter("@strwhere", SqlTagItem);
        //                    var Item = context.wm_TagItem.FromSql("sp_GetTagItem @strwhere", strwhereTagItem).FirstOrDefault();

        //                    GRLocationResult.TagItem_Index = Item.TagItem_Index;
        //                    GRLocationResult.Tag_Index = new Guid(TagIndex);
        //                    GRLocationResult.Tag_No = TagNo;
        //                    GRLocationResult.Product_Index = item.Product_Index;
        //                    GRLocationResult.Product_Name = item.Product_Name;
        //                    GRLocationResult.Product_Id = item.Product_Id;
        //                    GRLocationResult.Product_Name = item.Product_Name;
        //                    GRLocationResult.Product_SecondName = item.Product_SecondName;
        //                    GRLocationResult.Product_ThirdName = item.Product_ThirdName;
        //                    GRLocationResult.Product_Lot = item.Product_Lot;
        //                    GRLocationResult.ItemStatus_Index = item.ItemStatus_Index;
        //                    GRLocationResult.ItemStatus_Id = item.ItemStatus_Id;
        //                    GRLocationResult.ItemStatus_Name = item.ItemStatus_Name;
        //                    GRLocationResult.ProductConversion_Index = item.ProductConversion_Index;
        //                    GRLocationResult.ProductConversion_Id = item.ProductConversion_Id;
        //                    GRLocationResult.ProductConversion_Name = item.ProductConversion_Name;
        //                    GRLocationResult.MFG_Date = item.MFG_Date;
        //                    GRLocationResult.EXP_Date = item.EXP_Date;
        //                    GRLocationResult.UnitWeight = ProductConversion.ProductConversion_Weight;
        //                    GRLocationResult.Weight = item.Weight;
        //                    GRLocationResult.UnitWidth = ProductConversion.ProductConversion_Width;
        //                    GRLocationResult.UnitLength = ProductConversion.ProductConversion_Length;
        //                    GRLocationResult.UnitHeight = ProductConversion.ProductConversion_Height;
        //                    GRLocationResult.UnitVolume = ProductConversion.ProductConversion_Volume;
        //                    GRLocationResult.Volume = item.Volume;
        //                    GRLocationResult.UnitPrice = 0;
        //                    GRLocationResult.Price = 0;
        //                    GRLocationResult.Owner_Index = GRHeader.OwnerIndex;
        //                    GRLocationResult.Owner_Id = GRHeader.OwnerId;
        //                    GRLocationResult.Owner_Name = GRHeader.OwnerName;
        //                    GRLocationResult.Location_Index = Location_Index_To;
        //                    GRLocationResult.Location_Id = Location_Id_To;
        //                    GRLocationResult.Location_Name = Location_Name_To;
        //                    GRLocationResult.Qty = item.Qty;
        //                    GRLocationResult.TotalQty = item.TotalQty;
        //                    GRLocationResult.Ratio = item.Ratio;
        //                    GRLocationResult.UDF_1 = item.UDF_1;
        //                    GRLocationResult.UDF_2 = item.UDF_2;
        //                    GRLocationResult.UDF_3 = item.UDF_3;
        //                    GRLocationResult.UDF_4 = item.UDF_4;
        //                    GRLocationResult.UDF_5 = item.UDF_5;
        //                    GRLocationResult.Create_By = item.Create_By;
        //                    GRLocationResult.Create_Date = item.Create_Date;
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
        //                    BinBalance.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //                    BinBalance.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                    BinBalance.TagItem_Index = Item.TagItem_Index;
        //                    BinBalance.Tag_Index = new Guid(TagIndex);
        //                    BinBalance.Tag_No = TagNo;
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
        //                    BinBalance.BinBalance_QtyBegin = item.Qty;
        //                    BinBalance.BinBalance_WeightBegin = item.Weight;
        //                    BinBalance.BinBalance_VolumeBegin = item.Volume;
        //                    BinBalance.BinBalance_QtyBal = item.Qty;
        //                    BinBalance.BinBalance_WeightBal = item.Weight;
        //                    BinBalance.BinBalance_VolumeBal = item.Volume;
        //                    BinBalance.BinBalance_QtyReserve = 0;
        //                    BinBalance.BinBalance_WeightReserve = 0;
        //                    BinBalance.BinBalance_VolumeReserve = 0;
        //                    BinBalance.ProductConversion_Index = item.ProductConversion_Index;
        //                    BinBalance.ProductConversion_Id = item.ProductConversion_Id;
        //                    BinBalance.ProductConversion_Name = item.ProductConversion_Name;
        //                    BinBalance.UDF_1 = ViewPlanCarton.Ref_PlanGoodsIssue_No;
        //                    BinBalance.UDF_2 = item.UDF_2;
        //                    BinBalance.UDF_3 = item.UDF_3;
        //                    BinBalance.UDF_4 = item.UDF_4;
        //                    BinBalance.UDF_5 = item.UDF_5;
        //                    BinBalance.Create_By = GRHeader.Create_By;
        //                    BinBalance.Create_Date = item.Create_Date;


        //                    listBinBalance.Add(BinBalance);

        //                    ////--------------------Bin Card --------------------
        //                    var BinCard = new BinCardViewModel();

        //                    BinCard.BinCard_Index = Guid.NewGuid();
        //                    BinCard.Process_Index = new Guid("7B7D01C4-9C56-4131-AD58-C1ACC20BE740");
        //                    // fix DocumentType GR
        //                    BinCard.DocumentType_Index = GRHeader.DocumentTypeIndex;
        //                    BinCard.DocumentType_Id = GRHeader.DocumentTypeId;
        //                    BinCard.DocumentType_Name = GRHeader.DocumentTypeName;
        //                    BinCard.GoodsReceive_Index = GRHeader.GoodsReceiveIndex;
        //                    BinCard.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //                    BinCard.GoodsReceiveItemLocation_Index = GRLocationResult.GoodsReceiveItemLocation_Index;
        //                    BinCard.BinCard_No = GRHeader.GoodsReceiveNo;
        //                    BinCard.BinCard_Date = GRHeader.GoodsReceiveDate;
        //                    BinCard.TagItem_Index = Item.TagItem_Index;
        //                    BinCard.Tag_Index = new Guid(TagIndex);
        //                    BinCard.Tag_No = TagNo;
        //                    BinCard.Tag_Index_To = new Guid(TagIndex);
        //                    BinCard.Tag_No_To = TagNo;
        //                    BinCard.Product_Index = item.Product_Index;
        //                    BinCard.Product_Id = item.Product_Id;
        //                    BinCard.Product_Name = item.Product_Name;
        //                    BinCard.Product_SecondName = item.Product_SecondName;
        //                    BinCard.Product_ThirdName = item.Product_ThirdName;
        //                    BinCard.Product_Index_To = item.Product_Index;
        //                    BinCard.Product_Id_To = item.Product_Id;
        //                    BinCard.Product_Name_To = item.Product_Name;
        //                    BinCard.Product_SecondName_To = item.Product_SecondName;
        //                    BinCard.Product_ThirdName_To = item.Product_ThirdName;
        //                    BinCard.Product_Lot = item.Product_Lot;
        //                    BinCard.Product_Lot_To = item.Product_Lot;
        //                    BinCard.ItemStatus_Index = item.ItemStatus_Index;
        //                    BinCard.ItemStatus_Id = item.ItemStatus_Id;
        //                    BinCard.ItemStatus_Name = item.ItemStatus_Name;
        //                    BinCard.ItemStatus_Index_To = item.ItemStatus_Index;
        //                    BinCard.ItemStatus_Id_To = item.ItemStatus_Id;
        //                    BinCard.ItemStatus_Name_To = item.ItemStatus_Name;
        //                    BinCard.ProductConversion_Index = item.ProductConversion_Index;
        //                    BinCard.ProductConversion_Id = item.ProductConversion_Id;
        //                    BinCard.ProductConversion_Name = item.ProductConversion_Name;
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

        //                    BinCard.GoodsReceive_EXP_Date = item.EXP_Date;
        //                    BinCard.GoodsReceive_EXP_Date_To = item.EXP_Date;
        //                    BinCard.BinCard_QtyIn = item.TotalQty;
        //                    BinCard.BinCard_QtyOut = 0;
        //                    BinCard.BinCard_QtySign = item.TotalQty;
        //                    BinCard.BinCard_WeightIn = item.Weight;
        //                    BinCard.BinCard_WeightOut = 0;
        //                    BinCard.BinCard_WeightSign = item.Weight;
        //                    BinCard.BinCard_VolumeIn = item.Volume;
        //                    BinCard.BinCard_VolumeOut = 0;
        //                    BinCard.BinCard_VolumeSign = item.Volume;
        //                    BinCard.Ref_Document_No = GRHeader.GoodsReceiveNo;
        //                    BinCard.Ref_Document_Index = GRHeader.GoodsReceiveIndex;
        //                    BinCard.Ref_DocumentItem_Index = item.GoodsReceiveItem_Index;
        //                    BinCard.Create_By = item.Create_By;
        //                    BinCard.Create_Date = item.Create_Date;

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

        //                var transaction3 = context.Database.BeginTransaction(IsolationLevel.Serializable);
        //                try
        //                {
        //                    var rowsAffected3 = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", GoodsReceiveItemLocation, pBinBalance, pBinCard);

        //                    String SqlcmdTag = " Update wm_TagOut set " +
        //                      " TagOut_Status = -1 " +
        //                      " where Convert(Varchar(200),Ref_Document_No) ='" + TagOut.Ref_Document_No + "'";
        //                    var row = context.Database.ExecuteSqlCommand(SqlcmdTag);


        //                    transaction3.Commit();
        //                    IsBi = true;
        //                    return "Success";

        //                }
        //                catch (Exception exy)
        //                {
        //                    transaction3.Rollback();
        //                    msglog = State + " ex Rollback " + exy.Message.ToString();
        //                    olog.logging("TransferRelocation", msglog);
        //                    throw exy;
        //                }
        //            }

        //            //---------------------------//



        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        using (var context = new TransferDbContext())
        //        {
        //            if (IsBi == false)
        //            {
        //                String SqlCmd = "";

        //                SqlCmd = " Delete from im_GoodsReceive where Convert(Varchar(200),GoodsReceive_Index)  ='" + Gr_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from im_GoodsReceiveItem where Convert(Varchar(200),GoodsReceive_Index)  ='" + Gr_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);
        //            }

        //            else if (IsTag == false || IsBi == false)
        //            {
        //                String SqlCmd = "";

        //                SqlCmd = " Delete from im_GoodsReceive where Convert(Varchar(200),GoodsReceive_Index)  ='" + Gr_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from im_GoodsReceiveItem where Convert(Varchar(200),GoodsReceive_Index)  ='" + Gr_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from wm_Tag where Convert(Varchar(200),Tag_Index)  ='" + Tag_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);

        //                SqlCmd = " Delete from wm_TagItem where Convert(Varchar(200),Tag_Index)  ='" + Tag_Index.ToString() + "'";
        //                context.Database.ExecuteSqlCommand(SqlCmd);
        //            }
        //        }

        //        throw ex;
        //    }
        //}


    }
}
