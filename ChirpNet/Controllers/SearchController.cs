using ChirpNet.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChirpNet.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        public SearchController(ISearchService searchService)
        {
         this.searchService = searchService;   
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? keyword)
        {
            var model = await this.searchService.SearchAsync(keyword);
            return View(model);
        }
    }
}
