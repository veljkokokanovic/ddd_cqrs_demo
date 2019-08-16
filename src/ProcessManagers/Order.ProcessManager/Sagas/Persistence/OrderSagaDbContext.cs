using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Order.ProcessManager.Sagas.Persistence
{
    public class OrderSagaDbContext : SagaDbContext<SagaInstance, SagaInstanceMap>
    {
        public OrderSagaDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SagaInstance>().ToTable("SagaInstances");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SagaInstance> SagaInstances { get; set; }
    }
}
