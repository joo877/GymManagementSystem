using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Interface
{
    public interface IAnalytics
    {
        public Task<AnalyticsViewModel> AnalitcAsync(CancellationToken ct=default);


    }
}
