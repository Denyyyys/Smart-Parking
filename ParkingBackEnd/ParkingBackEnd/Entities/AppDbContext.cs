using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ParkingBackEnd.Entities
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<ParkingAdmin> ParkingAdmins { get; set; }
        public DbSet<ParkingAndDrivers> ParkingAndDrivers { get; set; }
        public DbSet<ParkingHistory> ParkingHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parking>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Parking>()
                .Property(p => p.NumberParkingSpaces)
                .IsRequired();

            modelBuilder.Entity<Parking>()
                .HasOne(p => p.ParkingAdmin)
                .WithOne()
                .HasForeignKey<Parking>(p => p.ParkingAdminId);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .HasMaxLength(50);

            modelBuilder.Entity<Parking>()
                .HasOne(p => p.Address)
                .WithOne()
                .HasForeignKey<Parking>(p => p.AddressId);

            modelBuilder.Entity<Driver>()
                .HasIndex(v => v.LicensePlateNumber)
                .IsUnique();


            modelBuilder.Entity<ParkingAndDrivers>()
                .HasKey(pd => new { pd.DriverId, pd.ParkingId });

            modelBuilder.Entity<ParkingAndDrivers>()
                .HasOne(pd => pd.Driver);

            modelBuilder.Entity<ParkingHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EntryDate).IsRequired();
                entity.Property(e => e.ExitDate);

                entity.HasOne(e => e.Driver)
                    .WithMany(d => d.ParkingHistory)
                    .HasForeignKey(e => e.DriverId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Parking)
                    .WithMany(p => p.ParkingHistory)
                    .HasForeignKey(e => e.ParkingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //modelBuilder.Entity<ParkingAndDrivers>()
            //    .HasOne(pd => pd.DriverId)    
            //    .WithOne()
            //    .HasForeignKey<ParkingAndDrivers>(pd => pd.UserId);

            //modelBuilder.Entity<ParkingAndDrivers>()
            //    .HasMany()
            //    .HasForeignKey<ParkingAndDrivers>(pd => pd.ParkingId);

            //modelBuilder.Entity<ParkingAdmin>()
            //    .HasOne(pa => pa.Parking)
            //    .WithOne()
            //    .HasForeignKey<ParkingAdmin>(pa => pa.ParkingId);

            /*modelBuilder.Entity<ParkingAdmin>()
                .HasOne(pa => pa.Parking)
                .WithOne();
                
            */

            /*modelBuilder.Entity<ParkingToAdmin>()
                .HasKey(pa => new { pa.ParkingId, pa.AdminId });

            modelBuilder.Entity<ParkingToAdmin>()
                .HasOne(pa => pa.Parking)
                .WithOne()
                .HasForeignKey<ParkingToAdmin>(pa => pa.AdminId);
*/

            /*
                        modelBuilder.Entity<ParkingToAdmin>()
            .HasOne(pa => pa.ParkingId)
                            .HasOne(p => p.ParkingToAdmin)
                            .HasForeignKey(pa => pa.ParkingId);
            */
            /*modelBuilder.Entity<ParkingToAdmin>()
                .HasOne(pa => pa.ParkingAdmin)
                .WithMany(a => a.)
                .HasForeignKey(pa => pa.ParkingAdminId);
*/
            //.OnDelete(DeleteBehavior.SetNull); 
            //modelBuilder.Entity<User>().UseTpcMappingStrategy();
        }
    }
}
