using ChirpNet.Data.Common;
using ChirpNet.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChirpNet.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = ApplicationConstants.AdministratorRoleName)]
public class DashboardController : Controller
{
    private readonly IAdminService adminService;

    public DashboardController(IAdminService adminService)
    {
        this.adminService = adminService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await this.adminService.GetDashboardAsync();

        return View(model);
    }
}