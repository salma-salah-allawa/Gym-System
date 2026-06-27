using GymManagementSystem.BLL.ViewModels.SessionsViewModels;
using GymSystemApp.BLL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDownListAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownListAsync(CancellationToken ct = default);

        Task<SessionViewModel> GetSessionByIdAsync(int sessionId, CancellationToken ct = default);

        Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default);

        Task<Result> UpdateSessionAsync(int sessionId ,UpdateSessionViewModel model, CancellationToken ct = default);

        Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default);
    }
}
