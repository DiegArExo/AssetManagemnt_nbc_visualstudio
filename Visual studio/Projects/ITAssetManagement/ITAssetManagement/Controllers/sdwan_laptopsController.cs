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
    public class sdwan_laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/sdwan_laptops
        public IQueryable<sdwan_laptops> Getsdwan_laptops(string token)
        {
            return db.sdwan_laptops;
        }

        // GET: api/sdwan_laptops/5
        [ResponseType(typeof(sdwan_laptops))]
        public IHttpActionResult Getsdwan_laptops(int id, string token)
        {
            sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
            if (sdwan_laptops == null)
            {
                return NotFound();
            }

            return Ok(sdwan_laptops);
        }

        // PUT: api/sdwan_laptops/5
        [HttpPut]
        [ResponseType(typeof(void))]
        
        public IHttpActionResult Putsdwan_laptops(int id, sdwan_laptops sdwan_laptops, string token)
        {
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

        //--------------------------------------------- api/sdwan_laptop/write_off/update/{id} (WRITEOFF SDWAN LAPTOP START)----------------------------------------------
        [ResponseType(typeof(void))]
        [Route("api/sdwan_laptop/write_off/update/{id}")]
        public IHttpActionResult Putsdwan_laptop_write_off(int id, sdwan_laptops sdwan_laptop, string token)
        {
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
                sdwan_laptop.user_updated = existingSDWANLAPTOP.user_updated;
                sdwan_laptop.date_updated = DateTime.Now;
                sdwan_laptop.status_id = 1; // Update status if needed

                // Update only the necessary fields
                existingSDWANLAPTOP.comments = sdwan_laptop.comments;
                existingSDWANLAPTOP.attachment = sdwan_laptop.attachment;

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

            return StatusCode(HttpStatusCode.NoContent);
        }



        //--------------------------------------------- api/sdwan_laptop/write_off/update/{id} (WRITEOFF SDWAN LAPTOP END)----------------------------------------------

        // POST: api/sdwan_laptops
        [ResponseType(typeof(sdwan_laptops))]
        public IHttpActionResult Postsdwan_laptops(sdwan_laptops sdwan_laptops, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.sdwan_laptops.Add(sdwan_laptops);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sdwan_laptops.id }, sdwan_laptops);
        }

        // DELETE: api/sdwan_laptops/5
        [ResponseType(typeof(sdwan_laptops))]
        public IHttpActionResult Deletesdwan_laptops(int id, string token)
        {
            sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
            if (sdwan_laptops == null)
            {
                return NotFound();
            }

            db.sdwan_laptops.Remove(sdwan_laptops);
            db.SaveChanges();

            return Ok(sdwan_laptops);
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