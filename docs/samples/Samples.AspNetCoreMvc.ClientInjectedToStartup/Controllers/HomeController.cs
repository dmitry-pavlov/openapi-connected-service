using System.Diagnostics;
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

        public IActionResult Index()
        {
            TempData["SoldPetsCount"] = _petStoreClient.GetSoldPetsCount().Result;
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
