using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface IMemberServices
    {
        Task<Result<IEnumerable<MemberViewModel>>> GetAllMemberAsync(CancellationToken ct =default);

        Task<Result> CreateMemberAsync( CreateMemberViewModel model  , CancellationToken ct = default);

        Task<Result<MemberViewModel>> GetMemberByIdAsync(int id, CancellationToken ct = default);

        Task<Result<HealthRecordViewModel>> GetMemberHealthRecordDetailsAsync(int memberid , CancellationToken ct =default );

        Task<Result<UpdateMemberViewModel>> GetMemberToUpdate(int memberId, CancellationToken ct = default);

        Task<Result> UpdateMemberDetails(int memberid ,UpdateMemberViewModel model, CancellationToken ct =default );

        Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default);

    }
}
