using AvantLifeWebBase.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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

                byte[] salt = new byte[128 / 8];
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                            password: senha,
                                            salt: salt,
                                            prf: KeyDerivationPrf.HMACSHA1,
                                            iterationCount: 10000,
                                            numBytesRequested: 256 / 8));

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = "SELECT ID, NOME, EMAIL, ID_EMPRESA, DATA_VIGENCIA, ATIVO  FROM USUARIO WHERE EMAIL = '{0}' AND SENHA = '{1}'";
                    query = string.Format(query, email, hashed);

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
                                login.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());

                                if (Convert.ToDateTime(reader["DATA_VIGENCIA"].ToString()) < DateTime.Now)
                                {
                                    InativarUsuario(login.Id.ToString());
                                    throw new Exception("Usuário com login bloqueado entre em contato com administrador pelo e-mail jalvarezbh@gmail.com");
                                }

                                if (!Convert.ToBoolean(reader["ATIVO"].ToString()))
                                    throw new Exception("Usuário com login bloqueado entre em contato com administrador pelo e-mail jalvarezbh@gmail.com");

                                AlterarUltimoAcesso(login.Id.ToString());
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

        public void ValidarEmail(string email)
        {
            try
            {                

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = "SELECT ID FROM USUARIO WHERE EMAIL = '{0}'";
                    query = string.Format(query, email);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.HasRows)
                                throw new Exception("E-mail já utilizado por outro usuário.");
                            
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
                                usuario.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
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

        public void InativarUsuario(string id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE USUARIO 
                                       SET ATIVO = 0
                                       WHERE ID = '{id}'";

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

        public void AlterarUltimoAcesso(string id)
        {
            try
            {
                string dataUltimoAcesso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE USUARIO 
                                       SET ULTIMO_LOGIN = '{dataUltimoAcesso}'
                                       WHERE ATIVO = 1 
                                         AND ID = '{id}'";

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

        public void AlterarSenhaUsuario(SenhaModel senha)
        {
            try
            {
                if (senha.SenhaNova != senha.SenhaRepetir)
                    throw new Exception("Campo Confirmar Senha Nova diferente do Campo Senha Nova.");

                byte[] salt = new byte[128 / 8];
                string hashedNova = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                            password: senha.SenhaNova,
                                            salt: salt,
                                            prf: KeyDerivationPrf.HMACSHA1,
                                            iterationCount: 10000,
                                            numBytesRequested: 256 / 8));

                string hashedAtual = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                           password: senha.SenhaAtual,
                                           salt: salt,
                                           prf: KeyDerivationPrf.HMACSHA1,
                                           iterationCount: 10000,
                                           numBytesRequested: 256 / 8));

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE USUARIO 
                                       SET SENHA = '{hashedNova}'
                                       WHERE ATIVO = 1 
                                         AND ID = '{senha.Id}'
                                         AND SENHA = '{hashedAtual}'";

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

        public void AlterarSenhaUsuarioToken(SenhaModel senha)
        {
            try
            {
                if (senha.SenhaNova != senha.SenhaRepetir)
                    throw new Exception("Campo Confirmar Senha Nova diferente do Campo Senha Nova.");

                byte[] salt = new byte[128 / 8];
                string hashedNova = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                            password: senha.SenhaNova,
                                            salt: salt,
                                            prf: KeyDerivationPrf.HMACSHA1,
                                            iterationCount: 10000,
                                            numBytesRequested: 256 / 8));                

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE USUARIO 
                                       SET SENHA = '{hashedNova}'
                                       WHERE ATIVO = 1 
                                         AND ID = '{senha.Id}'";

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

        public UsuarioModel InserirUsuario(UsuarioModel usuario)
        {
            try
            {
                ValidarEmail(usuario.Email);

                String telefone = Regex.Replace(usuario.Telefone, @"[^\w\d]", "");
                String celular = Regex.Replace(usuario.Celular, @"[^\w\d]", "");
                usuario.Telefone = telefone;
                usuario.Celular = celular;
                string dataInclusao = DateTime.Now.ToString("yyyy-MM-dd");
                string dataVigencia = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd");
                string ultimoLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                byte[] salt = new byte[128 / 8];
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                            password: usuario.Senha,
                                            salt: salt,
                                            prf: KeyDerivationPrf.HMACSHA1,
                                            iterationCount: 10000,
                                            numBytesRequested: 256 / 8));

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" INSERT INTO USUARIO
                                       ( NOME, 
                                         CPF, 
                                         EMAIL, 
                                         TELEFONE, 
                                         CELULAR, 
                                         SENHA, 
                                         DATA_INCLUSAO, 
                                         DATA_ULTIMO_PAGAMENTO, 
                                         DATA_VIGENCIA,
                                         ATIVO,
                                         ID_EMPRESA,
                                         ULTIMO_LOGIN)
                                        VALUES
                                        ( '{usuario.Nome}',
                                          '{usuario.CPF}',
                                          '{usuario.Email}',
                                          '{usuario.Telefone}',
                                          '{usuario.Celular}',
                                          '{hashed}',
                                          '{dataInclusao}',
                                           null,
                                          '{dataVigencia}',
                                          'true',
                                          '3EC26D33-997F-4FF2-9C5F-C328BF4CF38D',
                                          '{ultimoLogin}')";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        EnviarEmailNovoUsuario(usuario.Email);
                        return usuario;
                    }
                }                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EnviarEmailNovoUsuario(string email)
        {

            try
            {
                LoginModel login = new LoginModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = "SELECT ID, NOME, EMAIL, SENHA  FROM USUARIO WHERE EMAIL = '{0}' AND ATIVO = 1";
                    query = string.Format(query, email);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("E-mail não cadastrado.");

                            while (reader.Read())
                            {
                                login.Id = Guid.Parse(reader["ID"].ToString());
                                login.Nome = reader["NOME"].ToString();
                                login.Email = reader["EMAIL"].ToString();
                                login.Senha = reader["SENHA"].ToString();
                            }
                        }
                    }
                }

                string path = HttpContext.Current.Server.MapPath("\\Template.html");
                string html = string.Empty;
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("jalvarezbh@gmail.com");

                mailMessage.Subject = "Avant Life Planos";
                using (var arquivoHtml = new StreamReader(path))
                {
                    html = arquivoHtml.ReadToEnd();
                    html = html.Replace("[TITULO]", @"Seja Bem Vindo ao Avant Life.");
                    html = html.Replace("[SAUDACAO]", @"Prezado(a)," + login.Nome);


                    string linha1 = Linha(false, "Temos 3 opções de planos para melhor atendê-lo:", null);
                    string linha2 = Linha(true, "Plano Mensal: R$ 100,00", null);
                    string linha3 = Linha(true, "Plano Semestral: R$ 480,00", null);
                    string linha4 = Linha(true, "Plano Anual: R$ 720,00", null);
                    string linha5 = Linha(false, "Gentileza responder este e-mail com o plano escolhido ou ligar para (31)98860-8926.", null);
                    string linha6 = Linha(false, "Será encaminhado um boleto no valor correspondente para efetuar o pagamento com prazo de 3 dias úteis.", null); 
                    html = html.Replace("[LINHA]", linha1 + linha2 + linha3 + linha4 + linha5 + linha6);
                }

                mailMessage.IsBodyHtml = true;
                mailMessage.Body = html;
                mailMessage.To.Add(email);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("jalvarezbh@gmail.com", "Talita13")
                };

                smtp.Send(mailMessage);
                smtp.Dispose();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void EnviarEmailLembrarSenha(string email)
        {

            try
            {
                LoginModel login = new LoginModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = "SELECT ID, NOME, EMAIL, SENHA  FROM USUARIO WHERE EMAIL = '{0}' AND ATIVO = 1";
                    query = string.Format(query, email);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("E-mail não cadastrado.");

                            while (reader.Read())
                            {
                                login.Id = Guid.Parse(reader["ID"].ToString());
                                login.Nome = reader["NOME"].ToString();
                                login.Email = reader["EMAIL"].ToString();
                                login.Senha = reader["SENHA"].ToString();
                            }
                        }
                    }
                }

                string path = HttpContext.Current.Server.MapPath("\\Template.html");
                string html = string.Empty;
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("jalvarezbh@gmail.com");

                mailMessage.Subject = "Avant Life Redefinir Senha";
                using (var arquivoHtml = new StreamReader(path))
                {
                    html = arquivoHtml.ReadToEnd();
                    html = html.Replace("[TITULO]", @"Solicitação para redefinir a senha de acesso.");
                    html = html.Replace("[SAUDACAO]", @"Prezado(a)," + login.Nome);


                    string linha1 = Linha(false, "Para redefinir a senha click no link abaixo:", null);
                    string linha2 = Linha(false, "Link Avant Life", $"https://avantlife.com.br/redefinir?key={login.Id}&token={login.Senha}");
                    string linha3 = Linha(false, "Caso não tenha solicitado, favor desconsiderar o e-mail.", null);

                    html = html.Replace("[LINHA]", linha1 + linha2 + linha3);
                }

                mailMessage.IsBodyHtml = true;
                mailMessage.Body = html;
                mailMessage.To.Add(email);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("jalvarezbh@gmail.com", "Talita13")
                };

                smtp.Send(mailMessage);
                smtp.Dispose();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string Linha(bool strong, string texto, string link)
        {
            StringBuilder linha = new StringBuilder();
            linha.Append(@"<p style='font-family: arial,  helvetica, sans-serif; font-size: 12px; color: #606060;'>");

            if (strong)
            {
                linha.Append(@"<strong>");
                linha.Append(texto);
                linha.Append(@"</strong>");
            }
            else if (!string.IsNullOrEmpty(link))
            {
                linha.Append($"<a href='{link}'>");
                linha.Append(@"<strong>");
                linha.Append(texto);
                linha.Append(@"</strong>");
                linha.Append(@"</a>");
            }
            else
                linha.Append(texto);

            linha.Append(@"</p>");

            return linha.ToString();
        }
    }
}
