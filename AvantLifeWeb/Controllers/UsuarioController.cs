using AvantLifeWebBase;
using AvantLifeWebBase.Model;
using System;
using System.Net;
using System.Web.Http;

namespace AvantLifeWeb.Controllers
{
    public class UsuarioController : ApiController
    {
        private Usuario usuario = new Usuario();
        
        [ActionName("BuscarUsuario")]
        [HttpGet]
        public IHttpActionResult BuscarUsuario(string id, string idempresa)
        {
            try
            {               
                var retorno = usuario.BuscarUsuario(id, idempresa);               
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
        
        [ActionName("AlterarUsuario")]
        [HttpPut]
        public IHttpActionResult AlterarUsuario(UsuarioModel registro)
        {
            try
            {
                var retorno = usuario.AlterarUsuario(registro);              
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("AlterarSenhaUsuario")]
        [HttpPut]
        public IHttpActionResult AlterarSenhaUsuario(SenhaModel registro)
        {
            try
            {
                usuario.AlterarSenhaUsuario(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}