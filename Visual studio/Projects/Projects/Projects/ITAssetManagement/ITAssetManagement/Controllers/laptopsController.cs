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
using ITAssetManagement.Filters;

namespace ITAssetManagement.Controllers
{
    public class laptopsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/laptops
        // [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiKeyAuth]
        [ResponseType(typeof(laptop))]
        [HttpGet]
        [Route("api/laptops")]
        public IHttpActionResult Getlaptops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                // Return the IQueryable of laptops
                var laptops = db.laptops.ToList(); // Materialize to list to avoid lazy loading issues

                return Ok(new { Message = "Successfully fetched laptops.", laptops });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching laptops.", Details = ex.Message });
            }
        }

        // (Loanable laptopes type is 1 and non-loanable is 0)
        // API EndPoint to get all non loanable laptops. this are laptops that they can loan out to anyone*/
        [ResponseType(typeof(laptop))]
        [HttpGet]
        [Route("api/laptops/get_non_loanable_laptops")]
        public IHttpActionResult Getnonloanablelaptop(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
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
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        //------------------------------------------------------------------ --GET LOANABLE LAPTOPS-------------------------------- 
        [ResponseType(typeof(laptop))]
        [HttpGet]
        [Route("api/laptops/get_loanable_laptops")]
        public IHttpActionResult Getloanablelaptop(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
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
        } catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        // -------------------------------------------------------GET: api/laptops/5-------------------------------------------------------
        [ResponseType(typeof(laptop))]
        public IHttpActionResult GetLaptopWithStatus(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

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
        } catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message});
            }
        }


        //---------------------------- PUT: api/laptops/5(UPDATE) ---------------------------------
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlaptop(int id, laptop laptop, string token)
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
           existingLaptops.user_updated = laptop.user_updated;
                // existingLaptops.device_status_id = laptop.device_status_id;
                if (laptop.device_status_id != null)
                {
                    existingLaptops.device_status_id = laptop.device_status_id;
                }


                // Update type and status only if they are not null
                if (laptop.type != null)
            {
                existingLaptops.type = laptop.type;
            }

            if (laptop.device_status_id != null)
            {
                existingLaptops.device_status_id = laptop.device_status_id;
            }

                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    existingLaptops.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }
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
        } catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message});
            }
        }


        //------------------------------------------------------ POST: (POST laptop) Start----------------------------
        [ResponseType(typeof(laptop))]
        [HttpPost]
        [Route("api/laptops")]
        public IHttpActionResult Postlaptop(LaptopInvoiceModel model, string token)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Create an instance of laptop and set its properties
                    var laptop = model.Laptop;

                    laptop.date_created = DateTime.Now;
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        laptop.user_created = authenticatedUserId.Value; // Set to authenticated user
                        laptop.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }
                    db.laptops.Add(laptop);
                    db.SaveChanges();

                    // Created instance to insert into the invoice table
                    var laptopInvoice = new laptop_invoice
                    {
                        laptop_id = laptop.id,
                        invoice_document = model.InvoiceDescription,
                        user_created = laptop.user_created,
                        date_created = DateTime.Now
                    };

                    // Save data in the invoice table to store the invoice documents
                    db.laptop_invoice.Add(laptopInvoice);
                    db.SaveChanges();


                    transaction.Commit();

                    // Return OK response with the added laptop data
                    // return Ok(laptop);
                    return Content(HttpStatusCode.Created, new { message = "Laptop created successfully.", laptop = laptop });
                }
                catch (Exception ex)
                {
                    // Rollback incase of any issues in the transaction
                    transaction.Rollback();

                    // return InternalServerError(ex);
                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
                }


            }
        }

        //------------------------------------------------------ POST: (POST laptop) End----------------------------




        //-----------------------------------------------WRITE-OFF A LAPTOP START----------------------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/laptops/write_off_laptops/{id}")]
        public IHttpActionResult PutWriteOffLaptop(int id, laptop laptop, string token)
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
            // Update device_status_id for the laptop
            laptop.device_status_id = 5;
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                 
                    laptop.user_updated = authenticatedUserId.Value; 
                }



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
            catch (Exception ex)
            {
                 
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while Updating the assigned desktop.", error = ex.Message });
            }
        }
        //-----------------------------------------------WRITE-OFF A LAPTOP END------------------------------------------------------------------



        //---------------------------------------COUNT THE TOTAL NON-LOANABLE LAPTOPS --------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpGet]
        [Route("api/laptops/count_non_loanable_laptops")]
        public IHttpActionResult CountNonLoanableLaptops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count non-loanable laptops from the database
                var nonLoanableCount = db.laptops.Count(l => l.type == "0");

                return Ok(new { total_none_loanable_laptops = nonLoanableCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //---------------------------------------COUNT THE TOTAL LOANABLE LAPTOPS --------------------------------------------------
        [ResponseType(typeof(void))]
        [HttpGet]
        [Route("api/laptops/count_loanable_laptops")]
        public IHttpActionResult CountLoanableLaptops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count loanable laptops from the database
                var loanableCount = db.laptops.Count(l => l.type == "1");

                return Ok(new { total_loaneble_laptops = loanableCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //---------------


        // DELETE: api/laptops/5
        [ResponseType(typeof(laptop))]
        public IHttpActionResult Deletelaptop(int id, string token)
        {
            try
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
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while Deleting the assigned desktop.", error = ex.Message });
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

        private bool laptopExists(int id)
        {
            return db.laptops.Count(e => e.id == id) > 0;
        }
    }
}