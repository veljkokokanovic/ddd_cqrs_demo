using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Order.ProcessManager.Sagas.Persistence
{
    public class SagaInstanceMap : IEntityTypeConfiguration<SagaInstance>
    {
        public void Configure(EntityTypeBuilder<SagaInstance> builder)
        {
            builder.Property(x => x.CurrentState).HasMaxLength(20).IsRequired();
            builder.Property(x => x.CorrelationId);
            builder.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
