using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionsViewModels;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End Date Must be After Start Date");
            if(model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must be In The Future");

            var TrainerRepo = unitOfWork.GetRepo<Trainer>();
            var trainer = await TrainerRepo.GetById(model.TrainerId, ct);
            if(trainer == null) return Result.NotFound("Trainer Not Found");

            var CategoryRepo = unitOfWork.GetRepo<Category>();
            var category = await CategoryRepo.GetById(model.CategoryId, ct);
            if(category == null) return Result.NotFound("Category Not Found");

            var session = mapper.Map<CreateSessionViewModel, Session>(model);
            var SessionRepo = unitOfWork.GetRepo<Session>();
            SessionRepo.Add(session);

            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Create Session") ;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await unitOfWork.SessionRepo.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (!sessions.Any()) return [];

            sessions=sessions.OrderByDescending(s => s.StartDate);
            //map the sessions to SessionViewModel
            var MappedSessions = mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots =(session.Capacity - await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(session.Id,ct));
            }
            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownListAsync(CancellationToken ct = default)
        {
            var categories = await unitOfWork.GetRepo<Category>().GetAll(false,ct);
            return  mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<SessionViewModel> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepo.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, ct);
            if(session == null) return null;

            var MappedSession = mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = MappedSession.Capacity - await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(sessionId, ct);
            return MappedSession;
        }

        public async Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepo<Session>().GetById(sessionId, ct);
            if (session == null) return null;
            if(!await IsSessionValidForUpdateAsync(session, ct)) return null;

            return mapper.Map<Session, UpdateSessionViewModel>(session);
        }

        private async Task<bool> IsSessionValidForUpdateAsync(Session session, CancellationToken ct = default)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var bookedSlotsCount = await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(session.Id, ct);
            if (bookedSlotsCount > session.Capacity) return false;
            return bookedSlotsCount == 0;
        }
        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDownListAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork.GetRepo<Trainer>().GetAll(false, ct);
            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var sessionRepo = unitOfWork.GetRepo<Session>();
            var session = await sessionRepo.GetById(sessionId, ct);
            if(session is null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now) return Result.Fail("Session Has Already Started");

            var bookedSlotsCount = await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(sessionId, ct);
            if(bookedSlotsCount > 0) return Result.Fail("Can not Edit a Session That Has Booked Slots");

            if(model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must be In The Future");
            if(model.EndDate <= model.StartDate) return Result.Validation("End Date Must be After Start Date");

            var trainerRepo = unitOfWork.GetRepo<Trainer>();
            var trainer = await trainerRepo.GetById(model.TrainerId, ct);
            if(trainer is null) return Result.NotFound("Trainer Not Found");

            session.UpdatedAt = DateTime.Now;
            mapper.Map(model, session);
            sessionRepo.Update(session);

            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Update Session");
        }

        public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var sessionRepo = unitOfWork.GetRepo<Session>();
            var session = await sessionRepo.GetById(sessionId, ct);
            if(session is null) return Result.NotFound("Session Not Found");
            if(session.EndDate >= DateTime.Now) return Result.Fail("Can Not Delete a Session That Has Not Yet Ended");

            var bookedSlotsCount = await unitOfWork.SessionRepo.GetCountOfBookedSlotsAsync(sessionId, ct);
            if(bookedSlotsCount > 0) return Result.Fail("Can Not Delete a Session That Has Booked Slots");

            sessionRepo.Delete(sessionId);
            var rowEffected = await unitOfWork.SaveChangesAsync();
            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Delete Session");
        }
    }
}
