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
    public class desktop_monitorsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //------------------------------------------------GET: api/desktop_monitors(GET ALL)---------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors")]
        public IHttpActionResult Getdesktop_monitors(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

        
                return Ok(db.desktop_monitors);
            }
            catch (Exception ex)
            {
       
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching desktop monitors.", Details = ex.Message });
            }
        }
        //------------------------------------------------GET: api/desktop_monitors(GET ALL AVAILABLE) START---------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors_available")]
        public IHttpActionResult Getdesktop_monitors_available(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }


                return Ok(db.desktop_monitors.Where(r=>r.status_id==1));
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching desktop monitors.", Details = ex.Message });
            }
        }
        //------------------------------------------------GET: api/desktop_monitors(GET ALL AVAILABLE) END---------------------------------------------


        //-------------------------------------- GET: api/desktop_monitors/5(GET A SPECIFIC)---------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors/{id}")]
        public IHttpActionResult Getdesktop_monitors(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                desktop_monitors desktop_monitor = db.desktop_monitors.Find(id);

                if (desktop_monitor == null)
                {
                    return NotFound();
                }

                return Ok(desktop_monitor);
            }
            catch (Exception ex)
            {
        
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //-------------------------------------- GET: api/desktop_monitors/5(GET A SPECIFIC) END---------------------------------------------

        //----PUT: api/desktop_monitors/5(UPDATE)---------------------------------------------
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/desktop_monitors/{id}")]
        public IHttpActionResult Putdesktop_monitors(int id, desktop_monitors desktop_monitors, string token)
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
          
                existingDesktopsMonitors.Year = desktop_monitors.Year;
                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingDesktopsMonitors.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }


                db.Entry(existingDesktopsMonitors).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Content(HttpStatusCode.Created, new { message = "Monitor information successfully Updated.", desktop_monitors = desktop_monitors });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!desktop_monitorsExists(id))
                {
                   // return NotFound();
                    return Content(HttpStatusCode.NotFound, new { message = "Monitor with the provided ID was not found." });
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
               // return InternalServerError(innerException ?? ex);
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
            }

        }
            catch (Exception ex)
            {
        
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message});
            }
 
        }




        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF MONITOR START)----------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/monitor/write_off_monitor/{id}")]
        public IHttpActionResult PutWriteOffMonitor(int id, desktop_monitors desktop_monitors, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

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
            //Get authernticated user id and save it
            int? authenticatedUserId = GetUserIdFromToken(token);
            if (authenticatedUserId.HasValue)
            {
                desktop_monitors.user_updated = authenticatedUserId.Value; // Set to authenticated user
            }

            db.Entry(existingMonitor).CurrentValues.SetValues(desktop_monitors);

            try
            {
                db.SaveChanges();
                return Content(HttpStatusCode.Created, new { message = "Monitor successfully written-off.", desktop_monitors = desktop_monitors });
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
               // return InternalServerError(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
            }
           // return Ok(existingMonitor); // Return the updated laptop data
        }
        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF MONITOR END)----------------------------------------------


        //------------------------------------------------------ GET MONITOR INFORMATION WITH STATUS NAME START ------------------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors/get_monitors")]
        public IHttpActionResult Getmonitor(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Fetch non-loanable laptops along with their status from the database
                var getmonitor = from l_monitor in db.desktop_monitors

                                 join d_device_status in db.device_status
                                 on l_monitor.status_id equals d_device_status.id
                                 orderby l_monitor.date_created descending 

                                 select new
                                 {
                                     l_monitor.id,
                                     l_monitor.brand_name,
                                     l_monitor.model,
                                     l_monitor.monitor_serial_number,
                                     l_monitor.monitor_tag_number,
                                    
                                     l_monitor.Year,
                                     




                                     monitor_StatusName = d_device_status.name  
                                 };
                return Ok(getmonitor);
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the monitors.", Details = ex.Message });
            }

            // Return the result with the status included

        }
        //------------------------------------------------------ GET MONITOR INFORMATION WITH STATUS NAME END ---------------------------------------------------------------------

        //---------------------------------------------------------- PUT: api/desktop_monitors/5(WRITTING OFF A MONITOR)-----------------------------
        [ResponseType(typeof(void))]
        [Route("api/desktop_monitor/write_off/update/{id}")]
        [HttpPut]
        public IHttpActionResult Putdesktop_monitors_write_off(int id, desktop_monitors desktop_monitors, string token)
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
            var existingDesktopsMonitors = db.desktop_monitors.Find(id);
            if (existingDesktopsMonitors == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }
            //Defining Tableas that needs to be updated 
            existingDesktopsMonitors.comments = desktop_monitors.comments;
            existingDesktopsMonitors.attachment = desktop_monitors.attachment;
                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingDesktopsMonitors.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }



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
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the monitors.", Details = ex.Message});
            }
        }


        //------------------------------------------------------------------------------------------------POST: api/desktop_monitors start-------------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpPost]
        [Route("api/desktop_monitors")]
        public IHttpActionResult Postdesktop_monitors(MonitoInvoiceModel model, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    // Validate the token
                    if (validate_token(token))
                    {
                        return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                    }

                    var desktop_monitors = model.Desktop_monitors;

                    desktop_monitors.date_created = DateTime.Now;
                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        desktop_monitors.user_created = authenticatedUserId.Value; // Set to authenticated user
                        desktop_monitors.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }
                    db.desktop_monitors.Add(desktop_monitors);
                    db.SaveChanges();

                    // Created instance to insert into the invoice table
                    var MonitorInvoice = new monitor_invoice
                    {
                        monitor_id = desktop_monitors.id,
                        invoice_document = model.Invoice_monitor,
                        user_created = desktop_monitors.user_created,
                        date_created = DateTime.Now
                    };
                    // Save data in the invoice table to store the invoice documents
                    db.monitor_invoice.Add(MonitorInvoice);
                    db.SaveChanges();


                    transaction.Commit();

                    // return CreatedAtRoute("DefaultApi", new { id = desktop_monitors.id }, desktop_monitors);
                    return Content(HttpStatusCode.Created, new { message = "Monitor created successfully.", assigned_desktops = desktop_monitors });
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(new Exception("An error occurred while saving desktop and monitor information.", innerException ?? ex));
                }
                catch (Exception ex)
                {
                    // Rollback incase of any issues in the transaction
                    transaction.Rollback();
                    return InternalServerError(new Exception("An unexpected error occurred.", ex));
                }
            }
        }

        //------------------------------------------------------------------------------------------------POST: api/desktop_monitors End-------------------------------------------------------

        //------------------------------------------------------ UPDATE MONITOR TO BE AVAILABLE START -----------------------------------------------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/desktop_monitors/update_monitor_status_available/{id}")]
        public IHttpActionResult PutDesktopMonitorStatus(int id, string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var existingDesktopMonitor = db.desktop_monitors.Find(id);
                if (existingDesktopMonitor == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Monitor with ID {id} not found." });
                }

                existingDesktopMonitor.status_id = 1;
                // Get authenticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingDesktopMonitor.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                db.Entry(existingDesktopMonitor).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Content(HttpStatusCode.OK, new { Message = "Monitor status successfully updated to 1.", DesktopMonitor = existingDesktopMonitor });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (existingDesktopMonitor == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = "Monitor with the provided ID was not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.InnerException;
                    return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating the monitor status.", Error = innerException?.Message ?? ex.Message });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //------------------------------------------------------ UPDATE MONITOR TO BE AVAILABLE END -------------------------------------------------------------------------------------------
        //-------------------------------------------------------GET ALL MONITORS (TOTAL) END---------------------------------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors/Get_all_monitor")]
        public IHttpActionResult CountDesktopMonitors(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count all records in the desktop_monitors table
                var desktopMonitorsCount = db.desktop_monitors.Count();

                return Ok(new { total_monitors = desktopMonitorsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //-------------------------------------------------------GET ALL MONITORS END---------------------------------------------------------------------------

        //-------------------------------------------------------GET ALL AVAILABLE MONITORS START---------------------------------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors/not_in_joined_desktops_by_monitor_id")]
        public IHttpActionResult CountMonitorsNotInJoinedDesktopsByMonitorId(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count monitors that are not in joined_desktops_monitors by Monitor_id
                var monitorsNotInJoinedDesktopsByMonitorIdCount = db.desktop_monitors
                    .Count(dm => !db.joined_desktops_monitors.Any(jdm => jdm.desktop_monitor_id == dm.id));

                return Ok(new { total_available_monitor = monitorsNotInJoinedDesktopsByMonitorIdCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //-------------------------------------------------------GET ALL AVAILABLE MONITORS END----------------------------------------------------------------------------

        //-------------------------------------------------------GET ALL ASSIGNED MONITORS START---------------------------------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/desktop_monitors/in_joined_desktops_by_monitor_id")]
        public IHttpActionResult CountMonitorsInJoinedDesktopsByMonitorId(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count monitors that are in joined_desktops_monitors by Monitor_id
                var monitorsInJoinedDesktopsByMonitorIdCount = db.desktop_monitors
                    .Count(dm => db.joined_desktops_monitors.Any(jdm => jdm.desktop_monitor_id == dm.id));

                return Ok(new { Total_assigned_monitors = monitorsInJoinedDesktopsByMonitorIdCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //-------------------------------------------------------GET ALL ASSIGNED MONITORS START---------------------------------------------------------------------------


        //-------------------------------------------------GET MONITOR INVOICE START----------------------------------------------
       
        [Route("api/desktop_monitors/get_incoice")]
        [HttpGet]
        public IHttpActionResult GetMonitorInvoiceAttachment(int monitorId, string token)
        {

            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var monitorInoive = db.monitor_invoice
                                   .Where(invoidId => invoidId.monitor_id == monitorId)
                                   .FirstOrDefault();

                if (monitorInoive == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "Invoice not found." });
                }

                return Ok(monitorInoive);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the attachment.", Details = ex.Message });
            }
          
        }


       

        //-------------------------------------------------GET MONITOR INVOICE END----------------------------------------------

        //---------------------------------------------------DOWNLOAND THE ATTACHMENT(MONITOR WRITTEN OFF DOCUMENT) END-----------------------------
        // DELETE: api/desktop_monitors/5
        [ResponseType(typeof(desktop_monitors))]
        public IHttpActionResult Deletedesktop_monitors(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                desktop_monitors desktop_monitors = db.desktop_monitors.Find(id);
                if (desktop_monitors == null)
                {
                    return NotFound();
                }

                db.desktop_monitors.Remove(desktop_monitors);
                db.SaveChanges();

                return Ok(desktop_monitors);

            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while Deleting the monitors.", Details = ex.Message });
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

        private bool desktop_monitorsExists(int id)
        {
            return db.desktop_monitors.Count(e => e.id == id) > 0;
        }
    }
}