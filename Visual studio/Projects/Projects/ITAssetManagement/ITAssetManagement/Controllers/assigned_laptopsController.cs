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
    public class assigned_laptopsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //--------------------------------------------------GET: api/assigned_laptops(GET ALL ASSIGNED LAPTOPS)------------------
        [ResponseType(typeof(assigned_laptops))]
        [HttpGet]
        [Route("api/assigned_laptops")]
        public IHttpActionResult Getassigned_laptops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Retrieve assigned laptops from the database
                var assigned_laptops = db.assigned_laptops;

                // Return the assigned laptops
                return Ok(assigned_laptops);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving assigned laptops.", Details = ex.Message });
            }
        }


        //----------------------------------  GET: api/assigned_laptops/5(GET ASSIGEND LAPTOP BY ID) ---------------------------------- 
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Getassigned_laptops(int id, string token)
        {
            try
            {
    
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                assigned_laptops assigned_laptop = db.assigned_laptops.Find(id);
 
                if (assigned_laptop == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Assigned Laptop with ID {id} not found." });
                }

     
                return Ok(assigned_laptop);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned laptop.", Details = ex.Message });
            }

            
        }


        //-------------------------------------- GET ASSIGNED LAPTOP AND THEIR ASSOCIATED USERS START ------------------------------------
        [HttpGet]
        [Route("api/assigned_laptops_with_users_and_laptops")]
        public IHttpActionResult GetAssignedLaptopsWithUsersAndLaptops(int assigned_laptop_id, string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var result = from laptop in db.laptops
                                 // Left join to assigned_laptops to get user and assigned laptop data if exists
                             join assigned in db.assigned_laptops

                             on laptop.id equals assigned.laptop_id into assignedLaptopJoin
                             from assignedLaptop in assignedLaptopJoin.DefaultIfEmpty()

                                 // Left join to users to get user data if assigned
                             join user in db.users

                             on assignedLaptop.user_assigned_id equals user.id into userJoin
                             from userData in userJoin.DefaultIfEmpty()

                                 // Filter by laptop_id
                             where laptop.id == assigned_laptop_id
                             select new
                             {
                                 Assigned_Laptop = laptop,
                                 AssignedUser = (assignedLaptop != null) ? new { userData.fullname, userData.id, userData.username, userData.email } : null, // Anonymous type for assigned user
                                 IsAssigned = (assignedLaptop != null)
                             };

                // Retrieve the single result or null if not found
                var laptopDetails = result.FirstOrDefault();

                if (laptopDetails == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Assigned Laptop not found." });
                }

                return Ok(laptopDetails);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned laptop.", Details = ex.Message });
            }
        }
    

        //------------------------------------------------  PUT: api/assigned_laptops/5(UPDATE) ------------------------------------
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_laptops(int id, assigned_laptops assigned_laptops, string token)
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

                if (id != assigned_laptops.id)
                {
                    return BadRequest("IDs in the request URL and the assigned_laptops object do not match."); // Returns a custom message
                }

                db.Entry(assigned_laptops).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!assigned_laptopsExists(id))
                    {
                        return Content(HttpStatusCode.NotFound, new { Message = $"Assigned Laptop not found." });
                    }
                    else
                    {
                        throw;
                    }
                }

                //return StatusCode(HttpStatusCode.NoContent);
                return Ok(assigned_laptops);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating the assigned laptop.", Details = ex.Message });
            }
        }

       
        //-------------------------------------------------------ASSIGN A LAPTOP TO A USER START-------------------------------------------------------------------
        // POST: api/assigned_laptops
        [ResponseType(typeof(assigned_laptops))]
        [HttpPost]
        [Route("api/assigned_laptops")]
        public IHttpActionResult Postassigned_laptops(LaptopSignOut model, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Validate the token
                    if (validate_token(token))
                    {
                        return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                    }
                    var assigned_laptops = model.Assign_Laptop;
                    assigned_laptops.date_created = DateTime.Now;
                    assigned_laptops.user_created = 1;

                    var existingRecord = db.assigned_laptops.FirstOrDefault(x => x.laptop_id == assigned_laptops.laptop_id || x.user_assigned_id == assigned_laptops.user_assigned_id);

                    if (existingRecord != null)
                    {
                        return Content(HttpStatusCode.Conflict, new { message = "Record with same Laptop ID Or User ID already exists." });

                    }

                    db.assigned_laptops.Add(assigned_laptops);
                    db.SaveChanges();

                    // Update the laptop status to 2. which means Assigned
                    var laptop = db.laptops.Find(assigned_laptops.laptop_id);

                    if (laptop != null)
                    {

                        laptop.device_status_id = 2;
                        db.SaveChanges();
                    }

                    // Created instance to insert into the invoice table
                    var LapSignout_docs = new sign_out_laptop
                    {
                        laptop_id = assigned_laptops.laptop_id,
                        user_id = assigned_laptops.user_assigned_id,
                        signout_document = model.signout_document,
                        user_created = assigned_laptops.user_created,
                        date_created = DateTime.Now
                    };

                    // Save data in the invoice table to store the invoice documents
                    db.sign_out_laptop.Add(LapSignout_docs);
                    db.SaveChanges();

                    transaction.Commit();

                    // return CreatedAtRoute("DefaultApi", new { id = assigned_laptops.id }, assigned_laptops);
                    return Content(HttpStatusCode.Created, new { message = "Laptop assigned successfully.", assigned_laptops = assigned_laptops });
                }
                catch (Exception ex)
                {
                    // Rollback transaction if something goes wrong
                    transaction.Rollback();
                    // return InternalServerError(ex);
                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
                }
            }
        }
        //-------------------------------------------------------ASSIGN A LAPTOP TO A USER END----------------------------------------------------------------------------------------------

        //-------------------UN -ASSIGN A LAPTOP TO A USER START--------------------------------------------------------------------------
        // DELETE: api/ussigned_laptops/unassign_use/
        [ResponseType(typeof(assigned_laptops))]
        [HttpDelete]
        [Route("api/ussigned_laptops/unassign_user")]
        public IHttpActionResult Unassigned_laptops(int un_assign_laptop_id, string token)
        {

            // Check if any assigned laptops exist
            var assignedLaptops = db.assigned_laptops.Where(a => a.laptop_id == un_assign_laptop_id).ToList();
            if (!assignedLaptops.Any())
            {
                return NotFound();
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Validate the token
                    if (validate_token(token))
                    {
                        return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                    }
                    // Update device_status_id in laptops table
                    var laptopsToUpdate = db.laptops.Where(l => l.id == un_assign_laptop_id).ToList();

                    foreach (var laptop in laptopsToUpdate)
                    {
                        laptop.device_status_id = 1;
                        db.Entry(laptop).State = EntityState.Modified;
                    }
                    // Save The change in the Laptop table
                    db.SaveChanges();

                    // This code will remove the data from the database (Delete the record)
                    db.assigned_laptops.RemoveRange(assignedLaptops);

                    // Save all the changes after deleting
                    db.SaveChanges();

                    // Commit the transaction to ensure that all the Operarions  worked
                    transaction.Commit();

                    return Ok(assignedLaptops);
                }
                catch (Exception ex)
                {
                    // Incase the Transaction has failed Rollback the changes
                    transaction.Rollback();
                    return InternalServerError(ex);
                }
            }
        }

        //-------------------UN -ASSIGN A LAPTOP TO A USER END----------------------------------------------------------------------------

        // DELETE: api/assigned_laptops/5
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Deleteassigned_laptops(int id, string token)
        {
            try
            {

                // Validate the token
                if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            assigned_laptops assigned_laptops = db.assigned_laptops.Find(id);
            if (assigned_laptops == null)
            {
                return NotFound();
            }

            db.assigned_laptops.Remove(assigned_laptops);
            db.SaveChanges();

            return Ok(assigned_laptops);
        }
    catch (Exception ex)
    {
        return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while deleting the assigned_laptops record.", Details = ex.Message });
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

        private bool assigned_laptopsExists(int id)
        {
            return db.assigned_laptops.Count(e => e.id == id) > 0;
        }
    }
}