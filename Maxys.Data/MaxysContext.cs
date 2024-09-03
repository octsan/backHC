using ERP_MaxysHC.Maxys.Model;
using Microsoft.EntityFrameworkCore;

namespace ERP_MaxysHC.Maxys.Data
{
    public class MaxysContext : DbContext
    {
        public MaxysContext(DbContextOptions<MaxysContext> options) : base(options)
        {
        } 
        public DbSet<adCarint_admClientes> admClientes { get; set; } //Se agrega la tabla

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<adCarint_admClientes>().ToTable("admClientes"); //Se agrega la tabla
        }
    }
}
