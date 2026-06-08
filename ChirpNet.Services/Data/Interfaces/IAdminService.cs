using ChirpNet.Services.Data.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardServiceModel> GetDashboardAsync();

        Task<IEnumerable<AdminPostServiceModel>> GetAllPostsAsync();

        Task<bool> DeletePostAsync(int postId);

        Task<bool> RestorePostAsync(int postId);
    }
}
