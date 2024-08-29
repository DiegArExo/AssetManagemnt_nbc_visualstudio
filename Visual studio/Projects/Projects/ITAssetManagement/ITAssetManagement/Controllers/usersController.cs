﻿using System;
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
    public class usersController : NbcController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();


        // private NbcController nbcController = new NbcController();

        //------------------------------------------ GET: api/users(GET ALL THE USERS)----------------------------------------
        [HttpGet]
        [Route("api/users")]
        public IHttpActionResult GetAllusers(string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                var users = db.users.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching users.", Details = ex.Message });
            }
        }

        //------------------------------------------ GET: api/users(TESTING CONNECTIVITY OF THE TOKEN)----------------------------------------
        [Route("api/test_connectivity")]
        public bool Connectivity()
        {
            return true;
        }

        // ----------------------------------------------------------GET: api/users/5(GET A SPECIFIC )------------------------------------------------
      
        [ResponseType(typeof(user))]
        [HttpGet]
        [Route("api/users/{id}")]
        public IHttpActionResult Getuser(int id, string token)
        {
            try
            {
                if (validate_token(token))
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
                }

                user user = db.users.Find(id);
                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"User with ID {id} not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while fetching the user.", Details = ex.Message });
            }
        }

        //-----------------------------------------------PUT: api/users/5 (UPDATE SPECIFIC USER END)---------------------------------------------------------
        [ResponseType(typeof(user))]
        public IHttpActionResult Putuser(int id, user user, string token)
        {
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
            }

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new { Message = "Invalid model state.", ModelState = ModelState });
            }

            if (id != user.id)
            {
                return Content(HttpStatusCode.BadRequest, new { Message = "The ID in the URL does not match the ID in the user object." });
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"User with ID {id} not found." });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new { Message = "A concurrency error occurred while updating the user." });
                }
            }
            catch (DbUpdateException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "A database update error occurred while updating the user.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while updating the user.", Details = ex.Message });
            }

            return Ok(new { Message = "User updated successfully.", Data = user });
        }


        // ------- ------------------------POST: api/users.(POST< CREATE A USER) ------------------------------------------------------------
        [ResponseType(typeof(user))]
        [HttpGet]
        [Route("api/users/{id}")]
        public IHttpActionResult Postuser(user user, string token)
        {
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Unauthorized access. Token validation failed." });
            }

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new { Message = "Invalid model state.", ModelState = ModelState });
            }

            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            try
            {
                db.users.Add(user);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "A database update error occurred while creating the user.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while creating the user.", Details = ex.Message });
            }

            return CreatedAtRoute("DefaultApi", new { id = user.id }, new { Message = "User created successfully.", Data = user });
        }


        //--------------------------------------------DELETE: api/users/5 (DELETE A USER)--------------------------------------------
        [ResponseType(typeof(user))]
        [HttpGet]
        public IHttpActionResult Deleteuser(int id, string token)
        {
            if (validate_token(token))
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Invalid or expired token." });
            }

            user user = db.users.Find(id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, new { Message = $"User with ID {id} not found." });
            }

            try
            {
                db.users.Remove(user);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "A database update error occurred while deleting the user.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "An error occurred while deleting the user.", Details = ex.Message });
            }

            return Ok(new { Message = "User deleted successfully.", Data = user });
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
                        var roles = db.roles.Where(r => r.user_id == user.id).FirstOrDefault();



                        if (user != null && roles != null)
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


        //-------------------------------------------------------------------------------------GENERATE TOKEN START HERE------------------------------------------------------------------------------------
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