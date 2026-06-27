using GymManagementSystem.BLL.ViewModels.TrainersViewModels;
using GymSystemApp.BLL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Contracts
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default);
        Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default);

        Task<Result> UpdateTrainerAsync(int trainerId,TrainerToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default);
    }
}
