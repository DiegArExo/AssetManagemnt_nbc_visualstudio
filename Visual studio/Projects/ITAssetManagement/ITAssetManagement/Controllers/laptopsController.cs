/*NOTES 
 * 
 * g => g.Type == 1: This is a lambda expression where g is a placeholder representing each laptop in the laptops collection.
 *  db.laptops represents getting all the record from the table laptops
 
  */

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
    public class laptopsController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/laptops
        public IQueryable<laptop> Getlaptops()
        {
            return db.laptops;
        }
        /*(Loanable laptopes type is 1 and non-loanable is 0)
        API EndPoint to get all non loanable laptops*/
       // [ResponseType(typeof(laptop))]
        //[HttpGet]
       // [Route("api/laptops/nonloanlaptop")]
       // public IHttpActionResult Getnonloanablelaptop()
       // {
            //var non_loanable_laptop = db.laptops.Where(l => l.type == false ).ToList();
            ////Conditional statement to chack for availability
            ////Any() is a LINQ method that returns true if the collection contains any elements, and false if it is empty.
            //if (non_loanable_laptop == null || !non_loanable_laptop.Any())
            //{
            //    return NotFound();
            //}else
            //{
            //    return Ok(non_loanable_laptop);
            //}
            /* var non_loanable_laptop = from l_laptop in db.laptops
                                       join d_device_status in db.device_status
                                       on l_laptop.device_status_id equals d_device_status.id
                                      // where l_laptop.type == false

                                       select new
                                       {
                                           l_laptop, // This code will get all the laptops
                                           statusName = d_device_status.name // This code will get status only
                                       };
             return Ok(non_loanable_laptop);*/
           

       // }

       // [ResponseType(typeof(laptop))]
        //[HttpGet]
       // [Route("api/laptops/loanlaptop")]
       // public IHttpActionResult Getloanablelaptop()
       // {
            //var non_loanable_laptop = db.laptops.Where(l => l.type == true).ToList();
            ////Conditional statement to chack for availability
            ////Any() is a LINQ method that returns true if the collection contains any elements, and false if it is empty.
            //if (non_loanable_laptop == null || !non_loanable_laptop.Any())
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok(non_loanable_laptop);
            //}
          /*  var loanable_laptop = from loan_laptop in db.laptops
                                      join d_device_status in db.device_status
                                      on loan_laptop.device_status_id equals d_device_status.id
                                      //where loan_laptop.type == false

                                      select new
                                      {
                                          loan_laptop, // This code will get all the laptops
                                          statusName_ = d_device_status.name // This code will get status only
                                      };
            return Ok(loanable_laptop);
        }
        */
        // GET: api/laptops/5
        [ResponseType(typeof(laptop))]
        public IHttpActionResult Getlaptop(int id)
        {
            laptop laptop = db.laptops.Find(id);
            if (laptop == null)
            {
                return NotFound();
            }

            return Ok(laptop);
        }

        // PUT: api/laptops/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlaptop(int id, laptop laptop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != laptop.id)
            {
                return BadRequest();
            }

            db.Entry(laptop).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!laptopExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(laptop);
        }

        // POST: api/laptops
        [ResponseType(typeof(laptop))]
        public IHttpActionResult Postlaptop(laptop laptop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.laptops.Add(laptop);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = laptop.id }, laptop);
        }

        // DELETE: api/laptops/5
        [ResponseType(typeof(laptop))]
        public IHttpActionResult Deletelaptop(int id)
        {
            laptop laptop = db.laptops.Find(id);
            if (laptop == null)
            {
                return NotFound();
            }

            db.laptops.Remove(laptop);
            db.SaveChanges();

            return Ok(laptop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool laptopExists(int id)
        {
            return db.laptops.Count(e => e.id == id) > 0;
        }
    }
}