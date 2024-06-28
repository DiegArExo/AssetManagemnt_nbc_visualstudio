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
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Return IQueryable of firewalls
                return Ok(db.firewalls);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching firewalls.", Details = ex.Message });
            }
        }


        //------------------------------------------GET: api/firewalls/5 (GET A SPECIFIC)-------------------------------  
        [ResponseType(typeof(firewall))]
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
        [HttpPut]
        [ResponseType(typeof(void))]
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
        [HttpPut]
        [ResponseType(typeof(void))]
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
            firewall.status_id = 1;

            // Update specific fields
            existingFirewall.comments = firewall.comments;
            existingFirewall.attachment = firewall.attachment;



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

            db.firewalls.Add(firewall);
            db.SaveChanges();
                return Content(HttpStatusCode.Created, new { message = "Firewall created successfully.", firewall = firewall });
                //return CreatedAtRoute("DefaultApi", new { id = firewall.id }, firewall);
            }
            catch (Exception ex)
            {
               
                
                return InternalServerError(new Exception("An unexpected error occurred.", ex));
            }
        }

        // DELETE: api/firewalls/5
        [ResponseType(typeof(firewall))]
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