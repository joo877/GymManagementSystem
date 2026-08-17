using GymManagement.BLL.Class;
using GymManagement.BLL.Interface;
using GymManagement.BLL.ViewModels.BookingViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingServices _bookingServices;

        public BookingController(IBookingServices bookingServices)
        {
            _bookingServices = bookingServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _bookingServices.GetAllBookingSessionAsync(ct);
            return View(sessions.value);
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(int id , CancellationToken ct)
        {
            await PopulateDropDwonListAsync(id, ct);
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberBookingViewModel model ,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDwonListAsync(model.SessionId, ct);
                return View(model);
            }


            var result = await _bookingServices.CreateBookingAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Booking Added Successfully";
                return RedirectToAction(nameof(GetMembersForUpcomingSession), new {id=model.SessionId });
            }

            TempData["ErrorMessage"] = result.Error;
            await PopulateDropDwonListAsync(model.SessionId, ct);
            return View(model);
        
        
        }

        private async Task PopulateDropDwonListAsync(int SessionId ,CancellationToken ct)
        {
            
            var memberListDropDowm = await _bookingServices.GetMemberForDropDownListAsync(SessionId, ct);

            ViewBag.Members = new SelectList(memberListDropDowm.value, "Id", "Name");
         

        }
        #endregion


        public async Task<IActionResult> GetMembersForOngoingSessions(int id ,CancellationToken ct)
        {
            var members = await _bookingServices.GetAllMembersSessionAsyc(id, ct);
            return View(members.value);
        
        
        }
        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
        {
            var members = await _bookingServices.GetAllMembersSessionAsyc(id, ct);
            return View(members.value);


        }



        public async Task<IActionResult> Attended(int memberId,int sessionId , CancellationToken ct)
        {
        
            var result = await _bookingServices.MarkAttendedAsync(memberId, sessionId, ct);
           
            
                TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ?   "Mark Attented Successfully": result.Error;



                return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
      

        }

        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {

            var result = await _bookingServices.CanselBookingAsync(memberId, sessionId, ct);


            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
            result.Success ? "Booking Canceld Successfully" : result.Error;



            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });


        }

    }
}
