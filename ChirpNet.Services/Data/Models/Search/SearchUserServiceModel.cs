using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Models.Search
{
    public class SearchUserServiceModel
    {
        public string Id { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Bio { get; set; }
    }
}
