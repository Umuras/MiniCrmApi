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

            //Category Seed Data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" },
                new Category { Id = 3, Name = "Clothing" },
                new Category { Id = 4, Name = "Home & Kitchen" },
                new Category { Id = 5, Name = "Toys & Games" }
                );

            //Product Seed Data
            modelBuilder.Entity<Product>().HasData(
                // Electronics
                new Product { Id = 1, Name = "Laptop", Price = 10000, StockQuantity = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Price = 7000, StockQuantity = 15, CategoryId = 1 },
                new Product { Id = 3, Name = "Wireless Headphones", Price = 1500, StockQuantity = 25, CategoryId = 1 },

                // Books
                new Product { Id = 4, Name = "C# Programming Book", Price = 200, StockQuantity = 50, CategoryId = 2 },
                new Product { Id = 5, Name = "Design Patterns Book", Price = 180, StockQuantity = 40, CategoryId = 2 },
                new Product { Id = 6, Name = "Database Systems Book", Price = 220, StockQuantity = 30, CategoryId = 2 },

                // Clothing
                new Product { Id = 7, Name = "T-Shirt", Price = 100, StockQuantity = 100, CategoryId = 3 },
                new Product { Id = 8, Name = "Jeans", Price = 250, StockQuantity = 60, CategoryId = 3 },
                new Product { Id = 9, Name = "Jacket", Price = 500, StockQuantity = 40, CategoryId = 3 },

                // Home & Kitchen
                new Product { Id = 10, Name = "Coffee Maker", Price = 600, StockQuantity = 20, CategoryId = 4 },
                new Product { Id = 11, Name = "Microwave Oven", Price = 1200, StockQuantity = 15, CategoryId = 4 },
                new Product { Id = 12, Name = "Blender", Price = 400, StockQuantity = 25, CategoryId = 4 },

                // Toys & Games
                new Product { Id = 13, Name = "Board Game", Price = 300, StockQuantity = 50, CategoryId = 5 },
                new Product { Id = 14, Name = "Puzzle", Price = 150, StockQuantity = 70, CategoryId = 5 },
                new Product { Id = 15, Name = "RC Car", Price = 800, StockQuantity = 30, CategoryId = 5 }
                );

            //Customer Seed Data
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Ali Umur", City = "Istanbul", Address = "Istanbul", PhoneNumber = "99999999999" },
                new Customer { Id = 2, Name = "Mustafa Asim", City = "Istanbul", Address = "Istanbul", PhoneNumber = "88888888888" }
                );
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
