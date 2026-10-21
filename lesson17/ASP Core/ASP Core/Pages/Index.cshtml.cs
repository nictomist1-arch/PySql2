using college_DB.Models;
using College_DB.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_Core.Pages
{
    public class IndexModel : PageModel
    {
        private readonly StudentRepository _student_repo;

        public List<Student> Students { get; set; } = new List<Student>();

        public IndexModel(StudentRepository student_repo)
        {
            _student_repo = student_repo;
        }
        public void OnGet()
        {
            Students = _student_repo.GetAllStudents();
        }
    }
}
