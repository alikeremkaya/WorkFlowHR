using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlowHR.Domain.Core.Base;

namespace WorkFlowHR.Domain.Core.BaseEntityConfigurations
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {/// <summary>
     /// BaseEntity yapılandırmasını sağlar.
     /// </summary>
     /// <param name="builder">Entity türü yapılandırıcısı</param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.UpdatedBy).IsRequired(false);
            builder.Property(e => e.UpdatedDate).IsRequired(false);
        }
    }
}
