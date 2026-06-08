using ChirpNet.Data.Common;
using ChirpNet.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChirpNet.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ApplicationConstants.AdministratorRoleName)]
public class PostsController : Controller
{
    private readonly IAdminService adminService;

    public PostsController(IAdminService adminService)
    {
        this.adminService = adminService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var posts = await this.adminService.GetAllPostsAsync();

        return View(posts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await this.adminService.DeletePostAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Post was deleted by administrator.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        bool restored = await this.adminService.RestorePostAsync(id);

        if (!restored)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Post was restored by administrator.";

        return RedirectToAction(nameof(Index));
    }
}