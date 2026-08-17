using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.BookingViewModel;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface IBookingServices
    {

        Task<Result<IEnumerable<SessionViewModel>>> GetAllBookingSessionAsync(CancellationToken ct = default);

        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetAllMembersSessionAsyc(int sessionId, CancellationToken ct = default);

        Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMemberForDropDownListAsync( int sessionId,CancellationToken ct =default);
        Task<Result> CreateBookingAsync(CreateMemberBookingViewModel model , CancellationToken ct = default);

        Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct= default); 
        Task<Result> CanselBookingAsync(int memberId, int sessionId, CancellationToken ct= default); 

    }
}
