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
    public class loaned_laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/loaned_laptops
        public IQueryable<loaned_laptops> Getloaned_laptops()
        {
            return db.loaned_laptops;
        }

        // GET: api/loaned_laptops/5
        [ResponseType(typeof(loaned_laptops))]
        public IHttpActionResult Getloaned_laptops(int id)
        {
            loaned_laptops loaned_laptops = db.loaned_laptops.Find(id);
            if (loaned_laptops == null)
            {
                return NotFound();
            }

            return Ok(loaned_laptops);
        }

        // PUT: api/loaned_laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putloaned_laptops(int id, loaned_laptops loaned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != loaned_laptops.id)
            {
                return BadRequest();
            }

            db.Entry(loaned_laptops).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!loaned_laptopsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(loaned_laptops);
        }

        // POST: api/loaned_laptops
        [ResponseType(typeof(loaned_laptops))]
        public IHttpActionResult Postloaned_laptops(loaned_laptops loaned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.loaned_laptops.Add(loaned_laptops);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = loaned_laptops.id }, loaned_laptops);
        }

        // DELETE: api/loaned_laptops/5
        [ResponseType(typeof(loaned_laptops))]
        public IHttpActionResult Deleteloaned_laptops(int id)
        {
            loaned_laptops loaned_laptops = db.loaned_laptops.Find(id);
            if (loaned_laptops == null)
            {
                return NotFound();
            }

            db.loaned_laptops.Remove(loaned_laptops);
            db.SaveChanges();

            return Ok(loaned_laptops);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool loaned_laptopsExists(int id)
        {
            return db.loaned_laptops.Count(e => e.id == id) > 0;
        }
    }
}