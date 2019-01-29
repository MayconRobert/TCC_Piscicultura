using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Site_piscicultura.Models
{
    public class ModeloUsuario
    {
        private string nome, senha, email, estado, cidade, bairro, endereco, num_Casa, complemento, cpf;
        private int plano, atuacao, notificacao, estatus;

       // ABRE CONEXÃO COM O BANCO
        static SqlConnection con =
          new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);



        public int Estatus
        {
            get { return estatus; }
            set { estatus = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

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

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; }
        }

        public string Bairro
        {
            get { return bairro; }
            set { bairro = value; }
        }

        [DisplayName("Endereço")]
        public string Endereco
        {
            get { return endereco; }
            set { endereco = value; }
        }

        [DisplayName("Número da Casa")]
        public string Num_Casa
        {
            get { return num_Casa; }
            set { num_Casa = value; }
        }

        public string Complemento
        {
            get { return complemento; }
            set { complemento = value; }
        }

        public int Plano
        {
            get { return plano; }
            set { plano = value; }
        }

        [DisplayName("Atuação")]
        public int Atuacao
        {
            get { return atuacao; }
            set { atuacao = value; }
        }

        [DisplayName("Notificações")]
        public int Notificacao
        {
            get { return notificacao; }
            set { notificacao = value; }
        }

        [DisplayName("CPF")]
        public string Cpf
        {
            get { return cpf; }
            set { cpf = value; }
        }


        internal string CriarUsuario()
        {
            SqlConnection con1 =
             new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);

           

            //TESTAR A CONEXÃO PARA VER SE ESTÁ CERTO

            try
            {
                string autentica = "";

                con1.Open();
                con.Open();

                // Teste para ver se o já existe o email cadastrado

                SqlCommand verificaEmail = new SqlCommand("SELECT email from Usuarios WHERE email = @email", con1);

                verificaEmail.Parameters.AddWithValue("@email", email);

                SqlDataReader leitor = verificaEmail.ExecuteReader();

                while (leitor.Read())
                {
                    if (leitor.HasRows)
                    {

                        autentica = leitor["email"].ToString();

                        if (email == autentica)
                        {
                            return "Email já cadastrado. Entre com um novo email.";
                        }
                       
                    }
                }

                //Fecha a conexão do leitor
                if (con1.State == ConnectionState.Open)
                    con1.Close();

                SqlCommand query = new SqlCommand("INSERT INTO Usuarios(nome,senha,email,estado,cidade,bairro,endereco,numCasa,notificacoes,atuacao,complemento,cpf,plano)" +
                "Values (@nomeUsuario, @senha, @email, @estado, @cidade, @bairro, @endereco, @numCasa, @notificacoes, @atuacao, @complemento, @cpf,@plano)", con);
                query.Parameters.AddWithValue("@nomeUsuario", nome);
                query.Parameters.AddWithValue("@senha", senha);
                query.Parameters.AddWithValue("@email", email);
                query.Parameters.AddWithValue("@estado", estado);
                query.Parameters.AddWithValue("@cidade", cidade);
                query.Parameters.AddWithValue("@bairro", bairro);
                query.Parameters.AddWithValue("@endereco", endereco);
                query.Parameters.AddWithValue("@numCasa", num_Casa);
                query.Parameters.AddWithValue("@plano", plano);
                query.Parameters.AddWithValue("@notificacoes", notificacao);
                query.Parameters.AddWithValue("@atuacao", atuacao);
                query.Parameters.AddWithValue("@complemento", complemento);
                query.Parameters.AddWithValue("@cpf", cpf);
                //query.Parameters.AddWithValue("@estatus", estatus);


                query.ExecuteNonQuery();

            }
            

            catch (Exception e)
            {
                return e.Message;
            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            
            //RETORNA UMA MENSAGEM SE INSERIU OU NÃO
            return "Usuário criado com sucesso!"; // Retorna a mensagem
        }
        

    

        internal string EditarUsuario()
        {

            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE USUARIOS SET Nome = @nome , Email = @email , Estado = @estado ,Cidade = @cidade, Bairro = @Bairro, Endereco = @endereco, NumCasa = @numCasa, Complemento = @complemento where CPF = @cpf", con);

                atualizar.Parameters.AddWithValue("@nome", nome);
                atualizar.Parameters.AddWithValue("@email", email);
                atualizar.Parameters.AddWithValue("@estado", estado);
                atualizar.Parameters.AddWithValue("@cidade", cidade);
                atualizar.Parameters.AddWithValue("@bairro", bairro);
                atualizar.Parameters.AddWithValue("@endereco", endereco);
                atualizar.Parameters.AddWithValue("@numCasa", num_Casa);
                atualizar.Parameters.AddWithValue("@complemento", complemento);
                atualizar.Parameters.AddWithValue("@cpf", cpf);

                atualizar.ExecuteNonQuery();



            }
            catch (Exception m)
            {

                return m.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();


            return "Editado com Sucesso";

        }

        public ModeloUsuario PesquisaUsuario(string cpf)
        {
            ModeloUsuario m = new ModeloUsuario();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Usuarios WHERE cpf = @cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    m.Cpf = leitor["cpf"].ToString();
                    m.Atuacao = (int)leitor["Atuacao"];
                    m.Plano = (int)leitor["Plano"];
                    m.Nome = leitor["Nome"].ToString();
                    m.Email = leitor["Email"].ToString();
                    m.Estado = leitor["Estado"].ToString();
                    m.Cidade = leitor["Cidade"].ToString();
                    m.Bairro = leitor["Bairro"].ToString();
                    m.Senha = leitor["Senha"].ToString();
                    m.Complemento = leitor["Complemento"].ToString();
                    m.Num_Casa = leitor["NumCasa"].ToString();


                }
            }
            catch (Exception e)
            {
                m = null;
                string resposta = e.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return m;
        }

        public static ModeloUsuario BuscaUsuario(string cpf)
        {
            ModeloUsuario m = new ModeloUsuario();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Usuarios WHERE cpf = @cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    m.Cpf = leitor["cpf"].ToString();
                    m.Atuacao = (int)leitor["Atuacao"];
                    m.Plano = (int)leitor["Plano"];
                    m.Nome = leitor["Nome"].ToString();
                    m.Email = leitor["Email"].ToString();
                    m.Estado = leitor["Estado"].ToString();
                    m.Cidade = leitor["Cidade"].ToString();
                    m.Bairro = leitor["Bairro"].ToString();
                    m.Senha = leitor["Senha"].ToString();
                    m.Complemento = leitor["Complemento"].ToString();
                    m.Num_Casa = leitor["NumCasa"].ToString();
                    m.endereco = leitor["Endereco"].ToString();


                }
            }
            catch (Exception e)
            {
                m = null;
                string resposta = e.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return m;
        }
        
        internal string EditarPlano()
        {
            
            //Teste para verificar se aceita
            try
            {

                //Abrir conexão
                con.Open();

                //Comando para atualizar no banco de dados o plano do usuario

                SqlCommand atualizarplano = new SqlCommand("UPDATE Usuarios SET Plano = @plano WHERE cpf = @cpf ", con);

                atualizarplano.Parameters.AddWithValue("@cpf", cpf);
                atualizarplano.Parameters.AddWithValue("@plano", plano);

                atualizarplano.ExecuteNonQuery();



            }
            catch (Exception e)
            {

                return e.Message;

            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            return "Plano editado com suceso!";

        }

        public ModeloUsuario PesquisaPlanos(string cpf)
        {
            ModeloUsuario m = new ModeloUsuario();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand query =
                    new SqlCommand("SELECT * FROM Usuarios WHERE cpf = @cpf", con);
                query.Parameters.AddWithValue("@cpf", cpf);
                SqlDataReader leitor = query.ExecuteReader();

                while (leitor.Read())
                {
                    m.Cpf = leitor["cpf"].ToString();

                    m.Plano = (int)leitor["Plano"];
                }
            }
            catch (Exception e)
            {
                m = null;
                string resposta = e.Message;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return m;
        }

        public string validaCpf()
        {
            string strCpf = "";

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = "", digito = "";
            strCpf = Cpf;
            strCpf = strCpf.Trim();
            strCpf = strCpf.Replace(".", "").Replace(",", "").Replace("-", "");
            if (strCpf.Length != 11)
            {
                return "CPF Inválido pois não tem o tamanho correto";
            }
            else
            {
                tempCpf = strCpf.Substring(0, 9);
                int soma = 0, resto = 0;
                for (int i = 0; i < 9; i++)
                {
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                }
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();
                //MessageBox.Show(digito + "\n" + strCpf.Substring(10));
                if (digito == strCpf.Substring(9))
                    return "CPF Válido";
                else
                {
                    return "CPF Inválido pois os digitos verificadores não batem";
                }
            }

        }

        public bool validaEmail()
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (rg.IsMatch(Email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}