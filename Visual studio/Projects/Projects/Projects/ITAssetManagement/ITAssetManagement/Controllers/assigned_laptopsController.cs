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
                                 AssignedTabale = assignedLaptop,
                                 AssignedUser = (assignedLaptop != null) ? new { userData.fullname, userData.id, userData.username, userData.email } : null,  
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

                int? authenticatedUserId = GetUserIdFromToken(token);
               /* if (authenticatedUserId.HasValue)
                {
                    assigned_laptops.user_created = authenticatedUserId.Value; // Set to authenticated user
                    assigned_laptops.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }*/
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
                    // Extract user info from the token
                   // var  authenticatedUserId = GetUserIdFromToken(token);
                    var assigned_laptops = model.Assign_Laptop;
                    assigned_laptops.date_created = DateTime.Now;
                    //assigned_laptops.user_created = 1;
                    // assigned_laptops.user_created = authenticatedUserId; // Set to authenticated user

        
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        assigned_laptops.user_created = authenticatedUserId.Value; // Set to authenticated user
                        assigned_laptops.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }


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
        [Route("api/assigned_laptops/unassign_user")]
        public IHttpActionResult Unassigned_laptops(int id, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            // Check if any assigned laptops exist
            var assignedLaptops = db.assigned_laptops.Where(a => a.laptop_id == id).ToList();
            if (!assignedLaptops.Any())
            {
                return Content(HttpStatusCode.NotFound, new { Message = "Laptop with the provided ID was not found." });
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Update device_status_id in laptops table
                    var laptopsToUpdate = db.laptops.Where(l => l.id == id).ToList();
                    foreach (var laptop in laptopsToUpdate)
                    {
                        laptop.device_status_id = 1; // Set status to available
                        db.Entry(laptop).State = EntityState.Modified;
                    }

                    // Save the changes in the Laptop table
                    db.SaveChanges();

                    // Get the authenticated user ID from the token
                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        foreach (var assignedLaptop in assignedLaptops)
                        {
                            assignedLaptop.user_created = authenticatedUserId.Value; // Set to authenticated user
                            assignedLaptop.user_updated = authenticatedUserId.Value; // Set to authenticated user
                            db.Entry(assignedLaptop).State = EntityState.Modified;
                        }

                        // Save changes to update user_created and user_updated fields
                        db.SaveChanges();
                    }

                    // Remove the assigned laptop records
                    db.assigned_laptops.RemoveRange(assignedLaptops);

                    // Save all the changes after deleting
                    db.SaveChanges();

                    // Commit the transaction to ensure that all the operations worked
                    transaction.Commit();

                    return Ok(new { Message = "Success: The laptop was successfully unassigned and its status updated.", AssignedLaptops = assignedLaptops });
                }
                catch (Exception ex)
                {
                    // In case the transaction has failed, rollback the changes
                    transaction.Rollback();
                    return Content(HttpStatusCode.InternalServerError, new { Message = "Internal Server Error: An error occurred while unassigning the laptop.", ex });
                }
            }
        }


        //-------------------UN -ASSIGN A LAPTOP TO A USER END----------------------------------------------------------------------------




        //----------------------------------------------------------GET ALL ASSIGNED LAPTOPS START----------------------------------------------------------------------------------------------
        [ResponseType(typeof(loaned_laptops))]
        [HttpGet]
        [Route("api/assigned_laptops/count")]
        public IHttpActionResult CountAssignedLaptops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count all records in the assigned_laptops table
                var assignedLaptopsCount = db.assigned_laptops.Count();

                return Ok(new { total_assigned_laptops = assignedLaptopsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        //----------------------------------------------------------GET ALL ASSIGNED LAPTOPS END------------------------------------------------------------------

        //----------------------------------------------------------GET ALL AVAILABLE LOANABLE LAPTOPS START------------------------------------------------------------------
        [ResponseType(typeof(assigned_laptops))]
        [HttpGet]
        [Route("api/laptops/count_non_loanable_laptops_not_in_assigned")]
        public IHttpActionResult CountNonLoanableLaptopsNotInAssigned(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count non-loanable laptops from the database that are not found in assigned_laptops
                var nonLoanableCount = (from laptop in db.laptops
                                        where laptop.type == "0" && !db.assigned_laptops.Any(a => a.laptop_id == laptop.id)
                                        select laptop).Count();

                return Ok(new { total_available_non_loan_laptops = nonLoanableCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //----------------------------------------------------------GET ALL AVAILABLE LOANABLE LAPTOPS END------------------------------------------------------------------



        //--------------------------------------------------GET: api/assigned_laptops_user(GET THE ASSIGNED LAPTOP bASED ON THE USER WHO ITS ASSIGNED TOO) STARTS ------------------
        [ResponseType(typeof(assigned_laptops))]
        [HttpGet]
        [Route("api/assigned_laptops/assigned_laptops_user/{id}")]
        public IHttpActionResult GetAssignedLaptopUser(int id, string token)
        {
            try
            {
                // Validate the token (if needed)
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                //-----------------------------------------GET user informatiom---------------------------------------------------
                var userInformation = db.users.FirstOrDefault(al => al.id == id);
                var userInformationDataMessage = userInformation != null ? null : $"No User with ID {id}."; 

                //-----------------------------------------Assigned Laptop Data---------------------------------------------------
                var assignedLaptop = db.assigned_laptops.FirstOrDefault(al => al.user_assigned_id == id);
                var assignedLaptopDataMessage = assignedLaptop != null ? null : $"No laptop assigned to user with ID {id}.";

                var laptop = assignedLaptop != null ? db.laptops.FirstOrDefault(l => l.id == assignedLaptop.laptop_id) : null;
                var laptopDataMessage = assignedLaptop != null && laptop == null ? $"Laptop with ID {assignedLaptop.laptop_id} not found." : null;

                var User_assigned_Laptop = assignedLaptop != null ? db.users.FirstOrDefault(al => al.id == assignedLaptop.user_created) : null;//Get the user_craeted name for Assigned Laptop 
               // var User_assigned_Laptop = db.users.FirstOrDefault(al => al.id == assignedLaptop.user_created); //Get the user_craeted name for Assigned Laptop 
                var User_assigned_LaptopMessage = User_assigned_Laptop != null ? null : $"No User with ID {id}."; 

                //-----------------------------------------Loaned Laptop Data-------------------------------------------------------
                var loanedLaptop = db.loaned_laptops.FirstOrDefault(ll => ll.user_loaned_id == id);
                var loanedLaptopDataMessage = loanedLaptop != null ? null : $"No laptop loaned to user with ID {id}.";

                var loanedLaptopData = loanedLaptop != null ? db.laptops.FirstOrDefault(l => l.id == loanedLaptop.loaned_laptop_id) : null;
                var loanedLaptopDataMessageDetails = loanedLaptop != null && loanedLaptopData == null ? $"Laptop with ID {loanedLaptop.loaned_laptop_id} not found." : null;

               // var User_loaned_Laptop = db.users.FirstOrDefault(al => al.id == loanedLaptop.user_created); //Get the user_craeted name for Loaned Laptop 
                var User_loaned_Laptop = loanedLaptop != null ? db.users.FirstOrDefault(al => al.id == loanedLaptop.user_created) : null;//Get the user_craeted name for Loaned Laptop 
                var User_loaned_LaptopMessage = User_loaned_Laptop != null ? null : $"No User with ID {id}.";

                //-----------------------------------------Assigned SDWAN Data-------------------------------------------------------
                var AssignedSdwan = db.assigned_sdwans.FirstOrDefault(ll => ll.user_assigned_id == id);
                var AssignedSdwanDataMessage = AssignedSdwan != null ? null : $"No SDWAN assigned to user with ID {id}.";
                var SdwanStation = AssignedSdwan != null ? db.sdwans.FirstOrDefault(al => al.id == AssignedSdwan.sdwan_id) : null; //Get the station data to be used to get the monitor, cpu and firewall
                var SdwanStationDataMessage = SdwanStation != null && SdwanStation == null ? $"No SDWAN station found for user with ID {id}." : null;


                var AssignedFireWall = SdwanStation != null ? db.firewalls.FirstOrDefault(ll => ll.id == SdwanStation.firewall_id) : null;//Get the firewall
                var AssignedFireWallDataMessage = SdwanStation != null && AssignedFireWall == null ? $"No Firewall to user with ID {id}." : null;

 
                var AssignedRouter = SdwanStation != null ? db.router_mtc.FirstOrDefault(ll => ll.id == SdwanStation.router_id) : null;//Get the Router
                var AssignedRouterDataMessage = SdwanStation != null && AssignedRouter == null ? $"No Router to user with ID {id}." : null;

    
                var AssignedsdwanLaptop = SdwanStation != null ? db.sdwan_laptops.FirstOrDefault(ll => ll.id == SdwanStation.sdwanlaptop_id) : null; //Get the Router
                var AssignedsdwanLaptopMessage = SdwanStation != null && AssignedsdwanLaptop == null ? $"No Sdwan Laptop to user with ID {id}." : null;

               // var User_assigned_sdwan = db.users.FirstOrDefault(al => al.id == AssignedSdwan.user_created); //Get the user_craeted name for Loaned Laptop 
                var User_assigned_sdwan = AssignedSdwan != null ? db.users.FirstOrDefault(al => al.id == AssignedSdwan.user_created) : null; //Get the user_craeted name for Loaned Laptop  
                var User_assigned_sdwanMessage = User_assigned_sdwan != null ? null : $"No User with ID {id}."; 

                //-----------------------------------------Assigned DESKTOP STATION Data-------------------------------------------------------

                var AssignedDesktop= db.assigned_desktops.FirstOrDefault(ll => ll.user_assigned_id == id);
                var AssignedDesktopDataMessage = AssignedDesktop != null ? null : $"No Desktop assigned to user with ID {id}.";

                var DesktopStation = AssignedDesktop != null ? db.joined_desktops_monitors.FirstOrDefault(al => al.id == AssignedDesktop.joined_desktop_monitor_cpu_id) : null; //Get the station data to be used to get the monitor, cpu and firewall
                var DesktopStationDataMessage = DesktopStation != null && DesktopStation == null ? $"No Desktop station found for user with ID {id}." : null;

                var AssignedMonitor = DesktopStation != null ? db.desktop_monitors.FirstOrDefault(ll => ll.id == DesktopStation.desktop_monitor_id) : null;//Get the Monitor
                var AssignedMonitorDataMessage = DesktopStation != null && AssignedMonitor == null ? $"No Monitor assigned to user with ID {id}." : null;

                var AssignedCpu = DesktopStation != null ? db.desktop_cpus.FirstOrDefault(ll => ll.id == DesktopStation.desktop_cpu_id) : null;//Get the CPU
                var AssignedCpuDataMessage = DesktopStation != null && AssignedCpu == null ? $"No CPU assigned to user with ID {id}." : null;

               // var User_assigned_desktop = db.users.FirstOrDefault(al => al.id == DesktopStation.user_created); //Get the user_craeted name for Loaned Laptop 
                var User_assigned_desktop = DesktopStation != null ? db.users.FirstOrDefault(al => al.id == DesktopStation.user_created) : null; //Get the user_craeted name for Loaned Laptop 
                var User_assigned_desktopMessage = User_assigned_sdwan != null ? null : $"No User with ID {id}."; 

                // Prepare the response object
                var response = new
                {
                    //-----------------------------------------GET user informatiom---------------------------------------------------

                    UserInformationDate = userInformation,
                    UserInformationDateMessages = userInformationDataMessage,
                    //-----------------------------Assigned Laptop Return--------------
                    User_assigned_LaptopDate = User_assigned_Laptop,
                    User_assigned_LaptopDateMessage =  User_assigned_LaptopMessage,
                    AssignedLaptopData = assignedLaptop,
                    AssignedLaptopDataMessage = assignedLaptopDataMessage, //Message(Not in assigned)
                    LaptopData = laptop,
                    LaptopDataMessage = laptopDataMessage, //Message(Not in laptop)

                    //------------------------------Loaned Laptop Data-----------------------
                    User_loaned_LaptopDate = User_loaned_Laptop,
                    User_loaned_LaptopMessage = User_loaned_LaptopMessage,
                    LoanedLaptopData = loanedLaptop,
                    LoanedLaptopDataMessage = loanedLaptopDataMessage,//Message(Not in Laoned)
                    LoanedLaptopDataDetails = loanedLaptopData, //Message(Not in laptop)
                    LoanedLaptopDataMessageDetails = loanedLaptopDataMessageDetails,

                    //------------------------------Assigned SDWAN Data-----------------------
                    AssignedSdwanData = AssignedSdwan,
                    AssignedSdwanDataMessage = AssignedSdwanDataMessage,

                    User_assigned_sdwanDate = User_assigned_sdwan,
                    User_assigned_sdwanDateMessage = User_assigned_sdwanMessage,

                    //------------------------------SDWAN Station-----------------------
                    SdwanStationData = SdwanStation,
                    SdwanStationDataMessage = SdwanStationDataMessage,

                    //------------------------------SDWAN Firewall-----------------------
                    AssignedFireWallData = AssignedFireWall,
                    AssignedFireWallDataMessage,
                    //------------------------------SDWAN Router-----------------------
                    AssignedRouterData= AssignedRouter,
                    AssignedRouterDataMessage = AssignedRouterDataMessage,
                    //------------------------------SDWAN Sdwan Laptop-----------------------
                    AssignedsdwanLaptopData= AssignedsdwanLaptop,
                    AssignedsdwanLaptopDataMessage = AssignedsdwanLaptop,

                    //------------------------------Assigned Desktop-----------------------
                    AssignedDesktopData = AssignedDesktop,
                    AssignedDesktopDataMessage = AssignedDesktopDataMessage,
                    //------------------------------ Desktop Station-----------------------
                    DesktopStationData = DesktopStation,
                    DesktopStationDataMessage = DesktopStationDataMessage,
                    User_assigned_desktopData = User_assigned_desktop,
                    User_assigned_desktopDataMessage = User_assigned_desktopMessage,
                    //------------------------------ Desktop Monitor-----------------------
                    AssignedMonitorData = AssignedMonitor,
                    AssignedMonitorDataMessage = AssignedMonitorDataMessage,
                    //------------------------------ Desktop CPU-----------------------
                    AssignedCpuData = AssignedCpu,
                    AssignedCpuDataMessage = AssignedCpuDataMessage,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }
        //--------------------------------------------------GET: api/assigned_laptops_user(GET THE ASSIGNED LAPTOP bASED ON THE USER WHO ITS ASSIGNED TOO) END ------------------
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