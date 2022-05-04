using ComputerHouse.Data;
using ComputerHouse.Extensions;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string searchName)
        {
            //var brandList = await _dbContext.Brands.ToListAsync();
            IQueryable<Device> query = _dbContext.Devices
                                       .Include(d => d.Brand)
                                       .Include(d => d.BrandCategory)
                                       .Include(d => d.OperatingSystem);

            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(d => d.Name.ToLower().Trim().Contains(searchName.ToLower().Trim()));

            }

            //TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            ShoppingListVM shoppingList = new ShoppingListVM
            {
                DeviceList = await query.ToListAsync(),
                //SearchName = string.IsNullOrEmpty(searchName) ? "" : textInfo.ToTitleCase(searchName)
                SearchName = string.IsNullOrEmpty(searchName) ? "" : searchName
            };

            return View(shoppingList);
        }

        //Adding devices to CartItems and Session
        public async Task<IActionResult> AddToCart(int id)
        {
            List<int> sessionList = new List<int>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                //Adding the device to cartItem
                var device = await _dbContext.Devices.Where(d => d.Id == id).FirstOrDefaultAsync();
                CartItem cartItem = new CartItem
                {
                    DeviceId = id,
                    Device = device
                };

                await _dbContext.CartItems.AddAsync(cartItem);
                await _dbContext.SaveChangesAsync();

                //Adding CartItem Id to session, so that when there are thousands of customers each customer will get their own cartItem
                sessionList.Add(cartItem.Id);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                //To store devicelist for all session
                List<int> deviceList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (var sessionId in sessionList)
                {
                    var deviceId = await _dbContext.CartItems.Where(c => c.Id == sessionId).Select(c => c.DeviceId).FirstOrDefaultAsync();
                    deviceList.Add(deviceId);
                }

                //if devices in session does not contains this device then create one cartItem, and update the session
                if (!deviceList.Contains(id))
                {
                    var device = await _dbContext.Devices.Where(d => d.Id == id).FirstOrDefaultAsync();
                    CartItem cartItem = new CartItem
                    {
                        DeviceId = id,
                        Device = device,
                        ItemCount = 1
                    };

                    await _dbContext.CartItems.AddAsync(cartItem);
                    await _dbContext.SaveChangesAsync();

                    sessionList.Add(cartItem.Id);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }

            //If CartItem was not used, or say no any increment/decrement in cart item was required.
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            //{
            //    sessionList.Add(id);
            //    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            //}
            //else
            //{
            //    //Check does cartmenu item contains item with session id, if yes then again check if that cart contains 
            //    device id if not create cartitem and assign cartid to session
            //    sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            //    if (!sessionList.Contains(id))
            //    {
            //        sessionList.Add(id);
            //        HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            //    }
            //}

            //return RedirectToAction(nameof(Index));

            //It is better to redirect to cart
            return RedirectToAction(nameof(Index), "Carts", "Customer");
        }

        //Get: customer/shoppings/detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var device = await _dbContext.Devices
                .Include(d => d.Brand)
                .Include(d => d.BrandCategory)
                .Include(d => d.OperatingSystem)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (device == null)
                return NotFound("Sorry! No Item Found.");


            return View(device);
        }

        //Add to WishList
        [Authorize]
        public async Task<IActionResult> AddToWishList(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var isInWishlist = await _dbContext.WishLists
                .AnyAsync(w => w.DeviceId == id && w.UserId == claim.Value);

            if (!isInWishlist)
            {
                WishList wishList = new WishList
                {
                    DeviceId = id,
                    UserId = claim.Value
                };

                await _dbContext.WishLists.AddAsync(wishList);
                await _dbContext.SaveChangesAsync();
            }

            //It is better to redirect to wishlist
            return RedirectToAction(nameof(Index), "Wishlists", "Customer");
        }
    }
}