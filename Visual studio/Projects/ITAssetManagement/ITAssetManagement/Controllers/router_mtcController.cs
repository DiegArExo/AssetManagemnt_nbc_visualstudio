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
    public class router_mtcController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/router_mtc
        public IQueryable<router_mtc> Getrouter_mtc(string token)
        {
            return db.router_mtc;
        }

        // GET: api/router_mtc/5
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Getrouter_mtc(int id, string token)
        {
            router_mtc router_mtc = db.router_mtc.Find(id);
            if (router_mtc == null)
            {
                return NotFound();
            }

            return Ok(router_mtc);
        }

        // PUT: api/router_mtc/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putrouter_mtc(int id, router_mtc router_mtc, string token)
        {
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
        //--------------------------------------------- api/sdwan_router/write_off/update/{id} (WRITEOFF SDWAN ROUTER START)----------------------------------------------
        // PUT: api/router_mtc/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/sdwan_router/write_off/update/{id}")]
        public IHttpActionResult Putsdwan_ROUTER_write_of(int id, router_mtc router_mtc, string token)
        {
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

        //--------------------------------------------- api/sdwan_router/write_off/update/{id} (WRITEOFF SDWAN ROUTER END)----------------------------------------------

        // POST: api/router_mtc
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Postrouter_mtc(router_mtc router_mtc, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.router_mtc.Add(router_mtc);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = router_mtc.id }, router_mtc);
        }

        // DELETE: api/router_mtc/5
        [ResponseType(typeof(router_mtc))]
        public IHttpActionResult Deleterouter_mtc(int id, string token)
        {
            router_mtc router_mtc = db.router_mtc.Find(id);
            if (router_mtc == null)
            {
                return NotFound();
            }

            db.router_mtc.Remove(router_mtc);
            db.SaveChanges();

            return Ok(router_mtc);
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