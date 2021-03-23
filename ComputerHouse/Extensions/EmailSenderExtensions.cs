using ComputerHouse.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ComputerHouse.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendStatusEmailAsync(this IEmailSender emailSender, string email, string status, string orderId)
        {
            var subject = "";
            var message = "";

            if (status == SD.StatusPlaced)
            {
                subject = "Order Placed Successfully";
                message = "<h4>Your Order With Number: <strong>" + orderId + "</strong ></h4>" + "<p>Has Been Placed Successfully…….<p/>" + "<p>Thank You For Shopping With Us!<p/>" + "<p><strong> Computer House </strong><p/>";
            }
            if (status == SD.StatusShipped)
            {
                subject = "Order Shipped";
                message = "<h4>Your Order With Number: <strong>" + orderId + "</strong ></h4>" + "<p>Has Been Shipped. Please Allow Some Days To Reach The Destination.<p/>" + "<p>Thank You For Shopping With Us!<p/>" + "<p><strong>Computer House</strong><p/>";
            }
            if(status==SD.StatusOnWay)
            {
                subject = "Order On Way";
                message = "<h4>Your Order With Number: <strong>" + orderId + "</strong ></h4>" + "<p>Is On The Way. Please Allow Few Days To Reach The Destination.<p/>" + "<p>Thank You For Shopping With Us!<p/>" + "<p><strong>Computer House</strong><p/>";
            }
            if(status==SD.StatusDelivered)
            {
                subject = "Order Delivered";
                message = "<h4>Your Order With Number: <strong>" + orderId + "</strong ></h4>" + "<p>Has Been Delivered.<p/>" + "<p>Thank You For Shopping With Us!<p/>" + "<p><strong>Computer House</strong><p/>";
            }
            return emailSender.SendEmailAsync(email, subject, message);
        }
    }
}
