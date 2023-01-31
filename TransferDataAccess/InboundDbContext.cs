
using GIDataAccess.Models;
using TransferDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TransferDataAccess.Models;
using MasterDataDataAccess.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccess
{
    public class InboundDbContext : DbContext
    {
        public virtual DbSet<IM_GoodsReceive> IM_GoodsReceives { get; set; }
        public virtual DbSet<IM_GoodsReceiveItem> IM_GoodsReceiveItem { get; set; }
        public virtual DbSet<IM_GoodsReceiveItemLocation> IM_GoodsReceiveItemLocation { get; set; }
        public virtual DbSet<View_RPT_PrintOutTag> View_RPT_PrintOutTag { get; set; }
        public virtual DbSet<View_RPT_PrintOutTag_RePrint> View_RPT_PrintOutTag_RePrint { get; set; }
        public virtual DbSet<WM_TagItem> WM_TagItem { get; set; }
        public virtual DbSet<CheckTagPut> CheckTagPut { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("InboundConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
            //optionsBuilder.UseSqlServer(@"Server=10.0.177.33\SQLEXPRESS;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=cfrffmusr;password=ffmusr@cfr;");

            //optionsBuilder.UseSqlServer(@"Server=kascoit.ddns.net,22017;Database=WMSDB_QA;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");
        }
    }
}
