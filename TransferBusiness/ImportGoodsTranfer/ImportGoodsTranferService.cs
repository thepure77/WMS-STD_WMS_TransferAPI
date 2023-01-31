using AspNetCore.Reporting;
using BinBalanceDataAccess.Models;
using Business.Models;
using Comone.Utils;
using DataAccess;
using GRBusiness.GoodsReceive;
using InterfaceWMSBusiness.TranferExtensions;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using planGIBusiness.AutoNumber;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using TransferBusiness.ConfigModel;
using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.Library;
using TransferDataAccess.Models;



namespace TransferBusiness.Transfer
{
    public class ImportGoodsTranferService
    {
        #region ImportGoodsTranferService
        private TransferDbContext dbTransfer;

        private BinbalanceDbContext dbBinbalance;

        private InboundDbContext dbInbound;

        private MasterConnectionDbContext dbMaster;


        public ImportGoodsTranferService()
        {
            dbTransfer = new TransferDbContext();
            dbBinbalance = new BinbalanceDbContext();
            dbInbound = new InboundDbContext();
            dbMaster = new MasterConnectionDbContext();
        }

        public ImportGoodsTranferService(TransferDbContext dbTransfer, BinbalanceDbContext dbBinbalance, InboundDbContext dbInbound, MasterConnectionDbContext dbMaster)
        {
            this.dbTransfer = dbTransfer;
            this.dbBinbalance = dbBinbalance;
            this.dbInbound = dbInbound;
            this.dbMaster = dbMaster;

        }
        #endregion

        #region Validate
        public Validated_ImportModel validateImportTranfer(string jsonData)
        {
            var import_userindex = Guid.NewGuid();
            var import_Validate = Guid.Parse("162E87DE-769A-4437-85E0-54979C0D030A");
            try
            {
                
                dbTransfer.Database.SetCommandTimeout(360);
                dbBinbalance.Database.SetCommandTimeout(360);
                dbInbound.Database.SetCommandTimeout(360);
                dbMaster.Database.SetCommandTimeout(360);

                Import_Model import_model = ImportExtensions.GetImportModel(jsonData);
                Guid ImportIndex = Guid.NewGuid();
                DateTime ImportDateTime = DateTime.Now;
                Validated_ImportModel result_model = new Validated_ImportModel() { Import_GuID = ImportIndex };
                List<Invalid_ImportModel> invalid_model = new List<Invalid_ImportModel>();
                List<Valid_ImportModel> valid_model = new List<Valid_ImportModel>();

                var checkdupstep = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate);
                if (checkdupstep != null)
                {
                    if (checkdupstep.Import_Status != 0)
                    {
                        result_model.resultIsUse = false;
                        result_model.resultMsg = "ไม่สามารถ Import ได้ เนื่องจากมี User : " + checkdupstep.Import_By + " กำลัง Import อยู่";

                        return result_model;
                    }
                    else {

                        checkdupstep.Import_File_Name = import_model.Import_FileName;
                        checkdupstep.Import_By = import_model.Import_FileName;
                        checkdupstep.Import_Status = 1;
                        checkdupstep.import_userindex = import_userindex;

                        var transaction = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            dbTransfer.SaveChanges();
                            transaction.Commit();

                        }
                        catch (Exception exy)
                        {
                            transaction.Rollback();
                            result_model.resultIsUse = false;
                            result_model.resultMsg = "ไม่สามารถทำการ Import ได้ กรุณาติดต่อ Admin : "+exy.Message;
                            return result_model;
                        }

                        Thread.Sleep(2000);

                        _Prepare_Imports_step checkdupstep_user = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate && c.import_userindex == import_userindex);
                        if (checkdupstep_user == null)
                        {
                            result_model.resultIsUse = false;
                            result_model.resultMsg = "พบว่ามีการกด import ซ้ำ หรือ พร้อมกัน กรุณารอหรือลองอีกครั้งในภายหลัง";
                            return result_model;
                        }
                    }

                }
                else {
                    result_model.resultIsUse = false;
                    result_model.resultMsg = "กรุณาติดต่อ Admin";
                    return result_model;
                }

