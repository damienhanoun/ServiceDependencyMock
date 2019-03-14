using Microsoft.EntityFrameworkCore;

namespace DatabasesObjects.SqlServer
{
    public class MockStrategiesContext : DbContext
    {

        public MockStrategiesContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MockStrategy>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MethodId).IsRequired();
                entity.Property(e => e.SerializedStrategy).IsRequired();
                entity.Property(e => e.CreationDate).HasColumnType("datetime").IsRequired();
            });
        }

        public virtual DbSet<MockStrategy> MockStrategy { get; set; }
    }
}
