using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PieChartDto> GetBookingPieChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30) &&
            (b.Status != SD.StatusPending || b.Status == SD.StatusCancelled));

            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartDto PieChartDto = new()
            {
                Labels = new string[] { "New Customer Bookings", "Returning Customer Bookings" },
                Series = new decimal[] { Convert.ToDecimal(bookingsByNewCustomer), Convert.ToDecimal(bookingsByReturningCustomer) }
            };
            return PieChartDto;
        }

        public async Task<LineChartDto> GetMemberAndBookingLineChartDataAsync()
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

            LineChartDto LineChartDto = new()
            {
                Categories = categories,
                Series = chartDataList
            };
            return LineChartDto;
        }

        public async Task<RedialBarChartDto> GetRegisteredUserChartDataAsync()
        {
            var totalUsers = _unitOfWork.User.GetAll();

            // we need to retrieve the count by current month and previous month.
            var countByCurrentMonth = totalUsers.Count(b => b.CreatedAt >= currentMonthStartDate &&
            b.CreatedAt <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(b => b.CreatedAt >= previousMonthStartDate &&
            b.CreatedAt <= currentMonthStartDate);



            return SD.GetRedialChartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RedialBarChartDto> GetRevenueChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending
         || b.Status == SD.StatusCancelled);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));
            // we need to retrieve the count by current month and previous month.
            var countByCurrentMonth = totalBookings.Where(b => b.BookingDate >= currentMonthStartDate &&
            b.BookingDate <= DateTime.Now).Sum(u => u.TotalCost);

            var countByPreviousMonth = totalBookings.Where(b => b.BookingDate >= previousMonthStartDate &&
            b.BookingDate <= currentMonthStartDate).Sum(u => u.TotalCost);

            return SD.GetRedialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RedialBarChartDto> GetTotalBookingRedialChartDataAsync()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending
                      || b.Status == SD.StatusCancelled);

            // we need to retrieve the count by current month and previous month.
            var countByCurrentMonth = totalBookings.Count(b => b.BookingDate >= currentMonthStartDate &&
            b.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(b => b.BookingDate >= previousMonthStartDate &&
            b.BookingDate <= currentMonthStartDate);

            return SD.GetRedialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth);
        }



    }
}
