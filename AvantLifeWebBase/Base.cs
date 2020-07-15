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
    }
}
