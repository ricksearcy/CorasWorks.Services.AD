using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorasWorks.Services.AD.Models;

namespace CorasWorks.Services.AD.Controllers
{
    public class ADUserController : ApiController
    {
        /// <summary>
        /// Add a new user using the ADUser model
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ADUser Post([FromBody] ADUser user)
        {
            using (AD ad = new AD())
            {
                ad.AddUser(user);
            }
            return user;
        }

        /// <summary>
        /// Search for username by user Email address. Email only is required property of the ADUser model POSTED in the rquest BDOY
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("api/ADUser/Username")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetUserName([FromBody] ADUser user)
        {
            using (AD ad = new AD())
            {

                HttpResponseMessage message = this.Request.CreateResponse(HttpStatusCode.OK,
         new { username = ad.GetUserName(user) });
                return message;
            }


        }

        /// <summary>
        ///  Check if a user exists bu username. Username only is required property ofthe ADUser model POSTED in the rquest BDOY
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("api/ADUser/Exists")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetUser([FromBody] ADUser user)
        {
            using (AD ad = new AD())
            {
                HttpResponseMessage message = this.Request.CreateResponse(HttpStatusCode.OK,
      new { userExists = ad.GetUser(user) });
                return message;
            }

        }

        /// <summary>
        /// Resets a users password. Username only is required property of the ADUser model POSTED in the rquest BDOY
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("api/ADUser/ResetPassword")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Reset([FromBody] ADUser user)
        {
            using (AD ad = new AD())
            {
                HttpResponseMessage message = this.Request.CreateResponse(HttpStatusCode.OK,
         new { password = ad.ResetPassword(user) });
                return message;
            }


        }

        /// <summary>
        /// Changes a users password. UserName, OldPassword and Password are the required properties of the ADUser model POSTED in the rquest BDOY
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("api/ADUser/ChangePassword")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Change([FromBody] ADUser user)
        {
            using (AD ad = new AD())
            {
                HttpResponseMessage message = this.Request.CreateResponse(HttpStatusCode.OK,
         new { passwordChanged = ad.ChangePassword(user) });
                return message;
            }

        }
    }
}
