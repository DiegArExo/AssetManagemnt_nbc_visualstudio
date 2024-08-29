using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ITAssetManagement.Models;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Web;

namespace ITAssetManagement.Controllers
{
    public class NbcController : ApiController
    {
        private ITAssetManagementDB db = new ITAssetManagementDB();


        //------------------This is the Function that will take the token and validate to see if the token exist  Start-----------------------------
        [Route("Token Validation")]
        public Boolean validate_token(string token)
        {
            DateTime current = DateTime.Now;

            var auth = db.authentication.Where(r => r.token == token).FirstOrDefault();

            if (auth != null)
            {
                //Quarying the database to get the toke and check if it equal to the one that was provided by the user
                // Check if token exists in database and is not expired
                var auth1 = db.authentication.Where(r => r.token == token)
                                           .Where(r => r.expiry_time <= current)
                                           .FirstOrDefault();
                if (auth1 == null)
                {
                    // Token exists but is expired or about to expire
                    DateTime expire_time = DateTime.Now.AddMinutes(15);

                    // Try to extend token expiration if within 15 minutes of expiry
                    authentication extend = db.authentication.Where(r => r.token == token)
                                                     .Where(r => r.expiry_time <= expire_time)
                                                     .FirstOrDefault();
                    if (extend != null)
                    {
                       extend.expiry_time = DateTime.Now.AddHours(1);
                       // extend.expiry_time = DateTime.Now.AddMinutes(2);

                        db.SaveChanges();
                    }
                    return false; // Token validation failed (expired or about to expire)
                }
                return true; // Token is valid and not expired
            }


            return true; // Token does not exist in database or is invalid
        }


        //------------------This is the Function that will take the token and validate to see if the token exist  END-----------------------------

        protected void exclude_date_created(ref ITAssetManagementDB db, object model, string token)
        {
            try
            {
                // Validate token
                if (validate_token(token))
                {
                    throw new UnauthorizedAccessException("Invalid or expired token.");
                }

                db.Entry(model).State = EntityState.Modified;
                db.Entry(model).Property("date_created").IsModified = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new InvalidOperationException("An UnauthorizedAccessException", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while excluding date_created.", ex);
            }

        }
        //Getting the Id of the logged in users
        // Getting the Id of the logged-in users
        internal int? GetUserIdFromToken(string token)
        {
            var authRecord = db.authentication.FirstOrDefault(a => a.token == token && a.expiry_time > DateTime.Now);
            if (authRecord != null)
            {
                return authRecord.user_id;
            }
            return null; // Return null if no valid token is found
        }

    }
}
