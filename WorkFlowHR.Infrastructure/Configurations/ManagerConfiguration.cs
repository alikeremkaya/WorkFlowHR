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
    public class ManagerConfiguration : BaseUserConfiguration<Manager>
    {
        public override void Configure(EntityTypeBuilder<Manager> builder)
        {
            base.Configure(builder);

        }
    }
}
