
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHR.Domain.Core.BaseEntityConfigurations;
using WorkFlowHR.Domain.Entities;

namespace WorkFlowHR.Infrastructure.Configurations
{
    public class LeaveTypeConfiguration : AuditableEntityConfiguraton<LeaveType>
    {
        public override void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.ToTable("LeaveTypes");
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
            base.Configure(builder);
        }
    }
}
