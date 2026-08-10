using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class TrainerServices : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TrainerviewModel>>> GetAllTrainerAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            if (trainers == null) return Result <IEnumerable<TrainerviewModel>>.NotFound("Trainers Not Found");
            var trainersMapped = _mapper.Map<IEnumerable<TrainerviewModel>>(trainers);

            return Result<IEnumerable<TrainerviewModel>>.OK(trainersMapped);
        }

        public async Task<Result<TrainerviewModel>> GetTrainerById(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return Result<TrainerviewModel>.NotFound("Trainer Not Found");
            var trainerMapped = _mapper.Map<TrainerviewModel>(trainer);
            return Result<TrainerviewModel>.OK(trainerMapped);
        
        }


        public async Task<Result> CreateTrainerrAsync(TrainerCreateViewModel model, CancellationToken ct = default)
        {
            var EmailExsist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email);
            var PhoneExsist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone);

            if (EmailExsist || PhoneExsist) return Result.Validation("Email Or Phone Not Exsist");

            var trainerMapped = _mapper.Map<Trainer>(model);

         _unitOfWork.GetRepository<Trainer>().Add(trainerMapped);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK():Result.Failed("Trainer Failed To Create");


        }

        public async Task<Result<UpdateViewModel>> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return Result<UpdateViewModel>.NotFound("Trainer Not Found");
            var trainerMapped = _mapper.Map<UpdateViewModel>(trainer);
            return Result<UpdateViewModel>.OK(trainerMapped);
        }

        public async Task<Result> UpdateTrainerAsync (int trainerId, UpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer NOt Found");
            var EmailExsist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != trainerId);
            var PhoneExsist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t =>t.Phone == model.Phone && t.Id != trainerId);

            if (EmailExsist || PhoneExsist) return Result.Validation("Email or Phone Already Exsist");

            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;

         _unitOfWork.GetRepository<Trainer>().Update(trainer);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK(): Result.Failed("Trainer Failed To Update");
        }

        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer NOt Found"); 
            var FeutureSession = await _unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId==trainerId && s.EndDate > DateTime.Now);
            if (FeutureSession) return Result.Validation("Can Not Delte Trainer Has FeutureSession ");
           _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Trainer Failed To Deleted");

        }
    }
}
