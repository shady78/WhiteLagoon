using Microsoft.AspNetCore.Mvc;
using Syncfusion.Presentation;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVillaService _villaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IUnitOfWork unitOfWork, IVillaService villaService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _villaService = villaService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                Nights = 1
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
            foreach (var villa in homeVM.VillaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumbersList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(b => b.Status == SD.StatusApproved ||
            b.Status == SD.StatusCheckedIn).ToList();

            foreach (var villa in villaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count
                    (villa.Id, villaNumbersList, checkInDate, nights, bookedVillas);

                villa.IsAvailable = roomAvailable > 0 ? true : false;
            }
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = _villaService.GetVillasAvailabilityByDate(nights, checkInDate),
                Nights = nights
            };

            return PartialView("_VillaList", homeVM);
        }


        [HttpPost]
        public IActionResult GeneratePPTExport(int id)
        {
            var villa = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").FirstOrDefault(x => x.Id == id);
            if (villa is null)
            {
                return RedirectToAction(nameof(Error));
            }
            string basePath = _webHostEnvironment.WebRootPath;
            string filePath = basePath + @"/exports/ExportVillaDetails.pptx";

            using IPresentation presentation = Presentation.Open(filePath);

            ISlide slide = presentation.Slides[0];

            IShape? shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "txtVillaName") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Name;
            }
            shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "txtVillaDescription") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Description;
            }


            shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "txtOccupancy") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Max Occupancy: {0} adults", villa.Occupancy);
            }
            shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "txtVillaSize") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Villa Size: {0} sqft", villa.Sqft);
            }
            shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "txtPricePerNight") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("USD {0}/night", villa.Price.ToString("c"));
            }


            shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "txtVillaAmenitiesHeading") as IShape;
            if (shape is not null)
            {
                List<string> listItems = villa.VillaAmenity.Select(x => x.Name).ToList();

                shape.TextBody.Text = "";

                foreach (var item in listItems)
                {
                    IParagraph paragraph = shape.TextBody.AddParagraph();
                    ITextPart textPart = paragraph.AddTextPart(item);

                    paragraph.ListFormat.Type = ListType.Bulleted;
                    paragraph.ListFormat.BulletCharacter = '\u2022';
                    textPart.Font.FontSize = 18;
                    textPart.Font.Color = ColorObject.FromArgb(144, 148, 152);
                }
            }

            // image
            shape = slide.Shapes.FirstOrDefault(x => x.ShapeName == "imgVilla") as IShape;
            if (shape is not null)
            {
                byte[] imageData;
                string imageUrl;
                try
                {
                    imageUrl = string.Format("{0}{1}", basePath, villa.ImageUrl);
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                catch (Exception)
                {
                    imageUrl = string.Format("{0}{1}", basePath, "/images/placeholder.png");
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                slide.Shapes.Remove(shape);
                using MemoryStream imageStream = new MemoryStream(imageData);
                IPicture newPicture = slide.Pictures.AddPicture(imageStream, 60, 120, 300, 200);
            }

            MemoryStream memoryStream = new();
            presentation.Save(memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream, "application/pptx", "villa.pptx");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
