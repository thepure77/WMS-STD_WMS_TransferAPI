using BinBalanceDataAccess.Models;
using GRDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class BinbalanceDbContext : DbContext
    {

        public virtual DbSet<wm_BinBalance> wm_BinBalance { get; set; }
        public virtual DbSet<wm_BinCard> wm_BinCard { get; set; }
        public virtual DbSet<wm_BinCardReserve> wm_BinCardReserve { get; set; }
        public virtual DbSet<View_WaveBinBalance> View_WaveBinBalance { get; set; }
        public virtual DbSet<View_WaveBinBalance_Transfer> View_WaveBinBalance_Transfer { get; set; }
        public virtual DbSet<View_aging> View_aging { get; set; }

        public virtual DbSet<View_InquirySKU> View_InquirySKU { get; set; }
        public virtual DbSet<View_InquirySKU_AllocatedBy> View_InquirySKU_AllocatedBy { get; set; }
        public virtual DbSet<View_InquirySKU_Conversion> View_InquirySKU_Conversion { get; set; }
        public virtual DbSet<wm_BinBalanceServiceCharge> wm_BinBalanceServiceCharge { get; set; }

        public virtual DbSet<im_Invoice> im_Invoice { get; set; }
        public virtual DbSet<im_InvoiceItem> im_InvoiceItem { get; set; }

        public virtual DbSet<GetValueByColumn> GetValueByColumn { get; set; }
        public virtual DbSet<im_InvoiceStorageCharge> im_InvoiceStorageCharge { get; set; }

        public virtual DbSet<im_Memo> im_Memo { get; set; }
        public virtual DbSet<im_MemoItem> im_MemoItem { get; set; }
        public virtual DbSet<View_MEMO> View_MEMO { get; set; }
        public virtual DbSet<View_CalMemo> View_CalMemo { get; set; }

        public virtual DbSet<checklocation104> checklocation104 { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("BinConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }


        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
