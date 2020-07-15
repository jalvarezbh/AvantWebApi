using AvantLifeWebBase;
using System;
using System.Net;
using System.Web.Http;

namespace AvantLifeWeb.Controllers
{
    public class ProdutoController : ApiController
    {
        private Produto produto = new Produto();

        [ActionName("AutoCompleteProdutos")]
        [HttpGet]
        public IHttpActionResult AutoCompleteProdutos(string idempresa)
        {
            try
            {
                var retorno = produto.AutoCompleteProdutos(idempresa);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [ActionName("AutoCompleteFaixas")]
        [HttpGet]
        public IHttpActionResult AutoCompleteFaixas(string idproduto)
        {
            try
            {
                var retorno = produto.AutoCompleteFaixas(idproduto);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}
