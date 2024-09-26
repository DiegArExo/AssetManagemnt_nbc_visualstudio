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
    public class sdwan_laptopsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //  ----------------------------------------------------------------------GET: api/sdwan_laptops(GET ALL)-----------------------------------
        [ResponseType(typeof(sdwan_laptops))]
        [HttpGet]
        [Route("api/sdwan_laptops")]
        public IHttpActionResult Getsdwan_laptops(string token)
        {
          /*  try
            {
               // Validate the token
               if (validate_token(token))
               {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                return Ok(db.sdwan_laptops);
            }
            catch (Exception ex)
            {
              // Log the exception or handle it as needed
               return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching sdwan_laptops.", Details = ex.Message });
            }*/

            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Join sdwan_laptops with device_status to get the status_name
                var result = from sdwanlaptop in db.sdwan_laptops
                             join status in db.device_status on sdwanlaptop.status_id equals status.id
                             orderby sdwanlaptop.date_created descending
                             select new
                             {
                                 sdwan_laptop = sdwanlaptop,
                                 
                                 StatusName = status.name
                             };

                return Ok(result.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching sdwan_laptops.", Details = ex.Message });
            }
        }

        //  ----------------------------------------------------------------------GET: api/sdwan_laptops(GET AVAILABLE)-----------------------------------
        [ResponseType(typeof(sdwan_laptops))]
        [HttpGet]
        [Route("api/sdwan_laptops_available")]
        public IHttpActionResult Getsdwan_laptops_Available(string token)
        {
             try
              {
                 // Validate the token
                 if (validate_token(token))
                 {
                      return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                  }

                  return Ok(db.sdwan_laptops.Where(s=>s.status_id==1));
              }
              catch (Exception ex)
              {
                // Log the exception or handle it as needed
                 return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching sdwan_laptops.", Details = ex.Message });
              }

           
        }
        //  ----------------------------------------------------------------------GET: api/sdwan_laptops(GET AVAILABLE) END-----------------------------------

        //  ----------------------------------- -----------------------------------GET: api/sdwan_laptops/5(GET A SPECIFIC) -----------------------------------  
        [ResponseType(typeof(sdwan_laptops))]
        [HttpGet]
        [Route("api/sdwan_laptops/{id}")]
        public IHttpActionResult Getsdwan_laptops(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }

                sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
                if (sdwan_laptops == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = $"No SDWAN laptop found with id = {id}" });
                }

                // return Ok(sdwan_laptops);
                return Ok(sdwan_laptops);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the Sdwan Laptop /", Details = ex.Message });
            }
        }

        // --------------------------------------------  PUT: api/sdwan_laptops/5(UPDATE SDWAN) --------------------------------------------------------  
        
        [ResponseType(typeof(void))]
        [Route("api/sdwan_laptops/{id}")]
        [HttpPut]
        public IHttpActionResult Putsdwan_laptops(int id, sdwan_laptops sdwan_laptops, string token)
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
            var existingsdwanlaptop = db.sdwan_laptops.Find(id);
            if (existingsdwanlaptop == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }
            //Defining Tables that needs to be updated 
            existingsdwanlaptop.brand_name = sdwan_laptops.brand_name;
            existingsdwanlaptop.serial_number = sdwan_laptops.serial_number;
            existingsdwanlaptop.tag_number = sdwan_laptops.tag_number;
            existingsdwanlaptop.model = sdwan_laptops.model;
            existingsdwanlaptop.domain_pc_name = sdwan_laptops.domain_pc_name;
            existingsdwanlaptop.Processors = sdwan_laptops.Processors;
            existingsdwanlaptop.Year = sdwan_laptops.Year;

                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingsdwanlaptop.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                db.Entry(existingsdwanlaptop).State = EntityState.Modified;
            

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sdwan_laptopsExists(id))
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
            return Ok(sdwan_laptops);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing sdwan.", Details = ex.Message });
            }
        }

        //--------------------------------------------- api/sdwan_laptop/write_off/update/{id} (WRITEOFF SDWAN LAPTOP START)----------------------------------------------
        [ResponseType(typeof(void))]
        [Route("api/sdwan_laptop/write_off/update/{id}")]
        public IHttpActionResult Putsdwan_laptop_write_off(int id, sdwan_laptops sdwan_laptop, string token)
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
            var existingSDWANLAPTOP = db.sdwan_laptops.Find(id);
            if (existingSDWANLAPTOP == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            try
            {
                // Keep existing values for non-updated fields
                sdwan_laptop.model = existingSDWANLAPTOP.model;
                sdwan_laptop.brand_name = existingSDWANLAPTOP.brand_name;
                sdwan_laptop.serial_number = existingSDWANLAPTOP.serial_number;
                sdwan_laptop.tag_number = existingSDWANLAPTOP.tag_number;
                sdwan_laptop.user_created = existingSDWANLAPTOP.user_created;
                sdwan_laptop.date_created = existingSDWANLAPTOP.date_created;
               // sdwan_laptop.user_updated = existingSDWANLAPTOP.user_updated;
                sdwan_laptop.date_updated = DateTime.Now;
               // sdwan_laptop.status_id = 2;

                    // Update only the necessary fields
                    existingSDWANLAPTOP.status_id = 5;
                    existingSDWANLAPTOP.comments = sdwan_laptop.comments;
                existingSDWANLAPTOP.attachment = sdwan_laptop.attachment;
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        existingSDWANLAPTOP.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }

                    db.Entry(existingSDWANLAPTOP).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sdwan_laptopsExists(id))
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

            return Ok(existingSDWANLAPTOP);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing sdwan.", Details = ex.Message });
            }
        }



        //--------------------------------------------- api/sdwan_laptop/write_off/update/{id} (WRITEOFF SDWAN LAPTOP END)----------------------------------------------
        [ResponseType(typeof(sdwan_laptops))]
        [HttpPost]
        [Route("api/sdwan_laptops")]
        public IHttpActionResult Postsdwan_laptops(sdwan_laptops sdwan_laptops, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                sdwan_laptops.date_created = DateTime.Now;
                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    sdwan_laptops.user_created = authenticatedUserId.Value;
                    sdwan_laptops.user_updated = authenticatedUserId.Value;
                }
                db.sdwan_laptops.Add(sdwan_laptops);
                db.SaveChanges();
                return Content(HttpStatusCode.Created, new { message = "SDWAN laptop created successfully.", sdwan_laptops = sdwan_laptops });
               
            }
            catch (Exception ex)
            {
                // Log the exception details for further investigation
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    System.Diagnostics.Debug.WriteLine("Inner Exception Stack Trace: " + ex.InnerException.StackTrace);
                }

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing sdwan.", Details = ex.Message });
            }
        }
        //--------------------------------------------- api/sdwan_laptop/write_off/update/{id} (WRITEOFF SDWAN LAPTOP END)----------------------------------------------


        //------------------------------------------------------------------------------------------------POST(REPAIR): api/sdwan_laptops start-------------------------------------------------------
        [ResponseType(typeof(sdwan_laptops))]
        [HttpPost]
        [Route("api/sdwan_laptops/repair")]
        public IHttpActionResult PostSdwan_laptop_repair(sdwan_laptop_repair sdwan_laptop_repair, string token)
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

                    sdwan_laptop_repair.date_created = DateTime.Now;
                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        sdwan_laptop_repair.user_created = authenticatedUserId.Value; // Set to authenticated user
                        sdwan_laptop_repair.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }
                    db.sdwan_laptop_repair.Add(sdwan_laptop_repair);
                    db.SaveChanges();


                    // Update the laptop table
                    var sdwanlaptops = db.sdwan_laptops.SingleOrDefault(d => d.id == sdwan_laptop_repair.sdwan_laptop_id);
                    if (sdwanlaptops != null)
                    {
                        sdwanlaptops.status_id = 11; // Set to 11 (or any field you need to update)
                        db.SaveChanges();
                    }
                    else
                    {

                        transaction.Rollback();
                        return NotFound(); // or return a more specific message
                    }

                    transaction.Commit();
                    return Content(HttpStatusCode.Created, new { message = "Laptop allocted for repairs successfully.", sdwan_laptop_repair = sdwan_laptop_repair });
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(new Exception("An error occurred while saving desktop and monitor information.", innerException ?? ex));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return InternalServerError(new Exception("An unexpected error occurred.", ex));
                }
            }
        }
        //------------------------------------------------------------------------------------------------POST(REPAIR): api/sdwan_laptops End-------------------------------------------------------




        //------------------------------------------------------ UPDATE SDWAN LAPTOP TO BE AVAILABLE START -----------------------------------------------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/sdwan_laptops/update_sdwan_laptops_status_available/{id}")]
        public IHttpActionResult PutSdwanLaptopsStatus(int id, string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var existingSdwanLaptop = db.sdwan_laptops.Find(id);
                if (existingSdwanLaptop == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Sdwan laptops with ID {id} not found." });
                }

                existingSdwanLaptop.status_id = 1;
                // Get authenticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingSdwanLaptop.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                db.Entry(existingSdwanLaptop).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Content(HttpStatusCode.OK, new { Message = "Laptop status successfully updated to 1.", SdwanLaptopsStatus = existingSdwanLaptop });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (existingSdwanLaptop == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = "Laptop with the provided ID was not found." });
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
        //------------------------------------------------------ UPDATE SDWAN LAPTOP TO BE AVAILABLE END ------------------------------------------------------------------------------
        // DELETE: api/sdwan_laptops/5
        [ResponseType(typeof(sdwan_laptops))]
        [Route("api/sdwan_laptops/{id}")]
        [HttpDelete]
        public IHttpActionResult Deletesdwan_laptops(int id, string token)
        {

            try
            {
                // Validate token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }

                sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
                if (sdwan_laptops == null)
                {
                    return NotFound();
                }

                db.sdwan_laptops.Remove(sdwan_laptops);
                db.SaveChanges();

                return Ok(sdwan_laptops);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
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

        private bool sdwan_laptopsExists(int id)
        {
            return db.sdwan_laptops.Count(e => e.id == id) > 0;
        }
    }
}