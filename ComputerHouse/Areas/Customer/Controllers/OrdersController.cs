using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Customer)]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? searchNumber)
        {
            RecentOrderViewModel orderVM = new RecentOrderViewModel();
            if (searchNumber != null)
            {
                var order =await _context.OrderHeaders
                    .Where(o=>o.Id== searchNumber)
                    .SingleOrDefaultAsync();

                if(order!=null)
                {
                    orderVM.OrderStatus = order.Status;
                    orderVM.OrderNo = order.Id;
                    //Just Assigning Dummy Date
                    orderVM.ExpectedArrival = DateTime.Now.AddDays(10);

                    if(orderVM.OrderStatus==SD.StatusPlaced)
                    {
                        orderVM.StatusMessage = "Your Order Has Been Placed. Please See The Expected Arrival.";
                    }
                    if (orderVM.OrderStatus == SD.StatusShipped)
                    {
                        orderVM.StatusMessage = "Your Order Has Been Shipped. Please See The Expected Arrival.";
                    }
                    if (orderVM.OrderStatus == SD.StatusOnWay)
                    {
                        orderVM.StatusMessage = "Your Order Is On The Way. Please See The Expected Arrival.";
                    }
                    if (orderVM.OrderStatus == SD.StatusDelivered)
                    {
                        orderVM.StatusMessage = "Your Order Has Been Delivered. Please Contact Support If You Have Not Received.";
                    }
                    return View(orderVM);
                }
                //When No Item Is Found
                orderVM.OrderNo = searchNumber.GetValueOrDefault();
                orderVM.StatusMessage = "Order Does Not Exists For This Order Number.";

                return View(orderVM);
            }

            return View(new RecentOrderViewModel());
        }

        [ValidateAntiForgeryToken]
        public IActionResult SearchOrder([Bind("OrderNo")] RecentOrderViewModel orderVM)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { searchNumber = orderVM.OrderNo });
            }

            return View(orderVM);
        }

        //Get orders/MyOrderHistory
        public async Task<IActionResult> MyOrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var ordersList = await _context.OrderHeaders
                .Include(o=>o.ApplicationUser)
                .Where(o => o.UserId == claims.Value)
                .ToListAsync();

            if(ordersList!=null)
            {
                List<OrderDetail> orderDetailsList = new List<OrderDetail>();
                foreach (var order in ordersList)
                {
                    var orderDetails = await _context.OrderDetails
                            .Include(o=>o.Device)
                            .Where(o => o.OrderHeaderId == order.Id)
                            .ToListAsync();

                    orderDetailsList.AddRange(orderDetails);
                }

                OrderHistoryViewModel orderHistoryViewModel = new OrderHistoryViewModel
                {
                    OrderHeaderList = ordersList,
                    OrderDetailList = orderDetailsList
                };

                return View(orderHistoryViewModel);
            }

            return View();
        }
    }
}