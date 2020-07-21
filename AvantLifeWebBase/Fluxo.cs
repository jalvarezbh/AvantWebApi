using AvantLifeWebBase.Model;
using System;
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

    }
}
