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
    public class router_mtcController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // ------------------------GET: api/router_mtc --- (GET ALL ) ------------
        [ResponseType(typeof(router_mtc))]
        [HttpGet]
        [Route("api/router_mtc")]
        public IHttpActionResult Getrouter_mtc(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Unauthorized(); // Return 401 Unauthorized response
                }

                // Return the IQueryable of router_mtc
                var routerMtcList = db.router_mtc.ToList(); // Execute query to fetch data
                return Ok(routerMtcList); // Return 200 OK with data
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return InternalServerError(ex); // Return 500 Internal Server Error with exception details
            }
        }
        //-------------------------------- GET: api/router_mtc/5 (GET A SPECIFIC)------------------------------------------------
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Getrouter_mtc(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }

                // Fetch the router_mtc object
                router_mtc router_mtc = db.router_mtc.Find(id);
                if (router_mtc == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = $"Router MTC with ID {id} not found." });
                }

                return Ok(router_mtc);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the Router MTC.", Details = ex.Message });
            }
        }


        //----------------------------------------PUT: api/router_mtc/5(UPDATE) ----------------------------------------
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putrouter_mtc(int id, router_mtc router_mtc, string token)
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
                // Retrieve the existing entity from the database
                var existingsdwanrouter = db.router_mtc.Find(id);
                if (existingsdwanrouter == null)
                {
                    //return NotFound();
                    return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
                }
                //Defining Tables that needs to be updated 

                existingsdwanrouter.serial_number = router_mtc.serial_number;
                existingsdwanrouter.tag_number = router_mtc.tag_number;
                existingsdwanrouter.model = router_mtc.model;

                db.Entry(existingsdwanrouter).State = EntityState.Modified;



                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!router_mtcExists(id))
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
                return Ok(router_mtc);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the Router MTC.", Details = ex.Message });
            }
        }


        //--------------------------------------------- api/sdwan_router/write_off/update/{id} (WRITEOFF SDWAN ROUTER START)----------------------------------------------
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/sdwan_router/write_off/update/{id}")]
        public IHttpActionResult Putsdwan_ROUTER_write_of(int id, router_mtc router_mtc, string token)
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

            // Retrieve the existing entity from the database
            var existingRouter = db.router_mtc.Find(id);
            if (existingRouter == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            // Keep the records the same
            router_mtc.serial_number = existingRouter.serial_number;
            router_mtc.tag_number = existingRouter.tag_number;
            router_mtc.model = existingRouter.model;
            router_mtc.serial_number = router_mtc.serial_number;
            router_mtc.tag_number = router_mtc.tag_number;
            router_mtc.model = router_mtc.model;

            // Always update the date_updated field
            router_mtc.date_updated = DateTime.Now;
            // Update device_status_id for the laptop
            router_mtc.status_id = 1;

            // Update specific fields
            existingRouter.comments = router_mtc.comments;
            existingRouter.attachment = router_mtc.attachment;



            db.Entry(existingRouter).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!router_mtcExists(id))
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

            return Ok(router_mtc);
        }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the Router MTC.", Details = ex.Message});
            }
        }




        //--------------------------------------------- // POST: api/router_mtc (UPDATE)----------------------------------------------
        [ResponseType(typeof(router_mtc))]
        [HttpPost]
        [Route("api/router_mtc")]
        public IHttpActionResult Postrouter_mtc(router_mtc router_mtc, string token)
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

                router_mtc.date_created = DateTime.Now;

                db.router_mtc.Add(router_mtc);
                db.SaveChanges();

                return Content(HttpStatusCode.Created, new { message = "Router MTC created successfully.", router_mtc = router_mtc });
               
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        //-------------------------------------- DELETE: api/router_mtc/5--------------------------------------
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Deleterouter_mtc(int id, string token)
        {
           
            try
            {
                // Validate token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { message = "Invalid or expired token." });
                }
                router_mtc router_mtc = db.router_mtc.Find(id);
                if (router_mtc == null)
                {
                    return NotFound();
                }

                db.router_mtc.Remove(router_mtc);
                db.SaveChanges();

                return Ok(router_mtc);

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

        private bool router_mtcExists(int id)
        {
            return db.router_mtc.Count(e => e.id == id) > 0;
        }
    }
}