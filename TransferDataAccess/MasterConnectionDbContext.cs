using MasterDataDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using TransferDataAccess.Models;

namespace DataAccess
{
    public class MasterConnectionDbContext : DbContext
    {


        public DbSet<MS_Location> MS_Location { get; set; }

        public DbSet<View_LocationZoneputaway> View_LocationZoneputaway { get; set; }
        public DbSet<View_CheckLocation> View_CheckLocation { get; set; }

        public DbSet<View_LocatinSelectiveOnGroundExport> View_LocatinSelectiveOnGroundExport { get; set; }
        
        public DbSet<View_locationtype_import_tranfer> View_locationtype_import_tranfer { get; set; }
        public DbSet<MS_ProductConversion> ms_ProductConversion { get; set; }
        public DbSet<View_CheckLocation_unpack> View_CheckLocation_unpack { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("MasterConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
            //optionsBuilder.UseSqlServer(@"Server=10.0.177.33\SQLEXPRESS;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=cfrffmusr;password=ffmusr@cfr;");

            //optionsBuilder.UseSqlServer(@"Server=kascoit.ddns.net,22017;Database=WMSDB_QA;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");
        }
    }
}
