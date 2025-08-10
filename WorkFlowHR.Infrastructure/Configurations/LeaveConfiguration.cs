
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHR.Domain.Core.BaseEntityConfigurations;
using WorkFlowHR.Domain.Entities;


namespace WorkFlowHR.Infrastructure.Configurations
{
    public class LeaveConfiguration : AuditableEntityConfiguraton<Leave>
    {
        public override void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            base.Configure(builder);
        }

    }
}
