using AutoMapper;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AnalyticsService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var sessions = await unitOfWork.GetRepo<Session>().GetAll(false,ct);

            var totalMembers = await unitOfWork.GetRepo<Member>().CountAsync(cancellation:ct);
            var totalTrainers = await unitOfWork.GetRepo<Trainer>().CountAsync(cancellation:ct);
            var activeMembers = await unitOfWork.GetRepo<Membership>().CountAsync(m=>m.EndDate > DateTime.Now,ct);

            return new AnalyticsViewModel
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(s => s.EndDate < DateTime.Now)
            };
        }
    }
}
