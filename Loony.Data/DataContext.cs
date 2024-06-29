using Loony.Data.Entities.Product;
using Loony.Data.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Loony.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #region System
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Menu> Menu { get; set; }

        public DbSet<Menu_User> Menu_User { get; set; }

        public DbSet<FileGroup> FileGroups { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<File_Tag> File_Tag { get; set; }
        #endregion

        #region Product
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CareInstruction> CareInstructions { get; set; }
        //public DbSet<Size> Sizes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<AccessoryType> AccessoryTypes { get; set; }
        public DbSet<DesignType> DesignTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<ProductChain> ProductChains { get; set; }
        public DbSet<Article_DesignType> Article_DesignType { get; set; }
        public DbSet<Model_Tag> Model_Tag { get; set; }



        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();

            //modelBuilder.Entity<Product_Model>()
            //    .HasKey(x => new { x.ProductId, x.ModelId });

            //modelBuilder.Entity<Product_Model>()
            //    .HasOne(x => x.Product)
            //    .WithMany(p => p.Models)
            //    .HasForeignKey(pc => pc.PersonId);

            //modelBuilder.Entity<PersonClub>()
            //    .HasOne(pc => pc.Club)
            //    .WithMany(c => c.PersonClubs)
            //    .HasForeignKey(pc => pc.ClubId);

        }
    }
}
