using Comone.Utils;
using DataAccess;
using GIDataAccess.Models;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using planGIBusiness.AutoNumber;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TransferBusiness.ConfigModel;
using TransferBusiness.GoodIssue;
using TransferBusiness.Library;
using TransferDataAccess.Models;

namespace TransferBusiness.Transfer
{
    public class AssignService
    {
        private TransferDbContext db;

        public AssignService()
        {
            db = new TransferDbContext();
        }
        public AssignService(TransferDbContext db)
        {
            this.db = db;
        }

        #region CreateDataTable
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

        #endregion

        #region assign
        public String assign(AssignJobViewModel data)
        {


            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {

                var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => data.listGoodsTransferViewModel.Select(s => s.goodsTransfer_Index).Contains(c.GoodsTransfer_Index)).ToList();

                var GoodsTransfer = db.IM_GoodsTransfer.Where(c => data.listGoodsTransferViewModel.Select(s => s.goodsTransfer_Index).Contains(c.GoodsTransfer_Index)).ToList();

                #region 1 : 1

                if (data.Template == "1")
                {

                    var ViewJoin = (from GTI in GoodsTransferItem
                                             join GT in GoodsTransfer on GTI.GoodsTransfer_Index equals GT.GoodsTransfer_Index

                                             select new View_AssignJobViewModel
                                             {
                                                 goodsTransfer_Index = GT.GoodsTransfer_Index,
                                                 goodsTransfer_No = GT.GoodsTransfer_No,
                                                 goodsTransferItem_Index = GTI.GoodsTransferItem_Index,
                                                 goodsTransfer_Date = GT.GoodsTransfer_Date,
                                                 qty = GTI.Qty,
                                                 totalQty = GTI.TotalQty,

        

                                             }).AsQueryable();



                    var ResultGroup = ViewJoin.GroupBy(c => new { c.goodsTransfer_Index, c.goodsTransfer_Date })
                     .Select(group => new
                     {
                         GT = group.Key.goodsTransfer_Index,
                         GTD = group.Key.goodsTransfer_Date,



                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();

                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.GT, item.GTD, data.Create_By, data.Template);
                    }

                }

                #endregion

                #region Update Status GI 

                foreach (var ResultGoodsTransfer in GoodsTransfer)
                {
                    var FindGoodsTransfer = db.IM_GoodsTransfer.Find(ResultGoodsTransfer.GoodsTransfer_Index);
                    FindGoodsTransfer.Document_Status = 2;
                }

                #endregion


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveGoodsTransferTask", msglog);
                    transaction.Rollback();

                    return exy.ToString();

                }

                return "true";

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String assignByLoc(View_AssignJobLocViewModel data)
        {


            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {

                //var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => data.listGoodsTransferViewModel.Select(s => s.goodsTransfer_Index).Contains(c.GoodsTransfer_Index)).ToList();

                //var GoodsTransfer = db.IM_GoodsTransfer.Where(c => data.listGoodsTransferViewModel.Select(s => s.goodsTransfer_Index).Contains(c.GoodsTransfer_Index)).ToList();
                var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index  == data.goodsTransfer_Index).ToList();

                var GoodsTransfer = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == data.goodsTransfer_Index).ToList();

                #region 1 : 1

