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
    public class firewallsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/firewalls
        public IQueryable<firewall> Getfirewalls()
        {
            return db.firewalls;
        }

        // GET: api/firewalls/5
        [ResponseType(typeof(firewall))]
        public IHttpActionResult Getfirewall(int id)
        {
            firewall firewall = db.firewalls.Find(id);
            if (firewall == null)
            {
                return NotFound();
            }

            return Ok(firewall);
        }

        // PUT: api/firewalls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putfirewall(int id, firewall firewall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != firewall.id)
            {
                return BadRequest();
            }

            db.Entry(firewall).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!firewallExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // return StatusCode(HttpStatusCode.NoContent);
            return Ok(firewall);
        }


        // POST: api/firewalls
        [ResponseType(typeof(firewall))]
        public IHttpActionResult Postfirewall(firewall firewall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.firewalls.Add(firewall);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = firewall.id }, firewall);
        }

        // DELETE: api/firewalls/5
        [ResponseType(typeof(firewall))]
        public IHttpActionResult Deletefirewall(int id)
        {
            firewall firewall = db.firewalls.Find(id);
            if (firewall == null)
            {
                return NotFound();
            }

            db.firewalls.Remove(firewall);
            db.SaveChanges();

            return Ok(firewall);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool firewallExists(int id)
        {
            return db.firewalls.Count(e => e.id == id) > 0;
        }
    }
}