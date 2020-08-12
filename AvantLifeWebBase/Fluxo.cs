using AvantLifeWebBase.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AvantLifeWebBase
{

    public class Fluxo : Base
    {

        public void IncluirFluxoMensalNovaProposta(PropostaModel proposta)
        {
            try
            {
                if (proposta.IdUsuario == null || proposta.IdEmpresa == null)
                    throw new Exception("Usuário sem permissão.");

                //Calcula os valores previstos 
                Produto produto = new Produto();
                ProdutoValorModel valores = produto.BuscarProdutoValorFluxo(proposta.IdProdutoValores.ToString());
                decimal valorInicial = (valores.ComissaoInicial * proposta.ValorMensal / 100);
                decimal valorAnual = (valores.ComissaoAnual * proposta.ValorMensal / 100);
                decimal valorFinal = (valores.ComissaoFinal * proposta.ValorMensal / 100);
                decimal valorBanco;
                decimal percentual;
                DateTime dataReferencia = proposta.DataInicio;
                DateTime dataAtual = DateTime.Now;
                DateTime dataLimite = proposta.DataInicio.AddYears(10);
                string dataConfirmacao;
                Situacao situacao;

                while (dataReferencia < dataLimite)
                {
                    //Verifica ano comissão
                    TimeSpan ano = dataReferencia - proposta.DataInicio;
                    //Como a variação é mensal e temos anos bissexto com 370 garante que mudou o mês corrigindo o calculo
                    if (ano.TotalDays < 365)
                    {
                        valorBanco = valorInicial;
                        percentual = valores.ComissaoInicial;

                    }
                    else if (ano.TotalDays < 1460)
                    {
                        valorBanco = valorAnual;
                        percentual = valores.ComissaoAnual;
                    }
                    else
                    {
                        valorBanco = valorFinal;
                        percentual = valores.ComissaoFinal;
                    }

                    if(dataReferencia < dataAtual)
                    {
                        dataConfirmacao = $"'{dataReferencia.ToString("yyyy-MM-dd")}'";
                        situacao = Situacao.Pago;
                    }
                    else
                    {
                        dataConfirmacao = "Null";
                        situacao = Situacao.Pendente;
                    }

                    using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                    {
                        connection.Open();

                        String query = $@" INSERT INTO FLUXO_MENSAL ( 
                                       NOME,              
                                       NUMERO_APOLICE,    
                                       VALOR_PAGO,   
                                       VALOR_COMISSAO,
                                       PERCENTUAL,
                                       DATA_PREVISTA,      
                                       DATA_CONFIRMACAO,
                                       DIA_REFERENCIA,
                                       MES_REFERENCIA,       
                                       ANO_REFERENCIA, 
                                       SITUACAO,
                                       ATIVO,       
                                       OBSERVACAO,
                                       ID_PROPOSTA,        
                                       ID_USUARIO,        
                                       ID_EMPRESA)    
                                       VALUES (
                                       '{proposta.Nome}',
                                       {proposta.NumeroApolice},
                                       {proposta.ValorMensal.ToString().Replace(',', '.')},
                                       {valorBanco.ToString().Replace(',', '.')},
                                       {percentual.ToString().Replace(',', '.')},
                                       '{dataReferencia.ToString("yyyy-MM-dd")}',
                                       {dataConfirmacao},
                                       {dataReferencia.Day},
                                       {dataReferencia.Month},
                                       {dataReferencia.Year},
                                       {(int)situacao},
                                       'True',
                                        NULL,
                                       '{proposta.Id}',
                                       '{proposta.IdUsuario}',
                                       '{proposta.IdEmpresa}')";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    if (dataReferencia.Day != proposta.DiaPagamento)
                    {
                        proposta.DataInicio = new DateTime(proposta.DataInicio.Year, proposta.DataInicio.Month, proposta.DiaPagamento);
                        dataReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, proposta.DiaPagamento);
                    }

                    dataReferencia = dataReferencia.AddMonths(1);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void InativarFluxoMensalCancelarProposta(PropostaModel proposta)
        {
            try
            {
                if (proposta.IdUsuario == null || proposta.IdEmpresa == null)
                    throw new Exception("Usuário sem permissão.");

                string observacao = "Fluxo inativo porque a proposta foi cancelada no dia " + DateTime.Now.ToString("dd/MM/yyyy");
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE FLUXO_MENSAL SET 
                                       OBSERVACAO = '{observacao}',       
                                       ATIVO = 'false'          
                                       WHERE ID_PROPOSTA = '{proposta.Id}'
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

        public List<FluxoMensalModel> BuscarFluxoMensalComissaoSemana(string id_usuario, string id_empresa, string dataInicio, string dataFinal)
        {
            try
            {
                List<FluxoMensalModel> fluxos = new List<FluxoMensalModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT *
                                        FROM FLUXO_MENSAL
                                        WHERE ATIVO = 1 
                                         AND ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'
                                         AND DATA_PREVISTA >= '{dataInicio}'
                                         AND DATA_PREVISTA < '{dataFinal}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FluxoMensalModel fluxo = new FluxoMensalModel();
                                fluxo.Id = Guid.Parse(reader["ID"].ToString());
                                fluxo.Nome = reader["NOME"].ToString();
                                fluxo.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                fluxo.ValorPago = Convert.ToDecimal(reader["VALOR_PAGO"].ToString());
                                fluxo.DataPrevista = Convert.ToDateTime(reader["DATA_PREVISTA"].ToString());
                                fluxo.DataConfirmacao = !String.IsNullOrEmpty(reader["DATA_CONFIRMACAO"].ToString()) ? 
                                    Convert.ToDateTime(reader["DATA_CONFIRMACAO"].ToString()): (DateTime?)null;
                                fluxo.DiaReferencia = Convert.ToInt32(reader["DIA_REFERENCIA"].ToString());
                                fluxo.MesReferencia = Convert.ToInt32(reader["MES_REFERENCIA"].ToString());
                                fluxo.AnoReferencia = Convert.ToInt32(reader["ANO_REFERENCIA"].ToString());
                                fluxo.Situacao = Convert.ToInt32(reader["SITUACAO"].ToString());
                                fluxo.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                fluxo.IdProposta = Guid.Parse(reader["ID_PROPOSTA"].ToString());
                                fluxo.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                fluxo.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                fluxo.Observacao = reader["OBSERVACAO"].ToString();
                                fluxo.Percentual = Convert.ToDecimal(reader["PERCENTUAL"].ToString());
                                fluxo.ValorComissao = Convert.ToDecimal(reader["VALOR_COMISSAO"].ToString());
                                fluxos.Add(fluxo);

                            }

                            return fluxos;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TotalizadorModel> BuscarFluxoMensalInicioMes(string id_usuario, string id_empresa, int mes, int ano)
        {
            try
            {
                List<TotalizadorModel> totais = new List<TotalizadorModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT SITUACAO, SUM(VALOR_COMISSAO) AS NUMERO
                                        FROM FLUXO_MENSAL
                                        WHERE ATIVO = 1 
                                         AND ID_USUARIO = '{id_usuario}' 
                                         AND ID_EMPRESA = '{id_empresa}'
                                         AND MES_REFERENCIA = {mes}
                                         AND ANO_REFERENCIA = {ano}
                                        GROUP BY SITUACAO";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TotalizadorModel contador = new TotalizadorModel();
                                contador.Descricao = reader["SITUACAO"].ToString();
                                contador.Valor = reader["NUMERO"] != null ? Convert.ToDecimal(reader["NUMERO"].ToString()) : 0;

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

        public void ConfirmarFluxoMensalLancamentos(List<string> fluxos_id, string id_usuario, string id_empresa)
        {
            try
            {
                if (String.IsNullOrEmpty(id_usuario) || String.IsNullOrEmpty(id_empresa))
                    throw new Exception("Usuário sem permissão.");
                                
                string ids_selecionados = String.Join(",", fluxos_id);
                string observacao = "Comissão confirmada no dia " + DateTime.Now.ToString("dd/MM/yyyy");
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE FLUXO_MENSAL SET 
                                       SITUACAO = {(int)Situacao.Pago},
                                       OBSERVACAO = '{observacao}'
                                       WHERE ID IN ({ids_selecionados})
                                         AND ID_USUARIO = '{id_usuario}'
                                         AND ID_EMPRESA = '{id_empresa}'";

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

        public void CancelarFluxoMensalLancamentos(List<string> fluxos_id, string id_usuario, string id_empresa)
        {
            try
            {
                if (String.IsNullOrEmpty(id_usuario) || String.IsNullOrEmpty(id_empresa))
                    throw new Exception("Usuário sem permissão.");

                string ids_selecionados = String.Join(",", fluxos_id);
                string observacao = "Comissão cancelada no dia " + DateTime.Now.ToString("dd/MM/yyyy");
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE FLUXO_MENSAL SET 
                                       SITUACAO = {(int)Situacao.Cancelado},
                                       OBSERVACAO = '{observacao}'
                                       WHERE ID IN ({ids_selecionados})
                                         AND ID_USUARIO = '{id_usuario}'
                                         AND ID_EMPRESA = '{id_empresa}'";

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

        public List<FluxoMensalRelatorioModel> BuscarRelatorioFluxoMensal(string nome, string situacao,
            string dataInicial, string dataFinal, string numeroApolice, string produto, string faixa, string id_usuario, string id_empresa)
        {
            try
            {                
                List<FluxoMensalRelatorioModel> fluxos = new List<FluxoMensalRelatorioModel>();
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@"  SELECT FM.* , 
                                               PROD.DESCRICAO AS PRODUTO_DESCRICAO,
                                               PROD.COBERTURA_ADICIONAL AS COBERTURA_ADICIONAL,
                                               F.*
                                        FROM FLUXO_MENSAL FM
                                        INNER JOIN PROPOSTA P ON FM.ID_PROPOSTA = P.ID
                                        INNER JOIN PRODUTO PROD ON P.ID_PRODUTO = PROD.ID
                                        INNER JOIN PRODUTO_VALORES F ON P.ID_PRODUTO_VALORES = F.ID
                                        WHERE FM.ID_USUARIO = '{id_usuario}' 
                                          AND FM.ID_EMPRESA = '{id_empresa}' ";

                    if (!string.IsNullOrEmpty(nome))
                        query += $"AND FM.NOME LIKE '%{nome}%' ";

                    if (!string.IsNullOrEmpty(situacao))
                        query += $"AND FM.SITUACAO = {situacao} ";

                    if (!string.IsNullOrEmpty(dataInicial))
                    {
                        string[] textInicialDate = dataInicial.Split('/');
                        query += $"AND FM.DATA_PREVISTA >= '{textInicialDate[2]}-{textInicialDate[1]}-{textInicialDate[0]}' ";
                    }

                    if (!string.IsNullOrEmpty(dataFinal))
                    {
                        string[] textFinallDate = dataFinal.Split('/');
                        query += $"AND FM.DATA_PREVISTA <= '{textFinallDate[2]}-{textFinallDate[1]}-{textFinallDate[0]}' ";                       
                    }

                    if (!string.IsNullOrEmpty(numeroApolice))
                        query += $"AND FM.NUMERO_APOLICE = {numeroApolice} ";

                    if (!string.IsNullOrEmpty(produto))
                        query += $"AND P.ID_PRODUTO = '{produto}' ";

                    if (!string.IsNullOrEmpty(faixa))
                        query += $"AND P.ID_PRODUTO_VALORES = '{faixa}' ";

                    query += $"ORDER BY FM.DATA_PREVISTA";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FluxoMensalRelatorioModel fluxo = new FluxoMensalRelatorioModel();
                                fluxo.Id = Guid.Parse(reader["ID"].ToString());
                                fluxo.Nome = reader["NOME"].ToString();
                                fluxo.NumeroApolice = Convert.ToInt32(reader["NUMERO_APOLICE"].ToString());
                                fluxo.ValorPago = Convert.ToDecimal(reader["VALOR_PAGO"].ToString());
                                fluxo.DataPrevista = Convert.ToDateTime(reader["DATA_PREVISTA"].ToString());
                                fluxo.DataConfirmacao = !String.IsNullOrEmpty(reader["DATA_CONFIRMACAO"].ToString()) ?
                                    Convert.ToDateTime(reader["DATA_CONFIRMACAO"].ToString()) : (DateTime?)null;
                                fluxo.DiaReferencia = Convert.ToInt32(reader["DIA_REFERENCIA"].ToString());
                                fluxo.MesReferencia = Convert.ToInt32(reader["MES_REFERENCIA"].ToString());
                                fluxo.AnoReferencia = Convert.ToInt32(reader["ANO_REFERENCIA"].ToString());
                                fluxo.Situacao = Convert.ToInt32(reader["SITUACAO"].ToString());
                                fluxo.Ativo = Convert.ToBoolean(reader["ATIVO"].ToString());
                                fluxo.IdProposta = Guid.Parse(reader["ID_PROPOSTA"].ToString());
                                fluxo.IdUsuario = Guid.Parse(reader["ID_USUARIO"].ToString());
                                fluxo.IdEmpresa = Guid.Parse(reader["ID_EMPRESA"].ToString());
                                fluxo.Observacao = reader["OBSERVACAO"].ToString();
                                fluxo.Percentual = Convert.ToDecimal(reader["PERCENTUAL"].ToString());
                                fluxo.ValorComissao = Convert.ToDecimal(reader["VALOR_COMISSAO"].ToString());
                                fluxo.ProdutoDescricao = reader["PRODUTO_DESCRICAO"].ToString();
                                fluxo.ProdutoCobertura = reader["COBERTURA_ADICIONAL"].ToString();
                                fluxo.FaixaEtaria = reader["FAIXA_ETARIA"].ToString();
                                fluxo.ComissaoInicial = Convert.ToDecimal(reader["COMISSAO_INICIAL"].ToString());
                                fluxo.ComissaoAnual = Convert.ToDecimal(reader["COMISSAO_ANUAL"].ToString());
                                fluxo.ComissaoFinal = Convert.ToDecimal(reader["COMISSAO_FINAL"].ToString());
                                fluxo.CapitalSegurado = Convert.ToDecimal(reader["CAPITAL_SEGURADO"].ToString());
                                fluxo.PremioMinimo = Convert.ToDecimal(reader["PREMIO_MINIMO"].ToString());
                                fluxos.Add(fluxo);

                            }

                            return fluxos;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AtualizaFluxoMensalAtrasadoUsuario(string id_usuario)
        {
            try
            {
                string dataAtrasado = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                using (SqlConnection connection = new SqlConnection(sqlConnection.ToString()))
                {
                    connection.Open();

                    String query = $@" UPDATE FLUXO_MENSAL 
                                       SET SITUACAO = {(int)Situacao.Atrasado}
                                       WHERE ATIVO = 1 
                                         AND ID_USUARIO = '{id_usuario}'
                                         AND SITUACAO = {(int)Situacao.Pendente}
                                         AND DATA_PREVISTA < '{dataAtrasado}'";

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
