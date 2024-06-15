using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult Get_sdwan()
        {
            try
            {
                var sdwanRecords = from s in db.sdwans
                                   join f in db.firewalls on s.firewall_id equals f.id into firewallGroup
                                   from f in firewallGroup.DefaultIfEmpty()

                                   join l in db.sdwan_laptops on s.sdwanlaptop_id equals l.id into laptopGroup
                                   from l in laptopGroup.DefaultIfEmpty()

                                   join r in db.router_mtc on s.router_id equals r.id into routerGroup
                                   from r in routerGroup.DefaultIfEmpty()

                                   join ass_assign_sdwan in db.assigned_sdwans
                                     on s.id equals ass_assign_sdwan.sdwan_id into assignGroup
                                   from ass_assign_sdwan in assignGroup.DefaultIfEmpty()

                                   where s.firewall_id == 0 || s.router_id == 0 || s.sdwanlaptop_id == 0
                                            || (s.firewall_id != 0 && s.router_id != 0 && s.sdwanlaptop_id != 0)

                                   select new
                                   {
                                       Sdwan = s,
                                       Firewall = f,
                                       Laptop = l,
                                       Router = r,
                                        Assigned = ass_assign_sdwan != null ? "Assigned" : "Not Assigned"
                                   };

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
        public IHttpActionResult Get_sdwan(int id)
        {
            try
            {
                var sdwanRecord = from s in db.sdwans
                                  join f in db.firewalls on s.firewall_id equals f.id into firewallGroup
                                  from f in firewallGroup.DefaultIfEmpty()
                                  join l in db.sdwan_laptops on s.sdwanlaptop_id equals l.id into laptopGroup
                                  from l in laptopGroup.DefaultIfEmpty()
                                  join r in db.router_mtc on s.router_id equals r.id into routerGroup
                                  from r in routerGroup.DefaultIfEmpty()
                                  where s.id == id
                                  select new
                                  {
                                      Sdwan = s,
                                      Firewall = f,
                                      Laptop = l,
                                      Router = r
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
