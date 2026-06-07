using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Services
{
    public class FollowService : IFollowService
    {
        private readonly ApplicationDbContext dbContext;
        public FollowService(ApplicationDbContext dbContext)
        {
         this.dbContext = dbContext;   
        }
        public async Task<bool> FollowAsync(string followerId, string followingId)
        {
           if(followingId == followerId)
            {
                return false;
            }
           bool followingUserExists = await this.dbContext
                .Users
                .AnyAsync(u => u.Id == followingId);

            if (!followingUserExists) 
            {
                return false;
            }
            bool alreadyFollow = await this.IsFollowingAsync(followerId, followingId);

            if (alreadyFollow) 
            {
                return false;
            }
            Follow follow = new Follow
            {

                FollowerId = followerId,
                FollowingId = followingId,
                CreatedOn = DateTime.UtcNow
            };
            await this.dbContext.Follows.AddAsync(follow);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            return await this.dbContext
                .Follows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        }

        public async Task<bool> UnfollowAsync(string followerId, string followingId)
        {
           Follow? follow = await this.dbContext
                .Follows
                .FirstOrDefaultAsync(f=> f.FollowerId == followerId && f.FollowingId == followingId);

            if(follow == null)
            {
                return false;
            }
             this.dbContext.Follows.Remove(follow);
            await this.dbContext .SaveChangesAsync();
            return true;
        }
    }
}
