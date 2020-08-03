using System;

namespace AvantLifeWebBase.Model
{
    public class FluxoMensalRelatorioModel
    {
        public Guid Id { get; set; }

        public String Nome { get; set; }

        public long NumeroApolice { get; set; }

        public Decimal ValorPago { get; set; }

        public Decimal ValorComissao { get; set; }

        public Decimal Percentual { get; set; }

        public DateTime DataPrevista { get; set; }

        public DateTime? DataConfirmacao { get; set; }

        public int DiaReferencia { get; set; }

        public int MesReferencia { get; set; }

        public int AnoReferencia { get; set; }

        public int Situacao { get; set; }

        public bool Ativo { get; set; }

        public String Observacao { get; set; }

        public Guid IdProposta { get; set; }

        public Guid IdUsuario { get; set; }

        public Guid IdEmpresa { get; set; }

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
