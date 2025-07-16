using InternalBookingApp.Data;
using InternalBookingApp.Models;
using InternalBookingApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace InternalBookingApp.Controllers
{
    public class ResourceController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ResourceController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddResourceViewModel viewModel)
        {
            var resource = new Resource
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Location = viewModel.Location,
                Capacity = viewModel.Capacity,
                IsAvailable = viewModel.IsAvailable


            };

            await dbContext.Resources.AddAsync(resource);
            await dbContext.SaveChangesAsync();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var resource = await dbContext.Resources.ToListAsync();
            return View(resource);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var resource = await dbContext.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(Resource viewModel)
        {
            var resource = await dbContext.Resources.FindAsync(viewModel.Id);

            if (resource is not null)
            {
                resource.Name = viewModel.Name;
                resource.Description = viewModel.Description;
                resource.Location = viewModel.Location;
                resource.Capacity = viewModel.Capacity;
                resource.IsAvailable = viewModel.IsAvailable;
                await dbContext.SaveChangesAsync();

            }

            return RedirectToAction("List", "Resource");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Resource viewModel)
        {
            var resource = await dbContext.Resources.FindAsync(viewModel.Id);

            if (resource is not null)
            {
                dbContext.Resources.Remove(resource);
                await dbContext.SaveChangesAsync();

            }

            return RedirectToAction("List", "Resource");

        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || dbContext.Resources == null)
            {
                return NotFound();
            }

            var resource = await dbContext.Resources
                .FirstOrDefaultAsync(m => m.Id == id);

            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }


        //public IActionResult Details(Guid id)
        //{
        //    var resource = dbContext.Resources
        //        .Include(r => r.Bookings)
        //        .FirstOrDefault(r => r.Id == id);

        //    if (resource == null)
        //        return NotFound();

        //    return View(resource);
        //}




    }

}
