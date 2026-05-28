using ChirpNet.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Data.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c=>c.Author)
                .WithMany(a=> a.Comments)
                .HasForeignKey(c=>c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
