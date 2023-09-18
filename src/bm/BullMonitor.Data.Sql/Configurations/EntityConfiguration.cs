using BullMonitor.Data.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BullMonitor.Data.Sql.Configurations
{
    public class EntityConfiguration<T>
        : IEntityTypeConfiguration<T>
        where T : class, IIdCode
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired();

            //builder
            //    .Property(x => x.CreatedOn)
            //    .IsRequired()
            //    .HasDefaultValueSql("GETDATE()");

            builder
                .HasIndex(x => new { x.Id })
                .IsUnique();
        }
    }
}