using ChirpNet.Services.Data.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDetailsServiceModel?> GetProfileDetailsAsync(string userId);
        Task<IEnumerable<ProfilePostServiceModel>> GetMyPostsAsync(string userId);
    }
}
