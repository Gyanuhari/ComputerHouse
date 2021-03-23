using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class WishListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pageSize = 4;

        public WishListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get: List of Wishlist Items
        [Authorize]
        public async Task<IActionResult> Index(int productPage=1)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var wishList = await _context.WishLists
                .Include(w => w.Device)
                .Where(w => w.UserId == claim.Value)
                .OrderBy(w=>w.Device.Name)
                .ToListAsync();

            //PagingInfo pagingInfo = new PagingInfo()
            //{
            //    CurrentPage = productPage,
            //    ItemsPerPage = _pageSize,
            //    TotalItems = wishList.Count
            //};

            //An example to show student a clean code that will be done during code refactoring
            PagingInfo pagingInfo = new PagingInfo(wishList.Count, _pageSize, productPage);

            WishlistPaginationVM wishlistPaginationVM = new WishlistPaginationVM
            {
                PagingInfo=pagingInfo,
                WishLists = wishList.Skip((productPage - 1) * _pageSize).Take(_pageSize).ToList()
            };


            return View(wishlistPaginationVM);
        }

        //Remove Items From Wishlist
        [Authorize]
        public async Task<IActionResult> Remove(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var wishList = await _context.WishLists
                .Where(w => w.UserId == claim.Value && w.DeviceId==id)
                .FirstOrDefaultAsync();

            if(wishList==null)
            {
                return NotFound();
            }

            _context.WishLists.Remove(wishList);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}