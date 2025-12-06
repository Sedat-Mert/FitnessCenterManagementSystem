using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sedatodev1.Models;

namespace sedatodev1.Data
{
    public class UygulamaDbContext : IdentityDbContext<Uye>
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }
        public DbSet<AntrenorMusaitlik> AntrenorMusaitlikler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------------------
            // MANY-TO-MANY: Antrenör <-> Hizmet
            // ------------------------------
            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Antrenor)
                .WithMany(a => a.AntrenorHizmetler)
                .HasForeignKey(ah => ah.AntrenorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Hizmet)
                .WithMany(h => h.AntrenorHizmetler)
                .HasForeignKey(ah => ah.HizmetId)
                .OnDelete(DeleteBehavior.NoAction);

            // ------------------------------
            // ONE-TO-MANY: Randevu -> Hizmet
            // ------------------------------
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany(h => h.Randevular)
                .HasForeignKey(r => r.HizmetId)
                .OnDelete(DeleteBehavior.NoAction);

            // ------------------------------
            // ONE-TO-MANY: Randevu -> Antrenör
            // ------------------------------
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany(a => a.Randevular)
                .HasForeignKey(r => r.AntrenorId)
                .OnDelete(DeleteBehavior.NoAction);

            // ------------------------------
            // ONE-TO-MANY: Randevu -> Uye (Identity User)
            // ------------------------------
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Uye)
                .WithMany(u => u.Randevular)
                .HasForeignKey(r => r.UyeId)
                .OnDelete(DeleteBehavior.NoAction);

            // ------------------------------
            // ONE-TO-MANY: Antrenor -> Salon
            // ------------------------------
            modelBuilder.Entity<Antrenor>()
                .HasOne(a => a.Salon)
                .WithMany(s => s.Antrenorler)
                .HasForeignKey(a => a.SalonId)
                .OnDelete(DeleteBehavior.NoAction);

            // ------------------------------
            // ONE-TO-MANY: Hizmet -> Salon
            // ------------------------------
            modelBuilder.Entity<Hizmet>()
                .HasOne(h => h.Salon)
                .WithMany(s => s.Hizmetler)
                .HasForeignKey(h => h.SalonId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
