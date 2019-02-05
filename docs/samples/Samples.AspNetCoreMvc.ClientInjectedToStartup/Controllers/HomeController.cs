using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Samples.AspNetCoreMvc.ClientInjectedToStartup.Models;
using Samples.AspNetCoreMvc.ClientInjectedToStartup.PetStore;

namespace Samples.AspNetCoreMvc.ClientInjectedToStartup.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPetStoreClient _petStoreClient;

        public HomeController(IPetStoreClient petStoreClient)
        {
            _petStoreClient = petStoreClient;
        }

        public async Task<IActionResult> Index()
        {
            TempData["SoldPetsCount"] = await _petStoreClient.GetSoldPetsCountAsync();
            return View();
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