                if (data.Template == "1")
                {

                    var ViewJoin = (from GTI in GoodsTransferItem
                                    join GT in GoodsTransfer on GTI.GoodsTransfer_Index equals GT.GoodsTransfer_Index

                                    select new View_AssignJobViewModel
                                    {
                                        goodsTransfer_Index = GT.GoodsTransfer_Index,
                                        goodsTransfer_No = GT.GoodsTransfer_No,
                                        goodsTransferItem_Index = GTI.GoodsTransferItem_Index,
                                        goodsTransfer_Date = GT.GoodsTransfer_Date,
                                        qty = GTI.Qty,
                                        totalQty = GTI.TotalQty,

                                        location_Index = GTI.Location_Index ,
                                        location_Id = GTI.Location_Id,
                                        location_Name = GTI.Location_Name,

                                        tag_No = GTI.Tag_No

                                    }).AsQueryable();



                    var ResultGroup = ViewJoin.GroupBy(c => new { c.goodsTransfer_Index, c.goodsTransfer_Date, c.location_Index, c.location_Id, c.location_Name ,c.tag_No })
                     .Select(group => new
                     {
                         GT = group.Key.goodsTransfer_Index,
                         GTD = group.Key.goodsTransfer_Date,

                         TAG = group.Key.tag_No,

                         LOCI = group.Key.location_Index,
                         LOCID = group.Key.location_Id,
                         LOCN = group.Key.location_Name,

                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();

                    foreach (var item in ResultGroup)
                    {
                        //this.CreateTaskLocation(item.GT, item.GTD, data.Create_By, data.Template, item.LOCI.ToString(), item.LOCN.ToString());
                        this.CreateTaskLocationWithTAG(item.GT, item.GTD, data.Create_By, data.Template, item.LOCI.ToString(), item.LOCN.ToString(), item.TAG);
                    }

                }

                #endregion

                #region Update Status GI 

                foreach (var ResultGoodsTransfer in GoodsTransfer)
                {
                    var FindGoodsTransfer = db.IM_GoodsTransfer.Find(ResultGoodsTransfer.GoodsTransfer_Index);
                    FindGoodsTransfer.Document_Status = 2;
                }

                #endregion


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveGoodsTransferTask", msglog);
                    transaction.Rollback();

                    return exy.ToString();

                }

                //[sp_GetCheckAfterTask]
                try
                {
                    var GTTask_Index = new SqlParameter("@TF_Index", data.goodsTransfer_Index);
                    var resultTask = db.Database.ExecuteSqlCommand("EXEC sp_GetCheckAfterTransferTask @TF_Index", GTTask_Index);



                }
                catch (Exception exxx)
                {

                    olog.logging("SaveGoodsTransferTask", "sp_GetCheckAfterTask " + exxx.Message.ToString());

                    //throw exxx;
                }


                return "true";

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateTask
        public String CreateTask(Guid? Index, DateTime? GID,  String Create_By, String Tempalate)
        {
            decimal GTIQty = 0;
            decimal CountQty = 0;
            decimal QtyBreak = 5;
            String TaskTransferIndex = "";
            String TaskTransferNo = "";

            try
            {
                var FindGT = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == Index).ToList();

                foreach (var item in FindGT)
                {
                    #region Create Task Header

                    var result = new im_TaskTransfer();


                    var Gen = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();


                    filterModel.process_Index = new Guid("8F91C673-193B-48A2-A3FE-8B28020B8AA7");
                    filterModel.documentType_Index = new Guid("6A8B781E-B2D2-4215-95B3-506F9CDBD0C4");
                    //GetConfig
                    Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                    DataTable resultDocumentType = CreateDataTable(Gen);

                    //var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                    //DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                    //DocumentType.Value = resultDocumentType;

                    //var DocumentType_Index = new SqlParameter("@DocumentType_Index", filterModel.documentType_Index.ToString());
                    //var DocDate = new SqlParameter("@DocDate", GID);
                    //var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    //resultParameter.Size = 2000; // some meaningfull value
                    //resultParameter.Direction = ParameterDirection.Output;
                    //db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                    //TaskTransferNo = resultParameter.Value.ToString();

                    var genDoc = new AutoNumberService();
                    string DocNo = "";
                    DateTime DocumentDate = (DateTime)item.GoodsTransfer_Date;
                    DocNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                    TaskTransferNo = DocNo;

                    result.TaskTransfer_Index = Guid.NewGuid();
                    result.TaskTransfer_No = TaskTransferNo;
                    result.Create_By = Create_By;
                    result.Create_Date = DateTime.Now;

                    db.im_TaskTransfer.Add(result);

                    #endregion

                    #region Create TaskItem

                    var FindGTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == Index).ToList();

                    var TaskItem = new List<im_TaskTransferItem>();


                    foreach (var listGTI in FindGTI)
                    {
                        var resultItem = new im_TaskTransferItem();

                        resultItem.TaskTransferItem_Index = Guid.NewGuid();
                        resultItem.TaskTransfer_Index = result.TaskTransfer_Index;
                        resultItem.TaskTransfer_No = TaskTransferNo;
                        resultItem.Tag_Index = listGTI.Tag_Index;
                        resultItem.TagItem_Index = listGTI.TagItem_Index;
                        resultItem.Tag_No = listGTI.Tag_No;
                        resultItem.Product_Index = listGTI.Product_Index;
                        resultItem.Product_Id = listGTI.Product_Id;
                        resultItem.Product_Name = listGTI.Product_Name;
                        resultItem.Product_SecondName = listGTI.Product_SecondName;
                        resultItem.Product_ThirdName = listGTI.Product_ThirdName;
                        resultItem.Product_Lot = listGTI.Product_Lot;
                        resultItem.ItemStatus_Index = listGTI.ItemStatus_Index;
                        resultItem.ItemStatus_Id = listGTI.ItemStatus_Id;
                        resultItem.ItemStatus_Name = listGTI.ItemStatus_Name;
                        resultItem.Location_Index = listGTI.Location_Index;
                        resultItem.Location_Id = listGTI.Location_Id;
                        resultItem.Location_Name = listGTI.Location_Name;
                        resultItem.Qty = listGTI.Qty;
                        resultItem.Ratio = listGTI.Ratio;
                        resultItem.TotalQty = listGTI.TotalQty;
                        resultItem.ProductConversion_Index = listGTI.ProductConversion_Index;
                        resultItem.ProductConversion_Id = listGTI.ProductConversion_Id;
                        resultItem.ProductConversion_Name = listGTI.ProductConversion_Name;
                        resultItem.MFG_Date = null;
                        resultItem.EXP_Date = null;
                        resultItem.UnitWeight = null;
                        resultItem.Weight = listGTI.Weight;
                        resultItem.UnitWidth = null;
                        resultItem.UnitLength = null;
                        resultItem.UnitHeight = null;
                        resultItem.UnitVolume = null;
                        resultItem.Volume = listGTI.Volume;
                        resultItem.UnitPrice = null;
                        resultItem.Price = null;
                        resultItem.DocumentRef_No1 = listGTI.DocumentRef_No1;
                        resultItem.DocumentRef_No2 = listGTI.DocumentRef_No2;
                        resultItem.DocumentRef_No3 = listGTI.DocumentRef_No3;
                        resultItem.DocumentRef_No4 = listGTI.DocumentRef_No4;
                        resultItem.DocumentRef_No5 = listGTI.DocumentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.UDF_1 = listGTI.UDF_1;
                        resultItem.UDF_2 = listGTI.UDF_2;
                        resultItem.UDF_3 = listGTI.UDF_3;
                        resultItem.UDF_4 = listGTI.UDF_2;
                        resultItem.UDF_5 = listGTI.UDF_5;
                        resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                        resultItem.Ref_Document_Index = listGTI.GoodsTransfer_Index;
                        resultItem.Ref_Document_No = item.GoodsTransfer_No;
                        resultItem.Ref_Document_LineNum = listGTI.LineNum;
                        resultItem.Ref_DocumentItem_Index = listGTI.GoodsTransferItem_Index;
                        resultItem.Create_By = Create_By;
                        resultItem.Create_Date = DateTime.Now;
                        db.im_TaskTransferItem.Add(resultItem);
                    }

                    #endregion


                }

                return "success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public String CreateTaskLocation(Guid? Index, DateTime? GID, String Create_By, String Tempalate, String pLocation_Index, String pLocation_Name)
        {
            decimal GTIQty = 0;
            decimal CountQty = 0;
            decimal QtyBreak = 5;
            String TaskTransferIndex = "";
            String TaskTransferNo = "";

            try
            {
                var FindGT = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == Index).ToList();

                foreach (var item in FindGT)
                {
                    #region Create Task Header

                    var result = new im_TaskTransfer();


                    var Gen = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();


                    filterModel.process_Index = new Guid("8F91C673-193B-48A2-A3FE-8B28020B8AA7");
                    filterModel.documentType_Index = new Guid("6A8B781E-B2D2-4215-95B3-506F9CDBD0C4");
                    //GetConfig
                    Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                    DataTable resultDocumentType = CreateDataTable(Gen);

                    //var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                    //DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                    //DocumentType.Value = resultDocumentType;

                    //var DocumentType_Index = new SqlParameter("@DocumentType_Index", filterModel.documentType_Index.ToString());
                    //var DocDate = new SqlParameter("@DocDate", GID);
                    //var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    //resultParameter.Size = 2000; // some meaningfull value
                    //resultParameter.Direction = ParameterDirection.Output;
                    //db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                    //TaskTransferNo = resultParameter.Value.ToString();

                    var genDoc = new AutoNumberService();
                    string DocNo = "";
                    DateTime DocumentDate = (DateTime)item.GoodsTransfer_Date;
                    DocNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                    TaskTransferNo = DocNo;

                    result.TaskTransfer_Index = Guid.NewGuid();
                    result.TaskTransfer_No = TaskTransferNo;
                    result.Create_By = Create_By;
                    result.Create_Date = DateTime.Now;

                    db.im_TaskTransfer.Add(result);

                    #endregion

                    #region Create TaskItem

                    var FindGTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == Index && c.Document_Status == 0 && c.Location_Index == Guid.Parse(pLocation_Index)).ToList();

                    var TaskItem = new List<im_TaskTransferItem>();


                    foreach (var listGTI in FindGTI)
                    {
                        var resultItem = new im_TaskTransferItem();

                        resultItem.TaskTransferItem_Index = Guid.NewGuid();
                        resultItem.TaskTransfer_Index = result.TaskTransfer_Index;
                        resultItem.TaskTransfer_No = TaskTransferNo;
                        resultItem.Tag_Index = listGTI.Tag_Index;
                        resultItem.TagItem_Index = listGTI.TagItem_Index;
                        resultItem.Tag_No = listGTI.Tag_No;
                        resultItem.Product_Index = listGTI.Product_Index;
                        resultItem.Product_Id = listGTI.Product_Id;
                        resultItem.Product_Name = listGTI.Product_Name;
                        resultItem.Product_SecondName = listGTI.Product_SecondName;
                        resultItem.Product_ThirdName = listGTI.Product_ThirdName;
                        resultItem.Product_Lot = listGTI.Product_Lot;
                        resultItem.ItemStatus_Index = listGTI.ItemStatus_Index;
                        resultItem.ItemStatus_Id = listGTI.ItemStatus_Id;
                        resultItem.ItemStatus_Name = listGTI.ItemStatus_Name;
                        resultItem.Location_Index = listGTI.Location_Index;
                        resultItem.Location_Id = listGTI.Location_Id;
                        resultItem.Location_Name = listGTI.Location_Name;
                        resultItem.Qty = listGTI.Qty;
                        resultItem.Ratio = listGTI.Ratio;
                        resultItem.TotalQty = listGTI.TotalQty;
                        resultItem.ProductConversion_Index = listGTI.ProductConversion_Index;
                        resultItem.ProductConversion_Id = listGTI.ProductConversion_Id;
                        resultItem.ProductConversion_Name = listGTI.ProductConversion_Name;
                        resultItem.MFG_Date = null;
                        resultItem.EXP_Date = null;
                        resultItem.UnitWeight = null;
                        resultItem.Weight = listGTI.Weight;
                        resultItem.UnitWidth = null;
                        resultItem.UnitLength = null;
                        resultItem.UnitHeight = null;
                        resultItem.UnitVolume = null;
                        resultItem.Volume = listGTI.Volume;
                        resultItem.UnitPrice = null;
                        resultItem.Price = null;
                        resultItem.DocumentRef_No1 = listGTI.DocumentRef_No1;
                        resultItem.DocumentRef_No2 = listGTI.DocumentRef_No2;
                        resultItem.DocumentRef_No3 = listGTI.DocumentRef_No3;
                        resultItem.DocumentRef_No4 = listGTI.DocumentRef_No4;
                        resultItem.DocumentRef_No5 = listGTI.DocumentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.UDF_1 = listGTI.UDF_1;
                        resultItem.UDF_2 = listGTI.UDF_2;
                        resultItem.UDF_3 = listGTI.UDF_3;
                        resultItem.UDF_4 = listGTI.UDF_2;
                        resultItem.UDF_5 = listGTI.UDF_5;
                        resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                        resultItem.Ref_Document_Index = listGTI.GoodsTransfer_Index;
                        resultItem.Ref_Document_No = item.GoodsTransfer_No;
                        resultItem.Ref_Document_LineNum = listGTI.LineNum;
                        resultItem.Ref_DocumentItem_Index = listGTI.GoodsTransferItem_Index;
                        resultItem.Create_By = Create_By;
                        resultItem.Create_Date = DateTime.Now;
                        db.im_TaskTransferItem.Add(resultItem);
                    }

                    #endregion


                }

                return "success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public String CreateTaskLocationWithTAG(Guid? Index, DateTime? GID, String Create_By, String Tempalate, String pLocation_Index, String pLocation_Name, String Tag_No)
        {
            decimal GTIQty = 0;
            decimal CountQty = 0;
            decimal QtyBreak = 5;
            String TaskTransferIndex = "";
            String TaskTransferNo = "";

            try
            {
                var FindGT = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == Index).ToList();

                foreach (var item in FindGT)
                {
                    #region Create Task Header

                    var result = new im_TaskTransfer();


                    var Gen = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();


                    filterModel.process_Index = new Guid("8F91C673-193B-48A2-A3FE-8B28020B8AA7");
                    filterModel.documentType_Index = new Guid("6A8B781E-B2D2-4215-95B3-506F9CDBD0C4");
                    //GetConfig
                    Gen = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                    DataTable resultDocumentType = CreateDataTable(Gen);

                    //var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                    //DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                    //DocumentType.Value = resultDocumentType;

                    //var DocumentType_Index = new SqlParameter("@DocumentType_Index", filterModel.documentType_Index.ToString());
                    //var DocDate = new SqlParameter("@DocDate", GID);
                    //var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    //resultParameter.Size = 2000; // some meaningfull value
                    //resultParameter.Direction = ParameterDirection.Output;
                    //db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                    //TaskTransferNo = resultParameter.Value.ToString();

                    var genDoc = new AutoNumberService();
                    string DocNo = "";
                    DateTime DocumentDate = (DateTime)item.GoodsTransfer_Date;
                    DocNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                    TaskTransferNo = DocNo;

                    result.TaskTransfer_Index = Guid.NewGuid();
                    result.TaskTransfer_No = TaskTransferNo;
                    result.Create_By = Create_By;
                    result.Create_Date = DateTime.Now;

                    db.im_TaskTransfer.Add(result);

                    #endregion

                    #region Create TaskItem

                    var FindGTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == Index && c.Document_Status == 0 && c.Location_Index == Guid.Parse(pLocation_Index) && c.Tag_No == Tag_No).ToList();

                    var TaskItem = new List<im_TaskTransferItem>();


                    foreach (var listGTI in FindGTI)
                    {
                        var resultItem = new im_TaskTransferItem();

                        resultItem.TaskTransferItem_Index = Guid.NewGuid();
                        resultItem.TaskTransfer_Index = result.TaskTransfer_Index;
                        resultItem.TaskTransfer_No = TaskTransferNo;
                        resultItem.Tag_Index = listGTI.Tag_Index;
                        resultItem.TagItem_Index = listGTI.TagItem_Index;
                        resultItem.Tag_No = listGTI.Tag_No;
                        resultItem.Product_Index = listGTI.Product_Index;
                        resultItem.Product_Id = listGTI.Product_Id;
                        resultItem.Product_Name = listGTI.Product_Name;
                        resultItem.Product_SecondName = listGTI.Product_SecondName;
                        resultItem.Product_ThirdName = listGTI.Product_ThirdName;
                        resultItem.Product_Lot = listGTI.Product_Lot;
                        resultItem.ItemStatus_Index = listGTI.ItemStatus_Index;
                        resultItem.ItemStatus_Id = listGTI.ItemStatus_Id;
                        resultItem.ItemStatus_Name = listGTI.ItemStatus_Name;
                        resultItem.Location_Index = listGTI.Location_Index;
                        resultItem.Location_Id = listGTI.Location_Id;
                        resultItem.Location_Name = listGTI.Location_Name;
                        resultItem.Qty = listGTI.Qty;
                        resultItem.Ratio = listGTI.Ratio;
                        resultItem.TotalQty = listGTI.TotalQty;
                        resultItem.ProductConversion_Index = listGTI.ProductConversion_Index;
                        resultItem.ProductConversion_Id = listGTI.ProductConversion_Id;
                        resultItem.ProductConversion_Name = listGTI.ProductConversion_Name;
                        resultItem.MFG_Date = null;
                        resultItem.EXP_Date = null;
                        resultItem.UnitWeight = null;
                        resultItem.Weight = listGTI.Weight;
                        resultItem.UnitWidth = null;
                        resultItem.UnitLength = null;
                        resultItem.UnitHeight = null;
                        resultItem.UnitVolume = null;
                        resultItem.Volume = listGTI.Volume;
                        resultItem.UnitPrice = null;
                        resultItem.Price = null;
                        resultItem.DocumentRef_No1 = listGTI.DocumentRef_No1;
                        resultItem.DocumentRef_No2 = listGTI.DocumentRef_No2;
                        resultItem.DocumentRef_No3 = listGTI.DocumentRef_No3;
                        resultItem.DocumentRef_No4 = listGTI.DocumentRef_No4;
                        resultItem.DocumentRef_No5 = listGTI.DocumentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.UDF_1 = listGTI.UDF_1;
                        resultItem.UDF_2 = listGTI.UDF_2;
                        resultItem.UDF_3 = listGTI.UDF_3;
                        resultItem.UDF_4 = listGTI.UDF_2;
                        resultItem.UDF_5 = listGTI.UDF_5;
                        resultItem.Ref_Process_Index = new Guid("2E026669-99BD-4DE0-8818-534F29F7B89D");
                        resultItem.Ref_Document_Index = listGTI.GoodsTransfer_Index;
                        resultItem.Ref_Document_No = item.GoodsTransfer_No;
                        resultItem.Ref_Document_LineNum = listGTI.LineNum;
                        resultItem.Ref_DocumentItem_Index = listGTI.GoodsTransferItem_Index;
                        resultItem.Create_By = Create_By;
                        resultItem.Create_Date = DateTime.Now;
                        db.im_TaskTransferItem.Add(resultItem);
                    }

                    #endregion


                }

                return "success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region taskfilter
        public List<TaskfilterViewModel> taskfilter(TaskfilterViewModel model)
        {
            try
            {
                var query = db.View_TaskTransfer.AsQueryable();

                var result = new List<TaskfilterViewModel>();


                if (model.listTaskViewModel.Count != 0)
                {

                    query = query.Where(c => model.listTaskViewModel.Select(s => s.goodsTransfer_No).Contains(c.Ref_Document_No));

                    var queryresult = query.ToList();

                    foreach (var itemResult in queryresult)
                    {

                        var resultItem = new TaskfilterViewModel();

                        resultItem.taskTransfer_Index = itemResult.TaskTransfer_Index;
                        resultItem.goodsTransfer_No = itemResult.Ref_Document_No;
                        resultItem.taskTransfer_No = itemResult.TaskTransfer_No;
                        resultItem.goodsTransfer_Index = itemResult.Ref_Document_Index;
                        resultItem.userAssign = itemResult.UserAssign;
                        resultItem.create_By = itemResult.Create_By;
                        resultItem.create_Date = itemResult.Create_Date.toString();
                        resultItem.create_Time = itemResult.Create_Date.ToString("HH:mm");
                        resultItem.update_By = itemResult.update_By;
                        result.Add(resultItem);

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.goodsTransfer_No))
                    {
                        query = query.Where(c => c.Ref_Document_No.Contains(model.goodsTransfer_No));
                    }
                    else
                    {
                        return result;
                    }

                    var queryresult = query.ToList();


                    foreach (var item in queryresult)
                    {
                        var resultItem = new TaskfilterViewModel();

                        resultItem.taskTransfer_Index = item.TaskTransfer_Index;
                        resultItem.goodsTransfer_No = item.Ref_Document_No;
                        resultItem.taskTransfer_No = item.TaskTransfer_No;
                        resultItem.goodsTransfer_Index = item.Ref_Document_Index;
                        resultItem.userAssign = item.UserAssign;
                        resultItem.create_By = item.Create_By;
                        resultItem.create_Date = item.Create_Date.toString();
                        resultItem.create_Time = item.Create_Date.ToString("HH:mm");
                        resultItem.update_By = item.update_By;
                        result.Add(resultItem);

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

        #region confirmTask
        public String confirmTask(TaskfilterViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                foreach (var item in data.listTaskViewModel)
                {
                    var Task = db.im_TaskTransfer.Find(item.taskTransfer_Index);

                    if (Task != null)
                    {
                        Task.Document_Status = 1;
                        Task.Update_By = data.update_By;
                        Task.Update_Date = DateTime.Now;
                        Task.UserAssign = item.userAssign;
                        Task.Assign_By = item.update_By;
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
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("confirmTask", msglog);
                    transaction.Rollback();
                    throw exy;

                }

                return "Done";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region taskPopup
        public List<GoodsTransferItemViewModel> taskPopup(GoodsTransferViewModel model)
        {
            try
            {
                var query = db.IM_GoodsTransferItem.AsQueryable();


                query = query.Where(c => c.GoodsTransfer_Index == model.goodsTransfer_Index && c.Document_Status != -1);

                

                var Item = query.ToList();

                var result = new List<GoodsTransferItemViewModel>();


                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());


                foreach (var item in Item)
                {
                    String Statue = "";
                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();


                    var resultItem = new GoodsTransferItemViewModel();

                    resultItem.goodsTransfer_Index = item.GoodsTransfer_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.qty = item.Qty;
                    resultItem.location_Name = item.Location_Name;
                    resultItem.location_Name_To = item.Location_Name_To;
                    resultItem.processStatus_Name = ProcessStatusName?.processStatus_Name;

                    result.Add(resultItem);

                }

                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region  autoGoodTaskTransferNo
        public List<ItemListViewModel> autoGoodTaskTransferNo(ItemListViewModel data)
        {
            try
            {
                var query = db.View_TaskTransfer.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.Ref_Document_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.Ref_Document_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.Ref_Document_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  autoGoodTransferNo
        public List<ItemListViewModel> autoGoodTransferNo(ItemListViewModel data)
        {
            try
            {
                var query = db.IM_GoodsTransfer.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.GoodsTransfer_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.GoodsTransfer_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.GoodsTransfer_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region DropdownUser
        public List<UserViewModel> dropdownUser(UserViewModel data)
        {
            try
            {
                var result = new List<UserViewModel>();

                var filterModel = new ProcessStatusViewModel();

                //GetConfig
                result = utils.SendDataApi<List<UserViewModel>>(new AppSettingConfig().GetUrl("dropdownUser"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
