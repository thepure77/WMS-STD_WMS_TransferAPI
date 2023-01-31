using DataAccess;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using TransferBusiness.Transfer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Comone.Utils;
using TransferBusiness.Library;
using TransferDataAccess.Models;
using planGIBusiness.AutoNumber;
using TransferBusiness.ConfigModel;
using MasterDataBusiness.ViewModels;
using TransferBusiness.GoodsTransfer.ViewModel;
using GRBusiness.GoodsReceive;

namespace TransferBusiness.TranferDynamicSlottingService
{
    public class TranferDynamicSlottingService
    {
        private TransferDbContext dbTranfer;
        private BinbalanceDbContext dbBinbalance;
        private InboundDbContext dbInbound;
        private MasterConnectionDbContext dbMaster;

        public TranferDynamicSlottingService()
        {
            dbTranfer = new TransferDbContext();
            dbBinbalance = new BinbalanceDbContext();
            dbInbound = new InboundDbContext();
            dbMaster = new MasterConnectionDbContext();
        }

        public TranferDynamicSlottingService(TransferDbContext db, BinbalanceDbContext dbBinbalance, InboundDbContext dbInbound, MasterConnectionDbContext dbMaster)
        {
            this.dbTranfer = dbTranfer;
            this.dbBinbalance = dbBinbalance;
            this.dbInbound = dbInbound;
            this.dbMaster = dbMaster;

        }

