using Colegio.Models;
using Microsoft.EntityFrameworkCore;

namespace Colegio
{
    public interface IColegioContext
    {
        DbSet<EstudentDto> Estudents { get; set; }
        DbSet<GradeDto> Grades { get; set; }

        void SaveChanges();
    }
}