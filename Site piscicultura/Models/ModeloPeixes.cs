using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Site_piscicultura.Models
{
    public class ModeloPeixes
    {
        private int cod, temperatura;
        private string especie, racaoIdeal;


        //---Criar uma variavel para conexão com o Banco Dados------//

        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);

        // Getters e Setters
        [DisplayName("Código")]
        public int Cod
        {
            get { return cod; }
            set { cod = value; }
        }

        [DisplayName("Temperatura")]
        public int Temperatura
        {
            get { return temperatura; }
            set { temperatura = value; }
        }

        [DisplayName("Espécie")]
        public string Especie
        {
            get { return especie; }
            set { especie = value; }
        }

        [DisplayName("Ração Ideal")]
        public string RacaoIdeal
        {
            get { return racaoIdeal; }
            set { racaoIdeal = value; }
        }

        ////---MÉTODO PARA CADASTRAR PEIXES NO SISTEMA

        //internal string InserirPeixe()
        //{
            
        //    try{
        //        //Abrir conexão
        //        con.Open();

        //        //Comando para fazer a inserção das informações no BD
        //        SqlCommand inserir = new SqlCommand("INSERT INTO Peixes VALUES(@cod,@especie,@temperatura,@racaoIdeal)", con);

        //        //Atribuir os valores das variavéis do sistema para as colunas no BD
        //        inserir.Parameters.AddWithValue("@cod", cod);
        //        inserir.Parameters.AddWithValue("@especie", especie);
        //        inserir.Parameters.AddWithValue("@temperatura", temperatura);
        //        inserir.Parameters.AddWithValue("@racaoIdeal", racaoIdeal);

        //        //Executar o comando no BD
        //        inserir.ExecuteNonQuery();

        //    }catch(Exception e)
        //        {
        //        //Caso ocorra um erro retorna o tipo de erro para o ususario
        //        return e.Message;

        //        }


        //    //Verificar se a conexao está aberta
        //    if (con.State == System.Data.ConnectionState.Open)
            
        //        con.Close();

        //    //Retorna mensagem se cadastrou no banco

        //    return "Peixe cadastrado com Sucesso!";



        //}

        ////----MÉTODO PARA EDITAR PEIXES
        //internal string EditarPeixe()
        //{

        //    try
        //    {
        //        //Abrir conexão
        //        con.Open();

        //        //Comando para fazer atualização de informaçao no BD
        //        SqlCommand atualizar = new SqlCommand("UPDATE Peixes SET especie = @especie, temperatura = @temperatura, racaoIdeal = @racaoIdeal where cod = @cod", con);

        //        atualizar.Parameters.AddWithValue("@cod", cod);
        //        atualizar.Parameters.AddWithValue("@especie", especie);
        //        atualizar.Parameters.AddWithValue("@temperatura", temperatura);
        //        atualizar.Parameters.AddWithValue("@racaoIdeal", racaoIdeal);

        //        atualizar.ExecuteNonQuery();



        //    }
        //    catch (Exception m)
        //    {

        //        return m.Message;
        //    }

        //    if (con.State == ConnectionState.Open)
        //        con.Close();


        //    return "Editado com Sucesso";

        //}

        ////--CRIAR A LISTA DE PEIXES CADASTRADOS---//

        //public static List<ModeloPeixes>ListarPeixes()
        //{

        //    //Criar um array para guardar as informações que virão do BD
        //    List<ModeloPeixes> lista = new List<ModeloPeixes>();

        //    try
        //    {
        //        con.Open();

        //        //Comando para selecionar os itens da tabela peixes
        //        SqlCommand listar =  new SqlCommand("SELECT * FROM Peixes", con);
        //        SqlDataReader leitor = listar.ExecuteReader();

        //        while (leitor.Read())
        //        {
        //            ModeloPeixes p = new ModeloPeixes();
        //            p.Cod = (int)leitor["cod"];
        //            p.Temperatura = (int)leitor["temperatura"];
        //            p.Especie = leitor["especie"].ToString();
        //            p.RacaoIdeal = leitor["racaoIdeal"].ToString();
                    
        //            lista.Add(p);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        lista = null;
        //    }

        //    if (con.State == ConnectionState.Open)
        //        con.Close();

        //    return lista;
        //}

        ////---MÉTODO PARA BUSCAR UM PEIXE ESPECIFICO NA TABELA---//

        //public static ModeloPeixes BuscaPeixes(string cod)
        //{
        //    ModeloPeixes l = new ModeloPeixes();

        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
        //    try
        //    {
        //        con.Open();
        //        SqlCommand query =
        //            new SqlCommand("SELECT * FROM Peixes WHERE cod = @cod", con);
        //        query.Parameters.AddWithValue("@cod", cod);
        //        SqlDataReader leitor = query.ExecuteReader();

        //        while (leitor.Read())
        //        {
        //            l.cod = (int)leitor["cod"];
        //            l.especie = leitor["especie"].ToString();
        //            l.temperatura =(int) leitor["temperatura"];
        //            l.racaoIdeal = leitor["racaoIdeal"].ToString();
                    
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        l = null;
        //    }

        //    if (con.State == ConnectionState.Open)
        //        con.Close();

        //    return l;
        //}

        ////---MÉTODO para EXCLUIR PEIXES---//

        //internal string Remover()
        //{
        //    string res = "";
        //    try
        //    {
        //        con.Open();
        //        //Deletar a chave primária da tabela
        //        SqlCommand query =
        //            new SqlCommand("DELETE FROM Peixes WHERE cod = @cod", con);
        //        query.Parameters.AddWithValue("@cod", cod);

        //        query.ExecuteNonQuery();
        //        res = "Removido com sucesso!";
        //    }
        //    catch (Exception e)
        //    {
        //        res = e.Message.ToString();
        //    }

        //    if (con.State == ConnectionState.Open)
        //        con.Close();
        //    return res;//

        //}


    }
}