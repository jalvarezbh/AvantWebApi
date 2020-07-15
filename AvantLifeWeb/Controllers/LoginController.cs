
using AvantLifeWebBase;
using AvantLifeWebBase.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Http;

namespace AvantLifeWeb.Controllers
{   
    public class LoginController : ApiController
    {
        private Usuario usuario = new Usuario();
        
        [HttpPost]        
        public IHttpActionResult ValidarLogin(LoginModel loginModel)
        {
            try
            {              
                var login = usuario.ValidarLogin(loginModel.Email, loginModel.Senha);                
                return Ok(login);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}