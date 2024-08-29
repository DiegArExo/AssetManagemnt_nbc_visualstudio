using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ITAssetManagement.Models;
using System.Net;

namespace ITAssetManagement.Controllers
{
    public class sdwan_stationController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // ------------------------------------------ API EndPoint to get all the SD-WAN records with the user assigned to it START--------------------------------------------
        [ResponseType(typeof(object))]
        [HttpGet]
        [Route("api/get_all_sdwan_stations")]
        public IHttpActionResult Get_sdwan_station(string token)
        {
            try
            {

                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

                var sdwanRecords = (from sdwan in db.sdwans
                                    join firewall in db.firewalls on sdwan.firewall_id equals firewall.id into fwGroup
                                    from fw in fwGroup.DefaultIfEmpty()

                                    join laptop in db.sdwan_laptops on sdwan.sdwanlaptop_id equals laptop.id into ltGroup
                                    from lt in ltGroup.DefaultIfEmpty()

                                    join router in db.router_mtc on sdwan.router_id equals router.id into rtGroup
                                    from rt in rtGroup.DefaultIfEmpty()

                                    join assignedSdwan in db.assigned_sdwans on sdwan.id equals assignedSdwan.sdwan_id into assGroup
                                    from assSdwan in assGroup.DefaultIfEmpty()

                                        // Check if assignedSdwan is not null to determine assignment status
                                    let isAssigned = assSdwan != null ? "Assigned" : "Not Assigned"
                                    let assignedData = assSdwan != null ? assSdwan.id : (int?)null

                                    select new
                                    {
                                        SDWAN = sdwan,
                                        Firewall = fw,
                                        Laptop = lt,
                                        Router = rt,

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

                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        // ------------------------------------------ API EndPoint to get all the SD-WAN records with the user assigned to it END--------------------------------------------


        // ------------------------------------------ API EndPoint to get a specific SD-WAN station with the associated user START--------------------------------------------
        [ResponseType(typeof(object))]
        [HttpGet]
        [Route("api/get_sdwan_stations_user_associated")]
        public IHttpActionResult Get_sdwan_and_user_associated(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }

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

                                    where sdwan.id == id
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
                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // ------------------------------------------ API EndPoint to get a specific SD-WAN station with the associated user End------------------------------------

        // ------------------------------------------ API EndPoint to get a specific SD-WAN station to be assigned START--------------------------------------------
        // GET: api/sdwan/5
        [ResponseType(typeof(object))]
        [HttpGet]
        [Route("api/sdwan_station_to_assign/{id}")]
        public IHttpActionResult Get_sdwan(int id, string token)
        {
            try
            {
                // Validate the token
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
                }
                var sdwanRecord = from sdwan in db.sdwans
                                  join firewall in db.firewalls on sdwan.firewall_id equals firewall.id into firewallGroup
                                  from fw in firewallGroup.DefaultIfEmpty()

                                  join laptop in db.sdwan_laptops on sdwan.sdwanlaptop_id equals laptop.id into laptopGroup
                                  from lt in laptopGroup.DefaultIfEmpty()

                                  join router in db.router_mtc on sdwan.router_id equals router.id into routerGroup
                                  from rt in routerGroup.DefaultIfEmpty()

                                   

                                  where sdwan.id == id
                                  select new
                                  {
                                      SDWAN = sdwan,
                                      Firewall = fw,
                                      Laptop = lt,
                                      Router = rt,
                                    

                                  };

                if (!sdwanRecord.Any())
                {
                    return Ok(new object[] { });
                }

                return Ok(sdwanRecord.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        // ------------------------------------------ API EndPoint to get a specific SD-WAN station to be assigned END--------------------------------------------
    }
}
