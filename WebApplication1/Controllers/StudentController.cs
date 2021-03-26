using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private static IMongoCollection<StudentModels> studentCollection;
        private static List<StudentModels> students;

        public StudentController()
        {
            Mongo mongo = new Mongo();
            studentCollection = mongo.db.GetCollection<StudentModels>("Students");
            students = studentCollection.Find(new BsonDocument()).ToList();
        }

        // GET: Student
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var student = from s in students
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                student = student.Where(item => item.FirstMidName.Contains(searchString) 
                || item.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    student = student.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    student = student.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    student = student.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    student = student.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(student.ToPagedList(pageNumber, pageSize));
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(StudentModels student)
        {
            try
            {
                studentCollection.InsertOne(student);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/:id
        public ActionResult Edit(String id)
        {
            var student = students.Find(item => item.Id.ToString().CompareTo(id) == 0);
            return View(student);
        }

        // POST: Student/Edit/:id
        [HttpPost]
        public ActionResult Edit(String id, StudentModels student)
        {
            try
            {
                student.Id = new ObjectId(id);
                var filter = Builders<StudentModels>.Filter.Eq("Id", student.Id);
                var updateDef = Builders<StudentModels>.Update.Set("LastName", student.LastName);
                updateDef = updateDef.Set("FirstMidName", student.FirstMidName);
                updateDef = updateDef.Set("EnrollmentDate", student.EnrollmentDate);
                var result = studentCollection.UpdateOne(filter, updateDef);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Details/:id
        public ActionResult Details(String id)
        {
            var student = students.Find(item => item.Id.ToString().CompareTo(id) == 0);
            return View(student);
        }

        // GET: Student/Delete/:id
        public ActionResult Delete(String id)
        {
            var student = students.Find(item => item.Id.ToString().CompareTo(id) == 0);
            return View(student);
        }

        // POST: Student/Delete/:id
        [HttpPost]
        public ActionResult Delete(String id, FormCollection collection)
        {
            var student = students.Find(item => item.Id.ToString().CompareTo(id) == 0);
            try
            {
                studentCollection.DeleteOne<StudentModels>(e => e.Id == student.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}