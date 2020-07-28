﻿using AvantLifeWebBase.Model;
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
    }
}