                if (import_model.Data.Count > 200)
                {
                    result_model.resultIsUse = false;
                    result_model.resultMsg = "ไม่สามารถ Import ได้ เนื่องจาก : "+ import_model.Import_FileName +" มีข้อมูลเกิน 200 Rows";
                    userindex(import_userindex, import_Validate);
                    return result_model;
                }

                List<_Prepare_Imports> filename = dbTransfer._Prepare_Imports.Where(c => c.Import_File_Name.Trim() == import_model.Import_FileName.Trim()).ToList();
                if (filename.Count > 0)
                {
                    result_model.resultIsUse = false;
                    result_model.resultMsg = "ไม่สามารถ Import ได้ เนื่องจาก : " + import_model.Import_FileName + " มีการ import แล้ว";
                    userindex(import_userindex, import_Validate);
                    return result_model;
                }

                _Prepare_Imports_step checkstep_again = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate && c.import_userindex == import_userindex);
                if (checkstep_again == null)
                {
                    result_model.resultIsUse = false;
                    result_model.resultMsg = "กรุณารอหรือลองอีกครั้งในภายหลัง";
                    userindex(import_userindex, import_Validate);
                    return result_model;
                }

                import_model.Data.ForEach(e =>
                {
                    e.Import_Index = ImportIndex;
                    e.Import_Date = ImportDateTime;
                    e.Import_By = import_model.Import_By;
                    e.Import_Type = "Import Tranfer";
                    e.Import_File_Name = import_model.Import_FileName;
                    e.Import_Status = 1;
                    e.Import_Message = "Validated";
                    e.Import_Case = "Import Tranfer";
                    valid_model.Add(new Valid_ImportModel() { Model_Index = Guid.NewGuid(), Data = e });
                });

                _Prepare_Imports columnModel = new _Prepare_Imports();
                Import_Model.ColumnDetailModel column;


                // Validate pallet.
                column = import_model.FindColumn(columnModel.pallet(true));
                invalid_model.AddError(ref valid_model, w => w.Data.pallet().IsNull(), column, "Pallet not found");

                List<View_locationtype_import_tranfer> locationtype_Import_Tranfers = dbMaster.View_locationtype_import_tranfer.ToList();

                if (locationtype_Import_Tranfers.Count == 0 )
                {
                    result_model.resultIsUse = false;
                    result_model.resultMsg = "ไม่พบ Location ที่สามารถทำการจองได้ กรุณาติดต่อ Admin เพื่อเพิ่ม location ในการ Import";
                    userindex(import_userindex, import_Validate);
                    return result_model;
                }

                List<string> ListPallet = valid_model.Where(w => w.Data.pallet().IsNotNull()).GroupBy(g => g.Data.pallet().TrimWithNull()).Select(s => s.Key).ToList();

                List<string> dup_pallet = valid_model.GroupBy(x => x.Data.pallet().TrimWithNull()).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

                if (ListPallet.Count > 0)
                {
                    List<wm_BinBalance> findpallet = dbBinbalance.wm_BinBalance.Where(c => ListPallet.Contains(c.Tag_No)).ToList();
                    List<string> findpallet_no = findpallet.Select(c => c.Tag_No).ToList();

                    List<string> pallet_notfound = ListPallet.Except(findpallet_no).ToList();
                    if (findpallet.Count > 0)
                    {

                        foreach (Valid_ImportModel item in valid_model)
                        {
                            wm_BinBalance dataResult = findpallet.FirstOrDefault(c => c.Tag_No == item.Data.C0);
                            
                            if (dataResult != null)
                            {
                                MS_ProductConversion productConversion = dbMaster.ms_ProductConversion.FirstOrDefault(c => c.IsActive == 1 && c.IsDelete == 0 && c.Product_Index == dataResult.Product_Index && c.SALE_UNIT == 1);
                                item.Data.C2 = dataResult.Location_Name;
                                item.Data.C3 = dataResult.Product_Id;
                                item.Data.C4 = dataResult.Product_Name;
                                item.Data.C5 = productConversion == null ? dataResult.BinBalance_QtyBal.ToString() : Math.Round((dataResult.BinBalance_QtyBal / productConversion.ProductConversion_Ratio).Value,6).ToString();
                                item.Data.C6 = productConversion == null ?  dataResult.ProductConversion_Name : productConversion.ProductConversion_Name;
                                item.Data.C7 = dataResult.Product_Lot;
                                item.Data.C8 = dataResult.ERP_Location;
                                item.Data.C9 = dataResult.ItemStatus_Name;
                                item.Data.C10 = dataResult.GoodsReceive_EXP_Date.ToString();
                                item.Data.C11 = dataResult.GoodsReceive_MFG_Date.ToString();
                                item.Data.C12 = dataResult.GoodsReceive_No;
                                
                            }
                        }


                        List<Guid> locationtype_Import = locationtype_Import_Tranfers.GroupBy(c => c.Location_Index).Select(c => c.Key).ToList();
                        List<wm_BinBalance> findpallet_location = findpallet.Where(c => !locationtype_Import.Contains(c.Location_Index)).ToList();

                        if (dup_pallet.Count > 0)
                        {
                            dup_pallet.ForEach(e =>
                            { invalid_model.AddError(ref valid_model, w => w.Data.pallet().TrimWithNull() == e, column, "Duplicates pallet"); });
                        }

                        if (findpallet.Count > 0)
                        {
                            pallet_notfound.ForEach(e =>
                            { invalid_model.AddError(ref valid_model, w => w.Data.pallet().TrimWithNull() == e, column, "pallet not found"); });
                        }

                        if (findpallet_location.Count > 0)
                        {
                            findpallet_location.ForEach(e =>
                            { invalid_model.AddError(ref valid_model, w => w.Data.pallet().TrimWithNull() == e.Tag_No, column, "location Pallet isn't in type"); });
                        }

                        var findpallet_qty = findpallet.Where(c => c.BinBalance_QtyReserve != 0 && c.BinBalance_QtyBal > 0).ToList();
                        if (findpallet_qty.Count > 0)
                        {
                            findpallet_qty.ForEach(e =>
                            { invalid_model.AddError(ref valid_model, w => w.Data.pallet().TrimWithNull() == e.Tag_No, column, "Pallet has Reserve"); });
                        }

                    }
                    else {

                        result_model.resultIsUse = false;
                        result_model.resultMsg = "ไม่พบ tag ทั้งหมดที่ต้องการ Import";
                        userindex(import_userindex, import_Validate);
                        return result_model;
                    }

                    
                }
                
                //Valid Model
                valid_model.Where(w => w.Data.Import_Status != -1).ToList().ForEach(e =>
                { result_model.Valid_Data.Add(e.Data.ReturnModel(null)); });
                
                //Invalid Model
                valid_model.Where(w => w.Data.Import_Status == -1).ToList().ForEach(e =>
                { result_model.Invalid_Data.Add(e.Data.ReturnModel(invalid_model.FirstOrDefault(w => w.Model_Index == e.Model_Index)?.Errors)); });

                List<_Prepare_Imports> insert_data = new List<_Prepare_Imports>();
                insert_data.AddRange(valid_model.Select(s => s.Data).ToArray());

                _Prepare_Imports_step status_validate = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate && c.import_userindex == import_userindex);
                if (status_validate != null)
                {
                    checkdupstep.Import_Status = 0;
                    checkdupstep.Import_By = null;
                    checkdupstep.import_userindex = null;
                    checkdupstep.Import_File_Name = null;
                }
                dbTransfer._Prepare_Imports.AddRange(insert_data);
                dbTransfer.SaveChanges();

                if (result_model.Invalid_Data.Count > 0)
                {
                    result_model.resultIsUse = false;
                    result_model.resultMsg = "ไม่สามารถทำการ import ได้";
                }
                else {
                    result_model.resultIsUse = true;
                }

                return result_model;
            }
            catch (Exception ex)
            {
                #region Update status _Prepare_Imports_step
                _Prepare_Imports_step checkdupstep = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate && c.import_userindex == import_userindex);

                if (checkdupstep != null)
                {
                    checkdupstep.Import_Status = 0;
                    checkdupstep.Import_By = null;
                    checkdupstep.import_userindex = null;
                    checkdupstep.Import_File_Name = null;

                    var transaction = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        dbTransfer.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception exy)
                    {
                        transaction.Rollback();
                        throw exy;
                    }
                }
                #endregion

                Validated_ImportModel result_model_ex = new Validated_ImportModel();
                result_model_ex.resultIsUse = false;
                result_model_ex.resultMsg = ex.Message;
                return result_model_ex;
            }
        }
        #endregion

        #region Confirm
        public Result ConfirmImport(string jsonData)
        {
            logtxt log = new logtxt();
            log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now);
            Result result = new Result();
            string Tranfer_No = "";
            Guid import_userindex = Guid.NewGuid();
            bool is_success = true;
            Guid import_Validate = Guid.Parse("A27FBBDD-9973-443A-B305-7A2F15D694C5");
            try
            {
                ConfirmImport_Model model = ImportExtensions.GetConfirmImportModel(jsonData);
                _Prepare_Imports_step checkdupstep = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate);
                if (checkdupstep != null)
                {
                    if (checkdupstep.Import_Status != 0)
                    {
                        result.resultIsUse = false;
                        result.resultMsg = "ไม่สามารถ Confirm ได้ เนื่องจากมี User : " + checkdupstep.Import_By + " กำลัง Import อยู่";

                        return result;
                    }
                    else
                    {

                        checkdupstep.Import_By = model.Confirm_By;
                        checkdupstep.Import_Status = 1;
                        checkdupstep.import_userindex = import_userindex;

                        var transactionTransfer = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            dbTransfer.SaveChanges();
                            transactionTransfer.Commit();

                        }
                        catch (Exception exy)
                        {
                            transactionTransfer.Rollback();
                            result.resultIsUse = false;
                            result.resultMsg = "ไม่สามารถทำการ Confirm ได้ กรุณาติดต่อ Admin : " + exy.Message;
                            return result;
                        }

                        Thread.Sleep(2000);

                        _Prepare_Imports_step checkdupstep_user = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate && c.import_userindex == import_userindex);
                        if (checkdupstep_user == null)
                        {
                            result.resultIsUse = false;
                            result.resultMsg = "พบว่ามีการกด Confirm ซ้ำ หรือ พร้อมกัน กรุณารอหรือลองอีกครั้งในภายหลัง";
                            return result;
                        }
                    }
                    
                }
                else
                {
                    result.resultIsUse = false;
                    result.resultMsg = "กรุณาติดต่อ Admin";
                    return result;
                }

                
                List<_Prepare_Imports> data = dbTransfer._Prepare_Imports.Where(w => w.Import_Index.Equals(model.Import_Index.Value)).ToList();
                if (data.Count <= 0) {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่พบข้อมูลที่ใช้ในการ Confirm";
                    userindex(import_userindex, import_Validate);
                    return result;
                }
                
                List<_Prepare_Imports> valid_data = data.Where(w => w.Import_Status != 1).ToList();
                if (valid_data.Count > 0)
                {
                    result.resultIsUse = false;
                    result.resultMsg = "ไม่สามารถ Confirm ได้เนื่องจาก มีข้อมูลที่ไม่สามารถ Confirm ได้";
                    userindex(import_userindex, import_Validate);
                    return result;
                }

                Guid tranfer_index = Guid.NewGuid();
                List<string> pallet = data.GroupBy(c => c.C0).Select(c => c.Key).ToList();
                List<wm_BinBalance> findpallet = dbBinbalance.wm_BinBalance.Where(c => pallet.Contains(c.Tag_No) && c.BinBalance_QtyReserve == 0).ToList();
                if (pallet.Count != findpallet.Count)
                {
                    List<string> pallet_isUse = findpallet.GroupBy(c => c.Tag_No).Select(c => c.Key).ToList();
                    List<string> pallet_notfound = pallet.Except(pallet_isUse).ToList();

                    result.resultIsUse = false;
                    result.resultMsg = "กรุณาเช็ค Pallet และทำการ Import ใหม่";
                    userindex(import_userindex, import_Validate);
                    return result;
                }
                else {
                    log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "1 : " + DateTime.Now);
                    findpallet.ForEach(e => { e.IsUse = tranfer_index.ToString(); });
                    dbBinbalance.SaveChanges();

                    log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "2 : " + DateTime.Now);
                    Result save_H = CreateGoodsTransferHeader(tranfer_index, model.Confirm_By);
                    log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "3 : " + DateTime.Now);
                    Tranfer_No = save_H.No;
                    if (!save_H.resultIsUse)
                    {
                        log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "3... : " + DateTime.Now);
                        result.resultIsUse = false;
                        result.resultMsg = save_H.resultMsg;
                        userindex(import_userindex, import_Validate);
                        return result;
                    }
                    else
                    {
                        log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "4 : " + DateTime.Now);
                        foreach (wm_BinBalance item in findpallet)
                        {
                            PickbinbalanceViewModel model_pick = new PickbinbalanceViewModel();
                            MS_ProductConversion productConversion = dbMaster.ms_ProductConversion.FirstOrDefault(c => c.IsActive == 1 && c.IsDelete == 0 && c.Product_Index == item.Product_Index && c.SALE_UNIT == 1);
                            
                            model_pick.binbalance_Index = item.BinBalance_Index.ToString();
                            model_pick.goodsReceive_Index = item.GoodsReceive_Index.ToString();
                            model_pick.goodsReceive_No = item.GoodsReceive_No;
                            model_pick.tag_Index = item.Tag_Index.ToString();
                            model_pick.tag_No = item.Tag_No;
                            model_pick.product_Lot = item.Product_Lot;
                            model_pick.documentType_Index = "F5618093-38A1-46BF-8D84-20D9AD9F46F8";
                            model_pick.documentType_Id = "TFIMP";
                            model_pick.documentType_Name = "Import Tranfer";
                            model_pick.product_Index = item.Product_Index.ToString();
                            model_pick.product_Id = item.Product_Id;
                            model_pick.product_Name = item.Product_Name;
                            model_pick.qty = item.BinBalance_QtyBal ;
                            model_pick.weight = item.BinBalance_QtyBal ;
                            model_pick.productConversion_Index = item.ProductConversion_Index.ToString() ;
                            model_pick.productConversion_Id = item.ProductConversion_Id ;
                            model_pick.productConversion_Name = item.ProductConversion_Name ;
                            model_pick.productConversion_Ratio = item.BinBalance_Ratio ;
                            model_pick.status_Index = item.ItemStatus_Index.ToString() ;
                            model_pick.status_Id = item.ItemStatus_Id ;
                            model_pick.status_Name = item.ItemStatus_Name ;
                            model_pick.location_Index = item.Location_Index.ToString() ;
                            model_pick.location_Id = item.Location_Id ;
                            model_pick.location_Name = item.Location_Name ;
                            model_pick.create_By = model.Confirm_By;
                            model_pick.goodsReceive_Date = item.GoodsReceive_Date.ToString() ;
                            model_pick.goodsReceive_MFG_Date = item.GoodsReceive_MFG_Date.ToString() ;
                            model_pick.goodsReceive_EXP_Date = item.GoodsReceive_EXP_Date.ToString() ;
                            model_pick.warehouse_Index = null ;
                            model_pick.warehouse_Id = null;
                            model_pick.warehouse_Name = null;
                            model_pick.zone_Index = null;
                            model_pick.zone_Id = null;
                            model_pick.zone_Name = null;

                            if (productConversion == null)
                            {
                                MS_ProductConversion productConversion_base = dbMaster.ms_ProductConversion.FirstOrDefault(c => c.IsActive == 1 && c.IsDelete == 0 && c.Product_Index == item.Product_Index && c.ProductConversion_Index == item.ProductConversion_Index);
                                model_pick.unit.productConversion_Index = item.ProductConversion_Index.GetValueOrDefault();
                                model_pick.unit.product_Index = item.Product_Index;
                                model_pick.unit.productConversion_Id = productConversion_base.ProductConversion_Id;
                                model_pick.unit.productConversion_Name = productConversion_base.ProductConversion_Name;
                                model_pick.unit.productconversion_Ratio = productConversion_base.ProductConversion_Ratio;
                                model_pick.unit.productconversion_Weight = productConversion_base.ProductConversion_Weight;
                                model_pick.unit.productconversion_Width = productConversion_base.ProductConversion_Width;
                                model_pick.unit.productconversion_Length = productConversion_base.ProductConversion_Length;
                                model_pick.unit.productconversion_Height = productConversion_base.ProductConversion_Height;
                                model_pick.unit.productconversion_VolumeRatio = productConversion_base.ProductConversion_VolumeRatio;
                                model_pick.unit.isActive = 0;
                                model_pick.unit.isDelete = 0;
                                model_pick.unit.issystem = 0;
                                model_pick.unit.status_id = 0;
                                model_pick.unit.create_By = null;
                                model_pick.unit.create_Date = null;
                                model_pick.unit.update_By = null;
                                model_pick.unit.update_Date = null;
                                model_pick.unit.cancel_By = null;
                                model_pick.unit.cancel_Date = null;

                                model_pick.pick = item.BinBalance_QtyBal;
                            }
                            else {
                                ProductConversionViewModelDoc unit = new ProductConversionViewModelDoc();
                                unit.productConversion_Index = productConversion.ProductConversion_Index;
                                unit.product_Index = productConversion.Product_Index;
                                unit.productConversion_Id = productConversion.ProductConversion_Id;
                                unit.productConversion_Name = productConversion.ProductConversion_Name;
                                unit.productconversion_Ratio = productConversion.ProductConversion_Ratio;
                                unit.productconversion_Weight = productConversion.ProductConversion_Weight;
                                unit.productconversion_Width = productConversion.ProductConversion_Width;
                                unit.productconversion_Length = productConversion.ProductConversion_Length;
                                unit.productconversion_Height = productConversion.ProductConversion_Height;
                                unit.productconversion_VolumeRatio = productConversion.ProductConversion_VolumeRatio;
                                unit.isActive = productConversion.IsActive;
                                unit.isDelete = productConversion.IsDelete;
                                unit.issystem = productConversion.IsSystem;
                                unit.status_id = productConversion.Status_Id;
                                unit.create_By = productConversion.Create_By;
                                unit.create_Date = productConversion.Create_Date;
                                unit.update_By = productConversion.Update_By;
                                unit.update_Date = productConversion.Update_Date;
                                unit.cancel_By = productConversion.Cancel_By;
                                unit.cancel_Date = productConversion.Cancel_Date;

                                model_pick.unit = unit;

                                model_pick.pick = item.BinBalance_QtyBal / productConversion.ProductConversion_Ratio;
                            }
                            
                            
                            model_pick.goodsTransfer_Index = tranfer_index.ToString();
                            model_pick.goodsTransfer_No = save_H.No;
                            model_pick.owner_Index = "02b31868-9d3d-448e-b023-05c121a424f4";
                            model_pick.owner_Id = "3419";
                            model_pick.owner_Name = "Amazon";

                            var isuse = pickProduct(model_pick);
                            if (!isuse.resultIsUse)
                            {
                                is_success = false;
                                break;
                            }
                        }
                        log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "5 : " + DateTime.Now);

                        if (!is_success)
                        {
                            GoodsTransferService transferService = new GoodsTransferService();
                            List<IM_GoodsTransferItem> goodsTransferItem = dbTransfer.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == tranfer_index && c.Document_Status == 0).ToList();
                            foreach (var item in goodsTransferItem)
                            {
                                PickbinbalanceViewModel delete_pick = new PickbinbalanceViewModel();
                                var result_cancle = transferService.deletePickProduct(delete_pick);
                                if (!result_cancle)
                                {
                                    result.resultIsUse = false;
                                    result.resultMsg = "กรุณาติดต่อ Admin เนื่องจากทำรายการไม่สำเร็จ";
                                    userindex(import_userindex, import_Validate);
                                    return result;
                                }
                            }

                            result.resultIsUse = false;
                            result.resultMsg = "กรุณาติดต่อ Admin หรือลองใหม่ในภายหลัง";
                            userindex(import_userindex, import_Validate);
                            return result;

                        }

                    }
                }
                log.DataLogLines("ConfirmImport", "ConfirmImport" + DateTime.Now.ToString("yyyy-MM-dd"), "6 : " + DateTime.Now);


                data.ForEach(e => { e.Import_Message = "Imported"; e.Import_Status = 2; });
                dbTransfer.SaveChanges();
                findpallet.ForEach(e => { e.IsUse = ""; });
                dbBinbalance.SaveChanges();

                result.resultIsUse = false;
                result.resultMsg = "Import Tranfer No : " + Tranfer_No;
                userindex(import_userindex, import_Validate);
                return result;
            }
            catch (Exception ex)
            {
                #region Update status _Prepare_Imports_step
                _Prepare_Imports_step checkdupstep = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == import_Validate && c.import_userindex == import_userindex);

                if (checkdupstep != null)
                {
                    checkdupstep.Import_Status = 0;
                    checkdupstep.Import_By = null;
                    checkdupstep.import_userindex = null;
                    checkdupstep.Import_File_Name = null;

                    var transaction = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        dbTransfer.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception exy)
                    {
                        transaction.Rollback();
                        throw exy;
                    }
                }
                #endregion

                Validated_ImportModel result_model_ex = new Validated_ImportModel();
                result_model_ex.resultIsUse = false;
                result_model_ex.resultMsg = ex.Message;
                return result_model_ex;
            }
        }
        #endregion

        #region userindex
        public Result userindex(Guid index,Guid type)
        {
            Result result = new Result();
            try
            {

                dbTransfer.Database.SetCommandTimeout(360);

                var checkdupstep = dbTransfer._Prepare_Imports_step.FirstOrDefault(c => c.Import_Index == type && c.import_userindex == index);

                if (checkdupstep != null)
                {
                    checkdupstep.Import_Status = 0;
                    checkdupstep.Import_By = null;
                    checkdupstep.import_userindex = null;
                    checkdupstep.Import_File_Name = null;

                    var transaction = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        dbTransfer.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception exy)
                    {
                        transaction.Rollback();
                        throw exy;
                    }
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
        public Result CreateGoodsTransferHeader(Guid index,string Confirm_By)
        {
            Result result_data = new Result();
            logtxt log = new logtxt();
            log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "Start : " + DateTime.Now);
            string GoodsTransferNo = "";
            try
            {
                var filterModel = new DocumentTypeViewModel();
                var result = new List<GenDocumentTypeViewModel>();
                log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "1 : " + DateTime.Now);
                filterModel.process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");
                filterModel.documentType_Index = Guid.Parse("F5618093-38A1-46BF-8D84-20D9AD9F46F8");
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
                itemHeader.DocumentType_Index = Guid.Parse("F5618093-38A1-46BF-8D84-20D9AD9F46F8");
                itemHeader.DocumentType_Id = "TFIMP";
                itemHeader.DocumentType_Name = "Import Tranfer";
                itemHeader.DocumentRef_No2 = null;
                itemHeader.DocumentRef_No3 = null;
                itemHeader.DocumentRef_No4 = null;
                itemHeader.DocumentRef_Remark = null;
                itemHeader.Document_Status = -2;
                itemHeader.Create_By = Confirm_By;
                itemHeader.Create_Date = DateTime.Now;
                dbTransfer.IM_GoodsTransfer.Add(itemHeader);

                log.DataLogLines("CreateGoodsTransferHeader", "CreateGoodsTransferHeader" + DateTime.Now.ToString("yyyy-MM-dd"), "4 : " + DateTime.Now);
                var transaction = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    dbTransfer.SaveChanges();
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

        #region pickProduct
        public Result pickProduct(PickbinbalanceViewModel model)
        {
            try
            {
                var result = new Result();
                var dataBinbalance = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), model.sJson());
                if (!dataBinbalance.resultIsUse)
                {
                    result.resultMsg = "";
                    result.resultIsUse = false;
                    return result;
                }
                else
                {
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
                    GoodsTransferItem.Location_Index_To = Guid.Parse("0d25d307-f2e0-4495-b1d6-9b91a52ee5bd");
                    GoodsTransferItem.Location_Id_To = "EXPORT99";
                    GoodsTransferItem.Location_Name_To = "EXPORT99";
                    GoodsTransferItem.Qty = (Decimal)model.pick;
                    GoodsTransferItem.Ratio = (Decimal)model.productConversion_Ratio;
                    GoodsTransferItem.TotalQty = (Decimal)model.pick * (Decimal)model.productConversion_Ratio;
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
                    

                    GoodsTransferItem.Document_Status = -3;


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
                        GoodsTransferItem.UDF_2 = GoodsReceiveItem?.FirstOrDefault().ref_Document_No;

                        //getPGRI
                        var listPGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = GoodsReceiveItem?.FirstOrDefault().ref_DocumentItem_Index } };
                        var PGRItem = new DocumentViewModel();
                        PGRItem.listDocumentViewModel = listPGRItem;
                        var PlanGoodsReceiveItem = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGoodsReceiveItem"), PGRItem.sJson());
                        GoodsTransferItem.UDF_3 = PlanGoodsReceiveItem?.FirstOrDefault().documentRef_No2;
                    }


                    GoodsTransferItem.Create_By = model.create_By;
                    GoodsTransferItem.Create_Date = DateTime.Now;

                    dbTransfer.IM_GoodsTransferItem.Add(GoodsTransferItem);

                    var transaction = dbTransfer.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        dbTransfer.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exy)
                    {
                        transaction.Rollback();
                        throw exy;
                    }

                    model.goodsTransfer_Index = GoodsTransferItem.GoodsTransfer_Index.ToString();
                    model.goodsTransferItem_Index = GoodsTransferItem.GoodsTransferItem_Index.ToString();
                    model.goodsTransfer_No = model.goodsTransfer_No;
                    model.process_Index = "CE757517-EBBC-4BEA-93CC-F7E139AE422C";
                    model.goodsReceive_Index = dataBinbalance.goodsReceive_Index.ToString();
                    model.goodsReceive_No = dataBinbalance.goodsReceive_No;
                    model.goodsReceive_date = dataBinbalance.goodsReceive_Date.toString();
                    model.erp_Location = dataBinbalance.ERP_Location;
                    var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve_tranfer"), model.sJson());
                    if (insetBinRe.resultIsUse)
                    {
                        model.binCardReserve_Index = insetBinRe.items?.binCardReserve_Index;
                        var update_status_gt = dbTransfer.IM_GoodsTransferItem.Find(GoodsTransferItem.GoodsTransferItem_Index);
                        update_status_gt.Document_Status = 0;
                        dbTransfer.SaveChanges();
                    }
                    else
                    {
                        result.resultMsg = insetBinRe.resultMsg;
                        result.resultIsUse = false;
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
    }

}
