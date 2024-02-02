using BullMonitor.Data.Storage.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BullMonitor.Data.Sql.Configurations
{
    public class EntityCodeConfiguration<T>
        : EntityConfiguration<T>
        where T : class, IIdCode
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Code)
                .IsRequired();

            builder
                .HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}