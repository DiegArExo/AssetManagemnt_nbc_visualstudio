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
    public class desktop_cpusController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // -------------------------------------GET: api/desktop_cpus-------------------------------------------------------------------
        // GET: api/desktop_cpus
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_cpus")]
        public IHttpActionResult Getdesktop_cpus(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }


                var desktop_cpus = db.desktop_cpus;
                return Ok(desktop_cpus);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving desktop_cpus.", Details = ex.Message });
            }
        }


        // ---------------------------------------------------------------GET: api/desktop_cpus/5 (GET A SPECIFIC)---------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        public IHttpActionResult Getdesktop_cpus(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

            
                desktop_cpus desktop_cpus = db.desktop_cpus.Find(id);
                if (desktop_cpus == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Desktop CPU not found." });
                }

            
                return Ok(desktop_cpus);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving desktop_cpus.", Details = ex.Message });
            }
        }


        //--------------------------------------------------------------- PUT: api/desktop_cpus/5---------------------------------------------------------------
        
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult Putdesktop_cpus(int id, desktop_cpus desktop_cpus, string token)
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
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving desktop_cpus.", Details = ex.Message });
            }
        }


        //--------------------------------------------- api/laptops/write_off_laptops/{id} (WRITEOFF CPU START)----------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/cpu/write_off_cpu/{id}")]
        public IHttpActionResult PutWriteOffCPU(int id, desktop_cpus desktop_cpus, string token)
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
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating desktop_cpus.", Details = ex.Message });
            }
        }
     



        //------------------------------------------------ GET CPU INFORMATION WITH STATUS NAME START ----------------------------------------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_cpus/get_cpus")]
        public IHttpActionResult Getcpus(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
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
            catch(DbUpdateException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
            }
        }
        //------------------------------------------------ GET MONITOR INFORMATION WITH STATUS NAME END ------------------------------------------------------

        //--------------------------------------------------------------------------------- POST: api/desktop_cpus start ---------------------------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpPost]
        [Route("api/desktop_cpus")]
        public IHttpActionResult Postdesktop_cpus(CpuInvoiceModel model, string token)
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

                    var desktop_cpus = model.Desktop_cpu;
                    desktop_cpus.date_created = DateTime.Now;

                    db.desktop_cpus.Add(desktop_cpus);
                    db.SaveChanges();

                    // Created instance to insert into the invoice table
                    var CpuInvoice = new cpu_invoice
                    {
                        cpu_id = desktop_cpus.id,
                        invoice_document = model.Invoice_cpu,
                        user_created = desktop_cpus.user_created,
                        date_created = DateTime.Now
                    };
                    // Save data in the invoice table to store the invoice documents
                    db.cpu_invoice.Add(CpuInvoice);
                    db.SaveChanges();

                    transaction.Commit();

                    //return CreatedAtRoute("DefaultApi", new { id = desktop_cpus.id }, desktop_cpus);
                    return Content(HttpStatusCode.Created, new { message = "CPU created successfully.", assigned_desktops = desktop_cpus });
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(new Exception("An error occurred while saving desktop CPUs information.", innerException ?? ex));
                }
                catch (Exception ex)
                {
                    // Rollback incase of any issues in the transaction
                    transaction.Rollback();
                    return InternalServerError(new Exception("An unexpected error occurred.", ex));
                }
            }
        }

        //--------------------------------------------------------------------------------- POST: api/desktop_cpus start ---------------------------------------------------------------------------------


        // DELETE: api/desktop_cpus/5
        [ResponseType(typeof(desktop_cpus))]
        public IHttpActionResult Deletedesktop_cpus(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                desktop_cpus desktop_cpus = db.desktop_cpus.Find(id);
                if (desktop_cpus == null)
                {
                    return NotFound();
                }

                db.desktop_cpus.Remove(desktop_cpus);
                db.SaveChanges();

                return Ok(desktop_cpus);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while Deleting desktop_cpus.", Details = ex.Message });
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

        private bool desktop_cpusExists(int id)
        {
            return db.desktop_cpus.Count(e => e.id == id) > 0;
        }
    }
}