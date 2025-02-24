using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites.Order_Agg;

namespace Talbat.Repository.Data.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(D=>D.Cost)
                .HasColumnType("decimal(18,2)");
            builder.Property(D => D.ShortName)
                   .HasColumnName("ShortName");
        }
    }
}
