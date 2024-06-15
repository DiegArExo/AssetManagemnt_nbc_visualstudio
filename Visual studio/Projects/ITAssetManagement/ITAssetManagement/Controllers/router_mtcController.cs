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
    public class router_mtcController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/router_mtc
        public IQueryable<router_mtc> Getrouter_mtc()
        {
            return db.router_mtc;
        }

        // GET: api/router_mtc/5
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Getrouter_mtc(int id)
        {
            router_mtc router_mtc = db.router_mtc.Find(id);
            if (router_mtc == null)
            {
                return NotFound();
            }

            return Ok(router_mtc);
        }

        // PUT: api/router_mtc/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putrouter_mtc(int id, router_mtc router_mtc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != router_mtc.id)
            {
                return BadRequest();
            }

            db.Entry(router_mtc).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!router_mtcExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(router_mtc);
        }

        // POST: api/router_mtc
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Postrouter_mtc(router_mtc router_mtc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.router_mtc.Add(router_mtc);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = router_mtc.id }, router_mtc);
        }

        // DELETE: api/router_mtc/5
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Deleterouter_mtc(int id)
        {
            router_mtc router_mtc = db.router_mtc.Find(id);
            if (router_mtc == null)
            {
                return NotFound();
            }

            db.router_mtc.Remove(router_mtc);
            db.SaveChanges();

            return Ok(router_mtc);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool router_mtcExists(int id)
        {
            return db.router_mtc.Count(e => e.id == id) > 0;
        }
    }
}