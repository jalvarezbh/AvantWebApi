using AvantLifeWebBase;
using AvantLifeWebBase.Model;
using System;
using System.Collections.Generic;
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

        [ActionName("BuscarFluxoMensalComissaoSemana")]
        [HttpGet]
        public IHttpActionResult BuscarFluxoMensalComissaoSemana(string idusuario, string idempresa, string dataInicio , string dataFinal)
        {
            try
            {
                var retorno = fluxo.BuscarFluxoMensalComissaoSemana(idusuario, idempresa, dataInicio, dataFinal);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("BuscarFluxoMensalInicioMes")]
        [HttpGet]
        public IHttpActionResult BuscarFluxoMensalInicioMes(string idusuario, string idempresa, int mes, int ano)
        {
            try
            {
                var retorno = fluxo.BuscarFluxoMensalInicioMes(idusuario, idempresa, mes, ano);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("ConfirmarFluxoMensalLancamentos")]
        [HttpPut]
        public IHttpActionResult ConfirmarFluxoMensalLancamentos(FluxoAcaoModel parametros)
        {
            try
            {
                fluxo.ConfirmarFluxoMensalLancamentos(parametros.Ids , parametros.IdUsuario, parametros.IdEmpresa);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("CancelarFluxoMensalLancamentos")]
        [HttpPut]
        public IHttpActionResult CancelarFluxoMensalLancamentos(FluxoAcaoModel parametros)
        {
            try
            {
                fluxo.CancelarFluxoMensalLancamentos(parametros.Ids, parametros.IdUsuario, parametros.IdEmpresa);
                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}
