using UILHost.Infrastructure.Domain;
using UILHost.Service.Pattern;

namespace UILHost.Infrastructure.Services.Interface
{
    public interface ITeacherService : IService<Teacher>
    {
        Teacher Read(string email);
        Teacher Read(long teacherId);
    }
}
