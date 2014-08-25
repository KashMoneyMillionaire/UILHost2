using System;
using System.Collections.Generic;
using UILHost.Infrastructure.Domain;
using UILHost.Service.Pattern;

namespace UILHost.Infrastructure.Services.Interface
{
    public interface IMeetService : IService<Meet>
    {
        IEnumerable<Meet> Read(long schoolId);
        Meet ReadHostedMeet(long meetId);
        bool IsNameUniqure(string name, long schoolId);
        void CreateNewMeet(string name, 
            bool isHostCompeting, 
            DateTime startTime, 
            DateTime endTime, 
            long[] toArray, 
            School hostSchool);
    }
}
