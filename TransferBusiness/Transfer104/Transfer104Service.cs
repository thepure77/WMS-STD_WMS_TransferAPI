using DataAccess;
using TransferBusiness.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Comone.Utils;
using TransferBusiness.Library;
using TransferBusiness.GoodsTransfer.ViewModel;
using TransferBusiness.ConfigModel;
using TransferDataAccess.Models;
using planGIBusiness.AutoNumber;
using MasterDataBusiness.ViewModels;

namespace TransferBusiness.Transfer
{
    public class Transfer104Service
    {
        private TransferDbContext db;
        private BinbalanceDbContext dbbin;
        private string _defaultStagingLocation = new AppSettingConfig().GetUrl("defaultStagingLocation");

        public Transfer104Service()
        {
            db = new TransferDbContext();
            dbbin = new BinbalanceDbContext();
        }

        public Transfer104Service(TransferDbContext db , BinbalanceDbContext dbbin)
        {
            this.db = db;
            this.dbbin = dbbin;
        }


        public Transfer104model getlocation104(Transfer104model model)
        {
            Transfer104model transfer104Model = new Transfer104model();
            try
            {
                var location_D = new SqlParameter("@location_D", model.crane == null ? "" : model.crane);
                var location_N = new SqlParameter("@location_N", model.location == null ? "" : model.location);
                var checklocation104 = dbbin.checklocation104.FromSql("sp_Binbalance_tranfer104 @location_D ,@location_N", location_D, location_N).ToList().Take(10);
                foreach (var item in checklocation104)
                {
                    Transfer104model transfer104 = new Transfer104model();
                    transfer104.BinBalance_Index = item.BinBalance_Index;
                    transfer104.Tag_No = item.Tag_No;
                    transfer104.Product_Id = item.Product_Id;
                    transfer104.Product_Name = item.Product_Name;
                    transfer104.ProductConversion_Name = item.ProductConversion_Name;
                    transfer104.Location_Index = item.Location_Index;
                    transfer104.Location_Id = item.Location_Id;
                    transfer104.Location_Name = item.Location_Name;
                    transfer104.BinBalance_QtyBegin = item.BinBalance_QtyBegin;
                    transfer104.BinBalance_QtyBal = item.BinBalance_QtyBal;
                    transfer104.BinBalance_QtyReserve = item.BinBalance_QtyReserve;
                    transfer104.GoodsReceiveItemLocation_Index = item.GoodsReceiveItemLocation_Index;
                    transfer104Model.listTransferItemViewModel.Add(transfer104);

                }
                return transfer104Model;
            }
            catch (Exception ex)
            {
                transfer104Model.resultIsUse = true;
                transfer104Model.resultMsg = ex.Message;
                return transfer104Model;
            }
            
        }

        public statusmodel call104(Transfer104model model)
        {
            statusmodel statusmodel = new statusmodel();
            try
            {
                statusmodel.status = "10";
                messagemodel messagemodel = new messagemodel();
                messagemodel.description = "create success";
                statusmodel.message = messagemodel;
                return statusmodel;

            }
            catch (Exception ex)
            {
                statusmodel.resultIsUse = true;
                statusmodel.resultMsg = ex.Message;
                return statusmodel;
            }

        }

        public Transfer104model entercall104(Transfer104model model)
        {
            Transfer104model transfer104Model = new Transfer104model();
            try
            {
                tmp_SuggestLocationCheckTF suggestLocationCheckTF = db.tmp_SuggestLocationCheckTF.FirstOrDefault(c => c.PalletID == model.Tag_No);

                transfer104Model.BinBalance_Index = suggestLocationCheckTF.BinBalance_Index;
                transfer104Model.Tag_No = suggestLocationCheckTF.PalletID;
                transfer104Model.Location_Index = suggestLocationCheckTF.Location_Index;
                transfer104Model.Location_Id = suggestLocationCheckTF.Location_Id;
                transfer104Model.Location_Name = suggestLocationCheckTF.Location_Name;
                transfer104Model.BinBalance_QtyBal = suggestLocationCheckTF.QtyBal;
                transfer104Model.BinBalance_QtyReserve = suggestLocationCheckTF.QtyReserve;
                transfer104Model.GoodsReceiveItemLocation_Index = suggestLocationCheckTF.GoodsReceiveItemLocation_Index;
                transfer104Model.resultIsUse = true;

                return transfer104Model;
            }
            catch (Exception ex)
            {
                transfer104Model.resultIsUse = false;
                transfer104Model.resultMsg = ex.Message;
                return transfer104Model;
            }

        }

    }
}
