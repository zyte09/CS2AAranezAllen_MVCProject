using CS2AAranezAllen_MVCProject.BusLogic.Model;
using CS2AAranezAllen_MVCProject.BusLogic.Repository;


namespace CS2AAranezAllen_MVCProject.BusLogic.Service
{
    public class StudentService 
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();

        public IEnumerable<tblStudent> GetAll()
        {
            return _studentRepository.GetAll();

        }
    }
}
