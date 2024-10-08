using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text.RegularExpressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRedialChartDataAsync()
        {
           return Json(await _dashboardService.GetTotalBookingRedialChartDataAsync());

        }

        public async Task<IActionResult> GetRegisteredUserChartDataAsync()
        {
           return Json(await _dashboardService.GetRegisteredUserChartDataAsync());
        }

        public async Task<IActionResult> GetRevenueChartDataAsync()
        {
            return Json(await _dashboardService.GetRevenueChartDataAsync());
        }

        public async Task<IActionResult> GetBookingPieChartDataAsync()
        {
            return Json(await _dashboardService.GetBookingPieChartDataAsync());
        }


        //fetches booking and customer data from the last 30 days,
        //groups them by date, and counts new bookings and customers per day.
        //It then performs left and right joins to merge the data, sorts it by date,
        //and prepares it for a line chart.
        public async Task<IActionResult> GetMemberAndBookingLineChartDataAsync()
        {
            return Json(await _dashboardService.GetMemberAndBookingLineChartDataAsync());
        }

    }
}
