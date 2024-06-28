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
    public class laptop_invoiceController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

        // GET: api/laptop_invoice
        public IQueryable<laptop_invoice> Getlaptop_invoice()
        {
            return db.laptop_invoice;
        }

        // GET: api/laptop_invoice/5
        [ResponseType(typeof(laptop_invoice))]
        public IHttpActionResult Getlaptop_invoice(int id)
        {
            laptop_invoice laptop_invoice = db.laptop_invoice.Find(id);
            if (laptop_invoice == null)
            {
                return NotFound();
            }

            return Ok(laptop_invoice);
        }

        // PUT: api/laptop_invoice/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlaptop_invoice(int id, laptop_invoice laptop_invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != laptop_invoice.id)
            {
                return BadRequest();
            }

            db.Entry(laptop_invoice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!laptop_invoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/laptop_invoice
        [ResponseType(typeof(laptop_invoice))]
        public IHttpActionResult Postlaptop_invoice(laptop_invoice laptop_invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.laptop_invoice.Add(laptop_invoice);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = laptop_invoice.id }, laptop_invoice);
        }

        // DELETE: api/laptop_invoice/5
        [ResponseType(typeof(laptop_invoice))]
        public IHttpActionResult Deletelaptop_invoice(int id)
        {
            laptop_invoice laptop_invoice = db.laptop_invoice.Find(id);
            if (laptop_invoice == null)
            {
                return NotFound();
            }

            db.laptop_invoice.Remove(laptop_invoice);
            db.SaveChanges();

            return Ok(laptop_invoice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool laptop_invoiceExists(int id)
        {
            return db.laptop_invoice.Count(e => e.id == id) > 0;
        }
    }
}