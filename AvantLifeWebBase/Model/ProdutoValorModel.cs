using System;

namespace AvantLifeWebBase.Model
{
    public class ProdutoValorModel
    {
        public Guid Id { get; set; }

        public String FaixaEtaria { get; set; }

        public Decimal ComissaoInicial { get; set; }

        public Decimal ComissaoAnual { get; set; }

        public Decimal ComissaoFinal { get; set; }
        
        public Decimal CapitalSegurado { get; set; }

        public Decimal PremioMinimo { get; set; }

        public bool Ativo { get; set; }
    }
}
