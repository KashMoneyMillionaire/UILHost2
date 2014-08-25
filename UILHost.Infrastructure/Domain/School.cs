﻿using System.Collections.Generic;
using UILHost.Infrastructure.Entity;
using UILHost.Common;

namespace UILHost.Infrastructure.Domain
{
    public class School : EntityBase<long>
    {
        public string Name { get; set; }
        public Classification Classification { get; set; }
        public Address Address { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Meet> HostedMeets { get; set; }
    }
}
