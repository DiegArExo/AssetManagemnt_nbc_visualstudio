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
    public class firewallsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //------- ----- ----- ----- GET: api/firewalls(GET ALL)-------------------------------  
         [ResponseType(typeof(firewall))]
         [HttpGet]
         [Route("api/firewalls")]
         public IHttpActionResult Getfirewalls(string token)
         {
            //try
            //{
            //    // Validate the token
            //    if (validate_token(token))
            //    {
            //        return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
            //    }

            //    // Return IQueryable of firewalls
            //    return Ok(db.firewalls);
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception or handle it as needed
            //    return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching firewalls.", Details = ex.Message });
            //}
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Join sdwan_laptops with device_status to get the status_name
                var result = from sdwanFirewall in db.firewalls
                             join status in db.device_status on sdwanFirewall.status_id equals status.id
                             orderby sdwanFirewall.date_created descending
                             select new
                             {
                                 sdwan_firewall = sdwanFirewall,

                                 StatusName = status.name
                             };

                return Ok(result.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching sdwan Router.", Details = ex.Message });
            }
        }

        //------- ----- ----- ----- GET: api/firewalls(GET AVAILABLE) START-------------------------------  
        [ResponseType(typeof(firewall))]
        [HttpGet]
        [Route("api/firewalls_available")]
        public IHttpActionResult Getfirewalls_Available(string token)
        {
            try
            {
               // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Return IQueryable of firewalls
                return Ok(db.firewalls.Where(f=>f.status_id==1));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching firewalls.", Details = ex.Message });
            }
           
        }
        //------- ----- ----- ----- GET: api/firewalls(GET AVAILABLE) END-------------------------------  


        //------------------------------------------GET: api/firewalls/5 (GET A SPECIFIC)-------------------------------  
        [ResponseType(typeof(firewall))]
         [HttpGet]
         [Route("api/firewalls/{id}")]
         public IHttpActionResult Getfirewall(int id, string token)
         {
             try
             {
                 // Validate the token
                 if (validate_token(token))
                 {
                     return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                 }


                 firewall firewall = db.firewalls.Find(id);
                 if (firewall == null)
                 {
                     return Content(HttpStatusCode.NotFound, new { Message = $"Firewall with ID {id} not found." });
                 }

                 return Ok(firewall);
             }
             catch (Exception ex)
             {
                 return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving firewall information.", Details = ex.Message });
             }
         }

         //----------------------------------------------------  PUT: api/firewalls/5 (UPDATE INFORMATION)---------------------------------------------------- 

       [ResponseType(typeof(void))]
         [HttpPut]
         [Route("api/firewalls/{id}")]
         public IHttpActionResult Putfirewall(int id, firewall firewall, string token)
         {
             try {

                 // Validate the token
                 if (validate_token(token))
                 {
                     return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                 }

                 if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }
             var existingsdwanfirewall = db.firewalls.Find(id);
             if (existingsdwanfirewall == null)
             {
                 //return NotFound();
                 return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
             }

             //Defining Tableas that needs to be updated 

             existingsdwanfirewall.serial_number = firewall.serial_number;
             existingsdwanfirewall.tag_number = firewall.tag_number;
             existingsdwanfirewall.model = firewall.model;
             existingsdwanfirewall.comments = firewall.comments;
             existingsdwanfirewall.Year = firewall.Year;
             existingsdwanfirewall.Processors = firewall.Processors;

                int? authenticatedUserId = GetUserIdFromToken(token);
                 if (authenticatedUserId.HasValue)
                 {
                     existingsdwanfirewall.user_updated = authenticatedUserId.Value; // Set to authenticated user
                 }

                 db.Entry(existingsdwanfirewall).State = EntityState.Modified;

             try
             {
                 db.SaveChanges();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!firewallExists(id))
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

             // return StatusCode(HttpStatusCode.NoContent);
             return Ok(firewall);
         }
             catch (Exception ex)
             {
                 return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while Updationg firewall information.", Details = ex.Message });
             }
         }


        //--------------------------------------------- api/sdwan_firewall/write_off/update/{id} (WRITEOFF SDWAN firewall START)----------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/sdwan_firewall/write_off/update/{id}")]
        public IHttpActionResult Putsdwan_FIREWALL_write_of(int id, firewall firewall, string token)
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
            var existingFirewall = db.firewalls.Find(id);
            if (existingFirewall == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            // Keep the records the same
            firewall.serial_number = existingFirewall.serial_number;
            firewall.tag_number = existingFirewall.tag_number;
            firewall.model = existingFirewall.model;
            firewall.serial_number = existingFirewall.serial_number;
            firewall.tag_number = existingFirewall.tag_number;
     

            // Always update the date_updated field
            firewall.date_updated = DateTime.Now;
            // Update device_status_id for the laptop
         

                // Update specific fields
                existingFirewall.status_id = 5;
            existingFirewall.comments = firewall.comments;
            existingFirewall.attachment = firewall.attachment;
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingFirewall.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }


                db.Entry(existingFirewall).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!firewallExists(id))
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

            return Ok(firewall);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while Updationg firewall information.", Details = ex.Message });
            }
        }

        //--------------------------------------------- api/sdwan_firewall/write_off/update/{id} (WRITEOFF SDWAN firewall END)----------------------------------------------

 


        //--------------------------------------------- POST: api/firewalls(POST SDWAN firewall Informarion )---------------------------------------------
        [ResponseType(typeof(firewall))]
        [HttpPost]
        [Route("api/firewalls")]
        public IHttpActionResult Postfirewall(firewall firewall, string token)
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
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

                firewall.date_created = DateTime.Now;
                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    firewall.user_created = authenticatedUserId.Value;
                    firewall.user_updated = authenticatedUserId.Value;
                }

                db.firewalls.Add(firewall);
            db.SaveChanges();
                return Content(HttpStatusCode.Created, new { message = "Firewall created successfully.", firewall = firewall });
              
               
            }
            catch (Exception ex)
            {
               
                
                return InternalServerError(new Exception("An unexpected error occurred.", ex));
            }
        }
        //--------------------------------------------- POST: api/firewalls(POST SDWAN firewall Informarion )---------------------------------------------


        //------------------------------------------------------------------------------------------------POST(REPAIR): api/sdwan_firewall_repair start-------------------------------------------------------
        [ResponseType(typeof(sdwan_firewall_repair))]
        [HttpPost]
        [Route("api/sdwan_firewall_repair/repair")]
        public IHttpActionResult PostSdwan_firewall_repair(sdwan_firewall_repair sdwan_firewall_repair, string token)
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

                    sdwan_firewall_repair.date_created = DateTime.Now;
                    //Get authernticated user id and save it
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        sdwan_firewall_repair.user_created = authenticatedUserId.Value; // Set to authenticated user
                        sdwan_firewall_repair.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }
                    db.sdwan_firewall_repair.Add(sdwan_firewall_repair);
                    db.SaveChanges();


                    // Update the laptop table
                    var sdwanFirewall = db.firewalls.SingleOrDefault(d => d.id == sdwan_firewall_repair.sdwan_firewall_id);
                    if (sdwanFirewall != null)
                    {
                        sdwanFirewall.status_id = 11; // Set to 11 (or any field you need to update)
                        db.SaveChanges();
                    }
                    else
                    {

                        transaction.Rollback();
                        return NotFound(); // or return a more specific message
                    }

                    transaction.Commit();
                    return Content(HttpStatusCode.Created, new { message = "Firewall allocted for repairs successfully.", sdwan_firewall_repairs = sdwan_firewall_repair });
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.InnerException;
                    return InternalServerError(new Exception("An error occurred while saving Firewall information.", innerException ?? ex));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return InternalServerError(new Exception("An unexpected error occurred.", ex));
                }
            }
        }
        //------------------------------------------------------------------------------------------------POST(REPAIR): api/sdwan_firewall_repair End-------------------------------------------------------

        //------------------------------------------------------ UPDATE SDWAN FIREWALL TO BE AVAILABLE START -----------------------------------------------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/firewalls/update_firewalls_status_available/{id}")]
        public IHttpActionResult PutSdwanFirewallStatus(int id, string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var existingSdwanFirewall = db.firewalls.Find(id);
                if (existingSdwanFirewall == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Sdwan Firewall with ID {id} not found." });
                }

                existingSdwanFirewall.status_id = 1;
                // Get authenticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingSdwanFirewall.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                db.Entry(existingSdwanFirewall).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Content(HttpStatusCode.OK, new { Message = "Firewall status successfully updated to 1.", SdwanFirewallStatus = existingSdwanFirewall });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (existingSdwanFirewall == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = "Firewall with the provided ID was not found." });
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
        //------------------------------------------------------ UPDATE SDWAN FIREWALL TO BE AVAILABLE END ------------------------------------------------------------------------------

        // DELETE: api/firewalls/5
        [ResponseType(typeof(firewall))]
        [HttpDelete]
        [Route("api/firewalls/{id}")]
        public IHttpActionResult Deletefirewall(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                firewall firewall = db.firewalls.Find(id);
                if (firewall == null)
                {
                    return NotFound();
                }

                db.firewalls.Remove(firewall);
                db.SaveChanges();

                return Ok(firewall);
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

        private bool firewallExists(int id)
        {
            return db.firewalls.Count(e => e.id == id) > 0;
        }
    }
}