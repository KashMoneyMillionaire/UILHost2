using System.Data.Entity;
using System.Linq;
using UILHost.Infrastructure.Domain;
using UILHost.Repository.Pattern.Repositories;

namespace UILHost.Infrastructure.Repository
{
    public static class TeacherRepository
    {
        public static Teacher Read(this IRepositoryAsync<Teacher> repo, string email)
        {
            return repo.Queryable()
                .Include(t => t.School)
                .First(s => s.Email == email);
        }

        public static Teacher Read(this IRepositoryAsync<Teacher> repo, long teacherId)
        {
            return repo.Queryable()
                .Include(t => t.School)
                .FirstOrDefault(s => s.Id == teacherId);
        }
    }
}
