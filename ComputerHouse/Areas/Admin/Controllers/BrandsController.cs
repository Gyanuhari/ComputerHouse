using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles=SD.Admin)]
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandsController(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        //Get: List of Brands
        public async Task<IActionResult> Index()
        {
            var brandList = await _dbContext.Brands.ToListAsync();

            return View(brandList);
        }

        //Get: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        //Post: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.CreatedAt = DateTime.Now;

                await _dbContext.Brands.AddAsync(brand);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        //Get: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var brand = await _dbContext.Brands
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        //Post: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Brand brand)
        {
            if (id != brand.Id)
                return BadRequest();

            if(ModelState.IsValid)
            {
                var brandToEdit = await _dbContext.Brands
                    .Where(b => b.Id == id)
                    .FirstOrDefaultAsync();

                if (brandToEdit == null)
                    return NotFound();

                //Name Updated
                brandToEdit.Name = brand.Name;

                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        //Get: Brands/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var brand = await _dbContext.Brands
                .Where(b => b.Id == id)
                .SingleAsync();

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        //Get: Brands/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _dbContext.Brands
                .Where(b => b.Id == id)
                .SingleAsync();

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        //Get: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _dbContext.Brands
                .Where(b => b.Id == id)
                .SingleAsync();

            if (brand == null)
                return NotFound();

            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}