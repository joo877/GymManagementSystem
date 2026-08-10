using GymManagement.BLL.Common;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels;
using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Class
{
    public class Analytics : IAnalytics
    {
        private readonly IUnitOfWork _unitOfWork;

        public Analytics(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> AnalitcAsync(CancellationToken ct)
        {
            var now = DateTime.Now;
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var activeMembers = await _unitOfWork.GetRepository<MemberShip>().CountAsync(x => x.EndDate >now ,ct);
            var trainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var UPcomingSession = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > now, ct);
            var OngoningSession = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate <= now && s.EndDate >=now, ct);
            var CompletedSession = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate< now, ct);


            var Analytics = new AnalyticsViewModel()
            {

                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                Trainers = trainers,
                CompletedSession = CompletedSession,
                OngoningSession = OngoningSession,
                UPcomingSession = UPcomingSession

            };

            return Analytics;



        }
    }
}
