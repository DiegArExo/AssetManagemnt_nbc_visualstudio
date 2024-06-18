﻿using System;
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
    public class assigned_laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/assigned_laptops
        public IQueryable<assigned_laptops> Getassigned_laptops()
        {
            return db.assigned_laptops;
        }

        // GET: api/assigned_laptops/5
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Getassigned_laptops(int id)
        {
            assigned_laptops assigned_laptops = db.assigned_laptops.Find(id);
            if (assigned_laptops == null)
            {
                return NotFound();
            }

            return Ok(assigned_laptops);
        }
        //GET FUNTION -- This one gets all the laptops and the users assocciated witht the laptops.
        [HttpGet]
        [Route("api/assigned_laptops_with_users_and_laptops")]
        public IHttpActionResult GetAssignedLaptopsWithUsersAndLaptops(int assigned_laptop_id)
        {
            try
            {
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
                    return NotFound();
                }

                return Ok(laptopDetails);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/assigned_laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_laptops(int id, assigned_laptops assigned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assigned_laptops.id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(assigned_laptops);
        }

        // POST: api/assigned_laptops
        [ResponseType(typeof(assigned_laptops))]
        public IHttpActionResult Postassigned_laptops(assigned_laptops assigned_laptops)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Transaction Ensures that both Operation work togther or fail togther.
            //It prevents a situation where by assign is inserted but update is not done
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                   
                    assigned_laptops.date_created = DateTime.Now; //This is how you Insert the date from the API side
                    assigned_laptops.user_created = 1; // Replace with the authenticated user ID

                   
                    db.assigned_laptops.Add(assigned_laptops); //Save the recore
                    db.SaveChanges();

                    // Update the laptop status to 2. which means Assigned
                    var laptop = db.laptops.Find(assigned_laptops.laptop_id);

                    if (laptop != null)
                    {
                        
                        laptop.device_status_id = 2;
                        db.SaveChanges();
                    }

                    // Commiting a transaction this means that all transactions are made succefully made.
                    transaction.Commit();

                    return CreatedAtRoute("DefaultApi", new { id = assigned_laptops.id }, assigned_laptops);
                }
                catch (Exception ex)
                {
                    // Rollback transaction if something goes wrong
                    transaction.Rollback();
                    return InternalServerError(ex);
                }
            }
        }


        // DELETE: api/ussigned_laptops/unassign_use/
        [ResponseType(typeof(assigned_laptops))]
        [HttpDelete]
        [Route("api/ussigned_laptops/unassign_user")]
        public IHttpActionResult Unassigned_laptops(int un_assign_laptop_id)
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