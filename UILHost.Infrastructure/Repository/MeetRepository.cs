using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UILHost.Infrastructure.Domain;
using UILHost.Repository.Pattern.Repositories;

namespace UILHost.Infrastructure.Repository
{
    public static class MeetRepository
    {
        public static IEnumerable<Meet> Read(this IRepositoryAsync<Meet> repo, long schoolId)
        {
            return repo.Queryable()
                .Include(s => s.CompetingSchools.Select(c => c.School))
                .Where(s => s.HostSchool.Id == schoolId);
        }

        public static Meet ReadHostedMeet(this IRepositoryAsync<Meet> repo, long meetId)
        {
            return repo.Queryable()
                .Include(s => s.CompetingSchools.Select(c => c.School))
                .Include(s => s.CompetingSchools.Select(c => c.MeetStudents.Select(m => m.Student)))
                .Include(s => s.HostSchool)
                .Include(s => s.MeetEvents.Select(m => m.Event))
                .First(s => s.Id == meetId);
        }

        public static IEnumerable<Meet> Read(this IRepositoryAsync<Meet> repo, string name, long schoolId)
        {
            return repo.Queryable()
                .Where(s => s.Name == name && s.HostSchool.Id == schoolId).ToList();
        }
    }
}
