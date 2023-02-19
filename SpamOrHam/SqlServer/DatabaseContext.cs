using Microsoft.EntityFrameworkCore;
using SpamOrHam.Models;

namespace SpamOrHam.SqlServer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<DataPoint> DataPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dataset>(o =>
            {
                o.HasKey(x => x.Id);
                o.HasMany(x => x.DataPoints).WithOne(x => x.Dataset).HasForeignKey(x => x.DatasetId);
                o.Property(x => x.HamCount).HasDefaultValue(0);
                o.Property(x => x.SpamCount).HasDefaultValue(0);
            });

            modelBuilder.Entity<DataPoint>(o =>
            {
                o.HasKey(x => x.Id);
                o.HasOne(x => x.Dataset).WithMany(x => x.DataPoints);
                o.Property(x => x.TimesOccurredInHam).HasDefaultValue(0);
                o.Property(x => x.TimesOccurredInSpam).HasDefaultValue(0);
            });
        }
    }
}
