using ComputerHouse.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class InformationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutDeveloper()
        {
            return View(new ContactDeveloper());
        }
    }
}