        public TranferDynamicSlottingViewModel GenDynamicSlotting(TranferDynamicSlottingViewModel data)
        {
            logtxt log = new logtxt();
            var msglog = "";
            TranferDynamicSlottingViewModel result = new TranferDynamicSlottingViewModel();
            log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting" + DateTime.Now.ToString("yyyy-MM-dd"), "Start GenDynamicSlotting: " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            try
            {

                IM_GoodsTransfer goodsTransfer_status = dbTranfer.IM_GoodsTransfer.FirstOrDefault(c => c.UDF_5 == data.DynamicSlotting_Index.ToString() && c.Document_Status != -1 && c.WCS_status != 20);
                if (goodsTransfer_status != null)
                {
                    log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", "listBinbalnace not found.");
                    result.resultIsUse = false;
                    result.resultMsg = "Config Already Process";
                    return result;
                }


                var pZoneputaway_Index = new SqlParameter("@Zoneputaway_Index", data.Zoneputaway_Index); // Zoneputaway_Index

                var pProduct_Index = new SqlParameter("@Product_Index", data.Product_Index); //Product_Index

                var listBinbalnace = dbBinbalance.wm_BinBalance.FromSql("sp_GetLocationDynamicSlottingByZoneSKU @Zoneputaway_Index , @Product_Index ", pZoneputaway_Index, pProduct_Index).ToList();

                string crane_id = "";
                string crane_id_To = "";
               
                switch (data.crane_Name)
                {

                    case string crane when (crane == "1"):
                        crane_id = ("AA");
                        crane_id_To = ("AB");
                        break;

                    case string crane when (crane == "2"):
                        crane_id = ("AC");
                        crane_id_To = ("AD");
                        break;

                    case string crane when (crane == "3"):
                        crane_id = ("AE");
                        crane_id_To = ("AF");
                        break;

                    case string crane when (crane == "4"):
                        crane_id = ("AG");
                        crane_id_To = ("AH");
                        break;

                    case string crane when (crane == "5"):
                        crane_id = ("AI");
                        crane_id_To = ("AJ");
                        break;

                    case string crane when (crane == "6"):
                        crane_id = ("AK");
                        crane_id_To = ("AL");
                        break;

                    case string crane when (crane == "7"):
                        crane_id = ("AM");
                        crane_id_To = ("AN");
                        break;

                    case string crane when (crane == "8"):
                        crane_id = ("AO");
                        crane_id_To = ("AP");
                        break;

                    case string crane when (crane == "9"):
                        crane_id = ("AQ");
                        crane_id_To = ("AR");
                        break;

                    case string crane when (crane == "10"):
                        crane_id = ("AS");
                        crane_id_To = ("AT");
                        break;

                    case string crane when (crane == "11"):
                        crane_id = ("AU");
                        crane_id_To = ("AV");
                        break;
                }

                listBinbalnace = listBinbalnace.Where(c => c.Location_Name.Contains(crane_id) || c.Location_Name.Contains(crane_id_To)).OrderBy(c=> c.Location_Name).Take(15).ToList();

                if (listBinbalnace.Count == 0)
                {
                    log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", "listBinbalnace not found.");
                    result.resultIsUse = false;
                    result.resultMsg = "listBinbalnace not found.";
                    return result;
                }

                //--------- Create Trasnfer Heder----------
                var filterModel = new DocumentTypeViewModel();
                filterModel.process_Index = new Guid("CE757517-EBBC-4BEA-93CC-F7E139AE422C");
                filterModel.documentType_Index = Guid.Parse("12786477-EBE7-4FE9-B521-59DD829946E4");

                var result_data = new List<GenDocumentTypeViewModel> ();
                result_data = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                IM_GoodsTransfer itemHeader = new IM_GoodsTransfer();
                DateTime DocumentDate = DateTime.Now;
                var genDoc = new AutoNumberService();
                Guid GoodsTransfer_Index = Guid.NewGuid();
                string GoodsTransferNo = genDoc.genAutoDocmentNumber(result_data, DocumentDate);

                itemHeader.GoodsTransfer_Index = GoodsTransfer_Index;
                itemHeader.GoodsTransfer_No = GoodsTransferNo;
                itemHeader.GoodsTransfer_Date = DateTime.Now;
                itemHeader.GoodsTransfer_Time = null;
                itemHeader.GoodsTransfer_Doc_Date = DateTime.Now;
                itemHeader.GoodsTransfer_Doc_Time = null;
                itemHeader.Owner_Index = Guid.Parse("02B31868-9D3D-448E-B023-05C121A424F4");
                itemHeader.Owner_Id = "3419";
                itemHeader.Owner_Name = "ศูนย์กระจายสินค้าคาเฟ่อเมซอน	OR Amazon DC";
                itemHeader.DocumentType_Index = result_data[0].documentType_Index.GetValueOrDefault();
                itemHeader.DocumentType_Id = result_data[0].documentType_Id;
                itemHeader.DocumentType_Name = result_data[0].documentType_Name;
                itemHeader.DocumentRef_No2 = null;
                itemHeader.DocumentRef_No3 = null;
                itemHeader.DocumentRef_No4 = null;
                itemHeader.DocumentRef_No5 = null;
                itemHeader.UDF_1 = null;
                itemHeader.UDF_2 = null;
                itemHeader.UDF_3 = null;
                itemHeader.UDF_4 = null;
                itemHeader.UDF_5 = data.DynamicSlotting_Index.ToString();

                itemHeader.Document_Status = -2;

                itemHeader.Create_By = data.Create_By;
                itemHeader.Create_Date = DateTime.Now;

                dbTranfer.IM_GoodsTransfer.Add(itemHeader);

                //-----------------------------------------

                foreach (var item in listBinbalnace)
                {
                    // Get New Location In Zone Putaway

                    #region Get Data LocationZoneputaway
                    //binbalance
                    //View_LocationSuggestWCSV2

                    var _locationZP  = dbMaster.View_LocationZoneputaway.Where(c => c.zoneputaway_Index == data.Zoneputaway_Index && (c.location_Name.Contains(crane_id) || c.location_Name.Contains(crane_id_To))).OrderBy(o => o.PutAway_Seq).ThenBy(o => o.location_Name).FirstOrDefault(); //location_Id
                    if (_locationZP == null)
                    {
                        log.DataLogLines("GenDynamicSlotting" , "GenDynamicSlotting", "View_LocationZoneputaway not found.");
                        continue;
                    }

                    var _loModel = dbMaster.MS_Location.Where(c => c.Location_Index == _locationZP.location_Index).FirstOrDefault();
                    if (_loModel == null)
                    {
                        log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", "ms_Location not found.");
                        continue;
                    }
                    
                    var Temp_Index = Guid.NewGuid();
                    string cmd1 = "";
                    string cmd2 = "";
                    string cmd3 = "";
                    cmd1 = "  INSERT INTO  WMSDB_AMZ_Master_V3..tmp_SuggestLocationPutAway ";
                    cmd1 += "            ([Temp_Index]       ";
                    cmd1 += "            ,[PalletID]       ";
                    cmd1 += "            ,[Location_Index]      ";
                    cmd1 += "            ,[Location_Id]       ";
                    cmd1 += "            ,[Location_Name]      ";
                    cmd1 += "            ,[Create_By]       ";
                    cmd1 += "            ,[Create_Date]       ";
                    cmd1 += "            ,[IsComplete]       ";
                    cmd1 += "     ) VALUES (         ";
                    cmd1 += "             '" + Temp_Index.ToString() + "' ";  //<Temp_Index, uniqueidentifier,>  
                    cmd1 += "            ,'" + item.Tag_No + "' ";  //<PalletID, nvarchar(50),>    
                    cmd1 += "            ,'" + _loModel.Location_Index.ToString() + "' ";  //<Location_Index, uniqueidentifier,>
                    cmd1 += "            ,'" + _loModel.Location_Id + "' ";  //<Location_Id, nvarchar(50),>   
                    cmd1 += "            ,'" + _loModel.Location_Name + "' ";  //<Location_Name, nvarchar(200),>  
                    cmd1 += "            ,'" + "AutoDynamicSlotting" + "' ";  //<Create_By, nvarchar(200),>   
                    cmd1 += "            , getdate() ";  //<Create_Date, datetime,>    
                    cmd1 += "            , 0 )";  //<IsComplete, int,>) 


                    log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", "ExecuteSqlCommand cmd1 : " + cmd1);


                    cmd2 += "    UPdate  WMSDB_AMZ_Master_V3..tmp_SuggestLocationPutAway   set ";
                    cmd2 += "  [Location_Name] = [Location_Name] ";
                    cmd2 += " WHERE Location_Index = '" + _loModel.Location_Index.ToString() + "'";

                    log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", "ExecuteSqlCommand cmd2 : " + cmd2);

                    int rowAff1 = 0;
                    int rowAff2 = 0;
                    int rowAff3 = 0;
                    var transtmp = dbTranfer.Database.BeginTransaction();
                    try
                    {
                        //var strSQL1 = new SqlParameter("@strSQL", cmd1 );
                        //rowAff1 = db.Database.ExecuteSqlCommand("EXEC sp_RunExec @strSQL", strSQL1);
                        rowAff1 = dbTranfer.Database.ExecuteSqlCommand(cmd1);

                        transtmp.Commit();
                    }
                    catch (Exception exxx)
                    {
                        msglog = " exxx Rollback " + exxx.Message.ToString();
                        log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", msglog);
                        transtmp.Rollback();
                        // throw exxx;

                    }

                    var transtmp2 = dbTranfer.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    try
                    {
                        //var strSQL2 = new SqlParameter("@strSQL", cmd2);
                        //rowAff1 = db.Database.ExecuteSqlCommand("EXEC sp_RunExec @strSQL", strSQL2);

                        rowAff2 = dbTranfer.Database.ExecuteSqlCommand(cmd2);
                        transtmp2.Commit();
                    }

                    catch (Exception exxx2)
                    {

                        msglog = " exxx2 Rollback " + exxx2.Message.ToString();
                        log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", msglog);
                        transtmp2.Rollback();


                    }
                    if (rowAff1 != rowAff2 || rowAff1 == 0 || rowAff2 == 0)
                    {



                        cmd3 = "  delete  WMSDB_AMZ_Master_V3..tmp_SuggestLocationPutAway    ";
                        cmd3 += " WHERE Temp_Index = '" + Temp_Index.ToString() + "'";

                        log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", "ExecuteSqlCommand cmd3 : " + cmd3);

                        var transtmp3 = dbTranfer.Database.BeginTransaction();
                        try
                        {
                            rowAff3 = dbTranfer.Database.ExecuteSqlCommand(cmd3);
                            transtmp3.Commit();
                        }

                        catch (Exception exxx3)
                        {

                            msglog = " exxx2 Rollback " + exxx3.Message.ToString();
                            log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", msglog);
                            transtmp3.Rollback();


                        }

                        msglog = "continue tmp_SuggestLocationPutAway = 0";
                        log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting", msglog);

                        continue;
                    }
                    else
                    {

                        PickbinbalanceViewModel model = new PickbinbalanceViewModel();

                        //--------- Create Trasnfer ITEM --------

                        var GoodsTransferItem = new IM_GoodsTransferItem();
                        GoodsTransferItem.GoodsTransferItem_Index = Guid.NewGuid();
                        GoodsTransferItem.GoodsTransfer_Index = itemHeader.GoodsTransfer_Index;
                        GoodsTransferItem.TagItem_Index = item.TagItem_Index;
                        GoodsTransferItem.Tag_Index = item.Tag_Index;
                        GoodsTransferItem.Tag_No = item.Tag_No;
                        GoodsTransferItem.Product_Index = item.Product_Index;
                        GoodsTransferItem.Product_Id = item.Product_Id;
                        GoodsTransferItem.Product_Name = item.Product_Name;
                        GoodsTransferItem.Product_SecondName = item.Product_SecondName;
                        GoodsTransferItem.Product_ThirdName = item.Product_ThirdName;
                        GoodsTransferItem.Product_Lot = item.Product_Lot;
                        GoodsTransferItem.Product_Lot_To = item.Product_Lot;
                        GoodsTransferItem.ItemStatus_Index = item.ItemStatus_Index;
                        GoodsTransferItem.ItemStatus_Id = item.ItemStatus_Id;
                        GoodsTransferItem.ItemStatus_Name = item.ItemStatus_Name;
                        GoodsTransferItem.ItemStatus_Index_To = item.ItemStatus_Index;
                        GoodsTransferItem.ItemStatus_Id_To = item.ItemStatus_Id;
                        GoodsTransferItem.ItemStatus_Name_To = item.ItemStatus_Name;
                        GoodsTransferItem.Location_Index = item.Location_Index;
                        GoodsTransferItem.Location_Id = item.Location_Id;
                        GoodsTransferItem.Location_Name = item.Location_Name;
                        GoodsTransferItem.Location_Index_To = _loModel.Location_Index;
                        GoodsTransferItem.Location_Id_To = _loModel.Location_Id;
                        GoodsTransferItem.Location_Name_To = _loModel.Location_Name;
                        GoodsTransferItem.Qty = item.BinBalance_QtyBal.GetValueOrDefault();
                        GoodsTransferItem.Ratio = item.BinBalance_Ratio;
                        GoodsTransferItem.TotalQty = item.BinBalance_QtyBal;
                        GoodsTransferItem.ProductConversion_Index = item.ProductConversion_Index;
                        GoodsTransferItem.ProductConversion_Id = item.ProductConversion_Id;
                        GoodsTransferItem.ProductConversion_Name = item.ProductConversion_Name;

                        if (item.BinBalance_WeightBegin != 0)
                        {
                            var unitWeight = item.BinBalance_WeightBegin / item.BinBalance_QtyBegin;
                            model.unitWeight = unitWeight;

                            var WeightReserve = (item.BinBalance_QtyBal * unitWeight);
                            GoodsTransferItem.Weight = WeightReserve;

                        }

                        if (item.BinBalance_VolumeBegin != 0)
                        {
                            var unitVol = item.BinBalance_VolumeBegin / item.BinBalance_QtyBegin;

                            var VolReserve = (item.BinBalance_QtyBal * unitVol);
                            GoodsTransferItem.Volume = VolReserve;
                        }

                        GoodsTransferItem.Document_Status = -3;

                        GoodsTransferItem.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                        GoodsTransferItem.GoodsReceive_Index = item.GoodsReceive_Index;
                        GoodsTransferItem.GoodsReceiveItemLocation_Index = item.GoodsReceiveItemLocation_Index;

                        GoodsTransferItem.ERP_Location = item.ERP_Location;
                        GoodsTransferItem.ERP_Location_To = item.ERP_Location;

                        //getGRI
                        var listGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = item.GoodsReceiveItem_Index } };
                        var GRItem = new DocumentViewModel();
                        GRItem.listDocumentViewModel = listGRItem;
                        var GoodsReceiveItem = utils.SendDataApi<List<GoodsReceiveItemV2ViewModel>>(new AppSettingConfig().GetUrl("FindGoodsReceiveItem"), GRItem.sJson());
                        GoodsTransferItem.UDF_1 = item.GoodsReceive_No;
                        GoodsTransferItem.UDF_2 = GoodsReceiveItem?.FirstOrDefault().ref_Document_No;

                        //getPGRI
                        var listPGRItem = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = GoodsReceiveItem?.FirstOrDefault().ref_DocumentItem_Index } };
                        var PGRItem = new DocumentViewModel();
                        PGRItem.listDocumentViewModel = listPGRItem;
                        var PlanGoodsReceiveItem = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGoodsReceiveItem"), PGRItem.sJson());
                        GoodsTransferItem.UDF_3 = PlanGoodsReceiveItem?.FirstOrDefault().documentRef_No2;

