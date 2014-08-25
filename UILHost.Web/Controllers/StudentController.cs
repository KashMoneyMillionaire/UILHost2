using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UILHost.Infrastructure.Domain;
using UILHost.Repository.Pattern.UnitOfWork;

namespace UILHost.Web.Controllers
{
    public partial class StudentController : Controller
    {
        private readonly IUnitOfWorkAsync _uow;
        private readonly SessionFacade _session;

        public StudentController(IUnitOfWorkAsync uow, SessionFacade session)
        {
            _uow = uow;
            _session = session;
        }

        [HttpGet, Authorize]
        public virtual ActionResult List()
        {
            var students = _uow.RepositoryAsync<Student>().Queryable()
                .Where(s => s.School.Id == _session.CurrentTeacher.School.Id).ToList();
            return View(students);
        }
    }
}