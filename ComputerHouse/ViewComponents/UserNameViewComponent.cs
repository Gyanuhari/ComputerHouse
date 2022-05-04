using ComputerHouse.Data;
using ComputerHouse.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.ViewComponents
{
    public class UserNameViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public UserNameViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userId = UserIdentityExtensions.GetUserIdByClaimsPrincipal(HttpContext.User);

            var userFromDb =await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            return View(userFromDb);
        }
    }
}
