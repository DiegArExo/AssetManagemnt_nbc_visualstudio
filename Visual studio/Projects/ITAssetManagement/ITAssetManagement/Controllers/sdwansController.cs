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
    public class sdwansController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/sdwans
        public IQueryable<sdwan> Getsdwans()
        {
            return db.sdwans;
        }

        // GET: api/sdwans/5
        [ResponseType(typeof(sdwan))]
        public IHttpActionResult Getsdwan(int id)
        {
            sdwan sdwan = db.sdwans.Find(id);
            if (sdwan == null)
            {
                return NotFound();
            }

            return Ok(sdwan);
        }

        // PUT: api/sdwans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putsdwan(int id, sdwan sdwan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sdwan.id)
            {
                return BadRequest();
            }

            db.Entry(sdwan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sdwanExists(id))
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

        // POST: api/sdwans
        [ResponseType(typeof(sdwan))]
        public IHttpActionResult Postsdwan(sdwan sdwan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.sdwans.Add(sdwan);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sdwan.id }, sdwan);
        }

        // DELETE: api/sdwans/5
        [ResponseType(typeof(sdwan))]
        public IHttpActionResult Deletesdwan(int id)
        {
            sdwan sdwan = db.sdwans.Find(id);
            if (sdwan == null)
            {
                return NotFound();
            }

            db.sdwans.Remove(sdwan);
            db.SaveChanges();

            return Ok(sdwan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool sdwanExists(int id)
        {
            return db.sdwans.Count(e => e.id == id) > 0;
        }
    }
}