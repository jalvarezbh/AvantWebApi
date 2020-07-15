using AvantLifeWebBase.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AvantLifeWebBase
{
    public class Usuario : Base
    {
        public Usuario()
        {
            new Base();
        }

        public LoginModel ValidarLogin(string email, string senha)
        {
            try
            {
                LoginModel login = new LoginModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = "SELECT ID, NOME, EMAIL, ID_EMPRESA  FROM USUARIO WHERE EMAIL = '{0}' AND SENHA = '{1}'";
                    query = string.Format(query, email, senha);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("E-mail ou Senha Inválido");

                            while (reader.Read())
                            {
                                login.Id = Guid.Parse(reader["ID"].ToString());
                                login.Nome = reader["NOME"].ToString();
                                login.Email = reader["EMAIL"].ToString();
                                login.Id_Empresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                            }

                            return login;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UsuarioModel BuscarUsuario(string id, string id_empresa)
        {
            try
            {
                UsuarioModel usuario = new UsuarioModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT ID, 
                                              NOME, 
                                              EMAIL, 
                                              CPF, 
                                              TELEFONE, 
                                              CELULAR, 
                                              ID_EMPRESA  
                                       FROM USUARIO
                                       WHERE ATIVO = 1 
                                         AND ID = '{id}' 
                                         AND ID_EMPRESA = '{id_empresa}'";                    

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("Usuário não encontrado ou inativo.");

                            while (reader.Read())
                            {
                                usuario.Id = Guid.Parse(reader["ID"].ToString());
                                usuario.Nome = reader["NOME"].ToString();
                                usuario.Email = reader["EMAIL"].ToString();
                                usuario.CPF = reader["CPF"].ToString();
                                usuario.Telefone = reader["TELEFONE"].ToString();
                                usuario.Celular = reader["CELULAR"].ToString();
                                usuario.Id_Empresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                            }

                            return usuario;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UsuarioModel AlterarUsuario(UsuarioModel usuario)
        {
            try
            {
                String telefone = Regex.Replace(usuario.Telefone, @"[^\w\d]", ""); 
                String celular = Regex.Replace(usuario.Celular, @"[^\w\d]", "");
                usuario.Telefone = telefone;
                usuario.Celular = celular;
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE USUARIO 
                                       SET NOME = '{usuario.Nome}',
                                           EMAIL = '{usuario.Email}',
                                           CPF = '{usuario.CPF}', 
                                           TELEFONE = '{usuario.Telefone}', 
                                           CELULAR = '{usuario.Celular}'
                                       WHERE ATIVO = 1 
                                         AND ID = '{usuario.Id}'";                                                       

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        return usuario;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AlterarSenhaUsuario(SenhaModel senha)
        {
            try
            {
                if (senha.SenhaNova != senha.SenhaRepetir)
                    throw new Exception("Campo Confirmar Senha Nova diferente do Campo Senha Nova.");

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE USUARIO 
                                       SET SENHA = '{senha.SenhaNova}'
                                       WHERE ATIVO = 1 
                                         AND ID = '{senha.Id}'
                                         AND SENHA = '{senha.SenhaAtual}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
