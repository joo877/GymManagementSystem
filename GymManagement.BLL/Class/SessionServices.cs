using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSesssionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate Must be Greater Than StartDate");
            if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate Must be Greater Than DateOfNow"); 
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity Must be Between 1 And 25");

            var trainer  =await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category =await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.NotFound("category Not Found");

            var isVaild = Enum.TryParse<Specialties>(category.CategoryName , true,out var categorySpecialty );
            if (!isVaild || trainer.Specialties != categorySpecialty) return Result.Validation("Cannot Create This Session To This Trainer");

         var session = _mapper.Map<Session>(model);
            _unitOfWork.SessionRepository.Add(session);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK():Result.Failed("Session Failed To Created");

        }

        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory(ct:ct);
            if (sessions == null || !sessions.Any()) return Result<IEnumerable<SessionViewModel>>.NotFound("Sessions Not Found");
            var mappedSession = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSession)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(session.Id, ct);
            }
            return Result<IEnumerable<SessionViewModel>>.OK(mappedSession);

        }

        public async Task<Result<IEnumerable<CategorySelectViewModel>>> GetCategoryForDropDwonListAsync(CancellationToken ct)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);

           var categoryMaped= _mapper.Map<IEnumerable<CategorySelectViewModel>>(category);
            return Result<IEnumerable<CategorySelectViewModel>>.OK(categoryMaped);
        }

        public async Task<Result<SessionViewModel>> GetSessionById(int sessionId, CancellationToken ct = default)
        {
            var session =await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId, ct);
            if (session == null)
            {

                return Result<SessionViewModel>.NotFound("Session Not Found");
            }
            else
            {
                var sessionMaped = _mapper.Map<SessionViewModel>(session);
                sessionMaped.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(sessionId, ct);

                return Result<SessionViewModel>.OK(sessionMaped);

            }
           
        }

        public async Task<Result<IEnumerable<TrainerSelectViewModel>>> GetTrainerForDropDwonListAsync(CancellationToken ct)
        {
            var  trainer = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
             var trainerMaped =  _mapper.Map <IEnumerable<TrainerSelectViewModel>>(trainer);
            return Result<IEnumerable<TrainerSelectViewModel>>.OK(trainerMaped);


        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId,ct);
            if (session == null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");
            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Failed("Can Not Edit Session That Has Already Started Or Completed ");
            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result<UpdateSessionViewModel>.Failed("Can Not Edit Session That Has Already Bookings");
            var mapedSession = _mapper.Map<UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.OK(mapedSession);

        }

        public async Task<Result> UpdateSessionAsync(int sesssionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sesssionId, ct);
            if (session == null) return Result.NotFound("Session Not Found");
            if (session.StartDate <= DateTime.Now)
                return Result.Validation("Can Not Edit Session That Has Already Started Or Completed");

            if (model.EndDate <= model.StartDate) 
                return Result.Validation("EndDate Must be Greater Than StartDate");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(sesssionId, ct);

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Session Must be In Future");

            if (bookingCount > 0)
                return Result.Failed("Can Not Edit Session That Has Already Bookings");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);
           

            var isVaild = Enum.TryParse<Specialties>(category.CategoryName, true, out var categorySpecialty);
            if (!isVaild || trainer.Specialties != categorySpecialty) return Result.Validation("Cannot Create This Session To This Trainer");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            _unitOfWork.SessionRepository.Update(session);  
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Session Faild To Update");

        }

        public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId ,ct);
            if (session == null) return Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now) return
                    Result.Validation("Can Not Delete Session That Has Not Ended Yet");

            var booking = await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(sessionId, ct);
            if (booking > 0)
                return Result.Failed("Can Not Delete Session That Has Booking");

            _unitOfWork.SessionRepository.Delete(session);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Session Field TO Create");
        }
    }
}
