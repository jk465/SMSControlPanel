using Microsoft.AspNetCore.Mvc;
using SMSControlPanel.Repository;
using System.Threading.Tasks;

namespace SMSControlPanel.Controllers
{
    public class BaseController : Controller
    {
        private readonly IAuthService _authService;
        public BaseController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Initial()
        {
            _authService.LogUser();
            await _authService.SetSessionValues();
            return RedirectToAction("Index","SendSmsText");
        }

        public IActionResult Unauthorised()
        {
            return View();
        }
    }
}
