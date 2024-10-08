using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Application.Services.Interface
{
    public interface IDashboardService
    {
        Task<RedialBarChartDto> GetTotalBookingRedialChartDataAsync();

        Task<RedialBarChartDto> GetRegisteredUserChartDataAsync();

        Task<RedialBarChartDto> GetRevenueChartDataAsync();

        Task<PieChartDto> GetBookingPieChartDataAsync();

        Task<LineChartDto> GetMemberAndBookingLineChartDataAsync();
    }
}
