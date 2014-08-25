using System;
using System.Collections.Generic;
using System.Linq;
using UILHost.Infrastructure.Domain;
using UILHost.Infrastructure.Repository;
using UILHost.Infrastructure.Services.Interface;
using UILHost.Repository.Pattern.Repositories;
using UILHost.Repository.Pattern.UnitOfWork;
using UILHost.Service.Pattern;

namespace UILHost.Infrastructure.Services
{
    public class MeetService : Service<Meet>, IMeetService
    {
        private readonly IRepositoryAsync<Meet> _repo;
        private readonly IUnitOfWorkAsync _uow;

        public MeetService(IRepositoryAsync<Meet> repo, IUnitOfWorkAsync uow)
            : base(repo)
        {
            _repo = repo;
            _uow = uow;
        }

        public IEnumerable<Meet> Read(long schoolId)
        {
            return _repo.Read(schoolId);
        }

        public Meet ReadHostedMeet(long meetId)
        {
            return _repo.ReadHostedMeet(meetId);
        }

        public bool IsNameUniqure(string name, long schoolId)
        {
            return !_repo.Read(name, schoolId).Any();
        }

        public void CreateNewMeet(string name,
            bool isHostCompeting,
            DateTime startTime,
            DateTime endTime,
            long[] toArray,
            School hostSchool)
        {
            var meet = new Meet
            {
                EndTime = endTime,
                StartTime = startTime,
                HostSchool = hostSchool,
                Name = name,
                CompetingSchools = new List<MeetSchool>(),
                MeetEvents = new List<MeetEvent>()
            };
            _uow.RepositoryAsync<Meet>().Insert(meet);

            var events = _uow.RepositoryAsync<Event>()
                .Queryable()
                .Where(e => toArray.Contains(e.Id))
                .ToList();
            var meetEvents = events.Select(e => new MeetEvent
            {
                Event = e,
                Meet = meet,
            });
            _uow.RepositoryAsync<MeetEvent>().InsertRange(meetEvents);

            if (isHostCompeting)
            {
                _uow.RepositoryAsync<MeetSchool>().Insert(new MeetSchool
                {
                    School = hostSchool,
                    Meet = meet
                });
            }
        }
    }
}
