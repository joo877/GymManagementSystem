using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface ISessionServices
    {

        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionAsync(CancellationToken ct= default);
        Task<Result<SessionViewModel>> GetSessionById(int sessionId , CancellationToken ct= default);
        Task<Result> CreateSessionAsync(CreateSesssionViewModel model, CancellationToken ct = default);

        Task<Result<IEnumerable<TrainerSelectViewModel>>> GetTrainerForDropDwonListAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<CategorySelectViewModel>>> GetCategoryForDropDwonListAsync(CancellationToken ct= default);

        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId , CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int sesssionId, UpdateSessionViewModel model , CancellationToken ct =default);

        Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct= default);



        
    }
}
