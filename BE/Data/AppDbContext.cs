using Microsoft.EntityFrameworkCore;
using BE.Entities;

namespace BE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<OrderTable> Orders => Set<OrderTable>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<ShippingInfo> Shippings => Set<ShippingInfo>();
    public DbSet<ReturnRequest> ReturnRequests => Set<ReturnRequest>();
    public DbSet<Bid> Bids => Set<Bid>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<SalesSummary> SalesSummaries => Set<SalesSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Store
        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Seller)
                  .WithOne(u => u.Store)
                  .HasForeignKey<Store>(e => e.SellerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Addresses)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Parent)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(e => e.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Seller)
                  .WithMany(u => u.Products)
                  .HasForeignKey(e => e.SellerId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Inventory
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product)
                  .WithOne(p => p.Inventory)
                  .HasForeignKey<Inventory>(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Coupon
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.ToTable("Coupon");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Coupons)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Seller)
                  .WithMany()
                  .HasForeignKey(e => e.SellerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderTable
        modelBuilder.Entity<OrderTable>(entity =>
        {
            entity.ToTable("OrderTable");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Buyer)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(e => e.BuyerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Coupon)
                  .WithMany()
                  .HasForeignKey(e => e.CouponId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                  .WithOne(o => o.Payment)
                  .HasForeignKey<Payment>(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ShippingInfo
        modelBuilder.Entity<ShippingInfo>(entity =>
        {
            entity.ToTable("ShippingInfo");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                  .WithOne(o => o.ShippingInfo)
                  .HasForeignKey<ShippingInfo>(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ReturnRequest
        modelBuilder.Entity<ReturnRequest>(entity =>
        {
            entity.ToTable("ReturnRequest");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.ReturnRequests)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Bid
        modelBuilder.Entity<Bid>(entity =>
        {
            entity.ToTable("Bid");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Bids)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Bidder)
                  .WithMany(u => u.Bids)
                  .HasForeignKey(e => e.BidderId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Review
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Buyer)
                  .WithMany(u => u.Reviews)
                  .HasForeignKey(e => e.BuyerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Feedback
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Seller)
                  .WithOne(u => u.Feedback)
                  .HasForeignKey<Feedback>(e => e.SellerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Dispute
        modelBuilder.Entity<Dispute>(entity =>
        {
            entity.ToTable("Dispute");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.Disputes)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ReturnRequest)
                  .WithMany(r => r.Disputes)
                  .HasForeignKey(e => e.ReturnRequestId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Buyer)
                  .WithMany()
                  .HasForeignKey(e => e.BuyerId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Seller)
                  .WithMany()
                  .HasForeignKey(e => e.SellerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Message
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Sender)
                  .WithMany()
                  .HasForeignKey(e => e.SenderId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Receiver)
                  .WithMany()
                  .HasForeignKey(e => e.ReceiverId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // SalesSummary
        modelBuilder.Entity<SalesSummary>(entity =>
        {
            entity.ToTable("SalesSummary");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Seller)
                  .WithMany()
                  .HasForeignKey(e => e.SellerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
