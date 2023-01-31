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
    public class TransferUnpackService
    {
        private TransferDbContext db;

        private BinbalanceDbContext db2;

        private InboundDbContext dbInbound;

        private MasterConnectionDbContext dbMaster;

        public TransferUnpackService()
        {
            db = new TransferDbContext();
            db2 = new BinbalanceDbContext();
            dbInbound = new InboundDbContext();
            dbMaster = new MasterConnectionDbContext();
        }

        public TransferUnpackService(TransferDbContext db)
        {
            this.db = db;

        }

        #region find
        public GoodsTransferViewModel find(Guid id)
        {

            try
            {
                var queryResult = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == id).FirstOrDefault();

                var resultItem = new GoodsTransferViewModel();

                resultItem.goodsTransfer_Index = queryResult.GoodsTransfer_Index;
                resultItem.goodsTransfer_No = queryResult.GoodsTransfer_No;
                resultItem.goodsTransfer_Date = queryResult.GoodsTransfer_Date.toString();
                resultItem.goodsTransfer_Time = queryResult.GoodsTransfer_Time;
                resultItem.goodsTransfer_Doc_Date = queryResult.GoodsTransfer_Doc_Date.toString();
                resultItem.goodsTransfer_Doc_Time = queryResult.GoodsTransfer_Doc_Time;
                resultItem.documentType_Index = queryResult.DocumentType_Index;
                resultItem.documentType_Name = queryResult.DocumentType_Name;
                resultItem.documentType_Id = queryResult.DocumentType_Id;
                resultItem.owner_Index = queryResult.Owner_Index;
                resultItem.owner_Id = queryResult.Owner_Id;
                resultItem.owner_Name = queryResult.Owner_Name;
                resultItem.udf_1 = queryResult.UDF_1;
                resultItem.udf_2 = queryResult.UDF_2;
                resultItem.udf_3 = queryResult.UDF_3;
                resultItem.udf_4 = queryResult.UDF_4;
                resultItem.udf_5 = queryResult.UDF_5;
                resultItem.documentRef_No1 = queryResult.DocumentRef_No1;
                resultItem.documentRef_No2 = queryResult.DocumentRef_No2;
                resultItem.documentRef_No3 = queryResult.DocumentRef_No3;
                resultItem.documentRef_No4 = queryResult.DocumentRef_No4;
                resultItem.documentRef_No5 = queryResult.DocumentRef_No5;
                resultItem.documentRef_Remark = queryResult.DocumentRef_Remark;
                resultItem.document_Status = queryResult.Document_Status;
                //resultItem.isUseDocumentType = queryResult.DocumentType_Index == Guid.Parse("485DAA74-BF94-441F-828D-3D6BFB2C616F") ? true : false;
                resultItem.isUseDocumentType = queryResult.DocumentType_Index != Guid.Parse("9056ff09-29df-4bba-8fc5-6c524387f993") ? true : false;


                return resultItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region filter
        public actionResultGTViewModel filter(SearchGTModel model)
        {
            try
            {
                var filterModel = new ProcessStatusViewModel();
                filterModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");
                var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());
                var query = db.View_GTProcessStatus.AsQueryable();

                //query = query.Where(c => !(c.Document_Status == -2)).OrderByDescending(c => c.Create_Date);

                //AdvanceSearch
                if (model.advanceSearch == true)
                {
                    if (!string.IsNullOrEmpty(model.goodsTransfer_No))
                    {
                        query = query.Where(c => c.GoodsTransfer_No == (model.goodsTransfer_No));
                    }

                    if (!string.IsNullOrEmpty(model.document_Status.ToString()))
                    {
                        query = query.Where(c => c.Document_Status == (model.document_Status));
                    }

                    if (!string.IsNullOrEmpty(model.processStatus_Name))
                    {
                        int DocumentStatue = 0;

                        var StatusName = new List<ProcessStatusViewModel>();

                        var StatusModel = new ProcessStatusViewModel();

                        StatusModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");

                        StatusModel.processStatus_Name = model.processStatus_Name;

                        //GetConfig
                        StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                        if (StatusName.Count > 0)
                        {
                            DocumentStatue = StatusName.FirstOrDefault().processStatus_Id.sParse<int>();
                        }

                        query = query.Where(c => c.Document_Status == DocumentStatue);
                    }

                    //if (!string.IsNullOrEmpty(model.processStatus_Name))
                    //{
                    //    query = query.Where(c => c.ProcessStatus_Name == (model.processStatus_Name));
                    //}

                    if (!string.IsNullOrEmpty(model.documentType_Index.ToString()) && model.documentType_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        query = query.Where(c => c.DocumentType_Index == (model.documentType_Index));
                    }

                    if (!string.IsNullOrEmpty(model.goodsTransfer_Date) && !string.IsNullOrEmpty(model.goodsTransfer_Date_To))
                    {
                        var dateStart = model.goodsTransfer_Date.toBetweenDate();
                        var dateEnd = model.goodsTransfer_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsTransfer_Date >= dateStart.start && c.GoodsTransfer_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsTransfer_Date))
                    {
                        var _goodsTransfer_date_From = model.goodsTransfer_Date.toBetweenDate();
                        query = query.Where(c => c.GoodsTransfer_Date >= _goodsTransfer_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsTransfer_Date_To))
                    {
                        var _goodsTransfer_date_To = model.goodsTransfer_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsTransfer_Date <= _goodsTransfer_date_To.start);
                    }

                    if (!string.IsNullOrEmpty(model.create_Date) && !string.IsNullOrEmpty(model.create_Date_To))
                    {
                        var dateStart = model.create_Date.toBetweenDate();
                        var dateEnd = model.create_Date_To.toBetweenDate();
                        query = query.Where(c => c.Create_Date >= dateStart.start && c.Create_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.create_Date))
                    {
                        var create_Date_From = model.create_Date.toBetweenDate();
                        query = query.Where(c => c.Create_Date >= create_Date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.create_Date_To))
                    {
                        var create_Date_To = model.create_Date_To.toBetweenDate();
                        query = query.Where(c => c.Create_Date <= create_Date_To.start);
                    }
                    else if (!string.IsNullOrEmpty(model.create_By))
                    {
                        query = query.Where(c => c.Create_By == model.create_By);
                    }
                }
                //BasicSearch
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.GoodsTransfer_No.Contains(model.key));
                    }

                    if (!string.IsNullOrEmpty(model.goodsTransfer_Date) && !string.IsNullOrEmpty(model.goodsTransfer_Date_To))
                    {
                        var dateStart = model.goodsTransfer_Date.toBetweenDate();
                        var dateEnd = model.goodsTransfer_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsTransfer_Date >= dateStart.start && c.GoodsTransfer_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsTransfer_Date))
                    {
                        var _goodsTransfer_date_From = model.goodsTransfer_Date.toBetweenDate();
                        query = query.Where(c => c.GoodsTransfer_Date >= _goodsTransfer_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsTransfer_Date_To))
                    {
                        var _goodsTransfer_date_To = model.goodsTransfer_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsTransfer_Date <= _goodsTransfer_date_To.start);
                    }

                    var statusModels = new List<int?>();
                    var sortModels = new List<SortModel>();

                    if (model.status.Count > 0)
                    {
                        foreach (var item in model.status)
                        {
                            if (item.value == 3)
                            {
                                statusModels.Add(3);
                            }
                            if (item.value == 1)
                            {
                                statusModels.Add(1);
                            }
                            if (item.value == 0)
                            {
                                statusModels.Add(0);
                            }
                            if (item.value == -1)
                            {
                                statusModels.Add(-1);
                            }
                        }
                        query = query.Where(c => statusModels.Contains(c.Document_Status));
                    }

                    if (model.sort.Count > 0)
                    {
                        foreach (var item in model.sort)
                        {

                            if (item.value == "GoodsTransfer_No")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "GoodsTransfer_No",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "GoodsTransfer_Date")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "GoodsTransfer_Date",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "DocumentType_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "DocumentType_Name",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "ProcessStatus_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Document_Status",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Create_By")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Create_By",
                                    Sort = "desc"
                                });
                            }

                        }
                        query = query.KWOrderBy(sortModels);
                    }


                }

                var Item = new List<View_GTProcessStatus>();
                var TotalRow = new List<View_GTProcessStatus>();


                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.OrderByDescending(c => c.Create_Date).Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.OrderByDescending(c => c.Create_Date).Take(model.PerPage);

                }

                if (model.sort.Count > 0)
                {
                    Item = query.OrderByDescending(c => c.Create_Date).ToList();
                }
                else
                {
                    Item = query.OrderByDescending(c => c.Create_Date).ToList();
                }

                //if (model.sort.Count > 0)
                //{
                //    var perpages = model.PerPage == 0 ? query.ToList() : query.Skip((model.CurrentPage - 1) * model.PerPage).Take(model.PerPage).ToList();
                //}
                //else
                //{
                //    Item = query.OrderByDescending(c => c.PlanGoodsReceive_Date).ThenByDescending(c => c.PlanGoodsReceive_No).ToList();
                //}

                //var perpages = model.PerPage == 0 ? query.ToList() : query.Skip((model.CurrentPage - 1) * model.PerPage).Take(model.PerPage).ToList();

                var result = new List<SearchGTModel>();
                foreach (var item in Item)
                {
                    var resultItem = new SearchGTModel();

                    var Document_Status = item.Document_Status.ToString();
                    resultItem.goodsTransfer_Index = item.GoodsTransfer_Index;
                    resultItem.goodsTransfer_No = item.GoodsTransfer_No;
                    resultItem.goodsTransfer_Date = item.GoodsTransfer_Date.toString();
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date.toString();
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date.toString();
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.processStatus_Name = Process.Where(a => a.processStatus_Id == Document_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.udf_1 = item.UDF_1;
                    resultItem.udf_2 = item.UDF_2;
                    resultItem.udf_3 = item.UDF_3;
                    resultItem.wcs_Status = item.WCS_Status;
                    resultItem.wcs_Date = item.WCS_Date.toString();
                    result.Add(resultItem);

                }

                var count = TotalRow.Count;
                var actionResultGT = new actionResultGTViewModel();
                actionResultGT.lstGoodsTranfer = result.ToList();
                actionResultGT.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };

                return actionResultGT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DropdownDocumentType
        public List<DocumentTypeViewModel> dropdownDocumentType(DocumentTypeViewModel data)
        {
            try
            {
                var result = new List<DocumentTypeViewModel>();

                var filterModel = new DocumentTypeViewModel();


                filterModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");


                //GetConfig
                result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DropdownStatus
        public List<ProcessStatusViewModel> dropdownStatus(ProcessStatusViewModel data)
        {
            try
            {
                var result = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();


                filterModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");

                //GetConfig
                result = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("dropdownStatus"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateGoodsTransferHeader
        public GoodsTransferViewModel CreateGoodsTransferHeader(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String GoodsTransferNo = "";
            Guid GoodsTransfer_Index = Guid.Empty;

            try
            {

                var GoodsTransferOld = db.IM_GoodsTransfer.Find(data.goodsTransfer_Index);

                if (GoodsTransferOld == null)
                {
                    var filterModel = new DocumentTypeViewModel();
                    //var result = new List<DocumentTypeViewModel>();
                    var result = new List<GenDocumentTypeViewModel>();

                    filterModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");
                    filterModel.documentType_Index = data.documentType_Index;
                    //GetConfig
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
                    itemHeader.GoodsTransfer_Date = data.goodsTransfer_Date.toDateDefault();//Convert.ToDateTime(data.goodsTransfer_Date.toDateTimeString());
                    itemHeader.GoodsTransfer_Time = data.goodsTransfer_Time;
                    itemHeader.GoodsTransfer_Doc_Date = data.goodsTransfer_Doc_Date.toDateDefault();//Convert.ToDateTime(data.goodsTransfer_Doc_Date.toDateTimeString());
                    itemHeader.GoodsTransfer_Doc_Time = data.goodsTransfer_Doc_Time;
                    itemHeader.Owner_Index = data.owner_Index;
                    itemHeader.Owner_Id = data.owner_Id;
                    itemHeader.Owner_Name = data.owner_Name;
                    itemHeader.DocumentType_Index = data.documentType_Index;
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;
                    itemHeader.DocumentRef_No2 = data.documentRef_No2;
                    itemHeader.DocumentRef_No3 = data.documentRef_No3;
                    itemHeader.DocumentRef_No4 = data.documentRef_No4;
                    itemHeader.DocumentRef_Remark = data.documentRef_Remark;

                    itemHeader.Document_Status = -2;

                    itemHeader.Create_By = data.create_By;
                    itemHeader.Create_Date = DateTime.Now;

                    db.IM_GoodsTransfer.Add(itemHeader);

                    data.goodsTransfer_Index = GoodsTransfer_Index;
                    data.goodsTransfer_No = GoodsTransferNo;
                    //data.isUseDocumentType = data.documentType_Index == Guid.Parse("485DAA74-BF94-441F-828D-3D6BFB2C616F") ? true : false;
                    data.isUseDocumentType = data.documentType_Index != Guid.Parse("9056ff09-29df-4bba-8fc5-6c524387f993") ? true : false;
                }
                else
                {
                    GoodsTransferOld.GoodsTransfer_Date = data.goodsTransfer_Date.toDateDefault();//Convert.ToDateTime(data.goodsTransfer_Date.toDateTimeString());
                    GoodsTransferOld.GoodsTransfer_Time = data.goodsTransfer_Time;
                    GoodsTransferOld.GoodsTransfer_Doc_Date = data.goodsTransfer_Doc_Date.toDateDefault();//Convert.ToDateTime(data.goodsTransfer_Doc_Date.toDateTimeString());
                    GoodsTransferOld.GoodsTransfer_Doc_Time = data.goodsTransfer_Doc_Time;
                    GoodsTransferOld.Owner_Index = data.owner_Index;
                    GoodsTransferOld.Owner_Id = data.owner_Id;
                    GoodsTransferOld.Owner_Name = data.owner_Name;
                    GoodsTransferOld.DocumentType_Index = data.documentType_Index;
                    GoodsTransferOld.DocumentType_Id = data.documentType_Id;
                    GoodsTransferOld.DocumentType_Name = data.documentType_Name;
                    GoodsTransferOld.DocumentRef_No2 = data.documentRef_No2;
                    GoodsTransferOld.DocumentRef_No3 = data.documentRef_No3;
                    GoodsTransferOld.DocumentRef_No4 = data.documentRef_No4;
                    GoodsTransferOld.DocumentRef_Remark = data.documentRef_Remark;
                    //GoodsTransferOld.Document_Status = 0;
                    GoodsTransferOld.Update_By = data.create_By;
                    GoodsTransferOld.Update_Date = DateTime.Now;

                    data.isUseDocumentType = data.documentType_Index != Guid.Parse("9056ff09-29df-4bba-8fc5-6c524387f993") ? true : false;
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
                    olog.logging("CreateGoodsTransferHeader", msglog);
                    transaction.Rollback();
                    throw exy;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }
        #endregion

        #region pickProduct
        public actionResultPickbinbalanceViewModel pickProduct(PickbinbalanceViewModel model)
        {
            String State = "Start " + model.sJson();
            String msglog = "";
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
                    //decimal? QtyBal = dataBinbalance.binBalance_QtyBal - dataBinbalance.binBalance_QtyReserve;

                    if (!string.IsNullOrEmpty(model.unit?.productConversion_Index.ToString()))
                    {
                        model.productConversion_Ratio = model.unit.productconversion_Ratio;
                    }

                    var GoodsTransferItem = new IM_GoodsTransferItem();
                    GoodsTransferItem.GoodsTransferItem_Index = Guid.NewGuid();
                    GoodsTransferItem.GoodsTransfer_Index = new Guid(model.goodsTransfer_Index);
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
                    GoodsTransferItem.Location_Index_To = dataBinbalance.location_Index;
                    GoodsTransferItem.Location_Id_To = dataBinbalance.location_Id;
                    GoodsTransferItem.Location_Name_To = dataBinbalance.location_Name;
                    //GoodsIssueItemLocation.Qty = (Decimal)QtyBal / (Decimal)model.productConversion_Ratio;
                    //GoodsIssueItemLocation.Ratio = (Decimal)model.productConversion_Ratio;
                    //GoodsIssueItemLocation.TotalQty = (Decimal)QtyBal;
                    GoodsTransferItem.Qty = (Decimal)model.pick;
                    GoodsTransferItem.Ratio = (Decimal)model.productConversion_Ratio;
                    GoodsTransferItem.TotalQty = (Decimal)model.pick * (Decimal)model.productConversion_Ratio;
                    //GoodsTransferItem.ProductConversion_Index = (Guid)dataBinbalance.productConversion_Index;
                    //GoodsTransferItem.ProductConversion_Id = dataBinbalance.productConversion_Id;
                    //GoodsTransferItem.ProductConversion_Name = dataBinbalance.productConversion_Name;
                    GoodsTransferItem.ProductConversion_Index = model.unit.productConversion_Index;
                    GoodsTransferItem.ProductConversion_Id = model.unit.productConversion_Id;
                    GoodsTransferItem.ProductConversion_Name = model.unit.productConversion_Name;
                    GoodsTransferItem.GoodsReceive_MFG_Date = dataBinbalance.goodsReceive_MFG_Date;
                    GoodsTransferItem.GoodsReceive_MFG_Date_To = dataBinbalance.goodsReceive_MFG_Date;
                    GoodsTransferItem.GoodsReceive_EXP_Date = dataBinbalance.goodsReceive_EXP_Date;
                    GoodsTransferItem.GoodsReceive_EXP_Date_To = dataBinbalance.goodsReceive_EXP_Date;

                    if (dataBinbalance.binBalance_WeightBegin != 0)
                    {
                        var unitWeight = dataBinbalance.binBalance_WeightBegin / dataBinbalance.binBalance_QtyBegin;
                        model.unitWeight = unitWeight;

                        var WeightReserve = ((model.pick * model.productConversion_Ratio) * unitWeight);
                        GoodsTransferItem.Weight = WeightReserve;

                    }

                    if (dataBinbalance.binBalance_VolumeBegin != 0)
                    {
                        var unitVol = dataBinbalance.binBalance_VolumeBegin / dataBinbalance.binBalance_QtyBegin;

                        var VolReserve = ((model.pick * model.productConversion_Ratio) * unitVol);
                        GoodsTransferItem.Volume = VolReserve;
                    }
                    //GoodsTransferItem.Weight = (Decimal)dataBinbalance.binBalance_WeightBal;

                    //GoodsTransferItem.Volume = (Decimal)dataBinbalance.binBalance_VolumeBal;

                    GoodsTransferItem.Document_Status = -3;

                    //GoodsTransferItem.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process

                    GoodsTransferItem.GoodsReceiveItem_Index = dataBinbalance.goodsReceiveItem_Index;
                    GoodsTransferItem.GoodsReceive_Index = dataBinbalance.goodsReceive_Index;
                    GoodsTransferItem.GoodsReceiveItemLocation_Index = dataBinbalance.goodsReceiveItemLocation_Index;

                    GoodsTransferItem.ERP_Location = dataBinbalance.ERP_Location;
                    GoodsTransferItem.ERP_Location_To = dataBinbalance.ERP_Location;

                    if (model.documentType_Index.ToUpper() != "9056FF09-29DF-4BBA-8FC5-6C524387F993")
                    {
                        //getGRI
                        var listGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = dataBinbalance.goodsReceiveItem_Index } };
                        var GRItem = new DocumentViewModel();
                        GRItem.listDocumentViewModel = listGRItem;
                        var GoodsReceiveItem = utils.SendDataApi<List<GoodsReceiveItemV2ViewModel>>(new AppSettingConfig().GetUrl("FindGoodsReceiveItem"), GRItem.sJson());
                        GoodsTransferItem.UDF_1 = dataBinbalance.goodsReceive_No;
                        if (GoodsReceiveItem.Count() > 0)
                        {
                            GoodsTransferItem.UDF_2 = GoodsReceiveItem?.FirstOrDefault().ref_Document_No;
                            //getPGRI
                            var listPGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = GoodsReceiveItem?.FirstOrDefault().ref_DocumentItem_Index } };
                            var PGRItem = new DocumentViewModel();
                            PGRItem.listDocumentViewModel = listPGRItem;
                            var PlanGoodsReceiveItem = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGoodsReceiveItem"), PGRItem.sJson());
                            if (PlanGoodsReceiveItem.Count() > 0)
                            {
                                GoodsTransferItem.UDF_3 = PlanGoodsReceiveItem?.FirstOrDefault().documentRef_No2;
                            }
                        }
                        

                        
                        
                    }


                    GoodsTransferItem.Create_By = model.create_By;
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
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("pickGI", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                    model.goodsTransfer_Index = GoodsTransferItem.GoodsTransfer_Index.ToString();
                    model.goodsTransferItem_Index = GoodsTransferItem.GoodsTransferItem_Index.ToString();
                    model.goodsTransfer_No = model.goodsTransfer_No;
                    model.process_Index = "d8219c2c-15f6-4fc0-b15a-3ce6680970de";
                    //model.GIIL = GoodsIssueItemLocation;
                    model.goodsReceive_Index = dataBinbalance.goodsReceive_Index.ToString();
                    model.goodsReceive_No = dataBinbalance.goodsReceive_No;
                    model.goodsReceive_date = dataBinbalance.goodsReceive_Date.toString();
                    model.erp_Location = dataBinbalance.ERP_Location;
                    var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve_tranfer"), model.sJson());
                    //var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve"), model.sJson());
                    if (insetBinRe.resultIsUse)
                    {
                        model.binCardReserve_Index = insetBinRe.items?.binCardReserve_Index;
                        //model.binCard_Index = insetBinRe.items?.binCard_Index;
                        var update_status_gt = db.IM_GoodsTransferItem.Find(GoodsTransferItem.GoodsTransferItem_Index);
                        update_status_gt.Document_Status = 0;
                        db.SaveChanges();
                    }
                    else
                    {
                        msglog = State + " ex Rollback " + insetBinRe.resultMsg.ToString();
                        olog.logging("pickGT", msglog);
                        result.resultMsg = insetBinRe.resultMsg;
                        result.resultIsUse = false;
                        return result;
                    }
                    result.resultIsUse = true;
                    result.items = model;
                    return result;
                }
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("pickGI", msglog);
                var result = new actionResultPickbinbalanceViewModel();
                result.resultMsg = ex.Message;
                result.resultIsUse = false;
                return result;
            }
        }
        #endregion

        #region deletePickProduct
        public bool deletePickProduct(PickbinbalanceViewModel model)
        {

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var update_status_gti = db.IM_GoodsTransferItem.Find(new Guid(model.goodsTransferItem_Index));
                if (update_status_gti != null)
                {
                    update_status_gti.Document_Status = -1;
                    update_status_gti.Cancel_By = model.create_By;
                    update_status_gti.Cancel_Date = DateTime.Now;
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
                    olog.logging("deletePickProduct", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                var insetBinRe = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateBinCardReserve"), model.sJson());
                if (insetBinRe)
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
                return false;
            }
        }
        #endregion

        #region UpdateStatus
        public Boolean updateDocument(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var GoodsTransfer = db.IM_GoodsTransfer.Find(data.goodsTransfer_Index);

                if (GoodsTransfer != null)
                {
                    GoodsTransfer.Document_Status = data.document_Status;
                    GoodsTransfer.Create_By = data.create_By;
                    GoodsTransfer.Create_Date = DateTime.Now;
                    GoodsTransfer.DocumentRef_Remark = data.documentRef_Remark;

                    foreach (var item in data.listGoodsTransferItemViewModel)
                    {
                        var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransfer.GoodsTransfer_Index && c.UDF_1 == item.udf_1 && c.UDF_2 == item.udf_2).ToList();
                        foreach (var GTI in GoodsTransferItem)
                        {
                            GTI.DocumentRef_No1 = item.documentRef_No1;
                            GTI.Update_By = data.create_By;
                            GTI.Update_Date = DateTime.Now;
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
                        olog.logging("confirmStatus", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateStatusReturnTransNo
        public string updateDocumentReturnTransNo(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var result = "";

            try
            {

                var GoodsTransfer = db.IM_GoodsTransfer.Find(data.goodsTransfer_Index);

                if (GoodsTransfer != null)
                {
                    result = GoodsTransfer.GoodsTransfer_No;
                    if (GoodsTransfer.Document_Status == -2)
                    {
                        GoodsTransfer.Document_Status = data.document_Status;
                    }
                    
                    GoodsTransfer.Create_By = data.create_By;
                    GoodsTransfer.Create_Date = DateTime.Now;
                    GoodsTransfer.DocumentRef_Remark = data.documentRef_Remark;

                    foreach (var item in data.listGoodsTransferItemViewModel)
                    {
                        var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransfer.GoodsTransfer_Index && c.UDF_1 == item.udf_1 && c.UDF_2 == item.udf_2).ToList();
                        foreach (var GTI in GoodsTransferItem)
                        {
                            GTI.DocumentRef_No1 = item.documentRef_No1;
                            GTI.Update_By = data.create_By;
                            GTI.Update_Date = DateTime.Now;
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
                        olog.logging("confirmStatus", msglog);
                        transaction.Rollback();
                        throw exy;
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

        #region ConfirmStatus
        public Boolean confirmDocument(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var GoodsTransfer = db.IM_GoodsTransfer.Find(data.goodsTransfer_Index);
                if (GoodsTransfer == null)
                {
                    return false;
                }
                else
                {
                    
                    GoodsTransfer.Document_Status = 1;
                    GoodsTransfer.Update_By = data.update_By;
                    GoodsTransfer.Update_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatus", msglog);
                        transaction.Rollback();
                        throw exy;
                    }

                    View_AssignJobLocViewModel model = new View_AssignJobLocViewModel();
                    model.goodsTransfer_Index = data.goodsTransfer_Index;
                    model.Create_By = data.create_By;
                    model.Template = "1";
                    AssignService assignService = new AssignService();
                    var assignByLoc = assignService.assignByLoc(model);
                    if (assignByLoc != "true")
                    {
                        return false;
                    }

                    //var _dataPrepareWCS = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == data.goodsTransfer_Index && c.Document_Status == 2 && string.IsNullOrEmpty(c.WCS_status.ToString())).FirstOrDefault();
                    //if (_dataPrepareWCS != null)
                    //{
                    //    var modelTransferReple = new { docNo = _dataPrepareWCS.GoodsTransfer_No };
                    //    var resultSendWCSPutAwayVC = utils.SendDataApi<dynamic>(new AppSettingConfig().GetUrl("SendWCSPutAwayVC"), JsonConvert.SerializeObject(modelTransferReple));
                    //}
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region get_Tranfer_location
        public getTranferLocationViewModel get_Tranfer_location(getTranferLocationViewModel data)
        {

            var olog = new logtxt();
            getTranferLocationViewModel tranferLocationViewModels = new getTranferLocationViewModel();
            try
            {
                List<int> document = new List<int> {-1,3};

                var _location = (from GTFI in db.IM_GoodsTransferItem.Where(c => c.Document_Status == 0).ToList()
                                 join GTF in db.IM_GoodsTransfer.Where(c => !document.Contains(c.Document_Status.GetValueOrDefault())).ToList() on GTFI.GoodsTransfer_Index equals GTF.GoodsTransfer_Index
                                                                        select new 
                                                                        {
                                                                            location_index = GTFI.Location_Index_To,
                                                                            location_name = GTFI.Location_Name_To,
                                                                            location_id = GTFI.Location_Id_To,
                                                                        }).GroupBy(c=> new {
                                                                            c.location_index,
                                                                            c.location_name,
                                                                            c.location_id
                                                                        }).Select(c=> new {
                                                                            c.Key.location_index,
                                                                            c.Key.location_name,
                                                                            c.Key.location_id
                                                                        }).ToList();

                if (_location.Count > 0)
                {
                    
                    foreach (var item in _location)
                    {
                        locationmodel locationmodel = new locationmodel();
                        locationmodel.location_index = item.location_index;
                        locationmodel.location_id = item.location_id;
                        locationmodel.location_name = item.location_name;

                        tranferLocationViewModels.locationmodel.Add(locationmodel);
                    }
                    tranferLocationViewModels.resultIsUse = true;
                }
                
                return tranferLocationViewModels;

            }
            catch (Exception ex)
            {
                tranferLocationViewModels.resultIsUse = false;
                tranferLocationViewModels.resultMsg = ex.Message;
                return tranferLocationViewModels;
            }
        }

        #endregion

        #region ConfirmTransfer
        public Boolean confirmTransfer(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var _tag_Index = "";

            try
            {
                var process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");

                var GoodsTransfer = db.IM_GoodsTransfer.Find(data.goodsTransfer_Index);
                if (GoodsTransfer == null)
                {
                    return false;
                }

                var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == data.goodsTransfer_Index && c.Document_Status != -1);
                var filterModel = new DocumentTypeViewModel();

                filterModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");


                foreach (var items in GoodsTransferItem)
                {
                    #region Get default Location
                    var LocationViewModel = new { location_Name = items.Location_Name_To };
                    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocationV2"), LocationViewModel.sJson());
                    var DataLocation = GetLocation.FirstOrDefault();
                    #endregion


                    var modelTag = new { ref_Document_Index = items.GoodsTransfer_Index, ref_DocumentItem_Index = items.GoodsTransferItem_Index };
                    var dataBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalanceFromReserve"), modelTag.sJson()).Where(c => c.binBalance_QtyReserve > 0).FirstOrDefault();

                    var dataBinbalanceTo = new BinBalanceViewModel();
                    dataBinbalanceTo = null;


                    if (items.Tag_No_To == null)
                    {
                        var checkLocation = db2.wm_BinBalance.Where(c => c.Location_Index == items.Location_Index_To).FirstOrDefault();

                        if (checkLocation != null)
                        {
                            var datacheckTag = db2.wm_BinBalance.Where(c => c.Location_Index == items.Location_Index_To
                                 && c.BinBalance_QtyBal > 0
                                 && c.BinBalance_QtyReserve >= 0).FirstOrDefault();

                            if (datacheckTag != null)
                            {
                                items.Tag_No_To = datacheckTag.Tag_No;
                            }
                            else
                            {
                                return false;

                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(items.Tag_No_To))
                    {
                        var modelTagNew = new { Tag_No = items.Tag_No_To };
                        dataBinbalanceTo = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinBalance"), modelTagNew.sJson()).FirstOrDefault();
                    }


                    if ((items.Location_Id != items.Location_Id_To) || (items.ItemStatus_Id != items.ItemStatus_Id_To) || (items.Tag_No != items.Tag_No_To))
                    {

                        // Has Old Tag
                        if (dataBinbalanceTo != null)
                        {
                            var resultTag = new LPNViewModel();
                            var listTag = new List<DocumentViewModel> { new DocumentViewModel { document_Index = items.Tag_Index_To } };
                            var tag = new DocumentViewModel();
                            tag.listDocumentViewModel = listTag;
                            resultTag = utils.SendDataApi<List<LPNViewModel>>(new AppSettingConfig().GetUrl("FindTag"), tag.sJson()).FirstOrDefault();
                            _tag_Index = resultTag.tag_Index.ToString();
                        }

                        if (_tag_Index == "")
                        {
                            GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                            grModel.goodsReceive_Index = items.GoodsReceive_Index;
                            grModel.owner_Index = GoodsTransfer.Owner_Index;
                            // grModel.tag_No = items.Tag_No;
                            grModel.create_By = data.create_By;
                            _tag_Index = utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTag"), grModel.sJson());
                        }

                        // If tag_Index
                        if (!string.IsNullOrEmpty(_tag_Index) && items.TotalQty != dataBinbalance.binBalance_QtyBal)
                        {
                            #region Check Binbalance binBalance_QtyReserve
                            var binBalanceReserve = new
                            {
                                binbalance_Index = dataBinbalance.binBalance_Index
                            };
                            var dataBinbalanceReserve = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), binBalanceReserve.sJson());
                            #endregion

                            var TagItemIndex = new Guid();
                            //if (dataBinbalance.binBalance_QtyBal == items.Qty && dataBinbalanceReserve.binBalance_QtyBal == dataBinbalanceReserve.binBalance_QtyReserve)

                            if (dataBinbalance.binBalance_QtyBal != items.Qty && dataBinbalanceReserve.binBalance_QtyBal != dataBinbalanceReserve.binBalance_QtyReserve)
                            {
                                GoodsReceiveTagItemViewModel item = new GoodsReceiveTagItemViewModel();

                                item.tag_Index = new Guid(_tag_Index.ToString());
                                item.tagItem_Index = Guid.NewGuid();

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
                                item.qty = items.Qty;
                                item.productConversion_Ratio = Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                item.totalQty = Convert.ToDecimal(items.Qty) * Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                //item.productConversion_Index = new Guid(dataBinbalance.productConversion_Index.ToString());
                                //item.productConversion_Id = dataBinbalance.productConversion_Id;
                                //item.productConversion_Name = dataBinbalance.productConversion_Name;
                                item.productConversion_Index = items.ProductConversion_Index.GetValueOrDefault();
                                item.productConversion_Id = items.ProductConversion_Id;
                                item.productConversion_Name = items.ProductConversion_Name;
                                item.suggest_Location_Index = items.Location_Index_To;
                                item.suggest_Location_Id = items.Location_Id_To;
                                item.suggest_Location_Name = items.Location_Name_To;


                                item.weight = (dataBinbalance.binBalance_UnitWeightBal * items.Qty);//item.weight;
                                item.unitWeight = dataBinbalance.binBalance_UnitWeightBal;
                                item.netWeight = (dataBinbalance.binBalance_UnitNetWeightBal * items.Qty);
                                item.weight_Index = dataBinbalance.binBalance_UnitWeightBal_Index;
                                item.weight_Id = dataBinbalance.binBalance_UnitWeightBal_Id;
                                item.weight_Name = dataBinbalance.binBalance_UnitWeightBal_Name;
                                item.weightRatio = dataBinbalance.binBalance_UnitWeightBalRatio;

                                item.unitGrsWeight = dataBinbalance.binBalance_UnitGrsWeightBal;
                                item.grsWeight = (dataBinbalance.binBalance_UnitGrsWeightBal * items.Qty);
                                item.grsWeight_Index = dataBinbalance.binBalance_UnitGrsWeightBal_Index;
                                item.grsWeight_Id = dataBinbalance.binBalance_UnitGrsWeightBal_Id;
                                item.grsWeight_Name = dataBinbalance.binBalance_UnitGrsWeightBal_Name;
                                item.grsWeightRatio = dataBinbalance.binBalance_UnitGrsWeightBalRatio;

                                item.unitWidth = dataBinbalance.binBalance_UnitWidthBal;
                                item.width = dataBinbalance.binBalance_UnitWidthBal * items.Qty;
                                item.width_Index = dataBinbalance.binBalance_UnitWidthBal_Index;
                                item.width_Id = dataBinbalance.binBalance_UnitWidthBal_Id;
                                item.width_Name = dataBinbalance.binBalance_UnitWidthBal_Name;
                                item.widthRatio = dataBinbalance.binBalance_UnitWidthBalRatio;

                                item.unitLength = dataBinbalance.binBalance_UnitLengthBal;
                                item.length = (dataBinbalance.binBalance_UnitLengthBal / items.Qty);
                                item.length_Index = dataBinbalance.binBalance_UnitLengthBal_Index;
                                item.length_Id = dataBinbalance.binBalance_UnitLengthBal_Id;
                                item.length_Name = dataBinbalance.binBalance_UnitLengthBal_Name;
                                item.lengthRatio = dataBinbalance.binBalance_UnitLengthBalRatio;

                                item.unitHeight = dataBinbalance.binBalance_UnitHeightBal;
                                item.height = dataBinbalance.binBalance_UnitHeightBal * items.Qty;
                                item.height_Index = dataBinbalance.binBalance_UnitHeightBal_Index;
                                item.height_Id = dataBinbalance.binBalance_UnitHeightBal_Id;
                                item.height_Name = dataBinbalance.binBalance_UnitHeightBal_Name;
                                item.heightRatio = dataBinbalance.binBalance_UnitHeightBalRatio;

                                item.unitVolume = item.unitWidth * item.unitLength * item.unitHeight;
                                item.volume = (item.unitVolume * item.qty);//item.volume;

                                item.mfg_Date = dataBinbalance.goodsReceive_MFG_Date.toString();
                                item.exp_Date = dataBinbalance.goodsReceive_EXP_Date.toString();
                                item.tagRef_No1 = "";
                                item.tagRef_No2 = "";
                                item.tagRef_No3 = "";
                                item.tagRef_No4 = "";
                                item.tagRef_No5 = "";
                                item.tag_Status = 1;
                                item.udf_1 = data.udf_1;
                                item.udf_2 = data.udf_2;
                                item.udf_3 = data.udf_3;
                                item.udf_4 = data.udf_4;
                                item.udf_5 = data.udf_5;
                                item.create_By = data.create_By;
                                item.create_Date = DateTime.Now.toString();

                                TagItemIndex = item.tagItem_Index;
                                utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTagItem"), item.sJson());
                            }

                            if (dataBinbalance.binBalance_QtyBal != items.Qty)
                            {
                                GoodsReceiveTagItemViewModel item = new GoodsReceiveTagItemViewModel();

                                item.tag_Index = new Guid(_tag_Index.ToString());
                                item.tagItem_Index = Guid.NewGuid();

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
                                item.qty = items.Qty;
                                item.productConversion_Ratio = Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                item.totalQty = Convert.ToDecimal(items.Qty) * Convert.ToDecimal(dataBinbalance.binBalance_Ratio);
                                //item.productConversion_Index = new Guid(dataBinbalance.productConversion_Index.ToString());
                                //item.productConversion_Id = dataBinbalance.productConversion_Id;
                                //item.productConversion_Name = dataBinbalance.productConversion_Name;
                                item.productConversion_Index = items.ProductConversion_Index.GetValueOrDefault();
                                item.productConversion_Id = items.ProductConversion_Id;
                                item.productConversion_Name = items.ProductConversion_Name;
                                item.suggest_Location_Index = items.Location_Index_To;
                                item.suggest_Location_Id = items.Location_Id_To;
                                item.suggest_Location_Name = items.Location_Name_To;


                                item.weight = (dataBinbalance.binBalance_UnitWeightBal * items.Qty);//item.weight;
                                item.unitWeight = dataBinbalance.binBalance_UnitWeightBal;
                                item.netWeight = (dataBinbalance.binBalance_UnitNetWeightBal * items.Qty);
                                item.weight_Index = dataBinbalance.binBalance_UnitWeightBal_Index;
                                item.weight_Id = dataBinbalance.binBalance_UnitWeightBal_Id;
                                item.weight_Name = dataBinbalance.binBalance_UnitWeightBal_Name;
                                item.weightRatio = dataBinbalance.binBalance_UnitWeightBalRatio;

                                item.unitGrsWeight = dataBinbalance.binBalance_UnitGrsWeightBal;
                                item.grsWeight = (dataBinbalance.binBalance_UnitGrsWeightBal * items.Qty);
                                item.grsWeight_Index = dataBinbalance.binBalance_UnitGrsWeightBal_Index;
                                item.grsWeight_Id = dataBinbalance.binBalance_UnitGrsWeightBal_Id;
                                item.grsWeight_Name = dataBinbalance.binBalance_UnitGrsWeightBal_Name;
                                item.grsWeightRatio = dataBinbalance.binBalance_UnitGrsWeightBalRatio;

                                item.unitWidth = dataBinbalance.binBalance_UnitWidthBal;
                                item.width = dataBinbalance.binBalance_UnitWidthBal * items.Qty;
                                item.width_Index = dataBinbalance.binBalance_UnitWidthBal_Index;
                                item.width_Id = dataBinbalance.binBalance_UnitWidthBal_Id;
                                item.width_Name = dataBinbalance.binBalance_UnitWidthBal_Name;
                                item.widthRatio = dataBinbalance.binBalance_UnitWidthBalRatio;

                                item.unitLength = dataBinbalance.binBalance_UnitLengthBal;
                                item.length = (dataBinbalance.binBalance_UnitLengthBal / items.Qty);
                                item.length_Index = dataBinbalance.binBalance_UnitLengthBal_Index;
                                item.length_Id = dataBinbalance.binBalance_UnitLengthBal_Id;
                                item.length_Name = dataBinbalance.binBalance_UnitLengthBal_Name;
                                item.lengthRatio = dataBinbalance.binBalance_UnitLengthBalRatio;

                                item.unitHeight = dataBinbalance.binBalance_UnitHeightBal;
                                item.height = dataBinbalance.binBalance_UnitHeightBal * items.Qty;
                                item.height_Index = dataBinbalance.binBalance_UnitHeightBal_Index;
                                item.height_Id = dataBinbalance.binBalance_UnitHeightBal_Id;
                                item.height_Name = dataBinbalance.binBalance_UnitHeightBal_Name;
                                item.heightRatio = dataBinbalance.binBalance_UnitHeightBalRatio;

                                item.unitVolume = item.unitWidth * item.unitLength * item.unitHeight;
                                item.volume = (item.unitVolume * item.qty);//item.volume;

                                item.mfg_Date = dataBinbalance.goodsReceive_MFG_Date.toString();
                                item.exp_Date = dataBinbalance.goodsReceive_EXP_Date.toString();
                                item.tagRef_No1 = "";
                                item.tagRef_No2 = "";
                                item.tagRef_No3 = "";
                                item.tagRef_No4 = "";
                                item.tagRef_No5 = "";
                                item.tag_Status = 1;
                                item.udf_1 = data.udf_1;
                                item.udf_2 = data.udf_2;
                                item.udf_3 = data.udf_3;
                                item.udf_4 = data.udf_4;
                                item.udf_5 = data.udf_5;
                                item.create_By = data.create_By;
                                item.create_Date = DateTime.Now.toString();

                                TagItemIndex = item.tagItem_Index;
                                utils.SendDataApi<String>(new AppSettingConfig().GetUrl("createTagItem"), item.sJson());
                            }


                            var ModelLo = new LocationViewModel();

                            ModelLo.location_Name = items.Location_Name_To;
                            //ModelLo.locationType_Index = null;
                            //ModelLo.location_Index = null;
                            var resultStaging = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocationV2"), ModelLo.sJson());

                            var checkLocationStaging = resultStaging.Where(c => c.locationType_Index == new Guid("65A2D25D-5520-47D3-8776-AE064D909285")).FirstOrDefault();

                            if (checkLocationStaging == null)
                            {
                                var binCardModel = new
                                {
                                    binBalance_Index = dataBinbalance.binBalance_Index,
                                    process_Index = process_Index,
                                    documentType_Index = "9056FF09-29DF-4BBA-8FC5-6C524387F993",
                                    documentType_Id = "TF01",
                                    documentType_Name = "โอนย้ายสถานะทั่วไป",
                                    //goodsIssue_Date = data.create_Date,
                                    tagItem_Index = TagItemIndex,
                                    tag_Index = dataBinbalance.tag_Index,
                                    tag_No = dataBinbalance.tag_No,
                                    tag_Index_To = _tag_Index,
                                    tag_No_To = items.Tag_No_To,
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
                                    location_Index_To = dataBinbalanceTo == null ? DataLocation.location_Index : dataBinbalanceTo.location_Index,
                                    location_Id_To = dataBinbalanceTo == null ? DataLocation.location_Id : dataBinbalanceTo.location_Id,
                                    location_Name_To = dataBinbalanceTo == null ? DataLocation.location_Name : dataBinbalanceTo.location_Name,
                                    mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                    exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                    picking_Qty = (Decimal)items.Qty,
                                    picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                    picking_TotalQty = (Decimal)items.Qty * (Decimal)items.Ratio,
                                    Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)items.Qty,
                                    Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)items.Qty,
                                    ref_Document_No = GoodsTransfer.GoodsTransfer_No,
                                    ref_Document_Index = items.GoodsTransfer_Index,
                                    ref_DocumentItem_Index = items.GoodsTransferItem_Index,
                                    userName = data.create_By,
                                    erp_Location = items.ERP_Location,
                                    erp_Location_To = items.ERP_Location_To,
                                    isTransfer = true
                                };
                                var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransfer"), binCardModel.sJson());
                            }

                            else
                            {
                                var binCardModel = new
                                {
                                    binBalance_Index = dataBinbalance.binBalance_Index,
                                    process_Index = filterModel.process_Index,
                                    documentType_Index = data.documentType_Index,
                                    documentType_Id = data.documentType_Id,
                                    documentType_Name = data.documentType_Name,
                                    tag_Index = dataBinbalance.tag_Index,
                                    tag_No = dataBinbalance.tag_No,
                                    tag_Index_To = dataBinbalance.tag_Index,
                                    tag_No_To = dataBinbalance.tag_No,
                                    tagItem_Index = TagItemIndex,
                                    product_Index = dataBinbalance.product_Index,
                                    product_Id = dataBinbalance.product_Id,
                                    product_Name = dataBinbalance.product_Name,
                                    product_SecondName = dataBinbalance.product_SecondName,
                                    product_ThirdName = dataBinbalance.product_ThirdName,
                                    product_Lot = dataBinbalance.product_Lot,
                                    itemStatus_Index = dataBinbalance.itemStatus_Index,
                                    itemStatus_Id = dataBinbalance.itemStatus_Id,
                                    itemStatus_Name = dataBinbalance.itemStatus_Name,
                                    itemStatus_Index_To = items.ItemStatus_Index_To,
                                    itemStatus_Id_To = items.ItemStatus_Id_To,
                                    itemStatus_Name_To = items.ItemStatus_Name_To,
                                    productConversion_Index = dataBinbalance.productConversion_Index,
                                    productConversion_Id = dataBinbalance.productConversion_Id,
                                    productConversion_Name = dataBinbalance.productConversion_Name,
                                    owner_Index = dataBinbalance.owner_Index,
                                    owner_Id = dataBinbalance.owner_Id,
                                    owner_Name = dataBinbalance.owner_Name,
                                    location_Index = dataBinbalance.location_Index,
                                    location_Id = dataBinbalance.location_Id,
                                    location_Name = dataBinbalance.location_Name,
                                    location_Index_To = items.Location_Index_To,
                                    location_Id_To = items.Location_Id_To,
                                    location_Name_To = items.Location_Name_To,
                                    mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                    exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                    picking_Qty = (Decimal)items.Qty,
                                    picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                    picking_TotalQty = (Decimal)items.Qty * (Decimal)items.Ratio,
                                    Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)items.Qty,
                                    Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)items.Qty,
                                    ref_Document_No = GoodsTransfer.GoodsTransfer_No,
                                    ref_Document_Index = items.GoodsTransfer_Index,
                                    ref_DocumentItem_Index = items.GoodsTransferItem_Index,
                                    userName = data.create_By,
                                    erp_Location = items.ERP_Location,
                                    erp_Location_To = items.ERP_Location_To,
                                    isTransfer = true
                                };
                                var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransfer"), binCardModel.sJson());
                            }


                        }
                        else
                        {
                            var binCardModel = new
                            {
                                binBalance_Index = dataBinbalance.binBalance_Index,
                                process_Index = filterModel.process_Index,
                                documentType_Index = data.documentType_Index,
                                documentType_Id = data.documentType_Id,
                                documentType_Name = data.documentType_Name,
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
                                itemStatus_Index_To = items.ItemStatus_Index_To,
                                itemStatus_Id_To = items.ItemStatus_Id_To,
                                itemStatus_Name_To = items.ItemStatus_Name_To,
                                productConversion_Index = dataBinbalance.productConversion_Index,
                                productConversion_Id = dataBinbalance.productConversion_Id,
                                productConversion_Name = dataBinbalance.productConversion_Name,
                                owner_Index = dataBinbalance.owner_Index,
                                owner_Id = dataBinbalance.owner_Id,
                                owner_Name = dataBinbalance.owner_Name,
                                location_Index = dataBinbalance.location_Index,
                                location_Id = dataBinbalance.location_Id,
                                location_Name = dataBinbalance.location_Name,
                                location_Index_To = items.Location_Index_To,
                                location_Id_To = items.Location_Id_To,
                                location_Name_To = items.Location_Name_To,
                                mfg_Date = dataBinbalance.goodsReceive_MFG_Date,
                                exp_Date = dataBinbalance.goodsReceive_EXP_Date,
                                picking_Qty = (Decimal)items.Qty,
                                picking_Ratio = (Decimal)dataBinbalance.binBalance_Ratio,
                                picking_TotalQty = (Decimal)items.Qty * (Decimal)items.Ratio,
                                Weight = ((Decimal)dataBinbalance.binBalance_WeightBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)items.Qty,
                                Volume = ((Decimal)dataBinbalance.binBalance_VolumeBegin / (Decimal)dataBinbalance.binBalance_QtyBegin) * (Decimal)items.Qty,
                                ref_Document_No = GoodsTransfer.GoodsTransfer_No,
                                ref_Document_Index = items.GoodsTransfer_Index,
                                ref_DocumentItem_Index = items.GoodsTransferItem_Index,
                                userName = data.create_By,
                                erp_Location = items.ERP_Location,
                                erp_Location_To = items.ERP_Location_To,
                                isTransfer = true
                            };
                            var CreateTagHeader = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("InsertBinCardTransfer"), binCardModel.sJson());
                        }
                    }
                }


                if (GoodsTransferItem != null)
                {
                    GoodsTransfer.Document_Status = 3;
                    GoodsTransfer.Update_By = data.update_By;
                    GoodsTransfer.Update_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatus", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteStatus
        public Boolean deleteDocument(GoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            bool result = false;

            try
            {

                var GoodsTransfer = db.IM_GoodsTransfer.Find(data.goodsTransfer_Index);
                var GoodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == data.goodsTransfer_Index).ToList();

                List<PickbinbalanceViewModel> modelPick = new List<PickbinbalanceViewModel>();

                if (GoodsTransfer != null)
                {
                    foreach (IM_GoodsTransferItem items in GoodsTransferItem)
                    {
                        PickbinbalanceViewModel model = new PickbinbalanceViewModel();

                        items.Document_Status = -1;

                        model.goodsTransferItem_Index = items.GoodsTransferItem_Index.ToString();
                        model.goodsTransfer_Index = items.GoodsTransfer_Index.ToString();
                        model.ref_DocumentItem_Index = items.GoodsTransferItem_Index.ToString();
                        model.ref_Document_Index = items.GoodsTransfer_Index.ToString();
                        modelPick.Add(model);
                    }

                    foreach (PickbinbalanceViewModel model in modelPick)
                    {
                        var insetBinRe = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateBinCardReserve"), model.sJson());
                    }

                    GoodsTransfer.Document_Status = -1;
                    GoodsTransfer.Cancel_By = data.cancel_By;
                    GoodsTransfer.Cancel_Date = DateTime.Now;
                    result = true;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatus", msglog);
                        transaction.Rollback();
                        throw exy;
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

        #region AutoOwnerfilter
        public List<ItemListViewModel> autoOwnerfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoOwnerFilter"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoWarehousefilter
        public List<ItemListViewModel> autoWarehousefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoWarehousefilter"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoGoodsTransferNo
        public List<ItemListViewModel> autoGoodsTransferNo(ItemListViewModel data)
        {
            try
            {
                TransferDbContext dbTransfer = new TransferDbContext();

                var query = dbTransfer.IM_GoodsTransfer.AsQueryable();

                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.GoodsTransfer_No.Contains(data.key));

                }

                //if (!string.IsNullOrEmpty(data.key))
                //{
                //    query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.key));

                //}

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.GoodsTransfer_Index, c.GoodsTransfer_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.GoodsTransfer_Index,
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

        #region AutoGoodsTransferNo2
        public List<ItemListViewModel> autoGoodsTransferNo2(ItemListViewModel data)
        {
            try
            {
                TransferDbContext dbTransfer = new TransferDbContext();

                var query = dbTransfer.IM_GoodsTransfer.AsQueryable();
                query = query.Where(c => c.Document_Status == 2);

                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.GoodsTransfer_No.Contains(data.key));

                }

                //if (!string.IsNullOrEmpty(data.key))
                //{
                //    query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.key));

                //}

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.GoodsTransfer_Index, c.GoodsTransfer_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.GoodsTransfer_Index,
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

        #region ListPickProduct
        public actionResultPickbinbalanceViewModel ListPickProduct(ListPickbinbalanceViewModel models)
        {
            String State = "Start " + models.sJson();
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var result = new actionResultPickbinbalanceViewModel();

                if (models.items.Count() == 0)
                {
                    result.resultIsUse = false;
                    result.Listitems = models.items;
                    return result;
                }
                foreach (var model in models.items)
                {
                    var dataBinbalance = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), model.sJson());
                    if (!dataBinbalance.resultIsUse)
                    {
                        result.resultMsg = "";
                        result.resultIsUse = false;
                        return result;
                    }
                    else
                    {
                        //decimal? QtyBal = dataBinbalance.binBalance_QtyBal - dataBinbalance.binBalance_QtyReserve;

                        if (!string.IsNullOrEmpty(model.unit?.productConversion_Index.ToString()))
                        {
                            model.productConversion_Ratio = model.unit.productconversion_Ratio;
                        }

                        var GoodsTransferItem = new IM_GoodsTransferItem();
                        GoodsTransferItem.GoodsTransferItem_Index = Guid.NewGuid();
                        GoodsTransferItem.GoodsTransfer_Index = new Guid(model.goodsTransfer_Index);
                        GoodsTransferItem.TagItem_Index = dataBinbalance.tagItem_Index;
                        GoodsTransferItem.Tag_Index = dataBinbalance.tag_Index;
                        GoodsTransferItem.Tag_No = dataBinbalance.tag_No;
                        GoodsTransferItem.Product_Index = dataBinbalance.product_Index;
                        GoodsTransferItem.Product_Id = dataBinbalance.product_Id;
                        GoodsTransferItem.Product_Name = dataBinbalance.product_Name;
                        GoodsTransferItem.Product_SecondName = dataBinbalance.product_SecondName;
                        GoodsTransferItem.Product_ThirdName = dataBinbalance.product_ThirdName;
                        GoodsTransferItem.Product_Lot = dataBinbalance.product_Lot;
                        if (model.documentType_Index.ToUpper() != "9056FF09-29DF-4BBA-8FC5-6C524387F993")
                        {
                            // FIX not in โอนย้ายประเภททั่วไป
                            GoodsTransferItem.ItemStatus_Index = new Guid("CDFB5E82-7984-4169-891F-B309EC3BA7C6");
                            GoodsTransferItem.ItemStatus_Id = "QI";
                            GoodsTransferItem.ItemStatus_Name = "Quality inspection";
                            GoodsTransferItem.ItemStatus_Index_To = new Guid("C043169D-1D73-421B-9E33-69C770DCC3B4");
                            GoodsTransferItem.ItemStatus_Id_To = "UU";
                            GoodsTransferItem.ItemStatus_Name_To = "Unrestricted-use";
                        }
                        else
                        {
                            GoodsTransferItem.ItemStatus_Index = dataBinbalance.itemStatus_Index;
                            GoodsTransferItem.ItemStatus_Id = dataBinbalance.itemStatus_Id;
                            GoodsTransferItem.ItemStatus_Name = dataBinbalance.itemStatus_Name;
                            GoodsTransferItem.ItemStatus_Index_To = dataBinbalance.itemStatus_Index;
                            GoodsTransferItem.ItemStatus_Id_To = dataBinbalance.itemStatus_Id;
                            GoodsTransferItem.ItemStatus_Name_To = dataBinbalance.itemStatus_Name;
                        }

                        if (model.documentType_Index.ToUpper() == "091ADAB5-5EEF-402B-85D3-DF6E67823F3A")
                        {
                            GoodsTransferItem.ItemStatus_Index = new Guid("C043169D-1D73-421B-9E33-69C770DCC3B4");
                            GoodsTransferItem.ItemStatus_Id = "UU";
                            GoodsTransferItem.ItemStatus_Name = "Unrestricted-use";
                            GoodsTransferItem.ItemStatus_Index_To = new Guid("CDFB5E82-7984-4169-891F-B309EC3BA7C6");
                            GoodsTransferItem.ItemStatus_Id_To = "QI";
                            GoodsTransferItem.ItemStatus_Name_To = "Quality inspection";
                        }
                        GoodsTransferItem.Location_Index = dataBinbalance.location_Index;
                        GoodsTransferItem.Location_Id = dataBinbalance.location_Id;
                        GoodsTransferItem.Location_Name = dataBinbalance.location_Name;
                        GoodsTransferItem.Location_Index_To = dataBinbalance.location_Index;
                        GoodsTransferItem.Location_Id_To = dataBinbalance.location_Id;
                        GoodsTransferItem.Location_Name_To = dataBinbalance.location_Name;
                        //GoodsIssueItemLocation.Qty = (Decimal)QtyBal / (Decimal)model.productConversion_Ratio;
                        //GoodsIssueItemLocation.Ratio = (Decimal)model.productConversion_Ratio;
                        //GoodsIssueItemLocation.TotalQty = (Decimal)QtyBal;
                        GoodsTransferItem.Qty = (Decimal)dataBinbalance.binBalance_QtyBal - (Decimal)dataBinbalance.binBalance_QtyReserve;
                        GoodsTransferItem.Ratio = (Decimal)dataBinbalance.binBalance_Ratio;
                        GoodsTransferItem.TotalQty = (Decimal)dataBinbalance.binBalance_QtyBal - (Decimal)dataBinbalance.binBalance_QtyReserve;
                        //GoodsTransferItem.ProductConversion_Index = (Guid)dataBinbalance.productConversion_Index;
                        //GoodsTransferItem.ProductConversion_Id = dataBinbalance.productConversion_Id;
                        //GoodsTransferItem.ProductConversion_Name = dataBinbalance.productConversion_Name;
                        GoodsTransferItem.ProductConversion_Index = model.unit.productConversion_Index;
                        GoodsTransferItem.ProductConversion_Id = model.unit.productConversion_Id;
                        GoodsTransferItem.ProductConversion_Name = model.unit.productConversion_Name;
                        //GoodsTransferItem.MFG_Date = dataBinbalance.goodsReceive_MFG_Date;
                        //GoodsTransferItem.EXP_Date = dataBinbalance.goodsReceive_EXP_Date;
                        GoodsTransferItem.Weight = (Decimal)dataBinbalance.binBalance_WeightBal;

                        GoodsTransferItem.Volume = (Decimal)dataBinbalance.binBalance_VolumeBal;

                        GoodsTransferItem.Document_Status = -3;

                        //GoodsTransferItem.Ref_Process_Index = new Guid("22744590-55D8-4448-88EF-5997C252111F");  // PLAN GI Process

                        GoodsTransferItem.GoodsReceiveItem_Index = dataBinbalance.goodsReceiveItem_Index;
                        GoodsTransferItem.GoodsReceive_Index = dataBinbalance.goodsReceive_Index;
                        GoodsTransferItem.GoodsReceiveItemLocation_Index = dataBinbalance.goodsReceiveItemLocation_Index;

                        if (model.documentType_Index.ToUpper() != "9056FF09-29DF-4BBA-8FC5-6C524387F993")
                        {
                            //getGRI
                            var listGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = dataBinbalance.goodsReceiveItem_Index } };
                            var GRItem = new DocumentViewModel();
                            GRItem.listDocumentViewModel = listGRItem;
                            var GoodsReceiveItem = utils.SendDataApi<List<GoodsReceiveItemV2ViewModel>>(new AppSettingConfig().GetUrl("FindGoodsReceiveItem"), GRItem.sJson());
                            GoodsTransferItem.UDF_1 = dataBinbalance.goodsReceive_No;
                            GoodsTransferItem.UDF_2 = GoodsReceiveItem?.FirstOrDefault()?.ref_Document_No;

                            //getPGRI
                            var listPGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = GoodsReceiveItem?.FirstOrDefault()?.ref_DocumentItem_Index } };
                            var PGRItem = new DocumentViewModel();
                            PGRItem.listDocumentViewModel = listPGRItem;
                            var PlanGoodsReceiveItem = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGoodsReceiveItem"), PGRItem.sJson());
                            GoodsTransferItem.UDF_3 = PlanGoodsReceiveItem?.FirstOrDefault()?.documentRef_No2;
                        }


                        GoodsTransferItem.Create_By = model.create_By;
                        GoodsTransferItem.Create_Date = DateTime.Now;

                        db.IM_GoodsTransferItem.Add(GoodsTransferItem);

                        model.goodsTransfer_Index = GoodsTransferItem.GoodsTransfer_Index.ToString();
                        model.goodsTransferItem_Index = GoodsTransferItem.GoodsTransferItem_Index.ToString();
                        model.goodsTransfer_No = model.goodsTransfer_No;
                        model.process_Index = "d8219c2c-15f6-4fc0-b15a-3ce6680970de";
                        //model.GIIL = GoodsIssueItemLocation;
                        model.goodsReceive_Index = dataBinbalance.goodsReceive_Index.ToString();
                        model.goodsReceive_No = dataBinbalance.goodsReceive_No;
                        model.goodsReceive_date = dataBinbalance.goodsReceive_Date.toString();

                    }
                }

                var transaction = db.Database.BeginTransaction();
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("pickGI", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("ListinsertBinCardReserve"), models.sJson());
                if (insetBinRe.resultIsUse)
                {
                    foreach (var i in models.items)
                    {
                        i.binCardReserve_Index = insetBinRe.items?.binCardReserve_Index;
                        //model.binCard_Index = insetBinRe.items?.binCard_Index;
                        var update_status_gt = db.IM_GoodsTransferItem.Find(Guid.Parse(i.goodsTransferItem_Index));
                        update_status_gt.Document_Status = 0;
                    }
                    db.SaveChanges();
                }
                else
                {
                    msglog = State + " ex Rollback " + insetBinRe.resultMsg.ToString();
                    olog.logging("pickGT", msglog);
                    result.resultMsg = insetBinRe.resultMsg;
                    result.resultIsUse = false;
                    return result;
                }
                result.resultIsUse = true;
                result.Listitems = models.items;
                return result;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("pickGI", msglog);
                var result = new actionResultPickbinbalanceViewModel();
                result.resultMsg = ex.Message;
                result.resultIsUse = false;
                return result;
            }
        }
        #endregion

        #region deletePickProductQI
        public bool deletePickProductQI(PickbinbalanceViewModel model)
        {

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var listsdata = new ListPickbinbalanceViewModel();
                var listdata = new List<PickbinbalanceViewModel>();
                var update_status_gti = db.IM_GoodsTransferItem.Where(w => w.UDF_1 == model.udf_1 && w.UDF_2 == model.udf_2).ToList();
                foreach (var u in update_status_gti)
                {
                    u.Document_Status = -1;
                    u.Cancel_By = model.create_By;
                    u.Cancel_Date = DateTime.Now;
                    var data = new PickbinbalanceViewModel();
                    data.ref_DocumentItem_Index = u.GoodsTransferItem_Index.ToString();
                    data.ref_Document_Index = u.GoodsTransfer_Index.ToString();
                    listdata.Add(data);
                }

                listsdata.items = listdata;
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("deletePickProduct", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                var insetBinRe = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("updateBinCardReserveQI"), listsdata.sJson());
                if (insetBinRe)
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
                return false;
            }
        }
        #endregion

        #region GetReport
        public string GetReport(GoodsTransferViewModel model, string rootPath = "")
        {

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var dataReport = new List<ReportGoodsTransferViewModel>();
                var GT = db.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == model.goodsTransfer_Index && c.Document_Status != -1);

                if (GT == null)
                {
                    return "";
                }

                var GTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == model.goodsTransfer_Index && c.Document_Status != -1).ToList();

                foreach (var G in GTI)
                {
                    var data = new ReportGoodsTransferViewModel();
                    data.goodsTransfer_No = model.goodsTransfer_No;
                    data.goodsTransfer_Date = GT?.GoodsTransfer_Doc_Date == null ? GT.Create_Date.sParse<DateTime>().ToString("dd/MM/yyyy") : GT?.GoodsTransfer_Doc_Date.sParse<DateTime>().ToString("dd/MM/yyyy");
                    data.goodsTransfer_Time = string.IsNullOrEmpty(GT?.GoodsTransfer_Time) ? GT.Create_Date.sParse<DateTime>().ToString("HH:ss") : GT?.GoodsTransfer_Time?.ToString();


                    var listGR = new List<DocumentViewModel> { new DocumentViewModel { document_Index = G.GoodsReceive_Index } };
                    var GR = new DocumentViewModel();
                    GR.listDocumentViewModel = listGR;
                    var GoodsReceive = utils.SendDataApi<List<GoodsReceiveViewModelV2>>(new AppSettingConfig().GetUrl("FindGoodsReceive"), GR.sJson());
                    data.goodsReceive_No = GoodsReceive?.FirstOrDefault()?.goodsReceive_No;

                    data.customer_Code = GT.Owner_Name;
                    data.documentRef_Remark = GT.DocumentRef_Remark;
                    data.tag_No = G.Tag_No;
                    data.product_name = G.Product_Id + " " + G.Product_SecondName;
                    data.qty = G.Qty == null ? 0 : G.Qty.sParse<decimal>();
                    data.ProductConversion_Name = G.ProductConversion_Name;
                    data.Weight = G.Weight == null ? 0 : G.Weight.sParse<decimal>();
                    data.itemStatus_Name = G.ItemStatus_Name;
                    data.itemStatus_Name_To = G.ItemStatus_Name_To;
                    data.location_Name = G.Location_Name;
                    data.location_Name_To = G.Location_Name_To;
                    data.udf_1 = G.UDF_1;
                    data.udf_2 = G.UDF_2;
                    data.udf_3 = G.UDF_3;
                    data.udf_4 = G.UDF_4;
                    data.udf_5 = G.UDF_5;
                    data.mat_Doc = G.Mat_Doc;
                    data.fi_Doc = G.FI_Doc;

                    dataReport.Add(data);
                }

                rootPath = rootPath.Replace("\\TransferAPI", "");
                //var reportPath = rootPath + "\\TransferBusiness\\Reports\\GoodsTransfer\\GoodsTransfer.rdlc";
                var reportPath = rootPath + "\\Reports\\GoodsTransfer\\GoodsTransfer.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", dataReport.OrderBy(c => c.goodsReceive_No).ToList());

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region popupGoodsTransferfilter
        public List<SearchGTModel> popupGoodsTransferfilter(SearchGTModel data)
        {
            try
            {

                var items = new List<SearchGTModel>();



                var query = db.View_GoodsTransfer.AsQueryable();



                if (!string.IsNullOrEmpty(data.goodsTransfer_No))
                {
                    query = query.Where(c => c.GoodsTransfer_No == data.goodsTransfer_No);
                }


                var result = query.Take(100).OrderByDescending(o => o.Create_Date).ToList();

                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("d8219c2c-15f6-4fc0-b15a-3ce6680970de");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                foreach (var item in result)
                {
                    var resultItem = new SearchGTModel();

                    String Statue = "";
                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();


                    resultItem.goodsTransfer_Index = item.GoodsTransfer_Index;
                    resultItem.goodsTransfer_No = item.GoodsTransfer_No;
                    resultItem.goodsTransfer_Date = item.GoodsTransfer_Date.toString();
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.qty = item.Qty;
                    resultItem.processStatus_Name = ProcessStatusName?.processStatus_Name;

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

        #region SentToSap
        //public string SentToSap(ListGoodsTransferViewModel data)
        //{
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();
        //    try
        //    {
        //        var resultMsg = "";

        //        if (data.items.Count() == 0)
        //        {
        //            return "Please Select Order,";
        //        }

        //        foreach (var item in data.items)
        //        {
        //            State = "Select GT";
        //            var GT = db.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == item.goodsTransfer_Index && c.Document_Status == 3);

        //            if (GT == null)
        //            {
        //                resultMsg += item.goodsTransfer_No + " Order Not Complete,";
        //                continue;
        //            }

        //            State = "Select GTI";
        //            var GTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GT.GoodsTransfer_Index).ToList();
        //            State = "Group GTI by GoodsReceive";
        //            var groupGTI = GTI.GroupBy(g => g.GoodsReceive_Index).ToList();

        //            foreach (var GTIS in groupGTI)
        //            {
        //                State = "Get GR";
        //                var listGR = new List<DocumentViewModel> { new DocumentViewModel { document_Index = GTIS.Key } };
        //                var GR = new DocumentViewModel();
        //                GR.listDocumentViewModel = listGR;
        //                var GoodsReceive = utils.SendDataApi<List<GoodsReceiveViewModelV2>>(new AppSettingConfig().GetUrl("FindGoodsReceive"), GR.sJson()).FirstOrDefault();

        //                var Request = new TransferRequestViewModel();
        //                Request.GrNo = GoodsReceive.goodsReceive_No;
        //                Request.DocDate = GT.GoodsTransfer_Doc_Date.toString().Substring(0, 8);
        //                Request.HeaderTxt = GoodsReceive.documentRef_No4;
        //                Request.GmCode = "04";

        //                string movetype = "";
        //                if (GT.DocumentType_Id == "TF01")
        //                    movetype = "301";
        //                else if (GT.DocumentType_Id == "TF02" || GT.DocumentType_Id == "TF03")
        //                    movetype = "321";

        //                State = "Select GTI Where GoodsReceive";
        //                var GoodsTransferItem = GTI.Where(c => c.GoodsReceive_Index == GTIS.Key).ToList();
        //                foreach (var i in GoodsTransferItem)
        //                {
        //                    var RequestDetail = new TransferRequestDetail();
        //                    RequestDetail.Material = i.Product_Id;
        //                    RequestDetail.Plant = "9900";
        //                    RequestDetail.StgeLoc = "9930";
        //                    RequestDetail.Batch = i.Product_Lot;
        //                    RequestDetail.MoveType = movetype;
        //                    RequestDetail.EntryQnt = Math.Round(i.Qty.sParse<decimal>(), 3);
        //                    RequestDetail.EntryUom = i.ProductConversion_Name;
        //                    RequestDetail.MovePlant = "3000";
        //                    RequestDetail.MoveStloc = "3010";
        //                    RequestDetail.StckType = "Unrestricted-use";

        //                    Request.Detail.Add(RequestDetail);
        //                }
        //                State = "Sent To Sap";
        //                var result = utils.SendDataApi<TransferResponseViewModel>(new AppSettingConfig().GetUrl("SentToSap"), Request.sJson());

        //                if (result.status == "SUCCESS")
        //                {
        //                    State = "response SUCCESS";

        //                }
        //                else if (result.status == "ERROR")
        //                {
        //                    State = "response ERROR";
        //                    resultMsg += GT.GoodsTransfer_No + " ERROR " + result.message.eMessageField + ",";
        //                }
        //                else
        //                {
        //                    State = "response";
        //                    resultMsg += GT.GoodsTransfer_No + " ERROR,";
        //                }
        //            }
        //        }

        //        var transactionx = db.Database.BeginTransaction();
        //        try
        //        {
        //            State = "SaveChanges";

        //            db.SaveChanges();
        //            transactionx.Commit();
        //        }

        //        catch (Exception exy)
        //        {
        //            msglog = State + " exy Rollback " + exy.Message.ToString();
        //            olog.logging("SentToSapGr", msglog);
        //            transactionx.Rollback();

        //            throw exy;

        //        }

        //        return resultMsg;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region SentToSapGetJson
        public string SentToSapGetJson(string data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var resultMsg = "";

                State = "Select GT";
                var GT = db.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == new Guid(data));

                if (GT == null)
                {
                    resultMsg += " Order Not Complete,";
                }

                State = "Select GTI";
                var GTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GT.GoodsTransfer_Index).ToList();
                State = "Group GTI by GoodsReceive";
                var groupGTI = GTI.GroupBy(g => g.GoodsReceive_Index).ToList();

                foreach (var GTIS in groupGTI)
                {
                    State = "Get GR";
                    var listGR = new List<DocumentViewModel> { new DocumentViewModel { document_Index = GTIS.Key } };
                    var GR = new DocumentViewModel();
                    GR.listDocumentViewModel = listGR;
                    var GoodsReceive = utils.SendDataApi<List<GoodsReceiveViewModelV2>>(new AppSettingConfig().GetUrl("FindGoodsReceive"), GR.sJson()).FirstOrDefault();

                    var Request = new TransferRequestViewModel();
                    Request.GrNo = GoodsReceive.goodsReceive_No;
                    Request.DocDate = GT.GoodsTransfer_Doc_Date.toString().Substring(0, 8);
                    Request.HeaderTxt = GoodsReceive.documentRef_No4;
                    Request.GmCode = "04";

                    string movetype = "";
                    if (GT.DocumentType_Id == "TF01")
                        movetype = "301";
                    else if (GT.DocumentType_Id == "TF02" || GT.DocumentType_Id == "TF03")
                        movetype = "321";

                    State = "Select GTI Where GoodsReceive";
                    var GoodsTransferItem = GTI.Where(c => c.GoodsReceive_Index == GTIS.Key).ToList();
                    foreach (var i in GoodsTransferItem)
                    {
                        var RequestDetail = new TransferRequestDetail();
                        RequestDetail.Material = i.Product_Id;
                        RequestDetail.Plant = "9900";
                        RequestDetail.StgeLoc = "9930";
                        RequestDetail.Batch = i.Product_Lot;
                        RequestDetail.MoveType = movetype;
                        RequestDetail.EntryQnt = Math.Round(i.Qty.sParse<decimal>(), 3);
                        RequestDetail.EntryUom = i.ProductConversion_Name;
                        RequestDetail.MovePlant = "3000";
                        RequestDetail.MoveStloc = "3010";
                        RequestDetail.StckType = "";

                        Request.Detail.Add(RequestDetail);
                    }
                    State = "Sent To Sap";
                    resultMsg += Request.sJson() + Environment.NewLine;

                }

                return resultMsg;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region sentToSap
        public string sentToSap(ListGoodsTransferViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String msg = "";
            String msgTransfer = "";
            String msgError = "";
            try
            {
                foreach (var item in data.items.Where(c => c.selected))
                {
                    var GoodsTransfer = db.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == item.goodsTransfer_Index && c.Document_Status == 3);
                    if (GoodsTransfer == null)
                    {
                        msg += item.goodsTransfer_No + " เอกสารนี้ยังไม่เสร็จสิ้น,";
                        continue;
                    }

                    //foreach (var itemData in data.items.Where(c => c.selected))
                    //{
                    //    if (itemData.documentType_Id == "TF04")
                    //    {
                    //        msg += "เลขที่ใบโอนย้าย:" + itemData.goodsTransfer_No + " ประเภทเอกสารไม่ถูกต้อง" + " ,";

                    //        return msg;

                    //    }
                    //}

                    if (GoodsTransfer.DocumentType_Id != "TF04")
                    {
                        State = "Select GTI";
                        var GTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransfer.GoodsTransfer_Index && c.Document_Status != -1 && (c.Mat_Doc == null || c.Mat_Doc == string.Empty) && (c.FI_Doc == null || c.FI_Doc == string.Empty)).ToList();

                        State = "Group GTI by GoodsReceive";
                        var groupGTI = GTI.GroupBy(g => g.GoodsReceive_Index).ToList();
                        if (groupGTI.Count == 0)
                        {
                            msgTransfer += "เลขที่เอกสาร " + item.goodsTransfer_No + " ถูกส่ง SAP แล้ว,";
                            continue;
                        }



                        foreach (var GTIS in groupGTI)
                        {
                            State = "Get GR";
                            var listGR = new List<DocumentViewModel> { new DocumentViewModel { document_Index = GTIS.Key } };
                            var GR = new DocumentViewModel();
                            GR.listDocumentViewModel = listGR;
                            var GoodsReceive = utils.SendDataApi<List<GoodsReceiveViewModelV2>>(new AppSettingConfig().GetUrl("FindGoodsReceive"), GR.sJson()).FirstOrDefault();

                            var Request = new TransferRequestViewModel();
                            Request.GrNo = GoodsReceive.goodsReceive_No;
                            Request.DocDate = GoodsTransfer.GoodsTransfer_Doc_Date.toString().Substring(0, 8);
                            Request.HeaderTxt = GoodsReceive.documentRef_No4;
                            Request.GmCode = "04";

                            string movetype = "";
                            if (GoodsTransfer.DocumentType_Id == "TF01")
                                movetype = "301";
                            else if (GoodsTransfer.DocumentType_Id == "TF02" || GoodsTransfer.DocumentType_Id == "TF03")
                                movetype = "321";

                            State = "Select GTI Where GoodsReceive";
                            var whereGTI = GTI.Where(c => c.GoodsReceive_Index == GTIS.Key && (c.Mat_Doc == null || c.Mat_Doc == string.Empty) && (c.FI_Doc == null || c.FI_Doc == string.Empty)).ToList();
                            foreach (var i in whereGTI)
                            {
                                var RequestDetail = new TransferRequestDetail();
                                RequestDetail.Material = i.Product_Id;
                                RequestDetail.Plant = (GoodsTransfer.DocumentType_Id == "TF02") ? "" : "9900";
                                RequestDetail.StgeLoc = (GoodsTransfer.DocumentType_Id == "TF02") ? "" : "9930";
                                RequestDetail.Batch = i.Product_Lot;
                                RequestDetail.MoveType = movetype;
                                RequestDetail.EntryQnt = Math.Round(i.Qty.sParse<decimal>(), 3);
                                RequestDetail.EntryUom = i.ProductConversion_Name;
                                RequestDetail.MovePlant = "3000";
                                RequestDetail.MoveStloc = "3010";
                                RequestDetail.StckType = "";

                                Request.Detail.Add(RequestDetail);
                            }

                            Request.AuthSAP = new AuthenticationModel();
                            Request.AuthSAP.Username = data.authSAP.Username;
                            Request.AuthSAP.Password = data.authSAP.Password;

                            State = "Sent To Sap";

                            Task t = Task.Run(() =>
                            {
                                Task.Delay(5000).Wait(); // 5 seconds.
                            });

                            var result = utils.SendDataApi<TransferResponseViewModel>(new AppSettingConfig().GetUrl("SentToSap"), Request.sJson());
                            //var result = new TransferResponseViewModel { status = "SUCCESS", message = new TFMessage { eMaterailDocField = GoodsTransfer.GoodsTransfer_No.Substring(2, 8), eFiDocumentField = GoodsTransfer.GoodsTransfer_No.Substring(2, 8) } };

                            if (result.status == "SUCCESS")
                            {
                                State = "response SUCCESS";
                                foreach (var i in whereGTI)
                                {
                                    i.FI_Doc = result.message.eFiDocumentField;
                                    i.Mat_Doc = result.message.eMaterailDocField;
                                    i.Update_Date = DateTime.Now;
                                    i.Update_By = item.create_By;
                                }
                                msg += "เลขที่ใบโอนย้าย :" + GoodsTransfer.GoodsTransfer_No + " ส่ง SAP เรียบร้อย ,";

                                //// check ต้องได้ match doc ครบถึง update เป็น 11
                                //var checkGTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransfer.GoodsTransfer_Index && c.Document_Status != -1 && (c.Mat_Doc == null || c.Mat_Doc == string.Empty) && (c.FI_Doc == null || c.FI_Doc == string.Empty)).ToList();

                                //if(checkGTI.Count == 0)
                                //{
                                //    GoodsTransfer.Document_Status = 11;
                                //    GoodsTransfer.Update_By = item.create_By;
                                //    GoodsTransfer.Update_Date = DateTime.Now;
                                //} 
                            }
                            //else if (result.status == "ERROR")
                            //    return false;
                            //else
                            //    return false;

                            //if (result.status == "SUCCESS")
                            //{
                            //    State = "response SUCCESS";

                            //}

                            if (result.status == "ERROR")
                            {
                                State = "response ERROR";

                                //msg += "เลขที่ใบโอนย้าย :" + GoodsTransfer.GoodsTransfer_No + " ส่ง Sap ไม่ผ่าน " + result.message.eMessageField + " ,";
                                msgError += "เลขที่ใบโอนย้าย :" + GoodsTransfer.GoodsTransfer_No + " ส่ง Sap ไม่ผ่าน " + result.message.eMessageField + " ,";
                                continue;
                            }
                            else
                            {
                                State = "response";
                                msg += "เลขที่ใบโอนย้าย :" + GoodsTransfer.GoodsTransfer_No + " ส่ง Sap ผ่าน " + result.message.eMessageField + " ,";
                                continue;

                            }

                        }

                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            State = "SaveChanges";

                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " exy Rollback " + exy.Message.ToString();
                            olog.logging("SentToSapGT", msglog);
                            transaction.Rollback();

                            throw exy;

                        }

                    }
                    else
                    {
                        msg += "เลขที่ใบโอนย้าย:" + item.goodsTransfer_No + " ประเภทเอกสารไม่ถูกต้อง" + " ,";
                        continue;
                    }

                    // check ต้องได้ match doc ครบถึง update เป็น 11
                    var checkGTI = db.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransfer.GoodsTransfer_Index && c.Document_Status != -1 && (c.Mat_Doc == null || c.Mat_Doc == string.Empty) && (c.FI_Doc == null || c.FI_Doc == string.Empty)).ToList();

                    if (checkGTI.Count == 0)
                    {
                        GoodsTransfer.Document_Status = 11;
                        GoodsTransfer.Update_By = item.create_By;
                        GoodsTransfer.Update_Date = DateTime.Now;

                        //add msg
                        msgTransfer += "เลขที่ใบโอนย้าย :" + GoodsTransfer.GoodsTransfer_No + " ส่ง SAP เรียบร้อย ,";
                    }
                    else
                    {
                        msgTransfer += "เลขที่ใบโอนย้าย :" + GoodsTransfer.GoodsTransfer_No + " ส่ง SAP ไม่ผ่าน ," + msgError;
                    }

                    var transactionx = db.Database.BeginTransaction();
                    try
                    {
                        State = "SaveChanges GoodsTransfer";

                        db.SaveChanges();
                        transactionx.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " exy Rollback " + exy.Message.ToString();
                        olog.logging("SentToSapGT", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }

                }

                return msgTransfer;//msg;


            }
            catch (Exception ex)
            {
                return ex.Message;
                //throw ex;
            }
        }
        #endregion

        #region ListdeletePickProduct
        public bool ListdeletePickProduct(ListPickbinbalanceViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                //foreach (var m in model.items)
                //{
                //    var GT = db.IM_GoodsTransfer.Where(c => c.GoodsTransfer_Index == new Guid(m.goodsTransfer_Index) && c.Document_Status == -2).FirstOrDefault();
                //    //var GTI = db.IM_GoodsTransferItem.Find(GT.FirstOrDefault().GoodsTransfer_Index);

                //    var itemReserve = db2.wm_BinCardReserve.Where(c => c.Ref_Document_Index == GT.GoodsTransfer_Index && c.BinCardReserve_Status != -1).ToList();

                //    if (itemReserve.Count() == null)
                //    {
                //        return false;
                //    }
                //    foreach (var ir in itemReserve)
                //    {
                //        ir.BinCardReserve_Status = -1;
                //        var itemBin = db2.wm_BinBalance.Find(ir.BinBalance_Index);

                //        itemBin.BinBalance_QtyReserve = itemBin.BinBalance_QtyReserve - ir.BinCardReserve_QtyBal;
                //        itemBin.BinBalance_WeightReserve = itemBin.BinBalance_WeightReserve - ir.BinCardReserve_WeightBal;
                //        itemBin.BinBalance_VolumeReserve = itemBin.BinBalance_VolumeReserve - ir.BinCardReserve_VolumeBal;
                //    }
                //    // db.RemoveRange(GT);

                //}
                //var transaction = db2.Database.BeginTransaction();
                //try
                //{
                //    db2.SaveChanges();
                //    transaction.Commit();
                //}

                //catch (Exception exy)
                //{
                //    msglog = State + " ex Rollback " + exy.Message.ToString();
                //    olog.logging("deletePickProduct", msglog);
                //    transaction.Rollback();
                //    throw exy;
                //}
                return false;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("deletePickProduct", msglog);
                return false;
            }
        }
        #endregion
        
        #region printTagPutawayTransfer
        //public string printTagPutawayTransfer(TagPutawayTransferViewModel data, string rootPath = "")
        //{
        //    var culture = new System.Globalization.CultureInfo("en-US");
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    try
        //    {

        //        var resTagItem = new List<TagPutawayTransferViewModel>();


        //        var queryGoodsTransferItem = db.IM_GoodsTransferItem.Where(c => c.GoodsTransferItem_Index == data.goodsTransferItem_Index).ToList();

        //        var listGR = new List<DocumentViewModel>();
        //        var listGRI = new List<DocumentViewModel>();
        //        foreach (var item in data.listTagPutawayTransferViewModel)
        //        {
        //            listGR.Add(new DocumentViewModel { document_Index = item.goodsReceive_Index });
        //        }

        //        var GR = new DocumentViewModel();
        //        GR.listDocumentViewModel = listGR;
        //        var View_RPT_PrintOutTag = utils.SendDataApi<List<View_RPT_PrintOutTag>>(new AppSettingConfig().GetUrl("View_RPT_PrintOutTag"), GR.sJson());

        //        //var gr = GoodsReceive.ToList();
        //        //var gri = GoodsReceiveItem.ToList();


        //        //var query = (from GR_GRI in View_RPT_PrintOutTag
        //        //             join GT in queryGoodsTransferItem on GR_GRI.GoodsReceive_Index equals GT.GoodsReceive_Index into grJoinGT
        //        //             from rr in grJoinGT.DefaultIfEmpty()
        //        //             select new
        //        //             {
        //        //                 Gt = rr,
        //        //                 VRP = GR_GRI,
        //        //             }).ToList();


        //        var ProductConversionModel = new ProductConversionViewModel();
        //        var resultProductConversion = new List<ProductConversionViewModel>();
        //        resultProductConversion = utils.SendDataApi<List<ProductConversionViewModel>>(new AppSettingConfig().GetUrl("dropdownProductConversionV2"), ProductConversionModel.sJson());

        //        string DatePrint = DateTime.Now.ToString("dd/MM/yyyy", culture);
        //        var time = DateTime.Now.ToString("HH:mm");

        //        foreach (var item in data.listTagPutawayTransferViewModel.OrderBy(o => o.product_Id))
        //        {

        //            var resultItem = new TagPutawayTransferViewModel();

        //            var gr = View_RPT_PrintOutTag.FirstOrDefault(c => c.GoodsReceiveItem_Index == item.goodsReceiveItem_Index);
        //            var GT = db.IM_GoodsTransfer.Find(item.goodsTransfer_Index);
        //            //var DataTag = query.FirstOrDefault(c => c?.Gt?.Tag_Index_To == item.tag_Index && c?.Gt?.GoodsReceive_Index == item.goodsReceive_Index);

        //            //DataTag = DataTag == null ? query.FirstOrDefault(c => c?.VRP?.Tag_No == item.tag_No) : DataTag;

        //            //resultItem.location_Id_To = !string.IsNullOrEmpty(DataTag?.Gt?.Location_Id_To) ? DataTag?.Gt?.Location_Id_To  : DataTag?.VRP?.Location_Id;
        //            //resultItem.location_Id = !string.IsNullOrEmpty(DataTag?.Gt?.Location_Id) ? "(TF-" + DataTag?.Gt?.Location_Id + ")" : "";
        //            //resultItem.owner_Name = !string.IsNullOrEmpty(DataTag?.Gt?.Owner_Name) ? db.IM_GoodsTransfer.FirstOrDefault(f => f.GoodsTransfer_Index == DataTag.Gt.GoodsTransfer_Index).Owner_Name : DataTag?.VRP?.Owner_Name;
        //            //resultItem.product_Id = !string.IsNullOrEmpty(DataTag?.Gt?.Product_Id) ? DataTag?.Gt?.Product_Id : DataTag?.VRP?.Product_Id;
        //            //resultItem.product_Name = !string.IsNullOrEmpty(DataTag?.Gt?.Product_Name) ? DataTag?.Gt?.Product_Name : DataTag?.VRP?.Product_Name;
        //            //resultItem.qty = !string.IsNullOrEmpty(DataTag?.Gt?.Qty.ToString()) ? Convert.ToDecimal(DataTag?.Gt?.Qty) : Convert.ToDecimal(DataTag?.VRP?.Qty);
        //            //resultItem.tag_NoBarcode = !string.IsNullOrEmpty(DataTag?.Gt?.Tag_No_To) ? new NetBarcode.Barcode(DataTag?.Gt?.Tag_No_To, NetBarcode.Type.Code128B).GetBase64Image() : new NetBarcode.Barcode(DataTag?.VRP?.Tag_No, NetBarcode.Type.Code128B).GetBase64Image();
        //            //resultItem.productConversion_Name = !string.IsNullOrEmpty(DataTag?.Gt?.Tag_No_To) ? DataTag?.Gt?.ProductConversion_Name : DataTag?.VRP?.ProductConversion_Name;
        //            //resultItem.tag_No = !string.IsNullOrEmpty(DataTag?.Gt?.Tag_No_To) ? DataTag?.Gt?.Tag_No_To : DataTag?.VRP?.Tag_No;

        //            //string date = DataTag?.VRP?.GoodsReceive_Date.toString();
        //            //string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
        //            //System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
        //            //resultItem.goodsReceive_Date = GRDate;
        //            //resultItem.goodsReceive_No = DataTag?.VRP?.GoodsReceive_No;
        //            //resultItem.warehouse_Id = (DataTag?.Gt != null ? "TR_" : "R_") + DataTag?.VRP?.Warehouse_Id;

        //            //resultItem.planGoodsReceive_No = DataTag?.VRP?.Ref_Document_No;

        //            //if (item.productConversion_Index != null)
        //            //{
        //            //    if (resultProductConversion.Count > 0 && resultProductConversion != null)
        //            //    {
        //            //        var DataProductConversion = resultProductConversion.Find(c => c.productConversion_Index == item.productConversion_Index);
        //            //        if (DataProductConversion != null)
        //            //        {
        //            //            resultItem.ref_no1 = DataProductConversion.ref_No1;
        //            //            resultItem.ref_no2 = DataProductConversion.ref_No2;
        //            //            resultItem.ref_no3 = DataTag?.Gt != null ? DataProductConversion.ref_No3 : "";
        //            //        }
        //            //    }
        //            //}

        //            resultItem.location_Id_To = item.location_Id_To;
        //            resultItem.location_Id = item.location_Id;
        //            resultItem.owner_Name = GT?.Owner_Name;
        //            resultItem.product_Id = item.product_Id;
        //            resultItem.product_Name = item.product_Name;
        //            resultItem.qty = item.qty;
        //            resultItem.tag_NoBarcode = new NetBarcode.Barcode(item.tag_No_To, NetBarcode.Type.Code128B).GetBase64Image();
        //            resultItem.productConversion_Name = item.productConversion_Name;
        //            resultItem.tag_No = item.tag_No_To;

        //            string date = gr?.GoodsReceive_Date.toString();
        //            string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
        //            System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
        //            resultItem.goodsReceive_Date = GRDate;
        //            resultItem.goodsReceive_No = gr?.GoodsReceive_No;
        //            resultItem.goodsTransfer_No = GT?.GoodsTransfer_No;
        //            //resultItem.warehouse_Id = item.location_Id;

        //            resultItem.planGoodsReceive_No = gr?.Ref_Document_No;

        //            if (item.productConversion_Index != null)
        //            {
        //                if (resultProductConversion.Count > 0 && resultProductConversion != null)
        //                {
        //                    var DataProductConversion = resultProductConversion.Find(c => c.productConversion_Index == item.productConversion_Index);
        //                    if (DataProductConversion != null)
        //                    {
        //                        resultItem.ref_no1 = DataProductConversion.ref_No1;
        //                        resultItem.ref_no2 = DataProductConversion.ref_No2;
        //                        resultItem.ref_no3 = DataProductConversion.ref_No3;
        //                    }
        //                }
        //            }


        //            resultItem.date_Print = DatePrint + " " + time;


        //            resTagItem.Add(resultItem);

        //        }


        //        resTagItem.ToList();



        //        rootPath = rootPath.Replace("\\TransferAPI", "");
        //        var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportTagPutawayTransfer");
        //        LocalReport report = new LocalReport(reportPath);
        //        report.AddDataSource("DataSet1", resTagItem);

        //        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //        string fileName = "";
        //        string fullPath = "";
        //        fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

        //        var renderedBytes = report.Execute(RenderType.Pdf);

        //        Utils objReport = new Utils();
        //        fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
        //        var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
        //        return saveLocation;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #region printTagPutawayTransferNew
        public string printTagPutawayTransferNew(TagPutawayTransferViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var resTagItem = new List<ReportTagPutawayViewModel>();
                //var queryGr = db.View_RPT_PrintOutTag.FirstOrDefault(c => c.GoodsReceive_Index == data.listLPNItemViewModel[0].goodsReceive_Index);

                //string DatePrint = DateTime.Now.ToString("dd/MM/yyyy", culture);
                //var time = DateTime.Now.ToString("HH:mm");

                //string date = queryGr.GoodsReceive_Date.toString();
                //string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                //System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                var ProductBarcode = "";
                var RefNo1 = "";
                var RefNo2 = "";
                var RefNo3 = "";
                var ShortNameUnit = "";

                var datatag = (from tag in data.listTagPutawayTransferViewModel
                               join gri in dbInbound.IM_GoodsReceiveItem on tag.goodsReceiveItem_Index equals gri.GoodsReceiveItem_Index
                               select new
                               {
                                   tag.productConversion_Index,
                                   tag.productConversion_Name,
                                   tag.product_Id,
                                   tag.product_Name,
                                   tag.tag_No,
                                   tag.qty,
                                   //tag.suggest_Location_Id,
                                   tag.suggest_Location_Name,
                                   gri.Create_Date,
                                   gri.GoodsReceive_Index,
                                   gri.GoodsReceiveItem_Index
                               }
                               ).ToList();

                var ProductConversionModel = new ProductConversionViewModel();
                var resultProductConversion = new List<ProductConversionViewModel>();
                resultProductConversion = utils.SendDataApi<List<ProductConversionViewModel>>(new AppSettingConfig().GetUrl("dropdownProductConversionV2"), ProductConversionModel.sJson());

                foreach (var item in datatag.OrderBy(o => o.tag_No))
                {
                    var GoodsReceive_Index = new SqlParameter("@GoodsReceive_Index", item.GoodsReceive_Index);
                    var GoodsReceiveItem_Index = new SqlParameter("@GoodsReceiveItem_Index", item.GoodsReceiveItem_Index);
                    var tag_no = new SqlParameter("@tag_no", item.tag_No);
                    var queryGr = dbInbound.View_RPT_PrintOutTag_RePrint.FromSql("sp_RPT_PrintOutTag_RePrint @GoodsReceive_Index, @GoodsReceiveItem_Index,@tag_no", GoodsReceive_Index, GoodsReceiveItem_Index, tag_no).FirstOrDefault();

                    if (queryGr != null)
                    {
                        dbInbound.Entry(queryGr).State = EntityState.Detached;
                    }

                    //var queryGr = dbInbound.View_RPT_PrintOutTag_RePrint.FirstOrDefault(c => c.GoodsReceive_Index == item.GoodsReceive_Index && c.GoodsReceiveItem_Index == item.GoodsReceiveItem_Index && c.Tag_No == item.tag_No);

                    string DatePrint = DateTime.Now.ToString("dd/MM/yyyy", culture);
                    var time = DateTime.Now.ToString("HH:mm");

                    string date = queryGr.GoodsReceive_Date.toString();
                    string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                    var resultItem = new ReportTagPutawayViewModel();
                    if (item.productConversion_Index != null)
                    {
                        
                        if (resultProductConversion.Count > 0 && resultProductConversion != null)
                        {
                            var DataProductConversion = resultProductConversion.Find(c => c.productConversion_Index == item.productConversion_Index);
                            if (DataProductConversion != null)
                            {
                                RefNo1 = DataProductConversion.ref_No1;
                                RefNo2 = DataProductConversion.ref_No2;
                                RefNo3 = DataProductConversion.ref_No3;
                                ShortNameUnit = (DataProductConversion.sale_UNIT == 0 && DataProductConversion.in_UNIT == 0) ? "" : (DataProductConversion.sale_UNIT == 1) ? "SU" : "IU";
                            }
                        }

                        //var ProductConversionBarcodeModel = new ProductConversionBarcodeViewModel();
                        //var resultProductConversionBarcode = new List<ProductConversionBarcodeViewModel>();
                        //resultProductConversionBarcode = utils.SendDataApi<List<ProductConversionBarcodeViewModel>>(new AppSettingConfig().GetUrl("dropdownProductBarcode"), ProductConversionBarcodeModel.sJson());
                        //if (resultProductConversionBarcode.Count > 0 && resultProductConversionBarcode != null)
                        //{
                        //    var DataProductConversionBarcode = resultProductConversionBarcode.Find(c => c.productConversion_Index == item.productConversion_Index);
                        //    if (DataProductConversionBarcode != null)
                        //    {
                        //        ProductBarcode = DataProductConversionBarcode.productConversionBarcode;
                        //    }

                        //}
                    }
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.tag_No.Replace("I", ""), QRCodeGenerator.ECCLevel.Q); //queryGr.PalletID
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);


                    resultItem.warehouse_Id = "R_" + queryGr.Warehouse_Id;
                    resultItem.location_Id = queryGr.Location_Name;
                    resultItem.goodsReceive_Date = GRDate;
                    resultItem.planGoodsReceive_No = queryGr.Ref_Document_No;
                    resultItem.goodsReceive_No = queryGr.GoodsReceive_No;
                    resultItem.suggest_Location_Name = queryGr.Location_Name;
                    resultItem.owner_Name = queryGr.Owner_Name;
                    resultItem.product_Id = item.product_Id;
                    resultItem.product_Name = item.product_Name;
                    resultItem.qty = Convert.ToDecimal(item.qty);
                    resultItem.tag_NoBarcode = Convert.ToBase64String(BitmapToBytes(qrCodeImage));
                    //resultItem.tag_NoBarcode = new NetBarcode.Barcode(item.tag_No, NetBarcode.Type.Code128B).GetBase64Image();
                    resultItem.productConversion_Name = item.productConversion_Name;
                    resultItem.tag_No = item.tag_No;
                    resultItem.ref_no1 = RefNo1;
                    resultItem.ref_no2 = RefNo2;
                    resultItem.ref_no3 = RefNo3;
                    resultItem.date_Print = DatePrint + " " + time;
                    resultItem.shortNameUnit = ShortNameUnit;

                    //resultItem.receiverDate = GRDate;
                    //resultItem.supplier = queryGr.Supplier;


                    resultItem.productionLineNo = queryGr.ProductionLineNo;
                    resultItem.palletID = item.tag_No.Replace("I", ""); //queryGr.PalletID;
                    resultItem.sku = queryGr.SKU;
                    resultItem.skuBarcode = queryGr.SKUBarcode;
                    resultItem.isLastCarton = queryGr.IsLastCarton;
                    resultItem.description = queryGr.Description;
                    resultItem.mainType = queryGr.MainType;
                    //resultItem.quantityInCRT = queryGr.QuantityInCRT;
                    //resultItem.quantityInPC = queryGr.QuantityInPC;
                    resultItem.mfgDate = (queryGr.MFGDate != null) ? queryGr.MFGDate : " - ";
                    resultItem.expDate = (queryGr.EXPDate != null) ? queryGr.EXPDate : " - ";
                    resultItem.lotNo = (!string.IsNullOrEmpty(queryGr.LotNo)) ? queryGr.LotNo : " - ";
                    resultItem.refDoc = queryGr.RefDoc;
                    resultItem.cartonPerPallet = queryGr.CartonPerPallet;
                    resultItem.ti = queryGr.Ti;
                    resultItem.hi = queryGr.Hi;
                    resultItem.valTiHi = queryGr.Ti * queryGr.Hi;
                    resultItem.receiverDate = GRDate;
                    resultItem.receiver = queryGr.Receiver;
                    resultItem.status = queryGr.Status;

                    string sapCreateDT = DateTime.ParseExact(queryGr.SapCreateDT.toString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                    resultItem.sapCreateDT = sapCreateDT;

                    string productionEndDT = DateTime.ParseExact(queryGr.ProductionEndDT.toString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                    resultItem.productionEndDT = productionEndDT;

                    resultItem.saleQty = queryGr.SaleQty;
                    resultItem.saleUnit = queryGr.SaleUnit;

                    string giBeforeDate = DateTime.ParseExact(queryGr.GiBeforeDate.toString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                    resultItem.giBeforeDate = giBeforeDate;

                    resultItem.batchNo = (!string.IsNullOrEmpty(queryGr.BatchNo)) ? queryGr.BatchNo : " - ";
                    resultItem.type = queryGr.Type;
                    resultItem.unitOnPallet = queryGr.UnitOnPallet;
                    resultItem.palletWT = queryGr.PalletWT;
                    resultItem.bu = queryGr.BU;
                    resultItem.supplier = queryGr.Supplier;
                    resultItem.remark = queryGr.Remark;
                    resultItem.qtyInUnit = queryGr.QtyInUnit;
                    resultItem.unitOfInUnit = queryGr.UnitOfInUnit;
                    resultItem.qtyPOUnit = queryGr.QtyPOUnit;
                    resultItem.unitOfPOUnit = queryGr.UnitOfPOUnit;

                    resTagItem.Add(resultItem);

                }
                resTagItem.ToList();



                rootPath = rootPath.Replace("\\TransferAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\ReportTagPutaway\\ReportTagPutaway.rdlc";
                //var reportPath = rootPath + "\\Reports\\ReportTagPutaway\\ReportTagPutaway.rdlc";
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportTagPutaway");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region BitmapToBytes
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        #endregion


        #region FilterGoodsTransferUnPack
        public List<listTaskViewModel> FilterGoodsTransferUnPack(ScanRefTransferNoViewModel model)
        {
            try
            {
                List<Guid> Tranfertype = new List<Guid>
                {
                Guid.Parse("51E6E650-7FBA-49ED-ADCB-AA4201CFBF1A"),
                Guid.Parse("5131C416-3AA8-458D-9342-69CA281FBAFD")
                };

                var items = new List<listTaskViewModel>();
                items = (from gt in db.IM_GoodsTransfer.Where(c => Tranfertype.Contains(c.DocumentType_Index)).AsQueryable()
                         join tgti in db.im_TaskTransferItem.Where(c => c.Is_unpack == null).AsQueryable() on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                         join tgt in db.im_TaskTransfer.AsQueryable() on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                         where gt.Document_Status == 3
                         group gt by new
                         {
                             gt.GoodsTransfer_Index,
                             gt.GoodsTransfer_No,
                             gt.GoodsTransfer_Date,
                             tgti.Binbalance_index,
                             tgti.TaskTransferItem_Index,
                             tgti.ProductConversion_Index,
                             tgti.ProductConversion_Id,
                             tgti.ProductConversion_Name,
                             tgti.Qty
                         } into g
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

        #region FilterGoodsTransferUnPack
        public List<listTaskViewModel> FilterGoodsTransferPack(ScanRefTransferNoViewModel model)
        {
            try
            {
                List<Guid> Tranfertype = new List<Guid>
                {
                Guid.Parse("8384B34F-CB32-4EC3-BB56-B29BC9C4994F"),
                Guid.Parse("1BE96CE0-BCF9-4B53-9E08-23FD5DA83EFB")
                };

                var items = new List<listTaskViewModel>();
                items = (from gt in db.IM_GoodsTransfer.Where(c => Tranfertype.Contains(c.DocumentType_Index)).AsQueryable()
                         join tgti in db.im_TaskTransferItem.Where(c => c.Is_unpack == null).AsQueryable() on gt.GoodsTransfer_Index equals tgti.Ref_Document_Index
                         //join tgt in db.im_TaskTransfer.AsQueryable() on tgti.TaskTransfer_Index equals tgt.TaskTransfer_Index
                         where gt.Document_Status == 3
                         group gt by new
                         {
                             gt.GoodsTransfer_Index,
                             gt.GoodsTransfer_No,
                             gt.GoodsTransfer_Date,
                             //tgti.Binbalance_index,
                             //tgti.TaskTransferItem_Index,
                             //tgti.ProductConversion_Index,
                             //tgti.ProductConversion_Id,
                             //tgti.ProductConversion_Name,
                             //tgti.Qty
                         } into g
                         select new listTaskViewModel
                         {
                             goodsTransfer_Index = g.Key.GoodsTransfer_Index,
                             goodsTransfer_No = g.Key.GoodsTransfer_No,
                             goodsTransfer_Date = g.Key.GoodsTransfer_Date.toString(),
                             //binbalance_index = g.Key.Binbalance_index,
                             //taskTransfer_Index = g.Key.TaskTransferItem_Index,
                             //productConversion_Index = g.Key.ProductConversion_Index.ToString(),
                             //productConversion_Id = g.Key.ProductConversion_Id,
                             //productConversion_Name = g.Key.ProductConversion_Name,
                             //qty = g.Key.Qty

                         }).ToList();

                foreach (var item in items)
                {
                    var taskTransferItems = db.im_TaskTransferItem.Where(c => c.Ref_Document_Index == item.goodsTransfer_Index && c.Is_unpack == null).ToList();

                    foreach (var item_taskitem in taskTransferItems)
                    {
                        var result_item = new listTaskViewModel();
                        result_item.binbalance_index = item_taskitem.Binbalance_index;
                        item.items.Add(result_item);
                    }
                }

                //var Goodstranfer = db.IM_GoodsTransfer.Where (c => Tranfertype.Contains(c.DocumentType_Index)

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ScanLocation
        public TranferUnpackViewmodel ScanLocation(TransferViewModel data)
        {
            TranferUnpackViewmodel result = new TranferUnpackViewmodel();
            try
            {
                View_CheckLocation_unpack checklocation = dbMaster.View_CheckLocation_unpack.FirstOrDefault(c => c.Location_Name == data.LocationNew);
                if (checklocation == null)
                {
                    result.resultIsUse =  false;
                    result.resultMsg =  "location ที่ scan ถูกจอง หรือ ไม่ว่าง กรุณาลองใหม่";
                    return result;
                }
                else {
                    result.location_Index = checklocation.Location_Index;
                    result.location_Id = checklocation.Location_Id;
                    result.location_Name = checklocation.Location_Name;
                    result.resultIsUse = true;
                    return result;
                }


            }
            catch (Exception ex)
            {
                result.resultIsUse = false;
                result.resultMsg = "กรุณาติดต่อ Admin MSG ex: " + ex.Message;
                return result;
            }
        }
        #endregion

    }

}
