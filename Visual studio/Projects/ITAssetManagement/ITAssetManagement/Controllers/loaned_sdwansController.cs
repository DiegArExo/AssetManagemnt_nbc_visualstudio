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
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    public class loaned_sdwansController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/loaned_sdwans
        public IQueryable<loaned_sdwans> Getloaned_sdwans(string token)
        {
            return db.loaned_sdwans;
        }

        // GET: api/loaned_sdwans/5
        [ResponseType(typeof(loaned_sdwans))]
        public IHttpActionResult Getloaned_sdwans(int id, string token)
        {
            loaned_sdwans loaned_sdwans = db.loaned_sdwans.Find(id);
            if (loaned_sdwans == null)
            {
                return NotFound();
            }

            return Ok(loaned_sdwans);
        }

        // PUT: api/loaned_sdwans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putloaned_sdwans(int id, loaned_sdwans loaned_sdwans, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != loaned_sdwans.id)
            {
                return BadRequest();
            }

            db.Entry(loaned_sdwans).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!loaned_sdwansExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // return StatusCode(HttpStatusCode.NoContent);
            return Ok(loaned_sdwans);
        }

        // POST: api/loaned_sdwans
        [ResponseType(typeof(loaned_sdwans))]
        public IHttpActionResult Postloaned_sdwans(loaned_sdwans loaned_sdwans, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.loaned_sdwans.Add(loaned_sdwans);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = loaned_sdwans.id }, loaned_sdwans);
        }

        // DELETE: api/loaned_sdwans/5
        [ResponseType(typeof(loaned_sdwans))]
        public IHttpActionResult Deleteloaned_sdwans(int id, string token)
        {
            loaned_sdwans loaned_sdwans = db.loaned_sdwans.Find(id);
            if (loaned_sdwans == null)
            {
                return NotFound();
            }

            db.loaned_sdwans.Remove(loaned_sdwans);
            db.SaveChanges();

            return Ok(loaned_sdwans);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool loaned_sdwansExists(int id)
        {
            return db.loaned_sdwans.Count(e => e.id == id) > 0;
        }
    }
}