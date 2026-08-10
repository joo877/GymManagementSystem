using GymManagement.BLL.AttachMemnt;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberServices _member;
        private readonly IAttachmentService _attachmentService;

        public MemberController(IMemberServices member , IAttachmentService attachmentService)
        {
           _member = member;
            _attachmentService = attachmentService;
        }

        #region Get Photo
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _member.GetMemberByIdAsync(id);
            if (!member.Success ||string.IsNullOrEmpty(member.value.Photo))
                return NotFound(member.Error);

            var result = _attachmentService.GetFile("MemberPhotos", member.value.Photo);
            if (!result.Success)

                return NotFound(result.Error);

            return File(result.value.Stream,result.value.contantType);
        
        }

        #endregion

        public async Task<IActionResult> index(CancellationToken ct)
        {
            var members = await _member.GetAllMemberAsync(ct);
            return View(members.value);

        }



        // GET BaseUrl/ Member/ Details / {id}
        // MemberDetails -> Show one member's details
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {

            var member= await _member.GetMemberByIdAsync(id, ct);

            if (member.Success)

                return View(member.value);

            else 
            {
                TempData["Error Message"] = member!.Error;
            return RedirectToAction(nameof(index));
            
            }



        }



        //  GET  BaseUrl/Member/HealthRecordDetails / {id}
        // HealthRecordDetails ->  Show one member's HealthRecordDetails
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecordDetails =  await _member.GetMemberHealthRecordDetailsAsync(id, ct);
            if (healthRecordDetails == null)
            {
                TempData["Error Message"] = healthRecordDetails!.Error;
                return RedirectToAction(nameof(index));
            }
            return View(healthRecordDetails.value);
          
        
        }

        #region Create Member
        // GET   BaseUrl/Member/Create
        // Create -> Show empty form
        [HttpGet]
        public IActionResult Create() => View();





        // Post BaseUrl/Member/Create
        // CreateMember -> Submit Form 
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct )
        {

            if (!ModelState.IsValid) return View(nameof(Create), model);
          

                var result = await _member.CreateMemberAsync(model, ct);

            if (result.Success)
            {
                TempData["Success Message"] = "Member Created Successfully";
            }
            else
            {
                TempData["Error Message"] = result.Error;
            }

                return RedirectToAction(nameof(index));
            
            

        
        }

        #endregion

        #region Edit Member

        // GET   BaseUrl/Member/Edit /{id}
        // Edit -> Show form pre-filled
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _member.GetMemberToUpdate(id, ct);
            if (member is null)
            {

                TempData["Error Message"] = member.Error;
                return RedirectToAction(nameof(index));
            }
            return View(member.value);
        
        }


        // Post BaseUrl/Member/Edit / {id}
        // CreateMember -> Submit Form 
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id,UpdateMemberViewModel model ,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _member.UpdateMemberDetails(id, model, ct);
            if (result.Success)
            {

                TempData["Success Message"] = "Member Updated Successfully";
            }
            else
            {
                TempData["Error Message"] = result.Error;
            }

            return RedirectToAction(nameof(index));
        
        }

        #endregion

        #region Delete Member


        // GET   BaseUrl/Member/Delete /{id}
        // Delete -> Show  Confirm form
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _member.GetMemberByIdAsync(id,ct);
            if (member == null)
            {
                TempData["Error Message"] = member!.Error;
                return RedirectToAction(nameof(index));

            }

            return View();
        }

        // Post BaseUrl/Member/DeleteConfirmed / {id}
        // DeleteConfirmed -> Submit Form 
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _member.DeleteMemberAsync(id, ct);
            if (result.Success)
            {

                TempData["Success Message"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["Error Message"] = result.Error;
            }

            return RedirectToAction(nameof(index));

        }
        #endregion
    }

}
