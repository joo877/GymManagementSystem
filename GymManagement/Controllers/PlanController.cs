using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanServices _plan;

        public PlanController(IPlanServices plan) 
        {
            _plan = plan;
        }

     
        
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var plans = await _plan.GetAllPlanAsync(ct);
           
            return View(plans.value);
        }



        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {

            var planDetails = await _plan.GetPlanByIdAsync(id, ct);
            if (planDetails.Success)
            {
                return View(planDetails.value);
            
            }
            TempData["ErrorMessage"] = planDetails.Error;
            return RedirectToAction(nameof(Index));
        }

        #region Edit plan


       
        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct)
        {
            var plan = await _plan.GetPlanToUpdate(id, ct);
            if (plan.Success)
            {
            return View(plan.value);
            }

            TempData["ErrorMessage"] = plan.Error;
            return RedirectToAction(nameof(Index));

        }

      
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _plan.UpdataPlanAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
            }
            return RedirectToAction(nameof(Index));
        
        }

        #endregion

       

        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id, CancellationToken ct)
        {
            var result = await _plan.SoftDeletePlanAsync(id, ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Plan Status Change ";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
            }
            return RedirectToAction(nameof(Index));
        
        }

    }
}
