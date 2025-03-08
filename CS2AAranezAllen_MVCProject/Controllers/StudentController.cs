using CS2AAranezAllen_MVCProject.BusLogic.Service;
using Microsoft.AspNetCore.Mvc;

namespace CS2AAranezAllen_MVCProject.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService = new StudentService();
        public IActionResult Index()
        {
            var students = _studentService.GetAll();
            return View(students);
        }
    }
}
