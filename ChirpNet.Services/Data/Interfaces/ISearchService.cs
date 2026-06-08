using ChirpNet.Services.Data.Models.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResultsServiceModel> SearchAsync(string? keyword);

    }
}
