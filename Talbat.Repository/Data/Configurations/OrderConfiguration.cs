﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites.Order_Agg;

namespace Talbat.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O=>O.Status)
                .HasConversion(Ostatus=>Ostatus.ToString(),Ostatus=>(OrderStatus)Enum.Parse(typeof(OrderStatus),Ostatus));
            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(18,2)");
            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());
            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
