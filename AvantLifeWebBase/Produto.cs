using AvantLifeWebBase.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AvantLifeWebBase
{
    public class Produto : Base
    {
        public List<AutoCompleteModel> AutoCompleteProdutos(string id_empresa)
        {
            try
            {
                List<AutoCompleteModel> produtos = new List<AutoCompleteModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT ID, 
                                               DESCRICAO,                                               
                                               ID_EMPRESA  
                                       FROM PRODUTO
                                       WHERE ATIVO = 1 
                                         AND ID_EMPRESA = '{id_empresa}'
                                       ORDER BY DESCRICAO";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                throw new Exception("Produto não encontrado ou inativo.");

                            while (reader.Read())
                            {
                                AutoCompleteModel produto = new AutoCompleteModel();
                                produto.Id = Guid.Parse(reader["ID"].ToString());
                                produto.Descricao = reader["DESCRICAO"].ToString();
                                produtos.Add(produto);
                            }

                            return produtos;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AutoCompleteModel> AutoCompleteFaixas(string id_produto)
        {
            try
            {
                List<AutoCompleteModel> faixas = new List<AutoCompleteModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT ID, 
                                               (FAIXA_ETARIA + ' - Mínimo R$ ' + CAST(PREMIO_MINIMO AS VARCHAR(20))) AS DESCRICAO,                                              
                                               ID_PRODUTO  
                                       FROM PRODUTO_VALORES
                                       WHERE ATIVO = 1 
                                         AND ID_PRODUTO = '{id_produto}'
                                       ORDER BY DESCRICAO";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                throw new Exception("Produto valor não encontrado ou inativo.");

                            while (reader.Read())
                            {
                                AutoCompleteModel faixa = new AutoCompleteModel();
                                faixa.Id = Guid.Parse(reader["ID"].ToString());
                                faixa.Descricao = reader["DESCRICAO"].ToString();
                                faixas.Add(faixa);
                            }

                            return faixas;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ProdutoValorModel BuscarProdutoValorFluxo(string id)
        {
            try
            {
                ProdutoValorModel registro = new ProdutoValorModel();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" SELECT *
                                       FROM PRODUTO_VALORES
                                       WHERE ID = '{id}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                                throw new Exception("Produto valor não encontrado ou inativo.");

                            while (reader.Read())
                            {
                                registro.Id = Guid.Parse(reader["ID"].ToString());
                                registro.FaixaEtaria = reader["FAIXA_ETARIA"].ToString();
                                registro.ComissaoInicial = Convert.ToDecimal(reader["COMISSAO_INICIAL"].ToString());
                                registro.ComissaoAnual = Convert.ToDecimal(reader["COMISSAO_ANUAL"].ToString());
                                registro.ComissaoFinal = Convert.ToDecimal(reader["COMISSAO_FINAL"].ToString());
                                registro.CapitalSegurado = Convert.ToDecimal(reader["CAPITAL_SEGURADO"].ToString());
                                registro.PremioMinimo = Convert.ToDecimal(reader["PREMIO_MINIMO"].ToString());
                                registro.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                            }

                            return registro;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProdutoRelatorioModel> BuscarRelatorioProdutos(string descricao, string percentual,
           string capitalSegurado, string id_usuario, string id_empresa)
        {
            try
            {
                List<ProdutoRelatorioModel> produtos = new List<ProdutoRelatorioModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT P.ID AS PRODUTO_ID,
                                               P.DESCRICAO AS DESCRICAO,
                                               P.COBERTURA_ADICIONAL,
                                               P.ATIVO AS PRODUTO_ATIVO,
                                               PV.*
                                        FROM PRODUTO P
                                        INNER JOIN PRODUTO_VALORES PV ON P.ID = PV.ID_PRODUTO
                                        WHERE P.ID_EMPRESA = '{id_empresa}' ";

                    if (!string.IsNullOrEmpty(descricao))
                        query += $"AND P.DESCRICAO LIKE '%{descricao}%' ";

                    if (!string.IsNullOrEmpty(percentual))
                        query += $"AND PV.COMISSAO_INICIAL = {percentual} ";

                    if (!string.IsNullOrEmpty(capitalSegurado))
                        query += $"AND PV.CAPITAL_SEGURADO = {capitalSegurado} ";

                    query += $"ORDER BY P.DESCRICAO, PV.FAIXA_ETARIA";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProdutoRelatorioModel registro = new ProdutoRelatorioModel();
                                registro.ProdutoId = Guid.Parse(reader["ID"].ToString());
                                registro.ProdutoDescricao = reader["DESCRICAO"].ToString();
                                registro.ProdutoCobertura = reader["COBERTURA_ADICIONAL"].ToString();
                                registro.ProdutoAtivo = Convert.ToBoolean(reader["PRODUTO_ATIVO"].ToString());
                                registro.Id = Guid.Parse(reader["ID"].ToString());
                                registro.FaixaEtaria = reader["FAIXA_ETARIA"].ToString();
                                registro.ComissaoInicial = Convert.ToDecimal(reader["COMISSAO_INICIAL"].ToString());
                                registro.ComissaoAnual = Convert.ToDecimal(reader["COMISSAO_ANUAL"].ToString());
                                registro.ComissaoFinal = Convert.ToDecimal(reader["COMISSAO_FINAL"].ToString());
                                registro.CapitalSegurado = Convert.ToDecimal(reader["CAPITAL_SEGURADO"].ToString());
                                registro.PremioMinimo = Convert.ToDecimal(reader["PREMIO_MINIMO"].ToString());
                                registro.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());

                                produtos.Add(registro);
                            }

                            return produtos;
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
