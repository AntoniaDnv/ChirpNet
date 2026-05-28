using ChirpNet.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Data.Data.Configurations
{
    public class PostConfiguration:IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasOne(p=> p.Author)
                .WithMany(a=> a.Posts)
                .HasForeignKey(p=>p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
