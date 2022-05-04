using ComputerHouse.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComputerHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandCategory> BrandCategories { get; set; }
        public DbSet<OperatingSystem> OperatingSystems { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<ContactDeveloper> ContactDevelopers { get; set; }

        //To Delete Simply Remove From Here and Run Migration
        //public DbSet<HappyCustomer>HappyCustomers { get; set; }
        public DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public DbSet<CustomerContactUs> CustomerContactUs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Device>()
                .HasOne<Brand>(d => d.Brand)
                .WithMany()
                .HasForeignKey(d => d.BrandId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Device>()
                .HasOne<BrandCategory>(d => d.BrandCategory)
                .WithMany()
                .HasForeignKey(d => d.BrandCategoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            //DeleteBehavior.Restrict, 
            //when the Brand is deleted it restricts deleting its dependent (BrandCategory, Device)
            //So, we first need to delete the dependent and then only we can delete Brand.
            //DeleteBehavior.Cascade,
            //When Brand is deleted all of its dependent are deleted that contains its value
            //DeleteBehavior.SetNull
            //When Brand is Deleted its value in the dependent table will be null
            //DeleteBehavior.NoAction
            //Upon Deletion of Brand, no actions will be there in its value in the dependent entity
        }
    }
}
