using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface ITrainerServices
    {
        Task<Result<IEnumerable<TrainerviewModel>>> GetAllTrainerAsync(CancellationToken ct = default);
        Task<Result<TrainerviewModel>> GetTrainerById(int trainerId, CancellationToken ct = default);

        Task<Result> CreateTrainerrAsync(TrainerCreateViewModel model , CancellationToken ct=default);

        Task<Result<UpdateViewModel>> GetTrainerToUpdateAsync(int trainerId,CancellationToken ct = default);
        Task<Result> UpdateTrainerAsync(int trainerId, UpdateViewModel model , CancellationToken ct = default);
        Task<Result> DeleteTrainerAsync(int trainerId , CancellationToken ct = default);

    }
}
