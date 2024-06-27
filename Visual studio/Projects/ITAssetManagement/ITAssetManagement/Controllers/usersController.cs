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

using System.DirectoryServices.AccountManagement; // To interact with the active diretory we need to include this
using System.Security.Cryptography;
using ITAssetManagement.Controllers;


namespace ITAssetManagement.Controllers
{
    public class usersController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();

      
       private NbcController nbcController = new NbcController();

        //------------------------------------------ GET: api/users(GET ALL THE USERS)----------------------------------------
        public IQueryable<user> GetAllusers(string token)
        {
            if (nbcController.validate_token(token))
            {
               throw new Exception("401");
            }
            return db.users;
        }

        [Route("api/test_connectivity")]
        public bool Connectivity()
        {
            return true;
        }

        //---------------------------------------------------------------------------------GET SPECIFIC USER END----------------------------------------------------------------------------------------------------

        // ------------------------------------------------------------------------------------------------GET: api/users acc manager/5
        [Route("ape/account_manager/get_accounts")]
        [ResponseType(typeof(user))]
        public IHttpActionResult GetAccount_manager(string token)
        {
            if (nbcController.validate_token(token))
            {
                throw new Exception("401");
            }
            var acc_manager = from u in db.users
                              select new
                              {
                                  u.id,
                                  u.email,
                                  u.fullname,
                                  u.username,

                              };

            return Ok(acc_manager.OrderBy((item) => item.fullname));
        }



        // --------------------------------------------------------------------------------------------GET: api/users/5------------------------------------------------------------------------------------------------

        //[ResponseType(typeof(user))]
        // public IHttpActionResult Getuser(int id, string token)
        // {
        //     if (nbcController.validate_token(token))
        //     {
        //         throw new Exception("401");
        //     }
        //     user user = db.users.Find(id);
        //     if (user == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(user);
        // }

        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser(int id, string token)
        {

           user user = db.users.Find(id);
           if (user == null)
           {
                return NotFound();
            }

            return Ok(user);
        }
        //---------------------------------------------------------------------------------GET SPECIFIC USER END----------------------------------------------------------------------------------------------------




        // PUT: api/users/5

        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(int id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.id)
            {
                return BadRequest();
            }
            //------When updating a user also create a token for that user
            //base.exclude_date_created(ref db, user, user.token);

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);

            return Ok(user);
        }

        // ------- POST: api/users. Get all the user in the database but make sure that the user has a token
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user, string token)
        {
            if (!ModelState.IsValid ||nbcController.validate_token(user.token))
            {
                return BadRequest(ModelState);
            }

            db.users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.id }, user);
        }

        //--------------------------------------------DELETE: api/users/5--------------------------------------------
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(int id, string token)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }


        // ------- ------------------------------------------------------------AUTHENTICATION HAPPEN HERE GET: api/users/5---------------------------------------------------------------------------------------------
        [Route("api/user_authenticate")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult auth(string username, string password)
        {
            authentication auth_returned = null;
            //In this API we are authenticating a user based on the credentials
            //we create a token everytime a user logis in

            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "nbc.root"))
                {

                    if (pc.ValidateCredentials(username, password) == true)
                    {
                        //Get the user information where the username is equal to the username they provided
                        var user = db.users.Where(r => r.username == username).FirstOrDefault();


                        if (user != null)
                        {
                            authentication authentication = new authentication();
                            authentication.token = GenerateAPIKey();
                            authentication.user_id = user.id;
                            authentication.user_created = user.id;
                            authentication.user_updated = user.id;
                            authentication.expiry_time = DateTime.Now.AddHours(2);

                            db.authentication.Add(authentication);
                            db.SaveChanges();

                            auth_returned = authentication;

                            return Ok(new { auth_returned });
                            //return Ok(new
                            //{
                            //    token = authentication.token,
                            //    expiry_time = authentication.expiry_time,
                            //    user_id = authentication.user_id
                            //});
                        }

                    }

                }
            }
            catch (Exception x)
            {
                throw x.InnerException;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        // ------- ------------------------------------------------------------AUTHENTICATION HAPPEN HERE GET: api/users/5---------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------GENERATE TOKEN START HERE------------------------------------------------------------------------------------

        //This code her will be used to Generate a token 
        public static string GenerateAPIKey()
        {

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] apiKey = new byte[32];
            rng.GetBytes(apiKey);
            string s = System.Convert.ToBase64String(apiKey).Replace('+', '_')
                        .Replace('/', '-')
                        .TrimEnd('='); ;
            return s;
        }
        //-------------------------------------------------------------------------------------GENERATE TOKEN END HERE------------------------------------------------------------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.users.Count(e => e.id == id) > 0;
        }
    }
}