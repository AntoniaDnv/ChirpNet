using ChirpNet.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChirpNet.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Feed()
        {
            var posts = await this.postService.GetPublicFeedAsync();    
            return View(posts);
        }
    }
}
