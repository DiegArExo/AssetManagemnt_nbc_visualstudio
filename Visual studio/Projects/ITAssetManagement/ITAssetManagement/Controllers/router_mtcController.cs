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
            //try
            //{
            //    // Validate the token
            //    if (validate_token(token))
            //    {
            //        return Unauthorized(); // Return 401 Unauthorized response
            //    }

            //    // Return the IQueryable of router_mtc
            //    var routerMtcList = db.router_mtc.ToList(); // Execute query to fetch data
            //    return Ok(routerMtcList); // Return 200 OK with data
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception or handle it as needed
            //    return InternalServerError(ex); // Return 500 Internal Server Error with exception details
            //}

            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Join sdwan_laptops with device_status to get the status_name
                var result = from sdwanRouter in db.router_mtc
                             join status in db.device_status on sdwanRouter.status_id equals status.id
                             orderby sdwanRouter.date_created descending
                             select new
                             {
                                 sdwan_router = sdwanRouter,

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
        // ------------------------GET: api/router_mtc --- (GET AVAILABLE ) START -----------------------------------------------
        [ResponseType(typeof(router_mtc))]
        [HttpGet]
        [Route("api/router_mtc_available")]
        public IHttpActionResult Getrouter_mtc_available(string token)
        {
            try
            {
               // Validate the token
                if (validate_token(token))
               {
                    return Unauthorized(); // Return 401 Unauthorized response
                }

                
               return Ok(db.router_mtc.Where(ro=>ro.status_id==1)); // Return 200 OK with data
             }
            catch (Exception ex)
             {
                 // Log the exception or handle it as needed
                return InternalServerError(ex); 
            }

           
        }
        // ------------------------GET: api/router_mtc --- (GET AVAILABLE ) END -----------------------------------------------



        //-------------------------------- GET: api/router_mtc/5 (GET A SPECIFIC)------------------------------------------------
        [ResponseType(typeof(router_mtc))]
        [HttpGet]
        [Route("api/router_mtc/{id}")]
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
       
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/router_mtc/{id}")]
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
                existingsdwanrouter.Year = router_mtc.Year;
                existingsdwanrouter.Processors = router_mtc.Processors;

                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingsdwanrouter.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

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
           

                // Update specific fields
                existingRouter.status_id = 5;
                existingRouter.comments = router_mtc.comments;
            existingRouter.attachment = router_mtc.attachment;

                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingRouter.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }



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
                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    router_mtc.user_created = authenticatedUserId.Value;
                    router_mtc.user_updated = authenticatedUserId.Value;
                }
                db.router_mtc.Add(router_mtc);
                db.SaveChanges();

                return Content(HttpStatusCode.Created, new { message = "Router MTC created successfully.", router_mtc = router_mtc });
               
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        //------------------------------------------------------ UPDATE SDWAN ROUTER TO BE AVAILABLE START -----------------------------------------------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/router_mtc/update_router_status_available/{id}")]
        public IHttpActionResult PutSdwanRouterStatus(int id, string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var existingSdwanRouter = db.router_mtc.Find(id);
                if (existingSdwanRouter == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Sdwan laptops with ID {id} not found." });
                }

                existingSdwanRouter.status_id = 1;
                // Get authenticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingSdwanRouter.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                db.Entry(existingSdwanRouter).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    return Content(HttpStatusCode.OK, new { Message = "Router status successfully updated to 1.", SdwanRouterStatus = existingSdwanRouter });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (existingSdwanRouter == null)
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = "Router with the provided ID was not found." });
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
        //------------------------------------------------------ UPDATE SDWAN ROUTER TO BE AVAILABLE END ------------------------------------------------------------------------------
       

        //-------------------------------------- DELETE: api/router_mtc/5--------------------------------------
        [ResponseType(typeof(router_mtc))]
        [HttpDelete]
        [Route("api/router_mtc/{id}")]
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