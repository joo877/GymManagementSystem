using AutoMapper;
using GymManagement.BLL.AttachMemnt;
using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberServices(IUnitOfWork unitOfWork, IMapper mapper , IAttachmentService attachmentService )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            //check email exsist
            var emailExsist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email ,ct);
            //check phone exsist
            var phoneExsist =await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone ,ct);
            //email or phone exsist return fasle
            if (emailExsist || phoneExsist) return Result.Validation("Phone or Email Don't Exsist"); ;

            var member = _mapper.Map<Member>(model);

            //upload Photo
            var fileName = await _attachmentService.UploadAsyc(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "MemberPhotos");
            if (string.IsNullOrEmpty(fileName.value))
                return Result.NotFound("File Name Not Found");

            // add member 
            _unitOfWork.GetRepository<Member>().Add(member);
            member.Photo = fileName.value;
            var result = await _unitOfWork.SaveChangAsync(ct);
            if (result > 0)
                return Result.OK();

            else
            {
                _attachmentService.Delete("MemberPhotos", fileName.value);

                return Result.Failed("Member Field to Create");
            }
            
            
        }

        public async Task<Result<IEnumerable<MemberViewModel>>> GetAllMemberAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);

            if (!members.Any()) return Result<IEnumerable<MemberViewModel>>.NotFound("Members Not Found");

            var memberViewModel = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return Result<IEnumerable<MemberViewModel>>.OK(memberViewModel);
        }

         public async Task<Result<MemberViewModel>> GetMemberByIdAsync(int id, CancellationToken ct)
        {
           var member =await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member == null) return Result<MemberViewModel>.NotFound("Member Not Found");

            var MemberDetails = _mapper.Map<MemberViewModel>(member);

            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>().FristOrDefualtAsync(m => m.MemberShipsId == id && m.EndDate > DateTime.Now);

            if (activeMemberShip is not null)
            {

                MemberDetails.MembershipStartDate = activeMemberShip.CreatedAt.ToString();
                   MemberDetails.MembershipEndDate = activeMemberShip.EndDate.ToString();
              var activePlane= await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMemberShip.PlanId,ct);
                MemberDetails.PlanName = activePlane!.Name;

                //MemberDetails.PlanName = memberShips.Plan.Name;
                    

               
            
            }
            return Result<MemberViewModel>.OK(MemberDetails);


        }

        public async Task<Result<HealthRecordViewModel>> GetMemberHealthRecordDetailsAsync(int memberid, CancellationToken ct)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FristOrDefualtAsync(h => h.MemberId == memberid, ct:ct );
            if (healthRecord == null) return Result<HealthRecordViewModel>.NotFound("Health Record Not Found");
            var healthRecordMaped = _mapper.Map<HealthRecordViewModel>(healthRecord);
            return Result<HealthRecordViewModel>.OK(healthRecordMaped);
        }

        public async Task<Result<UpdateMemberViewModel>> GetMemberToUpdate(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return Result<UpdateMemberViewModel>.NotFound("Member Not Found");
            var memberMaped=  _mapper.Map<UpdateMemberViewModel>(member);
            return Result<UpdateMemberViewModel>.OK(memberMaped);
        }

        public async Task<Result> UpdateMemberDetails(int memberid, UpdateMemberViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberid, ct);
            if (member == null) return Result.NotFound("Member Not FOund");

            var EmailExsist =await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberid);
            var PhoneExsist =await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberid);
            if (EmailExsist || PhoneExsist) return Result.Validation("Phone Or Number Already Exsist");

            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

          _unitOfWork.GetRepository<Member>().Update(member);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ?Result.OK() : Result.Failed("Member Failed To Update") ;

        }


        public async Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId);
            if (member == null) return Result.NotFound("Member Not FOund");

            var hasFutureSessionBooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(x => x.MemberId == memberId && x.Session.StartDate > DateTime.Now,ct);
            if (hasFutureSessionBooking) return Result.Validation("Can Not Deleted Member Has Bookings");

         _unitOfWork.GetRepository<Member>().Delete(member);
            var result = await _unitOfWork.SaveChangAsync(ct);
          return result > 0 ?  Result.OK() : Result.Failed("Member Failed To Deleted");
        }

    }
}
