using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WorkFlowHR.Domain.Core.BaseEntityConfigurations;
using WorkFlowHR.Domain.Entities;

namespace WorkFlowHR.Infrastructure.Configurations
{
    public class AdvanceConfiguration:AuditableEntityConfiguraton<Advance>
    {
        public override void Configure(EntityTypeBuilder<Advance> builder)
        {
            builder.Property(a => a.Amount).IsRequired();
            builder.Property(a => a.AdvanceDate).IsRequired();
            builder.Property(a => a.Image).IsRequired(false);
            base.Configure(builder);
        }
    }
    
       
    
}
