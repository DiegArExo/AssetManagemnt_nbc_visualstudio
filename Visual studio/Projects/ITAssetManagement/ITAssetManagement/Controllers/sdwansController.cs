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
    public class sdwansController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/sdwans
        public IQueryable<sdwan> Getsdwans(string token)
        {
            return db.sdwans;
        }

        // GET: api/sdwans/5
        [ResponseType(typeof(sdwan))]
        public IHttpActionResult Getsdwan(int id, string token)
        {
            sdwan sdwan = db.sdwans.Find(id);
            if (sdwan == null)
            {
                return NotFound();
            }

            return Ok(sdwan);
        }

        // PUT: api/sdwans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putsdwan(int id, sdwan sdwan, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sdwan.id)
            {
                return BadRequest();
            }

            db.Entry(sdwan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sdwanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //---------------------------------------------- POST: api/sdwans Start-------------------------------------------------
        [ResponseType(typeof(sdwan))]
        [HttpPost]
        [Route("api/sdwans")]
        public IHttpActionResult Postsdwan(sdwan sdwan, string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the firewall_id, sdwanlaptop_id, or router_id is already assigned
                var existingFirewall = db.sdwans.Any(f => f.firewall_id == sdwan.firewall_id);
                var existingLaptop = db.sdwans.Any(l => l.sdwanlaptop_id == sdwan.sdwanlaptop_id);
                var existingRouter = db.sdwans.Any(r => r.router_id == sdwan.router_id);

                if (existingFirewall || existingLaptop || existingRouter)
                {
                    var errorMessage = "Conflict .";
                    if (existingFirewall )
                    {
                        errorMessage += " Firewall already assigned.";
                    }
                    if (existingLaptop)
                    {
                        errorMessage += " SDWAN laptop already assigned.";
                    }
                    if (existingRouter)
                    {
                        errorMessage += " Router already assigned.";
                    }
                    return Content(HttpStatusCode.Conflict, new { message = errorMessage });
                }



                // If none of the devices are already assigned, proceed to add the SDWAN
                db.sdwans.Add(sdwan);
                db.SaveChanges();

                // Return success message
                return Content(HttpStatusCode.Created, new { message = "SDWAN created successfully.", sdwan = sdwan });
            }
            catch (Exception ex)
            {
                // Return InternalServerError response with the error message
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the SDWAN.", error = ex.Message });
            }
        }


        //---------------------------------------------- POST: api/sdwans END-------------------------------------------------

        // DELETE: api/sdwans/5
        [ResponseType(typeof(sdwan))]
        public IHttpActionResult Deletesdwan(int id, string token)
        {
            sdwan sdwan = db.sdwans.Find(id);
            if (sdwan == null)
            {
                return NotFound();
            }

            db.sdwans.Remove(sdwan);
            db.SaveChanges();

            return Ok(sdwan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool sdwanExists(int id)
        {
            return db.sdwans.Count(e => e.id == id) > 0;
        }
    }
}