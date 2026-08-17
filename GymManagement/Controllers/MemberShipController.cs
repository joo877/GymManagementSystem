using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipServices _memberShipServices;

        public MemberShipController(IMemberShipServices memberShipServices)
        {
            _memberShipServices = memberShipServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberships = await _memberShipServices.GetAllMemberShipAsync(ct);
            return View(memberships.value);



        }


        #region CreateMemberShip


        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDwonListAsync(ct);
            return View();
        }
         



        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDwonListAsync(ct);
                return View(model);

            } 

            var result = await _memberShipServices.CreatMemberShipAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
                return RedirectToAction(nameof(Index));
            
            }

            TempData["ErrorMessage"] = result.Error;
            await  PopulateDropDwonListAsync(ct);
            return View(model);

          

        }
        private async Task PopulateDropDwonListAsync( CancellationToken ct)
        {
            var memberListDropDowm = await _memberShipServices.GetMembersToDropDownListAsync(ct);
            var planListDropDowm = await _memberShipServices.GetPlansToDropDownListAsync(ct);

            ViewBag.Members = new SelectList(memberListDropDowm.value, "Id", "Name");
            ViewBag.Plans = new SelectList(planListDropDowm.value, "Id", "Name");

        }


        #endregion


        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await _memberShipServices.DeleteMembershipAsync(id, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "MemberShip Deleted Successfully" : result.Error;

            return RedirectToAction(nameof(Index));
        
        }
    }
}
