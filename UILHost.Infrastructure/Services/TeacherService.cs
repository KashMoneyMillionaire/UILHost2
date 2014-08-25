using UILHost.Infrastructure.Domain;
using UILHost.Infrastructure.Repository;
using UILHost.Infrastructure.Services.Interface;
using UILHost.Repository.Pattern.Repositories;
using UILHost.Service.Pattern;

namespace UILHost.Infrastructure.Services
{
    public class TeacherService : Service<Teacher>, ITeacherService
    {
        private readonly IRepositoryAsync<Teacher> _repository; 
        public TeacherService(IRepositoryAsync<Teacher> repository) : base(repository)
        {
            _repository = repository;
        }

        public Teacher Read(string email)
        {
            return _repository.Read(email);
        }

        public Teacher Read(long teacherId)
        {
            return _repository.Read(teacherId);
        }
    }
}
