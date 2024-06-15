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
    public class assigned_sdwansController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/assigned_sdwans
        public IQueryable<assigned_sdwans> Getassigned_sdwans()
        {
            return db.assigned_sdwans;
        }

        // GET: api/assigned_sdwans/5
        [ResponseType(typeof(assigned_sdwans))]
        public IHttpActionResult Getassigned_sdwans(int id)
        {
            assigned_sdwans assigned_sdwans = db.assigned_sdwans.Find(id);
            if (assigned_sdwans == null)
            {
                return NotFound();
            }

            return Ok(assigned_sdwans);
        }

        // PUT: api/assigned_sdwans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_sdwans(int id, assigned_sdwans assigned_sdwans)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assigned_sdwans.id)
            {
                return BadRequest();
            }

            db.Entry(assigned_sdwans).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!assigned_sdwansExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(assigned_sdwans);
        }

        // POST: api/assigned_sdwans
        [ResponseType(typeof(assigned_sdwans))]
        public IHttpActionResult Postassigned_sdwans(assigned_sdwans assigned_sdwans)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.assigned_sdwans.Add(assigned_sdwans);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assigned_sdwans.id }, assigned_sdwans);
        }

        // DELETE: api/assigned_sdwans/5
        [ResponseType(typeof(assigned_sdwans))]
        public IHttpActionResult Deleteassigned_sdwans(int id)
        {
            assigned_sdwans assigned_sdwans = db.assigned_sdwans.Find(id);
            if (assigned_sdwans == null)
            {
                return NotFound();
            }

            db.assigned_sdwans.Remove(assigned_sdwans);
            db.SaveChanges();

            return Ok(assigned_sdwans);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool assigned_sdwansExists(int id)
        {
            return db.assigned_sdwans.Count(e => e.id == id) > 0;
        }
    }
}