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
        // -------------------------------------GET: api/desktop_cpus(AVAILABLE)-------------------------------------------------------------------
        // GET: api/desktop_cpus
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_cpus_available")]
        public IHttpActionResult Getdesktop_cpus_available(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }


                var desktop_cpus = db.desktop_cpus.Where(c=> c.status_id == 1);
                return Ok(desktop_cpus);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving desktop_cpus.", Details = ex.Message });
            }
        }
        // -------------------------------------GET: api/desktop_cpus(AVAILABLE)-------------------------------------------------------------------

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
            existingDesktopsCPU.Processors = desktop_cpus.Processors;
            existingDesktopsCPU.Year = desktop_cpus.Year;
            existingDesktopsCPU.domain_pc_name = desktop_cpus.domain_pc_name;

                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingDesktopsCPU.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

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

                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    desktop_cpus.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

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
                              orderby l_cpu.date_created descending
                              select new
                          {
                              l_cpu.id,
                              l_cpu.brand_name,
                              l_cpu.model,
                              l_cpu.cpu_serial_number,
                              l_cpu.cpu_tag_number,
                              l_cpu.Processors,
                              l_cpu.Year,


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

                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        desktop_cpus.user_created = authenticatedUserId.Value; // Set to authenticated user
                        desktop_cpus.user_updated = authenticatedUserId.Value; // Set to authenticated user

                    }

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

        //--------------------------------------------------------------------------------- POST: api/desktop_cpus end ---------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------- POST(REPAIR): api/desktop_cpus start ---------------------------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpPost]
        [Route("api/desktop_cpus/repair")]
        public IHttpActionResult Postdesktop_cpus_repair(cpu_repair cpu_repair, string token)
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


                    cpu_repair.date_created = DateTime.Now;

                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        cpu_repair.user_created = authenticatedUserId.Value; // Set to authenticated user
                        cpu_repair.user_updated = authenticatedUserId.Value; // Set to authenticated user

                    }

                    db.cpu_repair.Add(cpu_repair);
                    db.SaveChanges();

                    // Update the desktop_monitors table
                    var desktopCpu = db.desktop_cpus.SingleOrDefault(d => d.id == cpu_repair.cpu_id);
                    if (desktopCpu != null)
                    {
                        desktopCpu.status_id = 11; // Set to 11 (or any field you need to update)
                        db.SaveChanges();
                    }
                    else
                    {
                        // If no matching desktop monitor is found, handle as needed (e.g., log warning)
                        // For now, just roll back the transaction
                        transaction.Rollback();
                        return NotFound(); // or return a more specific message
                    }


                    transaction.Commit();




                    //return CreatedAtRoute("DefaultApi", new { id = desktop_cpus.id }, desktop_cpus);
                    return Content(HttpStatusCode.Created, new { message = "CPU Allocted for repair successfully.", assigned_desktops = cpu_repair });
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(new Exception("An error occurred while saving desktop CPUs information.", innerException ?? ex));
                }
                catch (Exception ex)
                {

                    return InternalServerError(new Exception("An unexpected error occurred.", ex));
                }

            }
        }

        //--------------------------------------------------------------------------------- POST(REPAIR): api/desktop_cpus end ---------------------------------------------------------------------------------

        //------------------------------------------------------ UPDATE CPU TO BE AVAILABLE START -----------------------------------------------------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/desktop_cpus/update_cpu_status_available/{id}")]
        public IHttpActionResult PutDesktopCpuStatus(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Retrieve the existing entity from the database
                var existingDesktopCpu = db.desktop_cpus.Find(id);
                if (existingDesktopCpu == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"CPU with ID {id} not found." });
                }

                // Update the status to 1
                existingDesktopCpu.status_id = 1;

                // Get authenticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingDesktopCpu.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                db.Entry(existingDesktopCpu).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Content(HttpStatusCode.OK, new { Message = "CPU status successfully updated to 1.", DesktopCpu = existingDesktopCpu });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesktopCpuExists(id))
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = "CPU with the provided ID was not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.InnerException;
                    return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating the CPU status.", Error = innerException?.Message ?? ex.Message });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        private bool DesktopCpuExists(int id)
        {
            return db.desktop_cpus.Count(e => e.id == id) > 0;
        }

        //------------------------------------------------------ UPDATE CPU TO BE AVAILABLE END -------------------------------------------------------------------------------------------
        //------------------------------------------------------ GET ALL CPU (TOTAL)-------------------------------------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_cpus/get_all_cpu")]
        public IHttpActionResult CountDesktopCPUs(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count all records in the desktop_cpus table
                var desktopCPUsCount = db.desktop_cpus.Count();

                return Ok(new { total_cpu = desktopCPUsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        //------------------------------------------------------ GET ALL CPU (TOTAL)-------------------------------------------------------------------------------------------

        //------------------------------------------------------ GET ALL AVAILABLE CPU (TOTAL) START-----------------------------------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_cpus/not_in_joined_desktops")]
        public IHttpActionResult CountCPUsNotInJoinedDesktops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count CPUs that are not in joined_desktops_monitors
                var cpusNotInJoinedDesktopsCount = db.desktop_cpus
                    .Count(cpu => !db.joined_desktops_monitors.Any(jdm => jdm.desktop_cpu_id == cpu.id));

                return Ok(new { total_available_cpu = cpusNotInJoinedDesktopsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }


        //------------------------------------------------------GET ALL ASSIGNED CPU (TOTAL) END-------------------------------------------------------------------------------------------
        [ResponseType(typeof(desktop_cpus))]
        [HttpGet]
        [Route("api/desktop_cpus/in_joined_desktops")]
        public IHttpActionResult CountCPUsInJoinedDesktops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count CPUs that are in joined_desktops_monitors
                var cpusInJoinedDesktopsCount = db.desktop_cpus
                    .Count(cpu => db.joined_desktops_monitors.Any(jdm => jdm.desktop_cpu_id == cpu.id));

                return Ok(new { total_assigned_cpu = cpusInJoinedDesktopsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //------------------------------------------------------ GET ALL ASSIGNED CPU (TOTAL) END-------------------------------------------------------------------------------------------

        //-------------------------------------------------GET CPU INVOICE START----------------------------------------------

        [Route("api/desktop_cpus/get_incoice")]
        [HttpGet]
        public IHttpActionResult GetCpuInvoiceAttachment(int cpuId, string token)
        {

            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var cpuInoive = db.cpu_invoice
                                   .Where(invoidId => invoidId.cpu_id == cpuId)
                                   .FirstOrDefault();

                if (cpuInoive == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "Invoice not found." });
                }

                return Ok(cpuInoive);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the attachment.", Details = ex.Message });
            }

        }




        //-------------------------------------------------GET CPU INVOICE END----------------------------------------------

        //-------------------------------------------------GET CPU SIGNED OUT DOCUMENT START----------------------------------------------
        [Route("api/desktop_cpus/get_sign_out_doc")]
        [HttpGet]
        public IHttpActionResult GetCpuOutDoc(int cpuId, int userId, string token)
        {

            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var laptopInoive = db.sign_out_desktop_cpu
                                   .Where(cpu => cpu.desktop_cpu_id == cpuId && cpu.user_id == userId)
                                   .OrderByDescending(sigout => sigout.date_created)
                                    .FirstOrDefault();


                if (laptopInoive == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "Sign out document not found." });
                }

                return Ok(laptopInoive);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the attachment.", Details = ex.Message });
            }

        }


        //-------------------------------------------------GET CPU SIGNED OUT DOCUMENT END----------------------------------------------

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