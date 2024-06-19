/*NOTES 
 * 
 * g => g.Type == 1: This is a lambda expression where g is a placeholder representing each laptop in the laptops collection.
 *  db.laptops represents getting all the record from the table laptops
 
  */

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
using Newtonsoft.Json;


namespace ITAssetManagement.Controllers
{
    public class laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB(); 

        // GET: api/laptops
        public IQueryable<laptop> Getlaptops()
        {
            return db.laptops;
        }
       // (Loanable laptopes type is 1 and non-loanable is 0)
       // API EndPoint to get all non loanable laptops. this are laptops that they can loan out to anyone*/
       [ResponseType(typeof(laptop))]
        [HttpGet]
       [Route("api/laptops/get_non_loanable_laptops")]
        public IHttpActionResult Getnonloanablelaptop()
        {
            // Fetch non-loanable laptops from the database
            var non_loanable_laptop = db.laptops.Where(getType => getType.type == "0").ToList();

            // Check if the fetched list is null or empty
            if (non_loanable_laptop == null || !non_loanable_laptop.Any())
            {
                return NotFound(); // Return 404 if no non-loanable laptops are found
            }

            // Fetch non-loanable laptops along with their status from the database
            var non_loanable_laptop_with_status = from l_laptop in db.laptops
                                                  join d_device_status in db.device_status
                                                  on l_laptop.device_status_id equals d_device_status.id
                                                  where l_laptop.type == "0"
                                                  select new
                                                  {
                                                      Laptop = l_laptop, // Get all laptop details
                                                      noLoanable_StatusName = d_device_status.name // Get the status name
                                                  };

            // Return the result with the status included
            return Ok(non_loanable_laptop_with_status);
        }

        [ResponseType(typeof(laptop))]
        [HttpGet]
         [Route("api/laptops/get_loanable_laptops")]
        public IHttpActionResult Getloanablelaptop()
        {
            // Fetch non-loanable laptops from the database where type is "0"
            var non_loanable_laptop = db.laptops.Where(l => l.type == "1").ToList();

            // Check if the fetched list is null or empty
            if (non_loanable_laptop == null || !non_loanable_laptop.Any())
            {
                return NotFound(); // Return 404 if no non-loanable laptops are found
            }

            // Fetch non-loanable laptops along with their status from the database
            var loanable_laptop_with_status = from loan_laptop in db.laptops
                                              join d_device_status in db.device_status
                                              on loan_laptop.device_status_id equals d_device_status.id
                                              where loan_laptop.type == "1"
                                              select new
                                              {
                                                  LoanableLaptop = loan_laptop, // Get all laptop details
                                                  Loanable_statusName = d_device_status.name // Get the status name
                                              };

            // Return the result with the status included
            return Ok(loanable_laptop_with_status);
        }



        // GET: api/laptops/5
        [ResponseType(typeof(laptop))]
        public IHttpActionResult GetLaptopWithStatus(int id)
        {
            var laptopWithStatus = (from laptop in db.laptops
                                    join status in db.device_status on laptop.device_status_id equals status.id
                                    where laptop.id == id
                                    select new
                                    {
                                        Laptops = laptop,
                           
                                        status.name 
                                    }).FirstOrDefault();

            if (laptopWithStatus == null)
            {
                return NotFound();
            }

            return Ok(laptopWithStatus);
        }

        // PUT: api/laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlaptop(int id, laptop laptop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the existing entity from the database
            var existingLaptops = db.laptops.Find(id);
            if (existingLaptops == null)
            {
                //return NotFound();
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." }); // Return 404 Not Found with a custom message
            }
            //Defining Tableas that needs to be updated 
            existingLaptops.brand_name = laptop.brand_name;
            existingLaptops.model = laptop.model;
            existingLaptops.serial_number = laptop.serial_number;
            existingLaptops.tag_number = laptop.tag_number;

            // Update type and status only if they are not null
            if (laptop.type != null)
            {
                existingLaptops.type = laptop.type;
            }

            if (laptop.device_status_id != null)
            {
                existingLaptops.device_status_id = laptop.device_status_id;
            }

           // existingLaptops.type = laptop.type;
           // existingLaptops.device_status_id = laptop.device_status_id;

            //Insert in the date updated
            laptop.date_updated = DateTime.Now;

            db.Entry(existingLaptops).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!laptopExists(id))
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
            return Ok(laptop);
        }

        //--------------------------------------------- PUT: api/users/5----------------------------------------------

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/laptops/write_off_laptops/{id}")]
        public IHttpActionResult PutWriteOffLaptop(int id, laptop laptop)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != laptop.id)
            {
                return BadRequest();
            }

            // Retrieve the existing entity from the database
            var existingLaptop = db.laptops.Find(id);
            if (existingLaptop == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"Record with ID {id} not found." });
            }

            // Preserve fields that should not be changed
            laptop.date_created = existingLaptop.date_created;
            laptop.type = existingLaptop.type;
            laptop.device_status_id = existingLaptop.device_status_id;
            laptop.user_assigned_id = existingLaptop.user_assigned_id;
            laptop.user_created = existingLaptop.user_created;

            // Always update the date_updated field
            laptop.date_updated = DateTime.Now;

            db.Entry(existingLaptop).CurrentValues.SetValues(laptop);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!laptopExists(id))
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
                var innerException = ex.InnerException?.InnerException;
                return InternalServerError(innerException ?? ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(existingLaptop); // Return the updated laptop data
        }

        // POST: api/laptops
        [ResponseType(typeof(laptop))]
        public IHttpActionResult Postlaptop(laptop laptop)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Set the current timestamp for date_created
                laptop.date_created = DateTime.Now;

                // Add the new laptop to the database
                db.laptops.Add(laptop);

                // Save changes to the database
                db.SaveChanges();

                // Return OK response with the added laptop data
                return Ok(laptop);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }


        }

       



        // DELETE: api/laptops/5
        [ResponseType(typeof(laptop))]
        public IHttpActionResult Deletelaptop(int id)
        {
            laptop laptop = db.laptops.Find(id);
            if (laptop == null)
            {
                return NotFound();
            }

            db.laptops.Remove(laptop);
            db.SaveChanges();

            return Ok(laptop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool laptopExists(int id)
        {
            return db.laptops.Count(e => e.id == id) > 0;
        }
    }
}