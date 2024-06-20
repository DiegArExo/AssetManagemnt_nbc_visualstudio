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
    public class desktop_monitorsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/desktop_monitors
        public IQueryable<desktop_monitors> Getdesktop_monitors()
        {
            return db.desktop_monitors;
        }

        // GET: api/desktop_monitors/5
        [ResponseType(typeof(desktop_monitors))]
        public IHttpActionResult Getdesktop_monitors(int id)
        {
            desktop_monitors desktop_monitors = db.desktop_monitors.Find(id);
            if (desktop_monitors == null)
            {
                return NotFound();
            }

            return Ok(desktop_monitors);
        }
        //------------------ GET MONITOR INFORMATION WITH STATUS NAME START ------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors/get_monitors")]
        public IHttpActionResult Getmonitor()
        {
           
            // Fetch non-loanable laptops along with their status from the database
            var getmonitor = from l_monitor in db.desktop_monitors

                                                  join d_device_status in db.device_status
                                                  on l_monitor.status_id equals d_device_status.id

                                                  select new
                                                  {
                                                      l_monitor.id,
                                                      l_monitor.brand_name,
                                                      l_monitor.model,
                                                      l_monitor.monitor_serial_number,
                                                      l_monitor.monitor_tag_number,
                                                   
                                                      // Get all laptop details
                                                      monitor_StatusName = d_device_status.name // Get the status name
                                                  };

            // Return the result with the status included
            return Ok(getmonitor);
        }
        //------------------ GET MONITOR INFORMATION WITH STATUS NAME END ------------------------

        // PUT: api/desktop_monitors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdesktop_monitors(int id, desktop_monitors desktop_monitors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the existing entity from the database
            var existingDesktopsMonitors = db.desktop_monitors.Find(id);
            if (existingDesktopsMonitors == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }
            //Defining Tableas that needs to be updated 
            existingDesktopsMonitors.brand_name = desktop_monitors.brand_name;
            existingDesktopsMonitors.model = desktop_monitors.model;
            existingDesktopsMonitors.monitor_serial_number = desktop_monitors.monitor_serial_number;
            existingDesktopsMonitors.monitor_tag_number = desktop_monitors.monitor_tag_number;


            db.Entry(existingDesktopsMonitors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!desktop_monitorsExists(id))
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
            return Ok(desktop_monitors);
        }


        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF MONITOR START)----------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/monitor/write_off_monitor/{id}")]
        public IHttpActionResult PutWriteOffMonitor(int id, desktop_monitors desktop_monitors)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != desktop_monitors.id)
            {
                return BadRequest();
            }

            // Retrieve the existing entity from the database
            var existingMonitor = db.desktop_monitors.Find(id);
            if (existingMonitor == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            desktop_monitors.model = existingMonitor.model;
            desktop_monitors.brand_name = existingMonitor.brand_name;
            desktop_monitors.monitor_serial_number = existingMonitor.monitor_serial_number;
            desktop_monitors.monitor_tag_number = existingMonitor.monitor_tag_number;
            desktop_monitors.user_created = existingMonitor.user_created;
            desktop_monitors.date_created = existingMonitor.date_created;


            // Always update the date_updated field
            desktop_monitors.date_updated = DateTime.Now;
            // Update device_status_id for the laptop
            desktop_monitors.status_id = 5;

            db.Entry(existingMonitor).CurrentValues.SetValues(desktop_monitors);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!desktop_monitorsExists(id))
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

            return Ok(existingMonitor); // Return the updated laptop data
        }
        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF MONITOR END)----------------------------------------------




        // PUT: api/desktop_monitors/5
        [ResponseType(typeof(void))]
        [Route("api/desktop_monitors/write_off/update/{id}")]
        public IHttpActionResult Putdesktop_monitors_write_off(int id, desktop_monitors desktop_monitors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the existing entity from the database
            var existingDesktopsMonitors = db.desktop_monitors.Find(id);
            if (existingDesktopsMonitors == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }
            //Defining Tableas that needs to be updated 
            existingDesktopsMonitors.comments = desktop_monitors.comments;
            existingDesktopsMonitors.attachment = desktop_monitors.attachment;
 


            db.Entry(existingDesktopsMonitors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!desktop_monitorsExists(id))
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
            return Ok(desktop_monitors);
        }
        // POST: api/desktop_monitors
        [ResponseType(typeof(desktop_monitors))]
        public IHttpActionResult Postdesktop_monitors(desktop_monitors desktop_monitors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.desktop_monitors.Add(desktop_monitors);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = desktop_monitors.id }, desktop_monitors);
        }

        // DELETE: api/desktop_monitors/5
        [ResponseType(typeof(desktop_monitors))]
        public IHttpActionResult Deletedesktop_monitors(int id)
        {
            desktop_monitors desktop_monitors = db.desktop_monitors.Find(id);
            if (desktop_monitors == null)
            {
                return NotFound();
            }

            db.desktop_monitors.Remove(desktop_monitors);
            db.SaveChanges();

            return Ok(desktop_monitors);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool desktop_monitorsExists(int id)
        {
            return db.desktop_monitors.Count(e => e.id == id) > 0;
        }
    }
}