using System;

namespace AvantLifeWebBase.Model
{
    public class UsuarioModel
    {
        public Guid Id { get; set; }

        public String Nome { get; set; }
               
        public String Email { get; set; }

        public String CPF { get; set; }

        public String Senha { get; set; }

        public String Telefone { get; set; }

        public String Celular { get; set; }

        public Guid IdEmpresa { get; set; }
    }
}
