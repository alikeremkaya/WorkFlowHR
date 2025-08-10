using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHR.Domain.Core.BaseEntityConfigurations;
using WorkFlowHR.Domain.Entities;

namespace WorkFlowHR.Infrastructure.Configurations
{
    public class ExpenseConfiguration : AuditableEntityConfiguraton<Expense>
    {
        public override void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(e => e.Amount).IsRequired();
            builder.Property(e => e.Image).IsRequired(false);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.ExpenseDate).IsRequired();


            base.Configure(builder);
        }
    }
}
