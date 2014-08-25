using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using UILHost.Infrastructure.Domain;
using UILHost.Infrastructure.Services.Interface;
using UILHost.Repository.Pattern.UnitOfWork;
using UILHost.Web.Models;

namespace UILHost.Web.Controllers
{
    public partial class MeetController : Controller
    {
        private readonly IUnitOfWorkAsync _uow;
        private readonly IMeetService _meetSvc;
        private readonly SessionFacade _session;

        public MeetController(IUnitOfWorkAsync uow, 
            IMeetService meetSvc, SessionFacade session)
        {
            _uow = uow;
            _meetSvc = meetSvc;
            _session = session;
        }

        [Authorize]
        public virtual ActionResult Hosted(DateTime? sortDate)
        {
            sortDate = sortDate == null ? DateTime.Now.AddDays(-30).Date : sortDate.Value.Date;

            var meet = _uow.RepositoryAsync<Meet>()
                .Queryable()
                .Where(m => m.StartTime >= sortDate)
                .OrderByDescending(m => m.StartTime);

            return View(meet.ToList());
        }

        [HttpGet, Authorize]
        public virtual ActionResult Participating()
        {
            var meets =
                _uow.RepositoryAsync<Meet>()
                    .Queryable()
                    .Include(m => m.CompetingSchools)
                    .Where(m => m.CompetingSchools.Select(c => c.School.Id).Contains(1));
            return View(meets.ToList());
        }

        [HttpGet, Authorize]
        public virtual ActionResult Run(long id)
        {
            if (id == 0)
            {
                return RedirectToAction(MVC.Meet.Hosted());
            }

            var meet = _uow.RepositoryAsync<Meet>().Queryable()
                .Include(m => m.CompetingSchools.Select(s => s.School))
                .Include(m => m.MeetEvents.Select(e => e.Event)).First(m => m.Id == id);

            var model = Mapper.Map<EditMeetViewModel>(meet);
            model.Events = _uow.RepositoryAsync<Event>().Queryable().ToList();

            return View(model);
        }

        [HttpPost, Authorize]
        public virtual ActionResult Run(EditMeetViewModel model)
        {
            // Check for model state errors
            var isError = false;
            if (!model.MeetEvents.Any(m => m.IsSelected))
            {
                ModelState.AddModelError("", "Select at least one event to compete in.");
                isError = true;
            }
            if (!_meetSvc.IsNameUniqure(model.Name, _session.CurrentTeacher.School.Id))
            {
                ModelState.AddModelError("", "The meet name cannot be the same as a past meet.");
                isError = true;
            }
            if (isError)
            {
                model.CompetingSchools = _uow.RepositoryAsync<MeetSchool>().Queryable()
                    .Include(m => m.School).Where(m => m.Meet.Id == model.MeetId).ToList();
                return View(model);
            }

            _meetSvc.CreateNewMeet(model.Name, model.IsHostCompeting, model.StartTime, model.EndTime,
                model.MeetEvents.Where(m => m.IsSelected).Select(m => m.EventId).ToArray(), _session.CurrentTeacher.School);

            _uow.SaveChanges();

            return RedirectToAction(MVC.Meet.Hosted());
        }

        [HttpGet, Authorize]
        public virtual ActionResult Add()
        {
            var model = new NewMeetViewModel
            {
                StartTime = DateTime.Today.Add(new TimeSpan(8,0,0)),
                EndTime = DateTime.Today.Add(new TimeSpan(18, 0, 0)),
                MeetEvents = Mapper.Map<List<MeetEventViewModel>>(_uow.RepositoryAsync<Event>().Queryable().ToList())
            };
            return View(model);
        }

        [HttpPost, Authorize]
        public virtual ActionResult Add(NewMeetViewModel model)
        {
            // Check for model state errors
            var isError = false;
            if (!model.MeetEvents.Any(m => m.IsSelected))
            {
                ModelState.AddModelError("", "Select at least one event to compete in.");
                isError = true;
            }
            if (!_meetSvc.IsNameUniqure(model.Name, _session.CurrentTeacher.School.Id))
            {
                ModelState.AddModelError("", "The meet name cannot be the same as a past meet.");
                isError = true;
            }
            if(isError)
                return View(model);

            _meetSvc.CreateNewMeet(model.Name, model.IsHostCompeting, model.StartTime, model.EndTime,
                model.MeetEvents.Where(m => m.IsSelected).Select(m => m.EventId).ToArray(), _session.CurrentTeacher.School);

            _uow.SaveChanges();

            return RedirectToAction(MVC.Meet.Hosted());
        }
    }
}