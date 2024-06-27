using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ITAssetManagement.Models;

namespace ITAssetManagement.Controllers
{
    public class sdwan_stationController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        /*
        API EndPoint to get all the SD-WAN records
        */
        [ResponseType(typeof(object))]
        [Route("api/sdwan")]
        public IHttpActionResult Get_sdwan(string token)
        {
            try
            {
                var sdwanRecords = (from sdwan in db.sdwans
                                    join firewall in db.firewalls on sdwan.firewall_id equals firewall.id into fwGroup
                                    from fw in fwGroup.DefaultIfEmpty()

                                    join laptop in db.sdwan_laptops on sdwan.sdwanlaptop_id equals laptop.id into ltGroup
                                    from lt in ltGroup.DefaultIfEmpty()

                                    join router in db.router_mtc on sdwan.router_id equals router.id into rtGroup
                                    from rt in rtGroup.DefaultIfEmpty()

                                    join assignedSdwan in db.assigned_sdwans on sdwan.id equals assignedSdwan.sdwan_id into assGroup
                                    from assSdwan in assGroup.DefaultIfEmpty()

                                    join user in db.users on assSdwan.user_assigned_id equals user.id into userGroup
                                    from usr in userGroup.DefaultIfEmpty()

                                        // Check if assignedSdwan is not null to determine assignment status
                                    let isAssigned = assSdwan != null ? "Assigned" : "Not Assigned"
                                    let assignedData = assSdwan != null ? assSdwan.id : (int?)null

                                    where db.firewalls.Any(f => f.id == sdwan.firewall_id) ||
                                          db.sdwan_laptops.Any(l => l.id == sdwan.sdwanlaptop_id) ||
                                          db.router_mtc.Any(r => r.id == sdwan.router_id) ||
                                          db.assigned_sdwans.Any(a => a.sdwan_id == sdwan.id)
                                    select new
                                    {
                                        SDWAN = sdwan,
                                        Firewall = fw,
                                        Laptop = lt,
                                        Router = rt,
                                        User = usr, // Include user information
                                        AssignedSdwan = assSdwan,
                                          AssignedStatus = isAssigned,
                                        AssignedData = assignedData

                                    }).ToList();

                if (!sdwanRecords.Any())
                {
                    return Ok(new { Message = "No valid records found" });
                }

                return Ok(sdwanRecords);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET: api/sdwan/5
        [ResponseType(typeof(object))]
        [Route("api/sdwan/{id}")]
        public IHttpActionResult Get_sdwan(int id, string token)
        {
            try
            {
                var sdwanRecord = from sdwan in db.sdwans
                                  join firewall in db.firewalls on sdwan.firewall_id equals firewall.id into firewallGroup
                                  from fw in firewallGroup.DefaultIfEmpty()

                                  join laptop in db.sdwan_laptops on sdwan.sdwanlaptop_id equals laptop.id into laptopGroup
                                  from lt in laptopGroup.DefaultIfEmpty()

                                  join router in db.router_mtc on sdwan.router_id equals router.id into routerGroup
                                  from rt in routerGroup.DefaultIfEmpty()

                                  join assignedSdwan in db.assigned_sdwans
                                      on sdwan.id equals assignedSdwan.sdwan_id into assignedGroup
                                  from assSdwan in assignedGroup.DefaultIfEmpty()

                                  join user in db.users
                                      on assSdwan.user_assigned_id equals user.id into userGroup
                                  from usr in userGroup.DefaultIfEmpty()

                                  where sdwan.id == id
                                  select new
                                  {
                                      SDWAN = sdwan,
                                      Firewall = fw,
                                      Laptop = lt,
                                      Router = rt,
                                      User = usr,
                                      AssignedSdwan = assSdwan

                                  };

                if (!sdwanRecord.Any())
                {
                    return Ok(new object[] { });
                }

                return Ok(sdwanRecord.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