                        GoodsTransferItem.Create_By = data.Create_By;
                        GoodsTransferItem.Create_Date = DateTime.Now;

                        dbTranfer.IM_GoodsTransferItem.Add(GoodsTransferItem);


                        // dbTranfer.SaveChanges();

                        var transaction = dbTranfer.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            dbTranfer.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception exy)
                        {
                            transaction.Rollback();
                            throw exy;
                        }

                        model.binbalance_Index = item.BinBalance_Index.ToString();
                        model.pick = item.BinBalance_QtyBal;
                        model.productConversion_Ratio = item.BinBalance_Ratio;
                        model.process_Index = "CE757517-EBBC-4BEA-93CC-F7E139AE422C";
                        model.goodsTransfer_Index = itemHeader.GoodsTransfer_Index.ToString();
                        model.goodsTransferItem_Index = GoodsTransferItem.GoodsTransferItem_Index.ToString();
                        model.goodsTransfer_No = itemHeader.GoodsTransfer_No;
                        model.create_By = data.Create_By;

                        // ไม่ได้ใช้ (model เดิม)
                        model.goodsReceive_Index = item.GoodsReceive_Index.ToString();
                        model.goodsReceive_No = item.GoodsReceive_No;
                        model.goodsReceive_date = item.GoodsReceive_Date.toString();
                        model.erp_Location = item.ERP_Location;


