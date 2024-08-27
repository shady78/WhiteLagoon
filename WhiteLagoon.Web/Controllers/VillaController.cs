using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var villas = _context.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            // Custom Model Validation
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "Name and Description should not be the same.");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Villas.Add(obj);
            _context.SaveChanges();
            TempData["success"] = "The villa has been created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int villaId)
        {
            var obj = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            //var objId = _context.Villas.Find(obj.Id);
            //if (objId is null)
            //{
            //    return RedirectToAction("Error", "Home");
            //}

            if (ModelState.IsValid && obj.Id > 0)
            {
                _context.Villas.Update(obj);
                _context.SaveChanges();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be updated.";
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            var obj = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }



        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var objFromDb = _context.Villas.FirstOrDefault(x => x.Id == obj.Id);

            if (obj is not null)
            {
                _context.Villas.Remove(objFromDb);
                _context.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be deleted.";

            return View();
        }

    }
}
