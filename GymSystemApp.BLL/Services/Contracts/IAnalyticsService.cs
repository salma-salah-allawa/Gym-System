using GymManagementBLL.ViewModels.AnalyticsViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default);
    }
}
