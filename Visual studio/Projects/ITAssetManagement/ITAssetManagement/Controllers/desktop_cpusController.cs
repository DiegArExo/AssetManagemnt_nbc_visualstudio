﻿using System;
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
    public class desktop_cpusController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/desktop_cpus
        public IQueryable<desktop_cpus> Getdesktop_cpus()
        {
            return db.desktop_cpus;
        }

        // GET: api/desktop_cpus/5
        [ResponseType(typeof(desktop_cpus))]
        public IHttpActionResult Getdesktop_cpus(int id)
        {
            desktop_cpus desktop_cpus = db.desktop_cpus.Find(id);
            if (desktop_cpus == null)
            {
                return NotFound();
            }

            return Ok(desktop_cpus);
        }

        // PUT: api/desktop_cpus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdesktop_cpus(int id, desktop_cpus desktop_cpus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the existing entity from the database
            var existingDesktopsCPU = db.desktop_cpus.Find(id);
            if (existingDesktopsCPU == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }
            //Defining Tableas that needs to be updated 
            existingDesktopsCPU.brand_name = desktop_cpus.brand_name;
            existingDesktopsCPU.model = desktop_cpus.model;
            existingDesktopsCPU.cpu_serial_number = desktop_cpus.cpu_serial_number;
            existingDesktopsCPU.cpu_tag_number = desktop_cpus.cpu_tag_number;

            db.Entry(existingDesktopsCPU).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!desktop_cpusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception
                var innerException = ex.InnerException?.InnerException;
                return InternalServerError(innerException ?? ex);
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(desktop_cpus);
        }

        // POST: api/desktop_cpus
        [ResponseType(typeof(desktop_cpus))]
        public IHttpActionResult Postdesktop_cpus(desktop_cpus desktop_cpus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.desktop_cpus.Add(desktop_cpus);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = desktop_cpus.id }, desktop_cpus);
        }

        // DELETE: api/desktop_cpus/5
        [ResponseType(typeof(desktop_cpus))]
        public IHttpActionResult Deletedesktop_cpus(int id)
        {
            desktop_cpus desktop_cpus = db.desktop_cpus.Find(id);
            if (desktop_cpus == null)
            {
                return NotFound();
            }

            db.desktop_cpus.Remove(desktop_cpus);
            db.SaveChanges();

            return Ok(desktop_cpus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool desktop_cpusExists(int id)
        {
            return db.desktop_cpus.Count(e => e.id == id) > 0;
        }
    }
}