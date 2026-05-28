using ChirpNet.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Data.Data.Configurations
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder
          .HasOne(f => f.Follower)
          .WithMany(u => u.Following)
          .HasForeignKey(f => f.FollowerId)
          .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(f => new { f.FollowerId, f.FollowingId })
                .IsUnique();
        }
    }
}
