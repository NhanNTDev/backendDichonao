using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DiCho.DataService.Models
{
    public partial class DiChoNaoContext : IdentityDbContext<AspNetUsers, AspNetRoles, string, AspNetUserClaims, AspNetUserRoles,
                                                             AspNetUserLogins, AspNetRoleClaims, AspNetUserTokens>
    {
        public DiChoNaoContext()
        {
        }

        public DiChoNaoContext(DbContextOptions<DiChoNaoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<CampaignDeliveryZone> CampaignDeliveryZones { get; set; }
        public virtual DbSet<Farm> Farms { get; set; }
        public virtual DbSet<FarmOrder> FarmOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductHarvest> ProductHarvests { get; set; }
        public virtual DbSet<ProductHarvestInCampaign> ProductHarvestInCampaigns { get; set; }
        public virtual DbSet<ProductHarvestOrder> ProductHarvestOrders { get; set; }
        public virtual DbSet<ProductSalesCampaign> ProductSalesCampaigns { get; set; }
        public virtual DbSet<ProductSystem> ProductSystems { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentDestination> ShipmentDestinations { get; set; }
        public virtual DbSet<WareHouse> WareHouses { get; set; }
        public virtual DbSet<WareHouseZone> WareHouseZones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=VUONGNGUYEN;Database=DiChoNao;user id=sa;password=123;Trusted_Connection=false;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Address1)
                    .HasMaxLength(256)
                    .HasColumnName("Address");

                entity.Property(e => e.CustomerId).HasMaxLength(450);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Address__Custome__71F1E3A2");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.Image).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.Property(e => e.CampaignZoneName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.EndRecruitmentAt).HasColumnType("datetime");

                entity.Property(e => e.Image1).HasMaxLength(500);

                entity.Property(e => e.Image2).HasMaxLength(500);

                entity.Property(e => e.Image3).HasMaxLength(500);

                entity.Property(e => e.Image4).HasMaxLength(500);

                entity.Property(e => e.Image5).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.StartAt).HasColumnType("datetime");

                entity.Property(e => e.StartRecruitmentAt).HasColumnType("datetime");

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<CampaignDeliveryZone>(entity =>
            {
                entity.ToTable("CampaignDeliveryZone");

                entity.Property(e => e.DeliveryZoneName).HasMaxLength(50);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignDeliveryZones)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK__CampaignD__Campa__44FF419A");
            });

            modelBuilder.Entity<Farm>(entity =>
            {
                entity.ToTable("Farm");

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.Property(e => e.Avatar).HasMaxLength(500);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.FarmZoneName).HasMaxLength(50);

                entity.Property(e => e.FarmerId).HasMaxLength(450);

                entity.Property(e => e.Image1).HasMaxLength(500);

                entity.Property(e => e.Image2).HasMaxLength(500);

                entity.Property(e => e.Image3).HasMaxLength(500);

                entity.Property(e => e.Image4).HasMaxLength(500);

                entity.Property(e => e.Image5).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.HasOne(d => d.Farmer)
                    .WithMany(p => p.Farms)
                    .HasForeignKey(d => d.FarmerId)
                    .HasConstraintName("FK__Farm__FarmerId__3E723F9C");
            });

            modelBuilder.Entity<FarmOrder>(entity =>
            {
                entity.ToTable("FarmOrder");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CollectionCode).HasMaxLength(50);

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.DriverId).HasMaxLength(450);

                entity.Property(e => e.FeedBackCreateAt).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.HasOne(d => d.Farm)
                    .WithMany(p => p.FarmOrders)
                    .HasForeignKey(d => d.FarmId)
                    .HasConstraintName("FK__FarmOrder__FarmI__004002F9");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.FarmOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__FarmOrder__Order__01342732");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasMaxLength(450);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.DeliveryCode).HasMaxLength(50);

                entity.Property(e => e.DriverId).HasMaxLength(450);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK__Order__CampaignI__76B698BF");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Order__CustomerI__77AABCF8");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Payment__OrderId__7C6F7215");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .HasConstraintName("FK__Payment__Payment__4BAC3F29");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.ToTable("PaymentType");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Image).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");
            });

            modelBuilder.Entity<ProductHarvest>(entity =>
            {
                entity.ToTable("ProductHarvest");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.EstimatedProduction).HasMaxLength(50);

                entity.Property(e => e.EstimatedTime).HasColumnType("datetime");

                entity.Property(e => e.Image1).HasMaxLength(500);

                entity.Property(e => e.Image2).HasMaxLength(500);

                entity.Property(e => e.Image3).HasMaxLength(500);

                entity.Property(e => e.Image4).HasMaxLength(500);

                entity.Property(e => e.Image5).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.StartAt).HasColumnType("datetime");

                entity.HasOne(d => d.Farm)
                    .WithMany(p => p.ProductHarvests)
                    .HasForeignKey(d => d.FarmId)
                    .HasConstraintName("FK__Harvest__FarmId__47FBA9D6");

                entity.HasOne(d => d.ProductSystem)
                    .WithMany(p => p.ProductHarvests)
                    .HasForeignKey(d => d.ProductSystemId)
                    .HasConstraintName("FK__Harvest__Product__48EFCE0F");
            });

            modelBuilder.Entity<ProductHarvestInCampaign>(entity =>
            {
                entity.ToTable("ProductHarvestInCampaign");

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.Unit).HasMaxLength(20);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.ProductHarvestInCampaigns)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK__HarvestCa__Campa__4CC05EF3");

                entity.HasOne(d => d.Harvest)
                    .WithMany(p => p.ProductHarvestInCampaigns)
                    .HasForeignKey(d => d.HarvestId)
                    .HasConstraintName("FK__HarvestCa__Harve__4BCC3ABA");
            });

            modelBuilder.Entity<ProductHarvestOrder>(entity =>
            {
                entity.ToTable("ProductHarvestOrder");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.Unit).HasMaxLength(20);

                entity.HasOne(d => d.FarmOrder)
                    .WithMany(p => p.ProductHarvestOrders)
                    .HasForeignKey(d => d.FarmOrderId)
                    .HasConstraintName("FK__HarvestOr__FarmO__041093DD");

                entity.HasOne(d => d.HarvestCampaign)
                    .WithMany(p => p.ProductHarvestOrders)
                    .HasForeignKey(d => d.HarvestCampaignId)
                    .HasConstraintName("FK__HarvestOr__Harve__0504B816");
            });

            modelBuilder.Entity<ProductSalesCampaign>(entity =>
            {
                entity.ToTable("ProductSalesCampaign");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.ProductSalesCampaigns)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK__ProductSa__Campa__52593CB8");

                entity.HasOne(d => d.ProductSystem)
                    .WithMany(p => p.ProductSalesCampaigns)
                    .HasForeignKey(d => d.ProductSystemId)
                    .HasConstraintName("FK__ProductSa__Produ__534D60F1");
            });

            modelBuilder.Entity<ProductSystem>(entity =>
            {
                entity.ToTable("ProductSystem");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(20);

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.ProductSystems)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__ProductSy__Produ__5441852A");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("Shipment");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateAt).HasColumnType("date");

                entity.Property(e => e.DriverId).HasMaxLength(450);

                entity.Property(e => e.From).HasMaxLength(256);

                entity.Property(e => e.To).HasMaxLength(256);
            });

            modelBuilder.Entity<ShipmentDestination>(entity =>
            {
                entity.ToTable("ShipmentDestination");

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.HasOne(d => d.Shipment)
                    .WithMany(p => p.ShipmentDestinations)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK__ShipmentD__Shipm__160F4887");
            });

            modelBuilder.Entity<WareHouse>(entity =>
            {
                entity.ToTable("WareHouse");

                entity.Property(e => e.Address).HasMaxLength(256);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .UseCollation("SQL_Latin1_General_CP1253_CI_AI");

                entity.Property(e => e.WarehouseManagerId).HasMaxLength(450);
            });

            modelBuilder.Entity<WareHouseZone>(entity =>
            {
                entity.ToTable("WareHouseZone");

                entity.Property(e => e.WareHouseZoneName).HasMaxLength(50);

                entity.HasOne(d => d.WareHouse)
                    .WithMany(p => p.WareHouseZones)
                    .HasForeignKey(d => d.WareHouseId)
                    .HasConstraintName("FK__WareHouse__WareH__19FFD4FC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
