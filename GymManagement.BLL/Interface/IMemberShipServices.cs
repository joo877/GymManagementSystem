using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface IMemberShipServices
    {
        Task<Result<IEnumerable<MemberShipViewModel>>> GetAllMemberShipAsync(CancellationToken ct = default);

        Task<Result> CreatMemberShipAsync(CreateMemberShipViewModel model , CancellationToken ct = default);

        Task<Result<IEnumerable<PlanSelectListViewModel>>> GetPlansToDropDownListAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberSelectListViewModel>>> GetMembersToDropDownListAsync(CancellationToken ct = default);

        Task<Result> DeleteMembershipAsync(int memberid, CancellationToken ct = default);

    }
}
