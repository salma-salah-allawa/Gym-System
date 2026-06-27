using AutoMapper;
using GymManagementSystem.BLL.ViewModels.TrainersViewModels;
using GymSystemApp.BLL.Common;
using GymSystemApp.BLL.Services.Contracts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default)
        {
            var trainerRepo = unitOfWork.GetRepo<Trainer>();
            var EmailExist = await trainerRepo.AnyAsync(t => t.Email == trainer.Email);
            var PhoneExist = await trainerRepo.AnyAsync(t => t.Phone == trainer.Phone);

            if(EmailExist || PhoneExist) return Result.Fail("Email And Phone Must be Uinque.");
            var MappedTrainer = mapper.Map<CreateTrainerViewModel, Trainer>(trainer);

            trainerRepo.Add(MappedTrainer);
            var rowAffected = await unitOfWork.SaveChangesAsync();
            return rowAffected > 0 ? Result.Ok() : Result.Fail("Failed to Create Trainer.");
        }

        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainerRepo = unitOfWork.GetRepo<Trainer>();
            var trainer = await trainerRepo.GetById(trainerId, ct);
            if(trainer is null) return Result.NotFound("Trainer Not Found.");

            trainerRepo.Delete(trainerId);
            var rowAffected = await unitOfWork.SaveChangesAsync();
            return rowAffected > 0 ? Result.Ok() : Result.Fail("Failed to Delete Trainer.");
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork.GetRepo<Trainer>().GetAll(false,ct);
            if (!trainers.Any()) return [];
            var MappedTrainers = mapper.Map<IEnumerable<Trainer>,IEnumerable<TrainerViewModel>>(trainers);
            return MappedTrainers;
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepo<Trainer>().GetById(trainerId, ct);
            if (trainer is null) return null;
            var MappedTrainer = mapper.Map<Trainer, TrainerViewModel>(trainer);
            return MappedTrainer;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepo<Trainer>().GetById(trainerId, ct);
            if (trainer is null) return null;
            return mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
        }

        public async Task<Result> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainerRepo = unitOfWork.GetRepo<Trainer>();
            var trainer = await trainerRepo.GetById(trainerId, ct);
            if(trainer is null) return Result.NotFound("Trainer Not Found.");

            if (await trainerRepo.AnyAsync(t => t.Email == model.Email && t.Id != trainerId,ct)) return Result.Validation("Email Must be Unique.");
            if(await trainerRepo.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId,ct)) return Result.Validation("Phone Must be Unique.");
            
            mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;
            trainerRepo.Update(trainer);

            var rowAffected = await unitOfWork.SaveChangesAsync();
            return rowAffected > 0 ? Result.Ok() : Result.Fail("Failed to Update Trainer.");
        }
    }
}
