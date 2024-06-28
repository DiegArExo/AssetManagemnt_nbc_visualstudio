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
    public class loaned_sdwansController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //------------------------------------------ GET: api/loaned_sdwans(GET ALL) ----------------------------------------------------
        [ResponseType(typeof(loaned_sdwans))]
        [HttpGet]
        [Route("api/loaned_sdwans")]
        public IHttpActionResult Getloaned_sdwans(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Return IQueryable of loaned_sdwans
                return Ok(db.loaned_sdwans);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching loaned_sdwans.", Details = ex.Message });
            }
        }


        //--------------------------------------------------- GET: api/loaned_sdwans/5 (GET A SPECIFIC) ----------------------------------------------------
        [ResponseType(typeof(loaned_sdwans))]
        public IHttpActionResult Getloaned_sdwans(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                loaned_sdwans loaned_sdwans = db.loaned_sdwans.Find(id);

                if (loaned_sdwans == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = $"Loaned SDWAN with ID {id} not found." });
                }

                return Ok(loaned_sdwans);

                
            }
            catch (Exception ex)
            {
      
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        //-------------------------------------------------------------------- PUT: api/loaned_sdwans/5(UPDATE) ---------------------------------------------- 
        [ResponseType(typeof(void))]
        public IHttpActionResult Putloaned_sdwans(int id, loaned_sdwans loaned_sdwans, string token)
        {
            try {

                // Validate the token
                if (validate_token(token))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new { message = "Invalid model state", errors = ModelState });
            }

            if (id != loaned_sdwans.id)
            {
                return Content(HttpStatusCode.BadRequest, new { message = "ID mismatch" });
            }

            db.Entry(loaned_sdwans).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!loaned_sdwansExists(id))
                {
                    return Content(HttpStatusCode.NotFound, new { message = $"Loaned SDWAN with ID {id} not found." });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new { message = "Concurrency error occurred while updating the loaned SDWAN." });
                }
            }
        }catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while updating the loaned SDWAN.", details = ex.Message });
            }

            return Ok(loaned_sdwans);
        }

        // -----------------------------POST: api/loaned_sdwans ------------------------------------------------------------ 
        [ResponseType(typeof(loaned_sdwans))]
        [HttpPost]
        [Route("api/loaned_sdwans")]
        public IHttpActionResult Postloaned_sdwans(loaned_sdwans loaned_sdwans, string token)
        {
            try
            {
                // Validate token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }

                // Validate model state
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "Invalid model state", errors = ModelState });
                }


                db.loaned_sdwans.Add(loaned_sdwans);
                db.SaveChanges();


                return Content(HttpStatusCode.Created, new { message = "Loaned SDWAN information successfully created.", loaned_sdwans = loaned_sdwans });
            }
            catch (DbUpdateException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the loaned SDWAN.", details = ex.Message });
            }
        
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }

        }

        // DELETE: api/loaned_sdwans/5
        [ResponseType(typeof(loaned_sdwans))]
        public IHttpActionResult Deleteloaned_sdwans(int id, string token)
        {
           
            try
            {
                // Validate token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }
                loaned_sdwans loaned_sdwans = db.loaned_sdwans.Find(id);
                if (loaned_sdwans == null)
                {
                    return NotFound();
                }

                db.loaned_sdwans.Remove(loaned_sdwans);
                db.SaveChanges();

                return Ok(loaned_sdwans);

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

        private bool loaned_sdwansExists(int id)
        {
            return db.loaned_sdwans.Count(e => e.id == id) > 0;
        }
    }
}