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
    public class sdwan_laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/sdwan_laptops
        public IQueryable<sdwan_laptops> Getsdwan_laptops()
        {
            return db.sdwan_laptops;
        }

        // GET: api/sdwan_laptops/5
        [ResponseType(typeof(sdwan_laptops))]
        public IHttpActionResult Getsdwan_laptops(int id)
        {
            sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
            if (sdwan_laptops == null)
            {
                return NotFound();
            }

            return Ok(sdwan_laptops);
        }

        // PUT: api/sdwan_laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putsdwan_laptops(int id, sdwan_laptops sdwan_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sdwan_laptops.id)
            {
                return BadRequest();
            }

            db.Entry(sdwan_laptops).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sdwan_laptopsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(sdwan_laptops);
        }

        // POST: api/sdwan_laptops
        [ResponseType(typeof(sdwan_laptops))]
        public IHttpActionResult Postsdwan_laptops(sdwan_laptops sdwan_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.sdwan_laptops.Add(sdwan_laptops);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sdwan_laptops.id }, sdwan_laptops);
        }

        // DELETE: api/sdwan_laptops/5
        [ResponseType(typeof(sdwan_laptops))]
        public IHttpActionResult Deletesdwan_laptops(int id)
        {
            sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
            if (sdwan_laptops == null)
            {
                return NotFound();
            }

            db.sdwan_laptops.Remove(sdwan_laptops);
            db.SaveChanges();

            return Ok(sdwan_laptops);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool sdwan_laptopsExists(int id)
        {
            return db.sdwan_laptops.Count(e => e.id == id) > 0;
        }
    }
}