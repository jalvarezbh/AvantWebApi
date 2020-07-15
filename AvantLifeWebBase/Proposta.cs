using AvantLifeWebBase.Model;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AvantLifeWebBase
{
    public class Proposta : Base
    {
        public void IncluirProposta(PropostaModel proposta)
        {
            try
            {
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
                                       ID_USUARIO,        
                                       ID_EMPRESA)    
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
                                       '{proposta.IdUsuario}',
                                       '{proposta.IdEmpresa}')";

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
