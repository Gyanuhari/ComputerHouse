using ComputerHouse.Data;
using ComputerHouse.Models;
using ComputerHouse.Models.ViewModels;
using ComputerHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles=SD.Admin)]
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        [BindProperty]
        public DeviceBrandCategoryAndBrandVM DeviceBrandCategoryBrandVM { get; set; }

        public DevicesController(ApplicationDbContext context)
        {
            _dbContext = context;
            DeviceBrandCategoryBrandVM = new DeviceBrandCategoryAndBrandVM
            {
                Device = new Models.Device(),
                OSList=_dbContext.OperatingSystems.ToList(),
                //BrandCategoryList = _dbContext.BrandCategories.ToList(),  //Only needed in edit to get this item selected
                BrandList = _dbContext.Brands.ToList()
            };
        }

        public async Task<IActionResult> Index()
        {
            var devicesList = await _dbContext.Devices
                .Include(d => d.Brand)
                .Include(d => d.BrandCategory)
                .ToListAsync();

            return View(devicesList);
        }

        //Get: Devices/Create
        public IActionResult Create()
        {
            //DeviceBrandCategoryAndBrandVM deviceBrandCategoryBrandVM = new DeviceBrandCategoryAndBrandVM
            //{
            //    Device=new Models.Device(),
            //    BrandCategoryList=await _dbContext.BrandCategories.ToListAsync(),
            //    BrandList=await _dbContext.Brands.ToListAsync()
            //};
            //Because this is replaced by the bind property.

            return View(DeviceBrandCategoryBrandVM);
        }

        //Supplies data to the cascading dropdown in brandCategories
        [HttpGet]
        public JsonResult GetBrandCategories(int id)
        {
            List<BrandCategory> brandCategoryList = new List<BrandCategory>();

            brandCategoryList = _dbContext.BrandCategories
                .Where(bc => bc.BrandId == id)
                .ToList();

            return Json(brandCategoryList);
        }

        //Post: Device Create
        [HttpPost , ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        byte[] p1 = null;

                        using (var fs1 = files[0].OpenReadStream())
                        {
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                        }
                        DeviceBrandCategoryBrandVM.Device.Image = p1;
                    }

                    await _dbContext.Devices.AddAsync(DeviceBrandCategoryBrandVM.Device);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    throw ex; 
                }
            }

            //we are not sending BrandCategoryList because it will be called based on the BrandList 
            //as cascading dropdown if not you can checked by putting debugger in the above getCategory
            //I have already tested this.
            DeviceBrandCategoryBrandVM.BrandList = await _dbContext.Brands.ToListAsync();
            DeviceBrandCategoryBrandVM.OSList = await _dbContext.OperatingSystems.ToListAsync();

            return View(DeviceBrandCategoryBrandVM);
        }

        //Get: Admin/Device/Edit/5
        public async Task<IActionResult> Edit(int id)   //id can be made deviceId, should be same in index asp-route-deviceId, but in postEdit method this creates problem
        {                                               //because if we changed here to deviceId, in postEdit you won't get value of id whatever id/deviceId you make.
            var device = await _dbContext.Devices
                .Include(d => d.Brand)
                .Include(d => d.BrandCategory)
                .Where(d=>d.Id== id)
                .SingleOrDefaultAsync();

            if (device == null)
                return NotFound();

            DeviceBrandCategoryBrandVM.Device = device;
            DeviceBrandCategoryBrandVM.BrandList = await _dbContext.Brands.ToListAsync();

            //Because of this the BrandCatgory in Db will get selected in dropdown while editing
            //which is very necessary if not other data will be selected if user is not sure about it.
            //Also, do not for get to disable the first getBrandCategory() javascript function if not it
            //will change the value of BrandCategory by calling api based on Brand that is selected.
            //Because of this it is necessary to add this in DeviceBrandCategoryAndBrandVM
            DeviceBrandCategoryBrandVM.BrandCategoryList = await _dbContext.BrandCategories.Where(bc=>bc.BrandId==device.BrandId).ToListAsync();
            DeviceBrandCategoryBrandVM.OSList = await _dbContext.OperatingSystems.ToListAsync();

            return View(DeviceBrandCategoryBrandVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)  
        {
            //this is just to demonstrate how we can retrive the data from client in different way
            //var testId = Convert.ToInt32(HttpContext.Request.Form["brandCategoryId"].ToString());

            if (id != DeviceBrandCategoryBrandVM.Device.Id)
                return BadRequest();

            var deviceToUpdate = await _dbContext.Devices
                    .Where(d => d.Id == id)
                    .SingleAsync();

            if (ModelState.IsValid)
            {
                //Get files from view
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        //Creating a new MemoryStream()
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    deviceToUpdate.Image = p1;
                }

                deviceToUpdate.Name = DeviceBrandCategoryBrandVM.Device.Name;
                deviceToUpdate.BrandId = DeviceBrandCategoryBrandVM.Device.BrandId;
                deviceToUpdate.BrandCategoryId = DeviceBrandCategoryBrandVM.Device.BrandCategoryId;
                deviceToUpdate.OSId = DeviceBrandCategoryBrandVM.Device.OSId;
                deviceToUpdate.HDType = DeviceBrandCategoryBrandVM.Device.HDType;
                deviceToUpdate.HDCapacity = DeviceBrandCategoryBrandVM.Device.HDCapacity;
                deviceToUpdate.RAMCapacity = DeviceBrandCategoryBrandVM.Device.RAMCapacity;
                deviceToUpdate.Price = DeviceBrandCategoryBrandVM.Device.Price;
                deviceToUpdate.ScreenSize = DeviceBrandCategoryBrandVM.Device.ScreenSize;
                deviceToUpdate.Bluetooth = DeviceBrandCategoryBrandVM.Device.Bluetooth;
                deviceToUpdate.HDMI = DeviceBrandCategoryBrandVM.Device.HDMI;
                deviceToUpdate.IsTouchScreen = DeviceBrandCategoryBrandVM.Device.IsTouchScreen;
                deviceToUpdate.Description = DeviceBrandCategoryBrandVM.Device.Description;

                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            DeviceBrandCategoryBrandVM.Device.Image = deviceToUpdate.Image;
            DeviceBrandCategoryBrandVM.BrandList = await _dbContext.Brands.ToListAsync();
            DeviceBrandCategoryBrandVM.BrandCategoryList = await _dbContext.BrandCategories
                .Where(bc => bc.BrandId == DeviceBrandCategoryBrandVM.Device.BrandId).ToListAsync();

            DeviceBrandCategoryBrandVM.OSList = await _dbContext.OperatingSystems.ToListAsync();

            return View(DeviceBrandCategoryBrandVM);
        }

        //Get: devices/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var device = await _dbContext.Devices
                .Include(d => d.Brand)
                .Include(d => d.BrandCategory)
                .Include(d => d.OperatingSystem)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
            //.SingleAsync();     
            //if we use .SingeAsync() it should execute one and only one item, if no item exists 
            //then error is thrown

            if (device == null)
                return NotFound("No item found");

            return View(device);
        }

        //Delete Admin/Devices/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var device = await _dbContext.Devices
                        .Where(d => d.Id == id)
                        .FirstOrDefaultAsync();

                if (device == null)
                    return NotFound("Item Not Found");

                _dbContext.Devices.Remove(device);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}