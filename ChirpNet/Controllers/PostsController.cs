using ChirpNet.Services.Data.Interfaces;
using ChirpNet.Services.Data.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
            {
                return Unauthorized();
            }
            await this.postService.CreateAsync(model, userId);
           
            //because the browser makes a new request and the meassage survives it 
            TempData["SuccessMessage"] = "Your post was created successfully.";

          return  RedirectToAction(nameof(Feed));
        }
    }
}
