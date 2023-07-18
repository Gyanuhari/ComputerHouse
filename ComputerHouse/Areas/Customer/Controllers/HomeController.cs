using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeIndexViewModel indexVM = new HomeIndexViewModel()
            {
                EmailSubscription = new EmailSubscription(),
                NewProductList = await _context.Devices
                .Where(d => d.IsNew == true)
                .Take(3)
                .ToListAsync()
            };

            return View(indexVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailSubscription([Bind("Email")] EmailSubscription emailSubscription)
        {
            if (ModelState.IsValid)
            {
                var emailExists = await _context.EmailSubscriptions
                    .AnyAsync(e => e.Email.ToLower().Trim().Equals(emailSubscription.Email.ToLower().Trim()));

                if (!emailExists)
                {
                    await _context.EmailSubscriptions.AddAsync(emailSubscription);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        //Privacy
        public IActionResult Privacy()
        {
            return View();
        }


        //Get: Customer/Home/Contact
        [HttpGet]
        public IActionResult Contact()
        {
            CustomerContactUs contactModel = new CustomerContactUs();

            return View(new CustomerContactUs());
        }

        //Post: Customer/Home/Contact
        [HttpPost]
        [ActionName("Contact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactPost([Bind("FullName, Email, PhoneNumber, Location, Message")] CustomerContactUs model)
        {
            if(ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                    //Linking the registered information of user who send contact message. 
                    model.ApplicationUserId = claim.Value;
                }
                await _context.CustomerContactUs.AddAsync(model);

                //Storing the emails for subscription if it is not present
                var doesEmailExists = await _context.EmailSubscriptions.AnyAsync(e => e.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()));
                if(!doesEmailExists)
                {
                    EmailSubscription email = new EmailSubscription(model.Email);
                    //Replace By Constructor
                    //email.Email = model.Email;
                    await _context.EmailSubscriptions.AddAsync(email);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Contact));
            }

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
