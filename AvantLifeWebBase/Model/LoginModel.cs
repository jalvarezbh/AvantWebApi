using System;

namespace AvantLifeWebBase.Model
{
    public class LoginModel
    {
        public Guid Id { get; set; }

        public String Nome { get; set; }

        public String Email { get; set; }

        public String Senha { get; set; }

        public Guid IdEmpresa { get; set; }

    }
}
