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
    public class assigned_laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/assigned_laptops
        public IQueryable<assigned_laptops> Getassigned_laptops()
        {
            return db.assigned_laptops;
        }

        // GET: api/assigned_laptops/5
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Getassigned_laptops(int id)
        {
            assigned_laptops assigned_laptops = db.assigned_laptops.Find(id);
            if (assigned_laptops == null)
            {
                return NotFound();
            }

            return Ok(assigned_laptops);
        }

        // PUT: api/assigned_laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_laptops(int id, assigned_laptops assigned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assigned_laptops.id)
            {
                return BadRequest();
            }

            db.Entry(assigned_laptops).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!assigned_laptopsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(assigned_laptops);
        }

        // POST: api/assigned_laptops
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Postassigned_laptops(assigned_laptops assigned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.assigned_laptops.Add(assigned_laptops);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assigned_laptops.id }, assigned_laptops);
        }

        // DELETE: api/assigned_laptops/5
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Deleteassigned_laptops(int id)
        {
            assigned_laptops assigned_laptops = db.assigned_laptops.Find(id);
            if (assigned_laptops == null)
            {
                return NotFound();
            }

            db.assigned_laptops.Remove(assigned_laptops);
            db.SaveChanges();

            return Ok(assigned_laptops);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool assigned_laptopsExists(int id)
        {
            return db.assigned_laptops.Count(e => e.id == id) > 0;
        }
    }
}