using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text.RegularExpressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRedialChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending
           || b.Status == SD.StatusCancelled);

            // we need to retrieve the count by current month and previous month.
            var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate &&
            b.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate &&
            b.BookingDate <= currentMonthStartDate);

            return Json(GetRedialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth));

        }

        public async Task<IActionResult> GetRegisteredUserChartDataAsync()
        {
            var totalUsers = _unitOfWork.User.GetAll();

            // we need to retrieve the count by current month and previous month.
            var countByCurrentMonth = totalUsers.Count(b => b.CreatedAt >= currentMonthStartDate &&
            b.CreatedAt <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(b => b.CreatedAt >= previousMonthStartDate &&
            b.CreatedAt <= currentMonthStartDate);



            return Json(GetRedialChartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetRevenueChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending
           || b.Status == SD.StatusCancelled);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));
            // we need to retrieve the count by current month and previous month.
            var countByCurrentMonth = totalBookings.Where(b => b.BookingDate >= currentMonthStartDate &&
            b.BookingDate <= DateTime.Now).Sum(u => u.TotalCost);

            var countByPreviousMonth = totalBookings.Where(b => b.BookingDate >= previousMonthStartDate &&
            b.BookingDate <= currentMonthStartDate).Sum(u => u.TotalCost);

            return Json(GetRedialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetBookingPieChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30) &&
            (b.Status != SD.StatusPending || b.Status == SD.StatusCancelled));

            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartVM pieChartVM = new()
            {
                Labels = new string[] { "New Customer Bookings", "Returning Customer Bookings" },
                Series = new decimal[] { Convert.ToDecimal(bookingsByNewCustomer), Convert.ToDecimal(bookingsByReturningCustomer) }
            };
            return Json(pieChartVM);
        }


        //fetches booking and customer data from the last 30 days,
        //groups them by date, and counts new bookings and customers per day.
        //It then performs left and right joins to merge the data, sorts it by date,
        //and prepares it for a line chart.
        public async Task<IActionResult> GetMemberAndBookingLineChartDataAsync()
        {
            var bookingData = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
            u.BookingDate <= DateTime.Now)
            .GroupBy(b => b.BookingDate.Date)
            .Select(u => new
            {
                DateTime = u.Key,
                NewBookingCount = u.Count()
            });

            var customerData = _unitOfWork.User.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30) &&
            u.CreatedAt <= DateTime.Now)
                .GroupBy(b => b.CreatedAt.Date)
                .Select(u => new
                {
                    DateTime = u.Key,
                    NewCustomerCount = u.Count()
                });

            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                (booking, customer) => new
                {
                    booking.DateTime,
                    booking.NewBookingCount,
                    NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
                });

            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
              (customer, booking) => new
              {
                  customer.DateTime,
                  NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                  customer.NewCustomerCount
              });

            var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
            var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartDataList = new()
            {
                new ChartData
                {
                    Name = "New Bookings",
                    Data = newBookingData
                },
                new ChartData
                {
                    Name = "New Members",
                    Data = newCustomerData
                 }
            };

            LineChartVM lineChartVM = new()
            {
                Categories = categories,
                Series = chartDataList
            };
            return Json(lineChartVM);
        }

        private static RedialBarChartVM GetRedialChartDataModel(int totalCount, double currentMonthCount, double previousMonthCount)
        {
            RedialBarChartVM redialBarChartVM = new();

            int increaseDecreaseRatio = 100;

            if (previousMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - previousMonthCount) / previousMonthCount * 100);
            }

            redialBarChartVM.TotalCount = totalCount;
            redialBarChartVM.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            redialBarChartVM.HasRatioIncreased = currentMonthCount > previousMonthCount;
            redialBarChartVM.Series = new int[] { increaseDecreaseRatio };


            return redialBarChartVM;
        }
    }
}
