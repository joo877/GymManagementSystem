using AutoMapper;
using GymManagement.BLL.ViewModels.BookingViewModel;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            mappigMember();
            mappingSession();
            MappedPlan();
            MappedTrainer();
            MappingMemberShip();
            MappingBooking();
        }

        private void mappigMember()
        {
            CreateMap<Member, MemberViewModel>()
              .ForMember(des => des.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
              .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();

            CreateMap<Member, UpdateMemberViewModel>()
                .ForMember(des => des.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.Address.City));


            CreateMap<UpdateMemberViewModel, Member>()
                .ForMember(des => des.Name, opt => opt.Ignore())
                .ForMember(des => des.Photo, opt => opt.Ignore())
               .AfterMap((src, des) =>
               {
                   des.Address.BuildingNumber = src.BuildingNumber;
                   des.Address.Street = src.Street;
                   des.Address.City = src.City;

               });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(des => des.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street


                }))
                .ForMember(des => des.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }

        private void mappingSession()
        {
            CreateMap<CreateSesssionViewModel, Session>();
            CreateMap<Trainer,TrainerSelectViewModel>();
            CreateMap<Category,CategorySelectViewModel>();
            CreateMap<Session, SessionViewModel>()
                   .ForMember(des => des.AvailableSlots, opt => opt.Ignore())
                   .ForMember(des => des.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                   .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Session , UpdateSessionViewModel>().ReverseMap();
          
        
        
        }

        public void MappedPlan ()
        {

            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();


        }

        public void MappedTrainer() 
        {
            CreateMap<Trainer, TrainerviewModel>()
                .ForMember(des => des.Address , opt=> opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));


            CreateMap<TrainerCreateViewModel, Trainer>()
                 .ForMember(des => des.Address, opt => opt.MapFrom(src => new Address()
                 {
                     BuildingNumber=src.BuildingNumber,
                     City =src.City,
                     Street=src.Street


                 }));

            CreateMap<Trainer, UpdateViewModel>()
                .ForMember(des => des.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(des => des.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(des => des.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<UpdateViewModel, Trainer>()
                 .ForMember(des => des.Name, opt => opt.Ignore())
                 .ForMember(des => des.DateOfBirth, opt => opt.Ignore())
                 .ForMember(des => des.Gender, opt => opt.Ignore())
                .ForMember(des => des.Address, opt => opt.MapFrom(src => new Address()
                {
                    BuildingNumber=src.BuildingNumber,
                    City=src.City,
                    Street=src.Street

                }));

        }

        public void MappingMemberShip()
        {
            CreateMap<MemberShip, MemberShipViewModel>()
              
                .ForMember(des => des.StartDate, opt => DateTime.Now.ToShortDateString())
                .ForMember(des => des.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(des => des.PlanName, opt => opt.MapFrom(src => src.Plan.Name));

            CreateMap<Plan, PlanSelectListViewModel>();
            CreateMap<Member, MemberSelectListViewModel>();

            CreateMap<CreateMemberShipViewModel, MemberShip>()
                
                  .ForMember(des => des.CreatedAt, opt => DateTime.Now.ToShortTimeString());
                
                  
        }

        public void MappingBooking()
        {
            CreateMap<Booking, MemberForSessionViewModel>()
                   .ForMember(des => des.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                   .ForMember(des => des.BookingDate, opt => opt.MapFrom(src => src.CreatedAt));
                 
                   

            CreateMap<CreateMemberBookingViewModel, Booking>();
           
        }

    }
}
