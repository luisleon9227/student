using Colegio.Models;

namespace Colegio.Services.Interfaces
{
    public interface IEstudent
    {
        public List<EstudentDto> GetEstudents();

        public EstudentDto GetEstudentByIdeentification(int identification);

        public bool CreateEstudent(EstudentDto estudent);
        public bool DeleteEstudent(Guid id);

        public EstudentDto FindEstudent(Guid id);
    }
}
