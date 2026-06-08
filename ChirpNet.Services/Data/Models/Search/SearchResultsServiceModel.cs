using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Models.Search
{
    public class SearchResultsServiceModel
    {
        public string? Keyword { get; set; }

        public IEnumerable<SearchPostServiceModel> Posts { get; set; }
            = new List<SearchPostServiceModel>();

        public IEnumerable<SearchUserServiceModel> Users { get; set; }
            = new List<SearchUserServiceModel>();
    }
}
