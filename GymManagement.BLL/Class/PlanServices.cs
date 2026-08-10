using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;

using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PlanViewModel>>> GetAllPlanAsync(CancellationToken ct)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            var plansMaped = _mapper.Map<IEnumerable<PlanViewModel>>(plans);

            return Result<IEnumerable<PlanViewModel>>.OK(plansMaped);
        }

        public async Task<Result<PlanViewModel>> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return Result<PlanViewModel>.NotFound("Plan Not Found");
            var planMpped = _mapper.Map<PlanViewModel>(plan);
            return Result<PlanViewModel>.OK(planMpped);
        }

        public async Task<Result<UpdatePlanViewModel>> GetPlanToUpdate(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan == null || !plan.IsActive) return Result<UpdatePlanViewModel>.NotFound("Plan Not FOund");
            var activeMembership = await _unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.Now, ct);
            if (activeMembership) return Result<UpdatePlanViewModel>.Failed("Can Not Update Plan Has Active Membership");
            else
            { 
                var planMapped = _mapper.Map<UpdatePlanViewModel>(plan);
                return Result<UpdatePlanViewModel>.OK(planMapped);
            
            }

           
        }

        public async Task<Result> UpdataPlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return Result.NotFound("Plan NOt FOund");
            var activeMembership = await _unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.Now,ct);
            if (activeMembership) return Result.Failed("Can Not Update Plan Has Active Membership");



            _mapper.Map(model , plan);

            plan.UpdatedAt = DateTime.Now;

          _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Can Not Updated Plan"); 

        }
        
        public async Task<Result> SoftDeletePlanAsync(int id, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return Result.NotFound("Plan NOt FOund");
            var activeMembership = await _unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == id && x.EndDate > DateTime.Now);
            if (activeMembership) return Result.Failed("Can Not Update Plan Has Active Membership");
            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Can Not Deleted Plan"); 

        }

    }
}
