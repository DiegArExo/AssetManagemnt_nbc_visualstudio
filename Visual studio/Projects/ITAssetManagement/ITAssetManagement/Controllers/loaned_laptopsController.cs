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
    public class loaned_laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/loaned_laptops
        public IQueryable<loaned_laptops> Getloaned_laptops()
        {
            return db.loaned_laptops;
        }

        // GET: api/loaned_laptops/5
        [ResponseType(typeof(loaned_laptops))]
        public IHttpActionResult Getloaned_laptops(int id)
        {
            loaned_laptops loaned_laptops = db.loaned_laptops.Find(id);
            if (loaned_laptops == null)
            {
                return NotFound();
            }

            return Ok(loaned_laptops);
        }
        [HttpGet]
        [Route("api/loaned_laptops_with_users_and_laptops")]
        public IHttpActionResult GetLoanedLaptopsWithUsersAndLaptops(int loaned_laptop_id)
        {
            try
            {
                var result = from laptop in db.laptops
                                 // Left join to assigned_laptops to get user and assigned laptop data if exists
                             join loaned in db.loaned_laptops

                             on laptop.id equals loaned.loaned_laptop_id into loanedLaptopJoin
                             from loanedLaptop in loanedLaptopJoin.DefaultIfEmpty()

                                 // Left join to users to get user data if assigned
                             join user in db.users

                             on loanedLaptop.user_loaned_id equals user.id into userJoin
                             from userData in userJoin.DefaultIfEmpty()

                                 // Filter by laptop_id
                             where laptop.id == loaned_laptop_id
                             select new
                             {
                                 Loaned_Laptop = laptop,
                                 AssignedUser = (loanedLaptop != null) ? new { userData.fullname, userData.id, userData.username, userData.email } : null, // Anonymous type for assigned user
                                 IsAssigned = (loanedLaptop != null)
                             };

                // Retrieve the single result or null if not found
                var laptopDetails = result.FirstOrDefault();

                if (laptopDetails == null)
                {
                    return NotFound();
                }

                return Ok(laptopDetails);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // PUT: api/loaned_laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putloaned_laptops(int id, loaned_laptops loaned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != loaned_laptops.id)
            {
                return BadRequest();
            }

            db.Entry(loaned_laptops).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!loaned_laptopsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(loaned_laptops);
        }

        //-------------------------------------------------------------- POST: api/loaned_laptops ------------------------------------------------------------------------------
        [ResponseType(typeof(loaned_laptops))]
        public IHttpActionResult Postloaned_laptops(LoanOut_laptopModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    var loaned_laptops = model.LoanOut_Laptops;
                    loaned_laptops.date_created = DateTime.Now;
                    //change to authenticated user
                    loaned_laptops.user_created = 1;

                    var existingRecord = db.loaned_laptops.FirstOrDefault(x => x.loaned_laptop_id == loaned_laptops.loaned_laptop_id || x.user_loaned_id == loaned_laptops.user_loaned_id);

                    if (existingRecord != null)
                    {
                        return Content(HttpStatusCode.Conflict, new { message = "Record with same Laptop ID Or User ID already exists." });

                    }

                    // Check if loan_description is null or empty and set it to null if true
                    if (string.IsNullOrWhiteSpace(loaned_laptops.descriptions))
                    {
                        loaned_laptops.descriptions = null;
                    }


                    db.loaned_laptops.Add(loaned_laptops);
                    db.SaveChanges();


                    // Update the laptop status to 2. which means Assigned
                    var laptop = db.laptops.Find(loaned_laptops.loaned_laptop_id);
                    if (laptop != null)
                    {

                        laptop.device_status_id = 3;
                        db.SaveChanges();
                    }


                    // Created instance to insert into the invoice table
                    var Laptop_LoanOut_docs = new loan_out_laptop
                    {
                        laptop_id = loaned_laptops.loaned_laptop_id,
                        user_id = loaned_laptops.user_loaned_id,
                        loan_out_document = model.loan_out_document,
                        user_created = loaned_laptops.user_created,
                        date_created = DateTime.Now
                    };

                    // Save data in the invoice table to store the invoice documents
                    db.loan_out_laptop.Add(Laptop_LoanOut_docs);
                    db.SaveChanges();


                    // return CreatedAtRoute("DefaultApi", new { id = assigned_laptops.id }, assigned_laptops);
                   // return Content(HttpStatusCode.Created, new { message = "Laptop loaned successfully.", loaned_laptops = loaned_laptops });

                    // Commiting a transaction this means that all transactions are made succefully made.
                    transaction.Commit();


                    // return CreatedAtRoute("DefaultApi", new { id = loaned_laptops.id }, loaned_laptops);
                 
                    return Content(HttpStatusCode.Created, new { message = "Laptop Loaned successfully.", loaned_laptops = loaned_laptops });
                }
                catch (Exception ex)
                {
                    // Rollback The transaction if something goes wrong
                    transaction.Rollback();
                    // return InternalServerError(ex);
                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the loan laptop.", error = ex.Message });
                }
            }
        }

        //-------------------------------------------------------------- POST: api/loaned_laptops End ------------------------------------------------------------------------------

        // DELETE: api/loaned_laptops/5
        [ResponseType(typeof(loaned_laptops))]
        [HttpDelete]
        [Route("api/loaned_laptops/unloan_user")]
        public IHttpActionResult Deleteloaned_laptops(int laptop_loaned_id)
        {
            //Getting the value by Id
            var loaned_laptops = db.loaned_laptops.Where(a => a.loaned_laptop_id == laptop_loaned_id).ToList();
            if (loaned_laptops == null)
            {
                return NotFound();
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                //Try and catch will help with error handing and avoidin the code from crahing. 
                try
                {
                    // Update device_status_id in laptops table
                    var laptopsToUpdate = db.laptops.Where(l => l.id == laptop_loaned_id).ToList();

                    foreach (var laptop in laptopsToUpdate)
                    {
                        laptop.device_status_id = 1;
                        db.Entry(laptop).State = EntityState.Modified;
                    }
                    // Save The change in the Laptop table
                    db.SaveChanges();

                    //Delete the record from the database
                    db.loaned_laptops.RemoveRange(loaned_laptops);
                    // Save all the changes after deleting
                    db.SaveChanges();

                    // Commit the transaction to ensure that all the Operarions  worked
                    transaction.Commit();

                    return Ok(loaned_laptops);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
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

        private bool loaned_laptopsExists(int id)
        {
            return db.loaned_laptops.Count(e => e.id == id) > 0;
        }
    }
}