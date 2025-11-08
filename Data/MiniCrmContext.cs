using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Models;

namespace MiniCrmApi.Data
{
    public class MiniCrmContext : DbContext
    {
        //Constructor oluşturuldu, bu Constructor sayesinde, DbContext'e ConnectionString ve hangi SqlServer'ına bağlanacağımız
        //gibi bilgileri söylüyoruz.
        public MiniCrmContext(DbContextOptions<MiniCrmContext> dbContextOptions) : base(dbContextOptions) { }


        //DbSetler
        //Burada DbSet<Custormer> bizim Customer tablomuzu, Customers Property ismi ise o tablodaki tüm kayıtları temsil ediyor.
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Burada ise FluentApi'yi kullanacağız, bu sayede DbSet'e tabloların ilişki durumunu daha açıkça belirteceğiz.
        //Bu metod EF Core tarafından migration işlemi yapılırken otomatik olarak çağrılır.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //CUSTOMER TABLOSU

            ConfigureCustomer(modelBuilder);

            //PRODUCT TABLOSU

            ConfigureProduct(modelBuilder);

            //ORDER TABLOSU

            ConfigureOrder(modelBuilder);

            //ORDERDETAIL TABLOSU

            ConfigureOrderDetail(modelBuilder);

            //Category TABLOSU

            ConfigureCategory(modelBuilder);
        }

        private void ConfigureCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer")
                      .HasKey(p => p.Id);

                entity.Property(p => p.Name) //Lambda expression üzerinden, Customer sınıfı türünde nesne alıp oradan Name propertysine erişiyoruz.
                      .IsRequired() //NOT NULL yapıyoruz, yani boş bırakılamaz.
                      .HasMaxLength(100); //Maximum 100 karakter olmalı, VARCHAR(100);

                entity.Property(p => p.Address)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.City)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(p => p.PhoneNumber)
                      .IsRequired()
                      .HasMaxLength(15);

                //Customer tablosu OneToMany ilişki, Bir müşterinin birden fazla siparişi olabilir.
                entity.HasMany(c => c.Orders)
                      .WithOne(c => c.Customer)
                      .HasForeignKey(o => o.CustomerId);
            });
        }

        private void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {

               entity.ToTable("Product")
                     .HasKey(p => p.Id);


               entity.Property(p => p.Name)
                     .IsRequired()
                     .HasMaxLength(30);


               entity.Property(p => p.Description)
                     .HasMaxLength(50);


               entity.Property(p => p.StockQuantity)
                     .IsRequired();


               entity.Property(p => p.Price)
                     .IsRequired()
                     .HasColumnType("decimal(18,2)");


               entity.HasOne(p => p.Category) //Product'ın bir Category'si vardır.
                     .WithMany(c => c.Products) //Category'nin birçok Product'ı vardır.
                     .HasForeignKey(p => p.CategoryId); //Foreign key alanı.


               entity.HasMany(p => p.OrderDetails)
                     .WithOne(od => od.Product)
                     .HasForeignKey(od => od.ProductId);
            });
        }

        private void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order")
                      .HasKey(o => o.Id);

                entity.Property(o => o.TotalPrice)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(o => o.TotalQuantity)
                      .IsRequired()
                      .HasColumnType("int");

                entity.Property(o => o.CreatedDate)
                      .IsRequired();

                entity.HasOne(o => o.Customer)
                      .WithMany(o => o.Orders)
                      .HasForeignKey(o => o.CustomerId);

                entity.HasMany(o => o.OrderDetails)
                      .WithOne(o => o.Order)
                      .HasForeignKey(o => o.OrderId);
            });
        }

        private void ConfigureOrderDetail(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail")
                      .HasKey(od => od.Id);

                entity.Property(od => od.Price)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(od => od.Product)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(od => od.ProductId);

                entity.HasOne(od => od.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderId);
            });
        }

        private void ConfigureCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category")
                      .HasKey(c => c.Id);

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasMany(c => c.Products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.CategoryId);
            });
        }
    }
}
