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

        //------------------ GET CPU INFORMATION WITH STATUS NAME START ------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_monitors/get_cpus")]
        public IHttpActionResult Getcpus()
        {

            // Fetch non-loanable laptops along with their status from the database
            var get_cpu = from l_cpu in db.desktop_cpus

                          join d_device_status in db.device_status
                          on l_cpu.status_id equals d_device_status.id

                          select new
                          {
                              l_cpu.id,
                              l_cpu.brand_name,
                              l_cpu.model,
                              l_cpu.cpu_serial_number,
                              l_cpu.cpu_tag_number,

                              
                              cpu_StatusName = d_device_status.name // Get the status name
                          };

            // Return the result with the status included
            return Ok(get_cpu);
        }
        //------------------ GET MONITOR INFORMATION WITH STATUS NAME END ------------------------

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

        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF CPU START)----------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/cpu/write_off_cpu/{id}")]
        public IHttpActionResult PutWriteOffCPU(int id, desktop_cpus desktop_cpus)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != desktop_cpus.id)
            {
                return BadRequest();
            }

            // Retrieve the existing entity from the database
            var existingCPU = db.desktop_cpus.Find(id);
            if (existingCPU == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            // Preserve fields that should not be changed
            desktop_cpus.model = existingCPU.model;
            desktop_cpus.brand_name = existingCPU.brand_name;
            desktop_cpus.cpu_serial_number = existingCPU.cpu_serial_number;
            desktop_cpus.cpu_tag_number = existingCPU.cpu_tag_number;
            desktop_cpus.user_created = existingCPU.user_created;
            desktop_cpus.date_created = existingCPU.date_created;

            // Always update the date_updated field
            desktop_cpus.date_updated = DateTime.Now;
            // Update device_status_id for the laptop
            desktop_cpus.status_id = 5;

            db.Entry(existingCPU).CurrentValues.SetValues(desktop_cpus);

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
                var innerException = ex.InnerException?.InnerException;
                return InternalServerError(innerException ?? ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(desktop_cpus); // Return the updated laptop data
        }
        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF CPU END)----------------------------------------------

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