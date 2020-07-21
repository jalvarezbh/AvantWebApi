using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AvantLifeWebBase.Model
{
    public class FluxoMensalModel
    {
        public Guid Id { get; set; }

        public String Nome { get; set; }

        public long NumeroApolice { get; set; }

        public Decimal ValorPago { get; set; }

        public Decimal ValorComissao { get; set; }

        public Decimal Percentual { get; set; }

        public DateTime DataPrevista { get; set; }

        public DateTime DataConfirmacao { get; set; }

        public int DiaReferencia { get; set; }

        public int MesReferencia { get; set; }

        public int AnoReferencia { get; set; }

        public int Situacao { get; set; }

        public bool Ativo { get; set; }

        public String Observacao { get; set; }

        public Guid IdProposta { get; set; }

        public Guid IdUsuario { get; set; }

        public Guid IdEmpresa { get; set; }

    }
        
    public enum Situacao : short
    {
        [EnumMember]
        [Description("Pendente")]
        Pendente = 0,

        [EnumMember]
        [Description("Pago")]
        Pago = 1,

        [EnumMember]
        [Description("Atrasado")]
        Atrasado = 2
    }
}
