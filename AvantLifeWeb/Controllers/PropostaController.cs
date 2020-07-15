using AvantLifeWebBase;
using AvantLifeWebBase.Model;
using System;
using System.Net;
using System.Web.Http;

namespace AvantLifeWeb.Controllers
{
    public class PropostaController : ApiController
    {
        private Proposta proposta = new Proposta();
        
        [ActionName("IncluirProposta")]
        [HttpPost]
        public IHttpActionResult IncluirProposta(PropostaModel registro)
        {
            try
            {
                proposta.IncluirProposta(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}