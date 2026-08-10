using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerServices _trainer;

        public TrainerController(ITrainerServices trainer)
        {
            _trainer = trainer;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainer.GetAllTrainerAsync(ct);


            return View(trainers.value);

        }

        public async Task<IActionResult> TrainerDetails(int Id, CancellationToken ct)
        {
            var trainer = await _trainer.GetTrainerById(Id, ct);
            if (trainer.Success)
            {
                return View(trainer.value);

            }
            TempData["ErrorMessage"] = trainer.Error;
            return RedirectToAction(nameof(Index));

        }

        #region Create Trainer
        [HttpGet]
        public IActionResult Create(CancellationToken ct) => View();

        [HttpPost]
        public async Task<IActionResult> Create(TrainerCreateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainer.CreateTrainerrAsync(model, ct);
          
          TempData[result.Success ? "SuccessMessage" : "ErrorMessage"]= result.Success? "Trainer Created Successfully" : result.Error;
                return RedirectToAction(nameof(Index));








        }
            #endregion



        #region ُEdit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var Trainer = await _trainer.GetTrainerToUpdateAsync(id, ct);
            if (Trainer.Success)
            {
                return View(Trainer.value);

            }
            TempData["ErrorMessage"] = Trainer.Error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateViewModel model, CancellationToken ct)
        {
            var result = await _trainer.UpdateTrainerAsync(id, model, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "Trainer Updated Successfully" : result.Error;

            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete() => View();




        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainer.DeleteTrainerAsync(id, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "Trainer Created Successfully" : result.Error;
            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
