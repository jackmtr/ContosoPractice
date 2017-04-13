using ContosoPractice.DAL;
using ContosoPractice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoPractice.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext _db = new SchoolContext();

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create() {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student, HttpPostedFileBase upload) {

            try {
                if (ModelState.IsValid) {
                    if (upload != null && upload.ContentLength > 0) {

                        var avatar = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };

                        using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }

                        student.Files = new List<File> { avatar };
                    }

                    _db.Students.Add(student);
                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException) {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }
    }
}