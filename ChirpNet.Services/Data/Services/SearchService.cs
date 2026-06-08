using ChirpNet.Data.Data;
using ChirpNet.Services.Data.Interfaces;
using ChirpNet.Services.Data.Models.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext dbContext;
        public SearchService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<SearchResultsServiceModel> SearchAsync(string? keyword)
        {
          SearchResultsServiceModel model = new SearchResultsServiceModel
          {
              Keyword = keyword
          };

            if (string.IsNullOrWhiteSpace(keyword)) 
            {
                return model;
            }
            string normalizedKeyword = keyword.Trim().ToLower();

            model.Posts = await this.dbContext.Posts
                .AsNoTracking()
                .Where(p=> !p.IsDeleted && 
                p.Content.ToLower().Contains(normalizedKeyword))
                .OrderByDescending(p=> p.CreatedOn)
                .Take(20)
                .Select(p => new SearchPostServiceModel{
                    Id = p.Id,
                    Content = p.Content,
                    AuthorId = p.AuthorId,
                    AuthorDisplayName = p.Author.DisplayName ?? p.Author.UserName!,
                    CreatedOn = p.CreatedOn
                }).ToListAsync();

            model.Users = await this.dbContext.Users
                .AsNoTracking()
                .Where(u =>
                (u.DisplayName != null && u.DisplayName.ToLower().Contains(normalizedKeyword)) ||
                (u.UserName != null && u.UserName.ToLower().Contains(normalizedKeyword)) ||
                (u.Email != null && u.Email.ToLower().Contains(normalizedKeyword)))
            .OrderBy(u => u.DisplayName ?? u.UserName)
            .Take(20)
            .Select(u => new SearchUserServiceModel
            {
                Id = u.Id,
                DisplayName = u.DisplayName ?? u.UserName!,
                Bio = u.Bio
            })
            .ToListAsync();

            return model;
        }
    }
}
