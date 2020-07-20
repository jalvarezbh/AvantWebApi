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

        [ActionName("BuscarPropostasPendente")]
        [HttpGet]
        public IHttpActionResult BuscarPropostasPendente(string idusuario, string idempresa)
        {
            try
            {
                var retorno = proposta.BuscarPropostasPendente(idusuario,idempresa);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("BuscarPropostas")]
        [HttpGet]
        public IHttpActionResult BuscarPropostas(string idusuario, string idempresa)
        {
            try
            {
                var retorno = proposta.BuscarPropostas(idusuario, idempresa);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }


        [ActionName("BuscarProposta")]
        [HttpGet]
        public IHttpActionResult BuscarProposta(string id, string idusuario, string idempresa)
        {
            try
            {
                var retorno = proposta.BuscarProposta(id, idusuario, idempresa);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
        
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

        [ActionName("AlterarProposta")]
        [HttpPut]
        public IHttpActionResult AlterarProposta(PropostaModel registro)
        {
            try
            {
                proposta.AlterarProposta(registro);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}