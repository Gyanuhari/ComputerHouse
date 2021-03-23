using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get ListOfUsers
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userList = await _context.ApplicationUsers.Where(u => u.Id != claim.Value).ToListAsync();

            if(userList!=null)
                return View(userList);
            else
            return View();
;        }

        //Get: Edit User
        public async Task<IActionResult> Edit(string id)
        {
            var userFromDb = await _context.ApplicationUsers.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        //Post: Edit User
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ApplicationUser model, string id)
        {
            if (id != model.Id)
            {
                return BadRequest("Id did not matched");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userFromDb = await _context.ApplicationUsers.Where(u => u.Id == model.Id).FirstOrDefaultAsync();
            if (userFromDb == null)
                return NotFound();

            userFromDb.LockoutEnd = model.LockoutEnd;
            userFromDb.LockoutReason = model.LockoutReason;
            userFromDb.AccessFailedCount = model.AccessFailedCount;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LockUser(string id)
        {
            var userFromDb = await _context.ApplicationUsers.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            //Add 100 years to todays date to lock
            userFromDb.LockoutEnd = DateTime.Now.AddYears(100);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnLockUser(string id)
        {
            var userFromDb = await _context.ApplicationUsers.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            //Subtract 1 day from todays date to unlock
            userFromDb.LockoutEnd = DateTime.Now.AddDays(-1);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}