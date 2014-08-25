using UILHost.Infrastructure.Entity;

namespace UILHost.Infrastructure.Domain
{
    public class Student : EntityBase<long>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Grade { get; set; }
        public School School { get; set; }
        public bool IsActive { get; set; }

        public Student()
        {
            IsActive = true;
        }
    }
    
}
