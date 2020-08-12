
using AvantLifeWebBase;
using AvantLifeWebBase.Model;
using System;
using System.Net;
using System.Web.Http;

namespace AvantLifeWeb.Controllers
{
    public class LoginController : ApiController
    {
        private Usuario usuario = new Usuario();
        private Fluxo fluxo = new Fluxo();

        [HttpPost]        
        public IHttpActionResult ValidarLogin(LoginModel loginModel)
        {
            try
            {              
                var login = usuario.ValidarLogin(loginModel.Email, loginModel.Senha);
                fluxo.AtualizaFluxoMensalAtrasadoUsuario(login.Id.ToString());
                return Ok(login);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("login"))
                    return Content(HttpStatusCode.Unauthorized, ex.Message);

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpGet]
        public IHttpActionResult ValidarTempoAcesso(string id)
        {
            try
            {
                var login = usuario.ValidarTempoAcesso(id);
                return Ok(login);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpGet]
        public IHttpActionResult EnviarEmailLembrarSenha(string email)
        {
            try
            {
                usuario.EnviarEmailLembrarSenha(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}