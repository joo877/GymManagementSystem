using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface IPlanServices
    {
        Task<Result<IEnumerable<PlanViewModel>>> GetAllPlanAsync(CancellationToken ct=default);
        Task<Result<PlanViewModel>> GetPlanByIdAsync(int id, CancellationToken ct = default);
        Task<Result<UpdatePlanViewModel>> GetPlanToUpdate(int id, CancellationToken ct = default);

        Task<Result> UpdataPlanAsync(int id ,UpdatePlanViewModel model ,CancellationToken ct= default);
        Task<Result> SoftDeletePlanAsync(int id, CancellationToken ct=default);


    }
}
