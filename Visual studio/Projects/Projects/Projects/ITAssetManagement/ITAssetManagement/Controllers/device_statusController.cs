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
    public class device_statusController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //-------------------------------------GET: api/device_status(GET ALL)---------------------------------------------------
        [ResponseType(typeof(device_status))]
        [HttpGet]
        [Route("api/device_status")]
        public IHttpActionResult Getdevice_status(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

              
                return Ok(db.device_status);
            }
            catch (Exception ex)
            {
             
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching device status.", Details = ex.Message });
            }
        }


        //--------------------------------------------------------------------------- GET: api/device_status/5(GET A SPECIFIC)------------------------------------------------
        [ResponseType(typeof(device_status))]
        public IHttpActionResult Getdevice_status(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                device_status device_status = db.device_status.Find(id);
                if (device_status == null)
                {
                    return NotFound();  
                }

                return Ok(device_status);  
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching device status.", Details = ex.Message });
            }
        }

        //----------------------------------- PUT: api/device_status/(UPDATE )-----------------------------------
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdevice_status(int id, device_status device_status, string token)
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

                if (id != device_status.id)
                {
                    return BadRequest();
                }

                db.Entry(device_status).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!device_statusExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(device_status);
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        //-------------------------------------------------------------- POST: api/device_status(POST INFORAMTION)------------------------------------------------
        [ResponseType(typeof(device_status))]
        [HttpPost]
        [Route("api/device_status")]
        public IHttpActionResult Postdevice_status(device_status device_status, string token)
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

                device_status.date_created = DateTime.Now;

                db.device_status.Add(device_status);
                db.SaveChanges();
 
                return CreatedAtRoute("DefaultApi", new { id = device_status.id }, device_status);
            }
            catch (Exception ex)
            {
 
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while creating device status.", Details = ex.Message });
            }
        }

        // DELETE: api/device_status/5
        [ResponseType(typeof(device_status))]
        public IHttpActionResult Deletedevice_status(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                device_status device_status = db.device_status.Find(id);
                if (device_status == null)
                {
                    return NotFound();
                }

                db.device_status.Remove(device_status);
                db.SaveChanges();

                return Ok(device_status);

            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while Deleting device status.", Details = ex.Message });
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

        private bool device_statusExists(int id)
        {
            return db.device_status.Count(e => e.id == id) > 0;
        }
    }
}