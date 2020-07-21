using AvantLifeWebBase;
using AvantLifeWebBase.Model;
using System;
using System.Net;
using System.Web.Http;

namespace AvantLifeWeb.Controllers
{
    public class FluxoController : ApiController
    {
        private Fluxo fluxo = new Fluxo();

        [ActionName("IncluirFluxoMensalNovaProposta")]
        [HttpPost]
        public IHttpActionResult IncluirFluxoMensalNovaProposta(PropostaModel registro)
        {
            try
            {
                fluxo.IncluirFluxoMensalNovaProposta(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("InativarFluxoMensalCancelarProposta")]
        [HttpPut]
        public IHttpActionResult InativarFluxoMensalCancelarProposta(PropostaModel registro)
        {
            try
            {
                fluxo.InativarFluxoMensalCancelarProposta(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}
