using AutoMapper;
using GymManagementSystem.BLL.ViewModels.PlansViewModels;
using GymManagementSystem.BLL.ViewModels.SessionsViewModels;
using GymManagementSystem.BLL.ViewModels.TrainersViewModels;
using GymSystemApp.BLL.ViewModels.MembershipsViewModels;
using GymSystemApp.BLL.ViewModels.MembersViewModels;
using GymSystemApp.DAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace GymSystemApp.BLL.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapPlan();
            MapMember();
            MapTrainer();
            MapMembership();
            
        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>().ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(des => des.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(des => des.AvailableSlots, opt => opt.Ignore()).ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category, CategorySelectViewModel>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(des => des.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>().ForMember(des => des.Name, opt => opt.Ignore());
        }
        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>().ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - " +
                                                                         $"{src.Address.Street} - " +
                                                                         $"{src.Address.City}"));


            CreateMap<Member, MemberToUpdateViewModel>().ForMember(des => des.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.Address.City));


            CreateMap<MemberToUpdateViewModel, Member>().ForPath(des => des.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(des => des.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(des => des.Address.City, opt => opt.MapFrom(src => src.City))
                .ForMember(des => des.Name, opt => opt.Ignore());



            CreateMap<CreateMemberViewModel, Member>().ForPath(des => des.HealthRecord.Height, opt => opt.MapFrom(src => src.HealthRecordViewModel.Height))
                .ForPath(des => des.HealthRecord.Weight, opt => opt.MapFrom(src => src.HealthRecordViewModel.Weight))
                .ForPath(des => des.HealthRecord.BloodType, opt => opt.MapFrom(src => src.HealthRecordViewModel.BloodType))
                .ForPath(des => des.HealthRecord.Note, opt => opt.MapFrom(src => src.HealthRecordViewModel.Note))
                .ForPath(des => des.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(des => des.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(des => des.Address.City, opt => opt.MapFrom(src => src.City));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
        }
        private void MapTrainer()
        {
            CreateMap<Trainer, TrainerViewModel>().ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - " +
                                                                         $"{src.Address.Street} - " +
                                                                         $"{src.Address.City}"))
                .ForMember(des=>des.Specialties,opt=> opt.MapFrom(scr=> $"{scr.Specialties.ToString()}"));

            CreateMap<CreateTrainerViewModel, Trainer>().ForPath(des => des.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(des => des.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(des => des.Address.City, opt => opt.MapFrom(src => src.City));

            CreateMap<Trainer, TrainerToUpdateViewModel>().ForMember(des => des.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.Address.City));
                

            CreateMap<TrainerToUpdateViewModel, Trainer>().ForPath(des => des.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(des => des.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(des => des.Address.City, opt => opt.MapFrom(src => src.City))
                .ForMember(des => des.Name, opt => opt.Ignore());
        }
        private void MapMembership()
        {
            CreateMap<Membership, MembershipViewModel>().ForMember(des => des.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(des => des.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(des => des.StartDate, opt => opt.MapFrom(src => src.Plan.CreatedAt));

            CreateMap<CreateMembershipViewModel, Membership>();
            CreateMap<Plan, PlanSelectListViewModel>();
            CreateMap<Member, MemberSelectListViewModel>();
        }
    }
}
