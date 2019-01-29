using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Site_piscicultura.Models
{
    public class ModeloTarefas
    {

        static SqlConnection con =
         new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);


        public string title, start , end;


        //CONSTRUTOR DA CLASSE  
        public ModeloTarefas(String title, String start, String end)
        {
            this.title = title;
            this.start = start;
            this.end = end;
        }

        //CONSTRUTOR VAZIO DA CLASSE  
        public ModeloTarefas()
        {

        }

        //MÉTODO PARA CRIAR UMA TAREFA ATRAVÉS DOS TANQUES
        internal string CriarTarefa(string tanqueNome)
        {
            //TESTAR A CONEXÃO PARA VER SE ESTÁ CERTO
            try
            {

                //Data to manipulate the Events
                string[] listaTarefas = new string[] {"Análise de Água", "Cálculo de Arrasoamento", "Biometria", "Despesca" };
                List<int> listaDias = new List<int>(new int[] {7, 14, 21, 60});

                //Open Connection
                con.Open();

                for(int i = 0; i < 4; i++)
                {

                    //Load data to manipulate the Events
                    string tarefa = listaTarefas[i].ToString();
                    double diasAdicondados = (double) listaDias[i];

                    //Manipulating data to create the Events
                    title = "Tanque " + tanqueNome.ToString() + ": " + tarefa;
                    start = DateTime.Now.Date.AddDays(diasAdicondados).ToString();
                    end = start;

                
                    SqlCommand query = new SqlCommand("INSERT INTO Tarefas values (@title, @start, @end)", con);
                    query.Parameters.AddWithValue("@title", title);
                    query.Parameters.AddWithValue("@start", start);
                    query.Parameters.AddWithValue("@end", end);

                    query.ExecuteNonQuery();
                }
                
            }


            catch (Exception e)
            {
                con.Close();
                return e.Message;
            }
            //VERIFICA SE A CONEXÃO ESTÁ ABERTO
            if (con.State == ConnectionState.Open)
                //SE TIVER ABERTA, FECHA CONEXÃO
                con.Close();
            //RETORNA UMA MENSAGEM SE INSERIU OU NÃO
            return "Tarefa registrada com sucesso!";

        }


        // MÉTODO PARA REMOVER UMA TAREFA
        internal string RemoverTarefa()
        {
            string res = "";
            try
            {
                con.Open();
                //Deletar a chave primária da tabela
                SqlCommand query =
                    new SqlCommand("DELETE FROM Tarefas WHERE id = @id", con);
                //query.Parameters.AddWithValue("@id", id);

                query.ExecuteNonQuery();
                res = "Removido com sucesso!";
            }
            catch (Exception e)
            {
                res = e.Message.ToString();
            }

            if (con.State == ConnectionState.Open)
                con.Close();
            return res;


        }

        //MÉTODO PARA EDITAR UMA TAREFA
        internal string EditarTarefa()
        {

            try
            {
                //Abrir conexão
                con.Open();

                //Comando para fazer atualização de informaçao no BD
                SqlCommand atualizar = new SqlCommand("UPDATE Tarefas SET nome = @nome, data_inicio = @data_inicio," +
                    "data_fim = @data_fim where ID = @id", con);

                //atualizar.Parameters.AddWithValue("@id", id);
                //atualizar.Parameters.AddWithValue("@nome", nome);
                //atualizar.Parameters.AddWithValue("@data_incio", data_inicio);
                //atualizar.Parameters.AddWithValue("@data_fim", data_fim);

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


        //--CRIAR A LISTA DE TAREFAS CADASTRADOS---//

        public static String ListarTarefas()
        {

            //Criar um array para guardar as informações que virão do BD
            List<ModeloTarefas> lista = new List<ModeloTarefas>();

            string tarefasJsonString = "";

            try
            {
                con.Open();

                //Comando para selecionar os itens da tabela peixes
                SqlCommand listar = new SqlCommand("SELECT * FROM Tarefas", con);
                SqlDataReader leitor = listar.ExecuteReader();

                //Realiza a captura dos dados caso haja algo na tabela
                if (leitor.HasRows)
                {
                    //var indexOfId = leitor.GetOrdinal("ID");
                    var indexOfNome = leitor.GetOrdinal("Title");
                    var indexOfDataInicio = leitor.GetOrdinal("start");
                    var indexOfDataFim = leitor.GetOrdinal("end");


                    while (leitor.Read())
                    {

                        //var id = (int)leitor.GetValue(indexOfId);
                        var title = leitor.GetValue(indexOfNome).ToString();
                        var start = leitor.GetValue(indexOfDataInicio);
                        var end = leitor.GetValue(indexOfDataFim);

                        //Convert Implicity typed var to Date Time
                        DateTime RealData_Inicio = Convert.ToDateTime(start);
                        DateTime RealData_Fim = Convert.ToDateTime(end);

                        //Convert Date Time to ISO
                        String ReadyData_Inicio = RealData_Inicio.ToString("s");
                        String ReadyData_Fim = RealData_Fim.ToString("s");

                        ModeloTarefas tarefa = new ModeloTarefas(title, ReadyData_Inicio, ReadyData_Fim);

                        lista.Add(tarefa);
                    }

                    tarefasJsonString = (new JavaScriptSerializer()).Serialize(lista);

                }
            }
            catch (Exception e)
            {
                lista = null;
            }

            if (con.State == ConnectionState.Open)
                con.Close();

            return tarefasJsonString;
        }

        //---MÉTODO PARA BUSCAR UM PEIXE ESPECIFICO NA TABELA---//

        //public static ModeloTarefas BuscaTarefa(string cod)
        //{
        //    ModeloTarefas t = new ModeloTarefas();

        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BCD"].ConnectionString);
        //    try
        //    {
        //        con.Open();
        //        SqlCommand query =
        //            new SqlCommand("SELECT * FROM Tarefas WHERE id = @id", con);
        //        query.Parameters.AddWithValue("@id", cod);
        //        SqlDataReader leitor = query.ExecuteReader();

        //        while (leitor.Read())
        //        {
        //            t.id = (int)leitor["id"];
        //            t.nome = leitor["nome"].ToString();
        //            t.data_inicio = leitor["data_inicio"].ToString();
        //            t.data_fim = leitor["data_fim"].ToString();

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        t = null;
        //    }

        //    if (con.State == ConnectionState.Open)
        //        con.Close();

        //    return t;
        //}



    }
}