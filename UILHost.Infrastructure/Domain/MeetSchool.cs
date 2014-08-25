using System.Collections.Generic;
using UILHost.Infrastructure.Entity;

namespace UILHost.Infrastructure.Domain
{
    public class MeetSchool : EntityBase<long>
    {
        public Meet Meet { get; set; }
        public School School { get; set; }
        public List<MeetStudent> MeetStudents { get; set; } 
    }
}
