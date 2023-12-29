using Colegio.Models;
using Colegio.Services.Interfaces;

namespace Colegio.Services
{
    public class Estudent : IEstudent
    {

        private readonly ILogger<Estudent> _logger;
        private readonly IColegioContext _dbContext;

        public Estudent(ILogger<Estudent> logger, IColegioContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<EstudentDto> GetEstudents()
        {
            List<EstudentDto> result = null;
            try
            {
                result = _dbContext.Estudents.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"error obteniendo data de estudiante {ex.Message}");
            }
            return result;
        }
        //1. Lista devolver datos de estudiante
        //2. sin datos lista vacia
        //3. Error de conexion

        public EstudentDto GetEstudentByIdeentification(int identification)
        {
            EstudentDto result = null;
            try
            {
                result = _dbContext.Estudents.Where(x => x.Identification == identification)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"error obteniendo data de estudiante {ex.Message.ToString()}");
            }
            return result;
        }

        public bool CreateEstudent(EstudentDto estudent)
        {
            bool result = false;
            try
            {
                estudent.Id = Guid.NewGuid();
                _dbContext.Estudents.Add(estudent);
                _dbContext.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error creando data de user {ex.Message.ToString()}");
            }
            return result;
        }

        public bool DeleteEstudent(Guid id)
        {
            bool result = false;
            try
            {
                EstudentDto estudent = FindEstudent(id);
                if (estudent != null)
                {
                    _dbContext.Estudents.Remove(estudent);
                    _dbContext.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error obteniendo data de user {ex.Message.ToString()}");
            }
            return result;
        }

        public EstudentDto FindEstudent(Guid id)
        {
            return _dbContext.Estudents.Find(id);
        }
    }
}
