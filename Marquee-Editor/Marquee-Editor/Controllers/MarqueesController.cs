using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Marquee_Editor.Models;

namespace Marquee_Editor.Controllers
{
    public class MarqueesController : Controller
    {
        private MyDB db = new MyDB();

        // GET: Marquees
        public ActionResult Index()
        {
            return View(db.Marquees.ToList());
        }

        // GET: Marquees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marquee marquee = db.Marquees.Find(id);
            if (marquee == null)
            {
                return HttpNotFound();
            }
            return View(marquee);
        }

        // GET: Marquees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marquees/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Location,Station")] Marquee marquee)
        {
            if (ModelState.IsValid)
            {
                db.Marquees.Add(marquee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marquee);
        }

        // GET: Marquees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marquee marquee = db.Marquees.Find(id);
            if (marquee == null)
            {
                return HttpNotFound();
            }
            return View(marquee);
        }

        // POST: Marquees/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Location,Station")] Marquee marquee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marquee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marquee);
        }

        // GET: Marquees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marquee marquee = db.Marquees.Find(id);
            if (marquee == null)
            {
                return HttpNotFound();
            }
            return View(marquee);
        }

        // POST: Marquees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marquee marquee = db.Marquees.Find(id);
            db.Marquees.Remove(marquee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
