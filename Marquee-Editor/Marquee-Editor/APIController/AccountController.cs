using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Marquee_Editor.Models;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Marquee_Editor.APIController
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [Route("Token")]
        public IHttpActionResult PostToken(LoginViewModel model)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:51320/token");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string PostData = "grant_type=password&" + "username=" + model.Email + "&password=" + model.Password;
            //將要傳遞的資料PostData轉成Byte陣列並寫入request
            byte[] byteWordPost = Encoding.ASCII.GetBytes(PostData);
            //設定PostData長度
            request.ContentLength = byteWordPost.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(byteWordPost, 0, byteWordPost.Length);

            //取得網頁結果
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string JSON = readStream.ReadToEnd();
            JObject obj = JsonConvert.DeserializeObject<JObject>(JSON);
            string returnString = obj["access_token"].ToString();
            response.Close();

            return Ok(returnString);
        }
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> PostRegister(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // 沒有要傳送的 ModelState 錯誤，因此僅傳回空的 BadRequest。
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
