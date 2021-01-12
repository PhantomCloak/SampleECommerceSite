using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFurni.Shared.Models
{
    public partial class EFurniContext : DbContext
    {
        public EFurniContext(DbContextOptions<EFurniContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BasketItem> BasketItem { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerBasket> CustomerBasket { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<CustomerOrderAddress> CustomerOrderAddress { get; set; }
        public virtual DbSet<CustomerOrderItem> CustomerOrderItem { get; set; }
        public virtual DbSet<CustomerReview> CustomerReview { get; set; }
        public virtual DbSet<PostalService> PostalService { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductSalesStatistic> ProductSalesStatistic { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreAddress> StoreAddress { get; set; }
        public virtual DbSet<StoreSalesStatistic> StoreSalesStatistic { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("Connection string not specified.");
            }
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasDefaultValueSql("nextval('account_seq'::regclass)");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(64);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<BasketItem>(entity =>
            {
                entity.ToTable("basket_item");

                entity.Property(e => e.BasketItemId)
                    .HasColumnName("basket_item_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.BasketId).HasColumnName("basket_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Basket)
                    .WithMany(p => p.BasketItem)
                    .HasForeignKey(d => d.BasketId)
                    .HasConstraintName("basket_item_basket_id_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BasketItem)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("basket_item_product_id_fkey");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");

                entity.Property(e => e.BrandId)
                    .HasColumnName("brand_id")
                    .HasDefaultValueSql("nextval('brand_seq'::regclass)");

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasColumnName("brand_name")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasDefaultValueSql("nextval('category_seq'::regclass)");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.AccountId)
                    .HasName("customer_account_id_key")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasDefaultValueSql("nextval('customer_seq'::regclass)");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(32);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(32);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(48);

                entity.Property(e => e.ProfilePictureUrl).HasColumnName("profile_picture_url");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.AccountId)
                    .HasConstraintName("customer_account_id_fkey");
            });

            modelBuilder.Entity<CustomerBasket>(entity =>
            {
                entity.HasKey(e => e.BasketId)
                    .HasName("customer_basket_pkey");

                entity.ToTable("customer_basket");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("customer_basket_customer_id_key")
                    .IsUnique();

                entity.Property(e => e.BasketId)
                    .HasColumnName("basket_id")
                    .HasDefaultValueSql("nextval('customer_basket_seq'::regclass)");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.HasOne(d => d.Customer)
                    .WithOne(p => p.CustomerBasket)
                    .HasForeignKey<CustomerBasket>(d => d.CustomerId)
                    .HasConstraintName("customer_basket_customer_id_fkey");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("customer_order_pkey");

                entity.ToTable("customer_order");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("idx_customer_order_customer_id");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasDefaultValueSql("nextval('customer_order_seq'::regclass)");

                entity.Property(e => e.AdditionalNote).HasColumnName("additional_note");

                entity.Property(e => e.CargoPrice).HasColumnName("cargo_price");

                entity.Property(e => e.CouponDiscount).HasColumnName("coupon_discount");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.OptionalMail)
                    .HasColumnName("optional_mail")
                    .HasMaxLength(64);

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.OrderStatus)
                    .IsRequired()
                    .HasColumnName("order_status")
                    .HasMaxLength(32);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(32);

                entity.Property(e => e.PostalServiceId).HasColumnName("postal_service_id");

                entity.Property(e => e.ReceiverFirst)
                    .IsRequired()
                    .HasColumnName("receiver_first")
                    .HasMaxLength(32);

                entity.Property(e => e.ReceiverLast)
                    .IsRequired()
                    .HasColumnName("receiver_last")
                    .HasMaxLength(32);

                entity.Property(e => e.RequiredDate)
                    .HasColumnName("required_date")
                    .HasColumnType("date");

                entity.Property(e => e.ShippedDate)
                    .HasColumnName("shipped_date")
                    .HasColumnType("date");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("customer_order_customer_id_fkey");

                entity.HasOne(d => d.PostalService)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.PostalServiceId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("customer_order_postal_service_id_fkey");
            });

            modelBuilder.Entity<CustomerOrderAddress>(entity =>
            {
                entity.ToTable("customer_order_address");

                entity.HasIndex(e => e.OrderId)
                    .HasName("customer_order_address_order_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressTextPrimary)
                    .IsRequired()
                    .HasColumnName("address_text_primary");

                entity.Property(e => e.AddressTextSecondary).HasColumnName("address_text_secondary");

                entity.Property(e => e.CountryTag)
                    .IsRequired()
                    .HasColumnName("country_tag")
                    .HasMaxLength(4);

                entity.Property(e => e.DestinationZipCode)
                    .IsRequired()
                    .HasColumnName("destination_zip_code")
                    .HasMaxLength(12);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("district")
                    .HasMaxLength(64);

                entity.Property(e => e.Neighborhood)
                    .IsRequired()
                    .HasColumnName("neighborhood")
                    .HasMaxLength(64);

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnName("province")
                    .HasMaxLength(64);

                entity.HasOne(d => d.Order)
                    .WithOne(p => p.CustomerOrderAddress)
                    .HasForeignKey<CustomerOrderAddress>(d => d.OrderId)
                    .HasConstraintName("customer_order_address_order_id_fkey");
            });

            modelBuilder.Entity<CustomerOrderItem>(entity =>
            {
                entity.ToTable("customer_order_item");

                entity.Property(e => e.CustomerOrderItemId)
                    .HasColumnName("customer_order_item_id")
                    .HasDefaultValueSql("nextval('customer_order_item_seq'::regclass)");

                entity.Property(e => e.ListPrice).HasColumnName("list_price");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductDiscount).HasColumnName("product_discount");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.CustomerOrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("customer_order_item_order_id_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerOrderItem)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("customer_order_item_product_id_fkey");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.CustomerOrderItem)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("customer_order_item_store_id_fkey");
            });

            modelBuilder.Entity<CustomerReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("customer_review_pkey");

                entity.ToTable("customer_review");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("idx_customer_review_product_id");

                entity.Property(e => e.ReviewId)
                    .HasColumnName("review_id")
                    .HasDefaultValueSql("nextval('customer_review_seq'::regclass)");

                entity.Property(e => e.CustomerComment)
                    .IsRequired()
                    .HasColumnName("customer_comment");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.CustomerRating).HasColumnName("customer_rating");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ReplyReviewId).HasColumnName("reply_review_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerReview)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("customer_review_customer_id_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerReview)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("customer_review_product_id_fkey");
            });

            modelBuilder.Entity<PostalService>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("postal_service_pkey");

                entity.ToTable("postal_service");

                entity.Property(e => e.ServiceId)
                    .HasColumnName("service_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AvgDeliveryDay).HasColumnName("avg_delivery_day");

                entity.Property(e => e.Postalservicename)
                    .IsRequired()
                    .HasColumnName("postalservicename")
                    .HasMaxLength(32);

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.ListPrice)
                    .HasName("idx_product_list_price");

                entity.HasIndex(e => e.ProductName)
                    .HasName("idx_product_name");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasDefaultValueSql("nextval('product_seq'::regclass)");

                entity.Property(e => e.BoxPieces).HasColumnName("box_pieces");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.ListPrice).HasColumnName("list_price");

                entity.Property(e => e.Listed).HasColumnName("listed");

                entity.Property(e => e.ModelYear).HasColumnName("model_year");

                entity.Property(e => e.ProductColor)
                    .IsRequired()
                    .HasColumnName("product_color")
                    .HasMaxLength(16);

                entity.Property(e => e.ProductHeight).HasColumnName("product_height");

                entity.Property(e => e.ProductImage)
                    .IsRequired()
                    .HasColumnName("product_image");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(128);

                entity.Property(e => e.ProductWeight).HasColumnName("product_weight");

                entity.Property(e => e.ProductWidth).HasColumnName("product_width");

                entity.Property(e => e.SubType)
                    .IsRequired()
                    .HasColumnName("sub_type")
                    .HasMaxLength(16);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_brand_id_fkey");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_category_id_fkey");
            });

            modelBuilder.Entity<ProductSalesStatistic>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("product_sales_statistic_pkey");

                entity.ToTable("product_sales_statistic");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("date");

                entity.Property(e => e.NumberSold).HasColumnName("number_sold");

                entity.Property(e => e.NumberViewed).HasColumnName("number_viewed");

                entity.Property(e => e.ProductRating).HasColumnName("product_rating");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.ProductSalesStatistic)
                    .HasForeignKey<ProductSalesStatistic>(d => d.ProductId)
                    .HasConstraintName("product_sales_statistic_product_id_fkey");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("stock");

                entity.HasIndex(e => e.ProductId)
                    .HasName("idx_stock_product_id");

                entity.Property(e => e.StockId)
                    .HasColumnName("stock_id")
                    .HasDefaultValueSql("nextval('stock_seq'::regclass)");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("stock_product_id_fkey");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Stock)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("stock_store_id_fkey");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("store");

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .HasDefaultValueSql("nextval('store_seq'::regclass)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(64);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(48);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasColumnName("store_name")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<StoreAddress>(entity =>
            {
                entity.HasKey(e => e.StoreId)
                    .HasName("store_address_pkey");

                entity.ToTable("store_address");

                entity.HasIndex(e => e.ZipCode)
                    .HasName("idx_store_address_zip_code");

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressTextPrimary)
                    .IsRequired()
                    .HasColumnName("address_text_primary");

                entity.Property(e => e.AddressTextSecondary).HasColumnName("address_text_secondary");

                entity.Property(e => e.CountryTag)
                    .IsRequired()
                    .HasColumnName("country_tag")
                    .HasMaxLength(4);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasColumnName("district")
                    .HasMaxLength(64);

                entity.Property(e => e.Neighborhood)
                    .IsRequired()
                    .HasColumnName("neighborhood")
                    .HasMaxLength(64);

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnName("province")
                    .HasMaxLength(64);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasColumnName("zip_code")
                    .HasMaxLength(32);

                entity.HasOne(d => d.Store)
                    .WithOne(p => p.StoreAddress)
                    .HasForeignKey<StoreAddress>(d => d.StoreId)
                    .HasConstraintName("store_address_store_id_fkey");
            });

            modelBuilder.Entity<StoreSalesStatistic>(entity =>
            {
                entity.HasKey(e => e.StoreId)
                    .HasName("store_sales_statistic_pkey");

                entity.ToTable("store_sales_statistic");

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ItemSold).HasColumnName("item_sold");

                entity.HasOne(d => d.Store)
                    .WithOne(p => p.StoreSalesStatistic)
                    .HasForeignKey<StoreSalesStatistic>(d => d.StoreId)
                    .HasConstraintName("store_sales_statistic_store_id_fkey");
            });

            modelBuilder.HasSequence("account_seq");

            modelBuilder.HasSequence("brand_seq");

            modelBuilder.HasSequence("category_seq");

            modelBuilder.HasSequence("customer_basket_seq");

            modelBuilder.HasSequence("customer_order_item_seq");

            modelBuilder.HasSequence("customer_order_seq");

            modelBuilder.HasSequence("customer_review_seq");

            modelBuilder.HasSequence("customer_seq");

            modelBuilder.HasSequence("product_seq");

            modelBuilder.HasSequence("stock_seq");

            modelBuilder.HasSequence("store_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
