using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles=SD.Admin)]
    public class BrandCategoriesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        [TempData]
        public string StatusMessage { get; set; }

        public BrandCategoriesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Gets List of BrandCategories
        public async Task<IActionResult> Index()
        {
            var brandCategoryList = await _dbContext.BrandCategories
                .Include(bc => bc.Brand)
                .ToListAsync();

            return View(brandCategoryList);
        }

        //Get: BrandCategories/Create
        public async Task<IActionResult> Create()
        {
            var brandCategoryVM = new BrandAndCategoryVM()
            {
                BrandCategory = new BrandCategory(),
                BrandList = await _dbContext.Brands.ToListAsync()
            };

            return View(brandCategoryVM);
        }

        //Post: BrandCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandAndCategoryVM brandCategoryVM)
        {
            if (ModelState.IsValid)
            {
                bool doesBrandCategoryExists = _dbContext.BrandCategories
                    .Any(bc => bc.Name.ToLower().Trim().Equals(brandCategoryVM.BrandCategory.Name.ToLower().Trim())
                    && bc.BrandId == brandCategoryVM.BrandCategory.BrandId);

                if (!doesBrandCategoryExists)
                {
                    await _dbContext.BrandCategories.AddAsync(brandCategoryVM.BrandCategory);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    BrandAndCategoryVM brandAndCategoryVM = new BrandAndCategoryVM()
                    {
                        BrandCategory = brandCategoryVM.BrandCategory,
                        BrandList = await _dbContext.Brands.ToListAsync(),
                        StatusMessage = $"Error: Brand Category {brandCategoryVM.BrandCategory.Name} already exists."
                    };

                    return View(brandAndCategoryVM);
                };
            }
            else
            {
                BrandAndCategoryVM brandAndCategoryVM = new BrandAndCategoryVM()
                {
                    BrandCategory = brandCategoryVM.BrandCategory,
                    BrandList = await _dbContext.Brands.ToListAsync()
                };

                return View(brandAndCategoryVM);
            }
        }

        #region API CALLS
        //Get BrandCategories
        [HttpGet]
        //[Route("/api/GetBrands")]
        public JsonResult GetBrandCategoriesByBrand(int brandId)
        {
            List<string> brandList = new List<string>();

            brandList = _dbContext.BrandCategories
                .Where(bc => bc.BrandId == brandId)
                .Select(bc => bc.Name)
                .ToList();

            //if (brandList == null)
            //    return null;

            return Json(brandList);
        }

        //Get: BrandCategories/id
        [HttpGet] //Not necessary because we have only one Edit method, if two or more need to mention which one to refer
        public async Task<IActionResult> Edit(int? brandCategoryId)
        {
            if (brandCategoryId != null)
            {
                BrandAndCategoryVM brandCategoryVM = new BrandAndCategoryVM()
                {
                    BrandList = await _dbContext.Brands.ToListAsync(),
                    BrandCategory = await _dbContext.BrandCategories.Where(bc => bc.Id == brandCategoryId).FirstOrDefaultAsync()
                };

                //var jsonData = JsonConvert.SerializeObject(brandCategoryVM);
                //return Ok(jsonData);

                return Json(brandCategoryVM);  //The above 2 lines of codes is replaced by this because here, Json will serialize data automatically.
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit([FromBody] BrandAndCategoryVM brandCategoryVM)
        {
            if (ModelState.IsValid)
            {
                var doesBrandCategoryExists = await _dbContext.BrandCategories
                    .AnyAsync(bc => bc.Name.ToLower().Trim().Equals(brandCategoryVM.BrandCategory.Name.ToLower().Trim())
                    && bc.BrandId == brandCategoryVM.BrandCategory.BrandId);

                if (!doesBrandCategoryExists)
                {

                    var brandCategoryToUpdate = await _dbContext.BrandCategories
                        .Where(bc => bc.Id == brandCategoryVM.BrandCategory.Id)
                        .SingleAsync();

                    brandCategoryToUpdate.Name = brandCategoryVM.BrandCategory.Name;
                    brandCategoryToUpdate.BrandId = brandCategoryVM.BrandCategory.BrandId;

                    await _dbContext.SaveChangesAsync();

                    return Json(new BrandAndCategoryVM { StatusMessage = "Success: Data Edited Successfully!" });
                    //return RedirectToAction(nameof(Index));
                }

                brandCategoryVM.StatusMessage = $"Error: Data Already Exists!";
                return Json(brandCategoryVM);

            }

            brandCategoryVM.StatusMessage = $"Error: Data Not Valid";
            return Json(brandCategoryVM);
            //return BadRequest();
        }

        //Get BrandCategories/Detail
        public async Task<IActionResult> Detail(int brandCategoryId)
        {
            var brandCategory = await _dbContext.BrandCategories
                .Include(bc => bc.Brand)
                .Where(bc => bc.Id == brandCategoryId)
                .SingleAsync();

            if (brandCategory == null)
                return NotFound();

            return Json(brandCategory);

            //Show student that this is the way of sending the data to the client
            //var brandCategoryDto = new BrandCategory
            //{
            //    Name = brandCategory.Name,
            //    Brand = new Brand
            //    {
            //        Name=brandCategory.Brand.Name
            //    }
            //};
            //return Json(brandCategoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int brandcategoryId)
        {
            var brandCategory = await _dbContext.BrandCategories
                .Where(bc => bc.Id == brandcategoryId)
                .SingleAsync();

            if (brandCategory == null)
                return BadRequest();

            _dbContext.BrandCategories.Remove(brandCategory);
            await _dbContext.SaveChangesAsync();

            return Json(brandcategoryId); //can send any data but be careful don't send important information.
        }

        #endregion END API CALLS
    }
}