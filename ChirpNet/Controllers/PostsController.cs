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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var post = await this.postService.GetDetailsAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(PostCommentInputModel inputModel)
        {
            if (inputModel.PostId <= 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Comment could not be added. Please check the content.";
                return RedirectToAction(nameof(Details), new { id = inputModel.PostId });
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            bool commentAdded = await this.postService.AddCommentAsync(inputModel, userId);

            if (!commentAdded)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Your comment was added successfully.";

            return RedirectToAction(nameof(Details), new { id = inputModel.PostId });
        }
    }
}
