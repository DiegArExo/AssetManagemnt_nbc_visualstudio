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
    public class assigned_sdwansController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //---------------------------------- GET: api/assigned_sdwans(GET ALL) ------------------------------------
        [ResponseType(typeof(assigned_sdwans))]
        [HttpGet]
        [Route("api/assigned_sdwans")]
        public IHttpActionResult Getassigned_sdwans(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Query to retrieve assigned_sdwans data
                var assigned_sdwans = db.assigned_sdwans;

                // Return the queried data
                return Ok(assigned_sdwans);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving assigned_sdwans data.", Details = ex.Message });
            }
        }


        // ----------------------------------------------------------------------GET: api/assigned_sdwans/5 ---------------------------------------------------------------
        [ResponseType(typeof(assigned_sdwans))]
        public IHttpActionResult Getassigned_sdwans(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                assigned_sdwans assigned_sdwans = db.assigned_sdwans.Find(id);
                if (assigned_sdwans == null)
                {
                    return NotFound();
                }

 
                return Ok(assigned_sdwans);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving assigned_sdwans data.", Details = ex.Message });
            }
        }

        //------------------------------------------ PUT: api/assigned_sdwans/5(UPDATE )------------------------------------------------------------------------------------
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_sdwans(int id, assigned_sdwans assigned_sdwans, string token)
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

               
                if (id != assigned_sdwans.id)
                {
                    return BadRequest("ID mismatch between URL and data.");
                }

                
                db.Entry(assigned_sdwans).State = EntityState.Modified;

                try
                {
                     
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                     
                    if (!assigned_sdwansExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(assigned_sdwans);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating assigned_sdwans data.", Details = ex.Message });
            }
           
        }

        // POST: api/assigned_sdwans
        [ResponseType(typeof(assigned_sdwans))]
        [HttpPost]
        [Route("api/assigned_sdwans")]
        public IHttpActionResult Postassigned_sdwans(assigned_sdwans assigned_sdwans, string token)
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

                // Check if the sdwan_id already exists
                var existingSdwans = db.assigned_sdwans.FirstOrDefault(a => a.sdwan_id == assigned_sdwans.sdwan_id);
                if (existingSdwans != null)
                {
                    return Content(HttpStatusCode.Conflict, new { message = "SDWAN has already been assigned." });
                }

                // Set date_created to current date and time
                assigned_sdwans.date_created = DateTime.Now;
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    assigned_sdwans.user_updated = authenticatedUserId.Value;
                    assigned_sdwans.user_created = authenticatedUserId.Value;
                }
                db.assigned_sdwans.Add(assigned_sdwans);
                db.SaveChanges();

                // return CreatedAtRoute("DefaultApi", new { id = assigned_sdwans.id }, assigned_sdwans);
                return Content(HttpStatusCode.Created, new { message = "SDWAN Assigned successfully.", assigned_sdwans = assigned_sdwans });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in another way (e.g., return an error response)
               // return InternalServerError(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
            }
        }

        //-------------------------------------------- DELETE: un- Assign SDWAN ------------------------------------------------------
        [ResponseType(typeof(assigned_sdwans))]
        [HttpDelete]
        [Route("api/un_assigned_sdwans/{sdwan_id}")]
        public IHttpActionResult Delete_Unassing_sdwans(int sdwan_id, string token)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                  
                    if (validate_token(token))
                    {
                        return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                    }

                
                    var assigned_sdwans = db.assigned_sdwans.FirstOrDefault(a => a.sdwan_id == sdwan_id);
                    if (assigned_sdwans == null)
                    {
          
                        return Content(HttpStatusCode.NotFound, new { message = "SDWAN is not assigned." });
                    }

      
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        assigned_sdwans.user_updated = authenticatedUserId.Value;
                        db.Entry(assigned_sdwans).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                    db.assigned_sdwans.Remove(assigned_sdwans);
                    db.SaveChanges();

                    // Commit the transaction
                    transaction.Commit();

                    return Ok(new { message = "SDWAN has successfully been unassigned.", assigned_sdwans = assigned_sdwans });
                }
                catch (Exception ex)
                {
                    // Rollback the transaction incase somthing goes wrong 
                    transaction.Rollback();

                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while deleting the assigned SD-WAN.", error = ex.Message });
                }
            }
        }
        //-------------------------------------------- DELETE: un- Assign SDWAN ------------------------------------------------------


        // DELETE: api/assigned_sdwans/5
        [ResponseType(typeof(assigned_sdwans))]
        public IHttpActionResult Deleteassigned_sdwans(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
            
            assigned_sdwans assigned_sdwans = db.assigned_sdwans.Find(id);
            if (assigned_sdwans == null)
            {
                return NotFound();
            }

            db.assigned_sdwans.Remove(assigned_sdwans);
            db.SaveChanges();

            return Ok(assigned_sdwans);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while deleting the assigned_laptops record.", Details = ex.Message });
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

        private bool assigned_sdwansExists(int id)
        {
            return db.assigned_sdwans.Count(e => e.id == id) > 0;
        }
    }
}