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
    public class joined_desktops_monitorsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // ----------------------------------GET: api/joined_desktops_monitors(GET ALL)----------------------------------
        [ResponseType(typeof(joined_desktops_monitors))]
        [HttpGet]
        [Route("api/joined_desktops_monitors")]
        public IHttpActionResult Getjoined_desktops_monitors(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }
                var joinedDesktopsMonitors = db.joined_desktops_monitors.ToList();  

                return Ok(new { Message = "Successfully fetched joined desktops and monitors.", joinedDesktopsMonitors });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching joined desktops and monitors.", Details = ex.Message });
            }
        }

        //---------------------------------- GET: api/joined_desktops_monitors/5(GET A SPECIFIC)----------------------------------
        [ResponseType(typeof(joined_desktops_monitors))]
        public IHttpActionResult Getjoined_desktops_monitors(int id, string token)
        {
            try { 
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            joined_desktops_monitors joined_desktops_monitors = db.joined_desktops_monitors.Find(id);
            if (joined_desktops_monitors == null)
            {
                return NotFound();
            }

            return Ok(joined_desktops_monitors);

            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate error response
                return InternalServerError(ex);
            }
        }

        //------------------------------------ PUT UNASSIGNING A MONITOR FROM THE JOINED TABLE: WE UPDATE THE MONITOR ID TO ZERO(0)--------------------------------------------------
        [ResponseType(typeof(void))]
        [Route("api/joined_desktops_monitors/unassign_monitor_from_user")]
        public IHttpActionResult Putjoined_desktops_monitor(int id, joined_desktops_monitors joined_desktops_monitors, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingJoinedDesktopsMonitors = db.joined_desktops_monitors.Find(id);
            if (existingJoinedDesktopsMonitors == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            // Get the desktop_cpu_id and save it
            int? desktopMonitorId = existingJoinedDesktopsMonitors.desktop_monitor_id;
            var desktopMonitor = db.desktop_monitors.Find(desktopMonitorId);
            if (desktopMonitor == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Monitor with ID {desktopMonitor} not found." });
            }
            desktopMonitor.status_id = 1;
            db.Entry(desktopMonitor).State = EntityState.Modified;
            db.SaveChanges();

            // Check if desktop_monitor_id already exists
            var monitorExists = db.joined_desktops_monitors
                .Where(jdm => jdm.desktop_monitor_id != 0) 
                .Any(jdm => jdm.desktop_monitor_id == joined_desktops_monitors.desktop_monitor_id && jdm.id != id);
            if (monitorExists)
            {
                return Content(HttpStatusCode.Conflict, new { Message = "A record with the same desktop_monitor_id already exists." });
            }

            //Get authernticated user id and save it
            int? authenticatedUserId = GetUserIdFromToken(token);
            if (authenticatedUserId.HasValue)
            {
                existingJoinedDesktopsMonitors.user_updated = authenticatedUserId.Value; // Set to authenticated user
            }

            existingJoinedDesktopsMonitors.desktop_monitor_id = joined_desktops_monitors.desktop_monitor_id;

            db.Entry(existingJoinedDesktopsMonitors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok(new { Message = "Record updated successfully.", joined_desktops_monitors });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!joined_desktops_monitorsExists(id))
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "Concurrency error: Record does not exist." });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new { Message = "Concurrency error occurred." });
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception
                var innerException = ex.InnerException?.InnerException;

                return InternalServerError(new Exception("An error occurred while saving desktop CPUs information.", innerException ?? ex));
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An unexpected error occurred.", ex));
            }
        }
        //------------------------------------ PUT UNASSIGNING A MONITOR FROM THE JOINED TABLE: WE UPDATE THE MONITOR ID TO ZERO(0)--------------------------------------------------

        //------------------------------------ PUT ASSIGNING A MONITOR FROM THE JOINED TABLE: WE UPDATE THE MONITOR ID TO THE GIVEN MONITOR ID--------------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/joined_desktops_monitors/assign_monitor_to_user")]
        public IHttpActionResult Putjoined_desktops_monitor_assign(int id, SignOut_DesktopMonitor model, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var joined_desktops_monitors = model.Sign_out_Monitor;
                    // Retrieve the existing entity from the database
                    var existingJoinedDesktopsMonitors = db.joined_desktops_monitors.Find(id);
                    if (existingJoinedDesktopsMonitors == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
                    }
                    // Check if the desktop_monitor_id is already assigned
                    var monitorExists = db.joined_desktops_monitors
                        .Any(jdm => jdm.desktop_monitor_id == joined_desktops_monitors.desktop_monitor_id);// && jdm.id != id);
                    if (monitorExists)
                    {
                        return Content(HttpStatusCode.Conflict, new { Message = "This monitor is already assigned to another user." });
                    }

                   


                    // Update the existing entity
                    existingJoinedDesktopsMonitors.desktop_monitor_id = joined_desktops_monitors.desktop_monitor_id;
                   
                    // existingJoinedDesktopsMonitors.user_updated = joined_desktops_monitors.user_updated;

                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        existingJoinedDesktopsMonitors.user_updated = authenticatedUserId.Value;  
                    }
                    db.Entry(existingJoinedDesktopsMonitors).State = EntityState.Modified;
                    db.SaveChanges();

                    // change the statuis to 2 because it was assigned
                    var desktopMonitor = db.desktop_monitors.Find(joined_desktops_monitors.desktop_monitor_id);
                    if (desktopMonitor == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = $"Monitor with ID {joined_desktops_monitors.desktop_monitor_id} not found." });
                    }
                    desktopMonitor.status_id = 2;
                    db.Entry(desktopMonitor).State = EntityState.Modified;
                    db.SaveChanges();

                    // Create an instance for sign_out_desktop_monitor
                    var MonitorSigout = new sign_out_desktop_monitor
                    {
                        desktop_monitor_id = model.desktop_monitor_id,
                        signout_document = model.signout_document,
                        user_id = model.user_id,
                        user_created = existingJoinedDesktopsMonitors.user_created,
                        date_created = DateTime.Now
                    };

                    // Save data in the sign_out_desktop_monitor table to store the sign-out documents
                    db.sign_out_desktop_monitor.Add(MonitorSigout);
                    db.SaveChanges();

                    // Commit the transaction
                    transaction.Commit();

                    //return Ok(new { message = "Record updated and monitor signed out successfully." });
                    return Content(HttpStatusCode.Created, new { message = "Record updated and monitor signed out successfully.", joined_desktops_monitors = joined_desktops_monitors });
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
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
                    transaction.Rollback();
                    // Log the inner exception
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(innerException ?? ex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while updating the record.", error = ex.Message });
                }
            }
        }

        //------------------------------------ PUT ASSIGNING A MONITOR FROM THE JOINED TABLE: WE UPDATE THE MONITOR ID TO THE GIVEN MONITOR ID--------------------------------------------------


        //--------------------------------------------------------  PUT: ASSIGNING A CPU FROM THE JOINED TABLE: WE UPDATE THE CPU ID TO GIVEN CPU ID ----------------------------------
        [ResponseType(typeof(void))]
        [Route("api/joined_desktops_monitors/assign_update_cpu")]
        public IHttpActionResult Putjoined_desktops_cpu(int id, SignOut_Desktop_CpuModel model, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var joined_desktops_monitors = model.Signout_Cpu;
                    // Retrieve the existing entity from the database
                    var existingJoinedDesktopsCPU = db.joined_desktops_monitors.Find(id);
                    if (existingJoinedDesktopsCPU == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });  
                    }

                    // Check if the desktop_monitor_id is already assigned
                    var CpuExists = db.joined_desktops_monitors
                        .Any(jdm => jdm.desktop_cpu_id == joined_desktops_monitors.desktop_cpu_id);// && jdm.id != id);
                    if (CpuExists)
                    {
                        return Content(HttpStatusCode.Conflict, new { Message = "This monitor is already assigned to another user." });
                    }

                    // Update the existing entity
                    existingJoinedDesktopsCPU.desktop_cpu_id = joined_desktops_monitors.desktop_cpu_id;


                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        existingJoinedDesktopsCPU.user_updated = authenticatedUserId.Value;  
                    }
                    db.Entry(existingJoinedDesktopsCPU).State = EntityState.Modified;
                    db.SaveChanges();

                    // change the statuis to 2 because it was assigned
                    var desktopCpu = db.desktop_cpus.Find(joined_desktops_monitors.desktop_cpu_id);
                    if (desktopCpu == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = $"Monitor with ID {joined_desktops_monitors.desktop_cpu_id} not found." });
                    }
                    desktopCpu.status_id = 2;
                    db.Entry(desktopCpu).State = EntityState.Modified;
                    db.SaveChanges();

                    // Create an instance for sign_out_desktop_cpu
                    var CPUSignOut = new sign_out_desktop_cpu
                    {
                        desktop_cpu_id = model.desktop_cpu_id,
                        signout_document = model.signout_document,
                        user_id = model.user_id,
                        user_created = existingJoinedDesktopsCPU.user_created,
                        date_created = DateTime.Now
                    };

                    // Save data in the sign_out_desktop_cpu table to store the sign-out documents
                    db.sign_out_desktop_cpu.Add(CPUSignOut);
                    db.SaveChanges();

                    // Commit the transaction
                    transaction.Commit();

                    return Ok(new { message = "Record updated and CPU signed out successfully." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
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
                    transaction.Rollback();
                    // Log the inner exception
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(innerException ?? ex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while updating the record.", error = ex.Message });
                }
            }
        }




        // ------------------------------ PUT: api/joined_desktops_monitors/update_monitor/5 (UNASSING CPU FROM USER)
        [ResponseType(typeof(void))]
        [Route("api/joined_desktops_monitors/unassign_cpu_from_user")]
        [HttpPut]
        public IHttpActionResult Putjoined_desktops_cpu(int id, joined_desktops_monitors joined_desktops_monitors, string token)
        {
            try
            {


                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

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
                // Check if desktop_monitor_id already exists
                var cpuExists = db.joined_desktops_monitors
                    .Where(jdm => jdm.desktop_cpu_id != 0)
                    .Any(jdm => jdm.desktop_cpu_id == joined_desktops_monitors.desktop_cpu_id && jdm.id != id);
                if (cpuExists)
                {
                    return Content(HttpStatusCode.Conflict, new { Message = "A record with the same desktop cpu id already exists." });
                }

                // Get the desktop_cpu_id and save it
                int? desktopCpuId = existingJoinedDesktopsCPU.desktop_cpu_id;
                var desktopCpu = db.desktop_cpus.Find(desktopCpuId);
                if (desktopCpu == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"CPU with ID {desktopCpuId} not found." });
                }
                desktopCpu.status_id = 1;
                db.Entry(desktopCpu).State = EntityState.Modified;
                db.SaveChanges();

                // Update only the fields that need to be changed
                existingJoinedDesktopsCPU.desktop_cpu_id = joined_desktops_monitors.desktop_cpu_id; //Update it to the value coming in which is zero

                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingJoinedDesktopsCPU.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }
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
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An unexpected error occurred.", ex));
            }

        }

        //---------------------------------------------------------------POST: api/joined_cpu_monitors start ------------------------------------------------
        [ResponseType(typeof(joined_desktops_monitors))]
        [HttpPost]
        [Route("api/joined_desktops_monitors")]
        public IHttpActionResult Postjoined_desktops_monitors(joined_desktops_monitors joined_desktops_monitors, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                

                // Check if the CPU or Monitor is already assigned if they are not null
                if (joined_desktops_monitors.desktop_cpu_id.HasValue)
                {
                    bool cpuAssigned = db.joined_desktops_monitors.Any(j => j.desktop_cpu_id == joined_desktops_monitors.desktop_cpu_id);
                    if (cpuAssigned)
                    {
                        return Content(HttpStatusCode.Conflict, new { message = "CPU was already assigned to a station." });
                    }
                }

                if (joined_desktops_monitors.desktop_monitor_id.HasValue)
                {
                    bool monitorAssigned = db.joined_desktops_monitors.Any(j => j.desktop_monitor_id == joined_desktops_monitors.desktop_monitor_id);
                    if (monitorAssigned)
                    {
                        return Content(HttpStatusCode.Conflict, new { message = "Monitor was already assigned to a station." });
                    }
                }
                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    joined_desktops_monitors.user_created = authenticatedUserId.Value; // Set to authenticated user
                    joined_desktops_monitors.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                joined_desktops_monitors.date_created = DateTime.Now;
                db.joined_desktops_monitors.Add(joined_desktops_monitors);
                db.SaveChanges();

                // return CreatedAtRoute("DefaultApi", new { id = joined_desktops_monitors.id }, joined_desktops_monitors);
                return Content(HttpStatusCode.Created, new { message = "Desktop Station created successfully.", assigned_desktops = joined_desktops_monitors });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                var innerException = ex.InnerException?.InnerException;
                return InternalServerError(new Exception("An error occurred while creating the assigned desktop.", innerException ?? ex));
            }
            catch (Exception ex)
            {
                // Log the exception
                return InternalServerError(new Exception("An unexpected error occurred.", ex));
            }
        }


        //---------------------------------------------------------------POST: api/joined_cpu_monitors start ------------------------------------------------

        // DELETE: api/joined_desktops_monitors/5
        [ResponseType(typeof(joined_desktops_monitors))]
        public IHttpActionResult Deletejoined_desktops_monitors(int id, string token)
        {
            try
            {

                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                joined_desktops_monitors joined_desktops_monitors = db.joined_desktops_monitors.Find(id);
                if (joined_desktops_monitors == null)
                {
                    return NotFound();
                }

                db.joined_desktops_monitors.Remove(joined_desktops_monitors);
                db.SaveChanges();

                return Ok(joined_desktops_monitors);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An unexpected error occurred.", ex));
            }
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