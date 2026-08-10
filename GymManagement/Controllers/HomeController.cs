using System.Diagnostics;
using System.Threading.Tasks;
using GymManagement.BLL.Interface;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalytics _analytics;

        public HomeController(ILogger<HomeController> logger,IAnalytics analytics)
        {
            _logger = logger;
            _analytics = analytics;
        }

        public async Task<IActionResult> Index( CancellationToken ct)
        {
            var model = await _analytics.AnalitcAsync(ct);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
