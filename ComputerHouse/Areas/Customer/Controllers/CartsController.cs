using ComputerHouse.Data;
using ComputerHouse.Extensions;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using ComputerHouse.Services.Background;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        [BindProperty]
        public CartItemAndOrderVM CartItemOrderVM { get; set; }

        public CartsController(ApplicationDbContext context, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _context = context;
            _backgroundTaskQueue = backgroundTaskQueue;
            CartItemOrderVM = new CartItemAndOrderVM
            {
                OrderHeader=new OrderHeader()
            };
        }

        //Get: customer/carts/Index
        public async Task<IActionResult> Index()
        {
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                //For storing session List
                List<int> sessionList = new List<int>();

                //For storing cart items to send to View
                List<CartItem> cartItems = new List<CartItem>();

                sessionList= HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                //_backgroundTaskQueue.QueueBackgroundWorkItem(async (serviceScopeFactory, token) => 
                //{
                //    using(var scope = serviceScopeFactory.CreateScope())
                //    {
                //        var service = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //    }
                //});

                foreach(var id in sessionList)
                {
                    var itemFromCart = await _context.CartItems
                        .Include(c => c.Device)
                        .Where(c=>c.Id==id)
                        .FirstOrDefaultAsync();

                    if(itemFromCart!=null)
                    {
                        cartItems.Add(itemFromCart);
                    }
                }

                return View(cartItems);
            }
            
            return View();
        }

        //Customer/Carts/IncreaseItem/5
        public async Task<IActionResult> IncreaseItem(int id)
        {
            var itemFromCart = await _context.CartItems
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if(itemFromCart!=null)
            {
                itemFromCart.ItemCount += 1;
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound("Item Not Found");
            }

            return RedirectToAction(nameof(Index));
        }

        //Customer/Carts/DecreaseItem/5
        public async Task<IActionResult> DecreaseItem(int id)
        {
            var itemFromCart = await _context.CartItems
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (itemFromCart != null)
            {
                if(itemFromCart.ItemCount>1)
                {
                    itemFromCart.ItemCount -= 1;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                return NotFound("Item Not Found");
            }

            return RedirectToAction(nameof(Index));
        }

        //Customer/Carts/RemoveItem/5
        public async Task<IActionResult> RemoveItem(int id)
        {
            var itemFromCart = await _context.CartItems
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (itemFromCart != null)
            {
                //Remove From Session
                var sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                if(sessionList.Contains(itemFromCart.Id))
                {
                    sessionList.Remove(itemFromCart.Id);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }

                //Remove From CartItems
                _context.CartItems.Remove(itemFromCart);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound("Item Not Found");
            }

            return RedirectToAction(nameof(Index));
        }

        //Customer/Carts/Checkout
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                CartItemOrderVM.CartItemList = new List<CartItem>();

                //Getting those cartItem in session
                foreach (var sessionId in sessionList)
                {
                    var cartItem = await _context.CartItems
                        .Include(c=>c.Device)
                        .Where(c => c.Id == sessionId)
                        .FirstOrDefaultAsync();

                    if(cartItem!=null)
                    {
                        CartItemOrderVM.CartItemList.Add(cartItem);
                    }
                }
                return View(CartItemOrderVM);
            }

            //Sending Null Value, if the Session is null or empty
            return View();
        }

        [Authorize]
        [HttpPost]
        [ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutPost()
        {
            if(ModelState.IsValid)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                
                if(sessionList!=null && sessionList.Count()>0)
                {
                    var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                    //I kept this section inside this enclosure because I do not want to save orderHeader if session is empty or expired
                    OrderHeader orderHeader = CartItemOrderVM.OrderHeader;
                    orderHeader.OrderDate = DateTime.Now;
                    orderHeader.UserId = claim.Value;
                    orderHeader.Status = SD.StatusPlaced;
                    await _context.OrderHeaders.AddAsync(orderHeader);
                    await _context.SaveChangesAsync();

                    //Getting OrderDetails through session and storing
                    foreach (var id in sessionList)
                    {
                        var cartItem = await _context.CartItems.Include(c => c.Device).Where(c => c.Id == id).FirstOrDefaultAsync();

                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderHeaderId = orderHeader.Id,
                            DeviceId = cartItem.DeviceId,
                            SubTotal = cartItem.ItemCount * cartItem.Device.Price,
                            ItemCount = cartItem.ItemCount
                        };

                        await _context.OrderDetails.AddAsync(orderDetail);
                        _context.CartItems.Remove(cartItem);
                    }

                    await _context.SaveChangesAsync();
                    HttpContext.Session.Clear();

                    return RedirectToAction(nameof(OrderConfirmation), "Carts", new { orderId=orderHeader.Id});
                }

                //If session null it sends null value to view that displays information about empty session
                return RedirectToAction(nameof(Checkout));
            }
            else
            {
            //Model is not valid 
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

            CartItemOrderVM.CartItemList = new List<CartItem>();

            //Getting those cartItem in session
            foreach (var sessionId in sessionList)
            {
                var cartItem = await _context.CartItems
                    .Include(c => c.Device)
                    .Where(c => c.Id == sessionId)
                    .FirstOrDefaultAsync();

                if (cartItem != null)
                {
                    CartItemOrderVM.CartItemList.Add(cartItem);
                }
            }
            return View(CartItemOrderVM);
            }
        }

        //Sending Order Number To User As A Display Message
        [Authorize]
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            _backgroundTaskQueue.QueueBackgroundWorkItem(async (serviceScopeFactory, token) =>
            {
                using(var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                    //Sending user notification email about the order placement
                    var userEmail = (await dbContext.OrderHeaders.Where(o => o.Id == orderId).FirstOrDefaultAsync()).Email;
                    await emailSender.SendStatusEmailAsync(userEmail, SD.StatusPlaced, orderId.ToString());
                }
            });

            return View(orderId);
        }
    }
}