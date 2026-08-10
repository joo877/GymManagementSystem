using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionServices _session;

        public SessionController(ISessionServices session)
        {
            _session = session;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _session.GetAllSessionAsync(ct);

            return View(sessions.value);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _session.GetSessionById(id, ct);
            if (result.Success)
          
                return View(result.value);

        
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
        
        }

        #region CreateSession
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct) 
        {
            await  PopulateDropDwonListAsync();
        return View();
        
        } 

        [HttpPost]
        public async Task<IActionResult> Create(CreateSesssionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) 
            {
               await PopulateDropDwonListAsync();
            return View(model);
            }
          var result = await _session.CreateSessionAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
               return RedirectToAction(nameof(Index));
        
            }
          
                TempData["ErrorMessage"] = result.Error;
            await PopulateDropDwonListAsync();
            return View(model);
        }

        private async Task PopulateDropDwonListAsync()
        {
            var trainerList = await _session.GetTrainerForDropDwonListAsync();
            var categoryLst = await _session.GetCategoryForDropDwonListAsync();
            ViewBag.Trainer = new SelectList(trainerList.value, "Id", "Name");
            ViewBag.Category = new SelectList(categoryLst.value, "Id", "CategoryName");

        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _session.GetSessionToUpdateAsync(id, ct);
            if (result.Success) 
            {
                var List = await _session.GetTrainerForDropDwonListAsync();
                ViewBag.Trainer = new SelectList(List.value, "Id", "Name");
                return View(result.value);
            }

            else
            {
                TempData["ErrorMessage"] = result.Error;
              return  RedirectToAction(nameof(Index));
            
            }
     
          
        
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                var List = await _session.GetTrainerForDropDwonListAsync();
                ViewBag.Trainer = new SelectList(List.value, "Id", "Name");
                return View(model);

            }


            var result = await _session.UpdateSessionAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                var List = await _session.GetTrainerForDropDwonListAsync();
                ViewBag.Trainer = new SelectList(List.value, "Id", "Name");
                TempData["ErrorMessage"] = result.Error;
                return View(model);

            }
        
        
        }


        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _session.GetSessionById(id, ct);
            if (result.Success)
            {
                return View();

            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
        
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _session.DeleteSessionAsync(id, ct);
          
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "Session Deleted succussfully" : result.Error;
            return RedirectToAction(nameof(Index));

        }

        #endregion


    }
}
