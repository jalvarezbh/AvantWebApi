using AvantLifeWebBase.Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AvantLifeWebBase
{
    public class Base
    {
        public SqlConnectionStringBuilder sqlConnection { get; set; }

        public Base()
        {
            if (sqlConnection == null)
            {
                sqlConnection = new SqlConnectionStringBuilder();
                sqlConnection.DataSource = "184.168.194.51";
                sqlConnection.UserID = "avantlifeadm";
                sqlConnection.Password = "JMAtalita13#";
                sqlConnection.InitialCatalog = "avantlife";
            }
        }

        public bool ValidarTempoAcesso(string id)
        {
            try
            {
                LoginModel login = new LoginModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $"SELECT ID, ULTIMO_LOGIN FROM USUARIO WHERE ID = '{id}'";                   

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("E-mail ou Senha Inválido");

                            while (reader.Read())
                            {
                                DateTime ultimo_login = !String.IsNullOrEmpty(reader["ULTIMO_LOGIN"].ToString()) ? Convert.ToDateTime(reader["ULTIMO_LOGIN"].ToString()) : DateTime.Now;
                                ultimo_login = ultimo_login.AddHours(4);
                                if (ultimo_login < DateTime.Now)
                                    return false;
                                else
                                    return true;
                            }

                            return true;
                        }
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
