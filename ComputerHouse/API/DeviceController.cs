using ComputerHouse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerHouse.API
{
    //[Route("{area}/api/[controller]")] if this api was inside any area
    //https://localhost:44391/customer/api/device

    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeviceController(ApplicationDbContext context)
        {
            _context = context;
        }


        //https://localhost:44391/api/device/
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var device = await _context.Devices
        //        .ToListAsync();

        //    if (device == null)
        //    {
        //        ModelState.AddModelError("", "Device Not Found");
        //        return NotFound(ModelState);
        //    }

        //    return Ok(device);
        //}

        //https://localhost:44391/api/device/2
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var device = await _context.Devices
                .Include(d=>d.Brand)
                .Include(d=>d.BrandCategory)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            if (device == null)
                return NotFound();


            //var base64 = Convert.ToBase64String(device.Image);
            //var imgSrc = string.Format("data:image/jpg;base64,{0}", base64);

            //var quickVM = new DeviceQuickViewDto
            //{
            //    Device = device,
            //    Picture = imgSrc
            //};
            //return Ok(quickVM);

            return Ok(device);
        }
    }
}