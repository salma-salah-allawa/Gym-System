using GymSystemApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.DAL.Repositories.Contracts
{
    public interface ISessionRepo:IGenericRepo<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId,CancellationToken ct);
        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct);
    }
}
