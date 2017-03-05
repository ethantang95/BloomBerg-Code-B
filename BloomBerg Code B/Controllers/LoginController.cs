using BloomBerg_Code_B.Client;
using BloomBerg_Code_B.Models;
using HelperCore.Optional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BloomBerg_Code_B.Controllers {
    [RoutePrefix("Login")]
    public class LoginController : ApiController {

        [HttpGet]
        [Route("")]
        public bool Get() {
            return BloomBergLoginClient.IsLoggedIn();
        }

        // POST: api/Login
        [HttpPost]
        [Route("")]
        public bool Post([FromBody]BloomBergLoginModel model) {
            try {
                Login(model);
                return true;
            } catch (Exception e) {
                throw e;
            }
        }

        private void Login(BloomBergLoginModel model) {
            BloomBergLoginClient.CreateClient(Optional.From(model));
            var client = BloomBergLoginClient.Client;
            client.Connect();
        }
    }
}
