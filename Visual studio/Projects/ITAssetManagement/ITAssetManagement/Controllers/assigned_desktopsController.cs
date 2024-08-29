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
    public class assigned_desktopsController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        //----------------------------------------------------- GET: api/assigned_desktops (GET ALL )--------------------------------
        [ResponseType(typeof(assigned_desktops))]
        [HttpGet]
        [Route("api/assigned_desktops")]
        public IHttpActionResult Getassigned_desktops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Retrieve the assigned desktops
                var assignedDesktops = db.assigned_desktops;

                // Check if any assigned desktops are found
                if (!assignedDesktops.Any())
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "No assigned desktops found." });
                }

                return Ok(new { Message = "Assigned desktops retrieved successfully.", Data = assignedDesktops });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned_desktops.", error = ex.Message });
            }
        }

        //-------------------------------------------------------------- GET: api/assigned_desktops/5 (GET A SPECIFIC )--------------------------------
        [ResponseType(typeof(assigned_desktops))]
        [HttpGet]
        [Route("api/assigned_desktops/{id}")]
        public IHttpActionResult Getassigned_desktops(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }


                var assignedDesktop = db.assigned_desktops.FirstOrDefault(ad => ad.joined_desktop_monitor_cpu_id == id);
                if (assignedDesktop == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"Assigned desktop with ID {id} not found." });
                }

                return Ok(assignedDesktop);
            }
            catch (Exception ex)
            {          
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }
        }

        //----------------------------------------------- GET: api/assigned_desktops/assigned_user_station/5 -----------------------------------------------

        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/assigned_desktops/assigned_user_station_nomonitor")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserNoMonito(int assigned_user_id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                var assigned_user_station = from assigned in db.assigned_desktops

                                        join user in db.users
                                        on assigned.user_assigned_id equals user.id

                                        join joined_desktop_cpu in db.joined_desktops_monitors
                                        on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                        where assigned.user_assigned_id == assigned_user_id && (joined_desktop_cpu.desktop_cpu_id == null || joined_desktop_cpu.desktop_cpu_id == 0
                                       || joined_desktop_cpu.desktop_monitor_id == null || joined_desktop_cpu.desktop_monitor_id == 0)

                                        select new
                                        {
                                            AssignedDesktop = assigned,
                                            User = user,
                                            DesktopStation_ = joined_desktop_cpu

                                        };
            return Ok(assigned_user_station);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }
        //------------------------------------ GET: api/assigned_desktops/5( This API EndPoint will Get all the users that are have been assigned a MONITOR) START----
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/assigned_desktops/get_assigned_user_station_with_monitor")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserWithMonitor(int assigned_user_id, int unassign_monitor_id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                var assigned_user_station = from assigned in db.assigned_desktops

                                            join user in db.users
                                            on assigned.user_assigned_id equals user.id

                                            join joined_desktop_cpu in db.joined_desktops_monitors
                                            on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                            where assigned.user_assigned_id == assigned_user_id && 
                                            (joined_desktop_cpu.desktop_cpu_id != null || joined_desktop_cpu.desktop_cpu_id != 0 || joined_desktop_cpu.desktop_monitor_id != null || joined_desktop_cpu.desktop_monitor_id != 0) &&
                                            (joined_desktop_cpu.desktop_monitor_id == unassign_monitor_id)

                                            select new
                                            {
                                                AssignedDesktop = assigned,
                                                User = user,
                                                DesktopStation_ = joined_desktop_cpu

                                            };
                return Ok(assigned_user_station);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }

      
        //------------------------------------ GET: api/assigned_desktops/assigned_user_station/5( This API EndPoint will Get all the users that are have not been assigned a CPU) START----
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_station_nocpu")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserNoCPU(int assigned_user_id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }


                var assigned_user_station = from assigned in db.assigned_desktops

                                        join user in db.users
                                        on assigned.user_assigned_id equals user.id

                                        join joined_desktop_cpu in db.joined_desktops_monitors
                                        on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                        where assigned.user_assigned_id == assigned_user_id && (joined_desktop_cpu.desktop_cpu_id == null || joined_desktop_cpu.desktop_cpu_id == 0
                                       || joined_desktop_cpu.desktop_monitor_id == null || joined_desktop_cpu.desktop_monitor_id == 0)

                                        select new
                                        {
                                            AssignedDesktop = assigned,
                                            User = user,
                                            DesktopStation_ = joined_desktop_cpu

                                        };
            return Ok(assigned_user_station);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }
        //------------------------------------ GET: api/assigned_desktops/assigned_user_station/5( This API EndPoint will Get all the users that are have not been assigned a CPU) END----



        //------------------------------------ GET: api/assigned_desktops/5( This API EndPoint will Get all the users that are have been assigned a CPU) START----
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/get_assigned_user_station_with_cpu")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserWithCPU(int assigned_user_id, int unassign_cpu_id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }


                var assigned_user_station = from assigned in db.assigned_desktops

                                        join user in db.users
                                        on assigned.user_assigned_id equals user.id

                                        join joined_desktop_cpu in db.joined_desktops_monitors
                                        on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                        where assigned.user_assigned_id == assigned_user_id && (joined_desktop_cpu.desktop_cpu_id != null || joined_desktop_cpu.desktop_cpu_id != 0
                                       || joined_desktop_cpu.desktop_monitor_id != null || joined_desktop_cpu.desktop_monitor_id != 0) && (joined_desktop_cpu.desktop_cpu_id == unassign_cpu_id)

                                            select new
                                        {
                                            AssignedDesktop = assigned,
                                            User = user,
                                            DesktopStation_ = joined_desktop_cpu

                                        };
            return Ok(assigned_user_station);

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }
        //------------------------------------ GET: api/assigned_desktops/5( This API EndPoint will Get all the users that are have been assigned a CPU) END----



        //------------------------------------ GET: api/assigned_desktops/5( This API EndPoint will Get all the users that are not assigned a Monitor) START ---

        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/assigned_desktops/assigned_user_without_monitor")]
        public IHttpActionResult GetAssigned_Assigned_User_No_Monito(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }


                var assigned_station_assigned_user = from assigned in db.assigned_desktops

                                                     join user in db.users
                                                     on assigned.user_assigned_id equals user.id

                                                     join joined_desktop_cpu in db.joined_desktops_monitors
                                                     on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                                     where //joined_desktop_cpu.desktop_cpu_id == null || joined_desktop_cpu.desktop_cpu_id == 0
                                                   joined_desktop_cpu.desktop_monitor_id == null || joined_desktop_cpu.desktop_monitor_id == 0

                                                     select new
                                                     {

                                                         User = user,
                                                         /*  Fullname = user.fullname,
                                                           Username = user.username,
                                                           Id = user.id*/


                                                     };
                return Ok(assigned_station_assigned_user);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }

        //------------------------------------ GET: api/assigned_desktops/5( This API EndPoint will Get all the users that are not assigned a Monitor) START ---

        //----------------- -------- - GET: api/assigned_desktops/5 (This API EndPoint will Get all the users that are not assigned a cpu)

        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_without_cpu")]
        public IHttpActionResult GetAssigned_Assigned_User_No_CPU(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                var assigned_station_assigned_user = from assigned in db.assigned_desktops

                                                 join user in db.users
                                                 on assigned.user_assigned_id equals user.id

                                                 join joined_desktop_cpu in db.joined_desktops_monitors
                                                 on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                                 where joined_desktop_cpu.desktop_cpu_id == null || joined_desktop_cpu.desktop_cpu_id == 0
                                                 //joined_desktop_cpu.desktop_monitor_id == null || joined_desktop_cpu.desktop_monitor_id == 0

                                                 select new
                                                 {

                                                     User = user,
                                                     /*  Fullname = user.fullname,
                                                       Username = user.username,
                                                       Id = user.id*/


                                                 };
            return Ok(assigned_station_assigned_user);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }


        //------------------------------------------------GETTING THE MONITOR AND THE USER IT IS ASSIGEND TOO  START ---------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/assigned_desktops/get_monitor_infor_user_assoc")]
        public IHttpActionResult GetMonitorInforUserAssoc(int monitorId, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Step 1: Get the monitor information
                var monitorInfo = from monitor in db.desktop_monitors
                                  where monitor.id == monitorId
                                  select monitor;

                // Step 2: Join with joined_desktops_monitors to get the joined ID
                var monitorJoinInfo = from monitor in monitorInfo

                                      join joinedDesktop in db.joined_desktops_monitors
                                      on monitor.id equals joinedDesktop.desktop_monitor_id into monitorJoinGroup

                                      from joinedDesktop in monitorJoinGroup.DefaultIfEmpty()
                                      select new
                                      {
                                          Monitor = monitor,
                                          JoinedDesktop = joinedDesktop
                                      };

                // Step 3: Join with assigned_desktops to get the user_assigned_id
                var monitorAssignedInfo = from joinInfo in monitorJoinInfo

                                          join assignedDesktop in db.assigned_desktops
                                          on joinInfo.JoinedDesktop.id equals assignedDesktop.joined_desktop_monitor_cpu_id into assignedGroup

                                          from assignedDesktop in assignedGroup.DefaultIfEmpty()
                                          select new
                                          {
                                              joinInfo.Monitor,
                                             
                                              joinInfo.JoinedDesktop,
                                              AssignedDesktop = assignedDesktop
                                          };

                // Step 4: Join with users to get the user information
                var result = from assignedInfo in monitorAssignedInfo

                             join user in db.users
                             on assignedInfo.AssignedDesktop.user_assigned_id equals user.id into userGroup

                             from user in userGroup.DefaultIfEmpty()
                             select new
                             {
                                 AssignedDesktop = assignedInfo.AssignedDesktop,
                                 Monitor = assignedInfo.Monitor,
                                 User = user,
                               //  Attachment = assignedInfo.Monitor.attachment // Assuming 'attachment' is the column name for the file
                             };

                // Step 5: Handle the case where desktop_monitor_id is 0 or null
                var resultList = result.ToList();
                if (resultList.Count == 0 || resultList[0].Monitor == null)
                {
                    var monitor = db.desktop_monitors
                                    .Where(m => m.id == monitorId)
                                    .FirstOrDefault();
                    if (monitor == null)
                    {
                        return NotFound();
                    }
                    return Ok(new
                    {
                        Monitor = monitor,
                        User = "No user",
                       // Attachment = monitor.attachment // Assuming 'attachment' is the column name for the file
                    });
                }

                return Ok(resultList.First());
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }
        //------------------------------------------------GETTING THE MONITOR AND THE USER IT IS ASSIGEND TOO  START ---------------------------------------------------

        //---------------------------------------------------------GETTING THE CPU AND THE USER IT IS ASSIGEND TOO  START -----------------------------------------------------
        [ResponseType(typeof(desktop_monitors))]
        [HttpGet]
        [Route("api/assigned_desktops/get_cpu_infor_user_assoc")]
        public IHttpActionResult GetCPUInforUserAssoc(int CPUId, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                // Step 1: Get the CPU information
                var cpuInfo = from cpu in db.desktop_cpus
                              where cpu.id == CPUId
                              select cpu;

                // Step 2: Join with joined_desktops_monitors to get the joined ID
                var cpuJoinInfo = from cpu in cpuInfo
                                  join joinedDesktop in db.joined_desktops_monitors
                                  on cpu.id equals joinedDesktop.desktop_cpu_id into cpuJoinGroup
                                  from joinedDesktop in cpuJoinGroup.DefaultIfEmpty()
                                  select new
                                  {
                                      CPU = cpu,
                                      JoinedDesktop = joinedDesktop
                                  };

                // Step 3: Join with assigned_desktops to get the user_assigned_id
                var cpuAssignedInfo = from joinInfo in cpuJoinInfo
                                      join assignedDesktop in db.assigned_desktops
                                      on joinInfo.JoinedDesktop.id equals assignedDesktop.joined_desktop_monitor_cpu_id into assignedGroup
                                      from assignedDesktop in assignedGroup.DefaultIfEmpty()
                                      select new
                                      {
                                          joinInfo.CPU,
                                          joinInfo.JoinedDesktop,
                                          AssignedDesktop = assignedDesktop
                                      };

                // Step 4: Join with users to get the user information
                var result = from assignedInfo in cpuAssignedInfo
                             join user in db.users
                             on assignedInfo.AssignedDesktop.user_assigned_id equals user.id into userGroup
                             from user in userGroup.DefaultIfEmpty()
                             select new
                             {
                                 AssignedDesktop = assignedInfo.AssignedDesktop,
                                 CPU = assignedInfo.CPU,
                                 User = user
                             };

                // Step 5: Handle the case where desktop_cpu_id is 0 or null
                var resultList = result.ToList();
                if (resultList.Count == 0 || resultList[0].CPU == null)
                {
                    var cpu = db.desktop_cpus
                                 .Where(c => c.id == CPUId)
                                 .FirstOrDefault();
                    if (cpu == null)
                    {
                        return NotFound();
                    }
                    return Ok(new
                    {
                        CPU = cpu,
                        User = "No user"
                    });
                }

                return Ok(resultList.First());

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message });
            }

        }
        //---------------------------------------------------------GETTING THE CPU AND THE USER IT IS ASSIGEND TOO  END -----------------------------------------------------



        // PUT: api/assigned_desktops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_desktops(int id, assigned_desktops assigned_desktops, string token)
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

            if (id != assigned_desktops.id)
            {
                return BadRequest();
            }

            db.Entry(assigned_desktops).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!assigned_desktopsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // return StatusCode(HttpStatusCode.NoContent);
            return Ok(assigned_desktops);
        }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the assigned desktop.", Details = ex.Message});
            }
        }

        //-------------------------------------------------------- POST: api/assigned_desktops ASSIG A DESKTOP STATION TO A USER START -------------------------------------------------------------
        [ResponseType(typeof(assigned_desktops))]
        [HttpPost]
        [Route("api/assigned_desktops")]
        public IHttpActionResult Postassigned_desktops(assigned_desktops assigned_desktops, int monitor_id, int cpu_id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Initialize date_created if it is not set
                if (assigned_desktops.date_created == DateTime.MinValue)
                {
                    assigned_desktops.date_created = DateTime.Now;
                }
                // This condition checks if all the incoming data(data to be store) has satisfied the conditions
                //example correct data types
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Check for uniqueness of desktop_monitor_id and user_assigned_id
                var existingRecord = db.assigned_desktops.FirstOrDefault(x => x.joined_desktop_monitor_cpu_id == assigned_desktops.joined_desktop_monitor_cpu_id); // && x.user_assigned_id == assigned_desktops.user_assigned_id);
                if (existingRecord != null)
                {

                    return Content(HttpStatusCode.Conflict, new { message = "Record with same desktop monitor ID and user assigned ID already exists." });

                }

                //Get authernticated user id and save it
                int? authenticatedUserId = GetUserIdFromToken(token);
                if (authenticatedUserId.HasValue)
                {
                    assigned_desktops.user_created = authenticatedUserId.Value; // Set to authenticated user
                    assigned_desktops.user_updated = authenticatedUserId.Value; // Set to authenticated user
                }

                // Update status_id in desktop_monitor table where id is monitor_id
              
                    var monitor = db.desktop_monitors.FirstOrDefault(m => m.id == monitor_id);
                    if (monitor != null)
                    {
                        monitor.status_id = 2; // Set to the appropriate status_id
                    }
               

                // Update status_id in desktop_cpu table where id is cpu_id
               
                    var cpu = db.desktop_cpus.FirstOrDefault(c => c.id == cpu_id);
                    if (cpu != null)
                    {
                        cpu.status_id = 2; // Set to the appropriate status_id
                    }
               
                db.assigned_desktops.Add(assigned_desktops);
                db.SaveChanges();

                return Content(HttpStatusCode.Created, new { message = " Desktop Assigned successfully.", assigned_desktops = assigned_desktops });



            }
            catch (Exception ex)
            {
                //Returns an error message
                //return InternalServerError(ex);
                // Return error response
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
            }
        }
        //-------------------------------------------------------- POST: api/assigned_desktops ASSIG A DESKTOP STATION TO A USER END -------------------------------------------------------------


        private IHttpActionResult StatusCode(int v, string msg)
        {
            throw new NotImplementedException();
        }
        //-------------------------------------------------------------------------------------GET ALL ASSIGNED DESKTOP STATION START----------------------------------------------------------------------------------------------------
        [ResponseType(typeof(assigned_desktops))]
        [HttpGet]
        [Route("api/assigned_desktops/get_all_ass_desktopStation")]
        public IHttpActionResult CountAssignedDesktops(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count all records in the assigned_desktop table
                var assignedDesktopsCount = db.assigned_desktops.Count();

                return Ok(new { total_assigned_desktops = assignedDesktopsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

        //-------------------------------------------------------------------------------------GET ALL ASSIGNED DESKTOP STATION END----------------------------------------------------------------------------------------------------

        //------------------------------------- DELETE(UN - Assign a Monitor)---------------------------------------------

        [ResponseType(typeof(assigned_desktops))]
        [HttpDelete]
        [Route("api/assigned_desktops/un_assign_desktop_from_user")]
        public IHttpActionResult Deleteassigned_desktops(int id, string token)
        {
            // Validate the token
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                   

                    assigned_desktops assigned_desktops = db.assigned_desktops.Find(id);
                    if (assigned_desktops == null)
                    {

                        return Content(HttpStatusCode.NotFound, new { message = "Desktop with the provided ID was not found." });
                    }

                    int? authenticatedUserId = GetUserIdFromToken(token);
                    if (authenticatedUserId.HasValue)
                    {
                        assigned_desktops.user_updated = authenticatedUserId.Value; // Set to authenticated user
                    }

                    db.assigned_desktops.Remove(assigned_desktops);
                    db.SaveChanges();


                    transaction.Commit();
                    return Content(HttpStatusCode.Created, new { message = "Assigned Desktop Station has been successfully un-assigned from this user.", assigned_desktops = assigned_desktops });

                }
                catch (Exception ex)
                {
                    // Rollback incase of any issues in the transaction
                    transaction.Rollback();
                    // Log the exception details (ex) if needed
                    return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
                }
            }
        }

        //------------------------------------- DELETE(UN - Assign a Monitor)---------------------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool assigned_desktopsExists(int id)
        {
            return db.assigned_desktops.Count(e => e.id == id) > 0;
        }
    }
}