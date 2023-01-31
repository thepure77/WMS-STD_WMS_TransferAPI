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

namespace TransferBusiness.Transfer
{
    public class GoodsTransferItemExportService
    {
        private TransferDbContext db;
        private MasterConnectionDbContext dbMaster;

        public GoodsTransferItemExportService()
        {
            db = new TransferDbContext();
            dbMaster = new MasterConnectionDbContext();
        }

        public GoodsTransferItemExportService(TransferDbContext db , MasterConnectionDbContext dbMaster)
        {
            this.db = db;
            this.dbMaster = dbMaster;
        }

        public List<GoodsTransferItemExportViewModel> GoodsTransferItem(string id,bool ischkQI)
        {
            try
            {
                var result = new List<GoodsTransferItemExportViewModel>();

                using (var context = new TransferDbContext())
                {

                    //string pstring = "";
                    //pstring += " and GoodsReceive_Index = '" + id + "'";
                    //pstring += " and Document_Status != -1 ";
                    //var strwhere = new SqlParameter("@strwhere", pstring);

                    //var queryResult = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();
                    Guid GoodsTransferIndex = new Guid(id);

                    var queryResult = db.IM_GoodsTransferItem.Where(c => c.Document_Status != -1 && c.Document_Status != -3 && c.GoodsTransfer_Index == GoodsTransferIndex).OrderBy(c=> c.Product_Id).ToList();

                    foreach (var data in queryResult)
                    {
                        var item = new GoodsTransferItemExportViewModel();
                        //var planGR = data.Ref_Document_Index.ToString();
                        item.goodsTransferItem_Index = data.GoodsTransferItem_Index;
                        item.goodsTransfer_Index = data.GoodsTransfer_Index;
                        item.goodsReceiveItem_Index = data.GoodsReceiveItem_Index;
                        //item.LineNum = data.LineNum;
                        item.product_Index = data.Product_Index;
                        item.product_Index = data.Product_Index;
                        item.product_Id = data.Product_Id;
                        item.product_Name = data.Product_Name;
                        item.location_Index = data.Location_Index;
                        item.location_Id = data.Location_Id;
                        item.location_Name = data.Location_Name;
                        item.location_Index_To = data.Location_Index_To;
                        item.location_Id_To = data.Location_Id_To;
                        item.location_Name_To = data.Location_Name_To;
                        item.itemStatus_Index = data.ItemStatus_Index;
                        item.itemStatus_Id = data.ItemStatus_Id;
                        item.itemStatus_Name = data.ItemStatus_Name;
                        item.itemStatus_Index_To = data.ItemStatus_Index_To;
                        item.itemStatus_Id_To = data.ItemStatus_Id_To;
                        item.itemStatus_Name_To = data.ItemStatus_Name_To;
                        item.tag_Index = data.Tag_Index;
                        item.tag_No = data.Tag_No;
                        item.tag_Index_To = data.Tag_Index_To;
                        item.tag_No_To = data.Tag_No_To;
                        item.product_SecondName = data.Product_SecondName;
                        item.product_ThirdName = data.Product_ThirdName;
                        item.product_Lot = data.Product_Lot;
                        item.product_Lot_To = data.Product_Lot_To;
                        item.itemStatus_Index = data.ItemStatus_Index;
                        item.itemStatus_Id = data.ItemStatus_Id;
                        item.itemStatus_Name = data.ItemStatus_Name;
                        item.qty = data.Qty;
                        item.pick = data.Qty;
                        item.ratio = data.Ratio;
                        item.totalQty = data.TotalQty;
                        ////if (data.Ref_Document_Index != null)
                        ////{
                        ////    string pstring1 = "";
                        ////    pstring1 += " and  PlanGoodsReceive_Index = '" + planGR + "'";
                        ////    pstring1 += " and Product_Index = '" + data.Product_Index + "'";
                        ////    pstring1 += " and PlanGoodsReceiveItem_Index = '" + data.Ref_DocumentItem_Index + "'";
                        ////    pstring1 += " and Document_Status != -1 ";
                        ////    var strwhere1 = new SqlParameter("@strwhere", pstring1);
                        ////    var query = context.IM_PlanGoodsReceiveItems.FromSql("sp_GetPlanGoodsReceiveItem @strwhere", strwhere1).FirstOrDefault();
                        ////    item.TotalQtyPlanGR = ((query.TotalQty - data.TotalQty) + data.TotalQty);
                        ////}
                        ////else
                        ////{
                        ////    item.TotalQtyPlanGR = data.TotalQty;
                        ////}
                        item.productConversion_Index = data.ProductConversion_Index;
                        item.productConversion_Id = data.ProductConversion_Id;
                        item.productConversion_Name = data.ProductConversion_Name;
                        item.ref_Document_No = data.Ref_Document_No;
                        //item.mfg_Date = data.MFG_Date.toString();
                        //item.exp_Date = data.EXP_Date.toString();
                        item.weight = data.Weight;
                        //item.unitWeight = data.UnitWeight;
                        //item.unitWidth = data.UnitWidth;
                        //item.unitLength = data.UnitLength;
                        //item.unitHeight = data.UnitHeight;
                        //item.unitVolume = data.UnitVolume;
                        item.volume = data.Volume;
                        //item.unitPrice = data.UnitPrice;
                        //item.price = data.Price;
                        item.ref_Document_Index = data.Ref_Document_Index;
                        item.ref_DocumentItem_Index = data.Ref_DocumentItem_Index;
                        item.ref_Document_No = data.Ref_Document_No;
                        //item.ref_Document_LineNum = data.Ref_Document_LineNum;
                        item.documentRef_No1 = data.DocumentRef_No1;
                        item.documentRef_No2 = data.DocumentRef_No2;
                        item.documentRef_No3 = data.DocumentRef_No3;
                        item.documentRef_No4 = data.DocumentRef_No4;
                        item.documentRef_No5 = data.DocumentRef_No5;
                        item.document_Status = data.Document_Status;
                        //item.goodsReceive_Remark = data.GoodsReceive_Remark;
                        item.udf_1 = data.UDF_1;
                        item.udf_2 = data.UDF_2;
                        item.udf_3 = data.UDF_3;
                        item.udf_4 = data.UDF_4;
                        item.udf_5 = data.UDF_5;
                        item.erp_Location = data.ERP_Location;
                        item.erp_Location_To = data.ERP_Location_To;
                        item.goodsReceive_EXP_Date = data.GoodsReceive_EXP_Date.toString();
                        item.goodsReceive_EXP_Date_To = data.GoodsReceive_EXP_Date_To.toString();
                        item.goodsReceive_MFG_Date = data.GoodsReceive_MFG_Date.toString();
                        item.goodsReceive_MFG_Date_To = data.GoodsReceive_MFG_Date_To.toString();

                        result.Add(item);
                    }
                }

                if (ischkQI)
                {
                    result = result.GroupBy(g => new { g.udf_1, g.udf_2, g.udf_3,g.documentRef_No1 }).Select(s => new GoodsTransferItemExportViewModel { udf_1 = s.Key.udf_1, udf_2 = s.Key.udf_2, udf_3 = s.Key.udf_3,documentRef_No1=s.Key.documentRef_No1 }).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AddNewLocation(GoodsTransferItemExportViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            string result = "S";


            var GoodTransferItem = db.IM_GoodsTransferItem.Find(data.goodsTransferItem_Index);

            if(data.itemStatus_Index_To != null)
            {
                GoodTransferItem.ItemStatus_Index_To = data.itemStatus_Index_To;
                GoodTransferItem.ItemStatus_Id_To = data.itemStatus_Id_To;
                GoodTransferItem.ItemStatus_Name_To = data.itemStatus_Name_To;
                GoodTransferItem.Update_By = data.update_By;
                GoodTransferItem.Update_Date = DateTime.Now;
            }

            #region GetLocation
            var LocationViewModel = new { location_Name = data.location_Name_To };
            var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocationV2"), LocationViewModel.sJson());

            var DataLocation = GetLocation.FirstOrDefault();

            #endregion

            if (GoodTransferItem.Location_Name != data.location_Name_To)
            {
                View_CheckLocation checklocation = dbMaster.View_CheckLocation.FirstOrDefault(c => c.Location_Name == data.location_Name_To);
                if (checklocation == null)
                {
                    result = "locationNotFound";
                    return result;
                }
                if (checklocation.LocationType_Index != new Guid("BA0142A8-98B7-4E0B-A1CE-6266716F5F67"))
                {
                    result = "Location Not Selective on Ground";
                    return result;
                }
            }

            if (GetLocation.Count > 0)
            {
                GoodTransferItem.Location_Index_To = DataLocation.location_Index;
                GoodTransferItem.Location_Id_To = DataLocation.location_Id;
                GoodTransferItem.Location_Name_To = DataLocation.location_Name;
                GoodTransferItem.Update_By = data.update_By;
                GoodTransferItem.Update_Date = DateTime.Now;
                    
            } else
            {
                result = "locationNotFound";
            }

            if (data.tag_No_To != null)
            {
                GoodTransferItem.Tag_No_To = data.tag_No_To;
                GoodTransferItem.Update_By = data.update_By;
                GoodTransferItem.Update_Date = DateTime.Now;
            }

            if (data.erp_Location != data.erp_Location_To)
            {
                GoodTransferItem.ERP_Location_To = data.erp_Location_To;
                GoodTransferItem.Update_By = data.update_By;
                GoodTransferItem.Update_Date = DateTime.Now;
            }

            if (data.product_Lot != data.product_Lot_To)
            {
                GoodTransferItem.Product_Lot_To = data.product_Lot_To;
                GoodTransferItem.Update_By = data.update_By;
                GoodTransferItem.Update_Date = DateTime.Now;
            }

            var transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                db.SaveChanges();
                transaction.Commit();
            }

            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("CreatePackItem", msglog);
                transaction.Rollback();
                throw ex;
            }
            

            return result;
        }

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

        public string CheckLocation(GoodsTransferItemExportViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            string result = "";


            var GetLocation = dbMaster.View_LocatinSelectiveOnGroundExport.Where(c => c.Location_Name == data.location_Name_To).ToList().Count();


            if (GetLocation == 0)
            {
                result = "Location Not Selective on Ground";
                return result;
            }

            return result;
        }

    }
}
