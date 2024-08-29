using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ITAssetManagement.Models;
namespace ITAssetManagement.Controllers
{
    public class desktop_stationController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        /*
       API EndPoint to get all the stations,
       station means Combinations of a Monitor and a CPU
       */

        [ResponseType(typeof(joined_desktops_monitors))]
        [HttpGet]
        [Route("api/desktop_station")]
        public IHttpActionResult Get_desktop_station(string token)
        {

            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                var desktop_station = from j_desktop_join in db.joined_desktops_monitors

                                      join m_monitor in db.desktop_monitors
                                      on j_desktop_join.desktop_monitor_id equals m_monitor.id into monitorGroup
                                      from m_monitor in monitorGroup.DefaultIfEmpty()

                                      join c_cpu in db.desktop_cpus
                                      on j_desktop_join.desktop_cpu_id equals c_cpu.id into cpuGroup
                                      from c_cpu in cpuGroup.DefaultIfEmpty()

                                      join ass_assign_desktop in db.assigned_desktops
                                      on j_desktop_join.id equals ass_assign_desktop.joined_desktop_monitor_cpu_id into assignGroup
                                      from ass_assign_desktop in assignGroup.DefaultIfEmpty()

                                     

                                      where j_desktop_join.desktop_cpu_id == 0 || j_desktop_join.desktop_monitor_id == 0
                                            || (j_desktop_join.desktop_cpu_id != 0 && j_desktop_join.desktop_monitor_id != 0)

                                      select new
                                      {
                                          Monitor = m_monitor,
                                          DesktopJoin = j_desktop_join,
                                          Cpu = c_cpu,
                                          Assigned = ass_assign_desktop != null ? "Assigned" : "Not Assigned",
                                          Assigned_data = ass_assign_desktop != null ? ass_assign_desktop.id : (int?)null
                                      };

                // Check if the result is empty and return an appropriate response
                if (!desktop_station.Any())
                {
                    return Ok(new { Message = "No valid records found" }); // Return a message if no valid records
                }

                return Ok(desktop_station);





            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }

          

        }

        // GET: api/joined_desktops_monitors/5
        [ResponseType(typeof(joined_desktops_monitors))]
        [HttpGet]
        [Route("api/specific/desktop_station")]
        public IHttpActionResult Get_desktop_station(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var desktop_station_specific = from j_desktop_join in db.joined_desktops_monitors

                                               join m_monitor in db.desktop_monitors
                                               on j_desktop_join.desktop_monitor_id equals m_monitor.id into monitorGroup
                                               from m_monitor in monitorGroup.DefaultIfEmpty()

                                               join c_cpu in db.desktop_cpus
                                               on j_desktop_join.desktop_cpu_id equals c_cpu.id into cpuGroup
                                               from c_cpu in cpuGroup.DefaultIfEmpty()

                                               join ass_assign_desktop in db.assigned_desktops
                                               on j_desktop_join.id equals ass_assign_desktop.joined_desktop_monitor_cpu_id into assignGroup
                                               from ass_assign_desktop in assignGroup.DefaultIfEmpty()
                                                   //Getting the user information
                                               join u_user in db.users
                                               on ass_assign_desktop.user_assigned_id equals u_user.id into userGroup
                                               from u_user in userGroup.DefaultIfEmpty()

                                               where j_desktop_join.id == id
                                               select new
                                               {
                                                   Monitor = m_monitor,
                                                   DesktopJoin = j_desktop_join,
                                                   Cpu = c_cpu,
                                                   Assigned = ass_assign_desktop,
                                                   User = u_user
                                               };

                if (!desktop_station_specific.Any())
                {
                    return Ok(new object[] { });

                }

                return Ok(desktop_station_specific);
            }
            catch (Exception ex)
            {
                //return InternalServerError(ex);
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
         
        }
        //-------------------------------------------------------------------------------------GET ALL DESKTOP STATION START----------------------------------------------------------------------------------------------------
        [ResponseType(typeof(joined_desktops_monitors))]
        [HttpGet]
        [Route("api/joined_desktops_monitors/count")]
        public IHttpActionResult CountJoinedDesktopsMonitors(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count all records in the joined_desktops_monitors table
                var joinedDesktopsMonitorsCount = db.joined_desktops_monitors.Count();

                return Ok(new { total_desktop_stations = joinedDesktopsMonitorsCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //-------------------------------------------------------------------------------------GET ALL DESKTOP STATION END----------------------------------------------------------------------------------------------------

        
        //-------------------------------------------------------------------------------------GET ALL AVAILABLE DESKTOP STATION START--------------------------------------------------------------------------------------------------

        [ResponseType(typeof(joined_desktops_monitors))]
        [HttpGet]
        [Route("api/joined_desktops_monitors/not_in_assigned_desktop_count")]
        public IHttpActionResult CountJoinedDesktopsMonitorsNotInAssignedDesktop(string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                // Count all records from joined_desktops_monitors that are not in assigned_desktop
                var joinedDesktopsMonitorsNotInAssignedDesktopCount = db.joined_desktops_monitors
                    .Count(jdm => !db.assigned_desktops.Any(ad => ad.joined_desktop_monitor_cpu_id == jdm.id));

                return Ok(new { total_available_desktop_stations = joinedDesktopsMonitorsNotInAssignedDesktopCount });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
        //-------------------------------------------------------------------------------------GET ALL AVAILABLE DESKTOP STATION END----------------------------------------------------------------------------------------------------






    }
}