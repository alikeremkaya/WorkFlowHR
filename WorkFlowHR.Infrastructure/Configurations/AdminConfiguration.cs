using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.BaseEntityConfigurations;
using WorkFlowHR.Domain.Entities;

namespace WorkFlowHR.Infrastructure.Configurations
{
    public class AdminConfiguration : BaseUserConfiguration<Admin>
    {
        public override void Configure(EntityTypeBuilder<Admin> builder)
        {
            base.Configure(builder);
        }
    }
}
