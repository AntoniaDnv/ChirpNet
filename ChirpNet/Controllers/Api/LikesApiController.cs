using System.Security.Claims;
using ChirpNet.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChirpNet.Web.Controllers.Api;

[ApiController]
[Route("api/likes")]
public class LikesApiController : ControllerBase
{
    private readonly ILikeService likeService;

    public LikesApiController(ILikeService likeService)
    {
        this.likeService = likeService;
    }

    [HttpPost("toggle/{postId:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int postId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await this.likeService.ToggleLikeAsync(postId, userId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            isLiked = result.IsLiked,
            likesCount = result.LikesCount
        });
    }
}