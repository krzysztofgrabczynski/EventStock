using EventStock.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventStock.Infrastructure
{
    public class Context : IdentityDbContext<User>
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EventEquipment> EventEquipments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Equipment>()
                .HasMany(eq => eq.EventEquipments)
                .WithOne(eeq => eeq.Equipment)
                .HasForeignKey(eeq => eeq.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasMany(e => e.EventEquipments)
                .WithOne(eeq => eeq.Event)
                .HasForeignKey(eeq => eeq.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
