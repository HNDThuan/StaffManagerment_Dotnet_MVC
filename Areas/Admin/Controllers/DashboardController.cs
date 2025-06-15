using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StaffManagement.Areas.Admin.Controllers
{

    public class DashboardController : AdminBaseController
    {
        // GET: DashboardController
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult CreateQRCode()
        {
            return View();
        }
    }
}
