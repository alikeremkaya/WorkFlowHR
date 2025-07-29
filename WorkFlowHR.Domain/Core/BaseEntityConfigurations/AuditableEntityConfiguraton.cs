using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.Base;

namespace WorkFlowHR.Domain.Core.BaseEntityConfigurations
{
    public class AuditableEntityConfiguraton<TEntity> : BaseEntityConfiguration<TEntity> where TEntity : AuditableEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.DeletedBy).IsRequired(false);
            builder.Property(e => e.DeletedDate).IsRequired(false);
            base.Configure(builder);
        }
    }
}
