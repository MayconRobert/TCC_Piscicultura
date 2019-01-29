using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site_piscicultura.Models
{
    public class ModeloLogin
    {
        private string nomeUsuario, senha, email, cpf;
        private int nivelAcesso;
        private bool administrador;

        //ABRE CONEXÃO COM O BANCO
    

        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);



        [DisplayName("Nome de usuário")]
        public string NomeUsuario
        {
            get { return nomeUsuario; }
            set { nomeUsuario = value; }
        }

        public int NivelAcesso
        {
            get { return nivelAcesso; }
            set { nivelAcesso = value; }
        }

        public string Cpf
        {
            get { return cpf; }
            set { cpf = value; }
        }
        [DisplayName("Senha")]
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public bool Administrador
        {
            get { return administrador; }
            set { administrador = value; }
        }

        public ModeloLogin metodoLogar()
        {
            ModeloLogin ml = new ModeloLogin();
            //ATRIBUI VALOR AOS ATRIBUTOS
            bool res = false;
            ml.NivelAcesso = 0;

            //INICIA O TESTE
            try
            {
                // ABRE CONEXÃO
                con.Open();
                //SELECIONA A TABELA
                SqlCommand query =
                    new SqlCommand("SELECT atuacao, CPF FROM Usuarios WHERE nome = @nomeUsuario AND senha = @senha", con);
                //COMPARA OS VALORES
                query.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                query.Parameters.AddWithValue("@senha", senha);
                SqlDataReader leitor = query.ExecuteReader();
                //PERCORRE LINHA POR LINHA
                res = leitor.HasRows;
                if (leitor.Read())
                {
                    ml.NivelAcesso = int.Parse(leitor["atuacao"].ToString());
                    ml.Cpf = (leitor["CPF"].ToString());
                    
                    if (ml.nivelAcesso == 1)
                    {
                        administrador = true;
                    }
                }
                
                if (res == false)
                {
                    // FECHA LEITOR CASO ESTEJA ABERTO
                    leitor.Close();
                    ml = null;

                }
            }
            //CASO NÃO ENCONTRE
            catch (Exception e)
            {
                res = false;
                administrador = false;
                ml = null;
            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            //CONFERE AS RESPOSTAS CASO FOR FALSO

            return ml;
        }

        //PESQUISAR USUARIO

        public bool Pesquisar()
        {
            //ATRIBUI VALOR AOS ATRIBUTOS
            bool res = false;
            //INICIA O TESTE
            try
            {
                // ABRE CONEXÃO
                con.Open();
                //SELECIONA A TABELA
                SqlCommand query =
                    new SqlCommand("Select * FROM Usuario WHERE nome = @nomeUsuario", con);
                //COMPARA OS VALORES
                query.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                SqlDataReader leitor = query.ExecuteReader();
                //PERCORRE LINHA POR LINHA
                res = leitor.HasRows;
            }
            //CASO NÃO ENCONTRE
            catch (Exception e)
            {
                res = false;
            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();

            return res;
        }


        //METODO PARA RECUPERAR SENHA

        public ModeloLogin RecuperarSenha(string email)
        {
            //bool psw = false;

            ModeloLogin rec = new ModeloLogin();
                        
            rec.NomeUsuario = "";
            rec.Senha = "";
            

            try
            {
                //abrir a conexao
                con.Open();
                          
                
                //comando para atualizar a senha do usuario

               
                SqlCommand selecionar = new SqlCommand("SELECT nome,senha FROM Usuarios where email = @email ", con);

                selecionar.Parameters.AddWithValue("@email", email);
                
                SqlDataReader leitor1 = selecionar.ExecuteReader();

                


                if (leitor1.Read())
                {
                    rec.NomeUsuario = leitor1["nome"].ToString();
                    rec.Senha = leitor1["senha"].ToString();

                    
                    
                }                               


            }catch(Exception e)
            {
                //psw = false;
            }
            if (con.State == ConnectionState.Open)
                con.Close();

            return rec;
            
        }

        public bool SelectEmail(string email)
        {
            bool slcEmail = false;

            ModeloLogin nvaSenha = new ModeloLogin();

            nvaSenha.Email = "";

            try
            {
                //Abrir conexão
                con.Open();

                //Rodar um comando de select no banco de dados
                SqlCommand procurarEmail = new SqlCommand("SELECT email From Usuarios where email = @email", con);
                procurarEmail.Parameters.AddWithValue("@email", email);

                SqlDataReader leitor = procurarEmail.ExecuteReader();

                slcEmail = leitor.HasRows;

                if(leitor.Read())
                {
                   nvaSenha.Email = leitor["email"].ToString();


                    slcEmail = true;

                }


            }
            catch (Exception e)
            {
                slcEmail = false;
            }

            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return slcEmail;

        }

        public string EditarSenha(string senha, string nome)
        {
            try
            {
                con.Open();
                //Método para atualizar a senha do usuario
                SqlCommand edtSenha = new SqlCommand("UPDATE Usuarios SET senha = @senha WHERE nome =@nomeUsuario", con);
                edtSenha.Parameters.AddWithValue("@nomeUsuario", nome);
                edtSenha.Parameters.AddWithValue("@senha", senha);

                edtSenha.ExecuteNonQuery();




            }catch(Exception m)
            {
                return m.Message;

            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return "Senha atualizada com Sucesso!";



        }



    }
}