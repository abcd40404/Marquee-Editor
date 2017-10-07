using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Marquee_Editor.Models;

namespace Marquee_Editor.APIController
{
    public class MarqueesController : ApiController
    {
        private MyDB db = new MyDB();

        // GET: api/Marquees
        public IQueryable<Marquee> GetMarquees()
        {
            return db.Marquees;
        }

        // GET: api/Marquees/5
        [ResponseType(typeof(Marquee))]
        public IHttpActionResult GetMarquee(int id)
        {
            Marquee marquee = db.Marquees.Find(id);
            if (marquee == null)
            {
                return NotFound();
            }

            return Ok(marquee);
        }

        // PUT: api/Marquees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMarquee(int id, Marquee marquee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != marquee.Id)
            {
                return BadRequest();
            }

            db.Entry(marquee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarqueeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Marquees
        [ResponseType(typeof(Marquee))]
        public IHttpActionResult PostMarquee(Marquee marquee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Marquees.Add(marquee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = marquee.Id }, marquee);
        }

        // DELETE: api/Marquees/5
        [ResponseType(typeof(Marquee))]
        public IHttpActionResult DeleteMarquee(int id)
        {
            Marquee marquee = db.Marquees.Find(id);
            if (marquee == null)
            {
                return NotFound();
            }

            db.Marquees.Remove(marquee);
            db.SaveChanges();

            return Ok(marquee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MarqueeExists(int id)
        {
            return db.Marquees.Count(e => e.Id == id) > 0;
        }
    }
}