using ChirpNet.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChirpNet.Web.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly IProfileService profileService;
        private readonly IFollowService followService;

        public ProfilesController(
            IProfileService profileService,
            IFollowService followService)
        {
            this.profileService = profileService;
            this.followService = followService;
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profile = await this.profileService.GetProfileDetailsAsync(id, currentUserId);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> MyPosts() 
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
            {

                return Unauthorized();
            }

            var posts = await this.profileService.GetMyPostsAsync(userId);
            return View(posts);
        
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Follow(string id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            bool followed = await this.followService.FollowAsync(currentUserId, id);

            if (!followed)
            {
                TempData["ErrorMessage"] = "You could not follow this user.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["SuccessMessage"] = "You are now following this user.";

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unfollow(string id)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            bool unfollowed = await this.followService.UnfollowAsync(currentUserId, id);

            if (!unfollowed)
            {
                TempData["ErrorMessage"] = "You could not unfollow this user.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["SuccessMessage"] = "You unfollowed this user.";

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
