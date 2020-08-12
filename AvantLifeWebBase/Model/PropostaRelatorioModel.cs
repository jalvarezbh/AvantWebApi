using System;

namespace AvantLifeWebBase.Model
{
    public class PropostaRelatorioModel
    {
        public Guid Id { get; set; }

        public String Nome { get; set; }

        public String Email { get; set; }

        public String Celular { get; set; }

        public DateTime DataNascimento { get; set; }

        public String Genero { get; set; }

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

        public String Observacao { get; set; }

        public String ProdutoDescricao { get; set; }

        public String ProdutoCobertura { get; set; }

        public String FaixaEtaria { get; set; }

        public Decimal ComissaoInicial { get; set; }

        public Decimal ComissaoAnual { get; set; }

        public Decimal ComissaoFinal { get; set; }

        public Decimal CapitalSegurado { get; set; }

        public Decimal PremioMinimo { get; set; }
    }
}
