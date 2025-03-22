using CS2AAranezAllen_MVCProject.BusLogic.Model;
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

        public IActionResult AddNewStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(tblStudent student)
        {
            try
            {
                bool result = _studentService.Add(student);
                return Json(new { success = result, message = result ? "Student Added Sucessfully" : "Failed to add student" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
