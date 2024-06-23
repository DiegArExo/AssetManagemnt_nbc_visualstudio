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
    public class assigned_desktopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/assigned_desktops
        public IQueryable<assigned_desktops> Getassigned_desktops()
        {
            return db.assigned_desktops;
        }

        // GET: api/assigned_desktops/5
        [ResponseType(typeof(assigned_desktops))]
        public IHttpActionResult Getassigned_desktops(int id)
        {
            assigned_desktops assigned_desktops = db.assigned_desktops.Find(id);
            if (assigned_desktops == null)
            {
                return NotFound();
            }

            return Ok(assigned_desktops);
        }

        // GET: api/assigned_desktops/assigned_user_station/5
        /*1, This API EndPoint will Get all the user information, That have a Desktop station But do not have a Monitor. 
         * 2. This users are assigned in the Assigned Table, with their station ID but are missing a monitor 
          */
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_station_nomonitor")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserNoMonitor(int assigned_user_id)
        {
            var assigned_user_station = from assigned in db.assigned_desktops

                         join user in db.users
                         on assigned.user_assigned_id equals user.id

                         join joined_desktop_cpu in db.joined_desktops_monitors
                         on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                        where assigned.user_assigned_id == assigned_user_id && (joined_desktop_cpu.desktop_cpu_id == null || joined_desktop_cpu.desktop_cpu_id == 0
                       || joined_desktop_cpu.desktop_monitor_id == null || joined_desktop_cpu.desktop_monitor_id == 0 )

                         select new
                         {
                             AssignedDesktop = assigned,
                             User = user,
                             DesktopStation_ = joined_desktop_cpu

                         };
            return Ok(assigned_user_station);

        }
        // GET: api/assigned_desktops/assigned_user_station/5
        /*1, This API EndPoint will Get all the user information, That have a Desktop station and have a desktop assigned to them Monitor. 
         * 2. This users are assigned in the Assigned Table, with their stationand have a monitor assigned to them 
          */
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_station_with_monitor")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserWithMonitor(int assigned_user_id)
        {
            var assigned_user_station = from assigned in db.assigned_desktops

                                        join user in db.users
                                        on assigned.user_assigned_id equals user.id

                                        join joined_desktop_cpu in db.joined_desktops_monitors
                                        on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                        where assigned.user_assigned_id == assigned_user_id && (joined_desktop_cpu.desktop_cpu_id != null || joined_desktop_cpu.desktop_cpu_id != 0
                                       || joined_desktop_cpu.desktop_monitor_id != null || joined_desktop_cpu.desktop_monitor_id != 0)

                                        select new
                                        {
                                            AssignedDesktop = assigned,
                                            User = user,
                                            DesktopStation_ = joined_desktop_cpu

                                        };
            return Ok(assigned_user_station);

        }

        // GET: api/assigned_desktops/assigned_user_station/5
        /*1, This API EndPoint will Get the Desktop Station ID  base on the Assigned User in the Assigned Desktop Table.
         2. This will get the desktop station and the users that do not have a CPU assigned to them 
         */
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_station_nocpu")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserNoCPU(int assigned_user_id)
        {
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
        // GET: api/assigned_desktops/assigned_user_station/5
        /*1, This API EndPoint will Get the Desktop Station ID  base on the Assigned User in the Assigned Desktop Table.
         2. This will get the desktop station and the users that have a CPU assigned to them 
         */
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_station_with_cpu")]
        public IHttpActionResult GetAssigned_Station_Assigned_UserWithCPU(int assigned_user_id)
        {
            var assigned_user_station = from assigned in db.assigned_desktops

                                        join user in db.users
                                        on assigned.user_assigned_id equals user.id

                                        join joined_desktop_cpu in db.joined_desktops_monitors
                                        on assigned.joined_desktop_monitor_cpu_id equals joined_desktop_cpu.id

                                        where assigned.user_assigned_id == assigned_user_id && (joined_desktop_cpu.desktop_cpu_id != null || joined_desktop_cpu.desktop_cpu_id != 0
                                       || joined_desktop_cpu.desktop_monitor_id != null || joined_desktop_cpu.desktop_monitor_id != 0)

                                        select new
                                        {
                                            AssignedDesktop = assigned,
                                            User = user,
                                            DesktopStation_ = joined_desktop_cpu

                                        };
            return Ok(assigned_user_station);

        }





        // GET: api/assigned_desktops/5
        /*1, This API EndPoint will Get all the users that are not assigned a Monitor */
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_without_monitor")]
        public IHttpActionResult GetAssigned_Assigned_User_No_Monito()
        {
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

        // GET: api/assigned_desktops/5
        /*1, This API EndPoint will Get all the users that are not assigned a cpu*/
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/assigned_user_without_cpu")]
        public IHttpActionResult GetAssigned_Assigned_User_No_CPU()
        {
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


        //--------GETTING THE MONITOR AND THE USER IT IS ASSIGEND TOO  START -----------
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/get_monitor_infor_user_assoc")]
        public IHttpActionResult GetMonitorInforUserAssoc(int monitorId)
        {
            // Step 1: Get the monitor information
            var monitorInfo = from monitor in db.desktop_monitors
                              where monitor.id == monitorId
                              select monitor;

            // Step 2: Join with joined_desktops_monitors to get the joined ID
            var monitorJoinInfo = from monitor in monitorInfo

                                  join joinedDesktop in db.joined_desktops_monitors
                                  on monitor.id equals joinedDesktop.desktop_monitor_id into monitorJoinGroup

                                  from joinedDesktop in monitorJoinGroup.DefaultIfEmpty()
                                  select new {
                                      Monitor = monitor,
                                      JoinedDesktop = joinedDesktop
                                  };

            // Step 3: Join with assigned_desktops to get the user_assigned_id
            var monitorAssignedInfo = from joinInfo in monitorJoinInfo

                                      join assignedDesktop in db.assigned_desktops
                                      on joinInfo.JoinedDesktop.id equals assignedDesktop.joined_desktop_monitor_cpu_id into assignedGroup

                                      from assignedDesktop in assignedGroup.DefaultIfEmpty()
                                      select new {
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
                             Monitor = assignedInfo.Monitor,
                             User = user
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
                    User = "No user"
                });
            }

            return Ok(resultList.First());
        }
        //--------GETTING THE MONITOR AND THE USER IT IS ASSIGEND TOO  END -----------

        //--------GETTING THE CPU AND THE USER IT IS ASSIGEND TOO  START -----------
        [ResponseType(typeof(desktop_monitors))]
        [Route("api/assigned_desktops/get_cpu_infor_user_assoc")]
        public IHttpActionResult GetCPUInforUserAssoc(int CPUId)
        {
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
        //--------GETTING THE CPU AND THE USER IT IS ASSIGEND TOO  END -----------

        // PUT: api/assigned_desktops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassigned_desktops(int id, assigned_desktops assigned_desktops)
        {
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

        // POST: api/assigned_desktops
        [ResponseType(typeof(assigned_desktops))]
        public IHttpActionResult Postassigned_desktops(assigned_desktops assigned_desktops)
        {
            try {
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
                var existingRecord = db.assigned_desktops.FirstOrDefault(x => x.joined_desktop_monitor_cpu_id == assigned_desktops.joined_desktop_monitor_cpu_id);// && x.user_assigned_id != assigned_desktops.user_assigned_id);
                if (existingRecord != null)
                {
                    // Record with same desktop_monitor_id and 

                    //return StatusCode(409, "Record with same desktop monitor ID ");
                    //return StatusCode(409);
                    // Record with same desktop monitor ID and user assigned ID already exists
                    return Content(HttpStatusCode.Conflict, new { message = "Record with same desktop monitor ID and user assigned ID already exists." });

                }
                db.assigned_desktops.Add(assigned_desktops);
                    db.SaveChanges();
                //Used for returning a response with status code 201(Created) and a Location header.
                //  return CreatedAtRoute("DefaultApi", new { id = assigned_desktops.id }, assigned_desktops);
                // Return success response
                return Content(HttpStatusCode.Created, new { message = "Assigned desktop created successfully.", assigned_desktops = assigned_desktops });



            }
            catch (Exception ex)
            {
                //Returns an error message
                //return InternalServerError(ex);
                // Return error response
                return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the assigned desktop.", error = ex.Message });
            }
        }

        private IHttpActionResult StatusCode(int v, string msg)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/assigned_desktops/5
        [ResponseType(typeof(assigned_desktops))]
        public IHttpActionResult Deleteassigned_desktops(int id)
        {
            assigned_desktops assigned_desktops = db.assigned_desktops.Find(id);
            if (assigned_desktops == null)
            {
                return NotFound();
            }

            db.assigned_desktops.Remove(assigned_desktops);
            db.SaveChanges();

            return Ok(assigned_desktops);
        }

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