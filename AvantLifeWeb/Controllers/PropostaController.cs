﻿using AvantLifeWebBase;
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

        [ActionName("BuscarPropostasAniversariantes")]
        [HttpGet]
        public IHttpActionResult BuscarPropostasAniversariantes(string idusuario, string idempresa, int mes)
        {
            try
            {
                var retorno = proposta.BuscarPropostasAniversariantes(idusuario, idempresa, mes);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("login"))
                    return Content(HttpStatusCode.Unauthorized, ex.Message);

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("BuscarPropostasInicioMes")]
        [HttpGet]
        public IHttpActionResult BuscarPropostasInicioMes(string idusuario, string idempresa, int mes, int ano)
        {
            try
            {
                var retorno = proposta.BuscarPropostasInicioMes(idusuario, idempresa, mes, ano);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("BuscarRelatorioPropostas")]
        [HttpGet]
        public IHttpActionResult BuscarRelatorioPropostas(string nome, string situacao,
          string dataInicial, string dataFinal, string numeroApolice, string produto,
          string faixa, string idusuario, string idempresa)
        {
            try
            {
                var retorno = proposta.BuscarRelatorioPropostas(nome, situacao, dataInicial, dataFinal, numeroApolice, produto, faixa, idusuario, idempresa);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("BuscarRelatorioClientes")]
        [HttpGet]
        public IHttpActionResult BuscarRelatorioClientes(string nome, string mesInicial, 
            string mesFinal, string possuiFilhos,string genero, string idusuario, string idempresa)
        {
            try
            {
                var retorno = proposta.BuscarRelatorioClientes(nome, mesInicial, mesFinal, possuiFilhos, genero, idusuario, idempresa);
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
                PropostaModel incluido = proposta.IncluirProposta(registro);
                return Ok(incluido);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("login"))
                    return Content(HttpStatusCode.Unauthorized, ex.Message);

                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("AlterarProposta")]
        [HttpPost]
        public IHttpActionResult AlterarProposta(PropostaModel registro)
        {
            try
            {
                proposta.AlterarProposta(registro);
                return Ok(registro);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("ConfirmarProposta")]
        [HttpPost]
        public IHttpActionResult ConfirmarProposta(PropostaModel registro)
        {
            try
            {
                proposta.ConfirmarProposta(registro);
                var retorno = proposta.BuscarProposta(registro.Id.ToString(), registro.IdUsuario.ToString(), registro.IdEmpresa.ToString());
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("CancelarProposta")]
        [HttpPost]
        public IHttpActionResult CancelarProposta(PropostaModel registro)
        {
            try
            {
                proposta.CancelarProposta(registro);
                var retorno = proposta.BuscarProposta(registro.Id.ToString(), registro.IdUsuario.ToString(), registro.IdEmpresa.ToString());
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}