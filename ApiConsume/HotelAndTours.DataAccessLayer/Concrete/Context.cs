using EntityLayer.Concrete;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.DataAccessLayer.Concrete;

public class Context : IdentityDbContext<AppUser, AppRole, int>
{
    public virtual DbSet<AppRole> AppRoles { get; set; }
    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<Hotel> Hotels { get; set; }
    public virtual DbSet<Logs> Logs { get; set; }
    public virtual DbSet<RoomNumbers> RoomNumbers { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<RoomSpecs> RoomSpecs { get; set; }
    public virtual DbSet<RRSpecs> RRSpecs { get; set; }
    public virtual DbSet<HotelCategory> HotelCategories { get; set; }
    public virtual DbSet<RSHotelCategory> RSHotelCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RRSpecs>()
            .HasKey(rs => new { rs.RoomId, rs.RoomSpecsId });

        modelBuilder.Entity<RRSpecs>()
            .HasOne(rs => rs.Room)
            .WithMany(r => r.RoomSpecs)
            .HasForeignKey(rs => rs.RoomId);

        modelBuilder.Entity<RRSpecs>()
            .HasOne(rs => rs.RoomSpecs)
            .WithMany(rs => rs.RRSpecs)
            .HasForeignKey(rs => rs.RoomSpecsId);

        modelBuilder.Entity<RSHotelCategory>()
            .HasKey(hc => new { hc.HotelId, hc.HotelCategoryId });

        modelBuilder.Entity<RSHotelCategory>()
            .HasOne(hc => hc.Hotel)
            .WithMany(h => h.HotelCategories)
            .HasForeignKey(hc => hc.HotelId);

        modelBuilder.Entity<RSHotelCategory>()
            .HasOne(hc => hc.HotelCategory)
            .WithMany(c => c.Hotels)
            .HasForeignKey(hc => hc.HotelCategoryId);

      

        // Additional configurations for other entities and relationships

        base.OnModelCreating(modelBuilder);
    }
    /*
    protected override void OnModelCreating(ModelBuilder builder)
    {
        /*  builder.Entity<Room>()
              .HasMany<RoomNumbers>(x => x.RoomNumbers)
              .WithOne(x => x.Room)
              .HasForeignKey(x => x.RoomId);

       // builder.Entity<RoomSpecs>().HasKey(sc => new { sc.Rooms, sc.RoomId});


        base.OnModelCreating(builder);
    }
*/


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost\\SQLEXPRESS;Database=HotelAndTours; Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}