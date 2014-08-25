using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using UILHost.Common.AutoMapper;
using UILHost.Infrastructure.Domain;

namespace UILHost.Web.Models
{
    public class NewMeetViewModel : IMap<Meet>
    {
        [Required]
        public string Name { get; set; }

        [DisplayName("Add yourself to competing schools?")]
        public bool IsHostCompeting { get; set; }

        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

        public List<MeetEventViewModel> MeetEvents { get; set; }
    }

    public class MeetEventViewModel : IHaveCustomMappings
    {
        public long EventId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Event, MeetEventViewModel>()
                .ForMember(d => d.EventId, opt => opt.MapFrom(s => s.Id));
            configuration.CreateMap<MeetEvent, MeetEventViewModel>()
                .ForMember(d => d.EventId, opt => opt.MapFrom(s => s.Event.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Event.Name))
                .ForMember(d => d.IsSelected, opt => opt.MapFrom(s => true));
        }
    }

    public class EditMeetViewModel : IHaveCustomMappings
    {
        public long MeetId { get; set; }
        public List<MeetSchool> CompetingSchools { get; set; }
        public string Name { get; set; }
        public List<MeetEventViewModel> MeetEvents { get; set; }
        public List<Event> Events { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [DisplayName("Add yourself to competing schools?")]
        public bool IsHostCompeting { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Meet, EditMeetViewModel>();
        }
    }
}