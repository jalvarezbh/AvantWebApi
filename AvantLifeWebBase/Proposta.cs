using AvantLifeWebBase.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AvantLifeWebBase
{
    public class Proposta : Base
    {
        public List<PropostaModel> BuscarPropostasPendente(string id_usuario, string id_empresa)
        {
            try
            {
                List<PropostaModel> propostas = new List<PropostaModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT *
                                        FROM PROPOSTA
                                        WHERE ATIVO = 1 
                                         AND ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'
                                         AND SITUACAO = 'Pendente'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PropostaModel proposta = new PropostaModel();
                                proposta.Id = Guid.Parse(reader["ID"].ToString());
                                proposta.Nome = reader["NOME"].ToString();
                                proposta.Celular = reader["CELULAR"].ToString();
                                proposta.Email = reader["EMAIL"].ToString();
                                proposta.DataNascimento = Convert.ToDateTime(reader["DATA_NASCIMENTO"].ToString());
                                proposta.PossuiFilho = Convert.ToBoolean(reader["POSSUI_FILHOS"].ToString());
                                proposta.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                proposta.IdProduto = Guid.Parse(reader["ID_PRODUTO"].ToString());
                                proposta.IdProdutoValores = Guid.Parse(reader["ID_PRODUTO_VALORES"].ToString());
                                proposta.ValorMensal = Convert.ToDecimal(reader["VALOR_MENSAL"].ToString());
                                proposta.FormaPagamento = reader["FORMA_PAGAMENTO"].ToString();
                                proposta.DiaPagamento = Convert.ToInt32(reader["DIA_PAGAMENTO"].ToString());
                                proposta.DataInicio = Convert.ToDateTime(reader["DATA_INICIO"].ToString());
                                proposta.Situacao = reader["SITUACAO"].ToString();
                                proposta.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                proposta.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                proposta.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                proposta.Observacao = reader["OBSERVACAO"].ToString();
                                propostas.Add(proposta);

                            }

                            return propostas;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PropostaModel> BuscarPropostas(string id_usuario, string id_empresa)
        {
            try
            {
                List<PropostaModel> propostas = new List<PropostaModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT *
                                        FROM PROPOSTA
                                        WHERE ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {                          
                            while (reader.Read())
                            {
                                PropostaModel proposta = new PropostaModel();
                                proposta.Id = Guid.Parse(reader["ID"].ToString());
                                proposta.Nome = reader["NOME"].ToString();
                                proposta.Celular = reader["CELULAR"].ToString();
                                proposta.Email = reader["EMAIL"].ToString();
                                proposta.DataNascimento = Convert.ToDateTime(reader["DATA_NASCIMENTO"].ToString());
                                proposta.PossuiFilho = Convert.ToBoolean(reader["POSSUI_FILHOS"].ToString());
                                proposta.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                proposta.IdProduto = Guid.Parse(reader["ID_PRODUTO"].ToString());
                                proposta.IdProdutoValores = Guid.Parse(reader["ID_PRODUTO_VALORES"].ToString());
                                proposta.ValorMensal = Convert.ToDecimal(reader["VALOR_MENSAL"].ToString());
                                proposta.FormaPagamento = reader["FORMA_PAGAMENTO"].ToString();
                                proposta.DiaPagamento = Convert.ToInt32(reader["DIA_PAGAMENTO"].ToString());
                                proposta.DataInicio = Convert.ToDateTime(reader["DATA_INICIO"].ToString());
                                proposta.Situacao = reader["SITUACAO"].ToString();
                                proposta.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                proposta.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                proposta.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                proposta.Observacao = reader["OBSERVACAO"].ToString();
                                propostas.Add(proposta);

                            }

                            return propostas;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PropostaModel> BuscarPropostasAniversariantes(string id_usuario, string id_empresa, int mes)
        {
            try
            {
                List<PropostaModel> propostas = new List<PropostaModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT *
                                        FROM PROPOSTA
                                        WHERE ATIVO = 1 
                                         AND ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'
                                         AND DATEPART(month, DATA_NASCIMENTO) = {mes}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PropostaModel proposta = new PropostaModel();
                                proposta.Id = Guid.Parse(reader["ID"].ToString());
                                proposta.Nome = reader["NOME"].ToString();
                                proposta.Celular = reader["CELULAR"].ToString();
                                proposta.Email = reader["EMAIL"].ToString();
                                proposta.DataNascimento = Convert.ToDateTime(reader["DATA_NASCIMENTO"].ToString());
                                proposta.PossuiFilho = Convert.ToBoolean(reader["POSSUI_FILHOS"].ToString());
                                proposta.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                proposta.IdProduto = Guid.Parse(reader["ID_PRODUTO"].ToString());
                                proposta.IdProdutoValores = Guid.Parse(reader["ID_PRODUTO_VALORES"].ToString());
                                proposta.ValorMensal = Convert.ToDecimal(reader["VALOR_MENSAL"].ToString());
                                proposta.FormaPagamento = reader["FORMA_PAGAMENTO"].ToString();
                                proposta.DiaPagamento = Convert.ToInt32(reader["DIA_PAGAMENTO"].ToString());
                                proposta.DataInicio = Convert.ToDateTime(reader["DATA_INICIO"].ToString());
                                proposta.Situacao = reader["SITUACAO"].ToString();
                                proposta.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                proposta.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                proposta.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                proposta.Observacao = reader["OBSERVACAO"].ToString();
                                propostas.Add(proposta);

                            }

                            return propostas;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TotalizadorModel> BuscarPropostasInicioMes(string id_usuario, string id_empresa, int mes, int ano)
        {
            try
            {
                List<TotalizadorModel> totais = new List<TotalizadorModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT SITUACAO, COUNT(*) AS NUMERO
                                        FROM PROPOSTA
                                        WHERE ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'
                                         AND DATEPART(month, DATA_INICIO) = {mes}
                                         AND DATEPART(year, DATA_INICIO) = {ano}
                                        GROUP BY SITUACAO";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TotalizadorModel contador = new TotalizadorModel();                              
                                contador.Descricao = reader["SITUACAO"].ToString();
                                contador.Valor = reader["NUMERO"] != null? Convert.ToDecimal(reader["NUMERO"].ToString()): 0;
                           
                                totais.Add(contador);

                            }

                            return totais;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PropostaRelatorioModel> BuscarRelatorioPropostas(string nome, string situacao,
            string dataInicial, string dataFinal, string numeroApolice, string produto, string faixa, string id_usuario, string id_empresa)
        {
            try
            {
                List<PropostaRelatorioModel> propostas = new List<PropostaRelatorioModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT P.* , 
                                               PROD.DESCRICAO AS PRODUTO_DESCRICAO,
                                               PROD.COBERTURA_ADICIONAL AS COBERTURA_ADICIONAL,
                                               F.*
                                        FROM PROPOSTA P ON FM.ID_PROPOSTA = P.ID
                                        INNER JOIN PRODUTO PROD ON P.ID_PRODUTO = PROD.ID
                                        INNER JOIN PRODUTO_VALORES F ON P.ID_PRODUTO_VALORES = F.ID
                                        WHERE FM.ID_USUARIO = '{id_usuario}' 
                                          AND FM.ID_EMPRESA = '{id_empresa}' ";

                    if (!string.IsNullOrEmpty(nome))
                        query += $"AND P.NOME LIKE '%{nome}%' ";

                    if (!string.IsNullOrEmpty(situacao))
                        query += $"AND P.SITUACAO = {situacao} ";

                    if (!string.IsNullOrEmpty(dataInicial))
                    {
                        string[] textInicialDate = dataInicial.Split('/');
                        query += $"AND P.DATA_INICIO >= '{textInicialDate[2]}-{textInicialDate[1]}-{textInicialDate[0]}' ";
                    }

                    if (!string.IsNullOrEmpty(dataFinal))
                    {
                        string[] textFinallDate = dataFinal.Split('/');
                        query += $"AND P.DATA_INICIO <= '{textFinallDate[2]}-{textFinallDate[1]}-{textFinallDate[0]}' ";
                    }

                    if (!string.IsNullOrEmpty(numeroApolice))
                        query += $"AND P.NUMERO_APOLICE = {numeroApolice} ";

                    if (!string.IsNullOrEmpty(produto))
                        query += $"AND P.ID_PRODUTO = '{produto}' ";

                    if (!string.IsNullOrEmpty(faixa))
                        query += $"AND P.ID_PRODUTO_VALORES = '{faixa}' ";

                    query += $"ORDER BY P.DATA_INICIO";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PropostaRelatorioModel proposta = new PropostaRelatorioModel();
                                proposta.Id = Guid.Parse(reader["ID"].ToString());
                                proposta.Nome = reader["NOME"].ToString();
                                proposta.Celular = reader["CELULAR"].ToString();
                                proposta.Email = reader["EMAIL"].ToString();
                                proposta.DataNascimento = Convert.ToDateTime(reader["DATA_NASCIMENTO"].ToString());
                                proposta.PossuiFilho = Convert.ToBoolean(reader["POSSUI_FILHOS"].ToString());
                                proposta.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                proposta.IdProduto = Guid.Parse(reader["ID_PRODUTO"].ToString());
                                proposta.IdProdutoValores = Guid.Parse(reader["ID_PRODUTO_VALORES"].ToString());
                                proposta.ValorMensal = Convert.ToDecimal(reader["VALOR_MENSAL"].ToString());
                                proposta.FormaPagamento = reader["FORMA_PAGAMENTO"].ToString();
                                proposta.DiaPagamento = Convert.ToInt32(reader["DIA_PAGAMENTO"].ToString());
                                proposta.DataInicio = Convert.ToDateTime(reader["DATA_INICIO"].ToString());
                                proposta.Situacao = reader["SITUACAO"].ToString();
                                proposta.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                proposta.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                proposta.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                proposta.Observacao = reader["OBSERVACAO"].ToString();
                                proposta.ProdutoDescricao = reader["PRODUTO_DESCRICAO"].ToString();
                                proposta.ProdutoCobertura = reader["COBERTURA_ADICIONAL"].ToString();
                                proposta.FaixaEtaria = reader["FAIXA_ETARIA"].ToString();
                                proposta.ComissaoInicial = Convert.ToDecimal(reader["COMISSAO_INICIAL"].ToString());
                                proposta.ComissaoAnual = Convert.ToDecimal(reader["COMISSAO_ANUAL"].ToString());
                                proposta.ComissaoFinal = Convert.ToDecimal(reader["COMISSAO_FINAL"].ToString());
                                proposta.CapitalSegurado = Convert.ToDecimal(reader["CAPITAL_SEGURADO"].ToString());
                                proposta.PremioMinimo = Convert.ToDecimal(reader["PREMIO_MINIMO"].ToString());
                                propostas.Add(proposta);

                            }

                            return propostas;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PropostaModel BuscarProposta(string id, string id_usuario, string id_empresa)
        {
            try
            {
                PropostaModel proposta = new PropostaModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT *
                                        FROM PROPOSTA
                                        WHERE ID = '{id}'
                                         AND ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("Não existe proposta.");

                            while (reader.Read())
                            {
                                
                                proposta.Id = Guid.Parse(reader["ID"].ToString());
                                proposta.Nome = reader["NOME"].ToString();
                                proposta.Celular = reader["CELULAR"].ToString();
                                proposta.Email = reader["EMAIL"].ToString();
                                proposta.DataNascimento = Convert.ToDateTime(reader["DATA_NASCIMENTO"].ToString());
                                proposta.PossuiFilho = Convert.ToBoolean(reader["POSSUI_FILHOS"].ToString());
                                proposta.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                proposta.IdProduto = Guid.Parse(reader["ID_PRODUTO"].ToString());
                                proposta.IdProdutoValores = Guid.Parse(reader["ID_PRODUTO_VALORES"].ToString());
                                proposta.ValorMensal = Convert.ToDecimal(reader["VALOR_MENSAL"].ToString());
                                proposta.FormaPagamento = reader["FORMA_PAGAMENTO"].ToString();
                                proposta.DiaPagamento = Convert.ToInt32(reader["DIA_PAGAMENTO"].ToString());
                                proposta.DataInicio = Convert.ToDateTime(reader["DATA_INICIO"].ToString());
                                proposta.Situacao = reader["SITUACAO"].ToString();
                                proposta.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                proposta.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                proposta.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                proposta.Observacao = reader["OBSERVACAO"].ToString();
                            }

                            return proposta;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PropostaModel IncluirProposta(PropostaModel proposta)
        {
            try
            {
                if (proposta.IdUsuario == null || proposta.IdEmpresa == null)
                    throw new Exception("Usuário sem permissão.");

                String celular = Regex.Replace(proposta.Celular, @"[^\w\d]", "");
                proposta.Celular = celular;
                string dataNascimento = proposta.DataNascimento.ToString("yyyy-MM-dd");
                String valorMensal = proposta.ValorMensal.ToString().Replace(',', '.');
                string dataInicio = proposta.DataInicio.ToString("yyyy-MM-dd");

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" INSERT INTO PROPOSTA ( 
                                       NOME,              
                                       CELULAR,           
                                       EMAIL,             
                                       DATA_NASCIMENTO,   
                                       POSSUI_FILHOS,     
                                       NUMERO_APOLICE,    
                                       ID_PRODUTO,        
                                       ID_PRODUTO_VALORES,
                                       VALOR_MENSAL,      
                                       FORMA_PAGAMENTO,
                                       DIA_PAGAMENTO,
                                       DATA_INICIO,       
                                       SITUACAO,          
                                       ATIVO,       
                                       OBSERVACAO,
                                       ID_USUARIO,        
                                       ID_EMPRESA)  
                                       OUTPUT INSERTED.ID
                                       VALUES (
                                       '{proposta.Nome}',
                                       '{proposta.Celular}',
                                       '{proposta.Email}',
                                       '{dataNascimento}',
                                       '{proposta.PossuiFilho}',
                                       {proposta.NumeroApolice},
                                       '{proposta.IdProduto}',
                                       '{proposta.IdProdutoValores}',
                                       {valorMensal},
                                       '{proposta.FormaPagamento}',
                                       {proposta.DiaPagamento},
                                       '{dataInicio}',
                                       '{proposta.Situacao}',
                                       '{proposta.Ativo}',
                                       '',
                                       '{proposta.IdUsuario}',
                                       '{proposta.IdEmpresa}')";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {                       
                        proposta.Id = (Guid)command.ExecuteScalar();
                    }
                }

                return proposta;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AlterarProposta(PropostaModel proposta)
        {
            try
            {
                if (proposta.IdUsuario == null || proposta.IdEmpresa == null)
                    throw new Exception("Usuário sem permissão.");

                String celular = Regex.Replace(proposta.Celular, @"[^\w\d]", "");
                proposta.Celular = celular;
                string dataNascimento = proposta.DataNascimento.ToString("yyyy-MM-dd");
                String valorMensal = proposta.ValorMensal.ToString().Replace(',', '.');
                string dataInicio = proposta.DataInicio.ToString("yyyy-MM-dd");

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE PROPOSTA SET 
                                       NOME = '{proposta.Nome}',              
                                       CELULAR = '{proposta.Celular}',           
                                       EMAIL =  '{proposta.Email}',             
                                       DATA_NASCIMENTO = '{dataNascimento}',   
                                       POSSUI_FILHOS = '{proposta.PossuiFilho}',     
                                       NUMERO_APOLICE = {proposta.NumeroApolice},    
                                       ID_PRODUTO = '{proposta.IdProduto}',        
                                       ID_PRODUTO_VALORES = '{proposta.IdProdutoValores}',
                                       VALOR_MENSAL = {valorMensal},      
                                       FORMA_PAGAMENTO = '{proposta.FormaPagamento}',
                                       DIA_PAGAMENTO =  {proposta.DiaPagamento},
                                       DATA_INICIO = '{dataInicio}',       
                                       SITUACAO = '{proposta.Situacao}',          
                                       ATIVO = '{proposta.Ativo}',
                                       OBSERVACAO = '{proposta.Observacao}'
                                       WHERE ID = '{proposta.Id}'
                                         AND ID_USUARIO = '{proposta.IdUsuario}'
                                         AND ID_EMPRESA = '{proposta.IdEmpresa}'";

                   

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

        public void ConfirmarProposta(PropostaModel proposta)
        {
            try
            {
                if (proposta.IdUsuario == null || proposta.IdEmpresa == null)
                    throw new Exception("Usuário sem permissão.");
                               
                string dataInicio = DateTime.Now.ToString("yyyy-MM-dd");

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE PROPOSTA SET 
                                       DATA_INICIO = '{dataInicio}',       
                                       SITUACAO = 'Confirmado'          
                                       WHERE ID = '{proposta.Id}'
                                         AND ID_USUARIO = '{proposta.IdUsuario}'
                                         AND ID_EMPRESA = '{proposta.IdEmpresa}'";



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

        public void CancelarProposta(PropostaModel proposta)
        {
            try
            {
                if (proposta.IdUsuario == null || proposta.IdEmpresa == null)
                    throw new Exception("Usuário sem permissão.");

                string observacao ="Proposta Cancelada no dia "+ DateTime.Now.ToString("dd/MM/yyyy");

                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE PROPOSTA SET 
                                       OBSERVACAO = '{observacao}',       
                                       SITUACAO = 'Cancelado'          
                                       WHERE ID = '{proposta.Id}'
                                         AND ID_USUARIO = '{proposta.IdUsuario}'
                                         AND ID_EMPRESA = '{proposta.IdEmpresa}'";



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
