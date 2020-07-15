using AvantLifeWebBase.Model;
using System;
using System.Data.SqlClient;

namespace AvantLifeWebBase
{
    public class Proposta : Base
    {
        public void IncluirProposta(PropostaModel proposta)
        {
            try
            {               
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
                                       {proposta.Nome},
                                       {proposta.Celular},
                                       {proposta.Email},
                                       {proposta.DataNascimento},
                                       {proposta.PossuiFilho},
                                       {proposta.Id_Produto},
                                       {proposta.Id_Produto_Valores},
                                       {proposta.ValorMensal},
                                       {proposta.FormaPagamento},
                                       {proposta.DiaPagamento},
                                       {proposta.DataInicio},
                                       {proposta.Situacao},
                                       1,
                                       {proposta.Id_Usuario},
                                       {proposta.Id_Empresa})";

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
