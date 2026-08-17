using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class MemberShipServices : IMemberShipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipServices(IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result> CreatMemberShipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {

            var memberExsit = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Id == model.MemberId,ct);
            if (!memberExsit) return Result.NotFound("member is Not Exsist"); 

            var planExsit = await _unitOfWork.GetRepository<Plan>().AnyAsync(x => x.Id == model.PlanId ,ct);
            if (!planExsit) return Result.NotFound("Plan is Not Exsist");

            var hasActiveMemberShip = await _unitOfWork.MembeshipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.UtcNow, ct);
            if (hasActiveMemberShip) return Result.Validation("Member Already Has Active MemberShip");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct);
            if (!plan.IsActive) return Result.Validation("Plan Is Not Active");


            var membershipMapped = _mapper.Map<MemberShip>(model);
            membershipMapped.EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.DurationDays);
            _unitOfWork.MembeshipRepository.Add(membershipMapped);
           var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Membership Failed To Create");
             
        }

        public async Task<Result> DeleteMembershipAsync(int memberid, CancellationToken ct = default)
        {
            

            var activeMembership = await _unitOfWork.MembeshipRepository.FristOrDefualtAsync(m => m.MemberId == memberid && m.EndDate > DateTime.UtcNow,true,ct);
            if(activeMembership is null)
            return Result.Validation("MemberShip Not Found For This Member");

            _unitOfWork.MembeshipRepository.Delete(activeMembership);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK():Result.Failed("MemberShip Failed To Deleted");

        }

        public async Task<Result<IEnumerable<MemberShipViewModel>>> GetAllMemberShipAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembeshipRepository.GetMembershipWithMemberAndPlanAsync( m => m.EndDate > DateTime.UtcNow , ct);
            var membershipsMapped = _mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
            return Result<IEnumerable<MemberShipViewModel>>.OK(membershipsMapped);

        }

        public async Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMembersToDropDownListAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            var membermaped = _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
            return Result<IEnumerable<MemberSelectListViewModel>>.OK(membermaped);
        }

        public async Task<Result<IEnumerable<PlanSelectListViewModel>>> GetPlansToDropDownListAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            var plansMapped = _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
            return Result<IEnumerable<PlanSelectListViewModel>>.OK(plansMapped);
        }
    }
}