                        var insetBinRe = utils.SendDataApi<actionResultPickbinbalanceViewModel>(new AppSettingConfig().GetUrl("insertBinCardReserve_tranfer"), model.sJson());
                        if (insetBinRe.resultIsUse)
                        {
                            var update_status_gt = dbTranfer.IM_GoodsTransferItem.Find(GoodsTransferItem.GoodsTransferItem_Index);
                            update_status_gt.Document_Status = 0;
                            dbTranfer.SaveChanges();
                        }
                        else
                        {
                            result.resultMsg = insetBinRe.resultMsg;
                            result.resultIsUse = false;
                            return result;
                        }
                        //-------------------------------------------

                    }



                    #endregion



                }


                List<IM_GoodsTransferItem> goodsTransferItems = dbTranfer.IM_GoodsTransferItem.Where(c => c.GoodsTransfer_Index == GoodsTransfer_Index && c.Document_Status == 0).ToList();

                if (goodsTransferItems.Count > 0)
                {
                    IM_GoodsTransfer goodsTransfer = dbTranfer.IM_GoodsTransfer.FirstOrDefault(c => c.GoodsTransfer_Index == GoodsTransfer_Index && c.Document_Status == -2);
                    if (goodsTransfer != null)
                    {
                        goodsTransfer.Document_Status = 0;

                        var transaction = dbTranfer.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            dbTranfer.SaveChanges();
                            transaction.Commit();

                            result.resultIsUse = true;
                            result.resultMsg = GoodsTransferNo;
                        }
                        catch (Exception exy)
                        {
                            transaction.Rollback();
                            throw exy;
                        }



                    }
                }
                else {
                    //esult.resultMsg = insetBinRe.resultMsg;
                    result.resultIsUse = false;
                    
                    return result;
                }


                return result;
            }
            catch (Exception ex)
            {
                log.DataLogLines("GenDynamicSlotting", "GenDynamicSlotting" + DateTime.Now.ToString("yyyy-MM-dd"), "GenDynamicSlotting Error EX: "+ JsonConvert.SerializeObject(ex) + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }





    }

}
