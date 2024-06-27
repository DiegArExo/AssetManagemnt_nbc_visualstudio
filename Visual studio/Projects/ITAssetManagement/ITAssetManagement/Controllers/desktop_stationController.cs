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
    public class desktop_stationController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        /*
       API EndPoint to get all the stations,
       station means Combinations of a Monitor and a CPU
       */

        [ResponseType(typeof(joined_desktops_monitors))]
        //[HttpGet]
        [Route("api/desktop_station")]
        public IHttpActionResult Get_desktop_station(string token)
        {

            try
            {
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
                return InternalServerError(ex);
            }

            /*  return Ok(desktop_station);
            var desktop_station = from m_monitor in db.desktop_monitors
                                  join j_desktop_join in db.joined_desktops_monitors
                                  on m_monitor.id equals j_desktop_join.desktop_monitor_id
                                  join c_cpu in db.desktop_cpus
                                  on j_desktop_join.desktop_cpu_id equals c_cpu.id
                                  select new
                                  {
                                      Monitor = m_monitor,
                                      DesktopJoin = j_desktop_join,
                                      Cpu = c_cpu
                                  };

            bool containsDesktopCPUId = desktop_station.Any(item => item.DesktopJoin.desktop_cpu_id == 59);
            bool containsDesktopMonitorId = desktop_station.Any(item => item.DesktopJoin.desktop_monitor_id == 59);

            if (containsDesktopCPUId && containsDesktopMonitorId == true)
            {
                return Ok(new { DesktopStation = desktop_station, Message = "5 found" });
            }*/





        }

        // GET: api/joined_desktops_monitors/5
        [ResponseType(typeof(joined_desktops_monitors))]
        //[HttpGet]
        [Route("api/specific/desktop_station")]
        public IHttpActionResult Get_desktop_station(int id, string token)
        {
            try
            {
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
                return InternalServerError(ex);
            }
            /*sdwan_laptops sdwan_laptops = db.sdwan_laptops.Find(id);
            if (sdwan_laptops == null)
            {
                return NotFound();
            }

            return Ok(sdwan_laptops);*/
        }


    }
}