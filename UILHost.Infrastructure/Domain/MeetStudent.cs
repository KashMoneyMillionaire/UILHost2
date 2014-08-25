using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILHost.Infrastructure.Entity;

namespace UILHost.Infrastructure.Domain
{
    public class MeetStudent : EntityBase<long>
    {
        public MeetSchool MeetSchool { get; set; }
        public Student Student { get; set; }
    }
}
