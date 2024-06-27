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
    public class device_statusController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/device_status
        public IQueryable<device_status> Getdevice_status(string token)
        {
            return db.device_status;
        }

        // GET: api/device_status/5
        [ResponseType(typeof(device_status))]
        public IHttpActionResult Getdevice_status(int id, string token)
        {
            device_status device_status = db.device_status.Find(id);
            if (device_status == null)
            {
                return NotFound();
            }

            return Ok(device_status);
        }

        // PUT: api/device_status/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdevice_status(int id, device_status device_status, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device_status.id)
            {
                return BadRequest();
            }

            db.Entry(device_status).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!device_statusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(device_status);
        }

        // POST: api/device_status
        [ResponseType(typeof(device_status))]
        public IHttpActionResult Postdevice_status(device_status device_status, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.device_status.Add(device_status);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = device_status.id }, device_status);
        }

        // DELETE: api/device_status/5
        [ResponseType(typeof(device_status))]
        public IHttpActionResult Deletedevice_status(int id, string token)
        {
            device_status device_status = db.device_status.Find(id);
            if (device_status == null)
            {
                return NotFound();
            }

            db.device_status.Remove(device_status);
            db.SaveChanges();

            return Ok(device_status);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool device_statusExists(int id)
        {
            return db.device_status.Count(e => e.id == id) > 0;
        }
    }
}