using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Xml.Linq;
using Trial.DatabaseContent;
using Trial.Models;
using WPFClassLibrary2;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Trial.Models;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;

namespace Trial.Controllers
{
    public class ModulesController : Controller
    {
        CalculateModuleData dc = CalculateModuleData.Instance;
        PROG6212Database dbcontent = new PROG6212Database();
        SingletonStudentIDHolder IDHolder = SingletonStudentIDHolder.Instance();
        // GET: ModulesController
        public ActionResult Index()
        {
            List<Models.Modules> modlist=dbcontent.GetModule(IDHolder.getid());
            return View(modlist);
        }

        // GET: ModulesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModulesController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Graph(string id)
        {
            List<Models.StudyLogs> studlogs = dbcontent.GetStudyLogsByCode(IDHolder.getid(),id);
            return View(studlogs);
        }
        // POST: ModulesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Modules modules)
        {
            try
            {

                int _credit = Convert.ToInt32(modules.NoOfCredits);
                int _classhrs = Convert.ToInt32(modules.ClassHrsPerWeek);
                int _Totalhrs = CalculateModuleData.calculations(_credit, _classhrs, modules.NoofWeeks);

                dbcontent.ModuleInsert(modules.Code, modules.Name, _credit, _classhrs, _Totalhrs, IDHolder.getid());


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult StudySessions(string id)
        {
            Models.Modules mod = dbcontent.getmodulebyid(id,IDHolder.getid());
            IDHolder.code = id;
            return View(mod);
        }

        public int CalcuHours(string code)
        {
            int answer = 0;
            Models.Modules mod = dbcontent.getmodulebyid(code, IDHolder.getid());
            answer = mod.RemainingHoursOfStudy;
            return answer;
        }

        // POST: ModulesController/Edit/5
        [HttpPost]
        public ActionResult CreateStudySessions(string id, Models.StudyLogs stlogs)
        {
            try
            {
                string _code = IDHolder.code;
                int _hours = Convert.ToInt32(stlogs.RemainingHoursOfStudy);
                int originalhrs = CalcuHours(_code);
                dbcontent.UpdateModuleList(IDHolder.getid(), _code, (originalhrs - _hours));

                DateTime dt = stlogs.Date;
                Random random = new Random();
                dbcontent.StudySessionInsert(random.Next(0, 10000), _code, _hours, dt, IDHolder.getid());


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ModulesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModulesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModulesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModulesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
