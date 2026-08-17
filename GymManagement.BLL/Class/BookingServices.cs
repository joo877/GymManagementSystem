using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.BookingViewModel;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingServices(IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllBookingSessionAsync( CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory(s => s.EndDate >= DateTime.Now,ct);
            var sessionsMapped = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in sessionsMapped)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(session.Id, ct);
            }
            return Result<IEnumerable<SessionViewModel>>.OK(sessionsMapped);
        }

        public async Task<Result<IEnumerable<MemberForSessionViewModel>>> GetAllMembersSessionAsyc(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBooKingBySessionIdAsyc(sessionId, ct);
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            var bookingsMapped = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
            foreach (var item in bookingsMapped)
            {
                item.IsAttended = session?.StartDate > DateTime.Now ? false : item.IsAttended; 
            }

            return Result<IEnumerable<MemberForSessionViewModel>>.OK(bookingsMapped);
        }

        public async Task<Result> CanselBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var sessiion = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId , ct);
            if (sessiion == null)
                return Result.NotFound("session Not Found");
            if (sessiion.StartDate <= DateTime.Now)
                return Result.Failed("Can Not Cansel Booking For Session Already Started");

            var booking = await _unitOfWork.BookingRepository.FristOrDefualtAsync(b => b.MemberId == memberId && b.SessionId == sessionId ,ct:ct);
            if (booking == null)
                return Result.NotFound("Booking Not Found");
            _unitOfWork.BookingRepository.Delete(booking);
            return await _unitOfWork.SaveChangAsync() > 0 ? Result.OK() : Result.Failed("Faield to Cansel this booking");


        }

        public async Task<Result> CreateBookingAsync(CreateMemberBookingViewModel model, CancellationToken ct = default)
        {

            var session = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId);
            if (session is null)
                Result.NotFound("Session Is Not Found");
            if (session!.StartDate <= DateTime.Now) return Result.Failed(" Can Not Booked Session Already Started");

            var activeShip = await _unitOfWork.MembeshipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now   , ct);

            if (!activeShip) return Result.Failed("You Don't Have Active Membership");

            var isAlreadyBooked = await _unitOfWork.BookingRepository.AnyAsync(b=> b.MemberId == model.MemberId && b.SessionId == model.SessionId  ,ct);
            if (isAlreadyBooked) return Result.Failed("You Already Booked This Session");

            var bookedSlots = await _unitOfWork.SessionRepository.GetCountOfSlotsAsync(model.SessionId,ct);
            if (bookedSlots >= session.Capacity)
                return Result.Failed("Session is Not Capacity");

            var sessionMapped = _mapper.Map<CreateMemberBookingViewModel, Booking>(model);
            _unitOfWork.BookingRepository.Add(sessionMapped);
            var result = await _unitOfWork.SaveChangAsync(ct);
            return result > 0 ? Result.OK() : Result.Failed("Booking Failed To Create");


        }

         public async Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMemberForDropDownListAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(b => b.SessionId == sessionId, ct: ct);
            var bookingMemberIds = bookings.Select(b => b.MemberId);
            var availableMembers = await _unitOfWork.GetRepository<Member>().GetAllAsync(m => !bookingMemberIds.Contains(m.Id));
              var memberMapped = _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembers);
            return Result<IEnumerable<MemberSelectListViewModel>>.OK(memberMapped);


        }

        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.FristOrDefualtAsync(b => b.MemberId == memberId && b.SessionId == sessionId, true, ct);
            if (booking == null)
                return Result.NotFound("Booking Not Found");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;

            _unitOfWork.BookingRepository.Update(booking);
           return  await _unitOfWork.SaveChangAsync() > 0 ? Result.OK() : Result.Failed("Faield to marke this member to attended") ;
        }
    }
}
