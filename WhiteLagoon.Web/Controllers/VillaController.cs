using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
            _unitOfWork.Villa.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "The villa has been created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int villaId)
        {
            var obj = _unitOfWork.Villa.Get(x => x.Id == villaId);
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
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be updated.";
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            var obj = _unitOfWork.Villa.Get(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }



        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var objFromDb = _unitOfWork.Villa.Get(v => v.Id == obj.Id);

            if (obj is not null)
            {
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa could not be deleted.";

            return View();
        }

    }
}
