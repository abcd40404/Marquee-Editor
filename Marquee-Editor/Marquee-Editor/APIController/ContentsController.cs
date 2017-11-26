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
    [Authorize]
    public class ContentsController : ApiController
    {
        private MyDB db = new MyDB();

        // GET: api/Contents
        public IQueryable<Content> GetContents()
        {
            return db.Contents;
        }

        // GET: api/Contents/5
        [ResponseType(typeof(Content))]
        public IHttpActionResult GetContent(int id)
        {
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            return Ok(content);
        }

        // PUT: api/Contents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContent(int id, Content content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != content.Id)
            {
                return BadRequest();
            }

            db.Entry(content).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContentExists(id))
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

        // POST: api/Contents
        [ResponseType(typeof(Content))]
        public IHttpActionResult PostContent(Content content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contents.Add(content);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = content.Id }, content);
        }

        // DELETE: api/Contents/5
        [ResponseType(typeof(Content))]
        public IHttpActionResult DeleteContent(int id)
        {
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            db.Contents.Remove(content);
            db.SaveChanges();

            return Ok(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContentExists(int id)
        {
            return db.Contents.Count(e => e.Id == id) > 0;
        }
    }
}