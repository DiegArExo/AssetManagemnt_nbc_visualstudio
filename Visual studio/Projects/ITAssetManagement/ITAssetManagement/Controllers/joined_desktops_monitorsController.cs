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
    public class joined_desktops_monitorsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/joined_desktops_monitors
        public IQueryable<joined_desktops_monitors> Getjoined_desktops_monitors()
        {
            return db.joined_desktops_monitors;
        }

        // GET: api/joined_desktops_monitors/5
        [ResponseType(typeof(joined_desktops_monitors))]
        public IHttpActionResult Getjoined_desktops_monitors(int id)
        {
            joined_desktops_monitors joined_desktops_monitors = db.joined_desktops_monitors.Find(id);
            if (joined_desktops_monitors == null)
            {
                return NotFound();
            }

            return Ok(joined_desktops_monitors);
        }

        // PUT: api/joined_desktops_monitors/update_monitor/5
        [ResponseType(typeof(void))]
        [Route("api/joined_desktops_monitors/update_monitor")]
        public IHttpActionResult Putjoined_desktops_monitor(int id, joined_desktops_monitors joined_desktops_monitors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            // Retrieve the existing entity from the database
            var existingJoinedDesktopsMonitors = db.joined_desktops_monitors.Find(id);
            if (existingJoinedDesktopsMonitors == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }

            // Update only the fields that need to be changed

            existingJoinedDesktopsMonitors.desktop_monitor_id = joined_desktops_monitors.desktop_monitor_id;

            // existingJoinedDesktopsMonitors.desktop_cpu_id = joined_desktops_monitors.desktop_cpu_id;
            // existingJoinedDesktopsMonitors.desktop_monitor_id = joined_desktops_monitors.desktop_monitor_id;
            // existingJoinedDesktopsMonitors.user_assigned_id = updatedJoinedDesktopsMonitors.user_assigned_id;
            // existingJoinedDesktopsMonitors.user_updated = joined_desktops_monitors.user_updated;
            // existingJoinedDesktopsMonitors.date_created = joined_desktops_monitors.date_created;
            // existingJoinedDesktopsMonitors.date_updated = joined_desktops_monitors.date_updated;

            // This code will update the date updated to the current one
            // existingJoinedDesktopsMonitors.date_updated = DateTime.UtcNow;  

            db.Entry(existingJoinedDesktopsMonitors).State = EntityState.Modified;

            //Try and Catch Error to avoid the code from crashing.
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!joined_desktops_monitorsExists(id))
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

            return Ok(joined_desktops_monitors);
        }
        // PUT: api/joined_desktops_monitors/update_monitor/5
        [ResponseType(typeof(void))]
        [Route("api/joined_desktops_monitors/update_cpu")]
        public IHttpActionResult Putjoined_desktops_cpu(int id, joined_desktops_monitors joined_desktops_monitors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Retrieve the existing entity from the database
            var existingJoinedDesktopsCPU = db.joined_desktops_monitors.Find(id);
            if (existingJoinedDesktopsCPU == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }

            // Update only the fields that need to be changed

            existingJoinedDesktopsCPU.desktop_cpu_id = joined_desktops_monitors.desktop_cpu_id;

            // existingJoinedDesktopsMonitors.desktop_cpu_id = joined_desktops_monitors.desktop_cpu_id;
            // existingJoinedDesktopsMonitors.desktop_monitor_id = joined_desktops_monitors.desktop_monitor_id;
            // existingJoinedDesktopsMonitors.user_assigned_id = updatedJoinedDesktopsMonitors.user_assigned_id;
            // existingJoinedDesktopsMonitors.user_updated = joined_desktops_monitors.user_updated;
            // existingJoinedDesktopsMonitors.date_created = joined_desktops_monitors.date_created;
            // existingJoinedDesktopsMonitors.date_updated = joined_desktops_monitors.date_updated;

            // This code will update the date updated to the current one
            // existingJoinedDesktopsMonitors.date_updated = DateTime.UtcNow;  

            db.Entry(existingJoinedDesktopsCPU).State = EntityState.Modified;

            //Try and Catch Error to avoid the code from crashing.
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!joined_desktops_monitorsExists(id))
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

            return Ok(joined_desktops_monitors);
        }


        // POST: api/joined_desktops_monitors
        [ResponseType(typeof(joined_desktops_monitors))]
        public IHttpActionResult Postjoined_desktops_monitors(joined_desktops_monitors joined_desktops_monitors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.joined_desktops_monitors.Add(joined_desktops_monitors);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = joined_desktops_monitors.id }, joined_desktops_monitors);
        }

        // DELETE: api/joined_desktops_monitors/5
        [ResponseType(typeof(joined_desktops_monitors))]
        public IHttpActionResult Deletejoined_desktops_monitors(int id)
        {
            joined_desktops_monitors joined_desktops_monitors = db.joined_desktops_monitors.Find(id);
            if (joined_desktops_monitors == null)
            {
                return NotFound();
            }

            db.joined_desktops_monitors.Remove(joined_desktops_monitors);
            db.SaveChanges();

            return Ok(joined_desktops_monitors);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool joined_desktops_monitorsExists(int id)
        {
            return db.joined_desktops_monitors.Count(e => e.id == id) > 0;
        }
    }
}