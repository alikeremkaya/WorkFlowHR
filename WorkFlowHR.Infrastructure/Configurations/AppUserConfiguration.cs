using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHR.Domain.Core.BaseEntityConfigurations;
using WorkFlowHR.Domain.Entities;

namespace WorkFlowHR.Infrastructure.Configurations;

public class AppUserConfiguration:BaseUserConfiguration<AppUser>
{
    public override void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(u => u.AzureAdObjectId).IsUnique();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.AzureAdObjectId).IsRequired();

        base.Configure(builder);

    }


}
