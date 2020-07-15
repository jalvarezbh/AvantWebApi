using System;

namespace AvantLifeWebBase.Model
{
    public class PropostaModel
    {
        public Guid Id { get; set; }

        public String Nome { get; set; }

        public String Email { get; set; }

        public String Celular { get; set; }

        public DateTime DataNascimento { get; set; }

        public bool PossuiFilho { get; set; }

        public long NumeroApolice { get; set; }

        public Guid IdProduto { get; set; }

        public Guid IdProdutoValores { get; set; }

        public Decimal ValorMensal { get; set; }

        public String FormaPagamento { get; set; }

        public int DiaPagamento { get; set; }

        public DateTime DataInicio { get; set; }

        public String Situacao { get; set; }

        public bool Ativo { get; set; }

        public Guid IdUsuario { get; set; }

        public Guid IdEmpresa { get; set; }
    }
}
