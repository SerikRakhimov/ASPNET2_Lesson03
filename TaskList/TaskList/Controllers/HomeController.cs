using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTask(Task task)
        {
            if (task == null)
            {
                return HttpNotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(task);
            }

            using (var context = new DataContext())
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
            return RedirectToAction("Tasks");
        }

        public ActionResult Tasks(string filter)
        {
            if (filter == null)
            {
                filter = "";
            }
            List<TaskInfo> tasksInfo = new List<TaskInfo>();
            using (var context = new DataContext())
            {
                foreach(var x in context.Tasks)
                {
                    tasksInfo.Add(
                        new TaskInfo
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Tags = context.Tags.Where(a => a.TaskId == x.Id).OrderBy(a => a.Name).Select(a => a.Name).ToList(),
                            Description = x.Description,
                            Perform = x.Perform
                        }
                        );
                }
            }

            if (filter != "")
            {
                tasksInfo = tasksInfo.Where(table => table.Name.ToUpper().Contains(filter.ToUpper().Trim())).ToList();
            }

            ViewBag.TasksInfo = tasksInfo;
            ViewBag.Filter = filter;
            ViewBag.Space = " ";
            return View();
        }

        [HttpGet]
        public ActionResult TaskPerform()
        {
            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public ActionResult TaskPerform(int? id, string perform, string filter)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new DataContext())
            {
                Task task = context.Tasks.Find(id);

                if (task == null)
                {
                    return HttpNotFound();
                }

                task.Perform = (perform != null);
                context.SaveChanges();

            }

            return RedirectToAction("Tasks", "Home", new { filter = filter });

        }

        [HttpGet]
        public ActionResult AddTag()
        {
            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public ActionResult AddTag(int? id, string name, string filter)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (var context = new DataContext())
            {
                Tag tag = new Tag();
                tag.Name = name;
                tag.TaskId = (int)id;
                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return RedirectToAction("Tasks", "Home", new { filter = filter });

        }
        [HttpGet]
        public ActionResult FormFilter()
        {
            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public ActionResult FormFilter(string filter)
        {
            if (filter == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Tasks", "Home", new { filter = filter });

        }
    }
}